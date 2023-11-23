using System;
using HalconDotNet;


namespace ViewROI
{

    /// <summary>
    /// This class demonstrates one of the possible implementations for a 
    /// (simple) rectangularly shaped ROI. To create this rectangle we use 
    /// a center point (midR, midC), an orientation 'phi' and the half 
    /// edge lengths 'length1' and 'length2', similar to the HALCON 
    /// operator gen_rectangle2(). 
    /// The class ROIRectangle2 inherits from the base class ROI and 
    /// implements (besides other auxiliary methods) all virtual methods 
    /// defined in ROI.cs.
    /// </summary>
    public class ROIRectangle2 : ROI
    {

        /// <summary>Half length of the rectangle side, perpendicular to phi</summary>
        private double length1;

        /// <summary>Half length of the rectangle side, in direction of phi</summary>
        private double length2;

        /// <summary>Row coordinate of midpoint of the rectangle</summary>
        private double midR;

        /// <summary>Column coordinate of midpoint of the rectangle</summary>
        private double midC;

        /// <summary>Orientation of rectangle defined in radians.</summary>
        private double phi;

        //auxiliary variables
        HTuple rowsInit;
        HTuple colsInit;
        HTuple rows;
        HTuple cols;

        HHomMat2D hom2D, tmp;

        /// <summary>Constructor</summary>
        public ROIRectangle2()
        {
            NumHandles = 6; // 4 corners +  1 midpoint + 1 rotationpoint			
            activeHandleIdx = 4;

            RoiType = ROI_TYPE_RECTANGLE2;

            Initial = false;
        }

        public ROIRectangle2(double row, double col, double dPhi, double len1, double len2)
        {
            NumHandles = 6;
            activeHandleIdx = 4;

            RoiType = ROI_TYPE_RECTANGLE2;

            midR = row;
            midC = col;
            phi = -dPhi;
            length1 = len1;
            length2 = len2;
            Initial = true;
        }

        /// <summary>Creates a new ROI instance at the mouse position</summary>
        /// <param name="midX">
        /// x (=column) coordinate for interactive ROI
        /// </param>
        /// <param name="midY">
        /// y (=row) coordinate for interactive ROI
        /// </param>
        public override void createROI(double midX, double midY)
        {
            midR = midY;
            midC = midX;

            length1 = 100;
            length2 = 50;

            phi = 0.0;

            rowsInit = new HTuple(new double[] {-1.0, -1.0, 1.0,
                                                   1.0,  0.0, 0.0 });
            colsInit = new HTuple(new double[] {-1.0, 1.0,  1.0,
                                                  -1.0, 0.0, 0.6 });
            //order        ul ,  ur,   lr,  ll,   mp, arrowMidpoint
            hom2D = new HHomMat2D();
            tmp = new HHomMat2D();

            updateHandlePos();
        }

        public override void createROI()
        {

            rowsInit = new HTuple(new double[] {-1.0, -1.0, 1.0,
                                                   1.0,  0.0, 0.0 });
            colsInit = new HTuple(new double[] {-1.0, 1.0,  1.0,
                                                  -1.0, 0.0, 0.6 });
            //order        ul ,  ur,   lr,  ll,   mp, arrowMidpoint
            hom2D = new HHomMat2D();
            tmp = new HHomMat2D();

            updateHandlePos();
        }

        /// <summary>Paints the ROI into the supplied window</summary>
        /// <param name="window">HALCON window</param>
        public override void draw(HalconDotNet.HWindow window)
        {
            window.DispRectangle2(midR, midC, -phi, length1, length2);
            for (int i = 0; i < NumHandles; i++)
                window.DispRectangle2(rows[i].D, cols[i].D, -phi, ROI_THUM_SIZE, ROI_THUM_SIZE);

            window.DispArrow(midR, midC, midR + (Math.Sin(phi) * length1 * 1.2),
                midC + (Math.Cos(phi) * length1 * 1.2), 2.0);

        }

        /// <summary> 
        /// Returns the distance of the ROI handle being
        /// closest to the image point(x,y)
        /// </summary>
        /// <param name="x">x (=column) coordinate</param>
        /// <param name="y">y (=row) coordinate</param>
        /// <returns> 
        /// Distance of the closest ROI handle.
        /// </returns>
        public override double distToClosestHandle(double x, double y)
        {
            double max = 10000;
            double[] val = new double[NumHandles];


            for (int i = 0; i < NumHandles; i++)
                val[i] = HMisc.DistancePp(y, x, rows[i].D, cols[i].D);

            for (int i = 0; i < NumHandles; i++)
            {
                if (val[i] < max)
                {
                    max = val[i];
                    activeHandleIdx = i;
                }
            }
            return val[activeHandleIdx];
        }

        /// <summary> 
        /// Paints the active handle of the ROI object into the supplied window
        /// </summary>
        /// <param name="window">HALCON window</param>
        public override void displayActive(HalconDotNet.HWindow window)
        {
            window.DispRectangle2(rows[activeHandleIdx].D,
                                  cols[activeHandleIdx].D,
                                  -phi, ROI_THUM_SIZE, ROI_THUM_SIZE);

            if (activeHandleIdx == 5)
                window.DispArrow(midR, midC,
                                 midR + (Math.Sin(phi) * length1 * 1.2),
                                 midC + (Math.Cos(phi) * length1 * 1.2),
                                 2.0);
        }


        /// <summary>Gets the HALCON region described by the ROI</summary>
        public override HRegion getRegion()
        {
            HRegion region = new HRegion();
            region.GenRectangle2(midR, midC, -phi, length1, length2);
            return region;
        }

        /// <summary>
        /// Gets the model information described by 
        /// the interactive ROI
        /// </summary> 
        public override HTuple getModelData()
        {
            return new HTuple(new double[] { midR, midC, -phi, length1, length2 });
        }

        public override bool setModelData(HTuple data)
        {
            if (data.Length != 5)
                return false;

            if (data[3].D < 0 || data[4].D < 0)
                return false;

            midR = data[0].D;
            midC = data[1].D;
            phi = -data[2].D;
            length1 = data[3].D;
            length2 = data[4].D;

            updateHandlePos();

            return false;
        }

        /// <summary> 
        /// Recalculates the shape of the ROI instance. Translation is 
        /// performed at the active handle of the ROI object 
        /// for the image coordinate (x,y)
        /// </summary>
        /// <param name="newX">x mouse coordinate</param>
        /// <param name="newY">y mouse coordinate</param>
        public override void moveByHandle(double newX, double newY)
        {
            double vX, vY, x = 0, y = 0;

            switch (activeHandleIdx)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    tmp = hom2D.HomMat2dInvert();
                    x = tmp.AffineTransPoint2d(newX, newY, out y);

                    length2 = Math.Abs(y);
                    length1 = Math.Abs(x);

                    System.Diagnostics.Debug.Print("x = {0}, y = {1}", x, y);

                    checkForRange(x, y);
                    break;
                case 4:
                    midC = newX;
                    midR = newY;
                    break;
                case 5:
                    vY = newY - rows[4].D;
                    vX = newX - cols[4].D;
                    phi = Math.Atan2(vY, vX);
                    break;
            }
            updateHandlePos();
        }//end of method


        /// <summary>
        /// Auxiliary method to recalculate the contour points of 
        /// the rectangle by transforming the initial row and 
        /// column coordinates (rowsInit, colsInit) by the updated
        /// homography hom2D
        /// </summary>
        private void updateHandlePos()
        {
            hom2D.HomMat2dIdentity();
            hom2D = hom2D.HomMat2dTranslate(midC, midR);
            hom2D = hom2D.HomMat2dRotateLocal(phi);
            tmp = hom2D.HomMat2dScaleLocal(length1, length2);
            cols = tmp.AffineTransPoint2d(colsInit, rowsInit, out rows);
        }


        /* This auxiliary method checks the half lengths 
		 * (length1, length2) using the coordinates (x,y) of the four 
		 * rectangle corners (handles 0 to 3) to avoid 'bending' of 
		 * the rectangular ROI at its midpoint, when it comes to a
		 * 'collapse' of the rectangle for length1=length2=0.
		 * */
        private void checkForRange(double x, double y)
        {
            switch (activeHandleIdx)
            {
                case 0:
                    if ((x < 0) && (y < 0))
                        return;
                    if (x >= 0) length1 = 0.01;
                    if (y >= 0) length2 = 0.01;
                    break;
                case 1:
                    if ((x > 0) && (y < 0))
                        return;
                    if (x <= 0) length1 = 0.01;
                    if (y >= 0) length2 = 0.01;
                    break;
                case 2:
                    if ((x > 0) && (y > 0))
                        return;
                    if (x <= 0) length1 = 0.01;
                    if (y <= 0) length2 = 0.01;
                    break;
                case 3:
                    if ((x < 0) && (y > 0))
                        return;
                    if (x >= 0) length1 = 0.01;
                    if (y <= 0) length2 = 0.01;
                    break;
                default:
                    break;
            }
        }
    }//end of class
}//end of namespace
