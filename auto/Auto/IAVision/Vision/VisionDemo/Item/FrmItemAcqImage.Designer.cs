namespace VisionDemo
{
    partial class FrmItemAcqImage
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
            this.tbcDevice = new System.Windows.Forms.TabControl();
            this.tbpFile = new System.Windows.Forms.TabPage();
            this.btnBrowser = new System.Windows.Forms.Button();
            this.txtPath = new System.Windows.Forms.TextBox();
            this.tbpCamera = new System.Windows.Forms.TabPage();
            this.chkReverseY = new System.Windows.Forms.CheckBox();
            this.chkReverseX = new System.Windows.Forms.CheckBox();
            this.cmbCameraUserID = new System.Windows.Forms.ComboBox();
            this.txtGain = new System.Windows.Forms.TextBox();
            this.txtExposureTime = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.rdbFolder = new System.Windows.Forms.RadioButton();
            this.label32 = new System.Windows.Forms.Label();
            this.rdbCamera = new System.Windows.Forms.RadioButton();
            this.rdbFile = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.pnlFootBase.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tbcDevice.SuspendLayout();
            this.tbpFile.SuspendLayout();
            this.tbpCamera.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlImage
            // 
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImage.Location = new System.Drawing.Point(0, 0);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(544, 460);
            this.pnlImage.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabControl1.Location = new System.Drawing.Point(544, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(256, 460);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tbcDevice);
            this.tabPage1.Controls.Add(this.rdbFolder);
            this.tabPage1.Controls.Add(this.label32);
            this.tabPage1.Controls.Add(this.rdbCamera);
            this.tabPage1.Controls.Add(this.rdbFile);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(248, 434);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本参数";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tbcDevice
            // 
            this.tbcDevice.Controls.Add(this.tbpFile);
            this.tbcDevice.Controls.Add(this.tbpCamera);
            this.tbcDevice.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbcDevice.Location = new System.Drawing.Point(3, 82);
            this.tbcDevice.Name = "tbcDevice";
            this.tbcDevice.SelectedIndex = 0;
            this.tbcDevice.Size = new System.Drawing.Size(242, 349);
            this.tbcDevice.TabIndex = 84;
            // 
            // tbpFile
            // 
            this.tbpFile.Controls.Add(this.btnBrowser);
            this.tbpFile.Controls.Add(this.txtPath);
            this.tbpFile.Location = new System.Drawing.Point(4, 22);
            this.tbpFile.Name = "tbpFile";
            this.tbpFile.Padding = new System.Windows.Forms.Padding(3);
            this.tbpFile.Size = new System.Drawing.Size(234, 323);
            this.tbpFile.TabIndex = 0;
            this.tbpFile.Text = "指定图像";
            this.tbpFile.UseVisualStyleBackColor = true;
            // 
            // btnBrowser
            // 
            this.btnBrowser.Location = new System.Drawing.Point(154, 152);
            this.btnBrowser.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowser.Name = "btnBrowser";
            this.btnBrowser.Size = new System.Drawing.Size(75, 23);
            this.btnBrowser.TabIndex = 3;
            this.btnBrowser.Text = "浏览";
            this.btnBrowser.UseVisualStyleBackColor = true;
            this.btnBrowser.Click += new System.EventHandler(this.btnBrowser_Click);
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(4, 102);
            this.txtPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtPath.Multiline = true;
            this.txtPath.Name = "txtPath";
            this.txtPath.Size = new System.Drawing.Size(225, 46);
            this.txtPath.TabIndex = 2;
            // 
            // tbpCamera
            // 
            this.tbpCamera.Controls.Add(this.chkReverseY);
            this.tbpCamera.Controls.Add(this.chkReverseX);
            this.tbpCamera.Controls.Add(this.cmbCameraUserID);
            this.tbpCamera.Controls.Add(this.txtGain);
            this.tbpCamera.Controls.Add(this.txtExposureTime);
            this.tbpCamera.Controls.Add(this.label7);
            this.tbpCamera.Controls.Add(this.label6);
            this.tbpCamera.Controls.Add(this.label4);
            this.tbpCamera.Location = new System.Drawing.Point(4, 22);
            this.tbpCamera.Name = "tbpCamera";
            this.tbpCamera.Padding = new System.Windows.Forms.Padding(3);
            this.tbpCamera.Size = new System.Drawing.Size(234, 323);
            this.tbpCamera.TabIndex = 2;
            this.tbpCamera.Text = "相机";
            this.tbpCamera.UseVisualStyleBackColor = true;
            // 
            // chkReverseY
            // 
            this.chkReverseY.AutoSize = true;
            this.chkReverseY.Location = new System.Drawing.Point(63, 185);
            this.chkReverseY.Name = "chkReverseY";
            this.chkReverseY.Size = new System.Drawing.Size(72, 16);
            this.chkReverseY.TabIndex = 105;
            this.chkReverseY.Text = "垂直镜像";
            this.chkReverseY.UseVisualStyleBackColor = true;
            // 
            // chkReverseX
            // 
            this.chkReverseX.AutoSize = true;
            this.chkReverseX.Location = new System.Drawing.Point(63, 147);
            this.chkReverseX.Name = "chkReverseX";
            this.chkReverseX.Size = new System.Drawing.Size(72, 16);
            this.chkReverseX.TabIndex = 104;
            this.chkReverseX.Text = "水平镜像";
            this.chkReverseX.UseVisualStyleBackColor = true;
            // 
            // cmbCameraUserID
            // 
            this.cmbCameraUserID.FormattingEnabled = true;
            this.cmbCameraUserID.Location = new System.Drawing.Point(63, 17);
            this.cmbCameraUserID.Name = "cmbCameraUserID";
            this.cmbCameraUserID.Size = new System.Drawing.Size(160, 20);
            this.cmbCameraUserID.TabIndex = 103;
            this.cmbCameraUserID.SelectedIndexChanged += new System.EventHandler(this.cmbCameraUserID_SelectedIndexChanged);
            // 
            // txtGain
            // 
            this.txtGain.Location = new System.Drawing.Point(63, 98);
            this.txtGain.Name = "txtGain";
            this.txtGain.Size = new System.Drawing.Size(160, 21);
            this.txtGain.TabIndex = 101;
            this.txtGain.Text = "0";
            // 
            // txtExposureTime
            // 
            this.txtExposureTime.Location = new System.Drawing.Point(63, 58);
            this.txtExposureTime.Name = "txtExposureTime";
            this.txtExposureTime.Size = new System.Drawing.Size(160, 21);
            this.txtExposureTime.TabIndex = 100;
            this.txtExposureTime.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 101);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 99;
            this.label7.Text = "增益";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 98;
            this.label6.Text = "曝光时间";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 95;
            this.label4.Text = "相机选择";
            // 
            // rdbFolder
            // 
            this.rdbFolder.AutoSize = true;
            this.rdbFolder.Location = new System.Drawing.Point(85, 42);
            this.rdbFolder.Margin = new System.Windows.Forms.Padding(2);
            this.rdbFolder.Name = "rdbFolder";
            this.rdbFolder.Size = new System.Drawing.Size(71, 16);
            this.rdbFolder.TabIndex = 83;
            this.rdbFolder.Text = "文件目录";
            this.rdbFolder.UseVisualStyleBackColor = true;
            this.rdbFolder.CheckedChanged += new System.EventHandler(this.rdbFile_CheckedChanged);
            // 
            // label32
            // 
            this.label32.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label32.Dock = System.Windows.Forms.DockStyle.Top;
            this.label32.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label32.ForeColor = System.Drawing.Color.White;
            this.label32.Location = new System.Drawing.Point(3, 3);
            this.label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(242, 20);
            this.label32.TabIndex = 82;
            this.label32.Text = "采集模式";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // rdbCamera
            // 
            this.rdbCamera.AutoSize = true;
            this.rdbCamera.Location = new System.Drawing.Point(160, 42);
            this.rdbCamera.Margin = new System.Windows.Forms.Padding(2);
            this.rdbCamera.Name = "rdbCamera";
            this.rdbCamera.Size = new System.Drawing.Size(47, 16);
            this.rdbCamera.TabIndex = 6;
            this.rdbCamera.Text = "相机";
            this.rdbCamera.UseVisualStyleBackColor = true;
            this.rdbCamera.CheckedChanged += new System.EventHandler(this.rdbFile_CheckedChanged);
            // 
            // rdbFile
            // 
            this.rdbFile.AutoSize = true;
            this.rdbFile.Checked = true;
            this.rdbFile.Location = new System.Drawing.Point(10, 42);
            this.rdbFile.Margin = new System.Windows.Forms.Padding(2);
            this.rdbFile.Name = "rdbFile";
            this.rdbFile.Size = new System.Drawing.Size(71, 16);
            this.rdbFile.TabIndex = 5;
            this.rdbFile.TabStop = true;
            this.rdbFile.Text = "指定图像";
            this.rdbFile.UseVisualStyleBackColor = true;
            this.rdbFile.CheckedChanged += new System.EventHandler(this.rdbFile_CheckedChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.comboBox2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.checkBox4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(248, 434);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "显示设置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(242, 20);
            this.label2.TabIndex = 108;
            this.label2.Text = "显示设置";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(67, 39);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(168, 20);
            this.comboBox2.TabIndex = 107;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 106;
            this.label1.Text = "显示窗体";
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(67, 74);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(120, 16);
            this.checkBox4.TabIndex = 105;
            this.checkBox4.Text = "自动显示结果图像";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // FrmItemAcqImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 508);
            this.Controls.Add(this.pnlImage);
            this.Controls.Add(this.tabControl1);
            this.Name = "FrmItemAcqImage";
            this.Text = "采集图像";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmItemAcqImage_FormClosing);
            this.Load += new System.EventHandler(this.FrmItemAcqImage_Load);
            this.Controls.SetChildIndex(this.pnlFootBase, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.pnlImage, 0);
            this.pnlFootBase.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tbcDevice.ResumeLayout(false);
            this.tbpFile.ResumeLayout(false);
            this.tbpFile.PerformLayout();
            this.tbpCamera.ResumeLayout(false);
            this.tbpCamera.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlImage;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.TabControl tbcDevice;
        private System.Windows.Forms.TabPage tbpFile;
        private System.Windows.Forms.Button btnBrowser;
        private System.Windows.Forms.TextBox txtPath;
        private System.Windows.Forms.TabPage tbpCamera;
        private System.Windows.Forms.CheckBox chkReverseY;
        private System.Windows.Forms.CheckBox chkReverseX;
        private System.Windows.Forms.ComboBox cmbCameraUserID;
        private System.Windows.Forms.TextBox txtGain;
        private System.Windows.Forms.TextBox txtExposureTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rdbFolder;
        private System.Windows.Forms.RadioButton rdbCamera;
        private System.Windows.Forms.RadioButton rdbFile;
        private System.Windows.Forms.Label label2;
    }
}