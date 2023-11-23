using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using HalconDotNet;

namespace InspectionNameSpace
{
    public static class UtilityImage
    {
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

        public static void ConcatColor2ConcatObject4(HObject hoColorImage, out HObject hoConcatObj4)
        {
            HOperatorSet.GenEmptyObj(out hoConcatObj4);

            HObject hoGrayImage;
            HObject hoImage1, hoImage2, hoImage3;
            HOperatorSet.GenEmptyObj(out hoImage1);
            HOperatorSet.GenEmptyObj(out hoImage2);
            HOperatorSet.GenEmptyObj(out hoImage3);
            HOperatorSet.GenEmptyObj(out hoGrayImage);

            try
            {
                HOperatorSet.Rgb1ToGray(hoColorImage, out hoGrayImage);
                HOperatorSet.Decompose3(hoColorImage, out hoImage1, out hoImage2, out hoImage3);
                ConcatObject4(hoGrayImage, hoImage1, hoImage2, hoImage3, out hoConcatObj4);
            }
            catch (HalconException exc)
            {
                throw exc;
            }
            finally
            {
                hoGrayImage.Dispose();
                hoImage1.Dispose();
                hoImage2.Dispose();
                hoImage3.Dispose();
            }

        }

        public static void ConcatObject4(HObject hoGrayImage, HObject hoImage1, HObject hoImage2, HObject hoImage3, out HObject hoConcatObj4)
        {
            HOperatorSet.GenEmptyObj(out hoConcatObj4);
            HObject hoAllObject;
            HObject dmy;
            HOperatorSet.GenEmptyObj(out hoAllObject);
            HOperatorSet.GenEmptyObj(out dmy);

            try
            {
                hoAllObject.Dispose();
                HOperatorSet.ConcatObj(hoGrayImage, hoImage1, out hoAllObject);
                dmy.Dispose();
                HOperatorSet.CopyObj(hoAllObject, out dmy, 1, -1);
                hoAllObject.Dispose();
                HOperatorSet.ConcatObj(dmy, hoImage2, out hoAllObject);
                dmy.Dispose();
                HOperatorSet.CopyObj(hoAllObject, out dmy, 1, -1);
                hoConcatObj4.Dispose();
                HOperatorSet.ConcatObj(dmy, hoImage3, out hoConcatObj4);
            }
            catch(HalconException exc)
            {
                throw exc;
            }
            finally
            {
                hoAllObject.Dispose();
                dmy.Dispose();
            }
        }

        /// <summary>
        /// イメージをコピーする
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static HObject CopyHalconImage(HObject image)
        {
            HObject img = null;
            try
            {
                if (image != null)
                    HOperatorSet.CopyObj(image, out img, 1, -1);
            }
            catch(HalconException)
            {
                return null;
            }
            return img;
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
                if (connectLines > 0)
                {
                    //イメージサイズを取得する
                    //[5000pix]
                    HTuple ht1, ht2;
                    HTuple ht3, ht4;
                    HOperatorSet.GetImageSize(beforeImage, out ht1, out ht2);
                    int befWidth = ht1.I;
                    int befHeight = ht2.I;
                    //[4000pix]
                    HOperatorSet.GetImageSize(currentImage, out ht3, out ht4);
                    int curWidth = ht3.I;
                    int curHeight = ht4.I;
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
                else
                {
                    HOperatorSet.CopyObj(currentImage, out outImage, 1, -1);
                }
            }
            catch (HalconException exc)
            {
                string ErrStr = string.Format("UtilityImage.ConnectHeaderImage() exc = {0}", exc.Message);
                LogingDllWrap.LogingDll.Loging_SetLogString(ErrStr);
                Debug.WriteLine(ErrStr);
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
                if (connectLines > 0)
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
                else
                {
                    HOperatorSet.CopyObj(currentImage, out outImage, 1, -1);
                }
            }
            catch (HalconException exc)
            {
				string ErrStr = string.Format("UtilityImage.ConnectFooterTileImage() err = {0}", exc.Message);
                LogingDllWrap.LogingDll.Loging_SetLogString(ErrStr);
                Debug.WriteLine(ErrStr);
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
                Debug.WriteLine(ErrStr);
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
