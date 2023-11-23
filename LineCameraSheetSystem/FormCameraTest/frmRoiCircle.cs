using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ViewROI;
using Fujita.Misc;

namespace Fujita.InspectionSystem
{
    public delegate void RoiCircleUserSettingEventHandler(object sender, RoiCircleUserSettingEventArgs e);

    public partial class frmRoiCircle : Form, ICallbackRoiCircle
    {
        public event RoiCircleUserSettingEventHandler UserSettingChange;

        private void setValueChangedEvent()
        {
            uniRow.ValueChanged += nudValue_ValueChanged;
            uniCol.ValueChanged += nudValue_ValueChanged;
            uniRad.ValueChanged += nudValue_ValueChanged;          
        }

        private void resetValueChangedEvent()
        {
            uniRow.ValueChanged -= nudValue_ValueChanged;
            uniCol.ValueChanged -= nudValue_ValueChanged;
            uniRad.ValueChanged -= nudValue_ValueChanged;        
        }
        public frmRoiCircle(double row, double col, double rad, string message)
        {
            InitializeComponent();

            uniRow.Value = (decimal)row;
            uniCol.Value = (decimal)col;
            uniRad.Value = (decimal)rad;          

            setValueChangedEvent();

            lblMessage.Text = message;
        }

        private void frmRoiCircle_Load(object sender, EventArgs e)
        {

        }

        private void nudValue_ValueChanged(object sender, EventArgs e)
        {         
            if (UserSettingChange != null)
            {
                UserSettingChange(this, new RoiCircleUserSettingEventArgs(UserSettingChangeType.ValueChange,
                    (double)uniRow.Value, (double)uniCol.Value, (double)uniRad.Value));
            }
        }

        private void nudValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!e.KeyChar.IsNumber())
                e.Handled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (UserSettingChange != null)
            {
                UserSettingChange(this, new RoiCircleUserSettingEventArgs(UserSettingChangeType.OK,
                    (double)uniRow.Value, (double)uniCol.Value, (double)uniRad.Value));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (UserSettingChange != null)
            {
                UserSettingChange(this, new RoiCircleUserSettingEventArgs(UserSettingChangeType.Cancel,
                    (double)uniRow.Value, (double)uniCol.Value, (double)uniRad.Value));
            }
        }

        public void Circle_Move(double row, double col, double rad, object oUser)
        {
            resetValueChangedEvent();

            uniRow.Value = (decimal)row;
            uniCol.Value = (decimal)col;
            uniRad.Value = (decimal)rad;           

            setValueChangedEvent();
        }

        public void Circle_Decide(double row, double col, double rad, object oUser)
        {
            // NOP
        }

        public void Circle_Cancel(object oUser)
        {
            // NOP
        }
    }

    public class RoiCircleUserSettingEventArgs : EventArgs
    {
        public double Row { get; private set; }
        public double Col { get; private set; }
        public double Rad { get; private set; }          

        public UserSettingChangeType Type { get; private set; }

        public RoiCircleUserSettingEventArgs(UserSettingChangeType type, double row, double col, double rad)
        {
            Row = row;
            Col = col;
            Rad = rad;                
            Type = type;
        }
    }
    
}
