using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

using VisionUtility;
using Basler.Pylon;
using System.Diagnostics;
using HalconDotNet;
using System.Runtime.Serialization;

namespace VisionCamera.Basler
{
    [Serializable]
    public class Basler : VisionCameraBase
    {
        public Basler(CameraType cameraType) : base(cameraType)
        {

        }
        [NonSerialized]
        private Version sfnc2_0_0 = new Version(2, 0, 0);
        [NonSerialized]
        private Camera camera = null;//相机对象
        [NonSerialized]
        private PixelDataConverter converter = new PixelDataConverter();//图像格式转换器
        [NonSerialized]
        private Stopwatch stopWatch = new Stopwatch();

        /// <summary>
        /// Indicates if the camera device is properly connected to the camera object while the camera object is open.
        /// </summary>
        public override bool IsConnected
        {
            get
            {
                if (camera == null) return false;
                return camera.IsConnected;
            }
        }
        public static void EnumerateCameras(out List<VisionCameraInfo> cameraInfoList)
        {
            cameraInfoList = new List<VisionCameraInfo>();
            try
            {
                foreach (ICameraInfo item in CameraFinder.Enumerate())
                {
                    VisionCameraInfo cameraInfo = new VisionCameraInfo();
                    cameraInfo.CameraVendorName = item[CameraInfoKey.VendorName];
                    cameraInfo.CameraModelName = item[CameraInfoKey.ModelName];
                    cameraInfo.CameraSerialNumber = item[CameraInfoKey.SerialNumber];
                    cameraInfo.CameraUserID = item[CameraInfoKey.UserDefinedName];
                    cameraInfo.CameraIPAddress = item[CameraInfoKey.DeviceIpAddress];
                    cameraInfo.CameraSubnetMask = item[CameraInfoKey.SubnetMask];
                    cameraInfo.CameraMACAddress = item[CameraInfoKey.DeviceMacAddress];
                    cameraInfo.CameraRemark = cameraInfo.CameraVendorName + "_" + cameraInfo.CameraSerialNumber;
                    cameraInfoList.Add(cameraInfo);
                }
            }
            catch (Exception ex)
            {
            }
        }
        public override event EventHandler <CameralossArgs> CameraLoss;

        public override void Connect()
        {
            try
            {
                
                //base.Open();
                // Destroy the old camera object.
                DisConnect();
                camera = new Camera(CameraSerialNumber);
                //Changes the configuration of the camera to software triggered acquisition.
                camera.CameraOpened += Configuration.AcquireSingleFrame;
                // Register for the events of the image provider needed for proper operation. 
                camera.ConnectionLost += OnConnectionLost;
                camera.CameraOpened += OnCameraOpened;
                camera.CameraClosed += OnCameraClosed;
                camera.StreamGrabber.GrabStarted += OnGrabStarted;
                camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;
                camera.StreamGrabber.GrabStopped += OnGrabStopped;
                //
                camera.Open();
               camera.Parameters[PLTransportLayer.HeartbeatTimeout].TrySetValue(30000, IntegerValueCorrection.Nearest);
                //IsOpen = true;
            }
            catch (Exception ex)
            {
                //IsOpen = false;
            }
        }
  
        public override void DisConnect()
        {
            //base.Close();
            // Destroy the camera object.
            try
            {
                if (camera != null)
                {
                    camera.Close();
                    camera.Dispose();
                    camera = null;
                }
            }
            catch
            {
                //ShowException(ex);
            }
            finally
            {
                //IsOpen = false;
            }
        }

        public override bool CaptureImage()
        {
            try
            {
                // Starts the grabbing of one image.
                camera.StreamGrabber.Start(1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override bool SetExposureTime(double val)
        {
            try
            {
                if (camera.Parameters.Contains(PLCamera.ExposureTimeAbs))
                {
                    camera.Parameters[PLCamera.ExposureTimeAbs].SetValue(val);
                }
                else
                {
                    if (camera.GetSfncVersion() < sfnc2_0_0)
                    {
                        // In previous SFNC versions, ExposureTimeRaw is an integer parameter.
                        camera.Parameters[PLCamera.ExposureTimeRaw].SetValue((long)val);
                    }
                    else
                    {
                        // In SFNC 2.0, ExposureTime is a float parameter.
                        camera.Parameters[PLCamera.ExposureTime].SetValue(val);
                    }
                }
            }
            catch (Exception ex)
            {
                //ShowException(ex);
                return false;
            }

            base.SetExposureTime(val);
            return true;
        }

        public override bool SetGain(double val)
        {
            try
            {
                if (camera.Parameters.Contains(PLCamera.GainAbs))
                {
                    camera.Parameters[PLCamera.GainAbs].SetValue(val);
                }
                else
                {
                    if (camera.GetSfncVersion() < sfnc2_0_0)
                    {
                        // In previous SFNC versions, GainRaw is an integer parameter.
                        camera.Parameters[PLCamera.GainRaw].SetValue((long)val);
                    }
                    else
                    {
                        // In SFNC 2.0, Gain is a float parameter.
                        camera.Parameters[PLCamera.Gain].SetValue(val);
                    }
                }
            }
            catch (Exception ex)
            {
                //ShowException(ex);
                return false;
            }

            base.SetGain(val);
            return true;

        }

        public override bool SetTriggerMode(TriggerMode mode)
        {
            try
            {
                //如果是连续采集，则先停止
                string str = camera.Parameters[PLCamera.AcquisitionMode].GetValue();
                if (camera.StreamGrabber.IsGrabbing && camera.Parameters[PLCamera.AcquisitionMode].GetValue() == PLCamera.AcquisitionMode.Continuous)
                {
                    Stop();
                }

                switch (mode)
                {
                    case TriggerMode.软件触发:
                        camera.Parameters[PLCamera.TriggerMode].SetValue(PLCamera.TriggerMode.Off);
                        // Starts the grabbing of one image.
                        camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.SingleFrame);

                        //OneShot();
                        break;
                    case TriggerMode.硬件触发:
                        camera.Parameters[PLCamera.TriggerMode].SetValue(PLCamera.TriggerMode.On);
                        //camera.Parameters[PLCamera.TriggerSource].SetValue(PLCamera.TriggerSource.Line1);
                        //camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);

                        break;
                    case TriggerMode.连续采集:
                        camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
                        // Start the grabbing of images until grabbing is stopped.
                        camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
                        break;
                    default:
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public override bool SetReverseX(bool val)
        {
            try
            {
                camera.Parameters[PLCamera.ReverseX].SetValue(val);
            }
            catch (Exception ex)
            {
                //ShowException(ex);
                return false;
            }
            base.SetReverseX(val);
            return true;
        }

        public override bool SetReverseY(bool val)
        {
            try
            {
                camera.Parameters[PLCamera.ReverseY].SetValue(val);
            }
            catch (Exception ex)
            {
                //ShowException(ex);
                return false;
            }
            base.SetReverseY(val);
            return true;
        }

        // Occurs when a device with an opened connection is removed.
        private void OnConnectionLost(Object sender, EventArgs e)
        {
            // Close the camera object.
            DisConnect();
            CameralossArgs Args = new CameralossArgs("loss");
            if(CameraLoss!=null)
            {
                CameraLoss(null, Args);
            }
           
        }


        // Occurs when the connection to a camera device is opened.
        private void OnCameraOpened(Object sender, EventArgs e)
        {
            CameralossArgs Args = new CameralossArgs("open");
            if(CameraLoss!=null)
            {
                CameraLoss(null, Args);
            }

        }


        // Occurs when the connection to a camera device is closed.
        private void OnCameraClosed(Object sender, EventArgs e)
        {
            //...
        }


        // Occurs when a camera starts grabbing.
        private void OnGrabStarted(Object sender, EventArgs e)
        {
            // Reset the stopwatch used to reduce the amount of displayed images. The camera may acquire images faster than the images can be displayed.
            stopWatch.Reset();
        }


        // Occurs when an image has been acquired and is ready to be processed.
        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            try
            {
                IGrabResult grabResult = e.GrabResult;
                if (grabResult.IsValid)
                {
                    if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 33)
                    {
                        stopWatch.Restart();
                        Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
                        BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                        //// Place the pointer to the buffer of the bitmap.//暂时不需要，使用默认格式
                        //converter.OutputPixelFormat = PixelType.BGRA8packed;
                        IntPtr ptrBmp = bmpData.Scan0;
                        converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult); 
                        bitmap.UnlockBits(bmpData);
                        int width = grabResult.Width;
                        int height = grabResult.Height;
                        Image = new HImage();
                        Image.GenImage1("byte", width, height, ptrBmp);
                        base.OnDisplayImage(Image.CopyImage());
                        Image.Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
            finally
            {
                // Dispose the grab result if needed for returning it to the grab loop.
                e.DisposeGrabResultIfClone();
            }
        }

        // Occurs when a camera has stopped grabbing.
        private void OnGrabStopped(Object sender, GrabStopEventArgs e)
        {
            // Reset the stopwatch.
            stopWatch.Reset();

            // If the grabbed stop due to an error, display the error message.
            if (e.Reason != GrabStopReason.UserRequest)
            {
                //MessageBox.Show("A grab error occured:\n" + e.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Shows exceptions in a message box.
        private void ShowException(Exception exception)
        {
            //MessageBox.Show("Exception caught:\n" + exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //errorMessage = cameraName + ":" + exception.Message;
        }

        public void OneShot()
        {
            try
            {
                // Starts the grabbing of one image.
                camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.SingleFrame);
                camera.StreamGrabber.Start(1, GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }

        // Starts the continuous grabbing of images and handles exceptions.
        public  void ContinuousShot()
        {
            try
            {
                // Start the grabbing of images until grabbing is stopped.
                camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
                camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }

        //stop image acquisition
        public void Stop()
        {
            // Stop the grabbing.
            try
            {
                camera.StreamGrabber.Stop();
            }
            catch (Exception exception)
            {
                ShowException(exception);
            }
        }

        [OnDeserializing()]
        internal void OnDeSerializingMethod(StreamingContext context)
        {
        }

        [OnDeserialized()]
        internal void OnDeSerializedMethod(StreamingContext context)
        {
            sfnc2_0_0 = new Version(2, 0, 0);//the GenICam Standard Feature Naming Convention (SFNC)
            converter = new PixelDataConverter();//图像格式转换器
            stopWatch = new Stopwatch();

            //m_bIsOpen = false;
            //m_bIsSnap = false;

            //m_objIGXFactory = IGXFactory.GetInstance();

            //m_objIGXFactory.Init();

            //List<IGXDeviceInfo> m_listIGXDeviceInfo = new List<IGXDeviceInfo>();

            //if (null != m_objIGXFactory)
            //{
            //    m_objIGXFactory.UpdateDeviceList(200, m_listIGXDeviceInfo);
            //}
        }



    }
}

