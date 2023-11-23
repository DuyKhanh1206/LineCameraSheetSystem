using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using LineCameraSheetSystem;
using LogingDllWrap;
using HalconDotNet;
using HalconCamera;

namespace InspectionNameSpace
{
    /// <summary>
    /// イメージ検査クラス
    /// </summary>
    class ImageInspection
    {
        public CameraInfo CamInfo { get; private set; }
        LengthMeas _lengthMeas;
		private double _resolutionX;
		private double _resolutionY;
		private double _onePixAreaResoluton;

		public AppData.InspID[] InspOrder;

        /// <summary>
        /// 原イメージから切り抜くNGイメージの幅サイズ(pix)
        /// </summary>
        public int CropImageWidth { get; set; }
        /// <summary>
        /// 原イメージから切り抜くNGイメージの縦サイズ(pix)
        /// </summary>
        public int CropImageHeight { get; set; }
        /// <summary>
        /// 切り抜いたNGイメージのスケーリング後の幅サイズ(pix)
        /// </summary>
        public int ScaleImageWidth { get; set; }
        /// <summary>
        /// 切り抜いたNGイメージのスケーリング後の縦サイズ(pix)
        /// </summary>
        public int ScaleImageHeight { get; set; }
        /// <summary>
        /// 左右連結および上下連結
        /// </summary>
        public HObject ImageCon { get; set; }

        /// <summary>
        /// 基準位置からカメラまでの距離
        /// </summary>
        public double BasePoint
        {
            get;
            set;
        }
		/// <summary>
		/// NG切り抜き用Xオフセット(pix)
		/// </summary>
		public int PareOffsetXpix { get; set; }
		/// <summary>
		/// NG切り抜き用Yオフセット(pix)
		/// </summary>
		public int PareOffsetYpix { get; set; }

		/// <summary>
		/// 中央部重なり部での除外範囲[pix]（最小値）
		/// </summary>
		public int HOverlopExceptMin { get; set; }
		/// <summary>
		/// 中央部重なり部での除外範囲[pix]（最大値）
		/// </summary>
		public int HOverlopExceptMax { get; set; }

		/// <summary>
		/// NG表示ヶ所
		/// 0:上流側から見て、左側NGが画面では右側に表示される
		/// 1:上流側から見て、左側NGが画面では左側に表示される
		/// </summary>
		public int NGPositionMode { get; set; }

		public int InspWidthStartPix { get; set; }
		public int InspWidthEndPix { get; set; }

		public void SetResolution(double resolutionX, double resolutionY)
		{
			System.Diagnostics.Debug.WriteLine(string.Format("ImageInspection({0})  x = {1:F3} y = {2:F3}", this.CamInfo.CamNo.ToString(), this.CamInfo.ResolutionX, this.CamInfo.ResolutionY));

			this._resolutionX = resolutionX;
			this._resolutionY = resolutionY;
			this._onePixAreaResoluton = resolutionX * resolutionY;
		}

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="imageparam"></param>
        public ImageInspection(CameraInfo camInfo, double resolutionX, double resolutionY, LengthMeas lengthMeas)
        {
            this.CamInfo = camInfo;
			this.SetResolution(resolutionX, resolutionY);
            this._lengthMeas = lengthMeas;
        }

        public Recipe Recipe { get; set; }
        public List<InspKandoParam> InspParam { get; set; }

        public void Start()
        {
            _continuNGCounter = 0;
            _contNGAfterCounter = 0;
        }

        /// <summary>
        /// 連続NG発生数
        /// </summary>
        private int _continuNGCounter = 0;
        /// <summary>
        /// 連続NG発生後の取込数
        /// </summary>
        private int _contNGAfterCounter = 0;

        public bool Run(HObject imageOrg, HObject imageTarget, HObject imageInspScale, out List<ImageResultData> resultData)
        {
            resultData = new List<ImageResultData>();

			int ngCount = 0;
            //int tmpNo = 0;
            try
            {
				InspKandoParam insp;
				for(int i=0; i<this.InspParam.Count; i++)
                {
					insp = this.InspParam[(int)this.InspOrder[i]];

                    bool run = true;

                    int thresMin = 0;
                    int thresMax = 255;
					if (insp.inspID == AppData.InspID.明大 || insp.inspID == AppData.InspID.明中 || insp.inspID == AppData.InspID.明小)
					{
						run = Utility.GetBrightThreshold(this.GetBrightBaseValue(), insp.Threshold, ref thresMin, ref thresMax);
					}
					else if (insp.inspID == AppData.InspID.暗大 || insp.inspID == AppData.InspID.暗中 || insp.inspID == AppData.InspID.暗小)
					{
						run = Utility.GetDarkThreshold(this.GetDarkBaseValue(), insp.Threshold, ref thresMin, ref thresMax);
                    }
                    //Debug.WriteLine(string.Format("{0} : {1} , {2}", insp.inspID.ToString(), thresMin, thresMax));
                    //Debug.WriteLine(insp.inspID);
                    if (run == true)
                    {
                        ngCount += this.Blob(imageOrg, imageTarget, imageInspScale, thresMin, thresMax, insp, ref resultData);
						//if (CamInfo.CamNo == AppData.CamID.cam1)
						//    Debug.WriteLine(string.Format("cam={0} i={1} ngcount={2}", CamInfo.CamNo, i, ngCount));
                    }
					if (SystemParam.GetInstance().InspFunc_CountNgMax <= ngCount)
					{
						break;
					}
                }
                //if (ngCount == 0)
                //{
                //    double ave = this.GetAverageValue(imageOrg);
                //    this.AddAverageData(ave);
                //}
            }
            catch(HalconException exc)
            {
				string ErrStr = string.Format("ImageInspection.Run() exc = {0}", exc.Message);
				LogingDll.Loging_SetLogString(ErrStr);
				Debug.WriteLine(ErrStr);
				throw exc;
            }

            if (SystemParam.GetInstance().ContinuNGEnable == true)
            {
                //NGキャンセル中でない？
                if (_contNGAfterCounter == 0)
                {
                    //NG発生したらカウントUPする
                    if (ngCount > 0)
                        _continuNGCounter++;
                    else
                        _continuNGCounter = 0;
                }

                //連続NG枚数に達したか？
                if (_continuNGCounter >= SystemParam.GetInstance().ContinuNGJudgeCount)
                {
                    _contNGAfterCounter++;
                    //キャンセル枚数以内か？
                    if (_contNGAfterCounter <= SystemParam.GetInstance().ContinuNGAfterCancelCount)
                    {
                        //NGをキャンセルする
                        resultData.Clear();
                        ngCount = 0;
                    }
                    else
                    {
                        //リセット
                        _contNGAfterCounter = 0;
                        _continuNGCounter = 0;
                    }
                }
            }

			return (ngCount == 0) ? true : false;
        }

        private int Blob(HObject imageOrg, HObject imageTarget, HObject imageInspScale, int thresMin, int thresMax, InspKandoParam inspParam, ref List<ImageResultData> resultData)
        {
            const int maxTmp = 200;
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[maxTmp];
			HObject  InspArea = new HObject();

            HTuple imgOrgWidth, imgOrgHeight;
			HTuple imgTargetWidth, imgTargetHeight;
            HTuple imgScaleWidth, imgScaleHeight;
            HTuple htCount;
            HTuple htArea, htCenterY, htCenterX;    //面積、中心XY
            HTuple htRow1, htCol1, htRow2, htCol2;  //縦、横
            HTuple htSortIndex;
            //HTuple htFeatures;
            //HTuple htMin, htMax;
            HTuple htNumber;

            HTuple htChannel;

            double pixArea = inspParam.Area / this._onePixAreaResoluton;
            double pixWidth = inspParam.LengthH / this._resolutionX;
            double pixHeight = inspParam.LengthV / this._resolutionY;

            int realCount = 0;
            int ngCnt = 0;
            int noTmp = 0;

            if (this.InspWidthStartPix >= this.InspWidthEndPix)
            {
                return realCount;
            }

            try
            {

                HOperatorSet.GetImageSize(imageOrg, out imgOrgWidth, out imgOrgHeight);
                HOperatorSet.GetImageSize(imageTarget, out imgTargetWidth, out imgTargetHeight);
                HOperatorSet.GetImageSize(imageInspScale, out imgScaleWidth, out imgScaleHeight);

                //Console.WriteLine("Height = {0} {1} {2}", imgOrgHeight.I, imgTargetHeight.I, imgScaleHeight.I);

                int startHeight, endHeight, underHeight;
                if (SystemParam.GetInstance().GetImageHeightArea(imgTargetHeight.I, out startHeight, out endHeight, out underHeight) == false)
                {
                    HOperatorSet.GenRectangle1(out InspArea, 0, this.InspWidthStartPix, imgTargetHeight, this.InspWidthEndPix);
                }
                else
                {
                    HOperatorSet.GenRectangle1(out InspArea, startHeight, this.InspWidthStartPix, endHeight, this.InspWidthEndPix);
                }

                HOperatorSet.CountChannels(imageOrg, out htChannel);
                if (htChannel.I == 3)
                {
                    BlobColor(imageInspScale, InspArea, thresMin, thresMax, out OTemp[++noTmp]);
                }
                else
                {
                    HOperatorSet.ReduceDomain(imageTarget, InspArea, out OTemp[++noTmp]);
                    HOperatorSet.Threshold(OTemp[noTmp], out OTemp[++noTmp], thresMin, thresMax);
                }


                HOperatorSet.ClosingCircle(OTemp[noTmp], out OTemp[++noTmp], SystemParam.GetInstance().InspFunc_BlobClosingCircle);
    			HOperatorSet.OpeningCircle(OTemp[noTmp], out OTemp[++noTmp], SystemParam.GetInstance().InspFunc_BlobOpeningCircle);

                //リージョンが存在するかチェックする
                HOperatorSet.SelectShape(OTemp[noTmp], out OTemp[++noTmp], "area", "and", 1, "max");
                HOperatorSet.CountObj(OTemp[noTmp], out htNumber);
                if (htNumber.I != 0)
                {
                    //リージョンを分割する
                    HOperatorSet.Connection(OTemp[noTmp], out OTemp[++noTmp]);

					//有効Pix数のブロブを抽出する
					HOperatorSet.SelectShape(OTemp[noTmp], out OTemp[++noTmp], "area", "and", SystemParam.GetInstance().InspFunc_BlobSelectArea, "max");

                    //データを取得する（面積、中心XY、縦、横）
                    HOperatorSet.AreaCenter(OTemp[noTmp], out htArea, out htCenterY, out htCenterX);
                    HOperatorSet.SmallestRectangle1(OTemp[noTmp], out htRow1, out htCol1, out htRow2, out htCol2);

                    //ソートする（面積の小さい順・・・データ格納時に大きい順にしている）
                    HOperatorSet.TupleSortIndex(htArea, out htSortIndex);
                    //データの数
                    HOperatorSet.TupleLength(htSortIndex, out htCount);
                    //NG処理できる最大数を超えた？
                    ngCnt = htCount.I;

                    LogingDll.Loging_SetLogString(string.Format("cam = {0} ngCnt = {1}", CamInfo.CamNo.ToString(), ngCnt.ToString()));

                    //データを格納する
                    for (int i = (ngCnt - 1); 0 <= i; i--)
                    {
                        if (SystemParam.GetInstance().InspFunc_CountNgMax <= realCount) break;

                        //Index
                        int idx = htSortIndex[i];
                        //Data
                        double area = htArea[idx].D;
                        double width = htCol2[idx].D - htCol1[idx].D;
                        double height = htRow2[idx].D - htRow1[idx].D;
                        double x = htCenterX[idx].D;
						double y = htCenterY[idx].D;

                        if (this.HOverlopExceptMin < x && x < this.HOverlopExceptMax)
                        {
                            //重複除外
                            //continue;
                        }

                        //実数値に変換
                        double startPos = _lengthMeas.TotalStartLength;
                        double endPos = _lengthMeas.TotalLength;
                        double realX = (x * this._resolutionX);
                        double realY = (((y - CamInfo.OverLapLines) + _lengthMeas.TotalStartPixel) * this._resolutionY);
                        double realArea = area * this._onePixAreaResoluton;
                        double realWidth = width * this._resolutionX;

                        double omoteSpeed = Recipe.CamRealSpeedValue;
                        double uraSpeed = Recipe.CamRealSpeedValueUra;
                        if (Recipe.UseCommonCamRealSpeed == true)
                        {
                            omoteSpeed = SystemParam.GetInstance().Common_RecipeRealSpeedOmote;
                            uraSpeed = SystemParam.GetInstance().Common_RecipeRealSpeedUra;
                        }
                        double realSpeed = (this.CamInfo.CamNo == AppData.CamID.cam1) ? omoteSpeed : uraSpeed;


                        double systemCamSpeed = (this.CamInfo.CamNo == AppData.CamID.cam1) ? SystemParam.GetInstance().CamSpeed : SystemParam.GetInstance().CamSpeedUra;
                        double realHeight = height * (this._resolutionY * (realSpeed / systemCamSpeed));

						realX += CamInfo.OffsetX;
						//realY += CamInfo.OffsetY;	//下で行う	

                        //四捨五入
                        realX = Utility.ToRound(realX);
                        realY = Utility.ToRound(realY);
                        realArea = Utility.ToRound(realArea);
                        realWidth = Utility.ToRound(realWidth);
                        realHeight = Utility.ToRound(realHeight);

                        //Mask位置を基準として、X座標値を変換する
                        double MaskShift;
                        if (Recipe.CommonInspAreaEnable == false)
                            MaskShift = Recipe.InspParam[(int)this.CamInfo.CamSide].MaskShift;
                        else
                            MaskShift = SystemParam.GetInstance().InspArea_CmnMaskShift[(int)this.CamInfo.CamSide];
                        double mask = MaskShift - this.BasePoint;
                        realX = realX - mask;

                        if (realX < 0.0)
                        {
                            //マスクされている箇所
                            continue;
                        }

                        //ゾーン番号
                        int zone = -1;
                        double pos = 0.0;

						if (this.NGPositionMode == 0)
						{
							for (int zNo = Recipe.Partition - 1; 0 <= zNo; zNo--)
							{
								pos += Recipe.InspParam[(int)this.CamInfo.CamSide].Zone[zNo];
								if (realX < pos)
								{
									zone = zNo;
									realX = Recipe.InspParam[(int)this.CamInfo.CamSide].Width - realX;
									break;
								}
							}
						}
						else
						{
							for (int zNo = 0; zNo < Recipe.Partition; zNo++)
							{
								pos += Recipe.InspParam[(int)this.CamInfo.CamSide].Zone[zNo];
								if (realX < pos)
								{
									zone = zNo;
									break;
								}
							}
						}

						if (zone == -1)
                        {
                            //ゾーン内にない
                            continue;
                        }

                        if (realY < startPos || endPos < realY)
                        {
                            //抽出範囲外
                            //continue;
                        }

                        if ((realArea < inspParam.Area) && (realWidth < inspParam.LengthH) && (realHeight < inspParam.LengthV))
                        {
                            //判定値より小さい
                            continue;
                        }

						realY = realY + CamInfo.OffsetY;
						realY = Utility.ToRound(realY);

                        ImageResultData res = new ImageResultData(inspParam.inspID, startPos, endPos, realArea, realWidth, realHeight, realX, realY, (AppData.ZoneID)zone, this.CamInfo);
                        //res.CropNgImage(x + NgCropOffsetXpix, y + NgCropOffsetYpix, ImageHorizontalConnect, this.CropImageWidth, this.CropImageHeight, this.ScaleImageWidth, this.ScaleImageHeight, ref camPos);

                        HTuple htw1, hth1, htw2, hth2;
                        HOperatorSet.GetImageSize(imageTarget, out htw1, out hth1);
                        HOperatorSet.GetImageSize(imageOrg, out htw2, out hth2);
                        HObject moveImage;
                        HTuple htIdentity, htTranslate;
                        HOperatorSet.HomMat2dIdentity(out htIdentity);
                        HOperatorSet.HomMat2dTranslate(htIdentity, hth1.I - hth2.I, 0, out htTranslate);
                        HOperatorSet.AffineTransImageSize(imageOrg, out moveImage, htTranslate, "constant", htw1, hth1);

                        HObject hoConcatImage;
                        HOperatorSet.ConcatObj(imageTarget, moveImage, out hoConcatImage);
                        HObject dmy;
                        HOperatorSet.CopyObj(hoConcatImage, out dmy, 1, -1);
                        hoConcatImage.Dispose();
                        HOperatorSet.ConcatObj(dmy, imageInspScale, out hoConcatImage);
                        dmy.Dispose();
                        res.CropNgImage(x, y, hoConcatImage, this.CropImageWidth, this.CropImageHeight, this.ScaleImageWidth, this.ScaleImageHeight, hoConcatImage, this.PareOffsetXpix, this.PareOffsetYpix, this.ImageCon);
						resultData.Add(res);
						realCount++;
                        hoConcatImage.Dispose();
                        moveImage.Dispose();
                        //Debug.WriteLine("camNo:{0} realCount={1}", this.CamInfo.CamNo.ToString(), realCount.ToString());
                    }
                }
            }
            catch(Exception exc)
            {
				string ErrStr = string.Format("ImageInspection.Blob() exc = {0}", exc.Message);
				LogingDll.Loging_SetLogString(ErrStr);
				Debug.WriteLine(ErrStr);
				throw exc;
            }
            finally
            {
                foreach (HObject tmp in OTemp)
                {
                    if (tmp != null)
                    {
                        tmp.Dispose();
                    }
                }
				if (InspArea!=null)
				{
					InspArea.Dispose();
				}
            }

            //Debug.WriteLine(string.Format("cam = {0} ngCnt={1} realCount={2}", CamInfo.CamNo.ToString(), ngCnt.ToString(), realCount.ToString()));

            return realCount;
        }





        private void BlobColor(HObject inspScaleImage, HObject InspArea, int colorKandoMin, int colorKandoMax, out HObject outRegion)
        {
            HOperatorSet.GenEmptyObj(out outRegion);

            HObject hoImageReduced;
            HObject hoSelectedObj;
            HObject hoThresRegion;
            HObject hoConnectedRegions;
            HObject hoSelectedRegions;
            HObject hoAllRegion;
            HObject hoRegionUnion1;
            //HObject hoRegionOpening;
            HObject dmy;
            HOperatorSet.GenEmptyObj(out hoImageReduced);
            HOperatorSet.GenEmptyObj(out hoSelectedObj);
            HOperatorSet.GenEmptyObj(out hoThresRegion);
            HOperatorSet.GenEmptyObj(out hoConnectedRegions);
            HOperatorSet.GenEmptyObj(out hoSelectedRegions);
            HOperatorSet.GenEmptyObj(out hoAllRegion);
            HOperatorSet.GenEmptyObj(out hoRegionUnion1);
            //HOperatorSet.GenEmptyObj(out hoRegionOpening);
            HOperatorSet.GenEmptyObj(out dmy);

            HTuple htChCnt;
            int chCnt;

            try
            {
                HOperatorSet.CountObj(inspScaleImage, out htChCnt);
                chCnt = htChCnt.I;

                hoImageReduced.Dispose();
                HOperatorSet.ReduceDomain(inspScaleImage, InspArea, out hoImageReduced);

                HOperatorSet.GenEmptyObj(out hoAllRegion);
                for (int chNo = 0; chNo < chCnt; chNo++)
                {
                    hoSelectedObj.Dispose();
                    HOperatorSet.SelectObj(hoImageReduced, out hoSelectedObj, chNo + 1);

                    hoThresRegion.Dispose();
                    HOperatorSet.GenEmptyObj(out hoThresRegion);
                    if (SystemParam.GetInstance().ColorCamInspImage[chNo] == true)
                        HOperatorSet.Threshold(hoSelectedObj, out hoThresRegion, colorKandoMin, colorKandoMax);

                    hoConnectedRegions.Dispose();
                    HOperatorSet.Connection(hoThresRegion, out hoConnectedRegions);
                    hoSelectedRegions.Dispose();
                    HOperatorSet.SelectShape(hoConnectedRegions, out hoSelectedRegions, "area", "and", SystemParam.GetInstance().InspFunc_BlobSelectArea, new HTuple("max"));
                    hoRegionUnion1.Dispose();
                    HOperatorSet.Union1(hoSelectedRegions, out hoRegionUnion1);

                    dmy.Dispose();
                    HOperatorSet.CopyObj(hoAllRegion, out dmy, 1, -1);
                    hoAllRegion.Dispose();
                    HOperatorSet.Union2(dmy, hoRegionUnion1, out hoAllRegion);

                    HTuple htCnt;
                    HOperatorSet.CountObj(hoAllRegion, out htCnt);
                }
                outRegion.Dispose();
                HOperatorSet.Union1(hoAllRegion, out outRegion);
                //hoRegionOpening.Dispose();
                //if (this.InspBlobOpeningCircleEnable == true)
                //    HOperatorSet.OpeningCircle(hoRegionUnion1, out hoRegionOpening, this.InspBlobOpeningCircle);
                //else
                //    HOperatorSet.CopyObj(hoRegionUnion1, out hoRegionOpening, 1, -1);
                //outRegion.Dispose();
                //HOperatorSet.Connection(hoRegionOpening, out outRegion);
            }
            catch(HalconException exc)
            {
                throw exc;
            }
            finally
            {
                hoImageReduced.Dispose();
                hoSelectedObj.Dispose();
                hoThresRegion.Dispose();
                hoConnectedRegions.Dispose();
                hoSelectedRegions.Dispose();
                hoAllRegion.Dispose();
                hoRegionUnion1.Dispose();
                //hoRegionOpening.Dispose();
                dmy.Dispose();
            }
        }













        const int KANDO_BASE = 128;
		const int KANDO_AVE_COUNT = 10;
		public int _kandoBaseValue;
		public bool AutoKandoBrightEnable { get; set; }
		public bool AutoKandoDarkEnable { get; set; }
		public int AutoKandoLimit { get; set; }
		/// <summary>
		/// 平均値のリストデータ
		/// </summary>
		private List<double> _imageAverages = new List<double>();
		/// <summary>
		/// 
		/// </summary>
		public void ResetAverageDatas()
		{
			lock (this._imageAverages)
			{
				this._imageAverages.Clear();
				for (int i = 0; i < KANDO_AVE_COUNT; i++)
				{
					_imageAverages.Add(KANDO_BASE);
				}
				this._kandoBaseValue = KANDO_BASE;

				if (this.CamInfo.CamNo == AppData.CamID.cam1)
					Debug.WriteLine("ResetAverageData()");
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="aveValue"></param>
		public void AddAverageData(double aveValue)
		{
			lock (this._imageAverages)
			{
				int cnt;
				//最新の平均値を追加する
				this._imageAverages.Add(aveValue);
				//
				cnt = this._imageAverages.Count;
				if (this._imageAverages.Count > KANDO_AVE_COUNT)
				{
					//古いデータを削除する
					this._imageAverages.RemoveRange(0, (cnt - KANDO_AVE_COUNT));
				}
				//if (CamInfo.CamNo == AppData.CamID.cam4)
				//{
				//    foreach (double d in _imageAverages)
				//    {
				//        Debug.Write(d.ToString("F1") + " ");
				//    }
				//    Debug.WriteLine("");
				//}

				double total = 0;
				cnt = _imageAverages.Count;
				for (int i = 0; i < cnt; i++)
				{
					total += _imageAverages[i];
				}
				this._kandoBaseValue = (int)(total / cnt);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int GetBrightBaseValue()
		{
			int value = KANDO_BASE;
			if (this.AutoKandoBrightEnable == true)
			{
				if (this.CheckKandoLimit() == true)
					value = this._kandoBaseValue;
			}
			return value;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int GetDarkBaseValue()
		{
			int value = KANDO_BASE;
			if (this.AutoKandoDarkEnable == true)
			{
				if (this.CheckKandoLimit() == true)
					value = this._kandoBaseValue;
			}
			return value;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private bool CheckKandoLimit()
		{
			if (this._kandoBaseValue > (KANDO_BASE + this.AutoKandoLimit))
			{
				string ErrStr = string.Format("cam={0} CheckKandoLimit() rtn = false(Bright) _kandoBaseValue = {1}", CamInfo.CamNo.ToString(), this._kandoBaseValue);
				LogingDll.Loging_SetLogString(ErrStr);
				return false;
			}
			if (this._kandoBaseValue < (KANDO_BASE - this.AutoKandoLimit))
			{
				string ErrStr = string.Format("cam={0} CheckKandoLimit() rtn = false(Dark) _kandoBaseValue = {1}", CamInfo.CamNo.ToString(), this._kandoBaseValue);
				LogingDll.Loging_SetLogString(ErrStr);
				return false;
			}
			return true;
		}

		public int MinMaxAveStartPos { get; set; }
		public int MinMaxAveEndPos { get; set; }

        public int LeftMaskWidthPix { get; set; }
        public int RightMaskWidthPix { get; set; }

		/// <summary>
		/// イメージから、Gray値の　最小・最大・平均　を算出する
		/// </summary>
		/// <param name="images"></param>
		/// <param name="camImgDatas"></param>
		public double GetAverageValue(HObject imageOrg)
		{
			const int tmpMax = 20;
			// Stack for temporary objects 
			HObject[] OTemp = new HObject[tmpMax];

			HTuple width, height;
            HTuple htMean, htDeviation;

            double average, deviation;
			average = KANDO_BASE;

			int tmpNo = 0;
			try
			{
                lock (imageOrg)
                {
                    //指定部分だけを有効にする
                    HOperatorSet.GetImageSize(imageOrg, out width, out height);
                    int w = width.I - 1;
                    int left;
                    int right;
                    if (MinMaxAveStartPos <= w && MinMaxAveEndPos >= 0)
                    {
                        left = MinMaxAveStartPos + LeftMaskWidthPix;
                        right = MinMaxAveEndPos - RightMaskWidthPix;
                        HOperatorSet.GenRectangle1(out OTemp[++tmpNo], 0, left, 50, (left < right) ? right : left + 1);
                        HOperatorSet.Intensity(OTemp[tmpNo], imageOrg, out htMean, out htDeviation);
                        average = htMean.D;
                        deviation = htDeviation.D;
                    }
                }
			}
			catch (HalconException exc)
			{
				string ErrStr = string.Format("ImageInspection.GetAverageValue() err = {0}", exc.Message);
				LogingDllWrap.LogingDll.Loging_SetLogString(ErrStr);
				System.Diagnostics.Debug.WriteLine(ErrStr);
				throw exc;
			}
			finally
			{
				for (int i = 0; i < OTemp.Length; i++)
				{
					UtilityImage.ClearHalconObject(ref OTemp[i]);
				}
			}

			return average;
		}
	}
}
