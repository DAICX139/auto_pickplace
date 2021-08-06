
namespace Poc2Auto.GUI.UCModeUI
{
    partial class FormOperationAxis
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
            this.btnYAxisJogSub = new System.Windows.Forms.Button();
            this.btnXAxisJogAdd = new System.Windows.Forms.Button();
            this.btnXAxisJogSub = new System.Windows.Forms.Button();
            this.btnYAxisJogAdd = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numAxisVel = new System.Windows.Forms.NumericUpDown();
            this.numAxisTarPos = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.btnAbsGo = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbxAxisName = new System.Windows.Forms.ComboBox();
            this.ckxAxisEnable = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnHome = new System.Windows.Forms.Button();
            this.lbAxisPos = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAxisVel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAxisTarPos)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnYAxisJogSub
            // 
            this.btnYAxisJogSub.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnYAxisJogSub.Location = new System.Drawing.Point(91, 177);
            this.btnYAxisJogSub.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnYAxisJogSub.Name = "btnYAxisJogSub";
            this.btnYAxisJogSub.Size = new System.Drawing.Size(70, 50);
            this.btnYAxisJogSub.TabIndex = 1;
            this.btnYAxisJogSub.Text = "v";
            this.btnYAxisJogSub.UseVisualStyleBackColor = true;
            this.btnYAxisJogSub.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnYAxisJogSub_MouseDown);
            this.btnYAxisJogSub.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnYAxisJogSub_MouseUp);
            // 
            // btnXAxisJogAdd
            // 
            this.btnXAxisJogAdd.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnXAxisJogAdd.Location = new System.Drawing.Point(167, 111);
            this.btnXAxisJogAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnXAxisJogAdd.Name = "btnXAxisJogAdd";
            this.btnXAxisJogAdd.Size = new System.Drawing.Size(70, 50);
            this.btnXAxisJogAdd.TabIndex = 2;
            this.btnXAxisJogAdd.Text = ">";
            this.btnXAxisJogAdd.UseVisualStyleBackColor = true;
            this.btnXAxisJogAdd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnXAxisJogAdd_MouseDown);
            this.btnXAxisJogAdd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnXAxisJogAdd_MouseUp);
            // 
            // btnXAxisJogSub
            // 
            this.btnXAxisJogSub.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnXAxisJogSub.Location = new System.Drawing.Point(14, 110);
            this.btnXAxisJogSub.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnXAxisJogSub.Name = "btnXAxisJogSub";
            this.btnXAxisJogSub.Size = new System.Drawing.Size(70, 50);
            this.btnXAxisJogSub.TabIndex = 3;
            this.btnXAxisJogSub.Text = "<";
            this.btnXAxisJogSub.UseVisualStyleBackColor = true;
            this.btnXAxisJogSub.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnXAxisJogSub_MouseDown);
            this.btnXAxisJogSub.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnXAxisJogSub_MouseUp);
            // 
            // btnYAxisJogAdd
            // 
            this.btnYAxisJogAdd.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnYAxisJogAdd.Location = new System.Drawing.Point(91, 43);
            this.btnYAxisJogAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnYAxisJogAdd.Name = "btnYAxisJogAdd";
            this.btnYAxisJogAdd.Size = new System.Drawing.Size(70, 50);
            this.btnYAxisJogAdd.TabIndex = 0;
            this.btnYAxisJogAdd.Text = "^";
            this.btnYAxisJogAdd.UseVisualStyleBackColor = true;
            this.btnYAxisJogAdd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnYAxisJogAdd_MouseDown);
            this.btnYAxisJogAdd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnYAxisJogAdd_MouseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDown);
            this.groupBox1.Controls.Add(this.btnUp);
            this.groupBox1.Controls.Add(this.btnYAxisJogAdd);
            this.groupBox1.Controls.Add(this.btnYAxisJogSub);
            this.groupBox1.Controls.Add(this.btnXAxisJogAdd);
            this.groupBox1.Controls.Add(this.btnXAxisJogSub);
            this.groupBox1.Location = new System.Drawing.Point(304, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(336, 263);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Shortcut operation";
            // 
            // btnDown
            // 
            this.btnDown.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDown.Location = new System.Drawing.Point(250, 177);
            this.btnDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(70, 50);
            this.btnDown.TabIndex = 5;
            this.btnDown.Text = "Down";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnDown_MouseDown);
            this.btnDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnDown_MouseUp);
            // 
            // btnUp
            // 
            this.btnUp.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnUp.Location = new System.Drawing.Point(250, 43);
            this.btnUp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(70, 50);
            this.btnUp.TabIndex = 4;
            this.btnUp.Text = "Up";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseDown);
            this.btnUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnUp_MouseUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Move velocity:";
            // 
            // numAxisVel
            // 
            this.numAxisVel.DecimalPlaces = 4;
            this.numAxisVel.Location = new System.Drawing.Point(107, 94);
            this.numAxisVel.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numAxisVel.Name = "numAxisVel";
            this.numAxisVel.Size = new System.Drawing.Size(135, 23);
            this.numAxisVel.TabIndex = 9;
            // 
            // numAxisTarPos
            // 
            this.numAxisTarPos.DecimalPlaces = 4;
            this.numAxisTarPos.Location = new System.Drawing.Point(107, 55);
            this.numAxisTarPos.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numAxisTarPos.Minimum = new decimal(new int[] {
            500,
            0,
            0,
            -2147483648});
            this.numAxisTarPos.Name = "numAxisTarPos";
            this.numAxisTarPos.Size = new System.Drawing.Size(135, 23);
            this.numAxisTarPos.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "Target position:";
            // 
            // btnAbsGo
            // 
            this.btnAbsGo.Location = new System.Drawing.Point(7, 22);
            this.btnAbsGo.Name = "btnAbsGo";
            this.btnAbsGo.Size = new System.Drawing.Size(57, 50);
            this.btnAbsGo.TabIndex = 12;
            this.btnAbsGo.Text = "ABSGo";
            this.btnAbsGo.UseVisualStyleBackColor = true;
            this.btnAbsGo.Click += new System.EventHandler(this.btnAbsGo_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 17);
            this.label4.TabIndex = 14;
            this.label4.Text = "Axis name:";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(70, 22);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(57, 50);
            this.btnStop.TabIndex = 15;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(133, 22);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(57, 50);
            this.btnReset.TabIndex = 16;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbxAxisName);
            this.groupBox2.Controls.Add(this.ckxAxisEnable);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.numAxisVel);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.numAxisTarPos);
            this.groupBox2.Location = new System.Drawing.Point(23, 54);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(261, 132);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Input";
            // 
            // cbxAxisName
            // 
            this.cbxAxisName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAxisName.FormattingEnabled = true;
            this.cbxAxisName.Location = new System.Drawing.Point(107, 19);
            this.cbxAxisName.Name = "cbxAxisName";
            this.cbxAxisName.Size = new System.Drawing.Size(76, 25);
            this.cbxAxisName.TabIndex = 21;
            this.cbxAxisName.SelectedIndexChanged += new System.EventHandler(this.cbxAxisName_SelectedIndexChanged);
            // 
            // ckxAxisEnable
            // 
            this.ckxAxisEnable.AutoSize = true;
            this.ckxAxisEnable.Location = new System.Drawing.Point(186, 21);
            this.ckxAxisEnable.Name = "ckxAxisEnable";
            this.ckxAxisEnable.Size = new System.Drawing.Size(66, 21);
            this.ckxAxisEnable.TabIndex = 20;
            this.ckxAxisEnable.Text = "Enable";
            this.ckxAxisEnable.UseVisualStyleBackColor = true;
            this.ckxAxisEnable.CheckedChanged += new System.EventHandler(this.ckxAxisEnable_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnHome);
            this.groupBox3.Controls.Add(this.btnAbsGo);
            this.groupBox3.Controls.Add(this.btnStop);
            this.groupBox3.Controls.Add(this.btnReset);
            this.groupBox3.Location = new System.Drawing.Point(23, 192);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(261, 83);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Control";
            // 
            // btnHome
            // 
            this.btnHome.Location = new System.Drawing.Point(197, 22);
            this.btnHome.Name = "btnHome";
            this.btnHome.Size = new System.Drawing.Size(57, 50);
            this.btnHome.TabIndex = 17;
            this.btnHome.Text = "Home";
            this.btnHome.UseVisualStyleBackColor = true;
            this.btnHome.Click += new System.EventHandler(this.btnHome_Click);
            // 
            // lbAxisPos
            // 
            this.lbAxisPos.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbAxisPos.Location = new System.Drawing.Point(23, 13);
            this.lbAxisPos.Name = "lbAxisPos";
            this.lbAxisPos.Size = new System.Drawing.Size(254, 37);
            this.lbAxisPos.TabIndex = 19;
            this.lbAxisPos.Text = "0.0000";
            this.lbAxisPos.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormOperationAxis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 283);
            this.Controls.Add(this.lbAxisPos);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormOperationAxis";
            this.Text = "Axis Operation";
            this.Load += new System.EventHandler(this.FormOperationAxis_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numAxisVel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAxisTarPos)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnYAxisJogAdd;
        private System.Windows.Forms.Button btnYAxisJogSub;
        private System.Windows.Forms.Button btnXAxisJogAdd;
        private System.Windows.Forms.Button btnXAxisJogSub;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numAxisVel;
        private System.Windows.Forms.NumericUpDown numAxisTarPos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAbsGo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lbAxisPos;
        private System.Windows.Forms.CheckBox ckxAxisEnable;
        private System.Windows.Forms.ComboBox cbxAxisName;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnHome;
    }
}