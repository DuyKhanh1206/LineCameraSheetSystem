namespace Fujita.InspectionSystem
{
    partial class frmMessageForm
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
            this.messageLabel = new System.Windows.Forms.Label();
            this.iconPicture = new System.Windows.Forms.PictureBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.ok2Button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.iconPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // messageLabel
            // 
            this.messageLabel.Location = new System.Drawing.Point(112, 9);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(536, 199);
            this.messageLabel.TabIndex = 0;
            this.messageLabel.Text = "Message";
            this.messageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // iconPicture
            // 
            this.iconPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.iconPicture.Location = new System.Drawing.Point(22, 67);
            this.iconPicture.Name = "iconPicture";
            this.iconPicture.Size = new System.Drawing.Size(84, 84);
            this.iconPicture.TabIndex = 1;
            this.iconPicture.TabStop = false;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(262, 219);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(150, 80);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(431, 219);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(150, 80);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "キャンセル";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // ok2Button
            // 
            this.ok2Button.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.ok2Button.Location = new System.Drawing.Point(96, 219);
            this.ok2Button.Name = "ok2Button";
            this.ok2Button.Size = new System.Drawing.Size(150, 80);
            this.ok2Button.TabIndex = 2;
            this.ok2Button.Text = "OK";
            this.ok2Button.UseVisualStyleBackColor = true;
            this.ok2Button.Click += new System.EventHandler(this.okButton_Click);
            // 
            // frmMessageForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(674, 322);
            this.ControlBox = false;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.ok2Button);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.iconPicture);
            this.Controls.Add(this.messageLabel);
            this.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMessageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "カスタムベースソフト";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.MessageForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.iconPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.PictureBox iconPicture;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button ok2Button;
    }
}