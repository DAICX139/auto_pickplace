using System;
using System.Windows.Forms;
using Poc2Auto.Common;
using AlcUtility;
using NetAndEvent.PlcDriver;
using AlcUtility.Language;
using System.Threading.Tasks;

namespace Poc2Auto.GUI.FormMode
{
    public partial class FMSlipTest : Form
    {
        private AdsDriverClient _client;
        public string CurrentLanguage { get; set; } = LangParser.DefaultLanguage;
        public FMSlipTest(AdsDriverClient client)
        {
            InitializeComponent();
            _client = client;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_client == null)
                return;

            Task.Run(new Action(
             () =>
             {
                 if (UCMain.Instance.Stop())
                 {
                     if (RunModeMgr.SlipTest(_client, ucModeParams_DoeSlipTest1.SlipParam, out string message))
                     {
                         RunModeMgr.RunMode = RunMode.DoeSlip;
                         RunModeMgr.Running = false;
                         AlcSystem.Instance.ShowMsgBox("OK", "Information");
                         UCMain.Instance.Reset();
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

        private void FMSlipTest_Shown(object sender, EventArgs e)
        {
            LangSwitch();
        }
    }
}
