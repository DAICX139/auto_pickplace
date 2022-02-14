using System.Windows.Forms;
using AlcUtility;
using AlcUtility.Plugin;
using NetAndEvent.PlcDriver;
using WeifenLuo.WinFormsUI.Docking;
using System;
using Poc2Auto.Common;
using System.IO;

namespace Poc2Auto.RotationPLC
{
    public partial class FormTester : DockContent
    {
        private PluginBase _plugin;
        UC_StateOfPlcPlugin stateOfPlcPlugin;
        public FormTester(PluginBase plugin)
        {
            InitializeComponent();
            stateOfPlcPlugin = new UC_StateOfPlcPlugin();
            stateOfPlcPlugin.Dock = DockStyle.Top;
            stateOfPlcPlugin.BindPlugin(plugin);
            //Controls.Add(uc);
            tableLayoutPanel1.Controls.Add(stateOfPlcPlugin, 0, 0);
            _plugin = plugin;
            var plcDriver = _plugin.PlcDriver;

            //权限管理
            authorityManagement();
            AlcSystem.Instance.UserAuthorityChanged += (o, n) => { authorityManagement(); };

            ucModeManual1.RecipeSave += recipeSave;
            ucModeManual1.CYL_COUNT = 9;
            ucModeManual1.AXIS_COUNT = 3;
            ucModeManual1.RecipePath = $"{Application.StartupPath}\\paramFiles\\RotationConfigFile\\";
            ucModeManual1.DefaultRecipe = $"{Application.StartupPath}\\paramFiles\\RotationConfigFile\\" + ConfigMgr.Instance.TesterDefaultRecipe;
            ucModeManual1.DioPath = Application.StartupPath + @"\UiParamFiles\RotationPLC_DIOmap.xlsx";
            if (plcDriver != null)
            {
                plcDriver.OnInitOk += () => { ucModeManual1.BindData(plcDriver); };
                if (plcDriver.IsInitOk)
                {
                    ucModeManual1.BindData(plcDriver);
                }
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text)) return;
            var client = _plugin.PlcDriver as AdsDriverClient;
            var value = client.ReadObject(textBoxName.Text, typeof(bool));// textBoxType.Text);
            textBoxValue.Text = value.ToString();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text)) return;
            var client = _plugin.PlcDriver as AdsDriverClient;
            var value = client.WriteObject(textBoxName.Text, textBoxValue.Text);
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
            ConfigMgr.Instance.TesterDefaultRecipe = Path.GetFileName(name); ;
        }


    }
}
