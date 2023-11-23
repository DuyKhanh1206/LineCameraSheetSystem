using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace LineCameraSheetSystem
{
    public partial class frmImageMainParam : Form
    {
        public frmImageMainParam()
        {
            InitializeComponent();
        }

        uclImageMain _uclImageMain;
        CheckBox[] _chkBoxDisplayTargetImage;

        public frmImageMainParam(uclImageMain ucl)
        {
            _uclImageMain = ucl;
            InitializeComponent();
        }
        private void frmImageMainParam_Load(object sender, EventArgs e)
        {
            _chkBoxDisplayTargetImage = new CheckBox[] { chkTargetOrg, chkTargetGray, chkTargetRed, chkTargetGreen, chkTargetBlue };
            for (int i = 0; i < _chkBoxDisplayTargetImage.Length; i++)
                _chkBoxDisplayTargetImage[i].Tag = i;

            //モノクロカメラ時は、表示する画像(RGB,Gray…)の選択タブは必要なし
            if (HalconCamera.APCameraManager.getInstance().GetCamera(0).IsColor == false)
            {
                grpTargetImage.Visible = false;
            }

            SetSystemParam();
        }

        private bool _setPartsFlag = false;
        public void SetSystemParam()
        {
            SystemParam sysp = SystemParam.GetInstance();

            _setPartsFlag = true;
            chkInspHeight.Checked = sysp.IM_DispInspHeight;
            chkInspWidth.Checked = sysp.IM_DispInspWidth;
            chkMaskWidth.Checked = sysp.IM_DispMaskWidth;
            chkGraphLight.Checked = sysp.IM_DispGraphLight;
            chkGraphDark.Checked = sysp.IM_DispGraphDark;
            chkGraphAvg.Checked = sysp.IM_DispGraphAvg;
            chkKando.Checked = sysp.IM_DispKandoLine;
            spinImageBufferCount.Value = sysp.IM_ImageBufferCount;
            //spinImageBufferCount.Enabled = true;
            spinNgCropSaveCount.Value = sysp.IM_NgCropSaveCount;
            chkAutoSave.Checked = sysp.IM_AutoSaveEnable;
            spinAutoSaveCount.Value = sysp.IM_AutoSaveCount;
            spinAutoSaveOneNgsaveCount.Value = sysp.IM_AutoSaveOneNGsaveCount;
            spinDispGraphWidth3ch.Value = sysp.IM_DispGraphWidth3ch;
            spinDispGraphWidth1ch.Value = sysp.IM_DispGraphWidth1ch;
            chkGraphCalcAll.Checked = sysp.IM_GraphCalcAreaAll;
            spinInspAreaConnectModeImagePoint.Value = sysp.InspArea_ConnectMode_ImagePoint;
            spinInspAreaConnectModeBufferArea.Value = sysp.InspArea_ConnectMode_BufferArea;
            chkOrgImageConnectMode.Checked = sysp.IM_OrgImageConnectMode;
            spinInspFuncNgMax.Value = sysp.InspFunc_CountNgMax;
            spinInspFuncClosing.Value = (decimal)sysp.InspFunc_BlobClosingCircle;
            spinInspFuncOpening.Value = (decimal)sysp.InspFunc_BlobOpeningCircle;
            spinInspFuncSelectArea.Value = sysp.InspFunc_BlobSelectArea;
            spinAutoLightCheckImageCount.Value = sysp.AutoLightCheckImageCount;
            spinAutoLightOkImageCount.Value = sysp.AutoLightOkImageCount;
            spinAutoLightOkLowLimit.Value = sysp.AutoLightOkLowLimit;
            spinAutoLightOkHighLimit.Value = sysp.AutoLightOkHighLimit;
            spinAutoLightDetailUpLevel.Value = sysp.AutoLightDetailUpLevel;

            _setPartsFlag = false;
        }

        #region 線表示
        /// <summary>
        /// 検査高さ表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkInspHeight_CheckedChanged(object sender, EventArgs e)
        {
            SystemParam sysp = SystemParam.GetInstance();
            sysp.IM_DispInspHeight = chkInspHeight.Checked;
            _uclImageMain.refreshImageBuffer();
        }
        /// <summary>
        /// 検査幅表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkInspWidth_CheckedChanged(object sender, EventArgs e)
        {
            SystemParam sysp = SystemParam.GetInstance();
            sysp.IM_DispInspWidth = chkInspWidth.Checked;
            _uclImageMain.refreshImageBuffer();
        }
        /// <summary>
        /// マスク幅表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkMaskWidth_CheckedChanged(object sender, EventArgs e)
        {
            SystemParam sysp = SystemParam.GetInstance();
            sysp.IM_DispMaskWidth = chkMaskWidth.Checked;
            _uclImageMain.refreshImageBuffer();
        }
        /// <summary>
        /// グラフ（明）表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkGraphLight_CheckedChanged(object sender, EventArgs e)
        {
            SystemParam sysp = SystemParam.GetInstance();
            sysp.IM_DispGraphLight = chkGraphLight.Checked;
            _uclImageMain.refreshImageBuffer();
        }
        /// <summary>
        /// グラフ（暗）表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkGraphDark_CheckedChanged(object sender, EventArgs e)
        {
            SystemParam sysp = SystemParam.GetInstance();
            sysp.IM_DispGraphDark = chkGraphDark.Checked;
            _uclImageMain.refreshImageBuffer();
        }
        /// <summary>
        /// グラフ（平均）表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkGraphAvg_CheckedChanged(object sender, EventArgs e)
        {
            SystemParam sysp = SystemParam.GetInstance();
            sysp.IM_DispGraphAvg = chkGraphAvg.Checked;
            _uclImageMain.refreshImageBuffer();
        }
        /// <summary>
        /// 感度ライン表示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkKando_CheckedChanged(object sender, EventArgs e)
        {
            SystemParam sysp = SystemParam.GetInstance();
            sysp.IM_DispKandoLine = chkKando.Checked;
            _uclImageMain.refreshImageBuffer();
        }
        #endregion
        #region 画像表示
        private void chkDisplayTargetImage_CheckedChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            int index = (int)((CheckBox)sender).Tag;
            _setPartsFlag = true;
            for (int i = 0; i < _chkBoxDisplayTargetImage.Length; i++)
            {
                _chkBoxDisplayTargetImage[i].Checked = (i == index);
                if (i == index)
                    _uclImageMain._displayTargetImage = index;
            }
            _setPartsFlag = false;

            _uclImageMain.refreshImageBuffer();
        }
        private void chkOrgImageConnectMode_CheckedChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.IM_OrgImageConnectMode = chkOrgImageConnectMode.Checked;
        }
        #endregion
        #region 保存１
        /// <summary>
        /// 取込バッファ数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spinImageBufferCount_ValueChanged(object sender, EventArgs e)
        {
            SystemParam sysp = SystemParam.GetInstance();
            sysp.IM_ImageBufferCount = (int)spinImageBufferCount.Value;
            _setPartsFlag = true;
            _uclImageMain.spinImageDispNo.Value = 1;
            _uclImageMain.spinImageDispNo.Maximum = sysp.IM_ImageBufferCount;
            _setPartsFlag = false;
        }
        /// <summary>
        /// 保存ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            string s;
            HObject saveImg;
            HOperatorSet.GenEmptyObj(out saveImg);

            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                DialogResult res = dlg.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    s = dlg.SelectedPath;
                    for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
                    {
                        int no = 0;
                        List<HObject> imageOrg = (side == 0) ? _uclImageMain._imageOrgOmote : _uclImageMain._imageOrgUra;
                        for (int imgNo = imageOrg.Count - 1; imgNo >= 0; imgNo--)
                        {
                            if (imageOrg[imgNo] != null)
                            {
                                saveImg.Dispose();
                                _uclImageMain.CutOverlapImage(imageOrg[imgNo], out saveImg);
                                HOperatorSet.WriteImage(saveImg, "bmp", 0, System.IO.Path.Combine(s, string.Format("{0}_{1:D3}.bmp", side, no++)));
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region 保存２
        private void chkAutoSave_CheckedChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.IM_AutoSaveEnable = chkAutoSave.Checked;
            _uclImageMain._bBackAutoSaveEnable = sysp.IM_AutoSaveEnable;
        }
        private void spinAutoSaveCount_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.IM_AutoSaveCount = (int)spinAutoSaveCount.Value;
        }
        private void spinAutoSaveOneNgsaveCount_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.IM_AutoSaveOneNGsaveCount = (int)spinAutoSaveOneNgsaveCount.Value;
        }
        #endregion
        #region 保存３
        private void spinNgCropSaveCount_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.IM_NgCropSaveCount = (int)spinNgCropSaveCount.Value;
        }
        #endregion
        #region 設定
        private void spinDispGraphWidth3ch_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam.GetInstance().IM_DispGraphWidth3ch = (int)spinDispGraphWidth3ch.Value;
        }
        private void spinDispGraphWidth1ch_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam.GetInstance().IM_DispGraphWidth1ch = (int)spinDispGraphWidth1ch.Value;
        }
        private void chkGraphCalcAll_CheckedChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam.GetInstance().IM_GraphCalcAreaAll = chkGraphCalcAll.Checked;
        }
        #endregion
        #region 検査１
        private void spinInspAreaConnectModeImagePoint_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam.GetInstance().InspArea_ConnectMode_ImagePoint = (int)spinInspAreaConnectModeImagePoint.Value;
        }
        private void spinInspAreaConnectModeBufferArea_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam.GetInstance().InspArea_ConnectMode_BufferArea = (int)spinInspAreaConnectModeBufferArea.Value;
        }
        #endregion
        #region 検査２
        private void spinInspFuncNgMax_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.InspFunc_CountNgMax = (int)spinInspFuncNgMax.Value;
        }

        private void spinInspFuncClosing_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.InspFunc_BlobClosingCircle = (int)spinInspFuncClosing.Value;
        }

        private void spinInspFuncOpening_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.InspFunc_BlobOpeningCircle = (int)spinInspFuncOpening.Value;
        }

        private void spinInspFuncSelectArea_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.InspFunc_BlobSelectArea = (int)spinInspFuncSelectArea.Value;
        }
        #endregion
        #region 自動調光
        private void spinAutoLightCheckImageCount_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.AutoLightCheckImageCount = (int)spinAutoLightCheckImageCount.Value;
        }
        private void spinAutoLightOkImageCount_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.AutoLightOkImageCount = (int)spinAutoLightOkImageCount.Value;
        }

        private void spinAutoLightOkLowLimit_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.AutoLightOkLowLimit = (int)spinAutoLightOkLowLimit.Value;
        }

        private void spinAutoLightOkHighLimit_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.AutoLightOkHighLimit = (int)spinAutoLightOkHighLimit.Value;
        }

        private void spinAutoLightDetailUpLevel_ValueChanged(object sender, EventArgs e)
        {
            if (_setPartsFlag == true)
                return;
            SystemParam sysp = SystemParam.GetInstance();
            sysp.AutoLightDetailUpLevel = (int)spinAutoLightDetailUpLevel.Value;
        }
        #endregion
    }
}
