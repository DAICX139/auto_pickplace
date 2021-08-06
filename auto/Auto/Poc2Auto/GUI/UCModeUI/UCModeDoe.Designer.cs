namespace DragonFlex.GUI.Factory
{
    partial class UCModeDoe
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
            this.ucSemiAutoAction1 = new AlcUtility.PlcDriver.SemiAutoUI.UCCommonAction();
            this.SuspendLayout();
            // 
            // ucSemiAutoAction1
            // 
            this.ucSemiAutoAction1.ActionName = "GoldenDutTest";
            // 
            // ucSemiAutoAction1.ContentPanel
            // 
            this.ucSemiAutoAction1.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSemiAutoAction1.ContentPanel.Location = new System.Drawing.Point(5, 5);
            this.ucSemiAutoAction1.ContentPanel.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ucSemiAutoAction1.ContentPanel.Name = "ContentPanel";
            this.ucSemiAutoAction1.ContentPanel.Size = new System.Drawing.Size(222, 1);
            this.ucSemiAutoAction1.ContentPanel.TabIndex = 0;
            this.ucSemiAutoAction1.Location = new System.Drawing.Point(32, 27);
            this.ucSemiAutoAction1.Margin = new System.Windows.Forms.Padding(4);
            this.ucSemiAutoAction1.Name = "ucSemiAutoAction1";
            this.ucSemiAutoAction1.PlcMode = AlcUtility.PlcMode.DoeMode;
            this.ucSemiAutoAction1.PlcSubMode = 0;
            this.ucSemiAutoAction1.Size = new System.Drawing.Size(232, 59);
            this.ucSemiAutoAction1.TabIndex = 1;
            // 
            // UCModeDoe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucSemiAutoAction1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UCModeDoe";
            this.Size = new System.Drawing.Size(607, 381);
            this.ResumeLayout(false);

        }

        #endregion

        private AlcUtility.PlcDriver.SemiAutoUI.UCCommonAction ucSemiAutoAction1;
    }
}
