using System;
namespace VisionUtility
{
    #region"VisionCamera"
    /// <summary>
    /// 相机类型
    /// </summary>
    /// [Serializable]
    public enum CameraType
    {
        None=-1,
        Basler = 0,
        Daheng,
        File,
        Folder,
        Hikvision,
    }

    /// <summary>
    /// 触发模式
    /// </summary>
    [Serializable]
    public enum TriggerMode
    {
        软件触发 = 0,
        硬件触发,
        连续采集
        
    }

    #endregion

    #region"VisionModules"
    /// <summary>
    /// 图像调整
    /// </summary>
    [Serializable]
    public enum IMG_ADJUST
    {
        垂直镜像=0,
        水平镜像,
        顺时针90度,
        逆时针90度,
        旋转180度
    }


    /// <summary>
    /// 视觉单元类型枚举
    /// </summary>
    public enum ItemType
    {
        采集图像=0,//
        显示图像,
        存储图像,
        预先处理,
        创建ROI,//
        模板匹配,//
        直线测量,
        圆形测量,//
        矩形测量,//
        椭圆测量,//
        畸变标定,
        N点标定,//
        机械手控制,//
        查找二维码,
        字符识别

    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataType         ///复数全部用list来存储
    {
        数值型 = 0,                ///数值类型   float
        字符串,                  ///CString 字符串类型
        点2D,                    ///2D点
        点3D,                    ///3D点
        直线,                    ///直线
        圆,                      ///圆
        椭圆,                    ///椭圆
        坐标系,                  ///坐标系
        矩形阵列,                ///矩形阵列 rectInfo
        图像,                    ///图像
        位置转换2D,              ///HHomMat2D
        布尔型,                  ///布尔型
        旋转矩形,                ///旋转矩形    rectangle2_info
        平面,                  ///面
    }

    /// <summary>
    /// 自定义变量数据类型
    /// </summary>
    public enum DataGroup
    {
        单量 = 0,           ///单个变量
        数组,              ///数组类型
    }

    /// <summary>
    /// 变量归属
    /// </summary>
    public enum DataAtrribution
    {
        全局变量 = 0,              ///全局变量，但无需保存
        流程变量,                  ///流程变量，需要保存到本地
    }

    public enum DrawMode
    {
        none=0,
        draw,
        erase
    }

    #endregion

    #region"Other"
    /// <summary>
    /// 运行模式
    /// </summary>
    public enum RunMode
    {
        None = 0,
        单步运行,
        执行一次,
        循环运行,
    }

    [Serializable]
    public enum AcqMode
    {
        Camera=0,
        File,
        Folder,
       

    }

    public enum InstallMode
    {
        Fix = 0,
        Move
    }
    #endregion






















}
