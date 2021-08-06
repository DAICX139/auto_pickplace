using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace VisionUtility
{
    /// <summary>
    /// 数据单元
    /// </summary>
    [Serializable]
    public struct F_DATA_CELL : ICloneable
    {
        private int _Data_CellID; ///所属单元ID，0 --表示全局变量

        public int m_Data_CellID
        {
            set { _Data_CellID = value; }
            get { return _Data_CellID; }
        }

        private DataGroup _Data_Group; //数据组合类型，0--单个变量，1--数组变量

        public DataGroup m_Data_Group
        {
            set { _Data_Group = value; }
            get { return _Data_Group; }
        }

        private DataType _Data_Type; //0-数值型,1--CString字符串型，2--2D点，3--3D点，4－直线，5－面

        public DataType m_Data_Type
        {
            set { _Data_Type = value; }
            get { return _Data_Type; }
        }

        private int _Data_Num; //变量组合数据的个数，单个变量为1，

        public int m_Data_Num
        {
            set { _Data_Num = value; }
            get { return _Data_Num; }
        }

        private DataAtrribution _Data_Atrr;//变量属性，全局变量，局部变量

        public DataAtrribution m_Data_Atrr
        {
            set { _Data_Atrr = value; }
            get { return _Data_Atrr; }
        }

        private String _Data_Name; //变量名称，不可重复，

        public String m_Data_Name
        {
            set { _Data_Name = value; }
            get { return _Data_Name; }
        }

        private String _DataTip; //注释

        public String m_DataTip
        {
            set { _DataTip = value; }
            get { return _DataTip; }
        }

        private String _Data_InitValue; //变量初值,为了兼容所有变量，将初值类型设为字符串型

        public String m_Data_InitValue
        {
            set { _Data_InitValue = value; }
            get { return _Data_InitValue; }
        }

        private Boolean _bUserDefineVariable;//是否用户自定义代码

        public Boolean m_bUserDefineVariable
        {
            set { _bUserDefineVariable = value; }
            get { return _bUserDefineVariable; }
        }

        //void* m_Data_Value; //变量的值,支持单量和数组形式
        [NonSerialized]
        private Object _Data_Value; //变量的值,支持单量和数组形式

        public Object m_Data_Value
        {
            set { _Data_Value = value; }
            get { return _Data_Value; }
        }

        public F_DATA_CELL(int _CellID, DataGroup _Group, DataType _type, String _Name, String _Tip,
                            String _InitValue, int _Num, Object _Value, DataAtrribution _Atrr)
        {
            _Data_CellID = _CellID;
            _Data_Group = _Group;
            _Data_Type = _type;
            _Data_Name = _Name;
            _DataTip = _Tip;
            _Data_InitValue = _InitValue;
            _Data_Num = _Num;
            _Data_Value = _Value;
            _bUserDefineVariable = false;
            _Data_Atrr = _Atrr;
        }

        public void InitValue(int _CellID, string _Name, DataType _DataType, string _InitValue)
        {
            _Data_CellID = _CellID;
            _Data_Name = _Name;
            InitValue(_DataType, _InitValue);
        }

        public void InitValue(DataType _DataType, string _InitValue)
        {
            _Data_Type = _DataType;
            _Data_InitValue = _InitValue;
            switch (_Data_Type)
            {
                case DataType.数值型:
                    _Data_Value = new List<Double>() { double.Parse(_InitValue) };
                    break;

                case DataType.字符串:
                    _Data_Value = new List<string>() { _InitValue };
                    break;

                    //case DataType.点2D:
                    //    _Data_Value = new List<PointF>() { new PointF() };
                    //    break;

                    //case DataType.点3D:
                    //    _Data_Value = new List<Point3DF>() { new Point3DF() };
                    //    break;

                    //case DataType.矩形阵列:
                    //    _Data_Value = new List<RectInfo>() { new RectInfo() };
                    //    break;

                    //case DataType.旋转矩形:
                    //    _Data_Value = new List<Rectangle2_INFO>() { new Rectangle2_INFO() };
                    //    break;

                    //case DataType.图像:
                    //    _Data_Value = new List<HImageExt>() { new HImageExt() };
                    //    break;

                    //case DataType.椭圆:
                    //    _Data_Value = new List<Ellipse_INFO>() { new Ellipse_INFO() };
                    //    break;

                    //case DataType.位置转换2D:
                    //    _Data_Value = new List<HHomMat2D>() { new HHomMat2D() };
                    //    break;

                    //case DataType.圆:
                    //    _Data_Value = new List<Circle_INFO>() { new Circle_INFO() };
                    //    break;

                    //case DataType.直线:
                    //    _Data_Value = new List<Line_INFO>() { new Line_INFO() };
                    //    break;

                    //case DataType.坐标系:
                    //    _Data_Value = new List<Coordinate_INFO>() { new Coordinate_INFO() };
                    //    break;

                    //case DataType.布尔型:
                    //    _Data_Value = new List<bool>() { Convert.ToBoolean(_InitValue.ToUpper()) };
                    //    break;

                    //case DataType.平面:
                    //    _Data_Value = new List<Plane_INFO>() { new Plane_INFO() };
                    //    break;

                    //default:
                    //    Helper.LogHandler.Instance.VTLogError("未处理数据类型 " + _Data_Type.ToString() + " 数据信息 ");
                    //    break;
            }
        }

        /// <summary>
        /// 根据类型设置值
        /// </summary>
        /// <param name="_DataType">类型</param>
        /// <param name="_value">值不为list</param>
        public void SetValue(DataType _DataType, object _value)
        {
            //if (_value == null)
            //{
            //    SetNull(_DataType);
            //}
            //else
            //{
            if (_value.GetType().Name.Contains("List"))
            {
                _Data_Value = _value;
            }
            else
            {
                switch (_Data_Type)
                {
                    case DataType.数值型:
                        _Data_Value = new List<Double>() { (double)_value };
                        break;

                    case DataType.字符串:
                        _Data_Value = new List<string>() { (string)_value };
                        break;

                        //case DataType.点2D:
                        //    _Data_Value = new List<PointF>() { (PointF)_value };
                        //    break;

                        //case DataType.点3D:
                        //    _Data_Value = new List<Point3DF>() { (Point3DF)_value };
                        //    break;

                        //case DataType.矩形阵列:
                        //    _Data_Value = new List<RectInfo>() { (RectInfo)_value };
                        //    break;

                        //case DataType.旋转矩形:
                        //    _Data_Value = new List<Rectangle2_INFO>() { (Rectangle2_INFO)_value };
                        //    break;

                        //case DataType.图像:
                        //    _Data_Value = new List<HImageExt>() { (HImageExt)_value };
                        //    break;

                        //case DataType.椭圆:
                        //    _Data_Value = new List<Ellipse_INFO>() { (Ellipse_INFO)_value };
                        //    break;

                        //case DataType.位置转换2D:
                        //    _Data_Value = new List<HHomMat2D>() { (HHomMat2D)_value };
                        //    break;

                        //case DataType.圆:
                        //    _Data_Value = new List<Circle_INFO>() { (Circle_INFO)_value };
                        //    break;

                        //case DataType.直线:
                        //    _Data_Value = new List<Line_INFO>() { (Line_INFO)_value };
                        //    break;

                        //case DataType.坐标系:
                        //    _Data_Value = new List<Coordinate_INFO>() { (Coordinate_INFO)_value };
                        //    break;

                        //case DataType.布尔型:
                        //    _Data_Value = new List<bool>() { (bool)_value };
                        //    break;

                        //case DataType.平面:
                        //    _Data_Value = new List<Plane_INFO>() { (Plane_INFO)_value };
                        //    break;

                        //default:
                        //    Helper.LogHandler.Instance.VTLogError("未处理数据类型 " + _Data_Type.ToString() + " 数据信息 " + _value.GetType().ToString());
                        //    break;
                }

                //}
            }
        }

        public void SetNull(DataType _DataType)
        {
            switch (_Data_Type)
            {
                case DataType.数值型:
                    _Data_Value = new List<Double>();
                    break;

                case DataType.字符串:
                    _Data_Value = new List<string>();
                    break;

                    //case DataType.点2D:
                    //    _Data_Value = new List<PointF>();
                    //    break;

                    //case DataType.点3D:
                    //    _Data_Value = new List<Point3DF>();
                    //    break;

                    //case DataType.矩形阵列:
                    //    _Data_Value = new List<RectInfo>();
                    //    break;

                    //case DataType.旋转矩形:
                    //    _Data_Value = new List<Rectangle2_INFO>();
                    //    break;

                    //case DataType.图像:
                    //    _Data_Value = new List<HImageExt>();
                    //    break;

                    //case DataType.椭圆:
                    //    _Data_Value = new List<Ellipse_INFO>();
                    //    break;

                    //case DataType.位置转换2D:
                    //    _Data_Value = new List<HHomMat2D>();
                    //    break;

                    //case DataType.圆:
                    //    _Data_Value = new List<Circle_INFO>();
                    //    break;

                    //case DataType.直线:
                    //    _Data_Value = new List<Line_INFO>();
                    //    break;

                    //case DataType.坐标系:
                    //    _Data_Value = new List<Coordinate_INFO>();
                    //    break;

                    //case DataType.布尔型:
                    //    _Data_Value = new List<bool>();
                    //    break;

                    //case DataType.平面:
                    //    _Data_Value = new List<Plane_INFO>();
                    //    break;

                    //default:
                    //    Helper.LogHandler.Instance.VTLogError("未处理数据类型 " + _Data_Type.ToString());
                    //    break;
            }
        }

        public object GetValue()
        {
            object _value = null;
            if (_Data_Group == DataGroup.单量)
            {
                switch (_Data_Type)
                {
                    //case DataType.点2D:
                    //    _value = ((List<PointF>)_Data_Value)[0];
                    //    break;

                    //case DataType.点3D:
                    //    _value = ((List<Point3DF>)_Data_Value)[0];
                    //    break;

                    //case DataType.矩形阵列:
                    //    _value = ((List<RectInfo>)_Data_Value)[0];
                    //    break;

                    //case DataType.旋转矩形:
                    //    _value = ((List<Rectangle2_INFO>)_Data_Value)[0];
                    //    break;

                    case DataType.数值型:
                        _value = ((List<Double>)_Data_Value)[0];
                        break;

                    //case DataType.图像:
                    //    _value = ((List<HImageExt>)_Data_Value)[0];
                    //    break;

                    //case DataType.椭圆:
                    //    _value = ((List<Ellipse_INFO>)_Data_Value)[0];
                    //    break;

                    //case DataType.位置转换2D:
                    //    _value = ((List<HHomMat2D>)_Data_Value)[0];
                    //    break;

                    //case DataType.圆:
                    //    _value = ((List<Circle_INFO>)_Data_Value)[0];
                    //    break;

                    //case DataType.直线:
                    //    _value = ((List<Line_INFO>)_Data_Value)[0];
                    //    break;

                    case DataType.字符串:
                        _value = ((List<string>)_Data_Value)[0];
                        break;

                        //case DataType.坐标系:
                        //    _value = ((List<Coordinate_INFO>)_Data_Value)[0];
                        //    break;

                        //case DataType.布尔型:
                        //    _value = ((List<bool>)_Data_Value)[0];
                        //    break;

                        //case DataType.平面:
                        //    _value = ((List<Plane_INFO>)_Data_Value)[0];
                        //    break;

                        //default:
                        //    Helper.LogHandler.Instance.VTLogError("未处理数据类型 " + _Data_Type.ToString());
                        //    break;
                }
            }
            else
            {
                _value = _Data_Value;
            }
            return _value;
        }

        [OnDeserializing()]
        internal void OnDeSerializingMethod(StreamingContext context)
        {
            _Data_InitValue = "-999.999";
        }

        [OnDeserialized()]
        internal void OnDeSerializedMethod(StreamingContext context)
        {
            InitValue(_Data_Type, _Data_InitValue);
        }

        public object Clone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            return formatter.Deserialize(stream);
        }
    }

    public struct SystemStatus
    {
        private RunMode _runMode;

        public RunMode m_RunMode
        {
            set { _runMode = value; }
            get { return _runMode; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class Roi
    {
        public Roi()
        {

        }

        public string Color { get; set; } = "red";

        public virtual HRegion GenRegion()
        {
            return new HRegion();
        }

        public virtual HXLDCont GenXLDCont()
        {
            return new HXLDCont();
        }
    }

    /// <summary>
    /// 圆形ROI
    /// </summary>
    [Serializable]
    public class CircleRoi : Roi, ICloneable
    {
        public CircleRoi()
        {
        }

        public CircleRoi(double row, double column, double radius)
        {
            Row = row;
            Column = column;
            Radius = radius;
        }

        public CircleRoi(double row, double column, double radius, double startPhi, double endPhi, string pointOrder, double resolution)
        {
            Row = row;
            Column = column;
            Radius = radius;
            StartPhi = startPhi;
            EndPhi = endPhi;
            PointOrder = pointOrder;
            Resolution = resolution;
        }

        public double Row { get; set; } = 0;
        public double Column { get; set; } = 0;
        public double Radius { get; set; } = 0;
        /// <summary>unit:deg,halcon:rad </summary>
        public double StartPhi { get; set; } = 0;
        /// <summary>unit:deg,halcon:rad </summary>
        public double EndPhi { get; set; } = 360;
        public string PointOrder { get; set; } = "positive";
        public double Resolution { get; set; } = 1.0;

        public override HRegion GenRegion()
        {
            HRegion region = new HRegion();
            region.GenCircle(Row, Column, Radius);
            return region;
        }

        public override HXLDCont GenXLDCont()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenCircleContourXld(Row, Column, Radius, StartPhi, EndPhi, PointOrder, Resolution);
            return xld;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    /// <summary>
    /// 矩形1ROI
    /// </summary>
    [Serializable]
    public class Rectangle1Roi : Roi, ICloneable
    {
        public Rectangle1Roi()
        {
        }

        public Rectangle1Roi(double row1, double column1, double row2, double column2)
        {
            Row1 = row1;
            Column1 = column1;
            Row2 = row2;
            Column2 = column2;
        }

        public double Row1 { get; set; } = 0;
        public double Column1 { get; set; } = 0;
        public double Row2 { get; set; } = 0;
        public double Column2 { get; set; } = 0;

        public override HRegion GenRegion()
        {
            HRegion region = new HRegion();
            region.GenRectangle1(Row1, Column1, Row2, Column2);
            return region;
        }

        public override HXLDCont GenXLDCont()
        {
            HXLDCont xld = new HXLDCont();
            HTuple rows = new HTuple(new double[] { Row1, Row1, Row2, Row2, Row1 });
            HTuple columns = new HTuple(new double[] { Column1, Column1, Column2, Column2, Column1 });
            xld.GenContourPolygonXld(rows, columns);
            return xld;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    /// <summary>
    /// 矩形2ROI
    /// </summary>
    [Serializable]
    public class Rectangle2Roi : Roi, ICloneable
    {
        public double Row { get; set; } = 0;
        public double Column { get; set; } = 0;
        /// <summary>unit:deg,halcon:rad </summary>
        public double Phi { get; set; } = 0;
        public double Length1 { get; set; } = 0;
        public double Length2 { get; set; } = 0;

        public Rectangle2Roi()
        {
        }

        public Rectangle2Roi(double row, double column, double phi, double length1, double length2)
        {
            Row = row;
            Column = column;
            Phi = phi;
            Length1 = length1;
            Length2 = length2;
        }

        public override HRegion GenRegion()
        {
            HRegion region = new HRegion();
            region.GenRectangle2(Row, Column, Phi, Length1, Length2);
            return region;
        }

        public override HXLDCont GenXLDCont()
        {
            HXLDCont xld = new HXLDCont();
            xld.GenRectangle2ContourXld(Row, Column, Phi, Length1, Length2);
            return xld;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    /// <summary>
    /// 模型匹配参数
    /// </summary>
    [Serializable]
    public class ModelParam
    {
        public ModelParam()
        {

        }

        public ModelParam(double angleStart, double angleExtent, double scaleMin, double scaleMax, int contrast, 
            double minScore, int numMatches, double maxOverlap, string subPixel, int numLevels, double greediness)
        {
            AngleStart = angleStart;
            AngleExtent = angleExtent;
            ScaleMin = scaleMin;
            ScaleMax = scaleMax;
            Contrast = contrast;
            MinScore = minScore;
            NumMatches = numMatches;
            MaxOverlap = maxOverlap;
            SubPixel = subPixel;
            NumLevels = numLevels;
            Greediness = greediness;
        }
        /// <summary>unit:deg,halcon:rad </summary>
        public double AngleStart { get; set; } = -22.3453;
        /// <summary>unit:deg,halcon:rad </summary>
        public double AngleExtent { get; set; } = 44.6907;
        public double ScaleMin { get; set; } = 0.9;
        public double ScaleMax { get; set; } = 1.1;
        public int Contrast { get; set; } = 40;
        public double MinScore { get; set; } = 0.5;
        public int NumMatches { get; set; } = 1;
        public double MaxOverlap { get; set; } = 0.5;
        public string SubPixel { get; set; } = "least_squares";
        public int NumLevels { get; set; } = 0;
        public double Greediness { get; set; } = 0.9;
    }

    /// <summary>
    /// 模板匹配结果数据
    /// </summary>
    [Serializable]
    public class ModelResult
    {
        public ModelResult()
        {
        }

        public double Row { get; set; }
        public double Column { get; set; }
        /// <summary>unit:deg,halcon:rad </summary>
        public double Angle { get; set; }
        public double Scale { get; set; }
        public double Score { get; set; }
    }

    /// <summary>
    /// 测量参数
    /// </summary>
    [Serializable]
    public class MeasureParam
    {
        public MeasureParam()
        {

        }
        public MeasureParam(double measureLength1, double measureLength2, double measureSigma, double measureThreshold,
            double measureDistance, string measureTransition, string measureSelect, double minScore)
        {
            MeasureLength1 = measureLength1;
            MeasureLength2 = measureLength2;
            MeasureSigma = measureSigma;
            MeasureThreshold = measureThreshold;
            MeasureDistance = measureDistance;
            MeasureTransition = measureTransition;
            MeasureSelect = measureSelect;
            MinScore = minScore;
        }

        public double MeasureLength1 { get; set; } = 20.0;
        public double MeasureLength2 { get; set; } = 5.0;
        public double MeasureSigma { get; set; } = 1.0;
        public double MeasureThreshold { get; set; } = 30.0;
        public double MeasureDistance { get; set; } = 10.0;
        public string MeasureTransition { get; set; } = "positive";
        public string MeasureSelect { get; set; } = "first";
        public double MinScore { get; set; } = 0.3;
    }

    /// <summary>
    /// NPoint标定参数
    /// </summary>
    [Serializable]
    public class NPointParam
    {
        public NPointParam()
        {

        }

        public NPointParam(double baseX, double baseY, double offsetX, double offsetY, int baseAngle)
        {
            BaseX = baseX;
            BaseY = baseY;
            OffsetX = offsetX;
            OffsetY = offsetY;
            BaseAngle = baseAngle;
        }
        /// <summary> 机械基准点坐标X</summary>
        public double BaseX { get; set; } = 100;
        /// <summary> 机械基准点坐标Y</summary>
        public double BaseY { get; set; } = 100;
        /// <summary> 机械X方向偏移量</summary>
        public double OffsetX { get; set; } = 5;
        /// <summary> 机械Y方向偏移量</summary>
        public double OffsetY { get; set; } = 5;
        /// <summary> 机械基准角度</summary>
        public double BaseAngle { get; set; } = 0;
    }

    /// <summary>
    /// N点标定结果
    /// </summary>
    [Serializable]
    public class NPointResult
    {
        public NPointResult()
        {
        }
        /// <summary> 像素当量X(mm)</summary>
        public double Sx { get; set; }
        /// <summary> 像素当量Y(mm)</summary>
        public double Sy { get; set; }
        /// <summary> 旋转角度(°)</summary>
        public double Phi { get; set; }
        /// <summary> 倾斜角度(°)</summary>
        public double Theta { get; set; }
        /// <summary> 平移X(mm)</summary>
        public double Tx { get; set; }
        /// <summary> 平移Y(mm)</summary>
        public double Ty { get; set; }
        /// <summary> 旋转中心R(pix)</summary>
        public double Rr { get; set; }
        /// <summary> 旋转中心C(pix)</summary>
        public double Rc { get; set; }
    }

    #region"Laser"
    public class MachineBasePoint
    {
        public MachineBasePoint()
        {

        }

        public MachineBasePoint(double baseX, double baseY, double offsetX, double offsetY)
        {
            BaseX = baseX;
            BaseY = baseY;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }
        /// <summary> 机械基准点坐标X</summary>
        public double BaseX { get; set; } = 100;
        /// <summary> 机械基准点坐标Y</summary>
        public double BaseY { get; set; } = 100;
        /// <summary> 机械X方向偏移量</summary>
        public double OffsetX { get; set; } = 5;
        /// <summary> 机械Y方向偏移量</summary>
        public double OffsetY { get; set; } = 5;

    }

    public class LocationParam
    {
        public LocationParam()
        {
        }

        public double MinScore { get; set; } = 0.5;
        public int NumMatches { get; set; } = 1;
        public double MaxOverlap { get; set; } = 0.5;
        public string SubPixel { get; set; } = "least_squares";
        public int NumLevels { get; set; } = 0;
        public double Greediness { get; set; } = 0.9;
        public double ratioArea { get; set; } = 0.02;
    }

    public class LocationResult
    {
        public LocationResult()
        {
        }

        public double Row { get; set; }
        public double Column { get; set; }
        /// <summary>unit:deg,halcon:rad </summary>
        public double Angle { get; set; }
        public bool IsCode { get; set; }
        public bool IsDark { get; set; }
    }

    public class DataCodeAndOcrParam
    {
        public DataCodeAndOcrParam()
        {
        }

        public LocationResult location { get; set; }
     
    }

    public class DataCodeAndOcrResult
    {
        public DataCodeAndOcrResult()
        {
        }
        public string  dataCode2DString { get; set; }
        public string  ocrString { get; set; }

    }












    #endregion






}
