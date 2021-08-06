using CYGKit.Factory.ConditionEditor;
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
        }

        public Root Root => uC_Rules1.Root;
    }
}
