using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;       // Marshalを使用する
using System.Reflection;

using InterfaceCorpDllWrap;                 // Interface DIO
using Fujita.Misc;


namespace Fujita.Communication
{
    public class CommunicationDIO : CommunicationBase, IDisposable
    {
        protected List<bool> _lstInOut;
        protected List<bool> _lstOutData;

        public int InNum { get; internal set; }
        public int OutNum { get; internal set; }

        public CommunicationDIO(string name, string jpnName)
            : base(name, jpnName, ECommunicationType.DIO)
        {
        }

        public void Dispose()
        {
            Close();
        }

        public virtual bool OUT1(int iAddr, bool blnData)
        {
            return false;
        }

        public virtual bool OUT8(int iIndex, byte bytData)
        {
            return false;
        }

        public virtual bool OUT16(int iIndex, ushort shtData)
        {
            return false;
        }

        public virtual bool OUT32(int iIndex, uint uiData)
        {
            return false;
        }

        public virtual bool IN1(int iAddr, ref bool blnData)
        {
            return false;
        }

        public virtual bool IN8(int iIndex, ref byte bytData)
        {
            return false;
        }

        public virtual bool IN16(int iIndex, ref ushort ushData)
        {
            return false;
        }

        public virtual bool IN32(int iIndex, ref uint uiData)
        {
            return false;
        }

        public virtual void SetFormAutoInspInfo(InspectionNameSpace.FormAutoInspInfo frm)
        {
            return;
        }
    }

    public class CommunicationDIOInterface : CommunicationDIO
    {
        IntPtr _ipHandle = (IntPtr)(-1);
        string _sDioName;
        public CommunicationDIOInterface(string name, string jpnName)
            : base(name, jpnName)
        {
        }

        public override bool Load(string sPath, string sSection)        // load thông tin DIO
        {
            IniFileAccess ifa = new IniFileAccess();

            InNum = ifa.GetIniInt(sSection, "InNum", sPath, 0);
            OutNum = ifa.GetIniInt(sSection, "OutNum", sPath, 0);

            _sDioName = ifa.GetIniString(sSection, "DioName", sPath, "");
            if (_sDioName == "")
                return false;

            base.Load(sPath, sSection);

            return true;
        }

        private bool IsHandleOpened()           //Tay cầm đã được mở
        {
            return (_ipHandle != (IntPtr)(-1)); /*kiểm tra xem _ipHandle có phải giá trị hợp lệ hay không?*/
        }

        public override bool Open() // kết nối DIO
        {
            _ipHandle = IFCDIO_ANY.DioOpen(_sDioName, IFCDIO.FBIDIO_FLAG_SHARE);

            if (_ipHandle == (IntPtr)(-1))
            {
                IsError = true;
            }
            else
            {
                IsError = false;
            }

            return (_ipHandle != (IntPtr)(-1));
        }

        private object _objLock = new object();

        int[] iOneBuffer = new int[1];
        public override bool OUT1(int iAddr, bool blnData)
        {
            if (!IsHandleOpened())// kiểm tra nếu true là đã kết nối tới DIO
            {
                IsError = true;
                return false;
            }

            if (iAddr <= 0 || iAddr > OutNum)
                return false;

            lock (_objLock)
            {
                iOneBuffer[0] = blnData ? 1 : 0;
                if (IFCDIO_ANY.FBIDIO_ERROR_SUCCESS == IFCDIO_ANY.DioOutputPoint(_ipHandle, iOneBuffer, (uint)iAddr, (uint)1)) // gửi 1 trạng thái blnData?  0 : 1  tới 1 cổng DIO ( có thể là bật tắt Thiết bị)
                {
                    IsError = false;
                }
                else
                {
                    IsError = true;
                }
            }
            LogingDllWrap.LogingDll.Loging_SetLogString("OUT01-" + "Address:" + iAddr.ToString("00") + " " + (blnData ? "1" : "0") + " " + IsError.ToString() + DateTime.Now.ToString("  yyyy/MM/dd HH:mm:ss.fff"));

            return !IsError;
        }
        public override bool OUT8(int iIndex, byte bytData)
        {
            if (!IsHandleOpened())
            {
                IsError = true;
                return false;
            }
            if (iIndex < 1)
                return false;
            lock (_objLock)
            {
                if (IFCDIO_ANY.FBIDIO_ERROR_SUCCESS == IFCDIO_ANY.DioOutputByte(_ipHandle, iIndex - 1, bytData))//iIndex là chỉ số của cổng DIO, bytData là giá trị byte cần gửi ra cổng đó.
                {
                    IsError = false;
                }
                else
                {
                    IsError = true;
                }
            }
            return !IsError;
        }
        public override bool OUT16(int iIndex, ushort shtData)
        {
            if (!IsHandleOpened())
            {
                IsError = true;
                return false;
            }

            if (iIndex < 1)
                return false;

            lock (_objLock)
            {
                if (IFCDIO_ANY.FBIDIO_ERROR_SUCCESS == IFCDIO_ANY.DioOutputWord(_ipHandle, iIndex - 1, shtData))//iIndex là chỉ số của cổng DIO, shtData là giá trị 16-bit cần gửi ra cổng đó.
                {
                    IsError = false;
                }
                else
                {
                    IsError = true;
                }
            }
            return !IsError;
        }
        public override bool OUT32(int iIndex, uint uiData)
        {
            if (!IsHandleOpened())
            {
                IsError = true;
                return false;
            }

            if (iIndex < 1)
                return false;

            lock (_objLock)
            {
                if (IFCDIO_ANY.FBIDIO_ERROR_SUCCESS == IFCDIO_ANY.DioOutputDword(_ipHandle, iIndex - 1, uiData))//iIndex là chỉ số của cổng DIO, uiData là giá trị 32-bit cần gửi ra cổng đó.
                {
                    IsError = false;
                }
                else
                {
                    IsError = true;
                }
            }
            return !IsError;
        }

        public override bool IN1(int iAddr, ref bool blnData)
        {
            if (!IsHandleOpened())
                return false;
            if (iAddr < 1)
                return false;

            lock (_objLock)
            {
                uint uiRet = IFCDIO_ANY.DioInputPoint(_ipHandle, iOneBuffer, (uint)iAddr, (uint)1);//iIndex là chỉ số của cổng DIO.
                blnData = (iOneBuffer[0] == 1) ? true : false;//bytData chứa giá trị byte đọc được từ cổng DIO.
                if (uiRet == IFCDIO_ANY.FBIDIO_ERROR_SUCCESS)
                {
                    IsError = false;
                }
                else
                {
                    IsError = true;
                }
            }
            return !IsError;
        }

        public override bool IN8(int iIndex, ref byte bytData)
        {
            if (!IsHandleOpened())
            {
                IsError = true;
                return false;
            }

            if (iIndex < 1)
                return false;
            lock (_objLock)
            {
                //Đọc giá trị byte từ một cổng DIO cụ thể (iIndex).

                if (IFCDIO_ANY.FBIDIO_ERROR_SUCCESS == IFCDIO_ANY.DioInputByte(_ipHandle, iIndex, out bytData))//iIndex là chỉ số của cổng DIO.----bytData chứa giá trị byte đọc được từ cổng DIO.
                {
                    IsError = false;
                }
                else
                {
                    IsError = true;
                }
            }
            return !IsError;
        }

        public override bool IN16(int iIndex, ref ushort ushData)
        {
            if (!IsHandleOpened())
            {
                IsError = true;
                return false;
            }
            if (iIndex < 1)
                return false;
            lock (_objLock)
            {
                if (IFCDIO_ANY.FBIDIO_ERROR_SUCCESS == IFCDIO_ANY.DioInputWord(_ipHandle, iIndex, out ushData)) //ushData chứa giá trị 16-bit đọc được từ cổng DIO.
                {
                    IsError = false;
                }
                else
                {
                    IsError = true;
                }
            }
            return !IsError;
        }

        public override bool IN32(int iIndex, ref uint uiData)
        {
            if (!IsHandleOpened())
            {
                IsError = true;
                return false;
            }

            if (iIndex < 1)
                return false;

            lock (_objLock)
            {
                if (IFCDIO_ANY.FBIDIO_ERROR_SUCCESS == IFCDIO_ANY.DioInputDword(_ipHandle, iIndex, out uiData)) //uiData chứa giá trị 32-bit đọc được từ cổng DIO.
                {
                    IsError = false;
                }
                else
                {
                    IsError = true;
                }
            }
            return !IsError;
        }

        public override bool Close()
        {
            if (!IsHandleOpened())
                return true;

            IFCDIO_ANY.DioClose(_ipHandle);

            _ipHandle = (IntPtr)(-1);

            return true;
        }

    }

    public class CommunicationDIOSharedMemory : CommunicationDIO
    {
        SharedMemory _sharedMemory = null;
        bool _bServer = true;
        public bool Server { get { return _bServer; } }
        string _sMemoryName = "";
        public string MemoryName { get { return _sMemoryName; } }
        public CommunicationDIOSharedMemory(string name, string jpnName)
            : base(name, jpnName)
        {
        }

        public override bool Load(string sPath, string sSection)
        {
            IniFileAccess ifa = new IniFileAccess();

            _bServer = ifa.GetIniInt(sSection, "Server", sPath, 1) == 1 ? true : false;
            _sMemoryName = ifa.GetIniString(sSection, "MemoryName", sPath, "");
            if (_sMemoryName == "")
                return false;

            InNum = ifa.GetIniInt(sSection, "InNum", sPath, 32);
            OutNum = ifa.GetIniInt(sSection, "OutNum", sPath, 32);

            return base.Load(sPath, sSection);
        }

        public override bool Open()
        {
            try
            {
                if (_bServer)
                {
                    _sharedMemory = SharedMemory.Create(_sMemoryName, InNum + OutNum);

                    //初期状態を書き込む
                    for (int i = 0; i < InNum; i++)
                    {
                        _sharedMemory.WriteByte(i, (byte)0);
                    }
                    for (int i = 0; i < OutNum; i++)
                    {
                        _sharedMemory.WriteByte(InNum + i, (byte)0);
                    }
                }
                else
                {
                    _sharedMemory = SharedMemory.Open(_sMemoryName, SharedMemoryAccessMode.AllAccess);
                }
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

        public override bool IN1(int iAddr, ref bool blnData)
        {
            if (_sharedMemory == null)
            {
                IsError = true;
                return false;
            }

            if (iAddr < 1 || iAddr > InNum)
                return false;

            int iOffset = Server ? 0 : OutNum;
            try
            {
                byte bytData = _sharedMemory.ReadByte(iOffset + iAddr - 1);
                blnData = bytData == 0 ? false : true;
            }
            catch (AccessViolationException)
            {
                IsError = true;
                return false;
            }
            IsError = true;
            return true;
        }

        public override bool OUT1(int iAddr, bool blnData)
        {
            if (_sharedMemory == null)
            {
                IsError = true;
                return false;
            }

            if (iAddr < 1 || iAddr > InNum)
                return false;

            int iOffset = Server ? InNum : 0;

            try
            {
                _sharedMemory.WriteByte(iOffset + iAddr - 1, blnData ? (byte)1 : (byte)0);
                LogingDllWrap.LogingDll.Loging_SetLogString("OUT01-" + "Address:" + iAddr.ToString("00") + " " + (blnData ? "1" : "0") + " " + IsError.ToString() + DateTime.Now.ToString("  yyyy/MM/dd HH:mm:ss.fff"));
            }
            catch (AccessViolationException)
            {
                IsError = true;
            }
            IsError = false;
            return true;
        }

#pragma warning disable 0649
        struct Buf8
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] bytData;
        }
#pragma warning restore 0649

        public override bool IN8(int iIndex, ref byte bytData)
        {
            if (_sharedMemory == null)
            {
                IsError = true;
                return false;
            }

            if (iIndex < 1 || iIndex > InNum / 8)
                return false;

            try
            {
                int iOffset = Server ? 0 : OutNum;
                Buf8 buf = (Buf8)_sharedMemory.ReadStructure(iOffset + (iIndex - 1) * 8, typeof(Buf8));
                bytData = (byte)0;
                for (int i = 0; i < buf.bytData.Length; i++)
                {
                    if (buf.bytData[i] != 0)
                        bytData |= (byte)(1 << i);
                }
            }
            catch
            {
                IsError = true;
                return false;
            }

            IsError = false;
            return true;
        }

        public override bool OUT8(int iIndex, byte bytData)
        {
            if (_sharedMemory == null)
            {
                IsError = true;
                return false;
            }

            if (iIndex < 1 || iIndex > InNum / 8)
                return false;
            try
            {
                Buf8 buf = new Buf8();
                for (int i = 0; i < 8; i++)
                {
                    if (((bytData >> i) & 1) == 1)
                    {
                        buf.bytData[i] = (byte)1;
                    }
                    else
                    {
                        buf.bytData[i] = (byte)0;
                    }
                }

                int iOffset = Server ? InNum : 0;
                _sharedMemory.WriteStructure(iOffset + (iIndex - 1) * 8, buf);
            }
            catch
            {
                IsError = true;
                return false;
            }

            IsError = false;
            return true;
        }


#pragma warning disable 0649
        struct Buf16
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] bytData;
        }
#pragma warning restore 0649

        public override bool IN16(int iIndex, ref ushort ushData)
        {
            if (_sharedMemory == null)
            {
                IsError = true;
                return false;
            }

            if (iIndex < 1 || iIndex > InNum / 16)
                return false;
            try
            {

                int iOffset = Server ? 0 : OutNum;
                Buf16 buf = (Buf16)_sharedMemory.ReadStructure(iOffset + (iIndex - 1) * 16, typeof(Buf16));

                ushData = 0;
                for (int i = 0; i < buf.bytData.Length; i++)
                {
                    if (buf.bytData[i] != 0)
                    {
                        ushData |= (ushort)(1 << i);
                    }
                }
            }
            catch
            {
                IsError = true;
            }
            IsError = false;
            return true;
        }

        public override bool OUT16(int iIndex, ushort shtData)
        {
            if (_sharedMemory == null)
            {
                IsError = true;
                return false;
            }

            if (iIndex < 1 || iIndex > InNum / 16)
                return false;
            try
            {
                Buf16 buf = new Buf16();
                for (int i = 0; i < 8; i++)
                {
                    if (((shtData >> i) & 1) == 1)
                    {
                        buf.bytData[i] = (byte)1;
                    }
                    else
                    {
                        buf.bytData[i] = (byte)0;
                    }
                }

                int iOffset = Server ? InNum : 0;
                _sharedMemory.WriteStructure(iOffset + (iIndex - 1) * 16, buf);
            }
            catch
            {
                IsError = true;
            }

            IsError = false;
            return true;
        }

#pragma warning disable 0649
        struct Buf32
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] bytData;
        }
#pragma warning restore 0649

        public override bool IN32(int iIndex, ref uint uiData)
        {
            if (_sharedMemory == null)
            {
                IsError = true;
                return false;
            }

            if (iIndex < 1 || iIndex > InNum / 32)
                return false;

            try
            {
                int iOffset = Server ? 0 : OutNum;
                Buf32 buf = (Buf32)_sharedMemory.ReadStructure(iOffset + (iIndex - 1) * 32, typeof(Buf32));

                uiData = 0;
                for (int i = 0; i < buf.bytData.Length; i++)
                {
                    if (buf.bytData[i] != 0)
                    {
                        uiData |= (uint)(1 << i);
                    }
                }
            }
            catch
            {
                IsError = true;
            }
            IsError = false;
            return true;
        }

        public override bool OUT32(int iIndex, uint uiData)
        {
            if (_sharedMemory == null)
            {
                IsError = true;
                return false;
            }

            if (iIndex < 1 || iIndex > InNum / 16)
                return false;

            Buf32 buf = new Buf32();
            for (int i = 0; i < 8; i++)
            {
                if (((uiData >> i) & 1) == 1)
                {
                    buf.bytData[i] = (byte)1;
                }
                else
                {
                    buf.bytData[i] = (byte)0;
                }
            }
            try
            {
                int iOffset = Server ? InNum : 0;
                _sharedMemory.WriteStructure(iOffset + (iIndex - 1) * 32, buf);
            }
            catch (Exception e)
            {
                IsError = true;
                System.Diagnostics.Trace.WriteLine(e.Message);
                return false;
            }
            IsError = false;
            return true;
        }


        public override bool Close()
        {
            if (_sharedMemory != null)
            {
                _sharedMemory.Close();
                _sharedMemory = null;
            }

            return base.Close();
        }
    }

    public class CommunicationDIOLogger : CommunicationDIO
    {
        private InspectionNameSpace.FormAutoInspInfo _fromAutoInspInfo = null;
        public override void SetFormAutoInspInfo(InspectionNameSpace.FormAutoInspInfo frm)
        {
            _fromAutoInspInfo = frm;
        }
        public CommunicationDIOLogger(string name, string jpnName)
            : base(name, jpnName)
        {

        }

        public override bool Open()
        {
            //            return LogingDllWrap.LogingDll.Loging_Init(Name, "");
            return true;
        }

        public override bool IN1(int iAddr, ref bool blnData)
        {
            if (_fromAutoInspInfo != null)
                _fromAutoInspInfo.GetInput1(iAddr, ref blnData);

            return true;
        }
        public override bool OUT1(int iAddr, bool blnData)
        {
            if (_fromAutoInspInfo != null)
                _fromAutoInspInfo.ChangeOutput1(iAddr, blnData);

            //System.Diagnostics.Debug.WriteLine("OUT01-" + "Address:" + iAddr.ToString("00") + " " + (blnData ? "1" : "0") + DateTime.Now.ToString("  yyyy/MM/dd HH:mm:ss.fff"));
            return LogingDllWrap.LogingDll.Loging_SetLogString("OUT01-" + "Address:" + iAddr.ToString("00") + " " + (blnData ? "1" : "0") + DateTime.Now.ToString("  yyyy/MM/dd HH:mm:ss.fff"));
        }

        public override bool OUT8(int iIndex, byte bytData)
        {
            return LogingDllWrap.LogingDll.Loging_SetLogString("OUT08-" + "Index  :" + iIndex.ToString("00") + " " + Convert.ToString(bytData, 2) + DateTime.Now.ToString("  yyyy/MM/dd HH:mm:ss.fff"));
        }

        public override bool OUT16(int iIndex, ushort shtData)
        {
            return LogingDllWrap.LogingDll.Loging_SetLogString("OUT16-" + "Index  :" + iIndex.ToString("00") + " " + Convert.ToString(shtData, 2) + DateTime.Now.ToString("  yyyy/MM/dd HH:mm:ss.fff"));
        }

        public override bool OUT32(int iIndex, uint uiData)
        {
            return LogingDllWrap.LogingDll.Loging_SetLogString("OUT32-" + "Index  :" + iIndex.ToString("00") + " " + Convert.ToString(uiData, 2) + DateTime.Now.ToString("  yyyy/MM/dd HH:mm:ss.fff"));
        }

        public override bool Close()
        {
            //        LogingDllWrap.LogingDll.Loging_End();
            return true;
        }

        public override bool Load(string sPath, string sSection)
        {
            IniFileAccess ifa = new IniFileAccess();

            InNum = ifa.GetIniInt(sSection, "InNum", sPath, 0);
            OutNum = ifa.GetIniInt(sSection, "OutNum", sPath, 0);

            return base.Load(sPath, sSection);
        }
    }
}
