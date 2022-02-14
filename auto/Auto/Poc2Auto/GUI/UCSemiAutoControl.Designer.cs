
namespace Poc2Auto.GUI
{
    partial class UCSemiAutoControl
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
            this.btnTmReset = new System.Windows.Forms.Button();
            this.btnTesterReset = new System.Windows.Forms.Button();
            this.btnSelectDut = new System.Windows.Forms.Button();
            this.btnRotateOneStation = new System.Windows.Forms.Button();
            this.btnSocketClose = new System.Windows.Forms.Button();
            this.btnSocketOpenCap = new System.Windows.Forms.Button();
            this.btnNozzle1TrayUload = new System.Windows.Forms.Button();
            this.btnNozzle2TrayUload = new System.Windows.Forms.Button();
            this.Nozzle2SocketPickDut = new System.Windows.Forms.Button();
            this.btnNozzle1SocketPutDut = new System.Windows.Forms.Button();
            this.btnNozzle1Load = new System.Windows.Forms.Button();
            this.btnNozzle1Scan = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnTesterCylinderMoveToWork = new System.Windows.Forms.Button();
            this.btnTesterCylinderMoveToBase = new System.Windows.Forms.Button();
            this.btnLightControl = new System.Windows.Forms.Button();
            this.btnMaintenanceLamp = new System.Windows.Forms.Button();
            this.btnIonFan = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTmReset
            // 
            this.btnTmReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTmReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTmReset.Location = new System.Drawing.Point(416, 39);
            this.btnTmReset.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnTmReset.Name = "btnTmReset";
            this.btnTmReset.Size = new System.Drawing.Size(79, 39);
            this.btnTmReset.TabIndex = 11;
            this.btnTmReset.Text = "TM Reset";
            this.btnTmReset.UseVisualStyleBackColor = true;
            this.btnTmReset.Click += new System.EventHandler(this.btnTmReset_Click);
            // 
            // btnTesterReset
            // 
            this.btnTesterReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTesterReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTesterReset.Location = new System.Drawing.Point(416, 0);
            this.btnTesterReset.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnTesterReset.Name = "btnTesterReset";
            this.btnTesterReset.Size = new System.Drawing.Size(79, 39);
            this.btnTesterReset.TabIndex = 6;
            this.btnTesterReset.Text = "转盘复位";
            this.btnTesterReset.UseVisualStyleBackColor = true;
            this.btnTesterReset.Click += new System.EventHandler(this.btnTesterReset_Click);
            // 
            // btnSelectDut
            // 
            this.btnSelectDut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectDut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelectDut.Location = new System.Drawing.Point(317, 0);
            this.btnSelectDut.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnSelectDut.Name = "btnSelectDut";
            this.btnSelectDut.Size = new System.Drawing.Size(79, 39);
            this.btnSelectDut.TabIndex = 10;
            this.btnSelectDut.Text = "挑料-扫码";
            this.btnSelectDut.UseVisualStyleBackColor = true;
            this.btnSelectDut.Click += new System.EventHandler(this.btnSelectDut_Click);
            // 
            // btnRotateOneStation
            // 
            this.btnRotateOneStation.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRotateOneStation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRotateOneStation.Location = new System.Drawing.Point(218, 78);
            this.btnRotateOneStation.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnRotateOneStation.Name = "btnRotateOneStation";
            this.btnRotateOneStation.Size = new System.Drawing.Size(79, 41);
            this.btnRotateOneStation.TabIndex = 9;
            this.btnRotateOneStation.Text = "转盘旋转";
            this.btnRotateOneStation.UseVisualStyleBackColor = true;
            this.btnRotateOneStation.Click += new System.EventHandler(this.btnRotateOneStation_Click);
            // 
            // btnSocketClose
            // 
            this.btnSocketClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSocketClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSocketClose.Location = new System.Drawing.Point(218, 39);
            this.btnSocketClose.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnSocketClose.Name = "btnSocketClose";
            this.btnSocketClose.Size = new System.Drawing.Size(79, 39);
            this.btnSocketClose.TabIndex = 8;
            this.btnSocketClose.Text = "Socket关盖";
            this.btnSocketClose.UseVisualStyleBackColor = true;
            this.btnSocketClose.Click += new System.EventHandler(this.btnSocketClose_Click);
            // 
            // btnSocketOpenCap
            // 
            this.btnSocketOpenCap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSocketOpenCap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSocketOpenCap.Location = new System.Drawing.Point(218, 0);
            this.btnSocketOpenCap.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnSocketOpenCap.Name = "btnSocketOpenCap";
            this.btnSocketOpenCap.Size = new System.Drawing.Size(79, 39);
            this.btnSocketOpenCap.TabIndex = 7;
            this.btnSocketOpenCap.Text = "Socket开盖";
            this.btnSocketOpenCap.UseVisualStyleBackColor = true;
            this.btnSocketOpenCap.Click += new System.EventHandler(this.btnSocketOpenCap_Click);
            // 
            // btnNozzle1TrayUload
            // 
            this.btnNozzle1TrayUload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNozzle1TrayUload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNozzle1TrayUload.Location = new System.Drawing.Point(119, 78);
            this.btnNozzle1TrayUload.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnNozzle1TrayUload.Name = "btnNozzle1TrayUload";
            this.btnNozzle1TrayUload.Size = new System.Drawing.Size(79, 41);
            this.btnNozzle1TrayUload.TabIndex = 5;
            this.btnNozzle1TrayUload.Text = "吸嘴1Tray放料";
            this.btnNozzle1TrayUload.UseVisualStyleBackColor = true;
            this.btnNozzle1TrayUload.Click += new System.EventHandler(this.btnNozzle1TrayUload_Click);
            // 
            // btnNozzle2TrayUload
            // 
            this.btnNozzle2TrayUload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNozzle2TrayUload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNozzle2TrayUload.Location = new System.Drawing.Point(0, 78);
            this.btnNozzle2TrayUload.Margin = new System.Windows.Forms.Padding(0);
            this.btnNozzle2TrayUload.Name = "btnNozzle2TrayUload";
            this.btnNozzle2TrayUload.Size = new System.Drawing.Size(99, 41);
            this.btnNozzle2TrayUload.TabIndex = 4;
            this.btnNozzle2TrayUload.Text = "吸嘴2Tray放料";
            this.btnNozzle2TrayUload.UseVisualStyleBackColor = true;
            this.btnNozzle2TrayUload.Click += new System.EventHandler(this.btnNozzle2TrayUload_Click);
            // 
            // Nozzle2SocketPickDut
            // 
            this.Nozzle2SocketPickDut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Nozzle2SocketPickDut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Nozzle2SocketPickDut.Location = new System.Drawing.Point(119, 39);
            this.Nozzle2SocketPickDut.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.Nozzle2SocketPickDut.Name = "Nozzle2SocketPickDut";
            this.Nozzle2SocketPickDut.Size = new System.Drawing.Size(79, 39);
            this.Nozzle2SocketPickDut.TabIndex = 3;
            this.Nozzle2SocketPickDut.Text = "吸嘴2Socket取料";
            this.Nozzle2SocketPickDut.UseVisualStyleBackColor = true;
            this.Nozzle2SocketPickDut.Click += new System.EventHandler(this.Nozzle2SocketPickDut_Click);
            // 
            // btnNozzle1SocketPutDut
            // 
            this.btnNozzle1SocketPutDut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNozzle1SocketPutDut.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNozzle1SocketPutDut.Location = new System.Drawing.Point(0, 39);
            this.btnNozzle1SocketPutDut.Margin = new System.Windows.Forms.Padding(0);
            this.btnNozzle1SocketPutDut.Name = "btnNozzle1SocketPutDut";
            this.btnNozzle1SocketPutDut.Size = new System.Drawing.Size(99, 39);
            this.btnNozzle1SocketPutDut.TabIndex = 2;
            this.btnNozzle1SocketPutDut.Text = "吸嘴1Socket放料";
            this.btnNozzle1SocketPutDut.UseVisualStyleBackColor = true;
            this.btnNozzle1SocketPutDut.Click += new System.EventHandler(this.btnNozzle1SocketPutDut_Click);
            // 
            // btnNozzle1Load
            // 
            this.btnNozzle1Load.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNozzle1Load.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNozzle1Load.Location = new System.Drawing.Point(0, 0);
            this.btnNozzle1Load.Margin = new System.Windows.Forms.Padding(0);
            this.btnNozzle1Load.Name = "btnNozzle1Load";
            this.btnNozzle1Load.Size = new System.Drawing.Size(99, 39);
            this.btnNozzle1Load.TabIndex = 0;
            this.btnNozzle1Load.Text = "吸嘴1Tray盘取料";
            this.btnNozzle1Load.UseVisualStyleBackColor = true;
            this.btnNozzle1Load.Click += new System.EventHandler(this.btnNozzle1Load_Click);
            // 
            // btnNozzle1Scan
            // 
            this.btnNozzle1Scan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNozzle1Scan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNozzle1Scan.Location = new System.Drawing.Point(119, 0);
            this.btnNozzle1Scan.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnNozzle1Scan.Name = "btnNozzle1Scan";
            this.btnNozzle1Scan.Size = new System.Drawing.Size(79, 39);
            this.btnNozzle1Scan.TabIndex = 1;
            this.btnNozzle1Scan.Text = "吸嘴1扫码";
            this.btnNozzle1Scan.UseVisualStyleBackColor = true;
            this.btnNozzle1Scan.Click += new System.EventHandler(this.btnNozzle1Scan_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 7;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28496F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28496F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28496F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28496F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28496F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28761F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28761F));
            this.tableLayoutPanel1.Controls.Add(this.btnNozzle1Scan, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnNozzle1Load, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnNozzle1SocketPutDut, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Nozzle2SocketPickDut, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnNozzle2TrayUload, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnNozzle1TrayUload, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSocketOpenCap, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSocketClose, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRotateOneStation, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSelectDut, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnTesterCylinderMoveToWork, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnTesterCylinderMoveToBase, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnTesterReset, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnTmReset, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnLightControl, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnMaintenanceLamp, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnIonFan, 5, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(695, 119);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnTesterCylinderMoveToWork
            // 
            this.btnTesterCylinderMoveToWork.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTesterCylinderMoveToWork.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTesterCylinderMoveToWork.Location = new System.Drawing.Point(317, 39);
            this.btnTesterCylinderMoveToWork.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnTesterCylinderMoveToWork.Name = "btnTesterCylinderMoveToWork";
            this.btnTesterCylinderMoveToWork.Size = new System.Drawing.Size(79, 39);
            this.btnTesterCylinderMoveToWork.TabIndex = 13;
            this.btnTesterCylinderMoveToWork.Text = "转盘气缸顶升";
            this.btnTesterCylinderMoveToWork.UseVisualStyleBackColor = true;
            this.btnTesterCylinderMoveToWork.Click += new System.EventHandler(this.btnTesterCylinderMoveToWork_Click);
            // 
            // btnTesterCylinderMoveToBase
            // 
            this.btnTesterCylinderMoveToBase.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTesterCylinderMoveToBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnTesterCylinderMoveToBase.Location = new System.Drawing.Point(317, 78);
            this.btnTesterCylinderMoveToBase.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnTesterCylinderMoveToBase.Name = "btnTesterCylinderMoveToBase";
            this.btnTesterCylinderMoveToBase.Size = new System.Drawing.Size(79, 41);
            this.btnTesterCylinderMoveToBase.TabIndex = 14;
            this.btnTesterCylinderMoveToBase.Text = "转盘气缸收回";
            this.btnTesterCylinderMoveToBase.UseVisualStyleBackColor = true;
            this.btnTesterCylinderMoveToBase.Click += new System.EventHandler(this.btnTesterCylinderMoveToBase_Click);
            // 
            // btnLightControl
            // 
            this.btnLightControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLightControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnLightControl.Location = new System.Drawing.Point(416, 78);
            this.btnLightControl.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnLightControl.Name = "btnLightControl";
            this.btnLightControl.Size = new System.Drawing.Size(79, 41);
            this.btnLightControl.TabIndex = 15;
            this.btnLightControl.Text = "机台照明开";
            this.btnLightControl.UseVisualStyleBackColor = true;
            this.btnLightControl.Click += new System.EventHandler(this.btnLightControl_Click);
            // 
            // btnMaintenanceLamp
            // 
            this.btnMaintenanceLamp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMaintenanceLamp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMaintenanceLamp.Location = new System.Drawing.Point(515, 0);
            this.btnMaintenanceLamp.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnMaintenanceLamp.Name = "btnMaintenanceLamp";
            this.btnMaintenanceLamp.Size = new System.Drawing.Size(79, 39);
            this.btnMaintenanceLamp.TabIndex = 16;
            this.btnMaintenanceLamp.Text = "检修照明灯开";
            this.btnMaintenanceLamp.UseVisualStyleBackColor = true;
            this.btnMaintenanceLamp.Click += new System.EventHandler(this.btnMaintenanceLamp_Click);
            // 
            // btnIonFan
            // 
            this.btnIonFan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnIonFan.Location = new System.Drawing.Point(515, 39);
            this.btnIonFan.Margin = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnIonFan.Name = "btnIonFan";
            this.btnIonFan.Size = new System.Drawing.Size(79, 39);
            this.btnIonFan.TabIndex = 19;
            this.btnIonFan.Text = "离子风扇开";
            this.btnIonFan.UseVisualStyleBackColor = true;
            this.btnIonFan.Click += new System.EventHandler(this.btnIonFan_Click);
            // 
            // UCSemiAutoControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UCSemiAutoControl";
            this.Size = new System.Drawing.Size(695, 119);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTmReset;
        private System.Windows.Forms.Button btnTesterReset;
        private System.Windows.Forms.Button btnSelectDut;
        private System.Windows.Forms.Button btnRotateOneStation;
        private System.Windows.Forms.Button btnSocketClose;
        private System.Windows.Forms.Button btnSocketOpenCap;
        private System.Windows.Forms.Button btnNozzle1TrayUload;
        private System.Windows.Forms.Button btnNozzle2TrayUload;
        private System.Windows.Forms.Button Nozzle2SocketPickDut;
        private System.Windows.Forms.Button btnNozzle1SocketPutDut;
        private System.Windows.Forms.Button btnNozzle1Load;
        private System.Windows.Forms.Button btnNozzle1Scan;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnTesterCylinderMoveToWork;
        private System.Windows.Forms.Button btnTesterCylinderMoveToBase;
        private System.Windows.Forms.Button btnLightControl;
        private System.Windows.Forms.Button btnMaintenanceLamp;
        private System.Windows.Forms.Button btnIonFan;
    }
}
