namespace LineCameraSheetSystem
{
    partial class uclNgThumbnail
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.dgvOnOffZone = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvOnOffKind = new System.Windows.Forms.DataGridView();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvOnOffSide = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelPageNow = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelPageMax = new System.Windows.Forms.Label();
            this.btnLast = new System.Windows.Forms.Button();
            this.btnFirst = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.uclMiniImage6 = new LineCameraSheetSystem.uclMiniImage();
            this.uclMiniImage5 = new LineCameraSheetSystem.uclMiniImage();
            this.uclMiniImage3 = new LineCameraSheetSystem.uclMiniImage();
            this.uclMiniImage4 = new LineCameraSheetSystem.uclMiniImage();
            this.uclMiniImage2 = new LineCameraSheetSystem.uclMiniImage();
            this.uclMiniImage1 = new LineCameraSheetSystem.uclMiniImage();
            this.shortcutKeyHelper1 = new Extension.ShortcutKeyHelper(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOnOffZone)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOnOffKind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOnOffSide)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBack.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnBack.Location = new System.Drawing.Point(109, 787);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(120, 60);
            this.btnBack.TabIndex = 9;
            this.btnBack.Text = "前の6件(B)";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNext.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnNext.Location = new System.Drawing.Point(372, 787);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(120, 60);
            this.btnNext.TabIndex = 10;
            this.btnNext.Text = "次の6件(N)";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // dgvOnOffZone
            // 
            this.dgvOnOffZone.AllowUserToAddRows = false;
            this.dgvOnOffZone.AllowUserToDeleteRows = false;
            this.dgvOnOffZone.AllowUserToResizeColumns = false;
            this.dgvOnOffZone.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            this.dgvOnOffZone.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOnOffZone.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvOnOffZone.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOnOffZone.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOnOffZone.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvOnOffZone.Location = new System.Drawing.Point(449, 3);
            this.dgvOnOffZone.MultiSelect = false;
            this.dgvOnOffZone.Name = "dgvOnOffZone";
            this.dgvOnOffZone.ReadOnly = true;
            this.dgvOnOffZone.RowHeadersVisible = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            this.dgvOnOffZone.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvOnOffZone.RowTemplate.Height = 40;
            this.dgvOnOffZone.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvOnOffZone.Size = new System.Drawing.Size(133, 343);
            this.dgvOnOffZone.TabIndex = 6;
            this.dgvOnOffZone.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOnOffZone_CellContentClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 65;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "表示";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 65;
            // 
            // dgvOnOffKind
            // 
            this.dgvOnOffKind.AllowUserToAddRows = false;
            this.dgvOnOffKind.AllowUserToDeleteRows = false;
            this.dgvOnOffKind.AllowUserToResizeColumns = false;
            this.dgvOnOffKind.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            this.dgvOnOffKind.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOnOffKind.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvOnOffKind.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOnOffKind.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5,
            this.Column6});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOnOffKind.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvOnOffKind.Location = new System.Drawing.Point(449, 461);
            this.dgvOnOffKind.MultiSelect = false;
            this.dgvOnOffKind.Name = "dgvOnOffKind";
            this.dgvOnOffKind.ReadOnly = true;
            this.dgvOnOffKind.RowHeadersVisible = false;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            this.dgvOnOffKind.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvOnOffKind.RowTemplate.Height = 40;
            this.dgvOnOffKind.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvOnOffKind.Size = new System.Drawing.Size(133, 263);
            this.dgvOnOffKind.TabIndex = 8;
            this.dgvOnOffKind.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOnOffKind_CellContentClick);
            // 
            // Column5
            // 
            this.Column5.HeaderText = "";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Width = 65;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "表示";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Width = 65;
            // 
            // dgvOnOffSide
            // 
            this.dgvOnOffSide.AllowUserToAddRows = false;
            this.dgvOnOffSide.AllowUserToDeleteRows = false;
            this.dgvOnOffSide.AllowUserToResizeColumns = false;
            this.dgvOnOffSide.AllowUserToResizeRows = false;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
            this.dgvOnOffSide.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOnOffSide.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvOnOffSide.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOnOffSide.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column4});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOnOffSide.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgvOnOffSide.Location = new System.Drawing.Point(449, 352);
            this.dgvOnOffSide.MultiSelect = false;
            this.dgvOnOffSide.Name = "dgvOnOffSide";
            this.dgvOnOffSide.ReadOnly = true;
            this.dgvOnOffSide.RowHeadersVisible = false;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            this.dgvOnOffSide.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvOnOffSide.RowTemplate.Height = 40;
            this.dgvOnOffSide.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgvOnOffSide.Size = new System.Drawing.Size(133, 103);
            this.dgvOnOffSide.TabIndex = 7;
            this.dgvOnOffSide.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOnOffSide_CellContentClick);
            // 
            // Column3
            // 
            this.Column3.HeaderText = "";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 65;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "表示";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 65;
            // 
            // labelPageNow
            // 
            this.labelPageNow.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPageNow.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelPageNow.Location = new System.Drawing.Point(5, 5);
            this.labelPageNow.Margin = new System.Windows.Forms.Padding(5);
            this.labelPageNow.Name = "labelPageNow";
            this.labelPageNow.Size = new System.Drawing.Size(45, 50);
            this.labelPageNow.TabIndex = 13;
            this.labelPageNow.Text = "9999";
            this.labelPageNow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(58, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 54);
            this.label1.TabIndex = 14;
            this.label1.Text = "/";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPageMax
            // 
            this.labelPageMax.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPageMax.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelPageMax.Location = new System.Drawing.Point(80, 5);
            this.labelPageMax.Margin = new System.Windows.Forms.Padding(5);
            this.labelPageMax.Name = "labelPageMax";
            this.labelPageMax.Size = new System.Drawing.Size(46, 50);
            this.labelPageMax.TabIndex = 15;
            this.labelPageMax.Text = "9999";
            this.labelPageMax.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnLast
            // 
            this.btnLast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLast.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnLast.Location = new System.Drawing.Point(507, 787);
            this.btnLast.Name = "btnLast";
            this.btnLast.Size = new System.Drawing.Size(90, 60);
            this.btnLast.TabIndex = 12;
            this.btnLast.Text = "最後(E)";
            this.btnLast.UseVisualStyleBackColor = true;
            // 
            // btnFirst
            // 
            this.btnFirst.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFirst.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnFirst.Location = new System.Drawing.Point(3, 787);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.Size = new System.Drawing.Size(90, 60);
            this.btnFirst.TabIndex = 11;
            this.btnFirst.Text = "最初(T)";
            this.btnFirst.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.labelPageNow, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelPageMax, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(235, 787);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(131, 60);
            this.tableLayoutPanel1.TabIndex = 16;
            // 
            // uclMiniImage6
            // 
            this.uclMiniImage6._miniImgResAcData = null;
            this.uclMiniImage6.BackColor = System.Drawing.SystemColors.Control;
            this.uclMiniImage6.CountOwn = 0;
            this.uclMiniImage6.LineIndex = 0;
            this.uclMiniImage6.Location = new System.Drawing.Point(230, 503);
            this.uclMiniImage6.Name = "uclMiniImage6";
            this.uclMiniImage6.SelectColor = System.Drawing.Color.Blue;
            this.uclMiniImage6.SelectItem = false;
            this.uclMiniImage6.Size = new System.Drawing.Size(186, 244);
            this.uclMiniImage6.TabIndex = 5;
            this.uclMiniImage6.TextData = "欠点種：　　サイズ：";
            this.uclMiniImage6.TextLength = "流：　　m  幅：　　mm";
            this.uclMiniImage6.TextSpot = "系列：　　Zone：";
            // 
            // uclMiniImage5
            // 
            this.uclMiniImage5._miniImgResAcData = null;
            this.uclMiniImage5.BackColor = System.Drawing.SystemColors.Control;
            this.uclMiniImage5.CountOwn = 0;
            this.uclMiniImage5.LineIndex = 0;
            this.uclMiniImage5.Location = new System.Drawing.Point(3, 503);
            this.uclMiniImage5.Name = "uclMiniImage5";
            this.uclMiniImage5.SelectColor = System.Drawing.Color.Blue;
            this.uclMiniImage5.SelectItem = false;
            this.uclMiniImage5.Size = new System.Drawing.Size(186, 244);
            this.uclMiniImage5.TabIndex = 4;
            this.uclMiniImage5.TextData = "欠点種：　　サイズ：";
            this.uclMiniImage5.TextLength = "流：　　m  幅：　　mm";
            this.uclMiniImage5.TextSpot = "系列：　　Zone：";
            // 
            // uclMiniImage3
            // 
            this.uclMiniImage3._miniImgResAcData = null;
            this.uclMiniImage3.BackColor = System.Drawing.SystemColors.Control;
            this.uclMiniImage3.CountOwn = 0;
            this.uclMiniImage3.LineIndex = 0;
            this.uclMiniImage3.Location = new System.Drawing.Point(3, 253);
            this.uclMiniImage3.Name = "uclMiniImage3";
            this.uclMiniImage3.SelectColor = System.Drawing.Color.Blue;
            this.uclMiniImage3.SelectItem = false;
            this.uclMiniImage3.Size = new System.Drawing.Size(186, 244);
            this.uclMiniImage3.TabIndex = 3;
            this.uclMiniImage3.TextData = "欠点種：　　サイズ：";
            this.uclMiniImage3.TextLength = "流：　　m  幅：　　mm";
            this.uclMiniImage3.TextSpot = "系列：　　Zone：";
            // 
            // uclMiniImage4
            // 
            this.uclMiniImage4._miniImgResAcData = null;
            this.uclMiniImage4.BackColor = System.Drawing.SystemColors.Control;
            this.uclMiniImage4.CountOwn = 0;
            this.uclMiniImage4.LineIndex = 0;
            this.uclMiniImage4.Location = new System.Drawing.Point(230, 253);
            this.uclMiniImage4.Name = "uclMiniImage4";
            this.uclMiniImage4.SelectColor = System.Drawing.Color.Blue;
            this.uclMiniImage4.SelectItem = false;
            this.uclMiniImage4.Size = new System.Drawing.Size(186, 244);
            this.uclMiniImage4.TabIndex = 3;
            this.uclMiniImage4.TextData = "欠点種：　　サイズ：";
            this.uclMiniImage4.TextLength = "流：　　m  幅：　　mm";
            this.uclMiniImage4.TextSpot = "系列：　　Zone：";
            // 
            // uclMiniImage2
            // 
            this.uclMiniImage2._miniImgResAcData = null;
            this.uclMiniImage2.BackColor = System.Drawing.SystemColors.Control;
            this.uclMiniImage2.CountOwn = 0;
            this.uclMiniImage2.LineIndex = 0;
            this.uclMiniImage2.Location = new System.Drawing.Point(230, 3);
            this.uclMiniImage2.Name = "uclMiniImage2";
            this.uclMiniImage2.SelectColor = System.Drawing.Color.Blue;
            this.uclMiniImage2.SelectItem = false;
            this.uclMiniImage2.Size = new System.Drawing.Size(186, 244);
            this.uclMiniImage2.TabIndex = 2;
            this.uclMiniImage2.TextData = "欠点種：　　サイズ：";
            this.uclMiniImage2.TextLength = "流：　　m  幅：　　mm";
            this.uclMiniImage2.TextSpot = "系列：　　Zone：";
            // 
            // uclMiniImage1
            // 
            this.uclMiniImage1._miniImgResAcData = null;
            this.uclMiniImage1.BackColor = System.Drawing.SystemColors.Control;
            this.uclMiniImage1.CountOwn = 0;
            this.uclMiniImage1.LineIndex = 0;
            this.uclMiniImage1.Location = new System.Drawing.Point(3, 3);
            this.uclMiniImage1.Name = "uclMiniImage1";
            this.uclMiniImage1.SelectColor = System.Drawing.Color.Blue;
            this.uclMiniImage1.SelectItem = false;
            this.uclMiniImage1.Size = new System.Drawing.Size(186, 244);
            this.uclMiniImage1.TabIndex = 5;
            this.uclMiniImage1.TextData = "欠点種：　　サイズ：";
            this.uclMiniImage1.TextLength = "流：　　m  幅：　　mm";
            this.uclMiniImage1.TextSpot = "系列：　　Zone：";
            this.uclMiniImage1.DoubleClick += new System.EventHandler(this.uclMiniImage1_DoubleClick);
            // 
            // uclNgThumbnail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnLast);
            this.Controls.Add(this.btnFirst);
            this.Controls.Add(this.dgvOnOffSide);
            this.Controls.Add(this.dgvOnOffKind);
            this.Controls.Add(this.dgvOnOffZone);
            this.Controls.Add(this.uclMiniImage6);
            this.Controls.Add(this.uclMiniImage5);
            this.Controls.Add(this.uclMiniImage3);
            this.Controls.Add(this.uclMiniImage4);
            this.Controls.Add(this.uclMiniImage2);
            this.Controls.Add(this.uclMiniImage1);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnBack);
            this.Name = "uclNgThumbnail";
            this.Size = new System.Drawing.Size(600, 850);
            this.Load += new System.EventHandler(this.uclNgThumbnail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOnOffZone)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOnOffKind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOnOffSide)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvOnOffZone;
        private System.Windows.Forms.DataGridView dgvOnOffKind;
        private System.Windows.Forms.DataGridView dgvOnOffSide;
        private System.Windows.Forms.Label labelPageNow;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelPageMax;
        public uclMiniImage uclMiniImage1;
        public uclMiniImage uclMiniImage2;
        public uclMiniImage uclMiniImage3;
        public uclMiniImage uclMiniImage4;
        public uclMiniImage uclMiniImage5;
        public uclMiniImage uclMiniImage6;
        public System.Windows.Forms.Button btnLast;
        public System.Windows.Forms.Button btnFirst;
        private Extension.ShortcutKeyHelper shortcutKeyHelper1;
        public System.Windows.Forms.Button btnBack;
        public System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
