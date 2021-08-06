using System;
using System.Windows.Forms;
using System.Threading;
using HalconDotNet;
using System.IO;

namespace VisionControls
{
    public partial class ImageWindow : UserControl
    {
        #region"构造函数"
        public ImageWindow()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
            hWindow = hWindowControl.HalconWindow;

            hWindow.SetDraw("margin");
            hWindow.SetColor("red");
            hWindow.SetLineWidth(1);
            hWindow.SetWindowParam("background_color", "white");

            //hWindowControl.SizeChanged += new EventHandler((s, e) => { FitSize(); });
            //contextMenuStrip.MouseEnter += new EventHandler((s, e) => { hWindowControl.HMouseWheel -= hWindowControl_HMouseWheel; });
            //contextMenuStrip.MouseLeave += new EventHandler((s, e) => { hWindowControl.HMouseWheel += hWindowControl_HMouseWheel; });

            适合大小FToolStripMenuItem.Enabled = false;
            显示十字CToolStripMenuItem.Enabled = false;
            显示原图OToolStripMenuItem.Enabled = false;
            保存图像SToolStripMenuItem.Enabled = false;
            显示网格GToolStripMenuItem.Enabled = false;

        }
        #endregion

        #region"字段"
        private HImage image;//图像
        private HWindow hWindow;//HWindowControl.HalconWindow

        private int imgWidth;//图像的宽
        private int imgHeight;//图像的高

        //窗口和图像的宽高比
        private double winRatio;// 窗口的宽高比
        private double imgRatio; //图像的宽高比

        //鼠标按钮的当前状态
        private bool leftButton, rightButton;

        //设置图像在窗口显示的矩形区域
        private int setDispStartRow, setDispStartCol, setDispEndRow, setDispEndCol;

        //获取图像在窗口显示的矩形区域
        private int getDispStartRow, getDispStartCol, getDispEndRow, getDispEndCol;

        //鼠标按下时位置
        private double startMouseRow, startMouseCol;

        //鼠标的当前位置
        private double curMouseRow, curMouseCol;

        //区域的涂抹和擦除
        private bool isDrawing = false;//绘制中标志
        private bool isDrawModel = false;//绘制模型区域
        private int drawMode = 0;//0:绘制，1：涂抹
        private DateTime lastTime = DateTime.Now;
        private HRegion brushRegion = new HRegion();

        //图像信息标签(lblImageInfo)的相关信息
        private string imgGray;//灰度值
        private string mousePos;//鼠标位置
        private string imgSize;//图像尺寸

        #endregion

        #region"属性"
        /// <summary>
        /// 获取图像，设置图像并更新相关图像信息
        /// </summary>
        public HImage Image
        {
            get { return image; }
            set
            {
                image = value;
                if (image != null && value.IsInitialized())
                {
                    image.GetImageSize(out imgWidth, out imgHeight);
                    //imgSize = $"W:{imgWidth} H:{imgHeight}";
                    imgSize = string.Format("W:{0} H:{1}", imgWidth, imgHeight);
                    //
                    适合大小FToolStripMenuItem.Enabled = true;
                    显示十字CToolStripMenuItem.Enabled = true;
                    显示原图OToolStripMenuItem.Enabled = true;
                    保存图像SToolStripMenuItem.Enabled = true;
                    显示网格GToolStripMenuItem.Enabled = true;
                    //
                    hWindow.DispObj(image);
                    PaintCross();
                }
            }
        }

        public HWindow HWindow
        {
            get { return hWindow; }
        }

        public int StartRow
        {
            get { return setDispStartRow; }
        }

        public int StartCol
        {
            get { return setDispStartCol; }
        }

        public int EndRow
        {
            get { return setDispEndRow; }
        }

        public int EndCol
        {
            get { return setDispEndCol; }
        }
        /// <summary>
        /// Mask是否绘制中
        /// </summary>
        public bool IsDrawing
        {
            get { return isDrawing; }
        }

        public bool IsDrawModel
        {
            get { return isDrawModel; }
            set { isDrawModel = value; }
        }

        public int DrawMode
        {
            get { return drawMode; }
            set { drawMode = value; }
        }

        #endregion

        #region"事件"
        public event Action Repaint;


        #endregion

        #region"方法"
        /// <summary>
        /// 在图像中心显示十字形
        /// </summary>
        private void PaintCross()
        {
            if (显示十字CToolStripMenuItem.Checked)
            {
                hWindow.SetColor("red");
                hWindowControl.HalconWindow.DispLine(imgHeight / 2.0, 0, imgHeight / 2.0, imgWidth);
                hWindowControl.HalconWindow.DispLine(0, imgWidth / 2.0, imgHeight, imgWidth / 2.0);
            }
        }

        private void PaintModelRegion()
        {
            if (isDrawModel)
            {
                hWindow.SetColor("green");
            }
        }

        /// <summary>
        /// 显示大小适合窗口的图像
        /// </summary>
        public void FitSize()
        {
            try
            {
                if (image != null && image.IsInitialized())
                {
                    winRatio = (double)hWindowControl.WindowSize.Width / (double)hWindowControl.WindowSize.Height;
                    imgRatio = (double)imgWidth / (double)imgHeight;

                    if (winRatio >= imgRatio)
                    {
                        setDispStartRow = 0;
                        setDispStartCol = (int)(-imgWidth * (winRatio / imgRatio - 1d) / 2d);
                        setDispEndRow = imgHeight - 1;
                        setDispEndCol = (int)(imgWidth + imgWidth * (winRatio / imgRatio - 1d) / 2d);
                    }
                    else
                    {
                        setDispStartRow = (int)(-imgHeight * (imgRatio / winRatio - 1d) / 2d);
                        setDispStartCol = 0;
                        setDispEndRow = (int)(imgHeight + imgHeight * (imgRatio / winRatio - 1d) / 2d);
                        setDispEndCol = imgWidth - 1;
                    }

                    hWindow.SetWindowParam("flush", "false");
                    hWindowControl.HalconWindow.ClearWindow();
                    hWindow.SetPart(setDispStartRow, setDispStartCol, setDispEndRow, setDispEndCol);
                    hWindow.DispObj(image);
                    PaintCross();
                    Repaint?.Invoke();
                    hWindow.FlushBuffer();
                    hWindow.SetWindowParam("flush", "true");
                }
            }
            catch (Exception ex)
            {
                lblImageInfo.Text = ex.Message;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawingObject"></param>
        public void DrawingObject(HDrawingObject drawingObject)
        {
            try
            {
                hWindowControl.HMouseDown -= hWindowControl_HMouseDown;
                HWindow.AttachDrawingObjectToWindow(drawingObject);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
        private void hWindowControl_HMouseDown(object sender, HMouseEventArgs e)
        {
            try
            {
                int btnState;
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        if (isDrawing)//绘制模式
                        {
                            Cursor = Cursors.Default;
                            //if ((DateTime.Now - lastTime).TotalMilliseconds < 500)//待优化
                            //    drawMode = Math.Abs(drawMode - 1);
                            //lastTime = DateTime.Now;
                        }
                        else
                            Cursor = Cursors.Hand;

                        hWindow.GetMpositionSubPix(out startMouseRow, out startMouseCol, out btnState);
                        leftButton = true;
                        break;
                    case MouseButtons.Right:
                        rightButton = true;
                        Cursor = Cursors.Default;
                        break;
                    case MouseButtons.Middle:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                lblImageInfo.Text = ex.Message;
            }
        }

        private void hWindowControl_HMouseUp(object sender, HMouseEventArgs e)
        {
            try
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        Cursor = Cursors.Default;
                        leftButton = false;
                        break;
                    case MouseButtons.Right:
                        rightButton = false;
                        break;
                    case MouseButtons.Middle:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                lblImageInfo.Text = ex.Message;
            }
        }

        private void hWindowControl_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (image != null && image.IsInitialized())
            {
                try
                {
                    int btnState;
                    bool _isXOut = true, _isYOut = true;
                    HTuple channel_count = image.CountChannels();

                    hWindow.GetMpositionSubPix(out curMouseRow, out curMouseCol, out btnState);
                    //mousePos = $"R:{curMouseRow} C:{curMouseCol}";
                    mousePos = string.Format("R:{0:0.000}, C:{1:0.000}", curMouseRow, curMouseCol);

                    _isXOut = (curMouseCol < 0 || curMouseCol >= imgWidth);
                    _isYOut = (curMouseRow < 0 || curMouseRow >= imgHeight);

                    if (!_isXOut && !_isYOut)
                    {
                        try
                        {
                            if ((int)channel_count == 1)
                            {
                                double gray;
                                gray = image.GetGrayval((int)curMouseRow, (int)curMouseCol);
                                //imgGray = $"Gray:{gray} ";
                                imgGray = string.Format("Gray:{0:0.000}", gray);
                            }
                            else
                            {
                                imgGray = "";
                            }

                            lblImageInfo.Text = imgSize + "| " + mousePos + "| " + imgGray;

                        }
                        catch (Exception ex)
                        {
                            lblImageInfo.Text = ex.Message;
                        }

                        //当鼠标左键按下移动时，移动图片，并且鼠标变成手状
                        switch (btnState)
                        {
                            case 0:
                                Cursor = Cursors.Default;
                                break;
                            case 1:
                                if (leftButton)
                                {
                                    Cursor = Cursors.Hand;
                                    hWindow.SetWindowParam("flush", "false");
                                    hWindow.ClearWindow();
                                    hWindow.SetWindowParam("flush", "true");
                                    hWindow.SetPaint(new HTuple("default"));
                                    //保持图像显示比例
                                    setDispStartRow -= (int)(curMouseRow - startMouseRow);
                                    setDispStartCol -= (int)(curMouseCol - startMouseCol);
                                    setDispEndRow -= (int)(curMouseRow - startMouseRow);
                                    setDispEndCol -= (int)(curMouseCol - startMouseCol);

                                    hWindow.SetPart(setDispStartRow, setDispStartCol, setDispEndRow, setDispEndCol);
                                    hWindow.DispObj(image);
                                    PaintCross();
                                    Repaint?.Invoke();
                                }

                                break;
                            case 2:
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblImageInfo.Text = ex.Message;
                }
            }
        }



        private void hWindowControl_HMouseWheel(object sender, HMouseEventArgs e)
        {
            if (image != null && image.IsInitialized())
            {
                try
                {
                    int btnState;
                    hWindow.GetMpositionSubPix(out curMouseRow, out curMouseCol, out btnState);
                    hWindow.GetPart(out getDispStartRow, out getDispStartCol, out getDispEndRow, out getDispEndCol);
                }
                catch (Exception ex)
                {
                    lblImageInfo.Text = ex.Message;
                }

                if (e.Delta > 0)//放大图像
                {
                    setDispStartRow = (int)(getDispStartRow + (curMouseRow - getDispStartRow) * 0.100d);
                    setDispStartCol = (int)(getDispStartCol + (curMouseCol - getDispStartCol) * 0.100d);
                    setDispEndRow = (int)(getDispEndRow - (getDispEndRow - curMouseRow) * 0.100d);
                    setDispEndCol = (int)(getDispEndCol - (getDispEndCol - curMouseCol) * 0.100d);
                }
                else //缩小图像
                {
                    setDispStartRow = (int)(curMouseRow - (curMouseRow - getDispStartRow) / 0.900d);
                    setDispStartCol = (int)(curMouseCol - (curMouseCol - getDispStartCol) / 0.900d);
                    setDispEndRow = (int)(curMouseRow + (getDispEndRow - curMouseRow) / 0.900d);
                    setDispEndCol = (int)(curMouseCol + (getDispEndCol - curMouseCol) / 0.900d);
                }

                try
                {
                    int hw_width, hw_height;
                    hw_width = hWindowControl.WindowSize.Width;
                    hw_height = hWindowControl.WindowSize.Height;

                    bool _isOutOfArea = true;
                    bool _isOutOfSize = true;
                    bool _isOutOfPixel = true; //避免像素过大

                    _isOutOfArea = setDispStartRow >= imgHeight || setDispEndRow <= 0 || setDispStartCol >= imgWidth || setDispEndCol < 0;
                    _isOutOfSize = (setDispEndRow - setDispStartRow) > imgHeight * 20 || (setDispEndCol - setDispStartCol) > imgWidth * 20;
                    _isOutOfPixel = hw_height / (setDispEndRow - setDispStartRow) > 500 || hw_width / (setDispEndCol - setDispStartCol) > 500;

                    if (_isOutOfArea || _isOutOfSize)
                    {
                        FitSize();
                    }
                    else if (!_isOutOfPixel)
                    {
                        hWindow.SetWindowParam("flush", "false");
                        hWindow.ClearWindow();
                        hWindow.SetWindowParam("flush", "true");

                        //保持图像显示比例
                        setDispEndCol = setDispStartCol + (setDispEndRow - setDispStartRow) * hw_width / hw_height;
                        hWindow.SetPart(setDispStartRow, setDispStartCol, setDispEndRow, setDispEndCol);
                        hWindow.DispObj(image);
                    }
                    PaintCross();
                    Repaint?.Invoke();
                }
                catch (Exception ex)
                {
                    FitSize();
                    lblImageInfo.Text = ex.Message;
                }
            }
        }

        private void 适合大小FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FitSize();
        }

        private void 显示十字CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hWindow.DispObj(image);
            PaintCross();
            Repaint?.Invoke();
        }

        private void 显示原图OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (image != null && image.IsInitialized())
                    hWindow.DispObj(image);
            }
            catch
            {
            }
        }

        private void 保存图像SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.Filter = "PNG图像|*.png|BMP图像|*.bmp|JPEG图像|*.jpg|所有文件|*.*";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(sfd.FileName))
                        return;

                    string ext = Path.GetExtension(sfd.FileName);
                    //image.DumpWindowImage(hWindow);
                    image.WriteImage(ext.Substring(1), 0, sfd.FileName);

                }
            }
            catch (Exception ex)
            {
                lblImageInfo.Text = ex.Message;
            }
        }

        private void 保存带图形图形GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.Filter = "PNG图像|*.png|BMP图像|*.bmp|JPEG图像|*.jpg|所有文件|*.*";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(sfd.FileName))
                        return;

                    string ext = Path.GetExtension(sfd.FileName);
                    image.DumpWindowImage(hWindow);
                    image.WriteImage(ext.Substring(1), 0, sfd.FileName);

                }
            }
            catch (Exception ex)
            {
                lblImageInfo.Text = ex.Message;
            }
        }


        private void 显示网格GToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void hWindowControl_SizeChanged(object sender, EventArgs e)
        {
            FitSize();
        }

        private void contextMenuStrip_MouseEnter(object sender, EventArgs e)
        {
            hWindowControl.HMouseWheel -= hWindowControl_HMouseWheel;
        }

        private void contextMenuStrip_MouseLeave(object sender, EventArgs e)
        {
            hWindowControl.HMouseWheel += hWindowControl_HMouseWheel;
        }

        #region "绘制任意屏蔽区域"
        public HRegion SetROI(HRegion region)
        {
            try
            {
                hWindowControl.Focus();//必须先聚焦
                hWindowControl.HMouseMove -= hWindowControl_HMouseMove;
                isDrawing = true;

                double row = 0, col = 0;
                int btnState = 0;

                while (true)
                {
                    Application.DoEvents();//一直在循环,需要让halcon控件也响应事件,不然到时候跳出循环,之前的事件会一起爆发触发

                    try
                    {
                        // 1:Left button,2:Middle button,4:Right button,8:Shift key,16:Ctrl key,32:Alt key.
                        hWindow.GetMpositionSubPix(out row, out col, out btnState);//获取鼠标坐标，失败报异常
                        hWindow.GetPart(out getDispStartRow, out getDispStartCol, out getDispEndRow, out getDispEndCol);
                        //获取图像显示部分对角线长度
                        double diagonal = Math.Sqrt(Math.Pow(getDispEndRow - getDispStartRow, 2) + Math.Pow(getDispEndCol - getDispStartCol, 2));
                        //brushRegion.GenCircle(row, col, diagonal / 50.0);
                        brushRegion.GenRectangle1(row - diagonal / 100.0, col - diagonal / 100.0, row + diagonal / 100.0, col + diagonal / 100.0);
                    }
                    catch
                    {
                    }

                    //鼠标左键按下
                    if (row >= 0 && col >= 0 && btnState == 1)
                    {
                        switch (drawMode)
                        {
                            case 0://涂抹
                                if (region.IsInitialized())
                                    region = region.Union2(brushRegion);
                                else
                                    region = brushRegion;
                                break;
                            case 1://擦除
                                region = region.Difference(brushRegion);
                                break;
                            default:
                                break;
                        }
                    }

                    hWindow.SetDraw("fill");
                    hWindow.SetColor("red");
                    hWindow.SetWindowParam("flush", "false");
                    hWindow.ClearWindow();
                    hWindow.DispObj(image);
                    if (region != null && region.IsInitialized())
                        hWindow.DispObj(region);

                    if(drawMode==0)
                        hWindow.SetColor("red");
                    else
                        hWindow.SetColor("gray");

                    if (brushRegion != null && brushRegion.IsInitialized())
                        hWindow.DispObj(brushRegion);

                    Repaint?.Invoke();
                    hWindow.FlushBuffer();
                    hWindow.SetWindowParam("flush", "true");

                    if (drawMode == -1 || btnState==2)
                        break;
                    Thread.Sleep(1);
                }

                isDrawing = false;
                hWindowControl.HMouseMove += hWindowControl_HMouseMove;
                //hWindowControl.ContextMenuStrip = contextMenuStrip;
                return region;
            }
            catch
            {
                return null;
            }
        }


        #endregion

    }
}
