namespace DragonFlex.GUI.Factory
{
    partial class UCModeManual
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
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.uC_DIOs1 = new AlcUtility.PlcDriver.UC_DIOs();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.flowPanelAxis = new System.Windows.Forms.FlowLayoutPanel();
            this.MenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.axisOperateToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.uC_Cylinders1 = new Poc2Auto.GUI.UCModeUI.UCAxisesCylinders.UC_Cylinders();
            this.btnPullPin = new System.Windows.Forms.Button();
            this.btnReleasePin = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.uC_ModeSemiAuto1 = new Poc2Auto.GUI.UCModeUI.UCSemiAuto.UC_ModeSemiAuto();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ucRecipe_New1 = new Poc2Auto.GUI.UCRecipe_New();
            this.tabPageTurntableDebug = new System.Windows.Forms.TabPage();
            this.lbSocketID = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnPullPutter = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnPushPutter = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRotate = new System.Windows.Forms.Button();
            this.btnZAxisDown = new System.Windows.Forms.Button();
            this.btnZAxisUp = new System.Windows.Forms.Button();
            this.btnSocketClose = new System.Windows.Forms.Button();
            this.btnOpenSocket = new System.Windows.Forms.Button();
            this.tabControl3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.MenuStrip.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPageTurntableDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage1);
            this.tabControl3.Controls.Add(this.tabPage4);
            this.tabControl3.Controls.Add(this.tabPage5);
            this.tabControl3.Controls.Add(this.tabPage2);
            this.tabControl3.Controls.Add(this.tabPage3);
            this.tabControl3.Controls.Add(this.tabPageTurntableDebug);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(951, 476);
            this.tabControl3.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.uC_DIOs1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(943, 443);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DIO";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // uC_DIOs1
            // 
            this.uC_DIOs1.DataSource = null;
            this.uC_DIOs1.DIsPropName = null;
            this.uC_DIOs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_DIOs1.DOsPropName = null;
            this.uC_DIOs1.Interval = 200;
            this.uC_DIOs1.IsInnerUpdatingOpen = true;
            this.uC_DIOs1.Location = new System.Drawing.Point(2, 2);
            this.uC_DIOs1.Margin = new System.Windows.Forms.Padding(0);
            this.uC_DIOs1.Name = "uC_DIOs1";
            this.uC_DIOs1.PlcDiVarName = null;
            this.uC_DIOs1.PlcDoVarName = null;
            this.uC_DIOs1.PlcVarName = null;
            this.uC_DIOs1.Size = new System.Drawing.Size(939, 439);
            this.uC_DIOs1.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.AutoScroll = true;
            this.tabPage4.Controls.Add(this.flowPanelAxis);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(943, 443);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Axis";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // flowPanelAxis
            // 
            this.flowPanelAxis.AutoScroll = true;
            this.flowPanelAxis.ContextMenuStrip = this.MenuStrip;
            this.flowPanelAxis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanelAxis.Location = new System.Drawing.Point(0, 0);
            this.flowPanelAxis.Margin = new System.Windows.Forms.Padding(4);
            this.flowPanelAxis.Name = "flowPanelAxis";
            this.flowPanelAxis.Size = new System.Drawing.Size(943, 443);
            this.flowPanelAxis.TabIndex = 0;
            // 
            // MenuStrip
            // 
            this.MenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveLocationToolStripMenuItem,
            this.axisOperateToolStrip});
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(206, 52);
            // 
            // saveLocationToolStripMenuItem
            // 
            this.saveLocationToolStripMenuItem.Name = "saveLocationToolStripMenuItem";
            this.saveLocationToolStripMenuItem.Size = new System.Drawing.Size(205, 24);
            this.saveLocationToolStripMenuItem.Text = "Location Operate";
            // 
            // axisOperateToolStrip
            // 
            this.axisOperateToolStrip.Name = "axisOperateToolStrip";
            this.axisOperateToolStrip.Size = new System.Drawing.Size(205, 24);
            this.axisOperateToolStrip.Text = "Axis operate";
            // 
            // tabPage5
            // 
            this.tabPage5.AutoScroll = true;
            this.tabPage5.Controls.Add(this.tableLayoutPanel1);
            this.tabPage5.Location = new System.Drawing.Point(4, 29);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(943, 443);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Cylinder";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.44926F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.798496F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.75224F));
            this.tableLayoutPanel1.Controls.Add(this.uC_Cylinders1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnPullPin, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnReleasePin, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.247711F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.247711F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.09293F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.41165F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(943, 443);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // uC_Cylinders1
            // 
            this.uC_Cylinders1.CYL_COUNT = 0;
            this.uC_Cylinders1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_Cylinders1.Location = new System.Drawing.Point(3, 2);
            this.uC_Cylinders1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uC_Cylinders1.Name = "uC_Cylinders1";
            this.uC_Cylinders1.PlcDriver = null;
            this.tableLayoutPanel1.SetRowSpan(this.uC_Cylinders1, 3);
            this.uC_Cylinders1.Size = new System.Drawing.Size(479, 290);
            this.uC_Cylinders1.TabIndex = 0;
            // 
            // btnPullPin
            // 
            this.btnPullPin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPullPin.Location = new System.Drawing.Point(485, 0);
            this.btnPullPin.Margin = new System.Windows.Forms.Padding(0);
            this.btnPullPin.Name = "btnPullPin";
            this.btnPullPin.Size = new System.Drawing.Size(82, 32);
            this.btnPullPin.TabIndex = 1;
            this.btnPullPin.Text = "拉Pin针";
            this.btnPullPin.UseVisualStyleBackColor = true;
            this.btnPullPin.Click += new System.EventHandler(this.btnPullPin_Click);
            // 
            // btnReleasePin
            // 
            this.btnReleasePin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReleasePin.Location = new System.Drawing.Point(485, 32);
            this.btnReleasePin.Margin = new System.Windows.Forms.Padding(0);
            this.btnReleasePin.Name = "btnReleasePin";
            this.btnReleasePin.Size = new System.Drawing.Size(82, 32);
            this.btnReleasePin.TabIndex = 2;
            this.btnReleasePin.Text = "放Pin针";
            this.btnReleasePin.UseVisualStyleBackColor = true;
            this.btnReleasePin.Click += new System.EventHandler(this.btnReleasePin_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.uC_ModeSemiAuto1);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(943, 443);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "SemiAuto";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // uC_ModeSemiAuto1
            // 
            this.uC_ModeSemiAuto1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_ModeSemiAuto1.Location = new System.Drawing.Point(4, 4);
            this.uC_ModeSemiAuto1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.uC_ModeSemiAuto1.Name = "uC_ModeSemiAuto1";
            this.uC_ModeSemiAuto1.Size = new System.Drawing.Size(935, 435);
            this.uC_ModeSemiAuto1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.ucRecipe_New1);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage3.Size = new System.Drawing.Size(943, 443);
            this.tabPage3.TabIndex = 4;
            this.tabPage3.Text = "Recipe";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ucRecipe_New1
            // 
            this.ucRecipe_New1.DefaultRecipe = "";
            this.ucRecipe_New1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucRecipe_New1.FilePath = null;
            this.ucRecipe_New1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucRecipe_New1.Location = new System.Drawing.Point(4, 4);
            this.ucRecipe_New1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucRecipe_New1.Name = "ucRecipe_New1";
            this.ucRecipe_New1.PlcDriver = null;
            this.ucRecipe_New1.Size = new System.Drawing.Size(935, 435);
            this.ucRecipe_New1.TabIndex = 0;
            this.ucRecipe_New1.Title = "Recipe Config";
            // 
            // tabPageTurntableDebug
            // 
            this.tabPageTurntableDebug.Controls.Add(this.lbSocketID);
            this.tabPageTurntableDebug.Controls.Add(this.label6);
            this.tabPageTurntableDebug.Controls.Add(this.label10);
            this.tabPageTurntableDebug.Controls.Add(this.label9);
            this.tabPageTurntableDebug.Controls.Add(this.label7);
            this.tabPageTurntableDebug.Controls.Add(this.label4);
            this.tabPageTurntableDebug.Controls.Add(this.btnPullPutter);
            this.tabPageTurntableDebug.Controls.Add(this.label8);
            this.tabPageTurntableDebug.Controls.Add(this.btnPushPutter);
            this.tabPageTurntableDebug.Controls.Add(this.label5);
            this.tabPageTurntableDebug.Controls.Add(this.label3);
            this.tabPageTurntableDebug.Controls.Add(this.label2);
            this.tabPageTurntableDebug.Controls.Add(this.label1);
            this.tabPageTurntableDebug.Controls.Add(this.btnRotate);
            this.tabPageTurntableDebug.Controls.Add(this.btnZAxisDown);
            this.tabPageTurntableDebug.Controls.Add(this.btnZAxisUp);
            this.tabPageTurntableDebug.Controls.Add(this.btnSocketClose);
            this.tabPageTurntableDebug.Controls.Add(this.btnOpenSocket);
            this.tabPageTurntableDebug.Location = new System.Drawing.Point(4, 29);
            this.tabPageTurntableDebug.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageTurntableDebug.Name = "tabPageTurntableDebug";
            this.tabPageTurntableDebug.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageTurntableDebug.Size = new System.Drawing.Size(943, 443);
            this.tabPageTurntableDebug.TabIndex = 5;
            this.tabPageTurntableDebug.Text = "Debug";
            this.tabPageTurntableDebug.UseVisualStyleBackColor = true;
            // 
            // lbSocketID
            // 
            this.lbSocketID.AutoSize = true;
            this.lbSocketID.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSocketID.Location = new System.Drawing.Point(365, 164);
            this.lbSocketID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSocketID.Name = "lbSocketID";
            this.lbSocketID.Size = new System.Drawing.Size(0, 27);
            this.lbSocketID.TabIndex = 20;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(229, 164);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 27);
            this.label6.TabIndex = 19;
            this.label6.Text = "当前Socket:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(51, 74);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(27, 54);
            this.label10.TabIndex = 18;
            this.label10.Text = "^\r\n^\r\n";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(51, 230);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(27, 54);
            this.label9.TabIndex = 17;
            this.label9.Text = "^\r\n^\r\n";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(49, 279);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 27);
            this.label7.TabIndex = 16;
            this.label7.Text = "<<<<";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(411, 279);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 27);
            this.label4.TabIndex = 15;
            this.label4.Text = "<<<<";
            // 
            // btnPullPutter
            // 
            this.btnPullPutter.Location = new System.Drawing.Point(485, 258);
            this.btnPullPutter.Margin = new System.Windows.Forms.Padding(2);
            this.btnPullPutter.Name = "btnPullPutter";
            this.btnPullPutter.Size = new System.Drawing.Size(100, 72);
            this.btnPullPutter.TabIndex = 14;
            this.btnPullPutter.Text = "拉Putter";
            this.btnPullPutter.UseVisualStyleBackColor = true;
            this.btnPullPutter.Click += new System.EventHandler(this.btnPullPutter_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(229, 279);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 27);
            this.label8.TabIndex = 13;
            this.label8.Text = "<<<<";
            // 
            // btnPushPutter
            // 
            this.btnPushPutter.Location = new System.Drawing.Point(485, 20);
            this.btnPushPutter.Margin = new System.Windows.Forms.Padding(2);
            this.btnPushPutter.Name = "btnPushPutter";
            this.btnPushPutter.Size = new System.Drawing.Size(100, 72);
            this.btnPushPutter.TabIndex = 11;
            this.btnPushPutter.Text = "推Putter";
            this.btnPushPutter.UseVisualStyleBackColor = true;
            this.btnPushPutter.Click += new System.EventHandler(this.btnPushPutter_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(534, 95);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 140);
            this.label5.TabIndex = 9;
            this.label5.Text = "V\r\nV\r\nV\r\nV\r\nV\r\nV\r\nV";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(412, 41);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 27);
            this.label3.TabIndex = 7;
            this.label3.Text = ">>>>";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(231, 41);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 27);
            this.label2.TabIndex = 6;
            this.label2.Text = ">>>>";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(51, 44);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 27);
            this.label1.TabIndex = 5;
            this.label1.Text = ">>>>";
            // 
            // btnRotate
            // 
            this.btnRotate.Location = new System.Drawing.Point(12, 132);
            this.btnRotate.Margin = new System.Windows.Forms.Padding(2);
            this.btnRotate.Name = "btnRotate";
            this.btnRotate.Size = new System.Drawing.Size(98, 92);
            this.btnRotate.TabIndex = 4;
            this.btnRotate.Text = "旋转一\r\n个工位";
            this.btnRotate.UseVisualStyleBackColor = true;
            this.btnRotate.Click += new System.EventHandler(this.btnRotate_Click);
            // 
            // btnZAxisDown
            // 
            this.btnZAxisDown.Location = new System.Drawing.Point(128, 258);
            this.btnZAxisDown.Margin = new System.Windows.Forms.Padding(2);
            this.btnZAxisDown.Name = "btnZAxisDown";
            this.btnZAxisDown.Size = new System.Drawing.Size(100, 72);
            this.btnZAxisDown.TabIndex = 3;
            this.btnZAxisDown.Text = "Z轴下降";
            this.btnZAxisDown.UseVisualStyleBackColor = true;
            this.btnZAxisDown.Click += new System.EventHandler(this.btnZAxisDown_Click);
            // 
            // btnZAxisUp
            // 
            this.btnZAxisUp.Location = new System.Drawing.Point(128, 22);
            this.btnZAxisUp.Margin = new System.Windows.Forms.Padding(2);
            this.btnZAxisUp.Name = "btnZAxisUp";
            this.btnZAxisUp.Size = new System.Drawing.Size(100, 72);
            this.btnZAxisUp.TabIndex = 2;
            this.btnZAxisUp.Text = "Z轴顶升";
            this.btnZAxisUp.UseVisualStyleBackColor = true;
            this.btnZAxisUp.Click += new System.EventHandler(this.btnZAxisUp_Click);
            // 
            // btnSocketClose
            // 
            this.btnSocketClose.Location = new System.Drawing.Point(305, 258);
            this.btnSocketClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnSocketClose.Name = "btnSocketClose";
            this.btnSocketClose.Size = new System.Drawing.Size(106, 72);
            this.btnSocketClose.TabIndex = 1;
            this.btnSocketClose.Text = "Socket 关盖";
            this.btnSocketClose.UseVisualStyleBackColor = true;
            this.btnSocketClose.Click += new System.EventHandler(this.btnSocketClose_Click);
            // 
            // btnOpenSocket
            // 
            this.btnOpenSocket.Location = new System.Drawing.Point(304, 20);
            this.btnOpenSocket.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpenSocket.Name = "btnOpenSocket";
            this.btnOpenSocket.Size = new System.Drawing.Size(106, 72);
            this.btnOpenSocket.TabIndex = 0;
            this.btnOpenSocket.Text = "Socket 开盖";
            this.btnOpenSocket.UseVisualStyleBackColor = true;
            this.btnOpenSocket.Click += new System.EventHandler(this.btnOpenSocket_Click);
            // 
            // UCModeManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tabControl3);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UCModeManual";
            this.Size = new System.Drawing.Size(951, 476);
            this.Load += new System.EventHandler(this.UCModeManual_Load);
            this.tabControl3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.MenuStrip.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPageTurntableDebug.ResumeLayout(false);
            this.tabPageTurntableDebug.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private AlcUtility.PlcDriver.UC_DIOs uC_DIOs1;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.FlowLayoutPanel flowPanelAxis;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private Poc2Auto.GUI.UCModeUI.UCSemiAuto.UC_ModeSemiAuto uC_ModeSemiAuto1;
        private System.Windows.Forms.ContextMenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem saveLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem axisOperateToolStrip;
        private System.Windows.Forms.TabPage tabPageTurntableDebug;
        private System.Windows.Forms.Button btnSocketClose;
        private System.Windows.Forms.Button btnOpenSocket;
        private System.Windows.Forms.Button btnZAxisDown;
        private System.Windows.Forms.Button btnZAxisUp;
        private System.Windows.Forms.Button btnRotate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPullPutter;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnPushPutter;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Poc2Auto.GUI.UCRecipe_New ucRecipe_New1;
        private System.Windows.Forms.Label lbSocketID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Poc2Auto.GUI.UCModeUI.UCAxisesCylinders.UC_Cylinders uC_Cylinders1;
        private System.Windows.Forms.Button btnPullPin;
        private System.Windows.Forms.Button btnReleasePin;
    }
}
