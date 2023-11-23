using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using HalconDotNet;
using ViewROI;
using HalconCamera;

using Fujita.HalconMisc;
using Fujita.Misc;
using Fujita.LightControl;

using PylonC.NET;

namespace Fujita.InspectionSystem
{
    public partial class frmCameraTest : System.Windows.Forms.Form, IFormForceCancel, IMainFormEqu, ICallbackRoiRectangle1
    {
        public string MainteDirectory { get; set; }
        public bool VisibleWhiteB { get; set; }

        private int COLUMN_NUM_MEAN = 1;
        private int COLUMN_NUM_DEVIATION = 2;
        private int COLUMN_NUM_LINEDEV_HORZ = 3;
        private int COLUMN_NUM_LINEMAX_HORZ = 4;
        private int COLUMN_NUM_LINEMIN_HORZ = 5;
        private int COLUMN_NUM_LINEDEV_VERT = 6;
        private int COLUMN_NUM_LINEMAX_VERT = 7;
        private int COLUMN_NUM_LINEMIN_VERT = 8;
        private int COLUMN_NUM_FOCUS = 9;
        private int COLUMN_NUM_FOCUS_MAX = 10;

        clsNUDTenkeyer _tenkeyer;

        public frmCameraTest()
        {
            InitializeComponent();

            _tenkeyer = new clsNUDTenkeyer(Controls);
            _tenkeyer.TenkeyOn = true;
            _tenkeyer.PositionX = Screen.PrimaryScreen.Bounds.Width - _tenkeyer.Width;
            _tenkeyer.PositionY = Screen.PrimaryScreen.Bounds.Height - _tenkeyer.Height;

            rdoWindows = new RadioButton[]
            {
                rdoWindow1,
                rdoWindow2,
                rdoWindow3,
                rdoWindow4
            };

			uclHWinMulti = new uclHWindowMulti[]
			{
				hWindowMulti1,
				hWindowMulti2,
				hWindowMulti3
			};

            uclCameraControls = new uclCameraControl[] 
            { 
                uclCameraControl1, 
                uclCameraControl2, 
                uclCameraControl3, 
                uclCameraControl4,
                uclCameraControl5, 
                uclCameraControl6, 
                uclCameraControl7, 
                uclCameraControl8,
                uclCameraControl9,
                uclCameraControl10,
                uclCameraControl11,
                uclCameraControl12
            };

            _lstLightControls = new List<uclLightControl>();
            _lstLightControls.Add(uclLightControl1);
            _lstLightControls.Add(uclLightControl2);
            _lstLightControls.Add(uclLightControl3);
            _lstLightControls.Add(uclLightControl4);
            _lstLightControls.Add(uclLightControl5);
            _lstLightControls.Add(uclLightControl6);
            _lstLightControls.Add(uclLightControl7);
            _lstLightControls.Add(uclLightControl8);

            rdoDivide.CheckedChanged += new EventHandler(rdoWindow_CheckedChanged);
            rdoWindow1.CheckedChanged += new EventHandler(rdoWindow_CheckedChanged);
            rdoWindow2.CheckedChanged += new EventHandler(rdoWindow_CheckedChanged);
            rdoWindow3.CheckedChanged += new EventHandler(rdoWindow_CheckedChanged);
            rdoWindow4.CheckedChanged += new EventHandler(rdoWindow_CheckedChanged);

            for (int i = 0; i < chkStatics.Items.Count; i++)
            {
                _lstTests.Add(chkStatics.GetItemChecked(i));
            }

            _ahoLastCaptureImages = new HObject[ CameraManager.getInstance().CameraNum ];
        }

        private RadioButton[] rdoWindows;
		private uclHWindowMulti[] uclHWinMulti;
        private uclCameraControl[] uclCameraControls;
        private List<uclLightControl> _lstLightControls;
        List<bool> _lstTests = new List<bool>();

        public void ForceCancel()
        {
            Action act = new Action(() =>
                {
                    if (chkLive.Checked)
                    {
                        Live(false);
                    }
                    if (chkHardTriggerWait.Checked)
                    {
                        HardTrigger(false);
                    }
                    termControls();
                    Close();
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

        private void frmCameraTest_Load(object sender, EventArgs e)
        {
            initialControls();
            MainteData2Disp();
        }

		private void initWindow()
		{
			int cameraNum = CameraManager.getInstance().CameraNum;
			if (cameraNum <= 4)
			{
				uclHWinMulti[0].WindowNum = cameraNum;
				uclHWinMulti[1].WindowNum = 0;
				uclHWinMulti[2].WindowNum = 0;
				tabCamera.TabPages.RemoveAt(2);
				tabCamera.TabPages.RemoveAt(1);
				tabSettings.TabPages.RemoveAt(2);
				tabSettings.TabPages.RemoveAt(1);
			}
			else if (cameraNum <= 8)
			{
				uclHWinMulti[0].WindowNum = 4;
				uclHWinMulti[1].WindowNum = cameraNum - 4;
				uclHWinMulti[2].WindowNum = 0;
				tabCamera.TabPages.RemoveAt(2);
				tabSettings.TabPages.RemoveAt(2);
				tabCamera.SelectedIndex = 1;
			}
			else
			{
				uclHWinMulti[0].WindowNum = 4;
				uclHWinMulti[1].WindowNum = 4;
				uclHWinMulti[2].WindowNum = cameraNum - 8;
				tabCamera.SelectedIndex = 2;
				tabCamera.SelectedIndex = 1;
			}
			tabCamera.SelectedIndex = 0;
			ChangeWindow();
		}

        private double[] _fcNowValues;
        private int[] _wbGrayNowValue;
        private int[] _fcGrayNowValue;

        private void initCameraControl()
        {
			initWindow();

            _fcNowValues = new double[CameraManager.getInstance().CameraNum];
            _wbGrayNowValue = new int[CameraManager.getInstance().CameraNum];
            _fcGrayNowValue = new int[CameraManager.getInstance().CameraNum];

            for (int i = 0; i < CameraManager.getInstance().CameraNum; i++)
            {
                if (i >= uclCameraControls.Length)
                    break;

                HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(i);
                uclCameraControls[i].SetCamera(camera);
                uclCameraControls[i].GainChanged += new GainChangedEventHandler(frmCameraTest_GainChanged);
                uclCameraControls[i].OffsetChanged += new OffsetChangedEventHandler(frmCameraTest_OffsetChanged);
                uclCameraControls[i].ExposureChanged += new ExposureChangedEventHandler(frmCameraTest_ExposureChanged);
                uclCameraControls[i].LineRateChanged += FrmCameraTest_LineRateChanged;
                uclCameraControls[i].EnableCameraCheckedChange += new EnableCameraCheckedChangeEventHandler(frmCameraTest_EnableCameraCheckedChange);
                uclCameraControls[i].CameraFcRoiButtonClicked += FrmCameraTest_CameraFcRoiButtonClicked;
                uclCameraControls[i].CameraFcEntryButtonClicked += FrmCameraTest_CameraFcEntryButtonClicked;
                uclCameraControls[i].CameraWhiteBRoiButtonClicked += FrmCameraTest_CameraWhiteBRoiButtonClicked;
                uclCameraControls[i].CameraWhiteBRunButtonClicked += FrmCameraTest_CameraWhiteBRunButtonClicked;
                uclCameraControls[i].CameraWhiteBEntryButtonClicked += FrmCameraTest_CameraWhiteBEntryButtonClicked;
                uclCameraControls[i].CameraWhiteBResetButtonClicked += FrmCameraTest_CameraWhiteBResetButtonClicked;
                uclCameraControls[i].CameraFcChangeLightButtonClicked += FrmCameraTest_CameraFcChangeLightButtonClicked;
                uclCameraControls[i].CameraWhiteBChangeLightButtonClicked += FrmCameraTest_CameraWhiteBChangeLightButtonClicked;
                camera.OnGrabbedImage += new HalconCamera.GrabbedImageEventHandler(camera_OnGrabbedImage);
                uclCameraControls[i].EnableCamera = true;
                uclCameraControls[i].VisibleWhiteB = this.VisibleWhiteB;

                int iRowIndex = dataGridView1.Rows.Add();
                dataGridView1[0, iRowIndex].Value = "カメラ" + (i + 1).ToString();
            }

            for (int i = CameraManager.getInstance().CameraNum; i < 9; i++)
            {
                uclCameraControls[i].Enabled = false;
            }

			int cameraNum = CameraManager.getInstance().CameraNum;
			for (int camInex = 0; camInex < cameraNum; camInex++)
			{
				int winMultiIndex = camInex / 4;
				int winIndex = camInex % 4;
				string winName = CameraManager.getInstance().GetCamera(camInex).Name;
				uclHWinMulti[winMultiIndex].SetWindowName(winIndex, winName);
				HWndCtrl ctrl = uclHWinMulti[winMultiIndex].GetWindowControl(winIndex);
				ctrl.Repaint += new RepaintEventHandler(ctrl_Repaint);
				ctrl.Fitting = true;
			}
		}

        private void termCameraControl()
        {
            for (int i = 0; i < CameraManager.getInstance().CameraNum; i++)
            {
                HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(i);
                if (i >= uclCameraControls.Length)
                    break;
                uclCameraControls[i].GainChanged -= new GainChangedEventHandler(frmCameraTest_GainChanged);
                uclCameraControls[i].OffsetChanged -= new OffsetChangedEventHandler(frmCameraTest_OffsetChanged);
                uclCameraControls[i].ExposureChanged -= new ExposureChangedEventHandler(frmCameraTest_ExposureChanged);
                uclCameraControls[i].LineRateChanged -= FrmCameraTest_LineRateChanged;
                uclCameraControls[i].EnableCameraCheckedChange -= new EnableCameraCheckedChangeEventHandler(frmCameraTest_EnableCameraCheckedChange);
                uclCameraControls[i].CameraFcRoiButtonClicked -= FrmCameraTest_CameraFcRoiButtonClicked;
                uclCameraControls[i].CameraFcEntryButtonClicked -= FrmCameraTest_CameraFcEntryButtonClicked;
                uclCameraControls[i].CameraWhiteBRoiButtonClicked -= FrmCameraTest_CameraWhiteBRoiButtonClicked;
                uclCameraControls[i].CameraWhiteBRunButtonClicked -= FrmCameraTest_CameraWhiteBRunButtonClicked;
                uclCameraControls[i].CameraWhiteBEntryButtonClicked -= FrmCameraTest_CameraWhiteBEntryButtonClicked;
                uclCameraControls[i].CameraWhiteBResetButtonClicked -= FrmCameraTest_CameraWhiteBResetButtonClicked;
                uclCameraControls[i].CameraFcChangeLightButtonClicked -= FrmCameraTest_CameraFcChangeLightButtonClicked;
                uclCameraControls[i].CameraWhiteBChangeLightButtonClicked -= FrmCameraTest_CameraWhiteBChangeLightButtonClicked;
                camera.OnGrabbedImage -= new HalconCamera.GrabbedImageEventHandler(camera_OnGrabbedImage);
                uclCameraControls[i].ResetCamera();

                uclCameraControls[i].Enabled = false;

            }

			int cameraNum = CameraManager.getInstance().CameraNum;
			for (int camInex = 0; camInex < cameraNum; camInex++)
			{
				int winMultiIndex = camInex / 4;
				int winIndex = camInex % 4;
				HWndCtrl ctrl = uclHWinMulti[winMultiIndex].GetWindowControl(winIndex);
				ctrl.Repaint -= new RepaintEventHandler(ctrl_Repaint);
			}
		}

        private void initialControls()
        {
            initCameraControl();

            for (int i = 0; i < LightControlManager.getInstance().LightCount; i++)
            {
                if (i >= _lstLightControls.Count)
                    break;

                LightType lt = LightControlManager.getInstance().GetLight(i);
                _lstLightControls[i].LigthName = lt.LightName;
                _lstLightControls[i].LightValueMin = lt.ValueMin;
                _lstLightControls[i].LightValueMax = lt.ValueMax;
                _lstLightControls[i].LightValue = clsMainteFunc.getInstance().LightParam[i].CalcLightValue;

                _lstLightControls[i].LightEnableChanged += new uclLightControl.LightEnableChangedEventHandler(frmCameraTest_LightEnableChanged);
                _lstLightControls[i].LightValueChanged += new uclLightControl.LightValueChangedEventHadler(frmCameraTest_LightValueChanged);
                _lstLightControls[i].LightRoiButtonClicked += new uclLightControl.LightRoiButtonClickedEventHandler(FrmCameraTest_LightRoiButtonClicked);
                _lstLightControls[i].LightDiffButtonClicked += FrmCameraTest_LightDiffButtonClicked;
            }

            for (int i = LightControlManager.getInstance().LightCount; i < _lstLightControls.Count; i++)
            {
                _lstLightControls[i].Enabled = false;
            }

			int cameraNum = CameraManager.getInstance().CameraNum;
			for (int camInex = 0; camInex < cameraNum; camInex++)
			{
				int winMultiIndex = camInex / 4;
				int winIndex = camInex % 4;
				uclHWinMulti[winMultiIndex].GetWindowControl(winIndex).CenterLine = chkCenterLine.Checked;
				uclHWinMulti[winMultiIndex].GetWindowControl(winIndex).GridLine= chkGridLine.Checked;
			}
        }

        private void termControls()
        {
            // 照明をすべて消灯する
            LightControlManager.getInstance().AllLightOff();

            // イメージをすべて削除する
            for (int i = 0; i < _ahoLastCaptureImages.Length; i++)
            {
                HalconExtFunc.Clear(ref _ahoLastCaptureImages[i]);
            }

            termCameraControl();
        }


        void frmCameraTest_LightValueChanged(object sender, uclLightControl.LightValueChangedEventArgs e)
        {
            int iIndex = _lstLightControls.IndexOf((uclLightControl)sender);
            LightType lt = LightControlManager.getInstance().GetLight(iIndex);
            if (e.Enable)
                lt.LightOn(e.Value + clsMainteFunc.getInstance().LightParam[iIndex].DiffLightValue, true);
        }

        void frmCameraTest_LightEnableChanged(object sender, uclLightControl.LightEnableChangedEventArgs e)
        {
            int iIndex = _lstLightControls.IndexOf((uclLightControl)sender);
            LightType lt = LightControlManager.getInstance().GetLight(iIndex);
            if (e.Enable)
                lt.LightOn(e.Value + clsMainteFunc.getInstance().LightParam[iIndex].DiffLightValue, true);
            else
                lt.LightOff();
        }

        private void Live(bool bStart)
        {
            if (bStart)
            {
                for (int i = 0; i < CameraManager.getInstance().CameraNum; i++)
                {
                    if (i >= uclCameraControls.Length)
                        break;
                    HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(i);
                    if (uclCameraControls[i].EnableCamera)
                    {
                        uclCameraControls[i].StartFrameRate();
                        camera.Live(true);
                    }
                    else
                    {
                        uclCameraControls[i].StopFrameRate();
                        camera.Live(false);
                    }
                }
            }
            else
            {
                for (int i = 0; i < CameraManager.getInstance().CameraNum; i++)
                {
                    if (i >= uclCameraControls.Length)
                        break;
                    HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(i);
                    uclCameraControls[i].StopFrameRate();
                    camera.Live(false);
                }
            }
        }

        private void HardTrigger(bool bStart)
        {
            if (bStart)
            {
                for (int i = 0; i < CameraManager.getInstance().CameraNum; i++)
                {
                    if (i >= uclCameraControls.Length)
                        break;
                    HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(i);
                    if (uclCameraControls[i].EnableCamera)
                    {
                        camera.SetHardTrigger(true, true);
                    }
                    else
                    {
                        camera.SetHardTrigger(false, true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < CameraManager.getInstance().CameraNum; i++)
                {
                    if (i >= uclCameraControls.Length)
                        break;
                    HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(i);
                    camera.SetHardTrigger(false, true);
                }
            }
        }

        void frmCameraTest_EnableCameraCheckedChange(object sender, EnableCameraCheckedChnageEventArgs e)
        {
            if (chkLive.Checked)
            {
                Live(true);
            }
            if (chkHardTriggerWait.Checked)
            {
                HardTrigger(true);
            }
        }

        void ctrl_Repaint(object sender, RepaintEventArgs e)
        {
            drawProjection(e.HWindowID, _lstTests[2], _lstTests[3], _adHorz, _adVert);
        }

        void frmCameraTest_ExposureChanged(object sender, ExposureChangedEventArgs e)
        {
            HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(e.Index);
            camera.SetExposureTime(e.Value);

            double min, max, step, now;
            min = max = step = now = -1;
            camera.GetLineRateRange(ref min, ref max, ref step, ref now);
            camera.LineRateMax = max;
        }

        void frmCameraTest_OffsetChanged(object sender, OffsetChangedEventArgs e)
        {
            HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(e.Index);
            camera.SetOffset(e.Value);
        }

        void FrmCameraTest_LineRateChanged(object sender, LineRateChangedEventArgs e)
        {
            HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(e.Index);
            camera.SetLineRate(e.Value);

            int min, max, step, now;
            min = max = step = now = -1;
            camera.GetExposureTimeRange(ref min, ref max, ref step, ref now);
            camera.ExposureTimeMax = max;
        }

        void frmCameraTest_GainChanged(object sender, GainChangedEventArgs e)
        {
            HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(e.Index);
            camera.SetGain(e.Value);
        }


        bool _bFocusMaxReset = true;
        double[] _adHorz = null, _adVert = null;

        HObject [] _ahoLastCaptureImages;

        void camera_OnGrabbedImage(object sender, HalconCamera.GrabbedImageEventArgs e)
        {
            HalconCamera.HalconCameraBase camera = sender as HalconCamera.HalconCameraBase;

            if (camera == null)
                return;

            bool bRepaint = false;

            if( rdoDivide.Checked || rdoWindows[(camera.Index%4)].Checked )
            {
                bRepaint = true;
            }

            if (_runWhiteBFlag == true)
            {
                if (_whiteBCamNo == e.Index)
                {
                    _runWhiteBFlag = !CameraManager.getInstance().GetCamera(e.Index).IsWhiteBalanceStop();
                    return;
                }
                return;
            }

            Action act = new Action( () =>
                {
                    try
                    {
                        HTuple htCnt;
                        HOperatorSet.CountObj(e.OrgImage, out htCnt);
                    }
                    catch (HOperatorException)
                    {
                        return;
                    }
                    HalconExtFunc.Clear(ref _ahoLastCaptureImages[camera.Index]);
                    _ahoLastCaptureImages[camera.Index] = e.OrgImage.CopyObj(1, -1);

                    double dMean = 0.0, dDeviation = 0.0, dFocus = 0.0;
                    double dMeanHorz = 0.0, dDevHorz = 0.0, dMeanVert = 0.0, dDevVert = 0.0;
                    double dMaxHorz = 0.0, dMinHorz = 0.0, dMaxVert = 0.0, dMinVert = 0.0;

                    if (_lstTests[0] || _lstTests[1])
                    {
                        Intensity(e.OrgImage, out dMean, out dDeviation);
                        if (_lstTests[0])
                        {
                            dataGridView1[COLUMN_NUM_MEAN, camera.Index].Value = dMean.ToString("F3");
                        }
                        if (_lstTests[1])
                        {
                            dataGridView1[COLUMN_NUM_DEVIATION, camera.Index].Value = dDeviation.ToString("F3");
                        }
                    }

                    if (_lstTests[2] || _lstTests[3])
                    {
                        LineProjection(e.OrgImage, _lstTests[2], _lstTests[3], out _adHorz, out _adVert, out dMeanHorz, out dDevHorz, out dMaxHorz, out dMinHorz, out dMeanVert, out dDevVert, out dMaxVert, out dMinVert );
                    }
                    if (_lstTests[4])
                    {
                        Focus(e.OrgImage, out dFocus);
                        dataGridView1[COLUMN_NUM_FOCUS, camera.Index].Value = dFocus.ToString("F3");
                        double dFocusMax;
                        if (_bFocusMaxReset)
                        {
                            dataGridView1[COLUMN_NUM_FOCUS_MAX, camera.Index].Value = dFocus.ToString("F3");
                            _bFocusMaxReset = false;
                        }
                        else
                        {
                            if (dataGridView1[COLUMN_NUM_FOCUS_MAX, camera.Index].Value != null &&
                                double.TryParse(dataGridView1[COLUMN_NUM_FOCUS_MAX, camera.Index].Value.ToString(), out dFocusMax))
                            {
                                if (dFocusMax < dFocus)
                                {
                                    dataGridView1[COLUMN_NUM_FOCUS_MAX, camera.Index].Value = dFocus.ToString("F3");
                                }
                                _bFocusMaxReset = false;
                            }
                            else
                            {
                                dataGridView1[COLUMN_NUM_FOCUS_MAX, camera.Index].Value = dFocus.ToString("F3");
                            }
                        }
                    }

                    if (_lstTests[2])
                    {
                        dataGridView1[COLUMN_NUM_LINEDEV_HORZ, camera.Index].Value = dDevHorz.ToString("F3");
                        dataGridView1[COLUMN_NUM_LINEMAX_HORZ, camera.Index].Value = dMaxHorz.ToString("F3");
                        dataGridView1[COLUMN_NUM_LINEMIN_HORZ, camera.Index].Value = dMinHorz.ToString("F3");
                    }
                    if (_lstTests[3])
                    {
                        dataGridView1[COLUMN_NUM_LINEDEV_VERT, camera.Index].Value = dDevVert.ToString("F3");
                        dataGridView1[COLUMN_NUM_LINEMAX_VERT, camera.Index].Value = dMaxVert.ToString("F3");
                        dataGridView1[COLUMN_NUM_LINEMIN_VERT, camera.Index].Value = dMinVert.ToString("F3");
                    }

                    uclCameraControls[camera.Index].AdvancedFrameRate();

                    //フォーカスROIのFC値の算出
                    MainteFocusValue(e.OrgImage, camera.Index);
                    //ホワイトバランスROIのGray値の算出
                    MainteWhiteBRoiGray(e.OrgImage, camera.Index);
                    //照明ROIのGray値の算出
                    MainteLightRoiGray(e.OrgImage, camera.Index);

                    if ( bRepaint )
                    {
						int winMultiIndex = camera.Index / 4;
						int winIndex = camera.Index % 4;
						ViewROI.HWndCtrl ctrl = uclHWinMulti[winMultiIndex].GetWindowControl(winIndex);
                        //フォーカス値　および　FC-ROIの表示
                        MainteDisplayFocusRoi(ctrl, camera.Index);
                        //ホワイトバランスROIの表示
                        MainteDisplayWhiteBRoi(ctrl, camera.Index);
                        //照明（ROI）の表示
                        MainteDisplayLightRoi(ctrl, camera.Index);
						ctrl.addIconicVar2(e.OrgImage);
                    }
                });

            if (InvokeRequired)
            {
                Invoke( act );
            }
            else
            {
                act.Invoke();
            }
        }

        private int iProjectionStep = 10;
        private void drawProjection(HWindow hHalconID, bool bHorz, bool bVert, double [] adHorz, double [] adVert )
        {
            double dBase = 50;
            try
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Restart();
                if (bHorz && adHorz != null)
                {
                    HOperatorSet.SetColor(hHalconID, "cyan");
                    HOperatorSet.DispLine(hHalconID, 0, dBase, adHorz.Length, dBase);
                    HOperatorSet.DispLine(hHalconID, 0, dBase + 128, adHorz.Length - 1, dBase + 128);
                    HOperatorSet.DispLine(hHalconID, 0, dBase + 256, adHorz.Length - 1, dBase + 256);

                    HOperatorSet.SetColor(hHalconID, "red");
                    int i;
                    for (i = 0; i < adHorz.Length - iProjectionStep; i += iProjectionStep)
                    {
                        HOperatorSet.DispLine(hHalconID, i, dBase + adHorz[i], i + iProjectionStep, dBase + adHorz[i + iProjectionStep]);
                    }
                    i -= iProjectionStep;
                    HOperatorSet.DispLine(hHalconID, i, dBase + adHorz[i], adHorz.Length - 1, dBase + adHorz[adHorz.Length - 1]);

                }
                if (bVert && adVert != null)
                {
                    HOperatorSet.SetColor(hHalconID, "cyan");
                    HOperatorSet.DispLine(hHalconID, dBase + 256, 0, dBase + 256, adVert.Length - 1);
                    HOperatorSet.DispLine(hHalconID, dBase + 128, 0, dBase + 128, adVert.Length - 1);
                    HOperatorSet.DispLine(hHalconID, dBase + 0, 0, dBase + 0, adVert.Length - 1);

                    HOperatorSet.SetColor(hHalconID, "red");
                    int i;
                    for (i = 0; i < adVert.Length - iProjectionStep; i += iProjectionStep)
                    {
                        HOperatorSet.DispLine(hHalconID, dBase + 256 - adVert[i], i, dBase + 256 - adVert[i + iProjectionStep], i + iProjectionStep);
                    }

                    i -= iProjectionStep;
                    HOperatorSet.DispLine(hHalconID, dBase + 256 - adVert[i], i, dBase + 256 - adVert[adVert.Length-1], adVert.Length-1);
                }
                sw.Stop();
            }
            catch (HOperatorException)
            {
            }
        }

        private void updateWindow(int iIndex, HalconDotNet.HObject img)
        {
        }

        private void movieCheckButton_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLive.Checked)
            {
                Live(true);
            }
            else
            {
                Live(false);
            }

            updateControls();
        }

        private void scanCheckButton_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHardTriggerWait.Checked)
            {
                HardTrigger(true);
            }
            else
            {
                HardTrigger(false);
            }
            updateControls();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void updateControls()
        {
            if (chkLive.Checked)
            {
                chkHardTriggerWait.Enabled = false;
                closeButton.Enabled = false;
                chkDisableShading.Enabled = false;
                btnCreateShadingFile.Enabled = false;
                btnShadingExeute.Enabled = false;
            }
            else if (chkHardTriggerWait.Checked)
            {
                chkLive.Enabled = false;
                closeButton.Enabled = false;
                chkDisableShading.Enabled = false;
                btnCreateShadingFile.Enabled = false;
                btnShadingExeute.Enabled = false;
            }
            else
            {
                chkLive.Enabled = true;
                chkHardTriggerWait.Enabled = true;
                closeButton.Enabled = true;
                chkDisableShading.Enabled = true;
                btnCreateShadingFile.Enabled = true;
                btnShadingExeute.Enabled = true;
            }
        }


        private void rdoWindow_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = (RadioButton)sender;
			if (rdo.Checked == false)
				return;

			ChangeWindow();
#if false
			for (int i = 0; i < uclHWinMulti.Length; i++)
			{
				if (uclHWinMulti[i] == null)
					continue;

				switch (rdo.Name)
				{
					case "rdoDivide":
						uclHWinMulti[i].LayoutDefault();
						break;
					case "rdoWindow1":
						uclHWinMulti[i].LayoutOne(0);
						break;
					case "rdoWindow2":
						uclHWinMulti[i].LayoutOne(1);
						break;
					case "rdoWindow3":
						uclHWinMulti[i].LayoutOne(2);
						break;
					case "rdoWindow4":
						uclHWinMulti[i].LayoutOne(3);
						break;
				}
			}
#endif
        }

		private void ChangeWindow()
		{
			if (rdoDivide.Checked == true)
			{
				uclHWinMulti[tabCamera.SelectedIndex].LayoutDefault();
			}
			else
			{
				int camNo = 0;
				foreach (RadioButton rdo in rdoWindows)
				{
					if (rdo.Checked == true)
					{
						break;
					}
					camNo++;
				}
				uclHWinMulti[tabCamera.SelectedIndex].LayoutOne(camNo);
			}
		}

        private void tabCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabCamera.SelectedIndex)
            {
                case 0:
                    rdoWindow1.Text = "1";
                    rdoWindow2.Text = "2";
                    rdoWindow3.Text = "3";
                    rdoWindow4.Text = "4";
                    break;
                case 1:
                    rdoWindow1.Text = "5";
                    rdoWindow2.Text = "6";
                    rdoWindow3.Text = "7";
                    rdoWindow4.Text = "8";
                    break;
				case 2:
					rdoWindow1.Text = "9";
					rdoWindow2.Text = "-";
					rdoWindow3.Text = "-";
					rdoWindow4.Text = "-";
					break;
			}
			ChangeWindow();
        }

        private bool Intensity(HalconDotNet.HObject img, out double dMean, out double dDeviation )
        {
            dMean = 0.0;
            dDeviation = 0.0;

            if( img == null )
                return false;

            HTuple htMean, htDeviation;
            HObject hoRegion = null;
            try
            {
                HOperatorSet.Intensity(img, img, out htMean, out htDeviation);
                dMean = htMean.D;
                dDeviation = htDeviation.D;
            }
            catch (HOperatorException)
            {
                return false;
            }
            finally
            {
                HalconExtFunc.Clear(ref hoRegion);
            }
            return true;
        }

        private bool LineProjection(HObject img, bool bHorz, bool bVert, out double[] adHorz, out double[] adVert, out double dMeanHorz, out double dDevHorz, out double dMaxHorz, out double dMinHorz, out double dMeanVert, out double dDevVert, out double dMaxVert, out double dMinVert )
        {
            dMeanHorz = 0.0;
            dMeanVert = 0.0;
            dDevHorz = 0.0;
            dDevVert = 0.0;

            dMaxHorz = 0.0;
            dMinHorz = 0.0;
            dMaxVert = 0.0;
            dMinVert = 0.0;

            adHorz = null;
            adVert = null;

            HTuple htHorProj, htVerProj;
            try
            {
                HOperatorSet.GrayProjections(img, img, "simple", out htHorProj, out htVerProj);

                if (bHorz)
                {
                    adHorz = new double[htHorProj.TupleLength()];
                    for (int i = 0; i < adHorz.Length; i++)
                    {
                        adHorz[i] = htHorProj[i].D;
                    }
                    dMeanHorz = htHorProj.TupleMean().D;
                    dDevHorz = htHorProj.TupleDeviation().D;
                    dMaxHorz = htHorProj.TupleMax();
                    dMinHorz = htHorProj.TupleMin();
                }

                if (bVert)
                {
                    adVert = new double[htVerProj.TupleLength()];
                    for (int i = 0; i < adVert.Length; i++)
                    {
                        adVert[i] = htVerProj[i].D;
                    }
                    dMeanVert = htVerProj.TupleMean().D;
                    dDevVert = htVerProj.TupleDeviation().D;
                    dMaxVert = htVerProj.TupleMax();
                    dMinVert = htVerProj.TupleMin();
                }
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

        private bool Focus(HObject img, out double dFocus)
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
                HalconExtFunc.Clear( ref hoEdgeAmp );
            }

            return true;
        }

        private void chkStatics_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox clb = (CheckedListBox)sender;
            bool bPrevChecked = false;
            if( clb.SelectedIndex == 4 )
            {
                bPrevChecked = _lstTests[clb.SelectedIndex];
            }
            _lstTests[clb.SelectedIndex] = ( e.NewValue == CheckState.Checked )?true:false;

            if( clb.SelectedIndex == 4 )
            {
                if( bPrevChecked != _lstTests[clb.SelectedIndex] && _lstTests[clb.SelectedIndex] )
                {
                    _bFocusMaxReset = true;
                }
            }
        }

        private void frmCameraTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (chkLive.Checked || chkHardTriggerWait.Checked)
            {
                e.Cancel = true;
                return;
            }

            if (DialogResult.Yes != MessageBox.Show("終了します。\nよろしいですか？", "確認", MessageBoxButtons.YesNo))
            {
                e.Cancel = true;
                return;
            }

            termControls();
        }

        const byte ShadingVersion_1       = 0x5a;
        const byte ShadingType_Gain       = 0xc3;
        const byte ShadingSensorType_Line = 0x02;
        const byte ShadingLineType_Single = 0x01;
        const byte ShadingLineType_Tri    = 0x03;

        private bool createShadingFile(HObject img, string sShadingFilePath)
        {
            double [] adHorz, adVert;
            double dMeanHorz, dDevHorz, dMeanVert, dDevVert, dMaxHorz, dMaxVert, dMinHorz, dMinVert;
            LineProjection(img, false, true, out adHorz, out adVert, out dMeanHorz, out dDevHorz, out dMaxHorz, out dMinHorz, out dMeanVert, out dDevVert, out dMaxVert, out dMinVert);

            double[] adCoeff = new double[adVert.Length];

            // 最大輝度になるように調整する
            for (int i = 0; i < adVert.Length; i++)
            {
                adCoeff[i] = dMaxVert / adVert[i];
            }

            uint [] aui32Coeff = new uint [adCoeff.Length];

            // 整数部16,小数部16の固定小数点を生成する
            for (int i = 0; i < adCoeff.Length; i++)
            {
                aui32Coeff[i] = (uint)(adCoeff[i] * (1 << 16));
                if (aui32Coeff[i] > 0x0003FFFF)
                    aui32Coeff[i] = 0x0003FFFF;
            }

            using (FileStream fs = new FileStream(sShadingFilePath, FileMode.Create))
            {
                using( BinaryWriter bw = new BinaryWriter( fs ))
                {
                    bw.Write(ShadingVersion_1);
                    bw.Write(ShadingType_Gain);
                    bw.Write(ShadingSensorType_Line);
                    bw.Write(ShadingLineType_Single);
                    bw.Write((UInt16)adVert.Length);
                    bw.Write((UInt16)0);

                    for (int i = 0; i < aui32Coeff.Length; i++)
                    {
                        bw.Write(aui32Coeff[i]);
                    }
                    bw.Close();
                }
                fs.Close();
            }
            return true;
        }

        //private void btnLightConfCreate_Click(object sender, EventArgs e)
        //{

        //    frmLightProfileNew frmLightProf = new frmLightProfileNew(_lstLightConfProfile);

        //    if (cmbLightConfName.SelectedIndex != -1)
        //    {
        //        clsLightConfProfile prof = _lstLightConfProfile[cmbLightConfName.SelectedIndex];

        //        frmLightProf.CameraIndex = prof.CameraIndex;
        //        frmLightProf.CameraGain = prof.CameraGain;
        //        frmLightProf.CameraOffset = prof.CameraOffset;
        //        frmLightProf.CameraExposureTime = prof.CameraExposure;

        //        frmLightProf.LightIndex = prof.LightIndex;
        //        frmLightProf.LightValueMin = prof.LightValueMin;
        //        frmLightProf.LightValueMax = prof.LightValueMin;
        //        frmLightProf.LightWaitTime = prof.LightWaitTime;

        //        frmLightProf.CaptureCount = prof.CaptureCount;
        //    }
            
        //    if (DialogResult.OK != frmLightProf.ShowDialog(this))
        //        return;

        //    clsLightConfProfile profnew = new clsLightConfProfile();
        //    profnew.Name = frmLightProf.ProfileName;
        //    profnew.CameraIndex = frmLightProf.CameraIndex;
        //    profnew.CameraGain = frmLightProf.CameraGain;
        //    profnew.CameraOffset = frmLightProf.CameraOffset;
        //    profnew.CameraExposure = frmLightProf.CameraExposureTime;
        //    profnew.LightIndex = frmLightProf.LightIndex;
        //    profnew.LightValueMin = frmLightProf.LightValueMin;
        //    profnew.LightValueMax = frmLightProf.LightValueMax;
        //    profnew.LightWaitTime = frmLightProf.LightWaitTime;
        //    profnew.CaptureCount = frmLightProf.CaptureCount;

        //    _lstLightConfProfile.Add(profnew);
        //    int iIndex = cmbLightConfName.Items.Add(profnew.Name);
        //    cmbLightConfName.SelectedIndex = iIndex;
        //}

        //private void btnLightConfDelete_Click(object sender, EventArgs e)
        //{
        //    if( cmbLightConfName.SelectedIndex == -1 )
        //        return;

        //    int iSelectedIndex = cmbLightConfName.SelectedIndex;
        //    clsLightConfProfile prof = _lstLightConfProfile[iSelectedIndex];

        //    if (DialogResult.Yes != this.ShowMessage("プロファイル:" + prof.Name + "を削除します\r\nよろしいですか？", MessageType.YesNo))
        //        return;

        //    _lstLightConfProfile.RemoveAt(iSelectedIndex);
        //    cmbLightConfName.Items.RemoveAt(iSelectedIndex);

        //    if (cmbLightConfName.Items.Count == 0)
        //        cmbLightConfName.SelectedIndex = -1;
        //    else
        //        cmbLightConfName.SelectedIndex = iSelectedIndex - 1;
        //}

        //private void btnLightConfStart_Click(object sender, EventArgs e)
        //{
        //    if (cmbLightConfName.SelectedIndex == -1)
        //        return;

        //    int iSelectedIndex = cmbLightConfName.SelectedIndex;

        //    string sReason ;
        //    if (!DoLightProfile(_lstLightConfProfile[iSelectedIndex], out sReason))
        //    {
        //        this.ShowMessage("照明のプロファイルに失敗しました\r\n理由：" + sReason, MessageType.Error );
        //        return;
        //    }
        //}

        public  class clsLightConfProfile
        {
            public string Name { get; set; }
            public int CameraIndex { get; set; }
            public int LightIndex { get; set; }

            public int CameraGain { get; set; }
            public int CameraOffset { get; set; }
            public int CameraExposure { get; set; }

            public int LightValueMin { get; set; }
            public int LightValueMax { get; set; }

            public int LightWaitTime { get; set; }
            public int CaptureCount { get; set; }

            private double[] adProfiles;

            public void CopyProfiles(double[] profs)
            {
                adProfiles = profs;
                for (int i = 0; i < profs.Length; i++)
                    adProfiles[i] = profs[i];
            }

            public bool WriteProfile(string sDirPath, DateTime dtNow )
            {
                if (adProfiles == null)
                    return false;

                sDirPath = sDirPath + sDirPath.DirectoryMark();

                FileStream fs = null;
                StreamWriter sw = null;
                try
                {
                    fs = new FileStream(sDirPath + Name + ".prf", FileMode.OpenOrCreate);
                    sw = new StreamWriter(fs, Encoding.GetEncoding("Shift-Jis"));

                    string sProfs = "";
                    for (int i = 0; i < adProfiles.Length; i++)
                    {
                        if (i != 0)
                            sProfs += ",";
                        sProfs += (adProfiles[i] == -1) ? adProfiles[i].ToString("F1") : adProfiles[i].ToString("F3");

                    }
                    sw.WriteLine(dtNow.ToString() + "," + sProfs);
                }
                catch (Exception)
                {
                    return false;
                }
                finally
                {   
                    if( sw != null )
                        sw.Close();
                    if( fs != null )
                        fs.Close();
                }

                return true;
            }

            public bool Save(string sPath, List<clsLightConfProfile> list)
            {
                int iIndex = list.IndexOf( this );
                IniFileAccess ifa = new IniFileAccess();
                string sSection = "Profile" + (iIndex + 1).ToString();

                ifa.SetIni(sSection, "Name", Name, sPath);
                ifa.SetIni(sSection, "CameraIndex", CameraIndex, sPath);
                ifa.SetIni(sSection, "CameraGain", CameraGain, sPath);
                ifa.SetIni(sSection, "CameraOffset", CameraOffset, sPath);
                ifa.SetIni(sSection, "CameraExposure", CameraExposure, sPath);
                ifa.SetIni(sSection, "LightIndex", LightIndex, sPath);
                ifa.SetIni(sSection, "LightValueMin", LightValueMin, sPath);
                ifa.SetIni(sSection, "LightValueMax", LightValueMax, sPath);
                ifa.SetIni(sSection, "LightWaitTime", LightWaitTime, sPath);
                ifa.SetIni(sSection, "CaptureCount", CaptureCount, sPath);

                return true;
            }

            public bool Load(string sPath, List<clsLightConfProfile> list)
            {
                int iIndex = list.Count;
                IniFileAccess ifa = new IniFileAccess();
                string sSection = "Profile" + (iIndex + 1).ToString();

                Name = ifa.GetIni(sSection, "Name", Name, sPath);
                if (Name == "")
                    return false;
                CameraIndex = ifa.GetIni(sSection, "CameraIndex", CameraIndex, sPath);
                CameraGain = ifa.GetIni(sSection, "CameraGain", CameraGain, sPath);
                CameraOffset = ifa.GetIni(sSection, "CameraOffset", CameraOffset, sPath);
                CameraExposure = ifa.GetIni(sSection, "CameraExposure", CameraExposure, sPath);
                LightIndex = ifa.GetIni(sSection, "LightIndex", LightIndex, sPath);
                LightValueMin = ifa.GetIni(sSection, "LightValueMin", LightValueMin, sPath);
                LightValueMax = ifa.GetIni(sSection, "LightValueMax", LightValueMax, sPath);
                LightWaitTime = ifa.GetIni(sSection, "LightWaitTime", LightWaitTime, sPath);
                CaptureCount = ifa.GetIni(sSection, "CaptureCount", CaptureCount, sPath);
                return true;
            }

        }

        private void tpgCalibration_Click(object sender, EventArgs e)
        {

        }

        private void chkCenterLine_CheckedChanged(object sender, EventArgs e)
        {
			int cameraNum = CameraManager.getInstance().CameraNum;
			for (int camIndex = 0; camIndex < cameraNum; camIndex++)
			{
				int winMultiIndex = camIndex / 4;
				int winIndex = camIndex % 4;
				uclHWinMulti[winMultiIndex].GetWindowControl(winIndex).CenterLine = chkCenterLine.Checked;
                uclHWinMulti[winMultiIndex].GetWindowControl(winIndex).repaint();
			}
        }

        private void chkGridLine_CheckedChanged(object sender, EventArgs e)
        {
			int cameraNum = CameraManager.getInstance().CameraNum;
			for (int camIndex = 0; camIndex < cameraNum; camIndex++)
			{
				int winMultiIndex = camIndex / 4;
				int winIndex = camIndex % 4;
				uclHWinMulti[winMultiIndex].GetWindowControl(winIndex).GridLine = chkGridLine.Checked;
                uclHWinMulti[winMultiIndex].GetWindowControl(winIndex).repaint();
            }
        }

        private string cameraSettingName(HalconCamera.HalconCameraBase camera)
        {
            return camera.Name + "_G" + camera.Gain.ToString() + "_O" + camera.Offset.ToString() + "_E" + camera.ExposureTime.ToString();
        }

        private string lightSettingName()
        {
            string sLight = "";
            for (int i = 0; i < _lstLightControls.Count; i++)
            {
                if (_lstLightControls[i].Enabled)
                {
                    sLight += _lstLightControls[i].LigthName + _lstLightControls[i].LightValue.ToString();
                }
            }
            return sLight;
        }

        private void btnCreateShadingFile_Click(object sender, EventArgs e)
        {
        }

        private void chkDisableShading_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void btnShadingExeute_Click(object sender, EventArgs e)
        {
        }

        List<PYLON_DEVICE_HANDLE> _lstDevHandles = new List<PYLON_DEVICE_HANDLE>();
        List<PYLON_DEVICE_INFO_HANDLE> _lstDevInfoHandles = new List<PYLON_DEVICE_INFO_HANDLE>();
        public bool initCamera()
        {
            uint numDevices = Pylon.EnumerateDevices();

            List<string> lstIPs = new List<string>();
            for (uint i = 0; i < numDevices; i++)
            {
                PYLON_DEVICE_INFO_HANDLE hInfo = Pylon.GetDeviceInfoHandle(i);
                lstIPs.Add(Pylon.DeviceInfoGetPropertyValueByName(hInfo, "IpAddress"));
            }

            lstIPs.Sort();

            for (uint i = 0; i < lstIPs.Count; i++)
            {
                for (uint c = 0; c < numDevices; c++)
                {
                    PYLON_DEVICE_INFO_HANDLE hInfo = Pylon.GetDeviceInfoHandle(c);
                    if (lstIPs[(int)i] == Pylon.DeviceInfoGetPropertyValueByName(hInfo, "IpAddress"))
                    {
                        PYLON_DEVICE_HANDLE hDev = Pylon.CreateDeviceByIndex(c);
                        Pylon.DeviceOpen(hDev, Pylon.cPylonAccessModeControl | Pylon.cPylonAccessModeStream);
                        _lstDevHandles.Add(hDev);
                        _lstDevInfoHandles.Add(hInfo);
                    }
                }
            }
            return true;
        }

        private bool termCamera()
        {
            for (int i = 0; i < _lstDevHandles.Count; i++)
            {
                if (Pylon.DeviceIsOpen(_lstDevHandles[i]))
                {
                    Pylon.DeviceClose(_lstDevHandles[i]);
                }
                Pylon.DestroyDevice(_lstDevHandles[i]);
            }
            _lstDevHandles.Clear();
            _lstDevInfoHandles.Clear();
            return true;
        }

        private int getCameraCount()
        {
            return _lstDevHandles.Count;
        }

        private string getCameraIPAddress(int i)
        {
            if (i < 0 || i >= _lstDevInfoHandles.Count)
                return "";

            return Pylon.DeviceInfoGetPropertyValueByName(_lstDevInfoHandles[i], "IpAddress");
        }

        private string getCameraName(int i)
        {
            if (i < 0 || i >= _lstDevInfoHandles.Count)
                return "";

            return Pylon.DeviceInfoGetPropertyValueByName(_lstDevInfoHandles[i], "ModelName");
        }

        public bool uploadShadingFile(PYLON_DEVICE_HANDLE hDev, string sCameraFile, string sLocalFile)
        {
            if (hDev == null)
                return false;

            if (sCameraFile == "")
                return false;

            if (sLocalFile == "")
                return false;
            byte[] datas;
            try
            {
                using (FileStream fs = new FileStream(sLocalFile, FileMode.Open))
                {
                    datas = new byte[fs.Length];
                    fs.Read(datas, 0, (int)fs.Length);
                    fs.Close();
                }
            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                PylonBuffer<byte> pBuf = new PylonBuffer<byte>(datas);
                NODEMAP_HANDLE hNodeMap;
                hNodeMap = Pylon.DeviceGetNodeMap(hDev);
                GENAPI_FILE_HANDLE hFile = GenApi.FileOpen(hNodeMap, sCameraFile, EGenApiFileAccessMode.GenApiFileWriteAccess);
                GenApi.FileWrite(hFile, pBuf, (uint)datas.Length);
                GenApi.FileClose(hFile);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// カメラIndex番号取得
        /// </summary>
        /// <param name="radioIndex"></param>
        /// <returns></returns>
        public int GetMainCameraNumber(out int radioIndex)
        {
            radioIndex = -1;
            if (rdoDivide.Checked == true)
                return -1;

            radioIndex = 0;
            int camNo = tabCamera.SelectedIndex * 4;
            foreach (RadioButton rdo in rdoWindows)
            {
                if (rdo.Checked == true)
                {
                    break;
                }
                camNo++;
                radioIndex++;
            }
            if (camNo >= CameraManager.getInstance().CameraNum)
                return -1;
            return camNo;
        }
        /// <summary>
        /// メイン画面コントロール取得
        /// </summary>
        /// <returns></returns>
        public HWndCtrl GetMainWnd()
        {
            int camRadioBtnIndex;
            int camNo = GetMainCameraNumber(out camRadioBtnIndex);
            if (camNo == -1)
                return null;
            return uclHWinMulti[tabCamera.SelectedIndex].GetWindowControl(camRadioBtnIndex);
        }

        #region メンテナンス機能
        #region 未使用
        public bool StartRoiRectangle2(double row, double col, double phi, double len1, double len2, string message, ICallbackRoiRectangle2 callback, object user)
        { return true; }
        public bool StartRoiCircle(double row, double col, double rad, string message, ICallbackRoiCircle callback, object user)
        { return true; }
        public bool StartThreshold(int iLow, int iHigh, double row1, double col1, double row2, double col2, string message, ICallbackThreshold callback, object user)
        { return true; }
        public bool StartRoiPointMulti(List<double> rows, List<double> cols, string sMessage, ICallbackPointMulti callback, object user)
        { return true; }
        public bool StartRoiRectangle1Multi(List<CRectangle1> lstRect, CRectangle1 rcInit, string sMessage, ICallbackRectangle1Multi callback, object user)
        { return true; }
        public bool AddRectangle1(string name, double row1, double col1, double row2, double col2, string color, string drawmode = GraphicsManager.HALCON_DRAWMODE_MARGIN, int linewidth = 1)
        { return true; }
        public bool AddRectangle2(string name, double row, double col, double phi, double len1, double len2, string color, string drawmode = GraphicsManager.HALCON_DRAWMODE_MARGIN, int linewidth = 1)
        { return true; }
        public bool AddCircle(string name, double row, double col, double rad, string color, string drawmode = GraphicsManager.HALCON_DRAWMODE_MARGIN, int linewidth = 1)
        { return true; }
        public bool AddText(string name, string message, double row, double col, int fontsize, bool window, bool box, string color)
        { return true; }
        public void DeleteObject(string name)
        { return; }
        public void DeleteAllObjects()
        { return; }
        public void StoreImage(HObject img)
        { return; }
        public void RestoreImage()
        { return; }
        public void ControlSender(string type, object[] arrayParams)
        { return; }
        public void DisableControls()
        { return; }
        public void EnableControls()
        { return; }
        public void UpdateResults()
        { return; }
        public void UpdateDatas(bool bUp)
        { return; }
        public HalconDotNet.HObject GetCurrentImage()
        { return new HalconDotNet.HObject(); }
        public HalconDotNet.HObject GetCurrentDisplayImage()
        { return new HalconDotNet.HObject(); }
        public bool IsCanPopupWindow()
        { return true; }
        public bool IsRegistImageDisplay()
        { return true; }
        public void Update(string type, object[] paramArray)
        { return; }
        #endregion

        #region Load/Save
        /// <summary>
        /// メンテナンスファイル　読込
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadMainteFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog fdlg = new OpenFileDialog())
            {
                fdlg.Filter = "メンテナンスファイル(*.ini)|*.ini|すべてのファイル(*.*)|*.*";
                fdlg.FilterIndex = 1;
                fdlg.Title = "開くファイルを選択してください";
                fdlg.CheckFileExists = true;
                fdlg.CheckPathExists = true;
                fdlg.RestoreDirectory = true;
                fdlg.InitialDirectory = this.MainteDirectory;
                fdlg.FileName = clsMainteFunc.DEFAULT_MAINTE_FILE + clsMainteFunc.DEFAULT_MAINTE_FILE_EXIST;
                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    string sPath = Path.Combine(this.MainteDirectory, fdlg.FileName);
                    clsMainteFunc.getInstance().Load(sPath);
                    MainteData2Disp();

                    string msgStr = "読込が完了しました。";
                    frmMessageForm mes = new frmMessageForm(msgStr, MessageType.Information, null);
                    mes.ShowDialog();
                }
            }
        }
        /// <summary>
        /// メンテナンスファイル　保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveMainteFile_Click(object sender, EventArgs e)
        {
            string sPath;
            string sBackupPath;
            string msgStr;
            frmMessageForm mes;
            System.Windows.Forms.DialogResult res;

            MainteDisp2Data();

            //ファイル名
            sPath = Path.Combine(this.MainteDirectory, clsMainteFunc.DEFAULT_MAINTE_FILE) + clsMainteFunc.DEFAULT_MAINTE_FILE_EXIST;
            //バックアップ名
            string dateStr = DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            string fName = clsMainteFunc.DEFAULT_MAINTE_FILE + "_" + dateStr;
            sBackupPath = Path.Combine(this.MainteDirectory, fName) + clsMainteFunc.DEFAULT_MAINTE_FILE_EXIST;

            if (File.Exists(sPath) == true)
            {
                msgStr = string.Format("バックアップを実施しますか？\n {0} -> {1}", Path.GetFileName(sPath), Path.GetFileName(sBackupPath));
                mes = new frmMessageForm(msgStr, MessageType.YesNo, null);
                res = mes.ShowDialog();
                if (res != DialogResult.Cancel)
                {
                    File.Move(sPath, sBackupPath);
                }
            }
            msgStr = string.Format("保存を実施しますか？\n{0}", Path.GetFileName(sPath));
            mes = new frmMessageForm(msgStr, MessageType.YesNo, null);
            res = mes.ShowDialog();
            if (res != DialogResult.Cancel)
            {
                clsMainteFunc.getInstance().Save(sPath, CameraManager.getInstance().CameraNum, LightControlManager.getInstance().LightCount);  // lưu lại thông tin của các camera và đèn

                msgStr = "保存が完了しました。";
                mes = new frmMessageForm(msgStr, MessageType.Information, null);
                mes.ShowDialog();
            }
        }
        #endregion

        /// <summary>
        /// ホワイトバランス実施したカメラ№
        /// </summary>
        private int _whiteBCamNo;
        /// <summary>
        /// ホワイトバランス実施
        /// </summary>
        private bool _runWhiteBFlag;
        /// <summary>
        /// カメラControlでROI枠指定しているカメラ№
        /// </summary>
        private int _roiCamNo;
        /// <summary>
        /// 照明ControlでROI枠指定している照明№
        /// </summary>
        private int _roiLightNo;

        #region カメラControlから
        /// <summary>
        /// フォーカスROI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCameraTest_CameraFcRoiButtonClicked(object sender, EventArgs e)
        {
            _roiCamNo = uclCameraControls.ToList().IndexOf((uclCameraControl)sender);

            string msgStr;
            HWndCtrl wnd = GetMainWnd();
            if (wnd == null)
            {
                msgStr = "領域設定するカメラを選択してください。";
                frmMessageForm mes = new frmMessageForm(msgStr, MessageType.Warning, null);
                mes.ShowDialog();
                return;
            }
            int cameraRadioBtnIndex;
            int cameraNumber = GetMainCameraNumber(out cameraRadioBtnIndex);
            if (_roiCamNo != cameraNumber)
            {
                msgStr = "選択カメラを一致させてください。";
                frmMessageForm mes = new frmMessageForm(msgStr, MessageType.Warning, null);
                mes.ShowDialog();
                return;
            }

            StartRoiRectangle1(
                clsMainteFunc.getInstance().CamParam[_roiCamNo].FCRow1,
                clsMainteFunc.getInstance().CamParam[_roiCamNo].FCColumn1,
                clsMainteFunc.getInstance().CamParam[_roiCamNo].FCRow2,
                clsMainteFunc.getInstance().CamParam[_roiCamNo].FCColumn2,
                "領域を設定してください", this, 0);
        }
        /// <summary>
        /// フォーカス　登録
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCameraTest_CameraFcEntryButtonClicked(object sender, EventArgs e)
        {
            int camIndex = uclCameraControls.ToList().IndexOf((uclCameraControl)sender);
            clsMainteFunc.getInstance().CamParam[camIndex].FCBaseValue = _fcNowValues[camIndex];
            for (int i = 0; i < LightControlManager.getInstance().LightCount; i++)
            {
                clsMainteFunc.getInstance().CamParam[camIndex].FcLightEnabled[i] = _lstLightControls[i].LightEnabled;
                clsMainteFunc.getInstance().CamParam[camIndex].FcLightValue[i] = _lstLightControls[i].LightValue;
            }
        }
        /// <summary>
        /// フォーカス時の照明値に　変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCameraTest_CameraFcChangeLightButtonClicked(object sender, EventArgs e)
        {
            int camIndex = uclCameraControls.ToList().IndexOf((uclCameraControl)sender);
            for (int i = 0; i < LightControlManager.getInstance().LightCount; i++)
            {
                bool bEnable = clsMainteFunc.getInstance().CamParam[camIndex].FcLightEnabled[i];
                int iValue = clsMainteFunc.getInstance().CamParam[camIndex].FcLightValue[i];
                _lstLightControls[i].LightEnabled = bEnable;
                _lstLightControls[i].LightValue = iValue;
                LightType lt = LightControlManager.getInstance().GetLight(i);
                if (bEnable)
                    lt.LightOn(iValue, true);
                else
                    lt.LightOff();
            }
        }

        /// <summary>
        /// ホワイトバランス　ROI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCameraTest_CameraWhiteBRoiButtonClicked(object sender, EventArgs e)
        {
            _roiCamNo = uclCameraControls.ToList().IndexOf((uclCameraControl)sender);

            string msgStr;
            HWndCtrl wnd = GetMainWnd();
            if (wnd == null)
            {
                msgStr = "領域設定するカメラを選択してください。";
                frmMessageForm mes = new frmMessageForm(msgStr, MessageType.Warning, null);
                mes.ShowDialog();
                return;
            }
            int cameraRadioBtnIndex;
            int cameraNumber = GetMainCameraNumber(out cameraRadioBtnIndex);
            if (_roiCamNo != cameraNumber)
            {
                msgStr = "選択カメラを一致させてください。";
                frmMessageForm mes = new frmMessageForm(msgStr, MessageType.Warning, null);
                mes.ShowDialog();
                return;
            }

            StartRoiRectangle1(
                clsMainteFunc.getInstance().CamParam[_roiCamNo].WhiteBRow1,
                clsMainteFunc.getInstance().CamParam[_roiCamNo].WhiteBColumn1,
                clsMainteFunc.getInstance().CamParam[_roiCamNo].WhiteBRow2,
                clsMainteFunc.getInstance().CamParam[_roiCamNo].WhiteBColumn2,
                "領域を設定してください", this, 1);
        }
        /// <summary>
        /// ホワイトバランス　実行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCameraTest_CameraWhiteBRunButtonClicked(object sender, EventArgs e)
        {
            MainteDisp2Data();
            bool bImalgeAll = clsMainteFunc.getInstance().CamParam[_whiteBCamNo].WhiteBImageAll;

            string msgStr;
            frmMessageForm mes;
            System.Windows.Forms.DialogResult res;

            if (bImalgeAll == false && chkLive.Checked == true)
            {
                msgStr = "動画を解除してください。";
                mes = new frmMessageForm(msgStr, MessageType.Warning, null);
                mes.ShowDialog();
                return;
            }
            if (chkHardTriggerWait.Checked == true)
            {
                msgStr = "ハード取込を解除してください。";
                mes = new frmMessageForm(msgStr, MessageType.Warning, null);
                mes.ShowDialog();
                return;
            }

            if (bImalgeAll == false)
            {
                msgStr = "指定領域で行います。\nよろしいですか？\n失敗した場合、システムを再起動してください。";
                mes = new frmMessageForm(msgStr, MessageType.YesNo, null);
                res = mes.ShowDialog();
                if (res == DialogResult.Cancel)
                    return;
            }

            _whiteBCamNo = uclCameraControls.ToList().IndexOf((uclCameraControl)sender);
            HalconCameraPylon cam = CameraManager.getInstance().GetCamera(_whiteBCamNo) as HalconCamera.HalconCameraPylonGigE;

            clsMainteFunc.CameraParameter mCamP = clsMainteFunc.getInstance().CamParam[_whiteBCamNo];
            bool ret = true;
            int errCnt = 0;
            msgStr = "";
            bool isNowLive = chkLive.Checked;
            try
            {
                Action act = new Action(() =>
                {
                    try
                    {
                        int val;
                        if (bImalgeAll == false)
                        {
                            cam.StopWhiteBAsyncGrab();
                            errCnt = 1;
                            val = ((int)(mCamP.WhiteBRow2 - mCamP.WhiteBRow1) / 2) * 2;
                            cam.SetWhiteBImageHeight(val);
                            errCnt = 2;
                            val = ((int)(mCamP.WhiteBColumn2 - mCamP.WhiteBColumn1) / 2) * 2;
                            cam.SetWhiteBImageWidth(val);
                            errCnt = 3;
                            val = ((int)mCamP.WhiteBRow1 / 2) * 2;
                            cam.SetWhiteBImageOffsetY(val);
                            errCnt = 4;
                            val = ((int)mCamP.WhiteBColumn1 / 2) * 2;
                            cam.SetWhiteBImageOffsetX(val);
                            errCnt = 5;
                            cam.WhiteBalanceColor();
                            errCnt = 6;
                            cam.StartAsyncGrab();
                            errCnt = 7;
                        }
                        else
                        {
                            cam.WhiteBalanceColor();
                        }

                        _runWhiteBFlag = true;
                        if (isNowLive == false)
                            Live(true);
                        while (_runWhiteBFlag)
                        {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(10);
                        }
                        if (isNowLive == false)
                            Live(false);

                        if (bImalgeAll == false)
                        {
                            cam.StopWhiteBAsyncGrab();
                            errCnt = 8;
                            cam.SetWhiteBImageOffsetY(cam.ImageOffsetY);
                            errCnt = 9;
                            cam.SetWhiteBImageOffsetX(cam.ImageOffsetX);
                            errCnt = 10;
                            cam.SetWhiteBImageHeight(cam.ImageHeight);
                            errCnt = 11;
                            cam.SetWhiteBImageWidth(cam.ImageWidth);
                            errCnt = 12;
                            cam.StartAsyncGrab();
                        }

                        if (isNowLive == false)
                            Live(true);
                        int loop = 100;
                        while (loop >= 0)
                        {
                            Application.DoEvents();
                            System.Threading.Thread.Sleep(10);
                            loop--;
                        }
                        if (isNowLive == false)
                            Live(false);
                    }
                    catch (HalconException exc)
                    {
                        msgStr = exc.Message;
                        ret = false;
                    }
                    finally
                    { }
                });
                Action actAbort = new Action(() =>
                {
                    _runWhiteBFlag = false;
                });

                frmProgressForm frmPrg = new frmProgressForm(act, actAbort);
                frmPrg.Description = "ホワイトバランスを実施しています。\nしばらくお待ちください。";
                frmPrg.ShowDialog(this);
                if (ret == false)
                {
                    cam.Close();
                    msgStr = string.Format("ERROR-{0} : {1}", errCnt, msgStr);
                    mes = new frmMessageForm(msgStr, MessageType.Error, null);
                    mes.ShowDialog();
                    cam.Open();
                }
            }
            catch (HalconException exc)
            {
                msgStr = "ERROR : " + exc.Message;
                mes = new frmMessageForm(msgStr, MessageType.Error, null);
                mes.ShowDialog();
            }
            finally
            {
            }
        }
        /// <summary>
        /// ホワイトバランス　登録
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCameraTest_CameraWhiteBEntryButtonClicked(object sender, EventArgs e)
        {
            string msgStr;
            if (chkLive.Checked == true)
            {
                msgStr = "動画を解除してください。";
                frmMessageForm mes = new frmMessageForm(msgStr, MessageType.Warning, null);
                mes.ShowDialog();
                return;
            }
            if (chkHardTriggerWait.Checked == true)
            {
                msgStr = "ハード取込を解除してください。";
                frmMessageForm mes = new frmMessageForm(msgStr, MessageType.Warning, null);
                mes.ShowDialog();
                return;
            }

            int camIndex = uclCameraControls.ToList().IndexOf((uclCameraControl)sender);
            HalconCameraPylon cam = CameraManager.getInstance().GetCamera(camIndex) as HalconCamera.HalconCameraPylonGigE;
            cam.StopWhiteBAsyncGrab();
            cam.UserSetSave();
            cam.StartAsyncGrab();

            for (int i = 0; i < LightControlManager.getInstance().LightCount; i++)
            {
                clsMainteFunc.getInstance().CamParam[camIndex].WhiteBLightEnabled[i] = _lstLightControls[i].LightEnabled;
                clsMainteFunc.getInstance().CamParam[camIndex].WhiteBLightValue[i] = _lstLightControls[i].LightValue;
            }
        }
        /// <summary>
        /// ホワイトバランス　リセット
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCameraTest_CameraWhiteBResetButtonClicked(object sender, EventArgs e)
        {
            int camIndex = uclCameraControls.ToList().IndexOf((uclCameraControl)sender);
            HalconCameraPylon cam = CameraManager.getInstance().GetCamera(camIndex) as HalconCamera.HalconCameraPylonGigE;
            cam.WhiteBalanceReset();
        }
        /// <summary>
        /// ホワイトバランス時の照明値に　変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCameraTest_CameraWhiteBChangeLightButtonClicked(object sender, EventArgs e)
        {
            int camIndex = uclCameraControls.ToList().IndexOf((uclCameraControl)sender);
            for (int i = 0; i < LightControlManager.getInstance().LightCount; i++)
            {
                bool bEnable = clsMainteFunc.getInstance().CamParam[camIndex].WhiteBLightEnabled[i];
                int iValue = clsMainteFunc.getInstance().CamParam[camIndex].WhiteBLightValue[i];
                _lstLightControls[i].LightEnabled = bEnable;
                _lstLightControls[i].LightValue = iValue;
                LightType lt = LightControlManager.getInstance().GetLight(i);
                if (bEnable)
                    lt.LightOn(iValue, true);
                else
                    lt.LightOff();
            }
        }
        #endregion
        
        #region 照明Controlから
        /// <summary>
        /// ROI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCameraTest_LightRoiButtonClicked(object sender, EventArgs e)
        {
            frmMessageForm mes;
            System.Windows.Forms.DialogResult res;
            string msgStr;
            HWndCtrl wnd = GetMainWnd();
            if (wnd == null)
            {
                msgStr = "領域設定するカメラを選択してください。";
                mes = new frmMessageForm(msgStr, MessageType.Warning, null);
                mes.ShowDialog();
                return;
            }
            int cameraRadioBtnIndex;
            int cameraNumber = GetMainCameraNumber(out cameraRadioBtnIndex);
            msgStr = string.Format("ｶﾒﾗ={0} に領域設定してよろしいですか？", cameraNumber + 1);
            mes = new frmMessageForm(msgStr, MessageType.YesNo, null);
            res = mes.ShowDialog();
            if (res == DialogResult.Cancel)
                return;

            _roiLightNo = _lstLightControls.IndexOf((uclLightControl)sender);

            StartRoiRectangle1(
                clsMainteFunc.getInstance().LightParam[_roiLightNo].Row1,
                clsMainteFunc.getInstance().LightParam[_roiLightNo].Column1,
                clsMainteFunc.getInstance().LightParam[_roiLightNo].Row2,
                clsMainteFunc.getInstance().LightParam[_roiLightNo].Column2,
                "領域を設定してください", this, 2);
        }
        /// <summary>
        /// 算出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmCameraTest_LightDiffButtonClicked(object sender, EventArgs e)
        {
            MainteDisp2Data();
        }
        #endregion
        
        #region ROI
        ROIControllerCallback _roiCallback;
        Rectangle1Manager _rect1Manager;
        public bool StartRoiRectangle1(double row1, double col1, double row2, double col2, string message, ICallbackRoiRectangle1 callback, object user)
        {
            HWndCtrl wnd = GetMainWnd();
            if (wnd == null)
                return false;

            _roiCallback = new ROIControllerCallback(wnd.ROIManager);

            frmRoiRectangle1 frmDummy = new frmRoiRectangle1(row1, col1, row2, col2, message);

            _rect1Manager = new Rectangle1Manager(this, this, wnd, _roiCallback,
                PointToScreen(new Point(Width - frmDummy.Width - 4, tabSettings.Location.Y)));

            frmDummy.Dispose();
            frmDummy = null;

            _rect1Manager.UserSettingEnd += new UserSettingEndEventHandler(_rect1Manager_UserSettingEnd);
            if (!_rect1Manager.Start(row1, col1, row2, col2, message, callback, user))
            {
                _rect1Manager = null;
                return false;
            }
            return true;
        }
        void _rect1Manager_UserSettingEnd(object sender, UserSettingEndEventArgs e)
        {
            _rect1Manager = null;
        }
        public void Rectangle1_Move(double row1, double col1, double row2, double col2, object oUser)
        { return; }
        public void Rectangle1_Decide(double row1, double col1, double row2, double col2, object oUser)
        {
            int iKind = (int)oUser;
            if (iKind == 0)
            {
                //Camera FC Roi
                clsMainteFunc.getInstance().CamParam[_roiCamNo].FCRow1 = row1;
                clsMainteFunc.getInstance().CamParam[_roiCamNo].FCColumn1 = col1;
                clsMainteFunc.getInstance().CamParam[_roiCamNo].FCRow2 = row2;
                clsMainteFunc.getInstance().CamParam[_roiCamNo].FCColumn2 = col2;
            }
            if(iKind == 1)
            {
                //Camera White Balance Roi
                clsMainteFunc.getInstance().CamParam[_roiCamNo].WhiteBRow1 = row1;
                clsMainteFunc.getInstance().CamParam[_roiCamNo].WhiteBColumn1 = col1;
                clsMainteFunc.getInstance().CamParam[_roiCamNo].WhiteBRow2 = row2;
                clsMainteFunc.getInstance().CamParam[_roiCamNo].WhiteBColumn2 = col2;
            }
            if (iKind == 2)
            {
                //Light Gray Roi
                clsMainteFunc.getInstance().LightParam[_roiLightNo].Row1 = row1;
                clsMainteFunc.getInstance().LightParam[_roiLightNo].Column1 = col1;
                clsMainteFunc.getInstance().LightParam[_roiLightNo].Row2 = row2;
                clsMainteFunc.getInstance().LightParam[_roiLightNo].Column2 = col2;
                int radioIndex;
                _lstLightControls[_roiLightNo].CameraIndex = GetMainCameraNumber(out radioIndex);
            }
            //
            RepaintMainteRoiRect();
            return;
        }
        public void Rectangle1_Cancel(object oUser)
        { return; }
        #endregion

        #region Data2Disp/Disp2Data
        /// <summary>
        /// データ　→　画面へ
        /// </summary>
        private void MainteData2Disp()
        {
            for (int i = 0; i < CameraManager.getInstance().CameraNum; i++)
            {
                if (i >= uclCameraControls.Length)
                    break;
                if (i >= clsMainteFunc.getInstance().CamParam.Count)
                    break;
                HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(i);
                camera.SetGain(clsMainteFunc.getInstance().CamParam[i].GainValue);
                camera.SetOffset(clsMainteFunc.getInstance().CamParam[i].OffsetValue);
                camera.SetExposureTime(clsMainteFunc.getInstance().CamParam[i].ExposureValue);
                camera.SetLineRate(clsMainteFunc.getInstance().CamParam[i].LineRate);
                uclCameraControls[i].Gain = clsMainteFunc.getInstance().CamParam[i].GainValue;
                uclCameraControls[i].Offset = clsMainteFunc.getInstance().CamParam[i].OffsetValue;
                uclCameraControls[i].ExposureTime = clsMainteFunc.getInstance().CamParam[i].ExposureValue;
                uclCameraControls[i].LineRate = clsMainteFunc.getInstance().CamParam[i].LineRate;
                uclCameraControls[i].CheckImageAll= clsMainteFunc.getInstance().CamParam[i].WhiteBImageAll;
            }
            for (int i = 0; i < LightControlManager.getInstance().LightCount; i++)
            {
                if (i >= _lstLightControls.Count)
                    break;
                if (i >= clsMainteFunc.getInstance().LightParam.Count)
                    break;
                _lstLightControls[i].CameraIndex = clsMainteFunc.getInstance().LightParam[i].CameraNumber;
                _lstLightControls[i].BaseGrayValue = clsMainteFunc.getInstance().LightParam[i].BaseGrayValue;
                _lstLightControls[i].BaseLightValue = clsMainteFunc.getInstance().LightParam[i].BaseLightValue;
                _lstLightControls[i].DiffLightValue = clsMainteFunc.getInstance().LightParam[i].DiffLightValue;
                _lstLightControls[i].LightValue = clsMainteFunc.getInstance().LightParam[i].CalcLightValue;
            }
        }
        /// <summary>
        /// 画面　→　データへ
        /// </summary>
        private void MainteDisp2Data()
        {
            for (int i = 0; i < CameraManager.getInstance().CameraNum; i++)
            {
                if (i >= uclCameraControls.Length)
                    break;
                if (i >= clsMainteFunc.getInstance().CamParam.Count)
                    break;
                HalconCamera.HalconCameraBase camera = CameraManager.getInstance().GetCamera(i);
                clsMainteFunc.getInstance().CamParam[i].GainValue = (int)camera.GetGain();
                clsMainteFunc.getInstance().CamParam[i].OffsetValue = camera.GetOffset();
                clsMainteFunc.getInstance().CamParam[i].ExposureValue = camera.GetExposureTime();
                clsMainteFunc.getInstance().CamParam[i].WhiteBImageAll = uclCameraControls[i].CheckImageAll;
            }
            for (int i = 0; i < LightControlManager.getInstance().LightCount; i++)
            {
                if (i >= _lstLightControls.Count)
                    break;
                if (i >= clsMainteFunc.getInstance().LightParam.Count)
                    break;
                clsMainteFunc.getInstance().LightParam[i].CameraNumber = _lstLightControls[i].CameraIndex;
                clsMainteFunc.getInstance().LightParam[i].BaseGrayValue = _lstLightControls[i].BaseGrayValue;
                clsMainteFunc.getInstance().LightParam[i].BaseLightValue = _lstLightControls[i].BaseLightValue;
                clsMainteFunc.getInstance().LightParam[i].DiffLightValue = _lstLightControls[i].DiffLightValue;
                clsMainteFunc.getInstance().LightParam[i].CalcLightValue = _lstLightControls[i].LightValue;
            }
        }
        #endregion

        #region Gray値・FC値の算出
        /// <summary>
        /// フォーカス値の算出
        /// </summary>
        /// <param name="img"></param>
        /// <param name="camIndex"></param>
        private void MainteFocusValue(HObject img, int camIndex)
        {
            HObject hoRectangle;
            HObject hoReduceDomain;
            HOperatorSet.GenEmptyObj(out hoRectangle);
            HOperatorSet.GenEmptyObj(out hoReduceDomain);

            int retData = 0;
            try
            {
                clsMainteFunc.CameraParameter mCamP = clsMainteFunc.getInstance().CamParam[camIndex];

                HOperatorSet.GenRectangle1(out hoRectangle, mCamP.FCRow1, mCamP.FCColumn1, mCamP.FCRow2, mCamP.FCColumn2);
                HOperatorSet.ReduceDomain(img, hoRectangle, out hoReduceDomain);
                _fcNowValues[camIndex] = KaTool.ImageTool.FocusValue(hoReduceDomain);

                retData = MainteCalcGray(img, mCamP.FCRow1, mCamP.FCColumn1, mCamP.FCRow2, mCamP.FCColumn2);
                _fcGrayNowValue[camIndex] = retData;
            }
            catch (HalconException exc)
            {
                throw exc;
            }
            finally
            {
                hoRectangle.Dispose();
                hoReduceDomain.Dispose();
            }
        }        
        /// <summary>
        /// ホワイトバランスROIのGray値　の算出
        /// </summary>
        /// <param name="img"></param>
        /// <param name="camIndex"></param>
        /// <returns></returns>
        private void MainteWhiteBRoiGray(HObject img, int camIndex)
        {
            int retData = 0;
            try
            {
                clsMainteFunc.CameraParameter mCamP = clsMainteFunc.getInstance().CamParam[camIndex];
                retData = MainteCalcGray(img, mCamP.WhiteBRow1, mCamP.WhiteBColumn1, mCamP.WhiteBRow2, mCamP.WhiteBColumn2);
                _wbGrayNowValue[camIndex] = retData;
            }
            catch (HalconException exc)
            {
                throw exc;
            }
            finally
            {
            }
            return;
        }
        /// <summary>
        /// 照明ROIのGray値　の算出
        /// </summary>
        /// <param name="img"></param>
        /// <param name="camIndex"></param>
        private void MainteLightRoiGray(HObject img, int camIndex)
        {
            int retData = 0;
            try
            {
                for (int i = 0; i < _lstLightControls.Count; i++)
                {
                    if (_lstLightControls[i].Enabled == false)
                        break;
                    if (camIndex == _lstLightControls[i].CameraIndex)
                    {
                        clsMainteFunc.LightParameter mLgtP = clsMainteFunc.getInstance().LightParam[i];
                        retData = MainteCalcGray(img, mLgtP.Row1, mLgtP.Column1, mLgtP.Row2, mLgtP.Column2);
                        _lstLightControls[i].SetNowGrayValue(retData);
                    }
                }
            }
            catch (HalconException exc)
            {
                throw exc;
            }
            finally
            {
            }
            return;
        }

        /// <summary>
        /// Gray値算出
        /// </summary>
        /// <param name="img"></param>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        /// <param name="row2"></param>
        /// <param name="col2"></param>
        /// <returns></returns>
        private int MainteCalcGray(HObject img, double row1, double col1, double row2, double col2)
        {
            HObject hoRegionRect;
            HObject hoReduceDomain;
            HObject hoGrayImage;
            HOperatorSet.GenEmptyObj(out hoRegionRect);
            HOperatorSet.GenEmptyObj(out hoReduceDomain);
            HOperatorSet.GenEmptyObj(out hoGrayImage);

            HTuple htChannel;
            HTuple htMean, htDeviation;

            int channelCnt;

            int retData = 0;

            try
            {
                HOperatorSet.CountChannels(img, out htChannel);
                channelCnt = htChannel.I;

                hoRegionRect.Dispose();
                HOperatorSet.GenRectangle1(out hoRegionRect, row1, col1, row2, col2);
                hoReduceDomain.Dispose();
                HOperatorSet.ReduceDomain(img, hoRegionRect, out hoReduceDomain);

                hoGrayImage.Dispose();
                if (channelCnt == 3)
                    HOperatorSet.Rgb1ToGray(hoReduceDomain, out hoGrayImage);
                else
                    HOperatorSet.CopyObj(hoReduceDomain, out hoGrayImage, 1, -1);
                HOperatorSet.Intensity(hoRegionRect, hoGrayImage, out htMean, out htDeviation);
                retData = (int)htMean.D;
            }
            catch (HalconException exc)
            {
                throw exc;
            }
            finally
            {
                hoRegionRect.Dispose();
                hoReduceDomain.Dispose();
                hoGrayImage.Dispose();
            }
            return retData;
        }
        #endregion

        #region ROI表示・非表示
        /// <summary>
        /// ROI表示・非表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkMainteLightGrayRoi_CheckedChanged(object sender, EventArgs e)
        {
            RepaintMainteRoiRect();
        }
        private void chkMainteFcRoi_CheckedChanged(object sender, EventArgs e)
        {
            RepaintMainteRoiRect();
        }
        private void chkMainteWhiteBRoi_CheckedChanged(object sender, EventArgs e)
        {
            RepaintMainteRoiRect();
        }
        /// <summary>
        /// ROI描画
        /// </summary>
        private void RepaintMainteRoiRect()
        {
            int cameraNum = CameraManager.getInstance().CameraNum;
            for (int camIndex = 0; camIndex < cameraNum; camIndex++)
            {
                int winMultiIndex = camIndex / 4;
                int winIndex = camIndex % 4;
                MainteDisplayFocusRoi(uclHWinMulti[winMultiIndex].GetWindowControl(winIndex), camIndex);
                MainteDisplayWhiteBRoi(uclHWinMulti[winMultiIndex].GetWindowControl(winIndex), camIndex);
                MainteDisplayLightRoi(uclHWinMulti[winMultiIndex].GetWindowControl(winIndex), camIndex);
                uclHWinMulti[winMultiIndex].GetWindowControl(winIndex).repaint();
            }
        }

        public int FCFontsize { get; set; }
        public bool FCBoxEnable { get; set; }
        public string FCColor { get; set; }
        public int FocusRoiTitleFontsize { get; set; }
        public bool FocusValueBox { get; set; }
        public string FocusRoiColor { get; set; }
        public string WhiteBRoiColor { get; set; }
        public int WhiteBRoiTitleFontsize { get; set; }
        public bool WhiteBRoiTitleBox { get; set; }
        public int LightRoiTitleFontsize { get; set; }
        public bool LightRoiTitleBox { get; set; }
        public string LightRoiColor { get; set; }

        /// <summary>
        /// フォーカスROI表示
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="camIndex"></param>
        private void MainteDisplayFocusRoi(HWndCtrl ctrl, int camIndex)
        {
            clsMainteFunc.CameraParameter mCamP = clsMainteFunc.getInstance().CamParam[camIndex];
            string fcStr = string.Format("FC:{0}[Base:{1}]", _fcNowValues[camIndex].ToString("F2"), mCamP.FCBaseValue.ToString("F2"));
            string grayStr = string.Format("FCGary:{0}", _fcGrayNowValue[camIndex].ToString());

            ctrl.GraphicManager.AddText("FcValue", fcStr, 10, 10,
                FCFontsize, true, FCBoxEnable, FCColor);

            if (chkMainteFcRoi.Checked == true)
            {
                ctrl.GraphicManager.AddRectangle1("FcRoi", mCamP.FCRow1, mCamP.FCColumn1, mCamP.FCRow2, mCamP.FCColumn2,
                    FocusRoiColor);
                ctrl.GraphicManager.AddText("FCGrayValue1", grayStr, mCamP.FCRow1, mCamP.FCColumn1,
                    FocusRoiTitleFontsize, false, FocusValueBox, FocusRoiColor);
            }
            else
            {
                ctrl.GraphicManager.DeleteObject("FcRoi");
                ctrl.GraphicManager.DeleteObject("FCGrayValue1");
            }
        }
        /// <summary>
        /// ホワイトバランスROI表示
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="camIndex"></param>
        private void MainteDisplayWhiteBRoi(HWndCtrl ctrl, int camIndex)
        {
            clsMainteFunc.CameraParameter mCamP = clsMainteFunc.getInstance().CamParam[camIndex];
            string grayStr = string.Format("WBGary:{0}", _wbGrayNowValue[camIndex].ToString());
            if (chkMainteWhiteBRoi.Checked == true)
            {
                ctrl.GraphicManager.AddRectangle1("WhiteBRoi", mCamP.WhiteBRow1, mCamP.WhiteBColumn1, mCamP.WhiteBRow2, mCamP.WhiteBColumn2,
                    WhiteBRoiColor);
                ctrl.GraphicManager.AddText("WhiteBGrayValue1", grayStr, mCamP.WhiteBRow1, mCamP.WhiteBColumn1,
                    WhiteBRoiTitleFontsize, false, WhiteBRoiTitleBox, WhiteBRoiColor);
            }
            else
            {
                ctrl.GraphicManager.DeleteObject("WhiteBRoi");
                ctrl.GraphicManager.DeleteObject("WhiteBGrayValue1");
                ctrl.GraphicManager.DeleteObject("WhiteBGrayValue2");
            }
        }

        /// <summary>
        /// 照明ROI表示
        /// </summary>
        /// <param name="ctrl"></param>
        /// <param name="camIndex"></param>
        private void MainteDisplayLightRoi(HWndCtrl ctrl, int camIndex)
        {
            for (int i = 0; i < _lstLightControls.Count; i++)
            {
                if (_lstLightControls[i].Enabled == false)
                    break;
                if (camIndex == _lstLightControls[i].CameraIndex)
                {
                    clsMainteFunc.LightParameter mLgtP = clsMainteFunc.getInstance().LightParam[i];
                    string objName = "_lstLightControls" + i.ToString();
                    if (chkMainteLightGrayRoi.Checked == true)
                    {
                        ctrl.GraphicManager.AddRectangle1(objName, mLgtP.Row1, mLgtP.Column1, mLgtP.Row2, mLgtP.Column2, LightRoiColor);
                        ctrl.GraphicManager.AddText(objName + "id", (i + 1).ToString() + ":" + _lstLightControls[i].LigthName, mLgtP.Row1, mLgtP.Column1,
                            LightRoiTitleFontsize, false, LightRoiTitleBox, LightRoiColor);
                    }
                    else
                    {
                        ctrl.GraphicManager.DeleteObject(objName);
                        ctrl.GraphicManager.DeleteObject(objName + "id");
                    }
                }
            }
        }
        #endregion
        #endregion
    }


}
