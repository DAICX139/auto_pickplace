using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
namespace VisionControls
{
    [Serializable]
    public class ROIUserDefine : ROI
    {
       public HRegion Ho_userDefine;
        public ROIUserDefine()
        {

            Ho_userDefine = new  HRegion();
        }
        public override void createROI(double midX, double midY)
        {
           
        }
        public override void draw(HalconDotNet.HWindow window)
        {
            window.DispObj(Ho_userDefine);
        }
        int isInside = 0;
        public override double distToClosestHandle(double x, double y)
        {
            HTuple isInside;
            HOperatorSet.TestRegionPoint(Ho_userDefine, y, x, out isInside);
            if (isInside.I == 1)
            {
                isInside = 1;
                return 1;
            }
            else
            {
                isInside = 0;
            }

            return 1000000;
        }
        public override void displayActive(HalconDotNet.HWindow window)
        {
            switch (isInside)
            {
                case 1:
                    window.DispObj(Ho_userDefine);
                    break;
                default:
                    // window.DispRectangle2(row1, col2, 0, 5, 5);
                    break;

            }
        }
        public override HRegion getRegion()
        {
            return Ho_userDefine;
        }
        public override HTuple getModelData()
        {
            if (!Ho_userDefine.IsInitialized())
                return null;
            HTuple  row1,  column1,  row2,  column2;
            //返回最小外接矩形
            HOperatorSet.SmallestRectangle1(Ho_userDefine, out row1, out column1, out row2, out column2);
            return new HTuple(new double[] { row1, column1, row2, column2 });
        }
        public override void moveByHandle(double newX, double newY)
        {
            base.ROIchange_event();

        }
    }
}
