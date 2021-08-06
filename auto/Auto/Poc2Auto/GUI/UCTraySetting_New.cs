using CYGKit.Factory.DataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CYGKit.Common;
using CYGKit.GUI;
using System.ComponentModel;
using AlcUtility;

namespace Poc2Auto.GUI
{
    public partial class UCTraySetting_New : UserControl
    {
        public UCTraySetting_New()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Tray盘行数
        /// </summary>
        public int TrayRow { get => gridViewTray.Row; set { gridViewTray.Row = value; } }
        /// <summary>
        /// Tray盘列数
        /// </summary>
        public int TrayColumn { get => gridViewTray.Col; set { gridViewTray.Col = value; } }
        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get => groupBox1.Text;
            set => groupBox1.Text = value;
        }

        /// <summary>
        /// tray盘控件
        /// </summary>
        public GridView Tray => gridViewTray;

        /// <summary>
        /// TrayID
        /// </summary>
        [Browsable(false)]
        public int TrayID => (int)comboBoxTrayName.SelectedValue;

        /// <summary>
        /// 所有区域
        /// </summary>
        [Browsable(false)]
        public List<BinRegion> Regions => new List<BinRegion>(_regions);

        /// <summary>
        /// 所有区域
        /// </summary>
        [Browsable(false)]
        public int[,] Region2D
        {
            get
            {
                var data = new int[TrayRow, TrayColumn];
                foreach (var region in _regions)
                {
                    for (var i = region.StartRow; i <= region.EndRow; i++)
                        for (var j = region.StartColumn; j <= region.EndColumn; j++)
                        {
                            data[i, j] = region.Bin;
                        }
                }
                return data;
            }
        }

        /// <summary>
        /// 选择数量
        /// </summary>
        [Browsable(false)]
        public int SelectionCount
        {
            get
            {
                var count = 0;
                foreach (var region in _regions)
                {
                    var hight = region.EndRow - region.StartRow + 1;
                    var width = region.EndColumn - region.StartColumn + 1;
                    count += hight * width;
                }
                return count;
            }
        }

        private List<BinRegion> _regions;
        private BinRegion _currentRegion;
        private BinRegion CurrentRegion
        {
            set
            {
                _currentRegion = value;
                if (value == null)
                    labelRegion.Text = "( - , - ) ~ ( - , - )";
                else
                    labelRegion.Text = $"( {value.StartRow + 1} , {value.StartColumn + 1} ) ~ ( {value.EndRow + 1} , {value.EndColumn + 1} )";
            }
        }
        private Func<DbContext> CreateContext;

        /// <summary>
        /// 绑定数据库
        /// </summary>
        public void BindDataBase<TContext>(List<TrayInfo> trays = null, bool replace = false) where TContext : DbContext, new()
        {
            CreateContext += () => new TContext();

            LoadData(trays, replace);
        }

        /// <summary>
        /// 读取数据库加载数据
        /// </summary>
        public void LoadData(List<TrayInfo> trays, bool replace)
        {
            using (var context = CreateContext())
            {
                comboBoxBin.DataSource = context.Set<BinColor>().OrderBy(c => c.Bin).ToList();
                comboBoxBin.DisplayMember = "Bin";

                var allTrays = new List<TrayInfo>();
                if (!replace) allTrays.AddRange(context.Set<TrayInfo>());
                if (trays != null) allTrays.AddRange(trays);
                comboBoxTrayName.DataSource = allTrays;
                comboBoxTrayName.DisplayMember = "Name";
                comboBoxTrayName.ValueMember = "ID";
                if (comboBoxTrayName.Items.Count == 0) return;
                comboBoxTrayName.SelectedIndex = 0;
            }

            LoadRegions();
        }

        private void LoadRegions()
        {
            using (var context = CreateContext())
            {
                var trayInfo = comboBoxTrayName.SelectedItem as TrayInfo;
                if (trayInfo == null) return;
                _regions = context.Set<BinRegion>().Where(r => r.TrayID == trayInfo.ID).ToList();

                //展示到网格
                gridViewTray.SelectedRegion = new Rectangle(-1, -1, -1, -1);
                gridViewTray.ClearColor();
                var binColors = context.Set<BinColor>().ToList();
                foreach (var region in _regions)
                {
                    var binColor = binColors.FirstOrDefault(c => c.Bin == region.Bin);
                    var color = binColor == null ? Color.DarkGray : binColor.Color;
                    gridViewTray.SetColor(color, region.StartRow, region.StartColumn, region.EndRow, region.EndColumn);
                }
            }
        }

        //private void UC_TraySetting_Load(object sender, EventArgs e)
        //{
        //    SetTrayLocation();
        //}

        //private void SetTrayLocation()
        //{
        //    var x = (panel1.Width - gridViewTray.Width) / 2;
        //    var y = (panel1.Height - gridViewTray.Height) / 2;
        //    gridViewTray.Location = new Point(x > 0 ? x : 0, y > 0 ? y : 0);
        //}

        //private void Panel1_SizeChanged(object sender, EventArgs e)
        //{
        //    SetTrayLocation();
        //}

        private void LabelColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() != DialogResult.OK) return;

            labelColor.BackColor = colorDialog1.Color;
        }

        private void GridViewTray_SelectionChaged()
        {
            var trayInfo = comboBoxTrayName.SelectedItem as TrayInfo;
            if (trayInfo == null) return;

            var selection = gridViewTray.SelectedRegion;
            if (selection.Width < 0 || selection.Height < 0)
            {
                CurrentRegion = null;
                buttonDelete.Enabled = false;
                return;
            }

            foreach (var region in _regions)
            {
                if (selection.Top <= region.EndRow &&
                    selection.Bottom >= region.StartRow &&
                    selection.Left <= region.EndColumn &&
                    selection.Right >= region.StartColumn)
                {
                    gridViewTray.SelectionChanged -= GridViewTray_SelectionChaged;
                    gridViewTray.SelectedRegion = new Rectangle
                    {
                        X = region.StartColumn,
                        Y = region.StartRow,
                        Width = region.EndColumn - region.StartColumn,
                        Height = region.EndRow - region.StartRow
                    };
                    gridViewTray.SelectionChanged += GridViewTray_SelectionChaged;
                    CurrentRegion = region;
                    comboBoxBin.Text = region.Bin.ToString();
                    buttonDelete.Enabled = true;
                    return;
                }
            }

            CurrentRegion = new BinRegion
            {
                TrayID = trayInfo.ID,
                StartRow = selection.Top,
                StartColumn = selection.Left,
                EndRow = selection.Bottom,
                EndColumn = selection.Right,
            };

            comboBoxBin.Text = "";
        }

        private void ComboBoxBin_TextChanged(object sender, EventArgs e)
        {
            var binStr = comboBoxBin.Text;
            if (string.IsNullOrEmpty(binStr)) return;
            if (!int.TryParse(binStr, out int bin)) return;

            var context = CreateContext();
            var binColor = context.Set<BinColor>().FirstOrDefault(c => c.Bin == bin);
            if (binColor == null)
            {
                labelColor.BackColor = Color.DarkGray;
            }
            else
            {
                labelColor.BackColor = binColor.Color;
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            var binStr = comboBoxBin.Text;
            if (string.IsNullOrEmpty(binStr))
            {
                AlcSystem.Instance.ShowMsgBox("请输入Bin值", "Error", icon: AlcMsgBoxIcon.Error);
                return;
            }
            if (!int.TryParse(binStr, out int bin))
            {
                AlcSystem.Instance.ShowMsgBox("输入的Bin不合法，请输入整数", "Error", icon: AlcMsgBoxIcon.Error);
                return;
            }

            var context = CreateContext();

            //保存分bin区域
            var trayInfo = comboBoxTrayName.SelectedItem as TrayInfo;
            if (_currentRegion != null && trayInfo != null)
            {
                _currentRegion.Bin = bin;
                if (_currentRegion.ID == 0)
                {
                    context.Set<BinRegion>().Add(_currentRegion);
                }
                else
                {
                    var region = context.Set<BinRegion>().FirstOrDefault(r => r.ID == _currentRegion.ID);
                    if (region == null)
                        context.Set<BinRegion>().Add(_currentRegion);
                    else
                        _currentRegion.CopyTo_PropertyOnly(region);
                }
            }

            //保存bin颜色对应关系
            var binColor = context.Set<BinColor>().FirstOrDefault(c => c.Bin == bin);
            if (binColor == null)
            {
                context.Set<BinColor>().Add(new BinColor
                {
                    Bin = bin,
                    Color = labelColor.BackColor,
                });
            }
            else
            {
                binColor.Color = labelColor.BackColor;
            }
            context.SaveChanges();
            comboBoxBin.DataSource = context.Set<BinColor>().OrderBy(c => c.Bin).ToList();
            comboBoxBin.Text = bin.ToString();
            LoadRegions();
        }

        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (_currentRegion == null)
            {
                AlcSystem.Instance.ShowMsgBox("请先选择区域", "Error", icon: AlcMsgBoxIcon.Error);
                return;
            }
            if (_currentRegion.ID <= 0) return;
            var context = CreateContext();
            var region = context.Set<BinRegion>().FirstOrDefault(r => r.ID == _currentRegion.ID);
            if (region != null)
            {
                context.Set<BinRegion>().Remove(region);
                context.SaveChanges();
            }
            LoadRegions();
        }

        private void ComboBoxTrayName_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadRegions();
        }
    }
}
