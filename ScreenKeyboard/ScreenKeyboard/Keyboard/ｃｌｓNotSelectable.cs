using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Fujita.ScreenKeyboard;

namespace Fujita.ScreenKeyboard
{
    class clsNotSelectableButton : System.Windows.Forms.Button
    {
        public clsNotSelectableButton()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }

    class clsNotSelectableCheckBox : System.Windows.Forms.CheckBox
    {
        public clsNotSelectableCheckBox()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }

    class clsNotSelectableUclKeyJpnAiu : uclKeyJpnAiu
    {
        public clsNotSelectableUclKeyJpnAiu()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }

    class clsNotSelectableUclKeyEngQwe : uclKeyEngQwe
    {
        public clsNotSelectableUclKeyEngQwe()
        {
            this.SetStyle(ControlStyles.Selectable, false);
        }
    }

}
