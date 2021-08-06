using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionFlows
{
    [Serializable]
    public class SystemPara
    {
        public static SystemPara Instance = new SystemPara();
        public string ImageSavePath { get; set; }
        public Point NozzleMarkAndCenterOffset { get; set; }
        public Point DutFrontMarkAndCenterOffset { get; set; }
        public Point DutBackMarkAndCenterOffset { get; set; }
        public Point SocketMarkAndCenterOffset { get; set; }
        public double SocketMarkAndCenterAngle { get; set; }
        public Point TraySlotAndCenterOffset { get; set; }
        public Point PreciseMarkAndCenterOffset { get; set; }
        public double Nozzle1AngleInInitStatus { get; set; }
        public Point Nozzle1CenterCoordiInInitStatus { get; set; }
        public Point Nozzle1RotateCenterCoordiInInitStatus { get; set; }

        //吸嘴和相机中心偏移
        public Point Cam1CenterAndNozzle1RotateCenterOffset { get; set; }

        public double Nozzle2AngleInInitStatus { get; set; }
        public Point Nozzle2CenterCoordiInInitStatus { get; set; }
        public Point Nozzle2RotateCenterCoordiInInitStatus { get; set; }

        public Point Cam2CenterAndNozzle2RotateCenterOffset { get; set; }
    }

    [Serializable]
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}