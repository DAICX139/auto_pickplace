using System.Windows.Forms;
using AlcUtility;
using AlcUtility.Plugin;
using CYGKit.GUI;
using Poc2Auto.Common;
using Poc2Auto.Model;
using WeifenLuo.WinFormsUI.Docking;
using NetAndEvent.PlcDriver;
using System;
using Poc2Auto.MTCP;
using Poc2Auto.Database;
using Poc2Auto.GUI;

namespace Poc2Auto.TM
{
    public partial class Form1 : DockContent
    {
        private TMPlugin _plugin;
        FMDutStatList fMDutStatList;
        private static AdsDriverClient HandlerClient = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Handler.ToString()) as AdsDriverClient;
        public Form1(PluginBase plugin)
        {
            InitializeComponent();

            var uc = new UC_StatesOfPlugin();
            uc.Dock = DockStyle.Fill;
            uc.BindPlugin(plugin);
            Controls.Add(uc);
            _plugin = plugin as TMPlugin;
            comboBox1.BindEnum<StationName>();

            fMDutStatList  = new FMDutStatList();
        } 
        private void TestStart_Click(object sender, System.EventArgs e)
        {
            //RunModeMgr.RunMode = RunMode.AutoAudit;
            var a = Overall.AuditSocketID;
            Overall.Stat.Passed++;
            return;
            var stationName = (StationName)comboBox1.SelectedValue;
            var station = StationManager.Stations[stationName];
            station.PutDut(0, 0, new Dut { Barcode = "123" });

            station.SocketGroup.Enable = true;
            station.Status = StationStatus.RotateDone;
            _plugin.TestStart(stationName);


            //station.Save();
            //station.Save();
 
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            Overall.Stat.Failed++;
            //RunModeMgr.RunMode = RunMode.AutoMark;
            return;
            var stationName = (StationName)comboBox1.SelectedValue;
            var station = StationManager.Stations[stationName];
            station.Load();
        }

        private void buttonLotStart_Click(object sender, EventArgs e)
        {
            if (MTCPHelper.SendMTCPLotStart(out int eCode, out string eString))
                EventCenter.ProcessInfo?.Invoke($"MTCP LotStart 发送成功", ErrorLevel.INFO);
            else
            {
                EventCenter.ProcessInfo?.Invoke($"MTCP LotStart 发送失败", ErrorLevel.FATAL);
                AlcSystem.Instance.ShowMsgBox($"MTCP LotStart send failed, error code:{eCode}, error message:{eString}.", "MTCP", icon: AlcMsgBoxIcon.Error);
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            var SN = $"FNG143405RS1L3PMZ";
            if (MTCPHelper.SendMTCPLotLoad(SN, "S"+2, out int eCode, out string eString))
                EventCenter.ProcessInfo?.Invoke($"MTCP Load 发送成功，Socket SN:{SN}", ErrorLevel.INFO);
            else
            {
                EventCenter.ProcessInfo?.Invoke($"MTCP Load 发送失败，Socket SN:{SN}", ErrorLevel.FATAL);
                //Error($"MTCP Load send failed, error code:{eCode}, error message:{eString}.", 0, AlcErrorLevel.WARN);
            }

            
        }
        int i;
        string GetBin;
        private void buttonUnload_Click(object sender, EventArgs e)
        {
            var SN = $"FNG143405RS1L3PMZ";
            if (MTCPHelper.SendMTCPLotUnload(SN, "S" + 2, true ? 1 : 0, out int eCode, out string eString, out string bin))
            {
                GetBin = bin;
                EventCenter.ProcessInfo?.Invoke($"MTCP Unload 发送成功，Socket SN:{SN}", ErrorLevel.INFO);
                AlcSystem.Instance.Log($"从MTCP获取到的总结果为{bin}", "MTCP");
                EventCenter.ProcessInfo?.Invoke($"从MTCP获取到的总结果为 {bin}", ErrorLevel.INFO);
            }
            else
            {
                GetBin = bin;
                if (eCode == -1)
                {
                    AlcSystem.Instance.Error($"ALC与MTCP未连接成功.", 0, AlcErrorLevel.WARN, "MTCP");
                }
                else
                {
                    EventCenter.ProcessInfo?.Invoke($"MTCP Unload 发送失败，Socket SN:{SN}", ErrorLevel.FATAL);
                    AlcSystem.Instance.Error($"MTCP Unload send failed, error code:{eCode}, error message:{eString}.", 0, AlcErrorLevel.WARN, "MTCP");
                }

            }
        }

        private void buttonLotEnd_Click(object sender, EventArgs e)
        {
            if (MTCPHelper.SendMTCPLotEnd(out int eCode, out string eString))
            {
                EventCenter.ProcessInfo?.Invoke($"MTCP LotEnd发送成功", ErrorLevel.INFO);
                //清除本地bin统计数据
                MTCPHelper.ClearTotalBinStats();
                //清除数据库bin统计数据
                DragonDbHelper.ClearToatlBinStats();
            }
            else
            {
                EventCenter.ProcessInfo?.Invoke($"MTCP LotEnd 发送失败", ErrorLevel.FATAL);
                AlcSystem.Instance.ShowMsgBox($"MTCP LotEnd send failed, error code:{eCode}, error message:{eString}.", "MTCP", icon: AlcMsgBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DragonDbHelper.SetDutTotal("1111224", "123asdfghjkt", 2, StationName.Test1_LIVW);
            //DragonDbHelper.SetDutTotal("1111223", "123asdfghjkr", 1, StationName.Test1_LIVW);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DragonDbHelper.SetDutTotal("1111224", "123asdfghjk5", 1, StationName.Test2_NFBP);
            //DragonDbHelper.SetDutTotal("1111223", "123asdfghjkt", 4, StationName.Test2_NFBP);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DragonDbHelper.SetDutTotal("1111224", "123asdfghjk6", 4, StationName.Test3_KYRL);
            //DragonDbHelper.SetDutTotal("1111223", "123asdfghjkr", 3, StationName.Test3_KYROL);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            DragonDbHelper.SetDutTotal("1111224", "123asdfghjk7", 3, StationName.Test4_BMPF);
            //DragonDbHelper.SetDutTotal("1111223", "123asdfghjku", 2, StationName.Test4_BMPF);

        }

        
        private void button7_Click(object sender, EventArgs e)
        {
            //DragonDbHelper.UpdateDutBin("1111224", "123asdfghjk6", 4);
            //var a = DragonDbHelper.GetStationBinTotal();

            if (fMDutStatList == null || fMDutStatList.IsDisposed)
            {
                fMDutStatList = new FMDutStatList() { StartPosition = FormStartPosition.CenterScreen };
            }
            fMDutStatList.ShowDialog();
        }

        private void button8_Click(object sender, EventArgs e)
        {
        }
    }
}
