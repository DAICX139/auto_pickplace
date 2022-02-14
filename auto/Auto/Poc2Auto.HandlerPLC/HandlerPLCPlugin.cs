using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlcUtility;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using Poc2Auto.GUI;
using Poc2Auto.Model;
using Poc2Auto.Database;
using AlcUtility.PlcDriver.CommonCtrl;
using Poc2Auto.MTCP;
using System;
using LogLib.Forms;

namespace Poc2Auto.HandlerPLC
{
    /// <summary>
    /// Plugin for control handler PLC
    /// </summary>
    class HandlerPLCPlugin : PluginBase
    {
        public static HandlerPLCPlugin Instance { get; private set; }
        private  FormMsgBox formMsgBox;
        private FormMsgBox formInfoMsgBox;
        private static AdsDriverClient TesterClient = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Tester.ToString()) as AdsDriverClient;
        /// <summary>
        /// Constructors
        /// </summary>
        public HandlerPLCPlugin() : base(ModuleTypes.Handler.ToString(), true)
        {
            Instance = this;
            PlcDriver = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleType);
            RunModeChanged();
            RunModeMgr.RunModeChanged += RunModeChanged;
            //CompleteFinish();
        }

        #region Fields

        private bool _isError = default;

        #endregion Fields

        #region override

        public override bool Load()
        {
            Overall.InitModels();

            ExpectedModuleIds = new List<string> { ModuleType };
            if (PlcDriver != null)
            {
                PlcDriver.OnInitOk += PlcDriver_OnInitOk;
                PlcDriver.OnInitOk += () => { TrayManager.ReadTrayCellsCoordination((AdsDriverClient)PlcDriver); };
                PlcDriver.OnDisconnected += PlcDriver_OnDisconnected;
            }

            #region 运行流程

            //1.1 PLC上料端从tray盘取料，完成后通知ALC并附带每个吸嘴的取料结果
            GetMessageHandler(MessageNames.CMD_PickDutFromTray).DataReceived += PickupDutFromTray;

            //1.2 当Socket打开后通知PLC下料，并附带socket中每个位置的dut信息
            //EventCenter.SocketOpened += ReadyToUnload;
            GetMessageHandler(MessageNames.CMD_InformSocketData).DataReceived += RequestLoadOrUnload;

            //2.1 当load吸取的产品有问题时直接往NGtray放
            GetMessageHandler(MessageNames.CMD_LoadPutDutToTray).DataReceived += LoadPutDutToTray;

            //2.2 PLC下料端从socket中取料，完成后通知ALC并附带每个吸嘴的取料结果
            GetMessageHandler(MessageNames.CMD_PickDutFromSocket).DataReceived += PickupDutFromSocket;

            //3 PLC上料端将dut放到socket，完成后通知ALC并附带每个吸嘴的放料结果
            GetMessageHandler(MessageNames.CMD_PutDutToSocket).DataReceived += PutDutToSocket;

            //4 PLC下料端将dut放到tray盘，完成后通知ALC并附带每个吸嘴的放料结果
            GetMessageHandler(MessageNames.CMD_PutDutToTray).DataReceived += PutDutToTray;

            GetMessageHandler(MessageNames.CMD_BinValue).DataReceived += SemiAutoBinValue;

            GetMessageHandler(MessageNames.CMD_GRREnd).DataReceived += GRREnd;
            GetMessageHandler(MessageNames.CMD_TrayDutStatus).DataReceived += TrayDutStatus;
            GetMessageHandler(CommonCommands.ErrorList).DataReceived += ErrorList;

            #endregion 运行流程

            base.Load();
            ModuleStates.Values.First().StateMachine.OnStateChanged += StateMachine_OnStateChanged;
            ModuleStates[ModuleTypes.Handler.ToString()].OnErrorListChanged += ShowErrorList;
            EventCenter.ClearError += ReqClearError;
            //当报警时是否使能蜂鸣器警报
            EventCenter.EnableBuzzer += SetBuzzerStatus;
            EventCenter.BinValueInput += BinValueInput;
            EventCenter.SocketClosed += SocketClosed;
            EventCenter.SocketOpened += SocketOpened;
            StationManager.EventRotateDone += StationRotateDone;
            EventCenter.TesterRunning += StationRotateDone;
            EventCenter.VisionScan += VisionScan;
            RunModeMgr.CompleteFlagChanged += CompleteFlagChanged;
            return true;
        }

        protected override void OnRegister(MessageHandler handler, ReceivedData data)
        {
            base.OnRegister(handler, data);

            StationManager.Stations[StationName.Load].IP = data.Ip;
            StationManager.Stations[StationName.Unload].IP = data.Ip;
        }

        protected override void OnDisconnected(MessageHandler handler, ReceivedData data)
        {
            base.OnDisconnected(handler, data);
            if (AlcSystem.Instance.GetSystemStatus() != SYSTEM_STATUS.Idle)
                Error("Handler PLC disconnected!", ErrorCode.EC_ClientDisonnected, AlcErrorLevel.FATAL);

            StationManager.Stations[StationName.Load].IP = "";
            StationManager.Stations[StationName.Unload].IP = "";
        }

        protected override void OnUnknownMessage(ReceivedData data)
        {
            var cmd = data.Data.Name;
            if (cmd == MessageNames.CMD_BottomCamera ||
                cmd == MessageNames.CMD_LeftTopCamera ||
                cmd == MessageNames.CMD_RightTopCamera) return;
            base.OnUnknownMessage(data);
        }

        public override UserControl GetControl()
        {
            return UCMain.Instance;
        }

        public override UserControl GetConfigView()
        {
            return new UCHandlerConfig_New((AdsDriverClient)PlcDriver) { Dock = DockStyle.Fill };
        }

        public override object GetDockContent()
        {
            return new FormHandler(this);
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
                    result = ShowMsgBox($"{msg}\r\nUnit ID: {unit}", "Error" , icon: AlcMsgBoxIcon.Error, buttons: AlcMsgBoxButtons.StopRetry, defaultButton: AlcMsgBoxDefaultButton.Button2); break;
                case AlcErrorLevel.WARN:
                    result = ShowMsgBox($"{msg}\r\nUnit ID: {unit}", "Error", icon: AlcMsgBoxIcon.Error, buttons: AlcMsgBoxButtons.StopContinue, defaultButton: AlcMsgBoxDefaultButton.Button2); break; 
                case AlcErrorLevel.ERROR1:
                    result = ShowMsgBox($"{msg}\r\nUnit ID: {unit}", "Error", icon: AlcMsgBoxIcon.Error, buttons: AlcMsgBoxButtons.StopRetryContinue, defaultButton: AlcMsgBoxDefaultButton.Button2); break;
                case AlcErrorLevel.ERROR2:
                    result = ShowMsgBox($"{msg}\r\nUnit ID: {unit}", "Error", icon: AlcMsgBoxIcon.Error, buttons: AlcMsgBoxButtons.ClearDutChangeTray, defaultButton: AlcMsgBoxDefaultButton.Button1); break;
                case AlcErrorLevel.FATAL:
                    result = ShowMsgBox($"{msg}", "Question", icon: AlcMsgBoxIcon.Question, buttons: AlcMsgBoxButtons.YesNo, defaultButton: AlcMsgBoxDefaultButton.Button1); break;
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
            else if (result == AlcMsgBoxResult.ChangeTray)
            {
                rsp = 2;
                Log($"弹框提醒，点击换Tray按钮", AlcErrorLevel.DEBUG);
            }
            else if (result == AlcMsgBoxResult.Continue)
            {
                rsp = 2;
                Log($"弹框提醒，点击继续按钮", AlcErrorLevel.DEBUG);
            }
            else if (result == AlcMsgBoxResult.Retry)
            {
                rsp = 1;
                Log($"弹框提醒，点击重试按钮", AlcErrorLevel.DEBUG);
            }
            else if (result == AlcMsgBoxResult.ClearDUT)// 用户选择清料时，给PLC相应回复，此时进入清料流程
            {
                rsp = 1;
                //handler.Reply(new ReceivedData
                //{
                //    Data = new MessageData
                //    {
                //        Param = new ErrorRspParam
                //        {
                //            Unit = unit,
                //            Code = code,
                //            Result = rsp,
                //        }
                //    }
                //});
                //Log($"Handler: ALC-->PLC Error param, unit: {unit}, code: {code}, Result: {rsp}", AlcErrorLevel.DEBUG);
                ////检测各工站DUT是否被取完
                ////CompleteFinish();
                //return;

                Log($"弹框提醒，点击清料按钮", AlcErrorLevel.DEBUG);
            }
            else if (result == AlcMsgBoxResult.Yes)
            {
                rsp = 1;
                Log($"弹框提醒，点击重测按钮", AlcErrorLevel.DEBUG);
            }
            else if (result == AlcMsgBoxResult.No)
            {
                rsp = 2;
                Log($"弹框提醒，点击取消重测按钮", AlcErrorLevel.DEBUG);
            }
            else
            {
                rsp = 0;
            }
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
        }

        public override bool Dispose()
        {
            base.Dispose();
            //保存各个工站的数据至文件
            foreach (StationName name in Enum.GetValues(typeof(StationName)))
            {
                StationManager.Stations[name].Save();
            }
            return true;
        }

        #endregion override

        #region Buttons

        public override bool Reset(string moduleId)
        {
            RunModeMgr.Running = false;
            //清错
            //ModuleStates[ModuleTypes.Handler.ToString()].CleaeError();
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 运行Handler", ErrorLevel.DEBUG);
            return base.Reset(moduleId);
        }

        public override bool Stop(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 停止Handler", ErrorLevel.DEBUG);
            return base.Stop(moduleId);
        }

        public override bool Resume(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 恢运行Handler", ErrorLevel.DEBUG);
            return base.Resume(moduleId);
        }

        public override bool Pause(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 暂停Handler", ErrorLevel.DEBUG);
            return base.Pause(moduleId);
        }

        public override bool Retry(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 重试Handler", ErrorLevel.DEBUG);
            return base.Retry(moduleId);
        }

        #endregion Buttons

        #region Events

        private void StateMachine_OnStateChanged()
        {
            EventCenter.StateChanged?.Invoke();

            //if (RunModeMgr.RunMode == RunMode.AutoNormal || RunModeMgr.RunMode == RunMode.AutoGRR || RunModeMgr.RunMode == RunMode.AutoAudit || RunModeMgr.RunMode == RunMode.DryRun)
            //{

            //}

            if (ModuleStates.Values.First().StateMachine.CurrentState == SYSTEM_STATUS.Idle)
            {
                if (RunModeMgr.SocketStatus == SocketStatus.SocketOpened)
                {
                    RunModeMgr.SocketStatus = SocketStatus.SocketClosed;
                }
            }

            if (RunModeMgr.RunMode == RunMode.AutoGRR && !RunModeMgr.Running)
            {
                var state = ModuleStates.Values.First().StateMachine.CurrentState;
                if (state == SYSTEM_STATUS.Running)
                {
                    RunModeMgr.Running = true;
                    EventCenter.ProcessInfo?.Invoke($"GRR测试开始", ErrorLevel.DEBUG);
#if OldGRR
                    GRRStart();
#endif
                }

            }
            else if (RunModeMgr.RunMode == RunMode.AutoNormal && !RunModeMgr.Running)
            {
                RunModeMgr.Running = true;
                EventCenter.ProcessInfo?.Invoke($"正常生产测试开始", ErrorLevel.DEBUG);
            }
            else if (RunModeMgr.RunMode == RunMode.AutoAudit && !RunModeMgr.Running)
            {
                RunModeMgr.Running = true;
                EventCenter.ProcessInfo?.Invoke($"Audit测试开始", ErrorLevel.DEBUG);
            }
        }

        private void PlcDriver_OnDisconnected()
        {
            EventCenter.OnHandlerPLCDisconnect?.Invoke();
        }

        private void PlcDriver_OnInitOk()
        {
            EventCenter.OnHandlerPLCInitOk?.Invoke((AdsDriverClient)PlcDriver);
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

            formMsgBox = new FormMsgBox($"Handler 出错:\r\n\r\n{errMsg}", "HandlerError", LogLib.MsgBoxButtons.OK, LogLib.MsgBoxIcon.Error, LogLib.MsgBoxDefaultButton.Button1);
            formMsgBox.TopMost = true;
            formMsgBox.show();
        }

        private void ReqClearError()
        {
            if (_isError)
            {
                ModuleStates[ModuleTypes.Handler.ToString()].CleaeError();
                _isError = false;
            }
            else
            {
                PlcDriver?.WriteObject(RunModeMgr.Name_SysCmdClearAlarm, true);

                //PlcDriver?.WriteObject(RunModeMgr.Name_SysCmdClearActiveEvent, true);
                //ModuleStates[ModuleTypes.Handler.ToString()].CleaeError();
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
                msgs = evens.Select(e => $"Handler:{e.MultiLanMsg.CHS}").ToList();
            }
            //EventCenter.ShowErrorMsgs?.Invoke(msgs ?? new List<string>());
            foreach (var msg in msgs)
            {
                EventCenter.ProcessInfo?.Invoke($"{msg}", ErrorLevel.FATAL);
            }
        }

        #endregion Events

        #region 运行流程函数

        /// <summary>
        /// 1.1. PLC上料端从tray盘取料，完成后通知ALC并附带每个吸嘴的取料结果
        /// </summary>
        private void PickupDutFromTray(MessageHandler handler, ReceivedData receivedData)
        {
            var stackerId = (ushort)handler.CmdParam.KeyValues[PLCParamNames.TrayID].Value;
            var pickRows = (ushort[])handler.CmdParam.KeyValues[PLCParamNames.TrayRows].Value;
            var pickCols = (ushort[])handler.CmdParam.KeyValues[PLCParamNames.TrayCols].Value;
            var pickResults = (ushort[])handler.CmdParam.KeyValues[PLCParamNames.Results].Value;
            handler.SendAdsMessage(-2);

            Station stationLoad = StationManager.Stations[StationName.Load];

            var trayName = (TrayName)stackerId;
            if (trayName < TrayName.Load1 || trayName > TrayName.Pass2)
            {
                Error($"接收到的Tray ID {(int)trayName} 无效！", 0, AlcErrorLevel.WARN);
                return;
            }
            var tray = TrayManager.Trays[trayName];
            for (int row = 0; row < SocketGroup.ROW; row++)
            {
                for (int col = 0; col < SocketGroup.COL; col++)
                {
                    int offset = row * SocketGroup.COL + col;
                    int trayRow = pickRows[offset] - 1;
                    int trayCol = pickCols[offset] - 1;
                    if (pickResults[offset] != 1)
                    {
                        tray.ChangeDutFlag(trayRow, trayCol, Dut.AbnormalDut);
                        continue;
                    }
                    if (trayRow >= 0 && trayRow < Tray.ROW && trayCol >= 0 && trayCol < Tray.COL)
                    {
                        var dut = tray.TakeDut(trayRow, trayCol);
                        //dut.Barcode = Overall.ScanResult;
                        stationLoad.PutDut(row, col, dut);
                    }
                    else
                    {
                        Error($"PickupDutFromTray: tray坐标异常, ({trayRow},{trayCol})", 0, AlcErrorLevel.WARN);
                    }
                }
            }
            EventCenter.ProcessInfo?.Invoke($"吸嘴1从{trayName}盘{pickRows[0]}行{pickCols[0]}列吸料", ErrorLevel.DEBUG);

#if HANDLER_ONLY
            if (_firstCmd)
            {
                ReadyToUnload();
                _firstCmd = false;
            }
#endif

#if HANDLER_ONLY2
            ReadyToUnload();
#endif
        }

        /// <summary>
        /// 1.2. 当转盘旋转完成后通知PLC下料，并附带socket中每个位置的dut信息
        /// </summary>
        private void ReadyToUnload()
        {
            Station stationDefault = StationManager.Stations[StationName.PNP];

#if TESTER_ONLY
            stationDefault.PutDut(0, 0, new Dut());
            stationDefault.Status = StationStatus.LoadDone;
            EventCenter.CloseSocket?.Invoke();
            stationDefault.Status = StationStatus.Done;
            return;
#endif

#if HANDLER_ONLY
            if (!stationDefault.SocketGroup.Enable)
            {
                stationDefault.Status = StationStatus.Disabled;
                return;
            }
#endif
            if (RunModeMgr.RunMode != RunMode.DoeTakeOff && RunModeMgr.RunMode != RunMode.DoeSocketUniformityTest)
            {
                if (!stationDefault.SocketGroup.Enable)
                {
                    stationDefault.Status = StationStatus.SocketDisabled;
                    return;
                }
            }

            if (RunModeMgr.RunMode == RunMode.AutoGRR)
            {
#if OldGRR
                if (RunModeMgr.LoadCount == 0)
                {
                    var socketID = stationDefault.SocketGroup.Index;
                    if (RunModeMgr.TestedSockets.Contains(socketID))
                    {
                        EventCenter.CloseSocket?.Invoke();
                        stationDefault.Status = StationStatus.Idle;
                        return;
                    }
                    else
                    {
                        RunModeMgr.TestedSockets.Add(socketID);
                    }
                }
                RunModeMgr.LoadCount++;
#endif
            }

            int len = SocketGroup.ROW * SocketGroup.COL;
            ushort[] testResult = new ushort[len];

            bool empty = true;
            for (int i = 0; i < SocketGroup.ROW; i++)
            {
                for (int j = 0; j < SocketGroup.COL; j++)
                {
                    var index = i * SocketGroup.COL + j;
                    int result;
                    while (true)
                    {
                       result = stationDefault.TestResult(i, j);
                        if (result != Dut.NoResult) break;
                        Thread.Sleep(100);
                    }
                    
                    if (result != 0 && RunModeMgr.CustomMode.HasFlag(CustomMode.AllBinOk))
                        result = Dut.PassBin;
                    if (result == Dut.NoDut)
                        testResult[index] = (ushort)result;
                    else if (result == Dut.NoTestBin)
                        result = Dut.Fail_All;
                    testResult[index] = (ushort) result;
                    if (empty && result != 0) empty = false;
                }
            }

            var handler = GetMessageHandler(MessageNames.CMD_InformSocketData);
            handler.CmdParam.KeyValues[PLCParamNames.Results].Value = testResult;

            if (empty)
            {
                if (RunModeMgr.CompleteFlag && StationManager.Stations[StationName.Load].Empty)
                {
                    EventCenter.CloseSocket?.Invoke();
                    stationDefault.Status = StationStatus.Idle;
                }
                else
                {
                    EventCenter.ProcessInfo?.Invoke($"通知Handler放料到{RunModeMgr.SocketID}号Socket", ErrorLevel.DEBUG);
                    stationDefault.Status = StationStatus.WaitLoad;
                    handler.SendAdsMessage(timeout: 5000);
                }
            }
            else
            {
                EventCenter.ProcessInfo?.Invoke($"通知Handler从{RunModeMgr.SocketID}号Socket中取料", ErrorLevel.DEBUG);
                stationDefault.Status = StationStatus.WaitUnload;
                handler.SendAdsMessage(timeout: 5000);
                
            }
        }

        private void RequestLoadOrUnload(MessageHandler handler, ReceivedData data)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (RunModeMgr.SocketStatus == SocketStatus.SocketOpened)
                        break;
                    else
                        Thread.Sleep(100);
                }

                Station stationDefault = StationManager.Stations[StationName.PNP];

                int len = SocketGroup.ROW * SocketGroup.COL;
                ushort[] testResult = new ushort[len];

                bool empty = true;
                for (int i = 0; i < SocketGroup.ROW; i++)
                {
                    for (int j = 0; j < SocketGroup.COL; j++)
                    {
                        var index = i * SocketGroup.COL + j;
                        int result;
                        var socket = stationDefault.SocketGroup.Sockets[i, j];
                        var enable = stationDefault.SocketGroup.Enable;
                        var dut = socket.Dut;

                        //RunModeMgr.UnloadTotal++;
                        if (dut !=null)
                        {
                            if (!ConfigMgr.Instance.EnableClientMTCP)
                            {
                                dut.GetBin();

                            }
                            else
                            {
                                //发送Unload数据,返回该产品Bin值
                                MTCPSendUnload(socket.Index, socket.Dut.Barcode, enable, out string bin);

                                var getBin = bin;
                                if (string.IsNullOrEmpty(getBin) || getBin == "99")
                                    dut.Result = Dut.MTCPFail;
                                else if (getBin == "F")
                                    dut.Result = Dut.Fail_All;
                                else
                                    dut.Result = int.Parse(getBin);
                            }

                            //更新bin统计
                            DragonDbHelper.SetTotalBin(dut.Result);
                            DragonDbHelper.AddOrUpdateBin(Overall.LotInfo?.LotID, dut.Result);
                            DragonDbHelper.SetBin(socket.Index, dut.Barcode, dut.Result);
                            DragonDbHelper.UpdateDutBin(Overall.LotInfo?.LotID, dut.Barcode, dut.Result);

                            //Socket 连续测试失败计数，达到条件弹框报警，停机
                            if (RunModeMgr.RunMode == RunMode.AutoNormal)
                            {
                                if (Dut.PassBin != dut.Result)
                                {
                                    stationDefault.SocketGroup.TestFailTimes++;
                                }
                                else
                                {
                                    stationDefault.SocketGroup.TestFailTimes = 0;
                                }
                            }
                        }

                        while (true)
                        {
                            result = stationDefault.TestResult(i, j);
                            if (result != Dut.NoResult) break;
                            Thread.Sleep(100);
                        }

                        if (result != 0 && RunModeMgr.CustomMode.HasFlag(CustomMode.AllBinOk))
                            result = Dut.PassBin;
                        else if (result == Dut.NoTestBin)
                            result = Dut.Fail_All;

                        testResult[index] = (ushort)result;
                        if (empty && result != 0) empty = false;
                    }
                }

                handler.CmdParam.KeyValues[PLCParamNames.Results].Value = testResult;

                if (empty)
                {
                    if (RunModeMgr.CompleteFlag && StationManager.Stations[StationName.Load].Empty)
                    {
                        EventCenter.CloseSocket?.Invoke();
                        stationDefault.Status = StationStatus.Idle;
                    }
                    else
                    {
                        stationDefault.Status = StationStatus.WaitLoad;
                        handler.SendAdsMessage(timeout: 5000);
                        EventCenter.ProcessInfo?.Invoke($"通知Handler放料到{RunModeMgr.SocketID}号Socket中", ErrorLevel.DEBUG);
                    }
                }
                else
                {
                    stationDefault.Status = StationStatus.WaitUnload;
                    handler.SendAdsMessage(timeout: 5000);
                    EventCenter.ProcessInfo?.Invoke($"通知Handler从{RunModeMgr.SocketID}号Socket中取料", ErrorLevel.DEBUG);
                }

                if (stationDefault.SocketGroup.TestFailTimes > ConfigMgr.Instance.SocketTestFailTimes)
                {
                    stationDefault.SocketGroup.TestFailTimes = 0;
                    Error($"{data.ModuleId} Error\r\n检测到{RunModeMgr.GetActualSocketID(stationDefault.SocketGroup.Index)}号Socket连续测试失败超过{ConfigMgr.Instance.SocketTestFailTimes}次！请检查该Socket！", 0, AlcErrorLevel.FATAL);
                }

            });
        }
        
        private void MTCPSendUnload(int socketIndex, string barcode, bool enable, out string getBin)
        {
            getBin = Dut.NoResult.ToString();
            //var Socket = RunModeMgr.GetActualSocketID(socketIndex);
            var Socket = "S" + RunModeMgr.SocketID.ToString();
            //给MTCP发送unload数据，返回该DUT测试的总结果
            if (MTCPHelper.SendMTCPLotUnload(barcode, Socket, enable ? 1 : 0, out int eCode, out string eString, out string bin))
            {
                getBin = bin;
                EventCenter.ProcessInfo?.Invoke($"MTCP Unload 发送成功,Dut SN:{barcode},Socket SN:{Socket}", ErrorLevel.INFO);
            }
            else
            {
                getBin = bin;
                if (eCode == -1)
                {
                    AlcSystem.Instance.Error($"ALC与MTCP未连接成功", 0, AlcErrorLevel.WARN, "MTCP");
                    EventCenter.ProcessInfo?.Invoke($"MTCP Unload 发送失败! ALC与MTCP未连接成功,错误码:{eCode},Dut SN:{barcode},Socket SN:{Socket}", ErrorLevel.FATAL);
                }
                else
                {
                    EventCenter.ProcessInfo?.Invoke($"MTCP Unload 发送失败! 错误码:{eCode},Dut SN:{barcode},Socket SN:{Socket}", ErrorLevel.FATAL);
                    AlcSystem.Instance.Error($"MTCP Unload 发送失败!, 错误码:{eCode}", 0, AlcErrorLevel.WARN, "MTCP");
                }
            }
            int time = 0;
            while (true)
            {
                if (getBin != Dut.NoResult.ToString())
                {
                    EventCenter.ProcessInfo?.Invoke($"从MTCP获取到{barcode}的总结果为 {getBin}", ErrorLevel.INFO);
                    AlcSystem.Instance.Log($"从MTCP获取到的总结果为{bin}", "MTCP");
                    break;
                }

                if (time >= ConfigMgr.Instance.MTCPTimeOut)
                {
                    getBin = "99";
                    EventCenter.ProcessInfo?.Invoke($"{barcode}接受MTCP Bin值超时,Bin值设置为{getBin}", ErrorLevel.WARNING);
                    AlcSystem.Instance.Log($"{barcode}接受MTCP Bin值超时,Bin值设置为{getBin}", "MTCP");
                    break;
                }
                Thread.Sleep(200);
                time++;
            }
        }

        private void CheckPutOrPickResult(Socket[,] sockets, ushort[] result)
        {
            for (int i = 0; i < SocketGroup.ROW; i++)
            {
                for (int j = 0; j < SocketGroup.COL; j++)
                {
                    var index = i * SocketGroup.COL + j;
                    if (sockets[i, j].Dut != null && result[index] == 0)
                    {
                        //Unhandled exception: dut miss
                        Overall.Stat.Miss++;
                        sockets[i, j].Dut = null;
                    }

                    if (sockets[i, j].Dut == null && result[index] == 1)
                    {
                        //Unhandled exception: unknown dut
                    }
                }
            }
        }

        /// <summary>
        /// 2.1 当load吸取的产品有问题时直接往NGtray放
        /// </summary>
        private void LoadPutDutToTray(MessageHandler handler, ReceivedData data)
        {
            var trayID = (ushort)handler.CmdParam.KeyValues[PLCParamNames.TrayID].Value;
            var putRows = (ushort[])handler.CmdParam.KeyValues[PLCParamNames.TrayRows].Value;
            var putCols = (ushort[])handler.CmdParam.KeyValues[PLCParamNames.TrayCols].Value;
            var putResults = (ushort[])handler.CmdParam.KeyValues[PLCParamNames.Results].Value;
            handler.Reply();

            Station station = StationManager.Stations[StationName.Load];

            var trayName = (TrayName)trayID;
            if (trayName < TrayName.Load1 || trayName > TrayName.Pass2)
            {
                Error($"接收到的Tray ID {(int)trayName} 无效！", 0, AlcErrorLevel.WARN);
                return;
            }
            var tray = TrayManager.Trays[trayName];
            for (int row = 0; row < SocketGroup.ROW; row++)
            {
                for (int col = 0; col < SocketGroup.COL; col++)
                {
                    int offset = row * SocketGroup.COL + col;
                    int putResult = putResults[offset];
                    int trayRow = putRows[offset] - 1;
                    int trayCol = putCols[offset] - 1;

                    if (trayRow < 0 || trayRow >= Tray.ROW || trayCol < 0 || trayCol >= Tray.COL)
                    {
                        Error($"PutDutToTray: tray坐标异常, ({trayRow},{trayCol})", 0, AlcErrorLevel.WARN);
                    }
                    if (RunModeMgr.RunMode == RunMode.AutoSelectSn)
                    {
                        if (!string.IsNullOrEmpty(Overall.ScanResult) || Overall.ScanResult != "扫码未启用" || Overall.ScanResult != "sample")
                        {
                            RunModeMgr.SaveDutSN(Overall.ScanResult, trayRow + 1, trayCol + 1);
                        }
                    }
                    if (putResult == 0)
                    {
                        //正常没放的，要放到其他地方去
                        continue;
                    }
                    if (putResult == 1)
                    {
                        var dut = station.TakeDutFromLoad(row, col);
                        if (dut == null)
                        {
                            dut = new Dut
                            {
                                Barcode = "noscan",
                            };
                        }
                        tray.PutDut(trayRow, trayCol, dut);
                    }
                    else if (putResult == 3)
                    {
                        var dut = station.TakeDutFromLoad(row, col);
                        if (dut == null)
                        {
                            dut = new Dut
                            {
                                Barcode = "noscan",
                            };
                        }
                        tray.PutDut(trayRow, trayCol, dut);
                        //tray.PutDut(trayRow, trayCol, dut, isLoad);
                    }
                    else if (station.TakeDutFromLoad(row, col) != null)
                    {
                        //Unhandled exception: dut miss
                        Overall.Stat.Miss++;
                    }
                }
            }
            EventCenter.ProcessInfo?.Invoke($"吸嘴1放料到{trayName}盘{putRows[0]}行{putCols[0]}列", ErrorLevel.DEBUG);
        }

        /// <summary>
        /// 2.2 PLC下料端从socket中取料，完成后通知ALC并附带每个吸嘴的取料结果
        /// </summary>
        private void PickupDutFromSocket(MessageHandler handler, ReceivedData receivedData)
        {
            var pickResults = (ushort[])handler.CmdParam.KeyValues[PLCParamNames.Results].Value;
            handler.SendAdsMessage(-2);

            Station stationUnload = StationManager.Stations[StationName.Unload];
            Station stationLoadUnload = StationManager.Stations[StationName.PNP];

            int[] result = new int[SocketGroup.ROW * SocketGroup.COL];
            string[] dutSN = new string[SocketGroup.ROW * SocketGroup.COL];
            for (int row = 0; row < SocketGroup.ROW; row++)
            {
                for (int col = 0; col < SocketGroup.COL; col++)
                {
                    int offset = row * SocketGroup.COL + col;
                    var bin = stationLoadUnload.TestResult(row, col);
                    var dut = stationLoadUnload.SocketGroup.Sockets[row, col].Dut;
                    result[offset] = bin;
                    dutSN[offset] = dut?.Barcode;  
                    //MTCPSendUnload(stationLoadUnload.SocketGroup.Sockets[row,col].Index, dutSN[0], true, out string bin1);

                }
            }

            EventCenter.UnloadDutDone?.Invoke(dutSN, RunModeMgr.SocketID.ToString(), result);
            stationUnload.TakeDutFrom(stationLoadUnload);
            var sockets = stationUnload.SocketGroup.Sockets;
            CheckPutOrPickResult(sockets, pickResults);
            RunModeMgr.UnloadCount++;

            if (RunModeMgr.RunMode == RunMode.AutoAudit &&  1 == pickResults[0])
            {
                if (Dut.PassBin != result[0])
                {
                    stationLoadUnload.Status = StationStatus.WaitLoad;
                }
                //else
                //{
                //    EventCenter.CloseSocket?.Invoke();
                //    stationLoadUnload.Status = StationStatus.Idle;
                //}
            }
            else
            {
                if (RunModeMgr.CompleteFlag && StationManager.Stations[StationName.Load].Empty)
                {
                    EventCenter.CloseSocket?.Invoke();
                    stationLoadUnload.Status = StationStatus.Idle;
                }
                else
                    stationLoadUnload.Status = StationStatus.WaitLoad;
            }

            //if (RunModeMgr.RunMode == RunMode.DoeTakeOff || RunModeMgr.RunMode == RunMode.DoeSocketUniformityTest)
            //{
            //    EventCenter.PickDutDone?.Invoke();
            //}
            EventCenter.ProcessInfo?.Invoke($"吸嘴2从{RunModeMgr.SocketID}号Socket中取料", ErrorLevel.DEBUG);
        }

        /// <summary>
        /// 3.1. PLC上料端将dut放到socket，完成后通知ALC并附带每个吸嘴的放料结果
        /// </summary>
        private void PutDutToSocket(MessageHandler handler, ReceivedData receivedData)
        {
            var putResults = (ushort[]) handler.CmdParam.KeyValues[PLCParamNames.Results].Value;
            handler.SendAdsMessage(-2);

            //记录产品输入数量
            RunModeMgr.LoadCount++;
            Station stationLoad = StationManager.Stations[StationName.Load];
            Station stationDefault = StationManager.Stations[StationName.PNP];

            //在Audit模式下如果TM测试失败并且在界面弹框上点击了取消重测按钮
            if (RunModeMgr.RunMode == RunMode.AutoAudit && putResults[0] == 0)
            {
                EventCenter.CloseSocket?.Invoke();
                stationDefault.Status = StationStatus.Idle;
                return;
            }

            //异常情况：如果在清料过程中最后一颗料（Load吸嘴）被人为拿走了或是掉了，让流程继续
            if (RunModeMgr.RunMode == RunMode.AutoNormal && putResults[0] == 0)
            {
                for (int row = 0; row < SocketGroup.ROW; row++)
                {
                    for (int col = 0; col < SocketGroup.COL; col++)
                    {
                        stationLoad.RemoveDut(row, col);
                    }
                }
                EventCenter.CloseSocket?.Invoke();
                stationDefault.Status = StationStatus.Idle;
                return;
            }

            int[] result = new int[SocketGroup.ROW * SocketGroup.COL];
            string[] dutSN = new string[SocketGroup.ROW * SocketGroup.COL];
            for (int row = 0; row < SocketGroup.ROW; row++)
            {
                for (int col = 0; col < SocketGroup.COL; col++)
                {
                    int offset = row * SocketGroup.COL + col;
                    if (stationLoad.SocketGroup.Sockets[row, col].Dut == null)
                        continue;
                    stationLoad.SocketGroup.Sockets[row, col].Dut.Barcode = Overall.ScanResult;
                    var barcode = stationLoad.SocketGroup.Sockets[row, col].Dut.Barcode;

                    DragonDbHelper.AddBarcode(stationLoad.SocketGroup.Sockets[row, col].Index, barcode + " — " + "S" + RunModeMgr.SocketID);
                    //发送MTCP Load数据
                    if (ConfigMgr.Instance.EnableClientMTCP)
                    {
                        if (MTCPHelper.SendMTCPLotLoad(barcode, "S" + RunModeMgr.SocketID.ToString(), out int eCode, out string eString))
                            EventCenter.ProcessInfo?.Invoke($"MTCP Load 发送成功,DUT SN:{barcode},Socket SN:S{RunModeMgr.SocketID}", ErrorLevel.INFO);
                        else
                        {
                            EventCenter.ProcessInfo?.Invoke($"MTCP Load 发送失败,错误码:{eCode},DUT SN:{barcode},Socket SN:S{RunModeMgr.SocketID}", ErrorLevel.FATAL);
                            Error($"MTCP Load 发送失败, 错误码:{eCode}", 0, AlcErrorLevel.WARN);
                        }

                    }
                    EventCenter.LoadDutDone?.Invoke(barcode, RunModeMgr.SocketID.ToString());
                }
            }
 
            stationLoad.PutDutTo(stationDefault);
            var sockets = stationDefault.SocketGroup.Sockets;
            CheckPutOrPickResult(sockets, putResults);

            if (StationManager.AllStationEmpty) return;
            stationDefault.Status = StationStatus.LoadDone;

            EventCenter.ProcessInfo?.Invoke($"吸嘴1放料到{RunModeMgr.SocketID}号Socket中", ErrorLevel.DEBUG);
            EventCenter.CloseSocket?.Invoke();
            stationDefault.Status = StationStatus.Done;
#if HANDLER_ONLY
            ReadyToUnload();
#endif

#if HANDLER_ONLY2
            ReadyToUnload();
#endif
        }

        /// <summary>
        /// 3.2. PLC下料端将dut放到tray盘，完成后通知ALC并附带每个吸嘴的放料结果
        /// </summary>
        private void PutDutToTray(MessageHandler handler, ReceivedData receivedData)
        {
            var trayID = (ushort)handler.CmdParam.KeyValues[PLCParamNames.TrayID].Value;
            var putRows = (ushort[])handler.CmdParam.KeyValues[PLCParamNames.TrayRows].Value;
            var putCols = (ushort[])handler.CmdParam.KeyValues[PLCParamNames.TrayCols].Value;
            var putResults = (ushort[])handler.CmdParam.KeyValues[PLCParamNames.Results].Value;
            handler.SendAdsMessage(-2);

            Station station = StationManager.Stations[StationName.Unload];

            var trayName = (TrayName)trayID;
            if (trayName < TrayName.Load1 || trayName > TrayName.Pass2)
            {
                Error($"接收到的Tray ID {(int)trayName} 无效！", 0, AlcErrorLevel.WARN);
                return;
            }
            var tray = TrayManager.Trays[trayName];
            for (int row = 0; row < SocketGroup.ROW; row++)
            {
                for (int col = 0; col < SocketGroup.COL; col++)
                {
                    int offset = row * SocketGroup.COL + col;
                    int putResult = putResults[offset];

                    if (putResult == 0)
                    {
                        //正常没放的，要放到其他地方去
                        continue;
                    }

                    if (putResult == 1)
                    {
                        int trayRow = putRows[offset] - 1;
                        int trayCol = putCols[offset] - 1;
                        if (trayRow >= 0 && trayRow < Tray.ROW && trayCol >= 0 && trayCol < Tray.COL)
                        {
                            tray.PutDut(trayRow, trayCol, station.TakeDut(row, col));
                        }
                        else
                        {
                            Error($"PutDutToTray: tray坐标异常, ({trayRow},{trayCol})", 0, AlcErrorLevel.WARN);
                            //Unhandled exception: tray坐标异常
                        }

                    }
                    else if (station.TakeDut(row, col) != null)
                    {
                        //Unhandled exception: dut miss
                    }
                }
            }
            EventCenter.ProcessInfo?.Invoke($"Handler吸嘴2放料到{trayName}盘{putRows[0]}行{putCols[0]}列中", ErrorLevel.DEBUG);
        }

        private void GRRStart()
        {
            RunModeMgr.LoadCount = 0;
            RunModeMgr.UnloadCount = 0;
            EventCenter.ProcessInfo?.Invoke($"GRR模式测试开始", ErrorLevel.DEBUG);
            GetMessageHandler(MessageNames.CMD_GRRStrat).SendAdsMessage(-2);
        }

        private void GRREnd(MessageHandler handler, ReceivedData data)
        {
            handler.SendAdsMessage(-2);
            Station stationDefault = StationManager.Stations[StationName.PNP];
            if (RunModeMgr.RunMode == RunMode.AutoGRR)
            {
#if OldGRR
                var count = StationManager.RotationStations.Count(s => StationManager.Stations[s].SocketGroup.Enable);
                if (RunModeMgr.TestedSockets.Count < count)
                {
                    GRRStart();
                    ReadyToUnload();
                }
                else
                {
                    AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Stop);
                    RunModeMgr.Running = false;
                    EventCenter.ProcessInfo?.Invoke($"GRR模式测试结束，自动停止");
                    ShowMsgBox("GRR done", "GRR");
                }
#endif
                TesterClient?.WriteObject(RunModeMgr.Name_GRRFinish, true);
                Log($"GRR完成，给{ModuleTypes.Tester}写GRRFinish命令true", AlcErrorLevel.DEBUG);
                var flag = (bool)TesterClient?.ReadObject(RunModeMgr.Name_GRRFinish, typeof(bool));
                Log($"读取到{ModuleTypes.Tester}的{RunModeMgr.Name_GRRFinish}值为{flag}", AlcErrorLevel.DEBUG);
                //Thread.Sleep(200);
                EventCenter.CloseSocket?.Invoke();

                if (StationManager.AllStationEmpty)
                {
                    stationDefault.Status = StationStatus.Idle;
                }
                else
                {
                    stationDefault.Status = StationStatus.Waiting;
                }

                RunModeMgr.Running = false;
                EventCenter.ProcessInfo?.Invoke($"GRR模式测试结束，自动停止", ErrorLevel.DEBUG);
                //ShowMsgBox("GRR 测试完成", "GRR");
                formInfoMsgBox = new FormMsgBox("GRR 测试完成", "GRR", LogLib.MsgBoxButtons.OK, LogLib.MsgBoxIcon.Information, LogLib.MsgBoxDefaultButton.Button1);
                formInfoMsgBox.TopMost = true;
                formInfoMsgBox.ShowDialog();
            }
            else if (RunModeMgr.RunMode == RunMode.AutoAudit)
            {
                //给Tester写入Audit完成信号变量
                TesterClient?.WriteObject(RunModeMgr.Name_AuditFinish, true);

                Log($"Audit完成，给{ModuleTypes.Tester}写AuditFinish命令true", AlcErrorLevel.DEBUG);
                 var flag = (bool)TesterClient?.ReadObject(RunModeMgr.Name_AuditFinish, typeof(bool));
                Log($"读取到{ModuleTypes.Tester}的{RunModeMgr.Name_AuditFinish}值为{flag}", AlcErrorLevel.DEBUG);
                //Thread.Sleep(200);
                //EventCenter.CloseSocket?.Invoke();

                if (StationManager.AllStationEmpty)
                {
                    stationDefault.Status = StationStatus.Idle;
                }
                else
                {
                    stationDefault.Status = StationStatus.Waiting;
                }

                RunModeMgr.Running = false;
                EventCenter.ProcessInfo?.Invoke($"Audit模式测试结束，自动停止", ErrorLevel.DEBUG);
                //ShowMsgBox("Audit 测试完成", "Audit");

                formInfoMsgBox = new FormMsgBox("Audit 测试完成", "Audit", LogLib.MsgBoxButtons.OK, LogLib.MsgBoxIcon.Information, LogLib.MsgBoxDefaultButton.Button1);
                formInfoMsgBox.TopMost = true;
                formInfoMsgBox.ShowDialog();
            }
        }

        private void TrayDutStatus(MessageHandler handler, ReceivedData data)
        {
            var trayID = (ushort)handler.CmdParam.KeyValues[PLCParamNames.TrayID].Value;
            var putRows = (ushort[])handler.CmdParam.KeyValues[PLCParamNames.TrayRows].Value;
            var putCols = (ushort[])handler.CmdParam.KeyValues[PLCParamNames.TrayCols].Value;
            var putResults = (ushort[])handler.CmdParam.KeyValues[PLCParamNames.Results].Value;
            handler.SendAdsMessage(-2);

            var trayName = (TrayName)trayID;
            if (trayName < TrayName.Load1 || trayName > TrayName.Pass2)
            {
                Error($"接收到的Tray ID {(int)trayName} 无效！", 0, AlcErrorLevel.WARN);
                return;
            }
            var tray = TrayManager.Trays[trayName];
            for (int row = 0; row < SocketGroup.ROW; row++)
                for (int col = 0; col < SocketGroup.COL; col++)
                {
                    int offset = row * SocketGroup.COL + col;
                    int putResult = putResults[offset];

                    int trayRow = putRows[offset] - 1;
                    int trayCol = putCols[offset] - 1;
                    if (trayRow < 0 || trayRow >= Tray.ROW || trayCol < 0 || trayCol >= Tray.COL)
                    {
                        Error($"PutDutToTray: tray坐标异常, ({trayRow},{trayCol})", 0, AlcErrorLevel.WARN);
                    }

                    if (putResult == 0)     //没有DUT
                    {
                        tray.RemoveTrayDut(trayRow, trayCol);
                    }
                    else if (putResult == 1)    //正常DUT
                    {
                        tray.ChangeDutFlag(trayRow, trayCol, Dut.NoTestBin);
                        if (RunModeMgr.RunMode == RunMode.AutoNormal)
                        {
                            Overall.Stat.Failed++;
                        }
                        //continue;
                    }
                    else if (putResult == 2)    //其他异常DUT,标记红色
                    {
                        tray.ChangeDutFlag(trayRow, trayCol, Dut.AbnormalDut);
                        //Overall.Stat.Failed++;
                    }
                    else if (putResult == 3)    // 未扫到码的DUT
                    {
                        tray.ChangeDutFlag(trayRow, trayCol, Dut.AbnormalDut);
                        //Overall.Stat.Failed++;
                    }
                    else if (putResult == 4 || putResult == 8 || putResult == 10 || putResult == 5)   
                    {
                        tray.ChangeDutFlag(trayRow, trayCol, Dut.ScanDut);
                    }
                    
                }
        }

        MessageHandler BinValuehandler;
        //半自动运行时无Bin值，PLC请求后下发给PLC Bin值
        private void SemiAutoBinValue(MessageHandler handler, ReceivedData data)
        {
            BinValuehandler = handler;
            FMInputBinValue fMInputBinValue = new FMInputBinValue() { StartPosition = FormStartPosition.CenterScreen};
            fMInputBinValue.ShowDialog();
        }

        private void SetBuzzerStatus(bool enable)
        {
            PlcDriver?.WriteObject(RunModeMgr.Name_EnableBuzzer, enable);
        }

        public void BinValueInput(string bin)
        {
            ushort[] value = new ushort[1] { ushort.Parse(bin) };
            BinValuehandler.CmdParam.KeyValues[PLCParamNames.BinValue].Value = value;
            BinValuehandler.SendAdsMessage(-2);
        }

        // 进入清料状态时，实时判断所有工站的DUT是否被取完
        private void CompleteFinish()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    bool empty = true;
                    foreach (var staion in StationManager.Stations)
                    {
                        if (staion.Key != StationName.Unload)
                        {
                            if (!StationManager.Stations[staion.Key].Empty) empty = false;
                        }
                    }
                    if (empty)
                    {
                        TesterClient?.WriteObject(RunModeMgr.Name_CompleteFinish, true);
                        Log($"清料完成，给{ModuleTypes.Tester}写CompleteFinish命令true", AlcErrorLevel.DEBUG);
                        bool flag = (bool)TesterClient?.ReadObject(RunModeMgr.Name_CompleteFinish, typeof(bool));
                        Log($"读取到{ModuleTypes.Tester}的{RunModeMgr.Name_CompleteFinish}值为{flag}", AlcErrorLevel.DEBUG);
                        Thread.Sleep(200);
                        DragonDbHelper.ClearOnlineDut();
                        break;
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }

                    //GetMessageHandler(MessageNames.CMD_CompelteFinish).SendAdsMessage(-2);
                    //PlcDriver?.WriteObject(RunModeMgr.Name_CompleteFinish, true);
                    //Log($"清料完成，给{ModuleTypes.Handler}写CompleteFinish命令true", AlcErrorLevel.DEBUG);
                    //bool flag = (bool)PlcDriver?.ReadObject(RunModeMgr.Name_CompleteFinish, typeof(bool));
                    //Log($"读取到{ModuleTypes.Handler}的{RunModeMgr.Name_CompleteFinish}值为{flag}", AlcErrorLevel.DEBUG);

                }

            });
        }

        private void RunModeChanged()
        {
            if ( RunModeMgr.RunMode == RunMode.TesterSemiAuto)
            {
                StateEnable = false;
            }
            else
            {
                StateEnable = true;
            }
        }

        private void SocketClosed()
        {
            Station stationDefault = StationManager.Stations[StationName.PNP];
            RunModeMgr.SocketStatus = SocketStatus.SocketClosed;
            ConfigMgr.Instance.SocketStatus = SocketStatus.SocketClosed;
            //idle状态下防止触发工站旋转条件
            if (RunModeMgr.RunMode == RunMode.ResetMode && !StationManager.AllStationEmpty)
            {
                stationDefault.Status = StationStatus.SocketClosed;
            }
            else
            {
                if (StationManager.AllStationEmpty)
                {
                    stationDefault.Status = StationStatus.Idle;
                }
                else
                {
                    if (!RunModeMgr.GRRMode && RunModeMgr.RunMode != RunMode.AutoNormal)
                    {
                        stationDefault.Status = StationStatus.Waiting;
                    }
                }
            }
            EventCenter.ProcessInfo?.Invoke($"{RunModeMgr.SocketID}号Socket盖子已关闭", ErrorLevel.DEBUG);
        }

        private void SocketOpened()
        {
            Station stationDefault = StationManager.Stations[StationName.PNP];
            stationDefault.Status = StationStatus.SocketOpened;
            RunModeMgr.SocketStatus = SocketStatus.SocketOpened;
            ConfigMgr.Instance.SocketStatus = SocketStatus.SocketOpened;
            EventCenter.ProcessInfo?.Invoke($"{RunModeMgr.GetActualSocketID(StationManager.Stations[StationName.PNP].SocketGroup.Index)}号Socket盖子已打开", ErrorLevel.DEBUG);
        }

        private void StationRotateDone()
        {
            Station stationDefault = StationManager.Stations[StationName.PNP];
            var CurrSocketID = RunModeMgr.SocketID;
            if (RunModeMgr.RunMode == RunMode.AutoAudit)
            {
                if (!stationDefault.SocketGroup.Enable)
                {
                    EventCenter.ProcessInfo?.Invoke($"{RunModeMgr.GetActualSocketID(StationManager.Stations[StationName.PNP].SocketGroup.Index)}号Socket已屏蔽", ErrorLevel.WARNING);
                    stationDefault.Status = StationStatus.SocketDisabled;
                }
                else
                {
                    //当前Socket SN是自定义文件里面匹配的SN
                    if (double.Parse(CurrSocketID.ToString()) == Overall.AuditSocketID)
                    {
                        if (!stationDefault.Empty && stationDefault.SocketGroup.Sockets[0, 0].Dut.TestResult.Count >= 2)
                        {
                            stationDefault.Status = StationStatus.Done;
                        }
                        else
                        {
                            stationDefault.Status = StationStatus.WaitSocketOpen;
                        }
                    }
                    else //不是匹配的socket
                    {
                        //if (-1 == Overall.AuditSocketID)
                        //{
                        //    stationDefault.Status = StationStatus.Waiting;
                        //}
                        //else
                        //{

                        //}

                        if (!stationDefault.Empty && stationDefault.TestResult(0, 0) != Dut.NoTestBin)
                        {
                            stationDefault.Status = StationStatus.WaitSocketOpen;
                        }
                        else
                        {
                            stationDefault.Status = StationStatus.Done;
                        }
                    }
                }
            }
            else
            {
                if (!stationDefault.SocketGroup.Enable)
                {
                    EventCenter.ProcessInfo?.Invoke($"{RunModeMgr.GetActualSocketID(StationManager.Stations[StationName.PNP].SocketGroup.Index)}号Socket已屏蔽", ErrorLevel.WARNING);
                    stationDefault.Status = StationStatus.SocketDisabled;
                }
                else if (stationDefault.Empty)
                {
                    stationDefault.Status = StationStatus.WaitSocketOpen;
                }
                else if (!stationDefault.Empty)
                {
                    if (stationDefault.SocketGroup.Sockets[0, 0].Dut.TestResult.Count>=2)
                    {
                        stationDefault.Status = StationStatus.WaitSocketOpen;
                    }
                }
                else if (RunModeMgr.SocketStatus == SocketStatus.SocketOpened)
                {

                }
                else
                {
                    stationDefault.Status = StationStatus.Done;
                }
                    
            }
            
        }

        private void VisionScan()
        {
            Station stationDefault = StationManager.Stations[StationName.PNP];
            var CurrSocketID = RunModeMgr.SocketID;
            EventCenter.ProcessInfo?.Invoke($"扫码成功 DutSN:{Overall.ScanResult}", ErrorLevel.INFO);
            //当前运行的是audit模式
            if (RunModeMgr.RunMode == RunMode.AutoAudit)
            {
                if (!stationDefault.SocketGroup.Enable)
                {
                    EventCenter.ProcessInfo?.Invoke($"{RunModeMgr.GetActualSocketID(StationManager.Stations[StationName.PNP].SocketGroup.Index)} Socket已屏蔽", ErrorLevel.WARNING);
                    stationDefault.Status = StationStatus.SocketDisabled;
                }
                else
                {
                    //当前Socket SN是自定义文件里面匹配的SN
                    if (double.Parse(CurrSocketID.ToString()) == Overall.AuditSocketID)
                    {
                        if (RunModeMgr.SocketStatus == SocketStatus.SocketOpened)
                        {
                            //stationDefault.Status = 
                        }
                        else
                        {
                            stationDefault.Status = StationStatus.WaitSocketOpen;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < SocketGroup.ROW; i++)
                        {
                            for (int j = 0; j < SocketGroup.COL; j++)
                            {
                                
                                if (!stationDefault.Empty && stationDefault.TestResult(i, j) != Dut.NoTestBin)
                                {
                                    stationDefault.Status = StationStatus.WaitSocketOpen;
                                }
                                else
                                {
                                    if (RunModeMgr.SocketStatus != SocketStatus.SocketClosed)
                                    {

                                    }
                                    else
                                    {
                                        stationDefault.Status = StationStatus.Done;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CompleteFlagChanged(bool value)
        {
            if (value)
            {
                CompleteFinish();
            }
        }
        #endregion

    }
}
