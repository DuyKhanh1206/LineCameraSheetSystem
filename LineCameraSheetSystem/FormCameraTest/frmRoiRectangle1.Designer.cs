namespace Fujita.InspectionSystem
{
    partial class frmRoiRectangle1
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
			this.lblMessage = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.uniRow1 = new Fujita.InspectionSystem.uclNumericInput();
			this.uniRow2 = new Fujita.InspectionSystem.uclNumericInput();
			this.uniCol1 = new Fujita.InspectionSystem.uclNumericInput();
			this.uniCol2 = new Fujita.InspectionSystem.uclNumericInput();
			this.SuspendLayout();
			// 
			// lblMessage
			// 
			this.lblMessage.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblMessage.Location = new System.Drawing.Point(2, -1);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(411, 76);
			this.lblMessage.TabIndex = 1;
			this.lblMessage.Text = "label1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(71, 149);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 24);
			this.label1.TabIndex = 2;
			this.label1.Text = "上:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Location = new System.Drawing.Point(71, 83);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(39, 24);
			this.label2.TabIndex = 2;
			this.label2.Text = "左:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label3.Location = new System.Drawing.Point(71, 286);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(39, 24);
			this.label3.TabIndex = 2;
			this.label3.Text = "下:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label4.Location = new System.Drawing.Point(71, 216);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(39, 24);
			this.label4.TabIndex = 2;
			this.label4.Text = "右:";
			// 
			// btnOK
			// 
			this.btnOK.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnOK.Location = new System.Drawing.Point(26, 349);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(166, 51);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnCancel.Location = new System.Drawing.Point(238, 349);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(164, 51);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// uniRow1
			// 
			this.uniRow1.DecimalPlaces = 0;
			this.uniRow1.EveryValueChanged = false;
			this.uniRow1.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.uniRow1.Location = new System.Drawing.Point(130, 140);
			this.uniRow1.Margin = new System.Windows.Forms.Padding(0);
			this.uniRow1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.uniRow1.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.uniRow1.MinimumSize = new System.Drawing.Size(124, 56);
			this.uniRow1.Name = "uniRow1";
			this.uniRow1.Size = new System.Drawing.Size(221, 56);
			this.uniRow1.TabIndex = 4;
			this.uniRow1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.uniRow1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// uniRow2
			// 
			this.uniRow2.DecimalPlaces = 0;
			this.uniRow2.EveryValueChanged = false;
			this.uniRow2.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.uniRow2.Location = new System.Drawing.Point(130, 277);
			this.uniRow2.Margin = new System.Windows.Forms.Padding(0);
			this.uniRow2.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.uniRow2.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.uniRow2.MinimumSize = new System.Drawing.Size(124, 56);
			this.uniRow2.Name = "uniRow2";
			this.uniRow2.Size = new System.Drawing.Size(221, 56);
			this.uniRow2.TabIndex = 4;
			this.uniRow2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.uniRow2.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// uniCol1
			// 
			this.uniCol1.DecimalPlaces = 0;
			this.uniCol1.EveryValueChanged = false;
			this.uniCol1.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.uniCol1.Location = new System.Drawing.Point(130, 74);
			this.uniCol1.Margin = new System.Windows.Forms.Padding(0);
			this.uniCol1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.uniCol1.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.uniCol1.MinimumSize = new System.Drawing.Size(124, 56);
			this.uniCol1.Name = "uniCol1";
			this.uniCol1.Size = new System.Drawing.Size(221, 56);
			this.uniCol1.TabIndex = 4;
			this.uniCol1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.uniCol1.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// uniCol2
			// 
			this.uniCol2.DecimalPlaces = 0;
			this.uniCol2.EveryValueChanged = false;
			this.uniCol2.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.uniCol2.Location = new System.Drawing.Point(130, 207);
			this.uniCol2.Margin = new System.Windows.Forms.Padding(0);
			this.uniCol2.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.uniCol2.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.uniCol2.MinimumSize = new System.Drawing.Size(124, 56);
			this.uniCol2.Name = "uniCol2";
			this.uniCol2.Size = new System.Drawing.Size(221, 56);
			this.uniCol2.TabIndex = 4;
			this.uniCol2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.uniCol2.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// frmRoiRectangle1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(414, 412);
			this.ControlBox = false;
			this.Controls.Add(this.uniCol2);
			this.Controls.Add(this.uniCol1);
			this.Controls.Add(this.uniRow2);
			this.Controls.Add(this.uniRow1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblMessage);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmRoiRectangle1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "矩形選択";
			this.Load += new System.EventHandler(this.frmRoiRectangle1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private uclNumericInput uniRow1;
        private uclNumericInput uniRow2;
        private uclNumericInput uniCol1;
        private uclNumericInput uniCol2;
    }
}