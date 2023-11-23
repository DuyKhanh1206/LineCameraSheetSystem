using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LineCameraSheetSystem
{
    public partial class frmSystemErrorDialog : Form
    {
        public frmSystemErrorDialog()
        {
            InitializeComponent();
            labelText.Text = "システムエラーが発生しました。\r\n再起動してください。";
        }

        public string _stError 
        {
            get
            {
                return labelError.Text;
            }
            set
            {
                labelError.Text = value;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmSystemErrorDialog_VisibleChanged(object sender, EventArgs e)
        {
        }
    }
}
