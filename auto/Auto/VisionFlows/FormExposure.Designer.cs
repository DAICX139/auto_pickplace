
namespace VisionFlows
{
    partial class FormExposure
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
            this.label1 = new System.Windows.Forms.Label();
            this.button_Save = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtExposure_LeftCamShotTray = new System.Windows.Forms.TextBox();
            this.txtExposure_DownCamScan = new System.Windows.Forms.TextBox();
            this.txtExposure_LeftCamPutSocket = new System.Windows.Forms.TextBox();
            this.txtExposure_CheckSocket = new System.Windows.Forms.TextBox();
            this.txtExposure_RightCamCheckTray = new System.Windows.Forms.TextBox();
            this.txtExposure_RightCamPutTray = new System.Windows.Forms.TextBox();
            this.txtExposure_RightCamCheckSocket = new System.Windows.Forms.TextBox();
            this.txtExposure_RightCamGetSocket = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button_Quit = new System.Windows.Forms.Button();
            this.txtExposure_LeftCamPutTray = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtExposure_LeftCamCheckTray = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(49, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "左相机Tray取料";
            // 
            // button_Save
            // 
            this.button_Save.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Save.Location = new System.Drawing.Point(423, 288);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(272, 34);
            this.button_Save.TabIndex = 1;
            this.button_Save.Text = "保存";
            this.button_Save.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(49, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "下相机扫码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(49, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "左相机Socket放料";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(49, 244);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 17);
            this.label4.TabIndex = 4;
            this.label4.Text = "左相机Socket检查";
            // 
            // txtExposure_LeftCamShotTray
            // 
            this.txtExposure_LeftCamShotTray.Location = new System.Drawing.Point(183, 56);
            this.txtExposure_LeftCamShotTray.Name = "txtExposure_LeftCamShotTray";
            this.txtExposure_LeftCamShotTray.Size = new System.Drawing.Size(141, 21);
            this.txtExposure_LeftCamShotTray.TabIndex = 5;
            this.txtExposure_LeftCamShotTray.Text = "7000";
            this.txtExposure_LeftCamShotTray.TextChanged += new System.EventHandler(this.txtExposure_LeftCamShotTray_TextChanged);
            // 
            // txtExposure_DownCamScan
            // 
            this.txtExposure_DownCamScan.Location = new System.Drawing.Point(183, 119);
            this.txtExposure_DownCamScan.Name = "txtExposure_DownCamScan";
            this.txtExposure_DownCamScan.Size = new System.Drawing.Size(141, 21);
            this.txtExposure_DownCamScan.TabIndex = 6;
            this.txtExposure_DownCamScan.Text = "18000";
            this.txtExposure_DownCamScan.TextChanged += new System.EventHandler(this.txtExposure_DownCamScan_TextChanged);
            // 
            // txtExposure_LeftCamPutSocket
            // 
            this.txtExposure_LeftCamPutSocket.Location = new System.Drawing.Point(183, 183);
            this.txtExposure_LeftCamPutSocket.Name = "txtExposure_LeftCamPutSocket";
            this.txtExposure_LeftCamPutSocket.Size = new System.Drawing.Size(141, 21);
            this.txtExposure_LeftCamPutSocket.TabIndex = 7;
            this.txtExposure_LeftCamPutSocket.Text = "7000";
            this.txtExposure_LeftCamPutSocket.TextChanged += new System.EventHandler(this.txtExposure_LeftCamPutSocket_TextChanged);
            // 
            // txtExposure_CheckSocket
            // 
            this.txtExposure_CheckSocket.Location = new System.Drawing.Point(183, 240);
            this.txtExposure_CheckSocket.Name = "txtExposure_CheckSocket";
            this.txtExposure_CheckSocket.Size = new System.Drawing.Size(141, 21);
            this.txtExposure_CheckSocket.TabIndex = 8;
            this.txtExposure_CheckSocket.Text = "7000";
            this.txtExposure_CheckSocket.TextChanged += new System.EventHandler(this.txtExposure_CheckSocket_TextChanged);
            // 
            // txtExposure_RightCamCheckTray
            // 
            this.txtExposure_RightCamCheckTray.Location = new System.Drawing.Point(554, 240);
            this.txtExposure_RightCamCheckTray.Name = "txtExposure_RightCamCheckTray";
            this.txtExposure_RightCamCheckTray.Size = new System.Drawing.Size(141, 21);
            this.txtExposure_RightCamCheckTray.TabIndex = 16;
            this.txtExposure_RightCamCheckTray.Text = "7000";
            this.txtExposure_RightCamCheckTray.TextChanged += new System.EventHandler(this.txtExposure_RightCamCheckTray_TextChanged);
            // 
            // txtExposure_RightCamPutTray
            // 
            this.txtExposure_RightCamPutTray.Location = new System.Drawing.Point(554, 183);
            this.txtExposure_RightCamPutTray.Name = "txtExposure_RightCamPutTray";
            this.txtExposure_RightCamPutTray.Size = new System.Drawing.Size(141, 21);
            this.txtExposure_RightCamPutTray.TabIndex = 15;
            this.txtExposure_RightCamPutTray.Text = "7000";
            this.txtExposure_RightCamPutTray.TextChanged += new System.EventHandler(this.txtExposure_RightCamPutTray_TextChanged);
            // 
            // txtExposure_RightCamCheckSocket
            // 
            this.txtExposure_RightCamCheckSocket.Location = new System.Drawing.Point(554, 119);
            this.txtExposure_RightCamCheckSocket.Name = "txtExposure_RightCamCheckSocket";
            this.txtExposure_RightCamCheckSocket.Size = new System.Drawing.Size(141, 21);
            this.txtExposure_RightCamCheckSocket.TabIndex = 14;
            this.txtExposure_RightCamCheckSocket.Text = "7000";
            this.txtExposure_RightCamCheckSocket.TextChanged += new System.EventHandler(this.txtExposure_RightCamCheckSocket_TextChanged);
            // 
            // txtExposure_RightCamGetSocket
            // 
            this.txtExposure_RightCamGetSocket.Location = new System.Drawing.Point(554, 56);
            this.txtExposure_RightCamGetSocket.Name = "txtExposure_RightCamGetSocket";
            this.txtExposure_RightCamGetSocket.Size = new System.Drawing.Size(141, 21);
            this.txtExposure_RightCamGetSocket.TabIndex = 13;
            this.txtExposure_RightCamGetSocket.Text = "7000";
            this.txtExposure_RightCamGetSocket.TextChanged += new System.EventHandler(this.txtExposure_RightCamGetSocket_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(420, 244);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "右相机Tray检查";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(420, 183);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "右相机Tray放料";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(420, 119);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 17);
            this.label7.TabIndex = 10;
            this.label7.Text = "右相机Socket检查";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(420, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 17);
            this.label8.TabIndex = 9;
            this.label8.Text = "右相机Socket取料";
            // 
            // button_Quit
            // 
            this.button_Quit.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Quit.Location = new System.Drawing.Point(423, 345);
            this.button_Quit.Name = "button_Quit";
            this.button_Quit.Size = new System.Drawing.Size(272, 34);
            this.button_Quit.TabIndex = 17;
            this.button_Quit.Text = "退出";
            this.button_Quit.UseVisualStyleBackColor = true;
            this.button_Quit.Click += new System.EventHandler(this.button_Quit_Click);
            // 
            // txtExposure_LeftCamPutTray
            // 
            this.txtExposure_LeftCamPutTray.Location = new System.Drawing.Point(183, 295);
            this.txtExposure_LeftCamPutTray.Name = "txtExposure_LeftCamPutTray";
            this.txtExposure_LeftCamPutTray.Size = new System.Drawing.Size(141, 21);
            this.txtExposure_LeftCamPutTray.TabIndex = 19;
            this.txtExposure_LeftCamPutTray.Text = "7000";
            this.txtExposure_LeftCamPutTray.TextChanged += new System.EventHandler(this.txtExposure_LeftCamPutTray_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(49, 299);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 17);
            this.label9.TabIndex = 18;
            this.label9.Text = "左相机Tray放料";
            // 
            // txtExposure_LeftCamCheckTray
            // 
            this.txtExposure_LeftCamCheckTray.Location = new System.Drawing.Point(183, 352);
            this.txtExposure_LeftCamCheckTray.Name = "txtExposure_LeftCamCheckTray";
            this.txtExposure_LeftCamCheckTray.Size = new System.Drawing.Size(141, 21);
            this.txtExposure_LeftCamCheckTray.TabIndex = 21;
            this.txtExposure_LeftCamCheckTray.Text = "7000";
            this.txtExposure_LeftCamCheckTray.TextChanged += new System.EventHandler(this.txtExposure_LeftCamCheckTray_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(49, 356);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(93, 17);
            this.label10.TabIndex = 20;
            this.label10.Text = "左相机Tray检查";
            // 
            // FormExposure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(762, 434);
            this.Controls.Add(this.txtExposure_LeftCamCheckTray);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtExposure_LeftCamPutTray);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button_Quit);
            this.Controls.Add(this.txtExposure_RightCamCheckTray);
            this.Controls.Add(this.txtExposure_RightCamPutTray);
            this.Controls.Add(this.txtExposure_RightCamCheckSocket);
            this.Controls.Add(this.txtExposure_RightCamGetSocket);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtExposure_CheckSocket);
            this.Controls.Add(this.txtExposure_LeftCamPutSocket);
            this.Controls.Add(this.txtExposure_DownCamScan);
            this.Controls.Add(this.txtExposure_LeftCamShotTray);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.label1);
            this.Name = "FormExposure";
            this.Text = "FormExposure";
            this.Load += new System.EventHandler(this.FormExposure_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtExposure_LeftCamShotTray;
        private System.Windows.Forms.TextBox txtExposure_DownCamScan;
        private System.Windows.Forms.TextBox txtExposure_LeftCamPutSocket;
        private System.Windows.Forms.TextBox txtExposure_CheckSocket;
        private System.Windows.Forms.TextBox txtExposure_RightCamCheckTray;
        private System.Windows.Forms.TextBox txtExposure_RightCamPutTray;
        private System.Windows.Forms.TextBox txtExposure_RightCamCheckSocket;
        private System.Windows.Forms.TextBox txtExposure_RightCamGetSocket;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button_Quit;
        private System.Windows.Forms.TextBox txtExposure_LeftCamPutTray;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtExposure_LeftCamCheckTray;
        private System.Windows.Forms.Label label10;
    }
}