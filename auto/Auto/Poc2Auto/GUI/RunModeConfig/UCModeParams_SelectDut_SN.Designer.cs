
namespace Poc2Auto.GUI.RunModeConfig
{
    partial class UCModeParams_SelectDut_SN
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
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.buttonAddSnList = new System.Windows.Forms.Button();
            this.menuSelectSN = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveSelectSn = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSelectSn = new System.Windows.Forms.ToolStripMenuItem();
            this.clearSelectSn = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.listBoxSNs = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ckbxOnlyScan = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnImport = new System.Windows.Forms.Button();
            this.ckbxAllSelect = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.saveFilePath = new System.Windows.Forms.SaveFileDialog();
            this.menuSelectSN.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxInput
            // 
            this.textBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxInput.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxInput.Location = new System.Drawing.Point(3, 5);
            this.textBoxInput.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(246, 27);
            this.textBoxInput.TabIndex = 6;
            // 
            // buttonAddSnList
            // 
            this.buttonAddSnList.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonAddSnList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddSnList.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonAddSnList.Location = new System.Drawing.Point(251, 0);
            this.buttonAddSnList.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.buttonAddSnList.Name = "buttonAddSnList";
            this.buttonAddSnList.Size = new System.Drawing.Size(111, 37);
            this.buttonAddSnList.TabIndex = 4;
            this.buttonAddSnList.Text = "Add";
            this.buttonAddSnList.UseVisualStyleBackColor = true;
            this.buttonAddSnList.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // menuSelectSN
            // 
            this.menuSelectSN.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuSelectSN.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveSelectSn,
            this.deleteSelectSn,
            this.clearSelectSn});
            this.menuSelectSN.Name = "menuSelectSN";
            this.menuSelectSN.Size = new System.Drawing.Size(127, 76);
            // 
            // saveSelectSn
            // 
            this.saveSelectSn.Name = "saveSelectSn";
            this.saveSelectSn.Size = new System.Drawing.Size(126, 24);
            this.saveSelectSn.Text = "Save";
            this.saveSelectSn.Click += new System.EventHandler(this.saveSelectSn_Click);
            // 
            // deleteSelectSn
            // 
            this.deleteSelectSn.Name = "deleteSelectSn";
            this.deleteSelectSn.Size = new System.Drawing.Size(126, 24);
            this.deleteSelectSn.Text = "Delete";
            this.deleteSelectSn.Click += new System.EventHandler(this.deleteSelectSn_Click);
            // 
            // clearSelectSn
            // 
            this.clearSelectSn.Name = "clearSelectSn";
            this.clearSelectSn.Size = new System.Drawing.Size(126, 24);
            this.clearSelectSn.Text = "Clear";
            this.clearSelectSn.Click += new System.EventHandler(this.clearSelectSn_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStrip,
            this.deleteToolStrip,
            this.clearToolStrip,
            this.importToolStripMenuItem});
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(129, 100);
            // 
            // saveToolStrip
            // 
            this.saveToolStrip.Name = "saveToolStrip";
            this.saveToolStrip.Size = new System.Drawing.Size(128, 24);
            this.saveToolStrip.Text = "Save";
            this.saveToolStrip.Click += new System.EventHandler(this.saveToolStrip_Click);
            // 
            // deleteToolStrip
            // 
            this.deleteToolStrip.Name = "deleteToolStrip";
            this.deleteToolStrip.Size = new System.Drawing.Size(128, 24);
            this.deleteToolStrip.Text = "Delete";
            this.deleteToolStrip.Click += new System.EventHandler(this.deleteToolStrip_Click);
            // 
            // clearToolStrip
            // 
            this.clearToolStrip.Name = "clearToolStrip";
            this.clearToolStrip.Size = new System.Drawing.Size(128, 24);
            this.clearToolStrip.Text = "Clear";
            this.clearToolStrip.Click += new System.EventHandler(this.clearToolStrip_Click);
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(128, 24);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.importToolStripMenuItem_Click);
            // 
            // openFile
            // 
            this.openFile.Filter = "CSV文件(*.csv)|*.csv";
            // 
            // saveFile
            // 
            this.saveFile.Filter = "CSV文件(*.csv)|*.csv";
            // 
            // listBoxSNs
            // 
            this.listBoxSNs.ContextMenuStrip = this.menuStrip;
            this.listBoxSNs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxSNs.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.listBoxSNs.FormattingEnabled = true;
            this.listBoxSNs.IntegralHeight = false;
            this.listBoxSNs.ItemHeight = 20;
            this.listBoxSNs.Location = new System.Drawing.Point(3, 79);
            this.listBoxSNs.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.listBoxSNs.Name = "listBoxSNs";
            this.tableLayoutPanel4.SetRowSpan(this.listBoxSNs, 5);
            this.listBoxSNs.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxSNs.Size = new System.Drawing.Size(358, 339);
            this.listBoxSNs.TabIndex = 1;
            this.listBoxSNs.SelectedIndexChanged += new System.EventHandler(this.ListBoxSNs_SelectedIndexChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel2, 0, 7);
            this.tableLayoutPanel4.Controls.Add(this.listBoxSNs, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 8;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.113057F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.336647F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.79879F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.500526F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.500526F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28.98853F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.823604F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.938322F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(364, 457);
            this.tableLayoutPanel4.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.74539F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.25461F));
            this.tableLayoutPanel2.Controls.Add(this.ckbxOnlyScan, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.button1, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 418);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(364, 39);
            this.tableLayoutPanel2.TabIndex = 14;
            // 
            // ckbxOnlyScan
            // 
            this.ckbxOnlyScan.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ckbxOnlyScan.AutoSize = true;
            this.ckbxOnlyScan.Location = new System.Drawing.Point(3, 7);
            this.ckbxOnlyScan.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.ckbxOnlyScan.Name = "ckbxOnlyScan";
            this.ckbxOnlyScan.Size = new System.Drawing.Size(102, 24);
            this.ckbxOnlyScan.TabIndex = 12;
            this.ckbxOnlyScan.Text = "Only Scan";
            this.ckbxOnlyScan.UseVisualStyleBackColor = true;
            this.ckbxOnlyScan.CheckedChanged += new System.EventHandler(this.ckbxOnlyScan_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Location = new System.Drawing.Point(144, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(217, 33);
            this.button1.TabIndex = 13;
            this.button1.Text = "Save path selection";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 42.26415F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.54717F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.56604F));
            this.tableLayoutPanel1.Controls.Add(this.btnImport, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.ckbxAllSelect, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 37);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(364, 42);
            this.tableLayoutPanel1.TabIndex = 13;
            // 
            // btnImport
            // 
            this.btnImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnImport.Location = new System.Drawing.Point(254, 0);
            this.btnImport.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(108, 42);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // ckbxAllSelect
            // 
            this.ckbxAllSelect.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ckbxAllSelect.AutoSize = true;
            this.ckbxAllSelect.Location = new System.Drawing.Point(153, 9);
            this.ckbxAllSelect.Margin = new System.Windows.Forms.Padding(0);
            this.ckbxAllSelect.Name = "ckbxAllSelect";
            this.ckbxAllSelect.Size = new System.Drawing.Size(96, 24);
            this.ckbxAllSelect.TabIndex = 1;
            this.ckbxAllSelect.Text = "Select all";
            this.ckbxAllSelect.UseVisualStyleBackColor = true;
            this.ckbxAllSelect.CheckedChanged += new System.EventHandler(this.ckbxAllSelect_CheckedChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Select DUT SN:";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.63469F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 31.36531F));
            this.tableLayoutPanel3.Controls.Add(this.textBoxInput, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.buttonAddSnList, 1, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(364, 37);
            this.tableLayoutPanel3.TabIndex = 15;
            // 
            // saveFilePath
            // 
            this.saveFilePath.DefaultExt = "csv";
            this.saveFilePath.Filter = "|*.csv";
            // 
            // UCModeParams_SelectDut_SN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel4);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCModeParams_SelectDut_SN";
            this.Size = new System.Drawing.Size(364, 457);
            this.menuSelectSN.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.ContextMenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem saveToolStrip;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.ToolStripMenuItem clearToolStrip;
        private System.Windows.Forms.ContextMenuStrip menuSelectSN;
        private System.Windows.Forms.ToolStripMenuItem saveSelectSn;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectSn;
        private System.Windows.Forms.ToolStripMenuItem clearSelectSn;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStrip;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.Button buttonAddSnList;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.ListBox listBoxSNs;
        private System.Windows.Forms.CheckBox ckbxOnlyScan;
        private System.Windows.Forms.SaveFileDialog saveFilePath;
        private System.Windows.Forms.CheckBox ckbxAllSelect;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    }
}
