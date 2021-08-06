
namespace Poc2Auto.GUI
{
    partial class UCStation
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
            this.uC_Station1 = new CYGKit.Factory.OtherUI.UC_Station();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.uC_StationStat1 = new CYGKit.Factory.Statistics.UC_StationStat();
            this.uC_OnlineDut1 = new CYGKit.Factory.TableView.UC_OnlineDut();
            this.uC_Station1.ContentPanel.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uC_Station1
            // 
            this.uC_Station1.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.uC_Station1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uC_Station1.ConnectState = false;
            // 
            // uC_Station1.ContentPanel
            // 
            this.uC_Station1.ContentPanel.Controls.Add(this.tableLayoutPanel1);
            this.uC_Station1.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_Station1.ContentPanel.Location = new System.Drawing.Point(3, 43);
            this.uC_Station1.ContentPanel.Name = "ContentPanel";
            this.uC_Station1.ContentPanel.Size = new System.Drawing.Size(233, 98);
            this.uC_Station1.ContentPanel.TabIndex = 0;
            this.uC_Station1.DataSource = null;
            this.uC_Station1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_Station1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_Station1.Location = new System.Drawing.Point(0, 0);
            this.uC_Station1.Margin = new System.Windows.Forms.Padding(0);
            this.uC_Station1.Name = "uC_Station1";
            this.uC_Station1.Row1Percent = 20;
            this.uC_Station1.Row2Percent = 20;
            this.uC_Station1.Size = new System.Drawing.Size(241, 146);
            this.uC_Station1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.uC_StationStat1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.uC_OnlineDut1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(233, 98);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // uC_StationStat1
            // 
            this.uC_StationStat1.DataSource = null;
            this.uC_StationStat1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_StationStat1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_StationStat1.HeadVisible = true;
            this.uC_StationStat1.Location = new System.Drawing.Point(0, 0);
            this.uC_StationStat1.Margin = new System.Windows.Forms.Padding(0);
            this.uC_StationStat1.Name = "uC_StationStat1";
            this.uC_StationStat1.Size = new System.Drawing.Size(233, 68);
            this.uC_StationStat1.TabIndex = 0;
            // 
            // uC_OnlineDut1
            // 
            this.uC_OnlineDut1.ArrayMember = null;
            this.uC_OnlineDut1.Col = 1;
            this.uC_OnlineDut1.DataSource = null;
            this.uC_OnlineDut1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_OnlineDut1.EnableDataUpdate = false;
            this.uC_OnlineDut1.FixSize = new System.Drawing.Size(0, 0);
            this.uC_OnlineDut1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_OnlineDut1.Location = new System.Drawing.Point(0, 68);
            this.uC_OnlineDut1.Margin = new System.Windows.Forms.Padding(0);
            this.uC_OnlineDut1.Name = "uC_OnlineDut1";
            this.uC_OnlineDut1.Row = 1;
            this.uC_OnlineDut1.Selectable = false;
            this.uC_OnlineDut1.SelectedRegion = new System.Drawing.Rectangle(-1, -1, -1, -1);
            this.uC_OnlineDut1.SelectionColor = System.Drawing.SystemColors.Highlight;
            this.uC_OnlineDut1.ShowColNumber = false;
            this.uC_OnlineDut1.ShowRowNumber = false;
            this.uC_OnlineDut1.ShowText = true;
            this.uC_OnlineDut1.ShowTitle = false;
            this.uC_OnlineDut1.Size = new System.Drawing.Size(233, 30);
            this.uC_OnlineDut1.SizeMode = CYGKit.GUI.SizeMode.None;
            this.uC_OnlineDut1.StationID = 0;
            this.uC_OnlineDut1.TabIndex = 1;
            this.uC_OnlineDut1.TextAlignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.uC_OnlineDut1.TextName = null;
            this.uC_OnlineDut1.Title = "title";
            this.uC_OnlineDut1.TitleAlignment = CYGKit.GUI.Alignment.Top;
            this.uC_OnlineDut1.ValueName = null;
            // 
            // UCStation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uC_Station1);
            this.Name = "UCStation";
            this.Size = new System.Drawing.Size(241, 146);
            this.Load += new System.EventHandler(this.UCStation_Load);
            this.uC_Station1.ContentPanel.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CYGKit.Factory.TableView.UC_OnlineDut uC_OnlineDut1;
        public CYGKit.Factory.OtherUI.UC_Station uC_Station1;
        public CYGKit.Factory.Statistics.UC_StationStat uC_StationStat1;
    }
}
