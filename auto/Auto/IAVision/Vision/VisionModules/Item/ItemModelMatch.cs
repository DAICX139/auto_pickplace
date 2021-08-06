using System;
using HalconDotNet;
using VisionUtility;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace VisionModules
{
    [Serializable]
    public class ItemModelMatch : Item
    {
        public ItemModelMatch() : base()
        {
        }
        public ItemModelMatch(int itemID) : base(ItemType.模板匹配, itemID)
        {
        }

        [NonSerialized]
        private ModelResult _resultData = new ModelResult();
        [NonSerialized]
        private HImage _imageRoi = new HImage();
        [NonSerialized]
        private HImage _imageCrop = new HImage();
        [NonSerialized]
        private HXLDCont _shapeModelContour = new HXLDCont();
        [NonSerialized]
        private HXLDCont _shapeModelContourTrans = new HXLDCont();
        [NonSerialized]
        private HRegion _modelRegion = new HRegion();
        [NonSerialized]
        protected HDrawingObject _drawingObject;
        //[NonSerialized]
        private HShapeModel _shapeModel = new HShapeModel();
        /// <summary> ROI</summary>
        public Rectangle1Roi Roi { set; get; } = new Rectangle1Roi(140, 220, 340, 420);
        /// <summary>Mask区域 </summary>
        public HRegion MaskRegion { get; set; } = new HRegion((double)0, 0, 0);
        /// <summary>模型参数</summary>
        public ModelParam ModelParam { set; get; } = new ModelParam();
        /// <summary>形状模型</summary>
        public HShapeModel ShapeModel
        {
            get { return _shapeModel; }
            set { _shapeModel = value; }
        }

        public HXLDCont ShapeModelContour
        {
            get
            {
                if (_shapeModel.IsInitialized())
                    _shapeModelContour = _shapeModel.GetShapeModelContours(1);

                return _shapeModelContour;
            }
        }

        public HXLDCont ShapeModelContourTrans
        {
            get { return _shapeModelContourTrans; }
        }

        public HRegion ModelRegion
        {
            get { return _modelRegion; }
        }

        /// <summary>
        /// 模板匹配结果数据
        /// </summary>
        public ModelResult ResultData
        {
            get { return _resultData; }
        }

        public HImage ImageRoi
        {
            get
            {
                _imageRoi = _image.ReduceDomain(new HRegion(Roi.Row1, Roi.Column1, Roi.Row2, Roi.Column2));
                return _imageRoi;
            }
        }

        public HImage ImageCrop
        {
            get
            {
                _imageCrop = _image.CropRectangle1(Roi.Row1, Roi.Column1, Roi.Row2, Roi.Column2);
                return _imageCrop;
            }
        }

        public HDrawingObject DrawingObject
        {
            get
            {
                if (_drawingObject != null)
                    _drawingObject.Dispose();

                _drawingObject = new HDrawingObject(Roi.Row1, Roi.Column1, Roi.Row2, Roi.Column2);
                _drawingObject.SetDrawingObjectParams("color", "red");
                // The HALCON/C# interface offers convenience methods that encapsulate the set_drawing_object_callback operator.
                _drawingObject.OnAttach(OnDrawingObject);
                _drawingObject.OnDrag(OnDrawingObject);
                _drawingObject.OnResize(OnDrawingObject);
                _drawingObject.OnSelect(OnDrawingObject);

                return _drawingObject;
            }
        }

        private void OnDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                Roi.Row1 = dobj.GetDrawingObjectParams("row1").D;
                Roi.Column1 = dobj.GetDrawingObjectParams("column1").D;
                Roi.Row2 = dobj.GetDrawingObjectParams("row2").D;
                Roi.Column2 = dobj.GetDrawingObjectParams("column2").D;
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
                VisionModulesHelper.FindScaledShapeModel(ImageRoi, ShapeModel, ModelParam, out _shapeModelContourTrans, out _resultData);
            }
            catch (Exception ex)
            {

            }
        }

        public void CreateModeBaseRoi()
        {
            try
            {
                VisionModulesHelper.CreateScaledShapeModel(ImageRoi, ModelParam, out _shapeModel, out _modelRegion);
            }
            catch (Exception ex)
            {
            }
        }

        public void CreateModeBaseMask()
        {
            try
            {
                HRegion regionImageModel = new HRegion();
                regionImageModel.GenEmptyObj();
                regionImageModel = ImageCrop.GetDomain();
                //
                HRegion RectangleRoi = regionImageModel.Difference(MaskRegion);
                VisionModulesHelper.CreateScaledShapeModel(ImageCrop.ReduceDomain(RectangleRoi), ModelParam, out _shapeModel, out _modelRegion);
            }
            catch (Exception ex)
            {
            }
        }

        public bool ReadShapeModel()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.DefaultExt = ".sbm";
                ofd.Filter = "SBM文件（*.sbm)|*.sbm|所有文件（*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.InitialDirectory = Application.StartupPath;
                ofd.Title = "打开模板";

                if (ofd.ShowDialog() == DialogResult.OK)
                    ShapeModel.ReadShapeModel(ofd.FileName);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool WriteShapeModel()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.Title = "保存模板";
                sfd.DefaultExt = ".sbm";
                sfd.Filter = "SBM文件（*.sbm)|*.sbm|所有文件（*.*)|*.*";
                sfd.FilterIndex = 1;
                sfd.InitialDirectory = Application.StartupPath;

                if (sfd.ShowDialog() == DialogResult.OK)
                    ShapeModel.WriteShapeModel(sfd.FileName);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [OnSerializing()]
        public void ItemModelMatchSerializing(StreamingContext context)
        {
        }
        [OnDeserializing()]
        public void ItemModelMatchDeSerializing(StreamingContext context)
        {
        }
        [OnDeserialized()]
        public void ItemModelMatchDeSerialized(StreamingContext context)
        {
            _resultData = new ModelResult();
        }
    }
}
