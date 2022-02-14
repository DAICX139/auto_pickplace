
namespace Poc2Auto.GUI
{
    partial class UCStationDutTestResult
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.清除数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ColDutSn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSocketSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLIVW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColNFBP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColKYRL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBMPF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColDutSn,
            this.ColSocketSN,
            this.ColLIVW,
            this.ColNFBP,
            this.ColKYRL,
            this.ColBMPF,
            this.ColBin});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 30;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(656, 420);
            this.dataGridView1.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.清除数据ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(139, 28);
            // 
            // 清除数据ToolStripMenuItem
            // 
            this.清除数据ToolStripMenuItem.Name = "清除数据ToolStripMenuItem";
            this.清除数据ToolStripMenuItem.Size = new System.Drawing.Size(138, 24);
            this.清除数据ToolStripMenuItem.Text = "清除数据";
            this.清除数据ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // ColDutSn
            // 
            this.ColDutSn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColDutSn.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColDutSn.HeaderText = "Dut SN";
            this.ColDutSn.MinimumWidth = 8;
            this.ColDutSn.Name = "ColDutSn";
            this.ColDutSn.ReadOnly = true;
            this.ColDutSn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColDutSn.Width = 160;
            // 
            // ColSocketSN
            // 
            this.ColSocketSN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColSocketSN.HeaderText = "Socket";
            this.ColSocketSN.MinimumWidth = 8;
            this.ColSocketSN.Name = "ColSocketSN";
            this.ColSocketSN.ReadOnly = true;
            this.ColSocketSN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColSocketSN.Width = 70;
            // 
            // ColLIVW
            // 
            this.ColLIVW.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColLIVW.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColLIVW.FillWeight = 99.75063F;
            this.ColLIVW.HeaderText = "LIVW";
            this.ColLIVW.MinimumWidth = 8;
            this.ColLIVW.Name = "ColLIVW";
            this.ColLIVW.ReadOnly = true;
            this.ColLIVW.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColLIVW.Width = 70;
            // 
            // ColNFBP
            // 
            this.ColNFBP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColNFBP.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColNFBP.HeaderText = "NFBP";
            this.ColNFBP.MinimumWidth = 8;
            this.ColNFBP.Name = "ColNFBP";
            this.ColNFBP.ReadOnly = true;
            this.ColNFBP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColNFBP.Width = 70;
            // 
            // ColKYRL
            // 
            this.ColKYRL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColKYRL.HeaderText = "KYRL";
            this.ColKYRL.MinimumWidth = 6;
            this.ColKYRL.Name = "ColKYRL";
            this.ColKYRL.ReadOnly = true;
            this.ColKYRL.Width = 70;
            // 
            // ColBMPF
            // 
            this.ColBMPF.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColBMPF.FillWeight = 100.0623F;
            this.ColBMPF.HeaderText = "BMPF";
            this.ColBMPF.MinimumWidth = 8;
            this.ColBMPF.Name = "ColBMPF";
            this.ColBMPF.ReadOnly = true;
            this.ColBMPF.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColBMPF.Width = 70;
            // 
            // ColBin
            // 
            this.ColBin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColBin.HeaderText = "Bin ";
            this.ColBin.MinimumWidth = 6;
            this.ColBin.Name = "ColBin";
            this.ColBin.ReadOnly = true;
            this.ColBin.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColBin.Width = 70;
            // 
            // UCStationDutTestResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCStationDutTestResult";
            this.Size = new System.Drawing.Size(656, 420);
            this.Load += new System.EventHandler(this.UC_DutResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 清除数据ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDutSn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSocketSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLIVW;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNFBP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColKYRL;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBMPF;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBin;
    }
}
