namespace Fujita.InspectionSystem
{
    partial class SplashForm
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
			this.pgbProgress = new System.Windows.Forms.ProgressBar();
			this.label1 = new System.Windows.Forms.Label();
			this.lblProgressContent = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pgbProgress
			// 
			this.pgbProgress.Location = new System.Drawing.Point(16, 192);
			this.pgbProgress.Name = "pgbProgress";
			this.pgbProgress.Size = new System.Drawing.Size(568, 23);
			this.pgbProgress.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("MS UI Gothic", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label1.Location = new System.Drawing.Point(72, 32);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(431, 37);
			this.label1.TabIndex = 1;
			this.label1.Text = "アプリケーションを起動中です";
			// 
			// lblProgressContent
			// 
			this.lblProgressContent.AutoSize = true;
			this.lblProgressContent.Location = new System.Drawing.Point(16, 176);
			this.lblProgressContent.Name = "lblProgressContent";
			this.lblProgressContent.Size = new System.Drawing.Size(27, 12);
			this.lblProgressContent.TabIndex = 2;
			this.lblProgressContent.Text = "Wait";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("MS UI Gothic", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.label2.Location = new System.Drawing.Point(72, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(320, 37);
			this.label2.TabIndex = 1;
			this.label2.Text = "しばらくお待ちください";
			// 
			// SplashForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(597, 233);
			this.ControlBox = false;
			this.Controls.Add(this.lblProgressContent);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.pgbProgress);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "SplashForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Fujita Inspection System";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ProgressBar pgbProgress;
        public System.Windows.Forms.Label lblProgressContent;
        private System.Windows.Forms.Label label2;
    }
}