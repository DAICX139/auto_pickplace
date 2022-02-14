
namespace Poc2Auto.GUI.UCModeUI.UCAxisesCylinders
{
    partial class UC_SingleAxis_New
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
            this.ckbxEnable = new System.Windows.Forms.CheckBox();
            this.labActPos = new System.Windows.Forms.Label();
            this.numericMovePos = new System.Windows.Forms.NumericUpDown();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.btnMoveTo = new System.Windows.Forms.Button();
            this.btnJogAdd = new System.Windows.Forms.Button();
            this.labAxisName = new System.Windows.Forms.Label();
            this.btnJogSub = new System.Windows.Forms.Button();
            this.numericVelocity = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericMovePos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericVelocity)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // ckbxEnable
            // 
            this.ckbxEnable.AutoSize = true;
            this.ckbxEnable.Location = new System.Drawing.Point(275, 4);
            this.ckbxEnable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ckbxEnable.Name = "ckbxEnable";
            this.ckbxEnable.Size = new System.Drawing.Size(76, 24);
            this.ckbxEnable.TabIndex = 7;
            this.ckbxEnable.Text = "轴使能";
            this.ckbxEnable.UseVisualStyleBackColor = true;
            this.ckbxEnable.Click += new System.EventHandler(this.ckbxEnable_Click);
            // 
            // labActPos
            // 
            this.labActPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labActPos.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel3.SetColumnSpan(this.labActPos, 2);
            this.labActPos.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labActPos.ForeColor = System.Drawing.Color.Lime;
            this.labActPos.Location = new System.Drawing.Point(3, 2);
            this.labActPos.Name = "labActPos";
            this.labActPos.Size = new System.Drawing.Size(91, 28);
            this.labActPos.TabIndex = 0;
            this.labActPos.Text = "0.00";
            this.labActPos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // numericMovePos
            // 
            this.numericMovePos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericMovePos.DecimalPlaces = 3;
            this.numericMovePos.Location = new System.Drawing.Point(84, 3);
            this.numericMovePos.Maximum = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.numericMovePos.Name = "numericMovePos";
            this.numericMovePos.Size = new System.Drawing.Size(87, 27);
            this.numericMovePos.TabIndex = 10;
            // 
            // btnHome
            // 
            this.btnHome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHome.Location = new System.Drawing.Point(363, 0);
            this.btnHome.Margin = new System.Windows.Forms.Padding(0);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(82, 32);
            this.btnHome.TabIndex = 9;
            this.btnHome.Text = "回原点";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnReset
            // 
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReset.Location = new System.Drawing.Point(201, 0);
            this.btnReset.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(66, 32);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "复位";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnStop
            // 
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStop.Location = new System.Drawing.Point(365, 0);
            this.btnStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(80, 32);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(304, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "mm/s";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label2
            // 
            this.Label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(181, 6);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(39, 20);
            this.Label2.TabIndex = 5;
            this.Label2.Text = "速度";
            this.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnMoveTo
            // 
            this.btnMoveTo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMoveTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMoveTo.Location = new System.Drawing.Point(0, 0);
            this.btnMoveTo.Margin = new System.Windows.Forms.Padding(0);
            this.btnMoveTo.Name = "btnMoveTo";
            this.btnMoveTo.Size = new System.Drawing.Size(81, 32);
            this.btnMoveTo.TabIndex = 2;
            this.btnMoveTo.Text = "移动到";
            this.btnMoveTo.UseVisualStyleBackColor = true;
            this.btnMoveTo.Click += new System.EventHandler(this.btnMoveTo_Click);
            // 
            // btnJogAdd
            // 
            this.btnJogAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnJogAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnJogAdd.Location = new System.Drawing.Point(97, 0);
            this.btnJogAdd.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.btnJogAdd.Name = "btnJogAdd";
            this.btnJogAdd.Size = new System.Drawing.Size(44, 32);
            this.btnJogAdd.TabIndex = 1;
            this.btnJogAdd.Text = "+";
            this.btnJogAdd.UseVisualStyleBackColor = true;
            this.btnJogAdd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogAdd_MouseDown);
            this.btnJogAdd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogAdd_MouseUp);
            // 
            // labAxisName
            // 
            this.labAxisName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labAxisName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labAxisName.Location = new System.Drawing.Point(3, 5);
            this.labAxisName.Name = "labAxisName";
            this.labAxisName.Size = new System.Drawing.Size(24, 28);
            this.labAxisName.TabIndex = 0;
            this.labAxisName.Text = "X";
            this.labAxisName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnJogSub
            // 
            this.btnJogSub.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnJogSub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnJogSub.Location = new System.Drawing.Point(151, 0);
            this.btnJogSub.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.btnJogSub.Name = "btnJogSub";
            this.btnJogSub.Size = new System.Drawing.Size(45, 32);
            this.btnJogSub.TabIndex = 3;
            this.btnJogSub.Text = "-";
            this.btnJogSub.UseVisualStyleBackColor = true;
            this.btnJogSub.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogSub_MouseDown);
            this.btnJogSub.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogSub_MouseUp);
            // 
            // numericVelocity
            // 
            this.numericVelocity.Location = new System.Drawing.Point(226, 4);
            this.numericVelocity.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numericVelocity.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericVelocity.Name = "numericVelocity";
            this.numericVelocity.Size = new System.Drawing.Size(67, 27);
            this.numericVelocity.TabIndex = 1;
            this.numericVelocity.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 6;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.31285F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.90802F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.18336F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.74751F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.10076F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.74751F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.btnHome, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.Label2, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.numericMovePos, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnMoveTo, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.numericVelocity, 3, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(33, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(445, 32);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 7;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.03531F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.095313F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.23596F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.46067F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.30337F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.06537F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.43341F));
            this.tableLayoutPanel3.Controls.Add(this.labActPos, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnStop, 6, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnJogAdd, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnJogSub, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnReset, 4, 0);
            this.tableLayoutPanel3.Controls.Add(this.ckbxEnable, 5, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(33, 41);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(445, 32);
            this.tableLayoutPanel3.TabIndex = 13;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.labAxisName, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(481, 76);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // UC_SingleAxis_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UC_SingleAxis_New";
            this.Size = new System.Drawing.Size(481, 76);
            ((System.ComponentModel.ISupportInitialize)(this.numericMovePos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericVelocity)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label labActPos;
        private System.Windows.Forms.CheckBox ckbxEnable;
        private System.Windows.Forms.NumericUpDown numericVelocity;
        private System.Windows.Forms.Button btnJogSub;
        private System.Windows.Forms.Label labAxisName;
        private System.Windows.Forms.Button btnJogAdd;
        private System.Windows.Forms.Button btnMoveTo;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.NumericUpDown numericMovePos;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
