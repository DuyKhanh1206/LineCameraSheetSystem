using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

#if FUJITA_INSPECTION_SYSTEM
using Fujita.InspectionSystem;
#endif

namespace Fujita.FormMisc
{
    public class ChangeControlValueEventArgs
    {
        public Control TargetControl { get; private set; }
        public ChangeControlValueEventArgs(Control ctrl)
        {
            TargetControl = ctrl;
        }
    }

    public delegate void ChangeControlValueEventHandler( object sender, ChangeControlValueEventArgs e );

    public class clsControlChangeChecker
    {
        public event ChangeControlValueEventHandler ChangeControlValue;

        List<Control> _lstControls = new List<Control>();
        bool _bStartMonitor = false;
        bool _bChange = false;

        public void StartMonitor()
        {
            _bStartMonitor = true;
        }

        public void EndMonitor()
        {
            _bStartMonitor = false;
        }

        public void Reset()
        {
            _bChange = false;
        }

        public bool IsChange
        {
            get 
            {
                return _bChange;
            }
        }

        public bool Regist(TextBox tb)
        {
            if (_lstControls.IndexOf(tb) != -1)
            {
                return false;
            }
            _lstControls.Add(tb);
            tb.TextChanged += new EventHandler(tb_TextChanged);
            return true;
        }

        public bool Regist(CheckBox cb)
        {
            if (_lstControls.IndexOf( cb ) != -1 )
            {
                return false;
            }
            _lstControls.Add(cb);
            cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
            return true;
        }

        public bool Regist(RadioButton rb)
        {
            if (_lstControls.IndexOf( rb ) != -1 )
            {
                return false;
            }
            _lstControls.Add(rb);
            rb.CheckedChanged += new EventHandler(rb_CheckedChanged);
            return true;
        }

        public bool Regist(NumericUpDown nud)
        {
            if (_lstControls.IndexOf(nud) != -1)
            {
                return false;
            }
            _lstControls.Add(nud);
            nud.ValueChanged += new EventHandler(nud_ValueChanged);
            return true;
        }

        public bool Regist(ComboBox cb)
        {
            if (_lstControls.IndexOf(cb) != -1)
            {
                return false;
            }
            _lstControls.Add( cb );
            cb.SelectedIndexChanged += new EventHandler(cb_SelectedIndexChanged);
            return true;
        }

        public bool Regist(ListBox lb)
        {
            if (_lstControls.IndexOf(lb) != -1)
            {
                return false;
            }
            _lstControls.Add( lb );
            lb.SelectedIndexChanged += new EventHandler(lb_SelectedIndexChanged);
            return true;
        }

        public bool Regist(MaskedTextBox mtb)
        {
            if (_lstControls.IndexOf( mtb ) != -1 )
            {
                return false;
            }
            _lstControls.Add(mtb);
            mtb.TextChanged += new EventHandler(mtb_TextChanged);
            return true;
        }

        public bool Regist(CheckedListBox clb)
        {
            if (_lstControls.IndexOf( clb ) != -1 )
            {
                return false;
            }
            _lstControls.Add(clb);
            clb.SelectedIndexChanged += new EventHandler(clb_SelectedIndexChanged);
            clb.ItemCheck += new ItemCheckEventHandler(clb_ItemCheck);
            return true;
        }

#if FUJITA_INSPECTION_SYSTEM

        public bool Regist(uclNumericInput uni)
        {
            if (_lstControls.IndexOf(uni) != -1 )
            {
                return false;
            }
            _lstControls.Add(uni);
            uni.ValueChanged += new ValueChangeEventHandler(uni_ValueChanged);
            return true;
        }

        void uni_ValueChanged(object sender, EventArgs e)
        {
            if (_bStartMonitor)
            {
                _bChange = true;
                if (ChangeControlValue != null)
                {
                    ChangeControlValue(this, new ChangeControlValueEventArgs((Control)sender));
                }
            }
        }

#endif


        public bool Regist(ListView lsv, TabControl tc = null)
        {
            if (_lstControls.Exists(x => x.Name == lsv.Name))
            {
                return false;
            }
            _lstControls.Add(lsv);
            if (tc != null)
            {
                tc.Selected += new TabControlEventHandler(tc_Selected);
                tc.SelectedIndexChanged += new EventHandler(tc_SelectedIndexChanged);
            }
            lsv.ItemChecked += new ItemCheckedEventHandler(lsv_ItemChecked);
            return true;
        }

        bool _bDisableLsvItemChecked = false;
        void tc_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 特殊処理
            _bDisableLsvItemChecked = false;
        }

        void tc_Selected(object sender, TabControlEventArgs e)
        {
            // 特殊処理
            _bDisableLsvItemChecked = true;
        }

        void lsv_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (_bStartMonitor && !_bDisableLsvItemChecked)
            {
                _bChange = true;
                if (ChangeControlValue != null)
                {
                    ChangeControlValue(this, new ChangeControlValueEventArgs((Control)sender));
                }
            }
        }

        void clb_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (_bStartMonitor)
            {
                _bChange = true;
                if (ChangeControlValue != null)
                {
                    ChangeControlValue(this, new ChangeControlValueEventArgs((Control)sender));
                }
            }
        }

        void clb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_bStartMonitor)
            {
                _bChange = true;
                if (ChangeControlValue != null)
                {
                    ChangeControlValue(this, new ChangeControlValueEventArgs((Control)sender));
                }
            }
        }

        void mtb_TextChanged(object sender, EventArgs e)
        {
            if (_bStartMonitor)
            {
                _bChange = true;
                if (ChangeControlValue != null)
                {
                    ChangeControlValue(this, new ChangeControlValueEventArgs((Control)sender));
                }
            }
        }

        void lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_bStartMonitor)
            {
                _bChange = true;
                if (ChangeControlValue != null)
                {
                    ChangeControlValue(this, new ChangeControlValueEventArgs((Control)sender));
                }
            }
        }

        void cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_bStartMonitor)
            {
                _bChange = true;
                if (ChangeControlValue != null)
                {
                    ChangeControlValue(this, new ChangeControlValueEventArgs((Control)sender));
                }
            }
        }

        void nud_ValueChanged(object sender, EventArgs e)
        {
            if (_bStartMonitor)
            {
                _bChange = true;
                if (ChangeControlValue != null)
                {
                    ChangeControlValue(this, new ChangeControlValueEventArgs((Control)sender));
                }
            }
        }

        void rb_CheckedChanged(object sender, EventArgs e)
        {
            if (_bStartMonitor)
            {
                _bChange = true;
                if (ChangeControlValue != null)
                {
                    ChangeControlValue(this, new ChangeControlValueEventArgs((Control)sender));
                }
            }
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            if (_bStartMonitor)
            {
                _bChange = true;
                if (ChangeControlValue != null)
                {
                    ChangeControlValue(this, new ChangeControlValueEventArgs((Control)sender));
                }
            }
        }

        private void cb_CheckedChanged(object sender, EventArgs e)
        {
            if (_bStartMonitor)
            {
                _bChange = true;
                if (ChangeControlValue != null)
                {
                    ChangeControlValue(this, new ChangeControlValueEventArgs((Control)sender));
                }
            }
        }
    }
}
