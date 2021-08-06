
namespace Poc2Auto.GUI.FormMode
{
    partial class FMDifferentTray
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
            this.ucModeParams_DoeDifferentTrayTest1 = new Poc2Auto.GUI.RunModeConfig.UCModeParams_DoeDifferentTrayTest();
            this.btnOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.76301F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.237F));
            this.tableLayoutPanel1.Controls.Add(this.ucModeParams_DoeDifferentTrayTest1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 92.64214F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.35786F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(346, 598);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ucModeParams_DoeDifferentTrayTest1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ucModeParams_DoeDifferentTrayTest1, 2);
            this.ucModeParams_DoeDifferentTrayTest1.CurrentLanguage = "en-US";
            this.ucModeParams_DoeDifferentTrayTest1.DifferentTrayDataSrc = null;
            this.ucModeParams_DoeDifferentTrayTest1.DifferentTrayFilePath = null;
            this.ucModeParams_DoeDifferentTrayTest1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucModeParams_DoeDifferentTrayTest1.Location = new System.Drawing.Point(0, 0);
            this.ucModeParams_DoeDifferentTrayTest1.Margin = new System.Windows.Forms.Padding(0);
            this.ucModeParams_DoeDifferentTrayTest1.Name = "ucModeParams_DoeDifferentTrayTest1";
            this.ucModeParams_DoeDifferentTrayTest1.Size = new System.Drawing.Size(346, 554);
            this.ucModeParams_DoeDifferentTrayTest1.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK.Location = new System.Drawing.Point(230, 554);
            this.btnOK.Margin = new System.Windows.Forms.Padding(0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(116, 44);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // FMDifferentTray
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 598);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FMDifferentTray";
            this.Text = "FMDifferentTray";
            this.Shown += new System.EventHandler(this.FMDifferentTray_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RunModeConfig.UCModeParams_DoeDifferentTrayTest ucModeParams_DoeDifferentTrayTest1;
        private System.Windows.Forms.Button btnOK;
    }
}