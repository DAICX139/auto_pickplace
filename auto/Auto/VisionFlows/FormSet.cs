using AlcUtility;
using AlcUtility.PlcDriver.CommonCtrl;
using HalconDotNet;
using Microsoft.VisualBasic;
using Poc2Auto.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionControls;
using VisionModules;

namespace VisionFlows.VisionCalculate
{
    public partial class FormLocationSet : Form
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

        ArrayList EditRoi =new ArrayList();

        public FormLocationSet()
        {
            InitializeComponent();
        }

        private void FormLocationTest_Load(object sender, EventArgs e)
        {
            comboBox_socket.SelectedIndex = 0;
            ShapeModel = new HShapeModel();
            this.numericUpDown_score_dutback.Value = Convert.ToDecimal(ImagePara.Instance.DutBackScore);
            this.numericUpDown_score_socktdut.Value = Convert.ToDecimal(ImagePara.Instance.SoketDutScore);
            this.numericUpDown_score_socktMark.Value = Convert.ToDecimal(ImagePara.Instance.SocketMarkScore);
            this.numericUpDown_score_Traydut.Value = Convert.ToDecimal(ImagePara.Instance.DutScore);
            this.numericUpDown_score_slot.Value = Convert.ToDecimal(ImagePara.Instance.SlotScore);
            numericUpDown_socktdut_max.Value = ImagePara.Instance.SoketDut_maxthreshold[0];
            numericUpDown_socktdut_min.Value = ImagePara.Instance.SoketDut_minthreshold[0];
            numericUpDown_trayDut_min.Value = ImagePara.Instance.TrayDut_minthreshold;
            numericUpDown_trayDut_max.Value = ImagePara.Instance.TrayDut_maxthreshold;
            numericUpDown_dut_widthmin.Value = ImagePara.Instance.SoketDut_widthmin;
            numericUpDown_dut_heightmin.Value = ImagePara.Instance.SoketDut_heightmin;
            numericUpDown_dut_widthmax.Value = ImagePara.Instance.SoketDut_widthmax;
            numericUpDown_dut_heightmax.Value = ImagePara.Instance.SoketDut_heightmax;
            numericUpDown_socktmark_min.Value = ImagePara.Instance.SoketMark_minthreshold;
            numericUpDown_socktmark_max.Value = ImagePara.Instance.SoketMark_maxthreshold;
            numericUpDown_PushBlock.Value= ImagePara.Instance.PushBlock;
            CBIsWriteStationImage.Checked = Utility.IsSaveImage;
            this.superWind1.image = new HImage("byte", 100, 100);
            this.superWind1.roiController.addRec2(500, 500, "");
            EditRoi= (ArrayList)this.superWind1.roiController.RoiInfo.ROIList.Clone();
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
                OutPutResult result = new OutPutResult();
                ShapeModel = new HShapeModel();
                HRegion roi = ImagePara.Instance.GetSerchROI("SlotDutROI");
                InputPara para = new InputPara(this.image, roi, this.ShapeModel, Convert.ToDouble(this.numericUpDown_score_Traydut.Value));
                result = AutoNormal_New.ImageProcess.TrayDutFront(para);
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
                this.superWind1.obj = roi;
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
                HRegion roi = ImagePara.Instance.GetSerchROI("DutBackROI");
                ShapeModel.ReadShapeModel(Utility.Model + "SecondDutBack.sbm");
                InputPara locationPara = new InputPara(image, roi, this.ShapeModel, Convert.ToDouble(this.numericUpDown_score_dutback.Value));
                OutPutResult locationResult = new OutPutResult();
               
                locationResult = AutoNormal_New.ImageProcess.SecondDutBack(locationPara, new PLCSend() { Func = 1 });
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
                this.superWind1.obj = locationResult.SmallestRec2Xld;
                this.superWind1.obj = roi;

            }
            catch (Exception ex)
            {
                Flow.Log(ex.Message);
                MessageBox.Show("Dut Back 定位失败");
            }
        }

        /// <summary>
        /// sockt  mark点定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnRunFunctionSocketMark_Click(object sender, EventArgs e)
        {
            OutPutResult locationResult = new OutPutResult();
            HRegion roi = ImagePara.Instance.GetSerchROI("SocketMarkROI");
            InputPara locationPara = new InputPara(image, roi, this.ShapeModel, Convert.ToDouble(this.numericUpDown_score_socktMark.Value));

            locationResult = AutoNormal_New.ImageProcess.SocketMark(locationPara, out HObject mark);
            if (locationResult.IsRunOk == false)
            {
                this.superWind1.image = locationPara.image;
                MessageBox.Show("Socket Mark 定位失败");
                return;
            }
            this.superWind1.obj = mark;
            mark.Dispose();

            HXLDCont hXLDCont = new HXLDCont();
            hXLDCont.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 300, locationResult.Angle);

            this.superWind1.ObjColor = "green";
            this.superWind1.obj = hXLDCont;
            this.superWind1.obj = roi;
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
            OutPutResult locationResult = new OutPutResult();
            ShapeModel.ReadShapeModel(Utility.Model + "TraySlot.sbm");
            HRegion roi = ImagePara.Instance.GetSerchROI("SlotROI");
            InputPara locationPara = new InputPara(image, roi, this.ShapeModel, Convert.ToDouble(this.numericUpDown_score_slot.Value));
            //
            locationResult = AutoNormal_New.ImageProcess.ShotSlot(locationPara);
            this.superWind1.image = locationPara.image;
            if (!locationResult.IsRunOk)
            {
                MessageBox.Show("TraySlot定位失败");
                return;
            }
            //模板匹配结果及其外接矩形轮廓显示
            this.superWind1.obj = locationResult.SmallestRec2Xld;
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
            locationResult.findPoint.Row = findrow;//最后取料中心
            locationResult.findPoint.Column = findcol;
            cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 200, locationResult.Angle);
            this.superWind1.ObjColor = "red";
            this.superWind1.obj = cross;
            this.superWind1.obj = roi;
            HRegion hRegionLine = new HRegion();
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
                Plc.SetIO(this.CBCameraSelect.SelectedIndex + 1, true);
                BtnCameraLive.Text = "Stop";
                BtnGrabImageOnce.Enabled = false;

                BtnCameraLive.BackColor = Color.Red;
                LiveCameraIndex = CBCameraSelect.SelectedIndex;
                IsLiveRun = true;
                TkCameraLive = new Task(FunctionTkCameraLive);
                VisionModulesManager.CameraList[LiveCameraIndex].SetExposureTime(Convert.ToDouble(this.numericUpDown_Expose.Value));
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
                VisionModulesManager.CameraList[LiveCameraIndex].SetExposureTime(Convert.ToDouble(this.numericUpDown_Expose.Value));
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

                switch (tabControl1.SelectedIndex)
                {
                    case 0:
                        ImagePara.Instance.DutExposeTime = Convert.ToDouble(numericUpDown_Expose.Value);
                        break;

                    case 1:
                        ImagePara.Instance.DutBackExposeTime = Convert.ToDouble(numericUpDown_Expose.Value);
                        break;

                    case 2:
                        ImagePara.Instance.SlotExposeTime = Convert.ToDouble(numericUpDown_Expose.Value);
                        break;

                    case 3:
                        ImagePara.Instance.SoketMarkExposeTime = Convert.ToDouble(numericUpDown_Expose.Value);
                        break;

                    case 4:
                        ImagePara.Instance.SoketDutExposeTime = Convert.ToDouble(numericUpDown_Expose.Value);
                        break;

                    default:
                        break;
                }
                double ExposeTime = Convert.ToDouble(numericUpDown_Expose.Value);
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

                    double ExposeTime = Convert.ToDouble(numericUpDown_Expose.Value);

                    VisionModulesManager.CameraList[LiveCameraIndex].SetExposureTime(ExposeTime);
                    MessageBox.Show("Set");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "曝光设置失败");
                }
            }
        }

     
     

        private void FormLocationTest_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void FormLocationTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.IsLiveRun = false;
            try
            {             
                XmlHelper.Instance().SerializeToXml(Utility.type + ConfigMgr.Instance.CurrentImageType, ImagePara.Instance);
            }
            catch (Exception ee)
            {
                AlcUtility.AlcSystem.Instance.ShowMsgBox("更新视觉配方失败", "提示", AlcUtility.AlcMsgBoxButtons.OK, AlcUtility.AlcMsgBoxIcon.Error);
            }
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
            
            OutPutResult locationResult = new OutPutResult();
            HRegion roi = ImagePara.Instance.GetSerchROI("SocketDutROI");
            InputPara locationPara = new InputPara(image, roi, null, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));

            locationResult = AutoNormal_New.ImageProcess.SocketDutFront(locationPara);
            this.superWind1.image = image;
            if (locationResult.IsRunOk == false)
            {
                MessageBox.Show(locationResult.ErrString);
                return;
            }
            HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Angle);
            //按照自定义的取料中心仿射变换
            HHomMat2D hv_HomMat2D = new HHomMat2D();
            if (locationResult.Angle < -0.79)
            {
                locationResult.Angle = 3.14159 + locationResult.Angle;
            }
            hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.DutMode_rowCenter[comboBox_socket.SelectedIndex], ImagePara.Instance.DutMode_colCenter[comboBox_socket.SelectedIndex],
                ImagePara.Instance.DutMode_angleCenter[comboBox_socket.SelectedIndex], locationResult.findPoint.Row, locationResult.findPoint.Column, locationResult.Angle);
            HTuple findrow = hv_HomMat2D.AffineTransPoint2d(ImagePara.Instance.SocketMark_GetDutRow[comboBox_socket.SelectedIndex], ImagePara.Instance.SocketMark_GetDutCol[comboBox_socket.SelectedIndex], out HTuple findcol);
            locationResult.findPoint.Row = findrow;
            locationResult.findPoint.Column = findcol;
            locationResult.IsRunOk = true;
            this.superWind1.obj = locationResult.region;
            this.superWind1.obj = locationResult.SmallestRec2Xld;
            this.superWind1.obj = locationResult.shapeModelContour;
            this.superWind1.obj = roi;
            this.superWind1.Message = string.Format("宽：{0} 高{1}", locationResult.Dutwidth, locationResult.Dutheight);
            HOperatorSet.SetColor(superWind1.viewController.viewPort.HalconWindow, "red");
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
            HRegion roi = ImagePara.Instance.GetSerchROI("SlotDutROI");
            InputPara para = new InputPara(this.image, roi, this.ShapeModel, Convert.ToDouble(this.numericUpDown_score_Traydut.Value));
            //判断是否有料
            if (!AutoNormal_New.ImageProcess.SlotDetect(para, out HObject yy))
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
            ImagePara.Instance.SoketDut_minthreshold[comboBox_socket.SelectedIndex] = (HTuple)numericUpDown_socktdut_min.Value;
        }

        private void numericUpDown_socktdut_max_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_socktdut_min.Value >= numericUpDown_socktdut_max.Value)
            {
                numericUpDown_socktdut_max.Value = numericUpDown_socktdut_min.Value + 1;
            }
            ImagePara.Instance.SoketDut_maxthreshold[comboBox_socket.SelectedIndex]= (HTuple)numericUpDown_socktdut_max.Value;
        }

        private void numericUpDown_dut_width_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.SoketDut_widthmin = (HTuple)numericUpDown_dut_widthmin.Value;
        }

        private void numericUpDown_dut_height_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.SoketDut_heightmin = (HTuple)numericUpDown_dut_heightmin.Value;
        }

        private void numericUpDown_dut_widthmax_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.SoketDut_widthmax = (HTuple)numericUpDown_dut_widthmax.Value;
        }

        private void numericUpDown_dut_heightmax_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.SoketDut_heightmax = (HTuple)numericUpDown_dut_heightmax.Value;
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
                if (MessageBox.Show("确定进入更新设置", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    button_edit_putcenter.BackColor = Color.Red;
                    this.superWind1.viewController.OnlyImage = false;
                    button_edit_putcenter.Text = "结束编辑";
                    this.superWind1.viewController.repaint();
                }
            }
            else
            {
                button_edit_putcenter.Text = "编辑放料中心";
                button_edit_putcenter.BackColor = Color.WhiteSmoke;
                this.superWind1.viewController.OnlyImage = true;
                this.superWind1.viewController.repaint();
                AutoNormal_New.ImageProcess.FindSocketMark(superWind1.image, out HTuple row_center, out HTuple col_center, out HTuple phi, out HObject mark);
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
                if (MessageBox.Show("确定进入更新设置", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    button_editget.BackColor = Color.Red;
                    this.superWind1.viewController.OnlyImage = false;
                    button_editget.Text = "结束编辑";
                    this.superWind1.viewController.repaint();
                }
            }
            else
            {
                button_editget.Text = "2.编辑取料中心";
                button_editget.BackColor = Color.WhiteSmoke;
                this.superWind1.viewController.OnlyImage = true;
                this.superWind1.viewController.repaint();
                OutPutResult locationResult = new OutPutResult();
                HRegion roi = ImagePara.Instance.GetSerchROI("SocketDutROI");
                InputPara locationPara = new InputPara(image, roi, null, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));

                locationResult = AutoNormal_New.ImageProcess.SocketDutFront(locationPara);
                this.superWind1.image = image;
                if (locationResult.IsRunOk == false)
                {
                    MessageBox.Show("定位失败");
                    return;
                }
                if (locationResult.Angle < -0.79)
                {
                    locationResult.Angle = 3.14159 + locationResult.Angle;
                }
                HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, locationResult.findPoint.Row,
                        locationResult.findPoint.Column, 100, locationResult.Angle);

                //料中心模板坐标
                ImagePara.Instance.DutMode_rowCenter[comboBox_socket.SelectedIndex] = Convert.ToSingle(locationResult.findPoint.Row);
                ImagePara.Instance.DutMode_colCenter[comboBox_socket.SelectedIndex] = Convert.ToSingle(locationResult.findPoint.Column);
                ImagePara.Instance.DutMode_angleCenter[comboBox_socket.SelectedIndex] = Convert.ToSingle(locationResult.Angle);

                //放料点模板坐标((ROIRectangle1)(superWind1.roiController.RoiInfo.ROIList[0])).getRegion();
                ImagePara.Instance.SocketMark_GetDutRow[comboBox_socket.SelectedIndex] = Convert.ToSingle(((ROIRectangle2)(superWind1.roiController.RoiInfo.ROIList[0])).midR);
                ImagePara.Instance.SocketMark_GetDutCol[comboBox_socket.SelectedIndex] = Convert.ToSingle(((ROIRectangle2)(superWind1.roiController.RoiInfo.ROIList[0])).midC);
                //新取料中心
                HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, ImagePara.Instance.SocketMark_GetDutRow[comboBox_socket.SelectedIndex],
                    ImagePara.Instance.SocketMark_GetDutCol[comboBox_socket.SelectedIndex], 100, locationResult.Angle);
                this.superWind1.obj = locationResult.region;
                this.superWind1.obj = locationResult.SmallestRec2Xld;
                this.superWind1.obj = locationResult.shapeModelContour;
                this.superWind1.obj = roi;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.FileDetect.Checked)
            {
                // 表示进行连续测试

                FolderBrowserDialog openFolderDialog = new FolderBrowserDialog();
                openFolderDialog.Description = "D:\\SaveImage";
                if (openFolderDialog.ShowDialog() == DialogResult.OK)
                {
                    ImageProcess_Poc2.list_image_files(openFolderDialog.SelectedPath, "default", new HTuple(), out HTuple hv_ImageFiles);
                    HImage image = new HImage();
                    for (int i = 0; i < hv_ImageFiles.Length - 1; i++)
                    {
                        image.Dispose();
                        image = new HImage(hv_ImageFiles[i].S);

                        bool result1 = AutoNormal_New.ImageProcess.SocketDetect(image.CopyImage(), out HObject obj1);
                        this.superWind1.image = image;
                        this.superWind1.obj = obj1;
                        if (result1)
                        {
                            MessageBox.Show("有料");
                        }
                        else
                        {
                            MessageBox.Show("无料");
                        }

                    }

                }
            }

            bool result = AutoNormal_New.ImageProcess.SocketDetect(image.CopyImage(), out HObject obj); ;
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
                if (MessageBox.Show("确定进入更新设置", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    button_serch.BackColor = Color.Red;
                    // this.superWind1.viewController.OnlyImage = false;
                    button_serch.Text = "结束编辑";
                    this.superWind1.viewController.repaint();
                    this.superWind1.roiController.obj = new HRegion();
                    this.superWind1.isDrawUserRegion = true;
                    this.superWind1.roiController.m_pen = 41;
                }
            }
            else
            {
                AutoNormal_New.ImageProcess.FindSocketMark(image, out HTuple Row_mark, out HTuple Column_mark, out HTuple Phi_mark, out HObject mark);
                //mark点中心
                ImagePara.Instance.SocketGet_rowCenter[comboBox_socket.SelectedIndex] = Convert.ToSingle(Row_mark.D);
                ImagePara.Instance.SocketGet_colCenter[comboBox_socket.SelectedIndex] = Convert.ToSingle(Column_mark.D);
                ImagePara.Instance.SocketGet_angleCenter[comboBox_socket.SelectedIndex] = Convert.ToSingle(Phi_mark.D);

                button_serch.Text = "1.编辑搜索区域";
                button_serch.BackColor = Color.WhiteSmoke;
                this.superWind1.isDrawUserRegion = false;
                string socketName = "SerchROI{0}.hobj";
                HOperatorSet.WriteObject(((ROI)(superWind1.roiController.RoiInfo.ROIList[1])).getRegion(),
                    Utility.Model + string.Format(socketName,(comboBox_socket.SelectedIndex).ToString()));
                superWind1.roiController.RoiInfo.ROIList.RemoveAt(1);
                superWind1.roiController.RoiInfo.RemarksList.RemoveAt(1);
            }
        }

        private void button_editSlot_Click(object sender, EventArgs e)
        {
            if (button_editSlot.Text == "编辑放料中心")
            {
                if (MessageBox.Show("确定进入更新设置", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    button_editSlot.BackColor = Color.Red;
                    this.superWind1.viewController.OnlyImage = false;
                    button_editSlot.Text = "结束编辑";
                    this.superWind1.viewController.repaint();
                }
            }
            else
            {
                button_editSlot.Text = "编辑放料中心";
                button_editSlot.BackColor = Color.WhiteSmoke;
                this.superWind1.viewController.OnlyImage = true;
                this.superWind1.viewController.repaint();
                OutPutResult locationResult = new OutPutResult();
                HRegion roi = ImagePara.Instance.GetSerchROI("SlotROI");
                InputPara locationPara = new InputPara(image, roi, null, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));

                locationResult = AutoNormal_New.ImageProcess.ShotSlot(locationPara);
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
                this.superWind1.obj = roi;
            }
        }

        private void button_messure_Click(object sender, EventArgs e)
        {
            this.superWind1.Message = "请画出要测量的长度!";
            superWind1.viewController.viewPort.ContextMenuStrip = null;
            HOperatorSet.GetImageSize(image, out HTuple width, out HTuple height);
            HOperatorSet.DrawLine(this.superWind1.viewController.viewPort.HalconWindow, out HTuple row1, out HTuple col1,
                out HTuple row2, out HTuple col2);
            HOperatorSet.DistancePp(row1, col1, row2, col2, out HTuple distance_pix);
            if ((width.D * height.D) > 11000000)
            {
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, 100, 0, out HTuple x1, out HTuple y1);
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, 100, 100, out HTuple x2, out HTuple y2);
                HOperatorSet.DistancePp(x1, y1, x2, y2, out HTuple distance_plc);
                MessageBox.Show("测量宽度:" + (distance_pix.D * distance_plc.D / 100).ToString("f3") + "mm");
            }
            else
            {
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_up1, 100, 0, out HTuple x1, out HTuple y1);
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_up1, 100, 100, out HTuple x2, out HTuple y2);
                HOperatorSet.DistancePp(x1, y1, x2, y2, out HTuple distance_plc);
                MessageBox.Show("测量宽度:" + (distance_pix.D * distance_plc.D / 100).ToString("f3") + "mm");
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    CBCameraSelect.SelectedIndex = 0;
                    numericUpDown_Expose.Value = Convert.ToDecimal(ImagePara.Instance.DutExposeTime);
                    break;

                case 1:
                    CBCameraSelect.SelectedIndex = 2;
                    numericUpDown_Expose.Value = Convert.ToDecimal(ImagePara.Instance.DutBackExposeTime);
                    break;

                case 2:
                    CBCameraSelect.SelectedIndex = 0;
                    numericUpDown_Expose.Value = Convert.ToDecimal(ImagePara.Instance.SlotExposeTime);
                    break;

                case 3:
                    CBCameraSelect.SelectedIndex = 1;
                    numericUpDown_Expose.Value = Convert.ToDecimal(ImagePara.Instance.SoketMarkExposeTime);
                    break;

                case 4:
                    CBCameraSelect.SelectedIndex = 1;
                    numericUpDown_Expose.Value = Convert.ToDecimal(ImagePara.Instance.SoketDutExposeTime);
                    break;

                default:
                    break;
            }
            this.superWind1.viewController.repaint();
        }
        
        private void numericUpDown_socktmark_min_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_socktmark_min.Value >= numericUpDown_socktmark_max.Value)
            {
                numericUpDown_socktmark_min.Value = numericUpDown_socktmark_max.Value - 1;
            }
            ImagePara.Instance.SoketMark_minthreshold = (HTuple)numericUpDown_socktmark_min.Value;
        }

        private void numericUpDown_socktmark_max_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown_socktmark_min.Value >= numericUpDown_socktmark_max.Value)
            {
                numericUpDown_socktmark_max.Value = numericUpDown_socktmark_min.Value + 1;
            }
            ImagePara.Instance.SoketMark_maxthreshold = (HTuple)numericUpDown_socktmark_max.Value;
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            try
            {
                String name = Interaction.InputBox("输入配方名称", "新建配方", "", -1, -1);
                if (name == "")
                    return;
                if (System.IO.File.Exists(Utility.type + name))
                {
                    if (MessageBox.Show("替换已存在配方？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
                XmlHelper.Instance().SerializeToXml(Utility.type + name + ".xml", ImagePara.Instance);
                MessageBox.Show("新增成功:" + Utility.type + name, "提示", MessageBoxButtons.OK);
                ConfigMgr.Instance.CurrentImageType = name + ".xml";
            }
            catch (Exception ee)
            {
                AlcUtility.AlcSystem.Instance.ShowMsgBox("更新失败" + ee.Message, "提示", AlcUtility.AlcMsgBoxButtons.OK, AlcUtility.AlcMsgBoxIcon.Error);
            }
        }

        private bool isrun;

        private void BtnRunFunctionDutFronts_Click(object sender, EventArgs e)
        {
            if (BtnRunFunctionDutFronts.Text == "批量产品定位")
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择文件路径";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    BtnRunFunctionDutFronts.Text = "停止";
                    isrun = true;
                    BtnRunFunctionDutFronts.BackColor = Color.Red;
                    string foldPath = dialog.SelectedPath;
                    HTuple hv_ImageFiles = new HTuple(), hv_Index = new HTuple();
                    hv_ImageFiles.Dispose();

                    Task.Factory.StartNew
                        (() =>
                        {
                            ImageProcess_Poc2.list_image_files(foldPath, "default", new HTuple(), out hv_ImageFiles);
                            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ImageFiles.TupleLength()
                                )) - 1); hv_Index = (int)hv_Index + 1)
                            {
                                if (isrun == false)
                                {
                                    break;
                                }
                                System.Threading.Thread.Sleep(500);

                                HImage image = new HImage(hv_ImageFiles.TupleSelect(hv_Index));
                                OutPutResult result = new OutPutResult();
                                HRegion roi = ImagePara.Instance.GetSerchROI("SlotDutROI");
                                InputPara para = new InputPara(image, roi, null, Convert.ToDouble(this.numericUpDown_score_Traydut.Value));
                                result = AutoNormal_New.ImageProcess.TrayDutFront(para);
                                this.superWind1.image = image;
                                image.Dispose();
                                if (result.IsRunOk)
                                {
                                    this.superWind1.obj = result.shapeModelContour;
                                    HXLDCont Cross = new HXLDCont();
                                    HOperatorSet.TupleRad(result.Angle, out HTuple rad);
                                    Cross.GenCrossContourXld(result.findPoint.Row, result.findPoint.Column, 200, rad);
                                    this.superWind1.obj = Cross;
                                    this.superWind1.obj = result.SmallestRec2Xld;
                                    this.superWind1.obj = roi;
                                    continue;
                                }
                                else
                                {
                                    this.superWind1.Message = "定位失败";
                                    MessageBox.Show("识别失败");
                                }
                            }
                            button_SocketDuts.Text = "批量产品定位";
                            button_SocketDuts.BackColor = Color.WhiteSmoke;
                        });
                }
            }
            else
            {
                BtnRunFunctionDutFronts.BackColor = Color.WhiteSmoke;
                BtnRunFunctionDutFronts.Text = "批量产品定位";
                isrun = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "批量产品定位")
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择文件路径";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    button4.Text = "停止";
                    isrun = true;
                    button4.BackColor = Color.Red;
                    string foldPath = dialog.SelectedPath;
                    HTuple hv_ImageFiles = new HTuple(), hv_Index = new HTuple();
                    hv_ImageFiles.Dispose();

                    Task.Factory.StartNew
                        (() =>
                        {
                            ImageProcess_Poc2.list_image_files(foldPath, "default", new HTuple(), out hv_ImageFiles);
                            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ImageFiles.TupleLength()
                                )) - 1); hv_Index = (int)hv_Index + 1)
                            {
                                if (isrun == false)
                                {
                                    break;
                                }
                                System.Threading.Thread.Sleep(1000);
                                HImage Image = new HImage(hv_ImageFiles.TupleSelect(hv_Index));
                                OutPutResult result = new OutPutResult();
                                HRegion roi = ImagePara.Instance.GetSerchROI("DutBackROI");
                                InputPara para = new InputPara(Image, roi,null, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));
                                result = AutoNormal_New.ImageProcess.SecondDutBack(para, new PLCSend() { Func = 1 });
                                this.superWind1.image = image;
                                image.Dispose();
                                if (result.IsRunOk)
                                {
                                    this.superWind1.obj = result.shapeModelContour;
                                    HXLDCont Cross = new HXLDCont();
                                    HOperatorSet.TupleRad(result.Angle, out HTuple rad);
                                    Cross.GenCrossContourXld(result.findPoint.Row, result.findPoint.Column, 200, rad);
                                    this.superWind1.obj = Cross;
                                    this.superWind1.obj = result.SmallestRec2Xld;
                                    this.superWind1.obj = roi;
                                    continue;
                                }
                                else
                                {
                                    this.superWind1.Message = "定位失败";
                                    MessageBox.Show("识别失败");
                                }
                            }
                            button4.Text = "批量产品定位";
                            button4.BackColor = Color.WhiteSmoke;
                        });
                }
            }
            else
            {
                button4.BackColor = Color.WhiteSmoke;
                button4.Text = "批量产品定位";
                isrun = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "批量产品定位")
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择文件路径";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    button3.Text = "停止";
                    isrun = true;
                    button3.BackColor = Color.Red;
                    string foldPath = dialog.SelectedPath;
                    HTuple hv_ImageFiles = new HTuple(), hv_Index = new HTuple();
                    hv_ImageFiles.Dispose();

                    Task.Factory.StartNew
                        (() =>
                        {
                            ImageProcess_Poc2.list_image_files(foldPath, "default", new HTuple(), out hv_ImageFiles);
                            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ImageFiles.TupleLength()
                                )) - 1); hv_Index = (int)hv_Index + 1)
                            {
                                if (isrun == false)
                                {
                                    break;
                                }
                                System.Threading.Thread.Sleep(1000);
                                HImage Image = new HImage(hv_ImageFiles.TupleSelect(hv_Index));
                                OutPutResult result = new OutPutResult();
                                HRegion roi = ImagePara.Instance.GetSerchROI("SocketMarkROI");
                                InputPara para = new InputPara(Image,roi, null, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));
                                result = AutoNormal_New.ImageProcess.SocketMark(para,out HObject mark);
                                this.superWind1.image = image;
                                image.Dispose();
                                if (result.IsRunOk)
                                {
                                    this.superWind1.obj = result.shapeModelContour;
                                    HXLDCont Cross = new HXLDCont();
                                    HOperatorSet.TupleRad(result.Angle, out HTuple rad);
                                    Cross.GenCrossContourXld(result.findPoint.Row, result.findPoint.Column, 200, rad);
                                    this.superWind1.obj = Cross;
                                    this.superWind1.obj = result.SmallestRec2Xld;
                                    this.superWind1.obj = roi;
                                    continue;
                                }
                                else
                                {
                                    this.superWind1.Message = "定位失败";
                                    MessageBox.Show("识别失败");
                                }
                            }
                            button3.Text = "批量产品定位";
                            button3.BackColor = Color.WhiteSmoke;
                        });
                }
            }
            else
            {
                button3.BackColor = Color.WhiteSmoke;
                button3.Text = "批量产品定位";
                isrun = false;
            }
        }

        private void BtnRunFunctionSocketMarks_Click(object sender, EventArgs e)
        {
            if (BtnRunFunctionSocketMarks.Text == "批量产品定位")
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择文件路径";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    BtnRunFunctionSocketMarks.Text = "停止";
                    isrun = true;
                    BtnRunFunctionSocketMarks.BackColor = Color.Red;
                    string foldPath = dialog.SelectedPath;
                    HTuple hv_ImageFiles = new HTuple(), hv_Index = new HTuple();
                    hv_ImageFiles.Dispose();

                    Task.Factory.StartNew
                        (() =>
                        {
                            ImageProcess_Poc2.list_image_files(foldPath, "default", new HTuple(), out hv_ImageFiles);
                            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ImageFiles.TupleLength()
                                )) - 1); hv_Index = (int)hv_Index + 1)
                            {
                                if (isrun == false)
                                {
                                    break;
                                }
                                System.Threading.Thread.Sleep(500);
                                HImage Image = new HImage(hv_ImageFiles.TupleSelect(hv_Index));
                                OutPutResult result = new OutPutResult();
                                HRegion roi = ImagePara.Instance.GetSerchROI("SocketMarkROI");
                                InputPara para = new InputPara(Image, roi, null, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));
                                result = AutoNormal_New.ImageProcess.SocketMark(para, out HObject mark);
                                this.superWind1.image = image;
                                image.Dispose();
                                if (result.IsRunOk)
                                {
                                    this.superWind1.obj = result.shapeModelContour;
                                    HXLDCont Cross = new HXLDCont();
                                    HOperatorSet.TupleRad(result.Angle, out HTuple rad);
                                    Cross.GenCrossContourXld(result.findPoint.Row, result.findPoint.Column, 200, rad);
                                    this.superWind1.obj = Cross;
                                    this.superWind1.obj = result.SmallestRec2Xld;
                                    this.superWind1.obj = roi;
                                    continue;
                                }
                                else
                                {
                                    this.superWind1.Message = "定位失败";
                                    MessageBox.Show("识别失败");
                                }
                            }
                            BtnRunFunctionSocketMarks.Text = "批量产品定位";
                            BtnRunFunctionSocketMarks.BackColor = Color.WhiteSmoke;
                        });
                }
            }
            else
            {
                BtnRunFunctionSocketMarks.BackColor = Color.WhiteSmoke;
                BtnRunFunctionSocketMarks.Text = "批量产品定位";
                isrun = false;
            }
        }

        private void button_SocketDuts_Click(object sender, EventArgs e)
        {
            if (button_SocketDuts.Text == "批量产品定位")
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择文件路径";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    button_SocketDuts.Text = "停止";
                    isrun = true;
                    button_SocketDuts.BackColor = Color.Red;
                    string foldPath = dialog.SelectedPath;
                    HTuple hv_ImageFiles = new HTuple(), hv_Index = new HTuple();
                    hv_ImageFiles.Dispose();

                    Task.Factory.StartNew
                        (() =>
                        {
                            ImageProcess_Poc2.list_image_files(foldPath, "default", new HTuple(), out hv_ImageFiles);
                            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ImageFiles.TupleLength()
                                )) - 1); hv_Index = (int)hv_Index + 1)
                            {
                                if (isrun == false)
                                {
                                    break;
                                }
                                System.Threading.Thread.Sleep(1000);
                                HImage image = new HImage(hv_ImageFiles.TupleSelect(hv_Index));
                                OutPutResult result = new OutPutResult();
                                HRegion roi = ImagePara.Instance.GetSerchROI("SocketDutROI");
                                InputPara para = new InputPara(image, roi, null, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));
                                result = AutoNormal_New.ImageProcess.SocketDutFront(para);
                                this.superWind1.image = image;
                                image.Dispose();
                                if (result.IsRunOk)
                                {
                                    this.superWind1.obj = result.region;
                                    this.superWind1.obj = result.shapeModelContour;
                                    this.superWind1.Message = string.Format("宽：{0} 高{1}", result.Dutwidth, result.Dutheight);
                                    HXLDCont Cross = new HXLDCont();
                                    HOperatorSet.TupleRad(result.Angle, out HTuple rad);
                                    Cross.GenCrossContourXld(result.findPoint.Row, result.findPoint.Column, 200, rad);
                                    this.superWind1.obj = Cross;
                                    this.superWind1.obj = result.SmallestRec2Xld;
                                    this.superWind1.obj = roi;
                                    continue;
                                }
                                else
                                {
                                    this.superWind1.Message = "定位失败";
                                    MessageBox.Show("识别失败");
                                }
                            }
                            button_SocketDuts.Text = "批量产品定位";
                            button_SocketDuts.BackColor = Color.WhiteSmoke;
                        });
                }
            }
            else
            {
                button_SocketDuts.BackColor = Color.WhiteSmoke;
                button_SocketDuts.Text = "批量产品定位";
                isrun = false;
            }
        }

        private void tabPage5_Click(object sender, EventArgs e)
        {
        }
        HTuple DrawID;
        private void button_dutRoi_Click(object sender, EventArgs e)
        {
            if (button_dutRoi.Text == "编辑搜索区域")
            {
                if (MessageBox.Show("确定进入更新设置", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    ImagePara.Instance.GetSerchROI("SlotDutROI",out HTuple row1,out HTuple col1,out HTuple row2,out HTuple col2);
                    HOperatorSet.CreateDrawingObjectRectangle1(row1, col1, row2, col2, out DrawID);
                    HOperatorSet.SetDrawingObjectParams(DrawID, "color", "red");
                    HOperatorSet.AttachDrawingObjectToWindow(this.superWind1.viewController.viewPort.HalconWindow,DrawID);
                    button_dutRoi.BackColor = Color.Red;
                    button_dutRoi.Text = "结束编辑";
                    this.superWind1.viewController.repaint();
                }
            }
            else
            {
                HOperatorSet.GetDrawingObjectIconic(out HObject obj, DrawID);
                HOperatorSet.SmallestRectangle1(obj, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                ImagePara.Instance.SetSerchROI("SlotDutROI", col1,row1,   col2 ,row2 );
                HOperatorSet.DetachDrawingObjectFromWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                button_dutRoi.Text = "编辑搜索区域";
                button_dutRoi.BackColor = Color.WhiteSmoke;
            }
        }

        private void button_dutbackROI_Click(object sender, EventArgs e)
        {
            if (button_dutbackROI.Text == "编辑搜索区域")
            {
                if (MessageBox.Show("确定进入更新设置", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    ImagePara.Instance.GetSerchROI("DutBackROI", out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                    HOperatorSet.CreateDrawingObjectRectangle1(row1, col1, row2, col2, out DrawID);
                    HOperatorSet.SetDrawingObjectParams(DrawID, "color", "red");
                    HOperatorSet.AttachDrawingObjectToWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                    button_dutbackROI.Text = "结束编辑";
                    button_dutbackROI.BackColor = Color.Red;
                    this.superWind1.viewController.repaint();
                }
            }
            else
            {
                HOperatorSet.GetDrawingObjectIconic(out HObject obj, DrawID);
                HOperatorSet.SmallestRectangle1(obj, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                ImagePara.Instance.SetSerchROI("DutBackROI", col1,row1,   col2,row2);
                HOperatorSet.DetachDrawingObjectFromWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                button_dutbackROI.Text = "编辑搜索区域";
                button_dutbackROI.BackColor = Color.WhiteSmoke;
            }
        }

        private void button_slotROI_Click(object sender, EventArgs e)
        {
            if (button_slotROI.Text == "编辑搜索区域")
            {
                if (MessageBox.Show("确定进入更新设置", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    ImagePara.Instance.GetSerchROI("SlotROI", out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                    HOperatorSet.CreateDrawingObjectRectangle1(row1, col1, row2, col2, out DrawID);
                    HOperatorSet.SetDrawingObjectParams(DrawID, "color", "red");
                    HOperatorSet.AttachDrawingObjectToWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                    button_slotROI.Text = "结束编辑";
                    button_slotROI.BackColor = Color.Red;
                    this.superWind1.viewController.repaint();
                }
            }
            else
            {
                HOperatorSet.GetDrawingObjectIconic(out HObject obj, DrawID);
                HOperatorSet.SmallestRectangle1(obj, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                ImagePara.Instance.SetSerchROI("SlotROI", col1,row1,  col2,row2);
                HOperatorSet.DetachDrawingObjectFromWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                button_slotROI.Text = "编辑搜索区域";
                button_slotROI.BackColor = Color.WhiteSmoke;
            }
        }

        private void button_SocketRoi_Click(object sender, EventArgs e)
        {
            if (button_SocketRoi.Text == "编辑搜索区域")
            {
                if (MessageBox.Show("确定进入更新设置", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    ImagePara.Instance.GetSerchROI("SocketMarkROI", out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                    HOperatorSet.CreateDrawingObjectRectangle1(row1, col1, row2, col2, out DrawID);
                    HOperatorSet.SetDrawingObjectParams(DrawID, "color", "red");
                    HOperatorSet.AttachDrawingObjectToWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                    button_SocketRoi.Text = "结束编辑";
                    button_SocketRoi.BackColor = Color.Red;
                    this.superWind1.viewController.repaint();
                }
            }
            else
            {
                HOperatorSet.GetDrawingObjectIconic(out HObject obj, DrawID);
                HOperatorSet.SmallestRectangle1(obj, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                ImagePara.Instance.SetSerchROI("SocketMarkROI", col1, row1,  col2,row2);
                HOperatorSet.DetachDrawingObjectFromWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                button_SocketRoi.Text = "编辑搜索区域";
                button_SocketRoi.BackColor = Color.WhiteSmoke;
            }
        }

        private void button_SocketDutRoi_Click(object sender, EventArgs e)
        {
            if (button_SocketDutRoi.Text == "编辑搜索区域")
            {
                if (MessageBox.Show("确定进入更新设置", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    ImagePara.Instance.GetSerchROI("SocketDutROI", out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                    HOperatorSet.CreateDrawingObjectRectangle1(row1, col1, row2, col2, out DrawID);
                    HOperatorSet.SetDrawingObjectParams(DrawID, "color", "red");
                    HOperatorSet.AttachDrawingObjectToWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                    button_SocketDutRoi.Text = "结束编辑";
                    button_SocketDutRoi.BackColor = Color.Red;
                    this.superWind1.viewController.repaint();
                }
            }
            else
            {
                HOperatorSet.GetDrawingObjectIconic(out HObject obj, DrawID);
                HOperatorSet.SmallestRectangle1(obj, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                ImagePara.Instance.SetSerchROI("SocketDutROI", col1,row1,  col2,row2);
                HOperatorSet.DetachDrawingObjectFromWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                button_SocketDutRoi.Text = "编辑搜索区域";
                button_SocketDutRoi.BackColor = Color.WhiteSmoke;
            }
        }

        private void comboBox_socket_SelectedIndexChanged(object sender, EventArgs e)
        {
            ImageProcess_Poc2.CurrentSoket = comboBox_socket.SelectedIndex;
            numericUpDown_socktdut_min.Value = ImagePara.Instance.SoketDut_minthreshold[ImageProcess_Poc2.CurrentSoket];
            numericUpDown_socktdut_max.Value = ImagePara.Instance.SoketDut_maxthreshold[ImageProcess_Poc2.CurrentSoket];
        }

        private void btn_DutDetect_Click(object sender, EventArgs e)
        {
            try
            {
                if (AutoNormal_New.DutBackgroundDetect(this.image))
                {
                    MessageBox.Show("有料");
                }
                else MessageBox.Show("无料");
            }
            catch (Exception)
            {

                MessageBox.Show("无料");
            }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown_PushBlock_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.PushBlock = (HTuple)numericUpDown_PushBlock.Value;
        }

        private void button_checkBlock_Click(object sender, EventArgs e)
        {
            this.superWind1.image = image;
            HRegion roi = ImagePara.Instance.GetSerchROI("SocketMarkROI");
            InputPara locationPara = new InputPara(image, roi, null, Convert.ToDouble(this.numericUpDown_score_socktMark.Value));
            AutoNormal_New.ImageProcess.FindSocketMark(locationPara.image, out HTuple row1, out HTuple col1,out HTuple phi,out HObject mark) ;
            if (row1.Length <=0 )
            {
                
                MessageBox.Show("Socket Mark 定位失败");
                return;
            }
            this.superWind1.obj = mark;
            mark.Dispose();
            bool result=  AutoNormal_New.ImageProcess.BlockDetect(row1, col1, locationPara, out int distance,out HObject arrow);
            this.superWind1.obj = arrow;
            this.superWind1.Message = "推块距离:" + distance.ToString();
            if(!result)
            {
                MessageBox.Show("推块位置太短！");
            }
        }
    }
}