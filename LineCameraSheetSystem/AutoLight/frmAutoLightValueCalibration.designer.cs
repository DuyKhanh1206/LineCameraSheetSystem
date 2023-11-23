namespace LineCameraSheetSystem
{
    partial class frmAutoLightValueCalibration
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
            this.btnAllPurpose = new System.Windows.Forms.Button();
            this.prbProgres = new System.Windows.Forms.ProgressBar();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.lblTittle = new System.Windows.Forms.Label();
            this.lblProgress1 = new System.Windows.Forms.Label();
            this.lblProgress2 = new System.Windows.Forms.Label();
            this.lstDebugUp = new System.Windows.Forms.ListBox();
            this.lstDebugDown = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // btnAllPurpose
            // 
            this.btnAllPurpose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAllPurpose.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAllPurpose.Location = new System.Drawing.Point(277, 467);
            this.btnAllPurpose.Name = "btnAllPurpose";
            this.btnAllPurpose.Size = new System.Drawing.Size(195, 59);
            this.btnAllPurpose.TabIndex = 0;
            this.btnAllPurpose.Text = "キャンセル";
            this.btnAllPurpose.UseVisualStyleBackColor = true;
            // 
            // prbProgres
            // 
            this.prbProgres.Location = new System.Drawing.Point(85, 145);
            this.prbProgres.Name = "prbProgres";
            this.prbProgres.Size = new System.Drawing.Size(590, 57);
            this.prbProgres.TabIndex = 1;
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMessage.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMessage.Location = new System.Drawing.Point(80, 67);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(603, 397);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.Text = "1\r\n2\r\n3\r\n4\r\n5\r\n6\r\n";
            // 
            // btnYes
            // 
            this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnYes.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnYes.Location = new System.Drawing.Point(149, 467);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(195, 59);
            this.btnYes.TabIndex = 0;
            this.btnYes.Text = "はい";
            this.btnYes.UseVisualStyleBackColor = true;
            // 
            // btnNo
            // 
            this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNo.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnNo.Location = new System.Drawing.Point(421, 467);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(195, 59);
            this.btnNo.TabIndex = 0;
            this.btnNo.Text = "いいえ";
            this.btnNo.UseVisualStyleBackColor = true;
            // 
            // lblTittle
            // 
            this.lblTittle.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblTittle.Location = new System.Drawing.Point(80, 9);
            this.lblTittle.Name = "lblTittle";
            this.lblTittle.Size = new System.Drawing.Size(595, 58);
            this.lblTittle.TabIndex = 2;
            this.lblTittle.Text = "照明自動調整を実行中です\r\nしばらくお待ちください";
            // 
            // lblProgress1
            // 
            this.lblProgress1.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblProgress1.Location = new System.Drawing.Point(80, 67);
            this.lblProgress1.Name = "lblProgress1";
            this.lblProgress1.Size = new System.Drawing.Size(595, 35);
            this.lblProgress1.TabIndex = 2;
            // 
            // lblProgress2
            // 
            this.lblProgress2.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblProgress2.Location = new System.Drawing.Point(80, 102);
            this.lblProgress2.Name = "lblProgress2";
            this.lblProgress2.Size = new System.Drawing.Size(595, 35);
            this.lblProgress2.TabIndex = 2;
            // 
            // lstDebugUp
            // 
            this.lstDebugUp.FormattingEnabled = true;
            this.lstDebugUp.ItemHeight = 12;
            this.lstDebugUp.Location = new System.Drawing.Point(85, 253);
            this.lstDebugUp.Name = "lstDebugUp";
            this.lstDebugUp.Size = new System.Drawing.Size(286, 184);
            this.lstDebugUp.TabIndex = 3;
            // 
            // lstDebugDown
            // 
            this.lstDebugDown.FormattingEnabled = true;
            this.lstDebugDown.ItemHeight = 12;
            this.lstDebugDown.Location = new System.Drawing.Point(389, 253);
            this.lstDebugDown.Name = "lstDebugDown";
            this.lstDebugDown.Size = new System.Drawing.Size(286, 184);
            this.lstDebugDown.TabIndex = 3;
            // 
            // frmAutoLightValueCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 544);
            this.Controls.Add(this.lstDebugDown);
            this.Controls.Add(this.lstDebugUp);
            this.Controls.Add(this.prbProgres);
            this.Controls.Add(this.lblTittle);
            this.Controls.Add(this.lblProgress2);
            this.Controls.Add(this.lblProgress1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.btnAllPurpose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAutoLightValueCalibration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "照明自動調整";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAllPurpose;
        private System.Windows.Forms.ProgressBar prbProgres;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Label lblTittle;
        private System.Windows.Forms.Label lblProgress1;
        private System.Windows.Forms.Label lblProgress2;
        private System.Windows.Forms.ListBox lstDebugUp;
        private System.Windows.Forms.ListBox lstDebugDown;
    }
}