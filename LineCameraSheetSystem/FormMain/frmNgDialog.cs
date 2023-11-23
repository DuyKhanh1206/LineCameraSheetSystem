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
    public partial class frmNgDialog : Form
    {
        public frmNgDialog(int topPos)
        {
            _topPosition = topPos;
            InitializeComponent();
        }

        public void  SetText(string title, string msg, Color back, Color foreText)
        {
            this.Text = title;
            labelText.Text = msg;
            this.labelText.BackColor = back;
            this.labelText.ForeColor = foreText;
        }
        public void SetBackColor(string color)
        {
            this.BackColor = ColorTranslator.FromHtml(color);
        }

        private int _topPosition;

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmNgDialog_Load(object sender, EventArgs e)
        {
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = this._topPosition;
        }
    }
}
