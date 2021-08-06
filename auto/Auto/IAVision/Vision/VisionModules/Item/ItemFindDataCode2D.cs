using System;
using System.Windows.Forms;
using HalconDotNet;
using VisionUtility;
using System.Runtime.Serialization;
using System.IO;

namespace VisionModules
{
    [Serializable]
    public class ItemFindDataCode2D : Item
    {
        public ItemFindDataCode2D() : base()
        {
        }
        public ItemFindDataCode2D(int itemID) : base(ItemType.查找二维码, itemID)
        {
        }
        private HDataCode2D _dataCode2DModel = new HDataCode2D();
        /// <summary>二维码模型</summary>
        public HDataCode2D DataCode2DModel
        {
            get { return _dataCode2DModel; }
        }
        [NonSerialized]
        private HTuple _decodedDataStrings = new HTuple();
        public string DecodedDataStrings
        {
            get { return _decodedDataStrings.ToString(); }
        }
        [NonSerialized]
        private HDrawingObject _drawingObject;
        public HDrawingObject DrawingObject
        {
            get
            {
                if (_drawingObject != null)
                    _drawingObject.Dispose();

                _drawingObject = new HDrawingObject(Roi.Row, Roi.Column, Roi.Phi / 180 * Math.PI, Roi.Length1, Roi.Length2);
                _drawingObject.SetDrawingObjectParams("color", "red");
                // The HALCON/C# interface offers convenience methods that encapsulate the set_drawing_object_callback operator.
                _drawingObject.OnAttach(OnDrawingObject);
                _drawingObject.OnDrag(OnDrawingObject);
                _drawingObject.OnResize(OnDrawingObject);
                _drawingObject.OnSelect(OnDrawingObject);

                return _drawingObject;
            }
        }
        [NonSerialized]
        private HImage _imageReduce = new HImage();
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
        [NonSerialized]
        private HTuple _resultHandles = new HTuple();
        public string ResultHandles
        {
            get { return _resultHandles.ToString(); }
        }
        public Rectangle2Roi Roi { get; set; } = new Rectangle2Roi(240, 320, 0, 160, 120);

        public string SymbolType { get; set; }   
        [NonSerialized]
        private HXLDCont _symbolXLD = new HXLDCont();
        public HXLDCont SymbolXLD
        {
            get { return _symbolXLD; }
        }
        public string TrainFolderName { get; set; }

        public override void Execute(bool blnByHand = false)
        {
            try
            {
                base.Execute(blnByHand);
                _symbolXLD.Dispose();
                _symbolXLD = _dataCode2DModel.FindDataCode2d(ImageReduce, new HTuple(), new HTuple(), out _resultHandles, out _decodedDataStrings);
            }
            catch (Exception ex)
            {

            }
        }

        public bool ReadDataCode2DModel()
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.DefaultExt = ".dcm";
                ofd.Filter = "DCM文件（*.dcm)|*.dcm|所有文件（*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.InitialDirectory = Application.StartupPath;
                ofd.Title = "打开模板";

                if (ofd.ShowDialog() == DialogResult.OK)
                    DataCode2DModel.ReadDataCode2dModel(ofd.FileName);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool TrainDataCode2DModel(string path)
        {
            try
            {
                _dataCode2DModel.Dispose();
                _dataCode2DModel.CreateDataCode2dModel(SymbolType, new HTuple(), new HTuple());

                DirectoryInfo theFolder = new DirectoryInfo(path);
                FileInfo[] fileInfo = theFolder.GetFiles();//获取所在目录的文件
                foreach (FileInfo file in fileInfo) //遍历文件
                {
                    _image.ReadImage(file.FullName);
                    //Train the model with the symbol in the image
                    _symbolXLD.Dispose();
                    _symbolXLD = _dataCode2DModel.FindDataCode2d(ImageReduce, "train", "all", out _resultHandles, out _decodedDataStrings);
                }
            }
            catch (Exception ex)
            {

            }
            return true;
        }


        public bool WriteDataCode2DModel()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();

                sfd.Title = "保存模板";
                sfd.DefaultExt = ".dcm";
                sfd.Filter = "DCM文件（*.dcm)|*.dcm|所有文件（*.*)|*.*";
                sfd.FilterIndex = 1;
                sfd.InitialDirectory = Application.StartupPath;

                if (sfd.ShowDialog() == DialogResult.OK)
                    DataCode2DModel.WriteDataCode2dModel(sfd.FileName);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [OnDeserializing()]
        public void ItemFindDataCode2DDeSerializing(StreamingContext context)
        {

        }

        [OnDeserialized()]
        public void ItemFindDataCode2DDeSerialized(StreamingContext context)
        {
            //_dataCode2DModel = new HDataCode2D();
            _decodedDataStrings = new HTuple();
            _imageReduce = new HImage();
            _resultHandles = new HTuple();
            _symbolXLD = new HXLDCont();


        }

        [OnSerializing()]
        public void ItemFindDataCode2DSerializing(StreamingContext context)
        {
        }



        public void OnDrawingObject(HDrawingObject dobj, HWindow hwin, string type)
        {
            try
            {
                Roi.Row = dobj.GetDrawingObjectParams("row").D;
                Roi.Column = dobj.GetDrawingObjectParams("column").D;
                Roi.Phi = dobj.GetDrawingObjectParams("phi").D / Math.PI * 180;
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
    }
}
