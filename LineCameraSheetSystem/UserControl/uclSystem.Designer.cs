namespace LineCameraSheetSystem
{
    partial class uclSystem
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
            this.components = new System.ComponentModel.Container();
            this.btnSetting = new System.Windows.Forms.Button();
            this.btnAjustment = new System.Windows.Forms.Button();
            this.btnAutoInspSetting = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtOnGrabbed2 = new System.Windows.Forms.TextBox();
            this.txtConnectImage2 = new System.Windows.Forms.TextBox();
            this.txtGetImageThread2 = new System.Windows.Forms.TextBox();
            this.txtGetImageThread1 = new System.Windows.Forms.TextBox();
            this.txtOnGrabbed1 = new System.Windows.Forms.TextBox();
            this.txtConnectImage1 = new System.Windows.Forms.TextBox();
            this.txtInspTime = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.txtSyncCount = new System.Windows.Forms.TextBox();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.txtCaptureCnt2 = new System.Windows.Forms.TextBox();
            this.txtCaptureCnt1 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDevelopment = new System.Windows.Forms.Button();
            this.shortcutKeyHelper1 = new Extension.ShortcutKeyHelper(this.components);
            this.txtCaptureFailCount1 = new System.Windows.Forms.TextBox();
            this.txtCaptureFailCount2 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSetting
            // 
            this.btnSetting.Font = new System.Drawing.Font("MS UI Gothic", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnSetting.Location = new System.Drawing.Point(160, 155);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(250, 110);
            this.btnSetting.TabIndex = 0;
            this.btnSetting.Text = "設定(S)";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // btnAjustment
            // 
            this.btnAjustment.Font = new System.Drawing.Font("MS UI Gothic", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAjustment.Location = new System.Drawing.Point(160, 445);
            this.btnAjustment.Name = "btnAjustment";
            this.btnAjustment.Size = new System.Drawing.Size(250, 110);
            this.btnAjustment.TabIndex = 2;
            this.btnAjustment.Text = "調整(A)";
            this.btnAjustment.UseVisualStyleBackColor = true;
            this.btnAjustment.Visible = false;
            this.btnAjustment.Click += new System.EventHandler(this.btnAjustment_Click);
            // 
            // btnAutoInspSetting
            // 
            this.btnAutoInspSetting.Font = new System.Drawing.Font("MS UI Gothic", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnAutoInspSetting.Location = new System.Drawing.Point(350, 3);
            this.btnAutoInspSetting.Name = "btnAutoInspSetting";
            this.btnAutoInspSetting.Size = new System.Drawing.Size(250, 48);
            this.btnAutoInspSetting.TabIndex = 2;
            this.btnAutoInspSetting.Text = "自動検査設定";
            this.btnAutoInspSetting.UseVisualStyleBackColor = true;
            this.btnAutoInspSetting.Visible = false;
            this.btnAutoInspSetting.Click += new System.EventHandler(this.btnAutoInspSetting_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtOnGrabbed2);
            this.panel1.Controls.Add(this.txtConnectImage2);
            this.panel1.Controls.Add(this.txtGetImageThread2);
            this.panel1.Controls.Add(this.txtGetImageThread1);
            this.panel1.Controls.Add(this.txtOnGrabbed1);
            this.panel1.Controls.Add(this.txtConnectImage1);
            this.panel1.Controls.Add(this.txtInspTime);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox6);
            this.panel1.Controls.Add(this.txtSyncCount);
            this.panel1.Controls.Add(this.textBox10);
            this.panel1.Controls.Add(this.textBox9);
            this.panel1.Controls.Add(this.textBox5);
            this.panel1.Controls.Add(this.txtCaptureFailCount2);
            this.panel1.Controls.Add(this.txtCaptureCnt2);
            this.panel1.Controls.Add(this.txtCaptureFailCount1);
            this.panel1.Controls.Add(this.txtCaptureCnt1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(0, 561);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(586, 260);
            this.panel1.TabIndex = 3;
            this.panel1.Visible = false;
            // 
            // txtOnGrabbed2
            // 
            this.txtOnGrabbed2.Location = new System.Drawing.Point(213, 28);
            this.txtOnGrabbed2.Name = "txtOnGrabbed2";
            this.txtOnGrabbed2.ReadOnly = true;
            this.txtOnGrabbed2.Size = new System.Drawing.Size(69, 19);
            this.txtOnGrabbed2.TabIndex = 32;
            this.txtOnGrabbed2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtConnectImage2
            // 
            this.txtConnectImage2.Location = new System.Drawing.Point(213, 3);
            this.txtConnectImage2.Name = "txtConnectImage2";
            this.txtConnectImage2.ReadOnly = true;
            this.txtConnectImage2.Size = new System.Drawing.Size(69, 19);
            this.txtConnectImage2.TabIndex = 32;
            this.txtConnectImage2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtGetImageThread2
            // 
            this.txtGetImageThread2.Location = new System.Drawing.Point(213, 53);
            this.txtGetImageThread2.Name = "txtGetImageThread2";
            this.txtGetImageThread2.ReadOnly = true;
            this.txtGetImageThread2.Size = new System.Drawing.Size(69, 19);
            this.txtGetImageThread2.TabIndex = 32;
            this.txtGetImageThread2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtGetImageThread1
            // 
            this.txtGetImageThread1.Location = new System.Drawing.Point(138, 53);
            this.txtGetImageThread1.Name = "txtGetImageThread1";
            this.txtGetImageThread1.ReadOnly = true;
            this.txtGetImageThread1.Size = new System.Drawing.Size(69, 19);
            this.txtGetImageThread1.TabIndex = 32;
            this.txtGetImageThread1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtOnGrabbed1
            // 
            this.txtOnGrabbed1.Location = new System.Drawing.Point(138, 28);
            this.txtOnGrabbed1.Name = "txtOnGrabbed1";
            this.txtOnGrabbed1.ReadOnly = true;
            this.txtOnGrabbed1.Size = new System.Drawing.Size(69, 19);
            this.txtOnGrabbed1.TabIndex = 32;
            this.txtOnGrabbed1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtConnectImage1
            // 
            this.txtConnectImage1.Location = new System.Drawing.Point(138, 3);
            this.txtConnectImage1.Name = "txtConnectImage1";
            this.txtConnectImage1.ReadOnly = true;
            this.txtConnectImage1.Size = new System.Drawing.Size(69, 19);
            this.txtConnectImage1.TabIndex = 32;
            this.txtConnectImage1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtInspTime
            // 
            this.txtInspTime.Location = new System.Drawing.Point(138, 78);
            this.txtInspTime.Name = "txtInspTime";
            this.txtInspTime.ReadOnly = true;
            this.txtInspTime.Size = new System.Drawing.Size(69, 19);
            this.txtInspTime.TabIndex = 32;
            this.txtInspTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(37, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 12);
            this.label8.TabIndex = 31;
            this.label8.Text = "①ｲﾒｰｼﾞ連結(ms)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(68, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 12);
            this.label9.TabIndex = 31;
            this.label9.Text = "①＋②(ms)";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 12);
            this.label7.TabIndex = 31;
            this.label7.Text = "②ｲﾒｰｼﾞ加工･描画(ms)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 12);
            this.label1.TabIndex = 31;
            this.label1.Text = "検査時間(ms)";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(492, 82);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(69, 19);
            this.textBox6.TabIndex = 26;
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSyncCount
            // 
            this.txtSyncCount.Location = new System.Drawing.Point(417, 133);
            this.txtSyncCount.Name = "txtSyncCount";
            this.txtSyncCount.ReadOnly = true;
            this.txtSyncCount.Size = new System.Drawing.Size(69, 19);
            this.txtSyncCount.TabIndex = 30;
            this.txtSyncCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(492, 107);
            this.textBox10.Name = "textBox10";
            this.textBox10.ReadOnly = true;
            this.textBox10.Size = new System.Drawing.Size(69, 19);
            this.textBox10.TabIndex = 22;
            this.textBox10.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(417, 108);
            this.textBox9.Name = "textBox9";
            this.textBox9.ReadOnly = true;
            this.textBox9.Size = new System.Drawing.Size(69, 19);
            this.textBox9.TabIndex = 21;
            this.textBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(416, 83);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(69, 19);
            this.textBox5.TabIndex = 23;
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCaptureCnt2
            // 
            this.txtCaptureCnt2.Location = new System.Drawing.Point(492, 7);
            this.txtCaptureCnt2.Name = "txtCaptureCnt2";
            this.txtCaptureCnt2.ReadOnly = true;
            this.txtCaptureCnt2.Size = new System.Drawing.Size(69, 19);
            this.txtCaptureCnt2.TabIndex = 15;
            this.txtCaptureCnt2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCaptureCnt1
            // 
            this.txtCaptureCnt1.Location = new System.Drawing.Point(417, 7);
            this.txtCaptureCnt1.Name = "txtCaptureCnt1";
            this.txtCaptureCnt1.ReadOnly = true;
            this.txtCaptureCnt1.Size = new System.Drawing.Size(69, 19);
            this.txtCaptureCnt1.TabIndex = 15;
            this.txtCaptureCnt1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(417, 58);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(69, 19);
            this.textBox1.TabIndex = 15;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(492, 57);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(69, 19);
            this.textBox2.TabIndex = 16;
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(369, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 29;
            this.label4.Text = "同期数";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(335, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "検査部ｷｭｰ数";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(299, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "検査部ｷｭｰ取込枚数";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(332, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 12);
            this.label6.TabIndex = 14;
            this.label6.Text = "リアル取込枚数";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(321, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "検査部取込枚数";
            // 
            // btnDevelopment
            // 
            this.btnDevelopment.Font = new System.Drawing.Font("MS UI Gothic", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnDevelopment.Location = new System.Drawing.Point(160, 271);
            this.btnDevelopment.Name = "btnDevelopment";
            this.btnDevelopment.Size = new System.Drawing.Size(250, 110);
            this.btnDevelopment.TabIndex = 0;
            this.btnDevelopment.Text = "開発者設定";
            this.btnDevelopment.UseVisualStyleBackColor = true;
            this.btnDevelopment.Visible = false;
            this.btnDevelopment.Click += new System.EventHandler(this.btnDevelopment_Click);
            // 
            // txtCaptureFailCount1
            // 
            this.txtCaptureFailCount1.Location = new System.Drawing.Point(416, 32);
            this.txtCaptureFailCount1.Name = "txtCaptureFailCount1";
            this.txtCaptureFailCount1.ReadOnly = true;
            this.txtCaptureFailCount1.Size = new System.Drawing.Size(69, 19);
            this.txtCaptureFailCount1.TabIndex = 15;
            this.txtCaptureFailCount1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtCaptureFailCount2
            // 
            this.txtCaptureFailCount2.Location = new System.Drawing.Point(492, 31);
            this.txtCaptureFailCount2.Name = "txtCaptureFailCount2";
            this.txtCaptureFailCount2.ReadOnly = true;
            this.txtCaptureFailCount2.Size = new System.Drawing.Size(69, 19);
            this.txtCaptureFailCount2.TabIndex = 15;
            this.txtCaptureFailCount2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(331, 35);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 12);
            this.label10.TabIndex = 14;
            this.label10.Text = "取込Fail枚数";
            // 
            // uclSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnAutoInspSetting);
            this.Controls.Add(this.btnAjustment);
            this.Controls.Add(this.btnDevelopment);
            this.Controls.Add(this.btnSetting);
            this.Name = "uclSystem";
            this.Size = new System.Drawing.Size(600, 850);
            this.Load += new System.EventHandler(this.uclSystem_Load);
            this.DoubleClick += new System.EventHandler(this.uclSystem_DoubleClick);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSetting;
        private Extension.ShortcutKeyHelper shortcutKeyHelper1;
        public System.Windows.Forms.Button btnAjustment;
        public System.Windows.Forms.Button btnAutoInspSetting;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox txtSyncCount;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtInspTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDevelopment;
        private System.Windows.Forms.TextBox txtCaptureCnt2;
        private System.Windows.Forms.TextBox txtCaptureCnt1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtConnectImage2;
        private System.Windows.Forms.TextBox txtConnectImage1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtOnGrabbed2;
        private System.Windows.Forms.TextBox txtGetImageThread2;
        private System.Windows.Forms.TextBox txtGetImageThread1;
        private System.Windows.Forms.TextBox txtOnGrabbed1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtCaptureFailCount2;
        private System.Windows.Forms.TextBox txtCaptureFailCount1;
        private System.Windows.Forms.Label label10;
    }
}
