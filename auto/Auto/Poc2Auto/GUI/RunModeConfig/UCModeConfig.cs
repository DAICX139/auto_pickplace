using NetAndEvent.PlcDriver;
using System;
using System.Windows.Forms;
using CYGKit.GUI;
using Poc2Auto.Common;
using AlcUtility;
using System.Linq;
using System.Collections.Generic;
using AlcUtility.Language;

namespace Poc2Auto.GUI.RunModeConfig
{
    public partial class UCModeConfig : UserControl
    {
        public UCModeConfig()
        {
            InitializeComponent();
            //comboBoxMode.BindEnum<RunMode>();
            if (!CYGKit.GUI.Common.IsDesignMode())
            {
                authorityManagement();
                AlcSystem.Instance.UserAuthorityChanged += (o, n) => { authorityManagement(); };
            }
            comboBoxMode.SelectedIndex = comboBoxMode.FindString(ConfigMgr.Instance.DefaultMode);  
        }

        private AdsDriverClient _client;
        private RunMode getMode;
        List<RunMode> operaterMode = new List<RunMode>()
        {
            RunMode.Manual,RunMode.AutoNormal,RunMode.AutoSelectSn,RunMode.AutoSelectBin,
            RunMode.AutoGRR,RunMode.AutoAudit,RunMode.AutoMark,RunMode.DryRun,
        };
        List<RunMode> engineerMode = Enum.GetValues(typeof(RunMode)).Cast<RunMode>().ToList();

        public void BindClient(AdsDriverClient client)
        {
            SetEnable(false);
            _client = client;
            if (_client == null) return;
            _client.OnInitOk += Client_OnInitOk;
            _client.OnDisconnected += Client_OnDisconnected;
            if (_client.IsInitOk) SyncMode();
        }

        private void Client_OnDisconnected()
        {
            SetEnable(false);
        }

        private void Client_OnInitOk()
        {
            SyncMode();
            SetEnable(true);
        }

        private void SyncMode()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(SyncMode));
                return;
            }
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
                comboBoxMode.Items.Clear();
                foreach (var mode in operaterMode)
                    comboBoxMode.Items.Add(mode);
                comboBoxMode.SelectedIndex = 0;
            }
            else
            {
                foreach (var mode in engineerMode)
                    if (!comboBoxMode.Items.Contains(mode))
                        comboBoxMode.Items.Add(mode);
            }
        }

        private void SetEnable(bool enable)
        {
            buttonOK.Enabled = enable;
        }

        string lastSwitchLanguage = LangParser.DefaultLanguage;
        private void ComboBoxMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //panelParams.Controls.Add(UCModeParams_SelectDut_SN.Instance);
            //panelParams.Controls.Add(UCModeParams_SelectDut_Bin.Instance);
            //panelParams.Controls.Add(UCModeParams_DoeSlipTest.Instance);
            //panelParams.Controls.Add(UCModeParams_DoeSameTrayTest.Instance);
            //panelParams.Controls.Add(UCModeParams_DoeDifferentTrayTest.Instance);
            //panelParams.Controls.Add(UCModeParams_DoeTakeOffTest.Instance);
            //panelParams.Controls.Add(UCModeParams_GRR.Instance);
            //panelParams.Controls.Add(UCModeParams_AutoMark.Instance);
            //
            //panelParams.Controls.Clear();
            //switch ((RunMode)comboBoxMode.SelectedItem)
            //{
            //    case RunMode.Manual:
            //        break;
            //    case RunMode.AutoNormal:
            //        break;
            //    case RunMode.DryRun:
            //        panelParams.Controls.Add(TestDryRun.Instance);
            //        break;
            //    case RunMode.AutoSelectSn:
            //        panelParams.Controls.Add(UCModeParams_SelectDut_SN.Instance);
            //        UCModeParams_SelectDut_SN.Instance.LangSwitch();
            //        break;
            //    case RunMode.AutoSelectBin:
            //        panelParams.Controls.Add(UCModeParams_SelectDut_Bin.Instance);
            //        UCModeParams_SelectDut_Bin.Instance.LangSwitch();
            //        break;
            //    case RunMode.DoeSlip:
            //        panelParams.Controls.Add(UCModeParams_DoeSlipTest.Instance);
            //        UCModeParams_DoeSlipTest.Instance.LangSwitch();
            //        break;
            //    case RunMode.DoeSameTray:
            //        panelParams.Controls.Add(UCModeParams_DoeSameTrayTest.Instance);
            //        UCModeParams_DoeSameTrayTest.Instance.LangSwitch();
            //        break;
            //    case RunMode.DoeDifferentTray:
            //        panelParams.Controls.Add(UCModeParams_DoeDifferentTrayTest.Instance);
            //        UCModeParams_DoeDifferentTrayTest.Instance.LangSwitch();
            //        break;
            //    case RunMode.DoeTakeOff:
            //        panelParams.Controls.Add(UCModeParams_DoeTakeOffTest.Instance);
            //        UCModeParams_DoeTakeOffTest.Instance.LangSwitch();
            //        break;
            //    case RunMode.AutoGRR:
            //    case RunMode.AutoAudit:
            //        panelParams.Controls.Add(UCModeParams_GRR.Instance);
            //        UCModeParams_GRR.Instance.LangSwitch();
            //        break;
            //    case RunMode.AutoMark:
            //        panelParams.Controls.Add(UCModeParams_AutoMark.Instance);
            //        UCModeParams_AutoMark.Instance.LangSwitch();
            //        break;
            //}
            //if (LangParser.CurrentLanguage != lastSwitchLanguage)
            //{
            //    LanguageSwitch.SetCtrl(panelParams, LangParser.CurrentLanguage, lastSwitchLanguage);
            //    lastSwitchLanguage = LangParser.CurrentLanguage;
            //}

        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            bool result = true;
            string message = "";
            var mode = (RunMode)comboBoxMode.SelectedItem;
            getMode = mode;
            //switch (mode)
            //{
            //    case RunMode.Manual:
            //        break;
            //    case RunMode.DryRun:
            //        RunModeMgr.DryRun1(out message);
            //        break;
            //    case RunMode.AutoNormal:
            //        result = RunModeMgr.AutoNormal(_client, out message);
            //        break;
            //    case RunMode.AutoSelectSn:
            //        var uc = (UCModeParams_SelectDut_SN)panelParams.Controls[0];
            //        result = RunModeMgr.AutoSelectSn(_client, uc.ParamData, out message);
            //        break;
            //    case RunMode.AutoSelectBin:
            //        var uc2 = (UCModeParams_SelectDut_Bin)panelParams.Controls[0];
            //        result = RunModeMgr.AutoSelectBin(_client, uc2.ParamData, out message);
            //        break;
            //    case RunMode.DoeSlip:
            //        var uc3 = (UCModeParams_DoeSlipTest)panelParams.Controls[0];
            //        result = RunModeMgr.SlipTest(_client, uc3.KeyValues, uc3.SlipParam, uc3.SlipDataSrc, out message);
            //        break;
            //    case RunMode.DoeSameTray:
            //        var uc4 = (UCModeParams_DoeSameTrayTest)panelParams.Controls[0];
            //        result = RunModeMgr.SameTrayTest(_client, uc4.KeyValues, uc4.SameTrayParam, uc4.SameTrayDataSrc, out message);
            //        break;
            //    case RunMode.DoeDifferentTray:
            //        var uc5 = (UCModeParams_DoeDifferentTrayTest)panelParams.Controls[0];
            //        result = RunModeMgr.DifferentTrayTest(_client, uc5.DifferentTrayParam, out message);
            //        break;
            //    case RunMode.DoeTakeOff:
            //        var uc6 = (UCModeParams_DoeTakeOffTest)panelParams.Controls[0];
            //        result = RunModeMgr.TakeOffTest(_client, uc6.TakeOffParam, out message);
            //        break;
            //    case RunMode.AutoGRR:
            //    case RunMode.AutoAudit:
            //        var uc7 = (UCModeParams_GRR)panelParams.Controls[0];
            //        result = RunModeMgr.GRR(_client, uc7.ParamData, out message);
            //        break;
            //    case RunMode.AutoMark:
            //        var uc8 = (UCModeParams_AutoMark)panelParams.Controls[0];
            //        result = RunModeMgr.AutoMark(_client, uc8.AutoMarkParam, out message);
            //        break;
            //}
            //if (result)
            //{
            //    RunModeMgr.RunMode = mode;
            //    RunModeMgr.Running = false;
            //    AlcSystem.Instance.ShowMsgBox("OK", "Information");
            //}
            //else
            //{
            //    AlcSystem.Instance.ShowMsgBox(message, "Error", icon: AlcMsgBoxIcon.Error);
            //}
        }

        private void ButtonReset_Click(object sender, EventArgs e)
        {
            comboBoxMode.SelectedItem = getMode;
        }
    }
}
