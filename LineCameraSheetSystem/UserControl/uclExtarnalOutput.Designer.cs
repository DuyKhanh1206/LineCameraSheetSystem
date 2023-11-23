
namespace LineCameraSheetSystem
{
    partial class uclExtarnalOutput
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
            this.spinExtShot = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinExtDelay = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinExtTimer = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.chkExtTimerSub = new System.Windows.Forms.CheckBox();
            this.chkExtTimer = new System.Windows.Forms.CheckBox();
            this.chkZ1 = new System.Windows.Forms.CheckBox();
            this.chkZ2 = new System.Windows.Forms.CheckBox();
            this.chkZ3 = new System.Windows.Forms.CheckBox();
            this.chkZ4 = new System.Windows.Forms.CheckBox();
            this.chkZ5 = new System.Windows.Forms.CheckBox();
            this.chkZ6 = new System.Windows.Forms.CheckBox();
            this.chkZ7 = new System.Windows.Forms.CheckBox();
            this.chkZ8 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // spinExtShot
            // 
            this.spinExtShot.DecimalPlaces = 0;
            this.spinExtShot.EveryValueChanged = false;
            this.spinExtShot.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spinExtShot.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinExtShot.Location = new System.Drawing.Point(0, 175);
            this.spinExtShot.Margin = new System.Windows.Forms.Padding(0);
            this.spinExtShot.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.spinExtShot.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinExtShot.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinExtShot.Name = "spinExtShot";
            this.spinExtShot.Size = new System.Drawing.Size(200, 44);
            this.spinExtShot.TabIndex = 42;
            this.spinExtShot.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinExtShot.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // spinExtDelay
            // 
            this.spinExtDelay.DecimalPlaces = 0;
            this.spinExtDelay.EveryValueChanged = false;
            this.spinExtDelay.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spinExtDelay.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinExtDelay.Location = new System.Drawing.Point(0, 128);
            this.spinExtDelay.Margin = new System.Windows.Forms.Padding(0);
            this.spinExtDelay.Maximum = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            this.spinExtDelay.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinExtDelay.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinExtDelay.Name = "spinExtDelay";
            this.spinExtDelay.Size = new System.Drawing.Size(200, 44);
            this.spinExtDelay.TabIndex = 43;
            this.spinExtDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinExtDelay.Value = new decimal(new int[] {
            60000,
            0,
            0,
            0});
            // 
            // spinExtTimer
            // 
            this.spinExtTimer.DecimalPlaces = 0;
            this.spinExtTimer.EveryValueChanged = false;
            this.spinExtTimer.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spinExtTimer.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinExtTimer.Location = new System.Drawing.Point(0, 81);
            this.spinExtTimer.Margin = new System.Windows.Forms.Padding(0);
            this.spinExtTimer.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.spinExtTimer.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinExtTimer.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinExtTimer.Name = "spinExtTimer";
            this.spinExtTimer.Size = new System.Drawing.Size(200, 44);
            this.spinExtTimer.TabIndex = 44;
            this.spinExtTimer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinExtTimer.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // chkExtTimerSub
            // 
            this.chkExtTimerSub.AutoSize = true;
            this.chkExtTimerSub.Location = new System.Drawing.Point(9, 22);
            this.chkExtTimerSub.Name = "chkExtTimerSub";
            this.chkExtTimerSub.Size = new System.Drawing.Size(15, 14);
            this.chkExtTimerSub.TabIndex = 40;
            this.chkExtTimerSub.Tag = "0";
            this.chkExtTimerSub.UseVisualStyleBackColor = true;
            this.chkExtTimerSub.CheckedChanged += new System.EventHandler(this.chkExtTimer1Sub_CheckedChanged);
            // 
            // chkExtTimer
            // 
            this.chkExtTimer.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkExtTimer.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkExtTimer.Location = new System.Drawing.Point(30, 0);
            this.chkExtTimer.Name = "chkExtTimer";
            this.chkExtTimer.Size = new System.Drawing.Size(170, 75);
            this.chkExtTimer.TabIndex = 41;
            this.chkExtTimer.Tag = "0";
            this.chkExtTimer.Text = "外部1(共通)[表]";
            this.chkExtTimer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkExtTimer.UseVisualStyleBackColor = true;
            this.chkExtTimer.CheckedChanged += new System.EventHandler(this.chkExtTimer1_CheckedChanged);
            // 
            // chkZ1
            // 
            this.chkZ1.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkZ1.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkZ1.Location = new System.Drawing.Point(0, 249);
            this.chkZ1.Name = "chkZ1";
            this.chkZ1.Size = new System.Drawing.Size(50, 50);
            this.chkZ1.TabIndex = 41;
            this.chkZ1.Tag = "0";
            this.chkZ1.Text = "1";
            this.chkZ1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkZ1.UseVisualStyleBackColor = true;
            this.chkZ1.CheckedChanged += new System.EventHandler(this.chkZone_CheckedChanged);
            // 
            // chkZ2
            // 
            this.chkZ2.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkZ2.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkZ2.Location = new System.Drawing.Point(50, 249);
            this.chkZ2.Name = "chkZ2";
            this.chkZ2.Size = new System.Drawing.Size(50, 50);
            this.chkZ2.TabIndex = 41;
            this.chkZ2.Tag = "0";
            this.chkZ2.Text = "2";
            this.chkZ2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkZ2.UseVisualStyleBackColor = true;
            this.chkZ2.CheckedChanged += new System.EventHandler(this.chkZone_CheckedChanged);
            // 
            // chkZ3
            // 
            this.chkZ3.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkZ3.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkZ3.Location = new System.Drawing.Point(100, 249);
            this.chkZ3.Name = "chkZ3";
            this.chkZ3.Size = new System.Drawing.Size(50, 50);
            this.chkZ3.TabIndex = 41;
            this.chkZ3.Tag = "0";
            this.chkZ3.Text = "3";
            this.chkZ3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkZ3.UseVisualStyleBackColor = true;
            this.chkZ3.CheckedChanged += new System.EventHandler(this.chkZone_CheckedChanged);
            // 
            // chkZ4
            // 
            this.chkZ4.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkZ4.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkZ4.Location = new System.Drawing.Point(150, 249);
            this.chkZ4.Name = "chkZ4";
            this.chkZ4.Size = new System.Drawing.Size(50, 50);
            this.chkZ4.TabIndex = 41;
            this.chkZ4.Tag = "0";
            this.chkZ4.Text = "4";
            this.chkZ4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkZ4.UseVisualStyleBackColor = true;
            this.chkZ4.CheckedChanged += new System.EventHandler(this.chkZone_CheckedChanged);
            // 
            // chkZ5
            // 
            this.chkZ5.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkZ5.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkZ5.Location = new System.Drawing.Point(0, 305);
            this.chkZ5.Name = "chkZ5";
            this.chkZ5.Size = new System.Drawing.Size(50, 50);
            this.chkZ5.TabIndex = 41;
            this.chkZ5.Tag = "0";
            this.chkZ5.Text = "5";
            this.chkZ5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkZ5.UseVisualStyleBackColor = true;
            this.chkZ5.CheckedChanged += new System.EventHandler(this.chkZone_CheckedChanged);
            // 
            // chkZ6
            // 
            this.chkZ6.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkZ6.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkZ6.Location = new System.Drawing.Point(50, 305);
            this.chkZ6.Name = "chkZ6";
            this.chkZ6.Size = new System.Drawing.Size(50, 50);
            this.chkZ6.TabIndex = 41;
            this.chkZ6.Tag = "0";
            this.chkZ6.Text = "6";
            this.chkZ6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkZ6.UseVisualStyleBackColor = true;
            this.chkZ6.CheckedChanged += new System.EventHandler(this.chkZone_CheckedChanged);
            // 
            // chkZ7
            // 
            this.chkZ7.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkZ7.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkZ7.Location = new System.Drawing.Point(100, 305);
            this.chkZ7.Name = "chkZ7";
            this.chkZ7.Size = new System.Drawing.Size(50, 50);
            this.chkZ7.TabIndex = 41;
            this.chkZ7.Tag = "0";
            this.chkZ7.Text = "7";
            this.chkZ7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkZ7.UseVisualStyleBackColor = true;
            this.chkZ7.CheckedChanged += new System.EventHandler(this.chkZone_CheckedChanged);
            // 
            // chkZ8
            // 
            this.chkZ8.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkZ8.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.chkZ8.Location = new System.Drawing.Point(150, 305);
            this.chkZ8.Name = "chkZ8";
            this.chkZ8.Size = new System.Drawing.Size(50, 50);
            this.chkZ8.TabIndex = 41;
            this.chkZ8.Tag = "0";
            this.chkZ8.Text = "8";
            this.chkZ8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkZ8.UseVisualStyleBackColor = true;
            this.chkZ8.CheckedChanged += new System.EventHandler(this.chkZone_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 222);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 24);
            this.label1.TabIndex = 45;
            this.label1.Text = "ゾーン";
            // 
            // uclExtarnalOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.spinExtShot);
            this.Controls.Add(this.spinExtDelay);
            this.Controls.Add(this.spinExtTimer);
            this.Controls.Add(this.chkExtTimerSub);
            this.Controls.Add(this.chkZ8);
            this.Controls.Add(this.chkZ4);
            this.Controls.Add(this.chkZ7);
            this.Controls.Add(this.chkZ3);
            this.Controls.Add(this.chkZ6);
            this.Controls.Add(this.chkZ5);
            this.Controls.Add(this.chkZ2);
            this.Controls.Add(this.chkZ1);
            this.Controls.Add(this.chkExtTimer);
            this.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "uclExtarnalOutput";
            this.Size = new System.Drawing.Size(200, 357);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkExtTimerSub;
        private System.Windows.Forms.CheckBox chkExtTimer;
        public Fujita.InspectionSystem.uclNumericInputSmall spinExtTimer;
        public Fujita.InspectionSystem.uclNumericInputSmall spinExtShot;
        public Fujita.InspectionSystem.uclNumericInputSmall spinExtDelay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkZ1;
        private System.Windows.Forms.CheckBox chkZ2;
        private System.Windows.Forms.CheckBox chkZ3;
        private System.Windows.Forms.CheckBox chkZ4;
        private System.Windows.Forms.CheckBox chkZ5;
        private System.Windows.Forms.CheckBox chkZ6;
        private System.Windows.Forms.CheckBox chkZ7;
        private System.Windows.Forms.CheckBox chkZ8;
    }
}
