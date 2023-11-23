using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

using System.Drawing;

using HalconDotNet;

using Fujita.Misc;
using System.Runtime.InteropServices;

namespace Fujita.HalconMisc
{
	public enum EHalconOperator
	{
		None,
		Connection,					//分割
		Union1,						//結合
		Invert,							//反転
		Fillup,							//穴埋め
		Convex,						//囲い込み
		SelectShapeMax,			//最大ブロブ選択
		SelectShape,				//ブロブ選択
		ClosingCircle,				//膨張→縮小
		OpeningCircle,				//縮小→膨張
		DilationCircle,				//全体膨張
		ErosionCircle,				//全体縮小
		Rect1Erosion1,			//Rectangle1で縮小
		Rect1Erosion2,			//Rectangle1の高機能で縮小
		Rect2Erosion1,			//Rectangle2で縮小
		Rect2Erosion2,			//Rectangle2の高機能で縮小
		EdgeDilation1,				//ｴｯｼﾞ検出して膨張する(高速：信頼性低い)
		EdgeDilation2,				//ｴｯｼﾞ検出して膨張する(低速：信頼性高い)
	};
	public enum EHalconOperatorArrow
	{
		TopArrow,					//上
		BottomArrow,				//下
		LeftArrow,					//左
		RightArrow,					//右
	}
	public class clsHalconOperator
	{
		/// <summary>
		/// 有効
		/// </summary>
		public bool EnableHO { get; set; }
		/// <summary>
		/// モード
		/// </summary>
		public EHalconOperator ModeHO { get; set; }
		/// <summary>
		/// 方向
		/// </summary>
		public EHalconOperatorArrow ArrowHO { get; set; }
		/// <summary>
		/// 値
		/// </summary>
		public double ValueHO { get; set; }

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public clsHalconOperator()
		{
			EnableHO = false;
			ModeHO = EHalconOperator.None;
			ArrowHO = EHalconOperatorArrow.TopArrow;
			ValueHO = 0.5;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cls"></param>
		public void Copy(clsHalconOperator cls)
		{
			this.EnableHO = cls.EnableHO;
			this.ModeHO = cls.ModeHO;
			this.ArrowHO = cls.ArrowHO;
			this.ValueHO = cls.ValueHO;
		}
	}

    public class TupleRectangle1
    {
        public TupleRectangle1(int row1, int col1, int row2, int col2)
        {
            _Row1 = row1;
            _Column1 = col1;
            _Row2 = row2;
            _Column2 = col2;
        }

        public TupleRectangle1(double row1, double col1, double row2, double col2)
        {
            _Row1 = row1;
            _Column1 = col1;
            _Row2 = row2;
            _Column2 = col2;
        }

        public TupleRectangle1(HTuple row1, HTuple col1, HTuple row2, HTuple col2)
        {
            _Row1 = row1;
            _Column1 = col1;
            _Row2 = row2;
            _Column2 = col2;
        }

        public TupleRectangle1(CRectangle1 rect)
        {
            _Row1 = rect.Row1;
            _Column1 = rect.Col1;
            _Row2 = rect.Row2;
            _Column2 = rect.Col2;
        }

        HTuple _Row1, _Column1, _Row2, _Column2;
        public HTuple Row1
        {
            get
            {
                return _Row1;
            }
            set
            {
                if (value.Type != HTupleType.INTEGER && value.Type != HTupleType.DOUBLE)
                    return;
                if (_Row1.Type == HTupleType.INTEGER)
                    _Row1 = (int)value[0].D;
                else
                    _Row1 = value[0].D;
            }
        }
        public HTuple Column1
        {
            get
            {
                return _Column1;
            }
            set
            {
                if (value.Type != HTupleType.INTEGER && value.Type != HTupleType.DOUBLE)
                    return;
                if (_Column1.Type == HTupleType.INTEGER)
                    _Column1 = (int)value[0].D;
                else
                    _Column1 = value[0].D;
            }
        }
        public HTuple Row2
        {
            get
            {
                return _Row2;
            }
            set
            {
                if (value.Type != HTupleType.INTEGER && value.Type != HTupleType.DOUBLE)
                    return;
                if (_Row2.Type == HTupleType.INTEGER)
                    _Row2 = (int)value[0].D;
                else
                    _Row2 = value[0].D;
            }
        }
        public HTuple Column2
        {
            get
            {
                return _Column2;
            }
            set
            {
                if (value.Type != HTupleType.INTEGER && value.Type != HTupleType.DOUBLE)
                    return;
                if (_Column2.Type == HTupleType.INTEGER)
                    _Column2 = (int)value[0].D;
                else
                    _Column2 = value[0].D;
            }
        }

        public HTuple Width
        {
            get
            {
                return _Column2 - _Column1;
            }
        }

        public HTuple Height
        {
            get
            {
                return _Row2 - _Row1;
            }
        }

        public void GetRectangle(ref CRectangle1 rect)
        {
            rect.Row1 = _Row1.D;
            rect.Col1 = _Column1.D;
            rect.Row2 = _Row2.D;
            rect.Col2 = _Column2.D;
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3}", _Row1.ToString(), _Column1.ToString(), _Row2.ToString(), _Column2.ToString());
        }
    }
    public class TupleRectangle2
    {
        public TupleRectangle2(double row, double col, double phi, double len1, double len2)
        {
            _Row = row;
            _Column = col;
            _Phi = phi;
            _Length1 = len1;
            _Length2 = len2;
        }

        public TupleRectangle2(HTuple row, HTuple col, HTuple phi, HTuple len1, HTuple len2)
        {
            _Row = row;
            _Column = col;
            _Phi = phi;
            _Length1 = len1;
            _Length2 = len2;
        }

        public TupleRectangle2(CRectangle2 rect)
        {
            _Row = rect.Row;
            _Column = rect.Col;
            _Phi = rect.Phi;
            _Length1 = rect.Length1;
            _Length2 = rect.Length2;
        }

        HTuple _Row, _Column, _Phi, _Length1, _Length2;
        public HTuple Row
        {
            get
            {
                return _Row;
            }
            set
            {
                if (value.Type != HTupleType.INTEGER
                    && value.Type != HTupleType.DOUBLE)
                    return;
                _Row = value[0].D;
            }
        }
        public HTuple Column
        {
            get
            {
                return _Column;
            }
            set
            {
                if (value.Type != HTupleType.INTEGER
                    && value.Type != HTupleType.DOUBLE)
                    return;
                _Column = value[0].D;
            }
        }
        public HTuple Phi
        {
            get
            {
                return _Phi;
            }
            set
            {
                if (value.Type != HTupleType.INTEGER
                    && value.Type != HTupleType.DOUBLE)
                    return;
                _Phi = value[0].D;
            }
        }
        public HTuple Length1
        {
            get
            {
                return _Length1;
            }
            set
            {
                if (value.Type != HTupleType.INTEGER
                    && value.Type != HTupleType.DOUBLE)
                    return;
                _Length1 = value[0].D;
            }
        }
        public HTuple Length2
        {
            get
            {
                return _Length2;
            }
            set
            {
                if (value.Type != HTupleType.INTEGER
                    && value.Type != HTupleType.DOUBLE)
                    return;
                _Length2 = value[0].D;
            }
        }

        public void GetRectangle(ref CRectangle2 rect)
        {
            rect.Row = _Row.D;
            rect.Col = _Column.D;
            rect.Phi = _Phi.D;
            rect.Length1 = Length1.D;
            rect.Length2 = Length2.D;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1},{2},{3},{4}", _Row.ToString(), _Column.ToString(), _Phi.ToString(), _Length1.ToString(), _Length2.ToString());
        }
    }

    public class HalconExtFunc
    {
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

        public const string HALCON_DRAW_MARGIN = "margin";
        public const string HALCON_DRAW_FILL = "fill";

        public static string HOperatorExceptionToString(HOperatorException e)
        {
            return e.Message;
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
        public static void Clear( ref HObject obj)
        {
            if (obj != null)
            {
                obj.Dispose();
                obj = null;
            }
        }

        public static void Clear(ref HImage img)
        {
            if (img != null)
            {
                img.Dispose();
                img = null;
            }
        }

        public static bool IsColorImage(HObject img)
        {
            try
            {
                HTuple htChannels;
                HOperatorSet.CountChannels(img, out htChannels);
                return (htChannels.I == 3);
            }
            catch (HOperatorException)
            {
                return false;
            }
        }

        public static bool IsLoadableImageFile(string sPath)
        {
            try
            {
                HObject wk_imgDummy;
                HOperatorSet.ReadImage(out wk_imgDummy, sPath);
                HalconExtFunc.Clear( ref wk_imgDummy);
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public static bool IsImage(HObject img)
        {
            HTuple htClassVal;
            try
            {
                if (img == null)
                    return false;
                HOperatorSet.GetObjClass(img, out htClassVal);
                if (htClassVal.S == "image")
                    return true;
            }
            catch (HOperatorException)
            {
                return false;
            }
            return false;
        }

        public static Size GetImageSize(HObject img)
        {
            try
            {
                if (img == null)
                    return new Size(0, 0);
                HTuple htWidth, htHeight;
                HOperatorSet.GetImageSize(img, out htWidth, out htHeight);
                return new Size(htWidth.I, htHeight.I);
            }
            catch (HOperatorException)
            {
                return new Size(0, 0);
            }
        }

        public static bool RaiseMaskEditor(string sAppDirPath, HObject hoBackImage, HObject hoOrgMaskImage, out HObject hoNewMaskImage, ref int iRetcode, ref string sDescription, System.ComponentModel.ISynchronizeInvoke oSync)
        {
            hoNewMaskImage = null;

            // 背景無いとダメ
            if (hoBackImage == null)
            {
                sDescription = "背景イメージが存在しない";
                return false;
            }


            // フルカラー未対応
            if (HalconExtFunc.IsColorImage(hoBackImage))
            {
                sDescription = "フルカラー未対応";
                return false;
            }

            sAppDirPath += sAppDirPath.DirectoryMark();
            string sMaskEditor = sAppDirPath + "maskeditor.exe";
            string sMaskEditorIni = sAppDirPath + "masked.ini";

            HTuple htWidth, htHeight;

            System.IO.File.Delete(sAppDirPath + "back.bmp");
            System.IO.File.Delete(sAppDirPath + "mask.bmp");

            try
            {
                HOperatorSet.GetImageSize( hoBackImage, out htWidth, out htHeight );
                HOperatorSet.WriteImage(hoBackImage, "bmp", 0, sAppDirPath + "back.bmp");
                if (hoOrgMaskImage != null)
                {
                    HOperatorSet.WriteImage(hoOrgMaskImage, "bmp", 0, sAppDirPath + "mask.bmp");
                }

                IniFileAccess ifa = new IniFileAccess();
                ifa.SetIniString("Bokudozaemon", "bmpfile", sAppDirPath + "back.bmp", sMaskEditorIni);
                ifa.SetIniString("Bokudozaemon", "maskfile", sAppDirPath + "mask.bmp", sMaskEditorIni);
                ifa.SetIniString("Bokudozaemon", "rect", "0,0," + htWidth.I.ToString() + "," + htHeight.I.ToString() , sMaskEditorIni);

                if (!raiseAndWaitApp(sMaskEditor, sMaskEditorIni, oSync))
                {
                    sDescription = "マスクエディタの起動に失敗";
                    return false;
                }

                iRetcode = ifa.GetIniInt("maskeditor", "result", sMaskEditorIni, 0);
                switch (iRetcode)
                {
                    case 1:
                        sDescription = "正常終了";
                        break;
                    case 0:
                        sDescription = "キャンセルされた";
                        break;
                    default:
                        sDescription = "エラー発生";
                        break;
                }

                if (iRetcode == 1)
                {
                    HOperatorSet.ReadImage(out hoNewMaskImage, sAppDirPath + "mask.bmp");
                }
            }
            catch (HOperatorException)
            {
                return false;
            }

            return true;
        }

        private static bool raiseAndWaitApp(string sExePath, string sArgument, System.ComponentModel.ISynchronizeInvoke oSync )
        {
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = sExePath;
            proc.StartInfo.Arguments = sArgument;
            proc.SynchronizingObject = oSync;
            proc.EnableRaisingEvents = true;
			proc.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

            try
            {
                if (!proc.Start())
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            // 終了を待機する
            do
            {
                // Win32 APIを使ってWindowsメッセージをすべて処理する場合のサンプル・コード
                MSG msg = new MSG();
                if (PeekMessage(ref msg, 0, WM_PAINT, WM_PAINT, PeekMsgOption.PM_REMOVE))
                {
                    DispatchMessage(ref msg);
                }
				Application.DoEvents();

            } while (!proc.HasExited);

            return true;
        }

        // Win32 APIのインポート
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool PeekMessage(
          ref MSG lpMsg,
          Int32 hwnd,
          Int32 wMsgFilterMin,
          Int32 wMsgFilterMax,
          PeekMsgOption wRemoveMsg);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool TranslateMessage(ref MSG lpMsg);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern Int32 DispatchMessage(ref MSG lpMsg);
        // メッセージの処理方法オプション
        private enum PeekMsgOption
        {
            PM_NOREMOVE = 0,  // 処理後、メッセージをキューから削除しない
            PM_REMOVE      // 処理後、メッセージをキューから削除する　
        }

        // Windowsメッセージの定義
        private static Int32 WM_PAINT = 0x000F;

        // メッセージ構造体
        [StructLayout(LayoutKind.Sequential)]
        struct MSG
        {
            public Int32 HWnd;    // ウィンドウ・ハンドル
            public Int32 Msg;    // メッセージID
            public Int32 WParam;  // WParamフィールド（メッセージIDごとに違う）
            public Int32 LParam;  // LParamフィールド（メッセージIDごとに違う）
            public Int32 Time;    // 時間
            public POINTAPI Pt;    // カーソル位置（スクリーン座標）
        }
        [StructLayout(LayoutKind.Sequential)]
        struct POINTAPI
        {
            public Int32 x;      // x座標
            public Int32 y;      // y座標
        }
    }

    public static class HalconStringExt
    {
        public static string ToStringIntorEmptyString(this HTuple ht)
        {
            try
            {
                if (ht.Type == HTupleType.INTEGER)
                    return ht[0].I.ToString();
                else
                    return "";
            }
            catch (HalconException)
            {
                return "";
            }
        }

        public static string ToStringDoubleorEmptyString(this HTuple ht)
        {
            try
            {
                if (ht.Type == HTupleType.INTEGER || ht.Type == HTupleType.DOUBLE)
                    return ht[0].D.ToString();
                else
                    return "";
            }
            catch (HalconException)
            {
                return "";
            }
        }

        public static HTuple ToHTupleValueInt(this string s)
        {
            try
            {
                if (s == "")
                    return "";
                else
                    return int.Parse(s);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static HTuple ToHTupleValueDouble(this string s)
        {
            try
            {
                if (s == "")
                    return "";
                else
                    return double.Parse(s);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static bool IsDOUBLE(this HTuple t)
        {
            if (t.Type == HTupleType.DOUBLE || t.Type == HTupleType.INTEGER)
                return true;
            return false;
        }

        public static bool IsINTEGER(this HTuple t)
        {
            if (t.Type == HTupleType.INTEGER)
                return true;
            return false;
        }
    }

	/// <summary>
	/// 
	/// </summary>
	public static class ColorCombList
	{
		/// <summary>
		/// カメラ　カラー種類
		/// </summary>
		/// <param name="sCol1"></param>
		/// <param name="sCol2"></param>
		/// <returns></returns>
		public static System.Data.DataTable GetCameraColorList(string sCol1, string sCol2)
		{
			System.Data.DataTable dtListData = new System.Data.DataTable();
			//Columns
			dtListData.Columns.Add(new System.Data.DataColumn(sCol1, typeof(string)));
			dtListData.Columns.Add(new System.Data.DataColumn(sCol2, typeof(ViewROI.EDispPlane)));
			//Rows
			dtListData.Rows.Add(new object[] { "1.グレー", ViewROI.EDispPlane.Gray });
			dtListData.Rows.Add(new object[] { "2.赤", ViewROI.EDispPlane.Red });
			dtListData.Rows.Add(new object[] { "3.緑", ViewROI.EDispPlane.Green });
			dtListData.Rows.Add(new object[] { "4.青", ViewROI.EDispPlane.Blue });
			dtListData.Rows.Add(new object[] { "5.色相", ViewROI.EDispPlane.Hue });
			dtListData.Rows.Add(new object[] { "6.彩度", ViewROI.EDispPlane.Saturation });
			dtListData.Rows.Add(new object[] { "7.明度", ViewROI.EDispPlane.Intensity });
			return dtListData;
		}
		/// <summary>
		/// 描画　色種類
		/// </summary>
		/// <param name="sCol1"></param>
		/// <param name="sCol2"></param>
		/// <returns></returns>
		public static System.Data.DataTable GetKindColorList(string sCol1, string sCol2)
		{
			System.Data.DataTable dtListData = new System.Data.DataTable();
			//Columns
			dtListData.Columns.Add(new System.Data.DataColumn(sCol1, typeof(string)));
			dtListData.Columns.Add(new System.Data.DataColumn(sCol2, typeof(string)));
			//Rows
			dtListData.Rows.Add(new object[] { "黒", HalconExtFunc.HALCON_COLOR_BLACK });
			dtListData.Rows.Add(new object[] { "白", HalconExtFunc.HALCON_COLOR_WHITE });
			dtListData.Rows.Add(new object[] { "赤", HalconExtFunc.HALCON_COLOR_RED });
			dtListData.Rows.Add(new object[] { "緑", HalconExtFunc.HALCON_COLOR_GREEN });
			dtListData.Rows.Add(new object[] { "青", HalconExtFunc.HALCON_COLOR_BLUE });
			dtListData.Rows.Add(new object[] { "水色", HalconExtFunc.HALCON_COLOR_CYAN });
			dtListData.Rows.Add(new object[] { "マジェンタ", HalconExtFunc.HALCON_COLOR_MAGENTA });
			dtListData.Rows.Add(new object[] { "黄", HalconExtFunc.HALCON_COLOR_YELLOW });
			dtListData.Rows.Add(new object[] { "グレー", HalconExtFunc.HALCON_COLOR_GRAY });
			dtListData.Rows.Add(new object[] { "オレンジ", HalconExtFunc.HALCON_COLOR_ORANGE });
			dtListData.Rows.Add(new object[] { "ピンク", HalconExtFunc.HALCON_COLOR_PINK });


			return dtListData;
		}
	}
	
	/// <summary>
	/// 
	/// </summary>
	public static class RegionCreate
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sCol1"></param>
		/// <param name="sCol2"></param>
		/// <returns></returns>
		public static System.Data.DataTable GetHalconOperatorList(string sCol1, string sCol2)
		{
			System.Data.DataTable dtListData = new System.Data.DataTable();
			//Columns
			dtListData.Columns.Add(new System.Data.DataColumn(sCol1, typeof(string)));
			dtListData.Columns.Add(new System.Data.DataColumn(sCol2, typeof(EHalconOperator)));
			//Rows
			dtListData.Rows.Add(new object[] { "なし", EHalconOperator.None });
			dtListData.Rows.Add(new object[] { "分割", EHalconOperator.Connection });
			dtListData.Rows.Add(new object[] { "結合", EHalconOperator.Union1 });
			dtListData.Rows.Add(new object[] { "反転", EHalconOperator.Invert });
			dtListData.Rows.Add(new object[] { "穴埋め", EHalconOperator.Fillup });
			dtListData.Rows.Add(new object[] { "囲い込み", EHalconOperator.Convex });
			dtListData.Rows.Add(new object[] { "最大選択", EHalconOperator.SelectShapeMax });
			dtListData.Rows.Add(new object[] { "指定選択", EHalconOperator.SelectShape });
			dtListData.Rows.Add(new object[] { "膨張→縮小", EHalconOperator.ClosingCircle });
			dtListData.Rows.Add(new object[] { "縮小→膨張", EHalconOperator.OpeningCircle });
			dtListData.Rows.Add(new object[] { "全体膨張", EHalconOperator.DilationCircle });
			dtListData.Rows.Add(new object[] { "全体縮小", EHalconOperator.ErosionCircle });
			dtListData.Rows.Add(new object[] { "垂直縮小1", EHalconOperator.Rect1Erosion1 });
			dtListData.Rows.Add(new object[] { "垂直縮小2", EHalconOperator.Rect1Erosion2 });
			dtListData.Rows.Add(new object[] { "角度縮小1", EHalconOperator.Rect2Erosion1 });
			dtListData.Rows.Add(new object[] { "角度縮小2", EHalconOperator.Rect2Erosion2 });
			dtListData.Rows.Add(new object[] { "エッジ膨張1", EHalconOperator.EdgeDilation1 });
			dtListData.Rows.Add(new object[] { "エッジ膨張2", EHalconOperator.EdgeDilation2 });
			return dtListData;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sCal1"></param>
		/// <param name="sCol2"></param>
		/// <returns></returns>
		public static System.Data.DataTable GetHalconOperatorArrowList(string sCol1, string sCol2)
		{
			System.Data.DataTable dtListData = new System.Data.DataTable();
			//Columns
			dtListData.Columns.Add(new System.Data.DataColumn(sCol1, typeof(string)));
			dtListData.Columns.Add(new System.Data.DataColumn(sCol2, typeof(EHalconOperatorArrow)));
			//Rows
			dtListData.Rows.Add(new object[] { "上", EHalconOperatorArrow.TopArrow });
			dtListData.Rows.Add(new object[] { "下", EHalconOperatorArrow.BottomArrow });
			dtListData.Rows.Add(new object[] { "左", EHalconOperatorArrow.LeftArrow });
			dtListData.Rows.Add(new object[] { "右", EHalconOperatorArrow.RightArrow });
			return dtListData;
		}
		public static bool GetEnableHalconOperatorArrow(EHalconOperator mode)
		{
			bool ret = true;
			if (mode == EHalconOperator.None
				|| mode == EHalconOperator.Connection
				|| mode == EHalconOperator.Union1
				|| mode == EHalconOperator.Invert
				|| mode == EHalconOperator.Fillup
				|| mode == EHalconOperator.Convex
				|| mode == EHalconOperator.SelectShapeMax
				|| mode == EHalconOperator.SelectShape
				|| mode == EHalconOperator.ClosingCircle
				|| mode == EHalconOperator.OpeningCircle
				|| mode == EHalconOperator.DilationCircle
				|| mode == EHalconOperator.ErosionCircle
				)
			{
				ret = false;
			}
			return ret;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public static bool GetEnableHalconOperatorValue(EHalconOperator mode)
		{
			bool ret = true;
			if (mode == EHalconOperator.None
				|| mode == EHalconOperator.Connection
				|| mode == EHalconOperator.Union1
				|| mode == EHalconOperator.Invert
				|| mode == EHalconOperator.Fillup
				|| mode == EHalconOperator.Convex
				|| mode == EHalconOperator.SelectShapeMax
				)
			{
				ret = false;
			}
			return ret;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="orgReg"></param>
		/// <param name="baseAreaReg"></param>
		/// <param name="newReg"></param>
		/// <param name="cmdList"></param>
		/// <returns></returns>
		public static bool CreateHalconOperator(HObject orgReg, HObject baseAreaReg, out HObject newReg, List<clsHalconOperator> cmdList, bool forceArrow, int imgWidth, int imgHeight)
		{
			const int maxTemp = 50;
			HObject[] hoTemp = new HObject[maxTemp];
			int tmpNo = 0;
			HTuple rows, cols;
            HTuple count;
			bool ret = true;

			HOperatorSet.CopyObj(orgReg, out hoTemp[++tmpNo], 1, -1);

			try
			{
				foreach (clsHalconOperator cmd in cmdList)
				{
					if (cmd.EnableHO == false || cmd.ModeHO == EHalconOperator.None)
						continue;

					switch (cmd.ModeHO)
					{
						case EHalconOperator.Connection:				//分割
							HOperatorSet.Connection(hoTemp[tmpNo], out hoTemp[++tmpNo]);
							break;
						case EHalconOperator.Union1:					//結合
							HOperatorSet.Union1(hoTemp[tmpNo], out hoTemp[++tmpNo]);
							break;
						case EHalconOperator.Invert:						//反転
							HOperatorSet.Complement(hoTemp[tmpNo], out hoTemp[++tmpNo]);
							HOperatorSet.Intersection(hoTemp[tmpNo], baseAreaReg, out hoTemp[++tmpNo]);
							break;
						case EHalconOperator.Fillup:						//穴埋め
							HOperatorSet.FillUp(hoTemp[tmpNo], out hoTemp[++tmpNo]);
							break;
						case EHalconOperator.Convex:					//囲い込み
                            HObject hoBuf1 = null;
                            HObject hoBuf2 = null;
                            HObject hoBuf3 = null;
                            HObject hoBufCopy = null;
                            HObject hoConcat = null;
                            HOperatorSet.CountObj(hoTemp[tmpNo], out count);
                            HOperatorSet.GenEmptyObj(out hoConcat);
                            for (int i = 0; i < count; i++)
                            {
                                if (hoBuf1 != null) { hoBuf1.Dispose(); hoBuf1 = null; }
                                if (hoBuf2 != null) { hoBuf2.Dispose(); hoBuf2 = null; }
                                if (hoBuf3 != null) { hoBuf3.Dispose(); hoBuf3 = null; }

                                HOperatorSet.SelectObj(hoTemp[tmpNo], out hoBuf1, i + 1);
                                HOperatorSet.GetRegionConvex(hoBuf1, out rows, out cols);
                                HOperatorSet.GenContourPolygonXld(out hoBuf2, rows, cols);
                                HOperatorSet.GenRegionContourXld(hoBuf2, out hoBuf3, "filled");
                                if (hoBufCopy != null) { hoBufCopy.Dispose(); hoBufCopy = null; }
                                HOperatorSet.CopyObj(hoConcat, out hoBufCopy, 1, -1);
                                if (hoConcat != null) { hoConcat.Dispose(); hoConcat = null; }
                                HOperatorSet.ConcatObj(hoBufCopy, hoBuf3, out hoConcat);
                            }
                            HOperatorSet.CopyObj(hoConcat, out hoTemp[++tmpNo], 1, -1);
                            if (hoBuf1 != null) { hoBuf1.Dispose(); hoBuf1 = null; }
                            if (hoBuf2 != null) { hoBuf2.Dispose(); hoBuf2 = null; }
                            if (hoBuf3 != null) { hoBuf3.Dispose(); hoBuf3 = null; }
                            if (hoBufCopy != null) { hoBufCopy.Dispose(); hoBufCopy = null; }
                            if (hoConcat != null) { hoConcat.Dispose(); hoConcat = null; }
							break;
						case EHalconOperator.SelectShapeMax:	//最大ブロブ選択
							HOperatorSet.Connection(hoTemp[tmpNo], out hoTemp[++tmpNo]);
							HOperatorSet.SelectShapeStd(hoTemp[tmpNo], out hoTemp[++tmpNo], "max_area", 70);
							break;
						case EHalconOperator.SelectShape:			//ブロブ選択
							HOperatorSet.Connection(hoTemp[tmpNo], out hoTemp[++tmpNo]);
							HOperatorSet.SelectShape(hoTemp[tmpNo], out hoTemp[++tmpNo], "area", "and", cmd.ValueHO, "max");
							break;
						case EHalconOperator.ClosingCircle:			//膨張→縮小
							HOperatorSet.ClosingCircle(hoTemp[tmpNo], out hoTemp[++tmpNo], cmd.ValueHO);
							break;
						case EHalconOperator.OpeningCircle:			//縮小→膨張
							HOperatorSet.OpeningCircle(hoTemp[tmpNo], out hoTemp[++tmpNo], cmd.ValueHO);
							break;
						case EHalconOperator.DilationCircle:			//全体膨張
							HOperatorSet.DilationCircle(hoTemp[tmpNo], out hoTemp[++tmpNo], cmd.ValueHO);
							break;
						case EHalconOperator.ErosionCircle:			//全体縮小
							HOperatorSet.ErosionCircle(hoTemp[tmpNo], out hoTemp[++tmpNo], cmd.ValueHO);
							break;
						case EHalconOperator.Rect1Erosion1:		//Rectangle1で縮小
							RegionCreate.ErosionSmallRect1(hoTemp[tmpNo], out hoTemp[++tmpNo], cmd.ArrowHO, cmd.ValueHO);
							break;
						case EHalconOperator.Rect1Erosion2:		//Rectangle1の高機能で縮小
							RegionCreate.HighSpecErosionSmallRect1(hoTemp[tmpNo], out hoTemp[++tmpNo], cmd.ArrowHO, cmd.ValueHO, imgWidth, imgHeight);
							break;
						case EHalconOperator.Rect2Erosion1:		//Rectangle2で縮小
							RegionCreate.ErosionSmallRect2(hoTemp[tmpNo], out hoTemp[++tmpNo], cmd.ArrowHO, cmd.ValueHO, forceArrow);
							break;
						case EHalconOperator.Rect2Erosion2:		//Rectangle2の高機能で縮小
							RegionCreate.HighSpecErosionSmallRect2(hoTemp[tmpNo], out hoTemp[++tmpNo], cmd.ArrowHO, cmd.ValueHO, forceArrow, imgWidth, imgHeight);
							break;
						case EHalconOperator.EdgeDilation1:			//ｴｯｼﾞ検出して膨張する(高速：信頼性低い)
							RegionCreate.EdgeDilation(0, hoTemp[tmpNo], out hoTemp[++tmpNo], cmd.ArrowHO, cmd.ValueHO, forceArrow, imgWidth, imgHeight);
							break;
						case EHalconOperator.EdgeDilation2:			//ｴｯｼﾞ検出して膨張する(低速：信頼性高い)
							RegionCreate.EdgeDilation(1, hoTemp[tmpNo], out hoTemp[++tmpNo], cmd.ArrowHO, cmd.ValueHO, forceArrow, imgWidth, imgHeight);
							break;
						default:
							break;
					}
					HTuple nonReg;
					HTuple area, row, col;
					HObject tmp = null;
					HOperatorSet.SelectShape(hoTemp[tmpNo], out tmp, "area", "and", 1, "max");
					HOperatorSet.AreaCenter(hoTemp[tmpNo], out area, out row, out col);
					HOperatorSet.CountObj(tmp, out nonReg);
					if (tmp != null)
						tmp.Dispose();
					if (nonReg.I == 0)
						break;
				}
				HOperatorSet.CopyObj(hoTemp[tmpNo], out newReg, 1, -1);
			}
			catch (HOperatorException oe)
			{
				ret = false;
				throw oe;
			}
			finally
			{
				for (int i = 0; i < maxTemp; i++)
				{
					if (hoTemp[i] != null)
						hoTemp[i].Dispose();
				}
			}
			return ret;
		}
		/// <summary>
		/// Rect1による指定方向の縮小
		/// </summary>
		/// <param name="orgReg"></param>
		/// <param name="newReg"></param>
		/// <param name="arrow"></param>
		/// <param name="val"></param>
		public static void ErosionSmallRect1(HObject orgReg, out HObject newReg, EHalconOperatorArrow arrow, double val)
		{
			HObject hoSmallRect1 = null;
			HTuple row1, col1, row2, col2;
			HOperatorSet.SmallestRectangle1(orgReg, out row1, out col1, out row2, out col2);
			switch (arrow)
			{
				case EHalconOperatorArrow.TopArrow:			//上　縮小
					HOperatorSet.GenRectangle1(out hoSmallRect1, row1 + val, col1, row2, col2);
					break;
				case EHalconOperatorArrow.BottomArrow:	//下　縮小
					HOperatorSet.GenRectangle1(out hoSmallRect1, row1, col1, row2 - val, col2);
					break;
				case EHalconOperatorArrow.LeftArrow:			//左　縮小
					HOperatorSet.GenRectangle1(out hoSmallRect1, row1, col1 + val, row2, col2);
					break;
				case EHalconOperatorArrow.RightArrow:		//右　縮小
					HOperatorSet.GenRectangle1(out hoSmallRect1, row1, col1, row2, col2 - val);
					break;
			}
			HOperatorSet.Intersection(orgReg, hoSmallRect1, out newReg);
			hoSmallRect1.Dispose();
		}
		/// <summary>
		/// Rect2による指定方向の縮小（高機能）
		/// </summary>
		/// <param name="orgReg"></param>
		/// <param name="newReg"></param>
		/// <param name="arrow"></param>
		/// <param name="val"></param>
		public static void HighSpecErosionSmallRect1(HObject orgReg, out HObject newReg, EHalconOperatorArrow arrow, double val, int imgWidth, int imgHeight)
		{
			const int maxTemp = 10;
			HObject[] hoTemp = new HObject[maxTemp];
			int tmpNo = 0;
			HObject hoBinImage = null;
			HObject hoRegPolygon = null;
			HObject hoMoveReg = null;
			HObject hoConnect = null;
			HTuple row1, col1, row2, col2;
			HTuple rad0, rad90, rad180, rad270;

			try
			{
				newReg = null;

				HOperatorSet.DilationCircle(orgReg, out hoTemp[++tmpNo], val);
				HOperatorSet.SmallestRectangle1(hoTemp[tmpNo], out row1, out col1, out row2, out col2);
				HOperatorSet.RegionToBin(hoTemp[tmpNo], out hoBinImage, 255, 0, imgWidth, imgHeight);

				HOperatorSet.TupleRad(0.0, out rad0);
				HOperatorSet.TupleRad(90.0, out rad90);
				HOperatorSet.TupleRad(180.0, out rad180);
				HOperatorSet.TupleRad(270.0, out rad270);

				int pich = 1;
				int pointCnt = 0;
				double len = 0.0;
				switch (arrow)
				{
					case EHalconOperatorArrow.TopArrow:			//上　縮小
						len = col2.D - col1.D;
						break;
					case EHalconOperatorArrow.BottomArrow:	//下　縮小
						len = col2.D - col1.D;
						break;
					case EHalconOperatorArrow.LeftArrow:			//左　縮小
						len = row2.D - row1.D;
						break;
					case EHalconOperatorArrow.RightArrow:		//右　縮小
						len = row2.D - row1.D;
						break;
				}
				pointCnt = (int)(len / pich);

				HTuple hvRows, hvCols;
				hvRows = new HTuple();
				hvCols = new HTuple();
				int lenOffset = (int)val + 10;
				double measLen2 = 5.0;
				double measLen1 = 0.0;
				double newRow = 0.0;
				double newCol = 0.0;
				double rad = 0.0;
				for (int i = 0; i <= pointCnt; i++)
				{
					HTuple measHandle = new HTuple();
					switch (arrow)
					{
						case EHalconOperatorArrow.TopArrow:			//上　縮小
							newRow = (row2.D + row1.D) / 2.0;
							newCol = col1.D + (pich * i);
							measLen1 = (row2.D - row1.D) / 2.0 + lenOffset;
							rad = rad270.D;
							break;
						case EHalconOperatorArrow.BottomArrow:	//下　縮小
							newRow = (row2.D + row1.D) / 2.0;
							newCol = col1.D + (pich * i);
							measLen1 = (row2.D - row1.D) / 2.0 + lenOffset;
							rad = rad90.D;
							break;
						case EHalconOperatorArrow.LeftArrow:			//左　縮小
							newRow = row1.D + (pich * i);
							newCol = (col2.D + col1.D) / 2.0;
							measLen1 = (col2.D - col1.D) / 2.0 + lenOffset;
							rad = rad0.D;
							break;
						case EHalconOperatorArrow.RightArrow:		//右　縮小
							newRow = row1.D + (pich * i);
							newCol = (col2.D + col1.D) / 2.0;
							measLen1 = (col2.D - col1.D) / 2.0 + lenOffset;
							rad = rad180.D;
							break;
					}
					if (newRow < 0.0 || imgHeight < newRow)
						continue;
					if (newCol < 0.0 || imgWidth < newCol)
						continue;
					HOperatorSet.GenMeasureRectangle2(newRow, newCol, rad, measLen1, measLen2, imgWidth, imgHeight, "nearest_neighbor", out measHandle);

					HTuple rowEdge, colEdge, amplitude, distance;
					HOperatorSet.MeasurePos(hoBinImage, measHandle, 1, 40, "positive", "first", out rowEdge, out colEdge, out amplitude, out distance);
					if (rowEdge.Length != 0)
					{
						HOperatorSet.TupleConcat(hvRows, rowEdge, out hvRows);
						HOperatorSet.TupleConcat(hvCols, colEdge, out hvCols);
					}
					HOperatorSet.CloseMeasure(measHandle);
				}
				HOperatorSet.GenRegionPolygon(out hoRegPolygon, hvRows, hvCols);

				HOperatorSet.CopyObj(orgReg, out hoConnect, 1, -1);
				for (double mc = 0.0; mc <= val * 2.0; mc += 0.5)
				{
					if (hoMoveReg != null)
						hoMoveReg.Dispose();
					double r = 0.0;
					double c = 0.0;
					switch (arrow)
					{
						case EHalconOperatorArrow.TopArrow:			//上　縮小
							r = mc;
							break;
						case EHalconOperatorArrow.BottomArrow:	//下　縮小
							r = -mc;
							break;
						case EHalconOperatorArrow.LeftArrow:			//左　縮小
							c = mc;
							break;
						case EHalconOperatorArrow.RightArrow:		//右　縮小
							c = -mc;
							break;
					}
					if (hoMoveReg != null)
						hoMoveReg.Dispose();
					HOperatorSet.MoveRegion(hoRegPolygon, out hoMoveReg, r, c);

					if (newReg != null)
						newReg.Dispose();
					HOperatorSet.Difference(hoConnect, hoMoveReg, out newReg);
					hoConnect.Dispose();
					HOperatorSet.CopyObj(newReg, out hoConnect, 1, -1);
				}
			}
			catch (HOperatorException oe)
			{
				throw oe;
			}
			finally
			{
				for (int i = 0; i < maxTemp; i++)
				{
					if (hoTemp[i] != null)
						hoTemp[i].Dispose();

					if (hoBinImage != null)
						hoBinImage.Dispose();
					if (hoRegPolygon != null)
						hoRegPolygon.Dispose();
					if (hoMoveReg != null)
						hoMoveReg.Dispose();
					if (hoConnect != null)
						hoConnect.Dispose();
				}
			}
		}
		/// <summary>
		/// Rect2による指定方向の縮小
		/// </summary>
		/// <param name="orgReg"></param>
		/// <param name="newReg"></param>
		/// <param name="arrow"></param>
		/// <param name="val"></param>
		/// <param name="forceArrow"></param>
		public static void ErosionSmallRect2(HObject orgReg, out HObject newReg, EHalconOperatorArrow arrow, double val, bool forceArrow)
		{
			HObject hoSmallRect2 = null;
			HTuple row, col, len1, len2;
			HTuple hv_Phi;
			HTuple rad90;
			HTuple rad45Plus, rad45Minus;

			double hoseiPhi;
			hoseiPhi = GetRegionRect2Arrow(orgReg, forceArrow);

			HOperatorSet.SmallestRectangle2(orgReg, out row, out col, out hv_Phi, out len1, out len2);

			HOperatorSet.TupleRad(90.0, out rad90);
			HOperatorSet.TupleRad(45.0, out rad45Plus);
			rad45Minus = rad45Plus.D * -1.0;

			double diffPhi;
			diffPhi = Math.Abs(hoseiPhi) - Math.Abs(hv_Phi.D);
			double hoseiLen1, hoseiLen2;
			hoseiLen1 = len1.D;
			hoseiLen2 = len2.D;
			if (diffPhi < rad45Minus.D || rad45Plus.D < diffPhi)
			{
				hoseiLen1 = len2.D;
				hoseiLen2 = len1.D;
			}

			HTuple sin, cos;
			HOperatorSet.TupleSin(rad90.D - hoseiPhi, out sin);
			HOperatorSet.TupleCos(rad90.D - hoseiPhi, out cos);

			double newRow, newCol;
			newRow = val * sin.D;
			newCol = val * cos.D;

			double newVal = val / 2.0;
			switch (arrow)
			{
				case EHalconOperatorArrow.TopArrow:			//上　縮小
					HOperatorSet.GenRectangle2(out hoSmallRect2, row + newCol, col - newRow, hoseiPhi, hoseiLen1 - newVal, hoseiLen2);
					break;
				case EHalconOperatorArrow.BottomArrow:	//下　縮小
					HOperatorSet.GenRectangle2(out hoSmallRect2, row - newCol, col + newRow, hoseiPhi, hoseiLen1 - newVal, hoseiLen2);
					break;
				case EHalconOperatorArrow.LeftArrow:			//左　縮小
					HOperatorSet.GenRectangle2(out hoSmallRect2, row + newRow, col + newCol, hoseiPhi, hoseiLen1, hoseiLen2 - newVal);
					break;
				case EHalconOperatorArrow.RightArrow:		//右　縮小
					HOperatorSet.GenRectangle2(out hoSmallRect2, row - newRow, col - newCol, hoseiPhi, hoseiLen1, hoseiLen2 - newVal);
					break;
			}
			HOperatorSet.Intersection(orgReg, hoSmallRect2, out newReg);
			hoSmallRect2.Dispose();
		}
		/// <summary>
		/// Rect2による指定方向の縮小（高機能）
		/// </summary>
		/// <param name="orgReg"></param>
		/// <param name="newReg"></param>
		/// <param name="arrow"></param>
		/// <param name="val"></param>
		public static void HighSpecErosionSmallRect2(HObject orgReg, out HObject newReg, EHalconOperatorArrow arrow, double val, bool forceArrow, int imgWidth, int imgHeight)
		{
			const int maxTemp = 10;
			HObject[] hoTemp = new HObject[maxTemp];
			int tmpNo = 0;
			HObject hoBinImage = null;
			HTuple row, col, len1, len2;
			HTuple hv_Phi;
			HTuple rad90;
			HTuple rad45Plus, rad45Minus;

			try
			{
				newReg = null;

				double hoseiPhi;
				hoseiPhi = GetRegionRect2Arrow(orgReg, forceArrow);

				HOperatorSet.SmallestRectangle2(orgReg, out row, out col, out hv_Phi, out len1, out len2);
				HOperatorSet.RegionToBin(orgReg, out hoBinImage, 255, 0, imgWidth, imgHeight);

				HOperatorSet.TupleRad(90.0, out rad90);
				HOperatorSet.TupleRad(45.0, out rad45Plus);
				rad45Minus = rad45Plus.D * -1.0;

				double diffPhi;
				diffPhi = Math.Abs(hoseiPhi) - Math.Abs(hv_Phi.D);
				double hoseiLen1, hoseiLen2;
				hoseiLen1 = len1.D;
				hoseiLen2 = len2.D;
				if (diffPhi < rad45Minus.D || rad45Plus.D < diffPhi)
				{
					hoseiLen1 = len2.D;
					hoseiLen2 = len1.D;
				}

				HTuple sin, cos;
				HOperatorSet.TupleSin(rad90.D - hoseiPhi, out sin);
				HOperatorSet.TupleCos(rad90.D - hoseiPhi, out cos);

				int pich = 1;
				int pointCnt = 0;
				double len = 0.0;
				switch (arrow)
				{
					case EHalconOperatorArrow.TopArrow:			//上　縮小
						pointCnt = (int)((hoseiLen2 * 2.0) / (double)pich);
						break;
					case EHalconOperatorArrow.BottomArrow:	//下　縮小
						pointCnt = (int)((hoseiLen2 * 2.0) / (double)pich);
						break;
					case EHalconOperatorArrow.LeftArrow:			//左　縮小
						pointCnt = (int)((hoseiLen1 * 2.0) / (double)pich);
						break;
					case EHalconOperatorArrow.RightArrow:		//右　縮小
						pointCnt = (int)((hoseiLen1 * 2.0) / (double)pich);
						break;
				}

				HTuple hvRows, hvCols;
				hvRows = new HTuple();
				hvCols = new HTuple();
				int lenOffset = 10;
				double measLen2 = 5.0;
				double measLen1 = 0.0;
				double newRow = 0.0;
				double newCol = 0.0;
				double rad = 0.0;
				for (int i = 0; i <= pointCnt; i++)
				{
					HTuple measHandle = new HTuple();
					switch (arrow)
					{
						case EHalconOperatorArrow.TopArrow:			//上　縮小
							len = (i != pointCnt) ? len2.D - (pich * i) : len2.D * -1.0;
							newRow = row.D + (len * sin.D);
							newCol = col.D + (len * cos.D);
							measLen1 = (int)(hoseiLen1 * 2.0) + lenOffset;
							rad = hoseiPhi + (rad90.D * 2.0);
							break;
						case EHalconOperatorArrow.BottomArrow:	//下　縮小
							len = (i != pointCnt) ? len2.D - (pich * i) : len2.D * -1.0;
							newRow = row.D + (len * sin.D);
							newCol = col.D + (len * cos.D);
							measLen1 = (int)(hoseiLen1 * 2.0) + lenOffset;
							rad = hoseiPhi;
							break;
						case EHalconOperatorArrow.LeftArrow:			//左　縮小
							len = (i != pointCnt) ? len1.D - (pich * i) : len1.D * -1.0;
							newRow = row.D - (len * cos.D);
							newCol = col.D + (len * sin.D);
							measLen1 = (int)(hoseiLen2 * 2.0) + lenOffset;
							rad = hoseiPhi - rad90.D;
							break;
						case EHalconOperatorArrow.RightArrow:		//右　縮小
							len = (i != pointCnt) ? len1.D - (pich * i) : len1.D * -1.0;
							newRow = row.D - (len * cos.D);
							newCol = col.D + (len * sin.D);
							measLen1 = (int)(hoseiLen2 * 2.0) + lenOffset;
							rad = hoseiPhi + rad90.D;
							break;
					}
					if (newRow < 0.0 || imgHeight < newRow)
						continue;
					if (newCol < 0.0 || imgWidth < newCol)
						continue;
					HOperatorSet.GenMeasureRectangle2(newRow, newCol, rad, measLen1, measLen2, imgWidth, imgHeight, "nearest_neighbor", out measHandle);

					HTuple rowEdge, colEdge, amplitude, distance;
					HOperatorSet.MeasurePos(hoBinImage, measHandle, 1, 40, "positive", "first", out rowEdge, out colEdge, out amplitude, out distance);
					if (rowEdge.Length != 0)
					{
						HOperatorSet.TupleConcat(hvRows, rowEdge, out hvRows);
						HOperatorSet.TupleConcat(hvCols, colEdge, out hvCols);
					}
					HOperatorSet.CloseMeasure(measHandle);
				}
				HOperatorSet.GenRegionPolygon(out hoTemp[++tmpNo], hvRows, hvCols);

				HOperatorSet.DilationCircle(hoTemp[tmpNo], out hoTemp[++tmpNo], val);
				HOperatorSet.Difference(orgReg, hoTemp[tmpNo], out newReg);
			}
			catch (HOperatorException oe)
			{
				throw oe;
			}
			finally
			{
				for (int i = 0; i < maxTemp; i++)
				{
					if (hoTemp[i] != null)
						hoTemp[i].Dispose();

					if (hoBinImage != null)
						hoBinImage.Dispose();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="orgReg"></param>
		/// <param name="newReg"></param>
		/// <param name="arrow"></param>
		/// <param name="val"></param>
		/// <param name="forceArrow"></param>
		/// <param name="imgWidth"></param>
		/// <param name="imgHeight"></param>
		public static void EdgeDilation(int mode, HObject orgReg, out HObject newReg, EHalconOperatorArrow arrow, double val, bool forceArrow, int imgWidth, int imgHeight)
		{
			const int maxTemp = 10;
			HObject[] hoTemp = new HObject[maxTemp];
			int tmpNo = 0;
			HObject hoBinImage = null;
			HTuple row, col, len1, len2;
			HTuple hv_Phi;
			HTuple rad90;
			HTuple rad45Plus, rad45Minus;

			try
			{
				newReg = null;

				double hoseiPhi;
				hoseiPhi = GetRegionRect2Arrow(orgReg, forceArrow);

				HOperatorSet.SmallestRectangle2(orgReg, out row, out col, out hv_Phi, out len1, out len2);
				HOperatorSet.RegionToBin(orgReg, out hoBinImage, 255, 0, imgWidth, imgHeight);

				HOperatorSet.TupleRad(90.0, out rad90);
				HOperatorSet.TupleRad(45.0, out rad45Plus);
				rad45Minus = rad45Plus.D * -1.0;

				double diffPhi;
				diffPhi = Math.Abs(hoseiPhi) - Math.Abs(hv_Phi.D);
				double hoseiLen1, hoseiLen2;
				hoseiLen1 = len1.D;
				hoseiLen2 = len2.D;
				if (diffPhi < rad45Minus.D || rad45Plus.D < diffPhi)
				{
					hoseiLen1 = len2.D;
					hoseiLen2 = len1.D;
				}

				HTuple sin, cos;
				HOperatorSet.TupleSin(rad90.D - hoseiPhi, out sin);
				HOperatorSet.TupleCos(rad90.D - hoseiPhi, out cos);

				int pich = 1;
				int pointCnt = 0;
				double len = 0.0;
				switch (arrow)
				{
					case EHalconOperatorArrow.TopArrow:			//上　縮小
						pointCnt = (int)((hoseiLen2 * 2.0) / (double)pich);
						break;
					case EHalconOperatorArrow.BottomArrow:	//下　縮小
						pointCnt = (int)((hoseiLen2 * 2.0) / (double)pich);
						break;
					case EHalconOperatorArrow.LeftArrow:			//左　縮小
						pointCnt = (int)((hoseiLen1 * 2.0) / (double)pich);
						break;
					case EHalconOperatorArrow.RightArrow:		//右　縮小
						pointCnt = (int)((hoseiLen1 * 2.0) / (double)pich);
						break;
				}

				HTuple hvRows, hvCols;
				hvRows = new HTuple();
				hvCols = new HTuple();
				int lenOffset = 10;
				double measLen2 = 5.0;
				double measLen1 = 0.0;
				double newRow = 0.0;
				double newCol = 0.0;
				double rad = 0.0;
				for (int i = 0; i <= pointCnt; i++)
				{
					HTuple measHandle = new HTuple();
					switch (arrow)
					{
						case EHalconOperatorArrow.TopArrow:			//上　縮小
							len = (i != pointCnt) ? len2.D - (pich * i) : len2.D * -1.0;
							newRow = row.D + (len * sin.D);
							newCol = col.D + (len * cos.D);
							measLen1 = (int)(hoseiLen1 * 2.0) + lenOffset;
							rad = hoseiPhi + (rad90.D * 2.0);
							break;
						case EHalconOperatorArrow.BottomArrow:	//下　縮小
							len = (i != pointCnt) ? len2.D - (pich * i) : len2.D * -1.0;
							newRow = row.D + (len * sin.D);
							newCol = col.D + (len * cos.D);
							measLen1 = (int)(hoseiLen1 * 2.0) + lenOffset;
							rad = hoseiPhi;
							break;
						case EHalconOperatorArrow.LeftArrow:			//左　縮小
							len = (i != pointCnt) ? len1.D - (pich * i) : len1.D * -1.0;
							newRow = row.D - (len * cos.D);
							newCol = col.D + (len * sin.D);
							measLen1 = (int)(hoseiLen2 * 2.0) + lenOffset;
							rad = hoseiPhi - rad90.D;
							break;
						case EHalconOperatorArrow.RightArrow:		//右　縮小
							len = (i != pointCnt) ? len1.D - (pich * i) : len1.D * -1.0;
							newRow = row.D - (len * cos.D);
							newCol = col.D + (len * sin.D);
							measLen1 = (int)(hoseiLen2 * 2.0) + lenOffset;
							rad = hoseiPhi + rad90.D;
							break;
					}
					if (newRow < 0.0 || imgHeight < newRow)
						continue;
					if (newCol < 0.0 || imgWidth < newCol)
						continue;
					HOperatorSet.GenMeasureRectangle2(newRow, newCol, rad, measLen1, measLen2, imgWidth, imgHeight, "nearest_neighbor", out measHandle);

					HTuple rowEdge, colEdge, amplitude, distance;
					HOperatorSet.MeasurePos(hoBinImage, measHandle, 1, 40, "positive", "first", out rowEdge, out colEdge, out amplitude, out distance);
					if (rowEdge.Length != 0)
					{
						HOperatorSet.TupleConcat(hvRows, rowEdge, out hvRows);
						HOperatorSet.TupleConcat(hvCols, colEdge, out hvCols);
					}
					HOperatorSet.CloseMeasure(measHandle);
				}
				HOperatorSet.GenRegionPolygon(out hoTemp[++tmpNo], hvRows, hvCols);

				HObject hoMoveRegion = null;
				HObject hoBaseEdge = null;


				if (mode == 0)
				{
					HOperatorSet.CopyObj(hoTemp[tmpNo], out hoBaseEdge, 1, 1);
					switch (arrow)
					{
						case EHalconOperatorArrow.TopArrow:			//上　縮小
							newRow = row.D - (val * cos.D);
							newCol = col.D + (val * sin.D);
							rad = hoseiPhi + (rad90.D * 2.0);
							break;
						case EHalconOperatorArrow.BottomArrow:	//下　縮小
							newRow = row.D + (val * cos.D);
							newCol = col.D - (val * sin.D);
							rad = hoseiPhi;
							break;
						case EHalconOperatorArrow.LeftArrow:			//左　縮小
							newRow = row.D - (val * sin.D);
							newCol = col.D - (val * cos.D);
							rad = hoseiPhi - rad90.D;
							break;
						case EHalconOperatorArrow.RightArrow:		//右　縮小
							newRow = row.D + (val * sin.D);
							newCol = col.D + (val * cos.D);
							rad = hoseiPhi + rad90.D;
							break;
					}
					newRow = row.D - newRow;
					newCol = col.D - newCol;

					HOperatorSet.MoveRegion(hoBaseEdge, out hoMoveRegion, newRow, newCol);

					HTuple rows1, cols1;
					HTuple rows2, cols2;
					HOperatorSet.GetRegionPoints(hoBaseEdge, out rows1, out cols1);
					HOperatorSet.GetRegionPoints(hoMoveRegion, out rows2, out cols2);

					HTuple invertRows2, invertCols2;
					HOperatorSet.TupleInverse(rows2, out invertRows2);
					HOperatorSet.TupleInverse(cols2, out invertCols2);

					HTuple c1Rows, c1Cols;
					HOperatorSet.TupleConcat(rows1, invertRows2, out c1Rows);
					HOperatorSet.TupleConcat(cols1, invertCols2, out c1Cols);

					HTuple c2Rows, c2Cols;
					HOperatorSet.TupleConcat(c1Rows, rows1[0], out c2Rows);
					HOperatorSet.TupleConcat(c1Cols, cols1[0], out c2Cols);

					HOperatorSet.GenContourPolygonXld(out hoTemp[++tmpNo], c2Rows, c2Cols);
					HOperatorSet.GenRegionContourXld(hoTemp[tmpNo], out newReg, "filled");

					HalconExtFunc.Clear(ref hoMoveRegion);
					HalconExtFunc.Clear(ref hoBaseEdge);
				}
				else if (mode == 1)
				{
					HObject hoAddEdge = null;
					HObject hoAddedRegion = null;
					HOperatorSet.CopyObj(hoTemp[tmpNo], out hoBaseEdge, 1, 1);
					HOperatorSet.CopyObj(hoTemp[tmpNo], out hoAddEdge, 1, 1);
					for (int i = 0; i < val; i++)
					{
						switch (arrow)
						{
							case EHalconOperatorArrow.TopArrow:			//上　縮小
								newRow = row.D - (i * cos.D);
								newCol = col.D + (i * sin.D);
								rad = hoseiPhi + (rad90.D * 2.0);
								break;
							case EHalconOperatorArrow.BottomArrow:	//下　縮小
								newRow = row.D + (i * cos.D);
								newCol = col.D - (i * sin.D);
								rad = hoseiPhi;
								break;
							case EHalconOperatorArrow.LeftArrow:			//左　縮小
								newRow = row.D - (i * sin.D);
								newCol = col.D - (i * cos.D);
								rad = hoseiPhi - rad90.D;
								break;
							case EHalconOperatorArrow.RightArrow:		//右　縮小
								newRow = row.D + (i * sin.D);
								newCol = col.D + (i * cos.D);
								rad = hoseiPhi + rad90.D;
								break;
						}
						newRow = row.D - newRow;
						newCol = col.D - newCol;

						HOperatorSet.MoveRegion(hoBaseEdge, out hoMoveRegion, newRow, newCol);

						HalconExtFunc.Clear(ref hoAddedRegion);
						HOperatorSet.Union2(hoAddEdge, hoMoveRegion, out hoAddedRegion);
						HalconExtFunc.Clear(ref hoMoveRegion);

						HalconExtFunc.Clear(ref hoAddEdge);
						HOperatorSet.CopyObj(hoAddedRegion, out hoAddEdge, 1, 1);
					}
					HalconExtFunc.Clear(ref hoBaseEdge);
					HalconExtFunc.Clear(ref hoAddEdge);
					HalconExtFunc.Clear(ref hoMoveRegion);
					HOperatorSet.ClosingCircle(hoAddedRegion, out newReg, 1.5);
					HalconExtFunc.Clear(ref hoAddedRegion);
				}

				//HOperatorSet.WriteRegion(hoBaseEdge, @"c:\reg1.reg");
				//HOperatorSet.WriteRegion(hoAddEdge, @"c:\reg2.reg");
			}
			catch (HOperatorException oe)
			{
				throw oe;
			}
			finally
			{
				for (int i = 0; i < maxTemp; i++)
				{
					if (hoTemp[i] != null)
						hoTemp[i].Dispose();

					if (hoBinImage != null)
						hoBinImage.Dispose();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reg"></param>
		/// <param name="arrow">true:上　false:右</param>
		/// <returns></returns>
		public static double GetRegionRect2Arrow(HObject reg, bool arrow)
		{
			HTuple phi;
			HTuple rad180;
			HTuple abs180;
			HTuple rad180Plus, rad180Minus, rad135Plus, rad135Minus;
			HOperatorSet.OrientationRegion(reg, out phi);
			double hoseiPhi = phi.D;

			HOperatorSet.TupleRad(180.0, out rad180);
			HOperatorSet.TupleAbs(rad180, out abs180);
			rad180Plus = new HTuple();
			rad180Plus = abs180.D;
			rad180Minus = new HTuple();
			rad180Minus = abs180.D * -1.0;
			HOperatorSet.TupleRad(135.0, out rad135Plus);
			rad135Minus = new HTuple();
			rad135Minus = rad135Plus.D * -1.0;
			if (arrow == true)
			{
				//上
				if (phi.D < 0.0)
				{
					hoseiPhi = phi.D + rad180Plus.D;
				}
			}
			else
			{
				//右
				if ((rad135Plus < phi.D && phi.D < rad180Plus) ||
					(rad180Minus < phi.D && phi.D < rad135Minus))
				{
					hoseiPhi = phi.D + rad180Plus.D;
				}
			}
			return hoseiPhi;
		}
	}
}
