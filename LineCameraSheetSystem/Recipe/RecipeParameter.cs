using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineCameraSheetSystem
{
    public class InspParameter
    {
        /// <summary>
        /// 検査幅(mm)
        /// </summary>
        public double Width { get; set; }
        /// <summary>
        /// マスク幅(mm)
        /// </summary>
        public double MaskWidth { get; set; }
        /// <summary>
        /// マスクシフト(mm)
        /// </summary>
        public double MaskShift { get; set; }
        /// <summary>
        /// Zone(mm)
        /// </summary>
        public double[] Zone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<InspKandoParam> Kando = new List<InspKandoParam>();

        public InspParameter()
        {
            Width = SystemParam.GetInstance().DefaultInspWidth;
            MaskWidth = SystemParam.GetInstance().DefaultMaskWidth;
            MaskShift = SystemParam.GetInstance().DefaultMaskShift;
            Zone = new double[AppData.MAX_PARTITION];
            for (int i = 0; i < AppData.MAX_PARTITION; i++)
                Zone[i] = (int)(Width / AppData.MAX_PARTITION);
            for (int i=0; i<Enum.GetNames(typeof(AppData.InspID)).Length; i++)
            {
                InspKandoParam kando = new InspKandoParam();
                kando.Threshold = 128;
                kando.LengthV = 0;
                kando.LengthH = 0;
                kando.Area = 0;
                Kando.Add(kando);
            }
        }
        public void Copy(InspParameter insp)
        {
            insp.Width = this.Width;
            insp.MaskWidth = this.MaskWidth;
            insp.MaskShift = this.MaskShift;
            for(int i=0; i<AppData.MAX_PARTITION;i++)
                insp.Zone[i] = this.Zone[i];
            for (int i = 0; i < Enum.GetNames(typeof(AppData.InspID)).Length; i++)
            {
                this.Kando[i].Copy(insp.Kando[i]);
            }
            return;
        }

    }

    public class InspKandoParam 
    {
        //ID
        public AppData.InspID inspID { get; set; }
        //感度(閾値)
        public int Threshold { get; set; }
        //縦判定値
        public double LengthV { get; set; }
        //横判定値
        public double LengthH { get; set; }
        //面積判定値
        public double Area { get; set; }

        public void Copy(InspKandoParam inspKando)
        {
            inspKando.inspID = this.inspID;
            inspKando.Threshold = this.Threshold;
            inspKando.LengthV = this.LengthV;
            inspKando.LengthH = this.LengthH;
            inspKando.Area = this.Area;
            return;
        }
    }

    public class LightParameter
    {
        public int LightValue { get; set; }
        public bool LightEnable { get; set; }

        public LightParameter()
        {
            LightValue = 0;
            LightEnable = true;
        }
        public void Copy(LightParameter lp)
        {
            lp.LightValue = this.LightValue;
            lp.LightEnable = this.LightEnable;
            return;
        }
    }
}
