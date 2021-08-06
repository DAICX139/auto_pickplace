
namespace Poc2Auto.GUI
{
    partial class UCStations
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
            this.ucStation1 = new Poc2Auto.GUI.UCStation();
            this.ucStation2 = new Poc2Auto.GUI.UCStation();
            this.ucStation3 = new Poc2Auto.GUI.UCStation();
            this.ucStation4 = new Poc2Auto.GUI.UCStation();
            this.ucStation5 = new Poc2Auto.GUI.UCStation();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.uphLabel1 = new CYGKit.Factory.UPH.UPHLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.labelFail = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelYield = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxNoSn = new System.Windows.Forms.CheckBox();
            this.ckbxEnableMTCP = new System.Windows.Forms.CheckBox();
            this.ckbxWithTm = new System.Windows.Forms.CheckBox();
            this.checkBoxAllOk = new System.Windows.Forms.CheckBox();
            this.btnTMReset = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Control;
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tableLayoutPanel1.Controls.Add(this.ucStation1, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.ucStation2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ucStation3, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.ucStation4, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ucStation5, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(608, 380);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // ucStation1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ucStation1, 2);
            this.ucStation1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucStation1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucStation1.Location = new System.Drawing.Point(204, 192);
            this.ucStation1.Margin = new System.Windows.Forms.Padding(2);
            this.ucStation1.Name = "ucStation1";
            this.ucStation1.Size = new System.Drawing.Size(198, 186);
            this.ucStation1.StationName = Poc2Auto.Common.StationName.Default;
            this.ucStation1.TabIndex = 0;
            // 
            // ucStation2
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ucStation2, 2);
            this.ucStation2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucStation2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucStation2.Location = new System.Drawing.Point(2, 192);
            this.ucStation2.Margin = new System.Windows.Forms.Padding(2);
            this.ucStation2.Name = "ucStation2";
            this.ucStation2.Size = new System.Drawing.Size(198, 186);
            this.ucStation2.StationName = Poc2Auto.Common.StationName.Test1_LIVW;
            this.ucStation2.TabIndex = 1;
            // 
            // ucStation3
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ucStation3, 2);
            this.ucStation3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucStation3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucStation3.Location = new System.Drawing.Point(406, 192);
            this.ucStation3.Margin = new System.Windows.Forms.Padding(2);
            this.ucStation3.Name = "ucStation3";
            this.ucStation3.Size = new System.Drawing.Size(200, 186);
            this.ucStation3.StationName = Poc2Auto.Common.StationName.Test4_BMPF;
            this.ucStation3.TabIndex = 2;
            // 
            // ucStation4
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ucStation4, 2);
            this.ucStation4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucStation4.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucStation4.Location = new System.Drawing.Point(2, 2);
            this.ucStation4.Margin = new System.Windows.Forms.Padding(2);
            this.ucStation4.Name = "ucStation4";
            this.ucStation4.Size = new System.Drawing.Size(198, 186);
            this.ucStation4.StationName = Poc2Auto.Common.StationName.Test2_DTGT;
            this.ucStation4.TabIndex = 3;
            // 
            // ucStation5
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ucStation5, 2);
            this.ucStation5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucStation5.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucStation5.Location = new System.Drawing.Point(204, 2);
            this.ucStation5.Margin = new System.Windows.Forms.Padding(2);
            this.ucStation5.Name = "ucStation5";
            this.ucStation5.Size = new System.Drawing.Size(198, 186);
            this.ucStation5.StationName = Poc2Auto.Common.StationName.Test3_Backup;
            this.ucStation5.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 2);
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.33823F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.66177F));
            this.tableLayoutPanel2.Controls.Add(this.uphLabel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(404, 0);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.86047F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 58.13953F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(204, 190);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // uphLabel1
            // 
            this.uphLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uphLabel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uphLabel1.HighColor = System.Drawing.Color.Lime;
            this.uphLabel1.Location = new System.Drawing.Point(0, 0);
            this.uphLabel1.LowColor = System.Drawing.Color.Red;
            this.uphLabel1.Margin = new System.Windows.Forms.Padding(0);
            this.uphLabel1.Name = "uphLabel1";
            this.uphLabel1.NormalColor = System.Drawing.Color.Aquamarine;
            this.uphLabel1.Size = new System.Drawing.Size(80, 79);
            this.uphLabel1.TabIndex = 0;
            this.uphLabel1.UpdateInterval = 1000;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tableLayoutPanel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(80, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(124, 79);
            this.panel2.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.labelFail, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.labelYield, 1, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(124, 79);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // labelFail
            // 
            this.labelFail.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelFail.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelFail.Location = new System.Drawing.Point(62, 46);
            this.labelFail.Margin = new System.Windows.Forms.Padding(0);
            this.labelFail.Name = "labelFail";
            this.labelFail.Size = new System.Drawing.Size(62, 25);
            this.labelFail.TabIndex = 4;
            this.labelFail.Text = "0.00%";
            this.labelFail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelFail.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(2, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "Fail:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(2, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Yield:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelYield
            // 
            this.labelYield.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelYield.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelYield.Location = new System.Drawing.Point(62, 7);
            this.labelYield.Margin = new System.Windows.Forms.Padding(0);
            this.labelYield.Name = "labelYield";
            this.labelYield.Size = new System.Drawing.Size(62, 25);
            this.labelYield.TabIndex = 2;
            this.labelYield.Text = "0.00%";
            this.labelYield.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel2.SetColumnSpan(this.tableLayoutPanel3, 2);
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.checkBoxNoSn, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.ckbxEnableMTCP, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.ckbxWithTm, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.checkBoxAllOk, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnTMReset, 0, 2);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 79);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 3;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(204, 111);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // checkBoxNoSn
            // 
            this.checkBoxNoSn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxNoSn.AutoSize = true;
            this.checkBoxNoSn.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxNoSn.Location = new System.Drawing.Point(105, 45);
            this.checkBoxNoSn.Name = "checkBoxNoSn";
            this.checkBoxNoSn.Size = new System.Drawing.Size(96, 21);
            this.checkBoxNoSn.TabIndex = 16;
            this.checkBoxNoSn.Text = "Enable Scan SN";
            this.checkBoxNoSn.UseVisualStyleBackColor = true;
            this.checkBoxNoSn.CheckedChanged += new System.EventHandler(this.checkBoxNoSn_CheckedChanged);
            // 
            // ckbxEnableMTCP
            // 
            this.ckbxEnableMTCP.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ckbxEnableMTCP.AutoSize = true;
            this.ckbxEnableMTCP.Location = new System.Drawing.Point(105, 8);
            this.ckbxEnableMTCP.Name = "ckbxEnableMTCP";
            this.ckbxEnableMTCP.Size = new System.Drawing.Size(96, 21);
            this.ckbxEnableMTCP.TabIndex = 15;
            this.ckbxEnableMTCP.Text = "Enable MTCP";
            this.ckbxEnableMTCP.UseVisualStyleBackColor = true;
            this.ckbxEnableMTCP.CheckedChanged += new System.EventHandler(this.ckbxEnableMTCP_CheckedChanged);
            // 
            // ckbxWithTm
            // 
            this.ckbxWithTm.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ckbxWithTm.AutoSize = true;
            this.ckbxWithTm.Location = new System.Drawing.Point(3, 45);
            this.ckbxWithTm.Name = "ckbxWithTm";
            this.ckbxWithTm.Size = new System.Drawing.Size(89, 21);
            this.ckbxWithTm.TabIndex = 14;
            this.ckbxWithTm.Text = "Enable TM";
            this.ckbxWithTm.UseVisualStyleBackColor = true;
            this.ckbxWithTm.CheckedChanged += new System.EventHandler(this.ckbxWithTm_CheckedChanged);
            // 
            // checkBoxAllOk
            // 
            this.checkBoxAllOk.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxAllOk.AutoSize = true;
            this.checkBoxAllOk.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxAllOk.Location = new System.Drawing.Point(3, 8);
            this.checkBoxAllOk.Name = "checkBoxAllOk";
            this.checkBoxAllOk.Size = new System.Drawing.Size(82, 21);
            this.checkBoxAllOk.TabIndex = 10;
            this.checkBoxAllOk.Text = "All bin ok";
            this.checkBoxAllOk.UseVisualStyleBackColor = true;
            this.checkBoxAllOk.CheckedChanged += new System.EventHandler(this.checkBoxAllOk_CheckedChanged);
            // 
            // btnTMReset
            // 
            this.btnTMReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTMReset.Location = new System.Drawing.Point(0, 74);
            this.btnTMReset.Margin = new System.Windows.Forms.Padding(0);
            this.btnTMReset.Name = "btnTMReset";
            this.btnTMReset.Size = new System.Drawing.Size(102, 37);
            this.btnTMReset.TabIndex = 17;
            this.btnTMReset.Text = "TM Reset";
            this.btnTMReset.UseVisualStyleBackColor = true;
            this.btnTMReset.Click += new System.EventHandler(this.btnTMReset_Click);
            // 
            // UCStations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCStations";
            this.Size = new System.Drawing.Size(608, 380);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UCStation ucStation1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private UCStation ucStation2;
        private UCStation ucStation3;
        private UCStation ucStation4;
        private UCStation ucStation5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private CYGKit.Factory.UPH.UPHLabel uphLabel1;
        private System.Windows.Forms.CheckBox checkBoxAllOk;
        private System.Windows.Forms.CheckBox ckbxWithTm;
        private System.Windows.Forms.CheckBox ckbxEnableMTCP;
        private System.Windows.Forms.CheckBox checkBoxNoSn;
        private System.Windows.Forms.Label labelYield;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelFail;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button btnTMReset;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    }
}
