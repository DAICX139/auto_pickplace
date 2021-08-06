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

        /// <summary>
        /// 是否显示半自动控制Tab页
        /// </summary>
        public bool IsShowSemiAutoTab 
        {
            set
            {
                if (!value)
                    tabControl3.TabPages.Remove(tabPage2);
                else
                {
                    if (!tabControl3.TabPages.Contains(tabPage2))
                        tabControl3.TabPages.Add(tabPage2);
                }    
            }
        }

        /// <summary>
        /// 是否显示Tester调试Tab页
        /// </summary>
        public bool IsShowDebugTab
        {
            set
            {
                if (!value)
                    tabControl3.TabPages.Remove(tabPageTurntableDebug);
                else
                {
                    if (!tabControl3.TabPages.Contains(tabPageTurntableDebug))
                        tabControl3.TabPages.Add(tabPageTurntableDebug);
                }
            }
        }

        /// <summary>
        /// 是否显示推拉Pin针两个按钮
        /// </summary>
        public bool IsShowTesterPinControlButton
        {
            set
            {
                btnPullPin.Visible = value;
                btnReleasePin.Visible = value;
            }
        }

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
            ucRecipe_New1.RecipeSave += (name) => { RecipeSave?.Invoke(name); };

            if (!CYGKit.GUI.Common.IsDesignMode())
            {
                authorityManagement();
                AlcSystem.Instance.UserAuthorityChanged += (o, n) => { authorityManagement(); };
            }
            tabControl3.SelectedIndexChanged += (e, sender) => { TabChanged(tabControl3.SelectedIndex); };
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
            
            //根据系统状态来开放控制权限
            plcDriver.OnInitOk += () => { authorityCtrl((int)plcDriver.GetSysInfoCtrl.Status.CurrentState); };
            plcDriver.OnStateChanged += (p) => { authorityCtrl(p); };

            ucRecipe_New1.PlcDriver = plcDriver;
            _plcDriver = plcDriver;
            if (_plcDriver.Name == ModuleTypes.Tester.ToString())
                TesterPlcDriver = _plcDriver;

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
            if (index == 4)
            {
                TesterPlcDriver.GetSysInfoCtrl.ModeCtrl(RunModeMgr.Doe);
                TesterPlcDriver.GetSysInfoCtrl.SubModeCtrl(RunModeMgr.Doe, RunModeMgr.Doe_TesterDebug);
            }
        }

        private void authorityManagement()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(authorityManagement));
                return;
            }
            uiEnable(!(AlcSystem.Instance.GetUserAuthority() == UserAuthority.OPERATOR.ToString()));
        }
        private void authorityCtrl(int state)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(authorityCtrl), state);
                return;
            }
            uiEnable(!(AlcSystem.Instance.GetUserAuthority() == UserAuthority.OPERATOR.ToString()));
        }

        //权限改变时使能相应控制界面按钮
        private void uiEnable(bool enable)
        {
            uC_DIOs1.AuthorityCtrl = enable;
            cylinders.AuthorityCtrl = enable;
            foreach (var axis in singleAxises)
                axis.AuthorityCtrl = enable;
            ucRecipe_New1.AuthorityCtrl = enable;
            uC_ModeSemiAuto1.AuthorityCtrl = enable;
        }

        //PLC模式改变时使能相应控制界面按钮
        public void UiEnable(PlcMode mode)
        {
            if (_plcDriver == null)
                return;
            if (mode == PlcMode.ManualMode && 
                AlcSystem.Instance.GetUserAuthority() == UserAuthority.ENGINEER.ToString() )
            {
                cylinders.AuthorityCtrl = true;

                foreach (var axis in singleAxises)
                    axis.AuthorityCtrl = true;
                saveLocationToolStripMenuItem.Enabled = true;
                axisOperateToolStrip.Enabled = true;
            }
            else
            {
                cylinders.AuthorityCtrl = false;

                foreach (var axis in singleAxises)
                    axis.AuthorityCtrl = false;
                saveLocationToolStripMenuItem.Enabled = false;
                axisOperateToolStrip.Enabled = false;
            }
        }

        private void UCModeManual_Load(object sender, EventArgs e)
        {
            init();
        }

        #region Tester相关控制

        private void btnOpenSocket_Click(object sender, EventArgs e)
        {
            TesterPlcDriver?.WriteObject(RunModeMgr.Name_DebugOpenSocket, true);
        }

        private void btnSocketClose_Click(object sender, EventArgs e)
        {
            TesterPlcDriver?.WriteObject(RunModeMgr.Name_DebugCloseSocket, true);
        }

        private void btnZAxisUp_Click(object sender, EventArgs e)
        {
            TesterPlcDriver?.WriteObject(RunModeMgr.Name_DebugZAxisUp, true);
        }

        private void btnZAxisDown_Click(object sender, EventArgs e)
        {
            TesterPlcDriver?.WriteObject(RunModeMgr.Name_DebugZAxisDown, true);
        }

        private void btnRotate_Click(object sender, EventArgs e)
        {
            TesterPlcDriver.WriteObject(RunModeMgr.Name_DryRunChoose, true);
            TesterPlcDriver.WriteObject(RunModeMgr.Name_DryRunTesterRotation, true);
        }

        private void btnPushPutter_Click(object sender, EventArgs e)
        {
            TesterPlcDriver?.WriteObject(RunModeMgr.Name_DebugPushPutter, true);
        }

        private void btnPullPutter_Click(object sender, EventArgs e)
        {
            TesterPlcDriver?.WriteObject(RunModeMgr.Name_DebugPullPutter, true);
        }

        private void btnPullPin_Click(object sender, EventArgs e)
        {
            TesterPlcDriver?.WriteObject(RunModeMgr.Name_TesterScoketPullCylider, true);
        }

        private void btnReleasePin_Click(object sender, EventArgs e)
        {
            TesterPlcDriver?.WriteObject(RunModeMgr.Name_TesterScoketReleaseCylider, true);
        }

        #endregion Tester相关控制

        //读取PLC当前Socket ID
        private void ReadPLCData()
        {
            while (true)
            {
                if (TesterPlcDriver != null && TesterPlcDriver.IsInitOk && TesterPlcDriver.IsConnected)
                {
                    RunModeMgr.SocketID = (ushort)TesterPlcDriver?.ReadObject(RunModeMgr.Name_SocketID, typeof(ushort));
                    lbSocketID.Text = RunModeMgr.SocketID.ToString();
                }

                Thread.Sleep(500);
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
