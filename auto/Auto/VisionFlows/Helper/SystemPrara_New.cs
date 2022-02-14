using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionFlows
{
    /// <summary>
    /// 使用新标定方法的系统参数参数
    /// </summary>
    [Serializable]
    public class SystemPrara
    {
        public static SystemPrara Instance = new SystemPrara();
        public bool UseNewCalib;

        //吸嘴1去槽放料
        public double Nozzle1_Slot_CompensateX;

        public double Nozzle1_Slot_CompensateY;
        public double Nozzle1_Slot_CompensateR;

        //吸嘴2去槽放料
        public double Nozzle2_Slot_CompensateX;

        public double Nozzle2_Slot_CompensateY;
        public double Nozzle2_Slot_CompensateR;

        //吸嘴1去槽取料
        public double Nozzle1_TrayDUT_CompensateX;

        public double Nozzle1_TrayDUT_CompensateY;
        public double Nozzle1_TrayDUT_CompensateR;

        //吸嘴2去槽取料
        public double Nozzle2_TrayDUT_CompensateX;

        public double Nozzle2_TrayDUT_CompensateY;
        public double Nozzle2_TrayDUT_CompensateR;

        //吸嘴1去Socket取料
        public double Nozzle1_Socket_Get_DUT_CompensateX;

        public double Nozzle1_Socket_Get_DUT_CompensateY;
        public double Nozzle1_Socket_Get_DUT_CompensateR;

        //吸嘴1去Socket放料
        public double Nozzle1_Socket_Put_DUT_CompensateX;

        public double Nozzle1_Socket_Put_DUT_CompensateY;
        public double Nozzle1_Socket_Put_DUT_CompensateR;

        //吸嘴2去Socket取料
        public double Nozzle2_Socket_Get_DUT_CompensateX;

        public double Nozzle2_Socket_Get_DUT_CompensateY;
        public double Nozzle2_Socket_Get_DUT_CompensateR;

        //吸嘴2去Socket放料
        public double Nozzle2_Socket_Put_DUT_CompensateX;

        public double Nozzle2_Socket_Put_DUT_CompensateY;
        public double Nozzle2_Socket_Put_DUT_CompensateR;

        public SystemPrara()
        {
            UseNewCalib = false;
            //吸嘴1去槽放料
            Nozzle1_Slot_CompensateX = 0;
            Nozzle1_Slot_CompensateY = 0;
            Nozzle1_Slot_CompensateR = 0;

            //吸嘴2去槽放料
            Nozzle2_Slot_CompensateX = 0;
            Nozzle2_Slot_CompensateY = 0;
            Nozzle2_Slot_CompensateR = 0;

            //吸嘴1去槽取料
            Nozzle1_TrayDUT_CompensateX = 0;
            Nozzle1_TrayDUT_CompensateY = 0;
            Nozzle1_TrayDUT_CompensateR = 0;

            //吸嘴2去槽取料
            Nozzle2_TrayDUT_CompensateX = 0;
            Nozzle2_TrayDUT_CompensateY = 0;
            Nozzle2_TrayDUT_CompensateR = 0;

            //吸嘴1去Socket取料
            Nozzle1_Socket_Get_DUT_CompensateX = 0;
            Nozzle1_Socket_Get_DUT_CompensateY = 0;
            Nozzle1_Socket_Get_DUT_CompensateR = 0;

            //吸嘴1去Socket放料
            Nozzle1_Socket_Put_DUT_CompensateX = 0;
            Nozzle1_Socket_Put_DUT_CompensateY = 0;
            Nozzle1_Socket_Put_DUT_CompensateR = 0;

            //吸嘴2去Socket取料
            Nozzle2_Socket_Get_DUT_CompensateX = 0;
            Nozzle2_Socket_Get_DUT_CompensateY = 0;
            Nozzle2_Socket_Get_DUT_CompensateR = 0;

            //吸嘴2去Socket放料
            Nozzle2_Socket_Put_DUT_CompensateX = 0;
            Nozzle2_Socket_Put_DUT_CompensateY = 0;
            Nozzle2_Socket_Put_DUT_CompensateR = 0;
        }
    }
}