
namespace VisionControls
{
    partial class SuperWind
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SuperWind));
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox_cross = new System.Windows.Forms.PictureBox();
            this.pictureBox_Color = new System.Windows.Forms.PictureBox();
            this.pictureBox_3D = new System.Windows.Forms.PictureBox();
            this.pictureBox_rainbow = new System.Windows.Forms.PictureBox();
            this.pictureBox_up = new System.Windows.Forms.PictureBox();
            this.pictureBox_down = new System.Windows.Forms.PictureBox();
            this.pictureBox_TurnLeft = new System.Windows.Forms.PictureBox();
            this.pictureBox_TurnRight = new System.Windows.Forms.PictureBox();
            this.pictureBox_reset = new System.Windows.Forms.PictureBox();
            this.pictureBox_max = new System.Windows.Forms.PictureBox();
            this.hwind = new HalconDotNet.HWindowControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_cross)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Color)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_3D)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_rainbow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_up)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_down)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TurnLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TurnRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_reset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_max)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.pictureBox_cross);
            this.panel1.Controls.Add(this.pictureBox_Color);
            this.panel1.Controls.Add(this.pictureBox_3D);
            this.panel1.Controls.Add(this.pictureBox_rainbow);
            this.panel1.Controls.Add(this.pictureBox_up);
            this.panel1.Controls.Add(this.pictureBox_down);
            this.panel1.Controls.Add(this.pictureBox_TurnLeft);
            this.panel1.Controls.Add(this.pictureBox_TurnRight);
            this.panel1.Controls.Add(this.pictureBox_reset);
            this.panel1.Controls.Add(this.pictureBox_max);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(620, 30);
            this.panel1.TabIndex = 7;
            this.panel1.Visible = false;
            // 
            // pictureBox_cross
            // 
            this.pictureBox_cross.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_cross.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox_cross.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_cross.Image")));
            this.pictureBox_cross.Location = new System.Drawing.Point(340, 0);
            this.pictureBox_cross.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox_cross.Name = "pictureBox_cross";
            this.pictureBox_cross.Size = new System.Drawing.Size(28, 30);
            this.pictureBox_cross.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_cross.TabIndex = 18;
            this.pictureBox_cross.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_cross, "ROI颜色");
            this.pictureBox_cross.Click += new System.EventHandler(this.pictureBox_cross_Click);
            // 
            // pictureBox_Color
            // 
            this.pictureBox_Color.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_Color.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox_Color.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_Color.Image")));
            this.pictureBox_Color.Location = new System.Drawing.Point(368, 0);
            this.pictureBox_Color.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox_Color.Name = "pictureBox_Color";
            this.pictureBox_Color.Size = new System.Drawing.Size(28, 30);
            this.pictureBox_Color.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_Color.TabIndex = 11;
            this.pictureBox_Color.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_Color, "ROI颜色");
            this.pictureBox_Color.Click += new System.EventHandler(this.pictureBox_Color_Click);
            // 
            // pictureBox_3D
            // 
            this.pictureBox_3D.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_3D.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox_3D.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_3D.Image")));
            this.pictureBox_3D.Location = new System.Drawing.Point(396, 0);
            this.pictureBox_3D.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox_3D.Name = "pictureBox_3D";
            this.pictureBox_3D.Size = new System.Drawing.Size(28, 30);
            this.pictureBox_3D.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_3D.TabIndex = 12;
            this.pictureBox_3D.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_3D, "3D显示");
            this.pictureBox_3D.Click += new System.EventHandler(this.pictureBox_3D_Click);
            // 
            // pictureBox_rainbow
            // 
            this.pictureBox_rainbow.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_rainbow.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox_rainbow.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_rainbow.Image")));
            this.pictureBox_rainbow.Location = new System.Drawing.Point(424, 0);
            this.pictureBox_rainbow.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox_rainbow.Name = "pictureBox_rainbow";
            this.pictureBox_rainbow.Size = new System.Drawing.Size(28, 30);
            this.pictureBox_rainbow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_rainbow.TabIndex = 13;
            this.pictureBox_rainbow.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_rainbow, "彩虹图");
            this.pictureBox_rainbow.Click += new System.EventHandler(this.pictureBox_rainbow_Click);
            // 
            // pictureBox_up
            // 
            this.pictureBox_up.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_up.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox_up.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_up.Image")));
            this.pictureBox_up.Location = new System.Drawing.Point(452, 0);
            this.pictureBox_up.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox_up.Name = "pictureBox_up";
            this.pictureBox_up.Size = new System.Drawing.Size(28, 30);
            this.pictureBox_up.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_up.TabIndex = 14;
            this.pictureBox_up.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_up, "放大");
            this.pictureBox_up.Click += new System.EventHandler(this.pictureBox_up_Click);
            // 
            // pictureBox_down
            // 
            this.pictureBox_down.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_down.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox_down.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_down.Image")));
            this.pictureBox_down.Location = new System.Drawing.Point(480, 0);
            this.pictureBox_down.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox_down.Name = "pictureBox_down";
            this.pictureBox_down.Size = new System.Drawing.Size(28, 30);
            this.pictureBox_down.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_down.TabIndex = 15;
            this.pictureBox_down.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_down, "缩小");
            this.pictureBox_down.Click += new System.EventHandler(this.pictureBox_down_Click);
            // 
            // pictureBox_TurnLeft
            // 
            this.pictureBox_TurnLeft.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_TurnLeft.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox_TurnLeft.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_TurnLeft.Image")));
            this.pictureBox_TurnLeft.Location = new System.Drawing.Point(508, 0);
            this.pictureBox_TurnLeft.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox_TurnLeft.Name = "pictureBox_TurnLeft";
            this.pictureBox_TurnLeft.Size = new System.Drawing.Size(28, 30);
            this.pictureBox_TurnLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_TurnLeft.TabIndex = 16;
            this.pictureBox_TurnLeft.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_TurnLeft, "左旋转");
            this.pictureBox_TurnLeft.Click += new System.EventHandler(this.pictureBox_TurnLeft_Click);
            // 
            // pictureBox_TurnRight
            // 
            this.pictureBox_TurnRight.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_TurnRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox_TurnRight.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_TurnRight.Image")));
            this.pictureBox_TurnRight.Location = new System.Drawing.Point(536, 0);
            this.pictureBox_TurnRight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox_TurnRight.Name = "pictureBox_TurnRight";
            this.pictureBox_TurnRight.Size = new System.Drawing.Size(28, 30);
            this.pictureBox_TurnRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_TurnRight.TabIndex = 17;
            this.pictureBox_TurnRight.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_TurnRight, "右旋转");
            this.pictureBox_TurnRight.Click += new System.EventHandler(this.pictureBox_TurnRight_Click);
            // 
            // pictureBox_reset
            // 
            this.pictureBox_reset.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_reset.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox_reset.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_reset.Image")));
            this.pictureBox_reset.Location = new System.Drawing.Point(564, 0);
            this.pictureBox_reset.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox_reset.Name = "pictureBox_reset";
            this.pictureBox_reset.Size = new System.Drawing.Size(28, 30);
            this.pictureBox_reset.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_reset.TabIndex = 10;
            this.pictureBox_reset.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_reset, "适应窗口");
            this.pictureBox_reset.Click += new System.EventHandler(this.pictureBox_reset_Click);
            // 
            // pictureBox_max
            // 
            this.pictureBox_max.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_max.Dock = System.Windows.Forms.DockStyle.Right;
            this.pictureBox_max.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox_max.Image")));
            this.pictureBox_max.Location = new System.Drawing.Point(592, 0);
            this.pictureBox_max.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pictureBox_max.Name = "pictureBox_max";
            this.pictureBox_max.Size = new System.Drawing.Size(28, 30);
            this.pictureBox_max.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_max.TabIndex = 9;
            this.pictureBox_max.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox_max, "最大化");
            this.pictureBox_max.Click += new System.EventHandler(this.pictureBox_max_Click);
            // 
            // hwind
            // 
            this.hwind.AutoSize = true;
            this.hwind.BackColor = System.Drawing.Color.LemonChiffon;
            this.hwind.BorderColor = System.Drawing.Color.LemonChiffon;
            this.hwind.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hwind.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hwind.Location = new System.Drawing.Point(0, 30);
            this.hwind.Name = "hwind";
            this.hwind.Size = new System.Drawing.Size(620, 412);
            this.hwind.TabIndex = 8;
            this.hwind.WindowSize = new System.Drawing.Size(620, 412);
            this.hwind.KeyDown += new System.Windows.Forms.KeyEventHandler(this.hwind_KeyDown);
            // 
            // SuperWind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hwind);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "SuperWind";
            this.Size = new System.Drawing.Size(620, 442);
            this.Load += new System.EventHandler(this.SuperWind_Load);
            this.SizeChanged += new System.EventHandler(this.UserControl_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SuperWind_KeyDown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_cross)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Color)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_3D)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_rainbow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_up)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_down)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TurnLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TurnRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_reset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_max)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox_Color;
        private System.Windows.Forms.PictureBox pictureBox_3D;
        private System.Windows.Forms.PictureBox pictureBox_rainbow;
        private System.Windows.Forms.PictureBox pictureBox_up;
        private System.Windows.Forms.PictureBox pictureBox_down;
        private System.Windows.Forms.PictureBox pictureBox_TurnLeft;
        private System.Windows.Forms.PictureBox pictureBox_TurnRight;
        private System.Windows.Forms.PictureBox pictureBox_reset;
        private System.Windows.Forms.PictureBox pictureBox_max;
        public HalconDotNet.HWindowControl hwind;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBox_cross;
    }
}
