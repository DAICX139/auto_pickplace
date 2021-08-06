using System;
using HalconDotNet;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace VisionControls
{
 
    [Serializable]
    public class  ROIInfo
    {
        public ArrayList ROIList;
        public ArrayList RemarksList;
        public ROIInfo()
        {
            ROIList = new ArrayList();
            RemarksList = new ArrayList();
        }
        public void clear()
        {
            ROIList.Clear();
            RemarksList.Clear();
        }
        public void AddDefaulRoi1()
        {
            ROIRectangle1 DefaultROI = new ROIRectangle1();
            ROIList.Add(DefaultROI);
            RemarksList.Add(" ");
        }
        public void AddDefaulRoi2()
        {
            ROIRectangle2 DefaultROI = new ROIRectangle2();
            ROIList.Add(DefaultROI);
            RemarksList.Add(" ");
        }
        public void AddDefaulRoi3()
        {
            ROICircle DefaultROI = new ROICircle();
            ROIList.Add(DefaultROI);
            RemarksList.Add(" ");
        }
        public ROIInfo clone()
        {
            ROIInfo ROIInfo = new ROIInfo();

            //浅拷贝
            //ROIInfo.ROIList = (ArrayList)this.ROIList.Clone();
            ////ROIInfo.RemarksList = (ArrayList)this.RemarksList.Clone();

            //深拷贝
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, this);
            ms.Seek(0, SeekOrigin.Begin);
            ROIInfo = (ROIInfo)bf.Deserialize(ms);
            return ROIInfo;
        }
    }
    [Serializable]
	public class ROIController
	{

        //画笔粗细
        public int m_pen = 0;//画笔大于0就是画刷  小于0就是橡皮
		/// </summary>
		public const int MODE_ROI_POS           = 21;

        
		/// </summary>
		public const int MODE_ROI_NEG           = 22;

       
		/// </summary>
		public const int MODE_ROI_NONE          = 23;

      
		public const int EVENT_UPDATE_ROI       = 50;

		public const int EVENT_CHANGED_ROI_SIGN = 51;

        
		public const int EVENT_MOVING_ROI       = 52;

		public const int EVENT_DELETED_ACTROI  	= 53;

		public const int EVENT_DELETED_ALL_ROIS = 54;

		public const int EVENT_ACTIVATED_ROI   	= 55;

		public const int EVENT_CREATED_ROI   	  = 56;


		private ROI     roiMode;
        private string  ROIName;
		private int     stateROI;
		private double  currX, currY;

      
		public int      activeROIidx;
		public int      deletedIdx;


        public ROIInfo RoiInfo;
		//public ArrayList ROIList;
       // public ArrayList RemarksList;

        /// </summary>
        public HRegion ModelROI;

        public string activeCol = "#00ff0030";
        public string activeHdlCol = "#ff000030";
        public string inactiveCol = "#00ffff30";

      
		/// </summary>
		public HWndCtrl viewController;
 
		/// </summary>
		public IconicDelegate  NotifyRCObserver;

		/// <summary>构造函数</summary>
		public ROIController()
		{
			stateROI = MODE_ROI_NONE;
            RoiInfo = new ROIInfo();
            activeROIidx = -1;
			ModelROI = new HRegion();
			NotifyRCObserver = new IconicDelegate(dummyI);
			deletedIdx = -1;
			currX = currY = -1;
		}      
		public void setViewController(HWndCtrl view)
		{
			viewController = view;
		}     
		public HRegion getModelRegion()
		{
			return ModelROI;
		}
        public void addRec1(int row,int colum,string name=" ")
        {
            ROIName = name;
            ROIRectangle1 m_roi = new ROIRectangle1();
            setROIShape(m_roi);
            AddStaicROI(row, colum);
            viewController.repaint();
        }
        public void addRec2(int row, int colum, string name = " ")
        {
            ROIName = name;
            ROIRectangle2 m_roi = new ROIRectangle2();
            setROIShape(m_roi);
            AddStaicROI(row, colum);
            viewController.repaint();
        }
        public void addCircle(int row, int colum, string name = " ")
        {
            ROIName = name;
            ROICircle m_roi = new ROICircle();
            setROIShape(m_roi);
            AddStaicROI(row, colum);
            viewController.repaint();
        }
        public void addLine(int row, int colum, string name = " ")
        {
            ROIName = name;
            ROILine m_roi = new ROILine();
            setROIShape(m_roi);
            AddStaicROI(row, colum);
            viewController.repaint();
        }
        public void addDXF()
        {
            HTuple statues;
            HXLDCont ContursDXF=new HXLDCont();
            System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Title = "请选择文件";
            fileDialog.Filter = "dxf文件(*.dxf)|*.dxf";
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string file = fileDialog.FileName;
               statues= ContursDXF.ReadContourXldDxf(file, new HTuple(), new HTuple());
                
            }
            else
            {
                return;
            }
            ROIDXF DXF = new ROIDXF(ContursDXF);
            setROIShape(DXF);
            AddStaicROI(viewController.imageHeight / 2, viewController.imageWidth / 2);
            viewController.repaint();
        }
        public void addCross(int row, int colum, string name = "")
        {
            ROIName = name;
            ROICross m_roi = new ROICross();
            setROIShape(m_roi);
            AddStaicROI(row, colum);
            viewController.repaint();
        }
		public ArrayList getROIList()
		{
            BinaryFormatter Formatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            Formatter.Serialize(stream, RoiInfo.ROIList);
            stream.Position = 0;
            ArrayList flightList = Formatter.Deserialize(stream) as ArrayList;
            stream.Close();
            return flightList;
		}
        public void Start_Draw(int pen)
        {
            m_pen = pen;
            viewController.viewPort.MouseMove -= hWindowControl1_MouseMove;
            viewController.viewPort.MouseMove += hWindowControl1_MouseMove;
        }
        public void addUserdefine()
        {
            if (obj.IsInitialized())
            {
                viewController.viewPort.MouseMove -= hWindowControl1_MouseMove;
                ROIUserDefine m_roi = new ROIUserDefine();
                m_roi.Ho_userDefine = obj.CopyObj(1, 1);
                obj.Dispose();
                setROIShape(m_roi);
                AddStaicROI(100, 100);
                viewController.repaint();
            }
        }

        HTuple hv_Row_yanmo = new HTuple(), hv_Column_yanmo = new HTuple(), hv_button_yanmo = new HTuple();
        HRegion ho_Circle_yanmo = new HRegion();
        public HRegion obj = new HRegion();
        public void hWindowControl1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (m_pen == 0)
            {
                return;
            }
            try
            {
                viewController.repaint();
                HOperatorSet.GetMposition(viewController.viewPort.HalconWindow, out hv_Row_yanmo, out hv_Column_yanmo, out hv_button_yanmo);
                HOperatorSet.SetColor(viewController.viewPort.HalconWindow, "#ff000080");
                if (m_pen >0)//画笔大于0就是画刷
                {
                    HOperatorSet.DispCircle(viewController.viewPort.HalconWindow, hv_Row_yanmo, hv_Column_yanmo, 3 * m_pen);
                }              
                else  //小于0就是橡皮
                {
                    HOperatorSet.DispCircle(viewController.viewPort.HalconWindow, hv_Row_yanmo, hv_Column_yanmo, 3 * Math.Abs(m_pen));
                }
         
                HOperatorSet.SetLineStyle(viewController.viewPort.HalconWindow, (new HTuple(20)).TupleConcat(7));
                viewController.viewPort.HalconWindow.DispCross(hv_Row_yanmo, hv_Column_yanmo, viewController.imageWidth, 0);
            }
            catch
            {

            }
            if (sender == null)
                return;
            if (e.Button == System.Windows.Forms.MouseButtons.Left && hv_Row_yanmo.Length > 0)//左键
            {
                if (m_pen > 0)
                {
                    ho_Circle_yanmo.GenCircle(hv_Row_yanmo, hv_Column_yanmo, 3 * m_pen);
                }
                

                if (!obj.IsInitialized() || obj == null)
                {
                    obj.GenEmptyObj();
                    obj = obj.Union2(ho_Circle_yanmo);
                }
                else
                {
                    obj = obj.Union2(ho_Circle_yanmo);
                }


            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right && hv_Row_yanmo.Length > 0)//右键
            {
                ho_Circle_yanmo.GenCircle(hv_Row_yanmo, hv_Column_yanmo, 3 * Math.Abs(m_pen));
                HOperatorSet.DispCircle(viewController.viewPort.HalconWindow, hv_Row_yanmo, hv_Column_yanmo, 3 * Math.Abs(m_pen));

                if (!obj.IsInitialized() || obj == null)
                {
                    obj.GenEmptyObj();
                }
                else
                {
                    obj = obj.Difference(ho_Circle_yanmo);
                }
            }

            if (obj.IsInitialized())
            {
                HOperatorSet.DispObj(obj, viewController.viewPort.HalconWindow);
            }
        } 
		public ROI getActiveROI()
		{
			if (activeROIidx != -1)
				return ((ROI)RoiInfo.ROIList[activeROIidx]);

			return null;
		}

		public int getActiveROIIdx()
		{
			return activeROIidx;
		}

		public void setActiveROIIdx(int active)
		{
			activeROIidx = active;
		}

		public int getDelROIIdx()
		{
			return deletedIdx;
		}

       
		public void setROIShape(ROI r)
		{
			roiMode = r;
			roiMode.setOperatorFlag(stateROI);
     
		}


       
		public void setROISign(int mode)
		{
			stateROI = mode;

			if (activeROIidx != -1)
			{
				((ROI)RoiInfo.ROIList[activeROIidx]).setOperatorFlag(stateROI);
				viewController.repaint();
				NotifyRCObserver(ROIController.EVENT_CHANGED_ROI_SIGN);
			}
		}    
		public void removeActive()
		{
			if (activeROIidx != -1)
			{
                RoiInfo.ROIList.RemoveAt(activeROIidx);
                RoiInfo.RemarksList.RemoveAt(activeROIidx);
                deletedIdx = activeROIidx;
				activeROIidx = -1;
				viewController.repaint();
				NotifyRCObserver(EVENT_DELETED_ACTROI);
			}
		}  
		public bool defineModelROI()
		{
			HRegion tmpAdd, tmpDiff, tmp;
			double row, col;

			if (stateROI == MODE_ROI_NONE)
				return true;

			tmpAdd = new HRegion();
			tmpDiff = new HRegion();
           tmpAdd.GenEmptyRegion();
             tmpDiff.GenEmptyRegion();

			for (int i=0; i < RoiInfo.ROIList.Count; i++)
			{
				switch (((ROI)RoiInfo.ROIList[i]).getOperatorFlag())
				{
					case ROI.POSITIVE_FLAG:
						tmp = ((ROI)RoiInfo.ROIList[i]).getRegion();
						tmpAdd = tmp.Union2(tmpAdd);
						break;
					case ROI.NEGATIVE_FLAG:
						tmp = ((ROI)RoiInfo.ROIList[i]).getRegion();
						tmpDiff = tmp.Union2(tmpDiff);
						break;
					default:
						break;
				}
			}

			ModelROI = null;

			if (tmpAdd.AreaCenter(out row, out col) > 0)
			{
				tmp = tmpAdd.Difference(tmpDiff);
				if (tmp.AreaCenter(out row, out col) > 0)
					ModelROI = tmp;
			}

            
			if (ModelROI == null || RoiInfo.ROIList.Count == 0)
				return false;

			return true;
		}
        
		public void reset()
		{
            RoiInfo.clear();
            activeROIidx = -1;
			ModelROI = null;
			roiMode = null;
			NotifyRCObserver(EVENT_DELETED_ALL_ROIS);
		}     
		public void resetROI()
		{
			activeROIidx = -1;
			roiMode = null;
		}     
		public void setDrawColor(string aColor,
								   string aHdlColor,
								   string inaColor)
		{
			if (aColor != "")
				activeCol = aColor;
			if (aHdlColor != "")
				activeHdlCol = aHdlColor;
			if (inaColor != "")
				inactiveCol = inaColor;
		}    
		public void paintData(HalconDotNet.HWindow window)
		{
            if (viewController.ShowMargin)
            {
                window.SetDraw("margin");
            }
            else
            {
                window.SetDraw("fill");
            }
			window.SetLineWidth(2);
            try
            {

          
			if (RoiInfo.ROIList.Count > 0)
			{
				window.SetColor(inactiveCol);				 
				for (int i=0; i < RoiInfo.ROIList.Count; i++)//画出列表中的ROI
				{
                    if (RoiInfo.ROIList[i] == null)
                    {
                        continue;
                    }
					window.SetLineStyle(((ROI)RoiInfo.ROIList[i]).flagLineStyle);
					((ROI)RoiInfo.ROIList[i]).draw(window);
                    window.SetColor(inactiveCol);
                    HTuple row1, colum1, row2, colum2;
                    if(!((RoiInfo.ROIList[i] is ROICross)|| (RoiInfo.ROIList[i] is ROIDXF)))
                    {
                        ((ROI)RoiInfo.ROIList[i]).getRegion().SmallestRectangle1(out row1, out colum1, out row2, out colum2);
                        window.SetTposition(row1.I - 30, colum1.I - 30);
                        window.WriteString(RoiInfo.RemarksList[i].ToString());
                    }                 
                }

				if (activeROIidx != -1)//活跃的ROI显示其他颜色
				{
					window.SetColor(activeCol);
					window.SetLineStyle(((ROI)RoiInfo.ROIList[activeROIidx]).flagLineStyle);
					((ROI)RoiInfo.ROIList[activeROIidx]).draw(window);
                    window.SetColor(activeHdlCol);
					((ROI)RoiInfo.ROIList[activeROIidx]).displayActive(window);
				}
			}
            }
            catch
            {


            }
            if (viewController.isShowRainBow)
            {
                HTuple hv_text = new HTuple();
                HOperatorSet.GetDomain(viewController.GetImage(), out HObject domain);
                HOperatorSet.MinMaxGray(domain, viewController.GetImage(), 0, out HTuple min,
                    out HTuple max, out HTuple range);
                hv_text[0] = max * viewController.scaleZ;
                hv_text[1] = (min + (max - min) / 2) * viewController.scaleZ;
                hv_text[2] = min * viewController.scaleZ;

                HTuple rec_height = viewController.imageHeight / (20 * 3);
                HTuple rec_width = viewController.imageWidth / 25;
                ImageHelper.DispRaiBowLable(window, hv_text, rec_height, rec_width, 10, 10);
            }
        }

     
        public int AddStaicROI(double imgX, double imgY)
        {
            int idxROI = -1;
            double max = 10000, dist = 0;// 
            double epsilon = 15.0;			//           
            if (roiMode != null)			 // 
            {
                roiMode.createROI(imgX, imgY);
                RoiInfo.ROIList.Add(roiMode);
                if (ROIName == null)
                    ROIName = "";
                RoiInfo.RemarksList.Add(ROIName);
                roiMode = null;
                activeROIidx = RoiInfo.ROIList.Count - 1;
                viewController.repaint();
                NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
            }
            else if (RoiInfo.ROIList.Count > 0)		// 
            {
                activeROIidx = -1;

                for (int i = 0; i < RoiInfo.ROIList.Count; i++)
                {
                    dist = ((ROI)RoiInfo.ROIList[i]).distToClosestHandle(imgX, imgY);
                    if ((dist < max) && (dist < epsilon))
                    {
                        max = dist;
                        idxROI = i;
                    }
                }//end of for

                if (idxROI >= 0)
                {
                    activeROIidx = idxROI;
                    NotifyRCObserver(ROIController.EVENT_ACTIVATED_ROI);
                }

                viewController.repaint();
            }
            return activeROIidx;
        }
        
		public int mouseDownAction(double imgX, double imgY)
		{
			int idxROI= -1;
            double max = 10000, dist = 0;// 
			double epsilon = 15.0;			// 

            if (roiMode != null)			 // 
			{
				roiMode.createROI(imgX, imgY);
                RoiInfo.ROIList.Add(roiMode);
                RoiInfo.RemarksList.Add(ROIName);
                roiMode = null;
				activeROIidx = RoiInfo.ROIList.Count - 1;
				viewController.repaint();
				NotifyRCObserver(ROIController.EVENT_CREATED_ROI);
			}
            else if (RoiInfo.ROIList.Count > 0)		//  
			{
				activeROIidx = -1;

				for (int i =0; i < RoiInfo.ROIList.Count; i++)
				{
                    if (RoiInfo.ROIList[i] == null)
                    {
                        continue;
                    }
					dist = ((ROI)RoiInfo.ROIList[i]).distToClosestHandle(imgX, imgY);
					if ((dist < max) && (dist < epsilon))
					{
						max = dist;
						idxROI = i;
					}
				}

				if (idxROI >= 0)
				{
					activeROIidx = idxROI;
					NotifyRCObserver(ROIController.EVENT_ACTIVATED_ROI);
				}

				viewController.repaint();
			}
			return activeROIidx;
		}

       
		public void mouseMoveAction(double newX, double newY)
		{
			if ((newX == currX) && (newY == currY))
				return;
            if(activeROIidx>= RoiInfo.ROIList.Count)
            {
                activeROIidx = 0;
            }
			((ROI)RoiInfo.ROIList[activeROIidx]).moveByHandle(newX, newY);
			viewController.repaint();
			currX = newX;
			currY = newY;
			NotifyRCObserver(ROIController.EVENT_MOVING_ROI);
		}
		public void dummyI(int v)
		{
		}

	}
}
