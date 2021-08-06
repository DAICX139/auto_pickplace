using System;
using HalconDotNet;
using System.Collections;
namespace VisionControls
{
    [Serializable]
	public class HObjectEntry
	{
		public Hashtable	gContext;

		public HObject		HObj;

		public HObjectEntry(HObject obj, Hashtable gc)
		{
			gContext = gc;
			HObj = obj;
		}     
		public void clear()
		{
			gContext.Clear();
			HObj.Dispose();
		}

	}
}
