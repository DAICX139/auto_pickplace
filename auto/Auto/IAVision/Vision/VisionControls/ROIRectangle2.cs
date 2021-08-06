using System;
using HalconDotNet;



namespace VisionControls
{
	[Serializable]
    public class ROIRectangle2 : ROI
	{   
		public double length1;  
        public double length2;    
        public double midR;     
        public double midC;

        
        public double phi;


        
		public  HTuple rowsInit;
		public HTuple colsInit;
        public HTuple rows;
		public HTuple cols;

		HHomMat2D hom2D, tmp;	 
		public ROIRectangle2()
		{
            length1 = 100;
            length2 = 50;

            phi = 0.0;

            rowsInit = new HTuple(new double[] {-1.0, -1.0, 1.0, 
												   1.0,  0.0, 0.0 });
            colsInit = new HTuple(new double[] {-1.0, 1.0,  1.0, 
												  -1.0, 0.0, 0.6 });
            //order        ul ,  ur,   lr,  ll,   mp, 箭头中点
            hom2D = new HHomMat2D();
            tmp = new HHomMat2D();
            midR = 170;
            midC = 250;
			NumHandles = 6; // 4 角点 +  1 中点 + 1 旋转点			
			activeHandleIdx = 4;
            updateHandlePos();
		}
		public override void createROI(double midX, double midY)
		{
			midR = midY;
			midC = midX;

			length1 = 100;
			length2 = 50;

			phi = 0.0;

			rowsInit = new HTuple(new double[] {-1.0, -1.0, 1.0, 
												   1.0,  0.0, 0.0 });
			colsInit = new HTuple(new double[] {-1.0, 1.0,  1.0, 
												  -1.0, 0.0, 0.6 });
            //order        ul ,  ur,   lr,  ll,   mp, 箭头中点
			hom2D = new HHomMat2D();
			tmp = new HHomMat2D();

			updateHandlePos();
		}  
		public override void draw(HalconDotNet.HWindow window)
		{
			window.DispRectangle2(midR, midC, -phi, length1, length2);
			window.SetColor("orange");
			for (int i =0; i < NumHandles; i++)
				window.DispRectangle2(rows[i].D, cols[i].D, -phi, 5, 5);

			window.DispArrow(midR, midC, midR + (Math.Sin(phi) * length1 * 1.2),
				midC + (Math.Cos(phi) * length1 * 1.2), 2.0);

		}   
		public override double distToClosestHandle(double x, double y)
		{
			double max = 10000;
			double [] val = new double[NumHandles];


			for (int i=0; i < NumHandles; i++)
				val[i] = HMisc.DistancePp(y, x, rows[i].D, cols[i].D);

			for (int i=0; i < NumHandles; i++)
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
			window.DispRectangle2(rows[activeHandleIdx].D,
								  cols[activeHandleIdx].D,
								  -phi, 5, 5);

			if (activeHandleIdx == 5)
				window.DispArrow(midR, midC,
								 midR + (Math.Sin(phi) * length1 * 1.2),
								 midC + (Math.Cos(phi) * length1 * 1.2),
								 2.0);
		}
 
		public override HRegion getRegion()
		{
			HRegion region = new HRegion();
			region.GenRectangle2(midR, midC, -phi, length1, length2);
			return region;
		}

        
		public override HTuple getModelData()
		{
			return new HTuple(new double[] { midR, midC, phi, length1, length2 });
		}

        
		public override void moveByHandle(double newX, double newY)
		{
			double vX, vY, x=0, y=0;

			switch (activeHandleIdx)
			{
				case 0:
				case 1:
				case 2:
				case 3:
					tmp = hom2D.HomMat2dInvert();
					x = tmp.AffineTransPoint2d(newX, newY, out y);

					length2 = Math.Abs(y);
					length1 = Math.Abs(x);

					checkForRange(x, y);
					break;
				case 4:
					midC = newX;
					midR = newY;
					break;
				case 5:
					vY = newY - rows[4].D;
					vX = newX - cols[4].D;
					phi = Math.Atan2(vY, vX);
					break;
			}
			updateHandlePos();
            base.ROIchange_event();
		}


   
		private void updateHandlePos()
		{
			hom2D.HomMat2dIdentity();
			hom2D = hom2D.HomMat2dTranslate(midC, midR);
			hom2D = hom2D.HomMat2dRotateLocal(phi);
			tmp = hom2D.HomMat2dScaleLocal(length1, length2);
			cols = tmp.AffineTransPoint2d(colsInit, rowsInit, out rows);
		}



 
   
		private void checkForRange(double x, double y)
		{
			switch (activeHandleIdx)
			{
				case 0:
					if ((x < 0) && (y < 0))
						return;
					if (x >= 0) length1 = 0.01;
					if (y >= 0) length2 = 0.01;
					break;
				case 1:
					if ((x > 0) && (y < 0))
						return;
					if (x <= 0) length1 = 0.01;
					if (y >= 0) length2 = 0.01;
					break;
				case 2:
					if ((x > 0) && (y > 0))
						return;
					if (x <= 0) length1 = 0.01;
					if (y <= 0) length2 = 0.01;
					break;
				case 3:
					if ((x < 0) && (y > 0))
						return;
					if (x >= 0) length1 = 0.01;
					if (y <= 0) length2 = 0.01;
					break;
				default:
					break;
			}
		}
	}
}
