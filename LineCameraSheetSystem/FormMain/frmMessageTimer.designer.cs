namespace Fujita.InspectionSystem
{
    partial class frmMessageTimer
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
            this.components = new System.ComponentModel.Container();
            this.cancelButton = new System.Windows.Forms.Button();
            this.ok2Button = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.iconPicture = new System.Windows.Forms.PictureBox();
            this.messageLabel = new System.Windows.Forms.Label();
            this.lblTimerMessage = new System.Windows.Forms.Label();
            this.timerTime = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.iconPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(467, 282);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(150, 80);
            this.cancelButton.TabIndex = 5;
            this.cancelButton.Text = "キャンセル";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // ok2Button
            // 
            this.ok2Button.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.ok2Button.Location = new System.Drawing.Point(46, 282);
            this.ok2Button.Name = "ok2Button";
            this.ok2Button.Size = new System.Drawing.Size(150, 80);
            this.ok2Button.TabIndex = 6;
            this.ok2Button.Text = "OK";
            this.ok2Button.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(262, 282);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(150, 80);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // iconPicture
            // 
            this.iconPicture.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.iconPicture.Location = new System.Drawing.Point(46, 82);
            this.iconPicture.Name = "iconPicture";
            this.iconPicture.Size = new System.Drawing.Size(64, 64);
            this.iconPicture.TabIndex = 4;
            this.iconPicture.TabStop = false;
            // 
            // messageLabel
            // 
            this.messageLabel.Location = new System.Drawing.Point(138, 15);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(510, 199);
            this.messageLabel.TabIndex = 3;
            this.messageLabel.Text = "Message";
            this.messageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTimerMessage
            // 
            this.lblTimerMessage.Location = new System.Drawing.Point(56, 230);
            this.lblTimerMessage.Name = "lblTimerMessage";
            this.lblTimerMessage.Size = new System.Drawing.Size(561, 39);
            this.lblTimerMessage.TabIndex = 3;
            this.lblTimerMessage.Text = "Message";
            this.lblTimerMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timerTime
            // 
            this.timerTime.Interval = 1000;
            this.timerTime.Tick += new System.EventHandler(this.timerTime_Tick);
            // 
            // frmMessageTimer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(672, 387);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.ok2Button);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.iconPicture);
            this.Controls.Add(this.lblTimerMessage);
            this.Controls.Add(this.messageLabel);
            this.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(7);
            this.Name = "frmMessageTimer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmMessageTimer";
            this.Shown += new System.EventHandler(this.frmMessageTimer_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.iconPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button ok2Button;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.PictureBox iconPicture;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Label lblTimerMessage;
        private System.Windows.Forms.Timer timerTime;

    }
}