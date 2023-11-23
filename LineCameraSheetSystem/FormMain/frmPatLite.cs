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
    public partial class frmPatLite : Form
    {
        public bool PatLiteEnable
        {
            get { return chkPatLiteEnable.Checked; }
            set { chkPatLiteEnable.Checked = value; }
        }
        public int PatLiteDelay
        {
            get { return (int)spinPatLiteDelay.Value; }
            set { spinPatLiteDelay.Value = value; }
        }
        public int PatLiteOnTime
        {
            get { return (int)spinPatLiteOnTime.Value; }
            set { spinPatLiteOnTime.Value = value; }
        }

        public frmPatLite()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void lblMaskUse_MouseDown(object sender, MouseEventArgs e)
        {
            //Ctrl+Shft+右クリック
            if ((e.Button == MouseButtons.Right) & ((Control.ModifierKeys & Keys.Control) == Keys.Control) & ((Control.ModifierKeys & Keys.Shift) == Keys.Shift))
            {
                //ボタン有効無効を反転
                lblMaskUse.Visible = !lblMaskUse.Visible;
            }
        }
    }
}
