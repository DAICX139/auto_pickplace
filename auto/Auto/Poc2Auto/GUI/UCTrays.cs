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
            plcDriver = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Handler.ToString()) as AdsDriverClient;
            if (plcDriver != null) 
                plcDriver.OnInitOk += () => { TraysStatusJuageMent(); };

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
        }

        private void Cells_Tray5MouseUp(object sender, MouseEventArgs e)
        {
            if (plcDriver == null) return;
            uC_Tray5.Selectable = false;
            if (AlcSystem.Instance.ShowMsgBox($"确定清空Pass2盘当前选择的数据吗？", "Question", icon: AlcMsgBoxIcon.Question, buttons: AlcMsgBoxButtons.YesNo) == AlcMsgBoxResult.No)
                return;
            var result = plcDriver.WriteTrayData(uC_Tray5.TrayID, ULoadRData, out string message);
            if (!result)
            {
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray5.TrayID} Tray盘数据下发失败: \r\n\r\n{message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                DragonDbHelper.ClearTrayData((int)(TrayName)uC_Tray5.TrayID);
                TrayManager.Trays[(TrayName)uC_Tray5.TrayID].SetData(ULoadRData);
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
            if (AlcSystem.Instance.ShowMsgBox($"确定清空Pass1盘当前选择的数据吗？", "Question", icon: AlcMsgBoxIcon.Question, buttons: AlcMsgBoxButtons.YesNo) == AlcMsgBoxResult.No)
                return;
            var result = plcDriver.WriteTrayData(uC_Tray4.TrayID, ULoadLData, out string message);
            if (!result)
            {
                AlcSystem.Instance.ShowMsgBox($"{(TrayName)uC_Tray4.TrayID} Tray盘数据下发失败: \r\n\r\n{message}", "Error", icon: AlcMsgBoxIcon.Error);
            }
            else
            {
                DragonDbHelper.ClearTrayData((int)(TrayName)uC_Tray4.TrayID);
                TrayManager.Trays[(TrayName)uC_Tray4.TrayID].SetData(ULoadLData);
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
            var data = SetTrayData(uC_Tray3);

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
            var data = SetTrayData(uC_Tray2);

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
            var data = SetTrayData(uC_Tray1);
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
                int[,] data = new int[Tray.ROW, Tray.COL];
                for (int i = 0; i < Tray.ROW; i++)
                    for (int j = 0; j < Tray.COL; j++)
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
                for (int i = 0; i < Tray.ROW; i++)
                    for (int j = 0; j < Tray.COL; j++)
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

        void loadBinRegion(int TrayID)
        {
            var binregion = DragonDbHelper.GetBinRegion(TrayID);
            foreach (var bin in binregion)
            {
                var bincolor = DragonDbHelper.GetBinColor(bin.Bin);
                for (int i = bin.StartRow; i <= bin.EndRow; i++)
                {
                    for (int j = bin.StartColumn; j <= bin.EndColumn; j++)
                    {
                        switch (TrayID)
                        {
                            case (int)TrayName.LoadL:
                                uC_Tray1.GetCell(i, j).Style.BackColor = bincolor; break;
                            case (int)TrayName.LoadR:
                                uC_Tray2.GetCell(i, j).Style.BackColor = bincolor; break;
                            case (int)TrayName.NG:
                                uC_Tray3.GetCell(i, j).Style.BackColor = bincolor; break;
                            case (int)TrayName.UnloadL:
                                uC_Tray4.GetCell(i, j).Style.BackColor = bincolor; break;
                            case (int)TrayName.UnloadR:
                                uC_Tray5.GetCell(i, j).Style.BackColor = bincolor; break;
                            default:
                                break;
                        }
                    }
                }
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

        private int[,] SetTrayData(UC_Tray tray)
        {
            int[,] result = new int[Tray.ROW, Tray.COL];
            var data = DragonDbHelper.GetTrayData(tray.TrayID);
            if (data == null) return result;
            if (tray.SelectedRegion.X == -1 || tray.SelectedRegion.Y == -1
                || tray.SelectedRegion.Bottom == -1 || tray.SelectedRegion.Right == -1)
                return result;

            foreach (var _ in data)
                result[_.Row, _.Column] = 1;
            if (result[tray.SelectedRegion.Y, tray.SelectedRegion.X] == 1 &&
                result[tray.SelectedRegion.Bottom, tray.SelectedRegion.Right] == 1)
            {
                for (int i = 0; i < Tray.ROW; i++)
                    for (int j = 0; j < Tray.COL; j++)
                        if (result[i, j] == 1 && tray.GetCell(i, j).Selected)
                            result[i, j] = 0;
            }
            else
            {
                for (int i = 0; i < Tray.ROW; i++)
                    for (int j = 0; j < Tray.COL; j++)
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

        private void ckbxDisplay_CheckedChanged_1(object sender, EventArgs e)
        {
            //if (!ckbxDisplay.Checked)
            //{
            //    uC_Tray1.Selectable = true;
            //    uC_Tray2.Selectable = true;
            //    uC_Tray3.Selectable = true;
            //
            //    uC_Tray1.EnableUpdate = false;
            //    uC_Tray2.EnableUpdate = false;
            //    uC_Tray3.EnableUpdate = false;
            //    uC_Tray4.EnableUpdate = false;
            //    uC_Tray5.EnableUpdate = false;
            //
            //    uC_Tray1.ClearColor();
            //    uC_Tray1.SelectedRegion = new Rectangle(-1, -1, -1, -1);
            //    uC_Tray2.ClearColor();
            //    uC_Tray1.SelectedRegion = new Rectangle(-1, -1, -1, -1);
            //    uC_Tray3.ClearColor();
            //    uC_Tray3.SelectedRegion = new Rectangle(-1, -1, -1, -1);
            //    uC_Tray4.ClearColor();
            //    uC_Tray4.SelectedRegion = new Rectangle(-1, -1, -1, -1);
            //    uC_Tray5.ClearColor();
            //    uC_Tray5.SelectedRegion = new Rectangle(-1, -1, -1, -1);
            //
            //    btnLoadTray1Set.Enabled = true;
            //    btnLoadTray2Set.Enabled = true;
            //    btnNGTraySet.Enabled = true;
            //    btnPassTray1Set.Enabled = true;
            //    btnPassTray2Set.Enabled = true;
            //}
            //else
            //{
            //    btnLoadTray1Set.Enabled = false;
            //    btnLoadTray2Set.Enabled = false;
            //    btnNGTraySet.Enabled = false;
            //    btnPassTray1Set.Enabled = false;
            //    btnPassTray2Set.Enabled = false;
            //
            //    uC_Tray1.ClearColor();
            //    uC_Tray1.Selectable = false;
            //    uC_Tray1.EnableUpdate = true;
            //    uC_Tray1.SelectedRegion = new Rectangle(-1, -1, -1, -1);
            //
            //    uC_Tray2.ClearColor();
            //    uC_Tray2.Selectable = false;
            //    uC_Tray2.EnableUpdate = true;
            //    uC_Tray2.SelectedRegion = new Rectangle(-1, -1, -1, -1);
            //
            //    uC_Tray3.ClearColor();
            //    uC_Tray3.Selectable = false;
            //    uC_Tray3.EnableUpdate = true;
            //    uC_Tray3.SelectedRegion = new Rectangle(-1, -1, -1, -1);
            //
            //    uC_Tray4.ClearColor();
            //    uC_Tray4.Selectable = false;
            //    uC_Tray4.EnableUpdate = true;
            //    uC_Tray4.SelectedRegion = new Rectangle(-1, -1, -1, -1);
            //
            //    uC_Tray5.ClearColor();
            //    uC_Tray5.Selectable = false;
            //    uC_Tray5.EnableUpdate = true;
            //    uC_Tray5.SelectedRegion = new Rectangle(-1, -1, -1, -1);
            //}
        }

        private void TraysStatusJuageMent()
        {
            if (plcDriver == null)
                return;
            List<TrayName> Load = new List<TrayName>() { TrayName.LoadL,TrayName.LoadR};
            List<TrayName> Unload = new List<TrayName>() { TrayName.NG,TrayName.UnloadL,TrayName.UnloadR};
            Task.Run(() =>
            {
                while (true)
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

    }

}
