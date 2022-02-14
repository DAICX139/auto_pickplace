using System;
using System.Collections;
using HalconDotNet;



namespace VisionControls
{
	public delegate void GCDelegate(string val);
	[Serializable]
	public class GraphicsContext
	{				        
		public const string GC_COLOR	 = "Color";	
		public const string GC_COLORED	 = "Colored";		
		public const string GC_LINEWIDTH = "LineWidth";	
		public const string GC_DRAWMODE  = "DrawMode";	
		public const string GC_SHAPE     = "Shape";	
		public const string GC_LUT       = "Lut";	
		public const string GC_PAINT     = "Paint";
		public const string GC_LINESTYLE = "LineStyle";
		private Hashtable		graphicalSettings;	 	
		public	Hashtable		stateOfSettings;

		private IEnumerator iterator;	
		public GCDelegate   gcNotification;		 
		public GraphicsContext()
		{
			graphicalSettings = new Hashtable(10, 0.2f);
			gcNotification = new GCDelegate(dummy);
			stateOfSettings = new Hashtable(10, 0.2f);
		}

		public GraphicsContext(Hashtable settings)
		{
			graphicalSettings = settings;
			gcNotification = new GCDelegate(dummy);
			stateOfSettings = new Hashtable(10, 0.2f);
		}
		public void applyContext(HWindow window, Hashtable cContext)
		{
			string key  = "";
			string valS = "";
			int    valI = -1;
			HTuple valH = null;

			iterator = cContext.Keys.GetEnumerator();

			try
			{
				while (iterator.MoveNext())
				{

					key = (string)iterator.Current;

					if (stateOfSettings.Contains(key) &&
						stateOfSettings[key] == cContext[key])
						continue;

					switch (key)
					{
						case GC_COLOR:
							valS = (string)cContext[key];
							window.SetColor(valS);
							if (stateOfSettings.Contains(GC_COLORED))
								stateOfSettings.Remove(GC_COLORED);

							break;
						case GC_COLORED:
							valI = (int)cContext[key];
							window.SetColored(valI);

							if (stateOfSettings.Contains(GC_COLOR))
								stateOfSettings.Remove(GC_COLOR);

							break;
						case GC_DRAWMODE:
							valS = (string)cContext[key];
							window.SetDraw(valS);
							break;
						case GC_LINEWIDTH:
							valI = (int)cContext[key];
							window.SetLineWidth(valI);
							break;
						case GC_LUT:
							valS = (string)cContext[key];
							window.SetLut(valS);
							break;
						case GC_PAINT:
							valS = (string)cContext[key];
							window.SetPaint(valS);
							break;
						case GC_SHAPE:
							valS = (string)cContext[key];
							window.SetShape(valS);
							break;
						case GC_LINESTYLE:
							valH = (HTuple)cContext[key];
							window.SetLineStyle(valH);
							break;
						default:
							break;
					}


					if (valI != -1)
					{
						if (stateOfSettings.Contains(key))
							stateOfSettings[key] = valI;
						else
							stateOfSettings.Add(key, valI);

						valI = -1;
					}
					else if (valS != "")
					{
						if (stateOfSettings.Contains(key))
							stateOfSettings[key] = valI;
						else
							stateOfSettings.Add(key, valI);

						valS = "";
					}
					else if (valH != null)
					{
						if (stateOfSettings.Contains(key))
							stateOfSettings[key] = valI;
						else
							stateOfSettings.Add(key, valI);

						valH = null;
					}
				}//while
			}
			catch (HOperatorException e)
			{
				gcNotification(e.Message);
				return;
			}
		}
		public void setColorAttribute(string val)
		{
			if (graphicalSettings.ContainsKey(GC_COLORED))
				graphicalSettings.Remove(GC_COLORED);

			addValue(GC_COLOR, val);
		}
		public void setColoredAttribute(int val)
		{
			if (graphicalSettings.ContainsKey(GC_COLOR))
				graphicalSettings.Remove(GC_COLOR);

			addValue(GC_COLORED, val);
		}
		public void setDrawModeAttribute(string val)
		{
			addValue(GC_DRAWMODE, val);
		}
        public void setLineWidthAttribute(int val)
		{
			addValue(GC_LINEWIDTH, val);
		}
		public void setLutAttribute(string val)
		{
			addValue(GC_LUT, val);
		}
		public void setPaintAttribute(string val)
		{
			addValue(GC_PAINT, val);
		}
		public void setShapeAttribute(string val)
		{
			addValue(GC_SHAPE, val);
		}
		public void setLineStyleAttribute(HTuple val)
		{
			addValue(GC_LINESTYLE, val);
		}		 
		private void addValue(string key, int val)
		{
			if (graphicalSettings.ContainsKey(key))
				graphicalSettings[key] = val;
			else
				graphicalSettings.Add(key, val);
		}
		private void addValue(string key, string val)
		{
			if (graphicalSettings.ContainsKey(key))
				graphicalSettings[key] = val;
			else
				graphicalSettings.Add(key, val);
		}
		private void addValue(string key, HTuple val)
		{
			if (graphicalSettings.ContainsKey(key))
				graphicalSettings[key] = val;
			else
				graphicalSettings.Add(key, val);
		}	
		public void clear()
		{
			graphicalSettings.Clear();
		}	
		public GraphicsContext copy()
		{
			return new GraphicsContext((Hashtable)this.graphicalSettings.Clone());
		}
		public object getGraphicsAttribute(string key)
		{
			if (graphicalSettings.ContainsKey(key))
				return graphicalSettings[key];

			return null;
		}
		public Hashtable copyContextList()
		{
			return (Hashtable)graphicalSettings.Clone();
		}
		public void dummy(string val) { }

	}
}
