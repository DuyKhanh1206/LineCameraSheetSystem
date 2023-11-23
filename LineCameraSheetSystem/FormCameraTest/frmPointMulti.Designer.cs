namespace Fujita.InspectionSystem
{
    partial class frmPointMulti
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
			this.nudCol = new System.Windows.Forms.NumericUpDown();
			this.nudRow = new System.Windows.Forms.NumericUpDown();
			this.lblMessage = new System.Windows.Forms.Label();
			this.lstPoints = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnAddNext = new System.Windows.Forms.Button();
			this.btnAddLast = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nudCol)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRow)).BeginInit();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Location = new System.Drawing.Point(211, 102);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(43, 24);
			this.label2.TabIndex = 5;
			this.label2.Text = "Col";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(17, 102);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 24);
			this.label1.TabIndex = 6;
			this.label1.Text = "Row";
			// 
			// nudCol
			// 
			this.nudCol.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nudCol.Location = new System.Drawing.Point(282, 93);
			this.nudCol.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.nudCol.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
			this.nudCol.Name = "nudCol";
			this.nudCol.Size = new System.Drawing.Size(120, 39);
			this.nudCol.TabIndex = 3;
			this.nudCol.ValueChanged += new System.EventHandler(this.nudCol_ValueChanged);
			// 
			// nudRow
			// 
			this.nudRow.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.nudRow.Location = new System.Drawing.Point(85, 93);
			this.nudRow.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.nudRow.Minimum = new decimal(new int[] {
            5000,
            0,
            0,
            -2147483648});
			this.nudRow.Name = "nudRow";
			this.nudRow.Size = new System.Drawing.Size(120, 39);
			this.nudRow.TabIndex = 4;
			this.nudRow.ValueChanged += new System.EventHandler(this.nudRow_ValueChanged);
			// 
			// lblMessage
			// 
			this.lblMessage.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lblMessage.Location = new System.Drawing.Point(2, 4);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(411, 76);
			this.lblMessage.TabIndex = 7;
			this.lblMessage.Text = "label1";
			// 
			// lstPoints
			// 
			this.lstPoints.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.lstPoints.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.lstPoints.FullRowSelect = true;
			this.lstPoints.GridLines = true;
			this.lstPoints.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lstPoints.HideSelection = false;
			this.lstPoints.Location = new System.Drawing.Point(12, 158);
			this.lstPoints.MultiSelect = false;
			this.lstPoints.Name = "lstPoints";
			this.lstPoints.Size = new System.Drawing.Size(390, 250);
			this.lstPoints.TabIndex = 8;
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
			this.columnHeader2.Text = "Row";
			this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader2.Width = 120;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Col";
			this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader3.Width = 120;
			// 
			// btnCancel
			// 
			this.btnCancel.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnCancel.Location = new System.Drawing.Point(227, 481);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(164, 51);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOK
			// 
			this.btnOK.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnOK.Location = new System.Drawing.Point(28, 481);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(166, 51);
			this.btnOK.TabIndex = 9;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnDelete.Location = new System.Drawing.Point(12, 414);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(114, 36);
			this.btnDelete.TabIndex = 9;
			this.btnDelete.Text = "削除";
			this.btnDelete.UseVisualStyleBackColor = true;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnAddNext
			// 
			this.btnAddNext.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnAddNext.Location = new System.Drawing.Point(159, 414);
			this.btnAddNext.Name = "btnAddNext";
			this.btnAddNext.Size = new System.Drawing.Size(114, 36);
			this.btnAddNext.TabIndex = 9;
			this.btnAddNext.Text = "追加";
			this.btnAddNext.UseVisualStyleBackColor = true;
			this.btnAddNext.Click += new System.EventHandler(this.btnAddNext_Click);
			// 
			// btnAddLast
			// 
			this.btnAddLast.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnAddLast.Location = new System.Drawing.Point(288, 414);
			this.btnAddLast.Name = "btnAddLast";
			this.btnAddLast.Size = new System.Drawing.Size(114, 36);
			this.btnAddLast.TabIndex = 9;
			this.btnAddLast.Text = "最後に追加";
			this.btnAddLast.UseVisualStyleBackColor = true;
			this.btnAddLast.Click += new System.EventHandler(this.btnAddLast_Click);
			// 
			// frmPointMulti
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(414, 544);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnAddLast);
			this.Controls.Add(this.btnAddNext);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.lstPoints);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nudCol);
			this.Controls.Add(this.nudRow);
			this.Name = "frmPointMulti";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "複数ポイント設定";
			((System.ComponentModel.ISupportInitialize)(this.nudCol)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudRow)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudCol;
        private System.Windows.Forms.NumericUpDown nudRow;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ListView lstPoints;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAddNext;
        private System.Windows.Forms.Button btnAddLast;
    }
}