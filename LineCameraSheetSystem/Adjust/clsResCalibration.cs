using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HalconDotNet;

namespace Adjustment
{
    class clsResCalibration
    {
        public class Blobs
        {
            public double Area
            {
                get;
                private set;
            }

            public double GravityX
            {
                get;
                private set;
            }

            public double GravityY
            {
                get;
                private set;
            }

            public Blobs(double area, double gx, double gy)
            {
                Area = area;
                GravityX = gx;
                GravityY = gy;
            }
        }

        public class Result : IDisposable
        {
            public bool Success
            {
                get;
                set;
            }

            public string ResultString
            {
                get;
                set;
            }

            /// <summary>
            /// ブロブリスト
            /// </summary>
            List<Blobs> _lstBlobs = new List<Blobs>();
            /// <summary>
            /// ブロブリスト
            /// </summary>
            public List<Blobs> Blobs
            {
                get
                {
                    return _lstBlobs;
                }
            }

            HObject _hoBlobs = null;
            public HObject RegionBlobs
            {
                get
                {
                    return _hoBlobs;
                }
            }

            public bool CopyBlobs(HObject blob)
            {
                try
                {
                    if (_hoBlobs != null)
                        _hoBlobs.Dispose();

                    HOperatorSet.CopyObj(blob, out _hoBlobs, 1, -1 );
                }
                catch (HOperatorException)
                {
                    return false;
                }
                return true;
            }

            public void Dispose()
            {
                if (_hoBlobs != null)
                    _hoBlobs.Dispose();
            }

            /// <summary>
            /// 解像度横
            /// </summary>
            public double ResolutionHorz
            {
                get;
                set;
            }

            /// <summary>
            /// 解像度縦
            /// </summary>
            public double ResolutionVert
            {
                get;
                set;
            }

            /// <summary>
            /// 角度（ラジアン）
            /// </summary>
            public double AngleRad
            {
                get;
                set;
            }

            /// <summary>
            /// 角度（度）
            /// </summary>
            public double AngleDeg
            {
                get
                {
                    return AngleRad * 180 / Math.PI;
                }
            }
        }

        public class Parameters
        {
            int _iThresholdLow = 0;
            public int ThresholdLow
            {
                get
                {
                    return _iThresholdLow;
                }
                set
                {
                    if (value < 0 || value > 255)
                        return;
                    _iThresholdLow = value;
                }
            }

            int _iThresholdHigh = 128;
            public int ThresholdHigh
            {
                get
                {
                    return _iThresholdHigh;
                }
                set
                {
                    if (value < 0 || value > 255)
                        return;
                    _iThresholdHigh = value;
                }
            }

            double _dOpeningRadius = 0.5d;
            public double OpeningRadius
            {
                get
                {
                    return _dOpeningRadius;
                }
                set
                {
                    if (value < 0.5)
                        return;
                    _dOpeningRadius = value;
                }
            }

            double _dLimitMaxHorzPix = 100.0;
            public double LimitMaxHorzPix
            {
                get
                {
                    return _dLimitMaxHorzPix;
                }
                set
                {
                    if (value <= 0d)
                        return;
                    _dLimitMaxHorzPix = value;
                }
            }
            double _dLimitMinHorzPix = 1.0;
            public double LimitMinHorzPix
            {
                get
                {
                    return _dLimitMinHorzPix;
                }
                set
                {
                    if (value <= 0d)
                        return;
                    _dLimitMinHorzPix = value;
                }
            }

            double _dLimitMaxVertPix = 100.0;
            public double LimitMaxVertPix
            {
                get
                {
                    return _dLimitMaxVertPix;
                }
                set
                {
                    if (value <= 0d)
                        return;
                    _dLimitMaxVertPix = value;
                }
            }
            double _dLimitMinVertPix = 1.0;
            public double LimitMinVertPix
            {
                get
                {
                    return _dLimitMinVertPix;
                }
                set
                {
                    if (value <= 0d)
                        return;
                    _dLimitMinVertPix = value;
                }
            }



            double _dRealHorzMili = 1.0d;
            public double RealHorzMili
            {
                get
                {
                    return _dRealHorzMili;
                }
                set
                {
                    if (value <= 0)
                        return;
                    _dRealHorzMili = value;
                }
            }

            double _dRealVertMili = 1.0d;
            public double RealVertMili
            {
                get
                {
                    return _dRealVertMili;
                }
                set
                {
                    if (value <= 0)
                        return;
                    _dRealVertMili = value;
                }
            }
        }

        public bool Run(HObject img, Parameters param, out Result result )
        {
            result = new Result();
            HObject[] ahoObj = new HObject[10];

            HTuple htImgWidth, htImgHeight;
            HTuple htNumber, htArea, htRows, htCols, htSortIndices;
            HTuple htRow1, htCol1, htRow2, htCol2;

            List<Blobs> lstBlobs = new List<Blobs>();
            try
            {
                HOperatorSet.GetImageSize(img, out htImgWidth, out htImgHeight);

                HOperatorSet.Threshold(img, out ahoObj[0], param.ThresholdLow, param.ThresholdHigh);
                HOperatorSet.OpeningCircle(ahoObj[0], out ahoObj[1], param.OpeningRadius);
                HOperatorSet.Connection(ahoObj[1], out ahoObj[2]);
                HOperatorSet.SelectShape(ahoObj[2], out ahoObj[3], "area", "and", 1.0, 9999999.9);

                HOperatorSet.CountObj(ahoObj[3], out htNumber);

                if (htNumber.I > 0)
                {
                    HOperatorSet.AreaCenter(ahoObj[3], out htArea, out htRows, out htCols);
                    HOperatorSet.SmallestRectangle1(ahoObj[3], out htRow1, out htCol1, out htRow2, out htCol2);
                    HOperatorSet.TupleSortIndex(htArea, out htSortIndices);

                    HOperatorSet.GenEmptyObj(out ahoObj[5]);
                    for (int i = htSortIndices.TupleLength() - 1; i >= 0; i--)
                    {
                        int iIndex = htSortIndices[i].I;
                        double dWidth = htCol2[iIndex].D - htCol1[iIndex].D + 1d;
                        double dHeight = htRow2[iIndex].D - htRow1[iIndex].D + 1d;
                        if (dWidth >= param.LimitMinHorzPix
                            && dWidth <= param.LimitMaxHorzPix
                            && dHeight >= param.LimitMinVertPix
                            && dHeight <= param.LimitMaxVertPix)
                        {
                            lstBlobs.Add(new Blobs(htArea[iIndex].D, htCols[iIndex].D, htRows[iIndex].D));

                            if (ahoObj[4] != null)
                                ahoObj[4].Dispose();
                            HOperatorSet.SelectObj(ahoObj[3], out ahoObj[4], iIndex + 1);
                            HOperatorSet.ConcatObj(ahoObj[5], ahoObj[4], out ahoObj[6]);
                            ahoObj[5].Dispose();
                            ahoObj[5] = ahoObj[6];
                        }

                        if (lstBlobs.Count == 3)
                            break;
                    }

                    result.CopyBlobs(ahoObj[5]);

                    //３つ見つけた場合、3点の位置関係を求め、水平、垂直の長さを求める
                    if (lstBlobs.Count == 3)
                    {
                        List<Blobs> lstBlobDest;
                        // どの３点か位置を検出する
                        posCheck(lstBlobs, out lstBlobDest, htImgWidth.I, htImgHeight.I);
                        // 長さと角度を求める
                        double dHorzPix = Math.Sqrt(Math.Pow(lstBlobDest[0].GravityX - lstBlobDest[1].GravityX, 2d) + Math.Pow(lstBlobDest[0].GravityY - lstBlobDest[1].GravityY, 2d));
                        double dVertPix = Math.Sqrt(Math.Pow(lstBlobDest[1].GravityX - lstBlobDest[2].GravityX, 2d) + Math.Pow(lstBlobDest[1].GravityY - lstBlobDest[2].GravityY, 2d));
                        double dAngleRad = Math.Atan2(lstBlobDest[0].GravityY - lstBlobDest[1].GravityY, lstBlobDest[0].GravityX - lstBlobDest[1].GravityX);
                        result.ResolutionHorz = param.RealHorzMili / dHorzPix;
                        result.ResolutionVert = param.RealVertMili / dVertPix;
                        result.AngleRad = dAngleRad;
                        result.Blobs.AddRange(lstBlobDest);
                        result.Success = true;
                        result.ResultString = "正常終了";
                    }
                    else
                    {
                        result.ResultString = "検出不良";
                        return false;
                    }
                }
                else
                {
                    result.ResultString = "検出不良";
                }
                return true;
            }
            catch (HOperatorException)
            {
                result.ResultString = "ﾗｲﾌﾞﾗﾘ例外";
            }
            finally
            {
                for (int i = 0; i < ahoObj.Length; i++)
                {
                    if (ahoObj[i] != null)
                        ahoObj[i].Dispose();
                }
            }
            return true;
        }

        private bool posCheck(List<Blobs> lstBlobSrc, out List<Blobs> lstBlobDest, int iWidth, int iHeight )
        {
            lstBlobDest = new List<Blobs>();

            double[] aiPosX = new double[] { 0, 0, iWidth, iWidth };
            double[] aiPosY = new double[] { 0, iHeight, 0, iHeight };

            double [,] dLen = new double[4,3];
            for (int n = 0; n < 4; n++)
            {
                for (int i = 0; i < lstBlobSrc.Count; i++)
                {
                    dLen[n, i] = Math.Sqrt(Math.Pow( aiPosX[n] - lstBlobSrc[i].GravityX, 2d) + Math.Pow( aiPosY[n] - lstBlobSrc[i].GravityY, 2d));
                }
            }

            //合計が一番長くなるところにはデータが存在しない
            double dMaxLen = 0;
            int iMaxIndex = -1;
            for (int n = 0; n < 4; n++)
            {
                double dSumLen = 0;
                for (int i = 0; i < 3; i++)
                {
                    dSumLen += dLen[n, i];
                }
                if (dMaxLen < dSumLen)
                {
                    iMaxIndex = n;
                    dMaxLen = dSumLen;
                }
            }

            // 無いところを基準に３点の情報を配列にソートする
            int[,] aiLenIndex = new int[4, 3] { { 1, 3, 2 }, { 0, 2, 3 }, { 3, 1, 0 }, { 2, 0, 1 } };
            for( int i = 0 ; i < 3 ; i++ )
            {
                int iMinIndex = -1;
                double dMin = double.MaxValue;
                int index = aiLenIndex[iMaxIndex, i];
                for (int n = 0; n < 3; n++)
                {
                    if (dMin > dLen[index, n])
                    {
                        dMin = dLen[index, n];
                        iMinIndex = n;
                    }
                }
                lstBlobDest.Add(lstBlobSrc[iMinIndex]);
            }
            return true;
        }
    }
}
