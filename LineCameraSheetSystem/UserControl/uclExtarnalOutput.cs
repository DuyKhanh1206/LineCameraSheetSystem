using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineCameraSheetSystem
{
    public partial class uclExtarnalOutput : UserControl
    {
        CheckBox[] _chkZone;
        public uclExtarnalOutput()
        {
            InitializeComponent();
            Initialize();
        }

        public void SetZoneSettingEnable(bool value)
        {
            foreach (var obj in this.Controls)
            {
                if (obj is CheckBox)
                {
                    CheckBox cnt = (CheckBox)obj;
                    if (true == cnt.Name.Contains("chkZ"))
                        cnt.Visible = value;
                }
            }
            label1.Visible = value;
        }

        private void Initialize()
        {
            _chkZone = new CheckBox[] {chkZ1,chkZ2, chkZ3, chkZ4, chkZ5, chkZ6, chkZ7, chkZ8 };
        }

        public string CheckButtonTitle
        {
            get { return chkExtTimer.Text; }
            set { chkExtTimer.Text = value; }
        }
        public bool ExtTimer
        {
            get { return chkExtTimer.Checked; }
            set { chkExtTimer.Checked = value; }
        }
        public void SetCheckZone(int index, bool b)
        {
            _chkZone[index].Checked = b;
        }
        public bool GetCheckZone(int index)
        {
            return _chkZone[index].Checked;
        }

        private void chkExtTimer1_CheckedChanged(object sender, EventArgs e)
        {
            int index = int.Parse((string)((CheckBox)sender).Tag);
            chkExtTimerSub.Checked = chkExtTimer.Checked;
            if (chkExtTimer.Checked)
                chkExtTimer.BackColor = Color.GreenYellow;
            else
                chkExtTimer.BackColor = SystemColors.Control;
        }
        private void chkExtTimer1Sub_CheckedChanged(object sender, EventArgs e)
        {
            int index = int.Parse((string)((CheckBox)sender).Tag);
            chkExtTimer.Checked = chkExtTimerSub.Checked;
        }

        private void chkZone_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            if (chk.Checked)
                chk.BackColor = Color.GreenYellow;
            else
                chk.BackColor = SystemColors.Control;
        }
    }
}
