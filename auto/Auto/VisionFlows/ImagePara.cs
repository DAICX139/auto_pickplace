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
        public double SlotExposeTime;

        public double SoketDutExposeTime;
        public double DutExposeTime;
        public double SoketMarkExposeTime;
        public double DutBackExposeTime;

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

        //socket mark中心
        public float[] DutMode_rowCenter;

        public float[] DutMode_colCenter;
        public float[] DutMode_angleCenter;

        //自定义的放料中心
        public float[] SocketMark_GetDutRow;

        public float[] SocketMark_GetDutCol;

        //Slot模板坐标
        public float slot_rowCenter;

        public float slot_colCenter;
        public float slot_angleCenter;

        //slot自定义放料中心
        public float slot_offe_rowCenter;

        public float slot_offe_colCenter;
        public float slot_offe_angleCenter;

        public int PushBlock;

        //搜索区域
        public List<Rectangle> SearchROI;
        public ImagePara()
        {
            SlotScore = 0.1f;
            SoketDutScore = 0.6f;
            DutScore = 0.6f;
            DutBackScore = 0.8f;
            SocketMarkScore = 0.9f;
            SoketDut_minthreshold=new int[5];
            SoketDut_maxthreshold = new int[5];
            SoketDut_widthmin = 200;
            SoketDut_widthmax = 400;
            SoketDut_heightmin = 600;
            SoketDut_heightmax = 1000;

            SocketMark_rowCenter = 0;
            SocketMark_colCenter = 0;
            SocketMark_angleCenter = 0;
            SocketMark_PutDutRow = 0;
            SocketMark_PutDutCol = 0;

            SlotExposeTime = 8000;
            SoketDutExposeTime = 8000;
            DutExposeTime = 8000;
            SoketMarkExposeTime = 8000;
            DutBackExposeTime = 8000;

            SoketMark_minthreshold = 125;
            SoketMark_maxthreshold = 255;
            SearchROI = new List<Rectangle>();
            return;
            SearchROI.Add(new Rectangle(268, 370, 2525, 3216));
            SearchROI.Add(new Rectangle(268, 370, 2525, 3216));
            SearchROI.Add(new Rectangle(268, 370, 2525, 3216));
            SearchROI.Add(new Rectangle(268, 370, 2525, 3216));
            SearchROI.Add(new Rectangle(268, 370, 2525, 3216));
           
        }
        public HRegion GetSerchROI(string ROIname)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            if (SearchROI.Count < 5)
            {
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
            }
            HRegion roi=new HRegion();
            switch (ROIname)
            {
                case "SlotROI":
                    roi.GenRectangle1( (HTuple)SearchROI[0].Y, SearchROI[0].X, 
                        SearchROI[0].Height+ SearchROI[0].Y, SearchROI[0].Width + SearchROI[0].X);
                    break;
                case "SlotDutROI":
                    roi.GenRectangle1((HTuple)SearchROI[1].Y, (HTuple)SearchROI[1].X,
                        (HTuple)SearchROI[1].Height + (HTuple)SearchROI[1].Y, (HTuple)SearchROI[1].Width + (HTuple)SearchROI[1].X);
                    break;
                case "DutBackROI":
                    roi.GenRectangle1( (HTuple)SearchROI[2].Y, SearchROI[2].X,
                        SearchROI[2].Height + SearchROI[2].Y, SearchROI[2].Width + SearchROI[2].X);
                    break;
                case "SocketMarkROI":
                    roi.GenRectangle1((HTuple)SearchROI[3].Y, SearchROI[3].X,
                       SearchROI[3].Height + SearchROI[3].Y, SearchROI[3].Width + SearchROI[3].X);
                    break;
                case "SocketDutROI":
                    roi.GenRectangle1((HTuple)SearchROI[4].Y, SearchROI[4].X,
                        SearchROI[4].Height + SearchROI[4].Y, SearchROI[4].Width + SearchROI[4].X);
                    break;
                default:
                    roi.GenRectangle1((HTuple)SearchROI[0].Y, SearchROI[0].X,
                       SearchROI[0].Height + SearchROI[0].Y, SearchROI[0].Width + SearchROI[0].X);
                    break;
            }
            if(roi.Area<2000000)
            {
                
                roi.GenRectangle1((HTuple)268, 370, 3204, 2538);
            }
            return roi;
        }
        public void GetSerchROI(string ROIname, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            if (SearchROI.Count < 5)
            {
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
            }
            HRegion roi = new HRegion();
            switch (ROIname)
            {
                case "SlotROI":
                    row1 = SearchROI[0].Y;
                    col1= SearchROI[0].X;
                    row2= SearchROI[0].Y+ SearchROI[0].Height;
                    col2=SearchROI[0].X+ SearchROI[0].Width;
                    break;
                case "SlotDutROI":
                    row1 = SearchROI[1].Y;
                    col1 = SearchROI[1].X;
                    row2 = SearchROI[1].Y + SearchROI[1].Height;
                    col2 = SearchROI[1].X + SearchROI[1].Width;
                    break;
                case "DutBackROI":
                    row1 = SearchROI[2].Y;
                    col1 = SearchROI[2].X;
                    row2 = SearchROI[2].Y + SearchROI[2].Height;
                    col2 = SearchROI[2].X + SearchROI[2].Width;
                    break;
                case "SocketMarkROI":
                    row1 = SearchROI[3].Y;
                    col1 = SearchROI[3].X;
                    row2 = SearchROI[3].Y + SearchROI[3].Height;
                    col2 = SearchROI[3].X + SearchROI[3].Width;
                    break;
                case "SocketDutROI":
                    row1 = SearchROI[4].Y;
                    col1 = SearchROI[4].X;
                    row2 = SearchROI[4].Y + SearchROI[4].Height;
                    col2 = SearchROI[4].X + SearchROI[4].Width;
                    break;
                default:
                    row1 = SearchROI[0].Y;
                    col1 = SearchROI[0].X;
                    row2 = SearchROI[0].Y + SearchROI[0].Height;
                    col2 = SearchROI[0].X + SearchROI[0].Width;
                    break;
            }

        }
        public void SetSerchROI(string ROIname,float X1, float Y1, float X2, float Y2)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            if (SearchROI.Count>5)
            {
                SearchROI = SearchROI.GetRange(0, 5);
            }
            if(SearchROI.Count<5)
            {
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
                SearchROI.Add(new Rectangle(268, 370, 3204, 2538));
            }
            switch (ROIname)
            {
                case "SlotROI":
                    SearchROI[0]=new Rectangle((int)X1, (int)Y1, (int)(X2 -X1), (int)(Y2 - Y1));
                    break;
                case "SlotDutROI":
                    SearchROI[1] = new Rectangle((int)X1, (int)Y1, (int)(X2 - X1), (int)(Y2 - Y1));
                    break;
                case "DutBackROI":
                    SearchROI[2] = new Rectangle((int)X1, (int)Y1, (int)(X2 - X1), (int)(Y2 - Y1));
                    break;
                case "SocketMarkROI":
                    SearchROI[3] = new Rectangle((int)X1, (int)Y1, (int)(X2 - X1), (int)(Y2 - Y1));
                    break;
                case "SocketDutROI":
                    SearchROI[4] = new Rectangle((int)X1, (int)Y1, (int)(X2 - X1), (int)(Y2 - Y1));
                    break;
                default:
                    SearchROI[0] = new Rectangle((int)X1, (int)Y1, (int)(X2 - X1), (int)(Y2 - Y1));
                    break;
            }

        }
    }
   
}           
            