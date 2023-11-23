using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using Fujita.Misc;
using Fujita.Communication;

namespace Fujita.LightControl
{
    public class LightControlManager
    {
        private static LightControlManager _singleton = new LightControlManager(); // singleton patton

        public static LightControlManager getInstance()
        {
            return _singleton;
        }

        List<LightPowerSupplayBase> _lstLPS;
        List<LightType> _lstLT;

        public bool InitNetMonitorEnable { get; set; }
        /// <summary>
        /// 照明電源への設定値
        /// 接続タイムアウト時間
        /// </summary>
        public int ConnectTimeOut { get; set; }

                                                                                // khởi tạo list;

        public bool Initialize(bool errorcreate = false) // khởi tạo khai báo nếu chưa khai báo mới lightpowersupplyBase và LightType (singleton patton)
        {
            if (_lstLPS != null)
                return true;

            _lstLPS = new List<LightPowerSupplayBase>();
            _lstLT = new List<LightType>();

            _bErrorCreate = errorcreate;

            return true;
        }


        public string Path { get; set; }
        public string Section { get; set; }
        public string LastErrorMessage { get; private set; }


        private bool _bErrorCreate = false;

                                                                            // Hàm load danh sách các đèn và laoij đèn có sẵn;
        public bool Load(string sPath, string sSection) // load danh sách quản lý các đèn 
        {
            if (_lstLPS == null)
                return false;

            bool bRet = true;
            Path = sPath;
            Section = sSection;

            // * lấy ra số lượng và tên cảu từng thiết bị 

            IniFileAccess ifa = new IniFileAccess(); // lớp này để đọc dữ liệu số camera và số đèn mà lưu trong file
            int iLPSC = ifa.GetIniInt("global", "LightPowerSupplayCount", sPath, 0); // lấy ra số lược section name = "global" mà lưu trong INI     kiểu int => số lượng

            for (int i = 1; i <= iLPSC; i++)
            {
                string sRealSection = "LightPowerSupplay" + i.ToString(); //realSection tên ReslSection thứ i
                string sName = ifa.GetIniString(sRealSection, "Name", sPath, ""); // lấy ra tên mặc định cài đặt trong INI
                string sType = ifa.GetIniString(sRealSection, "Type", sPath, "");// lấy ra kiểu mặc định cài đặt trong INI
                int iChannelNum = ifa.GetIniInt(sRealSection, "ChannelNum", sPath, 0);// lấy ra giá trị mặc định trong INI

                string sDioName = ifa.GetIniString(sRealSection, "DioName", sPath, "");
                string sSioName = ifa.GetIniString(sRealSection, "SioName", sPath, "");
                string sNetName = ifa.GetIniString(sRealSection, "NetName", sPath, "");

                int iValueMin = ifa.GetIniInt(sRealSection, "ValueMin", sPath, 0);// lấy ra giá trị nhỏ nhất của Power Supply
                int iValueMax = ifa.GetIniInt(sRealSection, "ValueMax", sPath, 255);// lấy ra giá trị lớn nhất của Power Supply

                int iManageLightNum = ifa.GetIniInt(sRealSection, "ManageLightNum", sPath, 0);

                CommunicationDIO dio = CommunicationManager.getInstance().getCommunicationDIO(sDioName);
                CommunicationSIO sio = CommunicationManager.getInstance().getCommunicationSIO(sSioName);
                CommunicationNet net = CommunicationManager.getInstance().getCommunicationNet(sNetName);

                LightPowerSupplayBase lps = createLPSInstance(sType, dio, sio, net, sName, iChannelNum, iValueMin, iValueMax);// khởi tạo = new power supply tương tự khai báo lớp con IWDV hoặc IDGB với tên tương ứng
                if (lps == null)// khởi tạo không thành công
                {
                    bRet = false;
                    TraceError(sName + "-オブジェクト生成に失敗", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    LastErrorMessage = sName + "-オブジェクト生成に失敗";

                    if (_bErrorCreate) // lỗi khởi tạo thì đưa ra thông báo
                    {
                        lps = new LightPowerSupplayError(sName, sName + "-オブジェクト生成に失敗");
                    }
                    else
                    {
                        continue; // tiếp tục
                    }
                }
                lps.MonitorEnable = InitNetMonitorEnable;
                lps.ConnectTimeOut = ConnectTimeOut;

                if (!lps.Load(sPath, sRealSection))
                {
                    bRet = false;
                    TraceError(sName + "-ロードに失敗", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    LastErrorMessage = sName + "-ロードに失敗";
                    if (_bErrorCreate)
                    {
                        lps = new LightPowerSupplayError(sName, sName + "- ");
                    }
                    else
                    {
                        continue;
                    }
                }

                // ライトを作成する              lấy ra từng thông tin của LightPower supply (Thông tin chi tiết từng thiết bị)
                for (int n = 1; n <= iManageLightNum; n++)
                {
                    string sSection2 = sRealSection + "." + "LightType" + n.ToString();
                    string sLightName = ifa.GetIniString(sSection2, "Name", sPath, "");
                    string sLightColor = ifa.GetIniString(sSection2, "Color", sPath, "");
                    int iChannel = ifa.GetIniInt(sSection2, "Channel", sPath, 0);
                    string sLightingType = ifa.GetIniString(sSection2, "LightingType", sPath, "");

                    //list này chứa các CH sau khi tách được từ dữ liệu đọc được của file INI với từ khóa PareChannel
                    List<int> lstPareChannel = ifa.GetIni(sSection2, "PareChannel", "-1", sPath).Split(new char[] { ',' }).Select<string, int>(x => int.Parse(x)).ToList();

                    // add vào list LightType
                    LightType lt = new LightType(lps, _lstLT.Count, iChannel, lstPareChannel, sLightName, getColorNameToType(sLightColor));// khai báo gán giá trị cho bên Light Control đọc được và lưu lại để sử dụng
                    _lstLT.Add(lt);// bên light manager cũng lưu lại để quản lý
                    lps.setLight(iChannel, lt);// nếu có nhều đèn trong 1 thiết bị sẽ add số CH tương ứng vào thư viện
                }

                if (lps.Initialize() == false)
                {
                    bRet = false;
                    TraceError(sName + "-Initializeに失敗", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    LastErrorMessage = sName + "-Initializeに失敗";
                }
                _lstLPS.Add(lps); //tạo kết nối kiểu new LightControlIWDV()--------- list light power supply 
            }
            return bRet;
        }

        // tắt tất cả các đèn. dù đang tắt hay đang bật
        public void AllLightOff()
        {
            if (_lstLPS == null)
                return;

            for (int i = 0; i < _lstLT.Count; i++)
            {
                _lstLT[i].LightOff();
            }
        }

        //Tắt đèn hiện đang bật
        //chấm dứt hoạt động do lỗi nào đó
        public void Terminate()
        {
            if (_lstLT != null)
            {
                // 現在つきっぱなしのものを消灯させる Tắt đèn hiện đang bật
                foreach (LightType lt in _lstLT)
                    lt.LightOff();
                _lstLT.Clear();
                _lstLT = null;
            }
            if (_lstLPS != null)
            {
                foreach (LightPowerSupplayBase lps in _lstLPS)
                    lps.Terminate();                         // tắt 1 lần nữa????????????????????????? 
                _lstLPS.Clear();                            // xóa list
                _lstLPS = null;
            }
        }

        // set up màu để chuyển đồi cho chương trình chạy
        ELightColorType getColorNameToType(string sColorName)
        {
            switch (sColorName)
            {
                case "赤": return ELightColorType.Red;
                case "青": return ELightColorType.Blue;
                case "緑": return ELightColorType.Green;
                case "赤外": return ELightColorType.IR;
                case "紫外": return ELightColorType.UV;
                case "白": return ELightColorType.White;
            }
            return ELightColorType.Unknown;
        }

        public bool Save(string sPath, string sSection)
        {
            Path = sPath;
            return true;
        }
        // setup số lightCount số lượng đèn
        public int LightCount
        {
            get
            {
                return _lstLT.Count;
            }
        }

        public LightPowerSupplayBase GetPowerSupplay(int iLightIndex)
        {
            if (iLightIndex < 0 || iLightIndex >= _lstLT.Count)
                return null;

            return _lstLT[iLightIndex].GetSupplay();
        }

        // đưa ra loại đèn với index tương ứng truyền vào
        public LightType GetLight(int iIndex)
        {
            if (iIndex < 0 || iIndex >= _lstLT.Count)
                return null;
            return _lstLT[iIndex];
        }

        public LightType GetLight(string name)
        {
            return _lstLT.Find(o => o.Name == name);
        }
        public int LightPowerSupplyCount
        {
            get
            {
                return _lstLPS.Count;
            }
        }
        public LightPowerSupplayBase GetLightPowerSupplay(int iIndex)
        {
            if (iIndex < 0 || iIndex >= _lstLPS.Count)
                return null;
            return _lstLPS[iIndex];
        }

        // khai báo light. tương tự như lightmanager _light = new   lightIDGB();
        private LightPowerSupplayBase createLPSInstance(string sType, CommunicationDIO dio, CommunicationSIO sio, CommunicationNet net, string name, int num, int min, int max)
        {
            // namespace 取得
            string sNameSpace = Assembly.GetExecutingAssembly().GetTypes().First(x => x.Name == GetType().Name).Namespace;

            LightPowerSupplayBase lps = null;
            Type instanceType = null;
            switch (sType.ToLower())
            {
                case "ccs_8bit_1ch":
                    instanceType = Type.GetType(sNameSpace + ".LightPowerSupplay_CCS_8bit_1ch");
                    if (instanceType != null)
                        lps = (LightPowerSupplayBase)Activator.CreateInstance(instanceType, new object[] { dio, name });
                    break;
                case "ccs_8bit_2ch":
                    instanceType = Type.GetType(sNameSpace + ".LightPowerSupplay_CCS_8bit_2ch");
                    if (instanceType != null)
                        lps = (LightPowerSupplayBase)Activator.CreateInstance(instanceType, new object[] { dio, name });
                    break;
                case "ccs_8bit_4ch":
                    instanceType = Type.GetType(sNameSpace + ".LightPowerSupplay_CCS_8bit_4ch");
                    if (instanceType != null)
                        lps = (LightPowerSupplayBase)Activator.CreateInstance(instanceType, new object[] { dio, name });
                    break;
                case "ccs_8bit_8ch":
                    instanceType = Type.GetType(sNameSpace + ".LightPowerSupplay_CCS_8bit_8ch");
                    if (instanceType != null)
                        lps = (LightPowerSupplayBase)Activator.CreateInstance(instanceType, new object[] { dio, name });
                    break;
                case "moritex_8bit_1ch":
                    instanceType = Type.GetType(sNameSpace + ".LightPowerSupplay_Moritex_8bit_1ch");
                    if (instanceType != null)
                        lps = (LightPowerSupplayBase)Activator.CreateInstance(instanceType, new object[] { dio, name });
                    break;
                case "moritex_8bit_4ch":
                    instanceType = Type.GetType(sNameSpace + ".LightPowerSupplay_Moritex_8bit_4ch");
                    if (instanceType != null)
                        lps = (LightPowerSupplayBase)Activator.CreateInstance(instanceType, new object[] { dio, name });
                    break;
                case "optex_10bit_nch":
                    instanceType = Type.GetType(sNameSpace + ".LightPowerSupplay_Optex_Oppf_10bit_Nch");
                    if (instanceType != null)
                        lps = (LightPowerSupplayBase)Activator.CreateInstance(instanceType, new object[] { dio, sio, name });
                    break;
                case "omron_plc":
                    instanceType = Type.GetType(sNameSpace + ".LightPowerSupplay_Omron_PLC");
                    if (instanceType != null)
                        lps = (LightPowerSupplayBase)Activator.CreateInstance(instanceType, new object[] { sio, name });
                    break;
                case "imac_ibc":
                    instanceType = Type.GetType(sNameSpace + ".LightPowerSupplay_IMAC_IBC");
                    if (instanceType != null)
                        lps = (LightPowerSupplayBase)Activator.CreateInstance(instanceType, new object[] { dio, name });
                    break;
                case "imac_iwdv_300s_24":
                    instanceType = Type.GetType(sNameSpace + ".LightPowerSupplay_IMAC_IWDV_300S_24");
                    if (instanceType != null)
                        lps = (LightPowerSupplayBase)Activator.CreateInstance(instanceType, new object[] { net, name });
                    break;
                case "imac_iwdv_600m2_24":
                    instanceType = Type.GetType(sNameSpace + ".LightPowerSupplay_IMAC_IWDV_600M2_24");
                    if (instanceType != null)
                        lps = (LightPowerSupplayBase)Activator.CreateInstance(instanceType, new object[] { net, name });
                    break;
                case "imac_idgb":
                    instanceType = Type.GetType(sNameSpace + ".LightPowerSupplay_IMAC_IDGB");
                    if (instanceType != null)
                        lps = (LightPowerSupplayBase)Activator.CreateInstance(instanceType, new object[] { net, name, num });
                    break;
                case "kyotodenkiki_lda":
                    instanceType = Type.GetType(sNameSpace + ".LightPowerSupplay_KYOTO_LDA");
                    if (instanceType != null)
                        lps = (LightPowerSupplayBase)Activator.CreateInstance(instanceType, new object[] { net, name, num });
                    break;
                case "dummy":
                    instanceType = Type.GetType(sNameSpace + ".LightPowerSupplayDummy");
                    if (instanceType != null)
                        lps = (LightPowerSupplayBase)Activator.CreateInstance(instanceType, new object[] { null, null, null, name, "dummy_vender", num, min, max, false });
                    break;

            }
            return lps;
        }

        private void TraceError(string sMessage, string sMethodName)
        {
            LogingDllWrap.LogingDll.Loging_SetLogString(string.Format("{0}-{1}", sMessage, sMethodName));
        }
    }
}
