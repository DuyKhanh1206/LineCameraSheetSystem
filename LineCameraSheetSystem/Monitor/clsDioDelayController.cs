using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fujita.Communication;

namespace LineCameraSheetSystem
{
    class clsDioDelayController
    {
        static long _lHandle = -1;
        static object _objLock = new object();
        static long getHandle()
        {
            lock (_objLock)
            {
                _lHandle++;
                return _lHandle;
            }
        }

        public enum ECmdType
        {
            Unknown,
            Active,
            Negative,
            AfterNegative,
        }

        public class Command
        {
            public Command(long handle, int idms, ECmdType cmdtype, int dioindex, bool aan, int aanms)
            {
                Handle = handle;
                FireTime = new DateTime( DateTime.Now.Ticks + (long)idms * 10000 );
                CmdType = cmdtype;
                DioIndex = dioindex;
                ActiveAfterNagative = aan;
                ActiveAfterNegativeMS = aanms;
            }

            public void SetCommand(ECmdType eCmd)
            {
                CmdType = eCmd;
            }

            public long Handle { get; private set; }
            public DateTime FireTime{get;private set;}
            public ECmdType CmdType { get; private set; }
            public int DioIndex { get; private set; }
            public bool ActiveAfterNagative { get; private set; }
            public int ActiveAfterNegativeMS { get; private set; }
        }

        public static long GetCommandObject(int iDelayMS, ECmdType eCmdType, int iDioIndex, out Command cmd)
        {
            return GetCommandObject(iDelayMS, eCmdType, iDioIndex, false, 0, out cmd);
        }

        public static long GetCommandObject(int iDelayMS, ECmdType eCmdType, int iDioIndex, bool bAAN, int iAfterMS, out Command cmd)
        {
            cmd = null;
            if (iDioIndex < 1)
                return -1L;

            long lHandle = getHandle();
            cmd = new Command(lHandle, iDelayMS, eCmdType, iDioIndex, bAAN, iAfterMS);
            return lHandle;
        }

        public bool AddCommand(Command cmd)
        {
            // 外部からスレッドが停止中の場合は外部からのコマンドを追加できない
            if (_bStop)
                return false;

            return addCommand(cmd);
        }

        private bool addCommand(Command cmd)
        {
            bool bInsert = false;
            // ハンドルが無効の場合
            if (cmd == null || cmd.Handle == -1L)
                return false;

            lock (_lstCommand)
            {
                for (int i = 0; i < _lstCommand.Count; i++)
                {
                    if (_lstCommand[i].FireTime > cmd.FireTime)
                    {
                        _lstCommand.Insert(i, cmd);
                        bInsert = true;
                        break;
                    }
                }
                if (!bInsert)
                    _lstCommand.Add(cmd);
            }
            return true;
        }

        public bool DeleteExtOut1ActiveCommand(int beforeMS)
        {
            lock (_lstCommand)
            {
                bool targetFlag = false;
                DateTime beforeTime = new DateTime();
                for (int i = _lstCommand.Count - 1; i >= 0; i--)
                {
                    if (_lstCommand[i].CmdType == ECmdType.Active && _lstCommand[i].DioIndex == SystemParam.GetInstance().OutPointExternal1)
                    {
                        if (targetFlag == false)
                        {
                            beforeTime = _lstCommand[i].FireTime.AddMilliseconds(-beforeMS);
                            _lstCommand.RemoveAt(i);
                            targetFlag = true;
                        }
                        else if (beforeTime < _lstCommand[i].FireTime)
                        {
                            _lstCommand.RemoveAt(i);
                        }
                    }
                }
            }
            return true;
        }
        public bool UnknownDefault
        {
            get;
            set;
        }

        CommunicationDIO _dio = null;

        List<Command> _lstCommand;
        int [] _iaActiveCnt;

        public bool Initialize(CommunicationDIO dio)
        {
            if (_dio != null)
                return false;

            if (dio == null)
                return false;

            _dio = dio;

            _iaActiveCnt = new int[_dio.OutNum];
            _lstCommand = new List<Command>();

            return true;
        }

        public void Terminate()
        {
            Stop();

            _iaActiveCnt = null;
            _lstCommand.Clear();
            _dio = null;
        }

        public bool OverwriteCommand(long handle, ECmdType cmdtype)
        {
            lock (_lstCommand)
            {
                for (int i = 0; i < _lstCommand.Count; i++)
                {
                    if (_lstCommand[i].Handle == handle)
                    {
                        _lstCommand[i].SetCommand(cmdtype);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool Start()
        {
            if (_dio == null)
                return false;

            if (_tThread != null)
                return false;

            _lstCommand.Clear();

            _bStop = false;
            _tThread = new System.Threading.Thread(commandMonitor);
            _tThread.Name = "DioDelayｶﾝｼ";
            _tThread.Start();

            return true;
        }

        public bool Stop()
        {
            if (_dio == null)
                return false;

            if (_tThread == null)
                return true;

            _bStop = true;

            do
            {
                _tThread.Join(100);
            }
            while (_tThread.IsAlive);

            _tThread = null;

            return true;
        }

        System.Threading.Thread _tThread = null;

        bool _bStop = false;
        private void commandMonitor()
        {
            while (!_bStop)
            {
                lock (_lstCommand)
                {
                    System.Diagnostics.Debug.WriteLine("■残りコマンド数■" + _lstCommand.Count);

                    if (_lstCommand.Count > 0)
                    {
                        if (_lstCommand[0].FireTime <= DateTime.Now)
                        {
                            //System.Diagnostics.Debug.WriteLine("▲" + DateTime.Now);
                            // 指定時間がたった時に、
                            if (_lstCommand[0].CmdType == ECmdType.Active || (_lstCommand[0].CmdType == ECmdType.Unknown && UnknownDefault))
                            {
                                _dio.OUT1(_lstCommand[0].DioIndex, true);
                                //V1322 外部出力 出力時間0の際のカウントアップバグ修正
                                //_iaActiveCnt[_lstCommand[0].DioIndex - 1]++;

                                if (_lstCommand[0].ActiveAfterNagative)
                                {
                                    _iaActiveCnt[_lstCommand[0].DioIndex - 1]++;//V1322 外部出力 出力時間0の際のカウントアップバグ修正
                                    Command cmd;
                                    GetCommandObject(_lstCommand[0].ActiveAfterNegativeMS, ECmdType.AfterNegative, _lstCommand[0].DioIndex, out cmd);
                                    addCommand(cmd);
                                }
                            }
                            else if (_lstCommand[0].CmdType == ECmdType.Negative
                                || (_lstCommand[0].CmdType == ECmdType.Unknown && !UnknownDefault))
                            {
                                if (_iaActiveCnt[_lstCommand[0].DioIndex - 1] == 0)
                                {
                                    _dio.OUT1(_lstCommand[0].DioIndex, false);
                                }
                            }
                            else if (_lstCommand[0].CmdType == ECmdType.AfterNegative)
                            {
                                _iaActiveCnt[_lstCommand[0].DioIndex - 1]--;
                                if (_iaActiveCnt[_lstCommand[0].DioIndex - 1] == 0)
                                {
                                    _dio.OUT1(_lstCommand[0].DioIndex, false);
                                }
                            }
                            _lstCommand.RemoveAt(0);
                        }
                    }
                }
                System.Threading.Thread.Sleep(1);
            }


            //v1326 GCustomEnableの場合は、全CHを落として終了
            if (false == SystemParam.GetInstance().GCustomEnable)
            {
                // 現在追加されているコマンド分をすべて実行して終了する
                while (true)
                {
                    lock (_lstCommand)
                    {
                        if (_lstCommand.Count == 0)
                            break;

                        if (_lstCommand[0].FireTime <= DateTime.Now)
                        {
                            // 指定時間がたった時に、
                            if (_lstCommand[0].CmdType == ECmdType.Active  || (_lstCommand[0].CmdType == ECmdType.Unknown && UnknownDefault))
                            {
                                _dio.OUT1(_lstCommand[0].DioIndex, true);
                                //V1322 外部出力 出力時間0の際のカウントアップバグ修正
                                //_iaActiveCnt[_lstCommand[0].DioIndex - 1]++;

                                if (_lstCommand[0].ActiveAfterNagative)
                                {
                                    _iaActiveCnt[_lstCommand[0].DioIndex - 1]++;//V1322 外部出力 出力時間0の際のカウントアップバグ修正
                                    Command cmd;
                                    GetCommandObject(_lstCommand[0].ActiveAfterNegativeMS, ECmdType.AfterNegative, _lstCommand[0].DioIndex, out cmd);
                                    addCommand(cmd);
                                }
                            }
                            else if (_lstCommand[0].CmdType == ECmdType.Negative
                                || (_lstCommand[0].CmdType == ECmdType.Unknown && !UnknownDefault))
                            {
                                if (_iaActiveCnt[_lstCommand[0].DioIndex - 1] == 0)
                                {
                                    _dio.OUT1(_lstCommand[0].DioIndex, false);
                                }
                            }
                            else if (_lstCommand[0].CmdType == ECmdType.AfterNegative)
                            {
                                _iaActiveCnt[_lstCommand[0].DioIndex - 1]--;
                                if (_iaActiveCnt[_lstCommand[0].DioIndex - 1] == 0)
                                {
                                    _dio.OUT1(_lstCommand[0].DioIndex, false);
                                }
                            }
                            _lstCommand.RemoveAt(0);
                        }
                    }
                }
            }
            else
            {
                foreach (var cmdCH in _lstCommand)
                {
                    _dio.OUT1(cmdCH.DioIndex, false);
                }
            }
        }

        //v1326 岐阜カスタム用
        public int GetPatLiteTimerNum()
        {
            List <Command> cmds = _lstCommand.FindAll(x => x.DioIndex == SystemParam.GetInstance().OutPointGCustomBuzzer);
            return cmds.Count;
        }
    }
}
