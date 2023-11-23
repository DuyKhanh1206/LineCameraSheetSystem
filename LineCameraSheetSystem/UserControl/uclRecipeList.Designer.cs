namespace LineCameraSheetSystem
{
    partial class uclRecipeList
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
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.listviewKindName = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textEditBox = new System.Windows.Forms.TextBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnListDown = new System.Windows.Forms.Button();
            this.btnListUp = new System.Windows.Forms.Button();
            this.shortcutKeyHelper1 = new Extension.ShortcutKeyHelper(this.components);
            this.SuspendLayout();
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCopy.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCopy.Location = new System.Drawing.Point(107, 787);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(100, 60);
            this.btnCopy.TabIndex = 2;
            this.btnCopy.Text = "コピー(C)";
            this.btnCopy.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDelete.Location = new System.Drawing.Point(397, 787);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 60);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "削除(D)";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // listviewKindName
            // 
            this.listviewKindName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listviewKindName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listviewKindName.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listviewKindName.Cursor = System.Windows.Forms.Cursors.Default;
            this.listviewKindName.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listviewKindName.ForeColor = System.Drawing.SystemColors.WindowFrame;
            this.listviewKindName.FullRowSelect = true;
            this.listviewKindName.GridLines = true;
            this.listviewKindName.HideSelection = false;
            this.listviewKindName.Location = new System.Drawing.Point(3, 66);
            this.listviewKindName.MultiSelect = false;
            this.listviewKindName.Name = "listviewKindName";
            this.listviewKindName.Size = new System.Drawing.Size(494, 715);
            this.listviewKindName.TabIndex = 0;
            this.listviewKindName.UseCompatibleStateImageBehavior = false;
            this.listviewKindName.View = System.Windows.Forms.View.Details;
            this.listviewKindName.SelectedIndexChanged += new System.EventHandler(this.listviewKindName_SelectedIndexChanged);
            this.listviewKindName.DoubleClick += new System.EventHandler(this.listviewKindName_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "No.";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "品種名";
            this.columnHeader2.Width = 350;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "";
            this.columnHeader3.Width = 65;
            // 
            // textEditBox
            // 
            this.textEditBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textEditBox.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textEditBox.Location = new System.Drawing.Point(70, 450);
            this.textEditBox.Name = "textEditBox";
            this.textEditBox.Size = new System.Drawing.Size(339, 32);
            this.textEditBox.TabIndex = 5;
            this.textEditBox.Visible = false;
            this.textEditBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textEditBox_MouseClick);
            this.textEditBox.TextChanged += new System.EventHandler(this.textEditBox_TextChanged);
            this.textEditBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEditBox_KeyDown);
            this.textEditBox.Leave += new System.EventHandler(this.textEditBox_Leave);
            // 
            // btnPaste
            // 
            this.btnPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPaste.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnPaste.Location = new System.Drawing.Point(248, 787);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(100, 60);
            this.btnPaste.TabIndex = 6;
            this.btnPaste.Text = "貼り付け(P)";
            this.btnPaste.UseVisualStyleBackColor = true;
            // 
            // btnListDown
            // 
            this.btnListDown.Font = new System.Drawing.Font("MS UI Gothic", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnListDown.Location = new System.Drawing.Point(291, 3);
            this.btnListDown.Name = "btnListDown";
            this.btnListDown.Size = new System.Drawing.Size(100, 60);
            this.btnListDown.TabIndex = 2;
            this.btnListDown.Text = "▼";
            this.btnListDown.UseVisualStyleBackColor = true;
            this.btnListDown.Click += new System.EventHandler(this.btnListDown_Click);
            // 
            // btnListUp
            // 
            this.btnListUp.Font = new System.Drawing.Font("MS UI Gothic", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnListUp.Location = new System.Drawing.Point(397, 3);
            this.btnListUp.Name = "btnListUp";
            this.btnListUp.Size = new System.Drawing.Size(100, 60);
            this.btnListUp.TabIndex = 2;
            this.btnListUp.Text = "▲";
            this.btnListUp.UseVisualStyleBackColor = true;
            this.btnListUp.Click += new System.EventHandler(this.btnListUp_Click);
            // 
            // uclRecipeList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.textEditBox);
            this.Controls.Add(this.listviewKindName);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnListUp);
            this.Controls.Add(this.btnListDown);
            this.Controls.Add(this.btnCopy);
            this.Name = "uclRecipeList";
            this.Size = new System.Drawing.Size(500, 850);
            this.Load += new System.EventHandler(this.uclRecipeList_Load);
            this.VisibleChanged += new System.EventHandler(this.uclRecipeList_VisibleChanged);
            this.Click += new System.EventHandler(this.uclRecipeList_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ListView listviewKindName;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        public System.Windows.Forms.TextBox textEditBox;
        private Extension.ShortcutKeyHelper shortcutKeyHelper1;
        public System.Windows.Forms.Button btnPaste;
        public System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        public System.Windows.Forms.Button btnListDown;
        public System.Windows.Forms.Button btnListUp;
    }
}
