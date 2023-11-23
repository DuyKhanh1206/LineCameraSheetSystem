namespace LineCameraSheetSystem
{
    partial class uclMiniImage
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
            this.labelLength = new System.Windows.Forms.Label();
            this.labelSpot = new System.Windows.Forms.Label();
            this.labelData = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 300, 300);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 20);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(185, 185);
            this.hWindowControl1.TabIndex = 1;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(185, 185);
            this.hWindowControl1.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseDown);
            // 
            // labelLength
            // 
            this.labelLength.BackColor = System.Drawing.SystemColors.Window;
            this.labelLength.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelLength.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelLength.Location = new System.Drawing.Point(0, 0);
            this.labelLength.Margin = new System.Windows.Forms.Padding(0);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(185, 19);
            this.labelLength.TabIndex = 0;
            this.labelLength.Text = "欠: 長:12345.6 幅:800";
            this.labelLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelLength.Click += new System.EventHandler(this.LabelClik);
            this.labelLength.DoubleClick += new System.EventHandler(this.ThumbnailDoubleClik);
            // 
            // labelSpot
            // 
            this.labelSpot.BackColor = System.Drawing.SystemColors.Window;
            this.labelSpot.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelSpot.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelSpot.Location = new System.Drawing.Point(0, 206);
            this.labelSpot.Margin = new System.Windows.Forms.Padding(0);
            this.labelSpot.Name = "labelSpot";
            this.labelSpot.Size = new System.Drawing.Size(185, 19);
            this.labelSpot.TabIndex = 2;
            this.labelSpot.Text = "面: 表 ｶﾒﾗ: 1 ｿﾞｰﾝ: Z1";
            this.labelSpot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelSpot.Click += new System.EventHandler(this.LabelClik);
            this.labelSpot.DoubleClick += new System.EventHandler(this.ThumbnailDoubleClik);
            // 
            // labelData
            // 
            this.labelData.BackColor = System.Drawing.SystemColors.Window;
            this.labelData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelData.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelData.Location = new System.Drawing.Point(0, 224);
            this.labelData.Margin = new System.Windows.Forms.Padding(0);
            this.labelData.Name = "labelData";
            this.labelData.Size = new System.Drawing.Size(185, 19);
            this.labelData.TabIndex = 3;
            this.labelData.Text = "縦:100.0 横:100.0 積:10000.0";
            this.labelData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelData.Click += new System.EventHandler(this.LabelClik);
            this.labelData.DoubleClick += new System.EventHandler(this.ThumbnailDoubleClik);
            // 
            // uclMiniImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelData);
            this.Controls.Add(this.labelSpot);
            this.Controls.Add(this.labelLength);
            this.Controls.Add(this.hWindowControl1);
            this.Name = "uclMiniImage";
            this.Size = new System.Drawing.Size(186, 244);
            this.DoubleClick += new System.EventHandler(this.ThumbnailDoubleClik);
            this.ResumeLayout(false);

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.Label labelSpot;
        private System.Windows.Forms.Label labelData;
    }
}
