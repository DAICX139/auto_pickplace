
namespace Poc2Auto.GUI.RunModeConfig
{
    partial class UCModeParams_SocketTest
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.numTestCol = new System.Windows.Forms.NumericUpDown();
            this.numTestRow = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numTestTimes = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.cbxTrayID = new System.Windows.Forms.ComboBox();
            this.cbxSocketID = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTestCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTestRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTestTimes)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.55634F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.98528F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.55808F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.90029F));
            this.tableLayoutPanel1.Controls.Add(this.comboBox1, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.numTestCol, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.numTestRow, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.numTestTimes, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.cbxTrayID, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbxSocketID, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.80952F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.80952F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.80952F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(519, 178);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(370, 77);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(146, 28);
            this.comboBox1.TabIndex = 17;
            // 
            // numTestCol
            // 
            this.numTestCol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numTestCol.Location = new System.Drawing.Point(114, 134);
            this.numTestCol.Maximum = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.numTestCol.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTestCol.Name = "numTestCol";
            this.numTestCol.Size = new System.Drawing.Size(118, 27);
            this.numTestCol.TabIndex = 11;
            this.numTestCol.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numTestRow
            // 
            this.numTestRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numTestRow.Location = new System.Drawing.Point(114, 75);
            this.numTestRow.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numTestRow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTestRow.Name = "numTestRow";
            this.numTestRow.Size = new System.Drawing.Size(118, 27);
            this.numTestRow.TabIndex = 9;
            this.numTestRow.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tray ID:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Test Row:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Test Col:";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(266, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "Test Socket:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(264, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 20);
            this.label6.TabIndex = 5;
            this.label6.Text = "Test Station:";
            // 
            // numTestTimes
            // 
            this.numTestTimes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numTestTimes.Location = new System.Drawing.Point(370, 134);
            this.numTestTimes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTestTimes.Name = "numTestTimes";
            this.numTestTimes.Size = new System.Drawing.Size(146, 27);
            this.numTestTimes.TabIndex = 12;
            this.numTestTimes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(273, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 20);
            this.label7.TabIndex = 6;
            this.label7.Text = "Test Times:";
            // 
            // cbxTrayID
            // 
            this.cbxTrayID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxTrayID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTrayID.FormattingEnabled = true;
            this.cbxTrayID.Location = new System.Drawing.Point(114, 18);
            this.cbxTrayID.Name = "cbxTrayID";
            this.cbxTrayID.Size = new System.Drawing.Size(118, 28);
            this.cbxTrayID.TabIndex = 14;
            // 
            // cbxSocketID
            // 
            this.cbxSocketID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxSocketID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSocketID.FormattingEnabled = true;
            this.cbxSocketID.Location = new System.Drawing.Point(370, 15);
            this.cbxSocketID.Name = "cbxSocketID";
            this.cbxSocketID.Size = new System.Drawing.Size(146, 28);
            this.cbxSocketID.TabIndex = 16;
            // 
            // UCModeParams_SocketTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UCModeParams_SocketTest";
            this.Size = new System.Drawing.Size(519, 178);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTestCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTestRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTestTimes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numTestCol;
        private System.Windows.Forms.NumericUpDown numTestRow;
        private System.Windows.Forms.ComboBox cbxTrayID;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.NumericUpDown numTestTimes;
        private System.Windows.Forms.ComboBox cbxSocketID;
    }
}
