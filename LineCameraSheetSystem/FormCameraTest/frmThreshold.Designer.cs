namespace Fujita.InspectionSystem
{
    partial class frmThreshold
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartHist = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.trbLowThreshold = new System.Windows.Forms.TrackBar();
            this.trbHighThreshold = new System.Windows.Forms.TrackBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.cmbMagnify = new System.Windows.Forms.ComboBox();
            this.uniLowThreshold = new Fujita.InspectionSystem.uclNumericInput();
            this.uniHighThreshold = new Fujita.InspectionSystem.uclNumericInput();
            ((System.ComponentModel.ISupportInitialize)(this.chartHist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbLowThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbHighThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // chartHist
            // 
            chartArea1.AxisX.IsMarginVisible = false;
            chartArea1.AxisX.Maximum = 255D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisY.IsMarginVisible = false;
            chartArea1.AxisY.Maximum = 10000D;
            chartArea1.AxisY.Minimum = 0D;
            chartArea1.Name = "ChartArea1";
            this.chartHist.ChartAreas.Add(chartArea1);
            this.chartHist.Location = new System.Drawing.Point(37, 179);
            this.chartHist.Name = "chartHist";
            this.chartHist.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.None;
            this.chartHist.PaletteCustomColors = new System.Drawing.Color[] {
        System.Drawing.Color.Cyan,
        System.Drawing.Color.Magenta,
        System.Drawing.Color.Cyan,
        System.Drawing.Color.Magenta};
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series1.Name = "Series1";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series2.Name = "Series2";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series3.Name = "Series3";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series4.Name = "Series4";
            this.chartHist.Series.Add(series1);
            this.chartHist.Series.Add(series2);
            this.chartHist.Series.Add(series3);
            this.chartHist.Series.Add(series4);
            this.chartHist.Size = new System.Drawing.Size(368, 216);
            this.chartHist.TabIndex = 0;
            this.chartHist.Text = "chart1";
            // 
            // trbLowThreshold
            // 
            this.trbLowThreshold.Location = new System.Drawing.Point(89, 399);
            this.trbLowThreshold.Maximum = 255;
            this.trbLowThreshold.Name = "trbLowThreshold";
            this.trbLowThreshold.Size = new System.Drawing.Size(301, 42);
            this.trbLowThreshold.TabIndex = 1;
            this.trbLowThreshold.TickFrequency = 16;
            this.trbLowThreshold.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trbLowThreshold.ValueChanged += new System.EventHandler(this.trbLowThreshold_ValueChanged);
            // 
            // trbHighThreshold
            // 
            this.trbHighThreshold.Location = new System.Drawing.Point(89, 134);
            this.trbHighThreshold.Maximum = 255;
            this.trbHighThreshold.Name = "trbHighThreshold";
            this.trbHighThreshold.Size = new System.Drawing.Size(303, 42);
            this.trbHighThreshold.TabIndex = 1;
            this.trbHighThreshold.TickFrequency = 16;
            this.trbHighThreshold.ValueChanged += new System.EventHandler(this.trbHighThreshold_ValueChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnCancel.Location = new System.Drawing.Point(226, 512);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(164, 51);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "キャンセル";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btnOK.Location = new System.Drawing.Point(27, 512);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(166, 51);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblMessage.Location = new System.Drawing.Point(2, 0);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(411, 64);
            this.lblMessage.TabIndex = 6;
            this.lblMessage.Text = "label1\r\n";
            // 
            // cmbMagnify
            // 
            this.cmbMagnify.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMagnify.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.cmbMagnify.FormattingEnabled = true;
            this.cmbMagnify.Location = new System.Drawing.Point(250, 95);
            this.cmbMagnify.Name = "cmbMagnify";
            this.cmbMagnify.Size = new System.Drawing.Size(163, 32);
            this.cmbMagnify.TabIndex = 7;
            this.cmbMagnify.SelectedIndexChanged += new System.EventHandler(this.cmbMagnify_SelectedIndexChanged);
            // 
            // uniLowThreshold
            // 
            this.uniLowThreshold.DecimalPlaces = 0;
            this.uniLowThreshold.EveryValueChanged = true;
            this.uniLowThreshold.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.uniLowThreshold.Location = new System.Drawing.Point(12, 444);
            this.uniLowThreshold.Margin = new System.Windows.Forms.Padding(0);
            this.uniLowThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.uniLowThreshold.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.uniLowThreshold.MinimumSize = new System.Drawing.Size(124, 56);
            this.uniLowThreshold.Name = "uniLowThreshold";
            this.uniLowThreshold.Size = new System.Drawing.Size(181, 56);
            this.uniLowThreshold.TabIndex = 8;
            this.uniLowThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.uniLowThreshold.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // uniHighThreshold
            // 
            this.uniHighThreshold.DecimalPlaces = 0;
            this.uniHighThreshold.EveryValueChanged = true;
            this.uniHighThreshold.Incriment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.uniHighThreshold.Location = new System.Drawing.Point(12, 75);
            this.uniHighThreshold.Margin = new System.Windows.Forms.Padding(0);
            this.uniHighThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.uniHighThreshold.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.uniHighThreshold.MinimumSize = new System.Drawing.Size(124, 56);
            this.uniHighThreshold.Name = "uniHighThreshold";
            this.uniHighThreshold.Size = new System.Drawing.Size(181, 56);
            this.uniHighThreshold.TabIndex = 8;
            this.uniHighThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.uniHighThreshold.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // frmThreshold
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 576);
            this.ControlBox = false;
            this.Controls.Add(this.uniLowThreshold);
            this.Controls.Add(this.uniHighThreshold);
            this.Controls.Add(this.cmbMagnify);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.trbHighThreshold);
            this.Controls.Add(this.trbLowThreshold);
            this.Controls.Add(this.chartHist);
            this.Name = "frmThreshold";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "しきい値設定";
            ((System.ComponentModel.ISupportInitialize)(this.chartHist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbLowThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trbHighThreshold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartHist;
        private System.Windows.Forms.TrackBar trbLowThreshold;
        private System.Windows.Forms.TrackBar trbHighThreshold;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ComboBox cmbMagnify;
        private uclNumericInput uniHighThreshold;
        private uclNumericInput uniLowThreshold;
    }
}