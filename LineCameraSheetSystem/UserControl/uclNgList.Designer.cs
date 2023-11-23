namespace LineCameraSheetSystem
{
    partial class uclNgList
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
            this.chkboxScrolRock = new System.Windows.Forms.CheckBox();
            this.listViewNGItem = new LineCameraSheetSystem.BufferdListView();
            this.columnHeader0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.shortcutKeyHelper1 = new Extension.ShortcutKeyHelper(this.components);
            this.btnListUp = new System.Windows.Forms.Button();
            this.btnListDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkboxScrolRock
            // 
            this.chkboxScrolRock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkboxScrolRock.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkboxScrolRock.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkboxScrolRock.Location = new System.Drawing.Point(333, 787);
            this.chkboxScrolRock.Name = "chkboxScrolRock";
            this.chkboxScrolRock.Size = new System.Drawing.Size(164, 60);
            this.chkboxScrolRock.TabIndex = 2;
            this.chkboxScrolRock.Text = "スクロールロック(L)";
            this.chkboxScrolRock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkboxScrolRock.UseVisualStyleBackColor = true;
            this.chkboxScrolRock.CheckedChanged += new System.EventHandler(this.chkboxScrolRock_CheckedChanged);
            // 
            // listViewNGItem
            // 
            this.listViewNGItem.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewNGItem.AutoArrange = false;
            this.listViewNGItem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listViewNGItem.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader0,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.listViewNGItem.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listViewNGItem.FullRowSelect = true;
            this.listViewNGItem.GridLines = true;
            this.listViewNGItem.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewNGItem.HideSelection = false;
            this.listViewNGItem.Location = new System.Drawing.Point(4, 69);
            this.listViewNGItem.MultiSelect = false;
            this.listViewNGItem.Name = "listViewNGItem";
            this.listViewNGItem.Size = new System.Drawing.Size(492, 712);
            this.listViewNGItem.TabIndex = 1;
            this.listViewNGItem.UseCompatibleStateImageBehavior = false;
            this.listViewNGItem.View = System.Windows.Forms.View.Details;
            this.listViewNGItem.SelectedIndexChanged += new System.EventHandler(this.listViewNGItem_SelectedIndexChanged);
            this.listViewNGItem.DoubleClick += new System.EventHandler(this.listViewNGItem_DoubleClick);
            // 
            // columnHeader0
            // 
            this.columnHeader0.Text = "No.";
            this.columnHeader0.Width = 40;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "測長  ";
            this.columnHeader1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader1.Width = 65;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "ﾓｰﾄﾞ";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 45;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "面ｶ";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 40;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "欠種";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 45;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "ｱﾄﾞ ";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader5.Width = 45;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "ｿﾞ";
            this.columnHeader6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader6.Width = 35;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "発生時間";
            this.columnHeader7.Width = 152;
            // 
            // btnListUp
            // 
            this.btnListUp.Font = new System.Drawing.Font("MS UI Gothic", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnListUp.Location = new System.Drawing.Point(397, 3);
            this.btnListUp.Name = "btnListUp";
            this.btnListUp.Size = new System.Drawing.Size(100, 60);
            this.btnListUp.TabIndex = 3;
            this.btnListUp.Text = "▲";
            this.btnListUp.UseVisualStyleBackColor = true;
            this.btnListUp.Click += new System.EventHandler(this.btnListUp_Click);
            // 
            // btnListDown
            // 
            this.btnListDown.Font = new System.Drawing.Font("MS UI Gothic", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnListDown.Location = new System.Drawing.Point(291, 3);
            this.btnListDown.Name = "btnListDown";
            this.btnListDown.Size = new System.Drawing.Size(100, 60);
            this.btnListDown.TabIndex = 4;
            this.btnListDown.Text = "▼";
            this.btnListDown.UseVisualStyleBackColor = true;
            this.btnListDown.Click += new System.EventHandler(this.btnListDown_Click);
            // 
            // uclNgList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnListUp);
            this.Controls.Add(this.btnListDown);
            this.Controls.Add(this.chkboxScrolRock);
            this.Controls.Add(this.listViewNGItem);
            this.Name = "uclNgList";
            this.Size = new System.Drawing.Size(500, 850);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader0;
        public System.Windows.Forms.CheckBox chkboxScrolRock;
//        public System.Windows.Forms.ListView listViewNGItem;
        public BufferdListView listViewNGItem;
        private Extension.ShortcutKeyHelper shortcutKeyHelper1;
        public System.Windows.Forms.Button btnListUp;
        public System.Windows.Forms.Button btnListDown;
    }
}
