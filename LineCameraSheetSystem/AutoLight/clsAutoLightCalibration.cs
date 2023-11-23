using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if !STANDALONE
using InspectionNameSpace;
#else
using HalconDotNet;
#endif

using Fujita.LightControl;
using HalconCamera;

using System.Windows.Forms;

namespace LineCameraSheetSystem
{
    public class clsAutoLightCalibration
    {
        public class CamExposure
        {
            public CamExposure()
            {
            }

            public CamExposure(int iVal1, int iVal2)
            {
                Val1 = iVal1;
                Val2 = iVal2;
            }
            public int Val1;
            public int Val2;
        }
        List<clsAutoLightValueCalibration> _lstCalib = new List<clsAutoLightValueCalibration>();

        List<int> _lstResult = new List<int>();
        List<List<int>> _llstExposure = new List<List<int>>();
        public enum ELightingType
        {
            Trans,
            Reflec,
            Trans_Reflec,
        }

        public ELightingType LightType { get; set; }
#if !STANDALONE
        AutoInspection _auto = null;
        public bool Initialize(AutoInspection auto)
        {
            if (_auto != null)
                return false;

            AutoLightWaitFrameCount = SystemParam.GetInstance().AutoLightCheckImageCount;
            AutoLightOkFrameCount = SystemParam.GetInstance().AutoLightOkImageCount;
            AutoLightGainUpStep = SystemParam.GetInstance().AutoLightGainUpStep;
            AutoLightGainMaxCount = SystemParam.GetInstance().AutoLightGainMaxCount;
            AutoLightOkLowLimit = SystemParam.GetInstance().AutoLightOkLowLimit;
            AutoLightOkHightLimit = SystemParam.GetInstance().AutoLightOkHighLimit;
            AutoLightDetailUpLevel = SystemParam.GetInstance().AutoLightDetailUpLevel;
            _auto = auto;

            return true;
        }

        public bool Terminate()
        {
            CalibrationCancel();//v1338 自動調光をキャンセルする

            _auto = null;
            return true;
        }

#else
        public bool Initialize()
        {
            AutoLightWaitFrameCount = 3;
            return true;
        }

        public bool Terminate()
        {
            return true;
        }
#endif
        public int AutoLightWaitFrameCount { get; set; }
        public int AutoLightOkFrameCount { get; set; }
        public double AutoLightGainUpStep { get; set; }
        public int AutoLightGainMaxCount { get; set; }
        public int AutoLightOkLowLimit { get; set; }
        public int AutoLightOkHightLimit { get; set; }
        public int AutoLightDetailUpLevel { get; set; }
        public bool AutoLightFull { get; set; }

        int _iBaseLightValue = 0;
        public int BaseLightValue
        {
            get
            {
                return _iBaseLightValue;
            }
            set
            {
                if (value >= 0 && _iBaseLightValue <= 255)
                    _iBaseLightValue = value;
            }
        }

#if false
        public static int[] GetLeds(clsAutoLightValueCalibration.ECameraSide side ,Recipe.InspTypes type, List<Tuple<LedNum,bool>> lstLedEnable = null)
        {
            LedInfo[] info = null;
            //SystemParam.GetInstance().GetInspectionLed(type, out info);
            if (info == null)
            {
                LogingDllWrap.LogingDll.Loging_SetLogString("GetLeds():LEDINFOがNULL");
                return new int[0];
            }

            AppData.SideID sideID = side == clsAutoLightValueCalibration.ECameraSide.UpSide ? AppData.SideID.表 : AppData.SideID.裏;
            List<int> lstLedNum = new List<int>();
            for (int i = 0; i < info.Length; i++)
            {
                if (info[i].LedParts == sideID)
                {
                    if (lstLedEnable != null)
                    {
                        for (int n = 0; n < lstLedEnable.Count; n++)
                        {
                            if (info[i].LedID == lstLedEnable[n].Item1 && lstLedEnable[n].Item2 )
                            {
                                lstLedNum.Add((int)info[i].LedID);
                                break;
                            }
                        }
                    }
                    else
                    {
                        lstLedNum.Add((int)info[i].LedID);
                    }
                }
            }
            return lstLedNum.ToArray();
        }
#endif

#if true
        //public static int[] GetCameras(clsAutoLightValueCalibration.ECameraSide side, Recipe.InspTypes type)
        //{
        //    List<CameraParam> campar = null; ;
        //    SystemParam.GetInstance().GetInspectionCmera(type, out campar);
        //    if (campar == null)
        //    {
        //        LogingDllWrap.LogingDll.Loging_SetLogString("GetCameras():CAMERA PARAMがNULL");
        //        return null;
        //    }

        //    AppData.SideID sideID = side == clsAutoLightValueCalibration.ECameraSide.UpSide ? AppData.SideID.表 : AppData.SideID.裏;
        //    List<int> lstCams = new List<int>();
        //    for (int i = 0; i < campar.Count; i++)
        //    {
        //        if (campar[i].CamParts == sideID)
        //        {
        //            lstCams.Add((int)campar[i].CamID);

        //        }
        //    }
        //    return lstCams.ToArray();
        //}

        //private bool addCalibration(clsAutoLightValueCalibration.ECameraSide side, Recipe.InspTypes type, List<Tuple<LedNum,bool>> lstLedEnable = null )
        private bool addCalibration(clsAutoLightValueCalibration.ECameraSide side, Recipe.InspTypes type, List<int> lstLedEnable = null)
        {
            _lstCalib.Add(new clsAutoLightValueCalibration());
            int iLastIndex = _lstCalib.Count - 1;
            //透過検査で使用するカメラの数を取得する
            //int[] aiLedNum = GetLeds(side, type, lstLedEnable);
            int[] aiLedNum = lstLedEnable.ToArray();
            if (aiLedNum.Length == 0)
            {
                LogingDllWrap.LogingDll.Loging_SetLogString("addCalibration():該当する照明が見つかりません");
                return false;
            }

            for (int i = 0; i < aiLedNum.Length; i++)
            {
                LightType lt = LightControlManager.getInstance().GetLight(aiLedNum[i]);
                if (lt == null)
                {
                    LogingDllWrap.LogingDll.Loging_SetLogString("addCalibration():照明が見つかりません");
                    return false;
                }
                _lstCalib[iLastIndex].AddLight(lt);
            }

            _lstCalib[iLastIndex].CameraSide = side;
            _lstCalib[iLastIndex].OnEvent += new clsAutoLightValueCalibration.LightValueEventHandler(clsAutoLightCalibration_OnEvent);
            _lstCalib[iLastIndex].OnProgress += new clsAutoLightValueCalibration.LightValueProgressEventHandler(clsAutoLightCalibration_OnProgress);
            _lstCalib[iLastIndex].OnDebugMessage += ClsAutoLightCalibration_OnDebugMessage;
            _lstCalib[iLastIndex].WaitCaptureCount = AutoLightWaitFrameCount;
            _lstCalib[iLastIndex].OkCaptureCount = AutoLightOkFrameCount;
            _lstCalib[iLastIndex].GainUpStep = AutoLightGainUpStep;
            _lstCalib[iLastIndex].GainMaxCount = AutoLightGainMaxCount;
            _lstCalib[iLastIndex].OkLowLimit = AutoLightOkLowLimit;
            _lstCalib[iLastIndex].OkHightLimit = AutoLightOkHightLimit;
            _lstCalib[iLastIndex].DetailupLevel = AutoLightDetailUpLevel;

            _lstCalib[iLastIndex].AutoLightFull = AutoLightFull;
            _lstCalib[iLastIndex].BaseLightValue = BaseLightValue;
            _lstCalib[iLastIndex].AddCamera(CameraManager.getInstance().GetCamera((int)side));

            // 系統ごとに結果を入れる
            _lstResult.Add(-1);

            //if (AutoLightFull)
            //{
            //    // 露光結果を入れる
            //    _llstExposure.Add(new List<int>());
            //    int[] aiCams = GetCameras(side, type);
            //    if (aiCams == null)
            //    {
            //        return false;
            //    }
            //    for (int i = 0; i < aiCams.Length; i++)
            //    {
            //        HalconCameraBase cam = CameraManager.getInstance().GetCamera(aiCams[i]);
            //        if (cam == null)
            //        {
            //            LogingDllWrap.LogingDll.Loging_SetLogString("addCalibration():該当するカメラが見つかりません");
            //            return false;
            //        }
            //        _lstCalib[iLastIndex].AddCamera(cam);
            //    }
            //}

            return true;
        }

        private void ClsAutoLightCalibration_OnDebugMessage(object sender, clsAutoLightValueCalibration.DebugMessageEventArgs e)
        {
            _frmAutoLightValueCalibration.AddDebugMessage(e.eCameraSide, e.strMessage);
        }
#endif
        private void formatResult(out List<int> lstResult, out List<double> lstCamGain, out List<List<CamExposure>> llExposure)
        {
            lstResult = new List<int>();
            lstCamGain = new List<double>();
            llExposure = new List<List<CamExposure>>();
            for (int i = 0; i < _lstCalib.Count; i++)
            {
                lstResult.Add(_lstCalib[i].ResultLightValue);
                lstCamGain.Add(_lstCalib[i].ResultCameraGain);
                llExposure.Add(new List<CamExposure>());
            }

            for (int i = 0; i < _lstCalib.Count; i++)
            {
                for (int n = 0; n < _lstCalib[i].ResultCameraExposures.Count; n++)
                {
                    if (_lstCalib[i].AutoLightFull)
                    {
                        int iVal1, iVal2;
                        HalconCamera.HalconCameraLinX_NEDLineCameraXCM4040SAT2.FindExposure(_lstCalib[i].ResultCameraExposures[n], out iVal1, out iVal2);
                        llExposure[i].Add(new CamExposure(iVal1, iVal2));
                    }
                    else
                    {
                        llExposure[i].Add(new CamExposure(0, 0));
                    }
                }
            }
        }

        //public bool Start(ELightingType lightType,  List<Tuple<LedNum,bool>> lstLedEnable, out bool bResult, out List<int> lstResultData, out List<List<CamExposure>> llstResultExposure)
        public bool Start(ELightingType lightType, List<int> lstUpSideLedEnable, List<int> lstDownSideLedEnable, out bool bResult, out List<int> lstResultData, out List<double> lstCamGain, out List<List<CamExposure>> llstResultExposure)
        {
            lstResultData = null;
            lstCamGain = null;
            llstResultExposure = null;
            bResult = false;

            if (_auto == null)
            {

                return false;
            }

            if (_frmAutoLightValueCalibration != null)
            {
                return false;
            }

            LightType = lightType;

            if (LightType == ELightingType.Trans)
            {
                if (!addCalibration(clsAutoLightValueCalibration.ECameraSide.UpSide, Recipe.InspTypes.透過, lstUpSideLedEnable))
                {
                    return false;
                }
            }
            else if (LightType == ELightingType.Reflec)
            {
                // 表側
                if (!addCalibration(clsAutoLightValueCalibration.ECameraSide.UpSide, Recipe.InspTypes.反射, lstUpSideLedEnable))
                {
                    return false;
                }

                // 裏側
                if (!addCalibration(clsAutoLightValueCalibration.ECameraSide.DownSide, Recipe.InspTypes.反射, lstDownSideLedEnable))
                {
                    return false;
                }
            }
            else if (LightType == ELightingType.Trans_Reflec)
            {
                //// 表側
                //if (!addCalibration(clsAutoLightValueCalibration.ECameraSide.UpSide, Recipe.InspTypes.透過, lstLedEnable))
                //{
                //    return false;
                //}

                //// 裏側
                //if (!addCalibration(clsAutoLightValueCalibration.ECameraSide.DownSide, Recipe.InspTypes.反射, lstLedEnable))
                //{
                //    return false;
                //}
            }

            _frmAutoLightValueCalibration = new frmAutoLightValueCalibration();
            _frmAutoLightValueCalibration.SetAutoLightValueCalibration(this);
            _frmAutoLightValueCalibration.ProgressCapacity(_lstCalib.Count);

            _frmAutoLightValueCalibration.Shown += new EventHandler(_frmAutoLightValueCalibration_Shown);

            // 自動調光値中ダイアログ表示
            if (DialogResult.Yes == _frmAutoLightValueCalibration.ShowDialog())
            {
                lstResultData = new List<int>();
                lstCamGain = new List<double>();
                formatResult(out lstResultData, out lstCamGain, out llstResultExposure);
                bResult = true;
            }

            _auto.ClearRefreshImageEvent(OnRefreshImage);

            for (int i = 0; i < _lstCalib.Count; i++)
            {
                _lstCalib[i].ClearLight();
            }

            if (AutoLightFull)
            {
                for (int i = 0; i < _lstCalib.Count; i++)
                {
                    _lstCalib[i].ClearCamera();
                }
            }

            return true;
        }

        frmAutoLightValueCalibration _frmAutoLightValueCalibration = null;
        void _frmAutoLightValueCalibration_Shown(object sender, EventArgs e)
        {
            //            _auto.ClearRefreshImageEvent();
#if !STANDALONE
            _auto.SetRefreshImageEvent(OnRefreshImage);
#else
//            HalconCamera.CameraManager.getInstance().GetCamera(0).OnGrabbedImage += new GrabbedImageEventHandler(clsAutoLightCalibration_OnGrabbedImage);
            APCameraManager.getInstance().SetGrabbedImageEvent(0, clsAutoLightCalibration_OnGrabbedImage);
#endif
            // フォームが表示されたところで、検査をスタートする
            for (int i = 0; i < _lstCalib.Count; i++)
                _lstCalib[i].Start();
        }

#if !STANDALONE
        void OnRefreshImage(object sender, RefreshImageEvent.RefreshImageEventArgs e)
        {
            // 両方に送る
            for (int i = 0; i < _lstCalib.Count; i++)
                _lstCalib[i].OnRefreshImage(sender, e);
        }
#else
        int _iRow1 = 0, _iCol1 = 0, _iRow2 = 100, _iCol2 = 100;
        public void SetInspectionROI(int iRow1, int iCol1, int iRow2, int iCol2)
        {
            _iRow1 = iRow1;
            _iCol1 = iCol1;
            _iRow2 = iRow2;
            _iCol2 = iCol2;
        }

        void clsAutoLightCalibration_OnGrabbedImage(object sender, GrabbedImageEventArgs e)
        {
            HTuple htWidth, htHeight;
            HTuple htMean, htDeviation;
            HTuple htMin, htMax, htRange;
            HObject hoRegionRectangle = null;
            try
            {
                HOperatorSet.GetImageSize(e.Image, out htWidth, out htHeight);
                HOperatorSet.GenRectangle1(out hoRegionRectangle, _iRow1, _iCol1, _iRow2, _iCol2);

                HOperatorSet.Intensity(hoRegionRectangle, e.Image, out htMean, out htDeviation);
                HOperatorSet.MinMaxGray(hoRegionRectangle, e.Image, 0, out htMin, out htMax, out htRange);
                for (int i = 0; i < _lstCalib.Count; i++)
                {
                    _lstCalib[i].OnRefreshImage(sender, htMean.D);
                }
            }
            catch (HOperatorException)
            {
            }
            finally
            {
                if (hoRegionRectangle != null)
                    hoRegionRectangle.Dispose();
            }

        }
#endif
        public bool Stop()
        {
            // ユーザーによるキャンセル動作
            for (int i = 0; i < _lstCalib.Count; i++)
                _lstCalib[i].Stop();

            return true;
        }


        void clsAutoLightCalibration_OnProgress(object sender, clsAutoLightValueCalibration.LightValueProgressEventArgs e)
        {
            if (_frmAutoLightValueCalibration == null)
                return;

            _frmAutoLightValueCalibration.UpdateProgress(e.Upto);
        }

        string makeCameraMessage(ELightingType lt, clsAutoLightValueCalibration.ECameraSide side)
        {
            //            int[] aiCams = SystemParam.GetInstance().Get
            int[] aiCams;
            if (side == clsAutoLightValueCalibration.ECameraSide.UpSide)
            {
                aiCams = new int[] { 0, 1 };
            }
            else
            {
                aiCams = new int[] { 2, 3 };
            }

            string sRet = "";
            if (lt == ELightingType.Trans)
            {
                sRet = "    透過(カメラ";
            }
            else if (lt == ELightingType.Reflec)
            {
                sRet = "    反射(カメラ";
            }
            else
            {
                if (side == clsAutoLightValueCalibration.ECameraSide.UpSide)
                {
                    sRet = "    透過(カメラ";
                }
                else
                {
                    sRet = "    反射(カメラ";
                }
            }

            for (int i = 0; i < aiCams.Length; i++)
            {
                if (i != 0)
                    sRet += "-";
                sRet += (aiCams[i] + 1).ToString();
            }

            sRet += ")";

            return sRet;
        }

        string makeCameraMessage(ELightingType lt, clsAutoLightValueCalibration calib)
        {
            string sRet = "";

            if (lt == ELightingType.Trans)
            {
                sRet = "    透過(カメラ";
            }
            else if (lt == ELightingType.Reflec)
            {
                sRet = "    反射(カメラ";
            }
            else if (lt == ELightingType.Trans_Reflec)
            {
                if (calib.CameraSide == clsAutoLightValueCalibration.ECameraSide.UpSide)
                {
                    sRet = "    透過(カメラ";
                }
                else
                {
                    sRet = "    反射(カメラ";
                }
            }

            for (int i = 0; i < calib.ListCameras.Count; i++)
            {
                if (i != 0)
                    sRet += "-";
                sRet += (calib.ListCameras[i].Index + 1).ToString();
            }

            sRet += ")";

            return sRet;
        }


        string queryUpdateMessage(ELightingType lightType, List<int> lstResult, List<List<int>> llexp)
        {
            string sValueMessage = "";
            //            int iResultIndex = 0;

            for (int i = 0; i < _lstCalib.Count; i++)
            {
                if (!_lstCalib[i].AutoLightFull)
                    sValueMessage += makeCameraMessage(lightType, _lstCalib[i].CameraSide);
                else
                    sValueMessage += makeCameraMessage(lightType, _lstCalib[i]);
                sValueMessage += ":" + _lstResult[i].ToString() + "\n";

                if (_lstCalib[i].AutoLightFull)
                {
                    if (llexp == null || llexp.Count == 0)
                    {
                        sValueMessage = "内部ｴﾗｰ:queryUpdateMessage():露光値リストがNullか0です";
                        LogingDllWrap.LogingDll.Loging_SetLogString("queryUpdateMessage():露光値リストがNullか0です");
                        break;
                    }

                    //                    sValueMessage += ":" + _lstResult[i].ToString() + "\n";
                    for (int n = 0; n < _lstCalib[i].ResultCameraExposures.Count; n++)
                    {
                        int iVal1, iVal2;
                        HalconCamera.HalconCameraLinX_NEDLineCameraXCM4040SAT2.FindExposure(llexp[i][n], out iVal1, out iVal2);
                        sValueMessage += "   カメラ" + (_lstCalib[i].ListCameras[n].Index + 1).ToString() + " 露光 Val1:"
                            + iVal1.ToString() + " Val2:" + iVal2.ToString() + "\n";
                    }
                }
            }
            return "自動調整終了\n更新しますか？\n\n" + sValueMessage;
        }

        void clsAutoLightCalibration_OnEvent(object sender, clsAutoLightValueCalibration.LightValueEventArgs e)
        {
            if (_frmAutoLightValueCalibration == null)
                return;

            clsAutoLightValueCalibration alvc = (clsAutoLightValueCalibration)sender;

            switch (e.EventType)
            {
                case clsAutoLightValueCalibration.EEventType.Start:
                    _frmAutoLightValueCalibration.SetMessage("", frmAutoLightValueCalibration.EDisplayMode.Run);
                    switch (alvc.CameraSide)
                    {
                        case clsAutoLightValueCalibration.ECameraSide.UpSide:
                            _frmAutoLightValueCalibration.SetProgressMessage(0, "表カメラ:" + e.Message);
                            break;
                        case clsAutoLightValueCalibration.ECameraSide.DownSide:
                            _frmAutoLightValueCalibration.SetProgressMessage(1, "裏カメラ:" + e.Message);
                            break;
                    }
                    break;

                case clsAutoLightValueCalibration.EEventType.Finish:
                    for (int i = 0; i < _lstCalib.Count; i++)
                    {
                        if (_lstCalib[i] == sender)
                        {
                            _lstResult[i] = e.LightValue;

                            if (_lstCalib[i].AutoLightFull)
                            {
                                for (int n = 0; n < _lstCalib[i].CameraNum; n++)
                                {
                                    _llstExposure[i].Add(e.Exposure[n]);
                                }
                            }
                        }
                    }

                    if (_lstResult.Count(x => x != -1) == _lstResult.Count)
                    {
                        // 両方とも埋まったら、アップデートメッセージを実行
                        _frmAutoLightValueCalibration.SetMessage(queryUpdateMessage(LightType, _lstResult, _llstExposure), frmAutoLightValueCalibration.EDisplayMode.UpdateQuery);
                    }
                    break;
                case clsAutoLightValueCalibration.EEventType.Cancel:
                    _frmAutoLightValueCalibration.SetMessage("ユーザーによってキャンセルされました", frmAutoLightValueCalibration.EDisplayMode.Cancel);
                    break;
                case clsAutoLightValueCalibration.EEventType.Error:
                    _frmAutoLightValueCalibration.SetMessage(e.Message, frmAutoLightValueCalibration.EDisplayMode.Cancel);
                    break;
            }
        }

        /// <summary>自動調光をキャンセルする　v1338のPC電源ボタン押下対応</summary>//v1338 yuasa
        private void CalibrationCancel()
        {
            _frmAutoLightValueCalibration.AutoLightValueCalibrationCancel();
        }
    }
}
