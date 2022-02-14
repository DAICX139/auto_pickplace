using Basler.Pylon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionSDK
{
    public class CameraManager
    {
        public static string CameraFlag = "Basler";
        public static List<CameraBase> CameraList = new List<CameraBase>();

        public static void Close()
        {
            foreach (var item in CameraList)
            {
                try
                {
                    item.Close();
                }
                catch (Exception ex)
                {
                }
            }
            CameraList = new List<CameraBase>();
        }

        /// <summary>
        /// 通过序列号查找相机
        /// </summary>
        /// <param name="CameraSerialNumber"></param>
        /// <returns></returns>
        public static CameraBase CameraBySerialNumber(string CameraSerialNumber)
        {
            return CameraList.Find(s => s.CameraSerialNumber == CameraSerialNumber);
        }

        /// <summary>
        /// 通过ID查找相机
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public static CameraBase CameraById(string UserID)
        {
            var cc = CameraList.Find(s => s.CameraID == UserID);
            return cc;
        }

        public static void open(ref List<CameraBase> CameraList, string CameraType)
        {
            try
            {
                if (CameraType == "basler")
                {
                    //打开相机列表
                    try
                    {
                        foreach (ICameraInfo info in CameraFinder.Enumerate())
                        {
                            ClassBasler device = new ClassBasler();
                            device.mCameraInfo = info;
                            device.camera = new Camera(device.CameraSerialNumber);
                            device.camera.ConnectionLost += device. OnConnectionLost;
                            device.camera.CameraOpened += device.OnCameraOpened;
                            device.camera.CameraClosed += device.OnCameraClosed;
                            device.camera.StreamGrabber.ImageGrabbed += device.ImageCallBack;
                            device.camera.Open();
                            device.camera.Parameters[PLTransportLayer.HeartbeatTimeout].TrySetValue(30000, IntegerValueCorrection.Nearest);
                            CameraList.Add(device);
                        }
                        return  ;
                    }
                    catch (Exception ex)
                    {

                        return  ;
                    }
                }
                
            }
            catch (Exception ex)
            {
            }
        }
    }
}
