
namespace LineCameraSheetSystem
{
    partial class uclPatLiteOutPut
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
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
            this.spinPatLiteDelay = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinPatLiteTimer = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // spinPatLiteDelay
            // 
            this.spinPatLiteDelay.DecimalPlaces = 0;
            this.spinPatLiteDelay.EveryValueChanged = false;
            this.spinPatLiteDelay.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spinPatLiteDelay.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinPatLiteDelay.Location = new System.Drawing.Point(0, 77);
            this.spinPatLiteDelay.Margin = new System.Windows.Forms.Padding(0);
            this.spinPatLiteDelay.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.spinPatLiteDelay.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinPatLiteDelay.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinPatLiteDelay.Name = "spinPatLiteDelay";
            this.spinPatLiteDelay.Size = new System.Drawing.Size(200, 44);
            this.spinPatLiteDelay.TabIndex = 43;
            this.spinPatLiteDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinPatLiteDelay.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // spinPatLiteTimer
            // 
            this.spinPatLiteTimer.DecimalPlaces = 0;
            this.spinPatLiteTimer.EveryValueChanged = false;
            this.spinPatLiteTimer.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spinPatLiteTimer.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinPatLiteTimer.Location = new System.Drawing.Point(0, 33);
            this.spinPatLiteTimer.Margin = new System.Windows.Forms.Padding(0);
            this.spinPatLiteTimer.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.spinPatLiteTimer.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinPatLiteTimer.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinPatLiteTimer.Name = "spinPatLiteTimer";
            this.spinPatLiteTimer.Size = new System.Drawing.Size(200, 44);
            this.spinPatLiteTimer.TabIndex = 44;
            this.spinPatLiteTimer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinPatLiteTimer.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(207, 33);
            this.lblTitle.TabIndex = 45;
            this.lblTitle.Text = "状態監視領域";
            // 
            // uclPatLiteOutPut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.spinPatLiteDelay);
            this.Controls.Add(this.spinPatLiteTimer);
            this.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "uclPatLiteOutPut";
            this.Size = new System.Drawing.Size(224, 125);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public Fujita.InspectionSystem.uclNumericInputSmall spinPatLiteTimer;
        public Fujita.InspectionSystem.uclNumericInputSmall spinPatLiteDelay;
        private System.Windows.Forms.Label lblTitle;
    }
}
