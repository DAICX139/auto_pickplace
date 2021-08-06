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
    [Serializable]
    public  class ItemCreateRoi:Item
    {
        //
        private ItemCreateRoi()
        {
        }
        //
        public ItemCreateRoi(int itemID) : base(ItemType.创建ROI, itemID)
        {
        }
        [NonSerialized]
        private HImage _imageReduce=new HImage();
        [NonSerialized]
        private  HDrawingObject _drawingObject;
        //
        public string ShapeModelFileName { get; set; }
        public Rectangle2Roi Roi { get; set; } = new Rectangle2Roi(240,320,0,160,120);
        public HImage ImageReduce
        {
            get
            {
                HRegion region = new HRegion();
                region.GenRectangle2(Roi.Row, Roi.Column, Roi.Phi / 180 * Math.PI, Roi.Length1, Roi.Length2);

                _imageReduce = _image.ReduceDomain(region);
                return _imageReduce;
            }
        }
        public HDrawingObject DrawingObject
        {
            get
            {
                if (_drawingObject != null)
                    _drawingObject.Dispose();

                _drawingObject = new HDrawingObject(Roi.Row, Roi.Column, Roi.Phi /180 * Math.PI, Roi.Length1, Roi.Length2);
                _drawingObject.SetDrawingObjectParams("color", "red");
                // The HALCON/C# interface offers convenience methods that encapsulate the set_drawing_object_callback operator.
                _drawingObject.OnAttach(OnDrawingObject);
                _drawingObject.OnDrag(OnDrawingObject);
                _drawingObject.OnResize(OnDrawingObject);
                _drawingObject.OnSelect(OnDrawingObject);

                return _drawingObject;
            }
        }

        public override void Execute(bool blnByHand = false)
        {
            base.Execute(blnByHand);
        }

        public void OnDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                Roi.Row = dobj.GetDrawingObjectParams("row").D;
                Roi.Column = dobj.GetDrawingObjectParams("column").D;
                Roi.Phi = dobj.GetDrawingObjectParams("phi").D/Math.PI*180;
                Roi.Length1 = dobj.GetDrawingObjectParams("length1").D;
                Roi.Length2 = dobj.GetDrawingObjectParams("length2").D;

                //hwin.SetWindowParam("flush", "false");
                //hwin.SetWindowParam("flush", "true");
                //hwin.FlushBuffer();
            }
            catch (HalconException ex)
            {
            }
        }

        [OnDeserializing()]
        public  void ItemCreateRoiDeSerializing(StreamingContext context)
        {
            _imageReduce = new HImage();
            _drawingObject = new HDrawingObject();
        }
    }
}
