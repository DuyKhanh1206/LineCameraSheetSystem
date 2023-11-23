using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using HalconDotNet;

namespace ViewROI
{

    public class GraphicsManager
    {
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

        public const string HALCON_DRAWMODE_MARGIN = "margin";
        public const string HALCON_DRAWMODE_FILL = "fill";

        public const string HALCON_COLOR_BLACK = "black";
        public const string HALCON_COLOR_WHITE = "white";
        public const string HALCON_COLOR_RED = "red";
        public const string HALCON_COLOR_GREEN = "green";
        public const string HALCON_COLOR_BLUE = "blue";
        public const string HALCON_COLOR_CYAN = "cyan";
        public const string HALCON_COLOR_MAGENTA = "magenta";
        public const string HALCON_COLOR_YELLOW = "yellow";
        public const string HALCON_COLOR_DIM_GRAY = "dim gray";
        public const string HALCON_COLOR_GRAY = "gray";
        public const string HALCON_COLOR_LIGHT_GRAY = "light gray";
        public const string HALCON_COLOR_MEDIUM_SLATE_BLUE = "medium slate blue";
        public const string HALCON_COLOR_CORAL = "coral";
        public const string HALCON_COLOR_SLATE_BLUE = "slate blue";
        public const string HALCON_COLOR_SPRING_GREEN = "spring green";
        public const string HALCON_COLOR_ORANGE_RED = "orange red";
        public const string HALCON_COLOR_ORANGE = "orange";
        public const string HALCON_COLOR_DARK_OLIVE_GREEN = "dark olive green";
        public const string HALCON_COLOR_PINK = "pink";
        public const string HALCON_COLOR_CADET_BLUE = "cadet blue";



        class Graphic
        {
            public Graphic(string name, string color, string drawmode, int linewidth, GraphicType type)
            {
                Visible = true;
                Name = name;
                Color = color;
                DrawMode = drawmode;
                Type = type;
                LineWidth = linewidth;
            }

            public string Name { get; private set; }
            public GraphicType Type { get; private set; }
            public string Color { get;  internal set; }
            public string DrawMode { get; internal set; }
            public int LineWidth { get; internal set; }

            public bool Visible { get; set; }

            public virtual void Draw(HWindow window) { }
        }

        class GraphicRectangle1 : Graphic
        {
            
            internal double _dRow1;
            internal double _dCol1;
            internal double _dRow2;
            internal double _dCol2;

            public GraphicRectangle1(string name, double row1, double col1, double row2, double col2, string color, string drawmode, int linewidth)
                :base(name,color,drawmode,linewidth, GraphicType.Rectangle1 )
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

            public GraphicRectangle2(string name, double row, double col, double phi, double len1, double len2, string color, string drawmode, int linewidth)
                : base(name, color, drawmode, linewidth, GraphicType.Rectangle2)
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

            public GraphicCircle(string name, double row, double col, double rad, string color, string drawmode, int linewidth)
                : base(name, color, drawmode, linewidth, GraphicType.Circle)
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

            public GraphicCross(string name, double row, double col, double size, double angle, string color, string drawmode, int linewidth)
                : base(name, color, drawmode, linewidth, GraphicType.Cross)
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

            public GraphicEllipse(string name, double row, double col, double phi, double rad1, double rad2, string color, string drawmode, int linewidth)
                : base(name, color, drawmode, linewidth, GraphicType.Ellipse)
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

            public GraphicArc(string name, double row, double col, double rad, double beginrow, double begincol, string color, string drawmode, int linewidth)
                : base(name, color, drawmode, linewidth, GraphicType.Arc)
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
                window.DispArc(_dRow, _dCol, _dAngle, _dBeginRow, _dBeginCol );
            }

        }

        class GraphicLine : Graphic
        {
            internal double _dBeginRow;
            internal double _dBeginCol;
            internal double _dEndRow;
            internal double _dEndCol;

            public GraphicLine(string name, double beginrow, double begincol, double endrow, double endcol, string color, string drawmode, int linewidth)
                : base(name, color, drawmode, linewidth, GraphicType.Line)
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

            public GraphicObject( string name, HObject obj, string color, string drawmode, int linewidth )
                :base( name, color, drawmode, linewidth, GraphicType.Object )
            {
                HOperatorSet.CopyObj( obj, out _oObject, 1, -1 );
            }

            public override void  Draw(HWindow window)
            {
                window.SetColor(Color);
                window.SetDraw(DrawMode );
                window.SetLineWidth(LineWidth);
                window.DispObj(_oObject);
            }
        }

        List<Graphic> _lstGraphicObjects = new List<Graphic>();
        HWndCtrl _wndctrl = null;
        public bool Visible { get; set; }

        public GraphicsManager()
        {
            Visible = true;
        }

        public void Refresh()
        {
            if (_wndctrl == null)
                return;

            _wndctrl.repaint();
        }

        internal void setViewController(HWndCtrl view)
        {
            _wndctrl = view;
        }

        internal void paintData(HWindow window)
        {
            if (!Visible)
                return;

            foreach (Graphic e in _lstGraphicObjects)
            {
                if (!e.Visible)
                    continue;
                e.Draw(window);
            }
        }

        public void DeleteObject(string name)
        {
            _lstGraphicObjects.RemoveAll(x => x.Name == name);
        }

        public void DeleteAllObjects()
        {
            _lstGraphicObjects.RemoveAll(x => true);
        }

        public bool TopmostObject(string name)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null)
                return false;

            _lstGraphicObjects.RemoveAll(x => x.Name == name);
            _lstGraphicObjects.Add(obj);

            return true;
        }

        public bool SetVisible(string name, bool visible)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null)
                return false;
            obj.Visible = visible;
            return true;
        }

        public bool SetColor(string name, string color)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null)
                return false;
            obj.Color = color;
            return true;
        }

        public bool SetDrawMode(string name, string drawmode)
        {
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj == null)
                return false;
            obj.DrawMode = drawmode;
            return true;
        }

        public void AddRectangle1(string name, double row1, double col1, double row2, double col2, string color, string drawmode = HALCON_DRAWMODE_MARGIN, int linewidth = 1)
        {
            // 既に存在している場合削除
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj != null )
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obj.Type == GraphicType.Rectangle1)
                {
                    GraphicRectangle1 rect1 = (GraphicRectangle1)obj;
                    rect1._dRow1 = row1;
                    rect1._dCol1 = col1;
                    rect1._dRow2 = row2;
                    rect1._dCol2 = col2;
                    rect1.Color = color;
                    rect1.DrawMode = drawmode;
                    rect1.LineWidth = linewidth;
                    _lstGraphicObjects.RemoveAll(x => x.Name == name);
                    _lstGraphicObjects.Add(obj);
                    return;
                }
            }
            _lstGraphicObjects.Add(new GraphicRectangle1(name, row1, col1, row2, col2, color, drawmode, linewidth));
        }

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

        public void AddRectangle2(string name, double row, double col, double phi, double len1, double len2, string color, string drawmode = HALCON_DRAWMODE_MARGIN, int linewidth = 1)
        {
            // 既に存在している場合削除
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj != null )
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obj.Type == GraphicType.Rectangle2)
                {
                    GraphicRectangle2 rect = obj as GraphicRectangle2;
                    if (rect == null)
                        return;

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
                    return;
                }
            }
            _lstGraphicObjects.Add(new GraphicRectangle2(name, row, col, phi, len1, len2, color, drawmode, linewidth));
        }

        public bool SetRectangle2( string name, double row, double col, double phi, double len1, double len2 )
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

        public void AddCircle(string name, double row, double col, double rad, string color, string drawmode = HALCON_DRAWMODE_MARGIN, int linewidth = 1)
        {
            // 既に存在している場合削除
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj != null)
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obj.Type == GraphicType.Rectangle2)
                {
                    GraphicCircle cir = obj as GraphicCircle;
                    if (cir == null)
                        return;

                    cir._dRow = row;
                    cir._dCol = col;
                    cir._dRad = rad;
                    cir.Color = color;
                    cir.DrawMode = drawmode;
                    cir.LineWidth = linewidth;
                    _lstGraphicObjects.RemoveAll(x => x.Name == name);
                    _lstGraphicObjects.Add(obj);
                    return;
                }
            }
            _lstGraphicObjects.Add(new GraphicCircle(name, row, col, rad, color, drawmode, linewidth));
        }

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

        public void AddEllipse(string name, double row, double col, double phi, double rad1, double rad2, string color, string drawmode = HALCON_DRAWMODE_MARGIN, int linewidth = 1)
        {
            // 既に存在している場合削除
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj != null)
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obj.Type == GraphicType.Ellipse)
                {
                    GraphicEllipse cir = obj as GraphicEllipse;
                    if (cir == null)
                        return;

                    cir._dRow = row;
                    cir._dCol = col;
                    cir._dPhi = phi;
                    cir._dRad1 = rad1;
                    cir._dRad2 = rad2;
                    cir.Color = color;
                    cir.DrawMode = drawmode;
                    cir.LineWidth = linewidth;
                    _lstGraphicObjects.RemoveAll(x => x.Name == name);
                    _lstGraphicObjects.Add(obj);
                    return;
                }
            }
            _lstGraphicObjects.Add(new GraphicEllipse(name, row, col, phi, rad1, rad2, color, drawmode, linewidth));
        }

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

        public void AddLine(string name, double beginrow, double begincol, double endrow, double endcol, string color, string drawmode = HALCON_DRAWMODE_MARGIN, int linewidth = 1)
        {
            // 既に存在している場合削除
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj != null)
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obj.Type == GraphicType.Line)
                {
                    GraphicLine lin = obj as GraphicLine;
                    if (lin == null)
                        return;

                    lin._dBeginRow = beginrow;
                    lin._dBeginCol = beginrow;
                    lin._dEndRow = endrow;
                    lin._dEndCol = endcol;
                    lin.Color = color;
                    lin.DrawMode = drawmode;
                    lin.LineWidth = linewidth;
                    _lstGraphicObjects.Add(obj);
                    return;
                }
            }
            _lstGraphicObjects.Add(new GraphicLine(name, beginrow, begincol, endrow, endcol, color, drawmode, linewidth));
        }

        public bool SetLine(string name, double beginrow, double begincol, double endrow, double endcol )
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

        public void AddArc(string name, double row, double col, double angle, double beginrow, double begincol, string color, string drawmode = HALCON_DRAWMODE_MARGIN, int linewidth = 1)
        {
            // 既に存在している場合削除
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj != null)
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obj.Type == GraphicType.Arc)
                {
                    GraphicArc lin = obj as GraphicArc;
                    if (lin == null)
                        return;

                    lin._dBeginRow = beginrow;
                    lin._dBeginCol = beginrow;
                    lin._dRow= row;
                    lin._dCol = col;
                    lin._dAngle = angle;
                    lin.Color = color;
                    lin.DrawMode = drawmode;
                    lin.LineWidth = linewidth;
                    _lstGraphicObjects.Add(obj);
                    return;
                }
            }
            _lstGraphicObjects.Add(new GraphicArc(name, beginrow, begincol, angle, row, col, color, drawmode, linewidth));
        }

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

        public void AddCross(string name, double row, double col, double size, double angle, string color, int linewidth = 1)
        {
            // 既に存在している場合削除
            Graphic obj = _lstGraphicObjects.Find(x => x.Name == name);
            if (obj != null)
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obj.Type == GraphicType.Cross)
                {
                    GraphicCross crs = obj as GraphicCross;
                    if (crs == null)
                        return;

                    crs._dRow = row;
                    crs._dCol = col;
                    crs._dAngle = angle;
                    crs._dSize = size;
                    crs.Color = color;
                    crs.DrawMode = HALCON_DRAWMODE_MARGIN;
                    crs.LineWidth = linewidth;
                    _lstGraphicObjects.Add(obj);
                    return;
                }
            }
            _lstGraphicObjects.Add(new GraphicCross(name, row, col, size, angle,color, HALCON_DRAWMODE_MARGIN, linewidth));
        }

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

        public void AddObject(string name, HObject obj, string color, string drawmode, int linewidth)
        {
            // 既に存在している場合削除
            Graphic obje = _lstGraphicObjects.Find(x => x.Name == name);
            if (obje != null)
            {
                _lstGraphicObjects.RemoveAll(x => x.Name == name);

                if (obje.Type == GraphicType.Object)
                {
                    GraphicObject obje2 = obje as GraphicObject;
                    if (obje2 == null)
                        return;

                    obje2._oObject.Dispose();
                    HOperatorSet.CopyObj(obj, out obje2._oObject, 1, -1);
                    obje2.Color = color;
                    obje2.DrawMode = HALCON_DRAWMODE_MARGIN;
                    obje2.LineWidth = linewidth;
                    _lstGraphicObjects.Add(obje);
                    return;
                }
            }

            _lstGraphicObjects.Add(new GraphicObject(name, obj, color, drawmode, linewidth));
        }

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
    }
}
