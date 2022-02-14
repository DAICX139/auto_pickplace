using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VisionFlows
{
 

    /// <summary>
    /// 相机枚举
    /// </summary>
    public enum EnumCamera
    {
        LeftTop = 0,
        RightTop = 1,
        Bottom = 2
    }

    /// <summary>
    /// 光源枚举
    /// </summary>
    public enum EnumLight
    {
        LeftTop = 1,
        RightTop = 2,
        Bottom = 3
    }

    /// <summary>
    /// 轴枚举
    /// </summary>
    public enum EnumAxis
    {
        X = 1,
        Y = 2,
        Z1 = 3,
        R1=4,
        Z2=5,
        R2=6
    }

    public enum EnumPLCSend
    {
        XPos = 0,  //double
        YPos = 1,      //double
        ZPos = 2,      //double
        RPos = 3,      //double
        PosID = 4,     //uint16,PLC位置ID
        Func = 5       //uingt16,bit0:DUT有无,bit1:扫码（视觉配置是否扫码）,bit2:产品定位,bit3:拍照存图
    }

    /// <summary>
    /// 用于存储发送数据的结构体
    /// </summary>
    public enum EnumPLCRecv
    {
        XPos = 0,  //double
        YPos = 1,      //double
        ZPos = 2,      //double
        RPos = 3,      //double
        Result = 4,    //uint16,bit0:总结果,bit1:有无DUT,bit2:有无SN码,bit3:挑选结果,bit7:其他异常
        BinValue = 5   //uint16
    }

    /// <summary>
    /// PLC工位枚举
    /// </summary>
    public enum EnumPlcWork
    {
        LoadTray1ID = 1,
        LoadTray2ID = 2,
        NGTrayID = 3,

        //放的功能码
        OKTray1ID = 4,

        //取的功能码
        OKTray2ID = 5,

        LoadSecondPosID = 6,
        UnloadSecondPosID = 7,

        //二次定位功能码
        SocketPosID = 8,

        TrayMark = 9
    }

  

   

    /// <summary>
    /// 用于存储接收数据的结构体
    /// </summary>
    public struct PLCSend
    {
        public double XPos;
        public double YPos;
        public double ZPos;
        public double RPos;
        public int PosID;//
        public int Func;//bit0:DUT有无,bit1:扫码（视觉配置是否扫码）,bit2:产品定位,bit3:拍照存图
    }

    /// <summary>
    /// 用于存储发送数据的结构体
    /// </summary>
    public struct PLCRecv
    {
        public double XPos;
        public double YPos;
        public double ZPos;
        public double RPos;
        public int Result;//bit0:总结果,bit1:有无DUT,bit2:有无SN码,bit3:挑选结果,bit7:其他异常
        public int BinValue;
        public double RecvSocketID;//(Audit模式)
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

 

    public class Utility
    {
 

        //应用程序启动视觉相关文件根目录
        public static string Vision
        {
            get 
            { 
                var path = @"c:\ALCvision\";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            } 
        }

        //应用程序配置文件路径
        public static string ConfigFile
        { 
            get
            { 
                var path= Vision + @"configs\";
                if(!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        //应用程序图像文件路径
        public static string ImageFile 
        { 
            get 
            {
                var path = Vision + @"images\";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            } 
        }
        //标定文件
        public static string CalibFile
        {
            get 
            {
                var path = Vision + @"calib\";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            } 
        }


        //应用程序配方路径
        public static string TypeFile 
        { 
            get 
            { 
                var path = Vision + @"type\";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            }
        }


        //应用程序模板文件路径
        public static string ModelFile 
        { 
            get 
            { 
                var path = Vision + @"models\";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            } 
        }

        /// <summary>
        /// Hobject对象文件
        /// </summary>

        public static string HobjectFile
        {
            get
            {
                var path = Vision + @"hobjects\";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            }
        }


        public static string CalibImageFile
        {
            get 
            {
                var path =  ImageFile + @"calibimagefile\";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public static string TopLeftCalibImageFile
        {
            get
            {
                var path = CalibImageFile + @"TopLeftCalib\";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            }
        }

        public static string TopRightCalibImageFile
        {
            get
            {
                var path = CalibImageFile + @"TopRightCalib\";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            }
        }


        public static string BottomCalibImageFile
        {
            get
            {
                var path = CalibImageFile + @"BottomCalib\";
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                return path;
            }
        }



        public static int CaptureDelayTime = 3000;
        public static double SecondX = 0.0;
        public static double SecondY = 0.0;
        /// <summary>
        /// 保存每个工位的拍照图片用于后面离线 测试图像定位算法
        /// </summary>
        public static bool IsSaveAllImage = false;
        public static bool IsSaveNgImage = false;

 

 
        /// <summary>
        /// 返回Int数据中某一位是否为1
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index">32位数据的从右向左的偏移位索引(0~31)</param>
        /// <returns>true表示该位为1，false表示该位为0</returns>
        public static bool GetBitValue(int value, ushort index)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index"); //索引出错
            var val = 1 << index;
            return (value & val) == val;
        }

        /// <summary>
        /// 设定Int数据中某一位的值
        /// </summary>
        /// <param name="value">位设定前的值</param>
        /// <param name="index">32位数据的从右向左的偏移位索引(0~31)</param>
        /// <param name="bitValue">true设该位为1,false设为0</param>
        /// <returns>返回位设定后的值</returns>
        public static int SetBitValue(int value, ushort index, bool bitValue)
        {
            if (index > 31) throw new ArgumentOutOfRangeException("index"); //索引出错
            var val = 1 << index;
            return bitValue ? (value | val) : (value & ~val);
        }
 
    }
}