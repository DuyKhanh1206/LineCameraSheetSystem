namespace LineCameraSheetSystem
{
    partial class uclLightTimeLabel
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDisplayValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(182, 34);
            this.lblTitle.TabIndex = 28;
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
            this.lblDisplayValue.TabIndex = 29;
            this.lblDisplayValue.Text = "9999999";
            this.lblDisplayValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // uclLightTimeLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblDisplayValue);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(7);
            this.Name = "uclLightTimeLabel";
            this.Size = new System.Drawing.Size(310, 35);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDisplayValue;
    }
}
