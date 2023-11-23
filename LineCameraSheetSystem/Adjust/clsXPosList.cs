using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Adjustment
{
    interface IPosition
    {
        double XPos
        {
            get;
        }
        double YPos
        {
            get;
        }
    }

    class clsLapList : List<IPosition>
    {
        private double _dLimitHorz;
        public double LimitHorz
        {
            get { return _dLimitHorz; }
            set
            {
                if (value < 0.0)
                    return;
                _dLimitHorz = value;
            }
        }
        private double _dLimitVert;
        public double LimitVert
        {
            get { return _dLimitVert; }
            set
            {
                if (value < 0.0)
                    return;
                _dLimitVert = value;
            }
        }

        public bool AddPosition(IPosition pos)
        {
            if (!Exists( o =>
                (o.XPos >= pos.XPos - _dLimitHorz && o.XPos <= pos.XPos + _dLimitHorz
                && o.YPos >= pos.YPos - _dLimitVert && o.YPos <= pos.YPos + _dLimitVert)))
            {
                Add(pos);
                return true;
            }
            return false;
        }

    }
}
