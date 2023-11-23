using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fujita.InspectionSystem;
using Fujita.Misc;
using Fujita.LightControl;
using HalconCamera;

namespace LineCameraSheetSystem
{
    public partial class SystemSettingForm : Form
    {
        /// <summary>
        /// メインフォーム
        /// </summary>
        MainForm _mainFrom { get; set; }

        /// <summary>
        /// 欠点NG色グリッド
        /// </summary>
        DataGridView[] _dgvNgColor;

        /// <summary>
        /// 照明警告：照明情報
        /// </summary>
        List<uclLightTimeReset> _lightTimeReset = new List<uclLightTimeReset>();

        Label[] _lblLineColors;
        uclNumericInputSmall[] _spinKandoThick;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mf"></param>
        public SystemSettingForm(MainForm mf)
        {
            InitializeComponent();

            _mainFrom = mf;

            _dgvNgColor = new DataGridView[] { dgvUpSideNgColor, dgvDownSideNgColor };

            //欠点種カラー指定グリッド
            for (int i = 0; i < _dgvNgColor.Length; i++)
            {
                _dgvNgColor[i].Rows.Add(2);
                _dgvNgColor[i][0, 0].Value = "明";
                _dgvNgColor[i][0, 1].Value = "暗";
                _dgvNgColor[i][0, 0].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
                _dgvNgColor[i][0, 1].Style.BackColor = Color.FromArgb(255, 212, 208, 200);
            }

            _spinKandoThick = new uclNumericInputSmall[] { spinLineKandoThick1, spinLineKandoThick2, spinLineKandoThick3, spinLineKandoThick4, spinLineKandoThick5, spinLineKandoThick6 };
            for (int i = 0; i < _spinKandoThick.Length; i++)
                _spinKandoThick[i].Tag = (int)i;

            _lblLineColors = new Label[] { lblLineColor1, lblLineColor2, lblLineColor3, lblLineColor4, lblLineColor5 };
            for (int i = 0; i < _lblLineColors.Length; i++)
                _lblLineColors[i].Tag = (int)i;

            //LED点灯時間の表示
            InitLightTime();

            //カメラ設定値Text
            CreateArray();

            //画面に表示
            SystemDisp();

            if (SystemStatus.GetInstance().NowState != SystemStatus.State.Stop)
            {
                //検査停止していなかったらfalse
                //EnabledDisp();
            }

            //ゾーン設定表示設定
            uclExtarnalOutput1.SetZoneSettingEnable(SystemParam.GetInstance().SystemSettingFormZoneDisp);//V1333「ZoneSettingEnable」から名称変更
            uclExtarnalOutput2.SetZoneSettingEnable(SystemParam.GetInstance().SystemSettingFormZoneDisp);
            uclExtarnalOutput3.SetZoneSettingEnable(SystemParam.GetInstance().SystemSettingFormZoneDisp);
            uclExtarnalOutput4.SetZoneSettingEnable(SystemParam.GetInstance().SystemSettingFormZoneDisp);

            //１Ｆパトライトタブ v1326
            if(false == SystemParam.GetInstance().GCustomEnable)
            {
                //岐阜カスタムでない場合はタブ削除
                this.tabControl1.TabPages.Remove(this.tabPatLite);
            }

            ChangeDebugMode();
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemSettingForm_Load(object sender, EventArgs e)
        {
        }

        private void SystemSettingForm_Shown(object sender, EventArgs e)
        {
        }

        private void InitLightTime()
        {
            LightControlManager ltCtrl = LightControlManager.getInstance();
            int yPt = 77;
            int height = 45;
            for (int i = 0; i < ltCtrl.LightCount; i++)
            {
                uclLightTimeReset ucl = new uclLightTimeReset();

                ucl.Location = new System.Drawing.Point(11, yPt);
                ucl.Tag = i;
                ucl.Name = "uclLightTimeReset" + i;
                ucl.Size = new System.Drawing.Size(425, height - 5);
                ucl.OnLightTimeReset += this.btnLightTimeReset_Click;

                _lightTimeReset.Add(ucl);
                this.grpLedTime.Controls.Add(ucl);

                yPt += height;
            }
        }


        //テキストボックスの配列
        TextBox[,] textboxArray;

        //テキストボックスとチェックボックスを配列にする
        private void CreateArray()
        {
            //テキストボックス配列の初期化
            textboxArray = new TextBox[,]
            {
                { textPicV1, textPicH1, textResoV1, textResoH1, textShiftX1, textShiftY1 },
                { textPicV2, textPicH2, textResoV2, textResoH2, textShiftX2, textShiftY2 },
                { textPicV3, textPicH3, textResoV3, textResoH3, textShiftX3, textShiftY3 },
                { textPicV4, textPicH4, textResoV4, textResoH4, textShiftX4, textShiftY4 }
            };
        }

        /// <summary>
        /// 変更　ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChange_Click(object sender, EventArgs e)
        {
            //画面の数値をコピー
            this.Save();
        }

        /// <summary>
        /// キャンセル　ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.CheckRecipeUpdate())
            {
                if (DialogResult.Yes == Utility.ShowMessage(this, "変更を保存しますか？", MessageType.YesNo))
                {
                    this.Save();
                }
            }
            this.Close();
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            this.Cursor = Cursors.WaitCursor;

            SystemParam systemparam = SystemParam.GetInstance();

            systemparam.LightWarningTime = (int)spinTimeDirect.Value;

            systemparam.DeleteMonth = (int)spinDeleteMonth.Value;

            systemparam.AutoShutdownEnable = chkAutoShutdown.Checked;
            systemparam.AutoShutdownWaitSec = (int)spinShutdownTime.Value;

            for (int i = 0; i < AppData.CAM_COUNT; i++)
            {
                systemparam.camParam[i].PixV = Convert.ToInt32(textboxArray[i, 0].Text);
                systemparam.camParam[i].PixH = Convert.ToInt32(textboxArray[i, 1].Text);
                systemparam.camParam[i].ResoV = Convert.ToDouble(textboxArray[i, 2].Text);
                systemparam.camParam[i].ResoH = Convert.ToDouble(textboxArray[i, 3].Text);
                systemparam.camParam[i].ShiftV = Convert.ToDouble(textboxArray[i, 4].Text);
                systemparam.camParam[i].ShiftH = Convert.ToDouble(textboxArray[i, 5].Text);

                _mainFrom.AutoInspection.CamInfos[i].ResolutionX = systemparam.camParam[i].ResoH;
                _mainFrom.AutoInspection.CamInfos[i].ResolutionY = systemparam.camParam[i].ResoV;
                _mainFrom.AutoInspection.CamInfos[i].OffsetX = systemparam.camParam[i].ShiftH;
                _mainFrom.AutoInspection.CamInfos[i].OffsetY = systemparam.camParam[i].ShiftV;

            }
            //datadridviewのbackcolorの保存
            for (int i = 0; i < _dgvNgColor.Length; i++)
            {
                List<MarkColor> mc = (i == 0) ? systemparam.markColorUpSide : systemparam.markColorDownSide;
                for (int j = 0; j < Enum.GetNames(typeof(AppData.InspID)).Length; j++)
                {
                    mc[j].colorARGB = (_dgvNgColor[i][j - (j / 3 * 3) + 1, (j / 3)].Value).ToString();
                }
            }
            systemparam.ExternalEnable1 = uclExtarnalOutput1.ExtTimer;
            systemparam.ExternalEnable2 = uclExtarnalOutput2.ExtTimer;
            systemparam.ExternalEnable3 = uclExtarnalOutput3.ExtTimer; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            systemparam.ExternalEnable4 = uclExtarnalOutput4.ExtTimer; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            LogingDllWrap.LogingDll.Loging_SetLogString("ExternalEnable1 <= " + systemparam.ExternalEnable1.ToString());
            LogingDllWrap.LogingDll.Loging_SetLogString("ExternalEnable2 <= " + systemparam.ExternalEnable2.ToString());
            LogingDllWrap.LogingDll.Loging_SetLogString("ExternalEnable3 <= " + systemparam.ExternalEnable3.ToString());
            LogingDllWrap.LogingDll.Loging_SetLogString("ExternalEnable4 <= " + systemparam.ExternalEnable4.ToString());

            systemparam.ExternalResetTime1 = (int)uclExtarnalOutput1.spinExtTimer.Value;
            systemparam.ExternalResetTime2 = (int)uclExtarnalOutput2.spinExtTimer.Value;
            systemparam.ExternalResetTime3 = (int)uclExtarnalOutput3.spinExtTimer.Value; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            systemparam.ExternalResetTime4 = (int)uclExtarnalOutput4.spinExtTimer.Value; //V1057 NG表裏修正 yuasa 20190118：外部４追加

            systemparam.ExternalDelayTime1 = (int)uclExtarnalOutput1.spinExtDelay.Value;
            systemparam.ExternalDelayTime2 = (int)uclExtarnalOutput2.spinExtDelay.Value;
            systemparam.ExternalDelayTime3 = (int)uclExtarnalOutput3.spinExtDelay.Value; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            systemparam.ExternalDelayTime4 = (int)uclExtarnalOutput4.spinExtDelay.Value; //V1057 NG表裏修正 yuasa 20190118：外部４追加

            systemparam.ExternalShot1 = (int)uclExtarnalOutput1.spinExtShot.Value;
            systemparam.ExternalShot2 = (int)uclExtarnalOutput2.spinExtShot.Value;
            systemparam.ExternalShot3 = (int)uclExtarnalOutput3.spinExtShot.Value; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            systemparam.ExternalShot4 = (int)uclExtarnalOutput4.spinExtShot.Value; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            for (int i = 0; i < systemparam.Extarnal1Zone.Count; i++)
            {
                systemparam.Extarnal1Zone[i] = uclExtarnalOutput1.GetCheckZone(i);
                systemparam.Extarnal2Zone[i] = uclExtarnalOutput2.GetCheckZone(i);
                systemparam.Extarnal3Zone[i] = uclExtarnalOutput3.GetCheckZone(i);
                systemparam.Extarnal4Zone[i] = uclExtarnalOutput4.GetCheckZone(i);
            }
            //systemparam.BuzzerTimer = (int)spinBuzzerTimer.Value; 
            //V1333 削除 代わりに「PatLiteResetTimeBothSide」（両端）と「PatLiteResetTimeCenter」（中央）に置き換え

            systemparam.TimeWarningPopupHour = (int)spinTimeWarningPopupHour.Value;

            //表示灯（両端）
            systemparam.PatLiteResetTimeBothSide = (int)uclPatLiteOutPutBothSide.spinPatLiteTimer.Value;//V1333 追加
            systemparam.PatLiteDelayTimeBothSide = (int)uclPatLiteOutPutBothSide.spinPatLiteDelay.Value;//V1333 追加

            //表示灯（中央）
            systemparam.PatLiteResetTimeCenter = (int)uclPatLiteOutPutCenter.spinPatLiteTimer.Value;//V1333 追加
            systemparam.PatLiteDelayTimeCenter = (int)uclPatLiteOutPutCenter.spinPatLiteDelay.Value;//V1333 追加

            //表示灯（両端）裏
            systemparam.PatLiteResetTimeBothSideUra = (int)uclPatLiteOutPutBothSideUra.spinPatLiteTimer.Value;//V1333 追加
            systemparam.PatLiteDelayTimeBothSideUra = (int)uclPatLiteOutPutBothSideUra.spinPatLiteDelay.Value;//V1333 追加

            //表示灯（中央）裏
            systemparam.PatLiteResetTimeCenterUra = (int)uclPatLiteOutPutCenterUra.spinPatLiteTimer.Value;//V1333 追加
            systemparam.PatLiteDelayTimeCenterUra = (int)uclPatLiteOutPutCenterUra.spinPatLiteDelay.Value;//V1333 追加

            systemparam.LineKandoGrapthColor = _lblLineColors[0].Text;
            systemparam.LineBaseKandoColor = _lblLineColors[1].Text;
            systemparam.LineInspHeightColor = _lblLineColors[2].Text;
            systemparam.LineInspWidthColor = _lblLineColors[3].Text;
            systemparam.LineStateMonitorColor = _lblLineColors[4].Text;

            systemparam.LineKandoGrapthThick = (int)spinLineThick1.Value;
            systemparam.LineBaseKandoThick = (int)spinLineThick2.Value;
            systemparam.LineInspHeightThick = (int)spinLineThick3.Value;
            systemparam.LineInspWidthThick = (int)spinLineThick4.Value;
            systemparam.LineStateMonitorThick = (int)spinLineThick5.Value;
            for (int i = 0; i < _spinKandoThick.Length; i++)
                systemparam.LineKandoThick[i] = (int)_spinKandoThick[i].Value;

            systemparam.ContinuNGEnable = chkContinuNGEnable.Checked;
            systemparam.ContinuNGJudgeCount = (int)spinContinuNGCount.Value;
            systemparam.ContinuNGAfterCancelCount = (int)spinContinuNGAfterCount.Value;

            systemparam.Common_RecipeRealSpeedOmote = (double)spinCommonRecipeRealSpeedOmote.Value;
            systemparam.Common_RecipeRealSpeedUra = (double)spinCommonRecipeRealSpeedUra.Value;

            systemparam.CommonPatLiteDelaySecond = (int)spinCommonPatLiteDelay.Value;//v1326
            systemparam.CommonPatLiteOnTimeSecond = (int)spinCommonPatLiteOnTime.Value;//v1326

            systemparam.SystemSave();

            _mainFrom.ChangeTimeAll();
            _mainFrom.SetTipColors();
            _mainFrom.SetNoInspMoni();

#if SPEED_MONITOR_NEW
            _mainFrom.SetSpeedMoniResoV();
#endif

            this.Cursor = Cursors.Default;
            Utility.ShowMessage(this, "変更を保存しました", MessageType.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        private void SystemDisp()
        {
            SystemParam systemparam = SystemParam.GetInstance();

            spinTimeDirect.Value = systemparam.LightWarningTime;

            spinDeleteMonth.Value = systemparam.DeleteMonth;

            chkAutoShutdown.Checked = systemparam.AutoShutdownEnable;
            spinShutdownTime.Value = systemparam.AutoShutdownWaitSec;

            for (int i = 0; i < AppData.CAM_COUNT; i++)
            {
                textboxArray[i, 0].Text = systemparam.camParam[i].PixV.ToString();
                textboxArray[i, 1].Text = systemparam.camParam[i].PixH.ToString();
                textboxArray[i, 2].Text = systemparam.camParam[i].ResoV.ToString("F4");
                textboxArray[i, 3].Text = systemparam.camParam[i].ResoH.ToString("F4");
                textboxArray[i, 4].Text = systemparam.camParam[i].ShiftV.ToString("F4");
                textboxArray[i, 5].Text = systemparam.camParam[i].ShiftH.ToString("F4");
            }

            for (int i = 0; i < _dgvNgColor.Length; i++)
            {
                List<MarkColor> mc = (i == 0) ? systemparam.markColorUpSide : systemparam.markColorDownSide;
                for (int j = 0; j < Enum.GetNames(typeof(AppData.InspID)).Length; j++)
                {
                    _dgvNgColor[i][j - (j / 3 * 3) + 1, (j / 3)].Value = mc[j].colorARGB;
                    _dgvNgColor[i][j - (j / 3 * 3) + 1, (j / 3)].Style.BackColor = ColorTranslator.FromHtml(mc[j].colorARGB);
                }
            }

            //点灯時間の表示
            LedLightTimeDisp();

            uclExtarnalOutput1.spinExtTimer.Value = systemparam.ExternalResetTime1;
            uclExtarnalOutput2.spinExtTimer.Value = systemparam.ExternalResetTime2;
            uclExtarnalOutput3.spinExtTimer.Value = systemparam.ExternalResetTime3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            uclExtarnalOutput4.spinExtTimer.Value = systemparam.ExternalResetTime4; //V1057 NG表裏修正 yuasa 20190118：外部４追加

            uclExtarnalOutput1.ExtTimer = systemparam.ExternalEnable1;
            uclExtarnalOutput2.ExtTimer = systemparam.ExternalEnable2;
            uclExtarnalOutput3.ExtTimer = systemparam.ExternalEnable3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            uclExtarnalOutput4.ExtTimer = systemparam.ExternalEnable4; //V1057 NG表裏修正 yuasa 20190118：外部４追加

            uclExtarnalOutput1.spinExtDelay.Value = systemparam.ExternalDelayTime1;
            uclExtarnalOutput2.spinExtDelay.Value = systemparam.ExternalDelayTime2;
            uclExtarnalOutput3.spinExtDelay.Value = systemparam.ExternalDelayTime3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            uclExtarnalOutput4.spinExtDelay.Value = systemparam.ExternalDelayTime4; //V1057 NG表裏修正 yuasa 20190118：外部４追加

            uclExtarnalOutput1.spinExtShot.Value = systemparam.ExternalShot1;
            uclExtarnalOutput2.spinExtShot.Value = systemparam.ExternalShot2;
            uclExtarnalOutput3.spinExtShot.Value = systemparam.ExternalShot3; //V1057 NG表裏修正 yuasa 20190118：外部３追加
            uclExtarnalOutput4.spinExtShot.Value = systemparam.ExternalShot4; //V1057 NG表裏修正 yuasa 20190118：外部４追加
            for (int i = 0; i < systemparam.Extarnal1Zone.Count; i++)
            {
                uclExtarnalOutput1.SetCheckZone(i, systemparam.Extarnal1Zone[i]);
                uclExtarnalOutput2.SetCheckZone(i, systemparam.Extarnal2Zone[i]);
                uclExtarnalOutput3.SetCheckZone(i, systemparam.Extarnal3Zone[i]);
                uclExtarnalOutput4.SetCheckZone(i, systemparam.Extarnal4Zone[i]);
            }

            //spinBuzzerTimer.Value = systemparam.BuzzerTimer;
            //V1333 削除 代わりに「PatLiteResetTimeBothSide」（両端）と「PatLiteResetTimeCenter」（中央）に置き換え  

            spinTimeWarningPopupHour.Value = systemparam.TimeWarningPopupHour;

            //表示灯（両端）
            uclPatLiteOutPutBothSide.spinPatLiteTimer.Value = systemparam.PatLiteResetTimeBothSide;//V1333 追加
            uclPatLiteOutPutBothSide.spinPatLiteDelay.Value = systemparam.PatLiteDelayTimeBothSide;//V1333 追加

            //表示灯（中央）
            uclPatLiteOutPutCenter.spinPatLiteTimer.Value = systemparam.PatLiteResetTimeCenter;//V1333 追加
            uclPatLiteOutPutCenter.spinPatLiteDelay.Value = systemparam.PatLiteDelayTimeCenter;//V1333 追加

            //表示灯（両端）裏
            uclPatLiteOutPutBothSideUra.spinPatLiteTimer.Value = systemparam.PatLiteResetTimeBothSideUra;//V1333 追加
            uclPatLiteOutPutBothSideUra.spinPatLiteDelay.Value = systemparam.PatLiteDelayTimeBothSideUra;//V1333 追加

            //表示灯（中央）裏
            uclPatLiteOutPutCenterUra.spinPatLiteTimer.Value = systemparam.PatLiteResetTimeCenterUra;//V1333 追加
            uclPatLiteOutPutCenterUra.spinPatLiteDelay.Value = systemparam.PatLiteDelayTimeCenterUra;//V1333 追加

            if (!SystemParam.GetInstance().ExternalFrontReverseDivide) //V1057 NG表裏修正 yuasa 20190118：iniファイルに応じてグレーアウト
            {
                uclExtarnalOutput3.Visible = false;
                uclExtarnalOutput4.Visible = false;

                uclExtarnalOutput1.CheckButtonTitle = "外部1(共通)";
                uclExtarnalOutput2.CheckButtonTitle = "外部2(共通)";
            }

            _lblLineColors[0].Text = systemparam.LineKandoGrapthColor;
            _lblLineColors[1].Text = systemparam.LineBaseKandoColor;
            _lblLineColors[2].Text = systemparam.LineInspHeightColor;
            _lblLineColors[3].Text = systemparam.LineInspWidthColor;
            _lblLineColors[4].Text = systemparam.LineStateMonitorColor;
            for (int i = 0; i < _lblLineColors.Length; i++)
                _lblLineColors[i].BackColor = ColorTranslator.FromHtml(_lblLineColors[i].Text);
            spinLineThick1.Value = systemparam.LineKandoGrapthThick;
            spinLineThick2.Value = systemparam.LineBaseKandoThick;
            spinLineThick3.Value = systemparam.LineInspHeightThick;
            spinLineThick4.Value = systemparam.LineInspWidthThick;
            spinLineThick5.Value = systemparam.LineStateMonitorThick;
            for (int i = 0; i < _spinKandoThick.Length; i++)
                _spinKandoThick[i].Value = systemparam.LineKandoThick[i];

            chkContinuNGEnable.Checked = systemparam.ContinuNGEnable;
            spinContinuNGCount.Value = systemparam.ContinuNGJudgeCount;
            spinContinuNGAfterCount.Value = systemparam.ContinuNGAfterCancelCount;
            DispContinuNGMs();
            DispContinuNGAfterMs();

            spinCommonRecipeRealSpeedOmote.Value = (decimal)systemparam.Common_RecipeRealSpeedOmote;
            spinCommonRecipeRealSpeedUra.Value = (decimal)systemparam.Common_RecipeRealSpeedUra;

            spinCommonPatLiteDelay.Value = (decimal)systemparam.CommonPatLiteDelaySecond; //v1326
            spinCommonPatLiteOnTime.Value = (decimal)systemparam.CommonPatLiteOnTimeSecond; //v1326
        }

        /// <summary>
        /// 照明警告時間表示            Hiển thị thời gian cảnh báo ánh sáng
        /// </summary>
        private void LedLightTimeDisp()
        {
            LightControlManager ltCtrl = LightControlManager.getInstance();
            SystemParam sysParam = SystemParam.GetInstance();
            SystemContext sysCont = SystemContext.GetInstance();
            for (int i = 0; i < ltCtrl.LightCount; i++)
            {
                _lightTimeReset[i].Title = ltCtrl.GetLight(i).Name;
                _lightTimeReset[i].DisplayValue = sysCont.LightMeasPeriod[i].AccumulateHour.ToString();
                _lightTimeReset[i].DisplayValueBackColor = (sysParam.LightWarningTime != 0 && sysCont.LightMeasPeriod[i].AccumulateHour >= sysParam.LightWarningTime) ? Color.Red : Color.White;
            }
        }

        private void dgvNgColor_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            Point point = new Point();
            point = dgv.CurrentCellAddress;

            if (point.X == 0 && point.Y == 0)
            {
            }
            else if (point.X == 0 && point.Y == 1)
            {
            }
            else
            {
                // ColorDialog の新しいインスタンス
                ColorDialog colorDialog1 = new ColorDialog();

                // 初期選択する色を設定する
                colorDialog1.Color = dgv[point.X, point.Y].Style.BackColor;

                // カスタム カラーを表示した状態にする (初期値 false)
                colorDialog1.FullOpen = true;

                // 使用可能なすべての色を基本セットに表示する (初期値 false)
                colorDialog1.AnyColor = true;

                // 純色のみ表示する (初期値 false)
                colorDialog1.SolidColorOnly = true;

                // カスタム カラーを任意の色で設定する
                colorDialog1.CustomColors = new int[] { 0x8040FF, 0xFF8040, 0x80FF40, 0x4080FF };

                // [ヘルプ] ボタンを表示する
                colorDialog1.ShowHelp = true;

                // ダイアログを表示し、戻り値が [OK] の場合は選択した色を textBox1 に適用する
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    dgv[point.X, point.Y].Style.BackColor = colorDialog1.Color;
                }


                int color;
                color = colorDialog1.Color.ToArgb();
                dgv[point.X, point.Y].Style.BackColor = Color.FromArgb(color);

                string stColor;
                stColor = ColorTranslator.ToHtml(colorDialog1.Color).ToString();

                dgv[point.X, point.Y].Value = stColor;


                // 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
                colorDialog1.Dispose();

                dgv.CurrentCell = null;
            }
        }
        private void dgvNgColor_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            Point point = dgv.CurrentCellAddress;
            dgv[point.X, point.Y].Style.SelectionBackColor = dgvUpSideNgColor[point.X, point.Y].Style.BackColor;
            dgv[point.X, point.Y].Style.SelectionForeColor = dgvUpSideNgColor[point.X, point.Y].Style.ForeColor;
        }

        /// <summary> Đặt lại thời gian chiếu sáng.</summary> 
        private void btnLightTimeReset_Click(object sender, EventArgs e)  
        {
            int no = (int)((uclLightTimeReset)sender).Tag;
            if (DialogResult.Yes == Utility.ShowMessage(this, "照明点灯時間をリセットします。\r\nよろしいですか？", MessageType.YesNo))
            {
                //点灯時間をクリアする
                SystemContext.GetInstance().LightMeasPeriod[no].Clear();
                //テキストボックスの表示を変える
                _mainFrom.ChangeTimeAll();

                //表示の更新
                LedLightTimeDisp();
            }

        }

        /// <summary>
        /// 変更チェック
        /// </summary>
        /// <returns></returns>
        private bool CheckRecipeUpdate()
        {
            bool bb = false;

            SystemParam systemparam = SystemParam.GetInstance();

            if (systemparam.LightWarningTime != (int)spinTimeDirect.Value)
                bb = true;
            if (systemparam.DeleteMonth != (int)spinDeleteMonth.Value)
                bb = true;
            if (systemparam.AutoShutdownWaitSec != (int)spinShutdownTime.Value)
                bb = true;

            //datadridviewのbackcolorの保存
            for (int i = 0; i < _dgvNgColor.Length; i++)
            {
                List<MarkColor> mc = (i == 0) ? systemparam.markColorUpSide : systemparam.markColorDownSide;
                for (int j = 0; j < Enum.GetNames(typeof(AppData.InspID)).Length; j++)
                {
                    if (mc[j].colorARGB != (_dgvNgColor[i][j - (j / 3 * 3) + 1, (j / 3)].Value).ToString())
                        bb = true;
                }
            }

            if (systemparam.ExternalResetTime1 != uclExtarnalOutput1.spinExtTimer.Value)
                bb = true;
            if (systemparam.ExternalResetTime2 != uclExtarnalOutput2.spinExtTimer.Value)
                bb = true;
            if (systemparam.ExternalResetTime3 != uclExtarnalOutput3.spinExtTimer.Value) //V1057 NG表裏修正 yuasa 20190118：外部３追加
                bb = true;
            if (systemparam.ExternalResetTime4 != uclExtarnalOutput4.spinExtTimer.Value) //V1057 NG表裏修正 yuasa 20190118：外部４追加
                bb = true;
            if (systemparam.ExternalEnable1 != uclExtarnalOutput1.ExtTimer)
                bb = true;
            if (systemparam.ExternalEnable2 != uclExtarnalOutput2.ExtTimer)
                bb = true;
            if (systemparam.ExternalEnable3 != uclExtarnalOutput3.ExtTimer) //V1057 NG表裏修正 yuasa 20190118：外部３追加
                bb = true;
            if (systemparam.ExternalEnable4 != uclExtarnalOutput4.ExtTimer) //V1057 NG表裏修正 yuasa 20190118：外部４追加
                bb = true;
            if (systemparam.ExternalDelayTime1 != uclExtarnalOutput1.spinExtDelay.Value)
                bb = true;
            if (systemparam.ExternalDelayTime2 != uclExtarnalOutput2.spinExtDelay.Value)
                bb = true;
            if (systemparam.ExternalDelayTime3 != uclExtarnalOutput3.spinExtDelay.Value) //V1057 NG表裏修正 yuasa 20190118：外部３追加
                bb = true;
            if (systemparam.ExternalDelayTime4 != uclExtarnalOutput4.spinExtDelay.Value) //V1057 NG表裏修正 yuasa 20190118：外部４追加
                bb = true;
            if (systemparam.ExternalShot1 != uclExtarnalOutput1.spinExtShot.Value)
                bb = true;
            if (systemparam.ExternalShot2 != uclExtarnalOutput2.spinExtShot.Value)
                bb = true;
            if (systemparam.ExternalShot3 != uclExtarnalOutput3.spinExtShot.Value) //V1057 NG表裏修正 yuasa 20190118：外部３追加
                bb = true;
            if (systemparam.ExternalShot4 != uclExtarnalOutput4.spinExtShot.Value) //V1057 NG表裏修正 yuasa 20190118：外部４追加
                bb = true;
            for (int i = 0; i < systemparam.Extarnal1Zone.Count; i++)
            {
                if (systemparam.Extarnal1Zone[i] != uclExtarnalOutput1.GetCheckZone(i))
                    bb = true;
                if (systemparam.Extarnal2Zone[i] != uclExtarnalOutput2.GetCheckZone(i))
                    bb = true;
                if (systemparam.Extarnal3Zone[i] != uclExtarnalOutput3.GetCheckZone(i))
                    bb = true;
                if (systemparam.Extarnal4Zone[i] != uclExtarnalOutput4.GetCheckZone(i))
                    bb = true;
            }
            //if (systemparam.BuzzerTimer != spinBuzzerTimer.Value) //V1333 spinBuzzerTimerを削除
            //    bb = true;
            if (systemparam.TimeWarningPopupHour != spinTimeWarningPopupHour.Value)
                bb = true;

            //表示灯（両端）
            if (systemparam.PatLiteResetTimeBothSide != uclPatLiteOutPutBothSide.spinPatLiteTimer.Value)//V1333 追加
                bb = true;
            if (systemparam.PatLiteDelayTimeBothSide != uclPatLiteOutPutBothSide.spinPatLiteDelay.Value)//V1333 追加
                bb = true;

            //表示灯（中央）
            if (systemparam.PatLiteResetTimeCenter != uclPatLiteOutPutCenter.spinPatLiteTimer.Value)//V1333 追加
                bb = true;
            if (systemparam.PatLiteDelayTimeCenter != uclPatLiteOutPutCenter.spinPatLiteDelay.Value)//V1333 追加
                bb = true;

            //表示灯（両端）裏
            if (systemparam.PatLiteResetTimeBothSideUra != uclPatLiteOutPutBothSideUra.spinPatLiteTimer.Value)//V1333 追加
                bb = true;
            if (systemparam.PatLiteDelayTimeBothSideUra != uclPatLiteOutPutBothSideUra.spinPatLiteDelay.Value)//V1333 追加
                bb = true;

            //表示灯（中央）裏
            if (systemparam.PatLiteResetTimeCenterUra != uclPatLiteOutPutCenterUra.spinPatLiteTimer.Value)//V1333 追加
                bb = true;
            if (systemparam.PatLiteDelayTimeCenterUra != uclPatLiteOutPutCenterUra.spinPatLiteDelay.Value)//V1333 追加
                bb = true;

            return bb;
        }

        private void ChangeDebugMode()
        {
            bool bb;
            if (!_mainFrom.GetAjustButtonVisible())
            {
                bb = false;
                btnChange.Location = labelChange.Location;
                btnCancel.Location = labelCancel.Location;
                this.Size = new Size(1092, 600);
            }
            else
            {
                bb = true; ;
            }
            tableLayoutCameara.Visible = bb;
        }

        private void EnabledDisp()
        {
            bool enable = false;

            ///設定１
            //データ管理
            grpData.Enabled = enable;
            //自動シャットダウン
            grpAuto.Enabled = enable;
            //時刻補正警告表示
            grpTimeWarning.Enabled = enable;

            ///欠点
            //マップ色
            grpNGKindColor.Enabled = enable;
            //ブザー音
            //grpBuzzer.Enabled = enable; //V1333 削除
            uclPatLiteOutPutBothSide.Enabled = enable;//V1333
            uclPatLiteOutPutCenter.Enabled = enable;//V1333

            ///外部出力
            //外部出力設定
            grpExtOutput.Enabled = enable;

            ///照明
            //照明点灯時間
            grpLedTime.Enabled = enable;
        }

        private void lblLineColor_Click(object sender, EventArgs e)
        {
            int index = (int)(((Label)sender).Tag);

            // ColorDialog の新しいインスタンス
            ColorDialog colorDialog1 = new ColorDialog();

            // 初期選択する色を設定する
            colorDialog1.Color = _lblLineColors[index].BackColor;

            // カスタム カラーを表示した状態にする (初期値 false)
            colorDialog1.FullOpen = true;

            // 使用可能なすべての色を基本セットに表示する (初期値 false)
            colorDialog1.AnyColor = true;

            // 純色のみ表示する (初期値 false)
            colorDialog1.SolidColorOnly = true;

            // カスタム カラーを任意の色で設定する
            colorDialog1.CustomColors = new int[] { 0x8040FF, 0xFF8040, 0x80FF40, 0x4080FF };

            // [ヘルプ] ボタンを表示する
            colorDialog1.ShowHelp = true;

            // ダイアログを表示し、戻り値が [OK] の場合は選択した色を textBox1 に適用する
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                _lblLineColors[index].BackColor = colorDialog1.Color;
            }

            int color;
            color = colorDialog1.Color.ToArgb();
            _lblLineColors[index].BackColor = Color.FromArgb(color);

            string stColor;
            stColor = ColorTranslator.ToHtml(colorDialog1.Color).ToString();

            _lblLineColors[index].Text = stColor;

            // 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
            colorDialog1.Dispose();
        }

        private void spinContinuNGCount_ValueChanged(object sender, EventArgs e)
        {
            DispContinuNGMs();
        }

        private void spinContinuNGAfterCount_ValueChanged(object sender, EventArgs e)
        {
            DispContinuNGAfterMs();
        }

        private void DispContinuNGMs()
        {
            double timeMs = GetHz2Time((int)spinContinuNGCount.Value);
            lblContinuNGms.Text = timeMs.ToString("F1") + "秒";
        }
        private void DispContinuNGAfterMs()
        {
            double timeMs = GetHz2Time((int)spinContinuNGAfterCount.Value);
            lblContinuNGAfterMs.Text = timeMs.ToString("F1") + "秒";
        }

        private double GetHz2Time(int imageCnt)
        {
            double imageHeight = CameraManager.getInstance().GetCamera(0).ImageHeight;
            double lineRate = CameraManager.getInstance().GetCamera(0).LineRate;
            return (imageHeight / lineRate * (double)imageCnt);
        }

        /// <summary> khởi động lại system </summary>
        private void btnSystemRestore_Click(object sender, EventArgs e)
        {
            if (SystemStatus.GetInstance().NowState == SystemStatus.State.Inspection
                || SystemStatus.GetInstance().NowState == SystemStatus.State.Suspend)
            {
                Utility.ShowMessage(this, "検査中は操作できません。", MessageType.Error);           // không thể kiểm tra trong quá trình vận hành
                return;
            }

            // Để khôi phục hệ thống,\nHãy tắt PC của bạn. \nBạn có chắc không?

            if (DialogResult.Yes == this.ShowMessage("システムを復旧するため、\nＰＣのシャットダウンを行います。\nよろしいですか？", MessageType.YesNo))   
            {
                this.Cursor = Cursors.WaitCursor;
                SystemParam.GetInstance().RestoreBackupFile();  // khôi phục file sao lưu
                SystemStatus.GetInstance().RestoreShutdown = true;   // tắt máy
                SystemStatus.GetInstance().MainForm.Close();  // đóng form
                this.Cursor = Cursors.Default;
            }
        }
        ///   Ctrl+Shift+Nhấp chuột phải thì nút đó ẩn hẳn đi k nhìn thấy
        private void lblMaskUse_MouseDown(object sender, MouseEventArgs e)             
        {
            //Ctrl+Shft+右クリック               Ctrl+Shift+Nhấp chuột phải thì nút đó ẩn hẳn đi k nhìn thấy
            if ((e.Button == MouseButtons.Right) & ((Control.ModifierKeys & Keys.Control) == Keys.Control) & ((Control.ModifierKeys & Keys.Shift) == Keys.Shift))
            {
                //ボタン有効無効を反転            Bật/tắt nút đảo ngược
                lblMaskUse.Visible = !lblMaskUse.Visible;
            }
        }
    }
}
