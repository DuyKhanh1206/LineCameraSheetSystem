using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Adjustment
{
    public enum EAdjustmentOffsetType
    {
        Camera1_2 = 0,
        Camera1_3 = 1,
        Camera1_4 = 2,
    }

    public interface IOffsetParamContainer
    {
        void SetOffsetParameter(EAdjustmentCameraType eType, double dOffsetX, double dOffsetY);
        void GetOffsetParameter(EAdjustmentCameraType eType, ref double dOffsetX, ref double dOffsetY);
    }

    public enum EAdjustmentCameraType
    {
        Camera1 = 0,
        Camera2 = 1,
        Camera3 = 2,
        Camera4 = 3,
    }

    public interface IResolutionParamContainer
    {
        void SetResolutionParameter(EAdjustmentCameraType eType, double dResX, double dResY);
        void GetResolutionParamtter(EAdjustmentCameraType eType, ref double dResX, ref double dResY);
    }

    public enum EApplicationDataType
    {
        OfflineMode = 0,
    }

    public interface IApplicationDataContainer
    {
        bool SetApplicationData(EApplicationDataType eType, bool bVal);
        bool SetApplicationData(EApplicationDataType eType, string sVal);
        bool SetApplicationData(EApplicationDataType eType, int iVal);
        bool SetApplicationData(EApplicationDataType eType, double dVal);

        bool GetApplicationData(EApplicationDataType eType, ref bool bVal);
        bool GetApplicationData(EApplicationDataType eType, ref string sVal);
        bool GetApplicationData(EApplicationDataType eType, ref int iVal);
        bool GetApplicationData(EApplicationDataType eType, ref double dVal);

    }
}
