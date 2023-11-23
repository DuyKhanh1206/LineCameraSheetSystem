using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ViewROI;
using HalconDotNet;

namespace InspectionNameSpace
{
    public class WindowControl
    {
        /// <summary>
        /// 倍率
        /// </summary>
        public static double[] MagnifyList = new double[] { 0.01, 0.05, 0.1, 0.2, 0.5, 0.8, 1, 2, 4 };

        public HWindowControl hWin { get; private set; }
        /// <summary>
        /// ウィンドウ制御（イメージ、スクロール、拡大縮小など）
        /// </summary>
        public HWndCtrl WinManager { get; private set; }
        /// <summary>
        /// グラフィック制御（矩形、円、十字線など）
        /// </summary>
        public GraphicsManager GraphManager { get; private set; }
        /// <summary>
        /// ROI描画制御
        /// </summary>
        public ROIControllerCallback RoiCallBack { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="hWin">HWindowControl</param>
        /// <param name="hScl">横スクロールバー</param>
        /// <param name="vScl">縦スクロールバー</param>
        /// <param name="imgWidth">イメージ幅</param>
        /// <param name="imgHeight">イメージ高さ</param>
        public WindowControl(HWindowControl hWin, HScrollBar hScl, VScrollBar vScl, bool magnify, int imgWidth, int imgHeight)
        {
            this.hWin = hWin;
            //ウィンドウ制御
            WinManager = new HWndCtrl(hWin);
            //スクロールを関連付ける
            if (hScl != null && vScl != null)
            {
                WinManager.SetScrollbarControl(hScl, vScl);
            }

            //ダミーイメージ
            HImage img = new HImage("byte", imgWidth, imgHeight);

            //グラフィック制御
            GraphManager = new GraphicsManager();
            WinManager.useGraphManager(GraphManager);
            //ROI制御
            WinManager.useROIController(new ROIController());
            RoiCallBack = new ROIControllerCallback(WinManager.ROIManager);
            //イメージ
            WinManager.addIconicVar(img);
            //倍率
            if (magnify == true)
            {
                WinManager.SetMagnifyList(WindowControl.MagnifyList);
                //_hWndCtrl[i].SetMagnifyComboControl((i==0)?this.cmbMagnify1:this.cmbMagnify2);
            }
            //フィット
            WinManager.FittingImage(false);

            img.Dispose();
        }
    }
}
