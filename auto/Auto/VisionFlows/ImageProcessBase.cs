using AlcUtility;
using HalconDotNet;
using Poc2Auto.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionModules;

namespace VisionFlows
{
  public  class ImageProcessBase
    {
        public ImageProcessBase()
        {

        }
        public static int CurrentSoket;
        public static void Execute_New(MessageHandler handler, EnumCamera cameraID)
        {
            Flow.Log(RunModeMgr.RunMode.ToString());
            AutoNormal_New.Execute(handler, cameraID);
        }  

        /// <summary>
        /// Socket Dut 检测
        /// </summary>
        /// <param name="image"></param>
        /// <param name="ho_RegionTrans"></param>
        /// <returns></returns>
        public virtual bool SocketDetect(HImage image, out HObject ho_RegionTrans)
        {
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            return true;
        }

        /// <summary>
        /// tray slot中 Dut 检测有无
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="ho_RegionTrans"></param>
        /// <returns></returns>
        public virtual bool SlotDetect(InputPara parameter, out HObject ho_RegionTrans)
        {
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            return true;
        }

        /// <summary>
        /// 推块距离检测 是否退出指定距离
        /// </summary>
        /// <param name="markrow"></param>
        /// <param name="markcol"></param>
        /// <param name="parameter"></param>
        /// <param name="distance"></param>
        /// <param name="ho_RegionTrans"></param>
        /// <returns></returns>
        public virtual bool BlockDetect(HTuple markrow, HTuple markcol, InputPara parameter,out int distance, out HObject ho_RegionTrans)
        {
            HOperatorSet.GenEmptyObj(out ho_RegionTrans);
            distance = 0;
            return true;
        }
        
        /// <summary>
        /// 空 tray mark点 定位
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual OutPutResult ShotSlotMark(InputPara parameter)
        {
            return new OutPutResult();
        }

        /// <summary>
        /// Tray slot Dut mark点定位
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual OutPutResult TrayDutFront(InputPara parameter)
        {
            return new OutPutResult();
        }

        /// <summary>
        /// 空Tary盘 slot 的 定位
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual OutPutResult ShotSlot(InputPara parameter)
        {
            return new OutPutResult();
        }
        /// <summary>
        /// Socket Mark定位
        /// </summary>
        /// <param name="locationPara"></param>
        /// <param name="mark"></param>
        /// <returns></returns>
        public virtual OutPutResult SocketMark(InputPara locationPara, out HObject mark)
        {
            HOperatorSet.GenEmptyObj(out mark);
            return new OutPutResult();
        }

        /// <summary>
        /// Socket 上面Mark点（两个圆的查找定位）
        /// </summary>
        /// <param name="ho_Image_white"></param>
        /// <param name="hv_Row1"></param>
        /// <param name="hv_Column1"></param>
        /// <param name="hv_Phi"></param>
        /// <param name="mark"></param>
       public virtual  void FindSocketMark(HObject ho_Image_white, out HTuple hv_Row1, out HTuple hv_Column1,
    out HTuple hv_Phi, out HObject mark)
        {
            hv_Row1 = 0;    hv_Column1 = 0;
            hv_Phi = 0;
            HOperatorSet.GenEmptyObj(out mark);
        }
        

        /// <summary>
        /// Socket 中 Dut mark点定位
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public virtual OutPutResult SocketDutFront(InputPara parameter)
        {
            return new OutPutResult();
        }

        /// <summary>
        /// 通过模型匹配 找到 DutBack 五个圆 进行 定位
        /// </summary>
        /// <param name="locationPara"></param>
        /// <param name="plcsend"></param>
        /// <returns></returns>
        public virtual OutPutResult SecondDutBack(InputPara locationPara, PLCSend plcsend)
        {
            return new OutPutResult();
        }

        /// <summary>
        /// 二维码句柄
        /// </summary>
        public static HDataCode2D hDataCode2D=new HDataCode2D();

        /// <summary>
        /// 读二维码
        /// </summary>
        /// <param name="iamge"></param>
        /// <param name="dataCode"></param>
        /// <param name="RecCodeData"></param>
        /// <returns></returns>
        public static bool ReadDataCode(HImage iamge, out string dataCode, out HXLDCont RecCodeData)
        {
            try
            {
                RecCodeData = new HXLDCont();
               if (!hDataCode2D.IsInitialized())
                {
                    hDataCode2D.ReadDataCode2dModel(Utility.Model+"ecc200_trained_model.dcm");
                }
                HTuple DataStrings="";
                for (int i = -50; i < 50; i++)
                {
                   // 设置不同的对比下 进行 二维码查找
                   HImage ScaledImage= iamge.ScaleImage((HTuple)2, i);
                   RecCodeData = hDataCode2D.FindDataCode2d(iamge, new HTuple(), new HTuple(), out HTuple handle, out  DataStrings);
                    if(DataStrings.Length>0)  // 表示就已经找到了
                    {
                        break;
                    }
                }
               
                dataCode = DataStrings;
                if (dataCode == "" && dataCode.Length < 17)
                {
                    RecCodeData = new HXLDCont();
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Flow.Log(ex.Message + ex.StackTrace);
                dataCode = "";
                RecCodeData = new HXLDCont();
                return false;
            }
        }

        /// <summary>
        /// 读取 PLC 发送的数据  PLCSend 这个是值类型
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="plcSend"></param>
        /// <param name="plcRecv"></param>
        public static void GetPlcData(MessageHandler handler, out PLCSend plcSend, out PLCRecv plcRecv)
        {
            try
            {
                plcSend = new PLCSend();
                plcRecv = new PLCRecv();
                var temp = (double[])handler.CmdParam.KeyValues[PLCParamNames.PLCSend].Value;
                plcSend.Func = (int)temp[(int)EnumPLCSend.Func];
                plcRecv.XPos = plcSend.XPos = temp[(int)EnumPLCSend.XPos];
                plcRecv.YPos = plcSend.YPos = temp[(int)EnumPLCSend.YPos];
                plcRecv.ZPos = plcSend.ZPos = temp[(int)EnumPLCSend.ZPos];
                plcRecv.RPos = plcSend.RPos = temp[(int)EnumPLCSend.RPos];
                plcRecv.Result = 1;//00000001
            }
            catch (Exception ex)
            {
                plcSend = new PLCSend();
                plcRecv = new PLCRecv();
                plcSend.Func = 0;
                plcRecv.XPos = plcSend.XPos = 0;
                plcRecv.YPos = plcSend.YPos = 0;
                plcRecv.ZPos = plcSend.ZPos = 0;
                plcRecv.RPos = plcSend.RPos = 0;
                plcRecv.Result = 1;//00000001
            }
        }

        /// <summary>
        /// CameraId= 0（左上）,1（右上）, 2（下）  
        /// </summary>
        /// <param name="CameraId"></param>
        /// <param name="ExposureTime"></param>
        /// <returns></returns>
        public static HImage GrabImage(int CameraId, double ExposureTime)
        {
            int index = 0;
            if (VisionModulesManager.CameraList[CameraId].Image != null)  // 在软触发拍照之前，判断图片又没有释放
            {
                VisionModulesManager.CameraList[CameraId].Image.Dispose();
            }
            else
            {
                VisionModulesManager.CameraList[CameraId].Image = new HImage();
            }                      
            Plc.SetIO(CameraId + 1, true);  // 控制  光源IO点       ？？这需要设置一个延迟
            VisionModulesManager.CameraList[CameraId].SetExposureTime(ExposureTime);
            VisionModulesManager.CameraList[CameraId].CaptureImage();
            resend:
            if (!VisionModulesManager.CameraList[CameraId].CaptureSignal.WaitOne(1500))
            {
                index++;
                if (index>4)
                {
                    Plc.SetIO(CameraId + 1, false);
                    AlcSystem.Instance.ShowMsgBox("图像采集失败", "提示", icon: AlcMsgBoxIcon.Error);
                    return null;
                }
                else
                {
                    goto resend;
                }              
            }
            Thread.Sleep(100);
            Plc.SetIO(CameraId + 1, false);
            if (VisionModulesManager.CameraList[CameraId].Image == null)
            {
                return null;
            }
            return VisionModulesManager.CameraList[CameraId].Image.CopyImage();
            // 返回图像的Copy
        }


        public static void gen_arrow_contour_xld(out HObject ho_Arrow, HTuple hv_Row1, HTuple hv_Column1,
      HTuple hv_Row2, HTuple hv_Column2, HTuple hv_HeadLength, HTuple hv_HeadWidth)
        {



            // Stack for temporary objects 
            HObject[] OTemp = new HObject[20];

            // Local iconic variables 

            HObject ho_TempArrow = null;

            // Local control variables 

            HTuple hv_Length = new HTuple(), hv_ZeroLengthIndices = new HTuple();
            HTuple hv_DR = new HTuple(), hv_DC = new HTuple(), hv_HalfHeadWidth = new HTuple();
            HTuple hv_RowP1 = new HTuple(), hv_ColP1 = new HTuple();
            HTuple hv_RowP2 = new HTuple(), hv_ColP2 = new HTuple();
            HTuple hv_Index = new HTuple();
            // Initialize local and output iconic variables 
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            HOperatorSet.GenEmptyObj(out ho_TempArrow);
            
            //
            //
            //Init
            ho_Arrow.Dispose();
            HOperatorSet.GenEmptyObj(out ho_Arrow);
            hv_Length.Dispose();
            // DistencePp  计算两个点之间的距离
            HOperatorSet.DistancePp(hv_Row1, hv_Column1, hv_Row2, hv_Column2, out hv_Length);
            //
            //Mark arrows with identical start and end point
            //(set Length to -1 to avoid division-by-zero exception)
            hv_ZeroLengthIndices.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_ZeroLengthIndices = hv_Length.TupleFind(
                    0);
            }
            if ((int)(new HTuple(hv_ZeroLengthIndices.TupleNotEqual(-1))) != 0)
            {
                if (hv_Length == null)
                    hv_Length = new HTuple();
                hv_Length[hv_ZeroLengthIndices] = -1;
            }
            //
            //Calculate auxiliary variables.
            hv_DR.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_DR = (1.0 * (hv_Row2 - hv_Row1)) / hv_Length;
            }
            hv_DC.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_DC = (1.0 * (hv_Column2 - hv_Column1)) / hv_Length;
            }
            hv_HalfHeadWidth.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_HalfHeadWidth = hv_HeadWidth / 2.0;
            }
            //
            //Calculate end points of the arrow head.
            hv_RowP1.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_RowP1 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) + (hv_HalfHeadWidth * hv_DC);
            }
            hv_ColP1.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_ColP1 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) - (hv_HalfHeadWidth * hv_DR);
            }
            hv_RowP2.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_RowP2 = (hv_Row1 + ((hv_Length - hv_HeadLength) * hv_DR)) - (hv_HalfHeadWidth * hv_DC);
            }
            hv_ColP2.Dispose();
            using (HDevDisposeHelper dh = new HDevDisposeHelper())
            {
                hv_ColP2 = (hv_Column1 + ((hv_Length - hv_HeadLength) * hv_DC)) + (hv_HalfHeadWidth * hv_DR);
            }
            //
            //Finally create output XLD contour for each input point pair
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_Length.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                if ((int)(new HTuple(((hv_Length.TupleSelect(hv_Index))).TupleEqual(-1))) != 0)
                {
                    //Create_ single points for arrows with identical start and end point
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_TempArrow.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, hv_Row1.TupleSelect(hv_Index),
                            hv_Column1.TupleSelect(hv_Index));
                    }
                }
                else
                {
                    //Create arrow contour
                    using (HDevDisposeHelper dh = new HDevDisposeHelper())
                    {
                        ho_TempArrow.Dispose();
                        HOperatorSet.GenContourPolygonXld(out ho_TempArrow, ((((((((((hv_Row1.TupleSelect(
                            hv_Index))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                            hv_RowP1.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)))).TupleConcat(
                            hv_RowP2.TupleSelect(hv_Index)))).TupleConcat(hv_Row2.TupleSelect(hv_Index)),
                            ((((((((((hv_Column1.TupleSelect(hv_Index))).TupleConcat(hv_Column2.TupleSelect(
                            hv_Index)))).TupleConcat(hv_ColP1.TupleSelect(hv_Index)))).TupleConcat(
                            hv_Column2.TupleSelect(hv_Index)))).TupleConcat(hv_ColP2.TupleSelect(
                            hv_Index)))).TupleConcat(hv_Column2.TupleSelect(hv_Index)));
                    }
                }
                {
                    HObject ExpTmpOutVar_0;
                    HOperatorSet.ConcatObj(ho_Arrow, ho_TempArrow, out ExpTmpOutVar_0);
                    ho_Arrow.Dispose();
                    ho_Arrow = ExpTmpOutVar_0;
                }
            }
            ho_TempArrow.Dispose();

            hv_Length.Dispose();
            hv_ZeroLengthIndices.Dispose();
            hv_DR.Dispose();
            hv_DC.Dispose();
            hv_HalfHeadWidth.Dispose();
            hv_RowP1.Dispose();
            hv_ColP1.Dispose();
            hv_RowP2.Dispose();
            hv_ColP2.Dispose();
            hv_Index.Dispose();

            return;
        }

        /// <summary>
        /// 数据写入CSV
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public static void WriteCsv(string name, string data)
        {
            using (FileStream fs = new System.IO.FileStream(name, FileMode.Append, FileAccess.Write))
            {
                //开始写入文件，streamwriter的第二个参数是设置编码，默认是utf-8
                using (StreamWriter m_streamWriter = new StreamWriter(fs))
                {
                    //写入文件标题，按照循环的方式写入数据
                    //  m_streamWriter.WriteLine("12" + "," + "21" + "," + "323" + "," + "333");
                    m_streamWriter.WriteLine(data);
                    //关闭写入数据，数据写入完成
                    m_streamWriter.Flush();
                    m_streamWriter.Close();
                }
                fs.Close();
            }
        }
    }
}
