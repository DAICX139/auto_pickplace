using HalconDotNet;
using Poc2Auto.Common;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using VisionModules;
using VisionUtility;

namespace VisionFlows
{
    public partial class FrmVisionUI : Form
    {
        private VisionCameraBase camera;

        public FrmVisionUI()
        {
            InitializeComponent();
        }

        private void FrmVisionUI_Load(object sender, EventArgs e)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            comboBox_type.Items.Clear();
            DirectoryInfo theFolder = new DirectoryInfo(Utility.type);
            FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo NextFile in thefileInfo)  //遍历文件
            {

                comboBox_type.Items.Add(NextFile.Name);
            }
            comboBox_type.Text = ConfigMgr.Instance.CurrentImageType;
            try
            {
                ImagePara.Instance = (ImagePara)XmlHelper.Instance().DeserializeFromXml(Utility.type + ConfigMgr.Instance.CurrentImageType, typeof(ImagePara));
            if (ImagePara.Instance.SoketDut_maxthreshold.Length<=0)
                {
                    ImagePara.Instance.SoketDut_maxthreshold = new int[5];
                    ImagePara.Instance.SoketDut_minthreshold = new int[5];
                }

             if(ImagePara.Instance.SocketGet_rowCenter.Length<=0)
                {
                    ImagePara.Instance.SocketGet_rowCenter = new float[5];

                    ImagePara.Instance.SocketGet_colCenter = new float[5];
                    ImagePara.Instance.SocketGet_angleCenter = new float[5];

                    //socket mark中心
                    ImagePara.Instance.DutMode_rowCenter = new float[5];

                    ImagePara.Instance.DutMode_colCenter = new float[5];
                    ImagePara.Instance.DutMode_angleCenter = new float[5];

                    //自定义的放料中心
                    ImagePara.Instance.SocketMark_GetDutRow = new float[5];

                    ImagePara.Instance.SocketMark_GetDutCol = new float[5];
                }
        

    }
            catch (Exception ex)
            {
                AlcUtility.AlcSystem.Instance.ShowMsgBox("读取视觉配方失败", "提示", AlcUtility.AlcMsgBoxButtons.OK);
            }
        }
        private void FormLocationTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            comboBox_type.Items.Clear();
            DirectoryInfo theFolder = new DirectoryInfo(Utility.type);
            FileInfo[] thefileInfo = theFolder.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo NextFile in thefileInfo)  //遍历文件
            {
                comboBox_type.Items.Add(NextFile.Name);
            }
            comboBox_type.Text = ConfigMgr.Instance.CurrentImageType;
        }

        private void chkOriginValue_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOriginValue.Checked)
            {
                Utility.OriginValue = true;
            }
            else
            {
                Utility.OriginValue = false;
            }
        }

        private void btnCalib_Click(object sender, EventArgs e)
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

        private void btnTest_Click(object sender, EventArgs e)
        {
            if (Flow.FormLocationTest == null || Flow.FormLocationTest.IsDisposed)
            {
                Flow.FormLocationTest = new VisionCalculate.FormLocationSet();
                Flow.FormLocationTest.Show();
            }
            else
            {
                Flow.FormLocationTest.Show();
            }
            Flow.FormLocationTest.FormClosing += FormLocationTest_FormClosing;
        }

        private void OnDisplayImage(HImage image)
        {
        }

        private void button_change_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("切换配方？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (!Directory.Exists("project"))
                {
                    Directory.CreateDirectory("project");
                }

                if (comboBox_type.SelectedIndex < 0)
                    return;
                try
                {
                    ImagePara.Instance = (ImagePara)XmlHelper.Instance().DeserializeFromXml(Utility.type +
                        comboBox_type.SelectedItem.ToString(), typeof(ImagePara));                  

                    ConfigMgr.Instance.CurrentImageType = comboBox_type.SelectedItem.ToString();
                    AlcUtility.AlcSystem.Instance.ShowMsgBox("读取视觉配方成功", "提示", AlcUtility.AlcMsgBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    AlcUtility.AlcSystem.Instance.ShowMsgBox("读取视觉配方失败" + ex.Message, "提示", AlcUtility.AlcMsgBoxButtons.OK, AlcUtility.AlcMsgBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HImage image = new HImage("d:\\1.bmp");
            HRegion roi = ImagePara.Instance.GetSerchROI("SlotDutROI");
          
                Flow.FrmMain.Windlist[0].image = image;
                //3590062
                Flow.FrmMain.Windlist[0].ObjColor = "red";
                Flow.FrmMain.Windlist[0].obj = roi;
                
               
            
           
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