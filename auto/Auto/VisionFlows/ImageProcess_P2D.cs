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
    public class ImageProcess_P2D:ImageProcessBase
    {
      

        /// <summary>
        /// Socket判断料有无
        /// </summary>
        /// <param name="ho_Image1"></param>
        /// <param name="ho_RegionTrans"></param>
        private static HTuple socket_Top_ModelID = null;
        private static HTuple socket_Bot_ModelID = null;
        private static HTuple tray_Left_ModelID = null;
        private static HTuple tray_Right_ModelID = null;
        private static HShapeModel tray_Slot_ModelID = null;

        public override int SocketDetect(HImage himage, out HObject ho_RegionTrans)
        {
            HOperatorSet.SetSystem("clip_region", "false");
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            using (HDevDisposeHelper hp = new HDevDisposeHelper())
            {
                try
                {
                    HImage image = himage.CopyImage();
                    HRegion ho_SerchRoi1 = ImagePara.Instance.GetSerchROI(ImagePara.Instance.RegionROI1[ImageProcessBase.CurrentSoket]);
                    HRegion ho_SerchRoi2 = ImagePara.Instance.GetSerchROI(ImagePara.Instance.RegionROI2[ImageProcessBase.CurrentSoket]);
                    HRegion HaveOrNot_Roi= ImagePara.Instance.GetSerchROI(ImagePara.Instance.SocketIsDutROI[ImageProcessBase.CurrentSoket]);
                    FindSocketMarkFirst(image, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Phi, out HObject mark);
                    if (hv_Row.Length <= 0)
                    {
                        FindSocketMarkSecond(image, out hv_Row, out hv_Column, out hv_Phi, out mark);
                        if (hv_Row.Length <= 0)
                        {
                            AutoNormal_New.ImageProcess.FindSocketMarkThird(image, out hv_Row, out hv_Column, out hv_Phi, out mark);
                        }
                    }
                    if (hv_Row.Length <= 0)
                    {
                        ho_SerchRoi1.Dispose();
                        ho_SerchRoi2.Dispose();
                        HaveOrNot_Roi.Dispose();
                        image.Dispose();
                        Flow.Log("Socket mark点未定位到");
                        return 0;
                    }
                    HHomMat2D hv_HomMat2D = new HHomMat2D();
                    hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.SocketGet_rowCenter[ImageProcessBase.CurrentSoket], ImagePara.Instance.SocketGet_colCenter[ImageProcessBase.CurrentSoket],
                        0, hv_Row, hv_Column, 0);
                    HRegion ho_RegionAffineTrans1 = hv_HomMat2D.AffineTransRegion(ho_SerchRoi1, "nearest_neighbor");
                    HRegion ho_RegionAffineTrans2 = hv_HomMat2D.AffineTransRegion(ho_SerchRoi2, "nearest_neighbor");
                    HRegion ho_RegionAffineTrans3= hv_HomMat2D.AffineTransRegion(HaveOrNot_Roi, "nearest_neighbor");
                    HImage ho_ImageReduced1 = image.ReduceDomain(ho_RegionAffineTrans1);
                    HImage ho_ImageReduced2 = image.ReduceDomain(ho_RegionAffineTrans2);
                    HImage ho_ImageReduced3 = image.ReduceDomain(ho_RegionAffineTrans3);
                    ho_SerchRoi1.Dispose();
                    ho_SerchRoi2.Dispose();
                    HaveOrNot_Roi.Dispose();
                    image.Dispose();

                    //匹配
                    if (socket_Top_ModelID == null)
                    {
                        HOperatorSet.ReadObject(out HObject obj1, Utility.HobjectFile + "P2D_socket_top_obj.hobj");
                        HOperatorSet.CreateShapeModelXld(obj1, "auto", -0.39, 0.79, "auto", "auto", "ignore_local_polarity",7, out socket_Top_ModelID);                    
                    }
                    HOperatorSet.GetShapeModelContours(out HObject ho_ModelContours, socket_Top_ModelID, 1);
                    HOperatorSet.FindShapeModel(ho_ImageReduced1, socket_Top_ModelID, -0.39, 0.79, ImagePara.Instance.SoketDutScore,
                            1, 0.3, "least_squares", 0, 0.9, out HTuple Row1, out HTuple Column1, out HTuple Angle1, out HTuple Score1);
                    
                    if (socket_Bot_ModelID == null)
                    {
                        HOperatorSet.ReadObject(out HObject obj2,Utility.HobjectFile+ "P2D_socket_bottom_obj.hobj");
                        HOperatorSet.CreateShapeModelXld(obj2, "auto", -0.39, 0.79, "auto", "auto", "ignore_local_polarity",  7, out socket_Bot_ModelID);                      
                    }
                    HOperatorSet.GetShapeModelContours(out HObject ho_ModelContoursBot, socket_Bot_ModelID, 1);
                    HOperatorSet.FindShapeModel(ho_ImageReduced2, socket_Bot_ModelID, -0.39, 0.79, ImagePara.Instance.SoketDutScore,
            1, 0.5, "least_squares", 0, 0.9, out HTuple Row2, out HTuple Column2, out HTuple Angle2, out HTuple Score2);
                    if (Row2.Length == 1 || Row1.Length == 1)
                    {
                        ho_RegionAffineTrans1.Dispose();
                        ho_RegionAffineTrans2.Dispose();
                        ho_RegionAffineTrans3.Dispose();
                        ho_ImageReduced1.Dispose();
                        ho_ImageReduced2.Dispose();
                        ho_ImageReduced3.Dispose();
                        ho_ModelContours.Dispose();
                        ho_ModelContoursBot.Dispose();
                        //有料,是正常的
                        return 3;
                    }
                    else if (Row2.Length < 1 && Row1.Length < 1)
                    {
                        HOperatorSet.Threshold(ho_ImageReduced3, out HObject region3, 200, 255);
                        //HOperatorSet.BinaryThreshold(ho_ImageReduced3, out HObject reg, "max_separability", "dark", out HTuple tuple);
                        HOperatorSet.AreaCenter(region3, out HTuple area, out HTuple row3, out HTuple col3);

                        HOperatorSet.ScaleImage(ho_ImageReduced3, out HObject imgScl, 3.75, -157);
                        HOperatorSet.Threshold(imgScl, out HObject regTh, 0, 100);
                        HOperatorSet.Connection(regTh, out HObject regCon);
                        HOperatorSet.FillUp(regCon, out HObject regFill);
                        HOperatorSet.ShapeTrans(regFill, out regFill, "convex");
                        HOperatorSet.SelectShape(regFill, out HObject s1, "area", "and", 30000, 80000);
                        HOperatorSet.SelectShape(s1, out HObject s2, "width", "and", 200, 400);
                        HOperatorSet.SelectShape(s2, out HObject s3, "height", "and", 200, 400);
                        HOperatorSet.SelectShape(s3, out HObject s4, "rectangularity","and", 0.7, 1);
                        HOperatorSet.CountObj(s4, out HTuple n1);
                        region3.Dispose();
                        imgScl.Dispose();
                        regTh.Dispose();
                        regCon.Dispose();
                        regFill.Dispose();
                        s1.Dispose();
                        s2.Dispose();
                        s3.Dispose();
                        s4.Dispose();
                        ho_RegionAffineTrans2.Dispose();
                        ho_RegionAffineTrans3.Dispose();
                        ho_ImageReduced1.Dispose();
                        ho_ImageReduced2.Dispose();
                        ho_ImageReduced3.Dispose();
                        ho_RegionAffineTrans1.Dispose();
                        if (area<150000&&n1>=1)
                        {
                            // 无料
                            ho_ModelContours.Dispose();
                            ho_ModelContoursBot.Dispose();
                            return 0;
                        }
                        //有料，是歪斜的
                        return 1;
                    }
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, Row1, Column1, Angle1, out HTuple HomMat2D1);
                    HOperatorSet.AffineTransContourXld(ho_ModelContours, out HObject ho_ContoursAffineTrans1, HomMat2D1);
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, Row2, Column2, Angle2, out HTuple HomMat2D2);
                    HOperatorSet.AffineTransContourXld(ho_ModelContoursBot, out HObject ho_ContoursAffineTrans2, HomMat2D2);
                    ho_ModelContours.Dispose();
                    ho_ModelContoursBot.Dispose();

                    //无料
                    return 0;
                }

                
                catch (Exception ex)
                {
                    Flow.Log(ex.Message + " + " + ex.StackTrace);
                    GC.Collect();
                    return 0;
                }
            }
            
            
        }

        /// <summary>
        /// 判断slot产品有无 有料返回true 无料返回false，料歪斜报NG
        /// </summary>
        /// <param name="ho_Image1"></param>
        /// <param name="ho_RegionTrans"></param>
        /// <returns></returns>    
        public override bool SlotDetect(InputPara parameter, out HObject ho_RegionShape)
        {
            HOperatorSet.GenEmptyObj(out ho_RegionShape);
            using (HDevDisposeHelper hp = new HDevDisposeHelper())
            {
                try
                {
                    HOperatorSet.SetSystem("clip_region", "false");
                    HOperatorSet.MedianImage(parameter.image, out HObject imageMedian, "circle", 9, "mirrored");
                    HOperatorSet.MultImage(imageMedian, imageMedian, out HObject imageMult, 0.1, 0);
                    HOperatorSet.GrayClosingRect(imageMult, out HObject imageClosing, 20, 20);
                    HOperatorSet.BinaryThreshold(imageClosing, out HObject regionBinary, "max_separability", "light", out HTuple usedT);
                    HOperatorSet.ErosionCircle(regionBinary, out HObject regionEros, 17);
                    HOperatorSet.OpeningCircle(regionEros, out HObject ho_RegionOpening, 31);
                    HOperatorSet.FillUp(ho_RegionOpening, out HObject ho_RegionFillUp);
                    HOperatorSet.Connection(ho_RegionFillUp, out HObject ho_ConnectedRegions);
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out HObject ho_SelectedRegions, "area",
                        "and", 130000, 250000);
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions, out ExpTmpOutVar_0, "rectangularity",
                        "and", 0.7, 1);
                    HOperatorSet.CountObj(ExpTmpOutVar_0, out HTuple number);
                    imageMedian.Dispose();
                    imageMult.Dispose();
                    imageClosing.Dispose();
                    regionBinary.Dispose();
                    regionEros.Dispose();
                    ho_RegionOpening.Dispose();
                    ho_RegionFillUp.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_SelectedRegions.Dispose();

                    if (number == 2)
                    {
                        HOperatorSet.ShapeTrans(ExpTmpOutVar_0, out HObject ho_RegionTrans, "rectangle2");
                        ExpTmpOutVar_0.Dispose();
                        HOperatorSet.AreaCenter(ho_RegionTrans, out HTuple hv_Area, out HTuple hv_Row, out HTuple hv_Column);
                        HTuple hv_row = ((hv_Row.TupleSelect(0)) + (hv_Row.TupleSelect(1))) / 2;
                        HTuple hv_col = ((hv_Column.TupleSelect(0)) + (hv_Column.TupleSelect(1))) / 2;
                        HOperatorSet.VectorAngleToRigid(ImagePara.Instance.tray_rough_posi_row,
                            ImagePara.Instance.tray_rough_posi_col, 0,
                            hv_row, hv_col, 0, out HTuple homate);
                        HRegion ho_ROI_left = ImagePara.Instance.GetSerchROI(ImagePara.Instance.TrayDutROI1);
                        HRegion ho_ROI_right = ImagePara.Instance.GetSerchROI(ImagePara.Instance.TrayDutROI2);

                        HOperatorSet.AffineTransRegion(ho_ROI_left, out HObject hRegion_left, homate, "nearest_neighbor");
                        HOperatorSet.AffineTransRegion(ho_ROI_right, out HObject hRegion_right, homate, "nearest_neighbor");
                        HOperatorSet.ReduceDomain(parameter.image, hRegion_left, out HObject image_left);
                        HOperatorSet.ReduceDomain(parameter.image, hRegion_right, out HObject image_right);
                        if (tray_Left_ModelID == null)
                        {
                            HOperatorSet.ReadObject(out HObject obj1, Utility.HobjectFile + "P2D_tray_left_obj.hobj");
                            HOperatorSet.CreateShapeModelXld(obj1, "auto", -0.39, 0.79, "auto", "auto", "ignore_local_polarity", 7, out tray_Left_ModelID);
                        }
                        if (tray_Right_ModelID == null)
                        {
                            HOperatorSet.ReadObject(out HObject obj2, Utility.HobjectFile + "P2D_tray_right_obj.hobj");
                            HOperatorSet.CreateShapeModelXld(obj2, "auto", -0.39, 0.79, "auto", "auto", "ignore_local_polarity", 7, out tray_Right_ModelID);
                        }
                        
                        HOperatorSet.FindShapeModel(image_left, tray_Left_ModelID, -0.39, 0.79, ImagePara.Instance.SlotScore, 1, 0.5, "least_squares", 0, 0.9, out HTuple Row_left,
                            out HTuple Column_left, out HTuple Angle_left, out HTuple Score_left);
                        HOperatorSet.FindShapeModel(image_right, tray_Right_ModelID, -0.39, 0.79, 0.7, 1, 0.5, "least_squares", 0, 0.9, out HTuple Row_right,
                            out HTuple Column_right, out HTuple Angle_right, out HTuple Score_right);
                        //HOperatorSet.ClearShapeModel(tray_Left_ModelID);
                        //HOperatorSet.ClearShapeModel(tray_Right_ModelID);
                        //tray_Left_ModelID = null;
                        //tray_Right_ModelID = null;
                        ho_RegionTrans.Dispose();
                        ho_ROI_left.Dispose();
                        ho_ROI_right.Dispose();
                        hRegion_left.Dispose();
                        hRegion_right.Dispose();
                        image_left.Dispose();
                        image_right.Dispose();
                        if (Score_left.Length == 1 && Score_right.Length == 1)
                        {
                            
                            return true;
                        }                       
                    }
                    HOperatorSet.GetImageSize(parameter.image, out HTuple width, out HTuple height);
                    HOperatorSet.GenRectangle2(out HObject rectangle, height / 2, width / 2, 0, 1000, 600);
                    HOperatorSet.ReduceDomain(parameter.image, rectangle, out HObject imgReduce);
                    HOperatorSet.Threshold(imgReduce, out HObject regThreshold, ImagePara.Instance.TrayDut_minthreshold, ImagePara.Instance.TrayDut_maxthreshold);
                    HOperatorSet.AreaCenter(regThreshold, out HTuple area, out HTuple row, out HTuple col);
                    parameter.image.Dispose();
                    rectangle.Dispose();
                    imgReduce.Dispose();
                    regThreshold.Dispose();

                    if (area>100000)
                    {
                        return true;
                    }


                    return false;
                }
                catch (Exception ex)
                {
                    Flow.Log("检测：" + ex.Message + ex.StackTrace);
                    return false;
                }


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

        //HShapeModel modelleftID,modelrightID;
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
           
                ///优先模板匹配，未识别就阈值处理
                try
                {
                using (HDevDisposeHelper hp = new HDevDisposeHelper())
                {
                    HObject  ho_RegionOpening;
                    HObject ho_RegionFillUp,  ho_ConnectedRegions;
                    HObject ho_SelectedRegions, ho_RegionTrans;
                    //粗定位
                    // Local control variables 
                    HTuple hv_Area = new HTuple(), hv_Row = new HTuple();
                    HTuple hv_Column = new HTuple(), hv_row = new HTuple();
                    HTuple hv_col = new HTuple();
                    // Initialize local and output iconic variables 
                    HOperatorSet.MedianImage(parameter.image, out HObject imageMedian, "circle", 9, "mirrored");
                    HOperatorSet.MultImage(imageMedian, imageMedian, out HObject imageMult, 0.1, 0);
                    HOperatorSet.GrayClosingRect(imageMult, out HObject imageClosing, 20, 20);
                    HOperatorSet.BinaryThreshold(imageClosing, out HObject regionBinary, "max_separability", "light", out HTuple usedT);
                    HOperatorSet.ErosionCircle(regionBinary, out HObject regionEros, 17);
                    HOperatorSet.OpeningCircle(regionEros, out ho_RegionOpening, 31);
                    HOperatorSet.FillUp(ho_RegionOpening, out ho_RegionFillUp);
                    HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                    HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                        "and", 130000, 250000);
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.SelectShape(ho_SelectedRegions, out ExpTmpOutVar_0, "rectangularity",
                        "and", 0.8, 1);
                    HOperatorSet.CountObj(ExpTmpOutVar_0, out HTuple number);
                    imageMedian.Dispose();
                    imageMult.Dispose();
                    imageClosing.Dispose();
                    regionBinary.Dispose();
                    regionEros.Dispose();
                    ho_RegionOpening.Dispose();
                    ho_RegionFillUp.Dispose();
                    ho_ConnectedRegions.Dispose();
                    ho_SelectedRegions.Dispose();

                    if (number == 2)
                    {
                        HOperatorSet.ShapeTrans(ExpTmpOutVar_0, out ho_RegionTrans, "rectangle2");
                        ExpTmpOutVar_0.Dispose();
                        HOperatorSet.AreaCenter(ho_RegionTrans, out hv_Area, out hv_Row, out hv_Column);
                        ho_RegionTrans.Dispose();
                        hv_row = ((hv_Row.TupleSelect(
                            0)) + (hv_Row.TupleSelect(1))) / 2;
                        hv_col = ((hv_Column.TupleSelect(
                            0)) + (hv_Column.TupleSelect(1))) / 2;
                        HOperatorSet.VectorAngleToRigid(ImagePara.Instance.tray_rough_posi_row,
                            ImagePara.Instance.tray_rough_posi_col, 0,
                            hv_row, hv_col, 0, out HTuple homate);
                        HRegion ho_ROI_left = ImagePara.Instance.GetSerchROI(ImagePara.Instance.TrayDutROI1);
                        HRegion ho_ROI_right = ImagePara.Instance.GetSerchROI(ImagePara.Instance.TrayDutROI2);

                        HOperatorSet.AffineTransRegion(ho_ROI_left, out HObject hRegion_left, homate, "nearest_neighbor");
                        HOperatorSet.AffineTransRegion(ho_ROI_right, out HObject hRegion_right, homate, "nearest_neighbor");
                        ho_ROI_left.Dispose();
                        ho_ROI_right.Dispose();

                        HOperatorSet.Union2(hRegion_left, hRegion_right, out HObject RegUnion);
                        HOperatorSet.ReduceDomain(parameter.image, hRegion_left, out HObject image_left);
                        HOperatorSet.ReduceDomain(parameter.image, hRegion_right, out HObject image_right);
                        hRegion_left.Dispose();
                        hRegion_right.Dispose();
                        if (tray_Left_ModelID == null)
                        {
                            HOperatorSet.ReadObject(out HObject obj1, Utility.HobjectFile + "P2D_tray_left_obj.hobj");
                            HOperatorSet.CreateShapeModelXld(obj1, "auto", -0.39, 0.79, "auto", "auto", "ignore_local_polarity", 7, out tray_Left_ModelID);
                        }
                        if (tray_Right_ModelID == null)
                        {
                            HOperatorSet.ReadObject(out HObject obj2, Utility.HobjectFile + "P2D_tray_right_obj.hobj");
                            HOperatorSet.CreateShapeModelXld(obj2, "auto", -0.39, 0.79, "auto", "auto", "ignore_local_polarity", 7, out tray_Right_ModelID);
                        }
                        HOperatorSet.FindShapeModel(image_left, tray_Left_ModelID, -0.39, 0.79, ImagePara.Instance.DutScore, 1, 0.5, "least_squares", 0, 0.9, out HTuple Row_left,
                            out HTuple Column_left, out HTuple Angle_left, out HTuple Score_left);
                        HOperatorSet.FindShapeModel(image_right, tray_Right_ModelID, -0.39, 0.79, ImagePara.Instance.DutScore, 1, 0.5, "least_squares", 0, 0.9, out HTuple Row_right,
                            out HTuple Column_right, out HTuple Angle_right, out HTuple Score_right);
                       
                    
                        if (Column_left.Length == 1 && Column_right.Length == 1)
                        {
                            HOperatorSet.GetShapeModelContours(out HObject modelContourLeft, tray_Left_ModelID, 1);
                            HOperatorSet.VectorAngleToRigid(0, 0, 0, Row_left, Column_left, Angle_left, out HTuple homMat1);
                            HOperatorSet.AffineTransContourXld(modelContourLeft, out HObject left_contour, homMat1);
                            HOperatorSet.ShapeTransXld(left_contour, out HObject leftContour, "rectangle2");
                            HOperatorSet.AreaCenterXld(leftContour, out HTuple area1, out HTuple row_centre_left,
                                out HTuple col_centre_left, out HTuple point1);
                            HOperatorSet.GetShapeModelContours(out HObject modelContourRight, tray_Right_ModelID, 1);
                            HOperatorSet.VectorAngleToRigid(0, 0, 0, Row_right, Column_right, Angle_right, out HTuple homMat2);
                            HOperatorSet.AffineTransContourXld(modelContourRight, out HObject right_contour, homMat2);
                            HOperatorSet.ShapeTransXld(right_contour, out HObject rightContour, "rectangle2");
                            HOperatorSet.AreaCenterXld(rightContour, out HTuple area2, out HTuple row_centre_right,
                                out HTuple col_centre_right, out HTuple point2);
                            HOperatorSet.GenRegionContourXld(leftContour, out HObject region1, "margin");
                            HOperatorSet.GenRegionContourXld(rightContour, out HObject region2, "margin");
                            HOperatorSet.Union2ClosedContoursXld(leftContour, rightContour, out HObject RegContour);
                            HOperatorSet.GenRegionContourXld(RegContour, out HObject RegGen,"margin");
                            HOperatorSet.ConcatObj(RegGen, RegUnion, out HObject RegDisp);
                            HOperatorSet.AngleLx(row_centre_left, col_centre_left, row_centre_right, col_centre_right,
                                out HTuple rad);
                            double centreRow = (row_centre_left + row_centre_right) / 2;
                            double centreCol = (col_centre_left + col_centre_right) / 2;
                            HOperatorSet.TupleDeg(rad, out HTuple angle);
                            locationResult.Phi = angle.D;
                            locationResult.findPoint = new PointPosition_Image();
                            locationResult.findPoint.Row = centreRow;
                            locationResult.findPoint.Column = centreCol;
                            locationResult.IsRunOk = true;
                            locationResult.SmallestRec2Xld = RegDisp.CopyObj(1,4);
                            parameter.image.Dispose();
                            RegUnion.Dispose();
                            modelContourLeft.Dispose();
                            leftContour.Dispose();
                            left_contour.Dispose();
                            modelContourRight.Dispose();
                            right_contour.Dispose();
                            rightContour.Dispose();
                            region1.Dispose();
                            region2.Dispose();
                            RegContour.Dispose();
                            RegGen.Dispose();
                            RegDisp.Dispose();
                            //HOperatorSet.ClearShapeModel(tray_Left_ModelID);
                            //HOperatorSet.ClearShapeModel(tray_Right_ModelID);
                            //tray_Left_ModelID = null;
                            //tray_Right_ModelID = null;
                            image_left.Dispose();
                            image_right.Dispose();
                            return locationResult;

                        }
                    }

                    ///模板匹配到了就不执行
                    HOperatorSet.MeanImage(parameter.image, out HObject image_Mean, 9, 9);
                    HOperatorSet.MultImage(image_Mean, image_Mean, out HObject image_Mult, 0.1, 0);
                    HOperatorSet.GrayClosingRect(image_Mult, out HObject imageC, 21, 21);
                    HOperatorSet.BinaryThreshold(imageC, out HObject regionB, "max_separability", "dark", out HTuple u);
                    HOperatorSet.Connection(regionB, out HObject regionCon);
                    HOperatorSet.SelectShapeStd(regionCon, out HObject selectRg, "max_area", 70);
                    HOperatorSet.FillUp(selectRg, out HObject filRg);
                    HOperatorSet.Difference(filRg, selectRg, out HObject diffReg);
                    HOperatorSet.Connection(diffReg, out HObject conReg);
                    HOperatorSet.OpeningCircle(conReg, out HObject openReg, 51);
                    HOperatorSet.SelectShape(openReg, out HObject s1, "area", "and", 1000000, 5000000);
                    HOperatorSet.SelectShape(s1, out HObject s2, "width", "and", 1000, 3500);
                    HOperatorSet.SelectShape(s2, out HObject s3, "height", "and", 500, 1500);
                    HOperatorSet.ReduceDomain(image_Mean, s3, out HObject ho_ImageReduced1);
                    //检查是否有料
                    HOperatorSet.Threshold(ho_ImageReduced1, out HObject region33, 200, 255);
                    HOperatorSet.Connection(region33, out region33);
                    HOperatorSet.SelectShapeStd(region33, out HObject SelectedRegions, "max_area", 70);
                    HOperatorSet.AreaCenter(SelectedRegions, out HTuple area33, out HTuple row1, out HTuple column1);
                    image_Mean.Dispose();
                    image_Mult.Dispose();
                    imageC.Dispose();
                    regionB.Dispose();
                    regionCon.Dispose();
                    selectRg.Dispose();
                    diffReg.Dispose();
                    conReg.Dispose();
                    filRg.Dispose();
                    openReg.Dispose();
                    s1.Dispose();
                    s2.Dispose();
                    s3.Dispose();
                    region33.Dispose();
                    SelectedRegions.Dispose();

                    if (area33.Length<=0||area33.D < 2000)
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
                    HOperatorSet.SmallestRectangle2(ho_RegionUnion, out HTuple row, out HTuple col, out HTuple phi, out HTuple length1, out HTuple length2);
                    int RegionNumber = ho_RegionUnion.CountObj();
                    ho_ImageReduced1.Dispose();
                    ho_Region1.Dispose();
                    ho_RegionFillUp2.Dispose();
                    ho_ConnectedRegions2.Dispose();
                    ho_SelectedRegions1.Dispose();
                    ho_RegionUnion.Dispose();
                    if (RegionNumber <= 0)
                    {

                        //locationResult.ErrString = "图像处理失败";
                        Flow.Log("Tray定位DUT 失败 ");
                        locationResult.ErrString = "识别失败，RegionNumber>1";
                        locationResult.IsRunOk = false;
                    }
                    HRegion Rectangle = new HRegion();
                    Rectangle.GenRectangle2(row, col, phi, length1, length2);
                    Rectangle.AreaCenter(out double arearow, out double areacol);
                    HXLDCont ContourXld = new HXLDCont(Rectangle.GenContourRegionXld("border"));
                    HOperatorSet.TupleDeg(phi, out HTuple angleff);
                    locationResult.Phi = angleff.D;
                    locationResult.findPoint = new PointPosition_Image();
                    locationResult.findPoint.Row = arearow;
                    locationResult.findPoint.Column = areacol;
                    locationResult.SmallestRec2Xld = new HXLDCont(ContourXld);
                    locationResult.IsRunOk = true;
                    locationResult.region = Rectangle;
                    Rectangle.Dispose();
                    ContourXld.Dispose();
                }
                   return locationResult;
                }              
                catch (Exception ex)
                {
                    Flow.Log(ex.Message + ex.StackTrace);
                    locationResult.IsRunOk = false;
                    return locationResult;
                }
        }
        //HShapeModel modelleftID,modelrightID;
        /// <summary>
        /// 定位 Tray盘中Dut位置 函数，返回值为 定位是否成功的标志位
        /// </summary>
        /// <param name="image"></param>
        /// <param name="shapeModel"></param>
        /// <param name="DutPosition"></param>
        /// <returns></returns>
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
                    if (tray_Slot_ModelID==null|| !tray_Slot_ModelID.IsInitialized())
                    {
                        tray_Slot_ModelID = new HShapeModel();
                        tray_Slot_ModelID.ReadShapeModel(Utility.ModelFile+ "P2D_tray_slot_model");
                        //HOperatorSet.ReadObject(out HObject obj1, Utility.HobjectFile + "P2D_tray_slot_obj.hobj");
                        //HOperatorSet.CreateShapeModelXld(obj1, "auto", -0.39, 0.79, "auto", "auto", "ignore_local_polarity", 5, out tray_Slot_ModelID);
                    }
                    HOperatorSet.ScaleImage(parameter.image, out HObject imageScale, 5.1, -15);
                    HOperatorSet.GetShapeModelContours(out HObject modelContours, tray_Slot_ModelID, 1);
                    HOperatorSet.FindShapeModel(imageScale, tray_Slot_ModelID, -0.39, 0.79, 0.3, 1, ImagePara.Instance.SlotScore, "least_squares", 
                        0, 0.9, out HTuple row, out HTuple col, out HTuple angle, out HTuple score);
                    //tray_Slot_ModelID.ClearShapeModel();
                    //tray_Slot_ModelID.Dispose();
                    if (row.Length != 1)
                    {
                        parameter.image.Dispose();
                        imageScale.Dispose();
                        modelContours.Dispose();
                        locationResult.ErrString = "定位失败";
                        locationResult.IsRunOk = false;
                        return locationResult;                        
                    }
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, row, col, angle, out HTuple hom2d);
                    HOperatorSet.AffineTransContourXld(modelContours, out HObject contours, hom2d);
                    HOperatorSet.ShapeTransXld(contours, out locationResult.SmallestRec2Xld, "rectangle2");
                    locationResult.findPoint = new PointPosition_Image(row, col);
                    locationResult.Phi = angle;                   
                    //locationResult.region = new HRegion();
                    //locationResult.region.GenEmptyRegion();
                    parameter.image.Dispose();
                    contours.Dispose();
                    imageScale.Dispose();
                    modelContours.Dispose();
                    hom2d.Dispose();
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
                if (row1.Length <= 0)
                {
                    FindSocketMarkSecond(locationPara.image, out row1, out colum1, out phi, out mark);
                    if (row1.Length <= 0)
                    {
                        AutoNormal_New.ImageProcess.FindSocketMarkThird(locationPara.image, out row1, out colum1, out phi, out mark);
                    }
                }
                if (colum1.Length > 0)
                {
                    HHomMat2D hv_HomMat2D = new HHomMat2D();
                    hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.SocketMark_rowCenter, 
                        ImagePara.Instance.SocketMark_colCenter,
                        ImagePara.Instance.SocketMark_angleCenter, row1,
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
            using (HDevDisposeHelper hp = new HDevDisposeHelper())
            {
                try
                {
                    HRegion Markroi = ImagePara.Instance.GetSerchROI(ImagePara.Instance.SocketMarkROI);
                    HImage markImage = parameter.image.ReduceDomain(Markroi);
                    FindSocketMarkFirst(markImage, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Phi, out HObject mark);
                    if (hv_Row.Length <= 0)
                    {
                        FindSocketMarkSecond(markImage, out hv_Row, out hv_Column, out hv_Phi, out mark);
                        if (hv_Row.Length <= 0)
                        {
                            AutoNormal_New.ImageProcess.FindSocketMarkThird(markImage, out hv_Row, out hv_Column, out hv_Phi, out mark);
                        }
                    }
                    HHomMat2D hv_HomMat2D = new HHomMat2D();
                    hv_HomMat2D.VectorAngleToRigid(ImagePara.Instance.SocketGet_rowCenter[ImageProcessBase.CurrentSoket], ImagePara.Instance.SocketGet_colCenter[ImageProcessBase.CurrentSoket],
                        0, hv_Row, hv_Column, 0);
                    HRegion ho_SerchRoi1 = ImagePara.Instance.GetSerchROI(ImagePara.Instance.RegionROI1[ImageProcessBase.CurrentSoket]);
                    HRegion ho_SerchRoi2 = ImagePara.Instance.GetSerchROI(ImagePara.Instance.RegionROI2[ImageProcessBase.CurrentSoket]);
                    HRegion ho_RegionAffineTrans1 = hv_HomMat2D.AffineTransRegion(ho_SerchRoi1, "nearest_neighbor");
                    HRegion ho_RegionAffineTrans2 = hv_HomMat2D.AffineTransRegion(ho_SerchRoi2, "nearest_neighbor");
                    HImage ho_ImageReduced1 = parameter.image.ReduceDomain(ho_RegionAffineTrans1);
                    HImage ho_ImageReduced2 = parameter.image.ReduceDomain(ho_RegionAffineTrans2);
                    // 新增模板匹配，以突出梯形为特征，取代圆
                    //上特征
                    if (socket_Top_ModelID == null)
                    {
                        HOperatorSet.ReadObject(out HObject obj1, Utility.HobjectFile + "P2D_socket_top_obj.hobj");
                        HOperatorSet.CreateShapeModelXld(obj1, "auto", -0.39, 0.79, "auto", "auto", "ignore_local_polarity", 9, out socket_Top_ModelID);
                    }
                    HOperatorSet.GetShapeModelContours(out HObject ho_ModelContours1, socket_Top_ModelID, 1);
                    HOperatorSet.FindShapeModel(ho_ImageReduced1, socket_Top_ModelID, -0.39, 0.79, ImagePara.Instance.SoketDutScore,
                                    1, 0.5, "least_squares", 0, 0.9, out HTuple Row1, out HTuple Column1, out HTuple Angle1, out HTuple Score1);
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, Row1, Column1, Angle1, out HTuple HomMat2D1);
                    HOperatorSet.AffineTransContourXld(ho_ModelContours1, out HObject ho_ContoursAffineTrans1, HomMat2D1);
                    HOperatorSet.GenRegionContourXld(ho_ContoursAffineTrans1, out HObject topRegion, "filled");
                    HOperatorSet.ShapeTrans(topRegion, out HObject top_region, "rectangle2");
                    HOperatorSet.AreaCenter(top_region, out HTuple area, out HTuple row_Top, out HTuple col_Top);
                    //下特征
                    if (socket_Bot_ModelID == null)
                    {
                        HOperatorSet.ReadObject(out HObject obj2, Utility.HobjectFile + "P2D_socket_bottom_obj.hobj");
                        HOperatorSet.CreateShapeModelXld(obj2, "auto", -0.39, 0.79, "auto", "auto", "ignore_local_polarity", 9, out socket_Bot_ModelID);
                    }
                    HOperatorSet.GetShapeModelContours(out HObject ho_ModelContours2, socket_Bot_ModelID, 1);
                    HOperatorSet.FindShapeModel(ho_ImageReduced2, socket_Bot_ModelID, -0.39, 0.79, ImagePara.Instance.SoketDutScore,
                                    1, 0.5, "least_squares", 0, 0.9, out HTuple Row2, out HTuple Column2, out HTuple Angle2, out HTuple Score2);
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, Row2, Column2, Angle2, out HTuple HomMat2D2);
                    HOperatorSet.AffineTransContourXld(ho_ModelContours2, out HObject ho_ContoursAffineTrans2, HomMat2D2);
                    HOperatorSet.GenRegionContourXld(ho_ContoursAffineTrans2, out HObject botRegion, "filled");
                    HOperatorSet.ShapeTrans(botRegion, out HObject bot_region, "rectangle2");
                    HOperatorSet.AreaCenter(bot_region, out HTuple area1, out HTuple row_Bottom, out HTuple col_Bottom);

                    Markroi.Dispose();
                    markImage.Dispose();
                    ho_SerchRoi1.Dispose();
                    ho_SerchRoi2.Dispose();
 
                    ho_ImageReduced1.Dispose();
                    ho_ImageReduced2.Dispose();
                    ho_ModelContours1.Dispose();
                    ho_ModelContours2.Dispose();

                    if (Row2.Length > 0 && Row1.Length > 0)
                    {
                        //找到目标
                        HOperatorSet.AngleLx(row_Top, col_Top, row_Bottom, col_Bottom, out HTuple phi);
                        //HOperatorSet.AngleLl(row_Top, col_Top,row_Bottom, col_Bottom, 0,0,0,100,out HTuple phi);//和水平线的夹角
                        locationResult.Phi = phi.TupleDeg();
                        locationResult.findPoint = new PointPosition_Image((Row2.D + Row1.D) / 2, (Column2.D + Column1.D) / 2);
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
                    ho_RegionAffineTrans1.Dispose();
                    ho_RegionAffineTrans2.Dispose();
                    ho_ContoursAffineTrans1.Dispose();
                    ho_ContoursAffineTrans2.Dispose();
                    topRegion.Dispose();
                    botRegion.Dispose();
                    bot_region.Dispose();
                    top_region.Dispose();


                    return locationResult;
                }
                catch (Exception ex)
                {
                    locationResult.ErrString = "图像处理失败!";
                    Flow.Log("Socket定位DUT 失败 " + ex.Message + ex.StackTrace);
                    locationResult.IsRunOk = false;
                }

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
            hv_Phi = new HTuple();
            mark = new HObject();
            try
            {

                // Local iconic variables 
                HObject ho_Region, ho_ConnectedRegions, ho_RegionOpening;
                HObject ho_SelectedRegions, ho_RegionClosing, ho_RegionFillUp;
                HObject ho_SortedRegions, ho_Sorted1, ho_Sorted2;
                HObject ho_Cross;

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
                HOperatorSet.GenEmptyObj(out ho_SortedRegions);
                HOperatorSet.GenEmptyObj(out ho_Sorted1);
                HOperatorSet.GenEmptyObj(out ho_Sorted2);
                HOperatorSet.GenEmptyObj(out ho_Cross);
                //HOperatorSet.Threshold(ho_Image, out ho_Region, ImagePara.Instance.SoketMark_minthreshold, ImagePara.Instance.SoketMark_maxthreshold);
                HOperatorSet.Threshold(ho_Image, out ho_Region, 200, 255);
                HOperatorSet.Connection(ho_Region, out ho_ConnectedRegions);
                HOperatorSet.ClosingCircle(ho_ConnectedRegions, out ho_RegionClosing, 41);
                HOperatorSet.OpeningCircle(ho_RegionClosing, out HObject regOpen, 5);
                HOperatorSet.SelectShape(ho_RegionClosing, out HObject hObject, "circularity", "and", 0.75, 1);
                HOperatorSet.SelectShape(hObject, out ho_SelectedRegions, "area", "and", 50000, 70000);
                HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "character",
                       "true", "row");
                HOperatorSet.CountObj(ho_SortedRegions, out HTuple num);
                HOperatorSet.SelectObj(ho_SortedRegions, out HObject select1, 1);
                HOperatorSet.SelectObj(ho_SortedRegions, out HObject select2, num);
                HOperatorSet.ConcatObj(select1, select2, out HObject regConcat);
                select1.Dispose();
                select2.Dispose();
                HOperatorSet.ShapeTrans(regConcat, out HObject regTrans, "outer_circle");
                HOperatorSet.AreaCenter(regTrans, out HTuple area, out HTuple row, out HTuple col);
                HOperatorSet.DistancePp(row[0], col[0], row[1], col[1], out HTuple distance);
                HOperatorSet.AngleLx(row[0], col[0], row[1], col[1], out hv_Phi);
                if (distance > 1200)
                {
                    hv_row_center = (row[0] + row[1]) / 2;
                    hv_col_center = (col[0] + col[1]) / 2;
                    hv_angle = hv_Phi;
                    HOperatorSet.GenCrossContourXld(out mark, hv_row_center, hv_col_center, 196, hv_Phi);
                }
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_RegionOpening.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionClosing.Dispose();
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
            catch 
            {

                 
            }
            


        }
        /// <summary>
        /// 提取Socket两个圆点的外接矩形二次提取
        /// </summary>
        /// <param name="ho_Image"></param>
        /// <param name="hv_Row1"></param>
        /// <param name="hv_Column1"></param>
        /// <param name="hv_Phi"></param>
        /// <param name="mark"></param>
        public override void FindSocketMarkSecond(HObject ho_Image, out HTuple hv_row_center, out HTuple hv_col_center,
    out HTuple hv_Phi, out HObject mark)
        {
            hv_row_center = new HTuple();
            hv_col_center = new HTuple();
            hv_Phi = new HTuple();
            mark = new HObject();
            try
            {
                // Local iconic variables 
               mark = new HObject();
                HObject ho_Region, ho_ConnectedRegions, ho_RegionOpening;
                HObject ho_SelectedRegions, ho_RegionClosing, ho_RegionFillUp;
                HObject ho_SortedRegions, ho_Sorted1, ho_Sorted2;
                HObject ho_Cross;

                // Local control variables 

                HTuple hv_Number = new HTuple(), hv_Radius1 = new HTuple();
                HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
                HTuple hv_Radius2 = new HTuple();
                HTuple hv_angle = new HTuple();
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_Region);
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
                HOperatorSet.GenEmptyObj(out ho_RegionOpening);
                HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                HOperatorSet.GenEmptyObj(out ho_RegionClosing);
                HOperatorSet.GenEmptyObj(out ho_SortedRegions);
                HOperatorSet.GenEmptyObj(out ho_Sorted1);
                HOperatorSet.GenEmptyObj(out ho_Sorted2);
                HOperatorSet.GenEmptyObj(out ho_Cross);
                HOperatorSet.MedianImage(ho_Image, out ho_Image, "circle",10, "mirrored");
                HOperatorSet.Threshold(ho_Image, out ho_Region, 200, 255);
                
                HOperatorSet.ClosingCircle(ho_Region, out ho_RegionClosing, 100);
                HOperatorSet.OpeningCircle(ho_RegionClosing, out HObject regOpen, 100);
                HOperatorSet.Connection(regOpen, out ho_ConnectedRegions);
                HOperatorSet.SelectShape(ho_ConnectedRegions, out HObject hObject, "circularity", "and", 0.8, 1);
                HOperatorSet.SelectShape(hObject, out ho_SelectedRegions, "area", "and", 30000, 60000);
                HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "character",
                       "true", "row");
                HOperatorSet.CountObj(ho_SortedRegions, out HTuple num);
                HOperatorSet.SelectObj(ho_SortedRegions, out HObject select1, 1);
                HOperatorSet.SelectObj(ho_SortedRegions, out HObject select2, num);
                HOperatorSet.ConcatObj(select1, select2, out HObject regConcat);
                select1.Dispose();
                select2.Dispose();
                HOperatorSet.ShapeTrans(regConcat, out HObject regTrans, "outer_circle");
                HOperatorSet.AreaCenter(regTrans, out HTuple area, out HTuple row, out HTuple col);
                HOperatorSet.DistancePp(row[0], col[0], row[1], col[1], out HTuple distance);
                HOperatorSet.AngleLx(row[0], col[0], row[1], col[1], out hv_Phi);
                if (distance > 1200)
                {
                    hv_row_center = (row[0] + row[1]) / 2;
                    hv_col_center = (col[0] + col[1]) / 2;
                    hv_angle = hv_Phi;
                    HOperatorSet.GenCrossContourXld(out mark, hv_row_center, hv_col_center, 196, hv_Phi);
                }
                ho_Region.Dispose();
                ho_ConnectedRegions.Dispose();
                ho_RegionOpening.Dispose();
                ho_SelectedRegions.Dispose();
                ho_RegionClosing.Dispose();
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
            }
            catch
            {

            }

        }

        public override void FindSocketMarkThird(HObject ho_Image, out HTuple hv_row_center, out HTuple hv_col_center,
     out HTuple hv_Phi, out HObject mark)
        {
            hv_row_center = new HTuple();
            hv_col_center = new HTuple();
            hv_Phi = new HTuple();
            mark = new HObject();
            try
            {
                // Local iconic variables 
                mark = new HObject();
                HObject ho_Cross;

                // Local control variables 

                HTuple hv_Number = new HTuple(), hv_Radius1 = new HTuple();
                HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
                HTuple hv_Radius2 = new HTuple();
                HTuple hv_angle = new HTuple();
                // Initialize local and output iconic variables 
                HOperatorSet.MedianImage(ho_Image, out ho_Image, "circle", 10, "mirrored");
                HOperatorSet.GenCircleContourXld(out HObject ho_ContCircle, 714.507, 1774.59, 117.872,
        0, 6.28318, "positive", 1);
                HOperatorSet.CreateShapeModelXld(ho_ContCircle, "auto", -0.39, 0.79, "auto",
                    "auto", "ignore_local_polarity", 5, out HTuple hv_ModelID);
                ho_ContCircle.Dispose();
                HOperatorSet.FindShapeModel(ho_Image, hv_ModelID, -0.39, 0.79, 0.50, 0, 0.8, "least_squares",
                    0, 0.9, out HTuple row, out HTuple col, out HTuple hv_Angle, out HTuple hv_Score);
                HOperatorSet.TupleLength(hv_Score, out HTuple num);
                HOperatorSet.GetShapeModelContours(out HObject mdCont, hv_ModelID, 1);
                HOperatorSet.GenEmptyObj(out HObject emptyOBJ);
                for (int i = 0; i < num; i++)
                {
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, row[i], col[i], 0, out HTuple hm1);
                    HOperatorSet.AffineTransContourXld(mdCont, out HObject ct1, hm1);
                    HOperatorSet.ConcatObj(emptyOBJ, ct1, out emptyOBJ);
                    ct1.Dispose();
                }
                HOperatorSet.SortContoursXld(emptyOBJ, out HObject sortcont, "character", "true", "row");
                HOperatorSet.AreaCenterXld(sortcont, out HTuple ar, out HTuple roww, out HTuple coll, out HTuple ord);


                HOperatorSet.DistancePp(roww[0], coll[0], roww[num-1], coll[num-1], out HTuple distance);               
                HOperatorSet.ClearShapeModel(hv_ModelID);
                hv_ModelID.Dispose();
                HOperatorSet.AngleLx(roww[0], coll[0], roww[num-1], coll[num-1], out hv_Phi);
                if (distance > 1200)
                {
                    hv_row_center = (roww[0] + roww[num-1]) / 2;
                    hv_col_center = (coll[0] + coll[num-1]) / 2;
                    hv_angle = hv_Phi;
                    HOperatorSet.GenCrossContourXld(out mark, hv_row_center, hv_col_center, 196, hv_Phi);
                }
                mdCont.Dispose();
                emptyOBJ.Dispose();
                hv_Number.Dispose();
                hv_Radius1.Dispose();
                hv_Row2.Dispose();
                hv_Column2.Dispose();
                hv_Radius2.Dispose();
                hv_row_center.Dispose();
                hv_col_center.Dispose();
                hv_angle.Dispose();
            }
            catch
            {

            }

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
                HOperatorSet.OpeningCircle(ho_Region, out HObject regOpening, 51);
                HOperatorSet.Connection(regOpening, out HObject ho_ConnectedRegions);
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

        /// <summary>
        /// 正常流程判断Tray中是否有DUT
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="ho_RegionTrans"></param>
        /// <returns></returns>
        public override bool IsTrayDutHaveOrNot_Normal(InputPara parameter, out HObject ho_RegionTrans)
        {
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            using (HDevDisposeHelper hp = new HDevDisposeHelper())
            {
                try
                {
                    HOperatorSet.MeanImage(parameter.image, out HObject image_Mean, 9, 9);
                    HOperatorSet.MultImage(image_Mean, image_Mean, out HObject image_Mult, 0.1, 0);
                    HOperatorSet.GrayClosingRect(image_Mult, out HObject imageC, 21, 21);
                    HOperatorSet.BinaryThreshold(imageC, out HObject regionB, "max_separability", "dark", out HTuple u);
                    HOperatorSet.Connection(regionB, out HObject regionCon);
                    HOperatorSet.SelectShapeStd(regionCon, out HObject selectRg, "max_area", 70);
                    HOperatorSet.FillUp(selectRg, out HObject filRg);
                    HOperatorSet.Difference(filRg, selectRg, out HObject diffReg);
                    HOperatorSet.Connection(diffReg, out HObject conReg);
                    HOperatorSet.OpeningCircle(conReg, out HObject openReg, 51);
                    HOperatorSet.SelectShape(openReg, out HObject s1, "area", "and", 1000000, 5000000);
                    HOperatorSet.SelectShape(s1, out HObject s2, "width", "and", 1000, 3500);
                    HOperatorSet.SelectShape(s2, out HObject s3, "height", "and", 500, 1500);
                    HOperatorSet.CountObj(s3, out HTuple number);
                    image_Mean.Dispose();
                    image_Mult.Dispose();
                    imageC.Dispose();
                    regionB.Dispose();
                    regionCon.Dispose();
                    selectRg.Dispose();
                    filRg.Dispose();
                    diffReg.Dispose();
                    conReg.Dispose();
                    openReg.Dispose();
                    s1.Dispose();
                    s2.Dispose();

                    if (number != 1)
                    {
                        HOperatorSet.GetImageSize(parameter.image, out HTuple width, out HTuple height);
                        HOperatorSet.GenRectangle2(out HObject rectangle, height / 2, width / 2, 0, 1500, 1000);
                        HOperatorSet.ReduceDomain(parameter.image, rectangle, out HObject imageRec);
                        HOperatorSet.Threshold(imageRec, out HObject reg1, 200, 255);
                        HOperatorSet.SelectShape(reg1, out HObject selectr, "area", "and", 30000, 9999999);
                        HOperatorSet.CountObj(selectr, out HTuple N);
                        imageRec.Dispose();
                        rectangle.Dispose();
                        reg1.Dispose();
                        selectr.Dispose();
                        if (N == 1)
                        {

                            return true;
                        }

                    }
                    HOperatorSet.ReduceDomain(parameter.image, s3, out HObject ho_ImageReduced1);
                    s3.Dispose();
                    HOperatorSet.Threshold(ho_ImageReduced1, out HObject threReg, 180, 255);
                    HOperatorSet.SelectShape(threReg, out HObject selctReg, "area", "and", 50000, 99999999);
                    HOperatorSet.CountObj(selctReg, out HTuple Num);
                    ho_ImageReduced1.Dispose();
                    threReg.Dispose();
                    selctReg.Dispose();
                    if (Num == 1)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception ex)
                {

                    return false;
                }
            }           
        }

        
    }
}