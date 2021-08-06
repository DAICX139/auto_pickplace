using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionUtility;
using System.IO;

namespace VisionModules
{

    public class VisionModulesHelper
    {
        #region"vision"
        /// <summary>更新当前流程</summary>
        public static Action<Flow> UpdateFlow;

        /// <summary>
        /// 创建多尺度形状模型
        /// </summary>
        /// <param name="image">输入图像</param>
        /// <param name="modelParam">模型参数</param>
        /// <param name="shapeModel">形状模型</param>
        /// <returns></returns>
        public static bool CreateScaledShapeModel(HImage image, ModelParam modelParam, out HShapeModel shapeModel)
        {
            try
            {
                shapeModel = new HShapeModel();

                shapeModel = image.CreateScaledShapeModel("auto", ((new HTuple(modelParam.AngleStart)).TupleRad()), ((new HTuple(modelParam.AngleExtent)).TupleRad()), "auto",
                    modelParam.ScaleMin, modelParam.ScaleMax, "auto", "auto", "use_polarity", modelParam.Contrast, "auto");

                return true;
            }

            catch (Exception ex)
            {
                shapeModel = new HShapeModel();
                return false;
            }
        }

        /// <summary>
        /// 创建多尺度形状模型
        /// </summary>
        /// <param name="image">输入图像</param>
        /// <param name="modelParam">模型参数</param>
        /// <param name="shapeModel">形状模型</param>
        /// <returns></returns>
        public static bool CreateScaledShapeModel(HImage image, HRegion region, ModelParam modelParam, out HShapeModel shapeModel)
        {
            try
            {
                HImage imageReduce = image.ReduceDomain(region);

                shapeModel = new HShapeModel();
                shapeModel = imageReduce.CreateScaledShapeModel("auto", ((new HTuple(modelParam.AngleStart)).TupleRad()), ((new HTuple(modelParam.AngleExtent)).TupleRad()), "auto",
                    modelParam.ScaleMin, modelParam.ScaleMax, "auto", "auto", "use_polarity", modelParam.Contrast, "auto");

                return true;
            }
            catch (Exception ex)
            {
                shapeModel = new HShapeModel();
                return false;
            }
        }

        /// <summary>
        /// 创建多尺度形状模型
        /// </summary>
        /// <param name="image">输入图像</param>
        /// <param name="modelParam">模型参数</param>
        /// <param name="shapeModel">形状模型</param>
        /// <param name="modelRegion">形状模型区域（第一层）</param>
        /// <returns></returns>
        public static bool CreateScaledShapeModel(HImage image, ModelParam modelParam, out HShapeModel shapeModel, out HRegion modelRegion)
        {
            try
            {
                //
                image.InspectShapeModel(out modelRegion, 1, modelParam.Contrast);
                //
                shapeModel = new HShapeModel();
                shapeModel = image.CreateScaledShapeModel("auto", ((new HTuple(modelParam.AngleStart)).TupleRad()), ((new HTuple(modelParam.AngleExtent)).TupleRad()), "auto",
                    modelParam.ScaleMin, modelParam.ScaleMax, "auto", "auto", "use_polarity", modelParam.Contrast, "auto");
                //HXLDCont shapeModelContour;
                //shapeModelContour = shapeModel.GetShapeModelContours(1);

                return true;
            }
            catch (Exception ex)
            {
                modelRegion = new HRegion();
                shapeModel = new HShapeModel();

                return false;
            }
        }

        public static bool CreateScaledShapeModel(HImage image, HRegion region, ModelParam modelParam, out HShapeModel shapeModel, out HRegion modelRegion)
        {
            try
            {
                HImage imageReduce = image.ReduceDomain(region);
                //
                imageReduce.InspectShapeModel(out modelRegion, 1, modelParam.Contrast);
                //
                shapeModel = new HShapeModel();
                shapeModel = imageReduce.CreateScaledShapeModel("auto", ((new HTuple(modelParam.AngleStart)).TupleRad()), ((new HTuple(modelParam.AngleExtent)).TupleRad()), "auto",
                    modelParam.ScaleMin, modelParam.ScaleMax, "auto", "auto", "use_polarity", modelParam.Contrast, "auto");
                return true;
            }

            catch (Exception ex)
            {
                modelRegion = new HRegion();
                shapeModel = new HShapeModel();
                return false;
            }
        }

        public static bool FindScaledShapeModel(HImage image, HShapeModel shapeModel, ModelParam modelParam, out HXLDCont shapeModelContourTrans, out ModelResult modelResult)
        {
            try
            {
                //
                shapeModelContourTrans = new HXLDCont();
                modelResult = new ModelResult();
                //
                double angleStart, angleExtent, angleStep, scaleMin, scaleMax, scaleStep;
                string metric;
                int minContrast;
                //
                shapeModel.GetShapeModelParams(out angleStart, out angleExtent, out angleStep, out scaleMin, out scaleMax, out scaleStep,
                out metric, out minContrast);
                //
                HTuple RowCheck, ColumnCheck, AngleCheck, ScaleCheck, Score;
                shapeModel.FindScaledShapeModel(image, angleStart, angleExtent, scaleMin, scaleMax, modelParam.MinScore, modelParam.NumMatches, modelParam.MaxOverlap, modelParam.SubPixel,
                    modelParam.NumLevels, modelParam.Greediness, out RowCheck, out ColumnCheck, out AngleCheck, out ScaleCheck, out Score);
                //
                modelResult.Row = RowCheck.D;
                modelResult.Column = ColumnCheck.D;
                modelResult.Angle = AngleCheck.TupleDeg().D;
                modelResult.Scale = ScaleCheck.D;
                modelResult.Score = Score.D;
                //
                double ModelRefPointRow=0.0, ModelRefPointCol=0.0;
                //shapeModel.GetShapeModelOrigin(out ModelRefPointRow, out ModelRefPointCol);
                //
                var Matrix = new HHomMat2D();
                Matrix.VectorAngleToRigid(ModelRefPointRow, ModelRefPointCol, 0.0, RowCheck.D, ColumnCheck.D, AngleCheck.D);
                //
                shapeModelContourTrans = shapeModel.GetShapeModelContours(1).AffineTransContourXld(Matrix);

                return true;

            }
            catch (Exception ex)
            {
                shapeModelContourTrans = new HXLDCont();
                modelResult = new ModelResult();
                return false;
            }
        }

        public static bool FindScaledShapeModel(HImage image, HRegion region, HShapeModel shapeModel, ModelParam modelParam, out HXLDCont shapeModelContourTrans, out ModelResult modelResult)
        {
            try
            {
                //
                shapeModelContourTrans = new HXLDCont();
                modelResult = new ModelResult();
                //
                double angleStart, angleExtent, angleStep, scaleMin, scaleMax, scaleStep;
                string metric;
                int minContrast;
                //
                shapeModel.GetShapeModelParams(out angleStart, out angleExtent, out angleStep, out scaleMin, out scaleMax, out scaleStep,
                out metric, out minContrast);
                //
                HTuple RowCheck, ColumnCheck, AngleCheck, ScaleCheck, Score;
                image = image.ReduceDomain(region);
                shapeModel.FindScaledShapeModel(image, angleStart, angleExtent, scaleMin, scaleMax, modelParam.MinScore, modelParam.NumMatches, modelParam.MaxOverlap, modelParam.SubPixel,
                    modelParam.NumLevels, modelParam.Greediness, out RowCheck, out ColumnCheck, out AngleCheck, out ScaleCheck, out Score);
                //
                modelResult.Row = RowCheck.D;
                modelResult.Column = ColumnCheck.D;
                modelResult.Angle = AngleCheck.TupleDeg().D;
                modelResult.Scale = ScaleCheck.D;
                modelResult.Score = Score.D;
                // Rotate the model for visualization purposes.
                double ModelRefPointRow, ModelRefPointCol;
                shapeModel.GetShapeModelOrigin(out ModelRefPointRow, out ModelRefPointCol);
                //
                HHomMat2D Matrix = new HHomMat2D();
                Matrix.VectorAngleToRigid(ModelRefPointRow, ModelRefPointCol, 0.0, RowCheck.D, ColumnCheck.D, AngleCheck.D);
                //
                shapeModelContourTrans = shapeModel.GetShapeModelContours(1).AffineTransContourXld(Matrix);

                return true;

            }
            catch (Exception ex)
            {
                shapeModelContourTrans = new HXLDCont();
                modelResult = new ModelResult();
                return false;
            }
        }

        public static bool GetShapeModelParams(HShapeModel shapeModel, out ModelParam modelParam)
        {
            try
            {
                double angleStart, angleExtent, angleStep, scaleMin, scaleMax, scaleStep;
                string metric;
                int minContrast;
                shapeModel.GetShapeModelParams(out angleStart, out angleExtent, out angleStep, out scaleMin, out scaleMax, out scaleStep, out metric, out minContrast);

                modelParam = new ModelParam();
                modelParam.AngleStart = angleStart;
                modelParam.AngleExtent = angleExtent;
                modelParam.ScaleMin = scaleMin;
                modelParam.ScaleMax = scaleMax;

                return true;
            }
            catch (Exception ex)
            {
                modelParam = new ModelParam();
                return false;
            }
        }

        /// <summary>
        /// 圆形测量
        /// </summary>
        /// <param name="image">输入图像</param>
        /// <param name="roi">输入圆</param>
        /// <param name="measureParam">输入测量参数</param>
        /// <param name="circle">输出结果圆</param>
        /// <param name="row">输出结果圆所有行坐标< </param>
        /// <param name="col">输出结果圆所有列坐标 </param>
        /// <param name="measureXLD">输出测量模型轮廓</param>
        public static void CircleMeasure(HImage image, CircleRoi roi, MeasureParam measureParam, HRegion maskRegion,
            out CircleRoi circle, out HTuple row, out HTuple col, out HXLDCont measureXLD)
        {
            HMetrologyModel hMetrologyModel = new HMetrologyModel();

            try
            {
                circle = new CircleRoi();

                //Create the metrology model and specify the image size
                hMetrologyModel.CreateMetrologyModel();
                int width, height;
                image.GetImageSize(out width, out height);
                hMetrologyModel.SetMetrologyModelImageSize(width, height);

                //Provide approximate values
                //HTuple shapeParam = new HTuple(new object[] { roi.Row, roi.Column, roi.Radius });
                HTuple shapeParam = new HTuple(new double[] { roi.Row, roi.Column, roi.Radius });
                HTuple genParamName = new HTuple(new object[] { "measure_distance", "measure_transition", "measure_select", "min_score" });
                HTuple genParamValue = new HTuple(new object[] { measureParam.MeasureDistance, measureParam.MeasureTransition,
                    measureParam.MeasureSelect, measureParam.MinScore });

                hMetrologyModel.AddMetrologyObjectGeneric("circle", shapeParam, measureParam.MeasureLength1, measureParam.MeasureLength2,
                            measureParam.MeasureSigma, measureParam.MeasureThreshold, genParamName, genParamValue);

                //Apply the measurement
                hMetrologyModel.ApplyMetrologyModel(image);

                //获取测量模型轮廓和测量点坐标
                measureXLD = hMetrologyModel.GetMetrologyObjectMeasures("all", "all", out row, out col);

                //获取测量结果轮廓
                //resultContour=hMetrologyModel.GetMetrologyObjectResultContour(0, "all", 1.5);

                //获取测量结果
                HTuple result = hMetrologyModel.GetMetrologyObjectResult(new HTuple("all"), new HTuple("all"), new HTuple("result_type"), new HTuple("all_param"));

                circle.Row = result[0].D;
                circle.Column = result[1].D;
                circle.Radius = result[2].D;

                hMetrologyModel.Dispose();
            }
            catch (Exception ex)
            {
                circle = new CircleRoi();
                row = new HTuple();
                col = new HTuple();
                measureXLD = new HXLDCont();
                hMetrologyModel.Dispose();
            }
        }


        public static bool TrainDataCode2D(string symbolType, string path, out HDataCode2D dataCode2DModel)
        {
            try
            {
                HImage image = new HImage();
                HTuple resultHandles = new HTuple();
                HTuple decodedDataStrings = new HTuple();

                dataCode2DModel = new HDataCode2D();
                dataCode2DModel.Dispose();
                dataCode2DModel.CreateDataCode2dModel(symbolType, new HTuple(), new HTuple());

                DirectoryInfo theFolder = new DirectoryInfo(path);
                FileInfo[] fileInfo = theFolder.GetFiles();//获取所在目录的文件
                foreach (FileInfo file in fileInfo) //遍历文件
                {
                    image.ReadImage(file.FullName);

                    dataCode2DModel.FindDataCode2d(image, "train", "all", out resultHandles, out decodedDataStrings);
                }

                return true;
            }
            catch (Exception ex)
            {
                dataCode2DModel = new HDataCode2D();
                return false;
            }
        }

        public static bool FindDataCode2D(HImage image, HDataCode2D dataCode2DModel, out HTuple decodedDataStrings)
        {
            try
            {
                HTuple resultHandles = new HTuple();
                decodedDataStrings = new HTuple();

                dataCode2DModel.FindDataCode2d(image, new HTuple(), new HTuple(), out resultHandles, out decodedDataStrings);

                return true;
            }
            catch (Exception ex)
            {
                decodedDataStrings = new HTuple();
                return false;
            }
        }

        public static bool FindDataCode2D(HImage image, HDataCode2D dataCode2DModel, out HXLDCont symbolXLD, out HTuple resultHandles, out HTuple decodedDataStrings)
        {
            try
            {
                symbolXLD = new HXLDCont();
                resultHandles = new HTuple();
                decodedDataStrings = new HTuple();

                symbolXLD.Dispose();
                symbolXLD = dataCode2DModel.FindDataCode2d(image, new HTuple(), new HTuple(), out resultHandles, out decodedDataStrings);

                return true;
            }
            catch (Exception ex)
            {
                symbolXLD = new HXLDCont();
                resultHandles = new HTuple();
                decodedDataStrings = new HTuple();
                return false;
            }
        }

        public static bool FindDataCode2D(HImage image, HRegion region, HDataCode2D dataCode2DModel, out HXLDCont symbolXLD, out HTuple resultHandles, out HTuple decodedDataStrings)
        {
            try
            {
                symbolXLD = new HXLDCont();
                resultHandles = new HTuple();
                decodedDataStrings = new HTuple();

                symbolXLD.Dispose();
                symbolXLD = dataCode2DModel.FindDataCode2d(image.ReduceDomain(region), new HTuple(), new HTuple(), out resultHandles, out decodedDataStrings);

                return true;
            }
            catch (Exception ex)
            {
                symbolXLD = new HXLDCont();
                resultHandles = new HTuple();
                decodedDataStrings = new HTuple();
                return false;
            }
        }
        #endregion

        #region"Laser"

        //激光验证测试代码
        //private void FrmMain_Load(object sender, EventArgs e)
        //{
        //    LocationResult locationResult = new LocationResult();
        //    HShapeModel hShapeModel = new HShapeModel(@"C:\Users\BOBO\Desktop\ocr\location.sbm");
        //    image.ReadImage(@"C:\Users\BOBO\Desktop\ocr\location\location_10.bmp");
        //    VisionModulesHelper.BuzzLocation(image, image.GetDomain(), hShapeModel, new LocationParam(), out locationResult);
        //    HDataCode2D hDataCode2d = new HDataCode2D(@"C:\Users\BOBO\Desktop\ocr\2d_data_code_model.dcm");
        //    HOCRMlp hOcrMlp = new HOCRMlp(@"C:\Users\BOBO\Desktop\ocr\Industrial_0-9_NoRej.omc");
        //    DataCodeAndOcrParam param = new DataCodeAndOcrParam();
        //    param.location = locationResult;
        //    DataCodeAndOcrResult result = new DataCodeAndOcrResult();
        //    VisionModulesHelper.FindDataCodeAndOcr(image, image.GetDomain(), hDataCode2d, hOcrMlp, param, out result);
        //}

        /// <summary>
        /// 一张图片包含九点N点标定
        /// </summary>
        /// <param name="image">输入图像</param>
        /// <param name="machineBasePoint">九点基准坐标</param>
        /// <param name="homMat2D">标定结果映射矩阵</param>
        /// <returns></returns>
        public static bool NPointCalib(HImage image, HRegion region, MachineBasePoint machineBasePoint, out HTuple homMat2D)
        {
            try
            {
                homMat2D = new HTuple();
                //
                double[] machineX = new double[9];
                double[] machineY = new double[9];
                //
                double[] imageR = new double[9];
                double[] imageC = new double[9];
                //
                double[] calibX = new double[] { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
                double[] calibY = new double[] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };

                for (int i = 0; i < 9; i++)
                {
                    machineX[i] = machineBasePoint.BaseX + calibX[i] * machineBasePoint.OffsetX;
                    machineY[i] = machineBasePoint.BaseY + calibY[i] * machineBasePoint.OffsetY;
                    imageR[i] = 0;
                    imageC[i] = 0;
                }

                // Local iconic variables 
                HObject ho_Region, ho_RegionOpening;
                HObject ho_RegionFillUp, ho_ConnectedRegions, ho_SelectedRegions;
                HObject ho_SortedRegions, ho_RegionBorder, ho_Contours;
                HObject ho_ObjectSelected = null;

                // Local control variables 
                HTuple hv_UsedThreshold = new HTuple(), hv_Number = new HTuple();
                HTuple hv_i = new HTuple(), hv_Row = new HTuple(), hv_Column = new HTuple();
                HTuple hv_Radius = new HTuple(), hv_StartPhi = new HTuple();
                HTuple hv_EndPhi = new HTuple(), hv_PointOrder = new HTuple();
                HTuple hv_HomMat2D = new HTuple();

                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_Region);
                HOperatorSet.GenEmptyObj(out ho_RegionOpening);
                HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
                HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                HOperatorSet.GenEmptyObj(out ho_SortedRegions);
                HOperatorSet.GenEmptyObj(out ho_RegionBorder);
                HOperatorSet.GenEmptyObj(out ho_Contours);
                HOperatorSet.GenEmptyObj(out ho_ObjectSelected);

                ho_Region.Dispose(); hv_UsedThreshold.Dispose();
                HOperatorSet.BinaryThreshold(image.ReduceDomain(region), out ho_Region, "max_separability", "light", out hv_UsedThreshold);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_Region, out ho_RegionOpening, 3.5);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUp(ho_RegionOpening, out ho_RegionFillUp);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionFillUp, out ho_ConnectedRegions);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area", "and", 39999, 99999);
                ho_SortedRegions.Dispose();
                HOperatorSet.SortRegion(ho_ConnectedRegions, out ho_SortedRegions, "first_point", "true", "row");
                ho_RegionBorder.Dispose();
                HOperatorSet.Boundary(ho_SortedRegions, out ho_RegionBorder, "inner");
                ho_Contours.Dispose();
                HOperatorSet.GenContoursSkeletonXld(ho_RegionBorder, out ho_Contours, 1, "filter");
                hv_Number.Dispose();
                HOperatorSet.CountObj(ho_Contours, out hv_Number);

                if (hv_Number.D != 9)
                    return false;

                HTuple end_val12 = hv_Number;
                HTuple step_val12 = 1;
                for (hv_i = 1; hv_i.Continue(end_val12, step_val12); hv_i = hv_i.TupleAdd(step_val12))
                {
                    ho_ObjectSelected.Dispose();
                    HOperatorSet.SelectObj(ho_Contours, out ho_ObjectSelected, hv_i);
                    hv_Row.Dispose(); hv_Column.Dispose(); hv_Radius.Dispose(); hv_StartPhi.Dispose(); hv_EndPhi.Dispose(); hv_PointOrder.Dispose();
                    HOperatorSet.FitCircleContourXld(ho_ObjectSelected, "algebraic", -1, 0, 0, 3, 2, out hv_Row, out hv_Column, out hv_Radius, out hv_StartPhi, out hv_EndPhi, out hv_PointOrder);

                    imageR[hv_i.I - 1] = hv_Row;
                    imageC[hv_i.I - 1] = hv_Column;
                }
                //齐次变换矩阵=HomMat2D
                hv_HomMat2D.Dispose();
                HOperatorSet.VectorToHomMat2d(imageR, imageC, machineX, machineY, out homMat2D);
                return true;
            }
            catch (Exception ex)
            {
                homMat2D = new HTuple();
                return false;
            }

        }

        //LocationResult locationResult = new LocationResult();
        //HShapeModel hShapeModel = new HShapeModel(@"C:\Users\BOBO\Desktop\ocr\location.sbm");
        //image.ReadImage(@"C:\Users\BOBO\Desktop\ocr\location\location_10.bmp");
        //VisionModulesHelper.BuzzLocation(image, image.GetDomain(), hShapeModel, new LocationParam(), out locationResult);

        //HDataCode2D hDataCode2d = new HDataCode2D(@"C:\Users\BOBO\Desktop\ocr\2d_data_code_model.dcm");
        //HOCRMlp hOcrMlp=new HOCRMlp(@"C:\Users\BOBO\Desktop\ocr\Industrial_0-9_NoRej.omc");
        //DataCodeAndOcrParam param = new DataCodeAndOcrParam();
        //param.location = locationResult;
        //DataCodeAndOcrResult result = new DataCodeAndOcrResult();
        //VisionModulesHelper.FindDataCodeAndOcr(image, image.GetDomain(), hDataCode2d, hOcrMlp,param, out result);

        /// <summary>
        /// Buzz定位
        /// </summary>
        /// <param name="image">输入图像</param>
        /// <param name="region">ROI</param>
        /// <param name="shapeModel">定位形状模板</param>
        /// <param name="locationParam">定位参数</param>
        /// <param name="locationResult">定位结果</param>
        /// <returns></returns>
        public static bool BuzzLocation(HImage image, HRegion region, HShapeModel shapeModel, LocationParam locationParam, out LocationResult locationResult)
        {
            try
            {
                //
                locationResult = new LocationResult();
                // Local iconic variables 
                HObject ho_Rectangle, ho_RegionAffineTrans;
                HObject ho_ImageReduced, ho_RegionChar, ho_RegionOpening;
                // Local control variables 
                HTuple hv_Width = new HTuple(), hv_Height = new HTuple();
                HTuple hv_ModelID = new HTuple(), hv_RowC = new HTuple();
                HTuple hv_ColumnC = new HTuple(), hv_Angle = new HTuple();
                HTuple hv_Scale = new HTuple(), hv_Score = new HTuple();
                HTuple hv_CR = new HTuple(), hv_CC = new HTuple(), hv_HomMat2DIdentity = new HTuple();
                HTuple hv_HomMat2DRotate = new HTuple(), hv_UsedThreshold = new HTuple();
                HTuple hv_AreaChar = new HTuple(), hv_Row = new HTuple();
                HTuple hv_Column = new HTuple(), hv_AreaRegion = new HTuple();
                HTuple hv_MR = new HTuple(), hv_MC = new HTuple();
                HTuple hv_MR1 = new HTuple(), hv_MC1 = new HTuple(), hv_RowBegin1 = new HTuple();
                HTuple hv_ColBegin1 = new HTuple(), hv_RowEnd1 = new HTuple();
                HTuple hv_ColEnd1 = new HTuple(), hv_RowBegin2 = new HTuple();
                HTuple hv_ColBegin2 = new HTuple(), hv_RowEnd2 = new HTuple();
                HTuple hv_ColEnd2 = new HTuple(), hv_RowBegin3 = new HTuple();
                HTuple hv_ColBegin3 = new HTuple(), hv_RowEnd3 = new HTuple();
                HTuple hv_ColEnd3 = new HTuple(), hv_RowBegin4 = new HTuple();
                HTuple hv_ColBegin4 = new HTuple(), hv_RowEnd4 = new HTuple();
                HTuple hv_ColEnd4 = new HTuple(), hv_MetrologyHandle = new HTuple();
                HTuple hv_Index1 = new HTuple(), hv_Index2 = new HTuple();
                HTuple hv_Index3 = new HTuple(), hv_Index4 = new HTuple();
                HTuple hv_Parameter = new HTuple(), hv_Row1 = new HTuple();
                HTuple hv_Column1 = new HTuple(), hv_IsOverlapping = new HTuple();
                HTuple hv_Row2 = new HTuple(), hv_Column2 = new HTuple();
                HTuple hv_RowF = new HTuple(), hv_ColF = new HTuple();
                HTuple hv_Phi = new HTuple();
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                HOperatorSet.GenEmptyObj(out ho_RegionChar);
                HOperatorSet.GenEmptyObj(out ho_RegionOpening);
                //
                double angleStart, angleExtent, angleStep, scaleMin, scaleMax, scaleStep;
                string metric;
                int minContrast;
                //
                shapeModel.GetShapeModelParams(out angleStart, out angleExtent, out angleStep, out scaleMin, out scaleMax, out scaleStep,
                out metric, out minContrast);
                //
                shapeModel.FindScaledShapeModel(image.ReduceDomain(region), angleStart, angleExtent, scaleMin, scaleMax, locationParam.MinScore, locationParam.NumMatches, locationParam.MaxOverlap, locationParam.SubPixel,
                    locationParam.NumLevels, locationParam.Greediness, out hv_RowC, out hv_ColumnC, out hv_Angle, out hv_Scale, out hv_Score);
                //
                if (hv_RowC.Length != 1)
                    return false;

                //判断有无字符
                hv_CR.Dispose();
                hv_CR = new HTuple();
                hv_CR[0] = -260;
                hv_CR[1] = -90;
                hv_CC.Dispose();
                hv_CC = new HTuple();
                hv_CC[0] = -420;
                hv_CC[1] = 520;
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle, hv_RowC + (hv_CR.TupleSelect(0)),
                        hv_ColumnC + (hv_CC.TupleSelect(0)), hv_RowC + (hv_CR.TupleSelect(1)), hv_ColumnC + (hv_CC.TupleSelect(
                        1)));
                }
                hv_HomMat2DIdentity.Dispose();
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                hv_HomMat2DRotate.Dispose();
                HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity, hv_Angle, hv_RowC, hv_ColumnC,
                    out hv_HomMat2DRotate);
                ho_RegionAffineTrans.Dispose();
                HOperatorSet.AffineTransRegion(ho_Rectangle, out ho_RegionAffineTrans, hv_HomMat2DRotate,
                    "nearest_neighbor");
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(image, ho_RegionAffineTrans, out ho_ImageReduced
                    );
                ho_RegionChar.Dispose(); hv_UsedThreshold.Dispose();
                HOperatorSet.BinaryThreshold(ho_ImageReduced, out ho_RegionChar, "max_separability",
                    "dark", out hv_UsedThreshold);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionChar, out ho_RegionOpening, 3.5);
                hv_AreaChar.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
                HOperatorSet.AreaCenter(ho_RegionOpening, out hv_AreaChar, out hv_Row, out hv_Column);
                hv_AreaRegion.Dispose(); hv_Row.Dispose(); hv_Column.Dispose();
                HOperatorSet.AreaCenter(ho_RegionAffineTrans, out hv_AreaRegion, out hv_Row,
                    out hv_Column);
                if (hv_AreaChar.D / hv_AreaRegion.D < locationParam.ratioArea || hv_AreaChar.D / hv_AreaRegion.D > 1 - locationParam.ratioArea)
                    locationResult.IsCode = false;
                else
                    locationResult.IsCode = true;
                //
                hv_MR.Dispose();
                hv_MR = new HTuple();
                hv_MR[0] = -15.8008;
                hv_MR[1] = -15.8008;
                hv_MR[2] = -5.80079;
                hv_MR[3] = 21.1992;
                hv_MR[4] = 21.1992;
                hv_MR[5] = -5.80079;
                hv_MR[6] = -15.8008;
                hv_MR[7] = -15.8008;
                hv_MC.Dispose();
                hv_MC = new HTuple();
                hv_MC[0] = -318.196;
                hv_MC[1] = -238.196;
                hv_MC[2] = -196.196;
                hv_MC[3] = -196.196;
                hv_MC[4] = 169.804;
                hv_MC[5] = 169.804;
                hv_MC[6] = 211.804;
                hv_MC[7] = 311.804;
                hv_MR1.Dispose();
                HOperatorSet.TupleGenConst(8, hv_RowC, out hv_MR1);
                hv_MC1.Dispose();
                HOperatorSet.TupleGenConst(8, hv_ColumnC, out hv_MC1);
                {
                    HTuple ExpTmpOutVar_0;
                    HOperatorSet.TupleAdd(hv_MR, hv_MR1, out ExpTmpOutVar_0);
                    hv_MR.Dispose();
                    hv_MR = ExpTmpOutVar_0;
                }
                {
                    HTuple ExpTmpOutVar_0;
                    HOperatorSet.TupleAdd(hv_MC, hv_MC1, out ExpTmpOutVar_0);
                    hv_MC.Dispose();
                    hv_MC = ExpTmpOutVar_0;
                }

                hv_HomMat2DIdentity.Dispose();
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                hv_HomMat2DRotate.Dispose();
                HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity, hv_Angle, hv_RowC, hv_ColumnC,
                    out hv_HomMat2DRotate);
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowBegin1.Dispose(); hv_ColBegin1.Dispose();
                    HOperatorSet.AffineTransPixel(hv_HomMat2DRotate, hv_MR.TupleSelect(0), hv_MC.TupleSelect(
                        0), out hv_RowBegin1, out hv_ColBegin1);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowEnd1.Dispose(); hv_ColEnd1.Dispose();
                    HOperatorSet.AffineTransPixel(hv_HomMat2DRotate, hv_MR.TupleSelect(1), hv_MC.TupleSelect(
                        1), out hv_RowEnd1, out hv_ColEnd1);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowBegin2.Dispose(); hv_ColBegin2.Dispose();
                    HOperatorSet.AffineTransPixel(hv_HomMat2DRotate, hv_MR.TupleSelect(2), hv_MC.TupleSelect(
                        2), out hv_RowBegin2, out hv_ColBegin2);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowEnd2.Dispose(); hv_ColEnd2.Dispose();
                    HOperatorSet.AffineTransPixel(hv_HomMat2DRotate, hv_MR.TupleSelect(3), hv_MC.TupleSelect(
                        3), out hv_RowEnd2, out hv_ColEnd2);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowBegin3.Dispose(); hv_ColBegin3.Dispose();
                    HOperatorSet.AffineTransPixel(hv_HomMat2DRotate, hv_MR.TupleSelect(4), hv_MC.TupleSelect(
                        4), out hv_RowBegin3, out hv_ColBegin3);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowEnd3.Dispose(); hv_ColEnd3.Dispose();
                    HOperatorSet.AffineTransPixel(hv_HomMat2DRotate, hv_MR.TupleSelect(5), hv_MC.TupleSelect(
                        5), out hv_RowEnd3, out hv_ColEnd3);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowBegin4.Dispose(); hv_ColBegin4.Dispose();
                    HOperatorSet.AffineTransPixel(hv_HomMat2DRotate, hv_MR.TupleSelect(6), hv_MC.TupleSelect(
                        6), out hv_RowBegin4, out hv_ColBegin4);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_RowEnd4.Dispose(); hv_ColEnd4.Dispose();
                    HOperatorSet.AffineTransPixel(hv_HomMat2DRotate, hv_MR.TupleSelect(7), hv_MC.TupleSelect(
                        7), out hv_RowEnd4, out hv_ColEnd4);
                }

                hv_MetrologyHandle.Dispose();
                HOperatorSet.CreateMetrologyModel(out hv_MetrologyHandle);
                hv_Width.Dispose(); hv_Height.Dispose();
                HOperatorSet.GetImageSize(image, out hv_Width, out hv_Height);
                HOperatorSet.SetMetrologyModelImageSize(hv_MetrologyHandle, hv_Width, hv_Height);
                hv_Index1.Dispose();
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_RowBegin1,
                    hv_ColBegin1, hv_RowEnd1, hv_ColEnd1, 20, 5, 1, 20, (new HTuple("measure_transition")).TupleConcat(
                    "measure_distance"), (new HTuple("negative")).TupleConcat(2), out hv_Index1);
                hv_Index2.Dispose();
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_RowBegin2,
                    hv_ColBegin2, hv_RowEnd2, hv_ColEnd2, 20, 5, 1, 20, (new HTuple("measure_transition")).TupleConcat(
                    "measure_distance"), (new HTuple("negative")).TupleConcat(2), out hv_Index2);
                hv_Index3.Dispose();
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_RowBegin3,
                    hv_ColBegin3, hv_RowEnd3, hv_ColEnd3, 20, 5, 1, 20, (new HTuple("measure_transition")).TupleConcat(
                    "measure_distance"), (new HTuple("negative")).TupleConcat(2), out hv_Index3);
                hv_Index4.Dispose();
                HOperatorSet.AddMetrologyObjectLineMeasure(hv_MetrologyHandle, hv_RowBegin4,
                    hv_ColBegin4, hv_RowEnd4, hv_ColEnd4, 20, 5, 1, 20, (new HTuple("measure_transition")).TupleConcat(
                    "measure_distance"), (new HTuple("negative")).TupleConcat(2), out hv_Index4);
                HOperatorSet.ApplyMetrologyModel(image, hv_MetrologyHandle);
                hv_Parameter.Dispose();
                HOperatorSet.GetMetrologyObjectResult(hv_MetrologyHandle, "all", "all", "result_type",
                    "all_param", out hv_Parameter);
                //
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Row1.Dispose(); hv_Column1.Dispose(); hv_IsOverlapping.Dispose();
                    HOperatorSet.IntersectionLines(hv_Parameter.TupleSelect(0), hv_Parameter.TupleSelect(
                        1), hv_Parameter.TupleSelect(2), hv_Parameter.TupleSelect(3), hv_Parameter.TupleSelect(
                        4), hv_Parameter.TupleSelect(5), hv_Parameter.TupleSelect(6), hv_Parameter.TupleSelect(
                        7), out hv_Row1, out hv_Column1, out hv_IsOverlapping);
                }
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    hv_Row2.Dispose(); hv_Column2.Dispose(); hv_IsOverlapping.Dispose();
                    HOperatorSet.IntersectionLines(hv_Parameter.TupleSelect(8), hv_Parameter.TupleSelect(
                        9), hv_Parameter.TupleSelect(10), hv_Parameter.TupleSelect(11), hv_Parameter.TupleSelect(
                        12), hv_Parameter.TupleSelect(13), hv_Parameter.TupleSelect(14), hv_Parameter.TupleSelect(
                        15), out hv_Row2, out hv_Column2, out hv_IsOverlapping);
                }

                hv_RowF.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    locationResult.Row = (hv_Row1.D + hv_Row2.D) / 2;
                }
                hv_ColF.Dispose();
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    locationResult.Column = (hv_Column1.D + hv_Column2.D) / 2;
                }
                HTuple angle = new HTuple();
                HOperatorSet.LineOrientation(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out angle);
                locationResult.Angle = angle;

                return true;
            }
            catch (Exception ex)
            {
                locationResult = new LocationResult();
                return false;
            }
        }

        public static bool MorganLocation(HImage image, HRegion region, HShapeModel shapeModel, LocationParam locationParam, out LocationResult locationResult)
        {
            locationResult = new LocationResult();
            return true;
        }

        public static bool FindDataCodeAndOcr(HImage image, HRegion region, HDataCode2D dataCode2D, HOCRMlp ocrMlp, DataCodeAndOcrParam param, out DataCodeAndOcrResult result)
        {
            try
            {
                result = new DataCodeAndOcrResult();

                // Local iconic variables 
                HObject ho_Rectangle, ho_RegionAffineTrans;
                HObject ho_ImageReduced, ho_SymbolXLDs, ho_Region, ho_ImageReduced1 = null;
                HObject ho_Region1, ho_RegionFillUp, ho_RegionOpening, ho_ConnectedRegions;
                HObject ho_SelectedRegions, ho_SortedRegions;

                // Local control variables   
                HTuple hv_HomMat2DIdentity = new HTuple(), hv_HomMat2DRotate = new HTuple();
                HTuple hv_MR = new HTuple(), hv_MC = new HTuple(), hv_MR1 = new HTuple();
                HTuple hv_MC1 = new HTuple();
                HTuple hv_ResultHandles = new HTuple(), hv_DecodedDataStrings = new HTuple();
                HTuple hv_UsedThreshold = new HTuple(), hv_AreaBlack = new HTuple();
                HTuple hv_Row = new HTuple(), hv_Column = new HTuple();
                HTuple hv_AreaAllROI = new HTuple(), hv_Row1 = new HTuple();
                HTuple hv_Column1 = new HTuple(), hv_K = new HTuple();
                HTuple hv_UsedThreshold1 = new HTuple();
                HTuple hv_Class = new HTuple(), hv_Confidence = new HTuple();
                // Initialize local and output iconic variables 
                HOperatorSet.GenEmptyObj(out ho_Rectangle);
                HOperatorSet.GenEmptyObj(out ho_RegionAffineTrans);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced);
                HOperatorSet.GenEmptyObj(out ho_SymbolXLDs);
                HOperatorSet.GenEmptyObj(out ho_Region);
                HOperatorSet.GenEmptyObj(out ho_ImageReduced1);
                HOperatorSet.GenEmptyObj(out ho_Region1);
                HOperatorSet.GenEmptyObj(out ho_RegionFillUp);
                HOperatorSet.GenEmptyObj(out ho_RegionOpening);
                HOperatorSet.GenEmptyObj(out ho_ConnectedRegions);
                HOperatorSet.GenEmptyObj(out ho_SelectedRegions);
                HOperatorSet.GenEmptyObj(out ho_SortedRegions);

                hv_HomMat2DIdentity.Dispose();
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
                hv_HomMat2DRotate.Dispose();
                HOperatorSet.HomMat2dRotate(hv_HomMat2DIdentity, new HTuple(param.location.Angle), new HTuple(param.location.Row),
                    new HTuple(param.location.Column), out hv_HomMat2DRotate);

                hv_MR.Dispose();
                hv_MR = new HTuple();
                hv_MR[0] = -286.628;
                hv_MR[1] = -66.6282;
                hv_MR[2] = -226.628;
                hv_MR[3] = -86.6282;
                hv_MC.Dispose();
                hv_MC = new HTuple();
                hv_MC[0] = -394.593;
                hv_MC[1] = -174.593;
                hv_MC[2] = -184.593;
                hv_MC[3] = 525.407;
                hv_MR1.Dispose();
                HOperatorSet.TupleGenConst(4, new HTuple(param.location.Row), out hv_MR1);
                hv_MC1.Dispose();
                HOperatorSet.TupleGenConst(4, new HTuple(param.location.Column), out hv_MC1);
                {
                    HTuple ExpTmpOutVar_0;
                    HOperatorSet.TupleAdd(hv_MR, hv_MR1, out ExpTmpOutVar_0);
                    hv_MR.Dispose();
                    hv_MR = ExpTmpOutVar_0;
                }
                {
                    HTuple ExpTmpOutVar_0;
                    HOperatorSet.TupleAdd(hv_MC, hv_MC1, out ExpTmpOutVar_0);
                    hv_MC.Dispose();
                    hv_MC = ExpTmpOutVar_0;
                }

                //二维码
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle, hv_MR.TupleSelect(0), hv_MC.TupleSelect(
                        0), hv_MR.TupleSelect(1), hv_MC.TupleSelect(1));
                }
                ho_RegionAffineTrans.Dispose();
                HOperatorSet.AffineTransRegion(ho_Rectangle, out ho_RegionAffineTrans, hv_HomMat2DRotate,
                    "nearest_neighbor");
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(image, ho_RegionAffineTrans, out ho_ImageReduced
                    );
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ScaleImageMax(ho_ImageReduced, out ExpTmpOutVar_0);
                    ho_ImageReduced.Dispose();
                    ho_ImageReduced = ExpTmpOutVar_0;
                }

                ho_SymbolXLDs.Dispose(); hv_ResultHandles.Dispose(); hv_DecodedDataStrings.Dispose();
                HOperatorSet.FindDataCode2d(ho_ImageReduced, out ho_SymbolXLDs, dataCode2D,
                    new HTuple(), new HTuple(), out hv_ResultHandles, out hv_DecodedDataStrings);
                result.dataCode2DString = hv_DecodedDataStrings.S;

                //字符识别
                using (HDevDisposeHelper dh = new HDevDisposeHelper())
                {
                    ho_Rectangle.Dispose();
                    HOperatorSet.GenRectangle1(out ho_Rectangle, hv_MR.TupleSelect(2), hv_MC.TupleSelect(
                        2), hv_MR.TupleSelect(3), hv_MC.TupleSelect(3));
                }
                ho_RegionAffineTrans.Dispose();
                HOperatorSet.AffineTransRegion(ho_Rectangle, out ho_RegionAffineTrans, hv_HomMat2DRotate,
                    "nearest_neighbor");
                ho_ImageReduced.Dispose();
                HOperatorSet.ReduceDomain(image, ho_RegionAffineTrans, out ho_ImageReduced
                    );
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ScaleImageMax(ho_ImageReduced, out ExpTmpOutVar_0);
                    ho_ImageReduced.Dispose();
                    ho_ImageReduced = ExpTmpOutVar_0;
                }

                if (param.location.IsDark)
                {
                    //黑底白字
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.InvertImage(ho_ImageReduced1, out ExpTmpOutVar_0);
                    ho_ImageReduced1.Dispose();
                    ho_ImageReduced1 = ExpTmpOutVar_0;
                }
                //白底黑字
                ho_Region1.Dispose(); hv_UsedThreshold1.Dispose();
                HOperatorSet.BinaryThreshold(ho_ImageReduced, out ho_Region1, "max_separability",
                    "dark", out hv_UsedThreshold1);
                ho_RegionFillUp.Dispose();
                HOperatorSet.FillUpShape(ho_Region1, out ho_RegionFillUp, "area", 1, 19);
                ho_RegionOpening.Dispose();
                HOperatorSet.OpeningCircle(ho_RegionFillUp, out ho_RegionOpening, 1.5);
                ho_ConnectedRegions.Dispose();
                HOperatorSet.Connection(ho_RegionOpening, out ho_ConnectedRegions);
                ho_SelectedRegions.Dispose();
                HOperatorSet.SelectShape(ho_ConnectedRegions, out ho_SelectedRegions, "area",
                    "and", 150, 99999);
                ho_SortedRegions.Dispose();
                HOperatorSet.SortRegion(ho_SelectedRegions, out ho_SortedRegions, "first_point",
                    "true", "column");
                hv_Class.Dispose(); hv_Confidence.Dispose();
                HOperatorSet.DoOcrMultiClassMlp(ho_SortedRegions, ho_ImageReduced, ocrMlp,
                    out hv_Class, out hv_Confidence);
                //*
                result.ocrString = hv_Class.ToString();
                return true;

            }
            catch (Exception ex)
            {
                result = new DataCodeAndOcrResult();
                return false;
            }
        }






    }





    #endregion






}





