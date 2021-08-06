
namespace Poc2Auto.GUI.FormMode
{
    partial class FMSelectSN
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
            this.btnOk = new System.Windows.Forms.Button();
            this.ucModeParams_SelectDut_SN1 = new Poc2Auto.GUI.RunModeConfig.UCModeParams_SelectDut_SN();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.34247F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.65753F));
            this.tableLayoutPanel1.Controls.Add(this.btnOk, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ucModeParams_SelectDut_SN1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.76087F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.23913F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(292, 368);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOk.Location = new System.Drawing.Point(220, 334);
            this.btnOk.Margin = new System.Windows.Forms.Padding(0);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(72, 34);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // ucModeParams_SelectDut_SN1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ucModeParams_SelectDut_SN1, 2);
            this.ucModeParams_SelectDut_SN1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucModeParams_SelectDut_SN1.Location = new System.Drawing.Point(4, 4);
            this.ucModeParams_SelectDut_SN1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ucModeParams_SelectDut_SN1.Name = "ucModeParams_SelectDut_SN1";
            this.ucModeParams_SelectDut_SN1.ParamData = null;
            this.ucModeParams_SelectDut_SN1.Size = new System.Drawing.Size(284, 326);
            this.ucModeParams_SelectDut_SN1.TabIndex = 2;
            // 
            // FMSelectSN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 368);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FMSelectSN";
            this.Text = "挑选SN";
            this.Shown += new System.EventHandler(this.FMSelectSN_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnOk;
        private RunModeConfig.UCModeParams_SelectDut_SN ucModeParams_SelectDut_SN1;
    }
}