using AlcUtility;
using HalconDotNet;
using Poc2Auto.Common;
using Poc2Auto.Database;
using Poc2Auto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
namespace VisionFlows
{
    /// <summary>
    /// AutoSelect模式上位机工位命名方式
    /// </summary>
    public enum EnumAutoSelect
    {
        Cam1Tray1Mark = 0,
        Cam1Tray1Slot = 1,
        Cam1Tray2Mark = 2,
        Cam1Tray2Slot = 3,
        Cam1Tray3Mark = 4,
        Cam1Tray3Slot = 5,
        Cam1Tray4Mark = 6,
        Cam1Tray4Slot = 7,
        Cam3Nozzle1Mark = 8,
        None
    }

    /// <summary>
    /// 自动挑选模式相关参数
    /// </summary>
    [Serializable]
    public class AutoSelectPara
    {
        public int WorkID { get; set; }
        public int PosiID { get; set; }
        public int AlgorithmID { get; set; }
        public double CompensateX { get; set; }
        public double CompensateY { get; set; }
        public double CompensateR { get; set; }
    }

    /// <summary>
    /// 自动挑选模式相关结果
    /// </summary>
    [Serializable]
    public class AutoSelectResult
    {
        public int WorkID { get; set; }
        public List<AutoSelectCoordi> AutoSelectCoordiList { get; set; }
    }

    [Serializable]
    public class AutoSelectCoordi
    {
        public uint ID { get; set; }
        public double ImageR { get; set; }
        public double ImageC { get; set; }
        public double RobotX { get; set; }
        public double RobotY { get; set; }
    }

    /// <summary>
    /// 自动挑选模式相关数据
    /// </summary>
    [Serializable]
    public class AutoSelectData
    {
        public static AutoSelectData Instance;
        public List<AutoSelectPara> AutoSelectParaList;
        public List<AutoSelectResult> AutoSelectResultList;
    }

    /// <summary>
    /// 自动挑选模式相关方法
    /// </summary>
    public class AutoSelect_New
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
            var para = AutoSelectData.Instance.AutoSelectParaList[(int)work];
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
                        //Nozzle1PutSocketDut(handler, para);
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
                   // Nozzle2PutTrayDut(handler, para);
                }
                //吸嘴2放料或者吸嘴2取socket料
                else if (func == 5)
                {
                    if (posid == 8)
                    {
                       // Nozzle2GetSocketDut(handler, para);
                    }
                    else
                    {
                       // Nozzle2PutTrayDut(handler, para);
                    }
                }
                else if (func == 33)
                {
                  //  Nozzle2GetTrayDut(handler, para);
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
        /// <param name="cameraID"></param>
        /// <param name="getpositionId"></param>
        /// <returns></returns>
        public static EnumAutoSelect GetWork(double posID, EnumCamera cameraID)
        {
            EnumAutoSelect work;
            //posID = (ushort)posID;
            //
            switch ((EnumPlcWork)posID)
            {
                case EnumPlcWork.LoadTray1ID:
                    work = EnumAutoSelect.Cam1Tray1Slot;
                    break;

                case EnumPlcWork.LoadTray2ID:
                    work = EnumAutoSelect.Cam1Tray2Slot;
                    break;

                case EnumPlcWork.NGTrayID:
                    work = EnumAutoSelect.Cam1Tray3Slot;
                    break;

                case EnumPlcWork.OKTray1ID:
                    work = EnumAutoSelect.Cam1Tray4Slot;
                    break;

                case EnumPlcWork.LoadSecondPosID:
                    work = EnumAutoSelect.Cam3Nozzle1Mark;
                    break;

                case EnumPlcWork.TrayMark:
                    work = EnumAutoSelect.Cam1Tray3Mark;
                    break;

                default:
                    work = EnumAutoSelect.None;
                    break;
            }
            return work;
        }

        /// <summary>
        /// 判断产品有无，读QR码，DUT定位，返回吸嘴到取料位的X,Y,R坐标
        /// </summary>
        /// <returns></returns>
        public static void Nozzle1PutTrayDut(MessageHandler handler, AutoSelectPara para)
        {
            AutoNormalPara normalPara = new AutoNormalPara();
            normalPara.AlgorithmID = para.AlgorithmID;
            normalPara.CompensateR = para.CompensateR;
            normalPara.CompensateX = para.CompensateX;
            normalPara.CompensateY = para.CompensateY;
            normalPara.PosiID = para.PosiID;
            normalPara.WorkID = para.WorkID;
            AutoNormal_New.Nozzle1PutTrayDut(handler, normalPara);
        }
        /// <summary>
        /// 判断产品有无，读QR码，DUT定位，返回吸嘴到取料位的X,Y,R坐标
        /// </summary>
        /// <returns></returns>
        public static void Nozzle1PutTrayDut_OneCam(MessageHandler handler, AutoSelectPara para)
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
        /// <summary>
        /// 判断产品有无，读QR码，DUT定位，返回吸嘴到取料位的X,Y,R坐标
        /// </summary>
        /// <returns></returns>
        public static void Nozzle1GetTrayDut(MessageHandler handler, AutoSelectPara para)
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
        /// 获取Tray放料位的特征点和角度，返回DUT旋转到放料位的X,Y,R坐标
        /// </summary>
        /// <returns></returns>

        public static void SecondDut(MessageHandler handler, AutoSelectPara para)
        {
            AutoNormalPara normalPara = new AutoNormalPara();
            normalPara.AlgorithmID = para.AlgorithmID;
            normalPara.CompensateR = para.CompensateR;
            normalPara.CompensateX = para.CompensateX;
            normalPara.CompensateY = para.CompensateY;
            normalPara.PosiID = para.PosiID;
            normalPara.WorkID = para.WorkID;

            AutoNormal_New.SecondDut(handler, normalPara);
        }
        public static void CheckPutDut(MessageHandler handler, AutoSelectPara para)
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
        public static void CheckPutSocketDut(MessageHandler handler, AutoSelectPara para)
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
    }
}