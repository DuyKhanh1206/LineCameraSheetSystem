#define SINGLETON_OBJECT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Fujita.Misc;

namespace Fujita.Communication
{
    public class CommunicationManager
    {
        public string Path { get; set; }
        public string Section { get; set; }
        public string LastErrorMessage { get; private set; }
#if SINGLETON_OBJECT
        private static CommunicationManager _singleton = new CommunicationManager();
        public static CommunicationManager getInstance()
        {
            return _singleton;
        }
        private CommunicationManager()
        {
        }
#else
        public CommunicationManager()
        {
        }
#endif

        public bool Save(string sPath, string sSection)
        {
            if (_lstCommunication == null)
                return false;
            return false;
        }

        bool _bErrorCreate = false;
        public bool Initialize(bool errorcreate = false)
        {
            if (_lstCommunication != null)
                return true;

            _lstCommunication = new List<CommunicationBase>();

            _bErrorCreate = errorcreate;

            return true;
        }

        public bool Load(string sPath, string sSection)
        {
            if (_lstCommunication == null)
                return false;
            Path = sPath;
            bool bRet = true;
            IniFileAccess ifa = new IniFileAccess();

            int iCommNum = ifa.GetIniInt("global", "CommunicationNum", sPath, 0);

            for (int i = 1; i <= iCommNum; i++)
            {
                string sRealSection = "Communication" + i.ToString();
                string sType = ifa.GetIniString(sRealSection, "Type", sPath, "");
                string sName = ifa.GetIniString(sRealSection, "Name", sPath, "");
                string sJpnName = ifa.GetIniString(sRealSection, "JpnName", sPath, "");
                if (sJpnName == "")
                    sJpnName = sName;

                bool bControl = ifa.GetIniInt(sRealSection, "IsControl", sPath, 0) == 0 ? false : true;
                bool bLightControl = ifa.GetIniInt(sRealSection, "IsLightControl", sPath, 0) == 0 ? false : true;

                object[] oaParams;
                ifa.GetIniArray(sRealSection, "Param", sPath, out oaParams);

                if (sType == "" | sName == "" | isUniqName(sName))
                {
                    System.Diagnostics.Debug.WriteLine("不正な設定です");
                    bRet = false;
                    continue;
                }
                CommunicationBase obj = createFactory(sType, sName, sJpnName, oaParams);        // khởi tạo  các DIO control
                if (obj == null)
                {
                    bRet = false;
                    TraceError(sName + "-生成に失敗", MethodBase.GetCurrentMethod().ToString());
                    if (_bErrorCreate)
                    {
                        _lstCommunication.Add(new CommunicationError(sName, sJpnName, sName + "-生成に失敗"));
                    }
                    continue;
                }

                if (!obj.Load(sPath, sRealSection))     // load thông tin cho từng DIO
                {
                    bRet = false;
                    TraceError(sName + "-不正な設定", MethodBase.GetCurrentMethod().ToString());
                    if (_bErrorCreate)
                    {
                        _lstCommunication.Add(new CommunicationError(sName, sJpnName, sName + "-不正な設定"));
                    }
                    continue;
                }

                obj.setControls(bControl, bLightControl);       //gán giá trị cho từng communicationBase

                if (!obj.Open())        //kết nối tới  DIO
                {
                    bRet = false;
                    TraceError(sName + "-オープンに失敗", MethodBase.GetCurrentMethod().ToString());
                    if (_bErrorCreate)
                    {
                        _lstCommunication.Add(new CommunicationError(sName, sJpnName, sName + "-オープンに失敗"));
                    }
                    continue;
                }
                _lstCommunication.Add(obj);
            }
            return bRet;
        }

        public CommunicationDIO getCommunicationDIO(string sName = "")
        {
            if (_lstCommunication == null || _lstCommunication.Count == 0)
                return null;
            try
            {
                if (sName != "")
                    return (CommunicationDIO)_lstCommunication.Find(o => o.Name == sName && o.Type == ECommunicationType.DIO);
                else
                    return (CommunicationDIO)_lstCommunication.Find(o => o.Type == ECommunicationType.DIO);
            }
            catch (Exception e)
            {
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }

        public CommunicationDIO getCommunicationDIOControl()
        {
            if (_lstCommunication == null || _lstCommunication.Count == 0)
                return null;
            try
            {
                return (CommunicationDIO)_lstCommunication.Find(o => o.IsControl && o.Type == ECommunicationType.DIO);
            }
            catch (Exception e)
            {
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }

        public CommunicationDIO getCommunicationDIOLightControl()
        {
            if (_lstCommunication == null || _lstCommunication.Count == 0)
                return null;
            try
            {
                return (CommunicationDIO)_lstCommunication.Find(o => o.IsLightControl && o.Type == ECommunicationType.DIO);
            }
            catch (Exception e)
            {
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }

        public CommunicationSIO getCommunicationSIO(string sName = "")
        {
            if (_lstCommunication == null || _lstCommunication.Count == 0)
                return null;
            try
            {
                if (sName != "")
                    return (CommunicationSIO)_lstCommunication.Find(o => o.Name == sName && o.Type == ECommunicationType.SIO);
                else
                    return (CommunicationSIO)_lstCommunication.Find(o => o.Type == ECommunicationType.SIO);
            }
            catch (Exception e)
            {
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }

        public CommunicationSIO getCommunicationSIOControl()
        {
            if (_lstCommunication == null || _lstCommunication.Count == 0)
                return null;

            try
            {
                return (CommunicationSIO)_lstCommunication.Find(o => o.IsControl && o.Type == ECommunicationType.SIO);
            }
            catch (Exception e)
            {
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }

        public CommunicationSIO getCommunicationSIOLightControl()
        {
            if (_lstCommunication == null || _lstCommunication.Count == 0)
                return null;

            try
            {
                return (CommunicationSIO)_lstCommunication.Find(o => o.IsLightControl && o.Type == ECommunicationType.SIO);
            }
            catch (Exception e)
            {
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }

        public CommunicationNet getCommunicationNet(string sName)
        {
            if (_lstCommunication == null || _lstCommunication.Count == 0)
                return null;
            try
            {
                return (CommunicationNet)_lstCommunication.Find(o => o.Name == sName && o.Type == ECommunicationType.Net);
            }
            catch (Exception e)
            {
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }

        public CommunicationNet getCommunicationNetControl()
        {
            if (_lstCommunication == null || _lstCommunication.Count == 0)
                return null;

            try
            {
                return (CommunicationNet)_lstCommunication.Find(o => o.IsControl && o.Type == ECommunicationType.Net);
            }
            catch (Exception e)
            {
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }

        public CommunicationNet getCommunicationNetLightControl()
        {
            if (_lstCommunication == null || _lstCommunication.Count == 0)
                return null;

            try
            {
                return (CommunicationNet)_lstCommunication.Find(o => o.IsLightControl && o.Type == ECommunicationType.Net);
            }
            catch (Exception e)
            {
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
                return null;
            }
        }

        private bool isUniqName(string name)
        {
            return (_lstCommunication.Count(o => o.Name == name) >= 1);
        }

        public void Terminate()
        {
            if (_lstCommunication == null)
                return;

            _lstCommunication.ForEach(o => o.Close());
            _lstCommunication.Clear();

            _lstCommunication = null;
        }

        private CommunicationBase createFactory(string sType, string name, string jpnName, object[] param)
        {
            string sNameSpace = Assembly.GetExecutingAssembly().GetTypes().First(x => x.Name == GetType().Name).Namespace;

            Type instanceType = null;
            CommunicationBase comm = null;
            switch (sType.ToLower())
            {
                case "dio":
                case "dio_interface":
                    instanceType = Type.GetType(sNameSpace + ".CommunicationDIOInterface");
                    break;
                case "dio_sharedmemory":
                    instanceType = Type.GetType(sNameSpace + ".CommunicationDIOSharedMemory");
                    break;
                case "dio_logger":
                    instanceType = Type.GetType(sNameSpace + ".CommunicationDIOLogger");
                    break;


                case "sio_plc_omron":
                    instanceType = Type.GetType(sNameSpace + ".CommunicationSIOPLCOmron");
                    break;                
                case "sio":
                    instanceType = Type.GetType(sNameSpace + ".CommunicationSIO");
                    break;


                case "ethernet":


                case "net":
                    instanceType = Type.GetType(sNameSpace + ".CommunicationNet");
                    break;
              
            }
            if (instanceType != null)
                comm = (CommunicationBase)Activator.CreateInstance(instanceType, new object[] { name, jpnName });
            return comm;
        }

        List<CommunicationBase> _lstCommunication;
        private void TraceError(string sMes, string sMethod)
        {
            LastErrorMessage = sMes;
            LogingDllWrap.LogingDll.Loging_SetLogString(string.Format("{0}-{1}", sMes, sMethod));
        }
    }
}
