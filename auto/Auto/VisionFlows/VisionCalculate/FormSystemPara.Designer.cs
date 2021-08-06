
namespace VisionFlows.VisionCalculate
{
    partial class FormSystemPara
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PNTitle = new System.Windows.Forms.Panel();
            this.LBTitleTxt = new System.Windows.Forms.Label();
            this.PNDataGrid = new System.Windows.Forms.Panel();
            this.dataGridViewSystemPara = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PNBtn = new System.Windows.Forms.Panel();
            this.BtnSaveAll = new System.Windows.Forms.Button();
            this.PNTitle.SuspendLayout();
            this.PNDataGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSystemPara)).BeginInit();
            this.PNBtn.SuspendLayout();
            this.SuspendLayout();
            // 
            // PNTitle
            // 
            this.PNTitle.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PNTitle.Controls.Add(this.LBTitleTxt);
            this.PNTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.PNTitle.Location = new System.Drawing.Point(0, 0);
            this.PNTitle.Name = "PNTitle";
            this.PNTitle.Size = new System.Drawing.Size(811, 47);
            this.PNTitle.TabIndex = 0;
            // 
            // LBTitleTxt
            // 
            this.LBTitleTxt.AutoSize = true;
            this.LBTitleTxt.Font = new System.Drawing.Font("Impact", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBTitleTxt.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LBTitleTxt.Location = new System.Drawing.Point(124, 2);
            this.LBTitleTxt.Name = "LBTitleTxt";
            this.LBTitleTxt.Size = new System.Drawing.Size(279, 43);
            this.LBTitleTxt.TabIndex = 0;
            this.LBTitleTxt.Text = "SystemParameter";
            // 
            // PNDataGrid
            // 
            this.PNDataGrid.Controls.Add(this.dataGridViewSystemPara);
            this.PNDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PNDataGrid.Location = new System.Drawing.Point(0, 47);
            this.PNDataGrid.Name = "PNDataGrid";
            this.PNDataGrid.Size = new System.Drawing.Size(811, 465);
            this.PNDataGrid.TabIndex = 1;
            // 
            // dataGridViewSystemPara
            // 
            this.dataGridViewSystemPara.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSystemPara.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dataGridViewSystemPara.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSystemPara.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSystemPara.MultiSelect = false;
            this.dataGridViewSystemPara.Name = "dataGridViewSystemPara";
            this.dataGridViewSystemPara.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dataGridViewSystemPara.RowHeadersVisible = false;
            this.dataGridViewSystemPara.RowTemplate.Height = 23;
            this.dataGridViewSystemPara.Size = new System.Drawing.Size(811, 465);
            this.dataGridViewSystemPara.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "参数名";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 300;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column2.HeaderText = "Value X";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Value Y";
            this.Column3.Name = "Column3";
            this.Column3.Width = 240;
            // 
            // PNBtn
            // 
            this.PNBtn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.PNBtn.Controls.Add(this.BtnSaveAll);
            this.PNBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PNBtn.Location = new System.Drawing.Point(0, 512);
            this.PNBtn.Name = "PNBtn";
            this.PNBtn.Size = new System.Drawing.Size(811, 42);
            this.PNBtn.TabIndex = 2;
            // 
            // BtnSaveAll
            // 
            this.BtnSaveAll.Location = new System.Drawing.Point(678, 6);
            this.BtnSaveAll.Name = "BtnSaveAll";
            this.BtnSaveAll.Size = new System.Drawing.Size(121, 28);
            this.BtnSaveAll.TabIndex = 0;
            this.BtnSaveAll.Text = "保存全部";
            this.BtnSaveAll.UseVisualStyleBackColor = true;
            this.BtnSaveAll.Click += new System.EventHandler(this.BtnSaveAll_Click);
            // 
            // FormSystemPara
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 554);
            this.Controls.Add(this.PNDataGrid);
            this.Controls.Add(this.PNTitle);
            this.Controls.Add(this.PNBtn);
            this.Name = "FormSystemPara";
            this.Text = "System Para";
            this.Load += new System.EventHandler(this.FormSystemPara_Load);
            this.PNTitle.ResumeLayout(false);
            this.PNTitle.PerformLayout();
            this.PNDataGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSystemPara)).EndInit();
            this.PNBtn.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PNTitle;
        private System.Windows.Forms.Label LBTitleTxt;
        private System.Windows.Forms.Panel PNDataGrid;
        private System.Windows.Forms.DataGridView dataGridViewSystemPara;
        private System.Windows.Forms.Panel PNBtn;
        private System.Windows.Forms.Button BtnSaveAll;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}