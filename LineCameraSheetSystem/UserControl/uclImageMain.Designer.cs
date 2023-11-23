namespace LineCameraSheetSystem
{
    partial class uclImageMain
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.hwcImageWnd1 = new HalconDotNet.HWindowControl();
            this.hwcImageWnd2 = new HalconDotNet.HWindowControl();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.cmbMagnify1 = new System.Windows.Forms.ComboBox();
            this.cmbMagnify2 = new System.Windows.Forms.ComboBox();
            this.lblKandoOmote = new System.Windows.Forms.Label();
            this.lblKandoUra = new System.Windows.Forms.Label();
            this.lblTitileOmote = new System.Windows.Forms.Label();
            this.lblTitleUra = new System.Windows.Forms.Label();
            this.lblInspStatusOmote = new System.Windows.Forms.Label();
            this.lblInspStatusUra = new System.Windows.Forms.Label();
            this.chkPause = new System.Windows.Forms.CheckBox();
            this.chkVerDisplayMode = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkDisplayModeTarget = new System.Windows.Forms.CheckBox();
            this.chkDisplayModeBase = new System.Windows.Forms.CheckBox();
            this.lblKandoMessage = new System.Windows.Forms.Label();
            this.btnKandoSave = new System.Windows.Forms.Button();
            this.btnKandoApplyCancel = new System.Windows.Forms.Button();
            this.tmKandoMessage = new System.Windows.Forms.Timer(this.components);
            this.grpTitleOmote = new System.Windows.Forms.TableLayoutPanel();
            this.grpTitleUra = new System.Windows.Forms.TableLayoutPanel();
            this.chkNgHistory = new System.Windows.Forms.CheckBox();
            this.lblNgHistoryTime = new System.Windows.Forms.Label();
            this.lblNgHistorySide = new System.Windows.Forms.Label();
            this.chkZoomEnable = new System.Windows.Forms.CheckBox();
            this.chkMoveEnable = new System.Windows.Forms.CheckBox();
            this.vScrollBar2 = new System.Windows.Forms.VScrollBar();
            this.hScrollBar2 = new System.Windows.Forms.HScrollBar();
            this.spinNgHistoryNumber = new Fujita.InspectionSystem.uclNumericInput();
            this.spinImageDispNo = new Fujita.InspectionSystem.uclNumericInput();
            this.uclKandoControl2 = new LineCameraSheetSystem.uclKandoControl();
            this.uclKandoControl1 = new LineCameraSheetSystem.uclKandoControl();
            this.groupBox2.SuspendLayout();
            this.grpTitleOmote.SuspendLayout();
            this.grpTitleUra.SuspendLayout();
            this.SuspendLayout();
            // 
            // hwcImageWnd1
            // 
            this.hwcImageWnd1.BackColor = System.Drawing.Color.Black;
            this.hwcImageWnd1.BorderColor = System.Drawing.Color.Black;
            this.hwcImageWnd1.BorderWidth = 1;
            this.hwcImageWnd1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hwcImageWnd1.Location = new System.Drawing.Point(3, 40);
            this.hwcImageWnd1.Name = "hwcImageWnd1";
            this.hwcImageWnd1.Size = new System.Drawing.Size(738, 360);
            this.hwcImageWnd1.TabIndex = 30;
            this.hwcImageWnd1.WindowSize = new System.Drawing.Size(736, 358);
            this.hwcImageWnd1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.hwcImageWnd1_MouseUp);
            // 
            // hwcImageWnd2
            // 
            this.hwcImageWnd2.BackColor = System.Drawing.Color.Black;
            this.hwcImageWnd2.BorderColor = System.Drawing.Color.Black;
            this.hwcImageWnd2.BorderWidth = 1;
            this.hwcImageWnd2.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hwcImageWnd2.Location = new System.Drawing.Point(3, 442);
            this.hwcImageWnd2.Name = "hwcImageWnd2";
            this.hwcImageWnd2.Size = new System.Drawing.Size(738, 360);
            this.hwcImageWnd2.TabIndex = 30;
            this.hwcImageWnd2.WindowSize = new System.Drawing.Size(736, 358);
            this.hwcImageWnd2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.hwcImageWnd2_MouseUp);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(3, 805);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(738, 10);
            this.hScrollBar1.TabIndex = 34;
            this.hScrollBar1.Visible = false;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(745, 40);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(10, 769);
            this.vScrollBar1.TabIndex = 33;
            this.vScrollBar1.Visible = false;
            // 
            // cmbMagnify1
            // 
            this.cmbMagnify1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMagnify1.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbMagnify1.FormattingEnabled = true;
            this.cmbMagnify1.Location = new System.Drawing.Point(888, 71);
            this.cmbMagnify1.Name = "cmbMagnify1";
            this.cmbMagnify1.Size = new System.Drawing.Size(10, 35);
            this.cmbMagnify1.TabIndex = 35;
            this.cmbMagnify1.Visible = false;
            this.cmbMagnify1.SelectedIndexChanged += new System.EventHandler(this.cmbMagnify1_SelectedIndexChanged);
            // 
            // cmbMagnify2
            // 
            this.cmbMagnify2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMagnify2.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbMagnify2.FormattingEnabled = true;
            this.cmbMagnify2.Location = new System.Drawing.Point(904, 71);
            this.cmbMagnify2.Name = "cmbMagnify2";
            this.cmbMagnify2.Size = new System.Drawing.Size(10, 35);
            this.cmbMagnify2.TabIndex = 35;
            this.cmbMagnify2.Visible = false;
            this.cmbMagnify2.SelectedIndexChanged += new System.EventHandler(this.cmbMagnify2_SelectedIndexChanged);
            // 
            // lblKandoOmote
            // 
            this.lblKandoOmote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKandoOmote.AutoSize = true;
            this.lblKandoOmote.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblKandoOmote.Location = new System.Drawing.Point(46, 2);
            this.lblKandoOmote.Name = "lblKandoOmote";
            this.lblKandoOmote.Size = new System.Drawing.Size(144, 31);
            this.lblKandoOmote.TabIndex = 36;
            this.lblKandoOmote.Text = "明--- 暗---";
            this.lblKandoOmote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblKandoUra
            // 
            this.lblKandoUra.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKandoUra.Font = new System.Drawing.Font("ＭＳ ゴシック", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblKandoUra.Location = new System.Drawing.Point(46, 2);
            this.lblKandoUra.Name = "lblKandoUra";
            this.lblKandoUra.Size = new System.Drawing.Size(144, 31);
            this.lblKandoUra.TabIndex = 36;
            this.lblKandoUra.Text = "明--- 暗---";
            this.lblKandoUra.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTitileOmote
            // 
            this.lblTitileOmote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitileOmote.AutoSize = true;
            this.lblTitileOmote.Font = new System.Drawing.Font("ＭＳ ゴシック", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTitileOmote.Location = new System.Drawing.Point(5, 2);
            this.lblTitileOmote.Name = "lblTitileOmote";
            this.lblTitileOmote.Size = new System.Drawing.Size(35, 31);
            this.lblTitileOmote.TabIndex = 36;
            this.lblTitileOmote.Text = "表";
            this.lblTitileOmote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitleUra
            // 
            this.lblTitleUra.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitleUra.Font = new System.Drawing.Font("ＭＳ ゴシック", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTitleUra.Location = new System.Drawing.Point(5, 2);
            this.lblTitleUra.Name = "lblTitleUra";
            this.lblTitleUra.Size = new System.Drawing.Size(35, 31);
            this.lblTitleUra.TabIndex = 36;
            this.lblTitleUra.Text = "裏";
            this.lblTitleUra.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInspStatusOmote
            // 
            this.lblInspStatusOmote.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInspStatusOmote.AutoSize = true;
            this.lblInspStatusOmote.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInspStatusOmote.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblInspStatusOmote.Location = new System.Drawing.Point(196, 2);
            this.lblInspStatusOmote.Name = "lblInspStatusOmote";
            this.lblInspStatusOmote.Size = new System.Drawing.Size(159, 31);
            this.lblInspStatusOmote.TabIndex = 47;
            this.lblInspStatusOmote.Text = "表面検査有効";
            this.lblInspStatusOmote.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInspStatusUra
            // 
            this.lblInspStatusUra.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInspStatusUra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInspStatusUra.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblInspStatusUra.Location = new System.Drawing.Point(196, 2);
            this.lblInspStatusUra.Name = "lblInspStatusUra";
            this.lblInspStatusUra.Size = new System.Drawing.Size(159, 31);
            this.lblInspStatusUra.TabIndex = 47;
            this.lblInspStatusUra.Text = "裏検査有効";
            this.lblInspStatusUra.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkPause
            // 
            this.chkPause.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkPause.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkPause.Location = new System.Drawing.Point(954, 519);
            this.chkPause.Name = "chkPause";
            this.chkPause.Size = new System.Drawing.Size(172, 65);
            this.chkPause.TabIndex = 1;
            this.chkPause.Text = "表示停止";
            this.chkPause.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkPause.UseVisualStyleBackColor = true;
            this.chkPause.CheckedChanged += new System.EventHandler(this.chkPause_CheckedChanged);
            // 
            // chkVerDisplayMode
            // 
            this.chkVerDisplayMode.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkVerDisplayMode.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkVerDisplayMode.Location = new System.Drawing.Point(786, 640);
            this.chkVerDisplayMode.Name = "chkVerDisplayMode";
            this.chkVerDisplayMode.Size = new System.Drawing.Size(140, 65);
            this.chkVerDisplayMode.TabIndex = 33;
            this.chkVerDisplayMode.Text = "縦並び配置";
            this.chkVerDisplayMode.UseVisualStyleBackColor = true;
            this.chkVerDisplayMode.CheckedChanged += new System.EventHandler(this.chkVerDisplayMode_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkDisplayModeTarget);
            this.groupBox2.Controls.Add(this.chkDisplayModeBase);
            this.groupBox2.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox2.Location = new System.Drawing.Point(782, 508);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(147, 126);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "画像";
            // 
            // chkDisplayModeTarget
            // 
            this.chkDisplayModeTarget.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkDisplayModeTarget.Checked = true;
            this.chkDisplayModeTarget.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDisplayModeTarget.Location = new System.Drawing.Point(4, 30);
            this.chkDisplayModeTarget.Name = "chkDisplayModeTarget";
            this.chkDisplayModeTarget.Size = new System.Drawing.Size(68, 90);
            this.chkDisplayModeTarget.TabIndex = 0;
            this.chkDisplayModeTarget.Tag = "0";
            this.chkDisplayModeTarget.Text = "検査";
            this.chkDisplayModeTarget.UseVisualStyleBackColor = true;
            this.chkDisplayModeTarget.CheckedChanged += new System.EventHandler(this.chkDisplayMode_CheckedChanged);
            // 
            // chkDisplayModeBase
            // 
            this.chkDisplayModeBase.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkDisplayModeBase.Location = new System.Drawing.Point(76, 30);
            this.chkDisplayModeBase.Name = "chkDisplayModeBase";
            this.chkDisplayModeBase.Size = new System.Drawing.Size(68, 90);
            this.chkDisplayModeBase.TabIndex = 33;
            this.chkDisplayModeBase.Tag = "1";
            this.chkDisplayModeBase.Text = "取込";
            this.chkDisplayModeBase.UseVisualStyleBackColor = true;
            this.chkDisplayModeBase.CheckedChanged += new System.EventHandler(this.chkDisplayMode_CheckedChanged);
            // 
            // lblKandoMessage
            // 
            this.lblKandoMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblKandoMessage.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblKandoMessage.Location = new System.Drawing.Point(782, 3);
            this.lblKandoMessage.Name = "lblKandoMessage";
            this.lblKandoMessage.Size = new System.Drawing.Size(344, 50);
            this.lblKandoMessage.TabIndex = 49;
            this.lblKandoMessage.Text = "検査中の感度は、保存されていません";
            this.lblKandoMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnKandoSave
            // 
            this.btnKandoSave.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnKandoSave.Location = new System.Drawing.Point(1026, 56);
            this.btnKandoSave.Name = "btnKandoSave";
            this.btnKandoSave.Size = new System.Drawing.Size(100, 50);
            this.btnKandoSave.TabIndex = 50;
            this.btnKandoSave.Text = "保存";
            this.btnKandoSave.UseVisualStyleBackColor = true;
            this.btnKandoSave.Click += new System.EventHandler(this.btnKandoSave_Click);
            // 
            // btnKandoApplyCancel
            // 
            this.btnKandoApplyCancel.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnKandoApplyCancel.Location = new System.Drawing.Point(782, 56);
            this.btnKandoApplyCancel.Name = "btnKandoApplyCancel";
            this.btnKandoApplyCancel.Size = new System.Drawing.Size(100, 50);
            this.btnKandoApplyCancel.TabIndex = 50;
            this.btnKandoApplyCancel.Text = "元に戻す";
            this.btnKandoApplyCancel.UseVisualStyleBackColor = true;
            this.btnKandoApplyCancel.Click += new System.EventHandler(this.btnKandoApplyCancel_Click);
            // 
            // tmKandoMessage
            // 
            this.tmKandoMessage.Tick += new System.EventHandler(this.tmKandoMessage_Tick);
            // 
            // grpTitleOmote
            // 
            this.grpTitleOmote.ColumnCount = 3;
            this.grpTitleOmote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.grpTitleOmote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.grpTitleOmote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.grpTitleOmote.Controls.Add(this.lblTitileOmote, 0, 0);
            this.grpTitleOmote.Controls.Add(this.lblKandoOmote, 1, 0);
            this.grpTitleOmote.Controls.Add(this.lblInspStatusOmote, 2, 0);
            this.grpTitleOmote.Location = new System.Drawing.Point(3, 3);
            this.grpTitleOmote.Name = "grpTitleOmote";
            this.grpTitleOmote.Padding = new System.Windows.Forms.Padding(2);
            this.grpTitleOmote.RowCount = 1;
            this.grpTitleOmote.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.grpTitleOmote.Size = new System.Drawing.Size(360, 35);
            this.grpTitleOmote.TabIndex = 52;
            // 
            // grpTitleUra
            // 
            this.grpTitleUra.ColumnCount = 3;
            this.grpTitleUra.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.grpTitleUra.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.grpTitleUra.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.grpTitleUra.Controls.Add(this.lblTitleUra, 0, 0);
            this.grpTitleUra.Controls.Add(this.lblKandoUra, 1, 0);
            this.grpTitleUra.Controls.Add(this.lblInspStatusUra, 2, 0);
            this.grpTitleUra.Location = new System.Drawing.Point(3, 406);
            this.grpTitleUra.Name = "grpTitleUra";
            this.grpTitleUra.Padding = new System.Windows.Forms.Padding(2);
            this.grpTitleUra.RowCount = 1;
            this.grpTitleUra.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.grpTitleUra.Size = new System.Drawing.Size(360, 35);
            this.grpTitleUra.TabIndex = 53;
            // 
            // chkNgHistory
            // 
            this.chkNgHistory.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkNgHistory.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkNgHistory.Location = new System.Drawing.Point(954, 640);
            this.chkNgHistory.Name = "chkNgHistory";
            this.chkNgHistory.Size = new System.Drawing.Size(172, 65);
            this.chkNgHistory.TabIndex = 55;
            this.chkNgHistory.Text = "NG履歴";
            this.chkNgHistory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkNgHistory.UseVisualStyleBackColor = true;
            this.chkNgHistory.CheckedChanged += new System.EventHandler(this.chkNgHistory_CheckedChanged);
            // 
            // lblNgHistoryTime
            // 
            this.lblNgHistoryTime.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblNgHistoryTime.Location = new System.Drawing.Point(821, 736);
            this.lblNgHistoryTime.Name = "lblNgHistoryTime";
            this.lblNgHistoryTime.Size = new System.Drawing.Size(157, 22);
            this.lblNgHistoryTime.TabIndex = 56;
            this.lblNgHistoryTime.Text = "hh:mm:ss:fff";
            this.lblNgHistoryTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblNgHistorySide
            // 
            this.lblNgHistorySide.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblNgHistorySide.Location = new System.Drawing.Point(950, 708);
            this.lblNgHistorySide.Name = "lblNgHistorySide";
            this.lblNgHistorySide.Size = new System.Drawing.Size(28, 28);
            this.lblNgHistorySide.TabIndex = 57;
            this.lblNgHistorySide.Text = "表";
            this.lblNgHistorySide.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkZoomEnable
            // 
            this.chkZoomEnable.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkZoomEnable.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkZoomEnable.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkZoomEnable.Location = new System.Drawing.Point(575, 0);
            this.chkZoomEnable.Name = "chkZoomEnable";
            this.chkZoomEnable.Size = new System.Drawing.Size(80, 38);
            this.chkZoomEnable.TabIndex = 58;
            this.chkZoomEnable.Text = "Zoom";
            this.chkZoomEnable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkZoomEnable.UseVisualStyleBackColor = true;
            this.chkZoomEnable.CheckedChanged += new System.EventHandler(this.chkZoomEnable_CheckedChanged);
            // 
            // chkMoveEnable
            // 
            this.chkMoveEnable.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkMoveEnable.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkMoveEnable.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkMoveEnable.Location = new System.Drawing.Point(661, 0);
            this.chkMoveEnable.Name = "chkMoveEnable";
            this.chkMoveEnable.Size = new System.Drawing.Size(80, 38);
            this.chkMoveEnable.TabIndex = 59;
            this.chkMoveEnable.Text = "Move";
            this.chkMoveEnable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkMoveEnable.UseVisualStyleBackColor = true;
            this.chkMoveEnable.CheckedChanged += new System.EventHandler(this.chkMoveEnable_CheckedChanged);
            // 
            // vScrollBar2
            // 
            this.vScrollBar2.Location = new System.Drawing.Point(755, 40);
            this.vScrollBar2.Name = "vScrollBar2";
            this.vScrollBar2.Size = new System.Drawing.Size(10, 769);
            this.vScrollBar2.TabIndex = 60;
            this.vScrollBar2.Visible = false;
            // 
            // hScrollBar2
            // 
            this.hScrollBar2.Location = new System.Drawing.Point(3, 815);
            this.hScrollBar2.Name = "hScrollBar2";
            this.hScrollBar2.Size = new System.Drawing.Size(738, 10);
            this.hScrollBar2.TabIndex = 34;
            this.hScrollBar2.Visible = false;
            // 
            // spinNgHistoryNumber
            // 
            this.spinNgHistoryNumber.DecimalPlaces = 0;
            this.spinNgHistoryNumber.EveryValueChanged = false;
            this.spinNgHistoryNumber.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinNgHistoryNumber.Location = new System.Drawing.Point(981, 708);
            this.spinNgHistoryNumber.Margin = new System.Windows.Forms.Padding(0);
            this.spinNgHistoryNumber.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.spinNgHistoryNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinNgHistoryNumber.MinimumSize = new System.Drawing.Size(0, 50);
            this.spinNgHistoryNumber.Name = "spinNgHistoryNumber";
            this.spinNgHistoryNumber.Size = new System.Drawing.Size(148, 50);
            this.spinNgHistoryNumber.TabIndex = 54;
            this.spinNgHistoryNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinNgHistoryNumber.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spinNgHistoryNumber.ValueChanged += new Fujita.InspectionSystem.ValueChangeEventHandler(this.spinNgHistoryNumber_ValueChanged);
            // 
            // spinImageDispNo
            // 
            this.spinImageDispNo.DecimalPlaces = 0;
            this.spinImageDispNo.EveryValueChanged = false;
            this.spinImageDispNo.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinImageDispNo.Location = new System.Drawing.Point(981, 587);
            this.spinImageDispNo.Margin = new System.Windows.Forms.Padding(0);
            this.spinImageDispNo.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.spinImageDispNo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinImageDispNo.MinimumSize = new System.Drawing.Size(0, 50);
            this.spinImageDispNo.Name = "spinImageDispNo";
            this.spinImageDispNo.Size = new System.Drawing.Size(148, 50);
            this.spinImageDispNo.TabIndex = 54;
            this.spinImageDispNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinImageDispNo.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spinImageDispNo.ValueChanged += new Fujita.InspectionSystem.ValueChangeEventHandler(this.spinImageDispNo_ValueChanged);
            // 
            // uclKandoControl2
            // 
            this.uclKandoControl2.EnableDark = true;
            this.uclKandoControl2.EnableLight = true;
            this.uclKandoControl2.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.uclKandoControl2.Location = new System.Drawing.Point(778, 319);
            this.uclKandoControl2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.uclKandoControl2.Name = "uclKandoControl2";
            this.uclKandoControl2.Size = new System.Drawing.Size(352, 192);
            this.uclKandoControl2.TabIndex = 51;
            this.uclKandoControl2.TitleName = "裏";
            this.uclKandoControl2.KandoValueChanged += new LineCameraSheetSystem.uclKandoControl.KandoValueEventHandler(this.uclKandoControl_KandoValueChanged);
            // 
            // uclKandoControl1
            // 
            this.uclKandoControl1.EnableDark = true;
            this.uclKandoControl1.EnableLight = true;
            this.uclKandoControl1.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.uclKandoControl1.Location = new System.Drawing.Point(778, 115);
            this.uclKandoControl1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.uclKandoControl1.Name = "uclKandoControl1";
            this.uclKandoControl1.Size = new System.Drawing.Size(352, 192);
            this.uclKandoControl1.TabIndex = 51;
            this.uclKandoControl1.TitleName = "表";
            this.uclKandoControl1.KandoValueChanged += new LineCameraSheetSystem.uclKandoControl.KandoValueEventHandler(this.uclKandoControl_KandoValueChanged);
            // 
            // uclImageMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vScrollBar2);
            this.Controls.Add(this.chkMoveEnable);
            this.Controls.Add(this.chkZoomEnable);
            this.Controls.Add(this.lblNgHistoryTime);
            this.Controls.Add(this.lblNgHistorySide);
            this.Controls.Add(this.chkNgHistory);
            this.Controls.Add(this.spinNgHistoryNumber);
            this.Controls.Add(this.spinImageDispNo);
            this.Controls.Add(this.chkPause);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.chkVerDisplayMode);
            this.Controls.Add(this.grpTitleUra);
            this.Controls.Add(this.grpTitleOmote);
            this.Controls.Add(this.uclKandoControl2);
            this.Controls.Add(this.uclKandoControl1);
            this.Controls.Add(this.btnKandoSave);
            this.Controls.Add(this.btnKandoApplyCancel);
            this.Controls.Add(this.lblKandoMessage);
            this.Controls.Add(this.cmbMagnify2);
            this.Controls.Add(this.cmbMagnify1);
            this.Controls.Add(this.hScrollBar2);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.hwcImageWnd2);
            this.Controls.Add(this.hwcImageWnd1);
            this.Name = "uclImageMain";
            this.Size = new System.Drawing.Size(1135, 842);
            this.Load += new System.EventHandler(this.uclImageMain_Load);
            this.VisibleChanged += new System.EventHandler(this.uclImageMain_VisibleChanged);
            this.DoubleClick += new System.EventHandler(this.uclImageMain_DoubleClick);
            this.groupBox2.ResumeLayout(false);
            this.grpTitleOmote.ResumeLayout(false);
            this.grpTitleOmote.PerformLayout();
            this.grpTitleUra.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HalconDotNet.HWindowControl hwcImageWnd1;
        private HalconDotNet.HWindowControl hwcImageWnd2;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.ComboBox cmbMagnify1;
        private System.Windows.Forms.ComboBox cmbMagnify2;
        private System.Windows.Forms.Label lblKandoOmote;
        private System.Windows.Forms.Label lblKandoUra;
        private System.Windows.Forms.Label lblTitileOmote;
        private System.Windows.Forms.Label lblTitleUra;
        private System.Windows.Forms.Label lblInspStatusOmote;
        private System.Windows.Forms.Label lblInspStatusUra;
        private System.Windows.Forms.CheckBox chkPause;
        private System.Windows.Forms.CheckBox chkDisplayModeTarget;
        private System.Windows.Forms.Label lblKandoMessage;
        private System.Windows.Forms.Button btnKandoSave;
        private uclKandoControl uclKandoControl1;
        private uclKandoControl uclKandoControl2;
        private System.Windows.Forms.Button btnKandoApplyCancel;
        private System.Windows.Forms.CheckBox chkDisplayModeBase;
        private System.Windows.Forms.Timer tmKandoMessage;
        private System.Windows.Forms.TableLayoutPanel grpTitleOmote;
        private System.Windows.Forms.TableLayoutPanel grpTitleUra;
        private System.Windows.Forms.CheckBox chkVerDisplayMode;
        private System.Windows.Forms.GroupBox groupBox2;
        public Fujita.InspectionSystem.uclNumericInput spinImageDispNo;
        private System.Windows.Forms.CheckBox chkNgHistory;
        public Fujita.InspectionSystem.uclNumericInput spinNgHistoryNumber;
        private System.Windows.Forms.Label lblNgHistoryTime;
        private System.Windows.Forms.Label lblNgHistorySide;
        private System.Windows.Forms.CheckBox chkZoomEnable;
        private System.Windows.Forms.CheckBox chkMoveEnable;
        private System.Windows.Forms.VScrollBar vScrollBar2;
        private System.Windows.Forms.HScrollBar hScrollBar2;
    }
}
