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

namespace Poc2Auto.HandlerPLC
{
    /// <summary>
    /// Plugin for control handler PLC
    /// </summary>
    class HandlerPLCPlugin : PluginBase
    {
        public static HandlerPLCPlugin Instance { get; private set; }

        /// <summary>
        /// Constructors
        /// </summary>
        public HandlerPLCPlugin() : base(ModuleTypes.Handler.ToString(), true)
        {
            Instance = this;
            PlcDriver = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleType);
        }

        #region Fields

        private bool _firstCmd = true;
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
            EventCenter.SocketOpened += ReadyToUnload;

            //2.1 当load吸取的产品有问题时直接往NGtray放
            GetMessageHandler(MessageNames.CMD_LoadPutDutToTray).DataReceived += LoadPutDutToTray;

            //2.2 PLC下料端从socket中取料，完成后通知ALC并附带每个吸嘴的取料结果
            GetMessageHandler(MessageNames.CMD_PickDutFromSocket).DataReceived += PickupDutFromSocket;

            //3 PLC上料端将dut放到socket，完成后通知ALC并附带每个吸嘴的放料结果
            GetMessageHandler(MessageNames.CMD_PutDutToSocket).DataReceived += PutDutToSocket;

            //4 PLC下料端将dut放到tray盘，完成后通知ALC并附带每个吸嘴的放料结果
            GetMessageHandler(MessageNames.CMD_PutDutToTray).DataReceived += PutDutToTray;

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

            EventCenter.LoadOrUnload += ReadyToUnload;
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
                    result = ShowMsgBox($"{msg}\r\nUnit ID: {unit}", "Error" , icon: AlcMsgBoxIcon.Error, buttons: AlcMsgBoxButtons.AbortRetry, defaultButton: AlcMsgBoxDefaultButton.Button2); break;
                case AlcErrorLevel.WARN:
                    result = ShowMsgBox($"{msg}\r\nUnit ID: {unit}", "Error", icon: AlcMsgBoxIcon.Error, buttons: AlcMsgBoxButtons.AbortContinue, defaultButton: AlcMsgBoxDefaultButton.Button2); break; 
                case AlcErrorLevel.ERROR1:
                    result = ShowMsgBox($"{msg}\r\nUnit ID: {unit}", "Error", icon: AlcMsgBoxIcon.Error, buttons: AlcMsgBoxButtons.AbortRetryContinue, defaultButton: AlcMsgBoxDefaultButton.Button2); break;
                case AlcErrorLevel.ERROR2:
                    result = ShowMsgBox($"{msg}\r\nUnit ID: {unit}", "Error", icon: AlcMsgBoxIcon.Error, buttons: AlcMsgBoxButtons.ClearDutChangeTray, defaultButton: AlcMsgBoxDefaultButton.Button1); break;
                default:
                    break;
            }
            int rsp;
            if (result == AlcMsgBoxResult.Abort) rsp = 3;
            else if (result == AlcMsgBoxResult.Continue || result == AlcMsgBoxResult.ChangeTray) rsp = 2;
            else if (result == AlcMsgBoxResult.Retry) rsp = 1;
            else if (result == AlcMsgBoxResult.ClearDUT)
            {
                rsp = 1;
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
                AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Complete);
            }
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
            Log($"Handler: ALC-->PLC Error param, unit: {unit}, code: {code}, Result: {rsp}", AlcErrorLevel.DEBUG);
        }

        #endregion override

        #region Buttons

        public override bool Complete(string moduleId)
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
                    if (empty) break;
                    Thread.Sleep(200);
                }
                GetMessageHandler(MessageNames.CMD_CompelteFinish).SendAdsMessage(-2);
            });
           return base.Complete(moduleId);
        }

        public override bool Reset(string moduleId)
        {
            _firstCmd = true;
            RunModeMgr.Running = false;
            //清错
            ModuleStates[ModuleTypes.Handler.ToString()].CleaeError();
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 复位Handler", ErrorLevel.Debug);
            return base.Reset(moduleId);
        }

        public override bool Stop(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 停止Handler", ErrorLevel.Warning);
            return base.Stop(moduleId);
        }

        public override bool Abort(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 异常中止Handler", ErrorLevel.Fatal);
            return base.Abort(moduleId);
        }

        public override bool Clear(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 清错Handler", ErrorLevel.Fatal);
            return base.Clear(moduleId);
        }

        public override bool Resume(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 恢复Handler", ErrorLevel.Debug);
            return base.Resume(moduleId);
        }

        public override bool Pause(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 暂停Handler", ErrorLevel.Warning);
            return base.Pause(moduleId);
        }

        public override bool Retry(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} 重试Handler", ErrorLevel.Warning);
            return base.Retry(moduleId);
        }

        public override bool Start(string moduleId)
        {
            EventCenter.ProcessInfo?.Invoke($"{AlcSystem.Instance.GetUserAuthority()} Handler启动", ErrorLevel.Debug);
            //EventCenter.isReset?.Invoke();
            return base.Start(moduleId);
        }

        #endregion Buttons

        #region Events

        private void StateMachine_OnStateChanged()
        {
            EventCenter.StateChanged?.Invoke();
            if (RunModeMgr.GRRMode && !RunModeMgr.Running)
            {
                var state = ModuleStates.Values.First().StateMachine.CurrentState;
                if (state == SYSTEM_STATUS.Running)
                {
                    RunModeMgr.Running = true;
                    EventCenter.ProcessInfo?.Invoke($"GRR模式测试开始", ErrorLevel.Debug);
#if OldGRR
                    GRRStart();
#endif
                }

            }

            if (ModuleStates[ModuleTypes.Handler.ToString()].StateMachine.CurrentState == SYSTEM_STATUS.Resetting)
            {
                Overall.IsResetting = true;
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
            var result = AlcSystem.Instance.ShowMsgBox(errMsg, "HandlerError", AlcMsgBoxButtons.OK, 
                AlcMsgBoxIcon.Error, AlcMsgBoxDefaultButton.Button1);
            while (result != AlcMsgBoxResult.OK) ;
        }

        private void ReqClearError()
        {
            if (_isError)
                ModuleStates[ModuleTypes.Handler.ToString()].CleaeError();
        }

        private void ErrorList(MessageHandler handler, ReceivedData data)
        {
            var evens = (PLCEventItem[])data.Data.Param;
            List<string> msgs = default;
            if (evens != null)
            {
                msgs = evens.Select(e => $"Handler {e.EventId }-->{e.MultiLanMsg.CHS}").ToList();
            }
            EventCenter.ShowErrorMsgs?.Invoke(msgs ?? new List<string>());
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
            if (trayName < TrayName.LoadL || trayName > TrayName.UnloadR)
            {
                Error($"接收到的Tray ID {(int)trayName} 无效！", 0, AlcErrorLevel.WARN);
                return;
            }
            EventCenter.ProcessInfo?.Invoke($"吸嘴1从{trayName} Tray吸料", ErrorLevel.Debug);
            var tray = TrayManager.Trays[trayName];

            for (int row = 0; row < SocketGroup.ROW; row++)
                for (int col = 0; col < SocketGroup.COL; col++)
                {
                    int offset = row * SocketGroup.COL + col;
                    int trayRow = pickRows[offset] - 1;
                    int trayCol = pickCols[offset] - 1;
                    if (pickResults[offset] != 1)
                    {
                        tray.AbnormalDut(trayRow, trayCol, Dut.AbnormalDut);
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
            Station stationDefault = StationManager.Stations[StationName.Default];

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
                if (GetSystemStatus() == SYSTEM_STATUS.Completing && StationManager.Stations[StationName.Load].Empty)
                {
                    EventCenter.CloseSocket?.Invoke();
                    stationDefault.Status = StationStatus.Idle;
                }
                else
                {
                    EventCenter.ProcessInfo?.Invoke("通知Handler放料到Socket", ErrorLevel.Debug);
                    stationDefault.Status = StationStatus.WaitLoad;
                    handler.SendAdsMessage(timeout: 5000);
                }
            }
            else
            {
                EventCenter.ProcessInfo?.Invoke("通知Handler从Socket中取料", ErrorLevel.Debug);
                stationDefault.Status = StationStatus.WaitUnload;
                handler.SendAdsMessage(timeout: 5000);
                
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
            if (trayName < TrayName.LoadL || trayName > TrayName.UnloadR)
            {
                Error($"接收到的Tray ID {(int)trayName} 无效！", 0, AlcErrorLevel.WARN);
                return;
            }
            EventCenter.ProcessInfo?.Invoke($"Handler放料到{trayName} Tray中", ErrorLevel.Debug);
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
                        var dut = station.TakeDut(row, col);
                        if (dut == null)
                        {
                            dut = new Dut
                            {
                                Barcode = "noscan",
                            };
                        }
                        tray.PutDut(trayRow, trayCol, dut);
                    }
                    else if (putResult == 2)    //异常DUT,标记红色
                    {
                        tray.PutDut(trayRow, trayCol, station.TakeDut(row, col));
                        tray.AbnormalDut(trayRow, trayCol, Dut.AbnormalDut);
                    }
                    else if (putResult == 3)    // 未扫到码的DUT
                    {
                        tray.PutDut(trayRow, trayCol, station.TakeDut(row, col));
                        //tray.AbnormalDut(trayRow, trayCol, Dut.ScanFailDut);
                    }
                    else if (putResult == 4)    //扫到SN码的DUT
                    {
                        tray.PutDut(trayRow, trayCol, station.TakeDut(row, col));
                        tray.AbnormalDut(trayRow, trayCol, Dut.ScanDut);
                    }
                    else if (station.TakeDut(row, col) != null)
                    {
                        //Unhandled exception: dut miss
                        Overall.Stat.Miss++;
                    }
                }
        }

        /// <summary>
        /// 2.2 PLC下料端从socket中取料，完成后通知ALC并附带每个吸嘴的取料结果
        /// </summary>
        private void PickupDutFromSocket(MessageHandler handler, ReceivedData receivedData)
        {
            var pickResults = (ushort[]) handler.CmdParam.KeyValues[PLCParamNames.Results].Value;
            handler.SendAdsMessage(-2);

            Station stationUnload = StationManager.Stations[StationName.Unload];
            Station stationLoadUnload = StationManager.Stations[StationName.Default];
            //发送MTCP Unload数据
            //if (ConfigMgr.Instance.EnableClientMTCP)
            //{
            //    var enable = stationLoadUnload.SocketGroup.Enable ? 1 : 0;
            //    foreach (var socket in stationLoadUnload.SocketGroup.Sockets)
            //    {
            //        if (MTCPHelper.SendMTCPLotUnload(socket.Dut.Barcode, enable, out int eCode, out string eString, out string bin))
            //            EventCenter.ProcessInfo?.Invoke($"MTCP Unload 发送成功，Socket SN:{socket.Dut.Barcode}", ErrorLevel.Debug);
            //        else
            //        {
            //            EventCenter.ProcessInfo?.Invoke($"MTCP Unload 发送失败，Socket SN:{socket.Dut.Barcode}", ErrorLevel.Fatal);
            //            Error($"MTCP Unload send failed, error code:{eCode}, error message:{eString}.", 0, AlcErrorLevel.WARN);
            //        }
            //    }
            //}
            stationUnload.TakeDutFrom(stationLoadUnload);
            var sockets = stationUnload.SocketGroup.Sockets;
            CheckPutOrPickResult(sockets, pickResults);
            RunModeMgr.UnloadCount++;
            EventCenter.ProcessInfo?.Invoke($"Handler吸嘴2从socket中取料", ErrorLevel.Debug);
            if (GetSystemStatus() == SYSTEM_STATUS.Completing && StationManager.Stations[StationName.Load].Empty)
            {
                //EventCenter.CloseSocket?.Invoke();
                //stationLoadUnload.Status = StationStatus.Idle;
            }
            else
                stationLoadUnload.Status = StationStatus.WaitLoad;
            if (RunModeMgr.RunMode == RunMode.DoeTakeOff || RunModeMgr.RunMode == RunMode.DoeSocketUniformityTest)
            {
                EventCenter.PickDutDone?.Invoke();
            }
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
            EventCenter.ProcessInfo?.Invoke($"Handler吸嘴1放料到socket中", ErrorLevel.Debug);
            Station stationLoad = StationManager.Stations[StationName.Load];
            Station stationDefault = StationManager.Stations[StationName.Default];
            //如果当前状态是清料状态
            if (GetSystemStatus() == SYSTEM_STATUS.Completing && StationManager.Stations[StationName.Load].Empty)
            {
                EventCenter.CloseSocket?.Invoke();
                stationDefault.Status = StationStatus.Idle;
                return;
            }
            var loadSockets = stationLoad.SocketGroup.Sockets;
            foreach (var socket in loadSockets)
            {
                if (socket.Dut == null)
                    continue;
                socket.Dut.Barcode = Overall.ScanResult;
                DragonDbHelper.addBarcode(socket.Index, Overall.ScanResult + " — " + "S" + RunModeMgr.SocketID);
                //发送MTCP Load数据
                if (ConfigMgr.Instance.EnableClientMTCP)
                {
                    if (MTCPHelper.SendMTCPLotLoad(socket.Dut.Barcode, out int eCode, out string eString))
                        EventCenter.ProcessInfo?.Invoke($"MTCP Load 发送成功，Socket SN:{socket.Dut.Barcode}", ErrorLevel.Debug);
                    else
                    {
                        EventCenter.ProcessInfo?.Invoke($"MTCP Load 发送失败，Socket SN:{socket.Dut.Barcode}", ErrorLevel.Fatal);
                        Error($"MTCP Load send failed, error code:{eCode}, error message:{eString}.", 0, AlcErrorLevel.WARN);
                    }
                }

                stationLoad.PutDutTo(stationDefault);
                var sockets = stationDefault.SocketGroup.Sockets;
                CheckPutOrPickResult(sockets, putResults);
            }
            if (StationManager.AllStationEmpty) return;
            stationDefault.Status = StationStatus.LoadDone;
            if (RunModeMgr.RunMode == RunMode.DoeTakeOff)
            {
                //不关盖测试，放料到Socket后准备下料
                if (!RunModeMgr.IsCloseSocketCap)
                {
                    //通知Tester放料完成，可以移动东方马达
                    EventCenter.CloseSocket?.Invoke();
                    stationDefault.Status = StationStatus.Done;
                    return;
                }
                else
                {
                    //关盖测试
                    EventCenter.CloseSocket?.Invoke();
                    stationDefault.Status = StationStatus.Done;
                    return;
                }
            }
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
            if (trayName < TrayName.LoadL || trayName > TrayName.UnloadR)
            {
                Error($"接收到的Tray ID {(int)trayName} 无效！", 0, AlcErrorLevel.WARN);
                return;
            }
            EventCenter.ProcessInfo?.Invoke($"Handler吸嘴2放料到{trayName} Tray中", ErrorLevel.Debug);
            var tray = TrayManager.Trays[trayName];
            for (int row = 0; row < SocketGroup.ROW; row++)
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

        private void GRRStart()
        {
            RunModeMgr.LoadCount = 0;
            RunModeMgr.UnloadCount = 0;
            EventCenter.ProcessInfo?.Invoke($"GRR模式测试开始", ErrorLevel.Debug);
            GetMessageHandler(MessageNames.CMD_GRRStrat).SendAdsMessage(-2);
        }

        private void GRREnd(MessageHandler handler, ReceivedData data)
        {
            handler.SendAdsMessage(-2);
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
                Thread.Sleep(2000);
                EventCenter.CloseSocket?.Invoke();
                AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Stop);
                RunModeMgr.Running = false;
                EventCenter.ProcessInfo?.Invoke($"GRR模式测试结束", ErrorLevel.Debug);
                ShowMsgBox("GRR done", "GRR");
            }
            else if (RunModeMgr.RunMode == RunMode.AutoAudit)
            {
                AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Stop);
                RunModeMgr.Running = false;
                EventCenter.ProcessInfo?.Invoke($"Audit模式测试结束，自动停止", ErrorLevel.Debug);
                ShowMsgBox("Audit done", "Audit");
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
            if (trayName < TrayName.LoadL || trayName > TrayName.UnloadR)
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
                        tray.AbnormalDut(trayRow, trayCol, Dut.NoDut);
                    }
                    else if (putResult == 1)    //正常DUT
                    {
                        //正常没放的，要放到其他地方去
                        continue;
                    }
                    else if (putResult == 2)    //其他异常DUT,标记红色
                    {
                        tray.AbnormalDut(trayRow, trayCol, Dut.AbnormalDut);
                    }
                    else if (putResult == 3)    // 未扫到码的DUT
                    {
                        //tray.AbnormalDut(trayRow, trayCol, Dut.ScanFailDut);
                    }
                    else if (putResult == 4)    //单独在扫码模式下使用，扫到SN码的DUT，放回原位，要在Tray盘上与还没有扫码的DUT区别开来
                    {
                        tray.AbnormalDut(trayRow, trayCol, Dut.ScanDut);
                    }
                }
        }

        private void SetBuzzerStatus(bool enable)
        {
            PlcDriver?.WriteObject(RunModeMgr.Name_EnableBuzzer, enable);
        }

#endregion

    }
}
