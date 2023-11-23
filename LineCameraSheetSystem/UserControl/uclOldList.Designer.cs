namespace LineCameraSheetSystem
{
    partial class uclOldList
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnOutResult = new System.Windows.Forms.Button();
            this.chkMonth = new System.Windows.Forms.CheckBox();
            this.chkHinsyu = new System.Windows.Forms.CheckBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.chkMultiSelect = new System.Windows.Forms.CheckBox();
            this.btnListUp = new System.Windows.Forms.Button();
            this.btnListDown = new System.Windows.Forms.Button();
            this.dgvOldList = new LineCameraSheetSystem.DataGridViewMulti();
            this.shortcutKeyHelper1 = new Extension.ShortcutKeyHelper(this.components);
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOldList)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnOpen.Location = new System.Drawing.Point(582, 755);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(120, 72);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "開く(O)";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDelete.Location = new System.Drawing.Point(993, 755);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(120, 72);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "削除(D)";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnOutResult
            // 
            this.btnOutResult.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.btnOutResult.Location = new System.Drawing.Point(226, 755);
            this.btnOutResult.Name = "btnOutResult";
            this.btnOutResult.Size = new System.Drawing.Size(120, 72);
            this.btnOutResult.TabIndex = 3;
            this.btnOutResult.Text = "出力(U)";
            this.btnOutResult.UseVisualStyleBackColor = true;
            this.btnOutResult.Click += new System.EventHandler(this.btnOutResult_Click);
            // 
            // chkMonth
            // 
            this.chkMonth.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkMonth.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.chkMonth.Location = new System.Drawing.Point(12, 755);
            this.chkMonth.Name = "chkMonth";
            this.chkMonth.Size = new System.Drawing.Size(100, 35);
            this.chkMonth.TabIndex = 4;
            this.chkMonth.Text = "年月選択";
            this.chkMonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkMonth.UseVisualStyleBackColor = true;
            this.chkMonth.CheckedChanged += new System.EventHandler(this.chkMonth_CheckedChanged);
            // 
            // chkHinsyu
            // 
            this.chkHinsyu.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkHinsyu.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.chkHinsyu.Location = new System.Drawing.Point(12, 792);
            this.chkHinsyu.Name = "chkHinsyu";
            this.chkHinsyu.Size = new System.Drawing.Size(100, 35);
            this.chkHinsyu.TabIndex = 4;
            this.chkHinsyu.Text = "品種選択";
            this.chkHinsyu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkHinsyu.UseVisualStyleBackColor = true;
            this.chkHinsyu.CheckedChanged += new System.EventHandler(this.chkHinsyu_CheckedChanged);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.btnSelectAll.Location = new System.Drawing.Point(120, 792);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(100, 35);
            this.btnSelectAll.TabIndex = 6;
            this.btnSelectAll.Text = "全選択";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // chkMultiSelect
            // 
            this.chkMultiSelect.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkMultiSelect.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            this.chkMultiSelect.Location = new System.Drawing.Point(120, 755);
            this.chkMultiSelect.Name = "chkMultiSelect";
            this.chkMultiSelect.Size = new System.Drawing.Size(100, 35);
            this.chkMultiSelect.TabIndex = 7;
            this.chkMultiSelect.Text = "複数選択";
            this.chkMultiSelect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkMultiSelect.UseVisualStyleBackColor = true;
            this.chkMultiSelect.CheckedChanged += new System.EventHandler(this.chkMultiSelect_CheckedChanged);
            // 
            // btnListUp
            // 
            this.btnListUp.Font = new System.Drawing.Font("MS UI Gothic", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnListUp.Location = new System.Drawing.Point(1027, -1);
            this.btnListUp.Name = "btnListUp";
            this.btnListUp.Size = new System.Drawing.Size(100, 60);
            this.btnListUp.TabIndex = 8;
            this.btnListUp.Text = "▲";
            this.btnListUp.UseVisualStyleBackColor = true;
            this.btnListUp.Click += new System.EventHandler(this.btnListUp_Click);
            // 
            // btnListDown
            // 
            this.btnListDown.Font = new System.Drawing.Font("MS UI Gothic", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnListDown.Location = new System.Drawing.Point(921, -1);
            this.btnListDown.Name = "btnListDown";
            this.btnListDown.Size = new System.Drawing.Size(100, 60);
            this.btnListDown.TabIndex = 9;
            this.btnListDown.Text = "▼";
            this.btnListDown.UseVisualStyleBackColor = true;
            this.btnListDown.Click += new System.EventHandler(this.btnListDown_Click);
            // 
            // dgvOldList
            // 
            this.dgvOldList.AllowUserToAddRows = false;
            this.dgvOldList.AllowUserToDeleteRows = false;
            this.dgvOldList.AllowUserToResizeColumns = false;
            this.dgvOldList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOldList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvOldList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOldList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column5,
            this.Column6,
            this.Column3,
            this.Column4,
            this.Column7,
            this.Column8});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvOldList.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvOldList.Location = new System.Drawing.Point(0, 65);
            this.dgvOldList.Name = "dgvOldList";
            this.dgvOldList.NoUpdate = false;
            this.dgvOldList.ReadOnly = true;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOldList.RowHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvOldList.RowHeadersVisible = false;
            this.dgvOldList.RowTemplate.Height = 21;
            this.dgvOldList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvOldList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOldList.Size = new System.Drawing.Size(1126, 684);
            this.dgvOldList.TabIndex = 0;
            this.dgvOldList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOldList_CellClick);
            this.dgvOldList.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvOldList_CellMouseDown);
            this.dgvOldList.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvOldList_CellMouseUp);
            this.dgvOldList.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvOldList_CellValueNeeded);
            this.dgvOldList.VisibleChanged += new System.EventHandler(this.dgvOldList_VisibleChanged);
            this.dgvOldList.DoubleClick += new System.EventHandler(this.dgvOldList_DoubleClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "番号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 65;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "開始時刻";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 225;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "品種名";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Width = 255;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "ロットNo.";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Width = 160;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "終了時刻";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 225;
            // 
            // Column4
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column4.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column4.HeaderText = "測長[m]";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Path";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Visible = false;
            this.Column7.Width = 5;
            // 
            // Column8
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Column8.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column8.HeaderText = "NG数";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column8.Width = 75;
            // 
            // uclOldList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.btnListUp);
            this.Controls.Add(this.btnListDown);
            this.Controls.Add(this.chkMultiSelect);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.chkHinsyu);
            this.Controls.Add(this.chkMonth);
            this.Controls.Add(this.btnOutResult);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.dgvOldList);
            this.Name = "uclOldList";
            this.Size = new System.Drawing.Size(1126, 830);
            this.Load += new System.EventHandler(this.uclOldList_Load);
            this.VisibleChanged += new System.EventHandler(this.uclOldList_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOldList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        //private System.Windows.Forms.DataGridView dgvOldList;        
        private DataGridViewMulti dgvOldList;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnDelete;
        private Extension.ShortcutKeyHelper shortcutKeyHelper1;
        private System.Windows.Forms.Button btnOutResult;
        private System.Windows.Forms.CheckBox chkMonth;
        private System.Windows.Forms.CheckBox chkHinsyu;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.CheckBox chkMultiSelect;
        public System.Windows.Forms.Button btnListUp;
        public System.Windows.Forms.Button btnListDown;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
    }
}
