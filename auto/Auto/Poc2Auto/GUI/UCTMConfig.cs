using AlcUtility;
using CYGKit.Factory.ConditionEditor;
using System;
using System.Windows.Forms;

namespace Poc2Auto.GUI
{
    public partial class UCTMConfig : UserControl
    {
        public static UCTMConfig Instance;

        public UCTMConfig()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            if (!CYGKit.GUI.Common.IsDesignMode())
                uC_Rules1.FilePath = $"{Application.StartupPath}\\UiParamFiles\\BinRule.xml";

            Instance = this;
            authorityManagement();
            AlcSystem.Instance.UserAuthorityChanged += (o, n) => { authorityManagement(); };
        }

        public Root Root => uC_Rules1.Root;

        private void authorityManagement()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(authorityManagement));
                return;
            }
            if (!(AlcSystem.Instance.GetUserAuthority() == UserAuthority.OPERATOR.ToString()))
            {
                this.uC_Rules1.Enabled = true;
            }
            else
            {
                this.uC_Rules1.Enabled = false;
            }
        }
    }
}
