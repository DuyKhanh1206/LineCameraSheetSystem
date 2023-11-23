using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using HalconDotNet;

namespace ViewROI
{

    public class GraphicsManager
    {
        /// <summary>
        /// グラフィックスタイプ
        /// </summary>
        enum GraphicType
        {
            Rectangle1,
            Rectangle2,
            Circle,
            Ellipse,
            Line,
            Cross,
            Arc,
            Object,
            Text,
        }
        /// <summary>
        /// DrawMode オペレーター margin
        /// </summary>
        public const string HALCON_DRAWMODE_MARGIN = "margin";
        /// <summary>
        /// DrawMode オペレーター fill
        /// </summary>
        public const string HALCON_DRAWMODE_FILL = "fill";

        /// <summary>
        /// SetColor オペレーター black
        /// </summary>
        public const string HALCON_COLOR_BLACK = "black";
        /// <summary>
        /// SetColor オペレーター white
        /// </summary>
        public const string HALCON_COLOR_WHITE = "white";
        /// <summary>
        /// SetColor オペレーター red
        /// </summary>
        public const string HALCON_COLOR_RED = "red";
        /// <summary>
        /// SetColor オペレーター green
        /// </summary>
        public const string HALCON_COLOR_GREEN = "green";
        /// <summary>
        /// SetColor オペレーター blue
        /// </summary>
        public const string HALCON_COLOR_BLUE = "blue";
        /// <summary>
        /// SetColor オペレーター cyan
        /// </summary>
        public const string HALCON_COLOR_CYAN = "cyan";
        /// <summary>
        /// SetColor オペレーター magenta
        /// </summary>
        public const string HALCON_COLOR_MAGENTA = "magenta";
        /// <summary>
        /// SetColor オペレーター yellow
        /// </summary>
        public const string HALCON_COLOR_YELLOW = "yellow";
        /// <summary>
        /// SetColor オペレーター dim gray
        /// </summary>
        public const string HALCON_COLOR_DIM_GRAY = "dim gray";
        /// <summary>
        /// SetColor オペレーター gray
        /// </summary>
        public const string HALCON_COLOR_GRAY = "gray";
        /// <summary>
        /// SetColor オペレーター light gray
        /// </summary>
        public const string HALCON_COLOR_LIGHT_GRAY = "light gray";
        /// <summary>
        /// SetColor オペレーター medium slate blue
        /// </summary>
        public const string HALCON_COLOR_MEDIUM_SLATE_BLUE = "medium slate blue";
        /// <summary>
        /// SetColor オペレーター coral
        /// </summary>
        public const string HALCON_COLOR_CORAL = "coral";
        /// <summary>
        /// SetColor オペレーター slate blue
        /// </summary>
        public const string HALCON_COLOR_SLATE_BLUE = "slate blue";
        /// <summary>
        /// SetColor オペレーター spring green
        /// </summary>
        public const string HALCON_COLOR_SPRING_GREEN = "spring green";
        /// <summary>
        /// SetColor オペレーター orage red
        /// </summary>
        public const string HALCON_COLOR_ORANGE_RED = "orange red";
        /// <summary>
        /// SetColor オペレーター orange
        /// </summary>
        public const string HALCON_COLOR_ORANGE = "orange";
        /// <summary>
        /// SetColor オペレーター dark olive green
        /// </summary>
        public const string HALCON_COLOR_DARK_OLIVE_GREEN = "dark olive green";
        /// <summary>
        /// SetColor オペレーター pink
        /// </summary>
        public const string HALCON_COLOR_PINK = "pink";
        /// <summary>
        /// SetColor オペレーター cadet blue
        /// </summary>
        public const string HALCON_COLOR_CADET_BLUE = "cadet blue";

        /// <summary>
        /// デフォルトフォント
        /// </summary>
        public const string DEFAULT_FONT_NAME = "ＭＳ 明朝";


        /// <summary>
        /// グラフィックオブジェクト ベースクラス
        /// </summary>
        class Graphic
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="mgr">GraphicsManagerオブジェクト</param>
            /// <param name="name">オブジェクト名</param>
            /// <param name="color">描画色</param>
            /// <param name="drawmode">描画モード</param>
            /// <param name="linewidth">線幅</param>
            /// <param name="type">グラフィックタイプ</param>
            public Graphic(GraphicsManager mgr, string name, string color, string drawmode, int linewidth, GraphicType type)
            {
                Visible = true;
                Name = name;
                Color = color;
                DrawMode = drawmode;
                Type = type;
                LineWidth = linewidth;

                _mgr = mgr;
            }

            /// <summary>
            /// オブジェクト名
            /// </summary>
            public string Name { get; private set; }
            /// <summary>
            /// オブジェクトタイプ
            /// </summary>
            public GraphicType Type { get; private set; }
            /// <summary>
            /// 色
            /// </summary>
            public string Color { get; internal set; }
            /// <summary>
            /// 描画モード
            /// </summary>
            public string DrawMode { get; internal set; }
            /// <summary>
            /// ライン幅
            /// </summary>
            public int LineWidth { get; internal set; }
            /// <summary>
            /// システムデータかどうか システムデータは一括削除ができない
            /// </summary>
            public bool System { get; internal set; }
            /// <summary>
            /// 描画を行うかどうか
            /// </summary>
            public bool Visible { get; set; }

            /// <summary>
            /// 管理オブジェクト
            /// </summary>
            private GraphicsManager _mgr;

            /// <summary>
            /// 描画メソッド 
            /// </summary>
            /// <param name="window">HWindowオブジェクト</param>
            public virtual void Draw(HWindow window) { }
        }

        class GraphicRectangle1 : Graphic
        {

            internal double _dRow1;
            internal double _dCol1;
            internal double _dRow2;
            internal double _dCol2;

            public GraphicRectangle1(GraphicsManager mgr, string name, double row1, double col1, double row2, double col2, string color, string drawmode, int linewidth)
                : base(mgr, name, color, drawmode, linewidth, GraphicType.Rectangle1)
            {
                _dRow1 = row1;
                _dCol1 = col1;
                _dRow2 = row2;
                _dCol2 = col2;
            }

            public override void Draw(HWindow window)
            {
                window.SetColor(Color);
                window.SetDraw(DrawMode);
                window.SetLineWidth(LineWidth);
                window.DispRectangle1(_dRow1, _dCol1, _dRow2, _dCol2);
            }
        }

        class GraphicRectangle2 : Graphic
        {
            internal double _dRow;
            internal double _dCol;
            internal double _dPhi;
            internal double _dLen1;
            internal double _dLen2;

            public GraphicRectangle2(GraphicsManager mgr, string name, double row, double col, double phi, double len1, double len2, string color, string drawmode, int linewidth)
                : base(mgr, name, color, drawmode, linewidth, GraphicType.Rectangle2)
            {
                _dRow = row;
                _dCol = col;
                _dPhi = phi;
                _dLen1 = len1;
                _dLen2 = len2;
            }

            public override void Draw(HWindow window)
            {
                window.SetColor(Color);
                window.SetDraw(DrawMode);
                window.SetLineWidth(LineWidth);
                window.DispRectangle2(_dRow, _dCol, _dPhi, _dLen1, _dLen2);
            }
        }

        class GraphicCircle : Graphic
        {
            internal double _dRow;
            internal double _dCol;
            internal double _dRad;

            public GraphicCircle(GraphicsManager mgr, string name, double row, double col, double rad, string color, string drawmode, int linewidth)
                : base(mgr, name, color, drawmode, linewidth, GraphicType.Circle)
            {
                _dRow = row;
                _dCol = col;
                _dRad = rad;
            }

            public override void Draw(HWindow window)
            {
                window.SetColor(Color);
                window.SetDraw(DrawMode);
                window.SetLineWidth(LineWidth);
                window.DispCircle(_dRow, _dCol, _dRad);
            }

        }

        class GraphicCross : Graphic
        {
            internal double _dRow;
            internal double _dCol;
            internal double _dSize;
            internal double _dAngle;

            public GraphicCross(GraphicsManager mgr, string name, double row, double col, double size, double angle, string color, string drawmode, int linewidth)
                : base(mgr, name, color, drawmode, linewidth, GraphicType.Cross)
            {
                _dRow = row;
                _dCol = col;
                _dSize = size;
                _dAngle = angle;
            }

            public override void Draw(HWindow window)
            {
                window.SetColor(Color);
                window.SetDraw(DrawMode);
                window.SetLineWidth(LineWidth);
                window.DispCross(_dRow, _dCol, _dSize, _dAngle);
            }
        }

        class GraphicEllipse : Graphic
        {
            internal double _dRow;
            internal double _dCol;
            internal double _dPhi;
            internal double _dRad1;
            internal double _dRad2;

            public GraphicEllipse(GraphicsManager mgr, string name, double row, double col, double phi, double rad1, double rad2, string color, string drawmode, int linewidth)
                : base(mgr, name, color, drawmode, linewidth, GraphicType.Ellipse)
            {
                _dRow = row;
                _dCol = col;
                _dPhi = phi;
                _dRad1 = rad1;
                _dRad2 = rad2;
            }

            public override void Draw(HWindow window)
            {
                window.SetColor(Color);
                window.SetDraw(DrawMode);
                window.SetLineWidth(LineWidth);
                window.DispEllipse(_dRow, _dCol, _dPhi, _dRad1, _dRad2);
            }
        }

        class GraphicArc : Graphic
        {
            internal double _dRow;
            internal double _dCol;
            internal double _dAngle;
            internal double _dBeginRow;
            internal double _dBeginCol;

            public GraphicArc(GraphicsManager mgr, string name, double row, double col, double rad, double beginrow, double begincol, string color, string drawmode, int linewidth)
                : base(mgr, name, color, drawmode, linewidth, GraphicType.Arc)
            {
                _dRow = row;
                _dCol = col;
                _dAngle = rad;
                _dBeginRow = beginrow;
                _dBeginCol = begincol;
            }

            public override void Draw(HWindow window)
            {
                window.SetColor(Color);
                window.SetDraw(DrawMode);
                window.SetLineWidth(LineWidth);
                window.DispArc(_dRow, _dCol, _dAngle, _dBeginRow, _dBeginCol);
            }

        }

        class GraphicLine : Graphic
        {
            internal double _dBeginRow;
            internal double _dBeginCol;
            internal double _dEndRow;
            internal double _dEndCol;

            public GraphicLine(GraphicsManager mgr, string name, double beginrow, double begincol, double endrow, double endcol, string color, string drawmode, int linewidth)
                : base(mgr, name, color, drawmode, linewidth, GraphicType.Line)
            {
                _dBeginRow = beginrow;
                _dBeginCol = begincol;
                _dEndRow = endrow;
                _dEndCol = endcol;
            }

            public override void Draw(HWindow window)
            {
                window.SetColor(Color);
                window.SetLineWidth(LineWidth);
                window.DispLine(_dBeginRow, _dBeginCol, _dEndRow, _dEndCol);
            }
        }

        class GraphicObject : Graphic
        {
            internal HObject _oObject = null;

            public GraphicObject(GraphicsManager mgr, string name, HObject obj, string color, string drawmode, int linewidth)
                : base(mgr, name, color, drawmode, linewidth, GraphicType.Object)
            {
                HOperatorSet.CopyObj(obj, out _oObject, 1, -1);
            }

            public override void Draw(HWindow window)
            {
                window.SetColor(Color);
                window.SetDraw(DrawMode);
                window.SetLineWidth(LineWidth);
                window.DispObj(_oObject);
            }
        }

        class GraphicText : Graphic
        {
            internal string _sText;
            internal int _iFontSize;
            internal bool _bBox;
            internal bool _bWindow;
            internal double _dX;
            internal double _dY;

            public void disp_message(HTuple hv_WindowHandle, HTuple hv_String, HTuple hv_CoordSystem,
                HTuple hv_Row, HTuple hv_Column, HTuple hv_Color, HTuple hv_Box)
            {


                // Local control variables 

                HTuple hv_Red, hv_Green, hv_Blue, hv_Row1Part;
                HTuple hv_Column1Part, hv_Row2Part, hv_Column2Part, hv_RowWin;
                HTuple hv_ColumnWin, hv_WidthWin, hv_HeightWin, hv_MaxAscent;
                HTuple hv_MaxDescent, hv_MaxWidth, hv_MaxHeight, hv_R1 = new HTuple();
                HTuple hv_C1 = new HTuple(), hv_FactorRow = new HTuple(), hv_FactorColumn = new HTuple();
                HTuple hv_Width = new HTuple(), hv_Index = new HTuple(), hv_Ascent = new HTuple();
                HTuple hv_Descent = new HTuple(), hv_W = new HTuple(), hv_H = new HTuple();
                HTuple hv_FrameHeight = new HTuple(), hv_FrameWidth = new HTuple();
                HTuple hv_R2 = new HTuple(), hv_C2 = new HTuple(), hv_DrawMode = new HTuple();
                HTuple hv_Exception = new HTuple(), hv_CurrentColor = new HTuple();

                HTuple hv_Color_COPY_INP_TMP = hv_Color.Clone();
                HTuple hv_Column_COPY_INP_TMP = hv_Column.Clone();
                HTuple hv_Row_COPY_INP_TMP = hv_Row.Clone();
                HTuple hv_String_COPY_INP_TMP = hv_String.Clone();

                // Initialize local and output iconic variables 

                //This procedure displays text in a graphics window.
                //
                //Input parameters:
                //WindowHandle: The WindowHandle of the graphics window, where
                //   the message should be displayed
                //String: A tuple of strings containing the text message to be displayed
                //CoordSystem: If set to 'window', the text position is given
                //   with respect to the window coordinate system.
                //   If set to 'image', image coordinates are used.
                //   (This may be useful in zoomed images.)
                //Row: The row coordinate of the desired text position
                //   If set to -1, a default value of 12 is used.
                //Column: The column coordinate of the desired text position
                //   If set to -1, a default value of 12 is used.
                //Color: defines the color of the text as string.
                //   If set to [], '' or 'auto' the currently set color is used.
                //   If a tuple of strings is passed, the colors are used cyclically
                //   for each new textline.
                //Box: If set to 'true', the text is written within a white box.
                //
                //prepare window
                HOperatorSet.GetRgb(hv_WindowHandle, out hv_Red, out hv_Green, out hv_Blue);
                HOperatorSet.GetPart(hv_WindowHandle, out hv_Row1Part, out hv_Column1Part, out hv_Row2Part,
                    out hv_Column2Part);
                HOperatorSet.GetWindowExtents(hv_WindowHandle, out hv_RowWin, out hv_ColumnWin,
                    out hv_WidthWin, out hv_HeightWin);
                HOperatorSet.SetPart(hv_WindowHandle, 0, 0, hv_HeightWin - 1, hv_WidthWin - 1);
                //
                //default settings
                if ((int)(new HTuple(hv_Row_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Row_COPY_INP_TMP = 12;
                }
                if ((int)(new HTuple(hv_Column_COPY_INP_TMP.TupleEqual(-1))) != 0)
                {
                    hv_Column_COPY_INP_TMP = 12;
                }
                if ((int)(new HTuple(hv_Color_COPY_INP_TMP.TupleEqual(new HTuple()))) != 0)
                {
                    hv_Color_COPY_INP_TMP = "";
                }
                //
                hv_String_COPY_INP_TMP = ((("" + hv_String_COPY_INP_TMP) + "")).TupleSplit("\n");
                //
                //Estimate extentions of text depending on font size.
                HOperatorSet.GetFontExtents(hv_WindowHandle, out hv_MaxAscent, out hv_MaxDescent,
                    out hv_MaxWidth, out hv_MaxHeight);
                if ((int)(new HTuple(hv_CoordSystem.TupleEqual("window"))) != 0)
                {
                    hv_R1 = hv_Row_COPY_INP_TMP.Clone();
                    hv_C1 = hv_Column_COPY_INP_TMP.Clone();
                }
                else
                {
                    //transform image to window coordinates
                    hv_FactorRow = (1.0 * hv_HeightWin) / ((hv_Row2Part - hv_Row1Part) + 1);
                    hv_FactorColumn = (1.0 * hv_WidthWin) / ((hv_Column2Part - hv_Column1Part) + 1);
                    hv_R1 = ((hv_Row_COPY_INP_TMP - hv_Row1Part) + 0.5) * hv_FactorRow;
                    hv_C1 = ((hv_Column_COPY_INP_TMP - hv_Column1Part) + 0.5) * hv_FactorColumn;
                }
                //
                //display text box depending on text size
                if ((int)(new HTuple(hv_Box.TupleEqual("true"))) != 0)
                {
                    //calculate box extents
                    hv_String_COPY_INP_TMP = (" " + hv_String_COPY_INP_TMP) + " ";
                    hv_Width = new HTuple();
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        HOperatorSet.GetStringExtents(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                            hv_Index), out hv_Ascent, out hv_Descent, out hv_W, out hv_H);
                        hv_Width = hv_Width.TupleConcat(hv_W);
                    }
                    hv_FrameHeight = hv_MaxHeight * (new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                        ));
                    hv_FrameWidth = (((new HTuple(0)).TupleConcat(hv_Width))).TupleMax();
                    hv_R2 = hv_R1 + hv_FrameHeight;
                    hv_C2 = hv_C1 + hv_FrameWidth;
                    //display rectangles
                    HOperatorSet.GetDraw(hv_WindowHandle, out hv_DrawMode);
                    HOperatorSet.SetDraw(hv_WindowHandle, "fill");
                    HOperatorSet.SetColor(hv_WindowHandle, "light gray");
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1 + 3, hv_C1 + 3, hv_R2 + 3, hv_C2 + 3);
                    HOperatorSet.SetColor(hv_WindowHandle, "white");
                    HOperatorSet.DispRectangle1(hv_WindowHandle, hv_R1, hv_C1, hv_R2, hv_C2);
                    HOperatorSet.SetDraw(hv_WindowHandle, hv_DrawMode);
                }
                else if ((int)(new HTuple(hv_Box.TupleNotEqual("false"))) != 0)
                {
                    hv_Exception = "Wrong value of control parameter Box";
                    throw new HalconException(hv_Exception);
                }
                //Write text.
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_String_COPY_INP_TMP.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    hv_CurrentColor = hv_Color_COPY_INP_TMP.TupleSelect(hv_Index % (new HTuple(hv_Color_COPY_INP_TMP.TupleLength()
                        )));
                    if ((int)((new HTuple(hv_CurrentColor.TupleNotEqual(""))).TupleAnd(new HTuple(hv_CurrentColor.TupleNotEqual(
                        "auto")))) != 0)
                    {
                        HOperatorSet.SetColor(hv_WindowHandle, hv_CurrentColor);
                    }
                    else
                    {
                        HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                    }
                    hv_Row_COPY_INP_TMP = hv_R1 + (hv_MaxHeight * hv_Index);
                    HOperatorSet.SetTposition(hv_WindowHandle, hv_Row_COPY_INP_TMP, hv_C1);
                    HOperatorSet.WriteString(hv_WindowHandle, hv_String_COPY_INP_TMP.TupleSelect(
                        hv_Index));
                }
                //reset changed window settings
                HOperatorSet.SetRgb(hv_WindowHandle, hv_Red, hv_Green, hv_Blue);
                HOperatorSet.SetPart(hv_WindowHandle, hv_Row1Part, hv_Column1Part, hv_Row2Part,
                    hv_Column2Part);

                return;
            }


            public GraphicText(GraphicsManager mgr, string name, string text, double x, double y, int fontsize, bool window, bool box, string color)
                : base(mgr, name, color, HALCON_DRAWMODE_FILL, 1, GraphicType.Text)
            {
                _sText = text;
                _dX = x;
                _dY = y;
                _iFontSize = fontsize;
                _bWindow = window;
                _bBox = box;
            }

#if HAL10
            //public static string BaseFontName = "ＭＳ ゴシック";
            public static string BaseFontName = "MS UI Gothic";
            public static string GetFontFormat(string sFontName, int iHeight, int iWidth = -1, bool bUnderLine = false, bool bBold = false)
            {
                return string.Format("-{0}-{1}-{2}-0-{3}-0-{4}-", sFontName, iHeight.ToString(), (iWidth == -1) ? "*" : iWidth.ToString(), bUnderLine ? "1" : "0", bBold ? "1" : "0");
            }
#else
            public static string BaseFontName = "MS UI Gothic";
            public static string GetFontFormat(string sFontName, int iHeight, int iWidth = -1, bool bUnderLine = false, bool bBold = false)
            {
                string s = string.Format("{0}-{1}-{2}", sFontName, (bBold == false) ? "Normal" : "Bold", iHeight.ToString());
                return s;
            }
#endif

            public override void Draw(HWindow window)
            {
                window.SetColor(Color);
                window.SetFont(GetFontFormat(BaseFontName, _iFontSize));
                disp_message(window, _sText, _bWindow ? "window" : "image", _dY, _dX, Color, _bBox ? "true" : "false");
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------------

        List<Graphic> _lstGraphicObjects = new List<Graphic>();
        HWndCtrl _wndctrl = null;
        public bool Visible { get; set; }
        internal bool FontSize { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public GraphicsManager()
        {
            Visible = true;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~GraphicsManager()
        {
            _lstGraphicObjects.RemoveAll(o => true);
        }

        /// <summary>
        /// 再描画
        /// </summary>
        public void Refresh()
        {
            if (_wndctrl == null)
                return;

            _wndctrl.repaint();
        }

        /// <summary>
        /// HWndCtrl設定
        /// </summary>
        /// <param name="view">HWndCtrlオブジェクト</param>
        internal void setViewController(HWndCtrl view)
        {
            _wndctrl = view;
        }

        /// <summary>
        /// 描画（HWndCtrlからのみ呼ばれる）
        /// </summary>
        /// <param name="window">HWindowオブジェクト</param>
        /// <param name="bSystem">システムを描画する</param>
        internal void paintData(HWindow window, bool bSysytem)
        {
            if (!Visible)
                return;

            foreach (Graphic e in _lstGraphicObjects)
            {
                if (!e.Visible)
                    continue;
                if (e.System == bSysytem)
                    e.Draw(window);
            }
        }

        /// <summary>
        /// 名前を指定してオブジェクトを削除する
        /// システムオブジェクトも削除される
        /// </summary>
        /// <param name="mgr">GraphicsManagerオブジェクト</param>
        public void DeleteObject(string name)
        {
            _lstGraphicObjects.RemoveAll(x => x.Name == name);
        }

        /// <summary>
        /// システム以外のすべてのオブジェクトを削除する
        /// </summary>
        public void DeleteAllObjects()
        {
            _lstGraphicObjects.RemoveAll(x => x.System ? false : true);
        }

        /// <summary>
        /// 指定のオブジェクトを最前面に送る
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        public bool TopmostObject(string name)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null)
                return false;

            _lstGraphicObjects.RemoveAll(x => x.Name == name);
            _lstGraphicObjects.Add(obj);

            return true;
        }

        /// <summary>
        /// 指定の名前のオブジェクトの描画をするかどうかを設定する
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="visible">描画するかどうか(boolean)</param>
        public bool SetVisible(string name, bool visible)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null)
                return false;
            obj.Visible = visible;
            return true;
        }

        /// <summary>
        /// 指定の名前のオブジェクトの色を設定する
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="color">描画色</param>
        /// <returns>設定できたかどうか</returns>
        public bool SetColor(string name, string color)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null)
                return false;
            obj.Color = color;
            return true;
        }

        /// <summary>
        /// 指定の名前のオブジェクトの描画モードを設定する
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="drawmode">描画モード</param>
        /// <returns>設定できたかどうか</returns>
        public bool SetDrawMode(string name, string drawmode)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null)
                return false;
            obj.DrawMode = drawmode;
            return true;
        }

        /// <summary>
        /// 指定の名前のオブジェクトをシステム属性にするかどうかを設定する
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="system">システム設定するかどうか(boolean)</param>
        /// <returns>設定できたかどうか</returns>
        public bool SetSystem(string name, bool system)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null)
                return false;
            obj.System = system;
            return true;
        }

        /// <summary>
        /// Rectangle1のオブジェクトを追加する(すでに使用されている名前の場合、前回のオブジェクトを上書きする)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="row1">左上Row</param>
        /// <param name="col1">左上Col</param>
        /// <param name="row2">右下Row</param>
        /// <param name="col2">右下Col</param>
        /// <param name="color">表示色</param>
        /// <param name="drawmode">描画モード</param>
        /// <param name="linewidth">ライン幅</param>
        /// <returns>追加出来たかどうか</returns>
        public bool AddRectangle1(string name, double row1, double col1, double row2, double col2, string color, string drawmode = HALCON_DRAWMODE_MARGIN, int linewidth = 1)
        {
            if (color == "" || drawmode == "" || linewidth < 1)
                return false;

            // 既に存在している場合削除
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj != null)
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obj.Type == GraphicType.Rectangle1)
                {
                    GraphicRectangle1 rect1 = obj as GraphicRectangle1;
                    if (rect1 == null)
                        return false;

                    rect1._dRow1 = row1;
                    rect1._dCol1 = col1;
                    rect1._dRow2 = row2;
                    rect1._dCol2 = col2;
                    rect1.Color = color;
                    rect1.DrawMode = drawmode;
                    rect1.LineWidth = linewidth;
                    _lstGraphicObjects.RemoveAll(x => x.Name == name);
                    _lstGraphicObjects.Add(obj);
                    return true;
                }
            }
            _lstGraphicObjects.Add(new GraphicRectangle1(this, name, row1, col1, row2, col2, color, drawmode, linewidth));
            return true;
        }

        /// <summary>
        /// Rectangle1のオブジェクトを変更する(存在しない、別のオブジェクトタイプの場合エラー)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="row1">左上Row</param>
        /// <param name="col1">左上Col</param>
        /// <param name="row2">右下Row</param>
        /// <param name="col2">右下Col</param>
        /// <returns>変更出来たかどうか</returns>
        public bool SetRectangle1(string name, double row1, double col1, double row2, double col2)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null || obj.Type != GraphicType.Rectangle1)
                return false;

            GraphicRectangle1 rect = (GraphicRectangle1)obj;
            rect._dRow1 = row1;
            rect._dCol1 = col1;
            rect._dRow2 = row2;
            rect._dCol2 = col2;
            return true;
        }

        /// <summary>
        /// Rectangle2のオブジェクトを追加する(すでに使用されている名前の場合、前回のオブジェクトを上書きする)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="row1">中心Row</param>
        /// <param name="col1">中心Col</param>
        /// <param name="phi">角度</param>
        /// <param name="len1">長手方向幅の半分</param>
        /// <param name="len2">短手方向幅の半分</param>
        /// <param name="color">表示色</param>
        /// <param name="drawmode">描画モード</param>
        /// <param name="linewidth">ライン幅</param>
        /// <returns>追加出来たかどうか</returns>
        public bool AddRectangle2(string name, double row, double col, double phi, double len1, double len2, string color, string drawmode = HALCON_DRAWMODE_MARGIN, int linewidth = 1)
        {
            if (color == "" || drawmode == "" || linewidth < 1)
                return false;

            // 既に存在している場合削除
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj != null)
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obj.Type == GraphicType.Rectangle2)
                {
                    GraphicRectangle2 rect = obj as GraphicRectangle2;
                    if (rect == null)
                        return false;

                    rect._dRow = row;
                    rect._dCol = col;
                    rect._dPhi = phi;
                    rect._dLen1 = len1;
                    rect._dLen2 = len2;
                    rect.Color = color;
                    rect.DrawMode = drawmode;
                    rect.LineWidth = linewidth;
                    _lstGraphicObjects.RemoveAll(x => x.Name == name);
                    _lstGraphicObjects.Add(obj);
                    return true;
                }
            }
            _lstGraphicObjects.Add(new GraphicRectangle2(this, name, row, col, phi, len1, len2, color, drawmode, linewidth));
            return true;
        }

        /// <summary>
        /// Rectangle2のオブジェクトを変更する(存在しない、別のオブジェクトタイプの場合エラー)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="row1">中心Row</param>
        /// <param name="col1">中心Col</param>
        /// <param name="phi">角度</param>
        /// <param name="len1">長手方向幅の半分</param>
        /// <param name="len2">短手方向幅の半分</param>
        /// <returns>変更出来たかどうか</returns>
        public bool SetRectangle2(string name, double row, double col, double phi, double len1, double len2)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null || obj.Type != GraphicType.Rectangle2)
                return false;

            GraphicRectangle2 rect = obj as GraphicRectangle2;
            if (rect == null)
                return false;

            rect._dRow = row;
            rect._dCol = col;
            rect._dPhi = phi;
            rect._dLen1 = len1;
            rect._dLen2 = len2;

            return true;
        }

        /// <summary>
        /// Circleのオブジェクトを追加する(すでに使用されている名前の場合、前回のオブジェクトを上書きする)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="row1">中心Row</param>
        /// <param name="col1">中心Col</param>
        /// <param name="rad">半径</param>
        /// <param name="color">表示色</param>
        /// <param name="drawmode">描画モード</param>
        /// <param name="linewidth">ライン幅</param>
        /// <returns>追加出来たかどうか</returns>
        public bool AddCircle(string name, double row, double col, double rad, string color, string drawmode = HALCON_DRAWMODE_MARGIN, int linewidth = 1)
        {
            if (color == "" || drawmode == "" || linewidth < 1)
                return false;

            // 既に存在している場合削除
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj != null)
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obj.Type == GraphicType.Circle)
                {
                    GraphicCircle cir = obj as GraphicCircle;
                    if (cir == null)
                        return false;

                    cir._dRow = row;
                    cir._dCol = col;
                    cir._dRad = rad;
                    cir.Color = color;
                    cir.DrawMode = drawmode;
                    cir.LineWidth = linewidth;
                    _lstGraphicObjects.Add(obj);
                    return true;
                }
            }
            _lstGraphicObjects.Add(new GraphicCircle(this, name, row, col, rad, color, drawmode, linewidth));
            return true;
        }

        /// <summary>
        /// Circleのオブジェクトを変更する(存在しない、別のオブジェクトタイプの場合エラー)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="row1">中心Row</param>
        /// <param name="col1">中心Col</param>
        /// <param name="rad">半径</param>
        /// <returns>追加出来たかどうか</returns>
        public bool SetCircle(string name, double row, double col, double rad)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null || obj.Type != GraphicType.Circle)
                return false;

            GraphicCircle cir = obj as GraphicCircle;
            if (cir == null)
                return false;

            cir._dRow = row;
            cir._dCol = col;
            cir._dRad = rad;

            return true;
        }

        /// <summary>
        /// Ellipseのオブジェクトを追加する(すでに使用されている名前の場合、前回のオブジェクトを上書きする)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="row1">中心Row</param>
        /// <param name="col1">中心Col</param>
        /// <param name="phi">角度</param>
        /// <param name="rad1">長軸方向幅の半分</param>
        /// <param name="rad2">短軸方向幅の半分</param>
        /// <param name="color">表示色</param>
        /// <param name="drawmode">描画モード</param>
        /// <param name="linewidth">ライン幅</param>
        /// <returns>追加出来たかどうか</returns>
        public bool AddEllipse(string name, double row, double col, double phi, double rad1, double rad2, string color, string drawmode = HALCON_DRAWMODE_MARGIN, int linewidth = 1)
        {
            if (color == "" || drawmode == "" || linewidth < 1)
                return false;

            // 既に存在している場合削除
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj != null)
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obj.Type == GraphicType.Ellipse)
                {
                    GraphicEllipse cir = obj as GraphicEllipse;
                    if (cir == null)
                        return false;

                    cir._dRow = row;
                    cir._dCol = col;
                    cir._dPhi = phi;
                    cir._dRad1 = rad1;
                    cir._dRad2 = rad2;
                    cir.Color = color;
                    cir.DrawMode = drawmode;
                    cir.LineWidth = linewidth;
                    _lstGraphicObjects.Add(obj);
                    return true;
                }
            }
            _lstGraphicObjects.Add(new GraphicEllipse(this, name, row, col, phi, rad1, rad2, color, drawmode, linewidth));
            return true;
        }

        /// <summary>
        /// Ellipseのオブジェクトを変更する(存在しない、別のオブジェクトタイプの場合エラー)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="row1">中心Row</param>
        /// <param name="col1">中心Col</param>
        /// <param name="phi">角度</param>
        /// <param name="rad1">長軸方向幅の半分</param>
        /// <param name="rad2">短軸方向幅の半分</param>
        /// <returns>変更出来たかどうか</returns>
        public bool SetEllipse(string name, double row, double col, double phi, double rad1, double rad2)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null || obj.Type != GraphicType.Ellipse)
                return false;

            GraphicEllipse cir = obj as GraphicEllipse;
            if (cir == null)
                return false;

            cir._dRow = row;
            cir._dCol = col;
            cir._dPhi = phi;
            cir._dRad1 = rad1;
            cir._dRad2 = rad2;

            return true;
        }

        /// <summary>
        /// Lineのオブジェクトを追加する(すでに使用されている名前の場合、前回のオブジェクトを上書きする)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="beginrow">開始点Row</param>
        /// <param name="begincol">開始点Col</param>
        /// <param name="endrow">終了点Row</param>
        /// <param name="endcol">終了点Col</param>
        /// <param name="color">表示色</param>
        /// <param name="linewidth">ライン幅</param>
        /// <returns>追加出来たかどうか</returns>
        public bool AddLine(string name, double beginrow, double begincol, double endrow, double endcol, string color, int linewidth = 1)
        {
            if (color == "" || linewidth < 1)
                return false;

            // 既に存在している場合削除
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj != null)
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obj.Type == GraphicType.Line)
                {
                    GraphicLine lin = obj as GraphicLine;
                    if (lin == null)
                        return false;

                    lin._dBeginRow = beginrow;
                    lin._dBeginCol = begincol;
                    lin._dEndRow = endrow;
                    lin._dEndCol = endcol;
                    lin.Color = color;
                    lin.DrawMode = HALCON_DRAWMODE_MARGIN;
                    lin.LineWidth = linewidth;
                    _lstGraphicObjects.Add(obj);
                    return true;
                }
            }
            _lstGraphicObjects.Add(new GraphicLine(this, name, beginrow, begincol, endrow, endcol, color, HALCON_DRAWMODE_MARGIN, linewidth));
            return true;
        }

        /// <summary>
        /// Lineのオブジェクトを変更する(存在しない、別のオブジェクトタイプの場合エラー)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="beginrow">開始点Row</param>
        /// <param name="begincol">開始点Col</param>
        /// <param name="endrow">終了点Row</param>
        /// <param name="endcol">終了点Col</param>
        /// <returns>変更出来たかどうか</returns>
        public bool SetLine(string name, double beginrow, double begincol, double endrow, double endcol)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null || obj.Type != GraphicType.Ellipse)
                return false;

            GraphicLine lin = obj as GraphicLine;
            if (lin == null)
                return false;

            lin._dBeginRow = beginrow;
            lin._dBeginCol = begincol;
            lin._dEndRow = endrow;
            lin._dEndCol = endcol;

            return true;
        }

        /// <summary>
        /// Arcのオブジェクトを追加する(すでに使用されている名前の場合、前回のオブジェクトを上書きする)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="row1">中心Row</param>
        /// <param name="col1">中心Col</param>
        /// <param name="angle">角度</param>
        /// <param name="beginrow">開始点Row</param>
        /// <param name="begincol">開始点Col</param>
        /// <param name="color">表示色</param>
        /// <param name="drawmode">描画モード</param>
        /// <param name="linewidth">ライン幅</param>
        /// <returns>追加出来たかどうか</returns>
        public bool AddArc(string name, double row, double col, double angle, double beginrow, double begincol, string color, string drawmode = HALCON_DRAWMODE_MARGIN, int linewidth = 1)
        {
            if (color == "" || drawmode == "" || linewidth < 1)
                return false;

            // 既に存在している場合削除
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj != null)
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obj.Type == GraphicType.Arc)
                {
                    GraphicArc lin = obj as GraphicArc;
                    if (lin == null)
                        return false;

                    lin._dBeginRow = beginrow;
                    lin._dBeginCol = beginrow;
                    lin._dRow = row;
                    lin._dCol = col;
                    lin._dAngle = angle;
                    lin.Color = color;
                    lin.DrawMode = drawmode;
                    lin.LineWidth = linewidth;
                    _lstGraphicObjects.Add(obj);
                    return true;
                }
            }
            _lstGraphicObjects.Add(new GraphicArc(this, name, beginrow, begincol, angle, row, col, color, drawmode, linewidth));
            return true;
        }

        /// <summary>
        /// Arcのオブジェクトを変更する(存在しない、別のオブジェクトタイプの場合エラー)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="row1">中心Row</param>
        /// <param name="col1">中心Col</param>
        /// <param name="angle">角度</param>
        /// <param name="beginrow">開始点Row</param>
        /// <param name="begincol">開始点Col</param>
        /// <returns>変更出来たかどうか</returns>
        public bool SetArc(string name, double row, double col, double angle, double beginrow, double begincol)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null || obj.Type != GraphicType.Arc)
                return false;

            GraphicArc arc = obj as GraphicArc;
            if (arc == null)
                return false;

            arc._dBeginRow = beginrow;
            arc._dBeginCol = begincol;
            arc._dRow = row;
            arc._dCol = col;
            arc._dAngle = angle;

            return true;
        }

        /// <summary>
        /// Crossのオブジェクトを追加する(すでに使用されている名前の場合、前回のオブジェクトを上書きする)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="row1">中心Row</param>
        /// <param name="col1">中心Col</param>
        /// <param name="size">長さ</param>
        /// <param name="angle">角度</param>
        /// <param name="color">表示色</param>
        /// <param name="linewidth">ライン幅</param>
        /// <returns>追加出来たかどうか</returns>
        public bool AddCross(string name, double row, double col, double size, double angle, string color, int linewidth = 1)
        {
            if (color == "" || linewidth < 1)
                return false;

            // 既に存在している場合削除
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj != null)
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obj.Type == GraphicType.Cross)
                {
                    GraphicCross crs = obj as GraphicCross;
                    if (crs == null)
                        return false;

                    crs._dRow = row;
                    crs._dCol = col;
                    crs._dAngle = angle;
                    crs._dSize = size;
                    crs.Color = color;
                    crs.DrawMode = HALCON_DRAWMODE_MARGIN;
                    crs.LineWidth = linewidth;
                    _lstGraphicObjects.Add(obj);
                    return true;
                }
            }
            _lstGraphicObjects.Add(new GraphicCross(this, name, row, col, size, angle, color, HALCON_DRAWMODE_MARGIN, linewidth));
            return true;
        }

        /// <summary>
        /// Crossのオブジェクトを変更する(存在しない、別のオブジェクトタイプの場合エラー)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="row1">中心Row</param>
        /// <param name="col1">中心Col</param>
        /// <param name="size">長さ</param>
        /// <param name="angle">角度</param>
        /// <returns>変更出来たかどうか</returns>
        public bool SetCross(string name, double row, double col, double size, double angle)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null || obj.Type != GraphicType.Arc)
                return false;

            GraphicCross crs = obj as GraphicCross;
            if (crs == null)
                return false;

            crs._dRow = row;
            crs._dCol = col;
            crs._dAngle = angle;
            crs._dSize = size;

            return true;
        }

        /// <summary>
        /// Objectのオブジェクトを追加する(すでに使用されている名前の場合、前回のオブジェクトを上書きする)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="obj">HObjectオブジェクト</param>
        /// <param name="color">表示色</param>
        /// <param name="linewidth">ライン幅</param>
        /// <returns>追加出来たかどうか</returns>
        public bool AddObject(string name, HObject obj, string color, string drawmode, int linewidth)
        {
            if (color == "" || drawmode == "" || linewidth < 1)
                return false;

            // 既に存在している場合削除
            Graphic obje = _lstGraphicObjects.Find(x => x.Name == name);
            if (obje != null)
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obje.Type == GraphicType.Object)
                {
                    GraphicObject obje2 = obje as GraphicObject;
                    if (obje2 == null)
                        return false;

                    obje2._oObject.Dispose();
                    HOperatorSet.CopyObj(obj, out obje2._oObject, 1, -1);
                    obje2.Color = color;
                    obje2.DrawMode = HALCON_DRAWMODE_MARGIN;
                    obje2.LineWidth = linewidth;
                    _lstGraphicObjects.Add(obje);
                    return true;
                }
            }

            _lstGraphicObjects.Add(new GraphicObject(this, name, obj, color, drawmode, linewidth));
            return true;
        }

        /// <summary>
        /// Objectのオブジェクトを変更する(存在しない、別のオブジェクトタイプの場合エラー)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="obj">HObjectオブジェクト</param>
        /// <returns>変更出来たかどうか</returns>
        public bool SetObject(string name, HObject obj)
        {
            Graphic obje = _lstGraphicObjects.Find(x => x.Name == name);
            if (obje == null || obje.Type != GraphicType.Object)
                return false;

            GraphicObject o = obje as GraphicObject;
            if (o == null)
                return false;

            o._oObject.Dispose();
            HOperatorSet.CopyObj(obj, out o._oObject, 1, -1);

            return true;
        }

        /// <summary>
        /// Textのオブジェクトを追加する(すでに使用されている名前の場合、前回のオブジェクトを上書きする)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="row">テキストの左上Row</param>
        /// <param name="col">テキストの左上Col</param>
        /// <param name="fontsize">フォントの大きさ</param>
        /// <param name="window">座標系をウインドウ似合わせるか、イメージに合わせるか</param>
        /// <param name="box">表示領域の背景にバック描画を行うか(白固定)</param>
        /// <param name="coloe">色</param>
        /// <returns>追加出来たかどうか</returns>
        public bool AddText(string name, string message, double row, double col, int fontsize, bool window, bool box, string color)
        {
            if (color == "")
                return false;

            // 既に存在している場合削除
            Graphic obje = _lstGraphicObjects.Find(o => o.Name == name);

            if (obje != null)
            {
                _lstGraphicObjects.RemoveAll(o => o.Name == name);

                if (obje.Type == GraphicType.Text)
                {
                    GraphicText obje2 = obje as GraphicText;
                    if (obje2 == null)
                        return false;

                    obje2._sText = message;
                    obje2._dX = col;
                    obje2._dY = row;
                    obje2._iFontSize = fontsize;
                    obje2._bWindow = window;
                    obje2._bBox = box;
                    obje2.Color = color;
                    obje2.DrawMode = HALCON_DRAWMODE_MARGIN;
                    obje2.LineWidth = 1;
                    _lstGraphicObjects.Add(obje);
                    return true;
                }
            }
            _lstGraphicObjects.Add(new GraphicText(this, name, message, col, row, fontsize, window, box, color));
            return true;
        }

        /// <summary>
        /// Textのオブジェクトのパラメーターを変更する(存在しない、別のオブジェクトタイプの場合エラー)
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <param name="row">テキストの左上Row</param>
        /// <param name="col">テキストの左上Col</param>
        /// <param name="fontsize">フォントの大きさ</param>
        /// <param name="window">座標系をウインドウ似合わせるか、イメージに合わせるか</param>
        /// <param name="box">表示領域の背景にバック描画を行うか(白固定)</param>
        /// <returns>変更出来たかどうか</returns>
        public bool SetText(string name, string message, double x, double y, int fontsize, bool window, bool box)
        {
            Graphic obje = _lstGraphicObjects.Find(o => o.Name == name);
            if (obje == null || obje.Type != GraphicType.Text)
                return false;

            GraphicText obj = obje as GraphicText;
            if (obje == null)
                return false;

            obj._bBox = box;
            obj._bWindow = window;
            obj._dX = x;
            obj._dY = y;
            obj._iFontSize = fontsize;
            obj._sText = message;

            return true;

        }

        /// <summary>
        /// Graphオブジェクト名の一覧を返す
        /// </summary>
        /// <returns>オブジェクト名のリスト</returns>
        public List<string> GetObjectNameList()
        {
            if (_lstGraphicObjects.Count == 0)
                return new List<string>();
            List<string> lstObjName = new List<string>(_lstGraphicObjects.Count);
            _lstGraphicObjects.ForEach(o => lstObjName.Add(o.Name));
            return lstObjName;
        }

        /// <summary>
        /// オブジェクトが存在するかどうか
        /// </summary>
        /// <param name="name">オブジェクト名</param>
        /// <returns>オブジェクトが存在するかどうか</returns>
        public bool IsObejctExist(string name)
        {
            Graphic obj = _lstGraphicObjects.Find(o => o.Name == name);
            return (obj != null);
        }
    }
}
