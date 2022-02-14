using AlcUtility;
using Newtonsoft.Json;
using Poc2Auto.Common;
using Poc2Auto.GUI;
using Poc2Auto.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poc2Auto.TM
{
    public class TMPlugin : PluginBase
    {
        public TMPlugin() : base(ModuleTypes.TM.ToString())
        {

        }

        #region Fields

        private readonly List<string> _tmNames = new List<string> { "TM_LIVW", "TM_NFBP", "TM_KYRL", "TM_BMPF" };

        #endregion Fields

        #region override

        public override bool Load()
        {
            ExpectedModuleIds = new List<string>(_tmNames);
            AutoSendCmds.Add(CommonCommands.Reset, ConfigMgr.Instance.TMResetTimeout);

            StateEnable = false;
            UpdateModuleStatus(true);
            DisableStateMachine = true;

            if (ConfigMgr.Instance.CanMoveDuringTest)
            {
                //如果开盖动作不影响测试，旋转结束后通知TM测试
                StationManager.EventRotateDone += TestStart;
            }
            else
            {
                //否则等待Socket打开后通知TM测试
                EventCenter.SocketOpened += TestStart;
            }

            EventCenter.Retest += Retest;
            EventCenter.TMReset += TMReset;

            //UpdateModuleStatus(!ConfigMgr.Instance.WithTM);

            GetMessageHandler(MessageNames.CMD_TestDone).DataReceived += TestDone;
            base.Load();

            //foreach (var stationName in StationManager.TestStations)
            //{
            //    var station = StationManager.Stations[stationName];
            //    var id = _tmNames[stationName - StationName.Test1_LIVW];
            //    station.EnableChanged += enable =>
            //    {
            //        if (enable)
            //            AddExpectedModuleId(id);
            //        else
            //            RemoveExpectedModuleId(id);
            //    };
            //    if (!station.Enable) RemoveExpectedModuleId(id);
            //}
            return true;
        }

        //public override Form GetForm()
        //{
        //    return new Form1(this);
        //}

        public override UserControl GetConfigView()
        {
            return new UCTMConfig();
        }

        #endregion override

        #region Events
        protected override void OnRegister(MessageHandler handler, ReceivedData data)
        {
            //if (!ConfigMgr.Instance.WithTM)
            //    return;
            base.OnRegister(handler, data);

            var tmIndex = _tmNames.IndexOf(data.ModuleId);
            if (tmIndex == -1) return;
            StationManager.Stations[StationName.Test1_LIVW + tmIndex].IP = data.Ip;
        }

        protected override void OnDisconnected(MessageHandler handler, ReceivedData data)
        {
            base.OnDisconnected(handler, data);

            var tmIndex = _tmNames.IndexOf(data.ModuleId);
            if (tmIndex == -1) return;
            StationManager.Stations[StationName.Test1_LIVW + tmIndex].IP = "";
        }

        protected override void OnError(MessageHandler handler, ReceivedData data)
        {
            var errMsg = data.Data.Message;
            var errCode = data.Data.Result;
            //var level = data.Data.
            handler.Reply(new ReceivedData
            {
                ModuleId = data.ModuleId,
                Data = new MessageData
                {
                     Message = errMsg,
                     Result = errCode,
                },
            } );
            if (-1 == errCode || 0 == errCode)
            {
                Error($"{data.ModuleId} Error\r\nError Code: {errCode}\r\nError Message: {errMsg}", 0, AlcErrorLevel.WARN);
            }
            else
            {
                //ButtonClickRequire(SYSTEM_EVENT.Stop);
                Error($"{data.ModuleId} Error\r\nError Code: {errCode}\r\nError Message: {errMsg}", 0, AlcErrorLevel.FATAL);
            }
        }

        #endregion Events

        #region 运行流程函数

        private void TestStart()
        {
            Parallel.ForEach(StationManager.TestStations, station => TestStart(station));
        }

        public void TestStart(StationName stationName)
        {
            var station = StationManager.Stations[stationName];

            //if (!ConfigMgr.Instance.WithTM || !station.Enable)
            if (!station.Enable)
            {
                //Thread.Sleep(8000);
                station.Status = StationStatus.Disabled;
                return;
            }

            if (!station.SocketGroup.Enable)
            {
                station.Status = StationStatus.SocketDisabled;
                return;
            }

            if (station.Empty)
            {
                station.Status = StationStatus.Idle;
                return;
            }

            if (station.Status == StationStatus.Idle) return;

            foreach (var socket in station.SocketGroup.Sockets)
            {
                if (socket.Dut.Barcode == "扫码未启用" || socket.Dut.Barcode == "sample" || string.IsNullOrEmpty(socket.Dut.Barcode))
                {
                    Error($"在{stationName}找不到DUT信息，二维码无效，不会通知{stationName}测试！", 0, AlcErrorLevel.WARN);
                    station.Status = StationStatus.Idle;
                    return;
                }
            }

            if (station.Status != StationStatus.RotateDone)
            {
                if (RunModeMgr.GRRMode && station.Status == StationStatus.Testing)
                {
                    //多次测试
                }
                else //非GRR和Audit模式下
                {                         
                    if (station.Status == StationStatus.Testing)
                    {
                        //当前工站测试无反应，超时过后重新发送测试命令
                        Dictionary<StationName, int> testResult = default;
                        string barcode = default;
                        for (int i = 0; i < SocketGroup.ROW; i++)
                        {
                            for (int j = 0; j < SocketGroup.COL; j++)
                            {
                                testResult = station.SocketGroup.Sockets[i, j].Dut.TestResult;
                                barcode = station.SocketGroup.Sockets[i, j].Dut.Barcode;
                            }
                        }
                        if (RunModeMgr.TMRetest)//可以进行重测操作
                        {
                            //直接通知TM测试
                        }
                        else
                        {
                            if (!testResult.ContainsKey(station.Name))//Dut没有在这个工位测试过
                            {
                                //重新发送测试命令
                            }
                            else
                            {
                                //Error($"TestStart：函数被触发，但工站{stationName}状态{station.Status}不正确(状态应为{StationStatus.RotateDone})，不会通知TM测试！", 0, AlcErrorLevel.WARN);
                                Error($"产品:{barcode}已经在{station.Name}有测试结果，测试结果为{testResult[station.Name]},因此不会通知TM重复测试!", 0, AlcErrorLevel.WARN);
                                EventCenter.ProcessInfo?.Invoke($"产品:{barcode}已经在{station.Name}有测试结果，测试结果为{testResult[station.Name]},因此不会通知TM重复测试!", ErrorLevel.WARNING);
                                station.Status = StationStatus.Done;
                                return;
                            }
                        }
                    }
                    else if (RunModeMgr.IsRotate) //如果已经达到了旋转条件（说明所有工站已经有了测试结果） 就不能给TM发送测试命令
                    {
                        Dictionary<StationName, int> testResult = default;
                        string barcode = default;
                        for (int i = 0; i < SocketGroup.ROW; i++)
                        {
                            for (int j = 0; j < SocketGroup.COL; j++)
                            {
                                testResult = station.SocketGroup.Sockets[i, j].Dut.TestResult;
                                barcode = station.SocketGroup.Sockets[i, j].Dut.Barcode;
                            }
                        }

                        Error($"产品:{barcode}已经在{station.Name}有测试结果，测试结果为{testResult[station.Name]},因此不会通知TM重复测试!", 0, AlcErrorLevel.WARN);
                        EventCenter.ProcessInfo?.Invoke($"产品:{barcode}已经在{station.Name}有测试结果，测试结果为{testResult[station.Name]},因此不会通知TM重复测试!", ErrorLevel.WARNING);
                        station.Status = StationStatus.Done;
                        return;
                    }
                    
                }
            }
            else
            {
                if (RunModeMgr.TMRetest)//可以进行重测操作
                {
                    //直接通知TM测试
                }
                else
                {
                    Dictionary<StationName, int> testResult = default;
                    string barcode = default;
                    for (int i = 0; i < SocketGroup.ROW; i++)
                    {
                        for (int j = 0; j < SocketGroup.COL; j++)
                        {
                            testResult = station.SocketGroup.Sockets[i, j].Dut.TestResult;
                            barcode = station.SocketGroup.Sockets[i, j].Dut.Barcode;
                        }
                    }

                    if (!testResult.ContainsKey(station.Name))//Dut没有在这个工位测试过
                    {
                        //给TM发送开始测试命令
                    }
                    else
                    {
                        //Error($"TestStart：函数被触发，但工站{stationName}状态{station.Status}不正确(状态应为{StationStatus.RotateDone})，不会通知TM测试！", 0, AlcErrorLevel.WARN);
                        Error($"产品:{barcode}已经在{station.Name}有测试结果，测试结果为{testResult[station.Name]},因此不会通知TM重复测试!", 0, AlcErrorLevel.WARN);
                        EventCenter.ProcessInfo?.Invoke($"产品:{barcode}已经在{station.Name}有测试结果，测试结果为{testResult[station.Name]},因此不会通知TM重复测试!", ErrorLevel.WARNING);
                        station.Status = StationStatus.Done;
                        return;
                    }
                }
            }

#if SOCKETSAFETYSIGNAL_ON

            Retry:
            //0.1, Socket安全检测信号断电
            RunModeMgr.SetSocketSignal = false;
            //0.2, 二次确认：读取Socket安全检测信号是否设置成功
            if (RunModeMgr.SocketSafetySignal) //没有设置成功,重复写入20次
            {
                for (int i = 0; i < 20; i++)
                {
                    RunModeMgr.SetSocketSignal = false;
                }
                if (RunModeMgr.SocketSafetySignal)
                {
                    var boxResult = AlcSystem.Instance.ShowMsgBox("Socket安全信号设置掉电失败," +
                     "请人为确认", "TM", AlcMsgBoxButtons.StopRetryContinue, 
                     AlcMsgBoxIcon.Error, AlcMsgBoxDefaultButton.Button1);
                    switch (boxResult)
                    {
                        case AlcMsgBoxResult.Retry:
                            goto Retry;
                        case AlcMsgBoxResult.Stop:
                            ButtonClickRequire(SYSTEM_EVENT.Stop);
                            return;
                        case AlcMsgBoxResult.Continue:
                        default:
                            break;
                    }
                }
            }
#endif

            station.Status = StationStatus.Starting;
            var result = GetMessageHandler(MessageNames.CMD_TestStart).SendMessage(new ReceivedData
            {
                ModuleId = _tmNames[stationName - StationName.Test1_LIVW],
                Data = new MessageData
                {
                    Param = new StartTestParam
                    {
                        DutSnArray = station.Barcodes,
                        SocketSn = "S"+RunModeMgr.GetActualSocketID(station.SocketGroup.Index).ToString(),
                        StressCode = Overall.LotInfo?.StressCode,
                        LotId = Overall.LotInfo?.LotID,
                        Mode = RunModeMgr.RunMode == RunMode.AutoAudit ? "audit":"normal",
                        MTCP = ConfigMgr.Instance.EnableClientMTCP,
                    }
                }
            }, ConfigMgr.Instance.TMStartTestTimeout);

            if (result != MessageSendResult.Ok)
            {
                station.Status = StationStatus.StartFailed;
                return;
            }
            EventCenter.ProcessInfo?.Invoke($"通知{station.Name}开始测试", ErrorLevel.DEBUG);
            station.Status = StationStatus.Testing;

            station.TestTimer.Enabled = true;
        }

        private void TestDone(MessageHandler handler, ReceivedData data)
        {
            AlcSystem.Instance.Log($"input TestDone()", "TM相关");
            //if (!ConfigMgr.Instance.WithTM)
            //    return;
            handler.Reply(new ReceivedData
            {
                ModuleId = data.ModuleId,
                Data = new MessageData { Channel = data.Data.Channel }
            });

            var tmIndex = _tmNames.IndexOf(data.ModuleId);
            if (tmIndex == -1) return;
            var station = StationManager.Stations[StationName.Test1_LIVW + tmIndex];
            station.TestTimer.Enabled = false;
            station.TimeOutTotal = 0;

            if (RunModeMgr.RunMode == RunMode.AutoGRR)
            {
#if OldGRR
                station.TestTimesForGRR++;
                if (station.TestTimesForGRR < RunModeMgr.TestTimes)
                {
                    TestStart(station.Name);
                    return;
                }
#endif
            }
            TestResult tr = JsonConvert.DeserializeObject<TestResult>(data.Data.Param.ToString());
            EventCenter.ProcessInfo?.Invoke($"{station.Name}测试完成，测试结果为 {tr.Result[0]}", ErrorLevel.INFO);
            //++station.TestTimesForGRR < RunModeMgr.TestTimes
            if (RunModeMgr.RunMode == RunMode.AutoAudit && !tr.AllOK && ConfigMgr.Instance.Retest)
            {
                //EventCenter.ProcessInfo?.Invoke($"{station.Name}测试失败，通知{station.Name}重测", ErrorLevel.Fatal);
                //var result =  AlcSystem.Instance.ShowMsgBox($"产品:{station.SocketGroup.Sockets[0,0].Dut.Barcode}" +
                //    $"在{station.Name}测试失败, 是否重测?", "Audit", AlcMsgBoxButtons.YesNo, 
                //    AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button1);
                //if (AlcMsgBoxResult.No == result)
                //{
                //    //啥也不做
                //}
                //else
                //{
                //    TestStart(station.Name);
                //}
            }
            station.TestTimesForGRR = 0;
            station.SetTestResult(tr.ResultArray);
            EventCenter.StationTestDone?.Invoke(station, station.SocketGroup.Duts[0, 0]?.Barcode, tr.ResultArray[0, 0]);

            if (RunModeMgr.RunMode == RunMode.AutoNormal)
            {
                if (tr.Result[0] != Dut.PassBin)
                {
                    station.TestFailTimes++;
                }
                else
                {
                    station.TestFailTimes = 0;
                }
            }

            //当工站连续测试失败次数大于设定次数次,弹框提醒
            if (station.TestFailTimes > ConfigMgr.Instance.StationTestFailTimes)
            {
                station.TestFailTimes = 0;
                Error($"{data.ModuleId} Error\r\n检测到{station.Name}工站连续测试失败超过{ConfigMgr.Instance.StationTestFailTimes}次！请检查该工站！", 0, AlcErrorLevel.FATAL);
            }

#if SOCKETSAFETYSIGNAL_ON
            Retry:
            //0.1, Socket安全检测信号上电
            RunModeMgr.SetSocketSignal = true;
            //0.2, 二次确认：读取Socket安全检测信号是否设置成功
            if (!RunModeMgr.SocketSafetySignal) //没有设置成功,重复写入20次
            {
                for (int i = 0; i < 20; i++)
                {
                    RunModeMgr.SetSocketSignal = true;
                }
                if (!RunModeMgr.SocketSafetySignal)
                {
                    var boxResult = AlcSystem.Instance.ShowMsgBox("Socket安全信号设置上电失败," +
                     "请人为确认", "TM", AlcMsgBoxButtons.StopRetryContinue,
                     AlcMsgBoxIcon.Error, AlcMsgBoxDefaultButton.Button1);
                    switch (boxResult)
                    {
                        case AlcMsgBoxResult.Retry:
                            goto Retry;
                        case AlcMsgBoxResult.Stop:
                            ButtonClickRequire(SYSTEM_EVENT.Stop);
                            return;
                        case AlcMsgBoxResult.Continue:
                        default:
                            break;
                    }
                }
            }
#endif

            station.Status = StationStatus.Done;

            AlcSystem.Instance.Log($"output TestDone()", "TM相关");
        }

        private void Retest(StationName stationName)
        {
            TestStart(stationName);
        }

        private void TMReset()
        {
            Task.Run(() => 
            {
                var stations = StationManager.TestStations.Where(s => StationManager.Stations[s].ConnectState).ToList();

                foreach (var stationName in stations)
                {
                    var result = GetMessageHandler(MessageNames.CMD_Reset).SendMessage(new ReceivedData
                    {
                        ModuleId = _tmNames[stationName - StationName.Test1_LIVW],

                    }, ConfigMgr.Instance.TMResetTimeout);
                    //Thread.Sleep(2);
                }
            });
        }
      
#endregion 运行流程函数

    }
}
