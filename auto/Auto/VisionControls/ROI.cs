using System;
using HalconDotNet;
using System.Drawing;
namespace VisionControls
{   
	[Serializable]
    public class ROI
	{
        public static event EventHandler ROIchange;
		protected int   NumHandles;
		protected int	activeHandleIdx;
		protected int     OperatorFlag;  
		public HTuple     flagLineStyle;   
		public const int  POSITIVE_FLAG	= ROIController.MODE_ROI_POS;  
		public const int  NEGATIVE_FLAG	= ROIController.MODE_ROI_NEG;
		public const int  ROI_TYPE_LINE       = 10;
		public const int  ROI_TYPE_CIRCLE     = 11;
		public const int  ROI_TYPE_CIRCLEARC  = 12;
		public const int  ROI_TYPE_RECTANCLE1 = 13;
		public const int  ROI_TYPE_RECTANGLE2 = 14;
		protected HTuple  posOperation = new HTuple();
		protected HTuple  negOperation = new HTuple(new int[] { 2, 2 });   
		public ROI() { }
		public virtual void createROI(double midX, double midY) { }       
		public virtual void draw(HalconDotNet.HWindow window) { }
		public virtual double distToClosestHandle(double x, double y)
		{
			return 0.0;
		}
       
		public virtual void displayActive(HalconDotNet.HWindow window) { }
       
		public virtual void moveByHandle(double x, double y) 
        {
           
        }
		public virtual HRegion getRegion()
		{
            
			return null;
		}
        public virtual HXLDCont getXLD()
        {

            return null;
        }

		public virtual double getDistanceFromStartPoint(double row, double col)
		{
			return 0.0;
		}
		public virtual HTuple getModelData()
		{
			return null;
		}     
		public int getNumHandles()
		{
			return NumHandles;
		} 
		public int getActHandleIdx()
		{
			return activeHandleIdx;
		}
		public int getOperatorFlag()
		{
			return OperatorFlag;
		}

        public void ROIchange_event()
        {
            if (ROIchange!=null)
            ROIchange(null, null);
        }
		public void setOperatorFlag(int flag)
		{
			OperatorFlag = flag;

			switch (OperatorFlag)
			{
				case ROI.POSITIVE_FLAG:
					flagLineStyle = posOperation;
					break;
				case ROI.NEGATIVE_FLAG:
					flagLineStyle = negOperation;
					break;
				default:
					flagLineStyle = posOperation;
					break;
			}
		}
	}
}
