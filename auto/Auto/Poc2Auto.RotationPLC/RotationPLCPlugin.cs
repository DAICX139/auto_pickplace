using System.Collections.Generic;
using System.Threading;
using AlcUtility;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using Poc2Auto.Model;
using System.Linq;
using Poc2Auto.Database;
using System;
using AlcUtility.PlcDriver.CommonCtrl;
using LogLib.Forms;

namespace Poc2Auto.RotationPLC
{
    public class RotationPLCPlugin : PluginBase
    {
        private FormMsgBox formMsgBox;
        public RotationPLCPlugin() : base(ModuleTypes.Tester.ToString(), true)
        {
            Instance = this;
            RunModeChanged();
            RunModeMgr.RunModeChanged += RunModeChanged;
            EventCenter.RotateReset += RotateReset;
            PlcDriver = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleType);
        }

        #region Fields
        public static RotationPLCPlugin Instance { get; private set; }
        private  bool  _isError = default;
        #endregion Fields

        #region override

        public override bool Load()
        {
            ExpectedModuleIds = new List<string> { ModuleType };

            //转盘旋转
            StationManager.EventRotate += Rotate;

            GetMessageHandler(MessageNames.CMD_RotateDone).DataReceived += RotateDone;
            GetMessageHandler(MessageNames.CMD_SocketOpened).DataReceived += SocketOpened;
            EventCenter.CloseSocket += CloseSocket;
            EventCenter.PickDutDone += PickDutDone;
            GetMessageHandler(MessageNames.CMD_SocketClosed).DataReceived += SocketClosed;
            GetMessageHandler(CommonCommands.ErrorList).DataReceived += ErrorList;

            GetMessageHandler(MessageNames.CMD_Rotate).DataReceived += RequestRotate;

            base.Load();
            ModuleStates[ModuleTypes.Tester.ToString()].OnErrorListChanged += ShowErrorList;
            EventCenter.ClearError += ReqClearError;
            EventCenter.ClearOnlineDut += ClearOnlineDut;
            ModuleStates[ModuleTypes.Tester.ToString()].StateMachine.OnStateChanged += StateMachine_OnStateChanged;
            return true;
        }

        bool isFirst;
        public override bool Reset(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 运行Tester", ErrorLevel.DEBUG);
            EventCenter.TesterRunning?.Invoke();
            if (!isFirst)
            {
                isFirst = true;
                Overall.ScanResult = string.Empty;
            }
            return base.Reset(moduleId);
        }

        public override bool Stop(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 停止Tester", ErrorLevel.WARNING);
            return base.Stop(moduleId);
        }

        public override bool Resume(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 恢复运行Tester", ErrorLevel.DEBUG);
            return base.Resume(moduleId);
        }

        public override bool Pause(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 暂停Tester", ErrorLevel.WARNING);
            return base.Pause(moduleId);
        }

        public override bool Retry(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 重试Tester", ErrorLevel.WARNING);
            return base.Retry(moduleId);
        }

        public override object GetDockContent()
        {
            return new FormTester(this);
        }

        protected override void OnError(MessageHandler handler, ReceivedData data)
        {
            var param = data.Data.Param as ErrorReqParam;
            var level = param.Level;
            var unit = param.Unit;
            var code = param.Code;
            var msg = param.Message;

            AlcMsgBoxResult result = AlcMsgBoxResult.None;
            switch (level)
            {
                case AlcErrorLevel.TRACE:
                    result = ShowMsgBox($"{msg}", "Error", icon: AlcMsgBoxIcon.Error, buttons: AlcMsgBoxButtons.StopRetry, defaultButton: AlcMsgBoxDefaultButton.Button2); break;
                case AlcErrorLevel.WARN:
                    result = ShowMsgBox($"{msg}", "Error", icon: AlcMsgBoxIcon.Error, buttons: AlcMsgBoxButtons.StopContinue, defaultButton: AlcMsgBoxDefaultButton.Button2); break;
                case AlcErrorLevel.ERROR1:
                    result = ShowMsgBox($"{msg}", "Error", icon: AlcMsgBoxIcon.Error, buttons: AlcMsgBoxButtons.StopRetryContinue, defaultButton: AlcMsgBoxDefaultButton.Button2); break;
                default:
                    break;
            }
            int rsp;
            if (result == AlcMsgBoxResult.Stop)
            {
                rsp = 3;
                handler.Reply(new ReceivedData
                {
                    Data = new MessageData
                    {
                        Param = new ErrorRspParam
                        {
                            Unit = unit,
                            Code = code,
                            Result = rsp,
                        }
                    }
                });
                Log($"Handler: ALC-->PLC Error param, unit: {unit}, code: {code}, Result: {rsp}", AlcErrorLevel.DEBUG);
                Log($"弹框提醒，点击停止按钮", AlcErrorLevel.DEBUG);

                ButtonClickRequire(SYSTEM_EVENT.Stop);
                return;

            }  
            else if (result == AlcMsgBoxResult.Continue) rsp = 2;
            else if (result == AlcMsgBoxResult.Retry) rsp = 1;
            else rsp = 0;
            handler.Reply(new ReceivedData
            {
                Data = new MessageData
                {
                    Param = new ErrorRspParam
                    {
                        Unit = unit,
                        Code = code,
                        Result = rsp,
                    }
                }
            });
            Log($"Tester: ALC-->PLC Error param, unit: {unit}, code: {code}, Result: {rsp}", AlcErrorLevel.DEBUG);
        }

        #endregion override

        #region 运行流程函数

        private ManualResetEvent _rotateDone = new ManualResetEvent(false);
        
        public void Rotate()
        {
            _rotateDone.Reset();
            RunModeMgr.IsRotate = true;
            //var handler = GetMessageHandler(MessageNames.CMD_Rotate);
            //handler.SendMessage(timeout: -2);
            EventCenter.ProcessInfo?.Invoke($"Tester转盘旋转", ErrorLevel.DEBUG);
            _rotateDone.WaitOne();
        }

        //private bool _isFirst;
        private void RotateDone(MessageHandler handler, ReceivedData arg2)
        {
            if (RunModeMgr.RunMode != RunMode.TesterSemiAuto)
            {
                RunModeMgr.IsRotate  = false;
                _rotateDone.Set();
            }
            //Thread.Sleep(50);

            //半自动功能下每旋转一次完成后交换数据
            if (RunModeMgr.RunMode == RunMode.TesterSemiAuto)
            {
                //交换各个工站数据
                SocketGroup temp =  StationManager.Stations[StationManager.RotationStations.Last()].SocketGroup;
                for (int i = StationManager.RotationStations.Count - 1; i > 0; i--)
                {
                    StationManager.Stations[StationManager.RotationStations[i]].SocketGroup = StationManager.Stations[StationManager.RotationStations[i - 1]].SocketGroup;
                }
                StationManager.Stations[StationManager.RotationStations[0]].SocketGroup = temp;

                //更新数据库
                DragonDbHelper.RotateDut();
                //保存各个工站的数据至文件
                foreach (StationName name in Enum.GetValues(typeof(StationName)))
                {
                    StationManager.Stations[name].Save();
                }
                EventCenter.ProcessInfo?.Invoke($"数据库更新完成-手动旋转", ErrorLevel.DEBUG);
            } 
            EventCenter.ProcessInfo?.Invoke($"Tester旋转完成", ErrorLevel.DEBUG);

            handler.Reply();
        }

        private void SocketOpened(MessageHandler handler, ReceivedData arg2)
        {
            handler.Reply();
            EventCenter.SocketOpened.ParallelInvoke();
        }

        private ManualResetEvent _socketClosed = new ManualResetEvent(false);
        private void CloseSocket()
        {
            Station stationDefault = StationManager.Stations[StationName.PNP];
            stationDefault.Status = StationStatus.WaitSocketClose;
            RunModeMgr.SocketStatus = SocketStatus.SocketClosing;
            _socketClosed.Reset();
            EventCenter.ProcessInfo?.Invoke($"通知Tester关闭{RunModeMgr.SocketID}号Socket盖子", ErrorLevel.DEBUG);
            var handler = GetMessageHandler(MessageNames.CMD_CloseSocket);
            handler.SendMessage(timeout: -2);
            _socketClosed.WaitOne();
        }

        private void PickDutDone()
        {
            var handler = GetMessageHandler(MessageNames.CMD_PickupDutDone);
            handler.SendMessage(timeout: -2);
        }

        private void SocketClosed(MessageHandler handler, ReceivedData arg2)
        {
            handler.Reply();
            EventCenter.SocketClosed?.Invoke();
            _socketClosed.Set();
        }

        private void RequestRotate(MessageHandler handler, ReceivedData data)
        {
            bool rotate = true;
            while (true)
            {
                //在半自动功能下PLC请求旋转时，需要判断是否可以旋转
                if (RunModeMgr.RunMode == RunMode.TesterSemiAuto)
                {
                    foreach (var name in StationManager.RotationStations)
                    {
                        if (rotate && StationManager.Stations[name].Status == StationStatus.Testing)
                        {
                            rotate = false;
                        }
                    }

                    if (rotate)
                    {
                        break;
                    }
                }
                else //正常流程下
                {
                    if (RunModeMgr.IsRotate)
                    {
                        break;
                    }
                }

                Thread.Sleep(200);
            }
            handler.Reply();

        }

        #endregion 运行流程函数

        #region Events

        private void RunModeChanged()
        {
            if (RunModeMgr.RunMode == RunMode.AutoSelectBin
                || RunModeMgr.RunMode == RunMode.AutoSelectSn
                || RunModeMgr.RunMode == RunMode.AutoMark
                || RunModeMgr.RunMode == RunMode.DoeSameTray
                || RunModeMgr.RunMode == RunMode.DoeDifferentTray
                || RunModeMgr.RunMode == RunMode.HandlerSemiAuto
                || RunModeMgr.RunMode == RunMode.DoeSlip)
            {
                StateEnable = false;
            }
            else
            {
                StateEnable = true;
            }
        }

        private void RotateReset()
        {
           // _rotateDone.Reset();
        }

        private void ShowErrorList(List<string> errors)
        {
            if (errors.Count == 0)
                return;
            _isError = true;
            var errMsg = string.Join("\r\n", errors.ToArray());
            if (null != formMsgBox && formMsgBox.IsHandleCreated)
            {
                if (formMsgBox.InvokeRequired)
                {
                    formMsgBox.Invoke(new Action(() => formMsgBox.Dispose()));
                }
                else
                {
                    formMsgBox.Dispose();
                }
            }

            formMsgBox = new FormMsgBox($"Tester 出错:\r\n\r\n{errMsg}", "TesterError", LogLib.MsgBoxButtons.OK, LogLib.MsgBoxIcon.Error, LogLib.MsgBoxDefaultButton.Button1);
            formMsgBox.TopMost = true;
            formMsgBox.show();
        }

        private void ReqClearError()
        {
            if (_isError)
            {
                ModuleStates[ModuleTypes.Tester.ToString()].CleaeError();
                _isError = false;
            }
            else
            {
                PlcDriver?.WriteObject(RunModeMgr.Name_SysCmdClearAlarm, true);
                //ModuleStates[ModuleTypes.Tester.ToString()].CleaeError();
                //EventCenter.ShowErrorMsgs?.Invoke(new List<string>());
            }
        }

        private void ErrorList(MessageHandler handler, ReceivedData data)
        {
            var evens = (PLCEventItem[])data.Data.Param;
            if (evens == null)
                return;
            List<string> msgs = default;
            if (evens != null)
            {
                msgs = evens.Select(e => $"Tester:{e.MultiLanMsg.CHS}").ToList();
            }
            foreach (var msg in msgs)
            {
                EventCenter.ProcessInfo?.Invoke($"{msg}", ErrorLevel.FATAL);
            }
        }

        private void ClearOnlineDut()
        {
            foreach (StationName name in Enum.GetValues(typeof(StationName)))
            {
                var station = StationManager.Stations[name];
                for (int row = 0; row < SocketGroup.ROW; row++)
                {
                    for (int col = 0; col < SocketGroup.COL; col++)
                    {
                        station.SocketGroup.Sockets[row, col].Dut = null;
                    }
                }
            }

            DragonDbHelper.ClearOnlineDut();
        }

        private void StateMachine_OnStateChanged()
        {
            //if (RunModeMgr.RunMode == RunMode.AutoNormal || RunModeMgr.RunMode == RunMode.AutoGRR || RunModeMgr.RunMode == RunMode.AutoAudit || RunModeMgr.RunMode == RunMode.DryRun)
            //{
                
            //}
            if (ModuleStates[ModuleTypes.Tester.ToString()].StateMachine.CurrentState == SYSTEM_STATUS.Idle)
            {
                if (RunModeMgr.SocketStatus == SocketStatus.SocketOpened)
                {
                    RunModeMgr.SocketStatus = SocketStatus.SocketClosed;
                }
            }

        }
        #endregion Events
    }
}
