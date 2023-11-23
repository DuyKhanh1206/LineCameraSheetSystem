using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using Fujita.Misc;
namespace Fujita.InspectionSystem
{

    public class FocusChangedEventArgs
    {
        public bool GotFocus { get; private set; }

        public FocusChangedEventArgs(bool bGotFocus)
        {
            GotFocus = bGotFocus;
        }
    }

    public delegate void FocusChagnedEventHandler( object sender, FocusChangedEventArgs e );

    public class clsNUDTenkeyer
    {
        List<NumericUpDown> _lstNumerics = new List<NumericUpDown>();
        TenkeyController _tenkey;

        public bool TenkeyOn { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public bool TenkeyOnNUD { get; set; }

        public int Width
        {
            get { return _tenkey.Width; }
        }

        public int Height
        {
            get { return _tenkey.Height; }
        }

        public clsNUDTenkeyer(Control.ControlCollection ctrls = null)
        {
            _tenkey = new TenkeyController(Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\') + 1));
            if( ctrls != null )
                getNUD(ctrls, _lstNumerics);
        }

        public event FocusChagnedEventHandler FocusChanged;

        private int getNUD(Control.ControlCollection cc, List<NumericUpDown> lsts)
        {
            foreach (Control c in cc)
            {
                NumericUpDown nud = c as NumericUpDown;
                if (nud != null)
                {
                    _lstNumerics.Add(nud);
                    nud.Enter += new EventHandler(nud_Enter);
                    nud.Leave += new EventHandler(nud_Leave);
                }


                if (c.Controls.Count > 0 )
                {
                    getNUD(c.Controls, lsts);
                }
            }
            return lsts.Count;
        }

        void nud_Leave(object sender, EventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;
            if (TenkeyOn)
            {
                _tenkey.Close();
            }
            if (FocusChanged != null)
            {
                FocusChanged(sender, new FocusChangedEventArgs(false));
            }
        }

        void nud_Enter(object sender, EventArgs e)
        {
            NumericUpDown nud = sender as NumericUpDown;
            if (TenkeyOn)
            {
                int iXPos = PositionX;
                int iYPos = PositionY;
                if (TenkeyOnNUD)
                {

                    Point pt = nud.Parent.PointToScreen(nud.Location);
                    Size sz = nud.Size;
                    Rectangle display = Screen.PrimaryScreen.Bounds;

                    if (pt.X + _tenkey.Width > display.Width)
                    {
                        iXPos = display.Width - _tenkey.Width;
                    }
                    if (pt.Y + _tenkey.Height > display.Height)
                    {
                        iYPos = pt.Y - _tenkey.Height - 1;
                    }
                }
                _tenkey.Start(iXPos, iYPos);
            }

            if (FocusChanged != null)
            {
                FocusChanged(sender, new FocusChangedEventArgs(true));
            }
        }
    }
}
