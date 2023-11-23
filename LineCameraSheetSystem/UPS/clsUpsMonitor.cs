using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace LineCameraSheetSystem
{
    class UpsShutdownEventArg : EventArgs
    {
        public DateTime UpsBreakDownTime
        {
            get;
            private set;
        }

        public UpsShutdownEventArg(DateTime dt)
        {
            UpsBreakDownTime = dt;
        }
    }

    delegate void UpsShutdownEventHandler( object sender, UpsShutdownEventArg e );

    class clsUpsShutdownMonitor
    {
        DateTime _dtUpsBreakdownTime;
        UpsController _UpsCtrl = null;
        Thread _tThread = null;
        bool _bStop = false;

        int _iShutdownIntervalSec = 30;
        
        public event UpsShutdownEventHandler UpsShutdown;

        public bool Initialize()
        {
            if (_UpsCtrl != null)
                return false;

            _UpsCtrl = new UpsController();
            _UpsCtrl.Initialize();
            _UpsCtrl.OnUpsRemoteEvent += _UpsCtrl_OnUpsRemoteEvent;

            return true;
        }

        public bool Terminate()
        {
            if (_UpsCtrl == null)
                return false;

            _UpsCtrl.OnUpsRemoteEvent -= _UpsCtrl_OnUpsRemoteEvent;
            _UpsCtrl.Dispose();
            _UpsCtrl = null;

            return true;
        }

        void _UpsCtrl_OnUpsRemoteEvent(object sender, UpsRemote.UpsRemoteEventArgs e)
        {
            if (e.EventCode == UpsRemote.UpsEventCode.BreakDown)
            {
                _dtUpsBreakdownTime = DateTime.Now;
                startThread();
            }
            else　if(  e.EventCode == UpsRemote.UpsEventCode.PowerFailRecovery )
            {
                stopThread();
            }
        }

        public int ShutdownIntervalSec
        {
            get { return _iShutdownIntervalSec; }
            set
            {
                if( _tThread != null ) 
                    return;

                // ５秒以下、1800秒以上は設定不可
                if (value < 5 || value > 1800 )
                    return;

                _iShutdownIntervalSec = value;
            }
        }

        private bool startThread()
        {
            if (_tThread != null)
                return false;

            _bStop = false;
            _tThread = new Thread(shutdownTimer);
            _tThread.Name = "ｼｬｯﾄﾀﾞｳﾝﾀｲﾏｰ";
            _tThread.Start();

            return true;
        }

        private bool stopThread()
        {
            if (_tThread == null)
            {
                return false;
            }

            if (!_tThread.IsAlive)
            {
                _tThread = null;
                return true;
            }

            do
            {
                _tThread.Join(100);
            } while (_tThread.IsAlive);
            return true;
        }

        private void shutdownTimer()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Start();
            int iCnt = 0;
            while (!_bStop)
            {
                System.Threading.Thread.Sleep(1000);
                iCnt++;

                if (_iShutdownIntervalSec * 1000 < sw.ElapsedMilliseconds )
                {
                    if (UpsShutdown != null)
                    {
                        UpsShutdown(this, new UpsShutdownEventArg(_dtUpsBreakdownTime));
                    }
                    break;
                }
            }
        }
    }
}
