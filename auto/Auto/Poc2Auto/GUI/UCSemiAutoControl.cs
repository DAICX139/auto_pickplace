using AlcUtility;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using Poc2Auto.Database;
using Poc2Auto.GUI.FormMode;
using Poc2Auto.Model;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poc2Auto.GUI
{
    public partial class UCSemiAutoControl : UserControl
    {
        private static AdsDriverClient HandlerClient = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Handler.ToString()) as AdsDriverClient;
        private static AdsDriverClient TesterClient = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Tester.ToString()) as AdsDriverClient;

        public UCSemiAutoControl()
        {
            InitializeComponent();
            fMSelectSN = new FMSelectSN(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };
            AlcSystem.Instance.Reset += Reset;
            RunModeMgr.RunModeChanged += RunModeMgr_RunModeChanged;
        }

        DateTime lastClickTime = DateTime.Now;
        private FMSelectSN fMSelectSN;

        private void RunModeMgr_RunModeChanged()
        {
            if (RunModeMgr.RunMode == RunMode.DryRun)
            {
                RunModeMgr.OriginValue = true;
            }
            else
            {
                RunModeMgr.OriginValue = false;
            }
        }

        private void btnNozzle1Load_Click(object sender, EventArgs e)
        {
            var a = new TimeSpan(0, 0, 0, 5);
            if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 5))
                return;
            lastClickTime = DateTime.Now;

            var mbxResult = AlcSystem.Instance.ShowMsgBox("确定执行吸嘴1 Tray盘取料动作？","手动操作", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2);
            if (AlcMsgBoxResult.No == mbxResult)
                return;
            AlcSystem.Instance.Log("点击吸嘴1Tray盘取料按钮", "界面操作", AlcErrorLevel.DEBUG);
            EventCenter.ProcessInfo?.Invoke("点击吸嘴1Tray盘取料按钮", ErrorLevel.DEBUG);
            Task.Run(  () =>
           {
               //0.Stop状态下去给PLC写数据
                if (UCMain.Instance.Stop(CtrlType.Handler))
                {
                   //1.写功能
                   HandlerClient?.WriteObject(RunModeMgr.Name_Nozzle1TrayLoad, true);
                   //2.切模式
                   init(HandlerClient);
                   Thread.Sleep(60);
                   //3.读取PLC模式，确保模式切换成功
                   if (ReadSemiAitoMode(HandlerClient))
                   {
                       //修改当前运行模式
                       RunModeMgr.RunMode = RunMode.HandlerSemiAuto;
                       //启动
                       HandlerClient?.WriteObject(RunModeMgr.Name_StateCmd, (uint)SYSTEM_EVENT.Reset);
                       //AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Reset);
                   }
                   else//模式切换失败
                   {
                       AlcSystem.Instance.Error("功能切换执行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
                   }
               }
                else
                    AlcSystem.Instance.Error("运行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
           });
        }

        private void btnNozzle1Scan_Click(object sender, EventArgs e)
        {
            if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 5))
                return;
            lastClickTime = DateTime.Now;

            var mbxResult = AlcSystem.Instance.ShowMsgBox("确定执行吸嘴1扫码动作？", "手动操作", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2);
            if (AlcMsgBoxResult.No == mbxResult)
                return;
            AlcSystem.Instance.Log("点击吸嘴1扫码按钮", "界面操作", AlcErrorLevel.DEBUG);
            EventCenter.ProcessInfo?.Invoke("点击吸嘴1扫码按钮", ErrorLevel.DEBUG);
            Task.Run( () =>
           {
               //0.Stop状态下去给PLC写数据
               if (UCMain.Instance.Stop(CtrlType.Handler))
               {
                   //1.写功能
                   HandlerClient?.WriteObject(RunModeMgr.Name_Nozzle1Scan, true);
                   //2.切模式
                   init(HandlerClient);
                   Thread.Sleep(60);
                   //3.读取PLC模式，确保模式切换成功
                   if (ReadSemiAitoMode(HandlerClient))
                   {
                       //修改当前运行模式
                       RunModeMgr.RunMode = RunMode.HandlerSemiAuto;
                       //启动
                       HandlerClient?.WriteObject(RunModeMgr.Name_StateCmd, (uint)SYSTEM_EVENT.Reset);
                       //AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Reset);
                   }
                   else//模式切换失败
                   {
                       AlcSystem.Instance.Error("功能切换执行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
                   }
               }
               else
                   AlcSystem.Instance.Error("运行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
           });
        }

        private void btnNozzle1SocketPutDut_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 5))
                    return;
                lastClickTime = DateTime.Now;

                var mbxResult = AlcSystem.Instance.ShowMsgBox("确定执行吸嘴1 Socket放料动作？", "手动操作", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2);
                if (AlcMsgBoxResult.No == mbxResult)
                    return;
                AlcSystem.Instance.Log("点击吸嘴1 Socket放料按钮", "界面操作", AlcErrorLevel.DEBUG);
                EventCenter.ProcessInfo?.Invoke("点击吸嘴1 Socket放料按钮", ErrorLevel.DEBUG);
                Task.Run( 
               () =>
               {
                   // 0.Stop状态下去给PLC写数据
                   if (UCMain.Instance.Stop(CtrlType.Handler))
                   {
                       //1.写功能
                       HandlerClient?.WriteObject(RunModeMgr.Name_Nozzle1SocketPut, true);
                       //2.切模式
                       init(HandlerClient);
                       Thread.Sleep(60);
                       //3.读取PLC模式，确保模式切换成功
                       if (ReadSemiAitoMode(HandlerClient))
                       {
                           //修改当前运行模式
                           RunModeMgr.RunMode = RunMode.HandlerSemiAuto;
                           //启动
                           //AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Reset);
                           HandlerClient?.WriteObject(RunModeMgr.Name_StateCmd, (uint)SYSTEM_EVENT.Reset);
                       }
                       else//模式切换失败
                       {
                           AlcSystem.Instance.Error("功能切换执行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
                       }
                   }
                   else
                       AlcSystem.Instance.Error("运行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
               });
            }
            catch (Exception ex)
            {
 
            }
        }

        private void Nozzle2SocketPickDut_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 5))
                    return;
                lastClickTime = DateTime.Now;

                var mbxResult = AlcSystem.Instance.ShowMsgBox("确定执行吸嘴2 Socket取料动作？", "手动操作", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2);
                if (AlcMsgBoxResult.No == mbxResult)
                    return;
                AlcSystem.Instance.Log("点击吸嘴2 Socket取料按钮", "界面操作", AlcErrorLevel.DEBUG);
                EventCenter.ProcessInfo?.Invoke("点击吸嘴2 Socket取料按钮", ErrorLevel.DEBUG);
                Task.Run( (() =>
               {
                   // 0.Stop状态下去给PLC写数据
                   if (UCMain.Instance.Stop(CtrlType.Handler))
                   {
                       //1.写功能
                       HandlerClient?.WriteObject(RunModeMgr.Name_Nozzle2SocketPick, true);
                       //2.切模式
                       init(HandlerClient);
                       Thread.Sleep(60);
                       //3.读取PLC模式，确保模式切换成功
                       if (ReadSemiAitoMode(HandlerClient))
                       {
                           //修改当前运行模式
                           RunModeMgr.RunMode = RunMode.HandlerSemiAuto;
                           //启动
                           //AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Reset);
                           HandlerClient?.WriteObject(RunModeMgr.Name_StateCmd, (uint)SYSTEM_EVENT.Reset);
                       }
                       else//模式切换失败
                       {
                           AlcSystem.Instance.Error("功能切换执行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
                       }
                   }
                   else
                       AlcSystem.Instance.Error("运行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
               }));
            }
            catch (Exception ex)
            {
 
            }
        }

        private void btnNozzle2TrayUload_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 5))
                    return;
                lastClickTime = DateTime.Now;

                var mbxResult = AlcSystem.Instance.ShowMsgBox("确定执行吸嘴2 Tray盘放料动作？", "手动操作", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2);
                if (AlcMsgBoxResult.No == mbxResult)
                    return;
                AlcSystem.Instance.Log("点击吸嘴2 Tray盘放料按钮", "界面操作", AlcErrorLevel.DEBUG);
                EventCenter.ProcessInfo?.Invoke("点击吸嘴2 Tray盘放料按钮", ErrorLevel.DEBUG);
                Task.Run( () =>
               {
                   // 0.Stop状态下去给PLC写数据
                   if (UCMain.Instance.Stop(CtrlType.Handler))
                   {
                       //1.写功能
                       HandlerClient?.WriteObject(RunModeMgr.Name_Nozzle2TrayUload, true);
                       //2.切模式
                       init(HandlerClient);
                       Thread.Sleep(60);
                       //3.读取PLC模式，确保模式切换成功
                       if (ReadSemiAitoMode(HandlerClient))
                       {
                           //修改当前运行模式
                           RunModeMgr.RunMode = RunMode.HandlerSemiAuto;
                           //启动
                           //AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Reset);
                           HandlerClient?.WriteObject(RunModeMgr.Name_StateCmd, (uint)SYSTEM_EVENT.Reset);
                       }
                       else//模式切换失败
                       {
                           AlcSystem.Instance.Error("功能切换执行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
                       }
                   }
                   else
                       AlcSystem.Instance.Error("运行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
               });
            }
            catch (Exception ex)
            {
 
            }
        }

        private void btnNozzle1TrayUload_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 5))
                    return;
                lastClickTime = DateTime.Now;

                var mbxResult = AlcSystem.Instance.ShowMsgBox("确定执行吸嘴1 Tray盘放料动作？", "手动操作", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2);
                if (AlcMsgBoxResult.No == mbxResult)
                    return;
                AlcSystem.Instance.Log("点击吸嘴1 Tray盘放料按钮", "界面操作", AlcErrorLevel.DEBUG);
                EventCenter.ProcessInfo?.Invoke("点击吸嘴1 Tray盘放料按钮", ErrorLevel.DEBUG);
                Task.Run( () =>
               {
                   // 0.Stop状态下去给PLC写数据
                   if (UCMain.Instance.Stop(CtrlType.Handler))
                   {
                       //1.写功能
                       HandlerClient?.WriteObject(RunModeMgr.Name_Nozzle1TrayUload, true);
                       //2.切模式
                       init(HandlerClient);
                       Thread.Sleep(60);
                       //3.读取PLC模式，确保模式切换成功
                       if (ReadSemiAitoMode(HandlerClient))
                       {
                           //修改当前运行模式
                           RunModeMgr.RunMode = RunMode.HandlerSemiAuto;
                           //启动
                           //AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Reset);
                           HandlerClient?.WriteObject(RunModeMgr.Name_StateCmd, (uint)SYSTEM_EVENT.Reset);
                       }
                       else//模式切换失败
                       {
                           AlcSystem.Instance.Error("功能切换执行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
                       }
                   }
                   else
                       AlcSystem.Instance.Error("运行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
               });
            }
            catch (Exception ex)
            {

 
            }
        }

        private void btnTesterReset_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 5))
                    return;
                lastClickTime = DateTime.Now;

                var mbxResult = AlcSystem.Instance.ShowMsgBox("确定执行转盘复位动作？", "手动操作", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2);
                if (AlcMsgBoxResult.No == mbxResult)
                    return;
                AlcSystem.Instance.Log("点击转盘复位按钮", "界面操作", AlcErrorLevel.DEBUG);
                EventCenter.ProcessInfo?.Invoke("点击转盘复位按钮", ErrorLevel.DEBUG);

                if ((SYSTEM_STATUS)RunModeMgr.TesterCurrentState != SYSTEM_STATUS.Idle)
                {
                    AlcSystem.Instance.Error("运行失败！请Stop后再操作！", 0, AlcErrorLevel.WARN, "提醒");
                    return;
                }

                //1.切模式
                if (RunModeMgr.Reset(TesterClient, out string msg))
                {
                    //修改当前运行模式
                    RunModeMgr.RunMode = RunMode.TesterSemiAuto;
                    RunModeMgr.Running = false;
                    //启动
                    TesterClient?.WriteObject(RunModeMgr.Name_StateCmd, (uint)SYSTEM_EVENT.Reset);
                    Thread.Sleep(60);
                    //while (!RunModeMgr.TesterAllAxisHomed) ;
                    //复位后数据初始化处理
                    ResetDataHandle();
                    EventCenter.RotateReset?.Invoke();
                }
                else
                    AlcSystem.Instance.ShowMsgBox($"Fail, {msg}", "Error", icon: AlcMsgBoxIcon.Error);
            }
            catch (Exception ex)
            {

             }
        }

        /// <summary>
        /// 整机复位的先后顺序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset()
        {
            if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 5))
                return;
            lastClickTime = DateTime.Now;

            var mbxResult = AlcSystem.Instance.ShowMsgBox("确定执行整机复位动作？", "复位", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2);
            if (AlcMsgBoxResult.No == mbxResult)
                return;
            AlcSystem.Instance.Log("点击整机复位按钮", "界面操作", AlcErrorLevel.DEBUG);
            EventCenter.ProcessInfo?.Invoke("点击整机复位按钮", ErrorLevel.DEBUG);

            if ((SYSTEM_STATUS)RunModeMgr.HandlerCurrentState != SYSTEM_STATUS.Idle || (SYSTEM_STATUS)RunModeMgr.TesterCurrentState != SYSTEM_STATUS.Idle)
            {
                AlcSystem.Instance.Error("运行失败！请Stop后再操作！", 0, AlcErrorLevel.WARN, "提醒");
                return;
            }


            //加条件   如果在测试中但是在Idle下就不行，不能复位!!!

            //1.切功能码
            if (RunModeMgr.Reset(TesterClient, out string msg) && RunModeMgr.Reset(HandlerClient, out string msg1))
            {
                RunModeMgr.LastMode = RunModeMgr.RunMode;
                //修改当前运行模式
                RunModeMgr.RunMode = RunMode.ResetMode;
                RunModeMgr.Running = false;
                //先Handler复位
                HandlerClient?.WriteObject(RunModeMgr.Name_StateCmd, (uint)SYSTEM_EVENT.Reset);
                Thread.Sleep(60);
                //如果Handler所有轴都没有回原点，就一直在等待回原
                while (!RunModeMgr.HandlerAllAxisHomed) ;
                //Handler回原后Tester复位
                TesterClient?.WriteObject(RunModeMgr.Name_StateCmd, (uint)SYSTEM_EVENT.Reset);
                Thread.Sleep(60);
                //如果Tester所有轴都没有回原点，就一直在等待回原
                while (!RunModeMgr.TesterAllAxisHomed) ;
                //TM复位
                EventCenter.TMReset?.Invoke();
                //复位后数据初始化处理
                ResetDataHandle();
                Thread.Sleep(60);
                // 切回复位前的功能
                SwitchMode(RunModeMgr.LastMode);
                EventCenter.RotateReset?.Invoke();
            }
            else
                AlcSystem.Instance.ShowMsgBox($"Fail, {msg}", "Error", icon: AlcMsgBoxIcon.Error);
        }

        private bool ReadSemiAitoMode(AdsDriverClient client)
        {
            if (!client.IsInitOk)
                return false;
            PlcMode mainMode = PlcMode.Invalid;
            uint subMode = 0;
            //if (client.Name == ModuleTypes.Handler.ToString())
            //{
            //    mainMode = (PlcMode)RunModeMgr.HandlerMode;
            //    subMode = RunModeMgr.HandlerSubMode;
            //}
            //else
            //{
            //    mainMode = (PlcMode)RunModeMgr.TesterMode;
            //    subMode = RunModeMgr.TesterSubMode;
            //}

            var  Result = client?.ReadObject(RunModeMgr.Name_nMode, typeof(uint));
            if (null != Result)
            {
                mainMode = (PlcMode)(uint)Result;
            }
            Result = client?.ReadObject(RunModeMgr.Name_nAutoSubMode, typeof(uint));
            if (null != Result)
            {
                subMode = (uint)Result;
            }

            return mainMode == PlcMode.AutoMode && subMode == RunModeMgr.Func_SemiAuto;
        }

        private void btnSocketOpenCap_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 5))
                    return;
                lastClickTime = DateTime.Now;

                var mbxResult = AlcSystem.Instance.ShowMsgBox("确定执行Socket开盖动作？", "手动操作", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2);
                if (AlcMsgBoxResult.No == mbxResult)
                    return;
                AlcSystem.Instance.Log("点击Socket开盖按钮", "界面操作", AlcErrorLevel.DEBUG);
                EventCenter.ProcessInfo?.Invoke("点击Socket开盖按钮", ErrorLevel.DEBUG);
                Task.Run( () =>
               {
                   // 0.Stop状态下去给PLC写数据
                   if (UCMain.Instance.Stop(CtrlType.Tester))
                   {
                       //1.写功能
                       TesterClient?.WriteObject(RunModeMgr.Name_OpenSocket, true);
                       //2.切模式
                       init(TesterClient);
                       Thread.Sleep(60);
                       //3.读取PLC模式，确保模式切换成功
                       if (ReadSemiAitoMode(TesterClient))
                       {
                           //修改当前运行模式
                           RunModeMgr.RunMode = RunMode.TesterSemiAuto;
                           //启动
                           //AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Reset);
                           TesterClient?.WriteObject(RunModeMgr.Name_StateCmd, (uint)SYSTEM_EVENT.Reset);
                       }
                       else//模式切换失败
                       {
                           AlcSystem.Instance.Error("功能切换执行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
                       }
                   }
                   else
                       AlcSystem.Instance.Error("运行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());

               });
            }
            catch (Exception ex)
            {
 
            }
        }

        private void btnSocketClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 5))
                    return;
                lastClickTime = DateTime.Now;

                var mbxResult = AlcSystem.Instance.ShowMsgBox("确定执行Socket关盖动作？", "手动操作", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2);
                if (AlcMsgBoxResult.No == mbxResult)
                    return;
                AlcSystem.Instance.Log("点击Socket关盖按钮", "界面操作", AlcErrorLevel.DEBUG);
                EventCenter.ProcessInfo?.Invoke("点击Socket关盖按钮", ErrorLevel.DEBUG);
                Task.Run( () =>
               {
                   // 0.Stop状态下去给PLC写数据
                   if (UCMain.Instance.Stop(CtrlType.Tester))
                   {
                       //1.写功能
                       TesterClient?.WriteObject(RunModeMgr.Name_CloseSocket, true);
                       //2.切模式
                       init(TesterClient);
                       Thread.Sleep(60);
                       //3.读取PLC模式，确保模式切换成功
                       if (ReadSemiAitoMode(TesterClient))
                       {
                           //修改当前运行模式
                           RunModeMgr.RunMode = RunMode.TesterSemiAuto;
                           //启动
                           //AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Reset);
                           TesterClient?.WriteObject(RunModeMgr.Name_StateCmd, (uint)SYSTEM_EVENT.Reset);
                       }
                       else//模式切换失败
                       {
                           AlcSystem.Instance.Error("功能切换执行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
                       }
                   }
                   else
                       AlcSystem.Instance.Error("运行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());

               });
            }
            catch (Exception ex)
            {
                 
            }
        }

        private void btnRotateOneStation_Click(object sender, EventArgs e)
        {
            if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 5))
                return;
            lastClickTime = DateTime.Now;

            var mbxResult = AlcSystem.Instance.ShowMsgBox("确定执行转盘旋转动作？", "手动操作", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2);
            if (AlcMsgBoxResult.No == mbxResult)
                return;
            AlcSystem.Instance.Log("点击转盘旋转按钮", "界面操作", AlcErrorLevel.DEBUG);
            EventCenter.ProcessInfo?.Invoke("点击转盘旋转按钮", ErrorLevel.DEBUG);
            Task.Run( () =>
           {
               // 0.Stop状态下去给PLC写数据
               if (UCMain.Instance.Stop(CtrlType.Tester))
               {
                   //1.写功能
                   TesterClient?.WriteObject(RunModeMgr.Name_TesterRotation, true);
                   //2.切模式
                   init(TesterClient);
                   Thread.Sleep(60);
                   //3.读取PLC模式，确保模式切换成功
                   if (ReadSemiAitoMode(TesterClient))
                   {
                       //修改当前运行模式
                       RunModeMgr.RunMode = RunMode.TesterSemiAuto;
                       //启动
                       //AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Reset);
                       TesterClient?.WriteObject(RunModeMgr.Name_StateCmd, (uint)SYSTEM_EVENT.Reset);
                   }
                   else//模式切换失败
                   {
                       AlcSystem.Instance.Error("功能切换执行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
                   }
               }
               else
                   AlcSystem.Instance.Error("运行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
           });
        }

        private void btnTesterCylinderMoveToWork_Click(object sender, EventArgs e)
        {
            if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 5))
                return;
            lastClickTime = DateTime.Now;

            var mbxResult = AlcSystem.Instance.ShowMsgBox("确定执行转盘气缸顶升动作？", "手动操作", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2);
            if (AlcMsgBoxResult.No == mbxResult)
                return;
            AlcSystem.Instance.Log("点击转盘气缸顶升按钮", "界面操作", AlcErrorLevel.DEBUG);
            EventCenter.ProcessInfo?.Invoke("点击转盘气缸顶升按钮", ErrorLevel.DEBUG);

            Task.Run(() =>
            {
                // 0.Stop状态下去给PLC写数据
                if (UCMain.Instance.Stop(CtrlType.Tester))
                {
                    //1.写功能
                    TesterClient?.WriteObject(RunModeMgr.Name_TurntableCyliderWork, true);
                }
                else
                    AlcSystem.Instance.Error("运行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
            });
        }

        private void btnTesterCylinderMoveToBase_Click(object sender, EventArgs e)
        {
            if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 5))
                return;
            lastClickTime = DateTime.Now;

            var mbxResult = AlcSystem.Instance.ShowMsgBox("确定执行转盘气缸收回动作？", "手动操作", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2);
            if (AlcMsgBoxResult.No == mbxResult)
                return;
            AlcSystem.Instance.Log("点击转盘气缸收回按钮", "界面操作", AlcErrorLevel.DEBUG);
            EventCenter.ProcessInfo?.Invoke("点击转盘气缸收回按钮", ErrorLevel.DEBUG);

            Task.Run(() =>
           {
               // 0.Stop状态下去给PLC写数据
               if (UCMain.Instance.Stop(CtrlType.Tester))
               {
                   //1.写功能
                   TesterClient?.WriteObject(RunModeMgr.Name_TurntableCyliderBase, true);
               }
               else
                   AlcSystem.Instance.Error("运行失败！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
           });
        }

        private void init(AdsDriverClient client)
        {
            if (!RunModeMgr.SemiAutoMode(client, out string message))
            {
                AlcSystem.Instance.ShowMsgBox($"Fail, {message}", "Error", icon: AlcMsgBoxIcon.Error);
                return;
            }
            else
            {
                RunModeMgr.Running = false;
            }
        }

        private void btnSelectDut_Click(object sender, EventArgs e)
        {
            if (fMSelectSN == null || fMSelectSN.IsDisposed)
            {
                fMSelectSN = new FMSelectSN(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };
            }
            AlcSystem.Instance.Log("点击挑料按钮", "界面操作", AlcErrorLevel.DEBUG);
            EventCenter.ProcessInfo?.Invoke("点击挑料按钮", ErrorLevel.DEBUG);
            fMSelectSN.ShowDialog();
        }

        public void EnableButton(bool enable)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { EnableButton(enable); }));
                return;
            }

            btnNozzle1Load.Enabled = enable;
            btnNozzle1Scan.Enabled = enable;
            btnNozzle1SocketPutDut.Enabled = enable;
            btnNozzle1TrayUload.Enabled = enable;
            btnNozzle2TrayUload.Enabled = enable;
            btnRotateOneStation.Enabled = enable;
            btnSelectDut.Enabled = enable;
            btnSocketClose.Enabled = enable;
            btnSocketOpenCap.Enabled = enable;
            btnTesterReset.Enabled = enable;
            Nozzle2SocketPickDut.Enabled = enable;
            btnTmReset.Enabled = enable;
            btnTesterCylinderMoveToBase.Enabled = enable;
            btnTesterCylinderMoveToWork.Enabled = enable;
            //btnLightControl.Enabled = enable;
            //btnMaintenanceLamp.Enabled = enable;

            //有关机台状态更新
            UpdateState();
        }
 
        private void btnTmReset_Click(object sender, EventArgs e)
        {
            if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 3))
                return;
            lastClickTime = DateTime.Now;
            AlcSystem.Instance.Log("点击TM复位按钮", "界面操作", AlcErrorLevel.DEBUG);
            EventCenter.ProcessInfo?.Invoke("点击TM复位按钮", ErrorLevel.DEBUG);
            EventCenter.TMReset?.Invoke();
        }

       private void ResetDataHandle()
        {
            //数据、状态初始化处理
            StationManager.Stations[StationName.PNP].Status = StationStatus.Waiting;
            foreach (var name in StationManager.TestStations)
            {
                var station = StationManager.Stations[name];
                station.Status = StationStatus.Idle;
            }

            var stationDefault = StationManager.Stations[StationName.PNP];
            if ((int)stationDefault.Name != stationDefault.SocketGroup.Index)
            {
                int distance = Math.Abs((int)stationDefault.Name - stationDefault.SocketGroup.Index);
                for (int j = 0; j < distance; j++)
                {
                    SocketGroup temp = StationManager.Stations[StationManager.RotationStations.Last()].SocketGroup;
                    for (int i = StationManager.RotationStations.Count - 1; i > 0; i--)
                    {
                        StationManager.Stations[StationManager.RotationStations[i]].SocketGroup =
                            StationManager.Stations[StationManager.RotationStations[i - 1]].SocketGroup;
                    }
                    StationManager.Stations[StationManager.RotationStations[0]].SocketGroup = temp;
                    DragonDbHelper.RotateDut();
                }
            }
            //EventCenter.StationReset?.Invoke();

            //保存各个工站的数据至文件
            foreach (StationName name in Enum.GetValues(typeof(StationName)))
            {
                StationManager.Stations[name].Save();
            }
        }

        private void UpdateState()
        {
            if (RunModeMgr.LightControl)
            {
                btnLightControl.Text = "机台照明关";
            }
            else
            {
                btnLightControl.Text = "机台照明开";
            }

            if (RunModeMgr.MaintenanceLamp)
            {
                btnMaintenanceLamp.Text = "检修照明灯关";
            }
            else
            {
                btnMaintenanceLamp.Text = "检修照明灯开";
            }

            if (RunModeMgr.IonFanContrl)
            {
                btnIonFan.Text = "离子风扇关";
            }
            else
            {
                btnIonFan.Text = "离子风扇开";
            }
        }

        private void btnLightControl_Click(object sender, EventArgs e)
        {
            if (btnLightControl.Text == "机台照明开")
            {
                HandlerClient?.WriteObject(RunModeMgr.MachineLight(1), true);
            }
            else if (btnLightControl.Text == "机台照明关")
            {
                HandlerClient?.WriteObject(RunModeMgr.MachineLight(1), false);
            }
        }

        private void btnMaintenanceLamp_Click(object sender, EventArgs e)
        {
            if (btnMaintenanceLamp.Text == "检修照明灯开")
            {
                HandlerClient?.WriteObject(RunModeMgr.MachineLight(2), true);
            }
            else if (btnMaintenanceLamp.Text == "检修照明灯关")
            {
               var a = HandlerClient?.WriteObject(RunModeMgr.MachineLight(2), false);
            }
        }
        int timer;
        private void SwitchMode(RunMode mode)
        {
            switch (mode)
            {
                case RunMode.AutoNormal:
                    var res =WaitIdle();
                    if (res)
                    {
                        Thread.Sleep(3000);
                        if (RunModeMgr.AutoNormal(HandlerClient, out string msg))
                        {
                            RunModeMgr.RunMode = RunMode.AutoNormal;
                            RunModeMgr.Running = false;
                        }
                    }
                    break;
                case RunMode.AutoSelectSn:
                    break;
                case RunMode.AutoSelectBin:
                    break;
                case RunMode.AutoGRR:
                    var res1 = WaitIdle();
                    if (res1)
                    {
                        Thread.Sleep(3000);
                        var param = new GRRParam
                        {
                            TestTimes = RunModeMgr.GRRTestTimes,
                        };
                        if (RunModeMgr.GRR(HandlerClient, param, out string msg1))
                        {
                            RunModeMgr.RunMode = RunMode.AutoGRR;
                            RunModeMgr.Running = false;
                        }
                    }
                    break;
                case RunMode.AutoAudit:
                    var res2 = WaitIdle();
                    if (res2)
                    {
                        Thread.Sleep(3000);
                        if (RunModeMgr.Audit(HandlerClient, out string msg2) && RunModeMgr.Audit(TesterClient, out string msg3))
                        {
                            RunModeMgr.RunMode = RunMode.AutoAudit;
                            RunModeMgr.Running = false;
                        }
                    }
                    break;
                case RunMode.AutoMark:
                    break;
                case RunMode.DryRun:
                    break;
                case RunMode.DoeSlip:
                    break;
                case RunMode.DoeSameTray:
                    break;
                case RunMode.DoeDifferentTray:
                    break;
                case RunMode.DoeTakeOff:
                    break;
                case RunMode.DoeSocketUniformityTest:
                    break;
                case RunMode.DoeSingleDebug:
                    break;
                case RunMode.HandlerSemiAuto:
                    break;
                case RunMode.TesterSemiAuto:
                    break;
                case RunMode.ResetMode:
                    break;
                default:
                    break;
            }
        }

        private bool WaitIdle()
        {
            timer = 0;
            int flag = 0;
            while (RunModeMgr.HandlerAllAxisHomed && RunModeMgr.TesterAllAxisHomed)
            {
                break;
            }
            while (true)
            {
                Thread.Sleep(60);
                if ((SYSTEM_STATUS)RunModeMgr.HandlerCurrentState == SYSTEM_STATUS.Idle && (SYSTEM_STATUS)RunModeMgr.TesterCurrentState == SYSTEM_STATUS.Idle)
                {
                    break;
                }
                if (timer > 150)
                {
                    flag = 1;
                    break;
                }
                timer++;
                Thread.Sleep(200);
            }
            if (1 == flag)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnIonFan_Click(object sender, EventArgs e)
        {
            if (btnIonFan.Text == "离子风扇开")
            {
                HandlerClient?.WriteObject(RunModeMgr.Name_IonFanContrl, true);
            }
            else if (btnIonFan.Text == "离子风扇关")
            {
                HandlerClient?.WriteObject(RunModeMgr.Name_IonFanContrl, false);
            }
        }
    }
}
