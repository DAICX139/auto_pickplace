using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionFlows
{
    /// <summary>
    /// AutoNormal模式上位机拍照位置枚举，命名方式：功能+工位
    /// </summary>
    public enum EnumAcqPosi
    {
        Cam1Tray1Mark = 0,
        Cam1Tray1Slot,
        Cam1Tray2Mark,
        Cam1Tray2Slot,
        Cam1Tray3Mark,
        Cam1Tray3Slot,
        Cam1Tray4Mark,
        Cam1Tray4Slot,
        Cam1SocketMark,
        Cam1PreciseMark,
        Cam2Tray2Mark,
        Cam2Tray2Slot,
        Cam2Tray3Mark,
        Cam2Tray3Slot,
        Cam2Tray4Mark,
        Cam2Tray4Slot,
        Cam2Tray5Mark,
        Cam2Tray5Slot,
        Cam2SocketMark,
        Cam2PreciseMark,
        Cam3Nozzle1Mark,
        Cam3Nozzle2Mark,
        None
    }

    [Serializable]
    public class AcqPosiData
    {
        public static AcqPosiData Instance;
        public List<AcqPosiPara> AcqPosiParaList;
    }

    [Serializable]
    public class AcqPosiPara
    {
        public int PosiID { get; set; }
        public string PosiName { get; set; }
        public int CameraID { get; set; }//
        public int XAxisID { get; set; }
        public int YAxisID { get; set; }
        public int ZAxisID { get; set; }
        public int RAxisID { get; set; }
        public double BaseX { get; set; }
        public double BaseY { get; set; }
        public double BaseZ { get; set; }
        public double BaseR { get; set; }
        public double OffsetX { get; set; }
        public double OffsetY { get; set; }
    }
}