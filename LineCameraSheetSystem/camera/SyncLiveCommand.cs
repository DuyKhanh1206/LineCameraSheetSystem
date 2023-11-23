using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fujita.Communication;

namespace HalconCamera
{
    public class SyncLiveCommand : ILiveCommand
    {
        CommunicationDIO _dio;
        int _iTriggerNo;
        public SyncLiveCommand(CommunicationDIO dio, int iTrigNo)
        {
            _dio = dio;
            _iTriggerNo = iTrigNo;
        }

        public void PrevCommand(bool bLive)
        {
            if (!bLive)
            {
                _dio.OUT1(_iTriggerNo, false);
            }
        }

        public void AfterCommand(bool bLive)
        {
            if (bLive)
            {
                _dio.OUT1(_iTriggerNo, true);
            }
        }
    }
}
