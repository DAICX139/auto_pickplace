using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlcUtility;
using Newtonsoft.Json;
using Poc2Auto.Common;
using Poc2Auto.GUI;
using Poc2Auto.Model;

namespace Poc2Auto.TM
{
    public class TMPlugin : PluginBase
    {
        public TMPlugin() : base(ModuleTypes.TM.ToString())
        {
            RunModeMgr.RunModeChanged += RunModeChanged;
        }

        #region Fields

        private readonly List<string> _tmNames = new List<string> { "Test1_LIVW", "Test2_DTGT", "Test3_Backup", "Test4_BMPF" };

        #endregion Fields

        #region override

        public override bool Load()
        {
            ExpectedModuleIds = new List<string>(_tmNames);
            AutoSendCmds.Add(CommonCommands.Reset, ConfigMgr.Instance.TMResetTimeout);

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
            EventCenter.EnableTM += e => { UpdateModuleStatus(!e); };
            UpdateModuleStatus(!ConfigMgr.Instance.WithTM);
            GetMessageHandler(MessageNames.CMD_TestDone).DataReceived += TestDone;
            base.Load();

            foreach (var stationName in StationManager.TestStations)
            {
                var station = StationManager.Stations[stationName];
                var id = _tmNames[stationName - StationName.Test1_LIVW];
                station.EnableChanged += enable =>
                {
                    if (enable)
                        AddExpectedModuleId(id);
                    else
                        RemoveExpectedModuleId(id);
                };
                if (!station.Enable) RemoveExpectedModuleId(id);
            }
            return true;
        }

        public override Form GetForm()
        {
            return new Form1(this);
        }

        public override UserControl GetConfigView()
        {
            return new UCTMConfig();
        }

        #endregion override

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

        protected override void OnRegister(MessageHandler handler, ReceivedData data)
        {
            if (!ConfigMgr.Instance.WithTM)
                return;
            base.OnRegister(handler, data);

            var tmIndex = _tmNames.IndexOf(data.ModuleId);
            if (tmIndex == -1) return;
            StationManager.Stations[StationName.Test1_LIVW + tmIndex].IP = data.Ip;
        }

        protected override void OnDisconnected(MessageHandler handler, ReceivedData data)
        {
            if (!ConfigMgr.Instance.WithTM)
                return;
            base.OnDisconnected(handler, data);

            var tmIndex = _tmNames.IndexOf(data.ModuleId);
            if (tmIndex == -1) return;
            StationManager.Stations[StationName.Test1_LIVW + tmIndex].IP = "";
        }

        protected override void OnError(MessageHandler handler, ReceivedData data)
        {
            var errMsg = data.Data.Message;
            var errCode = data.Data.Result;
            handler.Reply(new ReceivedData
            {
                Data = new MessageData
                {
                     Message = errMsg,
                     Result = errCode,
                },
            } );

            Error($"{data.ModuleId} Error\r\nError Code: {errCode}\r\nError Message: {errMsg}", 0, AlcErrorLevel.WARN);
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

            if (!ConfigMgr.Instance.WithTM || !station.Enable)
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

            if (station.Status != StationStatus.RotateDone)
            {
                if (RunModeMgr.GRRMode && station.Status == StationStatus.Testing)
                {
                    //多次测试
                }
                else
                {
                    Error($"TestStart：函数被触发，但工站{stationName}状态{station.Status}不正确(状态应为{StationStatus.RotateDone})，不会通知TM测试！", 0, AlcErrorLevel.WARN);
                    return;
                }
            }

            foreach (var socket in station.SocketGroup.Sockets)
            {
                if (socket.Dut.Barcode == "扫码未启用" || socket.Dut.Barcode == "sample" || string.IsNullOrEmpty(socket.Dut.Barcode))
                {
                    Error($"在{stationName}找不到DUT信息，二维码无效，不会通知{stationName}测试！", 0, AlcErrorLevel.WARN);
                    station.Status = StationStatus.Idle;
                    return;
                }
            }

            station.Status = StationStatus.Starting;
            var result = GetMessageHandler(MessageNames.CMD_TestStart).SendMessage(new ReceivedData
            {
                ModuleId = _tmNames[stationName - StationName.Test1_LIVW],
                Data = new MessageData
                {
                    Param = new StartTestParam
                    {
                        DutSnArray = station.Barcodes,
                        SocketSn = (station.SocketGroup.Index - 1).ToString(),
                        StressCode = Overall.LotInfo?.StressCode,
                        LotId = Overall.LotInfo?.LotID
                    }
                }
            }, ConfigMgr.Instance.TMStartTestTimeout);

            if (result != MessageSendResult.Ok)
            {
                station.Status = StationStatus.StartFailed;
                return;
            }

            station.Status = StationStatus.Testing;

            station.TestTimer.Enabled = true;
        }

        private void TestDone(MessageHandler handler, ReceivedData data)
        {
            if (!ConfigMgr.Instance.WithTM)
                return;
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
            if (RunModeMgr.RunMode == RunMode.AutoAudit && !tr.AllOK && ++station.TestTimesForGRR < RunModeMgr.TestTimes)
            { 
                TestStart(station.Name);
                return;
            }
            station.TestTimesForGRR = 0;
            station.SetTestResult(tr.ResultArray);
            station.Status = StationStatus.Done;
        }

        private void Retest(StationName stationName)
        {
            TestStart(stationName);
        }

        public override bool Reset(string moduleId)
        {
            return base.Reset(moduleId);
        }

        public override bool Stop(string moduleId)
        {
            return base.Stop(moduleId);
        }

        private bool Reset(StationName stationName)
        {
            if (ModuleStates.Keys.Count == 0)
                return false;
            var state = ModuleStates[_tmNames[stationName - StationName.Test1_LIVW]].StateMachine.CurrentState;
            if (state == SYSTEM_STATUS.Idle)
            {
                Reset(_tmNames[stationName - StationName.Test1_LIVW]);
                DateTime now = DateTime.Now;
                while (true)
                {
                    bool delay = (DateTime.Now - now).TotalSeconds == 10;
                    if (state == SYSTEM_STATUS.Ready)
                        return true;
                    else if (delay)
                    {
                        AlcSystem.Instance.Error($"TM {_tmNames[stationName - StationName.Test1_LIVW]}工站复位失败，请手动复位并开始！", 0, AlcErrorLevel.ERROR1, ModuleTypes.TM.ToString());
                        return false;
                    }
                    Thread.Sleep(500);
                }
            }
            return false;
        }

        private bool Stop(StationName stationName)
        {
            if (ModuleStates.Keys.Count == 0)
                return false;
            var state = ModuleStates[_tmNames[stationName - StationName.Test1_LIVW]].StateMachine.CurrentState;
            if (state == SYSTEM_STATUS.Idle)
                return true;
            if (state >= SYSTEM_STATUS.Ready)
            {
                Stop(_tmNames[stationName - StationName.Test1_LIVW]);
                DateTime now = DateTime.Now;
                while (true)
                {
                    bool delay = (DateTime.Now - now).TotalSeconds == 10;
                    if (state == SYSTEM_STATUS.Idle)
                        return true;
                    else if (delay)
                    {
                        AlcSystem.Instance.Error($"TM {_tmNames[stationName - StationName.Test1_LIVW]}工站停止失败，请手动停止并复位！", 0, AlcErrorLevel.ERROR1, ModuleTypes.TM.ToString());
                        return false;
                    }
                    Thread.Sleep(500);
                }
            }
            return false;
        }

        private void TMReset()
        {
            var stations = StationManager.TestStations.Where(s => StationManager.Stations[s].Enable).ToList();

            foreach (var name in stations)
            {
                Stop(name);
                Reset(name);
            }
        }
        #endregion 运行流程函数

    }
}
