using AlcUtility;
using AlcUtility.Language;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poc2Auto.GUI.FormMode
{
    public partial class FMSocketTest : Form
    {
        public AdsDriverClient _client;
        public string CurrentLanguage { get; set; } = LangParser.DefaultLanguage;
        private bool _isOk = false;
        public FMSocketTest(AdsDriverClient client)
        {
            InitializeComponent();
            _client = client;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_client == null)
                return;

            Task.Run(new Action(
             () =>
             {
                 if (UCMain.Instance.Stop())
                 {
                     if (RunModeMgr.SockeTest(_client, ucModeParams_SocketTest1.ParamData, out string message))
                     {
                         RunModeMgr.RunMode = RunMode.DoeSocketUniformityTest;
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

        private void FMSocketTest_Shown(object sender, EventArgs e)
        {
            LangSwitch();
        }

        private void btnContinueTest_Click(object sender, EventArgs e)
        {
            EventCenter.SocketTestContinue?.Invoke();
        }
    }
}
