namespace Fujita.InspectionSystem
{
    partial class uclHWindowMulti
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
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
			this.hWindowControl1 = new HalconDotNet.HWindowControl();
			this.hWindowControl2 = new HalconDotNet.HWindowControl();
			this.hWindowControl3 = new HalconDotNet.HWindowControl();
			this.hWindowControl4 = new HalconDotNet.HWindowControl();
			this.lblPane1 = new System.Windows.Forms.Label();
			this.lblPane2 = new System.Windows.Forms.Label();
			this.lblPane3 = new System.Windows.Forms.Label();
			this.lblPane4 = new System.Windows.Forms.Label();
			this.lblLayoutOne = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// hWindowControl1
			// 
			this.hWindowControl1.BackColor = System.Drawing.Color.Black;
			this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
			this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
			this.hWindowControl1.Location = new System.Drawing.Point(19, 17);
			this.hWindowControl1.Name = "hWindowControl1";
			this.hWindowControl1.Size = new System.Drawing.Size(400, 300);
			this.hWindowControl1.TabIndex = 0;
			this.hWindowControl1.WindowSize = new System.Drawing.Size(400, 300);
			// 
			// hWindowControl2
			// 
			this.hWindowControl2.BackColor = System.Drawing.Color.Black;
			this.hWindowControl2.BorderColor = System.Drawing.Color.Black;
			this.hWindowControl2.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
			this.hWindowControl2.Location = new System.Drawing.Point(425, 17);
			this.hWindowControl2.Name = "hWindowControl2";
			this.hWindowControl2.Size = new System.Drawing.Size(400, 300);
			this.hWindowControl2.TabIndex = 0;
			this.hWindowControl2.WindowSize = new System.Drawing.Size(400, 300);
			// 
			// hWindowControl3
			// 
			this.hWindowControl3.BackColor = System.Drawing.Color.Black;
			this.hWindowControl3.BorderColor = System.Drawing.Color.Black;
			this.hWindowControl3.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
			this.hWindowControl3.Location = new System.Drawing.Point(19, 323);
			this.hWindowControl3.Name = "hWindowControl3";
			this.hWindowControl3.Size = new System.Drawing.Size(400, 300);
			this.hWindowControl3.TabIndex = 0;
			this.hWindowControl3.WindowSize = new System.Drawing.Size(400, 300);
			// 
			// hWindowControl4
			// 
			this.hWindowControl4.BackColor = System.Drawing.Color.Black;
			this.hWindowControl4.BorderColor = System.Drawing.Color.Black;
			this.hWindowControl4.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
			this.hWindowControl4.Location = new System.Drawing.Point(425, 323);
			this.hWindowControl4.Name = "hWindowControl4";
			this.hWindowControl4.Size = new System.Drawing.Size(400, 300);
			this.hWindowControl4.TabIndex = 0;
			this.hWindowControl4.WindowSize = new System.Drawing.Size(400, 300);
			// 
			// lblPane1
			// 
			this.lblPane1.AutoSize = true;
			this.lblPane1.Location = new System.Drawing.Point(17, 2);
			this.lblPane1.Name = "lblPane1";
			this.lblPane1.Size = new System.Drawing.Size(35, 12);
			this.lblPane1.TabIndex = 1;
			this.lblPane1.Text = "label1";
			// 
			// lblPane2
			// 
			this.lblPane2.AutoSize = true;
			this.lblPane2.Location = new System.Drawing.Point(425, 2);
			this.lblPane2.Name = "lblPane2";
			this.lblPane2.Size = new System.Drawing.Size(35, 12);
			this.lblPane2.TabIndex = 1;
			this.lblPane2.Text = "label1";
			// 
			// lblPane3
			// 
			this.lblPane3.AutoSize = true;
			this.lblPane3.Location = new System.Drawing.Point(17, 626);
			this.lblPane3.Name = "lblPane3";
			this.lblPane3.Size = new System.Drawing.Size(35, 12);
			this.lblPane3.TabIndex = 1;
			this.lblPane3.Text = "label1";
			// 
			// lblPane4
			// 
			this.lblPane4.AutoSize = true;
			this.lblPane4.Location = new System.Drawing.Point(423, 626);
			this.lblPane4.Name = "lblPane4";
			this.lblPane4.Size = new System.Drawing.Size(35, 12);
			this.lblPane4.TabIndex = 1;
			this.lblPane4.Text = "label1";
			// 
			// lblLayoutOne
			// 
			this.lblLayoutOne.AutoSize = true;
			this.lblLayoutOne.Location = new System.Drawing.Point(17, 2);
			this.lblLayoutOne.Name = "lblLayoutOne";
			this.lblLayoutOne.Size = new System.Drawing.Size(35, 12);
			this.lblLayoutOne.TabIndex = 1;
			this.lblLayoutOne.Text = "label1";
			// 
			// uclHWindowMulti
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lblPane4);
			this.Controls.Add(this.lblPane3);
			this.Controls.Add(this.lblPane2);
			this.Controls.Add(this.lblLayoutOne);
			this.Controls.Add(this.lblPane1);
			this.Controls.Add(this.hWindowControl4);
			this.Controls.Add(this.hWindowControl3);
			this.Controls.Add(this.hWindowControl2);
			this.Controls.Add(this.hWindowControl1);
			this.Name = "uclHWindowMulti";
			this.Size = new System.Drawing.Size(843, 641);
			this.Load += new System.EventHandler(this.HWindowMulti_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl1;
        private HalconDotNet.HWindowControl hWindowControl2;
        private HalconDotNet.HWindowControl hWindowControl3;
        private HalconDotNet.HWindowControl hWindowControl4;
        private System.Windows.Forms.Label lblPane1;
        private System.Windows.Forms.Label lblPane2;
        private System.Windows.Forms.Label lblPane3;
        private System.Windows.Forms.Label lblPane4;
        private System.Windows.Forms.Label lblLayoutOne;
    }
}
