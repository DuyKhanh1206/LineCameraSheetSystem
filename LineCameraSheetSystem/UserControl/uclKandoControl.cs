using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Fujita.InspectionSystem;

namespace LineCameraSheetSystem
{
    public partial class uclKandoControl : UserControl
    {
        public string TitleName
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        public bool EnableLight
        {
            get { return spinLight1.Enabled; }
            set
            {
                spinLight1.Enabled = value;
                spinLight2.Enabled = value;
                spinLight3.Enabled = value;
            }
        }
        public bool EnableDark
        {
            get { return spinDark1.Enabled; }
            set
            {
                spinDark1.Enabled = value;
                spinDark2.Enabled = value;
                spinDark3.Enabled = value;
            }
        }

        private Panel[] _lstPanelBacks;
        private uclNumericInputSmall[] _lstSpinValues;

        public uclKandoControl()
        {
            InitializeComponent();
        }

        public void SetMainForm(MainForm _mf)
        {
            _lstPanelBacks = new Panel[] { panelLight1, panelLight2, panelLight3, panelDark1, panelDark2, panelDark3 };
            _lstSpinValues = new uclNumericInputSmall[] { spinLight1, spinLight2, spinLight3, spinDark1, spinDark2, spinDark3 };
            setEvent();
        }

        public void SetBackColor(int index, Color col)
        {
            _lstPanelBacks[index].BackColor = col;
            _lstSpinValues[index].BackColor = col;
        }
        public void SetValue(int index, int val)
        {
            resetEvent();
            _lstSpinValues[index].Value = val;
            setEvent();
        }
        public int GetValue(int index)
        {
            return (int)_lstSpinValues[index].Value;
        }

        private void setEvent()
        {
            for (int i=0; i<_lstSpinValues.Length; i++)
                _lstSpinValues[i].ValueChanged += UclKandoControl_ValueChanged;
        }

        private void resetEvent()
        {
            for (int i = 0; i < _lstSpinValues.Length; i++)
                _lstSpinValues[i].ValueChanged -= UclKandoControl_ValueChanged;
        }

        public delegate void KandoValueEventHandler(object sender, EventArgs e);
        public event KandoValueEventHandler KandoValueChanged;

        private void UclKandoControl_ValueChanged(object sender, EventArgs e)
        {
            if (KandoValueChanged != null)
                KandoValueChanged(this, new EventArgs());
        }
    }
}
