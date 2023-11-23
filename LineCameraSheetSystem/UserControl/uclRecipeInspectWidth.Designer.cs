namespace LineCameraSheetSystem
{
    partial class uclRecipeInspectWidth
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btnSetCommon = new System.Windows.Forms.Button();
            this.lblCmnMaskShift = new System.Windows.Forms.Label();
            this.lblCmnMaskWidth = new System.Windows.Forms.Label();
            this.lblCmnInspWidth = new System.Windows.Forms.Label();
            this.chkInspAreaCommonMode2 = new System.Windows.Forms.CheckBox();
            this.chkInspAreaCommonMode1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.spinMaskWidth = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinWidth = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.spinMaskShift = new Fujita.InspectionSystem.uclNumericInputSmall();
            this.label3 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.btnSetCommon);
            this.groupBox5.Controls.Add(this.lblCmnMaskShift);
            this.groupBox5.Controls.Add(this.lblCmnMaskWidth);
            this.groupBox5.Controls.Add(this.lblCmnInspWidth);
            this.groupBox5.Controls.Add(this.chkInspAreaCommonMode2);
            this.groupBox5.Controls.Add(this.chkInspAreaCommonMode1);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.spinMaskWidth);
            this.groupBox5.Controls.Add(this.spinWidth);
            this.groupBox5.Controls.Add(this.spinMaskShift);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.groupBox5.Location = new System.Drawing.Point(0, 0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(598, 105);
            this.groupBox5.TabIndex = 40;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "検査領域";
            // 
            // btnSetCommon
            // 
            this.btnSetCommon.Location = new System.Drawing.Point(461, 10);
            this.btnSetCommon.Name = "btnSetCommon";
            this.btnSetCommon.Size = new System.Drawing.Size(69, 89);
            this.btnSetCommon.TabIndex = 26;
            this.btnSetCommon.Text = "共通値に設定";
            this.btnSetCommon.UseVisualStyleBackColor = true;
            this.btnSetCommon.Click += new System.EventHandler(this.btnSetCommon_Click);
            // 
            // lblCmnMaskShift
            // 
            this.lblCmnMaskShift.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCmnMaskShift.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCmnMaskShift.Location = new System.Drawing.Point(315, 79);
            this.lblCmnMaskShift.Name = "lblCmnMaskShift";
            this.lblCmnMaskShift.Size = new System.Drawing.Size(54, 20);
            this.lblCmnMaskShift.TabIndex = 25;
            this.lblCmnMaskShift.Text = "000";
            this.lblCmnMaskShift.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCmnMaskWidth
            // 
            this.lblCmnMaskWidth.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCmnMaskWidth.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCmnMaskWidth.Location = new System.Drawing.Point(166, 79);
            this.lblCmnMaskWidth.Name = "lblCmnMaskWidth";
            this.lblCmnMaskWidth.Size = new System.Drawing.Size(54, 20);
            this.lblCmnMaskWidth.TabIndex = 25;
            this.lblCmnMaskWidth.Text = "000";
            this.lblCmnMaskWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCmnInspWidth
            // 
            this.lblCmnInspWidth.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblCmnInspWidth.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblCmnInspWidth.Location = new System.Drawing.Point(6, 79);
            this.lblCmnInspWidth.Name = "lblCmnInspWidth";
            this.lblCmnInspWidth.Size = new System.Drawing.Size(66, 20);
            this.lblCmnInspWidth.TabIndex = 25;
            this.lblCmnInspWidth.Text = "0000";
            this.lblCmnInspWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkInspAreaCommonMode2
            // 
            this.chkInspAreaCommonMode2.AutoSize = true;
            this.chkInspAreaCommonMode2.Location = new System.Drawing.Point(541, 70);
            this.chkInspAreaCommonMode2.Name = "chkInspAreaCommonMode2";
            this.chkInspAreaCommonMode2.Size = new System.Drawing.Size(56, 19);
            this.chkInspAreaCommonMode2.TabIndex = 24;
            this.chkInspAreaCommonMode2.Text = "個別";
            this.chkInspAreaCommonMode2.UseVisualStyleBackColor = true;
            this.chkInspAreaCommonMode2.CheckedChanged += new System.EventHandler(this.chkInspAreaCommonMode2_CheckedChanged);
            // 
            // chkInspAreaCommonMode1
            // 
            this.chkInspAreaCommonMode1.AutoSize = true;
            this.chkInspAreaCommonMode1.Location = new System.Drawing.Point(541, 45);
            this.chkInspAreaCommonMode1.Name = "chkInspAreaCommonMode1";
            this.chkInspAreaCommonMode1.Size = new System.Drawing.Size(56, 19);
            this.chkInspAreaCommonMode1.TabIndex = 24;
            this.chkInspAreaCommonMode1.Text = "共通";
            this.chkInspAreaCommonMode1.UseVisualStyleBackColor = true;
            this.chkInspAreaCommonMode1.CheckedChanged += new System.EventHandler(this.chkInspAreaCommonMode1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(3, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 15);
            this.label2.TabIndex = 15;
            this.label2.Text = "検査幅[㎜]";
            // 
            // spinMaskWidth
            // 
            this.spinMaskWidth.DecimalPlaces = 0;
            this.spinMaskWidth.EveryValueChanged = false;
            this.spinMaskWidth.Font = new System.Drawing.Font("MS UI Gothic", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spinMaskWidth.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinMaskWidth.Location = new System.Drawing.Point(166, 35);
            this.spinMaskWidth.Margin = new System.Windows.Forms.Padding(0);
            this.spinMaskWidth.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.spinMaskWidth.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinMaskWidth.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinMaskWidth.Name = "spinMaskWidth";
            this.spinMaskWidth.Size = new System.Drawing.Size(143, 44);
            this.spinMaskWidth.TabIndex = 23;
            this.spinMaskWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinMaskWidth.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.spinMaskWidth.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinMaskWidth_ValueChanged);
            // 
            // spinWidth
            // 
            this.spinWidth.DecimalPlaces = 0;
            this.spinWidth.EveryValueChanged = false;
            this.spinWidth.Font = new System.Drawing.Font("MS UI Gothic", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spinWidth.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinWidth.Location = new System.Drawing.Point(6, 35);
            this.spinWidth.Margin = new System.Windows.Forms.Padding(0);
            this.spinWidth.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.spinWidth.Minimum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spinWidth.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinWidth.Name = "spinWidth";
            this.spinWidth.Size = new System.Drawing.Size(156, 44);
            this.spinWidth.TabIndex = 23;
            this.spinWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinWidth.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.spinWidth.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinWidth_ValueChanged);
            // 
            // spinMaskShift
            // 
            this.spinMaskShift.DecimalPlaces = 0;
            this.spinMaskShift.EveryValueChanged = false;
            this.spinMaskShift.Font = new System.Drawing.Font("MS UI Gothic", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.spinMaskShift.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spinMaskShift.Location = new System.Drawing.Point(315, 35);
            this.spinMaskShift.Margin = new System.Windows.Forms.Padding(0);
            this.spinMaskShift.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.spinMaskShift.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinMaskShift.MinimumSize = new System.Drawing.Size(0, 20);
            this.spinMaskShift.Name = "spinMaskShift";
            this.spinMaskShift.Size = new System.Drawing.Size(143, 44);
            this.spinMaskShift.TabIndex = 23;
            this.spinMaskShift.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.spinMaskShift.Value = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.spinMaskShift.ValueChanged += new Fujita.InspectionSystem.ValueChangeSEventHandler(this.spinMaskShift_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(163, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 15);
            this.label3.TabIndex = 17;
            this.label3.Text = "状態監視幅[㎜]";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label13.Location = new System.Drawing.Point(312, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 15);
            this.label13.TabIndex = 19;
            this.label13.Text = "シフト[㎜]";
            // 
            // uclRecipeInspectWidth
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox5);
            this.Name = "uclRecipeInspectWidth";
            this.Size = new System.Drawing.Size(598, 105);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label2;
        private Fujita.InspectionSystem.uclNumericInputSmall spinMaskWidth;
        private Fujita.InspectionSystem.uclNumericInputSmall spinWidth;
        private Fujita.InspectionSystem.uclNumericInputSmall spinMaskShift;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkInspAreaCommonMode2;
        private System.Windows.Forms.CheckBox chkInspAreaCommonMode1;
        private System.Windows.Forms.Label lblCmnMaskWidth;
        private System.Windows.Forms.Label lblCmnInspWidth;
        private System.Windows.Forms.Label lblCmnMaskShift;
        private System.Windows.Forms.Button btnSetCommon;
    }
}
