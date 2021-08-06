using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionFlows
{
    /// <summary>
    /// 相机枚举
    /// </summary>
    public enum EnumCamera
    {
        LeftTop = 0,
        RightTop,   
        Bottom, 
    }
    /// <summary>
    /// 光源枚举
    /// </summary>
    public enum EnumLight
    {
        LeftTop = 1,
        RightTop,
        Bottom
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
        YAxis,             //Y轴
        LeftZAxis,         //Load升降轴
        LeftRAxis,         //Load旋转轴
        RightZAxis,        //Unload升降轴
        RightRAxis         //Unload旋转轴
    }
    /// <summary>
    /// 气缸枚举
    /// </summary>
    public enum EnumCylinder
    {
        LoadTrayCyl1 = 1,   //Load左侧Tray盘气缸
        LoadTrayCyl2,       //Load右侧Tray盘气缸
        NGTrayCyl,          //NGTray盘气缸
        OKTrayLCyl1,        //OKTray盘左侧气缸
        OKTrayCyl2,         //OKTray盘右侧气缸
        LoadVacuum,         //上料真空阀
        UloadVacuum,        //下料真空阀
        SecondVacuum        //二次定位真空阀
    }
    /// <summary>
    /// PLC工位枚举
    /// </summary>
    public enum EnumPlcWork
    {
        LoadTray1ID = 1,
        LoadTray2ID,
        NGTrayID,
        OKTray1ID,
        OKTray2ID,
        LoadSecondPosID,
        UnloadSecondPosID,
        SocketPosID,
    }
    /// <summary>
    /// AutoNormal模式上位机工位枚举，命名方式：功能+工位
    /// </summary>
    public enum EnumAutoNormal
    {
        LoadTray1Mark = 0,
        LoadTray1Dut,
        LoadTray2Mark,
        LoadTray2Dut,
        LoadNGTrayMark,
        LoadNGTrayDut,
        LoadSecond,
        LoadSocket,
        UnloadOKTray1Mark,
        UnloadOKTray1Dut,
        UnloadOKTray2Mark,
        UnloadOKTray2Dut,
        UnloadNGTrayMark,
        UnloadNGTrayDut,
        UnloadSecond,
        UnloadSocket,
        None
    }
    /// <summary>
    /// AutoSelect模式上位机工位命名方式：功能+工位
    /// </summary>
    public enum EnumAutoSelect
    {
        LoadTray1Mark = 0,
        LoadTray1Dut,
        LoadTray2Mark,
        LoadTray2Dut,
        UnloadNGTrayMark,
        UnloadNGTrayDut,
        UnloadOKTray1Mark,
        UnloadOKTray1Dut,
        UnloadOKTray2Mark,
        UnloadOKTray2Dut,
        None
    }
    /// <summary>
    /// 功能码枚举
    /// </summary>
    public enum EnumFunction
    {
        NoDUT = 0,
        NoCode,
        Normal
    }
    /// <summary>
    /// 视觉检测结果枚举
    /// </summary>
    public enum EnumResult
    {
        NoDUT = 0,
        NoCode,
        Normal
    }
}
