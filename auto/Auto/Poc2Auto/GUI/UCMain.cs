using Poc2Auto.Database;
using System.Windows.Forms;
using CYGKit.Factory.Lot;
using Poc2Auto.Model;
using AlcUtility;
using System;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using Poc2Auto.MTCP;
using Newtonsoft.Json;
using System.Collections.Generic;
using CYGKit.Factory.DataBase;
using System.IO;
using HalconDotNet;
using System.Drawing;
using Poc2Auto.GUI.FormMode;
using System.Threading;
using System.Threading.Tasks;

namespace Poc2Auto.GUI
{
    public partial class UCMain : UserControl
    {
        string currLotId = "";
        //Handler PlcDriver
        private static AdsDriverClient HandlerClient = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Handler.ToString()) as AdsDriverClient;
        private static AdsDriverClient TesterClient = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Tester.ToString()) as AdsDriverClient;
        public static UCMain Instance { get; } = new UCMain();
        public bool IsLoadLTray => rdbtnLoadLTray.Checked;

        private PlcMode testerMode;
        private PlcMode handlerMode;

        //模式窗体
        private FMAutoMark fMAutoMark;
        private FMDifferentTray fMDifferentTray;
        private FMGRR fMGRR;
        private FMSelectSN fMSelectSN;

        private bool IsExistDutsInLoadTray(int trayID)
        {
            for (int i = 0; i < Tray.ROW; i++)
            {
                for (int j = 0; j < Tray.COL; j++)
                {
                    if (DragonDbHelper.GetTrayData(trayID, i, j) != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private UCMain()
        {
            InitializeComponent();
            Dock = DockStyle.Fill;
            uphChart1.BindDatabase<DragonContext, Product>();
            var ucLot = new UC_Lot<DragonContext, LotInfo>
            {
                Dock = DockStyle.Fill,
                Row = 4,
                Column = 2
            };
            panelLotInfo.Controls.Add(ucLot);
            EventCenter.ShowErrorMsgs += ucErrorList1.ShowErrorList;

            //lot start/end时MTCP信息发送
            ucLot.Startlot += Startlot;
            ucLot.Endlot += Endlot;
            Overall.InitOk += () => { currLotId = Overall.LotInfo?.LotID; };

            //权限管理
            authorityManagement();
            AlcSystem.Instance.UserAuthorityChanged += (o, n) => { authorityManagement(); };

            rdbtnLoadLTray.Checked = true;
            fMDifferentTray = new FMDifferentTray(HandlerClient) { StartPosition = FormStartPosition.CenterParent };
            fMAutoMark = new FMAutoMark(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };

            fMGRR = new FMGRR(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };
            fMSelectSN = new FMSelectSN(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };

            //隐藏挑选Bin界面tab页
            tabControl1.TabPages.Remove(tabPagSelectBin);
            EventCenter.StateChanged += StateChanged;
            StateChanged();

            if (AlcSystem.Instance.GetSystemStatus() == SYSTEM_STATUS.Running)
            {
                this.rBtnLiveCamera1.Visible = false;
                this.rBtnLiveCamera2.Visible = false;
                this.rBtnLiveCamera3.Visible = false;
            }
        }

        private void Endlot()
        {
            //每一个lot结束后清空各工站的统计数据
            ucStations1.ClearStationStat();
            //给MTCP发送lot结束信息
            if (ConfigMgr.Instance.EnableClientMTCP)
            {
                if (MTCPHelper.SendMTCPLotEnd(out int eCode, out string eString))
                {
                    EventCenter.ProcessInfo?.Invoke($"MTCP LotEnd发送成功", ErrorLevel.Debug);
                    //清除本地bin统计数据
                    MTCPHelper.ClearTotalBinStats();
                    //清除数据库bin统计数据
                    DragonDbHelper.ClearToatlBinStats();
                }
                else
                {
                    EventCenter.ProcessInfo?.Invoke($"MTCP LotEnd 发送失败", ErrorLevel.Fatal);
                    AlcSystem.Instance.ShowMsgBox($"MTCP LotEnd send failed, error code:{eCode}, error message:{eString}.", "MTCP", icon: AlcMsgBoxIcon.Error);
                }
            }
            GenCSV();
        }

        private void Startlot()
        {
            //给MTCP发送lot结束信息
            if (ConfigMgr.Instance.EnableClientMTCP)
            {
                if (MTCPHelper.SendMTCPLotStart(out int eCode, out string eString))
                    EventCenter.ProcessInfo?.Invoke($"MTCP LotStart 发送成功", ErrorLevel.Debug);
                else
                {
                    EventCenter.ProcessInfo?.Invoke($"MTCP LotStart 发送失败", ErrorLevel.Fatal);
                    AlcSystem.Instance.ShowMsgBox($"MTCP LotStart send failed, error code:{eCode}, error message:{eString}.", "MTCP", icon: AlcMsgBoxIcon.Error);
                }
            }
            currLotId = Overall.LotInfo?.LotID;
        }

        private void authorityManagement()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(authorityManagement));
                return;
            }

            if (AlcSystem.Instance.GetUserAuthority() == UserAuthority.OPERATOR.ToString())
            {
                ucStations1.AuthorityCtrl = false;
                uphChart1.AuthorityCtrl = false;
            }
            else
            {
                ucStations1.AuthorityCtrl = true;
                uphChart1.AuthorityCtrl = true;
            }
        }

        private void GenCSV()
        {
            var lotbinStats = DragonDbHelper.GetLotBinStats(currLotId);
            //var stationBinStats = DragonDbHelper.GetStationBinStats(Overall.LotInfo.LotID);
            foreach (var lotbinStat in lotbinStats)
            {
                var stats = JsonConvert.DeserializeObject<List<BinStat>>(lotbinStat.StrBinStats);
                if (stats == null)
                    return;
                bool isFirstRow = true;
                string dataStr;
                string path = $"{Application.StartupPath}\\UiParamFiles\\";
                string file = path + DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss") + "_" + currLotId;
                FileStream fs = new FileStream(file, FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                var total = 0;
                foreach (var stat in stats)
                {
                    total += stat.Count;
                }
                foreach (var stat in stats)
                {
                    if (isFirstRow)
                    {
                        //行标题
                        isFirstRow = false;
                        string rowHead = $"Bin,Count,Total,Percent";
                        sw.WriteLine(rowHead);
                    }
                    //内容
                    var percent = ((float)stat.Count / total).ToString("0.00%");
                    dataStr = stat.Bin.ToString() + "," + stat.Count.ToString() + "," + total + "," + percent;
                    sw.WriteLine(dataStr);
                }
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }
        public void SetImage(HWindow wind,HObject image)
        {
            wind.SetDraw("margin");
            wind.SetColor("green");
            HOperatorSet.GetImageSize(image,out HTuple width,out HTuple height);
            HOperatorSet.SetPart(wind, 0, 0, height, width);
            HOperatorSet.DispObj(image, wind);

        }
        public void ShowObject(int index,HObject obj)
        {
            if(obj==null|| !obj.IsInitialized())
            {
                return;
            }
            if(index==0)
            {
                HOperatorSet.DispObj(obj, hWindowControl1.HalconWindow);
            }
            else if (index == 1)
            {
                HOperatorSet.DispObj(obj, hWindowControl2.HalconWindow);
            }
            else
            {
                HOperatorSet.DispObj(obj, hWindowControl3.HalconWindow);
            }
        }
        public void ShowImage(int index, HObject obj)
        {
            if (obj == null || !obj.IsInitialized())
            {
                return;
            }
            if (index == 0)
            {
                SetImage(hWindowControl1.HalconWindow, obj);
            }
            else if (index ==1)
            {
                SetImage(hWindowControl2.HalconWindow, obj);
            }
            else
            {
                SetImage(hWindowControl3.HalconWindow, obj);
            }
        }
        public void ShowLable(int index, bool state)
        {

        }
        public void ShowMessage(int index, string msg)
        {
            if (index == 0)
            {
                HOperatorSet.SetFont(hWindowControl1.HalconWindow, "楷体-25");
                hWindowControl1.HalconWindow.SetTposition(10, 100);
                hWindowControl1.HalconWindow.WriteString(msg);
            }
            else if (index == 1)
            {
                HOperatorSet.SetFont(hWindowControl1.HalconWindow, "楷体-25");
                hWindowControl2.HalconWindow.SetTposition(10, 100);
                hWindowControl2.HalconWindow.WriteString(msg);
            }
            else
            {
                HOperatorSet.SetFont(hWindowControl1.HalconWindow, "楷体-25");
                hWindowControl3.HalconWindow.SetTposition(10, 100);
                hWindowControl3.HalconWindow.WriteString(msg);
            }
           
        }
        public void AddInfo(string camera,string info, bool err)
        {
             
            ListViewItem item = new ListViewItem(DateTime.Now.ToLongTimeString().ToString());
            if (err)
            {
                item.ForeColor = Color.Orange;
            }
            else
            {
                item.ForeColor = Color.White;
            }
            //给每一项里面添加信息
            item.SubItems.Add((Convert.ToInt32(camera)+1).ToString());
            item.SubItems.Add(info);
            this.listView_vision_message.Items.Add(item);
            this.listView_vision_message.Items[this.listView_vision_message.Items.Count - 1].EnsureVisible();

        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            var result = AlcSystem.Instance.ShowMsgBox("确认运行正常生产吗？", "提醒", AlcMsgBoxButtons.YesNo, icon: AlcMsgBoxIcon.Question);
            if (result == AlcMsgBoxResult.No)
                return;

            //数据检查
            if (!IsExistDutsInLoadTray((int)TrayName.LoadL) && !IsExistDutsInLoadTray((int)TrayName.LoadR))
            {
                AlcSystem.Instance.ShowMsgBox("未设置上料盘数据，请设置数据后再操作！", "提醒");
                return;
            }

            Task.Run(new Action(
             () =>
             {
                 if (Stop())
                 {
                     if (RunModeMgr.AutoNormal(HandlerClient, out string msg))
                     {
                         RunModeMgr.RunMode = RunMode.AutoNormal;
                         RunModeMgr.Running = false;
                         AlcSystem.Instance.ShowMsgBox("OK", "Information");
                         Reset();

                         AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Start);
                     }
                     else
                         AlcSystem.Instance.ShowMsgBox($"Fail, {msg}", "Error", icon: AlcMsgBoxIcon.Error);
                 }
             }));

        }

        private void btnSelectBinOK_Click(object sender, EventArgs e)
        {
            if (HandlerClient == null)
                return;

            Task.Run(new Action(
             () =>
             {
                 if (Stop())
                 {
                     if (RunModeMgr.AutoSelectBin(HandlerClient, ucModeParams_SelectDut_Bin1.ParamData, out string message))
                     {
                         RunModeMgr.RunMode = RunMode.AutoSelectBin;
                         RunModeMgr.Running = false;
                         AlcSystem.Instance.ShowMsgBox("OK", "Information");
                         Reset();
                     }
                     else
                     {
                         AlcSystem.Instance.ShowMsgBox($"Fail, {message}", "Error", icon: AlcMsgBoxIcon.Error);
                     }
                 }
             }));
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            if (HandlerClient == null)
                return;

            Task.Run(new Action(
             () =>
             {
                 if (Stop())
                 {
                     if (RunModeMgr.ManualMode(HandlerClient, out string message))
                     {
                         RunModeMgr.RunMode = RunMode.Manual;
                         RunModeMgr.Running = false;
                         AlcSystem.Instance.ShowMsgBox("OK", "Information");
                     }
                     else
                     {
                         AlcSystem.Instance.ShowMsgBox($"Fail, {message}", "Error", icon: AlcMsgBoxIcon.Error);
                     }

                 }
             }));
        }

        private void btnDifferentTray_Click(object sender, EventArgs e)
        {
            if (HandlerClient == null)
                return;
            fMDifferentTray.ShowDialog();
        }

        private void btnMark_Click(object sender, EventArgs e)
        {
            if (HandlerClient == null)
                return;
            fMAutoMark.ShowDialog();
        }

        private void StateChanged()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { StateChanged(); }));
                return;
            }
            var state = AlcSystem.Instance.GetSystemStatus();
            if (state == SYSTEM_STATUS.Idle || state ==  SYSTEM_STATUS.Stopping)
            {
                btnNormal.Enabled = true;
                btnManual.Enabled = true;
                btnSelectSN.Enabled = true;
                btnGRR.Enabled = true;
                ucSocket_New1.Enabled = true; 
            }
            else
            {
                btnNormal.Enabled = false;
                btnManual.Enabled = false;
                btnSelectSN.Enabled = false;
                btnGRR.Enabled = false;
                ucSocket_New1.Enabled = false;
            }
        }

        public void SetButtonColor(Button button)
        {
            List<Button> buttons = new List<Button>()
            {
                btnManual,
                btnNormal,
                btnGRR,
                btnSelectSN,
                btnSelectBinOK,
            };
            button.BackColor = Color.Green;
            foreach (var btn in buttons)
            {
                if (btn == button) continue;
                btn.UseVisualStyleBackColor = true;
            }
        }

        public void SetButtonColor()
        {
            List<Button> buttons = new List<Button>()
            {
                btnManual,
                btnNormal,
                btnGRR,
                btnSelectSN,
                btnSelectBinOK,
            };
            foreach (var btn in buttons)
            {
                btn.UseVisualStyleBackColor = true;
            }
        }
        public bool Stop()
        {
            if (AlcSystem.Instance.GetSystemStatus() == SYSTEM_STATUS.Idle)
                return true;

            AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Stop);
            DateTime _now = DateTime.Now;
            while (true)
            {
                bool delay = (DateTime.Now - _now).TotalSeconds >= 10F;
                if (AlcSystem.Instance.GetSystemStatus() == SYSTEM_STATUS.Idle)
                    return true;
                else if (delay)
                {
                    AlcSystem.Instance.Error("停止失败，请手动停止或重试！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
                    return false;
                }
                Thread.Sleep(1000);
            }
        }

        public bool Reset()
        {
            AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Reset);
            DateTime _now = DateTime.Now;
            while (true)
            {
                bool delay = (DateTime.Now - _now).TotalSeconds >= 30F;
                if (AlcSystem.Instance.GetSystemStatus() == SYSTEM_STATUS.Ready)
                    return true;
                else if (delay)
                {
                    AlcSystem.Instance.Error("复位失败，请手动复位并开始！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
                    return false;
                }
                Thread.Sleep(1000);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (HandlerClient == null || TesterClient == null)
                    return;
                handlerMode = (PlcMode)(uint)HandlerClient?.ReadObject("GVL_MachineInterface.MachineCmd.nMode", typeof(uint));
                testerMode = (PlcMode)(uint)TesterClient?.ReadObject("GVL_MachineInterface.MachineCmd.nMode", typeof(uint));

                UpdateSubMode();

                // 实时 控件刷新
                if (AlcSystem.Instance.GetSystemStatus() == SYSTEM_STATUS.Running)
                {
                    this.rBtnLiveCamera1.Visible = false;
                    this.rBtnLiveCamera2.Visible = false;
                    this.rBtnLiveCamera3.Visible = false;
                }
                else
                {
                    this.rBtnLiveCamera1.Visible = true;
                    this.rBtnLiveCamera2.Visible = true;
                    this.rBtnLiveCamera3.Visible = true;
                }
            }
            catch
            {

            }

        }

        private void UCMain_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void UpdateSubMode()
        {
            if (handlerMode == PlcMode.ManualMode && testerMode == PlcMode.ManualMode)
            {
                SetButtonColor(btnManual);
            }
            else if (handlerMode == PlcMode.AutoMode && testerMode == PlcMode.AutoMode)
            {
                var handlerSubMode = (uint)HandlerClient.ReadObject("GVL_MachineInterface.MachineCmd.nAutoSubMode", typeof(uint));
                var testerSubMode = (uint)TesterClient.ReadObject("GVL_MachineInterface.MachineCmd.nAutoSubMode", typeof(uint));
                if (handlerSubMode == RunModeMgr.Auto_Normal)
                {
                    SetButtonColor(btnNormal);
                }
                else if (handlerSubMode == RunModeMgr.Auto_GoldenDut)
                {
                    SetButtonColor(btnGRR);
                }
                else
                {
                    SetButtonColor();
                }
            }
            else if(handlerMode == PlcMode.AutoMode)
            {
                var handlerSubMode = (uint)HandlerClient.ReadObject("GVL_MachineInterface.MachineCmd.nAutoSubMode", typeof(uint));
                if (handlerSubMode == RunModeMgr.Auto_SelectDUT)
                {
                    SetButtonColor(btnSelectSN);
                }
                else
                {
                    SetButtonColor();
                }
            }
            else
            {
                SetButtonColor();
            }
            
        }

        private void btnSelectSN_Click(object sender, EventArgs e)
        {
            if (fMSelectSN == null || fMSelectSN.IsDisposed)
            {
                fMSelectSN = new FMSelectSN(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };
            }
            fMSelectSN.ShowDialog();
        }

        private void btnGRR_Click(object sender, EventArgs e)
        {
            if (fMGRR == null || fMGRR.IsDisposed)
            {
                fMGRR = new FMGRR(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };
            }
            fMGRR.ShowDialog();
        }


        private void rBtnLiveCamera1_Click(object sender, EventArgs e)
        {
            EventCenter.LeftCamera?.Invoke(1, rBtnLiveCamera1.Checked);
        }

        private void rBtnLiveCamera2_Click(object sender, EventArgs e)
        {
            EventCenter.RightCamera?.Invoke(2, rBtnLiveCamera2.Checked);
        }

        private void rBtnLiveCamera3_Click(object sender, EventArgs e)
        {
            EventCenter.DownCamera?.Invoke(3, rBtnLiveCamera3.Checked);
        }
    }

}
