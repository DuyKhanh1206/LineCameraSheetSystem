using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using HalconDotNet;
using Fujita.HalconMisc;

using Fujita.Misc;

namespace Fujita.InspectionSystem
{
    public delegate void ThresholdUserSettingEventHandler(object sender, ThresholdUserSettingEventArgs e);

    public partial class frmThreshold : Form
    {
        public event ThresholdUserSettingEventHandler UserSettingChange;
        public frmThreshold()
        {
            InitializeComponent();
        }

        public enum EThresholdType
        {
            Low,
            High,
            Both,
        }

        EThresholdType _eThresholdType = EThresholdType.Both;
        public EThresholdType ThresholdType
        {
            get { return _eThresholdType; }
            set
            {
                _eThresholdType = value;
            }
        }

        CRectangle1 _rectHistArea;
        IMainFormEqu _MainForm;
        public frmThreshold(int iLow, int iHigh, double row1, double col1, double row2, double col2, string sMessage, IMainFormEqu mainform, EThresholdType type = EThresholdType.Both )
        {
            InitializeComponent();

            _rectHistArea = new CRectangle1(col1, row1, col2 - col1, row2 - row1);
            _MainForm = mainform;

            resetControlEvent();

            if (iLow >= 0)
            {
                uniLowThreshold.Value = iLow;
                trbLowThreshold.Value = iLow;
            }
            else
            {
                uniLowThreshold.Value = 0;
                trbLowThreshold.Value = 0;
            }

            if (iHigh >= 0)
            {
                uniHighThreshold.Value = iHigh;
                trbHighThreshold.Value = iHigh;
            }
            else
            {
                uniHighThreshold.Value = 255;
                trbHighThreshold.Value = 255;
            }

            _eThresholdType = type;

            setControlEvent();

            lblMessage.Text = sMessage;

            initControl();

            initChart();
        }

        private void setControlEvent()
        {
            trbLowThreshold.ValueChanged += trbLowThreshold_ValueChanged;
            trbHighThreshold.ValueChanged += trbHighThreshold_ValueChanged;
            uniLowThreshold.ValueChanged += nudLowThreshold_ValueChanged;
            uniHighThreshold.ValueChanged += nudHighThreshold_ValueChanged;
        }

        private void resetControlEvent()
        {
            trbLowThreshold.ValueChanged -= trbLowThreshold_ValueChanged;
            trbHighThreshold.ValueChanged -= trbHighThreshold_ValueChanged;
            uniLowThreshold.ValueChanged -= nudLowThreshold_ValueChanged;
            uniHighThreshold.ValueChanged -= nudHighThreshold_ValueChanged;
        }

        private void nudLowThreshold_ValueChanged(object sender, EventArgs e)
        {
            trbLowThreshold.Value = (int)uniLowThreshold.Value;
        }

        private void nudHighThreshold_ValueChanged(object sender, EventArgs e)
        {
            trbHighThreshold.Value = (int)uniHighThreshold.Value;
        }

        private void trbLowThreshold_ValueChanged(object sender, EventArgs e)
        {
            uniLowThreshold.Value = trbLowThreshold.Value;
            if (UserSettingChange != null)
            {
                UserSettingChange(this, new ThresholdUserSettingEventArgs(UserSettingChangeType.ValueChange, (int)(int)trbLowThreshold.Value, (int)trbHighThreshold.Value));
            }
            refreshChart();
        }

        private void trbHighThreshold_ValueChanged(object sender, EventArgs e)
        {
            uniHighThreshold.Value = trbHighThreshold.Value;
            if (UserSettingChange != null)
            {
                UserSettingChange(this, new ThresholdUserSettingEventArgs(UserSettingChangeType.ValueChange, (int)(int)trbLowThreshold.Value, (int)trbHighThreshold.Value));
            }
            refreshChart();
        }

        private void initControl()
        {
            if (_eThresholdType == EThresholdType.Low)
            {
                uniHighThreshold.Visible = false;
                trbHighThreshold.Visible = false;
            }

            if (_eThresholdType == EThresholdType.High)
            {
                uniLowThreshold.Visible = false;
                trbLowThreshold.Visible = false;
            }
        }

        HTuple _htHistAbs;
        private bool initChart()
        {
            HObject region = null;
            try
            {
                HObject img = _MainForm.GetCurrentImage();
                if (img != null)
                {
                    HTuple htHistRel;
                    HOperatorSet.GenRectangle1(out region, _rectHistArea.Row1, _rectHistArea.Col1, _rectHistArea.Row2, _rectHistArea.Col2);
                    HOperatorSet.GrayHisto(region, img, out _htHistAbs, out htHistRel);

                    HTuple htMax;
                    HOperatorSet.TupleMax(_htHistAbs, out htMax);

                    // 最大値からチャートのサイズを決定する
                    double dChartYAxis;
                    if (htMax.I < 10000)
                    {
                        dChartYAxis = 10000D;
                    }
                    else
                    {
                        dChartYAxis = ((htMax.I / 10000) + 1) * 10000D;
                    }

                    chartHist.ChartAreas[0].AxisY.Maximum = dChartYAxis;

                    makeMagnifyCombo(dChartYAxis);
                    cmbMagnify.SelectedIndex = 0;
                    modifyTrackBarSize(dChartYAxis);
                }
                else
                {
                    return false;
                }
            }
            catch (HOperatorException)
            {
                return false;
            }
            finally
            {
                HalconExtFunc.Clear(ref region);
            }
            refreshChart();
            return true;
        }

        int[] iMag = new int []{ 10000000, 5000000, 2500000, 1000000, 750000, 500000, 250000, 100000, 75000, 50000, 25000, 10000, };

        private void makeMagnifyCombo( double dMax )
        {
            int iMax = (int)dMax;

            cmbMagnify.Items.Add(iMax.ToString());

            for (int i = 0; i < iMag.Length; i++)
            {
                if (iMag[i] > iMax)
                    continue;
                cmbMagnify.Items.Add(iMag[i].ToString());
            }
        }

        private void modifyTrackBarSize(double dMag)
        {
            if (dMag < 90000)
            {
                trbLowThreshold.Location = new Point(92, trbLowThreshold.Location.Y);
                trbLowThreshold.Size = new Size(301, trbLowThreshold.Size.Height);
                trbHighThreshold.Location = new Point(92, trbHighThreshold.Location.Y);
                trbHighThreshold.Size = new Size(301, trbHighThreshold.Size.Height);
            }
            else if (dMag >= 90000 && dMag < 900000)
            {
                trbLowThreshold.Location = new Point(100, trbLowThreshold.Location.Y);
                trbLowThreshold.Size = new Size(293, trbLowThreshold.Size.Height);
                trbHighThreshold.Location = new Point(100, trbHighThreshold.Location.Y);
                trbHighThreshold.Size = new Size(293, trbHighThreshold.Size.Height);
            }
            else
            {
                trbLowThreshold.Location = new Point(108, trbLowThreshold.Location.Y);
                trbLowThreshold.Size = new Size(285, trbLowThreshold.Size.Height);
                trbHighThreshold.Location = new Point(108, trbHighThreshold.Location.Y);
                trbHighThreshold.Size = new Size(285, trbHighThreshold.Size.Height);
            }
        }

        private void refreshChart()
        {
            chartHist.Series[0].Points.Clear();
            chartHist.Series[1].Points.Clear();
            chartHist.Series[2].Points.Clear();
            chartHist.Series[3].Points.Clear();

            if (trbLowThreshold.Value <= trbHighThreshold.Value)
            {
                for (int i = 0; i < trbLowThreshold.Value; i++)
                {
                    chartHist.Series[0].Points.Add(new DataPoint(i, _htHistAbs[i].D));
                }
                for (int i = trbLowThreshold.Value; i < trbHighThreshold.Value; i++)
                {
                    chartHist.Series[1].Points.Add(new DataPoint(i, _htHistAbs[i].D));
                }
                for (int i = trbHighThreshold.Value; i <= 255; i++)
                {
                    chartHist.Series[2].Points.Add(new DataPoint(i, _htHistAbs[i].D));
                }
            }
            else
            {
                for (int i = 0; i < trbHighThreshold.Value; i++)
                {
                    chartHist.Series[1].Points.Add(new DataPoint(i, _htHistAbs[i].D));
                }
                for (int i = trbHighThreshold.Value; i < trbLowThreshold.Value; i++)
                {
                    chartHist.Series[2].Points.Add(new DataPoint(i, _htHistAbs[i].D));
                }
                for (int i = trbLowThreshold.Value; i <= 255; i++)
                {
                    chartHist.Series[3].Points.Add(new DataPoint(i, _htHistAbs[i].D));
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (UserSettingChange != null)
            {
                UserSettingChange(this, new ThresholdUserSettingEventArgs(UserSettingChangeType.OK, (int)(int)trbLowThreshold.Value, (int)trbHighThreshold.Value));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (UserSettingChange != null)
            {
                UserSettingChange(this, new ThresholdUserSettingEventArgs(UserSettingChangeType.Cancel, (int)(int)trbLowThreshold.Value, (int)trbHighThreshold.Value));
            }
        }

        private void cmbMagnify_SelectedIndexChanged(object sender, EventArgs e)
        {
            double dAreaYAxis = double.Parse(cmbMagnify.Text);
            chartHist.ChartAreas[0].AxisY.Maximum = dAreaYAxis;
            modifyTrackBarSize(dAreaYAxis);
        }
    }

    public class ThresholdUserSettingEventArgs : EventArgs
    {
        public UserSettingChangeType Type { get; private set; }

        public int Low { get; private set; }
        public int High { get; private set; }

        public ThresholdUserSettingEventArgs(UserSettingChangeType type, int iLow, int iHigh)
        {
            Type = type;
            Low = iLow;
            High = iHigh;
        }
    }

}
