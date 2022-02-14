using System;
using System.Collections;
using HalconDotNet;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;

namespace VisionControls
{
    public delegate void IconicDelegate(int val);
    public delegate void FuncDelegate();

    [Serializable]
    public class HWndCtrl
    {
        public bool ShowMargin = true;
        public bool CrossStatue = false;
        public bool isShowRainBow = false;
        public bool OnlyImage = false;
        public const int MODE_VIEW_NONE = 10;
        public const int MODE_VIEW_ZOOM = 11;
        public const int MODE_VIEW_MOVE = 12;
        public const int MODE_VIEW_ZOOMWINDOW = 13;
        public const int MODE_INCLUDE_ROI = 1;
        public const int MODE_EXCLUDE_ROI = 2;
        public const int EVENT_UPDATE_IMAGE = 31;
        public const int ERR_READING_IMG = 32;
        public const int ERR_DEFINING_GC = 33;
        private const int MAXNUMOBJLIST = 50;
        public HTuple scaleX = 0.0125, scaleY = 0.0125, scaleZ = 0.0016;

        public int stateView;
        private bool mousePressed = false;
        private double startX, startY;


        public HWindowControl viewPort;
        public HWindowControl SmallWindow;
        public bool IsDrawMode
        {
            set
            {
                if (value)
                {
                    viewPort.ContextMenuStrip = null;
                }
                else
                {
                    viewPort.ContextMenuStrip = hv_MenuStrip;
                }
            }
        }


        private ROIController roiManager;



        private int dispROI;



        private int windowWidth;
        private int windowHeight;
        public int imageWidth;
        public int imageHeight;

        private int[] CompRangeX;
        private int[] CompRangeY;


        private int prevCompX, prevCompY;
        private double stepSizeX, stepSizeY;



        private double ImgRow1, ImgCol1, ImgRow2, ImgCol2;


        public string exceptionText = "";



        public FuncDelegate addInfoDelegate;


        public IconicDelegate NotifyIconObserver;


        private HWindow ZoomWindow;
        private double zoomWndFactor;
        private double zoomAddOn;
        private int zoomWndSize;



        private ArrayList HObjList;
        private HImage RainBowImage = null;

        //显示实时处理的数据
        private HObject ResultObj = new HObject();

        private GraphicsContext mGC;

        public ContextMenuStrip hv_MenuStrip;    //右键菜单控件
        // 窗体控件右键菜单内容
        ToolStripMenuItem fit_strip;
        ToolStripMenuItem showMargin;
        ToolStripMenuItem fit_showImageOnly;
        ToolStripMenuItem saveImg_strip;
        ToolStripMenuItem saveWindow_strip;

        ToolStripMenuItem ShowHistogram_strip;
        ToolStripMenuItem ShowCross_shrip;
        public ToolStripMenuItem ShowRainBow_strip;
        public ToolStripMenuItem Show3D_strip;
        ToolStripMenuItem measureLine_strip;
        ToolStripMenuItem measureWidth_strip;
        ToolStripMenuItem LoadImage_strip;
        ToolStripMenuItem RotateF_strip;
        ToolStripMenuItem RotateN_strip;
        ToolStripMenuItem AddROI;
        ToolStripMenuItem RemoveROI;
        public HWndCtrl(HWindowControl view)
        {


            viewPort = view;
            stateView = MODE_VIEW_NONE;
            windowWidth = viewPort.Size.Width;
            windowHeight = viewPort.Size.Height;

            zoomWndFactor = (double)imageWidth / viewPort.Width;
            zoomAddOn = Math.Pow(0.9, 5);
            zoomWndSize = 150;

            /*default*/
            CompRangeX = new int[] { 0, 100 };
            CompRangeY = new int[] { 0, 100 };

            prevCompX = prevCompY = 0;

            dispROI = MODE_INCLUDE_ROI;//1;

            RegistEvent(true);

            addInfoDelegate = new FuncDelegate(dummyV);
            NotifyIconObserver = new IconicDelegate(dummy);

            // 能存放20个图形变量
            HObjList = new ArrayList(20);
            mGC = new GraphicsContext();
            mGC.gcNotification = new GCDelegate(exceptionGC);


            fit_strip = new ToolStripMenuItem("适应窗口");
            fit_strip.Click += new EventHandler((s, e) => ImageFit());

            fit_showImageOnly = new ToolStripMenuItem("只显示图像");
            fit_showImageOnly.Click += new EventHandler((s, e) => Olly_image());
            fit_showImageOnly.CheckOnClick = true;

            showMargin = new ToolStripMenuItem("显示margin");
            showMargin.Click += new EventHandler((s, e) => Olly_margin());
            showMargin.CheckOnClick = true;

            saveImg_strip = new ToolStripMenuItem("保存原始图像");
            saveImg_strip.Click += new EventHandler((s, e) => SaveImage(s, e));

            saveWindow_strip = new ToolStripMenuItem("截图另存");
            saveWindow_strip.Click += new EventHandler((s, e) => SaveWindowDump());

            LoadImage_strip = new ToolStripMenuItem("加载图像");
            LoadImage_strip.Click += new EventHandler((s, e) => LoadImage(s, e));

            ShowCross_shrip = new ToolStripMenuItem("显示十字架");
            ShowCross_shrip.Click += new EventHandler((s, e) => DispCross());
            ShowCross_shrip.CheckOnClick = true;

            ShowRainBow_strip = new ToolStripMenuItem("显示彩虹图");
            ShowRainBow_strip.Click += new EventHandler((s, e) => DispRainBow());
            ShowRainBow_strip.CheckOnClick = true;

            Show3D_strip = new ToolStripMenuItem("显示3D点云");
            Show3D_strip.Click += new EventHandler((s, e) => Disp3Dimage());
            Show3D_strip.CheckOnClick = true;

            ShowHistogram_strip = new ToolStripMenuItem("灰度直方图");
            ShowHistogram_strip.Click += ShowHistogram_strip_Click;

            measureLine_strip = new ToolStripMenuItem("距离测量");
            measureLine_strip.Click += MeasureLine_strip_Click;

            measureWidth_strip = new ToolStripMenuItem("测量边缘宽度");
            measureWidth_strip.Click += MeasureWidth_strip_Click;

            RotateF_strip = new ToolStripMenuItem("顺时针旋转90度");
            RotateF_strip.Click += RotateF_strip_Click;

            RotateN_strip = new ToolStripMenuItem("逆时针旋转90度");
            RotateN_strip.Click += RotateN_strip_Click;

            AddROI = new ToolStripMenuItem("添加ROI");
            AddROI.Click += AddROI_Click;
            RemoveROI = new ToolStripMenuItem("移除ROI");
            RemoveROI.Click += RemoveROI_Click;

            hv_MenuStrip = new ContextMenuStrip();
            hv_MenuStrip.Items.Add(measureWidth_strip);
            hv_MenuStrip.Items.Add(fit_strip);
            hv_MenuStrip.Items.Add(new ToolStripSeparator());
            hv_MenuStrip.Items.Add(showMargin);
            hv_MenuStrip.Items.Add(fit_showImageOnly);
            hv_MenuStrip.Items.Add(ShowCross_shrip);
            hv_MenuStrip.Items.Add(ShowRainBow_strip);
            hv_MenuStrip.Items.Add(Show3D_strip);
            hv_MenuStrip.Items.Add(LoadImage_strip);
            hv_MenuStrip.Items.Add(measureLine_strip);
            hv_MenuStrip.Items.Add(ShowHistogram_strip);
            hv_MenuStrip.Items.Add(new ToolStripSeparator());
            hv_MenuStrip.Items.Add(saveImg_strip);
            hv_MenuStrip.Items.Add(saveWindow_strip);
            hv_MenuStrip.Items.Add(RotateF_strip);
            hv_MenuStrip.Items.Add(RotateN_strip);
            viewPort.ContextMenuStrip = hv_MenuStrip;


            AddSmallWind();
        }

        public void AddSmallWind()
        {
            this.viewPort.Controls.Clear();
            SmallWindow = new HWindowControl();
            SmallWindow.Size = new Size(150, 100);
            SmallWindow.Location = new Point(this.viewPort.Width - 155, this.viewPort.Height - 105);
            this.viewPort.Controls.Add(SmallWindow);
            SmallWindow.Visible = false;
            SmallWindow.HMouseDown += SmallWindow_HMouseDown;
            SmallWindow.HMouseMove += SmallWindow_HMouseMove;
            SmallWindow.HMouseUp += SmallWindow_HMouseUp;
        }
        private void SmallWindow_HMouseUp(object sender, HMouseEventArgs e)
        {
            smallwindowDown = false;
        }

        bool smallwindowDown = false;
        Point PoseStart;



        private void SmallWindow_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (smallwindowDown)
            {
                int offx = (int)e.X - PoseStart.X;
                int offy = (int)e.Y - PoseStart.Y;
                Rectangle OldPart = this.viewPort.ImagePart;
                Rectangle NewPart = new Rectangle((int)e.X - PoseStart.X, (int)e.Y - PoseStart.Y, OldPart.Width, OldPart.Height);
                this.viewPort.ImagePart = NewPart;
                this.repaint();
            }
        }

        private void SmallWindow_HMouseDown(object sender, HMouseEventArgs e)
        {
            smallwindowDown = true;
            Rectangle OldPart = this.viewPort.ImagePart;
            if (OldPart.Contains((int)e.X, (int)e.Y))
            {
                PoseStart.X = (int)e.X - OldPart.X;
                PoseStart.Y = (int)e.Y - OldPart.Y;
            }
            else
            {
                PoseStart.X = 0;
                PoseStart.Y = 0;
            }

        }

        //原始大小
        public void ImageFit()
        {
            SmallWindow.Visible = false;
            setViewState(MODE_VIEW_NONE);
            resetWindow();
            repaint();
        }
        public void AddResult(HObject obj)
        {
            HOperatorSet.ConcatObj(obj, ResultObj, out ResultObj);
        }
        public void ClearRuslt()
        {
            ResultObj.GenEmptyObj();
        }
        private void AddROI_Click(object sender, EventArgs e)
        {

        }
        private void RemoveROI_Click(object sender, EventArgs e)
        {
            viewPort.Focus();
            OnlyImage = true;
            viewPort.ContextMenuStrip = null;
            if (Convert.ToInt32(viewPort.Tag) == 1)
            {
                roiManager.removeActive();
            }
            else if (Convert.ToInt32(viewPort.Tag) == 2)
            {
                roiManager.removeActive();
            }
            else if (Convert.ToInt32(viewPort.Tag) == 3)
            {
                roiManager.removeActive();
            }
            else if (Convert.ToInt32(viewPort.Tag) == 4)
            {
                roiManager.removeActive();
            }
            else if (Convert.ToInt32(viewPort.Tag) == 5)
            {
                roiManager.removeActive();
            }
            else if (Convert.ToInt32(viewPort.Tag) == 6)
            {
                roiManager.removeActive();
            }
            viewPort.ContextMenuStrip = hv_MenuStrip;
            OnlyImage = false;
            repaint();
        }
        //导入图像
        private void LoadImage(object s, EventArgs e)
        {

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "图像文件(*.BMP,*.PNG,*.JPG,*.tiff,*.tif)|*.BMP;*.PNG;*.JPG;*.tiff;*.tif";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string imagefile = fileDialog.FileName;
                try
                {
                    isShowRainBow = false;
                    RainBowImage = null;
                    ShowRainBow_strip.Checked = false;
                    this.addIconicVar(new HImage(imagefile));
                    repaint();
                }
                catch
                {
                    MessageBox.Show("打开文件失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        //保存图像
        private void SaveImage(object s, EventArgs e)
        {

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "BMP图像|*.bmp|所有文件|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                GetImage().WriteImage("bmp", 0, sfd.FileName);
            }
        }
        //截图另存为
        private void SaveWindowDump()
        {

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "PNG图像|*.png|所有文件|*.*";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (String.IsNullOrEmpty(sfd.FileName))
                    return;
                HOperatorSet.DumpWindow(viewPort.HalconWindow, "png best", sfd.FileName);
            }
        }
        //十字架
        private void DispCross()
        {

            if (ShowCross_shrip.CheckState == CheckState.Checked)
            {
                CrossStatue = true;
            }
            else
            {
                CrossStatue = false;
            }
            repaint();
        }
        public void DispRainBow()
        {
            if (ShowRainBow_strip.CheckState == CheckState.Checked && HObjList.Count > 0)
            {

                RainBowImage = ImageHelper.GetRainBowImage((HImage)((HObjectEntry)HObjList[0]).HObj, null);
                if (RainBowImage != null)
                {
                    isShowRainBow = true;
                    //  DispLinear();
                }
                else
                {

                    isShowRainBow = false;
                }
            }
            else
            {
                this.viewPort.Controls.RemoveByKey("LinearPanel");
                isShowRainBow = false;
                RainBowImage = null;
            }
            repaint();
        }
        private void DispLinear()
        {
            Panel p = new Panel();
            p.Name = "LinearPanel";
            p.BackColor = System.Drawing.Color.Transparent;
            p.Width = this.viewPort.Width / 7;
            p.Height = (int)(this.viewPort.Height / 1.4);
            this.viewPort.Controls.Add(p);

            Rectangle rec = p.ClientRectangle;
            Brush b = new LinearGradientBrush(new PointF(0, 0), new PointF(0, p.Height), Color.Red, Color.Blue);
            Rectangle ColorRec = new Rectangle(rec.X, rec.Y, rec.Width / 3, rec.Height);
            Graphics g = p.CreateGraphics();
            g.FillRectangle(b, ColorRec);
            ColorRec.Inflate(-1, -1);
            g.DrawRectangle(new Pen(Color.White), ColorRec);
            Font font = new Font("楷体", 6, FontStyle.Regular);
            StringFormat format = new StringFormat();
            //水平对齐方式
            format.Alignment = StringAlignment.Center;
            //垂直对齐方式
            format.LineAlignment = StringAlignment.Center;

            var dist = rec.Height / 10;
            for (int i = 0; i < 10; i++)
            {
                string text = (0.001 * i).ToString();
                if (i == 1)
                {
                    text += "mm";
                }
                g.DrawString(text, font, new SolidBrush(Color.White), new Rectangle(rec.Width / 3, i * dist, rec.Width * 2 / 3, dist));
            }
        }
        private void RegistEvent(bool state)
        {
            viewPort.HMouseUp -= new HalconDotNet.HMouseEventHandler(this.mouseUp);
            viewPort.HMouseDown -= new HalconDotNet.HMouseEventHandler(this.mouseDown);
            viewPort.HMouseMove -= new HalconDotNet.HMouseEventHandler(this.mouseMoved);
            viewPort.HMouseWheel -= new HalconDotNet.HMouseEventHandler(this.mouseWheel);
            if (fit_strip != null)
            {
                fit_strip.Enabled = state;
                showMargin.Enabled = state;
                fit_showImageOnly.Enabled = state;
                saveImg_strip.Enabled = state;
                saveWindow_strip.Enabled = state;

                ShowHistogram_strip.Enabled = state;
                ShowCross_shrip.Enabled = state;
                ShowRainBow_strip.Enabled = state;


                measureLine_strip.Enabled = state;
                measureWidth_strip.Enabled = state;
                LoadImage_strip.Enabled = state;
                RotateN_strip.Enabled = state;
                RotateN_strip.Enabled = state;
                AddROI.Enabled = state;
                RemoveROI.Enabled = state;
            }
            if (state)
            {
                viewPort.HMouseUp += new HalconDotNet.HMouseEventHandler(this.mouseUp);
                viewPort.HMouseDown += new HalconDotNet.HMouseEventHandler(this.mouseDown);
                viewPort.HMouseMove += new HalconDotNet.HMouseEventHandler(this.mouseMoved);
                viewPort.HMouseWheel += new HalconDotNet.HMouseEventHandler(this.mouseWheel);
            }

        }
        public void Disp3Dimage()
        {
            if (Show3D_strip.CheckState == CheckState.Checked)
            {
                ImageHelper.ExitLoop = false;
                ImageHelper.hv_ExpDefaultWinHandle = this.viewPort.HalconWindow;
                HTuple hv_Grayval = new HTuple(), hv_ObjectModel3D = null;
                HTuple hv_PoseOut = null;
                try
                {
                    RegistEvent(false);
                    HTuple hv_Instructions = new HTuple();
                    hv_Instructions[0] = "Rotate: Left button";
                    hv_Instructions[1] = "Zoom:   Shift + left button";
                    hv_Instructions[2] = "Move:   Ctrl  + left button";
                    hv_ObjectModel3D = ImageHelper.TifToObjectModel3D((HImage)((HObjectEntry)HObjList[0]).HObj, scaleX, scaleY, scaleZ);
                    if (hv_ObjectModel3D == null)
                    {
                        ImageHelper.ExitLoop = true;
                        Show3D_strip.Checked = false;
                        MessageBox.Show("非3D资源图");
                        RegistEvent(true);
                        return;
                    }
                    ImageHelper.visualize_object_model_3d(this.viewPort.HalconWindow, hv_ObjectModel3D, new HTuple(),
                         new HTuple(), (new HTuple("lut")).TupleConcat("color_attrib"), (new HTuple("color1")).TupleConcat(
                         "coord_z"), "点云3D显示", "object", hv_Instructions, out hv_PoseOut);
                    HOperatorSet.ClearObjectModel3d(hv_ObjectModel3D);
                    ImageHelper.ExitLoop = true;
                    Show3D_strip.Checked = false;
                }
                catch (HalconException HDevExpDefaultException)
                {
                    MessageBox.Show(HDevExpDefaultException.Message);
                }
            }
            else
            {
                RegistEvent(true);
                ImageHelper.ExitLoop = true;
            }
            RegistEvent(true);
            repaint();
        }
        //只显示图像
        private void Olly_image()
        {
            if (fit_showImageOnly.CheckState == CheckState.Checked)
            {
                OnlyImage = true;
            }
            else
            {
                OnlyImage = false;
            }
            repaint();
        }
        private void Olly_margin()
        {
            if (showMargin.CheckState == CheckState.Checked)
            {
                ShowMargin = true;
            }
            else
            {
                ShowMargin = false;
            }
            repaint();
        }
        //灰度直方图
        private void ShowHistogram_strip_Click(object sender, EventArgs e)
        {
            viewPort.Focus();
            viewPort.HalconWindow.SetTposition(10, 10);
            viewPort.HalconWindow.SetColor("red");
            viewPort.HalconWindow.WriteString("鼠标左键点击并拉取矩形区域,鼠标右键完成");
            OnlyImage = true;
            viewPort.ContextMenuStrip = null;
            double r1, c1, r2, c2;
            //HTuple dd;

            //获取当前显示信息
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            int hv_lineWidth;
            HWindow window = viewPort.HalconWindow;
            window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);
            hv_lineWidth = (int)window.GetLineWidth();
            string hv_Draw = window.GetDraw();

            window.SetLineWidth(2);//设置线宽
            window.SetLineStyle(new HTuple());
            window.SetColor("green");//画点的颜色

            window.DrawRectangle1(out r1, out c1, out r2, out c2);
            window.DispRectangle1(r1, c1, r2, c2);
            Form frm = new Form();

            FunctionPlotUnit pointUnit = new FunctionPlotUnit();
            Size size = pointUnit.Size;
            size.Height = (int)(size.Height + 50);
            size.Width = (int)(size.Width + 50);
            frm.Size = size;
            frm.Controls.Add(pointUnit);
            pointUnit.Dock = DockStyle.Fill;
            HTuple grayVals;

            grayVals = GetGrayHisto(new HTuple(r1, c1, r2, c2));

            pointUnit.SetAxisAdaption(FunctionPlot.AXIS_RANGE_INCREASING);
            pointUnit.SetLabel("灰度值", "频率");
            pointUnit.SetFunctionPlotValue(grayVals);
            pointUnit.ComputeStatistics(grayVals);
            frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            frm.ShowDialog();

            //恢复窗口显示信息
            window.SetRgb(hv_Red, hv_Green, hv_Blue);
            window.SetLineWidth(hv_lineWidth);
            window.SetDraw(hv_Draw);
            viewPort.ContextMenuStrip = hv_MenuStrip;
            OnlyImage = false;
            repaint();
        }
        //距离测量
        private void MeasureLine_strip_Click(object sender, EventArgs e)
        {
            viewPort.Focus();
            viewPort.HalconWindow.SetTposition(10, 10);
            viewPort.HalconWindow.SetColor("red");
            viewPort.HalconWindow.WriteString("鼠标点击两个位置后,单击鼠标右键完成。");
            OnlyImage = true;
            viewPort.ContextMenuStrip = null;
            double r1, c1, r2, c2;
            HTuple dd;

            //获取当前显示信息
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            int hv_lineWidth;
            HWindow window = viewPort.HalconWindow;
            window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);

            hv_lineWidth = (int)window.GetLineWidth();
            string hv_Draw = window.GetDraw();
            window.SetLineWidth(hv_lineWidth);//设置线宽
            window.SetLineStyle(new HTuple());
            window.SetColor("green");//画点的颜色

            window.DrawLine(out r1, out c1, out r2, out c2);
            window.DispLine(r1, c1, r2, c2);
            //恢复窗口显示信息
            window.SetRgb(hv_Red, hv_Green, hv_Blue);
            window.SetLineWidth(hv_lineWidth);
            window.SetDraw(hv_Draw);

            HOperatorSet.DistancePp(r1, c1, r2, c2, out dd);
            double dr = Math.Abs(r2 - r1);
            double dc = Math.Abs(c2 - c1);
            MessageBox.Show(string.Format("直线距离{0:f2}px\rx轴距离{1:f2}px\ry轴距离{2:f2}px", dd.D, dc, dr), "结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //hv_MenuStrip.Visible = true;
            viewPort.ContextMenuStrip = hv_MenuStrip;
            OnlyImage = false;
            repaint();
        }
        private void MeasureWidth_strip_Click(object sender, EventArgs e)
        {
            viewPort.Focus();
            viewPort.HalconWindow.SetTposition(10, 10);
            viewPort.HalconWindow.SetColor("red");
            viewPort.HalconWindow.WriteString("鼠标点击两个位置后,单击鼠标右键完成。");
            OnlyImage = true;
            viewPort.ContextMenuStrip = null;
            double r1, c1, r2, c2;

            //获取当前显示信息
            HTuple hv_Red = null, hv_Green = null, hv_Blue = null;
            int hv_lineWidth;
            HWindow window = viewPort.HalconWindow;
            window.GetRgb(out hv_Red, out hv_Green, out hv_Blue);

            hv_lineWidth = (int)window.GetLineWidth();
            string hv_Draw = window.GetDraw();
            window.SetLineWidth(hv_lineWidth);//设置线宽
            window.SetLineStyle(new HTuple());
            window.SetColor("green");//画点的颜色

            window.DrawLine(out r1, out c1, out r2, out c2);
            window.DispLine(r1, c1, r2, c2);
            //恢复窗口显示信息
            window.SetRgb(hv_Red, hv_Green, hv_Blue);
            window.SetLineWidth(hv_lineWidth);
            window.SetDraw(hv_Draw);
            HOperatorSet.DistancePp(r1, c1, r2, c2, out HTuple distance);

            MessageBox.Show(string.Format("测量距离:" + distance.D.ToString("f1")), "结果", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //hv_MenuStrip.Visible = true;
            viewPort.ContextMenuStrip = hv_MenuStrip;
            OnlyImage = false;
            repaint();
        }
        public void RotateF_strip_Click(object sender, EventArgs e)
        {
            HImage image = ((HImage)((HObjectEntry)HObjList[0]).HObj).RotateImage(90d, "constant");
            this.addIconicVar(image.CopyImage());
            repaint();
        }
        public void RotateN_strip_Click(object sender, EventArgs e)
        {
            HImage image = ((HImage)((HObjectEntry)HObjList[0]).HObj).RotateImage(-90d, "constant");
            this.addIconicVar(image.CopyImage());
            repaint();
        }
        HTuple GetGrayHisto(HTuple rectangle1)
        {
            if (HObjList == null || HObjList.Count < 1)
            {
                return null;
            }
            HObjectEntry entry = ((HObjectEntry)HObjList[0]);
            HImage hv_image = entry.HObj as HImage;

            if (hv_image != null)
            {
                try
                {
                    HTuple hv_AbsoluteHisto, hv_RelativeHisto;

                    HTuple channel = hv_image.CountChannels();
                    HImage imgTmp = null;
                    if (channel == 3)
                    {
                        imgTmp = hv_image.Rgb1ToGray();
                    }
                    else
                    {
                        imgTmp = hv_image.Clone();
                    }
                    HRegion region = new HRegion();
                    region.GenRectangle1(rectangle1[0].D, rectangle1[1], rectangle1[2], rectangle1[3]);
                    hv_AbsoluteHisto = imgTmp.GrayHisto(region, out hv_RelativeHisto);
                    return hv_AbsoluteHisto;
                }
                catch (Exception)
                {

                }
            }
            return null;
        }
        public void useROIController(ROIController rC)
        {
            roiManager = rC;
            rC.setViewController(this);
        }

        private void setImagePart(HImage image)
        {
            string s;
            int w, h;

            image.GetImagePointer1(out s, out w, out h);
            setImagePart(0, 0, h, w);
        }
        private void setImagePart(int r1, int c1, int r2, int c2)
        {
            ImgRow1 = r1;
            ImgCol1 = c1;
            ImgRow2 = imageHeight = r2;
            ImgCol2 = imageWidth = c2;

            System.Drawing.Rectangle rect = viewPort.ImagePart;
            rect.X = (int)ImgCol1;
            rect.Y = (int)ImgRow1;
            rect.Height = (int)imageHeight;
            rect.Width = (int)imageWidth;
            viewPort.ImagePart = rect;
        }
        public void setViewState(int mode)
        {
            stateView = mode;

            if (roiManager != null)
                roiManager.resetROI();
        }
        private void dummy(int val)
        {
        }

        private void dummyV()
        {
        }
        private void exceptionGC(string message)
        {
            exceptionText = message;
            NotifyIconObserver(ERR_DEFINING_GC);
        }
        public void setDispLevel(int mode)
        {
            dispROI = mode;
        }
        public void zoomImage(double x, double y, double scale)
        {
            double lengthC, lengthR;
            double percentC, percentR;
            int lenC, lenR;

            //行比例
            percentC = (x - ImgCol1) / (ImgCol2 - ImgCol1);
            //列比例
            percentR = (y - ImgRow1) / (ImgRow2 - ImgRow1);

            //行长度
            lengthC = (ImgCol2 - ImgCol1) * scale;
            //列长度
            lengthR = (ImgRow2 - ImgRow1) * scale;

            ImgCol1 = x - lengthC * percentC;
            ImgCol2 = x + lengthC * (1 - percentC);

            ImgRow1 = y - lengthR * percentR;
            ImgRow2 = y + lengthR * (1 - percentR);

            lenC = (int)Math.Round(lengthC);
            lenR = (int)Math.Round(lengthR);

            System.Drawing.Rectangle rect = viewPort.ImagePart;
            rect.X = (int)Math.Round(ImgCol1);
            rect.Y = (int)Math.Round(ImgRow1);
            rect.Width = (lenC > 0) ? lenC : 1;
            rect.Height = (lenR > 0) ? lenR : 1;
            viewPort.ImagePart = rect;
            zoomWndFactor *= scale;
            if (viewPort.ImagePart.Width < imageWidth)
            {
                SmallWindow.Visible = true;
            }
            else
            {
                SmallWindow.Visible = false;

            }
            repaint();
        }
        public void zoomImage(double scaleFactor)
        {
            double midPointX, midPointY;

            if (((ImgRow2 - ImgRow1) == scaleFactor * imageHeight) &&
                ((ImgCol2 - ImgCol1) == scaleFactor * imageWidth))
            {
                repaint();
                return;
            }

            ImgRow2 = ImgRow1 + imageHeight;
            ImgCol2 = ImgCol1 + imageWidth;

            midPointX = ImgCol1;
            midPointY = ImgRow1;

            zoomWndFactor = (double)imageWidth / viewPort.Width;
            zoomImage(midPointX, midPointY, scaleFactor);
        }
        public void scaleWindow(double scale)
        {
            ImgRow1 = 0;
            ImgCol1 = 0;

            ImgRow2 = imageHeight;
            ImgCol2 = imageWidth;

            viewPort.Width = (int)(ImgCol2 * scale);
            viewPort.Height = (int)(ImgRow2 * scale);

            zoomWndFactor = ((double)imageWidth / viewPort.Width);
        }
        public void setZoomWndFactor()
        {
            zoomWndFactor = ((double)imageWidth / viewPort.Width);
        }
        public void setZoomWndFactor(double zoomF)
        {
            zoomWndFactor = zoomF;
        }
        private void moveImage(double motionX, double motionY)
        {
            ImgRow1 += -motionY;
            ImgRow2 += -motionY;

            ImgCol1 += -motionX;
            ImgCol2 += -motionX;

            System.Drawing.Rectangle rect = viewPort.ImagePart;
            rect.X = (int)Math.Round(ImgCol1);
            rect.Y = (int)Math.Round(ImgRow1);
            viewPort.ImagePart = rect;

            repaint();
        }
        public void resetAll()
        {
            ImgRow1 = 0;
            ImgCol1 = 0;
            ImgRow2 = imageHeight;
            ImgCol2 = imageWidth;

            zoomWndFactor = (double)imageWidth / viewPort.Width;

            System.Drawing.Rectangle rect = viewPort.ImagePart;
            rect.X = (int)ImgCol1;
            rect.Y = (int)ImgRow1;
            rect.Width = (int)imageWidth;
            rect.Height = (int)imageHeight;
            viewPort.ImagePart = rect;


            if (roiManager != null)
                roiManager.reset();
        }

        public void resetWindow()
        {
            ImgRow1 = 0;
            ImgCol1 = 0;
            ImgRow2 = imageHeight;
            ImgCol2 = imageWidth;

            zoomWndFactor = (double)imageWidth / viewPort.Width;

            System.Drawing.Rectangle rect = viewPort.ImagePart;
            rect.X = (int)ImgCol1;
            rect.Y = (int)ImgRow1;
            rect.Width = (int)imageWidth;
            rect.Height = (int)imageHeight;
            viewPort.ImagePart = rect;
        }
        public void mouseWheel(object sender, HalconDotNet.HMouseEventArgs e)
        {

            setViewState(MODE_VIEW_ZOOM);
            //double scale;
            //if (e.Delta >= 0)
            //    scale = 0.9;
            //else
            //    scale = 1 / 0.9;
            //zoomImage(e.X, e.Y, scale);

            //mousePressed = true;
            int activeROIidx = -1;
            double scale;

            if (roiManager != null && (dispROI == MODE_INCLUDE_ROI))
            {
                activeROIidx = roiManager.mouseDownAction(e.X, e.Y);
            }

            if (activeROIidx == -1)
            {
                switch (stateView)
                {
                    case MODE_VIEW_MOVE:
                        startX = e.X;
                        startY = e.Y;
                        break;
                    case MODE_VIEW_ZOOM:
                        if (e.Delta >= 0)
                            scale = 0.9;
                        else
                            scale = 1 / 0.9;
                        zoomImage(e.X, e.Y, scale);
                        break;
                    case MODE_VIEW_NONE:
                        break;
                    case MODE_VIEW_ZOOMWINDOW:
                        activateZoomWindow((int)e.X, (int)e.Y);
                        break;
                    default:
                        break;
                }
            }
        }
        public void mouseDown(object sender, HalconDotNet.HMouseEventArgs e)
        {

            if (e.Button == MouseButtons.Middle)
            {
                setViewState(HWndCtrl.MODE_VIEW_MOVE);
                Cursor.Current = Cursors.Hand;
            }


            mousePressed = true;
            int activeROIidx = -1;
            //double scale;

            if (roiManager != null && (dispROI == MODE_INCLUDE_ROI) && !OnlyImage)
            {
                activeROIidx = roiManager.mouseDownAction(e.X, e.Y);
            }

            if (activeROIidx == -1)
            {
                switch (stateView)
                {
                    case MODE_VIEW_MOVE:
                        startX = e.X;
                        startY = e.Y;
                        break;
                    //case MODE_VIEW_ZOOM:
                    //   if (e.Button == System.Windows.Forms.MouseButtons.Left)                         
                    //        scale = 0.9;
                    //    else
                    //        scale = 1 / 0.9;
                    //    zoomImage(e.X, e.Y, scale);
                    //    break;
                    case MODE_VIEW_NONE:
                        break;
                    case MODE_VIEW_ZOOMWINDOW:
                        activateZoomWindow((int)e.X, (int)e.Y);
                        break;
                    default:
                        break;
                }
            }
        }
        public void activateZoomWindow(int X, int Y)
        {
            double posX, posY;
            int zoomZone;

            if (ZoomWindow != null)
                ZoomWindow.Dispose();

            HOperatorSet.SetSystem("border_width", 10);
            ZoomWindow = new HWindow();

            posX = ((X - ImgCol1) / (ImgCol2 - ImgCol1)) * viewPort.Width;
            posY = ((Y - ImgRow1) / (ImgRow2 - ImgRow1)) * viewPort.Height;

            zoomZone = (int)((zoomWndSize / 2) * zoomWndFactor * zoomAddOn);
            ZoomWindow.OpenWindow((int)posY - (zoomWndSize / 2), (int)posX - (zoomWndSize / 2),
                                   zoomWndSize, zoomWndSize,
                                   viewPort.HalconID, "visible", "");
            ZoomWindow.SetPart(Y - zoomZone, X - zoomZone, Y + zoomZone, X + zoomZone);
            repaint(ZoomWindow);
            ZoomWindow.SetColor("black");
        }
        public void mouseUp(object sender, HalconDotNet.HMouseEventArgs e)
        {
            try
            {
                mousePressed = false;

                if (roiManager != null
                    && (roiManager.activeROIidx != -1)
                    && (dispROI == MODE_INCLUDE_ROI))
                {
                    roiManager.NotifyRCObserver(ROIController.EVENT_UPDATE_ROI);
                }
                else if (stateView == MODE_VIEW_ZOOMWINDOW)
                {
                    ZoomWindow.Dispose();
                }
            }
            catch { }
        }

        public void mouseMoved(object sender, HalconDotNet.HMouseEventArgs e)
        {
            try
            {
                double motionX, motionY;
                double posX, posY;
                double zoomZone;
                if (Keyboard.IsKeyPressed(Keyboard.VirtualKeyStates.VK_CONTROL))
                {
                    repaint();
                    HTuple grayval;
                    HOperatorSet.GetMposition(viewPort.HalconWindow, out HTuple row, out HTuple colum, out HTuple Button);
                    HOperatorSet.GetGrayval(this.GetImage(), row, colum, out grayval);
                    if (grayval.Length == 1)
                    {
                        HOperatorSet.DispText(viewPort.HalconWindow, "GRAY: " + grayval.D.ToString(), "window", "bottom", "left", "white",
           (new HTuple("box_color")).TupleConcat("shadow"), (new HTuple("orange")).TupleConcat(
           0));

                    }
                    else
                    {
                        HOperatorSet.DispText(viewPort.HalconWindow, "R:" +
                            grayval[0].D.ToString() + "  G:" + grayval[1].D.ToString()
                            + "  B:" + grayval[2].D.ToString(), "window", "bottom", "left", "white",
             (new HTuple("box_color")).TupleConcat("shadow"), (new HTuple("orange")).TupleConcat(0));

                    }
                }


                if (!mousePressed)
                    return;

                if (roiManager != null && (roiManager.activeROIidx != -1) && (dispROI == MODE_INCLUDE_ROI))
                {
                    roiManager.mouseMoveAction(e.X, e.Y);
                }
                else if (stateView == MODE_VIEW_MOVE)
                {
                    motionX = ((e.X - startX));
                    motionY = ((e.Y - startY));

                    if (((int)motionX != 0) || ((int)motionY != 0))
                    {
                        moveImage(motionX, motionY);
                        startX = e.X - motionX;
                        startY = e.Y - motionY;
                    }
                }
                else if (stateView == MODE_VIEW_ZOOMWINDOW)
                {
                    HSystem.SetSystem("flush_graphic", "false");
                    ZoomWindow.ClearWindow();
                    posX = ((e.X - ImgCol1) / (ImgCol2 - ImgCol1)) * viewPort.Width;
                    posY = ((e.Y - ImgRow1) / (ImgRow2 - ImgRow1)) * viewPort.Height;
                    zoomZone = (zoomWndSize / 2) * zoomWndFactor * zoomAddOn;
                    ZoomWindow.SetWindowExtents((int)posY - (zoomWndSize / 2),
                                                (int)posX - (zoomWndSize / 2),
                                                zoomWndSize, zoomWndSize);
                    ZoomWindow.SetPart((int)(e.Y - zoomZone), (int)(e.X - zoomZone),
                                       (int)(e.Y + zoomZone), (int)(e.X + zoomZone));
                    repaint(ZoomWindow);

                    HSystem.SetSystem("flush_graphic", "true");
                    ZoomWindow.DispLine(-100.0, -100.0, -100.0, -100.0);
                }
            }
            catch { }

        }
        public void setGUICompRangeX(int[] xRange, int Init)
        {
            int cRangeX;

            CompRangeX = xRange;
            cRangeX = xRange[1] - xRange[0];
            prevCompX = Init;
            stepSizeX = ((double)imageWidth / cRangeX) * (imageWidth / windowWidth);

        }
        public void setGUICompRangeY(int[] yRange, int Init)
        {
            int cRangeY;

            CompRangeY = yRange;
            cRangeY = yRange[1] - yRange[0];
            prevCompY = Init;
            stepSizeY = ((double)imageHeight / cRangeY) * (imageHeight / windowHeight);
        }
        public void resetGUIInitValues(int xVal, int yVal)
        {
            prevCompX = xVal;
            prevCompY = yVal;
        }
        public void moveXByGUIHandle(int valX)
        {
            double motionX;

            motionX = (valX - prevCompX) * stepSizeX;

            if (motionX == 0)
                return;

            moveImage(motionX, 0.0);
            prevCompX = valX;
        }
        public void moveYByGUIHandle(int valY)
        {
            double motionY;

            motionY = (valY - prevCompY) * stepSizeY;

            if (motionY == 0)
                return;

            moveImage(0.0, motionY);
            prevCompY = valY;
        }
        public void zoomByGUIHandle(double valF)
        {
            double x, y, scale;
            double prevScaleC;



            x = (ImgCol1 + (ImgCol2 - ImgCol1) / 2);
            y = (ImgRow1 + (ImgRow2 - ImgRow1) / 2);

            prevScaleC = (double)((ImgCol2 - ImgCol1) / imageWidth);
            scale = ((double)1.0 / prevScaleC * (100.0 / valF));

            zoomImage(x, y, scale);
        }
        public void repaint()
        {
            try
            {
                if (Show3D_strip.Checked)
                {
                    return;
                }
                repaint(viewPort.HalconWindow);
            }
            catch
            {


            }
        }
        public void repaint(HalconDotNet.HWindow window)
        {
            int count = HObjList.Count;
            HObjectEntry entry;
            HSystem.SetSystem("flush_graphic", "false");
            window.ClearWindow();
            mGC.stateOfSettings.Clear();
            if (isShowRainBow)
            {
                window.DispObj(RainBowImage);
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    entry = ((HObjectEntry)HObjList[i]);
                    if(entry!=null)
                    {
                        mGC.applyContext(window, entry.gContext);
                        window.DispObj(entry.HObj);
                        RefreshSmallWindow(entry.HObj, viewPort.ImagePart);
                    }
                   
                }
            }

            addInfoDelegate();
            if (!OnlyImage == true)
            {
                if (roiManager != null && (dispROI == MODE_INCLUDE_ROI))
                    roiManager.paintData(window);

            }
            //显示结果图
            if (ResultObj.IsInitialized())
            {
                window.SetColor("red");
                window.DispObj(ResultObj);
            }

            window.SetLineWidth(2);
            //window.SetColor("black");
            window.DispLine(-100.0, -100.0, -101.0, -101.0);
            window.SetColor("turquoise");
            //十字架
            if (CrossStatue)
            {
                HTuple row = (HTuple)imageHeight / 2;
                HTuple colum = (HTuple)imageWidth / 2;
                double kedu = (20.0 / 780.0) * imageHeight;
                window.SetLineWidth(2);
                window.DispLine(0, imageWidth / 2.0, imageHeight, imageWidth / 2.0);
                window.DispLine(imageHeight / 2.0, 0, imageHeight / 2.0, imageWidth);

                HObject circle;
                HOperatorSet.GenCircleContourXld(out circle, row, colum, kedu, 0, 6.28318, "positive", 1);
                window.DispObj(circle);
                circle.Dispose();

                //画横线
                window.DispLine(row - (kedu * 2), colum + (kedu / 2.0), row - (kedu * 2), colum - (kedu / 2.0));
                window.DispLine(row - (kedu * 3), colum + kedu, row - (kedu * 3), colum - kedu);
                window.DispLine(row + (kedu * 2), colum + (kedu / 2.0), row + (kedu * 2), colum - (kedu / 2.0));
                window.DispLine(row + (kedu * 3), colum + kedu, row + (kedu * 3), colum - kedu);

                //画竖线
                window.DispLine(row - (kedu / 2), colum - (kedu * 2), row + (kedu / 2), colum - (kedu * 2));
                window.DispLine(row - kedu, colum - (kedu * 3), row + kedu, colum - (kedu * 3));
                window.DispLine(row - (kedu / 2), colum + (kedu * 2), row + (kedu / 2), colum + (kedu * 2));
                window.DispLine(row - kedu, colum + (kedu * 3), row + kedu, colum + (kedu * 3));

            }
            HSystem.SetSystem("flush_graphic", "true");
        }
        public void addIconicVar(HObject obj)
        {
            HObjectEntry entry;

            if (obj == null)
                return;

            if (obj is HImage)
            {
                double r, c;
                int h, w, area;
                string s;
                area = ((HImage)obj).GetDomain().AreaCenter(out r, out c);
                ((HImage)obj).GetImagePointer1(out s, out w, out h);

                if (area == (w * h))
                {
                    clearList();

                    if ((h != imageHeight) || (w != imageWidth))
                    {
                        imageHeight = h;
                        imageWidth = w;
                        zoomWndFactor = (double)imageWidth / viewPort.Width;
                        setImagePart(0, 0, h, w);
                    }
                }
                AddSmallIco(obj);

            }

            entry = new HObjectEntry(obj, mGC.copyContextList());

            HObjList.Add(entry);

            if (HObjList.Count > MAXNUMOBJLIST)
                HObjList.RemoveAt(1);
        }

        private void AddSmallIco(HObject obj)
        {
            HTuple ImageWidth, ImageHeight;
            HOperatorSet.GetImageSize(obj, out ImageWidth, out ImageHeight);
            HOperatorSet.SetPart(SmallWindow.HalconWindow, 0, 0, ImageHeight, ImageWidth);
            HOperatorSet.DispObj(obj, SmallWindow.HalconWindow);
        }
        private void RefreshSmallWindow(HObject obj, Rectangle rec)
        {
            if (SmallWindow.Visible)
            {
                // viewPort.SendToBack();
                //   SmallWindow.BringToFront();
                AddSmallIco(obj);
                HOperatorSet.GenRectangle1(out HObject region, rec.Y, rec.X, rec.Y + rec.Height, rec.X + rec.Width);
                HObject ho_Domain, ho_RegionDifference;
                HOperatorSet.SetDraw(SmallWindow.HalconWindow, "fill");
                HOperatorSet.SetColor(SmallWindow.HalconWindow, "#bebebe40");
                HOperatorSet.GetDomain(obj, out ho_Domain);
                HOperatorSet.Difference(ho_Domain, region, out ho_RegionDifference);
                HOperatorSet.DispObj(ho_RegionDifference, SmallWindow.HalconWindow);
                HOperatorSet.SetDraw(SmallWindow.HalconWindow, "margin");
                HOperatorSet.SetColor(SmallWindow.HalconWindow, "turquoise");
                HOperatorSet.DispObj(region, SmallWindow.HalconWindow);
                ho_Domain.Dispose();
                region.Dispose();
                ho_RegionDifference.Dispose();
                //SmallWindow.HalconWindow.DispImage(new HImage("1.png"));
                //SmallWindow.Refresh();
                //SmallWindow.Focus();
            }
        }
        public void clearList()
        {
            HObjList.Clear();
        }
        public int getListCount()
        {
            return HObjList.Count;
        }
        public void changeGraphicSettings(string mode, string val)
        {
            switch (mode)
            {
                case GraphicsContext.GC_COLOR:
                    mGC.setColorAttribute(val);
                    break;
                case GraphicsContext.GC_DRAWMODE:
                    mGC.setDrawModeAttribute(val);
                    break;
                case GraphicsContext.GC_LUT:
                    mGC.setLutAttribute(val);
                    break;
                case GraphicsContext.GC_PAINT:
                    mGC.setPaintAttribute(val);
                    break;
                case GraphicsContext.GC_SHAPE:
                    mGC.setShapeAttribute(val);
                    break;
                default:
                    break;
            }
        }
        public void changeGraphicSettings(string mode, int val)
        {
            switch (mode)
            {
                case GraphicsContext.GC_COLORED:
                    mGC.setColoredAttribute(val);
                    break;
                case GraphicsContext.GC_LINEWIDTH:
                    mGC.setLineWidthAttribute(val);
                    break;
                default:
                    break;
            }
        }
        public void changeGraphicSettings(string mode, HTuple val)
        {
            switch (mode)
            {
                case GraphicsContext.GC_LINESTYLE:
                    mGC.setLineStyleAttribute(val);
                    break;
                default:
                    break;
            }
        }
        public void clearGraphicContext()
        {
            mGC.clear();
        }
        public Hashtable getGraphicContext()
        {
            return mGC.copyContextList();
        }
        public HImage GetImage()
        {
            if (HObjList.Count > 0)
            {
                HObjectEntry entry;
                entry = ((HObjectEntry)HObjList[0]);
                return (HImage)entry.HObj;
            }
            else
            {
                return null;
            }

        }
    }
}
