namespace LineCameraSheetSystem
{
	partial class frmWarningDialog
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
			this.labelText = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// labelText
			// 
			this.labelText.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.labelText.Location = new System.Drawing.Point(12, 9);
			this.labelText.Name = "labelText";
			this.labelText.Size = new System.Drawing.Size(470, 168);
			this.labelText.TabIndex = 3;
			this.labelText.Text = "label1";
			this.labelText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnOk
			// 
			this.btnOk.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnOk.Location = new System.Drawing.Point(172, 180);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(150, 80);
			this.btnOk.TabIndex = 2;
			this.btnOk.Text = "確認";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// frmWarningDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 33F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(494, 272);
			this.ControlBox = false;
			this.Controls.Add(this.labelText);
			this.Controls.Add(this.btnOk);
			this.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(8, 8, 8, 8);
			this.Name = "frmWarningDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "警告";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label labelText;
		private System.Windows.Forms.Button btnOk;
	}
}