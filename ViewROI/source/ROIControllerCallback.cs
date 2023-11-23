using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using HalconDotNet;

namespace ViewROI
{
    public delegate void Callback_Rectangle1(double row1, double col1, double row2, double col2, object oUser);
    public delegate void Callback_Rectangle2(double row, double col, double phi, double len1, double len2, object oUser);
    public delegate void Callback_Circle(double row, double col, double rad, object oUser);
    public delegate void Callback_CircularArc(double row, double col, double rad, double startphi, double extentphi, object oUser);
    public delegate void Callback_Line(double srow, double scol, double erow, double ecol, object oUser);
    public delegate void Callback_Cancel( object oUser );

    public interface ICallbackRoiRectangle1
    {
        void Rectangle1_Move(double row1, double col1, double row2, double col2, object oUser);
        void Rectangle1_Decide(double row1, double col1, double row2, double col2, object oUser);
        void Rectangle1_Cancel(object oUser);
    };

    public interface ICallbackRoiRectangle2
    {
        void Rectangle2_Move(double row, double col, double phi, double len1, double len2, object oUser);
        void Rectangle2_Decide(double row, double col, double phi, double len1, double len2, object oUser);
        void Rectangle2_Cancel(object oUser);
    };

    public interface ICallbackRoiCircle
    {
        void Circle_Move(double row, double col, double rad, object oUser);
        void Circle_Decide(double row, double col, double rad, object oUser);
        void Circle_Cancel(object oUser);
    };

    public interface ICallbackRoiCircularArc
    {
        void CircularArc_Move(double row, double col, double rad, double startPhi, double extentPhi, object oUser);
        void CircularArc_Decide(double row, double col, double rad, double startPhi, double extentPhi, object oUser);
        void CircularArc_Cancel(object oUser);
    };

    public interface ICallbackRoiLine
    {
        void Line_Move(double srow, double scol, double erow, double ecol, object oUser);
        void Line_Decide(double srow, double scol, double erow, double ecol, object oUser);
        void Line_Cancel(object oUser);
    };

    public enum ROIMode
    {
        None,
        Rectangle1,
        Rectangle2,
        Circle,
        CircularArc,
        Line
    };

    public class ROIControllerCallback
    {

        public ROIMode RoiMode{ get;protected set;}

        private Callback_Rectangle1 rectangle1_move = null;
        private Callback_Rectangle1 rectangle1_decide = null;
        private Callback_Cancel rectangle1_cancel = null;

        private Callback_Rectangle2 rectangle2_move = null;
        private Callback_Rectangle2 rectangle2_decide = null;
        private Callback_Cancel rectangle2_cancel = null;

        private Callback_Circle circle_move = null;
        private Callback_Circle circle_decide = null;
        private Callback_Cancel circle_cancel = null;

        private Callback_CircularArc circulararc_move = null;
        private Callback_CircularArc circulararc_decide = null;
        private Callback_Cancel circulararc_cancel = null;

        private Callback_Line line_move = null;
        private Callback_Line line_decide = null;
        private Callback_Cancel line_cancel = null;

        private object oUserData;

        protected ROIController mRc;

        private void NotifyRoi(int val)
        {
            if (RoiMode == ROIMode.None)
                return;

            if (val == ROIController.EVENT_MOVING_ROI)
            {
                ROI roi = mRc.getActiveROI();
                HTuple data = roi.getModelData();

                switch (RoiMode)
                {
                    case ROIMode.Rectangle1:
                        rectangle1_move(data[0].D, data[1].D, data[2].D, data[3].D, oUserData);
                        break;
                    case ROIMode.Rectangle2:
                        rectangle2_move(data[0].D, data[1].D, data[2].D, data[3].D, data[4], oUserData);
                        break;
                    case ROIMode.Circle:
                        circle_move(data[0].D, data[1].D, data[2].D, oUserData);
                        break;
                    case ROIMode.CircularArc:
                        circulararc_move(data[0].D, data[1].D, data[2].D, data[3].D, data[4].D, oUserData);
                        break;
                    case ROIMode.Line:
                        line_move(data[0].D, data[1].D, data[2].D, data[3].D, oUserData);
                        break;
                }
            }
        }

        public ROIControllerCallback(ROIController rc)
        {
            mRc = rc;
            mRc.NotifyRCObserver += NotifyRoi;
            RoiMode = ROIMode.None;
        }

        public bool StartRoi_Rectanble1( double row1, double col1, double row2, double col2, ICallbackRoiRectangle1 rc, object user )
        {
            if (RoiMode != ROIMode.None)
                return false;
            RoiMode = ROIMode.Rectangle1;
            oUserData = user;
            rectangle1_decide = rc.Rectangle1_Decide;
            rectangle1_move = rc.Rectangle1_Move;
            rectangle1_cancel = rc.Rectangle1_Cancel;
            mRc.setROIShape(new ROIRectangle1(row1, col1, row2, col2));
            return true;
        }

        public bool UpdateRoi_Rectangle1(double row1, double col1, double row2, double col2, bool callback = true)
        {
            if (RoiMode != ROIMode.Rectangle1)
                return false;

            ArrayList al = mRc.getROIList();
            if (al.Count == 0)
                return false;
            ROI roi = (ROI)al[0];
            if (roi.RoiType != ROI.ROI_TYPE_RECTANGLE1)
                return false;
            roi.setModelData( new HTuple( new double[] {row1, col1, row2,col2} ));

            if (callback)
            {
                HTuple newdata = roi.getModelData();
                rectangle1_move(newdata[0].D, newdata[1].D, newdata[2].D, newdata[3].D, oUserData);
            }

            mRc.viewController.repaint();

            return true;
        }

        public bool DecideRoi_Rectangle1()
        {
            if (RoiMode != ROIMode.Rectangle1)
                return false;
            RoiMode = ROIMode.None;
            ArrayList al = mRc.getROIList();
            if (al.Count == 0)
                return false;
            ROI roi = (ROI)al[0];
            HTuple data = roi.getModelData();
            mRc.reset();
            mRc.viewController.repaint();
            rectangle1_decide(data[0].D, data[1].D, data[2].D, data[3].D, oUserData);
            if (RoiMode == ROIMode.None)
            {
                rectangle1_decide = null;
                rectangle1_move = null;
                rectangle1_cancel = null;
            }

            return true;
        }

        public bool CancelRoi_Rectangle1()
        {
            if (RoiMode != ROIMode.Rectangle1)
                return false;
            RoiMode = ROIMode.None;
            mRc.reset();
            rectangle1_cancel(oUserData);
            if (RoiMode == ROIMode.None)
            {
                rectangle1_decide = null;
                rectangle1_move = null;
                rectangle1_cancel = null;
            }
            return true;
        }

        public bool StartRoi_Rectangle2(double row, double col, double phi, double len1, double len2, ICallbackRoiRectangle2 rc, object user)
        {
            if (RoiMode != ROIMode.None)
                return false;
            RoiMode = ROIMode.Rectangle2;
            oUserData = user;
            rectangle2_decide = rc.Rectangle2_Decide;
            rectangle2_move = rc.Rectangle2_Move;
            rectangle2_cancel = rc.Rectangle2_Cancel;
            mRc.setROIShape(new ROIRectangle2(row, col, phi, len1, len2));

            return true;
        }

        public bool UpdateRoi_Rectangle2(double row, double col, double phi, double len1, double len2, bool callback = true)
        {
            if (RoiMode != ROIMode.Rectangle2)
                return false;

            ArrayList al = mRc.getROIList();
            if (al.Count == 0)
                return false;
            ROI roi = (ROI)al[0];
            if (roi.RoiType != ROI.ROI_TYPE_RECTANGLE2)
                return false;
            roi.setModelData(new HTuple(new double[] { row, col, phi, len1, len2 }));

            if (callback)
            {
                HTuple newdata = roi.getModelData();
                rectangle2_move(newdata[0].D, newdata[1].D, newdata[2].D, newdata[3].D, newdata[4].D, oUserData);
            }


            mRc.viewController.repaint();

            return true;

        }

        public bool DecideRoi_Rectangle2()
        {
            if (RoiMode != ROIMode.Rectangle2)
                return false;
            RoiMode = ROIMode.None;
            ArrayList al = mRc.getROIList();
            if (al.Count == 0)
                return false;
            ROI roi = (ROI)al[0];
            HTuple data = roi.getModelData();
            mRc.reset();
            mRc.viewController.repaint();
            rectangle2_decide(data[0].D, data[1].D, data[2].D, data[3].D, data[4].D, oUserData);
            if (RoiMode == ROIMode.None)
            {
                rectangle2_decide = null;
                rectangle2_move = null;
                rectangle2_cancel = null;
            }
            return true;
        }

        public bool CancelRoi_Rectangle2()
        {
            if (RoiMode != ROIMode.Rectangle2)
                return false;
            RoiMode = ROIMode.None;
            mRc.reset();
            mRc.viewController.repaint();
            rectangle2_cancel(oUserData);
            if (RoiMode == ROIMode.None)
            {
                rectangle2_decide = null;
                rectangle2_move = null;
                rectangle2_cancel = null;
            }
            return true;
        }

        public bool StartRoi_Circle(double row, double col, double rad, ICallbackRoiCircle rc, object user)
        {
            if (RoiMode != ROIMode.None)
                return false;
            RoiMode = ROIMode.Circle;
            oUserData = user;
            circle_decide = rc.Circle_Decide;
            circle_move = rc.Circle_Move;
            circle_cancel = rc.Circle_Cancel;
            mRc.setROIShape(new ROICircle(row, col, rad));

            return true;
        }

        public bool UpdateRoi_Circle(double row, double col, double rad, bool callback = true)
        {
            if (RoiMode != ROIMode.Rectangle2)
                return false;

            ArrayList al = mRc.getROIList();
            if (al.Count == 0)
                return false;
            ROI roi = (ROI)al[0];
            if (roi.RoiType != ROI.ROI_TYPE_CIRCLE)
                return false;

            roi.setModelData(new HTuple(new double[] { row, col, rad }));

            if (callback)
            {
                HTuple newdata = roi.getModelData();
                circle_decide(newdata[0].D, newdata[1].D, newdata[2].D, oUserData);
            }

            mRc.viewController.repaint();
            return true;

        }

        public bool DecideRoi_Circle()
        {
            if (RoiMode != ROIMode.Circle)
                return false;
            RoiMode = ROIMode.None;
            ArrayList al = mRc.getROIList();
            if (al.Count == 0)
                return false;
            ROI roi = (ROI)al[0];
            HTuple data = roi.getModelData();
            mRc.reset();
            mRc.viewController.repaint();
            circle_decide(data[0].D, data[1].D, data[2].D, oUserData);
            if (RoiMode == ROIMode.None)
            {
                circle_decide = null;
                circle_move = null;
                circle_cancel = null;
            }
            return true;
        }

        public bool CancelRoi_Circle()
        {
            if (RoiMode != ROIMode.Circle)
                return false;
            RoiMode = ROIMode.None;
            mRc.reset();
            mRc.viewController.repaint();
            circle_cancel(oUserData);
            if (RoiMode == ROIMode.None)
            {
                circle_decide = null;
                circle_move = null;
                circle_cancel = null;
            }
            return true;
        }

        public bool StartRoi_CircularArc(double row, double col, double rad, double startphi, double endphi, ICallbackRoiCircularArc rc, object user)
        {
            if (RoiMode != ROIMode.None)
                return false;

            RoiMode = ROIMode.CircularArc;
            oUserData = user;
            circulararc_decide = rc.CircularArc_Decide;
            circulararc_move = rc.CircularArc_Move;
            circulararc_cancel = rc.CircularArc_Cancel;
            mRc.setROIShape(new ROICircularArc(row, col, rad, startphi, endphi));

            return true;
        }

        public bool UpdateRoi_CircularArc(double row, double col, double rad, double startphi, double endphi, bool callback = true)
        {
            if (RoiMode != ROIMode.CircularArc)
                return false;

            ArrayList al = mRc.getROIList();
            if (al.Count == 0)
                return false;
            ROI roi = (ROI)al[0];
            if (roi.RoiType != ROI.ROI_TYPE_CIRCLEARC)
               return false;
            roi.setModelData(new HTuple(new double[] { row, col, rad, startphi, endphi }));

            if (callback)
            {
                HTuple newdata = roi.getModelData();
                circulararc_move(newdata[0].D, newdata[1].D, newdata[2].D, newdata[3].D, newdata[4].D, oUserData);
            }

            mRc.viewController.repaint();

            return true;

        }

        public bool DecideRoi_CircularArc()
        {
            if (RoiMode != ROIMode.CircularArc)
                return false;
            RoiMode = ROIMode.None;
            ArrayList al = mRc.getROIList();
            if (al.Count == 0)
                return false;
            ROI roi = (ROI)al[0];
            HTuple data = roi.getModelData();
            mRc.reset();
            mRc.viewController.repaint();
            circulararc_decide(data[0].D, data[1].D, data[2].D, data[3].D, data[4].D, oUserData);
            if (RoiMode == ROIMode.None)
            {
                circulararc_decide = null;
                circulararc_move = null;
                circulararc_cancel = null;
            }
            return true;
        }

        public bool CancelRoi_CircularArc()
        {
            if (RoiMode != ROIMode.Circle)
                return false;
            RoiMode = ROIMode.None;
            mRc.reset();
            mRc.viewController.repaint();
            circulararc_cancel(oUserData);
            if (RoiMode == ROIMode.None)
            {
                circulararc_decide = null;
                circulararc_move = null;
                circulararc_cancel = null;
            }
            return true;
        }

        public bool StartRoi_Line(double row1, double col1, double row2, double col2,  ICallbackRoiLine rc, object user)
        {
            if (RoiMode != ROIMode.None)
                return false;

            RoiMode = ROIMode.Line;
            oUserData = user;
            line_decide = rc.Line_Decide;
            line_move = rc.Line_Move;
            line_cancel = rc.Line_Cancel;
            mRc.setROIShape(new ROILine(row1, col1, row2, col2 ));

            return true;
        }

        public bool UpdateRoi_Line(double row1, double col1, double row2, double col2, bool callback = true)
        {
            if (RoiMode != ROIMode.Line)
                return false;

            ArrayList al = mRc.getROIList();
            if (al.Count == 0)
                return false;
            ROI roi = (ROI)al[0];
            if (roi.RoiType != ROI.ROI_TYPE_LINE)
                return false;
            roi.setModelData(new HTuple(new double[] { row1, col1, row2, col2 }));

            if (callback)
            {
                HTuple newdata = roi.getModelData();
                line_move(newdata[0].D, newdata[1].D, newdata[2].D, newdata[3].D, oUserData);
            }

            mRc.viewController.repaint();

            return true;
        }

        public bool DecideRoi_Line()
        {
            if (RoiMode != ROIMode.Line)
                return false;

            RoiMode = ROIMode.None;
            ArrayList al = mRc.getROIList();
            if (al.Count == 0)
                return false;
            ROI roi = (ROI)al[0];
            HTuple data = roi.getModelData();
            mRc.reset();
            mRc.viewController.repaint();
            line_decide(data[0].D, data[1].D, data[2].D, data[3].D, oUserData);

            if (RoiMode == ROIMode.None)
            {
                line_decide = null;
                line_move = null;
                line_cancel = null;
            }
            return true;
        }

        public bool CancelRoi_Line()
        {
            if (RoiMode != ROIMode.Circle)
                return false;

            RoiMode = ROIMode.None;
            mRc.reset();
            mRc.viewController.repaint();
            line_cancel(oUserData);
            if (RoiMode == ROIMode.None)
            {
                line_decide = null;
                line_move = null;
                line_cancel = null;
            }
            return true;
        }

    }
}
