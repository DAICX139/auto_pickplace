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
using Poc2Auto.GUI.UCModeUI.UCAxisesCylinders;

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
        private FMGRR fMGRR;

        UC_Lot<DragonContext, LotInfo> ucLot;
        public static LotInfo LotInfo;
        private UCMain()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            Dock = DockStyle.Fill;
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            uphChart1.BindDatabase<DragonContext, Product>();
            ucLot = new UC_Lot<DragonContext, LotInfo>
            {
                Dock = DockStyle.Fill,
                Row = 4,
                Column = 2
            };
            panelLotInfo.Controls.Add(ucLot);
            //EventCenter.ShowErrorMsgs += ucErrorList1.ShowErrorList;

            //lot start/end时MTCP信息发送
            ucLot.Startlot += Startlot;
            ucLot.Endlot += Endlot;
            Overall.InitOk += () => { currLotId = Overall.LotInfo?.LotID; };

            //权限管理
            authorityManagement();
            AlcSystem.Instance.UserAuthorityChanged += (o, n) => { authorityManagement(); };

            rdbtnLoadLTray.Checked = true;

            fMGRR = new FMGRR(HandlerClient) { StartPosition = FormStartPosition.CenterScreen };

            //隐藏挑选Bin界面tab页
            tabControl1.TabPages.Remove(tabPagSelectBin);
            //EventCenter.StateChanged += StateChanged;
            StateChanged();

            if (AlcSystem.Instance.GetSystemStatus() == SYSTEM_STATUS.Running)
            {
                this.rBtnLiveCamera1.Visible = false;
                this.rBtnLiveCamera2.Visible = false;
                this.rBtnLiveCamera3.Visible = false;
            }
            if (HandlerClient!=null)
            {
                if (HandlerClient.IsInitOk)
                {
                    BindDataSource(HandlerClient);
                }
                else
                {
                    HandlerClient.OnInitOk += () => { BindDataSource(HandlerClient); };
                }
            }
            if (TesterClient != null)
            {
                if (TesterClient.IsInitOk)
                {
                    BindDataSource(TesterClient);
                }
                else
                {
                    TesterClient.OnInitOk += () => { BindDataSource(TesterClient); };
                }
            }
            //RemovePage(tabPageTrayConfig);
            LotInfo = DragonDbHelper.GetLotInfo();

            //Tray盘配置读取
            ReadCfg();
        }

        public bool LoadBigTray
        {
            get => radioButtonLoadBigTray.Checked;
            set
            {
                if (value == radioButtonLoadBigTray.Checked) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => LoadBigTray = value));
                    return;
                }
                radioButtonLoadBigTray.Checked = value;
            }
        }

        public bool LoadSmallTray
        {
            get => radioButtonLoadSmallTray.Checked;
            set
            {
                if (value == radioButtonLoadSmallTray.Checked) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => LoadSmallTray = value));
                    return;
                }
                radioButtonLoadSmallTray.Checked = value;
            }
        }

        public bool NGBigTray
        {
            get => radioButtonNGBigTray.Checked;
            set
            {
                if (value == radioButtonNGBigTray.Checked) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => NGBigTray = value));
                    return;
                }
                radioButtonNGBigTray.Checked = value;
            }
        }

        public bool NGSmallTray
        {
            get => radioButtonNGSmallTray.Checked;
            set
            {
                if (value == radioButtonNGSmallTray.Checked) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => NGSmallTray = value));
                    return;
                }
                radioButtonNGSmallTray.Checked = value;
            }
        }

        public bool UnloadBigTray
        {
            get => radioButtonUnloadBigTray.Checked;
            set
            {
                if (value == radioButtonUnloadBigTray.Checked) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => UnloadBigTray = value));
                    return;
                }
                radioButtonUnloadBigTray.Checked = value;
            }
        }

        public bool UnloadSmallTray
        {
            get => radioButtonUnloadSmallTray.Checked;
            set
            {
                if (value == radioButtonUnloadSmallTray.Checked) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => UnloadSmallTray = value));
                    return;
                }
                radioButtonUnloadSmallTray.Checked = value;
            }
        }

        private void ReadCfg()
        {
            LoadBigTray = ConfigMgr.Instance.LoadTrayConfig;
            LoadSmallTray = !ConfigMgr.Instance.LoadTrayConfig;

            NGBigTray = ConfigMgr.Instance.NGTrayConfig;
            NGSmallTray = !ConfigMgr.Instance.NGTrayConfig;

            UnloadBigTray = ConfigMgr.Instance.UnloadTrayConfig;
            UnloadSmallTray = !ConfigMgr.Instance.UnloadTrayConfig;
        }

        public void AddPage(TabPage tabPage)
        {
            if (tabControl2.TabPages.Contains(tabPage))
                return;
            if (InvokeRequired)
            {
                Invoke(new Action( () => tabControl2.TabPages.Add(tabPage)));
                return;
            }
            tabControl2.TabPages.Add(tabPage);
        }

        public void RemovePage(TabPage tabPage)
        {
            if (!tabControl2.TabPages.Contains(tabPage))
                return;
            if (InvokeRequired)
            {
                Invoke(new Action(() => tabControl2.TabPages.Remove(tabPage)));
                return;
            }
            tabControl2.TabPages.Remove(tabPage);
        }

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

        private void Endlot()
        {
            //每一个lot结束后清空各工站的统计数据
            ucStations1.ClearStationStat();
            //清除界面上的显示数据
            EventCenter.EndLot?.Invoke();
            //给MTCP发送lot结束信息
            if (ConfigMgr.Instance.EnableClientMTCP)
            {
                if (MTCPHelper.SendMTCPLotEnd(out int eCode, out string eString))
                {
                    EventCenter.ProcessInfo?.Invoke($"MTCP LotEnd发送成功", ErrorLevel.INFO);
                    //清除上料个数
                    RunModeMgr.LoadCount = 0;
                    //清除本地bin统计数据
                    MTCPHelper.ClearTotalBinStats();
                    //清除数据库bin统计数据
                    DragonDbHelper.ClearToatlBinStats();
                }
                else
                {
                    EventCenter.ProcessInfo?.Invoke($"MTCP LotEnd 发送失败!", ErrorLevel.FATAL);
                    AlcSystem.Instance.ShowMsgBox($"MTCP LotEnd 发送失败!, 错误码:{eCode}", "MTCP", icon: AlcMsgBoxIcon.Error);
                    
                    ucLot.FormOnAccept(LotInfo);
                }
            }
            GenCSV();
        }

        private void Startlot()
        {
            if (Overall.LotInfo != null)
            {
                LotInfo = Overall.LotInfo;
                //给MTCP发送lot开始信息
                if (ConfigMgr.Instance.EnableClientMTCP)
                {
                    if (MTCPHelper.SendMTCPLotStart(out int eCode, out string eString))
                        EventCenter.ProcessInfo?.Invoke($"MTCP LotStart 发送成功", ErrorLevel.INFO);
                    else
                    {
                        if (eCode == -1)
                        {
                            AlcSystem.Instance.ShowMsgBox($"MTCP LotStart 发送失败! 错误码:{eCode}，请检查Macmini端到ALC工控机的网线是否连接好！", "MTCP", icon: AlcMsgBoxIcon.Error);
                            EventCenter.ProcessInfo?.Invoke($"MTCP LotStart 发送失败! 请检查Macmini端到ALC工控机的网线是否连接好！", ErrorLevel.FATAL);

                        }
                        else
                        {
                            AlcSystem.Instance.ShowMsgBox($"MTCP LotStart 发送失败! 错误码:{eCode}", "MTCP", icon: AlcMsgBoxIcon.Error);
                            EventCenter.ProcessInfo?.Invoke($"MTCP LotStart 发送失败!", ErrorLevel.FATAL);
                        }
                        //发送失败 清除界面上值显示
                        ucLot.ClearAllData();
                    }
                }
                currLotId = Overall.LotInfo?.LotID;
            }
            else
            {
                //ucLot.FormOnAccept(LotInfo);
                ucLot.ClearAllData();
                EventCenter.ProcessInfo?.Invoke($"Lot 信息为空,请新建Lot", ErrorLevel.WARNING);
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
            {
                ucStations1.AuthorityCtrl = false;
                uphChart1.AuthorityCtrl = false;
                RemovePage(tabPageControl);
                //RemovePage(tabPageTrayConfig);
            }
            else
            {
                ucStations1.AuthorityCtrl = true;
                uphChart1.AuthorityCtrl = true;
                AddPage(tabPageControl);
                //AddPage(tabPageTrayConfig);
            }
        }

        private void GenCSV()
        {
            try
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
            catch (Exception ex)
            {
                 
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
            if (!RunModeMgr.HandlerAllAxisHomed || !RunModeMgr.TesterAllAxisHomed)
            {
                AlcSystem.Instance.ShowMsgBox("机台待复位，请复位完成后再操作！", "提醒");
                return;
            }
            var result = AlcSystem.Instance.ShowMsgBox("确认切换至生产模式吗？", "提醒", AlcMsgBoxButtons.YesNo, icon: AlcMsgBoxIcon.Question);
            if (result == AlcMsgBoxResult.No)
                return;
            //数据检查
            if (!IsExistDutsInLoadTray((int)TrayName.Load1) && !IsExistDutsInLoadTray((int)TrayName.Load2))
            {
                AlcSystem.Instance.ShowMsgBox("未设置上料盘数据，请设置数据后再操作！", "提醒");
                return;
            }

            Task.Run(new Action(
             () =>
             {
                 if (Stop(CtrlType.Both))
                 {
                     if (RunModeMgr.AutoNormal(HandlerClient, out string msg))
                     {
                         RunModeMgr.RunMode = RunMode.AutoNormal;
                         RunModeMgr.Running = false;
                         RunModeMgr.OriginValue = false;
                         TesterClient?.WriteObject(RunModeMgr.Name_CompleteFinish, false);
                         AlcSystem.Instance.ShowMsgBox("OK", "Information");
                         //Reset();

                         //AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Reset);
                     }
                     else
                         AlcSystem.Instance.ShowMsgBox($"生产模式切换失败!, {msg}", "Error", icon: AlcMsgBoxIcon.Error);
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
                 if (Stop(CtrlType.Both))
                 {
                     if (RunModeMgr.AutoSelectBin(HandlerClient, ucModeParams_SelectDut_Bin1.ParamData, out string message))
                     {
                         RunModeMgr.RunMode = RunMode.AutoSelectBin;
                         RunModeMgr.Running = false;
                         RunModeMgr.OriginValue = false;
                         AlcSystem.Instance.ShowMsgBox("OK", "Information");
                         //Reset();
                     }
                     else
                     {
                         AlcSystem.Instance.ShowMsgBox($"Fail, {message}", "Error", icon: AlcMsgBoxIcon.Error);
                     }
                 }
             }));
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
                btnGRR.Enabled = true;
                ucSocket_New1.Enabled = true;
            }
            else
            {
                btnNormal.Enabled = false;
                btnGRR.Enabled = false;
                ucSocket_New1.Enabled = false;
            }
        }

        public void SetButtonColor(Button button)
        {
            List<Button> buttons = new List<Button>()
            {
                btnNormal,
                btnGRR,
                btnSelectBinOK,
                btnAudit,
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
                btnNormal,
                btnGRR,
                btnSelectBinOK,
                btnAudit,
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

        /// <summary>
        /// 分插件Stop
        /// </summary>
        /// <returns></returns>
        public bool Stop(CtrlType type)
        {
            if (type == CtrlType.Both)
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
            else if (type == CtrlType.Handler)
            {
                if ((SYSTEM_STATUS)RunModeMgr.HandlerCurrentState == SYSTEM_STATUS.Idle)
                    return true;
                HandlerClient?.WriteObject(RunModeMgr.Name_StateCmd, (uint)SYSTEM_EVENT.Stop);
                AlcSystem.Instance.Log("向Handler下发停止命令", "界面操作", AlcErrorLevel.DEBUG);
                DateTime _now = DateTime.Now;
                while (true)
                {
                    bool delay = (DateTime.Now - _now).TotalSeconds >= 10F;
                    if ((SYSTEM_STATUS)RunModeMgr.HandlerCurrentState == SYSTEM_STATUS.Idle)
                        return true;
                    else if (delay)
                    {
                        AlcSystem.Instance.Error("Handler停止失败，请手动停止或重试！", 0, AlcErrorLevel.WARN, ModuleTypes.Handler.ToString());
                        return false;
                    }
                    Thread.Sleep(1000);
                }
            }
            else if (type == CtrlType.Tester)
            {
                if ((SYSTEM_STATUS)RunModeMgr.TesterCurrentState == SYSTEM_STATUS.Idle)
                    return true;
                TesterClient?.WriteObject(RunModeMgr.Name_StateCmd, (uint)SYSTEM_EVENT.Stop);
                AlcSystem.Instance.Log("向Tester下发停止命令", "界面操作", AlcErrorLevel.DEBUG);
                DateTime _now = DateTime.Now;
                while (true)
                {
                    bool delay = (DateTime.Now - _now).TotalSeconds >= 10F;
                    if ((SYSTEM_STATUS)RunModeMgr.TesterCurrentState == SYSTEM_STATUS.Idle)
                        return true;
                    else if (delay)
                    {
                        AlcSystem.Instance.Error("Tester停止失败，请手动停止或重试！", 0, AlcErrorLevel.WARN, ModuleTypes.Tester.ToString());
                        return false;
                    }
                    Thread.Sleep(1000);
                }
            }
            else
                return false;

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
                if (!HandlerClient.IsInitOk || !TesterClient.IsInitOk)
                    return;

                bool isTesting = false;

                foreach (var name in StationManager.RotationStations)
                {
                    if (!isTesting && (StationManager.Stations[name].Status == StationStatus.Testing))
                    {
                        isTesting = true;
                        break;
                    }
                }
                handlerMode = (PlcMode)RunModeMgr.HandlerMode;
                testerMode = (PlcMode)RunModeMgr.TesterMode;

                UpdateSubMode();
                var status = AlcSystem.Instance.GetSystemStatus();
                // 实时控件刷新
                if (status == SYSTEM_STATUS.Running || AlcSystem.Instance.GetUserAuthority() == UserAuthority.OPERATOR.ToString() || isTesting)
                {
                    this.rBtnLiveCamera1.Visible = false;
                    this.rBtnLiveCamera2.Visible = false;
                    this.rBtnLiveCamera3.Visible = false;

                    ucSemiAutoControl1.EnableButton(false);

                    btnNormal.Enabled = false;
                    btnGRR.Enabled = false;
                    ucSocket_New1.Enabled = false;
                    btnAudit.Enabled = false;

                    groupBox6.Enabled = false;
                    groupBox7.Enabled = false;
                }
                else
                {
                    this.rBtnLiveCamera1.Visible = true;
                    this.rBtnLiveCamera2.Visible = true;
                    this.rBtnLiveCamera3.Visible = true;

                    ucSemiAutoControl1.EnableButton(true);

                    btnNormal.Enabled = true;
                    btnGRR.Enabled = true;
                    btnAudit.Enabled = true;
                    ucSocket_New1.Enabled = true;

                    groupBox6.Enabled = false;
                    groupBox7.Enabled = false;
                }
                ucLot.AuthorityCtrl = !(status == SYSTEM_STATUS.Running);

                UpdateTrayConfg();
            }
            catch
            {

            }

        }

        private void UCMain_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            tabControl1.SelectedTab = tabPageTMTestResult;
            tabControl1.SelectedTab = tabPageVision;
        }

        private void UpdateSubMode()
        {
            if (handlerMode == PlcMode.ManualMode && testerMode == PlcMode.ManualMode)
            {
                //SetButtonColor(btnSemiAuto);
            }
            else if (handlerMode == PlcMode.AutoMode && testerMode == PlcMode.AutoMode)
            {
                //var handlerSubMode = (uint)HandlerClient.ReadObject("GVL_MachineInterface.MachineCmd.nAutoSubMode", typeof(uint));
                //var testerSubMode = (uint)TesterClient.ReadObject("GVL_MachineInterface.MachineCmd.nAutoSubMode", typeof(uint));

                var testerSubMode = RunModeMgr.TesterSubMode;
                var handlerSubMode = RunModeMgr.HandlerSubMode;

                if (handlerSubMode == RunModeMgr.Func_AutoNormal && testerSubMode == RunModeMgr.Func_AutoNormal)
                {
                    SetButtonColor(btnNormal);
                }
                else if (handlerSubMode == RunModeMgr.Func_AutoGoldenDut && testerSubMode == RunModeMgr.Func_AutoNormal)
                {
                    SetButtonColor(btnGRR);
                }
                else if (handlerSubMode == RunModeMgr.Func_Audit && testerSubMode == RunModeMgr.Func_Audit)
                {
                    SetButtonColor(btnAudit);
                }
                else
                {
                    SetButtonColor();
                }
            }
            else if(handlerMode == PlcMode.AutoMode)
            {
                //var handlerSubMode = (uint)HandlerClient.ReadObject("GVL_MachineInterface.MachineCmd.nAutoSubMode", typeof(uint));
                var handlerSubMode = RunModeMgr.HandlerSubMode;
                if (handlerSubMode == RunModeMgr.Func_AutoSelectDUT)
                {
                    //SetButtonColor(btnSelectSN);
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

        private void btnGRR_Click(object sender, EventArgs e)
        {
            if (!RunModeMgr.HandlerAllAxisHomed || !RunModeMgr.TesterAllAxisHomed)
            {
                AlcSystem.Instance.ShowMsgBox("机台待复位，请复位完成后再操作！", "提醒");
                return;
            }
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

        private void btnAudit_Click(object sender, EventArgs e)
        {
            if (!RunModeMgr.HandlerAllAxisHomed || !RunModeMgr.TesterAllAxisHomed)
            {
                AlcSystem.Instance.ShowMsgBox("机台待复位，请复位完成后再操作！", "提醒");
                return;
            }
            var result = AlcSystem.Instance.ShowMsgBox("确认切换至Audit吗？", "提醒", AlcMsgBoxButtons.YesNo, icon: AlcMsgBoxIcon.Question);
            if (result == AlcMsgBoxResult.No)
                return;

            //数据检查
 
            Task.Run(new Action(
             () =>
             {
                 if (Stop(CtrlType.Both))
                 {
                     if (RunModeMgr.Audit(HandlerClient, out string msg) && RunModeMgr.Audit(TesterClient, out string msg1))
                     {
                         RunModeMgr.RunMode = RunMode.AutoAudit;
                         RunModeMgr.Running = false;
                         RunModeMgr.OriginValue = false;
                         TesterClient?.WriteObject(RunModeMgr.Name_CompleteFinish, false);
                         string path;
                         if (!File.Exists(ConfigMgr.Instance.AuditFile))
                         {
                             path = RunModeMgr.AuditFile;
                         }
                         else
                         {
                             path = ConfigMgr.Instance.AuditFile;
                         }
                         RunModeMgr.CSVToDic(path);
                     }
                     else
                         AlcSystem.Instance.ShowMsgBox($"Audit 切换失败!, {msg}", "Error", icon: AlcMsgBoxIcon.Error);
                 }
             }));
        }
        int index = 0;
        private void BindDataSource(AdsDriverClient client )
        {
            if (client.Name == ModuleTypes.Handler.ToString())
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < tableLayoutPanel9.ColumnCount; j++)
                    {
                        var offset = i * tableLayoutPanel9.ColumnCount + j + 1;
                        var axis = (UC_SingleAxis_New)tableLayoutPanel9.GetControlFromPosition(j, i);
                        if (axis != null)
                        {
                            axis.DataSource = client.GetSingleAxisCtrl(offset);
                        }
                    }
                }

                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < tableLayoutPanel10.ColumnCount; col++)
                    {
                        var offset = row * tableLayoutPanel10.ColumnCount + col + 1;
                        var cylinder = (UC_Cylinder_New)tableLayoutPanel10.GetControlFromPosition(col, row);
                        if (cylinder != null)
                        {
                            cylinder.Cylinder = client.GetCylinderCtrl(offset);
                        }
                    }
                }
            }
            else
            {
                
                for (int i = 3; i < tableLayoutPanel9.RowCount; i++)
                {
                    for (int j = 0; j < tableLayoutPanel9.ColumnCount; j++)
                    {
                        index++;
                        var axis = (UC_SingleAxis_New)tableLayoutPanel9.GetControlFromPosition(j, i);
                        if (axis != null )
                        {
                            if (index == 2)
                            {
                                axis.DataSource = client.GetSingleAxisCtrl(index + 1);
                            }
                            else if (i ==4 )
                            {
                                axis.DataSource = client.GetSingleAxisCtrl(i);
                            }
                            else
                                axis.DataSource = client.GetSingleAxisCtrl(index);

                        }
                    }
                }

                index = 0;
                for (int row = 2; row < tableLayoutPanel10.RowCount; row++)
                {
                    bool first = true;
                    for (int col = 2; col < tableLayoutPanel10.ColumnCount; col++)
                    {
                        if (row > 2)
                        {
                            if (first)
                            {
                                col = 0;
                                first = false;
                            }
                            
                        }
                        
                        index++;
                        var cylinder = (UC_Cylinder_New)tableLayoutPanel10.GetControlFromPosition(col, row);
                        if (cylinder != null)
                        {
                            cylinder.Cylinder = client.GetCylinderCtrl(index);
                        }
                    }
                }
            }
        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = tabControl2.SelectedIndex;
            for (int i = 0; i < tableLayoutPanel9.RowCount; i++)
            {
                for (int j = 0; j < tableLayoutPanel9.ColumnCount; j++)
                {
                    var axis = (UC_SingleAxis_New)tableLayoutPanel9.GetControlFromPosition(j, i);
                    if (axis != null)
                    {
                        axis.IsInnerUpdatingOpen = index == 1 ?  true : false;
                    }
                }
            }


            for (int row = 0; row < tableLayoutPanel10.RowCount; row++)
            {
                for (int col = 0; col < tableLayoutPanel10.ColumnCount; col++)
                {
                    var cylinder = (UC_Cylinder_New)tableLayoutPanel10.GetControlFromPosition(col, row);
                    if (cylinder != null)
                    {
                        cylinder.EnableUpdate = index == 1 ? true : false;
                    }
                }
            }
        }

        private void RadbtnLoadCheckChanged(object sender, EventArgs e)
        {
            var button = (RadioButton)sender;
            if (button.Name == "radioButtonLoadBigTray")
            {
                RunModeMgr.IsLoadBigTray = button.Checked;
                ConfigMgr.Instance.LoadTrayConfig = button.Checked;
            }
            else
            {
                RunModeMgr.IsLoadBigTray = !button.Checked;
                ConfigMgr.Instance.LoadTrayConfig = !button.Checked;
            }
        }

        private void RadbtnNGCheckChanged(object sender, EventArgs e)
        {
            var button = (RadioButton)sender;
            if (button.Name == "radioButtonNGBigTray")
            {
                RunModeMgr.IsNGBigTray = button.Checked;
                ConfigMgr.Instance.NGTrayConfig = button.Checked;
            }
            else
            {
                RunModeMgr.IsNGBigTray = !button.Checked;
                ConfigMgr.Instance.NGTrayConfig = !button.Checked;
            }
        }

        private void RadbtnUnlloadCheckChanged(object sender, EventArgs e)
        {
            var button = (RadioButton)sender;
            if (button.Name == "radioButtonUnloadBigTray")
            {
                RunModeMgr.IsUnloadBigTray = button.Checked;
                ConfigMgr.Instance.UnloadTrayConfig = button.Checked;
            }
            else
            {
                RunModeMgr.IsUnloadBigTray = !button.Checked;
                ConfigMgr.Instance.UnloadTrayConfig = !button.Checked;
            }
        }

        private void UpdateTrayConfg()
        {
            if (AlcSystem.Instance.GetSystemStatus() == SYSTEM_STATUS.Idle && AlcSystem.Instance.GetUserAuthority() == UserAuthority.ADMINISTRATOR.ToString())
            {
                AddPage(tabPageTrayConfig);
            }
            else
            {
                RemovePage(tabPageTrayConfig);
            }
        }
    }

}
