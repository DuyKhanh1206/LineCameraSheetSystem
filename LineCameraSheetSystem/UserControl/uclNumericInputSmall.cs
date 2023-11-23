using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fujita.InspectionSystem
{
    public delegate void ValueChangeSEventHandler( object sender, EventArgs e );

    public partial class uclNumericInputSmall : UserControl
    {
        private const int FIRST_INCRIMENT_INTERVAL = 300;
        private const int SECOND_INCRIMENT_INTERVAL = 150;

        public event ValueChangeSEventHandler ValueChanged = null;

        public HorizontalAlignment TextAlign
        {
            get
            {
                return txtNumeric.TextAlign;
            }
            set
            {
                txtNumeric.TextAlign = value;
            }
        }

        private decimal _deMaximum = 100;
        public decimal Maximum
        {
            get { return _deMaximum; }
            set
            {
                if (_deMinimum > value)
                {
                    _deMaximum = _deMinimum;
                }
                else
                {
                    _deMaximum = value;
                }
            }
        }

        private decimal _deMinimum = 0;
        public decimal Minimum
        {
            get { return _deMinimum; }
            set
            {
                if (_deMaximum < value)
                {
                    _deMinimum = _deMaximum;
                }
                else
                {
                    _deMinimum = value;
                }
                updateValue();
            }
        }

        private decimal _deValue = 0;
        public decimal Value 
        {
            get { return _deValue; }
            set
            {
                updateValue( value );
            }
        }

        private int _iDecimalPlace = 0;
        public int DecimalPlaces
        {
            get { return _iDecimalPlace; }
            set
            {
                if (value < 0 || value > 99)
                    new ArgumentException( value.ToString() + "の値は有効ではありません。0-99の間です");
                _iDecimalPlace = value;
                updateValue();
            }
        }

        private decimal _deIncriment = 1;
        public decimal Incriment
        {
            get { return _deIncriment; }
            set
            {
                if (value < 0)
                    new ArgumentOutOfRangeException(value.ToString() + "の値は有効ではありません");
                _deIncriment = value;
            }
        }

        public bool EveryValueChanged
        {
            get;
            set;
        }

        private void updateValue()
        {
            updateValue(_deValue);
        }

        bool _bOnLoadValueChange = false;
        private void updateValue(decimal value)
        {
            if (_deMinimum > value)
                _deValue = _deMinimum;
            else if (_deMaximum < value)
                _deValue = _deMaximum;
            else
                _deValue = value;

            if (EveryValueChanged)
            {
                if (!_bOnLoadValueChange && ValueChanged != null )
                    ValueChanged(this, new EventArgs());
            }
            else
            {
                if (!_bOnLoadValueChange && ValueChanged != null && !_bMouseDowned)
                    ValueChanged(this, new EventArgs());
            }

            updateText();

        }

        private string getValueString()
        {
            if (_iDecimalPlace == 0)
            {
                return ((long)_deValue).ToString();
            }
            else
            {
                return _deValue.ToString("F" + _iDecimalPlace.ToString());
            }
        }

        private void updateText()
        {
            txtNumeric.Text = getValueString();
        }

		public uclNumericInputSmall()
        {
            InitializeComponent();

            txtNumeric.ContextMenu = new System.Windows.Forms.ContextMenu();

            btnUp.MouseDown += new MouseEventHandler(btnIncriment_MouseDown);
            btnDown.MouseDown += new MouseEventHandler(btnIncriment_MouseDown);

            btnUp.MouseUp += new MouseEventHandler(btnIncriment_MouseUp);
            btnDown.MouseUp += new MouseEventHandler(btnIncriment_MouseUp);

            txtNumeric.KeyPress += new KeyPressEventHandler(txtNumeric_KeyPress);
            txtNumeric.KeyDown += new KeyEventHandler(txtNumeric_KeyDown);
        }

        void txtNumeric_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && txtNumeric.Text != "" )
            {
                updateValue( decimal.Parse( txtNumeric.Text));
            }
        }

        private void txtNumeric_TextChanged(object sender, EventArgs e)
        {
  //         updateText();
        }

        private void txtNumeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '\b')
                e.Handled = true;

            TextBox txt = (TextBox)sender;
            if (txt.Text.IndexOf('.') != -1)
                e.Handled = true;
        }

        private decimal _deNowInc = 0;
        private void btnIncriment_KeyDown(object sender, KeyEventArgs e)
        {
            tmrIncriment.Interval = FIRST_INCRIMENT_INTERVAL;
            tmrIncriment.Enabled = true;
            tmrKeydown.Enabled = true;

            if (btnUp == (Button)sender)
            {
                _deNowInc = _deIncriment;
            }
            else
            {
                _deNowInc = -_deIncriment;
            }
            Value = Value + _deNowInc;
        }

        private void tmrIncriment_Tick(object sender, EventArgs e)
        {
            Value = Value + _deNowInc;
        }

        private void tmrKeydown_Tick(object sender, EventArgs e)
        {
            tmrKeydown.Enabled = false;
            tmrIncriment.Enabled = false;
            tmrIncriment.Interval = SECOND_INCRIMENT_INTERVAL;
            tmrIncriment.Enabled = true;
        }

        private void btnIncriment_KeyUp(object sender, KeyEventArgs e)
        {
            tmrKeydown.Enabled = false;
            tmrIncriment.Enabled = false;
        }

        private void txtNumeric_Validated(object sender, EventArgs e)
        {
            decimal decVal;
            if (!decimal.TryParse(txtNumeric.Text, out decVal ))
                decVal = _deMinimum;
            updateValue(decVal);
        }

		private void uclNumericInputSmall_Load(object sender, EventArgs e)
        {
            _bOnLoadValueChange = true;
            updateValue();
            _bOnLoadValueChange = false;
        }

        bool _bMouseDowned = false;
        private void btnIncriment_MouseDown(object sender, MouseEventArgs e)
        {
            tmrIncriment.Interval = FIRST_INCRIMENT_INTERVAL;
            tmrIncriment.Enabled = true;
            tmrKeydown.Enabled = true;

            if (btnUp == (Button)sender)
            {
                _deNowInc = _deIncriment;
            }
            else
            {
                _deNowInc = -_deIncriment;
            }
            Value = Value + _deNowInc;
            _bMouseDowned = true;
        }

        private void btnIncriment_MouseUp(object sender, MouseEventArgs e)
        {
            tmrKeydown.Enabled = false;
            tmrIncriment.Enabled = false;
            _bMouseDowned = false;

            Value = Value;
        }

        private void txtNumeric_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void txtNumeric_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left )
            {
                Color colOld = txtNumeric.BackColor;
                txtNumeric.BackColor = Color.Pink;

                frmTenkey tenkey = new frmTenkey();
                tenkey.SetValues(this);
                if (DialogResult.OK == tenkey.ShowDialog())
                {
                    updateValue(tenkey.Value);
                }

                txtNumeric.BackColor = colOld;
            }
        }
    }
}
