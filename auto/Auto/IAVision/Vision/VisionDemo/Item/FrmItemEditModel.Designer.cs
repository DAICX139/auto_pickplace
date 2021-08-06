namespace VisionDemo
{
    partial class FrmItemEditModel
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
            this.rdbErase = new System.Windows.Forms.RadioButton();
            this.rdbDraw = new System.Windows.Forms.RadioButton();
            this.rdbNormal = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnDraw = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtAngleExtent = new System.Windows.Forms.TextBox();
            this.txtAngleStart = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.txtContrast = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.txtScaleMin = new System.Windows.Forms.TextBox();
            this.txtScaleMax = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.pnlFootBase.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExecute
            // 
            this.btnExecute.Text = "重新学习";
            // 
            // rdbErase
            // 
            this.rdbErase.AutoSize = true;
            this.rdbErase.Location = new System.Drawing.Point(10, 103);
            this.rdbErase.Name = "rdbErase";
            this.rdbErase.Size = new System.Drawing.Size(71, 16);
            this.rdbErase.TabIndex = 161;
            this.rdbErase.Text = "擦除模式";
            this.rdbErase.UseVisualStyleBackColor = true;
            this.rdbErase.CheckedChanged += new System.EventHandler(this.rdbNormal_CheckedChanged);
            // 
            // rdbDraw
            // 
            this.rdbDraw.AutoSize = true;
            this.rdbDraw.Checked = true;
            this.rdbDraw.Location = new System.Drawing.Point(10, 71);
            this.rdbDraw.Name = "rdbDraw";
            this.rdbDraw.Size = new System.Drawing.Size(71, 16);
            this.rdbDraw.TabIndex = 160;
            this.rdbDraw.TabStop = true;
            this.rdbDraw.Text = "绘制模式";
            this.rdbDraw.UseVisualStyleBackColor = true;
            this.rdbDraw.CheckedChanged += new System.EventHandler(this.rdbNormal_CheckedChanged);
            // 
            // rdbNormal
            // 
            this.rdbNormal.AutoSize = true;
            this.rdbNormal.Location = new System.Drawing.Point(10, 38);
            this.rdbNormal.Name = "rdbNormal";
            this.rdbNormal.Size = new System.Drawing.Size(71, 16);
            this.rdbNormal.TabIndex = 159;
            this.rdbNormal.Text = "正常显示";
            this.rdbNormal.UseVisualStyleBackColor = true;
            this.rdbNormal.CheckedChanged += new System.EventHandler(this.rdbNormal_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(544, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(256, 460);
            this.panel1.TabIndex = 162;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnDraw);
            this.panel3.Controls.Add(this.btnClear);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.rdbNormal);
            this.panel3.Controls.Add(this.rdbErase);
            this.panel3.Controls.Add(this.rdbDraw);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 290);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(256, 170);
            this.panel3.TabIndex = 164;
            // 
            // btnDraw
            // 
            this.btnDraw.Location = new System.Drawing.Point(135, 38);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(109, 23);
            this.btnDraw.TabIndex = 165;
            this.btnDraw.Text = "绘制掩模";
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(135, 71);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(109, 23);
            this.btnClear.TabIndex = 164;
            this.btnClear.Text = "清除掩模";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(256, 20);
            this.label2.TabIndex = 163;
            this.label2.Text = "编辑模式";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtAngleExtent);
            this.panel2.Controls.Add(this.txtAngleStart);
            this.panel2.Controls.Add(this.label30);
            this.panel2.Controls.Add(this.txtContrast);
            this.panel2.Controls.Add(this.label28);
            this.panel2.Controls.Add(this.label27);
            this.panel2.Controls.Add(this.label29);
            this.panel2.Controls.Add(this.label26);
            this.panel2.Controls.Add(this.txtScaleMin);
            this.panel2.Controls.Add(this.txtScaleMax);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(256, 290);
            this.panel2.TabIndex = 163;
            // 
            // txtAngleExtent
            // 
            this.txtAngleExtent.Location = new System.Drawing.Point(65, 105);
            this.txtAngleExtent.Margin = new System.Windows.Forms.Padding(2);
            this.txtAngleExtent.Name = "txtAngleExtent";
            this.txtAngleExtent.Size = new System.Drawing.Size(180, 21);
            this.txtAngleExtent.TabIndex = 173;
            // 
            // txtAngleStart
            // 
            this.txtAngleStart.Location = new System.Drawing.Point(65, 72);
            this.txtAngleStart.Margin = new System.Windows.Forms.Padding(2);
            this.txtAngleStart.Name = "txtAngleStart";
            this.txtAngleStart.Size = new System.Drawing.Size(180, 21);
            this.txtAngleStart.TabIndex = 164;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(8, 108);
            this.label30.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(53, 12);
            this.label30.TabIndex = 172;
            this.label30.Text = "角度范围";
            // 
            // txtContrast
            // 
            this.txtContrast.Location = new System.Drawing.Point(65, 39);
            this.txtContrast.Margin = new System.Windows.Forms.Padding(2);
            this.txtContrast.Name = "txtContrast";
            this.txtContrast.Size = new System.Drawing.Size(180, 21);
            this.txtContrast.TabIndex = 165;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(8, 174);
            this.label28.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(53, 12);
            this.label28.TabIndex = 171;
            this.label28.Text = "最大尺度";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(8, 42);
            this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(53, 12);
            this.label27.TabIndex = 166;
            this.label27.Text = "梯度阈值";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(8, 141);
            this.label29.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(53, 12);
            this.label29.TabIndex = 170;
            this.label29.Text = "最小尺度";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(8, 75);
            this.label26.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(53, 12);
            this.label26.TabIndex = 167;
            this.label26.Text = "最小角度";
            // 
            // txtScaleMin
            // 
            this.txtScaleMin.Location = new System.Drawing.Point(65, 138);
            this.txtScaleMin.Margin = new System.Windows.Forms.Padding(2);
            this.txtScaleMin.Name = "txtScaleMin";
            this.txtScaleMin.Size = new System.Drawing.Size(180, 21);
            this.txtScaleMin.TabIndex = 169;
            // 
            // txtScaleMax
            // 
            this.txtScaleMax.Location = new System.Drawing.Point(65, 171);
            this.txtScaleMax.Margin = new System.Windows.Forms.Padding(2);
            this.txtScaleMax.Name = "txtScaleMax";
            this.txtScaleMax.Size = new System.Drawing.Size(180, 21);
            this.txtScaleMax.TabIndex = 168;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(256, 20);
            this.label1.TabIndex = 163;
            this.label1.Text = "匹配参数";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlImage
            // 
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImage.Location = new System.Drawing.Point(0, 0);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(544, 460);
            this.pnlImage.TabIndex = 163;
            // 
            // FrmItemEditModel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 508);
            this.Controls.Add(this.pnlImage);
            this.Controls.Add(this.panel1);
            this.Name = "FrmItemEditModel";
            this.Text = "编辑模板";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmItemEditModel_FormClosing);
            this.Load += new System.EventHandler(this.FrmItemEditModel_Load);
            this.Controls.SetChildIndex(this.pnlFootBase, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.pnlImage, 0);
            this.pnlFootBase.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton rdbErase;
        private System.Windows.Forms.RadioButton rdbDraw;
        private System.Windows.Forms.RadioButton rdbNormal;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlImage;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox txtAngleExtent;
        private System.Windows.Forms.TextBox txtAngleStart;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox txtContrast;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox txtScaleMin;
        private System.Windows.Forms.TextBox txtScaleMax;
        private System.Windows.Forms.Button btnDraw;
    }
}