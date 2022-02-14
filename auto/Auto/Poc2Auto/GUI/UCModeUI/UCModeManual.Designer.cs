namespace DragonFlex.GUI.Factory
{
    partial class UCModeManual
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
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ucRecipe_New1 = new Poc2Auto.GUI.UCRecipe_New();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.uC_Cylinders1 = new Poc2Auto.GUI.UCModeUI.UCAxisesCylinders.UC_Cylinders();
            this.btnPullPin = new System.Windows.Forms.Button();
            this.btnReleasePin = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.flowPanelAxis = new System.Windows.Forms.FlowLayoutPanel();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.uC_DIOs1 = new AlcUtility.PlcDriver.UC_DIOs();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.ucRecipe_New1);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage3.Size = new System.Drawing.Size(943, 443);
            this.tabPage3.TabIndex = 4;
            this.tabPage3.Text = "Recipe";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ucRecipe_New1
            // 
            this.ucRecipe_New1.DefaultRecipe = "";
            this.ucRecipe_New1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucRecipe_New1.FilePath = null;
            this.ucRecipe_New1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucRecipe_New1.Location = new System.Drawing.Point(4, 4);
            this.ucRecipe_New1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucRecipe_New1.Name = "ucRecipe_New1";
            this.ucRecipe_New1.PlcDriver = null;
            this.ucRecipe_New1.Size = new System.Drawing.Size(935, 435);
            this.ucRecipe_New1.TabIndex = 0;
            this.ucRecipe_New1.Title = "Recipe Config";
            // 
            // tabPage5
            // 
            this.tabPage5.AutoScroll = true;
            this.tabPage5.Controls.Add(this.tableLayoutPanel1);
            this.tabPage5.Location = new System.Drawing.Point(4, 29);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(943, 443);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "Cylinder";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.44926F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.798496F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.75224F));
            this.tableLayoutPanel1.Controls.Add(this.uC_Cylinders1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnPullPin, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnReleasePin, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.247711F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.247711F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.09293F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.41165F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(943, 443);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // uC_Cylinders1
            // 
            this.uC_Cylinders1.CYL_COUNT = 0;
            this.uC_Cylinders1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_Cylinders1.Location = new System.Drawing.Point(3, 2);
            this.uC_Cylinders1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uC_Cylinders1.Name = "uC_Cylinders1";
            this.uC_Cylinders1.PlcDriver = null;
            this.tableLayoutPanel1.SetRowSpan(this.uC_Cylinders1, 3);
            this.uC_Cylinders1.Size = new System.Drawing.Size(479, 290);
            this.uC_Cylinders1.TabIndex = 0;
            // 
            // btnPullPin
            // 
            this.btnPullPin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnPullPin.Location = new System.Drawing.Point(485, 0);
            this.btnPullPin.Margin = new System.Windows.Forms.Padding(0);
            this.btnPullPin.Name = "btnPullPin";
            this.btnPullPin.Size = new System.Drawing.Size(82, 32);
            this.btnPullPin.TabIndex = 1;
            this.btnPullPin.Text = "拉Pin针";
            this.btnPullPin.UseVisualStyleBackColor = true;
            this.btnPullPin.Visible = false;
            this.btnPullPin.Click += new System.EventHandler(this.btnPullPin_Click);
            // 
            // btnReleasePin
            // 
            this.btnReleasePin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReleasePin.Location = new System.Drawing.Point(485, 32);
            this.btnReleasePin.Margin = new System.Windows.Forms.Padding(0);
            this.btnReleasePin.Name = "btnReleasePin";
            this.btnReleasePin.Size = new System.Drawing.Size(82, 32);
            this.btnReleasePin.TabIndex = 2;
            this.btnReleasePin.Text = "放Pin针";
            this.btnReleasePin.UseVisualStyleBackColor = true;
            this.btnReleasePin.Visible = false;
            this.btnReleasePin.Click += new System.EventHandler(this.btnReleasePin_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.AutoScroll = true;
            this.tabPage4.Controls.Add(this.flowPanelAxis);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(943, 443);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Axis";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // flowPanelAxis
            // 
            this.flowPanelAxis.AutoScroll = true;
            this.flowPanelAxis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanelAxis.Location = new System.Drawing.Point(0, 0);
            this.flowPanelAxis.Margin = new System.Windows.Forms.Padding(4);
            this.flowPanelAxis.Name = "flowPanelAxis";
            this.flowPanelAxis.Size = new System.Drawing.Size(943, 443);
            this.flowPanelAxis.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.uC_DIOs1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(943, 443);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "DIO";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // uC_DIOs1
            // 
            this.uC_DIOs1.DataSource = null;
            this.uC_DIOs1.DIsPropName = null;
            this.uC_DIOs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uC_DIOs1.DOsPropName = null;
            this.uC_DIOs1.Interval = 200;
            this.uC_DIOs1.IsInnerUpdatingOpen = true;
            this.uC_DIOs1.Location = new System.Drawing.Point(2, 2);
            this.uC_DIOs1.Margin = new System.Windows.Forms.Padding(0);
            this.uC_DIOs1.Name = "uC_DIOs1";
            this.uC_DIOs1.PlcDiVarName = null;
            this.uC_DIOs1.PlcDoVarName = null;
            this.uC_DIOs1.PlcVarName = null;
            this.uC_DIOs1.Size = new System.Drawing.Size(939, 439);
            this.uC_DIOs1.TabIndex = 0;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage1);
            this.tabControl3.Controls.Add(this.tabPage4);
            this.tabControl3.Controls.Add(this.tabPage5);
            this.tabControl3.Controls.Add(this.tabPage3);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl3.Location = new System.Drawing.Point(0, 0);
            this.tabControl3.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(951, 476);
            this.tabControl3.TabIndex = 0;
            // 
            // UCModeManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tabControl3);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UCModeManual";
            this.Size = new System.Drawing.Size(951, 476);
            this.Load += new System.EventHandler(this.UCModeManual_Load);
            this.tabPage3.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabPage3;
        private Poc2Auto.GUI.UCRecipe_New ucRecipe_New1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Poc2Auto.GUI.UCModeUI.UCAxisesCylinders.UC_Cylinders uC_Cylinders1;
        private System.Windows.Forms.Button btnPullPin;
        private System.Windows.Forms.Button btnReleasePin;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.FlowLayoutPanel flowPanelAxis;
        private System.Windows.Forms.TabPage tabPage1;
        private AlcUtility.PlcDriver.UC_DIOs uC_DIOs1;
        private System.Windows.Forms.TabControl tabControl3;
    }
}
