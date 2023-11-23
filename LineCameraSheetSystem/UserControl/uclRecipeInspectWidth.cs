using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineCameraSheetSystem
{
    public partial class uclRecipeInspectWidth : UserControl
    {
        public delegate void SheetWidthEventHandler(object sender, EventArgs args);
        public event SheetWidthEventHandler OnSheetWidthValueChanged;
        public delegate void MaskWidthEventHandler(object sender, EventArgs args);
        public event MaskWidthEventHandler OnMaskWidthValueChanged;
        public delegate void MaskShiftEventHandler(object sender, EventArgs args);
        public event MaskShiftEventHandler OnMaskShiftValueChanged;
        public delegate void SetCommonValueEventHandler(object sender, EventArgs args);
        public event SetCommonValueEventHandler OnSetCommonValue;

        public double SheetWidth
        {
            get { return (int)spinWidth.Value; }
            set
            {
                if (_setFlag == true)
                    return;
                _setFlag = true;
                spinWidth.Value = (decimal)value;
                _setFlag = false;
            }
        }
        public double MaskWidth
        {
            get { return (int)spinMaskWidth.Value; }
            set
            {
                if (_setFlag == true)
                    return;
                _setFlag = true;
                spinMaskWidth.Value = (decimal)value;
                _setFlag = false;
            }
        }
        public double MaskShift
        {
            get { return (int)spinMaskShift.Value; }
            set
            {
                if (_setFlag == true)
                    return;
                _setFlag = true;
                spinMaskShift.Value = (decimal)value;
                _setFlag = false;
            }
        }

        public uclRecipeInspectWidth()
        {
            InitializeComponent();
        }

        private void spinWidth_ValueChanged(object sender, EventArgs e)
        {
            if (_setFlag == true)
                return;
            _setFlag = true;
            if (OnSheetWidthValueChanged != null)
                OnSheetWidthValueChanged(this, e);
            _setFlag = false;
        }

        private void spinMaskWidth_ValueChanged(object sender, EventArgs e)
        {
            if (_setFlag == true)
                return;
            _setFlag = true;
            if (OnMaskWidthValueChanged != null)
                OnMaskWidthValueChanged(this, e);
            _setFlag = false;
        }

        private void spinMaskShift_ValueChanged(object sender, EventArgs e)
        {
            if (_setFlag == true)
                return;
            _setFlag = true;
            if (OnMaskShiftValueChanged != null)
                OnMaskShiftValueChanged(this, e);
            _setFlag = false;
        }


        public double CommonSheetWidth
        {
            get
            {
                return (int)double.Parse(lblCmnInspWidth.Text);
            }
            set
            {
                lblCmnInspWidth.Text = value.ToString("F0");
            }
        }
        public double CommonMaskWidth
        {
            get
            {
                return (int)double.Parse(lblCmnMaskWidth.Text);
            }
            set
            {
                lblCmnMaskWidth.Text = value.ToString("F0");
            }
        }
        public double CommonMaskShift
        {
            get
            {
                return (int)double.Parse(lblCmnMaskShift.Text);
            }
            set
            {
                lblCmnMaskShift.Text = value.ToString("F0");
            }
        }
        public bool CommonInspAreaEnable
        {
            get { return chkInspAreaCommonMode1.Checked; }
            set
            {
                _setFlag = true;
                chkInspAreaCommonMode1.Checked = value;
                chkInspAreaCommonMode2.Checked = !value;
                _setFlag = false;
            }
        }

        public delegate void CommonInspAreatEventHandler(object sender, EventArgs args);
        public event CommonInspAreatEventHandler OnCommonInspArea;

        private bool _setFlag = false;
        private void chkInspAreaCommonMode1_CheckedChanged(object sender, EventArgs e)
        {
            if (_setFlag == true)
                return;

            _setFlag = true;
            if (chkInspAreaCommonMode1.Checked == true)
            {
                chkInspAreaCommonMode2.Checked = false;
                if (OnCommonInspArea != null)
                    OnCommonInspArea(this, e);
            }
            else
            {
                chkInspAreaCommonMode1.Checked = true;
            }
            _setFlag = false;
        }

        private void chkInspAreaCommonMode2_CheckedChanged(object sender, EventArgs e)
        {
            if (_setFlag == true)
                return;

            _setFlag = true;
            if (chkInspAreaCommonMode2.Checked == true)
            {
                chkInspAreaCommonMode1.Checked = false;
                if (OnCommonInspArea != null)
                    OnCommonInspArea(this, e);
            }
            else
            {
                chkInspAreaCommonMode2.Checked = true;
            }
            _setFlag = false;
        }

        private void btnSetCommon_Click(object sender, EventArgs e)
        {
            if (OnSetCommonValue != null)
                OnSetCommonValue(this, e);
        }
    }
}
