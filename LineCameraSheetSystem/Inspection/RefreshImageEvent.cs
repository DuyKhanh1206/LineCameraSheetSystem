using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using LineCameraSheetSystem;
using HalconDotNet;

namespace InspectionNameSpace
{
    public class RefreshImageEvent
    {
        #region ■イベントハンドラ
        public delegate void RefreshImageEventHandler(object sender, RefreshImageEventArgs e);
        public event RefreshImageEventHandler OnEventRefreshImage;
		public event RefreshImageEventHandler OnBuffEventRefreshImage;
        #endregion

        #region ■メンバ変数
        public bool ClearAllOnEvent { get; set; }
        public bool ChangeOnEvent { get; set; }

        public int[] MinMaxAveStartPos;
        public int[] MinMaxAveEndPos;
        public int[] LeftMaskWidthPix;
        public int[] RightMaskWidthPix;

        public int[] MinMaxAveStartPosNowRecipe;
        public int[] MinMaxAveEndPosNowRecipe;
        public int[] LeftMaskWidthPixNowRecipe;
        public int[] RightMaskWidthPixNowRecipe;

        public int ImageDataGetLine { get; set; }
        #endregion

        /// <summary>コンストラクタ</summary>
        public RefreshImageEvent(List<CameraInfo> camInfos)
        {
			this.ClearAllOnEvent = false;
            this.ChangeOnEvent = false;
			MinMaxAveStartPos = new int[camInfos.Count];
			MinMaxAveEndPos = new int[camInfos.Count];
            LeftMaskWidthPix = new int[camInfos.Count];
            RightMaskWidthPix = new int[camInfos.Count];
            MinMaxAveStartPosNowRecipe = new int[camInfos.Count];
            MinMaxAveEndPosNowRecipe = new int[camInfos.Count];
            LeftMaskWidthPixNowRecipe = new int[camInfos.Count];
            RightMaskWidthPixNowRecipe = new int[camInfos.Count];
        }

        #region ■メソッド
        /// <summary>イベントを発行する</summary>
        public void Event(bool[] updown, bool[] enableCamera, List<CameraInfo> camInfos,
            HObject[] imageOrg, HObject[] imageTarget, HObject[] imageInspScale, List<int> brightKando, List<int> darkKando)
        {
			try
			{
				if (this.OnEventRefreshImage != null)
				{
                    //カメラ毎の　最小・最大・平均
                    List<ImageDatas> camDatas = new List<ImageDatas>();
                    CreateDatasColor(imageInspScale, ref camDatas);

                    List<double> minBuf = new List<double>();
					List<double> maxBuf = new List<double>();
					List<double> aveBuf = new List<double>();

					int brightK = 128, darkK = 128;
					//部位ごとの　最小・最大・平均
					List<ImageDatas> sideDatas = new List<ImageDatas>();
					foreach (AppData.SideID side in Enum.GetValues(typeof(AppData.SideID)))
					{
						bool kSet = false;
						foreach (CameraInfo cam in camInfos)
						{
							camDatas[(int)cam.CamNo].BrightKandoBase = brightKando[(int)cam.CamNo];
							camDatas[(int)cam.CamNo].DarkKandoBase = darkKando[(int)cam.CamNo];
							if (cam.Enabled == true && cam.CamSide == side && camDatas[(int)cam.CamNo].Enabled == true)
							{
								minBuf.Add(camDatas[(int)cam.CamNo].Minimum);
								maxBuf.Add(camDatas[(int)cam.CamNo].Maximum);
								aveBuf.Add(camDatas[(int)cam.CamNo].Average);
								if (kSet == false)
								{
									brightK = brightKando[(int)cam.CamNo];
									darkK = darkKando[(int)cam.CamNo];
									kSet = true;
								}
							}
						}
                        sideDatas.Add(new ImageDatas(true, (minBuf.Count != 0) ? minBuf.Min() : 0, (maxBuf.Count != 0) ? maxBuf.Max() : 0, (aveBuf.Count != 0) ? aveBuf.Average() : 0, brightK, darkK));
						minBuf.Clear();
						maxBuf.Clear();
						aveBuf.Clear();
					}

                    //imageOrg
                    //imageBuf
                    //imageTarget
					RefreshImageEventArgs args = new RefreshImageEventArgs(sideDatas, camDatas, updown);
					args.AddImages(imageOrg, imageTarget, imageInspScale);

					this.OnEventRefreshImage(this, args);
					args.Dispose();
					sideDatas.Clear();
					camDatas.Clear();
				}

				if (ClearAllOnEvent == true)
				{
					this.OnEventRefreshImage = null;
					ClearAllOnEvent = false;
				}
				if (ChangeOnEvent == true)
				{
					this.OnEventRefreshImage = OnBuffEventRefreshImage;
					ChangeOnEvent = false;
				}
			}
			catch (Exception exc)
			{
				throw exc;
			}
        }

        private void CreateDatasColor(HObject[] images, ref List<ImageDatas> camImgDatas)
        {
            const int tmpMax = 20;
            // Stack for temporary objects 
            HObject[] OTemp = new HObject[tmpMax];

            HTuple width, height;

            int tmpNo = 0;
            try
            {
                int camno = 0;
                foreach (HObject img in images)
                {
                    double min, max;
                    double average;
                    ImageDatas imageDatas;
                    //指定部分だけを有効にする
                    HOperatorSet.GetImageSize(img, out width, out height);
                    int w = width.I - 1;
                    int h = height.I - 1;
                    int left;
                    int right;
                    if (MinMaxAveStartPosNowRecipe[camno] <= w && MinMaxAveEndPosNowRecipe[camno] >= 0)
                    {
                        left = MinMaxAveStartPosNowRecipe[camno] + LeftMaskWidthPixNowRecipe[camno];
                        right = MinMaxAveEndPosNowRecipe[camno] - RightMaskWidthPixNowRecipe[camno];
                        HOperatorSet.GenRectangle1(out OTemp[++tmpNo],
                            0,
                            left,
                            h,
                            (left < right) ? right : left + 1);
                        //
                        MultiImageMinMaxAvg(OTemp[tmpNo], img, out min, out max, out average);
                        //
                        imageDatas = new ImageDatas(true, min, max, average, 128, 128);
                    }
                    else
                    {
                        imageDatas = new ImageDatas(false, 0.0, 0.0, 0.0, 128, 128);
                    }
                    camImgDatas.Add(imageDatas);
                    camno++;
                }
            }
            catch (HalconException exc)
            {
                string ErrStr = string.Format("RefreshImageEvent.CreateDatas() err = {0}", exc.Message);
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
        }

        private void MultiImageMinMaxAvg(HObject hoMask, HObject img, out double min, out double max, out double avg)
        {
            min = double.MaxValue;
            max = double.MinValue;
            avg = double.MinValue;

            HObject hoGrayImage;
            HObject hoImage1, hoImage2, hoImage3;
            HOperatorSet.GenEmptyObj(out hoGrayImage);
            HOperatorSet.GenEmptyObj(out hoImage1);
            HOperatorSet.GenEmptyObj(out hoImage2);
            HOperatorSet.GenEmptyObj(out hoImage3);

            HTuple htNumber;
            HTuple htMin, htMax, htRange;
            HTuple htMean, htDeviation;
            try
            {
                HOperatorSet.CountObj(img, out htNumber);
                if (SystemParam.GetInstance().ColorCamInspImage[0] == true)
                {
                    //Gray画像を使用する
                    hoGrayImage.Dispose();
                    HOperatorSet.SelectObj(img, out hoGrayImage, 1);
                }
                else
                {
                    //RGB画像を使用する
                    hoGrayImage.Dispose();
                    HOperatorSet.SelectObj(img, out hoGrayImage, 1);
                    HOperatorSet.SelectObj(img, out hoImage1, 2);
                    HOperatorSet.SelectObj(img, out hoImage2, 3);
                    HOperatorSet.SelectObj(img, out hoImage3, 4);
                    hoGrayImage.Dispose();
                    HOperatorSet.Rgb3ToGray(hoImage1, hoImage2, hoImage3, out hoGrayImage);
                }
                HOperatorSet.MinMaxGray(hoMask, hoGrayImage, 0, out htMin, out htMax, out htRange);
                HOperatorSet.Intensity(hoMask, hoGrayImage, out htMean, out htDeviation);
                min = htMin.D;
                max = htMax.D;
                avg = htMean.D;
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
        #endregion

        /// <summary>パラメータ</summary>
        public class RefreshImageEventArgs : EventArgs, IDisposable
        {
            /// <summary>イメージ</summary>
            public HObject[] ImageOriginals { get; private set; }
            public HObject[] ImageTargets { get; private set; }
            public HObject[] ImageInspScales { get; private set; }

            /// <summary>部位毎のイメージデータ</summary>
            public List<ImageDatas> SideDatas { get; private set; }

            /// <summary>カメラ毎のイメージデータ</summary>
            public List<ImageDatas> CameraDatas { get; private set; }

            public bool[] UpDownEnbled { get; private set; }

            /// <summary>コンストラクタ</summary>
            public RefreshImageEventArgs(List<ImageDatas> side, List<ImageDatas> camera, bool[] updown)
            {
                this.SideDatas = side;
                this.CameraDatas = camera;

                this.UpDownEnbled = updown;
            }

            /// <summary>デストラクタ</summary>
            public void Dispose()
            {
                if (ImageOriginals != null)
                {
                    for (int i = 0; i < ImageOriginals.Length; i++)
                    {
                        UtilityImage.ClearHalconObject(ref ImageOriginals[i]);
                    }
                }
                if (ImageTargets != null)
                {
                    for (int i = 0; i < ImageTargets.Length; i++)
                    {
                        UtilityImage.ClearHalconObject(ref ImageTargets[i]);
                    }
                }
                if (ImageInspScales != null)
                {
                    for (int i = 0; i < ImageInspScales.Length; i++)
                    {
                        UtilityImage.ClearHalconObject(ref ImageInspScales[i]);
                    }
                }
            }

            public void AddImages(HObject[] imageOrg, HObject[] imageTarget, HObject[] imageInspScale)
            {
                ImageOriginals = new HObject[imageOrg.Length];
                for (int i = 0; i < imageOrg.Length; i++)
                {
                    ImageOriginals[i] = UtilityImage.CopyHalconImage(imageOrg[i]);
                }
                ImageTargets = new HObject[imageTarget.Length];
                for (int i = 0; i < imageTarget.Length; i++)
                {
                    ImageTargets[i] = UtilityImage.CopyHalconImage(imageTarget[i]);
                }
                ImageInspScales = new HObject[imageInspScale.Length];
                for (int i = 0; i < imageInspScale.Length; i++)
                {
                    ImageInspScales[i] = UtilityImage.CopyHalconImage(imageInspScale[i]);
                }
            }
        }

        /// <summary>イメージ解析データ</summary>
        public class ImageDatas
        {
            public bool Enabled { get; private set; }
            public double Minimum { get; private set; }
            public double Maximum { get; private set; }
            public double Average { get; private set; }
            public int BrightKandoBase { get; set; }
            public int DarkKandoBase { get; set; }
            public ImageDatas(bool enabled, double min, double max, double ave, int brightK, int darkK)
            {
                this.Enabled = enabled;
                this.Minimum = min;
                this.Maximum = max;
                this.Average = ave;
                this.BrightKandoBase = brightK;
                this.DarkKandoBase = darkK;
            }
        }
    }
}
