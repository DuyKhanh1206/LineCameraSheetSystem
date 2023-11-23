using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using HalconDotNet;
using ViewROI;

namespace Fujita.HalconMisc
{
    public class HWindowLayouter
    {
        /// <summary>
        /// HWindowControlリスト
        /// </summary>
        List<HWindowControl> _lstHWindow;
		private int _TopLeftNo = 0;

		/// <summary>
        /// HWindowControlの矩形情報
        /// </summary>
        List<System.Drawing.Rectangle> _lstRectangle;
        /// <summary>
        /// ウインドウ数
        /// </summary>
        private int _windowNum;
        public int WindowNum 
        {
            get
            {
                return _windowNum;
            }
            set
            {
                if( value < 1 )
                {
                    value = 1;
                }
                if (value > 4)
                {
                    value = 4;
                }
                _windowNum = value;
            }
        }

        public HWindowControl this[int index]
        {
            get
            {
                if (index < 0 || index >= _windowNum)
                    throw new ArgumentOutOfRangeException( "index", "インデックスが範囲外です");
                return _lstHWindow[index];
            }
        }

        List<int> _lstWindowIndex = new List<int>();
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="wnd1">左上のウインドウ</param>
        /// <param name="wnd2">右上のウインドウ</param>
        /// <param name="wnd3">左下のウインドウ</param>
        /// <param name="wnd4">右下のウインドウ</param>
        public HWindowLayouter(HWindowControl wnd1, HWindowControl wnd2, HWindowControl wnd3, HWindowControl wnd4)
            : this( wnd1, wnd2, wnd3, wnd4, new List<int>(){0,1,2,3} )
        {
            
        }

        public HWindowLayouter(HWindowControl wnd1, HWindowControl wnd2, HWindowControl wnd3, HWindowControl wnd4, List<int> lstWinNo)
        {
            _windowNum = 4;

            _lstHWindow = new List<HWindowControl>(4);
            _lstRectangle = new List<Rectangle>(4);

            _lstWindowIndex = new List<int>(lstWinNo);

			for (int i = 0; i < _lstWindowIndex.Count;i++)
			{
				if (_lstWindowIndex[i] == 0)
					_TopLeftNo = i;
			}

            _lstHWindow.Add(wnd1);
            _lstRectangle.Add(new Rectangle(wnd1.Location, wnd1.Size));
            _lstHWindow.Add(wnd2);
            _lstRectangle.Add(new Rectangle(wnd2.Location, wnd2.Size));
            _lstHWindow.Add(wnd3);
            _lstRectangle.Add(new Rectangle(wnd3.Location, wnd3.Size));
            _lstHWindow.Add(wnd4);
            _lstRectangle.Add(new Rectangle(wnd4.Location, wnd4.Size));
        }

        Point getLayoutLocation(int iIndex)
        {
            switch (WindowNum)
            {
                case 1:
                    return _lstRectangle[0].Location;
                case 2:
                case 3:
                case 4:
                    return _lstRectangle[iIndex].Location;
                default:
                    return new Point(0, 0);
            }
        }

        Size getLayoutSize(int iIndex)
        {
            switch (WindowNum)
            {
                case 1:
                    return new Size(_lstRectangle[3].Location.X + _lstRectangle[3].Size.Width - _lstRectangle[0].Location.X,
                                    _lstRectangle[3].Location.Y + _lstRectangle[3].Size.Height - _lstRectangle[0].Location.Y);
                case 2:
                    return new Size(_lstRectangle[iIndex].Size.Width,
                                    _lstRectangle[3].Location.Y + _lstRectangle[3].Size.Height - _lstRectangle[iIndex].Location.Y);
                case 3:
                case 4:
                    return _lstRectangle[iIndex].Size;
                default:
                    return new Size(0, 0);
            }
        }

        /// <summary>
        /// ディフォルトレイアウトに変更する
        /// </summary>
        public void LayoutDefault()
        {
            switch (WindowNum)
            {
                case 1:
                    _lstHWindow[0].Visible = true;
                    _lstHWindow[0].Location = getLayoutLocation(_lstWindowIndex[0]);
                    _lstHWindow[0].Size = getLayoutSize(_lstWindowIndex[0]);
                    _lstHWindow[1].Visible = false;
                    _lstHWindow[2].Visible = false;
                    _lstHWindow[3].Visible = false;
                    break;
                case 2:
                    _lstHWindow[0].Visible = true;
                    _lstHWindow[0].Location = getLayoutLocation(_lstWindowIndex[0]);
                    _lstHWindow[0].Size = getLayoutSize(_lstWindowIndex[0]);
                    _lstHWindow[1].Visible = true;
                    _lstHWindow[1].Location = getLayoutLocation(_lstWindowIndex[1]);
                    _lstHWindow[1].Size = getLayoutSize(_lstWindowIndex[1]);
                    _lstHWindow[2].Visible = false;
                    _lstHWindow[3].Visible = false;
                    break;
                case 3:
                    for (int i = 0; i < _lstHWindow.Count; i++)
                    {
                        _lstHWindow[i].Location = getLayoutLocation(_lstWindowIndex[i]);
                        _lstHWindow[i].Size = getLayoutSize(_lstWindowIndex[i]);
                        if( i != 3 )
                            _lstHWindow[i].Visible = true;
                        else
                            _lstHWindow[i].Visible = false;
                    }
                    break;
                case 4:
                    for (int i = 0; i < _lstHWindow.Count; i++)
                    {
						_lstHWindow[i].Location = getLayoutLocation(_lstWindowIndex[i]);
						_lstHWindow[i].Size = getLayoutSize(_lstWindowIndex[i]);
						_lstHWindow[i].Visible = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// １つのウインドウを表示する
        /// </summary>
        /// <param name="iWindowNo">ウインドウ番号</param>
        /// <returns>
        /// true: 成功
        /// false :失敗
        /// </returns>
        public bool LayoutOne(int iWindowNo)
        {
            if ( iWindowNo < 0 || iWindowNo >= WindowNum)
                return false;

            for ( int i = 0 ; i < _lstHWindow.Count; i++ )
            {
                if( i == iWindowNo )
                    _lstHWindow[i].Visible = true;
                else
                    _lstHWindow[i].Visible = false;
            }

			_lstHWindow[iWindowNo].Location = _lstRectangle[_TopLeftNo].Location;
            _lstHWindow[iWindowNo].Size = LayoutOneSize();


            return true;
        }
        public Size GetSizeLayoutOne()
        {
            return LayoutOneSize();
        }
        private Size LayoutOneSize()
        {
            return (new Size(
                _lstRectangle[3].Location.X + _lstRectangle[3].Size.Width - _lstRectangle[0].Location.X,
                _lstRectangle[3].Location.Y + _lstRectangle[3].Size.Height - _lstRectangle[0].Location.Y));
        }
        public Size GetSizeLayoutMulti()
        {
            return _lstRectangle[0].Size;
        }
    }

	public class HWindowLayouter9
	{
		/// <summary>
		/// HWindowControlリスト
		/// </summary>
		List<HWindowControl> _lstHWindow;
		private int _TopLeftNo = 0;

		/// <summary>
		/// HWindowControlの矩形情報
		/// </summary>
		List<System.Drawing.Rectangle> _lstRectangle;
		/// <summary>
		/// ウインドウ数
		/// </summary>
		private int _windowNum;
		public int WindowNum
		{
			get
			{
				return _windowNum;
			}
			set
			{
				if (value < 1)
				{
					value = 1;
				}
				if (value > 9)
				{
					value = 9;
				}
				_windowNum = value;
			}
		}

		public HWindowControl this[int index]
		{
			get
			{
				if (index < 0 || index >= _windowNum)
					throw new ArgumentOutOfRangeException("index", "インデックスが範囲外です");
				return _lstHWindow[index];
			}
		}

		List<int> _lstWindowIndex = new List<int>();
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="wnd1">上左のウインドウ</param>
		/// <param name="wnd2">上中のウインドウ</param>
		/// <param name="wnd3">上右のウインドウ</param>
		/// <param name="wnd4">下左のウインドウ</param>
		/// <param name="wnd5">下中のウインドウ</param>
		/// <param name="wnd6">下右のウインドウ</param>
		public HWindowLayouter9(HWindowControl wnd1, HWindowControl wnd2, HWindowControl wnd3, HWindowControl wnd4, HWindowControl wnd5, HWindowControl wnd6, HWindowControl wnd7, HWindowControl wnd8, HWindowControl wnd9)
			: this(wnd1, wnd2, wnd3, wnd4, wnd5, wnd6, wnd7, wnd8, wnd9, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 })
		{

		}

		public HWindowLayouter9(HWindowControl wnd1, HWindowControl wnd2, HWindowControl wnd3, HWindowControl wnd4, HWindowControl wnd5, HWindowControl wnd6, HWindowControl wnd7, HWindowControl wnd8, HWindowControl wnd9, List<int> lstWinNo)
		{
			_windowNum = 9;

			_lstHWindow = new List<HWindowControl>(9);
			_lstRectangle = new List<Rectangle>(9);

			_lstWindowIndex = new List<int>(lstWinNo);

			for (int i = 0; i < _lstWindowIndex.Count; i++)
			{
				if (_lstWindowIndex[i] == 0)
					_TopLeftNo = i;
			}

			_lstHWindow.Add(wnd1);
			_lstRectangle.Add(new Rectangle(wnd1.Location, wnd1.Size));
			_lstHWindow.Add(wnd2);
			_lstRectangle.Add(new Rectangle(wnd2.Location, wnd2.Size));
			_lstHWindow.Add(wnd3);
			_lstRectangle.Add(new Rectangle(wnd3.Location, wnd3.Size));
			_lstHWindow.Add(wnd4);
			_lstRectangle.Add(new Rectangle(wnd4.Location, wnd4.Size));
			_lstHWindow.Add(wnd5);
			_lstRectangle.Add(new Rectangle(wnd5.Location, wnd5.Size));
			_lstHWindow.Add(wnd6);
			_lstRectangle.Add(new Rectangle(wnd6.Location, wnd6.Size));
			_lstHWindow.Add(wnd7);
			_lstRectangle.Add(new Rectangle(wnd7.Location, wnd7.Size));
			_lstHWindow.Add(wnd8);
			_lstRectangle.Add(new Rectangle(wnd8.Location, wnd8.Size));
			_lstHWindow.Add(wnd9);
			_lstRectangle.Add(new Rectangle(wnd9.Location, wnd9.Size));
		}

		Point getLayoutLocation(int iIndex)
		{
			switch (WindowNum)
			{
				case 1:
					return _lstRectangle[0].Location;
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
					return _lstRectangle[iIndex].Location;
				default:
					return new Point(0, 0);
			}
		}

		Size getLayoutSize(int iIndex)
		{
			switch (WindowNum)
			{
				case 1:
					return new Size(_lstRectangle[3].Location.X + _lstRectangle[3].Size.Width - _lstRectangle[0].Location.X,
									_lstRectangle[3].Location.Y + _lstRectangle[3].Size.Height - _lstRectangle[0].Location.Y);
				case 2:
					return new Size(_lstRectangle[iIndex].Size.Width,
									_lstRectangle[3].Location.Y + _lstRectangle[3].Size.Height - _lstRectangle[iIndex].Location.Y);
				case 3:
				case 4:
				case 5:
				case 6:
				case 7:
				case 8:
				case 9:
					return _lstRectangle[iIndex].Size;
				default:
					return new Size(0, 0);
			}
		}

		/// <summary>
		/// ディフォルトレイアウトに変更する
		/// </summary>
		public void LayoutDefault()
		{
			for (int i = 0; i < WindowNum; i++)
			{
				_lstHWindow[i].Location = getLayoutLocation(_lstWindowIndex[i]);
				_lstHWindow[i].Size = getLayoutSize(_lstWindowIndex[i]);
				_lstHWindow[i].Visible = true;
			}
			for (int i = WindowNum; i < _lstHWindow.Count; i++)
			{
				_lstHWindow[i].Visible = false;
			}
		}

		/// <summary>
		/// １つのウインドウを表示する
		/// </summary>
		/// <param name="iWindowNo">ウインドウ番号</param>
		/// <returns>
		/// true: 成功
		/// false :失敗
		/// </returns>
		public bool LayoutOne(int iWindowNo)
		{
			if (iWindowNo < 0 || iWindowNo >= WindowNum)
				return false;

			for (int i = 0; i < _lstHWindow.Count; i++)
			{
				if (i == iWindowNo)
					_lstHWindow[i].Visible = true;
				else
					_lstHWindow[i].Visible = false;
			}

			_lstHWindow[iWindowNo].Location = _lstRectangle[_TopLeftNo].Location;
            _lstHWindow[iWindowNo].Size = LayoutOneSize();


			return true;
		}
        public Size GetSizeLayoutOne()
        {
            return LayoutOneSize();
        }
        private Size LayoutOneSize()
        {
            return (new Size(
                _lstRectangle[8].Location.X + _lstRectangle[8].Size.Width - _lstRectangle[0].Location.X,
                _lstRectangle[8].Location.Y + _lstRectangle[8].Size.Height - _lstRectangle[0].Location.Y));
        }
        public Size GetSizeLayoutMulti()
        {
            return _lstRectangle[0].Size;
        }
    }
}
