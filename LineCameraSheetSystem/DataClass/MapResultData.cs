using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SheetMapping;
using ResultActionDataClassNameSpace;
using HalconDotNet;

namespace LineCameraSheetSystem
{
    class MapResultData: ISheetTipItemContainer
    {
        public MapResultData (uclSheetMap UCSheetMap)
        {
            //_resultActionDataCls = new ResultActionDataClass();

            _ucSheetMap = UCSheetMap;

            _ucSheetMap.SheetLengthMeter = _dLastLength;
            _ucSheetMap.SheetTipItemContainer = this;
            _ucSheetMap.TipClicked += new TipClickedEventHandler(UclSheetMapReal1_TipClicked);
            //再描画
            _ucSheetMap.Repaint();
            //NGの印のサイズ
            _ucSheetMap.TipSize = new System.Drawing.Size(7, 7);

        }

        private uclSheetMap _ucSheetMap = new uclSheetMap();
        //public ResultActionDataClass _resultActionDataCls;
        public List<clsItemData> _lstItem = new List<clsItemData>();
        double _dLastLength = 0;

        public List<HObject> _listHObj = new List<HObject>();

        //NGの印がクリックされた
        void UclSheetMapReal1_TipClicked(object sender, TipClickedEventArgs e)
        {
        }

        public void FetchSheetTipItems( uclSheetMap sender, double dStart, double dEnd, out List<clsSheetTipItem> lstSheetTipItems)
        {
            lstSheetTipItems = new List<clsSheetTipItem>();

            for (int i = 0; i < _lstItem.Count; i++)
            {
                if (_lstItem[i].Y >= dStart)
                    lstSheetTipItems.Add(new clsSheetTipItem(_lstItem[i].X, _lstItem[i].Y, _lstItem[i].Color, (object)i));
                if (_lstItem[i].Y > dEnd)
                    break;
            }
        }     
    }

    class clsItemData
    {
        //NGの座標、NG種の色
        public clsItemData(double x, double y, int col, int lineno)
        {
            X = x;
            Y = y;
            Color = col;
            LineNo = lineno;
        }

        public double X
        {
            get;
            private set;
        }
        public double Y
        {
            get;
            private set;
        }

        public int Color
        {
            get;
            private set;
        }

        public int LineNo
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return "X = " + X.ToString("F1") + "[mm] Y = " + Y.ToString("F1") + "[m]" + "ColorIndex = " + Color.ToString();
        }
    }
}
