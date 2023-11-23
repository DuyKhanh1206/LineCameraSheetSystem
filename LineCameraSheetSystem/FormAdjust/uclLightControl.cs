using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Fujita.LightControl;

namespace Adjustment
{
    public partial class uclLightControl : UserControl
    {
        LightType _light;
        clsTrackbarWait _trbWait;

        public bool Enable
        {
            get
            {
                return chkName.Checked;
            }
            set
            {
                chkName.Checked = value;
            }
        }

        public bool ControlEnable //V1058 メンテナンス追加 yuasa 20190126：チェックボックスの有効無効用
        {
            get
            {
                return chkName.Enabled;
            }
            set
            {
                chkName.Enabled = value;
            }
        }

        public int Value
        {
            get
            {
                return trbLightValue.Value;
            }

            set
            {
                if (value < trbLightValue.Minimum || value > trbLightValue.Maximum)
                    return;
                trbLightValue.Value = value;
                nudLightValue.Value = value;
            }
        }
        public string StdValue //V1058 メンテナンス追加 yuasa 20190126：基準値を追加
        {
            get
            {
                return textStdLightValue.Text;
            }

            set
            {
                textStdLightValue.Text = value;
            }
        }

        public string Difference //V1058 メンテナンス追加 yuasa 20190126：差を追加
        {
            get
            {
                return textDifference.Text;
            }

            set
            {
                textDifference.Text = value;
            }
        }


        public uclLightControl()
        {
            InitializeComponent();
            _trbWait = new clsTrackbarWait(null);
            _trbWait.AddTrackbar(trbLightValue);
            _trbWait.Start = true;
            _trbWait.TrackbarValueChangeTimerElapsed += new TrackbarValueChangeTimerElapsedEvent(_trbWait_TrackbarValueChangeTimerElapsed);

            setEvent();

            initControls();
            updateControls();
        }

        void _trbWait_TrackbarValueChangeTimerElapsed(object sender, TrackbarValueChangeTimerElapsedEventArgs e)
        {
            if (chkName.Checked && _light != null )
            {
                _light.LightOn(trbLightValue.Value);
            }
        }

        private void chkName_CheckedChanged(object sender, EventArgs e)
        {
            if (_light != null)
            {
                if (chkName.Checked)
                    _light.LightOn(trbLightValue.Value);
                else
                    _light.LightOff();
            }
        }

        public void SetLight(LightType light)
        {
            _light = light;

            initControls();
            updateControls();
        }

        private void setEvent()
        {
            trbLightValue.ValueChanged += new EventHandler(trbLightValue_ValueChanged);
            nudLightValue.ValueChanged += new EventHandler(nudLightValue_ValueChanged);
        }

        void nudLightValue_ValueChanged(object sender, EventArgs e)
        {
            trbLightValue.Value = (int)nudLightValue.Value;
            DifferenceCalc(); //V1058 メンテナンス追加 yuasa 20190126：差の計算追加
        }

        void trbLightValue_ValueChanged(object sender, EventArgs e)
        {
            resetEvent();
            nudLightValue.Value = trbLightValue.Value;
            DifferenceCalc(); //V1058 メンテナンス追加 yuasa 20190126：差の計算追加
            setEvent();
        }

        private void resetEvent()
        {
            trbLightValue.ValueChanged -= new EventHandler(trbLightValue_ValueChanged);
            nudLightValue.ValueChanged -= new EventHandler(nudLightValue_ValueChanged);
        }

        private void initControls()
        {
            resetEvent();
            if (_light != null)
            {
                chkName.Text = _light.Name;

                trbLightValue.Minimum = _light.ValueMin;
                trbLightValue.Maximum = _light.ValueMax;

                nudLightValue.Minimum = _light.ValueMin;
                nudLightValue.Maximum = _light.ValueMax;

                int iGrade = _light.ValueMax - _light.ValueMin + 1;
                switch (iGrade)
                {
                    case 64:
                        trbLightValue.LargeChange = trbLightValue.TickFrequency = 8;
                        break;
                    case 256:
                        trbLightValue.LargeChange = trbLightValue.TickFrequency = 32;
                        break;
                    case 1000:
                        trbLightValue.LargeChange = trbLightValue.TickFrequency = 100;
                        break;
                }
            }
            setEvent();
        }

        private void updateControls()
        {
            if (_light != null)
            {
                chkName.Enabled = true;

                trbLightValue.Enabled = true;
                nudLightValue.Enabled = true;

                label1.Enabled = true; //V1058 メンテナンス追加 yuasa 20190126：追記
                label2.Enabled = true; //V1058 メンテナンス追加 yuasa 20190126：追記
                label3.Enabled = true; //V1058 メンテナンス追加 yuasa 20190126：追記
            }
            else
            {
                chkName.Enabled = false;

                trbLightValue.Enabled = false;
                nudLightValue.Enabled = false;

                label1.Enabled = false; //V1058 メンテナンス追加 yuasa 20190126：追記
                label2.Enabled = false; //V1058 メンテナンス追加 yuasa 20190126：追記
                label3.Enabled = false; //V1058 メンテナンス追加 yuasa 20190126：追記
            }
        }

        /// <summary>
        /// 照明値（現在値）を基準照明値にコピー：V1058 メンテナンス追加 yuasa 20190126：
        /// </summary>
        public void CopyStdLightValue()
        {
            textStdLightValue.Text = nudLightValue.Value.ToString();
            DifferenceCalc();
        }

        /// <summary>
        /// 差を計算して表示する：V1058 メンテナンス追加 yuasa 20190126：
        /// </summary>
        public void DifferenceCalc()
        {
            if (textStdLightValue.Text != "")
            {
                int diff = (int)nudLightValue.Value - int.Parse(textStdLightValue.Text);
                textDifference.Text = diff.ToString();
            }
        }

    }
}
