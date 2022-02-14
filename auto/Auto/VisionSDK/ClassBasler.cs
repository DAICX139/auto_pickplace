using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Basler.Pylon;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Runtime.InteropServices;

namespace VisionSDK
{
    class ClassBasler : CameraBase
    {
        private Version sfnc2_0_0 = new Version(2, 0, 0);
        public Camera camera = null;//相机对象
        private PixelDataConverter converter = new PixelDataConverter();//图像格式转换器
        private IntPtr latestFrameAddress = IntPtr.Zero;
        public ICameraInfo mCameraInfo;
        public override int ImageAngle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        ManualResetEvent CallBackEvent = new ManualResetEvent(false);
        public override string CameraSerialNumber => mCameraInfo[CameraInfoKey.SerialNumber];
        public override string CameraID => mCameraInfo[CameraInfoKey.UserDefinedName];
        public override string DeviceMode => throw new NotImplementedException();
        public override long ShuterCur 
        { 
            get
            {
                if (camera.Parameters.Contains(PLCamera.ExposureTimeAbs))
                {
                   return (long)camera.Parameters[PLCamera.ExposureTimeAbs].GetValue();
                }
                else
                {
                    if (camera.GetSfncVersion() < sfnc2_0_0)
                    {
                       return camera.Parameters[PLCamera.ExposureTimeRaw].GetValue();
                    }
                    else
                    {
                       return (long)camera.Parameters[PLCamera.ExposureTime].GetValue();
                    }
                }
            }
            set
            {
                if (camera.Parameters.Contains(PLCamera.ExposureTimeAbs))
                {
                    camera.Parameters[PLCamera.ExposureTimeAbs].SetValue(value);
                }
                else
                {
                    if (camera.GetSfncVersion() < sfnc2_0_0)
                    {
                        camera.Parameters[PLCamera.ExposureTimeRaw].SetValue((long)value);
                    }
                    else
                    {
                        camera.Parameters[PLCamera.ExposureTime].SetValue(value);
                    }
                }
            }
        }

        public override long ShuterMin
        {
            get
            {
                if (camera.Parameters.Contains(PLCamera.ExposureTimeAbs))
                {
                    return (long)camera.Parameters[PLCamera.ExposureTimeAbs].GetMinimum();
                }
                else
                {
                    if (camera.GetSfncVersion() < sfnc2_0_0)
                    {
                        return camera.Parameters[PLCamera.ExposureTimeRaw].GetMinimum();
                    }
                    else
                    {
                        return (long)camera.Parameters[PLCamera.ExposureTime].GetMinimum();
                    }
                }
            }
        }


        public override long ShuterMax
        {
            get
            {
                    if (camera.Parameters.Contains(PLCamera.ExposureTimeAbs))
                    {
                        return (long)camera.Parameters[PLCamera.ExposureTimeAbs].GetMaximum();
                    }
                    else
                    {
                        if (camera.GetSfncVersion() < sfnc2_0_0)
                        {
                            return camera.Parameters[PLCamera.ExposureTimeRaw].GetMinimum();
                        }
                        else
                        {
                            return (long)camera.Parameters[PLCamera.ExposureTime].GetMinimum();
                        }
                    }
            }
        }

        public override double GainCur
        {
            get
            {
                if (camera.Parameters.Contains(PLCamera.GainAbs))
                {
                    return camera.Parameters[PLCamera.GainAbs].GetValue();
                }
                else
                {
                    if (camera.GetSfncVersion() < sfnc2_0_0)
                    {
                        return camera.Parameters[PLCamera.GainRaw].GetValue();
                    }
                    else
                    {
                        return camera.Parameters[PLCamera.Gain].GetValue();
                    }
                }
            }
            set
            {
                    if (camera.Parameters.Contains(PLCamera.GainAbs))
                    {
                        camera.Parameters[PLCamera.GainAbs].SetValue(value);
                    }
                    else
                    {
                        if (camera.GetSfncVersion() < sfnc2_0_0)
                        {
                            camera.Parameters[PLCamera.GainRaw].SetValue((long)value);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.Gain].SetValue(value);
                        }
                    }
            }
        }

        public override double GainMin
        {
            get
            {
                if (camera.Parameters.Contains(PLCamera.GainAbs))
                {
                    return camera.Parameters[PLCamera.GainAbs].GetMinimum();
                }
                else
                {
                    if (camera.GetSfncVersion() < sfnc2_0_0)
                    {
                        return camera.Parameters[PLCamera.GainRaw].GetMinimum();
                    }
                    else
                    {
                        return camera.Parameters[PLCamera.Gain].GetMinimum();
                    }
                }
            }
        }

        public override double GainMax
        {
            get
            {

                if (camera.Parameters.Contains(PLCamera.GainAbs))
                {
                    return camera.Parameters[PLCamera.GainAbs].GetMaximum();
                }
                else
                {
                    if (camera.GetSfncVersion() < sfnc2_0_0)
                    {
                        return camera.Parameters[PLCamera.GainRaw].GetMaximum();
                    }
                    else
                    {
                        return camera.Parameters[PLCamera.Gain].GetMaximum();
                    }
                }
            }
        }

        public override bool IsConnected { get ; set ; }
        public override CameraConmand command { get ; set ; }

        public override void Close()
        {
            try
            {
                if (camera != null)
                {
                    camera.Close();
                    camera.Dispose();
                    camera = null;
                    IsConnected = false;
                }
            }
            catch
            {

            }
             
        }

        public override void ContinuousShot()
        {
            try
            {
                // Set an enum parameter.
                if (camera.GetSfncVersion() < sfnc2_0_0)
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
                        }
                    }
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            //如果是连续采集，则先停止
            string str = camera.Parameters[PLCamera.AcquisitionMode].GetValue();
            if (camera.StreamGrabber.IsGrabbing && camera.Parameters[PLCamera.AcquisitionMode].GetValue() == PLCamera.AcquisitionMode.Continuous)
            {
                Stop();
            }
            camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
            camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
            IsSoftTrigger = false;
            IsContinuousShot = true;
            IsExtTrigger = false;
        }
        bool test;
        public override void ContinuousShotStop()
        {
            SetSoftTrigger();
        }

        public override HImage GrabImage(int delayMs)
        {
           
            if (!IsSoftTrigger)
            {
                //设置采集模式
                SetSoftTrigger();
            }
            //触发
            try
            {
                this.CallBackEvent.Reset();
                camera.ExecuteSoftwareTrigger();
                if (this.CallBackEvent.WaitOne(delayMs))
                {
                    return hPylonImage.CopyImage();
                }
                return null;
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public override HImage GrabImage_ex(int delayMs, int IO)
        {
            throw new NotImplementedException();
        }

         

        public void ImageCallBack(object sender, ImageGrabbedEventArgs e)
        {
            try
            {
                //IGrabResult grabResult = e.GrabResult;
                //if (grabResult.IsValid)
                //{
                //    Bitmap bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format32bppRgb);
                //    BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                //    IntPtr ptrBmp = bmpData.Scan0;
                //    converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult);
                //    bitmap.UnlockBits(bmpData);
                //    int width = grabResult.Width;
                //    int height = grabResult.Height;
                //    hPylonImage.Dispose();
                //    hPylonImage = new HImage();
                //    hPylonImage.GenImage1("byte", width, height, ptrBmp);
                //    this.CallBackEvent.Set();
                //    System.Diagnostics.Debug.WriteLine("setok");
                //    if (IsContinuousShot)
                //    {
                //        base.ImageEvent_vido?.Invoke(hPylonImage, this.CameraID);
                //    }
                //    if(IsExtTrigger)
                //    {
                //        base.ImageEvent_ex?.Invoke(hPylonImage, this.CameraID);
                //    }
                //}

                IGrabResult grabResult = e.GrabResult;
                if (grabResult.IsValid)
                {
                        // 判断是否是黑白图片格式
                        if (grabResult.PixelTypeValue == PixelType.Mono8)
                        {
                           // if (latestFrameAddress == IntPtr.Zero)
                           // {
                                latestFrameAddress = Marshal.AllocHGlobal((Int32)grabResult.PayloadSize);
                            //}
                            converter.OutputPixelFormat = PixelType.Mono8;
                            converter.Convert(latestFrameAddress, grabResult.PayloadSize, grabResult);
                        // 转换为Halcon图像显示
                        hPylonImage.Dispose();
                        hPylonImage = new HImage();
                        hPylonImage.GenImage1( "byte", (HTuple)grabResult.Width, (HTuple)grabResult.Height, (HTuple)latestFrameAddress);
                        Marshal.FreeHGlobal(latestFrameAddress);
                        this.CallBackEvent.Set();
                            if (IsContinuousShot)
                            {
                                base.ImageEvent_vido?.Invoke(hPylonImage, this.CameraID);
                            }
                            if (IsExtTrigger)
                            {
                                base.ImageEvent_ex?.Invoke(hPylonImage, this.CameraID);
                            }
                    }
                    else if (grabResult.PixelTypeValue == PixelType.BayerBG8 || grabResult.PixelTypeValue == PixelType.BayerGB8
                                    || grabResult.PixelTypeValue == PixelType.BayerRG8 || grabResult.PixelTypeValue == PixelType.BayerGR8 || grabResult.PixelTypeValue == PixelType.YUV422packed)
                        {
                            int imageWidth = grabResult.Width - 1;
                            int imageHeight = grabResult.Height - 1;
                            int payloadSize = imageWidth * imageHeight;
                            //if (latestFrameAddress == IntPtr.Zero)
                            //{
                                latestFrameAddress = Marshal.AllocHGlobal((Int32)(3 * payloadSize));
                            //}
                            //正常显示
                            converter.OutputPixelFormat = PixelType.BGR8packed;
                            converter.Parameters[PLPixelDataConverter.InconvertibleEdgeHandling].SetValue("Clip");
                            converter.Convert(latestFrameAddress, 3 * payloadSize, grabResult);
                            hPylonImage.Dispose();
                            hPylonImage = new HImage();
                            hPylonImage.GenImageInterleaved( latestFrameAddress, "bgr",
                                         (HTuple)imageWidth, (HTuple)imageHeight, -1, "byte", (HTuple)imageWidth, (HTuple)imageHeight, 0, 0, -1, 0);
                        Marshal.FreeHGlobal(latestFrameAddress);
                        this.CallBackEvent.Set();
                            if (IsContinuousShot)
                            {
                                base.ImageEvent_vido?.Invoke(hPylonImage, this.CameraID);
                            }
                            if (IsExtTrigger)
                            {
                                base.ImageEvent_ex?.Invoke(hPylonImage, this.CameraID);
                            }
                    }
                }

            }
            catch (Exception exception)
            {
            }
        }

        public void OnCameraClosed(object sender, EventArgs e)
        {
            IsConnected = false;
        }

        public void OnCameraOpened(object sender, EventArgs e)
        {
            IsConnected = true;
        }

        public void OnConnectionLost(object sender, EventArgs e)
        {
            IsConnected = false;
        }

        public override void SetExtTrigger()
        { 
            //如果是连续采集，则先停止
            string str = camera.Parameters[PLCamera.AcquisitionMode].GetValue();
            if (camera.StreamGrabber.IsGrabbing && camera.Parameters[PLCamera.AcquisitionMode].GetValue() == PLCamera.AcquisitionMode.Continuous)
            {
                Stop();
            }
            try
            {
                if (camera.GetSfncVersion() < sfnc2_0_0)
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Line1);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Line1);
                        }
                    }

                    //Sets the trigger delay time in microseconds.
                    camera.Parameters[PLCamera.TriggerDelayAbs].SetValue(5);        // 设置触发延时

                    //Sets the absolute value of the selected line debouncer time in microseconds
                    camera.Parameters[PLCamera.LineSelector].TrySetValue(PLCamera.LineSelector.Line1);
                    camera.Parameters[PLCamera.LineMode].TrySetValue(PLCamera.LineMode.Input);
                    camera.Parameters[PLCamera.LineDebouncerTimeAbs].SetValue(5);       // 设置去抖延时，过滤触发信号干扰

                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Line1);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Line1);
                        }
                    }

                    //Sets the trigger delay time in microseconds.//float
                    camera.Parameters[PLCamera.TriggerDelay].SetValue(5);       // 设置触发延时

                    //Sets the absolute value of the selected line debouncer time in microseconds
                    camera.Parameters[PLCamera.LineSelector].TrySetValue(PLCamera.LineSelector.Line1);
                    camera.Parameters[PLCamera.LineMode].TrySetValue(PLCamera.LineMode.Input);
                    camera.Parameters[PLCamera.LineDebouncerTime].SetValue(5);       // 设置去抖延时，过滤触发信号干扰

                }
            }
            catch (Exception e)
            {
            }
            IsSoftTrigger = false;
            IsContinuousShot = false;
            IsExtTrigger = true;
        }

        public override bool SetGPIO(int index, bool low, int DelayTime)
        {
            throw new NotImplementedException();
        }
        private void Stop()
        {
            try
            {
                camera.StreamGrabber.Stop();
            }
            catch (Exception exception)
            {

            }
        }
        public override void SetSoftTrigger()
        {
            //如果是连续采集，则先停止
            string str = camera.Parameters[PLCamera.AcquisitionMode].GetValue();
            if (camera.StreamGrabber.IsGrabbing && camera.Parameters[PLCamera.AcquisitionMode].GetValue() 
                == PLCamera.AcquisitionMode.Continuous)
            {
                Stop();
            }
            
            try
            {
                // Set an enum parameter.
                if (camera.GetSfncVersion() < sfnc2_0_0)
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.AcquisitionStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                        }
                    }
                }
                else // For SFNC 2.0 cameras, e.g. USB3 Vision cameras
                {
                    if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart))
                    {
                        if (camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart))
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.Off);

                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                        }
                        else
                        {
                            camera.Parameters[PLCamera.TriggerSelector].TrySetValue(PLCamera.TriggerSelector.FrameBurstStart);
                            camera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                            camera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            camera.Parameters[PLCamera.AcquisitionMode].SetValue(PLCamera.AcquisitionMode.Continuous);
            camera.StreamGrabber.Start(GrabStrategy.LatestImages, GrabLoop.ProvidedByStreamGrabber);
            IsSoftTrigger = true;
            IsContinuousShot = false;
            IsExtTrigger = false;
        }

        public override void UserSetSave()
        {
            throw new NotImplementedException();
        }

        protected override void GetCameraSettingData()
        {
            throw new NotImplementedException();
        }

        public override bool Connect()
        {
            try
            {
                Close();
                camera = new Camera(CameraSerialNumber);
                camera.CameraOpened -= Configuration.AcquireSingleFrame;
                camera.ConnectionLost -= OnConnectionLost;
                camera.CameraOpened -= OnCameraOpened;
                camera.CameraClosed -= OnCameraClosed;
                camera.StreamGrabber.ImageGrabbed -= ImageCallBack;
                camera.CameraOpened += Configuration.AcquireSingleFrame;
                camera.ConnectionLost += OnConnectionLost;
                camera.CameraOpened += OnCameraOpened;
                camera.CameraClosed += OnCameraClosed;
                camera.StreamGrabber.ImageGrabbed += ImageCallBack;

                //camera.StreamGrabber.GrabStarted += OnGrabStarted;
                // camera.StreamGrabber.GrabStopped += OnGrabStopped;
                camera.Open();
                if(camera.IsConnected)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                IsConnected = false;
                return false;
            }
        }
    }
}
