using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HalconDotNet;
using HalconCamera;
using System.Reflection;

using Fujita.Misc;

namespace HalconCamera
{
    class CameraManager
    {
        private static CameraManager _instance = new CameraManager();
        public static CameraManager getInstance()
        {
            return _instance;
        }

        public bool Initialize()
        {
            HalconCameraBase.CloseAllFrameGrabber();
            return true;
        }

        List<HalconCameraBase> _lstCameras = new List<HalconCameraBase>();
        List<Dictionary<string, HTuple>> _lstCameraParams = new List<Dictionary<string, HTuple>>();

        public string LastErrorMessage { get; private set; }

        ILiveCommand _LiveCommand = null;
        public void SetLiveCommand(ILiveCommand lc)
        {
            _LiveCommand = lc;
        }

        public bool Save(string sPath, string sSection)
        {
            Path = sPath;
            Section = sSection;
#if FUJITA_INSPECTION_SYSTEM
            IniFileAccess ifa = new IniFileAccess();
            for (int i = 0; i < _lstCameras.Count; i++)
            {
                string sRealSection = "Camera" + (i + 1).ToString();
                _lstCameras[i].Save(sPath, sRealSection);
            }
#endif
            return true;
        }

        public int CameraNum
        {
            get
            {
                return _lstCameras.Count;
            }
        }

        public HalconCameraBase GetCamera(int iIndex)
        {
            if (iIndex < 0 || iIndex >= _lstCameras.Count)
                return null;
            return _lstCameras[iIndex];
        }

        public HalconCameraBase GetCamera(string name)
        {
            return _lstCameras.Find(x => x.Name == name);
        }

        public bool Load(string sPath, string sSection)
        {
            Path = sPath;
            Section = sSection;

            IniFileAccess ifa = new IniFileAccess();
            int iCameraNum = ifa.GetIniInt("global", "CameraNum", sPath, 0);
            for (int i = 1; i <= iCameraNum; i++)
            {
                string sRealSection = "Camera" + i.ToString();
                string sID = ifa.GetIni(sRealSection, "ID", "", sPath);
                string sName = ifa.GetIniString(sRealSection, "Name", sPath, "");
                string sType = ifa.GetIniString(sRealSection, "Type", sPath, "");
                string sDescription = ifa.GetIniString(sRealSection, "Description", sPath, "");
                string sMirrorType = ifa.GetIniString(sRealSection, "MirrorType", sPath, "");


                // すでに同名のカメラ名がある場合
                if (_lstCameras.Find(cam => cam.Name == sName) != null)
                {
                    TraceError("重複した名前を持つカメラが存在します。", MethodBase.GetCurrentMethod().Name);
                    LastErrorMessage = "重複した名前を持つカメラが存在します。";
                    return false;
                }

                // その他パラメーターを取得する
                Dictionary<string, string> paramSS;
                ifa.GetIniSection(sRealSection, sPath, out paramSS, false);
                Dictionary<string, HTuple> paramST = paramSS.StrToTuple();
                HalconCameraBase camera = null;

#if FUJITA_INSPECTION_SYSTEM
                if (AppData.getInstance().param.OfflineCameraMode)
                {
                    camera = new HalconCameraDummy(_lstCameraParams.Count, sName, sDescription);
                }
                else
                {
                    switch (sType.ToLower())
                    {
                        case "pylon_gige":
                        case "pylongige":
                        case "gige":
                            camera = new HalconCameraPylonGigE(_lstCameraParams.Count, sName, sDescription);
                            break;
                        case "pylon_gige_linesensor":
                        case "pylongige_linesensor":
                            camera = new HalconCameraPylonGigELineSensor(_lstCameraParams.Count, sName, sDescription);
                            break;

                    }
                }
#else
                camera = createFactory(sType, sName, sDescription, sMirrorType);
#endif
                //インスタンス生成チェック
                if (camera != null)
                {
#if FUJITA_INSPECTION_SYSTEM
                    // キャリブレーション値のロード
                    camera.Load(sPath, sRealSection);
#endif
                    camera.SetOpenParam(paramST);

                    //カメラをオープンする
                    if (camera.Open())
                    {
                        //成功した場合、リストに追加する
                        _lstCameras.Add(camera);
                        _lstCameraParams.Add(paramST);
                    }
                    else
                    {
                        TraceError(string.Format("カメラのオープンに失敗しました。[{0}]", sRealSection), MethodBase.GetCurrentMethod().Name);
                        LastErrorMessage = string.Format("カメラのオープンに失敗しました。[{0}]", sRealSection);
                        return false;
                    }
                }
                else
                {
                    TraceError(string.Format("該当するカメラが有りません。[{0}]", sRealSection), MethodBase.GetCurrentMethod().Name);
                    LastErrorMessage = string.Format("該当するカメラが有りません。[{0}]", sRealSection);
                    return false;

                }
            }

            List<HalconCameraBase> bufCam = new List<HalconCameraBase>();
            List<Dictionary<string, HTuple>> bufParam = new List<Dictionary<string, HTuple>>();
            for (int iBaseNo = 0; iBaseNo < _lstCameras.Count; iBaseNo++)
            {
                if (_lstCameras[iBaseNo].DicOpenParams.Keys.Contains("serialnumber"))
                {
                    for (int iCheckNo = 0; iCheckNo < _lstCameras.Count; iCheckNo++)
                    {
                        HTuple htSerialNumber;
                        HOperatorSet.GetFramegrabberParam(_lstCameras[iCheckNo].AcqHandle, "DeviceSerialNumber", out htSerialNumber);
                        HTuple htParam = _lstCameras[iBaseNo].DicOpenParams["serialnumber"];    //V1332b
                        string stSerial = (htParam.Type == HTupleType.STRING) ? htParam.S : htParam.ToString(); //V1332b                        
                        if (stSerial == htSerialNumber.S)                          //V1332b
                        {
                            bufCam.Add(_lstCameras[iCheckNo]);
                            bufParam.Add(_lstCameraParams[iCheckNo]);
                            break;
                        }
                    }
                }
            }
            if (bufCam.Count > 0)
            {
                _lstCameras.Clear();
                _lstCameraParams.Clear();
                for (int i = 0; i < bufCam.Count; i++)
                {
                    _lstCameras.Add(bufCam[i]);
                    _lstCameraParams.Add(bufParam[i]);
                }
            }

            // 同期モード選択
            _bSyncMode = ifa.GetIni("global", "SyncMode", true, sPath);

            return true;
        }

        private HalconCameraBase createFactory(string sType, string sName, string sDescription, string sMirrorType)
        {
            string sNameSpace = Assembly.GetExecutingAssembly().GetTypes().First(x => x.Name == GetType().Name).Namespace;
            HalconCameraBase camera = null;
            Type instanceType = null;
            switch (sType.ToLower())
            {
                case "saperalt_dalsaline":
                    instanceType = Type.GetType(sNameSpace + ".HalconCameraSaperaLTDALSALineSensor");
                    if (instanceType != null)
                        camera = (HalconCameraBase)Activator.CreateInstance(instanceType, new object[] { _lstCameraParams.Count, sName, sDescription, sMirrorType });
                    break;
                case "gigevision":
                    instanceType = Type.GetType(sNameSpace + ".HalconCameraGigEVision");
                    if (instanceType != null)
                        camera = (HalconCameraBase)Activator.CreateInstance(instanceType, new object[] { _lstCameraParams.Count, sName, sDescription, sMirrorType });
                    break;
                case "gigevision_hitachi":
                    instanceType = Type.GetType(sNameSpace + ".HalconCameraGigEVisionHitachi");
                    if (instanceType != null)
                        camera = (HalconCameraBase)Activator.CreateInstance(instanceType, new object[] { _lstCameraParams.Count, sName, sDescription, sMirrorType });
                    break;
                case "gigevision_nedlinesensor":
                    instanceType = Type.GetType(sNameSpace + ".HalconCameraGigEVisionNEDLineSensor");
                    if (instanceType != null)
                        camera = (HalconCameraBase)Activator.CreateInstance(instanceType, new object[] { _lstCameraParams.Count, sName, sDescription, sMirrorType });
                    break;
                case "pylon_gige":
                case "pylongige":
                case "gige":
                    instanceType = Type.GetType(sNameSpace + ".HalconCameraPylonGigE");
                    if (instanceType != null)
                        camera = (HalconCameraBase)Activator.CreateInstance(instanceType, new object[] { _lstCameraParams.Count, sName, sDescription, sMirrorType });
                    break;
                case "pylon_gige_linesensor":
                case "pylongige_linesensor":
                    instanceType = Type.GetType(sNameSpace + ".HalconCameraPylonGigELineSensor");
                    if (instanceType != null)
                        camera = (HalconCameraBase)Activator.CreateInstance(instanceType, new object[] { _lstCameraParams.Count, sName, sDescription, sMirrorType });
                    break;
                case "linx_ned_xcm4040sat2":
                    instanceType = Type.GetType(sNameSpace + ".HalconCameraLinX_NEDLineCameraXCM4040SAT2");
                    if (instanceType != null)
                        camera = (HalconCameraBase)Activator.CreateInstance(instanceType, new object[] { _lstCameraParams.Count, sName, sDescription, sMirrorType });
                    break;
                case "file":
                    instanceType = Type.GetType(sNameSpace + ".HalconCameraFile");
                    if (instanceType != null)
                        camera = (HalconCameraBase)Activator.CreateInstance(instanceType, new object[] { _lstCameraParams.Count, sName, sDescription, sMirrorType });
                    break;
                case "filememory":
                    instanceType = Type.GetType(sNameSpace + ".HalconCameraFileMemory");
                    if (instanceType != null)
                        camera = (HalconCameraBase)Activator.CreateInstance(instanceType, new object[] { _lstCameraParams.Count, sName, sDescription, sMirrorType });
                    break;

            }
            return camera;
        }

        bool _bSyncMode = false;
        public string Path { get; set; }
        public string Section { get; set; }

        /// <summary>
        /// エラーメッセージトレイサー
        /// </summary>
        /// <param name="sMessage">エラーメッセージ</param>
        /// <param name="sMethod">エラーが発生したメソッド</param>
        protected void TraceError(string sMessage, string sMethod)
        {
#if FUJITA_INSPECTION_SYSTEM
            Log.Write(this, sMessage + "-" + sMethod, AppData.getInstance().logger);
#else
            LogingDllWrap.LogingDll.Loging_SetLogString(sMessage + "-" + sMethod);
#endif
        }

        public bool HardTirgger(bool bTrig, HalconCameraBase.TriggerMode trg, int[] cams = null)
        {
            // キャプチャスレッドの停止
            for (int i = 0; i < CameraNum; i++)
            {
                if (cams == null || Array.IndexOf(cams, i) != -1)
                {
                    _lstCameras[i].StopGrab();
                }
            }

            // すべてをクローズ状態にする
            for (int i = 0; i < CameraNum; i++)
            {
                if (cams == null || Array.IndexOf(cams, i) != -1)
                {
                    _lstCameras[i].Close();
                }
            }

            // トリガモードを変更してオープンする
            for (int i = 0; i < CameraNum; i++)
            {
                if (cams == null || Array.IndexOf(cams, i) != -1)
                {
                    if (bTrig)
                    {
                        _lstCameras[i].TriggerModeOpen(trg);
                    }
                    else
                    {
                        _lstCameras[i].TriggerModeOpen(HalconCameraBase.TriggerMode.Software);
                    }
                }
            }

            // すべてをスタートさせる
            for (int i = 0; i < CameraNum; i++)
            {
                if (cams == null || Array.IndexOf(cams, i) != -1)
                {
                    _lstCameras[i].StartGrab();
                }
            }
            return true;
        }

        public bool Live(bool bLive, int[] cams = null)
        {
            if (_LiveCommand != null)
            {
                _LiveCommand.PrevCommand(bLive);
            }

            bool[] abResult = new bool[CameraNum];
            for (int i = 0; i < abResult.Length; i++)
                abResult[i] = true;

            if (_bSyncMode)
            {
                // キャプチャスレッドの停止
                for (int i = 0; i < CameraNum; i++)
                {
                    if (cams == null || Array.IndexOf(cams, i) != -1)
                    {
                        abResult[i] |= _lstCameras[i].StopGrab();
                    }
                }

                // すべてをクローズ状態にする
                for (int i = 0; i < CameraNum; i++)
                {
                    if (cams == null || Array.IndexOf(cams, i) != -1)
                    {
                        abResult[i] |= _lstCameras[i].Close();
                    }
                }
                // トリガモードを変更してオープンする
                for (int i = 0; i < CameraNum; i++)
                {
                    if (cams == null || Array.IndexOf(cams, i) != -1)
                    {
                        if (bLive)
                        {
                            abResult[i] |= _lstCameras[i].TriggerModeOpen(HalconCameraBase.TriggerMode.FreeRun);
                        }
                        else
                        {
                            abResult[i] |= _lstCameras[i].TriggerModeOpen(HalconCameraBase.TriggerMode.Software);
                        }
                    }
                }
                // すべてをスタートさせる
                for (int i = 0; i < CameraNum; i++)
                {
                    if (cams == null || Array.IndexOf(cams, i) != -1)
                    {
                        abResult[i] |= _lstCameras[i].StartGrab();
                    }
                }
            }
            else
            {
                for (int i = 0; i < CameraNum; i++)
                {
                    if (cams == null || Array.IndexOf(cams, i) != -1)
                    {
                        abResult[i] |= _lstCameras[i].Live(bLive);
                    }
                }
            }

            if (_LiveCommand != null)
            {
                _LiveCommand.AfterCommand(bLive);
            }

            bool bResult = true;
            for (int i = 0; i < abResult.Length; i++)
                bResult |= abResult[i];

            return bResult;
        }

        public bool Terminate()
        {
            for (int i = 0; i < _lstCameras.Count; i++)
            {
                _lstCameras[i].TerminatePolling();//v1328
                _lstCameras[i].Close();
                _lstCameras[i].Dispose();
                _lstCameras[i] = null;
            }
            _lstCameras.Clear();
            _lstCameraParams.Clear();

            HalconCameraBase.CloseAllFrameGrabber();

            return true;
        }
    }

    interface ILiveCommand
    {
        /// <summary>
        /// ライブ関数前に行うコマンド
        /// </summary>
        /// <param name="bLive">true: ライブ開始
        ///                     false: ライブ停止</param>
        void PrevCommand(bool bLive);
        /// <summary>
        /// ライブ関数後に行うコマンド
        /// </summary>
        /// <param name="bLive">true: ライブ開始
        ///                     false: ライブ停止</param>
        void AfterCommand(bool bLive);
    }

    internal static class QuoteStringExt
    {
        public static string ToDeQuote(this string s)
        {
            return s.Trim('"');
        }

        public static bool isString(this string s)
        {
            double resdouble;
            int resint;
            return (!double.TryParse(s, out resdouble) && !int.TryParse(s, out resint));
        }

        public static bool isReal(this string s)
        {
            double resdouble;
            int resint;
            return (double.TryParse(s, out resdouble) && !int.TryParse(s, out resint));
        }

        public static bool isNumber(this string s)
        {
            int resint;
            return (int.TryParse(s, out resint));
        }
    }

    internal static class DictionaryStrtoStrExt
    {
        public static Dictionary<string, HTuple> StrToTuple(this Dictionary<string, string> e)
        {
            Dictionary<string, HTuple> tmp = new Dictionary<string, HTuple>();
            foreach (KeyValuePair<string, string> k in e)
            {
                if (k.Value.isNumber())
                    tmp.Add(k.Key, new HTuple(int.Parse(k.Value)));
                else if (k.Value.isReal())
                    tmp.Add(k.Key, new HTuple(double.Parse(k.Value)));
                else
                    tmp.Add(k.Key, new HTuple(k.Value.ToDeQuote()));
            }
            return tmp;
        }
    }
}
