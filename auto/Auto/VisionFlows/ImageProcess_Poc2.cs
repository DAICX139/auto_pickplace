using AlcUtility;
using HalconDotNet;
using Poc2Auto.Common;
using Poc2Auto.Model;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionFlows.VisionCalculate;
using VisionModules;
using VisionUtility;

namespace VisionFlows
{
    public class ImageProcess_Poc2:ImageProcessBase
    {
      

        /// <summary>
        /// Socket判断料有无
        /// </summary>
        /// <param name="ho_Image1"></param>
        /// <param name="ho_RegionTrans"></param>
        private static HTuple socket_ModelID = null;

        public override bool SocketDetect(HImage image, out HObject ho_RegionTrans)
        {
           
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            try
            {
                HRegion ho_SerchRoi = new HRegion();
                string SocketName = "SerchROI{0}.hobj";
                ho_SerchRoi.ReadObject(Utility.Model + string.Format(SocketName, CurrentSoket));
                FindSocketMark(image, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Phi, out HObject mark);
                HHomMat2D hv_HomMat2D = new HHomMat2D();
                hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.SocketGet_rowCenter[ImageProcessBase.CurrentSoket], ImagePara.Instance.SocketGet_colCenter[ImageProcessBase.CurrentSoket],
                    ImagePara.Instance.SocketGet_angleCenter[ImageProcessBase.CurrentSoket], hv_Row, hv_Column, hv_Phi);
                HRegion ho_RegionAffineTrans = hv_HomMat2D.AffineTransRegion(ho_SerchRoi, "nearest_neighbor");
                ho_SerchRoi.Dispose();
                HImage ho_ImageReduced = image.ReduceDomain(ho_RegionAffineTrans);
                //   HRegion ho_Regions = ho_ImageReduced.Threshold((HTuple)ImagePara.Instance.SoketDut_minthreshold[CurrentSoket], 255);
                //  ho_RegionTrans = ho_Regions.OpeningCircle(20d);
                //   HOperatorSet.AreaCenter(ho_RegionTrans, out HTuple area, out HTuple row, out HTuple colum);
                //   if (area.Length > 0 && area.D > 400000)
                //   {
                //有料
                //       return true;
                //    }
                //   return false;
                HOperatorSet.LocalThreshold(ho_ImageReduced, out HObject ho_Region, "adapted_std_deviation",
             "dark", "mask_size", 180);
                HOperatorSet.Connection(ho_Region, out HObject ho_ConnectedRegions);
                HOperatorSet.SelectShape(ho_ConnectedRegions, out HObject ho_SelectedRegions_area,
                    "area", "and", 10000, 80000);
                HOperatorSet.FillUp(ho_SelectedRegions_area, out HObject ho_RegionFillUp);

                HObject regionOPening = new HObject();
                regionOPening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionFillUp, out regionOPening, 25);
                HOperatorSet.Connection(regionOPening, out regionOPening);
                //HOperatorSet.Connection(ho_RegionFillUp, out HObject connectionRegion);
                //HOperatorSet.AreaCenter(connectionRegion, out HTuple are, out _, out _);
                HOperatorSet.SelectShape(regionOPening, out ho_RegionTrans, (((new HTuple("area")).TupleConcat(
                    "convexity")).TupleConcat("roundness")).TupleConcat("width"), "and", ((
                    (new HTuple(30000)).TupleConcat(0.7)).TupleConcat(0.7)).TupleConcat(200),
                    (((new HTuple(70000)).TupleConcat(2)).TupleConcat(2)).TupleConcat(400));
                HOperatorSet.CountObj(ho_RegionTrans, out HTuple hv_Number);
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_SelectedRegions_area.Dispose();
                ho_RegionFillUp.Dispose();

                // 添加 
                HOperatorSet.MedianImage(ho_ImageReduced, out HObject imageMean, "circle", 5, "mirrored");
                HOperatorSet.Emphasize(imageMean, out HObject imageEnphasize, 7, 7, 1);
                HOperatorSet.Threshold(imageEnphasize, out HObject region, 240, 255);
                HOperatorSet.ClosingCircle(region, out HObject regionClosing, 21);
                HOperatorSet.OpeningRectangle1(regionClosing, out HObject regionOpening, 21, 21);
                HOperatorSet.Connection(regionOpening, out HObject regionConnection);
                HOperatorSet.AreaCenter(regionConnection, out HTuple area, out _, out _);
                HOperatorSet.SelectShape(regionConnection, out HObject regionSelect,
                    new HTuple("area", "width", "height"), "and", new HTuple(10000, 181, 90), new HTuple(99332, 853, 800));
                imageMean.Dispose();
                imageEnphasize.Dispose();
                region.Dispose();
                regionClosing.Dispose();
                regionConnection.Dispose();
                regionOpening.Dispose();
                ho_ImageReduced.Dispose();
                regionOPening.Dispose();

                HOperatorSet.GenEmptyObj(out HObject emptyObject);
                HOperatorSet.TestEqualObj(regionSelect, emptyObject, out HTuple i);
                // 当 i 表示，当 RegionSelect 为emptyObject时，返回 1.其余返回 0
                // 1: 无料
                // 0: 有料


                if ((int)(new HTuple(hv_Number.TupleGreater(0))) != 0 && i.I != 0)
                {
                    //无料
                    ho_RegionTrans = ho_RegionTrans.ConcatObj(regionSelect);
                    emptyObject.Dispose();
                    regionSelect.Dispose();
                    
                    //GC.Collect();
                    return false;
                }
                else
                {
                    ho_RegionTrans = ho_RegionTrans.ConcatObj(regionSelect);
                    emptyObject.Dispose();
                    regionSelect.Dispose();                    
                    //GC.Collect();
                    return true;
                    //有料
                }
            }
            catch (Exception ex)
            {
                Flow.Log(ex.Message + " + " + ex.StackTrace);
                //GC.Collect();
                return true;
            }
        }

        /// <summary>
        /// 判断slot产品有无
        /// </summary>
        /// <param name="ho_Image1"></param>
        /// <param name="ho_RegionTrans"></param>
        /// <returns></returns>
        private static HTuple slot_ModelID = null;

     
        public override bool SlotDetect(InputPara parameter, out HObject ho_RegionTrans)
        {
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            try
            {
                //GC.Collect();
                HOperatorSet.MedianImage(parameter.image, out HObject ho_ImageMedian, "circle", 10, "mirrored");
                HOperatorSet.MeanImage(ho_ImageMedian, out HObject ho_ImageMean, 600, 600);
                HOperatorSet.DynThreshold(ho_ImageMedian, ho_ImageMean, out HObject ho_RegionDynThresh,10, "dark");
                HOperatorSet.ClosingCircle(ho_RegionDynThresh, out HObject ho_RegionClosing, 10);
                HOperatorSet.FillUpShape(ho_RegionClosing, out HObject ho_RegionFillUp, "area", 1000,1000000);
                HOperatorSet.Connection(ho_RegionFillUp, out HObject ho_ConnectedRegions);
                HOperatorSet.SelectShape(ho_ConnectedRegions, out HObject ho_SelectedRegions1, (new HTuple("area")).TupleConcat(
                    "convexity"), "and", (new HTuple(2168630)).TupleConcat(0.83953), (new HTuple(3668630)).TupleConcat(
                    2));
                HOperatorSet.GenEmptyObj(out HObject ho_EmptyObject);
                HOperatorSet.TestEqualObj(ho_EmptyObject, ho_SelectedRegions1, out HTuple hv_IsEqual);
                ho_ImageMedian.Dispose();
                ho_ImageMean.Dispose();
                ho_RegionDynThresh.Dispose();
                ho_RegionFillUp.Dispose();
                ho_RegionClosing.Dispose();
                ho_ConnectedRegions.Dispose();
                if ((int)(new HTuple(hv_IsEqual.TupleEqual(1))) != 0)
                {
                    ho_SelectedRegions1.Dispose();
                    return false;
                    //定位异常
                }
                else
                {
                    HOperatorSet.OpeningRectangle1(ho_SelectedRegions1, out HObject ho_RegionOpening,130, 130);
                    HOperatorSet.ShapeTrans(ho_RegionOpening, out ho_RegionTrans, "convex");
                    HOperatorSet.ErosionCircle(ho_RegionTrans, out HObject ho_RegionErosion, 50);
                    HOperatorSet.SmallestRectangle2(ho_RegionErosion, out HTuple hv_Row, out HTuple hv_Column,
                        out HTuple hv_Phi, out HTuple hv_Length1, out HTuple hv_Length2);
                    HOperatorSet.ReduceDomain(parameter.image, ho_RegionErosion, out HObject ho_ImageReduced1);
                    HOperatorSet.Threshold(ho_ImageReduced1, out ho_RegionTrans, ImagePara.Instance.TrayDut_minthreshold, ImagePara.Instance.TrayDut_maxthreshold);
                    HOperatorSet.AreaCenter(ho_RegionTrans, out HTuple hv_Area, out HTuple hv_Row1, out HTuple hv_Column1);
                    ho_SelectedRegions1.Dispose();
                    ho_RegionTrans.Dispose();
                    ho_RegionOpening.Dispose();
                    ho_RegionErosion.Dispose();
                    if ((int)(new HTuple(hv_Area.TupleGreater(50000))) != 0)
                    {
                        ho_ImageReduced1.Dispose();
                        return true;
                    }
                    ho_ImageReduced1.Dispose();
                    return false;
                }
               
            }
            catch (Exception ex)
            {
                Flow.Log("检测：" + ex.Message + ex.StackTrace);
                return false;
            }
        }
        /// <summary>
        /// 测试推块距离
        /// </summary>
        /// <param name="markrow"></param>
        /// <param name="markcol"></param>
        /// <param name="parameter"></param>
        /// <param name="distance"></param>
        /// <param name="ho_RegionTrans"></param>
        /// <returns></returns>
        public override bool BlockDetect(HTuple markrow,HTuple markcol, InputPara parameter, out int distance,out HObject ho_RegionTrans)
        {
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            try
            {
                //GC.Collect();
                HOperatorSet.GenRectangle1(out HObject ho_ROI_0, 1231.1, 86.2638, 2740.01, 3646.38);
                
                HOperatorSet.ReduceDomain( parameter.image, ho_ROI_0, out HObject ho_ImageReduced);
               
                HOperatorSet.Threshold(ho_ImageReduced, out HObject ho_Region, 35, 255);
               
                HOperatorSet.FillUp(ho_Region, out HObject ho_RegionFillUp);
                
                HOperatorSet.Connection(ho_RegionFillUp, out HObject ho_ConnectedRegions);
                 
                HOperatorSet.SelectShape(ho_ConnectedRegions, out HObject ho_SelectedRegions, "area","and", 18000, 9999999);
                
                HOperatorSet.Union1(ho_SelectedRegions, out HObject ho_RegionUnion);
                
                HOperatorSet.ShapeTrans(ho_RegionUnion, out ho_RegionTrans, "convex");
                
                HOperatorSet.SmallestRectangle1(ho_RegionTrans, out HTuple hv_Row1, out HTuple hv_Column1,out HTuple hv_Row2, out HTuple hv_Column2);
                gen_arrow_contour_xld(out HObject ho_Arrow1, markrow, markcol, markrow, hv_Column2, 80, 80);
                gen_arrow_contour_xld(out HObject ho_Arrow2, markrow, hv_Column2, markrow, markcol, 80,80);
                HOperatorSet.ConcatObj(ho_Arrow1, ho_Arrow2, out ho_RegionTrans);
                ho_Arrow1.Dispose();
                ho_Arrow2.Dispose();
                ho_ROI_0.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionFillUp.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionUnion.Dispose();
                distance = (int)(hv_Column2.D - markcol.D);
               if ((hv_Column2.D-markcol)< ImagePara.Instance.PushBlock)
                {
                    //推块距离过小
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                distance = 0;
                Flow.Log(ex.Message + ex.StackTrace);
                return true;
            }
        }
        /// <summary>
        /// 定位Tray盘上 Mark点的位置
        /// </summary>
        /// <returns></returns>
        public override OutPutResult ShotSlotMark(InputPara parameter)
        {
            OutPutResult locationResult = new OutPutResult();
            try
            {
                locationResult = ShotSlot(parameter);
            }
            catch (Exception ex)
            {
                Flow.Log("定位 Tray盘上 Mark点时 失败");
                locationResult.IsRunOk = false;
                return locationResult;
            }
            return locationResult;
        }

        /// <summary>
        /// 定位 Tray盘中Dut位置 函数，返回值为 定位是否成功的标志位
        /// </summary>
        /// <param name="image"></param>
        /// <param name="shapeModel"></param>
        /// <param name="DutPosition"></param>
        /// <returns></returns>
        public override OutPutResult TrayDutFront(InputPara parameter)
        {
            OutPutResult locationResult = new OutPutResult();
            try
            {
                if (true)
                {
                    string[] selectparaname1 = { "area", "outer_radius" };
                    double[] selectpara11 = { 732143, 592.26 };
                    double[] selectpara12 = { 1.34405e+006, 1455.36 };

                    string[] selectparaname2 = { "width", "height" };
                    double[] selectpara21 = { 1810.95, 560 };
                    double[] selectpara22 = { 2258.54, 1000 };
                    try
                    {
                        HImage ImageScaled = parameter.image.ScaleImage(36.4286, -2769);
                        HImage MedianImage = ImageScaled.MedianImage("circle", 15, "mirrored");
                        HRegion Region = MedianImage.BinaryThreshold("max_separability", "light", out HTuple usedThreshold);
                        HRegion RegionClosing = Region.ClosingCircle(50.0);
                        HRegion ConnectedRegions = RegionClosing.Connection();

                        HRegion SelectedRegions = ConnectedRegions.SelectShape(selectparaname1, "and", selectpara11, selectpara12);
                        HRegion RegionFillUp = SelectedRegions.FillUp();
                        HRegion RegionOpening = RegionFillUp.OpeningCircle(20.0);
                        HRegion SelectedRegions1 = RegionOpening.SelectShape(selectparaname2, "and", selectpara21, selectpara22);
                        HRegion RegionTrans = SelectedRegions1.ShapeTrans("convex");
                        RegionTrans.SmallestRectangle2(out double row, out double col, out double phi, out double length1, out double length2);

                        int RegionNumber = SelectedRegions1.CountObj();
                        if (RegionNumber > 1)
                        {
                            locationResult.ErrString = "识别失败，RegionNumber>1";
                            locationResult.IsRunOk = false;
                        }

                        HRegion Rectangle = new HRegion();
                        Rectangle.GenRectangle2(row, col, phi, length1, length2);
                        Rectangle.AreaCenter(out double arearow, out double areacol);
                        HXLDCont ContourXld = new HXLDCont(Rectangle.GenContourRegionXld("border"));

                        HOperatorSet.TupleDeg(phi, out HTuple angle);
                        locationResult.Angle = angle.D;
                        locationResult.findPoint = new PointPosition_Image();
                        locationResult.findPoint.Row = arearow;
                        locationResult.findPoint.Column = areacol;
                        locationResult.SmallestRec2Xld = new HXLDCont(ContourXld);
                        locationResult.IsRunOk = true;

                        locationResult.region = RegionTrans;
                        locationResult.shapeModelContour = new HXLDCont();
                        locationResult.shapeModelContour.GenEmptyObj();
                        ImageScaled.Dispose();
                        MedianImage.Dispose();
                        Region.Dispose();
                        RegionClosing.Dispose();
                        ConnectedRegions.Dispose();
                        SelectedRegions.Dispose();
                        RegionFillUp.Dispose();
                        RegionOpening.Dispose();
                        SelectedRegions1.Dispose();

                        Rectangle.Dispose();
                        ContourXld.Dispose();
                    }
                    catch (Exception ex)
                    {
                        locationResult.ErrString = "图像处理失败";
                        Flow.Log("Tray定位DUT 失败 " + ex.Message + ex.StackTrace);
                        locationResult.IsRunOk = false;
                    }
                    return locationResult;
                }
                locationResult.IsRunOk = true;
                parameter.shapeModel.Dispose();

                return locationResult;
            }
            catch (Exception ex)
            {
                Flow.Log(ex.Message + ex.StackTrace);
                locationResult.IsRunOk = false;
                return locationResult;
            }
        }

        /// <summary>
        /// 定位空Tary盘的位置
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override OutPutResult ShotSlot(InputPara parameter)
        {
            OutPutResult locationResult = new OutPutResult();

            try
            {
                if (true)
                {
                    //GC.Collect();
                  
                    HOperatorSet.MedianImage(parameter.image, out HObject ho_ImageMedian, "circle", 10, "mirrored");
                    HOperatorSet.MeanImage(ho_ImageMedian, out HObject ho_ImageMean, 600, 600);
                    HOperatorSet.DynThreshold(ho_ImageMedian, ho_ImageMean, out HObject ho_RegionDynThresh, 10, "dark");
                    HOperatorSet.FillUpShape(ho_RegionDynThresh, out HObject ho_RegionFillUp, "area", 1000, 1000000);
                    HOperatorSet.Connection(ho_RegionFillUp, out HObject ho_ConnectedRegions);
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out HObject ho_SelectedRegions1, (new HTuple("area")).TupleConcat(
                        "convexity"), "and", (new HTuple(2168630)).TupleConcat(0.83953), (new HTuple(3668630)).TupleConcat(
                        2));
                    HOperatorSet.GenEmptyObj(out HObject ho_EmptyObject);
                    HOperatorSet.TestEqualObj(ho_EmptyObject, ho_SelectedRegions1, out HTuple hv_IsEqual);
                    ho_ImageMedian.Dispose();
                    ho_ImageMean.Dispose();
                    ho_RegionDynThresh.Dispose();
                    ho_RegionFillUp.Dispose();
                    ho_ConnectedRegions.Dispose();
                    if ((int)(new HTuple(hv_IsEqual.TupleEqual(1))) != 0)
                    {
                        locationResult.ErrString = "定位失败";
                        locationResult.IsRunOk = false;
                        return locationResult;
                        //定位异常
                    }

                    HOperatorSet.OpeningRectangle1(ho_SelectedRegions1, out HObject ho_RegionOpening, 130, 130);
                    HOperatorSet.ShapeTrans(ho_RegionOpening, out HObject ho_RegionTrans, "convex");
                    HOperatorSet.ErosionCircle(ho_RegionTrans, out HObject ho_RegionErosion, 50);
                    HOperatorSet.SmallestRectangle2(ho_RegionErosion, out HTuple hv_Row, out HTuple hv_Column,
                        out HTuple hv_Phi, out HTuple hv_Length1, out HTuple hv_Length2);
                    locationResult.findPoint = new PointPosition_Image(hv_Row, hv_Column);
                    locationResult.Angle = hv_Phi;
                    locationResult.shapeModelContour.GenRectangle2ContourXld(hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2);
                    locationResult.region = new HRegion();
                    locationResult.region.GenEmptyRegion();
   
                    //模板匹配轮廓的外切矩形
                    locationResult.SmallestRec2Xld.GenRectangle2ContourXld(hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2);
                    ho_RegionOpening.Dispose();
                    ho_RegionTrans.Dispose();
                    ho_RegionErosion.Dispose();
                }
                locationResult.IsRunOk = true;
            }
            catch (Exception ex)
            {
                locationResult.ErrString = "图像处理错误";
                string err = ex.Message;
                Flow.Log(ex.Message + ex.StackTrace);
                locationResult.IsRunOk = false;
            }
            return locationResult;
        }

        public static void FindSocktMark(HObject ho_Image, HTuple hv_ModelID, out HTuple hv_center_row,
              out HTuple hv_center_col, out HTuple hv_Angle1, out HTuple hv_max_row, out HTuple hv_max_col,
              out HTuple hv_min_row, out HTuple hv_min_col)
        {
            // Local iconic variables

            HObject ho_ModelContours1, ho_ContCircle1;

            // Local control variables

            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Angle = new HTuple(), hv_Score = new HTuple();
            HTuple hv_Model = new HTuple(), hv_Indices = new HTuple();
            HTuple hv_Length = new HTuple();
            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_ModelContours1);
            HOperatorSet.GenEmptyObj(out ho_ContCircle1);
            hv_center_row = new HTuple();
            hv_center_col = new HTuple();
            hv_Angle1 = new HTuple();
            hv_max_row = new HTuple();
            hv_max_col = new HTuple();
            hv_min_row = new HTuple();
            hv_min_col = new HTuple();
            HOperatorSet.Threshold(ho_Image, out HObject region, 0, 143);
            HOperatorSet.FillUp(region, out HObject regionFill);
            HOperatorSet.Connection(regionFill, out regionFill);
            HOperatorSet.SelectShapeStd(regionFill, out HObject regionmax, "max_area", 70);
            HOperatorSet.ReduceDomain(ho_Image, regionmax, out HObject imagereduced);
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_Row.Dispose(); hv_Column.Dispose(); hv_Angle.Dispose(); hv_Score.Dispose(); hv_Model.Dispose();
                HOperatorSet.FindShapeModels(imagereduced, hv_ModelID, (new HTuple(0)).TupleRad()
                    , (new HTuple(360)).TupleRad(), ImagePara.Instance.SocketMarkScore, 0, 0.1, "least_squares", 0, 0.9, out hv_Row,
                    out hv_Column, out hv_Angle, out hv_Score, out hv_Model);
            }
            regionmax.Dispose();
            region.Dispose();
            regionFill.Dispose();
            imagereduced.Dispose();
            ho_ModelContours1.Dispose();
            HOperatorSet.GetShapeModelContours(out ho_ModelContours1, hv_ModelID, 1);

            hv_Indices.Dispose();
            HOperatorSet.TupleSortIndex(hv_Column, out hv_Indices);
            hv_Length.Dispose();
            HOperatorSet.TupleLength(hv_Indices, out hv_Length);
            hv_max_row.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_max_row = hv_Row.TupleSelect(
                    hv_Indices.TupleSelect(hv_Length - 1));
            }
            hv_max_col.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_max_col = hv_Column.TupleSelect(
                    hv_Indices.TupleSelect(hv_Length - 1));
            }

            hv_min_row.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_min_row = hv_Row.TupleSelect(
                    hv_Indices.TupleSelect(0));
            }
            hv_min_col.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_min_col = hv_Column.TupleSelect(
                    hv_Indices.TupleSelect(0));
            }
            hv_center_row.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_center_row = (hv_max_row + hv_min_row) / 2;
            }
            hv_center_col.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_center_col = (hv_max_col + hv_min_col) / 2;
            }
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                ho_ContCircle1.Dispose();
                HOperatorSet.GenCircleContourXld(out ho_ContCircle1, hv_min_row.TupleConcat(hv_max_row),
                    hv_min_col.TupleConcat(hv_max_col), (new HTuple(111.382)).TupleConcat(111.382),
                    (new HTuple((new HTuple(0)).TupleRad())).TupleConcat((new HTuple(0)).TupleRad()
                    ), (new HTuple((new HTuple(360)).TupleRad())).TupleConcat((new HTuple(360)).TupleRad()
                    ), "positive", 1);
            }
            hv_Angle1.Dispose();
            HOperatorSet.AngleLl(0, 0, 0, 100, hv_min_row, hv_min_col, hv_max_row, hv_max_col,
                out hv_Angle1);

            ho_ModelContours1.Dispose();
            ho_ContCircle1.Dispose();

            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_Angle.Dispose();
            hv_Score.Dispose();
            hv_Model.Dispose();
            hv_Indices.Dispose();
            hv_Length.Dispose();

            return;
        }

        /// <summary>
        /// 新标定函数   定位Socket mark点上的
        /// </summary>
        /// <param name="image"></param>
        /// <param name="findPoint"></param>
        /// <returns></returns>
        public override OutPutResult SocketMark(InputPara locationPara, out HObject mark)
        {
            //给出定位中心点位 给出定位轮廓 给出角度计算直线
            OutPutResult locationResult = new OutPutResult();
            HOperatorSet.GenEmptyObj(out mark);
            try
            {
                FindSocketMark(locationPara.image, out HTuple row1, out HTuple colum1, out HTuple phi, out mark);
                if (colum1.Length > 0)
                {
                    HHomMat2D hv_HomMat2D = new HHomMat2D();
                    hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.SocketMark_rowCenter, ImagePara.Instance.SocketMark_colCenter, ImagePara.Instance.SocketMark_angleCenter, row1,
                        colum1, phi);
                    HTuple CenterRow = hv_HomMat2D.AffineTransPoint2d(ImagePara.Instance.SocketMark_PutDutRow, ImagePara.Instance.SocketMark_PutDutCol, out HTuple CenterCol);
                    locationResult.findPoint = new PointPosition_Image(CenterRow, CenterCol);
                    locationResult.Angle = phi;
                    locationResult.IsRunOk = true;
                }
                else
                {
                    locationResult.ErrString = "mark点定位失败";
                    locationResult.findPoint = new PointPosition_Image(0, 0);
                    locationResult.Angle = 0;
                    locationResult.IsRunOk = false;
                }
            }
            catch (Exception ex)
            {
                locationResult.ErrString = "图像处理失败";
                Flow.Log(ex.Message + ex.StackTrace);
                locationResult.IsRunOk = false;
            }
            return locationResult;
        }

        

        /// <summary>
        /// 定位 Socket中Dut位置 函数 （圆定位方式）
        /// </summary>
        /// <param name="image"></param>
        /// <param name="shapeModel"></param>
        /// <param name="DutPosition"></param>
        /// <returns></returns>
        public override OutPutResult SocketDutFront(InputPara parameter)
        {
            OutPutResult locationResult = new OutPutResult();

            try
            {
                HRegion ho_SerchRoi = new HRegion();
                string SocketName = "SerchROI{0}.hobj";
                ho_SerchRoi.ReadObject(Utility.Model + string.Format(SocketName,CurrentSoket));
                FindSocketMark(parameter.image, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Phi, out HObject mark);
                HHomMat2D hv_HomMat2D = new HHomMat2D();

                //HOperatorSet.GenCrossContourXld(out HObject cross, hv_Row, hv_Column, 1000, hv_Phi);   // 验证定位是否准
                hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.SocketGet_rowCenter[ImageProcessBase.CurrentSoket], 
                    ImagePara.Instance.SocketGet_colCenter[ImageProcessBase.CurrentSoket],
                    ImagePara.Instance.SocketGet_angleCenter[ImageProcessBase.CurrentSoket], hv_Row, hv_Column, hv_Phi);
                HRegion ho_RegionAffineTrans = hv_HomMat2D.AffineTransRegion(ho_SerchRoi, "nearest_neighbor");

                HImage ho_ImageReduced = parameter.image.ReduceDomain(ho_RegionAffineTrans);

                HRegion ho_Regions = ho_ImageReduced.Threshold((HTuple)ImagePara.Instance.SoketDut_minthreshold[CurrentSoket],
                    (HTuple)ImagePara.Instance.SoketDut_maxthreshold[CurrentSoket]);
                HRegion ho_RegionOpening = ho_Regions.OpeningCircle(15d);

                HRegion ho_RegionTrans = ho_RegionOpening.ShapeTrans("convex");

                HOperatorSet.SmallestRectangle2(ho_RegionTrans, out HTuple hv_Row1, out HTuple hv_Column1,
                    out HTuple hv_Phi1, out HTuple hv_Length1, out HTuple hv_Length2);
                HRegion ho_Rectangle = new HRegion();
                ho_Rectangle.GenRectangle2(hv_Row1, hv_Column1, hv_Phi1, hv_Length1, hv_Length2);
                ho_ImageReduced.Dispose();
                ho_Regions.Dispose();
                ho_RegionOpening.Dispose();
                ho_SerchRoi.Dispose();
                locationResult.Dutwidth = (int)hv_Length2.D;
                locationResult.Dutheight = (int)hv_Length1.D;
                if (hv_Length1.D > ImagePara.Instance.SoketDut_heightmin && hv_Length1.D < ImagePara.Instance.SoketDut_heightmax &&
                    hv_Length2.D > ImagePara.Instance.SoketDut_widthmin && hv_Length2.D < ImagePara.Instance.SoketDut_widthmax)

                {
                    //找到目标
                    locationResult.Angle = hv_Phi1;
                    locationResult.findPoint = new PointPosition_Image(hv_Row1.D, hv_Column1.D);
                    locationResult.region = ho_RegionTrans.ConcatObj(ho_RegionAffineTrans);
                    locationResult.SmallestRec2Xld = ho_Rectangle;
                    locationResult.IsRunOk = true;
                }
                else
                {
                    locationResult.IsRunOk = false;
                    locationResult.ErrString = "Dut长宽不满足!";
                }
                return locationResult;
            }
            catch (Exception ex)
            {
                locationResult.ErrString = "图像处理失败!";
                Flow.Log("Socket定位DUT 失败 " + ex.Message + ex.StackTrace);
                locationResult.IsRunOk = false;
            }
            return locationResult;
        }

        /// <summary>
        /// 提取Socket两个圆点的外接矩形
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <param name="hv_Row1"></param>
        /// <param name="hv_Column1"></param>
        /// <param name="hv_Phi"></param>
        /// <param name="mark"></param>
        public override void FindSocketMark(HObject ho_Image, out HTuple hv_Row1, out HTuple hv_Column1,
    out HTuple hv_Phi, out HObject mark)
        {
            // Local iconic variables

            HObject ho_Region, ho_ConnectedRegions, ho_SelectedRegions;
            HObject ho_SelectedRegions1, ho_Circle = null;

            // Local control variables

            HTuple hv_UsedThreshold = new HTuple(), hv_Number = new HTuple();
            HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
            HTuple hv_Radius = new HTuple(), hv_Length1 = new HTuple();
            HTuple hv_Length2 = new HTuple();
            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions1);
            HOperatorSet.GenEmptyObj(out ho_Circle);
            HOperatorSet.GenEmptyObj(out mark);
            hv_Row1 = new HTuple();
            hv_Column1 = new HTuple();
            hv_Phi = new HTuple();
            ho_Region.Dispose(); hv_UsedThreshold.Dispose();
            HOperatorSet.Threshold(ho_Image, out ho_Region, ImagePara.Instance.SoketMark_minthreshold, ImagePara.Instance.SoketMark_maxthreshold);
            //HOperatorSet.BinaryThreshold(ho_Image_white, out ho_Region, "max_separability",
            //    "light", out hv_UsedThreshold);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
            ho_SelectedRegions.Dispose();
            HOperatorSet.OpeningCircle(ho_ConnectedRegions, out ho_ConnectedRegions, 30);
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, (new HTuple("area")).TupleConcat(
                "circularity"), "and", (new HTuple(30000)).TupleConcat(0.700), (new HTuple(60000)).TupleConcat(
                2));

            //HOperatorSet.Connection(ho_SelectedRegions, out ho_SelectedRegions);
            //HOperatorSet.AreaCenter(ho_SelectedRegions, out HTuple areea, out _, out _);
            // 验证 定位过程产生的问题
            ho_SelectedRegions1.Dispose();
            HOperatorSet.SelectShape(ho_SelectedRegions, out ho_SelectedRegions1, "row",
                "and", 900, 1800);
            hv_Number.Dispose();
            HOperatorSet.CountObj(ho_SelectedRegions1, out hv_Number);
            if (hv_Number.D > 1)
            {
                HOperatorSet.Connection(ho_SelectedRegions1, out ho_ConnectedRegions);
                HOperatorSet.AreaCenter(ho_ConnectedRegions, out HTuple area, out HTuple Row, out HTuple Column);
                HOperatorSet.TupleSortIndex(Column, out HTuple index);
                HOperatorSet.SelectObj(ho_ConnectedRegions, out HObject objmin, index[0] + 1);
                HOperatorSet.SelectObj(ho_ConnectedRegions, out HObject objmax, index[index.Length - 1] + 1);
                HOperatorSet.Union2(objmin, objmax, out HObject ho_circles);

                ho_ConnectedRegions.Dispose();
                HOperatorSet.Union1(ho_circles, out ho_circles);

                hv_Row.Dispose(); hv_Column.Dispose(); hv_Radius.Dispose();
                HOperatorSet.OpeningCircle(ho_circles, out ho_SelectedRegions1, 3.5);
                HOperatorSet.Connection(ho_SelectedRegions1, out ho_SelectedRegions1);
                HOperatorSet.SmallestCircle(ho_SelectedRegions1, out hv_Row, out hv_Column,
                    out hv_Radius);
                ho_Circle.Dispose();
                HOperatorSet.GenCircle(out ho_Circle, hv_Row, hv_Column, hv_Radius);
                HOperatorSet.Union1(ho_Circle, out mark);
                hv_Row1.Dispose(); hv_Column1.Dispose(); hv_Phi.Dispose(); hv_Length1.Dispose(); hv_Length2.Dispose();
                HOperatorSet.SmallestRectangle2(mark, out hv_Row1, out hv_Column1,
                    out hv_Phi, out hv_Length1, out hv_Length2);
            }
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_SelectedRegions1.Dispose();
            ho_Circle.Dispose();

            hv_UsedThreshold.Dispose();
            hv_Number.Dispose();
            hv_Row.Dispose();
            hv_Column.Dispose();
            hv_Radius.Dispose();
            hv_Length1.Dispose();
            hv_Length2.Dispose();
        }

        /// <summary>
        /// 定位Dut 在吸嘴上的
        /// </summary>
        /// <param name="image"></param>
        /// <param name="DutCenter"></param>
        /// <param name="AngleLX"></param>
        /// <returns></returns>
        public override OutPutResult SecondDutBack(InputPara locationPara, PLCSend plcsend)
        {
            OutPutResult locationResult = new OutPutResult();

            try
            {
                HTuple modelRow, modelCol, angle, score;
                locationPara.shapeModel.FindShapeModel(locationPara.image, 0, Math.PI, ImagePara.Instance.DutBackScore, 0, 0.5, "least_squares", 0, 0.9,
                    out modelRow, out modelCol, out angle, out score);
                if (score.Length != 5)
                {
                    for (int i = -3; i < 6; i++)
                    {
                        HImage SaleImage = locationPara.image.ScaleImage((HTuple)1, i * 10);
                        locationPara.shapeModel.FindShapeModel(SaleImage, 0, Math.PI, ImagePara.Instance.DutBackScore, 0, 0.5, "least_squares", 0, 0.9,
                   out modelRow, out modelCol, out angle, out score);
                        SaleImage.Dispose();
                        if (score.Length == 5)
                        {
                            break;
                        }
                    }
                }
                if (score.Length != 5)
                {
                    locationResult.ErrString = "找圆失败!,个数不满足！";
                    locationResult.IsRunOk = false;
                    return locationResult;
                }

                HTuple Indices = modelCol.TupleSortIndex();
                HTuple Length = Indices.TupleLength();
                double MaxRow = modelRow[(int)Indices[Length - 1]];
                double MaxCol = modelCol[(int)Indices[Length - 1]];
                double MinRow = modelRow[(int)Indices[0]];
                double MinCol = modelCol[(int)Indices[0]];
                double CenterRow = (MaxRow + MinRow) / 2;
                double CenterCol = (MaxCol + MinCol) / 2;
                HOperatorSet.AngleLl(0, 0, 0, 100, MinRow, MinCol, MaxRow, MaxCol, out HTuple Angle);
                HXLDCont CircleContourXld = new HXLDCont();

                HTuple RowList = new HTuple();
                RowList[0] = MinRow;
                RowList[1] = MaxRow;
                HTuple ColList = new HTuple();
                ColList[0] = MinCol;
                ColList[1] = MaxCol;
                HTuple RadiusList = new HTuple();
                RadiusList[0] = 163;
                RadiusList[1] = 163;
                HTuple StartAngleList = new HTuple();
                StartAngleList[0] = 0;
                StartAngleList[1] = 0;
                HTuple EndAngleList = new HTuple();
                EndAngleList[0] = Math.PI * 2;
                EndAngleList[1] = Math.PI * 2;
                CircleContourXld.GenCircleContourXld(RowList, ColList, RadiusList, StartAngleList, EndAngleList, (HTuple)"positive", 2);

                //创建读码 ROI
                HRegion ReadDataCodeRegion = new HRegion();
                ReadDataCodeRegion.GenRectangle2(CenterRow, CenterCol, 0, 800, 500);

                HHomMat2D hHomMat2D = new HHomMat2D();
                hHomMat2D = hHomMat2D.HomMat2dTranslate(222, 307.0);
                hHomMat2D = hHomMat2D.HomMat2dRotate(Angle.D, CenterRow, CenterCol);

                ReadDataCodeRegion = hHomMat2D.AffineTransRegion(ReadDataCodeRegion, "nearest_neighbor");
                //裁剪读码 图片
                HImage ReadCodeImage = locationPara.image.ReduceDomain(ReadDataCodeRegion);
                locationResult.IsReadDataCode = ReadDataCode(ReadCodeImage, out locationResult.DatacodeString, out HXLDCont RecDataCode);
                locationResult.SmallestRec2Xld = RecDataCode; //扫码的矩形框 利用形状模板xld给出去
                locationResult.region = ReadDataCodeRegion; //裁剪读码 图片区域 给出去

                locationResult.findPoint = new PointPosition_Image(CenterRow, CenterCol);
                locationResult.Angle = Angle;
                locationResult.shapeModelContour = CircleContourXld;
                locationResult.IsRunOk = true;
            }
            catch (Exception ex)
            {
                locationResult.ErrString = "图像处理错误";
                //定位吸嘴上的Dut 失败
                Flow.Log("定位吸嘴上的Dut 失败 " + ex.Message + ex.StackTrace);
                locationResult.IsRunOk = false;
            }
            return locationResult;
        }

        
       

      

        public static bool create_shape(HImage image, HWindow wind, HRegion ROI, HTuple MinAngle, HTuple MaxAngle,
   ref HShapeModel hv_ModelID, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Angle, out HObject origin_conturs)
        {
            try
            {
                HImage ho_ImageReduced;
                ho_ImageReduced = image.ReduceDomain(ROI);
                hv_ModelID = new HShapeModel();
                hv_ModelID.CreateShapeModel(ho_ImageReduced, "auto", (new HTuple(MinAngle)).TupleRad(), (new HTuple(MaxAngle)).TupleRad(),
                    "auto", "auto", "use_polarity", "auto", "auto");
                // 重新找模板
                hv_ModelID.FindShapeModel(image, (new HTuple(MinAngle)).TupleRad(),
                   (new HTuple(MaxAngle)).TupleRad(), 0.6, 1, 0.5, "least_squares_high", 0, 0.7, out hv_Row, out hv_Column, out hv_Angle, out HTuple hv_Score);
                HObject ho_ModelContours;
                HOperatorSet.GenEmptyObj(out ho_ModelContours);
                //获取模板轮廓
                ho_ModelContours = hv_ModelID.GetShapeModelContours(1);
                //变换到目标物体上2
                HTuple hv_HomMat2DIdentity, hv_HomMat2DTranslate, hv_HomMat2DRotate;
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity, hv_Row, hv_Column, out hv_HomMat2DTranslate);
                HOperatorSet.HomMat2dRotate(hv_HomMat2DTranslate, hv_Angle, hv_Row, hv_Column,
                    out hv_HomMat2DRotate);
                HOperatorSet.GenEmptyObj(out origin_conturs);
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out origin_conturs, hv_HomMat2DRotate);
                HOperatorSet.SetColor(wind, "red");
                HOperatorSet.DispObj(origin_conturs, wind);
                return true;
            }
            catch (Exception eee)
            {
                MessageBox.Show("创建模板失败！   " + eee.Message);
                hv_ModelID = null;
                hv_Row = null;
                hv_Column = null;
                hv_Angle = null;
                HOperatorSet.GenEmptyObj(out origin_conturs);
                return false;
            }
        }

        public static HRegion drawmodel(HWindow wind, int shape)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            HRegion ROI = new HRegion();
            HOperatorSet.SetLineWidth(wind, 2);
            HOperatorSet.SetColor(wind, "red");
            HOperatorSet.SetTposition(wind, 10, 10);
            HOperatorSet.WriteString(wind, "请手动创建模型区域，鼠标右键结束");
            //  HOperatorSet.GenEmptyObj(out ROI);
            if (shape == 2)
            {
                HTuple row, column, radio;
                HOperatorSet.DrawCircle(wind, out row, out column, out radio);
                ROI.GenCircle(row, column, radio);
                // HOperatorSet.DispObj(ROI, AppInstance.testwind.HalconWindow);
            }
            else if (shape == 1)
            {
                HTuple row, column, phi, length1, length2;
                HOperatorSet.DrawRectangle2(wind, out row, out column, out phi, out length1, out length2);
                ROI.GenRectangle2(row, column, phi, length1, length2);
                // HOperatorSet.DispObj(ROI, AppInstance.testwind.HalconWindow);
            }
            return ROI;
        }

        public static bool Find_Shape(HImage image, HRegion ROI, HShapeModel hv_ModelID, HTuple numlevel, HTuple minscore, HTuple NumMatch, HTuple MaxOverLop, HTuple MinAngle, HTuple MaxAngle, out HTuple hv_Score, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Angle)
        {
            HImage image_reduce = new HImage();
            try
            {
                if (ROI == null)
                {
                    ROI = new HRegion();
                    ROI = image.GetDomain();
                }

                image_reduce = image.ReduceDomain(ROI);
                //找模板
                hv_ModelID.FindShapeModel(image_reduce, MinAngle.TupleRad(),
                   (new HTuple(MaxAngle)).TupleRad(), minscore, NumMatch, MaxOverLop, (HTuple)"least_squares_high", (HTuple)0, (double)0.7, out hv_Row, out hv_Column, out hv_Angle, out hv_Score);
                image_reduce.Dispose();
                if ((int)(new HTuple((new HTuple(hv_Score.TupleLength())).TupleGreater(0))) != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                image_reduce.Dispose();
                hv_Score = null;
                hv_Row = null;
                hv_Column = null;
                hv_Angle = null;
                return false;
            }
        }

        public static void get_Calibcenter_nozzle(HRegion ho_ROI_0, HObject image, out HTuple hv_Row,
    out HTuple hv_Column, out HTuple hv_angle, out HObject ho_Rectangle)
        {
            // Local iconic variables

            HObject ho_ImageReduced, ho_Region;
            HObject ho_ConnectedRegions, ho_SelectedRegions, ho_RegionUnion;

            // Local control variables

            HTuple hv_UsedThreshold = new HTuple(), hv_Phi = new HTuple();
            HTuple hv_Length1 = new HTuple(), hv_Length2 = new HTuple();
            // Initialize local and output iconic variables
            HOperatorSet.GenEmptyObj(out ho_ImageReduced);
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionUnion);
            HOperatorSet.GenEmptyObj(out ho_Rectangle);
            hv_Row = new HTuple();
            hv_Column = new HTuple();
            hv_angle = new HTuple();
            ho_ImageReduced.Dispose();
            HOperatorSet.ReduceDomain(image, ho_ROI_0, out ho_ImageReduced);
            ho_Region.Dispose(); hv_UsedThreshold.Dispose();
            HOperatorSet.BinaryThreshold(ho_ImageReduced, out ho_Region, "max_separability",
                "dark", out hv_UsedThreshold);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                "and", 150, 99999999);
            ho_RegionUnion.Dispose();
            HOperatorSet.Union1(ho_SelectedRegions, out ho_RegionUnion);
            hv_Row.Dispose(); hv_Column.Dispose(); hv_Phi.Dispose(); hv_Length1.Dispose(); hv_Length2.Dispose();
            HOperatorSet.SmallestRectangle2(ho_RegionUnion, out hv_Row, out hv_Column, out hv_Phi,
                out hv_Length1, out hv_Length2);
            ho_Rectangle.Dispose();
            HOperatorSet.GenRectangle2(out ho_Rectangle, hv_Row, hv_Column, hv_Phi, hv_Length1,
                hv_Length2);
            hv_angle.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_angle = hv_Phi.TupleDeg();
            }

            ho_ImageReduced.Dispose();
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionUnion.Dispose();

            hv_UsedThreshold.Dispose();
            hv_Phi.Dispose();
            hv_Length1.Dispose();
            hv_Length2.Dispose();
        }

        public static void list_image_files(HTuple hv_ImageDirectory, HTuple hv_Extensions, HTuple hv_Options,
    out HTuple hv_ImageFiles)
        {
            // Local iconic variables

            // Local control variables

            HTuple hv_ImageDirectoryIndex = new HTuple();
            HTuple hv_ImageFilesTmp = new HTuple(), hv_CurrentImageDirectory = new HTuple();
            HTuple hv_HalconImages = new HTuple(), hv_OS = new HTuple();
            HTuple hv_Directories = new HTuple(), hv_Index = new HTuple();
            HTuple hv_Length = new HTuple(), hv_NetworkDrive = new HTuple();
            HTuple hv_Substring = new HTuple(), hv_FileExists = new HTuple();
            HTuple hv_AllFiles = new HTuple(), hv_i = new HTuple();
            HTuple hv_Selection = new HTuple();
            HTuple hv_Extensions_COPY_INP_TMP = new HTuple(hv_Extensions);

            // Initialize local and output iconic variables
            hv_ImageFiles = new HTuple();
            //This procedure returns all files in a given directory
            //with one of the suffixes specified in Extensions.
            //
            //Input parameters:
            //ImageDirectory: Directory or a tuple of directories with images.
            //   If a directory is not found locally, the respective directory
            //   is searched under %HALCONIMAGES%/ImageDirectory.
            //   See the Installation Guide for further information
            //   in case %HALCONIMAGES% is not set.
            //Extensions: A string tuple containing the extensions to be found
            //   e.g. ['png','tif',jpg'] or others
            //If Extensions is set to 'default' or the empty string '',
            //   all image suffixes supported by HALCON are used.
            //Options: as in the operator list_files, except that the 'files'
            //   option is always used. Note that the 'directories' option
            //   has no effect but increases runtime, because only files are
            //   returned.
            //
            //Output parameter:
            //ImageFiles: A tuple of all found image file names
            //
            if ((int)((new HTuple((new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(new HTuple()))).TupleOr(
                new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(""))))).TupleOr(new HTuple(hv_Extensions_COPY_INP_TMP.TupleEqual(
                "default")))) != 0)
            {
                hv_Extensions_COPY_INP_TMP.Dispose();
                hv_Extensions_COPY_INP_TMP = new HTuple();
                hv_Extensions_COPY_INP_TMP[0] = "ima";
                hv_Extensions_COPY_INP_TMP[1] = "tif";
                hv_Extensions_COPY_INP_TMP[2] = "tiff";
                hv_Extensions_COPY_INP_TMP[3] = "gif";
                hv_Extensions_COPY_INP_TMP[4] = "bmp";
                hv_Extensions_COPY_INP_TMP[5] = "jpg";
                hv_Extensions_COPY_INP_TMP[6] = "jpeg";
                hv_Extensions_COPY_INP_TMP[7] = "jp2";
                hv_Extensions_COPY_INP_TMP[8] = "jxr";
                hv_Extensions_COPY_INP_TMP[9] = "png";
                hv_Extensions_COPY_INP_TMP[10] = "pcx";
                hv_Extensions_COPY_INP_TMP[11] = "ras";
                hv_Extensions_COPY_INP_TMP[12] = "xwd";
                hv_Extensions_COPY_INP_TMP[13] = "pbm";
                hv_Extensions_COPY_INP_TMP[14] = "pnm";
                hv_Extensions_COPY_INP_TMP[15] = "pgm";
                hv_Extensions_COPY_INP_TMP[16] = "ppm";
                //
            }
            hv_ImageFiles.Dispose();
            hv_ImageFiles = new HTuple();
            //Loop through all given image directories.
            for (hv_ImageDirectoryIndex = 0; (int)hv_ImageDirectoryIndex <= (int)((new HTuple(hv_ImageDirectory.TupleLength()
                )) - 1); hv_ImageDirectoryIndex = (int)hv_ImageDirectoryIndex + 1)
            {
                hv_ImageFilesTmp.Dispose();
                hv_ImageFilesTmp = new HTuple();
                hv_CurrentImageDirectory.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_CurrentImageDirectory = hv_ImageDirectory.TupleSelect(
                        hv_ImageDirectoryIndex);
                }
                if ((int)(new HTuple(hv_CurrentImageDirectory.TupleEqual(""))) != 0)
                {
                    hv_CurrentImageDirectory.Dispose();
                    hv_CurrentImageDirectory = ".";
                }
                hv_HalconImages.Dispose();
                HOperatorSet.GetSystem("image_dir", out hv_HalconImages);
                hv_OS.Dispose();
                HOperatorSet.GetSystem("operating_system", out hv_OS);
                if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_HalconImages = hv_HalconImages.TupleSplit(
                                ";");
                            hv_HalconImages.Dispose();
                            hv_HalconImages = ExpTmpLocalVar_HalconImages;
                        }
                    }
                }
                else
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_HalconImages = hv_HalconImages.TupleSplit(
                                ":");
                            hv_HalconImages.Dispose();
                            hv_HalconImages = ExpTmpLocalVar_HalconImages;
                        }
                    }
                }
                hv_Directories.Dispose();
                hv_Directories = new HTuple(hv_CurrentImageDirectory);
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_HalconImages.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        {
                            HTuple
                              ExpTmpLocalVar_Directories = hv_Directories.TupleConcat(
                                ((hv_HalconImages.TupleSelect(hv_Index)) + "/") + hv_CurrentImageDirectory);
                            hv_Directories.Dispose();
                            hv_Directories = ExpTmpLocalVar_Directories;
                        }
                    }
                }
                hv_Length.Dispose();
                HOperatorSet.TupleStrlen(hv_Directories, out hv_Length);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_NetworkDrive.Dispose();
                    HOperatorSet.TupleGenConst(new HTuple(hv_Length.TupleLength()), 0, out hv_NetworkDrive);
                }
                if ((int)(new HTuple(((hv_OS.TupleSubstr(0, 2))).TupleEqual("Win"))) != 0)
                {
                    for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength()
                        )) - 1); hv_Index = (int)hv_Index + 1)
                    {
                        if ((int)(new HTuple(((((hv_Directories.TupleSelect(hv_Index))).TupleStrlen()
                            )).TupleGreater(1))) != 0)
                        {
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_Substring.Dispose();
                                HOperatorSet.TupleStrFirstN(hv_Directories.TupleSelect(hv_Index), 1,
                                    out hv_Substring);
                            }
                            if ((int)((new HTuple(hv_Substring.TupleEqual("//"))).TupleOr(new HTuple(hv_Substring.TupleEqual(
                                "\\\\")))) != 0)
                            {
                                if (hv_NetworkDrive == null)
                                    hv_NetworkDrive = new HTuple();
                                hv_NetworkDrive[hv_Index] = 1;
                            }
                        }
                    }
                }
                hv_ImageFilesTmp.Dispose();
                hv_ImageFilesTmp = new HTuple();
                for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Directories.TupleLength()
                    )) - 1); hv_Index = (int)hv_Index + 1)
                {
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        hv_FileExists.Dispose();
                        HOperatorSet.FileExists(hv_Directories.TupleSelect(hv_Index), out hv_FileExists);
                    }
                    if ((int)(hv_FileExists) != 0)
                    {
                        using (HDevDisposeHelper dh = new HDevDisposeHelper())
                        {
                            hv_AllFiles.Dispose();
                            HOperatorSet.ListFiles(hv_Directories.TupleSelect(hv_Index), (new HTuple("files")).TupleConcat(
                                hv_Options), out hv_AllFiles);
                        }
                        hv_ImageFilesTmp.Dispose();
                        hv_ImageFilesTmp = new HTuple();
                        for (hv_i = 0; (int)hv_i <= (int)((new HTuple(hv_Extensions_COPY_INP_TMP.TupleLength()
                            )) - 1); hv_i = (int)hv_i + 1)
                        {
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                hv_Selection.Dispose();
                                HOperatorSet.TupleRegexpSelect(hv_AllFiles, (((".*" + (hv_Extensions_COPY_INP_TMP.TupleSelect(
                                    hv_i))) + "$")).TupleConcat("ignore_case"), out hv_Selection);
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                {
                                    HTuple
                                      ExpTmpLocalVar_ImageFilesTmp = hv_ImageFilesTmp.TupleConcat(
                                        hv_Selection);
                                    hv_ImageFilesTmp.Dispose();
                                    hv_ImageFilesTmp = ExpTmpLocalVar_ImageFilesTmp;
                                }
                            }
                        }
                        {
                            HTuple ExpTmpOutVar_0;
                            HOperatorSet.TupleRegexpReplace(hv_ImageFilesTmp, (new HTuple("\\\\")).TupleConcat(
                                "replace_all"), "/", out ExpTmpOutVar_0);
                            hv_ImageFilesTmp.Dispose();
                            hv_ImageFilesTmp = ExpTmpOutVar_0;
                        }
                        if ((int)(hv_NetworkDrive.TupleSelect(hv_Index)) != 0)
                        {
                            {
                                HTuple ExpTmpOutVar_0;
                                HOperatorSet.TupleRegexpReplace(hv_ImageFilesTmp, (new HTuple("//")).TupleConcat(
                                    "replace_all"), "/", out ExpTmpOutVar_0);
                                hv_ImageFilesTmp.Dispose();
                                hv_ImageFilesTmp = ExpTmpOutVar_0;
                            }
                            using (HDevDisposeHelper dh = new HDevDisposeHelper())
                            {
                                {
                                    HTuple
                                      ExpTmpLocalVar_ImageFilesTmp = "/" + hv_ImageFilesTmp;
                                    hv_ImageFilesTmp.Dispose();
                                    hv_ImageFilesTmp = ExpTmpLocalVar_ImageFilesTmp;
                                }
                            }
                        }
                        else
                        {
                            {
                                HTuple ExpTmpOutVar_0;
                                HOperatorSet.TupleRegexpReplace(hv_ImageFilesTmp, (new HTuple("//")).TupleConcat(
                                    "replace_all"), "/", out ExpTmpOutVar_0);
                                hv_ImageFilesTmp.Dispose();
                                hv_ImageFilesTmp = ExpTmpOutVar_0;
                            }
                        }
                        break;
                    }
                }
                //Concatenate the output image paths.
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    {
                        HTuple
                          ExpTmpLocalVar_ImageFiles = hv_ImageFiles.TupleConcat(
                            hv_ImageFilesTmp);
                        hv_ImageFiles.Dispose();
                        hv_ImageFiles = ExpTmpLocalVar_ImageFiles;
                    }
                }
            }

            hv_Extensions_COPY_INP_TMP.Dispose();
            hv_ImageDirectoryIndex.Dispose();
            hv_ImageFilesTmp.Dispose();
            hv_CurrentImageDirectory.Dispose();
            hv_HalconImages.Dispose();
            hv_OS.Dispose();
            hv_Directories.Dispose();
            hv_Index.Dispose();
            hv_Length.Dispose();
            hv_NetworkDrive.Dispose();
            hv_Substring.Dispose();
            hv_FileExists.Dispose();
            hv_AllFiles.Dispose();
            hv_i.Dispose();
            hv_Selection.Dispose();

            return;
        }

      
    }
}