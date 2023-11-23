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
    public delegate void RoiRectangle1UserSettingEventHandler(object sender, RoiRectangle1UserSettingEventArgs e);

    public partial class frmRoiRectangle1 : Form, ICallbackRoiRectangle1
    {
        public event RoiRectangle1UserSettingEventHandler UserSettingChange;

        private void setValueChangedEvent()
        {
            uniRow1.ValueChanged += nudValue_ValueChanged;
            uniCol1.ValueChanged += nudValue_ValueChanged;
            uniRow2.ValueChanged += nudValue_ValueChanged;
            uniCol2.ValueChanged += nudValue_ValueChanged;
        }

        private void resetValueChangedEvent()
        {
            uniRow1.ValueChanged -= nudValue_ValueChanged;
            uniCol1.ValueChanged -= nudValue_ValueChanged;
            uniRow2.ValueChanged -= nudValue_ValueChanged;
            uniCol2.ValueChanged -= nudValue_ValueChanged;
        }

        public frmRoiRectangle1( double row1, double col1, double row2, double col2, string message )
        {
            InitializeComponent();

            uniRow1.Value = (decimal)row1;
            uniCol1.Value = (decimal)col1;
            uniRow2.Value = (decimal)row2;
            uniCol2.Value = (decimal)col2;

            setValueChangedEvent();

            lblMessage.Text = message;
        }

        private void frmRoiRectangle1_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 数値に変化があった
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudValue_ValueChanged(object sender, EventArgs e)
        {
//            _access.Rectangle1FormValueChange((double)nudRow1.Value, (double)nudCol1.Value, (double)nudRow2.Value, (double)nudCol2.Value);

            if (UserSettingChange != null)
            {
                UserSettingChange(this, new RoiRectangle1UserSettingEventArgs(UserSettingChangeType.ValueChange,
                    (double)uniRow1.Value, (double)uniCol1.Value, (double)uniRow2.Value, (double)uniCol2.Value));
            }
        }

        private void nudValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!e.KeyChar.IsNumber())
                e.Handled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
//            _access.Rectangle1FormOK((double)nudRow1.Value, (double)nudCol1.Value, (double)nudRow2.Value, (double)nudCol2.Value);
            if (UserSettingChange != null)
            {
                UserSettingChange(this, new RoiRectangle1UserSettingEventArgs(UserSettingChangeType.OK,
                    (double)uniRow1.Value, (double)uniCol1.Value, (double)uniRow2.Value, (double)uniCol2.Value));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
//            _access.Rectangle1FormCancel();
            if (UserSettingChange != null)
            {
                UserSettingChange(this, new RoiRectangle1UserSettingEventArgs(UserSettingChangeType.Cancel,
                    (double)uniRow1.Value, (double)uniCol1.Value, (double)uniRow2.Value, (double)uniCol2.Value));
            }

        }

        public void Rectangle1_Move(double row1, double col1, double row2, double col2, object oUser)
        {
            resetValueChangedEvent();

            uniRow1.Value = (decimal)row1;
            uniCol1.Value = (decimal)col1;
            uniRow2.Value = (decimal)row2;
            uniCol2.Value = (decimal)col2;

            setValueChangedEvent();
        }

        public void Rectangle1_Decide(double row1, double col1, double row2, double col2, object oUser)
        {
            // NOP
        }

        public void Rectangle1_Cancel(object oUser)
        {
            // NOP
        }

    }

    public class RoiRectangle1UserSettingEventArgs : EventArgs
    {
        public double Row1 { get; private set; }
        public double Col1 { get; private set; }
        public double Row2 { get; private set; }
        public double Col2 { get; private set; }

        public UserSettingChangeType Type { get; private set; }

        public RoiRectangle1UserSettingEventArgs(UserSettingChangeType type, double row1, double col1, double row2, double col2)
        {
            Row1 = row1;
            Col1 = col1;
            Row2 = row2;
            Col2 = col2;
            Type = type;
        }
    }
}
