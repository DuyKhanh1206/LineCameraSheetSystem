namespace SheetMapping
{
    partial class uclSheetMap
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
            this.vsbScroll = new System.Windows.Forms.VScrollBar();
            this.picMap = new System.Windows.Forms.PictureBox();
            this.btnRangeMinus = new System.Windows.Forms.Button();
            this.btnRangePlus = new System.Windows.Forms.Button();
            this.txtYPos = new System.Windows.Forms.TextBox();
            this.txtXPos = new System.Windows.Forms.TextBox();
            this.chkScrollLock = new System.Windows.Forms.CheckBox();
            this.txtRangeMeter = new System.Windows.Forms.TextBox();
            this.shortcutKeyHelper1 = new Extension.ShortcutKeyHelper(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picMap)).BeginInit();
            this.SuspendLayout();
            // 
            // vsbScroll
            // 
            this.vsbScroll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vsbScroll.Location = new System.Drawing.Point(571, 22);
            this.vsbScroll.Name = "vsbScroll";
            this.vsbScroll.Size = new System.Drawing.Size(26, 759);
            this.vsbScroll.TabIndex = 0;
            this.vsbScroll.ValueChanged += new System.EventHandler(this.vsbScroll_ValueChanged);
            // 
            // picMap
            // 
            this.picMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picMap.BackColor = System.Drawing.Color.Black;
            this.picMap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.picMap.Location = new System.Drawing.Point(3, 22);
            this.picMap.Name = "picMap";
            this.picMap.Size = new System.Drawing.Size(565, 759);
            this.picMap.TabIndex = 1;
            this.picMap.TabStop = false;
            this.picMap.Paint += new System.Windows.Forms.PaintEventHandler(this.picMap_Paint);
            this.picMap.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.picMap_MouseDoubleClick);
            this.picMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picMap_MouseDown);
            this.picMap.MouseLeave += new System.EventHandler(this.picMap_MouseLeave);
            this.picMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picMap_MouseMove);
            // 
            // btnRangeMinus
            // 
            this.btnRangeMinus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRangeMinus.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnRangeMinus.Location = new System.Drawing.Point(391, 787);
            this.btnRangeMinus.Name = "btnRangeMinus";
            this.shortcutKeyHelper1.SetShortcutKeys(this.btnRangeMinus, System.Windows.Forms.Keys.OemMinus);
            this.btnRangeMinus.Size = new System.Drawing.Size(100, 60);
            this.btnRangeMinus.TabIndex = 2;
            this.btnRangeMinus.Text = "レンジ(-)";
            this.btnRangeMinus.UseVisualStyleBackColor = true;
            // 
            // btnRangePlus
            // 
            this.btnRangePlus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRangePlus.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnRangePlus.Location = new System.Drawing.Point(497, 787);
            this.btnRangePlus.Name = "btnRangePlus";
            this.shortcutKeyHelper1.SetShortcutKeys(this.btnRangePlus, System.Windows.Forms.Keys.Oemplus);
            this.btnRangePlus.Size = new System.Drawing.Size(100, 60);
            this.btnRangePlus.TabIndex = 2;
            this.btnRangePlus.Text = "レンジ(+)";
            this.btnRangePlus.UseVisualStyleBackColor = true;
            // 
            // txtYPos
            // 
            this.txtYPos.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.txtYPos.ForeColor = System.Drawing.Color.White;
            this.txtYPos.Location = new System.Drawing.Point(103, 3);
            this.txtYPos.Name = "txtYPos";
            this.txtYPos.ReadOnly = true;
            this.txtYPos.Size = new System.Drawing.Size(97, 19);
            this.txtYPos.TabIndex = 3;
            this.txtYPos.TabStop = false;
            // 
            // txtXPos
            // 
            this.txtXPos.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.txtXPos.ForeColor = System.Drawing.Color.White;
            this.txtXPos.Location = new System.Drawing.Point(3, 3);
            this.txtXPos.Name = "txtXPos";
            this.txtXPos.ReadOnly = true;
            this.txtXPos.Size = new System.Drawing.Size(97, 19);
            this.txtXPos.TabIndex = 3;
            this.txtXPos.TabStop = false;
            // 
            // chkScrollLock
            // 
            this.chkScrollLock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkScrollLock.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkScrollLock.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkScrollLock.Location = new System.Drawing.Point(4, 797);
            this.chkScrollLock.Name = "chkScrollLock";
            this.chkScrollLock.Size = new System.Drawing.Size(164, 50);
            this.chkScrollLock.TabIndex = 4;
            this.chkScrollLock.Text = "ｽｸﾛｰﾙロック(L)";
            this.chkScrollLock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkScrollLock.UseVisualStyleBackColor = true;
            this.chkScrollLock.CheckedChanged += new System.EventHandler(this.chkScrollLock_CheckedChanged);
            // 
            // txtRangeMeter
            // 
            this.txtRangeMeter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRangeMeter.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.txtRangeMeter.ForeColor = System.Drawing.Color.White;
            this.txtRangeMeter.Location = new System.Drawing.Point(471, 2);
            this.txtRangeMeter.Name = "txtRangeMeter";
            this.txtRangeMeter.ReadOnly = true;
            this.txtRangeMeter.Size = new System.Drawing.Size(97, 19);
            this.txtRangeMeter.TabIndex = 3;
            this.txtRangeMeter.TabStop = false;
            // 
            // uclSheetMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkScrollLock);
            this.Controls.Add(this.txtXPos);
            this.Controls.Add(this.txtRangeMeter);
            this.Controls.Add(this.txtYPos);
            this.Controls.Add(this.btnRangePlus);
            this.Controls.Add(this.btnRangeMinus);
            this.Controls.Add(this.picMap);
            this.Controls.Add(this.vsbScroll);
            this.Name = "uclSheetMap";
            this.Size = new System.Drawing.Size(600, 850);
            this.SizeChanged += new System.EventHandler(this.UclSheetMapReal_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.picMap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.VScrollBar vsbScroll;
        private System.Windows.Forms.PictureBox picMap;
        private System.Windows.Forms.Button btnRangeMinus;
        private System.Windows.Forms.Button btnRangePlus;
        private System.Windows.Forms.TextBox txtYPos;
        private System.Windows.Forms.TextBox txtXPos;
        private System.Windows.Forms.CheckBox chkScrollLock;
        private Extension.ShortcutKeyHelper shortcutKeyHelper1;
        private System.Windows.Forms.TextBox txtRangeMeter;
    }
}
