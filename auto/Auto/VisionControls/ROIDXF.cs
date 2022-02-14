using System;
using HalconDotNet;
using System.Runtime.Serialization;
using System.IO;
using System.Windows.Forms;
namespace VisionControls
{
	[Serializable]
    public class ROIDXF : ROI
    {

        private double row1, col1;   // DXF缩放位置
        private double midR, midC;   // DXF中心位置
        private double rowsInit, colsInit;      //dxf的旋转箭头

        private double old_row1, old_col1;   // DXF可拖动的位置
        private double old_midR, old_midC;   // DXF中心位置
        private double old_rowsInit, old_colsInit;      //dxf的旋转箭头

        private HXLDCont ContursDXF;
        double phi = 0;
        HHomMat2D hom2D ;
        public ROIDXF(HXLDCont ContursDXF)
        {
            NumHandles = 3; //  
            activeHandleIdx = 1;
            this.ContursDXF = ContursDXF;
        }
        public override void createROI(double midX, double midY)
        {
            HHomMat2D homat=new HHomMat2D();
            HTuple Row1, column1, Row2, column2;
            HTuple area, row, column, pointOrder;
            HOperatorSet.AreaCenterXld(ContursDXF, out area, out row, out column, out pointOrder);
            if (row.Length > 1)//有多个XLD
            {
                row = row.TupleMean();
                column = column.TupleMean();
            }
            //先移动到中间位置
            homat.VectorAngleToRigid(row.D, column.D, 0, midX, midY, 0);
            ContursDXF= ContursDXF.AffineTransContourXld(homat);
            HObject region, region_union;
            HOperatorSet.GenRegionContourXld(ContursDXF, out region, "margin");
            HOperatorSet.Union1(region, out region_union);
            //求最小外接矩形
            HOperatorSet.SmallestRectangle1(region_union, out Row1, out column1, out Row2, out column2);
            //求面积找基准中心
            //中心点
            midR = (Row1.D + Row2.D) / 2;
            midC = (column1.D + column2.D) / 2;

            row1 = (Row1.D + Row2.D) / 2;
            col1 = column2;

            old_col1 = col1;
            old_midC = midC;
            old_row1 = Row1;
            old_midR = midR;
            rowsInit = Row1.D - 10;
            colsInit = (column1.D + column2.D) / 2;
            hom2D = new  HHomMat2D();
            
            updateArrowHandle();
        }


        public override void draw(HalconDotNet.HWindow window)
        {
            window.DispObj(ContursDXF);  //           
            window.SetColor("orange");
            window.DispRectangle2(row1, col1, phi, 5, 5);
            window.DispRectangle2(midR, midC, phi, 5, 5);
            window.DispRectangle2((midR + rowsInit) / 2, (midC + colsInit) / 2, phi, 5, 5);
            //画箭头
            window.DispArrow(midR, midC, rowsInit, colsInit, 2);
        }


        public override double distToClosestHandle(double x, double y)
        {

            double max = 10000;
            double[] val = new double[NumHandles];
            val[0] = HMisc.DistancePp(y, x, midR, midC); // 
            val[1] = HMisc.DistancePp(y, x, row1, col1); // 
            val[2] = HMisc.DistancePp(y, x, (midR + rowsInit) / 2, (midC + colsInit) / 2); // 

            for (int i = 0; i < NumHandles; i++)
            {
                if (val[i] < max)
                {
                    max = val[i];
                    activeHandleIdx = i;
                }
            }

            return val[activeHandleIdx];
        }
        public override void displayActive(HalconDotNet.HWindow window)
        {

            switch (activeHandleIdx)
            {
                case 1:
                    window.DispRectangle2(row1, col1, phi, 5, 5);
                    break;
                case 3:
                    window.DispObj(ContursDXF); //window.DispRectangle2(row2, col2, 0, 5, 5);
                    break;
                case 0:
                    window.DispRectangle2(midR, midC, phi, 5, 5);
                    break;
                case 2:
                    window.DispRectangle2((midR + rowsInit) / 2, (midC + colsInit) / 2, phi, 5, 5);
                    break;
            }
        }
        public override HXLDCont getXLD()
        {
            if (ContursDXF != null || ContursDXF.IsInitialized())
            {
                return ContursDXF.CopyObj(1, 1);
            }
           else
            {
                return null;
            }
        }
        public override HTuple getModelData()
        {
            return new HTuple(new double[] { row1, col1 });
        }
        public override void moveByHandle(double newX, double newY)
        {
            switch (activeHandleIdx)
            {
                case 0: //中间的小矩形
                    old_midR = midR;
                    old_midC = midC;
                    midR = newY;
                    midC = newX;
                    old_rowsInit = rowsInit;
                    old_colsInit = colsInit;
                    rowsInit = rowsInit + midR - old_midR;
                    colsInit = colsInit + midC - old_midC;
                    break;
                case 1: // 缩放的小矩形
                    if (col1 == midC)
                    {
                        col1 = midC + 1;
                        return;
                    }
                    old_row1 = row1;
                    old_col1 = col1;
                    row1 = newY;
                    col1 = newX;
                    break;
                case 2:
                    //求角度
                    HTuple _phi;
                    HOperatorSet.AngleLl(midR, midC, rowsInit, colsInit, midR, midC, newY, newX, out _phi);
                    phi = _phi.D;
                    // double vY = newY - colsInit;
                    //double vX = newX - rowsInit;
                    // phi = Math.Atan2(vY, vX);                
                    break;

            }
            updateArrowHandle();
        }
        private void updateArrowHandle()
        {
            HHomMat2D homMat2DIdentity=new HHomMat2D();
            switch (activeHandleIdx)
            {
                case 0://拖动
                    homMat2DIdentity.HomMat2dIdentity();
                    HHomMat2D  homMat2Dtranslate =homMat2DIdentity.HomMat2dTranslate(midR - old_midR, midC - old_midC);
                    ContursDXF = ContursDXF.AffineTransContourXld(homMat2Dtranslate);
                    HTuple row = 0, colum = 0;
                    HOperatorSet.AffineTransPoint2d(homMat2Dtranslate, row1, col1, out row, out colum);
                    row1 = row.D;
                    col1 = colum.D;
                    break;
                case 1: //缩放  
                    //上一次的lenth
                    double old_lenth = Math.Abs(old_col1 - old_midC);
                    double new_lenth = Math.Abs(col1 - midC);
                    //比例
                    double bili = new_lenth / old_lenth;
                    if (bili <= 0.05)
                        return;
                    old_col1 = col1;
                    old_midC = midC;

                    homMat2DIdentity.HomMat2dIdentity();
                    HHomMat2D  homMat2DScale= homMat2DIdentity.HomMat2dScale(bili, bili, midR, midC);
                    ContursDXF= homMat2DScale.AffineTransContourXld(ContursDXF);
                    
                    //求最小外接矩形
                    HObject region, region_union;
                    HOperatorSet.GenRegionContourXld(ContursDXF, out region, "margin");
                    HOperatorSet.Union1(region, out region_union);
                    HTuple Row1, column1, Row2, column2;
                    HOperatorSet.SmallestRectangle1(region_union, out Row1, out column1, out Row2, out column2);


                    row1 = (Row1.D + Row2.D) / 2;
                    col1 = column2;
                  //  rowsInit = Row1.D - 10;
                  //  colsInit = (column1.D + column2.D) / 2;
                    break;
                case 2: //旋转
                    hom2D.HomMat2dIdentity();
                    HHomMat2D HomatRotate = new HHomMat2D();
                    HomatRotate = hom2D.HomMat2dRotate(phi, midR, midC);
                    HTuple _rowsInit, _colsInit;
                    HOperatorSet.AffineTransPoint2d(HomatRotate, (HTuple)rowsInit, (HTuple)colsInit, out _rowsInit, out _colsInit);
                    rowsInit = _rowsInit.D;
                    colsInit = _colsInit.D;
                    HOperatorSet.AffineTransPoint2d(HomatRotate, (HTuple)row1, (HTuple)col1, out _rowsInit, out _colsInit);
                   // row1 = _rowsInit.D;
                   // col1 = _colsInit.D;
                    ContursDXF = ContursDXF.AffineTransContourXld(HomatRotate);
                    break;
            }
        }
    }
}
