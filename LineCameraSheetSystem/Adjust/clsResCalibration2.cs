using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using HalconDotNet;
using Fujita.Misc;

namespace Adjustment
{
    class clsResCalibration2
    {
        public class Blob : IPosition
        {
            double _dArea;
            public double Area
            {
                get;
                private set;
            }

            double _dGravityX;
            public double GravityX
            {
                get { return _dGravityX; }
                set { _dGravityX = value; }
            }

            double _dGravityY;
            public double GravityY
            {
                get { return _dGravityY; }
                set { _dGravityY = value; }
            }

            public Blob(double area, double gx, double gy)
            {
                _dArea = area;
                _dGravityX = gx;
                _dGravityY = gy;
            }

            public double XPos
            {
                get { return _dGravityX; }
            }

            public double YPos
            {
                get { return _dGravityY; }
            }
        }

        public class Result
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
            List<Blob> _lstBlobs = new List<Blob>();
            /// <summary>
            /// ブロブリスト
            /// </summary>
            public List<Blob> Blobs
            {
                get
                {
                    return _lstBlobs;
                }
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

            public void Clear()
            {
                Success = false;
                _lstBlobs.Clear();
                ResolutionHorz = 0;
                ResolutionVert = 0;
                AngleRad = 0;
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

            int _iThresholdHigh = 64;
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

            double _dClosingRadius = 0.5d;
            public double ClosingRadius
            {
                get
                {
                    return _dClosingRadius;
                }
                set
                {
                    if (value < 0.5)
                        return;
                    _dClosingRadius = value;
                }
            }

            double _dLimitMaxHorzPix = 300.0;
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

            double _dLimitMaxVertPix = 300.0;
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

            int _iInspectOffset = 0;
            public int InspectOffset
            {
                get
                {
                    return _iInspectOffset;
                }
                set
                {
                    if (value < 0)
                        value = 0;
                    _iInspectOffset = value;
                }
            }

            int _iInspectWidth = 0;
            public int InspectWidth
            {
                get
                {
                    return _iInspectWidth;
                }
                set
                {
                    if (value < 0)
                        return;
                    _iInspectWidth = value;
                }
            }
        }



        int _iLapSize;
        public int LapSize
        {
            get { return _iLapSize; }
            set
            {
                if (value < 0)
                    return;
                _iLapSize = value;
            }
        }

        clsImageBuffer _imgQue = new clsImageBuffer();
        clsLapList _posList = new clsLapList();

        public clsResCalibration2()
        {
            _imgQue.Initialize();
        }

        public bool Load(string sPath, string sSection)
        {
            IniFileAccess ifa = new IniFileAccess();

            _param.ThresholdLow = ifa.GetIni(sSection + "ResCalibration", "ThresholdLow", _param.ThresholdLow, sPath);
            _param.ThresholdHigh = ifa.GetIni(sSection + "ResCalibration", "ThresholdHigh", _param.ThresholdHigh, sPath);
            _param.InspectOffset = ifa.GetIni(sSection + "ResCalibration", "InspectOffset", _param.InspectOffset, sPath);
            _param.InspectWidth = ifa.GetIni(sSection + "ResCalibration", "InspectWidth", _param.InspectWidth, sPath);
            _param.LimitMaxHorzPix = ifa.GetIni(sSection + "ResCalibration", "LimitMaxHorzPix", _param.LimitMaxHorzPix, sPath);
            _param.LimitMaxVertPix = ifa.GetIni(sSection + "ResCalibration", "LimitMaxVertPix", _param.LimitMaxVertPix, sPath);
            _param.LimitMinHorzPix = ifa.GetIni(sSection + "ResCalibration", "LimitMinHorzPix", _param.LimitMinHorzPix, sPath);
            _param.LimitMinVertPix = ifa.GetIni(sSection + "ResCalibration", "LimitMinVertPix", _param.LimitMinVertPix, sPath);
            _param.ClosingRadius = ifa.GetIni(sSection + "ResCalibration", "ClosingRadius", _param.ClosingRadius, sPath);
            _param.OpeningRadius = ifa.GetIni(sSection + "ResCalibration", "OpeningRadius", _param.OpeningRadius, sPath);


            LapSize = ifa.GetIni(sSection + "ResCalibration", "LapSize", LapSize, sPath);
            CaptureLimit = ifa.GetIni(sSection + "ResCalibration", "CaptureLimit", CaptureLimit, sPath);

            _param.RealHorzMili = ifa.GetIni(sSection + "ResCalibration", "RealHorzMili", _param.RealHorzMili, sPath);
            _param.RealVertMili = ifa.GetIni(sSection + "ResCalibration", "RealVertMili", _param.RealVertMili, sPath);

            return true;
        }

        public bool Save(string sPath, string sSection)
        {
            IniFileAccess ifa = new IniFileAccess();

            ifa.SetIni(sSection + "ResCalibration", "ThresholdLow", _param.ThresholdLow, sPath);
            ifa.SetIni(sSection + "ResCalibration", "ThresholdHigh", _param.ThresholdHigh, sPath);
            ifa.SetIni(sSection + "ResCalibration", "InspectOffset", _param.InspectOffset, sPath);
            ifa.SetIni(sSection + "ResCalibration", "InspectWidth", _param.InspectWidth, sPath);
            ifa.SetIni(sSection + "ResCalibration", "LimitMaxHorzPix", _param.LimitMaxHorzPix, sPath);
            ifa.SetIni(sSection + "ResCalibration", "LimitMaxVertPix", _param.LimitMaxVertPix, sPath);
            ifa.SetIni(sSection + "ResCalibration", "LimitMinHorzPix", _param.LimitMinHorzPix, sPath);
            ifa.SetIni(sSection + "ResCalibration", "LimitMinVertPix", _param.LimitMinVertPix, sPath);

            ifa.SetIni(sSection + "ResCalibration", "LapSize", LapSize, sPath);
            ifa.SetIni(sSection + "ResCalibration", "CaptureLimit", CaptureLimit, sPath);

            ifa.SetIni(sSection + "ResCalibration", "RealHorzMili", _param.RealHorzMili, sPath);
            ifa.SetIni(sSection + "ResCalibration", "RealVertMili", _param.RealVertMili, sPath);

            ifa.SetIni(sSection + "ResCalibration", "ClosingRadius", _param.ClosingRadius, sPath);
            ifa.SetIni(sSection + "ResCalibration", "OpeningRadius", _param.OpeningRadius, sPath);


            return true;
        }

        Parameters _param = new Parameters();
        public Parameters Param
        {
            get { return _param; }
        }

        Result _result = new Result();
        public Result ResultData
        {
            get { return _result; }
        }

        public void ResultClear()
        {
            _result.Clear();
            _imgQue.Clear();
            _posList.Clear();

            _iTestTarget = 0;
        }

        public bool IsResultSuccess
        {
            get { return _result.Success; }
        }

        private int _iCaptureLimit = 10;
        public int CaptureLimit
        {
            get { return _iCaptureLimit; }
            set
            {
                if (value < 0)
                    return;
                _iCaptureLimit = value;
            }
        }

        public bool IsCaptureLimit()
        {
            return (_imgQue.BufferCount >= _iCaptureLimit);
        }


        int _iTestTarget = 0;
        public bool AddImage( HObject hoImg )
        {
            _imgQue.AddImage(hoImg);
            if (!_imgQue.IsReadyImage(_iTestTarget))
            {
                return true;
            }

            HObject hoAttachedImage;
            int iOffsetY, iYMin , iYMax;
            if (!_imgQue.GetAttachedImage(_iTestTarget, _iLapSize, out hoAttachedImage, out iOffsetY, out iYMin, out iYMax))
            {
                return true;
            }
            HTuple htWidth, htHeight;
            HOperatorSet.GetImageSize( hoImg, out htWidth, out htHeight );

            List<Blob> lstBlobs;
            if (!run(hoAttachedImage, Param, iYMin, iYMax, out lstBlobs))
            {
                return false;
            }

            if (lstBlobs.Count > 0)
            {
                lstBlobs.ForEach(x =>
                    { 
                        x.GravityY += iOffsetY;
                        _posList.AddPosition(x);
                    }
                    );

                if (_posList.Count >= 3)
                {
                    List<Blob> lstSrc = new List<Blob>();
                    List<Blob> lstDest;
                    for( int i = 0 ; i < 3; i++ )
                        lstSrc.Add((Blob)_posList[i]);
                    posSort(lstSrc, out lstDest, htWidth.I, iOffsetY + htHeight.I);

                    // 長さと角度を求める
                    double dHorzPix = Math.Sqrt(Math.Pow(lstDest[0].GravityX - lstDest[1].GravityX, 2d) + Math.Pow(lstDest[0].GravityY - lstDest[1].GravityY, 2d));
                    double dVertPix = Math.Sqrt(Math.Pow(lstDest[1].GravityX - lstDest[2].GravityX, 2d) + Math.Pow(lstDest[1].GravityY - lstDest[2].GravityY, 2d));
                    double dAngleRad = Math.Atan2(lstDest[0].GravityY - lstDest[1].GravityY, lstDest[0].GravityX - lstDest[1].GravityX);
                    _result.ResolutionHorz = _param.RealHorzMili / dHorzPix;
                    _result.ResolutionVert = _param.RealVertMili / dVertPix;
                    _result.AngleRad = dAngleRad;
                    _result.Blobs.AddRange(lstDest);
                    _result.Success = true;
                    _result.ResultString = "正常終了";
                }
            }
            _iTestTarget++;
            return true;
        }

        private bool run(HObject img, Parameters param, double dYMin, double dYMax, out List<Blob> lstBlobs )
        {
            lstBlobs = new List<Blob>();
            HObject[] ahoObj = new HObject[10];
            HTuple htWidth, htHeight, htNumber, htArea, htRow, htCol;

            try
            {
                HOperatorSet.GetImageSize(img, out htWidth, out htHeight);
                HOperatorSet.GenRectangle1(out ahoObj[4], 0, param.InspectOffset, htHeight, param.InspectWidth == 0 ? htWidth.I : param.InspectOffset + param.InspectWidth);
                HOperatorSet.ReduceDomain(img, ahoObj[4], out ahoObj[5]);

                HOperatorSet.Threshold(img, out ahoObj[5], param.ThresholdLow, param.ThresholdHigh);
                HOperatorSet.OpeningCircle(ahoObj[5], out ahoObj[1], param.OpeningRadius);
                HOperatorSet.ClosingCircle(ahoObj[1], out ahoObj[6], param.ClosingRadius);
                HOperatorSet.Connection(ahoObj[6], out ahoObj[2]);
                HOperatorSet.SelectShape(ahoObj[2], out ahoObj[3], 
                    new string[]{ "area", "width", "height", "row" }, 
                    "and", 
                    new double[]{ 1, param.LimitMinHorzPix, param.LimitMinVertPix, dYMin }, 
                    new double[]{ 99999999, param.LimitMaxHorzPix, param.LimitMaxVertPix, dYMax });

                HOperatorSet.CountObj(ahoObj[3], out htNumber);
                if (htNumber.I > 0)
                {
                    HOperatorSet.AreaCenter( ahoObj[3], out htArea, out htRow, out htCol );
                    for (int i = 0; i < htArea.Length; i++)
                    {
                        lstBlobs.Add(new Blob(htArea[i].D, htCol[i].D, htRow[i].D));
                    }
                }
            }
            catch (HOperatorException e) 
            {
                TraceError(e.Message, MethodBase.GetCurrentMethod().ToString());
                return false;
            }
            finally
            {
                for (int i = 0; i < ahoObj.Length; i++)
                    if (ahoObj[i] != null)
                        ahoObj[i].Dispose();
            }
            return true;
        }

        private bool posSort(List<Blob> lstBlobSrc, out List<Blob> lstBlobDest, int iWidth, int iHeight)
        {
            lstBlobDest = new List<Blob>();

            if (lstBlobSrc.Count == 0)
                return false;

            double dXMin = lstBlobSrc.Min(x => x.GravityX);
            double dXMax = lstBlobSrc.Max(x => x.GravityX);
            double dYMin = lstBlobSrc.Min(x => x.GravityY);
            double dYMax = lstBlobSrc.Max(x => x.GravityY);

            double[] aiPosX = new double[] { dXMin, dXMin, dXMax, dXMax };
            double[] aiPosY = new double[] { dYMin, dYMax, dYMin, dYMax };

            double[,] dLen = new double[4, 3];
            for (int n = 0; n < 4; n++)
            {
                for (int i = 0; i < lstBlobSrc.Count; i++)
                {
                    dLen[n, i] = Math.Sqrt(Math.Pow(aiPosX[n] - lstBlobSrc[i].GravityX, 2d) + Math.Pow(aiPosY[n] - lstBlobSrc[i].GravityY, 2d));
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
            for (int i = 0; i < 3; i++)
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

        private void TraceError(string sMes, string sMethod)
        {
            LogingDllWrap.LogingDll.Loging_SetLogString(string.Format("{0}-{1}", sMes, sMethod));
        }
    }
}
