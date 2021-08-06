namespace VisionFlows
{
    partial class FrmVisionUI
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnCalib = new System.Windows.Forms.Button();
            this.chkOriginValue = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button_change = new System.Windows.Forms.Button();
            this.label_type = new System.Windows.Forms.Label();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTest);
            this.groupBox1.Controls.Add(this.btnCalib);
            this.groupBox1.Controls.Add(this.chkOriginValue);
            this.groupBox1.Location = new System.Drawing.Point(15, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(460, 229);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            // 
            // btnTest
            // 
            this.btnTest.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTest.Location = new System.Drawing.Point(287, 94);
            this.btnTest.Margin = new System.Windows.Forms.Padding(4);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(155, 66);
            this.btnTest.TabIndex = 18;
            this.btnTest.Text = "设置";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnCalib
            // 
            this.btnCalib.Font = new System.Drawing.Font("楷体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCalib.Location = new System.Drawing.Point(28, 94);
            this.btnCalib.Margin = new System.Windows.Forms.Padding(4);
            this.btnCalib.Name = "btnCalib";
            this.btnCalib.Size = new System.Drawing.Size(155, 66);
            this.btnCalib.TabIndex = 17;
            this.btnCalib.Text = "标定";
            this.btnCalib.UseVisualStyleBackColor = true;
            this.btnCalib.Click += new System.EventHandler(this.btnCalib_Click);
            // 
            // chkOriginValue
            // 
            this.chkOriginValue.AutoSize = true;
            this.chkOriginValue.Font = new System.Drawing.Font("楷体", 9F);
            this.chkOriginValue.Location = new System.Drawing.Point(319, 198);
            this.chkOriginValue.Margin = new System.Windows.Forms.Padding(4);
            this.chkOriginValue.Name = "chkOriginValue";
            this.chkOriginValue.Size = new System.Drawing.Size(93, 19);
            this.chkOriginValue.TabIndex = 16;
            this.chkOriginValue.Text = "空跑模式";
            this.chkOriginValue.UseVisualStyleBackColor = true;
            this.chkOriginValue.CheckedChanged += new System.EventHandler(this.chkOriginValue_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.button_change);
            this.groupBox2.Controls.Add(this.label_type);
            this.groupBox2.Controls.Add(this.comboBox_type);
            this.groupBox2.Location = new System.Drawing.Point(15, 249);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(460, 405);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(69, 225);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(174, 50);
            this.button1.TabIndex = 23;
            this.button1.Text = "button1";
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_change
            // 
            this.button_change.Font = new System.Drawing.Font("楷体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_change.Location = new System.Drawing.Point(287, 66);
            this.button_change.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_change.Name = "button_change";
            this.button_change.Size = new System.Drawing.Size(155, 38);
            this.button_change.TabIndex = 22;
            this.button_change.Text = "切换";
            this.button_change.UseVisualStyleBackColor = true;
            this.button_change.Click += new System.EventHandler(this.button_change_Click);
            // 
            // label_type
            // 
            this.label_type.AutoSize = true;
            this.label_type.Font = new System.Drawing.Font("楷体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_type.Location = new System.Drawing.Point(23, 76);
            this.label_type.Name = "label_type";
            this.label_type.Size = new System.Drawing.Size(58, 24);
            this.label_type.TabIndex = 1;
            this.label_type.Text = "配方";
            // 
            // comboBox_type
            // 
            this.comboBox_type.Font = new System.Drawing.Font("楷体", 13.8F);
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Location = new System.Drawing.Point(93, 71);
            this.comboBox_type.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(121, 31);
            this.comboBox_type.TabIndex = 0;
            // 
            // FrmVisionUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 669);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmVisionUI";
            this.Text = "Vision UI";
            this.Load += new System.EventHandler(this.FrmVisionUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkOriginValue;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnCalib;
        private System.Windows.Forms.Button button_change;
        private System.Windows.Forms.Label label_type;
        private System.Windows.Forms.ComboBox comboBox_type;
        private System.Windows.Forms.Button button1;
    }
}