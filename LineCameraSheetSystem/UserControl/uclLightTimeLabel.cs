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
    public partial class uclLightTimeLabel : UserControl
    {
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
        public uclLightTimeLabel()
        {
            InitializeComponent();
        }
    }
}
