using AlcUtility;
using HalconDotNet;
using Poc2Auto.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using VisionDemo;

namespace VisionFlows
{
    public class Flow
    {
        public static FrmMain FrmMain { get; private set; }
        public static FrmPlc FrmPlc { get; private set; }

        public static FrmAutoNormal FrmAutoNormal { get; private set; }
        public static FrmAutoSelect FrmAutoSelect { get; private set; }

        /// <summary> Doe模式参数配置 </summary>
        public static FrmVisionUI FrmVisionUI { get; private set; }

        public static XmlHelper XmlHelper { get; private set; }

        /// <summary> 视觉 图像 定位功能 测试窗体</summary>
        public static VisionCalculate.FormLocationSet FormLocationTest { get; set; }

        public static FormNewCalib NewCalib { get; set; }



        public static ToolStripMenuItem ExtendMenu { get; private set; }

        
        static Flow()
        {
            try
            {
               
                InitExtendForm();
                InitConfigs();
                InitAlgorithm();
            }
            catch (Exception ex)
            {
                VisionPlugin.GetInstance().Log(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public static void InitExtendForm()
        {
            ExtendMenu = new ToolStripMenuItem();

            FrmMain = new FrmMain();
            //
            FrmVisionUI = new FrmVisionUI();
            FrmPlc = new FrmPlc();

            FrmAutoNormal = new FrmAutoNormal();
            FrmAutoSelect = new FrmAutoSelect();
            FormLocationTest = new VisionCalculate.FormLocationSet();
            NewCalib = new FormNewCalib();

            //
            FrmMain.SetFlowForm(FrmVisionUI);
            SetExtendForm(FrmPlc, ExtendMenu);
            ToolStripMenuItem calib = new ToolStripMenuItem("Calib");

            ExtendMenu.DropDownItems.Add(calib);
            ToolStripMenuItem auto = new ToolStripMenuItem("Auto");
            SetExtendForm(FrmAutoNormal, auto);
            SetExtendForm(FrmAutoSelect, auto);
            ExtendMenu.DropDownItems.Add(auto);
            FrmMain.SetExtendMenu(ExtendMenu);
            //
            FrmMain.SetUI();
        }

        public static void SetExtendForm(Form frm, ToolStripMenuItem menu)
        {
            foreach (ToolStripDropDownItem item in menu.DropDownItems)
            {
                if (item.Text == frm.Text) return;
            }

            IniForm(frm);
            ToolStripMenuItem tsm = new ToolStripMenuItem(frm.Text);
            frm.FormClosing += new FormClosingEventHandler((s, e) => { ((Form)s).Hide(); e.Cancel = true; });
            tsm.Click += new EventHandler((s, e) => { frm.Show(); });
            menu.DropDownItems.Add(tsm);
        }

        public static void IniForm(Form frm)
        {
            frm.ShowIcon = false;
            frm.MaximizeBox = false;
            frm.MinimizeBox = false;
            frm.TopMost = true;
            frm.BackColor = Color.White;
            frm.StartPosition = FormStartPosition.CenterScreen;
        }

        public static void InitConfigs()
        {
            XmlHelper = XmlHelper.Instance();
            if (File.Exists(Utility.Config + "System.xml"))
                SystemPara.Instance = (SystemPara)XmlHelper.DeserializeFromXml(Utility.Config + "System.xml", typeof(SystemPara));
            if (File.Exists(Utility.Config + "Compensate.xml"))
                SystemPrara_New.Instance = (SystemPrara_New)XmlHelper.DeserializeFromXml(Utility.Config + "Compensate.xml", typeof(SystemPrara_New));

            if (File.Exists(Utility.Config + "Plc.xml"))
                PlcPara.Instance = (PlcPara)XmlHelper.DeserializeFromXml(Utility.Config + "Plc.xml", typeof(PlcPara));
            if (File.Exists(Utility.Config + "AcqPosi.xml"))
                AcqPosiData.Instance = (AcqPosiData)XmlHelper.DeserializeFromXml(Utility.Config + "AcqPosi.xml", typeof(AcqPosiData));
            if (File.Exists(Utility.Config + "Algorithm.xml"))
                AlgorithmData.Instance = (AlgorithmData)XmlHelper.DeserializeFromXml(Utility.Config + "Algorithm.xml", typeof(AlgorithmData));

            if (File.Exists(Utility.Config + "AutoNormal.xml"))
                AutoNormalData.Instance = (AutoNormalData)XmlHelper.DeserializeFromXml(Utility.Config + "AutoNormal.xml", typeof(AutoNormalData));
            if (File.Exists(Utility.Config + "AutoSelect.xml"))
                AutoSelectData.Instance = (AutoSelectData)XmlHelper.DeserializeFromXml(Utility.Config + "AutoSelect.xml", typeof(AutoSelectData));
            if (File.Exists(Utility.Config + "DoeSameTray.xml"))
                DoeSameTrayData.Instance = (DoeSameTrayData)XmlHelper.DeserializeFromXml(Utility.Config + "DoeSameTray.xml", typeof(DoeSameTrayData));
             

   

        }

        public static void InitAlgorithm()
        {
            //
            //if (File.Exists(Utility.Tran + EnumCamera.LeftTop.ToString() + ".mat"))
            //    Utility.HomMat2D.Add(EnumCamera.LeftTop.ToString(), ImageProcessBase.GetHomMat2D((int)EnumCamera.LeftTop));
            //if (File.Exists(Utility.Tran + EnumCamera.RightTop.ToString() + ".mat"))
            //    Utility.HomMat2D.Add(EnumCamera.RightTop.ToString(), ImageProcessBase.GetHomMat2D((int)EnumCamera.RightTop));
            //if (File.Exists(Utility.Tran + EnumCamera.Bottom.ToString() + ".mat"))
            //    Utility.HomMat2D.Add(EnumCamera.Bottom.ToString(), ImageProcessBase.GetHomMat2D((int)EnumCamera.Bottom));
            //
            //if (File.Exists(Utility.Model + EnumAlgorithm.CalibMark.ToString() + ".sbm"))
            //    Utility.ShapeModel.Add(EnumAlgorithm.CalibMark.ToString(), new HShapeModel(Utility.Model + EnumAlgorithm.CalibMark.ToString() + ".sbm"));
            //Utility.ShapeModel.Add(EnumAlgorithm.NozzleMark.ToString(), new HShapeModel(Utility.Model + EnumAlgorithm.NozzleMark.ToString() + ".sbm"));
            //Utility.ShapeModel.Add(EnumAlgorithm.TrayMark.ToString(), new HShapeModel(Utility.Model + EnumAlgorithm.TrayMark.ToString() + ".sbm"));
            //Utility.ShapeModel.Add(EnumAlgorithm.TrayDutFront.ToString(), new HShapeModel(Utility.Model + EnumAlgorithm.TrayDutFront.ToString() + ".sbm"));
            //Utility.ShapeModel.Add(EnumAlgorithm.TraySlot.ToString(), new HShapeModel(Utility.Model + EnumAlgorithm.TraySlot.ToString() + ".sbm"));
            //Utility.ShapeModel.Add(EnumAlgorithm.SocketMark.ToString(), new HShapeModel(Utility.Model + EnumAlgorithm.SocketMark.ToString() + ".sbm"));
            //Utility.ShapeModel.Add(EnumAlgorithm.SocketDutFront.ToString(), new HShapeModel(Utility.Model + EnumAlgorithm.SocketDutFront.ToString() + ".sbm"));
            //Utility.ShapeModel.Add(EnumAlgorithm.SecondDutBack.ToString(), new HShapeModel(Utility.Model + EnumAlgorithm.SecondDutBack.ToString() + ".sbm"));
            //Utility.ShapeModel.Add(EnumAlgorithm.PreciseMark.ToString(), new HShapeModel(Utility.Model + EnumAlgorithm.PreciseMark.ToString() + ".sbm"));
            //
            //if (File.Exists(Utility.Metro + EnumAlgorithm.CalibMark.ToString()))
            //    Utility.MetrologyModel.Add(EnumAlgorithm.CalibMark.ToString(), new HMetrologyModel(Utility.Metro + EnumAlgorithm.CalibMark.ToString()));
            //Utility.MetrologyModel.Add(EnumAlgorithm.NozzleMark.ToString(), new HMetrologyModel(Utility.Metro + EnumAlgorithm.NozzleMark.ToString()));
            //Utility.MetrologyModel.Add(EnumAlgorithm.TrayMark.ToString(), new HMetrologyModel(Utility.Metro + EnumAlgorithm.TrayMark.ToString()));
            //Utility.MetrologyModel.Add(EnumAlgorithm.TrayDutFront.ToString(), new HMetrologyModel(Utility.Metro + EnumAlgorithm.TrayDutFront.ToString()));
            //Utility.MetrologyModel.Add(EnumAlgorithm.TraySlot.ToString(), new HMetrologyModel(Utility.Metro + EnumAlgorithm.TraySlot.ToString()));
            //Utility.MetrologyModel.Add(EnumAlgorithm.SocketMark.ToString(), new HMetrologyModel(Utility.Metro + EnumAlgorithm.SocketMark.ToString()));
            //Utility.MetrologyModel.Add(EnumAlgorithm.SocketDutFront.ToString(), new HMetrologyModel(Utility.Metro + EnumAlgorithm.SocketDutFront.ToString()));
            //Utility.MetrologyModel.Add(EnumAlgorithm.SecondDutBack.ToString(), new HMetrologyModel(Utility.Metro + EnumAlgorithm.SecondDutBack.ToString()));
            //Utility.MetrologyModel.Add(EnumAlgorithm.PreciseMark.ToString(), new HMetrologyModel(Utility.Metro + EnumAlgorithm.PreciseMark.ToString()));
        }

        public static void Log(string text, AlcErrorLevel errLevel = AlcErrorLevel.TRACE)
        {
            VisionPlugin.GetInstance().Log(text, errLevel);
        }
    }
}