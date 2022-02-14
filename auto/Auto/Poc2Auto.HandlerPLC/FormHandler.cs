using System.Windows.Forms;
using AlcUtility;
using AlcUtility.Plugin;
using WeifenLuo.WinFormsUI.Docking;
using Poc2Auto.HandlerPLC;
using Poc2Auto.Common;

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
