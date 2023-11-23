using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HalconDotNet;
using ResultActionDataClassNameSpace;

namespace LineCameraSheetSystem
{
    public partial class uclMiniImage : UserControl
    {
        public uclMiniImage()
        {
            InitializeComponent();
        }
        //流方向　幅方向
        public string TextLength
        {
            get { return labelLength.Text; }
            set { labelLength.Text = value; }
        }
        //系列　zone
        public string TextSpot
        {
            get { return labelSpot.Text; }
            set { labelSpot.Text = value; }
        }
        //NG種　サイズ
        public string TextData
        {
            get { return labelData.Text; }
            set { labelData.Text = value; }
        }
        //ラインインデックス
        public int LineIndex { get; set; }
       
        //何個目のユーザーコントロールか
        public int CountOwn { get; set; }

        public bool SelectItem
        {
            get
            {
                return (BackColor != SystemColors.Control);
            }

            set
            {
                if (value)
                {
                    BackColor = Color.Blue;
                }
                else
                {
                    BackColor = SystemColors.Control;
                }
            }
        }

        private Color _colSelectColor = Color.Blue;
        public Color SelectColor
        {
            get
            {
                return _colSelectColor;
            }

            set
            {
                _colSelectColor = value;    
            }
        }
 
        //各NGデータ
        public ResActionData _miniImgResAcData { set; get; }

        public　void SetImage(HObject hoImg )
        {
            if (hoImg != null)
            {
                if (hoImg != null)
                {
                    HTuple htWidth, htHeight;
                    HOperatorSet.GetImageSize(hoImg, out htWidth, out htHeight);
                    hWindowControl1.ImagePart = new Rectangle(0, 0, htWidth.I, htHeight.I);
                }
                //画面に表示
                HOperatorSet.DispObj(hoImg, hWindowControl1.HalconWindow);
                if (hoImg != null)
                {
                    hoImg.Dispose();
                }
            }
            else
            {
                HOperatorSet.SetTposition(hWindowControl1.HalconWindow, 140, 75);
                HOperatorSet.SetFont(hWindowControl1.HalconWindow, Fujita.HalconMisc.HalconExtFunc.GetFontFormat(Fujita.HalconMisc.HalconExtFunc.BaseFontName, 12));
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "white");
                HOperatorSet.WriteString(hWindowControl1.HalconWindow, "イメージなし");

            }
        }

        public void ClearImage()
        {
            //表示をクリア
            HOperatorSet.ClearWindow( hWindowControl1.HalconWindow);            
        }

        private void ThumbnailDoubleClik(object sender, EventArgs e)
        {
            FromNg1Image();
        }
        public class DoubleClickEventArgs : EventArgs
        {
            public int LineIndex { get; private set; }
            public DoubleClickEventArgs(int index)
            {
                LineIndex = index;
            }

        } 
   
        public delegate void DoubleClickEventHandler(object sender, DoubleClickEventArgs e);
        public event DoubleClickEventHandler OnThumbnailDoubleClick =null;

        private void FromNg1Image()
        {
            if (OnThumbnailDoubleClick != null)
            {
                OnThumbnailDoubleClick(this, new DoubleClickEventArgs(LineIndex));
            }

            //  frmNg1Image _frmNg1Image = new frmNg1Image();
          //  _frmNg1Image.SetNgData(  _miniImgResAcData);
          //  _frmNg1Image.ShowDialog();
           
        }

        //hwindowのクリックイベント
        private void hWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            if (!checkDoubleClick(new PointD(e.X, e.Y)))
            {
                SendClik();
                //return;
            }

            FromNg1Image();
        }

        private DateTime _dtPrevDown = DateTime.Now;
        PointD _ptPrevDown = new PointD();
        private bool checkDoubleClick(PointD pt)
        {
            DateTime dtNowDown = DateTime.Now;
            if ((dtNowDown - _dtPrevDown).TotalMilliseconds > SystemInformation.DoubleClickTime
                || Math.Abs(pt.X - _ptPrevDown.X) > (double)SystemInformation.DoubleClickSize.Width
                || Math.Abs(pt.Y - _ptPrevDown.Y) > (double)SystemInformation.DoubleClickSize.Height)
            {
                _dtPrevDown = dtNowDown;
                _ptPrevDown.X = pt.X;
                _ptPrevDown.Y = pt.Y;
                return false;
            }
            _dtPrevDown = dtNowDown.Subtract(TimeSpan.FromMilliseconds(SystemInformation.DoubleClickTime));
            return true;
        }
        public struct PointD
        {
            double _x;
            double _y;
            public PointD(double x, double y)
            {
                _x = x;
                _y = y;
            }

            public static PointD operator +(PointD pt1, PointD pt2)
            {
                return new PointD(pt1.X + pt2.X, pt1.Y + pt2.Y);
            }

            public static PointD operator -(PointD pt1, PointD pt2)
            {
                return new PointD(pt1.X - pt2.X, pt1.Y - pt2.Y);
            }

            public override bool Equals(object obj)
            {
                if (obj is PointD)
                {
                    return (X == ((PointD)obj).X && Y == ((PointD)obj).Y);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return (int)(X + Y);
            }

            public double X
            {
                get
                {
                    return _x;
                }
                set
                {
                    _x = value;
                }
            }
            public double Y
            {
                get
                {
                    return _y;
                }
                set
                {
                    _y = value;
                }
            }
        }

        private void LabelClik(object sender, EventArgs e)
        {
            SendClik();
        }

        public class ClickEventArgs : EventArgs
        {
          // public int LineIndex { get; private set; }
            public ClickEventArgs(/*int index*/)
            {
               // LineIndex = index;
            }

        }
        public delegate void ClickEventHandler(object sender, ClickEventArgs e);
        public event ClickEventHandler OnThumbnailClick = null;


        private void SendClik()
        {
            if (OnThumbnailClick != null)
            {
                OnThumbnailClick(this, new ClickEventArgs());
            }

        }
    

/*        public void SetPart(int Row1, int Column1, int Row2, int Column2)
        {
            try
            {
                //表示の領域を設定
                HOperatorSet.SetPart(hWindowControl1.HalconID, Row1, Column1, Row2, Column2);
            }
            catch (HOperatorException)
            {

            }
            
        }
*/

    }
}
