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
    public partial class frmTenkey : System.Windows.Forms.Form
    {
        decimal _decMinValue = 0;
        decimal _decMaxValue = 0;
        decimal _decPrevValue = 0;
        int _iDecimalPlaces = 0;

        decimal _decValue = 0;

        public decimal MinValue
        {
            get { return _decMinValue; }
            set
            {
                _decMinValue = value;
            }
        }

        public decimal MaxValue
        {
            get { return _decMaxValue; }
            set
            {
                _decMaxValue = value;
            }
        }

        public decimal PrevValue
        {
            get { return _decPrevValue; }
            set
            {
                _decPrevValue = value;
            }
        }

        string _sFormatString = "F0";

        public int DecimalPlaces
        {
            get { return _iDecimalPlaces; }
            set
            {
                if (_iDecimalPlaces < 0)
                    return;
                _iDecimalPlaces = value;
                _sFormatString = "F" + _iDecimalPlaces.ToString();
            }
        }

        public decimal Value
        {
            get { return _decValue; }
        }

        public void SetValues(decimal min, decimal max, decimal val, int decimalPlaces)
        {
            _decMinValue = min;
            _decMaxValue = max;
            _decPrevValue = val;
            DecimalPlaces = decimalPlaces;
        }
        public void SetValues( uclNumericInput input )
        {
            _decMinValue = input.Minimum;
            _decMaxValue = input.Maximum;
            _decPrevValue = input.Value;
            DecimalPlaces = input.DecimalPlaces;
        }

		public void SetValues(uclNumericInputSmall input)
		{
			_decMinValue = input.Minimum;
			_decMaxValue = input.Maximum;
			_decPrevValue = input.Value;
			DecimalPlaces = input.DecimalPlaces;
		}
	
		public void SetValues(NumericUpDown input)
        {
            _decMinValue = input.Minimum;
            _decMaxValue = input.Maximum;
            _decPrevValue = input.Value;
            _iDecimalPlaces = input.DecimalPlaces;
        }

        Button[] btnNums = null; 

        public frmTenkey()
        {
            InitializeComponent();

            btnNums = new Button[] { btnNum0, btnNum1, btnNum2, btnNum3, btnNum4, btnNum5, btnNum6, btnNum7, btnNum8, btnNum9 };

            for( int i = 0 ; i < btnNums.Length; i++ )
                btnNums[i].Click += new EventHandler(btnNum_Click);

            btnPeriod.Click += new EventHandler(btnNum_Click);
            btnMinus.Click += new EventHandler(btnNum_Click);
        }

        void btnNum_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string sTest = txtValue.Text;

            if (btn == btnPeriod && txtValue.Text == "")
            {
                sTest = "0";
            }

            //int iPeriodIndex = -1;
            //if ((iPeriodIndex = sTest.IndexOf('.')) != -1)
            //{
            //    if (sTest.Substring(iPeriodIndex + 1).Length >= _iDecimalPlaces)
            //    {
            //        updateControls();
            //        return;
            //    }
            //}           

            if (btn == btnMinus)
            {
                if (txtValue.Text != "")
                {
                    int iMinusIndex = -1;
                    if ((iMinusIndex = sTest.IndexOf('-')) == -1)
                    {
                        sTest = btn.Text + sTest;
                    }
                    else
                    {
                        
                        sTest = sTest.Trim('-');
                    }
                }
                else
                {
                    sTest += '-';
                }
            }
            else
            {
                int iPeriodIndex = -1;
                if ((iPeriodIndex = sTest.IndexOf('.')) != -1)
                {
                    if (sTest.Substring(iPeriodIndex + 1).Length >= _iDecimalPlaces)
                    {
                        updateControls();
                        return;
                    }
                }

                sTest += btn.Text;
            }


            decimal decValue;
            if (decimal.TryParse(sTest, out decValue))
            {
//                txtValue.Text = decValue.ToString(_sFormatString);
                txtValue.Text = sTest;
            }

            if (sTest == "-"||sTest == "")
            {
                txtValue.Text = sTest;
            }         

            updateControls();
        }

        private void frmTenkey_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtValue;

            lblMinValue.Text = _decMinValue.ToString(_sFormatString);
            lblMaxValue.Text = _decMaxValue.ToString(_sFormatString);
            lblPrevValue.Text = _decPrevValue.ToString(_sFormatString);

            clsControlSerialize.Restore(this);

            updateControls();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _decValue = decimal.Parse(txtValue.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtValue.Text = "";
            updateControls();
        }

        void updateControls()
        {
            btnOK.Enabled = true;
            btnPeriod.Enabled = true;
            btnBackSpace.Enabled = true;
            btnClear.Enabled = true;

            for (int i = 0; i < btnNums.Length; i++)
                btnNums[i].Enabled = true;

            if (txtValue.Text == "")
            {
                btnOK.Enabled = false;
                btnClear.Enabled = false;
                btnBackSpace.Enabled = false;
            }
            else
            {
                decimal decValue;
                if (!decimal.TryParse(txtValue.Text, out decValue))
                {
                    btnOK.Enabled = false;
                }
                else
                {
                    if (decValue < _decMinValue || decValue > _decMaxValue)
                    {
                        btnOK.Enabled = false;
                    }
                }

                int iPeriodIndex = -1;
                if (( iPeriodIndex = txtValue.Text.IndexOf('.')) != -1)
                {
                    btnPeriod.Enabled = false;                  

                    if (txtValue.Text.Substring(iPeriodIndex + 1).Length >= _iDecimalPlaces)
                    {                        
                        for (int i = 0; i < btnNums.Length; i++)
                            btnNums[i].Enabled = false;                        
                    }
                }

            }

            if (_iDecimalPlaces == 0)
            {
                btnPeriod.Enabled = false;
            }

            txtValue.Focus();
        }

        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            string sText = txtValue.Text;

            if (sText.Length > 0)
            {
                txtValue.Text = sText.Substring(0, sText.Length - 1);
            }
            updateControls();
        }

        private void frmTenkey_FormClosing(object sender, FormClosingEventArgs e)
        {
            clsControlSerialize.Store(this, clsControlSerialize.ESerializeType.Position, true);
//            Properties.Settings.Default.Save();
        }

        private void txtValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && btnClear.Enabled == true)
            {
                btnClear.PerformClick();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter && btnOK.Enabled == true)
            {
                btnOK.PerformClick();
                e.Handled = true;
            }
        }

        private void txtValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ('0' <=e.KeyChar && e.KeyChar <= '9')
            {
                int num = e.KeyChar - '0';
                if (btnNums[num].Enabled == true)
                    btnNums[num].PerformClick();
            }
            else if (e.KeyChar == '.' && btnPeriod.Enabled == true)
            {
                btnPeriod.PerformClick();
            }
            else if (e.KeyChar == '-' && btnMinus.Enabled == true)
            {
                btnMinus.PerformClick();
            }
            //Cancel
            e.Handled = true;
        }
    }
}
