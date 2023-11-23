using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fujita.InspectionSystem;
using System.Runtime.InteropServices;
using Fujita.LightControl;

namespace LineCameraSheetSystem
{
    class SystemContext
    {
        public static SystemContext GetInstance()
        {
            return _singleton;
        }
        private static SystemContext _singleton = new SystemContext();

        private SystemContext()
        {
        }

        //LED照明の点灯時間の管理
        public clsMeasPeriod[] LightMeasPeriod;
        //LED照明の点灯時間監視中か
        public bool[] bLightMeas;
 
        //PuroductTime
        public clsProductTime ProductTime = new clsProductTime();
        //UPSモニタ
        public clsUpsShutdownMonitor UpsMonitor = new clsUpsShutdownMonitor();

        public void Initialize()
        {
            //Puroducttimeの初期化
             ProductTime.Initialize(SystemParam.GetInstance().ProductTimeFolder);

            //UPSモニタの初期化
            UpsMonitor.Initialize();
            UpsMonitor.ShutdownIntervalSec = SystemParam.GetInstance().AutoShutdownWaitSec;               
        }
        public void InitializeLightMeasPeriod()
        {
            LightControlManager ltCtrl = LightControlManager.getInstance();
            LightMeasPeriod = new clsMeasPeriod[ltCtrl.LightCount];
            bLightMeas = new bool[ltCtrl.LightCount];
            for (int i = 0; i < ltCtrl.LightCount; i++)
            {
                LightMeasPeriod[i] = new clsMeasPeriod();
                LightMeasPeriod[i].Initialize(i);
                bLightMeas[i] = false;
                LightMeasPeriod[i].Load(AppData.EXE_FOLDER + "LightMeasPeriod_" + i, "");
            }
        }

        public void Terminate()
        {
            UpsMonitor.Terminate();

            foreach (clsMeasPeriod mp in LightMeasPeriod)
                mp.Terminate();
        }

        private IFormForceCancel _ActiveForm;
        public IFormForceCancel ActiveForm
        {
            get
            {
                return _ActiveForm;
            }
            set
            {
                _ActiveForm = value;
            }
        }

    }
}
