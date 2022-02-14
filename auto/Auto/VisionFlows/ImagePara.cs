using HalconDotNet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VisionControls;

namespace VisionFlows
{
    /// <summary>
    ///
    /// 使用新标定方法的系统参数参数
    /// </summary>
    [Serializable]
    public class ImagePara
    {
        public static ImagePara Instance = new ImagePara();

        //曝光
        public double Exposure_LeftCamPutDUT;//
        public double Exposure_LeftCamGetDUT;//
        public double Exposure_LeftCamPutSocket;//
        public double Exposure_LeftCamCheckSocket;//
        public double Exposure_LeftCamCheckTray;//


        public double Exposure_DownCamScan;//
        public double Exposure_RightCamPutDUT;
        public double Exposure_RightCamGetDUT;//
        public double Exposure_RightCamCheckTray;//
        public double Exposure_RightCamGetSocket;//
        public double Exposure_RightCamCheckSocket;//

        


        //模板匹配识别分数
        public float SlotScore;
        public float SoketDutScore;
        public float DutScore;
        public float DutBackScore;
        public float SocketMarkScore;

        //tray识别阈值
        public int SoketMark_minthreshold = 125;

        public int SoketMark_maxthreshold = 255;

        //socket识别阈值
        public int[] SoketDut_minthreshold;

        public int []SoketDut_maxthreshold;

        //tray识别阈值
        public int TrayDut_minthreshold = 150;

        public int TrayDut_maxthreshold = 255;

        //dut宽高
        public int SoketDut_widthmin;

        public int SoketDut_heightmin;
        public int SoketDut_widthmax;
        public int SoketDut_heightmax;

        //SocketMark模板坐标
        public float SocketMark_rowCenter;
        public float SocketMark_colCenter;
        public float SocketMark_angleCenter;

        //Socket自定义的放料中心
        public float SocketMark_PutDutRow;
        public float SocketMark_PutDutCol;

        //取料时候的mark中心
        public float[] SocketGet_rowCenter;
        public float[] SocketGet_colCenter;
        public float[] SocketGet_angleCenter;

        //Slot模板坐标
        public float slot_rowCenter;

        public float slot_colCenter;
        public float slot_angleCenter;

        //slot自定义放料中心
        public float slot_offe_rowCenter;

        public float slot_offe_colCenter;
        public float slot_offe_angleCenter;

        public int PushBlock;

        //新增tray粗定位基准点

        public double tray_rough_posi_row;
        public double tray_rough_posi_col;

        //新增，socket判断有无料区域
        public float []SocketDut_RoiRow;
        public float []SocketDut_RoiCol;

        //搜索区域
        public Rectangle SlotROI;
        public Rectangle SlotDutROI;
        public Rectangle DutBackROI;
        public Rectangle DutBackDataCodeROI;
        public Rectangle SocketMarkROI;
        public Rectangle SocketBlockROI;
        public Rectangle SocketDutROI;

        //新增
        public Rectangle TrayDutROI1;
        public Rectangle TrayDutROI2;

        public Rectangle[] SocketIsDutROI;



        //取socket找两个圆搜索区域
        public Rectangle[] RegionROI1;
        public Rectangle[] RegionROI2;
        public ImagePara()
        {
            SlotScore = 0.1f;
            SoketDutScore = 0.6f;
            DutScore = 0.6f;
            DutBackScore = 0.8f;
            SocketMarkScore = 0.9f;
            SoketDut_minthreshold=new int[5];
            SoketDut_maxthreshold = new int[5];
            RegionROI1 = new Rectangle[5];
            RegionROI2 = new Rectangle[5];
            SocketIsDutROI = new Rectangle[5];
            SoketDut_widthmin = 200;
            SoketDut_widthmax = 400;
            SoketDut_heightmin = 600;
            SoketDut_heightmax = 1000;

            SocketMark_rowCenter = 0;
            SocketMark_colCenter = 0;
            SocketMark_angleCenter = 0;
            SocketMark_PutDutRow = 0;
            SocketMark_PutDutCol = 0;

            Exposure_LeftCamGetDUT = 8000;
            Exposure_LeftCamPutSocket = 8000;
            Exposure_LeftCamGetDUT = 8000;
            Exposure_DownCamScan = 8000;

            SoketMark_minthreshold = 125;
            SoketMark_maxthreshold = 255;
        }
        public HRegion GetSerchROI(Rectangle rec)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            return new HRegion((HTuple)rec.Y, rec.X, rec.Y + rec.Height, rec.X + rec.Width);
        }
    }
   
}           
            