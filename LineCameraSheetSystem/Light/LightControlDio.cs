using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fujita.Misc;
using Fujita.Communication;

namespace Fujita.LightControl
{
    public class LightPowerSupplayDio : LightPowerSupplayBase
    {
        public LightPowerSupplayDio(CommunicationDIO dio, string name, string vender, int num, int min, int max, bool strobe)
            : base(dio, null, null, name, vender, num, min, max, strobe)
        {
        }

        public LightPowerSupplayDio(CommunicationDIO dio, CommunicationSIO sio, string name, string vender, int num, int min, int max, bool strobe)
            : base(dio, sio, null, name, vender, num, min, max, strobe)
        {
        }

        protected bool _bActive;
        protected int _iWriteBit;
        protected int[] _iaChannelSel;
        protected int[] _iaLightOn;
        protected int[] _iaValue;
        protected int _iExternal;
        protected bool _ValueComplex;

        protected bool Active(bool bIn)
        {
            return (_bActive) ? bIn : !bIn;
        }

        protected byte Active(byte bytIn)
        {
            return (_bActive) ? bytIn : (byte)~bytIn;
        }

        protected ushort Active(ushort shtIn)
        {
            return (_bActive) ? shtIn : (ushort)~shtIn;
        }

        protected uint Active(uint uiIn)
        {
            return (_bActive) ? uiIn : ~uiIn;
        }

    }
}
