using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ViewROI;

using HalconDotNet;
using Fujita.HalconMisc;

namespace Fujita.InspectionSystem
{
//    public delegate void WindowClickEventHander( object sender, WindowClickEventArgs e );

	public partial class uclHWindowMulti : UserControl, IHWinMulti
    {
        public event WindowClickEventHander WindowClick = null;

        private const int MAX_CONTROL = 4;

        Label[] _alblPaneName;

        List<int> _lstWindowIndex = new List<int>() { 0, 1, 2, 3 };

        public void SetWindowIndex(List<int> lstIndex)
        {
            _lstWindowIndex = new List<int>(lstIndex);
        }

        public List<int> WindowIndex
        {
            get
            {
                return _lstWindowIndex;
            }

            set
            {
                if (value.Count != MAX_CONTROL)
                    return;
                
            }
        }

        public uclHWindowMulti()
        {
            InitializeComponent();
            _alblPaneName = new Label[] { lblPane1, lblPane2, lblPane3, lblPane4 };

            hWindowControl1.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseDown);
            hWindowControl2.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseDown);
            hWindowControl3.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseDown);
            hWindowControl4.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseDown);
        }

        public uclHWindowMulti( List<int> lstWindowLayout )
        {
            InitializeComponent();
            _alblPaneName = new Label[] { lblPane1, lblPane2, lblPane3, lblPane4 };

            if (lstWindowLayout.Count == MAX_CONTROL)
            {
                _lstWindowIndex = new List<int>(lstWindowLayout);
            }

            hWindowControl1.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseDown);
            hWindowControl2.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseDown);
            hWindowControl3.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseDown);
            hWindowControl4.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseDown);
        }


        private int _windowNum = MAX_CONTROL;
        public int WindowNum 
        { 
            get
            {
                return _windowNum;
//                return _winLayout.WindowNum;
            }
            set
            {
                _windowNum = value;
                if (_winLayout != null)
                {
                    _winLayout.WindowNum = value;
                }
            }
        }

        public HWndCtrl GetWindowControl(int i)
        {
            if (i < 0 || i >= MAX_CONTROL)
                return null;

            return _lstWndCtrl[i];
        }

        public string GetWindowName(int i)
        {
            if (i < 0 || i >= MAX_CONTROL)
                return "";

            return _alblPaneName[_lstWindowIndex[i]].Text;
        }

        public bool SetWindowName(int i, string sName)
        {
            if (i < 0 || i >= MAX_CONTROL)
                return false;

            _alblPaneName[_lstWindowIndex[i]].Text = sName;

            return true;
        }

     
        HWindowLayouter _winLayout;
        List<HWndCtrl> _lstWndCtrl;
        List<ROIController> _lstRoiController;
        List<GraphicsManager> _lstGrpManager;

        private void HWindowMulti_Load(object sender, EventArgs e)
        {
            _winLayout = new HWindowLayouter(hWindowControl1, hWindowControl2, hWindowControl3, hWindowControl4, _lstWindowIndex );

            _lstWndCtrl = new List<HWndCtrl>(MAX_CONTROL);
            _lstWndCtrl.Add(new HWndCtrl(hWindowControl1));
//            _lstWndCtrl[0].Fitting = true;
            _lstWndCtrl.Add(new HWndCtrl(hWindowControl2));
//            _lstWndCtrl[1].Fitting = true;
            _lstWndCtrl.Add(new HWndCtrl(hWindowControl3));
//            _lstWndCtrl[2].Fitting = true;
            _lstWndCtrl.Add(new HWndCtrl(hWindowControl4));
//            _lstWndCtrl[3].Fitting = true;

            _lstRoiController = new List<ROIController>(MAX_CONTROL);
            _lstRoiController.Add(new ROIController());
            _lstRoiController.Add(new ROIController());
            _lstRoiController.Add(new ROIController());
            _lstRoiController.Add(new ROIController());

            _lstGrpManager = new List<GraphicsManager>(MAX_CONTROL);
            _lstGrpManager.Add(new GraphicsManager());
            _lstGrpManager.Add(new GraphicsManager());
            _lstGrpManager.Add(new GraphicsManager());
            _lstGrpManager.Add(new GraphicsManager());

            for (int i = 0; i < MAX_CONTROL; i++)
            {
                _lstWndCtrl[i].useROIController(_lstRoiController[i]);
                _lstWndCtrl[i].useGraphManager( _lstGrpManager[i]);
            }
            _winLayout.LayoutDefault();
            
        }

        public void LayoutDefault()
        {
            lblLayoutOne.Hide();
            for (int i = 0; i < _windowNum; i++)
            {
                _alblPaneName[_lstWindowIndex[i]].Show();
            }
            for (int i = _windowNum; i < MAX_CONTROL; i++)
            {
                _alblPaneName[_lstWindowIndex[i]].Hide();
            }
            _winLayout.LayoutDefault();
            for (int i = 0; i < _windowNum; i++)
            {
                _lstWndCtrl[i].FittingImage(true);
            }

        }

        public bool LayoutOne(int iWindowNo)
        {
            if (_alblPaneName.Length <= iWindowNo)
                return false;

            for (int i = 0; i < MAX_CONTROL; i++)
            {
                _alblPaneName[i].Hide();
            }
            lblLayoutOne.Show();
            lblLayoutOne.Text = _alblPaneName[_lstWindowIndex[iWindowNo]].Text;
            _winLayout.LayoutOne(iWindowNo);
            _lstWndCtrl[iWindowNo].FittingImage(true);

            return true;
        }
        public Size GetSizeLayoutOne()
        {
            return _winLayout.GetSizeLayoutOne();
        }
        public Size GetSizeLayoutMulti()
        {
            return _winLayout.GetSizeLayoutMulti();
        }

        public void DispCenterLine(bool bDisp)
        {
            for (int i = 0; i < MAX_CONTROL; i++)
            {
                _lstWndCtrl[i].dispCenterLine(bDisp);
            }
        }

        private void hWindowControl_Click(object sender, EventArgs e)
        {
/*
            int index = 0;
            if (WindowClick != null)
            {
                for (int i = 0; i < _lstWndCtrl.Count; i++)
                {
                    if (_lstWndCtrl[i] == sender)
                    {
                        index = i;
                    }
                }
                WindowClick(this, new WindowClickEventArgs(_lstWndCtrl[index], index));
            }
 */ 
        }

        private void hWindowControl1_HMouseMove(object sender, HalconDotNet.HMouseEventArgs e)
        {

        }

        private void hWindowControl_HMouseDown(object sender, HalconDotNet.HMouseEventArgs e)
        {
            int index = 0;
            if (WindowClick != null)
            {
                for (int i = 0; i < _lstWndCtrl.Count; i++)
                {
                    if (_lstWndCtrl[i].Window == sender)
                    {
                        index = i;
                        break;
                    }
                }
                WindowClick(this, new WindowClickEventArgs(_lstWndCtrl[index], index));
            }
        }
    }
}
