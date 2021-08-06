
namespace Poc2Auto.GUI
{
    partial class UCTMConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCTMConfig));
            this.uC_Rules1 = new CYGKit.Factory.ConditionEditor.UC_Rules();
            this.SuspendLayout();
            // 
            // uC_Rules1
            // 
            this.uC_Rules1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_Rules1.FilePath = "";
            this.uC_Rules1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uC_Rules1.Location = new System.Drawing.Point(0, 0);
            this.uC_Rules1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.uC_Rules1.Name = "uC_Rules1";
            this.uC_Rules1.Root = ((CYGKit.Factory.ConditionEditor.Root)(resources.GetObject("uC_Rules1.Root")));
            this.uC_Rules1.Size = new System.Drawing.Size(456, 453);
            this.uC_Rules1.TabIndex = 0;
            // 
            // UCTMConfig
            // 
            this.Controls.Add(this.uC_Rules1);
            this.Name = "UCTMConfig";
            this.Size = new System.Drawing.Size(456, 453);
            this.ResumeLayout(false);

        }

        #endregion

        private CYGKit.Factory.ConditionEditor.UC_Rules uC_Rules1;
    }
}
