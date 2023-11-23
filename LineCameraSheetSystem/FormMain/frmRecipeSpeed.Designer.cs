namespace LineCameraSheetSystem
{
    partial class frmRecipeSpeed
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.grpExtOutput = new System.Windows.Forms.GroupBox();
            this.spinCamSpeedUra = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.lblCamExpUra = new System.Windows.Forms.Label();
            this.spinCamSpeed = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.lblCamHzUra = new System.Windows.Forms.Label();
            this.lblCamExp = new System.Windows.Forms.Label();
            this.lblCamHz = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkSpeedEnable = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.spinCamExposureUra = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinCamExposure = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.label8 = new System.Windows.Forms.Label();
            this.chkExposureEnable = new System.Windows.Forms.CheckBox();
            this.grpExtOutput.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(447, 303);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 60);
            this.btnCancel.TabIndex = 38;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(12, 303);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(150, 60);
            this.btnOK.TabIndex = 37;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grpExtOutput
            // 
            this.grpExtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpExtOutput.Controls.Add(this.spinCamSpeedUra);
            this.grpExtOutput.Controls.Add(this.lblCamExpUra);
            this.grpExtOutput.Controls.Add(this.spinCamSpeed);
            this.grpExtOutput.Controls.Add(this.lblCamHzUra);
            this.grpExtOutput.Controls.Add(this.lblCamExp);
            this.grpExtOutput.Controls.Add(this.lblCamHz);
            this.grpExtOutput.Controls.Add(this.label4);
            this.grpExtOutput.Controls.Add(this.label10);
            this.grpExtOutput.Controls.Add(this.label2);
            this.grpExtOutput.Controls.Add(this.label3);
            this.grpExtOutput.Controls.Add(this.label1);
            this.grpExtOutput.Controls.Add(this.label6);
            this.grpExtOutput.Controls.Add(this.label5);
            this.grpExtOutput.Controls.Add(this.chkSpeedEnable);
            this.grpExtOutput.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.grpExtOutput.Location = new System.Drawing.Point(12, 12);
            this.grpExtOutput.Name = "grpExtOutput";
            this.grpExtOutput.Size = new System.Drawing.Size(585, 180);
            this.grpExtOutput.TabIndex = 36;
            this.grpExtOutput.TabStop = false;
            // 
            // spinCamSpeedUra
            // 
            this.spinCamSpeedUra.DecimalPlaces = 1;
            this.spinCamSpeedUra.EveryValueChanged = false;
            this.spinCamSpeedUra.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spinCamSpeedUra.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.spinCamSpeedUra.Location = new System.Drawing.Point(369, 57);
            this.spinCamSpeedUra.Margin = new System.Windows.Forms.Padding(0);
            this.spinCamSpeedUra.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.spinCamSpeedUra.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinCamSpeedUra.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinCamSpeedUra.Name = "spinCamSpeedUra";
            this.spinCamSpeedUra.Size = new System.Drawing.Size(199, 44);
            this.spinCamSpeedUra.TabIndex = 44;
            this.spinCamSpeedUra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinCamSpeedUra.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinCamSpeedUra.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinCamSpeedUra_ValueChanged);
            this.spinCamSpeedUra.Load += new System.EventHandler(this.spinCamSpeed_Load);
            // 
            // lblCamExpUra
            // 
            this.lblCamExpUra.AutoSize = true;
            this.lblCamExpUra.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCamExpUra.Location = new System.Drawing.Point(493, 137);
            this.lblCamExpUra.Name = "lblCamExpUra";
            this.lblCamExpUra.Size = new System.Drawing.Size(81, 24);
            this.lblCamExpUra.TabIndex = 41;
            this.lblCamExpUra.Text = "xxxxx.x";
            // 
            // spinCamSpeed
            // 
            this.spinCamSpeed.DecimalPlaces = 1;
            this.spinCamSpeed.EveryValueChanged = false;
            this.spinCamSpeed.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spinCamSpeed.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.spinCamSpeed.Location = new System.Drawing.Point(135, 57);
            this.spinCamSpeed.Margin = new System.Windows.Forms.Padding(0);
            this.spinCamSpeed.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.spinCamSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinCamSpeed.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinCamSpeed.Name = "spinCamSpeed";
            this.spinCamSpeed.Size = new System.Drawing.Size(199, 44);
            this.spinCamSpeed.TabIndex = 44;
            this.spinCamSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinCamSpeed.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinCamSpeed.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinCamSpeed_ValueChanged);
            this.spinCamSpeed.Load += new System.EventHandler(this.spinCamSpeed_Load);
            // 
            // lblCamHzUra
            // 
            this.lblCamHzUra.AutoSize = true;
            this.lblCamHzUra.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCamHzUra.Location = new System.Drawing.Point(493, 104);
            this.lblCamHzUra.Name = "lblCamHzUra";
            this.lblCamHzUra.Size = new System.Drawing.Size(81, 24);
            this.lblCamHzUra.TabIndex = 41;
            this.lblCamHzUra.Text = "xxxxx.x";
            // 
            // lblCamExp
            // 
            this.lblCamExp.AutoSize = true;
            this.lblCamExp.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCamExp.Location = new System.Drawing.Point(259, 137);
            this.lblCamExp.Name = "lblCamExp";
            this.lblCamExp.Size = new System.Drawing.Size(81, 24);
            this.lblCamExp.TabIndex = 41;
            this.lblCamExp.Text = "xxxxx.x";
            // 
            // lblCamHz
            // 
            this.lblCamHz.AutoSize = true;
            this.lblCamHz.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCamHz.Location = new System.Drawing.Point(259, 104);
            this.lblCamHz.Name = "lblCamHz";
            this.lblCamHz.Size = new System.Drawing.Size(81, 24);
            this.lblCamHz.TabIndex = 41;
            this.lblCamHz.Text = "xxxxx.x";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(392, 137);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 24);
            this.label4.TabIndex = 42;
            this.label4.Text = "露光(us)";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label10.Location = new System.Drawing.Point(444, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(34, 24);
            this.label10.TabIndex = 42;
            this.label10.Text = "裏";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(210, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 24);
            this.label2.TabIndex = 42;
            this.label2.Text = "表";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(365, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 24);
            this.label3.TabIndex = 42;
            this.label3.Text = "周波数(Hz)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(158, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 24);
            this.label1.TabIndex = 42;
            this.label1.Text = "露光(us)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(131, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(122, 24);
            this.label6.TabIndex = 42;
            this.label6.Text = "周波数(Hz)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 24);
            this.label5.TabIndex = 43;
            this.label5.Text = "速度(m/分)";
            // 
            // chkSpeedEnable
            // 
            this.chkSpeedEnable.AutoSize = true;
            this.chkSpeedEnable.Location = new System.Drawing.Point(10, 0);
            this.chkSpeedEnable.Name = "chkSpeedEnable";
            this.chkSpeedEnable.Size = new System.Drawing.Size(134, 28);
            this.chkSpeedEnable.TabIndex = 2;
            this.chkSpeedEnable.Text = "有効にする";
            this.chkSpeedEnable.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.spinCamExposureUra);
            this.groupBox1.Controls.Add(this.spinCamExposure);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.chkExposureEnable);
            this.groupBox1.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(12, 199);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(585, 89);
            this.groupBox1.TabIndex = 36;
            this.groupBox1.TabStop = false;
            // 
            // spinCamExposureUra
            // 
            this.spinCamExposureUra.DecimalPlaces = 1;
            this.spinCamExposureUra.EveryValueChanged = false;
            this.spinCamExposureUra.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spinCamExposureUra.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinCamExposureUra.Location = new System.Drawing.Point(369, 31);
            this.spinCamExposureUra.Margin = new System.Windows.Forms.Padding(0);
            this.spinCamExposureUra.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.spinCamExposureUra.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinCamExposureUra.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinCamExposureUra.Name = "spinCamExposureUra";
            this.spinCamExposureUra.Size = new System.Drawing.Size(199, 44);
            this.spinCamExposureUra.TabIndex = 44;
            this.spinCamExposureUra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinCamExposureUra.Value = new decimal(new int[] {
            190,
            0,
            0,
            0});
            this.spinCamExposureUra.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinCamSpeed_ValueChanged);
            this.spinCamExposureUra.Load += new System.EventHandler(this.spinCamSpeed_Load);
            // 
            // spinCamExposure
            // 
            this.spinCamExposure.DecimalPlaces = 1;
            this.spinCamExposure.EveryValueChanged = false;
            this.spinCamExposure.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spinCamExposure.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinCamExposure.Location = new System.Drawing.Point(135, 31);
            this.spinCamExposure.Margin = new System.Windows.Forms.Padding(0);
            this.spinCamExposure.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.spinCamExposure.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinCamExposure.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinCamExposure.Name = "spinCamExposure";
            this.spinCamExposure.Size = new System.Drawing.Size(199, 44);
            this.spinCamExposure.TabIndex = 44;
            this.spinCamExposure.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinCamExposure.Value = new decimal(new int[] {
            190,
            0,
            0,
            0});
            this.spinCamExposure.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinCamSpeed_ValueChanged);
            this.spinCamExposure.Load += new System.EventHandler(this.spinCamSpeed_Load);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 24);
            this.label8.TabIndex = 43;
            this.label8.Text = "露光値(us)";
            // 
            // chkExposureEnable
            // 
            this.chkExposureEnable.AutoSize = true;
            this.chkExposureEnable.Location = new System.Drawing.Point(10, 0);
            this.chkExposureEnable.Name = "chkExposureEnable";
            this.chkExposureEnable.Size = new System.Drawing.Size(134, 28);
            this.chkExposureEnable.TabIndex = 2;
            this.chkExposureEnable.Text = "有効にする";
            this.chkExposureEnable.UseVisualStyleBackColor = true;
            // 
            // frmRecipeSpeed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 375);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpExtOutput);
            this.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "frmRecipeSpeed";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "速度設定";
            this.grpExtOutput.ResumeLayout(false);
            this.grpExtOutput.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox grpExtOutput;
        private System.Windows.Forms.CheckBox chkSpeedEnable;
        private Fujita.InspectionSystem.uclNumericInputSmall spinCamSpeed;
        private System.Windows.Forms.Label lblCamHz;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblCamExp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private Fujita.InspectionSystem.uclNumericInputSmall spinCamExposure;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkExposureEnable;
        private Fujita.InspectionSystem.uclNumericInputSmall spinCamSpeedUra;
        private System.Windows.Forms.Label lblCamExpUra;
        private System.Windows.Forms.Label lblCamHzUra;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Fujita.InspectionSystem.uclNumericInputSmall spinCamExposureUra;
    }
}