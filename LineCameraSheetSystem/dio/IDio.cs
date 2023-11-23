using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fujita.Communication
{
    public interface IDio : IDisposable
    {
        string DeviceName { get; }

        void Open();
        void BeginIr();
        void EndIr();
        void OutputPoint(uint number, bool value);
        void OutputPoint(uint number, bool value, int holdTime);
        //void OutputPointAsync(uint number, bool value, int holdTime);
        bool InputPoint(uint number);
        void OutputByte(uint number, byte value);
        void Output4Bit(uint[] numbers, byte value);
    }
}
