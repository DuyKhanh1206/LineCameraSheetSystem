namespace LineCameraSheetSystem
{
    partial class uclLightTimeReset
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
            this.btnReset = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDisplayValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(314, 0);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(112, 40);
            this.btnReset.TabIndex = 25;
            this.btnReset.Text = "リセット";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(182, 34);
            this.lblTitle.TabIndex = 29;
            this.lblTitle.Text = "ああああああ[h]";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDisplayValue
            // 
            this.lblDisplayValue.BackColor = System.Drawing.Color.White;
            this.lblDisplayValue.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDisplayValue.Location = new System.Drawing.Point(188, 0);
            this.lblDisplayValue.Name = "lblDisplayValue";
            this.lblDisplayValue.Size = new System.Drawing.Size(120, 34);
            this.lblDisplayValue.TabIndex = 30;
            this.lblDisplayValue.Text = "9999999";
            this.lblDisplayValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uclLightTimeReset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblDisplayValue);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnReset);
            this.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Name = "uclLightTimeReset";
            this.Size = new System.Drawing.Size(425, 40);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDisplayValue;
    }
}
