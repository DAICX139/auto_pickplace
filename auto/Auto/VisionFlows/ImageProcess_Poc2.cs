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

        public override int SocketDetect(HImage image, out HObject ho_RegionTrans)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            try
            {
                HRegion ho_SerchRoi1 = ImagePara.Instance.GetSerchROI(ImagePara.Instance.RegionROI1[ImageProcessBase.CurrentSoket]);
                HRegion ho_SerchRoi2 = ImagePara.Instance.GetSerchROI(ImagePara.Instance.RegionROI2[ImageProcessBase.CurrentSoket]);
                FindSocketMarkFirst(image, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Phi, out HObject mark);
                if(hv_Row.Length<=0)
                {
                    Flow.Log("mark点未定位到");
                    return 0;
                }
                HHomMat2D hv_HomMat2D = new HHomMat2D();
                hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.SocketGet_rowCenter[ImageProcessBase.CurrentSoket], ImagePara.Instance.SocketGet_colCenter[ImageProcessBase.CurrentSoket],
                    ImagePara.Instance.SocketGet_angleCenter[ImageProcessBase.CurrentSoket], hv_Row, hv_Column, hv_Phi);
                HRegion ho_RegionAffineTrans1 = hv_HomMat2D.AffineTransRegion(ho_SerchRoi1, "nearest_neighbor");
                HRegion ho_RegionAffineTrans2 = hv_HomMat2D.AffineTransRegion(ho_SerchRoi2, "nearest_neighbor");
                HImage ho_ImageReduced1 = image.ReduceDomain(ho_RegionAffineTrans1);
                HImage ho_ImageReduced2 = image.ReduceDomain(ho_RegionAffineTrans2);

                //匹配
                if (SocketDUT_ModelID == null)
                {
                    HOperatorSet.GenCircleContourXld(out HObject ho_ContCircle, 2338.65, 2888.99, 82.5602, 0, 6.28318, "positive", 1);
                    HOperatorSet.CreateShapeModelXld(ho_ContCircle, "auto", -0.39, 0.79, "auto", "auto", "ignore_local_polarity", 3, out SocketDUT_ModelID);
                }
                HOperatorSet.GetShapeModelContours(out HObject ho_ModelContours, SocketDUT_ModelID, 1);
                HOperatorSet.FindShapeModel(ho_ImageReduced1, SocketDUT_ModelID, -0.39, 0.79, ImagePara.Instance.SoketDutScore,
                        1, 0.5, "least_squares", 0, 0.9, out HTuple Row1, out HTuple Column1, out HTuple Angle1, out HTuple Score1);
                if (Row1.Length<=0)
                {
                    return 0;
                }
                HOperatorSet.VectorAngleToRigid(0, 0, 0, Row1, Column1, Angle1, out HTuple HomMat2D1);
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out HObject ho_ContoursAffineTrans1, HomMat2D1);

                HOperatorSet.FindShapeModel(ho_ImageReduced2, SocketDUT_ModelID, -0.39, 0.79, ImagePara.Instance.SoketDutScore,
        1, 0.5, "least_squares", 0, 0.9, out HTuple Row2, out HTuple Column2, out HTuple Angle2, out HTuple Score2);
                if (Row2.Length <= 0)
                {
                    return 0;
                }
                HOperatorSet.VectorAngleToRigid(0, 0, 0, Row2, Column2, Angle2, out HTuple HomMat2D2);
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out HObject ho_ContoursAffineTrans2, HomMat2D2);
                if (Row2.Length > 0 || Row1.Length > 0)
                {
                    //有料
                    return 3;
                }
                //物料
                return 0;
            }
            catch (Exception ex)
            {
                Flow.Log(ex.Message + " + " + ex.StackTrace);
                //GC.Collect();
                return 0;
            }
        }

        /// <summary>
        /// 判断slot产品有无 有料返回true 无料返回false
        /// </summary>
        /// <param name="ho_Image1"></param>
        /// <param name="ho_RegionTrans"></param>
        /// <returns></returns>
        private static HTuple slot_ModelID = null;

     
        public override bool SlotDetect(InputPara parameter, out HObject ho_RegionShape)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            HOperatorSet.GenEmptyObj(out ho_RegionShape);
            try
            {
                HOperatorSet.MeanImage(parameter.image, out HObject ho_ImageMean, 600, 600);
                HOperatorSet.DynThreshold(parameter.image, ho_ImageMean, out HObject ho_RegionDynThresh, 10, "dark");
                HOperatorSet.FillUpShape(ho_RegionDynThresh, out HObject ho_RegionFillUp, "area", 1, 1000000);
                HOperatorSet.Connection(ho_RegionFillUp, out HObject ho_ConnectedRegions);
                HOperatorSet.SelectShape(ho_ConnectedRegions, out HObject ho_SelectedRegions, (new HTuple("area")).TupleConcat(
                    "convexity"), "and", (new HTuple(2168630)).TupleConcat(0.83953), (new HTuple(3668630)).TupleConcat(2));
                //HOperatorSet.GenEmptyObj(out HObject ho_EmptyObject);
                //HOperatorSet.TestEqualObj(ho_EmptyObject, ho_SelectedRegions, out HTuple hv_IsEqual);
                //ho_ImageMean.Dispose();
                //ho_RegionDynThresh.Dispose();
                //ho_RegionFillUp.Dispose();
                //ho_ConnectedRegions.Dispose();
                //if ((int)(new HTuple(hv_IsEqual.TupleNotEqual(0))) != 0)
                //{
                //    //定位失败有料
                //    return true;
                //}
                //HOperatorSet.OpeningCircle(ho_SelectedRegions, out HObject ho_RegionOpening, 30);
                //HOperatorSet.ReduceDomain(parameter.image, ho_RegionOpening, out HObject ho_ImageReduced);
                //HOperatorSet.BinaryThreshold(ho_ImageReduced, out HObject ho_Region, "max_separability",
                //    "light", out HTuple hv_UsedThreshold);
                //HOperatorSet.OpeningRectangle1(ho_Region, out HObject ho_RegionOpening1, 20, 20);
                //HOperatorSet.Connection(ho_RegionOpening1, out HObject ho_ConnectedRegions1);
                //HOperatorSet.SelectShape(ho_ConnectedRegions1, out HObject ho_SelectedRegions1, (new HTuple("width")).TupleConcat(
                //    "height"), "and", (new HTuple(420)).TupleConcat(450), (new HTuple(800)).TupleConcat(
                //    1000));
                //HOperatorSet.ShapeTrans(ho_SelectedRegions1, out ho_RegionShape, "convex");
                //HOperatorSet.CountObj(ho_RegionShape, out HTuple hv_Number);
                //ho_SelectedRegions.Dispose();
                //ho_RegionOpening.Dispose();
                //ho_ImageReduced.Dispose();
                //ho_Region.Dispose();
                //ho_RegionOpening1.Dispose();
                //ho_ConnectedRegions1.Dispose();
                //ho_SelectedRegions1.Dispose();
                //if ((int)(new HTuple(hv_Number.TupleNotEqual(3))) != 0)
                //{
                //    //有料存在
                //    return true;
                //}
                //return false;
                HOperatorSet.OpeningCircle(ho_SelectedRegions, out HObject ho_RegionOpening, 30);
                HOperatorSet.ReduceDomain(parameter.image, ho_RegionOpening, out HObject ho_ImageReduced);
                HOperatorSet.BinaryThreshold(ho_ImageReduced, out HObject ho_Region, "max_separability",
                    "light", out HTuple hv_UsedThreshold);
                HOperatorSet.OpeningRectangle1(ho_Region, out HObject ho_RegionOpening1, 20, 20);
                HOperatorSet.Connection(ho_RegionOpening1, out HObject ho_ConnectedRegions1);
                HOperatorSet.SelectShape(ho_ConnectedRegions1, out HObject ho_SelectedRegions1, (new HTuple("width")).TupleConcat(
                    "height"), "and", (new HTuple(420)).TupleConcat(450), (new HTuple(800)).TupleConcat(
                    1000));
                HOperatorSet.ShapeTrans(ho_SelectedRegions1, out ho_RegionShape, "convex");
                HOperatorSet.CountObj(ho_RegionShape, out HTuple hv_Number);
                ho_SelectedRegions.Dispose();
                ho_RegionOpening.Dispose();
                ho_ImageReduced.Dispose();
                ho_Region.Dispose();
                ho_RegionOpening1.Dispose();
                ho_ConnectedRegions1.Dispose();
                ho_SelectedRegions1.Dispose();
                if ((int)(new HTuple(hv_Number.TupleNotEqual(3))) != 0)
                {
                    //有料存在
                    return true;
                }
                //HOperatorSet.ScaleImage(parameter.image, out HObject imageScale, 5.1, -15);
                //HOperatorSet.ReadShapeModel(Utility.ModelFile + "P2D_Inner_Slot_model", out HTuple modelID);
                //HOperatorSet.FindShapeModel(imageScale, modelID, -0.39, 0.79, 0.7, 1, 0.5, "least_squares", 0, 0.9, out HTuple Row1, out HTuple Column1, out HTuple Angle1, 
                //    out HTuple Score1);
                //if (Score1.Length != 0)
                //{

                //    return false;
                //}
                //HOperatorSet.Threshold(parameter.image, out HObject region, 180, 255);
                //HOperatorSet.OpeningCircle(region, out HObject regionOpening, 3.5);
                //HOperatorSet.AreaCenter(regionOpening, out HTuple area, out HTuple row, out HTuple col);
                //if (area>100000)
                //{
                //    return true;
                //}


                return false;
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
            HOperatorSet.SetSystem("clip_region", "false");
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            try
            {
                //GC.Collect();
                HRegion ho_ROI_0 = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SocketBlockROI);
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
            HOperatorSet.SetSystem("clip_region", "false");
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
            HOperatorSet.SetSystem("clip_region", "false");
            OutPutResult locationResult = new OutPutResult();
            try
            {
                    try
                    {
                        HOperatorSet.Threshold(parameter.image, out HObject ho_Region, 0, 50);
                        HOperatorSet.Connection(ho_Region, out HObject ho_ConnectedRegions);
                        HOperatorSet.FillUpShape(ho_Region, out HObject ho_RegionFillUp, "area", 300, 1200000);
                        HOperatorSet.OpeningCircle(ho_RegionFillUp, out HObject ho_RegionOpening, 13.5);
                        HOperatorSet.Connection(ho_RegionOpening, out HObject ho_ConnectedRegions1);
                        HOperatorSet.SelectShapeStd(ho_ConnectedRegions1, out HObject ho_SelectedRegions, "max_area",70);
                        HOperatorSet.FillUp(ho_SelectedRegions, out HObject ho_RegionFillUp1);
                        HOperatorSet.ReduceDomain(parameter.image, ho_RegionFillUp1, out HObject ho_ImageReduced1);
                    //检查是否有料
                        HOperatorSet.Threshold(ho_ImageReduced1, out HObject region2, 200, 255);
                        HOperatorSet.Connection(region2, out region2);
                        HOperatorSet.SelectShapeStd(region2, out HObject SelectedRegions, "max_area", 70);
                        HOperatorSet.AreaCenter(SelectedRegions,out HTuple area1,out HTuple row1,out HTuple column1);

                    ho_Region.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_RegionFillUp.Dispose();
                    ho_RegionOpening.Dispose();
                    ho_ConnectedRegions1.Dispose();
                    ho_RegionFillUp1.Dispose();
                    ho_SelectedRegions.Dispose();
                    region2.Dispose();
                    SelectedRegions.Dispose();
                    if (area1.D<2000)
                    {
                        locationResult.ErrString = "识别失败，无料";
                        locationResult.IsRunOk = false;
                        return locationResult;
                    }

                    //提取有料的部分
                        HOperatorSet.Threshold(ho_ImageReduced1, out HObject ho_Region1, ImagePara.Instance.TrayDut_minthreshold, ImagePara.Instance.TrayDut_maxthreshold);
                        HOperatorSet.FillUp(ho_Region1, out HObject ho_RegionFillUp2);
                        HOperatorSet.Connection(ho_RegionFillUp2, out HObject ho_ConnectedRegions2);
                        HOperatorSet.SelectShape(ho_ConnectedRegions2, out HObject ho_SelectedRegions1, "area",
                            "and", 2000, 9999999);
                        HOperatorSet.Union1(ho_SelectedRegions1, out HObject ho_RegionUnion);
                        HOperatorSet.SmallestRectangle2(ho_RegionUnion,out HTuple row, out HTuple col,out HTuple phi,out HTuple length1, out HTuple length2);
                        ho_Region1.Dispose();
                        ho_RegionFillUp2.Dispose();
                        ho_ConnectedRegions2.Dispose();
                        ho_SelectedRegions1.Dispose();
                        int RegionNumber = ho_RegionUnion.CountObj();
                        ho_RegionUnion.Dispose();
                    if (RegionNumber <=0)
                        {
                            locationResult.ErrString = "识别失败，RegionNumber>1";
                            locationResult.IsRunOk = false;
                        }
                        HRegion Rectangle = new HRegion();
                        Rectangle.GenRectangle2(row, col, phi, length1, length2);
                        Rectangle.AreaCenter(out double arearow, out double areacol);
                        HXLDCont ContourXld = new HXLDCont(Rectangle.GenContourRegionXld("border"));

                        HOperatorSet.TupleDeg(phi, out HTuple angle);
                        locationResult.Phi = angle.D;
                        locationResult.findPoint = new PointPosition_Image();
                        locationResult.findPoint.Row = arearow;
                        locationResult.findPoint.Column = areacol;
                        locationResult.SmallestRec2Xld = new HXLDCont(ContourXld);
                        locationResult.IsRunOk = true;

                        locationResult.region = Rectangle;
                        locationResult.shapeModelContour = new HXLDCont();
                        locationResult.shapeModelContour.GenEmptyObj();
                        ContourXld.Dispose();
                        GC.Collect();
                    }
                    catch (Exception ex)
                    {
                        locationResult.ErrString = "图像处理失败";
                        Flow.Log("Tray定位DUT 失败 " + ex.Message + ex.StackTrace);
                        locationResult.IsRunOk = false;
                    }
                    return locationResult;
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

        public override bool IsTrayDutHaveOrNot_Normal(InputPara parameter, out HObject ho_RegionTrans)
        {
            return base.IsTrayDutHaveOrNot_Normal(parameter, out ho_RegionTrans);
        }

        /// <summary>
        /// 定位空Tary盘的位置
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override OutPutResult ShotSlot(InputPara parameter)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            OutPutResult locationResult = new OutPutResult();

            try
            {
                if (true)
                {
                    //GC.Collect();
                    HOperatorSet.ScaleImage(parameter.image, out HObject imageScale, 5.1, -15);
                    HOperatorSet.ReadShapeModel(Utility.ModelFile + "P2D_new_tray_model", out HTuple modelID);
                    HOperatorSet.GetShapeModelContours(out HObject modelContours, modelID, 1);
                    HOperatorSet.FindShapeModel(imageScale, modelID, -0.39, 0.79, 0.3, 1, 0.5, "least_squares", 0, 0.9, out HTuple row, out HTuple col, out HTuple angle, out HTuple score);
                    if (row.Length!=1)
                    {
                        locationResult.ErrString = "定位失败";
                        locationResult.IsRunOk = false;
                        return locationResult;
                        //    //定位异常
                    }
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, row, col, angle, out HTuple hom2d);
                    HOperatorSet.AffineTransContourXld(modelContours, out HObject contours, hom2d);
                    HOperatorSet.ShapeTransXld(contours, out locationResult.SmallestRec2Xld, "rectangle2");
                    locationResult.findPoint = new PointPosition_Image(row, col);
                    locationResult.Phi = angle;
                    //locationResult.shapeModelContour.GenRectangle2ContourXld(hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2);
                    locationResult.region = new HRegion();
                    locationResult.region.GenEmptyRegion();


                    //HOperatorSet.MedianImage(parameter.image, out HObject ho_ImageMedian, "circle", 10, "mirrored");
                    //HOperatorSet.MeanImage(ho_ImageMedian, out HObject ho_ImageMean, 600, 600);
                    //HOperatorSet.DynThreshold(ho_ImageMedian, ho_ImageMean, out HObject ho_RegionDynThresh, 10, "dark");
                    //HOperatorSet.FillUpShape(ho_RegionDynThresh, out HObject ho_RegionFillUp, "area", 1000, 1000000);
                    //HOperatorSet.Connection(ho_RegionFillUp, out HObject ho_ConnectedRegions);
                    //HOperatorSet.SelectShape(ho_ConnectedRegions, out HObject ho_SelectedRegions1, (new HTuple("area")).TupleConcat(
                    //    "convexity"), "and", (new HTuple(2168630)).TupleConcat(0.83953), (new HTuple(3668630)).TupleConcat(
                    //    2));
                    //HOperatorSet.GenEmptyObj(out HObject ho_EmptyObject);
                    //HOperatorSet.TestEqualObj(ho_EmptyObject, ho_SelectedRegions1, out HTuple hv_IsEqual);
                    ////ho_ImageMedian.Dispose();
                    ////ho_ImageMean.Dispose();
                    ////ho_RegionDynThresh.Dispose();
                    ////ho_RegionFillUp.Dispose();
                    ////ho_ConnectedRegions.Dispose();
                    //if ((int)(new HTuple(hv_IsEqual.TupleEqual(1))) != 0)
                    //{
                    //    locationResult.ErrString = "定位失败";
                    //    locationResult.IsRunOk = false;
                    //    return locationResult;
                    //    //定位异常
                    //}

                    //HOperatorSet.OpeningRectangle1(ho_SelectedRegions1, out HObject ho_RegionOpening, 130, 130);
                    //HOperatorSet.ShapeTrans(ho_RegionOpening, out HObject ho_RegionTrans, "convex");
                    //HOperatorSet.ErosionCircle(ho_RegionTrans, out HObject ho_RegionErosion, 50);
                    //HOperatorSet.SmallestRectangle2(ho_RegionErosion, out HTuple hv_Row, out HTuple hv_Column,
                    //    out HTuple hv_Phi, out HTuple hv_Length1, out HTuple hv_Length2);
                    //locationResult.findPoint = new PointPosition_Image(hv_Row, hv_Column);
                    //locationResult.Phi = hv_Phi;
                    //locationResult.shapeModelContour.GenRectangle2ContourXld(hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2);
                    //locationResult.region = new HRegion();
                    //locationResult.region.GenEmptyRegion();

                    ////模板匹配轮廓的外切矩形
                    //HOperatorSet.GenRectangle2ContourXld(out locationResult.SmallestRec2Xld, hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2);
                    //ho_RegionOpening.Dispose();
                    //ho_RegionTrans.Dispose();
                    //ho_RegionErosion.Dispose();
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
        /// <summary>
        /// 定位Socket mark点上的
        /// </summary>
        /// <param name="image"></param>
        /// <param name="findPoint"></param>
        /// <returns></returns>
        public override OutPutResult SocketMark(InputPara locationPara, out HObject mark)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            //给出定位中心点位 给出定位轮廓 给出角度计算直线
            OutPutResult locationResult = new OutPutResult();
            HOperatorSet.GenEmptyObj(out mark);
            try
            {
                FindSocketMarkFirst(locationPara.image, out HTuple row1, out HTuple colum1, out HTuple phi, out mark);
                if (colum1.Length > 0)
                {
                    HHomMat2D hv_HomMat2D = new HHomMat2D();
                    hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.SocketMark_rowCenter, ImagePara.Instance.SocketMark_colCenter, ImagePara.Instance.SocketMark_angleCenter, row1,
                        colum1, phi);
                    HTuple CenterRow = hv_HomMat2D.AffineTransPoint2d(ImagePara.Instance.SocketMark_PutDutRow, ImagePara.Instance.SocketMark_PutDutCol, out HTuple CenterCol);
                    locationResult.findPoint = new PointPosition_Image(CenterRow, CenterCol);
                    locationResult.Phi = phi;
                    locationResult.IsRunOk = true;
                }
                else
                {
                    locationResult.ErrString = "mark点定位失败";
                    locationResult.findPoint = new PointPosition_Image(0, 0);
                    locationResult.Phi = 0;
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



        HTuple SocketDUT_ModelID;

        /// <summary>
        /// 定位 Socket中Dut位置 函数 （圆定位方式）
        /// </summary>
        /// <param name="image"></param>
        /// <param name="shapeModel"></param>
        /// <param name="DutPosition"></param>
        /// <returns></returns>
        public override OutPutResult SocketDutFront(InputPara parameter)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            OutPutResult locationResult = new OutPutResult();
            try
            {
                
                FindSocketMarkFirst(parameter.image, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Phi, out HObject mark);
                HHomMat2D hv_HomMat2D = new HHomMat2D();
                hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.SocketGet_rowCenter[ImageProcessBase.CurrentSoket], ImagePara.Instance.SocketGet_colCenter[ImageProcessBase.CurrentSoket],
                    ImagePara.Instance.SocketGet_angleCenter[ImageProcessBase.CurrentSoket], hv_Row, hv_Column, hv_Phi);
                HRegion ho_SerchRoi1 = ImagePara.Instance.GetSerchROI(ImagePara.Instance.RegionROI1[ImageProcessBase.CurrentSoket]);
                HRegion ho_SerchRoi2 = ImagePara.Instance.GetSerchROI(ImagePara.Instance.RegionROI2[ImageProcessBase.CurrentSoket]);
                HRegion ho_RegionAffineTrans1 = hv_HomMat2D.AffineTransRegion(ho_SerchRoi1, "nearest_neighbor");
                HRegion ho_RegionAffineTrans2 = hv_HomMat2D.AffineTransRegion(ho_SerchRoi2, "nearest_neighbor");
                HImage ho_ImageReduced1 = parameter.image.ReduceDomain(ho_RegionAffineTrans1);
                HImage ho_ImageReduced2 = parameter.image.ReduceDomain(ho_RegionAffineTrans2);

                //匹配
                //if(SocketDUT_ModelID==null)
                //{
                //    HOperatorSet.GenCircleContourXld(out HObject ho_ContCircle, 2338.65, 2888.99, 82.5602, 0, 6.28318, "positive", 1);
                //    HOperatorSet.CreateShapeModelXld(ho_ContCircle, "auto", -0.39, 0.79, "auto", "auto", "ignore_local_polarity", 3, out SocketDUT_ModelID);
                //}
                // 新增模板匹配，以突出梯形为特征，取代圆
                //上特征                
                HOperatorSet.ReadShapeModel(Utility.ModelFile+ "trapezium_Top_model", out SocketDUT_ModelID);
                HOperatorSet.GetShapeModelContours(out HObject ho_ModelContours1, SocketDUT_ModelID, 1);
                HOperatorSet.FindShapeModel(ho_ImageReduced1, SocketDUT_ModelID, -0.39, 0.79, ImagePara.Instance.SoketDutScore,
                                1, 0.5, "least_squares", 0, 0.9, out HTuple Row1, out HTuple Column1, out HTuple Angle1,out HTuple Score1);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, Row1, Column1, Angle1, out HTuple HomMat2D1);
                HOperatorSet.AffineTransContourXld(ho_ModelContours1, out HObject ho_ContoursAffineTrans1,HomMat2D1);
                HOperatorSet.GenRegionContourXld(ho_ContoursAffineTrans1, out HObject topRegion,"filled");
                HOperatorSet.ShapeTrans(topRegion, out HObject top_region, "inner_circle");
                HOperatorSet.AreaCenter(top_region, out HTuple area, out HTuple row_Top, out HTuple col_Top);
                //下特征
                HOperatorSet.ReadShapeModel(Utility.ModelFile + "trapezium_Botm_model", out SocketDUT_ModelID);
                HOperatorSet.GetShapeModelContours(out HObject ho_ModelContours2, SocketDUT_ModelID, 1);
                HOperatorSet.FindShapeModel(ho_ImageReduced2, SocketDUT_ModelID, -0.39, 0.79, ImagePara.Instance.SoketDutScore,
                                1, 0.5, "least_squares", 0, 0.9, out HTuple Row2, out HTuple Column2, out HTuple Angle2, out HTuple Score2);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, Row2, Column2, Angle2, out HTuple HomMat2D2);
                HOperatorSet.AffineTransContourXld(ho_ModelContours2, out HObject ho_ContoursAffineTrans2, HomMat2D2);
                HOperatorSet.GenRegionContourXld(ho_ContoursAffineTrans2, out HObject botRegion, "filled");
                HOperatorSet.ShapeTrans(botRegion, out HObject bot_region, "inner_circle");
                HOperatorSet.AreaCenter(bot_region, out HTuple area1, out HTuple row_Bottom, out HTuple col_Bottom);
                //        HOperatorSet.GetShapeModelContours(out HObject ho_ModelContours, SocketDUT_ModelID, 1);
                //        HOperatorSet.FindShapeModel(ho_ImageReduced1, SocketDUT_ModelID, -0.39, 0.79, ImagePara.Instance.SoketDutScore,
                //                1, 0.5, "least_squares", 0, 0.9, out HTuple Row1, out HTuple Column1, out HTuple Angle1,out HTuple Score1);
                //            HOperatorSet.VectorAngleToRigid(0, 0, 0, Row1, Column1, Angle1, out HTuple HomMat2D1);
                //            HOperatorSet.AffineTransContourXld(ho_ModelContours, out HObject ho_ContoursAffineTrans1,HomMat2D1);

                //        HOperatorSet.FindShapeModel(ho_ImageReduced2, SocketDUT_ModelID, -0.39, 0.79, ImagePara.Instance.SoketDutScore,
                //1, 0.5, "least_squares", 0, 0.9, out HTuple Row2, out HTuple Column2, out HTuple Angle2, out HTuple Score2);
                //        HOperatorSet.VectorAngleToRigid(0, 0, 0, Row2, Column2, Angle2, out HTuple HomMat2D2);
                //        HOperatorSet.AffineTransContourXld(ho_ModelContours, out HObject ho_ContoursAffineTrans2, HomMat2D2);
                //        //排序

                //        HOperatorSet.ConcatObj(ho_ContoursAffineTrans1, ho_ContoursAffineTrans2, out HObject obj);
                //        HOperatorSet.SortContoursXld(obj, out HObject sortobj, "character", "true", "row");
                //        HOperatorSet.SelectObj(sortobj, out HObject sortobj1, 1);
                //        HOperatorSet.AreaCenterXld(sortobj1, out HTuple a, out HTuple row_Top, out HTuple col_Top, out HTuple p1);
                //        HOperatorSet.SelectObj(sortobj, out HObject sortobj2, 2);
                //        HOperatorSet.AreaCenterXld(sortobj2, out HTuple b, out HTuple row_Bottom, out HTuple col_Bottom, out HTuple p2);



                if (Row2.Length>0&& Row1.Length>0)
                {
                    //找到目标
                    HOperatorSet.AngleLx(row_Top, col_Top, row_Bottom, col_Bottom, out HTuple phi);
                    //HOperatorSet.AngleLl(row_Top, col_Top,row_Bottom, col_Bottom, 0,0,0,100,out HTuple phi);//和水平线的夹角
                    locationResult.Phi = phi.TupleDeg();
                    locationResult.findPoint = new PointPosition_Image((Row2.D+ Row1.D)/2, (Column2.D+ Column1.D)/2);
                    locationResult.region = ho_RegionAffineTrans1.ConcatObj(ho_RegionAffineTrans2);
                    locationResult.SmallestRec2Xld = ho_ContoursAffineTrans1.ConcatObj(ho_ContoursAffineTrans2);
                    locationResult.SmallestRec2Xld = bot_region.ConcatObj(top_region);
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
        public override void FindSocketMarkFirst(HObject ho_Image, out HTuple hv_row_center, out HTuple hv_col_center,
    out HTuple hv_Phi, out HObject mark)
        {

            hv_row_center = new HTuple();
            hv_col_center = new HTuple();
            // Local iconic variables 
            mark = new HObject();
            HObject ho_Region, ho_ConnectedRegions, ho_RegionOpening;
            HObject ho_SelectedRegions, ho_RegionClosing, ho_RegionFillUp;
            HObject ho_SortedRegions = null, ho_Sorted1 = null, ho_Sorted2 = null;
            HObject ho_Cross = null;

            // Local control variables 

            HTuple hv_Number = new HTuple(), hv_Radius1 = new HTuple();
            HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
            HTuple hv_Radius2 = new HTuple();
            HTuple hv_angle = new HTuple(), hv_WindowHandle = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Region);
            HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionOpening);
            HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
            HOperatorSet.GenEmptyObj(out ho_RegionClosing);
            HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
            HOperatorSet.GenEmptyObj(out ho_SortedRegions);
            HOperatorSet.GenEmptyObj(out ho_Sorted1);
            HOperatorSet.GenEmptyObj(out ho_Sorted2);
            HOperatorSet.GenEmptyObj(out ho_Cross);
            hv_Phi = new HTuple();
            ho_Region.Dispose();
            HOperatorSet.Threshold(ho_Image, out ho_Region, ImagePara.Instance.SoketMark_minthreshold, ImagePara.Instance.SoketMark_maxthreshold);
            ho_ConnectedRegions.Dispose();
            HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
            ho_RegionOpening.Dispose();
            HOperatorSet.ClosingCircle(ho_ConnectedRegions,out ho_ConnectedRegions,10);
            HOperatorSet.OpeningCircle(ho_ConnectedRegions, out ho_RegionOpening, 17);
            ho_SelectedRegions.Dispose();
            HOperatorSet.SelectShape(ho_RegionOpening, out HObject hObject, "area", "and", 20000, 70000);
            HOperatorSet.SelectShape(hObject, out ho_SelectedRegions, (new HTuple("ra")).TupleConcat(
                "roundness"), "and", (new HTuple(120)).TupleConcat(0.8), (new HTuple(150)).TupleConcat(
                1));
            
            ho_RegionClosing.Dispose();
            HOperatorSet.ClosingCircle(ho_SelectedRegions, out ho_RegionClosing, 7);
            ho_RegionFillUp.Dispose();
            HOperatorSet.FillUp(ho_RegionClosing, out ho_RegionFillUp);
            HOperatorSet.ShapeTrans(ho_RegionFillUp, out HObject region, "outer_circle");
            hv_Number.Dispose();
            HOperatorSet.CountObj(region, out hv_Number);
            if ((int)(new HTuple(hv_Number.TupleEqual(2))) != 0)
            {
                ho_SortedRegions.Dispose();
                HOperatorSet.SortRegion(region, out ho_SortedRegions, "character",
                    "true", "row");
                ho_Sorted1.Dispose();
                HOperatorSet.SelectObj(ho_SortedRegions, out ho_Sorted1, 1);
                ho_Sorted2.Dispose();
                HOperatorSet.SelectObj(ho_SortedRegions, out ho_Sorted2, 2);
                hv_Radius1.Dispose();
                HOperatorSet.SmallestCircle(ho_Sorted1, out HTuple hv_Row1, out HTuple hv_Column1, out hv_Radius1);
                hv_Row2.Dispose(); hv_Column2.Dispose(); hv_Radius2.Dispose();
                HOperatorSet.SmallestCircle(ho_Sorted2, out hv_Row2, out hv_Column2, out hv_Radius2);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_row_center = (hv_Row1 + hv_Row2) / 2;
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_col_center = (hv_Column1 + hv_Column2) / 2;
                }
                HOperatorSet.AngleLx(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Phi);
                hv_angle.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_angle = hv_Phi;
                }
                ho_Cross.Dispose();
                HOperatorSet.GenCrossContourXld(out mark, hv_row_center, hv_col_center,196, hv_Phi);
            }
            ho_Region.Dispose();
            ho_ConnectedRegions.Dispose();
            ho_RegionOpening.Dispose();
            ho_SelectedRegions.Dispose();
            ho_RegionClosing.Dispose();
            ho_RegionFillUp.Dispose();
            ho_SortedRegions.Dispose();
            ho_Sorted1.Dispose();
            ho_Sorted2.Dispose();
            ho_Cross.Dispose();

            hv_Number.Dispose();
            hv_Radius1.Dispose();
            hv_Row2.Dispose();
            hv_Column2.Dispose();
            hv_Radius2.Dispose();
            hv_row_center.Dispose();
            hv_col_center.Dispose();
            hv_angle.Dispose();
            hv_WindowHandle.Dispose();


        }

        /// <summary>
        /// 定位背面Dut 在吸嘴上的
        /// </summary>
        /// <param name="image"></param>
        /// <param name="DutCenter"></param>
        /// <param name="AngleLX"></param>
        /// <returns></returns>
        public override OutPutResult SecondDutBack_Circle(InputPara locationPara, PLCSend plcsend)
        {
            HOperatorSet.SetSystem("clip_region", "false");
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
                locationResult.Phi = Angle;
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

        /// <summary>
        /// 定位背面Dut 在吸嘴上的
        /// </summary>
        /// <param name="image"></param>
        /// <param name="DutCenter"></param>
        /// <param name="AngleLX"></param>
        /// <returns></returns>
        public override OutPutResult SecondDutBack(InputPara locationPara, PLCSend plcsend)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            OutPutResult locationResult = new OutPutResult();

            try
            {
                HOperatorSet.BinaryThreshold(locationPara.image, out HObject ho_Region, "max_separability", "dark", out HTuple hv_UsedThreshold);
                HOperatorSet.Connection(ho_Region, out HObject ho_ConnectedRegions);
                HOperatorSet.SelectShapeStd(ho_ConnectedRegions, out HObject ho_SelectedRegions, "max_area",70);
                HOperatorSet.SmallestRectangle2(ho_SelectedRegions, out HTuple hv_Row, out HTuple hv_Column,out HTuple hv_Phi, out HTuple hv_Length1, out HTuple hv_Length2);
                //HOperatorSet.GenRectangle2(out HObject ho_Rectangle, hv_Row, hv_Column, hv_Phi, hv_Length1,hv_Length2);
                HRegion ho_Rectangle = new HRegion();
                ho_Rectangle.GenRectangle2(hv_Row, hv_Column, hv_Phi, hv_Length1, hv_Length2);
                //裁剪读码 图片
                HImage ReadCodeImage = locationPara.image.ReduceDomain(ho_Rectangle);
                locationResult.IsReadDataCode = ReadDataCode(ReadCodeImage, out locationResult.DatacodeString, out HXLDCont RecDataCodeXld);
                locationResult.SmallestRec2Xld = RecDataCodeXld; //扫码的矩形框 利用形状模板xld给出去
                locationResult.region = ho_Rectangle; //裁剪读码 图片区域 给出去

                locationResult.findPoint = new PointPosition_Image(hv_Row, hv_Column);
                locationResult.Phi = hv_Phi;
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
            HOperatorSet.SetSystem("clip_region", "false");
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
            HOperatorSet.SetSystem("clip_region", "false");
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
            HOperatorSet.SetSystem("clip_region", "false");
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