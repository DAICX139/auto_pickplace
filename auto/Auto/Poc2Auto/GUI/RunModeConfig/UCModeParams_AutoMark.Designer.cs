
namespace Poc2Auto.GUI.RunModeConfig
{
    partial class UCModeParams_AutoMark
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
            this.label2 = new System.Windows.Forms.Label();
            this.cbxMarkTrayId = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbtnSpecifyTrayMark = new System.Windows.Forms.RadioButton();
            this.rdbtnCompleteProcessMark = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdbtnUnLoadMark = new System.Windows.Forms.RadioButton();
            this.rdbtnLoadMark = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tray ID:";
            // 
            // cbxMarkTrayId
            // 
            this.cbxMarkTrayId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMarkTrayId.FormattingEnabled = true;
            this.cbxMarkTrayId.Location = new System.Drawing.Point(82, 23);
            this.cbxMarkTrayId.Name = "cbxMarkTrayId";
            this.cbxMarkTrayId.Size = new System.Drawing.Size(121, 28);
            this.cbxMarkTrayId.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbtnSpecifyTrayMark);
            this.groupBox1.Controls.Add(this.rdbtnCompleteProcessMark);
            this.groupBox1.Location = new System.Drawing.Point(17, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(477, 62);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Calibration mode";
            // 
            // rdbtnSpecifyTrayMark
            // 
            this.rdbtnSpecifyTrayMark.AutoSize = true;
            this.rdbtnSpecifyTrayMark.Location = new System.Drawing.Point(271, 27);
            this.rdbtnSpecifyTrayMark.Name = "rdbtnSpecifyTrayMark";
            this.rdbtnSpecifyTrayMark.Size = new System.Drawing.Size(136, 24);
            this.rdbtnSpecifyTrayMark.TabIndex = 11;
            this.rdbtnSpecifyTrayMark.TabStop = true;
            this.rdbtnSpecifyTrayMark.Text = "指定Tray盘标定";
            this.rdbtnSpecifyTrayMark.UseVisualStyleBackColor = true;
            this.rdbtnSpecifyTrayMark.CheckedChanged += new System.EventHandler(this.rdbtnSpecifyTrayMark_CheckedChanged);
            // 
            // rdbtnCompleteProcessMark
            // 
            this.rdbtnCompleteProcessMark.AutoSize = true;
            this.rdbtnCompleteProcessMark.Location = new System.Drawing.Point(18, 27);
            this.rdbtnCompleteProcessMark.Name = "rdbtnCompleteProcessMark";
            this.rdbtnCompleteProcessMark.Size = new System.Drawing.Size(120, 24);
            this.rdbtnCompleteProcessMark.TabIndex = 10;
            this.rdbtnCompleteProcessMark.TabStop = true;
            this.rdbtnCompleteProcessMark.Text = "完整流程标定";
            this.rdbtnCompleteProcessMark.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdbtnUnLoadMark);
            this.groupBox2.Controls.Add(this.rdbtnLoadMark);
            this.groupBox2.Location = new System.Drawing.Point(17, 95);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(477, 58);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Vision select";
            // 
            // rdbtnUnLoadMark
            // 
            this.rdbtnUnLoadMark.AutoSize = true;
            this.rdbtnUnLoadMark.Location = new System.Drawing.Point(271, 22);
            this.rdbtnUnLoadMark.Name = "rdbtnUnLoadMark";
            this.rdbtnUnLoadMark.Size = new System.Drawing.Size(90, 24);
            this.rdbtnUnLoadMark.TabIndex = 15;
            this.rdbtnUnLoadMark.TabStop = true;
            this.rdbtnUnLoadMark.Text = "下料相机";
            this.rdbtnUnLoadMark.UseVisualStyleBackColor = true;
            this.rdbtnUnLoadMark.CheckedChanged += new System.EventHandler(this.rdbtnUnLoadMark_CheckedChanged);
            // 
            // rdbtnLoadMark
            // 
            this.rdbtnLoadMark.AutoSize = true;
            this.rdbtnLoadMark.Location = new System.Drawing.Point(18, 22);
            this.rdbtnLoadMark.Name = "rdbtnLoadMark";
            this.rdbtnLoadMark.Size = new System.Drawing.Size(90, 24);
            this.rdbtnLoadMark.TabIndex = 11;
            this.rdbtnLoadMark.TabStop = true;
            this.rdbtnLoadMark.Text = "上料相机";
            this.rdbtnLoadMark.UseVisualStyleBackColor = true;
            this.rdbtnLoadMark.CheckedChanged += new System.EventHandler(this.rdbtnLoadMark_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbxMarkTrayId);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(17, 178);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(477, 58);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Tray select";
            // 
            // UCModeParams_AutoMark
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UCModeParams_AutoMark";
            this.Size = new System.Drawing.Size(505, 246);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbxMarkTrayId;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rdbtnSpecifyTrayMark;
        private System.Windows.Forms.RadioButton rdbtnCompleteProcessMark;
        private System.Windows.Forms.RadioButton rdbtnUnLoadMark;
        private System.Windows.Forms.RadioButton rdbtnLoadMark;
    }
}
