using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fujita.Communication;
using Fujita.Misc;

namespace Adjustment
{
    enum ESpeedType
    {
        MeterPerSec,
        MeterPerMinute,
        MiliPerSec,
        MiliPerMinute,
    }

    class SpeedMonitorEventArgs : EventArgs
    {
        double _dHz;
        double _dSpeedMPS;
        static double[] _dMagnify = new double[] { 1 / 1000.0, 60 / 1000.0, 1 / 1.0, 60.0 };

        public SpeedMonitorEventArgs(double dHz, double dSpeedMPS)
        {
            _dHz = dHz;
            _dSpeedMPS = dSpeedMPS;
        }

        public double GetSpeed(ESpeedType eType)
        {
            return _dSpeedMPS * SpeedMonitorEventArgs._dMagnify[(int)eType];
        }

        public double GetHz()
        {
            return _dHz;
        }
    }

    delegate void SpeedMonitorEventHandler( object sender, SpeedMonitorEventArgs e );

    class clsSpeedMonitor : IThreadSafeFinish, IError
    {
        public clsSpeedMonitor()
        {
        }

        double _dResolution = 0.025;
        public double Resolution
        {
            get { return _dResolution; }
            set
            {
                if (value <= 0.0)
                    return;
                _dResolution = value;
            }
        }

        public bool IsError { get; set; }
        public string ErrorReason { get; set; }
        void setError(bool bError, string sReason = "")
        {
            IsError = bError;
            ErrorReason = sReason;
        }

        const string SEND_MARK = "%";
        const string RECEIVE_MARK = "@";
        const string DELIMITER = "\r";

        public event SpeedMonitorEventHandler SpeedMonitor;

        CommunicationSIO _sio;
        CommunicationDIO _dio;
        int _iResetDioIndex = -1;
        public bool Initialize(CommunicationSIO sio, CommunicationDIO dio = null, int iReset = -1)
        {
            if (sio == null)
                return false;

            if (_sio != null)
                return false;

            _sio = sio;
            _sio.ReadTimeout = 1000;
            _sio.SetCommandDelimiter(DELIMITER);

            _dio = dio;
            _iResetDioIndex = iReset;

            return true;
        }

        // システム
        public bool Load(string sPath, string sSection)
        {
            if (_sio == null)
                return false;

            if (isStart())
                return false;

            IniFileAccess ifa = new IniFileAccess();
            _iResetDioIndex = ifa.GetIni("SpeedMonitor_DioAssign", "Reset", -1, sPath);

            return true;

        }

        public bool Terminate()
        {
            if (_tThread != null && _threadSafe == null )
            {
                Stop();
            }

            // 完全にスレッドが停止するのを待つ
            while (_threadSafe != null) ;

            _sio = null;
            _dio = null;
            _iResetDioIndex = -1;

            return true;
        }

        bool isStart()
        {
            return (_tThread != null || _threadSafe != null);
        }

        bool _bStop = false;
        System.Threading.Thread _tThread = null;

        public bool Start()
        {
            if (_sio == null)
                return false;

            // すでにスタート済み
            if (isStart())
                return false;

            _bStop = false;
            _tThread = new System.Threading.Thread(speedMonitor);
            _tThread.Name = "ｽﾋﾟｰﾄﾞﾓﾆﾀ";
            _tThread.Start();
            return true;
        }

        clsThreadSafeFinish _threadSafe = null;
        public void ThreadSafeEnd()
        {
            _bStop = true;
            do
            {
                _tThread.Join(100);
            } while (_tThread.IsAlive);

            _tThread = null;
        }

        public bool Stop()
        {
            if (_sio == null)
                return false;

            if (_tThread == null)
                return false;

            // 終了中の再読み込み禁止
            if (_threadSafe != null)
                return true;

            _threadSafe = new clsThreadSafeFinish(this);
            _threadSafe.OnThreadEnded += new ThreadEndedEventHandler(_threadSafe_OnThreadEnded);
            _threadSafe.SafeFinish(true);

            return true;
        }

        void _threadSafe_OnThreadEnded(object sender, ThreadEndedEventArgs e)
        {
            _threadSafe.OnThreadEnded -= _threadSafe_OnThreadEnded;
            _threadSafe = null;
        }

        System.Diagnostics.Stopwatch _sw = new System.Diagnostics.Stopwatch();
        void resetController()
        {
            // データがあるのにﾌｫｰﾏｯﾄがおかしい場合スピード計測マイコンをﾘｾｯﾄする
            if (_dio != null && _iResetDioIndex != -1)
            {
                _dio.OUT1(_iResetDioIndex, true);
                System.Threading.Thread.Sleep(200);
                _dio.OUT1(_iResetDioIndex, false);
                // 初期化待ち
                System.Threading.Thread.Sleep(500);
            }
        }

        void speedMonitor()
        {
            string sReceive = "";
            while (!_bStop)
            {
                sReceive = "";
                _sw.Restart();
                if (!_sio.WriteString(SEND_MARK + "A" + DELIMITER))
                {
                    setError(true, "ｺﾏﾝﾄﾞ送信でｴﾗｰが発生しました。");
                }

                System.Threading.Thread.Sleep(700);
                if (!_sio.ReadString(ref sReceive))
                {
                    setError(true, "結果応答でｴﾗｰが発生しました。");
                }

                if (sReceive != "")
                {
                    string sData = "";
                    if (analyzeReceiveCommand("A", sReceive, ref sData))
                    {
                        int iHz;
                        if (int.TryParse(sData, out iHz))
                        {
                            if (SpeedMonitor != null && !_bStop)
                            {
                                //                                System.Diagnostics.Debug.WriteLine( iHz.ToString() );
                                SpeedMonitor(this, new SpeedMonitorEventArgs(iHz, iHz * _dResolution * 4));
                            }
                            setError(false);
                        }
                        else
                        {
                            // データがおかしいのでマイコンをリセットする
                            resetController();
                        }
                    }
                    else
                    {
                        // データがおかしいのでマイコンをリセットする
                        resetController();
                    }
                }
                else
                {
                    // データがおかしいのでマイコンをリセットする
                    resetController();
                }

                // 時刻合わせ
                int iElapsed = (int)_sw.ElapsedMilliseconds;
                int iSleepTime = 3000 - iElapsed;
                if (iSleepTime < 1)
                    iSleepTime = 1;
                System.Threading.Thread.Sleep(iSleepTime);
            }
        }

        bool analyzeReceiveCommand(string sSendCmd, string sRecive, ref string sData )
        {
            string sCmdRet = RECEIVE_MARK + sSendCmd;
            int iCmdLen = sCmdRet.Length;
            int iHeaderPos = sRecive.LastIndexOf(sCmdRet);
            int iFooterPos = sRecive.LastIndexOf(DELIMITER);

            if (iHeaderPos == -1)
                return false;

            if (iFooterPos < iHeaderPos)
                return false;

            if (iFooterPos == -1)
                sData = sRecive.Substring(iHeaderPos + iCmdLen);
            else
                sData = sRecive.Substring(iHeaderPos + iCmdLen, iFooterPos - (iHeaderPos+iCmdLen));

            return true;
        }

    }
}
