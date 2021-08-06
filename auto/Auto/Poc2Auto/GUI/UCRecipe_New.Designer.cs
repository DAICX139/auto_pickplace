
namespace Poc2Auto.GUI
{
    partial class UCRecipe_New
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
            this.grpbxTitle = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ColIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColControl = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cbxRecipeName = new System.Windows.Forms.ComboBox();
            this.dataGridParamModule = new System.Windows.Forms.DataGridView();
            this.ColModuleIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColModuleName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.grpbxTitle.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridParamModule)).BeginInit();
            this.SuspendLayout();
            // 
            // grpbxTitle
            // 
            this.grpbxTitle.Controls.Add(this.tableLayoutPanel1);
            this.grpbxTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpbxTitle.Location = new System.Drawing.Point(0, 0);
            this.grpbxTitle.Margin = new System.Windows.Forms.Padding(0);
            this.grpbxTitle.Name = "grpbxTitle";
            this.grpbxTitle.Size = new System.Drawing.Size(710, 691);
            this.grpbxTitle.TabIndex = 0;
            this.grpbxTitle.TabStop = false;
            this.grpbxTitle.Text = "groupBox1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnDelete, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.cbxRecipeName, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.dataGridParamModule, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 23);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.714286F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.28571F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(704, 665);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColIndex,
            this.ColKey,
            this.ColValue,
            this.ColDescription,
            this.ColControl});
            this.tableLayoutPanel1.SetColumnSpan(this.dataGridView1, 4);
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(200, 38);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(504, 627);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // ColIndex
            // 
            this.ColIndex.FillWeight = 101.5228F;
            this.ColIndex.HeaderText = "Index";
            this.ColIndex.MinimumWidth = 20;
            this.ColIndex.Name = "ColIndex";
            this.ColIndex.ReadOnly = true;
            // 
            // ColKey
            // 
            this.ColKey.FillWeight = 99.49239F;
            this.ColKey.HeaderText = "Key";
            this.ColKey.MinimumWidth = 6;
            this.ColKey.Name = "ColKey";
            this.ColKey.ReadOnly = true;
            // 
            // ColValue
            // 
            this.ColValue.FillWeight = 99.49239F;
            this.ColValue.HeaderText = "Value";
            this.ColValue.MinimumWidth = 30;
            this.ColValue.Name = "ColValue";
            // 
            // ColDescription
            // 
            this.ColDescription.FillWeight = 99.49239F;
            this.ColDescription.HeaderText = "Description";
            this.ColDescription.MinimumWidth = 6;
            this.ColDescription.Name = "ColDescription";
            // 
            // ColControl
            // 
            this.ColControl.HeaderText = "Control";
            this.ColControl.MinimumWidth = 20;
            this.ColControl.Name = "ColControl";
            this.ColControl.Text = "获取位置";
            this.ColControl.UseColumnTextForButtonValue = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.AutoSize = true;
            this.btnDelete.Location = new System.Drawing.Point(634, 0);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(70, 38);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.AutoSize = true;
            this.btnSave.Location = new System.Drawing.Point(556, 0);
            this.btnSave.Margin = new System.Windows.Forms.Padding(0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(78, 38);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbxRecipeName
            // 
            this.cbxRecipeName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxRecipeName.FormattingEnabled = true;
            this.cbxRecipeName.Location = new System.Drawing.Point(270, 7);
            this.cbxRecipeName.Margin = new System.Windows.Forms.Padding(0);
            this.cbxRecipeName.Name = "cbxRecipeName";
            this.cbxRecipeName.Size = new System.Drawing.Size(286, 28);
            this.cbxRecipeName.TabIndex = 4;
            this.cbxRecipeName.SelectedIndexChanged += new System.EventHandler(this.cbxRecipeName_SelectedIndexChanged);
            this.cbxRecipeName.TextChanged += new System.EventHandler(this.cbxRecipeName_TextChanged);
            // 
            // dataGridParamModule
            // 
            this.dataGridParamModule.AllowUserToAddRows = false;
            this.dataGridParamModule.AllowUserToDeleteRows = false;
            this.dataGridParamModule.AllowUserToResizeRows = false;
            this.dataGridParamModule.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridParamModule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridParamModule.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColModuleIndex,
            this.ColModuleName});
            this.dataGridParamModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridParamModule.Location = new System.Drawing.Point(0, 38);
            this.dataGridParamModule.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridParamModule.Name = "dataGridParamModule";
            this.dataGridParamModule.RowHeadersVisible = false;
            this.dataGridParamModule.RowHeadersWidth = 51;
            this.dataGridParamModule.RowTemplate.Height = 27;
            this.dataGridParamModule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridParamModule.Size = new System.Drawing.Size(200, 627);
            this.dataGridParamModule.TabIndex = 0;
            this.dataGridParamModule.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridParamModule_CellClick);
            // 
            // ColModuleIndex
            // 
            this.ColModuleIndex.HeaderText = "Index";
            this.ColModuleIndex.MinimumWidth = 20;
            this.ColModuleIndex.Name = "ColModuleIndex";
            this.ColModuleIndex.ReadOnly = true;
            // 
            // ColModuleName
            // 
            this.ColModuleName.HeaderText = "Module Name";
            this.ColModuleName.MinimumWidth = 40;
            this.ColModuleName.Name = "ColModuleName";
            this.ColModuleName.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Recipe Module:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(203, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Recipe:";
            // 
            // UCRecipe_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpbxTitle);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UCRecipe_New";
            this.Size = new System.Drawing.Size(710, 691);
            this.grpbxTitle.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridParamModule)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpbxTitle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dataGridParamModule;
        private System.Windows.Forms.ComboBox cbxRecipeName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColModuleIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColModuleName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDescription;
        private System.Windows.Forms.DataGridViewButtonColumn ColControl;
    }
}
