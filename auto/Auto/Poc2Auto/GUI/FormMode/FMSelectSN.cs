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
                 if (UCMain.Instance.Stop())
                 {
                     if (RunModeMgr.AutoSelectSn(_client, ucModeParams_SelectDut_SN1.ParamData, out string message))
                     {
                         RunModeMgr.RunMode = RunMode.AutoSelectSn;
                         RunModeMgr.Running = false;
                         AlcSystem.Instance.ShowMsgBox("OK", "Information");
                         UCMain.Instance.Reset();

                         AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Start);
                     }
                     else
                     {
                         AlcSystem.Instance.ShowMsgBox($"Fail, {message}", "Error", icon: AlcMsgBoxIcon.Error);
                     }
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
    }
}
