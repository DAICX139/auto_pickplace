using AlcUtility.PlcDriver.CommonCtrl;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionControls;
using VisionModules;

namespace VisionFlows
{
    public partial class FormNewCalib : Form
    {
        private HShapeModel ShapeHandle;
        private double step = 0.6;
        private double x_current;
        private double y_current;

        public SerializableData serializableData = new SerializableData();

        //当前炒操作的工位
        private int WorkIndex = 0;

        public FormNewCalib()
        {
            InitializeComponent();
        }

        private void FormNewCalib_Load(object sender, EventArgs e)
        {
            image = new HImage("byte", 512, 512);
            this.superWind1.image = image;
            //反序列化
            if (File.Exists(Utility.calib+"calib_up.xml"))
            {
                serializableData = (SerializableData)Flow.XmlHelper.DeserializeFromXml(Utility.calib+"calib_up.xml", typeof(SerializableData));
            }
            dataGridView_up1.DataSource = serializableData.MoveCameraCalib1;
            dataGridView_up1.Refresh();
            dataGridView_up2.DataSource = serializableData.MoveCameraCalib2;
            dataGridView_up2.Refresh();
            //展示模板数据
            textBox_upCam_angle1.Text = serializableData.UpCam_angle1.ToString();
            textBox_upCam_angle2.Text = serializableData.UpCam_angle2.ToString();
            textBox_mode_angle1.Text = serializableData.mode_angle1.ToString();
            textBox_mode_angle2.Text = serializableData.mode_angle2.ToString();
            textBox_mode_row1.Text = serializableData.mode_row1.ToString();
            textBox_mode_row2.Text = serializableData.mode_row2.ToString();
            textBox_mode_col1.Text = serializableData.mode_col1.ToString();
            textBox_mode_col2.Text = serializableData.mode_col2.ToString();
            //读取标定数据
            try
            {
                HOperatorSet.ReadTuple(Utility.calib+"calib_down1", out serializableData.HomMat2D_down1);
                HOperatorSet.ReadTuple(Utility.calib+"calib_down2", out serializableData.HomMat2D_down2);
                HOperatorSet.ReadTuple(Utility.calib+"calib_up1", out serializableData.HomMat2D_up1);
                HOperatorSet.ReadTuple(Utility.calib+"calib_up2", out serializableData.HomMat2D_up2);
            }
            catch (Exception)
            {
            }
            this.superWind1.viewController.viewPort.HMouseDown += hWindowControl1_HMouseDown;
            superWind1.roiController.addRec1(100, 100, "");
        }

        private void FunctionTkCameraLive()
        {
            while (true)
            {
                GC.Collect();

                VisionModulesManager.CameraList[WorkIndex].CaptureImage();
                if (VisionModulesManager.CameraList[WorkIndex].CaptureSignal.WaitOne(Utility.CaptureDelayTime) && VisionModulesManager.CameraList[WorkIndex].Image != null && VisionModulesManager.CameraList[WorkIndex].Image.IsInitialized())
                {
                    image = VisionModulesManager.CameraList[WorkIndex].Image.CopyImage();
                    if (image != null && image.IsInitialized())
                    {
                        this.superWind1.image = image;
                        GetCenter(out double row, out double col);
                        GetXZCenter(out HTuple row1, out HTuple col1, out HTuple angle1);
                    }
                }
                if (!IsLiveRun)
                {
                    return;
                }
            }
        }

        private Task TkCameraLive;
        private bool IsLiveRun = true;
        private HImage image;

        private void button_shot_Click(object sender, EventArgs e)
        {
            if (button_shot.Text == "实时")
            {
                Plc.SetIO(WorkIndex + 1, true);
                button_shot.Text = "停止";
                button_shot.BackColor = Color.Red;
                IsLiveRun = true;
                TkCameraLive = new Task(FunctionTkCameraLive);
                TkCameraLive.Start();
            }
            else
            {
                button_shot.Text = "实时";
                button_shot.BackColor = Color.WhiteSmoke;
                IsLiveRun = false;
                System.Threading.Thread.Sleep(500);
                Plc.SetIO(WorkIndex + 1, false);
            }
        }

        private void numericUpDown_expore_ValueChanged(object sender, EventArgs e)
        {
            VisionModulesManager.CameraList[WorkIndex].SetExposureTime(Convert.ToDouble(numericUpDown_expore.Value));
        }

        /// <summary>
        /// 在标定初始位置上走相对偏移
        /// </summary>
        private void goxy_offe(double offx, double offy)
        {
            SingleAxisCtrl axis_x = Plc.PlcDriver?.GetSingleAxisCtrl(1);
            SingleAxisCtrl axis_y = Plc.PlcDriver?.GetSingleAxisCtrl(2);
            axis_x.AbsGo(x_current + offx, 30, true);
            axis_y.AbsGo(y_current + offy, 30, true);
        }

        /// <summary>
        /// 在标定初始位置走旋转 deg是绝对角度
        /// </summary>
        private void goAix(int aix, double deg)
        {
            //吸嘴1旋转是4号轴  吸嘴2是6号轴
            SingleAxisCtrl axis_r = Plc.PlcDriver?.GetSingleAxisCtrl(aix);
            axis_r.AbsGo(deg, 30, true);
        }

        private double GetAix(int aix)
        {
            //x=1 y=2
            SingleAxisCtrl axis = Plc.PlcDriver?.GetSingleAxisCtrl(aix);
            return axis.Info.ActPos;
        }

        private bool FindShape(out HTuple findrow, out HTuple findcol, out HTuple findangle)
        {
            if (ImageProcess_Poc2.Find_Shape(this.image, null, ShapeHandle, 0, 0.6, 1, 0.3,
               0, 360, out HTuple find_score, out findrow, out findcol, out findangle))
            {
                //获取模板轮廓
                HObject ho_ModelContours = ShapeHandle.GetShapeModelContours(1);
                //变换到目标物体上2
                HTuple hv_HomMat2DIdentity, hv_HomMat2DTranslate, hv_HomMat2DRotate;
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity, findrow, findcol, out hv_HomMat2DTranslate);
                HOperatorSet.HomMat2dRotate(hv_HomMat2DTranslate, findangle, findrow, findcol,
                    out hv_HomMat2DRotate);
                HObject origin_conturs;
                HOperatorSet.GenEmptyObj(out origin_conturs);
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out origin_conturs, hv_HomMat2DRotate);
                this.superWind1.obj = origin_conturs;
                origin_conturs.Dispose();
                ho_ModelContours.Dispose();
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button_Q_MouseDown(object sender, MouseEventArgs e)
        {
            int aix = Convert.ToInt32(((Button)sender).Tag);
            Plc.AxisJogGo(Math.Abs(aix), 20 * aix / Math.Abs(aix), true);
        }

        private void button_Q_MouseUp(object sender, MouseEventArgs e)
        {
            int aix = Convert.ToInt32(((Button)sender).Tag);
            Plc.AxisJogGo(Math.Abs(aix), 20 * aix / Math.Abs(aix), false);
        }

        private void button_up_MouseDown(object sender, MouseEventArgs e)
        {
            int aix = 3;
            if (tabControl1.SelectedIndex == 3 || tabControl1.SelectedIndex == 1)
                aix = 5;
            int speed = Convert.ToInt32(((Button)sender).Tag);
            Plc.AxisJogGo(Math.Abs(aix), 30 * speed, true);
        }

        private void button_up_MouseUp(object sender, MouseEventArgs e)
        {
            int aix = 3;
            if (tabControl1.SelectedIndex == 3 || tabControl1.SelectedIndex == 1)
                aix = 5;
            int speed = Convert.ToInt32(((Button)sender).Tag);
            Plc.AxisJogGo(Math.Abs(aix), 30 * speed, false);
        }

        private void button_down_base_Click(object sender, EventArgs e)
        {
            SingleAxisCtrl axis_x = Plc.PlcDriver?.GetSingleAxisCtrl(1);
            SingleAxisCtrl axis_y = Plc.PlcDriver?.GetSingleAxisCtrl(2);
            SingleAxisCtrl axis_z = Plc.PlcDriver?.GetSingleAxisCtrl(3);
            SingleAxisCtrl axis_r = Plc.PlcDriver?.GetSingleAxisCtrl(4);
            axis_x.AbsGo(250.907, 40, false);
            axis_y.AbsGo(450.8415, 40, false);
            axis_z.AbsGo(15.064, 40, false);
            axis_r.AbsGo(0, 40, false);
            MessageBox.Show("运动完成");
        }

        public void goXYAbs(double X, double Y, double vel = 40)
        {
            //默认40的运动速度
            SingleAxisCtrl axis_x = Plc.PlcDriver?.GetSingleAxisCtrl(1);
            SingleAxisCtrl axis_y = Plc.PlcDriver?.GetSingleAxisCtrl(2);
            axis_x.AbsGo(X, vel, false);
            axis_y.AbsGo(Y, vel, false);
        }

        private void button_up_Click(object sender, EventArgs e)
        {
        }

        private void button_create_Click(object sender, EventArgs e)
        {
            superWind1.viewController.viewPort.ContextMenuStrip = null;
            if (ImageProcess_Poc2.create_shape(this.superWind1.image, superWind1.hwind.HalconWindow, ImageProcess_Poc2.drawmodel(superWind1.hwind.HalconWindow, 1), 0, 360, ref ShapeHandle, out HTuple findrow, out HTuple findcolum, out HTuple findangle,
            out HObject oringconturs))
            {
                //HOperatorSet.WriteShapeModel(tool1.ShapeHandle, Application.StartupPath + "\\CalibModeA");
                HOperatorSet.DispObj(oringconturs, superWind1.hwind.HalconWindow);
            }
            else
            {
                MessageBox.Show("创建模板失败");
            }
            superWind1.viewController.viewPort.ContextMenuStrip = superWind1.viewController.hv_MenuStrip;
        }

        /// <summary>
        /// 下相机开始标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => { DownCameraCalib1(); });
        }

        private void DownCameraCalib1()
        {
            dataGridView_down1.Rows.Clear();
            int index = 0;
            //获取当前XY位置
            SingleAxisCtrl axis_x = Plc.PlcDriver?.GetSingleAxisCtrl(1);
            SingleAxisCtrl axis_y = Plc.PlcDriver?.GetSingleAxisCtrl(2);
            x_current = axis_x.Info.ActPos;
            y_current = axis_y.Info.ActPos;
            StaticCameraCalib calibdata = new StaticCameraCalib();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    index++;
                    //移动
                    Plc.SetIO(WorkIndex + 1, true);
                    goxy_offe(step * i, step * j);
                    //拍照
                    VisionModulesManager.CameraList[WorkIndex].CaptureImage();
                    VisionModulesManager.CameraList[WorkIndex].CaptureSignal.WaitOne(Utility.CaptureDelayTime);
                    image = VisionModulesManager.CameraList[WorkIndex].Image;
                    this.superWind1.image = image;

                    Plc.SetIO(WorkIndex + 1, false);

                    if (!FindShape(out HTuple find_row, out HTuple find_column, out HTuple angle))

                    {
                        MessageBox.Show("匹配失败");
                        return;
                    }
                    //前九个点
                    string[] temp = new string[] { find_row.D.ToString("f1"), find_column.D.ToString("f1"), GetAix(1).ToString("f3"), GetAix(2).ToString("f3") };
                    this.dataGridView_down1.Rows.Add(temp);
                    HOperatorSet.TupleConcat(calibdata.ALL_row1, find_row, out calibdata.ALL_row1);
                    HOperatorSet.TupleConcat(calibdata.ALL_colum1, find_column, out calibdata.ALL_colum1);
                    HOperatorSet.TupleConcat(calibdata.ALL_X1, GetAix(1), out calibdata.ALL_X1);
                    HOperatorSet.TupleConcat(calibdata.ALL_Y1, GetAix(2), out calibdata.ALL_Y1);

                    Thread.Sleep(500);
                }
            }

            //旋转点

            for (int t = 0; t < 5; t++)
            {
                Plc.SetIO(WorkIndex + 1, true);
                goxy_offe(0, 0);
                goAix(4, t * 20);
                //拍照
                VisionModulesManager.CameraList[WorkIndex].CaptureImage();
                VisionModulesManager.CameraList[WorkIndex].CaptureSignal.WaitOne(Utility.CaptureDelayTime);
                image = VisionModulesManager.CameraList[WorkIndex].Image;
                this.superWind1.image = image;
                if (!FindShape(out HTuple find_row, out HTuple find_column, out HTuple angle))
                {
                    MessageBox.Show("匹配失败");
                    return;
                }
                HOperatorSet.TupleConcat(calibdata.rotate_row, find_row, out calibdata.rotate_row);
                HOperatorSet.TupleConcat(calibdata.rotate_colum, find_column, out calibdata.rotate_colum);
            }

            //开始标定
            try
            {
                //计算旋转中心
                HObject Contour;
                HTuple hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder;
                HOperatorSet.GenContourPolygonXld(out Contour, calibdata.rotate_row, calibdata.rotate_colum);
                HOperatorSet.FitCircleContourXld(Contour, "geotukey", -1, 0, 0, 3, 2, out HTuple hv_RowCenter1,
       out HTuple hv_ColCenter1, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                //显示3个点
                this.superWind1.ObjColor = "red";
                HOperatorSet.GenCrossContourXld(out HObject cross, calibdata.rotate_row, calibdata.rotate_colum, 156, 0);
                this.superWind1.obj = cross;
                //显示旋转中心
                this.superWind1.ObjColor = "green";
                HOperatorSet.GenCrossContourXld(out cross, hv_RowCenter1, hv_ColCenter1, 156, 0);
                this.superWind1.obj = cross;
                serializableData.Rotate_Center_Row1 = hv_RowCenter1;
                serializableData.Rotate_Center_Col1 = hv_ColCenter1;
                if (hv_RowCenter1.Length <= 0)
                {
                    MessageBox.Show("计算旋转中心失败");
                }
                else
                {
                    HOperatorSet.VectorToHomMat2d(calibdata.ALL_colum1, calibdata.ALL_row1, calibdata.ALL_X1, calibdata.ALL_Y1, out serializableData.HomMat2D_down1);
                    HOperatorSet.WriteTuple(serializableData.HomMat2D_down1, Utility.calib+"calib_down1");
                    MessageBox.Show("下相机标定成功");
                }
            }
            catch
            {
                MessageBox.Show("生成标定数据失败");
            }
        }

        private void button_down_calib2_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => { DownCameraCalib2(); });
        }

        private void DownCameraCalib2()
        {
            dataGridView_down2.Rows.Clear();
            int index = 0;
            //获取当前XY位置
            SingleAxisCtrl axis_x = Plc.PlcDriver?.GetSingleAxisCtrl(1);
            SingleAxisCtrl axis_y = Plc.PlcDriver?.GetSingleAxisCtrl(2);
            x_current = axis_x.Info.ActPos;
            y_current = axis_y.Info.ActPos;
            StaticCameraCalib calibdata = new StaticCameraCalib();
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    index++;
                    //移动
                    Plc.SetIO(WorkIndex + 1, true);
                    goxy_offe(step * i, step * j);
                    //拍照
                    VisionModulesManager.CameraList[WorkIndex].CaptureImage();
                    VisionModulesManager.CameraList[WorkIndex].CaptureSignal.WaitOne(Utility.CaptureDelayTime);
                    image = VisionModulesManager.CameraList[WorkIndex].Image;
                    this.superWind1.image = image;

                    Plc.SetIO(WorkIndex + 1, false);

                    if (!FindShape(out HTuple find_row, out HTuple find_column, out HTuple angle))

                    {
                        MessageBox.Show("匹配失败");
                        return;
                    }
                    //前九个点
                    string[] temp = new string[] { find_row.D.ToString("f1"), find_column.D.ToString("f1"), GetAix(1).ToString("f3"), GetAix(2).ToString("f3") };
                    this.dataGridView_down2.Rows.Add(temp);
                    HOperatorSet.TupleConcat(calibdata.ALL_row1, find_row, out calibdata.ALL_row1);
                    HOperatorSet.TupleConcat(calibdata.ALL_colum1, find_column, out calibdata.ALL_colum1);
                    HOperatorSet.TupleConcat(calibdata.ALL_X1, GetAix(1), out calibdata.ALL_X1);
                    HOperatorSet.TupleConcat(calibdata.ALL_Y1, GetAix(2), out calibdata.ALL_Y1);

                    Thread.Sleep(500);
                }
            }

            //旋转点

            for (int t = 0; t < 5; t++)
            {
                Plc.SetIO(WorkIndex + 1, true);
                goxy_offe(0, 0);
                goAix(6, t * 20);
                //拍照
                VisionModulesManager.CameraList[WorkIndex].CaptureImage();
                VisionModulesManager.CameraList[WorkIndex].CaptureSignal.WaitOne(Utility.CaptureDelayTime);
                image = VisionModulesManager.CameraList[WorkIndex].Image;
                this.superWind1.image = image;
                if (!FindShape(out HTuple find_row, out HTuple find_column, out HTuple angle))
                {
                    MessageBox.Show("匹配失败");
                    return;
                }
                HOperatorSet.TupleConcat(calibdata.rotate_row, find_row, out calibdata.rotate_row);
                HOperatorSet.TupleConcat(calibdata.rotate_colum, find_column, out calibdata.rotate_colum);
            }

            //开始标定
            try
            {
                //计算旋转中心
                HObject Contour;
                HTuple hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder;
                HOperatorSet.GenContourPolygonXld(out Contour, calibdata.rotate_row, calibdata.rotate_colum);
                HOperatorSet.FitCircleContourXld(Contour, "geotukey", -1, 0, 0, 3, 2, out HTuple hv_RowCenter1,
       out HTuple hv_ColCenter1, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                this.superWind1.ObjColor = "red";
                HOperatorSet.GenCrossContourXld(out HObject cross, calibdata.rotate_row, calibdata.rotate_colum, 156, 0);
                this.superWind1.obj = cross;
                //显示旋转中心
                this.superWind1.ObjColor = "green";
                HOperatorSet.GenCrossContourXld(out cross, hv_RowCenter1, hv_ColCenter1, 156, 0);
                this.superWind1.obj = cross;
                serializableData.Rotate_Center_Row2 = hv_RowCenter1;
                serializableData.Rotate_Center_Col2 = hv_ColCenter1;
                if (hv_RowCenter1.Length <= 0)
                {
                    MessageBox.Show("计算旋转中心失败");
                }
                else
                {
                    HOperatorSet.VectorToHomMat2d(calibdata.ALL_colum1, calibdata.ALL_row1, calibdata.ALL_X1, calibdata.ALL_Y1, out serializableData.HomMat2D_down2);
                    HOperatorSet.WriteTuple(serializableData.HomMat2D_down2,Utility.calib+ "calib_down2");
                    MessageBox.Show("下相机标定成功");
                }
            }
            catch
            {
                MessageBox.Show("生成标定数据失败");
            }
        }

        private void button_down_base2_Click(object sender, EventArgs e)
        {
            SingleAxisCtrl axis_x = Plc.PlcDriver?.GetSingleAxisCtrl(1);
            SingleAxisCtrl axis_y = Plc.PlcDriver?.GetSingleAxisCtrl(2);
            SingleAxisCtrl axis_z = Plc.PlcDriver?.GetSingleAxisCtrl(3);
            SingleAxisCtrl axis_r = Plc.PlcDriver?.GetSingleAxisCtrl(6);
            axis_x.AbsGo(-1.846, 40, false);
            axis_y.AbsGo(450.8415, 40, false);
            axis_z.AbsGo(15.064, 40, false);
            axis_r.AbsGo(0, 40, false);
            MessageBox.Show("完成");
        }

        /// <summary>
        /// 抓拍
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_grap_Click(object sender, EventArgs e)
        {
            VisionModulesManager.CameraList[WorkIndex].CaptureImage();
            VisionModulesManager.CameraList[WorkIndex].CaptureSignal.WaitOne(Utility.CaptureDelayTime);

            image = VisionModulesManager.CameraList[WorkIndex].Image;
            this.image = image;
        }

        private void GrapImage(int index)
        {
            Plc.SetIO(index + 1, true);
            Thread.Sleep(200);
            if (index == 2)
            {
                VisionModulesManager.CameraList[index].SetExposureTime(8000);
            }
            VisionModulesManager.CameraList[index].CaptureImage();
            VisionModulesManager.CameraList[index].CaptureSignal.WaitOne(Utility.CaptureDelayTime);
            image = VisionModulesManager.CameraList[index].Image;
            this.image = image;
            Plc.SetIO(index + 1, false);
        }

        /// <summary>
        /// 拍照并获取拍照位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_photoAndGetCenter1_Click(object sender, EventArgs e)
        {
            //获取当前XY位置
            GetXY(out double X, out double Y);
            //获取mark位置
            if (!GetCenter(out double row, out double col))
            {
                MessageBox.Show("识别mark点失败");
                return;
            }

            if (serializableData.MoveCameraCalib1.Count <= 0)
            {
                serializableData.MoveCameraCalib1.Add(new MoveCameraCalib(row, col, X, Y, 0, 0));
            }
            else if (serializableData.MoveCameraCalib1[serializableData.MoveCameraCalib1.Count - 1].tool1_x == 0 || serializableData.MoveCameraCalib1[serializableData.MoveCameraCalib1.Count - 1].row == 0)
            {
                serializableData.MoveCameraCalib1[serializableData.MoveCameraCalib1.Count - 1].row = row;
                serializableData.MoveCameraCalib1[serializableData.MoveCameraCalib1.Count - 1].colum = col;
                serializableData.MoveCameraCalib1[serializableData.MoveCameraCalib1.Count - 1].X = X;
                serializableData.MoveCameraCalib1[serializableData.MoveCameraCalib1.Count - 1].Y = Y;
            }
            else
            {
                serializableData.MoveCameraCalib1.Add(new MoveCameraCalib(row, col, X, Y, 0, 0));
            }

            dataGridView_up1.DataSource = serializableData.MoveCameraCalib1;
            dataGridView_up1.Refresh();
        }

        /// <summary>
        /// 获取对中位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_getcenter1_Click(object sender, EventArgs e)
        {
            GetXY(out double tool1_x, out double tool1_y);
            if (serializableData.MoveCameraCalib1.Count <= 0)
            {
                serializableData.MoveCameraCalib1.Add(new MoveCameraCalib(0, 0, 0, 0, tool1_x, tool1_y));
            }
            else if (serializableData.MoveCameraCalib1[serializableData.MoveCameraCalib1.Count - 1].tool1_x == 0 || serializableData.MoveCameraCalib1[serializableData.MoveCameraCalib1.Count - 1].row == 0)
            {
                serializableData.MoveCameraCalib1[serializableData.MoveCameraCalib1.Count - 1].tool1_x = tool1_x;
                serializableData.MoveCameraCalib1[serializableData.MoveCameraCalib1.Count - 1].tool1_y = tool1_y;
            }
            else
            {
                serializableData.MoveCameraCalib1.Add(new MoveCameraCalib(0, 0, 0, 0, tool1_x, tool1_y));
            }

            dataGridView_up1.DataSource = serializableData.MoveCameraCalib1;
            dataGridView_up1.Refresh();
        }

        /// <summary>
        /// 上相机2标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_calib2_Click(object sender, EventArgs e)
        {
            HTuple ALL_row = new HTuple();
            HTuple ALL_col = new HTuple();
            HTuple ALL_X = new HTuple();
            HTuple ALL_Y = new HTuple();
            HTuple ALL_tool1_X = new HTuple();
            HTuple ALL_tool1_Y = new HTuple();
            for (int i = 0; i < serializableData.MoveCameraCalib2.Count; i++)
            {
                HTuple temp = serializableData.MoveCameraCalib2[i].row;
                HOperatorSet.TupleConcat(ALL_row, temp, out ALL_row);

                temp = serializableData.MoveCameraCalib2[i].colum;
                HOperatorSet.TupleConcat(ALL_col, temp, out ALL_col);

                temp = serializableData.MoveCameraCalib2[i].tool1_x;
                HOperatorSet.TupleConcat(ALL_tool1_X, temp, out ALL_tool1_X);

                temp = serializableData.MoveCameraCalib2[i].tool1_y;
                HOperatorSet.TupleConcat(ALL_tool1_Y, temp, out ALL_tool1_Y);

                temp = serializableData.MoveCameraCalib2[i].tool1_x - serializableData.MoveCameraCalib2[i].X;
                HOperatorSet.TupleConcat(ALL_X, temp, out ALL_X);

                temp = serializableData.MoveCameraCalib2[i].tool1_y - serializableData.MoveCameraCalib2[i].Y;
                HOperatorSet.TupleConcat(ALL_Y, temp, out ALL_Y);
            }
            HOperatorSet.VectorToHomMat2d(ALL_col, ALL_row, ALL_X, ALL_Y, out serializableData.HomMat2D_up2);
            HOperatorSet.WriteTuple(serializableData.HomMat2D_up2, Utility.calib+"calib_up2");
            MessageBox.Show("标定成功");
        }

        /// <summary>
        /// 相机1标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_calib1_Click(object sender, EventArgs e)
        {
            HTuple ALL_row = new HTuple();
            HTuple ALL_col = new HTuple();
            HTuple ALL_X = new HTuple();
            HTuple ALL_Y = new HTuple();
            HTuple ALL_tool1_X = new HTuple();
            HTuple ALL_tool1_Y = new HTuple();
            for (int i = 0; i < serializableData.MoveCameraCalib1.Count; i++)
            {
                HTuple temp = serializableData.MoveCameraCalib1[i].row;
                HOperatorSet.TupleConcat(ALL_row, temp, out ALL_row);

                temp = serializableData.MoveCameraCalib1[i].colum;
                HOperatorSet.TupleConcat(ALL_col, temp, out ALL_col);

                temp = serializableData.MoveCameraCalib1[i].tool1_x;
                HOperatorSet.TupleConcat(ALL_tool1_X, temp, out ALL_tool1_X);

                temp = serializableData.MoveCameraCalib1[i].tool1_y;
                HOperatorSet.TupleConcat(ALL_tool1_Y, temp, out ALL_tool1_Y);

                temp = serializableData.MoveCameraCalib1[i].tool1_x - serializableData.MoveCameraCalib1[i].X;
                HOperatorSet.TupleConcat(ALL_X, temp, out ALL_X);

                temp = serializableData.MoveCameraCalib1[i].tool1_y - serializableData.MoveCameraCalib1[i].Y;
                HOperatorSet.TupleConcat(ALL_Y, temp, out ALL_Y);
            }
            HOperatorSet.VectorToHomMat2d(ALL_col, ALL_row, ALL_X, ALL_Y, out serializableData.HomMat2D_up1);
            HOperatorSet.WriteTuple(serializableData.HomMat2D_up1, Utility.calib+"calib_up1");
            MessageBox.Show("标定成功");
        }

        /// <summary>
        /// 上相机2拍照并获取拍照中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_photoAndGetCenter2_Click(object sender, EventArgs e)
        {
            //获取当前XY位置
            GetXY(out double X, out double Y);
            //获取mark位置
            if (!GetCenter(out double row, out double col))
            {
                MessageBox.Show("识别mark点失败");
                return;
            }
            if (serializableData.MoveCameraCalib2.Count <= 0)
            {
                serializableData.MoveCameraCalib2.Add(new MoveCameraCalib(row, col, X, Y, 0, 0));
            }
            else if (serializableData.MoveCameraCalib2[serializableData.MoveCameraCalib2.Count - 1].tool1_x == 0 || serializableData.MoveCameraCalib2[serializableData.MoveCameraCalib2.Count - 1].row == 0)
            {
                serializableData.MoveCameraCalib2[serializableData.MoveCameraCalib2.Count - 1].row = row;
                serializableData.MoveCameraCalib2[serializableData.MoveCameraCalib2.Count - 1].colum = col;
                serializableData.MoveCameraCalib2[serializableData.MoveCameraCalib2.Count - 1].X = X;
                serializableData.MoveCameraCalib2[serializableData.MoveCameraCalib2.Count - 1].Y = Y;
            }
            else
            {
                serializableData.MoveCameraCalib2.Add(new MoveCameraCalib(row, col, X, Y, 0, 0));
            }

            dataGridView_up2.DataSource = serializableData.MoveCameraCalib2;
            dataGridView_up2.Refresh();
        }

        /// <summary>
        /// 相机2获取对中位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_getcenter2_Click(object sender, EventArgs e)
        {
            GetXY(out double tool1_x, out double tool1_y);
            if (serializableData.MoveCameraCalib2.Count <= 0)
            {
                serializableData.MoveCameraCalib2.Add(new MoveCameraCalib(0, 0, 0, 0, tool1_x, tool1_y));
            }
            else if (serializableData.MoveCameraCalib2[serializableData.MoveCameraCalib2.Count - 1].tool1_x == 0 || serializableData.MoveCameraCalib2[serializableData.MoveCameraCalib2.Count - 1].row == 0)
            {
                serializableData.MoveCameraCalib2[serializableData.MoveCameraCalib2.Count - 1].tool1_x = tool1_x;
                serializableData.MoveCameraCalib2[serializableData.MoveCameraCalib2.Count - 1].tool1_y = tool1_y;
            }
            else
            {
                serializableData.MoveCameraCalib2.Add(new MoveCameraCalib(0, 0, 0, 0, tool1_x, tool1_y));
            }

            dataGridView_up2.DataSource = serializableData.MoveCameraCalib2;
            dataGridView_up2.Refresh();
        }

        /// <summary>
        /// 获取XY当前位置
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        private void GetXY(out double X, out double Y)
        {
            try
            {
                SingleAxisCtrl axis_x = Plc.PlcDriver?.GetSingleAxisCtrl(1);
                SingleAxisCtrl axis_y = Plc.PlcDriver?.GetSingleAxisCtrl(2);
                X = axis_x.Info.ActPos;
                Y = axis_y.Info.ActPos;
            }
            catch (Exception)
            {
                X = 0;
                Y = 0;
            }
        }

        private bool GetCenter(out double row, out double col)
        {
            HOperatorSet.BinaryThreshold(this.image, out HObject ho_Region, "max_separability", "dark",
        out HTuple hv_UsedThreshold);
            HOperatorSet.FillUp(ho_Region, out HObject ho_RegionFillUp);
            HOperatorSet.Connection(ho_RegionFillUp, out HObject ho_ConnectedRegions);
            HOperatorSet.SelectShape(ho_ConnectedRegions, out HObject ho_SelectedRegions, (new HTuple("circularity")).TupleConcat(
                "outer_radius"), "and", (new HTuple(0.88577)).TupleConcat(190.39), (new HTuple(1)).TupleConcat(302.75));
            HOperatorSet.CountObj(ho_SelectedRegions, out HTuple hv_Number);
            if ((int)(new HTuple(hv_Number.TupleEqual(1))) != 0)
            {
                HOperatorSet.SmallestCircle(ho_SelectedRegions, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Radius);
                this.superWind1.ObjColor = "red";
                HOperatorSet.GenCrossContourXld(out HObject cross, hv_Row, hv_Column, 76, 0.7);
                this.superWind1.obj = cross;
                row = hv_Row.D;
                col = hv_Column.D;
                return true;
            }
            row = 0;
            col = 0;
            return false;
        }

        private void GetXZCenter(out HTuple row, out HTuple col, out HTuple angle)
        {
            row = 0;
            col = 0;
            angle = 0;
            if (image != null && image.IsInitialized())
            {
                HOperatorSet.ReduceDomain(image, ((ROI)(superWind1.roiController.RoiInfo.ROIList[0])).getRegion(), out HObject reduced);
                HOperatorSet.Threshold(reduced, out HObject region, 0, 166);
                HOperatorSet.Connection(region, out HObject regionConnect);
                HOperatorSet.SelectShape(regionConnect, out HObject ho_SelectedRegions, "area",
             "and", 150, 100000);
                HOperatorSet.ShapeTrans(ho_SelectedRegions, out HObject ho_RegionTrans, "convex");

                HOperatorSet.Union1(ho_RegionTrans, out HObject ho_RegionUnion);

                HOperatorSet.SmallestRectangle2(ho_RegionUnion, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Phi,
                    out HTuple hv_Length1, out HTuple hv_Length2);
                if (hv_Row.Length <= 0)
                    return;
                HOperatorSet.GenRectangle2(out HObject ho_Rectangle, hv_Row, hv_Column, hv_Phi, hv_Length1,
                    hv_Length2);
                row = hv_Row;
                col = hv_Column;
                HOperatorSet.TupleDeg(hv_Phi, out angle);
                this.superWind1.obj = ho_Rectangle;
                HOperatorSet.GenCrossContourXld(out HObject cross, hv_Row, hv_Column, 146, hv_Phi);
                this.superWind1.obj = cross;
                region.Dispose();
                regionConnect.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionTrans.Dispose();
                ho_Rectangle.Dispose();
            }
        }

        private void button_clearup1_Click(object sender, EventArgs e)
        {
            serializableData.MoveCameraCalib1.Clear();
            dataGridView_up1.DataSource = serializableData.MoveCameraCalib1;
            dataGridView_up1.Refresh();
        }

        private void button_clearup2_Click(object sender, EventArgs e)
        {
            serializableData.MoveCameraCalib2.Clear();
            dataGridView_up2.DataSource = serializableData.MoveCameraCalib2;
            dataGridView_up2.Refresh();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormNewCalib_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsLiveRun = false;
            Thread.Sleep(300);
            Flow.XmlHelper.SerializeToXml(Utility.calib+"calib_up.xml", serializableData);
        }

        /// <summary>
        /// 注册为模板图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_register_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                HOperatorSet.WriteImage(superWind1.image, "bmp", 0, "ImageMode1.bmp");
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                HOperatorSet.WriteImage(superWind1.image, "bmp", 0, "ImageMode2.bmp");
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                HOperatorSet.WriteImage(superWind1.image, "bmp", 0, "ImageMode3.bmp");
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                HOperatorSet.WriteImage(superWind1.image, "bmp", 0, "ImageMode4.bmp");
            }
        }

        private void hWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {  //上相机1
            if (image.IsInitialized() && e.Clicks == 2 && WorkIndex == 0)
            {
                HOperatorSet.GetImageSize(image, out HTuple m_with, out HTuple m_hight);
                if (e.Y > m_hight || e.X > m_with || e.Y < 0 || e.X < 0)
                    return;
                double row = e.Y;
                double colum = e.X;
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, colum, row, out HTuple transX, out HTuple transY);
                GetXY(out double x_current, out double y_current);
                goXYAbs(transX.D + x_current, transY.D + y_current);
            }
            //上相机2
            else if (image.IsInitialized() && e.Clicks == 2 && WorkIndex == 1)
            {
                HOperatorSet.GetImageSize(image, out HTuple m_with, out HTuple m_hight);
                if (e.Y > m_hight || e.X > m_with || e.Y < 0 || e.X < 0)
                    return;
                double row = e.Y;
                double colum = e.X;
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up2, colum, row, out HTuple transX, out HTuple transY);
                GetXY(out double x_current, out double y_current);
                goXYAbs(transX.D + x_current, transY.D + y_current);
            }
        }

        private void button_Reference_nozzle1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要更新参吸嘴1考坐标？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                GetXY(out x_current, out y_current);
                serializableData.X1 = x_current;
                serializableData.Y1 = y_current;
            }
        }

        private void button_Reference_cam1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要更新上相机1参考坐标？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                GetXY(out x_current, out y_current);
                serializableData.XShot1 = x_current;
                serializableData.YShot1 = y_current;
                GetXZCenter(out HTuple row1, out HTuple col1, out HTuple angle1);
                this.textBox_upCam_angle1.Text = angle1.D.ToString("f3");
            }
        }

        private void button_Reference_nozzle2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要更新吸嘴2参考坐标？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                GetXY(out x_current, out y_current);
                serializableData.X2 = x_current;
                serializableData.Y2 = y_current;
            }
        }

        private void button_Reference_cam2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要更新上相机2参考坐标？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                GetXY(out x_current, out y_current);
                serializableData.XShot2 = x_current;
                serializableData.YShot2 = y_current;
                GetXZCenter(out HTuple row1, out HTuple col1, out HTuple angle1);
                this.textBox_upCam_angle2.Text = angle1.D.ToString("f3");
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                WorkIndex = 0;
                if (File.Exists("ImageMode1.bmp"))
                {
                    this.superWind1.image = new HImage("ImageMode1.bmp");
                }
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                WorkIndex = 1;
                if (File.Exists("ImageMode2.bmp"))
                {
                    this.superWind1.image = new HImage("ImageMode2.bmp");
                }
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                WorkIndex = 2;
                if (File.Exists("ImageMode3.bmp"))
                {
                    this.superWind1.image = new HImage("ImageMode3.bmp");
                }
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                WorkIndex = 2;
                if (File.Exists("ImageMode4.bmp"))
                {
                    this.superWind1.image = new HImage("ImageMode4.bmp");
                }
            }
        }

        /// <summary>
        /// 计算出PLC最终要移动的位置
        /// </summary>
        private double cal_X = 0;

        private double cal_Y = 0;
        private double cal_R = 0;

        //下相机
        private HTuple down_row;

        private HTuple down_col;
        private HTuple down_angle;

        private void button_upshot1_Click(object sender, EventArgs e)
        {
            GrapImage(0);
            InputPara P = new InputPara(this.image,image.FullDomain(), new HShapeModel(), 0.3);
            OutPutResult result = AutoNormal_New.ImageProcess.TrayDutFront(P);
            if (!result.IsRunOk)
            {
                MessageBox.Show("识别失败");
                return;
            }
            HOperatorSet.GenCrossContourXld(out HObject cross, result.findPoint.Row, result.findPoint.Column, 145, 0);
            this.superWind1.obj = cross;
            double CurrentAngle = GetAix(4);
            cal_R = CurrentAngle - (result.Angle - serializableData.UpCam_angle1);
            HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, result.findPoint.Column, result.findPoint.Row, out HTuple transX, out HTuple transY);
            double x_current = GetAix(1);
            double y_current = GetAix(2);
            double offx = serializableData.XShot1 - serializableData.X1;
            double offy = serializableData.YShot1 - serializableData.Y1;
            cal_X = transX.D + x_current - offx;
            cal_Y = transY.D + y_current - offy;
        }

        private void button_PLC_get1_Click(object sender, EventArgs e)
        {
            goXYAbs(cal_X, cal_Y);
            goAix(4, cal_R);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            goXYAbs(250.907, 450.8415);
            goAix(4, 0);
            goAix(3, 15.064);
        }

        private void button_test_downshot_Click(object sender, EventArgs e)
        {
            GrapImage(2);
            InputPara P = new InputPara(this.image, image.FullDomain(), new HShapeModel(Utility.Model + "SecondDutBack.sbm"),0.3);
            OutPutResult result = AutoNormal_New.ImageProcess.SecondDutBack(P, new PLCSend() { Func = 1 });
            //显示找到的目标
            HOperatorSet.GenCrossContourXld(out HObject cross, result.findPoint.Row, result.findPoint.Column, 145, 0);
            this.superWind1.obj = cross;

            //记录二次定位的物料位置和角度
            down_row = result.findPoint.Row;
            down_col = result.findPoint.Column;
            down_angle = result.Angle;
        }

        private void button_toslot_Click(object sender, EventArgs e)
        {
            goXYAbs(376.615, 358.3795);
            goAix(4, -90);
            goAix(3, 0);
        }

        private void button_upshot1_mark_Click(object sender, EventArgs e)
        {
            GrapImage(0);
            InputPara P = new InputPara(this.image, image.FullDomain(), new HShapeModel(Utility.Model + "TrayMark.sbm"), 0.3);
            OutPutResult result = AutoNormal_New.ImageProcess.ShotSlotMark(P);
            HOperatorSet.GenCrossContourXld(out HObject cross, result.findPoint.Row, result.findPoint.Column, 145, 0);
            this.superWind1.obj = cross;
            //当前角度
            double CurrentAngle = GetAix(4);
            //下相机角度偏差
            double downcam_angle = down_angle - serializableData.mode_angle1;
            //上相机角度偏差
            double upcam_angle = result.Angle - serializableData.UpCam_angle1;
            //当前拍照角度加上偏差值
            cal_R = CurrentAngle + upcam_angle - downcam_angle;

            //物料中心按照旋转中心旋转回去
            HOperatorSet.HomMat2dIdentity(out HTuple homate);
            HOperatorSet.TupleRad(upcam_angle + downcam_angle, out HTuple phi);
            HOperatorSet.HomMat2dRotate(homate, phi, serializableData.Rotate_Center_Row1, serializableData.Rotate_Center_Col1, out HTuple homate_rotate);
            HOperatorSet.AffineTransPoint2d(homate_rotate, down_row, down_col, out HTuple trans_findrow, out HTuple trans_findcolum);

            //像素点转换成实际的PLC点位置
            HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down1, trans_findrow, trans_findcolum, out HTuple transX, out HTuple transY);
            HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down1, serializableData.mode_row1, serializableData.mode_col1, out HTuple transX_mode, out HTuple transY_mode);
            //下相机的偏差
            double offx_down = transX_mode.D - transX.D;
            double offy_down = transY_mode.D - transY.D;

            //上相机偏差
            HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, result.findPoint.Column, result.findPoint.Row, out HTuple transX_up, out HTuple transY_up);
            double x_current = GetAix(1);
            double y_current = GetAix(2);
            double offx = serializableData.XShot1 - serializableData.X1;
            double offy = serializableData.YShot1 - serializableData.Y1;
            cal_X = transX_up.D + x_current - offx + offx_down;
            cal_Y = transY_up.D + y_current - offy + offy_down;
        }

        private void button_plc_put_Click(object sender, EventArgs e)
        {
            goXYAbs(cal_X, cal_Y);
            goAix(4, cal_R);
        }

        private void textBox_upCam_angle1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                serializableData.UpCam_angle1 = Convert.ToDouble(textBox_upCam_angle1.Text);
            }
            catch (Exception)
            {
            }
        }

        private void textBox_upCam_angle2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                serializableData.UpCam_angle2 = Convert.ToDouble(textBox_upCam_angle2.Text);
            }
            catch (Exception)
            {
            }
        }

        private void textBox_mode_angle1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                serializableData.mode_angle1 = Convert.ToDouble(textBox_mode_angle1.Text);
            }
            catch (Exception)
            {
            }
        }

        private void textBox_mode_row1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                serializableData.mode_row1 = Convert.ToDouble(textBox_mode_row1.Text);
            }
            catch (Exception)
            {
            }
        }

        private void textBox_mode_col1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                serializableData.mode_col1 = Convert.ToDouble(textBox_mode_col1.Text);
            }
            catch (Exception)
            {
            }
        }

        private void textBox_mode_angle2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                serializableData.mode_angle2 = Convert.ToDouble(textBox_mode_angle2.Text);
            }
            catch (Exception)
            {
            }
        }

        private void textBox_mode_row2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                serializableData.mode_row2 = Convert.ToDouble(textBox_mode_row2.Text);
            }
            catch (Exception)
            {
            }
        }

        private void textBox_mode_col2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                serializableData.mode_col2 = Convert.ToDouble(textBox_mode_col2.Text);
            }
            catch (Exception)
            {
            }
        }

        private void button_todut_Click(object sender, EventArgs e)
        {
            goXYAbs(376.615, 358.3795);
            goAix(4, -90);
            goAix(3, 0);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            goXYAbs(433.592, 674.57);
            goAix(4, 0);
            goAix(3, 0);
        }

        //拍socket
        private void button3_Click(object sender, EventArgs e)
        {
            GrapImage(0);
            InputPara P = new InputPara(this.image, image.FullDomain(), new HShapeModel(Utility.Model + "SocketMark_offe.sbm"), 0.3);
            OutPutResult result = AutoNormal_New.ImageProcess.SocketMark(P, out HObject mark);
            HOperatorSet.GenCrossContourXld(out HObject cross, result.findPoint.Row, result.findPoint.Column, 145, 0);
            this.superWind1.obj = cross;
            //当前角度
            double CurrentAngle = GetAix(4);
            //下相机角度偏差
            double downcam_angle = down_angle - serializableData.mode_angle1;
            //上相机角度偏差
            double upcam_angle = result.Angle - serializableData.UpCam_angle1;
            //当前拍照角度加上偏差值
            cal_R = CurrentAngle + upcam_angle - downcam_angle;

            //物料中心按照旋转中心旋转回去
            HOperatorSet.HomMat2dIdentity(out HTuple homate);
            HOperatorSet.TupleRad(upcam_angle + downcam_angle, out HTuple phi);
            HOperatorSet.HomMat2dRotate(homate, phi, serializableData.Rotate_Center_Row1, serializableData.Rotate_Center_Col1, out HTuple homate_rotate);
            HOperatorSet.AffineTransPoint2d(homate_rotate, down_row, down_col, out HTuple trans_findrow, out HTuple trans_findcolum);

            //像素点转换成实际的PLC点位置
            HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down1, trans_findrow, trans_findcolum, out HTuple transX, out HTuple transY);
            HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down1, serializableData.mode_row1, serializableData.mode_col1, out HTuple transX_mode, out HTuple transY_mode);
            //下相机的偏差
            double offx_down = transX_mode.D - transX.D;
            double offy_down = transY_mode.D - transY.D;

            //上相机偏差
            HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, result.findPoint.Column, result.findPoint.Row, out HTuple transX_up, out HTuple transY_up);
            double x_current = GetAix(1);
            double y_current = GetAix(2);
            double offx = serializableData.XShot1 - serializableData.X1;
            double offy = serializableData.YShot1 - serializableData.Y1;
            cal_X = transX_up.D + x_current - offx + offx_down;
            cal_Y = transY_up.D + y_current - offy + offy_down;
        }

        private void button_upshot2_Click(object sender, EventArgs e)
        {
            GrapImage(1);
            InputPara P = new InputPara(this.image, image.FullDomain(), new HShapeModel(), 0.5);
            OutPutResult result = AutoNormal_New.ImageProcess.SocketDutFront(P);
            if (!result.IsRunOk)
            {
                MessageBox.Show("识别失败");
                return;
            }
            HOperatorSet.GenCrossContourXld(out HObject cross, result.findPoint.Row, result.findPoint.Column, 145, 0);
            this.superWind1.obj = cross;
            double CurrentAngle = GetAix(4);
            cal_R = CurrentAngle - (result.Angle - serializableData.UpCam_angle2);
            HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up2, result.findPoint.Column, result.findPoint.Row, out HTuple transX, out HTuple transY);
            double x_current = GetAix(1);
            double y_current = GetAix(2);
            double offx = serializableData.XShot2 - serializableData.X2;
            double offy = serializableData.YShot2 - serializableData.Y2;
            cal_X = transX.D + x_current - offx;
            cal_Y = transY.D + y_current - offy;
        }

        private void button_PLC_get2_Click(object sender, EventArgs e)
        {
            goXYAbs(cal_X, cal_Y);
            goAix(6, cal_R);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            goXYAbs(182.587, 677.1915);
            goAix(4, 0);
            goAix(3, 0);
        }

        private void button_offeset_Click(object sender, EventArgs e)
        {
            Form_offset form = new Form_offset();
            form.Show();
            form.TopMost = true;
        }

        private void button_getcalibcenter_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要更新上吸嘴1标准位置？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                superWind1.viewController.viewPort.ContextMenuStrip = null;
                ImageProcess_Poc2.get_Calibcenter_nozzle(this.superWind1.viewController.viewPort.HalconWindow.DrawRegion(), this.superWind1.image,
                out HTuple row, out HTuple col, out HTuple angle, out HObject rec);
                this.superWind1.ObjColor = "green";
                this.superWind1.obj = rec;
                this.textBox_mode_angle1.Text = angle.D.ToString("f3");
                this.textBox_mode_row1.Text = row.D.ToString("f3");
                this.textBox_mode_col1.Text = col.D.ToString("f3");
                superWind1.viewController.viewPort.ContextMenuStrip = superWind1.viewController.hv_MenuStrip;
            }
        }

        private void button_getcalibcenter2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要更新上吸嘴2标准位置？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                superWind1.viewController.viewPort.ContextMenuStrip = null;
                ImageProcess_Poc2.get_Calibcenter_nozzle(this.superWind1.viewController.viewPort.HalconWindow.DrawRegion(), this.superWind1.image,
                out HTuple row, out HTuple col, out HTuple angle, out HObject rec);
                this.superWind1.ObjColor = "green";
                this.superWind1.obj = rec;
                this.textBox_mode_angle2.Text = angle.D.ToString("f3");
                this.textBox_mode_row2.Text = row.D.ToString("f3");
                this.textBox_mode_col2.Text = col.D.ToString("f3");
                superWind1.viewController.viewPort.ContextMenuStrip = superWind1.viewController.hv_MenuStrip;
            }
        }
    }
}