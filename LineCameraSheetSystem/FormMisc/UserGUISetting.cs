using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fujita.Misc;

namespace Fujita.InspectionSystem
{
    public enum UserSettingChangeType
    {
        ValueChange,
        OK,
        Cancel,
        MultiSettingChange,
    }

    public enum MultiSettingChange
    {
        None,
        Current_Change,
        Add_Next,
        Add_Last,
        Delete,
    }

    public interface ICallbackThreshold
    {
        void Threshold_ValueChange(int iLow, int iHigh, object oUser);
        void Threshold_Decide(int iLow, int iHigh, object oUser);
        void Threshold_Cancel(object oUser);
    }

    public interface ICallbackPointMulti
    {
        void PointMulti_ValueChange(List<double> daRows, List<double> daCols, object oUser);
        void PointMulti_Decide(List<double> daRows, List<double> daCols, object oUser);
        void PointMulti_Cancel(object oUser);
    }

    public interface ICallbackRectangle1Multi
    {
        void Rectangle1Multi_ValueChange(List<CRectangle1> lstRect, object oUser);
        void Rectangle1Multi_Decide(List<CRectangle1> lstRect, object oUser);
        void Rectangle1Multi_Cancel(object oUser);
    }
}
