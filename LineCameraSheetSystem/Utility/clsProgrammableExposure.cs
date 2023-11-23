using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineCameraSheetSystem
{
    public class clsProgrammableExposure
    {
        public clsProgrammableExposure()
        {
            ListProExp = new List<ProgrammableExposure> { };
            GenProgrammableExposure();
        }

        List<ProgrammableExposure> ListProExp { get; set; }

        struct ProgrammableExposure
        {
            //露光時間(μs)
            public double dExpsure { get; set; }
            //カメラ用パラメータ VAl1　(0-11)
            public int iVal1 { get; set; }
            //カメラ用パラメータ VAl2　(61-1023)
            public int iVal2 { get; set; }
        }



        //プログラマブル露光値の全パターンの計算
        private void GenProgrammableExposure()
        {
            //Val1の最小値
            int iVal1Min = 0;
            int iVal1Max = 11;
            int iVal2Min = 61;
            int iVal2Max = 1023;

            for (int i = 0; (iVal1Max - iVal1Min + 1) > i; i++)
            {
                for (int j = 0; (iVal2Max - iVal2Min + 1) > j; j++)
                {


                    ProgrammableExposure ProgExpo = new ProgrammableExposure();

                    ProgExpo.dExpsure = (((iVal2Min + j) / (20000000 / (16 * Math.Pow(2, (iVal1Min + i))))) * 1000000);
                    ProgExpo.iVal1 = i;
                    ProgExpo.iVal2 = iVal2Min + j;

                    ListProExp.Add(ProgExpo);
                }
            }

            //露光値の小さい順に並び替え
            // ListProExp.Sort((X, Y) => { return (int)(X.dExpsure - Y.dExpsure); });    
            ListProExp.Sort((X, Y) => { return SortHikaku(X.dExpsure, Y.dExpsure); });
        }

        private int SortHikaku(double X, double Y)
        {
            if (X > Y)
            {
                return 1;
            }
            else if (X < Y)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        //露光時間(μｓ)を入れてVAL1,VAL2を返す。
        public void FindExposure(double dExpTme, out int iVal1, out int iVal2)
        {
            if (dExpTme < 48.8)
            {
                iVal1 = 0;
                iVal2 = 61;

                return;
            }
            else if (dExpTme > 167683.2)
            {
                iVal1 = 11;
                iVal2 = 1023;
                return;
            }

            int index = -1;
            index = ListProExp.FindIndex(x => x.dExpsure >= dExpTme);

            iVal1 = ListProExp[index].iVal1;
            iVal2 = ListProExp[index].iVal2;
        }
    }
}
