using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.IO.Ports;

namespace KaTool.Rs232c
{
    public class Rs232cSerial
    {
        #region 受信データEventArgs
        /// <summary>
        /// 受信データ
        /// </summary>
        public class ReceiveEventArgs
        {
            public string ReceiveData { get; private set; }
            public ReceiveEventArgs(string data)
            {
                this.ReceiveData = data;
            }
        }
        #endregion

        #region シリアルハンドル
        /// <summary>
		/// シリアルハンドル
		/// </summary>
        SerialPort _serialHandle = null;
        #endregion

        #region ポート番号
        /// <summary>
        /// ポート番号
        /// </summary>
        public string PortName { get; private set; }
        #endregion

        #region イベントハンドラ
        /// <summary>
        /// イベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		public delegate void ReceiveSerialEventHandler(object sender, ReceiveEventArgs e);
		public event ReceiveSerialEventHandler OnEventReceiveSerial = null;
        #endregion

        #region 改行コード
        /// <summary>
		/// 改行コード
		/// </summary>
		public enum ReturnCode
		{
			CR,
            LF,
            CRLF,
			EXT,
		};
        #endregion

		private bool _sxtextMode = false;

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="portName">PORT名</param>
        /// <param name="baudRate">ボーレート</param>
        /// <param name="parity">パリティ</param>
        /// <param name="dataBits">データビット</param>
        /// <param name="stopBits">ストップビット</param>
        /// <param name="handShake">制御</param>
        /// <param name="enCoding">文字コード</param>
        /// <param name="rtnCode">改行コード</param>
        public Rs232cSerial(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, Handshake handShake, string enCoding, ReturnCode rtnCode)
		{
            this.PortName = portName;

            _serialHandle = new SerialPort();
            _serialHandle.PortName = portName;		//PORT名称
            _serialHandle.BaudRate = baudRate;		//ボーレート
            _serialHandle.Parity = parity;			//パリティ
            _serialHandle.DataBits = dataBits;		//データビット
            _serialHandle.StopBits = stopBits;		//ストップビット
            _serialHandle.Handshake = handShake;	//制御
            _serialHandle.Encoding = Encoding.GetEncoding(enCoding);	//文字コード
            //_serialHandle.DtrEnable = true;		//DTRをON
            //_serialHandle.RtsEnable = true;		//RSTをON
            switch (rtnCode)
            {
                case ReturnCode.CR: _serialHandle.NewLine = "\r"; break;
                case ReturnCode.LF: _serialHandle.NewLine = "\n"; break;
				case ReturnCode.CRLF: _serialHandle.NewLine = "\r\n"; break;
				case ReturnCode.EXT:
					_serialHandle.NewLine = "\u0003";
					_sxtextMode = true;
					break;
			}
            //受信を開始する
			//EntryOnRecive();
        }
        #endregion

        #region デストラクタ
        /// <summary>
		/// リソースを破棄する。
		/// </summary>
		public void Dispose()
		{
			this.Close();
		}
        #endregion


        //public bool IsOpen
        //{
        //    get { return (_serialHandle != null) ? true : false; }
        //}

        #region オープン
        /// <summary>
		/// SERIALオープン
		/// </summary>
		/// <param name="portName">PORT名</param>
		/// <param name="baudRate">ボーレート</param>
		/// <param name="parity">パリティ</param>
		/// <param name="dataBits">データビット</param>
		/// <param name="stopBits">ストップビット</param>
		/// <param name="handShake">制御</param>
		/// <param name="enCoding">文字コード</param>
		/// <param name="rtnCode">改行コード</param>
		/// <returns></returns>
		public bool Open()
		{
			try
			{
				_serialHandle.Open();
			}
			catch(Exception ex)
			{
                _serialHandle = null;
				Debug.WriteLine(string.Format("SendData() ex = {0}", ex.Message));
				return false;
			}
			return true;
		}
        #endregion

        #region クローズ
        /// <summary>
		/// SERIALクローズ
		/// </summary>
		public void Close()
		{
            try
            {
                if (_serialHandle != null)
                {
                    _serialHandle.Close();
                    //_serialHandle = null;
                }
            }
            catch
            {
            }
		}
        #endregion


		private bool _entry = false;
		public void EntryOnRecive()
		{
			if (_entry == true)
				return;
			_serialHandle.DataReceived += OnDataReceived;
			_entry = true;
		}
		public void RemoveOnRecive()
		{
			if (_entry == false)
				return;
			_serialHandle.DataReceived -= OnDataReceived;
			_entry = false;
		}

        #region 受信イベント
        /// <summary>
		/// 受信
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			//受信データを読み込む
			// NewLineで指定したコードが受信されるまで固まる
			string reciveData = _serialHandle.ReadLine();
			string str;
			str = reciveData;
			if (_sxtextMode == true)
			{
				if (reciveData[0] != (char)0x2)
					return;
				str = reciveData.Substring(1);				
			}
			if (OnEventReceiveSerial != null)
			{
				OnEventReceiveSerial(this, new ReceiveEventArgs(str));
			}
		}
        #endregion


		#region 手動受信
		public bool ReciveData(ref string reciveData)
		{
			try
			{
				_serialHandle.ReadTimeout = 3000;
				string str = _serialHandle.ReadLine();
				reciveData = str;
				if (_sxtextMode == true)
				{
					if (str[0] != (char)0x2)
					{
						return false;
					}
					reciveData = reciveData.Substring(1);
				}
			}
			catch
			{
				return false;
			}
			return true;
		}
		#endregion

		#region 送信
		/// <summary>
		/// 送信
		/// </summary>
		/// <param name="sendData"></param>
		public void SendData(string sendData)
		{
			try
			{
				string str = sendData;
				if (_sxtextMode == true)
				{
					str = "\u0002" + sendData;
				}
				_serialHandle.WriteLine(str);
			}
			catch(Exception ex)
			{
				Debug.WriteLine(string.Format("SendData() ex = {0}", ex.Message));
			}
        }
        #endregion

    }
}
