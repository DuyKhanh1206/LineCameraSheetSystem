namespace LineCameraSheetSystem
{
    partial class uclRecipeLightControl
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
            this.lblLightName = new System.Windows.Forms.Label();
            this.chkLightEnable = new System.Windows.Forms.CheckBox();
            this.spinLightValue = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.SuspendLayout();
            // 
            // lblLightName
            // 
            this.lblLightName.AutoSize = true;
            this.lblLightName.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblLightName.Location = new System.Drawing.Point(0, 0);
            this.lblLightName.Name = "lblLightName";
            this.lblLightName.Size = new System.Drawing.Size(43, 15);
            this.lblLightName.TabIndex = 16;
            this.lblLightName.Text = "Name";
            // 
            // chkLightEnable
            // 
            this.chkLightEnable.Enabled = false;
            this.chkLightEnable.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkLightEnable.Location = new System.Drawing.Point(172, 15);
            this.chkLightEnable.Margin = new System.Windows.Forms.Padding(2);
            this.chkLightEnable.Name = "chkLightEnable";
            this.chkLightEnable.Size = new System.Drawing.Size(68, 44);
            this.chkLightEnable.TabIndex = 24;
            this.chkLightEnable.Text = "有効";
            this.chkLightEnable.UseVisualStyleBackColor = true;
            this.chkLightEnable.CheckedChanged += new System.EventHandler(this.chkLightEnable_CheckedChanged);
            // 
            // spinLightValue
            // 
            this.spinLightValue.DecimalPlaces = 0;
            this.spinLightValue.EveryValueChanged = false;
            this.spinLightValue.Font = new System.Drawing.Font("MS UI Gothic", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spinLightValue.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinLightValue.Location = new System.Drawing.Point(0, 15);
            this.spinLightValue.Margin = new System.Windows.Forms.Padding(0);
            this.spinLightValue.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spinLightValue.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinLightValue.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinLightValue.Name = "spinLightValue";
            this.spinLightValue.Size = new System.Drawing.Size(170, 44);
            this.spinLightValue.TabIndex = 25;
            this.spinLightValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinLightValue.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spinLightValue.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinLightValue_ValueChanged);
            // 
            // uclRecipeLightControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spinLightValue);
            this.Controls.Add(this.chkLightEnable);
            this.Controls.Add(this.lblLightName);
            this.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "uclRecipeLightControl";
            this.Size = new System.Drawing.Size(240, 60);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLightName;
        private System.Windows.Forms.CheckBox chkLightEnable;
        private Fujita.InspectionSystem.uclNumericInputSmall spinLightValue;
    }
}
