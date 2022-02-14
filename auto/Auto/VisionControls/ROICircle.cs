using System;
using HalconDotNet;

namespace VisionControls
{
	[Serializable]
    public class ROICircle : ROI
	{
        public double radius;
		private double row1, col1;  // first handle
		public double midR, midC;  // second handle     
		public ROICircle()
		{
            NumHandles = 2; 
			activeHandleIdx = 1;
            midR = 200;
            midC = 200;
            radius = 50;
            row1 = midR;
            col1 = midC + radius;
		}
		public override void createROI(double midX, double midY)
		{
      
			midR = midY;
			midC = midX;

			radius = 100;

			row1 = midR;
			col1 = midC + radius;
		}
		public override void draw(HalconDotNet.HWindow window)
		{
			window.DispCircle(midR, midC, radius);
			window.SetColor("orange");
			window.DispRectangle2(row1, col1, 0, 5, 5);
			window.DispRectangle2(midR, midC, 0, 5, 5);
		}
		public override double distToClosestHandle(double x, double y)
		{
			double max = 10000;
			double [] val = new double[NumHandles];

			val[0] = HMisc.DistancePp(y, x, row1, col1); // border handle 
			val[1] = HMisc.DistancePp(y, x, midR, midC); // midpoint 

			for (int i=0; i < NumHandles; i++)
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
					window.DispRectangle2(row1, col1, 0, 5, 5);
					break;
				case 1:
					window.DispRectangle2(midR, midC, 0, 5, 5);
					break;
			}
		}
		public override HRegion getRegion()
		{
			HRegion region = new HRegion();
			region.GenCircle(midR, midC, radius);
			return region;
		}

		public override double getDistanceFromStartPoint(double row, double col)
		{
            double sRow = midR; //   假设：我们的角度从0点开始。
			double sCol = midC + 1 * radius;

			double angle = HMisc.AngleLl(midR, midC, sRow, sCol, midR, midC, row, col);

			if (angle < 0)
				angle += 2 * Math.PI;

			return (radius * angle);
		}
		public override HTuple getModelData()
		{
			return new HTuple(new double[] { midR, midC, radius });
		} 
        
		public override void moveByHandle(double newX, double newY)
		{
			HTuple distance;
			double shiftX,shiftY;

			switch (activeHandleIdx)
			{
				case 0: 

					row1 = newY;
					col1 = newX;
					HOperatorSet.DistancePp(new HTuple(row1), new HTuple(col1),
											new HTuple(midR), new HTuple(midC),
											out distance);

					radius = distance[0].D;
					break;
				case 1: // midpoint 

					shiftY = midR - newY;
					shiftX = midC - newX;

					midR = newY;
					midC = newX;

					row1 -= shiftY;
					col1 -= shiftX;
					break;
			}
            base.ROIchange_event();
		}

       
	}
}
