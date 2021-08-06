using AlcUtility;
using AlcUtility.Language;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poc2Auto.GUI.FormMode
{
    public partial class FMDifferentTray : Form
    {
        public AdsDriverClient _client;
        public string CurrentLanguage { get; set; } = LangParser.DefaultLanguage;
        public FMDifferentTray(AdsDriverClient client)
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
                    Invoke(new Action(() => btnOK.Enabled = value));
                    return;
                }
                btnOK.Enabled = value;
            }
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
                     if (RunModeMgr.DifferentTrayTest(_client, ucModeParams_DoeDifferentTrayTest1.DifferentTrayParam, out string message))
                     {
                         RunModeMgr.RunMode = RunMode.DoeDifferentTray;
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

        private void FMDifferentTray_Shown(object sender, EventArgs e)
        {
            LangSwitch();
        }
    }
}
