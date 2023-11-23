using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adjustment;

namespace LineCameraSheetSystem.Adjust
{
    class clsAdjustmentAppData : IApplicationDataContainer, IResolutionParamContainer, IOffsetParamContainer
    {
            static clsAdjustmentAppData instance = new clsAdjustmentAppData();
            public static clsAdjustmentAppData getInstance()
            {
                return instance;
            }

            //bool _bOfflineMode = true;
            double[] dResolutionHorz = new double[4] { 1d, 1d, 1d, 1d };
            double[] dResolutionVert = new double[4] { 1d, 1d, 1d, 1d };
            double[] dOffsetHorz = new double[4] { 0d, 0d, 0d, 0d };
            double[] dOffsetVert = new double[4] { 0d, 0d, 0d, 0d };

            private clsAdjustmentAppData()
            {
                LoadSystemParam();
            }

            private void LoadSystemParam()
            {
                SystemParam sysparam = SystemParam.GetInstance();
                //_bOfflineMode = sysparam.OffLineMode;

                int i;
                for (i = 0; AppData.CAM_COUNT > i; i++)
                {
                    dResolutionHorz[i] = sysparam.camParam[i].ResoH;
                    dResolutionVert[i] = sysparam.camParam[i].ResoV;

                    //if (i >= 1)
                    //{
                    //    dOffsetHorz[i - 1] = sysparam.camParam[i].ShiftH;
                    //    dOffsetVert[i - 1] = sysparam.camParam[i].ShiftV;
                    //}
                    dOffsetHorz[i] = sysparam.camParam[i].ShiftH;
                    dOffsetVert[i] = sysparam.camParam[i].ShiftV;
                }

            }

            private void SaveSystemParam()
            {
                SystemParam sysparam = SystemParam.GetInstance();

                int i;
                for (i = 0; AppData.CAM_COUNT > i; i++)
                {
                    sysparam.camParam[i].ResoH = dResolutionHorz[i];
                    sysparam.camParam[i].ResoV = dResolutionVert[i];

                    //if (i >= 1)
                    //{
                    //    sysparam.camParam[i].ShiftH = dOffsetHorz[i - 1];
                    //    sysparam.camParam[i].ShiftV = dOffsetVert[i - 1];
                    //}

                    sysparam.camParam[i].ShiftH = dOffsetHorz[i];
                    sysparam.camParam[i].ShiftV = dOffsetVert[i];
                }

                sysparam.SystemSave();

            }

            public void SetResolutionParameter(EAdjustmentCameraType eType, double dResX, double dResY)
            {
                dResolutionHorz[(int)eType] = dResX;
                dResolutionVert[(int)eType] = dResY;

                SaveSystemParam();
            }

            public void GetResolutionParamtter(EAdjustmentCameraType eType, ref double dResX, ref double dResY)
            {
                dResX = dResolutionHorz[(int)eType];
                dResY = dResolutionVert[(int)eType];

            }

            public void SetOffsetParameter(EAdjustmentCameraType eType, double dOffsetX, double dOffsetY)
            {
                dOffsetHorz[(int)eType] = dOffsetX;
                dOffsetVert[(int)eType] = dOffsetY;

                SaveSystemParam();
            }

            public void GetOffsetParameter(EAdjustmentCameraType eType, ref double dOffsetX, ref double dOffsetY)
            {
                dOffsetX = dOffsetHorz[(int)eType];
                dOffsetY = dOffsetVert[(int)eType];
            }

            public bool SetApplicationData(EApplicationDataType eType, bool bVal)
            {
                if (eType == EApplicationDataType.OfflineMode)
                {
                    //_bOfflineMode = bVal;
                    //SaveSystemParam();
                    return true;
                }
                return false;
            }

            public bool SetApplicationData(EApplicationDataType eType, string sVal)
            {
                return false;
            }

            public bool SetApplicationData(EApplicationDataType eType, int iVal)
            {
                return false;
            }

            public bool SetApplicationData(EApplicationDataType eType, double dVal)
            {
                return false;
            }

        public bool GetApplicationData(EApplicationDataType eType, ref bool bVal)
        {
            if (eType == EApplicationDataType.OfflineMode)
            {
                //bVal = _bOfflineMode;
                return true;
            }
            return false;
        }
        public bool GetApplicationData(EApplicationDataType eType, ref string sVal)
            {
                return false;
            }
            public bool GetApplicationData(EApplicationDataType eType, ref int iVal)
            {
                return false;
            }
            public bool GetApplicationData(EApplicationDataType eType, ref double dVal)
            {
                return false;
            }
       // }

    }
}
