using System;
using HalconDotNet;

namespace VisionControls
{
     [Serializable]
    public class ROICross: ROI
    {

        
        public double midR = 100;
        public double midC = 100;   // 

        private HObject arrowHandleXLD;

        public ROICross()
        {
            NumHandles = 1;        // 
            activeHandleIdx = 2;
            arrowHandleXLD = new HObject();
            arrowHandleXLD.GenEmptyObj();
        }      
        public override void createROI(double midX, double midY)
        {
            midR = midY;
            midC = midX;
            updateArrowHandle();
        }  
        public override void draw(HalconDotNet.HWindow window)
        {

            window.DispObj(arrowHandleXLD);  // 
            window.DispRectangle2(midR, midC, 0, 5, 5);
        }    
        public override double distToClosestHandle(double x, double y)
        {

            double max = 10000;
            double[] val = new double[NumHandles];


            val[0] = HMisc.DistancePp(y, x, midR, midC); // 

            for (int i = 0; i < NumHandles; i++)
            {
                if (val[i] < max)
                {
                    max = val[i];
                    activeHandleIdx = i;
                }
            }// end of for 

            return val[activeHandleIdx];
        }
        public override void displayActive(HalconDotNet.HWindow window)
        {

            switch (activeHandleIdx)
            {
               
                case 0:
                    window.DispObj(arrowHandleXLD); // 
                    break;
                case 1:
                    window.DispRectangle2(midR, midC, 0, 5, 5);
                    break;
            }
        } 
        public override HRegion getRegion()
        {
            
            return null;
        }

        public override double getDistanceFromStartPoint(double row, double col)
        {
            double distance = HMisc.DistancePp(row, col, midR, midC);
            return distance;
        }
        public override HTuple getModelData()
        {
            return new HTuple(new double[] { midR, midC });
        }
        public override void moveByHandle(double newX, double newY)
        {
            midR = newY;
            midC = newX;

            updateArrowHandle();

            base.ROIchange_event();
        }
        private void updateArrowHandle()
        {
            arrowHandleXLD.Dispose();
            arrowHandleXLD.GenEmptyObj();
            HOperatorSet.GenCrossContourXld(out arrowHandleXLD, midR, midC, 50, 0);
        }

    }
}