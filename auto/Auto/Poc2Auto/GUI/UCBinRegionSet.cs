using AlcUtility;
using CYGKit.AdsProtocol;
using CYGKit.Common;
using CYGKit.Factory.DataBase;
using CYGKit.GUI;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using Poc2Auto.Database;
using Poc2Auto.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CYGKit.Factory.OtherUI.Test
{
    public partial class UCBinRegionSet : UserControl
    {
        public UCBinRegionSet()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            
            InitDBBinColor();
            SetBinColor();
            string defaultBin = "1";//默认bin值
            CurrentSelectBin = defaultBin;

            NGTray.cells.MouseDown += Cells_NGTrayMouseDown;
            NGTray.cells.MouseUp += Cells_NGTrayMouseUp;

            OK1Tray.cells.MouseDown += Cells_OK1TrayMouseDown;
            OK1Tray.cells.MouseUp += Cells_OK1TrayMouseUp;

            OK2Tray.cells.MouseDown += Cells_OK2TrayMouseDown;
            OK2Tray.cells.MouseUp += Cells_OK2TrayMouseUp;

            NGTray.cells.MouseClick += Cells_NGTrayClick;
            OK1Tray.cells.MouseClick += Cells_OK1TrayClick;
            OK2Tray.cells.MouseClick += Cells_OK2TrayClick;

            RunModeMgr.NGTraySizeChanged += NGTraySizeChanged;
            RunModeMgr.UnloadTraySizeChanged += UnloadTraySizeChanged;

            if (_client != null)
            {
                if (_client.IsInitOk)
                {
                    InitTraySize();
                }
                else
                {
                    _client.OnInitOk += () => { InitTraySize(); };
                }
            }

            if (ConfigMgr.Instance.NGTrayConfig)
            {
                gridNGTray.Row = Tray.ROW;
                gridNGTray.Col = Tray.COL;
            }
            else
            {
                gridNGTray.Row = Tray.S_ROW;
                gridNGTray.Col = Tray.S_Col;
            }

        }

        private void Cells_OK2TrayClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                OK2CurrentRegion = null;
            }
        }
 
        private void Cells_NGTrayClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                NGCurrentRegion = null;
            }
        }

        private void Cells_OK1TrayClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                OK1CurrentRegion = null;
            }
        }

        private void Cells_NGTrayClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Cells_OK2TrayClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public string  CurrentSelectBin;

        public Color CurrentSelectBinColor;
        /// <summary>
        /// tray盘控件
        /// </summary>
        public GridView NGTray => gridNGTray;

        /// <summary>
        /// tray盘控件
        /// </summary>
        public GridView OK1Tray => gridOK1Tray;

        /// <summary>
        /// tray盘控件
        /// </summary>
        public GridView OK2Tray => gridView1;

        private BinRegion _nGCurrentRegion;
        public BinRegion NGCurrentRegion
        {
            get { return _nGCurrentRegion; }
            set 
            { 
                _nGCurrentRegion = value;
            }
        }

        private BinRegion _OK1CurrentRegion;
        public BinRegion OK1CurrentRegion
        {
            get { return _OK1CurrentRegion; }
            set
            {
                _OK1CurrentRegion = value;
            }
        }

        private BinRegion _OK2CurrentRegion;
        public BinRegion OK2CurrentRegion
        {
            get { return _OK2CurrentRegion; }
            set
            {
                _OK2CurrentRegion = value;
            }
        }

        private List<BinRegion> _nGRegions;
        private List<BinRegion> NGRegions => new List<BinRegion>(_nGRegions);
        private List<BinRegion> _OK1Regions;
        private List<BinRegion> OK1Regions => new List<BinRegion>(_OK1Regions);
        private List<BinRegion> _OK2Regions;
        private List<BinRegion> OK2Regions => new List<BinRegion>(_OK2Regions);

        private Func<DbContext> CreateContext;

        public int NGTrayID { get; set; }

        public int OK1TrayID { get; set; }

        public int OK2TrayID { get; set; }

        private AdsDriverClient _client;
        /// <summary>
        /// AdsDriverClient
        /// </summary>
        public AdsDriverClient Client
        {
            get => _client;
            set
            {
                _client = value;
            }
        }
 
        public void BindDataBase<TContext>(List<TrayInfo> trays = null, bool replace = false) where TContext : DbContext, new()
        {
            CreateContext += () => new TContext();

            LoadData(trays, replace);
        }
 
        private void InitDBBinColor()
        {
            List<string> bins = new List<string>() { "1","2","3","4","5","6","7","8","9","10", Dut.NoTestBin.ToString() ,"200", "99"};
            foreach (var bin in bins)
            {
                DragonDbHelper.SetBinColor(int.Parse(bin), Color.Blue, false);
            }
        }
        private void SetBinColor()
        {
            List<string> bins = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "99" };//已用的bin值
            dataGridView1.Rows.Clear();
            foreach (var bin in bins)
            {
                //var tmpBin = bin;
                //if (tmpBin == "F") tmpBin = "4";
                //else if (tmpBin == "99") tmpBin = "5";
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[ColBin.Index].Value = bin;
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[ColColor.Index].Style.BackColor = DragonDbHelper.GetBinColor(int.Parse(bin));
            }
        }
 
        public void LoadData(List<TrayInfo> trays, bool replace)
        {
            using (var context = CreateContext())
            {
                var binList = context.Set<BinColor>().OrderBy(c => c.Bin).ToList(); 
            }

            _nGRegions = LoadRegionsData(NGTrayID);
            _OK1Regions = LoadRegionsData(OK1TrayID);
            _OK2Regions = LoadRegionsData(OK2TrayID);

            SetBinRegionTextAndColor(NGTrayID, NGTray);
            SetBinRegionTextAndColor(OK1TrayID, OK1Tray);
            SetBinRegionTextAndColor(OK2TrayID, OK2Tray);

        }
 
        private List<BinRegion> LoadRegionsData(int trayID)
        {
            return DragonDbHelper.GetBinRegion(trayID);
        }

        private void WriteRegions(List<BinRegion> binRegions, TrayName trayName)
        {
            if (_client == null) return;
            if (!_client.IsInitOk) return;
            //int trayRow;
            //int trayColumn;
            //if (TrayName.Load1 == trayName || TrayName.Load2 == trayName || TrayName.NG == trayName)
            //{
            //    trayRow = Tray.S_ROW;
            //    trayColumn = Tray.S_Col;
            //}
            //else
            //{
            //    trayRow = Tray.ROW;
            //    trayColumn = Tray.COL;
            //}
            var regions = binRegions.Select(r => new CYGKit.AdsProtocol.Models.BinRegion
            {
                TrayIndex = r.TrayID,
                StartColume = r.StartColumn,
                StartRow = r.StartRow,
                EndColumn = r.EndColumn,
                EndRow = r.EndRow,
                Value = r.Bin,
            }).ToList();


            var ret = _client.WriteBinRegion(regions, Tray.ROW, Tray.COL, out var message);
            if (ret)
            {
                //AlcSystem.Instance.ShowMsgBox("写入成功!", "Information", icon: AlcMsgBoxIcon.Information);
            }
            else
            {
                AlcSystem.Instance.ShowMsgBox("写入失败!" + message, "Error", icon: AlcMsgBoxIcon.Error);
            }
        }
 
        private void gridNGTray_SelectionChanged()
        {
            var selection = gridNGTray.SelectedRegion;
            if (selection.Width < 0 || selection.Height < 0)
            {
                NGCurrentRegion= null;
 
                return;
            }

            _nGRegions = LoadRegionsData(NGTrayID);
            foreach (var region in _nGRegions)
            {
                if (selection.Top <= region.EndRow &&
                    selection.Bottom >= region.StartRow &&
                    selection.Left <= region.EndColumn &&
                    selection.Right >= region.StartColumn)
                {
                    gridNGTray.SelectionChanged -= gridNGTray_SelectionChanged;
                    gridNGTray.SelectedRegion = new Rectangle
                    {
                        X = region.StartColumn,
                        Y = region.StartRow,
                        Width = region.EndColumn - region.StartColumn,
                        Height = region.EndRow - region.StartRow
                    };
                    gridNGTray.SelectionChanged += gridNGTray_SelectionChanged;
                    NGCurrentRegion = region;
 
                    return;
                }
            }

            NGCurrentRegion = new BinRegion
            {
                TrayID = NGTrayID,
                StartRow = selection.Top,
                StartColumn = selection.Left,
                EndRow = selection.Bottom,
                EndColumn = selection.Right,
            };
 
        }

        private void gridOK1Tray_SelectionChanged()
        {
            var selection = gridOK1Tray.SelectedRegion;
            if (selection.Width < 0 || selection.Height < 0)
            {
                OK1CurrentRegion = null;
  
                return;
            }

            _OK1Regions = LoadRegionsData(OK1TrayID);
            foreach (var region in _OK1Regions)
            {
                if (selection.Top <= region.EndRow &&
                    selection.Bottom >= region.StartRow &&
                    selection.Left <= region.EndColumn &&
                    selection.Right >= region.StartColumn)
                {
                    gridOK1Tray.SelectionChanged -= gridOK1Tray_SelectionChanged;
                    gridOK1Tray.SelectedRegion = new Rectangle
                    {
                        X = region.StartColumn,
                        Y = region.StartRow,
                        Width = region.EndColumn - region.StartColumn,
                        Height = region.EndRow - region.StartRow
                    };
                    gridOK1Tray.SelectionChanged += gridOK1Tray_SelectionChanged;
                    OK1CurrentRegion = region;
 
                    return;
                }
            }

            OK1CurrentRegion = new BinRegion
            {
                TrayID = OK1TrayID,
                StartRow = selection.Top,
                StartColumn = selection.Left,
                EndRow = selection.Bottom,
                EndColumn = selection.Right,
            };
 
        }

        private void gridOK2Tray_SelectionChanged()
        {
            var selection = gridView1.SelectedRegion;
            if (selection.Width < 0 || selection.Height < 0)
            {
                OK2CurrentRegion = null;
 
                return;
            }

            _OK2Regions = LoadRegionsData(OK2TrayID);
            foreach (var region in _OK2Regions)
            {
                if (selection.Top <= region.EndRow &&
                    selection.Bottom >= region.StartRow &&
                    selection.Left <= region.EndColumn &&
                    selection.Right >= region.StartColumn)
                {
                    gridView1.SelectionChanged -= gridOK2Tray_SelectionChanged;
                    gridView1.SelectedRegion = new Rectangle
                    {
                        X = region.StartColumn,
                        Y = region.StartRow,
                        Width = region.EndColumn - region.StartColumn,
                        Height = region.EndRow - region.StartRow
                    };
                    gridView1.SelectionChanged += gridOK2Tray_SelectionChanged;
                    OK2CurrentRegion = region;
 
                    return;
                }
            }

            OK2CurrentRegion = new BinRegion
            {
                TrayID = OK2TrayID,
                StartRow = selection.Top,
                StartColumn = selection.Left,
                EndRow = selection.Bottom,
                EndColumn = selection.Right,
            };
 
        }

        private void SetBinRegionTextAndColor(int trayID, GridView tray)
        {
            using (var context = new DragonContext())
            {
                //tray.SelectedRegion = new Rectangle(-1, -1, -1, -1);
                tray.ClearColor();
                tray.ClearDisplayText();
                var Colors = context.Set<BinColor>().ToList();

                var regions = DragonDbHelper.GetBinRegion(trayID);
                //展示到网格
                foreach (var region in regions)
                {
                    var binColor = Colors.FirstOrDefault(c => c.Bin == region.Bin);
                    var color = binColor == null ? Color.DarkGray : binColor.Color;
                    tray.SetColor(color, region.StartRow, region.StartColumn, region.EndRow, region.EndColumn);

                    var binText = region.Bin.ToString();
                    //if (binText == "4") binText = "F";
                    //else if (binText == "5") binText = "99";
                    tray.SetText(binText, region.StartRow, region.StartColumn, region.EndRow, region.EndColumn);
                }
            }
        }
       
        private void Cells_OK2TrayMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var context = CreateContext();
                //保存分bin区域
                if (_OK2CurrentRegion != null)
                {
                    _OK2CurrentRegion.Bin = int.Parse(CurrentSelectBin == "F" ? "4" : CurrentSelectBin); ;
                    if (_OK2CurrentRegion.ID == 0)
                    {
                        context.Set<BinRegion>().Add(_OK2CurrentRegion);
                    }
                    else
                    {
                        var region = context.Set<BinRegion>().FirstOrDefault(r => r.ID == _OK2CurrentRegion.ID);
                        if (region == null)
                            context.Set<BinRegion>().Add(_OK2CurrentRegion);
                        else
                            _OK2CurrentRegion.CopyTo_PropertyOnly(region);
                    }
                    context.SaveChanges();
                    EventCenter.OK2TrayBinSet?.Invoke();
                }

                _OK2Regions = LoadRegionsData(OK2TrayID);
                // 写入bin区域到PLC
                WriteRegions(OK2Regions, TrayName.Pass2);
                // 设置Bin Text和对应的Bin颜色
                SetBinRegionTextAndColor(OK2TrayID, OK2Tray);
            }
            
        }

        private void Cells_OK2TrayMouseDown(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
 
        }

        private void Cells_OK1TrayMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var context = CreateContext();
                //保存分bin区域
                if (_OK1CurrentRegion != null)
                {
                    _OK1CurrentRegion.Bin = int.Parse(CurrentSelectBin == "F" ? "4" : CurrentSelectBin); ;
                    if (_OK1CurrentRegion.ID == 0)
                    {
                        context.Set<BinRegion>().Add(_OK1CurrentRegion);
                    }
                    else
                    {
                        var region = context.Set<BinRegion>().FirstOrDefault(r => r.ID == _OK1CurrentRegion.ID);
                        if (region == null)
                            context.Set<BinRegion>().Add(_OK1CurrentRegion);
                        else
                            _OK1CurrentRegion.CopyTo_PropertyOnly(region);
                    }
                    context.SaveChanges();
                    EventCenter.OK1TrayBinSet?.Invoke();
                }

                _OK1Regions = LoadRegionsData(OK1TrayID);
                // 写入bin区域到PLC
                WriteRegions(OK1Regions, TrayName.Pass1);
                // 设置Bin Text和对应的Bin颜色
                SetBinRegionTextAndColor(OK1TrayID, OK1Tray);
            }
        }

        private void Cells_OK1TrayMouseDown(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Cells_NGTrayMouseUp(object sender, MouseEventArgs e)
        {
            var selection = gridNGTray.SelectedRegion;
            if (e.Button == MouseButtons.Left)
            {
                var context = CreateContext();
                //保存分bin区域
                if (_nGCurrentRegion != null)
                {
                    _nGCurrentRegion.Bin = int.Parse(CurrentSelectBin == "F" ? "4" : CurrentSelectBin); ;
                    if (_nGCurrentRegion.ID == 0)
                    {
                        context.Set<BinRegion>().Add(_nGCurrentRegion);
                    }
                    else
                    {
                        var region = context.Set<BinRegion>().FirstOrDefault(r => r.ID == _nGCurrentRegion.ID);
                        if (region == null)
                            context.Set<BinRegion>().Add(_nGCurrentRegion);
                        else
                            _nGCurrentRegion.CopyTo_PropertyOnly(region);
                    }
                    context.SaveChanges();
                    EventCenter.NGTrayBinSet?.Invoke();
                }

                _nGRegions = LoadRegionsData(NGTrayID);
                // 写入bin区域到PLC
                WriteRegions(NGRegions, TrayName.NG);
                // 设置Bin Text和对应的Bin颜色
                SetBinRegionTextAndColor(NGTrayID, NGTray);
            }
        }

        private void Cells_NGTrayMouseDown(object sender, MouseEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) return;

            CurrentSelectBin = dataGridView1.Rows[e.RowIndex].Cells[ColBin.Index].Value.ToString();
            //if (CurrentSelectBin == "F") CurrentSelectBin = "4";
            //else if (CurrentSelectBin == "99") CurrentSelectBin = "5";
            CurrentSelectBinColor = dataGridView1.Rows[e.RowIndex].Cells[ColColor.Index].Style.BackColor;
            if (e.ColumnIndex == ColColor.Index)
            {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[ColColor.Index].Style.BackColor = colorDialog1.Color;
                    CurrentSelectBinColor = dataGridView1.Rows[e.RowIndex].Cells[ColColor.Index].Style.BackColor;
                    DragonDbHelper.SetBinColor(int.Parse(CurrentSelectBin), CurrentSelectBinColor, true);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            deleteBinRegion(NGCurrentRegion, NGTray);
            deleteBinRegion(OK1CurrentRegion, OK1Tray);
            deleteBinRegion(OK2CurrentRegion, OK2Tray);

            SetBinRegionTextAndColor(NGTrayID, NGTray);
            SetBinRegionTextAndColor(OK1TrayID, OK1Tray);
            SetBinRegionTextAndColor(OK2TrayID, OK2Tray);


            EventCenter.NGTrayBinSet?.Invoke();
            EventCenter.OK1TrayBinSet?.Invoke();
            EventCenter.OK2TrayBinSet?.Invoke();

            _nGRegions = LoadRegionsData(NGTrayID);
            _OK1Regions = LoadRegionsData(OK1TrayID);
            _OK2Regions = LoadRegionsData(OK2TrayID);

            WriteRegions(NGRegions, TrayName.NG);
            WriteRegions(OK1Regions, TrayName.Pass1);
            WriteRegions(OK2Regions, TrayName.Pass2);
        }
 
        private void deleteBinRegion(BinRegion binRegion, GridView tray)
        {
            var context = CreateContext();
            if (binRegion != null)
            {
                if (binRegion.ID > 0)
                {
                    var regions = context.Set<BinRegion>().FirstOrDefault(r => r.ID == binRegion.ID);
                    if (regions != null)
                    {
                        context.Set<BinRegion>().Remove(regions);
                        context.SaveChanges();

                        //展示到网格
                        tray.SelectedRegion = new Rectangle(-1, -1, -1, -1);
                        tray.ClearColor();
                        tray.ClearDisplayText();
                    }
                }
            }
        }
        private void NGTraySizeChanged()
        {
            DragonDbHelper.ClearBinRegion((int)TrayName.NG);
            if (RunModeMgr.IsNGBigTray)
            {
                this.gridNGTray.Row = Tray.ROW;
                this.gridNGTray.Col = Tray.COL;
                if (_client.IsInitOk)
                {
                    ClearTrayData(this.gridNGTray, TrayName.NG);
                    _client?.WriteObject(RunModeMgr.TraySwitch(3), false);
                }

            }
            else
            {
                this.gridNGTray.Row = Tray.S_ROW;
                this.gridNGTray.Col = Tray.S_Col;

                if (_client.IsInitOk)
                {
                    ClearTrayData(this.gridNGTray, TrayName.NG);
                    _client?.WriteObject(RunModeMgr.TraySwitch(3), true);
                }

            }
        }

        private void UnloadTraySizeChanged()
        {
            DragonDbHelper.ClearBinRegion((int)TrayName.Pass1);
            DragonDbHelper.ClearBinRegion((int)TrayName.Pass2);
            if (RunModeMgr.IsUnloadBigTray)
            {
                this.gridOK1Tray.Row = Tray.ROW;
                this.gridOK1Tray.Col = Tray.COL;
                this.gridView1.Row = Tray.ROW;
                this.gridView1.Col = Tray.COL;
                if (_client.IsInitOk)
                {
                    ClearTrayData(this.gridOK1Tray, TrayName.Pass1);
                    ClearTrayData(this.gridView1, TrayName.Pass2);
                    _client?.WriteObject(RunModeMgr.TraySwitch(4), false);
                    _client?.WriteObject(RunModeMgr.TraySwitch(5), false);
                }

            }
            else
            {
                this.gridOK1Tray.Row = Tray.S_ROW;
                this.gridOK1Tray.Col = Tray.S_Col;
                this.gridView1.Row = Tray.S_ROW;
                this.gridView1.Col = Tray.S_Col;

                if (_client.IsInitOk)
                {
                    ClearTrayData(this.gridOK1Tray, TrayName.Pass1);
                    ClearTrayData(this.gridView1, TrayName.Pass2);
                    _client?.WriteObject(RunModeMgr.TraySwitch(4), true);
                    _client?.WriteObject(RunModeMgr.TraySwitch(5), true);
                }

            }
        }

        private void ClearTrayData(GridView tray, TrayName trayName)
        {

            //清空 PLC Tray Regions
            var name = $"{RunModeMgr.Name_TrayInfo}[{(int)trayName}].{RunModeMgr.Name_RegionNum}";
             Client.WriteObject(name, (ushort)0);

            name = RunModeMgr. Name_RegionMax;
            var regionMax = (ushort)Client.ReadObject(name, typeof(ushort));
            name = RunModeMgr.TrayInfoRegionValue(3);
            //清空BinValue一维数组
            Client?.WriteObject(name, new ushort[regionMax]);
            name = RunModeMgr.TrayInfoStTrayRegion(3);
            //清空BinRegion
            Client?.WriteObject(name, new short[Tray.ROW, Tray.COL]);

            tray.ClearDisplayText();
            tray.ClearColor();
            //tray.EnableUpdate = true;
            tray.SelectedRegion = new Rectangle(x: -1, -1, -1, -1);
        }


        private void InitTraySize()
        {
            if (_client == null)
                return;

            if (RunModeMgr.IsNGBigTray)
            {
                _client?.WriteObject(RunModeMgr.TraySwitch(3), false);
            }
            else
            {
                _client?.WriteObject(RunModeMgr.TraySwitch(3), true);
            }

            if (RunModeMgr.IsUnloadBigTray)
            {
                _client?.WriteObject(RunModeMgr.TraySwitch(4), false);
                _client?.WriteObject(RunModeMgr.TraySwitch(5), false);
            }
            else
            {
                _client?.WriteObject(RunModeMgr.TraySwitch(4), true);
                _client?.WriteObject(RunModeMgr.TraySwitch(5), true);
            }
        }
 
    }
}
