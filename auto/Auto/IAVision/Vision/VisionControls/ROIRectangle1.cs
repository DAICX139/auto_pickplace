using System;
using HalconDotNet;


namespace VisionControls
{ 
	[Serializable]
    public class ROIRectangle1 : ROI
	{
		public double row1, col1;   // 
        public double row2, col2;   //  
        public double midR, midC;   // 
        public double m_x, m_y;		 
		public ROIRectangle1()
		{          
            row1 = 35;
            col1 = 35;
            row2 = 330;
            col2 = 480;
            midR = 170;
            midC = 250;
            NumHandles = 5; 
			activeHandleIdx = 4;
		}      
		public override void createROI(double midX, double midY)
		{
			midR = midY;
			midC = midX;

			row1 = midR - 50;
			col1 = midC - 50;
			row2 = midR + 50;
			col2 = midC + 50;
		}        
		public override void draw(HalconDotNet.HWindow window)
		{
			window.DispRectangle1(row1, col1, row2, col2);
		 	window.SetColor("orange");
			window.DispRectangle2(row1, col1, 0, 5, 5);
			window.DispRectangle2(row1, col2, 0, 5, 5);
			window.DispRectangle2(row2, col2, 0, 5, 5);
			window.DispRectangle2(row2, col1, 0, 5, 5);
			window.DispRectangle2(midR, midC, 0, 5, 5);
		}       
		public override double distToClosestHandle(double x, double y)
		{
			double max = 10000;
			double [] val = new double[NumHandles];

			midR = ((row2 - row1) / 2) + row1;
			midC = ((col2 - col1) / 2) + col1;

			val[0] = HMisc.DistancePp(y, x, row1, col1); //  
			val[1] = HMisc.DistancePp(y, x, row1, col2); // 
			val[2] = HMisc.DistancePp(y, x, row2, col2); //  
			val[3] = HMisc.DistancePp(y, x, row2, col1); //  
			val[4] = HMisc.DistancePp(y, x, midR, midC); // 
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
					window.DispRectangle2(row1, col2, 0, 5, 5);
					break;
				case 2:
					window.DispRectangle2(row2, col2, 0, 5, 5);
					break;
				case 3:
					window.DispRectangle2(row2, col1, 0, 5, 5);
					break;
				case 4:
					window.DispRectangle2(midR, midC, 0, 5, 5);
                    m_x = midR;
                    m_y = midC;
					break;
			}
		}        
		public override HRegion getRegion()
		{
			HRegion region = new HRegion();
			region.GenRectangle1(row1, col1, row2, col2);
			return region;
		}      
		public override HTuple getModelData()
		{
			return new HTuple(new double[] { row1, col1, row2, col2 });
		}
		public override void moveByHandle(double newX, double newY)
		{
			double len1, len2;
			double tmp;
			switch (activeHandleIdx)
			{
				case 0: //  
					row1 = newY;
					col1 = newX;
					break;
				case 1: //  
					row1 = newY;
					col2 = newX;
					break;
				case 2: //  
					row2 = newY;
					col2 = newX;
					break;
				case 3: // 
					row2 = newY;
					col1 = newX;
					break;
				case 4: // 
					len1 = ((row2 - row1) / 2);
					len2 = ((col2 - col1) / 2);

					row1 = newY - len1;
					row2 = newY + len1;

					col1 = newX - len2;
					col2 = newX + len2;
					break;
			}
			if (row2 <= row1)
			{
				tmp = row1;
				row1 = row2;
				row2 = tmp;
			}

			if (col2 <= col1)
			{
				tmp = col1;
				col1 = col2;
				col2 = tmp;
			}
			midR = ((row2 - row1) / 2) + row1;
			midC = ((col2 - col1) / 2) + col1;
            base.ROIchange_event();
		}
	}
}
