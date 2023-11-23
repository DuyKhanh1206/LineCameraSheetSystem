using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalconDotNet;

namespace KaTool
{
    public static class ImageTool
    {
        /// <summary>
        /// 指定位置の多値レベル値を返す
        /// </summary>
        /// <param name="img">イメージ</param>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <returns></returns>
        public static int GrayValue(HObject img, int x, int y)
        {
            if (img != null)
            {
                HTuple w, h;
                HOperatorSet.GetImageSize(img, out w, out h);
                if (x < w.I && y < h.I)
                {
                    HTuple value;
                    HOperatorSet.GetGrayval(img, y, x, out value);
                    return value.I;
                }
            }
            return 0;
        }
        /// <summary>
        /// 指定位置の多値レベル値のフォーマット文字列を返す
        /// </summary>
        /// <param name="img"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static string GrayValueStr(HObject img, int x, int y)
        {
            return string.Format("座標:{0:D4},{1:D4} 多値:{2:D3}", x, y, GrayValue(img, x, y));
        }

        /// <summary>
		/// フォーカス数値を返す
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static double FocusValue(HObject img)
        {
            HTuple Mean, Deviation;
            HObject edgeAmplitude;
            HOperatorSet.SobelAmp(img, out edgeAmplitude, "sum_abs", 3);
            HOperatorSet.Intensity(edgeAmplitude, edgeAmplitude, out Mean, out Deviation);
            edgeAmplitude.Dispose();
            return Mean.D;
        }
        /// <summary>
		/// フォーカス数値をフォーマット文字列で返す
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static string FocusValueStr(HObject img)
        {
            HTuple htCount;
            HObject hoGrayImg;
            HOperatorSet.CountChannels(img, out htCount);
            if (htCount == 3)
                HOperatorSet.Rgb1ToGray(img, out hoGrayImg);
            else
                HOperatorSet.CopyObj(img, out hoGrayImg, 1, -1);
            return string.Format("FC:{0:F2}", FocusValue(hoGrayImg));
        }
		/// <summary>
		/// イメージを初期設定する
		/// </summary>
		/// <param name="width">イメージ幅</param>
		/// <param name="height">イメージ高さ</param>
		/// <returns></returns>
		public static void InitializeImage(int width, int height, out HObject image, int grayval)
		{
			HObject bufImage;
			HOperatorSet.GenImageConst(out bufImage, "byte", width, height);
			HOperatorSet.GenImageProto(bufImage, out image, grayval);
			bufImage.Dispose();
		}
        public static void InitializeImage3(int width, int height, out HObject image, int grayval)
        {
            HObject bufImage;
            HObject grayImage;
            HOperatorSet.GenImageConst(out bufImage, "byte", width, height);
            HOperatorSet.GenImageProto(bufImage, out grayImage, grayval);
            HOperatorSet.Compose3(grayImage, grayImage, grayImage, out image);
            bufImage.Dispose();
            grayImage.Dispose();
        }
        public static void InitializeImage4(int width, int height, out HObject image, int grayval)
        {
            HObject bufImage;
            HObject grayImage;
            HOperatorSet.GenImageConst(out bufImage, "byte", width, height);
            HOperatorSet.GenImageProto(bufImage, out grayImage, grayval);
            HOperatorSet.Compose3(grayImage, grayImage, grayImage, out image);
            bufImage.Dispose();
            grayImage.Dispose();
        }

        /// <summary>
        /// HObjectをクリアする
        /// </summary>
        /// <param name="obj"></param>
        public static void ClearHalconObject(ref HObject obj)
		{
			if (obj != null)
			{
				obj.Dispose();
				obj = null;
			}
		}
		/// <summary>
		/// HObjectをクリアする
		/// </summary>
		/// <param name="obj"></param>
		public static void ClearHalconObject(ref HRegion region)
		{
			if (region != null)
			{
				region.Dispose();
				region = null;
			}
		}
		/// <summary>
		/// イメージをコピーする
		/// </summary>
		/// <param name="fromImage"></param>
		/// <returns></returns>
		public static void CopyHalconImage(HObject fromImage, out HObject toImage)
		{
			HOperatorSet.CopyObj(fromImage, out toImage, 1, -1);
		}

		/// <summary>
		/// イメージ(beforeImage)末尾をイメージ(currentImage)の先頭に連結する
		/// </summary>
		/// <param name="beforeImage"></param>
		/// <param name="currentImage"></param>
		/// <param name="outImage"></param>
		/// <param name="connectLines"></param>
		public static void ConnectHeaderImage(HObject beforeImage, HObject currentImage, out HObject outImage, int connectLines)
		{
			const int tmpMax = 20;
			// Stack for temporary objects 
			HObject[] OTemp = new HObject[tmpMax];

			//outImage = null;

			int tmpNo = 0;
			try
			{
				//イメージサイズを取得する
				//[5000pix]
				HTuple ht1, ht2;
				HOperatorSet.GetImageSize(beforeImage, out ht1, out ht2);
				int befWidth = ht1.I;
				int befHeight = ht2.I;
				//[4000pix]
				HOperatorSet.GetImageSize(currentImage, out ht1, out ht2);
				int curWidth = ht1.I;
				int curHeight = ht2.I;
				//[5000pix]イメージの下[1000pix]
				int cutRow = befHeight - connectLines;
				//前のイメージの連結する部分（下部）を切り取る[1000pix]
				HOperatorSet.CropPart(beforeImage, out OTemp[++tmpNo], cutRow, 0, befWidth, connectLines);
				//切り取ったイメージと現在のイメージをConcatする[1000pix , 4000pix]
				HOperatorSet.ConcatObj(OTemp[tmpNo], currentImage, out OTemp[++tmpNo]);

				//イメージを連結する[1000pix + 4000pix = 5000pix]
				HOperatorSet.TileImagesOffset(OTemp[tmpNo], out OTemp[++tmpNo],
					//4000pixイメージを下へ1000pixシフト
					new HTuple(0, connectLines),
					new HTuple(0, 0), new HTuple(-1, -1), new HTuple(-1, -1), new HTuple(-1, -1), new HTuple(-1, -1),
					curWidth, curHeight + connectLines);
				//イメージを保持する
				HOperatorSet.CopyObj(OTemp[tmpNo], out outImage, 1, -1);
			}
			catch (HalconException exc)
			{
				string ErrStr = string.Format("UtilityImage.ConnectHeaderImage() exc = {0}", exc.Message);
				LogingDllWrap.LogingDll.Loging_SetLogString(ErrStr);
				System.Diagnostics.Debug.WriteLine(ErrStr);
				throw exc;
			}
			finally
			{
				for (int i = 0; i < OTemp.Length; i++)
				{
					ClearHalconObject(ref OTemp[i]);
				}
			}
		}

		/// <summary>
		/// イメージ(nextImage)の頭をイメージ(currentImage)のお尻に連結する
		/// </summary>
		/// <param name="nextImage"></param>
		/// <param name="CurrentImage"></param>
		/// <param name="camInfo"></param>
		/// <param name="outImage"></param>
		public static void ConnectFooterTileImage(HObject currentImage, HObject nextImage, out HObject outImage, int connectLines, int nextImageOffset)
		{
			const int tmpMax = 20;
			// Stack for temporary objects 
			HObject[] OTemp = new HObject[tmpMax];

			int tmpNo = 0;
			try
			{
				//イメージサイズを取得する
				HTuple ht1, ht2;
				HOperatorSet.GetImageSize(currentImage, out ht1, out ht2);
				int curWidth = ht1.I;
				int curHeight = ht2.I;
				HOperatorSet.GetImageSize(nextImage, out ht1, out ht2);
				int nextWidth = ht1.I;
				int nextHeight = ht2.I;

				//次のイメージの連結する部分（上部）を切り取る
				HOperatorSet.CropPart(nextImage, out OTemp[++tmpNo], nextImageOffset, 0, nextWidth, connectLines);
				//切り取ったイメージと現在のイメージをConcatする
				HOperatorSet.ConcatObj(currentImage, OTemp[tmpNo], out OTemp[++tmpNo]);
				//イメージを連結する
				HOperatorSet.TileImagesOffset(OTemp[tmpNo], out OTemp[++tmpNo],
					new HTuple(0, curHeight),
					new HTuple(0, 0), new HTuple(-1, -1), new HTuple(-1, -1), new HTuple(-1, -1), new HTuple(-1, -1),
					curWidth, curHeight + connectLines);
				//イメージを保持する
				HOperatorSet.CopyObj(OTemp[tmpNo], out outImage, 1, -1);
			}
			catch (HalconException exc)
			{
				string ErrStr = string.Format("UtilityImage.ConnectFooterTileImage() err = {0}", exc.Message);
				LogingDllWrap.LogingDll.Loging_SetLogString(ErrStr);
				System.Diagnostics.Debug.WriteLine(ErrStr);
				throw exc;
			}
			finally
			{
				for (int i = 0; i < OTemp.Length; i++)
				{
					ClearHalconObject(ref OTemp[i]);
				}
			}
		}

		/// <summary>
		/// イメージを左右に連結する
		/// </summary>
		/// <param name="leftPrioHigh"></param>
		/// <param name="leftImage"></param>
		/// <param name="rightImage"></param>
		/// <param name="offsetX"></param>
		public static void ConnectSideTileImage(bool leftPriorityHigh, bool rightEnabled, HObject leftImage, HObject rightImage, out HObject outImage, double offsetX, double offsetY, double center)
		{
			const int tmpMax = 20;
			// Stack for temporary objects 
			HObject[] OTemp = new HObject[tmpMax];

			HObject leftCrop = null;

			int tmpNo = 0;
			try
			{
				//上に被せて表示するイメージの優先順位を決める
				HObject priorityHigh;
				HObject priorityLow;
				HTuple htOffsetX;
				HTuple htOffsetY;
				HTuple htw, hth;

				//左：○　右：○
				if (leftPriorityHigh == true && rightEnabled == true)
				{
					//左イメージの右端を削除する
					HOperatorSet.GetImageSize(leftImage, out htw, out hth);
					HOperatorSet.CropPart(leftImage, out leftCrop, 0, 0, center, hth.I);
				}
				else
				{
					HOperatorSet.CopyObj(leftImage, out leftCrop, 1, -1);
				}

				if (leftPriorityHigh == true)
				{
					priorityHigh = leftCrop;
					priorityLow = rightImage;
					htOffsetX = new HTuple(offsetX, 0);
					htOffsetY = new HTuple(offsetY, 0);
				}
				else
				{
					priorityHigh = rightImage;
					priorityLow = leftCrop;
					htOffsetX = new HTuple(0, offsetX);
					htOffsetY = new HTuple(0, offsetY);
				}
				//イメージサイズを取得する
				HOperatorSet.GetImageSize(rightImage, out htw, out hth);
				double width = htw.I + offsetX;
				double height = hth.I;
				//イメージを連結する
				HOperatorSet.ConcatObj(priorityLow, priorityHigh, out OTemp[++tmpNo]);
				HOperatorSet.TileImagesOffset(OTemp[tmpNo], out OTemp[++tmpNo],
					htOffsetY,
					htOffsetX,
					new HTuple(-1, -1), new HTuple(-1, -1), new HTuple(-1, -1), new HTuple(-1, -1),
					width, height);
				//イメージを保持する
				HOperatorSet.CopyObj(OTemp[tmpNo], out outImage, 1, -1);
			}
			catch (HalconException exc)
			{
				string ErrStr = string.Format("UtilityImage.ConnectSideTileImage() err = {0}", exc.Message);
				LogingDllWrap.LogingDll.Loging_SetLogString(ErrStr);
				System.Diagnostics.Debug.WriteLine(ErrStr);
				throw exc;
			}
			finally
			{
				ClearHalconObject(ref leftCrop);
				for (int i = 0; i < OTemp.Length; i++)
				{
					ClearHalconObject(ref OTemp[i]);
				}
			}
			//sw.Stop();
			//Debug.WriteLine(string.Format("ConnectSideTileImage() time = {0}", sw.ElapsedMilliseconds.ToString("F1")));
		}
	}
}
