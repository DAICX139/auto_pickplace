using System;
using System.IO;
using System.Windows.Forms;
using AlcUtility;
using AlcUtility.Plugin;
using Poc2Auto.Common;

namespace DragonFlex.GUI.Factory
{
    public partial class UCModeUI : UserControl
    {
        public UCModeUI()
        {
            InitializeComponent();
            ucModeManual1.RecipeSave += recipeSave;
            
        }

        UC_StateOfPlcPlugin stateOfPlcPlugin;
        public void BindData(PluginBase plugin)
        {
            if (plugin == null)
                return;

            stateOfPlcPlugin = new UC_StateOfPlcPlugin();
            stateOfPlcPlugin.Dock = DockStyle.Top;
            stateOfPlcPlugin.BindPlugin(plugin);
            tableLayoutPanel1.Controls.Add(stateOfPlcPlugin, 0, 0);
            var plcDriver = plugin.PlcDriver;
            ucModeManual1.RecipePath = $"{Application.StartupPath}\\paramFiles\\HandlerConfigFile\\";
            
            ucModeManual1.DefaultRecipe = $"{Application.StartupPath}\\paramFiles\\HandlerConfigFile\\" + ConfigMgr.Instance.HandlerDefaultRecipe;
            ucModeManual1.SemiAutoPath = $"{Application.StartupPath}\\paramFiles\\HandlerSemiAutoConfigFile\\HanderPLC_SemiAutoCfgParams.xml";
            ucModeManual1.DioPath = Application.StartupPath + @"\UiParamFiles\DIOmap.xlsx";
            ucModeManual1.CYL_COUNT = 10;
            ucModeManual1.AXIS_COUNT = 6;

            if (plcDriver != null)
                plcDriver.OnInitOk += () => { ucModeManual1.BindData(plcDriver); };

            //权限管理
            authorityManagement();
            AlcSystem.Instance.UserAuthorityChanged += (o, n) => { authorityManagement(); };
        }

        private void authorityManagement()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(authorityManagement));
                return;
            }

            if (AlcSystem.Instance.GetUserAuthority() == UserAuthority.OPERATOR.ToString())
                stateOfPlcPlugin.Enabled = false;
            else
                stateOfPlcPlugin.Enabled = true;
        }

        private void recipeSave(string name)
        {
            ConfigMgr.Instance.HandlerDefaultRecipe = Path.GetFileName(name);
        }
    }
}
