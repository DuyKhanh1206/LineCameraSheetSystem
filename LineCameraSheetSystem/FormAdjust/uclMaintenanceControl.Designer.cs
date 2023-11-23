namespace Adjustment
{
    partial class uclMaintenanceControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.numOriginX = new System.Windows.Forms.NumericUpDown();
            this.numOriginY = new System.Windows.Forms.NumericUpDown();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.numHeight = new System.Windows.Forms.NumericUpDown();
            this.textGrayValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.btnLine1 = new System.Windows.Forms.Button();
            this.btnLine2 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textStdGrayValue = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.numOriginX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOriginY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // numOriginX
            // 
            this.numOriginX.Enabled = false;
            this.numOriginX.Location = new System.Drawing.Point(38, 36);
            this.numOriginX.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numOriginX.Name = "numOriginX";
            this.numOriginX.Size = new System.Drawing.Size(61, 19);
            this.numOriginX.TabIndex = 0;
            this.numOriginX.TextChanged += new System.EventHandler(this.numOriginX_TextChanged);
            // 
            // numOriginY
            // 
            this.numOriginY.Enabled = false;
            this.numOriginY.Location = new System.Drawing.Point(38, 57);
            this.numOriginY.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numOriginY.Name = "numOriginY";
            this.numOriginY.Size = new System.Drawing.Size(61, 19);
            this.numOriginY.TabIndex = 1;
            this.numOriginY.TextChanged += new System.EventHandler(this.numOriginY_TextChanged);
            // 
            // numWidth
            // 
            this.numWidth.Enabled = false;
            this.numWidth.Location = new System.Drawing.Point(130, 35);
            this.numWidth.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(61, 19);
            this.numWidth.TabIndex = 2;
            this.numWidth.TextChanged += new System.EventHandler(this.numWidth_TextChanged);
            // 
            // numHeight
            // 
            this.numHeight.Enabled = false;
            this.numHeight.Location = new System.Drawing.Point(129, 57);
            this.numHeight.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numHeight.Name = "numHeight";
            this.numHeight.Size = new System.Drawing.Size(61, 19);
            this.numHeight.TabIndex = 3;
            this.numHeight.TextChanged += new System.EventHandler(this.numHeight_TextChanged);
            // 
            // textGrayValue
            // 
            this.textGrayValue.Enabled = false;
            this.textGrayValue.Location = new System.Drawing.Point(308, 31);
            this.textGrayValue.Name = "textGrayValue";
            this.textGrayValue.ReadOnly = true;
            this.textGrayValue.Size = new System.Drawing.Size(68, 19);
            this.textGrayValue.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(20, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(12, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(20, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(12, 12);
            this.label2.TabIndex = 13;
            this.label2.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(110, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "W";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(110, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "H";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(213, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "グレー値（現在値）";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Enabled = false;
            this.labelTitle.Location = new System.Drawing.Point(15, 3);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(41, 12);
            this.labelTitle.TabIndex = 24;
            this.labelTitle.Text = "未定義";
            // 
            // btnLine1
            // 
            this.btnLine1.Enabled = false;
            this.btnLine1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLine1.Location = new System.Drawing.Point(6, 8);
            this.btnLine1.Name = "btnLine1";
            this.btnLine1.Size = new System.Drawing.Size(378, 82);
            this.btnLine1.TabIndex = 25;
            this.btnLine1.UseVisualStyleBackColor = true;
            // 
            // btnLine2
            // 
            this.btnLine2.Enabled = false;
            this.btnLine2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLine2.Location = new System.Drawing.Point(13, 23);
            this.btnLine2.Name = "btnLine2";
            this.btnLine2.Size = new System.Drawing.Size(193, 58);
            this.btnLine2.TabIndex = 26;
            this.btnLine2.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Enabled = false;
            this.label9.Location = new System.Drawing.Point(23, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 12);
            this.label9.TabIndex = 27;
            this.label9.Text = "ROI設定";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(213, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 12);
            this.label6.TabIndex = 28;
            this.label6.Text = "グレー値（基準値）";
            // 
            // textStdGrayValue
            // 
            this.textStdGrayValue.Enabled = false;
            this.textStdGrayValue.Location = new System.Drawing.Point(308, 56);
            this.textStdGrayValue.Name = "textStdGrayValue";
            this.textStdGrayValue.ReadOnly = true;
            this.textStdGrayValue.Size = new System.Drawing.Size(68, 19);
            this.textStdGrayValue.TabIndex = 29;
            // 
            // uclMaintenanceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textStdGrayValue);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textGrayValue);
            this.Controls.Add(this.numHeight);
            this.Controls.Add(this.numWidth);
            this.Controls.Add(this.numOriginY);
            this.Controls.Add(this.numOriginX);
            this.Controls.Add(this.btnLine2);
            this.Controls.Add(this.btnLine1);
            this.Name = "uclMaintenanceControl";
            this.Size = new System.Drawing.Size(394, 100);
            ((System.ComponentModel.ISupportInitialize)(this.numOriginX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOriginY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeight)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numOriginX;
        private System.Windows.Forms.NumericUpDown numOriginY;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.NumericUpDown numHeight;
        private System.Windows.Forms.TextBox textGrayValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button btnLine1;
        private System.Windows.Forms.Button btnLine2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textStdGrayValue;
    }
}
