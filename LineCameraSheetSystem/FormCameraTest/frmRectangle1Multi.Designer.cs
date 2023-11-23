namespace Fujita.InspectionSystem
{
    partial class frmRectangle1Multi
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
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblMessage = new System.Windows.Forms.Label();
			this.nudCol2 = new System.Windows.Forms.NumericUpDown();
			this.nudRow2 = new System.Windows.Forms.NumericUpDown();
			this.nudCol1 = new System.Windows.Forms.NumericUpDown();
			this.nudRow1 = new System.Windows.Forms.NumericUpDown();
			this.btnAddLast = new System.Windows.Forms.Button();
			this.btnAddNext = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.lstPoints = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			((System.ComponentModel.ISupportInitialize)(this.nudCol2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRow2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudCol1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRow1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnCancel.Location = new System.Drawing.Point(233, 509);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(164, 51);
			this.btnCancel.TabIndex = 13;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnOK.Location = new System.Drawing.Point(34, 509);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(166, 51);
			this.btnOK.TabIndex = 14;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label4.Location = new System.Drawing.Point(206, 158);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(55, 24);
			this.label4.TabIndex = 10;
			this.label4.Text = "Col2";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label3.Location = new System.Drawing.Point(12, 158);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(65, 24);
			this.label3.TabIndex = 11;
			this.label3.Text = "Row2";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Location = new System.Drawing.Point(206, 102);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 24);
			this.label2.TabIndex = 12;
			this.label2.Text = "Col1";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(12, 102);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(65, 24);
			this.label1.TabIndex = 9;
			this.label1.Text = "Row1";
			// 
			// lblMessage
			// 
			this.lblMessage.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblMessage.Location = new System.Drawing.Point(2, 3);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(411, 76);
			this.lblMessage.TabIndex = 8;
			this.lblMessage.Text = "label1";
			// 
			// nudCol2
			// 
			this.nudCol2.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nudCol2.Location = new System.Drawing.Point(277, 149);
			this.nudCol2.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.nudCol2.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
			this.nudCol2.Name = "nudCol2";
			this.nudCol2.Size = new System.Drawing.Size(120, 39);
			this.nudCol2.TabIndex = 5;
			this.nudCol2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// nudRow2
			// 
			this.nudRow2.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nudRow2.Location = new System.Drawing.Point(80, 149);
			this.nudRow2.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.nudRow2.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
			this.nudRow2.Name = "nudRow2";
			this.nudRow2.Size = new System.Drawing.Size(120, 39);
			this.nudRow2.TabIndex = 4;
			this.nudRow2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// nudCol1
			// 
			this.nudCol1.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nudCol1.Location = new System.Drawing.Point(277, 93);
			this.nudCol1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.nudCol1.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
			this.nudCol1.Name = "nudCol1";
			this.nudCol1.Size = new System.Drawing.Size(120, 39);
			this.nudCol1.TabIndex = 7;
			this.nudCol1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// nudRow1
			// 
			this.nudRow1.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nudRow1.Location = new System.Drawing.Point(80, 93);
			this.nudRow1.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.nudRow1.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
			this.nudRow1.Name = "nudRow1";
			this.nudRow1.Size = new System.Drawing.Size(120, 39);
			this.nudRow1.TabIndex = 6;
			this.nudRow1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnAddLast
			// 
			this.btnAddLast.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnAddLast.Location = new System.Drawing.Point(292, 459);
			this.btnAddLast.Name = "btnAddLast";
			this.btnAddLast.Size = new System.Drawing.Size(114, 36);
			this.btnAddLast.TabIndex = 16;
			this.btnAddLast.Text = "最後に追加";
			this.btnAddLast.UseVisualStyleBackColor = true;
			this.btnAddLast.Click += new System.EventHandler(this.btnAddLast_Click);
			// 
			// btnAddNext
			// 
			this.btnAddNext.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnAddNext.Location = new System.Drawing.Point(163, 459);
			this.btnAddNext.Name = "btnAddNext";
			this.btnAddNext.Size = new System.Drawing.Size(114, 36);
			this.btnAddNext.TabIndex = 18;
			this.btnAddNext.Text = "追加";
			this.btnAddNext.UseVisualStyleBackColor = true;
			this.btnAddNext.Click += new System.EventHandler(this.btnAddNext_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnDelete.Location = new System.Drawing.Point(16, 459);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(114, 36);
			this.btnDelete.TabIndex = 17;
			this.btnDelete.Text = "削除";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// lstPoints
			// 
			this.lstPoints.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
			this.lstPoints.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lstPoints.FullRowSelect = true;
			this.lstPoints.GridLines = true;
			this.lstPoints.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstPoints.HideSelection = false;
			this.lstPoints.Location = new System.Drawing.Point(16, 203);
			this.lstPoints.MultiSelect = false;
			this.lstPoints.Name = "lstPoints";
			this.lstPoints.Size = new System.Drawing.Size(390, 250);
			this.lstPoints.TabIndex = 15;
			this.lstPoints.UseCompatibleStateImageBehavior = false;
			this.lstPoints.View = System.Windows.Forms.View.Details;
			this.lstPoints.SelectedIndexChanged += new System.EventHandler(this.lstPoints_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "No.";
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Row1";
			this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader2.Width = 80;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Col1";
			this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader3.Width = 80;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Row2";
			this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader4.Width = 80;
			// 
			// columnHeader5
			// 
			this.columnHeader5.Text = "Col2";
			this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader5.Width = 80;
			// 
			// frmRectangle1Multi
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(414, 573);
			this.Controls.Add(this.btnAddLast);
			this.Controls.Add(this.btnAddNext);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.lstPoints);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.nudCol2);
			this.Controls.Add(this.nudRow2);
			this.Controls.Add(this.nudCol1);
			this.Controls.Add(this.nudRow1);
			this.Name = "frmRectangle1Multi";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "複数矩形選択";
			((System.ComponentModel.ISupportInitialize)(this.nudCol2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRow2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudCol1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRow1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.NumericUpDown nudCol2;
        private System.Windows.Forms.NumericUpDown nudRow2;
        private System.Windows.Forms.NumericUpDown nudCol1;
        private System.Windows.Forms.NumericUpDown nudRow1;
        private System.Windows.Forms.Button btnAddLast;
        private System.Windows.Forms.Button btnAddNext;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ListView lstPoints;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
    }
}