
namespace Poc2Auto.GUI
{
    partial class UCGeneralConfig
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.ckbxWithTm = new System.Windows.Forms.CheckBox();
            this.ckbxCloseBuzzer = new System.Windows.Forms.CheckBox();
            this.ckbxDutJudge = new System.Windows.Forms.CheckBox();
            this.ckbxEnableMTCP = new System.Windows.Forms.CheckBox();
            this.btnSocketOpen = new System.Windows.Forms.Button();
            this.checkBoxAllOk = new System.Windows.Forms.CheckBox();
            this.checkBoxNoSn = new System.Windows.Forms.CheckBox();
            this.tabPageTrayData = new System.Windows.Forms.TabPage();
            this.tabPageBinRegion = new System.Windows.Forms.TabPage();
            this.tabPageSocketSet = new System.Windows.Forms.TabPage();
            this.ucSocket1 = new Poc2Auto.GUI.UCSocket();
            this.tabControl1.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageSocketSet.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageTrayData);
            this.tabControl1.Controls.Add(this.tabPageBinRegion);
            this.tabControl1.Controls.Add(this.tabPageSocketSet);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(793, 599);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.ckbxWithTm);
            this.tabPageGeneral.Controls.Add(this.ckbxCloseBuzzer);
            this.tabPageGeneral.Controls.Add(this.ckbxDutJudge);
            this.tabPageGeneral.Controls.Add(this.ckbxEnableMTCP);
            this.tabPageGeneral.Controls.Add(this.btnSocketOpen);
            this.tabPageGeneral.Controls.Add(this.checkBoxAllOk);
            this.tabPageGeneral.Controls.Add(this.checkBoxNoSn);
            this.tabPageGeneral.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 29);
            this.tabPageGeneral.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Size = new System.Drawing.Size(785, 566);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // ckbxWithTm
            // 
            this.ckbxWithTm.AutoSize = true;
            this.ckbxWithTm.Location = new System.Drawing.Point(27, 141);
            this.ckbxWithTm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckbxWithTm.Name = "ckbxWithTm";
            this.ckbxWithTm.Size = new System.Drawing.Size(107, 24);
            this.ckbxWithTm.TabIndex = 7;
            this.ckbxWithTm.Text = "Enable TM";
            this.ckbxWithTm.UseVisualStyleBackColor = true;
            this.ckbxWithTm.CheckedChanged += new System.EventHandler(this.ckbxWithTm_CheckedChanged);
            // 
            // ckbxCloseBuzzer
            // 
            this.ckbxCloseBuzzer.AutoSize = true;
            this.ckbxCloseBuzzer.Location = new System.Drawing.Point(27, 89);
            this.ckbxCloseBuzzer.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckbxCloseBuzzer.Name = "ckbxCloseBuzzer";
            this.ckbxCloseBuzzer.Size = new System.Drawing.Size(168, 24);
            this.ckbxCloseBuzzer.TabIndex = 6;
            this.ckbxCloseBuzzer.Text = "Turn off the buzzer";
            this.ckbxCloseBuzzer.UseVisualStyleBackColor = true;
            this.ckbxCloseBuzzer.CheckedChanged += new System.EventHandler(this.ckbxCloseBuzzer_CheckedChanged);
            // 
            // ckbxDutJudge
            // 
            this.ckbxDutJudge.AutoSize = true;
            this.ckbxDutJudge.Location = new System.Drawing.Point(225, 89);
            this.ckbxDutJudge.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckbxDutJudge.Name = "ckbxDutJudge";
            this.ckbxDutJudge.Size = new System.Drawing.Size(317, 24);
            this.ckbxDutJudge.TabIndex = 5;
            this.ckbxDutJudge.Text = "DUT identifies whether to stop after fail";
            this.ckbxDutJudge.UseVisualStyleBackColor = true;
            this.ckbxDutJudge.CheckedChanged += new System.EventHandler(this.ckbxDutJudge_CheckedChanged);
            // 
            // ckbxEnableMTCP
            // 
            this.ckbxEnableMTCP.AutoSize = true;
            this.ckbxEnableMTCP.Location = new System.Drawing.Point(389, 28);
            this.ckbxEnableMTCP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ckbxEnableMTCP.Name = "ckbxEnableMTCP";
            this.ckbxEnableMTCP.Size = new System.Drawing.Size(126, 24);
            this.ckbxEnableMTCP.TabIndex = 4;
            this.ckbxEnableMTCP.Text = "Enable MTCP";
            this.ckbxEnableMTCP.UseVisualStyleBackColor = true;
            this.ckbxEnableMTCP.CheckedChanged += new System.EventHandler(this.cbxEnableMTCP_CheckedChanged);
            // 
            // btnSocketOpen
            // 
            this.btnSocketOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSocketOpen.Location = new System.Drawing.Point(577, 18);
            this.btnSocketOpen.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSocketOpen.Name = "btnSocketOpen";
            this.btnSocketOpen.Size = new System.Drawing.Size(145, 44);
            this.btnSocketOpen.TabIndex = 3;
            this.btnSocketOpen.Text = "LoadOrUnload";
            this.btnSocketOpen.UseVisualStyleBackColor = true;
            this.btnSocketOpen.Click += new System.EventHandler(this.btnSocketOpen_Click);
            // 
            // checkBoxAllOk
            // 
            this.checkBoxAllOk.AutoSize = true;
            this.checkBoxAllOk.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxAllOk.Location = new System.Drawing.Point(225, 28);
            this.checkBoxAllOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxAllOk.Name = "checkBoxAllOk";
            this.checkBoxAllOk.Size = new System.Drawing.Size(99, 24);
            this.checkBoxAllOk.TabIndex = 1;
            this.checkBoxAllOk.Text = "All bin ok";
            this.checkBoxAllOk.UseVisualStyleBackColor = true;
            this.checkBoxAllOk.CheckedChanged += new System.EventHandler(this.CheckBoxAllOk_CheckedChanged);
            // 
            // checkBoxNoSn
            // 
            this.checkBoxNoSn.AutoSize = true;
            this.checkBoxNoSn.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxNoSn.Location = new System.Drawing.Point(27, 28);
            this.checkBoxNoSn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxNoSn.Name = "checkBoxNoSn";
            this.checkBoxNoSn.Size = new System.Drawing.Size(142, 24);
            this.checkBoxNoSn.TabIndex = 0;
            this.checkBoxNoSn.Text = "Enable Scan SN";
            this.checkBoxNoSn.UseVisualStyleBackColor = true;
            this.checkBoxNoSn.CheckedChanged += new System.EventHandler(this.CheckBoxNoSn_CheckedChanged);
            // 
            // tabPageTrayData
            // 
            this.tabPageTrayData.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPageTrayData.Location = new System.Drawing.Point(4, 29);
            this.tabPageTrayData.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageTrayData.Name = "tabPageTrayData";
            this.tabPageTrayData.Size = new System.Drawing.Size(785, 566);
            this.tabPageTrayData.TabIndex = 1;
            this.tabPageTrayData.Text = "Tray Data";
            this.tabPageTrayData.UseVisualStyleBackColor = true;
            // 
            // tabPageBinRegion
            // 
            this.tabPageBinRegion.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPageBinRegion.Location = new System.Drawing.Point(4, 29);
            this.tabPageBinRegion.Margin = new System.Windows.Forms.Padding(0);
            this.tabPageBinRegion.Name = "tabPageBinRegion";
            this.tabPageBinRegion.Size = new System.Drawing.Size(785, 566);
            this.tabPageBinRegion.TabIndex = 2;
            this.tabPageBinRegion.Text = "Bin Region";
            this.tabPageBinRegion.UseVisualStyleBackColor = true;
            // 
            // tabPageSocketSet
            // 
            this.tabPageSocketSet.Controls.Add(this.ucSocket1);
            this.tabPageSocketSet.Location = new System.Drawing.Point(4, 29);
            this.tabPageSocketSet.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageSocketSet.Name = "tabPageSocketSet";
            this.tabPageSocketSet.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageSocketSet.Size = new System.Drawing.Size(785, 566);
            this.tabPageSocketSet.TabIndex = 3;
            this.tabPageSocketSet.Text = "Socket set";
            this.tabPageSocketSet.UseVisualStyleBackColor = true;
            // 
            // ucSocket1
            // 
            this.ucSocket1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucSocket1.Location = new System.Drawing.Point(7, 8);
            this.ucSocket1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucSocket1.MaximumSize = new System.Drawing.Size(373, 222);
            this.ucSocket1.Name = "ucSocket1";
            this.ucSocket1.Size = new System.Drawing.Size(373, 222);
            this.ucSocket1.TabIndex = 0;
            // 
            // UCGeneralConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UCGeneralConfig";
            this.Size = new System.Drawing.Size(793, 599);
            this.Load += new System.EventHandler(this.UCGeneralConfig_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabPageSocketSet.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.CheckBox checkBoxAllOk;
        private System.Windows.Forms.CheckBox checkBoxNoSn;
        public System.Windows.Forms.TabPage tabPageTrayData;
        public System.Windows.Forms.TabPage tabPageBinRegion;
        private System.Windows.Forms.Button btnSocketOpen;
        private System.Windows.Forms.TabPage tabPageSocketSet;
        private UCSocket ucSocket1;
        public System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.CheckBox ckbxEnableMTCP;
        private System.Windows.Forms.CheckBox ckbxDutJudge;
        private System.Windows.Forms.CheckBox ckbxCloseBuzzer;
        private System.Windows.Forms.CheckBox ckbxWithTm;
    }
}
