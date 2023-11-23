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
    public partial class uclLightTimeReset : UserControl
    {
        public delegate void LightTimeResetEventHandler(object sender, EventArgs e);
        public event LightTimeResetEventHandler OnLightTimeReset;

        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }
        public string DisplayValue
        {
            get { return lblDisplayValue.Text; }
            set { lblDisplayValue.Text = value; }
        }
        public Color DisplayValueBackColor
        {
            get { return lblDisplayValue.BackColor; }
            set { lblDisplayValue.BackColor = value; }
        }

        public uclLightTimeReset()
        {
            InitializeComponent();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (OnLightTimeReset != null)
                OnLightTimeReset(this, e);
        }
    }
}
