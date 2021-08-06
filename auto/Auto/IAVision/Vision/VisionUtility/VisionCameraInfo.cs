using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionUtility
{
    public class VisionCameraInfo
    {
        /// <summary>厂商名称 </summary>
        public string CameraVendorName{set;get; }
        /// <summary>相机型号 </summary>
        public string CameraModelName { set; get; }
        /// <summary>相机序列号 </summary>
        public string CameraSerialNumber { set; get; }
        /// <summary>用户自定义名称 </summary>
        public string CameraUserID { set; get; }

        /// <summary>相机IP地址 </summary>
        public string CameraIPAddress { set; get; }
        /// <summary>相机子网掩码 </summary>
        public string CameraSubnetMask { set; get; }

        /// <summary>相机MAC地址 </summary>
        public string CameraMACAddress { set; get; }
        /// <summary>相机MAC地址 </summary>
        public string CameraRemark { set; get; }
    }
}
