using Poc2Auto.Database;
using System;
using System.Drawing;
using System.Windows.Forms;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using System.Threading.Tasks;
using System.Threading;
using AlcUtility;
using System.Collections.Generic;
using Poc2Auto.Model;
using CYGKit.AdsProtocol;
using CYGKit.Factory.TableView;
using System.Linq;
using CYGKit.GUI;

namespace Poc2Auto.GUI
{
    public partial class UCTrays : UserControl
    {
        private AdsDriverClient plcDriver;

        public UCTrays()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            uC_Tray1.BindDataBase<DragonContext>();
            uC_Tray2.BindDataBase<DragonContext>();
            uC_Tray3.BindDataBase<DragonContext>();
            uC_Tray4.BindDataBase<DragonContext>();
            uC_Tray5.BindDataBase<DragonContext>();

            //Tray盘上加载Bin值以及对应的Bincolor
            SetBinRegionTextAndColor(uC_Tray3.TrayID, uC_Tray3);
            SetBinRegionTextAndColor(uC_Tray4.TrayID, uC_Tray4);
            SetBinRegionTextAndColor(uC_Tray5.TrayID, uC_Tray5);

            plcDriver = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Handler.ToString()) as AdsDriverClient;
            if (plcDriver != null)
            {
                if (plcDriver.IsInitOk)
                {
                    TraysStatusJuageMent();
                }
                else
                {
                    plcDriver.OnInitOk += () => { TraysStatusJuageMent(); };
                }
            }

            uC_Tray1.cells.MouseDown += Cells_Tray1MouseDown;
            uC_Tray1.cells.MouseUp += Cells_Tray1MouseUp;

            uC_Tray2.cells.MouseDown += Cells_Tray2MouseDown;
            uC_Tray2.cells.MouseUp += Cells_Tray2MouseUp;

            uC_Tray3.cells.MouseDown += Cells_Tray3MouseDown;
            uC_Tray3.cells.MouseUp += Cells_Tray3MouseUp;

            uC_Tray4.cells.MouseDown += Cells_Tray4MouseDown;
            uC_Tray4.cells.MouseUp += Cells_Tray4MouseUp;

            uC_Tray5.cells.MouseDown += Cells_Tray5MouseDown;
            uC_Tray5.cells.MouseUp += Cells_Tray5MouseUp;

            EventCenter.OK2TrayBinSet += OK2TrayBinSet;
            EventCenter.NGTrayBinSet += NGTrayBinSet;
            EventCenter.OK1TrayBinSet += OK1TrayBinSet;
            RunModeMgr.LoadTraySizeChanged += LoadTraySizeChanged;
            RunModeMgr.NGTraySizeChanged += NGTraySizeChanged;
            RunModeMgr.UnloadTraySizeChanged += UnloadTraySizeChanged;
        }

        private void OK2TrayBinSet()
        {
            SetBinRegionTextAndColor(uC_Tray5.TrayID, uC_Tray5); 
        }

        private void NGTrayBinSet()
        {
            SetBinRegionTextAndColor(uC_Tray3.TrayID, uC_Tray3);
        }

        private void OK1TrayBinSet()
        {
            SetBinRegionTextAndColor(uC_Tray4.TrayID, uC_Tray4);
        }

        private void Cells_Tray5MouseUp(object sender, MouseEventArgs e)
        {
            if (plcDriver == null) return;
            uC_Tray5.Selectable = false;
            //if (AlcSystem.Instance.ShowMsgBox($"确定清空Pass2盘当前选择的数据吗？", "Question", icon: AlcMsgBoxIcon.Question, buttons: AlcMsgBoxButtons.YesNo) == AlcMsgBoxResult.No)
            //    return;
            //var result = plcDriver.WriteTrayData(uC_Tray5.TrayID, ULoadRData, out string message);

            var data = SetTrayData(uC_Tray5, false);

            var result = plcDriver.WriteTrayData(uC_Tray5.TrayID, data, out string message);
            if (!result)
            {
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray5.TrayID} Tray盘数据下发失败: \r\n\r\n{message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                DragonDbHelper.ClearTrayData((int)(TrayName)uC_Tray5.TrayID);
                TrayManager.Trays[(TrayName)uC_Tray5.TrayID].SetData(data);
                //AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray3.TrayID} Tray盘数据下发成功!", "Information", icon: AlcMsgBoxIcon.Information);
            }
            uC_Tray5.EnableUpdate = true;
            uC_Tray5.SelectedRegion = new Rectangle(x: -1, -1, -1, -1);
        }

        private void Cells_Tray5MouseDown(object sender, MouseEventArgs e)
        {
            uC_Tray5.Selectable = true;
            uC_Tray5.EnableUpdate = false;
            uC_Tray5.SelectedRegion = new Rectangle(-1, -1, -1, -1);
        }

        private void Cells_Tray4MouseUp(object sender, MouseEventArgs e)
        {
            if (plcDriver == null) return;
            uC_Tray4.Selectable = false;
            //if (AlcSystem.Instance.ShowMsgBox($"确定清空Pass1盘当前选择的数据吗？", "Question", icon: AlcMsgBoxIcon.Question, buttons: AlcMsgBoxButtons.YesNo) == AlcMsgBoxResult.No)
            //    return;
            //var result = plcDriver.WriteTrayData(uC_Tray4.TrayID, ULoadLData, out string message);

            var data = SetTrayData(uC_Tray4, false);

            var result = plcDriver.WriteTrayData(uC_Tray4.TrayID, data, out string message);
            if (!result)
            {
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray4.TrayID} Tray盘数据下发失败: \r\n\r\n{message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                DragonDbHelper.ClearTrayData((int)(TrayName)uC_Tray4.TrayID);
                TrayManager.Trays[(TrayName)uC_Tray4.TrayID].SetData(data);
                //AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray3.TrayID} Tray盘数据下发成功!", "Information", icon: AlcMsgBoxIcon.Information);
            }
            uC_Tray4.EnableUpdate = true;
            uC_Tray4.SelectedRegion = new Rectangle(x: -1, -1, -1, -1);
        }

        private void Cells_Tray4MouseDown(object sender, MouseEventArgs e)
        {
            uC_Tray4.Selectable = true;
            uC_Tray4.EnableUpdate = false;
            uC_Tray4.SelectedRegion = new Rectangle(-1, -1, -1, -1);
        }

        private void Cells_Tray3MouseUp(object sender, MouseEventArgs e)
        {
            if (plcDriver == null) return;
            uC_Tray3.Selectable = false;
            var data = SetTrayData(uC_Tray3,true);

            var result = plcDriver.WriteTrayData(uC_Tray3.TrayID, data, out string message);
            if (!result)
            {
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray3.TrayID} Tray盘数据下发失败: \r\n\r\n{message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                DragonDbHelper.ClearTrayData((int)(TrayName)uC_Tray3.TrayID);
                TrayManager.Trays[(TrayName)uC_Tray3.TrayID].SetData(data);
                //AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray3.TrayID} Tray盘数据下发成功!", "Information", icon: AlcMsgBoxIcon.Information);
            }
            uC_Tray3.EnableUpdate = true;
            uC_Tray3.SelectedRegion = new Rectangle(x: -1, -1, -1, -1);
        }

        private void Cells_Tray3MouseDown(object sender, MouseEventArgs e)
        {
            uC_Tray3.Selectable = true;
            uC_Tray3.EnableUpdate = false;
            uC_Tray3.SelectedRegion = new Rectangle(-1, -1, -1, -1);
        }

        private void Cells_Tray2MouseUp(object sender, MouseEventArgs e)
        {
            if (plcDriver == null) return;
            uC_Tray2.Selectable = false;
            var data = SetTrayData(uC_Tray2, true);

            var result = plcDriver.WriteTrayData(uC_Tray2.TrayID, data, out string message);
            if (!result)
            {
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray2.TrayID} Tray盘数据下发失败: \r\n\r\n{message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                DragonDbHelper.ClearTrayData((int)(TrayName)uC_Tray2.TrayID);
                TrayManager.Trays[(TrayName)uC_Tray2.TrayID].SetData(data);
                //AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray2.TrayID} Tray盘数据下发成功!", "Information", icon: AlcMsgBoxIcon.Information);
            }
            uC_Tray2.EnableUpdate = true;
            uC_Tray2.SelectedRegion = new Rectangle(x: -1, -1, -1, -1);
        }

        private void Cells_Tray2MouseDown(object sender, MouseEventArgs e)
        {
            uC_Tray2.Selectable = true;
            uC_Tray2.EnableUpdate = false;
            uC_Tray2.SelectedRegion = new Rectangle(-1, -1, -1, -1);
        }

        private void Cells_Tray1MouseUp(object sender, MouseEventArgs e)
        {
            if (plcDriver == null) return;
            uC_Tray1.Selectable = false;
            var data = SetTrayData(uC_Tray1, true);
            var result = plcDriver.WriteTrayData(uC_Tray1.TrayID, data, out string message);
            if (!result)
            {
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray1.TrayID} Tray盘数据下发失败: \r\n\r\n{message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                DragonDbHelper.ClearTrayData((int)(TrayName)uC_Tray1.TrayID);
                TrayManager.Trays[(TrayName)uC_Tray1.TrayID].SetData(data);
                //AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray1.TrayID} Tray盘数据下发成功!", "Information", icon: AlcMsgBoxIcon.Information);
            }
            uC_Tray1.EnableUpdate = true;
            uC_Tray1.SelectedRegion = new Rectangle( x:-1, -1, -1, -1);
        }

        private void Cells_Tray1MouseDown(object sender, MouseEventArgs e)
        {
            uC_Tray1.Selectable = true;
            uC_Tray1.EnableUpdate = false;
            uC_Tray1.SelectedRegion = new Rectangle(-1, -1, -1, -1);
        }

        public int[,] LoadLTrayData
        {
            get
            {
                int row;
                int col;
                int[,] data = new int[Tray.ROW, Tray.COL];
                if (!RunModeMgr.IsLoadBigTray)
                {
                    row = Tray.S_ROW;
                    col = Tray.S_Col;
                }
                else
                {
                    row = Tray.ROW;
                    col = Tray.COL;
                }
                for (int i = 0; i < row; i++)
                    for (int j = 0; j < col; j++)
                    {
                        if (uC_Tray1.GetCell(i, j).Style.BackColor != Color.White)
                            data[i, j] = 1;
                    }
                        
                return data;
            }
        }

        public int[,] LoadRTrayData
        {
            get
            {
                int[,] data = new int[Tray.ROW, Tray.COL];
                for (int i = 0; i < Tray.ROW; i++)
                    for (int j = 0; j < Tray.COL; j++)
                        if (uC_Tray2.GetCell(i, j).Style.BackColor != Color.White)
                            data[i, j] = 1;
                return data;
            }
        }

        public int[,] NGTrayData
        {
            get
            {
                int[,] data = new int[Tray.ROW, Tray.COL];
                for (int i = 0; i < Tray.ROW; i++)
                    for (int j = 0; j < Tray.COL; j++)
                        if (uC_Tray3.GetCell(i, j).Style.BackColor != Color.White)
                            data[i, j] = 1;
                return data;
            }
        }

        public int LoadLRegionNumber
        {
            get
            {
                int num = 0;
                int row;
                int col;
                if (!RunModeMgr.IsLoadBigTray)
                {
                    row = Tray.S_ROW;
                    col = Tray.S_Col;
                }
                else
                {
                    row = Tray.ROW;
                    col = Tray.COL;
                }
                
                for (int i = 0; i < row; i++)
                    for (int j = 0; j < col; j++)
                        if (uC_Tray1.GetCell(i, j).Style.BackColor != Color.White)
                            num++;
                return num;
            }
        }

        public int LoadRRegionNumber
        {
            get
            {
                int num = 0;
                for (int i = 0; i < Tray.ROW; i++)
                    for (int j = 0; j < Tray.COL; j++)
                        if (uC_Tray2.GetCell(i, j).Style.BackColor != Color.White)
                            num++;
                return num;
            }
        }

        public int[,] ULoadLData
        {
            get
            {
                int[,] data = new int[Tray.ROW, Tray.COL];
                for (int i = 0; i < Tray.ROW; i++)
                    for (int j = 0; j < Tray.COL; j++)
                        if (uC_Tray4.GetCell(i, j).Selected)
                            data[i, j] = 0;
                return data;
            }
        }

        public int[,] ULoadRData
        {
            get
            {
                int[,] data = new int[Tray.ROW, Tray.COL];
                for (int i = 0; i < Tray.ROW; i++)
                    for (int j = 0; j < Tray.COL; j++)
                        if (uC_Tray5.GetCell(i, j).Selected)
                            data[i, j] = 0;
                return data;
            }
        }
 
        private const int _minSpace = 10, _maxSpace = 30;

        private void UCTrays_SizeChanged(object sender, EventArgs e)
        {
            var space = (Size.Width - uC_Tray1.Width * 5) / 6;
            if (space < _minSpace) space = _minSpace;
            if (space > _maxSpace) space = _maxSpace;
            var x0 = (Size.Width - uC_Tray1.Width * 5 - space * 4) / 2;
            if (x0 < 0) x0 = 0;
            var x1 = x0 + uC_Tray1.Width + space;
            var x2 = x1 + uC_Tray1.Width + space;
            var x3 = x2 + uC_Tray1.Width + space;
            var x4 = x3 + uC_Tray1.Width + space;

            var y = (Size.Height - uC_Tray1.Height) / 2;
            if (y < 0) y = 0;

            uC_Tray1.Location = new Point(x0, y);
            uC_Tray2.Location = new Point(x1, y);
            uC_Tray3.Location = new Point(x2, y);
            uC_Tray4.Location = new Point(x3, y);
            uC_Tray5.Location = new Point(x4, y);
        }

        private int[,] SetTrayData(UC_Tray tray ,bool loadTray)
        {
            int[,] result = new int[Tray.ROW, Tray.COL];
            var data = DragonDbHelper.GetTrayData(tray.TrayID);
            if (data == null) return result;
            if (tray.SelectedRegion.X == -1 || tray.SelectedRegion.Y == -1
                || tray.SelectedRegion.Bottom == -1 || tray.SelectedRegion.Right == -1)
                return result;

            // data[,] startRow,startCol, endRow, EndCol, Bin

            int row = 0;
            int col = 0;

            if (loadTray)
            {
                if (RunModeMgr.IsLoadBigTray)
                {
                    row = Tray.ROW;
                    col = Tray.COL;
                }
                else
                {
                    row = Tray.S_ROW;
                    col = Tray.S_Col;
                }
            }
            else
            {
                row = Tray.ROW;
                col = Tray.COL;
            }

            foreach (var _ in data)
            {
                result[_.Row, _.Column] = 1; 
            }
            if (result[tray.SelectedRegion.Y, tray.SelectedRegion.X] == 1 &&
                result[tray.SelectedRegion.Bottom, tray.SelectedRegion.Right] == 1)
            {
                for (int i = 0; i < row; i++)
                    for (int j = 0; j < col; j++)
                        if (result[i, j] == 1 && tray.GetCell(i, j).Selected)
                            result[i, j] = 0;
            }
            else
            {
                for (int i = 0; i < row; i++)
                    for (int j = 0; j < col; j++)
                        if (tray.GetCell(i, j).Selected)
                            result[i, j] = 1;
            }
            return result;
        }

        private void btnLoadTray1Set_Click(object sender, EventArgs e)
        {
            if (plcDriver == null) return;

            var result = plcDriver.WriteTrayData(uC_Tray1.TrayID, LoadLTrayData, out string message);
            if (!result)
            {
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray1.TrayID} Tray盘数据下发失败: \r\n\r\n{message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                DragonDbHelper.ClearTrayData((int)(TrayName)uC_Tray1.TrayID);
                TrayManager.Trays[(TrayName)uC_Tray1.TrayID].SetData(LoadLTrayData);
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray1.TrayID} Tray盘数据下发成功!", "Information", icon: AlcMsgBoxIcon.Information);

            }
        }

        private void btnLoadTray2Set_Click(object sender, EventArgs e)
        {
            if (plcDriver == null) return;

            var result = plcDriver.WriteTrayData(uC_Tray2.TrayID, LoadRTrayData, out string message);
            if (!result)
            {
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray2.TrayID} Tray盘数据下发失败: \r\n\r\n{message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                DragonDbHelper.ClearTrayData((int)(TrayName)uC_Tray2.TrayID);
                TrayManager.Trays[(TrayName)uC_Tray2.TrayID].SetData(LoadRTrayData);
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray2.TrayID} Tray盘数据下发成功!", "Information", icon: AlcMsgBoxIcon.Information);
            }
        }

        private void btnNGTraySet_Click(object sender, EventArgs e)
        {
            if (plcDriver == null) return;
            var result = plcDriver.WriteTrayData(uC_Tray3.TrayID, NGTrayData, out string message);
            if (!result)
            {
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray3.TrayID} Tray盘数据下发失败: \r\n\r\n{message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                DragonDbHelper.ClearTrayData((int)(TrayName)uC_Tray3.TrayID);
                TrayManager.Trays[(TrayName)uC_Tray3.TrayID].SetData(NGTrayData);
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray3.TrayID} Tray盘数据下发成功!", "Information", icon: AlcMsgBoxIcon.Information);
            }
        }

        private void btnPassTray1Set_Click(object sender, EventArgs e)
        {
            if (plcDriver == null) return;

            var result = plcDriver.WriteTrayData(uC_Tray4.TrayID, new int[Tray.ROW, Tray.COL], out string message);
            if (!result)
            {
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray4.TrayID} Tray盘数据下发失败: \r\n\r\n{message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                DragonDbHelper.ClearTrayData((int)(TrayName)uC_Tray4.TrayID);
                TrayManager.Trays[(TrayName)uC_Tray4.TrayID].SetData(new int[Tray.ROW, Tray.COL]);
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray4.TrayID} Tray盘数据下发成功!", "Information", icon: AlcMsgBoxIcon.Information);
            }
        }

        private void btnPassTray2Set_Click(object sender, EventArgs e)
        {
            if (plcDriver == null) return;

            var result = plcDriver.WriteTrayData(uC_Tray5.TrayID, new int[Tray.ROW, Tray.COL], out string message);
            if (!result)
            {
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray5.TrayID} Tray盘数据下发失败: \r\n\r\n{message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                DragonDbHelper.ClearTrayData((int)(TrayName)uC_Tray5.TrayID);
                TrayManager.Trays[(TrayName)uC_Tray5.TrayID].SetData(new int[Tray.ROW, Tray.COL]);
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray5.TrayID} Tray盘数据下发成功!", "Information", icon: AlcMsgBoxIcon.Information);
            }
        }

        private void TraysStatusJuageMent()
        {
            if (plcDriver == null)
                return;

            if (RunModeMgr.IsLoadBigTray)
            {
                plcDriver?.WriteObject(RunModeMgr.TraySwitch(1), false);
                plcDriver?.WriteObject(RunModeMgr.TraySwitch(2), false);
            }
            else
            {
                plcDriver?.WriteObject(RunModeMgr.TraySwitch(1), true);
                plcDriver?.WriteObject(RunModeMgr.TraySwitch(2), true);
            }

            if (RunModeMgr.IsNGBigTray)
            {
                plcDriver?.WriteObject(RunModeMgr.TraySwitch(3), false);
            }
            else
            {
                plcDriver?.WriteObject(RunModeMgr.TraySwitch(3), true);
            }

            if (RunModeMgr.IsUnloadBigTray)
            {
                plcDriver?.WriteObject(RunModeMgr.TraySwitch(4), false);
                plcDriver?.WriteObject(RunModeMgr.TraySwitch(5), false);
            }
            else
            {
                plcDriver?.WriteObject(RunModeMgr.TraySwitch(4), true);
                plcDriver?.WriteObject(RunModeMgr.TraySwitch(5), true);
            }

            List<TrayName> Load = new List<TrayName>() { TrayName.Load1,TrayName.Load2};
            List<TrayName> Unload = new List<TrayName>() { TrayName.NG,TrayName.Pass1,TrayName.Pass2};
            Task.Run(() =>
            {
                while (false)
                {
                    if (RunModeMgr.RunMode == RunMode.AutoNormal && AlcSystem.Instance.GetSystemStatus() == SYSTEM_STATUS.Running)
                    {
                        foreach (TrayName l in Load)
                            if (plcDriver.IsConnected)
                                if ((bool)plcDriver.ReadObject(RunModeMgr.TrayInfoIsEmpty((int)l), typeof(bool)))
                                {
                                    if (StationManager.AllStationEmpty)
                                    {
                                        AlcSystem.Instance.ShowMsgBox($"{l} is empty, please change a tray!", "TrayInfo", icon: AlcMsgBoxIcon.Error);
                                        Thread.Sleep(20000);
                                    }
                                }
                        foreach (TrayName u in Unload)
                            if (plcDriver.IsConnected)
                                if ((bool)plcDriver.ReadObject(RunModeMgr.TrayInfoIsFull((int)u), typeof(bool)))
                                {
                                    AlcSystem.Instance.ShowMsgBox($"{u} is full, please change a tray!", "TrayInfo", icon: AlcMsgBoxIcon.Error);
                                    Thread.Sleep(20000);
                                }
                    }
                    Thread.Sleep(1000);
                }
            });
        }

        private void SetBinRegionTextAndColor(int trayID, GridView tray)
        {
            using (var context = new DragonContext())
            {
                //tray.SelectedRegion = new Rectangle(-1, -1, -1, -1);
                tray.ClearColor();
                tray.ClearDisplayText();
 
                var regions = DragonDbHelper.GetBinRegion(trayID);
                //展示到网格
                foreach (var region in regions)
                {
                    var binText = region.Bin.ToString();
                    //if (binText == "4") binText = "F";
                    //else if (binText == "5") binText = "99";
                    tray.SetText(binText, region.StartRow, region.StartColumn, region.EndRow, region.EndColumn);
                }
            }
        }

        private void LoadTraySizeChanged()
        {
            if (RunModeMgr.IsLoadBigTray)
            {
                this.uC_Tray1.Row = this.uC_Tray2.Row = Tray.ROW;
                this.uC_Tray1.Col = this.uC_Tray2.Col = Tray.COL;
                if (plcDriver.IsInitOk)
                {
                    plcDriver?.WriteObject(RunModeMgr.TraySwitch(1), false);
                    plcDriver?.WriteObject(RunModeMgr.TraySwitch(2), false);
                }
            }
            else
            {
                this.uC_Tray1.Row = this.uC_Tray2.Row = Tray.S_ROW;
                this.uC_Tray1.Col = this.uC_Tray2.Col  = Tray.S_Col;

                if (plcDriver.IsInitOk)
                {
                    ClearTrayData(uC_Tray1);
                    ClearTrayData(uC_Tray2);


                    var res = plcDriver?.WriteObject(RunModeMgr.TraySwitch(1), true);
                    res = plcDriver?.WriteObject(RunModeMgr.TraySwitch(2), true);
                }
            }

        }

        private void ClearTrayData(UC_Tray tray)
        {
            bool result = plcDriver.WriteTrayData(tray.TrayID, new int[Tray.ROW, Tray.COL], out string message);
            if (!result)
            {
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)tray.TrayID} Tray盘数据清空失败: \r\n\r\n{message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                DragonDbHelper.ClearTrayData((int)(TrayName)tray.TrayID);
            }

            tray.EnableUpdate = true;
            tray.SelectedRegion = new Rectangle(x: -1, -1, -1, -1);
        }

        private void NGTraySizeChanged()
        {
            if (RunModeMgr.IsNGBigTray)
            {
                this.uC_Tray3.Row = Tray.ROW;
                this.uC_Tray3.Col = Tray.COL;
                if (plcDriver.IsInitOk)
                {
                    plcDriver?.WriteObject(RunModeMgr.TraySwitch(3), false);
                    uC_Tray3.ClearDisplayText();
                }
                
            }
            else
            {
                this.uC_Tray3.Row = Tray.S_ROW;
                this.uC_Tray3.Col = Tray.S_Col;
                if (plcDriver.IsInitOk)
                {
                    ClearTrayData(uC_Tray3);
                    plcDriver?.WriteObject(RunModeMgr.TraySwitch(3), true);
                    uC_Tray3.ClearDisplayText();
                }

            }
        }

        private void UnloadTraySizeChanged()
        {
            if (RunModeMgr.IsUnloadBigTray)
            {
                this.uC_Tray4.Row = this.uC_Tray5.Row = Tray.ROW;
                this.uC_Tray4.Col = this.uC_Tray5.Col = Tray.COL;

                if (plcDriver.IsInitOk)
                {
                    plcDriver?.WriteObject(RunModeMgr.TraySwitch(4), false);
                    plcDriver?.WriteObject(RunModeMgr.TraySwitch(5), false);
                    uC_Tray4.ClearDisplayText();
                    uC_Tray5.ClearDisplayText();
                }
                
            }
            else
            {
                this.uC_Tray4.Row = this.uC_Tray5.Row = Tray.S_ROW;
                this.uC_Tray4.Col = this.uC_Tray5.Col = Tray.S_Col;

                if (plcDriver.IsInitOk)
                {
                    ClearTrayData(uC_Tray4);
                    ClearTrayData(uC_Tray5);

                    plcDriver?.WriteObject(RunModeMgr.TraySwitch(4), true);
                    plcDriver?.WriteObject(RunModeMgr.TraySwitch(5), true);
                    uC_Tray4.ClearDisplayText();
                    uC_Tray5.ClearDisplayText();
                }
        }
        }
    }

}
