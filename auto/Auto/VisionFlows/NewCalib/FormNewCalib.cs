using AlcUtility;
using AlcUtility.PlcDriver.CommonCtrl;
using HalconDotNet;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
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
using VisionSDK;

namespace VisionFlows
{
    public partial class FormNewCalib : Form
    {
        private HImage image;
        //private int WorkIndex;
        private EnumCamera CurrentCamera;
        /// <summary>轴运动速度</summary>
        private int axisVel = 10;
        /// <summary>九点标定X路径</summary>
        private double[] offsetX = new double[] { 0, 1, 1, 0, -1, -1, -1, 0, 1 };
        /// <summary>九点标定Y路径</summary>
        private double[] offsetY = new double[] { 0, 0, 1, 1, 1, 0, -1, -1, -1 };
        private static AdsDriverClient HandlerClient = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Handler.ToString()) as AdsDriverClient;
        public FormNewCalib()
        {
            InitializeComponent();
        }
        private void FormNewCalib_Load(object sender, EventArgs e)
        {
            try
            {
                //
                var info = Enum.GetValues(typeof(EnumCamera));
                cmbCameraSelect.Items.AddRange(new object[] { info.GetValue(0), info.GetValue(1), info.GetValue(2) });
                //
                image = new HImage("byte", 512, 512);
                this.superWind1.image = image;
                this.superWind1.viewController.viewPort.HMouseDown += hWindowControl1_HMouseDown;
                superWind1.roiController.addRec1(200, 200, "");
                //
                cmbCameraSelect.SelectedIndex = 0;
                UpdateUI();
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 关闭窗口并保存数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormNewCalib_FormClosing(object sender, FormClosingEventArgs e)
        {
            IsLiveRun = false;
            Thread.Sleep(100);
            if (MessageBox.Show("是否保存标定文件到本地？", "tips", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Flow.XmlHelper.SerializeToXml(Utility.CalibFile + "Calib.xml", AutoNormal_New.serializableData);
        }
        private Task TkCameraLive;
        private bool IsLiveRun = true;
        private void FunctionTkCameraLive()
        {
            while (IsLiveRun)
            {
                GC.Collect();
                image = CameraManager.CameraById(CurrentCamera.ToString()).GrabImage(Utility.CaptureDelayTime);
                if (image != null && image.IsInitialized())
                {
                    this.superWind1.image = image;
                    if (CurrentCamera == EnumCamera.LeftTop || CurrentCamera == EnumCamera.RightTop)
                    {
                        ImageHelper.UpCamFindMarkCenter(image, this.superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(),
                            out HTuple row, out HTuple col);
                    }
                    else
                    {
                        ImageHelper.DownCamFindMarkCenter(image, this.superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(),
                            out HTuple row, out HTuple col);
                        ImageHelper.DownCamFindNozzlCenter(image, this.superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(),
                           out row, out col, out HTuple angle);
                    }
                }
            }
        }

        private void btnContinuousShot_Click(object sender, EventArgs e)
        {
            if (btnContinuousShot.Text == "连续拍照")
            {
                CameraManager.CameraById(CurrentCamera.ToString()).ShuterCur = (long)(Convert.ToDouble(numericUpDown_expore.Value));
                //Plc.SetIO((int)CurrentCamera + 1, true);
                btnContinuousShot.Text = "停止";
                btnContinuousShot.BackColor = Color.Red;
                IsLiveRun = true;
                TkCameraLive = new Task(FunctionTkCameraLive);
                TkCameraLive.Start();
            }
            else
            {
                btnContinuousShot.Text = "连续拍照";
                btnContinuousShot.BackColor = Color.WhiteSmoke;
                IsLiveRun = false;
                System.Threading.Thread.Sleep(500);
                //Plc.SetIO((int)CurrentCamera + 1, false);
            }
        }
        private void numericUpDown_expore_ValueChanged(object sender, EventArgs e)
        {
            CameraManager.CameraById(CurrentCamera.ToString()).ShuterCur = (long)(Convert.ToDouble(numericUpDown_expore.Value));
        }
        private void ResetAllNozzle()
        {
            try
            {
                Plc.PlcDriver.GetCylinderCtrl(9).MoveToBase();//左气缸
                Plc.PlcDriver.GetCylinderCtrl(10).MoveToBase();//右气缸
                Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Z1).AbsGo(0, axisVel, true);
                Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Z2).AbsGo(0, axisVel, true);
            }
            catch
            {
            }
        }
        private double GetAixActPos(int aix)
        {
            //x=1 y=2
            SingleAxisCtrl axis = Plc.PlcDriver?.GetSingleAxisCtrl(aix);
            return axis.Info.ActPos;
        }
        private void btnXSub_MouseDown(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.X, axisVel * -1, true);
        }
        private void btnXSub_MouseUp(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.X, axisVel * -1, false);
        }
        private void btnXPlus_MouseDown(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.X, axisVel, true);
        }
        private void btnXPlus_MouseUp(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.X, axisVel, false);
        }
        private void btnYPlus_MouseDown(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.Y, axisVel, true);
        }

        private void btnYPlus_MouseUp(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.Y, axisVel, false);
        }
        private void btnYSub_MouseDown(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.Y, axisVel * -1, true);
        }
        private void btnYSub_MouseUp(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.Y, axisVel * -1, false);
        }
        private void btnZ1Up_MouseDown(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.Z1, axisVel * -1, true);
        }
        private void btnZ1Up_MouseUp(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.Z1, axisVel * -1, false);
        }
        private void btnZ1Down_MouseDown(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.Z1, axisVel, true);
        }
        private void btnZ1Down_MouseUp(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.Z1, axisVel, false);
        }
        private void btnZ2Up_MouseDown(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.Z2, axisVel * -1, true);
        }
        private void btnZ2Up_MouseUp(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.Z2, axisVel * -1, false);
        }
        private void btnZ2Down_MouseDown(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.Z2, axisVel, true);
        }
        private void btnZ2Down_MouseUp(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.Z2, axisVel, false);
        }
        private void btnR1Plus_MouseDown(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.R1, axisVel, true);
        }
        private void btnR1Plus_MouseUp(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.R1, axisVel, false);
        }
        private void btnR1Sub_MouseDown(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.R1, axisVel * -1, true);
        }
        private void btnR1Sub_MouseUp(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.R1, axisVel * -1, false);
        }
        private void btnR2Plus_MouseDown(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.R2, axisVel, true);
        }
        private void btnR2Plus_MouseUp(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.R2, axisVel, false);
        }
        private void btnR2Sub_MouseDown(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.R2, axisVel * -1, true);
        }
        private void btnR2Sub_MouseUp(object sender, MouseEventArgs e)
        {
            Plc.AxisJogGo((int)EnumAxis.R2, axisVel * -1, false);
        }
        private void btnSop_Click(object sender, EventArgs e)
        {
            try
            {
                Plc.AxisStop((int)EnumAxis.X);
                Plc.AxisStop((int)EnumAxis.Y);
                Plc.AxisStop((int)EnumAxis.Z1);
                Plc.AxisStop((int)EnumAxis.R1);
                Plc.AxisStop((int)EnumAxis.Z2);
                Plc.AxisStop((int)EnumAxis.R2);
            }
            catch
            {
            }
        }
        public void goXYAbs(double X, double Y, double vel = 40)
        {
            SingleAxisCtrl axis_x = Plc.PlcDriver?.GetSingleAxisCtrl(1);
            SingleAxisCtrl axis_y = Plc.PlcDriver?.GetSingleAxisCtrl(2);

            axis_x.AbsGo(X, vel, false);
            axis_y.AbsGo(Y, vel, false);
        }
        /// <summary>
        /// 下相机左吸嘴开始标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeftNozzleCalib_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => { DownCameraLeftNozzleCalib(); });
        }
        private void btnRightNozzleCalib_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() => { DownCameraRightNozzleCalib(); });
        }
        /// <summary>
        /// 下相机左吸嘴标定
        /// </summary>
        private void DownCameraLeftNozzleCalib()
        {
            Flow.Log("DownCameraLeftNozzleCalib标定开始!");
            //初始化标定相关轴
            var axisX = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.X);
            var axisY = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Y);
            var axisZ1 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Z1);
            var axisR1 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.R1);
            //所有吸嘴到初始位
            ResetAllNozzle();
            //获取标定吸嘴初始位置和步长
            var iniPosX = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadX_RecipePos").KeyValues["LoadUpTakePhotosPosX"].Value);
            var iniPosY = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadY_RecipePos").KeyValues["SecondUpTakePhotosPosY"].Value);
            //var iniPosZ1 = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadZ1_RecipePos").KeyValues["SecondUpTakePhotosPosZ1"].Value);
            var iniPosZ1 = 15.53;
            var iniPosR1 = 0.0;
            var stepX = double.Parse(txtLeftNozzleStepX.Text.ToString());
            var stepY = double.Parse(txtLeftNozzleStepY.Text.ToString());
            var stepZ1 = double.Parse(txtLeftNozzleStepZ1.Text.ToString());
            var stepR1 = double.Parse(txtLeftNozzleStepR1.Text.ToString());

            //标定吸嘴到初始位
            axisX.AbsGo(iniPosX, axisVel, true);
            axisY.AbsGo(iniPosY, axisVel, true);
            Plc.PlcDriver.GetCylinderCtrl(9).MoveToWork();
            Thread.Sleep(1000);
            axisZ1.AbsGo(iniPosZ1, axisVel, true);
            axisR1.AbsGo(iniPosR1, axisVel, true);
            Thread.Sleep(1000);
            Flow.Log("LeftNozzle已到标定初始位置");
            //删除上次标定数据
            AutoNormal_New.serializableData.StaticCameraCalib1.Clear();
            dgvBotLeftNozzleCalib.DataSource = AutoNormal_New.serializableData.MoveCameraCalib1;
            dgvBotLeftNozzleCalib.Refresh();
            //dgvBotLeftNozzleCalib.Rows.Clear();
            StaticCameraCalib calibdata = new StaticCameraCalib();
            //九点标定
            for (int i = 0; i < 9; i++)
            {
                Flow.Log("LeftNozzle第" + (i + 1) + "次标定拍照!");
                axisX.AbsGo(iniPosX + stepX * offsetX[i], axisVel, true);//移动X
                axisY.AbsGo(iniPosY + stepY * offsetY[i], axisVel, true);//移动Y
                Plc.SetIO((int)EnumLight.Bottom, true);//打开光源
                Thread.Sleep(1000);
                //拍照
                image = CameraManager.CameraById(EnumCamera.Bottom.ToString()).GrabImage(Utility.CaptureDelayTime);
                this.superWind1.image = image;
                if (image == null)
                {
                    Flow.Log("LeftNozzle第" + (i + 1) + "次标定拍照失败!");
                    return;
                }
                Plc.SetIO((int)EnumLight.Bottom, false);//关闭光源
                //
                if (!ImageHelper.DownCamFindNozzlCenter(superWind1.image, superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(),
                    out HTuple find_row, out HTuple find_column, out HTuple angle))
                {
                    Flow.Log("LeftNozzle第" + (i + 1) + "次图像处理失败!");
                    return;
                }

                //获取当前XY位置
                GetAxisXY(out double X, out double Y);
                AutoNormal_New.serializableData.StaticCameraCalib1.Add(new MoveCameraCalib(find_row, find_column, X, Y, 0, 0));
                dgvBotLeftNozzleCalib.DataSource = AutoNormal_New.serializableData.StaticCameraCalib1;
                dgvBotLeftNozzleCalib.Refresh();

                HOperatorSet.TupleConcat(calibdata.ALL_row1, find_row, out calibdata.ALL_row1);
                HOperatorSet.TupleConcat(calibdata.ALL_colum1, find_column, out calibdata.ALL_colum1);
                HOperatorSet.TupleConcat(calibdata.ALL_X1, GetAixActPos((int)EnumAxis.X), out calibdata.ALL_X1);
                HOperatorSet.TupleConcat(calibdata.ALL_Y1, GetAixActPos((int)EnumAxis.Y), out calibdata.ALL_Y1);
                Thread.Sleep(500);
            }

            //旋转中心标定
            axisX.AbsGo(iniPosX, axisVel, true);//移动X到初始位
            axisY.AbsGo(iniPosY, axisVel, true);//移动Y到初始位
            //Plc.PlcDriver.GetCylinderCtrl(9).MoveToWork(true);
            //axisZ1.AbsGo(iniPosZ1, axisVel, true);
            //axisR1.AbsGo(iniPosR1, axisVel, true);
            for (int i = -5; i < 5; i++)
            {
                Flow.Log("LeftNozzle第" + (i + 11) + "次标定拍照!");
                axisR1.AbsGo(iniPosR1 + stepR1 * i, axisVel, true);//移动R1
                Plc.SetIO((int)EnumLight.Bottom, true);//打开光源
                Thread.Sleep(1000);
                //拍照
                image = CameraManager.CameraById(EnumCamera.Bottom.ToString()).GrabImage(Utility.CaptureDelayTime);
                this.superWind1.image = image;
                if (image == null)
                {
                    Flow.Log("LeftNozzle第" + (i + 11) + "次标定拍照失败!");
                    return;
                }
                Plc.SetIO((int)EnumLight.Bottom, false);//关闭光源
                //
                if (!ImageHelper.DownCamFindNozzlCenter(superWind1.image, superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(),
                        out HTuple find_row, out HTuple find_column, out HTuple angle))
                {
                    Flow.Log("LeftNozzle第" + (i + 11) + "次图像处理失败!");
                    return;
                }
                //获取吸嘴初始姿态数据
                if (i == 0)
                {
                    GetAxisXY(out double X3, out double Y3);
                    AutoNormal_New.serializableData.mode_angle1 = angle.D;
                    AutoNormal_New.serializableData.mode_row1 = find_row.D;
                    AutoNormal_New.serializableData.mode_col1 = find_column.D;
                    Flow.Log("LeftNozzle初始姿态获取完成!");
                }

                HOperatorSet.TupleConcat(calibdata.rotate_row, find_row, out calibdata.rotate_row);
                HOperatorSet.TupleConcat(calibdata.rotate_colum, find_column, out calibdata.rotate_colum);
            }

            //所有吸嘴到初始位
            ResetAllNozzle();
            //计算标定结果
            try
            {
                //计算九点标定
                HOperatorSet.VectorToHomMat2d(calibdata.ALL_row1,calibdata.ALL_colum1,  calibdata.ALL_X1, calibdata.ALL_Y1, out AutoNormal_New.serializableData.HomMat2D_down1);
                HOperatorSet.WriteTuple(AutoNormal_New.serializableData.HomMat2D_down1, Utility.CalibFile + "calib_down1");
                Flow.Log("LeftNozzle九点标定结果计算完成!");
                //计算吸嘴旋转中心
                HObject Contour;
                HTuple hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder;
                HOperatorSet.GenContourPolygonXld(out Contour, calibdata.rotate_row, calibdata.rotate_colum);
                HOperatorSet.FitCircleContourXld(Contour, "geotukey", -1, 0, 0, 3, 2,
                    out HTuple hv_RowCenter1, out HTuple hv_ColCenter1, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                AutoNormal_New.serializableData.Rotate_Center_Row1 = hv_RowCenter1.D;
                AutoNormal_New.serializableData.Rotate_Center_Col1 = hv_ColCenter1.D;
                Flow.Log("LeftNozzle吸嘴旋转中心标定结果计算完成!");
                //显示旋转中心
                this.superWind1.ObjColor = "green";
                HOperatorSet.GenCrossContourXld(out HObject cross, hv_RowCenter1, hv_ColCenter1, 156, 0);
                this.superWind1.obj = cross;
                //显示旋转中心拟合点
                this.superWind1.ObjColor = "red";
                HOperatorSet.GenCrossContourXld(out cross, calibdata.rotate_row, calibdata.rotate_colum, 156, 0);
                this.superWind1.obj = cross;
                this.UpdateBotLeftNozzleCalibUI();
            }
            catch (Exception ex)
            {
                Flow.Log("LeftNozzle吸嘴旋标定结果计算异常!");
            }
        }
        /// <summary>
        /// 下相机右吸嘴标定
        /// </summary>
        private void DownCameraRightNozzleCalib()
        {
            Flow.Log("DownCameraRightNozzleCalib标定开始!");
            //初始化标定相关轴
            var axisX = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.X);
            var axisY = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Y);
            var axisZ2 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Z2);
            var axisR2 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.R2);
            //所有吸嘴到初始位
            ResetAllNozzle();
            //获取标定吸嘴初始位置和步长
            var iniPosX = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("UloadX_RecipePos").KeyValues["SecondUloadUpTakePhotosPosX"].Value);
            var iniPosY = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("UloadY_RecipePos").KeyValues["SecondUloadUpTakePhotosPosY"].Value);
            //var iniPosZ2 = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("UloadZ2_RecipePos").KeyValues["SecondUpTakePhotosPosZ2"].Value);
            var iniPosZ2 = 15.917;
            var iniPosR2 = 0.0;
            var stepX = double.Parse(txtRightNozzleStepX.Text.ToString());
            var stepY = double.Parse(txtRightNozzleStepY.Text.ToString());
            var stepZ2 = double.Parse(txtRightNozzleStepZ2.Text.ToString());
            var stepR2 = double.Parse(txtRightNozzleStepR2.Text.ToString());
            //标定吸嘴到初始位
            axisX.AbsGo(iniPosX, axisVel, true);
            axisY.AbsGo(iniPosY, axisVel, true);
            Plc.PlcDriver.GetCylinderCtrl(10).MoveToWork();
            Thread.Sleep(1000);
            axisZ2.AbsGo(iniPosZ2, axisVel, true);
            axisR2.AbsGo(iniPosR2, axisVel, true);
            Thread.Sleep(1000);
            Flow.Log("RightNozzle已到标定初始位置");
            //删除上次标定数据
            AutoNormal_New.serializableData.StaticCameraCalib2.Clear();
            dgvBotRightNozzleCalib.DataSource = AutoNormal_New.serializableData.MoveCameraCalib1;
            dgvBotRightNozzleCalib.Refresh();
            //dgvBotRightNozzleCalib.Rows.Clear();
            StaticCameraCalib calibdata = new StaticCameraCalib();
            //九点标定
            for (int i = 0; i < 9; i++)
            {
                Flow.Log("RightNozzle第" + (i + 1) + "次标定拍照!");
                axisX.AbsGo(iniPosX + stepX * offsetX[i], axisVel, true);//移动X
                axisY.AbsGo(iniPosY + stepY * offsetY[i], axisVel, true);//移动Y
                Plc.SetIO((int)EnumLight.Bottom, true);//打开光源
                Thread.Sleep(1000);
                //拍照
                image = CameraManager.CameraById(EnumCamera.Bottom.ToString()).GrabImage(Utility.CaptureDelayTime);
                this.superWind1.image = image;
                if (image == null)
                {
                    Flow.Log("RightNozzle第" + (i + 1) + "次标定拍照失败!");
                    return;
                }
                Plc.SetIO((int)EnumLight.Bottom, false);//关闭光源
                //
                if (!ImageHelper.DownCamFindNozzlCenter(superWind1.image, superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(),
                    out HTuple find_row, out HTuple find_column, out HTuple angle))
                {
                    Flow.Log("RightNozzle第" + (i + 1) + "次图像处理失败!");
                    return;
                }
                //获取当前XY位置
                //string[] temp = new string[] { find_row.D.ToString("F3"), find_column.D.ToString("F3"), GetAixActPos(1).ToString("F3"), GetAixActPos(2).ToString("F3") };
                //this.dgvBotRightNozzleCalib.Rows.Add(temp);
                GetAxisXY(out double X, out double Y);
                AutoNormal_New.serializableData.StaticCameraCalib2.Add(new MoveCameraCalib(find_row, find_column, X, Y, 0, 0));
                dgvBotRightNozzleCalib.DataSource = AutoNormal_New.serializableData.StaticCameraCalib1;
                dgvBotRightNozzleCalib.Refresh();

                HOperatorSet.TupleConcat(calibdata.ALL_row1, find_row, out calibdata.ALL_row1);
                HOperatorSet.TupleConcat(calibdata.ALL_colum1, find_column, out calibdata.ALL_colum1);
                HOperatorSet.TupleConcat(calibdata.ALL_X1, GetAixActPos((int)EnumAxis.X), out calibdata.ALL_X1);
                HOperatorSet.TupleConcat(calibdata.ALL_Y1, GetAixActPos((int)EnumAxis.Y), out calibdata.ALL_Y1);
                Thread.Sleep(500);
            }

            //旋转中心标定
            axisX.AbsGo(iniPosX, axisVel, true);//移动X到初始位
            axisY.AbsGo(iniPosY, axisVel, true);//移动Y到初始位
            //Plc.PlcDriver.GetCylinderCtrl(9).MoveToWork(true);
            //axisZ1.AbsGo(iniPosZ1, axisVel, true);
            //axisR1.AbsGo(iniPosR1, axisVel, true);
            for (int i = -2; i < 3; i++)
            {
                Flow.Log("RightNozzle第" + (i + 11) + "次标定拍照!");
                axisR2.AbsGo(iniPosR2 + stepR2 * i, axisVel, true);//移动R2
                Plc.SetIO((int)EnumLight.Bottom, true);//打开光源
                Thread.Sleep(1000);
                //拍照
                image = CameraManager.CameraById(EnumCamera.Bottom.ToString()).GrabImage(Utility.CaptureDelayTime);
                this.superWind1.image = image;
                if (image == null)
                {
                    Flow.Log("LeftNozzle第" + (i + 11) + "次标定拍照失败!");
                    return;
                }
                Plc.SetIO((int)EnumLight.Bottom, false);//关闭光源
                //
                if (!ImageHelper.DownCamFindNozzlCenter(superWind1.image, superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(),
                        out HTuple find_row, out HTuple find_column, out HTuple angle))
                {
                    Flow.Log("LeftNozzle第" + (i + 11) + "次图像处理失败!");
                    return;
                }
                //获取吸嘴初始姿态数据
                if (i == 0)
                {
                    AutoNormal_New.serializableData.mode_angle2 = angle.D;
                    AutoNormal_New.serializableData.mode_row2 = find_row.D;
                    AutoNormal_New.serializableData.mode_col2 = find_column.D;
                    Flow.Log("RightNozzle初始姿态获取完成!");
                }

                HOperatorSet.TupleConcat(calibdata.rotate_row, find_row, out calibdata.rotate_row);
                HOperatorSet.TupleConcat(calibdata.rotate_colum, find_column, out calibdata.rotate_colum);
            }
            //所有吸嘴到初始位
            ResetAllNozzle();
            //计算标定结果
            try
            {
                //计算九点标定
                HOperatorSet.VectorToHomMat2d(calibdata.ALL_colum1, calibdata.ALL_row1, calibdata.ALL_X1, calibdata.ALL_Y1, out AutoNormal_New.serializableData.HomMat2D_down2);
                //HOperatorSet.WriteTuple(AutoNormal_New.serializableData.HomMat2D_down2, Utility.CalibFile + "calib_down2");
                Flow.Log("RightNozzle九点标定结果计算完成!");
                //计算吸嘴旋转中心
                HObject Contour;
                HTuple hv_Radius, hv_StartPhi, hv_EndPhi, hv_PointOrder;
                HOperatorSet.GenContourPolygonXld(out Contour, calibdata.rotate_row, calibdata.rotate_colum);
                HOperatorSet.FitCircleContourXld(Contour, "geotukey", -1, 0, 0, 3, 2,
                    out HTuple hv_RowCenter1, out HTuple hv_ColCenter1, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);
                AutoNormal_New.serializableData.Rotate_Center_Row2 = hv_RowCenter1.D;
                AutoNormal_New.serializableData.Rotate_Center_Col2 = hv_ColCenter1.D;
                Flow.Log("RightNozzle吸嘴旋转中心标定结果计算完成!");
                //显示旋转中心
                this.superWind1.ObjColor = "green";
                HOperatorSet.GenCrossContourXld(out HObject cross, hv_RowCenter1, hv_ColCenter1, 156, 0);
                this.superWind1.obj = cross;
                //显示旋转中心拟合点
                this.superWind1.ObjColor = "red";
                HOperatorSet.GenCrossContourXld(out cross, calibdata.rotate_row, calibdata.rotate_colum, 156, 0);
                this.superWind1.obj = cross;
                this.UpdateBotRightNozzleCalibUI();
            }
            catch (Exception ex)
            {
                Flow.Log("RightNozzle吸嘴旋标定结果计算异常!");
            }
        }
        /// <summary>
        /// 抓拍
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSingleShot_Click(object sender, EventArgs e)
        {
            try
            {
                image = CameraManager.CameraById(CurrentCamera.ToString()).GrabImage(Utility.CaptureDelayTime);
                this.image = image;
                this.superWind1.image = this.image.CopyImage();
            }
            catch (Exception ex) { }
        }
        /// <summary>
        /// 拍照并获取拍照位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetLeftTopMarkCenter_Click(object sender, EventArgs e)
        {
            try
            {
                //获取当前XY位置
                GetAxisXY(out double X, out double Y);
                //获取mark位置
                if (!ImageHelper.UpCamFindCalibCenter(superWind1.image, superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(),
                           out HTuple row, out HTuple col))
                {
                    superWind1.Message = "获取标定板Mark点失败!";
                    return;
                }
                if (AutoNormal_New.serializableData.MoveCameraCalib1.Count <= 0)
                {
                    AutoNormal_New.serializableData.MoveCameraCalib1.Add(new MoveCameraCalib(row, col, X, Y, 0, 0));
                }
                else if (AutoNormal_New.serializableData.MoveCameraCalib1[AutoNormal_New.serializableData.MoveCameraCalib1.Count - 1].tool1_x == 0 || AutoNormal_New.serializableData.MoveCameraCalib1[AutoNormal_New.serializableData.MoveCameraCalib1.Count - 1].row == 0)
                {
                    AutoNormal_New.serializableData.MoveCameraCalib1[AutoNormal_New.serializableData.MoveCameraCalib1.Count - 1].row = row;
                    AutoNormal_New.serializableData.MoveCameraCalib1[AutoNormal_New.serializableData.MoveCameraCalib1.Count - 1].colum = col;
                    AutoNormal_New.serializableData.MoveCameraCalib1[AutoNormal_New.serializableData.MoveCameraCalib1.Count - 1].X = X;
                    AutoNormal_New.serializableData.MoveCameraCalib1[AutoNormal_New.serializableData.MoveCameraCalib1.Count - 1].Y = Y;
                }
                else
                {
                    AutoNormal_New.serializableData.MoveCameraCalib1.Add(new MoveCameraCalib(row, col, X, Y, 0, 0));
                }

                dgvLeftTopCalibData.DataSource = AutoNormal_New.serializableData.MoveCameraCalib1;
                dgvLeftTopCalibData.Refresh();
            }
            catch { }
        }
        /// <summary>
        /// 获取对中位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetLeftTopAlignPos_Click(object sender, EventArgs e)
        {
            try
            {
                GetAxisXY(out double tool1_x, out double tool1_y);
                if (AutoNormal_New.serializableData.MoveCameraCalib1.Count <= 0)
                {
                    AutoNormal_New.serializableData.MoveCameraCalib1.Add(new MoveCameraCalib(0, 0, 0, 0, tool1_x, tool1_y));
                }
                else if (AutoNormal_New.serializableData.MoveCameraCalib1[AutoNormal_New.serializableData.MoveCameraCalib1.Count - 1].tool1_x == 0 || AutoNormal_New.serializableData.MoveCameraCalib1[AutoNormal_New.serializableData.MoveCameraCalib1.Count - 1].row == 0)
                {
                    AutoNormal_New.serializableData.MoveCameraCalib1[AutoNormal_New.serializableData.MoveCameraCalib1.Count - 1].tool1_x = tool1_x;
                    AutoNormal_New.serializableData.MoveCameraCalib1[AutoNormal_New.serializableData.MoveCameraCalib1.Count - 1].tool1_y = tool1_y;
                }
                else
                {
                    AutoNormal_New.serializableData.MoveCameraCalib1.Add(new MoveCameraCalib(0, 0, 0, 0, tool1_x, tool1_y));
                }

                //dgvLeftTopCalibData.DataSource = AutoNormal_New.serializableData.MoveCameraCalib1;
                //dgvLeftTopCalibData.Refresh();
            }
            catch { }
        }
        /// <summary>
        /// 上相机2标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRightTopCalib_Click(object sender, EventArgs e)
        {
            HTuple ALL_row = new HTuple();
            HTuple ALL_col = new HTuple();
            HTuple ALL_X = new HTuple();
            HTuple ALL_Y = new HTuple();
            HTuple ALL_tool1_X = new HTuple();
            HTuple ALL_tool1_Y = new HTuple();
            for (int i = 0; i < AutoNormal_New.serializableData.MoveCameraCalib2.Count; i++)
            {
                HTuple temp = AutoNormal_New.serializableData.MoveCameraCalib2[i].row;
                HOperatorSet.TupleConcat(ALL_row, temp, out ALL_row);

                temp = AutoNormal_New.serializableData.MoveCameraCalib2[i].colum;
                HOperatorSet.TupleConcat(ALL_col, temp, out ALL_col);

                temp = AutoNormal_New.serializableData.MoveCameraCalib2[i].tool1_x;
                HOperatorSet.TupleConcat(ALL_tool1_X, temp, out ALL_tool1_X);

                temp = AutoNormal_New.serializableData.MoveCameraCalib2[i].tool1_y;
                HOperatorSet.TupleConcat(ALL_tool1_Y, temp, out ALL_tool1_Y);

                temp = AutoNormal_New.serializableData.MoveCameraCalib2[i].tool1_x - AutoNormal_New.serializableData.MoveCameraCalib2[i].X;
                HOperatorSet.TupleConcat(ALL_X, temp, out ALL_X);

                temp = AutoNormal_New.serializableData.MoveCameraCalib2[i].tool1_y - AutoNormal_New.serializableData.MoveCameraCalib2[i].Y;
                HOperatorSet.TupleConcat(ALL_Y, temp, out ALL_Y);
            }
            HOperatorSet.VectorToHomMat2d(ALL_row,ALL_col,  ALL_X, ALL_Y, out AutoNormal_New.serializableData.HomMat2D_up2);
            //HOperatorSet.WriteTuple(AutoNormal_New.serializableData.HomMat2D_up2, Utility.CalibFile + "calib_up2");
            superWind1.Message = ("RightTop相机标定成功!");
            UpdateRightTopCalibUI();
        }
        /// <summary>
        /// 相机1标定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeftTopCalib_Click(object sender, EventArgs e)
        {
            try
            {
                HTuple ALL_row = new HTuple();
                HTuple ALL_col = new HTuple();
                HTuple ALL_X = new HTuple();
                HTuple ALL_Y = new HTuple();
                HTuple ALL_tool1_X = new HTuple();
                HTuple ALL_tool1_Y = new HTuple();
                for (int i = 0; i < AutoNormal_New.serializableData.MoveCameraCalib1.Count; i++)
                {
                    HTuple temp = AutoNormal_New.serializableData.MoveCameraCalib1[i].row;
                    HOperatorSet.TupleConcat(ALL_row, temp, out ALL_row);

                    temp = AutoNormal_New.serializableData.MoveCameraCalib1[i].colum;
                    HOperatorSet.TupleConcat(ALL_col, temp, out ALL_col);

                    temp = AutoNormal_New.serializableData.MoveCameraCalib1[i].tool1_x;
                    HOperatorSet.TupleConcat(ALL_tool1_X, temp, out ALL_tool1_X);

                    temp = AutoNormal_New.serializableData.MoveCameraCalib1[i].tool1_y;
                    HOperatorSet.TupleConcat(ALL_tool1_Y, temp, out ALL_tool1_Y);

                    temp = AutoNormal_New.serializableData.MoveCameraCalib1[i].tool1_x - AutoNormal_New.serializableData.MoveCameraCalib1[i].X;
                    HOperatorSet.TupleConcat(ALL_X, temp, out ALL_X);

                    temp = AutoNormal_New.serializableData.MoveCameraCalib1[i].tool1_y - AutoNormal_New.serializableData.MoveCameraCalib1[i].Y;
                    HOperatorSet.TupleConcat(ALL_Y, temp, out ALL_Y);
                }
                HOperatorSet.VectorToHomMat2d(ALL_row,ALL_col,  ALL_X, ALL_Y, out AutoNormal_New.serializableData.HomMat2D_up1);
                //HOperatorSet.WriteTuple(AutoNormal_New.serializableData.HomMat2D_up1, Utility.CalibFile + "calib_up1");
                UpdateLeftTopCalibUI();
                superWind1.Message = "LeftTop相机标定成功！";
            }
            catch
            {
            }
        }
        /// <summary>
        /// 更新标定UI
        /// </summary>
        private void UpdateUI()
        {
            //
            UpdateLeftTopCalibUI();
            UpdateRightTopCalibUI();
            UpdateBotLeftNozzleCalibUI();
            UpdateBotRightNozzleCalibUI();
            UpdateLeftNozzleOffsetCalibUI();
            UpdateRightNozzleOffsetCalibUI();
        }
        /// <summary>
        /// 更新LeftTop映射变换UI
        /// </summary>
        private void UpdateLeftTopCalibUI()
        {
            try
            {
                dgvLeftTopCalibData.DataSource = AutoNormal_New.serializableData.MoveCameraCalib1;
                dgvLeftTopCalibData.Refresh();
                //
                HOperatorSet.HomMat2dToAffinePar(AutoNormal_New.serializableData.HomMat2D_up1,
                    out HTuple sx, out HTuple sy, out HTuple phi, out HTuple theta, out HTuple tx, out HTuple ty);
                //
                txtLeftTopSx.Text = ((double)sx).ToString("F5");
                txtLeftTopSy.Text = ((double)sy).ToString("F5");
                txtLeftTopPhi.Text = ((double)phi.TupleDeg()).ToString("F3");
                txtLeftTopTheta.Text = ((double)theta.TupleDeg()).ToString("F3");
                txtLeftTopTx.Text = ((double)tx).ToString("F3");
                txtLeftTopTy.Text = ((double)ty).ToString("F3");
                
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 保存标定数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveLeftTopCalibData_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.WriteTuple(AutoNormal_New.serializableData.HomMat2D_up1, Utility.CalibFile + "calib_up1");
                Flow.XmlHelper.SerializeToXml(Utility.CalibFile + "Calib.xml", AutoNormal_New.serializableData);
                Flow.Log("SaveLeftTopCalibData成功!");
            }
            catch
            {
                Flow.Log("SaveLeftTopCalibData失败!");
            }
        }
        /// <summary>
        /// 删除标定数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearLeftTopCalibData_Click(object sender, EventArgs e)
        {
            try
            {
                AutoNormal_New.serializableData.MoveCameraCalib1.Clear();
                dgvLeftTopCalibData.DataSource = AutoNormal_New.serializableData.MoveCameraCalib1;
                dgvLeftTopCalibData.Refresh();
                Flow.Log("ClearLeftTopCalibData成功!");
            }
            catch
            {
                Flow.Log("ClearLeftTopCalibData失败!");
            }
        }
        /// <summary>
        /// 更新RightTop映射变换UI
        /// </summary>
        private void UpdateRightTopCalibUI()
        {
            try
            {
                //
                dgvRightTopCalibData.DataSource = AutoNormal_New.serializableData.MoveCameraCalib2;
                dgvRightTopCalibData.Refresh();

                HOperatorSet.HomMat2dToAffinePar(AutoNormal_New.serializableData.HomMat2D_up2,
                    out HTuple sx, out HTuple sy, out HTuple phi, out HTuple theta, out HTuple tx, out HTuple ty);
                txtRightTopSx.Text = ((double)sx).ToString("F5");
                txtRightTopSy.Text = ((double)sy).ToString("F5");
                txtRightTopPhi.Text = ((double)phi.TupleDeg()).ToString("F3");
                txtRightTopTheta.Text = ((double)theta.TupleDeg()).ToString("F3");
                txtRightTopTx.Text = ((double)tx).ToString("F3");
                txtRightTopTy.Text = ((double)ty).ToString("F3");
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 保存标定数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveRightTopCalibData_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.WriteTuple(AutoNormal_New.serializableData.HomMat2D_up2, Utility.CalibFile + "calib_up2");
                Flow.XmlHelper.SerializeToXml(Utility.CalibFile + "Calib.xml", AutoNormal_New.serializableData);
                Flow.Log("SaveRightTopCalibData成功!");
            }
            catch
            {
                Flow.Log("SaveRightTopCalibData失败!");
            }

        }
        /// <summary>
        /// 删除标定数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearRightTopCalibData_Click(object sender, EventArgs e)
        {
            try
            {
                AutoNormal_New.serializableData.MoveCameraCalib2.Clear();
                dgvRightTopCalibData.DataSource = AutoNormal_New.serializableData.MoveCameraCalib2;
                dgvRightTopCalibData.Refresh();
                Flow.Log("ClearRightTopCalibData成功!");
            }
            catch
            {
                Flow.Log("ClearRightTopCalibData失败!");
            }
        }

        /// <summary>
        /// 更新BotLeftNozzle映射变换UI
        /// </summary>
        private void UpdateBotLeftNozzleCalibUI()
        {
            try
            {
                dgvBotLeftNozzleCalib.DataSource = AutoNormal_New.serializableData.StaticCameraCalib1;
                dgvBotLeftNozzleCalib.Refresh();
                //
                HOperatorSet.HomMat2dToAffinePar(AutoNormal_New.serializableData.HomMat2D_down1,
                    out HTuple sx, out HTuple sy, out HTuple phi, out HTuple theta, out HTuple tx, out HTuple ty);
                //
                txtLeftNozzleSx.Text = ((double)sx).ToString("F5");
                txtLeftNozzleSy.Text = ((double)sy).ToString("F5");
                txtLeftNozzlePhi.Text = ((double)phi.TupleDeg()).ToString("F3");
                txtLeftNozzleTheta.Text = ((double)theta.TupleDeg()).ToString("F3");
                txtLeftNozzleTx.Text = ((double)tx).ToString("F3");
                txtLeftNozzleTy.Text = ((double)ty).ToString("F3");
                //
                txtLeftNozzleRotateCenterRow.Text = AutoNormal_New.serializableData.Rotate_Center_Row1.ToString("F3");
                txtLeftNozzleRotateCenterCol.Text = AutoNormal_New.serializableData.Rotate_Center_Col1.ToString("F3");
                txtLeftNozzleRow.Text = AutoNormal_New.serializableData.mode_row1.ToString("F3");
                txtLeftNozzleCol.Text = AutoNormal_New.serializableData.mode_col1.ToString("F3");
                txtLeftNozzleAngle.Text = AutoNormal_New.serializableData.mode_angle1.ToString("F3");
                //
                txtLeftNozzleIniX.Text = Convert.ToString(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadX_RecipePos").KeyValues["LoadUpTakePhotosPosX"].Value);
                txtLeftNozzleIniY.Text = Convert.ToString(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadY_RecipePos").KeyValues["SecondUpTakePhotosPosY"].Value);
                txtLeftNozzleIniZ1.Text = Convert.ToString(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadZ1_RecipePos").KeyValues["SecondUpTakePhotosPosZ1"].Value);
                txtLeftNozzleIniR1.Text = "0.0";
                //
                txtLeftNozzleStepX.Text = "0.6";
                txtLeftNozzleStepY.Text = "0.6";
                txtLeftNozzleStepZ1.Text = "0";
                txtLeftNozzleStepR1.Text = "15";
            }
            catch (Exception ex)
            {

            }
        }
        private void btnSaveLeftNozzleCalibData_Click(object sender, EventArgs e)
        {
            HOperatorSet.WriteTuple(AutoNormal_New.serializableData.HomMat2D_down1, Utility.CalibFile + "calib_down1");
            Flow.XmlHelper.SerializeToXml(Utility.CalibFile + "Calib.xml", AutoNormal_New.serializableData);
            Flow.Log("SaveLeftNozzleCalibData成功!");
        }
        /// <summary>
        /// 更新BotRightNozzle映射变换UI
        /// </summary>
        private void UpdateBotRightNozzleCalibUI()
        {
            try
            {
                dgvBotRightNozzleCalib.DataSource = AutoNormal_New.serializableData.StaticCameraCalib2;
                dgvBotRightNozzleCalib.Refresh();
                //
                HOperatorSet.HomMat2dToAffinePar(AutoNormal_New.serializableData.HomMat2D_down2,
                    out HTuple sx, out HTuple sy, out HTuple phi, out HTuple theta, out HTuple tx, out HTuple ty);
                //
                txtRightNozzleSx.Text = ((double)sx).ToString("F5");
                txtRightNozzleSy.Text = ((double)sy).ToString("F5");
                txtRightNozzlePhi.Text = ((double)phi.TupleDeg()).ToString("F3");
                txtRightNozzleTheta.Text = ((double)theta.TupleDeg()).ToString("F3");
                txtRightNozzleTx.Text = ((double)tx).ToString("F3");
                txtRightNozzleTy.Text = ((double)ty).ToString("F3");
                //
                txtRightNozzleRotateCenterRow.Text = AutoNormal_New.serializableData.Rotate_Center_Row2.ToString("F3");
                txtRightNozzleRotateCenterCol.Text = AutoNormal_New.serializableData.Rotate_Center_Col2.ToString("F3");
                txtRightNozzleRow.Text = AutoNormal_New.serializableData.mode_row2.ToString("F3");
                txtRightNozzleCol.Text = AutoNormal_New.serializableData.mode_col2.ToString("F3");
                txtRightNozzleAngle.Text = AutoNormal_New.serializableData.mode_angle2.ToString("F3");
                //
                txtRightNozzleIniX.Text = Convert.ToString(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("UloadX_RecipePos").KeyValues["SecondUloadUpTakePhotosPosX"].Value);
                txtRightNozzleIniY.Text = Convert.ToString(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("UloadY_RecipePos").KeyValues["SecondUloadUpTakePhotosPosY"].Value);
                txtRightNozzleIniZ2.Text = Convert.ToString(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("UloadZ2_RecipePos").KeyValues["SecondUpTakePhotosPosZ2"].Value);
                txtRightNozzleIniR2.Text = "0.0";

                txtRightNozzleStepX.Text = "0.6";
                txtRightNozzleStepY.Text = "0.6";
                txtRightNozzleStepZ2.Text = "0";
                txtRightNozzleStepR2.Text = "15";
            }
            catch (Exception ex)
            {

            }
        }
        private void btnSaveRightNozzleCalibData_Click(object sender, EventArgs e)
        {
            HOperatorSet.WriteTuple(AutoNormal_New.serializableData.HomMat2D_down2, Utility.CalibFile + "calib_down2");
            Flow.XmlHelper.SerializeToXml(Utility.CalibFile + "Calib.xml", AutoNormal_New.serializableData);
            Flow.Log("SaveRightNozzleCalibData成功!");
        }
        /// <summary>
        /// 更新LeftNozzleOffsetCalib UI
        /// </summary>
        private void UpdateLeftNozzleOffsetCalibUI()
        {
            txtLeftNozzleCalibMarkX.Text = AutoNormal_New.serializableData.DownCam_mark_Row.ToString("F3");
            txtLeftNozzleCalibMarkY.Text = AutoNormal_New.serializableData.DownCam_mark_Col.ToString("F3");
            txtLeftNozzleCameraAlignX.Text = AutoNormal_New.serializableData.XShot1.ToString("F3");
            txtLeftNozzleCameraAlignY.Text = AutoNormal_New.serializableData.YShot1.ToString("F3");
            txtLeftNozzleNozzleAlignX.Text = AutoNormal_New.serializableData.X1.ToString("F3");
            txtLeftNozzleNozzleAlignY.Text = AutoNormal_New.serializableData.Y1.ToString("F3");
            txtLeftNozzleOffsetX.Text = AutoNormal_New.serializableData.xLeftOffset.ToString("F3");
            txtLeftNozzleOffsetY.Text = AutoNormal_New.serializableData.yLeftOffset.ToString("F3");
        }
        /// <summary>
        /// 更新RightNozzleOffsetCalib UI
        /// </summary>
        private void UpdateRightNozzleOffsetCalibUI()
        {
            txtRightNozzleCalibMarkX.Text = AutoNormal_New.serializableData.DownCam_mark_Row.ToString("F3");
            txtRightNozzleCalibMarkY.Text = AutoNormal_New.serializableData.DownCam_mark_Col.ToString("F3");
            txtRightNozzleCameraAlignX.Text = AutoNormal_New.serializableData.XShot2.ToString("F3");
            txtRightNozzleCameraAlignY.Text = AutoNormal_New.serializableData.YShot2.ToString("F3");
            txtRightNozzleNozzleAlignX.Text = AutoNormal_New.serializableData.X2.ToString("F3");
            txtRightNozzleNozzleAlignY.Text = AutoNormal_New.serializableData.Y2.ToString("F3");
            txtRightNozzleOffsetX.Text = AutoNormal_New.serializableData.xRightOffset.ToString("F3");
            txtRightNozzleOffsetY.Text = AutoNormal_New.serializableData.yRightOffset.ToString("F3");
        }
        /// <summary>
        /// 上相机2拍照并获取拍照中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetRightTopMarkCenter_Click(object sender, EventArgs e)
        {
            //获取当前XY位置
            GetAxisXY(out double X, out double Y);
            //获取mark位置
            if (!ImageHelper.UpCamFindCalibCenter(superWind1.image, superWind1,
                           ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(),
                           out HTuple row, out HTuple col))
            {
                superWind1.Message = "识别mark点失败!";
                return;
            }

            if (AutoNormal_New.serializableData.MoveCameraCalib2.Count <= 0)
            {
                AutoNormal_New.serializableData.MoveCameraCalib2.Add(new MoveCameraCalib(row, col, X, Y, 0, 0));
            }
            else if (AutoNormal_New.serializableData.MoveCameraCalib2[AutoNormal_New.serializableData.MoveCameraCalib2.Count - 1].tool1_x == 0
                || AutoNormal_New.serializableData.MoveCameraCalib2[AutoNormal_New.serializableData.MoveCameraCalib2.Count - 1].row == 0)
            {
                AutoNormal_New.serializableData.MoveCameraCalib2[AutoNormal_New.serializableData.MoveCameraCalib2.Count - 1].row = row;
                AutoNormal_New.serializableData.MoveCameraCalib2[AutoNormal_New.serializableData.MoveCameraCalib2.Count - 1].colum = col;
                AutoNormal_New.serializableData.MoveCameraCalib2[AutoNormal_New.serializableData.MoveCameraCalib2.Count - 1].X = X;
                AutoNormal_New.serializableData.MoveCameraCalib2[AutoNormal_New.serializableData.MoveCameraCalib2.Count - 1].Y = Y;
            }
            else
            {
                AutoNormal_New.serializableData.MoveCameraCalib2.Add(new MoveCameraCalib(row, col, X, Y, 0, 0));
            }

            dgvRightTopCalibData.DataSource = AutoNormal_New.serializableData.MoveCameraCalib2;
            dgvRightTopCalibData.Refresh();
        }
        /// <summary>
        /// 相机2获取对中位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetRightTopAlignPos_Click(object sender, EventArgs e)
        {
            GetAxisXY(out double tool1_x, out double tool1_y);
            if (AutoNormal_New.serializableData.MoveCameraCalib2.Count <= 0)
            {
                AutoNormal_New.serializableData.MoveCameraCalib2.Add(new MoveCameraCalib(0, 0, 0, 0, tool1_x, tool1_y));
            }
            else if (AutoNormal_New.serializableData.MoveCameraCalib2[AutoNormal_New.serializableData.MoveCameraCalib2.Count - 1].tool1_x == 0 || AutoNormal_New.serializableData.MoveCameraCalib2[AutoNormal_New.serializableData.MoveCameraCalib2.Count - 1].row == 0)
            {
                AutoNormal_New.serializableData.MoveCameraCalib2[AutoNormal_New.serializableData.MoveCameraCalib2.Count - 1].tool1_x = tool1_x;
                AutoNormal_New.serializableData.MoveCameraCalib2[AutoNormal_New.serializableData.MoveCameraCalib2.Count - 1].tool1_y = tool1_y;
            }
            else
            {
                AutoNormal_New.serializableData.MoveCameraCalib2.Add(new MoveCameraCalib(0, 0, 0, 0, tool1_x, tool1_y));
            }

            dgvRightTopCalibData.DataSource = AutoNormal_New.serializableData.MoveCameraCalib2;
            dgvRightTopCalibData.Refresh();
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
                HOperatorSet.WriteImage(superWind1.image, "bmp", 0, Utility.ImageFile + "ImageMode1.bmp");
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                HOperatorSet.WriteImage(superWind1.image, "bmp", 0, Utility.ImageFile + "ImageMode2.bmp");
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                HOperatorSet.WriteImage(superWind1.image, "bmp", 0, Utility.ImageFile + "ImageMode3.bmp");
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                HOperatorSet.WriteImage(superWind1.image, "bmp", 0, Utility.ImageFile + "ImageMode4.bmp");
            }
        }
        private void hWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {  //左上相机
            if (!ckbx_Touch_Move.Checked)
                return;
            if (this.superWind1.image.IsInitialized() && e.Clicks == 2 && (int)CurrentCamera == 0)
            {
                HOperatorSet.GetImageSize(this.superWind1.image, out HTuple m_with, out HTuple m_hight);
                if (e.Y > m_hight || e.X > m_with || e.Y < 0 || e.X < 0)
                    return;
                double row = e.Y;
                double colum = e.X;
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_up1, row, colum,  out HTuple transX, out HTuple transY);
                GetAxisXY(out double x_current, out double y_current);
                goXYAbs(transX.D + x_current, transY.D + y_current);
            }
            //右上相机
            else if (this.superWind1.image.IsInitialized() && e.Clicks == 2 && (int)CurrentCamera == 1)
            {
                HOperatorSet.GetImageSize(this.superWind1.image, out HTuple m_with, out HTuple m_hight);
                if (e.Y > m_hight || e.X > m_with || e.Y < 0 || e.X < 0)
                    return;
                double row = e.Y;
                double colum = e.X;
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_up2, row,colum,  out HTuple transX, out HTuple transY);
                GetAxisXY(out double x_current, out double y_current);
                goXYAbs(transX.D + x_current, transY.D + y_current);
            }
            //下相机吸嘴1
            else if (this.superWind1.image.IsInitialized() && e.Clicks == 2 && (int)CurrentCamera == 2 && tabControl1.SelectedIndex == 2)
            {
                HOperatorSet.GetImageSize(this.superWind1.image, out HTuple m_with, out HTuple m_hight);
                if (e.Y > m_hight || e.X > m_with || e.Y < 0 || e.X < 0)
                    return;
                double row = e.Y;
                double colum = e.X;
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, (m_hight.D / 2),m_with.D / 2,  out HTuple transX1, out HTuple transY1);
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, row,colum,  out HTuple transX2, out HTuple transY2);
                double goX = transX1 - transX2;
                double goY = transY1 - transY2;
                GetAxisXY(out double x_current, out double y_current);
                goXYAbs(goX + x_current, goY + y_current);
            }
            //下相机吸嘴2
            else if (this.superWind1.image.IsInitialized() && e.Clicks == 2 && (int)CurrentCamera == 2 && tabControl1.SelectedIndex == 3)
            {
                HOperatorSet.GetImageSize(this.superWind1.image, out HTuple m_with, out HTuple m_hight);
                if (e.Y > m_hight || e.X > m_with || e.Y < 0 || e.X < 0)
                    return;
                double row = e.Y;
                double colum = e.X;
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down2, m_with.D / 2, (m_hight.D / 2), out HTuple transX1, out HTuple transY1);
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down2, colum, row, out HTuple transX2, out HTuple transY2);
                double goX = transX1 - transX2;
                double goY = transY1 - transY2;
                GetAxisXY(out double x_current, out double y_current);
                goXYAbs(goX + x_current, goY + y_current);
            }

        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                CurrentCamera = EnumCamera.LeftTop;
                if (File.Exists(Utility.ImageFile + "ImageMode1.bmp"))
                {
                    this.superWind1.image = new HImage(Utility.ImageFile + "ImageMode1.bmp");
                }
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                CurrentCamera = EnumCamera.RightTop;
                if (File.Exists(Utility.ImageFile + "ImageMode2.bmp"))
                {
                    this.superWind1.image = new HImage(Utility.ImageFile + "ImageMode2.bmp");
                }
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                CurrentCamera = EnumCamera.Bottom;
                if (File.Exists(Utility.ImageFile + "ImageMode3.bmp"))
                {
                    this.superWind1.image = new HImage(Utility.ImageFile + "ImageMode3.bmp");
                }
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                CurrentCamera = EnumCamera.Bottom;
                if (File.Exists(Utility.ImageFile + "ImageMode4.bmp"))
                {
                    this.superWind1.image = new HImage(Utility.ImageFile + "ImageMode4.bmp");
                }
            }
        }
        /// <summary>
        ///吸嘴操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeftCylinderUp_Click(object sender, EventArgs e)
        {
            Plc.PlcDriver.GetCylinderCtrl(9).MoveToBase();
        }
        /// <summary>
        ///吸嘴操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRightCylinderUp_Click(object sender, EventArgs e)
        {
            Plc.PlcDriver.GetCylinderCtrl(10).MoveToBase();
        }
        /// <summary>
        ///吸嘴操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLeftCylinderDown_Click(object sender, EventArgs e)
        {
            Plc.PlcDriver.GetCylinderCtrl(9).MoveToWork();
        }
        /// <summary>
        ///吸嘴操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRightCylinderDown_Click(object sender, EventArgs e)
        {
            Plc.PlcDriver.GetCylinderCtrl(10).MoveToWork();
        }
        private void chkLeftTopLight_CheckedChanged(object sender, EventArgs e)
        {
            Plc.SetIO((int)EnumLight.LeftTop, chkLeftTopLight.Checked);
        }
        private void chkRightTopLight_CheckedChanged(object sender, EventArgs e)
        {
            Plc.SetIO((int)EnumLight.RightTop, chkRightTopLight.Checked);
        }
        private void chkBottomLight_CheckedChanged(object sender, EventArgs e)
        {
            Plc.SetIO((int)EnumLight.Bottom, chkBottomLight.Checked);
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            HandlerClient.WriteObject(Poc2Auto.Common.RunModeMgr.Name_VisionCalibration, !chkAxisSafeLimit.Checked);
        }
        private void trackBar_Velocity_Scroll(object sender, EventArgs e)
        {
            axisVel = trkVel.Value;
            lblAxisVel.Text = trkVel.Value.ToString();
        }
        private void cmbCameraSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentCamera = (EnumCamera)((ComboBox)sender).SelectedIndex;
        }
        private void btnLeftVacuumOn_Click(object sender, EventArgs e)
        {
            Plc.PlcDriver.GetCylinderCtrl(6).MoveToWork();
        }
        private void btnLeftVacuumOff_Click(object sender, EventArgs e)
        {
            Plc.PlcDriver.GetCylinderCtrl(6).MoveToNone();
        }
        private void btnRightVacuumOff_Click(object sender, EventArgs e)
        {
            Plc.PlcDriver.GetCylinderCtrl(7).MoveToNone();
        }
        private void btnRightVacuumOn_Click(object sender, EventArgs e)
        {
            Plc.PlcDriver.GetCylinderCtrl(7).MoveToWork();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Plc.PlcDriver == null || !Plc.PlcDriver.IsConnected || !Plc.PlcDriver.IsInitOk)
                    return;

                double x = (double)(Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.X).Info.ActPos);
                double y = (double)(Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Y).Info.ActPos);
                double z1 = (double)(Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Z1).Info.ActPos);
                double z2 = (double)(Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Z2).Info.ActPos);
                double r1 = (double)(Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.R1).Info.ActPos);
                double r2 = (double)(Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.R2).Info.ActPos);

                lblCurrPos.Text = "X=" + x.ToString("F3")
                    + "," + "Y=" + y.ToString("F3")
                    + "," + "Z1=" + z1.ToString("F3")
                    + "," + "Z2=" + z2.ToString("F3")
                    + "," + "R1=" + r1.ToString("F3")
                    + "," + "R2=" + r2.ToString("F3");
                //
                bool bottomLightStatus = Plc.GetIO((int)EnumLight.Bottom);
                bool topLeftLightStatus = Plc.GetIO((int)EnumLight.LeftTop);
                bool topRightLightStatus = Plc.GetIO((int)EnumLight.RightTop);

                bool leftCylinderStatus = Plc.GetCylinder(9).Info.IsWork;
                bool RightCylinderStatus = Plc.GetCylinder(10).Info.IsWork;

                bool leftVacuumStatus = Plc.PlcDriver.GetCylinderCtrl(6).Info.IsWork;
                bool rightVacuumStatus = Plc.PlcDriver.GetCylinderCtrl(7).Info.IsWork;
                #region  光源颜色及状态设置
                if (topLeftLightStatus)
                {
                    chkLeftTopLight.Checked = true;
                    lbl_light_topleft.ForeColor = Color.PaleGreen;
                }
                else
                {
                    chkLeftTopLight.Checked = false;
                    lbl_light_topleft.ForeColor = Color.Red;
                }
                if (topRightLightStatus)
                {
                    chkRightTopLight.Checked = true;
                    lbl_light_topright.ForeColor = Color.PaleGreen;
                }
                else
                {
                    chkRightTopLight.Checked = false;
                    lbl_light_topright.ForeColor = Color.Red;
                }
                if (bottomLightStatus)
                {
                    chkBottomLight.Checked = true;
                    lbl_light_bottom.ForeColor = Color.PaleGreen;
                }
                else
                {
                    chkBottomLight.Checked = false;
                    lbl_light_bottom.ForeColor = Color.Red;
                }
                #endregion

                #region 气缸显示设置
                if (leftCylinderStatus)
                {
                    lbl_cylinder_left.Text = "↓";
                }
                else
                {
                    lbl_cylinder_left.Text = "↑";
                }

                if (RightCylinderStatus)
                {
                    lbl_cylinder_right.Text = "↓";
                }
                else
                {
                    lbl_cylinder_right.Text = "↑";
                }

                #endregion

                #region 真空显示设置
                if (leftVacuumStatus)
                {
                    btnLeftVacuumOn.Enabled = false;
                    btnLeftVacuumOff.Enabled = true;
                    lbl_vacuum_left.Text = "○";
                }
                else
                {
                    btnLeftVacuumOn.Enabled = true;
                    btnLeftVacuumOff.Enabled = false;
                    lbl_vacuum_left.Text = "⊙";
                }

                if (rightVacuumStatus)
                {
                    btnRightVacuumOn.Enabled = false;
                    btnRightVacuumOff.Enabled = true;
                    lbl_vacuum_right.Text = "○";
                }
                else
                {
                    btnRightVacuumOn.Enabled = true;
                    btnRightVacuumOff.Enabled = false;
                    lbl_vacuum_right.Text = "⊙";
                }
                #endregion
            }
            catch
            {
            }
        }
        private void btnGetLeftNozzleCalibMark_Click(object sender, EventArgs e)
        {
            try
            {
                ImageHelper.DownCamFindMarkCenter(this.superWind1.image, this.superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(),
                    out HTuple row, out HTuple col);
                HOperatorSet.WriteImage(this.superWind1.image, "bmp", 0, Utility.CalibImageFile + "方法1_左偏移_下相机_Mark");
                if (row.Length > 0) ;
                {
                    txtLeftNozzleCalibMarkX.Text = row.D.ToString("F3");
                    txtLeftNozzleCalibMarkY.Text = col.D.ToString("F3");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void btnGetLeftNozzleCameraAlign_Click(object sender, EventArgs e)
        {
            try
            {
                txtLeftNozzleCameraAlignX.Text = GetAixActPos((int)EnumAxis.X).ToString("F3");
                txtLeftNozzleCameraAlignY.Text = GetAixActPos((int)EnumAxis.Y).ToString("F3");
                HOperatorSet.WriteImage(this.superWind1.image, "bmp", 0, Utility.CalibImageFile + "方法1_左偏移_上相机_Mark_对中");
            }
            catch { }
        }
        private void btnGetLeftNozzleNozzleAlign_Click(object sender, EventArgs e)
        {
            try
            {
                txtLeftNozzleNozzleAlignX.Text = GetAixActPos((int)EnumAxis.X).ToString("F3");
                txtLeftNozzleNozzleAlignY.Text = GetAixActPos((int)EnumAxis.Y).ToString("F3");
                HOperatorSet.WriteImage(this.superWind1.image, "bmp", 0, Utility.CalibImageFile + "方法1_左偏移_下相机_吸嘴_对中");
            }
            catch { }

        }
        private void btnSaveLeftNozzleOffset_Click(object sender, EventArgs e)
        {
            try
            {
                AutoNormal_New.serializableData.DownCam_mark_Row = Convert.ToDouble(txtLeftNozzleCalibMarkX.Text);
                AutoNormal_New.serializableData.DownCam_mark_Col = Convert.ToDouble(txtLeftNozzleCalibMarkY.Text);

                AutoNormal_New.serializableData.XShot1 = Convert.ToDouble(txtLeftNozzleCameraAlignX.Text);
                AutoNormal_New.serializableData.YShot1 = Convert.ToDouble(txtLeftNozzleCameraAlignY.Text);

                AutoNormal_New.serializableData.X1 = Convert.ToDouble(txtLeftNozzleNozzleAlignX.Text);
                AutoNormal_New.serializableData.Y1 = Convert.ToDouble(txtLeftNozzleNozzleAlignY.Text);

                AutoNormal_New.serializableData.xLeftOffset = Convert.ToDouble(txtLeftNozzleOffsetX.Text);
                AutoNormal_New.serializableData.yLeftOffset = Convert.ToDouble(txtLeftNozzleOffsetY.Text);


                Flow.XmlHelper.SerializeToXml(Utility.CalibFile + "Calib.xml", AutoNormal_New.serializableData);
                Flow.Log("SaveLeftNozzleOffset成功!");
                MessageBox.Show("标定数据保存成功！");
            }
            catch
            {
                Flow.Log("SaveLeftNozzleOffset失败!");
                MessageBox.Show("标定数据保存失败！");
            }
        }
        private void btnGetRightNozzleCalibMark_Click(object sender, EventArgs e)
        {
            try
            {
                ImageHelper.DownCamFindMarkCenter(this.superWind1.image, this.superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(),
                   out HTuple row, out HTuple col);        
                HOperatorSet.WriteImage(this.superWind1.image, "bmp", 0, Utility.CalibImageFile + "方法1_右偏移_下相机_Mark");
                if (row.Length > 0) ;
                {
                    txtRightNozzleCalibMarkX.Text = row.D.ToString("F3");
                    txtRightNozzleCalibMarkY.Text = col.D.ToString("F3");
                }
            }
            catch (Exception)
            {

                
            }
        }
        private void btnGetRightNozzleCameraAlign_Click(object sender, EventArgs e)
        {
            try
            {
                txtRightNozzleCameraAlignX.Text = GetAixActPos((int)EnumAxis.X).ToString("F3");
                txtRightNozzleCameraAlignY.Text = GetAixActPos((int)EnumAxis.Y).ToString("F3");
                HOperatorSet.WriteImage(this.superWind1.image, "bmp", 0, Utility.CalibImageFile + "方法1_右偏移_上相机_Mark_对中");
            }
            catch { }
        }
        private void btnGetRightNozzleNozzleAlign_Click(object sender, EventArgs e)
        {
            try
            {
                txtRightNozzleNozzleAlignX.Text = GetAixActPos((int)EnumAxis.X).ToString("F3");
                txtRightNozzleNozzleAlignY.Text = GetAixActPos((int)EnumAxis.Y).ToString("F3");
                HOperatorSet.WriteImage(this.superWind1.image, "bmp", 0, Utility.CalibImageFile + "方法1_右偏移_下相机_吸嘴_对中");
            }
            catch { }
        }
        private void btnSaveRightNozzleOffset_Click(object sender, EventArgs e)
        {
            try
            {
                AutoNormal_New.serializableData.DownCam_mark_Row = Convert.ToDouble(txtRightNozzleCalibMarkX.Text);
                AutoNormal_New.serializableData.DownCam_mark_Col = Convert.ToDouble(txtRightNozzleCalibMarkY.Text);

                AutoNormal_New.serializableData.XShot2 = Convert.ToDouble(txtRightNozzleCameraAlignX.Text);
                AutoNormal_New.serializableData.YShot2 = Convert.ToDouble(txtRightNozzleCameraAlignY.Text);

                AutoNormal_New.serializableData.X2 = Convert.ToDouble(txtRightNozzleNozzleAlignX.Text);
                AutoNormal_New.serializableData.Y2 = Convert.ToDouble(txtRightNozzleNozzleAlignY.Text);

                AutoNormal_New.serializableData.xRightOffset = Convert.ToDouble(txtRightNozzleOffsetX.Text);
                AutoNormal_New.serializableData.yRightOffset = Convert.ToDouble(txtRightNozzleOffsetY.Text);
                Flow.XmlHelper.SerializeToXml(Utility.CalibFile + "Calib.xml", AutoNormal_New.serializableData);
                Flow.Log("SaveRightNozzleOffset成功!");
                MessageBox.Show("标定数据保存成功！");
            }
            catch
            {
                Flow.Log("SaveRightNozzleOffset失败!");
                MessageBox.Show("标定数据保存失败！");
            }
        }

        /// <summary>
        /// 获取XY当前位置
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        private void GetAxisXY(out double X, out double Y)
        {
            try
            {
                X = (Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.X)).Info.ActPos;
                Y = (Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Y)).Info.ActPos;
            }
            catch (Exception)
            {
                X = 0;
                Y = 0;
            }
        }

        private void btnVerifyCentre_Click(object sender, EventArgs e)
        {
            try
            {
                var axisX = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.X);
                var axisY = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Y);
                var axisZ1 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Z1);
                var axisR1 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.R1);

                var iniPosX = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadX_RecipePos").KeyValues["LoadUpTakePhotosPosX"].Value);
                var iniPosY = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadY_RecipePos").KeyValues["SecondUpTakePhotosPosY"].Value);
                var iniPosZ1 = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadZ1_RecipePos").KeyValues["SecondUpTakePhotosPosZ1"].Value);
                var iniPosR1 = 0.0;
              

                //标定吸嘴到初始位
                axisX.AbsGo(iniPosX, axisVel,true);
                axisY.AbsGo(iniPosY, axisVel,true);
                Plc.PlcDriver.GetCylinderCtrl(9).MoveToWork();
                axisZ1.AbsGo(iniPosZ1, axisVel,true);
                axisR1.AbsGo(iniPosR1, axisVel,true);
                Plc.SetIO((int)EnumLight.Bottom, true);
                Thread.Sleep(200);
                HImage hImage = ImageProcessBase.GrabImage(2, 12000);
                Plc.SetIO((int)EnumLight.Bottom, false);
                if (hImage == null)
                {
                    MessageBox.Show("未采集图像！");
                    return;
                }
                this.superWind1.image = hImage;
                MessageBox.Show("已到标定初始位");
                
            }
            catch (Exception ex)
            {


            }




        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btn_Z1GoZero_Click(object sender, EventArgs e)
        {     
            Plc.AxisAbsGo((int)EnumAxis.Z1, 0.0, axisVel);
        }

        private void btn_Z2GoZero_Click(object sender, EventArgs e)
        {
            Plc.AxisAbsGo((int)EnumAxis.Z2, 0, axisVel);
        }

        private void btnLeftNozzleXGo_Click(object sender, EventArgs e)
        {
            var xPos =Convert.ToDouble( txtLeftNozzleIniX.Text);
            Plc.AxisAbsGo((int)EnumAxis.X, xPos, axisVel);
        }

        private void btnLeftNozzleYGo_Click(object sender, EventArgs e)
        {
            var yPos = Convert.ToDouble(txtLeftNozzleIniY.Text);
            Plc.AxisAbsGo((int)EnumAxis.Y, yPos, axisVel);
        }

        private void btnLeftNozzleZ1Go_Click(object sender, EventArgs e)
        {
            var zPos = Convert.ToDouble(txtLeftNozzleIniZ1.Text);
            Plc.AxisAbsGo((int)EnumAxis.Z1, zPos, axisVel);
        }

        private void btnLeftNozzleR1Go_Click(object sender, EventArgs e)
        {
            var rPos = Convert.ToDouble(txtLeftNozzleIniR1.Text);
            Plc.AxisAbsGo((int)EnumAxis.R1, rPos, axisVel);
        }

        private void btnRightNozzleXGo_Click(object sender, EventArgs e)
        {
            var xPos = Convert.ToDouble(txtRightNozzleIniX.Text);
            Plc.AxisAbsGo((int)EnumAxis.X, xPos, axisVel);
        }

        private void btnRightNozzleYGo_Click(object sender, EventArgs e)
        {
            var yPos = Convert.ToDouble(txtRightNozzleIniY.Text);
            Plc.AxisAbsGo((int)EnumAxis.Y, yPos, axisVel);
        }

        private void btnRightNozzleZ2Go_Click(object sender, EventArgs e)
        {
            var zPos = Convert.ToDouble(txtRightNozzleIniZ2.Text);
            Plc.AxisAbsGo((int)EnumAxis.Z2, zPos, axisVel);
        }

        private void btnRightNozzleR2Go_Click(object sender, EventArgs e)
        {
            var rPos = Convert.ToDouble(txtRightNozzleIniR2.Text);
            Plc.AxisAbsGo((int)EnumAxis.R2, rPos, axisVel);
        }

        /// <summary>
        /// 验证旋转到中心
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartVerify_Click(object sender, EventArgs e)
        {
            var axisX = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.X);
            var axisY = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Y);
            var axisZ1 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Z1);
            var axisR1 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.R1);
            try
            {
                Plc.SetIO((int)EnumLight.Bottom, true);
                Thread.Sleep(500);
                HImage hImage = ImageProcessBase.GrabImage(2, 12000);
                Plc.SetIO((int)EnumLight.Bottom, false);
                if (hImage == null)
                {
                    MessageBox.Show("未采集图像！");
                    return;
                }
                HOperatorSet.ReduceDomain(hImage, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(), out HObject himage_reduce);
                ImageHelper.DownCameraFindDut(hImage, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(),
                    out HTuple find_row, out HTuple find_column, out HTuple find_rad);
                var rotate_row = Convert.ToDouble(txtLeftNozzleRotateCenterRow.Text);
                var rotate_col = Convert.ToDouble(txtLeftNozzleRotateCenterCol.Text);
                HOperatorSet.HomMat2dIdentity(out HTuple homateMat);
                HOperatorSet.HomMat2dRotate(homateMat, -find_rad, rotate_row, rotate_col, out HTuple homMat2dRotate);
                HOperatorSet.AffineTransPoint2d(homMat2dRotate, find_row, find_column, out HTuple offsetRow, out HTuple offsetCol);
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, offsetCol, offsetRow, out HTuple offsetX, out HTuple offsetY);
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, 2048, 1500, out HTuple centreX, out HTuple centreY);
                double X = centreX - offsetX;
                double Y = centreY - offsetY;
                
                HOperatorSet.TupleDeg(find_rad, out HTuple find_angle);
                SingleAxisCtrl axis_x = Plc.PlcDriver?.GetSingleAxisCtrl(1);
                SingleAxisCtrl axis_y = Plc.PlcDriver?.GetSingleAxisCtrl(2);
                var currentX = axis_x.Info.ActPos;
                var currentY = axis_y.Info.ActPos;
                var goX = currentX + X;
                var goY = currentY + Y;
                axisR1.AbsGo(find_angle, axisVel, true);
                axisX.AbsGo(goX, axisVel, true);
                axisY.AbsGo(goY, axisVel, true);
                Plc.SetIO((int)EnumLight.Bottom, true);
                Thread.Sleep(500);
                HImage hImage2 = ImageProcessBase.GrabImage(2, 12000);
                this.superWind1.image = hImage2;
                HOperatorSet.GenCrossContourXld(out HObject hCross, find_row, find_column, 200, 0);
                this.superWind1.obj = hCross;
               // HOperatorSet.GenCrossContourXld(out HObject hCross1, offsetRow, offsetCol, 200, 0);
               // this.superWind1.obj = hCross1;
            }
            catch (Exception ex)
            {

                 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("请将标定板放置下相机视野","标定",MessageBoxButtons.OKCancel)==DialogResult.OK)
            {
                //* stand_offset_X:=-1.082
                //* stand_offset_Y:= 78.4495
                Plc.SetIO((int)EnumLight.Bottom, true);
                Thread.Sleep(200);
                HImage hImage = ImageProcessBase.GrabImage(2, 12000);
                Plc.SetIO((int)EnumLight.Bottom, false);
                if (!hImage.IsInitialized()&&hImage==null)
                {
                    MessageBox.Show("下相机采集失败，请检查！");
                    return ;
                }
                this.superWind1.image = hImage;
                this.superWind1.Message = "下相机图像采集成功!";
                var axisX = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.X);
                var axisY = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Y);
                var axisZ1 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Z1);
                var axisR1 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.R1);

                //var iniPosX = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadX_RecipePos").KeyValues["LoadUpTakePhotosPosX"].Value);
                //var iniPosY = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadY_RecipePos").KeyValues["SecondUpTakePhotosPosY"].Value);
                //var iniPosZ1 = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadZ1_RecipePos").KeyValues["SecondUpTakePhotosPosZ1"].Value); ;
                //var iniPosR1 = 0.0;
                var iniPosX = 248.714;
                var iniPosY = 530.475;
                var iniPosZ1 = 0.0 ;
                var iniPosR1 = 0.0;


                axisX.AbsGo(iniPosX, axisVel, true);
                axisY.AbsGo(iniPosY, axisVel, true);
                Plc.PlcDriver.GetCylinderCtrl(9).MoveToWork();
                axisZ1.AbsGo(iniPosZ1, axisVel, true);
                axisR1.AbsGo(iniPosR1, axisVel, true);
                Thread.Sleep(200);
                Plc.SetIO((int)EnumLight.LeftTop, true);
                Thread.Sleep(500);
                HImage hImage1 = ImageProcessBase.GrabImage(0, 12000);
                Plc.SetIO((int)EnumLight.LeftTop, false);
                if (!hImage1.IsInitialized() && hImage1 == null)
                {
                    MessageBox.Show("下相机采集失败，请检查！");
                    return;
                }
                this.superWind1.image = hImage1;
                this.superWind1.Message = "确认圆点在视野内!";
                if (MessageBox.Show("继续移动?", "标定", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    axisY.AbsGo(iniPosY-78, axisVel, true);
                    Plc.SetIO((int)EnumLight.Bottom, true);
                    Thread.Sleep(200);
                    HImage hImage2 = ImageProcessBase.GrabImage(0, 12000);
                    Plc.SetIO((int)EnumLight.Bottom, false);
                    if (!hImage2.IsInitialized() && hImage2 == null)
                    {
                        MessageBox.Show("上相机采集失败，请检查！");
                        return;
                    }
                    this.superWind1.image = hImage;
                    this.superWind1.Message = "上相机图像采集成功!";
                }
                else
                {
                    return;
                }
            }
            
        }

        private void btn_UpMark_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("请将标定板放置下相机视野", "标定", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    HImage   hImage= this.superWind1.image.CopyImage();
                    bool flag = ImageHelper.UpCamFindMarkCenter(hImage, this.superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(), out HTuple row, out HTuple col);
                    if (!flag)
                    {
                        MessageBox.Show("识别失败");
                        return;
                    }
                    HOperatorSet.WriteImage(hImage, "bmp", 0, Utility.CalibImageFile + "方法2_左偏移_上相机拍Mark");
                    HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_up1, row, col, out HTuple outX, out HTuple outY);
                    txt_UpX1.Text = outX.ToString();
                    txt_UpY1.Text = outY.ToString();
                    hImage.Dispose();
                }
                   

            }
            catch (Exception ex)
            {


            }
        }

        private void btn_DownMark_Click(object sender, EventArgs e)
        {
            try
            {

                HImage hImage = this.superWind1.image.CopyImage();
                bool flag= ImageHelper.DownCamFindMarkCenter(hImage, this.superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(), out HTuple row, out HTuple col);
                if (!flag)
                {
                    MessageBox.Show("识别失败！");
                    return;
                }
                HOperatorSet.WriteImage(hImage, "bmp", 0, Utility.CalibImageFile + "方法2_左偏移_下相机拍Mark");
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, row,col,  out HTuple outX, out HTuple outY);
                txt_DownX1.Text = outX.ToString();
                txt_DownY1.Text = outY.ToString();
                hImage.Dispose();
            }
            catch (Exception ex)
            {


            }
        }

        private void btn_NozzleMove_Click(object sender, EventArgs e)
        {
            try
            {
                var axisX = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.X);
                var axisY = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Y);

                double x = (double)(Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.X).Info.ActPos);
                double y = (double)(Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Y).Info.ActPos);
                double offsetX = Convert.ToDouble(txt_upmarkX.Text);
                double offsetY = Convert.ToDouble(txt_upmarkY.Text);

                /// -1 和 78 只是需要大概移动的位置，保证吸嘴在视野中就行了,可以手动更改的
                double goX = x + offsetX + 1+ resx;
                double goY = y + offsetY - 78+ resy;
                axisX.AbsGo(goX, axisVel, true);
                axisY.AbsGo(goY, axisVel, true);
                MessageBox.Show("运动完成！");

            }
            catch (Exception ex)
            {


            }
        }
        double resx=0;
        double resy=0;
        private void btn_DownNoz_Click(object sender, EventArgs e)
        {
            try
            {
                var axisZ1 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Z1);
                var axisR1 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.R1);
                axisR1.AbsGo(0, axisVel, true);
                Plc.PlcDriver.GetCylinderCtrl(9).MoveToWork();
                axisZ1.AbsGo(15.491, axisVel, true);
                Thread.Sleep(100);
                Plc.SetIO((int)EnumLight.Bottom, true);
                Thread.Sleep(500);
                HImage hImage = ImageProcessBase.GrabImage(2, 9000);
                Plc.SetIO((int)EnumLight.Bottom, false);
                if (hImage == null)
                {
                    MessageBox.Show("未采集图像！");
                    return;
                }
                this.superWind1.image = hImage;
                HOperatorSet.WriteImage(hImage, "bmp", 0, Utility.CalibImageFile + "方法2_左偏移_下相机拍吸嘴");
                ImageHelper.DownCamFindNozzlCenter(hImage, this.superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(), out HTuple row, out HTuple col, out HTuple angle);
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, row,col,  out HTuple nozzleX, out HTuple nozzleY);
                txt_DownX2.Text = nozzleX.ToString();
                txt_DownY2.Text = nozzleY.ToString();

                double markX =Convert.ToDouble( txt_DownX1.Text);
                double markY = Convert.ToDouble(txt_DownY1.Text);

                 resx = markX - nozzleX;
                 resy = markY - nozzleY;

                /// -1 和 78 只是需要大概移动的位置，保证吸嘴在视野中就行了,可以手动更改的
                txt_ResX.Text = (-1 - resx).ToString();
                txt_ResY.Text = (78 - resy).ToString();


            }
            catch (Exception ex)
            {


            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var axisX = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.X);
            var axisY = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Y);
            var axisZ1 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Z1);
            var axisR1 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.R1);
            Plc.PlcDriver.GetCylinderCtrl(9).MoveToWork();
            var iniPosZ1 = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadZ1_RecipePos").KeyValues["SecondUpTakePhotosPosZ1"].Value);          
            axisZ1.AbsGo(iniPosZ1, 20, true);
            axisR1.AbsGo(0, 20, true);
            try
            {
                Plc.SetIO((int)EnumLight.Bottom, true);
                Thread.Sleep(500);
                HImage hImage = ImageProcessBase.GrabImage(2, 12000);
                Plc.SetIO((int)EnumLight.Bottom, false);
                if (hImage == null)
                {
                    MessageBox.Show("未采集图像！");
                    return;
                }
                HOperatorSet.WriteImage(hImage, "tiff", 0,Utility.CalibFile+"1");
                //HOperatorSet.ReduceDomain(hImage, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(), out HObject himage_reduce);
                //ImageHelper.DownCameraFindDut(hImage, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(),
                //    out HTuple find_row, out HTuple find_column, out HTuple find_rad);
                //if (find_column.Length<=0)
                //{
                //    return;
                //}
                double currentRad = 1.2733238961817344E-05;
                double upcam_rad = -1.564;
                double MatrixRad = -1.559;
                double downRad = -0.200069; //- (-0.04356);
                double offRad= -upcam_rad - downRad + MatrixRad;
                double resRad = currentRad - offRad;
                HOperatorSet.TupleDeg(resRad, out HTuple angle);
                axisR1.AbsGo(angle, 20, true);
                MessageBox.Show("运动完成！");


            }
            catch
            { }
        }

        private void btnVerifyCentre_Click_1(object sender, EventArgs e)
        {

        }

        private void btnDisplay1_Click(object sender, EventArgs e)
        {
            try
            {
                double Row = Convert.ToDouble(txtLeftNozzleCalibMarkX.Text);
                double Col = Convert.ToDouble(txtLeftNozzleCalibMarkY.Text);
                HOperatorSet.ReadTuple(@"C:\ALCvision\calib\calib_down1", out HTuple calib_down);
                HOperatorSet.AffineTransPoint2d(calib_down, Row,Col, out HTuple xC, out HTuple yC);
                HOperatorSet.AffineTransPoint2d(calib_down, 1500,2048,  out HTuple xCentre, out HTuple yCentre);
                double downX = xCentre - xC;
                double downY = yCentre - yC;

                double xShot = Convert.ToDouble(txtLeftNozzleCameraAlignX.Text);
                double yShot = Convert.ToDouble(txtLeftNozzleCameraAlignY.Text);

                double X = Convert.ToDouble(txtLeftNozzleNozzleAlignX.Text);
                double Y = Convert.ToDouble(txtLeftNozzleNozzleAlignY.Text);

                double offX =(xShot - X) + downX;
                double offY =( yShot - Y) + downY;

                txtLeftNozzleOffsetX.Text = offX.ToString("F3");
                txtLeftNozzleOffsetY.Text = offY.ToString("F3");
            }
            catch (Exception ex)
            {

            }

        }

        private void btnDisplay2_Click(object sender, EventArgs e)
        {
            try
            {
                double Row = Convert.ToDouble(txtRightNozzleCalibMarkX.Text);
                double Col = Convert.ToDouble(txtRightNozzleCalibMarkY.Text);
                HOperatorSet.ReadTuple(@"C:\ALCvision\calib\calib_down1", out HTuple calib_down);
                HOperatorSet.AffineTransPoint2d(calib_down, Row,Col,  out HTuple xC, out HTuple yC);
                HOperatorSet.AffineTransPoint2d(calib_down, 1500,2048,  out HTuple xCentre, out HTuple yCentre);
                double downX = xCentre - xC;
                double downY = yCentre - yC;

                double xShot = Convert.ToDouble(txtRightNozzleCameraAlignX.Text);
                double yShot = Convert.ToDouble(txtRightNozzleCameraAlignY.Text);

                double X = Convert.ToDouble(txtRightNozzleNozzleAlignX.Text);
                double Y = Convert.ToDouble(txtRightNozzleNozzleAlignY.Text);

                double offX = xShot - X + downX;
                double offY = yShot - Y + downY;

                txtRightNozzleOffsetX.Text = offX.ToString("F3");
                txtRightNozzleOffsetY.Text = offY.ToString("F3");
            }
            catch (Exception ex)
            {

            }
        }

        private void btn_UpMark1_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("请将标定板放置下相机视野", "标定", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {

                    HImage hImage=this.superWind1.image.CopyImage();
                    bool flag = ImageHelper.UpCamFindMarkCenter(hImage, this.superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(), out HTuple row, out HTuple col);
                    if (!flag)
                    {
                        MessageBox.Show("识别失败");
                        return;
                    }
                    HOperatorSet.WriteImage(hImage, "bmp", 0, Utility.CalibImageFile + "方法2_右偏移_上相机拍Mark");
                    HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_up1, col, row, out HTuple outX, out HTuple outY);
                    txt_upmarkX.Text = outX.ToString();
                    txt_upmarkY.Text = outY.ToString();
                }

            }
            catch (Exception ex)
            {


            }
        }

        private void btn_save1_Click(object sender, EventArgs e)
        {
            try
            {
                AutoNormal_New.serializableData.xLeftOffset = Convert.ToDouble(txt_ResX.Text);
                AutoNormal_New.serializableData.yLeftOffset = Convert.ToDouble(txt_ResY.Text);
                Flow.XmlHelper.SerializeToXml(Utility.CalibFile + "Calib.xml", AutoNormal_New.serializableData);
                Flow.Log("SaveLeftNozzleOffset成功!");
                MessageBox.Show("标定数据保存成功！");
            }
            catch (Exception ex)
            {

                
            }
        }

        private void btn_save2_Click(object sender, EventArgs e)
        {
            try
            {
                AutoNormal_New.serializableData.xRightOffset = Convert.ToDouble(txt_offX.Text);
                AutoNormal_New.serializableData.yRightOffset = Convert.ToDouble(txt_offY.Text);
                Flow.XmlHelper.SerializeToXml(Utility.CalibFile + "Calib.xml", AutoNormal_New.serializableData);
                Flow.Log("SaveRightNozzleOffset成功!");
                MessageBox.Show("标定数据保存成功！");
            }
            catch (Exception ex)
            {


            }
        }

        private void btn_NozzleMove1_Click(object sender, EventArgs e)
        {
            try
            {
                var axisX = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.X);
                var axisY = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Y);

                double x = (double)(Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.X).Info.ActPos);
                double y = (double)(Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Y).Info.ActPos);
                double offsetX = Convert.ToDouble(txt_UpX1.Text);
                double offsetY = Convert.ToDouble(txt_UpY1.Text);

                /// -1 和 78 只是需要大概移动的位置，保证吸嘴在视野中就行了,可以手动更改的
                double goX = x + offsetX + 1 ;
                double goY = y + offsetY - 78 ;
                axisX.AbsGo(goX, axisVel, true);
                axisY.AbsGo(goY, axisVel, true);
                MessageBox.Show("运动完成！");

            }
            catch (Exception ex)
            {


            }
        }

        private void btn_DownMark1_Click(object sender, EventArgs e)
        {
            try
            {

                HImage hImage = this.superWind1.image.CopyImage();
                bool flag = ImageHelper.DownCamFindMarkCenter(hImage, this.superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(), out HTuple row, out HTuple col);
                if (!flag)
                {
                    MessageBox.Show("识别失败！");
                    return;
                }
                HOperatorSet.WriteImage(hImage, "bmp", 0, Utility.CalibImageFile + "方法2_右偏移_下相机拍Mark");
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, col, row, out HTuple outX, out HTuple outY);
                txt_downmarkX.Text = outX.ToString();
                txt_downmarkY.Text = outY.ToString();
                hImage.Dispose();
            }
            catch (Exception ex)
            {


            }
        }

        private void btn_DownNoz1_Click(object sender, EventArgs e)
        {
            try
            {
                var axisZ2 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Z2);
                var axisR2 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.R2);
                axisR2.AbsGo(0, axisVel, true);
                Plc.PlcDriver.GetCylinderCtrl(10).MoveToWork();
                axisZ2.AbsGo(15.927, axisVel, true);
                Thread.Sleep(100);
                Plc.SetIO((int)EnumLight.Bottom, true);
                Thread.Sleep(500);
                HImage hImage = ImageProcessBase.GrabImage(2, 9000);
                Plc.SetIO((int)EnumLight.Bottom, false);
                if (hImage == null)
                {
                    MessageBox.Show("未采集图像！");
                    return;
                }
                this.superWind1.image = hImage;
                HOperatorSet.WriteImage(hImage, "bmp", 0, Utility.CalibImageFile + "方法2_右偏移_下相机拍吸嘴");
                ImageHelper.DownCamFindNozzlCenter(hImage, this.superWind1, ((ROI)this.superWind1.roiController.RoiInfo.ROIList[0]).getRegion(), out HTuple row, out HTuple col, out HTuple angle);
                HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, col, row, out HTuple nozzleX, out HTuple nozzleY);
                txt_downozzleX.Text = nozzleX.ToString();
                txt_downozzleY.Text = nozzleY.ToString();

                double markX = Convert.ToDouble(txt_downmarkX.Text);
                double markY = Convert.ToDouble(txt_downmarkY.Text);

                double X = markX - nozzleX;
                double Y = markY - nozzleY;

                /// -1 和 78 只是需要大概移动的位置，保证吸嘴在视野中就行了,可以手动更改的
                txt_offX.Text = (-2 - X).ToString();
                txt_offY.Text = (76.5 - Y).ToString();
            }
            catch (Exception ex)
            {
            }
        }

        private void button_Recheck_Click(object sender, EventArgs e)
        {
            //拍照
            var axisZ1 = Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Z1);
            Plc.SetIO((int)EnumLight.Bottom, true);
            var iniPosZ1 = Convert.ToDouble(Plc.PlcDriver.CfgParamsConfig.GetParamsModule("LoadZ1_RecipePos").KeyValues["SecondUpTakePhotosPosZ1"].Value);
            Plc.PlcDriver.GetCylinderCtrl(9).MoveToWork();
            axisZ1.AbsGo(iniPosZ1, 20, true);
            Thread.Sleep(300);
            HImage image = CameraManager.CameraById("Bottom").GrabImage(2000);
            //拍照识别dut背面
            ImageProcess_P2D hand = new ImageProcess_P2D();
            HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.DutBackROI);
            InputPara inputPara = new InputPara(image, roi, new HShapeModel(),0.6);
            var result= hand.SecondDutBack(inputPara,new PLCSend());

            //计算要旋转的角度
            //下相机角度偏差二次定位角度和模板角度差
            double downcam_rad = result.Phi;
            double cal_Phi = downcam_rad;
            HOperatorSet.TupleDeg(cal_Phi, out HTuple cal_R);
            //物料中心按照旋转中心旋转回去
            HOperatorSet.HomMat2dIdentity(out HTuple homate);
            HOperatorSet.HomMat2dRotate(homate, -cal_Phi, AutoNormal_New.serializableData.Rotate_Center_Row1, AutoNormal_New.serializableData.Rotate_Center_Col1, out HTuple homate_rotate);
            HOperatorSet.AffineTransPoint2d(homate_rotate, result.findPoint.Row, result.findPoint.Column, out HTuple AfterRotateRow, out HTuple AfterRotateCol);
            //像素点转换成实际的PLC点位置
            HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, AfterRotateRow, AfterRotateCol,  out HTuple transX, out HTuple transY);
            //模板位置
            // HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, AutoNormal_New.serializableData.mode_row1, AutoNormal_New.serializableData.mode_col1, out HTuple transX_mode, out HTuple transY_mode);
            HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, 3000/2,  4096/2, out HTuple transX_mode, out HTuple transY_mode);
            //下相机的偏差
            double offx_down = transX_mode.D - transX.D;
            double offy_down = transY_mode.D - transY.D;


            double CurrentX = (double)(Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.X).Info.ActPos);
            double CurrentY = (double)(Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.Y).Info.ActPos);
            double CurrentR = (double)(Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.R1).Info.ActPos);
            //让吸嘴旋转走回去
            Plc.AxisAbsGo((int)EnumAxis.R1, CurrentR + cal_R.D, axisVel,true);
            CurrentR = (double)(Plc.PlcDriver?.GetSingleAxisCtrl((int)EnumAxis.R1).Info.ActPos);
            Plc.AxisAbsGo((int)EnumAxis.X, CurrentX + offx_down, axisVel, true);
            Plc.AxisAbsGo((int)EnumAxis.Y, CurrentY + offy_down, axisVel, true);
            Thread.Sleep(2000);
            //拍照显示


            //拍照
            HImage image2 = CameraManager.CameraById("Bottom").GrabImage(2000);
            //拍照识别dut背面
            ImageProcess_P2D hand2 = new ImageProcess_P2D();
            InputPara inputPara2 = new InputPara(image2, roi, new HShapeModel(), 0.6);
            var result2 = hand.SecondDutBack(inputPara2, new PLCSend());
            this.superWind1.image = image2;
            HOperatorSet.GenCrossContourXld(out HObject cross_Dut,result2.findPoint.Row,result2.findPoint.Column,98,0);
            // HOperatorSet.GenCrossContourXld(out HObject cross_mode, AutoNormal_New.serializableData.mode_row1, AutoNormal_New.serializableData.mode_col1, 98, 0)
            HOperatorSet.GenCrossContourXld(out HObject cross_center, 3000/2, 4096/2, 98, 0);
            HOperatorSet.GenCrossContourXld(out HObject cross_rotate_center, AutoNormal_New.serializableData.Rotate_Center_Row1,
                AutoNormal_New.serializableData.Rotate_Center_Col1, 98, 0);
            HOperatorSet.GenCrossContourXld(out HObject cross_rotate_before, result.findPoint.Row,
               result.findPoint.Column, 98, 0);
            HOperatorSet.GenCrossContourXld(out HObject cross_afterrotate, AfterRotateRow,
             AfterRotateCol, 98, 0);
            this.superWind1.ObjColor = "orange";
            this.superWind1.obj = cross_rotate_before;
            this.superWind1.ObjColor = "yellow";
            this.superWind1.obj = cross_rotate_center;
            this.superWind1.ObjColor = "red";
            this.superWind1.obj = cross_afterrotate;
            this.superWind1.ObjColor = "green";
            this.superWind1.obj = cross_center;
            this.superWind1.obj = cross_Dut;

            // HOperatorSet.DistancePp(AutoNormal_New.serializableData.mode_row1, AutoNormal_New.serializableData.mode_col1, result2.findPoint.Row, result2.findPoint.Column,out HTuple distance);
            HOperatorSet.DistancePp(3000/2,4096/2, result2.findPoint.Row, result2.findPoint.Column, out HTuple distance);
            MessageBox.Show("偏差"+ distance.D.ToString()+"像素");
        }
    }
}