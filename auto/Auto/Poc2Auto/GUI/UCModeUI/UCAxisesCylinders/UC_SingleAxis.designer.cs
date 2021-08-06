
namespace DragonFlex.GUI.Factory.UC_handlePLC
{
    partial class UC_SingleAxis
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
            this.components = new System.ComponentModel.Container();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnHome = new System.Windows.Forms.Button();
            this.btnJogBack = new System.Windows.Forms.Button();
            this.btnJogGo = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.labelEnable = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxSpeed = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cBoxPos = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.labelCurrent = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labelHome = new System.Windows.Forms.Label();
            this.labelMove = new System.Windows.Forms.Label();
            this.labelError = new System.Windows.Forms.Label();
            this.labelDone = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.labelName = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReset
            // 
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnReset.Location = new System.Drawing.Point(227, 56);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(106, 47);
            this.btnReset.TabIndex = 24;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // btnStop
            // 
            this.btnStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStop.Location = new System.Drawing.Point(115, 56);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(106, 47);
            this.btnStop.TabIndex = 23;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnHome
            // 
            this.btnHome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnHome.Location = new System.Drawing.Point(3, 56);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(106, 47);
            this.btnHome.TabIndex = 22;
            this.btnHome.Text = "Home";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // btnJogBack
            // 
            this.btnJogBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnJogBack.Location = new System.Drawing.Point(227, 3);
            this.btnJogBack.Name = "btnJogBack";
            this.btnJogBack.Size = new System.Drawing.Size(106, 47);
            this.btnJogBack.TabIndex = 21;
            this.btnJogBack.Text = "-";
            this.btnJogBack.UseVisualStyleBackColor = true;
            this.btnJogBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogBack_MouseDown);
            this.btnJogBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogBack_MouseUp);
            // 
            // btnJogGo
            // 
            this.btnJogGo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnJogGo.Location = new System.Drawing.Point(115, 3);
            this.btnJogGo.Name = "btnJogGo";
            this.btnJogGo.Size = new System.Drawing.Size(106, 47);
            this.btnJogGo.TabIndex = 20;
            this.btnJogGo.Text = "+";
            this.btnJogGo.UseVisualStyleBackColor = true;
            this.btnJogGo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogGo_MouseDown);
            this.btnJogGo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogGo_MouseUp);
            // 
            // btnGo
            // 
            this.btnGo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGo.Location = new System.Drawing.Point(3, 3);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(106, 47);
            this.btnGo.TabIndex = 19;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBox1.Location = new System.Drawing.Point(253, 105);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 46);
            this.checkBox1.TabIndex = 18;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // labelEnable
            // 
            this.labelEnable.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelEnable.BackColor = System.Drawing.Color.White;
            this.labelEnable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelEnable.Location = new System.Drawing.Point(98, 113);
            this.labelEnable.Name = "labelEnable";
            this.labelEnable.Size = new System.Drawing.Size(30, 30);
            this.labelEnable.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(3, 102);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 52);
            this.label10.TabIndex = 16;
            this.label10.Text = "Enable";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxSpeed
            // 
            this.textBoxSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSpeed.Location = new System.Drawing.Point(167, 81);
            this.textBoxSpeed.Name = "textBoxSpeed";
            this.textBoxSpeed.Size = new System.Drawing.Size(166, 23);
            this.textBoxSpeed.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(3, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(158, 40);
            this.label7.TabIndex = 14;
            this.label7.Text = "Speed[mm/s]";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cBoxPos
            // 
            this.cBoxPos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cBoxPos.FormattingEnabled = true;
            this.cBoxPos.Location = new System.Drawing.Point(167, 42);
            this.cBoxPos.Name = "cBoxPos";
            this.cBoxPos.Size = new System.Drawing.Size(166, 25);
            this.cBoxPos.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(3, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 39);
            this.label6.TabIndex = 12;
            this.label6.Text = "Pos";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCurrent
            // 
            this.labelCurrent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCurrent.BackColor = System.Drawing.Color.Black;
            this.labelCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelCurrent.ForeColor = System.Drawing.Color.Lime;
            this.labelCurrent.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelCurrent.Location = new System.Drawing.Point(167, 3);
            this.labelCurrent.Name = "labelCurrent";
            this.labelCurrent.Size = new System.Drawing.Size(166, 32);
            this.labelCurrent.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(158, 39);
            this.label9.TabIndex = 10;
            this.label9.Text = "Current[mm]";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHome
            // 
            this.labelHome.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelHome.BackColor = System.Drawing.Color.White;
            this.labelHome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelHome.Location = new System.Drawing.Point(253, 10);
            this.labelHome.Name = "labelHome";
            this.labelHome.Size = new System.Drawing.Size(30, 30);
            this.labelHome.TabIndex = 8;
            // 
            // labelMove
            // 
            this.labelMove.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelMove.BackColor = System.Drawing.Color.White;
            this.labelMove.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelMove.Location = new System.Drawing.Point(253, 61);
            this.labelMove.Name = "labelMove";
            this.labelMove.Size = new System.Drawing.Size(30, 30);
            this.labelMove.TabIndex = 7;
            // 
            // labelError
            // 
            this.labelError.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelError.BackColor = System.Drawing.Color.White;
            this.labelError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelError.Location = new System.Drawing.Point(98, 61);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(30, 30);
            this.labelError.TabIndex = 6;
            // 
            // labelDone
            // 
            this.labelDone.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelDone.BackColor = System.Drawing.Color.White;
            this.labelDone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDone.Location = new System.Drawing.Point(98, 10);
            this.labelDone.Name = "labelDone";
            this.labelDone.Size = new System.Drawing.Size(30, 30);
            this.labelDone.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(158, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 51);
            this.label4.TabIndex = 4;
            this.label4.Text = "Moving";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(158, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 51);
            this.label3.TabIndex = 3;
            this.label3.Text = "Homed";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 51);
            this.label2.TabIndex = 2;
            this.label2.Text = "Error";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 51);
            this.label1.TabIndex = 1;
            this.label1.Text = "IsDone";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 86F));
            this.tableLayoutPanel1.Controls.Add(this.label5, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelHome, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelMove, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBox1, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelEnable, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelError, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelDone, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label10, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(336, 154);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(158, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 52);
            this.label5.TabIndex = 19;
            this.label5.Text = "ON/OFF";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 164F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.labelCurrent, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.cBoxPos, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBoxSpeed, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 154);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(336, 118);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel3.Controls.Add(this.btnReset, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnGo, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnStop, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnJogGo, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.btnHome, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.btnJogBack, 2, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 272);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(336, 106);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 36);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 3;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 154F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 118F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(336, 378);
            this.tableLayoutPanel4.TabIndex = 4;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 1;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.labelName, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 2;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(336, 414);
            this.tableLayoutPanel5.TabIndex = 5;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelName.Location = new System.Drawing.Point(0, 0);
            this.labelName.Margin = new System.Windows.Forms.Padding(0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(336, 36);
            this.labelName.TabIndex = 5;
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UC_SingleAxis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel5);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UC_SingleAxis";
            this.Size = new System.Drawing.Size(336, 414);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelDone;
        private System.Windows.Forms.Label labelHome;
        private System.Windows.Forms.Label labelMove;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelCurrent;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cBoxPos;
        private System.Windows.Forms.Label labelEnable;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxSpeed;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnJogGo;
        private System.Windows.Forms.Button btnJogBack;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnHome;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label labelName;
    }
}
