using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionControls;

namespace VisionFlows
{
   public class ImageHelper
    {
        /// <summary>
        /// 下相机拍标定板上的mark点
        /// </summary>
        /// <returns></returns>
        static HTuple hv_ModelIDDwonCalib = null;
        public static bool DownCamFindMarkCenter(HImage image, SuperWind wind, HRegion roi, out HTuple row, out HTuple col)
        {
           
            
            row = 0;
            col = 0;
            HImage ReducedImage = image.ReduceDomain(roi);
            if (hv_ModelIDDwonCalib == null)
            {
                HOperatorSet.GenCircleContourXld(out HObject ho_ContCircle, 1379.77, 2570.15, 736, 0, 6.28318, "positive", 1);
                HOperatorSet.CreateScaledShapeModelXld(ho_ContCircle, "auto", (new HTuple(-20)).TupleRad(), (new HTuple(20)).TupleRad()
                        , "auto", 0.95, 1.05, "auto", "auto", "ignore_local_polarity", 2, out hv_ModelIDDwonCalib);
                ho_ContCircle.Dispose();
            }
            HOperatorSet.FindScaledShapeModel(ReducedImage, hv_ModelIDDwonCalib, (new HTuple(-15)).TupleRad(), (new HTuple(15)).TupleRad(), 0.9, 1.1,
                0.6, 2, 0.5, "least_squares", 0, 0.9, out row, out col, out HTuple hv_Angle,
                out HTuple hv_Scale, out HTuple hv_Score);
            ReducedImage.Dispose();
            if ((int)(new HTuple((new HTuple(row.TupleLength())).TupleEqual(1))) != 0)
            {
                HOperatorSet.GenCrossContourXld(out HObject ho_Cross, row, col, 56, 0);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, row.TupleSelect(0), col.TupleSelect(0), 0, out HTuple hv_HomMat2D1);
                HOperatorSet.GetShapeModelContours(out HObject ho_ModelContours, hv_ModelIDDwonCalib, 1);
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out HObject ho_ContoursAffineTrans1, hv_HomMat2D1);
                wind.obj = ho_ContoursAffineTrans1;
                wind.obj = ho_Cross;
                ho_ModelContours.Dispose();
                hv_HomMat2D1.Dispose();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 下相机识别吸嘴的中心，使用的模板匹配，需优化
        /// </summary>
        /// <returns></returns>
        static HTuple hv_ModelID_Nozzl = null;
        public static bool DownCamFindNozzlCenter(HImage image, SuperWind wind, HRegion roi, out HTuple row, out HTuple col,out HTuple angle)
        {
            angle = 0;
            row = 0;
            col = 0;
            HImage ReducedImage = image.ReduceDomain(roi);
            if (hv_ModelID_Nozzl == null)
            {
                HOperatorSet.GenCircleContourXld(out HObject ho_ContCircle, 945.172, 998.004, 100, 0, 6.28318, "positive", 1);
                HOperatorSet.CreateScaledShapeModelXld(ho_ContCircle, "auto", (new HTuple(-20)).TupleRad(), (new HTuple(20)).TupleRad()
                        , "auto", 0.95, 1.05, "auto", "auto", "ignore_local_polarity", 5, out hv_ModelID_Nozzl);
                ho_ContCircle.Dispose();
            }
            HOperatorSet.FindScaledShapeModel(ReducedImage, hv_ModelID_Nozzl, (new HTuple(-20)).TupleRad(), (new HTuple(20)).TupleRad(), 0.95, 1.01,
                0.6, 2, 0.5, "least_squares", 0, 0.9, out HTuple hv_Row, out HTuple hv_Column, out HTuple hv_Angle,
                out HTuple hv_Scale, out HTuple hv_Score);
            ReducedImage.Dispose();
            if ((int)(new HTuple((new HTuple(hv_Row.TupleLength())).TupleEqual(2))) != 0)
            {
                row = ((hv_Row.TupleSelect(0)) + (hv_Row.TupleSelect(1))) / 2;
                col = ((hv_Column.TupleSelect(0)) + (hv_Column.TupleSelect(1))) / 2;
                HOperatorSet.GenCrossContourXld(out HObject ho_Cross, row, col, 150, 0);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row.TupleSelect(0), hv_Column.TupleSelect(0), 0, out HTuple hv_HomMat2D1);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row.TupleSelect(1), hv_Column.TupleSelect(1), 0, out HTuple hv_HomMat2D2);
                HOperatorSet.GetShapeModelContours(out HObject ho_ModelContours, hv_ModelID_Nozzl, 1);
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out HObject ho_ContoursAffineTrans1, hv_HomMat2D1);
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out HObject ho_ContoursAffineTrans2, hv_HomMat2D2);                
                HOperatorSet.ConcatObj(ho_ContoursAffineTrans1, ho_ContoursAffineTrans2, out HObject ho_TowCircle);

                HOperatorSet.SortContoursXld(ho_TowCircle, out HObject sortObj, "character", "true", "column");
                HOperatorSet.CountObj(sortObj, out HTuple number);
                if (number!=2)
                {
                    return false;
                }
                HOperatorSet.SelectObj(sortObj, out HObject obj1, 1);      
                HOperatorSet.AreaCenterXld(obj1, out HTuple area1, out HTuple rowx1, out HTuple col1, out HTuple pointer);
                HOperatorSet.SelectObj(sortObj, out HObject obj2, 2);
                HOperatorSet.AreaCenterXld(obj2, out HTuple area2, out HTuple rowx2, out HTuple col2, out HTuple pointer1);
                HOperatorSet.AngleLx(rowx1, col1, rowx2, col2, out angle);

                wind.obj = ho_TowCircle;
                wind.obj = ho_Cross;
                ho_ModelContours.Dispose();
                ho_ContoursAffineTrans1.Dispose();
                ho_ContoursAffineTrans2.Dispose();
                hv_HomMat2D1.Dispose();
                hv_HomMat2D2.Dispose();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 上相机拍同轴上的mark点
        /// </summary>
        /// <returns></returns>
      static  HTuple hv_ModelIDUpCalib;
        public static bool UpCamFindMarkCenter(HImage image, SuperWind wind, HRegion roi, out HTuple row, out HTuple col)
        {
            row = 0;
            col = 0;
            HImage ReducedImage = image.ReduceDomain(roi);
            if (hv_ModelIDUpCalib == null)
            {
                HOperatorSet.GenCircleContourXld(out HObject ho_ContCircle, 1379.77, 2570.15, 620, 0, 6.28318, "positive", 1);
                HOperatorSet.CreateScaledShapeModelXld(ho_ContCircle, "auto", (new HTuple(-20)).TupleRad(), (new HTuple(20)).TupleRad()
                        , "auto", 0.95, 1.05, "auto", "auto", "ignore_local_polarity", 3, out hv_ModelIDUpCalib);
                ho_ContCircle.Dispose();
            }
            HOperatorSet.FindScaledShapeModel(ReducedImage, hv_ModelIDUpCalib, (new HTuple(-20)).TupleRad(), (new HTuple(20)).TupleRad(), 0.95, 1.05,
                0.7, 2, 0.5, "least_squares", 0, 0.9, out row, out col, out HTuple hv_Angle,
                out HTuple hv_Scale, out HTuple hv_Score);
            ReducedImage.Dispose();
            if ((int)(new HTuple((new HTuple(row.TupleLength())).TupleEqual(1))) != 0)
            {
                HOperatorSet.GenCrossContourXld(out HObject ho_Cross, row, col, 156, 0);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, row.TupleSelect(0), col.TupleSelect(0), 0, out HTuple hv_HomMat2D1);
                HOperatorSet.GetShapeModelContours(out HObject ho_ModelContours, hv_ModelIDUpCalib, 1);
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out HObject ho_ContoursAffineTrans1, hv_HomMat2D1);
                wind.obj = ho_ContoursAffineTrans1;
                wind.obj = ho_Cross;
                ho_ModelContours.Dispose();
                hv_HomMat2D1.Dispose();
                return true;
            }
            return false;
        }
        /// <summary>
        /// 上相机拍九点标定的圆
        /// </summary>
        /// <returns></returns>
        public static bool UpCamFindCalibCenter(HImage image, SuperWind wind, HRegion roi, out HTuple hv_Row, out HTuple hv_Column)
        {
            //   hv_Row = hv_Column = 0;
            //   HImage ImageReduced = image.ReduceDomain(roi);
            //   HOperatorSet.BinaryThreshold(ImageReduced, out HObject ho_Region, "max_separability", "light",
            //out HTuple hv_UsedThreshold);
            //   HOperatorSet.FillUp(ho_Region, out HObject ho_RegionFillUp);
            //   HOperatorSet.ReduceDomain(image, ho_RegionFillUp, out HObject ho_ImageReduced);
            //   HOperatorSet.ThresholdSubPix(ho_ImageReduced, out HObject ho_Border, 128);
            //   HOperatorSet.SelectShapeXld(ho_Border, out HObject ho_SelectedXLD, (new HTuple("circularity")).TupleConcat(
            //       "contlength"), "and", (new HTuple(0.7)).TupleConcat(10), (new HTuple(1)).TupleConcat(20000000));
            //   HOperatorSet.CountObj(ho_SelectedXLD, out HTuple hv_Number);
            //   ho_Region.Dispose();
            //   ho_RegionFillUp.Dispose();
            //   ho_ImageReduced.Dispose();
            //   ho_Border.Dispose();
            //   ImageReduced.Dispose();
            //   if ((int)(new HTuple(hv_Number.TupleEqual(1))) != 0)
            //   {
            //       HOperatorSet.SmallestCircleXld(ho_SelectedXLD, out  hv_Row, out  hv_Column, out HTuple radio);
            //       HOperatorSet.GenCircleContourXld(out HObject circe_xld, hv_Row, hv_Column, radio, 0, 6.28, "positive", 1);
            //       HOperatorSet.GenCrossContourXld(out HObject ho_Cross, hv_Row, hv_Column, 56, 0.785398);
            //       wind.obj = circe_xld;
            //       wind.obj = ho_Cross;
            //       ho_SelectedXLD.Dispose();
            //       return true;
            //   }
            //   return false;
            hv_Row = 0;
            hv_Column = 0;
            HImage ReducedImage = image.ReduceDomain(roi);
            if (hv_ModelIDUpCalib == null)
            {
                HOperatorSet.GenCircleContourXld(out HObject ho_ContCircle, 1379.77, 2570.15, 230, 0, 6.28318, "positive", 1);
                HOperatorSet.CreateScaledShapeModelXld(ho_ContCircle, "auto", (new HTuple(-20)).TupleRad(), (new HTuple(20)).TupleRad()
                        , "auto", 0.95, 1.05, "auto", "auto", "ignore_local_polarity", 3, out hv_ModelIDUpCalib);
                ho_ContCircle.Dispose();
            }
            HOperatorSet.FindScaledShapeModel(ReducedImage, hv_ModelIDUpCalib, (new HTuple(-20)).TupleRad(), (new HTuple(20)).TupleRad(), 0.95, 1.05,
                0.7, 2, 0.5, "least_squares", 0, 0.9, out hv_Row, out hv_Column, out HTuple hv_Angle,
                out HTuple hv_Scale, out HTuple hv_Score);
            ReducedImage.Dispose();
            if ((int)(new HTuple((new HTuple(hv_Row.TupleLength())).TupleEqual(1))) != 0)
            {
                HOperatorSet.GenCrossContourXld(out HObject ho_Cross, hv_Row, hv_Column, 156, 0);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, hv_Row.TupleSelect(0), hv_Column.TupleSelect(0), 0, out HTuple hv_HomMat2D1);
                HOperatorSet.GetShapeModelContours(out HObject ho_ModelContours, hv_ModelIDUpCalib, 1);
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out HObject ho_ContoursAffineTrans1, hv_HomMat2D1);
                wind.obj = ho_ContoursAffineTrans1;
                wind.obj = ho_Cross;
                ho_ModelContours.Dispose();
                hv_HomMat2D1.Dispose();
                return true;
            }
            return false;
        }

        public static bool DownCameraFindDut(HImage hImage,  HRegion hRegion, out HTuple CenterRow, out HTuple CenterCol, out HTuple Angle)
        {
            CenterRow = 0;
            CenterCol = 0;
            Angle = 0;
            try
            {
               HTuple modelRow, modelCol, angle, score;
                HOperatorSet.ReadShapeModel(Utility.ModelFile + "SecondDutBack.sbm", out HTuple modeID);
                HOperatorSet.FindShapeModel(hImage, modeID, -0.7, 0.7, 0.6, 5, 0.5, "least_squares", 0, 0.9, out HTuple find_row, out HTuple find_col, out HTuple find_rad, out  score);
                if (score.Length != 5)
                {
                    for (int i = -3; i < 6; i++)
                    {
                        HImage SaleImage = hImage.ScaleImage((HTuple)1, i * 10);
                        HOperatorSet.FindShapeModel(hImage, modeID, -0.7, 0.7, 0.6, 5, 0.5, "least_squares", 0, 0.9, out find_row, out find_col, out find_rad, out score);
                        SaleImage.Dispose();
                        if (score.Length == 5)
                        {
                            break;
                        }
                    }
                }
                if (score.Length != 5)
                {
                    
                    return false;
                }

                HTuple Indices = find_col.TupleSortIndex();
                HTuple Length = Indices.TupleLength();
                double MaxRow = find_row[(int)Indices[Length - 1]];
                double MaxCol = find_col[(int)Indices[Length - 1]];
                double MinRow = find_row[(int)Indices[0]];
                double MinCol = find_row[(int)Indices[0]];
                 CenterRow = (MaxRow + MinRow) / 2;
                 CenterCol = (MaxCol + MinCol) / 2;
                HOperatorSet.AngleLl(0, 0, 0, 100, MinRow, MinCol, MaxRow, MaxCol, out  Angle);
                
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

                return true;
            }
            catch (Exception ex)
            {
                return false;
                
            }

        }
    }
}
