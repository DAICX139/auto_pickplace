
namespace Poc2Auto.GUI
{
    partial class UCStationDutTestResult
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColDutSn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDTGT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLIVW = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBMPF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColDutSn,
            this.ColDTGT,
            this.ColLIVW,
            this.ColBMPF});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 30;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(492, 336);
            this.dataGridView1.TabIndex = 0;
            // 
            // ColDutSn
            // 
            this.ColDutSn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColDutSn.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColDutSn.HeaderText = "DutSn";
            this.ColDutSn.MinimumWidth = 8;
            this.ColDutSn.Name = "ColDutSn";
            this.ColDutSn.ReadOnly = true;
            this.ColDutSn.Width = 157;
            // 
            // ColDTGT
            // 
            this.ColDTGT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColDTGT.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColDTGT.HeaderText = "DTGT";
            this.ColDTGT.MinimumWidth = 8;
            this.ColDTGT.Name = "ColDTGT";
            this.ColDTGT.Width = 70;
            // 
            // ColLIVW
            // 
            this.ColLIVW.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.ColLIVW.DefaultCellStyle = dataGridViewCellStyle3;
            this.ColLIVW.FillWeight = 99.75063F;
            this.ColLIVW.HeaderText = "LIVW";
            this.ColLIVW.MinimumWidth = 8;
            this.ColLIVW.Name = "ColLIVW";
            this.ColLIVW.Width = 70;
            // 
            // ColBMPF
            // 
            this.ColBMPF.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColBMPF.FillWeight = 100.0623F;
            this.ColBMPF.HeaderText = "BMPF";
            this.ColBMPF.MinimumWidth = 8;
            this.ColBMPF.Name = "ColBMPF";
            this.ColBMPF.Width = 70;
            // 
            // UCStationDutTestResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UCStationDutTestResult";
            this.Size = new System.Drawing.Size(492, 336);
            this.Load += new System.EventHandler(this.UC_DutResult_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDutSn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDTGT;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLIVW;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBMPF;
    }
}
