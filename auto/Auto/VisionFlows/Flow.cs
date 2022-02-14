using AlcUtility;
using HalconDotNet;
using Poc2Auto.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VisionControls;
using VisionSDK;

namespace VisionFlows
{
    public class Flow
    {
        public static List<SuperWind> Windlist;
        public static FrmVisionUI FrmVisionUI { get; private set; }
        public static XmlHelper XmlHelper { get; private set; }
        public static FormNewCalib NewCalib { get; set; }
        public static ToolStripMenuItem ExtendMenu { get; private set; }
        static Flow()
        {
            try
            {
                InitExtendForm();
                InitConfigs();
            }
            catch (Exception ex)
            {
                VisionPlugin.GetInstance().Log(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// 插件初始化
        /// </summary>
        public static void InitExtendForm()
        {
            CameraManager.open(ref CameraManager.CameraList, "basler");
            ExtendMenu = new ToolStripMenuItem();
            FrmVisionUI = new FrmVisionUI();
            NewCalib = new FormNewCalib();
        }
        /// <summary>
        /// 初始化文件
        /// </summary>
        public static void InitConfigs()
        {
            XmlHelper = XmlHelper.Instance();
            if (File.Exists(Utility.ConfigFile + "Compensate.xml"))
                SystemPrara.Instance = (SystemPrara)XmlHelper.DeserializeFromXml(Utility.ConfigFile + "Compensate.xml", typeof(SystemPrara));
        }

       
        public static void Log(string text, AlcErrorLevel errLevel = AlcErrorLevel.TRACE)
        {
            return;
            VisionPlugin.GetInstance().Log(text, errLevel);
        }
    }
}