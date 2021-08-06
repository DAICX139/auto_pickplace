using System;
using System.Windows.Forms;
using Poc2Auto.Common;
using AlcUtility;
using NetAndEvent.PlcDriver;
using AlcUtility.Language;

namespace Poc2Auto.GUI.FormMode
{
    public partial class FMSelectBin : Form
    {
        public string CurrentLanguage { get; set; } = LangParser.DefaultLanguage;
        private AdsDriverClient _client;
        public FMSelectBin(AdsDriverClient client)
        {
            InitializeComponent();
            _client = client;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (_client == null)
                return;
            var ret =  RunModeMgr.AutoSelectBin(_client, ucModeParams_SelectDut_Bin1.ParamData, out string message);
            if (ret)
            {
                RunModeMgr.RunMode = RunMode.AutoSelectBin;
                RunModeMgr.Running = false;
                AlcSystem.Instance.ShowMsgBox("OK", "Information");
                Close();
                //Start

            }
            else
            {
                AlcSystem.Instance.ShowMsgBox($"Fail, {message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
        }

        public void LangSwitch()
        {
            if (LangParser.CurrentLanguage == CurrentLanguage) return;
            LanguageSwitch.SetCtrl(this, LangParser.CurrentLanguage, CurrentLanguage);
            CurrentLanguage = LangParser.CurrentLanguage;
        }

        private void FMSelectBin_Shown(object sender, EventArgs e)
        {
            LangSwitch();
        }
    }
}
