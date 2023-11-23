using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fujita.Misc;

namespace LineCameraSheetSystem
{
    class SpeedErrorEventArgs : EventArgs
    {
    }

    delegate void SpeedErrorEventHandler( object sender, SpeedErrorEventArgs e );

    class clsNoInspectionSpeedMonitor
    {
        private static clsNoInspectionSpeedMonitor _singleton = new clsNoInspectionSpeedMonitor();

       private clsNoInspectionSpeedMonitor()
        {
            Initialize();
        }

       public static clsNoInspectionSpeedMonitor GetInstance()
        {
            return _singleton;
        }
        
        public bool Initialize()
        {
            return true;
        }

        public bool Load(string sPath, string sSection = "")
        {
            IniFileAccess ifa = new IniFileAccess();

            _iLimitMinute = ifa.GetIni("NoInspectionSpeedMonitor_Param", "LimitMinute", _iLimitMinute, sPath);
            _dLimitSpeedMPM = ifa.GetIni("NoInspectionSpeedMonitor_Param", "LimitSpeedMPM", _dLimitSpeedMPM, sPath);
            _iLimitSeriesCnt = ifa.GetIni("NoInspectionSpeedMonitor_Param", "LimitSeriesCnt", _iLimitSeriesCnt, sPath);

            return true;
        }

        public bool Save(string sPath, string sSection = "")
        {
            IniFileAccess ifa = new IniFileAccess();

            ifa.SetIni("NoInspectionSpeedMonitor_Param", "LimitMinute", _iLimitMinute, sPath);
            ifa.SetIni("NoInspectionSpeedMonitor_Param", "LimitSpeedMPM", _dLimitSpeedMPM, sPath);
            ifa.SetIni("NoInspectionSpeedMonitor_Param", "LimitSeriesCnt", _iLimitSeriesCnt, sPath);

            return true;
        }

        public event SpeedErrorEventHandler SpeedError;
        int _iLimitMinute = 15;
        public int LimitMinute
        {
            get { return _iLimitMinute; }
            set
            {
                if (value < 0)
                    return;
                _iLimitMinute = value;
            }
        }

        double _dLimitSpeedMPM = 4.0;
        public double LimitSpeedMPM
        {
            get { return _dLimitSpeedMPM; }
            set
            {
                if (value < 0.0)
                    return;
                _dLimitSpeedMPM = value;
            }
        }

        /// <summary>
        /// リミット
        /// </summary>
        int _iLimitSeriesCnt = 3;
        public int LimitSeriesCnt
        {
            get { return _iLimitSeriesCnt; }
            set
            {
                if (value < 0 )
                    return;
                _iLimitSeriesCnt = value;
            }
        }

        int _iInterval = 1000;

        int _iSeriesCnt = 0;
        double _dNowSpeed = 0.0;
        /// <summary>
        /// 現在のスピードをセットする
        /// </summary>
        /// <param name="dNowSpeed"></param>
        public void SetSpeed(double dNowSpeed)
        {
            _dNowSpeed = dNowSpeed;
        }

        bool _bStop = false;
        System.Threading.Thread _tThread = null;
        public bool Start()
        {
            if (_tThread != null)
                return false;

            _bStop = false;

            _tThread = new System.Threading.Thread(monitor);
            _tThread.Name = "未検査速度監視ｽﾚｯﾄﾞ";
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

            _iSeriesCnt = 0;
            _bLimitOver = false;

            _tThread = null;
            return true;
        }

        public void Reset()
        {
            _iSeriesCnt = 0;
            _bLimitOver = false;
        }

        bool _bLimitOver = false;
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        void monitor()
        {
            while (!_bStop)
            {
                System.Threading.Thread.Sleep(_iInterval);

                if (_bLimitOver)
                {
                    if (_dNowSpeed < _dLimitSpeedMPM)
                    {
                        _iSeriesCnt++;
                        if (_iSeriesCnt > _iLimitSeriesCnt)
                        {
                            sw.Stop();
                            _iSeriesCnt = 0;
                            _bLimitOver = false;
                        }
                    }
                    else
                    {
                        _iSeriesCnt = 0;
                    }
                }
                else
                {
                    if (_dNowSpeed >= _dLimitSpeedMPM)
                    {
                        _iSeriesCnt++;
                        if (_iSeriesCnt > _iLimitSeriesCnt)
                        {
                            sw.Restart();
                            _iSeriesCnt = 0;
                            _bLimitOver = true;
                        }
                    
                    }
                    else
                    {
                        _iSeriesCnt = 0;
                    }
                }

                if (_iLimitMinute * 1000 * 60 < sw.ElapsedMilliseconds)
                {
                    if (SpeedError != null)
                        SpeedError(this, new SpeedErrorEventArgs());
                    _iSeriesCnt = 0;
                    _bLimitOver = false;
                    sw.Reset();
                    sw.Stop();
                }
            }

            _iSeriesCnt = 0;
            _bLimitOver = false;
            sw.Reset();
            sw.Stop();
        }
    }
}
