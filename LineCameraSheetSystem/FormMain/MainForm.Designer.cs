namespace LineCameraSheetSystem
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnNgList = new System.Windows.Forms.Button();
            this.btnMap = new System.Windows.Forms.Button();
            this.btnNgMiniImg = new System.Windows.Forms.Button();
            this.btnNg1Img = new System.Windows.Forms.Button();
            this.btnTotal = new System.Windows.Forms.Button();
            this.btnRecipe = new System.Windows.Forms.Button();
            this.btnOldList = new System.Windows.Forms.Button();
            this.btnSystem = new System.Windows.Forms.Button();
            this.btnSuspend = new System.Windows.Forms.Button();
            this.btnEnd = new System.Windows.Forms.Button();
            this.labelState = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textLength = new System.Windows.Forms.TextBox();
            this.textKindName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textLotNo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textWidth = new System.Windows.Forms.TextBox();
            this.textStartTime = new System.Windows.Forms.TextBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolSpeed1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolSpeed2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel11 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabelUpSide = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusLabelDownSide = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUpInspTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblDownInspTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel10 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCaptureBuffCount1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCaptureBuffCount2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCropSaveBufferCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabelNowTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label12 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textMask = new System.Windows.Forms.TextBox();
            this.tmrUpsShutdown = new System.Windows.Forms.Timer(this.components);
            this.UclSystem = new LineCameraSheetSystem.uclSystem();
            this.UclSheetMapReal = new SheetMapping.uclSheetMap();
            this.UclTotalReal = new LineCameraSheetSystem.uclTotal();
            this.UclTotalOld = new LineCameraSheetSystem.uclTotal();
            this.UclNgThumbnailReal = new LineCameraSheetSystem.uclNgThumbnail();
            this.UclRecipeContentsReal = new LineCameraSheetSystem.uclRecipeContents();
            this.UclNgThumbnailOld = new LineCameraSheetSystem.uclNgThumbnail();
            this.UclSheetMapOld = new SheetMapping.uclSheetMap();
            this.UclRecipeContentsOld = new LineCameraSheetSystem.uclRecipeContents();
            this.UclNgListReal = new LineCameraSheetSystem.uclNgList();
            this.UclNgListOld = new LineCameraSheetSystem.uclNgList();
            this.UclRecipeList = new LineCameraSheetSystem.uclRecipeList();
            this.UclOldList = new LineCameraSheetSystem.uclOldList();
            this.shortcutKeyHelper1 = new Extension.ShortcutKeyHelper(this.components);
            this.btnStart = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLightTime = new System.Windows.Forms.Button();
            this.tmTimeWorningPopup = new System.Windows.Forms.Timer(this.components);
            this.btnApplicationClose = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textSpeed = new System.Windows.Forms.TextBox();
            this.chkExtDinInsp = new System.Windows.Forms.CheckBox();
            this.UclImageMain = new LineCameraSheetSystem.uclImageMain();
            this.btnImageMain = new System.Windows.Forms.Button();
            this.btnNgReset = new System.Windows.Forms.Button();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNgList
            // 
            this.btnNgList.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnNgList.Location = new System.Drawing.Point(1141, 744);
            this.btnNgList.Name = "btnNgList";
            this.btnNgList.Size = new System.Drawing.Size(120, 60);
            this.btnNgList.TabIndex = 0;
            this.btnNgList.Text = "戻る";
            this.btnNgList.UseVisualStyleBackColor = true;
            this.btnNgList.Visible = false;
            this.btnNgList.Click += new System.EventHandler(this.btnNgList_Click);
            // 
            // btnMap
            // 
            this.btnMap.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnMap.Location = new System.Drawing.Point(1141, 480);
            this.btnMap.Name = "btnMap";
            this.btnMap.Size = new System.Drawing.Size(120, 60);
            this.btnMap.TabIndex = 1;
            this.btnMap.Text = "マップ";
            this.btnMap.UseVisualStyleBackColor = true;
            this.btnMap.Click += new System.EventHandler(this.btnMap_Click);
            // 
            // btnNgMiniImg
            // 
            this.btnNgMiniImg.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnNgMiniImg.Location = new System.Drawing.Point(1141, 546);
            this.btnNgMiniImg.Name = "btnNgMiniImg";
            this.btnNgMiniImg.Size = new System.Drawing.Size(120, 60);
            this.btnNgMiniImg.TabIndex = 2;
            this.btnNgMiniImg.Text = "画像";
            this.btnNgMiniImg.UseVisualStyleBackColor = true;
            this.btnNgMiniImg.Click += new System.EventHandler(this.btnNgMiniImg_Click);
            // 
            // btnNg1Img
            // 
            this.btnNg1Img.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnNg1Img.Location = new System.Drawing.Point(1140, 930);
            this.btnNg1Img.Name = "btnNg1Img";
            this.btnNg1Img.Size = new System.Drawing.Size(120, 22);
            this.btnNg1Img.TabIndex = 42;
            this.btnNg1Img.Text = "1NG画像";
            this.btnNg1Img.UseVisualStyleBackColor = true;
            this.btnNg1Img.Visible = false;
            this.btnNg1Img.Click += new System.EventHandler(this.btnNg1Img_Click);
            // 
            // btnTotal
            // 
            this.btnTotal.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnTotal.Location = new System.Drawing.Point(1141, 612);
            this.btnTotal.Name = "btnTotal";
            this.btnTotal.Size = new System.Drawing.Size(120, 60);
            this.btnTotal.TabIndex = 3;
            this.btnTotal.Text = "累計";
            this.btnTotal.UseVisualStyleBackColor = true;
            this.btnTotal.Click += new System.EventHandler(this.btnTotal_Click);
            // 
            // btnRecipe
            // 
            this.btnRecipe.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnRecipe.Location = new System.Drawing.Point(1141, 414);
            this.btnRecipe.Name = "btnRecipe";
            this.btnRecipe.Size = new System.Drawing.Size(120, 60);
            this.btnRecipe.TabIndex = 4;
            this.btnRecipe.Text = "品種";
            this.btnRecipe.UseVisualStyleBackColor = true;
            this.btnRecipe.Click += new System.EventHandler(this.btnRecipe_Click);
            // 
            // btnOldList
            // 
            this.btnOldList.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnOldList.Location = new System.Drawing.Point(1141, 678);
            this.btnOldList.Name = "btnOldList";
            this.btnOldList.Size = new System.Drawing.Size(120, 60);
            this.btnOldList.TabIndex = 5;
            this.btnOldList.Text = "過去ﾘｽﾄ";
            this.btnOldList.UseVisualStyleBackColor = true;
            this.btnOldList.Click += new System.EventHandler(this.btnOldList_Click);
            // 
            // btnSystem
            // 
            this.btnSystem.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSystem.Location = new System.Drawing.Point(1141, 875);
            this.btnSystem.Name = "btnSystem";
            this.btnSystem.Size = new System.Drawing.Size(119, 60);
            this.btnSystem.TabIndex = 6;
            this.btnSystem.Text = "ｼｽﾃﾑ";
            this.btnSystem.UseVisualStyleBackColor = true;
            this.btnSystem.Click += new System.EventHandler(this.btnSystem_Click);
            // 
            // btnSuspend
            // 
            this.btnSuspend.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSuspend.Location = new System.Drawing.Point(1111, 88);
            this.btnSuspend.Name = "btnSuspend";
            this.btnSuspend.Size = new System.Drawing.Size(24, 60);
            this.btnSuspend.TabIndex = 9;
            this.btnSuspend.Text = "中断";
            this.btnSuspend.UseVisualStyleBackColor = true;
            this.btnSuspend.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnEnd
            // 
            this.btnEnd.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnEnd.Location = new System.Drawing.Point(1141, 154);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.Size = new System.Drawing.Size(120, 60);
            this.btnEnd.TabIndex = 10;
            this.btnEnd.Text = "終了";
            this.btnEnd.UseVisualStyleBackColor = true;
            this.btnEnd.Click += new System.EventHandler(this.btnEnd_Click);
            // 
            // labelState
            // 
            this.labelState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelState.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelState.Location = new System.Drawing.Point(11, 7);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(110, 64);
            this.labelState.TabIndex = 0;
            this.labelState.Text = "label1";
            this.labelState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(125, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "測長";
            // 
            // textLength
            // 
            this.textLength.BackColor = System.Drawing.SystemColors.Window;
            this.textLength.Enabled = false;
            this.textLength.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textLength.Location = new System.Drawing.Point(155, 8);
            this.textLength.Name = "textLength";
            this.textLength.ReadOnly = true;
            this.textLength.Size = new System.Drawing.Size(75, 26);
            this.textLength.TabIndex = 2;
            this.textLength.TabStop = false;
            this.textLength.Text = "90000.0";
            this.textLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textKindName
            // 
            this.textKindName.BackColor = System.Drawing.SystemColors.Window;
            this.textKindName.Enabled = false;
            this.textKindName.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textKindName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.textKindName.Location = new System.Drawing.Point(320, 8);
            this.textKindName.Multiline = true;
            this.textKindName.Name = "textKindName";
            this.textKindName.ReadOnly = true;
            this.textKindName.Size = new System.Drawing.Size(270, 28);
            this.textKindName.TabIndex = 8;
            this.textKindName.TabStop = false;
            this.textKindName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(288, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "品種";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(276, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "ﾛｯﾄNo.";
            // 
            // textLotNo
            // 
            this.textLotNo.BackColor = System.Drawing.SystemColors.Window;
            this.textLotNo.Enabled = false;
            this.textLotNo.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textLotNo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textLotNo.Location = new System.Drawing.Point(320, 43);
            this.textLotNo.Multiline = true;
            this.textLotNo.Name = "textLotNo";
            this.textLotNo.ReadOnly = true;
            this.textLotNo.Size = new System.Drawing.Size(270, 28);
            this.textLotNo.TabIndex = 10;
            this.textLotNo.TabStop = false;
            this.textLotNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textLotNo.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textLotNo_MouseClick);
            this.textLotNo.TextChanged += new System.EventHandler(this.textLotNo_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(590, 50);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "検査幅[㎜]";
            this.label6.Visible = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(598, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "開始時刻";
            // 
            // textWidth
            // 
            this.textWidth.BackColor = System.Drawing.SystemColors.Window;
            this.textWidth.Enabled = false;
            this.textWidth.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textWidth.Location = new System.Drawing.Point(655, 43);
            this.textWidth.Name = "textWidth";
            this.textWidth.ReadOnly = true;
            this.textWidth.Size = new System.Drawing.Size(50, 26);
            this.textWidth.TabIndex = 14;
            this.textWidth.TabStop = false;
            this.textWidth.Text = "000";
            this.textWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textWidth.Visible = false;
            // 
            // textStartTime
            // 
            this.textStartTime.BackColor = System.Drawing.SystemColors.Window;
            this.textStartTime.Enabled = false;
            this.textStartTime.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textStartTime.Location = new System.Drawing.Point(655, 8);
            this.textStartTime.Name = "textStartTime";
            this.textStartTime.ReadOnly = true;
            this.textStartTime.Size = new System.Drawing.Size(185, 26);
            this.textStartTime.TabIndex = 12;
            this.textStartTime.TabStop = false;
            this.textStartTime.Text = "2016/11/03 12:12:12";
            this.textStartTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolSpeed1,
            this.toolStripStatusLabel3,
            this.toolSpeed2,
            this.toolStripStatusLabel11,
            this.toolStripStatusLabel4,
            this.StatusLabelUpSide,
            this.toolStripStatusLabel8,
            this.StatusLabelDownSide,
            this.lblUpInspTime,
            this.lblDownInspTime,
            this.toolStripStatusLabel10,
            this.lblCaptureBuffCount1,
            this.lblCaptureBuffCount2,
            this.lblCropSaveBufferCount,
            this.statusLabelNowTime});
            this.statusStrip.Location = new System.Drawing.Point(0, 952);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1268, 29);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 31;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(16, 24);
            this.toolStripStatusLabel1.Text = "S1";
            // 
            // toolSpeed1
            // 
            this.toolSpeed1.ActiveLinkColor = System.Drawing.Color.Red;
            this.toolSpeed1.AutoSize = false;
            this.toolSpeed1.Name = "toolSpeed1";
            this.toolSpeed1.Size = new System.Drawing.Size(40, 24);
            this.toolSpeed1.Text = "-";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.AutoSize = false;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(16, 24);
            this.toolStripStatusLabel3.Text = "S2";
            // 
            // toolSpeed2
            // 
            this.toolSpeed2.ActiveLinkColor = System.Drawing.Color.Red;
            this.toolSpeed2.AutoSize = false;
            this.toolSpeed2.Name = "toolSpeed2";
            this.toolSpeed2.Size = new System.Drawing.Size(40, 24);
            this.toolSpeed2.Text = "-";
            // 
            // toolStripStatusLabel11
            // 
            this.toolStripStatusLabel11.Name = "toolStripStatusLabel11";
            this.toolStripStatusLabel11.Size = new System.Drawing.Size(240, 24);
            this.toolStripStatusLabel11.Spring = true;
            this.toolStripStatusLabel11.Text = "     ";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(53, 24);
            this.toolStripStatusLabel4.Text = "感度 表:";
            // 
            // StatusLabelUpSide
            // 
            this.StatusLabelUpSide.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.StatusLabelUpSide.Name = "StatusLabelUpSide";
            this.StatusLabelUpSide.Size = new System.Drawing.Size(298, 24);
            this.StatusLabelUpSide.Text = "明000 暗000(000,000,000)";
            // 
            // toolStripStatusLabel8
            // 
            this.toolStripStatusLabel8.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            this.toolStripStatusLabel8.Size = new System.Drawing.Size(29, 24);
            this.toolStripStatusLabel8.Text = " 裏:";
            // 
            // StatusLabelDownSide
            // 
            this.StatusLabelDownSide.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.StatusLabelDownSide.Name = "StatusLabelDownSide";
            this.StatusLabelDownSide.Size = new System.Drawing.Size(298, 24);
            this.StatusLabelDownSide.Text = "明000 暗000(000,000,000)";
            // 
            // lblUpInspTime
            // 
            this.lblUpInspTime.AutoSize = false;
            this.lblUpInspTime.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUpInspTime.Name = "lblUpInspTime";
            this.lblUpInspTime.Size = new System.Drawing.Size(38, 24);
            this.lblUpInspTime.Text = "000ms";
            this.lblUpInspTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDownInspTime
            // 
            this.lblDownInspTime.AutoSize = false;
            this.lblDownInspTime.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDownInspTime.Name = "lblDownInspTime";
            this.lblDownInspTime.Size = new System.Drawing.Size(38, 24);
            this.lblDownInspTime.Text = "000ms";
            this.lblDownInspTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripStatusLabel10
            // 
            this.toolStripStatusLabel10.Name = "toolStripStatusLabel10";
            this.toolStripStatusLabel10.Size = new System.Drawing.Size(0, 24);
            // 
            // lblCaptureBuffCount1
            // 
            this.lblCaptureBuffCount1.Name = "lblCaptureBuffCount1";
            this.lblCaptureBuffCount1.Size = new System.Drawing.Size(12, 24);
            this.lblCaptureBuffCount1.Text = "-";
            // 
            // lblCaptureBuffCount2
            // 
            this.lblCaptureBuffCount2.Name = "lblCaptureBuffCount2";
            this.lblCaptureBuffCount2.Size = new System.Drawing.Size(12, 24);
            this.lblCaptureBuffCount2.Text = "-";
            // 
            // lblCropSaveBufferCount
            // 
            this.lblCropSaveBufferCount.Name = "lblCropSaveBufferCount";
            this.lblCropSaveBufferCount.Size = new System.Drawing.Size(13, 24);
            this.lblCropSaveBufferCount.Text = "0";
            // 
            // statusLabelNowTime
            // 
            this.statusLabelNowTime.Name = "statusLabelNowTime";
            this.statusLabelNowTime.Size = new System.Drawing.Size(110, 24);
            this.statusLabelNowTime.Text = "2000/01/01 12:12:12";
            this.statusLabelNowTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label12.Location = new System.Drawing.Point(1003, 32);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(22, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "[h]";
            this.label12.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label8.Location = new System.Drawing.Point(230, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "[m]";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(728, 49);
            this.label13.Margin = new System.Windows.Forms.Padding(0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(64, 13);
            this.label13.TabIndex = 15;
            this.label13.Text = "ﾏｽｸ幅[㎜]";
            this.label13.Visible = false;
            // 
            // textMask
            // 
            this.textMask.BackColor = System.Drawing.SystemColors.Window;
            this.textMask.Enabled = false;
            this.textMask.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textMask.Location = new System.Drawing.Point(790, 42);
            this.textMask.Name = "textMask";
            this.textMask.ReadOnly = true;
            this.textMask.Size = new System.Drawing.Size(50, 26);
            this.textMask.TabIndex = 16;
            this.textMask.TabStop = false;
            this.textMask.Text = "000";
            this.textMask.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textMask.Visible = false;
            // 
            // tmrUpsShutdown
            // 
            this.tmrUpsShutdown.Interval = 2000;
            this.tmrUpsShutdown.Tick += new System.EventHandler(this.tmrUpsShutdown_Tick);
            // 
            // UclSystem
            // 
            this.UclSystem.Location = new System.Drawing.Point(12, 580);
            this.UclSystem.Name = "UclSystem";
            this.UclSystem.Size = new System.Drawing.Size(332, 243);
            this.UclSystem.TabIndex = 37;
            this.UclSystem.Visible = false;
            this.UclSystem.Load += new System.EventHandler(this.UclSystem_Load);
            // 
            // UclSheetMapReal
            // 
            this.UclSheetMapReal.BackGroundColor = System.Drawing.Color.Black;
            this.UclSheetMapReal.CurrentPositionColor = System.Drawing.Color.Red;
            this.UclSheetMapReal.CurrentPosLineSize = 1F;
            this.UclSheetMapReal.CurrentPosMeter = 0D;
            this.UclSheetMapReal.HeaderDispFont = new System.Drawing.Font("ＭＳ ゴシック", 10F);
            this.UclSheetMapReal.HeaderSheetInfoColor = System.Drawing.Color.CadetBlue;
            this.UclSheetMapReal.HeaderSheetInfoHeight = 48;
            this.UclSheetMapReal.HeaderTopColor = System.Drawing.SystemColors.Control;
            this.UclSheetMapReal.HeaderTopHeight = 16;
            this.UclSheetMapReal.Location = new System.Drawing.Point(676, 168);
            this.UclSheetMapReal.LockPosition = false;
            this.UclSheetMapReal.Name = "UclSheetMapReal";
            this.UclSheetMapReal.OffsetBottom = 0F;
            this.UclSheetMapReal.OffsetLeft = 0F;
            this.UclSheetMapReal.OffsetRight = 0F;
            this.UclSheetMapReal.OffsetTop = 0F;
            this.UclSheetMapReal.RangeMeter = 50D;
            this.UclSheetMapReal.SheetLengthMeter = 0D;
            this.UclSheetMapReal.SheetTipItemContainer = null;
            this.UclSheetMapReal.SheetWidth = 800D;
            this.UclSheetMapReal.Size = new System.Drawing.Size(200, 200);
            this.UclSheetMapReal.SokucyouColumnWidth = 120;
            this.UclSheetMapReal.TabIndex = 38;
            this.UclSheetMapReal.TipColorDefault = System.Drawing.Color.Magenta;
            this.UclSheetMapReal.TipColors = new System.Drawing.Color[0];
            this.UclSheetMapReal.TipDefaultColor = System.Drawing.Color.Blue;
            this.UclSheetMapReal.TipSize = new System.Drawing.Size(0, 0);
            this.UclSheetMapReal.VertGridRange = 69F;
            this.UclSheetMapReal.VisibleScrollLock = false;
            this.UclSheetMapReal.Zones = new double[0];
            this.UclSheetMapReal.TipDoubleClicked += new SheetMapping.TipClickedEventHandler(this.UclSheetMapReal_TipDoubleClicked);
            // 
            // UclTotalReal
            // 
            this.UclTotalReal.EnableResetButton = true;
            this.UclTotalReal.Location = new System.Drawing.Point(570, 168);
            this.UclTotalReal.Name = "UclTotalReal";
            this.UclTotalReal.Size = new System.Drawing.Size(100, 200);
            this.UclTotalReal.TabIndex = 39;
            this.UclTotalReal.Visible = false;
            // 
            // UclTotalOld
            // 
            this.UclTotalOld.EnableResetButton = false;
            this.UclTotalOld.Location = new System.Drawing.Point(572, 374);
            this.UclTotalOld.Name = "UclTotalOld";
            this.UclTotalOld.Size = new System.Drawing.Size(100, 200);
            this.UclTotalOld.TabIndex = 40;
            this.UclTotalOld.Visible = false;
            // 
            // UclNgThumbnailReal
            // 
            this.UclNgThumbnailReal.iPageMax = 1;
            this.UclNgThumbnailReal.iPageNow = 1;
            this.UclNgThumbnailReal.iRemainder = 0;
            this.UclNgThumbnailReal.Location = new System.Drawing.Point(882, 168);
            this.UclNgThumbnailReal.LockAutoChangePage = false;
            this.UclNgThumbnailReal.Name = "UclNgThumbnailReal";
            this.UclNgThumbnailReal.Size = new System.Drawing.Size(200, 200);
            this.UclNgThumbnailReal.TabIndex = 41;
            this.UclNgThumbnailReal.Visible = false;
            // 
            // UclRecipeContentsReal
            // 
            this.UclRecipeContentsReal.BackColor = System.Drawing.SystemColors.Control;
            this.UclRecipeContentsReal.Location = new System.Drawing.Point(464, 168);
            this.UclRecipeContentsReal.Name = "UclRecipeContentsReal";
            this.UclRecipeContentsReal.Size = new System.Drawing.Size(100, 200);
            this.UclRecipeContentsReal.TabIndex = 36;
            this.UclRecipeContentsReal.Visible = false;
            // 
            // UclNgThumbnailOld
            // 
            this.UclNgThumbnailOld.iPageMax = 1;
            this.UclNgThumbnailOld.iPageNow = 1;
            this.UclNgThumbnailOld.iRemainder = 0;
            this.UclNgThumbnailOld.Location = new System.Drawing.Point(882, 374);
            this.UclNgThumbnailOld.LockAutoChangePage = false;
            this.UclNgThumbnailOld.Name = "UclNgThumbnailOld";
            this.UclNgThumbnailOld.Size = new System.Drawing.Size(200, 200);
            this.UclNgThumbnailOld.TabIndex = 41;
            this.UclNgThumbnailOld.Visible = false;
            // 
            // UclSheetMapOld
            // 
            this.UclSheetMapOld.BackGroundColor = System.Drawing.Color.Black;
            this.UclSheetMapOld.CurrentPositionColor = System.Drawing.Color.Red;
            this.UclSheetMapOld.CurrentPosLineSize = 1F;
            this.UclSheetMapOld.CurrentPosMeter = 0D;
            this.UclSheetMapOld.HeaderDispFont = new System.Drawing.Font("ＭＳ ゴシック", 10F);
            this.UclSheetMapOld.HeaderSheetInfoColor = System.Drawing.Color.CadetBlue;
            this.UclSheetMapOld.HeaderSheetInfoHeight = 48;
            this.UclSheetMapOld.HeaderTopColor = System.Drawing.SystemColors.Control;
            this.UclSheetMapOld.HeaderTopHeight = 16;
            this.UclSheetMapOld.Location = new System.Drawing.Point(676, 374);
            this.UclSheetMapOld.LockPosition = false;
            this.UclSheetMapOld.Name = "UclSheetMapOld";
            this.UclSheetMapOld.OffsetBottom = 0F;
            this.UclSheetMapOld.OffsetLeft = 0F;
            this.UclSheetMapOld.OffsetRight = 0F;
            this.UclSheetMapOld.OffsetTop = 0F;
            this.UclSheetMapOld.RangeMeter = 50D;
            this.UclSheetMapOld.SheetLengthMeter = 0D;
            this.UclSheetMapOld.SheetTipItemContainer = null;
            this.UclSheetMapOld.SheetWidth = 800D;
            this.UclSheetMapOld.Size = new System.Drawing.Size(200, 200);
            this.UclSheetMapOld.SokucyouColumnWidth = 120;
            this.UclSheetMapOld.TabIndex = 33;
            this.UclSheetMapOld.TipColorDefault = System.Drawing.Color.Magenta;
            this.UclSheetMapOld.TipColors = new System.Drawing.Color[0];
            this.UclSheetMapOld.TipDefaultColor = System.Drawing.Color.Blue;
            this.UclSheetMapOld.TipSize = new System.Drawing.Size(0, 0);
            this.UclSheetMapOld.VertGridRange = 69F;
            this.UclSheetMapOld.Visible = false;
            this.UclSheetMapOld.VisibleScrollLock = false;
            this.UclSheetMapOld.Zones = new double[0];
            this.UclSheetMapOld.TipDoubleClicked += new SheetMapping.TipClickedEventHandler(this.UclSheetMapOld_TipDoubleClicked);
            // 
            // UclRecipeContentsOld
            // 
            this.UclRecipeContentsOld.BackColor = System.Drawing.SystemColors.Control;
            this.UclRecipeContentsOld.Location = new System.Drawing.Point(466, 374);
            this.UclRecipeContentsOld.Name = "UclRecipeContentsOld";
            this.UclRecipeContentsOld.Size = new System.Drawing.Size(100, 200);
            this.UclRecipeContentsOld.TabIndex = 36;
            this.UclRecipeContentsOld.Visible = false;
            // 
            // UclNgListReal
            // 
            this.UclNgListReal.Location = new System.Drawing.Point(11, 168);
            this.UclNgListReal.Name = "UclNgListReal";
            this.UclNgListReal.Size = new System.Drawing.Size(200, 200);
            this.UclNgListReal.TabIndex = 35;
            // 
            // UclNgListOld
            // 
            this.UclNgListOld.Location = new System.Drawing.Point(12, 374);
            this.UclNgListOld.Name = "UclNgListOld";
            this.UclNgListOld.Size = new System.Drawing.Size(200, 200);
            this.UclNgListOld.TabIndex = 34;
            this.UclNgListOld.Visible = false;
            // 
            // UclRecipeList
            // 
            this.UclRecipeList.Location = new System.Drawing.Point(217, 168);
            this.UclRecipeList.Name = "UclRecipeList";
            this.UclRecipeList.selectItem = null;
            this.UclRecipeList.Size = new System.Drawing.Size(200, 406);
            this.UclRecipeList.TabIndex = 33;
            this.UclRecipeList.Visible = false;
            // 
            // UclOldList
            // 
            this.UclOldList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UclOldList.Location = new System.Drawing.Point(480, 630);
            this.UclOldList.Name = "UclOldList";
            this.UclOldList.Size = new System.Drawing.Size(601, 118);
            this.UclOldList.TabIndex = 36;
            this.UclOldList.Visible = false;
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnStart.Location = new System.Drawing.Point(1141, 88);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(120, 60);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "開始";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click_1);
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnReset.Location = new System.Drawing.Point(1141, 220);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(120, 60);
            this.btnReset.TabIndex = 7;
            this.btnReset.Text = "リセット";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(1003, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "[h]";
            this.label1.Visible = false;
            // 
            // btnLightTime
            // 
            this.btnLightTime.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnLightTime.Location = new System.Drawing.Point(845, 7);
            this.btnLightTime.Name = "btnLightTime";
            this.btnLightTime.Size = new System.Drawing.Size(77, 62);
            this.btnLightTime.TabIndex = 47;
            this.btnLightTime.Text = "照明点灯時間";
            this.btnLightTime.UseVisualStyleBackColor = true;
            this.btnLightTime.Click += new System.EventHandler(this.btnLightTime_Click);
            // 
            // tmTimeWorningPopup
            // 
            this.tmTimeWorningPopup.Enabled = true;
            this.tmTimeWorningPopup.Interval = 60000;
            this.tmTimeWorningPopup.Tick += new System.EventHandler(this.tmTimeWorningPopup_Tick);
            // 
            // btnApplicationClose
            // 
            this.btnApplicationClose.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnApplicationClose.Location = new System.Drawing.Point(1111, 7);
            this.btnApplicationClose.Name = "btnApplicationClose";
            this.btnApplicationClose.Size = new System.Drawing.Size(150, 62);
            this.btnApplicationClose.TabIndex = 10;
            this.btnApplicationClose.Text = "システム終了";
            this.btnApplicationClose.UseVisualStyleBackColor = true;
            this.btnApplicationClose.Click += new System.EventHandler(this.btnApplicationClose_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(125, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "速度";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label11.Location = new System.Drawing.Point(230, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(45, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "[m/分]";
            // 
            // textSpeed
            // 
            this.textSpeed.BackColor = System.Drawing.SystemColors.Window;
            this.textSpeed.Enabled = false;
            this.textSpeed.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textSpeed.Location = new System.Drawing.Point(155, 43);
            this.textSpeed.Name = "textSpeed";
            this.textSpeed.ReadOnly = true;
            this.textSpeed.Size = new System.Drawing.Size(75, 26);
            this.textSpeed.TabIndex = 5;
            this.textSpeed.TabStop = false;
            this.textSpeed.Text = "10.0";
            this.textSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chkExtDinInsp
            // 
            this.chkExtDinInsp.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkExtDinInsp.Font = new System.Drawing.Font("MS UI Gothic", 18F);
            this.chkExtDinInsp.Location = new System.Drawing.Point(982, 7);
            this.chkExtDinInsp.Name = "chkExtDinInsp";
            this.chkExtDinInsp.Size = new System.Drawing.Size(123, 62);
            this.chkExtDinInsp.TabIndex = 48;
            this.chkExtDinInsp.Text = "手動";
            this.chkExtDinInsp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkExtDinInsp.UseVisualStyleBackColor = true;
            this.chkExtDinInsp.CheckedChanged += new System.EventHandler(this.chkExtDinInsp_CheckedChanged);
            // 
            // UclImageMain
            // 
            this.UclImageMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UclImageMain.Location = new System.Drawing.Point(11, 88);
            this.UclImageMain.Name = "UclImageMain";
            this.UclImageMain.Size = new System.Drawing.Size(62, 49);
            this.UclImageMain.TabIndex = 49;
            this.UclImageMain.Visible = false;
            // 
            // btnImageMain
            // 
            this.btnImageMain.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnImageMain.Location = new System.Drawing.Point(1141, 348);
            this.btnImageMain.Name = "btnImageMain";
            this.btnImageMain.Size = new System.Drawing.Size(120, 60);
            this.btnImageMain.TabIndex = 4;
            this.btnImageMain.Text = "メイン";
            this.btnImageMain.UseVisualStyleBackColor = true;
            this.btnImageMain.Click += new System.EventHandler(this.btnImageMain_Click);
            // 
            // btnNgReset
            // 
            this.btnNgReset.Font = new System.Drawing.Font("MS UI Gothic", 18F);
            this.btnNgReset.Location = new System.Drawing.Point(1141, 810);
            this.btnNgReset.Name = "btnNgReset";
            this.btnNgReset.Size = new System.Drawing.Size(120, 60);
            this.btnNgReset.TabIndex = 50;
            this.btnNgReset.Text = "NGリセット";
            this.btnNgReset.UseVisualStyleBackColor = true;
            this.btnNgReset.Click += new System.EventHandler(this.btnNgReset_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 981);
            this.ControlBox = false;
            this.Controls.Add(this.btnNgReset);
            this.Controls.Add(this.UclImageMain);
            this.Controls.Add(this.chkExtDinInsp);
            this.Controls.Add(this.btnLightTime);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.UclSheetMapOld);
            this.Controls.Add(this.UclSheetMapReal);
            this.Controls.Add(this.UclRecipeContentsReal);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.UclSystem);
            this.Controls.Add(this.UclTotalReal);
            this.Controls.Add(this.UclTotalOld);
            this.Controls.Add(this.UclNgThumbnailReal);
            this.Controls.Add(this.UclNgThumbnailOld);
            this.Controls.Add(this.UclRecipeContentsOld);
            this.Controls.Add(this.UclNgListReal);
            this.Controls.Add(this.UclNgListOld);
            this.Controls.Add(this.UclRecipeList);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.textStartTime);
            this.Controls.Add(this.textMask);
            this.Controls.Add(this.textWidth);
            this.Controls.Add(this.textLotNo);
            this.Controls.Add(this.textSpeed);
            this.Controls.Add(this.textKindName);
            this.Controls.Add(this.textLength);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.btnApplicationClose);
            this.Controls.Add(this.btnEnd);
            this.Controls.Add(this.btnSuspend);
            this.Controls.Add(this.btnSystem);
            this.Controls.Add(this.btnOldList);
            this.Controls.Add(this.btnImageMain);
            this.Controls.Add(this.btnRecipe);
            this.Controls.Add(this.btnTotal);
            this.Controls.Add(this.btnNg1Img);
            this.Controls.Add(this.btnNgMiniImg);
            this.Controls.Add(this.btnMap);
            this.Controls.Add(this.btnNgList);
            this.Controls.Add(this.UclOldList);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "メイン画面";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.DoubleClick += new System.EventHandler(this.MainForm_DoubleClick);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnNgList;
        private System.Windows.Forms.Button btnMap;
        private System.Windows.Forms.Button btnNgMiniImg;
        private System.Windows.Forms.Button btnNg1Img;
        private System.Windows.Forms.Button btnTotal;
        private System.Windows.Forms.Button btnRecipe;
        private System.Windows.Forms.Button btnOldList;
        private System.Windows.Forms.Button btnSystem;
        private System.Windows.Forms.Button btnSuspend;
        private System.Windows.Forms.Button btnEnd;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textLength;
        private System.Windows.Forms.TextBox textKindName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textLotNo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textWidth;
        private System.Windows.Forms.TextBox textStartTime;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolSpeed1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolSpeed2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolStripStatusLabel statusLabelNowTime;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textMask;
        private SheetMapping.uclSheetMap UclSheetMapReal;
        private SheetMapping.uclSheetMap UclSheetMapOld;
        private uclNgList UclNgListReal;
        private uclRecipeContents UclRecipeContentsReal;
        private uclTotal UclTotalReal;
        private uclSystem UclSystem;
        private uclRecipeList UclRecipeList;
        private uclOldList UclOldList;
        private uclNgThumbnail UclNgThumbnailReal;
        private uclRecipeContents UclRecipeContentsOld;
        private uclTotal UclTotalOld;
        private uclNgThumbnail UclNgThumbnailOld;
        private uclNgList UclNgListOld;
        private System.Windows.Forms.Timer tmrUpsShutdown;
        private Extension.ShortcutKeyHelper shortcutKeyHelper1;
		private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabelUpSide;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
		private System.Windows.Forms.ToolStripStatusLabel StatusLabelDownSide;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel11;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLightTime;
		private System.Windows.Forms.Timer tmTimeWorningPopup;
        private System.Windows.Forms.Button btnApplicationClose;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textSpeed;
        private System.Windows.Forms.CheckBox chkExtDinInsp;
        private uclImageMain UclImageMain;
        private System.Windows.Forms.Button btnImageMain;
        private System.Windows.Forms.ToolStripStatusLabel lblUpInspTime;
        private System.Windows.Forms.ToolStripStatusLabel lblDownInspTime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel10;
        private System.Windows.Forms.ToolStripStatusLabel lblCaptureBuffCount1;
        private System.Windows.Forms.ToolStripStatusLabel lblCaptureBuffCount2;
        private System.Windows.Forms.ToolStripStatusLabel lblCropSaveBufferCount;
        private System.Windows.Forms.Button btnNgReset;
    }
}

