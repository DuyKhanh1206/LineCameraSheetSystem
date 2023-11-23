namespace Adjustment
{
    partial class uclMaintenanceLightControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.textStdLightValue = new System.Windows.Forms.TextBox();
            this.textOffset = new System.Windows.Forms.TextBox();
            this.btnLine1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textNowLightValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(14, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 24);
            this.label1.TabIndex = 7;
            this.label1.Text = "　照明値\r\n（基準値）";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(296, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "差";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Enabled = false;
            this.labelTitle.Location = new System.Drawing.Point(15, 3);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(41, 12);
            this.labelTitle.TabIndex = 10;
            this.labelTitle.Text = "未定義";
            // 
            // textStdLightValue
            // 
            this.textStdLightValue.Enabled = false;
            this.textStdLightValue.Location = new System.Drawing.Point(75, 24);
            this.textStdLightValue.Name = "textStdLightValue";
            this.textStdLightValue.Size = new System.Drawing.Size(56, 19);
            this.textStdLightValue.TabIndex = 11;
            // 
            // textOffset
            // 
            this.textOffset.Enabled = false;
            this.textOffset.Location = new System.Drawing.Point(319, 24);
            this.textOffset.Name = "textOffset";
            this.textOffset.Size = new System.Drawing.Size(56, 19);
            this.textOffset.TabIndex = 12;
            // 
            // btnLine1
            // 
            this.btnLine1.Enabled = false;
            this.btnLine1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLine1.Location = new System.Drawing.Point(6, 8);
            this.btnLine1.Name = "btnLine1";
            this.btnLine1.Size = new System.Drawing.Size(378, 46);
            this.btnLine1.TabIndex = 26;
            this.btnLine1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(137, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 24);
            this.label3.TabIndex = 27;
            this.label3.Text = "　照明値\r\n（現在値）";
            // 
            // textNowLightValue
            // 
            this.textNowLightValue.Enabled = false;
            this.textNowLightValue.Location = new System.Drawing.Point(200, 24);
            this.textNowLightValue.Name = "textNowLightValue";
            this.textNowLightValue.Size = new System.Drawing.Size(56, 19);
            this.textNowLightValue.TabIndex = 28;
            // 
            // uclMaintenanceLightControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textNowLightValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textOffset);
            this.Controls.Add(this.textStdLightValue);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLine1);
            this.Name = "uclMaintenanceLightControl";
            this.Size = new System.Drawing.Size(394, 61);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox textStdLightValue;
        private System.Windows.Forms.TextBox textOffset;
        private System.Windows.Forms.Button btnLine1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textNowLightValue;
    }
}
