using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fujita.InspectionSystem
{
    public partial class uclLightControl : UserControl
    {
        public event LightEnableChangedEventHandler LightEnableChanged;
        public event LightValueChangedEventHadler LightValueChanged;

        public uclLightControl()
        {
            InitializeComponent();
            setEvet();
        }

        public bool LightEnabled
        {
            get
            {
                return chkLightEnable.Checked;
            }
            set
            {
                resetEvent();
                chkLightEnable.Checked = value;
                setEvet();
            }
        }
        public string LigthName
        {
            get
            {
                return chkLightEnable.Text;
            }
            set
            {
                chkLightEnable.Text = value;
            }
        }

        public int LightValueMin
        {
            get
            {
                return (int)spinLightValue.Minimum;
            }
            set
            {
                spinLightValue.Minimum = value;
                sclLightValue.Minimum = value;
            }
        }

        public int LightValueMax
        {
            get
            {
                return (int)spinLightValue.Maximum;
            }
            set
            {
                spinLightValue.Maximum = value;
                sclLightValue.Maximum = value;
            }
        }

        public int LightValue
        {
            get
            {
                return (int)spinLightValue.Value;
            }
            set
            {
                resetEvent();
                spinLightValue.Value = value;
                sclLightValue.Value = value;
                setEvet();
            }
        }
        private void updateControls()
        {
        }

        private void setEvet()
        {
            chkLightEnable.CheckedChanged += new EventHandler(chkLightEnable_CheckedChanged);

            this.spinLightValue.ValueChanged += new System.EventHandler(this.spinLightValue_ValueChanged);
            this.sclLightValue.Scroll += new System.EventHandler(this.sclLightValue_Scroll);

        }

        private void resetEvent()
        {
            chkLightEnable.CheckedChanged -= new EventHandler(chkLightEnable_CheckedChanged);

            this.spinLightValue.ValueChanged -= new System.EventHandler(this.spinLightValue_ValueChanged);
            this.sclLightValue.Scroll -= new System.EventHandler(this.sclLightValue_Scroll);
        }

        private void spinLightValue_ValueChanged(object sender, EventArgs e)
        {
            resetEvent();
            sclLightValue.Value = (int)spinLightValue.Value;
            setEvet();

            LightValueChanged(this, new LightValueChangedEventArgs(chkLightEnable.Checked, (int)spinLightValue.Value));
        }
        private void sclLightValue_Scroll(object sender, EventArgs e)
        {
            resetEvent();
            spinLightValue.Value = sclLightValue.Value;
            setEvet();

            LightValueChanged(this, new LightValueChangedEventArgs(chkLightEnable.Checked, (int)sclLightValue.Value));
        }

        private void chkLightEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (LightEnableChanged != null)
                LightEnableChanged(this, new LightEnableChangedEventArgs(chkLightEnable.Checked, (int)spinLightValue.Value));
            updateControls();
        }

        public class LightEnableChangedEventArgs : EventArgs
        {
            public bool Enable { get; private set; }
            public int Value { get; private set; }

            public LightEnableChangedEventArgs(bool bEnable, int iValue)
            {
                Enable = bEnable;
                Value = iValue;
            }
        }
        public delegate void LightEnableChangedEventHandler(object sender, LightEnableChangedEventArgs e);

        public class LightValueChangedEventArgs
        {
            public bool Enable { get; private set; }
            public int Value { get; private set; }

            public LightValueChangedEventArgs(bool bEnable, int iValue)
            {
                Enable = bEnable;
                Value = iValue;
            }
        }
        public delegate void LightValueChangedEventHadler(object sender, LightValueChangedEventArgs e);







        public int CameraIndex
        {
            get { return (int)spinCameraIndex.Value - 1; }
            set { spinCameraIndex.Value = value + 1; }
        }
        public int BaseGrayValue
        {
            get { return (int)spinGrayBase.Value; }
            set { spinGrayBase.Value = value; }
        }
        public int BaseLightValue
        {
            get { return (int)spinLightBase.Value; }
            set { spinLightBase.Value = value; }
        }
        public int DiffLightValue
        {
            get { return (int)spinLightDiff.Value; }
            set { spinLightDiff.Value = value; }
        }


        public delegate void LightRoiButtonClickedEventHandler(object sender, EventArgs e);
        public event LightRoiButtonClickedEventHandler LightRoiButtonClicked;
        private void btnRoi_Click(object sender, EventArgs e)
        {
            if (LightRoiButtonClicked != null)
                LightRoiButtonClicked(this, new EventArgs());
        }

        /// <summary>
        /// 現在のGray値
        /// </summary>
        /// <param name="grayVal"></param>
        public void SetNowGrayValue(int grayVal)
        {
            lblGrayNow.Text = grayVal.ToString();
        }
        /// <summary>
        /// Gray基準値変更チェックボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkGrayBaseEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGrayBaseEnable.Checked == true)
            {
                string msgStr = "基準値の変更を実施しますか？";
                frmMessageForm mes = new frmMessageForm(msgStr, MessageType.YesNo, null);
                System.Windows.Forms.DialogResult res = mes.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.Cancel)
                {
                    chkGrayBaseEnable.Checked = false;
                    return;
                }
            }
            spinGrayBase.Enabled = chkGrayBaseEnable.Checked;
        }
        /// <summary>
        /// 差　変更チェックボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkLightDiffEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkLightDiffEnable.Checked == true)
            {
                string msgStr = "差の変更を実施しますか？";
                frmMessageForm mes = new frmMessageForm(msgStr, MessageType.YesNo, null);
                System.Windows.Forms.DialogResult res = mes.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.Cancel)
                {
                    chkLightDiffEnable.Checked = false;
                    return;
                }
            }
            spinLightDiff.Enabled = chkLightDiffEnable.Checked;
        }
        /// <summary>
        /// 登録
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEntryLightBase_Click(object sender, EventArgs e)
        {
            string msgStr = "現在の照明値を基準値として登録しますか？";
            frmMessageForm mes = new frmMessageForm(msgStr, MessageType.YesNo, null);
            System.Windows.Forms.DialogResult res = mes.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.Cancel)
                return;
            spinLightBase.Value = spinLightValue.Value;
        }

        public delegate void LightDiffButtonClickedEventHandler(object sender, EventArgs e);
        public event LightDiffButtonClickedEventHandler LightDiffButtonClicked;
        /// <summary>
        /// 算出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalcLightDiff_Click(object sender, EventArgs e)
        {
            string msgStr = "現在の差を登録しますか？";
            frmMessageForm mes = new frmMessageForm(msgStr, MessageType.YesNo, null);
            System.Windows.Forms.DialogResult res = mes.ShowDialog();
            if (res == System.Windows.Forms.DialogResult.Cancel)
                return;
            int diffValue = (int)(spinLightBase.Value - spinLightValue.Value);
            spinLightDiff.Value = diffValue;

            if (LightDiffButtonClicked != null)
                LightDiffButtonClicked(this, new EventArgs());
        }
    }
}
