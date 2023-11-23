using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Fujita.Misc;

namespace SheetMapping
{
    public partial class uclSheetMap : UserControl, IShortcutClient
    {
        const string DEF_STR_SOKUCYOU = "測長(m)";
        const string DEF_STR_KETTENICHI = "欠点位置";



        /// <summary>
        /// 表示レンジ
        /// </summary>
        private double _dRangeMeter = 1.0;
        /// <summary>
        /// 表示レンジ
        /// </summary>
        public double RangeMeter
        {
            get
            {
                return _dRangeMeter;
            }
            set
            {
                if (value <= 0.0)
                    return;
                double d = _dDispStart;
                _dRangeMeter = value;
                recalcScrollbar();
                scrollTarget(d);
                repaint();
            }
        }

        /// <summary>
        /// シート長延長可能
        /// </summary>
        double _dSheetLengthMeter = 0;
        /// <summary>
        /// シートの幅
        /// </summary>
        public double SheetLengthMeter
        {
            get
            {
                return _dSheetLengthMeter;
            }
            set
            {
                if (value < 0)
                    return;

                _dSheetLengthMeter = value;
                if (_dCurSheetPosMeter > value)
                    _dCurSheetPosMeter = value;

                double dCurPos = _dDispStart;
                // スクロールバーの更新
                recalcScrollbar();

                // 最後にスクロールする
                if (!LockPosition)
                {
                    scrollLast();
                }
                else
                {
                    scrollTarget(dCurPos);
                }
            }
        }

        /// <summary>
        /// ポジションのロック
        /// </summary>
        public bool LockPosition
        {
            get;
            set;
        }

        /// <summary>
        /// スクロールロックボタンの表示非表示
        /// </summary>
        public bool VisibleScrollLock
        {
            get
            {
                return chkScrollLock.Visible;
            }
            set
            {
                chkScrollLock.Visible = value;
            }
        }

        /// <summary>
        /// カレントポジション
        /// </summary>
        double _dCurSheetPosMeter;
        /// <summary>
        /// カレント位置(メートル単位)
        /// </summary>
        public double CurrentPosMeter
        {
            get
            {
                return _dCurSheetPosMeter;
            }
            set
            {
                if (value < 0)
                    return;

                _dCurSheetPosMeter = value;
                repaint();
            }
        }

        List<Color> _lstTipColors = new List<Color>();

        private Color _colDefaultTipColor = Color.Blue;
        /// <summary>
        /// チップのデフォルト色
        /// </summary>
        public Color TipDefaultColor
        {
            get
            {
                return _colDefaultTipColor;
            }
            set
            {
                _colDefaultTipColor = value;
                repaint();
            }
        }

        private Color[] _acolTipColors = new Color[0];
        /// <summary>
        /// チップカラー
        /// </summary>
        public Color[] TipColors
        {
            get
            {
                return _acolTipColors;
            }
            set
            {
                if (value == null)
                    _acolTipColors = new Color[0];
                else
                {
                    _acolTipColors = new Color[value.Length];
                    value.CopyTo(_acolTipColors, 0);
                    repaint();
                }
            }
        }

        private Color _colTipDefault = Color.Magenta;
        public Color TipColorDefault
        {
            get
            {
                return _colTipDefault;
            }
            set
            {
                _colTipDefault = value;
            }
        }


        private double _dSheetWidth = 800;
        public double SheetWidth
        {
            get
            {
                return _dSheetWidth;
            }
            set
            {
                if (value < 0)
                    return;
                _dSheetWidth = value;
            }
        }

        private double[] _adZones = new double[0];
        public double[] Zones
        {
            get
            {
                return _adZones;
            }
            set
            {
                _adZones = new double[value.Length];
                value.CopyTo(_adZones, 0);
                repaint();
            }
        }


        /// <summary>
        /// 現在表示中のオブジェクトの位置情報
        /// </summary>
        class TipDisplayInfo
        {
            private PointF _ptCenter;

            public RectangleF Rect
            {
                get;
                private set;
            }
            public int Index
            {
                get;
                private set;
            }

            public TipDisplayInfo( int iIndex, PointF ptCenter, SizeF szSize)
            {
                Rect = new RectangleF(new PointF(ptCenter.X - szSize.Width / 2f, ptCenter.Y - szSize.Height / 2f), szSize);
                _ptCenter = ptCenter;
                Index = iIndex;
            }

            public bool IsMouseOn(Point pt)
            {
                if (pt.X < Rect.Left || pt.X > Rect.Right)
                    return false;
                if (pt.Y < Rect.Top || pt.Y > Rect.Bottom)
                    return false;
                return true;
            }

            public double Length(Point pt)
            {
                return Math.Sqrt(Length_Pow2(pt));
            }

            public double Length_Pow2(Point pt)
            {
                return (pt.X - _ptCenter.X) * (pt.X - _ptCenter.X) + (pt.Y - _ptCenter.Y) * (pt.Y - _ptCenter.Y);
            }
        }
        List<TipDisplayInfo> _lstTipDispInfos = new List<TipDisplayInfo>();

        // 表示されるチップサイズ
        Size _szTipSize;
        public Size TipSize
        {
            set
            {
                _szTipSize.Width = value.Width;
                _szTipSize.Height = value.Height;
            }
            get
            {
                return _szTipSize;
            }
        }

        // 縦軸の幅
        float _fVertGridRange = 50;
        public float VertGridRange
        {
            get
            {
                return _fVertGridRange;
            }
            set
            {
                if (value < 10 || value > 100)
                    return;

                _fVertGridRange = value;
                repaint();
            }
        }

        Font _fntHeaderDisp = new Font("ＭＳ ゴシック", 10);
        public Font HeaderDispFont
        {
            get
            {
                return _fntHeaderDisp;
            }
            set
            {
                if (_fntHeaderDisp != null)
                    _fntHeaderDisp.Dispose();
                _fntHeaderDisp = value;
                repaint();
            }
        }

        float _fOffsetLeft = 0;
        public float OffsetLeft
        {
            get
            {
                return _fOffsetLeft;
            }
            set
            {
                if (value < 0)
                    return;
                _fOffsetLeft = value;
                repaint();
            }
        }

        float _fOffsetTop = 0;
        public float OffsetTop
        {
            get
            {
                return _fOffsetTop;
            }
            set
            {
                if (value < 0)
                    return;
                _fOffsetTop = value;
                repaint();
            }
        }
        float _fOffsetRight = 0;
        public float OffsetRight
        {
            get
            {
                return _fOffsetRight;
            }
            set
            {
                if (value < 0)
                    return;
                _fOffsetRight = value;
                repaint();
            }
        }
        float _fOffsetBottom = 0;
        public float OffsetBottom
        {
            get
            {
                return _fOffsetBottom;
            }
            set
            {
                if (value < 0)
                    return;
                _fOffsetBottom = value;
                repaint();
            }
        }
        
        /// <summary>
        /// 現在表示されているメートル位置
        /// </summary>
        double _dDispStart;
        /// <summary>
        /// 表示されている開始メートル位置
        /// </summary>
        public double DispStart
        {
            get
            {
                return _dDispStart;
            }
        }
        double _dDispEnd;
        /// <summary>
        /// 表示されている終了メートル位置
        /// </summary>
        public double DispEnd
        {
            get
            {
                return _dDispEnd;
            }
        }

        /// <summary>
        /// ヘッダー部の高さ
        /// </summary>
        int _iHeaderTopHeight = 16;
        public int HeaderTopHeight
        {
            get
            {
                return _iHeaderTopHeight;
            }
            set
            {
                if (value < 16)
                    return;
                _iHeaderTopHeight = value;
                repaint();
            }
        }
        /// <summary>
        /// トップColumnの色
        /// </summary>
        Color _colHeaderTop = SystemColors.Control;
        public Color HeaderTopColor
        {
            set
            {
                if (_colHeaderTop == value)
                    return;
                _colHeaderTop = value;
                if (_brhHeaderTop != null)
                    _brhHeaderTop.Dispose();
                _brhHeaderTop = new SolidBrush(_colHeaderTop);
                repaint();
            }

            get
            {
                return _colHeaderTop;
            }

        }
        SolidBrush _brhHeaderTop = new SolidBrush(SystemColors.Control);

        int _iHeaderSheetInfoHeight = 48;
        public int HeaderSheetInfoHeight
        {
            get
            {
                return _iHeaderSheetInfoHeight;
            }
            set
            {
                if (value < 32)
                    return;
                _iHeaderSheetInfoHeight = value;
                repaint();
            }
        }


        Color _colHeaderSheetInfo = Color.CadetBlue;
        public Color HeaderSheetInfoColor
        {
            set
            {
                if (_colHeaderSheetInfo == value)
                    return;
                _colHeaderSheetInfo = value;
                if (_brhHeaderSheetInfo != null)
                    _brhHeaderSheetInfo.Dispose();
                _brhHeaderSheetInfo = new SolidBrush(_colHeaderSheetInfo);
                repaint();
            }

            get
            {
                return _colHeaderSheetInfo;
            }
        }
        SolidBrush _brhHeaderSheetInfo = new SolidBrush(Color.CadetBlue);

        Color _colBackGroundColor = Color.Black;
        public Color BackGroundColor
        {
            get
            {
                return _colBackGroundColor;
            }
            set
            {
                if (_colBackGroundColor == value)
                    return;
                _colBackGroundColor = value;
                if (_brhBackGround != null)
                    _brhBackGround.Dispose();
                _brhBackGround = new SolidBrush(_colBackGroundColor);
                repaint();
            }
        }
        SolidBrush _brhBackGround = new SolidBrush(Color.Black);

        int _iSokucyouColumnWidth = 120;
        public int SokucyouColumnWidth
        {
            set
            {
                if( value < 60 || value > 200 )
                    return;
                _iSokucyouColumnWidth = value;
                repaint();
            }
            get
            {
                return _iSokucyouColumnWidth;
            }
        }

        Pen _penLine1;
        Pen _penLine2;
        Pen _penCurPosition;
        /// <summary>
        /// カレントポジションの表示色
        /// </summary>
        Color _colCurPosition = Color.Red;
        /// <summary>
        /// カレントポジションの表示色
        /// </summary>
        public Color CurrentPositionColor
        {
            get
            {
                return _colCurPosition;
            }

            set
            {
                if (_colCurPosition == value)
                    return;
                _colCurPosition = value;
                if (_penCurPosition != null)
                    _penCurPosition.Dispose();
                _penCurPosition = new Pen(_colCurPosition, _fCurPosPenSize);
                repaint();
            }
        }

        float _fCurPosPenSize = 1.0f;
        /// <summary>
        /// カレントポジションの線幅
        /// </summary>
        public float CurrentPosLineSize
        {
            get
            {
                return _fCurPosPenSize;
            }

            set
            {
                if (_fCurPosPenSize == value)
                    return;
                _fCurPosPenSize = value;
                if (_penCurPosition != null)
                    _penCurPosition.Dispose();
                _penCurPosition = new Pen(_colCurPosition, _fCurPosPenSize);
                repaint();
            }
        }

        private void repaint()
        {
            drawMap();
            picMap.Refresh();
        }


        /// <summary>
        /// 測長文字列のサイズ
        /// </summary>
        SizeF _szfSokucyou;

        /// <summary>
        /// 欠点位置文字列のサイズ
        /// </summary>
        SizeF _szfKettenIchi;

        /// <summary>
        /// 　バックグラウンドイメージ
        /// </summary>
        Bitmap _bmpBG;
        private void init()
        {
            _bmpBG = new Bitmap(picMap.Size.Width, picMap.Size.Height);
            picMap.Image = _bmpBG;

            Graphics g = Graphics.FromImage(picMap.Image);

            _szfSokucyou = g.MeasureString(DEF_STR_SOKUCYOU, _fntHeaderDisp);
            _szfKettenIchi = g.MeasureString(DEF_STR_KETTENICHI, _fntHeaderDisp);

            _penLine1 = new Pen(Brushes.White, 1.0f);
            _penLine2 = new Pen(Brushes.White, 2.0f);
            _penCurPosition = new Pen(_colCurPosition, _fCurPosPenSize);

            repaint();
        }

        private void term()
        {
            if( _penLine1 != null )
                _penLine1.Dispose();
            
            if( _penLine2 != null )
                _penLine2.Dispose();

            if (_brhBackGround != null)
                _brhBackGround.Dispose();

            if (_brhHeaderSheetInfo != null)
                _brhHeaderSheetInfo.Dispose();

            if (_brhHeaderTop != null)
                _brhHeaderTop.Dispose();

            if (_bmpBG != null)
                _bmpBG.Dispose();
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public uclSheetMap()
        {
            InitializeComponent();

            btnRangeMinus.Click += new EventHandler(btnRange_Click);
            btnRangePlus.Click += new EventHandler(btnRange_Click);

            init();
        }

        double[] _adButtonRange = new double[] { 0.1, 0.2, 0.5, 1, 2, 5, 10, 20, 30, 40, 50, 100, 200, 500 };
        int _iCurRange = 10;

        void btnRange_Click(object sender, EventArgs e)
        {
            if (!this.Visible)
                return;

            Button btn = (Button)sender;
            if (btn.Name == "btnRangeMinus")
            {
                _iCurRange--;
                if (_iCurRange < 0)
                    _iCurRange = 0;
            }
            else
            {
                _iCurRange++;
                if (_iCurRange > _adButtonRange.Length - 1)
                    _iCurRange = _adButtonRange.Length - 1;
            }
            RangeMeter = _adButtonRange[_iCurRange];
        }

        /// <summary>
        /// スクロールバーの位置を再計算して描画しなおす
        /// </summary>
        private void recalcScrollbar()
        {
            vsbScroll.Minimum = 0;
            vsbScroll.Maximum = (int)(_dSheetLengthMeter / _dRangeMeter) + getDispRangeCount() / 2 - 1;
            vsbScroll.LargeChange = getDispRangeCount();
        }

        private void drawMap()
        {
            Graphics g = Graphics.FromImage(_bmpBG);

            clearScreen(g);

            drawHeader(g);

            drawCurPos(g);

            drawTips(g);

        }

        private PointF center2LT(PointF ptCenter, SizeF szSize)
        {
            return new PointF(ptCenter.X - szSize.Width / 2.0f, ptCenter.Y - szSize.Height / 2.0f);
        }

        private PointF left2Lt(PointF ptLeft, SizeF szSize)
        {
            return new PointF(ptLeft.X - szSize.Width, ptLeft.Y - szSize.Height / 2.0f);
        }

        public double getDispStartMeter()
        {
            return _dRangeMeter * vsbScroll.Value;
        }

        private void clearScreen(Graphics g)
        {
            g.FillRectangle(_brhBackGround, 0, 0, _bmpBG.Width, _bmpBG.Height);
        }

        private int getDispRangeCount()
        {
            float fVertLineNow = _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight;
            // 表示のスタート位置に合わせて描画
            // 表示可能なサイズ
            int iCnt = 0;
            do
            {
                fVertLineNow += _fVertGridRange;
                iCnt++;
            } while (fVertLineNow < _bmpBG.Height);

            return iCnt;
        }

        private void drawHeader(Graphics g)
        {
            // トップ表示
            g.FillRectangle(_brhHeaderTop, _fOffsetLeft, _fOffsetTop, _bmpBG.Width-_fOffsetRight-_fOffsetLeft, _iHeaderTopHeight);
            
            // 文字表示
            g.DrawString(DEF_STR_SOKUCYOU, _fntHeaderDisp, Brushes.Black, center2LT(new PointF( _iSokucyouColumnWidth / (float)2.0 + _fOffsetLeft,  _fOffsetTop +  _iHeaderTopHeight  / 2.0f ), _szfSokucyou));

            int iKettenIchiWidth = _bmpBG.Width - _iSokucyouColumnWidth;
            g.DrawString(DEF_STR_KETTENICHI, _fntHeaderDisp, Brushes.Black, center2LT(new PointF( _iSokucyouColumnWidth + iKettenIchiWidth  / 2.0f + _fOffsetLeft, _fOffsetTop + _iHeaderTopHeight / 2.0f), _szfKettenIchi));

            // 網線描画
            _penLine1.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            g.DrawLine(_penLine1, _fOffsetLeft + (float)_iSokucyouColumnWidth, _fOffsetTop, _fOffsetLeft + (float)_iSokucyouColumnWidth, _fOffsetTop + (float)_iHeaderTopHeight); 

            // シート情報表示
            g.FillRectangle(_brhHeaderSheetInfo, getSheetLeft(), _fOffsetTop + _iHeaderTopHeight, _bmpBG.Width - _iSokucyouColumnWidth -_fOffsetLeft - _fOffsetRight, _iHeaderSheetInfoHeight);

            // 横線描画
            g.DrawLine(_penLine2, getSheetLeft(), _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight / 2.0f, _bmpBG.Width - _fOffsetRight, _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight / 2.0f);
            g.DrawLine(_penLine2, getSheetLeft(), _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight, _bmpBG.Width - _fOffsetRight, _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight);
            g.DrawLine(_penLine2, getSheetLeft(), _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight, _fOffsetLeft + _iSokucyouColumnWidth, _bmpBG.Height - _fOffsetBottom);

            // 縦軸の描画
            _penLine1.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            // 表示位置の取得
            _dDispStart = getDispStartMeter();
            _dDispEnd = _dDispStart + _dRangeMeter * getDispRangeCount();

            // テキスト表示
            txtRangeMeter.Text = "ﾚﾝｼﾞ = " + (_dDispEnd - _dDispStart - RangeMeter).ToString("F1") + "(m)";

            float fVertLineNow =  _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight;
            double dRangeMeter = _dDispStart;
            do
            {
                g.DrawLine( _penLine1, _fOffsetLeft + _iSokucyouColumnWidth, fVertLineNow,  _bmpBG.Width - _fOffsetRight, fVertLineNow );
                string sDispRangeMeter = dRangeMeter.ToString("F1");
                g.DrawString( sDispRangeMeter, _fntHeaderDisp, Brushes.White, left2Lt( new PointF(_fOffsetLeft + _iSokucyouColumnWidth - 2, fVertLineNow ), g.MeasureString( sDispRangeMeter, _fntHeaderDisp )));
                dRangeMeter += _dRangeMeter;
                fVertLineNow += _fVertGridRange;
            }while( fVertLineNow < _bmpBG.Height );

            // 文字表示
            string sSheetWidth = string.Format("表面{0}(mm)", _dSheetWidth);
            g.DrawString(sSheetWidth, _fntHeaderDisp, Brushes.Black, center2LT(new PointF(_iSokucyouColumnWidth + iKettenIchiWidth / 2.0f + _fOffsetLeft, _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight / 4.0f), g.MeasureString(sSheetWidth, _fntHeaderDisp)));

            double fSheetSizePix = _bmpBG.Width - _fOffsetLeft - _fOffsetRight - _iSokucyouColumnWidth;
            // ゾーンとｻｲｽﾞの計算
            if (_dSheetWidth != 0.0)
            {
                float[] afZonePix = new float[_adZones.Length];
                for (int i = 0; i < _adZones.Length; i++)
                {
                    afZonePix[i] = (float)(_adZones[i] / _dSheetWidth * fSheetSizePix);
                }

                // ゾーン位置に対して、数字の描画
                float fHorZoneNow = _fOffsetLeft + _iSokucyouColumnWidth;
                for (int i = 0; i <= _adZones.Length; i++)
                {
                    if (i != _adZones.Length)
                        g.DrawString((i + 1).ToString(), _fntHeaderDisp, Brushes.Black, center2LT(new PointF(fHorZoneNow + afZonePix[i] / 2.0f, _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight * 3.0f / 4.0f), g.MeasureString((i + 1).ToString(), _fntHeaderDisp)));
                    if (i != 0)
                        g.DrawLine(_penLine1, fHorZoneNow, _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight, fHorZoneNow, _bmpBG.Height);
                    if (i != _adZones.Length)
                        fHorZoneNow += afZonePix[i];
                }
            }
        }

        private float getSheetLeft()
        {
            return _fOffsetLeft + _iSokucyouColumnWidth;
        }

        private float getSheetTop()
        {
            return _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight;
        }

        private float getSheetRight()
        {
            return _bmpBG.Width - _fOffsetRight;
        }

        private float getSheetBottom()
        {
            return _bmpBG.Height - _fOffsetRight;
        }

        private void drawCurPos(Graphics g)
        {
            if ( _dCurSheetPosMeter < _dDispStart || _dCurSheetPosMeter > _dDispEnd)
                return;

            PointF ptDevice;
            getSheetRealToDevice(0, _dCurSheetPosMeter, out ptDevice);
            g.DrawLine(_penCurPosition, getSheetLeft(), ptDevice.Y, getSheetRight(), ptDevice.Y);
        }

        List<clsSheetTipItem> _lstItems;
        private void drawTips( Graphics g )
        {
            if (_lstItems != null)
                _lstItems.Clear();

            if (SheetTipItemContainer != null)
            {
                SheetTipItemContainer.FetchSheetTipItems(this, _dDispStart * 1000.0, _dDispEnd * 1000.0, out _lstItems);
                _lstTipDispInfos.Clear();

                Dictionary<int, Brush> dicBrshes = new Dictionary<int, Brush>();

                for (int i = 0; i < _lstItems.Count; i++)
                {
                    PointF ptDevice;
                    getSheetRealToDevice(_lstItems[i].X, _lstItems[i].Y * 0.001, out ptDevice);
                    TipDisplayInfo tdi = new TipDisplayInfo(i, ptDevice, _szTipSize);
                    _lstTipDispInfos.Add(tdi);
                    if (!dicBrshes.ContainsKey(_lstItems[i].ColorIndex))
                    {
                        if (_lstItems[i].ColorIndex >= 0 && _lstItems[i].ColorIndex < _acolTipColors.Length)
                            dicBrshes.Add(_lstItems[i].ColorIndex, new SolidBrush(_acolTipColors[_lstItems[i].ColorIndex]));
                        else
                            dicBrshes.Add(_lstItems[i].ColorIndex, new SolidBrush(_colTipDefault));
                    }
                    g.FillRectangle(dicBrshes[_lstItems[i].ColorIndex], tdi.Rect);
                }
            }
        }

        private bool getSheetDeviceToReal(PointF ptDevice, out double dRealX, out double dRealY)
        {
            dRealX = 0.0;
            dRealY = 0.0;

            double fXDev = ptDevice.X - _fOffsetLeft - _iSokucyouColumnWidth;
            double fYDev = ptDevice.Y - _fOffsetTop - _iHeaderTopHeight - _iHeaderSheetInfoHeight;
            double fWidth = _bmpBG.Width - _fOffsetLeft - _iSokucyouColumnWidth - _fOffsetRight;
            double fHeight = _bmpBG.Height - _fOffsetTop - _iHeaderTopHeight - _iHeaderSheetInfoHeight - _fOffsetBottom;

            if (fXDev < 0 || fYDev < 0 || fXDev > fWidth || fYDev > fHeight )
            {
                return false;
            }

            double fWidthRate = fWidth / _dSheetWidth;
            double fHeightRate = _fVertGridRange / (float)_dRangeMeter;

            dRealX = fXDev / fWidthRate;
            dRealY = fYDev / fHeightRate + (vsbScroll.Value * _dRangeMeter);
//            dRealY *= 1000.0;   // メートル->ミリ変換

            return true;
        }

        private bool getSheetRealToDevice(double dRealX, double dRealY, out PointF ptDevice)
        {
            ptDevice = new PointF();

            // ミリ->メートル変換
//            dRealY /= 1000.0;

            // マップ左上
            float fTopX = _fOffsetLeft + _iSokucyouColumnWidth;
            float fTopY = _fOffsetTop + _iHeaderTopHeight + _iHeaderSheetInfoHeight;

            double fWidth = _bmpBG.Width - _fOffsetLeft - _iSokucyouColumnWidth - _fOffsetRight;
            double fHeight = _bmpBG.Height - _fOffsetTop - _iHeaderTopHeight - _iHeaderSheetInfoHeight - _fOffsetBottom;

            double fWidthRate = fWidth / _dSheetWidth;
            double fHeightRate = _fVertGridRange / (double)_dRangeMeter;

            ptDevice.X =(float)( dRealX * fWidthRate ) + fTopX;
            ptDevice.Y = (float)( ( dRealY - _dDispStart ) * fHeightRate) + fTopY;

            if (ptDevice.X >= (picMap.Size.Width - 5))
                ptDevice.X = picMap.Size.Width - 5;

            return true;
        }

        private void scrollLast()
        {
            vsbScroll.Value = vsbScroll.Maximum - vsbScroll.LargeChange + 1;
        }

        private void scrollStart()
        {
            vsbScroll.Value = 0;
        }

        private void scrollTarget(double dMeter)
        {
            // 最寄りの位置を表示する
            int iValue = (int)( dMeter / _dRangeMeter);

            if (iValue > vsbScroll.Maximum - vsbScroll.LargeChange + 1 )
                iValue = vsbScroll.Maximum - vsbScroll.LargeChange + 1;
            else if (iValue < vsbScroll.Minimum)
                iValue = vsbScroll.Minimum;
            vsbScroll.Value = iValue;
        }

        public void DispMeter(double dMeter)
        {
            scrollTarget(dMeter);
        }

        private void UclSheetMapReal_SizeChanged(object sender, EventArgs e)
        {
            if( _bmpBG != null )
            {
                _bmpBG.Dispose();
            }
            _bmpBG = new Bitmap( picMap.Size.Width, picMap.Size.Height );

            drawMap();
            picMap.Image = _bmpBG;
        }

        private void picMap_MouseMove(object sender, MouseEventArgs e)
        {
            // マウスの座標
            double dRealX, dRealY;
            if (getSheetDeviceToReal(new PointF(e.X, e.Y), out dRealX, out dRealY))
            {
                txtXPos.Text = "X = " + dRealX.ToString("F0") + "(mm)";
                txtYPos.Text = "Y = " + dRealY.ToString("F1") + "(m)";
            }
            else
            {
                txtXPos.Text = "";
                txtYPos.Text = "";
            }
        }

        private void picMap_Paint(object sender, PaintEventArgs e)
        {
            drawMap();
        }

        private void vsbScroll_ValueChanged(object sender, EventArgs e)
        {
//            System.Diagnostics.Debug.WriteLine("scrollbar = " + vsbScroll.Value.ToString());
            repaint();
        }

        private void clickEvent(object sender, MouseEventArgs e, TipClickedEventHandler handler)
        {
            List<clsSheetTipItem> lstTarget = new List<clsSheetTipItem>();
            List<int> lstIndex = new List<int>();

            Point pt = new Point(e.X, e.Y);
            for (int i = 0; i < _lstTipDispInfos.Count; i++)
            {
                if (_lstTipDispInfos[i].IsMouseOn(pt))
                {
                    lstTarget.Add(_lstItems[i]);
                    lstIndex.Add(i);
                }
            }

            if (lstTarget.Count == 0)
                return;

            if (handler != null)
            {
                if (lstTarget.Count == 1)
                {
                    handler(this, new TipClickedEventArgs(lstTarget));
                }
                else
                {
                    // 複数ある場合、クリック位置に中心が近い方を先に持ってくる
                    List<Tuple<int, double>> lstTests = new List<Tuple<int, double>>();
                    for (int i = 0; i < lstIndex.Count; i++)
                    {
                        lstTests.Add(new Tuple<int, double>(lstIndex[i], _lstTipDispInfos[lstIndex[i]].Length_Pow2(pt)));
                    }
                    // 距離が近い方からソートする
                    lstTests.Sort((x, y) => (int)(x.Item2 - y.Item2));
                    // ソートに合わせて、リスト再構築
                    List<clsSheetTipItem> lstTarget2 = new List<clsSheetTipItem>();
                    for (int i = 0; i < lstTarget.Count; i++)
                    {
                        lstTarget2.Add(_lstItems[lstTests[i].Item1]);
                    }
                    handler(this, new TipClickedEventArgs(lstTarget2));
                }
            }
        }

        private void picMap_MouseDown(object sender, MouseEventArgs e)
        {
            clickEvent(sender, e, TipClicked);
        }

        private void picMap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            clickEvent(sender, e, TipDoubleClicked);
        }

        public void Repaint()
        {
            repaint();
        }

        private void picMap_MouseLeave(object sender, EventArgs e)  
        {
            txtXPos.Text = "";
            txtYPos.Text = "";
        }

        public ISheetTipItemContainer SheetTipItemContainer
        {
            get;
            set;
        }

        public event TipClickedEventHandler TipClicked = null;
        public event TipClickedEventHandler TipDoubleClicked = null;

        private void chkScrollLock_CheckedChanged(object sender, EventArgs e)
        {
            if (this.Visible && chkScrollLock.Visible)
            {
                LockPosition = chkScrollLock.Checked;
            }
        }

        public bool ProcessShortcutKey(Keys keyData)
        {
            if (shortcutKeyHelper1.PerformClickByKeys(keyData))
            {
                return true;
            }
            return false;
        }
    }

    public class TipClickedEventArgs : EventArgs
    {
        public List<clsSheetTipItem> SheetTipItems
        {
            get;
            private set;
        }

        public TipClickedEventArgs(List<clsSheetTipItem> lstSheetTipItems)
        {
            SheetTipItems = new List<clsSheetTipItem>(lstSheetTipItems);
        }
    }

    public delegate void TipClickedEventHandler( object sender, TipClickedEventArgs e ); 


    public interface ISheetTipItemContainer
    {
        void FetchSheetTipItems(uclSheetMap sender, double dStart, double dEnd, out List<clsSheetTipItem> lstSheetTipItems);
    }

    public class clsSheetTipItem
    {
        public double X
        {
            get; 
            private set;
        }
        public double Y
        {
            get; 
            private set;
        }
        public int ColorIndex
        {
            get;
            private set;
        }
        public object User
        {
            get; 
            private set;
        }

        public clsSheetTipItem(double x, double y, int colorindex, object user)
        {
            X = x;
            Y = y;
            ColorIndex = colorindex;
            User = user;
        }
    }
}
