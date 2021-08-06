using System;
using System.Windows.Forms;
using AlcUtility;
using AlcUtility.Plugin;
using CYGKit.GUI;
using Poc2Auto.Common;
using Poc2Auto.Model;
using WeifenLuo.WinFormsUI.Docking;

namespace Poc2Auto.TM
{
    public partial class Form1 : DockContent
    {
        private TMPlugin _plugin;

        public Form1(PluginBase plugin)
        {
            InitializeComponent();

            var uc = new UC_StatesOfPlugin();
            uc.Dock = DockStyle.Fill;
            uc.BindPlugin(plugin);
            Controls.Add(uc);
            _plugin = plugin as TMPlugin;
            comboBox1.BindEnum<StationName>();
        }

        private void TestStart_Click(object sender, System.EventArgs e)
        {
            RunModeMgr.FailDut++;
            return;

            var stationName = (StationName)comboBox1.SelectedValue;
            var station = StationManager.Stations[stationName];
            station.PutDut(0, 0, new Dut { Barcode = "123" });
            station.Status = StationStatus.RotateDone;
            _plugin.TestStart(stationName);
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            RunModeMgr.YieldDut++;
            return;
            var stationName = (StationName)comboBox1.SelectedValue;
            var station = StationManager.Stations[stationName];

            station.Status = StationStatus.Done;
            station.TestTimer.Enabled = false;
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            RunModeMgr.UnloadTotal++;
           
            var code = "FNG117401THPX" + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");

            //if (RunModeMgr.CustomMode.HasFlag(CustomMode.NoSn))
            //{
            //    //socket.Dut.Barcode = Overall.ScanResult;
            //}
        }
    }
}
