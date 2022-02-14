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
using VisionSDK;

namespace VisionFlows.VisionCalculate
{
    public partial class FormLocationSet : Form
    {

        /// <summary>
        /// 测试 批量处理图像时 图片序列
        /// </summary>
        private List<FileInfo> fileInfos = new List<FileInfo>();

        ArrayList EditRoi =new ArrayList();

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

        EnumCamera CurrentCameraUserID;

        bool isLoad;
        public FormLocationSet()
        {
            InitializeComponent();
        }

        private void FormLocationTest_Load(object sender, EventArgs e)
        {
            comboBox_socket.SelectedIndex = 0;
            this.numericUpDown_score_dutback.Value = Convert.ToDecimal(ImagePara.Instance.DutBackScore);
            this.numericUpDown_score_socktdut.Value = Convert.ToDecimal(ImagePara.Instance.SoketDutScore);
            this.numericUpDown_score_socktMark.Value = Convert.ToDecimal(ImagePara.Instance.SocketMarkScore);
            this.numericUpDown_score_Traydut.Value = Convert.ToDecimal(ImagePara.Instance.DutScore);
            this.numericUpDown_score_slot.Value = Convert.ToDecimal(ImagePara.Instance.SlotScore);
            numericUpDown_socktdut_max.Value = ImagePara.Instance.SoketDut_maxthreshold[0];
            numericUpDown_socktdut_min.Value = ImagePara.Instance.SoketDut_minthreshold[0];
            numericUpDown_trayDut_min.Value = ImagePara.Instance.TrayDut_minthreshold;
            numericUpDown_trayDut_max.Value = ImagePara.Instance.TrayDut_maxthreshold;
            numericUpDown_rad_min.Value = ImagePara.Instance.SoketDut_widthmin;
            numericUpDown_distance_heightmin.Value = ImagePara.Instance.SoketDut_heightmin;
            numericUpDown_rad_max.Value = ImagePara.Instance.SoketDut_widthmax;
            numericUpDown_dut_heightmax.Value = ImagePara.Instance.SoketDut_heightmax;
            numericUpDown_socktmark_min.Value = ImagePara.Instance.SoketMark_minthreshold;
            numericUpDown_socktmark_max.Value = ImagePara.Instance.SoketMark_maxthreshold;
            numericUpDown_PushBlock.Value= ImagePara.Instance.PushBlock;           
            this.superWind1.image = new HImage("byte", 500, 500);
            this.superWind1.roiController.addRec2(500, 500, "");
            EditRoi= (ArrayList)this.superWind1.roiController.RoiInfo.ROIList.Clone();
            this.superWind1.viewController.OnlyImage = true;
            this.superWind1.viewController.repaint();
            this.text = "配方:" + Utility.TypeFile + ConfigMgr.Instance.CurrentImageType;
            CBCameraSelect.SelectedIndex =0;
            CurrentCameraUserID = EnumCamera.LeftTop;
            Utility.IsSaveAllImage = true;
            Utility.IsSaveNgImage = false;

            isLoad = true;
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
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotDutROI);
                InputPara para = new InputPara(this.superWind1.image, roi, new HShapeModel(), Convert.ToDouble(this.numericUpDown_score_Traydut.Value));
                result = AutoNormal_New.ImageProcess.TrayDutFront(para);
                if (!result.IsRunOk)
                {
                    this.superWind1.image = para.image;
                    MessageBox.Show("DutFront 定位失败");
                    return;
                }
                this.superWind1.obj = result.shapeModelContour;
                HXLDCont Cross = new HXLDCont();
                HOperatorSet.TupleRad(result.Phi, out HTuple rad);
                Cross.GenCrossContourXld(result.findPoint.Row, result.findPoint.Column, 200, rad);
                this.superWind1.obj = result.region;
                this.superWind1.obj = Cross;
                this.superWind1.obj = result.SmallestRec2Xld;
                this.superWind1.obj = roi;
                HOperatorSet.DumpWindowImage(out HObject iimage, this.superWind1.hwind.HalconWindow);
                ///AutoNormal_New.SaveOriginalImage("111", this.superWind1.image);
                //AutoNormal_New.SaveDumpImage("222", iimage);

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
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.DutBackROI);
                InputPara locationPara = new InputPara(superWind1.image, roi, new HShapeModel(), Convert.ToDouble(this.numericUpDown_score_dutback.Value));
                OutPutResult locationResult = new OutPutResult();
               
                locationResult = AutoNormal_New.ImageProcess.SecondDutBack(locationPara, new PLCSend() { Func = 1 });
                if (locationResult.IsRunOk == false)
                {
                    MessageBox.Show("Dut Back 定位失败");
                    return;
                }
                HXLDCont hXLDCont = new HXLDCont();
                hXLDCont.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 300, locationResult.Phi);
                this.superWind1.obj = hXLDCont;
                this.superWind1.obj = locationResult.shapeModelContour;
                this.superWind1.obj = locationResult.region;
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
            HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SocketMarkROI);
            InputPara locationPara = new InputPara(superWind1.image, roi,new HShapeModel(), Convert.ToDouble(this.numericUpDown_score_socktMark.Value));

            locationResult = AutoNormal_New.ImageProcess.SocketMark(locationPara, out HObject mark);
            if (locationResult.IsRunOk == false)
            {
                MessageBox.Show("Socket Mark 定位失败");
                return;
            }
            this.superWind1.obj = mark;
            mark.Dispose();

            HXLDCont hXLDCont = new HXLDCont();
            hXLDCont.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 300, locationResult.Phi);

            this.superWind1.ObjColor = "green";
            this.superWind1.obj = hXLDCont;
            this.superWind1.obj = roi;
        }
        /// <summary>
        /// slot定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void BtnRunTraySlot_Click(object sender, EventArgs e)
        {
            OutPutResult locationResult = new OutPutResult();
            HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotROI);
            InputPara locationPara = new InputPara(superWind1.image, roi,new HShapeModel(), Convert.ToDouble(this.numericUpDown_score_slot.Value));
            //
            locationResult = AutoNormal_New.ImageProcess.ShotSlot(locationPara);
            if (!locationResult.IsRunOk)
            {
                MessageBox.Show("TraySlot定位失败");
                return;
            }
            //模板匹配结果及其外接矩形轮廓显示
            this.superWind1.obj = locationResult.SmallestRec2Xld;
            HXLDCont cross = new HXLDCont();
            cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 200, locationResult.Phi);
            this.superWind1.obj = cross;
            this.superWind1.obj = locationResult.SmallestRec2Xld;
            //按照自定义的取料中心仿射变换
            HHomMat2D hv_HomMat2D = new HHomMat2D();
            if (locationResult.Phi < -0.79)
            {
                locationResult.Phi = 3.14159 + locationResult.Phi;
            }
            hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.slot_rowCenter, ImagePara.Instance.slot_colCenter,
                ImagePara.Instance.slot_angleCenter, locationResult.findPoint.Row, locationResult.findPoint.Column, locationResult.Phi);
            HTuple findrow = hv_HomMat2D.AffineTransPoint2d(ImagePara.Instance.slot_offe_rowCenter, ImagePara.Instance.slot_offe_colCenter, out HTuple findcol);
            locationResult.findPoint.Row = findrow;//最后取料中心
            locationResult.findPoint.Column = findcol;
            cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 200, locationResult.Phi);
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
                CameraManager.CameraById(CurrentCameraUserID.ToString()).ShuterCur=(long)(this.numericUpDown_Expose.Value);
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
                superWind1.image = CameraManager.CameraById(CurrentCameraUserID.ToString()).GrabImage(Utility.CaptureDelayTime);
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
                CameraManager.CameraById(CurrentCameraUserID.ToString()).ShuterCur = (long)(Convert.ToDouble(this.numericUpDown_Expose.Value));
                LiveCameraIndex = CBCameraSelect.SelectedIndex;
                Plc.SetIO(this.CBCameraSelect.SelectedIndex + 1, true);
                superWind1.image = CameraManager.CameraById(CurrentCameraUserID.ToString()).GrabImage(Utility.CaptureDelayTime);
                Plc.SetIO(this.CBCameraSelect.SelectedIndex + 1, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("相机 实时 失败");
                Plc.SetIO(this.CBCameraSelect.SelectedIndex + 1, false);
            }
        }

     
        private void FormLocationTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Utility.IsSaveImage = CBIsWriteStationImage.Checked;
            this.IsLiveRun = false;
            HalconAPI.CancelDraw();
        }
        private void CBIsWriteStationImage_CheckedChanged(object sender, EventArgs e)
        {
             //Utility.IsSaveImage = CBIsWriteStationImage.Checked;
        }
        /// <summary>
        /// 取Sockt  dut料产品定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLocationSocketDutTest_Click(object sender, EventArgs e)
        {
            
            OutPutResult locationResult = new OutPutResult();
            HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SocketDutROI);
            InputPara locationPara = new InputPara(superWind1.image, superWind1.image.GetDomain(), null, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));
            locationResult = AutoNormal_New.ImageProcess.SocketDutFront(locationPara);
            if (locationResult.IsRunOk == false)
            {
                MessageBox.Show(locationResult.ErrString);
                return;
            }
            HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Phi);
            //按照自定义的取料中心仿射变换
            if (locationResult.Phi < -0.79)
            {
                locationResult.Phi = 3.14159 + locationResult.Phi;
            }
            locationResult.IsRunOk = true;
            this.superWind1.obj = locationResult.region;
            this.superWind1.obj = locationResult.SmallestRec2Xld;
            this.superWind1.obj = locationResult.shapeModelContour;
            this.superWind1.obj = roi;
            HOperatorSet.SetColor(superWind1.viewController.viewPort.HalconWindow, "red");
            HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Phi);
        }
        /// <summary>
        /// 槽有无dut测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_isdut_Click(object sender, EventArgs e)
        {
            HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotROI);
            InputPara para = new InputPara(superWind1.image, roi, new HShapeModel(), Convert.ToDouble(this.numericUpDown_score_Traydut.Value));
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
            ImagePara.Instance.SoketDut_widthmin = (HTuple)numericUpDown_rad_min.Value;
        }

        private void numericUpDown_dut_height_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.SoketDut_heightmin = (HTuple)numericUpDown_distance_heightmin.Value;
        }

        private void numericUpDown_dut_widthmax_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.SoketDut_widthmax = (HTuple)numericUpDown_rad_max.Value;
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
        /// <summary>
        ///编辑放料到Socket中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_edit_putcenter_Click(object sender, EventArgs e)
        {
            if (button_edit_putcenter.Text == "编辑放料中心")
            {
                if (MessageBox.Show("确定进入更新设置", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    HOperatorSet.CreateDrawingObjectRectangle2(ImagePara.Instance.SocketMark_PutDutRow, ImagePara.Instance.SocketMark_PutDutCol, 0, 100, 100, out DrawID);
                    HOperatorSet.SetDrawingObjectParams(DrawID, "color", "red");
                    HOperatorSet.AttachDrawingObjectToWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                    button_edit_putcenter.BackColor = Color.Red;
                    button_edit_putcenter.Text = "结束编辑";
                }
            }
            else
            {
                button_edit_putcenter.Text = "编辑放料中心";
                button_edit_putcenter.BackColor = Color.WhiteSmoke;
                AutoNormal_New.ImageProcess.FindSocketMarkFirst(superWind1.image, out HTuple row_center, out HTuple col_center, out HTuple phi, out HObject mark);
                if(row_center.Length<=0)
                {
                    AutoNormal_New.ImageProcess.FindSocketMarkSecond(superWind1.image, out  row_center, out  col_center, out  phi, out  mark);
                    if(row_center.Length<=0)
                    {
                        AutoNormal_New.ImageProcess.FindSocketMarkThird(superWind1.image, out row_center, out col_center, out phi, out mark);
                    }
                }
                HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, row_center, col_center, 100, phi);
                //mark点模板坐标
                ImagePara.Instance.SocketMark_rowCenter = Convert.ToSingle(row_center.D);
                ImagePara.Instance.SocketMark_colCenter = Convert.ToSingle(col_center.D);
                ImagePara.Instance.SocketMark_angleCenter = Convert.ToSingle(phi.D);

                HOperatorSet.GetDrawingObjectParams(DrawID, "row", out HTuple row1);
                HOperatorSet.GetDrawingObjectParams(DrawID, "column", out HTuple col1);
                ImagePara.Instance.SocketMark_PutDutRow = (float)row1.D;
                ImagePara.Instance.SocketMark_PutDutCol = (float)col1.D;
                HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, ImagePara.Instance.SocketMark_PutDutRow,
                    ImagePara.Instance.SocketMark_PutDutCol, 100, phi);
                HOperatorSet.DetachDrawingObjectFromWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                HOperatorSet.WriteImage(this.superWind1.image, "bmp", 0, Utility.ImageFile+"SocketMarkImage");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int  result1 = AutoNormal_New.ImageProcess.SocketDetect(this.superWind1.image, out HObject obj1);
            this.superWind1.obj = obj1;
            if (result1==3)
            {
                MessageBox.Show("有料");
            }
            else if (result1 == 0)
            {
                MessageBox.Show("无料");
            }
            else
            {
                MessageBox.Show("有料，但歪斜，请确认！");
            }
        }

        private void button_serch_Click(object sender, EventArgs e)
        {
            SetROI(sender as Button, ref ImagePara.Instance.RegionROI1[comboBox_socket.SelectedIndex]);
            if(button_serch.Text!="结束")
            {
                AutoNormal_New.ImageProcess.FindSocketMarkFirst(superWind1.image, out HTuple Row_mark, out HTuple Column_mark, out HTuple Phi_mark, out HObject mark);
                if (Row_mark.Length <= 0)
                {
                    AutoNormal_New.ImageProcess.FindSocketMarkSecond(superWind1.image, out Row_mark, out Column_mark, out Phi_mark, out mark);
                    if (Row_mark.Length <= 0)
                    {
                        AutoNormal_New.ImageProcess.FindSocketMarkThird(superWind1.image, out Row_mark, out Column_mark, out Phi_mark, out mark);
                    }
                }
                //mark点中心
                if (Column_mark.Length>0)
                {
                    ImagePara.Instance.SocketGet_rowCenter[comboBox_socket.SelectedIndex] = Convert.ToSingle(Row_mark.D);
                    ImagePara.Instance.SocketGet_colCenter[comboBox_socket.SelectedIndex] = Convert.ToSingle(Column_mark.D);
                    ImagePara.Instance.SocketGet_angleCenter[comboBox_socket.SelectedIndex] = Convert.ToSingle(Phi_mark.D);
                }
               
                HOperatorSet.WriteImage(this.superWind1.image, "bmp", 0,Utility.ImageFile+ "SocketDutImage" + (comboBox_socket.SelectedIndex + 1).ToString());
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            SetROI(sender as Button, ref ImagePara.Instance.RegionROI2[comboBox_socket.SelectedIndex]);
            if (button_serch.Text != "结束")
            {
                AutoNormal_New.ImageProcess.FindSocketMarkFirst(superWind1.image, out HTuple Row_mark, out HTuple Column_mark, out HTuple Phi_mark, out HObject mark);
                if (Row_mark.Length <= 0)
                {
                    AutoNormal_New.ImageProcess.FindSocketMarkSecond(superWind1.image, out Row_mark, out Column_mark, out Phi_mark, out mark);
                    if (Row_mark.Length <= 0)
                    {
                        AutoNormal_New.ImageProcess.FindSocketMarkThird(superWind1.image, out Row_mark, out Column_mark, out Phi_mark, out mark);
                    }
                }
                if (Column_mark.Length!=1)
                {
                    MessageBox.Show("mark识别失败");
                    return;
                }
                //mark点中心
                ImagePara.Instance.SocketGet_rowCenter[comboBox_socket.SelectedIndex] = Convert.ToSingle(Row_mark.D);
                ImagePara.Instance.SocketGet_colCenter[comboBox_socket.SelectedIndex] = Convert.ToSingle(Column_mark.D);
                ImagePara.Instance.SocketGet_angleCenter[comboBox_socket.SelectedIndex] = Convert.ToSingle(Phi_mark.D);
                HOperatorSet.WriteImage(this.superWind1.image,"bmp",0,"SocketDutImage"+ (comboBox_socket.SelectedIndex+1).ToString());
            }
        }

        private void button_editSlot_Click(object sender, EventArgs e)
        {
            if (button_editSlot.Text == "编辑放料中心")
            {
                if (MessageBox.Show("确定进入更新设置", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    HOperatorSet.CreateDrawingObjectRectangle2(ImagePara.Instance.slot_offe_rowCenter, ImagePara.Instance.slot_offe_colCenter, 0, 100, 100, out DrawID);
                    HOperatorSet.SetDrawingObjectParams(DrawID, "color", "red");
                    HOperatorSet.AttachDrawingObjectToWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                    button_editSlot.BackColor = Color.Red;
                    button_editSlot.Text = "结束编辑";
                }
            }
            else
            {
                button_editSlot.Text = "编辑放料中心";
                button_editSlot.BackColor = Color.WhiteSmoke;
                OutPutResult locationResult = new OutPutResult();
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotROI);
                InputPara locationPara = new InputPara(superWind1.image, roi, null, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));
                locationResult = AutoNormal_New.ImageProcess.ShotSlot(locationPara);
                if (locationResult.IsRunOk == false)
                {
                    MessageBox.Show("定位失败");
                    return;
                }
                HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, locationResult.findPoint.Row,
                    locationResult.findPoint.Column, 100, locationResult.Phi);

                //料中心模板坐标
                ImagePara.Instance.slot_rowCenter = Convert.ToSingle(locationResult.findPoint.Row);
                ImagePara.Instance.slot_colCenter = Convert.ToSingle(locationResult.findPoint.Column);
                ImagePara.Instance.slot_angleCenter = Convert.ToSingle(locationResult.Phi);

                HOperatorSet.GetDrawingObjectParams(DrawID, "row", out HTuple row1);
                HOperatorSet.GetDrawingObjectParams(DrawID, "column", out HTuple col1);
                ImagePara.Instance.slot_offe_rowCenter = (float)row1.D;
                ImagePara.Instance.slot_offe_colCenter = (float)col1.D;
                HOperatorSet.DetachDrawingObjectFromWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                //放料点模板坐标((ROIRectangle1)(superWind1.roiController.RoiInfo.ROIList[0])).getRegion();
                //新取料中心
                HOperatorSet.DispCross(superWind1.viewController.viewPort.HalconWindow, ImagePara.Instance.slot_offe_rowCenter,
                    ImagePara.Instance.slot_offe_colCenter, 100, locationResult.Phi);
                this.superWind1.obj = locationResult.region;
                this.superWind1.obj = locationResult.SmallestRec2Xld;
                this.superWind1.obj = locationResult.shapeModelContour;
                this.superWind1.obj = roi;
                HOperatorSet.WriteImage(this.superWind1.image, "bmp", 0, Utility.ImageFile+"SlotImage");
            }
        }

        private void button_messure_Click(object sender, EventArgs e)
        {
            this.superWind1.Message = "请画出要测量的长度!";
            superWind1.viewController.viewPort.ContextMenuStrip = null;
            HOperatorSet.GetImageSize(superWind1.image, out HTuple width, out HTuple height);
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
                    numericUpDown_Expose.Value = Convert.ToDecimal(ImagePara.Instance.Exposure_LeftCamGetDUT);
                    if(File.Exists(Utility.ImageFile+ "DutImage.bmp"))
                       this.superWind1.image = new HImage(Utility.ImageFile + "DutImage.bmp");
                    break;

                case 1:
                    CBCameraSelect.SelectedIndex = 2;
                    numericUpDown_Expose.Value = Convert.ToDecimal(ImagePara.Instance.Exposure_DownCamScan);
                    if (File.Exists(Utility.ImageFile + "DutbackImage.bmp"))
                        this.superWind1.image = new HImage(Utility.ImageFile + "DutbackImage.bmp");
                    break;

                case 2:
                    CBCameraSelect.SelectedIndex = 0;
                    numericUpDown_Expose.Value = Convert.ToDecimal(ImagePara.Instance.Exposure_LeftCamGetDUT);
                    if (File.Exists(Utility.ImageFile + "SlotImage.bmp"))
                        this.superWind1.image = new HImage(Utility.ImageFile + "SlotImage.bmp");
                    break;

                case 3:
                    CBCameraSelect.SelectedIndex = 1;
                    numericUpDown_Expose.Value = Convert.ToDecimal(ImagePara.Instance.Exposure_RightCamGetSocket);
                    if (File.Exists(Utility.ImageFile + "SocketMarkImage.bmp"))
                        this.superWind1.image = new HImage(Utility.ImageFile + "SocketMarkImage.bmp");
                    break;

                case 4:
                    CBCameraSelect.SelectedIndex = 1;
                    numericUpDown_Expose.Value = Convert.ToDecimal(ImagePara.Instance.Exposure_LeftCamPutSocket);
                    if (File.Exists(Utility.ImageFile + "SocketDutImage1.bmp"))
                        this.superWind1.image = new HImage(Utility.ImageFile + "SocketDutImage1.bmp");
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
                                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotDutROI);
                                InputPara para = new InputPara(image, roi, null, Convert.ToDouble(this.numericUpDown_score_Traydut.Value));
                                result = AutoNormal_New.ImageProcess.TrayDutFront(para);
                                this.superWind1.image = image;
                                image.Dispose();
                                if (result.IsRunOk)
                                {
                                    this.superWind1.obj = result.shapeModelContour;
                                    HXLDCont Cross = new HXLDCont();
                                    HOperatorSet.TupleRad(result.Phi, out HTuple rad);
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
                                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.DutBackROI);
                                InputPara para = new InputPara(Image, roi,null, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));
                                result = AutoNormal_New.ImageProcess.SecondDutBack_Circle(para, new PLCSend() { Func = 1 });
                                this.superWind1.image = Image;
                                Image.Dispose();
                                if (result.IsRunOk)
                                {
                                    this.superWind1.obj = result.shapeModelContour;
                                    HXLDCont Cross = new HXLDCont();
                                    HOperatorSet.TupleRad(result.Phi, out HTuple rad);
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
                                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SocketDutROI);
                                InputPara para = new InputPara(Image,roi, null, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));
                                result = AutoNormal_New.ImageProcess.ShotSlot(para);
                                this.superWind1.image = Image;
                                Image.Dispose();
                                if (result.IsRunOk)
                                {
                                    this.superWind1.obj = result.shapeModelContour;
                                    HXLDCont Cross = new HXLDCont();
                                    HOperatorSet.TupleRad(result.Phi, out HTuple rad);
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
                                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SocketMarkROI);
                                InputPara para = new InputPara(Image, roi, null, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));
                                result = AutoNormal_New.ImageProcess.SocketMark(para, out HObject mark);
                                this.superWind1.image = Image;
                                Image.Dispose();
                                if (result.IsRunOk)
                                {
                                    this.superWind1.obj = result.shapeModelContour;
                                    HXLDCont Cross = new HXLDCont();
                                    HOperatorSet.TupleRad(result.Phi, out HTuple rad);
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
                                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SocketDutROI);
                                InputPara para = new InputPara(image, image.GetDomain(), null, Convert.ToDouble(this.numericUpDown_score_socktdut.Value));
                                result = AutoNormal_New.ImageProcess.SocketDutFront(para);
                                this.superWind1.image = image;
                                image.Dispose();
                                if (result.IsRunOk)
                                {
                                    this.superWind1.obj = result.region;
                                    this.superWind1.obj = result.shapeModelContour;
                                    this.superWind1.Message = string.Format("宽：{0} 高{1}", result.Dutwidth, result.Dutheight);
                                    HXLDCont Cross = new HXLDCont();
                                    HOperatorSet.TupleRad(result.Phi, out HTuple rad);
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
      
        string text;
        private void SetROI(Button button, ref System.Drawing.Rectangle rec)
        {
            if (button.Text != "结束")
            {
                text = button.Text;
                if (MessageBox.Show("确定进入更新设置", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (rec == null)
                        rec = new Rectangle();
                    HOperatorSet.CreateDrawingObjectRectangle1(rec.Y, rec.X, rec.Y + rec.Height + 10, rec.X + rec.Width + 10, out DrawID);
                    HOperatorSet.SetDrawingObjectParams(DrawID, "color", "red");
                    HOperatorSet.AttachDrawingObjectToWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                    button.BackColor = Color.Red;
                    button.Text = "结束";
                }
            }
            else
            {
                HOperatorSet.GetDrawingObjectIconic(out HObject obj, DrawID);
                HOperatorSet.SmallestRectangle1(obj, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                rec = new System.Drawing.Rectangle(col1, row1, col2 - col1, row2 - row1);
                HOperatorSet.DetachDrawingObjectFromWindow(this.superWind1.viewController.viewPort.HalconWindow, DrawID);
                button.Text = text;
                button.BackColor = Color.WhiteSmoke;
            }
        }
 
        HTuple DrawID;
        /// <summary>
        /// 上料DUT搜索区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_dutRoi_Click(object sender, EventArgs e)
        {

            SetROI(button_dutRoi, ref ImagePara.Instance.SlotDutROI);
            
            HOperatorSet.WriteImage(this.superWind1.image, "bmp", 0, Utility.ImageFile+"DutImage");
        }
        /// <summary>
        /// DUT背面搜索区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_dutbackROI_Click(object sender, EventArgs e)
        {
            SetROI(button_dutbackROI, ref ImagePara.Instance.DutBackROI);
            HOperatorSet.WriteImage(this.superWind1.image, "bmp", 0, Utility.ImageFile+"DutbackImage");
        }
        /// <summary>
        /// slot搜索区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_slotROI_Click(object sender, EventArgs e)
        {
            SetROI(button_slotROI, ref ImagePara.Instance.SlotROI);
        }
        /// <summary>
        /// Socket  Mark搜索区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SocketRoi_Click(object sender, EventArgs e)
        {
            SetROI(button_SocketRoi, ref ImagePara.Instance.SocketDutROI);
        }
        /// <summary>
        /// soket dut搜索区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_SocketDutRoi_Click(object sender, EventArgs e)
        {
            SetROI(button_SocketDutRoi, ref ImagePara.Instance.SocketMarkROI);
        }

        private void comboBox_socket_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoad)
                return;
            ImageProcess_Poc2.CurrentSoket = comboBox_socket.SelectedIndex;
            numericUpDown_socktdut_min.Value = ImagePara.Instance.SoketDut_minthreshold[ImageProcess_Poc2.CurrentSoket];
            numericUpDown_socktdut_max.Value = ImagePara.Instance.SoketDut_maxthreshold[ImageProcess_Poc2.CurrentSoket];
            var path = Utility.ImageFile + "SocketDutImage" + (comboBox_socket.SelectedIndex + 1).ToString()+".bmp";
            if (File.Exists(path))
            {
                this.superWind1.image = new HImage(path);
            }
        }

        private void btn_DutDetect_Click(object sender, EventArgs e)
        {
            try
            {
                if (AutoNormal_New.DutBackgroundDetect(superWind1.image))
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

        private void numericUpDown_PushBlock_ValueChanged(object sender, EventArgs e)
        {
            ImagePara.Instance.PushBlock = (HTuple)numericUpDown_PushBlock.Value;
        }
        /// <summary>
        /// 检查推块
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_checkBlock_Click(object sender, EventArgs e)
        {
            HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SocketMarkROI);
            InputPara locationPara = new InputPara(superWind1.image, roi, null, Convert.ToDouble(this.numericUpDown_score_socktMark.Value));
            AutoNormal_New.ImageProcess.FindSocketMarkFirst(locationPara.image, out HTuple row1, out HTuple col1,out HTuple phi,out HObject mark) ;
            if (row1.Length <= 0)
            {
                AutoNormal_New.ImageProcess.FindSocketMarkSecond(superWind1.image, out row1, out col1, out phi, out mark);
                if (row1.Length <= 0)
                {
                    AutoNormal_New.ImageProcess.FindSocketMarkThird(superWind1.image, out row1, out col1, out phi, out mark);
                }
            }
            if (row1.Length <=0 )
            {
                
                MessageBox.Show("Socket Mark 定位失败");
                return;
            }
            this.superWind1.obj = mark;
            mark.Dispose();
            roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SocketBlockROI);
            locationPara = new InputPara(superWind1.image, roi, null, Convert.ToDouble(this.numericUpDown_score_socktMark.Value));
            bool result=  AutoNormal_New.ImageProcess.BlockDetect(row1, col1, locationPara, out int distance,out HObject arrow);
            this.superWind1.obj = arrow;
            this.superWind1.Message = "推块距离:" + distance.ToString();
            if(!result)
            {
                MessageBox.Show("推块位置太短！");
            }
        }
        /// <summary>
        /// 新增配方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateRecipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                String name = Interaction.InputBox("输入配方名称", "新建配方", "", -1, -1);
                if (name == "")
                    return;
                if (System.IO.File.Exists(Utility.TypeFile + name))
                {
                    if (MessageBox.Show("替换已存在配方？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
                XmlHelper.Instance().SerializeToXml(Utility.TypeFile + name + ".xml", ImagePara.Instance);
                MessageBox.Show("新增成功:" + Utility.TypeFile + name, "提示", MessageBoxButtons.OK);
                ConfigMgr.Instance.CurrentImageType = name + ".xml";
            }
            catch (Exception ee)
            {
                AlcUtility.AlcSystem.Instance.ShowMsgBox("更新失败" + ee.Message, "提示", AlcUtility.AlcMsgBoxButtons.OK, AlcUtility.AlcMsgBoxIcon.Error);
            }
        }
        /// <summary>
        /// 保存配方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveRecipeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                XmlHelper.Instance().SerializeToXml(Utility.TypeFile + ConfigMgr.Instance.CurrentImageType, ImagePara.Instance);
                MessageBox.Show("保存OK");
            }
            catch (Exception ee)
            {
                AlcUtility.AlcSystem.Instance.ShowMsgBox("更新视觉配方失败", "提示", AlcUtility.AlcMsgBoxButtons.OK, AlcUtility.AlcMsgBoxIcon.Error);
            }
        }

        private void CompensateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOffset form = new FormOffset();
            form.Show();
            form.TopMost = true;
        }

        private void button_DutbackDataCodeRoi_Click(object sender, EventArgs e)
        {
            SetROI(sender as Button, ref ImagePara.Instance.DutBackDataCodeROI);
            HOperatorSet.WriteImage(this.superWind1.image, "bmp", 0, Utility.ImageFile + "DutbackImage");
        }
        /// <summary>
        /// 相机选择更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBCameraSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoad)
                return;
            this.CurrentCameraUserID =(EnumCamera)Enum.Parse(typeof(EnumCamera) , CBCameraSelect.SelectedIndex.ToString()) ;
            if(CameraManager.CameraById(CurrentCameraUserID.ToString()) !=null)
            {
                this.numericUpDown_Expose.Value = CameraManager.CameraById(CurrentCameraUserID.ToString()).ShuterCur;
                this.numericUpDown_Expose.Enabled = true;
            }
            else
            {
                this.numericUpDown_Expose.Enabled = false;
            }
        }
        /// <summary>
        /// 设置曝光
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numericUpDown_Expose_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoad)
                return;
                double ExposeTime = Convert.ToDouble(numericUpDown_Expose.Value);
                CameraManager.CameraById(CurrentCameraUserID.ToString()).ShuterCur = (long)(ExposeTime);
        }

        private void button_BlockROI_Click(object sender, EventArgs e)
        {
            SetROI(button_BlockROI, ref ImagePara.Instance.SocketBlockROI);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SetROI(sender as Button, ref ImagePara.Instance.TrayDutROI1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SetROI(button7, ref ImagePara.Instance.TrayDutROI2);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            HObject ho_Region, ho_RegionOpening;
            HObject ho_RegionFillUp, ho_RegionDifference, ho_ConnectedRegions;
            HObject ho_SelectedRegions, ho_RegionTrans, ho_Cross;
            HTuple hv_Area = new HTuple(), hv_Row = new HTuple();
            HTuple hv_Column = new HTuple(), hv_row = new HTuple();
            HTuple hv_col = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_RegionDifference);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            HOperatorSet.GenEmptyObj(out ho_Cross);


            ho_Region.Dispose();
            HOperatorSet.Threshold(this.superWind1.image, out ho_Region, 0, 50);
            ho_RegionOpening.Dispose();
            HOperatorSet.OpeningCircle(ho_Region, out ho_RegionOpening, 21);
            ho_RegionFillUp.Dispose();
            HOperatorSet.FillUp(ho_RegionOpening, out ho_RegionFillUp);
            ho_RegionDifference.Dispose();
            HOperatorSet.Difference(ho_RegionFillUp, ho_RegionOpening, out ho_RegionDifference
                );
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_RegionDifference, out ho_ConnectedRegions);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                "and", 120000, 200000);

            HObject ExpTmpOutVar_0;
            HOperatorSet.SelectShape(ho_SelectedRegions, out ExpTmpOutVar_0, "rectangularity",
                "and", 0.7, 1);


            HOperatorSet.CountObj(ExpTmpOutVar_0, out HTuple number);
            if (number != 2)
            {
                return;

            }
            ho_RegionTrans.Dispose();
            HOperatorSet.ShapeTrans(ho_SelectedRegions, out ho_RegionTrans, "rectangle2");
            hv_Area.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
            HOperatorSet.AreaCenter(ho_RegionTrans, out hv_Area, out hv_Row, out hv_Column);
            hv_row.Dispose();
            //粗定位mark点
            hv_row = ((hv_Row.TupleSelect(
                0)) + (hv_Row.TupleSelect(1))) / 2;
            hv_col = ((hv_Column.TupleSelect(
                0)) + (hv_Column.TupleSelect(1))) / 2;
            ImagePara.Instance.tray_rough_posi_row = hv_row;
            ImagePara.Instance.tray_rough_posi_col = hv_col;
            HOperatorSet.GenCrossContourXld(out HObject cross, hv_row, hv_col, 200, 0);
            this.superWind1.obj = cross;



        }

        private void btn_HaveOrNotRoi_Click(object sender, EventArgs e)
        {
            try
            {
                SetROI(sender as Button, ref ImagePara.Instance.SocketIsDutROI[comboBox_socket.SelectedIndex]);
                if (button_serch.Text != "结束")
                {
                    AutoNormal_New.ImageProcess.FindSocketMarkFirst(superWind1.image, out HTuple Row_mark, out HTuple Column_mark, out HTuple Phi_mark, out HObject mark);
                    if (Row_mark.Length <= 0)
                    {
                        AutoNormal_New.ImageProcess.FindSocketMarkSecond(superWind1.image, out Row_mark, out Column_mark, out Phi_mark, out mark);
                        if (Row_mark.Length <= 0)
                        {
                            AutoNormal_New.ImageProcess.FindSocketMarkThird(superWind1.image, out Row_mark, out Column_mark, out Phi_mark, out mark);
                        }
                    }
                    //mark点中心
                    if (Column_mark.Length > 0)
                    {
                        ImagePara.Instance.SocketDut_RoiRow[comboBox_socket.SelectedIndex] = Convert.ToSingle(Row_mark.D);
                        ImagePara.Instance.SocketDut_RoiCol[comboBox_socket.SelectedIndex] = Convert.ToSingle(Column_mark.D);
                        //ImagePara.Instance.SocketGet_angleCenter[comboBox_socket.SelectedIndex] = Convert.ToSingle(Phi_mark.D);
                    }

                    //HOperatorSet.WriteImage(this.superWind1.image, "bmp", 0, Utility.ImageFile + "SocketDutImage" + (comboBox_socket.SelectedIndex + 1).ToString());
                }
            }
            catch (Exception ex)
            {

                
            }
            
        }

        private void button_RangeArea_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.Threshold(this.superWind1.image, out HObject region, ImagePara.Instance.TrayDut_minthreshold, ImagePara.Instance.TrayDut_maxthreshold);
                HOperatorSet.AreaCenter(region, out HTuple area, out HTuple row, out HTuple col);
                this.superWind1.ObjColor = "red";
                this.superWind1.obj = region;
                MessageBox.Show("阈值范围内的面积为" + area.ToString());
            }
            catch (Exception ex)
            {
            }

        }

        private void ExposureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FormExposure formExposure = new FormExposure();
                formExposure.ShowDialog();
            }
            catch (Exception ex)
            {

                
            }


        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                bool flag = Utility.IsSaveAllImage;
                bool flag1 = Utility.IsSaveNgImage;
                AutoNormal_New.SaveImage("Nozzle1_Socket",this.superWind1.image);
            }
            catch (Exception ex)
            {

                
            }
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            try
            {
                AutoNormal_New.SaveOriginalImage("", "Get TrayDut", this.superWind1.image, false);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.Threshold(this.superWind1.image, out HObject region, 200, 255);
                HOperatorSet.DispObj(region, this.superWind1.hwind.HalconWindow);
                HOperatorSet.DumpWindowImage(out HObject image1, this.superWind1.hwind.HalconWindow);
                AutoNormal_New.SaveDumpImage("FNL123555", "PUT socket", image1, true);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}