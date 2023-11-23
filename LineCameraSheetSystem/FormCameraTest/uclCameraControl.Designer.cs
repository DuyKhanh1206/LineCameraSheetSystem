namespace Fujita.InspectionSystem
{
    partial class uclCameraControl
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.nudGain = new System.Windows.Forms.NumericUpDown();
            this.nudOffset = new System.Windows.Forms.NumericUpDown();
            this.nudExposure = new System.Windows.Forms.NumericUpDown();
            this.trbGain = new System.Windows.Forms.TrackBar();
            this.trbOffset = new System.Windows.Forms.TrackBar();
            this.trbExposure = new System.Windows.Forms.TrackBar();
            this.lblWidth = new System.Windows.Forms.Label();
            this.lblHeight = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblFrameRate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblLineRate = new System.Windows.Forms.Label();
            this.chkCamera = new System.Windows.Forms.CheckBox();
            this.nudTriggerDelay = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.nudLineRate = new System.Windows.Forms.NumericUpDown();
            this.btnFcRoi = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnChangeLightFc = new System.Windows.Forms.Button();
            this.btnEntryFcBase = new System.Windows.Forms.Button();
            this.grpWhiteB = new System.Windows.Forms.GroupBox();
            this.btnChangeLightWhiteB = new System.Windows.Forms.Button();
            this.chkAllImage = new System.Windows.Forms.CheckBox();
            this.btnResetWhiteB = new System.Windows.Forms.Button();
            this.btnEntryWhiteB = new System.Windows.Forms.Button();
            this.btnRunWhiteB = new System.Windows.Forms.Button();
            this.btnWhiteBalanceRoi = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExposure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbExposure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTriggerDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineRate)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.grpWhiteB.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Gain";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(91, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "Offset";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(190, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "Exposure";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "Width:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(90, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 12);
            this.label6.TabIndex = 1;
            this.label6.Text = "Height:";
            // 
            // nudGain
            // 
            this.nudGain.Location = new System.Drawing.Point(35, 50);
            this.nudGain.Name = "nudGain";
            this.nudGain.Size = new System.Drawing.Size(50, 19);
            this.nudGain.TabIndex = 2;
            this.nudGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudOffset
            // 
            this.nudOffset.Location = new System.Drawing.Point(134, 50);
            this.nudOffset.Name = "nudOffset";
            this.nudOffset.Size = new System.Drawing.Size(50, 19);
            this.nudOffset.TabIndex = 2;
            this.nudOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // nudExposure
            // 
            this.nudExposure.Location = new System.Drawing.Point(248, 50);
            this.nudExposure.Name = "nudExposure";
            this.nudExposure.Size = new System.Drawing.Size(76, 19);
            this.nudExposure.TabIndex = 2;
            this.nudExposure.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // trbGain
            // 
            this.trbGain.AutoSize = false;
            this.trbGain.Location = new System.Drawing.Point(340, 0);
            this.trbGain.Name = "trbGain";
            this.trbGain.Size = new System.Drawing.Size(23, 20);
            this.trbGain.TabIndex = 3;
            this.trbGain.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trbGain.Visible = false;
            // 
            // trbOffset
            // 
            this.trbOffset.AutoSize = false;
            this.trbOffset.Location = new System.Drawing.Point(340, 3);
            this.trbOffset.Name = "trbOffset";
            this.trbOffset.Size = new System.Drawing.Size(23, 20);
            this.trbOffset.TabIndex = 3;
            this.trbOffset.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trbOffset.Visible = false;
            // 
            // trbExposure
            // 
            this.trbExposure.AutoSize = false;
            this.trbExposure.Location = new System.Drawing.Point(340, 13);
            this.trbExposure.Name = "trbExposure";
            this.trbExposure.Size = new System.Drawing.Size(23, 20);
            this.trbExposure.TabIndex = 3;
            this.trbExposure.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trbExposure.Visible = false;
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(48, 24);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(33, 12);
            this.lblWidth.TabIndex = 1;
            this.lblWidth.Text = "Width";
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(140, 24);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(38, 12);
            this.lblHeight.TabIndex = 1;
            this.lblHeight.Text = "Height";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(199, 13);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "FrameRate";
            // 
            // lblFrameRate
            // 
            this.lblFrameRate.AutoSize = true;
            this.lblFrameRate.Location = new System.Drawing.Point(266, 13);
            this.lblFrameRate.Name = "lblFrameRate";
            this.lblFrameRate.Size = new System.Drawing.Size(61, 12);
            this.lblFrameRate.TabIndex = 0;
            this.lblFrameRate.Text = "FrameRate";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(199, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "LineRate";
            // 
            // lblLineRate
            // 
            this.lblLineRate.AutoSize = true;
            this.lblLineRate.Location = new System.Drawing.Point(266, 29);
            this.lblLineRate.Name = "lblLineRate";
            this.lblLineRate.Size = new System.Drawing.Size(50, 12);
            this.lblLineRate.TabIndex = 0;
            this.lblLineRate.Text = "LineRate";
            // 
            // chkCamera
            // 
            this.chkCamera.AutoSize = true;
            this.chkCamera.Location = new System.Drawing.Point(3, 3);
            this.chkCamera.Name = "chkCamera";
            this.chkCamera.Size = new System.Drawing.Size(60, 16);
            this.chkCamera.TabIndex = 4;
            this.chkCamera.Text = "未定義";
            this.chkCamera.UseVisualStyleBackColor = true;
            this.chkCamera.CheckedChanged += new System.EventHandler(this.chkCamera_CheckedChanged);
            // 
            // nudTriggerDelay
            // 
            this.nudTriggerDelay.Location = new System.Drawing.Point(54, 72);
            this.nudTriggerDelay.Name = "nudTriggerDelay";
            this.nudTriggerDelay.Size = new System.Drawing.Size(76, 19);
            this.nudTriggerDelay.TabIndex = 2;
            this.nudTriggerDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 24);
            this.label7.TabIndex = 0;
            this.label7.Text = "Trigger\r\n   Delay";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(136, 74);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "LineRate";
            // 
            // nudLineRate
            // 
            this.nudLineRate.DecimalPlaces = 1;
            this.nudLineRate.Location = new System.Drawing.Point(192, 72);
            this.nudLineRate.Name = "nudLineRate";
            this.nudLineRate.Size = new System.Drawing.Size(87, 19);
            this.nudLineRate.TabIndex = 2;
            this.nudLineRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnFcRoi
            // 
            this.btnFcRoi.Location = new System.Drawing.Point(6, 18);
            this.btnFcRoi.Name = "btnFcRoi";
            this.btnFcRoi.Size = new System.Drawing.Size(61, 23);
            this.btnFcRoi.TabIndex = 7;
            this.btnFcRoi.Text = "領域設定";
            this.btnFcRoi.UseVisualStyleBackColor = true;
            this.btnFcRoi.Click += new System.EventHandler(this.btnFcRoi_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnChangeLightFc);
            this.groupBox1.Controls.Add(this.btnEntryFcBase);
            this.groupBox1.Controls.Add(this.btnFcRoi);
            this.groupBox1.Location = new System.Drawing.Point(3, 99);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(357, 52);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "ﾌｫｰｶｽ";
            // 
            // btnChangeLightFc
            // 
            this.btnChangeLightFc.Location = new System.Drawing.Point(249, 18);
            this.btnChangeLightFc.Name = "btnChangeLightFc";
            this.btnChangeLightFc.Size = new System.Drawing.Size(102, 23);
            this.btnChangeLightFc.TabIndex = 8;
            this.btnChangeLightFc.Text = "FC照明値に変更";
            this.btnChangeLightFc.UseVisualStyleBackColor = true;
            this.btnChangeLightFc.Click += new System.EventHandler(this.btnChangeLightFc_Click);
            // 
            // btnEntryFcBase
            // 
            this.btnEntryFcBase.Location = new System.Drawing.Point(73, 18);
            this.btnEntryFcBase.Name = "btnEntryFcBase";
            this.btnEntryFcBase.Size = new System.Drawing.Size(37, 23);
            this.btnEntryFcBase.TabIndex = 8;
            this.btnEntryFcBase.Text = "登録";
            this.btnEntryFcBase.UseVisualStyleBackColor = true;
            this.btnEntryFcBase.Click += new System.EventHandler(this.btnEntryFcBase_Click);
            // 
            // grpWhiteB
            // 
            this.grpWhiteB.Controls.Add(this.btnChangeLightWhiteB);
            this.grpWhiteB.Controls.Add(this.chkAllImage);
            this.grpWhiteB.Controls.Add(this.btnResetWhiteB);
            this.grpWhiteB.Controls.Add(this.btnEntryWhiteB);
            this.grpWhiteB.Controls.Add(this.btnRunWhiteB);
            this.grpWhiteB.Controls.Add(this.btnWhiteBalanceRoi);
            this.grpWhiteB.Location = new System.Drawing.Point(5, 157);
            this.grpWhiteB.Name = "grpWhiteB";
            this.grpWhiteB.Size = new System.Drawing.Size(355, 52);
            this.grpWhiteB.TabIndex = 8;
            this.grpWhiteB.TabStop = false;
            this.grpWhiteB.Text = "ﾎﾜｲﾄﾊﾞﾗﾝｽ";
            // 
            // btnChangeLightWhiteB
            // 
            this.btnChangeLightWhiteB.Location = new System.Drawing.Point(247, 18);
            this.btnChangeLightWhiteB.Name = "btnChangeLightWhiteB";
            this.btnChangeLightWhiteB.Size = new System.Drawing.Size(102, 23);
            this.btnChangeLightWhiteB.TabIndex = 8;
            this.btnChangeLightWhiteB.Text = "WB照明値に変更";
            this.btnChangeLightWhiteB.UseVisualStyleBackColor = true;
            this.btnChangeLightWhiteB.Click += new System.EventHandler(this.btnChangeLightWhiteB_Click);
            // 
            // chkAllImage
            // 
            this.chkAllImage.AutoSize = true;
            this.chkAllImage.Location = new System.Drawing.Point(73, 23);
            this.chkAllImage.Name = "chkAllImage";
            this.chkAllImage.Size = new System.Drawing.Size(15, 14);
            this.chkAllImage.TabIndex = 9;
            this.chkAllImage.UseVisualStyleBackColor = true;
            this.chkAllImage.CheckedChanged += new System.EventHandler(this.chkAllImage_CheckedChanged);
            // 
            // btnResetWhiteB
            // 
            this.btnResetWhiteB.Location = new System.Drawing.Point(191, 18);
            this.btnResetWhiteB.Name = "btnResetWhiteB";
            this.btnResetWhiteB.Size = new System.Drawing.Size(39, 23);
            this.btnResetWhiteB.TabIndex = 8;
            this.btnResetWhiteB.Text = "ﾘｾｯﾄ";
            this.btnResetWhiteB.UseVisualStyleBackColor = true;
            this.btnResetWhiteB.Click += new System.EventHandler(this.btnResetWhiteB_Click);
            // 
            // btnEntryWhiteB
            // 
            this.btnEntryWhiteB.Location = new System.Drawing.Point(148, 18);
            this.btnEntryWhiteB.Name = "btnEntryWhiteB";
            this.btnEntryWhiteB.Size = new System.Drawing.Size(37, 23);
            this.btnEntryWhiteB.TabIndex = 8;
            this.btnEntryWhiteB.Text = "登録";
            this.btnEntryWhiteB.UseVisualStyleBackColor = true;
            this.btnEntryWhiteB.Click += new System.EventHandler(this.btnEntryWhiteB_Click);
            // 
            // btnRunWhiteB
            // 
            this.btnRunWhiteB.Location = new System.Drawing.Point(102, 18);
            this.btnRunWhiteB.Name = "btnRunWhiteB";
            this.btnRunWhiteB.Size = new System.Drawing.Size(40, 23);
            this.btnRunWhiteB.TabIndex = 7;
            this.btnRunWhiteB.Text = "実行";
            this.btnRunWhiteB.UseVisualStyleBackColor = true;
            this.btnRunWhiteB.Click += new System.EventHandler(this.btnRunWhiteB_Click);
            // 
            // btnWhiteBalanceRoi
            // 
            this.btnWhiteBalanceRoi.Location = new System.Drawing.Point(6, 18);
            this.btnWhiteBalanceRoi.Name = "btnWhiteBalanceRoi";
            this.btnWhiteBalanceRoi.Size = new System.Drawing.Size(61, 23);
            this.btnWhiteBalanceRoi.TabIndex = 7;
            this.btnWhiteBalanceRoi.Text = "領域設定";
            this.btnWhiteBalanceRoi.UseVisualStyleBackColor = true;
            this.btnWhiteBalanceRoi.Click += new System.EventHandler(this.btnWhiteBalanceRoi_Click);
            // 
            // uclCameraControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpWhiteB);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkCamera);
            this.Controls.Add(this.trbExposure);
            this.Controls.Add(this.trbOffset);
            this.Controls.Add(this.trbGain);
            this.Controls.Add(this.nudLineRate);
            this.Controls.Add(this.nudTriggerDelay);
            this.Controls.Add(this.nudExposure);
            this.Controls.Add(this.nudOffset);
            this.Controls.Add(this.nudGain);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblWidth);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblFrameRate);
            this.Controls.Add(this.lblLineRate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label9);
            this.Name = "uclCameraControl";
            this.Size = new System.Drawing.Size(363, 220);
            ((System.ComponentModel.ISupportInitialize)(this.nudGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudExposure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbExposure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTriggerDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineRate)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.grpWhiteB.ResumeLayout(false);
            this.grpWhiteB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudGain;
        private System.Windows.Forms.NumericUpDown nudOffset;
        private System.Windows.Forms.NumericUpDown nudExposure;
        private System.Windows.Forms.TrackBar trbGain;
        private System.Windows.Forms.TrackBar trbOffset;
        private System.Windows.Forms.TrackBar trbExposure;
        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblFrameRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblLineRate;
        private System.Windows.Forms.CheckBox chkCamera;
        private System.Windows.Forms.NumericUpDown nudTriggerDelay;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudLineRate;
        private System.Windows.Forms.Button btnFcRoi;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnEntryFcBase;
        private System.Windows.Forms.GroupBox grpWhiteB;
        private System.Windows.Forms.Button btnEntryWhiteB;
        private System.Windows.Forms.Button btnRunWhiteB;
        private System.Windows.Forms.Button btnWhiteBalanceRoi;
        private System.Windows.Forms.Button btnResetWhiteB;
        private System.Windows.Forms.CheckBox chkAllImage;
        private System.Windows.Forms.Button btnChangeLightFc;
        private System.Windows.Forms.Button btnChangeLightWhiteB;
    }
}
