namespace Poc2Auto.GUI.UCModeUI.UCAxisesCylinders
{
    partial class UC_SingleAxisOperation
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
            this.LabelTitle = new System.Windows.Forms.Label();
            this.ListBoxDisplay = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.BtnGoTo = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.BtnJogSub = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.BtnReset = new System.Windows.Forms.Button();
            this.BtnHome = new System.Windows.Forms.Button();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.LabNel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnJogAdd = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.LabPel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TextRunVeloctity = new System.Windows.Forms.TextBox();
            this.CheckIsEnable = new System.Windows.Forms.CheckBox();
            this.TextActualVel = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextTargetPos = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TextActualPos = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // LabelTitle
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.LabelTitle, 2);
            this.LabelTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LabelTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LabelTitle.Location = new System.Drawing.Point(0, 0);
            this.LabelTitle.Margin = new System.Windows.Forms.Padding(0);
            this.LabelTitle.Name = "LabelTitle";
            this.LabelTitle.Size = new System.Drawing.Size(488, 37);
            this.LabelTitle.TabIndex = 0;
            this.LabelTitle.Text = "AxisName";
            this.LabelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ListBoxDisplay
            // 
            this.ListBoxDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListBoxDisplay.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ListBoxDisplay.FormattingEnabled = true;
            this.ListBoxDisplay.HorizontalScrollbar = true;
            this.ListBoxDisplay.IntegralHeight = false;
            this.ListBoxDisplay.ItemHeight = 20;
            this.ListBoxDisplay.Location = new System.Drawing.Point(2, 22);
            this.ListBoxDisplay.Margin = new System.Windows.Forms.Padding(0);
            this.ListBoxDisplay.Name = "ListBoxDisplay";
            this.ListBoxDisplay.ScrollAlwaysVisible = true;
            this.ListBoxDisplay.Size = new System.Drawing.Size(217, 283);
            this.ListBoxDisplay.TabIndex = 1;
            this.ListBoxDisplay.SelectedIndexChanged += new System.EventHandler(this.ListBoxDisplay_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(0, 52);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 24);
            this.label5.TabIndex = 2;
            this.label5.Text = "ActVel[mm/s]";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(0, 80);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 40);
            this.label6.TabIndex = 3;
            this.label6.Text = "ActPos[mm]";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // BtnGoTo
            // 
            this.BtnGoTo.AutoSize = true;
            this.BtnGoTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnGoTo.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnGoTo.Location = new System.Drawing.Point(0, 200);
            this.BtnGoTo.Margin = new System.Windows.Forms.Padding(0);
            this.BtnGoTo.Name = "BtnGoTo";
            this.BtnGoTo.Size = new System.Drawing.Size(97, 40);
            this.BtnGoTo.TabIndex = 9;
            this.BtnGoTo.Text = "GoTo";
            this.BtnGoTo.UseVisualStyleBackColor = true;
            this.BtnGoTo.Click += new System.EventHandler(this.BtnGoTo_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(221, 37);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel4.SetRowSpan(this.groupBox1, 6);
            this.groupBox1.Size = new System.Drawing.Size(267, 307);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.70412F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.21348F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.BtnJogSub, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.BtnStop, 2, 6);
            this.tableLayoutPanel2.Controls.Add(this.BtnReset, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.BtnHome, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.BtnJogAdd, 2, 5);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.BtnGoTo, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.TextRunVeloctity, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.CheckIsEnable, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.TextActualVel, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.TextTargetPos, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.TextActualPos, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 22);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.31602F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.25541F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(263, 283);
            this.tableLayoutPanel2.TabIndex = 21;
            // 
            // BtnJogSub
            // 
            this.BtnJogSub.AutoSize = true;
            this.BtnJogSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnJogSub.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnJogSub.Location = new System.Drawing.Point(97, 200);
            this.BtnJogSub.Margin = new System.Windows.Forms.Padding(0);
            this.BtnJogSub.Name = "BtnJogSub";
            this.BtnJogSub.Size = new System.Drawing.Size(77, 40);
            this.BtnJogSub.TabIndex = 17;
            this.BtnJogSub.Text = "Jog-";
            this.BtnJogSub.UseVisualStyleBackColor = true;
            this.BtnJogSub.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnJogSub_MouseDown);
            this.BtnJogSub.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnJogSub_MouseUp);
            // 
            // BtnStop
            // 
            this.BtnStop.AutoSize = true;
            this.BtnStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnStop.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnStop.Location = new System.Drawing.Point(174, 240);
            this.BtnStop.Margin = new System.Windows.Forms.Padding(0);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(89, 43);
            this.BtnStop.TabIndex = 20;
            this.BtnStop.Text = "Stop";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // BtnReset
            // 
            this.BtnReset.AutoSize = true;
            this.BtnReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnReset.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnReset.Location = new System.Drawing.Point(97, 240);
            this.BtnReset.Margin = new System.Windows.Forms.Padding(0);
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.Size = new System.Drawing.Size(77, 43);
            this.BtnReset.TabIndex = 19;
            this.BtnReset.Text = "Reset";
            this.BtnReset.UseVisualStyleBackColor = true;
            this.BtnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // BtnHome
            // 
            this.BtnHome.AutoSize = true;
            this.BtnHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnHome.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnHome.Location = new System.Drawing.Point(0, 240);
            this.BtnHome.Margin = new System.Windows.Forms.Padding(0);
            this.BtnHome.Name = "BtnHome";
            this.BtnHome.Size = new System.Drawing.Size(97, 43);
            this.BtnHome.TabIndex = 10;
            this.BtnHome.Text = "HomeGo";
            this.BtnHome.UseVisualStyleBackColor = true;
            this.BtnHome.Click += new System.EventHandler(this.BtnHome_Click);
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.LabNel, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(174, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(89, 49);
            this.tableLayoutPanel5.TabIndex = 22;
            // 
            // LabNel
            // 
            this.LabNel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.LabNel.BackColor = System.Drawing.Color.White;
            this.LabNel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabNel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LabNel.Location = new System.Drawing.Point(22, 13);
            this.LabNel.Margin = new System.Windows.Forms.Padding(0);
            this.LabNel.Name = "LabNel";
            this.LabNel.Size = new System.Drawing.Size(22, 22);
            this.LabNel.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(44, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 49);
            this.label3.TabIndex = 9;
            this.label3.Text = "NEL";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnJogAdd
            // 
            this.BtnJogAdd.AutoSize = true;
            this.BtnJogAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnJogAdd.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BtnJogAdd.Location = new System.Drawing.Point(174, 200);
            this.BtnJogAdd.Margin = new System.Windows.Forms.Padding(0);
            this.BtnJogAdd.Name = "BtnJogAdd";
            this.BtnJogAdd.Size = new System.Drawing.Size(89, 40);
            this.BtnJogAdd.TabIndex = 18;
            this.BtnJogAdd.Text = "Jog+";
            this.BtnJogAdd.UseVisualStyleBackColor = true;
            this.BtnJogAdd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnJogAdd_MouseDown);
            this.BtnJogAdd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnJogAdd_MouseUp);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.LabPel, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label8, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(97, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(77, 49);
            this.tableLayoutPanel3.TabIndex = 22;
            // 
            // LabPel
            // 
            this.LabPel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.LabPel.BackColor = System.Drawing.Color.White;
            this.LabPel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabPel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LabPel.Location = new System.Drawing.Point(16, 13);
            this.LabPel.Margin = new System.Windows.Forms.Padding(0);
            this.LabPel.Name = "LabPel";
            this.LabPel.Size = new System.Drawing.Size(22, 22);
            this.LabPel.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(38, 0);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 49);
            this.label8.TabIndex = 14;
            this.label8.Text = "PEL";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextRunVeloctity
            // 
            this.TextRunVeloctity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.TextRunVeloctity, 2);
            this.TextRunVeloctity.Location = new System.Drawing.Point(97, 166);
            this.TextRunVeloctity.Margin = new System.Windows.Forms.Padding(0);
            this.TextRunVeloctity.Name = "TextRunVeloctity";
            this.TextRunVeloctity.Size = new System.Drawing.Size(166, 27);
            this.TextRunVeloctity.TabIndex = 14;
            this.TextRunVeloctity.Text = "0";
            this.TextRunVeloctity.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextRunVeloctity_KeyPress);
            // 
            // CheckIsEnable
            // 
            this.CheckIsEnable.AutoSize = true;
            this.CheckIsEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckIsEnable.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CheckIsEnable.Location = new System.Drawing.Point(0, 0);
            this.CheckIsEnable.Margin = new System.Windows.Forms.Padding(0);
            this.CheckIsEnable.Name = "CheckIsEnable";
            this.CheckIsEnable.Size = new System.Drawing.Size(97, 49);
            this.CheckIsEnable.TabIndex = 16;
            this.CheckIsEnable.Text = "Enable";
            this.CheckIsEnable.UseVisualStyleBackColor = true;
            this.CheckIsEnable.Click += new System.EventHandler(this.CheckIsEnable_Click);
            // 
            // TextActualVel
            // 
            this.TextActualVel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.TextActualVel, 2);
            this.TextActualVel.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextActualVel.Location = new System.Drawing.Point(97, 51);
            this.TextActualVel.Margin = new System.Windows.Forms.Padding(0);
            this.TextActualVel.Name = "TextActualVel";
            this.TextActualVel.ReadOnly = true;
            this.TextActualVel.Size = new System.Drawing.Size(166, 27);
            this.TextActualVel.TabIndex = 6;
            this.TextActualVel.Text = "0";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(0, 170);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "Vel[mm/s]";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextTargetPos
            // 
            this.TextTargetPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.TextTargetPos, 2);
            this.TextTargetPos.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextTargetPos.Location = new System.Drawing.Point(97, 126);
            this.TextTargetPos.Margin = new System.Windows.Forms.Padding(0);
            this.TextTargetPos.Name = "TextTargetPos";
            this.TextTargetPos.Size = new System.Drawing.Size(166, 27);
            this.TextTargetPos.TabIndex = 0;
            this.TextTargetPos.Text = "0";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(0, 120);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 40);
            this.label4.TabIndex = 14;
            this.label4.Text = "ObjPos[mm]";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextActualPos
            // 
            this.TextActualPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.TextActualPos, 2);
            this.TextActualPos.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextActualPos.Location = new System.Drawing.Point(97, 86);
            this.TextActualPos.Margin = new System.Windows.Forms.Padding(0);
            this.TextActualPos.Name = "TextActualPos";
            this.TextActualPos.ReadOnly = true;
            this.TextActualPos.Size = new System.Drawing.Size(166, 27);
            this.TextActualPos.TabIndex = 5;
            this.TextActualPos.Text = "0";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ListBoxDisplay);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 37);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel4.SetRowSpan(this.groupBox2, 6);
            this.groupBox2.Size = new System.Drawing.Size(221, 307);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Axis pos list";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.35519F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54.64481F));
            this.tableLayoutPanel4.Controls.Add(this.LabelTitle, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.groupBox1, 1, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 7;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.78067F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.47212F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(488, 344);
            this.tableLayoutPanel4.TabIndex = 12;
            // 
            // UC_SingleAxisOperation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tableLayoutPanel4);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MinimumSize = new System.Drawing.Size(488, 266);
            this.Name = "UC_SingleAxisOperation";
            this.Size = new System.Drawing.Size(488, 344);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LabelTitle;
        private System.Windows.Forms.ListBox ListBoxDisplay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BtnGoTo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox CheckIsEnable;
        private System.Windows.Forms.Label LabNel;
        private System.Windows.Forms.Label LabPel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button BtnHome;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Button BtnReset;
        private System.Windows.Forms.Button BtnJogAdd;
        private System.Windows.Forms.Button BtnJogSub;
        private System.Windows.Forms.TextBox TextRunVeloctity;
        private System.Windows.Forms.TextBox TextActualVel;
        private System.Windows.Forms.TextBox TextActualPos;
        private System.Windows.Forms.TextBox TextTargetPos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    }
}
