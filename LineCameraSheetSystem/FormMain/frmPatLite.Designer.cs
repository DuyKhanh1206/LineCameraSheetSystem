namespace LineCameraSheetSystem
{
    partial class frmPatLite
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblPatLiteDelay = new System.Windows.Forms.Label();
            this.spinPatLiteDelay = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.chkPatLiteEnable = new System.Windows.Forms.CheckBox();
            this.lblOnTime = new System.Windows.Forms.Label();
            this.spinPatLiteOnTime = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.lblMaskUse = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnCancel.Location = new System.Drawing.Point(248, 127);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 60);
            this.btnCancel.TabIndex = 36;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnOK.Location = new System.Drawing.Point(36, 128);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(150, 60);
            this.btnOK.TabIndex = 37;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblMaskUse);
            this.groupBox1.Controls.Add(this.lblOnTime);
            this.groupBox1.Controls.Add(this.spinPatLiteOnTime);
            this.groupBox1.Controls.Add(this.lblPatLiteDelay);
            this.groupBox1.Controls.Add(this.spinPatLiteDelay);
            this.groupBox1.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(406, 105);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            // 
            // lblPatLiteDelay
            // 
            this.lblPatLiteDelay.AutoSize = true;
            this.lblPatLiteDelay.Location = new System.Drawing.Point(6, 27);
            this.lblPatLiteDelay.Name = "lblPatLiteDelay";
            this.lblPatLiteDelay.Size = new System.Drawing.Size(144, 24);
            this.lblPatLiteDelay.TabIndex = 21;
            this.lblPatLiteDelay.Text = "ﾃﾞｨﾚｲ時間[s]";
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
            this.spinPatLiteDelay.Location = new System.Drawing.Point(10, 56);
            this.spinPatLiteDelay.Margin = new System.Windows.Forms.Padding(0);
            this.spinPatLiteDelay.Maximum = new decimal(new int[] {
            1800,
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
            this.spinPatLiteDelay.Size = new System.Drawing.Size(180, 44);
            this.spinPatLiteDelay.TabIndex = 39;
            this.spinPatLiteDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinPatLiteDelay.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // chkPatLiteEnable
            // 
            this.chkPatLiteEnable.AutoSize = true;
            this.chkPatLiteEnable.Location = new System.Drawing.Point(22, 12);
            this.chkPatLiteEnable.Name = "chkPatLiteEnable";
            this.chkPatLiteEnable.Size = new System.Drawing.Size(134, 28);
            this.chkPatLiteEnable.TabIndex = 40;
            this.chkPatLiteEnable.Text = "有効にする";
            this.chkPatLiteEnable.UseVisualStyleBackColor = true;
            // 
            // lblOnTime
            // 
            this.lblOnTime.AutoSize = true;
            this.lblOnTime.Location = new System.Drawing.Point(202, 27);
            this.lblOnTime.Name = "lblOnTime";
            this.lblOnTime.Size = new System.Drawing.Size(133, 24);
            this.lblOnTime.TabIndex = 40;
            this.lblOnTime.Text = "出力時間[s]";
            // 
            // spinPatLiteOnTime
            // 
            this.spinPatLiteOnTime.DecimalPlaces = 0;
            this.spinPatLiteOnTime.EveryValueChanged = false;
            this.spinPatLiteOnTime.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spinPatLiteOnTime.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinPatLiteOnTime.Location = new System.Drawing.Point(206, 56);
            this.spinPatLiteOnTime.Margin = new System.Windows.Forms.Padding(0);
            this.spinPatLiteOnTime.Maximum = new decimal(new int[] {
            1800,
            0,
            0,
            0});
            this.spinPatLiteOnTime.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinPatLiteOnTime.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinPatLiteOnTime.Name = "spinPatLiteOnTime";
            this.spinPatLiteOnTime.Size = new System.Drawing.Size(180, 44);
            this.spinPatLiteOnTime.TabIndex = 41;
            this.spinPatLiteOnTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinPatLiteOnTime.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lblMaskUse
            // 
            this.lblMaskUse.BackColor = System.Drawing.SystemColors.Control;
            this.lblMaskUse.Location = new System.Drawing.Point(202, 18);
            this.lblMaskUse.Name = "lblMaskUse";
            this.lblMaskUse.Size = new System.Drawing.Size(198, 82);
            this.lblMaskUse.TabIndex = 42;
            this.lblMaskUse.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblMaskUse_MouseDown);
            // 
            // frmPatLite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 199);
            this.ControlBox = false;
            this.Controls.Add(this.chkPatLiteEnable);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "frmPatLite";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "１Ｆパトライト設定";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblPatLiteDelay;
        private Fujita.InspectionSystem.uclNumericInputSmall spinPatLiteDelay;
        private System.Windows.Forms.CheckBox chkPatLiteEnable;
        private System.Windows.Forms.Label lblOnTime;
        private Fujita.InspectionSystem.uclNumericInputSmall spinPatLiteOnTime;
        private System.Windows.Forms.Label lblMaskUse;
    }
}