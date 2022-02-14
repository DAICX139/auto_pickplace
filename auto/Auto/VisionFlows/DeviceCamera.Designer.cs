
namespace VisionFlows
{
    partial class DeviceCamera
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
            this.button_onece = new System.Windows.Forms.Button();
            this.button_continiu = new System.Windows.Forms.Button();
            this.trackBar_expor1 = new System.Windows.Forms.TrackBar();
            this.trackBar_gen1 = new System.Windows.Forms.TrackBar();
            this.numericUpDown_expor1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_gen1 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox_imagelist = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.superWind1 = new VisionControls.SuperWind();
            this.button_open = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_expor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_gen1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_expor1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_gen1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_onece
            // 
            this.button_onece.BackColor = System.Drawing.Color.Transparent;
            this.button_onece.Enabled = false;
            this.button_onece.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_onece.ForeColor = System.Drawing.Color.Black;
            this.button_onece.Location = new System.Drawing.Point(709, 357);
            this.button_onece.Name = "button_onece";
            this.button_onece.Size = new System.Drawing.Size(99, 23);
            this.button_onece.TabIndex = 227;
            this.button_onece.Text = "单张拍照";
            this.button_onece.UseVisualStyleBackColor = false;
            this.button_onece.Click += new System.EventHandler(this.button_onece_Click);
            // 
            // button_continiu
            // 
            this.button_continiu.BackColor = System.Drawing.Color.Transparent;
            this.button_continiu.Enabled = false;
            this.button_continiu.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_continiu.ForeColor = System.Drawing.Color.Black;
            this.button_continiu.Location = new System.Drawing.Point(553, 357);
            this.button_continiu.Name = "button_continiu";
            this.button_continiu.Size = new System.Drawing.Size(99, 23);
            this.button_continiu.TabIndex = 226;
            this.button_continiu.Text = "连续拍照";
            this.button_continiu.UseVisualStyleBackColor = false;
            this.button_continiu.Click += new System.EventHandler(this.button_continiu_Click);
            // 
            // trackBar_expor1
            // 
            this.trackBar_expor1.BackColor = System.Drawing.SystemColors.Control;
            this.trackBar_expor1.Enabled = false;
            this.trackBar_expor1.Location = new System.Drawing.Point(549, 133);
            this.trackBar_expor1.Maximum = 200000;
            this.trackBar_expor1.Minimum = 50;
            this.trackBar_expor1.Name = "trackBar_expor1";
            this.trackBar_expor1.Size = new System.Drawing.Size(185, 45);
            this.trackBar_expor1.TabIndex = 222;
            this.trackBar_expor1.Value = 200;
            this.trackBar_expor1.Scroll += new System.EventHandler(this.trackBar_expor1_Scroll);
            // 
            // trackBar_gen1
            // 
            this.trackBar_gen1.BackColor = System.Drawing.SystemColors.Control;
            this.trackBar_gen1.Enabled = false;
            this.trackBar_gen1.Location = new System.Drawing.Point(549, 249);
            this.trackBar_gen1.Maximum = 32;
            this.trackBar_gen1.Name = "trackBar_gen1";
            this.trackBar_gen1.Size = new System.Drawing.Size(185, 45);
            this.trackBar_gen1.TabIndex = 223;
            this.trackBar_gen1.Value = 1;
            this.trackBar_gen1.Scroll += new System.EventHandler(this.trackBar_gen1_Scroll);
            // 
            // numericUpDown_expor1
            // 
            this.numericUpDown_expor1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(243)))), ((int)(((byte)(245)))));
            this.numericUpDown_expor1.Enabled = false;
            this.numericUpDown_expor1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown_expor1.Location = new System.Drawing.Point(744, 144);
            this.numericUpDown_expor1.Maximum = new decimal(new int[] {
            200000,
            0,
            0,
            0});
            this.numericUpDown_expor1.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDown_expor1.Name = "numericUpDown_expor1";
            this.numericUpDown_expor1.Size = new System.Drawing.Size(78, 21);
            this.numericUpDown_expor1.TabIndex = 224;
            this.numericUpDown_expor1.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numericUpDown_expor1.ValueChanged += new System.EventHandler(this.numericUpDown_expor1_ValueChanged);
            // 
            // numericUpDown_gen1
            // 
            this.numericUpDown_gen1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(243)))), ((int)(((byte)(245)))));
            this.numericUpDown_gen1.Enabled = false;
            this.numericUpDown_gen1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDown_gen1.Location = new System.Drawing.Point(744, 260);
            this.numericUpDown_gen1.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericUpDown_gen1.Name = "numericUpDown_gen1";
            this.numericUpDown_gen1.Size = new System.Drawing.Size(78, 21);
            this.numericUpDown_gen1.TabIndex = 225;
            this.numericUpDown_gen1.ValueChanged += new System.EventHandler(this.numericUpDown_gen1_ValueChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(549, 228);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 221;
            this.label7.Text = "增益";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(550, 112);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 220;
            this.label6.Text = "曝光";
            // 
            // comboBox_imagelist
            // 
            this.comboBox_imagelist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_imagelist.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_imagelist.FormattingEnabled = true;
            this.comboBox_imagelist.Location = new System.Drawing.Point(553, 40);
            this.comboBox_imagelist.Name = "comboBox_imagelist";
            this.comboBox_imagelist.Size = new System.Drawing.Size(185, 20);
            this.comboBox_imagelist.TabIndex = 219;
            this.comboBox_imagelist.SelectedIndexChanged += new System.EventHandler(this.comboBox_imagelist_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(549, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 218;
            this.label1.Text = "相机";
            // 
            // superWind1
            // 
            this.superWind1.BackColor = System.Drawing.SystemColors.Control;
            this.superWind1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.superWind1.Location = new System.Drawing.Point(2, 5);
            this.superWind1.Margin = new System.Windows.Forms.Padding(1);
            this.superWind1.Name = "superWind1";
            this.superWind1.Size = new System.Drawing.Size(526, 410);
            this.superWind1.TabIndex = 228;
            // 
            // button_open
            // 
            this.button_open.BackColor = System.Drawing.Color.White;
            this.button_open.Enabled = false;
            this.button_open.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_open.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button_open.Location = new System.Drawing.Point(744, 40);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(78, 23);
            this.button_open.TabIndex = 229;
            this.button_open.Text = "连接";
            this.button_open.UseVisualStyleBackColor = false;
            this.button_open.Click += new System.EventHandler(this.button_open_Click);
            // 
            // DeviceCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 416);
            this.Controls.Add(this.button_open);
            this.Controls.Add(this.superWind1);
            this.Controls.Add(this.button_onece);
            this.Controls.Add(this.button_continiu);
            this.Controls.Add(this.trackBar_expor1);
            this.Controls.Add(this.trackBar_gen1);
            this.Controls.Add(this.numericUpDown_expor1);
            this.Controls.Add(this.numericUpDown_gen1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox_imagelist);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.Name = "DeviceCamera";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "相机设备";
            this.Load += new System.EventHandler(this.DeviceCamera_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_expor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_gen1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_expor1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_gen1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_onece;
        private System.Windows.Forms.Button button_continiu;
        public System.Windows.Forms.TrackBar trackBar_expor1;
        public System.Windows.Forms.TrackBar trackBar_gen1;
        private System.Windows.Forms.NumericUpDown numericUpDown_expor1;
        private System.Windows.Forms.NumericUpDown numericUpDown_gen1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox_imagelist;
        private System.Windows.Forms.Label label1;
        private VisionControls.SuperWind superWind1;
        private System.Windows.Forms.Button button_open;
    }
}