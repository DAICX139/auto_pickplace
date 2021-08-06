
namespace Poc2Auto.GUI
{
    partial class UCErrorList
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
            this.lbxErrorList = new System.Windows.Forms.ListBox();
            this.MenuStripSaveMsg = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClearError = new System.Windows.Forms.Button();
            this.MenuStripSaveMsg.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbxErrorList
            // 
            this.lbxErrorList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbxErrorList.ContextMenuStrip = this.MenuStripSaveMsg;
            this.lbxErrorList.Font = new System.Drawing.Font("Microsoft YaHei UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbxErrorList.ForeColor = System.Drawing.Color.Red;
            this.lbxErrorList.FormattingEnabled = true;
            this.lbxErrorList.HorizontalScrollbar = true;
            this.lbxErrorList.ItemHeight = 30;
            this.lbxErrorList.Location = new System.Drawing.Point(0, 0);
            this.lbxErrorList.Margin = new System.Windows.Forms.Padding(0);
            this.lbxErrorList.Name = "lbxErrorList";
            this.lbxErrorList.Size = new System.Drawing.Size(263, 184);
            this.lbxErrorList.TabIndex = 0;
            // 
            // MenuStripSaveMsg
            // 
            this.MenuStripSaveMsg.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStripSaveMsg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem});
            this.MenuStripSaveMsg.Name = "MenuStripSaveMsg";
            this.MenuStripSaveMsg.Size = new System.Drawing.Size(113, 28);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(112, 24);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbxErrorList, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClearError, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(263, 222);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // btnClearError
            // 
            this.btnClearError.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearError.Location = new System.Drawing.Point(0, 192);
            this.btnClearError.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearError.Name = "btnClearError";
            this.btnClearError.Size = new System.Drawing.Size(263, 30);
            this.btnClearError.TabIndex = 1;
            this.btnClearError.Text = "Clear error";
            this.btnClearError.UseVisualStyleBackColor = true;
            this.btnClearError.Click += new System.EventHandler(this.btnClearError_Click);
            // 
            // UCErrorList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UCErrorList";
            this.Size = new System.Drawing.Size(263, 222);
            this.MenuStripSaveMsg.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbxErrorList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnClearError;
        private System.Windows.Forms.ContextMenuStrip MenuStripSaveMsg;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    }
}
