using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fujita.LightControl;

namespace Adjustment
{
    public partial class uclMaintenanceLightControl : UserControl
    {
        LightType _light;

        public uclMaintenanceLightControl()
        {
            InitializeComponent();
            initControls();
            updateControls();
        }
        
        public string Value
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

        public string Offset
        {
            get
            {
                return textOffset.Text;
            }

            set
            {
                textOffset.Text = value;
            }
        }

        public void SetLight(LightType light)
        {
            _light = light;

            initControls();
            updateControls();
        }


        private void initControls()
        {
            if (_light != null)
            {
                labelTitle.Text = _light.Name;
            }
        }

        private void updateControls()
        {

            foreach (Control c in Controls)
            {
                if (_light != null && (c as Label) != null)
                    c.Enabled = true;
            }

        }
        /// <summary>
        /// 照明値（現在値）を基準照明値にコピー
        /// </summary>
        public void CopyStdLightValue(string stdLightValue)
        {
            textStdLightValue.Text = stdLightValue;
        }

        public void LightOffsetUpdate(string Value)
        {

            textNowLightValue.Text = Value;
            if(textStdLightValue.Text!="")
            {
                int a = int.Parse(Value) - int.Parse(textStdLightValue.Text);
                textOffset.Text = a.ToString();
            }
        }
    }
}
