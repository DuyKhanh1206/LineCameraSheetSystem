using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineCameraSheetSystem
{
    public partial class uclRecipeLightControl : UserControl
    {
        public delegate void ChangedValueEventHandler(object sender, EventArgs arg);
        public event ChangedValueEventHandler OnLightValueChanged;
        public delegate void ChangedEnableEventHandler(object sender, EventArgs arg);
        public event ChangedEnableEventHandler OnLightEnableChanged;

        public string LightName
        {
            get { return lblLightName.Text; }
            set { lblLightName.Text = value; }
        }
        public int LightValue
        {
            get { return (int)spinLightValue.Value; }
            set { spinLightValue.Value = value; }
        }
        public int LightMaxValue
        {
            get { return (int)spinLightValue.Maximum; }
            set { spinLightValue.Maximum = value; }
        }
        public bool LightEnable
        {
            get { return chkLightEnable.Checked; }
            set { chkLightEnable.Checked = value; }
        }
        public uclRecipeLightControl()
        {
            InitializeComponent();
        }

        private void chkLightEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (OnLightEnableChanged != null)
                OnLightEnableChanged(this, e);
        }

        private void spinLightValue_ValueChanged(object sender, EventArgs e)
        {
            if (OnLightValueChanged != null)
                OnLightValueChanged(this, e);
        }
    }
}
