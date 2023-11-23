namespace Fujita.InspectionSystem
{
    partial class frmRoiCircle
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
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.uniRad = new Fujita.InspectionSystem.uclNumericInput();
			this.uniRow = new Fujita.InspectionSystem.uclNumericInput();
			this.uniCol = new Fujita.InspectionSystem.uclNumericInput();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblMessage
			// 
			this.lblMessage.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblMessage.Location = new System.Drawing.Point(1, 4);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(411, 76);
			this.lblMessage.TabIndex = 2;
			this.lblMessage.Text = "label1";
			// 
			// btnCancel
			// 
			this.btnCancel.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnCancel.Location = new System.Drawing.Point(232, 349);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(164, 51);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnOK.Location = new System.Drawing.Point(20, 349);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(166, 51);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// uniRad
			// 
			this.uniRad.DecimalPlaces = 0;
			this.uniRad.EveryValueChanged = false;
			this.uniRad.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.uniRad.Location = new System.Drawing.Point(126, 245);
			this.uniRad.Margin = new System.Windows.Forms.Padding(0);
			this.uniRad.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.uniRad.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.uniRad.MinimumSize = new System.Drawing.Size(124, 56);
			this.uniRad.Name = "uniRad";
			this.uniRad.Size = new System.Drawing.Size(221, 56);
			this.uniRad.TabIndex = 9;
			this.uniRad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.uniRad.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// uniRow
			// 
			this.uniRow.DecimalPlaces = 0;
			this.uniRow.EveryValueChanged = false;
			this.uniRow.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.uniRow.Location = new System.Drawing.Point(126, 112);
			this.uniRow.Margin = new System.Windows.Forms.Padding(0);
			this.uniRow.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.uniRow.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.uniRow.MinimumSize = new System.Drawing.Size(124, 56);
			this.uniRow.Name = "uniRow";
			this.uniRow.Size = new System.Drawing.Size(221, 56);
			this.uniRow.TabIndex = 10;
			this.uniRow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.uniRow.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// uniCol
			// 
			this.uniCol.DecimalPlaces = 0;
			this.uniCol.EveryValueChanged = false;
			this.uniCol.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.uniCol.Location = new System.Drawing.Point(126, 178);
			this.uniCol.Margin = new System.Windows.Forms.Padding(0);
			this.uniCol.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.uniCol.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
			this.uniCol.MinimumSize = new System.Drawing.Size(124, 56);
			this.uniCol.Name = "uniCol";
			this.uniCol.Size = new System.Drawing.Size(221, 56);
			this.uniCol.TabIndex = 11;
			this.uniCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.uniCol.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label4.Location = new System.Drawing.Point(40, 254);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(81, 24);
			this.label4.TabIndex = 7;
			this.label4.Text = "Radius:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Location = new System.Drawing.Point(65, 124);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(58, 24);
			this.label2.TabIndex = 6;
			this.label2.Text = "Row:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(33, 191);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(90, 24);
			this.label1.TabIndex = 8;
			this.label1.Text = "Column:";
			// 
			// frmRoiCircle
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(414, 409);
			this.ControlBox = false;
			this.Controls.Add(this.uniRad);
			this.Controls.Add(this.uniRow);
			this.Controls.Add(this.uniCol);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lblMessage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximumSize = new System.Drawing.Size(420, 437);
			this.MinimumSize = new System.Drawing.Size(420, 437);
			this.Name = "frmRoiCircle";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "円選択";
			this.Load += new System.EventHandler(this.frmRoiCircle_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private uclNumericInput uniRad;
        private uclNumericInput uniRow;
        private uclNumericInput uniCol;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}