using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;
using HalconDotNet;
using VisionControls;
using VisionUtility;
using VisionModules;
using System.Linq;
using System.ComponentModel;

namespace VisionDemo
{
    public partial class FrmMain : WeifenLuo.WinFormsUI.Docking.DockContent, IVisionExtend
    //public partial class FrmMain :System.Windows.Forms, IVisionExtend
    {
        public static FrmMain Instance { get; private set; }//
        private DataGridView dgvFlowList = new DataGridView();
        private ContextMenuStrip cmsDgvFlowList = new ContextMenuStrip();
        private DataTable dtFlowList;///流程列表

        private DataGridView dgvFlow = new DataGridView();
        private ContextMenuStrip cmsDgvFlow = new ContextMenuStrip();
        private DataTable dtFlow;///流程

        private int curListIndex = 0;
        private FrmItemBase frmItemBase;

        public List<SuperWind> Windlist = new List<SuperWind>(); 
        public HImage image = new HImage();
        public HRegion region = new HRegion();

        public FrmMain()
        {
            InitializeComponent();
            //
            Instance = this;
            //
            IniDgv(dgvFlowList);
            IniDtFlowList(out dtFlowList);
            dgvFlowList.DataSource = dtFlowList;
            IniCmsDgvFlowList();
            dgvFlowList.CellClick += dgvFlowList_CellClick;
            pnlFlowListSub.Controls.Add(dgvFlowList);
            //
            Opacity = 0;
            IniDgv(dgvFlow);
            IniDtFlow(out dtFlow);
            dgvFlow.DataSource = dtFlow;
            IniCmsDgvFlow();
            IniDragDrop();
            dgvFlow.CellDoubleClick += dgvFlow_CellDoubleClick;
            tbpFlow.Controls.Add(dgvFlow);
            //
            
 
            for (int i = 0; i < tableLayoutPanel1.RowCount* tableLayoutPanel1.ColumnCount; i++)
            {
                Windlist.Add(new SuperWind());
                tableLayoutPanel1.Controls.Add(Windlist[Windlist.Count-1]);
                Windlist[Windlist.Count - 1].image = new HImage("byte", 100, 100);       
            }      
        }
        private void FrmMain_Shown(object sender, EventArgs e)
        {
          
            this.Opacity = 1;
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            //允许跨线程访问用户界面控件
            Control.CheckForIllegalCrossThreadCalls = false;
            //设置窗体属性
            this.MaximumSize = new Size(Screen.PrimaryScreen.WorkingArea.Width, Screen.PrimaryScreen.WorkingArea.Height);
            this.WindowState = FormWindowState.Maximized;
            //
            LoadPlugins();
            //
            VisionModulesManager.InitialVisionProgram(VisionModulesManager.CfgFileName);
            if (VisionModulesManager.FlowList.Count == 0)
            {
                VisionModulesManager.CurrFlow = new Flow();
                VisionModulesManager.FlowList.Add(VisionModulesManager.CurrFlow);
            }
            else if (VisionModulesManager.CurrFlow == null)
            {
                VisionModulesManager.CurrFlow = VisionModulesManager.FlowList[0];
            }
            UpdateDgvFlowList();
            UpdateDgvFlow();
            //
            image = new HImage("byte", 100, 100);
 
   

        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (MessageShow.MsgQuestionOkCancel("您确定退出系统？") == DialogResult.Cancel)
            //{
            //    e.Cancel = true;
            //    return;
            //}

            //Application.DoEvents();
            foreach (VisionCameraBase item in VisionModulesManager.CameraList)
                item.DisConnect();
        }

      

        #region "菜单栏"
        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VisionModulesManager.ReadConfig(VisionModulesManager.CfgFileName);
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VisionModulesManager.SaveConfig(VisionModulesManager.CfgFileName);
        }

        private void 另存为AToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 流程管理FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (((ToolStripMenuItem)sender).Checked == true)
            //    pnlFlowList.Show();
            //else
            //    pnlFlowList.Hide();
            UpdatePnlTool();
            pnlFlowList.Visible = ((ToolStripMenuItem)sender).Checked;

        }

        private void 视觉模块VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdatePnlTool();
            pnlVisionModule.Visible = ((ToolStripMenuItem)sender).Checked;
        }

        private void 图像显示IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlImage.Visible = ((ToolStripMenuItem)sender).Checked;
        }

        private void 流程栏FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlFlow.Visible = ((ToolStripMenuItem)sender).Checked;
        }

        private void 日志栏LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlLog.Visible = ((ToolStripMenuItem)sender).Checked;
        }

        private void 状态栏SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            状态栏SToolStripMenuItem.Visible = ((ToolStripMenuItem)sender).Checked;
        }
        private void 工具栏TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip1.Visible = ((ToolStripMenuItem)sender).Checked;
        }

        private void 采集设备AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAcqDevice frm = new FrmAcqDevice();
            OpenForm(frm);
        }


        private void 通讯设置CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FrmComSet frm = new FrmComSet();
            //OpenForm(frm);
        }

        private void 全局变量VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmGlobalVariable frm = new FrmGlobalVariable();
            OpenForm(frm);
        }

        private void 系统参数PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSystemPara frm = new FrmSystemPara();
            OpenForm(frm);
        }

        private void 切换用户UToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 修改密码PToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 注销EToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 用户手则UToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 关于SToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region"工具栏"
        private void 解决方案列表SToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void 新建NToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void 打开OToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void 单次执行SToolStripButton_Click(object sender, EventArgs e)
        {
            VisionModulesManager.SystemStatus.m_RunMode = RunMode.执行一次;
            VisionModulesManager.CurrFlow.StartThread();
        }

        private void 循环执行RToolStripButton_Click(object sender, EventArgs e)
        {
            VisionModulesManager.SystemStatus.m_RunMode = RunMode.循环运行;
            VisionModulesManager.CurrFlow.StartThread();
        }

        private void 停止SToolStripButton_Click(object sender, EventArgs e)
        {
            VisionModulesManager.CurrFlow.StopThread();
        }

        private void 采集设备AToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void 通讯设置CToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void 全局变量VToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void 帮助HToolStripButton_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region"helper"

        public void LoadPlugins()
        {

        }
        

        


        private void IniDgv(DataGridView dgv)
        {
            dgv.RowHeadersVisible = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.BackgroundColor = Color.White;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;
            dgvFlow.AllowDrop = true;
            dgv.Dock = DockStyle.Fill;

        }

        private void IniDtFlow(out DataTable dt)
        {
            dt = new DataTable();
            dt.Columns.Add("序号", Type.GetType("System.String"));  //单元标识
            dt.Columns.Add("项名称", Type.GetType("System.String"));//单元名称
            dt.Columns.Add("备注", Type.GetType("System.String"));  //单元描述
        }

        private void IniCmsDgvFlow()
        {
            ToolStripMenuItem tsmShiftUp = new ToolStripMenuItem("上移");
            tsmShiftUp.Click += new EventHandler((s, e) => OnShiftUp(s, e));
            ToolStripMenuItem tsmShiftDown = new ToolStripMenuItem("下移");
            tsmShiftDown.Click += new EventHandler((s, e) => OnShiftDown(s, e));
            ToolStripMenuItem tsmRemoveAt = new ToolStripMenuItem("删除项");
            tsmRemoveAt.Click += new EventHandler((s, e) => OnRemoveAt(s, e));
            ToolStripMenuItem tsmRemoveAll = new ToolStripMenuItem("删除所有");
            tsmRemoveAll.Click += new EventHandler((s, e) => OnRemoveAll(s, e));

            cmsDgvFlow.Items.Add(tsmShiftUp);
            cmsDgvFlow.Items.Add(tsmShiftDown);
            cmsDgvFlow.Items.Add(tsmRemoveAt);
            cmsDgvFlow.Items.Add(tsmRemoveAll);

            cmsDgvFlow.Opening += new CancelEventHandler((s, e) => OnOpening());
            dgvFlow.ContextMenuStrip = cmsDgvFlow;
        }


        private void IniDragDrop()
        {
            采集图像ToolStripButton.MouseDown += OnMouseDown;
            显示图像ToolStripButton.MouseDown += OnMouseDown;
            存储图像ToolStripButton.MouseDown += OnMouseDown;
            预先处理ToolStripButton.MouseDown += OnMouseDown;
            创建ROIToolStripButton.MouseDown += OnMouseDown;
            模板匹配ToolStripButton.MouseDown += OnMouseDown;
            直线测量ToolStripButton.MouseDown += OnMouseDown;
            圆形测量ToolStripButton.MouseDown += OnMouseDown;
            矩形测量ToolStripButton.MouseDown += OnMouseDown;
            椭圆测量ToolStripButton.MouseDown += OnMouseDown;
            畸变标定ToolStripButton.MouseDown += OnMouseDown;
            N点标定ToolStripButton.MouseDown += OnMouseDown;
            机械手控制ToolStripButton.MouseDown += OnMouseDown;
            查找二维码ToolStripButton.MouseDown += OnMouseDown;
            字符识别ToolStripButton.MouseDown += OnMouseDown;

            //采集图像ToolStripButton.Tag = ItemType.采集图像;

            dgvFlow.DragEnter += OnDragEnter;
            dgvFlow.DragDrop += OnDragDrop;
        }


        private void dgvFlow_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = e.RowIndex;

                if (index < 0)
                    return;

                DataGridView dgv = sender as DataGridView;

                int itemID = int.Parse(dgv.Rows[index].Cells[0].Value.ToString().Substring(0));
                IEnumerable<Item> resultList = from item in VisionModulesManager.CurrFlow.ItemList
                                               where item.ItemID == itemID
                                               select item;
                Item curItem = resultList.ToList()[0];

                ShowItemForm(ref curItem);
                curListIndex = e.RowIndex;
            }
            catch
            { }
        }

        private void IniDtFlowList(out DataTable dt)
        {
            dt = new DataTable();
            dt.Columns.Add("序号", Type.GetType("System.String"));  //单元标识
            dt.Columns.Add("流程名称", Type.GetType("System.String"));//单元名称
        }

        private void IniCmsDgvFlowList()
        {
            ToolStripMenuItem tsmAdd = new ToolStripMenuItem("添加");
            tsmAdd.Click += new EventHandler((s, e) => OnAdd(s, e));
            ToolStripMenuItem tsmEdit = new ToolStripMenuItem("编辑");
            tsmEdit.Click += new EventHandler((s, e) => OnEdit(s, e));
            ToolStripMenuItem tsmRemove = new ToolStripMenuItem("删除");
            tsmRemove.Click += new EventHandler((s, e) => OnRemove(s, e));

            cmsDgvFlowList.Items.Add(tsmAdd);
            cmsDgvFlowList.Items.Add(tsmEdit);
            cmsDgvFlowList.Items.Add(tsmRemove);

            dgvFlowList.ContextMenuStrip = cmsDgvFlowList;
        }

        private void dgvFlowList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int index = e.RowIndex;

                if (index < 0)
                    return;

                VisionModulesManager.CurrFlow = VisionModulesManager.FlowList[index];
                UpdateDgvFlow();
            }
            catch
            { }
        }

        private void ShowItemForm(ref Item item)
        {
            if (frmItemBase == null || frmItemBase.IsDisposed)
            {
                switch (item.ItemType)
                {
                    case ItemType.采集图像:
                        frmItemBase = new FrmItemAcqImage(item);
                        break;
                    case ItemType.显示图像:
                        frmItemBase = new FrmItemShowImage(item);
                        break;
                    case ItemType.存储图像:
                        frmItemBase = new FrmItemSaveImage(item);
                        break;
                    case ItemType.预先处理:

                        break;
                    case ItemType.创建ROI:
                        frmItemBase = new FrmItemCreateRoi(item);
                        break;
                    case ItemType.模板匹配:
                        frmItemBase = new FrmItemModelMatch(item);
                        break;
                    case ItemType.圆形测量:
                        frmItemBase = new FrmItemMeasureCircle(item);
                        break;
                    case ItemType.畸变标定:
                        break;
                    case ItemType.N点标定:
                        frmItemBase = new FrmItemNPointCalib(item);
                        break;
                    case ItemType.机械手控制:
                        frmItemBase = new FrmItemRobotControl(item);
                        break;
                    case ItemType.查找二维码:
                        frmItemBase = new FrmItemFindDataCode2D(item);
                        break;
                    case ItemType.字符识别:
                        frmItemBase = new FrmItemOcr(item);
                        break;
                    default:
                        break;
                }

                try
                {
                    //frmItemBase.Show(this);
                    frmItemBase.ShowDialog(this);
                    frmItemBase.Dispose();
                }
                catch (Exception ex)
                {
                    frmItemBase.Dispose();
                }
            }
        }


        /// <summary>
        /// 打开或激活窗体
        /// </summary>
        /// <param name="frm"></param>
        private void OpenForm(Form frm)
        {
            //子窗体
            //Form form = Application.OpenForms[frm.Name];

            //if (form != null)
            //    form.Activate();
            //else
            //{
            //    frm.ShowIcon = false;
            //    frm.ShowInTaskbar = true;
            //    frm.Show();
            //}

            //模式对话框
            frm.ShowIcon = false;
            frm.MinimizeBox = false;
            frm.MaximizeBox = false;
            frm.ShowInTaskbar = true;
            frm.ShowDialog();
        }

        #endregion

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ToolStripButton button = (ToolStripButton)sender;
                string text = button.Text;
                if (text != null)
                {
                    button.DoDragDrop(text, DragDropEffects.Copy);
                }
            }
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void OnDragDrop(object sender, DragEventArgs e)
        {
            //得到要拖拽到的位置
            Point p = dgvFlow.PointToClient(new Point(e.X, e.Y));
            DataGridView.HitTestInfo hit = this.dgvFlow.HitTest(p.X, p.Y);

            if (hit.Type == DataGridViewHitTestType.None)
            {
                ItemType itemType = (ItemType)Enum.Parse(typeof(ItemType), (string)e.Data.GetData(typeof(string)));
                Item item;
                switch (itemType)
                {
                    case ItemType.采集图像:
                        item = new ItemAcqImage(VisionModulesManager.CurrFlow.LastItemID);
                        break;
                    case ItemType.显示图像:
                        item = new ItemShowImage(VisionModulesManager.CurrFlow.LastItemID);
                        break;
                    case ItemType.存储图像:
                        item = new ItemSaveImage(VisionModulesManager.CurrFlow.LastItemID);
                        break;
                    case ItemType.创建ROI:
                        item = new ItemCreateRoi(VisionModulesManager.CurrFlow.LastItemID);
                        break;
                    case ItemType.模板匹配:
                        item = new ItemModelMatch(VisionModulesManager.CurrFlow.LastItemID);
                        break;
                    case ItemType.直线测量:
                        item = new ItemModelMatch(VisionModulesManager.CurrFlow.LastItemID);
                        break;
                    case ItemType.圆形测量:
                        item = new ItemMeasureCircle(VisionModulesManager.CurrFlow.LastItemID);
                        break;
                    case ItemType.矩形测量:
                        item = new ItemMeasureCircle(VisionModulesManager.CurrFlow.LastItemID);
                        break;
                    case ItemType.椭圆测量:
                        item = new ItemMeasureCircle(VisionModulesManager.CurrFlow.LastItemID);
                        break;
                    case ItemType.N点标定:
                        item = new ItemNPointCalib(VisionModulesManager.CurrFlow.LastItemID);
                        break;
                    case ItemType.机械手控制:
                        item = new ItemRobotControl(VisionModulesManager.CurrFlow.LastItemID);
                        break;
                    case ItemType.查找二维码:
                        item = new ItemFindDataCode2D(VisionModulesManager.CurrFlow.LastItemID);
                        break;
                    case ItemType.字符识别:
                        item = new ItemOcr(VisionModulesManager.CurrFlow.LastItemID);
                        break;
                    default:
                        item = new Item(itemType, VisionModulesManager.CurrFlow.LastItemID);
                        break;
                }

                DataRow dr = dtFlow.NewRow();
                dr[0] = item.ItemID;
                dr[1] = item.ItemType;
                dr[2] = item.ItemRemark;
                dtFlow.Rows.Add(dr);

                item.Owner = VisionModulesManager.CurrFlow;


                //如果只想允许拖拽到某一个特定列，比如Target Field Expression，则先要判断列是否为Target Field Expression，如下：
                //if (0 == string.Compare(clickedCell.OwningColumn.Name, "Target Field Expression"))
                //{
                //    clickedCell.Value = (System.String)e.Data.GetData(typeof(System.String));
                //}
            }
        }

        public void OnOpening()
        {
            //cmsDgvFlowList.Items[0].Enabled = true;
            //cmsDgvFlowList.Items[1].Enabled = true;

            //if (dgvFlowList.CurrentCell.RowIndex < 0)
            //{
            //    cmsDgvFlowList.Enabled = false;
            //}
            //if (dgvFlowList.SelectedRows.Count <= 0)
            //{
            //    cmsDgvFlowList.Items[0].Enabled = false;
            //}
            //else if (dgvFlowList.CurrentRow.Index == dgvFlowList.Rows.Count - 1)
            //{
            //    cmsDgvFlowList.Items[1].Enabled = false;
            //}
        }

        private void OnShiftUp(object sender, EventArgs e)
        {
            int currIndex = dgvFlow.CurrentRow.Index;
            var temp = VisionModulesManager.CurrFlow.ItemList[currIndex];
            VisionModulesManager.CurrFlow.ItemList[currIndex] = VisionModulesManager.CurrFlow.ItemList[currIndex - 1];
            VisionModulesManager.CurrFlow.ItemList[currIndex - 1] = temp;
            UpdateDgvFlow();
            this.dgvFlow.CurrentCell = this.dgvFlow.Rows[currIndex - 1].Cells[0];
        }

        private void OnShiftDown(object sender, EventArgs e)
        {
            int currIndex = dgvFlow.CurrentRow.Index;
            var temp = VisionModulesManager.CurrFlow.ItemList[currIndex];
            VisionModulesManager.CurrFlow.ItemList[currIndex] = VisionModulesManager.CurrFlow.ItemList[currIndex + 1];
            VisionModulesManager.CurrFlow.ItemList[currIndex + 1] = temp;
            UpdateDgvFlow();
            this.dgvFlow.CurrentCell = this.dgvFlow.Rows[currIndex + 1].Cells[0];
        }

        private void OnRemoveAt(object sender, EventArgs e)
        {
            int currIndex = dgvFlow.CurrentRow.Index;
            var item = VisionModulesManager.CurrFlow.ItemList[currIndex];
            VisionModulesManager.CurrFlow.ItemList.RemoveAt(currIndex);
            var itemList = VisionModulesManager.CurrFlow.ItemList.RemoveAll(c => c.ItemID == item.ItemID);
            UpdateDgvFlow();

            if (currIndex > 0)
            {
                dgvFlow.CurrentCell = dgvFlow.Rows[currIndex - 1].Cells[0];
            }
        }

        private void OnRemoveAll(object sender, EventArgs e)
        {
            VisionModulesManager.CurrFlow.ItemList.Clear();
            VisionModulesManager.CurrFlow.ItemList.RemoveAll(c => c.ItemID != VisionModulesManager.U000);
            VisionModulesManager.CurrFlow.LastItemID = 1;
            UpdateDgvFlow();
        }


        private void OnAdd(object sender, EventArgs e)
        {
            int currIndex = dgvFlowList.CurrentRow.Index;
            VisionModulesManager.FlowList.Add(new Flow());
            UpdateDgvFlowList();
            this.dgvFlowList.CurrentCell = this.dgvFlowList.Rows[currIndex].Cells[0];
        }

        private void OnEdit(object sender, EventArgs e)
        {

        }

        private void OnRemove(object sender, EventArgs e)
        {
            int currIndex = dgvFlowList.CurrentRow.Index;
            var item = VisionModulesManager.FlowList[currIndex];
            VisionModulesManager.FlowList.RemoveAt(currIndex);
            var itemList = VisionModulesManager.FlowList.RemoveAll(c => c.FlowID == item.FlowID);
            UpdateDgvFlowList();

            if (currIndex > 0)
            {
                dgvFlowList.CurrentCell = dgvFlowList.Rows[currIndex - 1].Cells[0];
            }
        }

        private void UpdateDgvFlowList()
        {
            try
            {
                dtFlowList.Clear();
                foreach (Flow flow in VisionModulesManager.FlowList)
                    dtFlowList.Rows.Add(new object[] { flow.FlowID.ToString(), flow.FlowName });
                if(dgvFlowList.Visible)
                {
                    dgvFlowList.Columns[0].Width = 40;
                    dgvFlowList.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
               
                UpdateDgvFlow();
            }
            catch (Exception ex)
            {

            }
        }

        private void UpdateDgvFlow()
        {
            try
            {
                dtFlow.Clear();
                foreach (Item item in VisionModulesManager.CurrFlow.ItemList)
                    dtFlow.Rows.Add(new object[] { item.ItemID.ToString(), item.ItemType, item.ItemRemark });
                if(dgvFlow.Visible)
                {
                    dgvFlow.Columns[0].Width = 40;
                    dgvFlow.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }           
            }
            catch (Exception ex)
            {

            }
        }

        private void UpdatePnlTool()
        {
            if (流程管理FToolStripMenuItem.Checked == false && 视觉模块VToolStripMenuItem.Checked == false)
                pnlTool.Hide();
            else
                pnlTool.Show();
        }

        #region"扩展"
        public virtual Form GetForm()
        {
            //throw new NotImplementedException();
            return Instance;
        }

        public UserControl GetControl()
        {
            return null;
        }

        public Dictionary<string, ToolStripItem[]> GetMenu()
        {
            MenuStrip1.Visible = false;
            return MenuStrip1.Items.Cast<ToolStripMenuItem>().ToDictionary(m => m.Tag.ToString(), m => m.DropDownItems.Cast<ToolStripItem>().ToArray());

        }

        public void SetExtendForm(Form frm)
        {
            foreach (ToolStripDropDownItem item in 扩展EToolStripMenuItem.DropDownItems)
            {
                if (item.Text == frm.Text) return;
            }

            IniForm(frm);
            ToolStripMenuItem tsm = new ToolStripMenuItem(frm.Text);
            frm.FormClosing += new FormClosingEventHandler((s, e) => { ((Form)s).Hide(); e.Cancel = true; });
            tsm.Click += new EventHandler((s, e) => { frm.Show(); });
            扩展EToolStripMenuItem.DropDownItems.Add(tsm);
        }
        public void SetExtendMenu(ToolStripMenuItem menu)
        {
            扩展EToolStripMenuItem.DropDownItems.AddRange(menu.DropDownItems.Cast<ToolStripItem>().ToArray());
        }


        public void SetFlowForm(Form frm)
        {
            foreach (Control item in tbcFlow.Controls)
            {
                if (item.Text == frm.Text) return;
            }
            IniForm(frm);
            TabPage tbp = new TabPage(frm.Text);
            frm.TopLevel = false;
            tbp.Controls.Add(frm);
            tbcFlow.Controls.Add(tbp);
            frm.Show();

            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill; ;
        }
        public void SetControl(UserControl uc)
        {
        }
        public void SetUI()
        {
            流程管理FToolStripMenuItem.Visible = false;
            视觉模块VToolStripMenuItem.Visible = false;
            工具栏TToolStripMenuItem.Visible = false;
            toolStrip1.Hide();
            pnlTool.Hide();
            tbpFlow.Parent = null;
        }
        public void IniForm(Form frm)
        {
            frm.ShowIcon = false;
            frm.MaximizeBox = false;
            frm.MinimizeBox = false;
            frm.TopMost = true;
            frm.BackColor = Color.White;
            frm.StartPosition = FormStartPosition.CenterScreen;
        }


        #endregion

      
    }
}
