using System;
using ViewROI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using HalconDotNet;



namespace ViewROI
{
    public delegate void IconicDelegate(int val);
    public delegate void FuncDelegate();


    public class MouseMoveOnImageEventArgs : EventArgs
    {
        public int RValue { get; private set; }
        public int GValue { get; private set; }
        public int BValue { get; private set; }

        public int HValue { get; private set; }
        public int SValue { get; private set; }
        public int VValue { get; private set; }

        public int GrayValue { get; private set; }
        public EDispPlane DispPlane { get; private set; }

        public int ImagePosX { get; private set; }
        public int ImagePosY { get; private set; }
        public int ControlPosX { get; private set; }
        public int ControlPosY { get; private set; }

        public int ImageWidth { get; private set; }
        public int ImageHeight { get; private set; }

        public MouseMoveOnImageEventArgs(int rvalue, int gvalue, int bvalue, int hvalue, int svalue, int vvalue, EDispPlane plane, int imgx, int imgy, int ctrlx, int ctrly, int imgw, int imgh)
        {
            RValue = rvalue;
            GValue = gvalue;
            BValue = bvalue;

            HValue = hvalue;
            SValue = svalue;
            VValue = vvalue;

            GrayValue = -1;
            DispPlane = plane;

            ImagePosX = imgx;
            ImagePosY = imgy;
            ControlPosX = ctrlx;
            ControlPosY = ctrly;

            ImageWidth = imgw;
            ImageHeight = imgh;
        }

        public MouseMoveOnImageEventArgs(int grayvalue, EDispPlane plane, int imgx, int imgy, int ctrlx, int ctrly, int imgw, int imgh)
        {
            GrayValue = grayvalue;
            RValue = -1;
            GValue = -1;
            BValue = -1;
            HValue = -1;
            SValue = -1;
            VValue = -1;
            DispPlane = plane;

            ImagePosX = imgx;
            ImagePosY = imgy;
            ControlPosX = ctrlx;
            ControlPosY = ctrly;

            ImageWidth = imgw;
            ImageHeight = imgh;
        }

        public MouseMoveOnImageEventArgs(int imgx, int imgy, int ctrlx, int ctrly, int imgw, int imgh)
        {
            GrayValue = -1;
            RValue = -1;
            GValue = -1;
            BValue = -1;
            HValue = -1;
            SValue = -1;
            VValue = -1;
            DispPlane = EDispPlane.Default;

            ImagePosX = imgx;
            ImagePosY = imgy;
            ControlPosX = ctrlx;
            ControlPosY = ctrly;

            ImageWidth = imgw;
            ImageHeight = imgh;
        }
    }

    public delegate void MouseMoveOnImageEventHandler(object sender, MouseMoveOnImageEventArgs e);


    public class RepaintEventArgs : EventArgs
    {
        public IntPtr IHalconID { get; private set; }
        public HWindow HWindowID { get; private set; }

        public RepaintEventArgs(IntPtr iHalconID, HWindow hWindowID)
        {
            IHalconID = iHalconID;
            HWindowID = hWindowID;
        }
    }

    public delegate void RepaintEventHandler(object sender, RepaintEventArgs e);

    public enum EDispPlane
    {
        Default,
        Gray,
        Red,
        Blue,
        Green,
        Hue,
        Saturation,
        Intensity,
    }

    public class ChangeDispPlaneEventArgs : EventArgs
    {
        public EDispPlane NewPlane { get; private set; }
        public EDispPlane OldPlane { get; private set; }

        public ChangeDispPlaneEventArgs(EDispPlane newplane, EDispPlane oldplane)
        {
            NewPlane = newplane;
            OldPlane = oldplane;
        }
    }

    public delegate void ChangeDispPlaneEventHandler(object sender, ChangeDispPlaneEventArgs e);

    public class MouseDownActionEventArgs : EventArgs
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public MouseDownActionEventArgs(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public delegate void MouseDownActionEventHandler(object sender, MouseDownActionEventArgs e);

    public class MagnifyChageEventArgs : EventArgs
    {
        public double Magnify { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }

        public MagnifyChageEventArgs(double dMag, double dX, double dY)
        {
            Magnify = dMag;
            X = dX;
            Y = dY;
        }
    }

    public delegate void MagnifyChangeEventHadler(object sender, MagnifyChageEventArgs e);

    /// <summary>
    /// This class works as a wrapper class for the HALCON window
    /// HWindow. HWndCtrl is in charge of the visualization.
    /// You can move and zoom the visible image part by using GUI component 
    /// inputs or with the mouse. The class HWndCtrl uses a graphics stack 
    /// to manage the iconic objects for the display. Each object is linked 
    /// to a graphical context, which determines how the object is to be drawn.
    /// The context can be changed by calling changeGraphicSettings().
    /// The graphical "modes" are defined by the class GraphicsContext and 
    /// map most of the dev_set_* operators provided in HDevelop.
    /// </summary>
    public class HWndCtrl
    {
        /// <summary>No action is performed on mouse events</summary>
        public const int MODE_VIEW_NONE = 10;

        /// <summary>Zoom is performed on mouse events</summary>
        public const int MODE_VIEW_ZOOM = 11;

        /// <summary>Move is performed on mouse events</summary>
        public const int MODE_VIEW_MOVE = 12;

        /// <summary>Magnification is performed on mouse events</summary>
        public const int MODE_VIEW_ZOOMWINDOW = 13;


        public const int MODE_INCLUDE_ROI = 1;

        public const int MODE_EXCLUDE_ROI = 2;


        /// <summary>
        /// Constant describes delegate message to signal new image
        /// </summary>
        public const int EVENT_UPDATE_IMAGE = 31;
        /// <summary>
        /// Constant describes delegate message to signal error
        /// when reading an image from file
        /// </summary>
        public const int ERR_READING_IMG = 32;
        /// <summary> 
        /// Constant describes delegate message to signal error
        /// when defining a graphical context
        /// </summary>
        public const int ERR_DEFINING_GC = 33;

        /// <summary> 
        /// Maximum number of HALCON objects that can be put on the graphics 
        /// stack without loss. For each additional object, the first entry 
        /// is removed from the stack again.
        /// </summary>
        private const int MAXNUMOBJLIST = 50;


        private int stateView;
        private bool mousePressed = false;
        private double startX, startY;

        /// <summary>HALCON window</summary>
        private HWindowControl viewPort;

        /// <summary>
        /// Instance of ROIController, which manages ROI interaction
        /// </summary>
        private ROIController roiManager;

        /// <summary>
        /// Instance of ROIController, which manages ROI interaction
        /// </summary>
        private GraphicsManager grpManager;

        /* dispROI is a flag to know when to add the ROI models to the 
		   paint routine and whether or not to respond to mouse events for 
		   ROI objects */
        private int dispROI;


        /* Basic parameters, like dimension of window and displayed image part */
        private int windowWidth;
        private int windowHeight;
        private int imageWidth;

        public int ImageWidth
        {
            get { return imageWidth; }
        }
        private int imageHeight;
        public int ImageHeight
        {
            get { return imageHeight; }
        }

        private int[] CompRangeX;
        private int[] CompRangeY;


        private int prevCompX, prevCompY;
        private double stepSizeX, stepSizeY;


        /* Image coordinates, which describe the image part that is displayed  
		   in the HALCON window */
        private double ImgRow1, ImgCol1, ImgRow2, ImgCol2;

        /// <summary>Error message when an exception is thrown</summary>
        public string exceptionText = "";


        /* Delegates to send notification messages to other classes */
        /// <summary>
        /// Delegate to add information to the HALCON window after 
        /// the paint routine has finished
        /// </summary>
        public FuncDelegate addInfoDelegate;

        /// <summary>
        /// Delegate to notify about failed tasks of the HWndCtrl instance
        /// </summary>
        public IconicDelegate NotifyIconObserver;

        private EDispPlane _dispPlane = EDispPlane.Default;
        public EDispPlane DispPlane
        {
            get
            {
                return _dispPlane;
            }
            set
            {
                if (_dispPlane != value)
                {
                    EDispPlane old = _dispPlane;
                    _dispPlane = value;

                    if (ChangeDispPlane != null)
                    {
                        ChangeDispPlane(this, new ChangeDispPlaneEventArgs(_dispPlane, old));
                    }
                }
            }
        }
        public event ChangeDispPlaneEventHandler ChangeDispPlane = null;

        private HWindow ZoomWindow;
        private double zoomWndFactor;
        private double zoomAddOn;
        private int zoomWndSize;


        /// <summary> 
        /// List of HALCON objects to be drawn into the HALCON window. 
        /// The list shouldn't contain more than MAXNUMOBJLIST objects, 
        /// otherwise the first entry is removed from the list.
        /// </summary>
        private List<HObjectEntry> HObjList;
        //        private ArrayList HObjList;

        /// <summary>
        /// Instance that describes the graphical context for the
        /// HALCON window. According on the graphical settings
        /// attached to each HALCON object, this graphical context list 
        /// is updated constantly.
        /// </summary>
        private GraphicsContext mGC;


        /// <summary>
        /// 中心線を描画するかどうかを設定します
        /// 描画を行う場合、GraphManagerオブジェクトを生成し
        /// useGraphManagerメソッドで関連付けておく必要があります
        /// </summary>
        private bool _centerLine;
        public bool CenterLine
        {
            get { return _centerLine; }
            set
            {
                _centerLine = value;
            }
        }


        public bool NoRepaint { get; set; }

        public event MouseMoveOnImageEventHandler MouseMoveOnImage = null;

        public event RepaintEventHandler Repaint = null;
        public event RepaintEventHandler RepaintRoiBefore = null;
        public event RepaintEventHandler RepaintRoiAfter = null;

        public event MouseDownActionEventHandler MouseDownAction = null;

        public event MagnifyChangeEventHadler MagnifyChange = null;

        /// <summary>
        /// グリッド線を描画するかどうかを設定します
        /// 描画を行う場合、GraphManagerオブジェクトを生成し
        /// useGraphManagerメソッドで関連付けておく必要があります
        /// </summary>
        public bool GridLine { get; set; }

        /// <summary>
        /// グリッドラインのスパンを設定します
        /// </summary>
        public int GridLineSpan { get; set; }

        /// <summary>
        /// ２値化表示を行うかどうかを指定します
        /// </summary>
        public bool DispBin { get; set; }

        /// <summary>
        /// 初めて画像が追加されるとき、フィッティングを実行するかどうか
        /// </summary>
        public bool FirstTimeFitting { get; set; }

        /// <summary>
        /// 画面フィッティングを行う
        /// </summary>
        public bool Fitting { get; set; }

        /// <summary>
        /// Ctrlキーを押さずに、
        /// マウスクリックしながら移動するとイメージが移動する
        /// </summary>
        public bool IsNonCtrlKeyDragImageMove { get; set; }
        /// <summary>
        /// Shiftキーを押さずに、
        /// マウスクリックしながら、矩形囲みするとZoomする
        /// </summary>
        public bool IsNonShiftKeyImageZoom { get; set; }

        /// <summary>
        /// サイズ以上の場合の
        /// </summary>
        public bool FittingActualMagnification { get; set; }

        /// <summary>
        /// 表示枠より小さい倍率のとき、中央に表示する
        /// </summary>
        public bool IsDisplayInTheCenter { get; set; }

        /// <summary>
        /// ダブルクリック時の倍率合わせ
        /// false:コントロールに合わせる
        /// true:倍率リストから最適な数値を求める
        /// </summary>
        public bool IsDoubleClickMagnifyMode { get; set; }

        /// <summary>
        /// ダブルクリックと判断するクリック間隔時間[ms]
        /// </summary>
        public int DoubleClickTime
        {
            get
            {
                string sPath = @".\ViewROI.ini";
                Fujita.Misc.IniFileAccess ini = new Fujita.Misc.IniFileAccess();
                _doubleClickTime = ini.GetIni("DOUBLE_CLICK", "Click_Time", 0, sPath);
                return _doubleClickTime;
            }
            set
            {
                _doubleClickTime = value;
                string sPath = @".\ViewROI.ini";
                Fujita.Misc.IniFileAccess ini = new Fujita.Misc.IniFileAccess();
                ini.SetIni("DOUBLE_CLICK", "Click_Time", _doubleClickTime, sPath);
            }
        }
        private int _doubleClickTime;

        /// <summary>
        /// ダブルクリックと判断する領域
        /// </summary>
        public int DoubleClickArea
        {
            get
            {
                string sPath = @".\ViewROI.ini";
                Fujita.Misc.IniFileAccess ini = new Fujita.Misc.IniFileAccess();
                _doubleClickArea = ini.GetIni("DOUBLE_CLICK", "Click_Area", 20, sPath);
                return _doubleClickArea;
            }
            set
            {
                _doubleClickArea = value;
                string sPath = @".\ViewROI.ini";
                Fujita.Misc.IniFileAccess ini = new Fujita.Misc.IniFileAccess();
                ini.SetIni("DOUBLE_CLICK", "Click_Area", _doubleClickArea, sPath);
            }
        }
        private int _doubleClickArea;


        /// <summary>
        /// 下位閾値を設定します
        /// Thresholdプロパティがtrueの時のみ意味があります
        /// </summary>
        /// 
        private int __lowthreshold = 0;
        public int LowThreshold
        {
            get
            {
                return __lowthreshold;
            }

            set
            {
                if (value < 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                __lowthreshold = value;
            }
        }

        /// <summary>
        /// 下位閾値を設定します
        /// Thresholdプロパティがtrueの時のみ意味があります
        /// </summary>
        private int __highthreshold = 128;
        public int HighThreshold
        {
            get
            {
                return __highthreshold;
            }

            set
            {
                if (value < 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                __highthreshold = value;
            }
        }

        public bool DispBinPart { get; set; }
        private int _iHighThresholdPart = 128;
        public int HighThresholdPart
        {
            get { return _iHighThresholdPart; }
            set
            {
                if (value < 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _iHighThresholdPart = value;
            }
        }

        private int _iLowThresholdPart = 0;
        public int LowThresholdPart
        {
            get { return _iLowThresholdPart; }
            set
            {
                if (value < 0)
                    value = 0;
                if (value > 255)
                    value = 255;
                _iLowThresholdPart = value;
            }
        }

        // 表示領域
        public double DispBinPartRow1 { get; set; }
        public double DispBinPartCol1 { get; set; }
        public double DispBinPartRow2 { get; set; }
        public double DispBinPartCol2 { get; set; }


        /// <summary> 
        /// Initializes the image dimension, mouse delegation, and the 
        /// graphical context setup of the instance.
        /// </summary>
        /// <param name="view"> HALCON window </param>
        public HWndCtrl(HWindowControl view)
        {
            viewPort = view;
            stateView = MODE_VIEW_NONE;
            windowWidth = viewPort.Size.Width;
            windowHeight = viewPort.Size.Height;

            zoomWndFactor = (double)imageWidth / viewPort.Width;
            zoomAddOn = Math.Pow(0.9, 5);
            zoomWndSize = 150;

            /*default*/
            CompRangeX = new int[] { 0, 100 };
            CompRangeY = new int[] { 0, 100 };

            prevCompX = prevCompY = 0;

            dispROI = MODE_INCLUDE_ROI;//1;

            viewPort.HMouseUp += new HalconDotNet.HMouseEventHandler(this.mouseUp);
            viewPort.HMouseDown += new HalconDotNet.HMouseEventHandler(this.mouseDown);
            viewPort.HMouseMove += new HalconDotNet.HMouseEventHandler(this.mouseMoved);

            viewPort.MouseDown += ViewPort_MouseDown;
            viewPort.MouseMove += ViewPort_MouseMove;
            viewPort.MouseLeave += ViewPort_MouseLeave;
            viewPort.MouseUp += ViewPort_MouseUp;

            addInfoDelegate = new FuncDelegate(dummyV);
            NotifyIconObserver = new IconicDelegate(dummy);

            // graphical stack 
            HObjList = new List<HObjectEntry>();
            mGC = new GraphicsContext();
            mGC.gcNotification = new GCDelegate(exceptionGC);

            CenterLine = false;
            GridLine = false;
            GridLineSpan = 100;

            DispBin = false;
            LowThreshold = 0;
            HighThreshold = 128;

            _cxmMagnify = new ContextMenuStrip();
            viewPort.ContextMenuStrip = _cxmMagnify;

            FirstTimeFitting = false;
            Fitting = false;

            FittingActualMagnification = true;
        }

        public void SetBackColor()
        {
            string sPath = @".\ViewROI.ini";
            Fujita.Misc.IniFileAccess ini = new Fujita.Misc.IniFileAccess();
            string backColorName = ini.GetIni("WINDOW", "BackColorName", "Black", sPath);
            ini.SetIni("WINDOW", "BackColorName", backColorName, sPath);

            System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml(backColorName);
            string mColName = string.Format("#{0:X2}{1:X2}{2:X2}", c.R, c.G, c.B);
            HOperatorSet.SetWindowParam(Window.HalconWindow, "background_color", mColName);
        }
        public HWindowControl Window
        {
            get
            {
                return viewPort;
            }
        }

        /// <summary>
        /// Registers an instance of an ROIController with this window 
        /// controller (and vice versa).
        /// </summary>
        /// <param name="rC"> 
        /// Controller that manages interactive ROIs for the HALCON window 
        /// </param>
        public void useROIController(ROIController rC)
        {
            roiManager = rC;
            rC.setViewController(this);
        }

        public ROIController ROIManager
        {
            get
            {
                return roiManager;
            }
        }

        /// <summary>
        /// Registers an instance of an ROIController with this window 
        /// controller (and vice versa).
        /// </summary>
        /// <param name="gM"> 
        /// Controller that manages interactive ROIs for the HALCON window 
        /// </param>
        public void useGraphManager(GraphicsManager gM)
        {
            grpManager = gM;
            gM.setViewController(this);
        }

        public GraphicsManager GraphicManager
        {
            get
            {
                return grpManager;
            }
        }

        /// <summary>
        /// Read dimensions of the image to adjust own window settings
        /// </summary>
        /// <param name="image">HALCON image</param>
        private void setImagePart(HImage image)
        {
            string s;
            int w, h;

            image.GetImagePointer1(out s, out w, out h);
            setImagePart(0, 0, h, w);
        }


        /// <summary>
        /// Adjust window settings by the values supplied for the left 
        /// upper corner and the right lower corner
        /// </summary>
        /// <param name="r1">y coordinate of left upper corner</param>
        /// <param name="c1">x coordinate of left upper corner</param>
        /// <param name="r2">y coordinate of right lower corner</param>
        /// <param name="c2">x coordinate of right lower corner</param>
        private void setImagePart(int r1, int c1, int r2, int c2)
        {
            ImgRow1 = r1;
            ImgCol1 = c1;
            ImgRow2 = imageHeight = r2;
            ImgCol2 = imageWidth = c2;

            System.Drawing.Rectangle rect = viewPort.ImagePart;
            rect.X = (int)ImgCol1;
            rect.Y = (int)ImgRow1;
            rect.Height = (int)imageHeight;
            rect.Width = (int)imageWidth;
            viewPort.ImagePart = rect;
        }


        /// <summary>
        /// Sets the view mode for mouse events in the HALCON window
        /// (zoom, move, magnify or none).
        /// </summary>
        /// <param name="mode">One of the MODE_VIEW_* constants</param>
        public void setViewState(int mode)
        {
            stateView = mode;

            if (roiManager != null)
                roiManager.resetROI();
        }

        /********************************************************************/
        private void dummy(int val)
        {
        }

        private void dummyV()
        {
        }

        /*******************************************************************/
        private void exceptionGC(string message)
        {
            exceptionText = message;
            NotifyIconObserver(ERR_DEFINING_GC);
        }

        /// <summary>
        /// Paint or don't paint the ROIs into the HALCON window by 
        /// defining the parameter to be equal to 1 or not equal to 1.
        /// </summary>
        public void setDispLevel(int mode)
        {
            dispROI = mode;
        }

        /****************************************************************************/
        /*                          graphical element                               */
        /****************************************************************************/
        public void zoomImage(double x, double y, double scale)
        {
            double lengthC, lengthR;
            double percentC, percentR;
            int lenC, lenR;

            percentC = (x - ImgCol1) / (ImgCol2 - ImgCol1);
            percentR = (y - ImgRow1) / (ImgRow2 - ImgRow1);

            lengthC = (ImgCol2 - ImgCol1) * scale;
            lengthR = (ImgRow2 - ImgRow1) * scale;

            ImgCol1 = x - lengthC * percentC;
            ImgCol2 = x + lengthC * (1 - percentC);

            ImgRow1 = y - lengthR * percentR;
            ImgRow2 = y + lengthR * (1 - percentR);

            lenC = (int)Math.Round(lengthC);
            lenR = (int)Math.Round(lengthR);

            System.Drawing.Rectangle rect = viewPort.ImagePart;
            rect.X = (int)Math.Round(ImgCol1);
            rect.Y = (int)Math.Round(ImgRow1);
            rect.Width = (lenC > 0) ? lenC : 1;
            rect.Height = (lenR > 0) ? lenR : 1;
            viewPort.ImagePart = rect;

            zoomWndFactor *= scale;
            repaint();
        }

        /// <summary>
        /// Scales the image in the HALCON window according to the 
        /// value scaleFactor
        /// </summary>
        public void zoomImage(double scaleFactor)
        {
            double midPointX, midPointY;

            if (((ImgRow2 - ImgRow1) == scaleFactor * imageHeight) &&
                ((ImgCol2 - ImgCol1) == scaleFactor * imageWidth))
            {
                repaint();
                return;
            }

            ImgRow2 = ImgRow1 + imageHeight;
            ImgCol2 = ImgCol1 + imageWidth;

            midPointX = ImgCol1;
            midPointY = ImgRow1;

            zoomWndFactor = (double)imageWidth / viewPort.Width;
            zoomImage(midPointX, midPointY, scaleFactor);
        }


        /// <summary>
        /// Scales the HALCON window according to the value scale
        /// </summary>
        public void scaleWindow(double scale)
        {
            ImgRow1 = 0;
            ImgCol1 = 0;

            ImgRow2 = imageHeight;
            ImgCol2 = imageWidth;

            viewPort.Width = (int)(ImgCol2 * scale);
            viewPort.Height = (int)(ImgRow2 * scale);

            zoomWndFactor = ((double)imageWidth / viewPort.Width);
        }

        /// <summary>
        /// Recalculates the image-window-factor, which needs to be added to 
        /// the scale factor for zooming an image. This way the zoom gets 
        /// adjusted to the window-image relation, expressed by the equation 
        /// imageWidth/viewPort.Width.
        /// </summary>
        public void setZoomWndFactor()
        {
            zoomWndFactor = ((double)imageWidth / viewPort.Width);
        }

        /// <summary>
        /// Sets the image-window-factor to the value zoomF
        /// </summary>
        public void setZoomWndFactor(double zoomF)
        {
            zoomWndFactor = zoomF;
        }

        /*******************************************************************/
        private void moveImage(double motionX, double motionY)
        {
            ImgRow1 += -motionY;
            ImgRow2 += -motionY;

            ImgCol1 += -motionX;
            ImgCol2 += -motionX;

            System.Drawing.Rectangle rect = viewPort.ImagePart;
            rect.X = (int)Math.Round(ImgCol1);
            rect.Y = (int)Math.Round(ImgRow1);
            viewPort.ImagePart = rect;

            repaint();
        }


        /// <summary>
        /// Resets all parameters that concern the HALCON window display 
        /// setup to their initial values and clears the ROI list.
        /// </summary>
        public void resetAll()
        {
            ImgRow1 = 0;
            ImgCol1 = 0;
            ImgRow2 = imageHeight;
            ImgCol2 = imageWidth;

            zoomWndFactor = (double)imageWidth / viewPort.Width;

            System.Drawing.Rectangle rect = viewPort.ImagePart;
            rect.X = (int)ImgCol1;
            rect.Y = (int)ImgRow1;
            rect.Width = (int)imageWidth;
            rect.Height = (int)imageHeight;
            viewPort.ImagePart = rect;


            if (roiManager != null)
                roiManager.reset();
        }

        public void resetWindow()
        {
            ImgRow1 = 0;
            ImgCol1 = 0;
            ImgRow2 = imageHeight;
            ImgCol2 = imageWidth;

            zoomWndFactor = (double)imageWidth / viewPort.Width;

            System.Drawing.Rectangle rect = viewPort.ImagePart;
            rect.X = (int)ImgCol1;
            rect.Y = (int)ImgRow1;
            rect.Width = (int)imageWidth;
            rect.Height = (int)imageHeight;
            viewPort.ImagePart = rect;
        }

        private DateTime _dtPrevDown = DateTime.Now;
        System.Drawing.PointF _ptPrevDown = new System.Drawing.PointF();
        /// <summary>
        /// ダブルクリックされたかどうか判断する
        /// </summary>
        /// <param name="pt">クリックされたマウス位置</param>
        /// <returns>
        /// true : ダブルクリックされた
        /// false : ダブルクリックされていない
        /// </returns>
        private bool checkDoubleClick(System.Drawing.PointF pt)
        {
            DateTime dtNowDown = DateTime.Now;
            if ((dtNowDown - _dtPrevDown).TotalMilliseconds > (SystemInformation.DoubleClickTime + DoubleClickTime)
                || Math.Abs(pt.X - _ptPrevDown.X) > (double)(SystemInformation.DoubleClickSize.Width + DoubleClickArea)
                || Math.Abs(pt.Y - _ptPrevDown.Y) > (double)(SystemInformation.DoubleClickSize.Height + DoubleClickArea))
            {
                _dtPrevDown = dtNowDown;
                _ptPrevDown.X = pt.X;
                _ptPrevDown.Y = pt.Y;
                return false;
            }
            _dtPrevDown = dtNowDown.Subtract(TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime));
            return true;
        }

        /*************************************************************************/
        /*      			 Event handling for mouse	   	                     */
        /*************************************************************************/
        private void mouseDown(object sender, HalconDotNet.HMouseEventArgs e)
        {
            mousePressed = true;
            int activeROIidx = -1;
            double scale;

            if (MouseDownAction != null)
            {
                MouseDownAction(this, new MouseDownActionEventArgs(e.X, e.Y));
            }

            if (roiManager != null && (dispROI == MODE_INCLUDE_ROI) && e.Button == MouseButtons.Left)
            {
                activeROIidx = roiManager.mouseDownAction(e.X, e.Y);
            }

            if (activeROIidx == -1)
            {
                // ディフォルト動作
                if (checkDoubleClick(new System.Drawing.PointF((float)e.X, (float)e.Y)))
                {
                    FittingImage(IsDoubleClickMagnifyMode);
                }
                else if (IsNonShiftKeyImageZoom == true || Control.ModifierKeys == Keys.Shift)
                {
                    if ((Control.ModifierKeys & Keys.Control) != Keys.Control)
                    {
                        _nowDragZoom = true;
                        _clickX = e.X;
                        _clickY = e.Y;
                    }
                }

                switch (stateView)
                {
                    case MODE_VIEW_MOVE:
                        startX = e.X;
                        startY = e.Y;
                        break;
                    case MODE_VIEW_ZOOM:
                        if (e.Button == System.Windows.Forms.MouseButtons.Left)
                            scale = 0.9;
                        else
                            scale = 1 / 0.9;
                        zoomImage(e.X, e.Y, scale);
                        break;
                    case MODE_VIEW_NONE:
                        break;
                    case MODE_VIEW_ZOOMWINDOW:
                        activateZoomWindow((int)e.X, (int)e.Y);
                        break;
                    default:
                        break;
                }
            }
            //end of if
        }

        /*******************************************************************/
        private void activateZoomWindow(int X, int Y)
        {
            double posX, posY;
            int zoomZone;

            if (ZoomWindow != null)
                ZoomWindow.Dispose();

            HOperatorSet.SetSystem("border_width", 10);
            ZoomWindow = new HWindow();

            posX = ((X - ImgCol1) / (ImgCol2 - ImgCol1)) * viewPort.Width;
            posY = ((Y - ImgRow1) / (ImgRow2 - ImgRow1)) * viewPort.Height;

            zoomZone = (int)((zoomWndSize / 2) * zoomWndFactor * zoomAddOn);
            ZoomWindow.OpenWindow((int)posY - (zoomWndSize / 2), (int)posX - (zoomWndSize / 2),
                                   zoomWndSize, zoomWndSize,
                                   viewPort.HalconID, "visible", "");
            ZoomWindow.SetPart(Y - zoomZone, X - zoomZone, Y + zoomZone, X + zoomZone);
            repaint(ZoomWindow);
            ZoomWindow.SetColor("black");
        }

        /*******************************************************************/
        private void mouseUp(object sender, HalconDotNet.HMouseEventArgs e)
        {
            mousePressed = false;

            if (roiManager != null
                && (roiManager.activeROIidx != -1)
                && (dispROI == MODE_INCLUDE_ROI)
                && (e.Button == MouseButtons.Left))
            {
                roiManager.NotifyRCObserver(ROIController.EVENT_UPDATE_ROI);
            }
            else if (stateView == MODE_VIEW_ZOOMWINDOW)
            {
                ZoomWindow.Dispose();
            }
            else if (e.Button == MouseButtons.Right)
            {
                popupMagnifyMenu(e.X, e.Y);
            }
            else if (_nowDragZoom)
            {
                int width = Math.Abs((int)e.X - (int)_clickX);
                int height = Math.Abs((int)e.Y - (int)_clickY);
                int x = (int)((e.X + _clickX) / 2);
                int y = (int)((e.Y + _clickY) / 2);
                if (width > 20 && height > 20)
                {
                    double dMagW = (double)((double)viewPort.Width / (double)width);
                    double dMagH = (double)((double)viewPort.Height / (double)height);
                    ChangeMagnify((double)((dMagW < dMagH) ? dMagW : dMagH), x, y);
                    IsDragZoom = true;
                }
            }
        }

        /*******************************************************************/
        private void mouseMoved(object sender, HalconDotNet.HMouseEventArgs e)
        {
            double motionX, motionY;
            double posX, posY;
            double zoomZone;

            System.Drawing.Point pt = viewPort.PointToClient(System.Windows.Forms.Cursor.Position);

            if (getListCount() == 0)
                return;

            // 現在のマウス位置における表示
            if (e.X >= 0
                && e.X < _imageRealWidth
                && e.Y >= 0 && e.Y < _imageRealHeight
                && MouseMoveOnImage != null)
            {
                try
                {
                    if (DispPlane == EDispPlane.Default)
                    {
                        if (isColorImage(HObjList[0].HObj))
                        {
                            HObject hoR, hoG, hoB;
                            HObject hoH, hoS, hoV;
                            HTuple htRVal, htGVal, htBVal;
                            HTuple htHVal, htSVal, htVVal;

                            HOperatorSet.Decompose3(HObjList[0].HObj, out hoR, out hoG, out hoB);
                            HOperatorSet.GetGrayval(hoR, (int)e.Y, (int)e.X, out htRVal);
                            HOperatorSet.GetGrayval(hoG, (int)e.Y, (int)e.X, out htGVal);
                            HOperatorSet.GetGrayval(hoB, (int)e.Y, (int)e.X, out htBVal);

                            HObject hoRect, hoReduceR, hoReduceG, hoReduceB;
                            HOperatorSet.GenRectangle1(out hoRect, e.Y - 1, e.X - 1, e.Y + 1, e.X + 1);
                            HOperatorSet.ReduceDomain(hoR, hoRect, out hoReduceR);
                            HOperatorSet.ReduceDomain(hoG, hoRect, out hoReduceG);
                            HOperatorSet.ReduceDomain(hoB, hoRect, out hoReduceB);
                            HOperatorSet.TransFromRgb(hoReduceR, hoReduceG, hoReduceB, out hoH, out hoS, out hoV, "hsv");
                            HOperatorSet.GetGrayval(hoH, (int)e.Y, (int)e.X, out htHVal);
                            HOperatorSet.GetGrayval(hoS, (int)e.Y, (int)e.X, out htSVal);
                            HOperatorSet.GetGrayval(hoV, (int)e.Y, (int)e.X, out htVVal);
                            MouseMoveOnImage(this, new MouseMoveOnImageEventArgs(htRVal.I, htGVal.I, htBVal.I, htHVal.I, htSVal.I, htVVal.I, DispPlane, (int)e.X, (int)e.Y, pt.X, pt.Y, _imageRealWidth, _imageRealHeight));
                            hoR.Dispose(); hoG.Dispose(); hoB.Dispose();
                            hoH.Dispose(); hoS.Dispose(); hoV.Dispose();
                            hoRect.Dispose();
                            hoReduceR.Dispose(); hoReduceG.Dispose(); hoReduceB.Dispose();
                        }
                        else
                        {
                            HTuple htGrayVal;
                            HOperatorSet.GetGrayval(HObjList[0].HObj, (int)e.Y, (int)e.X, out htGrayVal);
                            MouseMoveOnImage(this, new MouseMoveOnImageEventArgs(htGrayVal.I, DispPlane, (int)e.X, (int)e.Y, pt.X, pt.Y, _imageRealWidth, _imageRealHeight));
                        }
                    }
                    else
                    {
                        HObject hoAdapt;
                        HTuple htGrayVal;
                        GetAdaptedImage(HObjList[0].HObj, out hoAdapt);
                        HOperatorSet.GetGrayval(hoAdapt, (int)e.Y, (int)e.X, out htGrayVal);
                        MouseMoveOnImage(this, new MouseMoveOnImageEventArgs(htGrayVal.I, DispPlane, (int)e.X, (int)e.Y, pt.X, pt.Y, _imageRealWidth, _imageRealHeight));
                        hoAdapt.Dispose();
                    }
                }
                catch (HalconException)
                {
                }
            }
            else if (MouseMoveOnImage != null)
            {
                MouseMoveOnImage(this, new MouseMoveOnImageEventArgs((int)e.X, (int)e.Y, pt.X, pt.Y, _imageRealWidth, _imageRealHeight));
            }

            if (!mousePressed)
                return;

            if (roiManager != null
                && (roiManager.activeROIidx != -1)
                && (dispROI == MODE_INCLUDE_ROI)
                && (e.Button == MouseButtons.Left))
            {
                roiManager.mouseMoveAction(e.X, e.Y);
            }
            else if (stateView == MODE_VIEW_MOVE)
            {
                motionX = ((e.X - startX));
                motionY = ((e.Y - startY));

                if (((int)motionX != 0) || ((int)motionY != 0))
                {
                    moveImage(motionX, motionY);
                    startX = e.X - motionX;
                    startY = e.Y - motionY;
                }
            }
            else if (stateView == MODE_VIEW_ZOOMWINDOW)
            {
                HSystem.SetSystem("flush_graphic", "false");
                ZoomWindow.ClearWindow();


                posX = ((e.X - ImgCol1) / (ImgCol2 - ImgCol1)) * viewPort.Width;
                posY = ((e.Y - ImgRow1) / (ImgRow2 - ImgRow1)) * viewPort.Height;
                zoomZone = (zoomWndSize / 2) * zoomWndFactor * zoomAddOn;

                ZoomWindow.SetWindowExtents((int)posY - (zoomWndSize / 2),
                                            (int)posX - (zoomWndSize / 2),
                                            zoomWndSize, zoomWndSize);
                ZoomWindow.SetPart((int)(e.Y - zoomZone), (int)(e.X - zoomZone),
                                   (int)(e.Y + zoomZone), (int)(e.X + zoomZone));
                repaint(ZoomWindow);

                HSystem.SetSystem("flush_graphic", "true");
                ZoomWindow.DispLine(-100.0, -100.0, -100.0, -100.0);
            }
        }

        /// <summary>
        /// To initialize the move function using a GUI component, the HWndCtrl
        /// first needs to know the range supplied by the GUI component. 
        /// For the x direction it is specified by xRange, which is 
        /// calculated as follows: GuiComponentX.Max()-GuiComponentX.Min().
        /// The starting value of the GUI component has to be supplied 
        /// by the parameter Init
        /// </summary>
        public void setGUICompRangeX(int[] xRange, int Init)
        {
            int cRangeX;

            CompRangeX = xRange;
            cRangeX = xRange[1] - xRange[0];
            prevCompX = Init;
            stepSizeX = ((double)imageWidth / cRangeX) * (imageWidth / windowWidth);

        }

        /// <summary>
        /// To initialize the move function using a GUI component, the HWndCtrl
        /// first needs to know the range supplied by the GUI component. 
        /// For the y direction it is specified by yRange, which is 
        /// calculated as follows: GuiComponentY.Max()-GuiComponentY.Min().
        /// The starting value of the GUI component has to be supplied 
        /// by the parameter Init
        /// </summary>
        public void setGUICompRangeY(int[] yRange, int Init)
        {
            int cRangeY;

            CompRangeY = yRange;
            cRangeY = yRange[1] - yRange[0];
            prevCompY = Init;
            stepSizeY = ((double)imageHeight / cRangeY) * (imageHeight / windowHeight);
        }


        /// <summary>
        /// Resets to the starting value of the GUI component.
        /// </summary>
        public void resetGUIInitValues(int xVal, int yVal)
        {
            prevCompX = xVal;
            prevCompY = yVal;
        }

        /// <summary>
        /// Moves the image by the value valX supplied by the GUI component
        /// </summary>
        public void moveXByGUIHandle(int valX)
        {
            double motionX;

            motionX = (valX - prevCompX) * stepSizeX;

            if (motionX == 0)
                return;

            moveImage(motionX, 0.0);
            prevCompX = valX;
        }


        /// <summary>
        /// Moves the image by the value valY supplied by the GUI component
        /// </summary>
        public void moveYByGUIHandle(int valY)
        {
            double motionY;

            motionY = (valY - prevCompY) * stepSizeY;

            if (motionY == 0)
                return;

            moveImage(0.0, motionY);
            prevCompY = valY;
        }

        /// <summary>
        /// Zooms the image by the value valF supplied by the GUI component
        /// </summary>
        public void zoomByGUIHandle(double valF)
        {
            double x, y, scale;
            double prevScaleC;



            x = (ImgCol1 + (ImgCol2 - ImgCol1) / 2);
            y = (ImgRow1 + (ImgRow2 - ImgRow1) / 2);

            prevScaleC = (double)((ImgCol2 - ImgCol1) / imageWidth);
            scale = ((double)1.0 / prevScaleC * (100.0 / valF));

            zoomImage(x, y, scale);
        }

        /// <summary>
        /// Triggers a repaint of the HALCON window
        /// </summary>
        public void repaint()
        {
            repaint(viewPort.HalconWindow);
        }

        private bool isColorImage(HObject img)
        {
            if (!isImage(img))
                return false;
            try
            {
                HTuple htNumber;
                HOperatorSet.CountChannels(img, out htNumber);
                if (htNumber != 3)
                    return false;
            }
            catch (HalconException)
            {
                return false;
            }
            return true;
        }

        public bool GetAdaptedImage(HObject img, out HObject destImg)
        {
            destImg = null;

            if (!isImage(img))
                return false;

            if (!isColorImage(img))
            {
                try
                {
                    HOperatorSet.CopyObj(img, out destImg, 1, -1);
                }
                catch (HalconException)
                {
                    return false;
                }
                return true;
            }

            try
            {
                HObject hoTemp1 = null, hoTemp2 = null, hoTemp3 = null, hoTemp4 = null, hoTemp5 = null;
                switch (DispPlane)
                {
                    case EDispPlane.Default:
                        HOperatorSet.CopyObj(img, out destImg, 1, -1);
                        break;
                    case EDispPlane.Red:
                        HOperatorSet.Decompose3(img, out destImg, out hoTemp1, out hoTemp2);
                        break;
                    case EDispPlane.Green:
                        HOperatorSet.Decompose3(img, out hoTemp1, out destImg, out hoTemp2);
                        break;
                    case EDispPlane.Blue:
                        HOperatorSet.Decompose3(img, out hoTemp1, out hoTemp2, out destImg);
                        break;
                    case EDispPlane.Gray:
                        HOperatorSet.Rgb1ToGray(img, out destImg);
                        break;
                    case EDispPlane.Hue:
                        HOperatorSet.Decompose3(img, out hoTemp1, out hoTemp2, out hoTemp3);
                        HOperatorSet.TransFromRgb(hoTemp1, hoTemp2, hoTemp3, out destImg, out hoTemp4, out hoTemp5, "hsv");
                        break;
                    case EDispPlane.Intensity:
                        HOperatorSet.Decompose3(img, out hoTemp1, out hoTemp2, out hoTemp3);
                        HOperatorSet.TransFromRgb(hoTemp1, hoTemp2, hoTemp3, out hoTemp4, out hoTemp5, out destImg, "hsv");
                        break;
                    case EDispPlane.Saturation:
                        HOperatorSet.Decompose3(img, out hoTemp1, out hoTemp2, out hoTemp3);
                        HOperatorSet.TransFromRgb(hoTemp1, hoTemp2, hoTemp3, out hoTemp4, out destImg, out hoTemp5, "hsv");
                        break;
                }

                if (hoTemp1 != null)
                    hoTemp1.Dispose();
                if (hoTemp2 != null)
                    hoTemp2.Dispose();
                if (hoTemp3 != null)
                    hoTemp3.Dispose();
                if (hoTemp4 != null)
                    hoTemp4.Dispose();
                if (hoTemp5 != null)
                    hoTemp5.Dispose();
            }
            catch (HalconException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Repaints the HALCON window 'window'
        /// </summary>
        public void repaint(HalconDotNet.HWindow window, bool bClear = true)
        {
            int count = HObjList.Count;
            HObjectEntry entry;

            try
            {
                HSystem.SetSystem("flush_graphic", "false");

                // ウインドウをクリアする
                window.ClearWindow();

                mGC.stateOfSettings.Clear();

                for (int i = 0; i < count; i++)
                {
                    entry = ((HObjectEntry)HObjList[i]);
                    mGC.applyContext(window, entry.gContext);

                    if (isImage(entry.HObj))
                    {
                        HObject img = null;
                        if (DispPlane == EDispPlane.Default)
                        {
                            window.DispObj(entry.HObj);
                            // カラーではない場合イメージコピー
                            if (!isColorImage(entry.HObj))
                            {
                                HOperatorSet.CopyObj(entry.HObj, out img, 1, -1);
                            }
                        }
                        else
                        {
                            if (GetAdaptedImage(entry.HObj, out img))
                                window.DispObj(img);
                        }
                        displayBinary(img, window);
                        if (img != null)
                            img.Dispose();
                    }
                    else
                    {
                        window.DispObj(entry.HObj);
                    }
                }


                addInfoDelegate();

                // 通常描画
                if (grpManager != null)
                {
                    grpManager.paintData(window, false);
                }

                if (Repaint != null)
                {
                    Repaint(this, new RepaintEventArgs(viewPort.HalconID, viewPort.HalconWindow));
                }

                // システム描画
                if (grpManager != null)
                {
                    grpManager.paintData(window, true);
                }

                // グリッドライン描画
                drawGridLine(window, GridLineSpan);

                // センターライン描画
                drawCenterLine(window);


                // ROI描画
                if (roiManager != null && (dispROI == MODE_INCLUDE_ROI))
                {
                    if (RepaintRoiBefore != null)
                    {
                        RepaintRoiBefore(this, new RepaintEventArgs(viewPort.HalconID, viewPort.HalconWindow));
                    }

                    roiManager.paintData(window);

                    if (RepaintRoiAfter != null)
                    {
                        RepaintRoiAfter(this, new RepaintEventArgs(viewPort.HalconID, viewPort.HalconWindow));
                    }
                }

                HSystem.SetSystem("flush_graphic", "true");

                window.SetColor("black");
                window.DispLine(-100.0, -100.0, -101.0, -101.0);
            }
            catch (HalconException)
            {
            }
        }

        private void drawCenterLine(HWindow window)
        {
            if (!CenterLine)
                return;

            try
            {
                HOperatorSet.SetLineWidth(viewPort.HalconWindow, 1);
                HOperatorSet.SetColor(viewPort.HalconWindow, GraphicsManager.HALCON_COLOR_RED);
                window.DispLine(_imageRealHeight / 2.0, 0, _imageRealHeight / 2.0, _imageRealWidth);
                window.DispLine(0, _imageRealWidth / 2.0, _imageRealHeight, _imageRealWidth / 2.0);
            }
            catch (HalconException)
            {

            }

        }

        private void drawGridLine(HWindow window, int iSpan)
        {
            if (!GridLine)
                return;

            try
            {
                HOperatorSet.SetLineWidth(viewPort.HalconWindow, 1);
                HOperatorSet.SetColor(viewPort.HalconWindow, GraphicsManager.HALCON_COLOR_GREEN);

                double dRowPosUp = _imageRealHeight / 2.0;
                double dRowPosDown = dRowPosUp + iSpan;

                do
                {
                    window.DispLine(dRowPosUp, 0, dRowPosUp, _imageRealWidth);
                    dRowPosUp -= iSpan;

                } while (dRowPosUp >= 0);

                do
                {
                    window.DispLine(dRowPosDown, 0, dRowPosDown, _imageRealWidth);
                    dRowPosDown += iSpan;

                } while (dRowPosDown <= _imageRealHeight);

                double dColPosLeft = _imageRealWidth / 2.0;
                double dColPosRight = dColPosLeft + iSpan;

                do
                {
                    window.DispLine(0, dColPosLeft, _imageRealHeight, dColPosLeft);
                    dColPosLeft -= iSpan;
                } while (dColPosLeft >= 0);

                do
                {
                    window.DispLine(0, dColPosRight, _imageRealHeight, dColPosRight);
                    dColPosRight += iSpan;
                } while (dColPosRight <= _imageRealWidth);
            }
            catch (HalconException)
            {
            }

        }

        private void displayBinary(HObject hoImg, HalconDotNet.HWindow window)
        {
            if (!DispBin && !DispBinPart)
                return;

            for (int i = 0; i < HObjList.Count; i++)
            {
                if (hoImg is HImage)
                {
                    HImage img = (HImage)hoImg;
                    HTuple width, height;
                    img.GetImageSize(out width, out height);

                    HImage imgTarget;
                    int iLowThreshold, iHighThreshold;
                    if (DispBinPart)
                    {
                        using (HRegion rgnTemp = new HRegion(DispBinPartRow1, DispBinPartCol1, DispBinPartRow2, DispBinPartCol2))
                        {
                            imgTarget = img.ReduceDomain(rgnTemp);
                        }
                        iLowThreshold = LowThresholdPart;
                        iHighThreshold = HighThresholdPart;
                    }
                    else
                    {
                        using (HRegion rgnTemp = new HRegion(0, 0, height.D, width.D))
                        {
                            imgTarget = img.ReduceDomain(rgnTemp);
                        }
                        iLowThreshold = LowThreshold;
                        iHighThreshold = HighThreshold;
                    }

                    HRegion rgn = null;
                    if (iLowThreshold <= iHighThreshold)
                    {
                        rgn = imgTarget.Threshold((double)iLowThreshold, (double)iHighThreshold);
                    }
                    else
                    {
                        using (HRegion rgnTemp = imgTarget.Threshold(new HTuple(new double[] { 0.0, iLowThreshold }), new HTuple(new double[] { iHighThreshold, 255.0 })))
                        {
                            rgn = rgnTemp.Union1();
                        }
                    }
                    HRegion rgnComl = rgn.Complement();
                    window.SetDraw("fill");
                    window.SetColor("white");
                    window.DispRegion(rgnComl);
                    window.SetColor("black");
                    window.DispRegion(rgn);

                    if (imgTarget != null)
                        imgTarget.Dispose();
                    if (rgnComl != null)
                        rgnComl.Dispose();
                    if (rgn != null)
                        rgn.Dispose();
                    return;
                }
                else if (isImage(hoImg))
                {
                    HObject hoRegion;
                    HObject hoImgTarget;
                    HObject hoRegionComplement;
                    HObject hoRegnTemp;
                    HObject hoRegionIntersection;

                    HTuple width, height;
                    HOperatorSet.GetImageSize(hoImg, out width, out height);

                    int iLowThreshold, iHighThreshold;
                    if (DispBinPart)
                    {
                        HOperatorSet.GenRectangle1(out hoRegnTemp, DispBinPartRow1, DispBinPartCol1, DispBinPartRow2, DispBinPartCol2);
                        HOperatorSet.ReduceDomain(hoImg, hoRegnTemp, out hoImgTarget);
                        iLowThreshold = LowThresholdPart;
                        iHighThreshold = HighThresholdPart;
                    }
                    else
                    {
                        HOperatorSet.GenRectangle1(out hoRegnTemp, 0, 0, height.D, width.D);
                        HOperatorSet.ReduceDomain(hoImg, hoRegnTemp, out hoImgTarget);
                        iLowThreshold = LowThreshold;
                        iHighThreshold = HighThreshold;
                    }

                    if (iLowThreshold <= iHighThreshold)
                    {
                        HOperatorSet.Threshold(hoImgTarget, out hoRegion, iLowThreshold, iHighThreshold);
                    }
                    else
                    {
                        HObject hoTemp;
                        HOperatorSet.Threshold(hoImgTarget, out hoTemp, new HTuple(new double[] { 0.0, iLowThreshold }), new HTuple(new double[] { iHighThreshold, 255.0 }));
                        HOperatorSet.Union1(hoTemp, out hoRegion);
                        hoTemp.Dispose();
                    }
                    HOperatorSet.Complement(hoRegion, out hoRegionComplement);
                    HOperatorSet.Intersection(hoRegionComplement, hoRegnTemp, out hoRegionIntersection);
                    window.SetDraw("fill");
                    window.SetColor("black");
                    HOperatorSet.DispObj(hoRegion, window);
                    window.SetColor("white");
                    HOperatorSet.DispObj(hoRegionIntersection, window);

                    hoRegion.Dispose();
                    hoRegionComplement.Dispose();
                    hoImgTarget.Dispose();

                    hoRegnTemp.Dispose();
                    hoRegionIntersection.Dispose();

                    return;
                }
            }
        }


        /********************************************************************/
        /*                      GRAPHICSSTACK                               */
        /********************************************************************/

        /// <summary>
        /// Adds an iconic object to the graphics stack similar to the way
        /// it is defined for the HDevelop graphics stack.
        /// </summary>
        /// <param name="obj">Iconic object</param>
        public void addIconicVar(HObject obj)
        {
            HObjectEntry entry;

            if (obj == null)
            {
                clearList();
                repaint();
                return;
            }

            bool wk_bFirstTime = false;
            if (obj is HImage || isImage(obj))
            {
                int h, w, area;
                HTuple hoArea, hoRow, hoColumn, hoPointer, hoType, hoWidth, hoHeight;
                HObject hoDomain;
                HOperatorSet.GetDomain(obj, out hoDomain);
                HOperatorSet.AreaCenter(hoDomain, out hoArea, out hoRow, out hoColumn);
                HOperatorSet.GetImagePointer1(obj, out hoPointer, out hoType, out hoWidth, out hoHeight);
                area = hoArea[0].I;
                h = hoHeight[0].I;
                w = hoWidth[0].I;

                if (area == (w * h))
                {
                    if (getListCount() == 0)
                        wk_bFirstTime = true;

                    clearList();

                    if ((h != imageHeight) || (w != imageWidth))
                    {
                        _imageRealWidth = w;
                        _imageRealHeight = h;
                        imageHeight = h;
                        imageWidth = w;

                        zoomWndFactor = (double)imageWidth / viewPort.Width;
                        //						setImagePart(0, 0, h, w);
                        // イメージの等倍表示を行う
                        setImagePart(0, 0, viewPort.Height, viewPort.Width);
                    }
                }//if
            }//if

            try
            {
                HObject wk_hoObj;
                HOperatorSet.CopyObj(obj, out wk_hoObj, 1, -1);
                entry = new HObjectEntry(wk_hoObj, mGC.copyContextList());
                HObjList.Add(entry);
            }
            catch (HOperatorException oe)
            {
                System.Diagnostics.Debug.WriteLine(oe.Message);
            }


            if (HObjList.Count > MAXNUMOBJLIST)
                HObjList.RemoveAt(1);

            if (Fitting)
            {
                FittingImage(IsDoubleClickMagnifyMode);
            }
            else if (FirstTimeFitting && wk_bFirstTime)
            {
                FittingImage(IsDoubleClickMagnifyMode);
                wk_bFirstTime = false;
            }
            else
            {
                ChangeMagnify(-1.0, -2.0, -2.0);
            }
        }
        public void addIconicVar2(HObject obj)
        {
            HObjectEntry entry;

            if (obj == null)
            {
                clearList();
                repaint();
                return;
            }

            bool wk_bFirstTime = false;
            if (obj is HImage || isImage(obj))
            {
                int h, w, area;
                HTuple hoArea, hoRow, hoColumn, hoPointer, hoType, hoWidth, hoHeight;
                HObject hoDomain;
                HOperatorSet.GetDomain(obj, out hoDomain);
                HOperatorSet.AreaCenter(hoDomain, out hoArea, out hoRow, out hoColumn);
                HOperatorSet.GetImagePointer1(obj, out hoPointer, out hoType, out hoWidth, out hoHeight);
                area = hoArea[0].I;
                h = hoHeight[0].I;
                w = hoWidth[0].I;

                //if (area == (w * h))
                {
                    if (getListCount() == 0)
                        wk_bFirstTime = true;

                    clearList();

                    if ((h != imageHeight) || (w != imageWidth))
                    {
                        _imageRealWidth = w;
                        _imageRealHeight = h;
                        imageHeight = h;
                        imageWidth = w;

                        zoomWndFactor = (double)imageWidth / viewPort.Width;
                        //						setImagePart(0, 0, h, w);
                        // イメージの等倍表示を行う
                        setImagePart(0, 0, viewPort.Height, viewPort.Width);
                    }
                }//if
            }//if

            try
            {
                HObject wk_hoObj;
                HOperatorSet.CopyObj(obj, out wk_hoObj, 1, -1);
                entry = new HObjectEntry(wk_hoObj, mGC.copyContextList());
                HObjList.Add(entry);
            }
            catch (HOperatorException oe)
            {
                System.Diagnostics.Debug.WriteLine(oe.Message);
            }


            if (HObjList.Count > MAXNUMOBJLIST)
                HObjList.RemoveAt(1);

            if (Fitting)
            {
                FittingImage(IsDoubleClickMagnifyMode);
            }
            else if (FirstTimeFitting && wk_bFirstTime)
            {
                FittingImage(IsDoubleClickMagnifyMode);
                wk_bFirstTime = false;
            }
            else
            {
                ChangeMagnify(-1.0, -2.0, -2.0);
            }
        }

        /// <summary>
        /// Clears all entries from the graphics stack 
        /// </summary>
        public void clearList()
        {
            foreach (HObjectEntry entry in HObjList)
            {
                entry.HObj.Dispose();
                entry.HObj = null;
            }
            HObjList.Clear();
        }

        /// <summary>
        /// Returns the number of items on the graphics stack
        /// </summary>
        public int getListCount()
        {
            return HObjList.Count;
        }

        /// <summary>
        /// Changes the current graphical context by setting the specified mode
        /// (constant starting by GC_*) to the specified value.
        /// </summary>
        /// <param name="mode">
        /// Constant that is provided by the class GraphicsContext
        /// and describes the mode that has to be changed, 
        /// e.g., GraphicsContext.GC_COLOR
        /// </param>
        /// <param name="val">
        /// Value, provided as a string, 
        /// the mode is to be changed to, e.g., "blue" 
        /// </param>
        public void changeGraphicSettings(string mode, string val)
        {
            switch (mode)
            {
                case GraphicsContext.GC_COLOR:
                    mGC.setColorAttribute(val);
                    break;
                case GraphicsContext.GC_DRAWMODE:
                    mGC.setDrawModeAttribute(val);
                    break;
                case GraphicsContext.GC_LUT:
                    mGC.setLutAttribute(val);
                    break;
                case GraphicsContext.GC_PAINT:
                    mGC.setPaintAttribute(val);
                    break;
                case GraphicsContext.GC_SHAPE:
                    mGC.setShapeAttribute(val);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Changes the current graphical context by setting the specified mode
        /// (constant starting by GC_*) to the specified value.
        /// </summary>
        /// <param name="mode">
        /// Constant that is provided by the class GraphicsContext
        /// and describes the mode that has to be changed, 
        /// e.g., GraphicsContext.GC_LINEWIDTH
        /// </param>
        /// <param name="val">
        /// Value, provided as an integer, the mode is to be changed to, 
        /// e.g., 5 
        /// </param>
        public void changeGraphicSettings(string mode, int val)
        {
            switch (mode)
            {
                case GraphicsContext.GC_COLORED:
                    mGC.setColoredAttribute(val);
                    break;
                case GraphicsContext.GC_LINEWIDTH:
                    mGC.setLineWidthAttribute(val);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Changes the current graphical context by setting the specified mode
        /// (constant starting by GC_*) to the specified value.
        /// </summary>
        /// <param name="mode">
        /// Constant that is provided by the class GraphicsContext
        /// and describes the mode that has to be changed, 
        /// e.g.,  GraphicsContext.GC_LINESTYLE
        /// </param>
        /// <param name="val">
        /// Value, provided as an HTuple instance, the mode is 
        /// to be changed to, e.g., new HTuple(new int[]{2,2})
        /// </param>
        public void changeGraphicSettings(string mode, HTuple val)
        {
            switch (mode)
            {
                case GraphicsContext.GC_LINESTYLE:
                    mGC.setLineStyleAttribute(val);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Clears all entries from the graphical context list
        /// </summary>
        public void clearGraphicContext()
        {
            mGC.clear();
        }

        /// <summary>
        /// Returns a clone of the graphical context list (hashtable)
        /// </summary>
        public Hashtable getGraphicContext()
        {
            return mGC.copyContextList();
        }

        /// <summary>
        /// 中心線を描画する
        /// </summary>
        /// <param name="disp">描画するかどうか</param>
        /// <param name="refresh">再描画するかどうか</param>
        public void dispCenterLine(bool disp, bool refresh = false)
        {
            if (grpManager != null)
            {
                grpManager.SetVisible("centerline_vert", disp);
                grpManager.SetVisible("centerline_horz", disp);

                if (refresh)
                {
                    repaint();
                }
            }
        }

        /// <summary>
        /// HObejct型がイメージかどうか判断する
        /// </summary>
        /// <param name="hoTest">テストするHObject</param>
        /// <returns>true イメージ型 false イメージ型ではない</returns>
        private bool isImage(HObject hoTest)
        {
            if (hoTest == null)
                return false;
            try
            {
                HTuple htClassObj;
                HOperatorSet.GetObjClass(hoTest, out htClassObj);
                if (htClassObj.Type != HTupleType.STRING)
                    return false;

                if (htClassObj[0].S == "image")
                    return true;
            }
            catch (HOperatorException)
            {
                return false;
            }
            return false;
        }

        /////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////
        public void SetMagnifyList()
        {
            string sPath = @".\ViewROI.ini";
            Fujita.Misc.IniFileAccess ini = new Fujita.Misc.IniFileAccess();
            int Start1 = ini.GetIni("DEFAULT_MAGNIFY", "1_START", 1, sPath);
            int End1 = ini.GetIni("DEFAULT_MAGNIFY", "1_END", 49, sPath);
            int Up1 = ini.GetIni("DEFAULT_MAGNIFY", "1_UP", 1, sPath);
            int Start2 = ini.GetIni("DEFAULT_MAGNIFY", "2_START", 50, sPath);
            int End2 = ini.GetIni("DEFAULT_MAGNIFY", "2_END", 200, sPath);
            int Up2 = ini.GetIni("DEFAULT_MAGNIFY", "2_UP", 10, sPath);

            List<double> mag = new List<double>();
            for (int i = Start1; i < End1; i += Up1)
                mag.Add((double)(i / 100.0));
            for (int i = Start2; i <= End2; i += Up2)
                mag.Add((double)(i / 100.0));
            SetMagnifyList(mag.ToArray());

            ini.SetIni("DEFAULT_MAGNIFY", "1_START", Start1, sPath);
            ini.SetIni("DEFAULT_MAGNIFY", "1_END", End1, sPath);
            ini.SetIni("DEFAULT_MAGNIFY", "1_UP", Up1, sPath);
            ini.SetIni("DEFAULT_MAGNIFY", "2_START", Start2, sPath);
            ini.SetIni("DEFAULT_MAGNIFY", "2_END", End2, sPath);
            ini.SetIni("DEFAULT_MAGNIFY", "2_UP", Up2, sPath);
        }
        List<double> _lstMagnify = null;
        HScrollBar _managedHScrollBar = null;
        VScrollBar _managedVScrollBar = null;
        double _magnify = 0.0;
        public double Magnify
        {
            get
            {
                return _magnify;
            }
        }

        int _imageRealWidth = 0;
        int _imageRealHeight = 0;
        /// <summary>
        /// 倍率のリストを表示する
        /// </summary>
        /// <param name="damagnify"></param>
        public void SetMagnifyList(double[] damagnify)
        {
            if (_lstMagnify != null)
            {
                _lstMagnify.Clear();
                _lstMagnify = null;
            }

            _lstMagnify = new List<double>(damagnify.Distinct());
            _lstMagnify.Sort((x, y) => y.CompareTo(x));

            // メニューリストを変更する
            makeMagnifyMenu();
        }

        /// <summary>
        /// スクロールバーコントロールを設定する
        /// </summary>
        /// <param name="hScroll"></param>
        /// <param name="vScroll"></param>
        public void SetScrollbarControl(HScrollBar hScroll, VScrollBar vScroll)
        {
            if (hScroll == null || vScroll == null)
                return;

            _managedHScrollBar = hScroll;
            _managedHScrollBar.Scroll += new ScrollEventHandler(_managedHScrollBar_Scroll);
            _managedVScrollBar = vScroll;
            _managedVScrollBar.Scroll += new ScrollEventHandler(_managedVScrollBar_Scroll);

            // 暫定的に無効にしておく
            _managedHScrollBar.Enabled = false;
            _managedVScrollBar.Enabled = false;
        }

        /// <summary>
        /// スクロールバーコントロールを解除する
        /// </summary>
        public void ResetScrollbarControl()
        {
            if (_managedHScrollBar != null)
            {
                _managedHScrollBar.Scroll -= new ScrollEventHandler(_managedHScrollBar_Scroll);
                _managedHScrollBar = null;
            }

            if (_managedVScrollBar != null)
            {
                _managedVScrollBar.Scroll -= new ScrollEventHandler(_managedVScrollBar_Scroll);
                _managedVScrollBar = null;
            }
        }

        ComboBox _cmbMagnifyComboBox = null;
        DataTable _dtMagnifyCombo = null;
        /// <summary>
        /// 倍率コンボコントロールを設定する
        /// </summary>
        /// <param name="cmbMag">倍率指定のコンボボックス</param>
        public bool SetMagnifyComboControl(ComboBox cmbMag)
        {
            // 倍率リストが生成されていない場合抜ける
            if (_lstMagnify == null)
                return false;

            _cmbMagnifyComboBox = cmbMag;
            _cmbMagnifyComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbMagnifyComboBox.Items.Clear();
            _cmbMagnifyComboBox.Sorted = false;

            if (_dtMagnifyCombo != null)
                _dtMagnifyCombo.Dispose();

            _dtMagnifyCombo = new DataTable();
            _dtMagnifyCombo.Columns.Add(new DataColumn("DispMember", typeof(string)));
            _dtMagnifyCombo.Columns.Add(new DataColumn("ValueMember", typeof(double)));
            for (int i = 0; i < _lstMagnify.Count; i++)
            {
                _dtMagnifyCombo.Rows.Add(new object[] { ((int)(_lstMagnify[i] * 100)).ToString() + "%", _lstMagnify[i] });
            }
            _cmbMagnifyComboBox.DataSource = _dtMagnifyCombo;
            _cmbMagnifyComboBox.DisplayMember = "DispMember";
            _cmbMagnifyComboBox.ValueMember = "ValueMember";

            setMagnifyComboEventHander();

            return true;
        }

        /// <summary>
        /// 倍率コンボコントロールを解除する
        /// </summary>
        public void ResetMagnifyComboControl()
        {
            resetMagnifyComboEventHandler();
            _cmbMagnifyComboBox = null;
        }

        private void setMagnifyComboEventHander()
        {
            if (_cmbMagnifyComboBox == null)
                return;
            _cmbMagnifyComboBox.SelectedIndexChanged += _cmbMagnifyComboBox_SelectedIndexChanged;
            _cmbMagnifyComboBox.DropDown += new EventHandler(_cmbMagnifyComboBox_DropDown);
            _cmbMagnifyComboBox.DropDownClosed += new EventHandler(_cmbMagnifyComboBox_DropDownClosed);
        }

        bool _bNowDropdown = false;
        void _cmbMagnifyComboBox_DropDownClosed(object sender, EventArgs e)
        {
            _bNowDropdown = false;
        }

        void _cmbMagnifyComboBox_DropDown(object sender, EventArgs e)
        {
            _bNowDropdown = true;
        }

        private void resetMagnifyComboEventHandler()
        {
            if (_cmbMagnifyComboBox == null)
                return;
            _cmbMagnifyComboBox.SelectedIndexChanged -= _cmbMagnifyComboBox_SelectedIndexChanged;
            _cmbMagnifyComboBox.DropDown -= new EventHandler(_cmbMagnifyComboBox_DropDown);
            _cmbMagnifyComboBox.DropDownClosed -= new EventHandler(_cmbMagnifyComboBox_DropDownClosed);
        }

        /// <summary>
        /// コンボボックスの倍率が変更された
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _cmbMagnifyComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeMagnify((double)_cmbMagnifyComboBox.SelectedValue, -1, -1, ECallFrom.FromInnerCombo);
        }

        /// <summary>
        /// 画面コントロールに全体が表示されるように表示する
        /// </summary>
        /// <param name="bAjustMagnifyList">
        /// true: 登録されている倍率で一番近い全体表示が可能な倍率に合わせる
        /// false: コントロールの大きさに合わせる
        /// </param>
        public void FittingImage(bool bAjustMagnifyList)
        {
            if (HObjList.Count == 0)
                return;

            if (!bAjustMagnifyList || _lstMagnify == null || _lstMagnify.Count == 0)
            {
                double wk_dWRate = (_imageRealWidth + 1) / (double)viewPort.Width;
                double wk_dHRate = (_imageRealHeight + 1) / (double)viewPort.Height;

                if (FittingActualMagnification && wk_dWRate < 1.0 && wk_dHRate < 1.0)
                {
                    System.Drawing.Rectangle rect = viewPort.ImagePart;
                    rect.X = 0;
                    rect.Y = 0;
                    rect.Width = viewPort.Width;
                    rect.Height = viewPort.Height;
                    viewPort.ImagePart = rect;
                    _magnify = 1.0;
                    if (wk_dWRate > wk_dHRate)
                        _magnify = 1 / wk_dWRate;
                    else
                        _magnify = 1 / wk_dHRate;
                }
                else
                {
                    if (wk_dWRate > wk_dHRate)
                    {
                        //				set_part( m_lWindowID,  0, 0, (Hlong)(wk_rc.Height() * wk_dWRate) , (Hlong)(wk_rc.Width() * wk_dWRate));
                        _magnify = 1 / wk_dWRate;
                    }
                    else
                    {
                        //				set_part( m_lWindowID,  0, 0, (Hlong)(wk_rc.Height() * wk_dHRate) , (Hlong)(wk_rc.Width() * wk_dHRate));
                        _magnify = 1 / wk_dHRate;
                    }
                }
                ChangeMagnify(_magnify, -1.0, -1.0);
            }
            else if (_lstMagnify.Count > 0)
            {
                for (int wk_i = _lstMagnify.Count - 1; wk_i >= 0; wk_i--)
                {
                    double wk_dMagWidth = _imageRealWidth * _lstMagnify[wk_i];
                    double wk_dMagHeight = _imageRealHeight * _lstMagnify[wk_i];

                    if (viewPort.Width < wk_dMagWidth || viewPort.Height < wk_dMagHeight)
                    {
                        wk_i++;
                        if (wk_i > _lstMagnify.Count - 1)
                            wk_i = _lstMagnify.Count - 1;
                        ChangeMagnify(_lstMagnify[wk_i], -1, -1);
                        break;
                    }
                }
            }
        }

        private enum ECallFrom
        {
            FromUser,
            FromInnerCombo,
            FromInnerMenu,
        }

        /// <summary>
        /// 倍率を変更する
        /// </summary>
        /// <param name="mag">倍率 1.0 定倍 4倍表示の場合4.0 1/2倍表示の時0.5を指定</param>
        /// <param name="centerx">イメージがコントロールからはみ出るときの中心のX座標 -1.0の場合中心</param>
        /// <param name="centery">イメージがコントロールからはみ出るときの中心のY座標 -1.0の場合中心</param>
        public void ChangeMagnify(double mag, double centerx = -1.0, double centery = -1.0)
        {
            ChangeMagnify(mag, centerx, centery, ECallFrom.FromUser);
        }

        /// <summary>
        /// 倍率変更
        /// </summary>
        /// <param name="mag"></param>
        /// <param name="centerx"></param>
        /// <param name="centery"></param>
        /// <param name="from">どこから呼ばれたか</param>
        private void ChangeMagnify(double mag, double centerx, double centery, ECallFrom from)
        {
            if (HObjList.Count == 0)
                return;

            HObjectEntry oe = (HObjectEntry)HObjList[0];

            // 倍率が-1.0の場合、現在の倍率を維持する
            if (mag == -1.0)
            {
                // 未定義倍率の場合暫定的に1を入れる
                if (_magnify == 0.0)
                {
                    _magnify = 1.0;
                }
                else
                {
                    mag = _magnify;
                }
            }

            if (mag < 0.0)
                return;

            double wk_dMagW = _imageRealWidth * mag;	// 倍率を考慮した幅
            double wk_dMagH = _imageRealHeight * mag;	// 倍率を考慮した高さ
            double wk_dRateW = wk_dMagW / viewPort.Width;
            double wk_dRateH = wk_dMagH / viewPort.Height;

            double dCenterColOffset = 0.0;
            double dCenterRowOffset = 0.0;

            int iRow1, iCol1, iRow2, iCol2;
            if (wk_dRateW <= 1.0)
            {
                if (_managedHScrollBar != null)
                {
                    _managedHScrollBar.Minimum = 0;
                    _managedHScrollBar.Maximum = 0;
                    _managedHScrollBar.Enabled = false;
                }
                iCol1 = 0;
                if (IsDisplayInTheCenter == true)
                {
                    dCenterColOffset = (viewPort.Width - (_imageRealWidth * mag)) / 2.0;
                    dCenterColOffset = dCenterColOffset / mag;
                }
            }
            else
            {
                double dScrollRange = _imageRealWidth - viewPort.Width / mag;
                if (_managedHScrollBar != null)
                {
                    _managedHScrollBar.Enabled = true;
                    _managedHScrollBar.Minimum = 0;
                    int iLargeChange = (int)(dScrollRange / 10);        //Ver1388
                    _managedHScrollBar.LargeChange = iLargeChange;
                    _managedHScrollBar.Maximum = (int)(dScrollRange + iLargeChange);
                }

                if (centerx == -1.0)
                {
                    iCol1 = (int)((dScrollRange) / 2.0);
                }
                else if (centerx == -2.0)
                {
                    if (_managedHScrollBar != null)
                    {
                        iCol1 = _managedHScrollBar.Value;
                    }
                    else
                    {
                        iCol1 = (int)((dScrollRange) / 2.0);
                    }
                }
                else
                {
                    iCol1 = (int)(centerx - viewPort.Width / mag / 2.0);
                    if (iCol1 < 0)
                        iCol1 = 0;
                    if (iCol1 > (int)(dScrollRange))
                        iCol1 = (int)(dScrollRange);
                }
                if (_managedHScrollBar != null)
                    _managedHScrollBar.Value = iCol1;
            }
            iCol2 = (int)(viewPort.Width / mag) + iCol1;

            if (wk_dRateH <= 1.0)
            {
                if (_managedVScrollBar != null)
                {
                    _managedVScrollBar.Minimum = 0;
                    _managedVScrollBar.Maximum = 0;
                    _managedVScrollBar.Enabled = false;
                }
                iRow1 = 0;
                if (IsDisplayInTheCenter == true)
                {
                    dCenterRowOffset = (viewPort.Height - (_imageRealHeight * mag)) / 2.0;
                    dCenterRowOffset = dCenterRowOffset / mag;
                }
            }
            else
            {
                double dScrollRange = _imageRealHeight - viewPort.Height / mag;
                if (_managedVScrollBar != null)
                {
                    _managedVScrollBar.Enabled = true;
                    _managedVScrollBar.Minimum = 0;
                    int iLargrChange = (int)dScrollRange / 10;              //Ver1388
                    _managedVScrollBar.LargeChange = iLargrChange;
                    _managedVScrollBar.Maximum = (int)(dScrollRange + iLargrChange);
                }

                if (centery == -1.0)
                {
                    iRow1 = (int)((dScrollRange) / 2.0);
                }
                else if (centery == -2.0)
                {
                    if (_managedVScrollBar != null)
                    {
                        iRow1 = _managedVScrollBar.Value;
                    }
                    else
                    {
                        iRow1 = (int)((dScrollRange) / 2.0);
                    }
                }
                else
                {
                    iRow1 = (int)(centery - viewPort.Height / mag / 2.0);
                    if (iRow1 < 0)
                        iRow1 = 0;
                    if (iRow1 > (int)(dScrollRange))
                        iRow1 = (int)(dScrollRange);
                }
                if (_managedVScrollBar != null)
                    _managedVScrollBar.Value = iRow1;
            }
            iRow2 = (int)(viewPort.Height / mag) + iRow1;

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(iCol1 - (int)dCenterColOffset, iRow1 - (int)dCenterRowOffset, iCol2 - iCol1, iRow2 - iRow1);
            viewPort.ImagePart = rect;

            _magnify = mag;

            // コンボボックスが登録されている場合、倍率を合わせる
            if (_cmbMagnifyComboBox != null && !_bNowDropdown)
            {
                switch (from)
                {
                    case ECallFrom.FromUser:
                    case ECallFrom.FromInnerMenu:
                        resetMagnifyComboEventHandler();
                        if (_cmbMagnifyComboBox.SelectedIndex != -1)
                        {
                            if ((double)_dtMagnifyCombo.Rows[_cmbMagnifyComboBox.SelectedIndex][1] != _magnify)
                            {
                                _cmbMagnifyComboBox.SelectedIndex = -1;
                                for (int i = 0; i < _dtMagnifyCombo.Rows.Count; i++)
                                {
                                    if ((double)_dtMagnifyCombo.Rows[i][1] == mag)
                                    {
                                        _cmbMagnifyComboBox.SelectedIndex = i;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            _cmbMagnifyComboBox.SelectedIndex = -1;
                            for (int i = 0; i < _dtMagnifyCombo.Rows.Count; i++)
                            {
                                if ((double)_dtMagnifyCombo.Rows[i][1] == mag)
                                {
                                    _cmbMagnifyComboBox.SelectedIndex = i;
                                    break;
                                }
                            }
                        }
                        setMagnifyComboEventHander();
                        break;
                }
            }

            if (!NoRepaint)
            {
                // 再描画
                repaint();
            }

            if (from != ECallFrom.FromUser)
            {
                if (MagnifyChange != null)
                {
                    MagnifyChange(this, new MagnifyChageEventArgs(mag, centerx, centery));
                }
            }
        }

        public bool IsDragZoom { get; private set; }
        private bool _nowClickMove;
        private bool _nowDragZoom;
        private double _clickX;
        private double _clickY;
        /// <summary>
        /// Windowマウスクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewPort_MouseDown(object sender, MouseEventArgs e)
        {
            //左マウスボタン：クリック
            if (e.Button == MouseButtons.Left)
            {
                if (IsNonCtrlKeyDragImageMove == true || Control.ModifierKeys == Keys.Control)
                {
                    _nowClickMove = true;
                    _clickX = e.X;
                    _clickY = e.Y;
                }
            }
        }
        /// <summary>
        /// Windowマウス移動
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewPort_MouseMove(object sender, MouseEventArgs e)
        {
            if (_nowClickMove)
            {
                if (_managedHScrollBar != null)
                {
                    int x = _managedHScrollBar.Value + (int)(_clickX - e.X);
                    int y = _managedVScrollBar.Value + (int)(_clickY - e.Y);

                    _managedHScrollBar.Value = (x < _managedHScrollBar.Minimum) ? _managedHScrollBar.Minimum : ((x > _managedHScrollBar.Maximum) ? _managedHScrollBar.Maximum : x);
                    _managedVScrollBar.Value = (y < _managedVScrollBar.Minimum) ? _managedVScrollBar.Minimum : ((y > _managedVScrollBar.Maximum) ? _managedVScrollBar.Maximum : y);
                    HSclBarScroll(_managedHScrollBar.Value);
                    VSclBarScroll(_managedVScrollBar.Value);
                }
                _clickX = e.X;
                _clickY = e.Y;
            }
        }
        /// <summary>
        /// WindowマウスクリックUP
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewPort_MouseUp(object sender, MouseEventArgs e)
        {
            IsDragZoom = false;
        }
        /// <summary>
        /// Windowマウスクリック解放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewPort_MouseLeave(object sender, EventArgs e)
        {
            _nowClickMove = false;
            _nowDragZoom = false;
        }

        /// <summary>
        /// 水平方向のスクロールが行われた時のイベントハンドラー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _managedHScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            HSclBarScroll(e.NewValue);
        }
        private void HSclBarScroll(int pos)
        {
            System.Drawing.Rectangle rect = viewPort.ImagePart;

            rect.X = pos;
            viewPort.ImagePart = rect;

            repaint();
        }

        /// <summary>
        /// 垂直方向のスクロールが行われた時のイベントハンドラー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _managedVScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            VSclBarScroll(e.NewValue);
        }
        private void VSclBarScroll(int pos)
        {
            System.Drawing.Rectangle rect = viewPort.ImagePart;

            rect.Y = pos;
            viewPort.ImagePart = rect;

            repaint();
        }

        ContextMenuStrip _cxmMagnify = null;
        private void makeMagnifyMenu()
        {
            if (_lstMagnify.Count == 0)
                return;

            // 既存のものがある場合削除しておく
            for (int i = 0; i < _cxmMagnify.Items.Count; i++)
            {
                _cxmMagnify.Items[i].Click -= magnifytoolMenu_Click;
            }
            _cxmMagnify.Items.Clear();

            // 新規倍率を設定する
            for (int i = 0; i < _lstMagnify.Count; i++)
            {
                ToolStripItem magnifytoolMenu = new ToolStripMenuItem();
                magnifytoolMenu.Text = ((int)(_lstMagnify[i] * 100)).ToString() + "%";
                magnifytoolMenu.Tag = _lstMagnify[i];
                magnifytoolMenu.Click += new EventHandler(magnifytoolMenu_Click);
                _cxmMagnify.Items.Add(magnifytoolMenu);
            }
        }

        double _dMagnifyMenuX = -1.0;
        double _dMagnifyMenuY = -1.0;
        /// <summary>
        /// メニューを表示する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void popupMagnifyMenu(double x, double y)
        {
            // 倍率リストが存在しない場合、抜ける
            _dMagnifyMenuX = x;
            _dMagnifyMenuY = y;
        }

        /// <summary>
        /// 倍率変更メニューがクリックされた時のイベントハンドラー
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void magnifytoolMenu_Click(object sender, EventArgs e)
        {
            ToolStripItem wk_ToolMenuItem = (ToolStripItem)sender;
            ChangeMagnify((double)wk_ToolMenuItem.Tag, _dMagnifyMenuX, _dMagnifyMenuY, ECallFrom.FromInnerMenu);
        }

        public bool IsImageExist()
        {
            return (HObjList.Count != 0);
        }

    }//end of class
}//end of namespace
