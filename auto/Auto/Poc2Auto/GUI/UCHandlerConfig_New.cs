using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using Poc2Auto.Database;
using CYGKit.Factory.TableView;
using System;
using System.Windows.Forms;
using Poc2Auto.GUI.FormMode;
using AlcUtility;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Drawing;

namespace Poc2Auto.GUI
{
    public partial class UCHandlerConfig_New : UserControl
    {
        public UCHandlerConfig_New(AdsDriverClient client)
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            _client = client;
            init(client);
            RunModeMgr.RunModeChanged += modeChanged;

            //权限管理
            authorityManagement();
            AlcSystem.Instance.UserAuthorityChanged += (o, n) => { authorityManagement(); };
            RunModeMgr.CurrentSocketIDChanged += SocketIDChanged;
        }
        private AdsDriverClient _client;
        private static AdsDriverClient HandlerClient = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Handler.ToString()) as AdsDriverClient;
        private static AdsDriverClient TesterClient = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Tester.ToString()) as AdsDriverClient;

        //各种模式窗体
        private FMAutoMark fMAutoMark;
        private FMGRR fMGRR;
        private FMSameTray fMSameTray;
        private FMTakeOff fMTakeOff;
        private FMSlipTest fMSlipTest;
        private FMDifferentTray fMDifferentTray;
        private FMSocketTest fMSocketTest;

        private PlcMode testerMode;
        private PlcMode handlerMode;

        public bool TurnOffBuzzer
        {
            get => ckbxCloseBuzzer.Checked;
            set
            {
                if (ckbxCloseBuzzer.Checked == value) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => TurnOffBuzzer = value));
                    return;
                }
                ckbxCloseBuzzer.Checked = value;
            }
        }

        private void init(AdsDriverClient client)
        {
            uC_BinRegionSet1.Client = client;
            uC_BinRegionSet1.NGTrayID = (int)TrayName.NG;
            uC_BinRegionSet1.OK1TrayID = (int)TrayName.UnloadL;
            uC_BinRegionSet1.OK2TrayID = (int)TrayName.UnloadR;
            uC_BinRegionSet1.BindDataBase<DragonContext>();

            fMAutoMark = new FMAutoMark(client) { StartPosition = FormStartPosition.CenterScreen }; 
            fMGRR = new FMGRR(client) { StartPosition = FormStartPosition.CenterScreen };
            fMSameTray = new FMSameTray(client) { StartPosition = FormStartPosition.CenterScreen };
            fMTakeOff = new FMTakeOff(client) { StartPosition = FormStartPosition.CenterScreen };
            fMSlipTest = new FMSlipTest(client) { StartPosition = FormStartPosition.CenterScreen };
            fMDifferentTray = new FMDifferentTray(client) { StartPosition = FormStartPosition.CenterParent };
            fMSocketTest = new FMSocketTest(client) { StartPosition = FormStartPosition.CenterScreen };
        }

        private void UCHandlerConfig_New_Load(object sender, EventArgs e)
        {
            modeChanged();
            timer1.Enabled = true;
        }

        private void btnAutoMark_Click(object sender, EventArgs e)
        {
            if (fMAutoMark == null || fMAutoMark.IsDisposed)
            {
                fMAutoMark = new FMAutoMark(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };
            }
            fMAutoMark.ShowDialog(); 
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            if (fMGRR == null || fMGRR.IsDisposed)
            {
                fMGRR = new FMGRR(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };
            }
            fMGRR.ShowDialog(); 
        }

        private void btnSameTray_Click(object sender, EventArgs e)
        {
            if (fMSameTray == null || fMSameTray.IsDisposed)
            {
                fMSameTray = new FMSameTray(HandlerClient) { StartPosition = FormStartPosition.CenterParent };
            }
            fMSameTray.ShowDialog(); 
        }

        private void btnTakeOff_Click(object sender, EventArgs e)
        {
            if (fMTakeOff == null || fMTakeOff.IsDisposed)
            {
                fMTakeOff = new FMTakeOff(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };
            }
            fMTakeOff.ShowDialog();
        }

        private void btnDryRun_Click(object sender, EventArgs e)
        {

        }

        private void modeChanged()
        {
            var mode = RunModeMgr.RunMode;
            switch (mode)
            {
                case RunMode.Manual: 
                    grpModeSetArea.Text = "Manual"; 
                    break;
                case RunMode.AutoNormal: 
                    grpModeSetArea.Text = "AutoNormal"; 
                    break;
                case RunMode.AutoSelectSn: 
                    grpModeSetArea.Text = "Select SN"; 
                    break;
                case RunMode.AutoSelectBin: 
                    grpModeSetArea.Text = "Select Bin"; 
                    break;
                case RunMode.AutoGRR: 
                    grpModeSetArea.Text = "GRR"; 
                    break;
                case RunMode.AutoAudit: 
                    grpModeSetArea.Text = "Audit"; 
                    break;
                case RunMode.AutoMark: 
                    grpModeSetArea.Text = "AutoMark"; 
                    break;
                case RunMode.DryRun: 
                    grpModeSetArea.Text = "DryRun"; 
                    break;
                case RunMode.DoeSlip:
                    grpModeSetArea.Text = "SlipTest";
                    break;
                case RunMode.DoeSameTray: 
                    grpModeSetArea.Text = "SameTray"; 
                    break;
                case RunMode.DoeDifferentTray:
                    break;
                case RunMode.DoeTakeOff: 
                    grpModeSetArea.Text = "TakeOff"; 
                    break;
                default:
                    break;
            }
        }

        private void btnDoeSlip_Click(object sender, EventArgs e)
        {
            if (fMSlipTest == null || fMSlipTest.IsDisposed)
            {
                fMSlipTest = new FMSlipTest(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };
            }
            fMSlipTest.ShowDialog();
        }

        private void btnSocketTest_Click(object sender, EventArgs e)
        {
            if (fMSocketTest == null || fMSocketTest.IsDisposed)
            {
                fMSocketTest = new FMSocketTest(_client) { StartPosition = FormStartPosition.CenterScreen };
                fMSocketTest.Show();
            }
            else
            {
                fMSocketTest.Show();
            }
        }
        private void btnSocketOpen_Click(object sender, EventArgs e)
        {
            EventCenter.LoadOrUnload?.Invoke();
        }

        private void ckbxCloseBuzzer_CheckedChanged(object sender, EventArgs e)
        {
            ConfigMgr.Instance.TurnOffBuzzer = ckbxCloseBuzzer.Checked;
            EventCenter.EnableBuzzer?.Invoke(ckbxCloseBuzzer.Checked);
        }

        private void authorityManagement()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(authorityManagement));
                return;
            }

            if (AlcSystem.Instance.GetUserAuthority() == UserAuthority.OPERATOR.ToString())
            {
                uC_BinRegionSet1.Enabled = false;
                grpModeSetArea.Visible = false;
                groupBox1.Visible = false;
            }
            else
            {
                uC_BinRegionSet1.Enabled = true;
                grpModeSetArea.Visible = true;
                groupBox1.Visible = true;
            }
        }

        private void btnNozzle1Load_Click(object sender, EventArgs e)
        {
            HandlerClient?.WriteObject(RunModeMgr.Name_DebugNozzle1TrayLoad, true);
        }

        private void btnNozzle1Scan_Click(object sender, EventArgs e)
        {
            HandlerClient?.WriteObject(RunModeMgr.Name_DebugNozzle1Scan, true);
        }

        private void btnRotateOneStation_Click(object sender, EventArgs e)
        {
            TesterClient?.WriteObject(RunModeMgr.Name_DryRunTesterRotation, true);
        }

        private void btnSocketZAxisUp_Click(object sender, EventArgs e)
        {
            TesterClient?.WriteObject(RunModeMgr.Name_DebugZAxisUp, true);
        }

        private void btnNozzle1SocketPutDut_Click(object sender, EventArgs e)
        {
            HandlerClient?.WriteObject(RunModeMgr.Name_DebugNozzle1SocketPut, true);
        }

        private void btnPullPutter_Click(object sender, EventArgs e)
        {
            TesterClient?.WriteObject(RunModeMgr.Name_DebugPullPutter, true);
        }

        private void btnSocketClose_Click(object sender, EventArgs e)
        {
            TesterClient?.WriteObject(RunModeMgr.Name_DebugCloseSocket, true);
        }

        private void btnZAxisDown_Click(object sender, EventArgs e)
        {
            TesterClient?.WriteObject(RunModeMgr.Name_DebugZAxisDown, true);
        }

        private void Nozzle2SocketPickDut_Click(object sender, EventArgs e)
        {
            HandlerClient?.WriteObject(RunModeMgr.Name_DebugNozzle2SocketPick, true);
        }

        private void btnNozzle1TrayUload_Click(object sender, EventArgs e)
        {
            HandlerClient?.WriteObject(RunModeMgr.Name_DebugNozzle1TrayUload, true);
        }

        private void btnNozzle2TrayUload_Click(object sender, EventArgs e)
        {
            HandlerClient?.WriteObject(RunModeMgr.Name_DebugNozzle2TrayUload, true);
        }

        private void btnSocketOpenCap_Click(object sender, EventArgs e)
        {
            TesterClient?.WriteObject(RunModeMgr.Name_DebugOpenSocket, true);
        }

        private void btnPushPutter_Click(object sender, EventArgs e)
        {
            TesterClient?.WriteObject(RunModeMgr.Name_DebugPushPutter, true);
        }

        private void SocketIDChanged()
        {
            lbSocketID.BeginInvoke(new Action(() =>
            {
                lbSocketID.Text = RunModeMgr.SocketID.ToString();
            }));
        }

        private void btnSingleStepDebug_Click(object sender, EventArgs e)
        {
            Task.Run(new Action(
            () =>
            {
                if (UCMain.Instance.Stop())
                {
                    HandlerClient?.GetSysInfoCtrl.ModeCtrl(RunModeMgr.Doe);
                    HandlerClient.GetSysInfoCtrl.SubModeCtrl(RunModeMgr.Doe, RunModeMgr.Doe_TesterDebug);

                    TesterClient?.GetSysInfoCtrl.ModeCtrl(RunModeMgr.Doe);
                    TesterClient.GetSysInfoCtrl.SubModeCtrl(RunModeMgr.Doe, RunModeMgr.Doe_TesterDebug);

                    RunModeMgr.RunMode = RunMode.DoeSingleDebug;
                    UCMain.Instance.Reset();
                }
            }));
            
        }

        private void btnDifferentTray_Click(object sender, EventArgs e)
        {
            if (fMDifferentTray == null || fMDifferentTray.IsDisposed)
            {
                fMDifferentTray = new FMDifferentTray(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };
            }
            fMDifferentTray.ShowDialog();
        }

        private void btnAutoMark_Click_1(object sender, EventArgs e)
        {
            if (fMAutoMark == null || fMAutoMark.IsDisposed)
            {
                fMAutoMark = new FMAutoMark(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };
            }
            fMAutoMark.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (HandlerClient == null || TesterClient == null)
                return;
            handlerMode = (PlcMode)(uint)HandlerClient?.ReadObject("GVL_MachineInterface.MachineCmd.nMode", typeof(uint));
            testerMode = (PlcMode)(uint)TesterClient?.ReadObject("GVL_MachineInterface.MachineCmd.nMode", typeof(uint));
            UpdateSubMode();
        }

        private void UpdateSubMode()
        {
            if (handlerMode == PlcMode.DoeMode && testerMode == PlcMode.DoeMode)
            {
                var handlerSubMode = (uint)HandlerClient.ReadObject("GVL_MachineInterface.MachineCmd.nDOESubMode", typeof(uint));
                var testerSubMode = (uint)TesterClient?.ReadObject("GVL_MachineInterface.MachineCmd.nDOESubMode", typeof(uint));

                if (handlerSubMode == RunModeMgr.Doe_TakeoffTest && testerSubMode == RunModeMgr.Doe_TakeoffTest)
                {
                    SetButtonColor(btnTakeOff);
                }
                else if (handlerSubMode == RunModeMgr.Doe_SameTrayTest)
                {
                    SetButtonColor(btnSameTray);
                }
                else if (handlerSubMode == RunModeMgr.Doe_SlipTest)
                {
                    SetButtonColor(btnDoeSlip);
                }
                else if (handlerSubMode == RunModeMgr.Doe_TesterDebug && testerSubMode == RunModeMgr.Doe_TesterDebug)
                {
                    SetButtonColor(btnSingleStepDebug);
                }
                else if (handlerSubMode == RunModeMgr.Doe_SocketUniformityTest && testerSubMode == RunModeMgr.Doe_SocketUniformityTest)
                {
                    SetButtonColor(btnSocketTest);
                }
                else
                {
                    SetButtonColor();
                }
            }
            else if (handlerMode == PlcMode.AutoMode)
            {
                var handlerSubMode = (uint)HandlerClient.ReadObject("GVL_MachineInterface.MachineCmd.nAutoSubMode", typeof(uint));
                if (handlerSubMode == RunModeMgr.Auto_Mark)
                {
                    SetButtonColor(btnAutoMark);
                }
                else
                {
                    SetButtonColor();
                }
            }
            else
            {
                SetButtonColor();
            }
        }

        public void SetButtonColor(Button button)
        {
            List<Button> buttons = new List<Button>()
            {
                btnAudit,
                btnAutoMark,
                btnDifferentTray,
                btnDifferentTray,
                btnDoeSlip,
                btnSameTray,
                btnSocketTest,
                btnSingleStepDebug,
                btnDryRun,
            };
            button.BackColor = Color.Green;
            foreach (var btn in buttons)
            {
                if (btn == button) continue;
                btn.UseVisualStyleBackColor = true;
            }
        }

        public void SetButtonColor()
        {
            List<Button> buttons = new List<Button>()
            {
                btnAudit,
                btnAutoMark,
                btnDifferentTray,
                btnDifferentTray,
                btnDoeSlip,
                btnSameTray,
                btnSocketTest,
                btnSingleStepDebug,
                btnDryRun,
            };
            foreach (var btn in buttons)
            {
                btn.UseVisualStyleBackColor = true;
            }
        }

    }
}
