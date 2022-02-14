using System;
using System.Drawing;
using System.Windows.Forms;
using HalconDotNet;


namespace VisionControls
{
	[Serializable]
    public class FunctionPlot
	{
		private Graphics gPanel, backBuffer;
		private Pen          pen, penCurve, penCursor;
		private SolidBrush   brushCS, brushFuncPanel;
		private Font         drawFont;
		private StringFormat format;
		private Bitmap       functionMap;
		private float panelWidth;
		private float panelHeight;
		private float margin;
		private float originX;
		private float originY;
		private PointF[]    points;
		private HFunction1D func;
		private int   axisAdaption;
		private float axisXLength;
		private float axisYLength;
		private float scaleX, scaleY;
		public const int AXIS_RANGE_FIXED       = 3;
		public const int AXIS_RANGE_INCREASING  = 4;
		public const int AXIS_RANGE_ADAPTING    = 5;
		int PreX, BorderRight, BorderTop;
        private string xName, yName;
        public string XName
        {
            get
            {
                return xName;
            }

            private set
            {
                xName = value;
            }
        }
        public string YName
        {
            get
            {
                return yName;
            }

            set
            {
                yName = value;
            }
        }
		public FunctionPlot(Control panel, bool useMouseHandle)
		{
			gPanel = panel.CreateGraphics();

			panelWidth = panel.Size.Width - 32;
			panelHeight = panel.Size.Height - 22;

			originX = 32;
			originY = panel.Size.Height - 22;
			margin = 5.0f;

			BorderRight = (int)(panelWidth + originX - margin);
			BorderTop = (int)panelHeight;

			PreX = 0;
			scaleX = scaleY = 0.0f;


        
			axisAdaption = AXIS_RANGE_ADAPTING;
			axisXLength = 10.0f;
			axisYLength = 10.0f;

			pen = new Pen(System.Drawing.Color.DarkGray, 1);
			penCurve = new Pen(System.Drawing.Color.Blue, 1);
			penCursor = new Pen(System.Drawing.Color.LightSteelBlue, 1);
			penCursor.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

			brushCS = new SolidBrush(Color.Black);
			brushFuncPanel = new SolidBrush(Color.White);
			drawFont = new Font("Arial", 6);
			format = new StringFormat();
			format.Alignment = StringAlignment.Far;

			functionMap = new Bitmap(panel.Size.Width, panel.Size.Height);
			backBuffer = Graphics.FromImage(functionMap);
			resetPlot();

			panel.Paint += new System.Windows.Forms.PaintEventHandler(this.paint);

			if (useMouseHandle)
				panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseMoved);
		}

		public FunctionPlot(Control panel)
			: this(panel, false)
		{

		}

		public void setOrigin(int x, int y)
		{
			float tmpX;

			if (x < 1 || y < 1)
				return;

			tmpX = originX;
			originX = x;
			originY = y;

			panelWidth = panelWidth + tmpX - originX;
			panelHeight = originY;
			BorderRight = (int)(panelWidth + originX - margin);
			BorderTop = (int)panelHeight;
		}
 
		public void setAxisAdaption(int mode, float val)
		{
			switch (mode)
			{
				case AXIS_RANGE_FIXED:
					axisAdaption = mode;
					axisYLength = (val > 0) ? val : 255.0f;
					break;
				default:
					axisAdaption = mode;
					break;
			}
		}

		public void setAxisAdaption(int mode)
		{
			setAxisAdaption(mode, -1.0f);
		}

		public void plotFunction(double[] grayValues)
		{
			drawFunction(new HTuple(grayValues));
		}

		public void plotFunction(float[] grayValues)
		{
			drawFunction(new HTuple(grayValues));
		}


        
		public void plotFunction(int[] grayValues)
		{
			drawFunction(new HTuple(grayValues));
		}


        
		public void drawFunction(HTuple tuple)
		{
			HTuple val;
			int maxY,maxX;
			float stepOffset;

			if (tuple.Length == 0)
			{
				resetPlot();
				return;
			}

			val = tuple.TupleSortIndex();
			maxX = tuple.Length - 1;
			maxY = (int)tuple[val[val.Length - 1].I].D;

			axisXLength = maxX;

			switch (axisAdaption)
			{
				case AXIS_RANGE_ADAPTING:
					axisYLength = maxY;
					break;
				case AXIS_RANGE_INCREASING:
					axisYLength = (maxY > axisYLength) ? maxY : axisYLength;
					break;
			}

			backBuffer.Clear(System.Drawing.Color.WhiteSmoke);
			backBuffer.FillRectangle(brushFuncPanel, originX, 0, panelWidth, panelHeight);

			stepOffset = drawXYLabels();
			drawLineCurve(tuple, stepOffset);
			backBuffer.Flush();

			gPanel.DrawImageUnscaled(functionMap, 0, 0);
			gPanel.Flush();
		}


       
		public void resetPlot()
		{
			backBuffer.Clear(System.Drawing.Color.WhiteSmoke);
			backBuffer.FillRectangle(brushFuncPanel, originX, 0, panelWidth, panelHeight);
			func = null;
			drawXYLabels();
			backBuffer.Flush();
			repaint();
		}


	
		private void repaint()
		{
			gPanel.DrawImageUnscaled(functionMap, 0, 0);
			gPanel.Flush();
		}


		
		private void drawLineCurve(HTuple tuple, float stepOffset)
		{
			int length;

			if (stepOffset > 1)
				points = scaleDispValue(tuple);
			else
				points = scaleDispBlockValue(tuple);

			length = points.Length;

			func = new HFunction1D(tuple);

			for (int i = 0; i < length - 1; i++)
				backBuffer.DrawLine(penCurve, points[i], points[i + 1]);

		}


	
		private PointF[] scaleDispValue(HTuple tup)
		{
			PointF [] pVals;
			float  xMax, yMax, yV, x, y;
			int length;

			xMax = axisXLength;
			yMax = axisYLength;

			scaleX = (xMax != 0.0f) ? ((panelWidth - margin) / xMax) : 0.0f;
			scaleY = (yMax != 0.0f) ? ((panelHeight - margin) / yMax) : 0.0f;

			length = tup.Length;
			pVals = new PointF[length];

			for (int j=0; j < length; j++)
			{
				yV = (float)tup[j].D;
				x = originX + (float)j * scaleX;
				y = panelHeight - (yV * scaleY);
				pVals[j] = new PointF(x, y);
			}

			return pVals;
		}



		private PointF[] scaleDispBlockValue(HTuple tup)
		{
			PointF [] pVals;
			float  xMax, yMax, yV,x,y;
			int length, idx;

			xMax = axisXLength;
			yMax = axisYLength;

			scaleX = (xMax != 0.0f) ? ((panelWidth - margin) / xMax) : 0.0f;
			scaleY = (yMax != 0.0f) ? ((panelHeight - margin) / yMax) : 0.0f;

			length = tup.Length;
			pVals = new PointF[length * 2];

			y = 0;
			idx = 0;

			for (int j=0; j < length; j++)
			{
				yV = (float)tup[j].D;
				x = originX + (float)j * scaleX - (scaleX / 2.0f);
				y = panelHeight - (yV * scaleY);
				pVals[idx] = new PointF(x, y);
				idx++;

				x = originX + (float)j * scaleX + (scaleX / 2.0f);
				pVals[idx] = new PointF(x, y);
				idx++;
			}

           

			idx--;
			x = originX + (float)(length - 1) * scaleX;
			pVals[idx] = new PointF(x, y);

			idx = 0;
			yV = (float)tup[idx].D;
			x = originX;
			y = panelHeight - (yV * scaleY);
			pVals[idx] = new PointF(x, y);

			return pVals;
		}


   
		private float drawXYLabels()
		{
			float pX,pY,length, XStart,YStart;
			float YCoord, XCoord, XEnd, YEnd, offset, offsetString, offsetStep;
			float scale = 0.0f;

			offsetString = 5;
			XStart = originX;
			YStart = originY;

 

			pX = axisXLength;
			if (pX != 0.0)
				scale = (panelWidth - margin) / pX;

			if (scale > 10.0)
				offset = 1.0f;
			else if (scale > 2)
				offset = 10.0f;
			else if (scale > 0.2)
				offset = 100.0f;
			else
				offset = 1000.0f;



			XCoord = 0.0f;
			YCoord = YStart;
			XEnd = (scale * pX);

			backBuffer.DrawLine(pen, XStart, YStart, XStart + panelWidth - margin, YStart);
			backBuffer.DrawLine(pen, XStart + XCoord, YCoord, XStart + XCoord, YCoord + 6);
			backBuffer.DrawString(0 + "", drawFont, brushCS, XStart + XCoord + 4, YCoord + 8, format);
			backBuffer.DrawLine(pen, XStart + XEnd, YCoord, XStart + XEnd, YCoord + 6);
			backBuffer.DrawString(((int)pX + ""), drawFont, brushCS, XStart + XEnd + 4, YCoord + 8, format);

			length = (int)(pX / offset);
			length = (offset == 10) ? length - 1 : length;
			for (int i=1; i <= length; i++)
			{
				XCoord = (float)(offset * i * scale);

				if ((XEnd - XCoord) < 20)
					continue;

				backBuffer.DrawLine(pen, XStart + XCoord, YCoord, XStart + XCoord, YCoord + 6);
				backBuffer.DrawString(((int)(i * offset) + ""), drawFont, brushCS, XStart + XCoord + 5, YCoord + 8, format);
			}

			offsetStep = offset;

           
			pY = axisYLength;
			if (pY != 0.0)
				scale = ((panelHeight - margin) / pY);

			if (scale > 10.0)
				offset = 1.0f;
			else if (scale > 2)
				offset = 10.0f;
			else if (scale > 0.8)
				offset = 50.0f;
			else if (scale > 0.12)
				offset = 100.0f;
			else
				offset = 1000.0f;

          
			XCoord = XStart;
			YCoord = 5.0f;
			YEnd = YStart - (scale * pY);

			backBuffer.DrawLine(pen, XStart, YStart, XStart, YStart - (panelHeight - margin));
			backBuffer.DrawLine(pen, XCoord, YStart, XCoord - 10, YStart);
			backBuffer.DrawString(0 + "", drawFont, brushCS, XCoord - 12, YStart - offsetString, format);
			backBuffer.DrawLine(pen, XCoord, YEnd, XCoord - 10, YEnd);
			backBuffer.DrawString(pY + "", drawFont, brushCS, XCoord - 12, YEnd - offsetString, format);

			length = (int)(pY / offset);
			length = (offset == 10) ? length - 1 : length;
			for (int i=1; i <= length; i++)
			{
				YCoord = (YStart - ((float)offset * i * scale));

				if ((YCoord - YEnd) < 10)
					continue;

				backBuffer.DrawLine(pen, XCoord, YCoord, XCoord - 10, YCoord);
				backBuffer.DrawString(((int)(i * offset) + ""), drawFont, brushCS, XCoord - 12, YCoord - offsetString, format);
			}

			return offsetStep;
		}


        public int Xc; 
        public float Yc;
		private void mouseMoved(object sender, MouseEventArgs e)
		{
			int Xh;
			HTuple Ytup;
			float Yh;

			Xh = e.X;

			if (PreX == Xh || Xh < originX || Xh > BorderRight || func == null)
				return;

			PreX = Xh;

			Xc = (int)Math.Round((Xh - originX) / scaleX);
			Ytup = func.GetYValueFunct1d(new HTuple(Xc), "zero");

			Yc = (float)Ytup[0].D;
			Yh = panelHeight - (Yc * scaleY);

			gPanel.DrawImageUnscaled(functionMap, 0, 0);
			gPanel.DrawLine(penCursor, Xh, 0, Xh, BorderTop);
			gPanel.DrawLine(penCursor, originX, Yh, BorderRight + margin, Yh);
			gPanel.DrawString(("X = " + Xc), drawFont, brushCS, panelWidth - margin, 10);
			gPanel.DrawString(("Y = " + (int)Yc), drawFont, brushCS, panelWidth - margin, 20);
			gPanel.Flush();
		}

		private void paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			repaint();
		}
        public void SetAxisAdaption(int mode, float val)
        {
            switch (mode)
            {
                case AXIS_RANGE_FIXED:
                    axisAdaption = mode;
                    axisYLength = (val > 0) ? val : 255.0f;
                    break;
                default:
                    axisAdaption = mode;
                    break;
            }
        }
        public void SetAxisAdaption(int mode)
        {
            SetAxisAdaption(mode, -1.0f);
        }
        public void SetLabel(string x, string y)
        {
            XName = x;
            YName = y;
        }
      
        public void ResetPlot()
        {
            backBuffer.Clear(System.Drawing.Color.WhiteSmoke);
            backBuffer.FillRectangle(brushFuncPanel, originX, 0, panelWidth, panelHeight);
            func = null;
            drawXYLabels();
            backBuffer.Flush();
            repaint();
        }

	}
}
