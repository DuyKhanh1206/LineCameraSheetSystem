namespace LineCameraSheetSystem
{
    partial class frmNg1Image
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.hwndctrlImage = new HalconDotNet.HWindowControl();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.chkEnableGraph = new System.Windows.Forms.CheckBox();
            this.picMap = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblOmoteUra = new System.Windows.Forms.Label();
            this.lblVertical = new System.Windows.Forms.Label();
            this.lblHorizontal = new System.Windows.Forms.Label();
            this.lblDateTime = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblLineNo = new System.Windows.Forms.Label();
            this.chkEnableBaseImage = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMap)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hwndctrlImage
            // 
            this.hwndctrlImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hwndctrlImage.BackColor = System.Drawing.Color.Black;
            this.hwndctrlImage.BorderColor = System.Drawing.Color.Black;
            this.hwndctrlImage.ImagePart = new System.Drawing.Rectangle(0, 0, 300, 300);
            this.hwndctrlImage.Location = new System.Drawing.Point(5, 55);
            this.hwndctrlImage.Name = "hwndctrlImage";
            this.hwndctrlImage.Size = new System.Drawing.Size(500, 500);
            this.hwndctrlImage.TabIndex = 2;
            this.hwndctrlImage.WindowSize = new System.Drawing.Size(500, 500);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ActiveBorder;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Enabled = false;
            this.dataGridView1.Location = new System.Drawing.Point(5, 722);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView1.Size = new System.Drawing.Size(16, 22);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.Visible = false;
            // 
            // Column1
            // 
            this.Column1.FillWeight = 10F;
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 200;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 200;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnClose.Location = new System.Drawing.Point(197, 663);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(146, 80);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkEnableGraph
            // 
            this.chkEnableGraph.AutoSize = true;
            this.chkEnableGraph.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkEnableGraph.Location = new System.Drawing.Point(5, 663);
            this.chkEnableGraph.Name = "chkEnableGraph";
            this.chkEnableGraph.Size = new System.Drawing.Size(148, 23);
            this.chkEnableGraph.TabIndex = 5;
            this.chkEnableGraph.Text = "波形を表示する";
            this.chkEnableGraph.UseVisualStyleBackColor = true;
            this.chkEnableGraph.CheckedChanged += new System.EventHandler(this.chkEnableGraph_CheckedChanged);
            // 
            // picMap
            // 
            this.picMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picMap.Location = new System.Drawing.Point(5, 5);
            this.picMap.Name = "picMap";
            this.picMap.Size = new System.Drawing.Size(500, 50);
            this.picMap.TabIndex = 6;
            this.picMap.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblOmoteUra);
            this.groupBox1.Controls.Add(this.lblVertical);
            this.groupBox1.Controls.Add(this.lblHorizontal);
            this.groupBox1.Controls.Add(this.lblDateTime);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(5, 557);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(500, 100);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            // 
            // lblOmoteUra
            // 
            this.lblOmoteUra.BackColor = System.Drawing.Color.White;
            this.lblOmoteUra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblOmoteUra.Font = new System.Drawing.Font("ＭＳ ゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblOmoteUra.Location = new System.Drawing.Point(103, 15);
            this.lblOmoteUra.Name = "lblOmoteUra";
            this.lblOmoteUra.Size = new System.Drawing.Size(240, 40);
            this.lblOmoteUra.TabIndex = 0;
            this.lblOmoteUra.Text = "表";
            this.lblOmoteUra.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVertical
            // 
            this.lblVertical.BackColor = System.Drawing.Color.White;
            this.lblVertical.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblVertical.Font = new System.Drawing.Font("ＭＳ ゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblVertical.Location = new System.Drawing.Point(418, 15);
            this.lblVertical.Name = "lblVertical";
            this.lblVertical.Size = new System.Drawing.Size(75, 40);
            this.lblVertical.TabIndex = 0;
            this.lblVertical.Text = "999.9";
            this.lblVertical.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHorizontal
            // 
            this.lblHorizontal.BackColor = System.Drawing.Color.White;
            this.lblHorizontal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHorizontal.Font = new System.Drawing.Font("ＭＳ ゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblHorizontal.Location = new System.Drawing.Point(418, 54);
            this.lblHorizontal.Name = "lblHorizontal";
            this.lblHorizontal.Size = new System.Drawing.Size(75, 40);
            this.lblHorizontal.TabIndex = 0;
            this.lblHorizontal.Text = "999.9";
            this.lblHorizontal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDateTime
            // 
            this.lblDateTime.BackColor = System.Drawing.Color.White;
            this.lblDateTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDateTime.Font = new System.Drawing.Font("ＭＳ ゴシック", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDateTime.Location = new System.Drawing.Point(103, 54);
            this.lblDateTime.Name = "lblDateTime";
            this.lblDateTime.Size = new System.Drawing.Size(240, 40);
            this.lblDateTime.TabIndex = 0;
            this.lblDateTime.Text = "yyyy/mm/dd hh:mm:ss";
            this.lblDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("ＭＳ ゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(4, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 40);
            this.label2.TabIndex = 0;
            this.label2.Text = "発生時間";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(343, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 40);
            this.label4.TabIndex = 0;
            this.label4.Text = "横[㎜]";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(343, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 40);
            this.label3.TabIndex = 0;
            this.label3.Text = "縦[㎜]";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("ＭＳ ゴシック", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(4, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "面";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPrev
            // 
            this.btnPrev.Font = new System.Drawing.Font("MS UI Gothic", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnPrev.Location = new System.Drawing.Point(349, 663);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(75, 55);
            this.btnPrev.TabIndex = 8;
            this.btnPrev.Text = "◀";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("MS UI Gothic", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnNext.Location = new System.Drawing.Point(430, 663);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 55);
            this.btnNext.TabIndex = 8;
            this.btnNext.Text = "▶";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblLineNo
            // 
            this.lblLineNo.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblLineNo.Location = new System.Drawing.Point(406, 721);
            this.lblLineNo.Name = "lblLineNo";
            this.lblLineNo.Size = new System.Drawing.Size(99, 27);
            this.lblLineNo.TabIndex = 9;
            this.lblLineNo.Text = "No. 000";
            this.lblLineNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkEnableBaseImage
            // 
            this.chkEnableBaseImage.AutoSize = true;
            this.chkEnableBaseImage.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkEnableBaseImage.Location = new System.Drawing.Point(5, 693);
            this.chkEnableBaseImage.Name = "chkEnableBaseImage";
            this.chkEnableBaseImage.Size = new System.Drawing.Size(186, 23);
            this.chkEnableBaseImage.TabIndex = 5;
            this.chkEnableBaseImage.Text = "取込画像を表示する";
            this.chkEnableBaseImage.UseVisualStyleBackColor = true;
            this.chkEnableBaseImage.CheckedChanged += new System.EventHandler(this.chkEnableBaseImage_CheckedChanged);
            // 
            // frmNg1Image
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(509, 751);
            this.ControlBox = false;
            this.Controls.Add(this.lblLineNo);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrev);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.picMap);
            this.Controls.Add(this.chkEnableBaseImage);
            this.Controls.Add(this.chkEnableGraph);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.hwndctrlImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNg1Image";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "欠点画像";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmNg1Image_FormClosed);
            this.Load += new System.EventHandler(this.frmNg1Image_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmNg1Image_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMap)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HalconDotNet.HWindowControl hwndctrlImage;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkEnableGraph;
        private System.Windows.Forms.PictureBox picMap;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblOmoteUra;
        private System.Windows.Forms.Label lblVertical;
        private System.Windows.Forms.Label lblHorizontal;
        private System.Windows.Forms.Label lblDateTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblLineNo;
        private System.Windows.Forms.CheckBox chkEnableBaseImage;
    }
}