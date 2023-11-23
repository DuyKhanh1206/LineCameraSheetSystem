using Fujita.InspectionSystem;
using HalconCamera;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LineCameraSheetSystem
{
    public partial class frmRecipeSpeed : Form
    {
        public bool CamSpeedEnable
        {
            get { return chkSpeedEnable.Checked; }
            set { chkSpeedEnable.Checked = value; }
        }

        public double CamSpeedValue
        {
            get { return (double)spinCamSpeed.Value; }
            set
            {
                decimal val = (decimal)value;
                if ((decimal)val > spinCamSpeed.Maximum)
                    val = spinCamSpeed.Maximum;
                spinCamSpeed.Value = val;
            }
        }

        public double CamSpeedValueUra
        {
            get { return (double)spinCamSpeedUra.Value; }
            set
            {
                decimal val = (decimal)value;
                if ((decimal)val > spinCamSpeedUra.Maximum)
                    val = spinCamSpeedUra.Maximum;
                spinCamSpeedUra.Value = val;
            }
        }

        public bool CamExposureEnable
        {
            get { return chkExposureEnable.Checked; }
            set { chkExposureEnable.Checked = value; }
        }
        public double CamExposureValue
        {
            get { return (double)spinCamExposure.Value; }
            set { spinCamExposure.Value = (decimal)value; }
        }
        public double CamExposureValueUra
        {
            get { return (double)spinCamExposureUra.Value; }
            set { spinCamExposureUra.Value = (decimal)value; }
        }

        private uclNumericInputSmall[] _spinCamSpeed;
        private Label[] _lblCamHz;
        private Label[] _lblCamExp;
        private uclNumericInputSmall[] _spinExposureTime;

        public frmRecipeSpeed()
        {
            InitializeComponent();
        }

        private void spinCamSpeed_Load(object sender, EventArgs e)
        {
            _spinCamSpeed = new uclNumericInputSmall[] { spinCamSpeed, spinCamSpeedUra };
            _lblCamHz = new Label[] { lblCamHz, lblCamHzUra };
            _lblCamExp = new Label[] { lblCamExp, lblCamExpUra };
            _spinExposureTime = new uclNumericInputSmall[] { spinCamExposure, spinCamExposureUra };

            SystemParam systemparam = SystemParam.GetInstance();

            //速度
            SetSpeedRange();
            for (int i = 0; i < _spinCamSpeed.Length; i++)
            {
                double val = (double)_spinCamSpeed[i].Value;

                if ((decimal)val < _spinCamSpeed[i].Minimum || _spinCamSpeed[i].Maximum < (decimal)val)
                    _spinCamSpeed[i].Value = (decimal)systemparam.Hz2Speed(CameraManager.getInstance().GetCamera(0).LineRate);
                else
                    _spinCamSpeed[i].Value = (decimal)val;
                systemparam.DispHz(systemparam.Speed2Hz((double)_spinCamSpeed[i].Value), _lblCamHz[i], _lblCamExp[i]);
            }

            //露光値
            SetExposureRange();
            for (int i = 0; i < _spinExposureTime.Length; i++)
            {
                int val = (int)_spinExposureTime[i].Value;
                if (val < _spinExposureTime[i].Minimum || _spinExposureTime[i].Maximum < val)
                    _spinExposureTime[i].Value = CameraManager.getInstance().GetCamera(0).ExposureTime;
                else
                    _spinExposureTime[i].Value = val;
            }
        }

        private int[] GetLinkCameraNumber()
        {
            SystemParam sysp = SystemParam.GetInstance();
            int[] camNo = new int[] { 0, 0 };
            foreach (CameraParam cp in sysp.camParam)
            {
                if (cp.OnOff == true)
                {
                    camNo[(int)cp.CamParts] = cp.CamNo;
                }
            }
            return camNo;
        }
        private void SetSpeedRange()
        {
            int[] camNo = GetLinkCameraNumber();

            SystemParam sysp = SystemParam.GetInstance();
            for (int i = 0; i < _spinCamSpeed.Length; i++)
            {
                double min, max, step, now;
                min = max = step = now = -1;
                CameraManager.getInstance().GetCamera(camNo[i]).GetLineRateRange(ref min, ref max, ref step, ref now);

                double val;
                val = sysp.Hz2Speed(min);
                _spinCamSpeed[i].Minimum = (decimal)val;
                val = sysp.Hz2Speed(max);
                _spinCamSpeed[i].Maximum = spinCamSpeed.Maximum;
            }
        }
        private void SetExposureRange()
        {
            int[] camNo = GetLinkCameraNumber();

            for (int i = 0; i < _spinExposureTime.Length; i++)
            {
                int min, max, step, now;
                min = max = step = now = -1;
                CameraManager.getInstance().GetCamera(camNo[i]).GetExposureTimeRange(ref min, ref max, ref step, ref now);

                _spinExposureTime[i].Minimum = min;
                _spinExposureTime[i].Maximum = max;
            }
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void spinCamSpeed_ValueChanged(object sender, EventArgs e)
        {
            double hz = SystemParam.GetInstance().Speed2Hz((double)spinCamSpeed.Value);
            SystemParam.GetInstance().DispHz(hz, lblCamHz, lblCamExp);
        }

        private void spinCamSpeedUra_ValueChanged(object sender, EventArgs e)
        {
            double hz = SystemParam.GetInstance().Speed2Hz((double)spinCamSpeedUra.Value);
            SystemParam.GetInstance().DispHz(hz, lblCamHzUra, lblCamExpUra);
        }
    }
}
