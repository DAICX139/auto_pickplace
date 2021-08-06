using NetAndEvent.PlcDriver;
using System.Collections.Generic;
using CYGKit.AdsProtocol;
using Poc2Auto.Model;
using System;
using AlcUtility;
using Poc2Auto.Database;
using System.IO;
using LogLib.Managers;

namespace Poc2Auto.Common
{
    public static class RunModeMgr
    {
        private static RunMode _runMode;
        public static event Action RunModeChanged;
        public static Action YieldDutChanged;
        public static Action FailDutChanged;
        public static Action CurrentSocketIDChanged;

        /// <summary>
        /// 当前运行模式
        /// </summary>
        public static RunMode RunMode
        {
            get => _runMode;
            set
            {
                if (_runMode == value) return;
                ConfigMgr.Instance.RunMode = value;
                _runMode = value;
                RunModeChanged?.Invoke();
            }
        }

        private static CustomMode _customMode;
        public static event Action CustomModeChanged;
        private static AdsDriverClient testerClient = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Tester.ToString()) as AdsDriverClient;

        public static CustomMode CustomMode
        {
            get => _customMode;
            set
            {
                if (_customMode == value) return;
                ConfigMgr.Instance.CustomMode = value;
                _customMode = value;
                CustomModeChanged?.Invoke();
            }
        }

        private static ushort _socketID;

        public static ushort SocketID
        {
            get { return _socketID; }
            set 
            { 
                _socketID = value;
                CurrentSocketIDChanged?.Invoke();
            }
        }
        public static string IsMatchDutSN;

        private static int _yieldDut;

        public static int YieldDut
        {
            get { return _yieldDut; }
            set 
            { 
                _yieldDut = value;
                YieldDutChanged?.Invoke();
            }
        }

        private static int _failDut;

        public static int FailDut
        {
            get { return _failDut; }
            set 
            { 
                _failDut = value;
                FailDutChanged?.Invoke();
            }
        }

        public static int UnloadTotal;

        public static void Init()
        {
            RunMode = ConfigMgr.Instance.RunMode;
            CustomMode = ConfigMgr.Instance.CustomMode;
        }

        #region PLCData

        public const int Manual = 1;
        public const int Auto = 2;
        public const int DryRun = 3;
        public const int Doe = 7;

        public const int Auto_Normal = 1;
        public const int Auto_SelectDUT = 2;
        public const int Auto_Mark = 3;
        public const int Auto_GoldenDut = 4;
        public const int Doe_SlipTest = 1;
        public const int Doe_SameTrayTest = 2;
        public const int Doe_DifferentTrayTest = 3;
        public const int Doe_TakeoffTest = 4;
        //新增：Socket一致性测试模式
        public const int Doe_SocketUniformityTest = 5;
        //新增：转盘的Debug模式（开Socket盖子， 关闭Socket盖子等等）
        public const int Doe_TesterDebug = 6;
        //新增转盘模式：当只有Handler有动作而Teater没有动作时（如挑料、扫码模式），给Tester写入当前模式(该模式下不做任何动作)，确保两边状态统一
        public const int Doe_TesterStateSyncMode = 10;

        public static string Name_SelectLoadtrayID = "GVL_MachineCom.SelectLoadtrayID";
        public static string Name_SelectUnLoadtrayID = "GVL_MachineCom.SelectUnLoadtrayID";
        public static string Name_SelectDUTnumber = "GVL_MachineCom.SelectDUTnumber";
        public static string Name_SelectLoadBinID = "GVL_MachineCom.SelectLoadBinID";
        public static string Name_SelectUnLoadBinID = "GVL_MachineCom.SelectUnLoadBinID";
        //挑SN和单独扫码模式切换变量
        public static string Name_SelectSnMode = "GVL_MachineCom.SelectSnMode";

        //Old GRR params
        //public static string Name_GRRTrayID = "GVL_MachineCom.GoldenDutLoadtrayID";
        //public static string Name_GRRDutCount = "GVL_MachineCom.GoldenDutNumber";

        //New GRR params
        public static string Name_GRRTrayID = "GVL_MachineCom.GoldenDutLoadtrayID";
        public static string Name_GRRDutNumber = "GVL_MachineCom.GRRDutNumber";
        public static string Name_GRRTestNumbert = "GVL_MachineCom.GRRTMTestNumber";
        public static string Name_GRRTestRow = "GVL_MachineCom.GRRRow";
        public static string Name_GRRTestCol= "GVL_MachineCom.GRRCol";


        public static string Name_SlipTesttrayID = "GVL_MachineCom.SlipTesttrayID";
        public static string Name_SlipTestSuction = "GVL_MachineCom.SlipTestSuction";
        public static string Name_SlipTestRow = "GVL_MachineCom.SlipTestRow";
        public static string Name_SlipTestCol = "GVL_MachineCom.SlipTestCol";
        public static string Name_SlipTestApos = "GVL_MachineCom.ATestPosition";
        public static string Name_SlipTestBpos = "GVL_MachineCom.BTestPosition";
        public static string Name_SlipTestNumber = "GVL_MachineCom.SlipTestNumber";

        public static string Name_SameTrayID = "GVL_MachineCom.SameTrayLoadtrayID";
        public static string Name_SameTrayBinID = "GVL_MachineCom.SameTrayLoadBinID";
        public static string Name_SameTrayStartRow = "GVL_MachineCom.SameTrayStartRow";
        public static string Name_SameTrayStartCol = "GVL_MachineCom.SameTrayStartCol";

        public static string Name_DifferenttTrayLoadTrayID = "GVL_MachineCom.DifferentTrayLoadtrayID";
        public static string Name_DifferenttTrayUnLoadTrayID = "GVL_MachineCom.DifferentTrayUnLoadtrayID";

        public static string Name_TakeOffTestTrayID = "GVL_MachineCom.Take_offTestTrayID";
        public static string Name_TakeOffTestRow = "GVL_MachineCom.Take_offTestRow";
        public static string Name_TakeOffTestCol = "GVL_MachineCom.Take_offTestCol";
        public static string Name_TakeOffTestTimes = "GVL_MachineCom.Take_offTestNumber";
        public static string Name_TakeOffIsCloseCap = "GVL_MachineCom.Take_offTestChoose";
        public static string Name_Take_offSocketID = "GVL_MachineCom.Take_offSocketID";

        public static string Name_MarkMode = "GVL_MachineCom.MarkMode";
        public static string Name_MarkTrayChoose = "GVL_MachineCom.MarkTrayChoose";
        public static string Name_MarkTrayID = "GVL_MachineCom.MarkTrayID";

        public static string Name_EnableBuzzer = "GVL_MachineDevice.bSilencer";

        //调试用
        public static string Name_DryRunTesterRotation = "GVL_MachineCom.DrayStart";
        public static string Name_DryRunChoose = "GVL_MachineCom.DrayRunChoose";

        //Handler Debug用的变量
        public static string Name_DebugNozzle1TrayLoad = "GVL_MachineCom.Nozzle1TrayLoad";
        public static string Name_DebugNozzle1Scan = "GVL_MachineCom.Nozzle1Scan";
        public static string Name_DebugNozzle1SocketPut	= "GVL_MachineCom.Nozzle1SocketPut";
        public static string Name_DebugNozzle2SocketPick = "GVL_MachineCom.Nozzle2SocketPick";
        public static string Name_DebugNozzle1TrayUload = "GVL_MachineCom.Nozzle1TrayUload";
        public static string Name_DebugNozzle2TrayUload = "GVL_MachineCom.Nozzle2TrayUload";

        //Tester Debug用的变量
        public static string Name_DebugOpenSocket = "GVL_MachineCom.bOpenSocket";
        public static string Name_DebugCloseSocket = "GVL_MachineCom.bCloseSocket";
        public static string Name_DebugZAxisUp = "GVL_MachineCom.bZUp";
        public static string Name_DebugZAxisDown = "GVL_MachineCom.bZdn";
        public static string Name_DebugPushPutter= "GVL_MachineCom.bPushPutter";
        public static string Name_DebugPullPutter = "GVL_MachineCom.bPullPutter";
        public static string Name_SocketID = "GVL_MachineCom.nSocketID";

        //Socket一致性测试PLC变量
        public static string Name_TMContinueTest = "GVL_MachineCom.TMContinueTest";
        public static string Name_TMTestSocketID = "GVL_MachineCom.TMTestSocketID";
        public static string Name_TMTestStationID = "GVL_MachineCom.TMTestStationID";
        public static string Name_TMTestTrayID = "GVL_MachineCom.TMTestTrayID";
        public static string Name_TMTestRow = "GVL_MachineCom.TMTestRow";
        public static string Name_TMTestCol = "GVL_MachineCom.TMTestCol";
        public static string Name_TMTestTimes = "GVL_MachineCom.TMTestNumber";

        //Tester变量：控制手动推拉pin针气缸
        public static string Name_TesterScoketPullCylider = "GVL_MachineCom.bAutoPullCylider";
        public static string Name_TesterScoketReleaseCylider = "GVL_MachineCom.bAutoReleaseCylider";

        public static string TrayInfoIsEmpty(int index)
        {
            return $"GVL_MachineDevice.stTrayInfo[{index}].bEmpty";
        }

        public static string TrayInfoIsFull(int index)
        {
            return $"GVL_MachineDevice.stTrayInfo[{index}].bFull";
        }

        public static string STLoadTrayPoint(int index)
        {
           return $"GVL_MachineParam.stLoadTrayPoint[{index}].TrayXYRanks";
        }

        #endregion PLCData

        public static int SelectTrayOffsetID = 1000;

        public static bool GRRMode => RunMode == RunMode.AutoGRR || RunMode == RunMode.AutoAudit;
        public static bool Running;
        public static int TestTimes;
        public static bool IsCloseSocketCap;
        public static int LoadCount;
        public static int UnloadCount;
        public static List<int> TestedSockets = new List<int>();
        public const int TMTestTimeOut = 180;
        public static bool IsOnlyScanMode;
        public static bool IsLoadLTray;
        public static ushort PickRow;
        public static ushort PickCol;
        public static string SelectSNSavePath;

        public static bool ManualMode(AdsDriverClient client, out string message)
        {
            if (testerClient == null || client == null)
            {
                message = "Tester 未连接成功!";
                return false;
            }
            var result = client.GetSysInfoCtrl.ModeCtrl(Manual);
            if (result != 0)
            {
                message = "Handler 手段模式切换失败!";
                return false;
            }
            result = testerClient.GetSysInfoCtrl.ModeCtrl(Manual);
            if (result != 0)
            {
                message = "Tester 手段模式切换失败!";
                return false;
            }

            message = "OK";
            return true;
        }

        public static bool AutoNormal(AdsDriverClient client, out string message)
        {
            if (testerClient == null || client == null)
            {
                message = "Tester 未连接成功!";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Auto_Normal);
            var ret3 = testerClient.GetSysInfoCtrl.ModeCtrl(Auto);
            if (ret1 + ret2 + ret3 != 0)
            {
                message = "生产模式切换失败!";
                return false;
            }

            message = "OK";
            return true;
        }

        public static bool AutoSelectSn(AdsDriverClient client, AutoSelectSnParam param, out string message)
        {
            if (testerClient == null)
            {
                message = "Tester 未连接成功!";
                return false;
            }
            if (param.LoadNum == 0)
            {
                message = @"上料盘LoadL数据为空，请重新选择!";
                return false;
            }
            if (param.LoadNum < param.DutSnList.Count)
            {
                message = @"上料盘LoadL数据个数小于挑选SN个数，请重新选择!";
                return false;
            }

            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Auto_SelectDUT);

            var ret3 = testerClient.GetSysInfoCtrl.ModeCtrl(Doe);
            var ret4 = testerClient.GetSysInfoCtrl.SubModeCtrl(Doe, Doe_TesterStateSyncMode);
            if (ret1 + ret2 + ret3 + ret4 != 0)
            {
                message = "挑选SN模式切换失败!";
                return false;
            }
            //var ret = client.WriteObject(Name_SelectLoadtrayID, (ushort)param.LoadTaryID, out message);
            //if (ret != 0) return false;
            //ret = client.WriteObject(Name_SelectDUTnumber, (ushort)param.DutSnList.Count, out message);
            //if (ret != 0) return false;

            var ret = client.WriteObject(Name_SelectSnMode, (ushort)(IsOnlyScanMode ? 2 : 1), out message);
            if (ret != 0) return false;

            //var result = client.WriteBinRegion(param.LoadTaryID, param.LoadRegion, out message);
            //if (!result) return false;
            //var result = client.WriteTrayData(param.LoadTaryID, param.LoadRegion, out message);
            //if (!result) return false;


            //DragonDbHelper.ClearTrayData((int)(TrayName)param.LoadTaryID);
            //TrayManager.Trays[(TrayName)param.LoadTaryID].SetData(param.LoadRegion);

            //result = client.WriteBinRegion(param.UnloadTrayID, param.UnloadRegion, out message);
            //if (!result) return false;

            var result = client.WriteTrayData((int)TrayName.LoadR, new int[Tray.ROW, Tray.COL], out message);
            if (!result) return false;
            result = client.WriteTrayData((int)TrayName.NG, new int[Tray.ROW, Tray.COL], out message);
            if (!result) return false;

            //清空数据库中上料2盘的数据
            DragonDbHelper.ClearTrayData((int)TrayName.LoadR);
            TrayManager.Trays[TrayName.LoadR].SetData(new int[Tray.ROW, Tray.COL]);

            //清空数据库中NG盘的数据
            DragonDbHelper.ClearTrayData((int)TrayName.NG);
            TrayManager.Trays[TrayName.NG].SetData(new int[Tray.ROW, Tray.COL]);

            Overall.SelectionList = new List<string>(param.DutSnList);

            return true;
        }

        public static bool AutoSelectBin(AdsDriverClient client, AutoSelectBinParam param, out string message)
        {
            if (testerClient == null)
            {
                message = "Tester 未连接成功!";
                return false;
            }
            if (param.LoadNum == 0)
            {
                message = @"上料盘数据为空，请重新选择!";
                return false;
            }
            if (param.UnloadNum < param.LoadNum)
            {
                message = @"下料区域小于上料区域";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Auto_SelectDUT);

            var ret3 = testerClient.GetSysInfoCtrl.ModeCtrl(Doe);
            var ret4 = testerClient.GetSysInfoCtrl.SubModeCtrl(Doe, Doe_TesterStateSyncMode);
            if (ret1 + ret2 + ret3 + ret4 != 0)
            {
                message = "挑选Bin模式切换失败!";
                return false;
            }

            var ret = client.WriteObject(Name_SelectDUTnumber, (ushort)param.LoadNum, out message);
            if (ret != 0) return false;
            ret = client.WriteObject(Name_SelectLoadtrayID, (ushort)param.LoadTaryID, out message);
            if (ret != 0) return false;
            //var result = client.WriteBinRegion(param.LoadTaryID, param.LoadRegion, out message);
            //if (!result) return false;
            var result = client.WriteTrayData(param.LoadTaryID, param.LoadRegion, out message);
            if (!result) return false;

            DragonDbHelper.ClearTrayData((int)(TrayName)param.LoadTaryID);
            TrayManager.Trays[(TrayName)param.LoadTaryID].SetData(param.LoadRegion);

            ret = client.WriteObject(Name_SelectUnLoadtrayID, (ushort)param.UnloadTrayID);
            if (ret != 0) return false;
            result = client.WriteBinRegion(param.UnloadTrayID, param.UnloadRegion, out message);
            if (!result) return false;
            result = client.WriteTrayData(param.UnloadTrayID, new int[Tray.ROW, Tray.COL], out message);
            if (!result) return false;

            DragonDbHelper.ClearTrayData((int)(TrayName)param.UnloadTrayID);
            TrayManager.Trays[(TrayName)param.UnloadTrayID].SetData(new int[Tray.ROW, Tray.COL]);

            return true;
        }

        public static bool GRR(AdsDriverClient client, GRRParam param, out string message)
        {
            if (testerClient == null)
            {
                message = "Tester 未连接成功!";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Auto_GoldenDut);
            var ret3 = testerClient.GetSysInfoCtrl.ModeCtrl(Auto);
            if (ret1 + ret2 + ret3 != 0)
            {
                message = "GRR模式模式切换失败!";
                return false;
            }
#if OldGRR
            var result = client.WriteObject(Name_GRRTrayID, (ushort)param.TrayID, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_GRRDutCount, (ushort)param.DutCount, out message);
            if (result != 0) return false;
#endif
            var result = client.WriteObject(Name_GRRTestNumbert, (ushort)param.TestTimes, out message);
            if (result != 0) return false;
            //var ret = client.WriteTrayData((int)TrayName.NG, param.Region, out message);
            //if (!ret) return false;

            Running = false;
#if OldGRR
            TestTimes = param.TestTimes;
            LoadCount = 0;
            UnloadCount = 0;
            TestedSockets = new List<int>();
#endif
            return true;
        }

        public static bool Audit(AdsDriverClient client, GRRParam param, out string message)
        {
            client.GetSysInfoCtrl.ModeCtrl(Auto);
            client.GetSysInfoCtrl.SubModeCtrl(Auto, Auto_GoldenDut);
            testerClient.GetSysInfoCtrl.ModeCtrl(Auto);

            var result = client.WriteObject(Name_GRRTrayID, (ushort)param.TrayID, out message);
            if (result != 0) return false;
            //result = client.WriteObject(Name_GRRDutCount, (ushort)param.DutCount, out message);
            //if (result != 0) return false;

            Running = false;
            TestTimes = param.TestTimes;
            LoadCount = 0;
            UnloadCount = 0;
            TestedSockets = new List<int>();

            return true;
        }

        public static bool SlipTest(AdsDriverClient client, SlipParam param, out string message)
        {
            if (testerClient == null)
            {
                message = "Tester 未连接成功!";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Doe);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Doe, Doe_SlipTest);

            var ret3 = testerClient.GetSysInfoCtrl.ModeCtrl(Doe);
            var ret4 = testerClient.GetSysInfoCtrl.SubModeCtrl(Doe, Doe_TesterStateSyncMode);
            if (ret1 + ret2 + ret3 + ret4 != 0)
            {
                message = "滑移测试模式切换失败!";
                return false;
            }
            var result = client.WriteObject(Name_SlipTesttrayID , param.TaryID, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_SlipTestSuction, param.TestNozzle, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_SlipTestRow, param.TestRow, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_SlipTestCol, param.TestCol, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_SlipTestApos, param.ATestPos, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_SlipTestBpos, param.BTestPos, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_SlipTestNumber, param.TestNumber, out message);
            if (result != 0) return false;

            return true;
        }

        public static bool SameTrayTest(AdsDriverClient client, SameTrayParam param, out string message)
        {
            if (testerClient == null)
            {
                message = "Tester 未连接成功!";
                return false;
            }
            if (param.StartRow == 0 || param.StartCol == 0)
            {
                message = @"请选择Dut！";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Doe);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Doe, Doe_SameTrayTest);

            var ret3 = testerClient.GetSysInfoCtrl.ModeCtrl(Doe);
            var ret4 = testerClient.GetSysInfoCtrl.SubModeCtrl(Doe, Doe_TesterStateSyncMode);
            if (ret1 + ret2 + ret3 + ret4 != 0)
            {
                message = "同Tray验证模式切换失败!";
                return false;
            }
            var result = client.WriteObject(Name_SameTrayID, (ushort)param.TaryID, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_SameTrayStartRow, param.StartRow, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_SameTrayStartCol, param.StartCol, out message);
            if (result != 0) return false;

            DragonDbHelper.ClearTrayData((int)(TrayName)param.TaryID);
            TrayManager.Trays[(TrayName)param.TaryID].SetData(param.TrayRegion);

            return true;
        }

        public static bool DifferentTrayTest(AdsDriverClient client, DifferentTrayParam param, out string message)
        {
            if (testerClient == null)
            {
                message = "Tester 未连接成功!";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Doe);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Doe, Doe_DifferentTrayTest);

            var ret3 = testerClient.GetSysInfoCtrl.ModeCtrl(Doe);
            var ret4 = testerClient.GetSysInfoCtrl.SubModeCtrl(Doe, Doe_TesterStateSyncMode);
            if (ret1 + ret2 + ret3 + ret4 != 0)
            {
                message = "不同Tray模式切换失败!";
                return false;
            }

            var result = client.WriteObject(Name_DifferenttTrayLoadTrayID, (ushort)param.LoadTaryID, out message);
            if (result != 0) return false;

            result = client.WriteObject(Name_DifferenttTrayUnLoadTrayID, (ushort)TrayName.NG, out message);
            if (result != 0) return false;

            var ret = client.WriteTrayData(param.LoadTaryID, param.LoadRegion, out message);
            if (!ret) return false;
            DragonDbHelper.ClearTrayData((int)(TrayName)param.LoadTaryID);
            TrayManager.Trays[(TrayName)param.LoadTaryID].SetData(param.LoadRegion);

            return true;
        }

        public static bool TakeOffTest(AdsDriverClient client, TakeOffParam param, out string message)
        {
            if (testerClient == null)
            {
                message = "Tester 未连接成功!";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Doe);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Doe, Doe_TakeoffTest);
            var ret3 = testerClient.GetSysInfoCtrl.ModeCtrl(Doe);
            var ret4 = testerClient.GetSysInfoCtrl.SubModeCtrl(Doe, Doe_TakeoffTest);
            if (ret1 + ret2 + ret3 + ret4 != 0)
            {
                message = "三伤验证模式切换失败!";
                return false;
            }

            var result = client.WriteObject(Name_TakeOffTestTrayID, (ushort)param.TrayID, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_TakeOffTestRow, (ushort)param.TestRow, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_TakeOffTestCol, (ushort)param.TestCol, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_TakeOffTestTimes, (ushort)param.TestTimes, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_TakeOffIsCloseCap, (ushort)(param.IsCloseCap ? 1 : 0), out message);
            if (result != 0) return false;
            result = testerClient.WriteObject(Name_TakeOffIsCloseCap, (ushort)(param.IsCloseCap ? 1 : 0), out message);
            if (result != 0) return false;
            result = testerClient.WriteObject(Name_Take_offSocketID, (ushort)param.SocketID, out message);
            if (result != 0) return false;

            IsCloseSocketCap = param.IsCloseCap;
            return true;
        }

        public static bool AutoMark(AdsDriverClient client, AutoMark param, out string message)
        {
            if (testerClient == null)
            {
                message = "Tester 未连接成功!";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Auto_Mark);

            var ret3 = testerClient.GetSysInfoCtrl.ModeCtrl(Doe);
            var ret4 = testerClient.GetSysInfoCtrl.SubModeCtrl(Doe, Doe_TesterStateSyncMode);
            if (ret1 + ret2 + ret3 + ret4 != 0)
            {
                message = "自动标定模式q切换失败";
                return false;
            }

            if (param.MarkMode == 0)
            {
                param.MarkTrayChoose = 0;
                param.MarkTrayId = 0;
            }
            else if (param.MarkMode == -1 || param.MarkTrayChoose == -1)
            {
                message = "选择的参数不正确！";
                return false;
            }

            var result = client.WriteObject(Name_MarkMode, (ushort)param.MarkMode, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_MarkTrayChoose, (ushort)param.MarkTrayChoose, out message);
            if (result != 0) return false; 
            result = client.WriteObject(Name_MarkTrayID, (ushort)param.MarkTrayId, out message);
            if (result != 0) return false;

            return true;
        }

        public static bool DryRun1(out string message)
        {
            testerClient.GetSysInfoCtrl.ModeCtrl(DryRun);
            message = "ok";

            return true;
        }

        public static bool SockeTest(AdsDriverClient client, SocketUniformityTest param, out string message)
        {
            if (testerClient == null)
            {
                message = "Tester 未连接成功!";
                return false;
            }
            //Handler切换模式
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Doe);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Doe, Doe_SocketUniformityTest);

            //Tester切换模式
            var ret3 = testerClient.GetSysInfoCtrl.ModeCtrl(Doe);
            var ret4 = testerClient.GetSysInfoCtrl.SubModeCtrl(Doe, Doe_SocketUniformityTest);
            if (ret1 + ret2 + ret3 + ret4 != 0)
            {
                message = "Socket 一致性模式切换失败";
                return false;
            }

            //变量写入
            var result = client.WriteObject(Name_TMTestTrayID, (ushort)param.TrayID, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_TMTestRow, (ushort)param.TestRow, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_TMTestCol, (ushort)param.TestCol, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_TMTestTimes, (ushort)param.TestTimes, out message);
            if (result != 0) return false;
            result = testerClient.WriteObject(Name_TMTestSocketID, (ushort)param.SocketID, out message);
            if (result != 0) return false;
            result = testerClient.WriteObject(Name_TMTestStationID, (ushort)param.StationID, out message);
            if (result != 0) return false;

            return true;
        }

        public static ushort[,] IntArrayToUshortArray(int[,] data)
        {
            if (data.Length == 0 || data == null)
                return null;
            var x = data.GetLength(0);
            var y = data.GetLength(1);
            ushort[,] result = new ushort[x, y];
            for (var i = 0; i < x; i++)
                for (var j = 0; j < y; j++)
                    result[i, j] = (ushort)data[i, j];
            return result;
        }

        public static void SaveDutSN(string name, string sN)
        {
            //string path = "C:\\Users\\Administrator.DESKTOP-KDKC337\\Desktop\\";
            //string file = $"{path}{name}";
            FileStream fs = new FileStream(name, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

            sw.WriteLine(sN);

            sw.Flush();
            sw.Close();
            fs.Close();
        }

        public static void SaveDutSN(string sn, int row, int col)
        {
            string head = string.Format(@"{0},{1},{2},{3}", "Row", "Column", "DutSn","Reslut");
            string text = string.Format(@"{0},{1},{2},{3}", row, col, sn, IsMatchDutSN);
            Log4netMgr.Instance.SaveSCVForFullPath(SelectSNSavePath, text, head);
        }
    }

    public enum RunMode
    {
        Manual = 0,
        AutoNormal,
        AutoSelectSn,
        AutoSelectBin,
        AutoGRR,
        AutoAudit,
        AutoMark,
        DryRun,
        DoeSlip,
        DoeSameTray,
        DoeDifferentTray,
        DoeTakeOff,
        DoeSocketUniformityTest,
        DoeSingleDebug,
    }

    /// <summary>
    /// 定制模式，可组合
    /// </summary>
    [Flags]
    public enum CustomMode
    {
        None = 0,
        NoSn = 1 << 0,
        AllBinOk = 1 << 1
    }

    public class AutoSelectSnParam
    {
        public int LoadNum { get; set; }
        public int LoadTaryID { get; set; }
        public int[,] LoadRegion { get; set; }
        public List<string> DutSnList { get; set; }
    }

    public class AutoSelectBinParam
    {
        public int LoadNum { get; set; }
        public int LoadTaryID { get; set; }
        public int[,] LoadRegion { get; set; }
        public int UnloadNum { get; set; }
        public int UnloadTrayID { get; set; }
        public int[,] UnloadRegion { get; set; }
    }

    public class GRRParam
    {
        public int TrayID { get; set; }
        public int DutCount { get; set; }
        public int TestTimes { get; set; }
        public int TestRow { get; set; }
        public int TestCol { get; set; }
        public int [,] Region { get; set; }
    }

    public class SlipParam
    {
        public ushort TestNumber { get; set; }
        public ushort TaryID { get; set; }
        public ushort TestRow { get; set; }
        public ushort TestCol { get; set; }
        public double[,] ATestPos { get; set; }
        public double[,] BTestPos { get; set; }
        public ushort TestNozzle { get; set; }
    }

    public class SameTrayParam
    {
        public int TaryID { get; set; }
        public int[,] TrayRegion { get; set; }
        public ushort StartRow { get; set; }
        public ushort StartCol { get; set; }
    }

    public class DifferentTrayParam
    {
        public int LoadNum { get; set; }
        public int LoadTaryID { get; set; }
        public int[,] LoadRegion { get; set; }
        public int UnloadNum { get; set; }
        public int UnloadTrayID { get; set; }
        public int[,] UnloadRegion { get; set; }
    }

    public class TakeOffParam
    {   
        public int TrayID { get; set; }
        public int TestRow { get; set; }
        public int TestCol { get; set; }
        public int TestTimes { get; set; }
        public bool IsCloseCap { get; set; }
        public int SocketID { get; set; }
    }

    public class AutoMark
    {
        public int MarkMode { get; set; }
        public int MarkTrayChoose { get; set; }
        public int MarkTrayId { get; set; }
    }

    public class SocketUniformityTest
    {
        public int TrayID { get; set; }
        public int TestRow { get; set; }
        public int TestCol { get; set; }
        public int TestTimes { get; set; }
        public bool ContinueTest { get; set; }
        public int SocketID { get; set; }
        public int StationID { get; set; }
    }
}
