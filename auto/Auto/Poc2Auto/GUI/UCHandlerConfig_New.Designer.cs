
namespace Poc2Auto.GUI
{
    partial class UCHandlerConfig_New
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lbSocketID = new System.Windows.Forms.Label();
            this.btnClearOnlineDut = new System.Windows.Forms.Button();
            this.ckbxCloseBuzzer = new System.Windows.Forms.CheckBox();
            this.checkBoxDoorLockControl = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.btnTakeOff = new System.Windows.Forms.Button();
            this.btnAutoMark = new System.Windows.Forms.Button();
            this.btnSameTray = new System.Windows.Forms.Button();
            this.btnDoeSlip = new System.Windows.Forms.Button();
            this.btnDifferentTray = new System.Windows.Forms.Button();
            this.btnDryRun = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnClearSocketStatus = new System.Windows.Forms.Button();
            this.btnClearHandlerProessData = new System.Windows.Forms.Button();
            this.btnClearTesterProessData = new System.Windows.Forms.Button();
            this.btnVersion = new System.Windows.Forms.Button();
            this.ucBinRegionSet1 = new CYGKit.Factory.OtherUI.Test.UCBinRegionSet();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.92744F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.46372F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.52997F));
            this.tableLayoutPanel1.Controls.Add(this.ucBinRegionSet1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 77.59067F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.40933F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1268, 772);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tableLayoutPanel4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(997, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 592);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.25373F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.74627F));
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.numericUpDown1, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.numericUpDown2, 1, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 12;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(262, 566);
            this.tableLayoutPanel4.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "工站连续测试失败报警次数:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numericUpDown1.Location = new System.Drawing.Point(194, 10);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(59, 27);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(194, 40);
            this.label2.TabIndex = 2;
            this.label2.Text = "Socket连续测试失败报警次数:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numericUpDown2.Location = new System.Drawing.Point(194, 57);
            this.numericUpDown2.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(64, 27);
            this.numericUpDown2.TabIndex = 3;
            this.numericUpDown2.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tableLayoutPanel3);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 598);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox3.Size = new System.Drawing.Size(557, 174);
            this.groupBox3.TabIndex = 17;
            this.groupBox3.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 5;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.67145F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.38959F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.72352F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.99906F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.99906F));
            this.tableLayoutPanel3.Controls.Add(this.lbSocketID, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.btnClearOnlineDut, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.ckbxCloseBuzzer, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.checkBoxDoorLockControl, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnClearSocketStatus, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnClearHandlerProessData, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.btnClearTesterProessData, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnVersion, 2, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 20);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(557, 154);
            this.tableLayoutPanel3.TabIndex = 21;
            // 
            // lbSocketID
            // 
            this.lbSocketID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSocketID.Location = new System.Drawing.Point(3, 76);
            this.lbSocketID.Name = "lbSocketID";
            this.lbSocketID.Size = new System.Drawing.Size(98, 38);
            this.lbSocketID.TabIndex = 15;
            this.lbSocketID.Text = "当前Socket: 1";
            this.lbSocketID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClearOnlineDut
            // 
            this.btnClearOnlineDut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearOnlineDut.Location = new System.Drawing.Point(104, 0);
            this.btnClearOnlineDut.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearOnlineDut.Name = "btnClearOnlineDut";
            this.btnClearOnlineDut.Size = new System.Drawing.Size(108, 38);
            this.btnClearOnlineDut.TabIndex = 20;
            this.btnClearOnlineDut.Text = "Clear Online dut";
            this.btnClearOnlineDut.UseVisualStyleBackColor = true;
            this.btnClearOnlineDut.Click += new System.EventHandler(this.btnClearOnlineDut_Click);
            // 
            // ckbxCloseBuzzer
            // 
            this.ckbxCloseBuzzer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbxCloseBuzzer.AutoSize = true;
            this.ckbxCloseBuzzer.Location = new System.Drawing.Point(5, 45);
            this.ckbxCloseBuzzer.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.ckbxCloseBuzzer.Name = "ckbxCloseBuzzer";
            this.ckbxCloseBuzzer.Size = new System.Drawing.Size(99, 24);
            this.ckbxCloseBuzzer.TabIndex = 12;
            this.ckbxCloseBuzzer.Text = "Buzzer";
            this.ckbxCloseBuzzer.UseVisualStyleBackColor = true;
            this.ckbxCloseBuzzer.CheckedChanged += new System.EventHandler(this.ckbxCloseBuzzer_CheckedChanged);
            // 
            // checkBoxDoorLockControl
            // 
            this.checkBoxDoorLockControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxDoorLockControl.AutoSize = true;
            this.checkBoxDoorLockControl.Location = new System.Drawing.Point(5, 7);
            this.checkBoxDoorLockControl.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.checkBoxDoorLockControl.Name = "checkBoxDoorLockControl";
            this.checkBoxDoorLockControl.Size = new System.Drawing.Size(99, 24);
            this.checkBoxDoorLockControl.TabIndex = 19;
            this.checkBoxDoorLockControl.Text = "Door lock detection";
            this.checkBoxDoorLockControl.UseVisualStyleBackColor = true;
            this.checkBoxDoorLockControl.CheckedChanged += new System.EventHandler(this.checkBoxDoorLockControl_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tableLayoutPanel2);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(557, 598);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox4.Size = new System.Drawing.Size(437, 174);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.btnTakeOff, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnAutoMark, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnSameTray, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDoeSlip, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.btnDifferentTray, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDryRun, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 20);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 27.27273F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.45454F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(437, 154);
            this.tableLayoutPanel2.TabIndex = 18;
            // 
            // btnTakeOff
            // 
            this.btnTakeOff.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTakeOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTakeOff.Location = new System.Drawing.Point(0, 84);
            this.btnTakeOff.Margin = new System.Windows.Forms.Padding(0);
            this.btnTakeOff.Name = "btnTakeOff";
            this.btnTakeOff.Size = new System.Drawing.Size(145, 70);
            this.btnTakeOff.TabIndex = 8;
            this.btnTakeOff.Text = "TakeOff";
            this.btnTakeOff.UseVisualStyleBackColor = true;
            this.btnTakeOff.Visible = false;
            this.btnTakeOff.Click += new System.EventHandler(this.btnTakeOff_Click);
            // 
            // btnAutoMark
            // 
            this.btnAutoMark.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAutoMark.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAutoMark.Location = new System.Drawing.Point(0, 0);
            this.btnAutoMark.Margin = new System.Windows.Forms.Padding(0);
            this.btnAutoMark.Name = "btnAutoMark";
            this.btnAutoMark.Size = new System.Drawing.Size(145, 42);
            this.btnAutoMark.TabIndex = 16;
            this.btnAutoMark.Text = "TrayMark";
            this.btnAutoMark.UseVisualStyleBackColor = true;
            this.btnAutoMark.Click += new System.EventHandler(this.btnAutoMark_Click_1);
            // 
            // btnSameTray
            // 
            this.btnSameTray.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSameTray.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSameTray.Location = new System.Drawing.Point(145, 0);
            this.btnSameTray.Margin = new System.Windows.Forms.Padding(0);
            this.btnSameTray.Name = "btnSameTray";
            this.btnSameTray.Size = new System.Drawing.Size(145, 42);
            this.btnSameTray.TabIndex = 6;
            this.btnSameTray.Text = "SameTray";
            this.btnSameTray.UseVisualStyleBackColor = true;
            this.btnSameTray.Click += new System.EventHandler(this.btnSameTray_Click);
            // 
            // btnDoeSlip
            // 
            this.btnDoeSlip.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDoeSlip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDoeSlip.Location = new System.Drawing.Point(145, 84);
            this.btnDoeSlip.Margin = new System.Windows.Forms.Padding(0);
            this.btnDoeSlip.Name = "btnDoeSlip";
            this.btnDoeSlip.Size = new System.Drawing.Size(145, 70);
            this.btnDoeSlip.TabIndex = 3;
            this.btnDoeSlip.Text = "Slip";
            this.btnDoeSlip.UseVisualStyleBackColor = true;
            this.btnDoeSlip.Visible = false;
            this.btnDoeSlip.Click += new System.EventHandler(this.btnDoeSlip_Click);
            // 
            // btnDifferentTray
            // 
            this.btnDifferentTray.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDifferentTray.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDifferentTray.Location = new System.Drawing.Point(290, 0);
            this.btnDifferentTray.Margin = new System.Windows.Forms.Padding(0);
            this.btnDifferentTray.Name = "btnDifferentTray";
            this.btnDifferentTray.Size = new System.Drawing.Size(147, 42);
            this.btnDifferentTray.TabIndex = 17;
            this.btnDifferentTray.Text = "DifferentTray";
            this.btnDifferentTray.UseVisualStyleBackColor = true;
            this.btnDifferentTray.Click += new System.EventHandler(this.btnDifferentTray_Click);
            // 
            // btnDryRun
            // 
            this.btnDryRun.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDryRun.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnDryRun.Location = new System.Drawing.Point(0, 42);
            this.btnDryRun.Margin = new System.Windows.Forms.Padding(0);
            this.btnDryRun.Name = "btnDryRun";
            this.btnDryRun.Size = new System.Drawing.Size(145, 42);
            this.btnDryRun.TabIndex = 11;
            this.btnDryRun.Text = "DryRun";
            this.btnDryRun.UseVisualStyleBackColor = true;
            this.btnDryRun.Click += new System.EventHandler(this.btnDryRun_Click);
            // 
            // btnClearSocketStatus
            // 
            this.btnClearSocketStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearSocketStatus.Location = new System.Drawing.Point(104, 38);
            this.btnClearSocketStatus.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearSocketStatus.Name = "btnClearSocketStatus";
            this.btnClearSocketStatus.Size = new System.Drawing.Size(108, 38);
            this.btnClearSocketStatus.TabIndex = 21;
            this.btnClearSocketStatus.Text = "Clear current socket status";
            this.btnClearSocketStatus.UseVisualStyleBackColor = true;
            this.btnClearSocketStatus.Click += new System.EventHandler(this.btnClearSocketStatus_Click);
            // 
            // btnClearHandlerProessData
            // 
            this.btnClearHandlerProessData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearHandlerProessData.Location = new System.Drawing.Point(104, 76);
            this.btnClearHandlerProessData.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearHandlerProessData.Name = "btnClearHandlerProessData";
            this.btnClearHandlerProessData.Size = new System.Drawing.Size(108, 38);
            this.btnClearHandlerProessData.TabIndex = 22;
            this.btnClearHandlerProessData.Text = "Clear Handler process data";
            this.btnClearHandlerProessData.UseVisualStyleBackColor = true;
            this.btnClearHandlerProessData.Click += new System.EventHandler(this.btnClearHandlerProessData_Click);
            // 
            // btnClearTesterProessData
            // 
            this.btnClearTesterProessData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearTesterProessData.Location = new System.Drawing.Point(212, 0);
            this.btnClearTesterProessData.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearTesterProessData.Name = "btnClearTesterProessData";
            this.btnClearTesterProessData.Size = new System.Drawing.Size(121, 38);
            this.btnClearTesterProessData.TabIndex = 23;
            this.btnClearTesterProessData.Text = "Clear Tester process data";
            this.btnClearTesterProessData.UseVisualStyleBackColor = true;
            this.btnClearTesterProessData.Click += new System.EventHandler(this.btnClearTesterProessData_Click);
            // 
            // btnVersion
            // 
            this.btnVersion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnVersion.Location = new System.Drawing.Point(212, 38);
            this.btnVersion.Margin = new System.Windows.Forms.Padding(0);
            this.btnVersion.Name = "btnVersion";
            this.btnVersion.Size = new System.Drawing.Size(121, 38);
            this.btnVersion.TabIndex = 24;
            this.btnVersion.Text = "View underlying version";
            this.btnVersion.UseVisualStyleBackColor = true;
            this.btnVersion.Click += new System.EventHandler(this.btnVersion_Click);
            // 
            // ucBinRegionSet1
            // 
            this.ucBinRegionSet1.Client = null;
            this.tableLayoutPanel1.SetColumnSpan(this.ucBinRegionSet1, 2);
            this.ucBinRegionSet1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucBinRegionSet1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucBinRegionSet1.Location = new System.Drawing.Point(0, 0);
            this.ucBinRegionSet1.Margin = new System.Windows.Forms.Padding(0);
            this.ucBinRegionSet1.Name = "ucBinRegionSet1";
            this.ucBinRegionSet1.NGCurrentRegion = null;
            this.ucBinRegionSet1.NGTrayID = 0;
            this.ucBinRegionSet1.OK1CurrentRegion = null;
            this.ucBinRegionSet1.OK1TrayID = 0;
            this.ucBinRegionSet1.OK2CurrentRegion = null;
            this.ucBinRegionSet1.OK2TrayID = 0;
            this.ucBinRegionSet1.Size = new System.Drawing.Size(994, 598);
            this.ucBinRegionSet1.TabIndex = 0;
            // 
            // UCHandlerConfig_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UCHandlerConfig_New";
            this.Size = new System.Drawing.Size(1268, 772);
            this.Load += new System.EventHandler(this.UCHandlerConfig_New_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CYGKit.Factory.OtherUI.Test.UCBinRegionSet ucBinRegionSet1;
        private System.Windows.Forms.CheckBox ckbxCloseBuzzer;
        private System.Windows.Forms.Label lbSocketID;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.CheckBox checkBoxDoorLockControl;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnDoeSlip;
        private System.Windows.Forms.Button btnSameTray;
        private System.Windows.Forms.Button btnDifferentTray;
        private System.Windows.Forms.Button btnDryRun;
        private System.Windows.Forms.Button btnAutoMark;
        private System.Windows.Forms.Button btnTakeOff;
        private System.Windows.Forms.Button btnClearOnlineDut;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnClearSocketStatus;
        private System.Windows.Forms.Button btnClearHandlerProessData;
        private System.Windows.Forms.Button btnClearTesterProessData;
        private System.Windows.Forms.Button btnVersion;
    }
}
