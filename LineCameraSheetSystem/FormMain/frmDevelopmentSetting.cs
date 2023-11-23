using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconCamera;
using Fujita.InspectionSystem;

namespace LineCameraSheetSystem
{
    public partial class frmDevelopmentSetting : Form
    {
        private MainForm _mainFrom;

        private CheckBox[] _colCamInsp;

        private uclNumericInputSmall[] _spinCamSpeed;
        private Label[] _lblCamHz;
        private Label[] _lblCamExp;
        private uclNumericInputSmall[] _spinExposureTime;

        public frmDevelopmentSetting(MainForm mf)
        {
            _mainFrom = mf;
            InitializeComponent();
        }

        private void setEvent()
        {
            for (int i=0; i< _spinCamSpeed.Length; i++)
            {
                _spinCamSpeed[i].Tag = i;
                _spinCamSpeed[i].ValueChanged += spinCamSpeed_ValueChanged;
            }
            for (int i=0; i<_spinExposureTime.Length; i++)
            {
                _spinExposureTime[i].Tag = i;
                _spinExposureTime[i].ValueChanged += spinCamExposure_ValueChanged;
            }
        }
        private void resetEvent()
        {
            for (int i = 0; i < _spinCamSpeed.Length; i++)
            {
                _spinCamSpeed[i].ValueChanged -= spinCamSpeed_ValueChanged;
            }
            for (int i = 0; i < _spinExposureTime.Length; i++)
            {
                _spinExposureTime[i].ValueChanged -= spinCamExposure_ValueChanged;
            }
        }

        private void frmDevelopmentSetting_Load(object sender, EventArgs e)
        {
            _colCamInsp = new CheckBox[] {chkColInspGray, chkColInspRed, chkColInspGreen, chkColInspBlue };
            _spinCamSpeed = new uclNumericInputSmall[] { spinCamSpeed, spinCamSpeedUra };
            _lblCamHz = new Label[] { lblCamHz, lblCamHzUra};
            _lblCamExp = new Label[] { lblCamExp, lblCamExpUra };
            _spinExposureTime = new uclNumericInputSmall[] { spinCamExposure, spinCamExposureUra };

            setEvent();

            SystemParam systemparam = SystemParam.GetInstance();

            chkInspBright.Checked = systemparam.InspBrightEnable;
            chkInspDark.Checked = systemparam.InspDarkEnable;

            for (int i = 0; i < _colCamInsp.Length; i++)
                _colCamInsp[i].Checked = systemparam.ColorCamInspImage[i];

            //白黒除外
            chkOutofWhiteEnable.Checked = systemparam.OutofWhiteEnabled;
            chkOutofBlackEnable.Checked = systemparam.OutofBlackEnabled;
            spinOutofWhiteLimit.Value = systemparam.OutofWhiteLimit;
            spinOutofBlackLimit.Value = systemparam.OutofBlackLimit;

            chkSoftShadingEnable.Checked = systemparam.SoftShadingEnable;
            spinSoftShadingTargetGrayLevel.Value = systemparam.SoftShadingTargetGrayLevel;
            spinSoftShadingLimit.Value = systemparam.SoftShadingLimit;
            spinSoftShadingCalcCnt.Value = systemparam.SoftShadingCalcImageCount;

            chkSideMaskEnable.Checked = systemparam.SideMaskEnable;
            spinSideMaskThreshold.Value = systemparam.SideMaskThreshold;
            spinSideMaskDilation.Value = systemparam.SideMaskDilation;

            resetEvent();
            //速度
            SetSpeedRange();
            for (int i = 0; i < _spinCamSpeed.Length; i++)
            {
                double val = (i == 0) ? systemparam.CamSpeed : systemparam.CamSpeedUra;

                if ((decimal)val < _spinCamSpeed[i].Minimum || _spinCamSpeed[i].Maximum < (decimal)val)
                    _spinCamSpeed[i].Value = (decimal)systemparam.Hz2Speed(CameraManager.getInstance().GetCamera(0).LineRate);
                else
                    _spinCamSpeed[i].Value = (decimal)val;
                systemparam.DispHz(systemparam.Speed2Hz((double)_spinCamSpeed[i].Value), _lblCamHz[i], _lblCamExp[i]);
            }

            //露光値
            SetExposureRange();
            for (int i = 0; i < _spinExposureTime.Length; i++)
            {
                int val = (i == 0) ? systemparam.CamExposure : systemparam.CamExposureUra;
                if (val < _spinExposureTime[i].Minimum || _spinExposureTime[i].Maximum < val)
                    _spinExposureTime[i].Value = CameraManager.getInstance().GetCamera(0).ExposureTime;
                else
                    _spinExposureTime[i].Value = val;
            }
            setEvent();

            //
            chkCamCloseOpenEnable.Checked = systemparam.CamCloseOpenEnable;
            chkCamCloseOpenAutoLightEnable.Checked = systemparam.CamCloseOpenAutoLightingEnable;
        }

        private int[] GetLinkCameraNumber()
        {
            SystemParam sysp = SystemParam.GetInstance();
            int[] camNo = new int[] { 0, 0 };
            foreach (CameraParam cp in sysp.camParam)
            {
                if (cp.OnOff == true)
                {
                    camNo[(int)cp.CamParts] = cp.CamNo;
                }
            }
            return camNo;
        }
        private void SetSpeedRange()
        {
            int[] camNo = GetLinkCameraNumber();

            SystemParam sysp = SystemParam.GetInstance();
            for (int i = 0; i < _spinCamSpeed.Length; i++)
            {
                double min, max, step, now;
                min = max = step = now = -1;
                CameraManager.getInstance().GetCamera(camNo[i]).GetLineRateRange(ref min, ref max, ref step, ref now);

                double val;
                val = sysp.Hz2Speed(min);
                _spinCamSpeed[i].Minimum = (decimal)val;
                val = sysp.Hz2Speed(max);
                _spinCamSpeed[i].Maximum = (decimal)val;
            }
        }
        private void SetExposureRange()
        {
            int[] camNo = GetLinkCameraNumber();

            for (int i = 0; i < _spinExposureTime.Length; i++)
            {
                int min, max, step, now;
                min = max = step = now = -1;
                CameraManager.getInstance().GetCamera(camNo[i]).GetExposureTimeRange(ref min, ref max, ref step, ref now);

                _spinExposureTime[i].Minimum = min;
                _spinExposureTime[i].Maximum =max;
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            SystemParam systemparam = SystemParam.GetInstance();

            systemparam.InspBrightEnable= chkInspBright.Checked;
            systemparam.InspDarkEnable = chkInspDark.Checked;

            for (int i = 0; i < _colCamInsp.Length; i++)
                systemparam.ColorCamInspImage[i] = _colCamInsp[i].Checked;

            //白黒除外
            systemparam.OutofWhiteEnabled = chkOutofWhiteEnable.Checked;
            systemparam.OutofBlackEnabled = chkOutofBlackEnable.Checked;
            systemparam.OutofWhiteLimit = (int)spinOutofWhiteLimit.Value;
            systemparam.OutofBlackLimit = (int)spinOutofBlackLimit.Value;

            systemparam.SoftShadingEnable = chkSoftShadingEnable.Checked;
            systemparam.SoftShadingTargetGrayLevel = (int)spinSoftShadingTargetGrayLevel.Value;
            systemparam.SoftShadingLimit = (int)spinSoftShadingLimit.Value;
            systemparam.SoftShadingCalcImageCount = (int)spinSoftShadingCalcCnt.Value;

            systemparam.SideMaskEnable = chkSideMaskEnable.Checked;
            systemparam.SideMaskThreshold = (int)spinSideMaskThreshold.Value;
            systemparam.SideMaskDilation = (int)spinSideMaskDilation.Value;

            systemparam.CamExposure = (int)spinCamExposure.Value;
            systemparam.CamSpeed = (double)spinCamSpeed.Value;
            systemparam.CamExposureUra = (int)spinCamExposureUra.Value;
            systemparam.CamSpeedUra = (double)spinCamSpeedUra.Value;
            systemparam.CamCloseOpenEnable = chkCamCloseOpenEnable.Checked;
            systemparam.CamCloseOpenAutoLightingEnable = chkCamCloseOpenAutoLightEnable.Checked;

            systemparam.SystemSave();

            this.Cursor = Cursors.Default;
            MessageBox.Show("適応を完了しました。");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkSoftShadingRun_CheckedChanged(object sender, EventArgs e)
        {
            _mainFrom.AutoInspection.ShadingStart = chkSoftShadingRun.Checked;
        }

        /// <summary>
        /// 速度値　ValueChange
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spinCamSpeed_ValueChanged(object sender, EventArgs e)
        {
            int no = (int)(((uclNumericInputSmall)sender).Tag);

            double hz = SystemParam.GetInstance().Speed2Hz((double)_spinCamSpeed[no].Value);
            SystemParam.GetInstance().DispHz(hz, _lblCamHz[no], _lblCamExp[no]);

            resetEvent();
            SetExposureRange();
            setEvent();
        }
        private void spinCamExposure_ValueChanged(object sender, EventArgs e)
        {
            resetEvent();
            SetSpeedRange();
            setEvent();
        }

        /// <summary>
        /// 露光値をカメラへ設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCamExposure_Click(object sender, EventArgs e)
        {
            //this.Cursor = Cursors.WaitCursor;
            //int exposure;
            //for (int i = 0; i < APCameraManager.getInstance().CameraNum; i++)
            //{
            //    AppData.SideID side;
            //    if (true == SystemParam.GetInstance().CheckCameraIndex(i, out side))
            //    {
            //        exposure = (side == AppData.SideID.表) ? (int)spinCamExposure.Value : (int)spinCamExposureUra.Value;

            //        HalconCameraBase cam = APCameraManager.getInstance().GetCamera(i);
            //        cam.SetExposureTime(exposure);
            //    }
            //}
            SystemParam.GetInstance().CamExposure = (int)spinCamExposure.Value;
            SystemParam.GetInstance().CamExposureUra = (int)spinCamExposureUra.Value;
            _mainFrom.CameraLightHosei(true);

//            resetEvent();
//            SetSpeedRange();
//            SetExposureRange();
//            setEvent();

            this.Cursor = Cursors.Default;
        }
        /// <summary>
        /// 周波数をカメラへ設定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCamSpeed_Click(object sender, EventArgs e)
        {
            //this.Cursor = Cursors.WaitCursor;
            //double hz;
            //for (int i = 0; i < APCameraManager.getInstance().CameraNum; i++)
            //{
            //    AppData.SideID side;
            //    if (true == SystemParam.GetInstance().CheckCameraIndex(i, out side))
            //    {
            //        //double bufHz = (side == AppData.SideID.表) ? (double)spinCamSpeed.Value : (double)spinCamSpeedUra.Value;
            //        //hz = SystemParam.GetInstance().Speed2Hz(bufHz);
            //        //HalconCameraBase cam = APCameraManager.getInstance().GetCamera(i);
            //        //cam.SetLineRate(hz);
            //    }
            //}
            SystemParam.GetInstance().CamSpeed = (double)spinCamSpeed.Value;
            SystemParam.GetInstance().CamSpeedUra = (double)spinCamSpeedUra.Value;
            _mainFrom.CameraLightHosei(true);

//            resetEvent();
//            SetSpeedRange();
//            SetExposureRange();
//            setEvent();

            this.Cursor = Cursors.Default;
        }

        private void btnSystemIniSaveBackup_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            SystemParam.GetInstance().SystemSave();
            SystemParam.GetInstance().SystemSaveBackup();
            this.Cursor = Cursors.Default;
        }

        private void btnSystemIniLoad_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            SystemParam.GetInstance().SystemLoad();
            this.Cursor = Cursors.Default;
        }
    }
}
