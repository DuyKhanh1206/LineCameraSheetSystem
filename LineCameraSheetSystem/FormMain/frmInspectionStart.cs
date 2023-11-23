using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fujita.Misc;

namespace LineCameraSheetSystem
{
    public partial class frmInspectionStart : Form
    {
        public frmInspectionStart()
        {
            InitializeComponent();
            _stKindame = "";
            _stLotNo = "";

            _keyMask = new clsTextboxKeyPressMask(new KeyPressMask_InvalidFileCharUnderBar());
            _keyMask.SetTextBox(textLotNo);

            textLotNo.Focus();
            AcceptButton = btnOk;

            label2.Visible = SystemParam.GetInstance().LotNoEnable;
            textLotNo.Visible = SystemParam.GetInstance().LotNoEnable;
        }

        private void frmInspectionStart_Load(object sender, EventArgs e)
        {
            textKindName.Text = _stKindame;
            textLotNo.Text = _stLotNo;
        }

        // キー入力制限
        clsTextboxKeyPressMask _keyMask = null;

        public string _stKindame { get; set; }
        public string _stLotNo { get; set; }

        private void btnOk_Click(object sender, EventArgs e)
        {
            _stLotNo = textLotNo.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textLotNo_TextChanged(object sender, EventArgs e)
        {
            if (textLotNo.Text.Length > 20)
            {
                //    Utility.ShowMessage(this, "LotNoは20文字までです。", MessageType.Error);

                textLotNo.Text = textLotNo.Text.Remove(20);
                textLotNo.Select(textLotNo.Text.Length, 0);
            }
        }

    }
}
