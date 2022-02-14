using System.Collections.Generic;
using System.Windows.Forms;
using AlcUtility;
using System.Runtime.InteropServices;
using System;
using Poc2Auto.GUI.UCModeUI.UCAxisesCylinders;
using Poc2Auto.Common;
using System.Threading;

namespace DragonFlex.GUI.Factory
{
    public partial class UCModeManual : UserControl
    {
        public UCModeManual()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;

            //权限管理
            authorityManagement();
            AlcSystem.Instance.UserAuthorityChanged += (o, n) => { authorityManagement(); };
        }

        private IPlcDriver _plcDriver;
        UC_Cylinders cylinders = new UC_Cylinders();
        private int _cylinderCount;
        public int CYL_COUNT { get => _cylinderCount; set => _cylinderCount = value; }
        List<UC_SingleAxisOperation> singleAxises = new List<UC_SingleAxisOperation>();
        private int _axisCount;
        private ParamsConfig config;
        public int AXIS_COUNT { get => _axisCount; set { _axisCount = value; } }
        public string DioPath { get; set; }
        public string RecipePath { get; set; }
        public string SemiAutoPath { get; set; }
        private string _defaultRecipe;
        private bool _isStop = false;
        public string DefaultRecipe
        {
            get { return _defaultRecipe; }
            set 
            {
                if (value == null)
                    return;
                _defaultRecipe = value;
                config = ParamsConfig.Upload(value);
            }
        }
        //配方保存后触发
        public Action<string> RecipeSave;
        //Tester PlcDriver
        private IPlcDriver TesterPlcDriver;
        private IPlcDriver HandlerPlcDriver;

        /// <summary>
        /// 是否显示推拉Pin针两个按钮
        /// </summary>
        //public bool IsShowTesterPinControlButton
        //{
        //    set
        //    {
        //        btnPullPin.Visible = value;
        //        btnReleasePin.Visible = value;
        //    }
        //}

        void init()
        {
            uC_Cylinders1.CYL_COUNT = _cylinderCount;
            uC_Cylinders1.IsInnerUpdatingOpen = false;

            for (var i = 0; i < _axisCount; i++)
            {
                var axis = new UC_SingleAxisOperation
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(20),
                    BorderStyle = BorderStyle.FixedSingle,
                    Size = new System.Drawing.Size(450, 500),
                };
                axis.IsInnerUpdatingOpen = false;
                flowPanelAxis.Controls.Add(axis);
                singleAxises.Add(axis);
            }

            ucRecipe_New1.DefaultRecipe = _defaultRecipe;
            ucRecipe_New1.FilePath = RecipePath;
            ucRecipe_New1.RecipeSave += (name) => 
            { 
                RecipeSave?.Invoke(name);
                UpdatePos();
            };

            tabControl3.SelectedIndexChanged += (e, sender) => { TabChanged(tabControl3.SelectedIndex); };
        }


        private void authorityManagement()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(authorityManagement));
                return;
            }

            if (AlcSystem.Instance.GetUserAuthority() != UserAuthority.OPERATOR.ToString() && AlcSystem.Instance.GetSystemStatus() == SYSTEM_STATUS.Idle)
            {
                ucRecipe_New1.AuthorityCtrl = true;
            }
            else
            {
                ucRecipe_New1.AuthorityCtrl = false;
            }
        }

        public void BindData(IPlcDriver plcDriver)
        {
            //气缸的plcDriver
            uC_Cylinders1.PlcDriver = plcDriver;

            //跳过读取Tester模块中的第二根轴(OpenSocket 轴)
            if (plcDriver.Name == ModuleTypes.Tester.ToString())
            {
                int j = 0;
                for (int i = 0; i < _axisCount; i++)
                {
                    if (i==1)
                    {
                        j = i;
                        singleAxises[i].DataSource = plcDriver.GetSingleAxisCtrl(j + 2);
                        singleAxises[i].AxisName = plcDriver.GetSingleAxisCtrl(j + 2).Info.Name;
                        singleAxises[i].Config = config;
                        continue;
                    }
                    singleAxises[i].DataSource = plcDriver.GetSingleAxisCtrl(i + j + 1);
                    singleAxises[i].AxisName = plcDriver.GetSingleAxisCtrl(i + j + 1).Info.Name;
                    singleAxises[i].Config = config;
                }
            }
            else
            {
                //正常读取Handler模块中的每一根轴
                for (int i = 0; i < _axisCount; i++)
                {
                    singleAxises[i].DataSource = plcDriver.GetSingleAxisCtrl(i + 1);
                    singleAxises[i].AxisName = plcDriver.GetSingleAxisCtrl(i + 1).Info.Name;
                    singleAxises[i].Config = config;
                }
            }

            uC_DIOs1.BindData(plcDriver, typeof(PLCDIO), DioPath, "EL2889s", "EL1889s");
            

            ucRecipe_New1.PlcDriver = plcDriver;
            _plcDriver = plcDriver;
            if (_plcDriver.Name == ModuleTypes.Tester.ToString())
                TesterPlcDriver = _plcDriver;
            else if (_plcDriver.Name == ModuleTypes.Handler.ToString())
                HandlerPlcDriver = _plcDriver;

            Thread thread = new Thread(ReadPLCData);
            thread.IsBackground = true;
            thread.Start();
        }

        private void TabChanged(int index)
        {
            uC_DIOs1.IsInnerUpdatingOpen = index == 0 ? true : false;
            foreach (var axis in singleAxises)
                axis.IsInnerUpdatingOpen = index == 1 ? true : false;
            uC_Cylinders1.IsInnerUpdatingOpen = index == 2 ? true : false;
        }

        //PLC模式改变时使能相应控制界面按钮
        public void UiEnable(bool enable)
        {
            if (_plcDriver == null)
                return;

            uC_DIOs1.AuthorityCtrl = enable;
            foreach (var axis in singleAxises)
                axis.AuthorityCtrl = enable;
            
            uC_Cylinders1.AuthorityCtrl = enable;
        }

        private void UCModeManual_Load(object sender, EventArgs e)
        {
            init();
        }

        #region Tester相关控制
        private void btnPullPin_Click(object sender, EventArgs e)
        {
            TesterPlcDriver?.WriteObject(RunModeMgr.Name_TesterScoketPullCylider, true);
        }

        private void btnReleasePin_Click(object sender, EventArgs e)
        {
            TesterPlcDriver?.WriteObject(RunModeMgr.Name_TesterScoketReleaseCylider, true);
        }

        #endregion Tester相关控制

        //读取PLC变量
        private void ReadPLCData()
        {
            try
            {
                while (true)
                {
                    if (TesterPlcDriver != null && TesterPlcDriver.IsInitOk && TesterPlcDriver.IsConnected)
                    {
                        var Result = TesterPlcDriver?.ReadObject(RunModeMgr.Name_SocketID, typeof(ushort));
                        if (null != Result)
                        {
                            RunModeMgr.SocketID = (ushort)Result;
                        }

                        Result = TesterPlcDriver?.ReadObject(RunModeMgr.Name_AxisAllHomed, typeof(bool));
                        if (null != Result)
                        {
                            RunModeMgr.TesterAllAxisHomed = (bool)Result;
                        }

                        Result = TesterPlcDriver?.ReadObject(RunModeMgr.Name_CurrentState, typeof(uint));
                        if (null != Result)
                        {
                            RunModeMgr.TesterCurrentState = (uint)Result;
                        }

                        //RunModeMgr.SocketSafetySignal = (bool)TesterPlcDriver?.ReadObject(RunModeMgr.Name_SocketSafetySignal, typeof(bool));
                        Result = TesterPlcDriver?.ReadObject(RunModeMgr.Name_ActiveEventClass, typeof(uint));
                        if (null != Result)
                        {
                            RunModeMgr.EventClass = (uint)Result;
                        }

                        Result = TesterPlcDriver?.ReadObject(RunModeMgr.Name_nMode, typeof(uint));

                        if (null != Result)
                        {
                            RunModeMgr.TesterMode = (uint)Result;
                        }

                        Result = TesterPlcDriver?.ReadObject(RunModeMgr.Name_nAutoSubMode, typeof(uint));

                        if (null != Result)
                        {
                            RunModeMgr.TesterSubMode = (uint)Result;
                        }
                    }

                    if (HandlerPlcDriver != null && HandlerPlcDriver.IsInitOk && HandlerPlcDriver.IsConnected)
                    {
                        var Result = HandlerPlcDriver?.ReadObject(RunModeMgr.Name_CompleteFlag, typeof(bool));
                        if (null != Result)
                        {
                            RunModeMgr.CompleteFlag = (bool)Result;
                        }
                        Result = HandlerPlcDriver?.ReadObject(RunModeMgr.Name_AxisAllHomed, typeof(bool));
                        if (null != Result)
                        {
                            RunModeMgr.HandlerAllAxisHomed = (bool)Result;
                        }
                        Result = HandlerPlcDriver?.ReadObject(RunModeMgr.Name_CurrentState, typeof(uint));
                        if (null != Result)
                        {
                            RunModeMgr.HandlerCurrentState = (uint)Result;
                        }
                        Result = HandlerPlcDriver?.ReadObject(RunModeMgr.MachineLight(1), typeof(bool));
                        if (null != Result)
                        {
                            RunModeMgr.LightControl = (bool)Result;
                        }
                        Result = HandlerPlcDriver?.ReadObject(RunModeMgr.MachineLight(2), typeof(bool));
                        if (null != Result)
                        {
                            RunModeMgr.MaintenanceLamp = (bool)Result;
                        }
                        Result = HandlerPlcDriver?.ReadObject(RunModeMgr.Name_IonFanContrl, typeof(bool));

                        if (null != Result)
                        {
                            RunModeMgr.IonFanContrl = (bool)Result;
                        }
                        Result = HandlerPlcDriver?.ReadObject(RunModeMgr.EL1889Struct(3, 11), typeof(bool));
                        if (null != Result)
                        {
                            RunModeMgr.LoadVacuum = (bool)Result;
                        }
                        Result = HandlerPlcDriver?.ReadObject(RunModeMgr.EL1889Struct(3, 12), typeof(bool));
                        if (null != Result)
                        {
                            RunModeMgr.UnloadVacuum = (bool)Result;
                        }
                        Result = HandlerPlcDriver?.ReadObject(RunModeMgr.Name_ActiveEventClass, typeof(uint));

                        if (null != Result)
                        {
                            RunModeMgr.EventClass = (uint)Result;
                        }
                        Result = HandlerPlcDriver?.ReadObject(RunModeMgr.Name_nMode, typeof(uint));
                        if (null != Result)
                        {
                            RunModeMgr.HandlerMode = (uint)Result;
                        }
                        Result = HandlerPlcDriver?.ReadObject(RunModeMgr.Name_nAutoSubMode, typeof(uint));
                        if (null != Result)
                        {
                            RunModeMgr.HandlerSubMode = (uint)Result;
                        }
                    }
                    UiEnable(AlcSystem.Instance.GetUserAuthority() == UserAuthority.ENGINEER.ToString() && AlcSystem.Instance.GetSystemStatus() ==  SYSTEM_STATUS.Idle);
                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void UpdatePos()
        {
            string file = string.Empty;
            if (this._plcDriver.Name == ModuleTypes.Handler.ToString())
            {
                file =$"{Application.StartupPath}\\paramFiles\\HandlerConfigFile\\" + ConfigMgr.Instance.HandlerDefaultRecipe;
            }
            else if (this._plcDriver.Name == ModuleTypes.Tester.ToString())
            {
                file = $"{Application.StartupPath}\\paramFiles\\RotationConfigFile\\" + ConfigMgr.Instance.TesterDefaultRecipe;
            }
            config = ParamsConfig.Upload(file);
            for (int i = 0; i < _axisCount; i++)
            {
                singleAxises[i].Config = config;
            }
        }
    }

    //DIO结构定义
    [StructLayout(LayoutKind.Sequential, Pack = 0)]
    public class PLCDIO
    {
        public const int EL1889S_ROW = 10;
        public const int EL1889S_COL = 16;

        public const int EL2889S_ROW = 10;
        public const int EL2889S_COL = 16;


        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = EL2889S_ROW * EL2889S_COL)]
        public bool[] el2889s;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.I1, SizeConst = EL1889S_ROW * EL1889S_COL)]
        public bool[] el1889s;


        public bool[,] EL1889s
        {
            get
            {
                if (el1889s == null)
                    return null;

                var datas = new bool[EL1889S_ROW, EL1889S_COL];
                for (int i = 0; i < EL1889S_ROW; i++)
                {
                    for (int j = 0; j < EL1889S_COL; j++)
                    {
                        var index = i * EL1889S_COL + j;
                        datas[i, j] = el1889s[index];
                    }
                }
                return datas;
            }
        }

        public bool[,] EL2889s
        {
            get
            {
                if (el2889s == null)
                    return null;

                var datas = new bool[EL2889S_ROW, EL2889S_COL];
                for (int i = 0; i < EL2889S_ROW; i++)
                {
                    for (int j = 0; j < EL2889S_COL; j++)
                    {
                        var index = i * EL2889S_COL + j;
                        datas[i, j] = el2889s[index];
                    }
                }
                return datas;
            }


            set
            {
                if (value != null && value.GetLength(0) == EL2889S_ROW && value.GetLength(1) == EL2889S_COL)
                {
                    for (int i = 0; i < EL2889S_ROW; i++)
                    {
                        for (int j = 0; j < EL2889S_COL; j++)
                        {
                            var index = i * EL2889S_COL + j;

                            el2889s[index] = value[i, j];
                        }
                    }
                }
            }

        }
    }
}
