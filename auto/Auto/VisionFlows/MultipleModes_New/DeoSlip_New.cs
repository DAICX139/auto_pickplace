using AlcUtility;
using HalconDotNet;
using Poc2Auto.Common;
using Poc2Auto.Database;
using Poc2Auto.Model;
using System;
using System.Collections.Generic;

namespace VisionFlows
{
    /// <summary>
    /// 自动运行模式相关数据
    /// </summary>
    public class DeoSlip_New
    {
        //下相机
        private static HTuple down_row = 0;

        private static HTuple down_col = 0;
        private static HTuple down_angle = 0;
        private static SerializableData _serializableData;

        //标定数据
        public static SerializableData serializableData
        {
            get
            {
                if (_serializableData == null)
                {
                    _serializableData = (SerializableData)Flow.XmlHelper.DeserializeFromXml(Utility.calib+"calib_up.xml", typeof(SerializableData));
                    HOperatorSet.ReadTuple(Utility.calib+"calib_down1", out _serializableData.HomMat2D_down1);
                    HOperatorSet.ReadTuple(Utility.calib+"calib_down2", out _serializableData.HomMat2D_down2);
                    HOperatorSet.ReadTuple(Utility.calib+"calib_up1", out _serializableData.HomMat2D_up1);
                    HOperatorSet.ReadTuple(Utility.calib+"calib_up2", out _serializableData.HomMat2D_up2);
                }
                return _serializableData;
            }
        }

      
        public static void Execute(MessageHandler handler, EnumCamera cameraID)
        {            //判断当前生产的是哪种料

            //判断当前生产的是哪种料
            if (ConfigMgr.Instance.CurrentImageType == "")
            {

            }
            else
            {
                //POC2这款产品
                AutoNormal_New.ImageProcess = new ImageProcess_Poc2();
            }
            var plcSend = (double[])handler.CmdParam.KeyValues[PLCParamNames.PLCSend].Value;
            var work = GetWork(plcSend[(int)EnumPLCSend.PosID], cameraID);
            double func = plcSend[5];
            double posid = plcSend[4];
            var para = AutoNormalData.Instance.AutoNormalParaList[(int)work];
            //左上相机
            if (cameraID == EnumCamera.LeftTop)
            {
                //吸嘴1放料
                if (func == 21)
                {
                    //Nozzle1PutTrayDut_OneCam(handler, para);
                }
                //吸嘴1取料或者Sockt定位
                else if (func == 5)
                {
                    if (posid == 1 || posid == 2||posid==3||posid==9)
                    {
                        Nozzle1GetTrayDut(handler, para);
                    }
                    else
                    {
                       // Nozzle1PutSocketDut(handler, para);
                    }
                }
                //吸嘴1下料后拍照存图或者Sockt放料完拍照存图
                else if (func == 9)
                {
                    if (posid == 3 || posid == 4 || posid == 5 || posid == 9)
                    {
                        //放完料拍照看是否放进去了
                       AutoNormal_New.CheckPutDut_left(handler, para);
                    }
                    else
                    {
                        //放完料拍照看是否放进去了 socket
                        AutoNormal_New.CheckPutSocketDut(handler, para);
                    }
                }
            }
            //右上相机
            else if (cameraID == EnumCamera.RightTop)
            {
                if (func == 9)
                {
                    AutoNormal_New.CheckPutDut_right(handler, para);
                }
                //吸嘴2放料或者吸嘴2取socket料
                else if (func == 5)
                {
                    if (posid == 8)
                    {
                       //  Nozzle2GetSocketDut(handler, para);
                    }
                    else
                    {
                       // Nozzle2PutTrayDut(handler, para);
                    }
                }
                else if (func == 33)
                {
                   // Nozzle2GetTrayDut(handler, para);
                }
            }
            //下相机
            else if (cameraID == EnumCamera.Bottom)
            {
                //扫码+定位
                if (func == 7)
                {
                    SecondDut(handler, para);
                }
                //只定位
                else if (func == 5)
                {
                    SecondDut(handler, para);
                }
                else if (func == 8)
                {
                    //二次定位存图
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
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.SlotExposeTime);
                if (image == null)
                {
                    return;
                }
                //判断是否有料
                HRegion roi = ImagePara.Instance.GetSerchROI("SlotDutROI");
                InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                {
                    Flow.FrmMain.Windlist[CameraID].image = image;
                    //无料
                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, false);//无dut
                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 7, false);//代表定位失败

                    Flow.FrmMain.Windlist[CameraID].Message = "无料！";
                    Plc.SendMessage(handler, plcRecv);
                    image.Dispose();
                    return;
                }
                //图像处理
                InputPara locationPara = new InputPara(image, roi, null, 0);
                OutPutResult locationResult = new OutPutResult();
                locationResult = AutoNormal_New.ImageProcess.TrayDutFront(locationPara);

                if (!locationResult.IsRunOk)
                {
                    //匹配失败 显示拍照图片
                    Flow.FrmMain.Windlist[CameraID].image = locationPara.image;
                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, false);//无dut
                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 7, false); //定位失败
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("TrayDutPos Location Failed");
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
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
                Plc.SendMessage(handler, plcRecv);
                HXLDCont Cross = new HXLDCont();
                HOperatorSet.TupleRad(locationResult.Angle, out HTuple phi);
                Cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 6000, phi);

                //显示结果到FormMain
                Flow.FrmMain.Windlist[CameraID].image = locationPara.image;
                Flow.FrmMain.Windlist[CameraID].obj = Cross;
                Flow.FrmMain.Windlist[CameraID].obj = locationResult.SmallestRec2Xld;
                locationResult.Dispos();
                image.Dispose();
                return;
            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, false);//无dut
                plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 7, false);//定位失败
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("TrayDutPos " + ex.Message + Environment.NewLine + ex.StackTrace);
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

            //   var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID = 0;
            try
            {
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.SlotExposeTime);
                if (image == null)
                {
                    Flow.Log("采集的图片 null");
                    return;
                }
                HRegion roi = ImagePara.Instance.GetSerchROI("SlotROI");
                //保存图像
                if (Utility.GetBitValue(plcSend.Func, 3))
                {
                    VisionLocation.SaveImage(image, SystemPara.Instance.ImageSavePath + ((EnumAutoNormal)CameraID).ToString() + DateTime.Now.ToString("yyyyMMddHHmmss"));
                    //PLC需要判断产品有无

                    if (Utility.GetBitValue(plcSend.Func, 0))
                    {

                        InputPara para_temp = new InputPara(image, roi, null, ImagePara.Instance.DutScore);
                        if (!AutoNormal_New.ImageProcess.SlotDetect(para_temp, out HObject yy))
                        {
                            Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                            //有料 则返回失败
                            plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, false);//无dut
                            plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                            plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 7, false);//定位失
                        }
                    }
                }

                InputPara locationPara = new InputPara(image, roi, null, 0);
                OutPutResult locationResult = new OutPutResult();
                //图像处理
                locationResult = AutoNormal_New.ImageProcess.ShotSlot(locationPara);

                if (!locationResult.IsRunOk)
                {
                    Flow.FrmMain.Windlist[CameraID].image = locationPara.image;
                    plcRecv.RPos = plcSend.RPos;
                    plcRecv.XPos = plcSend.XPos;
                    plcRecv.YPos = plcSend.YPos;
                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, false);//无dut
                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 7, true);//其它异常
                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("TrayPlaceDutPos(Slot) Location Failed");
                    Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
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

                plcRecv.XPos = cal_X + SystemPrara_New.Instance.Nozzle1_Slot_CompensateX;
                plcRecv.YPos = cal_Y + SystemPrara_New.Instance.Nozzle1_Slot_CompensateY;
                plcRecv.RPos = cal_R + SystemPrara_New.Instance.Nozzle1_Slot_CompensateR;
                down_row = 0;
                down_col = 0;
                down_angle = 0;

                #endregion 计算偏移和角度**

                Plc.SendMessage(handler, plcRecv);
                //显示结果

                Flow.FrmMain.Windlist[CameraID].image = image;
                HXLDCont cross = new HXLDCont();
                cross.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 6000, locationResult.Angle);
                Flow.FrmMain.Windlist[CameraID].obj = cross;
                locationResult.Dispos();
                image.Dispose();
            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 0, false);//总结果 视觉定位失败
                plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, false);//无dut
                plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 7, false);//定位失败
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("TrayPlaceDutPos " + ex.Message + Environment.NewLine + ex.StackTrace);
                return;
            }
        }

         
      
        public static void SecondDut(MessageHandler handler, AutoNormalPara para)
        {
            ImageProcessBase.GetPlcData(handler, out PLCSend plcSend, out PLCRecv plcRecv);

            //var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            int CameraID =2;
            try
            {
                var image = ImageProcessBase.GrabImage(CameraID, ImagePara.Instance.DutBackExposeTime);
                HRegion roi = ImagePara.Instance.GetSerchROI("DutBackROI");
                //保存图像
                if (Utility.GetBitValue(plcSend.Func, 3))
                {
                    string path = "D:\\DeoSlipImage";
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    VisionLocation.SaveImage(image, path + "\\" + ((EnumAutoNormal)CameraID).ToString() + DateTime.Now.ToString("yyyyMMddHHmmss"));
                }
                //图像处理
                InputPara locationPara = new InputPara(image, roi, AutoNormal_New.SecondDutBackMode, 0);
                OutPutResult locationResult = new OutPutResult();

                locationResult = AutoNormal_New.ImageProcess.SecondDutBack(locationPara, plcSend);

                if (!locationResult.IsRunOk)
                {
                    Flow.FrmMain.Windlist[CameraID].image = locationPara.image;

                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 7, true);//其它异常

                    Plc.SendMessage(handler, plcRecv);
                    Flow.Log("SecondDutPos Vision Location Failed");
                    return;
                }

                #region 计算偏移和角度

                HOperatorSet.AffineTransPoint2d(serializableData.HomMat2D_up1, locationResult.findPoint.Column, locationResult.findPoint.Row, out HTuple transX, out HTuple transY);
                string data = DateTime.Now.ToLocalTime().ToString() + "," + transX.D.ToString("f3") + "," + transY.D.ToString("f3");
                ImageProcessBase.WriteCsv("DoeSlipData.csv", data);
                //记录二次定位的物料位置和角度
                down_row = locationResult.findPoint.Row;
                down_col = locationResult.findPoint.Column;
                down_angle = locationResult.Angle;

                #endregion 计算偏移和角度

                if (!locationResult.IsReadDataCode) //如果读码 未成功
                {
                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 2, false);//其它异常
                }
                //PLC不扫码
                if (!Utility.GetBitValue(plcSend.Func, 1))
                {
                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, true);//有DUT
                    plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 0, true);
                }
                //sn码模式并且sn在列表里面  需要把Double[6]:BinValue值 写1
                if (RunModeMgr.RunMode == RunMode.AutoSelectSn)
                {
                    if (Overall.SelectionList.Contains(Overall.ScanResult))
                    {
                        plcRecv.BinValue = 1;
                    }
                    else
                    {
                        plcRecv.BinValue = 0;
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
                Plc.SendMessage(handler, plcRecv);
                //显示结果

                Flow.FrmMain.Windlist[CameraID].image = image;
                Flow.FrmMain.Windlist[CameraID].obj = locationResult.SmallestRec2Xld;
                HXLDCont CrossContour = new HXLDCont();
                CrossContour.GenCrossContourXld(locationResult.findPoint.Row, locationResult.findPoint.Column, 6000, locationResult.Angle);
                Flow.FrmMain.Windlist[CameraID].obj = CrossContour;
                Flow.FrmMain.Windlist[CameraID].Message = "二维码:" + Overall.ScanResult;
                image.Dispose();
                locationResult.Dispos();
            }
            catch (Exception ex)
            {
                Flow.FrmMain.Windlist[CameraID].OKNGlable = false;
                plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 0, false);//总结果
                plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 1, false);//无料
                plcRecv.Result = (ushort)Utility.SetBitValue(plcRecv.Result, 7, false);//定位失败
                plcRecv.BinValue = 0;
                Plc.SendMessage(handler, plcRecv);
                Flow.Log("SecondDutPos 定位失败" + ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}