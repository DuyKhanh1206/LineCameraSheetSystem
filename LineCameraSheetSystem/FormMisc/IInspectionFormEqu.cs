using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fujita.InspectionSystem
{
    public interface IInspectionFormEqu
    {
        void FormControl(string type, object[] paramArray);
    }
}
