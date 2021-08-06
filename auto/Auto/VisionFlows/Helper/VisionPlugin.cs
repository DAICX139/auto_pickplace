using AlcUtility;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using VisionDemo;
using VisionModules;

using Poc2Auto.GUI;
using System.Threading.Tasks;

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
            ExpectedModuleIds = null;
            PlcDriver = PlcDriverClientManager.GetInstance().GetPlcDriver("Handler");//Add and change according to actual demand
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
                        VisionModulesManager.CameraList[0].CaptureImage();
                        VisionModulesManager.CameraList[0].CaptureSignal.WaitOne(Utility.CaptureDelayTime);
                        UCMain.Instance.ShowObject(0, VisionModulesManager.CameraList[0].Image);
                        if (!isCamera1Dsiplay)
                        {
                            return;
                        }
                    }
                });

                VisionModulesManager.CameraList[0].SetExposureTime(ImagePara.Instance.SlotExposeTime);
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
                        VisionModulesManager.CameraList[1].CaptureImage();
                        VisionModulesManager.CameraList[1].CaptureSignal.WaitOne(Utility.CaptureDelayTime);
                        UCMain.Instance.ShowObject(1, VisionModulesManager.CameraList[1].Image);
                        if (!isCamera2Dsiplay) return;
                    }
                });
                VisionModulesManager.CameraList[1].SetExposureTime(ImagePara.Instance.SlotExposeTime);
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
                        VisionModulesManager.CameraList[2].CaptureImage();
                        VisionModulesManager.CameraList[2].CaptureSignal.WaitOne(Utility.CaptureDelayTime);
                        UCMain.Instance.ShowObject(2, VisionModulesManager.CameraList[2].Image);
                        if (!isCamera3Dsiplay) return;
                    }
                });
                VisionModulesManager.CameraList[2].SetExposureTime(ImagePara.Instance.SlotExposeTime);
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
            foreach (var item in VisionModulesManager.CameraList)
            {
                item.DisConnect();
            }
            return true;
        }

        public override Form GetForm()
        {
            return Flow.FrmMain;
        }

        public override bool Load()
        {
            if (VisionModulesManager.CameraList.Count <= 0 || !VisionModulesManager.CameraList[1].IsConnected || !VisionModulesManager.CameraList[0].IsConnected || !VisionModulesManager.CameraList[2].IsConnected)
            {
                UpdateModuleStatus(false);
            }
            else
            {
                UpdateModuleStatus(true);
            }
            foreach (var item in VisionModulesManager.CameraList)
            {
                item.CameraLoss += VisionPlugin_CameraLoss;
            }

            Flow.FrmMain.Show();
            Flow.FrmMain.Hide();
            return base.Load();
        }

        private void VisionPlugin_CameraLoss(object sender, VisionUtility.CameralossArgs e)
        {
            if (e.Message == "loss")
            {
                UpdateModuleStatus(false);
                AlcSystem.Instance.ShowMsgBox("相机掉线", "提示", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                if (VisionModulesManager.CameraList.Count == 3 && VisionModulesManager.CameraList[1].IsConnected && VisionModulesManager.CameraList[0].IsConnected && VisionModulesManager.CameraList[2].IsConnected)
                {
                    UpdateModuleStatus(true);
                }
            }
        }

        public override Dictionary<string, ToolStripItem[]> GetMenuList()
        {
            //seq.SetRightMouseMenuStatus();
            return Flow.FrmMain.GetMenu();
        }

        private void LeftTopCamera(MessageHandler handler, ReceivedData data)
        {
            if (Utility.OriginValue)
            {
                Plc.SetIO((int)EnumLight.LeftTop + 1, true);
                var plcSend = (double[])handler.CmdParam.KeyValues[PLCParamNames.PLCSend].Value;
                var plcRecv = new double[6];
                plcRecv[(int)EnumPLCRecv.XPos] = plcSend[(int)EnumPLCSend.XPos];
                plcRecv[(int)EnumPLCRecv.YPos] = plcSend[(int)EnumPLCSend.YPos];
                plcRecv[(int)EnumPLCRecv.ZPos] = plcSend[(int)EnumPLCSend.ZPos];
                plcRecv[(int)EnumPLCRecv.RPos] = plcSend[(int)EnumPLCSend.RPos];
                plcRecv[(int)EnumPLCRecv.Result] = 1;
                handler.CmdParam.KeyValues[PLCParamNames.PLCRecv].Value = plcRecv;
                handler.SendMessage(new ReceivedData()
                {
                    ModuleId = ModuleTypes.Handler.ToString(),
                    Data = new MessageData() { Param = handler.CmdParam }
                }, -2);
                Plc.SetIO((int)EnumLight.LeftTop + 1, false);
                Flow.Log("视觉屏蔽已开启   plcRecv[(int)EnumPLCRecv.XPos]:" + plcRecv[(int)EnumPLCRecv.XPos] + "    plcRecv[(int)EnumPLCRecv.YPos]" + plcRecv[(int)EnumPLCRecv.YPos]);
            }
            else
            {
                ImageProcessBase.Execute_New(handler, EnumCamera.LeftTop);
            }
        }

        private void RightTopCamera(MessageHandler handler, ReceivedData data)
        {
            if (Utility.OriginValue)
            {
                Plc.SetIO((int)EnumLight.RightTop + 1, true);
                var plcSend = (double[])handler.CmdParam.KeyValues[PLCParamNames.PLCSend].Value;
                var plcRecv = new double[6];
                plcRecv[(int)EnumPLCRecv.XPos] = plcSend[(int)EnumPLCSend.XPos];
                plcRecv[(int)EnumPLCRecv.YPos] = plcSend[(int)EnumPLCSend.YPos];
                plcRecv[(int)EnumPLCRecv.ZPos] = plcSend[(int)EnumPLCSend.ZPos];
                plcRecv[(int)EnumPLCRecv.RPos] = plcSend[(int)EnumPLCSend.RPos];
                plcRecv[(int)EnumPLCRecv.Result] = 1;
                handler.CmdParam.KeyValues[PLCParamNames.PLCRecv].Value = plcRecv;
                handler.SendMessage(new ReceivedData()
                {
                    ModuleId = ModuleTypes.Handler.ToString(),
                    Data = new MessageData() { Param = handler.CmdParam }
                }, -2);
                Plc.SetIO((int)EnumLight.RightTop + 1, false);
                Flow.Log("视觉屏蔽已开启   plcRecv[(int)EnumPLCRecv.XPos]:" + plcRecv[(int)EnumPLCRecv.XPos] + "    plcRecv[(int)EnumPLCRecv.YPos]" + plcRecv[(int)EnumPLCRecv.YPos]);
            }
            else
            {
                ImageProcessBase.Execute_New(handler, EnumCamera.RightTop);
            }
        }

        private void BottomCamera(MessageHandler handler, ReceivedData data)
        {
            if (Utility.OriginValue)
            {
                Plc.SetIO((int)EnumLight.Bottom + 1, true);
                var plcSend = (double[])handler.CmdParam.KeyValues[PLCParamNames.PLCSend].Value;
                var plcRecv = new double[6];
                plcRecv[(int)EnumPLCRecv.XPos] = plcSend[(int)EnumPLCSend.XPos];
                plcRecv[(int)EnumPLCRecv.YPos] = plcSend[(int)EnumPLCSend.YPos];
                plcRecv[(int)EnumPLCRecv.ZPos] = plcSend[(int)EnumPLCSend.ZPos];
                plcRecv[(int)EnumPLCRecv.RPos] = plcSend[(int)EnumPLCSend.RPos];
                plcRecv[(int)EnumPLCRecv.Result] = 1;
                handler.CmdParam.KeyValues[PLCParamNames.PLCRecv].Value = plcRecv;
                handler.SendMessage(new ReceivedData()
                {
                    ModuleId = ModuleTypes.Handler.ToString(),
                    Data = new MessageData() { Param = handler.CmdParam }
                }, -2);

                Plc.SetIO((int)EnumLight.Bottom + 1, false);
                Flow.Log("视觉屏蔽已开启   plcRecv[(int)EnumPLCRecv.XPos]:" + plcRecv[(int)EnumPLCRecv.XPos] + "    plcRecv[(int)EnumPLCRecv.YPos]" + plcRecv[(int)EnumPLCRecv.YPos]);
            }
            else
            {
                ImageProcessBase.Execute_New(handler, EnumCamera.Bottom);
            }
        }

        protected override void OnUnknownMessage(ReceivedData data)
        {
            //do nothing
        }

        protected override void OnError(MessageHandler handler, ReceivedData data)
        {
            //do nothing
        }
    }
}