namespace LineCameraSheetSystem
{
    partial class frmImageMainParam
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
            this.tabDebugControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.chkGraphAvg = new System.Windows.Forms.CheckBox();
            this.chkGraphDark = new System.Windows.Forms.CheckBox();
            this.chkGraphLight = new System.Windows.Forms.CheckBox();
            this.chkKando = new System.Windows.Forms.CheckBox();
            this.chkMaskWidth = new System.Windows.Forms.CheckBox();
            this.chkInspHeight = new System.Windows.Forms.CheckBox();
            this.chkInspWidth = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.grpTargetImage = new System.Windows.Forms.GroupBox();
            this.chkTargetRed = new System.Windows.Forms.CheckBox();
            this.chkTargetGreen = new System.Windows.Forms.CheckBox();
            this.chkTargetOrg = new System.Windows.Forms.CheckBox();
            this.chkTargetGray = new System.Windows.Forms.CheckBox();
            this.chkTargetBlue = new System.Windows.Forms.CheckBox();
            this.chkOrgImageConnectMode = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.grpImageManual = new System.Windows.Forms.GroupBox();
            this.btnSaveImage = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.spinImageBufferCount = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.grpImageAuto = new System.Windows.Forms.GroupBox();
            this.chkAutoSave = new System.Windows.Forms.CheckBox();
            this.spinAutoSaveOneNgsaveCount = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinAutoSaveCount = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.spinNgCropSaveCount = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkGraphCalcAll = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.spinDispGraphWidth1ch = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinDispGraphWidth3ch = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.spinInspAreaConnectModeBufferArea = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinInspAreaConnectModeImagePoint = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.spinInspFuncOpening = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinInspFuncSelectArea = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinInspFuncClosing = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinInspFuncNgMax = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.spinAutoLightDetailUpLevel = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinAutoLightOkHighLimit = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinAutoLightOkLowLimit = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinAutoLightOkImageCount = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinAutoLightCheckImageCount = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.tabDebugControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.grpTargetImage.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.grpImageManual.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.grpImageAuto.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabDebugControl
            // 
            this.tabDebugControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabDebugControl.Controls.Add(this.tabPage1);
            this.tabDebugControl.Controls.Add(this.tabPage2);
            this.tabDebugControl.Controls.Add(this.tabPage3);
            this.tabDebugControl.Controls.Add(this.tabPage4);
            this.tabDebugControl.Controls.Add(this.tabPage8);
            this.tabDebugControl.Controls.Add(this.tabPage5);
            this.tabDebugControl.Controls.Add(this.tabPage6);
            this.tabDebugControl.Controls.Add(this.tabPage7);
            this.tabDebugControl.Controls.Add(this.tabPage9);
            this.tabDebugControl.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tabDebugControl.Location = new System.Drawing.Point(5, 7);
            this.tabDebugControl.Name = "tabDebugControl";
            this.tabDebugControl.SelectedIndex = 0;
            this.tabDebugControl.Size = new System.Drawing.Size(555, 194);
            this.tabDebugControl.TabIndex = 49;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.chkGraphAvg);
            this.tabPage1.Controls.Add(this.chkGraphDark);
            this.tabPage1.Controls.Add(this.chkGraphLight);
            this.tabPage1.Controls.Add(this.chkKando);
            this.tabPage1.Controls.Add(this.chkMaskWidth);
            this.tabPage1.Controls.Add(this.chkInspHeight);
            this.tabPage1.Controls.Add(this.chkInspWidth);
            this.tabPage1.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(547, 161);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "線表示";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 19);
            this.label1.TabIndex = 33;
            this.label1.Text = "感度グラフ表示";
            // 
            // chkGraphAvg
            // 
            this.chkGraphAvg.AutoSize = true;
            this.chkGraphAvg.Location = new System.Drawing.Point(240, 65);
            this.chkGraphAvg.Name = "chkGraphAvg";
            this.chkGraphAvg.Size = new System.Drawing.Size(66, 23);
            this.chkGraphAvg.TabIndex = 32;
            this.chkGraphAvg.Text = "平均";
            this.chkGraphAvg.UseVisualStyleBackColor = true;
            this.chkGraphAvg.CheckedChanged += new System.EventHandler(this.chkGraphAvg_CheckedChanged);
            // 
            // chkGraphDark
            // 
            this.chkGraphDark.AutoSize = true;
            this.chkGraphDark.Location = new System.Drawing.Point(187, 65);
            this.chkGraphDark.Name = "chkGraphDark";
            this.chkGraphDark.Size = new System.Drawing.Size(47, 23);
            this.chkGraphDark.TabIndex = 32;
            this.chkGraphDark.Text = "暗";
            this.chkGraphDark.UseVisualStyleBackColor = true;
            this.chkGraphDark.CheckedChanged += new System.EventHandler(this.chkGraphDark_CheckedChanged);
            // 
            // chkGraphLight
            // 
            this.chkGraphLight.AutoSize = true;
            this.chkGraphLight.Location = new System.Drawing.Point(134, 65);
            this.chkGraphLight.Name = "chkGraphLight";
            this.chkGraphLight.Size = new System.Drawing.Size(47, 23);
            this.chkGraphLight.TabIndex = 32;
            this.chkGraphLight.Text = "明";
            this.chkGraphLight.UseVisualStyleBackColor = true;
            this.chkGraphLight.CheckedChanged += new System.EventHandler(this.chkGraphLight_CheckedChanged);
            // 
            // chkKando
            // 
            this.chkKando.AutoSize = true;
            this.chkKando.Location = new System.Drawing.Point(6, 94);
            this.chkKando.Name = "chkKando";
            this.chkKando.Size = new System.Drawing.Size(175, 23);
            this.chkKando.TabIndex = 32;
            this.chkKando.Text = "設定感度線を表示";
            this.chkKando.UseVisualStyleBackColor = true;
            this.chkKando.CheckedChanged += new System.EventHandler(this.chkKando_CheckedChanged);
            // 
            // chkMaskWidth
            // 
            this.chkMaskWidth.AutoSize = true;
            this.chkMaskWidth.Location = new System.Drawing.Point(167, 34);
            this.chkMaskWidth.Name = "chkMaskWidth";
            this.chkMaskWidth.Size = new System.Drawing.Size(160, 23);
            this.chkMaskWidth.TabIndex = 32;
            this.chkMaskWidth.Text = "マスク幅線を表示";
            this.chkMaskWidth.UseVisualStyleBackColor = true;
            this.chkMaskWidth.CheckedChanged += new System.EventHandler(this.chkMaskWidth_CheckedChanged);
            // 
            // chkInspHeight
            // 
            this.chkInspHeight.AutoSize = true;
            this.chkInspHeight.Location = new System.Drawing.Point(5, 5);
            this.chkInspHeight.Name = "chkInspHeight";
            this.chkInspHeight.Size = new System.Drawing.Size(156, 23);
            this.chkInspHeight.TabIndex = 32;
            this.chkInspHeight.Text = "検査高線を表示";
            this.chkInspHeight.UseVisualStyleBackColor = true;
            this.chkInspHeight.CheckedChanged += new System.EventHandler(this.chkInspHeight_CheckedChanged);
            // 
            // chkInspWidth
            // 
            this.chkInspWidth.AutoSize = true;
            this.chkInspWidth.Location = new System.Drawing.Point(167, 5);
            this.chkInspWidth.Name = "chkInspWidth";
            this.chkInspWidth.Size = new System.Drawing.Size(156, 23);
            this.chkInspWidth.TabIndex = 32;
            this.chkInspWidth.Text = "検査幅線を表示";
            this.chkInspWidth.UseVisualStyleBackColor = true;
            this.chkInspWidth.CheckedChanged += new System.EventHandler(this.chkInspWidth_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.grpTargetImage);
            this.tabPage2.Controls.Add(this.chkOrgImageConnectMode);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(547, 161);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "表示画像";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // grpTargetImage
            // 
            this.grpTargetImage.Controls.Add(this.chkTargetRed);
            this.grpTargetImage.Controls.Add(this.chkTargetGreen);
            this.grpTargetImage.Controls.Add(this.chkTargetOrg);
            this.grpTargetImage.Controls.Add(this.chkTargetGray);
            this.grpTargetImage.Controls.Add(this.chkTargetBlue);
            this.grpTargetImage.Location = new System.Drawing.Point(5, 5);
            this.grpTargetImage.Name = "grpTargetImage";
            this.grpTargetImage.Size = new System.Drawing.Size(184, 107);
            this.grpTargetImage.TabIndex = 34;
            this.grpTargetImage.TabStop = false;
            // 
            // chkTargetRed
            // 
            this.chkTargetRed.AutoSize = true;
            this.chkTargetRed.Location = new System.Drawing.Point(100, 20);
            this.chkTargetRed.Name = "chkTargetRed";
            this.chkTargetRed.Size = new System.Drawing.Size(59, 23);
            this.chkTargetRed.TabIndex = 0;
            this.chkTargetRed.Tag = "2";
            this.chkTargetRed.Text = "Red";
            this.chkTargetRed.UseVisualStyleBackColor = true;
            this.chkTargetRed.CheckedChanged += new System.EventHandler(this.chkDisplayTargetImage_CheckedChanged);
            // 
            // chkTargetGreen
            // 
            this.chkTargetGreen.AutoSize = true;
            this.chkTargetGreen.Location = new System.Drawing.Point(100, 49);
            this.chkTargetGreen.Name = "chkTargetGreen";
            this.chkTargetGreen.Size = new System.Drawing.Size(78, 23);
            this.chkTargetGreen.TabIndex = 0;
            this.chkTargetGreen.Tag = "3";
            this.chkTargetGreen.Text = "Green";
            this.chkTargetGreen.UseVisualStyleBackColor = true;
            this.chkTargetGreen.CheckedChanged += new System.EventHandler(this.chkDisplayTargetImage_CheckedChanged);
            // 
            // chkTargetOrg
            // 
            this.chkTargetOrg.AutoSize = true;
            this.chkTargetOrg.Checked = true;
            this.chkTargetOrg.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkTargetOrg.Location = new System.Drawing.Point(6, 20);
            this.chkTargetOrg.Name = "chkTargetOrg";
            this.chkTargetOrg.Size = new System.Drawing.Size(88, 23);
            this.chkTargetOrg.TabIndex = 0;
            this.chkTargetOrg.Tag = "0";
            this.chkTargetOrg.Text = "Original";
            this.chkTargetOrg.UseVisualStyleBackColor = true;
            this.chkTargetOrg.CheckedChanged += new System.EventHandler(this.chkDisplayTargetImage_CheckedChanged);
            // 
            // chkTargetGray
            // 
            this.chkTargetGray.AutoSize = true;
            this.chkTargetGray.Location = new System.Drawing.Point(6, 51);
            this.chkTargetGray.Name = "chkTargetGray";
            this.chkTargetGray.Size = new System.Drawing.Size(66, 23);
            this.chkTargetGray.TabIndex = 0;
            this.chkTargetGray.Tag = "1";
            this.chkTargetGray.Text = "Gray";
            this.chkTargetGray.UseVisualStyleBackColor = true;
            this.chkTargetGray.CheckedChanged += new System.EventHandler(this.chkDisplayTargetImage_CheckedChanged);
            // 
            // chkTargetBlue
            // 
            this.chkTargetBlue.AutoSize = true;
            this.chkTargetBlue.Location = new System.Drawing.Point(100, 78);
            this.chkTargetBlue.Name = "chkTargetBlue";
            this.chkTargetBlue.Size = new System.Drawing.Size(64, 23);
            this.chkTargetBlue.TabIndex = 0;
            this.chkTargetBlue.Tag = "4";
            this.chkTargetBlue.Text = "Blue";
            this.chkTargetBlue.UseVisualStyleBackColor = true;
            this.chkTargetBlue.CheckedChanged += new System.EventHandler(this.chkDisplayTargetImage_CheckedChanged);
            // 
            // chkOrgImageConnectMode
            // 
            this.chkOrgImageConnectMode.AutoSize = true;
            this.chkOrgImageConnectMode.Location = new System.Drawing.Point(5, 116);
            this.chkOrgImageConnectMode.Name = "chkOrgImageConnectMode";
            this.chkOrgImageConnectMode.Size = new System.Drawing.Size(265, 23);
            this.chkOrgImageConnectMode.TabIndex = 0;
            this.chkOrgImageConnectMode.Tag = "1";
            this.chkOrgImageConnectMode.Text = "取込画像を連結数と同じにする";
            this.chkOrgImageConnectMode.UseVisualStyleBackColor = true;
            this.chkOrgImageConnectMode.CheckedChanged += new System.EventHandler(this.chkOrgImageConnectMode_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.grpImageManual);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(547, 161);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "保存1";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // grpImageManual
            // 
            this.grpImageManual.Controls.Add(this.btnSaveImage);
            this.grpImageManual.Controls.Add(this.label2);
            this.grpImageManual.Controls.Add(this.spinImageBufferCount);
            this.grpImageManual.Location = new System.Drawing.Point(6, 6);
            this.grpImageManual.Name = "grpImageManual";
            this.grpImageManual.Size = new System.Drawing.Size(332, 71);
            this.grpImageManual.TabIndex = 6;
            this.grpImageManual.TabStop = false;
            this.grpImageManual.Text = "画像保持";
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Enabled = false;
            this.btnSaveImage.Location = new System.Drawing.Point(243, 22);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(83, 40);
            this.btnSaveImage.TabIndex = 4;
            this.btnSaveImage.Text = "保存";
            this.btnSaveImage.UseVisualStyleBackColor = true;
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "保持数";
            // 
            // spinImageBufferCount
            // 
            this.spinImageBufferCount.DecimalPlaces = 0;
            this.spinImageBufferCount.EveryValueChanged = false;
            this.spinImageBufferCount.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinImageBufferCount.Location = new System.Drawing.Point(75, 22);
            this.spinImageBufferCount.Margin = new System.Windows.Forms.Padding(0);
            this.spinImageBufferCount.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.spinImageBufferCount.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.spinImageBufferCount.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinImageBufferCount.Name = "spinImageBufferCount";
            this.spinImageBufferCount.Size = new System.Drawing.Size(125, 44);
            this.spinImageBufferCount.TabIndex = 2;
            this.spinImageBufferCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinImageBufferCount.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.spinImageBufferCount.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinImageBufferCount_ValueChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.grpImageAuto);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(547, 161);
            this.tabPage4.TabIndex = 4;
            this.tabPage4.Text = "保存2";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // grpImageAuto
            // 
            this.grpImageAuto.Controls.Add(this.chkAutoSave);
            this.grpImageAuto.Controls.Add(this.spinAutoSaveOneNgsaveCount);
            this.grpImageAuto.Controls.Add(this.spinAutoSaveCount);
            this.grpImageAuto.Controls.Add(this.label10);
            this.grpImageAuto.Controls.Add(this.label4);
            this.grpImageAuto.Location = new System.Drawing.Point(6, 6);
            this.grpImageAuto.Name = "grpImageAuto";
            this.grpImageAuto.Size = new System.Drawing.Size(332, 152);
            this.grpImageAuto.TabIndex = 6;
            this.grpImageAuto.TabStop = false;
            this.grpImageAuto.Text = "                                            ";
            // 
            // chkAutoSave
            // 
            this.chkAutoSave.AutoSize = true;
            this.chkAutoSave.Location = new System.Drawing.Point(6, 0);
            this.chkAutoSave.Name = "chkAutoSave";
            this.chkAutoSave.Size = new System.Drawing.Size(268, 23);
            this.chkAutoSave.TabIndex = 1;
            this.chkAutoSave.Text = "NG発生時画像をBMP保存する";
            this.chkAutoSave.UseVisualStyleBackColor = true;
            this.chkAutoSave.CheckedChanged += new System.EventHandler(this.chkAutoSave_CheckedChanged);
            // 
            // spinAutoSaveOneNgsaveCount
            // 
            this.spinAutoSaveOneNgsaveCount.DecimalPlaces = 0;
            this.spinAutoSaveOneNgsaveCount.EveryValueChanged = false;
            this.spinAutoSaveOneNgsaveCount.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinAutoSaveOneNgsaveCount.Location = new System.Drawing.Point(208, 70);
            this.spinAutoSaveOneNgsaveCount.Margin = new System.Windows.Forms.Padding(0);
            this.spinAutoSaveOneNgsaveCount.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.spinAutoSaveOneNgsaveCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinAutoSaveOneNgsaveCount.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinAutoSaveOneNgsaveCount.Name = "spinAutoSaveOneNgsaveCount";
            this.spinAutoSaveOneNgsaveCount.Size = new System.Drawing.Size(121, 44);
            this.spinAutoSaveOneNgsaveCount.TabIndex = 2;
            this.spinAutoSaveOneNgsaveCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinAutoSaveOneNgsaveCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.spinAutoSaveOneNgsaveCount.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinAutoSaveOneNgsaveCount_ValueChanged);
            // 
            // spinAutoSaveCount
            // 
            this.spinAutoSaveCount.DecimalPlaces = 0;
            this.spinAutoSaveCount.EveryValueChanged = false;
            this.spinAutoSaveCount.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinAutoSaveCount.Location = new System.Drawing.Point(163, 26);
            this.spinAutoSaveCount.Margin = new System.Windows.Forms.Padding(0);
            this.spinAutoSaveCount.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.spinAutoSaveCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinAutoSaveCount.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinAutoSaveCount.Name = "spinAutoSaveCount";
            this.spinAutoSaveCount.Size = new System.Drawing.Size(166, 44);
            this.spinAutoSaveCount.TabIndex = 2;
            this.spinAutoSaveCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinAutoSaveCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spinAutoSaveCount.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinAutoSaveCount_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(38, 70);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(152, 19);
            this.label10.TabIndex = 3;
            this.label10.Text = "1NGでの保存枚数";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 19);
            this.label4.TabIndex = 3;
            this.label4.Text = "最大フォルダ数";
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.label11);
            this.tabPage8.Controls.Add(this.label9);
            this.tabPage8.Controls.Add(this.spinNgCropSaveCount);
            this.tabPage8.Location = new System.Drawing.Point(4, 29);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(547, 161);
            this.tabPage8.TabIndex = 7;
            this.tabPage8.Text = "保存3";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(16, 52);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(212, 12);
            this.label11.TabIndex = 12;
            this.label11.Text = "1:ColorShading 2:ColorOrg 3:Dark 4:Right";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(111, 19);
            this.label9.TabIndex = 11;
            this.label9.Text = "NGCrop枚数";
            // 
            // spinNgCropSaveCount
            // 
            this.spinNgCropSaveCount.DecimalPlaces = 0;
            this.spinNgCropSaveCount.EveryValueChanged = false;
            this.spinNgCropSaveCount.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinNgCropSaveCount.Location = new System.Drawing.Point(122, 8);
            this.spinNgCropSaveCount.Margin = new System.Windows.Forms.Padding(0);
            this.spinNgCropSaveCount.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.spinNgCropSaveCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinNgCropSaveCount.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinNgCropSaveCount.Name = "spinNgCropSaveCount";
            this.spinNgCropSaveCount.Size = new System.Drawing.Size(123, 44);
            this.spinNgCropSaveCount.TabIndex = 10;
            this.spinNgCropSaveCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinNgCropSaveCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinNgCropSaveCount.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinNgCropSaveCount_ValueChanged);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox1);
            this.tabPage5.Location = new System.Drawing.Point(4, 29);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(547, 161);
            this.tabPage5.TabIndex = 3;
            this.tabPage5.Text = "設定";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkGraphCalcAll);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.spinDispGraphWidth1ch);
            this.groupBox1.Controls.Add(this.spinDispGraphWidth3ch);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 126);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "グラフ";
            // 
            // chkGraphCalcAll
            // 
            this.chkGraphCalcAll.AutoSize = true;
            this.chkGraphCalcAll.Location = new System.Drawing.Point(97, 95);
            this.chkGraphCalcAll.Name = "chkGraphCalcAll";
            this.chkGraphCalcAll.Size = new System.Drawing.Size(176, 23);
            this.chkGraphCalcAll.TabIndex = 5;
            this.chkGraphCalcAll.Text = "ON:全体 OFF:下部";
            this.chkGraphCalcAll.UseVisualStyleBackColor = true;
            this.chkGraphCalcAll.CheckedChanged += new System.EventHandler(this.chkGraphCalcAll_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(154, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 19);
            this.label7.TabIndex = 4;
            this.label7.Text = "1ch時幅";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 19);
            this.label8.TabIndex = 4;
            this.label8.Text = "算出場所";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 19);
            this.label6.TabIndex = 4;
            this.label6.Text = "3ch時幅";
            // 
            // spinDispGraphWidth1ch
            // 
            this.spinDispGraphWidth1ch.DecimalPlaces = 0;
            this.spinDispGraphWidth1ch.EveryValueChanged = false;
            this.spinDispGraphWidth1ch.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinDispGraphWidth1ch.Location = new System.Drawing.Point(158, 41);
            this.spinDispGraphWidth1ch.Margin = new System.Windows.Forms.Padding(0);
            this.spinDispGraphWidth1ch.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.spinDispGraphWidth1ch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinDispGraphWidth1ch.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinDispGraphWidth1ch.Name = "spinDispGraphWidth1ch";
            this.spinDispGraphWidth1ch.Size = new System.Drawing.Size(142, 44);
            this.spinDispGraphWidth1ch.TabIndex = 3;
            this.spinDispGraphWidth1ch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinDispGraphWidth1ch.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.spinDispGraphWidth1ch.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinDispGraphWidth1ch_ValueChanged);
            // 
            // spinDispGraphWidth3ch
            // 
            this.spinDispGraphWidth3ch.DecimalPlaces = 0;
            this.spinDispGraphWidth3ch.EveryValueChanged = false;
            this.spinDispGraphWidth3ch.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinDispGraphWidth3ch.Location = new System.Drawing.Point(3, 41);
            this.spinDispGraphWidth3ch.Margin = new System.Windows.Forms.Padding(0);
            this.spinDispGraphWidth3ch.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.spinDispGraphWidth3ch.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinDispGraphWidth3ch.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinDispGraphWidth3ch.Name = "spinDispGraphWidth3ch";
            this.spinDispGraphWidth3ch.Size = new System.Drawing.Size(142, 44);
            this.spinDispGraphWidth3ch.TabIndex = 3;
            this.spinDispGraphWidth3ch.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinDispGraphWidth3ch.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.spinDispGraphWidth3ch.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinDispGraphWidth3ch_ValueChanged);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.label13);
            this.tabPage6.Controls.Add(this.label12);
            this.tabPage6.Controls.Add(this.spinInspAreaConnectModeBufferArea);
            this.tabPage6.Controls.Add(this.spinInspAreaConnectModeImagePoint);
            this.tabPage6.Location = new System.Drawing.Point(4, 29);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(547, 161);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "検査1";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(64, 54);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(117, 16);
            this.label13.TabIndex = 5;
            this.label13.Text = "次画像Top部Pix";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(11, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(170, 16);
            this.label12.TabIndex = 5;
            this.label12.Text = "検査位置(下から何枚目)";
            // 
            // spinInspAreaConnectModeBufferArea
            // 
            this.spinInspAreaConnectModeBufferArea.DecimalPlaces = 0;
            this.spinInspAreaConnectModeBufferArea.EveryValueChanged = false;
            this.spinInspAreaConnectModeBufferArea.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinInspAreaConnectModeBufferArea.Location = new System.Drawing.Point(184, 54);
            this.spinInspAreaConnectModeBufferArea.Margin = new System.Windows.Forms.Padding(0);
            this.spinInspAreaConnectModeBufferArea.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.spinInspAreaConnectModeBufferArea.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinInspAreaConnectModeBufferArea.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinInspAreaConnectModeBufferArea.Name = "spinInspAreaConnectModeBufferArea";
            this.spinInspAreaConnectModeBufferArea.Size = new System.Drawing.Size(142, 44);
            this.spinInspAreaConnectModeBufferArea.TabIndex = 3;
            this.spinInspAreaConnectModeBufferArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinInspAreaConnectModeBufferArea.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinInspAreaConnectModeBufferArea.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinInspAreaConnectModeBufferArea_ValueChanged);
            // 
            // spinInspAreaConnectModeImagePoint
            // 
            this.spinInspAreaConnectModeImagePoint.DecimalPlaces = 0;
            this.spinInspAreaConnectModeImagePoint.EveryValueChanged = false;
            this.spinInspAreaConnectModeImagePoint.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinInspAreaConnectModeImagePoint.Location = new System.Drawing.Point(184, 10);
            this.spinInspAreaConnectModeImagePoint.Margin = new System.Windows.Forms.Padding(0);
            this.spinInspAreaConnectModeImagePoint.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spinInspAreaConnectModeImagePoint.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinInspAreaConnectModeImagePoint.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinInspAreaConnectModeImagePoint.Name = "spinInspAreaConnectModeImagePoint";
            this.spinInspAreaConnectModeImagePoint.Size = new System.Drawing.Size(142, 44);
            this.spinInspAreaConnectModeImagePoint.TabIndex = 3;
            this.spinInspAreaConnectModeImagePoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinInspAreaConnectModeImagePoint.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinInspAreaConnectModeImagePoint.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinInspAreaConnectModeImagePoint_ValueChanged);
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.label14);
            this.tabPage7.Controls.Add(this.label15);
            this.tabPage7.Controls.Add(this.label5);
            this.tabPage7.Controls.Add(this.label3);
            this.tabPage7.Controls.Add(this.spinInspFuncOpening);
            this.tabPage7.Controls.Add(this.spinInspFuncSelectArea);
            this.tabPage7.Controls.Add(this.spinInspFuncClosing);
            this.tabPage7.Controls.Add(this.spinInspFuncNgMax);
            this.tabPage7.Location = new System.Drawing.Point(4, 29);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(547, 161);
            this.tabPage7.TabIndex = 8;
            this.tabPage7.Text = "検査2";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label14.Location = new System.Drawing.Point(6, 100);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 16);
            this.label14.TabIndex = 6;
            this.label14.Text = "除去";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label15.Location = new System.Drawing.Point(195, 81);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(93, 16);
            this.label15.TabIndex = 6;
            this.label15.Text = "SelectShape";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(6, 56);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "結合";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(6, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 16);
            this.label3.TabIndex = 6;
            this.label3.Text = "1画像内の有効NG数";
            // 
            // spinInspFuncOpening
            // 
            this.spinInspFuncOpening.DecimalPlaces = 1;
            this.spinInspFuncOpening.EveryValueChanged = false;
            this.spinInspFuncOpening.Incriment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.spinInspFuncOpening.Location = new System.Drawing.Point(49, 100);
            this.spinInspFuncOpening.Margin = new System.Windows.Forms.Padding(0);
            this.spinInspFuncOpening.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinInspFuncOpening.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.spinInspFuncOpening.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinInspFuncOpening.Name = "spinInspFuncOpening";
            this.spinInspFuncOpening.Size = new System.Drawing.Size(134, 44);
            this.spinInspFuncOpening.TabIndex = 4;
            this.spinInspFuncOpening.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinInspFuncOpening.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinInspFuncOpening.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinInspFuncOpening_ValueChanged);
            // 
            // spinInspFuncSelectArea
            // 
            this.spinInspFuncSelectArea.DecimalPlaces = 0;
            this.spinInspFuncSelectArea.EveryValueChanged = false;
            this.spinInspFuncSelectArea.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinInspFuncSelectArea.Location = new System.Drawing.Point(198, 100);
            this.spinInspFuncSelectArea.Margin = new System.Windows.Forms.Padding(0);
            this.spinInspFuncSelectArea.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinInspFuncSelectArea.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinInspFuncSelectArea.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinInspFuncSelectArea.Name = "spinInspFuncSelectArea";
            this.spinInspFuncSelectArea.Size = new System.Drawing.Size(134, 44);
            this.spinInspFuncSelectArea.TabIndex = 4;
            this.spinInspFuncSelectArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinInspFuncSelectArea.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinInspFuncSelectArea.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinInspFuncSelectArea_ValueChanged);
            // 
            // spinInspFuncClosing
            // 
            this.spinInspFuncClosing.DecimalPlaces = 1;
            this.spinInspFuncClosing.EveryValueChanged = false;
            this.spinInspFuncClosing.Incriment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.spinInspFuncClosing.Location = new System.Drawing.Point(49, 56);
            this.spinInspFuncClosing.Margin = new System.Windows.Forms.Padding(0);
            this.spinInspFuncClosing.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinInspFuncClosing.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.spinInspFuncClosing.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinInspFuncClosing.Name = "spinInspFuncClosing";
            this.spinInspFuncClosing.Size = new System.Drawing.Size(134, 44);
            this.spinInspFuncClosing.TabIndex = 4;
            this.spinInspFuncClosing.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinInspFuncClosing.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinInspFuncClosing.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinInspFuncClosing_ValueChanged);
            // 
            // spinInspFuncNgMax
            // 
            this.spinInspFuncNgMax.DecimalPlaces = 0;
            this.spinInspFuncNgMax.EveryValueChanged = false;
            this.spinInspFuncNgMax.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinInspFuncNgMax.Location = new System.Drawing.Point(155, 12);
            this.spinInspFuncNgMax.Margin = new System.Windows.Forms.Padding(0);
            this.spinInspFuncNgMax.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.spinInspFuncNgMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinInspFuncNgMax.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinInspFuncNgMax.Name = "spinInspFuncNgMax";
            this.spinInspFuncNgMax.Size = new System.Drawing.Size(142, 44);
            this.spinInspFuncNgMax.TabIndex = 4;
            this.spinInspFuncNgMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinInspFuncNgMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinInspFuncNgMax.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinInspFuncNgMax_ValueChanged);
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.label20);
            this.tabPage9.Controls.Add(this.label19);
            this.tabPage9.Controls.Add(this.label18);
            this.tabPage9.Controls.Add(this.label17);
            this.tabPage9.Controls.Add(this.label16);
            this.tabPage9.Controls.Add(this.spinAutoLightDetailUpLevel);
            this.tabPage9.Controls.Add(this.spinAutoLightOkHighLimit);
            this.tabPage9.Controls.Add(this.spinAutoLightOkLowLimit);
            this.tabPage9.Controls.Add(this.spinAutoLightOkImageCount);
            this.tabPage9.Controls.Add(this.spinAutoLightCheckImageCount);
            this.tabPage9.Location = new System.Drawing.Point(4, 29);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(547, 161);
            this.tabPage9.TabIndex = 9;
            this.tabPage9.Text = "自動調光";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label20.Location = new System.Drawing.Point(266, 100);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(108, 16);
            this.label20.TabIndex = 8;
            this.label20.Text = "詳細輝度値UP";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label19.Location = new System.Drawing.Point(246, 56);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(128, 16);
            this.label19.TabIndex = 8;
            this.label19.Text = "基準128＋範囲値";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label18.Location = new System.Drawing.Point(246, 12);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(128, 16);
            this.label18.TabIndex = 8;
            this.label18.Text = "基準128－範囲値";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label17.Location = new System.Drawing.Point(6, 56);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(74, 16);
            this.label17.TabIndex = 8;
            this.label17.Text = "Ok判定数";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label16.Location = new System.Drawing.Point(6, 12);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(79, 16);
            this.label16.TabIndex = 8;
            this.label16.Text = "Capture数";
            // 
            // spinAutoLightDetailUpLevel
            // 
            this.spinAutoLightDetailUpLevel.DecimalPlaces = 0;
            this.spinAutoLightDetailUpLevel.EveryValueChanged = false;
            this.spinAutoLightDetailUpLevel.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinAutoLightDetailUpLevel.Location = new System.Drawing.Point(377, 100);
            this.spinAutoLightDetailUpLevel.Margin = new System.Windows.Forms.Padding(0);
            this.spinAutoLightDetailUpLevel.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinAutoLightDetailUpLevel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinAutoLightDetailUpLevel.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinAutoLightDetailUpLevel.Name = "spinAutoLightDetailUpLevel";
            this.spinAutoLightDetailUpLevel.Size = new System.Drawing.Size(142, 44);
            this.spinAutoLightDetailUpLevel.TabIndex = 7;
            this.spinAutoLightDetailUpLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinAutoLightDetailUpLevel.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.spinAutoLightDetailUpLevel.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinAutoLightDetailUpLevel_ValueChanged);
            // 
            // spinAutoLightOkHighLimit
            // 
            this.spinAutoLightOkHighLimit.DecimalPlaces = 0;
            this.spinAutoLightOkHighLimit.EveryValueChanged = false;
            this.spinAutoLightOkHighLimit.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinAutoLightOkHighLimit.Location = new System.Drawing.Point(377, 56);
            this.spinAutoLightOkHighLimit.Margin = new System.Windows.Forms.Padding(0);
            this.spinAutoLightOkHighLimit.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.spinAutoLightOkHighLimit.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinAutoLightOkHighLimit.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinAutoLightOkHighLimit.Name = "spinAutoLightOkHighLimit";
            this.spinAutoLightOkHighLimit.Size = new System.Drawing.Size(142, 44);
            this.spinAutoLightOkHighLimit.TabIndex = 7;
            this.spinAutoLightOkHighLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinAutoLightOkHighLimit.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinAutoLightOkHighLimit.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinAutoLightOkHighLimit_ValueChanged);
            // 
            // spinAutoLightOkLowLimit
            // 
            this.spinAutoLightOkLowLimit.DecimalPlaces = 0;
            this.spinAutoLightOkLowLimit.EveryValueChanged = false;
            this.spinAutoLightOkLowLimit.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinAutoLightOkLowLimit.Location = new System.Drawing.Point(377, 12);
            this.spinAutoLightOkLowLimit.Margin = new System.Windows.Forms.Padding(0);
            this.spinAutoLightOkLowLimit.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinAutoLightOkLowLimit.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            -2147483648});
            this.spinAutoLightOkLowLimit.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinAutoLightOkLowLimit.Name = "spinAutoLightOkLowLimit";
            this.spinAutoLightOkLowLimit.Size = new System.Drawing.Size(142, 44);
            this.spinAutoLightOkLowLimit.TabIndex = 7;
            this.spinAutoLightOkLowLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinAutoLightOkLowLimit.Value = new decimal(new int[] {
            5,
            0,
            0,
            -2147483648});
            this.spinAutoLightOkLowLimit.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinAutoLightOkLowLimit_ValueChanged);
            // 
            // spinAutoLightOkImageCount
            // 
            this.spinAutoLightOkImageCount.DecimalPlaces = 0;
            this.spinAutoLightOkImageCount.EveryValueChanged = false;
            this.spinAutoLightOkImageCount.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinAutoLightOkImageCount.Location = new System.Drawing.Point(88, 56);
            this.spinAutoLightOkImageCount.Margin = new System.Windows.Forms.Padding(0);
            this.spinAutoLightOkImageCount.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.spinAutoLightOkImageCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinAutoLightOkImageCount.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinAutoLightOkImageCount.Name = "spinAutoLightOkImageCount";
            this.spinAutoLightOkImageCount.Size = new System.Drawing.Size(142, 44);
            this.spinAutoLightOkImageCount.TabIndex = 7;
            this.spinAutoLightOkImageCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinAutoLightOkImageCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinAutoLightOkImageCount.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinAutoLightOkImageCount_ValueChanged);
            // 
            // spinAutoLightCheckImageCount
            // 
            this.spinAutoLightCheckImageCount.DecimalPlaces = 0;
            this.spinAutoLightCheckImageCount.EveryValueChanged = false;
            this.spinAutoLightCheckImageCount.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinAutoLightCheckImageCount.Location = new System.Drawing.Point(88, 12);
            this.spinAutoLightCheckImageCount.Margin = new System.Windows.Forms.Padding(0);
            this.spinAutoLightCheckImageCount.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.spinAutoLightCheckImageCount.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.spinAutoLightCheckImageCount.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinAutoLightCheckImageCount.Name = "spinAutoLightCheckImageCount";
            this.spinAutoLightCheckImageCount.Size = new System.Drawing.Size(142, 44);
            this.spinAutoLightCheckImageCount.TabIndex = 7;
            this.spinAutoLightCheckImageCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinAutoLightCheckImageCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.spinAutoLightCheckImageCount.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinAutoLightCheckImageCount_ValueChanged);
            // 
            // frmImageMainParam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 205);
            this.Controls.Add(this.tabDebugControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmImageMainParam";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmImageMainParam";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmImageMainParam_Load);
            this.tabDebugControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.grpTargetImage.ResumeLayout(false);
            this.grpTargetImage.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.grpImageManual.ResumeLayout(false);
            this.grpImageManual.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.grpImageAuto.ResumeLayout(false);
            this.grpImageAuto.PerformLayout();
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage9.ResumeLayout(false);
            this.tabPage9.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabDebugControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkGraphAvg;
        private System.Windows.Forms.CheckBox chkGraphDark;
        private System.Windows.Forms.CheckBox chkGraphLight;
        private System.Windows.Forms.CheckBox chkKando;
        private System.Windows.Forms.CheckBox chkMaskWidth;
        private System.Windows.Forms.CheckBox chkInspHeight;
        private System.Windows.Forms.CheckBox chkInspWidth;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox grpTargetImage;
        private System.Windows.Forms.CheckBox chkTargetRed;
        private System.Windows.Forms.CheckBox chkTargetGreen;
        private System.Windows.Forms.CheckBox chkTargetOrg;
        private System.Windows.Forms.CheckBox chkTargetGray;
        private System.Windows.Forms.CheckBox chkTargetBlue;
        private System.Windows.Forms.CheckBox chkOrgImageConnectMode;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox grpImageManual;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage4;
        private Fujita.InspectionSystem.uclNumericInputSmall spinAutoSaveOneNgsaveCount;
        private Fujita.InspectionSystem.uclNumericInputSmall spinAutoSaveCount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tabPage8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private Fujita.InspectionSystem.uclNumericInputSmall spinNgCropSaveCount;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkGraphCalcAll;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private Fujita.InspectionSystem.uclNumericInputSmall spinDispGraphWidth1ch;
        private Fujita.InspectionSystem.uclNumericInputSmall spinDispGraphWidth3ch;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private Fujita.InspectionSystem.uclNumericInputSmall spinInspAreaConnectModeBufferArea;
        private Fujita.InspectionSystem.uclNumericInputSmall spinInspAreaConnectModeImagePoint;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private Fujita.InspectionSystem.uclNumericInputSmall spinInspFuncOpening;
        private Fujita.InspectionSystem.uclNumericInputSmall spinInspFuncSelectArea;
        private Fujita.InspectionSystem.uclNumericInputSmall spinInspFuncClosing;
        private Fujita.InspectionSystem.uclNumericInputSmall spinInspFuncNgMax;
        public System.Windows.Forms.Button btnSaveImage;
        public System.Windows.Forms.CheckBox chkAutoSave;
        public Fujita.InspectionSystem.uclNumericInputSmall spinImageBufferCount;
        public System.Windows.Forms.GroupBox grpImageAuto;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.Label label16;
        private Fujita.InspectionSystem.uclNumericInputSmall spinAutoLightCheckImageCount;
        private Fujita.InspectionSystem.uclNumericInputSmall spinAutoLightOkImageCount;
        private System.Windows.Forms.Label label17;
        private Fujita.InspectionSystem.uclNumericInputSmall spinAutoLightOkLowLimit;
        private Fujita.InspectionSystem.uclNumericInputSmall spinAutoLightOkHighLimit;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private Fujita.InspectionSystem.uclNumericInputSmall spinAutoLightDetailUpLevel;
    }
}