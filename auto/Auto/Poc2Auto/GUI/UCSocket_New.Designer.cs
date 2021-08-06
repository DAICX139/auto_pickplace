
namespace Poc2Auto.GUI
{
    partial class UCSocket_New
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
            this.ckbxSocket2 = new System.Windows.Forms.CheckBox();
            this.ckbxSocket5 = new System.Windows.Forms.CheckBox();
            this.ckbxSocket1 = new System.Windows.Forms.CheckBox();
            this.ckbxSocket3 = new System.Windows.Forms.CheckBox();
            this.ckbxSocket4 = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ckbxSocket2
            // 
            this.ckbxSocket2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbxSocket2.AutoSize = true;
            this.ckbxSocket2.Location = new System.Drawing.Point(3, 69);
            this.ckbxSocket2.Name = "ckbxSocket2";
            this.ckbxSocket2.Size = new System.Drawing.Size(99, 24);
            this.ckbxSocket2.TabIndex = 0;
            this.ckbxSocket2.Text = "S2";
            this.ckbxSocket2.UseVisualStyleBackColor = true;
            this.ckbxSocket2.CheckedChanged += new System.EventHandler(this.ckbxSocket2_CheckedChanged);
            // 
            // ckbxSocket5
            // 
            this.ckbxSocket5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbxSocket5.AutoSize = true;
            this.ckbxSocket5.Location = new System.Drawing.Point(108, 69);
            this.ckbxSocket5.Name = "ckbxSocket5";
            this.ckbxSocket5.Size = new System.Drawing.Size(106, 24);
            this.ckbxSocket5.TabIndex = 1;
            this.ckbxSocket5.Text = "S5";
            this.ckbxSocket5.UseVisualStyleBackColor = true;
            this.ckbxSocket5.CheckedChanged += new System.EventHandler(this.ckbxSocket5_CheckedChanged);
            // 
            // ckbxSocket1
            // 
            this.ckbxSocket1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbxSocket1.AutoSize = true;
            this.ckbxSocket1.Location = new System.Drawing.Point(3, 15);
            this.ckbxSocket1.Name = "ckbxSocket1";
            this.ckbxSocket1.Size = new System.Drawing.Size(99, 24);
            this.ckbxSocket1.TabIndex = 2;
            this.ckbxSocket1.Text = "S1";
            this.ckbxSocket1.UseVisualStyleBackColor = true;
            this.ckbxSocket1.CheckedChanged += new System.EventHandler(this.ckbxSocket1_CheckedChanged);
            // 
            // ckbxSocket3
            // 
            this.ckbxSocket3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbxSocket3.AutoSize = true;
            this.ckbxSocket3.Location = new System.Drawing.Point(3, 123);
            this.ckbxSocket3.Name = "ckbxSocket3";
            this.ckbxSocket3.Size = new System.Drawing.Size(99, 24);
            this.ckbxSocket3.TabIndex = 3;
            this.ckbxSocket3.Text = "S3";
            this.ckbxSocket3.UseVisualStyleBackColor = true;
            this.ckbxSocket3.CheckedChanged += new System.EventHandler(this.ckbxSocket3_CheckedChanged);
            // 
            // ckbxSocket4
            // 
            this.ckbxSocket4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.ckbxSocket4.AutoSize = true;
            this.ckbxSocket4.Location = new System.Drawing.Point(108, 15);
            this.ckbxSocket4.Name = "ckbxSocket4";
            this.ckbxSocket4.Size = new System.Drawing.Size(106, 24);
            this.ckbxSocket4.TabIndex = 4;
            this.ckbxSocket4.Text = "S4";
            this.ckbxSocket4.UseVisualStyleBackColor = true;
            this.ckbxSocket4.CheckedChanged += new System.EventHandler(this.ckbxSocket4_CheckedChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.52941F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.47059F));
            this.tableLayoutPanel1.Controls.Add(this.ckbxSocket1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ckbxSocket5, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.ckbxSocket4, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.ckbxSocket2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ckbxSocket3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(217, 162);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // UCSocket_New
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UCSocket_New";
            this.Size = new System.Drawing.Size(217, 162);
            this.Load += new System.EventHandler(this.UCSocket_New_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox ckbxSocket2;
        private System.Windows.Forms.CheckBox ckbxSocket5;
        private System.Windows.Forms.CheckBox ckbxSocket1;
        private System.Windows.Forms.CheckBox ckbxSocket3;
        private System.Windows.Forms.CheckBox ckbxSocket4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
