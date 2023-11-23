namespace LineCameraSheetSystem
{
	partial class FormAutoInspSetting
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
			this.btnOk = new System.Windows.Forms.Button();
			this.chkAutoKandoBrightEnable = new System.Windows.Forms.CheckBox();
			this.chkAutoKandoDarkEnable = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.spinAutoKandoLimit = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.spinAutoKandoLimit)).BeginInit();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(259, 13);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(112, 31);
			this.btnCancel.TabIndex = 0;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnOk
			// 
			this.btnOk.Location = new System.Drawing.Point(13, 13);
			this.btnOk.Margin = new System.Windows.Forms.Padding(4);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(112, 31);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// chkAutoKandoBrightEnable
			// 
			this.chkAutoKandoBrightEnable.AutoSize = true;
			this.chkAutoKandoBrightEnable.Location = new System.Drawing.Point(13, 52);
			this.chkAutoKandoBrightEnable.Margin = new System.Windows.Forms.Padding(4);
			this.chkAutoKandoBrightEnable.Name = "chkAutoKandoBrightEnable";
			this.chkAutoKandoBrightEnable.Size = new System.Drawing.Size(219, 20);
			this.chkAutoKandoBrightEnable.TabIndex = 1;
			this.chkAutoKandoBrightEnable.Text = "[明]自動感度調整を実施する";
			this.chkAutoKandoBrightEnable.UseVisualStyleBackColor = true;
			// 
			// chkAutoKandoDarkEnable
			// 
			this.chkAutoKandoDarkEnable.AutoSize = true;
			this.chkAutoKandoDarkEnable.Location = new System.Drawing.Point(13, 80);
			this.chkAutoKandoDarkEnable.Margin = new System.Windows.Forms.Padding(4);
			this.chkAutoKandoDarkEnable.Name = "chkAutoKandoDarkEnable";
			this.chkAutoKandoDarkEnable.Size = new System.Drawing.Size(219, 20);
			this.chkAutoKandoDarkEnable.TabIndex = 1;
			this.chkAutoKandoDarkEnable.Text = "[暗]自動感度調整を実施する";
			this.chkAutoKandoDarkEnable.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 104);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "自動感度調整有効範囲";
			// 
			// spinAutoKandoLimit
			// 
			this.spinAutoKandoLimit.Location = new System.Drawing.Point(189, 102);
			this.spinAutoKandoLimit.Margin = new System.Windows.Forms.Padding(4);
			this.spinAutoKandoLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.spinAutoKandoLimit.Name = "spinAutoKandoLimit";
			this.spinAutoKandoLimit.Size = new System.Drawing.Size(55, 23);
			this.spinAutoKandoLimit.TabIndex = 3;
			this.spinAutoKandoLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.spinAutoKandoLimit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// FormAutoInspSetting
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(384, 262);
			this.Controls.Add(this.spinAutoKandoLimit);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.chkAutoKandoDarkEnable);
			this.Controls.Add(this.chkAutoKandoBrightEnable);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "FormAutoInspSetting";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FormAutoInspSetting";
			this.Shown += new System.EventHandler(this.FormAutoInspSetting_Shown);
			((System.ComponentModel.ISupportInitialize)(this.spinAutoKandoLimit)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.CheckBox chkAutoKandoBrightEnable;
		private System.Windows.Forms.CheckBox chkAutoKandoDarkEnable;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown spinAutoKandoLimit;
	}
}