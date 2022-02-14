using AlcUtility;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Poc2Auto.GUI;
using System.Threading.Tasks;
using VisionSDK;
using HalconDotNet;

namespace VisionFlows
{
    public class VisionPlugin : PluginBase
    {
        private static VisionPlugin _instance;
        
        public static VisionPlugin GetInstance()
        {
            return _instance;
        }

        //相机实时显示事件

        //public static VisionPlugin Instance { get; private set; }
        public VisionPlugin() : base("Vision", new List<string> { "Handler" }, false)
        {
            _instance = this;

            HOperatorSet.SetSystem("global_mem_cache", "idle");
            HOperatorSet.SetSystem("tsp_temporary_mem_cache", "idle");
            HOperatorSet.SetSystem("temporary_mem_cache", "idle");

            ExpectedModuleIds = null;
            DisableStateMachine = true;
            PlcDriver = PlcDriverClientManager.GetInstance().GetPlcDriver("Handler");//Add and change according to actual demand
            GetMessageHandler(MessageNames.CMD_LeftTopCamera).DataReceived -= LeftTopCamera;//Add and change according to actual demand
            GetMessageHandler(MessageNames.CMD_RightTopCamera).DataReceived -= RightTopCamera;//Add and change according to actual demand
            GetMessageHandler(MessageNames.CMD_BottomCamera).DataReceived -= BottomCamera;

            // 需要实时显示时 触发
            EventCenter.LeftCamera -= Camera1LiveDisplay;
            EventCenter.RightCamera -= Camera2LiveDisplay;
            EventCenter.DownCamera -= Camera3LiveDisplay;
            EventCenter.StateChanged -= StateChanged;
            GetMessageHandler(MessageNames.CMD_LeftTopCamera).DataReceived += LeftTopCamera;//Add and change according to actual demand
            GetMessageHandler(MessageNames.CMD_RightTopCamera).DataReceived += RightTopCamera;//Add and change according to actual demand
            GetMessageHandler(MessageNames.CMD_BottomCamera).DataReceived += BottomCamera;

            // 需要实时显示时 触发
            EventCenter.LeftCamera += Camera1LiveDisplay;
            EventCenter.RightCamera += Camera2LiveDisplay;
            EventCenter.DownCamera += Camera3LiveDisplay;
            EventCenter.StateChanged += StateChanged;


        }
        private void StateChanged()
        {
            if (AlcSystem.Instance.GetSystemStatus() == SYSTEM_STATUS.Starting || AlcSystem.Instance.GetSystemStatus() == SYSTEM_STATUS.Running)
            {
                isCamera1Dsiplay = false;
                isCamera2Dsiplay = false;
                isCamera3Dsiplay = false;

                UCMain.Instance.rBtnLiveCamera1.ForeColor = System.Drawing.Color.Black;
                UCMain.Instance.rBtnLiveCamera1.Text = "Live";
                UCMain.Instance.rBtnLiveCamera1.Checked = false;

                UCMain.Instance.rBtnLiveCamera2.ForeColor = System.Drawing.Color.Black;
                UCMain.Instance.rBtnLiveCamera2.Text = "Live";
                UCMain.Instance.rBtnLiveCamera2.Checked = false;

                UCMain.Instance.rBtnLiveCamera3.ForeColor = System.Drawing.Color.Black;
                UCMain.Instance.rBtnLiveCamera3.Text = "Live";
                UCMain.Instance.rBtnLiveCamera3.Checked = false;
            }
        }

        #region  实时显示操作
        static Task TkCameraLive1;
        static Task TkCameraLive2;
        static Task TkCameraLive3;
        static bool isCamera1Dsiplay = false;
        static bool isCamera2Dsiplay = false;
        static bool isCamera3Dsiplay = false;
        private static void Camera1LiveDisplay(int CameraID, bool isDisplay)
        {
            isCamera1Dsiplay = isDisplay;

            if (isCamera1Dsiplay)
            {
                Plc.SetIO(CameraID, true);  // 开启光源
                UCMain.Instance.rBtnLiveCamera1.ForeColor = System.Drawing.Color.Red;
                UCMain.Instance.rBtnLiveCamera1.Text = "Living";

                TkCameraLive1 = new Task(() =>
                {
                    while (isCamera1Dsiplay)
                    {
                        GC.Collect();
                        var image = CameraManager.CameraList[1].GrabImage(Utility.CaptureDelayTime);
                        UCMain.Instance.ShowObject(0, image);
                        image?.Dispose();
                        if (!isCamera1Dsiplay)
                        {
                            return;
                        }

                    }
                });

                CameraManager.CameraList[0].ShuterCur = (long)(ImagePara.Instance.Exposure_LeftCamGetDUT);
                TkCameraLive1.Start();
            }
            else
            {
                UCMain.Instance.rBtnLiveCamera1.ForeColor = System.Drawing.Color.Black;
                UCMain.Instance.rBtnLiveCamera1.Text = "Live";
                System.Threading.Thread.Sleep(500);
                Plc.SetIO(CameraID, false);
            }
        }

        private static void Camera2LiveDisplay(int CameraID, bool isDisplay)
        {
            isCamera2Dsiplay = isDisplay;
            if (isCamera2Dsiplay)
            {
                Plc.SetIO(CameraID, true);  // 开启光源
                UCMain.Instance.rBtnLiveCamera2.ForeColor = System.Drawing.Color.Red;
                UCMain.Instance.rBtnLiveCamera2.Text = "Living";

                TkCameraLive2 = new Task(() =>
                {
                    while (isCamera2Dsiplay)
                    {
                        GC.Collect();
                        var image = CameraManager.CameraList[0].GrabImage(Utility.CaptureDelayTime);
                        UCMain.Instance.ShowObject(1, image);
                        image?.Dispose();
                        if (!isCamera2Dsiplay) return;
                    }
                });
                CameraManager.CameraList[1].ShuterCur = (long)(ImagePara.Instance.Exposure_LeftCamGetDUT);
                TkCameraLive2.Start();
            }

            else
            {
                UCMain.Instance.rBtnLiveCamera2.ForeColor = System.Drawing.Color.Black;
                UCMain.Instance.rBtnLiveCamera2.Text = "Live";
                System.Threading.Thread.Sleep(500);
                Plc.SetIO(CameraID, false);
            }
        }

        protected override void OnError(MessageHandler handler, ReceivedData data)
        {
            //
        }

        protected override void OnState(MessageHandler handler, ReceivedData data)
        {
            //
        }

        protected override void OnDisconnected(MessageHandler handler, ReceivedData data)
        {
            if (RegisteredModuleIds.Contains(data.ModuleId))
            {
                RegisteredModuleIds.Remove(data.ModuleId);
            }
        }

        protected override void OnRegister(MessageHandler handler, ReceivedData data)
        {
            if (!RegisteredModuleIds.Contains(data.ModuleId))
            {
                RegisteredModuleIds.Add(data.ModuleId);
            }
        }

        protected override void OnUnknownMessage(ReceivedData data)
        {
            //
        }
        private static void Camera3LiveDisplay(int id, bool isDisplay)
        {
            isCamera3Dsiplay = isDisplay;
            if (isCamera3Dsiplay)
            {
                Plc.SetIO(id, true);  // 开启光源
                UCMain.Instance.rBtnLiveCamera3.ForeColor = System.Drawing.Color.Red;
                UCMain.Instance.rBtnLiveCamera3.Text = "Living";
                TkCameraLive3 = new Task(() =>
                {
                    while (isCamera3Dsiplay)
                    {
                        GC.Collect();
                        var image= CameraManager.CameraList[2].GrabImage(Utility.CaptureDelayTime);
                        UCMain.Instance.ShowObject(2, image);
                        image.Dispose();
                        if (!isCamera3Dsiplay) return;
                    }
                });
                CameraManager.CameraList[2].ShuterCur=(long)(ImagePara.Instance.Exposure_LeftCamGetDUT);
                TkCameraLive3.Start();
            }
            else
            {
                UCMain.Instance.rBtnLiveCamera3.ForeColor = System.Drawing.Color.Black;
                UCMain.Instance.rBtnLiveCamera3.Text = "Live";
                System.Threading.Thread.Sleep(500);
                Plc.SetIO(id, false);
            }
        }

        #endregion


        public override bool Dispose()
        {
            foreach (var item in CameraManager.CameraList)
            {
                item.Close();
            }
            return true;
        }
        //public override 
        public override Form GetForm()
        {
            return Flow.FrmVisionUI;
        }

        public override bool Load()
        {
            if (CameraManager.CameraList.Count <3)
            {
                UpdateModuleStatus(false);
            }
            else
            {
                UpdateModuleStatus(true);
            }
            foreach (var item in CameraManager.CameraList)
            {
                item.Camera_loss-= VisionPlugin_CameraLoss;
                item.Camera_loss += VisionPlugin_CameraLoss;
            }

           // Flow.FrmMain.Show();
            //Flow.FrmMain.Hide();
            return base.Load();
        }

 
        private void VisionPlugin_CameraLoss(bool arg1, string arg2)
        {
            if (arg1)
            {
                UpdateModuleStatus(false);
                AlcSystem.Instance.ShowMsgBox("相机掉线", "提示", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                if (CameraManager.CameraList.Count == 3 && CameraManager.CameraList[1].IsConnected && CameraManager.CameraList[0].IsConnected && CameraManager.CameraList[2].IsConnected)
                {
                    UpdateModuleStatus(true);
                }
            }
        }

        public override Dictionary<string, ToolStripItem[]> GetMenuList()
        {
            return null;
        }

        public static void LeftTopCamera(MessageHandler handler, ReceivedData data)
        {
            GC.Collect();
            UCMain.Instance.hWindowControl1.HalconWindow.ClearWindow();
            Flow.Windlist[0].viewController.viewPort.HalconWindow.ClearWindow();
            SetLightON((int)EnumLight.LeftTop);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ImageProcessBase.Execute_New(handler, EnumCamera.LeftTop);
            }
            SetLightOFF((int)EnumLight.LeftTop);
            GC.Collect();
        }

        public static void RightTopCamera(MessageHandler handler, ReceivedData data)
        {
            GC.Collect();
            UCMain.Instance.hWindowControl2.HalconWindow.ClearWindow();
            Flow.Windlist[1].viewController.viewPort.HalconWindow.ClearWindow();
            SetLightON((int)EnumLight.RightTop);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ImageProcessBase.Execute_New(handler, EnumCamera.RightTop);
            }
            SetLightOFF((int)EnumLight.RightTop);
            GC.Collect();
        }
        public static void BottomCamera(MessageHandler handler, ReceivedData data)
        {
            GC.Collect();
            UCMain.Instance.hWindowControl3.HalconWindow.ClearWindow();
            Flow.Windlist[2].viewController.viewPort.HalconWindow.ClearWindow();
            SetLightON((int)EnumLight.Bottom);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ImageProcessBase.Execute_New(handler, EnumCamera.Bottom);
            }
            SetLightOFF((int)EnumLight.Bottom);
            GC.Collect();
        }
        private void SetLight(int io,bool targ)
        {
            Plc.SetIO(io, targ);
            Thread.Sleep(100);
            bool state = Plc.ReadIO(io);
            int index = 0;
            ropen:
            if (!state && index < 30)
            {
                Thread.Sleep(20);
                Plc.SetIO(io, targ);
                Thread.Sleep(80);
                state = Plc.ReadIO(io);
                index++;
                if (!state)
                {
                    goto ropen;
                }
            }
        }

        private static void SetLightON(int io)
        {
            Plc.SetIO(io, true);
            Thread.Sleep(120);
            bool state = Plc.ReadIO(io);
            int index = 0;
            ropen:
            if (!state && index < 30)
            {
                Thread.Sleep(20);
                Plc.SetIO(io, true);
                Thread.Sleep(80);
                state = Plc.ReadIO(io);
                index++;
                if (!state)
                {
                    goto ropen;
                }
            }
        }

        private static void SetLightOFF(int io)
        {
            Plc.SetIO(io, false);
            Thread.Sleep(120);
            bool state = Plc.ReadIO(io);
            int index = 0;
            ropen:
            if (state && index < 10)
            {
                Thread.Sleep(20);
                Plc.SetIO(io, false);
                Thread.Sleep(80);
                state = Plc.ReadIO(io);
                index++;
                if (state)
                {
                    goto ropen;
                }
            }
        }

      
    }
}