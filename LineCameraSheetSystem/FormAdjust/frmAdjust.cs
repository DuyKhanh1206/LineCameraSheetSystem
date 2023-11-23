using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using HalconCamera;
using Fujita.LightControl;
using Fujita.Misc;
using HalconDotNet;
using ViewROI;
using LineCameraSheetSystem; //V1058 メンテナンス追加 yuasa 20190125

namespace Adjustment
{
    public partial class frmAdjust : Form
    {
        const string ADJUSTMENT_SETTING_FILE = "adjustment.ini";

        List<HWindowControl> _lstWndCtrls = new List<HWindowControl>();
        List<uclCameraControl> _lstCamCtrl = new List<uclCameraControl>();
        List<uclLightControl> _lstLightCtrl = new List<uclLightControl>();
        List<RadioButton> _lstDisplay = new List<RadioButton>();
        List<RadioButton> _lstResCameras = new List<RadioButton>();
        List<RadioButton> _lstPosCalib = new List<RadioButton>();
        List<NumericUpDown> _lstThreshold = new List<NumericUpDown>();
        List<uclMaintenanceControl> _lstMainteCtrl = new List<uclMaintenanceControl>(); //V1058 メンテナンス追加 yuasa 20190125：メンテナンスのユーザコントロールをListで生成

        ViewROI.HWndCtrl _hWndCtrl = null;

        bool _bDoPosCalib = false;
        bool _bDoneCalibPosCam = false;

        bool _bDoneCalibResCam = false;
        bool _bDoResCalib = false;


        IOffsetParamContainer _OffsetParamContainer = null;
        IResolutionParamContainer _ResolutionParamContainer = null;

        HObject[] _ahoGrabbedImage = new HObject[4];

        clsPosCalibration _PosCalib = new clsPosCalibration();

        const int CLB_ITEM_GRAY_AVEDEV = 0;
        const int CLB_ITEM_GRAY_MINMAX = 1;
        const int CLB_ITEM_GRAY_FOCUS = 2;
        const int CLB_ITEM_HORZ_AVEDEV = 3;
        const int CLB_ITEM_HORZ_MINMAX = 4;
        const int CLB_ITEM_VERT_AVEDEV = 5;
        const int CLB_ITEM_VERT_MINMAX = 6;
        const int CLB_ITEM_FRAME_COUNT = 7;

        public void SetOffsetParamContainer(IOffsetParamContainer op)
        {
            _OffsetParamContainer = op;
        }

        public void SetResolutionParamContainer(IResolutionParamContainer rp)
        {
            _ResolutionParamContainer = rp;
        }

        private void loadParameters(string sPath)
        {
            _ResCalib2.Load(sPath, "");
            _PosCalib.Load(sPath, "");

            IniFileAccess ifa = new IniFileAccess();
            uclLightControl1.Enable = ifa.GetIni("frmAdjust_Params", "LightEnable1", false, sPath);
            uclLightControl1.Value = ifa.GetIni("frmAdjust_Params", "LightValue1", 0, sPath);
            uclLightControl2.Enable = ifa.GetIni("frmAdjust_Params", "LightEnable2", false, sPath);
            uclLightControl2.Value = ifa.GetIni("frmAdjust_Params", "LightValue2", 0, sPath);
            uclLightControl3.Enable = ifa.GetIni("frmAdjust_Params", "LightEnable3", false, sPath);
            uclLightControl3.Value = ifa.GetIni("frmAdjust_Params", "LightValue3", 0, sPath);
            uclLightControl4.Enable = ifa.GetIni("frmAdjust_Params", "LightEnable4", false, sPath);
            uclLightControl4.Value = ifa.GetIni("frmAdjust_Params", "LightValue4", 0, sPath);
            uclLightControl5.Enable = ifa.GetIni("frmAdjust_Params", "LightEnable5", false, sPath);
            uclLightControl5.Value = ifa.GetIni("frmAdjust_Params", "LightValue5", 0, sPath);

            //V1058 メンテナンス追加 yuasa 20190128：ROI部分を追加
            //ROI部分
            for (int i = 0; i < _lstMainteCtrl.Count; i++)
            {
                _lstMainteCtrl[i].UserContData.x = ifa.GetIni("Setting_Camera" + (i + 1).ToString(), "X", 0, sPath);
                _lstMainteCtrl[i].UserContData.y = ifa.GetIni("Setting_Camera" + (i + 1).ToString(), "Y", 0, sPath);
                _lstMainteCtrl[i].UserContData.w = ifa.GetIni("Setting_Camera" + (i + 1).ToString(), "W", 1, sPath);
                _lstMainteCtrl[i].UserContData.h = ifa.GetIni("Setting_Camera" + (i + 1).ToString(), "H", 1, sPath);
            }
            //カメラ部　表示の更新
            for (int i = 0; i < _lstMainteCtrl.Count; i++)
                _lstMainteCtrl[i].userContDataUpdate();

        }

        private void saveParameters(string sPath)
        {
            _ResCalib2.Save(sPath, "");
            _PosCalib.Save(sPath, "");

            IniFileAccess ifa = new IniFileAccess();
            ifa.SetIni("frmAdjust_Params", "LightEnable1", uclLightControl1.Enable, sPath);
            ifa.SetIni("frmAdjust_Params", "LightValue1", uclLightControl1.Value, sPath);
            ifa.SetIni("frmAdjust_Params", "LightEnable2", uclLightControl2.Enable, sPath);
            ifa.SetIni("frmAdjust_Params", "LightValue2", uclLightControl2.Value, sPath);
            ifa.SetIni("frmAdjust_Params", "LightEnable3", uclLightControl3.Enable, sPath);
            ifa.SetIni("frmAdjust_Params", "LightValue3", uclLightControl3.Value, sPath);
            ifa.SetIni("frmAdjust_Params", "LightEnable4", uclLightControl4.Enable, sPath);
            ifa.SetIni("frmAdjust_Params", "LightValue4", uclLightControl4.Value, sPath);
            ifa.SetIni("frmAdjust_Params", "LightEnable5", uclLightControl5.Enable, sPath);
            ifa.SetIni("frmAdjust_Params", "LightValue5", uclLightControl5.Value, sPath);

            //V1058 メンテナンス追加 yuasa 20190128：ROI部分を追加
            //ROI部分
            for (int i = 0; i < _lstMainteCtrl.Count; i++)
            {
                ifa.SetIni("Setting_Camera" + (i + 1).ToString(), "X", _lstMainteCtrl[i].UserContData.x, sPath);
                ifa.SetIni("Setting_Camera" + (i + 1).ToString(), "Y", _lstMainteCtrl[i].UserContData.y, sPath);
                ifa.SetIni("Setting_Camera" + (i + 1).ToString(), "W", _lstMainteCtrl[i].UserContData.w, sPath);
                ifa.SetIni("Setting_Camera" + (i + 1).ToString(), "H", _lstMainteCtrl[i].UserContData.h, sPath);
            }
        }

        public frmAdjust()
        {
            InitializeComponent();

            string sAppPath = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\') + 1);

            loadParameters(sAppPath + ADJUSTMENT_SETTING_FILE);

            _lstWndCtrls.Add(hWindowControl1);
            _lstWndCtrls.Add(hWindowControl2);
            _lstWndCtrls.Add(hWindowControl3);
            _lstWndCtrls.Add(hWindowControl4);
            
            _lstMainteCtrl.Add(uclMaintenanceControl1); //V1058 メンテナンス追加 yuasa 20190125：Listに実体を追加
            _lstMainteCtrl.Add(uclMaintenanceControl2);
            _lstMainteCtrl.Add(uclMaintenanceControl3);
            _lstMainteCtrl.Add(uclMaintenanceControl4);

            _lstCamCtrl.Add(uclCameraControl1);
            _lstCamCtrl.Add(uclCameraControl2);
            _lstCamCtrl.Add(uclCameraControl3);
            _lstCamCtrl.Add(uclCameraControl4);

            _lstLightCtrl.Add(uclLightControl1);
            _lstLightCtrl.Add(uclLightControl2);
            _lstLightCtrl.Add(uclLightControl3);
            _lstLightCtrl.Add(uclLightControl4);
            _lstLightCtrl.Add(uclLightControl5);

            _lstResCameras.Add(rdoResCam1);
            _lstResCameras.Add(rdoResCam2);
            _lstResCameras.Add(rdoResCam3);
            _lstResCameras.Add(rdoResCam4);

            _ardoBaseCams = new RadioButton[] { rdoBaseCam1, rdoBaseCam2, rdoBaseCam3, rdoBaseCam4 };
            _ardoTargetCams = new RadioButton[] { rdoTargetCam1, rdoTargetCam2, rdoTargetCam3, rdoTargetCam4 };

            _lstDisplay.Add(rdoDisplay1);
            _lstDisplay.Add(rdoDisplay2);
            _lstDisplay.Add(rdoDisplay3);
            _lstDisplay.Add(rdoDisplay4);
            _lstDisplay.Add(rdoDisplayAll);

            _lstThreshold.Add(nudBinDispThresholdLow);
            _lstThreshold.Add(nudBinDispThresholdHigh);

            _hWndCtrl = new HWndCtrl(hWindowControlMono);
            _hWndCtrl.SetMagnifyList(new double[] { 0.1, 0.15, 0.2, 0.25, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 1.0, 2.0 });
            _hWndCtrl.SetMagnifyComboControl(cmbMagnify);
            _hWndCtrl.SetScrollbarControl(hsbWindowMono, vsbWindowMono);
            _hWndCtrl.FirstTimeFitting = true;
            _hWndCtrl.CenterLine = chkDispCrossLine.Checked;
            _hWndCtrl.Repaint += new RepaintEventHandler(_hWndCtrl_Repaint);

            layoutWindow(_eWindowDisplay);

            // カメラ
            for (int i = 0; i < APCameraManager.getInstance().CameraNum; i++)
            {
                if (i >= _lstCamCtrl.Count)
                    break;


                {
                    HalconCameraPylonGigELineSensor cam = APCameraManager.getInstance().GetCamera(i) as HalconCameraPylonGigELineSensor;
                    if (cam != null)
                    {
                        _lstCamCtrl[i].SetCamera(cam);
                        _lstMainteCtrl[i].SetCamera(cam); //V1058 メンテナンス追加 yuasa 20190125：メンテナンスのユーザコントロール分セットカメラを実行。ライト用は別記。
                    }
                }

                {
                    HalconCameraSaperaLTDALSALineSensor  cam = APCameraManager.getInstance().GetCamera(i) as HalconCameraSaperaLTDALSALineSensor;
                    if (cam != null)
                    {
                        _lstCamCtrl[i].SetCamera(cam);
                        _lstMainteCtrl[i].SetCamera(cam); //V1058 メンテナンス追加 yuasa 20190125：メンテナンスのユーザコントロール分セットカメラを実行。ライト用は別記。
                    }
                }

                {
                    HalconCameraFile cam = APCameraManager.getInstance().GetCamera(i) as HalconCameraFile;
                    if (cam != null)
                    {
                        _lstCamCtrl[i].SetCamera(cam);
                        _lstMainteCtrl[i].SetCamera(cam); //V1058 メンテナンス追加 yuasa 20190125：メンテナンスのユーザコントロール分セットカメラを実行。ライト用は別記。
                    }
                }

                {
                    HalconCameraFileMemory cam = APCameraManager.getInstance().GetCamera(i) as HalconCameraFileMemory;
                    if (cam != null)
                    {
                        _lstCamCtrl[i].SetCamera(cam);
                        _lstMainteCtrl[i].SetCamera(cam); //V1058 メンテナンス追加 yuasa 20190125：メンテナンスのユーザコントロール分セットカメラを実行。ライト用は別記。
                    }
                }

                _lstCamCtrl[i].GainChanged += new GainChangedEventHandler(frmCameraTest_GainChanged);
                _lstCamCtrl[i].OffsetChanged += new OffsetChangedEventHandler(frmCameraTest_OffsetChanged);
                _lstCamCtrl[i].ExposureChanged += new ExposureChangedEventHandler(frmCameraTest_ExposureChanged);
                _lstCamCtrl[i].EnableCameraCheckedChange += new EnableCameraCheckedChangeEventHandler(frmCameraTest_EnableCameraCheckedChange);

                APCameraManager.getInstance().SetGrabbedImageEvent(i, frmAdjust_OnGrabbedImage);

                // カメラの数分増やす
                dgvInspRes.Rows.Add();
            }

            // 照明のセット
            for (int i = 0; i < LightControlManager.getInstance().LightCount; i++)
            {
                if (i >= _lstLightCtrl.Count)
                    break;
                _lstLightCtrl[i].SetLight(LightControlManager.getInstance().GetLight(i));
            }

            loadParameters(sAppPath + ADJUSTMENT_SETTING_FILE); //V1058 メンテナンス追加 yuasa 20190128：MinMax範囲設定後にiniデータを格納

            // イベント登録
            for (int i = 0; i < _lstResCameras.Count; i++)
            {
                _lstResCameras[i].Click += new EventHandler(frmAdjust_ResCameraClick);
            }

            for (int i = 0; i < _ardoBaseCams.Length; i++)
            {
                _ardoBaseCams[i].CheckedChanged += new EventHandler(frmAdjust_PosCalibBaseCheckedChanged);
            }

            for (int i = 0; i < _ardoTargetCams.Length; i++)
            {
                _ardoTargetCams[i].CheckedChanged += new EventHandler(frmAdjust_PosCalibTargetCheckedChanged);
            }

            for (int i = 0; i < _lstDisplay.Count; i++)
            {
                _lstDisplay[i].Click += new EventHandler(frmAdjust_DisplayClick);
            }
            for (int i = 0; i < _lstThreshold.Count; i++)
            {
                _lstThreshold[i].ValueChanged += new EventHandler(Threshold_ValueChanged);
            }
            // キャリブレーション値を設定する
            rdoResCam1.Checked = true;

        }
        void frmCameraTest_EnableCameraCheckedChange(object sender, EnableCameraCheckedChnageEventArgs e)
        {
            //if (chkLive.Checked)
            //{
            //    Live(true);
            //}
            //if (chkHardTriggerWait.Checked)
            //{
            //    HardTrigger(true);
            //}
        }
        void frmCameraTest_ExposureChanged(object sender, ExposureChangedEventArgs e)
        {
            HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(e.Index);
            camera.SetExposureTime(e.Value);
        }

        void frmCameraTest_OffsetChanged(object sender, OffsetChangedEventArgs e)
        {
            HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(e.Index);
            camera.SetOffset(e.Value);
        }

        void frmCameraTest_GainChanged(object sender, GainChangedEventArgs e)
        {
            HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(e.Index);
            camera.SetGain(e.Value);
        }

        void frmAdjust_PosCalibTargetCheckedChanged(object sender, EventArgs e)
        {
            _PosCalib.TargetCamNo = (EAdjustmentCameraType)getPosCalibTargetCam();

            updatePosCalibDatas(true);
            double dResX = 0d, dResY = 0d;
            _ResolutionParamContainer.GetResolutionParamtter(_PosCalib.TargetCamNo, ref dResX, ref dResY);
            _PosCalib.TargetResolutionHorz = dResX;
            _PosCalib.TargetResolutionVert = dResY;
            _bDoneCalibPosCam = false;
            txtOffsetXCam.Text = "";
            txtOffsetYCam.Text = "";
            resetPosCalibResult();
            updatePosCalibDatas(false);
            updateControls();
        }

        void frmAdjust_PosCalibBaseCheckedChanged(object sender, EventArgs e)
        {
            EAdjustmentCameraType eOldBase = _PosCalib.BaseCamNo;
            EAdjustmentCameraType eOldTarget = _PosCalib.TargetCamNo;


            _PosCalib.BaseCamNo = (EAdjustmentCameraType)getPosCalibBaseCam();
            _PosCalib.TargetCamNo = (EAdjustmentCameraType)getPosCalibTargetCam();

            if (eOldBase != _PosCalib.BaseCamNo)
            {
                updatePosCalibDatas(true);

                double dResX = 0d, dResY = 0d;
                _ResolutionParamContainer.GetResolutionParamtter(_PosCalib.BaseCamNo, ref dResX, ref dResY);
                _PosCalib.BaseResolutionHorz = dResX;
                _PosCalib.BaseResolutionVert = dResY;

                _ResolutionParamContainer.GetResolutionParamtter(_PosCalib.TargetCamNo, ref dResX, ref dResY);
                _PosCalib.TargetResolutionHorz = dResX;
                _PosCalib.TargetResolutionVert = dResY;

                _bDoneCalibPosCam = false;
                txtOffsetXCam.Text = "";
                txtOffsetYCam.Text = "";

                resetPosCalibResult();

                updatePosCalibDatas(false);

                updateControls();
            }
        }

        enum EWindowDisplay
        {
            Camera1,
            Camera2,
            Camera3,
            Camera4,
            CameraAll,
        }

        EWindowDisplay _eWindowDisplay = EWindowDisplay.CameraAll;
        void layoutWindow(EWindowDisplay eWD)
        {
            if (eWD == EWindowDisplay.CameraAll)
            {
                hWindowControlMono.Visible = false;
                hsbWindowMono.Visible = false;
                vsbWindowMono.Visible = false;
                hWindowControl1.Visible = true;
                hWindowControl2.Visible = true;
                hWindowControl3.Visible = true;
                hWindowControl4.Visible = true;

                cmbMagnify.Enabled = false;
                chkDispCrossLine.Enabled = false;

                chkBinDisp.Enabled = false;
                nudBinDispThresholdHigh.Enabled = false;
                nudBinDispThresholdLow.Enabled = false;
            }
            else
            {
                hWindowControlMono.Visible = true;
                hsbWindowMono.Visible = true;
                vsbWindowMono.Visible = true;
                hWindowControl1.Visible = false;
                hWindowControl2.Visible = false;
                hWindowControl3.Visible = false;
                hWindowControl4.Visible = false;

                cmbMagnify.Enabled = true;
                chkDispCrossLine.Enabled = true;

                chkBinDisp.Enabled = true;
                nudBinDispThresholdHigh.Enabled = true;
                nudBinDispThresholdLow.Enabled = true;

            }
        }

        void frmAdjust_DisplayClick(object sender, EventArgs e)
        {
            for( int i = 0 ; i < _lstDisplay.Count; i++ )
            {
                if( _lstDisplay[i] == sender )
                    _eWindowDisplay = (EWindowDisplay)i;
            }
            layoutWindow(_eWindowDisplay);
        }

        void updateResControls()
        {
            if ( _ResCalib2.IsResultSuccess )
            {
                btnRegResCalib.Enabled = true;
            }
            else
            {
                btnRegResCalib.Enabled = false;
            }
        }

        void setResolutionData()
        {
            if (_ResolutionParamContainer != null)
            {
                if (_ResCalib2.IsResultSuccess )
                {
                    _ResolutionParamContainer.SetResolutionParameter(_eResCameraNo, _ResCalib2.ResultData.ResolutionHorz, _ResCalib2.ResultData.ResolutionVert);
                }
            }
        }

        void updatePosCalibDatas(bool bUp)
        {
            if (bUp)
            {
                _PosCalib.ThresholdParamBaseCam.ThresholdLow = (int)nudThresholdLow1.Value;
                _PosCalib.ThresholdParamBaseCam.ThresholdHigh = (int)nudThresholdHigh1.Value;
                _PosCalib.ThresholdParamBaseCam.OpeningRadius = (double)nudOpeningArea1.Value;
                _PosCalib.ThresholdParamBaseCam.ClosingRadius = (double)nudClosingArea1.Value;
                _PosCalib.ThresholdParamBaseCam.InspectOffset = (int)nudInspectOffset1.Value;
                _PosCalib.ThresholdParamBaseCam.InspectWidth = (int)nudInspectWidth1.Value;

                _PosCalib.ThresholdParamBaseCam.LimitMinWidthMili = (double)nudLimitMinHorzMili.Value;
                _PosCalib.ThresholdParamBaseCam.LimitMaxWidthMili = (double)nudLimitMaxHorzMili.Value;
                _PosCalib.ThresholdParamBaseCam.LimitMinHeightMili = (double)nudLimitMinVertMili.Value;
                _PosCalib.ThresholdParamBaseCam.LimitMaxHeightMili = (double)nudLimitMaxVertMili.Value;

                _PosCalib.ThresholdParamTargetCam.ThresholdLow = (int)nudThresholdLowN.Value;
                _PosCalib.ThresholdParamTargetCam.ThresholdHigh = (int)nudThresholdHighN.Value;
                _PosCalib.ThresholdParamTargetCam.OpeningRadius = (double)nudOpeningAreaN.Value;
                _PosCalib.ThresholdParamTargetCam.ClosingRadius = (double)nudClosingAreaN.Value;
                _PosCalib.ThresholdParamTargetCam.InspectOffset = (int)nudInspectOffsetN.Value;
                _PosCalib.ThresholdParamTargetCam.InspectWidth = (int)nudInspectWidthN.Value;

                _PosCalib.ThresholdParamTargetCam.LimitMinWidthMili = (double)nudLimitMinHorzMili.Value;
                _PosCalib.ThresholdParamTargetCam.LimitMaxWidthMili = (double)nudLimitMaxHorzMili.Value;
                _PosCalib.ThresholdParamTargetCam.LimitMinHeightMili = (double)nudLimitMinVertMili.Value;
                _PosCalib.ThresholdParamTargetCam.LimitMaxHeightMili = (double)nudLimitMaxVertMili.Value;

                if (_ResolutionParamContainer != null)
                {
                    double dResCam1X = 0d, dResCam1Y = 0d;
                    _ResolutionParamContainer.GetResolutionParamtter(_PosCalib.BaseCamNo, ref dResCam1X, ref dResCam1Y);
                    _PosCalib.BaseResolutionHorz = dResCam1X;
                    _PosCalib.BaseResolutionVert = dResCam1Y;
                    double dResCamNX = 0d, dResCamNY = 0d;
                    _ResolutionParamContainer.GetResolutionParamtter(_PosCalib.TargetCamNo, ref dResCamNX, ref dResCamNY);
                    _PosCalib.TargetResolutionHorz = dResCamNX;
                    // カメラ1の分解能を使用する
//                    _PosCalib.TargetResolutionVert = dResCamNY;
                    _PosCalib.TargetResolutionVert = dResCam1Y;
                }

                _PosCalib.LimitCount = (int)nudLimitCount.Value;
            }
            else
            {
                setPosCalibBaseCam(_PosCalib.BaseCamNo);
                setPosCalibTargetCam(_PosCalib.TargetCamNo);

                nudThresholdLow1.Value = _PosCalib.ThresholdParamBaseCam.ThresholdLow;
                nudThresholdHigh1.Value = _PosCalib.ThresholdParamBaseCam.ThresholdHigh;
                nudOpeningArea1.Value = (decimal)_PosCalib.ThresholdParamBaseCam.OpeningRadius;
                nudClosingArea1.Value = (decimal)_PosCalib.ThresholdParamBaseCam.ClosingRadius;
                nudInspectOffset1.Value = _PosCalib.ThresholdParamBaseCam.InspectOffset;
                nudInspectWidth1.Value = _PosCalib.ThresholdParamBaseCam.InspectWidth;


                nudLimitMinHorzMili.Value = (decimal)_PosCalib.ThresholdParamBaseCam.LimitMinWidthMili;
                nudLimitMaxHorzMili.Value = (decimal)_PosCalib.ThresholdParamBaseCam.LimitMaxWidthMili;
                nudLimitMinVertMili.Value = (decimal)_PosCalib.ThresholdParamBaseCam.LimitMinHeightMili;
                nudLimitMaxVertMili.Value = (decimal)_PosCalib.ThresholdParamBaseCam.LimitMaxHeightMili;

                nudThresholdLowN.Value = _PosCalib.ThresholdParamTargetCam.ThresholdLow;
                nudThresholdHighN.Value = _PosCalib.ThresholdParamTargetCam.ThresholdHigh;
                nudOpeningAreaN.Value = (decimal)_PosCalib.ThresholdParamTargetCam.OpeningRadius;
                nudClosingAreaN.Value = (decimal)_PosCalib.ThresholdParamTargetCam.ClosingRadius;
                nudInspectOffsetN.Value = _PosCalib.ThresholdParamTargetCam.InspectOffset;
                nudInspectWidthN.Value = _PosCalib.ThresholdParamTargetCam.InspectWidth;

                nudLimitCount.Value = _PosCalib.LimitCount;

                double dOffX = 0d, dOffY = 0d;
                if (_OffsetParamContainer != null)
                {
                    _OffsetParamContainer.GetOffsetParameter(_PosCalib.TargetCamNo, ref dOffX, ref dOffY);
                    lblOffsetXCamNow.Text = dOffX.ToString("F6");
                    lblOffsetYCamNow.Text = dOffY.ToString("F6");
                }
            }
        }

        void updateResCalibDatas( bool bUp )
        {
            if (bUp)
            {
                _ResCalib2.Param.ThresholdLow = (int)nudResThresholdLow.Value;
                _ResCalib2.Param.ThresholdHigh = (int)nudResThresholdHigh.Value;

                _ResCalib2.Param.LimitMinHorzPix = (double)nudResLimitMinHorzPix.Value;
                _ResCalib2.Param.LimitMaxHorzPix = (double)nudResLimitMaxHorzPix.Value;
                _ResCalib2.Param.LimitMinVertPix = (double)nudResLimitMinVertPix.Value;
                _ResCalib2.Param.LimitMaxVertPix = (double)nudResLimitMaxVertPix.Value;

                _ResCalib2.Param.RealHorzMili = (double)nudResRealHorzMili.Value;
                _ResCalib2.Param.RealVertMili = (double)nudResRealVertMili.Value;

                _ResCalib2.CaptureLimit = (int)nudResCaptureLimit.Value;
                _ResCalib2.LapSize = (int)nudResLapSize.Value;

                _ResCalib2.Param.InspectOffset = (int)nudResInspectOffset.Value;
                _ResCalib2.Param.InspectWidth = (int)nudResInspectWidth.Value;
                _ResCalib2.Param.ClosingRadius = (double)nudResClosingArea.Value;
                _ResCalib2.Param.OpeningRadius = (double)nudResOpeningArea.Value;
            }
            else
            {
                nudResThresholdLow.Value = _ResCalib2.Param.ThresholdLow;
                nudResThresholdHigh.Value = _ResCalib2.Param.ThresholdHigh;

                nudResLimitMinHorzPix.Value = (decimal)_ResCalib2.Param.LimitMinHorzPix;
                nudResLimitMaxHorzPix.Value = (decimal)_ResCalib2.Param.LimitMaxHorzPix;
                nudResLimitMinVertPix.Value = (decimal)_ResCalib2.Param.LimitMinVertPix;
                nudResLimitMaxVertPix.Value = (decimal)_ResCalib2.Param.LimitMaxVertPix;

                nudResRealHorzMili.Value = (decimal)_ResCalib2.Param.RealHorzMili;
                nudResRealVertMili.Value = (decimal)_ResCalib2.Param.RealVertMili;

                nudResCaptureLimit.Value = _ResCalib2.CaptureLimit;
                nudResLapSize.Value = _ResCalib2.LapSize;

                nudResInspectOffset.Value = _ResCalib2.Param.InspectOffset;
                nudResInspectWidth.Value = _ResCalib2.Param.InspectWidth;

                nudResOpeningArea.Value = (decimal)_ResCalib2.Param.OpeningRadius;
                nudResClosingArea.Value = (decimal)_ResCalib2.Param.ClosingRadius;

                double dX = 0d, dY = 0d;
                if (_ResolutionParamContainer != null)
                {
                    _ResolutionParamContainer.GetResolutionParamtter(_eResCameraNo, ref dX, ref dY);
                    lblResHorzNow.Text = dX.ToString("F6");
                    lblResVertNow.Text = dY.ToString("F6");
                }

                if (_ResCalib2.IsResultSuccess)
                {
                    txtResHorzMeas.Text = _ResCalib2.ResultData.ResolutionHorz.ToString("F6");
                    txtResVertMeas.Text = _ResCalib2.ResultData.ResolutionVert.ToString("F6");
                }
                else
                {
                    txtResHorzMeas.Text = "";
                    txtResVertMeas.Text = "";
                }
            }

        }

        EAdjustmentCameraType _eResCameraNo = EAdjustmentCameraType.Camera1;
        void checkResCameraNo()
        {
            EAdjustmentCameraType eOld = _eResCameraNo;

            if (rdoResCam1.Checked)
            {
                _eResCameraNo = EAdjustmentCameraType.Camera1;
            }
            else if (rdoResCam2.Checked)
            {
                _eResCameraNo = EAdjustmentCameraType.Camera2;
            }
            else if (rdoResCam3.Checked)
            {
                _eResCameraNo = EAdjustmentCameraType.Camera3;
            }
            else if (rdoResCam4.Checked)
            {
                _eResCameraNo = EAdjustmentCameraType.Camera4;
            }

            if (eOld != _eResCameraNo)
            {
                _ResCalib2.ResultClear();
            }
        }

        void frmAdjust_ResCameraClick(object sender, EventArgs e)
        {
            updateResCalibDatas(true);
            checkResCameraNo();
            updateResCalibDatas(false);
            updateResControls();
        }

        void frmAdjust_RegParamCalibPosCamClick(object sender, EventArgs e)
        {
        }

        void frmAdjust_CalibPosCamClick(object sender, EventArgs e)
        {

        }

        clsResCalibration2 _ResCalib2 = new clsResCalibration2();
        void progressResCalib(GrabbedImageEventArgs e)
        {
            Action act = new Action(() =>
            {
                _ResCalib2.AddImage(e.OrgImage);
                if (_ResCalib2.IsResultSuccess)
                {
                    updateResCalibResult(true, "正常終了");
                    txtResHorzMeas.Text = _ResCalib2.ResultData.ResolutionHorz.ToString("F6");
                    txtResVertMeas.Text = _ResCalib2.ResultData.ResolutionVert.ToString("F6");
                    _bDoneCalibResCam = true;
                    _bDoResCalib = false;
                    LiveFinishAction();
                }
                else if (_ResCalib2.IsCaptureLimit())
                {
                    _bDoResCalib = false;
                    updateResCalibResult(false, "ｷｬﾌﾟﾁｬﾘﾐｯﾄｵﾊﾞｰ");
                    LiveFinishAction();
                }
                updateControls();
            });

            if (InvokeRequired)
            {
                Invoke(act);
            }
            else
            {
                act.Invoke();
            }
        }


        RadioButton[] _ardoBaseCams;
        RadioButton[] _ardoTargetCams;

        private int getPosCalibBaseCam()
        {
            for (int i = 0; i < _ardoBaseCams.Length; i++)
            {
                if (_ardoBaseCams[i].Checked)
                    return i;
            }
            return -1;
        }

        private void setPosCalibBaseCam(EAdjustmentCameraType eCam)
        {
            _ardoBaseCams[(int)eCam].Checked = true;
        }

        private int getPosCalibTargetCam()
        {
            for (int i = 0; i < _ardoTargetCams.Length; i++)
            {
                if (_ardoTargetCams[i].Checked)
                    return i;
            }
            return -1;
        }

        private void setPosCalibTargetCam(EAdjustmentCameraType eCam)
        {
            _ardoTargetCams[(int)eCam].Checked = true;
        }

        /// <summary>
        /// 位置キャリブレーションの初期化
        /// </summary>
        void readyPosCalib()
        {
            lsvPosCalibProg.Items.Clear();
            for (int i = 1; i <= _PosCalib.LimitCount; i++)
            {
                ListViewItem lvi = lsvPosCalibProg.Items.Add(i.ToString());
                lvi.SubItems.Add("－");
                lvi.SubItems.Add("－");
            }
        }

        void LiveFinishAction()
        {
            APCameraManager.getInstance().LiveFinishComplete += frmAdjust_LiveFinishComplete;
            _bLiveFinishTry = true;
            APCameraManager.getInstance().SyncStopLive(false);
        }

        void progressPosCalib(GrabbedImageEventArgs e)
        {
            EAdjustmentCameraType eCam = EAdjustmentCameraType.Camera1;
            switch (e.Index)
            {
                case 1:
                    eCam = EAdjustmentCameraType.Camera2;
                    break;
                case 2:
                    eCam = EAdjustmentCameraType.Camera3;
                    break;
                case 3:
                    eCam = EAdjustmentCameraType.Camera4;
                    break;
            }

            _PosCalib.AddImage(e.OrgImage, eCam);
            Action act = new Action(() =>
            {

                if (_PosCalib.MeasPos())
                {
                    // 値が正常に入ったので値を取得する
                    updatePosCalibResult(true, "正常終了");
                    txtOffsetXCam.Text = _PosCalib.MeasOffsetX.ToString("F6");
                    txtOffsetYCam.Text = _PosCalib.MeasOffsetY.ToString("F6");
                    _bDoneCalibPosCam = true;
                    _bDoPosCalib = false;
                    LiveFinishAction();
                }
                else if (_PosCalib.CheckLimitCountOver())
                {
                    _bDoPosCalib = false;
                    updatePosCalibResult(false, "ｷｬﾌﾟﾁｬﾘﾐｯﾄｵﾊﾞｰ");
                    LiveFinishAction();
                }

                for (int i = 0; i < _PosCalib.BaseImageCount; i++)
                {
                    if (lsvPosCalibProg.Items.Count <= i)
                        break;
                    lsvPosCalibProg.Items[i].SubItems[1].Text = _PosCalib.IsBaseResultReady(i) ? "●" : "■";
                }

                for (int i = 0; i < _PosCalib.TargetImageCount; i++)
                {
                    if (lsvPosCalibProg.Items.Count <= i)
                        break;
                    lsvPosCalibProg.Items[i].SubItems[2].Text = _PosCalib.IsTargetResultReady(i) ? "●" : "■";
                }
                updateControls();

            });

            if (InvokeRequired)
            {
                Invoke(act);
            }
            else
            {
                act.Invoke();
            }
        }

        enum EProjectionType
        {
            Horizontal,
            Vertical,
        }

        void drawProjection( HTuple htHorz, HTuple htVert, EProjectionType eType, HWindowControl hWC, HObject img )
        {
            double dMagnify = clsHalconWindowControlLite.GetMagnify(img, hWC);
            double dRate = 1 / dMagnify;
            HObject hoXLD = null;


            int iWidth = 0, iHeight = 0;
            try
            {
                HTuple htWidth, htHeight;
                HOperatorSet.GetImageSize(img, out htWidth, out htHeight);
                iWidth = htWidth.I;
                iHeight = htHeight.I;
            }
            catch (HOperatorException)
            {
                return;
            }

            double [] adVert = null, adHorz = null;
            if( eType == EProjectionType.Horizontal )
            {
                double dBottom = 255 * dRate;
                if (dBottom > (double)iHeight)
                {
                    dBottom = iHeight;
                    dRate = iHeight / 255.0;
                }

                adVert = htVert.DArr;
                adHorz = new double[adVert.Length];
                int iLength = adVert.Length;
                for (int i = 0; i < iLength; i++)
                {
                    adVert[i] = dBottom - adVert[i] * dRate;
                    adHorz[i] = i;
                }
            }
            else
            {
                double dRight = 255 * dRate;
                if (dRight > (double)iWidth)
                {
                    dRight = iWidth;
                    dRate = iWidth / 255.0;
                }
                adHorz = htHorz.DArr;
                adVert = new double[adHorz.Length];

                int iLength = adHorz.Length;
                for (int i = 0; i < iLength; i++)
                {
                    adHorz[i] = adHorz[i] * dRate;
                    adVert[i] = i;
                }
            }

            try
            {
                HOperatorSet.GenContourPolygonXld(out hoXLD, new HTuple(adVert), new HTuple(adHorz));
                if( eType == EProjectionType.Horizontal )
                    clsHalconWindowControlLite.DispObj(hoXLD, hWC, clsHalconWindowControlLite.EHalconColor.red);
                else
                    clsHalconWindowControlLite.DispObj(hoXLD, hWC, clsHalconWindowControlLite.EHalconColor.blue);
            }
            catch (HOperatorException)
            {
            }
            finally
            {
                if (hoXLD != null)
                    hoXLD.Dispose();
            }
        }

        void progressImage(GrabbedImageEventArgs e)
        {
            if (e.Index >= APCameraManager.getInstance().CameraNum)
                return;

            if (clbInspectionItems.GetItemChecked(CLB_ITEM_GRAY_AVEDEV))
            {
                double dMean, dDevi;
                if (ImgIntensity(e.OrgImage, out dMean, out dDevi))
                {
                    dgvInspRes[CLB_ITEM_GRAY_AVEDEV, e.Index].Value = dMean.ToString("F2") + "/" + dDevi.ToString("F2");
                }
                else
                {
                    dgvInspRes[CLB_ITEM_GRAY_AVEDEV, e.Index].Value = "E";
                }
            }

            if (clbInspectionItems.GetItemChecked(CLB_ITEM_GRAY_MINMAX))
            {
                double dMin, dMax;
                if (ImgMinMax(e.OrgImage, out dMin, out dMax))
                {
                    dgvInspRes[CLB_ITEM_GRAY_MINMAX, e.Index].Value = dMin.ToString("F0") + "/" + dMax.ToString("F0");
                }
                else
                {
                    dgvInspRes[CLB_ITEM_GRAY_MINMAX, e.Index].Value = "E";
                }
            }

            if (clbInspectionItems.GetItemChecked(CLB_ITEM_GRAY_FOCUS))
            {
                double dFocus;
                if (ImgFocus(e.OrgImage, out dFocus))
                {
                    dgvInspRes[CLB_ITEM_GRAY_FOCUS, e.Index].Value = dFocus.ToString( "F2");
                }
                else
                {
                    dgvInspRes[CLB_ITEM_GRAY_FOCUS, e.Index].Value = "E";
                }
            }

            if (clbInspectionItems.GetItemChecked(CLB_ITEM_HORZ_AVEDEV)
                || clbInspectionItems.GetItemChecked(CLB_ITEM_HORZ_MINMAX)
                || clbInspectionItems.GetItemChecked(CLB_ITEM_VERT_AVEDEV)
                || clbInspectionItems.GetItemChecked(CLB_ITEM_VERT_MINMAX))
            {
                HTuple htHorz, htVert;
                if (ImgProjection(e.OrgImage, out htHorz, out htVert))
                {
                    if (clbInspectionItems.GetItemChecked(CLB_ITEM_HORZ_AVEDEV))
                    {
                        dgvInspRes[CLB_ITEM_HORZ_AVEDEV, e.Index].Value = htHorz.TupleMean().D.ToString("F1") + "/" + htHorz.TupleDeviation().D.ToString("F1");
                    }
                    if (clbInspectionItems.GetItemChecked(CLB_ITEM_HORZ_MINMAX))
                    {
                        dgvInspRes[CLB_ITEM_HORZ_MINMAX, e.Index].Value = htHorz.TupleMin().D.ToString("F1") + "/" + htHorz.TupleMax().D.ToString("F1");
                    }
                    if (clbInspectionItems.GetItemChecked(CLB_ITEM_VERT_AVEDEV))
                    {
                        dgvInspRes[CLB_ITEM_VERT_AVEDEV, e.Index].Value = htVert.TupleMean().D.ToString("F1") + "/" + htVert.TupleDeviation().D.ToString("F1");
                    }
                    if (clbInspectionItems.GetItemChecked(CLB_ITEM_VERT_MINMAX))
                    {
                        dgvInspRes[CLB_ITEM_VERT_MINMAX, e.Index].Value = htVert.TupleMin().D.ToString("F1") + "/" + htVert.TupleMax().D.ToString("F1");
                    }

                    if (_eWindowDisplay == EWindowDisplay.CameraAll)
                    {
                        if (clbInspectionItems.GetItemChecked(CLB_ITEM_HORZ_AVEDEV) || clbInspectionItems.GetItemChecked(CLB_ITEM_HORZ_MINMAX))
                        {
                            drawProjection(htHorz, htVert, EProjectionType.Horizontal, _lstWndCtrls[e.Index], e.OrgImage);
                        }

                        if (clbInspectionItems.GetItemChecked(CLB_ITEM_VERT_AVEDEV) || clbInspectionItems.GetItemChecked(CLB_ITEM_VERT_MINMAX))
                        {
                            drawProjection(htHorz, htVert, EProjectionType.Vertical, _lstWndCtrls[e.Index], e.OrgImage);
                        }
                    }
                    else
                    {
                        if (e.Index == (int)_eWindowDisplay)
                        {
                            if (clbInspectionItems.GetItemChecked(CLB_ITEM_HORZ_AVEDEV) || clbInspectionItems.GetItemChecked(CLB_ITEM_HORZ_MINMAX))
                            {
                                drawProjection(htHorz, htVert, EProjectionType.Horizontal, hWindowControlMono, e.OrgImage);
                            }

                            if (clbInspectionItems.GetItemChecked(CLB_ITEM_VERT_AVEDEV) || clbInspectionItems.GetItemChecked(CLB_ITEM_VERT_MINMAX))
                            {
                                drawProjection(htHorz, htVert, EProjectionType.Vertical, hWindowControlMono, e.OrgImage);
                            }
                        }
                    }
                }
                else
                {
                    if (clbInspectionItems.GetItemChecked(CLB_ITEM_HORZ_AVEDEV))
                    {
                        dgvInspRes[CLB_ITEM_HORZ_AVEDEV, e.Index].Value = "E";
                    }
                    if (clbInspectionItems.GetItemChecked(CLB_ITEM_HORZ_MINMAX))
                    {
                        dgvInspRes[CLB_ITEM_HORZ_MINMAX, e.Index].Value = "E";
                    }
                    if (clbInspectionItems.GetItemChecked(CLB_ITEM_VERT_AVEDEV))
                    {
                        dgvInspRes[CLB_ITEM_VERT_AVEDEV, e.Index].Value = "E";
                    }
                    if (clbInspectionItems.GetItemChecked(CLB_ITEM_VERT_MINMAX))
                    {
                        dgvInspRes[CLB_ITEM_VERT_MINMAX, e.Index].Value = "E";
                    }
                }
            }
        }

        private bool ImgProjection(HObject img, out HTuple htHorz, out HTuple htVert)
        {
            htHorz = null;
            htVert = null;

            if (img == null)
                return false;

            try
            {
                HOperatorSet.GrayProjections(img, img, "simple", out htHorz, out htVert);
            }
            catch (HOperatorException)
            {
                return false;
            }
            finally
            {
            }
            return true;
        }

        private bool ImgMinMax(HObject img, out double dMin, out double dMax)
        {
            dMin = 0d;
            dMax = 0d;

            if (img == null)
                return false;

            HTuple htMin, htMax, htRange;
            try
            {
                HOperatorSet.MinMaxGray(img, img, 0, out htMin, out htMax, out htRange);
                dMin = htMin.D;
                dMax = htMax.D;
            }
            catch (HOperatorException)
            {
                return false;
            }
            finally
            {
            }

            return true;
        }

        private bool ImgIntensity(HObject img, out double dMean, out double dDevi)
        {
            dMean = 0d;
            dDevi = 0d;

            if (img == null)
                return false;

            HTuple htMean, htDevi;
            try
            {
                HOperatorSet.Intensity(img, img, out htMean, out htDevi);
                dMean = htMean.D;
                dDevi = htDevi.D;
            }
            catch (HOperatorException)
            {
                return false;
            }
            finally
            {
            }
            return true;
        }

        private bool ImgFocus(HObject img, out double dFocus)
        {
            dFocus = 0.0;

            if (img == null)
                return false;

            HTuple htMean, htDeviation;
            HObject hoEdgeAmp = null;

            try
            {
                HOperatorSet.SobelAmp(img, out hoEdgeAmp, "sum_abs", 3);
                HOperatorSet.Intensity(hoEdgeAmp, hoEdgeAmp, out htMean, out htDeviation);
                dFocus = htMean.D;
            }
            catch (HOperatorException)
            {
                return false;
            }
            finally
            {
                if (hoEdgeAmp != null)
                    hoEdgeAmp.Dispose();
            }

            return true;
        }

        void updateFrameCount(GrabbedImageEventArgs e)
        {
            if (e.Index >= APCameraManager.getInstance().CameraNum)
                return;

            if (dgvInspRes[CLB_ITEM_FRAME_COUNT, e.Index].Value == null)
                dgvInspRes[CLB_ITEM_FRAME_COUNT, e.Index].Value = 0;
            dgvInspRes[CLB_ITEM_FRAME_COUNT, e.Index].Value = (int)dgvInspRes[CLB_ITEM_FRAME_COUNT, e.Index].Value + 1;
        }

        void resetFrameCount()
        {
            for (int i = 0; i < APCameraManager.getInstance().CameraNum; i++)
            {
                dgvInspRes[CLB_ITEM_FRAME_COUNT, i].Value = (int)0;
            }
        }

        void drawRois(GrabbedImageEventArgs e)
        {
            try
            {
                HTuple htWidth, htHeight;
                HOperatorSet.GetImageSize(e.OrgImage, out htWidth, out htHeight);
                IntPtr iHalconID = _lstWndCtrls[e.Index].HalconID;

                if (chkResDispInspectArea.Checked && e.Index == (int)_eResCameraNo )
                {
                    HOperatorSet.SetColor( iHalconID, "cyan");
                    HOperatorSet.SetDraw(iHalconID, "margin");
                    int iCol1 = (int)nudResInspectOffset.Value;
                    int iCol2 = ((int)nudResInspectWidth.Value == 0 )?htWidth.I:iCol1 + (int)nudResInspectWidth.Value;
                    HOperatorSet.DispRectangle1(iHalconID, 0, iCol1, htHeight.I, iCol2);
                }

                if (chkDispInspectArea.Checked )
                {
                    if (e.Index == (int)getPosCalibBaseCam())
                    {
                        HOperatorSet.SetColor(iHalconID, "magenta");
                        HOperatorSet.SetDraw(iHalconID, "margin");
                        int iCol1 = (int)nudInspectOffset1.Value;
                        int iCol2 = ((int)nudInspectWidth1.Value == 0) ? htWidth.I : iCol1 + (int)nudInspectWidth1.Value;
                        HOperatorSet.DispRectangle1(iHalconID, 0, iCol1, htHeight.I, iCol2);
                    }
                    else if (e.Index == getPosCalibTargetCam())
                    {
                        HOperatorSet.SetColor(iHalconID, "magenta");
                        HOperatorSet.SetDraw(iHalconID, "margin");
                        int iCol1 = (int)nudInspectOffsetN.Value;
                        int iCol2 = ((int)nudInspectWidthN.Value == 0) ? htWidth.I : iCol1 + (int)nudInspectWidthN.Value;
                        HOperatorSet.DispRectangle1(iHalconID, 0, iCol1, htHeight.I, iCol2);
                    }
                }

                //V1058 メンテナンス追加 yuasa 20190126：画面番号とROI設定番号の対応付け
                int useRoiNum = selectRoiNum(e.Index);

                //V1058 メンテナンス追加 yuasa 20190125：メンテナンス用に追加
                {
                    HObject rectangle;
                    HTuple mean, deviation;

                    int iCol1 = (int)_lstMainteCtrl[useRoiNum].UserContData.x;
                    int iCol2 = iCol1 + (int)_lstMainteCtrl[useRoiNum].UserContData.w;
                    int iRow1 = (int)_lstMainteCtrl[useRoiNum].UserContData.y;
                    int iRow2 = iRow1 + (int)_lstMainteCtrl[useRoiNum].UserContData.h;

                    if (chkRoiDisp.Checked)
                    {
                        HOperatorSet.SetDraw(iHalconID, "margin");
                        HOperatorSet.SetColor(iHalconID, "red");
                        HOperatorSet.DispRectangle1(iHalconID, iRow1, iCol1, iRow2, iCol2);
                    }

                    HOperatorSet.GenRectangle1(out rectangle, iRow1, iCol1, iRow2, iCol2);
                    HOperatorSet.Intensity(rectangle, e.OrgImage, out mean, out deviation);

                    _lstMainteCtrl[useRoiNum].glay(mean.D);
                }

            }
            catch (HOperatorException)
            {
            }
        }

        //V1058 メンテナンス追加 yuasa 20190126：どのROIを使用するか選択する
        private int selectRoiNum(int Index)
        {
            int RoiEnableNum = _lstMainteCtrl.Count(x => x.UserContData.ControlEnable);//_lstMainteCtrlの中でControlEnableになっているオブジェクトの数。２カメラこだと2になる。
            int useRoiNum = Index;
            if (RoiEnableNum == 1)//１カメラの場合（ROIの有効数が1）
            {
                if (Index == 1)//右上画面の処理（カメラ番号が1）
                {
                    useRoiNum = 0;
                }
            }
            else if (RoiEnableNum == 2)//２カメラの場合（ROIの有効数が2）
            {
                if (Index == 1)//右上画面の処理（カメラ番号が1）
                {
                    useRoiNum = 0;
                }
                else if (Index == 2 || Index == 3)//左下もしくは右下画面の処理（カメラ番号が2または3）
                {
                    useRoiNum = 1;
                }
            }
            return useRoiNum;
        }

        private bool saveImage( GrabbedImageEventArgs e )
        {
            if (chkSaveImage.Checked)
            {
                if (_eWindowDisplay == EWindowDisplay.CameraAll
                    || e.Index == (int)_eWindowDisplay)
                {

                    LineCameraSheetSystem.SystemParam sysparam = LineCameraSheetSystem.SystemParam.GetInstance();
                    string sPath = sysparam.ImageSaveFolder;
                    sPath += (sPath[sPath.Length - 1] != '\\') ? "\\" : "";
                    sPath += "cam" + (e.Index + 1).ToString() + "\\";

                    if (!System.IO.Directory.Exists(sPath))
                    {
                        System.IO.Directory.CreateDirectory(sPath);
                    }


                    try
                    {
                        // 画像保存を行う
                        HOperatorSet.WriteImage(e.OrgImage, "bmp", 0, sPath + "cam" + (e.Index + 1).ToString() + "_" + DateTime.Now.ToString("yyyyMMddhhmmssfff"));

                    }
                    catch (HOperatorException oe)
                    {
                        LogingDllWrap.LogingDll.Loging_SetLogString(oe.Message);
                        return false;
                    }
                }
            }

            return true;
        }

        void frmAdjust_OnGrabbedImage(object sender, GrabbedImageEventArgs e)
        {
            HalconCameraBase camera = (HalconCameraBase)sender;
            if (camera.Index >= 4)
                return;

            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    frmAdjust_OnGrabbedImage(sender, e);
                }));
                return;
            }

            // 画像を保存しておく
            if (_ahoGrabbedImage[camera.Index] != null)
                _ahoGrabbedImage[camera.Index].Dispose();

            HOperatorSet.CopyObj(e.OrgImage, out _ahoGrabbedImage[camera.Index], 1, -1);

            saveImage(e);
            updateWindowMono(e);

            if (_eWindowDisplay == EWindowDisplay.CameraAll)
            {
                clsHalconWindowControlLite.LockGraphic(_lstWndCtrls[e.Index]);
                clsHalconWindowControlLite.FittingImage(e.OrgImage, _lstWndCtrls[e.Index]);
                
                
            }
            else
            {
                clsHalconWindowControlLite.LockGraphic(hWindowControlMono);
            }

            if (_bDoPosCalib)
            {
                progressPosCalib(e);
            }
            else if (_bDoResCalib)
            {
                progressResCalib(e);
            }
            else
            {
                progressImage(e);
            }

            drawRois(e);

            if (_eWindowDisplay == EWindowDisplay.CameraAll)
            {
                clsHalconWindowControlLite.UnlockGraphic(_lstWndCtrls[e.Index]);
            }
            else
            {
                clsHalconWindowControlLite.UnlockGraphic(hWindowControlMono);
            }

            updateFrameCount(e);
        }

        bool _bDispResArea = false;
        bool _bDispPosArea1 = false;
        bool _bDispPosAreaN = false;
        bool _bDispMainteArea = false; //V1058 メンテナンス追加 yuasa 20190125：メンテナンスROI表示有無用

        int _iImageWidth = 0;
        int _iImageHeight = 0;

        void _hWndCtrl_Repaint(object sender, RepaintEventArgs e) //拡大表示の時に呼ばれる
        {
            if (!_bDispResArea && !_bDispPosArea1 && !_bDispPosAreaN && !_bDispMainteArea) //V1058 メンテナンス追加 yuasa 20190125：条件にメンテナンスROI表示有無を追加
                return;
            try
            {
                if (_bDispResArea)
                {
                    int iCol1 = (int)nudResInspectOffset.Value;
                    int iCol2 = ((int)nudResInspectWidth.Value) == 0 ? _iImageWidth : iCol1 + (int)nudResInspectWidth.Value;
                    HOperatorSet.SetDraw(e.HalconWindowID, "margin");
                    HOperatorSet.SetColor(e.HalconWindowID, "cyan");
                    HOperatorSet.DispRectangle1(e.HalconWindowID, 0, iCol1, _iImageHeight, iCol2);
                }
                if (_bDispPosArea1)
                {
                    int iCol1 = (int)nudInspectOffset1.Value;
                    int iCol2 = ((int)nudInspectWidth1.Value) == 0 ? _iImageWidth : iCol1 + (int)nudInspectWidth1.Value;
                    HOperatorSet.SetDraw(e.HalconWindowID, "margin");
                    HOperatorSet.SetColor(e.HalconWindowID, "magenta");
                    HOperatorSet.DispRectangle1(e.HalconWindowID, 0, iCol1, _iImageHeight, iCol2);
                }
                if (_bDispPosAreaN)
                {
                    int iCol1 = (int)nudInspectOffsetN.Value;
                    int iCol2 = ((int)nudInspectWidthN.Value) == 0 ? _iImageWidth : iCol1 + (int)nudInspectWidthN.Value;
                    HOperatorSet.SetDraw(e.HalconWindowID, "margin");
                    HOperatorSet.SetColor(e.HalconWindowID, "magenta");
                    HOperatorSet.DispRectangle1(e.HalconWindowID, 0, iCol1, _iImageHeight, iCol2);
                }
                
                
                if (_bDispMainteArea) //V1058 メンテナンス追加 yuasa 20190125：メンテナンス追加
                {
                    for (int i = 0; i < _lstMainteCtrl.Count; i++)
                    {
                        if (i == (int)_eWindowDisplay)//表示番号が同じならROIを表示
                        {
                            int useRoiNum = selectRoiNum(i);
                            int iCol1 = (int)_lstMainteCtrl[useRoiNum].UserContData.x;
                            int iCol2 = iCol1 + (int)_lstMainteCtrl[useRoiNum].UserContData.w;
                            int iRow1 = (int)_lstMainteCtrl[useRoiNum].UserContData.y;
                            int iRow2 = iRow1 + (int)_lstMainteCtrl[useRoiNum].UserContData.h;

                            //string[] roiColor = { "red", "blue", "green", "pink" };

                            HOperatorSet.SetDraw(e.HalconWindowID, "margin");
                            HOperatorSet.SetColor(e.HalconWindowID, "red");
                            HOperatorSet.DispRectangle1(e.HalconWindowID, iRow1, iCol1, iRow2, iCol2);
                        }
                    }
                }
                
                
            }
            catch (HOperatorException)
            {
            }
        }

        void updateWindowMono(GrabbedImageEventArgs e)
        {
            Action act = new Action(() =>
                {
                    updateWindowMono(e);
                });

            if (_eWindowDisplay != EWindowDisplay.CameraAll)
            {
                if (e.Index == (int)_eWindowDisplay)
                {
                    if (InvokeRequired)
                    {
                        Invoke(act);
                    }
                    else
                    {
                        try
                        {
                            HTuple htWidth, htHeight;
                            HOperatorSet.GetImageSize(e.OrgImage, out htWidth, out htHeight);
                            _iImageWidth = htWidth.I;
                            _iImageHeight = htHeight.I;
                        }
                        catch (HOperatorException)
                        {
                            return;
                        }

                        if (chkResDispInspectArea.Checked && e.Index == (int)_eResCameraNo)
                        {
                            _bDispResArea = true;
                        }
                        else
                        {
                            _bDispResArea = false;
                        }

                        if (chkDispInspectArea.Checked && e.Index == getPosCalibBaseCam())
                        {
                            _bDispPosArea1 = true;
                        }
                        else
                        {
                            _bDispPosArea1 = false;
                        }

                        if (chkDispInspectArea.Checked && e.Index == getPosCalibTargetCam())
                        {
                            _bDispPosAreaN = true;
                        }
                        else
                        {
                            _bDispPosAreaN = false;
                        }


                        //V1058 メンテナンス追加 yuasa 20190125
                        //if (_lstMainteCtrl[e.Index].UserContData.disp)
                        if (chkRoiDisp.Checked)
                        {
                            _bDispMainteArea = true;
                        }
                        else
                        {
                            _bDispMainteArea = false;
                        }


                        _hWndCtrl.addIconicVar(e.OrgImage);
                    }
                }
            }
        }

        void updateResCalibResult(bool bResult, string sMes)
        {
            lblResResult.Text = sMes;
            if (bResult)
            {
                lblResResult.BackColor = Color.Green;
            }
            else
            {
                lblResResult.BackColor = Color.Red;
            }
        }

        void resetResCalibResult()
        {
            lblResResult.Text = "未検査";
            lblResResult.BackColor = SystemColors.ControlDark;
        }

        void updatePosCalibResult(bool bResult, string sMes)
        {
            lblPosCalibResult.Text = sMes;
            if (bResult)
            {
                lblPosCalibResult.BackColor = Color.Green;
            }
            else
            {
                lblPosCalibResult.BackColor = Color.Red;
            }
        }

        void resetPosCalibResult()
        {
            lblPosCalibResult.Text = "未検査";
            lblPosCalibResult.BackColor = SystemColors.ControlDark;
        }

        private void updateControls()
        {
            if( chkHardTrigger.Checked )
            {
                btnSoftTrigger.Enabled = false;
                chkLive.Enabled = false;
                btnRunPosCalib.Enabled = false;
                btnRunResCalib.Enabled = false;
                btnRegPosCalib.Enabled = false;
                btnRegResCalib.Enabled = false;
            }
            else if (chkLive.Checked)
            {
                btnSoftTrigger.Enabled = false;
                chkHardTrigger.Enabled = false;
                btnRunPosCalib.Enabled = false;
                btnRunResCalib.Enabled = false;
                btnRegPosCalib.Enabled = false;
                btnRegResCalib.Enabled = false;
            }
            else if (_bDoPosCalib || _bDoResCalib )
            {
                btnSoftTrigger.Enabled = false;
                chkLive.Enabled = false;
                chkHardTrigger.Enabled = false;
                if( _bDoPosCalib )
                    btnRunResCalib.Enabled = false;
                if (_bDoResCalib)
                    btnRunPosCalib.Enabled = false;
                for (int i = 0; i < _lstPosCalib.Count; i++)
                    _lstPosCalib[i].Enabled = false;
                for (int i = 0; i < _lstResCameras.Count; i++)
                    _lstResCameras[i].Enabled = false;
            }
            else
            {
                if (_bLiveFinishTry)
                {
                    btnSoftTrigger.Enabled = false;
                    chkHardTrigger.Enabled = false;
                    btnRunPosCalib.Enabled = false;
                    btnRunResCalib.Enabled = false;
                    btnRegPosCalib.Enabled = false;
                    chkLive.Enabled = false;
                    for (int i = 0; i < _lstPosCalib.Count; i++)
                    {
                        _lstPosCalib[i].Enabled = false;
                    }
                    for (int i = 0; i < _lstResCameras.Count; i++)
                    {
                        _lstResCameras[i].Enabled = false;
                    }
                }
                else
                {
                    btnSoftTrigger.Enabled = true;
                    chkHardTrigger.Enabled = true;
                    btnRunPosCalib.Enabled = true;
                    btnRunResCalib.Enabled = true;
                    btnRegPosCalib.Enabled = true;
                    chkLive.Enabled = true;
                    for (int i = 0; i < _lstPosCalib.Count; i++)
                    {
                        _lstPosCalib[i].Enabled = true;
                    }
                    for (int i = 0; i < _lstResCameras.Count; i++)
                    {
                        _lstResCameras[i].Enabled = true;
                    }
                }
            }

            lsvPosCalibProg.Columns[1].Text = ((int)_PosCalib.BaseCamNo + 1).ToString();
            lsvPosCalibProg.Columns[2].Text = ((int)_PosCalib.TargetCamNo + 1).ToString();

            for (int i = 0; i < _ardoTargetCams.Length; i++)
            {
                if (i == getPosCalibBaseCam())
                {
                    _ardoTargetCams[i].Enabled = false;
                }
                else
                {
                    _ardoTargetCams[i].Enabled = true;
                }
            }

            if (_PosCalib.DoneMeas && _bDoneCalibPosCam)
            {
                btnRegPosCalib.Enabled = true;
            }
            else
            {
                btnRegPosCalib.Enabled = false;
            }

            if (_ResCalib2.IsResultSuccess && _bDoneCalibResCam)
            {
                btnRegResCalib.Enabled = true;
            }
            else
            {
                btnRegResCalib.Enabled = false;
            }
        }

        private void btnSoftTrigger_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < APCameraManager.getInstance().CameraNum; i++)
            {
                if (i >= 4)
                    break;

                if (_lstCamCtrl[i].EnableCamera )
                {
                    HalconCameraBase camera = APCameraManager.getInstance().GetCamera(i);
                    camera.GrabAsync(1000);
                }
            }

            for (int i = 0; i < APCameraManager.getInstance().CameraNum; i++)
            {
                if (i >= 4)
                    break;

                if (_lstCamCtrl[i].EnableCamera)
                {
                    HObject hoImg = null;
                    try
                    {
                        HalconCameraBase camera = APCameraManager.getInstance().GetCamera(i);
                        camera.WaitGrabAsync(out hoImg);

                        // 画像を保存しておく
                        if (_ahoGrabbedImage[camera.Index] != null)
                            _ahoGrabbedImage[camera.Index].Dispose();
                        HOperatorSet.CopyObj(hoImg, out _ahoGrabbedImage[camera.Index], 1, -1);
                        clsHalconWindowControlLite.FittingImage(hoImg, _lstWndCtrls[camera.Index]);
                    }
                    catch (HOperatorException)
                    {
                    }
                    finally
                    {
                        if( hoImg != null )
                            hoImg.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// フォームが閉じられるとき
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmAdjust_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (chkHardTrigger.Checked 
                || chkLive.Checked 
                || _bDoPosCalib 
                || APCameraManager.getInstance().IsLive())
            {
                e.Cancel = true;
                return;
            }

            // 照明が点灯していた場合消灯する
            LightControlManager.getInstance().AllLightOff();

            //SetGrabbedImageEventをリセットする
            ResetGrabbedImageEvent();

            string sAppPath = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\') + 1);
            saveParameters(sAppPath + ADJUSTMENT_SETTING_FILE);
        }

        private void ResetGrabbedImageEvent()
        {
            // カメラ
            for (int i = 0; i < APCameraManager.getInstance().CameraNum; i++)
            {
                if (i >= _lstCamCtrl.Count)
                    break;

                APCameraManager.getInstance().ResetGrabbedImageEvent(i, frmAdjust_OnGrabbedImage);
               
            }
        }

        bool _bRecursiveCheckedChanged = false;
        bool _bLiveFinishTry = false;
        private void chkLive_CheckedChanged(object sender, EventArgs e)
        {
            // 再帰時抜ける
            if (_bRecursiveCheckedChanged)
                return;

            CheckBox chk = (CheckBox)sender;
            // ライブ移行でまだライブ状態が終了していないライブ状態にしない
            if (chk.Checked)
            {
                if (APCameraManager.getInstance().IsLive())
                {
                    _bRecursiveCheckedChanged = true;
                    chk.Checked = false;
                    _bRecursiveCheckedChanged = false;
                    return;
                }
            }

            int iCnt = _lstCamCtrl.Count(x => x.EnableCamera);
            if (chk.Checked)
            {
                int[] aiTargetCam = new int[iCnt];
                int n = 0;
                for (int i = 0; i < _lstCamCtrl.Count; i++)
                {
                    if (_lstCamCtrl[i].EnableCamera)
                    {
                        aiTargetCam[n] = i;
                        n++;
                    }
                }
                APCameraManager.getInstance().SyncStartLive(aiTargetCam);
            }
            else
            {
                LiveFinishAction();
            }

            updateControls();
        }

        void frmAdjust_LiveFinishComplete(object sender, EventArgs e)
        {
            _bLiveFinishTry = false;
            APCameraManager.getInstance().LiveFinishComplete -= frmAdjust_LiveFinishComplete;

            Action act = new Action(() =>
                {
                    updateControls();
                });
            if (InvokeRequired)
            {
                Invoke(act);
            }
            else
            {
                act.Invoke();
            }
        }

        private void chkHardTrigger_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            int iCnt = _lstCamCtrl.Count( x => x.EnableCamera );
            if (chk.Checked)
            {
                int [] aiTargetCam = new int[iCnt];
                int n = 0 ;
                for (int i = 0; i < _lstCamCtrl.Count; i++)
                {
                    if( _lstCamCtrl[i].EnableCamera )
                    {
                        aiTargetCam[n] = i;
                        n++;
                    }
                }
                APCameraManager.getInstance().StartHardTrig(aiTargetCam);
            }
            else
            {
                APCameraManager.getInstance().StopHardTrig();
            }
            updateControls();
        }

        private void frmAdjust_Load(object sender, EventArgs e)
        {
            initContols();

            updatePosCalibDatas(false);
            updateResCalibDatas(false);

            updateControls();

            _hWndCtrl.SetBackColor();
        }

        private void initContols()
        {
            if (_OffsetParamContainer != null)
            {
                double dPosX = 0d, dPosY = 0d;
                _OffsetParamContainer.GetOffsetParameter(_PosCalib.TargetCamNo, ref dPosX, ref dPosY);
                lblOffsetXCamNow.Text = dPosX.ToString("F6");
                lblOffsetYCamNow.Text = dPosY.ToString("F6");
            }
        }

        void dispResResult(string sMes, bool bSuccess)
        {
            lblResResult.Text = sMes;
            if (bSuccess)
            {
                lblResResult.BackColor = Color.Green;
            }
            else
            {
                lblResResult.BackColor = Color.Red;
            }
        }

        void clearResResult()
        {
            lblResResult.Text = "未検査";
            lblResResult.BackColor = SystemColors.ControlDark;
        }

        private void btnResRun_Click(object sender, EventArgs e)
        {
            updateResCalibDatas(true);

            _ResCalib2.ResultClear();

            // 対応するカメラを実行する
            int [] aiCam = new int[]{(int)_eResCameraNo};
            APCameraManager.getInstance().SyncStartLive(aiCam);

            _bDoneCalibResCam = false;
            _bDoResCalib = true;
        }

        private void btnRegParam_Click(object sender, EventArgs e)
        {
            if (_ResCalib2.IsResultSuccess && _ResolutionParamContainer != null )
            {
                _ResolutionParamContainer.SetResolutionParameter(_eResCameraNo, _ResCalib2.ResultData.ResolutionHorz, _ResCalib2.ResultData.ResolutionVert);
                updateResCalibDatas(false);
            }
        }

        private void btnRegPosCalib_Click(object sender, EventArgs e)
        {
            if (_PosCalib.DoneMeas)
            {
                _OffsetParamContainer.SetOffsetParameter(_PosCalib.BaseCamNo, 0, 0);
                _OffsetParamContainer.SetOffsetParameter(_PosCalib.TargetCamNo, _PosCalib.MeasOffsetX, _PosCalib.MeasOffsetY);
                updatePosCalibDatas(false);
            }
        }

        /// <summary>
        /// 位置伽リブレーションを行う
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRunPosCalib_Click(object sender, EventArgs e)
        {
            if (!_bDoPosCalib)
            {
                updatePosCalibDatas(true);
                int[] aiCams = new int[2] { (int)_PosCalib.BaseCamNo, (int)_PosCalib.TargetCamNo };
                _PosCalib.ClearMeasData();
                readyPosCalib();
                _bDoneCalibPosCam = false;
                _bDoPosCalib = true;
                APCameraManager.getInstance().SyncStartLive(aiCams);
            }
            else
            {
                // キャンセルを行う
                _bDoPosCalib = false;
                updatePosCalibResult(false, "ﾕｰｻﾞｰｷｬﾝｾﾙ");
                LiveFinishAction();
            }

            updateControls();
        }

        /// <summary>
        /// フレームリセットを行う
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFrameReaset_Click(object sender, EventArgs e)
        {
            resetFrameCount();
        }

        /// <summary>
        /// 解像度伽リブレーションを行う
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRunResCalib_Click(object sender, EventArgs e)
        {
            if (!_bDoResCalib)
            {
                updateResCalibDatas(true);
                _ResCalib2.ResultClear();
                // ライブをスタートする
                int[] aiCam = new int[] {(int)_eResCameraNo };
                APCameraManager.getInstance().SyncStartLive(aiCam);
                _bDoneCalibResCam = false;
                _bDoResCalib = true;
                updateResCalibResult(true, "実行中");
                updateControls();
            }
            else
            {
                // 中止する
                _bDoResCalib = false;
                updateResCalibResult(false, "ﾕｰｻﾞｷｬﾝｾﾙ");
                _ResCalib2.ResultClear();

                LiveFinishAction();
                updateControls();
            }
        }

        /// <summary>
        /// クロス線表示非表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDispCrossLine_CheckedChanged(object sender, EventArgs e)
        {
            _hWndCtrl.CenterLine = chkDispCrossLine.Checked;
        }

        /// <summary>
        /// 2値化表示非表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkBinDisp_CheckedChanged(object sender, EventArgs e)
        {
            _hWndCtrl.DispBin = chkBinDisp.Checked;
            _hWndCtrl.LowThreshold = (int)nudBinDispThresholdLow.Value;
            _hWndCtrl.HighThreshold = (int)nudBinDispThresholdHigh.Value;
        }

        /// <summary>
        /// 2値化閾値変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Threshold_ValueChanged(object sender, EventArgs e)
        {
            _hWndCtrl.HighThreshold = (int)nudBinDispThresholdHigh.Value;
            _hWndCtrl.LowThreshold = (int)nudBinDispThresholdLow.Value;
        }


        //V1058 メンテナンス追加 yuasa 20190125：
        /// <summary>
        /// 基準値に設定する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStdSetting_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("現在値を基準値として登録しますか？\n注）基準値設定は導入時に行って下さい。", "", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                //グレー値（現在値）をグレー値（基準値）へコピー
                for (int i = 0; i < _lstMainteCtrl.Count; i++)
                {
                    _lstMainteCtrl[i].CopyStdGrayValue();
                }
                //照明値（現在値）を照明値（基準値）へコピー
                for (int i = 0; i < _lstLightCtrl.Count; i++)
                {
                    _lstLightCtrl[i].CopyStdLightValue();
                    //_lstMainteLightCtrl[i].CopyStdLightValue(_lstLightCtrl[i].Value.ToString());
                }
            }
        }

        //V1058 メンテナンス追加 yuasa 20190125：
        /// <summary>
        /// 設定を保存する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
                //SaveFileDialogクラスのインスタンスを作成
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "txtファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
                sfd.FilterIndex = 1;
                sfd.Title = "保存先のファイルを選択してください";

                //ダイアログを表示する
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        IniFileAccess ifa = new IniFileAccess();
                        //カメラ部分
                        for (int i = 0; i < _lstMainteCtrl.Count; i++)
                        {
                            ifa.SetIni("Setting_Camera" + (i + 1).ToString(), "Gain", _lstCamCtrl[i].Gain, sfd.FileName);
                            ifa.SetIni("Setting_Camera" + (i + 1).ToString(), "Offset", _lstCamCtrl[i].Offset, sfd.FileName);
                            ifa.SetIni("Setting_Camera" + (i + 1).ToString(), "ExposureTime", _lstCamCtrl[i].ExposureTime, sfd.FileName);

                            ifa.SetIni("Setting_Camera" + (i + 1).ToString(), "ControlEnable", _lstMainteCtrl[i].UserContData.ControlEnable, sfd.FileName);
                            ifa.SetIni("Setting_Camera" + (i + 1).ToString(), "X", _lstMainteCtrl[i].UserContData.x, sfd.FileName);
                            ifa.SetIni("Setting_Camera" + (i + 1).ToString(), "Y", _lstMainteCtrl[i].UserContData.y, sfd.FileName);
                            ifa.SetIni("Setting_Camera" + (i + 1).ToString(), "W", _lstMainteCtrl[i].UserContData.w, sfd.FileName);
                            ifa.SetIni("Setting_Camera" + (i + 1).ToString(), "H", _lstMainteCtrl[i].UserContData.h, sfd.FileName);
                            ifa.SetIni("Setting_Camera" + (i + 1).ToString(), "nowValue", _lstMainteCtrl[i].UserContData.nowValue, sfd.FileName);
                            ifa.SetIni("Setting_Camera" + (i + 1).ToString(), "StdGrayValue", _lstMainteCtrl[i].UserContData.stdGrayValue, sfd.FileName);
                        }
                        //照明部分
                        ifa.SetIni("Setting_Light", "LightEnable1", uclLightControl1.Enable, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightControlEnable1", uclLightControl1.ControlEnable, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightValue1", uclLightControl1.Value, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightStdValue1", uclLightControl1.StdValue, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightDifference1", uclLightControl1.Difference, sfd.FileName);

                        ifa.SetIni("Setting_Light", "LightEnable2", uclLightControl2.Enable, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightControlEnable2", uclLightControl2.ControlEnable, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightValue2", uclLightControl2.Value, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightStdValue2", uclLightControl2.StdValue, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightDifference2", uclLightControl2.Difference, sfd.FileName);

                        ifa.SetIni("Setting_Light", "LightEnable3", uclLightControl3.Enable, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightControlEnable3", uclLightControl3.ControlEnable, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightValue3", uclLightControl3.Value, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightStdValue3", uclLightControl3.StdValue, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightDifference3", uclLightControl3.Difference, sfd.FileName);

                        ifa.SetIni("Setting_Light", "LightEnable4", uclLightControl4.Enable, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightControlEnable4", uclLightControl4.ControlEnable, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightValue4", uclLightControl4.Value, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightStdValue4", uclLightControl4.StdValue, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightDifference4", uclLightControl4.Difference, sfd.FileName);

                        ifa.SetIni("Setting_Light", "LightEnable5", uclLightControl5.Enable, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightControlEnable5", uclLightControl5.ControlEnable, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightValue5", uclLightControl5.Value, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightStdValue5", uclLightControl5.StdValue, sfd.FileName);
                        ifa.SetIni("Setting_Light", "LightDifference5", uclLightControl5.Difference, sfd.FileName);
                        MessageBox.Show("設定を保存しました。");
                    }
                    catch
                    {
                        MessageBox.Show("設定の保存に失敗しました。");
                    }
                }

        }


        /// <summary>
        /// オフセットを登録する//V1058 メンテナンス追加 yuasa 20190125：
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLightOffset_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("照明のオフセットをSystemParamに反映しますか？", "", MessageBoxButtons.YesNo, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                    SystemParam.GetInstance().MainteLightOffset[0] = int.Parse(uclLightControl1.Difference);
                    SystemParam.GetInstance().MainteLightOffset[1] = int.Parse(uclLightControl2.Difference);
                    SystemParam.GetInstance().MainteLightOffset[2] = int.Parse(uclLightControl3.Difference);
                    SystemParam.GetInstance().MainteLightOffset[3] = int.Parse(uclLightControl4.Difference);
                    SystemParam.GetInstance().MainteLightOffset[4] = int.Parse(uclLightControl5.Difference);
            }
        }

        //V1058 メンテナンス追加 yuasa 20190125：
        /// <summary>
        /// ■■■設定を読み込む■■■
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResSetting_Click(object sender, EventArgs e)
        {
            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "txtファイル(*.txt)|*.txt|すべてのファイル(*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.Title = "開くファイルを選択してください";

            //ダイアログを表示する
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    IniFileAccess ifa = new IniFileAccess();
                    //カメラ部分
                    for (int i = 0; i < _lstMainteCtrl.Count; i++)
                    {
                        _lstCamCtrl[i].Gain = ifa.GetIni("Setting_Camera" + (i + 1).ToString(), "Gain", 0, ofd.FileName);
                        _lstCamCtrl[i].Offset = ifa.GetIni("Setting_Camera" + (i + 1).ToString(), "Offset", 0, ofd.FileName);
                        _lstCamCtrl[i].ExposureTime = ifa.GetIni("Setting_Camera" + (i + 1).ToString(), "ExposureTime", 0, ofd.FileName);

                        _lstMainteCtrl[i].UserContData.x = ifa.GetIni("Setting_Camera" + (i + 1).ToString(), "X", 0, ofd.FileName);
                        _lstMainteCtrl[i].UserContData.y = ifa.GetIni("Setting_Camera" + (i + 1).ToString(), "Y", 0, ofd.FileName);
                        _lstMainteCtrl[i].UserContData.w = ifa.GetIni("Setting_Camera" + (i + 1).ToString(), "W", 1, ofd.FileName);
                        _lstMainteCtrl[i].UserContData.h = ifa.GetIni("Setting_Camera" + (i + 1).ToString(), "H", 1, ofd.FileName);
                        _lstMainteCtrl[i].UserContData.stdGrayValue = ifa.GetIni("Setting_Camera" + (i + 1).ToString(), "StdGrayValue", "", ofd.FileName);

                    }
                    //照明部分
                    uclLightControl1.StdValue = ifa.GetIni("Setting_Light", "LightStdValue1", "", ofd.FileName);

                    uclLightControl2.StdValue = ifa.GetIni("Setting_Light", "LightStdValue2", "", ofd.FileName);

                    uclLightControl3.StdValue = ifa.GetIni("Setting_Light", "LightStdValue3", "", ofd.FileName);

                    uclLightControl4.StdValue = ifa.GetIni("Setting_Light", "LightStdValue4", "", ofd.FileName);

                    uclLightControl5.StdValue = ifa.GetIni("Setting_Light", "LightStdValue5", "", ofd.FileName);
                    //カメラ部　表示の更新
                    for (int i = 0; i < _lstMainteCtrl.Count; i++)
                        _lstMainteCtrl[i].userContDataUpdate();
                    //照明部　差の更新
                    for (int i = 0; i < _lstLightCtrl.Count; i++)
                    {
                        _lstLightCtrl[i].DifferenceCalc();
                    }

                    MessageBox.Show("設定を読み込みました。");
                }
                catch
                {
                    MessageBox.Show("設定の読み込みに失敗しました。");
                }
            }
        }

    }
}
