using Fujita.LightControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LineCameraSheetSystem
{
    public partial class frmLightTime : Form
    {
        List<uclLightTimeLabel> _lightTime = new List<uclLightTimeLabel>();
        public frmLightTime()
        {
            InitializeComponent();

            InitLightLabel();
        }
        /// <summary>
        /// Khởi tạo lable lighttiem hiển thị số lable light Time. có bao nhiêu đèn trong  thì có bấy nhiêu lable
        /// </summary>
        private void InitLightLabel()
        {
            LightControlManager ltCtrl = LightControlManager.getInstance();
            int yPt = 65;
            int height = 40;
            for (int i=0; i<ltCtrl.LightCount;i++)
            {
                uclLightTimeLabel ucl = new uclLightTimeLabel();
                ucl.Location = new System.Drawing.Point(16, yPt);
                ucl.Name = "LightTime" + i;
                ucl.Size = new System.Drawing.Size(310, height - 5);
                _lightTime.Add(ucl);
                this.Controls.Add(ucl);

                yPt += height;
            }
            this.Size = new Size(this.Size.Width, yPt + 130);
        }

        /// <summary>
        /// hiển thị thời gian cảnh báo và thời gian chiếu sáng
        /// </summary>
        private void frmLightTime_Load(object sender, EventArgs e)
        {
            SystemContext sysCont = SystemContext.GetInstance();
            SystemParam sysParam = SystemParam.GetInstance();
            LightControlManager ltCtrl = LightControlManager.getInstance();

            //警告時間 
            uclWarningTime.DisplayValue = sysParam.LightWarningTime.ToString();

            //点灯時間
            for (int i = 0; i < sysCont.LightMeasPeriod.Length; i++)
            {
                _lightTime[i].Title = ltCtrl.GetLight(i).Name;
                _lightTime[i].DisplayValue = sysCont.LightMeasPeriod[i].AccumulateHour.ToString();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
