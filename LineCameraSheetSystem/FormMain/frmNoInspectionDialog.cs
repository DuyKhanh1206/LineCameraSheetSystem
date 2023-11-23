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
    public partial class frmNoInspectionDialog : Form
    {
        public frmNoInspectionDialog(int topPos)
        {
            _topPosition = topPos;
            InitializeComponent();
        }
        
        public  void SetText(string msg)
        {
            labelText.Text = msg;
        }
        public void SetBackColor(string color)
        {
            this.BackColor = (color != "") ? ColorTranslator.FromHtml(color) : SystemColors.Control;
            this.labelText.BackColor = (color != "") ? ColorTranslator.FromHtml(color) : SystemColors.Control;
        }

        public void SetTittle(string title)
        {
            Text = title;
        }

        private int _topPosition;

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmNoInspectionDialog_Load(object sender, EventArgs e)
        {
            this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2;
            this.Top = this._topPosition;
        }
    }
}
