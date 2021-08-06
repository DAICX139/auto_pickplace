
namespace Poc2Auto.GUI.RunModeConfig
{
    partial class UCModeParams_DoeTakeOffTest
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
            this.cbxTrayId = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nbTestRow = new System.Windows.Forms.NumericUpDown();
            this.nbTestCol = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nbTestTimes = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.ckxCloseCap = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.nmSocketID = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nbTestRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbTestCol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbTestTimes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmSocketID)).BeginInit();
            this.SuspendLayout();
            // 
            // cbxTrayId
            // 
            this.cbxTrayId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTrayId.FormattingEnabled = true;
            this.cbxTrayId.Location = new System.Drawing.Point(108, 15);
            this.cbxTrayId.Margin = new System.Windows.Forms.Padding(0);
            this.cbxTrayId.Name = "cbxTrayId";
            this.cbxTrayId.Size = new System.Drawing.Size(139, 28);
            this.cbxTrayId.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tray ID:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(290, 18);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Test Row:";
            // 
            // nbTestRow
            // 
            this.nbTestRow.Location = new System.Drawing.Point(371, 15);
            this.nbTestRow.Margin = new System.Windows.Forms.Padding(0);
            this.nbTestRow.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.nbTestRow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nbTestRow.Name = "nbTestRow";
            this.nbTestRow.Size = new System.Drawing.Size(140, 27);
            this.nbTestRow.TabIndex = 3;
            this.nbTestRow.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nbTestCol
            // 
            this.nbTestCol.Location = new System.Drawing.Point(371, 66);
            this.nbTestCol.Margin = new System.Windows.Forms.Padding(0);
            this.nbTestCol.Maximum = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.nbTestCol.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nbTestCol.Name = "nbTestCol";
            this.nbTestCol.Size = new System.Drawing.Size(140, 27);
            this.nbTestCol.TabIndex = 5;
            this.nbTestCol.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(296, 70);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Test Col:";
            // 
            // nbTestTimes
            // 
            this.nbTestTimes.Location = new System.Drawing.Point(108, 69);
            this.nbTestTimes.Margin = new System.Windows.Forms.Padding(0);
            this.nbTestTimes.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nbTestTimes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nbTestTimes.Name = "nbTestTimes";
            this.nbTestTimes.Size = new System.Drawing.Size(140, 27);
            this.nbTestTimes.TabIndex = 7;
            this.nbTestTimes.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 72);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Test times：";
            // 
            // ckxCloseCap
            // 
            this.ckxCloseCap.AutoSize = true;
            this.ckxCloseCap.Location = new System.Drawing.Point(305, 118);
            this.ckxCloseCap.Margin = new System.Windows.Forms.Padding(0);
            this.ckxCloseCap.Name = "ckxCloseCap";
            this.ckxCloseCap.Size = new System.Drawing.Size(213, 24);
            this.ckxCloseCap.TabIndex = 8;
            this.ckxCloseCap.Text = "Close the socket cap test";
            this.ckxCloseCap.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 120);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Test Socket:";
            // 
            // nmSocketID
            // 
            this.nmSocketID.Location = new System.Drawing.Point(108, 118);
            this.nmSocketID.Margin = new System.Windows.Forms.Padding(0);
            this.nmSocketID.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nmSocketID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmSocketID.Name = "nmSocketID";
            this.nmSocketID.Size = new System.Drawing.Size(140, 27);
            this.nmSocketID.TabIndex = 10;
            this.nmSocketID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // UCModeParams_DoeTakeOffTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nmSocketID);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ckxCloseCap);
            this.Controls.Add(this.nbTestTimes);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nbTestCol);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nbTestRow);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxTrayId);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "UCModeParams_DoeTakeOffTest";
            this.Size = new System.Drawing.Size(543, 167);
            ((System.ComponentModel.ISupportInitialize)(this.nbTestRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbTestCol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbTestTimes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmSocketID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxTrayId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nbTestRow;
        private System.Windows.Forms.NumericUpDown nbTestCol;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nbTestTimes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox ckxCloseCap;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nmSocketID;
    }
}
