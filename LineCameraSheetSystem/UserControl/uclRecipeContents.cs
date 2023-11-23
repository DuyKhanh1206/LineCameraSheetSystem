using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fujita.InspectionSystem;
using Fujita.LightControl;
using Fujita.Misc;
using System.IO;

using HalconDotNet;
using ViewROI;
using InspectionNameSpace;
using HalconCamera;

namespace LineCameraSheetSystem
{
    public partial class uclRecipeContents : UserControl, IShortcutClient
    {
        /// <summary>
        /// MainForm
        /// </summary>
        MainForm _mainForm { get; set; }

        /// <summary>
        /// ViewROI
        /// </summary>
        ViewROI.HWndCtrl _hwndCtrl = null;

        /// <summary>
        /// 画像表示倍率
        /// </summary>
        private double[] _magnifyList = new double[] { 0.1, 0.11, 0.12, 0.13, 0.14, 0.16, 0.18, 0.2, 0.25, 0.265, 0.27, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 1.0, 2.0 };

        /// <summary>
        /// 感度GridView
        /// </summary>
        private DataGridView[] _dgvInsp;

        /// <summary>
        /// 検査幅GridView
        /// </summary>
        private uclRecipeInspectWidth[] _uclInspWidth;

        /// <summary>
        /// 分割GridView
        /// </summary>
        private DataGridView[] _dgvZone;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public uclRecipeContents()
        {
            InitializeComponent();
            //v1324 ZoneSettingEnableによって非表示に
            chkBothEndAny.Visible = SystemParam.GetInstance().RecipeZoneSetteingEnable;
            //v1326 岐阜以外では非表示
            btnPatLite.Visible = SystemParam.GetInstance().GCustomEnable;
        }

        /// <summary>
        /// SetMainForm
        /// </summary>
        /// <param name="_mf"></param>
        public void SetMainForm(MainForm _mf)
        {
            _mainForm = _mf;

            btnExternalOutput.Text = "外部出力" + System.Environment.NewLine + "(品種)";

            _dgvInsp = new DataGridView[] { dgvInspUpSide, dgvInspDownSide };
            _uclInspWidth = new uclRecipeInspectWidth[] { uclInspectWidthUpSide, uclInspectWidthDownSide };
            _dgvZone = new DataGridView[] { dgvZoneUpSide, dgvZoneDownSide };

            //感度
            KandoDataGridViewCreate();

            //Zone
            ZoneDataGridViewCreate();

            shortcutKeyHelper1.SetShortcutKeys(btnSave, Keys.S);
            shortcutKeyHelper1.SetShortcutKeys(btnLightAjust, Keys.A);
            shortcutKeyHelper1.SetShortcutKeys(btnUndo, Keys.R);

            _hwndCtrl = new HWndCtrl(hwcImageWnd);
            _hwndCtrl.Fitting = false;
            _hwndCtrl.SetScrollbarControl(hScrollBar1, vScrollBar1);
            _hwndCtrl.useGraphManager(new GraphicsManager());
            _hwndCtrl.FirstTimeFitting = true;
            _hwndCtrl.DoubleClickTime = _hwndCtrl.DoubleClickTime;
            _hwndCtrl.DoubleClickArea = _hwndCtrl.DoubleClickArea;
            _hwndCtrl.SetMagnifyList(_magnifyList);
            //_hwndCtrl.Repaint += new RepaintEventHandler(_hwndCtrl_Repaint);
            _hwndCtrl.SetBackColor();
        }




        /// <summary>
        /// 感度DataGridView　Initialize
        /// </summary>
        private void KandoDataGridViewCreate()
        {
            Color colKandoTitleBack = Color.FromArgb(255, 212, 208, 200);
            Color colKandoTitleFore = Color.Black;
            //感度
            for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
            {
                _dgvInsp[side].Rows.Add(4);

                _dgvInsp[side][0, 0].Value = "感度";
                _dgvInsp[side][0, 1].Value = "縦[㎜]";
                _dgvInsp[side][0, 2].Value = "横[㎜]";
                _dgvInsp[side][0, 3].Value = "面積[㎜²]";
                for (int i = 0; i < 4; i++)
                {
                    _dgvInsp[side][0, i].Style.SelectionBackColor = colKandoTitleBack;
                    _dgvInsp[side][0, i].Style.BackColor = colKandoTitleBack;
                    _dgvInsp[side][0, i].Style.SelectionForeColor = colKandoTitleFore;
                    _dgvInsp[side][0, i].Style.ForeColor = colKandoTitleFore;
                    _dgvInsp[side][0, i].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
                //_dgvInsp[side][0, 0].Selected = false;

                for (int inspID = 0; inspID < Enum.GetNames(typeof(AppData.InspID)).Length; inspID++)
                {
                    string ttl = Enum.GetNames(typeof(AppData.InspID))[inspID];
                    _dgvInsp[side].Columns[inspID + 1].HeaderText = ttl.Replace("暗", "");
                }

            }
        }

        /// <summary>
        /// Zone　DataGridView　Initialize
        /// </summary>
        private void ZoneDataGridViewCreate()
        {
            //Zone
            for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
            {
                _dgvZone[side].Rows.Add(1);
            }
        }

        /// <summary>
        /// 照明コントロールInitialize
        /// </summary>
        private void InitLightControl()
        {
            LightControlManager ltCtrl = LightControlManager.getInstance();
            //照明台数
            if (ltCtrl.LightCount != spinLightNo.Maximum)
            {
                spinLightNo.Maximum = ltCtrl.LightCount;
                spinLightNo.Minimum = 1;
                spinLightNo.Value = 1;
            }
            grpLightControl.Visible = (ltCtrl.LightCount != 0);
            if (grpLightControl.Visible == true)
                uclLightControl.LightMaxValue = ltCtrl.GetLight(0).ValueMax;
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uclRecipeContents_Load(object sender, EventArgs e)
        {
            SystemParam sysParam = SystemParam.GetInstance();
            chkDispDownSide.Visible = sysParam.DownSideEnable;
            chkInspDownSide.Visible = sysParam.DownSideEnable;

            //待機中での自動調光実施ボタン
            btnLightAjust.Visible = sysParam.LightAjustEnable;
        }

        #region コールバック
        /// <summary>
        /// 保存　ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes != Utility.ShowMessage(_mainForm, "保存しますか？", MessageType.YesNo))
                return;

            SaveRecipe();

            SystemStatus.GetInstance().RecipeEdit = false;
            _clsCheckRecipeEdit.AllFalse();
        }
        /// <summary>
        /// 戻す　ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUndo_Click(object sender, EventArgs e)
        {
            Recipe recipe = Recipe.GetInstance();
            if (_mainForm.CancelRecipe != null)
            {
                _mainForm.CancelRecipe.Copy(recipe);
                SetRecipeDisp();

                SystemStatus.GetInstance().RecipeEdit = false;
                _clsCheckRecipeEdit.AllFalse();
            }
        }
        /// <summary>
        /// 表　チェックボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDispUpSide_CheckedChanged(object sender, EventArgs e)
        {
            if (_chkDispSideFlag == true)
                return;
            _chkDispSideFlag = true;
            if (chkDispUpSide.Checked == false)
            {
                chkDispUpSide.Checked = true;
            }
            else
            {
                lblRealSpeedTitle.Text = "表";
                spinCamRealSpeed.Value = (decimal)_dCamRealSpeed;

                chkDispDownSide.Checked = false;
                _dispSelectSide = 0;
                for (int i = 0; i < Enum.GetNames(typeof(AppData.SideID)).Length; i++)
                {
                    bool enable = (_dispSelectSide == i) ? true : false;
                    _dgvInsp[i].Visible = enable;
                    _uclInspWidth[i].Visible = enable;
                    _dgvZone[i].Visible = enable;
                }
            }
            _chkDispSideFlag = false;
        }
        /// <summary>
        /// 裏　チェックボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkDispDownSide_CheckedChanged(object sender, EventArgs e)
        {
            if (_chkDispSideFlag == true)
                return;
            _chkDispSideFlag = true;
            if (chkDispDownSide.Checked == false)
            {
                chkDispDownSide.Checked = true;
            }
            else
            {
                lblRealSpeedTitle.Text = "裏";
                spinCamRealSpeed.Value = (decimal)_dCamRealSpeedUra;

                chkDispUpSide.Checked = false;
                _dispSelectSide = 1;
                for (int i = 0; i < Enum.GetNames(typeof(AppData.SideID)).Length; i++)
                {
                    bool enable = (_dispSelectSide == i) ? true : false;
                    _dgvInsp[i].Visible = enable;
                    _uclInspWidth[i].Visible = enable;
                    _dgvZone[i].Visible = enable;
                }
            }
            _chkDispSideFlag = false;
        }
        /// <summary>
        /// 表裏Common
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkUpDownSideCmmon_CheckedChanged(object sender, EventArgs e)
        {
            _dgvInsp[1].Enabled = !chkUpDownSideCmmon.Checked;

            //変更があったか確認
            _clsCheckRecipeEdit.bUpDownSideCommon = (chkUpDownSideCmmon.Checked != Recipe.GetInstance().UpDownSideCommon);
        }
        /// <summary>
        /// 感度：数値
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvInsp_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex;
            int colIndex = e.ColumnIndex;
            if (colIndex < 0 || rowIndex < 0)
                return;

            DataGridView dgv = (DataGridView)sender;
            DataGridViewCell dg = dgv[colIndex, rowIndex];

            if (colIndex == 0)
                return;

            SystemParam sp = SystemParam.GetInstance();
            if (sp.InspBrightEnable == false)
            {
                if (colIndex <= 3)
                    return;
            }
            if (sp.InspDarkEnable == false)
            {
                if (colIndex >= 4)
                    return;
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                frmTenkey tenkey = new frmTenkey();
                decimal min, max;
                int dec;
                if (rowIndex == 0)
                {
                    min = 0; max = 128; dec = 0;
                }
                else
                {
                    min = 0; max = 100000; dec = 1;
                }
                tenkey.SetValues(min, max, (decimal)Convert.ToDouble(dg.Value), dec);
                if (DialogResult.OK == tenkey.ShowDialog())
                {
                    dg.Value = tenkey.Value;
                    ChangeInspData(colIndex, rowIndex, dgv.Tag.ToString());
                }
            }
        }
        /// <summary>
        /// 検査幅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spinWidth_ValueChanged(object sender, EventArgs e)
        {
            //Zone別の幅更新
            SetZoneValue(_dispSelectSide);

            _clsCheckRecipeEdit.InspWidth = (_uclInspWidth[_dispSelectSide].SheetWidth != Recipe.GetInstance().InspParam[_dispSelectSide].Width);

            if (_mainForm != null && _mainForm.IsControlReal(this))
            {
                for (int i = 0; i < _uclInspWidth.Length; i++)
                {
                    SetInspAreaData_Data2Cmn(_uclInspWidth[i].CommonInspAreaEnable, i);
                }

                bool bCommon = _uclInspWidth[0].CommonInspAreaEnable;
                setMaskWidthAndInspWidth(
                    new double[] { GetInspArea_MaskWidth(bCommon, 0), GetInspArea_MaskWidth(bCommon, 1) },
                    new double[] { GetInspArea_MaskShift(bCommon, 0), GetInspArea_MaskShift(bCommon, 1) },
                    new double[] { GetInspArea_SheetWidth(bCommon, 0), GetInspArea_SheetWidth(bCommon, 1) });
                refreshImage();
            }
        }
        /// <summary>
        /// マスク幅
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spinMaskWidth_ValueChanged(object sender, EventArgs e)
        {
            _clsCheckRecipeEdit.MaskWidth = (_uclInspWidth[_dispSelectSide].MaskWidth != Recipe.GetInstance().InspParam[_dispSelectSide].MaskWidth);

            if (_mainForm != null && _mainForm.IsControlReal(this))
            {
                for (int i = 0; i < _uclInspWidth.Length; i++)
                {
                    SetInspAreaData_Data2Cmn(_uclInspWidth[i].CommonInspAreaEnable, i);
                }

                bool bCommon = _uclInspWidth[0].CommonInspAreaEnable;
                setMaskWidthAndInspWidth(
                    new double[] { GetInspArea_MaskWidth(bCommon, 0), GetInspArea_MaskWidth(bCommon, 1) },
                    new double[] { GetInspArea_MaskShift(bCommon, 0), GetInspArea_MaskShift(bCommon, 1) },
                    new double[] { GetInspArea_SheetWidth(bCommon, 0), GetInspArea_SheetWidth(bCommon, 1) });
                refreshImage();
            }

            //v1324 ゾーン幅計算をメソッド化
            this.ZoneRecalc();
        }
        /// <summary>
        /// シフト
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spinMaskShift_ValueChanged(object sender, EventArgs e)
        {
            _clsCheckRecipeEdit.MaskShift = (_uclInspWidth[_dispSelectSide].MaskShift != Recipe.GetInstance().InspParam[_dispSelectSide].MaskShift);

            if (_mainForm != null && _mainForm.IsControlReal(this))
            {
                for (int i = 0; i < _uclInspWidth.Length; i++)
                {
                    SetInspAreaData_Data2Cmn(_uclInspWidth[i].CommonInspAreaEnable, i);
                }

                bool bCommon = _uclInspWidth[0].CommonInspAreaEnable;
                setMaskWidthAndInspWidth(
                    new double[] { GetInspArea_MaskWidth(bCommon, 0), GetInspArea_MaskWidth(bCommon, 1) },
                    new double[] { GetInspArea_MaskShift(bCommon, 0), GetInspArea_MaskShift(bCommon, 1) },
                    new double[] { GetInspArea_SheetWidth(bCommon, 0), GetInspArea_SheetWidth(bCommon, 1) });
                refreshImage();
            }
        }
        /// <summary>
        /// 検査領域共通・個別
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void chk_OnCommonInspArea(object sender, EventArgs args)
        {
            uclRecipeInspectWidth uclInspW = sender as uclRecipeInspectWidth;
            bool bCommon = uclInspW.CommonInspAreaEnable;
            for (int i = 0; i < _uclInspWidth.Length; i++)
            {
                _uclInspWidth[i].CommonInspAreaEnable = bCommon;
                SetInspAreaData_Data2Cmn(bCommon, i);
                SetZoneValue(i);
                //
                _clsCheckRecipeEdit.CommonInspArea = true;
            }
            setMaskWidthAndInspWidth(
                new double[] { GetInspArea_MaskWidth(bCommon, 0), GetInspArea_MaskWidth(bCommon, 1) },
                new double[] { GetInspArea_MaskShift(bCommon, 0), GetInspArea_MaskShift(bCommon, 1) },
                new double[] { GetInspArea_SheetWidth(bCommon, 0), GetInspArea_SheetWidth(bCommon, 1) });
        }
        private void button_OnSetCommonValue(object sender, EventArgs args)
        {
            if (_mainForm != null && _mainForm.IsControlReal(this))
            {
                if (DialogResult.Yes == Utility.ShowMessage(this, "検査幅etcを共通値に設定しますか？", MessageType.YesNo))
                {
                    for (int side = 0; side < _uclInspWidth.Length; side++)
                    {
                        //レシピで設定されているデータを　共通として表示する
                        _uclInspWidth[side].CommonSheetWidth = _uclInspWidth[side].SheetWidth;
                        _uclInspWidth[side].CommonMaskWidth = _uclInspWidth[side].MaskWidth;
                        _uclInspWidth[side].CommonMaskShift = _uclInspWidth[side].MaskShift;
                        SystemParam.GetInstance().InspArea_CmnSheetWidth[side] = _uclInspWidth[side].CommonSheetWidth;
                        SystemParam.GetInstance().InspArea_CmnMaskWidth[side] = _uclInspWidth[side].CommonMaskWidth;
                        SystemParam.GetInstance().InspArea_CmnMaskShift[side] = _uclInspWidth[side].CommonMaskShift;
                    }
                    bool bCommon = _uclInspWidth[0].CommonInspAreaEnable;
                    setMaskWidthAndInspWidth(
                        new double[] { GetInspArea_MaskWidth(bCommon, 0), GetInspArea_MaskWidth(bCommon, 1) },
                        new double[] { GetInspArea_MaskShift(bCommon, 0), GetInspArea_MaskShift(bCommon, 1) },
                        new double[] { GetInspArea_SheetWidth(bCommon, 0), GetInspArea_SheetWidth(bCommon, 1) });
                    refreshImage();

                    //v1324 ゾーン幅計算をメソッド化
                    this.ZoneRecalc();
                }
            }
        }

        /// <summary>
        /// 共通幅表示
        /// 　レシピの幅を表示させるか
        /// 　システムデータの幅を表示させるか
        /// </summary>
        /// <param name="bCommon"></param>
        /// <param name="side"></param>
        private void SetInspAreaData_Data2Cmn(bool bCommon, int side)
        {
            //if (bCommon == true)
            //{
            //    //レシピで設定されているデータを　共通として表示する
            //    _uclInspWidth[side].CommonSheetWidth = _uclInspWidth[side].SheetWidth;
            //    _uclInspWidth[side].CommonMaskWidth = _uclInspWidth[side].MaskWidth;
            //    _uclInspWidth[side].CommonMaskShift = _uclInspWidth[side].MaskShift;
            //}
            //else
            //{
            //    //システムで設定されているデータを　共通表示
            //    _uclInspWidth[side].CommonSheetWidth = SystemParam.GetInstance().InspArea_CmnSheetWidth[side];
            //    _uclInspWidth[side].CommonMaskWidth = SystemParam.GetInstance().InspArea_CmnMaskWidth[side];
            //    _uclInspWidth[side].CommonMaskShift = SystemParam.GetInstance().InspArea_CmnMaskShift[side];
            //}
        }
        private double GetInspArea_SheetWidth(bool bCommon, int side)
        {
            return (bCommon == true) ? _uclInspWidth[side].CommonSheetWidth : _uclInspWidth[side].SheetWidth;
        }
        private double GetInspArea_MaskWidth(bool bCommon, int side)
        {
            return (bCommon == true) ? _uclInspWidth[side].CommonMaskWidth : _uclInspWidth[side].MaskWidth;
        }
        private double GetInspArea_MaskShift(bool bCommon, int side)
        {
            return (bCommon == true) ? _uclInspWidth[side].CommonMaskShift : _uclInspWidth[side].MaskShift;
        }

        /// <summary>
        /// 分割数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spinPartition_ValueChanged(object sender, EventArgs e)
        {
            //v1324 ゾーン幅計算をメソッド化
            ZoneRecalc();
        }
        /// <summary>
        ///  照明：有効　チェックボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void uclLightControl_OnLightEnableChanged(object sender, EventArgs arg)
        {
            int selLightNo = (int)spinLightNo.Value - 1;
            if (selLightNo >= 0)
            {
                //変更があったか確認
                _clsCheckRecipeEdit.LightLevel[selLightNo] |= (uclLightControl.LightEnable != Recipe.GetInstance().LightParam[selLightNo].LightEnable);
                //値をレシピへ
                Recipe.GetInstance().LightParam[selLightNo].LightEnable = uclLightControl.LightEnable;
            }
        }
        /// <summary>
        /// 照明：輝度値
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void uclLightControl_OnLightValueChanged(object sender, EventArgs arg)
        {
            int selLightNo = (int)spinLightNo.Value - 1;
            if (selLightNo >= 0)
            {
                //変更があったか確認
                _clsCheckRecipeEdit.LightLevel[selLightNo] |= (uclLightControl.LightValue != Recipe.GetInstance().LightParam[selLightNo].LightValue);
                //値をレシピへ
                Recipe.GetInstance().LightParam[selLightNo].LightValue = uclLightControl.LightValue;

                if (chkLightOn.Checked)
                    _mainForm.LightOn();
            }
        }
        /// <summary>
        /// 照明：番号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spinLightNo_ValueChanged(object sender, EventArgs e)
        {
            int selLightNo = (int)spinLightNo.Value - 1;
            SetLightParts(selLightNo);
        }
        /// <summary>
        /// 照明：点灯　ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkLightOn_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLightOn.Checked)
                _mainForm.LightOn();
            else
                LightControlManager.getInstance().AllLightOff();
        }
        /// <summary>
        /// 裏：感度：Enabled
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvInspDownSide_EnabledChanged(object sender, EventArgs e)
        {
            EnabledColorChange(_dgvInsp[1], _dgvInsp[1].Enabled, SystemParam.GetInstance().markColorDownSide);
        }

        #endregion

        public void ChangeOldDataGridViewColor()
        {
            bool b = chkUpDownSideCmmon.Checked;
            EnabledColorChange(_dgvInsp[1], !b, SystemParam.GetInstance().markColorDownSide);
        }

        private void EnabledColorChange(DataGridView dgv, bool bEnabled, List<MarkColor> mc)
        {
            for (int y = 0; y < 4; y++)
            {
                for (int x = 1; x < 7; x++)
                {
                    dgv[x, y].Style.BackColor = ((bEnabled)) ? ColorTranslator.FromHtml(mc[x - 1].colorARGB) : Color.Gray;
                }
            }
        }

        private bool _setFlag = false;
        public void Recipe2Disp()
        {
            _setFlag = true;
            Recipe recipe = Recipe.GetInstance();

            //検査設定表裏共通
            chkUpDownSideCmmon.Checked = recipe.UpDownSideCommon;

            for (int i = 0; i < _uclInspWidth.Length; i++)
            {
                _uclInspWidth[i].CommonInspAreaEnable = recipe.CommonInspAreaEnable;

                //if (_uclInspWidth[i].CommonInspAreaEnable == false)
                //{
                _uclInspWidth[i].SheetWidth = recipe.InspParam[i].Width;
                _uclInspWidth[i].MaskWidth = recipe.InspParam[i].MaskWidth;
                _uclInspWidth[i].MaskShift = recipe.InspParam[i].MaskShift;
                //}
                //else
                //{
                //    _uclInspWidth[i].SheetWidth = SystemParam.GetInstance().InspArea_CmnSheetWidth[i];
                //    _uclInspWidth[i].MaskWidth = SystemParam.GetInstance().InspArea_CmnMaskWidth[i];
                //    _uclInspWidth[i].MaskShift = SystemParam.GetInstance().InspArea_CmnMaskShift[i];
                //}

                _uclInspWidth[i].CommonSheetWidth = SystemParam.GetInstance().InspArea_CmnSheetWidth[i];
                _uclInspWidth[i].CommonMaskWidth = SystemParam.GetInstance().InspArea_CmnMaskWidth[i];
                _uclInspWidth[i].CommonMaskShift = SystemParam.GetInstance().InspArea_CmnMaskShift[i];
            }

            InitLightControl();
            int selLightNo = (int)spinLightNo.Value - 1;
            SetLightParts(selLightNo);
            spinPartition.Value = (decimal)recipe.Partition;

            chkBothEndAny.Checked = recipe.IsBothEndAny;
            textKindName.Text = recipe.KindName;


            for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
            {
                for (int i = 1; i <= Enum.GetNames(typeof(AppData.InspID)).Length; i++)
                {
                    _dgvInsp[side][i, 0].Value = recipe.InspParam[side].Kando[i - 1].Threshold.ToString();
                    _dgvInsp[side][i, 1].Value = recipe.InspParam[side].Kando[i - 1].LengthV.ToString(SystemParam.GetInstance().NgDataDecimal);
                    _dgvInsp[side][i, 2].Value = recipe.InspParam[side].Kando[i - 1].LengthH.ToString(SystemParam.GetInstance().NgDataDecimal);
                    _dgvInsp[side][i, 3].Value = recipe.InspParam[side].Kando[i - 1].Area.ToString(SystemParam.GetInstance().NgDataDecimal);
                }
            }

            //表面検査有効        //20181202 moteki   V1053
            chkInspUpSide.Checked = recipe.UpSideInspEnable;
            chkInspUpSide.BackColor = (chkInspUpSide.Checked) ? Color.LightGreen : Color.Pink;
            //裏面検査有効        //20181202 moteki   V1053
            chkInspDownSide.Checked = recipe.DownsideInspEnable;
            chkInspDownSide.BackColor = (chkInspDownSide.Checked) ? Color.LightGreen : Color.Pink;

            //外部出力
            _externalEnable = recipe.ExternalEnable;
            _externalDelay1 = recipe.ExternalDelayTime1;
            _externalDelay2 = recipe.ExternalDelayTime2;
            _externalDelay3 = recipe.ExternalDelayTime3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            _externalDelay4 = recipe.ExternalDelayTime4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            _externalReset1 = recipe.ExternalResetTime1;
            _externalReset2 = recipe.ExternalResetTime2;
            _externalReset3 = recipe.ExternalResetTime3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            _externalReset4 = recipe.ExternalResetTime4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            _externalShot1 = recipe.ExternalShot1;
            _externalShot2 = recipe.ExternalShot2;
            _externalShot3 = recipe.ExternalShot3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            _externalShot4 = recipe.ExternalShot4; //V1057 NG表裏修正 yuasa 20190118：外部４追加

            //実速度
            chkUseCommonCamRealSpeed.Checked = recipe.UseCommonCamRealSpeed;
            _bUseCommonCamRealSpeed = recipe.UseCommonCamRealSpeed;
            _chkDispSideFlag = true;
            if (chkDispUpSide.Checked)
                spinCamRealSpeed.Value = (decimal)recipe.CamRealSpeedValue;
            else
                spinCamRealSpeed.Value = (decimal)recipe.CamRealSpeedValueUra;
            _chkDispSideFlag = false;
            _dCamRealSpeed = recipe.CamRealSpeedValue;
            _dCamRealSpeedUra = recipe.CamRealSpeedValueUra;

            //速度
            _camSpeedEnable = recipe.CamSpeedEnable;
            _camSpeedValue = recipe.CamSpeedValue;
            _camSpeedValueUra = recipe.CamSpeedValueUra;

            //露光
            _camExposureEnable = recipe.CamExposureEnable;
            _camExposureValue = recipe.CamExposureValue;
            _camExposureValueUra = recipe.CamExposureValueUra;

            //岐阜カスタム パトライト設定 v1326
            _patLiteEnable = recipe.PatLiteEnable;
            _patLiteDelay = recipe.PatLiteDelay;
            _patLiteOnTime = recipe.PatLiteOnTime;

            SetButtonColor();//v1326

            SetZone();
            for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
            {
                for (int i = 0; i < AppData.MAX_PARTITION; i++)
                {
                    _dgvZone[side][i, 0].Value = (decimal)recipe.InspParam[side].Zone[i];
                    //v1324 両端データ追加
                    if (i == 0)
                    {
                        OldValueLeftCell[side] = (int)recipe.InspParam[side].Zone[i];
                    }
                    else if (i == recipe.Partition - 1)
                    {
                        OldValueRightCell[side] = (int)recipe.InspParam[side].Zone[i];
                    }
                }
            }
            _setFlag = false;
        }

        /// <summary>ボタンの色をセットする</summary>
        private void SetButtonColor()//v1326
        {
            btnExternalOutput.BackColor = (_externalEnable == true) ? Color.GreenYellow : Color.LightYellow;
            btnCamSpeed.BackColor = (_camSpeedEnable == true || _camExposureEnable == true) ? Color.GreenYellow : Color.LightYellow;
            btnPatLite.BackColor = (_patLiteEnable == true) ? Color.GreenYellow : Color.LightYellow;
        }

        private void SetZone()
        {
            for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
            {
                for (int i = 0; i < AppData.MAX_PARTITION; i++)
                {
                    //背景
                    Color backColor = (i < spinPartition.Value) ? Color.White : Color.Gray;
                    _dgvZone[side][i, 0].Style.BackColor = backColor;
                    _dgvZone[side][i, 0].Style.SelectionBackColor = backColor;
                    _dgvZone[side][i, 0].Style.SelectionForeColor = Color.Black;
                }
                //SetZoneValue(side);
            }
        }

        private void SetLightParts(int selLightNo)
        {
            if (selLightNo >= 0)
            {
                LightControlManager ltCtrl = LightControlManager.getInstance();
                uclLightControl.LightName = ltCtrl.GetLight(selLightNo).Name;
                uclLightControl.LightValue = Recipe.GetInstance().LightParam[selLightNo].LightValue;
                uclLightControl.LightEnable = Recipe.GetInstance().LightParam[selLightNo].LightEnable;
            }
        }

        public bool Disp2Recipe()
        {
            Recipe recipe = Recipe.GetInstance();

            if (recipe.KindName == "" || recipe.KindName == "未登録")
            {
                Utility.ShowMessage(_mainForm, "品種名がありません。", MessageType.Error);
                return false;
            }
            recipe.IsBothEndAny = chkBothEndAny.Checked;
            recipe.Partition = (int)spinPartition.Value;

            recipe.CommonInspAreaEnable = _uclInspWidth[0].CommonInspAreaEnable;
            for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
            {
                //if (recipe.CommonInspAreaEnable == true)
                //{
                //    SystemParam.GetInstance().InspArea_CmnSheetWidth[side] = _uclInspWidth[side].SheetWidth;
                //    SystemParam.GetInstance().InspArea_CmnMaskWidth[side] = _uclInspWidth[side].MaskWidth;
                //    SystemParam.GetInstance().InspArea_CmnMaskShift[side] = _uclInspWidth[side].MaskShift;
                //}
                //検査幅
                recipe.InspParam[side].Width = _uclInspWidth[side].SheetWidth;
                recipe.InspParam[side].MaskWidth = _uclInspWidth[side].MaskWidth;
                recipe.InspParam[side].MaskShift = _uclInspWidth[side].MaskShift;
                //分割
                SetZoneValue(side);
                for (int i = 0; i < AppData.MAX_PARTITION; i++)//v1324 recipe.Partition→AppData.MAX_PARTITION
                {
                    recipe.InspParam[side].Zone[i] = Convert.ToInt32(_dgvZone[side][i, 0].Value);
                }
                //感度
                for (int i = 0; i < Enum.GetNames(typeof(AppData.InspID)).Length; i++)
                {
                    recipe.InspParam[side].Kando[i].Threshold = Convert.ToInt32(_dgvInsp[side][i + 1, 0].Value);
                    recipe.InspParam[side].Kando[i].LengthV = Convert.ToDouble(_dgvInsp[side][i + 1, 1].Value);
                    recipe.InspParam[side].Kando[i].LengthH = Convert.ToDouble(_dgvInsp[side][i + 1, 2].Value);
                    recipe.InspParam[side].Kando[i].Area = Convert.ToDouble(_dgvInsp[side][i + 1, 3].Value);
                }
            }

            //表面検査有効        //20181202 moteki   V1053
            recipe.UpSideInspEnable = chkInspUpSide.Checked;
            //裏面検査有効        //20181202 moteki   V1053
            recipe.DownsideInspEnable = chkInspDownSide.Checked;

            //外部出力
            recipe.ExternalEnable = _externalEnable;
            recipe.ExternalDelayTime1 = _externalDelay1;
            recipe.ExternalDelayTime2 = _externalDelay2;
            recipe.ExternalDelayTime3 = _externalDelay3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            recipe.ExternalDelayTime4 = _externalDelay4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            recipe.ExternalResetTime1 = _externalReset1;
            recipe.ExternalResetTime2 = _externalReset2;
            recipe.ExternalResetTime3 = _externalReset3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            recipe.ExternalResetTime4 = _externalReset4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            recipe.ExternalShot1 = _externalShot1;
            recipe.ExternalShot2 = _externalShot2;
            recipe.ExternalShot3 = _externalShot3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            recipe.ExternalShot4 = _externalShot4; //V1057 NG表裏修正 yuasa 20190118：外部４追加

            //実速度
            recipe.UseCommonCamRealSpeed = _bUseCommonCamRealSpeed;
            recipe.CamRealSpeedValue = _dCamRealSpeed;
            recipe.CamRealSpeedValueUra = _dCamRealSpeedUra;
            //速度
            recipe.CamSpeedEnable = _camSpeedEnable;
            recipe.CamSpeedValue = _camSpeedValue;
            recipe.CamSpeedValueUra = _camSpeedValueUra;

            //露光
            recipe.CamExposureEnable = _camExposureEnable;
            recipe.CamExposureValue = _camExposureValue;
            recipe.CamExposureValueUra = _camExposureValueUra;

            //岐阜カスタム パトライト設定 v1326
            recipe.PatLiteEnable = _patLiteEnable;
            recipe.PatLiteDelay = _patLiteDelay;
            recipe.PatLiteOnTime = _patLiteOnTime;

            //LightParamはリアルタイムでRecipeに更新しているため不良

            recipe.SelectItem = _mainForm.GetSelectItem();

            recipe.UpDownSideCommon = chkUpDownSideCmmon.Checked;

            if (System.IO.File.Exists(recipe.Path) == false)
            {
                int length;
                string path = "";

                length = recipe.Path.Length - AppData.RCP_FILE.Length;
                path = recipe.Path.Remove(length, AppData.RCP_FILE.Length);

                System.IO.Directory.CreateDirectory(path);
            }

            return true;
        }

        public void SaveRecipe()
        {
            //ディスプレイの数値をrecipeに入れる
            if (Disp2Recipe())
            {
                //保存
                Recipe recipe = Recipe.GetInstance();
                recipe.Save();

                if (SystemStatus.GetInstance().NowState == SystemStatus.State.Stop)
                {
                    clsFilebackup.Backup(SystemParam.GetInstance().RecipeFoldr, SystemParam.GetInstance().BackupFolder);
                }

                if (SystemStatus.GetInstance().SelectRecipe == false)
                {
                    return;
                }

                if (recipe.KindName != _mainForm.InspRecipe.KindName)
                {
                    return;
                }

                if (SystemStatus.GetInstance().NowState == SystemStatus.State.Stop)
                {
                    _mainForm.StandbyInsp();
                }
                else
                {
                    _mainForm.UpdateRecipe();
                }
            }
        }

        private void SetZoneValue(int side)
        {
            double dEvenly, dRemaind;
            int parCnt;
            double sheetWidth;

            if (chkBothEndAny.Checked == false || SystemParam.GetInstance().RecipeZoneSetteingEnable == false)
            {
                parCnt = (int)spinPartition.Value;
                if (_uclInspWidth[side].CommonInspAreaEnable == false)
                    sheetWidth = _uclInspWidth[side].SheetWidth;
                else
                    sheetWidth = _uclInspWidth[side].CommonSheetWidth;
                dEvenly = (int)(sheetWidth / parCnt);
                dRemaind = (double)(sheetWidth) - dEvenly * (parCnt - 1);
                for (int i = 0; i < AppData.MAX_PARTITION; i++)
                {
                    int X = i, Y = 0;

                    if (i < parCnt - 1)
                    {
                        _dgvZone[side][X, Y].Value = (decimal)dEvenly;
                    }
                    else if (i == parCnt - 1)
                    {
                        _dgvZone[side][X, Y].Value = (decimal)dRemaind;
                    }
                    else
                    {
                        _dgvZone[side][X, Y].Value = (decimal)0;
                    }
                }
            }
            else
            {
                //v1324 処理見直し
                Recipe recipe = Recipe.GetInstance();

                parCnt = (int)spinPartition.Value - 2;
                if (_uclInspWidth[side].CommonInspAreaEnable == false)
                    sheetWidth = _uclInspWidth[side].SheetWidth;
                else
                    sheetWidth = _uclInspWidth[side].CommonSheetWidth;
                //decimal both1 = (decimal)_dgvZone[side][0, 0].Value;
                int both1 = OldValueLeftCell[side];
                int x = (int)spinPartition.Value - 1;
                //decimal both2 = (decimal)_dgvZone[side][x, 0].Value;
                int both2 = OldValueRightCell[side];
                sheetWidth -= both1 + both2;
                dEvenly = (int)(sheetWidth / parCnt);
                dRemaind = (double)(sheetWidth) - dEvenly * (parCnt - 1);
                for (int i = 0; i < AppData.MAX_PARTITION; i++)
                {
                    //if (i == 0 || i == x)
                    //    continue;
                    int X = i, Y = 0;

                    if (i == 0)
                    {
                        _dgvZone[side][X, Y].Value = both1;
                    }
                    else if (i < (x - 1))
                    {
                        _dgvZone[side][X, Y].Value = (decimal)dEvenly;
                    }
                    else if (i == (x - 1))
                    {
                        _dgvZone[side][X, Y].Value = (decimal)dRemaind;
                    }
                    else if (i == x)
                    {
                        _dgvZone[side][X, Y].Value = both2;
                    }
                    else
                    {
                        _dgvZone[side][X, Y].Value = (decimal)0;
                    }
                }
            }
        }

        public void DispOldNameLot()
        {
            labelOldLotNo.Visible = SystemParam.GetInstance().LotNoEnable;
            textOldLotNumber.Visible = SystemParam.GetInstance().LotNoEnable;
        }

        public void SetKindName(string stKind)
        {
            textKindName.Text = stKind;
        }

        public void SetOldLotNo(string stLotNo)
        {
            textOldLotNumber.Text = stLotNo;
        }

        public void ChangeEnabled()
        {
            bool bb;
            if (ExtDisplayMode == EDisplayModeManualExt.Ext) //V1057 手動外部修正 yuasa 20190115：条件分岐追加
            {
                bb = false;
            }
            else
            {
                if (SystemStatus.GetInstance().DataDispMode == SystemStatus.ModeID.Old)
                    return;
                if (SystemStatus.GetInstance().NowState == SystemStatus.State.Stop)
                {
                    bb = true;
                }
                else
                {
                    if (_mainForm.InspRecipe != null)
                    {
                        if (_mainForm.InspRecipe.KindName == Recipe.GetInstance().KindName)
                        {
                            bb = false;
                        }
                        else
                        {
                            bb = true;
                        }
                    }
                    else
                    {
                        bb = true;
                    }
                }
            }

            //感度共通
            chkUpDownSideCmmon.Enabled = bb;
            //裏：感度
            _dgvInsp[1].Enabled = !chkUpDownSideCmmon.Checked;
            //検査幅
            for (int i = 0; i < _uclInspWidth.Length; i++)
                _uclInspWidth[i].Enabled = bb;
            //分割数
            spinPartition.Enabled = bb;
            //両端任意設定  v1324
            chkBothEndAny.Enabled = bb;
            //Zone
            for (int i = 0; i < _dgvZone.Length; i++)
                _dgvZone[i].Enabled = bb;
            //表面検査有効        //20181202 moteki   V1053   //自動検査中に変えていいならここはいらない
            chkInspUpSide.Enabled = bb;
            //裏面検査有効        //20181202 moteki   V1053   //自動検査中に変えていいならここはいらない
            chkInspDownSide.Enabled = bb;
            //照明
            grpLightControl.Enabled = bb;
            //速度・露光
            btnCamSpeed.Enabled = bb;
            //実速度
            chkUseCommonCamRealSpeed.Enabled = bb;
            spinCamRealSpeed.Enabled = bb;
        }

        private void ChangeInspData(int Col, int Row, String stTab)
        {
            InspKandoParam inspparam = new InspKandoParam();
            DataGridView dgv = new DataGridView();
            bool bCheckEdit = false;

            int side;
            side = (stTab == "UpSide") ? 0 : 1;
            inspparam = Recipe.GetInstance().InspParam[side].Kando[Col - 1];
            dgv = _dgvInsp[side];

            if (Row == 0)
                bCheckEdit = (inspparam.Threshold != Convert.ToInt32(dgv[Col, Row].Value));
            else if (Row == 1)
                bCheckEdit = (inspparam.LengthV != Convert.ToDouble(dgv[Col, Row].Value));
            else if (Row == 2)
                bCheckEdit = (inspparam.LengthH != Convert.ToDouble(dgv[Col, Row].Value));
            else if (Row == 3)
                bCheckEdit = (inspparam.Area != Convert.ToDouble(dgv[Col, Row].Value));

            bool[,] sideParam = (stTab == "UpSide") ? _clsCheckRecipeEdit.KandoUpSide : _clsCheckRecipeEdit.KandoDownSide;
            sideParam[Col - 1, Row] = bCheckEdit;
        }


        public clsBmpSaveThread _bmpSaveThread;

        #region 自動調光レベル検出
        clsAutoLightCalibration _autoLight = null;
        private void btnLightAjust_Click(object sender, EventArgs e)
        {
#if true
            // 再帰的に呼ばれることはない(すでにボタンが押されている等)
            if (_autoLight != null)
                return;

            // インスタンスが生成されていなければ抜ける
            if (_mainForm == null || _mainForm.AutoInspection == null)
                return;


            // 検査を行っている場合は設定不可
            if (SystemStatus.GetInstance().NowState != SystemStatus.State.Stop)
            {
                Utility.ShowMessage(_mainForm, "検査中は実行できません。", MessageType.Warning);
                return;
            }

            if (SystemStatus.GetInstance().SelectRecipe)
            {
                Utility.ShowMessage(_mainForm, "検査用品種が選択されている場合実行できません。", MessageType.Warning);
                return;
            }

#if false
            // オフラインモードで呼ばれることは無い
            if (false)
            {
                if (chkFull.Checked)
                {
                    Utility.ShowMessage(_mainForm, "フル調光完了(ｵﾌﾗｲﾝ)", MessageType.Information);

                    //初回調光完了
                    Recipe.GetInstance().AutoLight = true;

                    ////露光値を保存
                    //rec.SaveExposure();
                    ////完了を保存
                    //rec.SaveAutoLight();

                    updateExposureFull();
           
                }
                else
                {
                    Utility.ShowMessage(_mainForm, "自動調光完了(ｵﾌﾗｲﾝ)", MessageType.Information);
                }

                return;
            }
#endif
            if (SystemParam.GetInstance().CamCloseOpenAutoLightingEnable == true)
                _mainForm.CameraLightHosei(true);

            // マスクと検査幅をパラメーターに与える
            bool bCommon = _uclInspWidth[0].CommonInspAreaEnable;
            double[] dMaskWidth = new double[] { GetInspArea_MaskWidth(bCommon, 0), GetInspArea_MaskWidth(bCommon, 1) };
            double[] dMaskShift = new double[] { GetInspArea_MaskShift(bCommon, 0), GetInspArea_MaskShift(bCommon, 1) };
            double[] dWidth = new double[] { GetInspArea_SheetWidth(bCommon, 0), GetInspArea_SheetWidth(bCommon, 1) };
            _mainForm.AutoInspection.SetMinMaxAveCalcRange(dMaskWidth, dMaskShift, dWidth);

            LogingDllWrap.LogingDll.Loging_SetLogString("**自動調光開始:品種-" + Recipe.GetInstance().KindName);
            _autoLight = new clsAutoLightCalibration();
            _autoLight.Initialize(_mainForm.AutoInspection);
            _autoLight.AutoLightFull = false;

            clsAutoLightCalibration.ELightingType eLightType;
            if (SystemParam.GetInstance().DownSideEnable)
                eLightType = clsAutoLightCalibration.ELightingType.Reflec;//反射（表・裏）
            else
                eLightType = clsAutoLightCalibration.ELightingType.Trans;//透過（表だけ）
            List<int> lstResult;
            List<List<clsAutoLightCalibration.CamExposure>> llCamExp;
            List<double> lstCamGain;

            bool bResult;

            SystemParam sysParam = SystemParam.GetInstance();
            List<int> upSideUseLight = new List<int>();
            List<int> downSideUseLight = new List<int>();
            List<int> lightNo = new List<int>();
            for (int i = 0; i < sysParam.camParam.Count; i++)
            {
                if (sysParam.camParam[i].OnOff == true)
                {
                    if (sysParam.camParam[i].UseLightNo[0] == -1)
                        continue;
                    List<int> useLight = (sysParam.camParam[i].CamParts == AppData.SideID.表) ? upSideUseLight : downSideUseLight;
                    foreach (int ltNo in sysParam.camParam[i].UseLightNo)
                    {
                        if (Recipe.GetInstance().LightParam[ltNo].LightEnable)
                        {
                            useLight.Add(ltNo);
                            lightNo.Add(ltNo);
                        }
                    }
                }
            }

            bool ret = _autoLight.Start(eLightType, upSideUseLight, downSideUseLight, out bResult, out lstResult, out lstCamGain, out llCamExp);
            if (!ret)
            {
                LogingDllWrap.LogingDll.Loging_SetLogString("_autoLight.Start() return false" + Recipe.GetInstance().KindName);
            }

            SaveAutoLightValue(ret, bResult, lstResult);

            if (bResult)
            {
                foreach (int i in upSideUseLight)
                {
                    Recipe.GetInstance().LightParam[i].LightValue = lstResult[0];
                }
                foreach (int i in downSideUseLight)
                {
                    Recipe.GetInstance().LightParam[i].LightValue = lstResult[1];
                }
                int selLightNo = (int)spinLightNo.Value - 1;
                SetLightParts(selLightNo);

                if (lstCamGain.Count >= 1)
                    Recipe.GetInstance().CamGainOmote = lstCamGain[0];
                if (lstCamGain.Count >= 2)
                    Recipe.GetInstance().CamGainUra = lstCamGain[1];
                //変更があったか確認
                _clsCheckRecipeEdit.LightLevel[0] = true;
#if false
                setExposureDefault();

                LogingDllWrap.LogingDll.Loging_SetLogString("**自動調光終了:品種-" + Recipe.GetInstance().KindName);
                if (eLightType == clsAutoLightCalibration.ELightingType.Trans )
                {
                    spinLightLv5.Value = lstResult[0];

                    if ( chkFull.Checked )
                    {
                        int[] aiCams = clsAutoLightCalibration.GetCameras(clsAutoLightValueCalibration.ECameraSide.UpSide, Recipe.InspTypes.透過);
                        if ( aiCams != null && llCamExp.Count == 1 && llCamExp[0].Count == aiCams.Length )
                        {
                            for (int i = 0; i < aiCams.Length; i++)
                            {
                                Recipe.GetInstance().Exposure[aiCams[i], 0] = llCamExp[0][i].Val1;
                                Recipe.GetInstance().Exposure[aiCams[i], 1] = llCamExp[0][i].Val2;
                            }
                            Recipe.GetInstance().AutoLight = true;
                        }
                        else
                        {
                            MessageBox.Show("DEBUG:自動調光の配列がおかしい");
                        }
                    }
                }
                else if( eLightType == clsAutoLightCalibration.ELightingType.Reflec )
                {
                    if (lstResult.Count >= 1)
                    {
                        if (listLed[0].Item2)
                        {
                            spinLightLv1.Value = lstResult[0];
                        }
                        if (listLed[1].Item2)
                        {
                            spinLightLv2.Value = lstResult[0];
                        }
                    }
                    if( lstResult.Count >= 2 )
                    {
                        if (listLed[2].Item2)
                        {
                            spinLightLv3.Value = lstResult[1];
                        }
                        if (listLed[3].Item2)
                        {
                            spinLightLv4.Value = lstResult[1];
                        }
                    }

                    if (chkFull.Checked)
                    {
                        int[] aiCams = clsAutoLightCalibration.GetCameras(clsAutoLightValueCalibration.ECameraSide.UpSide, Recipe.InspTypes.反射);
                        if ( aiCams != null && llCamExp.Count >= 1 && llCamExp[0].Count == aiCams.Length)
                        {
                            for (int i = 0; i < aiCams.Length; i++)
                            {
                                Recipe.GetInstance().Exposure[aiCams[i], 0] = llCamExp[0][i].Val1;
                                Recipe.GetInstance().Exposure[aiCams[i], 1] = llCamExp[0][i].Val2;
                            }
                        }
                        else
                        {
                            MessageBox.Show("DEBUG:自動調光の配列がおかしい");
                        }

                        aiCams = clsAutoLightCalibration.GetCameras(clsAutoLightValueCalibration.ECameraSide.DownSide, Recipe.InspTypes.反射);
                        if ( aiCams != null && llCamExp.Count >= 2 && llCamExp[1].Count == aiCams.Length)
                        {
                            for (int i = 0; i < aiCams.Length; i++)
                            {
                                Recipe.GetInstance().Exposure[aiCams[i], 0] = llCamExp[1][i].Val1;
                                Recipe.GetInstance().Exposure[aiCams[i], 1] = llCamExp[1][i].Val2;
                            }
                        }
                        else
                        {
                            MessageBox.Show("DEBUG:自動調光の配列がおかしい");
                        }
                        Recipe.GetInstance().AutoLight = true;
                    }
                }
                else if (eLightType == clsAutoLightCalibration.ELightingType.Trans_Reflec)
                {
                    spinLightLv5.Value = lstResult[0];

                    if (listLed[2].Item2)
                    {
                        spinLightLv3.Value = lstResult[1];
                    }
                    if (listLed[3].Item2)
                    {
                        spinLightLv4.Value = lstResult[1];
                    }

                    if (chkFull.Checked)
                    {
                        int[] aiCams = clsAutoLightCalibration.GetCameras(clsAutoLightValueCalibration.ECameraSide.UpSide, Recipe.InspTypes.透過);
                        if ( aiCams != null && llCamExp.Count >= 1 && llCamExp[0].Count == aiCams.Length)
                        {
                            for (int i = 0; i < aiCams.Length; i++)
                            {
                                Recipe.GetInstance().Exposure[aiCams[i], 0] = llCamExp[0][i].Val1;
                                Recipe.GetInstance().Exposure[aiCams[i], 1] = llCamExp[0][i].Val2;
                            }
                        }
                        else
                        {
                            MessageBox.Show("DEBUG:自動調光の配列がおかしい");
                        }

                        aiCams = clsAutoLightCalibration.GetCameras(clsAutoLightValueCalibration.ECameraSide.DownSide, Recipe.InspTypes.反射);
                        if ( aiCams != null && llCamExp.Count >= 2 && llCamExp[1].Count == aiCams.Length)
                        {
                            for (int i = 0; i < aiCams.Length; i++)
                            {
                                Recipe.GetInstance().Exposure[aiCams[i], 0] = llCamExp[1][i].Val1;
                                Recipe.GetInstance().Exposure[aiCams[i], 1] = llCamExp[1][i].Val2;
                            }
                        }
                        else
                        {
                            MessageBox.Show("DEBUG:自動調光の配列がおかしい");
                        }
                        Recipe.GetInstance().AutoLight = true;
                    }
                }
#endif
            }
            else
            {
                LogingDllWrap.LogingDll.Loging_SetLogString("**自動調光ｷｬﾝｾﾙ:品種-" + Recipe.GetInstance().KindName);
                if (_mainForm.InspRecipe != null)
                {
                }
            }
            _autoLight.Terminate();
            _autoLight = null;

            if (chkLightOn.Checked)
                _mainForm.LightOn();
            else
                LightControlManager.getInstance().AllLightOff();
#endif
            if (SystemParam.GetInstance().CamCloseOpenAutoLightingEnable == true)
                _mainForm.CameraLightHosei(false);
        }
        #endregion

        public void Paste()
        {
            if (SystemStatus.GetInstance().NowState != SystemStatus.State.Stop)
            {
                if (_mainForm.InspRecipe.KindName == Recipe.GetInstance().KindName)
                {
                    Utility.ShowMessage(_mainForm, "検査中の品種には貼り付け出来ません。", MessageType.Error);
                    return;
                }
            }

            if (_mainForm.TempoRecipe == null)
            {
                Utility.ShowMessage(_mainForm, "コピーをしていません。", MessageType.Error);
                return;
            }

            string stMessage = "コピーした品種「" + _mainForm.TempoRecipe.KindName + "」を貼り付けますか?\r\nすでに品種のある場所では上書きされます。";
            if (DialogResult.Yes != Utility.ShowMessage(_mainForm, stMessage, MessageType.YesNo))
            {
                return;
            }

            string name = _mainForm.TempoRecipe.KindName;

            int i = _mainForm.CheckNeme(name);

            if (i == -1)
            {
                Utility.ShowMessage(_mainForm, "同一品種には貼り付け出来ません。", MessageType.Error);
                return;
            }

            frmTextEdit _frmTextEdit = new frmTextEdit(_mainForm, name);

            DialogResult _dilgres = _frmTextEdit.ShowDialog();

            if (_dilgres == DialogResult.Cancel)
            {
                return;
            }

            Recipe rec = Recipe.GetInstance();
            string stSelectItem = rec.SelectItem;

            rec = _mainForm.TempoRecipe.Copy();
            rec.SelectItem = _mainForm.GetSelectItem();
            string ss = SystemParam.GetInstance().RecipeFoldr + rec.SelectItem;
            if (!Directory.Exists(ss))
            {
                Directory.CreateDirectory(ss);
            }
            //名前の変更
            rec.KindName = _frmTextEdit.NewName;

            ss += "\\" + AppData.RCP_FILE;
            rec.Path = ss;
            rec.Save();
            if (!rec.Load(rec.SelectItem))
            {
                Utility.ShowMessage(this, "品種ロードエラー", MessageType.Error);
                return;
            }

            //キャンセル用のレシピコピー
            _mainForm.CancelRecipe = rec.Copy();

            _mainForm.ChangeListName(rec.SelectItem, rec.KindName);
            this.SetRecipeDisp();

        }

        public void Copy()
        {
            if ("" == _mainForm.SelectRecipeListItem())
            {
                Utility.ShowMessage(_mainForm, "品種データがありません。", MessageType.Error);
                return;
            }

            _mainForm.ChangeRecipeMessage();

            _mainForm.TempoRecipe = Recipe.GetInstance().Copy();
        }

        public void NotRegistaredEnable(bool enable)
        {
            //共通
            chkUpDownSideCmmon.Enabled = !enable;
            //感度
            for (int side = 0; side < _dgvInsp.Length; side++)
                _dgvInsp[side].Enabled = !enable;
            //検査幅
            for (int side = 0; side < _uclInspWidth.Length; side++)
                _uclInspWidth[side].Enabled = !enable;
            //分割数
            spinPartition.Enabled = !enable;
            //両端任意設定 v1324
            chkBothEndAny.Enabled = !enable;

            //Zone
            for (int side = 0; side < _dgvZone.Length; side++)
                _dgvZone[side].Enabled = !enable;
            //照明
            grpLightControl.Enabled = !enable;
            //実速度
            chkUseCommonCamRealSpeed.Enabled = !enable;
            spinCamRealSpeed.Enabled = !enable;

            //戻す
            btnUndo.Enabled = !enable;
            //保存
            btnSave.Enabled = !enable;
        }

        private void setMaskWidthAndInspWidth(double[] dMaskWidth, double[] dMaskShift, double[] dInsp)
        {
            if (_mainForm == null || _mainForm.AutoInspection == null)
                return;
            if (dInsp[0] != 0.0 && dInsp[1] != 0.0)
                _mainForm.AutoInspection.SetMinMaxAveCalcRange(dMaskWidth, dMaskShift, dInsp);
            else
                _mainForm.AutoInspection.ResetMinMaxAveCalcRange();
        }

        public void SetRecipeDisp()
        {
            this.Recipe2Disp();

            if (Recipe.GetInstance().KindName == "未登録")
            {
                this.NotRegistaredEnable(true);
            }
            else
            {
                this.NotRegistaredEnable(false);
                this.ChangeEnabled();
            }
            // マスク幅と検査幅をセットする
            if (Recipe.GetInstance().CommonInspAreaEnable == true)
            {
                SystemParam sp = SystemParam.GetInstance();
                setMaskWidthAndInspWidth(
                    new double[] { sp.InspArea_CmnMaskWidth[0], sp.InspArea_CmnMaskWidth[1] },
                    new double[] { sp.InspArea_CmnMaskShift[0], sp.InspArea_CmnMaskShift[1] },
                    new double[] { sp.InspArea_CmnSheetWidth[0], sp.InspArea_CmnSheetWidth[1] });
            }
            else
            {
                setMaskWidthAndInspWidth(
                    new double[] { Recipe.GetInstance().InspParam[0].MaskWidth, Recipe.GetInstance().InspParam[1].MaskWidth },
                    new double[] { Recipe.GetInstance().InspParam[0].MaskShift, Recipe.GetInstance().InspParam[1].MaskShift },
                    new double[] { Recipe.GetInstance().InspParam[0].Width, Recipe.GetInstance().InspParam[1].Width });
            }
        }

        public bool ProcessShortcutKey(Keys keyData)
        {
            if (shortcutKeyHelper1.PerformClickByKeys(keyData))
            {
                return true;
            }
            return false;


        }

        public clsCheckRecipeEdit _clsCheckRecipeEdit = new clsCheckRecipeEdit();

        public class clsCheckRecipeEdit
        {
            public bool bUpDownSideCommon = false;
            public bool InspWidth = false;
            public bool MaskWidth = false;
            public bool MaskShift = false;
            public bool CommonInspArea = false;
            public bool Partition = false;
            public bool IsBothEndAny = false;
            public bool[,] KandoUpSide = new bool[,] { { false, false, false, false }, { false, false, false, false }, { false, false, false, false }, { false, false, false, false }, { false, false, false, false }, { false, false, false, false } };
            public bool[,] KandoDownSide = new bool[,] { { false, false, false, false }, { false, false, false, false }, { false, false, false, false }, { false, false, false, false }, { false, false, false, false }, { false, false, false, false } };
            public bool[] bLedUse = new bool[] { false, false, false, false, false, false };
            public bool[] LightLevel = new bool[] { false, false, false, false, false, false };
            public bool UpSideInspEnable = false;      //20181202 moteki   V1053
            public bool DownSideInspEnable = false;     //20181202 moteki   V1053
            public bool ExternalOutput = false;
            public bool CamRealSpeed = false;
            public bool CamSpeed = false;
            public bool CamExposure = false;
            public bool PatLite = false;//v1326

            public void AllFalse()
            {
                bUpDownSideCommon = false;
                InspWidth = false;
                MaskWidth = false;
                MaskShift = false;
                CommonInspArea = false;
                Partition = false;
                IsBothEndAny = false;
                KandoUpSide = new bool[,] { { false, false, false, false }, { false, false, false, false }, { false, false, false, false }, { false, false, false, false }, { false, false, false, false }, { false, false, false, false } };
                KandoDownSide = new bool[,] { { false, false, false, false }, { false, false, false, false }, { false, false, false, false }, { false, false, false, false }, { false, false, false, false }, { false, false, false, false } };
                bLedUse = new bool[] { false, false, false, false, false, false };
                LightLevel = new bool[] { false, false, false, false, false, false };
                UpSideInspEnable = false;      //20181202 moteki   V1053
                DownSideInspEnable = false;     //20181202 moteki   V1053
                ExternalOutput = false;
                CamRealSpeed = false;
                CamSpeed = false;
                CamExposure = false;
                PatLite = false;//v1326
            }

            public void CheckEdit()
            {
                //各項目
                //if (MaskWidth == true || MaskShift == true || Partition == true || InspWidth == true || bUpDownSideCommon == true)
                if (MaskWidth == true || MaskShift == true || Partition == true || InspWidth == true || CommonInspArea == true || bUpDownSideCommon == true || UpSideInspEnable == true || DownSideInspEnable == true || IsBothEndAny == true)   //20181202 moteki   V1053
                {
                    SystemStatus.GetInstance().RecipeEdit = true;
                }
                //表：感度
                for (int i = 0; i < KandoUpSide.GetLength(0); i++)
                {
                    for (int j = 0; j < KandoUpSide.GetLength(1); j++)
                    {
                        if (KandoUpSide[i, j])
                        {
                            SystemStatus.GetInstance().RecipeEdit = true;
                            break;
                        }
                    }
                }
                //裏：感度
                for (int i = 0; i < KandoDownSide.GetLength(0); i++)
                {
                    for (int j = 0; j < KandoDownSide.GetLength(1); j++)
                    {
                        if (KandoDownSide[i, j])
                        {
                            SystemStatus.GetInstance().RecipeEdit = true;
                            break;
                        }
                    }
                }
                //照明有効
                for (int i = 0; i < bLedUse.Length; i++)
                {
                    if (bLedUse[i])
                    {
                        SystemStatus.GetInstance().RecipeEdit = true;
                        break;
                    }
                }
                //照明レベル値
                for (int i = 0; i < LightLevel.Length; i++)
                {
                    if (LightLevel[i])
                    {
                        SystemStatus.GetInstance().RecipeEdit = true;
                    }
                }
                //外部出力
                if (ExternalOutput == true)
                    SystemStatus.GetInstance().RecipeEdit = true;
                //実速度
                if (CamRealSpeed == true)
                    SystemStatus.GetInstance().RecipeEdit = true;
                //速度
                if (CamSpeed == true)
                    SystemStatus.GetInstance().RecipeEdit = true;
                //露光
                if (CamExposure == true)
                    SystemStatus.GetInstance().RecipeEdit = true;
                //岐阜カスタム パトライト設定 v1326
                if (PatLite == true)
                    SystemStatus.GetInstance().RecipeEdit = true;
            }
        }

        #region 画像描画
        HObject _hoDispImage = null;
        int _iPrevLeft = -1, _iPrevRight = -1;
        int _iPrevLeftOffset = -1, _iPrevRightOffset = -1;
        int _iCropHeightSize = 0;
        private void refreshImage(HObject[] hoImage = null)
        {
            if (_mainForm == null || _mainForm.AutoInspection == null)
                return;

            if (hoImage == null && _hoDispImage == null)
                return;

            HTuple htWidth, htHeight;
            bool bFirstTime = false;
            try
            {
                int side;
                side = _dispSelectSide;
                int iImageHeight = hwcImageWnd.Size.Height;
                double dMag = 1.0;
                if (hoImage != null)
                {
                    if (_hoDispImage != null)
                        _hoDispImage.Dispose();
                    else
                        bFirstTime = true;

                    HOperatorSet.GetImageSize(hoImage[side], out htWidth, out htHeight);
                    dMag = hwcImageWnd.Size.Width / htWidth.D;

                    foreach (double m in _magnifyList.Reverse())
                    {
                        if (dMag >= m)
                        {
                            dMag = m;
                            break;
                        }
                    }

                    _iCropHeightSize = (int)((hwcImageWnd.Size.Height * htWidth.D) / hwcImageWnd.Size.Width);
                    int startHeight, endHeight, underHeight;
                    if (SystemParam.GetInstance().GetImageHeightArea(htHeight.I, out startHeight, out endHeight, out underHeight) == false)
                    {
                        HOperatorSet.CropPart(hoImage[side], out _hoDispImage, 0, 0, htWidth, _iCropHeightSize);
                    }
                    else
                    {
                        HOperatorSet.CropPart(hoImage[side], out _hoDispImage, startHeight, 0, htWidth, _iCropHeightSize);
                    }

                    chkFocus.Text = KaTool.ImageTool.FocusValueStr(hoImage[side]);

                    HTuple htHorz, htVert;
                    ImgProjection(hoImage[side], out htHorz, out htVert);
                    drawProjection(htHorz, htVert, _hoDispImage);

                    drawLine();
                }
                HOperatorSet.GetImageSize(_hoDispImage, out htWidth, out htHeight);
                _hwndCtrl.addIconicVar2(_hoDispImage);
                if (bFirstTime)
                    _hwndCtrl.FittingImage(false);

                int iLeft, iRight;
                _mainForm.AutoInspection.GetMinMaxAveCalcRangePix((side == 0) ? AppData.SideID.表 : AppData.SideID.裏, out iLeft, out iRight);

                if (iLeft <= 0) iLeft = 1;
                if (iLeft > htWidth.I) iLeft = htWidth.I;
                if (iRight <= 0) iRight = 1;
                if (iRight > htWidth.I) iRight = htWidth.I;

                _iPrevLeft = iLeft;
                _iPrevRight = iRight;
                double resH = SystemParam.GetInstance().camParam[0].ResoH;
                if (side != 0)
                {
                    int index = SystemParam.GetInstance().camParam.FindIndex(x => x.CamParts == AppData.SideID.裏);
                    if (index > 0)
                        resH = SystemParam.GetInstance().camParam[index].ResoH;
                }

                int maskWidthPix = (int)((double)GetInspArea_MaskWidth(_uclInspWidth[0].CommonInspAreaEnable, _dispSelectSide) / resH);
                _iPrevLeftOffset = _iPrevLeft + maskWidthPix;
                _iPrevRightOffset = _iPrevRight - maskWidthPix;

                _hwndCtrl.repaint();
            }
            catch (Exception oe)
            {
                LogingDllWrap.LogingDll.Loging_SetLogString(oe.Message);
            }
            finally
            {
            }
        }

        public void RefreshImage(HObject[] hoImage, bool[] updown)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                    {
                        RefreshImage(hoImage, updown);
                    }
                ));
                return;
            }

            refreshImage(hoImage);
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
        void drawProjection(HTuple htHorz, HTuple htVert, HObject img)
        {
            double dMagnify = _hwndCtrl.Magnify;
            //Adjustment.clsHalconWindowControlLite.GetMagnify(img, hWC);
            double dRate = 1 / dMagnify;
            HObject hoXLD = null;


            if (htHorz.Length == 0 || htVert.Length == 0)
                return;

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

            double[] adVert = null, adHorz = null;
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

            try
            {
                HOperatorSet.GenContourPolygonXld(out hoXLD, new HTuple(adVert), new HTuple(adHorz));
                if (chkFocus.Checked == true)
                {
                    _hwndCtrl.GraphicManager.AddObject("XldObject", hoXLD,
                        SystemParam.GetInstance().LineKandoGrapthColor, "margin", 1);
                }
                else
                {
                    _hwndCtrl.GraphicManager.DeleteObject("XldObject");
                }

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
        private void drawLine()
        {
            Color c;
            c = ColorTranslator.FromHtml(SystemParam.GetInstance().LineStateMonitorColor);
            string mColName_stateLeft = string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
            c = ColorTranslator.FromHtml(SystemParam.GetInstance().LineStateMonitorColor);
            string mColName_stateRight = string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
            int iLineWidth_state = SystemParam.GetInstance().LineStateMonitorThick;

            c = ColorTranslator.FromHtml(SystemParam.GetInstance().LineInspWidthColor);
            string mColName_inspW = string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
            int iLineWidth_inspW = SystemParam.GetInstance().LineInspWidthThick;

            if (iLineWidth_state > 0)
            {
                _hwndCtrl.GraphicManager.AddLine("LeftLine", 0, _iPrevLeftOffset, _iCropHeightSize, _iPrevLeftOffset, mColName_stateLeft, iLineWidth_state);
                _hwndCtrl.GraphicManager.AddLine("RightLine", 0, _iPrevRightOffset, _iCropHeightSize, _iPrevRightOffset, mColName_stateRight, iLineWidth_state);
            }
            else
            {
                _hwndCtrl.GraphicManager.DeleteObject("LeftLine");
                _hwndCtrl.GraphicManager.DeleteObject("RightLine");
            }
            if (iLineWidth_inspW > 0)
            {
                _hwndCtrl.GraphicManager.AddLine("LeftMaskLine", 0, _iPrevLeft, _iCropHeightSize, _iPrevLeft, mColName_inspW, iLineWidth_inspW);
                _hwndCtrl.GraphicManager.AddLine("RightMaskLine", 0, _iPrevRight, _iCropHeightSize, _iPrevRight, mColName_inspW, iLineWidth_inspW);
            }
            else
            {
                _hwndCtrl.GraphicManager.DeleteObject("LeftMaskLine");
                _hwndCtrl.GraphicManager.DeleteObject("RightMaskLine");
            }
        }
        #endregion

        private void uclRecipeContents_VisibleChanged(object sender, EventArgs e)
        {
            if (_mainForm == null)
                return;

            if (_mainForm.IsControlReal((UserControl)sender))
            {
                if (Visible)
                {
                    if (_uclInspWidth[0].CommonInspAreaEnable == false)
                    {
                        _mainForm.AutoInspection.SetMinMaxAveCalcRange(
                            new double[] { _uclInspWidth[0].MaskWidth, _uclInspWidth[1].MaskWidth },
                            new double[] { _uclInspWidth[0].MaskShift, _uclInspWidth[1].MaskShift },
                            new double[] { _uclInspWidth[0].SheetWidth, _uclInspWidth[1].SheetWidth });
                    }
                    else
                    {
                        _mainForm.AutoInspection.SetMinMaxAveCalcRange(
                            new double[] { _uclInspWidth[0].CommonMaskWidth, _uclInspWidth[1].CommonMaskWidth },
                            new double[] { _uclInspWidth[0].CommonMaskShift, _uclInspWidth[1].CommonMaskShift },
                            new double[] { _uclInspWidth[0].CommonSheetWidth, _uclInspWidth[1].CommonSheetWidth });
                    }
                    if (Recipe.GetInstance().KindName == "未登録")
                    {
                        this.NotRegistaredEnable(true);
                    }
                    else
                    {
                        this.ChangeEnabled();
                    }
                    EnabledColorChange(_dgvInsp[0], true, SystemParam.GetInstance().markColorUpSide);
                    EnabledColorChange(_dgvInsp[1], !Recipe.GetInstance().UpDownSideCommon, SystemParam.GetInstance().markColorDownSide);
                    this.Recipe2Disp();
                }
                else
                {
                    //_mainForm.AutoInspection.ResetMinMaxAveCalcRange();
                }
            }
        }

        private DateTime _dtPrevDown = DateTime.Now;
        PointD _ptPrevDown = new PointD();
        private bool checkDoubleClick(PointD pt)
        {
            DateTime dtNowDown = DateTime.Now;
            if ((dtNowDown - _dtPrevDown).TotalMilliseconds > SystemInformation.DoubleClickTime
                || Math.Abs(pt.X - _ptPrevDown.X) > (double)SystemInformation.DoubleClickSize.Width
                || Math.Abs(pt.Y - _ptPrevDown.Y) > (double)SystemInformation.DoubleClickSize.Height)
            {
                _dtPrevDown = dtNowDown;
                _ptPrevDown.X = pt.X;
                _ptPrevDown.Y = pt.Y;
                return false;
            }
            _dtPrevDown = dtNowDown.Subtract(TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime));
            return true;
        }

        private void hwcImageWnd_HMouseDown(object sender, HMouseEventArgs e)
        {
            if (checkDoubleClick(new PointD(e.X, e.Y)))
            {
                _hwndCtrl.FittingImage(false);
            }
        }

        //点灯させるチェックボックスがtrueならfalseにする
        public void CheckLedOnOff()
        {
            chkLightOn.Checked = false;
        }

        private bool _externalEnable;
        private int _externalDelay1;
        private int _externalDelay2;
        private int _externalDelay3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
        private int _externalDelay4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
        private int _externalReset1;
        private int _externalReset2;
        private int _externalReset3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
        private int _externalReset4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
        private int _externalShot1;
        private int _externalShot2;
        private int _externalShot3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
        private int _externalShot4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
        private void btnExternalOutput_Click(object sender, EventArgs e)
        {
            using (frmRecipeExternalOutput frm = new frmRecipeExternalOutput())
            {
                frm.ExternalEnable = _externalEnable;
                frm.ExternalDelay1 = _externalDelay1;
                frm.ExternalDelay2 = _externalDelay2;
                frm.ExternalDelay3 = _externalDelay3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
                frm.ExternalDelay4 = _externalDelay4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
                frm.ExternalReset1 = _externalReset1;
                frm.ExternalReset2 = _externalReset2;
                frm.ExternalReset3 = _externalReset3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
                frm.ExternalReset4 = _externalReset4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
                frm.ExternalShot1 = _externalShot1;
                frm.ExternalShot2 = _externalShot2;
                frm.ExternalShot3 = _externalShot3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
                frm.ExternalShot4 = _externalShot4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _externalEnable = frm.ExternalEnable;
                    _externalDelay1 = frm.ExternalDelay1;
                    _externalDelay2 = frm.ExternalDelay2;
                    _externalDelay3 = frm.ExternalDelay3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
                    _externalDelay4 = frm.ExternalDelay4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
                    _externalReset1 = frm.ExternalReset1;
                    _externalReset2 = frm.ExternalReset2;
                    _externalReset3 = frm.ExternalReset3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
                    _externalReset4 = frm.ExternalReset4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
                    _externalShot1 = frm.ExternalShot1;
                    _externalShot2 = frm.ExternalShot2;
                    _externalShot3 = frm.ExternalShot3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
                    _externalShot4 = frm.ExternalShot4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
                    if (_externalEnable != Recipe.GetInstance().ExternalEnable ||
                        _externalDelay1 != Recipe.GetInstance().ExternalDelayTime1 ||
                        _externalDelay2 != Recipe.GetInstance().ExternalDelayTime2 ||
                        _externalDelay3 != Recipe.GetInstance().ExternalDelayTime3 || //V1057 NG表裏修正 yuasa 20190118：外部３追加
                        _externalDelay4 != Recipe.GetInstance().ExternalDelayTime4 || //V1057 NG表裏修正 yuasa 20190118：外部４追加
                        _externalReset1 != Recipe.GetInstance().ExternalResetTime1 ||
                        _externalReset2 != Recipe.GetInstance().ExternalResetTime2 ||
                        _externalReset3 != Recipe.GetInstance().ExternalResetTime3 || //V1057 NG表裏修正 yuasa 20190118：外部３追加
                        _externalReset4 != Recipe.GetInstance().ExternalResetTime4 || //V1057 NG表裏修正 yuasa 20190118：外部４追加
                        _externalShot1 != Recipe.GetInstance().ExternalShot1 ||
                        _externalShot2 != Recipe.GetInstance().ExternalShot2 ||
                        _externalShot3 != Recipe.GetInstance().ExternalShot3 || //V1057 NG表裏修正 yuasa 20190118：外部３追加
                        _externalShot4 != Recipe.GetInstance().ExternalShot4) //V1057 NG表裏修正 yuasa 20190118：外部４追加
                        _clsCheckRecipeEdit.ExternalOutput = true;
                    else
                        _clsCheckRecipeEdit.ExternalOutput = false;
                }
            }
            //v1326 追記
            btnExternalOutput.BackColor = (_externalEnable == true) ? Color.GreenYellow : Color.LightYellow;
        }

        private bool _camSpeedEnable;
        private double _camSpeedValue;
        private double _camSpeedValueUra;
        private bool _camExposureEnable;
        private double _camExposureValue;
        private double _camExposureValueUra;
        private void btnCamSpeed_Click(object sender, EventArgs e)
        {
            using (frmRecipeSpeed frm = new frmRecipeSpeed())
            {
                frm.CamSpeedEnable = _camSpeedEnable;
                frm.CamSpeedValue = _camSpeedValue;
                frm.CamSpeedValueUra = _camSpeedValueUra;
                frm.CamExposureEnable = _camExposureEnable;
                frm.CamExposureValue = _camExposureValue;
                frm.CamExposureValueUra = _camExposureValueUra;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    _camSpeedEnable = frm.CamSpeedEnable;
                    _camSpeedValue = frm.CamSpeedValue;
                    _camSpeedValueUra = frm.CamSpeedValueUra;
                    _camExposureEnable = frm.CamExposureEnable;
                    _camExposureValue = frm.CamExposureValue;
                    _camExposureValueUra = frm.CamExposureValueUra;
                    if (_camSpeedEnable != Recipe.GetInstance().CamSpeedEnable ||
                        _camSpeedValue != Recipe.GetInstance().CamSpeedValue ||
                        _camSpeedValueUra != Recipe.GetInstance().CamSpeedValueUra)
                        _clsCheckRecipeEdit.CamSpeed = true;
                    else
                        _clsCheckRecipeEdit.CamSpeed = false;

                    if (_camExposureEnable != Recipe.GetInstance().CamExposureEnable ||
                        _camExposureValue != Recipe.GetInstance().CamExposureValue ||
                        _camExposureValueUra != Recipe.GetInstance().CamExposureValueUra)
                        _clsCheckRecipeEdit.CamExposure = true;
                    else
                        _clsCheckRecipeEdit.CamExposure = false;
                }
            }
            //v1326 追記
            btnCamSpeed.BackColor = (_camSpeedEnable == true || _camExposureEnable == true) ? Color.GreenYellow : Color.LightYellow;
        }

        private void chkInspUpSide_CheckedChanged(object sender, EventArgs e)   //20181202 moteki   V1053
        {
            _clsCheckRecipeEdit.UpSideInspEnable = (chkInspUpSide.Checked != Recipe.GetInstance().UpSideInspEnable);
            if (chkInspUpSide.Checked)
                chkInspUpSide.Text = "表面検査有効";
            else
                chkInspUpSide.Text = "表面検査無効";
            chkInspUpSide.BackColor = (chkInspUpSide.Checked) ? Color.LightGreen : Color.Pink;
        }

        private void chkInspDownSide_CheckedChanged(object sender, EventArgs e) //20181202 moteki   V1053
        {
            _clsCheckRecipeEdit.UpSideInspEnable = (chkInspDownSide.Checked != Recipe.GetInstance().DownsideInspEnable);
            if (chkInspDownSide.Checked)
                chkInspDownSide.Text = "裏面検査有効";//20181218 yuasa文言修正 V1056
            else
                chkInspDownSide.Text = "裏面検査無効";//20181218 yuasa文言修正 V1056
            chkInspDownSide.BackColor = (chkInspDownSide.Checked) ? Color.LightGreen : Color.Pink;
        }

        private int _dispSelectSide = 0;
        private bool _chkDispSideFlag = false;

        private EDisplayModeManualExt ExtDisplayMode = EDisplayModeManualExt.Manual;//V1057 手動外部修正 yuasa 20190115：手動外部での表示用に追加

        private bool _bUseCommonCamRealSpeed;
        private double _dCamRealSpeed;
        private double _dCamRealSpeedUra;
        private void spinRealSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (_chkDispSideFlag == true)
                return;

            _clsCheckRecipeEdit.CamRealSpeed = true;
            if (chkDispUpSide.Checked == true)
                _dCamRealSpeed = (double)spinCamRealSpeed.Value;
            else
                _dCamRealSpeedUra = (double)spinCamRealSpeed.Value;
        }

        public void ExtDisplayModeChange(bool Ext) //V1057 手動外部修正 yuasa 20190115：手動外部での表示用に追加
        {
            if (Ext)
            {
                ExtDisplayMode = EDisplayModeManualExt.Ext;
            }
            else
            {
                ExtDisplayMode = EDisplayModeManualExt.Manual;
            }
            this.ChangeEnabled();
        }

        private void chkUseCommonCamRealSpeed_CheckedChanged(object sender, EventArgs e)
        {
            _clsCheckRecipeEdit.CamRealSpeed = true;
            _bUseCommonCamRealSpeed = chkUseCommonCamRealSpeed.Checked;
        }

        private void chkBothEndAny_CheckedChanged(object sender, EventArgs e)
        {
            //v1324 処理見直し
            if (_setFlag == true)
                return;
            //■■■ここから■■■チェックボックスON/OFF時の動きがおかしい。
            Recipe recipe = Recipe.GetInstance();

            if (true == chkBothEndAny.Checked && SystemParam.GetInstance().RecipeZoneSetteingEnable == true)
            {
                //チェックオンにした時は、表示されている両端のデータをそのまま格納
                for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
                {
                    //OldValueLeftCell[side] = (int)recipe.InspParam[side].Zone[0];
                    //OldValueRightCell[side] = (int)recipe.InspParam[side].Zone[recipe.Partition - 1];
                    OldValueLeftCell[side] = Convert.ToInt32(_dgvZone[side][0, 0].Value);
                    OldValueRightCell[side] = Convert.ToInt32(_dgvZone[side][(int)spinPartition.Value - 1, 0].Value);
                }
            }
            else
            {
                //チェックオフした時は、全データ再計算
                for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
                    SetZoneValue(side);
            }
            //else
            //{
            //    for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
            //    {
            //        OldValueLeftCell[side] = 
            //        OldValueRightCell[side] = (int)recipe.InspParam[side].Zone[recipe.Partition - 1];
            //    }
            //}


            //SetZone();
            //for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
            //    SetZoneValue(side);

            _clsCheckRecipeEdit.IsBothEndAny = (chkBothEndAny.Checked != Recipe.GetInstance().IsBothEndAny);

        }

        /// <summary>■■■変数追加</summary>//v1324
        int[] OldValueLeftCell = new int[] { 0, 0 };
        int[] OldValueRightCell = new int[] { 0, 0 };

        private void dgvZone_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (chkBothEndAny.Checked == false || SystemParam.GetInstance().RecipeZoneSetteingEnable == false)
                return;

            int rowIndex = e.RowIndex;
            int colIndex = e.ColumnIndex;
            if (colIndex < 0 || rowIndex < 0)
                return;

            DataGridView dgv = (DataGridView)sender;
            DataGridViewCell dg = dgv[colIndex, rowIndex];

            int partition = (int)spinPartition.Value;

            if (colIndex != 0 && colIndex != (partition - 1))
                return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                frmTenkey tenkey = new frmTenkey();
                decimal min, max;
                int dec;
                min = 1; max = 1000; dec = 0;
                tenkey.SetValues(min, max, (decimal)Convert.ToDouble(dg.Value), dec);
                if (DialogResult.OK == tenkey.ShowDialog())
                {
                    //v1324 追加
                    if (colIndex == 0)
                    {
                        //OldValueLeftCell[chkDispUpSide.Checked ? 0 : 1] = (int)tenkey.Value;
                        //V1333 マップやNGポップアップの統一化が図れない為、表裏を同値とするように変更
                        OldValueLeftCell[0] = (int)tenkey.Value;
                        OldValueLeftCell[1] = (int)tenkey.Value;
                    }
                    else if (colIndex == (partition - 1))
                    {
                        //OldValueRightCell[chkDispUpSide.Checked ? 0 : 1] = (int)tenkey.Value;
                        //V1333 マップやNGポップアップの統一化が図れない為、表裏を同値とするように変更
                        OldValueRightCell[0] = (int)tenkey.Value;
                        OldValueRightCell[1] = (int)tenkey.Value;
                    }

                    dg.Value = tenkey.Value;

                    //V1333 マップやNGポップアップの統一化が図れない為、表裏を同値とするように変更
                    if (chkDispUpSide.Checked == true)
                    {
                        //表が変更された
                        CopyZone(true);
                    }
                    else
                    {
                        //裏が変更された
                        CopyZone(false);
                    }

                    //SetZoneValue(_dispSelectSide);
                    //表も裏も更新
                    SetZoneValue(0);
                    SetZoneValue(1);

                    _clsCheckRecipeEdit.Partition = true;
                }
            }
        }

        private void CopyZone(bool isTopChange)//V1333 追加
        {
            int copyMoto;
            int copySaki;
            if (isTopChange == true)
            {
                copyMoto = 0;
                copySaki = 1;
            }else
            {
                copyMoto = 1;
                copySaki = 0;
            }

            for (int i = 0; i < AppData.MAX_PARTITION; i++)
            {
                //if (i == 0 || i == x)
                //    continue;
                int X = i, Y = 0;
                _dgvZone[copySaki][X, Y].Value = _dgvZone[copyMoto][X, Y].Value;
            }
        }


        public enum EDisplayModeManualExt //V1057 手動外部修正 yuasa 20190115：手動外部での表示用に追加
        {
            Manual,
            Ext,
        }

        private bool _patLiteEnable;//v1326
        private int _patLiteDelay;//v1326
        private int _patLiteOnTime;//v1326


        private void btnPatLite_Click(object sender, EventArgs e)//v1326
        {
            using(frmPatLite frmPL = new frmPatLite())
            {
                frmPL.PatLiteEnable = _patLiteEnable;
                frmPL.PatLiteDelay = _patLiteDelay;
                frmPL.PatLiteOnTime = _patLiteOnTime;

                if (frmPL.ShowDialog() == DialogResult.OK)
                {
                    _patLiteEnable = frmPL.PatLiteEnable;
                    _patLiteDelay = frmPL.PatLiteDelay;
                    _patLiteOnTime = frmPL.PatLiteOnTime;
                }

                //データ変更有無
                if (_patLiteEnable!=Recipe.GetInstance().PatLiteEnable|| _patLiteDelay != Recipe.GetInstance().PatLiteDelay || _patLiteOnTime != Recipe.GetInstance().PatLiteOnTime)
                    _clsCheckRecipeEdit.PatLite = true;
                else
                    _clsCheckRecipeEdit.PatLite = false;
            }
            btnPatLite.BackColor = (_patLiteEnable == true) ? Color.GreenYellow : Color.LightYellow;
        }

        private void SaveAutoLightValue(bool bRet, bool bResult, List<int> lstLightValue)
        {
            int iMaxLine = 10000;

            string sDir = Path.GetDirectoryName(Recipe.GetInstance().Path);
            string sPath = Path.Combine(sDir, "AutoLightValue.txt");

            List<string> sLineDatas = new List<string>();

            string sNewData = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            sNewData += "," + bRet.ToString();
            sNewData += "," + bResult.ToString();
            if (lstLightValue != null)
            {
                foreach (int val in lstLightValue)
                {
                    sNewData += "," + val.ToString("000");
                }
            }
            sLineDatas.Add(sNewData);

            if (File.Exists(sPath))
            {
                using (StreamReader sr = new StreamReader(sPath, Encoding.ASCII))
                {
                    while (sr.Peek() >= 0)
                        sLineDatas.Add(sr.ReadLine());
                }
            }

            using (StreamWriter sw = new StreamWriter(sPath, false, Encoding.ASCII))
            {
                foreach (string s in sLineDatas)
                {
                    sw.WriteLine(s);
                    iMaxLine--;
                    if (iMaxLine <= 0)
                        break;
                }
            }
        }
        /// <summary>ゾーン再計算</summary>
        private void ZoneRecalc()
        {
            if (_setFlag == true)
                return;
            SetZone();
            for (int side = 0; side < Enum.GetNames(typeof(AppData.SideID)).Length; side++)
                SetZoneValue(side);

            _clsCheckRecipeEdit.Partition = (spinPartition.Value != Recipe.GetInstance().Partition);
        }


    }
}
