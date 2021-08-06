using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HalconDotNet;
using System.Runtime.Serialization;

namespace VisionUtility
{
    public class CameralossArgs : EventArgs
    {
        public string Message;
        public CameralossArgs(string s)
            {
              Message = s;
            }
    }

        
    [Serializable]
    public class VisionCameraBase : IVisionCamera
    {
        public VisionCameraBase(CameraType cameraType)
        {
            CameraType = cameraType;
            LastCameraID++;
            CameraUserID = "Dev_" + LastCameraID.ToString();
        }

        /// <summary>最新设备编号 </summary>
        public static int LastCameraID = 0;
        private double _exposureTime = 0;
        private double _gain = 0;
        private int _width = 0;
        private int _height = 0;
        private bool _reverseX = false;
        private bool _reverseY = false;
        [NonSerialized]
        private HImage _image = new HImage();
        [NonSerialized]
        private AutoResetEvent _captureSignal = new AutoResetEvent(false);
        public event Action<HImage> DisplayImage;
        public CameraType CameraType { get; set; }
        public string CameraUserID { get; set; }
        public string CameraRemark { get; set; }
        public string CameraSerialNumber { get; set; }
        public string CameraMACAdress { get; set;}
        public virtual bool IsConnected { get; }
        public virtual event EventHandler <CameralossArgs> CameraLoss;
        public double ExposureTime
        {
            get { return _exposureTime; }
        }
        public double Gain
        {
            get { return _gain; }
        }
        public virtual bool ReverseX
        {
            get { return _reverseX; }
        }
        public virtual bool ReverseY
        {
            get { return _reverseY; }
        }
        public HImage Image
        {
            get { return _image; }
            set { _image = value; }
        }
        public int Width
        {
            get { return _width; }
        }
        public int Height
        {
            get { return _height; }
        }

        public AutoResetEvent CaptureSignal
        {
            get { return _captureSignal; }
            set { _captureSignal = value; }
        }
        /// <summary>
        /// 打开相机
        /// </summary>
        public virtual void Connect()
        {
        }

        /// <summary>
        /// 关闭相机
        /// </summary>
        public virtual void DisConnect()
        {
        }

        /// <summary>
        /// 复位或重连相机
        /// </summary>
        public virtual void Reset()
        {
        }

        /// <summary>
        /// 采集图像
        /// </summary>
        /// <returns></returns>
        public virtual bool CaptureImage()
        {
            return true;
        }

        /// <summary>
        /// 设置曝光时间
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool SetExposureTime(double val)
        {
            _exposureTime = val;
            return true;
        }
        public virtual bool SetReverseX(bool val)
        {
            _reverseX = val;
            return true;
        }

        public virtual bool SetReverseY(bool val)
        {
            _reverseY = val;
            return true;
        }

        /// <summary>
        /// 设置增益
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public virtual bool SetGain(double val)
        {
            _gain = val;
            return true;
        }

        /// <summary>
        /// 设置触发模式
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public virtual bool SetTriggerMode(TriggerMode mode)
        {
            return true;
        }


        protected virtual void OnDisplayImage(HImage image)
        {
            DisplayImage?.Invoke(image);
            CaptureSignal.Set();
        }



        [OnSerializing()]
        public void OnSerializing(StreamingContext context)
        {
            //DisplayImage = null;
        }

        [OnDeserializing()]
        internal void OnDeSerializingMethod(StreamingContext context)
        {
            _image = new HImage();
            _captureSignal = new AutoResetEvent(false);
        }
    }
}
