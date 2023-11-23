using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InspectionNameSpace
{
    public static class Utility
    {
		/// <summary>
		/// 有効少数桁数
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
        public static double ToRound(double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
		/// <summary>
		/// 明　しきい値算出
		/// </summary>
		/// <param name="kando"></param>
		/// <param name="thresMin"></param>
		/// <param name="thresMax"></param>
		/// <returns></returns>
		public static bool GetBrightThreshold(int baseValue, int kando, ref int thresMin, ref int thresMax)
		{
			bool run = true;

			thresMax = 255;
			if (kando >= 128)
			{
				thresMin = 256;
				return false;
			}

			//明
			//[128 - 255 , 256]　～　255
			//[  0 - 127 , 128]
			thresMin = baseValue + kando;
			if (thresMin > thresMax)
				run = false;

			return run;
		}
		/// <summary>
		/// 暗　しきい値算出
		/// </summary>
		/// <param name="kando"></param>
		/// <param name="thresMin"></param>
		/// <param name="thresMax"></param>
		/// <returns></returns>
		public static bool GetDarkThreshold(int baseValue, int kando, ref int thresMin, ref int thresMax)
		{
			bool run = true;

			thresMin = 0;
			if (kando >= 128)
			{
				thresMax = -1;
				return false;
			}

			//暗
			//0　～　[ -1 ,   0 - 127]
			//       [128 , 127 -   0]
			thresMax = (baseValue - 1) - kando;
			if (thresMax < thresMin)
				run = false;

			return run;
		}
    }
}
