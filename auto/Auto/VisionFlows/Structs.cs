using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionFlows
{
    /// <summary>
    /// 用于存储接收数据的结构体
    /// </summary>
    public struct RecData
    {
        public double XPos;
        public double YPos;
        public double RDegree;
        public ushort Mode;//模式0:DUT，1: Mark，2：挑选模式
        public ushort Func;//bit0:DUT有无,bit1:扫码（视觉配置是否扫码）,bit2:产品定位,bit3:拍照存图
    }
    /// <summary>
    /// 用于存储发送数据的结构体
    /// </summary>
    public struct SendData
    {
        public double XPosMark;
        public double YPosMark;
        public double RDegreeMark;
        public ushort Result;//bit0:总结果,bit1:有无DUT,bit2:有无SN码,bit3:挑选结果,bit7:其他异常
        public ushort BinValue;
    }

    /// <summary>
    /// 轴状态结构体
    /// </summary>
    public struct AxisStatus
    {
        public bool Enable;
        public bool Homed;
        public bool Busy;
        public bool Done;
        public bool Error;
        public uint ErrorID;
        public double ActPos;
        public double ActVel;
    }
    
}
