using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using Poc2Auto.Database;
using System;
using System.Windows.Forms;
using Poc2Auto.GUI.FormMode;
using AlcUtility;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

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

            //权限管理
            authorityManagement();
            AlcSystem.Instance.UserAuthorityChanged += (o, n) => { authorityManagement(); };
            RunModeMgr.CurrentSocketIDChanged += SocketIDChanged;

            ReadConfig();
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

        public int StationTestFailTimes
        {
            get => (int)numericUpDown1.Value;
            set
            {
                if (value == (int)numericUpDown1.Value) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => StationTestFailTimes = value));
                    return;
                }
                numericUpDown1.Value = value;
            }
        }

        public int SocketTestFailTimes
        {
            get => (int)numericUpDown2.Value;
            set
            {
                if (value == (int)numericUpDown2.Value) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => SocketTestFailTimes = value));
                    return;
                }
                numericUpDown2.Value = value;
            }
        }

        public bool DoorLockControl
        {
            get => checkBoxDoorLockControl.Checked;
            set
            {
                if (value == checkBoxDoorLockControl.Checked) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => DoorLockControl = value));
                    return;
                }
                checkBoxDoorLockControl.Checked = value;
            }
        }

        private void init(AdsDriverClient client)
        {
            ucBinRegionSet1.Client = client;
            ucBinRegionSet1.NGTrayID = (int)TrayName.NG;
            ucBinRegionSet1.OK1TrayID = (int)TrayName.Pass1;
            ucBinRegionSet1.OK2TrayID = (int)TrayName.Pass2;
            ucBinRegionSet1.BindDataBase<DragonContext>();

            fMAutoMark = new FMAutoMark(client) { StartPosition = FormStartPosition.CenterScreen }; 
            fMGRR = new FMGRR(client) { StartPosition = FormStartPosition.CenterScreen };
            fMSameTray = new FMSameTray(client) { StartPosition = FormStartPosition.CenterScreen };
            fMTakeOff = new FMTakeOff(client) { StartPosition = FormStartPosition.CenterScreen };
            fMSlipTest = new FMSlipTest(client) { StartPosition = FormStartPosition.CenterScreen };
            fMDifferentTray = new FMDifferentTray(client) { StartPosition = FormStartPosition.CenterParent };
            fMSocketTest = new FMSocketTest(client) { StartPosition = FormStartPosition.CenterScreen };
        }

        private void ReadConfig()
        {
            TurnOffBuzzer = ConfigMgr.Instance.TurnOffBuzzer;
            StationTestFailTimes = ConfigMgr.Instance.StationTestFailTimes;
            SocketTestFailTimes = ConfigMgr.Instance.SocketTestFailTimes;
            DoorLockControl = ConfigMgr.Instance.DoorLockControl;
        }

        private void UCHandlerConfig_New_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
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
            if (HandlerClient == null || TesterClient == null)
                return;

            Task.Run(new Action(
             () =>
             {
                 if (UCMain.Instance.Stop(CtrlType.Both))
                 {
                     if (RunModeMgr.DryRun1(HandlerClient, out string message) && RunModeMgr.DryRun1(TesterClient, out string message1))
                     {
                         RunModeMgr.RunMode = RunMode.DryRun;
                         RunModeMgr.Running = false;
                         RunModeMgr.OriginValue = true;
                         //AlcSystem.Instance.ShowMsgBox("OK", "Information");
                         //UCMain.Instance.Reset();
                     }
                     else
                     {
                         AlcSystem.Instance.ShowMsgBox($"Fail, {message}", "Error", icon: AlcMsgBoxIcon.Error);
                     }
                 } 
             }));
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

        private void ckbxCloseBuzzer_CheckedChanged(object sender, EventArgs e)
        {
            ConfigMgr.Instance.TurnOffBuzzer = ckbxCloseBuzzer.Checked;
            EventCenter.EnableBuzzer?.Invoke(!ckbxCloseBuzzer.Checked);
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
                ucBinRegionSet1.Enabled = false;
                groupBox3.Enabled = false;
                groupBox2.Enabled = false;
                groupBox4.Visible = false;
            }
            else
            {
                ucBinRegionSet1.Enabled = true;
                groupBox3.Enabled = true;
                groupBox2.Enabled = true;
                groupBox4.Visible = true;
            }
        }

        private void SocketIDChanged()
        {
            lbSocketID.BeginInvoke(new Action(() =>
            {
                lbSocketID.Text =  $"当前Socket: {RunModeMgr.SocketID}";
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
            try
            {
                if (HandlerClient == null || TesterClient == null)
                    return;
                if (!HandlerClient.IsInitOk || !TesterClient.IsInitOk)
                    return;
                handlerMode = (PlcMode)RunModeMgr.HandlerMode;
                testerMode = (PlcMode)RunModeMgr.TesterMode;
                UpdateSubMode();
            }
            catch (Exception)
            {


            }
        }

        private void UpdateSubMode()
        {
            if (handlerMode == PlcMode.AutoMode && testerMode == PlcMode.AutoMode)
            {
                //var testerSubMode = (uint)TesterClient?.ReadObject("GVL_MachineInterface.MachineCmd.nAutoSubMode", typeof(uint));
                //var handlerSubMode = (uint)HandlerClient.ReadObject("GVL_MachineInterface.MachineCmd.nAutoSubMode", typeof(uint));
                var testerSubMode = RunModeMgr.TesterSubMode;
                var handlerSubMode = RunModeMgr.HandlerSubMode;

                if (testerSubMode == RunModeMgr.Func_DryRun && handlerSubMode == RunModeMgr.Func_DryRun)
                {
                    SetButtonColor(btnDryRun);
                }
                else if (handlerSubMode == RunModeMgr.Func_AutoMark)
                {
                    SetButtonColor(btnAutoMark);
                }
                else if (handlerSubMode == RunModeMgr.Func_DoeSameTrayTest)
                {
                    SetButtonColor(btnSameTray);
                }
                else if (handlerSubMode == RunModeMgr.Func_DoeSlipTest)
                {
                    SetButtonColor(btnDoeSlip);
                }
                else if (handlerSubMode == RunModeMgr.Func_DoeDifferentTrayTest)
                {
                    SetButtonColor(btnDifferentTray);
                }
                else if(handlerSubMode == RunModeMgr.Func_DoeTakeoffTest && testerSubMode == RunModeMgr.Func_DoeTakeoffTest)
                {
                    SetButtonColor(btnTakeOff);
                }
                //else if (handlerSubMode == RunModeMgr.Func_Script)
                //{
                //    SetButtonColor(btnScript);
                //}
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
                btnAutoMark,
                btnDifferentTray,
                btnDifferentTray,
                btnDoeSlip,
                btnSameTray,
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
                btnAutoMark,
                btnDifferentTray,
                btnDifferentTray,
                btnDoeSlip,
                btnSameTray,
                btnDryRun,
            };
            foreach (var btn in buttons)
            {
                btn.UseVisualStyleBackColor = true;
            }
        }
 
        private void btnScript_Click(object sender, EventArgs e)
        {
            if (HandlerClient == null )
                return;

            Task.Run(new Action(
             () =>
             {
                 if (UCMain.Instance.Stop(CtrlType.Handler))
                 {
                     if (RunModeMgr.Script(HandlerClient, out string message))
                     {
                         RunModeMgr.RunMode = RunMode.Script;
                         RunModeMgr.Running = false;
                         //AlcSystem.Instance.ShowMsgBox("OK", "Information");
                         //UCMain.Instance.Reset();
                     }
                     else
                     {
                         AlcSystem.Instance.ShowMsgBox($"Fail, {message}", "Error", icon: AlcMsgBoxIcon.Error);
                     }
                 }
             }));
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ConfigMgr.Instance.StationTestFailTimes = (int)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            ConfigMgr.Instance.SocketTestFailTimes = (int)numericUpDown2.Value;
        }

        private void btnAutoMark_Click(object sender, EventArgs e)
        {
            if (fMAutoMark == null || fMAutoMark.IsDisposed)
            {
                fMAutoMark = new FMAutoMark(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };
            }
            fMAutoMark.ShowDialog();
        }

        private void checkBoxDoorLockControl_CheckedChanged(object sender, EventArgs e)
        {
            var enable = checkBoxDoorLockControl.Checked;
            if (HandlerClient.IsInitOk)
            {
                HandlerClient?.WriteObject(RunModeMgr.Name_DoorLockControl, enable);
            }
            if (TesterClient.IsInitOk)
            {
                TesterClient?.WriteObject(RunModeMgr.Name_DoorLockControl, enable);
            }
            
            ConfigMgr.Instance.DoorLockControl = enable;
        }

        private void btnClearOnlineDut_Click(object sender, EventArgs e)
        {
            var result = AlcSystem.Instance.ShowMsgBox("确定清空线上DUT？", "", AlcMsgBoxButtons.YesNo, AlcMsgBoxIcon.Question, AlcMsgBoxDefaultButton.Button2);
 
            if (AlcMsgBoxResult.No == result)
                return;
            EventCenter.ClearOnlineDut?.Invoke();
        }

        private void btnClearSocketStatus_Click(object sender, EventArgs e)
        {
            try
            {
                if (HandlerClient.IsInitOk)
                {
                    HandlerClient.WriteObject(RunModeMgr.Name_ClearSokcetStatus, true);
                }
                
            }
            catch (Exception ex)
            {
 
            }
        }

        private void btnClearHandlerProessData_Click(object sender, EventArgs e)
        {
            try
            {
                if (HandlerClient.IsInitOk)
                {
                    HandlerClient.WriteObject(RunModeMgr.Name_ClearData, true);
                }
                
            }
            catch (Exception ex)
            {
 
            }
            
        }

        private void btnVersion_Click(object sender, EventArgs e)
        {
        //    var ver1 =  (string)HandlerClient.ReadObject(RunModeMgr.Name_PlcProgramVer, typeof(string));
        //    var ver2 =  (string)TesterClient.ReadObject(RunModeMgr.Name_PlcProgramVer, typeof(string));
        }

        private void btnClearTesterProessData_Click(object sender, EventArgs e)
        {
            try
            {
                if (TesterClient.IsInitOk)
                {
                    TesterClient.WriteObject(RunModeMgr.Name_ClearData, true);
                }
                
            }
            catch (Exception ex)
            {
 
            }
        }
    }
}
