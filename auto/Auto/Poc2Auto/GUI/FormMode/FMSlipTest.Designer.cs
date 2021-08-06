
namespace Poc2Auto.GUI.FormMode
{
    partial class FMSlipTest
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ucModeParams_DoeSlipTest1 = new Poc2Auto.GUI.RunModeConfig.UCModeParams_DoeSlipTest();
            this.btnOk = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.32184F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.67816F));
            this.tableLayoutPanel1.Controls.Add(this.ucModeParams_DoeSlipTest1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOk, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.37993F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.62007F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(435, 279);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ucModeParams_DoeSlipTest1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ucModeParams_DoeSlipTest1, 2);
            this.ucModeParams_DoeSlipTest1.CurrentLanguage = "en-US";
            this.ucModeParams_DoeSlipTest1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucModeParams_DoeSlipTest1.Location = new System.Drawing.Point(3, 4);
            this.ucModeParams_DoeSlipTest1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ucModeParams_DoeSlipTest1.Name = "ucModeParams_DoeSlipTest1";
            this.ucModeParams_DoeSlipTest1.Size = new System.Drawing.Size(429, 233);
            this.ucModeParams_DoeSlipTest1.SlipFilePath = null;
            this.ucModeParams_DoeSlipTest1.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Location = new System.Drawing.Point(331, 241);
            this.btnOk.Margin = new System.Windows.Forms.Padding(0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(104, 38);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FMSlipTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 279);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FMSlipTest";
            this.Text = "FMSlipTest";
            this.Shown += new System.EventHandler(this.FMSlipTest_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RunModeConfig.UCModeParams_DoeSlipTest ucModeParams_DoeSlipTest1;
        private System.Windows.Forms.Button btnOk;
    }
}