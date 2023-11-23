//#define SINGLETON_INSTANCE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fujita.Communication;
using LineCameraSheetSystem;

namespace Fujita.Misc
{
    public delegate void OnDioCommandEventHandler( object sender,clsDioCommandMonitor.DioCommandEventArgs e );

    /// <summary>
    /// DIOのINをチェックし、INの変化に応じてイベンtのを返す
    /// </summary>
    public class clsDioCommandMonitor: IThreadSafeFinish
    {
        public class DioCommandEventArgs : EventArgs
        {
            public int DioIndex { get; private set; }
            public EPublishEventType PublishEventType { get; private set; }
            public object Args { get; private set; }

            public DioCommandEventArgs(int iDioIndex,EPublishEventType eType, object args)
            {
                DioIndex = iDioIndex;
                PublishEventType = eType;
                Args = args;
            }
        }

        public enum EPublishEventMode
        {
            Alternate,
            While,
            Inspect, //V1057 手動外部修正 yuasa 20190115：検査開始終了用に追加
            PowerOffButton, // v1338 yuasa
        }

        public enum EPublishEventType
        {
            Active = 0x1,
            Negative = 0x2,
        }

#if SINGLETON_INSTANCE
        static clsDioCommandMonitor _instance = null;
        static public clsDioCommandMonitor GetInstance()
        {
            if( _instance == null )
                _instance = new clsDioCommandMonitor();
            return _instance;
        }
        private clsDioCommandMonitor()
        {
        }
#else
        public clsDioCommandMonitor()
        {
            _iCycleTime = (true == SystemParam.GetInstance().GCustomEnable) ? SystemParam.GetInstance().GCustomDIOCycleTime : 100;
        }
#endif


        CommunicationDIO _dio = null;

        public bool Initialize( CommunicationDIO dio)
        {
            if (_dio != null)
                return false;

            if (dio == null)
                return false;

            _dio = dio;
            _lstCommands = new List<clsCommand>();

            return true;
        }

        public bool Terminate()
        {
            if (_dio == null)
                return true;

            if (_tThread != null)
            {
                Stop();
                while (IsStoped()) ;
            }

            _dio = null;

            for (int i = 0; i < _lstCommands.Count; i++)
            {
                _lstCommands[i].handler = null;
            }
            _lstCommands.Clear();

            return true;
        }
        
        int _iCycleTime;
        public int CycleTime
        {
            get{return _iCycleTime;}
            set
            {
                if( value < 10 )
                    return;
                _iCycleTime = value;
            }
        }

        System.Threading.Thread _tThread = null;
        bool _bStop = false;
        public bool Start()
        {
            if (_dio == null)
                return false;

            if (_tThread != null)
                return false;

            _bStop = false;
            _tThread = new System.Threading.Thread(monitor);
            _tThread.Name = "DioMonitor";
            _tThread.Start();

            return true;
        }

        clsThreadSafeFinish _safeFin;
        public bool Stop()
        {
            if (_tThread == null)
                return false;

            _safeFin = new clsThreadSafeFinish(this);
            _safeFin.OnThreadEnded += new ThreadEndedEventHandler(safeFin_OnThreadEnded);
            _safeFin.SafeFinish(true);

            return true;
        }

        public bool IsStoped()
        {
            if (_tThread == null && _safeFin == null)
                return true;
            return false;
        }

        void safeFin_OnThreadEnded(object sender, ThreadEndedEventArgs e)
        {
            _tThread = null;
            _safeFin.OnThreadEnded -= safeFin_OnThreadEnded;
            _safeFin = null;
        }

        public void ThreadSafeEnd()
        {
            _bStop = true;
            do
            {
                _tThread.Join(100);
            } while (_tThread.IsAlive);
        }

        public bool extReciveEnableBtnValue = false; //V1057 手動外部修正 yuasa 20190115：手動外部ボタン押下したらTrue
        public bool extRecivePop = false; //V1057 手動外部修正 yuasa 20190115：検査のポップアップ表示中にTrue

        void monitor()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            DateTime PowerOffButtonTimer = DateTime.Now; //v1338 yuasa
            bool bAlreadyPowerOff = false;//v1338 yuasa ：PC電源ボタン押下の反応を１回だけにするためのフラグ

            while (!_bStop)
            {
                sw.Restart();
                lock (_lstCommands)
                {
                    for (int i = 0; i < _lstCommands.Count; i++)
                    {
                        //v1326 外部入力　検査開始／終了の条件分岐を変更
                        if (_lstCommands[i].iDioIndex == SystemParam.GetInstance().InInspectionStart)
                        {
                            if (!extReciveEnableBtnValue || extRecivePop) //V1057 手動外部修正 yuasa 20190115：Dioイベント追加条件追加
                                continue;
                        }

                            bool bData = false;
                            _dio.IN1(_lstCommands[i].iDioIndex, ref bData);

                            if (_lstCommands[i].bFirstTime)
                            {
                                _lstCommands[i].bFirstTime = false;
                            }
                            else
                            {
                                if (_lstCommands[i].eMode == EPublishEventMode.Alternate)
                                {
                                    if (_lstCommands[i].bPrevCondition != bData)
                                    {
                                        if ((bData == true) && ((_lstCommands[i].eType & EPublishEventType.Active) != 0))//bDataがtrueかつActiveの場合
                                        {
                                            _lstCommands[i].handler(this, new DioCommandEventArgs(_lstCommands[i].iDioIndex, EPublishEventType.Active, _lstCommands[i].oArgs));
                                        }
                                        else if ((bData == false) && (_lstCommands[i].eType & EPublishEventType.Negative) != 0)//bDataがfalseかつNegativeの場合 v1326バグ修正
                                        {
                                            _lstCommands[i].handler(this, new DioCommandEventArgs(_lstCommands[i].iDioIndex, EPublishEventType.Negative, _lstCommands[i].oArgs));
                                        }
                                    }
                                    _lstCommands[i].bPrevCondition = bData; //V1057 手動外部修正 yuasa 20190115：それぞの分岐に記載。
                                }
                                else if (_lstCommands[i].eMode == EPublishEventMode.While)
                                {
                                    if ((bData == true) && ((_lstCommands[i].eType & EPublishEventType.Active) != 0))//bDataがtrueかつActiveの場合
                                    {
                                        _lstCommands[i].handler(this, new DioCommandEventArgs(_lstCommands[i].iDioIndex, EPublishEventType.Active, _lstCommands[i].oArgs));
                                    }
                                    else if ((bData == false) && (_lstCommands[i].eType & EPublishEventType.Negative) != 0)//bDataがfalseかつNegativeの場合 v1326バグ修正
                                    {
                                        _lstCommands[i].handler(this, new DioCommandEventArgs(_lstCommands[i].iDioIndex, EPublishEventType.Negative, _lstCommands[i].oArgs));
                                    }
                                    _lstCommands[i].bPrevCondition = bData; //V1057 手動外部修正 yuasa 20190115：それぞの分岐に記載。
                                }
                                else if (_lstCommands[i].eMode == EPublishEventMode.Inspect) //V1057 手動外部修正 yuasa 20190115：Inspect条件を追記。
                                {
                                    //オフで停止中の場合にNegativeのイベントを追加
                                    if (!bData && (LineCameraSheetSystem.SystemStatus.GetInstance().NowState == LineCameraSheetSystem.SystemStatus.State.Stop)) //NowStateが「suspend」（中断）の場合は、手動で中断状態となるので、外部信号は処理しない
                                    {
                                        _lstCommands[i].handler(this, new DioCommandEventArgs(_lstCommands[i].iDioIndex, EPublishEventType.Negative, _lstCommands[i].oArgs));
                                        _lstCommands[i].bPrevCondition = bData;
                                        System.Threading.Thread.Sleep(1000);
                                    }
                                    //オンで検査中の場合Acriveのイベントを追加
                                    else if (bData && LineCameraSheetSystem.SystemStatus.GetInstance().NowState == LineCameraSheetSystem.SystemStatus.State.Inspection)
                                    {
                                        _lstCommands[i].handler(this, new DioCommandEventArgs(_lstCommands[i].iDioIndex, EPublishEventType.Active, _lstCommands[i].oArgs));
                                        _lstCommands[i].bPrevCondition = bData;
                                        System.Threading.Thread.Sleep(1000);
                                    }
                                }
                            else if (_lstCommands[i].eMode == EPublishEventMode.PowerOffButton) //v1338 yuasa
                            {
                                if (bAlreadyPowerOff == false)
                                {
                                    if ((bData == true) && ((_lstCommands[i].eType & EPublishEventType.Active) != 0))//bDataがtrueかつ設定がActiveの場合
                                    {
                                        //1000ms以上違わなかったら
                                        if ((int)(DateTime.Now- PowerOffButtonTimer).TotalMilliseconds >= SystemParam.GetInstance().PowerOffButtonOnTime)
                                        {
                                            bAlreadyPowerOff = true;
                                            this.PowerOffButtonFlag = true;//フラグを立てる
                                            //電源オフ（画面メソッド）を実行
                                            _lstCommands[i].handler(this, new DioCommandEventArgs(_lstCommands[i].iDioIndex, _lstCommands[i].eType, _lstCommands[i].oArgs));
                                        }

                                    }
                                    else if ((bData == false) && (_lstCommands[i].eType & EPublishEventType.Negative) != 0)//bDataがfalseかつ設定がNegativeの場合
                                    {
                                        //1000ms以上違わなかったら
                                        if ((int)(DateTime.Now - PowerOffButtonTimer).TotalMilliseconds >= SystemParam.GetInstance().PowerOffButtonOnTime)
                                        {
                                            bAlreadyPowerOff = true;
                                            this.PowerOffButtonFlag = true;//フラグを立てる
                                            //電源オフ（画面メソッド）を実行
                                            _lstCommands[i].handler(this, new DioCommandEventArgs(_lstCommands[i].iDioIndex, _lstCommands[i].eType, _lstCommands[i].oArgs));
                                        }

                                    }
                                    else //違ったときだけ時間更新
                                    {
                                        PowerOffButtonTimer = DateTime.Now;
                                    }
                                }
                            }
                        }
                            //_lstCommands[i].bPrevCondition = bData; //V1057 手動外部修正 yuasa 20190115：コメントアウト。それぞの分岐に記載。
                        
                    }
                }

                int iSleepTime = _iCycleTime - (int)sw.ElapsedMilliseconds;
                if (iSleepTime < 0)
                {
                    System.Diagnostics.Debug.WriteLine("●");
                    iSleepTime = 0;
                }
                System.Threading.Thread.Sleep(iSleepTime);
            }
        }

        class clsCommand
        {
            public int iDioIndex;
            public EPublishEventMode eMode;
            public EPublishEventType eType;
            public OnDioCommandEventHandler handler;
            public bool bPrevCondition;
            public bool bFirstTime;
            public object oArgs;
        }
        List<clsCommand> _lstCommands = null;

        public bool AddCommand(int iDioIndex, OnDioCommandEventHandler handler, object objArgs = null, EPublishEventMode eMode = EPublishEventMode.Alternate, EPublishEventType eType = EPublishEventType.Active)
        {
            if (_dio == null)
                return false;

            if (_lstCommands == null)
                return false;

            if (handler == null)
                return false;

            clsCommand cmd = new clsCommand();
            cmd.bFirstTime = true;
            cmd.bPrevCondition = false;
            cmd.eMode = eMode;
            cmd.eType = eType;
            cmd.handler = handler;
            cmd.iDioIndex = iDioIndex;
            cmd.oArgs = objArgs;
            lock (_lstCommands)
            {
                _lstCommands.Add(cmd);
            }

            return true;
        }

        /// <summary>PowerOffButtonFlag</summary>
        public bool PowerOffButtonFlag { get; set; } = false;//v1338
    }
}
