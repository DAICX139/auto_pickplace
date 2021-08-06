namespace Poc2Auto.GUI.UCModeUI.UCSemiAuto
{
    partial class UC_SemiAuto
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
            this.ContentPanel = new System.Windows.Forms.Panel();
            this.tPTrayPut = new System.Windows.Forms.TabPage();
            this.tPSocketTake = new System.Windows.Forms.TabPage();
            this.tPSocketPut = new System.Windows.Forms.TabPage();
            this.tPTrayTake = new System.Windows.Forms.TabPage();
            this.tPTrayCalib = new System.Windows.Forms.TabPage();
            this.ucSemiAutoCtrl1 = new AlcUtility.PlcDriver.SemiAutoUI.UCSemiAutoCtrl();
            this.ucPlcVarList1 = new Poc2Auto.GUI.UCModeUI.UCSemiAuto.UC_PlcVarList();
            this.ucSemiAutoCtrl1.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContentPanel
            // 
            this.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentPanel.Location = new System.Drawing.Point(3, 112);
            this.ContentPanel.Name = "ContentPanel";
            this.ContentPanel.Size = new System.Drawing.Size(573, 575);
            this.ContentPanel.TabIndex = 0;
            // 
            // tPTrayPut
            // 
            this.tPTrayPut.Location = new System.Drawing.Point(8, 45);
            this.tPTrayPut.Name = "tPTrayPut";
            this.tPTrayPut.Padding = new System.Windows.Forms.Padding(3);
            this.tPTrayPut.Size = new System.Drawing.Size(557, 522);
            this.tPTrayPut.TabIndex = 4;
            this.tPTrayPut.Text = "TrayPut";
            this.tPTrayPut.UseVisualStyleBackColor = true;
            // 
            // tPSocketTake
            // 
            this.tPSocketTake.Location = new System.Drawing.Point(8, 45);
            this.tPSocketTake.Name = "tPSocketTake";
            this.tPSocketTake.Padding = new System.Windows.Forms.Padding(3);
            this.tPSocketTake.Size = new System.Drawing.Size(557, 522);
            this.tPSocketTake.TabIndex = 3;
            this.tPSocketTake.Text = "SocketTake";
            this.tPSocketTake.UseVisualStyleBackColor = true;
            // 
            // tPSocketPut
            // 
            this.tPSocketPut.Location = new System.Drawing.Point(8, 45);
            this.tPSocketPut.Name = "tPSocketPut";
            this.tPSocketPut.Padding = new System.Windows.Forms.Padding(3);
            this.tPSocketPut.Size = new System.Drawing.Size(557, 522);
            this.tPSocketPut.TabIndex = 2;
            this.tPSocketPut.Text = "SocketPut";
            this.tPSocketPut.UseVisualStyleBackColor = true;
            // 
            // tPTrayTake
            // 
            this.tPTrayTake.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tPTrayTake.Location = new System.Drawing.Point(8, 45);
            this.tPTrayTake.Name = "tPTrayTake";
            this.tPTrayTake.Padding = new System.Windows.Forms.Padding(3);
            this.tPTrayTake.Size = new System.Drawing.Size(557, 522);
            this.tPTrayTake.TabIndex = 1;
            this.tPTrayTake.Text = "TrayTake";
            this.tPTrayTake.UseVisualStyleBackColor = true;
            // 
            // tPTrayCalib
            // 
            this.tPTrayCalib.Location = new System.Drawing.Point(8, 45);
            this.tPTrayCalib.Name = "tPTrayCalib";
            this.tPTrayCalib.Padding = new System.Windows.Forms.Padding(3);
            this.tPTrayCalib.Size = new System.Drawing.Size(557, 522);
            this.tPTrayCalib.TabIndex = 0;
            this.tPTrayCalib.Text = "TrayCalib";
            this.tPTrayCalib.UseVisualStyleBackColor = true;
            // 
            // ucSemiAutoCtrl1
            // 
            this.ucSemiAutoCtrl1.ActionName = "";
            // 
            // ucSemiAutoCtrl1.ContentPanel
            // 
            this.ucSemiAutoCtrl1.ContentPanel.Controls.Add(this.ucPlcVarList1);
            this.ucSemiAutoCtrl1.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSemiAutoCtrl1.ContentPanel.Location = new System.Drawing.Point(3, 112);
            this.ucSemiAutoCtrl1.ContentPanel.Name = "ContentPanel";
            this.ucSemiAutoCtrl1.ContentPanel.Size = new System.Drawing.Size(277, 192);
            this.ucSemiAutoCtrl1.ContentPanel.TabIndex = 0;
            this.ucSemiAutoCtrl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSemiAutoCtrl1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucSemiAutoCtrl1.Location = new System.Drawing.Point(0, 0);
            this.ucSemiAutoCtrl1.Margin = new System.Windows.Forms.Padding(2);
            this.ucSemiAutoCtrl1.Name = "ucSemiAutoCtrl1";
            this.ucSemiAutoCtrl1.ObjDataSrc = null;
            this.ucSemiAutoCtrl1.ParamDataSrc = null;
            this.ucSemiAutoCtrl1.PlcMode = AlcUtility.PlcMode.SemiAutoMode;
            this.ucSemiAutoCtrl1.PlcSubMode = 0;
            this.ucSemiAutoCtrl1.Size = new System.Drawing.Size(575, 412);
            this.ucSemiAutoCtrl1.TabIndex = 0;
            this.ucSemiAutoCtrl1.GetInputData += new System.Action<AlcUtility.ParamsModule>(this.ucSemiAutoCtrl1_GetInputData);
            // 
            // ucPlcVarList1
            // 
            this.ucPlcVarList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPlcVarList1.Location = new System.Drawing.Point(0, 0);
            this.ucPlcVarList1.Margin = new System.Windows.Forms.Padding(0);
            this.ucPlcVarList1.Name = "ucPlcVarList1";
            this.ucPlcVarList1.Size = new System.Drawing.Size(277, 192);
            this.ucPlcVarList1.TabIndex = 0;
            // 
            // UC_SemiAuto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ucSemiAutoCtrl1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "UC_SemiAuto";
            this.Size = new System.Drawing.Size(575, 412);
            this.ucSemiAutoCtrl1.ContentPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ContentPanel;
        private System.Windows.Forms.TabPage tPTrayPut;
        private System.Windows.Forms.TabPage tPSocketTake;
        private System.Windows.Forms.TabPage tPSocketPut;
        private System.Windows.Forms.TabPage tPTrayTake;
        private System.Windows.Forms.TabPage tPTrayCalib;
        public UC_PlcVarList ucPlcVarList1;
        public AlcUtility.PlcDriver.SemiAutoUI.UCSemiAutoCtrl ucSemiAutoCtrl1;
    }
}
