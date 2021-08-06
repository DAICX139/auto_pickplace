using System;
using System.Collections.Generic;

namespace VisionFlows
{
    /// <summary>
    /// 算法枚举
    /// </summary>
    public enum EnumAlgorithm
    {
        CalibMark = 0,
        NozzleMark = 1,
        TrayMark = 2,
        TrayDutFront = 3,
        TraySlot = 4,
        SocketMark = 5,
        SocketDutFront = 6,
        SecondDutBack = 7,
        PreciseMark = 8
    }

    [Serializable]
    public class AlgorithmData
    {
        public static AlgorithmData Instance;
        public List<AlgorithmPara> AlgorithmParaList;
    }

    [Serializable]
    public class AlgorithmPara
    {
        public int AlgorithmID { get; set; }
        public string AlgorithmName { get; set; }
        public double ExposureTime { get; set; }
        public double Gain { get; set; }
        public double MatchMinScore { get; set; }
        public double MetroMinScore { get; set; }
        public string ShapeModelFileName { get; set; }
        public string MetrologyModelFileName { get; set; }
        //public string Reserve1 { get; set; }
        //public string Reserve2 { get; set; }
        //public string Reserve3 { get; set; }
        //public string Reserve4 { get; set; }
    }
}