namespace LineCameraSheetSystem
{
    partial class frmDevelopmentSetting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnApply = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.spinOutofBlackLimit = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinOutofWhiteLimit = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.chkOutofBlackEnable = new System.Windows.Forms.CheckBox();
            this.chkOutofWhiteEnable = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkColInspBlue = new System.Windows.Forms.CheckBox();
            this.chkColInspGreen = new System.Windows.Forms.CheckBox();
            this.chkColInspRed = new System.Windows.Forms.CheckBox();
            this.chkColInspGray = new System.Windows.Forms.CheckBox();
            this.grpInspBrightDark = new System.Windows.Forms.GroupBox();
            this.chkInspDark = new System.Windows.Forms.CheckBox();
            this.chkInspBright = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.spinSoftShadingCalcCnt = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.chkSoftShadingRun = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.grpSoftShading = new System.Windows.Forms.GroupBox();
            this.spinSoftShadingLimit = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.chkSoftShadingEnable = new System.Windows.Forms.CheckBox();
            this.label43 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.spinSoftShadingTargetGrayLevel = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.spinSideMaskDilation = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.chkSideMaskEnable = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.spinSideMaskThreshold = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.chkCamCloseOpenAutoLightEnable = new System.Windows.Forms.CheckBox();
            this.chkCamCloseOpenEnable = new System.Windows.Forms.CheckBox();
            this.spinCamSpeedUra = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinCamSpeed = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.lblCamExpUra = new System.Windows.Forms.Label();
            this.spinCamExposureUra = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinCamExposure = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.label10 = new System.Windows.Forms.Label();
            this.lblCamExp = new System.Windows.Forms.Label();
            this.lblCamHzUra = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblCamHz = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCamSpeed = new System.Windows.Forms.Button();
            this.btnCamExposure = new System.Windows.Forms.Button();
            this.btnSystemIniSaveBackup = new System.Windows.Forms.Button();
            this.btnSystemIniLoad = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpInspBrightDark.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.grpSoftShading.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApply.Location = new System.Drawing.Point(13, 440);
            this.btnApply.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(139, 65);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "適応";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(506, 440);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(139, 65);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(13, 12);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(632, 422);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.grpInspBrightDark);
            this.tabPage1.Location = new System.Drawing.Point(4, 42);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage1.Size = new System.Drawing.Size(624, 376);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "検査";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.spinOutofBlackLimit);
            this.groupBox3.Controls.Add(this.spinOutofWhiteLimit);
            this.groupBox3.Controls.Add(this.chkOutofBlackEnable);
            this.groupBox3.Controls.Add(this.chkOutofWhiteEnable);
            this.groupBox3.Location = new System.Drawing.Point(20, 206);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(435, 145);
            this.groupBox3.TabIndex = 41;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "カラー画像時（白黒除外）";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(111, 92);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 33);
            this.label11.TabIndex = 41;
            this.label11.Text = "0～";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(285, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 33);
            this.label9.TabIndex = 41;
            this.label9.Text = "～255";
            // 
            // spinOutofBlackLimit
            // 
            this.spinOutofBlackLimit.DecimalPlaces = 0;
            this.spinOutofBlackLimit.EveryValueChanged = false;
            this.spinOutofBlackLimit.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinOutofBlackLimit.Location = new System.Drawing.Point(177, 91);
            this.spinOutofBlackLimit.Margin = new System.Windows.Forms.Padding(0);
            this.spinOutofBlackLimit.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.spinOutofBlackLimit.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinOutofBlackLimit.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinOutofBlackLimit.Name = "spinOutofBlackLimit";
            this.spinOutofBlackLimit.Size = new System.Drawing.Size(165, 44);
            this.spinOutofBlackLimit.TabIndex = 40;
            this.spinOutofBlackLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinOutofBlackLimit.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // spinOutofWhiteLimit
            // 
            this.spinOutofWhiteLimit.DecimalPlaces = 0;
            this.spinOutofWhiteLimit.EveryValueChanged = false;
            this.spinOutofWhiteLimit.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinOutofWhiteLimit.Location = new System.Drawing.Point(117, 38);
            this.spinOutofWhiteLimit.Margin = new System.Windows.Forms.Padding(0);
            this.spinOutofWhiteLimit.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spinOutofWhiteLimit.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.spinOutofWhiteLimit.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinOutofWhiteLimit.Name = "spinOutofWhiteLimit";
            this.spinOutofWhiteLimit.Size = new System.Drawing.Size(165, 44);
            this.spinOutofWhiteLimit.TabIndex = 40;
            this.spinOutofWhiteLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinOutofWhiteLimit.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // chkOutofBlackEnable
            // 
            this.chkOutofBlackEnable.AutoSize = true;
            this.chkOutofBlackEnable.Location = new System.Drawing.Point(6, 91);
            this.chkOutofBlackEnable.Name = "chkOutofBlackEnable";
            this.chkOutofBlackEnable.Size = new System.Drawing.Size(107, 37);
            this.chkOutofBlackEnable.TabIndex = 17;
            this.chkOutofBlackEnable.Text = "Black";
            this.chkOutofBlackEnable.UseVisualStyleBackColor = true;
            // 
            // chkOutofWhiteEnable
            // 
            this.chkOutofWhiteEnable.AutoSize = true;
            this.chkOutofWhiteEnable.Location = new System.Drawing.Point(6, 38);
            this.chkOutofWhiteEnable.Name = "chkOutofWhiteEnable";
            this.chkOutofWhiteEnable.Size = new System.Drawing.Size(108, 37);
            this.chkOutofWhiteEnable.TabIndex = 17;
            this.chkOutofWhiteEnable.Text = "White";
            this.chkOutofWhiteEnable.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkColInspBlue);
            this.groupBox2.Controls.Add(this.chkColInspGreen);
            this.groupBox2.Controls.Add(this.chkColInspRed);
            this.groupBox2.Controls.Add(this.chkColInspGray);
            this.groupBox2.Location = new System.Drawing.Point(20, 113);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(435, 87);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "カラー画像時（検査画像）";
            // 
            // chkColInspBlue
            // 
            this.chkColInspBlue.AutoSize = true;
            this.chkColInspBlue.Checked = true;
            this.chkColInspBlue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkColInspBlue.Location = new System.Drawing.Point(322, 38);
            this.chkColInspBlue.Name = "chkColInspBlue";
            this.chkColInspBlue.Size = new System.Drawing.Size(93, 37);
            this.chkColInspBlue.TabIndex = 17;
            this.chkColInspBlue.Text = "Blue";
            this.chkColInspBlue.UseVisualStyleBackColor = true;
            // 
            // chkColInspGreen
            // 
            this.chkColInspGreen.AutoSize = true;
            this.chkColInspGreen.Checked = true;
            this.chkColInspGreen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkColInspGreen.Location = new System.Drawing.Point(201, 38);
            this.chkColInspGreen.Name = "chkColInspGreen";
            this.chkColInspGreen.Size = new System.Drawing.Size(115, 37);
            this.chkColInspGreen.TabIndex = 17;
            this.chkColInspGreen.Text = "Green";
            this.chkColInspGreen.UseVisualStyleBackColor = true;
            // 
            // chkColInspRed
            // 
            this.chkColInspRed.AutoSize = true;
            this.chkColInspRed.Checked = true;
            this.chkColInspRed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkColInspRed.Location = new System.Drawing.Point(109, 38);
            this.chkColInspRed.Name = "chkColInspRed";
            this.chkColInspRed.Size = new System.Drawing.Size(86, 37);
            this.chkColInspRed.TabIndex = 17;
            this.chkColInspRed.Text = "Red";
            this.chkColInspRed.UseVisualStyleBackColor = true;
            // 
            // chkColInspGray
            // 
            this.chkColInspGray.AutoSize = true;
            this.chkColInspGray.Checked = true;
            this.chkColInspGray.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkColInspGray.Location = new System.Drawing.Point(6, 38);
            this.chkColInspGray.Name = "chkColInspGray";
            this.chkColInspGray.Size = new System.Drawing.Size(97, 37);
            this.chkColInspGray.TabIndex = 17;
            this.chkColInspGray.Text = "Gray";
            this.chkColInspGray.UseVisualStyleBackColor = true;
            // 
            // grpInspBrightDark
            // 
            this.grpInspBrightDark.Controls.Add(this.chkInspDark);
            this.grpInspBrightDark.Controls.Add(this.chkInspBright);
            this.grpInspBrightDark.Location = new System.Drawing.Point(20, 20);
            this.grpInspBrightDark.Name = "grpInspBrightDark";
            this.grpInspBrightDark.Size = new System.Drawing.Size(163, 87);
            this.grpInspBrightDark.TabIndex = 40;
            this.grpInspBrightDark.TabStop = false;
            this.grpInspBrightDark.Text = "有効項目";
            // 
            // chkInspDark
            // 
            this.chkInspDark.AutoSize = true;
            this.chkInspDark.Checked = true;
            this.chkInspDark.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInspDark.Location = new System.Drawing.Point(78, 38);
            this.chkInspDark.Name = "chkInspDark";
            this.chkInspDark.Size = new System.Drawing.Size(66, 37);
            this.chkInspDark.TabIndex = 17;
            this.chkInspDark.Text = "暗";
            this.chkInspDark.UseVisualStyleBackColor = true;
            // 
            // chkInspBright
            // 
            this.chkInspBright.AutoSize = true;
            this.chkInspBright.Checked = true;
            this.chkInspBright.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInspBright.Location = new System.Drawing.Point(6, 38);
            this.chkInspBright.Name = "chkInspBright";
            this.chkInspBright.Size = new System.Drawing.Size(66, 37);
            this.chkInspBright.TabIndex = 17;
            this.chkInspBright.Text = "明";
            this.chkInspBright.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox4);
            this.tabPage2.Controls.Add(this.grpSoftShading);
            this.tabPage2.Location = new System.Drawing.Point(4, 42);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage2.Size = new System.Drawing.Size(624, 376);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "シェーディング";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.spinSoftShadingCalcCnt);
            this.groupBox4.Controls.Add(this.chkSoftShadingRun);
            this.groupBox4.Controls.Add(this.label14);
            this.groupBox4.Location = new System.Drawing.Point(20, 219);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(528, 140);
            this.groupBox4.TabIndex = 21;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "シェーディング";
            // 
            // spinSoftShadingCalcCnt
            // 
            this.spinSoftShadingCalcCnt.DecimalPlaces = 0;
            this.spinSoftShadingCalcCnt.EveryValueChanged = false;
            this.spinSoftShadingCalcCnt.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinSoftShadingCalcCnt.Location = new System.Drawing.Point(355, 87);
            this.spinSoftShadingCalcCnt.Margin = new System.Windows.Forms.Padding(0);
            this.spinSoftShadingCalcCnt.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.spinSoftShadingCalcCnt.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinSoftShadingCalcCnt.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinSoftShadingCalcCnt.Name = "spinSoftShadingCalcCnt";
            this.spinSoftShadingCalcCnt.Size = new System.Drawing.Size(165, 44);
            this.spinSoftShadingCalcCnt.TabIndex = 39;
            this.spinSoftShadingCalcCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinSoftShadingCalcCnt.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // chkSoftShadingRun
            // 
            this.chkSoftShadingRun.AutoSize = true;
            this.chkSoftShadingRun.Location = new System.Drawing.Point(6, 38);
            this.chkSoftShadingRun.Name = "chkSoftShadingRun";
            this.chkSoftShadingRun.Size = new System.Drawing.Size(260, 37);
            this.chkSoftShadingRun.TabIndex = 20;
            this.chkSoftShadingRun.Text = "シェーディングRUN";
            this.chkSoftShadingRun.UseVisualStyleBackColor = true;
            this.chkSoftShadingRun.CheckedChanged += new System.EventHandler(this.chkSoftShadingRun_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 88);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(346, 33);
            this.label14.TabIndex = 21;
            this.label14.Text = "係数を算出する画像枚数";
            // 
            // grpSoftShading
            // 
            this.grpSoftShading.Controls.Add(this.spinSoftShadingLimit);
            this.grpSoftShading.Controls.Add(this.chkSoftShadingEnable);
            this.grpSoftShading.Controls.Add(this.label43);
            this.grpSoftShading.Controls.Add(this.label2);
            this.grpSoftShading.Controls.Add(this.spinSoftShadingTargetGrayLevel);
            this.grpSoftShading.Location = new System.Drawing.Point(20, 20);
            this.grpSoftShading.Name = "grpSoftShading";
            this.grpSoftShading.Size = new System.Drawing.Size(359, 193);
            this.grpSoftShading.TabIndex = 19;
            this.grpSoftShading.TabStop = false;
            this.grpSoftShading.Text = "画像均一化";
            // 
            // spinSoftShadingLimit
            // 
            this.spinSoftShadingLimit.DecimalPlaces = 0;
            this.spinSoftShadingLimit.EveryValueChanged = false;
            this.spinSoftShadingLimit.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinSoftShadingLimit.Location = new System.Drawing.Point(183, 135);
            this.spinSoftShadingLimit.Margin = new System.Windows.Forms.Padding(0);
            this.spinSoftShadingLimit.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.spinSoftShadingLimit.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinSoftShadingLimit.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinSoftShadingLimit.Name = "spinSoftShadingLimit";
            this.spinSoftShadingLimit.Size = new System.Drawing.Size(165, 44);
            this.spinSoftShadingLimit.TabIndex = 39;
            this.spinSoftShadingLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinSoftShadingLimit.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // chkSoftShadingEnable
            // 
            this.chkSoftShadingEnable.AutoSize = true;
            this.chkSoftShadingEnable.Checked = true;
            this.chkSoftShadingEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSoftShadingEnable.Location = new System.Drawing.Point(6, 38);
            this.chkSoftShadingEnable.Name = "chkSoftShadingEnable";
            this.chkSoftShadingEnable.Size = new System.Drawing.Size(174, 37);
            this.chkSoftShadingEnable.TabIndex = 17;
            this.chkSoftShadingEnable.Text = "有効にする";
            this.chkSoftShadingEnable.UseVisualStyleBackColor = true;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(17, 78);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(163, 33);
            this.label43.TabIndex = 19;
            this.label43.Text = "ターゲット値";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 33);
            this.label2.TabIndex = 21;
            this.label2.Text = "有効リミット";
            // 
            // spinSoftShadingTargetGrayLevel
            // 
            this.spinSoftShadingTargetGrayLevel.DecimalPlaces = 0;
            this.spinSoftShadingTargetGrayLevel.EveryValueChanged = false;
            this.spinSoftShadingTargetGrayLevel.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinSoftShadingTargetGrayLevel.Location = new System.Drawing.Point(183, 78);
            this.spinSoftShadingTargetGrayLevel.Margin = new System.Windows.Forms.Padding(0);
            this.spinSoftShadingTargetGrayLevel.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spinSoftShadingTargetGrayLevel.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinSoftShadingTargetGrayLevel.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinSoftShadingTargetGrayLevel.Name = "spinSoftShadingTargetGrayLevel";
            this.spinSoftShadingTargetGrayLevel.Size = new System.Drawing.Size(165, 44);
            this.spinSoftShadingTargetGrayLevel.TabIndex = 39;
            this.spinSoftShadingTargetGrayLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinSoftShadingTargetGrayLevel.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 42);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage3.Size = new System.Drawing.Size(624, 376);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "マスク";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.spinSideMaskDilation);
            this.groupBox1.Controls.Add(this.chkSideMaskEnable);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.spinSideMaskThreshold);
            this.groupBox1.Location = new System.Drawing.Point(20, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 193);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "両端無効化";
            // 
            // spinSideMaskDilation
            // 
            this.spinSideMaskDilation.DecimalPlaces = 0;
            this.spinSideMaskDilation.EveryValueChanged = false;
            this.spinSideMaskDilation.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinSideMaskDilation.Location = new System.Drawing.Point(276, 135);
            this.spinSideMaskDilation.Margin = new System.Windows.Forms.Padding(0);
            this.spinSideMaskDilation.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.spinSideMaskDilation.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinSideMaskDilation.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinSideMaskDilation.Name = "spinSideMaskDilation";
            this.spinSideMaskDilation.Size = new System.Drawing.Size(165, 44);
            this.spinSideMaskDilation.TabIndex = 39;
            this.spinSideMaskDilation.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinSideMaskDilation.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // chkSideMaskEnable
            // 
            this.chkSideMaskEnable.AutoSize = true;
            this.chkSideMaskEnable.Checked = true;
            this.chkSideMaskEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSideMaskEnable.Location = new System.Drawing.Point(6, 38);
            this.chkSideMaskEnable.Name = "chkSideMaskEnable";
            this.chkSideMaskEnable.Size = new System.Drawing.Size(174, 37);
            this.chkSideMaskEnable.TabIndex = 17;
            this.chkSideMaskEnable.Text = "有効にする";
            this.chkSideMaskEnable.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 33);
            this.label1.TabIndex = 19;
            this.label1.Text = "ワーク検出しきい値";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 33);
            this.label3.TabIndex = 21;
            this.label3.Text = "膨張値";
            // 
            // spinSideMaskThreshold
            // 
            this.spinSideMaskThreshold.DecimalPlaces = 0;
            this.spinSideMaskThreshold.EveryValueChanged = false;
            this.spinSideMaskThreshold.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinSideMaskThreshold.Location = new System.Drawing.Point(276, 78);
            this.spinSideMaskThreshold.Margin = new System.Windows.Forms.Padding(0);
            this.spinSideMaskThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spinSideMaskThreshold.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinSideMaskThreshold.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinSideMaskThreshold.Name = "spinSideMaskThreshold";
            this.spinSideMaskThreshold.Size = new System.Drawing.Size(165, 44);
            this.spinSideMaskThreshold.TabIndex = 39;
            this.spinSideMaskThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinSideMaskThreshold.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.chkCamCloseOpenAutoLightEnable);
            this.tabPage4.Controls.Add(this.chkCamCloseOpenEnable);
            this.tabPage4.Controls.Add(this.spinCamSpeedUra);
            this.tabPage4.Controls.Add(this.spinCamSpeed);
            this.tabPage4.Controls.Add(this.lblCamExpUra);
            this.tabPage4.Controls.Add(this.spinCamExposureUra);
            this.tabPage4.Controls.Add(this.spinCamExposure);
            this.tabPage4.Controls.Add(this.label10);
            this.tabPage4.Controls.Add(this.lblCamExp);
            this.tabPage4.Controls.Add(this.lblCamHzUra);
            this.tabPage4.Controls.Add(this.label13);
            this.tabPage4.Controls.Add(this.label12);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Controls.Add(this.lblCamHz);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.label5);
            this.tabPage4.Controls.Add(this.label4);
            this.tabPage4.Controls.Add(this.btnCamSpeed);
            this.tabPage4.Controls.Add(this.btnCamExposure);
            this.tabPage4.Location = new System.Drawing.Point(4, 42);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tabPage4.Size = new System.Drawing.Size(624, 376);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "カメラ";
            // 
            // chkCamCloseOpenAutoLightEnable
            // 
            this.chkCamCloseOpenAutoLightEnable.AutoSize = true;
            this.chkCamCloseOpenAutoLightEnable.Checked = true;
            this.chkCamCloseOpenAutoLightEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCamCloseOpenAutoLightEnable.Location = new System.Drawing.Point(22, 325);
            this.chkCamCloseOpenAutoLightEnable.Name = "chkCamCloseOpenAutoLightEnable";
            this.chkCamCloseOpenAutoLightEnable.Size = new System.Drawing.Size(492, 37);
            this.chkCamCloseOpenAutoLightEnable.TabIndex = 41;
            this.chkCamCloseOpenAutoLightEnable.Text = "自動調光時にカメラを再オープンする";
            this.chkCamCloseOpenAutoLightEnable.UseVisualStyleBackColor = true;
            // 
            // chkCamCloseOpenEnable
            // 
            this.chkCamCloseOpenEnable.AutoSize = true;
            this.chkCamCloseOpenEnable.Checked = true;
            this.chkCamCloseOpenEnable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCamCloseOpenEnable.Location = new System.Drawing.Point(22, 282);
            this.chkCamCloseOpenEnable.Name = "chkCamCloseOpenEnable";
            this.chkCamCloseOpenEnable.Size = new System.Drawing.Size(492, 37);
            this.chkCamCloseOpenEnable.TabIndex = 41;
            this.chkCamCloseOpenEnable.Text = "検査開始時にカメラを再オープンする";
            this.chkCamCloseOpenEnable.UseVisualStyleBackColor = true;
            // 
            // spinCamSpeedUra
            // 
            this.spinCamSpeedUra.DecimalPlaces = 1;
            this.spinCamSpeedUra.EveryValueChanged = false;
            this.spinCamSpeedUra.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.spinCamSpeedUra.Location = new System.Drawing.Point(408, 49);
            this.spinCamSpeedUra.Margin = new System.Windows.Forms.Padding(0);
            this.spinCamSpeedUra.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.spinCamSpeedUra.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinCamSpeedUra.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinCamSpeedUra.Name = "spinCamSpeedUra";
            this.spinCamSpeedUra.Size = new System.Drawing.Size(199, 44);
            this.spinCamSpeedUra.TabIndex = 40;
            this.spinCamSpeedUra.Tag = "1";
            this.spinCamSpeedUra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinCamSpeedUra.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // spinCamSpeed
            // 
            this.spinCamSpeed.DecimalPlaces = 1;
            this.spinCamSpeed.EveryValueChanged = false;
            this.spinCamSpeed.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.spinCamSpeed.Location = new System.Drawing.Point(181, 49);
            this.spinCamSpeed.Margin = new System.Windows.Forms.Padding(0);
            this.spinCamSpeed.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.spinCamSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinCamSpeed.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinCamSpeed.Name = "spinCamSpeed";
            this.spinCamSpeed.Size = new System.Drawing.Size(199, 44);
            this.spinCamSpeed.TabIndex = 40;
            this.spinCamSpeed.Tag = "0";
            this.spinCamSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinCamSpeed.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // lblCamExpUra
            // 
            this.lblCamExpUra.AutoSize = true;
            this.lblCamExpUra.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCamExpUra.Location = new System.Drawing.Point(547, 130);
            this.lblCamExpUra.Name = "lblCamExpUra";
            this.lblCamExpUra.Size = new System.Drawing.Size(70, 24);
            this.lblCamExpUra.TabIndex = 20;
            this.lblCamExpUra.Text = "xxxx.x";
            // 
            // spinCamExposureUra
            // 
            this.spinCamExposureUra.DecimalPlaces = 0;
            this.spinCamExposureUra.EveryValueChanged = false;
            this.spinCamExposureUra.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinCamExposureUra.Location = new System.Drawing.Point(408, 181);
            this.spinCamExposureUra.Margin = new System.Windows.Forms.Padding(0);
            this.spinCamExposureUra.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.spinCamExposureUra.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinCamExposureUra.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinCamExposureUra.Name = "spinCamExposureUra";
            this.spinCamExposureUra.Size = new System.Drawing.Size(199, 44);
            this.spinCamExposureUra.TabIndex = 40;
            this.spinCamExposureUra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinCamExposureUra.Value = new decimal(new int[] {
            190,
            0,
            0,
            0});
            // 
            // spinCamExposure
            // 
            this.spinCamExposure.DecimalPlaces = 0;
            this.spinCamExposure.EveryValueChanged = false;
            this.spinCamExposure.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinCamExposure.Location = new System.Drawing.Point(181, 181);
            this.spinCamExposure.Margin = new System.Windows.Forms.Padding(0);
            this.spinCamExposure.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.spinCamExposure.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinCamExposure.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinCamExposure.Name = "spinCamExposure";
            this.spinCamExposure.Size = new System.Drawing.Size(199, 44);
            this.spinCamExposure.TabIndex = 40;
            this.spinCamExposure.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinCamExposure.Value = new decimal(new int[] {
            190,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(432, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(95, 24);
            this.label10.TabIndex = 20;
            this.label10.Text = "露光(us)";
            // 
            // lblCamExp
            // 
            this.lblCamExp.AutoSize = true;
            this.lblCamExp.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCamExp.Location = new System.Drawing.Point(320, 130);
            this.lblCamExp.Name = "lblCamExp";
            this.lblCamExp.Size = new System.Drawing.Size(70, 24);
            this.lblCamExp.TabIndex = 20;
            this.lblCamExp.Text = "xxxx.x";
            // 
            // lblCamHzUra
            // 
            this.lblCamHzUra.AutoSize = true;
            this.lblCamHzUra.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCamHzUra.Location = new System.Drawing.Point(536, 96);
            this.lblCamHzUra.Name = "lblCamHzUra";
            this.lblCamHzUra.Size = new System.Drawing.Size(81, 24);
            this.lblCamHzUra.TabIndex = 20;
            this.lblCamHzUra.Text = "xxxxx.x";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(492, 15);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(34, 24);
            this.label13.TabIndex = 20;
            this.label13.Text = "裏";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(265, 15);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(34, 24);
            this.label12.TabIndex = 20;
            this.label12.Text = "表";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(205, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 24);
            this.label7.TabIndex = 20;
            this.label7.Text = "露光(us)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(404, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(122, 24);
            this.label8.TabIndex = 20;
            this.label8.Text = "周波数(Hz)";
            // 
            // lblCamHz
            // 
            this.lblCamHz.AutoSize = true;
            this.lblCamHz.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCamHz.Location = new System.Drawing.Point(309, 96);
            this.lblCamHz.Name = "lblCamHz";
            this.lblCamHz.Size = new System.Drawing.Size(81, 24);
            this.lblCamHz.TabIndex = 20;
            this.lblCamHz.Text = "xxxxx.x";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(177, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 24);
            this.label6.TabIndex = 20;
            this.label6.Text = "周波数(Hz)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(171, 33);
            this.label5.TabIndex = 20;
            this.label5.Text = "速度(m/分)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 33);
            this.label4.TabIndex = 20;
            this.label4.Text = "露光値(us)";
            // 
            // btnCamSpeed
            // 
            this.btnCamSpeed.Location = new System.Drawing.Point(70, 96);
            this.btnCamSpeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCamSpeed.Name = "btnCamSpeed";
            this.btnCamSpeed.Size = new System.Drawing.Size(100, 44);
            this.btnCamSpeed.TabIndex = 0;
            this.btnCamSpeed.Text = "設定";
            this.btnCamSpeed.UseVisualStyleBackColor = true;
            this.btnCamSpeed.Click += new System.EventHandler(this.btnCamSpeed_Click);
            // 
            // btnCamExposure
            // 
            this.btnCamExposure.Location = new System.Drawing.Point(70, 217);
            this.btnCamExposure.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnCamExposure.Name = "btnCamExposure";
            this.btnCamExposure.Size = new System.Drawing.Size(100, 44);
            this.btnCamExposure.TabIndex = 0;
            this.btnCamExposure.Text = "設定";
            this.btnCamExposure.UseVisualStyleBackColor = true;
            this.btnCamExposure.Click += new System.EventHandler(this.btnCamExposure_Click);
            // 
            // btnSystemIniSaveBackup
            // 
            this.btnSystemIniSaveBackup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSystemIniSaveBackup.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSystemIniSaveBackup.Location = new System.Drawing.Point(160, 436);
            this.btnSystemIniSaveBackup.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSystemIniSaveBackup.Name = "btnSystemIniSaveBackup";
            this.btnSystemIniSaveBackup.Size = new System.Drawing.Size(120, 73);
            this.btnSystemIniSaveBackup.TabIndex = 0;
            this.btnSystemIniSaveBackup.Text = "システムIniバックアップ保存";
            this.btnSystemIniSaveBackup.UseVisualStyleBackColor = true;
            this.btnSystemIniSaveBackup.Click += new System.EventHandler(this.btnSystemIniSaveBackup_Click);
            // 
            // btnSystemIniLoad
            // 
            this.btnSystemIniLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSystemIniLoad.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSystemIniLoad.Location = new System.Drawing.Point(332, 436);
            this.btnSystemIniLoad.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnSystemIniLoad.Name = "btnSystemIniLoad";
            this.btnSystemIniLoad.Size = new System.Drawing.Size(120, 73);
            this.btnSystemIniLoad.TabIndex = 0;
            this.btnSystemIniLoad.Text = "システムIni読み込み";
            this.btnSystemIniLoad.UseVisualStyleBackColor = true;
            this.btnSystemIniLoad.Click += new System.EventHandler(this.btnSystemIniLoad_Click);
            // 
            // frmDevelopmentSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 33F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 517);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSystemIniLoad);
            this.Controls.Add(this.btnSystemIniSaveBackup);
            this.Controls.Add(this.btnApply);
            this.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(9, 8, 9, 8);
            this.Name = "frmDevelopmentSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "開発者設定";
            this.Load += new System.EventHandler(this.frmDevelopmentSetting_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.grpInspBrightDark.ResumeLayout(false);
            this.grpInspBrightDark.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.grpSoftShading.ResumeLayout(false);
            this.grpSoftShading.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox grpInspBrightDark;
        private System.Windows.Forms.CheckBox chkInspDark;
        private System.Windows.Forms.CheckBox chkInspBright;
        private System.Windows.Forms.GroupBox grpSoftShading;
        private Fujita.InspectionSystem.uclNumericInputSmall spinSoftShadingLimit;
        private System.Windows.Forms.CheckBox chkSoftShadingEnable;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label2;
        private Fujita.InspectionSystem.uclNumericInputSmall spinSoftShadingTargetGrayLevel;
        private System.Windows.Forms.GroupBox groupBox1;
        private Fujita.InspectionSystem.uclNumericInputSmall spinSideMaskDilation;
        private System.Windows.Forms.CheckBox chkSideMaskEnable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private Fujita.InspectionSystem.uclNumericInputSmall spinSideMaskThreshold;
        private System.Windows.Forms.CheckBox chkSoftShadingRun;
        private System.Windows.Forms.TabPage tabPage4;
        private Fujita.InspectionSystem.uclNumericInputSmall spinCamSpeed;
        private Fujita.InspectionSystem.uclNumericInputSmall spinCamExposure;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCamSpeed;
        private System.Windows.Forms.Button btnCamExposure;
        private System.Windows.Forms.Label lblCamHz;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblCamExp;
        private System.Windows.Forms.Label label7;
        private Fujita.InspectionSystem.uclNumericInputSmall spinCamSpeedUra;
        private System.Windows.Forms.Label lblCamExpUra;
        private Fujita.InspectionSystem.uclNumericInputSmall spinCamExposureUra;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblCamHzUra;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkCamCloseOpenEnable;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkColInspBlue;
        private System.Windows.Forms.CheckBox chkColInspGreen;
        private System.Windows.Forms.CheckBox chkColInspRed;
        private System.Windows.Forms.CheckBox chkColInspGray;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private Fujita.InspectionSystem.uclNumericInputSmall spinOutofBlackLimit;
        private Fujita.InspectionSystem.uclNumericInputSmall spinOutofWhiteLimit;
        private System.Windows.Forms.CheckBox chkOutofBlackEnable;
        private System.Windows.Forms.CheckBox chkOutofWhiteEnable;
        private System.Windows.Forms.GroupBox groupBox4;
        private Fujita.InspectionSystem.uclNumericInputSmall spinSoftShadingCalcCnt;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chkCamCloseOpenAutoLightEnable;
        private System.Windows.Forms.Button btnSystemIniSaveBackup;
        private System.Windows.Forms.Button btnSystemIniLoad;
    }
}