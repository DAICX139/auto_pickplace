
namespace Poc2Auto.GUI
{
    partial class UCHandlerConfig
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
            this.uC_BinRegion1 = new CYGKit.Factory.TableView.UC_BinRegion();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ucModeConfig1 = new Poc2Auto.GUI.RunModeConfig.UCModeConfig();
            this.ucGeneralConfig1 = new Poc2Auto.GUI.UCGeneralConfig();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uC_BinRegion1
            // 
            this.uC_BinRegion1.Client = null;
            this.uC_BinRegion1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_BinRegion1.Location = new System.Drawing.Point(601, 0);
            this.uC_BinRegion1.Margin = new System.Windows.Forms.Padding(0);
            this.uC_BinRegion1.Name = "uC_BinRegion1";
            this.uC_BinRegion1.Size = new System.Drawing.Size(421, 414);
            this.uC_BinRegion1.TabIndex = 2;
            this.uC_BinRegion1.Title = "Setting";
            this.uC_BinRegion1.TrayColumn = 14;
            this.uC_BinRegion1.TrayRow = 32;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.ucModeConfig1);
            this.flowLayoutPanel1.Controls.Add(this.ucGeneralConfig1);
            this.flowLayoutPanel1.Controls.Add(this.uC_BinRegion1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1030, 646);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // ucModeConfig1
            // 
            this.ucModeConfig1.AutoSize = true;
            this.ucModeConfig1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ucModeConfig1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucModeConfig1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucModeConfig1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucModeConfig1.Location = new System.Drawing.Point(3, 4);
            this.ucModeConfig1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucModeConfig1.Name = "ucModeConfig1";
            this.ucModeConfig1.Size = new System.Drawing.Size(595, 87);
            this.ucModeConfig1.TabIndex = 1;
            // 
            // ucGeneralConfig1
            // 
            this.ucGeneralConfig1.AllOk = false;
            this.ucGeneralConfig1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucGeneralConfig1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucGeneralConfig1.Location = new System.Drawing.Point(3, 99);
            this.ucGeneralConfig1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucGeneralConfig1.Name = "ucGeneralConfig1";
            this.ucGeneralConfig1.NoSn = false;
            this.ucGeneralConfig1.Size = new System.Drawing.Size(595, 479);
            this.ucGeneralConfig1.TabIndex = 4;
            // 
            // UCHandlerConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "UCHandlerConfig";
            this.Size = new System.Drawing.Size(1030, 646);
            this.Load += new System.EventHandler(this.HandlerConfig_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CYGKit.Factory.TableView.UC_BinRegion uC_BinRegion1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private RunModeConfig.UCModeConfig ucModeConfig1;
        private UCGeneralConfig ucGeneralConfig1;
    }
}
