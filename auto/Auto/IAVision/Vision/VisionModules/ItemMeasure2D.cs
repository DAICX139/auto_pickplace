using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionUtility;
using System.Runtime.Serialization;

namespace VisionModules
{
    /// <summary>
    /// 2D测量
    /// </summary>
    [Serializable]
    public class ItemMeasure2D : Item
    {
        public ItemMeasure2D()
        {
        }

        protected ItemMeasure2D(ItemType itemType, int itemID) : base(itemType, itemID)
        {
        }

        [NonSerialized]
        protected HDrawingObject _drawingObject;
        [NonSerialized]
        protected HXLDCont _measureObjectContour = new HXLDCont();
        [NonSerialized]
        protected HXLDCont _measureResultContour = new HXLDCont();
        [NonSerialized]
        protected HXLDCont _measurePointCross = new HXLDCont();
        [NonSerialized]
        protected HXLDCont _measureCenterCross = new HXLDCont();
        [NonSerialized]
        protected HTuple _measureRow = new HTuple();
        [NonSerialized]
        protected HTuple _measureCol = new HTuple();

        /// <summary>
        /// 绘制ROI对象
        /// </summary>
        public virtual HDrawingObject DrawingObject
        {
            get { return _drawingObject; }
        }
        /// <summary>
        /// 测量模型轮廓
        /// </summary>
        public HXLDCont MeasureObjectContour
        {
            get { return _measureObjectContour; }
        }
        /// <summary>
        /// 测量结果轮廓
        /// </summary>
        public HXLDCont MeasureResultContour
        {
            get { return _measureResultContour; }
        }
        /// <summary>
        /// 测量点十字
        /// </summary>
        public HXLDCont MeasurePointCross
        {
            get { return _measurePointCross; }
        }
        /// <summary>
        /// 测量中心点轮廓
        /// </summary>
        public HXLDCont MeasureCenterCross
        {
            get { return _measureCenterCross; }
        }

        /// <summary>
        /// 测量点行坐标
        /// </summary>
        public HTuple MeasureRow
        {
            get { return _measureRow; }
        }

        /// <summary>
        /// 测量点列坐标
        /// </summary>
        public HTuple MeasureCol
        {
            get { return _measureCol; }
        }


        /// <summary>
        /// 测量参数
        /// </summary>
        public MeasureParam MeasureParam { get; set; } = new MeasureParam();

        /// <summary>
        /// Mask区域
        /// </summary>
        public HRegion MaskRegion { get; set; } = new HRegion((double)0, 0, 0);





        public virtual void OnDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
        }


        public override void Execute(bool blnByHand = false)
        {
            base.Execute(blnByHand);
        }

        [OnSerializing()]
        public void ItemMeasure2DSerializing(StreamingContext context)
        {

            if (MaskRegion == null || MaskRegion.IsInitialized() == false)
            {
                MaskRegion = new HRegion();
            }
        }

        [OnDeserializing()]
        public void ItemMeasure2DDeSerializing(StreamingContext context)
        {
            _measureRow = new HTuple();
            _measureCol = new HTuple();
            _measureObjectContour = new HXLDCont();
            _measureResultContour = new HXLDCont();
            _measurePointCross = new HXLDCont();
            _measureCenterCross = new HXLDCont();

        }



        [OnDeserialized()]
        public void ItemMeasure2DDeSerialized(StreamingContext context)
        {
            if (MaskRegion == null)
            {
                MaskRegion = new HRegion();
            }
        }


    }
}
