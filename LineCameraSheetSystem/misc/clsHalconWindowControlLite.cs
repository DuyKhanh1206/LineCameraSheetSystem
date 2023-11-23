using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using HalconDotNet;

namespace Adjustment
{
    class clsHalconWindowControlLite
    {
        public static bool FittingImage(HObject img, HWindowControl hWC )
        {
            HTuple htImgW, htImgH;
            try
            {
                HOperatorSet.GetImageSize(img, out htImgW, out htImgH);
                double dWRate = htImgW.I / (double)hWC.WindowSize.Width;
                double dHRate = htImgH.I / (double)hWC.WindowSize.Height;

                double dMagnify = 0d;
                if( dWRate < 1d && dHRate < 1d )
                {
                    hWC.ImagePart = new System.Drawing.Rectangle( 0, 0, hWC.WindowSize.Width, hWC.WindowSize.Height );
                }
                else
                {
                    if( dWRate > dHRate )
                    {
                        dMagnify = 1 / dWRate;
                    }
                    else
                    {
                        dMagnify = 1 / dHRate;
                    }
                    hWC.ImagePart = new System.Drawing.Rectangle(0, 0, (int)Math.Floor(hWC.WindowSize.Width / dMagnify), (int)Math.Floor(hWC.WindowSize.Height / dMagnify));
                }
                HOperatorSet.DispObj(img, hWC.HalconWindow);
            }
            catch (HOperatorException)
            {
                return false;
            }
            finally
            {
            }
            return true;
        }

        public static double GetMagnify(HObject img, HWindowControl hWC)
        {
            HTuple htImgW, htImgH;
            double dMagnify = 1d;
            try
            {
                HOperatorSet.GetImageSize(img, out htImgW, out htImgH);
                double dWRate = htImgW.I / (double)hWC.WindowSize.Width;
                double dHRate = htImgH.I / (double)hWC.WindowSize.Height;
                if( dWRate > 1d || dHRate > 1d )
                {
                    if (dWRate > dHRate)
                    {
                        dMagnify = 1 / dWRate;
                    }
                    else
                    {
                        dMagnify = 1 / dHRate;
                    }
                }
            }
            catch (HOperatorException)
            {
            }
            finally
            {
            }
            return dMagnify;
        }

        public static bool DispObj(HObject obj, HWindowControl hWC)
        {
            try
            {
                HOperatorSet.DispObj(obj, hWC.HalconWindow);
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public enum EDrawMode
        {
            fill,
            margin
        }

        public enum EHalconColor
        {
            black, 
            white, 
            red, 
            green, 
            blue, 
            cyan, 
            magenta, 
            yellow, 
            dim_gray, 
            gray, 
            light_gray, 
            medium_slate_blue, 
            coral, 
            slate_blue, 
            spring_green, 
            orange_red, 
            orange, 
            dark_olive_green, 
            pink, 
            cadet_blue
        }

        public static bool DispObj(HObject obj, HWindowControl hWC, EDrawMode eDraw)
        {
            try
            {
                HOperatorSet.SetDraw(hWC.HalconWindow, eDraw.ToString());
                HOperatorSet.DispObj(obj, hWC.HalconWindow);
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public static bool DispObj(HObject obj, HWindowControl hWC, EHalconColor eColor )
        {
            try
            {
                HOperatorSet.SetColor(hWC.HalconWindow, eColor.ToString().Replace('_', ' ' ));
                HOperatorSet.DispObj(obj, hWC.HalconWindow);
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public static bool DispObj(HObject obj, HWindowControl hWC, EDrawMode eDraw, EHalconColor eColor)
        {
            try
            {
                HOperatorSet.SetDraw(hWC.HalconWindow, eDraw.ToString());
                HOperatorSet.SetColor(hWC.HalconWindow, eColor.ToString().Replace('_', ' '));
                HOperatorSet.DispObj(obj, hWC.HalconWindow);
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public static bool DispObj(HObject obj, HWindowControl hWC, EDrawMode eDraw, int iColored)
        {
            try
            {
                HOperatorSet.SetDraw(hWC.HalconWindow, eDraw.ToString());
                HOperatorSet.SetColored(hWC.HalconWindow, iColored);
                HOperatorSet.DispObj(obj, hWC.HalconWindow);
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public static bool LockGraphic( HWindowControl hWC )
        {
            try
            {
                HSystem.SetSystem("flush_graphic", "false");
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }

        public static bool UnlockGraphic(HWindowControl hWC)
        {
            try
            {
                HSystem.SetSystem("flush_graphic", "true" );
                // 再描画
                hWC.HalconWindow.SetColor("black");
                hWC.HalconWindow.DispLine(-100.0, -100.0, -101.0, -101.0);
            }
            catch (HOperatorException)
            {
                return false;
            }
            return true;
        }
    }
}
