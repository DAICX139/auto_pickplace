namespace DragonFlex.GUI.Factory
{
    partial class UCModeUI
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
            this.ucModeManual1 = new DragonFlex.GUI.Factory.UCModeManual();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 501F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ucModeManual1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(726, 481);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // ucModeManual1
            // 
            this.ucModeManual1.AXIS_COUNT = 0;
            this.ucModeManual1.CYL_COUNT = 0;
            this.ucModeManual1.DefaultRecipe = null;
            this.ucModeManual1.DioPath = null;
            this.ucModeManual1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucModeManual1.Location = new System.Drawing.Point(501, 0);
            this.ucModeManual1.Margin = new System.Windows.Forms.Padding(0);
            this.ucModeManual1.Name = "ucModeManual1";
            this.ucModeManual1.RecipePath = null;
            this.ucModeManual1.SemiAutoPath = null;
            this.ucModeManual1.Size = new System.Drawing.Size(225, 481);
            this.ucModeManual1.TabIndex = 0;
            // 
            // UCModeUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UCModeUI";
            this.Size = new System.Drawing.Size(726, 481);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        public UCModeManual ucModeManual1;
    }
}
