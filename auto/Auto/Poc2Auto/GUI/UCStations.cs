using System.Windows.Forms;
using Poc2Auto.Model;
using Poc2Auto.Database;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using System;
using System.Threading;
using System.Drawing;
using AlcUtility;

namespace Poc2Auto.GUI
{
    public partial class UCStations : UserControl
    {
        AdsDriverClient Client = (AdsDriverClient)PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Handler.ToString());
        bool isError;
        public UCStations()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            //读取配置
            RunModeMgr.CustomModeChanged += ReadCfg;
            ReadCfg();

            //吸嘴信息
            //uC_LoadNozzle.BindDataBase<DragonContext, OnlineDut>();
            //uC_LoadNozzle.StationID = (int)StationName.Load;
            //uC_UnloadNozzle.BindDataBase<DragonContext, OnlineDut>();
            //uC_UnloadNozzle.StationID = (int)StationName.Unload;

            //UPH信息
            uphLabel1.BindDatabase<DragonContext, Product>();

            Thread thread = new Thread(UpdateState);
            thread.IsBackground = true;
            thread.Start();

            //运行模式改变
            RunModeMgr.RunModeChanged += RunModeChanged;
            RunModeMgr.EventClassChanged += EventClassChanged;

            RunModeMgr.LoadVacuumChanged += LoadVacuumChanged;
            RunModeMgr.UnloadVacuumChanged += UnloadVacuumChanged;

            Client.OnInitOk += Client_OnInitOk;
            BindData();
        }

        private void Client_OnInitOk()
        {
            Client?.WriteObject(RunModeMgr.Name_Retest, ckbxRetest.Checked);
        }

        private void RunModeChanged()
        {
            if (RunModeMgr.RunMode == RunMode.AutoNormal)
            {
                this.Retest = false;
                this.MTCP = true;
            }
            else if (RunModeMgr.RunMode == RunMode.AutoAudit)
            {
                this.Retest = true;
                this.MTCP = false ;
            }
            else
            {
                this.Retest = false;
                this.MTCP = false;
            }
        }

        public bool AuthorityCtrl 
        {
            set
            {
                ucStation1.uC_Station1.AuthorityCtrl = ucStation1.uC_StationStat1.AuthorityCtrl = value;
                ucStation2.uC_Station1.AuthorityCtrl = ucStation2.uC_StationStat1.AuthorityCtrl = value;
                ucStation3.uC_Station1.AuthorityCtrl = ucStation3.uC_StationStat1.AuthorityCtrl = value;
                ucStation4.uC_Station1.AuthorityCtrl = ucStation4.uC_StationStat1.AuthorityCtrl = value;
                ucStation5.uC_Station1.AuthorityCtrl = ucStation5.uC_StationStat1.AuthorityCtrl = value;
            }
        }

        public bool NoSn
        {
            get => checkBoxNoSn.Checked;
            set
            {
                if (checkBoxNoSn.Checked == value) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => NoSn = value));
                    return;
                }
                checkBoxNoSn.Checked = value;
            }
        }

        public bool AllOk
        {
            get => checkBoxAllOk.Checked;
            set
            {
                if (checkBoxAllOk.Checked == value) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => AllOk = value));
                    return;
                }
                checkBoxAllOk.Checked = value;
            }
        }

        public bool MTCP
        {
            get => ckbxEnableMTCP.Checked;
            set
            {
                if (ckbxEnableMTCP.Checked == value) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => MTCP = value));
                    return;
                }
                ckbxEnableMTCP.Checked = value;
            }
        }

        //public bool WithTM
        //{
        //    get => ckbxWithTm.Checked;
        //    set
        //    {
        //        if (ckbxWithTm.Checked == value) return;
        //        if (InvokeRequired)
        //        {
        //            Invoke(new Action(() => WithTM = value));
        //            return;
        //        }
        //        ckbxWithTm.Checked = value;
        //    }
        //}

        public bool Retest
        {
            get => ckbxRetest.Checked;
            set
            {
                if (ckbxRetest.Checked == value) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => Retest = value));
                    return;
                }
                ckbxRetest.Checked = value;
            }
        }

        public bool BigTray
        {
            get => radioButtonBigTray.Checked;
            set
            {
                if (value == radioButtonBigTray.Checked) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => BigTray = value));
                    return;
                }
                radioButtonBigTray.Checked = value;
            }
        }

        public bool SmallTray
        {
            get => radioButtonSmallTray.Checked;
            set
            {
                if (value == radioButtonSmallTray.Checked) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => SmallTray = value));
                    return;
                }
                radioButtonSmallTray.Checked = value;
            }
        }

        public void BindData()
        {
            labelYield.DataBindings.Add(new Binding("text", Overall.Stat, "PassRate",true, DataSourceUpdateMode.OnPropertyChanged, "0","0.00%"));
            labelFailCount.DataBindings.Add(new Binding("text", Overall.Stat, "Failed"));
            labelPassCount.DataBindings.Add(new Binding("text", Overall.Stat, "Passed"));
            labelTotalCount.DataBindings.Add(new Binding("text", Overall.Stat, "Output"));
        }

        public void ClearStationStat()
        {
            foreach (var name in StationManager.TestStations)
            {
                var station = StationManager.Stations[name];
                station.Stat.Passed = 0;
                station.Stat.Failed = 0;
                station.Stat.Miss = 0;
                station.Stat.Untested = 0;

                Overall.Stat.Passed = 0;
                Overall.Stat.Failed = 0;
            }
        }

        private void checkBoxAllOk_CheckedChanged(object sender, System.EventArgs e)
        {
            if (AllOk)
            {
                RunModeMgr.CustomMode |= CustomMode.AllBinOk;
            }
            else
            {
                RunModeMgr.CustomMode &= ~CustomMode.AllBinOk;
            }
        }

        private void ckbxEnableMTCP_CheckedChanged(object sender, System.EventArgs e)
        {
            ConfigMgr.Instance.EnableClientMTCP = ckbxEnableMTCP.Checked;
        }

        private void checkBoxNoSn_CheckedChanged(object sender, System.EventArgs e)
        {
            if (NoSn)
            {
                RunModeMgr.CustomMode |= CustomMode.NoSn;
            }
            else
            {
                RunModeMgr.CustomMode &= ~CustomMode.NoSn;
            }
        }

        private void ReadCfg()
        {
            NoSn = RunModeMgr.CustomMode.HasFlag(CustomMode.NoSn);
            AllOk = RunModeMgr.CustomMode.HasFlag(CustomMode.AllBinOk);
            MTCP = ConfigMgr.Instance.EnableClientMTCP;
            Retest = ConfigMgr.Instance.Retest;
            BigTray = ConfigMgr.Instance.LoadTrayConfig;
            SmallTray = !ConfigMgr.Instance.LoadTrayConfig;
            Overall.Stat.Passed = ConfigMgr.Instance.DUTPassCount;
            Overall.Stat.Failed = ConfigMgr.Instance.DUTFailCount;
        }

        private void UpdateState()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => { UpdateState(); }));
                    return;
                }
                while (true)
                {
                    if (RunModeMgr.CompleteFlag)
                    {
                        btnClearDut.Text = "清料中";
                        btnClearDut.BackColor = Color.Green;
                    }
                    else
                    {
                        btnClearDut.Text = "清料";
                        btnClearDut.UseVisualStyleBackColor = true;
                    }

                    if (isError)
                    {
                        if (btnClearAlarm.BackColor == Color.Red)
                        {
                            btnClearAlarm.BackColor = Color.Yellow;
                        }
                        else
                        {
                            btnClearAlarm.BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        btnClearAlarm.UseVisualStyleBackColor = true;
                    }

                    EnableUI();
                    this.labelSocketID.Text = "S" + RunModeMgr.SocketID;
                    var status = AlcSystem.Instance.GetSystemStatus();
                    if (status == SYSTEM_STATUS.Running)
                    {
                        btnClearDut.Enabled = false;
                    }
                    else
                    {
                        btnClearDut.Enabled = true;
                    }

                    Thread.Sleep(200);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void EnableUI()
        {
            // 实时 控件刷新
            if (AlcSystem.Instance.GetSystemStatus() == SYSTEM_STATUS.Running || AlcSystem.Instance.GetUserAuthority() == UserAuthority.OPERATOR.ToString())
            {
                this.ckbxEnableMTCP.Enabled = false;
                this.ckbxRetest.Enabled = false;
                this.checkBoxNoSn.Enabled = false;
                this.checkBoxAllOk.Enabled = false;
            }
            else
            {
                this.ckbxEnableMTCP.Enabled = true;
                this.ckbxRetest.Enabled = true;
                this.checkBoxNoSn.Enabled = true;
                this.checkBoxAllOk.Enabled = true;
            }

            if (AlcSystem.Instance.GetSystemStatus() == SYSTEM_STATUS.Idle && AlcSystem.Instance.GetUserAuthority() == UserAuthority.ADMINISTRATOR.ToString())
            {
                this.radioButtonBigTray.Enabled = true;
                this.radioButtonSmallTray.Enabled = true;
            }
            else
            {
                this.radioButtonBigTray.Enabled = false;
                this.radioButtonSmallTray.Enabled = false;
            }
        }

        private void btnClearDut_Click(object sender, EventArgs e)
        {
            if (btnClearDut.Text == "清料" || btnClearDut.Text == "Complete")
            {
                Client?.WriteObject(RunModeMgr.Name_CompleteFlag, true);
            }
            else if (btnClearDut.Text == "清料中")
            {
                Client?.WriteObject(RunModeMgr.Name_CompleteFlag, false);
            }
        }

        private void ckbxRetest_CheckedChanged(object sender, EventArgs e)
        {
            ConfigMgr.Instance.Retest = ckbxRetest.Checked;
            RunModeMgr.TMRetest = ckbxRetest.Checked;
            if (Client.IsInitOk)
            {
                Client?.WriteObject(RunModeMgr.Name_Retest, ckbxRetest.Checked);
            }
        }

        private void btnClearAlarm_Click(object sender, EventArgs e)
        {
            EventCenter.ClearError?.Invoke();
        }

        private void EventClassChanged(uint value)
        {
            if (value >= 1)
            {
                isError = true;
            }
            else
            {
                isError = false;
            }
        }

        private void RadbtnCheckChanged(object sender, EventArgs e)
        {
            var button = (RadioButton)sender;
            if (button.Name == "radioButtonBigTray")
            {
                RunModeMgr.IsLoadBigTray = button.Checked;
                RunModeMgr.IsNGBigTray = button.Checked;
                ConfigMgr.Instance.LoadTrayConfig = button.Checked;
                ConfigMgr.Instance.NGTrayConfig = button.Checked;
            }
            else
            {
                RunModeMgr.IsLoadBigTray = !button.Checked;
                RunModeMgr.IsNGBigTray = !button.Checked;
                ConfigMgr.Instance.LoadTrayConfig = !button.Checked;
                ConfigMgr.Instance.NGTrayConfig = !button.Checked;
            }
        }

        private void LoadVacuumChanged(bool value)
        {
            if (value)
            {
                this.dataGridViewLoadNozzle.BackgroundColor = Color.DarkOrange;
            }
            else
            {
                this.dataGridViewLoadNozzle.BackgroundColor = Color.White;
            }
            EventCenter.ProcessInfo?.Invoke($"Handler上料真空阀当前状态为{value}", ErrorLevel.DEBUG);
        }

        private void UnloadVacuumChanged(bool value)
        {
            if (value)
            {
                this.dataGridViewUnloadNozzle.BackgroundColor = Color.DarkOrange;
            }
            else
            {
                this.dataGridViewUnloadNozzle.BackgroundColor = Color.White;
            }
            EventCenter.ProcessInfo?.Invoke($"Handler下料真空阀当前状态为{value}", ErrorLevel.DEBUG);
        }
    }
}
