namespace VisionDemo
{
    partial class FrmItemMeasureCircle
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
            this.pnlImage = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtRadius = new System.Windows.Forms.TextBox();
            this.txtColumn = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRow = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbInputImage = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.cboMeasureSelect = new System.Windows.Forms.ComboBox();
            this.cboMeasureTransition = new System.Windows.Forms.ComboBox();
            this.txtMeasureLength2 = new System.Windows.Forms.TextBox();
            this.txtMeasureThreshold = new System.Windows.Forms.TextBox();
            this.txtMeasureDistance = new System.Windows.Forms.TextBox();
            this.txtMeasureLength1 = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtResultRadius = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.txtResultRow = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.txtResultColumn = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.pnlFootBase.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlImage
            // 
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImage.Location = new System.Drawing.Point(0, 0);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(544, 460);
            this.pnlImage.TabIndex = 7;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabControl1.Location = new System.Drawing.Point(544, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(256, 460);
            this.tabControl1.TabIndex = 6;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(248, 434);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本参数";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtRadius);
            this.panel2.Controls.Add(this.txtColumn);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.txtRow);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 87);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(242, 344);
            this.panel2.TabIndex = 1;
            // 
            // txtRadius
            // 
            this.txtRadius.Location = new System.Drawing.Point(67, 105);
            this.txtRadius.Name = "txtRadius";
            this.txtRadius.Size = new System.Drawing.Size(164, 21);
            this.txtRadius.TabIndex = 98;
            this.txtRadius.Text = "240";
            // 
            // txtColumn
            // 
            this.txtColumn.Location = new System.Drawing.Point(67, 72);
            this.txtColumn.Name = "txtColumn";
            this.txtColumn.Size = new System.Drawing.Size(164, 21);
            this.txtColumn.TabIndex = 97;
            this.txtColumn.Text = "1210";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(8, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 95;
            this.label6.Text = "圆心C";
            // 
            // txtRow
            // 
            this.txtRow.Location = new System.Drawing.Point(67, 39);
            this.txtRow.Name = "txtRow";
            this.txtRow.Size = new System.Drawing.Size(164, 21);
            this.txtRow.TabIndex = 94;
            this.txtRow.Text = "1010";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(8, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 93;
            this.label5.Text = "半径";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(8, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 92;
            this.label4.Text = "圆心R";
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
            this.label2.Size = new System.Drawing.Size(242, 20);
            this.label2.TabIndex = 83;
            this.label2.Text = "ROI信息";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbInputImage);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label32);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(242, 84);
            this.panel1.TabIndex = 0;
            // 
            // cmbInputImage
            // 
            this.cmbInputImage.FormattingEnabled = true;
            this.cmbInputImage.Location = new System.Drawing.Point(67, 39);
            this.cmbInputImage.Name = "cmbInputImage";
            this.cmbInputImage.Size = new System.Drawing.Size(164, 20);
            this.cmbInputImage.TabIndex = 108;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 107;
            this.label3.Text = "输入图像";
            // 
            // label32
            // 
            this.label32.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label32.Dock = System.Windows.Forms.DockStyle.Top;
            this.label32.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label32.ForeColor = System.Drawing.Color.White;
            this.label32.Location = new System.Drawing.Point(0, 0);
            this.label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(242, 20);
            this.label32.TabIndex = 83;
            this.label32.Text = "图像设置";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(248, 434);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "参数设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.cboMeasureSelect);
            this.panel5.Controls.Add(this.cboMeasureTransition);
            this.panel5.Controls.Add(this.txtMeasureLength2);
            this.panel5.Controls.Add(this.txtMeasureThreshold);
            this.panel5.Controls.Add(this.txtMeasureDistance);
            this.panel5.Controls.Add(this.txtMeasureLength1);
            this.panel5.Controls.Add(this.label40);
            this.panel5.Controls.Add(this.label39);
            this.panel5.Controls.Add(this.label38);
            this.panel5.Controls.Add(this.label37);
            this.panel5.Controls.Add(this.label36);
            this.panel5.Controls.Add(this.label18);
            this.panel5.Controls.Add(this.label8);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(242, 428);
            this.panel5.TabIndex = 3;
            // 
            // cboMeasureSelect
            // 
            this.cboMeasureSelect.FormattingEnabled = true;
            this.cboMeasureSelect.Items.AddRange(new object[] {
            "first",
            "last",
            "all"});
            this.cboMeasureSelect.Location = new System.Drawing.Point(65, 204);
            this.cboMeasureSelect.Margin = new System.Windows.Forms.Padding(2);
            this.cboMeasureSelect.Name = "cboMeasureSelect";
            this.cboMeasureSelect.Size = new System.Drawing.Size(164, 20);
            this.cboMeasureSelect.TabIndex = 155;
            this.cboMeasureSelect.Text = "first";
            // 
            // cboMeasureTransition
            // 
            this.cboMeasureTransition.FormattingEnabled = true;
            this.cboMeasureTransition.Items.AddRange(new object[] {
            "positive",
            "negative",
            "all"});
            this.cboMeasureTransition.Location = new System.Drawing.Point(65, 171);
            this.cboMeasureTransition.Margin = new System.Windows.Forms.Padding(2);
            this.cboMeasureTransition.Name = "cboMeasureTransition";
            this.cboMeasureTransition.Size = new System.Drawing.Size(164, 20);
            this.cboMeasureTransition.TabIndex = 154;
            this.cboMeasureTransition.Text = "positive";
            // 
            // txtMeasureLength2
            // 
            this.txtMeasureLength2.Location = new System.Drawing.Point(65, 72);
            this.txtMeasureLength2.Margin = new System.Windows.Forms.Padding(2);
            this.txtMeasureLength2.Name = "txtMeasureLength2";
            this.txtMeasureLength2.Size = new System.Drawing.Size(164, 21);
            this.txtMeasureLength2.TabIndex = 153;
            this.txtMeasureLength2.Text = "5.0";
            // 
            // txtMeasureThreshold
            // 
            this.txtMeasureThreshold.Location = new System.Drawing.Point(65, 105);
            this.txtMeasureThreshold.Margin = new System.Windows.Forms.Padding(2);
            this.txtMeasureThreshold.Name = "txtMeasureThreshold";
            this.txtMeasureThreshold.Size = new System.Drawing.Size(164, 21);
            this.txtMeasureThreshold.TabIndex = 152;
            this.txtMeasureThreshold.Text = "30.0";
            // 
            // txtMeasureDistance
            // 
            this.txtMeasureDistance.Location = new System.Drawing.Point(65, 138);
            this.txtMeasureDistance.Margin = new System.Windows.Forms.Padding(2);
            this.txtMeasureDistance.Name = "txtMeasureDistance";
            this.txtMeasureDistance.Size = new System.Drawing.Size(164, 21);
            this.txtMeasureDistance.TabIndex = 151;
            this.txtMeasureDistance.Text = "10.0";
            // 
            // txtMeasureLength1
            // 
            this.txtMeasureLength1.Location = new System.Drawing.Point(65, 39);
            this.txtMeasureLength1.Margin = new System.Windows.Forms.Padding(2);
            this.txtMeasureLength1.Name = "txtMeasureLength1";
            this.txtMeasureLength1.Size = new System.Drawing.Size(164, 21);
            this.txtMeasureLength1.TabIndex = 150;
            this.txtMeasureLength1.Text = "20";
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(8, 207);
            this.label40.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(53, 12);
            this.label40.TabIndex = 149;
            this.label40.Text = "点位筛选";
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Location = new System.Drawing.Point(8, 174);
            this.label39.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(53, 12);
            this.label39.TabIndex = 148;
            this.label39.Text = "颜色模式";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(8, 141);
            this.label38.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(53, 12);
            this.label38.TabIndex = 147;
            this.label38.Text = "卡尺间隔";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(8, 108);
            this.label37.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(53, 12);
            this.label37.TabIndex = 146;
            this.label37.Text = "灰度阈值";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(8, 75);
            this.label36.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(53, 12);
            this.label36.TabIndex = 145;
            this.label36.Text = "卡尺高度";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(8, 42);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(53, 12);
            this.label18.TabIndex = 144;
            this.label18.Text = "卡尺宽度";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(242, 20);
            this.label8.TabIndex = 83;
            this.label8.Text = "测量参数";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label8.Click += new System.EventHandler(this.label8_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel3);
            this.tabPage3.Controls.Add(this.panel4);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(248, 434);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "数据结果";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.checkBox2);
            this.panel3.Controls.Add(this.checkBox1);
            this.panel3.Controls.Add(this.checkBox4);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 153);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(242, 278);
            this.panel3.TabIndex = 3;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(47, 104);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(96, 16);
            this.checkBox2.TabIndex = 108;
            this.checkBox2.Text = "显示测量轮廓";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(47, 73);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(84, 16);
            this.checkBox1.TabIndex = 107;
            this.checkBox1.Text = "显示结果圆";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(47, 42);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(84, 16);
            this.checkBox4.TabIndex = 106;
            this.checkBox4.Text = "显示结果点";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label11.Dock = System.Windows.Forms.DockStyle.Top;
            this.label11.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(0, 0);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(242, 20);
            this.label11.TabIndex = 83;
            this.label11.Text = "显示设置";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.txtResultRadius);
            this.panel4.Controls.Add(this.label46);
            this.panel4.Controls.Add(this.txtResultRow);
            this.panel4.Controls.Add(this.label44);
            this.panel4.Controls.Add(this.txtResultColumn);
            this.panel4.Controls.Add(this.label45);
            this.panel4.Controls.Add(this.label13);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(242, 150);
            this.panel4.TabIndex = 2;
            // 
            // txtResultRadius
            // 
            this.txtResultRadius.Location = new System.Drawing.Point(47, 105);
            this.txtResultRadius.Margin = new System.Windows.Forms.Padding(2);
            this.txtResultRadius.Name = "txtResultRadius";
            this.txtResultRadius.Size = new System.Drawing.Size(182, 21);
            this.txtResultRadius.TabIndex = 137;
            this.txtResultRadius.Text = "0";
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Location = new System.Drawing.Point(8, 108);
            this.label46.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(29, 12);
            this.label46.TabIndex = 136;
            this.label46.Text = "半径";
            // 
            // txtResultRow
            // 
            this.txtResultRow.Location = new System.Drawing.Point(47, 39);
            this.txtResultRow.Margin = new System.Windows.Forms.Padding(2);
            this.txtResultRow.Name = "txtResultRow";
            this.txtResultRow.Size = new System.Drawing.Size(182, 21);
            this.txtResultRow.TabIndex = 135;
            this.txtResultRow.Text = "0";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Location = new System.Drawing.Point(8, 42);
            this.label44.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(35, 12);
            this.label44.TabIndex = 134;
            this.label44.Text = "圆心R";
            // 
            // txtResultColumn
            // 
            this.txtResultColumn.Location = new System.Drawing.Point(47, 72);
            this.txtResultColumn.Margin = new System.Windows.Forms.Padding(2);
            this.txtResultColumn.Name = "txtResultColumn";
            this.txtResultColumn.Size = new System.Drawing.Size(182, 21);
            this.txtResultColumn.TabIndex = 133;
            this.txtResultColumn.Text = "0";
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Location = new System.Drawing.Point(8, 75);
            this.label45.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(35, 12);
            this.label45.TabIndex = 132;
            this.label45.Text = "圆心C";
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label13.Dock = System.Windows.Forms.DockStyle.Top;
            this.label13.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(0, 0);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(242, 20);
            this.label13.TabIndex = 83;
            this.label13.Text = "结果圆";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FrmItemMeasureCircle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 508);
            this.Controls.Add(this.pnlImage);
            this.Controls.Add(this.tabControl1);
            this.Name = "FrmItemMeasureCircle";
            this.Text = "圆形测量";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmItemCircleMeasure_FormClosing);
            this.Load += new System.EventHandler(this.FrmItemCircleMeasure_Load);
            this.Controls.SetChildIndex(this.pnlFootBase, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.pnlImage, 0);
            this.pnlFootBase.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlImage;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtRadius;
        private System.Windows.Forms.TextBox txtColumn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRow;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cmbInputImage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboMeasureSelect;
        private System.Windows.Forms.ComboBox cboMeasureTransition;
        private System.Windows.Forms.TextBox txtMeasureLength2;
        private System.Windows.Forms.TextBox txtMeasureThreshold;
        private System.Windows.Forms.TextBox txtMeasureDistance;
        private System.Windows.Forms.TextBox txtMeasureLength1;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtResultRadius;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.TextBox txtResultRow;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox txtResultColumn;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox4;
    }
}