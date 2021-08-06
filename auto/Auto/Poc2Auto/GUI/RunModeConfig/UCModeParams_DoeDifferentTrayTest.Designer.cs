
namespace Poc2Auto.GUI.RunModeConfig
{
    partial class UCModeParams_DoeDifferentTrayTest
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.uC_GridviewTray1 = new CYGKit.Factory.OtherUI.UC_GridviewTray();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.uC_GridviewTray1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.35156F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(365, 640);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // uC_GridviewTray1
            // 
            this.uC_GridviewTray1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_GridviewTray1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_GridviewTray1.Location = new System.Drawing.Point(0, 0);
            this.uC_GridviewTray1.Margin = new System.Windows.Forms.Padding(0);
            this.uC_GridviewTray1.Name = "uC_GridviewTray1";
            this.uC_GridviewTray1.ShowCoordinate = null;
            this.uC_GridviewTray1.Size = new System.Drawing.Size(365, 640);
            this.uC_GridviewTray1.TabIndex = 0;
            this.uC_GridviewTray1.Title = "Load Tray";
            this.uC_GridviewTray1.TrayCol = 14;
            this.uC_GridviewTray1.TrayRow = 32;
            // 
            // UCModeParams_DoeDifferentTrayTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UCModeParams_DoeDifferentTrayTest";
            this.Size = new System.Drawing.Size(365, 640);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CYGKit.Factory.OtherUI.UC_GridviewTray uC_GridviewTray1;
    }
}
