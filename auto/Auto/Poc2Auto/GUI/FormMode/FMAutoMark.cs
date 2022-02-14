using System;
using System.Windows.Forms;
using Poc2Auto.Common;
using AlcUtility;
using NetAndEvent.PlcDriver;
using AlcUtility.Language;
using System.Threading.Tasks;

namespace Poc2Auto.GUI.FormMode
{
    public partial class FMAutoMark : Form
    {
        private AdsDriverClient _client;
        public string CurrentLanguage { get; set; } = LangParser.DefaultLanguage;
        public FMAutoMark(AdsDriverClient client)
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
                 if (UCMain.Instance.Stop(CtrlType.Handler))
                 {
                     if (RunModeMgr.AutoMark(_client, ucModeParams_AutoMark1.AutoMarkParam, out string message))
                     {
                         RunModeMgr.RunMode = RunMode.AutoMark;
                         RunModeMgr.Running = false;
                         RunModeMgr.OriginValue = false;
                         AlcSystem.Instance.ShowMsgBox("OK", "Information");
                         //UCMain.Instance.Reset();
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

        private void FMAutoMark_Shown(object sender, EventArgs e)
        {
            LangSwitch();
        }
    }
}
