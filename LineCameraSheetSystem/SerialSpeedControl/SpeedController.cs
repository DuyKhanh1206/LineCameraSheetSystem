using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace KaTool.Rs232c
{
	public class SpeedController : IDisposable
	{
		private Rs232cSerial _serial;
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="serial"></param>
		public SpeedController(Rs232cSerial serial, int iCycleWaitTime)
		{
			_serial = serial;
            _cycleWaitTime = iCycleWaitTime;
		}
		/// <summary>
		/// デストラクタ
		/// </summary>
		public void Dispose()
		{
			this.Close();
		}
		public bool IsOpen { get; private set; }
		/// <summary>
		/// オープン
		/// </summary>
		/// <returns></returns>
		public bool Open()
		{
			if (_serial.Open() == false)
			{
				return false;
			}
			this.IsOpen = true;
            StartThread();
			return true;
		}
		/// <summary>
		/// クローズ
		/// </summary>
		public void Close()
		{
            StopThread();
			_serial.Close();
			this.IsOpen = false;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public int IsACK(ref double dSpeed)
		{
			string recvData = "";
			bool ret = _serial.ReciveData(ref recvData);
            if (ret == true)
            {
                if ("SPRSP_" == recvData.Substring(0, 6))
                {
                    string sSpeedData = recvData.Substring(6);
                    double dd;
                    if (double.TryParse(sSpeedData, out dd) == true)
                        dSpeed = dd;
                    else
                        return -5;
                }
                else if (10 != recvData.Length)
                {
                    return -2;
                }
                else if ("NAK___" == recvData.Substring(0, 6))
                {
                    return -3;
                }
                else
                {
                    return -4;
                }
            }
            else
            {
                return -1;
            }
			return 0;
		}

        private Thread _th = null;
        private bool _bRunFlag = false;
        private int _cycleWaitTime = 1000;

        private void StartThread()
        {
            if (_th != null)
                return;
            _th = new Thread(new ThreadStart(Monitor));
            _th.Name = "SpeedMonitor_" + _serial.PortName;
            _th.IsBackground = true;
            _bRunFlag = true;
            _th.Start();
        }
        private void StopThread()
        {
            if (_th == null)
                return;

            _bRunFlag = false;
            //_th.Join();
            _th = null;
        }

        private void Monitor()
        {
            while(_bRunFlag)
            {
                _serial.SendData("SPREQ_");
                Thread.Sleep(10);
                double dSpeed = 0.0;
                if (IsACK(ref dSpeed) == 0)
                {
                    if (OnGetSpeed != null)
                        OnGetSpeed(this, dSpeed);
                }
                Thread.Sleep(_cycleWaitTime);
            }
        }
        public delegate void GetSpeedEventHandler(object sender, double val);
        public event GetSpeedEventHandler OnGetSpeed;
    }
}
