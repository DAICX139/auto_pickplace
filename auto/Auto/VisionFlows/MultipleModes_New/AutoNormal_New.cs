using AlcUtility;
using HalconDotNet;
using Poc2Auto.Common;
using Poc2Auto.Database;
using Poc2Auto.GUI;
using Poc2Auto.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisionFlows
{ /// <summary>
  /// AutoNormal模式上位机工位枚举
  /// </summary>
    public enum EnumAutoNormal
    {
        Cam1Tray1Mark = 0,
        Cam1Tray1Slot = 1,
        Cam1Tray2Mark = 2,
        Cam1Tray2Slot = 3,
        Cam1Tray3Mark = 4,
        Cam1Tray3Slot = 5,
        Cam1SocketMark = 6,
        Cam1PreciseMark = 7,
        Cam2Tray3Mark = 8,
        Cam2Tray3Slot = 9,
        Cam2Tray4Mark = 10,
        Cam2Tray4Slot = 11,
        Cam2Tray5Mark = 12,
        Cam2Tray5Slot = 13,
        Cam2SocketMark = 14,
        Cam2PreciseMark = 15,
        Cam3Nozzle1Mark = 16,
        Cam3Nozzle2Mark = 17,
        None
    }

    public class AutoNormalPara
    {
        public int WorkID { get; set; }
        public int PosiID { get; set; }
        public int AlgorithmID { get; set; }
        public double CompensateX { get; set; }
        public double CompensateY { get; set; }
        public double CompensateR { get; set; }
    }

    public class AutoNormalResult
    {
        public int WorkID { get; set; }
        public List<AutoNormalCoordi> AutoNormalCoordiList { get; set; }
    }

    public class AutoNormalCoordi
    {
        public int ID { get; set; }
        public double ImageR { get; set; }
        public double ImageC { get; set; }
        public double RobotX { get; set; }
        public double RobotY { get; set; }
    }

    public class AutoNormalData
    {
        public static AutoNormalData Instance;
        public List<AutoNormalPara> AutoNormalParaList;
        public List<AutoNormalResult> AutoNormalResultList;
    }

    
    
    /// <summary>
    /// 自动运行模式相关数据
    /// </summary>
    public class AutoNormal_New
    {

         
        //下相机
        private static HTuple down_row = 0;

        private static HTuple down_col = 0;
        private static HTuple down_angle = 0;
        private static SerializableData _serializableData;
        static ImageProcessBase _ImageProcess;
        public static ImageProcessBase ImageProcess
        {
            get
            {
                if (_ImageProcess == null)
                {
                    //判断当前生产的是哪种料
                    if (ConfigMgr.Instance.CurrentImageType == "")
                    {
                        
                    }
                    else
                    {
                        //POC2这款产品
                        _ImageProcess = new ImageProcess_Poc2();
                    }
                }
                return _ImageProcess;
            }
            set
            {
                _ImageProcess = value;
            }
        }
        //标定数据
        public static SerializableData serializableData
        {
            get
            {
                if (_serializableData == null)
                {
                    _serializableData = (SerializableData)Flow.XmlHelper.DeserializeFromXml(Utility.Config+"calib_up.xml", typeof(SerializableData));
                    HOperatorSet.ReadTuple(Utility.calib+"calib_down1", out _serializableData.HomMat2D_down1);
                    HOperatorSet.ReadTuple(Utility.calib+"calib_down2", out _serializableData.HomMat2D_down2);
                    HOperatorSet.ReadTuple(Utility.calib+"calib_up1", out _serializableData.HomMat2D_up1);
                    HOperatorSet.ReadTuple(Utility.calib+"calib_up2", out _serializableData.HomMat2D_up2);
                }
                return _serializableData;
            }
        }
       
        static HShapeModel _SecondDutBackMode;
        public static HShapeModel SecondDutBackMode
        {
            get
            {
                if (_SecondDutBackMode == null || !_SecondDutBackMode.IsInitialized())
                {
                    return new HShapeModel(Utility.Model + "SecondDutBack.sbm");
                }
                return _SecondDutBackMode;
            }
        }
      

        public static void Execute(MessageHandler handler, EnumCamera cameraID)
        {
            ImageProcessBase.CurrentSoket = RunModeMgr.SocketID-1;

            //判断当前生产的是哪种料
            if (ConfigMgr.Instance.CurrentImageType=="")
            {

            }
            else
            {
                //POC2这款产品
                ImageProcess = new ImageProcess_Poc2();
            }
            var plcSend = (double[])handler.CmdParam.KeyValues[PLCParamNames.PLCSend].Value;
            var work = GetWork(plcSend[(int)EnumPLCSend.PosID], cameraID);
            double func = plcSend[5];
            double posid= plcSend[4];
            var para = AutoNormalData.Instance.AutoNormalParaList[(int)work];
            //左上相机
            if (cameraID == EnumCamera.LeftTop)
            {
                //吸嘴1放料
                if (func == 21)
                {
                    Nozzle1PutTrayDut_OneCam(handler, para);
                }
                //吸嘴1tray取料或者Sockt定位
                else if (func == 5)
                {
                    if (posid == 1 || posid == 2 || posid == 3 || posid == 9)
                    {
                        Nozzle1GetTrayDut(handler, para);
                    }
                    else
                    {
                        Nozzle1PutSocketDut(handler, para);
                    }
                }
                //吸嘴1下料后拍照存图或者Sockt放料完拍照存图
                else if (func == 9)
                {
                    if (posid ==1||posid ==2 || posid == 3 || posid == 4 || posid == 5 || posid == 9)
                    {
                        //放完料拍照看是否放进去了
                        CheckPutDut_left(handler, para);
                    }
                    else
                    {
                        //放完料拍照看是否放进去了 socket
                        CheckPutSocketDut(handler, para);
                    }
                }
                else if(func == 0)
                {
                    ShotSlotMark_left(handler, para);
                }
               
            }
             //右上相机
             else if(cameraID == EnumCamera.RightTop)
            {
                if (func == 9)
                {
                    if( posid == 3 || posid == 4 || posid == 5 || posid == 9)
                    {
                        // tray检测
                        CheckPutDut_right(handler, para);
                    }
                    else
                    {
                        /// SOcket、dut检测
                        CheckPutSocketDut(handler, para);
                    }
                }
                //吸嘴2放料或者吸嘴2取socket料
                else if(func==5)
                {
                    if(posid==8)
                    {
                        Nozzle2GetSocketDut(handler, para);
                    }
                    else
                    {
                        Nozzle2PutTrayDut(handler, para);
                    }
                }
                else if(func == 33)
                {
                    Nozzle2GetTrayDut(handler, para);
                }
                else if (func == 0)
                {
                    ShotSlotMark_right(handler, para);
                }
            }
             //下相机
            else if(cameraID == EnumCamera.Bottom)
            {
                //扫码+定位
                if(func==7)
                {
                    SecondDut(handler, para);
                }
                //只定位
                else if(func == 5)
                {
                    SecondDut(handler, para);
                }
                else if(func == 9)  // 只判断产品有无
                {
                    NozzleDutDetect(handler, para);
                    
                }

                else if (func == 8)  // 只存图
                {

                    SecondDut(handler, para);
                }
            }
            //                    
        }
         
        /// <summary>
        /// 将PLC的工位划分转换为符合视觉习惯的工位划分
        /// </summary>
        /// <param name="posID"></param>
        /// <param name="cameraID"></param>
        /// <returns></returns>
        public static EnumAutoNormal GetWork(double posID, EnumCamera cameraID)
        {
            EnumAutoNormal work;
            switch ((EnumPlcWork)posID)
            {
                case EnumPlcWork.LoadTray1ID:
                    work = EnumAutoNormal.Cam1Tray1Slot;
                    break;

                case EnumPlcWork.LoadTray2ID:
                    work = EnumAutoNormal.Cam1Tray2Slot;
                    break;

                case EnumPlcWork.NGTrayID:
                    work = cameraID == EnumCamera.LeftTop ? EnumAutoNormal.Cam1Tray3Slot : EnumAutoNormal.Cam2Tray3Slot;
                    break;

                case EnumPlcWork.OKTray1ID:
                    work = EnumAutoNormal.Cam2Tray4Slot;
                    break;

                case EnumPlcWork.OKTray2ID:
                    work = EnumAutoNormal.Cam2Tray5Slot;
                    break;

                case EnumPlcWork.LoadSecondPosID:
                    work = EnumAutoNormal.Cam3Nozzle1Mark;
                    break;

                case EnumPlcWork.UnloadSecondPosID:
                    work = EnumAutoNormal.Cam3Nozzle2Mark;
                    break;

                case EnumPlcWork.SocketPosID:
                    work = cameraID == EnumCamera.LeftTop ? EnumAutoNormal.Cam1SocketMark : EnumAutoNormal.Cam2SocketMark;
                    break;

                case EnumPlcWork.TrayMark:
                    work = cameraID == EnumCamera.LeftTop ? EnumAutoNormal.Cam1Tray3Mark : EnumAutoNormal.Cam2Tray3Mark;
                    break;

                default:
                    work = EnumAutoNormal.None;
                    break;
            }
            return work;
        }

        public static void SaveImage(string path, HImage image)
        {
            if (Utility.IsSaveImage)
            {
                string Path = "D:\\SaveImage\\" + path;
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                image.WriteImage("bmp", 0, Path + "\\" + DateTime.Now.ToString("yyyyy-MM-dd-HH-mm-ss"));
            }
        }

        /// <summary>
        /// 相机1获取Tray盘Mark点的拍照位置
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void ShotSlotMark_left(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);

            //var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 0;
            try
            {
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.SlotExposeTime);
                HRegion roi = ImagePara.Instance.GetSerchROI("SlotROI");
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                SaveImage("TraySlot", image);

                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.FrmMain.Windlist[CameraID].obj = roi;

                InputPara parameter = new InputPara(image, roi, null, 0);

                //图像处理
                OutPutResult locationResult = AutoNormal_New.ImageProcess.ShotSlotMark(parameter);

                if (!locationResult.IsRunOk)
                {
                    //Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                    //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, true);//有dut
                    //Utility.SetBitValue(plcRecv.Result, 7, true);//定位失败
                    plcRecv.Result = 128;
                    Flow.Log("TrayMarkPos视觉定位失败", AlcErrorLevel.DEBUG);
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    UCMain.Instance.AddInfo((CameraID+1).ToString(), "图像定位失败", false);
                    UCMain.Instance.ShowMessage(CameraID, "图像定位失败");
                    Flow.FrmMain.Windlist[CameraID].Message = locationResult.ErrString;
                    Plc.SendMessage(handler, plcRecv);
                    image.Dispose();
                    parameter.Dispos();
                    locationResult.Dispos();
                    return;
                }

                #region 计算偏移和角度

                double CurrentAngle = plcSend.RPos;
                double cal_R = CurrentAngle - (locationResult.Angle - serializableData.UpCam_angle1);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, locationResult.findPoint.Column, locationResult.findPoint.Row, out HTuple transX, out HTuple transY);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                //double offx = serializableData.XShot1 - serializableData.X1;
                //double offy = serializableData.YShot1 - serializableData.Y1;
                double cal_X = transX.D + x_current;
                double cal_Y = transY.D + y_current;

                plcRecv.XPos = cal_X + SystemPrara_New.Instance.Nozzle1_Slot_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara_New.Instance.Nozzle1_Slot_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara_New.Instance.Nozzle1_Slot_CompensateR;//角度应该是Tray在机械坐标系的角度，不应该是吸嘴的角度

                #endregion 计算偏移和角度
                plcRecv.Result = 1;
                Plc.SendMessage(handler, plcRecv);
                //显示结果
                HXLDCont hXLDCont = new HXLDCont();
                hXLDCont.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Angle);
                Flow.FrmMain.Windlist[CameraID].obj = hXLDCont;
                UCMain.Instance.ShowObject(CameraID, hXLDCont);
                parameter.Dispos();
                locationResult.Dispos();
                image.Dispose();
                return;
            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                //Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                //Utility.SetBitValue(plcRecv.Result, 7, true);//定位失败
                //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, true);//有dut
                plcRecv.Result = 128;
                Plc.SendMessage(handler, plcRecv);
                UCMain.Instance.AddInfo((CameraID+1).ToString(), "TrayMarkPos致命性错误", true);
                UCMain.Instance.ShowMessage(CameraID, "TrayMarkPos致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "TrayMarkPos致命性错误";
                Flow.Log("TrayMarkPos致命性错误 " + ex.Message + Environment.NewLine + ex.StackTrace);
                return;
            }
        }

        /// <summary>
        /// 相机1获取Tray盘Mark点的拍照位置
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void ShotSlotMark_right(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);

            //var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 1;
            try
            {
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.SlotExposeTime);
                HRegion roi = ImagePara.Instance.GetSerchROI("SlotROI");
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                    //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, false);//无料
                    //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 7, true);//定位失败
                    plcRecv.Result = 128;
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                SaveImage("TraySlot", image);

                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.FrmMain.Windlist[CameraID].obj = roi;

                InputPara parameter = new InputPara(image, roi, null, 0);

                //图像处理
                OutPutResult locationResult = AutoNormal_New.ImageProcess.ShotSlotMark(parameter);

                if (!locationResult.IsRunOk)
                {
                    //Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                    //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, true);//有dut
                    //Utility.SetBitValue(plcRecv.Result, 7, true);//定位失败
                    plcRecv.Result = 128;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("TrayMarkPos视觉定位失败", AlcErrorLevel.DEBUG);
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "图像定位失败", false);
                    UCMain.Instance.ShowMessage(CameraID, "图像定位失败");
                    Flow.FrmMain.Windlist[CameraID].Message = locationResult.ErrString;
                    parameter.Dispos();
                    locationResult.Dispos();
                    image.Dispose();
                    return;
                }

                #region 计算偏移和角度

                double CurrentAngle = plcSend.RPos;
                double cal_R = CurrentAngle - (locationResult.Angle - serializableData.UpCam_angle2);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up2, locationResult.findPoint.Column, locationResult.findPoint.Row, out HTuple transX, out HTuple transY);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                //double offx = serializableData.XShot1 - serializableData.X1;
                //double offy = serializableData.YShot1 - serializableData.Y1;
                double cal_X = transX.D + x_current;
                double cal_Y = transY.D + y_current;

                plcRecv.XPos = cal_X + SystemPrara_New.Instance.Nozzle1_Slot_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara_New.Instance.Nozzle1_Slot_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara_New.Instance.Nozzle1_Slot_CompensateR;//角度应该是Tray在机械坐标系的角度，不应该是吸嘴的角度

                #endregion 计算偏移和角度
                 plcRecv.Result = 1;
                Plc.SendMessage(handler, plcRecv);
                //显示结果
                HXLDCont hXLDCont = new HXLDCont();
                hXLDCont.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Angle);
                Flow.FrmMain.Windlist[CameraID].obj = hXLDCont;
                UCMain.Instance.ShowObject(CameraID, hXLDCont);
                parameter.Dispos();
                locationResult.Dispos();
                image.Dispose();
                return;
            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                //Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                //Utility.SetBitValue(plcRecv.Result, 7, true);//定位失败
                //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, true);//有dut
                plcRecv.Result = 128;
                Plc.SendMessage(handler, plcRecv);
                UCMain.Instance.AddInfo((CameraID + 1).ToString(), "TrayMarkPos致命性错误", true);
                UCMain.Instance.ShowMessage(CameraID, "TrayMarkPos致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "TrayMarkPos致命性错误";
                Flow.Log("TrayMarkPos致命性错误 " + ex.Message + Environment.NewLine + ex.StackTrace);
                return;
            }
        }

        /// <summary>
        /// 吸嘴1拍料中心然后去取
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void Nozzle1GetTrayDut(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);

            //  var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 0;
            try
            {
                HRegion roi = ImagePara.Instance.GetSerchROI("SlotDutROI");
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.DutExposeTime);
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 130;
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                SaveImage("Nozzle1_TrayDutFront", image.CopyImage());

                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.FrmMain.Windlist[CameraID].obj = roi;
                //判断是否有料
                InputPara para_temp = new InputPara(image,  roi, null, ImagePara.Instance.DutScore);
                if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                {
                    //无料
                    plcRecv.Result = 0;
                    UCMain.Instance.AddInfo((CameraID+1).ToString(), "无料", false);
                    UCMain.Instance.ShowMessage(CameraID, "无料！");
                    Flow.FrmMain.Windlist[CameraID].Message = "无料！";
                    Plc.SendMessage(handler, plcRecv);
                    image.Dispose();
                    para_temp.Dispos();

                    return;
                }
                //图像处理
                InputPara parameter = new InputPara(image, roi, null,0) ;
                OutPutResult locationResult = AutoNormal_New.ImageProcess.TrayDutFront(parameter);
                if (!locationResult.IsRunOk)
                {
                    //匹配失败 显示拍照图片
                    plcRecv.Result = 130;
                    Plc.SendMessage(handler, plcRecv);
                    UCMain.Instance.AddInfo((CameraID+1).ToString(), "GetTrayDut定位失败", true);
                    UCMain.Instance.ShowMessage(CameraID, "GetTrayDut定位失败");
                    Flow.FrmMain.Windlist[CameraID].Message = locationResult.ErrString;
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    parameter.Dispos();
                    locationResult.Dispos();
                    return;
                }

                //DUT定位

                #region 计算偏移和角度

                double CurrentAngle = plcSend.RPos;
                double cal_R = CurrentAngle - (locationResult.Angle - serializableData.UpCam_angle1);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, locationResult.findPoint.Column, locationResult.findPoint.Row, out HTuple transX, out HTuple transY);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                double offx = serializableData.XShot1 - serializableData.X1;
                double offy = serializableData.YShot1 - serializableData.Y1;
                double cal_X = transX.D + x_current - offx;
                double cal_Y = transY.D + y_current - offy;

                plcRecv.XPos = cal_X + SystemPrara_New.Instance.Nozzle1_TrayDUT_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara_New.Instance.Nozzle1_TrayDUT_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara_New.Instance.Nozzle1_TrayDUT_CompensateR;

                #endregion 计算偏移和角度

                Flow.Log("TrayDutPos success");
                plcRecv.Result = 3;
                Plc.SendMessage(handler, plcRecv);
                HXLDCont Cross = new HXLDCont();
                HOperatorSet.TupleRad(locationResult.Angle, out HTuple phi);
                Cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, phi);

                //显示结果到FormMain
                Flow.FrmMain.Windlist[CameraID].obj = Cross;
                Flow.FrmMain.Windlist[CameraID].obj = locationResult.SmallestRec2Xld;
                UCMain.Instance.ShowObject(CameraID, Cross);
                UCMain.Instance.ShowObject(CameraID, locationResult.SmallestRec2Xld);
                para_temp.Dispos();
                parameter.Dispos();
                locationResult.Dispos();
                image.Dispose();
                return;
            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 130;
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("TrayDutPos " + ex.Message + Environment.NewLine + ex.StackTrace);
                UCMain.Instance.AddInfo((CameraID+1).ToString(), "GetTrayDut致命性错误", true);
                UCMain.Instance.ShowMessage(CameraID, "GetTrayDut致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "GetTrayDut致命性错误";
                return;
            }
        }
        /// <summary>
        /// 吸嘴1拍料中心然后去取
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void Nozzle2GetTrayDut(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);

            //  var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 1;
            try
            {
                HRegion roi = ImagePara.Instance.GetSerchROI("SlotDutROI");
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.DutExposeTime);
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 130;   // 重拍
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                SaveImage("Nozzle1_TrayDutFront", image);

                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.FrmMain.Windlist[CameraID].obj = roi;
                //判断是否有料
                InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                {
                    //无料
                    plcRecv.Result = 0;   // 无DUT
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "无料", false);
                    UCMain.Instance.ShowMessage(CameraID, "无料！");
                    Flow.FrmMain.Windlist[CameraID].Message = "无料！";
                    Plc.SendMessage(handler, plcRecv);
                    para_temp.Dispos();
                    image.Dispose();
                    return;
                }
                //图像处理
                InputPara parameter = new InputPara(image, roi, null, 0);
                OutPutResult locationResult = AutoNormal_New.ImageProcess.TrayDutFront(parameter);
                if (!locationResult.IsRunOk)
                {
                    //匹配失败 显示拍照图片
                    plcRecv.Result = 130;   // 标记为异常 DUT
                    Plc.SendMessage(handler, plcRecv);
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "GetTrayDut定位失败", true);
                    UCMain.Instance.ShowMessage(CameraID, "GetTrayDut定位失败");
                    Flow.FrmMain.Windlist[CameraID].Message = locationResult.ErrString;
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    image.Dispose();
                    para_temp.Dispos();
                    locationResult.Dispos();
                    parameter.Dispos();
                    return;
                }

                //DUT定位

                #region 计算偏移和角度

                double CurrentAngle = plcSend.RPos;
                double cal_R = CurrentAngle - (locationResult.Angle - serializableData.UpCam_angle2);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up2, locationResult.findPoint.Column, locationResult.findPoint.Row, out HTuple transX, out HTuple transY);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                double offx = serializableData.XShot2 - serializableData.X2;
                double offy = serializableData.YShot2 - serializableData.Y2;
                double cal_X = transX.D + x_current - offx;
                double cal_Y = transY.D + y_current - offy;

                plcRecv.XPos = cal_X + SystemPrara_New.Instance.Nozzle1_TrayDUT_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara_New.Instance.Nozzle1_TrayDUT_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara_New.Instance.Nozzle1_TrayDUT_CompensateR;

                #endregion 计算偏移和角度

                Flow.Log("TrayDutPos success");
                plcRecv.Result = 3;   // 视觉OK可以取料
                Plc.SendMessage(handler, plcRecv);
                HXLDCont Cross = new HXLDCont();
                HOperatorSet.TupleRad(locationResult.Angle, out HTuple phi);
                Cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, phi);

                //显示结果到FormMain
                Flow.FrmMain.Windlist[CameraID].obj = Cross;
                Flow.FrmMain.Windlist[CameraID].obj = locationResult.SmallestRec2Xld;
                UCMain.Instance.ShowObject(CameraID, Cross);
                UCMain.Instance.ShowObject(CameraID, locationResult.SmallestRec2Xld);
                para_temp.Dispos();
                parameter.Dispos();
                locationResult.Dispos();
                image.Dispose();
                return;
            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 130;  //视觉算法异常，定位异常
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("TrayDutPos " + ex.Message + Environment.NewLine + ex.StackTrace);
                UCMain.Instance.AddInfo((CameraID + 1).ToString(), "GetTrayDut致命性错误", true);
                UCMain.Instance.ShowMessage(CameraID, "GetTrayDut致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "GetTrayDut致命性错误";
                return;
            }
        }
        /// <summary>
        /// 吸嘴1 拍槽去放料  加二次定位
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void Nozzle1PutTrayDut(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);

            //  var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 0;
            try
            {
                HRegion roi = ImagePara.Instance.GetSerchROI("SlotROI");
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.SlotExposeTime);
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                SaveImage("Nozzle1_TrayDutFront", image);
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.FrmMain.Windlist[CameraID].obj = roi;
                //保存图像
                if (Utility.GetBitValue(plcSend.Func, 3))
                {
                    //PLC需要判断产品有无
                    if (Utility.GetBitValue(plcSend.Func, 0))
                    {
                        InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                        if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                        {
                            Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                            //有料 则返回失败
                            plcRecv.Result = 2;
                            Plc.SendMessage(handler, plcRecv);
                            image.Dispose();
                            para_temp.Dispos();
                            return;
                        }
                    }
                }
                else
                {
                    InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                    if (AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                    {
                        Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                        //有料 则返回失败
                        plcRecv.Result = 2;
                        Plc.SendMessage(handler, plcRecv);
                        image.Dispose();
                        para_temp.Dispos();
                        return;
                    }
                }

                InputPara parameter = new InputPara(image, roi, null, 0);
                //图像处理
                OutPutResult locationResult = AutoNormal_New.ImageProcess.ShotSlot(parameter);

                if (!locationResult.IsRunOk)
                {
                    plcRecv.RPos = plcSend.RPos;
                    plcRecv.XPos = plcSend.XPos;
                    plcRecv.YPos = plcSend.YPos;
                    plcRecv.Result = 128;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("TrayPlaceDutPos(Slot) Location Failed");
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    UCMain.Instance.AddInfo((CameraID+1).ToString(), "PutTrayDut定位失败", true);
                    UCMain.Instance.ShowMessage(CameraID, "PutTrayDut定位失败");
                    Flow.FrmMain.Windlist[CameraID].Message = locationResult.ErrString;
                    image.Dispose();
                    parameter.Dispos();
                    locationResult.Dispos();
                    return;
                }
                //按照自定义的放料中心仿射变换
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

                #region 计算偏移和角度**

                //当前角度
                double CurrentAngle = plcSend.RPos;
                //下相机角度偏差
                double downcam_angle = down_angle - serializableData.mode_angle1;
                //上相机角度偏差
                double upcam_angle = locationResult.Angle - serializableData.UpCam_angle1;
                //当前拍照角度加上偏差值
                double offeR = upcam_angle - downcam_angle;
                double cal_R = CurrentAngle - offeR;

                //物料中心按照旋转中心旋转回去
                HOperatorSet.HomMat2dIdentity(out HTuple homate);
                HOperatorSet.TupleRad(offeR, out HTuple phi);
                HOperatorSet.HomMat2dRotate(homate, phi, serializableData.Rotate_Center_Row1, serializableData.Rotate_Center_Col1, out HTuple homate_rotate);
                HOperatorSet.AffineTransPoint2d(homate_rotate, down_row, down_col, out HTuple trans_findrow, out HTuple trans_findcolum);

                //像素点转换成实际的PLC点位置
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down1, trans_findrow, trans_findcolum, out HTuple transX, out HTuple transY);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down1, serializableData.mode_row1, serializableData.mode_col1, out HTuple transX_mode, out HTuple transY_mode);
                //下相机的偏差
                double offx_down = transX_mode.D - transX.D;
                double offy_down = transY_mode.D - transY.D;

                //上相机偏差
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, locationResult.findPoint.Column, locationResult.findPoint.Row, out HTuple transX_up, out HTuple transY_up);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                double offx = serializableData.XShot1 - serializableData.X1;
                double offy = serializableData.YShot1 - serializableData.Y1;
                double cal_X = transX_up.D + x_current - offx + offx_down;
                double cal_Y = transY_up.D + y_current - offy + offy_down;

                plcRecv.XPos = cal_X + SystemPrara_New.Instance.Nozzle1_Slot_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara_New.Instance.Nozzle1_Slot_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara_New.Instance.Nozzle1_Slot_CompensateR;
                down_row = 0;
                down_col = 0;
                down_angle = 0;

                #endregion 计算偏移和角度**
                plcRecv.Result = 1;
                Plc.SendMessage(handler, plcRecv);
                //显示结果

                HXLDCont cross = new HXLDCont();
                cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Angle);
                Flow.FrmMain.Windlist[CameraID].obj = cross;
                UCMain.Instance.ShowObject(CameraID, cross);
                parameter.Dispos();
                locationResult.Dispos();
                image.Dispose();
            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 128;
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("TrayPlaceDutPos " + ex.Message + Environment.NewLine + ex.StackTrace);
                UCMain.Instance.AddInfo((CameraID+1).ToString(), "PutTrayDut致命性错误", true);
                UCMain.Instance.ShowMessage(CameraID, "PutTrayDut致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "PutTrayDut致命性错误";
                return;
            }
        }

        /// <summary>
        /// 吸嘴1 拍槽去放料 不加二次定位
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void Nozzle1PutTrayDut_OneCam(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);
            // var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 0;
            try
            {
                HRegion roi = ImagePara.Instance.GetSerchROI("SlotROI");
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.SlotExposeTime);
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;  // 拍照失败，重拍
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.FrmMain.Windlist[CameraID].obj = roi;
                //保存图像
                if (Utility.GetBitValue(plcSend.Func, 3))
                {
                    SaveImage("Nozzle1_Slot", image);
                    //PLC需要判断产品有无
                    if (Utility.GetBitValue(plcSend.Func, 0))
                    {
                        InputPara para_temp = new InputPara(image,roi , null, ImagePara.Instance.DutScore);
                        if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                        {
                            Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                            //有料 则返回失败
                            plcRecv.Result = 2;
                            Plc.SendMessage(handler, plcRecv);
                            para_temp.Dispos();
                            return;
                        }
                    }
                }
                else
                {
                    InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                    if (AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                    {
                        Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                        //有料 则返回失败
                        plcRecv.Result = 2;
                        Plc.SendMessage(handler, plcRecv);
                        image.Dispose();
                        para_temp.Dispos();
                        return;
                    }
                }
                InputPara parameter = new InputPara(image, roi, null, 0);
                //图像处理
                OutPutResult locationResult = AutoNormal_New.ImageProcess.ShotSlot(parameter);
                if (!locationResult.IsRunOk)
                {
                    plcRecv.RPos = plcSend.RPos;
                    plcRecv.XPos = plcSend.XPos;
                    plcRecv.YPos = plcSend.YPos;
                    plcRecv.Result = 128;  // 视觉定位失败
                    Plc.SendMessage(handler, plcRecv);
                    UCMain.Instance.AddInfo((CameraID+1).ToString(), "Nozzle1PutTrayDut图像处理失败", true);
                    UCMain.Instance.ShowMessage(CameraID, "Nozzle1PutTrayDut图像处理失败");
                    Flow.FrmMain.Windlist[CameraID].Message = locationResult.ErrString;
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    parameter.Dispos();
                    locationResult.Dispos();
                    image.Dispose();
                    return;
                }
                //按照自定义的放料中心仿射变换
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

                #region 计算偏移和角度

                double CurrentAngle = plcSend.RPos;
                double cal_R = CurrentAngle - (locationResult.Angle - serializableData.UpCam_angle1);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, locationResult.findPoint.Column, locationResult.findPoint.Row, out HTuple transX, out HTuple transY);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                double offx = serializableData.XShot1 - serializableData.X1;
                double offy = serializableData.YShot1 - serializableData.Y1;
                double cal_X = transX.D + x_current - offx;
                double cal_Y = transY.D + y_current - offy;

                plcRecv.XPos = cal_X + SystemPrara_New.Instance.Nozzle1_Slot_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara_New.Instance.Nozzle1_Slot_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara_New.Instance.Nozzle1_Slot_CompensateR;

                #endregion 计算偏移和角度
                plcRecv.Result = 1;
                Plc.SendMessage(handler, plcRecv);
                //显示结果
                HXLDCont cross = new HXLDCont();
                cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Angle);
                Flow.FrmMain.Windlist[CameraID].obj = cross;
                UCMain.Instance.ShowObject(CameraID, cross);
                parameter.Dispos();
                locationResult.Dispos();
                image.Dispose();
            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 128;
                Plc.SendMessage(handler, plcRecv);
                UCMain.Instance.AddInfo((CameraID+1).ToString(), "Nozzle1PutTrayDut致命性错误", true);
                UCMain.Instance.ShowMessage(CameraID, "Nozzle1PutTrayDut致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "Nozzle1PutTrayDut致命性错误";
                Flow.Log("TrayPlaceDutPos " + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public static void CheckPutDut_left(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);

            //  var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 0;
            try
            {
                HRegion roi = ImagePara.Instance.GetSerchROI("SlotROI");
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.SlotExposeTime);
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.FrmMain.Windlist[CameraID].obj = roi;
                //保存图像               
                    SaveImage("Nozzle1_Slot", image);
                //PLC需要判断产品有无               
                InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                    if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                    {
                       //无料则失败
                        Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                        plcRecv.Result = 0;
                    }
                    else
                {
                    //有料则成功
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = true;
                    plcRecv.Result = 3;
                }
                Plc.SendMessage(handler, plcRecv);
                para_temp.Dispos();
                image.Dispose();
            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 128;
                Plc.SendMessage(handler, plcRecv);
                UCMain.Instance.AddInfo((CameraID+1).ToString(), "Nozzle1PutTrayDut致命性错误", true);
                UCMain.Instance.ShowMessage(CameraID, "Nozzle1PutTrayDut致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "Nozzle1PutTrayDut致命性错误";
                Flow.Log("TrayPlaceDutPos " + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        public static void CheckPutDut_right(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);

            //  var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 1;
            try
            {
                HRegion roi = ImagePara.Instance.GetSerchROI("SlotROI");
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.SlotExposeTime);
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;  // 定位失败，需要重拍
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.FrmMain.Windlist[CameraID].obj = roi;
                //保存图像               
                SaveImage("Nozzle1_Slot", image);

                //PLC需要判断产品有无               
                InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                {
                    //无料则失败
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 0;  // 无DUT
                }
                else
                {
                    //有料则成功
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 3;  // 有DUT
                }
                Plc.SendMessage(handler, plcRecv);
                image.Dispose();
                para_temp.Dispos();
            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 128;   // 出现错处  理解为定位异常
                Plc.SendMessage(handler, plcRecv);
                UCMain.Instance.AddInfo((CameraID + 1).ToString(), "Nozzle1PutTrayDut致命性错误", true);
                UCMain.Instance.ShowMessage(CameraID, "Nozzle1PutTrayDut致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "Nozzle1PutTrayDut致命性错误";
                Flow.Log("TrayPlaceDutPos " + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
        /// <summary>
        /// 吸嘴1拍Socke并放置dut
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void Nozzle1PutSocketDut(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);
            //  var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
          int  CameraID = 0;
            try
            {
                HRegion roi = ImagePara.Instance.GetSerchROI("SocketMarkROI");
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.SoketMarkExposeTime);
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.FrmMain.Windlist[CameraID].obj = roi;
              
               

                //保存图像
                if (Utility.GetBitValue(plcSend.Func, 3))
                {
                    SaveImage("Nozzle1_PutSocketDut(检查)", image);
                    //PLC需要判断产品有无
                    if (Utility.GetBitValue(plcSend.Func, 0))
                    {
                        if (AutoNormal_New.ImageProcess.SocketDetect(image, out HObject yy))
                        {
                            //有料是对的
                            plcRecv.Result = 3;
                            Flow.FrmMain.Windlist[CameraID].OKNGlable = true;
                        }
                        else
                        {
                            //料不见了
                            plcRecv.Result = 0;
                            UCMain.Instance.AddInfo((CameraID+1).ToString(), "无料", false);
                            UCMain.Instance.ShowMessage(CameraID, "无料");
                            Flow.FrmMain.Windlist[CameraID].Message = "无料!";
                            Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                            Plc.SendMessage(handler, plcRecv);
                            image.Dispose();
                            return;
                        }
                    }
                    plcRecv.Result = 3;
                    Plc.SendMessage(handler, plcRecv);
                    image.Dispose();
                    return;
                }
                else
                {
                    SaveImage("Nozzle1_SocketMark" + (ImageProcessBase.CurrentSoket + 1).ToString(), image);
                    if (AutoNormal_New.ImageProcess.SocketDetect(image, out HObject yy))
                    {
                        //有料则不能去放
                        plcRecv.Result = 2;
                        Flow.FrmMain.Windlist[CameraID].OKNGlable = true;
                        Plc.SendMessage(handler, plcRecv);
                        image.Dispose();
                        return;
                    }
                }

                //图像处理
                InputPara parameter = new InputPara(image, roi, null,0);

                //检查推块位置是否到位
                ImageProcess.FindSocketMark(image, out HTuple row1, out HTuple col1, out HTuple phi1, out HObject mark1);
                mark1.Dispose();
                if (row1.Length > 0)
                {
                    bool result = ImageProcess.BlockDetect(row1, col1, parameter, out int distance, out HObject arrow);
                    if(!result)
                    {
                        plcRecv.RPos = 0;
                        plcRecv.XPos = plcSend.XPos + para.CompensateX;
                        plcRecv.YPos = plcSend.YPos + para.CompensateY;
                        // 推块太短
                        plcRecv.Result = 0;
                        Plc.SendMessage(handler, plcRecv);
                        UCMain.Instance.AddInfo((CameraID + 1).ToString(), "推块位置太短！", true);
                        UCMain.Instance.ShowMessage(CameraID, "推块位置太短！");
                        Flow.FrmMain.Windlist[CameraID].Message = "推块位置太短！";
                        Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                        Flow.FrmMain.Windlist[CameraID].obj = arrow;
                        arrow.Dispose();
                        image.Dispose();
                        parameter.Dispos();
                        return;
                    }
                   
                }
                //正常处理
                OutPutResult locationResult = AutoNormal_New.ImageProcess.SocketMark(parameter, out HObject mark);
                Flow.FrmMain.Windlist[CameraID].obj = mark;
                UCMain.Instance.ShowObject(CameraID, mark);
                if (locationResult.IsRunOk != true)
                {
                    plcRecv.RPos = 0;
                    plcRecv.XPos = plcSend.XPos + para.CompensateX;
                    plcRecv.YPos = plcSend.YPos + para.CompensateY;
                    plcRecv.Result = 128;
                    Plc.SendMessage(handler, plcRecv);
                    UCMain.Instance.AddInfo((CameraID+1).ToString(), "定位失败", true);
                    UCMain.Instance.ShowMessage(CameraID, "定位失败");
                    Flow.FrmMain.Windlist[CameraID].Message = locationResult.ErrString;
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    locationResult.Dispos();
                    parameter.Dispos();
                    return;
                }

                #region 计算偏移和角度**

                //当前角度
                double CurrentAngle = plcSend.RPos;
                //下相机角度偏差
                double downcam_angle = down_angle - serializableData.mode_angle1;
                //上相机角度偏差
                double upcam_angle = locationResult.Angle - serializableData.UpCam_angle1;
                //当前拍照角度加上偏差值
                double offeR = upcam_angle - downcam_angle;
                double cal_R = CurrentAngle - offeR;

                //物料中心按照旋转中心旋转回去
                HOperatorSet.HomMat2dIdentity(out HTuple homate);
                HOperatorSet.TupleRad(offeR, out HTuple phi);
                HOperatorSet.HomMat2dRotate(homate, phi, serializableData.Rotate_Center_Row1, serializableData.Rotate_Center_Col1, out HTuple homate_rotate);
                HOperatorSet.AffineTransPoint2d(homate_rotate, down_row, down_col, out HTuple trans_findrow, out HTuple trans_findcolum);

                //像素点转换成实际的PLC点位置
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down1, trans_findrow, trans_findcolum, out HTuple transX, out HTuple transY);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down1, serializableData.mode_row1, serializableData.mode_col1, out HTuple transX_mode, out HTuple transY_mode);
                //下相机的偏差
                double offx_down = transX_mode.D - transX.D;
                double offy_down = transY_mode.D - transY.D;

                //上相机偏差
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, locationResult.findPoint.Column, locationResult.findPoint.Row, out HTuple transX_up, out HTuple transY_up);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                double offx = serializableData.XShot1 - serializableData.X1;
                double offy = serializableData.YShot1 - serializableData.Y1;

                double cal_X = transX_up.D + x_current - offx + offx_down;
                double cal_Y = transY_up.D + y_current - offy + offy_down;

                plcRecv.XPos = cal_X + SystemPrara_New.Instance.Nozzle1_Socket_Put_DUT_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara_New.Instance.Nozzle1_Socket_Put_DUT_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara_New.Instance.Nozzle1_Socket_Put_DUT_CompensateR;
                down_row = 0;
                down_col = 0;
                down_angle = 0;

                #endregion 计算偏移和角度**
                plcRecv.Result = 1;
                Plc.SendMessage(handler, plcRecv);
                //显示结果
                HXLDCont cross = new HXLDCont();
                cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Angle);
                Flow.FrmMain.Windlist[CameraID].obj = cross;
                UCMain.Instance.ShowObject(CameraID, cross);
                image.Dispose();
                locationResult.Dispos();
                parameter.Dispos();
                return;
            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 128;
                UCMain.Instance.AddInfo((CameraID+1).ToString(), "PutSocketDut致命性错误", true);
                UCMain.Instance.ShowMessage(CameraID, "PutSocketDut致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "PutSocketDut致命性错误";
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("SocketMarkPos " + ex.Message + Environment.NewLine + ex.StackTrace);
                return;
            }
        }
        /// <summary>
        /// 判断产品有没有放入socket
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void CheckPutSocketDut(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);
            //  var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 0;
            try
            {
                HRegion roi = ImagePara.Instance.GetSerchROI("SocketMarkROI");
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.SoketMarkExposeTime);
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;   // 采集图像失败， 理解为 定位失败，重新拍照
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.FrmMain.Windlist[CameraID].obj = roi;
                //保存图像
                SaveImage("Nozzle1_PutSocketDut(检查)", image);                  
                if (AutoNormal_New.ImageProcess.SocketDetect(image, out HObject yy))
                {
                    //有料是对的
                    plcRecv.Result = 3;  // 有DUT
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = true;
                }
                else
                {
                    //料不见了
                    plcRecv.Result = 0;  // 无DUT
                    UCMain.Instance.AddInfo((CameraID+1).ToString(), "无料", false);
                    UCMain.Instance.ShowMessage(CameraID, "无料");
                    Flow.FrmMain.Windlist[CameraID].Message = "无料!";
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                }
                Plc.SendMessage(handler, plcRecv);
                image.Dispose();

            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 128; // 算法异常，定位失败。重拍
                UCMain.Instance.AddInfo((CameraID+1).ToString(), "PutSocketDut致命性错误", true);
                UCMain.Instance.ShowMessage(CameraID, "PutSocketDut致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "PutSocketDut致命性错误";
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("SocketMarkPos " + ex.Message + Environment.NewLine + ex.StackTrace);
                return;
            }
        }

        /// <summary>
        /// 判断产品从socket中有没有被取起来
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void CheckGetSocketDut(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);
            //  var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 1;
            try
            {
                HRegion roi = ImagePara.Instance.GetSerchROI("SocketMarkROI");
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.SoketMarkExposeTime);
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.FrmMain.Windlist[CameraID].obj = roi;
                //保存图像
                SaveImage("Nozzle1_PutSocketDut(检查)", image);
                if (AutoNormal_New.ImageProcess.SocketDetect(image, out HObject yy))
                {
                    //有料是错的 代表没有被取起来
                    plcRecv.Result = 3;
                    UCMain.Instance.AddInfo((CameraID+1).ToString(), "料未取起来", false);
                    UCMain.Instance.ShowMessage(CameraID, "料未取起来");
                    Flow.FrmMain.Windlist[CameraID].Message = "料未取起来!";
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                  
                }
                else
                {
                    //空的槽代表料被取走了 对的
                    plcRecv.Result = 0;
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = true;
                }
                Plc.SendMessage(handler, plcRecv);
                image.Dispose();

            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 128;
                UCMain.Instance.AddInfo((CameraID+1).ToString(), "PutSocketDut致命性错误", true);
                UCMain.Instance.ShowMessage(CameraID, "PutSocketDut致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "PutSocketDut致命性错误";
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("SocketMarkPos " + ex.Message + Environment.NewLine + ex.StackTrace);
                return;
            }
        }
        public static void Nozzle2GetSocketDut(MessageHandler handler, AutoNormalPara para)
        {
            
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);
          //  var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
           
            int CameraID = 1;
            try
            {
                HRegion roi = ImagePara.Instance.GetSerchROI("SocketDutROI");
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.SoketDutExposeTime);
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 130;   // 拍照错误，理解为 定位异常
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.FrmMain.Windlist[CameraID].obj = roi;
                SaveImage("Nozzle2_GetSocketDut_"+ (ImageProcessBase.CurrentSoket+1).ToString(), image);

                if (!AutoNormal_New.ImageProcess.SocketDetect(image, out HObject yy))
                {
                    //无料 则不能再取料  失败
                    plcRecv.Result = 0; //没有DUT 不能去取
                    UCMain.Instance.AddInfo((CameraID+1).ToString(), "无料", false);
                    UCMain.Instance.ShowMessage(CameraID, "无料");
                    Flow.FrmMain.Windlist[CameraID].Message = "无料!";
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    Plc.SendMessage(handler, plcRecv);
                    image.Dispose();
                    return;
                }

                //图像处理
                InputPara parameter = new InputPara(image, roi, null, 0);

                OutPutResult locationResult = AutoNormal_New.ImageProcess.SocketDutFront(parameter);
                //定位是否成功判断
                if (!locationResult.IsRunOk)
                {
                    plcRecv.Result = 130;  // 有 DUT 定位失败
                    Plc.SendMessage(handler, plcRecv);
                    UCMain.Instance.AddInfo((CameraID+1).ToString(), "定位失败", true);
                    UCMain.Instance.ShowMessage(CameraID, "定位失败");
                    Flow.FrmMain.Windlist[CameraID].Message = locationResult.ErrString;
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    image.Dispose();
                    locationResult.Dispos();
                    parameter.Dispos();
                    return;
                }
                HXLDCont cross = new HXLDCont();
                cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Angle);
                Flow.FrmMain.Windlist[CameraID].obj = cross;
                UCMain.Instance.ShowObject(CameraID, cross);
                //按照自定义的取料中心仿射变换
                HHomMat2D hv_HomMat2D = new HHomMat2D();
                if (locationResult.Angle < -0.79)
                {
                    locationResult.Angle = 3.14159 + locationResult.Angle;
                }
                hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.DutMode_rowCenter[ImageProcessBase.CurrentSoket], ImagePara.Instance.DutMode_colCenter[ImageProcessBase.CurrentSoket],
                    ImagePara.Instance.DutMode_angleCenter[ImageProcessBase.CurrentSoket], locationResult.findPoint.Row, locationResult.findPoint.Column, locationResult.Angle);
                HTuple findrow = hv_HomMat2D.AffineTransPoint2d(ImagePara.Instance.SocketMark_GetDutRow[ImageProcessBase.CurrentSoket], ImagePara.Instance.SocketMark_GetDutCol[ImageProcessBase.CurrentSoket], out HTuple findcol);
                locationResult.findPoint.Row = findrow;
                locationResult.findPoint.Column = findcol;

                #region 计算偏移和角度

                double CurrentAngle = plcSend.RPos;
                double cal_R = CurrentAngle - (locationResult.Angle - serializableData.UpCam_angle1);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up2, locationResult.findPoint.Column, locationResult.findPoint.Row, out HTuple transX, out HTuple transY);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                double offx = serializableData.XShot2 - serializableData.X2;
                double offy = serializableData.YShot2 - serializableData.Y2;
                double cal_X = transX.D + x_current - offx;
                double cal_Y = transY.D + y_current - offy;

                plcRecv.XPos = cal_X + SystemPrara_New.Instance.Nozzle2_Socket_Get_DUT_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara_New.Instance.Nozzle2_Socket_Get_DUT_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara_New.Instance.Nozzle2_Socket_Get_DUT_CompensateR;

                #endregion 计算偏移和角度

                plcRecv.Result = 3;  // 有DUT 视觉定位成功
                Plc.SendMessage(handler, plcRecv);

                //显示结果
                cross = new HXLDCont();
                cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Angle);
                Flow.FrmMain.Windlist[CameraID].obj = cross;
                Flow.FrmMain.Windlist[CameraID].obj = locationResult.region;
                Flow.FrmMain.Windlist[CameraID].obj = locationResult.SmallestRec2Xld;
                UCMain.Instance.ShowObject(CameraID, cross);
                UCMain.Instance.ShowObject(CameraID, locationResult.region);
                UCMain.Instance.ShowObject(CameraID, locationResult.SmallestRec2Xld);
                locationResult.Dispos();
                parameter.Dispos();
                image.Dispose();
                return;
            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 130;   // 视觉算法错误，重新拍照，定位失败
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("SocketDutPos " + ex.Message + Environment.NewLine + ex.StackTrace);
                UCMain.Instance.AddInfo((CameraID+1).ToString(), "PutSocketDut致命性错误", true);
                UCMain.Instance.ShowMessage(CameraID, "PutSocketDut致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "PutSocketDut致命性错误";
            }
        }

        /// <summary>
        /// 吸嘴2 放置DUT到Tray
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
      


        public static void Nozzle2PutTrayDut(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);

            //var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 1;
            try
            {
                HRegion roi = ImagePara.Instance.GetSerchROI("SlotROI");
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.SlotExposeTime);
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;  // 拍照
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.FrmMain.Windlist[CameraID].obj = roi;

                InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                //保存图像
                if (Utility.GetBitValue(plcSend.Func, 3))
                {
                    //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 0, true);//总结果OK

                    //PLC需要判断产品有无
                    if (Utility.GetBitValue(plcSend.Func, 0))
                    {
                        if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy1))
                        {
                            //无料 放的料不见了 报错
                            plcRecv.Result = 0;   // 无DUT
                            Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                            Plc.SendMessage(handler, plcRecv);
                            image.Dispose();
                            para_temp.Dispos();
                            return;
                        }
                        else
                        {
                            //有料  放的料OK 
                            plcRecv.Result = 3;   // 有DUT
                            Flow.FrmMain.Windlist[CameraID].OKNGlable = true;
                            Plc.SendMessage(handler, plcRecv);
                            image.Dispose();
                            para_temp.Dispos();
                            return;
                        }
                    }
                }

                if (AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                {
                    //有料则不能去放
                    plcRecv.Result = 2;    //有料则不能去放
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    Plc.SendMessage(handler, plcRecv);
                    image.Dispose();
                    para_temp.Dispos();
                    return;
                }
                //图像处理
                InputPara parameter = new InputPara(image, roi, new HShapeModel(Utility.Model + "TraySlot.sbm"), 0);
                OutPutResult locationResult = AutoNormal_New.ImageProcess.ShotSlot(parameter);

                if (!locationResult.IsRunOk)
                {
                    plcRecv.Result = 128;   // 视觉定位异常
                    Plc.SendMessage(handler, plcRecv);
                    UCMain.Instance.AddInfo(CameraID.ToString(), "定位失败", true);
                    UCMain.Instance.ShowMessage(CameraID, "定位失败");
                    Flow.FrmMain.Windlist[CameraID].Message = locationResult.ErrString;
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    image.Dispose();
                    locationResult.Dispos();
                    parameter.Dispos();
                    return;
                }
                //按照自定义的放料中心仿射变换
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

                #region 计算偏移和角度**

                double CurrentAngle = plcSend.RPos;
                double cal_R = CurrentAngle - (locationResult.Angle - serializableData.UpCam_angle1);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up2, locationResult.findPoint.Column, locationResult.findPoint.Row, out HTuple transX, out HTuple transY);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                double offx = serializableData.XShot2 - serializableData.X2;
                double offy = serializableData.YShot2 - serializableData.Y2;
                double cal_X = transX.D + x_current - offx;
                double cal_Y = transY.D + y_current - offy;

                plcRecv.XPos = cal_X + SystemPrara_New.Instance.Nozzle2_Slot_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara_New.Instance.Nozzle2_Slot_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara_New.Instance.Nozzle2_Slot_CompensateR;

                #endregion 计算偏移和角度**
                // 视觉 OK  可以去放疗
                plcRecv.Result = 1;

                Plc.SendMessage(handler, plcRecv);

                //显示结果
                HXLDCont cross = new HXLDCont();
                cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Angle);
                Flow.FrmMain.Windlist[CameraID].obj = cross;
                UCMain.Instance.ShowObject(CameraID, cross);
                image.Dispose();
                locationResult.Dispos();
                parameter.Dispos();
                return;
            }
            catch (Exception ex)
            {
                plcRecv.Result = 128;  // 视觉算法异常 重拍
                Plc.SendMessage(handler, plcRecv);
                UCMain.Instance.AddInfo(CameraID.ToString(), "致命性错误", true);
                UCMain.Instance.ShowMessage(CameraID, "致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "致命性错误";
                Flow.Log("TrayPlaceDutPos(Slot) " + ex.Message + Environment.NewLine + ex.StackTrace);
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                return;
            }
        }

      
        public static void SecondDut(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);

            //   var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 2;
            try
            {
                HRegion roi = ImagePara.Instance.GetSerchROI("DutBackROI");
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.DutBackExposeTime);
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 130;  // 出现异常
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.FrmMain.Windlist[CameraID].obj = roi;
                //保存图像
                SaveImage("SecondDutBack", image);


                //图像处理
                InputPara parameter = new InputPara(image, roi, SecondDutBackMode, 0);

                OutPutResult locationResult = AutoNormal_New.ImageProcess.SecondDutBack(parameter, plcSend);
                if (!locationResult.IsRunOk)
                {
                    // 有DUT 视觉定位失败
                    plcRecv.Result = 130;
                    
                    UCMain.Instance.AddInfo(CameraID.ToString(), "定位失败", true);
                    UCMain.Instance.ShowMessage(CameraID, "定位失败");
                    Flow.FrmMain.Windlist[CameraID].Message = locationResult.ErrString;
                    Plc.SendMessage(handler, plcRecv);
                    image.Dispose();
                    locationResult.Dispos();
                    parameter.Dispos();
                    return;
                }

                #region 计算偏移和角度

                //记录二次定位的物料位置和角度
                down_row = locationResult.findPoint.Row;
                down_col = locationResult.findPoint.Column;
                down_angle = locationResult.Angle;

                #endregion 计算偏移和角度

                //PLC不扫码
                if (!Utility.GetBitValue(plcSend.Func, 1))
                {
                    // 有DUT视觉只定位OK
                    plcRecv.Result = 3;
                    
                }
                else
                {
                    if (!locationResult.IsReadDataCode) //如果读码未成功
                    {
                        // 有DUT 视觉定位成功 扫码失败
                        plcRecv.Result = 2;

                        Plc.SendMessage(handler, plcRecv);
                        Overall.ScanResult = "";
                        image.Dispose();
                        locationResult.Dispos();
                        parameter.Dispos();
                        return;
                    }
                    else
                    {
                        // 有DUT 视觉定位成功 扫码成功
                        plcRecv.Result = 7;

                        Overall.ScanResult = locationResult.DatacodeString;
                    }
                }

                //sn码模式并且sn在列表里面  需要把Double[6]:BinValue值 写1
                if (RunModeMgr.RunMode == RunMode.AutoSelectSn)
                {
                    if (Overall.SelectionList.Contains(Overall.ScanResult))
                    {
                        RunModeMgr.IsMatchDutSN = "Yes";
                        plcRecv.BinValue = 1;
                    }
                    else
                    {
                        RunModeMgr.IsMatchDutSN = "No";
                        plcRecv.BinValue = 4;
                    }
                }
                if (RunModeMgr.RunMode == RunMode.AutoSelectBin)
                {
                    if (!locationResult.IsReadDataCode)
                    {
                        plcRecv.BinValue = 98;
                    }
                    else
                    {
                        plcRecv.BinValue = DragonDbHelper.GetBinFromProducts(Overall.ScanResult);
                    }
                }


                // Doeslip 滑移模式 保存数据
                if (RunModeMgr.RunMode == RunMode.DoeSlip)
                {
                    HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down1, locationResult.findPoint.Column, locationResult.findPoint.Row, out HTuple transX, out HTuple transY);
                    string data = DateTime.Now.ToLocalTime().ToString() + "," + transX.D.ToString("f3") + "," + transY.D.ToString("f3");
                    SaveData.WriteCsv("DoeSlipData.csv", data);
                }


                Plc.SendMessage(handler, plcRecv);
                //显示结果
                UCMain.Instance.ShowObject(CameraID, locationResult.SmallestRec2Xld);
                Flow.FrmMain.Windlist[CameraID].obj = locationResult.SmallestRec2Xld;
                HXLDCont CrossContour = new HXLDCont();
                CrossContour.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Angle);
                UCMain.Instance.ShowObject(CameraID, CrossContour);
                Flow.FrmMain.Windlist[CameraID].obj = CrossContour;
                UCMain.Instance.AddInfo(CameraID.ToString(), "二维码:" + Overall.ScanResult, false);
                UCMain.Instance.ShowMessage(CameraID, "二维码:" + Overall.ScanResult);
                Flow.FrmMain.Windlist[CameraID].Message = "二维码:" + Overall.ScanResult;
                image.Dispose();
                locationResult.Dispos();
                parameter.Dispos();
            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                // 有DUT 算法处理过程出现异常
                plcRecv.Result = 130;
                plcRecv.BinValue = 0;
                Plc.SendMessage(handler, plcRecv);
                UCMain.Instance.AddInfo(CameraID.ToString(), "致命性错误", false);
                UCMain.Instance.ShowMessage(CameraID, "致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "致命性错误";
                Flow.Log("SecondDutPos 定位失败" + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// 检测产品是否在吸嘴上
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void NozzleDutDetect(MessageHandler handler, AutoNormalPara para)
        {
            PLCSend plcSend = new PLCSend();
            PLCRecv plcRecv = new PLCRecv();

            ImageProcessBase.GetPlcData(handler, out plcSend, out plcRecv);

            //   var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 2;
            try
            {
                HImage image = new HImage();
                image.Dispose();

                image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.DutBackExposeTime);
                if (image == null)
                {
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 130;    //总结果
                   
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.FrmMain.Windlist[CameraID].image = image;

                // 保存图像
                SaveImage("SecondDutBack", image);
                // 图像处理
                if (DutBackgroundDetect(image))
                {
                    // 有料，
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = true;
                    plcRecv.Result = 3;    //有料

                    // 显示
                    UCMain.Instance.AddInfo(CameraID.ToString(), "有料", true);
                    UCMain.Instance.ShowMessage(CameraID, "有料");
                    Flow.FrmMain.Windlist[CameraID].Message = "有料";

                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("有料");

                    // 释放
                    image.Dispose();

                    return;
                }
                else
                {
                    // 无料，
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = true;
                    plcRecv.Result = 0;    //无料
                    

                    // 显示
                    UCMain.Instance.AddInfo(CameraID.ToString(), "无料", true);
                    UCMain.Instance.ShowMessage(CameraID, "无料");
                    Flow.FrmMain.Windlist[CameraID].Message = "无料";

                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("有料");

                    // 释放

                    image.Dispose();
                    return;
                }
            }

            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 130;   // 出现异常
                Plc.SendMessage(handler, plcRecv);
                UCMain.Instance.AddInfo(CameraID.ToString(), "致命性错误", false);
                UCMain.Instance.ShowMessage(CameraID, "致命性错误");
                Flow.FrmMain.Windlist[CameraID].Message = "致命性错误";
                Flow.Log("SecondDutPos 定位失败" + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public static bool DutBackgroundDetect(HImage image)
        {
            
            #region 图像处理


            //图像处理
            HObject region ;
            HObject regionFillUp;
            HObject connectionRegion ;
            HObject selectRegion ;
            HObject imageReduce;
            HObject unionRegion ;
            HOperatorSet.Threshold(image, out region, 230, 255);
            HOperatorSet.FillUp(region, out regionFillUp);
            HOperatorSet.Connection(regionFillUp, out connectionRegion);
            HOperatorSet.SelectShape(connectionRegion, out selectRegion, new HTuple("outer_radius", "circularity"), "and",
                new HTuple(1200, 0.8), new HTuple(1800, 1));
            HOperatorSet.ReduceDomain(image, selectRegion, out imageReduce);
            HOperatorSet.Threshold(imageReduce, out region, 0, 120);
            HOperatorSet.Connection(region, out connectionRegion);
            HOperatorSet.SelectShape(connectionRegion, out selectRegion, new HTuple("outer_radius"), "and",
                new HTuple(200), new HTuple(1400));
            HOperatorSet.FillUp(selectRegion, out regionFillUp);
            HOperatorSet.Union1(regionFillUp, out unionRegion);
            HTuple Length1 = new HTuple(), Length2 = new HTuple();
            Length1.Dispose(); Length2.Dispose();
            HOperatorSet.SmallestRectangle2(unionRegion, out _, out _, out _, out Length1, out Length2);
            #endregion

            if (1100 < Length1.D && Length1.D < 1300 && 300 < Length2.D && Length2.D < 500)
            {
                // 有料，

                // 释放
                imageReduce.Dispose();
                region.Dispose();
                regionFillUp.Dispose();
                connectionRegion.Dispose();
                selectRegion.Dispose();
                unionRegion.Dispose();
                Length1.Dispose();
                Length2.Dispose();

                return true;

            }

            else
            {
                // 无料，
                // 释放
                imageReduce.Dispose();
                region.Dispose();
                regionFillUp.Dispose();
                connectionRegion.Dispose();
                selectRegion.Dispose();
                unionRegion.Dispose();
                Length1.Dispose();
                Length2.Dispose();

                return false;

            }
        }
    }

    #region CSV数据保存

    class SaveData
    {

        public static void WriteCsv(string name, string data)
        {
            using (FileStream fs = new System.IO.FileStream(name, FileMode.Append, FileAccess.Write))
            {
                //开始写入文件，streamwriter的第二个参数是设置编码，默认是utf-8
                using (StreamWriter m_streamWriter = new StreamWriter(fs))
                {
                    //写入文件标题，按照循环的方式写入数据
                    //  m_streamWriter.WriteLine("12" + "," + "21" + "," + "323" + "," + "333");
                    m_streamWriter.WriteLine(data);
                    //关闭写入数据，数据写入完成
                    m_streamWriter.Flush();
                    m_streamWriter.Close();
                }
                fs.Close();
            }
        }
    }
    #endregion
}