namespace Fujita.InspectionSystem
{
    partial class frmProgressForm
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
            this.abortButton = new System.Windows.Forms.Button();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.loadingPicture = new System.Windows.Forms.PictureBox();
            this.lblKeikaTime = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.loadingPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // abortButton
            // 
            this.abortButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.abortButton.BackColor = System.Drawing.SystemColors.Control;
            this.abortButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.abortButton.Location = new System.Drawing.Point(639, 35);
            this.abortButton.Name = "abortButton";
            this.abortButton.Size = new System.Drawing.Size(206, 223);
            this.abortButton.TabIndex = 1;
            this.abortButton.Text = "停止";
            this.abortButton.UseVisualStyleBackColor = false;
            this.abortButton.Click += new System.EventHandler(this.abortButton_Click);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionLabel.Location = new System.Drawing.Point(102, 35);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(531, 223);
            this.descriptionLabel.TabIndex = 2;
            this.descriptionLabel.Text = "しばらくお待ちください";
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // loadingPicture
            // 
            this.loadingPicture.Image = global::LineCameraSheetSystem.Properties.Resources.LoadingImage;
            this.loadingPicture.Location = new System.Drawing.Point(31, 150);
            this.loadingPicture.Name = "loadingPicture";
            this.loadingPicture.Size = new System.Drawing.Size(35, 32);
            this.loadingPicture.TabIndex = 0;
            this.loadingPicture.TabStop = false;
            // 
            // lblKeikaTime
            // 
            this.lblKeikaTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKeikaTime.Location = new System.Drawing.Point(102, 258);
            this.lblKeikaTime.Name = "lblKeikaTime";
            this.lblKeikaTime.Size = new System.Drawing.Size(531, 40);
            this.lblKeikaTime.TabIndex = 3;
            this.lblKeikaTime.Text = "経過時間： 0 秒";
            this.lblKeikaTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblKeikaTime.Visible = false;
            // 
            // frmProgressForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(884, 306);
            this.ControlBox = false;
            this.Controls.Add(this.lblKeikaTime);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.abortButton);
            this.Controls.Add(this.loadingPicture);
            this.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmProgressForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fujita Inspection System";
            this.Shown += new System.EventHandler(this.ProgressForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.loadingPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox loadingPicture;
        private System.Windows.Forms.Button abortButton;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label lblKeikaTime;
    }
}