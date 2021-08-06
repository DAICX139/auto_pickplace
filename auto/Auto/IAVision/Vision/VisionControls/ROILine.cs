using System;
using HalconDotNet;
using System.Runtime.Serialization;
namespace VisionControls
{
	[Serializable]
    public class ROILine : ROI
	{
		private double row1, col1;   //  
		private double row2, col2;   // 
		private double midR, midC;   // 
		private HXLDCont arrowHandleXLD;

		public ROILine()
		{
            NumHandles = 3; //  
			activeHandleIdx = 2;
			arrowHandleXLD = new HXLDCont();
			arrowHandleXLD.GenEmptyObj();
		}
        protected ROILine(SerializationInfo info, StreamingContext context)  
                {  
                     
                } 
         
		 
		public override void createROI(double midX, double midY)
		{
			midR = midY;
			midC = midX;

			row1 = midR;
			col1 = midC - 50;
			row2 = midR;
			col2 = midC + 50;

			updateArrowHandle();
		} 
		public override void draw(HalconDotNet.HWindow window)
		{

			window.DispLine(row1, col1, row2, col2);
			window.SetColor("orange");
			window.DispRectangle2(row1, col1, 0, 5, 5);
			window.DispObj(arrowHandleXLD);  // 
			window.DispRectangle2(midR, midC, 0, 5, 5);
		}		 
		public override double distToClosestHandle(double x, double y)
		{

			double max = 10000;
			double [] val = new double[NumHandles];

			val[0] = HMisc.DistancePp(y, x, row1, col1); // 
			val[1] = HMisc.DistancePp(y, x, row2, col2); // 
			val[2] = HMisc.DistancePp(y, x, midR, midC); // 

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
					window.DispObj(arrowHandleXLD); //window.DispRectangle2(row2, col2, 0, 5, 5);
					break;
				case 2:
					window.DispRectangle2(midR, midC, 0, 5, 5);
					break;
			}
		}

 
		public override HRegion getRegion()
		{
			HRegion region = new HRegion();
			region.GenRegionLine(row1, col1, row2, col2);
			return region;
		}

		public override double getDistanceFromStartPoint(double row, double col)
		{
			double distance = HMisc.DistancePp(row, col, row1, col1);
			return distance;
		}
		public override HTuple getModelData()
		{
			return new HTuple(new double[] { row1, col1, row2, col2 });
		}
		public override void moveByHandle(double newX, double newY)
		{
			double lenR, lenC;

			switch (activeHandleIdx)
			{
				case 0: // first end point
					row1 = newY;
					col1 = newX;

					midR = (row1 + row2) / 2;
					midC = (col1 + col2) / 2;
					break;
				case 1: // last end point
					row2 = newY;
					col2 = newX;

					midR = (row1 + row2) / 2;
					midC = (col1 + col2) / 2;
					break;
				case 2: // midpoint 
					lenR = row1 - midR;
					lenC = col1 - midC;

					midR = newY;
					midC = newX;

					row1 = midR + lenR;
					col1 = midC + lenC;
					row2 = midR - lenR;
					col2 = midC - lenC;
					break;
			}
			updateArrowHandle();
            base.ROIchange_event();
		}
		private void updateArrowHandle()
		{
			double length,dr,dc, halfHW;
			double rrow1, ccol1,rowP1, colP1, rowP2, colP2;

			double headLength = 15;
			double headWidth  = 15;


			arrowHandleXLD.Dispose();
			arrowHandleXLD.GenEmptyObj();

			rrow1 = row1 + (row2 - row1) * 0.8;
			ccol1 = col1 + (col2 - col1) * 0.8;

			length = HMisc.DistancePp(rrow1, ccol1, row2, col2);
			if (length == 0)
				length = -1;

			dr = (row2 - rrow1) / length;
			dc = (col2 - ccol1) / length;

			halfHW = headWidth / 2.0;
			rowP1 = rrow1 + (length - headLength) * dr + halfHW * dc;
			rowP2 = rrow1 + (length - headLength) * dr - halfHW * dc;
			colP1 = ccol1 + (length - headLength) * dc - halfHW * dr;
			colP2 = ccol1 + (length - headLength) * dc + halfHW * dr;

			if (length == -1)
				arrowHandleXLD.GenContourPolygonXld(rrow1, ccol1);
			else
				arrowHandleXLD.GenContourPolygonXld(new HTuple(new double[] { rrow1, row2, rowP1, row2, rowP2, row2 }),
													new HTuple(new double[] { ccol1, col2, colP1, col2, colP2, col2 }));
		}

	}
}
