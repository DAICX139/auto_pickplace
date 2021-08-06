
namespace Poc2Auto.GUI
{
    partial class UCSocketDisableList
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
            this.lbxDisableList = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // lbxDisableList
            // 
            this.lbxDisableList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbxDisableList.FormattingEnabled = true;
            this.lbxDisableList.IntegralHeight = false;
            this.lbxDisableList.ItemHeight = 17;
            this.lbxDisableList.Location = new System.Drawing.Point(0, 0);
            this.lbxDisableList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lbxDisableList.Name = "lbxDisableList";
            this.lbxDisableList.Size = new System.Drawing.Size(128, 98);
            this.lbxDisableList.TabIndex = 0;
            // 
            // UCSocketDisableList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbxDisableList);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UCSocketDisableList";
            this.Size = new System.Drawing.Size(128, 98);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbxDisableList;
    }
}
