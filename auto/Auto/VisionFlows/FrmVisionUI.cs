using HalconDotNet;
using Poc2Auto.Common;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using VisionFlows.VisionCalculate;
using WeifenLuo.WinFormsUI.Docking;
using AlcUtility.Language;
using AlcUtility;
using System.Threading;
using Poc2Auto.GUI;
using System.Threading.Tasks;
namespace VisionFlows
{
    public partial class FrmVisionUI : DockContent
    {
        public FrmVisionUI()
        {
            InitializeComponent();
            Flow.Windlist = new System.Collections.Generic.List<VisionControls.SuperWind>();
            this.superWind1.image = new HImage("byte", 256, 256);
            this.superWind2.image = new HImage("byte", 256, 256);
            this.superWind3.image = new HImage("byte", 256, 256);
            this.superWind4.image = new HImage("byte", 256, 256);
            Flow.Windlist.Add(this.superWind1);
            Flow.Windlist.Add(this.superWind2);
            Flow.Windlist.Add(this.superWind3);
            Flow.Windlist.Add(this.superWind4);
            AlcSystem.Instance.LanguageChanged += LanguageChanged;
            UserAuthorityChanged();
            AlcSystem.Instance.UserAuthorityChanged += (o, n) => { UserAuthorityChanged(); };
        }
        private void FrmVisionUI_Load(object sender, EventArgs e)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            RecipeListToolStripComboBox.Items.Clear();
            if (!Directory.Exists(Utility.TypeFile))
                Directory.CreateDirectory(Utility.TypeFile);
            DirectoryInfo theFolder = new DirectoryInfo(Utility.TypeFile);
            FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo NextFile in thefileInfo)  //遍历文件
            {
                RecipeListToolStripComboBox.Items.Add(NextFile.Name);
            }
            RecipeListToolStripComboBox.Text = ConfigMgr.Instance.CurrentImageType;
            try
            {
                ImagePara.Instance = (ImagePara)XmlHelper.Instance().DeserializeFromXml(Utility.TypeFile + ConfigMgr.Instance.CurrentImageType, typeof(ImagePara));
                if (ImagePara.Instance.SoketDut_maxthreshold == null || ImagePara.Instance.SoketDut_maxthreshold.Length <= 0)
                {
                    ImagePara.Instance.SoketDut_maxthreshold = new int[5];
                    ImagePara.Instance.SoketDut_minthreshold = new int[5];
                }

                if (ImagePara.Instance.SocketGet_colCenter == null || ImagePara.Instance.SocketGet_rowCenter == null || ImagePara.Instance.SocketGet_rowCenter.Length <= 0)
                {
                    ImagePara.Instance.SocketGet_rowCenter = new float[5];
                    ImagePara.Instance.SocketGet_colCenter = new float[5];
                    ImagePara.Instance.SocketGet_angleCenter = new float[5];
                    //搜索区域
                    ImagePara.Instance.RegionROI1 = new System.Drawing.Rectangle[5];
                    ImagePara.Instance.RegionROI2 = new System.Drawing.Rectangle[5];

                }
                if (ImagePara.Instance.SocketDut_RoiRow == null || ImagePara.Instance.SocketDut_RoiCol == null)
                {
                    ImagePara.Instance.SocketDut_RoiRow = new float[5];
                    ImagePara.Instance.SocketDut_RoiCol = new float[5];
                    ImagePara.Instance.SocketIsDutROI = new System.Drawing.Rectangle[5];
                }
                UserAuthorityChanged();
            }

            
            catch (Exception ex)
            {
              //  AlcUtility.AlcSystem.Instance.ShowMsgBox("读取视觉配方失败", "提示", AlcUtility.AlcMsgBoxButtons.OK);
            }
          
        }

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormLocationTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            RecipeListToolStripComboBox.Items.Clear();
            DirectoryInfo theFolder = new DirectoryInfo(Utility.TypeFile);
            FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo NextFile in thefileInfo)  //遍历文件
            {
                RecipeListToolStripComboBox.Items.Add(NextFile.Name);
            }
            RecipeListToolStripComboBox.Text = ConfigMgr.Instance.CurrentImageType;
        }
 
        /// <summary>
        /// 标定界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CalibToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Flow.NewCalib == null || Flow.NewCalib.IsDisposed)
            {
                Flow.NewCalib = new FormNewCalib();
                Flow.NewCalib.Show();
            }
            else
            {
                Flow.NewCalib.Show();
            }
        }
        /// <summary>
        /// 设置界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormLocationSet FormLocationTest = new VisionCalculate.FormLocationSet();
            if (FormLocationTest == null || FormLocationTest.IsDisposed)
            {
                FormLocationTest = new VisionCalculate.FormLocationSet();
                FormLocationTest.Show();
            }
            else
            {
                FormLocationTest.Show();
            }
            FormLocationTest.FormClosing += FormLocationTest_FormClosing;
        }
        /// <summary>
        /// 切换配方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 切换配方ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("切换配方？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (!Directory.Exists("project"))
                {
                    Directory.CreateDirectory("project");
                }

                if (RecipeListToolStripComboBox.SelectedIndex < 0)
                    return;
                try
                {
                    ImagePara.Instance = (ImagePara)XmlHelper.Instance().DeserializeFromXml(Utility.TypeFile +
                        RecipeListToolStripComboBox.SelectedItem.ToString(), typeof(ImagePara));

                    ConfigMgr.Instance.CurrentImageType = RecipeListToolStripComboBox.SelectedItem.ToString();
                    AlcUtility.AlcSystem.Instance.ShowMsgBox("读取视觉配方成功", "提示", AlcUtility.AlcMsgBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    AlcUtility.AlcSystem.Instance.ShowMsgBox("读取视觉配方失败" + ex.Message, "提示", AlcUtility.AlcMsgBoxButtons.OK, AlcUtility.AlcMsgBoxIcon.Error);
                }
            }
        }

        private void CameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeviceCamera deviceCamera = new DeviceCamera();
            deviceCamera.ShowDialog();
        }

        private void LanguageChanged(string target, string current)
        {
            LanguageSwitch.SetMenu(this.menuStrip1, target, current);
            LanguageSwitch.SetForm(this,target,current);
        }

        private void UserAuthorityChanged()
        {
            if (AlcSystem.Instance.GetUserAuthority() == UserAuthority.OPERATOR.ToString())
            {
                CameraToolStripMenuItem.Enabled = false;
                CalibToolStripMenuItem.Enabled = false;
                SetToolStripMenuItem.Enabled = false;
                SwitchRecipeToolStripMenuItem.Enabled = false;
                menuStrip1.Enabled = false;

            }
            else if (AlcSystem.Instance.GetUserAuthority() == UserAuthority.ENGINEER.ToString())
            {
                CameraToolStripMenuItem.Enabled = true;
                CalibToolStripMenuItem.Enabled = true;
                SetToolStripMenuItem.Enabled = true;
                SwitchRecipeToolStripMenuItem.Enabled = true;
                menuStrip1.Enabled = true;

            }
        }

 

        private void HelperToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Utility.Vision+"manual\\"+ "7953 2nd SA Vision Manual.pdf");
            }
            catch (Exception ex)
            {
                MessageBox.Show("帮助手册已丢失！");
                
            }
        }





        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {

               
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = 0;
            Task.Factory.StartNew(()=>
            {
                while(index<500)
                {
                    index++;
                     test3();
                   
                }
            });
            
        }

        private void test2()
        {
            GC.Collect();
            Plc.SetIO((int)EnumLight.LeftTop, true);
            Thread.Sleep(100);
            bool state = Plc.ReadIO((int)EnumLight.LeftTop);
            if (!state)
            {
                Thread.Sleep(20);
                Plc.SetIO((int)EnumLight.LeftTop, true);
            }
            var para = new AutoNormalPara();


            AutoNormal_New.Nozzle1GetTrayDut(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, para);
            AutoNormal_New.CheckPutDut_left(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, para);
            AutoNormal_New.Nozzle1PutTrayDut_OneCam(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, para);
            AutoNormal_New.Nozzle2GetTrayDut(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, para);
            AutoNormal_New.Nozzle2PutTrayDut(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, para);
            AutoNormal_New.SecondDut(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, para);
            AutoNormal_New.Nozzle1PutSocketDut(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, para);
            AutoNormal_New.Nozzle2GetSocketDut(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, para);

            Plc.SetIO((int)EnumLight.LeftTop, false);
            Thread.Sleep(20);
            state = Plc.ReadIO((int)EnumLight.LeftTop);
            if (state)
                Plc.SetIO((int)EnumLight.LeftTop, false);
            GC.Collect();
        }
        private void test3()
        {
            //VisionPlugin.BottomCamera(new MessageHandler(VisionPlugin.GetInstance(), "61") { },new ReceivedData());
            //Thread.Sleep(200);
            //VisionPlugin.LeftTopCamera(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, new ReceivedData());
            //Thread.Sleep(200);
            //VisionPlugin.RightTopCamera(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, new ReceivedData());
            //Thread.Sleep(200);
            var handler1 = new MessageHandler(VisionPlugin.GetInstance(), "61") { };
            handler1.CmdParam.KeyValues[PLCParamNames.PLCSend].Value = new double[] { 0, 0, 0,0, 1, 5 };
            VisionPlugin.LeftTopCamera(handler1, new ReceivedData());
            Thread.Sleep(200);
            var handler2=new MessageHandler(VisionPlugin.GetInstance(), "61") { };
            handler2.CmdParam.KeyValues[PLCParamNames.PLCSend].Value = new double[] { 0, 0, 0,0, 1, 21 };
            VisionPlugin.LeftTopCamera(handler2, new ReceivedData());
            Thread.Sleep(200);
            var handler3 = new MessageHandler(VisionPlugin.GetInstance(), "61") { };
            handler3.CmdParam.KeyValues[PLCParamNames.PLCSend].Value = new double[] { 0, 0, 0,0, 1, 9 };
            VisionPlugin.LeftTopCamera(handler3,  new ReceivedData());
            Thread.Sleep(200);
        }

        private void test4()
        {
            //VisionPlugin.BottomCamera(new MessageHandler(VisionPlugin.GetInstance(), "61") { },new ReceivedData());
            //Thread.Sleep(200);
            //VisionPlugin.LeftTopCamera(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, new ReceivedData());
            //Thread.Sleep(200);
            //VisionPlugin.RightTopCamera(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, new ReceivedData());
            //Thread.Sleep(200);
            var handler1 = new MessageHandler(VisionPlugin.GetInstance(), "62") { };
            handler1.CmdParam.KeyValues[PLCParamNames.PLCSend].Value = new double[] { 0, 0, 0, 0, 8, 5 };
            VisionPlugin.RightTopCamera(handler1, new ReceivedData());
            Thread.Sleep(200);
            var handler2 = new MessageHandler(VisionPlugin.GetInstance(), "62") { };
            handler2.CmdParam.KeyValues[PLCParamNames.PLCSend].Value = new double[] { 0, 0, 0, 0, 3, 5 };
            VisionPlugin.RightTopCamera(handler2, new ReceivedData());
            Thread.Sleep(200);
            var handler3 = new MessageHandler(VisionPlugin.GetInstance(), "62") { };
            handler3.CmdParam.KeyValues[PLCParamNames.PLCSend].Value = new double[] { 0, 0, 0, 0, 3, 9 };
            VisionPlugin.RightTopCamera(handler3, new ReceivedData());
            Thread.Sleep(200);
        }

        private void test5()
        {
            //VisionPlugin.BottomCamera(new MessageHandler(VisionPlugin.GetInstance(), "61") { },new ReceivedData());
            //Thread.Sleep(200);
            //VisionPlugin.LeftTopCamera(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, new ReceivedData());
            //Thread.Sleep(200);
            //VisionPlugin.RightTopCamera(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, new ReceivedData());
            //Thread.Sleep(200);
            var handler1 = new MessageHandler(VisionPlugin.GetInstance(), "60") { };
            handler1.CmdParam.KeyValues[PLCParamNames.PLCSend].Value = new double[] { 0, 0, 0, 0, 1,7 };
            VisionPlugin.BottomCamera(handler1, new ReceivedData());
            Thread.Sleep(200);         
        }


        HShapeModel tray_Slot_ModelID;
        private void test()
        {
            GC.Collect();
            Plc.SetIO((int)EnumLight.LeftTop, true);
            Thread.Sleep(100);
            bool state = Plc.ReadIO((int)EnumLight.LeftTop);
            if (!state)
            {
                Thread.Sleep(20);
                Plc.SetIO((int)EnumLight.LeftTop, true);
            }


            HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotDutROI);
            var image = ImageProcessBase.GrabImage(0, ImagePara.Instance.Exposure_LeftCamGetDUT);
            if (image == null)
            {
                Flow.Windlist[0].OKNGlable = false;
                Flow.Log("采图失败!");
                return;
            }
            UCMain.Instance.ShowImage(0, image);
            Flow.Windlist[0].image = image;
            UCMain.Instance.ShowObject(0, roi);
            Flow.Windlist[0].obj = roi;
            //判断是否有料
            InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
            if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
            {
                //无料
                UCMain.Instance.AddInfo((0 + 1).ToString(), "无料或料异常", false);
                UCMain.Instance.ShowMessage(0, "无料或料异常！");
                Flow.Windlist[0].Message = "无料或料异常！";
                image.Dispose();
                para_temp.Dispos();
                return;
            }
            //图像处理
            InputPara parameter = new InputPara(image, roi, null, 0);
            OutPutResult locationResult = AutoNormal_New.ImageProcess.TrayDutFront(parameter);
            if (!locationResult.IsRunOk)
            {
                //匹配失败 显示拍照图片
                UCMain.Instance.AddInfo((0 + 1).ToString(), "Nozzle1GetTrayDut流程失败-TrayDutFront（）", true);
                UCMain.Instance.ShowMessage(0, "Nozzle1GetTrayDut流程失败-TrayDutFront（）");
                Flow.Windlist[0].Message = locationResult.ErrString;
                Flow.Windlist[0].OKNGlable = false;
                parameter.Dispos();
                locationResult.Dispos();
                return;
            }
            double CurrentAngle = 0;
            HOperatorSet.TupleDeg((AutoNormal_New.serializableData.DownCam1_MatrixRad - AutoNormal_New.serializableData.UpCam1_MatrixRad + AutoNormal_New.serializableData.mode_angle1), out HTuple upCameraNozzleAngle);
            double cal_R = 0 - (locationResult.Phi - upCameraNozzleAngle);
            //物料中心按照旋转中心旋转回参考角度
            HOperatorSet.HomMat2dIdentity(out HTuple homateMat);
            HOperatorSet.TupleRad(cal_R, out HTuple phi1);
            HOperatorSet.HomMat2dRotate(homateMat, -phi1, AutoNormal_New.serializableData.Rotate_Center_Row1, AutoNormal_New.serializableData.Rotate_Center_Col1, out HTuple homateRotate);
            HOperatorSet.AffineTransPoint2d(homateRotate, AutoNormal_New.serializableData.mode_row1, AutoNormal_New.serializableData.mode_col1, out HTuple rotedRefNollzeRow, out HTuple rotedRefNollzeCol);
            HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_up1, AutoNormal_New.serializableData.mode_row1, AutoNormal_New.serializableData.mode_col1, out HTuple RefNozzleX, out HTuple RefNozzleY);
            HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_up1, rotedRefNollzeRow, rotedRefNollzeCol, out HTuple rotedRefNollzeX, out HTuple rotedRefNollzeY);
            double rotateOffsetX = RefNozzleX - rotedRefNollzeX;
            double rotateOffsetY = RefNozzleY - rotedRefNollzeY;
            //
            HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_up1, locationResult.findPoint.Row, locationResult.findPoint.Column, out HTuple transX, out HTuple transY);
            HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, AutoNormal_New.serializableData.DownCam_mark_Row, AutoNormal_New.serializableData.DownCam_mark_Col, out HTuple offx1, out HTuple offy1);
            HOperatorSet.AffineTransPoint2d(AutoNormal_New.serializableData.HomMat2D_down1, 1500.0, 2048.0, out HTuple offx2, out HTuple offy2);
            //double x_current = plcSend.XPos;
            //double y_current = plcSend.YPos;
            double x_current = 0;
            double y_current = 0;
            //double offx = serializableData.XShot1 - serializableData.X1;
            //double offy = serializableData.YShot1 - serializableData.Y1;
            ////同轴光源上的mark到相机中心偏差
            //GetMarkOffeset(out double MarkOffesetX, out double MarkOffesetY);
            //double cal_X = transX.D + x_current - offx + MarkOffesetX;
            //double cal_Y = transY.D + y_current - offy + MarkOffesetY;
            double offsetX = AutoNormal_New.serializableData.xLeftOffset;
            double offsetY = AutoNormal_New.serializableData.yLeftOffset;
            //double cal_X = transX.D + x_current - (-1.082);
            //double cal_Y = transY.D + y_current - (78.4495);
            double cal_X = transX.D + x_current - offsetX;
            double cal_Y = transY.D + y_current - offsetY;
            Flow.Windlist[0].Message = "ssssss";
            UCMain.Instance.ShowObject(0, parameter.roi);
            Flow.Windlist[0].obj = parameter.roi;
            UCMain.Instance.ShowObject(0, locationResult.SmallestRec2Xld);
            Flow.Windlist[0].obj = locationResult.SmallestRec2Xld;
            UCMain.Instance.ShowObject(0, locationResult.shapeModelContour);
            Flow.Windlist[0].obj = locationResult.shapeModelContour;
            UCMain.Instance.ShowObject(0, locationResult.region);
            Flow.Windlist[0].obj = locationResult.region;
            parameter.Dispos();
            image.Dispose();
            locationResult.Dispos();
            Plc.SetIO((int)EnumLight.LeftTop, false);
            Thread.Sleep(20);
            state = Plc.ReadIO((int)EnumLight.LeftTop);
            if (state)
                Plc.SetIO((int)EnumLight.LeftTop, false);
            GC.Collect();

            Plc.SendMessage(new MessageHandler(VisionPlugin.GetInstance(), "61") { }, new PLCRecv() { });

            HOperatorSet.SetSystem("clip_region", "false");
            OutPutResult locationResult2 = new OutPutResult();
            try
            {
                if (tray_Slot_ModelID == null || !tray_Slot_ModelID.IsInitialized())
                {
                    tray_Slot_ModelID = new HShapeModel();
                    tray_Slot_ModelID.ReadShapeModel(Utility.ModelFile + "P2D_tray_slot_model");
                    //HOperatorSet.ReadObject(out HObject obj1, Utility.HobjectFile + "P2D_tray_slot_obj.hobj");
                    //HOperatorSet.CreateShapeModelXld(obj1, "auto", -0.39, 0.79, "auto", "auto", "ignore_local_polarity", 5, out tray_Slot_ModelID);
                }
                HOperatorSet.ScaleImage(parameter.image, out HObject imageScale, 5.1, -15);
                HOperatorSet.GetShapeModelContours(out HObject modelContours, tray_Slot_ModelID, 1);
                HOperatorSet.FindShapeModel(imageScale, tray_Slot_ModelID, -0.39, 0.79, 0.3, 1, ImagePara.Instance.SlotScore, "least_squares",
                    0, 0.9, out HTuple row, out HTuple col, out HTuple angle, out HTuple score);
                tray_Slot_ModelID.ClearShapeModel();
                tray_Slot_ModelID.Dispose();
                if (row.Length != 1)
                {
                    parameter.image.Dispose();
                    imageScale.Dispose();
                    modelContours.Dispose();
                    locationResult2.ErrString = "定位失败";
                    locationResult2.IsRunOk = false;
                }
                HOperatorSet.VectorAngleToRigid(0, 0, 0, row, col, angle, out HTuple hom2d);
                HOperatorSet.AffineTransContourXld(modelContours, out HObject contours, hom2d);
                HOperatorSet.ShapeTransXld(contours, out locationResult2.SmallestRec2Xld, "rectangle2");
                locationResult2.findPoint = new PointPosition_Image(row, col);
                locationResult2.Phi = angle;
                //locationResult.region = new HRegion();
                //locationResult.region.GenEmptyRegion();
                parameter.image.Dispose();
                contours.Dispose();
                imageScale.Dispose();
                modelContours.Dispose();
                hom2d.Dispose();
                locationResult2.IsRunOk = true;
            }
            catch (Exception ex)
            {
                locationResult2.ErrString = "图像处理错误";
                string err = ex.Message;
                Flow.Log(ex.Message + ex.StackTrace);
                locationResult2.IsRunOk = false;
            }


            Thread.Sleep(100);
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string path = "D:\\SaveImage";
                if (Directory.Exists(path))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(path);
                    directoryInfo.Delete(true);
                    MessageBox.Show("图片内存清理成功");
                }
                else
                {
                    MessageBox.Show("图片内存为空，无需清理");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("图片内存清理失败");

            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            int index = 0;
            Task.Factory.StartNew(() =>
            {
                while (index < 500)
                {
                    index++;
                    test4();

                }
            });
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            int index = 0;
            Task.Factory.StartNew(() =>
            {
                while (index < 500)
                {
                    index++;
                    test5();

                }
            });
        }

        private void sametrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = 0;
            Task.Factory.StartNew(() =>
            {
                while (index < 500)
                {
                    index++;
                    test6();

                }
            });
        }

        private void test6()
        {
            var handler1 = new MessageHandler(VisionPlugin.GetInstance(), "61") { };
            handler1.CmdParam.KeyValues[PLCParamNames.PLCSend].Value = new double[] { 0, 0, 0, 0, 1, 5 };
            VisionPlugin.LeftTopCamera(handler1, new ReceivedData());
            Thread.Sleep(200);
            var handler2 = new MessageHandler(VisionPlugin.GetInstance(), "61") { };
            handler2.CmdParam.KeyValues[PLCParamNames.PLCSend].Value = new double[] { 0, 0, 0, 0, 1, 21 };
            VisionPlugin.LeftTopCamera(handler2, new ReceivedData());
            Thread.Sleep(200);
            var handler3 = new MessageHandler(VisionPlugin.GetInstance(), "61") { };
            handler3.CmdParam.KeyValues[PLCParamNames.PLCSend].Value = new double[] { 0, 0, 0, 0, 1, 9 };
            VisionPlugin.LeftTopCamera(handler3, new ReceivedData());
            Thread.Sleep(200);
        }
    }
    public class UBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            return ass.GetType(typeName);
        }
    }
}