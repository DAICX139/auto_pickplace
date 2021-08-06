using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HalconDotNet;
using System.Threading;

namespace VisionUtility
{
    public interface IVisionCamera
    {
        CameraType CameraType { get; set; }//相机类型
        string CameraUserID { get; set; }//相机用户ID
        string CameraSerialNumber { get; set; }//相机序列号
        string CameraRemark { get; set; }//相机备注
        bool IsConnected { get;}//相机连接状态
        double ExposureTime{ get; }
        double Gain { get; }
        int Width { get; }//采集图像宽度
        int Height { get; }//采集图像高度
        bool ReverseX { get; }
        bool ReverseY { get; }
        HImage Image { get; set; }//采集图像
        //AutoResetEvent CaptureSignal { get; set; }//采集信号，采集前复位，采集后等待
        event Action<HImage> DisplayImage;
      

        //
        void Connect();//打开相机
        void DisConnect();//关闭相机
        bool CaptureImage();//采集图像
        bool SetExposureTime(double val);//设置曝光时间
        bool SetGain(double val);//设置增益
        bool SetTriggerMode(TriggerMode mode);//设置触发模式
        bool SetReverseX(bool val);//设置X翻转
        bool SetReverseY(bool val);//设置Y翻转
        void Reset(); //复位或重连相机


    }
}
