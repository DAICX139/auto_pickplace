
namespace Poc2Auto.GUI.FormMode
{
    partial class FMSocketTest
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
            this.btnOK = new System.Windows.Forms.Button();
            this.ucModeParams_SocketTest1 = new Poc2Auto.GUI.RunModeConfig.UCModeParams_SocketTest();
            this.btnContinueTest = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 54F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 166F));
            this.tableLayoutPanel1.Controls.Add(this.ucModeParams_SocketTest1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOK, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnContinueTest, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.42583F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.57416F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(467, 181);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK.Location = new System.Drawing.Point(300, 143);
            this.btnOK.Margin = new System.Windows.Forms.Padding(0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(167, 38);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ucModeParams_SocketTest1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ucModeParams_SocketTest1, 3);
            this.ucModeParams_SocketTest1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucModeParams_SocketTest1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucModeParams_SocketTest1.Location = new System.Drawing.Point(0, 0);
            this.ucModeParams_SocketTest1.Margin = new System.Windows.Forms.Padding(0);
            this.ucModeParams_SocketTest1.Name = "ucModeParams_SocketTest1";
            this.ucModeParams_SocketTest1.Size = new System.Drawing.Size(467, 143);
            this.ucModeParams_SocketTest1.TabIndex = 0;
            // 
            // btnContinueTest
            // 
            this.btnContinueTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnContinueTest.Location = new System.Drawing.Point(0, 143);
            this.btnContinueTest.Margin = new System.Windows.Forms.Padding(0);
            this.btnContinueTest.Name = "btnContinueTest";
            this.btnContinueTest.Size = new System.Drawing.Size(162, 38);
            this.btnContinueTest.TabIndex = 2;
            this.btnContinueTest.Text = "Continue Test";
            this.btnContinueTest.UseVisualStyleBackColor = true;
            this.btnContinueTest.Click += new System.EventHandler(this.btnContinueTest_Click);
            // 
            // FMSocketTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 181);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FMSocketTest";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FMSocketTest";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.FMSocketTest_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private RunModeConfig.UCModeParams_SocketTest ucModeParams_SocketTest1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnContinueTest;
    }
}