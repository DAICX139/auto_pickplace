namespace VisionDemo
{
    partial class FrmAcqDevice
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dgvDeviceList = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbTriggerMode = new System.Windows.Forms.ComboBox();
            this.btnAcqImage = new System.Windows.Forms.Button();
            this.btnDisConnect = new System.Windows.Forms.Button();
            this.txtGain = new System.Windows.Forms.TextBox();
            this.txtExposureTime = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCurrDevice = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAddToDeviceList = new System.Windows.Forms.Button();
            this.cmbDeviceName = new System.Windows.Forms.ComboBox();
            this.cmbDeviceType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dgvSdk = new System.Windows.Forms.DataGridView();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.btnReset = new System.Windows.Forms.Button();
            this.pnlFootBase.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeviceList)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSdk)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFootBase
            // 
            this.pnlFootBase.Location = new System.Drawing.Point(0, 467);
            this.pnlFootBase.Size = new System.Drawing.Size(800, 41);
            // 
            // btnExecute
            // 
            this.btnExecute.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabControl1.Location = new System.Drawing.Point(447, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(353, 467);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(345, 441);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "设备信息";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dgvDeviceList);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 103);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(339, 194);
            this.panel3.TabIndex = 2;
            // 
            // dgvDeviceList
            // 
            this.dgvDeviceList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeviceList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDeviceList.Location = new System.Drawing.Point(0, 20);
            this.dgvDeviceList.Name = "dgvDeviceList";
            this.dgvDeviceList.RowTemplate.Height = 23;
            this.dgvDeviceList.Size = new System.Drawing.Size(339, 174);
            this.dgvDeviceList.TabIndex = 83;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(339, 20);
            this.label1.TabIndex = 82;
            this.label1.Text = "设备列表";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnReset);
            this.panel2.Controls.Add(this.cmbTriggerMode);
            this.panel2.Controls.Add(this.btnAcqImage);
            this.panel2.Controls.Add(this.btnDisConnect);
            this.panel2.Controls.Add(this.txtGain);
            this.panel2.Controls.Add(this.txtExposureTime);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.txtCurrDevice);
            this.panel2.Controls.Add(this.btnConnect);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 297);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(339, 141);
            this.panel2.TabIndex = 1;
            // 
            // cmbTriggerMode
            // 
            this.cmbTriggerMode.FormattingEnabled = true;
            this.cmbTriggerMode.Location = new System.Drawing.Point(234, 20);
            this.cmbTriggerMode.Name = "cmbTriggerMode";
            this.cmbTriggerMode.Size = new System.Drawing.Size(100, 20);
            this.cmbTriggerMode.TabIndex = 94;
            this.cmbTriggerMode.SelectedIndexChanged += new System.EventHandler(this.cmbTriggerMode_SelectedIndexChanged);
            // 
            // btnAcqImage
            // 
            this.btnAcqImage.Location = new System.Drawing.Point(249, 106);
            this.btnAcqImage.Name = "btnAcqImage";
            this.btnAcqImage.Size = new System.Drawing.Size(75, 23);
            this.btnAcqImage.TabIndex = 93;
            this.btnAcqImage.Text = "采集图像";
            this.btnAcqImage.UseVisualStyleBackColor = true;
            this.btnAcqImage.Click += new System.EventHandler(this.btnAcqImage_Click);
            // 
            // btnDisConnect
            // 
            this.btnDisConnect.Location = new System.Drawing.Point(87, 106);
            this.btnDisConnect.Name = "btnDisConnect";
            this.btnDisConnect.Size = new System.Drawing.Size(75, 23);
            this.btnDisConnect.TabIndex = 92;
            this.btnDisConnect.Text = "断开";
            this.btnDisConnect.UseVisualStyleBackColor = true;
            this.btnDisConnect.Click += new System.EventHandler(this.btnDisConnect_Click);
            // 
            // txtGain
            // 
            this.txtGain.Location = new System.Drawing.Point(234, 60);
            this.txtGain.Name = "txtGain";
            this.txtGain.Size = new System.Drawing.Size(100, 21);
            this.txtGain.TabIndex = 91;
            this.txtGain.Text = "0";
            // 
            // txtExposureTime
            // 
            this.txtExposureTime.Location = new System.Drawing.Point(62, 60);
            this.txtExposureTime.Name = "txtExposureTime";
            this.txtExposureTime.Size = new System.Drawing.Size(100, 21);
            this.txtExposureTime.TabIndex = 90;
            this.txtExposureTime.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(175, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 89;
            this.label7.Text = "增益";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 88;
            this.label6.Text = "曝光时间";
            // 
            // txtCurrDevice
            // 
            this.txtCurrDevice.Location = new System.Drawing.Point(62, 20);
            this.txtCurrDevice.Name = "txtCurrDevice";
            this.txtCurrDevice.Size = new System.Drawing.Size(100, 21);
            this.txtCurrDevice.TabIndex = 86;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(8, 106);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 23);
            this.btnConnect.TabIndex = 85;
            this.btnConnect.Text = "连接";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(175, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 84;
            this.label5.Text = "触发模式";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 83;
            this.label4.Text = "当前设备";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAddToDeviceList);
            this.panel1.Controls.Add(this.cmbDeviceName);
            this.panel1.Controls.Add(this.cmbDeviceType);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label32);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(339, 100);
            this.panel1.TabIndex = 0;
            // 
            // btnAddToDeviceList
            // 
            this.btnAddToDeviceList.Location = new System.Drawing.Point(258, 61);
            this.btnAddToDeviceList.Name = "btnAddToDeviceList";
            this.btnAddToDeviceList.Size = new System.Drawing.Size(75, 23);
            this.btnAddToDeviceList.TabIndex = 86;
            this.btnAddToDeviceList.Text = "添加";
            this.btnAddToDeviceList.UseVisualStyleBackColor = true;
            this.btnAddToDeviceList.Click += new System.EventHandler(this.btnAddToDeviceList_Click);
            // 
            // cmbDeviceName
            // 
            this.cmbDeviceName.FormattingEnabled = true;
            this.cmbDeviceName.Location = new System.Drawing.Point(65, 62);
            this.cmbDeviceName.Name = "cmbDeviceName";
            this.cmbDeviceName.Size = new System.Drawing.Size(163, 20);
            this.cmbDeviceName.TabIndex = 85;
            this.cmbDeviceName.SelectedIndexChanged += new System.EventHandler(this.cmbDeviceName_SelectedIndexChanged);
            // 
            // cmbDeviceType
            // 
            this.cmbDeviceType.FormattingEnabled = true;
            this.cmbDeviceType.Location = new System.Drawing.Point(65, 29);
            this.cmbDeviceType.Name = "cmbDeviceType";
            this.cmbDeviceType.Size = new System.Drawing.Size(163, 20);
            this.cmbDeviceType.TabIndex = 84;
            this.cmbDeviceType.SelectedIndexChanged += new System.EventHandler(this.cmbDeviceType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 83;
            this.label3.Text = "设备名称";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 82;
            this.label2.Text = "设备型号";
            // 
            // label32
            // 
            this.label32.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.label32.Dock = System.Windows.Forms.DockStyle.Top;
            this.label32.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label32.ForeColor = System.Drawing.Color.White;
            this.label32.Location = new System.Drawing.Point(0, 0);
            this.label32.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(339, 20);
            this.label32.TabIndex = 81;
            this.label32.Text = "设备选择";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dgvSdk);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(345, 441);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SDK版本";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dgvSdk
            // 
            this.dgvSdk.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSdk.Location = new System.Drawing.Point(30, 149);
            this.dgvSdk.Name = "dgvSdk";
            this.dgvSdk.RowTemplate.Height = 23;
            this.dgvSdk.Size = new System.Drawing.Size(240, 150);
            this.dgvSdk.TabIndex = 0;
            // 
            // pnlImage
            // 
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlImage.Location = new System.Drawing.Point(0, 0);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(447, 467);
            this.pnlImage.TabIndex = 1;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(168, 106);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 95;
            this.btnReset.Text = "复位";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // FrmAcqDevice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 508);
            this.Controls.Add(this.pnlImage);
            this.Controls.Add(this.tabControl1);
            this.Name = "FrmAcqDevice";
            this.Text = "相机配置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAcqDevice_FormClosing);
            this.Load += new System.EventHandler(this.FrmAcqDevice_Load);
            this.Controls.SetChildIndex(this.pnlFootBase, 0);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.pnlImage, 0);
            this.pnlFootBase.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeviceList)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSdk)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel pnlImage;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.ComboBox cmbDeviceType;
        private System.Windows.Forms.ComboBox cmbDeviceName;
        private System.Windows.Forms.Button btnAddToDeviceList;
        private System.Windows.Forms.TextBox txtGain;
        private System.Windows.Forms.TextBox txtExposureTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCurrDevice;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvDeviceList;
        private System.Windows.Forms.ComboBox cmbTriggerMode;
        private System.Windows.Forms.Button btnAcqImage;
        private System.Windows.Forms.Button btnDisConnect;
        private System.Windows.Forms.DataGridView dgvSdk;
        private System.Windows.Forms.Button btnReset;
    }
}