using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionFlows;

namespace VisionFlows
{
    internal class VisionLocation
    {
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="path"></param>
        public static void SaveImage(HImage image, string path)
        {
            if (image == null || !image.IsInitialized())
                return;
            Task.Run(() =>
            {
                image.WriteImage("png", 0, path);
            });
        }
    }

    /// <summary>
    /// 视觉定位 参数传递类
    /// </summary>
    public class InputPara
    {
        public HRegion roi;
        /// <summary>
        /// 输入图片
        /// </summary>
        public HImage image;

        /// <summary>
        /// 输入 模板文件
        /// </summary>
        public HShapeModel shapeModel;


        public InputPara(HImage ima, HRegion roi,HShapeModel hShapeModel, double MinScore)
        {
            this.image = ima.ReduceDomain(roi);
            if(hShapeModel!=null)
            {
                this.shapeModel = hShapeModel;
            }
            else
            {
                this.shapeModel = null;
            }
           
        }

        public void Dispos()
        {
            this.image.Dispose();
            if(shapeModel!=null&& shapeModel.IsInitialized())
            {
                try
                {
                    HOperatorSet.ClearShapeModel(shapeModel);
                    shapeModel.Dispose();
                }
                catch
                {


                }
            }
            
           
        }
    }

    public class OutPutResult
    {
        /// <summary>
        /// 输出 仿射变换后 模板Xld
        /// </summary>
        public HXLDCont shapeModelContour;

        /// <summary>
        /// 计算出来的定位点
        /// </summary>
        public PointPosition_Image findPoint;

        /// <summary>
        /// 拟合出来的角度（在图像中）
        /// </summary>
        public double Phi;

        /// <summary>
        /// 输出 最小外接矩形 的xld对象
        /// </summary>
        public HObject SmallestRec2Xld;

        /// <summary>
        /// 输出 区域
        /// </summary>
        public HRegion region;

     
        /// <summary>
        /// 模板匹配得分
        /// </summary>
        public double Score;

        /// <summary>
        /// 是否读码 成功
        /// </summary>
        public bool IsReadDataCode;

        /// <summary>
        /// 是否运行正常
        /// </summary>
        public bool IsRunOk = false;
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrString;
        public string DatacodeString;

        //dut宽高
        public int Dutwidth;

        public int Dutheight;

        public OutPutResult()
        {
            shapeModelContour = new HXLDCont();
            SmallestRec2Xld = new HXLDCont();
            region = new HRegion();
        }

        public void Dispos()
        {
            if(shapeModelContour!=null&& shapeModelContour.IsInitialized())
            {
                shapeModelContour.Dispose();
            }
            if (SmallestRec2Xld != null && SmallestRec2Xld.IsInitialized())
            {
                SmallestRec2Xld.Dispose();
            }
            if (region != null && region.IsInitialized())
            {
                region.Dispose();
            }
        }
    }

    /// <summary>
    /// 单个点位置 图像 数据类
    /// </summary>
    public class PointPosition_Image
    {
        /// <summary>
        /// 单个点的Row坐标
        /// </summary>
        public double Row;

        /// <summary>
        /// 单个点的Column坐标
        /// </summary>
        public double Column;

        public PointPosition_Image()
        {
            this.Row = 0;
            this.Column = 0;
        }

        public PointPosition_Image(double valRow, double valColumn)
        {
            this.Row = valRow;
            this.Column = valColumn;
        }
    }
}