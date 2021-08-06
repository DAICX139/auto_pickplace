using AlcUtility;
using AlcUtility.PlcDriver.CommonCtrl;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionControls;
using VisionModules;

namespace VisionFlows.VisionCalculate
{
    public partial class FormLocationTest : Form
    {
        /// <summary>
        /// halcon 加载测试 图片
        /// </summary>
        private HImage image = new HImage();

        /// <summary>
        ///  定位 使用的模板
        /// </summary>
        private HShapeModel ShapeModel;

        /// <summary>
        /// 测试 批量处理图像时 图片序列
        /// </summary>
        private List<FileInfo> fileInfos = new List<FileInfo>();

        /// <summary>
        /// 批量测试图像处理 线程
        /// </summary>
        private Action ActionRunTask;

        /// <summary>
        /// 批量测试图像处理 线程
        /// </summary>
        private Task RunTask;

        /// <summary>
        /// 批量测试图像处理 模式
        /// </summary>
        private string testRunMode = "";

        /// <summary>
        /// 静态 定位结果
        /// </summary>
        private static LocationResult ResultStatic = new LocationResult();

        public FormLocationTest()
        {
            InitializeComponent();
        }

        private void FormLocationTest_Load(object sender, EventArgs e)
        {
            ShapeModel = new HShapeModel();
            this.numericUpDown_score_dutback.Value = Convert.ToDecimal(ImagePara.Instance.DutBackScore);
            this.numericUpDown_score_socktdut.Value = Convert.ToDecimal(ImagePara.Instance.SoketDutScore);
            this.numericUpDown_score_socktMark.Value = Convert.ToDecimal(ImagePara.Instance.SocketMarkScore);
            this.numericUpDown_score_Traydut.Value = Convert.ToDecimal(ImagePara.Instance.DutScore);
            this.numericUpDown_score_slot.Value = Convert.ToDecimal(ImagePara.Instance.SlotScore);
            numericUpDown_socktdut_max.Value = ImagePara.Instance.SoketDut_maxthreshold;
            numericUpDown_socktdut_min.Value = ImagePara.Instance.SoketDut_minthreshold;
            numericUpDown_trayDut_min.Value = ImagePara.Instance.TrayDut_minthreshold;
            numericUpDown_trayDut_max.Value = ImagePara.Instance.TrayDut_maxthreshold;
            numericUpDown_dut_width.Value = ImagePara.Instance.SoketDut_width;
            numericUpDown_dut_height.Value = ImagePara.Instance.SoketDut_height;

            this.superWind1.image = new HImage("byte", 100, 100);
            this.superWind1.roiController.addRec2(500, 500, "");
            this.superWind1.viewController.OnlyImage = true;
            this.superWind1.viewController.repaint();
        }

        /// <summary>
        /// 加载模型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoadShapeModelDutFront_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                ShapeModel.ReadShapeModel(openFileDialog.FileName);
                TBDutFrontModelPath.Text = openFileDialog.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Dut正面定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRunFunctionDutFront_Click(object sender, EventArgs e)
        {
            try
            {
                LocationResult result = new LocationResult();
                ShapeModel = new HShapeModel();
                LocationPara para = new LocationPara(this.image, this.ShapeModel, Convert.ToDouble(this.numericUpDown_score_Traydut.Value));
                result = ImageProcess.TrayDutFront(para);
                if (!result.IsRunOk)
                {
                    this.superWind1.image = para.image;
                    MessageBox.Show("DutFront 定位失败");
                    return;
                }
                this.superWind1.image = this.image;
                this.superWind1.obj = result.shapeModelContour;
                HXLDCont Cross = new HXLDCont();
                HOperatorSet.TupleRad(result.Angle, out HTuple rad);
                Cross.GenCrossContourXld(result.findPoint.Row, result.findPoint.Column, 200, rad);
                this.superWind1.obj = Cross;
                this.superWind1.obj = result.SmallestRec2Xld;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// DUT背面定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRunFunctionDutBack_Click(object sender, EventArgs e)
        {
            try
            {
                LocationPara locationPara = new LocationPara(image, this.ShapeModel, Convert.ToDouble(this.numericUpDown_score_dutback.Value));
                LocationResult locationResult = new LocationResult();
                ShapeModel.ReadShapeModel(Utility.Model + "SecondDutBack.sbm");
                locationResult = ImageProcess.SecondDutBack(locationPara, new PLCSend() { Func = 1 });
                if (locationResult.IsRunOk == false)
                {
                    this.superWind1.image = locationPara.image;
                    MessageBox.Show("Dut Back 定位失败");
                    return;
                }
                HXLDCont hXLDCont = new HXLDCont();
                hXLDCont.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 300, locationResult.Angle);
                this.superWind1.image = locationPara.image;
                this.superWind1.obj = hXLDCont;
                this.superWind1.obj = locationResult.shapeModelContour;
                this.superWind1.obj = locationResult.region;
                this.superWind1.obj = locationResult.SmallestRec2Xld;
            }
            catch (Exception ex)
            {
                Flow.Log(ex.Message);
            }
        }

        /// <summary>
        /// sockt  mark点定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRunFunctionSocketMark_Click(object sender, EventArgs e)
        {
            int kick = Environment.TickCount;

            LocationResult locationResult = new LocationResult();
            ShapeModel.ReadShapeModel(Utility.Model + "SocketMark_offe.sbm");
            LocationPara locationPara = new LocationPara(image, this.ShapeModel, Convert.ToDouble(this.numericUpDown_score_socktMark.Value));

            locationResult = ImageProcess.SocketMark_NewCalib(locationPara);
            if (locationResult.IsRunOk == false)
            {
                this.superWind1.image = locationPara.image;
                MessageBox.Show("Socket Mark 定位失败");
                return;
            }
            this.superWind1.image = locationPara.image;
            HXLDCont hXLDCont = new HXLDCont();
            hXLDCont.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 300, locationResult.Angle);

            this.superWind1.ObjColor = "green";
            this.superWind1.obj = hXLDCont;
        }

        /// <summary>
        /// 加载图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName == "")
            {
                return;
            }
            try
            {
                this.image = new HImage(openFileDialog.FileName);
                this.superWind1.image = new HImage(openFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// slot定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void BtnRunTraySlot_Click(object sender, EventArgs e)
        {
            LocationResult locationResult = new LocationResult();
            ShapeModel.ReadShapeModel(Utility.Model + "TraySlot.sbm");
            LocationPara locationPara = new LocationPara(image, this.ShapeModel, Convert.ToDouble(this.numericUpDown_score_slot.Value));

            locationResult = ImageProcess.ShotSlot(locationPara);
            this.superWind1.image = locationPara.image;

            if (!locationResult.IsRunOk)
            {
                MessageBox.Show("TraySlot定位失败");
                return;
            }
            this.superWind1.obj = locationResult.SmallestRec2Xld;
          //  this.superWind1.obj = locationResult.regionFitLine;

            HXLDCont cross = new HXLDCont();
            cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 200, locationResult.Angle);
            this.superWind1.obj = cross;
            this.superWind1.obj = locationResult.SmallestRec2Xld;
            //按照自定义的取料中心仿射变换
            HHomMat2D hv_HomMat2D = new HHomMat2D();
            if (locationResult.Angle < -0.79)
            {
                locationResult.Angle = 3.14159 + locationResult.Angle;
            }
            hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.slot_rowCenter, ImagePara.Instance.slot_colCenter,
                ImagePara.Instance.slot_angleCenter, locationResult.findPoint.Row, locationResult.findPoint.Column, locationResult.Angle);
            HTuple findrow = hv_HomMat2D.AffineTransPoint2d(ImagePara.Instance.slot_offe_rowCenter, ImagePara.Instance.slot_offe_colCenter, out HTuple findcol);
            locationResult.findPoint.Row = findrow;
            locationResult.findPoint.Column = findcol;
            cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 200, locationResult.Angle);
            this.superWind1.obj = cross;
            HRegion hRegionLine = new HRegion();
            hRegionLine.GenRegionLine(locationResult.hLine_Image.StartRow
                                        , locationResult.hLine_Image.StartColumn
                                        , locationResult.hLine_Image.StopRow
                                        , locationResult.hLine_Image.StopColumn);
         //   this.superWind1.obj = hRegionLine;
        }

        /// <summary>
        /// 打开路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnOpenFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (string.IsNullOrEmpty(dialog.SelectedPath))
                {
                    MessageBox.Show(this, "文件夹路径不能为空", "提示");
                    return;
                }
                DirectoryInfo info = new DirectoryInfo(dialog.SelectedPath);

                fileInfos.Clear();
                foreach (var item in info.GetFiles())
                {
                    fileInfos.Add(item);
                    LBShowImageList.Items.Add(item.Name.ToString());
                }
                LBResidueNum.Text = fileInfos.Count.ToString();
                MessageBox.Show("add files : " + fileInfos.Count);
                this.BtnRunTask.Enabled = true;
            }
        }

        /// <summary>
        /// 当前 实时的相机 编号
        /// </summary>
        private int LiveCameraIndex = 0;

        /// <summary>
        /// 相机实时 所开启的线程
        /// </summary>
        private Task TkCameraLive;

        /// <summary>
        /// 中止线程 所使用布尔量
        /// </summary>
        private bool IsLiveRun = true;

        private void BtnCameraLive_Click(object sender, EventArgs e)
        {
            if (CBCameraSelect.SelectedIndex < 0)
            {
                MessageBox.Show("开启失败");
                return;
            }

            if (BtnCameraLive.Text == "Live")
            {              
                Plc.SetIO(this.CBCameraSelect.SelectedIndex+1, true);
                BtnCameraLive.Text = "Stop";
                BtnGrabImageOnce.Enabled = false;

                BtnCameraLive.BackColor = Color.Red;
                LiveCameraIndex = CBCameraSelect.SelectedIndex;
                IsLiveRun = true;
                TkCameraLive = new Task(FunctionTkCameraLive);
                TkCameraLive.Start();
            }
            else
            {
                
                BtnCameraLive.Text = "Live";
                BtnGrabImageOnce.Enabled = true;
                BtnCameraLive.BackColor = Color.WhiteSmoke;
                IsLiveRun = false;
                System.Threading.Thread.Sleep(500);
                Plc.SetIO(this.CBCameraSelect.SelectedIndex + 1, false);
            }
        }

        private void FunctionTkCameraLive()
        {
            while (true)
            {
                GC.Collect();

                VisionModulesManager.CameraList[LiveCameraIndex].CaptureImage();
                VisionModulesManager.CameraList[LiveCameraIndex].CaptureSignal.WaitOne(Utility.CaptureDelayTime);

                this.image = VisionModulesManager.CameraList[LiveCameraIndex].Image;
                this.Invoke(new Action(() =>
                {
                    HOperatorSet.GetImageSize(image, out HTuple width, out HTuple height);
                    this.superWind1.image = image;
                }));

                if (!IsLiveRun)
                {
                    return;
                }
            }
        }

        private void BtnGrabImageOnce_Click(object sender, EventArgs e)
        {
            try
            {
                if (CBCameraSelect.SelectedIndex < 0)
                {
                    MessageBox.Show("开启失败");
                    return;
                }
                LiveCameraIndex = CBCameraSelect.SelectedIndex;
                Plc.SetIO(this.CBCameraSelect.SelectedIndex + 1, true);
                VisionModulesManager.CameraList[LiveCameraIndex].CaptureImage();
                VisionModulesManager.CameraList[LiveCameraIndex].CaptureSignal.WaitOne(Utility.CaptureDelayTime);

                this.image = VisionModulesManager.CameraList[LiveCameraIndex].Image;
                this.Invoke(new Action(() =>
                {
                    this.superWind1.image = image;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("相机 实时 失败");
            }
        }

        private void BtnSetExpose_Click(object sender, EventArgs e)
        {
            try
            {
                LiveCameraIndex = CBCameraSelect.SelectedIndex;

                double ExposeTime = double.Parse(TBExpose.Text);

                VisionModulesManager.CameraList[LiveCameraIndex].SetExposureTime(ExposeTime);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "曝光设置失败");
            }
        }

       

        private void TBExpose_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    LiveCameraIndex = CBCameraSelect.SelectedIndex;

                    double ExposeTime = double.Parse(TBExpose.Text);

                    VisionModulesManager.CameraList[LiveCameraIndex].SetExposureTime(ExposeTime);
                    MessageBox.Show("Set");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "曝光设置失败");
                }
            }
        }

        private void 点位窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Flow.FormPointPosition.Show();
        }

        private void 系统参数窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Flow.FrmParaSet.Show();
        }

        private void FormLocationTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Hide();
        }

        private void FormLocationTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.IsLiveRun = false;
            Flow.XmlHelper.SerializeToXml(Utility.Config + "ImagePara.xml", ImagePara.Instance);
            this.Close();
        }

        private void BtnStopThread_Click(object sender, EventArgs e)
        {
            //停止 图像序列遍历线程
            testRunMode = "";
        }

        private void CBIsWriteStationImage_CheckedChanged(object sender, EventArgs e)
        {
            if (CBIsWriteStationImage.Checked)
            {
                Utility.IsSaveImage = true;
                CBIsWriteStationImage.BackColor = Color.Fuchsia;
            }
            else
            {
                Utility.IsSaveImage = false;
                CBIsWriteStationImage.BackColor = Color.WhiteSmoke;
            }
        }

        private void BtnLocationSocketDutTest_Click(object sender, EventArgs e)
        {
            LocationResult locationResult = new LocationResult();
            ShapeModel = new HShapeModel();
            LocationPara locationPara = new LocationPara(image, this.ShapeModel, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));

            locationResult = ImageProcess.SocketDutFront(locationPara);
            this.superWind1.image = image;
            if (locationResult.IsRunOk == false)
            {
                MessageBox.Show("Socket Mark 定位失败");
                return;
            }
            HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Angle);
            //按照自定义的取料中心仿射变换
            HHomMat2D hv_HomMat2D = new HHomMat2D();
            if (locationResult.Angle < -0.79)
            {
                locationResult.Angle = 3.14159 + locationResult.Angle;
            }
            hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.DutMode_rowCenter, ImagePara.Instance.DutMode_colCenter,
                ImagePara.Instance.DutMode_angleCenter, locationResult.findPoint.Row, locationResult.findPoint.Column, locationResult.Angle);
            HTuple findrow = hv_HomMat2D.AffineTransPoint2d(ImagePara.Instance.SocketMark_GetDutRow, ImagePara.Instance.SocketMark_GetDutCol, out HTuple findcol);
            locationResult.findPoint.Row = findrow;
            locationResult.findPoint.Column = findcol;
            locationResult.IsRunOk = true;
            this.superWind1.obj = locationResult.region;
            this.superWind1.obj = locationResult.SmallestRec2Xld;
            this.superWind1.obj = locationResult.shapeModelContour;
            HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Angle);
        }

        private void 置于最上层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
        }

        private void 收缩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.Width == 900)
            {
                this.Width = 1381;
                panelRight.Dock = DockStyle.Right;
            }
            else
            {
                panelRight.Dock = DockStyle.None;
                this.Width = 900;
            }
        }

        /// <summary>
        /// 槽有无dut测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_isdut_Click(object sender, EventArgs e)
        {
            ShapeModel.ReadShapeModel(Utility.Model + "TraySlot.sbm");
            LocationPara para = new LocationPara(this.image, this.ShapeModel, Convert.ToDouble(this.numericUpDown_score_Traydut.Value));
            //判断是否有料
            if (!ImageProcess.SlotDetect1(para, out HObject yy))
            {
                this.superWind1.obj = yy;
                MessageBox.Show("无料");
                return;
            }
            this.superWind1.obj = yy;
            MessageBox.Show("有料");
        }

        /// <summary>
        /// socket 有无dut测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button_issocket_Click(object sender, EventArgs e)
        {
        }

        private void numericUpDown_socktdut_min_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_socktdut_min.Value >= numericUpDown_socktdut_max.Value)
            {
                numericUpDown_socktdut_min.Value = numericUpDown_socktdut_max.Value - 1;
            }
            ImagePara.Instance.SoketDut_minthreshold = (HTuple)numericUpDown_socktdut_min.Value;
        }

        private void numericUpDown_socktdut_max_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_socktdut_min.Value >= numericUpDown_socktdut_max.Value)
            {
                numericUpDown_socktdut_max.Value = numericUpDown_socktdut_min.Value + 1;
            }
            ImagePara.Instance.SoketDut_maxthreshold = (HTuple)numericUpDown_socktdut_max.Value;
        }

        private void numericUpDown_dut_width_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.SoketDut_width = (HTuple)numericUpDown_dut_width.Value;
        }

        private void numericUpDown_dut_height_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.SoketDut_height = (HTuple)numericUpDown_dut_height.Value;
        }

        private void numericUpDown_score_Traydut_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.DutScore = Convert.ToSingle(numericUpDown_score_Traydut.Value);
        }

        private void numericUpDown_score_dutback_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.DutBackScore = Convert.ToSingle(numericUpDown_score_dutback.Value);
        }

        private void numericUpDown_score_slot_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.SlotScore = Convert.ToSingle(numericUpDown_score_slot.Value);
        }

        private void numericUpDown_score_socktMark_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.SocketMarkScore = Convert.ToSingle(numericUpDown_score_socktMark.Value);
        }

        private void numericUpDown_score_socktdut_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.SoketDutScore = Convert.ToSingle(numericUpDown_score_socktdut.Value);
        }

        private void button_edit_putcenter_Click(object sender, EventArgs e)
        {
            if (button_edit_putcenter.Text == "编辑放料中心")
            {
                button_edit_putcenter.BackColor = Color.Red;
                this.superWind1.viewController.OnlyImage = false;
                button_edit_putcenter.Text = "结束编辑";
                this.superWind1.viewController.repaint();
            }
            else
            {
                button_edit_putcenter.Text = "编辑放料中心";
                button_edit_putcenter.BackColor = Color.WhiteSmoke;
                this.superWind1.viewController.OnlyImage = true;
                this.superWind1.viewController.repaint();
                ImageProcess.FindSocketMark(superWind1.image, out HTuple row_center, out HTuple col_center, out HTuple phi);
                HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, row_center, col_center, 100, phi);

                //mark点模板坐标
                ImagePara.Instance.SocketMark_rowCenter = Convert.ToSingle(row_center.D);
                ImagePara.Instance.SocketMark_colCenter = Convert.ToSingle(col_center.D);
                ImagePara.Instance.SocketMark_angleCenter = Convert.ToSingle(phi.D);

                //放料点模板坐标((ROIRectangle1)(superWind1.roiController.RoiInfo.ROIList[0])).getRegion();
                ImagePara.Instance.SocketMark_PutDutRow = Convert.ToSingle(((ROIRectangle2)(superWind1.roiController.RoiInfo.ROIList[0])).midR);
                ImagePara.Instance.SocketMark_PutDutCol = Convert.ToSingle(((ROIRectangle2)(superWind1.roiController.RoiInfo.ROIList[0])).midC);
                HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, ImagePara.Instance.SocketMark_PutDutRow,
                    ImagePara.Instance.SocketMark_PutDutCol, 100, phi);
            }
        }

        private void button_editget_Click(object sender, EventArgs e)
        {
            if (button_editget.Text == "2.编辑取料中心")
            {
                button_editget.BackColor = Color.Red;
                this.superWind1.viewController.OnlyImage = false;
                button_editget.Text = "结束编辑";
                this.superWind1.viewController.repaint();
            }
            else
            {
                button_editget.Text = "2.编辑取料中心";
                button_editget.BackColor = Color.WhiteSmoke;
                this.superWind1.viewController.OnlyImage = true;
                this.superWind1.viewController.repaint();
                LocationResult locationResult = new LocationResult();
                LocationPara locationPara = new LocationPara(image, this.ShapeModel, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));
          
                locationResult = ImageProcess.SocketDutFront(locationPara);
                this.superWind1.image = image;
                if (locationResult.IsRunOk == false)
                {
                    MessageBox.Show("定位失败");
                    return;
                }
                HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, locationResult.findPoint.Row,
                    locationResult.findPoint.Column, 100, locationResult.Angle);

               

                //料中心模板坐标
                ImagePara.Instance.DutMode_rowCenter = Convert.ToSingle(locationResult.findPoint.Row);
                ImagePara.Instance.DutMode_colCenter = Convert.ToSingle(locationResult.findPoint.Column);
                ImagePara.Instance.DutMode_angleCenter = Convert.ToSingle(locationResult.Angle);

                //放料点模板坐标((ROIRectangle1)(superWind1.roiController.RoiInfo.ROIList[0])).getRegion();
                ImagePara.Instance.SocketMark_GetDutRow = Convert.ToSingle(((ROIRectangle2)(superWind1.roiController.RoiInfo.ROIList[0])).midR);
                ImagePara.Instance.SocketMark_GetDutCol = Convert.ToSingle(((ROIRectangle2)(superWind1.roiController.RoiInfo.ROIList[0])).midC);
                //新取料中心
                HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, ImagePara.Instance.SocketMark_GetDutRow,
                    ImagePara.Instance.SocketMark_GetDutCol, 100, locationResult.Angle);
                this.superWind1.obj = locationResult.region;
                this.superWind1.obj = locationResult.SmallestRec2Xld;
                this.superWind1.obj = locationResult.shapeModelContour;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool result = ImageProcess.SocketDetect(image, out HObject obj);
            this.superWind1.image = image;
            this.superWind1.obj = obj;
            if (result)
            {
                MessageBox.Show("有料");
            }
            else
            {
                MessageBox.Show("无料");
            }
        }

        private void button_serch_Click(object sender, EventArgs e)
        {
            if (button_serch.Text == "1.编辑搜索区域")
            {
                button_serch.BackColor = Color.Red;
               // this.superWind1.viewController.OnlyImage = false;
                button_serch.Text = "结束编辑";
                this.superWind1.viewController.repaint();
                this.superWind1.isDrawUserRegion = true;
                this.superWind1.roiController.m_pen = 11;
            }
            else
            {
                ImageProcess.FindSocketMark(image, out HTuple Row_mark, out HTuple Column_mark, out HTuple Phi_mark);
                //mark点中心
                ImagePara.Instance.SocketGet_rowCenter = Convert.ToSingle(Row_mark.D);
                ImagePara.Instance.SocketGet_colCenter = Convert.ToSingle(Column_mark.D);
                ImagePara.Instance.SocketGet_angleCenter = Convert.ToSingle(Phi_mark.D);

                button_serch.Text = "1.编辑搜索区域";
                button_serch.BackColor = Color.WhiteSmoke;
                this.superWind1.isDrawUserRegion = false;
                HOperatorSet.WriteObject(((ROI)(superWind1.roiController.RoiInfo.ROIList[1])).getRegion(), Utility.Model + "SerchROI.hobj");
                superWind1.roiController.RoiInfo.ROIList.RemoveAt(1);
                superWind1.roiController.RoiInfo.RemarksList.RemoveAt(1);
            }
        }

        private void button_editSlot_Click(object sender, EventArgs e)
        {
            if (button_editSlot.Text == "编辑放料中心")
            {
                button_editSlot.BackColor = Color.Red;
                this.superWind1.viewController.OnlyImage = false;
                button_editSlot.Text = "结束编辑";
                this.superWind1.viewController.repaint();
            }
            else
            {
                button_editSlot.Text = "编辑放料中心";
                button_editSlot.BackColor = Color.WhiteSmoke;
                this.superWind1.viewController.OnlyImage = true;
                this.superWind1.viewController.repaint();
                LocationResult locationResult = new LocationResult();
                LocationPara locationPara = new LocationPara(image, this.ShapeModel, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));

                locationResult = ImageProcess.ShotSlot(locationPara);
                this.superWind1.image = image;
                if (locationResult.IsRunOk == false)
                {
                    MessageBox.Show("定位失败");
                    return;
                }
                HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, locationResult.findPoint.Row,
                    locationResult.findPoint.Column, 100, locationResult.Angle);



                //料中心模板坐标
                ImagePara.Instance.slot_rowCenter = Convert.ToSingle(locationResult.findPoint.Row);
                ImagePara.Instance.slot_colCenter = Convert.ToSingle(locationResult.findPoint.Column);
                ImagePara.Instance.slot_angleCenter = Convert.ToSingle(locationResult.Angle);

                //放料点模板坐标((ROIRectangle1)(superWind1.roiController.RoiInfo.ROIList[0])).getRegion();
                ImagePara.Instance.slot_offe_rowCenter = Convert.ToSingle(((ROIRectangle2)(superWind1.roiController.RoiInfo.ROIList[0])).midR);
                ImagePara.Instance.slot_offe_colCenter = Convert.ToSingle(((ROIRectangle2)(superWind1.roiController.RoiInfo.ROIList[0])).midC);
                //新取料中心
                HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, ImagePara.Instance.slot_offe_rowCenter,
                    ImagePara.Instance.slot_offe_colCenter, 100, locationResult.Angle);
                this.superWind1.obj = locationResult.region;
                this.superWind1.obj = locationResult.SmallestRec2Xld;
                this.superWind1.obj = locationResult.shapeModelContour;
            }
        }

        private void button_messure_Click(object sender, EventArgs e)
        {
            this.superWind1.Message = "请画出要测量的长度!";
            superWind1.viewController.viewPort.ContextMenuStrip = null;
            HOperatorSet.GetImageSize(image, out HTuple width, out HTuple height);
            HOperatorSet.DrawLine(this.superWind1.viewController.viewPort.HalconWindow,out HTuple row1,out HTuple col1,
                out HTuple row2,out HTuple col2);
            HOperatorSet.DistancePp(row1, col1, row2, col2, out HTuple distance_pix);
            if ((width.D* height.D)>11000000)
            {
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1,100,0,out HTuple x1,out HTuple y1);
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, 100, 100, out HTuple x2, out HTuple y2);
                HOperatorSet.DistancePp(x1, y1, x2, y2, out HTuple distance_plc);
                MessageBox.Show("测量宽度:" + (distance_pix.D * distance_plc.D / 100).ToString("f3")+"mm");
            }
            else
            {
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_up1, 100, 0, out HTuple x1, out HTuple y1);
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_up1, 100, 100, out HTuple x2, out HTuple y2);
                HOperatorSet.DistancePp(x1, y1, x2, y2, out HTuple distance_plc);
                MessageBox.Show("测量宽度:"+ (distance_pix .D* distance_plc.D/100).ToString("f3")+ "mm");
            }
            superWind1.viewController.viewPort.ContextMenuStrip = superWind1.viewController.hv_MenuStrip;
        }

        private void numericUpDown_trayDut_min_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_trayDut_min.Value >= numericUpDown_trayDut_max.Value)
            {
                numericUpDown_trayDut_min.Value = numericUpDown_trayDut_max.Value - 1;
            }
            ImagePara.Instance.TrayDut_minthreshold = (HTuple)numericUpDown_trayDut_min.Value;
        }

        private void numericUpDown_trayDut_max_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_trayDut_min.Value >= numericUpDown_trayDut_max.Value)
            {
                numericUpDown_trayDut_max.Value = numericUpDown_trayDut_min.Value + 1;
            }
            ImagePara.Instance.TrayDut_maxthreshold = (HTuple)numericUpDown_trayDut_max.Value;
        }
    }
}