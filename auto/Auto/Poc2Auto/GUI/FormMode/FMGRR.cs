using System;
using System.Windows.Forms;
using Poc2Auto.Common;
using AlcUtility;
using NetAndEvent.PlcDriver;
using AlcUtility.Language;
using WeifenLuo.WinFormsUI.Docking;
using System.Threading.Tasks;

namespace Poc2Auto.GUI.FormMode
{
    public partial class FMGRR : Form
    {
        public AdsDriverClient _client;
        public string CurrentLanguage { get; set; } = LangParser.DefaultLanguage;
        public FMGRR(AdsDriverClient client)
        {
            InitializeComponent();
            _client = client;
        }
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

            Task.Run(new Action(
             () =>
             {
                 if (UCMain.Instance.Stop())
                 {

                     if (RunModeMgr.GRR(_client, ucModeParams_GRR1.ParamData, out string message))
                     {
                         RunModeMgr.RunMode = RunMode.AutoGRR;
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

        private void FMGRR_Shown(object sender, EventArgs e)
        {
            LangSwitch();
        }
    }
}
