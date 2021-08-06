using System;
using System.Windows.Forms;
using Poc2Auto.Common;
using AlcUtility;
using AlcUtility.Language;

namespace Poc2Auto.GUI.RunModeConfig
{
    public partial class UCModeParams_GRR : UserControl
    {
        public static UCModeParams_GRR Instance { get; } = new UCModeParams_GRR();

        public UCModeParams_GRR()
        {
            InitializeComponent();
            if (!CYGKit.GUI.Common.IsDesignMode())
            {
                //BindEnum(TrayName.NG);
                authorityManagement();
                AlcSystem.Instance.UserAuthorityChanged += (o, n) => { authorityManagement(); };
                TestTimes.Value = 5;
            }
        }

        //public void BindEnum<T>(params T[] trayNames) where T: Enum
        //{
        //    if (trayNames == null || trayNames.Length == 0)
        //        comboBoxTrayId.BindEnum<T>();
        //    else
        //        comboBoxTrayId.DataSource = trayNames;
        //}

        public GRRParam ParamData
        {
            get
            {
                return new GRRParam
                {
                    TestTimes = (int)TestTimes.Value,
                    //Region = UCMain.Instance.ucTrays1.NGTrayData,
                };
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
                this.Enabled = false;
            else
                this.Enabled = true;
            
        }

        public string CurrentLanguage { get; set; } = LangParser.DefaultLanguage;
        public void LangSwitch()
        {
            if (LangParser.CurrentLanguage == CurrentLanguage) return;
            LanguageSwitch.SetCtrl(Instance, LangParser.CurrentLanguage, CurrentLanguage);
            CurrentLanguage = LangParser.CurrentLanguage;
        }

    }
}
