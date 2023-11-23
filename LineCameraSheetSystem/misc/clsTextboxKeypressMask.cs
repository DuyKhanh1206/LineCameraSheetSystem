using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fujita.Misc
{
    interface IKeyPressMask
    {
        bool IsCodeNG(Char code);
    }

    class KeyPressMask_NumericOnly : IKeyPressMask
    {
        public bool IsCodeNG(Char code)
        {
            return !(char.IsNumber(code) || code == '\b');  
        }
    }

    class KeyPressMask_NumericMinusOnly : IKeyPressMask
    {
        public bool IsCodeNG(Char code)
        {
            return !(char.IsNumber(code) || code == '\b' || code == '-' );
        }
    }

    class KeyPressMask_NumericPeriodOnly : IKeyPressMask
    {
        public bool IsCodeNG(Char code)
        {
            return !(char.IsNumber(code) || code == '\b' || code == '.' );
        }
    }

    class KeyPressMask_NumericPeriodMinusOnly : IKeyPressMask
    {
        public bool IsCodeNG(Char code)
        {
            return !(char.IsNumber(code) || code == '\b' || code == '.' || code == '-' );
        }
    }

    class KeyPressMask_InvalidPathChar : IKeyPressMask
    {
        public bool IsCodeNG(Char code)
        {
            return ( code !='\b' && System.IO.Path.GetInvalidPathChars().Contains(code));
        }
    }

    class KeyPressMask_InvalidFileChar : IKeyPressMask
    {
        public bool IsCodeNG(Char code)
        {
            return ( code != '\b' && System.IO.Path.GetInvalidFileNameChars().Contains(code));
        }
    }

    class KeyPressMask_InvalidFileCharUnderBar : IKeyPressMask
    {
        public bool IsCodeNG(Char code)
        {
            return ( code != '\b' &&( System.IO.Path.GetInvalidFileNameChars().Contains(code) || code == '_' ));
        }
    }

    class KeyPressMask_List : IKeyPressMask
    {
        public KeyPressMask_List(List<char> listChar)
        {
            SetCodeList(listChar);
        }

        List<char> _listChar = new List<char>();

        private void SetCodeList(List<char> listchar)
        {

            for (int i = 0; listchar.Count > i; i++)
            {
                _listChar.Add(listchar[i]);

            }
        }

        private bool CheckCodeList(Char code)
        {
            if (_listChar.Count == 0)
            {
                return false;
            }

            int i = -1;
            i = _listChar.FindIndex(x => x == code);
            if (i > -1)
            {
                return true;
            }

            return false;
        }

        public bool IsCodeNG(Char code)
        {
            bool b = CheckCodeList(code);

            return (code != '\b' && b);
        }


    }

    class clsTextboxKeyPressMask : IDisposable
    {
        IKeyPressMask _mask;
        List<TextBox> _lstTextBox = new List<TextBox>();
        List<NumericUpDown> _lstNumUD = new List<NumericUpDown>();

        public clsTextboxKeyPressMask(IKeyPressMask mask)
        {
            if (mask == null)
                throw new ArgumentException("引数がNull");
            _mask = mask;
        }

        public bool SetTextBox( TextBox tb )
        {
            if( _lstTextBox.Contains( tb ))
                return false;

            tb.KeyPress += new KeyPressEventHandler(KeyPress);
            _lstTextBox.Add(tb);
            return true;
        }

        public bool SetNumericUpDown(NumericUpDown nud)
        {
            if (_lstNumUD.Contains(nud))
                return false;
            nud.KeyPress += new KeyPressEventHandler(KeyPress);
            _lstNumUD.Add(nud);
            return true;
        }

        void KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = _mask.IsCodeNG(e.KeyChar);
        }

        void Clear()
        {
            for (int i = 0; i < _lstTextBox.Count; i++)
            {
                _lstTextBox[i].KeyPress -= KeyPress;
            }
            _lstTextBox.Clear();

            for (int i = 0; i < _lstNumUD.Count; i++)
            {
                _lstNumUD[i].KeyPress -= KeyPress;
            }
            _lstNumUD.Clear();
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
