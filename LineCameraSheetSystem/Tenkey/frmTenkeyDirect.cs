using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Fujita.FormMisc;

namespace Fujita.InspectionSystem
{
    public partial class frmTenkeyDirect : Form
    {
        public frmTenkeyDirect()
        {
            InitializeComponent();
        }

        TextBox _txtTarget;
        Button[] btnNums = null;

        string _sOldText = "";

        public frmTenkeyDirect(TextBox tb)
        {
            InitializeComponent();
            _txtTarget = tb;
            _sOldText = tb.Text;

            btnNums = new Button[] { btnNum0, btnNum1, btnNum2, btnNum3, btnNum4, btnNum5, btnNum6, btnNum7, btnNum8, btnNum9 };

            for (int i = 0; i < btnNums.Length; i++)
                btnNums[i].Click += new EventHandler(btnNum_Click);

            btnPeriod.Click += new EventHandler(btnNum_Click);

        }

        void btnNum_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            _txtTarget.Text += btn.Text;
        }

        /// <summary>
        /// ピリオドを許すかどうか
        /// </summary>
        public bool Period { get; set; }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _txtTarget.Text = "";
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            if (_txtTarget.Text.Length > 0)
            {
                _txtTarget.Text = _txtTarget.Text.Substring(0, _txtTarget.Text.Length - 1);
            }
        }

        void updateControls()
        {
            if (!Period)
            {
                btnPeriod.Enabled = false;
            }

            if (_txtTarget.Text.Length == 0)
            {
                btnClear.Enabled = false;
                btnBackSpace.Enabled = false;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _txtTarget.Text = _sOldText;
        }

        private void frmTenkeyDirect_Load(object sender, EventArgs e)
        {
            clsControlSerialize.Restore(this);

        }

        private void frmTenkeyDirect_FormClosing(object sender, FormClosingEventArgs e)
        {
            clsControlSerialize.Store(this);
        }
    }
}
