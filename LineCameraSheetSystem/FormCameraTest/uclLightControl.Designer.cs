namespace Fujita.InspectionSystem
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
            this.chkLightEnable = new System.Windows.Forms.CheckBox();
            this.sclLightValue = new System.Windows.Forms.TrackBar();
            this.spinLightValue = new System.Windows.Forms.NumericUpDown();
            this.btnRoi = new System.Windows.Forms.Button();
            this.lblGrayNow = new System.Windows.Forms.Label();
            this.spinGrayBase = new System.Windows.Forms.NumericUpDown();
            this.chkGrayBaseEnable = new System.Windows.Forms.CheckBox();
            this.btnEntryLightBase = new System.Windows.Forms.Button();
            this.btnCalcLightDiff = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.spinLightDiff = new System.Windows.Forms.NumericUpDown();
            this.chkLightDiffEnable = new System.Windows.Forms.CheckBox();
            this.spinLightBase = new System.Windows.Forms.NumericUpDown();
            this.spinCameraIndex = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.sclLightValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinLightValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinGrayBase)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinLightDiff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinLightBase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinCameraIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // chkLightEnable
            // 
            this.chkLightEnable.AutoSize = true;
            this.chkLightEnable.Location = new System.Drawing.Point(3, 3);
            this.chkLightEnable.Name = "chkLightEnable";
            this.chkLightEnable.Size = new System.Drawing.Size(60, 16);
            this.chkLightEnable.TabIndex = 0;
            this.chkLightEnable.Text = "未定義";
            this.chkLightEnable.UseVisualStyleBackColor = true;
            // 
            // sclLightValue
            // 
            this.sclLightValue.AutoSize = false;
            this.sclLightValue.Location = new System.Drawing.Point(130, 3);
            this.sclLightValue.Name = "sclLightValue";
            this.sclLightValue.Size = new System.Drawing.Size(236, 30);
            this.sclLightValue.TabIndex = 5;
            this.sclLightValue.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            // 
            // spinLightValue
            // 
            this.spinLightValue.Location = new System.Drawing.Point(79, 3);
            this.spinLightValue.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.spinLightValue.Name = "spinLightValue";
            this.spinLightValue.Size = new System.Drawing.Size(45, 19);
            this.spinLightValue.TabIndex = 4;
            this.spinLightValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinLightValue.Value = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            // 
            // btnRoi
            // 
            this.btnRoi.Location = new System.Drawing.Point(3, 37);
            this.btnRoi.Name = "btnRoi";
            this.btnRoi.Size = new System.Drawing.Size(70, 23);
            this.btnRoi.TabIndex = 6;
            this.btnRoi.Text = "領域設定";
            this.btnRoi.UseVisualStyleBackColor = true;
            this.btnRoi.Click += new System.EventHandler(this.btnRoi_Click);
            // 
            // lblGrayNow
            // 
            this.lblGrayNow.BackColor = System.Drawing.Color.Gainsboro;
            this.lblGrayNow.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGrayNow.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblGrayNow.Location = new System.Drawing.Point(60, 15);
            this.lblGrayNow.Name = "lblGrayNow";
            this.lblGrayNow.Size = new System.Drawing.Size(41, 19);
            this.lblGrayNow.TabIndex = 7;
            this.lblGrayNow.Text = "000";
            this.lblGrayNow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // spinGrayBase
            // 
            this.spinGrayBase.Enabled = false;
            this.spinGrayBase.Location = new System.Drawing.Point(60, 41);
            this.spinGrayBase.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.spinGrayBase.Name = "spinGrayBase";
            this.spinGrayBase.Size = new System.Drawing.Size(53, 19);
            this.spinGrayBase.TabIndex = 8;
            this.spinGrayBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinGrayBase.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // chkGrayBaseEnable
            // 
            this.chkGrayBaseEnable.AutoSize = true;
            this.chkGrayBaseEnable.Location = new System.Drawing.Point(6, 42);
            this.chkGrayBaseEnable.Name = "chkGrayBaseEnable";
            this.chkGrayBaseEnable.Size = new System.Drawing.Size(48, 16);
            this.chkGrayBaseEnable.TabIndex = 9;
            this.chkGrayBaseEnable.Text = "基準";
            this.chkGrayBaseEnable.UseVisualStyleBackColor = true;
            this.chkGrayBaseEnable.CheckedChanged += new System.EventHandler(this.chkGrayBaseEnable_CheckedChanged);
            // 
            // btnEntryLightBase
            // 
            this.btnEntryLightBase.Location = new System.Drawing.Point(99, 13);
            this.btnEntryLightBase.Name = "btnEntryLightBase";
            this.btnEntryLightBase.Size = new System.Drawing.Size(41, 23);
            this.btnEntryLightBase.TabIndex = 6;
            this.btnEntryLightBase.Text = "登録";
            this.btnEntryLightBase.UseVisualStyleBackColor = true;
            this.btnEntryLightBase.Click += new System.EventHandler(this.btnEntryLightBase_Click);
            // 
            // btnCalcLightDiff
            // 
            this.btnCalcLightDiff.Location = new System.Drawing.Point(99, 37);
            this.btnCalcLightDiff.Name = "btnCalcLightDiff";
            this.btnCalcLightDiff.Size = new System.Drawing.Size(41, 23);
            this.btnCalcLightDiff.TabIndex = 6;
            this.btnCalcLightDiff.Text = "算出";
            this.btnCalcLightDiff.UseVisualStyleBackColor = true;
            this.btnCalcLightDiff.Click += new System.EventHandler(this.btnCalcLightDiff_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "ｶﾒﾗ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "現在";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lblGrayNow);
            this.groupBox1.Controls.Add(this.chkGrayBaseEnable);
            this.groupBox1.Controls.Add(this.spinGrayBase);
            this.groupBox1.Location = new System.Drawing.Point(79, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(121, 65);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Gray値";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.spinLightDiff);
            this.groupBox2.Controls.Add(this.chkLightDiffEnable);
            this.groupBox2.Controls.Add(this.spinLightBase);
            this.groupBox2.Controls.Add(this.btnCalcLightDiff);
            this.groupBox2.Controls.Add(this.btnEntryLightBase);
            this.groupBox2.Location = new System.Drawing.Point(206, 37);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(150, 65);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "照明値";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 10;
            this.label8.Text = "基準";
            // 
            // spinLightDiff
            // 
            this.spinLightDiff.Enabled = false;
            this.spinLightDiff.Location = new System.Drawing.Point(48, 40);
            this.spinLightDiff.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.spinLightDiff.Minimum = new decimal(new int[] {
            9999,
            0,
            0,
            -2147483648});
            this.spinLightDiff.Name = "spinLightDiff";
            this.spinLightDiff.Size = new System.Drawing.Size(45, 19);
            this.spinLightDiff.TabIndex = 8;
            this.spinLightDiff.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chkLightDiffEnable
            // 
            this.chkLightDiffEnable.AutoSize = true;
            this.chkLightDiffEnable.Location = new System.Drawing.Point(6, 41);
            this.chkLightDiffEnable.Name = "chkLightDiffEnable";
            this.chkLightDiffEnable.Size = new System.Drawing.Size(36, 16);
            this.chkLightDiffEnable.TabIndex = 9;
            this.chkLightDiffEnable.Text = "差";
            this.chkLightDiffEnable.UseVisualStyleBackColor = true;
            this.chkLightDiffEnable.CheckedChanged += new System.EventHandler(this.chkLightDiffEnable_CheckedChanged);
            // 
            // spinLightBase
            // 
            this.spinLightBase.Enabled = false;
            this.spinLightBase.Location = new System.Drawing.Point(48, 15);
            this.spinLightBase.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.spinLightBase.Name = "spinLightBase";
            this.spinLightBase.Size = new System.Drawing.Size(45, 19);
            this.spinLightBase.TabIndex = 8;
            this.spinLightBase.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinLightBase.Value = new decimal(new int[] {
            128,
            0,
            0,
            0});
            // 
            // spinCameraIndex
            // 
            this.spinCameraIndex.Enabled = false;
            this.spinCameraIndex.Location = new System.Drawing.Point(35, 66);
            this.spinCameraIndex.Name = "spinCameraIndex";
            this.spinCameraIndex.Size = new System.Drawing.Size(38, 19);
            this.spinCameraIndex.TabIndex = 8;
            this.spinCameraIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinCameraIndex.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // uclLightControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.spinCameraIndex);
            this.Controls.Add(this.btnRoi);
            this.Controls.Add(this.sclLightValue);
            this.Controls.Add(this.spinLightValue);
            this.Controls.Add(this.chkLightEnable);
            this.Name = "uclLightControl";
            this.Size = new System.Drawing.Size(372, 105);
            ((System.ComponentModel.ISupportInitialize)(this.sclLightValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinLightValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinGrayBase)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinLightDiff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinLightBase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinCameraIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TrackBar sclLightValue;
        private System.Windows.Forms.NumericUpDown spinLightValue;
        private System.Windows.Forms.Button btnRoi;
        private System.Windows.Forms.Label lblGrayNow;
        private System.Windows.Forms.NumericUpDown spinGrayBase;
        private System.Windows.Forms.CheckBox chkGrayBaseEnable;
        private System.Windows.Forms.Button btnEntryLightBase;
        private System.Windows.Forms.Button btnCalcLightDiff;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown spinCameraIndex;
        private System.Windows.Forms.NumericUpDown spinLightDiff;
        private System.Windows.Forms.NumericUpDown spinLightBase;
        private System.Windows.Forms.CheckBox chkLightDiffEnable;
        private System.Windows.Forms.CheckBox chkLightEnable;
    }
}
