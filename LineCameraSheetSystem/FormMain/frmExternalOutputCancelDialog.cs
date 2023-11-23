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
    public partial class frmExternalOutputCancelDialog : Form
    {
        public frmExternalOutputCancelDialog(int topPos, int leftPos)
        {
            _topPosition = topPos;
            _leftPosition = leftPos;
            InitializeComponent();
        }

        public void  SetText(string title, string msg)
        {
            this.Text = title;
            labelText.Text = msg;
        }
        public void SetBackColor(string color)
        {
            this.BackColor = ColorTranslator.FromHtml(color);
            this.labelText.BackColor = ColorTranslator.FromHtml(color);
        }

        private int _topPosition;
        private int _leftPosition;

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmExternalOutputCancelDialog_Load(object sender, EventArgs e)
        {
            this.Left = this._leftPosition;
            this.Top = this._topPosition;
        }
    }
}
