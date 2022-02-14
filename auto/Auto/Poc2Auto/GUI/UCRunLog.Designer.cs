
namespace Poc2Auto.GUI
{
    partial class UCRunLog
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
            this.menuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richboxLog = new System.Windows.Forms.RichTextBox();
            this.SelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.CopyText = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SelectAll,
            this.CopyText});
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(109, 52);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // richboxLog
            // 
            this.richboxLog.ContextMenuStrip = this.menuStrip;
            this.richboxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richboxLog.Location = new System.Drawing.Point(0, 0);
            this.richboxLog.Name = "richboxLog";
            this.richboxLog.Size = new System.Drawing.Size(372, 170);
            this.richboxLog.TabIndex = 2;
            this.richboxLog.Text = "";
            this.richboxLog.TextChanged += new System.EventHandler(this.richboxLog_TextChanged);
            // 
            // SelectAll
            // 
            this.SelectAll.Name = "SelectAll";
            this.SelectAll.Size = new System.Drawing.Size(210, 24);
            this.SelectAll.Text = "全选";
            this.SelectAll.Click += new System.EventHandler(this.SelectAll_Click);
            // 
            // CopyText
            // 
            this.CopyText.Name = "CopyText";
            this.CopyText.Size = new System.Drawing.Size(210, 24);
            this.CopyText.Text = "复制";
            this.CopyText.Click += new System.EventHandler(this.CopyText_Click);
            // 
            // UCRunLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.richboxLog);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCRunLog";
            this.Size = new System.Drawing.Size(372, 170);
            this.menuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richboxLog;
        private System.Windows.Forms.ToolStripMenuItem SelectAll;
        private System.Windows.Forms.ToolStripMenuItem CopyText;
    }
}
