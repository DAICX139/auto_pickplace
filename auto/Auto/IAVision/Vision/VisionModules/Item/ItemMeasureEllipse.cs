using System;
using HalconDotNet;
using VisionUtility;
using System.Runtime.Serialization;

namespace VisionModules
{
    [Serializable]
    public class ItemMeasureEllipse : ItemMeasure2D
    {
        public ItemMeasureEllipse() : base()
        {
        }

        public ItemMeasureEllipse(int itemID) : base(ItemType.椭圆测量, itemID)
        {
        }

        [NonSerialized]
        private CircleRoi _result = new CircleRoi();

        /// <summary>
        /// ROI
        /// </summary>
        public CircleRoi Roi { set; get; } = new CircleRoi(240, 320, 120);

        /// <summary>
        /// 测量结果圆
        /// </summary>
        public CircleRoi Result
        {
            get { return _result; }
        }

        public override HDrawingObject DrawingObject
        {
            get
            {
                if (_drawingObject != null)
                    _drawingObject.Dispose();

                _drawingObject = new HDrawingObject(Roi.Row, Roi.Column, Roi.Radius);
                _drawingObject.SetDrawingObjectParams("color", "red");
                // The HALCON/C# interface offers convenience methods that encapsulate the set_drawing_object_callback operator.
                _drawingObject.OnAttach(OnDrawingObject);
                _drawingObject.OnDrag(OnDrawingObject);
                _drawingObject.OnResize(OnDrawingObject);
                _drawingObject.OnSelect(OnDrawingObject);

                return _drawingObject;
            }
        }

        public override void OnDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                Roi.Row = dobj.GetDrawingObjectParams("row").D;
                Roi.Column = dobj.GetDrawingObjectParams("column").D;
                Roi.Radius = dobj.GetDrawingObjectParams("radius").D;
            }
            catch (HalconException ex)
            {
            }
        }

        public override void Execute(bool blnByHand = false)
        {
            base.Execute(blnByHand);

            try
            {
                VisionModulesHelper.CircleMeasure(Image, Roi, MeasureParam, MaskRegion, out _result, out _measureRow, out _measureCol, out _measureObjectContour);

                //显示结果
                _measureResultContour.GenCircleContourXld(Result.Row, Result.Column, Result.Radius, 0, 2 * Math.PI, "positive", 1.5);
                _measurePointCross.GenCrossContourXld(_measureRow, _measureCol, new HTuple(MeasureParam.MeasureLength2), new HTuple(45).TupleRad());
                _measureCenterCross.GenCrossContourXld(new HTuple(Result.Row), new HTuple(Result.Column), new HTuple(MeasureParam.MeasureLength2), new HTuple(45).TupleRad());
            }
            catch (Exception ex)
            {
            }
        }

        [OnSerializing()]
        public void ItemMeasureEllipseSerializing(StreamingContext context)
        {
        }

        [OnDeserializing()]
        public void ItemMeasureEllipseDeSerializing(StreamingContext context)
        {
            _result = new CircleRoi();

        }



        [OnDeserialized()]
        public void ItemMeasureEllipseDeSerialized(StreamingContext context)
        {

        }
    }
}
