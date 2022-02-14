
namespace CYGKit.Factory.OtherUI.Test
{
    partial class UCBinRegionSet
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColBin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gridView1 = new CYGKit.GUI.GridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.gridOK1Tray = new CYGKit.GUI.GridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.gridNGTray = new CYGKit.GUI.GridView();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.colorDialog2 = new System.Windows.Forms.ColorDialog();
            this.colorDialog3 = new System.Windows.Forms.ColorDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.2761F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.90796F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.90796F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.90796F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox4, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1209, 569);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(190, 563);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.88119F));
            this.tableLayoutPanel2.Controls.Add(this.dataGridView1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnDelete, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 64.05959F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35.94041F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(184, 537);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColBin,
            this.ColColor});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(178, 338);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // ColBin
            // 
            this.ColBin.HeaderText = "Bin ";
            this.ColBin.MinimumWidth = 6;
            this.ColBin.Name = "ColBin";
            this.ColBin.ReadOnly = true;
            this.ColBin.Width = 80;
            // 
            // ColColor
            // 
            this.ColColor.HeaderText = "Color";
            this.ColColor.MinimumWidth = 6;
            this.ColColor.Name = "ColColor";
            this.ColColor.ReadOnly = true;
            this.ColColor.Width = 80;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(3, 347);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(98, 44);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gridView1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(873, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(333, 563);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Pass2";
            // 
            // gridView1
            // 
            this.gridView1.ArrayMember = null;
            this.gridView1.Col = 14;
            this.gridView1.DataSource = null;
            this.gridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridView1.FixSize = new System.Drawing.Size(0, 0);
            this.gridView1.Font = new System.Drawing.Font("Microsoft YaHei UI", 6.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gridView1.Location = new System.Drawing.Point(3, 23);
            this.gridView1.Margin = new System.Windows.Forms.Padding(0);
            this.gridView1.Name = "gridView1";
            this.gridView1.Row = 32;
            this.gridView1.SelectedRegion = new System.Drawing.Rectangle(-1, -1, -1, -1);
            this.gridView1.SelectionColor = System.Drawing.SystemColors.Highlight;
            this.gridView1.ShowColNumber = true;
            this.gridView1.ShowRowNumber = true;
            this.gridView1.ShowTitle = false;
            this.gridView1.Size = new System.Drawing.Size(327, 537);
            this.gridView1.SizeMode = CYGKit.GUI.SizeMode.None;
            this.gridView1.TabIndex = 0;
            this.gridView1.TextAlignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.gridView1.TextName = null;
            this.gridView1.Title = "title";
            this.gridView1.TitleAlignment = CYGKit.GUI.Alignment.Top;
            this.gridView1.ValueName = null;
            this.gridView1.SelectionChanged += new System.Action(this.gridOK2Tray_SelectionChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.gridOK1Tray);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(536, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(331, 563);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Pass1";
            // 
            // gridOK1Tray
            // 
            this.gridOK1Tray.ArrayMember = null;
            this.gridOK1Tray.Col = 14;
            this.gridOK1Tray.DataSource = null;
            this.gridOK1Tray.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridOK1Tray.FixSize = new System.Drawing.Size(0, 0);
            this.gridOK1Tray.Font = new System.Drawing.Font("Microsoft YaHei UI", 6.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gridOK1Tray.Location = new System.Drawing.Point(3, 23);
            this.gridOK1Tray.Margin = new System.Windows.Forms.Padding(0);
            this.gridOK1Tray.Name = "gridOK1Tray";
            this.gridOK1Tray.Row = 32;
            this.gridOK1Tray.SelectedRegion = new System.Drawing.Rectangle(-1, -1, -1, -1);
            this.gridOK1Tray.SelectionColor = System.Drawing.SystemColors.Highlight;
            this.gridOK1Tray.ShowColNumber = true;
            this.gridOK1Tray.ShowRowNumber = true;
            this.gridOK1Tray.ShowTitle = false;
            this.gridOK1Tray.Size = new System.Drawing.Size(325, 537);
            this.gridOK1Tray.SizeMode = CYGKit.GUI.SizeMode.None;
            this.gridOK1Tray.TabIndex = 0;
            this.gridOK1Tray.TextAlignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.gridOK1Tray.TextName = null;
            this.gridOK1Tray.Title = "title";
            this.gridOK1Tray.TitleAlignment = CYGKit.GUI.Alignment.Top;
            this.gridOK1Tray.ValueName = null;
            this.gridOK1Tray.SelectionChanged += new System.Action(this.gridOK1Tray_SelectionChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.gridNGTray);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(199, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(331, 563);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "NG";
            // 
            // gridNGTray
            // 
            this.gridNGTray.ArrayMember = null;
            this.gridNGTray.Col = 14;
            this.gridNGTray.DataSource = null;
            this.gridNGTray.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridNGTray.FixSize = new System.Drawing.Size(0, 0);
            this.gridNGTray.Font = new System.Drawing.Font("Microsoft YaHei UI", 6.6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gridNGTray.Location = new System.Drawing.Point(3, 23);
            this.gridNGTray.Margin = new System.Windows.Forms.Padding(0);
            this.gridNGTray.Name = "gridNGTray";
            this.gridNGTray.Row = 32;
            this.gridNGTray.SelectedRegion = new System.Drawing.Rectangle(-1, -1, -1, -1);
            this.gridNGTray.SelectionColor = System.Drawing.SystemColors.Highlight;
            this.gridNGTray.ShowColNumber = true;
            this.gridNGTray.ShowRowNumber = true;
            this.gridNGTray.ShowTitle = false;
            this.gridNGTray.Size = new System.Drawing.Size(325, 537);
            this.gridNGTray.SizeMode = CYGKit.GUI.SizeMode.None;
            this.gridNGTray.TabIndex = 1;
            this.gridNGTray.TextAlignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.gridNGTray.TextName = null;
            this.gridNGTray.Title = "title";
            this.gridNGTray.TitleAlignment = CYGKit.GUI.Alignment.Top;
            this.gridNGTray.ValueName = null;
            this.gridNGTray.SelectionChanged += new System.Action(this.gridNGTray_SelectionChanged);
            // 
            // UCBinRegionSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UCBinRegionSet";
            this.Size = new System.Drawing.Size(1209, 569);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ColorDialog colorDialog2;
        private System.Windows.Forms.ColorDialog colorDialog3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox4;
        private GUI.GridView gridOK1Tray;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBin;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColColor;
        private GUI.GridView gridView1;
        private GUI.GridView gridNGTray;
    }
}
