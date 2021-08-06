using System;
using System.Drawing;
using System.Windows.Forms;
using HalconDotNet;
namespace VisionControls
{
    public partial class SuperWind : UserControl
    {
        /// <summary></summary>
        public HWndCtrl viewController;
        /// <summary></summary>
        public ROIController roiController;
        /// <summary></summary>
        private ROIInfo ROIArrayList;

        /// <summary>
        /// 获取或设置图像（相关字段初始化）
        /// </summary>
        public HImage image
        {
            get
            {
                return viewController.GetImage();
            }
            set
            {
                if (value != null && value.IsInitialized())
                {
                    if (roiController == null)
                    {
                        roiController = new ROIController();
                        viewController = new HWndCtrl(this.hwind);
                        viewController.ShowMargin = false;
                        roiController.viewController = viewController;
                        roiController.RoiInfo = ROIArrayList;
                        viewController.useROIController(roiController);
                        viewController.setViewState(HWndCtrl.MODE_VIEW_NONE);
                        ROI.ROIchange += ROI_ROIchange;
                    }
                    viewController.addIconicVar(value.CopyImage());
                    viewController.repaint();
                }
            }
        }

        /// <summary>
        /// 打印文本到图形窗口（字体和位置硬编码）
        /// </summary>
        public string Message
        {
            set
            {
                HOperatorSet.SetFont(viewController.viewPort.HalconWindow, "楷体-30");
                viewController.viewPort.HalconWindow.SetTposition(10, 100);
                viewController.viewPort.HalconWindow.WriteString(value);
            }
        }

        /// <summary>
        /// 在图像指定位置打印OK（绿色）或NG（红色）
        /// </summary>
        public bool OKNGlable
        {
            set
            {
                HTuple hv_Row1 = new HTuple(), hv_Column1 = new HTuple();
                HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
                HTuple hv_fontSize = new HTuple(), hv_Font = new HTuple();
                HTuple hv_Substrings = new HTuple(), hv_NewFont = new HTuple();

                hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Row2.Dispose(); hv_Column2.Dispose();
                HOperatorSet.GetPart(viewController.viewPort.HalconWindow, out hv_Row1, out hv_Column1, out hv_Row2,out hv_Column2);
                hv_fontSize.Dispose();
                hv_fontSize = 30;
                hv_Font.Dispose();
                HOperatorSet.GetFont(viewController.viewPort.HalconWindow, out hv_Font);
                hv_Substrings.Dispose();
                HOperatorSet.TupleSplit(hv_Font, "-", out hv_Substrings);
                hv_NewFont.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_NewFont = ((((hv_Substrings.TupleSelect(
                        0)) + "-") + "Italic") + "-") + hv_fontSize;
                }
                HOperatorSet.SetFont(viewController.viewPort.HalconWindow, hv_NewFont);
                string lable = value ? "OK" : "NG";
                string color = value ? "green" : "red";

                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    HOperatorSet.DispText(viewController.viewPort.HalconWindow, lable, "image", 10, hv_Column2 - 150,
                        "white", (new HTuple("box_color")).TupleConcat("shadow"), (new HTuple(color)).TupleConcat(
                        0));
                }

                HOperatorSet.SetFont(viewController.viewPort.HalconWindow, hv_Font);
                hv_Row1.Dispose();
                hv_Column1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_fontSize.Dispose();
                hv_Font.Dispose();
                hv_Substrings.Dispose();
                hv_NewFont.Dispose();
            }
        }

        /// <summary>
        /// 显示对象
        /// </summary>
        public HObject obj
        {
            set
            {
                if (value != null && value.IsInitialized())
                {
                    viewController.viewPort.HalconWindow.SetDraw("margin");
                    viewController.viewPort.HalconWindow.DispObj(value.CopyObj(1, 100));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ObjColor
        {

            set
            {
                viewController.viewPort.HalconWindow.SetColor(value);

            }
        }
        public Color RoiColor
        {
            set
            {
                roiController.inactiveCol = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(value.R, value.G, value.B)) + "40";
                viewController.repaint();
            }
        }
        public bool menubarVisible
        {
            get
            {
                return this.panel1.Visible;
            }
            set
            {
                if (this.viewController != null)
                {
                    this.panel1.Visible = value;
                    this.viewController.repaint();
                    this.viewController.AddSmallWind();
                }

            }
        }
        public bool isDrawUserRegion
        {

            set
            {
                if (!value)
                {
                    roiController.m_pen = 0;
                    if (roiController.obj != null && roiController.obj.IsInitialized())
                    {
                        roiController.addUserdefine();
                    }
                }
                else
                {
                    roiController.obj = new HRegion();
                    roiController.Start_Draw(1);
                }

            }
        }

        public SuperWind()
        {
            InitializeComponent();
            ROIArrayList = new ROIInfo();
        }

        private void SuperWind_Load(object sender, EventArgs e)
        {

        }

        private void ROI_ROIchange(object sender, EventArgs e)
        {

        }


        private void pictureBox_Color_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog();
            if (DialogResult.Cancel != color.ShowDialog())
            {
                RoiColor = color.Color;
            }

        }
        private void pictureBox_3D_Click(object sender, EventArgs e)
        {
            viewController.Show3D_strip.Checked = !viewController.Show3D_strip.Checked;
            viewController.Disp3Dimage();
            pictureBox_3D.BackColor = viewController.Show3D_strip.Checked ? Color.Orange : Color.Transparent;
        }

        private void pictureBox_cross_Click(object sender, EventArgs e)
        {
            viewController.CrossStatue = !viewController.CrossStatue;
            pictureBox_cross.BackColor = viewController.CrossStatue ? Color.Orange : Color.Transparent;
            viewController.repaint();
        }
        private void pictureBox_up_Click(object sender, EventArgs e)
        {
            scale += 10;
            if (scale >= 500)
                scale = 500;
            viewController.zoomByGUIHandle(scale);
        }
        double scale = 100;
        private void pictureBox_rainbow_Click(object sender, EventArgs e)
        {
            viewController.ShowRainBow_strip.Checked = !viewController.ShowRainBow_strip.Checked;
            viewController.DispRainBow();
        }

        private void pictureBox_down_Click(object sender, EventArgs e)
        {
            scale -= 10;
            if (scale <= 10)
                scale = 10;
            viewController.zoomByGUIHandle(scale);
        }

        private void pictureBox_TurnLeft_Click(object sender, EventArgs e)
        {
            viewController.RotateN_strip_Click(null, null);
        }

        private void pictureBox_TurnRight_Click(object sender, EventArgs e)
        {
            viewController.RotateF_strip_Click(null, null);
        }

        private void pictureBox_reset_Click(object sender, EventArgs e)
        {
            viewController.ImageFit();
        }
        private void fullscreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ((Form)sender).Close();
            }
        }
        HWndCtrl viewControllerMax;
        ROIController roiControllerMax;
        private void pictureBox_max_Click(object sender, EventArgs e)
        {
            Form fullscreen = new Form();
            Rectangle rect = System.Windows.Forms.SystemInformation.VirtualScreen;
            fullscreen.Width = rect.Width;
            fullscreen.Height = rect.Height;
            fullscreen.FormBorderStyle = FormBorderStyle.None;

            HWindowControl wind = new HWindowControl();
            wind.Dock = DockStyle.Fill;
            fullscreen.Controls.Add(wind);
            fullscreen.KeyDown += fullscreen_KeyDown;

            viewControllerMax = new HWndCtrl(wind);
            roiControllerMax = new ROIController();
            roiControllerMax.viewController = viewControllerMax;
            roiControllerMax.RoiInfo.ROIList = new System.Collections.ArrayList();
            viewControllerMax.useROIController(roiControllerMax);
            viewControllerMax.setViewState(HWndCtrl.MODE_VIEW_NONE);
            HImage cc = this.viewController.GetImage();
            viewControllerMax.addIconicVar(cc.CopyImage());
            viewControllerMax.repaint();



            fullscreen.KeyPreview = true;
            fullscreen.Shown += fullscreen_Load;
            fullscreen.ShowDialog();
        }
        void fullscreen_Load(object sender, EventArgs e)
        {
            viewControllerMax.repaint();
        }

        private void UserControl_SizeChanged(object sender, EventArgs e)
        {
            if (this.viewController != null)
            {
                this.viewController.repaint();
                this.viewController.AddSmallWind();
            }

        }
        private void hwind_KeyDown(object sender, KeyEventArgs e)
        {

        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Delete && roiController.activeROIidx >= 0)
            {
                roiController.removeActive();
            }
            if (keyData == Keys.Add || keyData == Keys.Oemplus)
            {
                if (roiController.m_pen >= 42 || roiController.m_pen == 0)
                    return true;
                roiController.m_pen += 1;
                roiController.hWindowControl1_MouseMove(null, null);
                viewController.repaint();
            }

            else if (keyData == Keys.Subtract || keyData == Keys.OemMinus)
            {
                if (roiController.m_pen <= 1 || roiController.m_pen == 0)
                    return true;
                roiController.m_pen -= 1;
                roiController.hWindowControl1_MouseMove(null, null);
                viewController.repaint();
            }
            return true;
        }
        private void SuperWind_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}
