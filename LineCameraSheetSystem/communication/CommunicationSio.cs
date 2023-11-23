using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO.Ports;
using System.Reflection;

using Fujita.Misc;

namespace Fujita.Communication
{
    public class CommunicationSIO : CommunicationBase, IDisposable
    {
        protected SerialPort _serial;

        public string PortName { get; private set; }
        public int BaudRate { get; private set; }
        public int Databit { get; private set; }
        public StopBits Stopbit { get; private set; }
        public Parity Parity { get; private set; }
        public Handshake HandShake { get; private set; }
        public string NewLine { get; private set; }

        public bool DtrEnable { get; private set; }
        public bool RtsEnable { get; private set; }

        public int ReadTimeout
        {
            get { return _serial.ReadTimeout; }
            set
            {
                _serial.ReadTimeout = value;
            }
        }
        public int WriteTimeout
        {
            get { return _serial.WriteTimeout; }
            set
            {
                _serial.WriteTimeout = value;
            }
        }

        public SerialPort SerialPort
        {
            get
            {
                return _serial;
            }
        }

        public void Dispose()
        {
            Close();
        }

        public bool IsOpen
        {
            get
            {
                return _serial.IsOpen;
            }
        }

        public CommunicationSIO(string name, string jpnName)
            : base(name, jpnName, ECommunicationType.SIO)
        {
            _serial = new SerialPort();
        }

        public override bool Open()
        {
            string[] saSerialPorts = SerialPort.GetPortNames();

            if (saSerialPorts.Count(o => o == PortName) != 1)
            {
                IsError = true;
                return false;
            }

            try
            {
                _serial.PortName = PortName;
                _serial.BaudRate = BaudRate;
                _serial.DataBits = Databit;
                _serial.StopBits = Stopbit;
                _serial.Parity = Parity;
                _serial.Handshake = HandShake;
                _serial.Open();
                _serial.Encoding = System.Text.Encoding.GetEncoding("SHIFT-JIS");

                if (NewLine != null && NewLine != "")
                    _serial.NewLine = NewLine;

                _serial.DtrEnable = DtrEnable;
                _serial.RtsEnable = RtsEnable;
            }
            catch (Exception e)
            {
#if FUJITA_INSPECTION_SYSTEM
                Log.Write(this, e, AppData.getInstance().logger);
#else
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
#endif
                IsError = true;
                return false;
            }
            IsError = false;
            return true;
        }

        public override bool Load(string sPath, string sSection)
        {
            IniFileAccess ifa = new IniFileAccess();
            PortName = ifa.GetIniString(sSection, "PortName", sPath, "");
            BaudRate = ifa.GetIniInt(sSection, "BaudRate", sPath, 9600);
            Databit = ifa.GetIniInt(sSection, "DataBit", sPath, 8);

            string sStopbit = ifa.GetIniString(sSection, "StopBit", sPath, "None");
            switch (sStopbit.ToLower())
            {
                case "none":
                case "n":
                    Stopbit = StopBits.None;
                    break;
                case "one":
                case "1":
                    Stopbit = StopBits.One;
                    break;
                case "onepointfive":
                case "1.5":
                    Stopbit = StopBits.OnePointFive;
                    break;
                case "two":
                case "2":
                    Stopbit = StopBits.Two;
                    break;
            }
            string sParity = ifa.GetIniString(sSection, "ParityBit", sPath, "None");
            switch (sParity.ToLower())
            {
                case "none":
                case "n":
                    Parity = Parity.None;
                    break;
                case "even":
                case "e":
                    Parity = Parity.Even;
                    break;
                case "odd":
                case "o":
                    Parity = Parity.Odd;
                    break;
                case "space":
                case "s":
                    Parity = Parity.Space;
                    break;
                case "mark":
                case "m":
                    Parity = Parity.Mark;
                    break;
            }
            string sHandShake = ifa.GetIniString(sSection, "HandShake", sPath, "none");
            switch (sHandShake.ToLower())
            {

                case "rts":
                case "requesttosend":
                    HandShake = Handshake.RequestToSend;
                    break;
                case "rtsxonxoff":
                case "requesttosendxonxoff":
                    HandShake = Handshake.RequestToSendXOnXOff;
                    break;
                case "xonxoff":
                    HandShake = Handshake.XOnXOff;
                    break;
                case "":
                case "none":
                default:
                    HandShake = Handshake.None;
                    break;
            }

            string sNewLine = ifa.GetIniString(sSection, "NewLine", sPath, "");
            Regex reg = new Regex(@"^\[(([\da-fA-F]{2})+)\]$");

            // "["で始まり、"]"で終わる場合、バイナリコードとして扱う
            // 0-9A-Fまでの2文字で1byteを表す
            if (sNewLine != "")
            {
                if (reg.IsMatch(sNewLine))
                {
                    Match m = reg.Match(sNewLine);
                    Group g = m.Groups[1];
                    string s = g.Value;
                    sNewLine = "";
                    for (int i = 0; i < s.Length; i += 2)
                    {
                        sNewLine += char.ConvertFromUtf32(Convert.ToInt32(s.Substring(i, 2), 16));
                    }
                }
                else
                {
                    // 通常モードの場合
                    sNewLine = sNewLine.Replace("[CR]", "\r");
                    sNewLine = sNewLine.Replace("[LF]", "\n");
                }
            }
            NewLine = sNewLine;

            DtrEnable = ifa.GetIniBoolean(sSection, "DtrEnable", sPath, false);
            RtsEnable = ifa.GetIniBoolean(sSection, "RtsEnable", sPath, false);

            ReadTimeout = ifa.GetIni(sSection, "ReadTimeout", _serial.ReadTimeout, sPath);
            WriteTimeout = ifa.GetIni(sSection, "WriteTimeout", _serial.WriteTimeout, sPath);

            return true;
        }

        public void SetCommandDelimiter(string sDel)
        {
            _serial.NewLine = sDel;
        }

        public override bool Close()
        {
            if (!_serial.IsOpen)
                return true;

            _serial.Close();
            IsError = false;
            return true;
        }

        public bool WriteStringLine(string sData)
        {
            if (!_serial.IsOpen)
            {
                IsError = true;
                return false;
            }

            try
            {
                _serial.WriteLine(sData);
            }
            catch (Exception e)
            {
#if FUJITA_INSPECTION_SYSTEM
                Log.Write(this, e, AppData.getInstance().logger);
#else
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
#endif
                IsError = true;
                return false;
            }
            IsError = false;
            return true;
        }

        public bool WriteString(string sData)
        {
            if (!_serial.IsOpen)
            {
                IsError = true;
                return false;
            }


            try
            {
                _serial.Write(sData);
            }
            catch (Exception e)
            {
#if FUJITA_INSPECTION_SYSTEM
                Log.Write(this, e, AppData.getInstance().logger);
#else
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
#endif
                IsError = true;
                return false;
            }
            IsError = false;
            return true;
        }

        public bool ReadStringLine(ref string sData)
        {
            if (!_serial.IsOpen)
            {
                IsError = true;
                return false;
            }

            try
            {
                sData = _serial.ReadLine();
            }
            catch (Exception e)
            {
#if FUJITA_INSPECTION_SYSTEM
                Log.Write(this, e, AppData.getInstance().logger);
#else
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
#endif
                IsError = true;
                return false;
            }
            IsError = false;
            return true;
        }

        public bool ReadString(ref string sData)
        {
            if (!_serial.IsOpen)
            {
                IsError = true;
                return false;
            }

            try
            {
                byte[] bytBuf = new byte[256];
                int iReadSize = _serial.Read(bytBuf, 0, bytBuf.Length);
                sData = Encoding.ASCII.GetString(bytBuf, 0, iReadSize);
            }
            catch (Exception e)
            {
#if FUJITA_INSPECTION_SYSTEM
                Log.Write(this, e, AppData.getInstance().logger);
#else
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
#endif
                IsError = true;
                return false;
            }
            IsError = false;
            return true;
        }
    }
}
