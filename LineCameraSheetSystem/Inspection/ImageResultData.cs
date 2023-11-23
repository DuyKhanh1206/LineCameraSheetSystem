using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using LineCameraSheetSystem;
using ResultActionDataClassNameSpace;
using LogingDllWrap;
using HalconDotNet;
using KaTool;

namespace InspectionNameSpace
{
    /// <summary>
    /// イメージ結果クラス
    /// 
    /// </summary>
    public class ImageResultData : IDisposable
    {
        /// <summary>
        /// 開始位置
        /// </summary>
        public double StartPosition { get; private set; }
        /// <summary>
        /// 終了位置
        /// </summary>
        public double EndPosition { get; private set; }

        /// <summary>
        /// カメラ番号
        /// </summary>
        public AppData.CamID CamId { get; private set; }
        /// <summary>
        /// カメラ部位
        /// </summary>
        public AppData.SideID SideId { get; private set; }
        /// <summary>
        /// NG項目
        /// </summary>
        public AppData.InspID InspId { get; private set; }
        /// <summary>
        /// 横方向の位置(mm)
        /// </summary>
        public double PositionX { get; private set; }
        /// <summary>
        /// 縦方向の位置(mm)
        /// </summary>
        public double PositionY { get; private set; }
        /// <summary>
        /// 横の長さ(mm)
        /// </summary>
        public double Width { get; private set; }
        /// <summary>
        /// 縦の長さ(mm)
        /// </summary>
        public double Height { get; private set; }
        /// <summary>
        /// 面積(mm^2)
        /// </summary>
        public double Area { get; private set; }
        /// <summary>
        /// ゾーン番号(1-...)
        /// </summary>
        public AppData.ZoneID ZoneId { get; private set; }

        /// <summary>
        /// NG画像
        /// </summary>
        public HObject ImageNG { get { return _imageNg; } }
        private HObject _imageNg;

        public HObject TargetImage { get { return _targetImg; } }
        private HObject _targetImg;
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="area"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="imageOrg"></param>
        /// <param name="imageParam"></param>
        public ImageResultData(AppData.InspID inspId, double startPos, double endPos, double area, double width, double height, double x, double y, AppData.ZoneID zoneId, CameraInfo camInfo)
        {
            //開始位置
            this.StartPosition = startPos;
            //終了位置
            this.EndPosition = endPos;

            //カメラ番号
            this.CamId = camInfo.CamNo;
            //カメラ部位
            this.SideId = camInfo.CamSide;
            //NG項目
            this.InspId = inspId;

            //横方向の位置
            this.PositionX = x;
            //縦方向の位置
            this.PositionY = y;
            //面積
            this.Area = area;
            //横
            this.Width = width;
            //縦
            this.Height = height;
            //ゾーン番号
            this.ZoneId = zoneId;
        }
        /// <summary>
        /// デストラクタ
        /// </summary>
        public void Dispose()
        {
            if (_imageNg != null)
            {
                _imageNg.Dispose();
            }
            if (_targetImg != null)
                _targetImg.Dispose();
        }

        /// <summary>
        /// NGイメージを切り抜く
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="imageOrg"></param>
        public void CropNgImage(double x, double y, HObject imageOrg, int cropWidth, int cropHeight, int scaleWidth, int scaleHeight, HObject pareImage, int pareOffsetX, int pareOffsetY, HObject imageCon)
        {
            const int maxTmp = 100;
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[maxTmp];
            HObject hoSelectedObj;
            HObject hoSelObj1, hoSelObj2;
            HObject hoTileImageObj;
            HObject hoConcatObj;
            HObject dmy;
            HOperatorSet.GenEmptyObj(out hoSelectedObj);
            HOperatorSet.GenEmptyObj(out hoSelObj1);
            HOperatorSet.GenEmptyObj(out hoSelObj2);
            HOperatorSet.GenEmptyObj(out hoTileImageObj);
            HOperatorSet.GenEmptyObj(out hoConcatObj);
            HOperatorSet.GenEmptyObj(out dmy);

            HTuple homMat2DIdentity;
            HTuple homMat2DScale;
			HTuple htCropDomainWidth;
			HTuple htCropDomainHeight;
			HTuple htImgWidth;
			HTuple htImgHeight;
            HTuple htNumber;
            HTuple htObjCnt1, htObjCnt2;

			HObject myCropImg = null;
			HObject pareCropImage = null;

            if (cropHeight == 0)
                cropHeight = scaleHeight;

            int cnt;
            int objCnt1, objCnt2;

            string ErrStr;

            int noTmp = 0;
            try
            {
                HOperatorSet.CopyObj(imageCon, out _targetImg, 1, -1);

                //原イメージから切り抜く
                int row1, col1, row2, col2;
				row1 = (int)(y - (cropHeight / 2));
				col1 = (int)(x - (cropWidth / 2));
				row2 = row1 + cropHeight - 1;
				col2 = col1 + cropWidth - 1;

                ErrStr = string.Format("x:{0} y:{1} row1:{2} col1:{3} row2:{4} col2:{5}", x, y, row1, col1, row2, col2);
                LogingDll.Loging_SetLogString(ErrStr);

                HOperatorSet.GenRectangle1(out OTemp[++noTmp], row1, col1, row2, col2);
                HOperatorSet.ReduceDomain(imageOrg, OTemp[noTmp], out OTemp[++noTmp]);
                if (col1 >= 0)
                {
                    HOperatorSet.CropDomain(OTemp[noTmp], out OTemp[++noTmp]);
                }
                else
                {
                    ErrStr = string.Format("CropPart(1)={0} {1} {2} {3}", row1, 0, col2 + 1, row2 - row1 + 1);
                    LogingDll.Loging_SetLogString(ErrStr);
                    HTuple htObjCnt;
                    HOperatorSet.CountObj(OTemp[noTmp], out htObjCnt);
                    try
                    {
                        HOperatorSet.CropPart(OTemp[noTmp], out OTemp[++noTmp], row1, 0, col2 + 1, row2 - row1 + 1);
                    }
                    catch
                    {
                        ErrStr = string.Format("CropPart(1)=ERROR");
                        LogingDll.Loging_SetLogString(ErrStr);
                        CreateDmyCrop(htObjCnt.I, cropWidth, cropHeight, out OTemp[++noTmp]);
                    }
                }
                //自身のクロップイメージ
                HOperatorSet.CopyObj(OTemp[noTmp], out myCropImg, 1, -1);
				//クロップイメージサイズ
				HOperatorSet.GetImageSize(myCropImg, out htCropDomainWidth, out htCropDomainHeight);
                HOperatorSet.CountObj(myCropImg, out htNumber);
				//イメージサイズ
				HOperatorSet.GetImageSize(imageOrg, out htImgWidth, out htImgHeight);

				AppData.CamPosition? cropPosition;
				cropPosition = null;
				//左にはみ出している
				if (col1 < 0)
					cropPosition = AppData.CamPosition.Left;
				//右にはみ出している
				else if (col2 >= htImgWidth)
					cropPosition = AppData.CamPosition.Right;
                //中央部(重なり)で、はみ出しているので、Tileする
                if (cropPosition != null && cropPosition != AppData.CamPosition.Left)
                {
                    HObject leftImg;
                    HObject rightImg;
                    HTuple htOffsetX;

                    int myCropWidth = htCropDomainWidth.I;
                    int pareCropWidth = cropWidth - myCropWidth;
                    int offx;
                    int offy = row1 - pareOffsetY;

                    offx = pareOffsetX;
                    if (offx < 0)
                        offx = 0;
                    if (offy < 0)
                        offy = 0;

                    ErrStr = string.Format("CropPart(2)={0} {1} {2} {3}", offy, offx, pareCropWidth, cropHeight);
                    LogingDll.Loging_SetLogString(ErrStr);

                    if (offx >= htImgWidth.I)
                        offx = htImgWidth.I - pareCropWidth;
                    if (offy >= htImgHeight.I)
                        offy = htImgHeight.I - cropHeight;
                    HTuple htObjCnt;
                    HOperatorSet.CountObj(pareImage, out htObjCnt);
                    try
                    {
                        HOperatorSet.CropPart(pareImage, out pareCropImage, offy, offx, pareCropWidth, cropHeight);
                    }
                    catch
                    {
                        ErrStr = string.Format("CropPart(2)=ERROR");
                        LogingDll.Loging_SetLogString(ErrStr);
                        CreateDmyCrop(htObjCnt.I, cropWidth, cropHeight, out OTemp[++noTmp]);
                    }
                    leftImg = myCropImg;
                    rightImg = pareCropImage;
                    htOffsetX = new HTuple(0, myCropWidth);

                    HOperatorSet.CountObj(leftImg, out htObjCnt1);
                    HOperatorSet.CountObj(rightImg, out htObjCnt2);
                    objCnt1 = htObjCnt1.I;
                    objCnt2 = htObjCnt2.I;
                    //イメージを連結する
                    cnt = htNumber.I;
                    HOperatorSet.GenEmptyObj(out hoConcatObj);
                    for (int i = 0; i < cnt; i++)
                    {
                        int no = i + 1;
                        hoSelObj1.Dispose();
                        HOperatorSet.SelectObj(leftImg, out hoSelObj1, no);
                        hoSelObj2.Dispose();
                        HOperatorSet.SelectObj(rightImg, out hoSelObj2, no);
                        if (objCnt1 != objCnt2)
                        {
                            HTuple HtChannel;
                            HOperatorSet.CountChannels(hoSelObj1, out HtChannel);
                            hoSelObj2.Dispose();
                            if (HtChannel.I == 1)
                                ImageTool.InitializeImage(htCropDomainWidth, htCropDomainHeight, out hoSelObj2, 255);
                            else
                                ImageTool.InitializeImage3(htCropDomainWidth, htCropDomainHeight, out hoSelObj2, 255);
                        }
                        hoSelectedObj.Dispose();
                        HOperatorSet.ConcatObj(hoSelObj1, hoSelObj2, out hoSelectedObj);
                        hoTileImageObj.Dispose();
                        HOperatorSet.TileImagesOffset(hoSelectedObj, out hoTileImageObj,
                            new HTuple(0, 0),
                            htOffsetX,
                            new HTuple(-1, -1), new HTuple(-1, -1), new HTuple(-1, -1), new HTuple(-1, -1),
                            cropWidth, cropHeight);

                        dmy.Dispose();
                        HOperatorSet.CopyObj(hoConcatObj, out dmy, 1, -1);
                        hoConcatObj.Dispose();
                        HOperatorSet.ConcatObj(dmy, hoTileImageObj, out hoConcatObj);
                    }
                    HOperatorSet.CopyObj(hoConcatObj, out OTemp[++noTmp], 1, -1);
                }
                else if (cropPosition == AppData.CamPosition.Left)
                {
                    cnt = htNumber.I;
                    HOperatorSet.GenEmptyObj(out hoConcatObj);
                    for (int i = 0; i < cnt; i++)
                    {
                        hoSelectedObj.Dispose();
                        HOperatorSet.SelectObj(myCropImg, out hoSelectedObj, i + 1);
                        hoTileImageObj.Dispose();
                        HOperatorSet.TileImagesOffset(hoSelectedObj, out hoTileImageObj,
                            0,
                            -col1,
                            -1, -1, -1, -1,
                            cropWidth, cropHeight);

                        dmy.Dispose();
                        HOperatorSet.CopyObj(hoConcatObj, out dmy, 1, -1);
                        hoConcatObj.Dispose();
                        HOperatorSet.ConcatObj(dmy, hoTileImageObj, out hoConcatObj);
                    }
                    HOperatorSet.CopyObj(hoConcatObj, out OTemp[++noTmp], 1, -1);
                }
                else if (cropPosition == AppData.CamPosition.Right)
                {
                    cnt = htNumber.I;
                    HOperatorSet.GenEmptyObj(out hoConcatObj);
                    for (int i = 0; i < cnt; i++)
                    {
                        hoSelectedObj.Dispose();
                        HOperatorSet.SelectObj(myCropImg, out hoSelectedObj, i + 1);
                        hoTileImageObj.Dispose();
                        HOperatorSet.TileImagesOffset(hoSelectedObj, out hoTileImageObj,
                        0,
                        0,
                        -1, -1, -1, -1,
                        cropWidth, cropHeight);

                        dmy.Dispose();
                        HOperatorSet.CopyObj(hoConcatObj, out dmy, 1, -1);
                        hoConcatObj.Dispose();
                        HOperatorSet.ConcatObj(dmy, hoTileImageObj, out hoConcatObj);
                    }
                    HOperatorSet.CopyObj(hoConcatObj, out OTemp[++noTmp], 1, -1);
                }
                else
                {
                    if (row1 < 0)
                    {
                        row2 = row2 - row1;
                        row1 = 0;
                    }
                    ErrStr = string.Format("CropPart(3)={0} {1} {2} {3}", row1, col1, col2 - col1 + 1, row2 - row1 + 1);
                    LogingDll.Loging_SetLogString(ErrStr);
                    HTuple htObjCnt;
                    HOperatorSet.CountObj(imageOrg, out htObjCnt);
                    //row, col, width, height
                    try
                    {
                        HOperatorSet.CropPart(imageOrg, out OTemp[++noTmp], row1, col1, col2 - col1 + 1, row2 - row1 + 1);
                    }
                    catch
                    {
                        ErrStr = string.Format("CropPart(3)=ERROR");
                        LogingDll.Loging_SetLogString(ErrStr);
                        CreateDmyCrop(htObjCnt.I, cropWidth, cropHeight, out OTemp[++noTmp]);
                    }
                }

                //切り抜いたイメージをスケールサイズに変換する
                double scalex = (double)scaleHeight / (double)cropHeight;
                double scaley = (double)scaleWidth / (double)cropWidth;
                HOperatorSet.HomMat2dIdentity(out homMat2DIdentity);
                HOperatorSet.HomMat2dScale(homMat2DIdentity, scalex, scaley, 0.0, 0.0, out homMat2DScale);
                HOperatorSet.AffineTransImageSize(OTemp[noTmp], out OTemp[++noTmp], homMat2DScale, "constant", scaleWidth, scaleHeight);

                HOperatorSet.CopyObj(OTemp[noTmp], out this._imageNg, 1, -1);
            }
            catch (HalconException exc)
            {
				ErrStr = string.Format("ImageResultData.CropNgImage() exc = {0}", exc.Message);
				LogingDll.Loging_SetLogString(ErrStr);
				Debug.WriteLine(ErrStr);
				throw exc;
            }
            finally
            {
				UtilityImage.ClearHalconObject(ref myCropImg);
				UtilityImage.ClearHalconObject(ref pareCropImage);

                for (int i = 0; i < OTemp.Length; i++)
                {
                    UtilityImage.ClearHalconObject(ref OTemp[i]);
                }

                hoSelectedObj.Dispose();
                hoSelObj1.Dispose();
                hoSelObj2.Dispose();
                hoTileImageObj.Dispose();
                hoConcatObj.Dispose();
                dmy.Dispose();
            }
        }

        private void CreateDmyCrop(int iObjCnt, int cropWidth, int cropHeight, out HObject hoConcatObj)
        {
            HOperatorSet.GenEmptyObj(out hoConcatObj);

            HObject hoRectangle;
            HObject hoDmy;
            HOperatorSet.GenEmptyObj(out hoRectangle);
            HOperatorSet.GenEmptyObj(out hoDmy);


            HObject[] hoConstImage = new HObject[iObjCnt];
            try
            {
                for (int i = 0; i < iObjCnt; i++)
                {
                    hoRectangle.Dispose();
                    HOperatorSet.GenRectangle1(out hoRectangle, 0, 0, cropHeight, cropWidth);
                    HOperatorSet.RegionToBin(hoRectangle, out hoConstImage[i], 128, 0, cropWidth, cropHeight);
                }
                HOperatorSet.GenEmptyObj(out hoDmy);
                for (int i = 0; i < iObjCnt; i++)
                {
                    hoDmy.Dispose();
                    HOperatorSet.CopyObj(hoConcatObj, out hoDmy, 1, -1);
                    hoConcatObj.Dispose();
                    HOperatorSet.ConcatObj(hoDmy, hoConstImage[i], out hoConcatObj);
                }
            }
            catch(HalconException exc)
            {
                throw exc;
            }
            finally
            {
                for (int i=0; i<iObjCnt; i++)
                {
                    if (hoConstImage[i] != null)
                        hoConstImage[i].Dispose();
                }
                hoRectangle.Dispose();
                hoDmy.Dispose();
            }
        }

        public ImageResultData Copy()
        {
            ImageResultData res;
            res = (ImageResultData)MemberwiseClone();
            HOperatorSet.CopyObj(this._imageNg, out res._imageNg, 1, -1);
            HOperatorSet.CopyObj(this._targetImg, out res._targetImg, 1, -1);
            return res;
        }
    }
}
