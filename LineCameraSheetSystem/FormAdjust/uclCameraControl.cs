using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Adjustment
{

    public partial class uclCameraControl : UserControl
    {
        public uclCameraControl()
        {
            InitializeComponent();
            setEvent();
        }

        public bool EnableCamera
        {
            get 
            {
                return chkCamera.Checked;
            }
            set
            {
                chkCamera.Checked = value;
                updateControls();
            }
        }

        public void updateControls()
        {
            foreach (Control c in Controls)
            {
                if (chkCamera.Checked)
                {
                    if (chkCamera != c)
                        c.Enabled = true;

                    if (_Camera != null)
                    {
                        if (_Camera.EnableGain)
                        {
                            trbGain.Enabled = true;
                            nudGain.Enabled = true;
                        }
                        else
                        {
                            trbGain.Enabled = false;
                            nudGain.Enabled = false;
                        }

                        if (_Camera.EnableOffset)
                        {
                            trbOffset.Enabled = true;
                            nudOffset.Enabled = true;
                        }
                        else
                        {
                            trbOffset.Enabled = false;
                            nudOffset.Enabled = false;
                        }

                        if (_Camera.EnableExposureTime)
                        {
                            trbExposure.Enabled = true;
                            nudExposure.Enabled = true;
                        }
                        else
                        {
                            trbExposure.Enabled = false;
                            nudExposure.Enabled = false;
                        }

                        if (_Camera.IsLineSensor)
                        {
                            lblLineRate.Enabled = true;
                            nudLineRate.Enabled = true;
                        }
                        else
                        {
                            lblLineRate.Enabled = false;
                            nudLineRate.Enabled = false;
                        }

                        if (_Camera.EnableTriggerDelay)
                        {
                            label7.Enabled = true;
                            nudTriggerDelay.Enabled = true;
                        }
                        else
                        {
                            label7.Enabled = false;
                            nudTriggerDelay.Enabled = false;
                        }
                    }
                    else
                    {
                        trbGain.Enabled = false;
                        nudGain.Enabled = false;
                        trbOffset.Enabled = false;
                        nudOffset.Enabled = false;
                        trbExposure.Enabled = false;
                        nudExposure.Enabled = false;
                        lblLineRate.Enabled = false;
                        nudLineRate.Enabled = false;
                        label7.Enabled = false;
                        nudTriggerDelay.Enabled = false;
                    }
                }
                else
                {
                    if (chkCamera != c || (c as Label) != null )
                    {
                        c.Enabled = false;
                    }
                }
            }
        }

        HalconCamera.HalconCameraBase _Camera = null;

        private int _GainOrg = 0;
        private int _OffsetOrg = 0;
        private int _ExposureTimeOrg = 0;
        private double _TriggerDelayOrg = 0;
        private double _LineRateOrg = 0.0;

        private void RestoreCamera()
        {
            if (_Camera == null)
                return;

            if (_Camera.EnableGain)
            {
                _Camera.Gain = _GainOrg;
            }

            if (_Camera.EnableOffset)
            {
                _Camera.Offset = _OffsetOrg;
            }

            if (_Camera.EnableExposureTime)
            {
                _Camera.ExposureTime = _ExposureTimeOrg;
            }

            if (_Camera.EnableTriggerDelay)
            {
                _Camera.TriggerDelay = _TriggerDelayOrg;
            }
        }

        public void ResetCamera()
        {
           // ResetCamera();
            _Camera = null;
            updateControls();
        }

        public bool SetCamera(HalconCamera.HalconCameraBase camera)
        {
            RestoreCamera();

            _Camera = camera;

            if (camera == null)
            {
                updateControls();
                return false;
            }

            chkCamera.Text = camera.Name;
            lblWidth.Text = camera.ImageWidth.ToString();
            lblHeight.Text = camera.ImageHeight.ToString();

            resetEvent();

            try
            {
                if (camera.EnableGain)
                {
                    trbGain.Minimum = camera.GainMin;
                    trbGain.Maximum = camera.GainMax;
                    AdjustFrequency(trbGain, 10);
                    nudGain.Minimum = camera.GainMin;
                    nudGain.Maximum = camera.GainMax;

                    trbGain.Value = camera.Gain;
                    nudGain.Value = camera.Gain;

                    _GainOrg = camera.Gain;
                }

                if (camera.EnableOffset)
                {
                    trbOffset.Minimum = camera.OffsetMin;
                    trbOffset.Maximum = camera.OffsetMax;
                    AdjustFrequency(trbOffset, 10);
                    nudOffset.Minimum = camera.OffsetMin;
                    nudOffset.Maximum = camera.OffsetMax;

                    trbOffset.Value = camera.Offset;
                    nudOffset.Value = camera.Offset;

                    _OffsetOrg = camera.Offset;
                }

                if (camera.EnableExposureTime)
                {
                    trbExposure.Minimum = camera.ExposureTimeMin;
                    trbExposure.Maximum = camera.ExposureTimeMax;
                    AdjustFrequency(trbExposure, 10);
                    nudExposure.Minimum = camera.ExposureTimeMin;
                    nudExposure.Maximum = camera.ExposureTimeMax;

                    trbExposure.Value = camera.ExposureTime;
                    nudExposure.Value = camera.ExposureTime;

                    _ExposureTimeOrg = camera.ExposureTime;
                }

                if (camera.IsLineSensor)
                {
                    lblLineRate.Text = camera.GetResultingLineRate().ToString("F3") + "[Hz]";

                    nudLineRate.Minimum = (decimal)camera.LineRateMin;
                    nudLineRate.Maximum = (decimal)camera.LineRateMax + 1;
                    nudLineRate.Value = (decimal)camera.LineRate;
                    AdjustFrequency(nudLineRate, 10);
                    _LineRateOrg = camera.LineRate;
                }

                if (camera.EnableTriggerDelay)
                {
                    nudTriggerDelay.Minimum = (decimal)camera.TriggerDelayMin;
                    nudTriggerDelay.Maximum = (decimal)camera.TriggerDelayMax;
                    nudTriggerDelay.Value = (decimal)((camera.TriggerDelay < camera.TriggerDelayMin || camera.TriggerDelayMax < camera.TriggerDelay) ? camera.TriggerDelayMin : camera.TriggerDelay);
                    AdjustFrequency(nudTriggerDelay, 100);
                    _TriggerDelayOrg = camera.TriggerDelay;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                setEvent();
                updateControls();
            }

            return true;
        }

        private void AdjustFrequency(TrackBar trb, int iStep)
        {
            int Diff = trb.Maximum - trb.Minimum;
            if (Diff > iStep)
            {
                trb.TickFrequency = ( Diff / iStep ) + 1;
            }
        }

        private void AdjustFrequency(NumericUpDown nud, decimal decStep)
        {
            decimal decDiff = nud.Maximum - nud.Minimum;
            if (decDiff > decStep)
            {
                nud.Increment =  decStep; 
            }
            else
            {
                nud.Increment = 1;
            }
        }

        bool _bUserSet = false;
        public int Gain
        {
            get
            {
                return (int)nudGain.Value;
            }
            set
            {
                if (value < nudGain.Minimum || value > nudGain.Maximum)
                    return;
                _bUserSet = true;
                nudGain.Value = value;
                _bUserSet = false;
            }
        }

        public int Offset
        {
            get
            {
                return (int)nudOffset.Value;
            }
            set
            {
                if (value < nudOffset.Minimum || value > nudOffset.Maximum)
                    return;
                _bUserSet = true;
                nudOffset.Value = value;
                _bUserSet = false;
            }
        }

        public int ExposureTime
        {
            get
            {
                return (int)nudExposure.Value;
            }
            set
            {
                if (value < nudExposure.Minimum || value > nudExposure.Maximum)
                    return;
                _bUserSet = true;
                nudExposure.Value = value;
                _bUserSet = false;
            }
        }

        public double TriggerDelay
        {
            get
            {
                return (double)nudTriggerDelay.Value;
            }
            set
            {
                if (value < (double)nudTriggerDelay.Minimum || value > (double)nudTriggerDelay.Maximum)
                    return;
                _bUserSet = true;
                nudTriggerDelay.Value = (decimal)value;
                _bUserSet = false;
            }
        }

        private void setEvent()
        {
            this.nudGain.ValueChanged += new System.EventHandler(this.nudGain_ValueChanged);
            this.nudOffset.ValueChanged += new System.EventHandler(this.nudOffset_ValueChanged);
            this.nudExposure.ValueChanged += new System.EventHandler(this.nudExposure_ValueChanged);
            this.trbGain.Scroll += new System.EventHandler(this.trbGain_Scroll);
            this.trbOffset.Scroll += new System.EventHandler(this.trbOffset_Scroll);
            this.trbExposure.Scroll += new System.EventHandler(this.trbExposure_Scroll);
            this.nudTriggerDelay.ValueChanged += new EventHandler(nudTriggerDelay_ValueChanged);
            this.nudLineRate.ValueChanged += new EventHandler(nudLineRate_ValueChanged);
        }

        private void resetEvent()
        {
            this.nudGain.ValueChanged -= new System.EventHandler(this.nudGain_ValueChanged);
            this.nudOffset.ValueChanged -= new System.EventHandler(this.nudOffset_ValueChanged);
            this.nudExposure.ValueChanged -= new System.EventHandler(this.nudExposure_ValueChanged);
            this.trbGain.Scroll -= new System.EventHandler(this.trbGain_Scroll);
            this.trbOffset.Scroll -= new System.EventHandler(this.trbOffset_Scroll);
            this.trbExposure.Scroll -= new System.EventHandler(this.trbExposure_Scroll);
            this.nudTriggerDelay.ValueChanged -= new EventHandler(nudTriggerDelay_ValueChanged);
            this.nudLineRate.ValueChanged -= new EventHandler(nudLineRate_ValueChanged);
        }

        private void nudGain_ValueChanged(object sender, EventArgs e)
        {
            resetEvent();
            trbGain.Value =(int) nudGain.Value;
            setEvent();
            if (_Camera != null)
            {
                _Camera.Gain = (int)nudGain.Value;
            }
            if (GainChanged != null && !_bUserSet )
            {
                GainChanged(this, new GainChangedEventArgs(trbGain.Value, _Camera.Index));
            }
        }

        private void nudOffset_ValueChanged(object sender, EventArgs e)
        {
            resetEvent();
            trbOffset.Value = (int)nudOffset.Value;
            setEvent();

            if (_Camera != null)
            {
                _Camera.Offset = (int)nudOffset.Value;
            }

            if (OffsetChanged != null && !_bUserSet)
            {
                OffsetChanged(this, new OffsetChangedEventArgs(trbOffset.Value, _Camera.Index));
            }
        }

        private void nudExposure_ValueChanged(object sender, EventArgs e)
        {
            resetEvent();
            trbExposure.Value = (int)nudExposure.Value;
            setEvent();

            if (_Camera != null)
            {
                _Camera.ExposureTime = (int)nudExposure.Value;
            }

            if (_Camera.IsLineSensor)
            {
                lblLineRate.Text = _Camera.GetResultingLineRate().ToString("F3") + " Hz";
            }
            if ( ExposureChanged != null && !_bUserSet )
            {
                ExposureChanged(this, new ExposureChangedEventArgs(trbExposure.Value, _Camera.Index));
            }
        }

        private void trbGain_Scroll(object sender, EventArgs e)
        {
            resetEvent();
            nudGain.Value = trbGain.Value;
            setEvent();

            if (_Camera != null)
            {
                _Camera.Gain = (int)trbGain.Value;
            }

            if (GainChanged != null && !_bUserSet )
            {
                GainChanged(this, new GainChangedEventArgs(trbGain.Value, _Camera.Index));
            }
        }

        private void trbOffset_Scroll(object sender, EventArgs e)
        {
            resetEvent();
            nudOffset.Value = trbOffset.Value;
            setEvent();

            if (_Camera != null)
            {
                _Camera.Offset = (int)trbOffset.Value;
            }

            if (OffsetChanged != null && !_bUserSet )
            {
                OffsetChanged(this, new OffsetChangedEventArgs(trbOffset.Value, _Camera.Index));
            }
        }

        private void trbExposure_Scroll(object sender, EventArgs e)
        {
            resetEvent();
            nudExposure.Value = trbExposure.Value;
            setEvent();

            if (_Camera != null)
            {
                _Camera.ExposureTime = (int)trbExposure.Value;
            }

            if (_Camera.IsLineSensor)
            {
                lblLineRate.Text = _Camera.GetResultingLineRate().ToString("F3") + "[Hz]";
            }
            if (ExposureChanged != null && !_bUserSet )
            {
                ExposureChanged(this, new ExposureChangedEventArgs(trbExposure.Value, _Camera.Index));
            }
        }

        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        long _lPrevMiliseconds;
        public void StartFrameRate()
        {
            sw.Restart();
            _lPrevMiliseconds = sw.ElapsedMilliseconds;
        }

        public void AdvancedFrameRate()
        {
            if (!sw.IsRunning)
                return;

            long lFrames = sw.ElapsedMilliseconds - _lPrevMiliseconds;
            lblFrameRate.Text = (1000 / (double)lFrames).ToString("F3") + " fps";
            _lPrevMiliseconds = sw.ElapsedMilliseconds;
        }

        public void StopFrameRate()
        {
            sw.Stop();
        }

        public event EnableCameraCheckedChangeEventHandler EnableCameraCheckedChange;
        public event GainChangedEventHandler GainChanged;
        public event OffsetChangedEventHandler OffsetChanged;
        public event ExposureChangedEventHandler ExposureChanged;

        private void chkCamera_CheckedChanged(object sender, EventArgs e)
        {
            updateControls();

            if (EnableCameraCheckedChange != null)
                EnableCameraCheckedChange(this, new EnableCameraCheckedChnageEventArgs(chkCamera.Checked, _Camera.Index));
        }

        private void nudTriggerDelay_ValueChanged(object sender, EventArgs e)
        {
            _Camera.TriggerDelay = (double)nudTriggerDelay.Value;
        }

        private void nudLineRate_ValueChanged(object sender, EventArgs e)
        {
            _Camera.LineRate = (double)nudLineRate.Value;
            lblLineRate.Text = _Camera.GetResultingLineRate().ToString( "F3" ) + "[Hz]";
        }
    }

    public class EnableCameraCheckedChnageEventArgs : EventArgs
    {
        public bool Checked { get; private set; }
        public int Index { get; private set; }

        public EnableCameraCheckedChnageEventArgs( bool bChecked, int iIndex )
        {
            Checked = bChecked;
            Index = iIndex;
        }
    }

    public delegate void EnableCameraCheckedChangeEventHandler( object sender, EnableCameraCheckedChnageEventArgs e );

    public class GainChangedEventArgs : EventArgs
    {
        public int Value { get; private set; }
        public int Index { get; private set; }
        public GainChangedEventArgs(int value, int iIndex)
        {
            Value = value;
            Index = iIndex;
        }
    }
    public delegate void GainChangedEventHandler(object sender, GainChangedEventArgs e);
    public class OffsetChangedEventArgs : EventArgs
    {
        public int Value { get; private set; }
        public int Index { get; private set; }
        public OffsetChangedEventArgs(int value, int iIndex)
        {
            Value = value;
            Index = iIndex;
        }
    }
    public delegate void OffsetChangedEventHandler(object sender, OffsetChangedEventArgs e);
    public class ExposureChangedEventArgs : EventArgs
    {
        public int Value { get; private set; }
        public int Index { get; private set; }
        public ExposureChangedEventArgs(int value, int iIndex)
        {
            Value = value;
            Index = iIndex;
        }
    }
    public delegate void ExposureChangedEventHandler(object sender, ExposureChangedEventArgs e);

}
