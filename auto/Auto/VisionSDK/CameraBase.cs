using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Windows.Forms;
namespace VisionSDK
{
    public enum CameraConmand
    {
        ExtTrigger,
        SoftTrigger,
        Video,
    }
    public abstract class CameraBase
    {
        public HImage hPylonImage=new HImage();

        #region 相机参数

        public   Action<HImage, string> ImageEvent_vido; //连续采集回调函数
        public   Action<HImage, string> ImageEvent_ex; //连续采集回调函数
        public   Action<bool,string> Camera_loss; //相机丢失回调函数
        public abstract int ImageAngle { get; set; }
        public abstract string CameraSerialNumber { get;  }
        public abstract string CameraID { get;  }

        public abstract string DeviceMode { get;  }
        /// <summary>
        /// 当前曝光值
        /// </summary>
        public abstract long ShuterCur { get; set; }
        /// <summary>
        /// 最小曝光值
        /// </summary>
        public abstract long ShuterMin { get; }
        /// <summary>
        /// 最大曝光值
        /// </summary>
        public abstract long ShuterMax { get; }

        public  void ImageEvent_vido_Changed(HImage image,string CameraID)
        {
            ImageEvent_vido?.Invoke(image,CameraID);
        }
        public void ImageEvent_ex_Changed(HImage image, string CameraID)
        {
            ImageEvent_ex?.Invoke(image, CameraID);
        }
        public  void Camera_loss_Changed(bool state, string CameraID)
        {
            Camera_loss?.Invoke(state, CameraID);
        }
        

        /// <summary>
        /// 当前增益值
        /// </summary>
        public abstract double GainCur { get; set; }

        /// <summary>
        /// 最小增益值
        /// </summary>
        public abstract double GainMin { get; }

        /// <summary>
        /// 最大增益值
        /// </summary>

        public abstract double GainMax { get; }


        #endregion
        /// <summary>
        /// 异常信息
        /// </summary>
        public string ErrMessage { get; set; }
        /// <summary>
        /// 是否建立采集连接
        /// </summary>
        public abstract bool IsConnected { get; set; }
        /// <summary>
        /// 是否在连续采集中
        /// </summary>
        public bool IsContinuousShot { get; set; }
        public bool ignoreImage { get; set; }

        public bool IsExtTrigger { get; set; }
        public bool IsSoftTrigger { get; set; }

        public abstract bool Connect();
        /// <summary>
        /// 当前触发模式
        /// </summary>
        public abstract CameraConmand command { get; set; }
        /// <summary>

 
        protected abstract void GetCameraSettingData();//获取曝光增益等参数

         

        /// <summary>
        /// 关闭设备
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// 连续采集图像
        /// </summary>
        public abstract void ContinuousShot();

        /// <summary>
        /// 停止连续采集
        /// </summary>
        public abstract void ContinuousShotStop();

        public abstract bool SetGPIO(int index,bool low,int DelayTime);

        public abstract void UserSetSave();

        public abstract HImage GrabImage(int delayMs);

        public abstract HImage GrabImage_ex(int delayMs, int IO);

        /// <summary>
        /// 设置为外触发模式
        /// </summary>
        public abstract void SetExtTrigger();
        /// <summary>
        /// 设置为软触发模式
        /// </summary>
        public abstract void SetSoftTrigger();
    }

    public class ImageEventAgv : EventArgs, IDisposable
    {

        public HImage image;
        public int id;
        public String SerialNumber;
        public void Dispose()
        {

            GC.SuppressFinalize(this); //系统不会执行终结这个数据
        }
        public ImageEventAgv(HImage image, int id, string SerialNumber)
        {
            this.image = image;
            this.id = id;
            this.SerialNumber = SerialNumber;
        }

        ~ImageEventAgv()
        {
            this.Dispose();
        }

        public ImageEventAgv Clone()
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, this);

                BinaryFormatter b = new BinaryFormatter();
                object obj = b.Deserialize(objectStream);
                return obj as ImageEventAgv;
            }
        }

    }
}
