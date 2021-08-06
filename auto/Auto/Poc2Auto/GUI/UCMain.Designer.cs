
namespace Poc2Auto.GUI
{
    partial class UCMain
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelLotInfo = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageVision = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rBtnLiveCamera1 = new System.Windows.Forms.CheckBox();
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rBtnLiveCamera2 = new System.Windows.Forms.CheckBox();
            this.hWindowControl2 = new HalconDotNet.HWindowControl();
            this.listView_vision_message = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel3 = new System.Windows.Forms.Panel();
            this.rBtnLiveCamera3 = new System.Windows.Forms.CheckBox();
            this.hWindowControl3 = new HalconDotNet.HWindowControl();
            this.tabPageStatistics = new System.Windows.Forms.TabPage();
            this.ucStatistics1 = new Poc2Auto.GUI.UCStatistics();
            this.tabPageUPHChart = new System.Windows.Forms.TabPage();
            this.uphChart1 = new CYGKit.Factory.UPH.UPHChart();
            this.tabPagSelectBin = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rdbtnLoadRTray = new System.Windows.Forms.RadioButton();
            this.rdbtnLoadLTray = new System.Windows.Forms.RadioButton();
            this.btnSelectBinOK = new System.Windows.Forms.Button();
            this.ucModeParams_SelectDut_Bin1 = new Poc2Auto.GUI.RunModeConfig.UCModeParams_SelectDut_Bin();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ucRunLog1 = new Poc2Auto.GUI.UCRunLog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ucErrorList1 = new Poc2Auto.GUI.UCErrorList();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ucSocket_New1 = new Poc2Auto.GUI.UCSocket_New();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNormal = new System.Windows.Forms.Button();
            this.btnManual = new System.Windows.Forms.Button();
            this.btnSelectSN = new System.Windows.Forms.Button();
            this.btnGRR = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ucStations1 = new Poc2Auto.GUI.UCStations();
            this.ucTrays1 = new Poc2Auto.GUI.UCTrays();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageVision.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabPageStatistics.SuspendLayout();
            this.tabPageUPHChart.SuspendLayout();
            this.tabPagSelectBin.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1060, 637);
            this.splitContainer1.SplitterDistance = 395;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.09091F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.97902F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.47484F));
            this.tableLayoutPanel1.Controls.Add(this.panelLotInfo, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.04762F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.67857F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.39286F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(395, 637);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panelLotInfo
            // 
            this.panelLotInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLotInfo.Location = new System.Drawing.Point(0, 0);
            this.panelLotInfo.Margin = new System.Windows.Forms.Padding(0);
            this.panelLotInfo.Name = "panelLotInfo";
            this.panelLotInfo.Size = new System.Drawing.Size(234, 121);
            this.panelLotInfo.TabIndex = 5;
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tableLayoutPanel1.SetColumnSpan(this.tabControl1, 3);
            this.tabControl1.Controls.Add(this.tabPageVision);
            this.tabControl1.Controls.Add(this.tabPageStatistics);
            this.tabControl1.Controls.Add(this.tabPageUPHChart);
            this.tabControl1.Controls.Add(this.tabPagSelectBin);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(0, 265);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(395, 372);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPageVision
            // 
            this.tabPageVision.Controls.Add(this.tableLayoutPanel2);
            this.tabPageVision.Location = new System.Drawing.Point(4, 29);
            this.tabPageVision.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageVision.Name = "tabPageVision";
            this.tabPageVision.Size = new System.Drawing.Size(387, 339);
            this.tabPageVision.TabIndex = 2;
            this.tabPageVision.Text = "Vision";
            this.tabPageVision.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.listView_vision_message, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(387, 339);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.rBtnLiveCamera1);
            this.panel1.Controls.Add(this.hWindowControl1);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(189, 165);
            this.panel1.TabIndex = 29;
            // 
            // rBtnLiveCamera1
            // 
            this.rBtnLiveCamera1.AutoSize = true;
            this.rBtnLiveCamera1.Location = new System.Drawing.Point(1, 1);
            this.rBtnLiveCamera1.Name = "rBtnLiveCamera1";
            this.rBtnLiveCamera1.Size = new System.Drawing.Size(49, 21);
            this.rBtnLiveCamera1.TabIndex = 3;
            this.rBtnLiveCamera1.Text = "Live";
            this.rBtnLiveCamera1.UseVisualStyleBackColor = true;
            this.rBtnLiveCamera1.Click += new System.EventHandler(this.rBtnLiveCamera1_Click);
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl1.Margin = new System.Windows.Forms.Padding(2);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(189, 165);
            this.hWindowControl1.TabIndex = 2;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(189, 165);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rBtnLiveCamera2);
            this.panel2.Controls.Add(this.hWindowControl2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(195, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(190, 165);
            this.panel2.TabIndex = 30;
            // 
            // rBtnLiveCamera2
            // 
            this.rBtnLiveCamera2.AutoSize = true;
            this.rBtnLiveCamera2.Location = new System.Drawing.Point(1, 1);
            this.rBtnLiveCamera2.Name = "rBtnLiveCamera2";
            this.rBtnLiveCamera2.Size = new System.Drawing.Size(49, 21);
            this.rBtnLiveCamera2.TabIndex = 4;
            this.rBtnLiveCamera2.Text = "Live";
            this.rBtnLiveCamera2.UseVisualStyleBackColor = true;
            this.rBtnLiveCamera2.Click += new System.EventHandler(this.rBtnLiveCamera2_Click);
            // 
            // hWindowControl2
            // 
            this.hWindowControl2.BackColor = System.Drawing.Color.Black;
            this.hWindowControl2.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl2.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl2.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl2.Margin = new System.Windows.Forms.Padding(2);
            this.hWindowControl2.Name = "hWindowControl2";
            this.hWindowControl2.Size = new System.Drawing.Size(190, 165);
            this.hWindowControl2.TabIndex = 2;
            this.hWindowControl2.WindowSize = new System.Drawing.Size(190, 165);
            // 
            // listView_vision_message
            // 
            this.listView_vision_message.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView_vision_message.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(71)))), ((int)(((byte)(74)))));
            this.listView_vision_message.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView_vision_message.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listView_vision_message.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listView_vision_message.FullRowSelect = true;
            this.listView_vision_message.GridLines = true;
            this.listView_vision_message.HideSelection = false;
            this.listView_vision_message.LabelWrap = false;
            this.listView_vision_message.Location = new System.Drawing.Point(196, 172);
            this.listView_vision_message.Name = "listView_vision_message";
            this.listView_vision_message.Size = new System.Drawing.Size(188, 164);
            this.listView_vision_message.TabIndex = 28;
            this.listView_vision_message.UseCompatibleStateImageBehavior = false;
            this.listView_vision_message.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "时间";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "相机";
            this.columnHeader5.Width = 50;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "说明";
            this.columnHeader6.Width = 500;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rBtnLiveCamera3);
            this.panel3.Controls.Add(this.hWindowControl3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(2, 171);
            this.panel3.Margin = new System.Windows.Forms.Padding(2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(189, 166);
            this.panel3.TabIndex = 31;
            // 
            // rBtnLiveCamera3
            // 
            this.rBtnLiveCamera3.AutoSize = true;
            this.rBtnLiveCamera3.Location = new System.Drawing.Point(2, 1);
            this.rBtnLiveCamera3.Name = "rBtnLiveCamera3";
            this.rBtnLiveCamera3.Size = new System.Drawing.Size(49, 21);
            this.rBtnLiveCamera3.TabIndex = 5;
            this.rBtnLiveCamera3.Text = "Live";
            this.rBtnLiveCamera3.UseVisualStyleBackColor = true;
            this.rBtnLiveCamera3.Click += new System.EventHandler(this.rBtnLiveCamera3_Click);
            // 
            // hWindowControl3
            // 
            this.hWindowControl3.BackColor = System.Drawing.Color.Black;
            this.hWindowControl3.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl3.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl3.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl3.Margin = new System.Windows.Forms.Padding(2);
            this.hWindowControl3.Name = "hWindowControl3";
            this.hWindowControl3.Size = new System.Drawing.Size(189, 166);
            this.hWindowControl3.TabIndex = 3;
            this.hWindowControl3.WindowSize = new System.Drawing.Size(189, 166);
            // 
            // tabPageStatistics
            // 
            this.tabPageStatistics.Controls.Add(this.ucStatistics1);
            this.tabPageStatistics.Location = new System.Drawing.Point(4, 29);
            this.tabPageStatistics.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageStatistics.Name = "tabPageStatistics";
            this.tabPageStatistics.Size = new System.Drawing.Size(387, 339);
            this.tabPageStatistics.TabIndex = 0;
            this.tabPageStatistics.Text = "Statistics";
            this.tabPageStatistics.UseVisualStyleBackColor = true;
            // 
            // ucStatistics1
            // 
            this.ucStatistics1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucStatistics1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucStatistics1.Location = new System.Drawing.Point(0, 0);
            this.ucStatistics1.Margin = new System.Windows.Forms.Padding(0);
            this.ucStatistics1.Name = "ucStatistics1";
            this.ucStatistics1.Size = new System.Drawing.Size(387, 339);
            this.ucStatistics1.TabIndex = 0;
            // 
            // tabPageUPHChart
            // 
            this.tabPageUPHChart.Controls.Add(this.uphChart1);
            this.tabPageUPHChart.Location = new System.Drawing.Point(4, 29);
            this.tabPageUPHChart.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageUPHChart.Name = "tabPageUPHChart";
            this.tabPageUPHChart.Size = new System.Drawing.Size(387, 339);
            this.tabPageUPHChart.TabIndex = 1;
            this.tabPageUPHChart.Text = "UPH Chart";
            this.tabPageUPHChart.UseVisualStyleBackColor = true;
            // 
            // uphChart1
            // 
            this.uphChart1.AutoScroll = true;
            this.uphChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uphChart1.Location = new System.Drawing.Point(0, 0);
            this.uphChart1.Margin = new System.Windows.Forms.Padding(4);
            this.uphChart1.Name = "uphChart1";
            this.uphChart1.Size = new System.Drawing.Size(387, 339);
            this.uphChart1.TabIndex = 0;
            // 
            // tabPagSelectBin
            // 
            this.tabPagSelectBin.Controls.Add(this.tableLayoutPanel5);
            this.tabPagSelectBin.Location = new System.Drawing.Point(4, 29);
            this.tabPagSelectBin.Margin = new System.Windows.Forms.Padding(2);
            this.tabPagSelectBin.Name = "tabPagSelectBin";
            this.tabPagSelectBin.Padding = new System.Windows.Forms.Padding(2);
            this.tabPagSelectBin.Size = new System.Drawing.Size(387, 339);
            this.tabPagSelectBin.TabIndex = 4;
            this.tabPagSelectBin.Text = "Select Bin";
            this.tabPagSelectBin.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.73589F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.52822F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.96163F));
            this.tableLayoutPanel5.Controls.Add(this.groupBox5, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.btnSelectBinOK, 2, 1);
            this.tableLayoutPanel5.Controls.Add(this.ucModeParams_SelectDut_Bin1, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.08059F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.91941F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(383, 335);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.tableLayoutPanel5.SetColumnSpan(this.groupBox5, 2);
            this.groupBox5.Controls.Add(this.rdbtnLoadRTray);
            this.groupBox5.Controls.Add(this.rdbtnLoadLTray);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(0, 288);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox5.Size = new System.Drawing.Size(309, 47);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Load Tray Select";
            // 
            // rdbtnLoadRTray
            // 
            this.rdbtnLoadRTray.AutoSize = true;
            this.rdbtnLoadRTray.Location = new System.Drawing.Point(136, 14);
            this.rdbtnLoadRTray.Margin = new System.Windows.Forms.Padding(2);
            this.rdbtnLoadRTray.Name = "rdbtnLoadRTray";
            this.rdbtnLoadRTray.Size = new System.Drawing.Size(59, 21);
            this.rdbtnLoadRTray.TabIndex = 1;
            this.rdbtnLoadRTray.TabStop = true;
            this.rdbtnLoadRTray.Text = "load2";
            this.rdbtnLoadRTray.UseVisualStyleBackColor = true;
            // 
            // rdbtnLoadLTray
            // 
            this.rdbtnLoadLTray.AutoSize = true;
            this.rdbtnLoadLTray.Location = new System.Drawing.Point(20, 14);
            this.rdbtnLoadLTray.Margin = new System.Windows.Forms.Padding(2);
            this.rdbtnLoadLTray.Name = "rdbtnLoadLTray";
            this.rdbtnLoadLTray.Size = new System.Drawing.Size(59, 21);
            this.rdbtnLoadLTray.TabIndex = 0;
            this.rdbtnLoadLTray.TabStop = true;
            this.rdbtnLoadLTray.Text = "load1";
            this.rdbtnLoadLTray.UseVisualStyleBackColor = true;
            // 
            // btnSelectBinOK
            // 
            this.btnSelectBinOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectBinOK.Location = new System.Drawing.Point(309, 294);
            this.btnSelectBinOK.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.btnSelectBinOK.Name = "btnSelectBinOK";
            this.btnSelectBinOK.Size = new System.Drawing.Size(74, 41);
            this.btnSelectBinOK.TabIndex = 2;
            this.btnSelectBinOK.Text = "Execute";
            this.btnSelectBinOK.UseVisualStyleBackColor = true;
            this.btnSelectBinOK.Click += new System.EventHandler(this.btnSelectBinOK_Click);
            // 
            // ucModeParams_SelectDut_Bin1
            // 
            this.ucModeParams_SelectDut_Bin1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel5.SetColumnSpan(this.ucModeParams_SelectDut_Bin1, 3);
            this.ucModeParams_SelectDut_Bin1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucModeParams_SelectDut_Bin1.Location = new System.Drawing.Point(3, 4);
            this.ucModeParams_SelectDut_Bin1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucModeParams_SelectDut_Bin1.Name = "ucModeParams_SelectDut_Bin1";
            this.ucModeParams_SelectDut_Bin1.Size = new System.Drawing.Size(377, 280);
            this.ucModeParams_SelectDut_Bin1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ucRunLog1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 123);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(228, 140);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "State monitoring";
            // 
            // ucRunLog1
            // 
            this.ucRunLog1.AutoScroll = true;
            this.ucRunLog1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucRunLog1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucRunLog1.Location = new System.Drawing.Point(3, 16);
            this.ucRunLog1.Margin = new System.Windows.Forms.Padding(4);
            this.ucRunLog1.Name = "ucRunLog1";
            this.ucRunLog1.Size = new System.Drawing.Size(222, 122);
            this.ucRunLog1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox2, 2);
            this.groupBox2.Controls.Add(this.ucErrorList1);
            this.groupBox2.Location = new System.Drawing.Point(237, 123);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox2.Size = new System.Drawing.Size(155, 140);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Error message";
            // 
            // ucErrorList1
            // 
            this.ucErrorList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucErrorList1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucErrorList1.Location = new System.Drawing.Point(3, 16);
            this.ucErrorList1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucErrorList1.Name = "ucErrorList1";
            this.ucErrorList1.Size = new System.Drawing.Size(149, 122);
            this.ucErrorList1.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ucSocket_New1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox3.Location = new System.Drawing.Point(234, 0);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(83, 121);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Socket Set";
            // 
            // ucSocket_New1
            // 
            this.ucSocket_New1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSocket_New1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucSocket_New1.Location = new System.Drawing.Point(2, 18);
            this.ucSocket_New1.Margin = new System.Windows.Forms.Padding(2);
            this.ucSocket_New1.Name = "ucSocket_New1";
            this.ucSocket_New1.Size = new System.Drawing.Size(79, 101);
            this.ucSocket_New1.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tableLayoutPanel3);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(317, 0);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(78, 121);
            this.groupBox4.TabIndex = 11;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Mode Set";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.btnNormal, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnManual, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnSelectSN, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.btnGRR, 0, 3);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tableLayoutPanel3.Location = new System.Drawing.Point(2, 16);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(74, 103);
            this.tableLayoutPanel3.TabIndex = 12;
            // 
            // btnNormal
            // 
            this.btnNormal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNormal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNormal.Location = new System.Drawing.Point(0, 0);
            this.btnNormal.Margin = new System.Windows.Forms.Padding(0);
            this.btnNormal.Name = "btnNormal";
            this.btnNormal.Size = new System.Drawing.Size(74, 25);
            this.btnNormal.TabIndex = 0;
            this.btnNormal.Text = "Normal";
            this.btnNormal.UseVisualStyleBackColor = true;
            this.btnNormal.Click += new System.EventHandler(this.btnNormal_Click);
            // 
            // btnManual
            // 
            this.btnManual.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnManual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnManual.Location = new System.Drawing.Point(0, 25);
            this.btnManual.Margin = new System.Windows.Forms.Padding(0);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(74, 25);
            this.btnManual.TabIndex = 1;
            this.btnManual.Text = "Manual";
            this.btnManual.UseVisualStyleBackColor = true;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // btnSelectSN
            // 
            this.btnSelectSN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectSN.Location = new System.Drawing.Point(0, 50);
            this.btnSelectSN.Margin = new System.Windows.Forms.Padding(0);
            this.btnSelectSN.Name = "btnSelectSN";
            this.btnSelectSN.Size = new System.Drawing.Size(74, 25);
            this.btnSelectSN.TabIndex = 2;
            this.btnSelectSN.Text = "Select SN";
            this.btnSelectSN.UseVisualStyleBackColor = true;
            this.btnSelectSN.Click += new System.EventHandler(this.btnSelectSN_Click);
            // 
            // btnGRR
            // 
            this.btnGRR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGRR.Location = new System.Drawing.Point(0, 75);
            this.btnGRR.Margin = new System.Windows.Forms.Padding(0);
            this.btnGRR.Name = "btnGRR";
            this.btnGRR.Size = new System.Drawing.Size(74, 28);
            this.btnGRR.TabIndex = 3;
            this.btnGRR.Text = "GRR";
            this.btnGRR.UseVisualStyleBackColor = true;
            this.btnGRR.Click += new System.EventHandler(this.btnGRR_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ucStations1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ucTrays1);
            this.splitContainer2.Size = new System.Drawing.Size(663, 637);
            this.splitContainer2.SplitterDistance = 192;
            this.splitContainer2.SplitterWidth = 2;
            this.splitContainer2.TabIndex = 0;
            // 
            // ucStations1
            // 
            this.ucStations1.BackColor = System.Drawing.SystemColors.Control;
            this.ucStations1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucStations1.Location = new System.Drawing.Point(0, 0);
            this.ucStations1.Margin = new System.Windows.Forms.Padding(4);
            this.ucStations1.Name = "ucStations1";
            this.ucStations1.Size = new System.Drawing.Size(663, 192);
            this.ucStations1.TabIndex = 0;
            // 
            // ucTrays1
            // 
            this.ucTrays1.AutoScroll = true;
            this.ucTrays1.BackColor = System.Drawing.SystemColors.Control;
            this.ucTrays1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucTrays1.Location = new System.Drawing.Point(0, 0);
            this.ucTrays1.Margin = new System.Windows.Forms.Padding(4);
            this.ucTrays1.Name = "ucTrays1";
            this.ucTrays1.Size = new System.Drawing.Size(663, 443);
            this.ucTrays1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // UCMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCMain";
            this.Size = new System.Drawing.Size(1060, 637);
            this.Load += new System.EventHandler(this.UCMain_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageVision.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabPageStatistics.ResumeLayout(false);
            this.tabPageUPHChart.ResumeLayout(false);
            this.tabPagSelectBin.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private UCStations ucStations1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageStatistics;
        private UCStatistics ucStatistics1;
        private System.Windows.Forms.TabPage tabPageUPHChart;
        private CYGKit.Factory.UPH.UPHChart uphChart1;
        private System.Windows.Forms.Panel panelLotInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private UCRunLog ucRunLog1;
        private System.Windows.Forms.GroupBox groupBox2;
        private UCErrorList ucErrorList1;
        private System.Windows.Forms.TabPage tabPageVision;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        public System.Windows.Forms.ListView listView_vision_message;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.GroupBox groupBox3;
        private UCSocket_New ucSocket_New1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnNormal;
        private System.Windows.Forms.Button btnManual;
        private System.Windows.Forms.TabPage tabPagSelectBin;
        public UCTrays ucTrays1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton rdbtnLoadRTray;
        private System.Windows.Forms.RadioButton rdbtnLoadLTray;
        private System.Windows.Forms.Button btnSelectBinOK;
        private RunModeConfig.UCModeParams_SelectDut_Bin ucModeParams_SelectDut_Bin1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnSelectSN;
        private System.Windows.Forms.Button btnGRR;
        private System.Windows.Forms.Panel panel1;
        public HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Panel panel2;
        public HalconDotNet.HWindowControl hWindowControl2;
        private System.Windows.Forms.Panel panel3;
        public HalconDotNet.HWindowControl hWindowControl3;
        public System.Windows.Forms.CheckBox rBtnLiveCamera1;
        public System.Windows.Forms.CheckBox rBtnLiveCamera2;
        public System.Windows.Forms.CheckBox rBtnLiveCamera3;
    }
}
