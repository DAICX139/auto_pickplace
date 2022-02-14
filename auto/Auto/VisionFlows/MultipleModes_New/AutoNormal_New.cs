using AlcUtility;
using AlcUtility.PlcDriver.CommonCtrl;
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

        public AutoNormal_New()
        {
            RunModeMgr.RunModeChanged += RunModeMgr_RunModeChanged;
        }

        private void RunModeMgr_RunModeChanged()
        {
            if (RunModeMgr.RunMode == RunMode.DryRun)
            {
                RunModeMgr.OriginValue = true;
            }
            else
            {
                RunModeMgr.OriginValue = false;
            }
        }

        //下相机
        private static HTuple down_row = 0;

        private static HTuple down_col = 0;
        private static HTuple down_phi = 0;
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
                    else if (ConfigMgr.Instance.CurrentImageType == "P2D.xml")
                    {
                        if(!(_ImageProcess is ImageProcess_P2D) || _ImageProcess == null)
                        {
                            //P2D产品
                            _ImageProcess = new ImageProcess_P2D();
                        }
                       
                    }
                    else if (ConfigMgr.Instance.CurrentImageType == "P1O.xml")
                    {
                        if (!(_ImageProcess is ImageProcess_Poc2)|| _ImageProcess==null)
                        {
                            //POC2这款产品
                            _ImageProcess = new ImageProcess_Poc2();
                        }
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
                    _serializableData = (SerializableData)Flow.XmlHelper.DeserializeFromXml(Utility.CalibFile + "Calib.xml", typeof(SerializableData));
                    HOperatorSet.ReadTuple(Utility.CalibFile + "calib_down1", out _serializableData.HomMat2D_down1);
                    HOperatorSet.ReadTuple(Utility.CalibFile + "calib_down2", out _serializableData.HomMat2D_down2);
                    HOperatorSet.ReadTuple(Utility.CalibFile + "calib_up1", out _serializableData.HomMat2D_up1);
                    HOperatorSet.ReadTuple(Utility.CalibFile + "calib_up2", out _serializableData.HomMat2D_up2);
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
                    return new HShapeModel(Utility.ModelFile + "SecondDutBack.sbm");
                }
                return _SecondDutBackMode;
            }
        }
        private static void OriginValueSendBack(MessageHandler handler, double result, int CameraID)
        {
            var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_LeftCamGetDUT);
            if (image != null && image.IsInitialized())
            {
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.Windlist[CameraID].image = image;
                image.Dispose();
            }
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend2, out PLCRecv plcRecv2);
            var plcSend = (double[])handler.CmdParam.KeyValues[PLCParamNames.PLCSend].Value;
            var plcRecv = new double[6];
            plcRecv[(int)EnumPLCRecv.XPos] = plcSend[(int)EnumPLCSend.XPos];
            plcRecv[(int)EnumPLCRecv.YPos] = plcSend[(int)EnumPLCSend.YPos];
            plcRecv[(int)EnumPLCRecv.ZPos] = plcSend[(int)EnumPLCSend.ZPos];
            plcRecv[(int)EnumPLCRecv.RPos] = plcSend[(int)EnumPLCSend.RPos];


            plcRecv[(int)EnumPLCRecv.Result] = result;
            handler.CmdParam.KeyValues[PLCParamNames.PLCRecv].Value = plcRecv;
            handler.SendMessage(new ReceivedData()
            {
                ModuleId = ModuleTypes.Handler.ToString(),
                Data = new MessageData() { Param = handler.CmdParam }
            }, -2);
            Flow.Log("视觉屏蔽已开启   plcRecv[(int)EnumPLCRecv.XPos]:" + plcRecv[(int)EnumPLCRecv.XPos] + "    plcRecv[(int)EnumPLCRecv.YPos]" + plcRecv[(int)EnumPLCRecv.YPos]);
        }

        public static void Execute(MessageHandler handler, EnumCamera cameraID)
        {
            ImageProcessBase.CurrentSoket = RunModeMgr.SocketID - 1;

            var plcSend = (double[])handler.CmdParam.KeyValues[PLCParamNames.PLCSend].Value;
            //var work = GetWork(plcSend[(int)EnumPLCSend.PosID], cameraID);
            double func = plcSend[5];
            double posid = plcSend[4];
            //var para = AutoNormalData.Instance.AutoNormalParaList[(int)work];
            var para = new AutoNormalPara();
            //左上相机
            if (cameraID == EnumCamera.LeftTop)
            {
                //吸嘴1放料
                if (func == 21)
                {
                    if (RunModeMgr.OriginValue)
                    {
                        //OriginValueSendBack(handler, 1, 0);
                        Nozzle1PutTrayDut_OneCam(handler, para);
                        return;
                    }
                    Nozzle1PutTrayDut_OneCam(handler, para);
                }
                //吸嘴1tray取料或者Sockt定位
                else if (func == 5)
                {
                    if (posid == 1 || posid == 2 || posid == 3 || posid == 9)
                    {
                        if (RunModeMgr.OriginValue)
                        {
                            OriginValueSendBack(handler, 3, 0);
                            return;
                        }
                        Nozzle1GetTrayDut(handler, para);
                    }
                    else
                    {
                        if (RunModeMgr.OriginValue)
                        {
                            OriginValueSendBack(handler, 1, 0);
                            return;
                        }
                        HOperatorSet.CountSeconds(out HTuple s1);
                        Nozzle1PutSocketDut(handler, para);
                        HOperatorSet.CountSeconds(out HTuple s2);
                        double s = s2 - s1;

                    }
                }
                //吸嘴1下料后拍照存图或者Sockt放料完拍照存图
                else if (func == 9)
                {
                    if (posid == 1 || posid == 2 || posid == 3 || posid == 4 || posid == 5 || posid == 9)
                    {
                        if (RunModeMgr.OriginValue)
                        {
                            OriginValueSendBack(handler, 3, 0);
                            return;
                        }
                        //放完料拍照看是否放进去了
                        if (RunModeMgr.RunMode == RunMode.AutoSelectBin|| RunModeMgr.RunMode == RunMode.AutoSelectSn|| RunModeMgr.RunMode == RunMode.AutoNormal)
                        {
                            CheckPutDut_left_plus(handler,para);
                        }
                        else
                        {
                            CheckPutDut_left(handler, para);
                        }
                        
                    }
                    else
                    {
                        if (RunModeMgr.OriginValue)
                        {
                            OriginValueSendBack(handler, 3, 0);
                            return;
                        }
                        //放完料拍照看是否放进去了 socket
                        CheckPutSocketDut(handler, para);
                    }
                }
                else if (func == 0)
                {
                    if (RunModeMgr.OriginValue)
                    {
                        OriginValueSendBack(handler, 1, 0);
                        return;
                    }
                    ShotSlotMark_left(handler, para);
                }

            }
            //右上相机
            else if (cameraID == EnumCamera.RightTop)
            {
                if (func == 9)
                {
                    if (posid == 3 || posid == 4 || posid == 5 || posid == 9)
                    {
                        if (RunModeMgr.OriginValue)
                        {
                            OriginValueSendBack(handler, 3, 1);
                            return;
                        }
                        // tray检测
                        if (RunModeMgr.RunMode == RunMode.AutoSelectBin || RunModeMgr.RunMode == RunMode.AutoSelectSn || RunModeMgr.RunMode == RunMode.AutoNormal)
                        {
                            CheckPutDut_right_plus(handler, para);
                        }
                        else
                        {
                            CheckPutDut_right(handler, para); 
                        }
                        
                    }
                    else
                    {
                        if (RunModeMgr.OriginValue)
                        {
                            OriginValueSendBack(handler, 3, 1);
                            return;
                        }
                        /// SOcket、dut检测
                        /// 
                        ///
                        CheckPutSocketDut(handler, para);
                    }
                }
                //吸嘴2放料或者吸嘴2取socket料
                else if (func == 5)
                {
                    if (posid == 8)
                    {
                        if (RunModeMgr.OriginValue)
                        {
                            OriginValueSendBack(handler, 3, 1);
                            return;
                        }
                        Nozzle2GetSocketDut(handler, para);
                    }
                    else
                    {
                        if (RunModeMgr.OriginValue)
                        {
                            OriginValueSendBack(handler, 1, 1);
                            return;
                        }
                        Nozzle2PutTrayDut(handler, para);
                    }
                }
                else if (func == 33)
                {
                    if (RunModeMgr.OriginValue)
                    {
                        OriginValueSendBack(handler, 3, 1);
                        return;
                    }
                    Nozzle2GetTrayDut(handler, para);
                }
                else if (func == 0)
                {
                    if (RunModeMgr.OriginValue)
                    {
                        OriginValueSendBack(handler, 1, 1);
                        return;
                    }
                    ShotSlotMark_right(handler, para);
                }
            }
            //下相机
            else if (cameraID == EnumCamera.Bottom)
            {
                //扫码+定位
                if (func == 7)
                {
                    if (RunModeMgr.OriginValue)
                    {
                        OriginValueSendBack(handler, 7, 2);
                        return;
                    }
                    SecondDut(handler, para);

                }
                //只定位
                else if (func == 5)
                {
                    if (RunModeMgr.OriginValue)
                    {
                        OriginValueSendBack(handler, 7, 2);
                        return;
                    }
                    SecondDut(handler, para);
                }
                else if (func == 9)  // 只判断产品有无
                {
                    if (RunModeMgr.OriginValue)
                    {
                        OriginValueSendBack(handler, 3, 2);
                        return;
                    }
                    NozzleDutDetect(handler, para);

                }

                else if (func == 8)  // 只存图
                {
                    if (RunModeMgr.OriginValue)
                    {
                        OriginValueSendBack(handler, 7, 2);
                        return;
                    }
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
            if (Utility.IsSaveAllImage)
            {
                string Path = "D:\\SaveImage\\" + "所有图片\\" + path;
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                image.WriteImage("bmp", 0, Path + "\\" + DateTime.Now.ToString("yyyyy-MM-dd-HH-mm-ss"));
            }
            else if (Utility.IsSaveNgImage)
            {
                string Path = "D:\\SaveImage\\" + "异常图片\\" + path;
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                image.WriteImage("bmp", 0, Path + "\\" + DateTime.Now.ToString("yyyyy-MM-dd-HH-mm-ss"));
            }
        }

        /// <summary>
        /// 保存原图
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hImage"></param>
        public static void SaveOriginalImage(string DutSN,string path, HObject hImage,bool socketFlag)
        {
            string socketID = "";
            if (socketFlag)
            {
                socketID ='_'+"S"+ RunModeMgr.SocketID.ToString();
            }           
            string nowDate = DateTime.Now.ToString("yyyy-MM-dd") + "\\";
            string nowTime = '_'+ DateTime.Now.ToString("yyyyMMdd-HHmmss");
            string savepath = "D:\\SaveImage\\原图\\" + nowDate + path;
            string SN ='_'+ DutSN ;
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }
            try
            {
                String Name = savepath + "\\" + path + socketID + SN + nowTime;
                HOperatorSet.WriteImage(hImage, "bmp", 0, Name);
                
            }
            catch (Exception ex)
            {

            }



        }

        /// <summary>
        /// 保存截图
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hImage"></param>
        public static void SaveDumpImage(string DutSN, string path, HObject hImage, bool socketFlag)
        {
            string socketID = "";
            if (socketFlag)
            {
                socketID = '_' + "S" + RunModeMgr.SocketID.ToString();
            }
            string nowDate = DateTime.Now.ToString("yyyy-MM-dd") + "\\";
            string nowTime = '_' + DateTime.Now.ToString("yyyyMMdd-HHmmss");
            string savepath = "D:\\SaveImage\\截图\\" + nowDate + path;
            string SN = '_' + DutSN;
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }
            try
            {
                String Name = savepath + "\\" + path + socketID + SN + nowTime;
                HOperatorSet.WriteImage(hImage, "bmp", 0, Name);

            }
            catch (Exception ex)
            {

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
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_LeftCamPutDUT);
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotROI);
                if (image == null)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");            
                    return;
                }
                //保存图片

                SaveOriginalImage("","markCalib_Left", image,false);
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.Windlist[CameraID].obj = roi;
                InputPara parameter = new InputPara(image, roi, null, 0);
                //图像处理
                OutPutResult locationResult = AutoNormal_New.ImageProcess.ShotSlotMark(parameter);
                if (!locationResult.IsRunOk)
                {
                    //SaveImage("markCalib_Left_NG", image);
                    plcRecv.Result = 128;
                    Flow.Log("ShotSlotMark_left流程失败-ShotSlotMark()", AlcErrorLevel.DEBUG);
                    Flow.Windlist[CameraID].OKNGlable = false;
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "图像定位失败", false);
                    UCMain.Instance.ShowMessage(CameraID, "ShotSlotMark_left流程失败-ShotSlotMark()");
                    Flow.Windlist[CameraID].Message = locationResult.ErrString;
                    HOperatorSet.DumpWindowImage(out HObject dumpimg, Flow.Windlist[CameraID].hwind.HalconWindow);
                    //SaveDumpImage("markCalib_Left", image);
                    SaveDumpImage("", "markCalib_Left", dumpimg, false);
                    
                    dumpimg.Dispose();
                    Plc.SendMessage(handler, plcRecv);
                    image.Dispose();
                    parameter.Dispos();
                    locationResult.Dispos();                   
                    return;
                }

                #region 计算偏移和角度

                double CurrentAngle = plcSend.RPos;
                double cal_R = CurrentAngle - (locationResult.Phi - serializableData.UpCam1_MatrixRad);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, locationResult.findPoint.Row,locationResult.findPoint.Column,  out HTuple transX, out HTuple transY);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;

                double cal_X = transX.D + x_current;
                double cal_Y = transY.D + y_current;

                plcRecv.XPos = cal_X; //+ SystemPrara.Instance.Nozzle1_Slot_CompensateX;
                plcRecv.YPos = cal_Y; //+ SystemPrara.Instance.Nozzle1_Slot_CompensateY;
                plcRecv.RPos = cal_R; //+ SystemPrara.Instance.Nozzle1_Slot_CompensateR;//角度应该是Tray在机械坐标系的角度，不应该是吸嘴的角度

                #endregion 计算偏移和角度
                plcRecv.Result = 1;
                Plc.SendMessage(handler, plcRecv);
                //显示结果
                HXLDCont hXLDCont = new HXLDCont();
                hXLDCont.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Phi);
                Flow.Windlist[CameraID].obj = hXLDCont;
                UCMain.Instance.ShowObject(CameraID, hXLDCont);

     

                parameter.Dispos();
                locationResult.Dispos();
                image.Dispose();
                return;
            }
            catch (Exception ex)
            {
                Flow.Windlist[CameraID].OKNGlable = false;
                //Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                //Utility.SetBitValue(plcRecv.Result, 7, true);//定位失败
                //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, true);//有dut
                plcRecv.Result = 128;
                Plc.SendMessage(handler, plcRecv);
                UCMain.Instance.AddInfo((CameraID + 1).ToString(), "ShotSlotMark_left流程失败-ShotSlotMark()", true);
                UCMain.Instance.ShowMessage(CameraID, "ShotSlotMark_left流程失败-ShotSlotMark()");
                Flow.Windlist[CameraID].Message = "ShotSlotMark_left流程失败-ShotSlotMark()";
                Flow.Log("ShotSlotMark_left流程失败-ShotSlotMark() " + ex.Message + Environment.NewLine + ex.StackTrace);
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
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_RightCamPutDUT);
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotROI);
                if (image == null)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                    //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, false);//无料
                    //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 7, true);//定位失败
                    plcRecv.Result = 128;
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");                   
                    return;
                }
                SaveOriginalImage("","MarkCalib_Right", image,false);
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.Windlist[CameraID].obj = roi;
                InputPara parameter = new InputPara(image, roi, null, 0);
                //图像处理
                OutPutResult locationResult = AutoNormal_New.ImageProcess.ShotSlotMark(parameter);

                if (!locationResult.IsRunOk)
                {
                    //Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                    //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, true);//有dut
                    //Utility.SetBitValue(plcRecv.Result, 7, true);//定位失败
                    //SaveImage("MarkCalib_Right_NG", image);
                    plcRecv.Result = 128;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("ShotSlotMark_right流程失败-ShotSlotMark（）", AlcErrorLevel.DEBUG);
                    Flow.Windlist[CameraID].OKNGlable = false;
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "图像定位失败", false);
                    UCMain.Instance.ShowMessage(CameraID, "ShotSlotMark_right流程失败-ShotSlotMark（）");
                    Flow.Windlist[CameraID].Message = locationResult.ErrString;
                    HOperatorSet.DumpWindowImage(out HObject dumpimg, Flow.Windlist[CameraID].hwind.HalconWindow);
                    //SaveDumpImage("markCalib_Right", dumpimg);
                    SaveDumpImage("", "markCalib_Right", dumpimg, false);
                    dumpimg.Dispose();
                    parameter.Dispos();
                    locationResult.Dispos();
                    image.Dispose();
                    return;
                }

                #region 计算偏移和角度

                double CurrentAngle = plcSend.RPos;
                double cal_R = CurrentAngle - (locationResult.Phi - serializableData.UpCam2_MatrixRad);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up2, locationResult.findPoint.Row,locationResult.findPoint.Column,  out HTuple transX, out HTuple transY);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                //double offx = serializableData.XShot1 - serializableData.X1;
                //double offy = serializableData.YShot1 - serializableData.Y1;
                double cal_X = transX.D + x_current;
                double cal_Y = transY.D + y_current;

                //同轴光源上的mark到相机中心偏差
                GetMarkOffeset(out double OffesetX, out double OffesetY);


                plcRecv.XPos = cal_X;// + SystemPrara.Instance.Nozzle1_Slot_CompensateX;
                plcRecv.YPos = cal_Y;// + SystemPrara.Instance.Nozzle1_Slot_CompensateY;
                plcRecv.RPos = cal_R;// + SystemPrara.Instance.Nozzle1_Slot_CompensateR;//角度应该是Tray在机械坐标系的角度，不应该是吸嘴的角度

                #endregion 计算偏移和角度
                plcRecv.Result = 1;
                Plc.SendMessage(handler, plcRecv);
                //显示结果
                HXLDCont hXLDCont = new HXLDCont();
                hXLDCont.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Phi);
                Flow.Windlist[CameraID].obj = hXLDCont;
                UCMain.Instance.ShowObject(CameraID, hXLDCont);
                //SaveImage("MarkCalib_Right", image);

                parameter.Dispos();
                locationResult.Dispos();
                image.Dispose();
                return;
            }
            catch (Exception ex)
            {
                Flow.Windlist[CameraID].OKNGlable = false;
                //Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                //Utility.SetBitValue(plcRecv.Result, 7, true);//定位失败
                //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, true);//有dut
                plcRecv.Result = 128;
                Plc.SendMessage(handler, plcRecv);
                UCMain.Instance.AddInfo((CameraID + 1).ToString(), "ShotSlotMark_right流程失败-ShotSlotMark（）", true);
                UCMain.Instance.ShowMessage(CameraID, "ShotSlotMark_right流程失败-ShotSlotMark（）");
                Flow.Windlist[CameraID].Message = "ShotSlotMark_right流程失败-ShotSlotMark（）";
                Flow.Log("ShotSlotMark_right流程失败-ShotSlotMark（） " + ex.Message + Environment.NewLine + ex.StackTrace);
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
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotDutROI);
                var image1 = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_LeftCamGetDUT);
                if (image1 == null)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 130;
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");                   
                    return;
                }
               
                UCMain.Instance.ShowImage(CameraID, image1);
                Flow.Windlist[CameraID].image = image1;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.Windlist[CameraID].obj = roi;
                //判断是否有料
                InputPara para_temp = new InputPara(image1, roi, null, ImagePara.Instance.DutScore);
                if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                {
                    //无料
                    plcRecv.Result = 0;
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "无料或料异常", false);
                    UCMain.Instance.ShowMessage(CameraID, "无料或料异常！");
                    Flow.Windlist[CameraID].Message = "无料或料异常！";
                    HOperatorSet.DumpWindowImage(out HObject dumpimg1, Flow.Windlist[CameraID].hwind.HalconWindow);
                    SaveDumpImage("", "N1GetTrayDut_NoDut", dumpimg1, false);
                    SaveOriginalImage("", "N1GetTrayDut_NoDut", image1,false);
                    //SaveDumpImage("Nozzle1_GetTrayDut", dumpimg1);
                    dumpimg1.Dispose();
                    Plc.SendMessage(handler, plcRecv);
                    image1.Dispose();
                    para_temp.Dispos();
                    return;
                }
                //图像处理
                InputPara parameter = new InputPara(image1, roi, null, 0);
                OutPutResult locationResult = AutoNormal_New.ImageProcess.TrayDutFront(parameter);
                if (!locationResult.IsRunOk)
                {
                    //匹配失败 显示拍照图片
                    plcRecv.Result = 130;
                    Plc.SendMessage(handler, plcRecv);
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "Nozzle1GetTrayDut流程失败-TrayDutFront（）", true);
                    UCMain.Instance.ShowMessage(CameraID, "Nozzle1GetTrayDut流程失败-TrayDutFront（）");
                    Flow.Windlist[CameraID].Message = locationResult.ErrString;
                    Flow.Windlist[CameraID].OKNGlable = false;
                    HOperatorSet.DumpWindowImage(out HObject dumpimg2, Flow.Windlist[CameraID].hwind.HalconWindow);
                    SaveDumpImage("", "N1GetTrayDut_Error", dumpimg2, false);
                    SaveOriginalImage("", "N1GetTrayDut_Error", image1, false);

                    dumpimg2.Dispose();
                    image1.Dispose();
                    parameter.Dispos();
                    locationResult.Dispos();
                    return;
                }
                SingleAxisCtrl axis_x = Plc.PlcDriver?.GetSingleAxisCtrl(1);
                SingleAxisCtrl axis_y = Plc.PlcDriver?.GetSingleAxisCtrl(2);
                var Xxx = axis_x.Info.ActPos;
                var Yyy = axis_y.Info.ActPos;
                //DUT定位

                #region 计算偏移和角度

                double CurrentAngle = plcSend.RPos;
                HOperatorSet.TupleDeg((serializableData.DownCam1_MatrixRad - serializableData.UpCam1_MatrixRad + serializableData.mode_angle1), out HTuple upCameraNozzleAngle);
                double cal_R = 0 - (locationResult.Phi - upCameraNozzleAngle);
                //物料中心按照旋转中心旋转回参考角度
                HOperatorSet.HomMat2dIdentity(out HTuple homateMat);
                HOperatorSet.TupleRad(cal_R, out HTuple phi1);
                HOperatorSet.HomMat2dRotate(homateMat, -phi1, serializableData.Rotate_Center_Row1, serializableData.Rotate_Center_Col1, out HTuple homateRotate);
                HOperatorSet.AffineTransPoint2d(homateRotate, serializableData.mode_row1, serializableData.mode_col1, out HTuple rotedRefNollzeRow, out HTuple rotedRefNollzeCol);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, serializableData.mode_row1,serializableData.mode_col1,  out HTuple RefNozzleX, out HTuple RefNozzleY);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, rotedRefNollzeRow,rotedRefNollzeCol,  out HTuple rotedRefNollzeX, out HTuple rotedRefNollzeY);
                double rotateOffsetX = RefNozzleX - rotedRefNollzeX;
                double rotateOffsetY = RefNozzleY - rotedRefNollzeY;
                //
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, locationResult.findPoint.Row,locationResult.findPoint.Column,  out HTuple transX, out HTuple transY);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down1, serializableData.DownCam_mark_Row, serializableData.DownCam_mark_Col, out HTuple offx1, out HTuple offy1);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down1, 1500.0, 2048.0, out HTuple offx2, out HTuple offy2);
                //double x_current = plcSend.XPos;
                //double y_current = plcSend.YPos;
                double x_current = Xxx;
                double y_current = Yyy;
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
                plcRecv.XPos = cal_X + SystemPrara.Instance.Nozzle1_TrayDUT_CompensateX;//+ rotateOffsetX;
                plcRecv.YPos = cal_Y + SystemPrara.Instance.Nozzle1_TrayDUT_CompensateY;//+ rotateOffsetY;
                plcRecv.RPos = cal_R + SystemPrara.Instance.Nozzle1_TrayDUT_CompensateR;
                #endregion 计算偏移和角度

                Flow.Log("TrayDutPos success");
                plcRecv.Result = 3;
                Plc.SendMessage(handler, plcRecv);
                HXLDCont Cross = new HXLDCont();
                HOperatorSet.TupleRad(locationResult.Phi, out HTuple phi);
                Cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, phi);

                //显示结果到FormMain
                Flow.Windlist[CameraID].obj = Cross;
                Flow.Windlist[CameraID].obj = roi;
                Flow.Windlist[CameraID].obj = locationResult.SmallestRec2Xld;
                UCMain.Instance.ShowObject(CameraID, Cross);
                UCMain.Instance.ShowObject(CameraID, locationResult.SmallestRec2Xld);
                HOperatorSet.DumpWindowImage(out HObject dumpimg, Flow.Windlist[CameraID].hwind.HalconWindow);
                SaveDumpImage("", "N1GetTrayDut_OK", dumpimg, false);
                SaveOriginalImage("", "N1GetTrayDut_OK", image1, false);
                //SaveDumpImage("Nozzle1_GetTrayDut", dumpimg);
                dumpimg.Dispose();
                para_temp.Dispos();
                parameter.Dispos();
                locationResult.Dispos();
                image1.Dispose();
                return;
            }
            catch (Exception ex)
            {
                Flow.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 130;
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("TrayDutPos " + ex.Message + Environment.NewLine + ex.StackTrace);
                UCMain.Instance.AddInfo((CameraID + 1).ToString(), "Nozzle1GetTrayDut流程失败-TrayDutFront（）", true);
                UCMain.Instance.ShowMessage(CameraID, "Nozzle1GetTrayDut流程失败-TrayDutFront（）");
                Flow.Windlist[CameraID].Message = "Nozzle1GetTrayDut流程失败-TrayDutFront（）";
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
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotDutROI);
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_RightCamGetDUT);

                if (image == null)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 130;   // 重拍
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");                   
                    return;
                }
                //SaveImage("Nozzle1_TrayDutFront", image);

                UCMain.Instance.ShowImage(CameraID, image);
                Flow.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.Windlist[CameraID].obj = roi;
                //判断是否有料
                InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                {
                    //无料
                    plcRecv.Result = 0;   // 无DUT
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "无料", false);
                    UCMain.Instance.ShowMessage(CameraID, "无料！");
                    Flow.Windlist[CameraID].Message = "无料！";
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
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "Nozzle2GetTrayDut流程失败-TrayDutFront（）", true);
                    UCMain.Instance.ShowMessage(CameraID, "Nozzle2GetTrayDut流程失败-TrayDutFront（）");
                    Flow.Windlist[CameraID].Message = locationResult.ErrString;
                    Flow.Windlist[CameraID].OKNGlable = false;
                    //SaveImage("Nozzle1_TrayDutFront", image);
                    image.Dispose();
                    para_temp.Dispos();
                    locationResult.Dispos();
                    parameter.Dispos();
                    return;
                }

                //DUT定位

                #region 计算偏移和角度

                double CurrentAngle = plcSend.RPos;
                double cal_R = CurrentAngle - (locationResult.Phi - serializableData.UpCam2_MatrixRad);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up2, locationResult.findPoint.Column, locationResult.findPoint.Row, out HTuple transX, out HTuple transY);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                double offx = serializableData.XShot2 - serializableData.X2;
                double offy = serializableData.YShot2 - serializableData.Y2;
                //同轴光源上的mark到相机中心偏差
                GetMarkOffeset(out double MarkOffesetX, out double MarkOffesetY);
                double cal_X = transX.D + x_current - offx - MarkOffesetX;
                double cal_Y = transY.D + y_current - offy - MarkOffesetY;

                plcRecv.XPos = cal_X + SystemPrara.Instance.Nozzle1_TrayDUT_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara.Instance.Nozzle1_TrayDUT_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara.Instance.Nozzle1_TrayDUT_CompensateR;

                #endregion 计算偏移和角度

                Flow.Log("TrayDutPos success");
                plcRecv.Result = 3;   // 视觉OK可以取料
                Plc.SendMessage(handler, plcRecv);
                HXLDCont Cross = new HXLDCont();
                HOperatorSet.TupleRad(locationResult.Phi, out HTuple phi);
                Cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, phi);

                ///SaveOriginalImage("", "Nozzle1_TrayDutFront", image);
                //显示结果到FormMain
                Flow.Windlist[CameraID].obj = Cross;
                Flow.Windlist[CameraID].obj = locationResult.SmallestRec2Xld;
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
                Flow.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 130;  //视觉算法异常，定位异常
                
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("TrayDutPos " + ex.Message + Environment.NewLine + ex.StackTrace);
                UCMain.Instance.AddInfo((CameraID + 1).ToString(), "Nozzle2GetTrayDut流程失败-TrayDutFront（）", true);
                UCMain.Instance.ShowMessage(CameraID, "Nozzle2GetTrayDut流程失败-TrayDutFront（）");
                Flow.Windlist[CameraID].Message = "Nozzle2GetTrayDut流程失败-TrayDutFront（）";
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
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotROI);
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_LeftCamGetDUT);
                if (image == null)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                   
                    return;
                }

                ///SaveOriginalImage("","Nozzle1PutTrayDut", image);
                //SaveImage("Nozzle1_TrayDutFront", image);
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.Windlist[CameraID].obj = roi;
                //保存图像
                if (Utility.GetBitValue(plcSend.Func, 3))
                {
                    //PLC需要判断产品有无
                    if (Utility.GetBitValue(plcSend.Func, 0))
                    {
                        InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                        if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                        {
                            Flow.Windlist[CameraID].OKNGlable = false;
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
                        Flow.Windlist[CameraID].OKNGlable = false;
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
                    Flow.Log("Nozzle1PutTrayDut流程失败-ShotSlot（）");
                    Flow.Windlist[CameraID].OKNGlable = false;
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "Nozzle1PutTrayDut流程失败-ShotSlot（）", true);
                    UCMain.Instance.ShowMessage(CameraID, "Nozzle1PutTrayDut流程失败-ShotSlot（）");
                    Flow.Windlist[CameraID].Message = locationResult.ErrString;
                    image.Dispose();
                    parameter.Dispos();
                    locationResult.Dispos();
                    return;
                }
                //按照自定义的放料中心仿射变换
                HHomMat2D hv_HomMat2D = new HHomMat2D();
                if (locationResult.Phi < -0.79)
                {
                    locationResult.Phi = 3.14159 + locationResult.Phi;
                }
                hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.slot_rowCenter, ImagePara.Instance.slot_colCenter,
                    ImagePara.Instance.slot_angleCenter, locationResult.findPoint.Row, locationResult.findPoint.Column, locationResult.Phi);
                HTuple findrow = hv_HomMat2D.AffineTransPoint2d(ImagePara.Instance.slot_offe_rowCenter, ImagePara.Instance.slot_offe_colCenter, out HTuple findcol);
                locationResult.findPoint.Row = findrow;
                locationResult.findPoint.Column = findcol;

                #region 计算偏移和角度**

                //当前角度
                double CurrentAngle = plcSend.RPos;
                //下相机角度偏差
                double downcam_angle = down_phi - serializableData.mode_angle1;
                //上相机角度偏差
                double upcam_angle = locationResult.Phi - serializableData.UpCam1_MatrixRad;
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
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, locationResult.findPoint.Row,locationResult.findPoint.Column,  out HTuple transX_up, out HTuple transY_up);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                double offx = serializableData.XShot1 - serializableData.X1;
                double offy = serializableData.YShot1 - serializableData.Y1;
                //同轴光源上的mark到相机中心偏差
                GetMarkOffeset(out double MarkOffesetX, out double MarkOffesetY);
                //double cal_X = transX_up.D + x_current - offx + offx_down - MarkOffesetX;
                //double cal_Y = transY_up.D + y_current - offy + offy_down - MarkOffesetY;
                double cal_X = transX_up.D + x_current  + offx_down - (-1.49144);
                double cal_Y = transY_up.D + y_current  + offy_down - (78.9408);
                //(-1.49144); //(-1.082);
                //(78.9408);//78.4495;

                plcRecv.XPos = cal_X + SystemPrara.Instance.Nozzle1_Slot_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara.Instance.Nozzle1_Slot_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara.Instance.Nozzle1_Slot_CompensateR;
                down_row = 0;
                down_col = 0;
                down_phi = 0;

                #endregion 计算偏移和角度**
                plcRecv.Result = 1;
                Plc.SendMessage(handler, plcRecv);
 
                //显示结果

                HXLDCont cross = new HXLDCont();
                cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Phi);
                Flow.Windlist[CameraID].obj = cross;
                UCMain.Instance.ShowObject(CameraID, cross);
                parameter.Dispos();
                locationResult.Dispos();
                image.Dispose();
            }
            catch (Exception ex)
            {
                Flow.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 128;
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("TrayPlaceDutPos " + ex.Message + Environment.NewLine + ex.StackTrace);
                UCMain.Instance.AddInfo((CameraID + 1).ToString(), "Nozzle1PutTrayDut流程失败-ShotSlot（）", true);
                UCMain.Instance.ShowMessage(CameraID, "Nozzle1PutTrayDut流程失败-ShotSlot（）");
                Flow.Windlist[CameraID].Message = "Nozzle1PutTrayDut流程失败-ShotSlot（）";
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
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotROI);

                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_LeftCamPutDUT);
                if (image == null)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;  // 拍照失败，重拍
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }

                //var plcSend1 = (double[])handler.CmdParam.KeyValues[PLCParamNames.PLCSend].Value;
                //var plcRecv1 = new double[6];
                //plcRecv1[(int)EnumPLCRecv.XPos] = plcSend1[(int)EnumPLCSend.XPos];
                //plcRecv1[(int)EnumPLCRecv.YPos] = plcSend1[(int)EnumPLCSend.YPos];
                //plcRecv1[(int)EnumPLCRecv.ZPos] = plcSend1[(int)EnumPLCSend.ZPos];
                //plcRecv1[(int)EnumPLCRecv.RPos] = plcSend1[(int)EnumPLCSend.RPos];


                //plcRecv1[(int)EnumPLCRecv.Result] = (double)1;
                //handler.CmdParam.KeyValues[PLCParamNames.PLCRecv].Value = plcRecv;
                //handler.SendMessage(new ReceivedData()
                //{
                //    ModuleId = ModuleTypes.Handler.ToString(),
                //    Data = new MessageData() { Param = handler.CmdParam }
                //}, -2);

                //return;

                ///SaveOriginalImage("","Nozzle1PutTrayDut_OneCam", image);
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.Windlist[CameraID].obj = roi;
                //保存图像
                if (Utility.GetBitValue(plcSend.Func, 3))
                {
                    //PLC需要判断产品有无
                    if (Utility.GetBitValue(plcSend.Func, 0))
                    {
                        InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                        if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                        {
                            Flow.Windlist[CameraID].OKNGlable = false;
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
                        Flow.Windlist[CameraID].OKNGlable = false;
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
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "Nozzle1PutTrayDut_OneCam流程失败-ShotSlot（）", true);
                    UCMain.Instance.ShowMessage(CameraID, "Nozzle1PutTrayDut_OneCam流程失败-ShotSlot（）");
                    Flow.Windlist[CameraID].Message = locationResult.ErrString;
                    Flow.Windlist[CameraID].OKNGlable = false;
                    HOperatorSet.DumpWindowImage(out HObject dumpimg, Flow.Windlist[CameraID].hwind.HalconWindow);
                    SaveDumpImage("", "SameTray_Put_Error", dumpimg, false);
                    SaveOriginalImage("", "SameTray_Put_Error", image, false);
                    dumpimg.Dispose();
                    parameter.Dispos();
                    locationResult.Dispos();
                    image.Dispose();
                    return;
                }
                //按照自定义的放料中心仿射变换
                HHomMat2D hv_HomMat2D = new HHomMat2D();
                if (locationResult.Phi < -0.79)
                {
                    locationResult.Phi = 3.14159 + locationResult.Phi;
                }
                hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.slot_rowCenter, ImagePara.Instance.slot_colCenter,
                    ImagePara.Instance.slot_angleCenter, locationResult.findPoint.Row, locationResult.findPoint.Column, locationResult.Phi);
                HTuple findrow = hv_HomMat2D.AffineTransPoint2d(ImagePara.Instance.slot_offe_rowCenter, ImagePara.Instance.slot_offe_colCenter, out HTuple findcol);
                locationResult.findPoint.Row = findrow;
                locationResult.findPoint.Column = findcol;

                #region 计算偏移和角度

                double CurrentAngle = plcSend.RPos;
                HOperatorSet.TupleDeg((serializableData.DownCam1_MatrixRad - serializableData.UpCam1_MatrixRad + serializableData.mode_angle1), out HTuple upCameraNozzleAngle);
                double cal_R = 0 - (locationResult.Phi - upCameraNozzleAngle);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, locationResult.findPoint.Row, locationResult.findPoint.Column, out HTuple transX, out HTuple transY);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                //double offx = serializableData.XShot1 - serializableData.X1;
                //double offy = serializableData.YShot1 - serializableData.Y1;
                //同轴光源上的mark到相机中心偏差
                GetMarkOffeset(out double MarkOffesetX, out double MarkOffesetY);
                //double cal_X = transX.D + x_current - offx- MarkOffesetX;
                //double cal_Y = transY.D + y_current - offy-MarkOffesetY;
                double offsetX = AutoNormal_New.serializableData.xLeftOffset;
                double offsetY = AutoNormal_New.serializableData.yLeftOffset;
                //double cal_X = transX.D + x_current - (-1.082);
                //double cal_Y = transY.D + y_current - (78.4495);
                double cal_X = transX.D + x_current - offsetX;
                double cal_Y = transY.D + y_current - offsetY;
                plcRecv.XPos = cal_X + SystemPrara.Instance.Nozzle1_Slot_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara.Instance.Nozzle1_Slot_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara.Instance.Nozzle1_Slot_CompensateR;

                #endregion 计算偏移和角度
                plcRecv.Result = 1;
                Plc.SendMessage(handler, plcRecv);
                //显示结果
                HXLDCont cross = new HXLDCont();

                cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Phi);
                Flow.Windlist[CameraID].obj = cross;
                UCMain.Instance.ShowObject(CameraID, cross);
                HOperatorSet.DumpWindowImage(out HObject dumpimg1, Flow.Windlist[CameraID].hwind.HalconWindow);
                SaveDumpImage("", "SameTray_Put_OK", dumpimg1, false);
                SaveOriginalImage("", "SameTray_Put_OK", image, false);
                dumpimg1.Dispose();
                parameter.Dispos();
                locationResult.Dispos();
                image.Dispose();
            }
            catch (Exception ex)
            {
                Flow.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 128;
                Plc.SendMessage(handler, plcRecv);
                UCMain.Instance.AddInfo((CameraID + 1).ToString(), "Nozzle1PutTrayDut_OneCam流程失败-ShotSlot（）", true);
                UCMain.Instance.ShowMessage(CameraID, "Nozzle1PutTrayDut_OneCam流程失败-ShotSlot（）");
                Flow.Windlist[CameraID].Message = "Nozzle1PutTrayDut_OneCam流程失败-ShotSlot（）";
                Flow.Log("Nozzle1PutTrayDut_OneCam流程失败-ShotSlot（） " + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// 左相机 检测DUT是否在槽内，GRR模式及same tray等测试模式下，歪斜判定为NG
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void CheckPutDut_left(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);
            //  var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 0;
            using (HDevDisposeHelper hp = new HDevDisposeHelper())
            {
                try
                {
                    HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotROI);
                    var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_LeftCamGetDUT);
                    if (image == null)
                    {
                        Flow.Windlist[CameraID].OKNGlable = false;
                        plcRecv.Result = 128;
                        plcRecv.BinValue = 0;
                        Plc.SendMessage(handler, plcRecv);
                        Flow.Log("采图失败!");
                        
                        return;
                    }

                    ///SaveOriginalImage("","CheckPutDut_left", image);
                    UCMain.Instance.ShowImage(CameraID, image);
                    Flow.Windlist[CameraID].image = image;
                    UCMain.Instance.ShowObject(CameraID, roi);
                    Flow.Windlist[CameraID].obj = roi;
                    //保存图像               
                    //SaveImage("Nozzle1_Slot", image);
                    //PLC需要判断产品有无               
                    InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                    if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                    {
                        //无料则失败
                        //SaveImage("Nozzle1_Slot", image);
                        Flow.Windlist[CameraID].OKNGlable = false;
                        plcRecv.Result = 0;
                    }
                    else
                    {
                        //有料则成功
                        Flow.Windlist[CameraID].OKNGlable = true;
                        plcRecv.Result = 3;
                    }
                    
                    Plc.SendMessage(handler, plcRecv);
                    para_temp.Dispos();
                    image.Dispose();
                }
                catch (Exception ex)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;
                    Plc.SendMessage(handler, plcRecv);
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "CheckPutDut_left流程失败", true);
                    UCMain.Instance.ShowMessage(CameraID, "CheckPutDut_left流程失败");
                    Flow.Windlist[CameraID].Message = "CheckPutDut_left流程失败";
                    Flow.Log("CheckPutDut_left " + ex.Message + Environment.NewLine + ex.StackTrace);
                }


            }
            
        }

        /// <summary>
        /// 左相机 检测DUT是否在槽内，挑料和正常模式下，产品放Tray 只要有料就判定为OK
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void CheckPutDut_left_plus(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);

            //  var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 0;
            using (HDevDisposeHelper hp = new HDevDisposeHelper())
            
            {
                try
                {
                    HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotROI);
                    var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_LeftCamGetDUT);
                    if (image == null)
                    {
                        Flow.Windlist[CameraID].OKNGlable = false;
                        plcRecv.Result = 128;
                        plcRecv.BinValue = 0;
                        Plc.SendMessage(handler, plcRecv);
                        Flow.Log("采图失败!");                      
                        return;
                    }

                    //保存图像               
                    ///SaveOriginalImage("","CheckPutDut_left_plus", image);
                    UCMain.Instance.ShowImage(CameraID, image);
                    Flow.Windlist[CameraID].image = image;
                    UCMain.Instance.ShowObject(CameraID, roi);
                    Flow.Windlist[CameraID].obj = roi;

                    //PLC需要判断产品有无               
                    InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                    //if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                    if (!AutoNormal_New.ImageProcess.IsTrayDutHaveOrNot_Normal(para_temp, out HObject yy))
                    {
                        //无料则失败
                        //SaveImage("Nozzle1_Slot", image);
                        SaveOriginalImage("", "N1CheckTrayDut_NoDut", image, false);
                        Flow.Windlist[CameraID].OKNGlable = false;
                        plcRecv.Result = 0;
                    }
                    else
                    {
                        //有料则成功
                        SaveOriginalImage("", "N1CheckTrayDut_OK", image, false);
                        Flow.Windlist[CameraID].OKNGlable = true;
                        plcRecv.Result = 3;
                    }
                    Plc.SendMessage(handler, plcRecv);
                    para_temp.Dispos();
                    image.Dispose();
                }
                catch (Exception ex)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;
                    Plc.SendMessage(handler, plcRecv);
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "CheckPutDut_left_plus流程失败-IsTrayDutHaveOrNot_Normal（）", true);
                    UCMain.Instance.ShowMessage(CameraID, "CheckPutDut_left_plus流程失败-IsTrayDutHaveOrNot_Normal（）");
                    Flow.Windlist[CameraID].Message = "CheckPutDut_left_plus流程失败-IsTrayDutHaveOrNot_Normal（）";
                    Flow.Log("CheckPutDut_left_plus " + ex.Message + Environment.NewLine + ex.StackTrace);
                }


            }
            
        }
        public static void CheckPutDut_right(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);

            //  var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 1;
            using (HDevDisposeHelper hp = new HDevDisposeHelper()) 
            {
                try
                {
                    HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotROI);
                    var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_RightCamGetDUT);
                    if (image == null)
                    {
                        Flow.Windlist[CameraID].OKNGlable = false;
                        plcRecv.Result = 128;  // 定位失败，需要重拍
                        plcRecv.BinValue = 0;
                        Plc.SendMessage(handler, plcRecv);
                        Flow.Log("采图失败!");                      
                        return;
                    }
                    //保存图像               

                    ////SaveOriginalImage("","CheckPutDut_right", image);
                    UCMain.Instance.ShowImage(CameraID, image);
                    Flow.Windlist[CameraID].image = image;
                    UCMain.Instance.ShowObject(CameraID, roi);
                    Flow.Windlist[CameraID].obj = roi;

                    //PLC需要判断产品有无               
                    InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                    if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                    {
                        //无料则失败
                        //SaveImage("Nozzle1_Slot", image);
                        Flow.Windlist[CameraID].OKNGlable = false;
                        plcRecv.Result = 0;  // 无DUT
                    }
                    else
                    {
                        //有料则成功
                        Flow.Windlist[CameraID].OKNGlable = true;
                        plcRecv.Result = 3;  // 有DUT
                    }
                    Plc.SendMessage(handler, plcRecv);
                    image.Dispose();
                    para_temp.Dispos();
                }
                catch (Exception ex)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;   // 出现错处  理解为定位异常
                    Plc.SendMessage(handler, plcRecv);
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "CheckPutDut_right流程失败-SlotDetect（）", true);
                    UCMain.Instance.ShowMessage(CameraID, "CheckPutDut_right流程失败-SlotDetect（）");
                    Flow.Windlist[CameraID].Message = "CheckPutDut_right流程失败-SlotDetect（）";
                    Flow.Log("CheckPutDut_right " + ex.Message + Environment.NewLine + ex.StackTrace);
                }

            }
            
        }
        /// <summary>
        /// 右相机 检测DUT是否在槽内，挑料和正常模式下，产品放Tray 只要有料就判定为OK
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void CheckPutDut_right_plus(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);

            //  var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 1;
            using (HDevDisposeHelper hp = new HDevDisposeHelper())
            
            {
                try
                {
                    HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotROI);
                    var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_RightCamGetDUT);
                    if (image == null)
                    {
                        Flow.Windlist[CameraID].OKNGlable = false;
                        plcRecv.Result = 128;  // 定位失败，需要重拍
                        plcRecv.BinValue = 0;
                        Plc.SendMessage(handler, plcRecv);
                        Flow.Log("采图失败!");
                        
                        return;
                    }

                    //保存图像               

                    ///SaveOriginalImage("","CheckPutDut_right_plus", image);
                    UCMain.Instance.ShowImage(CameraID, image);
                    Flow.Windlist[CameraID].image = image;
                    UCMain.Instance.ShowObject(CameraID, roi);
                    Flow.Windlist[CameraID].obj = roi;

                    //PLC需要判断产品有无               
                    InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                    //if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                    if (!AutoNormal_New.ImageProcess.IsTrayDutHaveOrNot_Normal(para_temp, out HObject yy))
                    {
                        //无料则失败
                        SaveOriginalImage("", "N2CheckTrayDut_NoDut", image, false);
                        Flow.Windlist[CameraID].OKNGlable = false;
                        plcRecv.Result = 0;  // 无DUT
                    }
                    else
                    {
                        //有料则成功
                        SaveOriginalImage("", "N2CheckTrayDut_OK", image, false);
                        Flow.Windlist[CameraID].OKNGlable = true;
                        plcRecv.Result = 3;  // 有DUT
                    }
                    Plc.SendMessage(handler, plcRecv);
                    image.Dispose();
                    para_temp.Dispos();
                }
                catch (Exception ex)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;   // 出现错处  理解为定位异常
                    Plc.SendMessage(handler, plcRecv);
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "CheckPutDut_right_plus流程失败-IsTrayDutHaveOrNot_Normal（）", true);
                    UCMain.Instance.ShowMessage(CameraID, "CheckPutDut_right_plus流程失败-IsTrayDutHaveOrNot_Normal（）");
                    Flow.Windlist[CameraID].Message = "CheckPutDut_right_plus流程失败-IsTrayDutHaveOrNot_Normal（）";
                    Flow.Log("CheckPutDut_right_plus " + ex.Message + Environment.NewLine + ex.StackTrace);
                }


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
            int CameraID = 0;
            try
            {
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SocketMarkROI);
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_LeftCamPutSocket);
                if (image == null)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                ///SaveOriginalImage("","Nozzle1_SocketMark" + (ImageProcessBase.CurrentSoket + 1).ToString(), image);
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.Windlist[CameraID].obj = roi;


                string DutSN = "";
                if (Poc2Auto.Model.Overall.ScanResult != null)
                {
                    DutSN = Poc2Auto.Model.Overall.ScanResult;
                }

                //保存图像
                if (Utility.GetBitValue(plcSend.Func, 3))
                {

                    //SaveImage("Nozzle1_PutSocketDut(检查)", image);
                    //PLC需要判断产品有无
                    if (Utility.GetBitValue(plcSend.Func, 0))
                    {
                        int res = AutoNormal_New.ImageProcess.SocketDetect(image, out HObject yy);
                        if (res==3)
                        {

                            //有料是对的
                            SaveOriginalImage(DutSN, "N1CheckDUT_OK", image, true);
                            plcRecv.Result = 3;
                            Flow.Windlist[CameraID].OKNGlable = true;
                        }
                        else if (res == 0)
                        {
                            //料不见了

                            SaveOriginalImage(DutSN, "N1CheckDUT_Disappear", image, true);
                            plcRecv.Result = 0;
                            UCMain.Instance.AddInfo((CameraID + 1).ToString(), "无料", false);
                            UCMain.Instance.ShowMessage(CameraID, "无料");
                            Flow.Windlist[CameraID].Message = "无料!";
                            Flow.Windlist[CameraID].OKNGlable = false;
                            Plc.SendMessage(handler, plcRecv);
                            image.Dispose();
                            return;
                        }
                        else
                        {
                            plcRecv.Result = 1;
                            SaveOriginalImage(DutSN, "N1CheckDUT_Skew", image, true);
                            UCMain.Instance.AddInfo((CameraID + 1).ToString(), "有料，但歪斜！", false);
                            UCMain.Instance.ShowMessage(CameraID, "有料，但歪斜！");
                            Flow.Windlist[CameraID].Message = "有料，但歪斜！!";
                            Flow.Windlist[CameraID].OKNGlable = false;
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
                    //SaveImage("Nozzle1_SocketMark" + (ImageProcessBase.CurrentSoket + 1).ToString(), image);
                    
                    if (AutoNormal_New.ImageProcess.SocketDetect(image, out HObject yy)==3|| AutoNormal_New.ImageProcess.SocketDetect(image, out HObject xxx) == 1)
                    {
                        //有料则不能去放
                        string SN = "";
                        if (null != StationManager.Stations[StationName.PNP].SocketGroup.Sockets[0, 0].Dut?.Barcode)
                        {
                            SN = StationManager.Stations[StationName.PNP].SocketGroup.Sockets[0, 0].Dut?.Barcode;
                        }

                        SaveOriginalImage(SN, "N1PutSocket_HaveDut", image, true);
                        plcRecv.Result = 2;
                        Flow.Windlist[CameraID].OKNGlable = true;
                        Plc.SendMessage(handler, plcRecv);
                        image.Dispose();
                        return;
                    }
                }

                //图像处理
                InputPara parameter = new InputPara(image, roi, null, 0);

                //寻找Socket Mark点 ，三种方法处理  
                ImageProcess.FindSocketMarkFirst(image, out HTuple row1, out HTuple col1, out HTuple phi1, out HObject mark1);
                if (row1.Length <= 0)
                {
                    ImageProcess.FindSocketMarkSecond(image, out row1, out col1, out phi1, out mark1);
                    if(row1.Length <= 0)
                    {
                        ImageProcess.FindSocketMarkThird(image, out row1, out col1, out phi1, out mark1);
                    }
                }
                mark1.Dispose();
                if (row1.Length > 0)
                {
                    parameter = new InputPara(image, image.GetDomain(), null, 0);
                    ///检查推块位置是否到位
                    bool result = ImageProcess.BlockDetect(row1, col1, parameter, out int distance, out HObject arrow);
                    if (!result)
                    {
                        plcRecv.RPos = 0;
                        plcRecv.XPos = plcSend.XPos + para.CompensateX;
                        plcRecv.YPos = plcSend.YPos + para.CompensateY;
                        // 推块太短
                        plcRecv.Result = 0;
                        Plc.SendMessage(handler, plcRecv);
                        UCMain.Instance.AddInfo((CameraID + 1).ToString(), "推块位置太短！", true);
                        UCMain.Instance.ShowMessage(CameraID, "推块位置太短！");
                        Flow.Windlist[CameraID].Message = "推块位置太短！";
                        Flow.Windlist[CameraID].OKNGlable = false;
                        Flow.Windlist[CameraID].obj = arrow;
                        HOperatorSet.DumpWindowImage(out HObject dumpimg2, Flow.Windlist[CameraID].hwind.HalconWindow);
                        //SaveDumpImage("Socket_ShortBlock", dumpimg2);
                        SaveDumpImage("", "Socket_ShortBlock", dumpimg2, true);
                        SaveOriginalImage("", "Socket_ShortBlock", image, true);
                        dumpimg2.Dispose();
                        arrow.Dispose();
                        image.Dispose();
                        parameter.Dispos();
                        return;
                    }
                    UCMain.Instance.ShowMessage(CameraID, "BLOCK:"+ distance.ToString());
                    Flow.Windlist[CameraID].Message = "BLOCK:" + distance.ToString();
                    Flow.Windlist[CameraID].obj = arrow;
                    UCMain.Instance.ShowObject(CameraID, arrow);
                    arrow.Dispose();
                }
                //正常处理
                OutPutResult locationResult = AutoNormal_New.ImageProcess.SocketMark(parameter, out HObject mark);
                Flow.Windlist[CameraID].obj = mark;
                UCMain.Instance.ShowObject(CameraID, mark);
                if (locationResult.IsRunOk != true)
                {
                    SaveOriginalImage(DutSN, "N1PutSocket_MarkNG", image, true);
                    plcRecv.RPos = 0;
                    plcRecv.XPos = plcSend.XPos + para.CompensateX;
                    plcRecv.YPos = plcSend.YPos + para.CompensateY;
                    plcRecv.Result = 128;
                    Plc.SendMessage(handler, plcRecv);
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "定位失败", true);
                    UCMain.Instance.ShowMessage(CameraID, "定位失败");
                    Flow.Windlist[CameraID].Message = locationResult.ErrString;
                    Flow.Windlist[CameraID].OKNGlable = false;
                    locationResult.Dispos();
                    parameter.Dispos();
                    return;
                }

                #region 计算偏移和角度**
                //当前角度
                double CurrentAngle = plcSend.RPos;
                HOperatorSet.TupleRad(CurrentAngle, out HTuple CurrentRad);
                var MatrixRad = serializableData.DownCam1_MatrixRad - serializableData.UpCam1_MatrixRad;
                //下相机角度偏差二次定位角度和模板角度差
                double downcam_rad = down_phi ;
                //上相机角度偏差 
                double upcam_rad = locationResult.Phi;
                //当前拍照角度加上偏差值
                double offeR = -upcam_rad - downcam_rad+ MatrixRad;
                double cal_Phi = CurrentRad - offeR;
                HOperatorSet.TupleDeg(cal_Phi, out HTuple cal_R);
                //物料中心按照旋转中心旋转回去
                HOperatorSet.HomMat2dIdentity(out HTuple homate);
                HOperatorSet.HomMat2dRotate(homate, -cal_Phi, serializableData.Rotate_Center_Row1, serializableData.Rotate_Center_Col1, out HTuple homate_rotate);
                HOperatorSet.AffineTransPoint2d(homate_rotate, down_row, down_col, out HTuple trans_findrow, out HTuple trans_findcolum);
                //像素点转换成实际的PLC点位置
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down1, trans_findrow, trans_findcolum, out HTuple transX, out HTuple transY);
                //模板位置
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down1, serializableData.mode_row1, serializableData.mode_col1, out HTuple transX_mode, out HTuple transY_mode);
                //下相机的偏差
                double offx_down = transX_mode.D - transX.D;
                double offy_down = transY_mode.D - transY.D;
                //上相机偏差
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1,
                    locationResult.findPoint.Row,locationResult.findPoint.Column, out HTuple transX_up, out HTuple transY_up);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;


                //double offxx = serializableData.XShot1 - serializableData.X1;
                //double offyy = serializableData.YShot1 - serializableData.Y1;


                ////同轴光源上的mark到相机中心偏差
                // GetMarkOffeset(out double MarkOffesetX, out double MarkOffesetY);
                //double cal_Xx = transX_up.D + x_current - offxx + offx_down - MarkOffesetX;
                //double cal_Yy = transY_up.D + y_current - offyy + offy_down - MarkOffesetY;

                double offx = AutoNormal_New.serializableData.xLeftOffset;
                double offy = AutoNormal_New.serializableData.yLeftOffset;
                //double cal_X = transX_up.D + x_current + offx_down - (-1.49144); 
                //double cal_Y = transY_up.D + y_current + offy_down - (78.9408);
                double cal_X = transX_up.D + x_current + offx_down - offx;
                double cal_Y = transY_up.D + y_current + offy_down - offy;

                plcRecv.XPos = cal_X + SystemPrara.Instance.Nozzle1_Socket_Put_DUT_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara.Instance.Nozzle1_Socket_Put_DUT_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara.Instance.Nozzle1_Socket_Put_DUT_CompensateR;

                #endregion 计算偏移和角度**
                plcRecv.Result = 1;
                Plc.SendMessage(handler, plcRecv);
                //显示结果
                HXLDCont cross = new HXLDCont();
                cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Phi);
                Flow.Windlist[CameraID].obj = cross;
                UCMain.Instance.ShowObject(CameraID, cross);
                HOperatorSet.DumpWindowImage(out HObject dumpimg3, Flow.Windlist[CameraID].hwind.HalconWindow);
                SaveDumpImage(DutSN, "N1PutSocket_MarkOK" + (ImageProcessBase.CurrentSoket + 1).ToString(), dumpimg3, false);
                SaveOriginalImage(DutSN, "N1PutSocket_MarkOK", image, true);
                //SaveDumpImage("Nozzle1_SocketMark" + (ImageProcessBase.CurrentSoket + 1).ToString(), dumpimg3);
                dumpimg3.Dispose();
                image.Dispose();
                locationResult.Dispos();
                parameter.Dispos();
                return;
            }
            catch (Exception ex)
            {
                Flow.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 128;
                UCMain.Instance.AddInfo((CameraID + 1).ToString(), "Nozzle1PutSocketDut流程错误", true);
                UCMain.Instance.ShowMessage(CameraID, "Nozzle1PutSocketDut流程错误");
                Flow.Windlist[CameraID].Message = "Nozzle1PutSocketDut流程错误";
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("Nozzle1PutSocketDut " + ex.Message + Environment.NewLine + ex.StackTrace);
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
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SocketMarkROI);
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_LeftCamCheckSocket);
                if (image == null)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;   // 采集图像失败， 理解为 定位失败，重新拍照
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                //保存图像
                string DutSN = "";
                if ( Poc2Auto.Model.Overall.ScanResult!= null)
                {
                    DutSN = Poc2Auto.Model.Overall.ScanResult;
                }
               // SaveOriginalImage(DutSN, "CheckPutSocketDut", image,true);
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.Windlist[CameraID].obj = roi;

                if (AutoNormal_New.ImageProcess.SocketDetect(image, out HObject yy)==3)
                {
                    //有料是对的
                    SaveOriginalImage(DutSN, "CheckSocketDut_HaveDUT", image, true);
                    plcRecv.Result = 3;  // 有DUT
                    Flow.Windlist[CameraID].OKNGlable = true;
                    HOperatorSet.DumpWindowImage(out HObject dumpimg1, Flow.Windlist[CameraID].hwind.HalconWindow);
                    SaveDumpImage(DutSN, "CheckSocketDut_HaveDUT", dumpimg1, true);
                    //SaveDumpImage("Socket_HaveDUT", dumpimg1);
                    dumpimg1.Dispose();
                }
                else if (AutoNormal_New.ImageProcess.SocketDetect(image, out HObject xx) == 0)
                {
                    //料不见了
                    plcRecv.Result = 0;  // 无DUT
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "无料", false);
                    UCMain.Instance.ShowMessage(CameraID, "无料");
                    Flow.Windlist[CameraID].Message = "无料!";
                    Flow.Windlist[CameraID].OKNGlable = false;
                    HOperatorSet.DumpWindowImage(out HObject dumpimg1, Flow.Windlist[CameraID].hwind.HalconWindow);
                    SaveDumpImage(DutSN, "CheckSocketDut_NoDUT", dumpimg1, true);
                    SaveOriginalImage(DutSN, "CheckSocketDut_NoDUT", image, true);
                    //SaveDumpImage("Socket_NotDUT", dumpimg1);
                    dumpimg1.Dispose();
                }
                else
                {
                    //料放歪了，弹窗让操作人员确认
                    plcRecv.Result = 1;  // DUT歪斜
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "DUT歪斜", false);
                    UCMain.Instance.ShowMessage(CameraID, "DUT歪斜");
                    Flow.Windlist[CameraID].Message = "DUT歪斜!";
                    Flow.Windlist[CameraID].OKNGlable = false;
                    HOperatorSet.DumpWindowImage(out HObject dumpimg1, Flow.Windlist[CameraID].hwind.HalconWindow);
                    SaveDumpImage(DutSN, "CheckSocketDut_SkewDUT", dumpimg1, true);
                    SaveOriginalImage(DutSN, "CheckSocketDut_SkewDUT", image, true);
                    dumpimg1.Dispose();

                }


                Plc.SendMessage(handler, plcRecv);
                image.Dispose();

            }
            catch (Exception ex)
            {
                Flow.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 128; // 算法异常，定位失败。重拍
                UCMain.Instance.AddInfo((CameraID + 1).ToString(), "CheckPutSocketDut流程错误-SocketDetect（）", true);
                UCMain.Instance.ShowMessage(CameraID, "CheckPutSocketDut流程错误-SocketDetect（）");
                Flow.Windlist[CameraID].Message = "CheckPutSocketDut流程错误-SocketDetect（）";
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("CheckPutSocketDut " + ex.Message + Environment.NewLine + ex.StackTrace);
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
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SocketMarkROI);
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_RightCamCheckSocket);
                if (image == null)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.Windlist[CameraID].obj = roi;
                //保存图像
                //SaveImage("Nozzle1_PutSocketDut(检查)", image);
                
                if (AutoNormal_New.ImageProcess.SocketDetect(image, out HObject yy)==3)
                {
                    //有料是错的 代表没有被取起来
                    //SaveImage("Nozzle1_PutSocketDut(检查)", image);
                    plcRecv.Result = 3;
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "料未取起来", false);
                    UCMain.Instance.ShowMessage(CameraID, "料未取起来");
                    Flow.Windlist[CameraID].Message = "料未取起来!";
                    Flow.Windlist[CameraID].OKNGlable = false;
                }
                else
                {
                    //空的槽代表料被取走了 对的
                    plcRecv.Result = 0;
                    Flow.Windlist[CameraID].OKNGlable = true;
                }
                Plc.SendMessage(handler, plcRecv);
                image.Dispose();

            }
            catch (Exception ex)
            {
                Flow.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 128;
                UCMain.Instance.AddInfo((CameraID + 1).ToString(), "CheckGetSocketDut流程错误-SocketDetect（）", true);
                UCMain.Instance.ShowMessage(CameraID, "CheckGetSocketDut流程错误-SocketDetect（）");
                Flow.Windlist[CameraID].Message = "CheckGetSocketDut流程错误-SocketDetect（）";
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("CheckGetSocketDut " + ex.Message + Environment.NewLine + ex.StackTrace);
                return;
            }
        }

        /// <summary>
        /// 吸嘴2从Socket取料
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void Nozzle2GetSocketDut(MessageHandler handler, AutoNormalPara para)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);
            //  var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];

            int CameraID = 1;
            try
            {
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SocketDutROI);
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_RightCamGetSocket);
                //image = new HImage(@"C:\Users\Administrator.DESKTOP-KDKC337\Desktop\10087.bmp");
                if (image == null)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 130;   // 拍照错误，理解为 定位异常
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);                   
                    Flow.Log("采图失败!");
                    return;
                }


                string DutSN = "";
                if (null!= StationManager.Stations[StationName.PNP].SocketGroup.Sockets[0, 0].Dut?.Barcode)
                {
                    DutSN = StationManager.Stations[StationName.PNP].SocketGroup.Sockets[0, 0].Dut?.Barcode;
                }
                ///SaveOriginalImage(DutSN,"Nozzle2GetSocketDut", image);
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.Windlist[CameraID].obj = roi;             
                if (AutoNormal_New.ImageProcess.SocketDetect(image, out HObject yy)==0|| AutoNormal_New.ImageProcess.SocketDetect(image, out HObject xxx) == 1)
                {
                    SaveOriginalImage(DutSN, "N2GetSocketDut_NoDUT", image,true);
                    //无料 则不能再取料  失败
                    plcRecv.Result = 0; //没有DUT 不能去取
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), "无料", false);
                    UCMain.Instance.ShowMessage(CameraID, "无料");
                    Flow.Windlist[CameraID].Message = "无料!";
                    Flow.Windlist[CameraID].OKNGlable = false;
                    HOperatorSet.DumpWindowImage(out HObject dumpimg1, Flow.Windlist[CameraID].hwind.HalconWindow);
                    SaveDumpImage(DutSN, "N2GetSocketDut_NoDUT", dumpimg1, true);
                    //SaveDumpImage("Nozzle2GetSocketDut", dumpimg1);
                    dumpimg1.Dispose();
                    Plc.SendMessage(handler, plcRecv);
                    image.Dispose();
                    return;
                }

                //图像处理
                InputPara parameter = new InputPara(image, image.GetDomain(), null, 0);
                OutPutResult locationResult = AutoNormal_New.ImageProcess.SocketDutFront(parameter);
                //定位是否成功判断
                if (!locationResult.IsRunOk)
                {
                    SaveOriginalImage(DutSN, "N2GetSocketDut_Error", image, true);
                    plcRecv.Result = 130;  // 有 DUT 定位失败
                    Plc.SendMessage(handler, plcRecv);
                    UCMain.Instance.AddInfo((CameraID + 1).ToString(), locationResult.ErrString, true);
                    UCMain.Instance.ShowMessage(CameraID, locationResult.ErrString);
                    Flow.Windlist[CameraID].Message = locationResult.ErrString;
                    Flow.Windlist[CameraID].OKNGlable = false;
                    image.Dispose();
                    locationResult.Dispos();
                    parameter.Dispos();
                    return;
                }
                HXLDCont cross = new HXLDCont();
                cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Phi);
                Flow.Windlist[CameraID].obj = cross;
                UCMain.Instance.ShowObject(CameraID, cross);
                //按照自定义的取料中心仿射变换
                HHomMat2D hv_HomMat2D = new HHomMat2D();
                if (locationResult.Phi < -0.79)
                {
                    locationResult.Phi = 3.14159 + locationResult.Phi;
                }

                #region 计算偏移和角度

                //角度
                double CurrentAngle = plcSend.RPos;
                HOperatorSet.TupleDeg((serializableData.DownCam2_MatrixRad - serializableData.UpCam2_MatrixRad + serializableData.mode_angle2), out HTuple upCameraNozzleAngle);                              
                double cal_R = 90-(CurrentAngle-locationResult.Phi+ upCameraNozzleAngle);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up2, locationResult.findPoint.Row,locationResult.findPoint.Column,  out HTuple transX, out HTuple transY);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                //同轴光源上的mark到相机中心偏差
                //double cal_X = transX.D + x_current - 1.032;
                //double cal_Y = transY.D + y_current - 78.211;
                double offx = serializableData.xRightOffset;
                double offy = serializableData.yRightOffset;
                double cal_X = transX.D + x_current - offx;
                double cal_Y = transY.D + y_current - offy;

                plcRecv.XPos = cal_X + SystemPrara.Instance.Nozzle2_Socket_Get_DUT_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara.Instance.Nozzle2_Socket_Get_DUT_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara.Instance.Nozzle2_Socket_Get_DUT_CompensateR;

                #endregion 计算偏移和角度

                plcRecv.Result = 3;  // 有DUT 视觉定位成功
                 Plc.SendMessage(handler, plcRecv);

                //显示结果
                cross = new HXLDCont();
                cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Phi);
                Flow.Windlist[CameraID].obj = cross;
                Flow.Windlist[CameraID].obj = locationResult.region;
                Flow.Windlist[CameraID].obj = locationResult.SmallestRec2Xld;
                UCMain.Instance.ShowObject(CameraID, cross);
                UCMain.Instance.ShowObject(CameraID, locationResult.region);
                UCMain.Instance.ShowObject(CameraID, locationResult.SmallestRec2Xld);
                HOperatorSet.DumpWindowImage(out HObject dumpimg2, Flow.Windlist[CameraID].hwind.HalconWindow);
                SaveOriginalImage(DutSN, "N2GetSocketDut_OK", image, true);
                SaveDumpImage(DutSN, "N2GetSocketDut_OK", dumpimg2, true);
                //SaveDumpImage("Nozzle2GetSocketDut", dumpimg2);
                dumpimg2.Dispose();
                locationResult.Dispos();
                parameter.Dispos();
                image.Dispose();
                return;
            }
            catch (Exception ex)
            {
                Flow.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 130;   // 视觉算法错误，重新拍照，定位失败
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("Nozzle2GetSocketDut " + ex.Message + Environment.NewLine + ex.StackTrace);
                UCMain.Instance.AddInfo((CameraID + 1).ToString(), "Nozzle2GetSocketDut流程错误", true);
                UCMain.Instance.ShowMessage(CameraID, "Nozzle2GetSocketDut流程错误");
                Flow.Windlist[CameraID].Message = "Nozzle2GetSocketDut流程错误";
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
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SlotROI);
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_RightCamPutDUT);
               // image = new HImage(@"C:\Users\Administrator.DESKTOP-KDKC337\Desktop\10085.bmp");
                if (image == null)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 128;  // 拍照
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                //保存图像

                string DutSN = "";
                if (null != StationManager.Stations[StationName.Unload].SocketGroup.Sockets[0, 0].Dut.Barcode)
                {
                    DutSN = StationManager.Stations[StationName.Unload].SocketGroup.Sockets[0, 0].Dut.Barcode;
                }
                else
                {
                    //无DUT信息  无码
                }


                ///SaveOriginalImage(DutSN,"Nozzle2PutTrayDut", image);
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.Windlist[CameraID].obj = roi;

                InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);

                if (Utility.GetBitValue(plcSend.Func, 3))
                {
                    //plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 0, true);//总结果OK

                    //PLC需要判断产品有无
                    if (Utility.GetBitValue(plcSend.Func, 0))
                    {
                        if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy1))
                        {
                            SaveOriginalImage(DutSN,"N2PutTayCheck_NoDut",image,false);
                            //无料 放的料不见了 报错
                            plcRecv.Result = 0;   // 无DUT
                            Flow.Windlist[CameraID].OKNGlable = false;
                            Plc.SendMessage(handler, plcRecv);
                            image.Dispose();
                            yy1.Dispose();
                            para_temp.Dispos();
                            return;
                        }
                        else
                        {
                            //有料  放的料OK 
                            SaveOriginalImage(DutSN, "N2PutTayCheck_OK", image, false);
                            plcRecv.Result = 3;   // 有DUT
                            Flow.Windlist[CameraID].OKNGlable = true;
                            Plc.SendMessage(handler, plcRecv);
                            image.Dispose();
                            para_temp.Dispos();
                            yy1.Dispose();
                            return;
                        }
                    }
                }

                if (AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                {
                    //有料则不能去放
                    SaveOriginalImage(DutSN, "N2PutTray_NotPut", image, false);
                    plcRecv.Result = 2;    //有料则不能去放
                    Flow.Windlist[CameraID].OKNGlable = false;
                    Plc.SendMessage(handler, plcRecv);
                    image.Dispose();
                    para_temp.Dispos();
                    return;
                }
                para_temp.Dispos();
                //图像处理
                InputPara parameter = new InputPara(image, roi, new HShapeModel(), 0);
                OutPutResult locationResult = AutoNormal_New.ImageProcess.ShotSlot(parameter);

                if (!locationResult.IsRunOk)
                {
                    SaveOriginalImage(DutSN, "N2PutTray_Error", image, false);
                    plcRecv.Result = 128;   // 视觉定位异常
                    Plc.SendMessage(handler, plcRecv);
                    UCMain.Instance.AddInfo(CameraID.ToString(), "Nozzle2PutTrayDut流程失败", true);
                    UCMain.Instance.ShowMessage(CameraID, "Nozzle2PutTrayDut流程失败");
                    Flow.Windlist[CameraID].Message = locationResult.ErrString;
                    Flow.Windlist[CameraID].OKNGlable = false;
                    image.Dispose();
                    locationResult.Dispos();
                    parameter.Dispos();
                    return;
                }
                //按照自定义的放料中心仿射变换
                HHomMat2D hv_HomMat2D = new HHomMat2D();
                if (locationResult.Phi < -0.79)
                {
                    locationResult.Phi = 3.14159 + locationResult.Phi;
                }
                hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.slot_rowCenter, ImagePara.Instance.slot_colCenter,
                    ImagePara.Instance.slot_angleCenter, locationResult.findPoint.Row, locationResult.findPoint.Column, locationResult.Phi);
                HTuple findrow = hv_HomMat2D.AffineTransPoint2d(ImagePara.Instance.slot_offe_rowCenter, ImagePara.Instance.slot_offe_colCenter, out HTuple findcol);
                locationResult.findPoint.Row = findrow;
                locationResult.findPoint.Column = findcol;

                #region 计算偏移和角度**

                double CurrentAngle = plcSend.RPos;
                HOperatorSet.TupleDeg((serializableData.DownCam2_MatrixRad - serializableData.UpCam2_MatrixRad + serializableData.mode_angle2 ), out HTuple upCameraNozzleAngle);
                double cal_R = CurrentAngle + (locationResult.Phi + upCameraNozzleAngle);
                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up2, locationResult.findPoint.Row,locationResult.findPoint.Column,  out HTuple transX, out HTuple transY);
                double x_current = plcSend.XPos;
                double y_current = plcSend.YPos;
                //同轴光源上的mark到相机中心偏差
                //double cal_X = transX.D + x_current - (1.004);
                //double cal_Y = transY.D + y_current - (79.663);

                double offx = serializableData.xRightOffset;
                double offy = serializableData.yRightOffset;
                double cal_X = transX.D + x_current - offx;
                double cal_Y = transY.D + y_current - offy;

                plcRecv.XPos = cal_X + SystemPrara.Instance.Nozzle2_Slot_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara.Instance.Nozzle2_Slot_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara.Instance.Nozzle2_Slot_CompensateR;

                #endregion 计算偏移和角度**
                // 视觉 OK  可以去放了
                plcRecv.Result = 1;
                Plc.SendMessage(handler, plcRecv);
                //显示结果
                HXLDCont cross = new HXLDCont();
                cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Phi);
                Flow.Windlist[CameraID].obj = cross;
                UCMain.Instance.ShowObject(CameraID, cross);
                HOperatorSet.DumpWindowImage(out HObject dumpimg2, Flow.Windlist[CameraID].hwind.HalconWindow);

                SaveDumpImage(DutSN, "N2PutTray_OK", dumpimg2, false);
                SaveOriginalImage(DutSN, "N2PutTray_OK", image, false);
                //SaveDumpImage("Nozzle2PutTrayDut", dumpimg2);
                dumpimg2.Dispose();
                image.Dispose();
                locationResult.Dispos();
                parameter.Dispos();
                return;
            }
            catch (Exception ex)
            {
                plcRecv.Result = 128;  // 视觉算法异常 重拍
                Plc.SendMessage(handler, plcRecv);
                UCMain.Instance.AddInfo(CameraID.ToString(), "Nozzle2PutTrayDut流程失败", true);
                UCMain.Instance.ShowMessage(CameraID, "Nozzle2PutTrayDut流程失败");
                Flow.Windlist[CameraID].Message = "Nozzle2PutTrayDut流程失败";
                Flow.Log("Nozzle2PutTrayDut " + ex.Message + Environment.NewLine + ex.StackTrace);
                Flow.Windlist[CameraID].OKNGlable = false;
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
                HRegion roi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.DutBackROI);
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_DownCamScan);
                if (image == null)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 130;  // 出现异常
                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    return;
                }
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.Windlist[CameraID].image = image;
                UCMain.Instance.ShowObject(CameraID, roi);
                Flow.Windlist[CameraID].obj = roi;
                //保存图像

                string DutSN = "";
                if (null!=Poc2Auto.Model.Overall.ScanResult)
                {
                    DutSN = Poc2Auto.Model.Overall.ScanResult;
                }

                SaveOriginalImage(DutSN,"SecondDut", image,false);
                //图像处理
                InputPara parameter = new InputPara(image, roi, SecondDutBackMode, 0);

                //OutPutResult locationResult = AutoNormal_New.ImageProcess.SecondDutBack_Circle(parameter, plcSend);
                OutPutResult locationResult = AutoNormal_New.ImageProcess.SecondDutBack(parameter, plcSend);
                //SecondDutBack()
                if (!locationResult.IsRunOk)
                {
                    //SaveImage("SecondDutBack", image);
                    // 有DUT 视觉定位失败
                    plcRecv.Result = 130;
                    UCMain.Instance.AddInfo(CameraID.ToString(), "SecondDut流程失败-SecondDutBack（）", true);
                    UCMain.Instance.ShowMessage(CameraID, "SecondDut流程失败-SecondDutBack（）");
                    Flow.Windlist[CameraID].Message = locationResult.ErrString;
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
                down_phi = locationResult.Phi;

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
                        plcRecv.RecvSocketID = 999;
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
                        if (RunModeMgr.RunMode == RunMode.AutoAudit)
                        {

                            if (Overall.AuditSN.ContainsKey(locationResult.DatacodeString))
                            {
                                plcRecv.RecvSocketID = Overall.AuditSN[locationResult.DatacodeString];
                            }
                            else
                            {
                                AlcSystem.Instance.ShowMsgBox($"audit.csv文件里面的所有DUTSN与当前扫码的DUTSN不匹配，请检查audit.csv文件", "Vision", icon: AlcMsgBoxIcon.Error);
                                AlcSystem.Instance.ButtonClickRequire(SYSTEM_EVENT.Stop);
                                EventCenter.ProcessInfo?.Invoke($"audit.csv文件里面的所有DUTSN与当前扫码的DUTSN不匹配，请检查audit.csv文件", ErrorLevel.FATAL);
                                return;
                            }
                            
                        }
                        Overall.ScanResult = locationResult.DatacodeString;
                        EventCenter.VisionScan?.Invoke();
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
                    HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down1, locationResult.findPoint.Row,locationResult.findPoint.Column,  out HTuple transX, out HTuple transY);
                    string data = DateTime.Now.ToLocalTime().ToString() + "," + transX.D.ToString("f3") + "," + transY.D.ToString("f3");
                    SaveData.WriteCsv("DoeSlipData.csv", data);
                }


                Plc.SendMessage(handler, plcRecv);
                //显示结果
                UCMain.Instance.ShowObject(CameraID, locationResult.SmallestRec2Xld);
                UCMain.Instance.ShowObject(CameraID, locationResult.region);
                Flow.Windlist[CameraID].obj = locationResult.SmallestRec2Xld;
                Flow.Windlist[CameraID].obj = locationResult.region;
                HXLDCont CrossContour = new HXLDCont();
                CrossContour.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 100, locationResult.Phi);
                UCMain.Instance.ShowObject(CameraID, CrossContour);
                Flow.Windlist[CameraID].obj = CrossContour;
                UCMain.Instance.AddInfo(CameraID.ToString(), "二维码:" + Overall.ScanResult, false);
                UCMain.Instance.ShowMessage(CameraID, "二维码:" + Overall.ScanResult);
                Flow.Windlist[CameraID].Message = "二维码:" + Overall.ScanResult;
                image.Dispose();
                locationResult.Dispos();
                parameter.Dispos();
            }
            catch (Exception ex)
            {
                Flow.Windlist[CameraID].OKNGlable = false;
                // 有DUT 算法处理过程出现异常
                plcRecv.Result = 130;
                plcRecv.BinValue = 0;
                Plc.SendMessage(handler, plcRecv);
                UCMain.Instance.AddInfo(CameraID.ToString(), "SecondDut流程失败-SecondDutBack（）", false);
                UCMain.Instance.ShowMessage(CameraID, "SecondDut流程失败-SecondDutBack（）");
                Flow.Windlist[CameraID].Message = "SecondDut流程失败-SecondDutBack（）";
                Flow.Log("SecondDut" + ex.Message + Environment.NewLine + ex.StackTrace);
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

                image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.Exposure_DownCamScan);
                if (image == null)
                {
                    Flow.Windlist[CameraID].OKNGlable = false;
                    plcRecv.Result = 130;    //总结果

                    plcRecv.BinValue = 0;
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("采图失败!");
                    
                    return;
                }
                UCMain.Instance.ShowImage(CameraID, image);
                Flow.Windlist[CameraID].image = image;

                // 保存图像
                //SaveImage("SecondDutBack", image);
                // 图像处理
                if (DutBackgroundDetect(image))
                {
                    // 有料，
                    SaveImage("SecondDutBack", image);
                    Flow.Windlist[CameraID].OKNGlable = true;
                    plcRecv.Result = 3;    //有料
                    // 显示
                    UCMain.Instance.AddInfo(CameraID.ToString(), "有料", true);
                    UCMain.Instance.ShowMessage(CameraID, "有料");
                    Flow.Windlist[CameraID].Message = "有料";

                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("有料");

                    // 释放
                    image.Dispose();

                    return;
                }
                else
                {
                    // 无料，
                    Flow.Windlist[CameraID].OKNGlable = true;
                    plcRecv.Result = 0;    //无料


                    // 显示
                    UCMain.Instance.AddInfo(CameraID.ToString(), "无料", true);
                    UCMain.Instance.ShowMessage(CameraID, "无料");
                    Flow.Windlist[CameraID].Message = "无料";

                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("无料");

                    // 释放

                    image.Dispose();
                    return;
                }
            }

            catch (Exception ex)
            {
                Flow.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = 130;   // 出现异常
                Plc.SendMessage(handler, plcRecv);
                UCMain.Instance.AddInfo(CameraID.ToString(), "NozzleDutDetect流程失败-DutBackgroundDetect（）", false);
                UCMain.Instance.ShowMessage(CameraID, "NozzleDutDetect流程失败-DutBackgroundDetect（）");
                Flow.Windlist[CameraID].Message = "NozzleDutDetect流程失败-DutBackgroundDetect（）";
                Flow.Log("NozzleDutDetect" + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public static bool DutBackgroundDetect(HImage image)
        {

            #region 图像处理


            //图像处理
            HObject region;
            HObject regionFillUp;
            HObject connectionRegion;
            HObject selectRegion;
            HObject imageReduce;
            HObject unionRegion;
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
        /// <summary>
        /// 获取相机中心到mark点的距离
        /// </summary>
        /// <param name="OffesetX"></param>
        /// <param name="OffesetY"></param>
        public static void GetMarkOffeset(out double OffesetX, out double OffesetY)
        {
            //1112.9,873.90
            HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down2,3000 / 2,  4096 / 2,
                     out HTuple transX1, out HTuple transY1);
            HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down2, serializableData.DownCam_mark_Row,serializableData.DownCam_mark_Col,
                 out HTuple transX2, out HTuple transY2);

            HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_down2, 873.90,
     1112.9, out transX2, out transY2);
            OffesetX = transX1 - transX2;
            OffesetY = transY1 - transY2;
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