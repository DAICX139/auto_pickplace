namespace Poc2Auto.GUI
{
    partial class UCStatistics
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.uC_LotBinStatDB1 = new CYGKit.Factory.Statistics.UC_LotBinStatDB();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.uC_StationLotBinStat1 = new CYGKit.Factory.Statistics.UC_StationLotBinStatDB();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSocketId = new System.Windows.Forms.ComboBox();
            this.uC_SocketStat1 = new CYGKit.Factory.Statistics.UC_SocketStat();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(509, 522);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.uC_LotBinStatDB1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(501, 489);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "LotBin";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // uC_LotBinStatDB1
            // 
            this.uC_LotBinStatDB1.CurrentLotId = null;
            this.uC_LotBinStatDB1.DataSource = null;
            this.uC_LotBinStatDB1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_LotBinStatDB1.Location = new System.Drawing.Point(0, 0);
            this.uC_LotBinStatDB1.Margin = new System.Windows.Forms.Padding(0);
            this.uC_LotBinStatDB1.Name = "uC_LotBinStatDB1";
            this.uC_LotBinStatDB1.Size = new System.Drawing.Size(501, 489);
            this.uC_LotBinStatDB1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.uC_StationLotBinStat1);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(501, 489);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "StationBin";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // uC_StationLotBinStat1
            // 
            this.uC_StationLotBinStat1.CurrentLotId = null;
            this.uC_StationLotBinStat1.DataSource = null;
            this.uC_StationLotBinStat1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_StationLotBinStat1.Location = new System.Drawing.Point(0, 0);
            this.uC_StationLotBinStat1.Margin = new System.Windows.Forms.Padding(0);
            this.uC_StationLotBinStat1.Name = "uC_StationLotBinStat1";
            this.uC_StationLotBinStat1.Size = new System.Drawing.Size(501, 489);
            this.uC_StationLotBinStat1.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.tableLayoutPanel4);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(501, 489);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "SocketGroup";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.cmbSocketId, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.uC_SocketStat1, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(501, 489);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 7);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(242, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Socket Id";
            // 
            // cmbSocketId
            // 
            this.cmbSocketId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbSocketId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSocketId.FormattingEnabled = true;
            this.cmbSocketId.Location = new System.Drawing.Point(250, 0);
            this.cmbSocketId.Margin = new System.Windows.Forms.Padding(0);
            this.cmbSocketId.Name = "cmbSocketId";
            this.cmbSocketId.Size = new System.Drawing.Size(251, 28);
            this.cmbSocketId.TabIndex = 1;
            this.cmbSocketId.SelectedIndexChanged += new System.EventHandler(this.cmbSocketId_SelectedIndexChanged);
            // 
            // uC_SocketStat1
            // 
            this.tableLayoutPanel4.SetColumnSpan(this.uC_SocketStat1, 2);
            this.uC_SocketStat1.DataSource = null;
            this.uC_SocketStat1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_SocketStat1.Location = new System.Drawing.Point(0, 34);
            this.uC_SocketStat1.Margin = new System.Windows.Forms.Padding(0);
            this.uC_SocketStat1.Name = "uC_SocketStat1";
            this.uC_SocketStat1.Size = new System.Drawing.Size(501, 455);
            this.uC_SocketStat1.TabIndex = 2;
            // 
            // UCStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UCStatistics";
            this.Size = new System.Drawing.Size(509, 522);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbSocketId;
        private CYGKit.Factory.Statistics.UC_LotBinStatDB uC_LotBinStatDB1;
        private CYGKit.Factory.Statistics.UC_StationLotBinStatDB uC_StationLotBinStat1;
        private CYGKit.Factory.Statistics.UC_SocketStat uC_SocketStat1;
    }
}
