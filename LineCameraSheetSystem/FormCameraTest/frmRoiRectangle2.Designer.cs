namespace Fujita.InspectionSystem
{
    partial class frmRoiRectangle2
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
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.nudCol = new System.Windows.Forms.NumericUpDown();
			this.nudRow = new System.Windows.Forms.NumericUpDown();
			this.nudLen1 = new System.Windows.Forms.NumericUpDown();
			this.nudLen2 = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.nudPhi = new System.Windows.Forms.NumericUpDown();
			this.label5 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudCol)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRow)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudLen1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudLen2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPhi)).BeginInit();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Location = new System.Drawing.Point(205, 99);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(43, 24);
			this.label2.TabIndex = 6;
			this.label2.Text = "Col";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(11, 99);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 24);
			this.label1.TabIndex = 7;
			this.label1.Text = "Row";
			// 
			// lblMessage
			// 
			this.lblMessage.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblMessage.Location = new System.Drawing.Point(2, 3);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(411, 76);
			this.lblMessage.TabIndex = 5;
			this.lblMessage.Text = "label1";
			// 
			// nudCol
			// 
			this.nudCol.DecimalPlaces = 2;
			this.nudCol.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nudCol.Location = new System.Drawing.Point(276, 90);
			this.nudCol.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.nudCol.Name = "nudCol";
			this.nudCol.Size = new System.Drawing.Size(120, 39);
			this.nudCol.TabIndex = 3;
			// 
			// nudRow
			// 
			this.nudRow.DecimalPlaces = 2;
			this.nudRow.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nudRow.Location = new System.Drawing.Point(79, 90);
			this.nudRow.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.nudRow.Name = "nudRow";
			this.nudRow.Size = new System.Drawing.Size(120, 39);
			this.nudRow.TabIndex = 4;
			// 
			// nudLen1
			// 
			this.nudLen1.DecimalPlaces = 2;
			this.nudLen1.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nudLen1.Location = new System.Drawing.Point(79, 199);
			this.nudLen1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.nudLen1.Name = "nudLen1";
			this.nudLen1.Size = new System.Drawing.Size(120, 39);
			this.nudLen1.TabIndex = 4;
			// 
			// nudLen2
			// 
			this.nudLen2.DecimalPlaces = 2;
			this.nudLen2.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nudLen2.Location = new System.Drawing.Point(276, 199);
			this.nudLen2.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.nudLen2.Name = "nudLen2";
			this.nudLen2.Size = new System.Drawing.Size(120, 39);
			this.nudLen2.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label3.Location = new System.Drawing.Point(11, 208);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(59, 24);
			this.label3.TabIndex = 7;
			this.label3.Text = "Len1";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label4.Location = new System.Drawing.Point(205, 208);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(59, 24);
			this.label4.TabIndex = 6;
			this.label4.Text = "Len2";
			// 
			// nudPhi
			// 
			this.nudPhi.DecimalPlaces = 2;
			this.nudPhi.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nudPhi.Location = new System.Drawing.Point(79, 144);
			this.nudPhi.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
			this.nudPhi.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            -2147483648});
			this.nudPhi.Name = "nudPhi";
			this.nudPhi.Size = new System.Drawing.Size(120, 39);
			this.nudPhi.TabIndex = 4;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label5.Location = new System.Drawing.Point(11, 153);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(42, 24);
			this.label5.TabIndex = 7;
			this.label5.Text = "Phi";
			// 
			// btnCancel
			// 
			this.btnCancel.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnCancel.Location = new System.Drawing.Point(232, 259);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(164, 51);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnOK.Location = new System.Drawing.Point(33, 259);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(166, 51);
			this.btnOK.TabIndex = 8;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// frmRoiRectangle2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(417, 326);
			this.ControlBox = false;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.nudLen2);
			this.Controls.Add(this.nudPhi);
			this.Controls.Add(this.nudLen1);
			this.Controls.Add(this.nudCol);
			this.Controls.Add(this.nudRow);
			this.Name = "frmRoiRectangle2";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "矩形選択 ";
			this.Load += new System.EventHandler(this.frmRoiRectangle2_Load);
			((System.ComponentModel.ISupportInitialize)(this.nudCol)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRow)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudLen1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudLen2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPhi)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.NumericUpDown nudCol;
        private System.Windows.Forms.NumericUpDown nudRow;
        private System.Windows.Forms.NumericUpDown nudLen1;
        private System.Windows.Forms.NumericUpDown nudLen2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudPhi;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}