using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VisionFlows
{
    #region Enums

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
        LeftTop = 0,
        RightTop = 1,
        Bottom = 2
    }

    /// <summary>
    /// 吸嘴枚举
    /// </summary>
    public enum EnumNozzle
    {
        Left = 1,
        Right
    }

    public enum EnumNozzleCalibWork
    {
        CalibFront = 0,
        CalibBack,
        NozzleMark
    }

    /// <summary>
    /// 轴枚举
    /// </summary>
    public enum EnumAxis
    {
        XAxis = 1,         //X轴
        YAxis = 1,             //Y轴
        LeftZAxis = 2,         //Load升降轴
        LeftRAxis = 3,         //Load旋转轴
        RightZAxis = 4,        //Unload升降轴
        RightRAxis = 5        //Unload旋转轴
    }

    /// <summary>
    /// IO枚举
    /// </summary>
    public enum EnumCylinder
    {
        Tray1Cyl = 1,        //Load左侧Tray盘气缸
        Tray2Cyl,            //Load右侧Tray盘气缸
        Tray3Cyl,            //NGTray盘气缸
        Tray4Cyl,            //OKTray盘左侧气缸
        Tray5Cyl,            //OKTray盘右侧气缸
        LeftNozzleVacuum,    //上料真空阀
        RightNozzleVacuum,   //下料真空阀
        PreciseVacuum        //二次定位真空阀
    }

    /// <summary>
    /// 功能码枚举
    /// </summary>
    public enum EnumFunction
    {
        IsDUT = 0,      //有无产品
        IsReadSN = 1,   //是否读SN码
        IsLocation = 2, //是否产品定位
        IsSave          //是否保存图像
    }

    /// <summary>
    /// 视觉检测结果枚举
    /// </summary>
    public enum EnumResult
    {
        AllResult = 0,  //总结果
        NoDUT = 1,      //有无DUT
        NoSN = 2,       //有无SN码
        Abnormal = 7    //其它异常
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

    #endregion Enums

    #region Structs

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

    #endregion Structs

    public class Utility
    {
        #region Path

        //应用程序启动视觉相关文件根目录
        public static string Vision { get { return Application.StartupPath + @"\vision\"; } }

        //应用程序配置文件路径
        public static string Config { get { return Vision + @"configs\"; } }

        //应用程序图像文件路径
        public static string Image { get { return Vision + @"images\"; } }
        //标定文件
        public static string calib { get { return Vision + @"calib\"; } }


        //应用程序配方路径
        public static string type { get { return Vision + @"type\"; } }


        //应用程序模板文件路径
        public static string Model { get { return Vision + @"models\"; } }


        #endregion Path

        #region TempVar

        //
        public static int LightDelayTime = 1000;

        public static int CaptureDelayTime = 1500;
        public static bool OriginValue = false;

        //
        //public static Dictionary<string, HShapeModel> ShapeModel = new Dictionary<string, HShapeModel>();

        //public static Dictionary<string, HMetrologyModel> MetrologyModel = new Dictionary<string, HMetrologyModel>();
       // public static Dictionary<string, HHomMat2D> HomMat2D = new Dictionary<string, HHomMat2D>();
        //

        public static double SecondX = 0.0;
        public static double SecondY = 0.0;

        /***************************************************************/

        /*******************************************************/

        /// <summary>
        /// 本地测试时 图片从本地读取 实际运行时 图片从相机抓取
        /// </summary>
        public static GrabImageMode grabImageMode = GrabImageMode.FromCamera;

        public enum GrabImageMode
        {
            FromCamera,
            FromDisk,
        }

        /// <summary>
        /// 保存每个工位的拍照图片用于后面离线 测试图像定位算法
        /// </summary>
        public static bool IsSaveImage = true;

        /*******************************************************/

        #endregion TempVar

        #region GetBit

        /// <summary>
        /// 根据Int类型的值，返回用1或0(对应True或Flase)填充的数组
        /// <remarks>从右侧开始向左索引(0~31)</remarks>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IEnumerable<bool> GetBitList(int value)
        {
            var list = new List<bool>(32);
            for (var i = 0; i < 16; i++)
            {
                var val = 1 << i;
                list.Add((value & val) == val);
            }
            return list;
        }

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

        #endregion GetBit
    }
}