namespace Adjustment
{
    partial class uclLightControl
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
            this.chkName = new System.Windows.Forms.CheckBox();
            this.nudLightValue = new System.Windows.Forms.NumericUpDown();
            this.textStdLightValue = new System.Windows.Forms.TextBox();
            this.textDifference = new System.Windows.Forms.TextBox();
            this.trbLightValue = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudLightValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbLightValue)).BeginInit();
            this.SuspendLayout();
            // 
            // chkName
            // 
            this.chkName.AutoSize = true;
            this.chkName.Location = new System.Drawing.Point(4, 4);
            this.chkName.Name = "chkName";
            this.chkName.Size = new System.Drawing.Size(80, 16);
            this.chkName.TabIndex = 0;
            this.chkName.Text = "checkBox1";
            this.chkName.UseVisualStyleBackColor = true;
            this.chkName.CheckedChanged += new System.EventHandler(this.chkName_CheckedChanged);
            // 
            // nudLightValue
            // 
            this.nudLightValue.Location = new System.Drawing.Point(56, 24);
            this.nudLightValue.Maximum = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.nudLightValue.Name = "nudLightValue";
            this.nudLightValue.Size = new System.Drawing.Size(58, 19);
            this.nudLightValue.TabIndex = 4;
            // 
            // textStdLightValue
            // 
            this.textStdLightValue.Enabled = false;
            this.textStdLightValue.Location = new System.Drawing.Point(56, 55);
            this.textStdLightValue.Name = "textStdLightValue";
            this.textStdLightValue.Size = new System.Drawing.Size(56, 19);
            this.textStdLightValue.TabIndex = 12;
            // 
            // textDifference
            // 
            this.textDifference.Enabled = false;
            this.textDifference.Location = new System.Drawing.Point(182, 54);
            this.textDifference.Name = "textDifference";
            this.textDifference.Size = new System.Drawing.Size(56, 19);
            this.textDifference.TabIndex = 13;
            // 
            // trbLightValue
            // 
            this.trbLightValue.AutoSize = false;
            this.trbLightValue.Location = new System.Drawing.Point(120, 20);
            this.trbLightValue.Maximum = 11;
            this.trbLightValue.Name = "trbLightValue";
            this.trbLightValue.Size = new System.Drawing.Size(250, 30);
            this.trbLightValue.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 24);
            this.label1.TabIndex = 14;
            this.label1.Text = "　照明値\r\n（基準値）";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 24);
            this.label3.TabIndex = 28;
            this.label3.Text = "　照明値\r\n（現在値）";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(159, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 29;
            this.label2.Text = "差";
            // 
            // uclLightControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textDifference);
            this.Controls.Add(this.textStdLightValue);
            this.Controls.Add(this.nudLightValue);
            this.Controls.Add(this.trbLightValue);
            this.Controls.Add(this.chkName);
            this.Name = "uclLightControl";
            this.Size = new System.Drawing.Size(380, 81);
            ((System.ComponentModel.ISupportInitialize)(this.nudLightValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbLightValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkName;
        private System.Windows.Forms.NumericUpDown nudLightValue;
        private System.Windows.Forms.TextBox textStdLightValue;
        private System.Windows.Forms.TextBox textDifference;
        private System.Windows.Forms.TrackBar trbLightValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}
