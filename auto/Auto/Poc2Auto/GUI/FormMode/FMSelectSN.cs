using System;
using System.Windows.Forms;
using Poc2Auto.Common;
using AlcUtility;
using NetAndEvent.PlcDriver;
using AlcUtility.Language;
using System.Threading.Tasks;

namespace Poc2Auto.GUI.FormMode
{
    public partial class FMSelectSN : Form
    {
        private AdsDriverClient _client;
        public FMSelectSN(AdsDriverClient client)
        {
            InitializeComponent();
            _client = client;
            this.rdbtnLoad1.Checked = true;
            EventCenter.ScanMode += ScanMode;
            groupBox1.Enabled = false; ;
        }
        public string CurrentLanguage { get; set; } = LangParser.DefaultLanguage;
        public bool AuthorityCtrl
        {
            set
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => btnOk.Enabled = value));
                    return;
                }
                btnOk.Enabled = value;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_client == null)
                return;

            if (string.IsNullOrEmpty(RunModeMgr.SelectSNSavePath))
            {
                AlcSystem.Instance.Error("请选择文件存放路径以及文件名！", 0, AlcErrorLevel.WARN, "挑选SN");
                return;
            }
            Task.Run(new Action(
             () =>
             {
                 if (UCMain.Instance.Stop(CtrlType.Handler))
                 {
                     if (RunModeMgr.AutoSelectSn(_client, ucModeParams_SelectDut_SN1.ParamData, out string message))
                     {
                         RunModeMgr.RunMode = RunMode.AutoSelectSn;
                         RunModeMgr.Running = false;
                         RunModeMgr.OriginValue = false;
                         var result = AlcSystem.Instance.ShowMsgBox("确认执行挑料-扫码动作吗？", "提醒", AlcMsgBoxButtons.YesNo, icon: AlcMsgBoxIcon.Question);
                         if (result == AlcMsgBoxResult.No)
                             return;
                         else
                             AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Reset);
                     }
                     else
                     {
                         AlcSystem.Instance.ShowMsgBox($"Fail, {message}", "Error", icon: AlcMsgBoxIcon.Error);
                     }
                 }
                 if (InvokeRequired)
                 {
                     Invoke(new Action(() => { Close(); }));
                     return;
                 }
                 Close();
             }));
        }

        public void LangSwitch()
        {
            if (LangParser.CurrentLanguage == CurrentLanguage) return;
            LanguageSwitch.SetCtrl(this, LangParser.CurrentLanguage, CurrentLanguage);
            CurrentLanguage = LangParser.CurrentLanguage;
        }

        private void FMSelectSN_Shown(object sender, EventArgs e)
        {
            LangSwitch();
        }

        private void TraySlectionChanged(object sender, EventArgs e)
        {
            var rdBtn = sender as RadioButton;
            if (rdBtn.Text == "load1")
            {
                if (rdBtn.Checked)
                {
                    RunModeMgr.SelectSNTray = (ushort)TrayName.Load1;
                }
            }
            else if (rdBtn.Text == "load2")
            {
                if (rdBtn.Checked)
                {
                    RunModeMgr.SelectSNTray = (ushort)TrayName.Load2;
                }
            }
            else
            {
                if (rdBtn.Checked)
                {
                    RunModeMgr.SelectSNTray = (ushort)TrayName.NG;
                }
            }
        }

        private void ScanMode()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ScanMode));
                return;
            }
            if (RunModeMgr.IsOnlyScanMode)
            {
                groupBox1.Enabled = true;
            }
            else
            {
                groupBox1.Enabled = false;
            }
        }
    }
}
