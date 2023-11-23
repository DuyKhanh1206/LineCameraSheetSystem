using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fujita.Misc;
using Fujita.Communication;

namespace Fujita.LightControl
{
    public enum ELightColorType : int
    {
        White,
        Red,
        Green,
        Blue,
        UV,
        IR,
        Unknown,
    }

    public static class LightColorTypeExt
    {
        public static string ToStringExt(this ELightColorType e)
        {
            switch (e)
            {
                case ELightColorType.Red: return "赤";
                case ELightColorType.White: return "白";
                case ELightColorType.Green: return "緑";
                case ELightColorType.Blue: return "青";
                case ELightColorType.UV: return "紫外";
                case ELightColorType.IR: return "赤外";
            }
            return "不明";
        }
    }

                                                                                                        đánh dấu;
    /// <summary>lớp Light Type             --------------------------------------- </summary>
    public class LightType : IError
    {
        public int Index { get; private set; }
        public int Channel { get; private set; }

        // list liệt kê danh sách các CH hay các kênh ví dụ 01, 02, 03 ......
        public List<int> PareChannel { get; private set; }
        public string Name { get; private set; }
        public ELightColorType Color { get; private set; }

        public bool IsError { get; set; }
        public string ErrorReason { get; set; }
        private int _iLightOnErrorCnt = 0;
        void setError(bool bError, string sReason)
        {
            IsError = bError;
            ErrorReason = sReason;
        }

        public string LightColorName
        {
            get
            {
                return Color.ToStringExt();
            }
        }

        public string LightName
        {
            get
            {
                return Name + "(" + LightColorName + ")";
            }
        }

        LightPowerSupplayBase _Supplay;

        public int ValueMax
        {
            get
            {
                return _Supplay.ValueMax;
            }
        }

        public int ValueMin
        {
            get
            {
                return _Supplay.ValueMin;
            }
        }

        public LightPowerSupplayBase GetSupplay()
        {
            return _Supplay;
        }

        public LightType(LightPowerSupplayBase supplay, int index, int channel, List<int> pareChannel, string name, ELightColorType color)
        {
            Index = index;
            Channel = channel;
            if (pareChannel[0] == -1)
                PareChannel = new List<int>();
            else
                PareChannel = pareChannel;
            Name = name;
            Color = color;
            _Supplay = supplay;

            if (_Supplay is LightPowerSupplayError)
            {
                setError(true, "PowerSupply Error");
            }
        }

        public bool LightOn(int iValue, bool isCheckAck)
        {
            if (_Supplay == null)
            {
                setError(true, "PowerSupply object is null");
                return false;
            }

            //メンテナンスで基準値より輝度が足りないと、負数になっているのでマイナスする。　結果：指定値より大きい値を設定することになる
            int offsetValue = iValue - InspectionSystem.clsMainteFunc.getInstance().LightParam[Index].DiffLightValue;
            if (offsetValue > ValueMax)
                offsetValue = ValueMax;
            if (offsetValue < ValueMin)
                offsetValue = ValueMin;

            if (_Supplay.LightOn(Channel, offsetValue, isCheckAck))
            {
                setError(false, "");
                _iLightOnErrorCnt = 0;
                return true;
            }
            // 1.0.3.9 エラー回避の為
            else
            {
                _iLightOnErrorCnt++;
                if (_iLightOnErrorCnt >= 5)
                {
                    //setError(true, "PowerSupply.LightOn Error Name = " + Name);
                }
                LogingDllWrap.LogingDll.Loging_SetLogString("PowerSupply.LightOn Error Name =" + Name);
                return false;
            }
        }

        public bool LightOn255(int iValue, bool isCheckAck)
        {
            return _Supplay.LightOn255(Channel, iValue, isCheckAck);
        }

        public bool LightOff()
        {
            if (_Supplay == null)
            {
                return false;
            }

            return _Supplay.LightOff(Channel);
        }

        public override string ToString()
        {
            return Name + "(" + Color.ToStringExt() + ")";
        }
    }
                                                                            đánh dấu đỏ;
    /// <summary>  Lớp light power supply base để kế dùng kế thừa -------------------------------------------------------------------------------------------------------------</summary>
    public class LightPowerSupplayBase : IError
    {
        public static int RESPONSE_WAIT_SLEEPTIME = 20;

        public int ChannelNum { get; internal set; }

        public string Name { get; private set; }
        public string VenderName { get; private set; }

        public int ValueMin { get; private set; }
        public int ValueMax { get; private set; }

        public int StrobeDelayMax { get; private set; }
        public int StrobeDelayMin { get; private set; }
        public int StrobeRangeMax { get; private set; }
        public int StrobeRangeMin { get; private set; }
        public bool Strobe { get; private set; }

        protected int[] _iaLightValue;

        protected object _objSendLock = new object();

        Dictionary<int, LightType> _dicLight = new Dictionary<int, LightType>();

        protected CommunicationDIO _dio;
        protected CommunicationSIO _sio;
        protected CommunicationNet _net;

        /// <summary>
        /// 照明電源との通信チェックモニタ実施
        /// </summary>
        public bool MonitorEnable { get; set; }
        /// <summary>
        /// 照明電源への設定値
        /// 接続タイムアウト時間
        /// </summary>
        public int ConnectTimeOut { get; set; }

        public bool IsError { get; set; }
        public string ErrorReason { get; set; }

        public LightPowerSupplayBase(CommunicationDIO dio, CommunicationSIO sio, string name, string vendername, int num, int min, int max, bool strobe)
        {
            Name = name;
            VenderName = vendername;
            ValueMin = min;
            ValueMax = max;
            Strobe = strobe;
            ChannelNum = num;

            _iaLightValue = new int[num];
            for (int i = 0; i < num; i++)
            {
                _iaLightValue[i] = -1;
            }

            _dio = dio;
            _sio = sio;
            _net = null;

            MonitorEnable = false;
        }

        public LightPowerSupplayBase(CommunicationDIO dio, CommunicationSIO sio, CommunicationNet net, string name, string vendername, int num, int min, int max, bool strobe)
        {
            Name = name;
            VenderName = vendername;
            ValueMin = min;
            ValueMax = max;
            Strobe = strobe;
            ChannelNum = num;

            _iaLightValue = new int[num];
            for (int i = 0; i < num; i++)
            {
                _iaLightValue[i] = -1;
            }

            _dio = dio;
            _sio = sio;
            _net = net;

            MonitorEnable = true;
        }
        // lưu lại cho thông tin load được từ light manager vào _dicLight gồm tất cả thông tin lightType của cổng đó
        internal bool setLight(int iChannel, LightType lt)
        {
            if (lt.PareChannel.Count == 0) // chưa có danh sách kênh nào 
                if (iChannel < 1 || iChannel >= ChannelNum) // số CH < 1 ví dụ = 0 hoặc > channelNum tức là tổng số kênh thì báo sai( vì danh sách kênh tính theo thứ tự nên k lớn hơn được
                    return false;
            _dicLight.Add(iChannel, lt);    // nếu đúng thì add vào Dictionary light gồm số cổng và kiểu light
            return true;
        }

        // lấy ra LightType với key là iChannel 
        public LightType getLight(int iChannel)
        {
            if (_dicLight.Keys.Contains(iChannel))
                return _dicLight[iChannel];
            return null;
        }

        public void SetStrobeParam(int delaymin, int delaymax, int rangemin, int rangemax)
        {
            StrobeDelayMax = delaymax;
            StrobeDelayMin = delaymin;
            StrobeRangeMax = rangemax;
            StrobeRangeMin = rangemin;
        }

        public virtual bool SetStrobeDelay(int iChannle, int iDelay)
        {
            return false;
        }

        public virtual bool SetStrobeRange(int iChannel, int iRange)
        {
            return false;
        }

        public virtual bool GetStrobeDelay(int iChannel, ref int iDelay)
        {
            return false;
        }

        public virtual bool GetStrobeRange(int iChannel, ref int iRange)
        {
            return false;
        }

        public virtual bool Initialize()
        {
            return true;
        }

        public virtual bool Terminate()
        {
            return true;
        }

        public virtual bool SetLightValue(int iChannel, int iValue)
        {
            return false;
        }

        public virtual bool LightOn(int iChannel, int iValue, bool isCheckAck)
        {
            return false;
        }

        public bool LightOn255(int iChannel, int iValue, bool isCheckAck)
        {
            if (iValue >= 255)
            {
                iValue = ValueMax;
            }
            else if (iValue <= 0)
            {
                iValue = ValueMin;
            }
            else
            {
                iValue = (int)(((ValueMax - ValueMin) / 255.0) * iValue) + ValueMin;
            }
            return LightOn(iChannel, iValue, isCheckAck);
        }

        public virtual bool LightOff(int iChannel)
        {
            return false;
        }

        public virtual bool LightOffAll()
        {
            return false;
        }

        public virtual bool Load(string sPath, string sSection)
        {
            return true;
        }

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        double freq_micro = System.Diagnostics.Stopwatch.Frequency / 1000000.0;
        protected void PerformanceSleep(double micro)
        {
            long start = sw.ElapsedTicks;
            do
            {
            } while ((sw.ElapsedTicks - start) / freq_micro <= micro);
        }
    }

    public class LightPowerSupplayError : LightPowerSupplayBase
    {
        public LightPowerSupplayError(string sName, string sReason)
            : base(null, null, null, sName, "", 0, 0, 255, false)
        {
            IsError = true;
            ErrorReason = sReason;
        }
    }

    public class LightPowerSupplayDummy : LightPowerSupplayBase
    {
        public LightPowerSupplayDummy(CommunicationDIO dio, CommunicationSIO sio, CommunicationNet net, string name, string vendername, int num, int min, int max, bool strobe)
            : base(dio, sio, net, name, vendername, num, min, max, strobe)
        {
            LogingDllWrap.LogingDll.Loging_SetLogString("LightPowerSupplayDummy call Create");
        }

        public override bool Initialize()
        {
            LogingDllWrap.LogingDll.Loging_SetLogString("LightPowerSupplayDummy call Initialize");
            return true;
        }

        public override bool LightOn(int iChannel, int iValue, bool isCheckAck)
        {
            LogingDllWrap.LogingDll.Loging_SetLogString("LightPowerSupplayDummy call LightOn Channel=" + iChannel.ToString() + " Value=" + iValue.ToString());
            return true;
        }

        public override bool LightOff(int iChannel)
        {
            LogingDllWrap.LogingDll.Loging_SetLogString("LightPowerSupplayDummy call LightOff Channel=" + iChannel.ToString());
            return true;
        }

        public override bool LightOffAll()
        {
            LogingDllWrap.LogingDll.Loging_SetLogString("LightPowerSupplayDummy call LightOffAll");
            return true;
        }

        public override bool SetLightValue(int iChannel, int iValue)
        {
            LogingDllWrap.LogingDll.Loging_SetLogString("LightPowerSupplayDummy call SetLightValue Channel=" + iChannel.ToString() + " Value=" + iValue.ToString());
            return true;
        }

        public override bool Terminate()
        {
            LogingDllWrap.LogingDll.Loging_SetLogString("LightPowerSupplayDummy call Terminate");
            return true;
        }
    }
}
