using System.Windows.Forms;
using AlcUtility;
using AlcUtility.Plugin;
using WeifenLuo.WinFormsUI.Docking;

namespace Poc2Auto.HandlerPLC
{
    public partial class FormHandler : DockContent
    {
        public FormHandler(PluginBase plugin)
        {
            InitializeComponent();
            ucModeUI1.BindData(plugin);
        }
    }
}
