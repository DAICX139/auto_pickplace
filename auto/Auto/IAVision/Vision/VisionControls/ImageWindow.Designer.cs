namespace VisionControls
{
    partial class ImageWindow
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
            this.hWindowControl = new HalconDotNet.HWindowControl();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.适合大小FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示十字CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示原图OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存图像SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存带图形图形GToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示网格GToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblImageInfo = new System.Windows.Forms.Label();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // hWindowControl
            // 
            this.hWindowControl.BackColor = System.Drawing.Color.Black;
            this.hWindowControl.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl.ContextMenuStrip = this.contextMenuStrip;
            this.hWindowControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl.Name = "hWindowControl";
            this.hWindowControl.Size = new System.Drawing.Size(640, 480);
            this.hWindowControl.TabIndex = 0;
            this.hWindowControl.WindowSize = new System.Drawing.Size(640, 480);
            this.hWindowControl.HMouseMove += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseMove);
            this.hWindowControl.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseDown);
            this.hWindowControl.HMouseUp += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseUp);
            this.hWindowControl.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.hWindowControl_HMouseWheel);
            this.hWindowControl.SizeChanged += new System.EventHandler(this.hWindowControl_SizeChanged);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.适合大小FToolStripMenuItem,
            this.显示十字CToolStripMenuItem,
            this.显示原图OToolStripMenuItem,
            this.保存图像SToolStripMenuItem,
            this.保存带图形图形GToolStripMenuItem,
            this.显示网格GToolStripMenuItem});
            this.contextMenuStrip.Name = "cmsMain";
            this.contextMenuStrip.Size = new System.Drawing.Size(161, 136);
            this.contextMenuStrip.MouseEnter += new System.EventHandler(this.contextMenuStrip_MouseEnter);
            this.contextMenuStrip.MouseLeave += new System.EventHandler(this.contextMenuStrip_MouseLeave);
            // 
            // 适合大小FToolStripMenuItem
            // 
            this.适合大小FToolStripMenuItem.Name = "适合大小FToolStripMenuItem";
            this.适合大小FToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.适合大小FToolStripMenuItem.Text = "适合大小";
            this.适合大小FToolStripMenuItem.Click += new System.EventHandler(this.适合大小FToolStripMenuItem_Click);
            // 
            // 显示十字CToolStripMenuItem
            // 
            this.显示十字CToolStripMenuItem.CheckOnClick = true;
            this.显示十字CToolStripMenuItem.Name = "显示十字CToolStripMenuItem";
            this.显示十字CToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.显示十字CToolStripMenuItem.Text = "显示十字";
            this.显示十字CToolStripMenuItem.Click += new System.EventHandler(this.显示十字CToolStripMenuItem_Click);
            // 
            // 显示原图OToolStripMenuItem
            // 
            this.显示原图OToolStripMenuItem.Name = "显示原图OToolStripMenuItem";
            this.显示原图OToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.显示原图OToolStripMenuItem.Text = "显示原图";
            this.显示原图OToolStripMenuItem.Click += new System.EventHandler(this.显示原图OToolStripMenuItem_Click);
            // 
            // 保存图像SToolStripMenuItem
            // 
            this.保存图像SToolStripMenuItem.Name = "保存图像SToolStripMenuItem";
            this.保存图像SToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.保存图像SToolStripMenuItem.Text = "保存图像";
            this.保存图像SToolStripMenuItem.Click += new System.EventHandler(this.保存图像SToolStripMenuItem_Click);
            // 
            // 保存带图形图形GToolStripMenuItem
            // 
            this.保存带图形图形GToolStripMenuItem.Name = "保存带图形图形GToolStripMenuItem";
            this.保存带图形图形GToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.保存带图形图形GToolStripMenuItem.Text = "保存带图形图形";
            this.保存带图形图形GToolStripMenuItem.Click += new System.EventHandler(this.保存带图形图形GToolStripMenuItem_Click);
            // 
            // 显示网格GToolStripMenuItem
            // 
            this.显示网格GToolStripMenuItem.Name = "显示网格GToolStripMenuItem";
            this.显示网格GToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.显示网格GToolStripMenuItem.Text = "显示网格";
            this.显示网格GToolStripMenuItem.Click += new System.EventHandler(this.显示网格GToolStripMenuItem_Click);
            // 
            // lblImageInfo
            // 
            this.lblImageInfo.AutoSize = true;
            this.lblImageInfo.BackColor = System.Drawing.Color.White;
            this.lblImageInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblImageInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblImageInfo.ForeColor = System.Drawing.SystemColors.InfoText;
            this.lblImageInfo.Location = new System.Drawing.Point(0, 468);
            this.lblImageInfo.Margin = new System.Windows.Forms.Padding(3);
            this.lblImageInfo.Name = "lblImageInfo";
            this.lblImageInfo.Size = new System.Drawing.Size(89, 12);
            this.lblImageInfo.TabIndex = 2;
            this.lblImageInfo.Text = "no input image";
            // 
            // ImageWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblImageInfo);
            this.Controls.Add(this.hWindowControl);
            this.Name = "ImageWindow";
            this.Size = new System.Drawing.Size(640, 480);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 适合大小FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示十字CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示原图OToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存图像SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示网格GToolStripMenuItem;
        private System.Windows.Forms.Label lblImageInfo;
        private System.Windows.Forms.ToolStripMenuItem 保存带图形图形GToolStripMenuItem;
    }
}
