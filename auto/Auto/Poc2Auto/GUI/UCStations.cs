using System.Windows.Forms;
using Poc2Auto.Model;
using Poc2Auto.Database;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using System;

namespace Poc2Auto.GUI
{
    public partial class UCStations : UserControl
    {
        public UCStations()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            var TesterPlc = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Tester.ToString());
            var HandlerPlc = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Handler.ToString());
            uphLabel1.BindDatabase<DragonContext, Product>();
            UCRunMode HandlerMode = new UCRunMode();
            UCRunMode TesterMode = new UCRunMode();
            HandlerMode.ModeChanged += (m) => { EventCenter.HandlerModeChanged?.Invoke(m); };
            TesterMode.ModeChanged += (m) => { EventCenter.TesterModeChanged?.Invoke(m); };
            HandlerMode.PlcDriver = HandlerPlc;
            TesterMode.PlcDriver = TesterPlc;
            RunModeMgr.CustomModeChanged += ReadCfg;
            ReadCfg();

            //良率、失败率展示
            RunModeMgr.YieldDutChanged += YieldTotal;
            RunModeMgr.FailDutChanged += FailTotal;
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

        public bool EnableMTCP
        {
            get => ckbxEnableMTCP.Checked;
            set
            {
                if (ckbxEnableMTCP.Checked == value) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => EnableMTCP = value));
                    return;
                }
                ckbxEnableMTCP.Checked = value;
            }
        }

        public bool WithTM
        {
            get => ckbxWithTm.Checked;
            set
            {
                if (ckbxWithTm.Checked == value) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => WithTM = value));
                    return;
                }
                ckbxWithTm.Checked = value;
            }
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

        private void ckbxWithTm_CheckedChanged(object sender, System.EventArgs e)
        {
            ConfigMgr.Instance.WithTM = ckbxWithTm.Checked;
            EventCenter.EnableTM?.Invoke(ckbxWithTm.Checked);
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
            EnableMTCP = ConfigMgr.Instance.EnableClientMTCP;
            WithTM = ConfigMgr.Instance.WithTM;
        }

        private void YieldTotal()
        {
            labelYield.BeginInvoke(new Action(() =>
            {
                labelYield.Text = ((double)RunModeMgr.YieldDut / RunModeMgr.UnloadTotal).ToString("0.00%");
            }));
        }

        private void FailTotal()
        {
            labelFail.BeginInvoke(new Action(() =>
            {
                labelFail.Text = ((double)RunModeMgr.FailDut / RunModeMgr.UnloadTotal).ToString("0.00%");
            }));
        }

        DateTime lastClickTime = DateTime.Now;
        private void btnTMReset_Click(object sender, EventArgs e)
        {
            if (DateTime.Now - lastClickTime < new TimeSpan(0, 0, 0, 3))
                return;
            lastClickTime = DateTime.Now;
            EventCenter.TMReset?.Invoke();
        }
    }
}
