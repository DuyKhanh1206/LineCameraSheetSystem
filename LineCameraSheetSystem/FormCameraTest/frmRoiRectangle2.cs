using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ViewROI;

namespace Fujita.InspectionSystem
{
    public delegate void RoiRectangle2UserSettingEventHandler(object sender, RoiRectangle2UserSettingEventArgs e);

    public partial class frmRoiRectangle2 : System.Windows.Forms.Form, ICallbackRoiRectangle2
    {
        public event RoiRectangle2UserSettingEventHandler UserSettingChanged;

        private void setValueChangedEvent()
        {
            nudRow.ValueChanged += nudValue_ValueChanged;
            nudCol.ValueChanged += nudValue_ValueChanged;
            nudPhi.ValueChanged += nudValue_ValueChanged;
            nudLen1.ValueChanged += nudValue_ValueChanged;
            nudLen2.ValueChanged += nudValue_ValueChanged;
        }

        private void resetValueChangedEvent()
        {
            nudRow.ValueChanged -= nudValue_ValueChanged;
            nudCol.ValueChanged -= nudValue_ValueChanged;
            nudPhi.ValueChanged -= nudValue_ValueChanged;
            nudLen1.ValueChanged -= nudValue_ValueChanged;
            nudLen2.ValueChanged -= nudValue_ValueChanged;
        }

        public frmRoiRectangle2(double row, double col, double phi, double len1, double len2, string message )
        {
            InitializeComponent();

            lblMessage.Text = message;

            nudRow.Value = (decimal)row;
            nudCol.Value = (decimal)col;
            nudPhi.Value = (decimal)phi;
            nudLen1.Value = (decimal)len1;
            nudLen2.Value = (decimal)len2;

            setValueChangedEvent();
        }

        private void frmRoiRectangle2_Load(object sender, EventArgs e)
        {

        }
        private void nudValue_ValueChanged(object sender, EventArgs e)
        {
            if (UserSettingChanged != null)
            {
                UserSettingChanged( this, 
                    new RoiRectangle2UserSettingEventArgs( UserSettingChangeType.ValueChange, (double)nudRow.Value, (double)nudCol.Value, (double)nudPhi.Value, (double)nudLen1.Value, (double)nudLen2.Value));
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (UserSettingChanged != null)
            {
                UserSettingChanged(this,
                    new RoiRectangle2UserSettingEventArgs(UserSettingChangeType.OK, (double)nudRow.Value, (double)nudCol.Value, (double)nudPhi.Value, (double)nudLen1.Value, (double)nudLen2.Value));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (UserSettingChanged != null)
            {
                UserSettingChanged(this,
                    new RoiRectangle2UserSettingEventArgs(UserSettingChangeType.Cancel, (double)nudRow.Value, (double)nudCol.Value, (double)nudPhi.Value, (double)nudLen1.Value, (double)nudLen2.Value));
            }
        }

        public void Rectangle2_Move(double row, double col, double phi, double len1, double len2, object oUser)
        {
            resetValueChangedEvent();

            nudRow.Value = (decimal)row;
            nudCol.Value = (decimal)col;
            nudPhi.Value = (decimal)phi;
            nudLen1.Value = (decimal)len1;
            nudLen2.Value = (decimal)len2;

            setValueChangedEvent();
        }

        public void Rectangle2_Decide(double row, double col, double phi, double len1, double len2, object oUser)
        {
            // nop
        }

        public void Rectangle2_Cancel(object oUser)
        {
            // nop
        }
    }

    public class RoiRectangle2UserSettingEventArgs : EventArgs
    {
        public double Row { get; private set; }
        public double Col { get; private set; }
        public double Phi { get; private set; }
        public double Len1 { get; private set; }
        public double Len2 { get; private set; }

        public UserSettingChangeType Type { get; private set; }

        public RoiRectangle2UserSettingEventArgs(UserSettingChangeType type, double row, double col, double phi, double len1, double len2)
        {
            Row = row;
            Col = col;
            Phi = phi;
            Len1 = len1;
            Len2 = len2;

            Type = type;
        }
    }
}
