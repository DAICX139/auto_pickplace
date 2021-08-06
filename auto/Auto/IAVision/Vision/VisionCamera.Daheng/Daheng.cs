using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using HalconDotNet;
using VisionUtility;
using GxIAPINET;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Threading;

namespace VisionCamera.Daheng
{
    [Serializable]
    public class Daheng : VisionCameraBase
    {
        public Daheng(CameraType cameraType) : base(cameraType)
        {
        }

        [NonSerialized]
        private static IGXFactory m_objIGXFactory = null;                    ///<Factory对像
        [NonSerialized]
        private IGXDevice m_objIGXDevice = null;                             ///<设备对像
        [NonSerialized]
        private IGXStream m_objIGXStream = null;                             ///<流对像
        [NonSerialized]
        private IGXFeatureControl m_objIGXFeatureControl = null;             ///<远端设备属性控制器对像
        [NonSerialized]
        private IGXFeatureControl m_objIGXStreamFeatureControl = null;       ///<流层属性控制器对象
        [NonSerialized]
        private bool m_bIsOpen = false;                                      ///<设备打开状态
        [NonSerialized]
        private bool m_bIsSnap = false;                                      ///<发送开采命令标识

        /// <summary>
        /// Indicates if the camera device is properly connected to the camera object while the camera object is open.
        /// </summary>
        public override bool IsConnected
        {
            get { return m_bIsOpen && m_bIsSnap; }
        }

        public static void EnumerateCameras(out List<VisionCameraInfo> cameraInfoList)
        {
            cameraInfoList = new List<VisionCameraInfo>();

            try
            {
                m_objIGXFactory = IGXFactory.GetInstance();
                m_objIGXFactory.Init();

                List<IGXDeviceInfo> m_listIGXDeviceInfo = new List<IGXDeviceInfo>();

                if (null != m_objIGXFactory)
                {
                    m_objIGXFactory.UpdateDeviceList(200, m_listIGXDeviceInfo);
                }

                // 判断当前连接设备个数
                if (m_listIGXDeviceInfo.Count <= 0)
                {
                    //MessageBox.Show("未检测到设备,请确保设备正常连接然后重启程序!");
                    return;
                }

                foreach (IGXDeviceInfo item in m_listIGXDeviceInfo)
                {
                    VisionCameraInfo cameraInfo = new VisionCameraInfo();

                    cameraInfo.CameraVendorName = item.GetVendorName();
                    cameraInfo.CameraModelName = item.GetModelName();
                    cameraInfo.CameraSerialNumber = item.GetSN();
                    cameraInfo.CameraUserID = item.GetUserID();
                    cameraInfo.CameraIPAddress = item.GetIP();
                    cameraInfo.CameraSubnetMask = item.GetSubnetMask();
                    cameraInfo.CameraMACAddress = item.GetMAC();
                    cameraInfo.CameraRemark = cameraInfo.CameraVendorName + "_"+ cameraInfo.CameraSerialNumber;
                    cameraInfoList.Add(cameraInfo);
                }
            }
            catch
            {
            }
        }

        public override void Connect()
        {
            try
            {
                //关闭流
                __CloseStream();
                // 如果设备已经打开则关闭，保证相机在初始化出错情况下能再次打开
                __CloseDevice();

                //打开设备
                m_objIGXDevice = m_objIGXFactory.OpenDeviceBySN(CameraSerialNumber, GX_ACCESS_MODE.GX_ACCESS_EXCLUSIVE);
                m_objIGXFeatureControl = m_objIGXDevice.GetRemoteFeatureControl();

                ////注册掉线事件，在关闭设备之前一定要注销事件
                //hDeviceOffline = m_objIGXDevice.RegisterDeviceOfflineCallback(null, OnDeviceOfflineEvent);

                //打开流
                if (null != m_objIGXDevice)
                {
                    m_objIGXStream = m_objIGXDevice.OpenStream(0);
                    m_objIGXStreamFeatureControl = m_objIGXStream.GetFeatureControl();
                }

                // 建议用户在打开网络相机之后，根据当前网络环境设置相机的流通道包长值，
                // 以提高网络相机的采集性能,设置方法参考以下代码。
                GX_DEVICE_CLASS_LIST objDeviceClass = m_objIGXDevice.GetDeviceInfo().GetDeviceClass();
                if (GX_DEVICE_CLASS_LIST.GX_DEVICE_CLASS_GEV == objDeviceClass)
                {
                    // 判断设备是否支持流通道数据包功能
                    if (true == m_objIGXFeatureControl.IsImplemented("GevSCPSPacketSize"))
                    {
                        // 获取当前网络环境的最优包长值
                        uint nPacketSize = m_objIGXStream.GetOptimalPacketSize();
                        // 将最优包长值设置为当前设备的流通道包长值
                        m_objIGXFeatureControl.GetIntFeature("GevSCPSPacketSize").SetValue(nPacketSize);
                    }
                }

                //初始化相机参数
                __InitDevice();

                // 更新设备打开标识
                m_bIsOpen = true;

                //开始采集
                Start();

            }
            catch(Exception ex)
            {
                m_bIsOpen = false;
            }
        }

        public override void DisConnect()
        {
            try
            {
                // 如果未停采则先停止采集
                if (m_bIsSnap)
                {
                    if (null != m_objIGXFeatureControl)
                    {
                        m_objIGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                        m_objIGXFeatureControl = null;
                    }
                }
            }
            catch
            {
            }

            m_bIsSnap = false;

            try
            {
                //停止流通道、注销采集回调和关闭流
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.StopGrab();
                    //注销采集回调函数
                    m_objIGXStream.UnregisterCaptureCallback();
                    m_objIGXStream.Close();
                    m_objIGXStream = null;
                    m_objIGXStreamFeatureControl = null;
                }
            }
            catch
            {
            }

            try
            {
                //注销掉线事件、关闭设备
                if (null != m_objIGXDevice)
                {
                    //m_objIGXDevice.UnregisterDeviceOfflineCallback(hDeviceOffline);
                    m_objIGXDevice.Close();
                    m_objIGXDevice = null;
                }
            }
            catch
            {
            }

            m_bIsOpen = false;
        }

        public override void Reset()
        {
            try 
            {
                if (m_objIGXFactory != null)
                {
                    //m_objIGXFactory.GigEResetDevice(CameraMACAdress, GX_RESET_DEVICE_MODE.GX_MANUFACTURER_SPECIFIC_RESET);
                    //Thread.Sleep(10000);
                    //List<VisionCameraInfo> cameraInfoList;
                    //EnumerateCameras(out cameraInfoList);

                    m_objIGXFactory.GigEResetDevice(CameraMACAdress, GX_RESET_DEVICE_MODE.GX_MANUFACTURER_SPECIFIC_RECONNECT);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public override bool CaptureImage()
        {
            try
            {
                SetTriggerMode(TriggerMode.软件触发);

                //发送软触发命令
                if (null != m_objIGXFeatureControl)
                {
                    m_objIGXFeatureControl.GetCommandFeature("TriggerSoftware").Execute();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool SetExposureTime(double val)
        {
            base.SetExposureTime(val);

            try
            {
                //获取当前相机的曝光值、最小值、最大值和单位
                if (null != m_objIGXFeatureControl)
                {
                    double dMin = 0.0;                       //最小值
                    double dMax = 0.0;                       //最大值

                    dMin = m_objIGXFeatureControl.GetFloatFeature("ExposureTime").GetMin();
                    dMax = m_objIGXFeatureControl.GetFloatFeature("ExposureTime").GetMax();
                    //判断输入值是否在曝光时间的范围内
                    //若大于最大值则将曝光值设为最大值
                    if (val > dMax)
                    {
                        val = dMax;
                    }
                    //若小于最小值将曝光值设为最小值
                    if (val < dMin)
                    {
                        val = dMin;
                    }

                    m_objIGXFeatureControl.GetFloatFeature("ExposureTime").SetValue(val);
                }

                return true;
            }
            catch
            {
                //ShowException(ex);
                return false;
            }
        }

        public override bool SetGain(double val)
        {
            base.SetGain(val);

            try
            {
                //当前相机的增益值、最小值、最大值
                if (null != m_objIGXFeatureControl)
                {
                    double dMin = 0.0;                       //最小值
                    double dMax = 0.0;                       //最大值

                    dMin = m_objIGXFeatureControl.GetFloatFeature("Gain").GetMin();
                    dMax = m_objIGXFeatureControl.GetFloatFeature("Gain").GetMax();

                    //判断输入值是否在增益值的范围内
                    //若输入的值大于最大值则将增益值设置成最大值
                    if (val > dMax)
                    {
                        val = dMax;
                    }
                    //若小于最小值将增益值设为最小值
                    if (val < dMin)
                    {
                        val = dMin;
                    }

                    m_objIGXFeatureControl.GetFloatFeature("Gain").SetValue(val);
                }

                return true;

            }
            catch
            {
                //ShowException(ex);
                return false;
            }
        }

        public override bool SetTriggerMode(TriggerMode mode)
        {
            try
            {
                ////如果是连续采集，则先停止
                //if (m_bIsOpen==true)
                //{
                //    Stop();
                //}

                switch (mode)
                {
                    case TriggerMode.软件触发:
                        m_objIGXFeatureControl.GetEnumFeature("AcquisitionMode").SetValue("Continuous");
                        m_objIGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("On");
                        m_objIGXFeatureControl.GetEnumFeature("TriggerSource").SetValue("Software");
                        break;
                    case TriggerMode.硬件触发:
                        //m_objIGXFeatureControl.GetEnumFeature("AcquisitionMode").SetValue("Continuous");
                        //m_objIGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("On");
                        //m_objIGXFeatureControl.GetEnumFeature("TriggerSource").SetValue("Line0");

                        break;
                    case TriggerMode.连续采集:
                        m_objIGXFeatureControl.GetEnumFeature("AcquisitionMode").SetValue("Continuous");
                        m_objIGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("Off");
                        break;
                    default:
                        break;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        /// <returns></returns>
        public void Start()
        {
            try
            {
                if (null != m_objIGXStreamFeatureControl)
                {
                    //设置流层Buffer处理模式为OldestFirst
                    m_objIGXStreamFeatureControl.GetEnumFeature("StreamBufferHandlingMode").SetValue("OldestFirst");
                }

                //开启采集流通道
                if (null != m_objIGXStream)
                {
                    //RegisterCaptureCallback第一个参数属于用户自定参数(类型必须为引用
                    //类型)，若用户想用这个参数可以在委托函数中进行使用
                    m_objIGXStream.RegisterCaptureCallback(this, __CaptureCallbackPro);
                    m_objIGXStream.StartGrab();
                }

                //发送开采命令
                if (null != m_objIGXFeatureControl)
                {
                    m_objIGXFeatureControl.GetCommandFeature("AcquisitionStart").Execute();
                }
                m_bIsSnap = true;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        /// <returns></returns>
        public void Stop()
        {
            try
            {
                //发送停采命令
                if (null != m_objIGXFeatureControl)
                {
                    m_objIGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                }

                //关闭采集流通道
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.StopGrab();
                    //注销采集回调函数
                    m_objIGXStream.UnregisterCaptureCallback();
                }

                m_bIsSnap = false;

            }
            catch
            {
            }
        }

        /// <summary>
        /// 相机初始化
        /// </summary>
        void __InitDevice()
        {
            if (null != m_objIGXFeatureControl)
            {
                m_objIGXFeatureControl.GetEnumFeature("AcquisitionMode").SetValue("Continuous");
                m_objIGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("On");
                m_objIGXFeatureControl.GetEnumFeature("TriggerSource").SetValue("Software");
            }
        }

        /// <summary>
        /// 关闭流
        /// </summary>
        private void __CloseStream()
        {
            try
            {
                //关闭流
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.Close();
                    m_objIGXStream = null;
                    m_objIGXStreamFeatureControl = null;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        private void __CloseDevice()
        {
            try
            {
                //关闭设备
                if (null != m_objIGXDevice)
                {
                    m_objIGXDevice.Close();
                    m_objIGXDevice = null;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 回调函数,用于获取图像信息和显示图像
        /// </summary>
        /// <param name="obj">用户自定义传入参数</param>
        /// <param name="objIFrameData">图像信息对象</param>
        private void __CaptureCallbackPro(object objUserParam, IFrameData objIFrameData)
        {
            try
            {
                //GX_VALID_BIT_LIST emValidBits = GX_VALID_BIT_LIST.GX_BIT_0_7;
                if (null != objIFrameData)
                {
                    IntPtr pBufferMono = IntPtr.Zero;
                    pBufferMono = objIFrameData.GetBuffer();
                    byte[] byImageBuffer = new byte[objIFrameData.GetWidth() * objIFrameData.GetHeight()];

                    Marshal.Copy(pBufferMono, byImageBuffer, 0, (int)(objIFrameData.GetWidth() * objIFrameData.GetHeight()));
                    Image.GenImage1("byte", (int)objIFrameData.GetWidth(), (int)objIFrameData.GetHeight(), pBufferMono);

                    base.OnDisplayImage(Image);
                }
            }
            catch(Exception ex)
            {
                
            }
        }

        [OnDeserializing()]
        internal void OnDeSerializingMethod(StreamingContext context)
        {
        }

        [OnDeserialized()]
        internal void OnDeSerializedMethod(StreamingContext context)
        {
            m_bIsOpen = false;
            m_bIsSnap = false;

            m_objIGXFactory = IGXFactory.GetInstance();
            m_objIGXFactory.Init();

            List<IGXDeviceInfo> m_listIGXDeviceInfo = new List<IGXDeviceInfo>();

            if (null != m_objIGXFactory)
            {
                m_objIGXFactory.UpdateDeviceList(200, m_listIGXDeviceInfo);
            }
        }

    }
}
