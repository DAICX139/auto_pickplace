using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VisionFlows
{
    public class StaticCameraCalib
    {
        //存九个点像素坐标和世界坐标
        public HTuple ALL_row1, ALL_colum1, ALL_X1, ALL_Y1;

        //存五个点旋转坐标
        public HTuple rotate_row, rotate_colum;

        public StaticCameraCalib()
        {
            ALL_row1 = new HTuple();
            ALL_colum1 = new HTuple();
            ALL_X1 = new HTuple();
            ALL_Y1 = new HTuple();
            rotate_row = new HTuple();
            rotate_colum = new HTuple();
        }
    }

    /// <summary>
    /// 移动相机标定数据
    /// </summary>
    [Serializable]
    public class MoveCameraCalib
    {
        /// <summary>拍照位行坐标</summary>
        public double row { get; set; }
        /// <summary>拍照位列坐标</summary>
        public double colum { get; set; }
        /// <summary>拍照位X坐标</summary>
        public double X { get; set; }
        /// <summary>拍照位Y坐标</summary>
        public double Y { get; set; }
        /// <summary>对中位X坐标</summary>
        public double tool1_x { get; set; }
        /// <summary>对中位Y坐标</summary>
        public double tool1_y { get; set; }
        public MoveCameraCalib(double _row, double _colum, double _X, double _Y, double _tool1_x, double _tool1_y)
        {
            row = _row;
            colum = _colum;
            X = _X;
            Y = _Y;
            tool1_x = _tool1_x;
            tool1_y = _tool1_y;
        }

        public MoveCameraCalib()
        { }
    }

    /// <summary>
    /// 序列化数据
    /// </summary>
    [Serializable]
    public class SerializableData
    {
        /// <summary>左上相机标定参数 </summary>
        public BindingList<MoveCameraCalib> MoveCameraCalib1 = new BindingList<MoveCameraCalib>();
        /// <summary>右上相机标定参数 </summary>
        public BindingList<MoveCameraCalib> MoveCameraCalib2 = new BindingList<MoveCameraCalib>();
        /// <summary>下相机左上吸嘴标定参数 </summary>
        public BindingList<MoveCameraCalib> StaticCameraCalib1 = new BindingList<MoveCameraCalib>();
        /// <summary>下相机右上吸嘴标定参数 </summary>
        public BindingList<MoveCameraCalib> StaticCameraCalib2 = new BindingList<MoveCameraCalib>();
        /// <summary> 偏移量标定下相机拍Mark点行坐标</summary>
        public double DownCam_mark_Row;
        /// <summary> 偏移量标定下相机拍Mark点列坐标</summary>
        public double DownCam_mark_Col;

        private double _DownCam1_MatrixRad;
        /// <summary>下相机左吸嘴标定映射矩阵旋转角度</summary>
        public double DownCam1_MatrixRad
        {
            get
            {
                HOperatorSet.HomMat2dToAffinePar(AutoNormal_New.serializableData.HomMat2D_down1,
                    out HTuple sx, out HTuple sy, out HTuple phi, out HTuple theta, out HTuple tx, out HTuple ty);
                return phi;
            }
            set
            {
                _DownCam1_MatrixRad = value;
            }
        }

        private double _DownCam2_MatrixRad;
        /// <summary>下相机右吸嘴标定映射矩阵旋转角度</summary>
        public double DownCam2_MatrixRad
        {
            get
            {
                HOperatorSet.HomMat2dToAffinePar(AutoNormal_New.serializableData.HomMat2D_down2,
                    out HTuple sx, out HTuple sy, out HTuple phi, out HTuple theta, out HTuple tx, out HTuple ty);
                return phi;
            }
            set
            {
                _DownCam2_MatrixRad = value;
            }
        }
        #region  新增,左相机吸嘴与光学中心的偏移，可直接使用
        public double xLeftOffset;

        public double yLeftOffset;

        #endregion

        #region 上相机1标定参数

        /// <summary> 偏移量标定左上相机对齐Mark点X坐标</summary>
        public double XShot1;
        /// <summary> 偏移量标定左上相机对齐Mark点Y坐标</summary>
        public double YShot1;


        [XmlIgnore]
        public HTuple HomMat2D_up1;

        private double _UpCam1_MatrixRad;
        /// <summary>左上相机标定映射矩阵旋转角度</summary>
        public double UpCam1_MatrixRad
        {
            get
            {
                HOperatorSet.HomMat2dToAffinePar(AutoNormal_New.serializableData.HomMat2D_up1,
                    out HTuple sx, out HTuple sy, out HTuple phi, out HTuple theta, out HTuple tx, out HTuple ty);
                return phi;
            }
            set
            {
                _UpCam1_MatrixRad = value;
            }
        }

        #endregion 上相机1标定参数

        #region  新增,右相机吸嘴与光学中心的偏移，可直接使用
        public double xRightOffset;

        public double yRightOffset;
        #endregion
        #region 上相机2标定参数

        /// <summary> 偏移量标定右上相机对齐Mark点X坐标</summary>
        public double XShot2;
        /// <summary> 偏移量标定右上相机对齐Mark点Y坐标</summary>
        public double YShot2;

        [XmlIgnore]
        public HTuple HomMat2D_up2;

        private double _UpCam2_MatrixRad;
        /// <summary>右上相机标定映射矩阵旋转角度</summary>
        public double UpCam2_MatrixRad
        {
            get
            {
                HOperatorSet.HomMat2dToAffinePar(AutoNormal_New.serializableData.HomMat2D_up2,
                    out HTuple sx, out HTuple sy, out HTuple phi, out HTuple theta, out HTuple tx, out HTuple ty);
                return phi;
            }
            set
            {
                _UpCam2_MatrixRad = value;
            }
        }
        #endregion 上相机2标定参数

        #region 吸嘴1标定参数
        /// <summary> 左吸嘴旋转中心行坐标</summary>
        public double Rotate_Center_Row1;
        /// <summary> 左吸嘴旋转中心列坐标</summary>
        public double Rotate_Center_Col1;
        /// <summary> 偏移量标定下相机对齐左吸嘴中心点X坐标</summary>
        public double X1;
        /// <summary> 偏移量标定下相机对齐左吸嘴中心点Y坐标</summary>
        public double Y1;
        /// <summary> 左吸嘴下相机角度</summary>
        public double mode_angle1;
        /// <summary> 左吸嘴几何中心行坐标</summary>
        public double mode_row1;
        /// <summary> 左吸嘴几何中心列坐标</summary>
        public double mode_col1;
        [XmlIgnore]
        public HTuple HomMat2D_down1;
        #endregion 吸嘴1标定参数

        #region 吸嘴2标定参数
        /// <summary> 右吸嘴旋转中心行坐标</summary>
        public double Rotate_Center_Row2;
        /// <summary> 右吸嘴旋转中心列坐标</summary>
        public double Rotate_Center_Col2;
        /// <summary> 偏移量标定下相机对齐右吸嘴中心点X坐标</summary>
        public double X2;
        /// <summary> 偏移量标定下相机对齐右吸嘴中心点Y坐标</summary>
        public double Y2;
        /// <summary> 右吸嘴下相机角度</summary>
        public double mode_angle2;
        /// <summary> 右吸嘴几何中心行坐标</summary>
        public double mode_row2;
        /// <summary> 右吸嘴几何中心列坐标</summary>
        public double mode_col2;

        [XmlIgnore]
        public HTuple HomMat2D_down2;

        #endregion 吸嘴2标定参数

        public SerializableData()
        {
            MoveCameraCalib1 = new BindingList<MoveCameraCalib>();
            MoveCameraCalib2 = new BindingList<MoveCameraCalib>();
            HomMat2D_down1 = new HHomMat2D();
            HomMat2D_down2 = new HHomMat2D();
            HomMat2D_up1 = new HHomMat2D();
            HomMat2D_up2 = new HHomMat2D();
        }
    }
}