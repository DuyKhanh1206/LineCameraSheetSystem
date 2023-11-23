#define SINGLETON_MODE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using InspectionNameSpace;

namespace LineCameraSheetSystem
{
    public class clsLengthMeasMonitor
    {
#if SINGLETON_MODE
        private clsLengthMeasMonitor()
        {
        }

        static clsLengthMeasMonitor _instance = null;
        static public clsLengthMeasMonitor getInstance()
        {
            if (_instance == null)
                _instance = new clsLengthMeasMonitor();
            return _instance;
        }

#else
        public clsLengthMeasMonitor()
        {
        }
#endif

        public delegate void LengthMeasMonitorEventHandler(object sender, EventArgs e);
        public event LengthMeasMonitorEventHandler LengthMeasError = null;
        AutoInspection _autoInsp = null;
        int _iLengthMeasMonitorSec = 0;

        public int LengthMeasMonitorSec
        {
            get { return _iLengthMeasMonitorSec; }
            set
            {
                if (value < 0)
                    return;
                _iLengthMeasMonitorSec = value;
            }
        }

        bool _bInitialize = false;
        public bool Initialize()
        {
            _bInitialize = true;
            return true;
        }

        public void SetAutoInspection(AutoInspection ai)
        {
            _autoInsp = ai;
        }

        public bool Start()
        {
            if (!_bInitialize)
                return false;

            if (_tThread != null)
                return false;

            if (_autoInsp == null)
                return false;

            _bStop = false;

            _tThread = new Thread(monitor);
            _tThread.Name = "検査長監視";
            _tThread.Start();

            return true;
        }

        public bool Stop()
        {
            if (_tThread == null)
                return false;

            _bStop = true;
            do
            {
                _tThread.Join(100);

            } while (_tThread.IsAlive);

            _tThread = null;

            return true;
        }

        public void SetIdle(bool bVal)
        {
            _bIdle = bVal;
        }

        bool _bIdle = false;
        bool _bStop = false;
        Thread _tThread  = null;
        private void monitor()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            bool bSame = false;
            double dPrevLength = 0.0;

            _bIdle = false;
            
            while (!_bStop)
            {
                if (!_bIdle)
                {
                    if (_autoInsp == null)
                        continue;
                    double dNowLength = _autoInsp.LengthMeas.TotalStartLength;
                    if (dPrevLength == dNowLength)
                    {
                        if (!bSame)
                        {
                            sw.Restart();
                            bSame = true;
                        }
                    }
                    else
                    {
                        // 変化していた場合
                        sw.Stop();
                        sw.Reset();
                        bSame = false;
                        dPrevLength = dNowLength;
                    }

                    if (bSame)
                    {
                        if (_iLengthMeasMonitorSec * 1000 <= sw.ElapsedMilliseconds)
                        {
                            if (LengthMeasError != null && !_bStop)
                                LengthMeasError(this, new EventArgs());
                            sw.Reset();
                            bSame = false;
                        }
                    }
                }
                else
                {
                    bSame = false;
                    sw.Stop();
                    sw.Reset();
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

    }
}
