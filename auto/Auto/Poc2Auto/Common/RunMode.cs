using NetAndEvent.PlcDriver;
using System.Collections.Generic;
using CYGKit.AdsProtocol;
using Poc2Auto.Model;
using System;
using Poc2Auto.Database;
using LogLib.Managers;
using System.IO;
using System.Text;
using AlcUtility;

namespace Poc2Auto.Common
{
    public static class RunModeMgr
    {
        private static RunMode _runMode;
        public static event Action RunModeChanged;
        public static Action YieldDutChanged;
        public static Action FailDutChanged;
        public static Action CurrentSocketIDChanged;
        public static Action TesterStateChanged;
        public static Action HandlerStateChanged;
        public static Action<bool> LoadVacuumChanged;
        public static Action<bool> UnloadVacuumChanged;
        public static Action<uint> EventClassChanged;
        public static Action<bool> CompleteFlagChanged;
        public static Action LoadTraySizeChanged;
        public static Action NGTraySizeChanged;
        public static Action UnloadTraySizeChanged;

        private static CustomMode _customMode;
        public static event Action CustomModeChanged;
        private static AdsDriverClient testerClient = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Tester.ToString()) as AdsDriverClient;
        private static AdsDriverClient HandlerClient = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Handler.ToString()) as AdsDriverClient;

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

        public static SocketStatus SocketStatus { set; get; }

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
        private static bool _CompleteFlag;

        public static bool CompleteFlag
        {
            get { return _CompleteFlag; }
            set
            {
                if (value == _CompleteFlag)
                    return;
                _CompleteFlag = value;
                CompleteFlagChanged?.Invoke(value);
            }
        }

        private static bool _handlerAllAxisHomed;

        public static bool HandlerAllAxisHomed
        {
            get { return _handlerAllAxisHomed; }
            set { _handlerAllAxisHomed = value; }
        }

        private static bool _testerAllAxisHomed;

        public static bool TesterAllAxisHomed
        {
            get { return _testerAllAxisHomed; }
            set { _testerAllAxisHomed = value; }
        }

        private static uint _testerCurrentState;

        public static uint TesterCurrentState
        {
            get { return _testerCurrentState; }
            set
            {
                if (value == _testerCurrentState) return;
                _testerCurrentState = value;
                TesterStateChanged?.Invoke();
            }
        }

        private static uint _handlerCurrentState;

        public static uint HandlerCurrentState
        {
            get { return _handlerCurrentState; }
            set
            {
                if (value == _handlerCurrentState) return;
                _handlerCurrentState = value;
                TesterStateChanged?.Invoke();
            }
        }

        private static bool _lightControl;

        /// <summary>
        /// 普通照明灯
        /// </summary>
        public static bool LightControl
        {
            get { return _lightControl; }
            set { _lightControl = value; }
        }

        private static bool _maintenanceLamp;

        /// <summary>
        /// 检修照明灯
        /// </summary>
        public static bool MaintenanceLamp
        {
            get { return _maintenanceLamp; }
            set { _maintenanceLamp = value; }
        }

        public static string IsMatchDutSN;

        private static bool _lampYellow;

        /// <summary>
        /// 三色灯-黄色
        /// </summary>
        public static bool LampYellow
        {
            get { return _lampYellow; }
            set
            {
                _lampYellow = value;
                HandlerClient?.WriteObject(Name_TricolorLampYellow, value);
            }
        }

        private static bool _lampGreen;

        /// <summary>
        /// 三色灯-绿色
        /// </summary>
        public static bool LampGreen
        {
            get { return _lampGreen; }
            set
            {
                _lampGreen = value;
                HandlerClient?.WriteObject(Name_TricolorLampGreen, value);
            }
        }

        private static bool _lampRed;

        /// <summary>
        /// 三色灯-红色
        /// </summary>
        public static bool LampRed
        {
            get { return _lampRed; }
            set
            {
                _lampRed = value;
                HandlerClient?.WriteObject(Name_TricolorLampRed, value);
            }
        }

        private static bool _buzzer;
        /// <summary>
        /// 蜂鸣器控制
        /// </summary>
        public static bool Buzzer
        {
            get { return _buzzer; }
            set
            {
                _buzzer = value;
                HandlerClient?.WriteObject(Name_Buzzer, value);
            }
        }

        private static bool _socketSafetySignal;
        /// <summary>
        /// Socket 安全信号状态
        /// </summary>
        public static bool SocketSafetySignal
        {
            get { return _socketSafetySignal; }
            set { _socketSafetySignal = value; }
        }

        /// <summary>
        /// Socket安全检测信号控制
        /// </summary>
        public static bool SetSocketSignal
        {
            set
            {
                testerClient?.WriteObject(Name_SocketSafetySignal, value);
            }
        }

        /// <summary>
        /// 当前运行模式
        /// </summary>
        public static RunMode LastMode { get; set; }

        private static bool _loadVacuum;

        public static bool LoadVacuum
        {
            get { return _loadVacuum; }
            set
            {
                if (value == _loadVacuum)
                    return;
                _loadVacuum = value;
                LoadVacuumChanged?.Invoke(value);
            }
        }

        private static bool _unloadVacuum;

        public static bool UnloadVacuum
        {
            get { return _unloadVacuum; }
            set
            {

                if (value == _unloadVacuum)
                    return;
                _unloadVacuum = value;
                UnloadVacuumChanged?.Invoke(value);
            }
        }

        private static uint _eventClass;

        public static uint EventClass
        {
            get { return _eventClass; }
            set
            {
                _eventClass = value;
                EventClassChanged?.Invoke(value);
            }
        }
        private static bool _isLoadBigTray;

        public static bool IsLoadBigTray
        {
            get { return _isLoadBigTray; }
            set
            {
                //if (value == _isBigTray)
                //    return;
                _isLoadBigTray = value;
                LoadTraySizeChanged?.Invoke();
            }
        }

        private static bool _isNGBigTray = true;

        public static bool IsNGBigTray
        {
            get { return _isNGBigTray; }
            set
            {
                _isNGBigTray = value;
                NGTraySizeChanged?.Invoke();
            }
        }

        private static bool _isUnloadBigTray = true;

        public static bool IsUnloadBigTray
        {
            get { return _isUnloadBigTray; }
            set
            {
                _isUnloadBigTray = value;
                UnloadTraySizeChanged?.Invoke();
            }
        }

        private static bool _ionFanContrl;

        public static bool IonFanContrl
        {
            get { return _ionFanContrl; }
            set { _ionFanContrl = value; }
        }

        private static uint _testerSubMode;

        public static uint TesterSubMode
        {
            get { return _testerSubMode; }
            set { _testerSubMode = value; }
        }

        private static uint _handlerSubMode;

        public static uint HandlerSubMode
        {
            get { return _handlerSubMode; }
            set { _handlerSubMode = value; }
        }

        private static uint _testerMode;

        public static uint TesterMode
        {
            get { return _testerMode; ; }
            set { _testerMode = value; }
        }

        private static uint _handlerMode;

        public static uint HandlerMode
        {
            get { return _handlerMode; }
            set { _handlerMode = value; }
        }

        private static bool _tMRetest;

        public static bool TMRetest
        {
            get { return _tMRetest; }
            set { _tMRetest = value; }
        }

        public static void Init()
        {
            RunMode = ConfigMgr.Instance.RunMode;
            if (RunMode == RunMode.AutoAudit)
            {
                string path;
                if (!File.Exists(ConfigMgr.Instance.AuditFile))
                {
                    path = AuditFile;
                }
                else
                {
                    path = ConfigMgr.Instance.AuditFile;
                }
                CSVToDic(path);
            }
            CustomMode = ConfigMgr.Instance.CustomMode;
            SocketStatus = ConfigMgr.Instance.SocketStatus;
        }

        #region PLCData

        //模式
        public const int Auto = 2;

        //功能码
        public const int Func_Reset = 1;                    //回原点
        public const int Func_AutoNormal = 2;               //生产流程
        public const int Func_AutoGoldenDut = 3;            //GRR流程
        public const int Func_AutoSelectDUT = 4;            //挑料流程
        public const int Func_AutoMark = 5;                 //Tray标定
        public const int Func_DoeSameTrayTest = 6;          //DOE-相同盘测试
        public const int Func_DoeDifferentTrayTest = 7;     //DOE-不同盘测试
        public const int Func_DoeSlipTest = 8;              //DOE-滑移测试
        public const int Func_DoeTakeoffTest = 9;           //DOE-TakeOff测试
        public const int Func_DryRun = 10;                  //DrayRun
        public const int Func_SemiAuto = 11;                //Handler 半自动(Tray取料、Tray放料、扫码、Socket取料、Socket放料) //Tester  半自动（转盘转动1个工位、Socket开盖、Socket关盖）
        //public const int Func_Script = 12;                  //脚本运行功能码
        public const int Func_Audit = 12;                   //Audit模式

        #region PLC变量名

        public readonly static string Name_SelectLoadtrayID = "GVL_MachineCom.SelectLoadtrayID";
        public readonly static string Name_SelectUnLoadtrayID = "GVL_MachineCom.SelectUnLoadtrayID";
        public readonly static string Name_SelectDUTnumber = "GVL_MachineCom.SelectDUTnumber";
        public readonly static string Name_SelectLoadBinID = "GVL_MachineCom.SelectLoadBinID";
        public readonly static string Name_SelectUnLoadBinID = "GVL_MachineCom.SelectUnLoadBinID";
        //挑SN和单独扫码模式切换变量
        public readonly static string Name_SelectSnMode = "GVL_MachineCom.SelectSnMode";

        //Old GRR params
        //public readonly static string Name_GRRTrayID = "GVL_MachineCom.GoldenDutLoadtrayID";
        //public readonly static string Name_GRRDutCount = "GVL_MachineCom.GoldenDutNumber";

        public readonly static string Name_GRRTrayID = "GVL_MachineCom.GoldenDutLoadtrayID";
        public readonly static string Name_GRRDutNumber = "GVL_MachineCom.GRRDutNumber";
        public readonly static string Name_GRRTestRow = "GVL_MachineCom.GRRRow";
        public readonly static string Name_GRRTestCol = "GVL_MachineCom.GRRCol";
        //New GRR params
        public readonly static string Name_GRRTestNumbert = "GVL_MachineCom.GRRTMTestNumber"; //GRR 模式下测试的次数
        public readonly static string Name_GRRCoute = "GVL_MachineCom.GRRCoute"; //PLC GRR模式下已经执行的次数

        public readonly static string Name_SlipTesttrayID = "GVL_MachineCom.SlipTesttrayID";
        public readonly static string Name_SlipTestSuction = "GVL_MachineCom.SlipTestSuction";
        public readonly static string Name_SlipTestRow = "GVL_MachineCom.SlipTestRow";
        public readonly static string Name_SlipTestCol = "GVL_MachineCom.SlipTestCol";
        public readonly static string Name_SlipTestApos = "GVL_MachineCom.ATestPosition";
        public readonly static string Name_SlipTestBpos = "GVL_MachineCom.BTestPosition";
        public readonly static string Name_SlipTestNumber = "GVL_MachineCom.SlipTestNumber";
        //滑移测试已执行的次数
        public readonly static string Name_SlipTestExecutionTimes = "GVL_MachineCom.SlipTestCoute";

        public readonly static string Name_SameTrayID = "GVL_MachineCom.SameTrayLoadtrayID";
        public readonly static string Name_SameTrayBinID = "GVL_MachineCom.SameTrayLoadBinID";
        public readonly static string Name_SameTrayStartRow = "GVL_MachineCom.SameTrayStartRow";
        public readonly static string Name_SameTrayStartCol = "GVL_MachineCom.SameTrayStartCol";

        public readonly static string Name_DifferenttTrayLoadTrayID = "GVL_MachineCom.DifferentTrayLoadtrayID";
        public readonly static string Name_DifferenttTrayUnLoadTrayID = "GVL_MachineCom.DifferentTrayUnLoadtrayID";

        public readonly static string Name_TakeOffTestTrayID = "GVL_MachineCom.Take_offTestTrayID";
        public readonly static string Name_TakeOffTestRow = "GVL_MachineCom.Take_offTestRow";
        public readonly static string Name_TakeOffTestCol = "GVL_MachineCom.Take_offTestCol";
        public readonly static string Name_TakeOffTestTimes = "GVL_MachineCom.Take_offTestNumber";
        public readonly static string Name_TakeOffIsCloseCap = "GVL_MachineCom.Take_offTestChoose";
        public readonly static string Name_Take_offSocketID = "GVL_MachineCom.Take_offSocketID";

        public readonly static string Name_MarkMode = "GVL_MachineCom.MarkMode";
        public readonly static string Name_MarkTrayChoose = "GVL_MachineCom.MarkTrayChoose";
        public readonly static string Name_MarkTrayID = "GVL_MachineCom.MarkTrayID";

        public readonly static string Name_EnableBuzzer = "GVL_MachineDevice.bSilencer";

        //Handler 半自动用的变量
        public readonly static string Name_Nozzle1TrayLoad = "GVL_MachineCom.Nozzle1TrayLoad";
        public readonly static string Name_Nozzle1Scan = "GVL_MachineCom.Nozzle1Scan";
        public readonly static string Name_Nozzle1SocketPut = "GVL_MachineCom.Nozzle1SocketPut";
        public readonly static string Name_Nozzle2SocketPick = "GVL_MachineCom.Nozzle2SocketPick";
        public readonly static string Name_Nozzle1TrayUload = "GVL_MachineCom.Nozzle1TrayUload";
        public readonly static string Name_Nozzle2TrayUload = "GVL_MachineCom.Nozzle2TrayUload";

        //Tester 半自动用的变量
        public readonly static string Name_OpenSocket = "GVL_MachineCom.bOpenSocket";
        public readonly static string Name_CloseSocket = "GVL_MachineCom.bCloseSocket";
        public readonly static string Name_TesterRotation = "GVL_MachineCom.bRotateTurntable";
        public readonly static string Name_SocketID = "GVL_MachineCom.nSocketID";
        public readonly static string Name_TurntableCyliderWork = "GVL_MachineDevice.TurntableCyliderWork";  //转盘气缸顶升
        public readonly static string Name_TurntableCyliderBase = "GVL_MachineDevice.TurntableCyliderBase";  //转盘气缸收回

        //Socket一致性测试PLC变量
        public readonly static string Name_TMContinueTest = "GVL_MachineCom.TMContinueTest";
        public readonly static string Name_TMTestSocketID = "GVL_MachineCom.TMTestSocketID";
        public readonly static string Name_TMTestStationID = "GVL_MachineCom.TMTestStationID";
        public readonly static string Name_TMTestTrayID = "GVL_MachineCom.TMTestTrayID";
        public readonly static string Name_TMTestRow = "GVL_MachineCom.TMTestRow";
        public readonly static string Name_TMTestCol = "GVL_MachineCom.TMTestCol";
        public readonly static string Name_TMTestTimes = "GVL_MachineCom.TMTestNumber";

        //Tester变量：控制手动推拉pin针气缸
        public readonly static string Name_TesterScoketPullCylider = "GVL_MachineCom.bAutoPullCylider";
        public readonly static string Name_TesterScoketReleaseCylider = "GVL_MachineCom.bAutoReleaseCylider";

        //清料标志,表示正在清料中
        public readonly static string Name_CompleteFlag = "GVL_MachineCom.CompelteFlag";
        //清料完成
        public readonly static string Name_CompleteFinish = "GVL_MachineCom.CompelteFalish";
        //GRR测试完成
        public readonly static string Name_GRRFinish = "GVL_MachineCom.GRRFalish";
        //Audit测试完成
        public readonly static string Name_AuditFinish = "GVL_MachineCom.AuditFinish";
        //Socket禁用
        public readonly static string Name_DisableSocket = "GVL_MachineCom.DisableSocket";

        //系统命令，报警清除
        public readonly static string Name_SysCmdClearAlarm = "GVL_MachineInterface.MachineCmd.bClearAlarm";
        // 清除活跃事件
        public readonly static string Name_SysCmdClearActiveEvent = "GVL_MachineEvent.ActiveEvent.bClearEvent";

        //所有轴回原标志
        public readonly static string Name_AxisAllHomed = "GVL_MachineDevice.bAllAxisHome";
        //PLC当前状态
        public readonly static string Name_CurrentState = "GVL_MachineInterface.MachineStatus.nCurrentState";
        //PLC系统命令
        public readonly static string Name_StateCmd = "GVL_MachineInterface.MachineCmd.nStateCmd";
        //空跑模式切换
        public readonly static string Name_DryRunComad = "GVL_MachineDevice.DryRunComad";
        //检修灯控制
        public readonly static string Name_MachineLight = "GVL_MachineDevice.MachineLight";
        //三色灯：黄灯
        public readonly static string Name_TricolorLampYellow = "GVL_MachineDevice.LampContrl.YellowLight";
        //三色灯：绿灯
        public readonly static string Name_TricolorLampGreen = "GVL_MachineDevice.LampContrl.GreenLight";
        //三色灯：红灯
        public readonly static string Name_TricolorLampRed = "GVL_MachineDevice.LampContrl.RedLight";
        //蜂鸣器
        public readonly static string Name_Buzzer = "GVL_MachineDevice.LampContrl.Buzzer";
        //视觉标定,True 不做轴运动限制
        public readonly static string Name_VisionCalibration = "GVL_MachineDevice.VisionCalibration";
        //TM报警后给PLC写报警变量，PLC做三色灯提醒
        public readonly static string Name_TMError = "GVL_MachineEvent.bTMError";
        //Socket下降到位信号
        public readonly static string Name_SocketSafetySignal = "GVL_MachineDevice.OutputCtrl[1]";
        //EL1889结构体
        public readonly static string Name_DigitalIO_LoadVacuum = "GVL_MachineDevice.DigitalIO_HW.EL1889[3,11]";
        public readonly static string Name_DigitalIO_UnloadVacuum = "GVL_MachineDevice.DigitalIO_HW.EL1889[3,12]";
        //重测勾选框  True：启用 False：不启用
        public readonly static string Name_Retest = "GVL_MachineCom.CheckResurvey";
        //报警等级
        public readonly static string Name_ActiveEventClass = "GVL_MachineEvent.ActiveEvent.nClass";
        //离子风扇控制
        public readonly static string Name_IonFanContrl = "GVL_MachineDevice.IonFanContrl";
        //门锁有效 False ：无效 ,True：有效
        public readonly static string Name_DoorLockControl = "GVL_MachineDevice.bDoorLockValid";

        public readonly static string Name_nMode = "GVL_MachineInterface.MachineCmd.nMode";

        public readonly static string Name_nAutoSubMode = "GVL_MachineInterface.MachineCmd.nAutoSubMode";

        //清除当前状态Socket状态
        public readonly static string Name_ClearSokcetStatus = "GVL_MachineCom.ClearSokcetStatus";
        //清除过程数据
        public readonly static string Name_ClearData = "GVL_MachineCom.ClearData";
        //PLC软件版本号
        public readonly static string Name_PlcProgramVer = "GVL_MachineCom.PlcProgramVer";

        // TrayInfo全路径名称
        public readonly static string Name_TrayInfo = "GVL_MachineDevice.stTrayInfo";

        // RegionNum相对于TrayInfo的路径名称
        public readonly static string Name_RegionNum = "nRegion_Num";

        public static string Name_RegionValue = "aRegion_Value";

        public readonly static string Name_TrayRanks = "stTrayData.aTray_Ranks";

        public readonly static string Name_RegionMax = "GVL_MachineDevice.nRegion_Max";


        #endregion PLC变量名

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
        /// <summary>
        /// Socket屏蔽控制变量
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string DisableSocket(int index)
        {
            return $"GVL_MachineCom.DisableSocket[{index}]";
        }
        /// <summary>
        /// 照明灯控制变量
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string MachineLight(int index)
        {
            return $"GVL_MachineDevice.MachineLight[{index}]";
        }
        /// <summary>
        /// 大小Tray切换变量: False：大Tray  True：小Tray
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string TraySwitch(int index)
        {
            return $"GVL_MachineDevice.stTraySwitch[{index}]";
        }
        /// <summary>
        /// 获取光源对应的IO变量名
        /// </summary>
        /// <param name="indx"></param>
        /// <returns></returns>
        public static string GetIOVarName(int indx)
        {
            return $"GVL_MachineDevice.OutputCtrl[{indx}]";
        }
        public static string EL1889Struct(int moduleId, int indexId)
        {
            return $"GVL_MachineDevice.DigitalIO_HW.EL1889[{moduleId},{indexId}]";
        }

        public static string TrayInfoRegionValue(int trayId)
        {
            return $"{Name_TrayInfo}[{trayId}].{Name_RegionValue}";
        }

        //CYG7953SA.CYG7953SA_Handler.GVL_MachineDevice.stTrayInfo[3].stTrayRegion

        public static string TrayInfoStTrayRegion(int trayId)
        {
            return $"{Name_TrayInfo}[{trayId}].{Name_TrayRanks}";
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
        //var iniPosX = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadX_RecipePos").KeyValues["LoadUpTakePhotosPosX"].Value);
        public static int TMTestTimeOut =>Convert.ToInt32(testerClient.CfgParamsConfig.GetParamsModule("ParameterSet").KeyValues["RequestTimeOut"].Value);
        public static bool IsOnlyScanMode;
        public static ushort SelectSNTray;
        public static ushort PickRow;
        public static ushort PickCol;
        public static string SelectSNSavePath;
        public static int GRRTestTimes;

        public static bool OriginValue = false;

        public static bool IsRotate = false;

        public static readonly string AuditFile = @"C:\data\audit.csv";

        public static bool Reset(AdsDriverClient client, out string message)
        {
            if (client == null)
            {
                message = "ALC与机台未连接成功!";
                return false;
            }
            var ret1 = client?.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client?.GetSysInfoCtrl.SubModeCtrl(Auto, Func_Reset);
            if (ret1 + ret2 != 0)
            {
                message = "复位功能切换失败";
                return false;
            }

            message = "OK";
            return true;
        }

        public static bool SemiAutoMode(AdsDriverClient client, out string message)
        {
            if (client == null)
            {
                message = "ALC与机台未连接成功!";
                return false;
            }
            var ret1 = client?.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client?.GetSysInfoCtrl.SubModeCtrl(Auto, Func_SemiAuto);
            if (ret1 + ret2 != 0)
            {
                message = "单步运行功能切换失败";
                return false;
            }

            message = "OK";
            return true;
        }

        public static bool AutoNormal(AdsDriverClient client, out string message)
        {
            if (testerClient == null || client == null)
            {
                message = "ALC与机台未连接成功!";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Func_AutoNormal);
            var ret3 = testerClient.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret4 = testerClient.GetSysInfoCtrl.SubModeCtrl(Auto, Func_AutoNormal);

            if (ret1 + ret2 + ret3 + ret4 != 0)
            {
                message = "生产模式切换失败!";
                return false;
            }

            message = "OK";
            return true;
        }

        public static bool AutoSelectSn(AdsDriverClient client, AutoSelectSnParam param, out string message)
        {

            var a = TMTestTimeOut;
            if (testerClient == null)
            {
                message = "ALC与机台未连接成功!";
                return false;
            }
            if (!IsOnlyScanMode)
            {
                if (param.LoadNum == 0)
                {
                    message = @"上料盘Load1数据为空，请重新选择!";
                    return false;
                }
                if (param.LoadNum < param.DutSnList.Count)
                {
                    message = @"上料盘Load1挑选产品个数小于SN个数，请重新选择!";
                    return false;
                }
            }

            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Func_AutoSelectDUT);

            if (ret1 + ret2 != 0)
            {
                message = "挑选SN功能切换失败!";
                return false;
            }

            var ret = client.WriteObject(Name_SelectSnMode, (ushort)(IsOnlyScanMode ? 2 : 1), out message);
            if (ret != 0) return false;
            if (IsOnlyScanMode)
            {
                var ret3 = client.WriteObject(Name_SelectLoadtrayID, SelectSNTray);
                if (ret3 != 0) return false;
            }
            else
            {
                int[,] load2Data = new int[Tray.ROW, Tray.COL];

                if (IsLoadBigTray)
                {
                    for (int i = 0; i < Tray.ROW; i++)
                    {
                        for (int j = 0; j < Tray.COL; j++)
                        {
                            if (i >= 2 && i <= 30 && j >= 2 && j <= 11)
                            {
                                load2Data[i, j] = 0;
                            }
                            else
                                load2Data[i, j] = 2;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Tray.S_ROW; i++)
                    {
                        for (int j = 0; j < Tray.S_Col; j++)
                        {
                            if (i >= 2 && i <= 18 && j >= 2 && j <= 6)
                            {
                                load2Data[i, j] = 0;
                            }
                            else
                                load2Data[i, j] = 2;
                        }
                    }
                }

                var ret3 = client.WriteObject(Name_SelectLoadtrayID, SelectSNTray);
                if (ret3 != 0) return false;

                ////清空数据库中上料2盘的数据
                //var result = client.WriteTrayData((int)TrayName.Load2, load2Data, out message);
                //if (!result) return false;
                //DragonDbHelper.ClearTrayData((int)TrayName.Load2);
                //TrayManager.Trays[TrayName.Load2].SetData(new int[Tray.ROW, Tray.COL]);

                //清空数据库中NG盘的数据
                var result = client.WriteTrayData((int)TrayName.NG, new int[Tray.ROW, Tray.COL], out message);
                if (!result) return false;
                DragonDbHelper.ClearTrayData((int)TrayName.NG);
                TrayManager.Trays[TrayName.NG].SetData(new int[Tray.ROW, Tray.COL]);

                Overall.SelectionList = new List<string>(param.DutSnList);
            }

            return true;
        }

        public static bool AutoSelectBin(AdsDriverClient client, AutoSelectBinParam param, out string message)
        {
            if (testerClient == null)
            {
                message = "ALC与机台未连接成功!";
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
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Func_AutoSelectDUT);

            if (ret1 + ret2 != 0)
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
                message = "ALC与机台未连接成功!";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Func_AutoGoldenDut);
            var ret3 = testerClient.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret4 = testerClient.GetSysInfoCtrl.SubModeCtrl(Auto, Func_AutoNormal);
            if (ret1 + ret2 + ret3 + ret4 != 0)
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
            var ret5 = client.WriteObject(Name_GRRTestNumbert, (ushort)param.TestTimes, out message);
            if (ret5 != 0) return false;
            var ret6 = client.WriteObject(Name_GRRCoute, (ushort)0, out message);
            if (ret6 != 0) return false;
            
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

        public static bool Audit(AdsDriverClient client, out string message)
        {
            client.GetSysInfoCtrl.ModeCtrl(Auto);
            client.GetSysInfoCtrl.SubModeCtrl(Auto, Func_Audit);

            Running = false;
            //TestTimes = param.TestTimes;
            LoadCount = 0;
            UnloadCount = 0;
            TestedSockets = new List<int>();

            message = "OK";
            return true;
        }

        public static bool SlipTest(AdsDriverClient client, SlipParam param, out string message)
        {
            if (testerClient == null)
            {
                message = "ALC与机台未连接成功!";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Func_DoeSlipTest);

            if (ret1 + ret2 != 0)
            {
                message = "滑移测试模式切换失败!";
                return false;
            }
            var result = client.WriteObject(Name_SlipTesttrayID , param.TaryID, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_SlipTestSuction, param.TestNozzle, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_SlipTestApos, param.ATestPos, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_SlipTestBpos, param.BTestPos, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_SlipTestNumber, param.TestNumber, out message);
            if (result != 0) return false;
            result = client.WriteObject(Name_SlipTestExecutionTimes, (ushort)0, out message);
            if (result != 0) return false;

            return true;
        }

        public static bool SameTrayTest(AdsDriverClient client, SameTrayParam param, out string message)
        {
            if (testerClient == null)
            {
                message = "ALC与机台未连接成功!";
                return false;
            }
            if (param.StartRow == 0 || param.StartCol == 0)
            {
                message = @"请选择Dut！";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Func_DoeSameTrayTest);

            if (ret1 + ret2 != 0)
            {
                message = "同Tray验证模式切换失败!";
                return false;
            }
            var result = client.WriteObject(Name_SameTrayID, (ushort)param.TaryID, out message);
            if (result != 0) return false;

            var sTrayData = param.TrayRegion;
            int[,] bTraydata = new int[Tray.ROW, Tray.COL];
            for (int i = 0; i < sTrayData.GetLength(0); i++)
            {
                for (int j = 0; j < sTrayData.GetLength(1); j++)
                {
                    bTraydata[i, j] = sTrayData[i, j];
                }
            }


            //写入选择的Tray数据
            var ret = client.WriteTrayData((ushort)param.TaryID, bTraydata, out message);
            if (!ret) return false;
            DragonDbHelper.ClearTrayData((int)(TrayName)param.TaryID);
            TrayManager.Trays[(TrayName)param.TaryID].SetData(bTraydata);

            return true;
        }

        public static bool DifferentTrayTest(AdsDriverClient client, DifferentTrayParam param, out string message)
        {
            if (testerClient == null)
            {
                message = "ALC与机台未连接成功!";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Func_DoeDifferentTrayTest);

            if (ret1 + ret2 != 0)
            {
                message = "不同Tray模式切换失败!";
                return false;
            }

            var result = client.WriteObject(Name_DifferenttTrayLoadTrayID, (ushort)param.LoadTaryID, out message);
            if (result != 0) return false;

            result = client.WriteObject(Name_DifferenttTrayUnLoadTrayID, (ushort)TrayName.NG, out message);
            if (result != 0) return false;

            var sTrayData = param.LoadRegion;
            int[,] bTraydata = new int[Tray.ROW, Tray.COL];
            for (int i = 0; i < sTrayData.GetLength(0); i++)
            {
                for (int j = 0; j < sTrayData.GetLength(1); j++)
                {
                    bTraydata[i, j] = sTrayData[i, j];
                }
            }

            var ret = client.WriteTrayData(param.LoadTaryID, bTraydata, out message);
            if (!ret) return false;
            DragonDbHelper.ClearTrayData((int)(TrayName)param.LoadTaryID);
            TrayManager.Trays[(TrayName)param.LoadTaryID].SetData(bTraydata);

            return true;
        }

        public static bool TakeOffTest(AdsDriverClient client, TakeOffParam param, out string message)
        {
            if (testerClient == null)
            {
                message = "ALC与机台未连接成功!";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Func_DoeTakeoffTest);
            var ret3 = testerClient.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret4 = testerClient.GetSysInfoCtrl.SubModeCtrl(Auto, Func_DoeTakeoffTest);
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
                message = "ALC与机台未连接成功!";
                return false;
            }
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Func_AutoMark);

            if (ret1 + ret2 != 0)
            {
                message = "自动标定模式切换失败";
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

        public static bool DryRun1(AdsDriverClient client, out string message)
        {
            var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Func_DryRun);
            var ret3 = 0;

            if (ret1 + ret2 + ret3 != 0)
            {
                message = "空跑功能切换失败";
                return false;
            }
            message = "ok";

            return true;
        }

        public static bool SockeTest(AdsDriverClient client, SocketUniformityTest param, out string message)
        {
            if (testerClient == null)
            {
                message = "ALC与机台未连接成功!";
                return false;
            }
            //Handler切换模式
            //var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            //var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Doe_SocketUniformityTest);
            //
            ////Tester切换模式
            //var ret3 = testerClient.GetSysInfoCtrl.ModeCtrl(Auto);
            //var ret4 = testerClient.GetSysInfoCtrl.SubModeCtrl(Auto, Doe_SocketUniformityTest);
            //if (ret1 + ret2 + ret3 + ret4 != 0)
            //{
            //    message = "Socket 一致性模式切换失败";
            //    return false;
            //}

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

        public static bool Script(AdsDriverClient client, out string message)
        {
            //var ret1 = client.GetSysInfoCtrl.ModeCtrl(Auto);
            //var ret2 = client.GetSysInfoCtrl.SubModeCtrl(Auto, Func_Script);
            //
            //if (client.Name == ModuleTypes.Handler.ToString())
            //{
            //    client.WriteObject(Name_DryRunComad, false);
            //}
            //
            //if (ret1 + ret2 != 0)
            //{
            //    message = "脚本功能切换失败";
            //    return false;
            //}
            message = "该功能还在完善中...";

            return false;
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
 
        public static void SaveDutSN(string sn, int row, int col)
        {
            string head = string.Format(@"{0},{1},{2},{3}", "Row", "Column", "DutSn","Reslut");
            string text = string.Format(@"{0},{1},{2},{3}", row, col, sn, IsMatchDutSN);
            Log4netMgr.Instance.SaveSCVForFullPath(SelectSNSavePath, text, head);
        }

        /// <summary>
        /// 将指定格式的CSV文件转换成对应字典 Path：C:\Users\86139\Desktop\audit1.csv
        /// </summary>
        /// <param name="filePath">CSV文件的全路径名（包含文件名）</param>
        public static void CSVToDic(string filePath)
        {
            try
            {
                Encoding encoding = Encoding.Default;
                string strLine = "";
                using (StreamReader reader = new StreamReader(filePath, encoding))
                {
                    while ((strLine = reader.ReadLine()) != null)
                    {
                       var data = strLine.Split(',');
                        if (data.Length >= 2)
                        {
                            if (double.TryParse(System.Text.RegularExpressions.Regex.Replace(data[1], @"[^0-9]+", ""), out double result))
                            {
                                Overall.AuditSN[data[0]] = result;
                            } 
                        }
                        
                    }
                     
                }

            }
            catch (Exception ex)
            {

                AlcSystem.Instance.ShowMsgBox($"{ex.Message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
        }

        public static int GetActualSocketID(int id)
        {
            int socetID = 1;
            switch (id)
            {
                case 2:
                    socetID = 1;
                    break;
                case 6:
                    socetID = 2;
                    break;
                case 5:
                    socetID = 3;
                    break;
                case 4:
                    socetID = 4;
                    break;
                case 3:
                    socetID = 5;
                    break;
                default:
                    break;
            }

            return socetID;
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
        HandlerSemiAuto,
        TesterSemiAuto,
        ResetMode,
        Script,
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
    
    /// <summary>
    /// 当前Socket状态枚举
    /// </summary>
    public enum SocketStatus
    {
        Idle,
        SocketOpened,
        SocketClosed,
        SocketClosing,
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
