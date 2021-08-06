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

namespace Poc2Auto.RotationPLC
{
    public class RotationPLCPlugin : PluginBase
    {
        public RotationPLCPlugin() : base(ModuleTypes.Tester.ToString(), true)
        {
            Instance = this;
            RunModeMgr.RunModeChanged += RunModeChanged;
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

            //Socket一致性测试模式下点击继续测试后触发
            EventCenter.SocketTestContinue += SocketContinueTest;

            GetMessageHandler(MessageNames.CMD_RotateDone).DataReceived += RotateDone;
            GetMessageHandler(MessageNames.CMD_SocketOpened).DataReceived += SocketOpened;
            EventCenter.CloseSocket += CloseSocket;
            EventCenter.PickDutDone += PickDutDone;
            GetMessageHandler(MessageNames.CMD_SocketClosed).DataReceived += SocketClosed;
            GetMessageHandler(CommonCommands.ErrorList).DataReceived += ErrorList;

            base.Load();
            ModuleStates[ModuleTypes.Tester.ToString()].OnErrorListChanged += ShowErrorList;
            EventCenter.ClearError += ReqClearError;
            StationManager.EventRotateDone += StationRotateDone;
            return true;
        }

        public override bool Reset(string moduleId)
        {
            StationManager.Stations[StationName.Default].Status = StationStatus.Waiting;
            foreach (var name in StationManager.TestStations)
            {
                var station = StationManager.Stations[name];
                station.Status = StationStatus.Idle;
            }
            ModuleStates[ModuleTypes.Tester.ToString()].CleaeError();
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 手动复位Tester", ErrorLevel.Debug);
            var stationDefault = StationManager.Stations[StationName.Default];
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
            EventCenter.isReset?.Invoke();
            return base.Reset(moduleId);
        }

        public override bool Stop(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 停止Tester", ErrorLevel.Warning);
            return base.Stop(moduleId);
        }

        public override bool Abort(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 异常中止Tester", ErrorLevel.Fatal);
            return base.Abort(moduleId);
        }

        public override bool Clear(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 清错Tester", ErrorLevel.Fatal);
            return base.Clear(moduleId);
        }

        public override bool Resume(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 恢复Tester", ErrorLevel.Debug);
            return base.Resume(moduleId);
        }

        public override bool Pause(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 暂停Tester", ErrorLevel.Warning);
            return base.Pause(moduleId);
        }

        public override bool Retry(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 重试Tester", ErrorLevel.Warning);
            return base.Retry(moduleId);
        }

        public override bool Start(string moduleId)
        {
            _isFirst = true;
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} Tester启动", ErrorLevel.Debug);
            return base.Start(moduleId);
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
                    result = ShowMsgBox($"{msg}", "Error", icon: AlcMsgBoxIcon.Error, buttons: AlcMsgBoxButtons.AbortRetry, defaultButton: AlcMsgBoxDefaultButton.Button2); break;
                case AlcErrorLevel.WARN:
                    result = ShowMsgBox($"{msg}", "Error", icon: AlcMsgBoxIcon.Error, buttons: AlcMsgBoxButtons.AbortContinue, defaultButton: AlcMsgBoxDefaultButton.Button2); break;
                case AlcErrorLevel.ERROR1:
                    result = ShowMsgBox($"{msg}", "Error", icon: AlcMsgBoxIcon.Error, buttons: AlcMsgBoxButtons.AbortRetryContinue, defaultButton: AlcMsgBoxDefaultButton.Button2); break;
                default:
                    break;
            }
            int rsp;
            if (result == AlcMsgBoxResult.Abort) rsp = 3;
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
            var handler = GetMessageHandler(MessageNames.CMD_Rotate);
            handler.SendMessage(timeout: -2);
            EventCenter.ProcessInfo?.Invoke($"Tester转盘旋转", ErrorLevel.Debug);
            _rotateDone.WaitOne();
        }

        private bool _isFirst;
        private void RotateDone(MessageHandler handler, ReceivedData arg2)
        {
            handler.Reply();

            if (_isFirst)
            {
                _isFirst = false;
                StationRotateDone();
                return;
            }

            _rotateDone.Set();
            EventCenter.ProcessInfo?.Invoke($"Tester旋转完成", ErrorLevel.Debug);
        }

        private void SocketOpened(MessageHandler handler, ReceivedData arg2)
        {
            handler.Reply();
            EventCenter.ProcessInfo?.Invoke($"Socket盖子已打开", ErrorLevel.Debug);
            Station stationDefault = StationManager.Stations[StationName.Default];
            EventCenter.SocketOpened.ParallelInvoke();
        }

        private ManualResetEvent _socketClosed = new ManualResetEvent(false);
        private void CloseSocket()
        {
            Station stationDefault = StationManager.Stations[StationName.Default];
            stationDefault.Status = StationStatus.WaitSocketClose;
            _socketClosed.Reset();
            EventCenter.ProcessInfo?.Invoke($"Tester关闭Socket盖子", ErrorLevel.Debug);
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
            EventCenter.ProcessInfo?.Invoke($"Socket盖子已关闭", ErrorLevel.Debug);
            _socketClosed.Set();
        }

        //调试用
        private void TesterRotaion()
        {
            var plcDriver = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Tester.ToString()) as AdsDriverClient;
            plcDriver?.WriteBool(RunModeMgr.Name_DryRunTesterRotation, true);
        }

        //调试用
        private void ChangeMode(bool isChange)
        {
            var plcDriver = PlcDriver as AdsDriverClient;
            plcDriver?.WriteBool(RunModeMgr.Name_DryRunChoose, isChange);
        }

        private void TestOpenSocket()
        {
            var plcDriver = PlcDriver as AdsDriverClient;
            plcDriver?.WriteBool(RunModeMgr.Name_DebugOpenSocket, true);
        }

        private void TestCloseSocket()
        {
            var plcDriver = PlcDriver as AdsDriverClient;
            plcDriver?.WriteBool(RunModeMgr.Name_DebugCloseSocket, true);
        }

        private void SocketContinueTest()
        {
            var plcDriver = PlcDriver as AdsDriverClient;
            plcDriver?.WriteBool(RunModeMgr.Name_TMContinueTest, true);
        }
        #endregion 运行流程函数

        #region Events

        private void RunModeChanged()
        {

            if (RunModeMgr.RunMode == RunMode.AutoSelectBin || RunModeMgr.RunMode == RunMode.AutoSelectSn)
            {
                StateEnable = false;
            }
            else
            {
                StateEnable = true;
            }
        }

        private void ShowErrorList(List<string> errors)
        {
            if (errors.Count == 0)
                return;
            _isError = true;
            var errMsg = string.Join("\r\n", errors.ToArray());
            var result = AlcSystem.Instance.ShowMsgBox(errMsg, "TesterError", AlcMsgBoxButtons.OK, 
                AlcMsgBoxIcon.Error, AlcMsgBoxDefaultButton.Button1);
            while (result != AlcMsgBoxResult.OK);
        }

        private void ReqClearError()
        {
            if (_isError)
                ModuleStates[ModuleTypes.Tester.ToString()].CleaeError();
        }

        private void ErrorList(MessageHandler handler, ReceivedData data)
        {
            var evens = (PLCEventItem[])data.Data.Param;
            List<string> msgs = default;
            if (evens != null)
            {
                msgs = evens.Select(e => $"Tester {e.EventId }-->{e.MultiLanMsg.CHS}").ToList();
            }
            EventCenter.ShowErrorMsgs?.Invoke(msgs ?? new List<string>());
        }

        private void StationRotateDone()
        {
            var handler = GetMessageHandler(MessageNames.CMD_SocketDisable);
            if (!StationManager.Stations[StationName.Default].SocketGroup.Enable)
            {
                EventCenter.ProcessInfo?.Invoke($"{(StationName)StationManager.Stations[StationName.Default].SocketGroup.Index} Socket已屏蔽", ErrorLevel.Warning);
                //通知PLC当前Socket已屏蔽，PLC不做Socket顶升动作，等待ALC发送旋转指令
                handler.CmdParam.KeyValues[PLCParamNames.Results].Value = (ushort)1;
                handler.SendAdsMessage(timeout: 5000);

                EventCenter.SocketOpened?.Invoke();
            }
            else
            {
                //通知PLC当前Socket没有屏蔽，PLC做Socket顶升动作
                handler.CmdParam.KeyValues[PLCParamNames.Results].Value = (ushort)2;
                handler.SendAdsMessage(timeout: 5000);
                StationManager.Stations[StationName.Default].Status = StationStatus.WaitSocketOpen;
            }
        }

        #endregion Events
    }
}
