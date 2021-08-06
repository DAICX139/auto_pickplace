using AlcUtility;
using HalconDotNet;
using Poc2Auto.Common;
using System;
using System.Collections.Generic;

namespace VisionFlows
{
    /// <summary>
    /// DoeSameTray模式上位机工位枚举
    /// </summary>
    public enum EnumDoeSameTray
    {
        Cam1Tray3Mark = 0,
        Cam1Tray3Slot,
        None
    }

    [Serializable]
    public class DoeSameTrayPara
    {
        public int WorkID { get; set; }
        public int PosiID { get; set; }
        public int AlgorithmID { get; set; }
        public double CompensateX { get; set; }
        public double CompensateY { get; set; }
        public double CompensateR { get; set; }
    }

    [Serializable]
    public class DoeSameTrayData
    {
        public static DoeSameTrayData Instance;
        public List<DoeSameTrayPara> DoeSameTrayParaList;
    }

    public class DoeSameTray_New
    {
     
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
            var para = DoeSameTrayData.Instance.DoeSameTrayParaList[(int)work];
            //左上相机
            if (cameraID == EnumCamera.LeftTop)
            {
                //吸嘴1放料
                if (func == 21)
                {
                    Nozzle1PutTrayDut_OneCam(handler, para);
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
                      //  Nozzle1PutSocketDut(handler, para);
                    }
                }
                //吸嘴1下料后拍照存图或者Sockt放料完拍照存图
                else if (func == 9)
                {
                    if (posid == 3 || posid == 4 || posid == 5 || posid == 9)
                    {
                        //放完料拍照看是否放进去了
                        CheckPutDut(handler, para);
                    }
                    else
                    {
                        //放完料拍照看是否放进去了 socket
                        CheckPutSocketDut(handler, para);
                    }
                }
            }
            //右上相机
            else if (cameraID == EnumCamera.RightTop)
            {
                if (func == 9)
                {
                 //   AutoNormal_New.CheckPutDut_right(handler, para);
                }
                //吸嘴2放料或者吸嘴2取socket料
                else if (func == 5)
                {
                    if (posid == 8)
                    {
                     //   Nozzle2GetSocketDut(handler, para);
                    }
                    else
                    {
                       //  Nozzle2PutTrayDut(handler, para);
                    }
                }
                else if (func == 33)
                {
                    //Nozzle2GetTrayDut(handler, para);
                }
            }
            //下相机
            else if (cameraID == EnumCamera.Bottom)
            {
                //扫码+定位
                if (func == 7)
                {
                  //  SecondDut(handler, para);
                }
                //只定位
                else if (func == 5)
                {
                   //  SecondDut(handler, para);
                }
                else if (func == 8)
                {
                    //二次定位存图
                 //   SecondDut(handler, para);
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
        public static EnumDoeSameTray GetWork(double posID, EnumCamera cameraID)
        {
            EnumDoeSameTray work;

            switch ((EnumPlcWork)posID)
            {
                case EnumPlcWork.LoadTray1ID:
                    work = EnumDoeSameTray.Cam1Tray3Slot;
                    break;

                case EnumPlcWork.NGTrayID:
                    work = EnumDoeSameTray.Cam1Tray3Slot;
                    break;

                case EnumPlcWork.TrayMark:
                    work = EnumDoeSameTray.Cam1Tray3Mark;
                    break;

                case EnumPlcWork.LoadTray2ID:
                    work = EnumDoeSameTray.Cam1Tray3Slot;
                    break;

                default:
                    work = EnumDoeSameTray.None;
                    break;
            }
            return work;
        }

        public static void Nozzle1GetTrayDut(MessageHandler handler, DoeSameTrayPara para)
        {
            AutoNormalPara normalPara = new AutoNormalPara();
            normalPara.AlgorithmID = para.AlgorithmID;
            normalPara.CompensateR = para.CompensateR;
            normalPara.CompensateX = para.CompensateX;
            normalPara.CompensateY = para.CompensateY;
            normalPara.PosiID = para.PosiID;
            normalPara.WorkID = para.WorkID;

            AutoNormal_New.Nozzle1GetTrayDut(handler, normalPara);
        }
      
        /// <summary>
        ///
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="para"></param>
        public static void Nozzle1PutTrayDut_OneCam(MessageHandler handler, DoeSameTrayPara para)
        {
            AutoNormalPara normalPara = new AutoNormalPara();
            normalPara.AlgorithmID = para.AlgorithmID;
            normalPara.CompensateR = para.CompensateR;
            normalPara.CompensateX = para.CompensateX;
            normalPara.CompensateY = para.CompensateY;
            normalPara.PosiID = para.PosiID;
            normalPara.WorkID = para.WorkID;

            AutoNormal_New.Nozzle1PutTrayDut_OneCam(handler, normalPara);
        }
        public static void CheckPutSocketDut(MessageHandler handler, DoeSameTrayPara para)
        {
            AutoNormalPara normalPara = new AutoNormalPara();
            normalPara.AlgorithmID = para.AlgorithmID;
            normalPara.CompensateR = para.CompensateR;
            normalPara.CompensateX = para.CompensateX;
            normalPara.CompensateY = para.CompensateY;
            normalPara.PosiID = para.PosiID;
            normalPara.WorkID = para.WorkID;

            AutoNormal_New.CheckPutSocketDut(handler, normalPara);
        }
        public static void CheckPutDut(MessageHandler handler, DoeSameTrayPara para)
        {
            AutoNormalPara normalPara = new AutoNormalPara();
            normalPara.AlgorithmID = para.AlgorithmID;
            normalPara.CompensateR = para.CompensateR;
            normalPara.CompensateX = para.CompensateX;
            normalPara.CompensateY = para.CompensateY;
            normalPara.PosiID = para.PosiID;
            normalPara.WorkID = para.WorkID;

            AutoNormal_New.CheckPutDut_left(handler, normalPara);
        }
       
    }
}