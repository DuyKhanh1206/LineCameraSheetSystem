using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fujita.Misc;
using Fujita.Communication;

namespace LineCameraSheetSystem
{
    enum EInSignalControl
    {
        Shot = 0,       //ショット信号
    }

    public class clsDinCountController
    {
        public delegate void ShotEndedEventHandler(object sender, EventArgs e);
//        public event ShotEndedEventHandler OnShotEnded;

        /// <summary>
        /// 
        /// </summary>
        public class Command
        {
            /// <summary>
            /// 
            /// </summary>
            public int ShotCnt { get; private set; }
            /// <summary>
            /// 
            /// </summary>
            public ShotEndedEventHandler Evt;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="shotCnt"></param>
            /// <param name="e"></param>
            public Command(int shotCnt, ShotEndedEventHandler e)
            {
                ShotCnt = shotCnt;
                Evt = e;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool CountDown()
            {
                ShotCnt--;
                if (ShotCnt <= 0)
                    return true;
                return false;
            }
        }

        bool _bInitialize = false;
        CommunicationDIO _dio = null;
        List<Command> _lstCommand;
        int[] _iaDinMap;

        public bool Initialize(CommunicationDIO dio)
        {
            if (dio == null)
                return false;
            _dio = dio;
            _iaDinMap = new int[Enum.GetValues(typeof(EInSignalControl)).Length];
            _lstCommand = new List<Command>();
            _bInitialize = true;

            return true;
        }

        public void Terminate()
        {
            Stop();

            _lstCommand.Clear();
            _dio = null;
        }

        public bool Load(string sPath, string sSection = "")
        {
            if (!_bInitialize)
                return false;

            IniFileAccess ifa = new IniFileAccess();

            foreach (EInSignalControl e in Enum.GetValues(typeof(EInSignalControl)))
            {
                _iaDinMap[(int)e] = ifa.GetIni("SignalControl_DinAssign", e.ToString(), -1, sPath);
            }

            return true;
        }

        public bool AddCommand(Command cmd)
        {
            if (_bStop)
                return false;

            if (cmd == null)
                return false;

            lock (_lstCommand)
            {
                _lstCommand.Add(cmd);
            }
            return true;
        }
        public bool Start()
        {
            if (_dio == null)
                return false;

            if (_tThread != null)
                return false;

            _lstCommand.Clear();

            _bStop = false;
            _tThread = new System.Threading.Thread(monitor);
            _tThread.Name = "Dinｼｮｯﾄ数ｶﾝｼ";
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

        public void Clear()
        {
            lock (_lstCommand)
            {
                _lstCommand.Clear();
            }
        }

        System.Threading.Thread _tThread = null;

        bool _bStop = false;
        private void monitor()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            bool onFlg = false;
            while (!_bStop)
            {
                sw.Restart();

                bool value = false;
                _dio.IN1(_iaDinMap[(int)EInSignalControl.Shot], ref value);
                if (value)
                {
                    onFlg = true;
                }
                else if (onFlg == true)
                {
                    lock (_lstCommand)
                    {
                        for (int i = _lstCommand.Count - 1; 0 <= i; i--)
                        {
                            Command cmd = _lstCommand[i];
                            if (true == cmd.CountDown())
                            {
                                if (cmd != null)
                                    cmd.Evt(this, new EventArgs());
                                _lstCommand.RemoveAt(i);
                            }
                        }
                    }

                    onFlg = false;
                }

                int iSleepTime = 10 - (int)sw.ElapsedMilliseconds;
                if (iSleepTime < 0)
                    iSleepTime = 0;
                System.Threading.Thread.Sleep(iSleepTime);
            }

            // 現在追加されているコマンド分をすべて実行して終了する
            while (true)
            {
                lock (_lstCommand)
                {
                    if (_lstCommand.Count == 0)
                        break;
                    _lstCommand.Clear();
                }
            }
        }
    }
}
