using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HalconDotNet;

namespace ViewROI
{
    public class ROIPoint : ROI
    {
        private double dRow, dCol;

        public ROIPoint()
        {
            NumHandles = 1;
            activeHandleIdx = 0;
            RoiType = ROI_TYPE_POINT;
            Initial = false;
        }

        public ROIPoint(double row, double col)
        {
            NumHandles = 1;
            activeHandleIdx = 0;

            RoiType = ROI_TYPE_POINT;

            dRow = row;
            dCol = col;

            Initial = true;
        }

        public override void createROI()
        {

        }

        public override void createROI(double midX, double midY)
        {
            dRow = midY;
            dCol = midX;
        }

        public override void draw(HalconDotNet.HWindow window)
        {
            window.DispRectangle2(dRow, dCol, 0, ROI_THUM_SIZE, ROI_THUM_SIZE);
            window.DispCross(dRow, dCol, 20, 0);
        }

        public override double distToClosestHandle(double x, double y)
        {
            return HMisc.DistancePp(y, x, dRow, dCol);
        }

        public override void displayActive(HWindow window)
        {
            window.DispRectangle2(dRow, dCol, 0, ROI_THUM_SIZE, ROI_THUM_SIZE);
            window.DispCross(dRow, dCol, 20, 0);
        }

        public override HRegion getRegion()
        {
            // no support
            return null;
        }

        public override double getDistanceFromStartPoint(double row, double col)
        {
            return HMisc.DistancePp(row, col, dRow, dCol);
        }

        public override HTuple getModelData()
        {
            return new HTuple(new double[] { dRow, dCol });
        }

        public override bool setModelData(HTuple data)
        {
            if (data.Length != 2)
                return false;

            dRow = data[0].D;
            dCol = data[1].D;

            return true;
        }

        public override void moveByHandle(double x, double y)
        {
            dRow = y;
            dCol = x;
        }
    }
}
