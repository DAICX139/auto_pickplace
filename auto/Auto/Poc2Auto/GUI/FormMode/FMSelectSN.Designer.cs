
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbtnNG = new System.Windows.Forms.RadioButton();
            this.rdbtnLoad2 = new System.Windows.Forms.RadioButton();
            this.rdbtnLoad1 = new System.Windows.Forms.RadioButton();
            this.ucModeParams_SelectDut_SN1 = new Poc2Auto.GUI.RunModeConfig.UCModeParams_SelectDut_SN();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.34247F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.65753F));
            this.tableLayoutPanel1.Controls.Add(this.btnOk, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ucModeParams_SelectDut_SN1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.6087F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.3913F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(392, 463);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOk.Location = new System.Drawing.Point(302, 412);
            this.btnOk.Margin = new System.Windows.Forms.Padding(7);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(83, 44);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbtnNG);
            this.groupBox1.Controls.Add(this.rdbtnLoad2);
            this.groupBox1.Controls.Add(this.rdbtnLoad1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 405);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(295, 53);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tray 选择";
            // 
            // rdbtnNG
            // 
            this.rdbtnNG.AutoSize = true;
            this.rdbtnNG.Location = new System.Drawing.Point(195, 24);
            this.rdbtnNG.Margin = new System.Windows.Forms.Padding(0);
            this.rdbtnNG.Name = "rdbtnNG";
            this.rdbtnNG.Size = new System.Drawing.Size(53, 24);
            this.rdbtnNG.TabIndex = 2;
            this.rdbtnNG.TabStop = true;
            this.rdbtnNG.Text = "NG";
            this.rdbtnNG.UseVisualStyleBackColor = true;
            this.rdbtnNG.CheckedChanged += new System.EventHandler(this.TraySlectionChanged);
            // 
            // rdbtnLoad2
            // 
            this.rdbtnLoad2.AutoSize = true;
            this.rdbtnLoad2.Location = new System.Drawing.Point(103, 24);
            this.rdbtnLoad2.Margin = new System.Windows.Forms.Padding(0);
            this.rdbtnLoad2.Name = "rdbtnLoad2";
            this.rdbtnLoad2.Size = new System.Drawing.Size(71, 24);
            this.rdbtnLoad2.TabIndex = 1;
            this.rdbtnLoad2.TabStop = true;
            this.rdbtnLoad2.Text = "load2";
            this.rdbtnLoad2.UseVisualStyleBackColor = true;
            this.rdbtnLoad2.CheckedChanged += new System.EventHandler(this.TraySlectionChanged);
            // 
            // rdbtnLoad1
            // 
            this.rdbtnLoad1.AutoSize = true;
            this.rdbtnLoad1.Location = new System.Drawing.Point(12, 24);
            this.rdbtnLoad1.Margin = new System.Windows.Forms.Padding(0);
            this.rdbtnLoad1.Name = "rdbtnLoad1";
            this.rdbtnLoad1.Size = new System.Drawing.Size(71, 24);
            this.rdbtnLoad1.TabIndex = 0;
            this.rdbtnLoad1.TabStop = true;
            this.rdbtnLoad1.Text = "load1";
            this.rdbtnLoad1.UseVisualStyleBackColor = true;
            this.rdbtnLoad1.CheckedChanged += new System.EventHandler(this.TraySlectionChanged);
            // 
            // ucModeParams_SelectDut_SN1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ucModeParams_SelectDut_SN1, 2);
            this.ucModeParams_SelectDut_SN1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucModeParams_SelectDut_SN1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucModeParams_SelectDut_SN1.Location = new System.Drawing.Point(2, 0);
            this.ucModeParams_SelectDut_SN1.Margin = new System.Windows.Forms.Padding(2, 0, 0, 2);
            this.ucModeParams_SelectDut_SN1.Name = "ucModeParams_SelectDut_SN1";
            this.ucModeParams_SelectDut_SN1.ParamData = null;
            this.ucModeParams_SelectDut_SN1.Size = new System.Drawing.Size(390, 403);
            this.ucModeParams_SelectDut_SN1.TabIndex = 2;
            // 
            // FMSelectSN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 463);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(410, 510);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(410, 510);
            this.Name = "FMSelectSN";
            this.Text = "挑选SN";
            this.Shown += new System.EventHandler(this.FMSelectSN_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnOk;
        private RunModeConfig.UCModeParams_SelectDut_SN ucModeParams_SelectDut_SN1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbtnNG;
        private System.Windows.Forms.RadioButton rdbtnLoad2;
        private System.Windows.Forms.RadioButton rdbtnLoad1;
    }
}