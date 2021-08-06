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

        //存三个点旋转坐标
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
        public double row { get; set; }
        public double colum { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        //对位位置
        public double tool1_x { get; set; }

        public double tool1_y { get; set; }

        public MoveCameraCalib(double _row, double _colum, double _X, double _Y, double _tool1_x, double _tool1_y)
        {
            row = _row;
            colum = _colum;
            X = _X;
            Y = _Y;
            //对位位置
            tool1_x = _tool1_x;
            tool1_y = _tool1_y;
        }

        private MoveCameraCalib()
        {
        }
    }

    /// <summary>
    /// 序列化数据
    /// </summary>

    [Serializable]
    public class SerializableData
    {
        public BindingList<MoveCameraCalib> MoveCameraCalib1 = new BindingList<MoveCameraCalib>();
        public BindingList<MoveCameraCalib> MoveCameraCalib2 = new BindingList<MoveCameraCalib>();

        #region 上相机1标定参数

        /// <summary>
        /// 上相机1的相机参考坐标
        /// </summary>
        public double XShot1;

        public double YShot1;

        [XmlIgnore]
        public HTuple HomMat2D_up1;

        //上相机1模板图（印泥图）的角度
        public double UpCam_angle1;

        #endregion 上相机1标定参数

        #region 上相机2标定参数

        /// <summary>
        /// 上相机2的相机参考坐标
        /// </summary>
        public double XShot2;

        public double YShot2;

        [XmlIgnore]
        public HTuple HomMat2D_up2;

        //上相机2模板图（印泥图）的角度
        public double UpCam_angle2;

        #endregion 上相机2标定参数

        #region 吸嘴1标定参数

        // 计算出来的旋转中心吸嘴1
        public double Rotate_Center_Row1;

        public double Rotate_Center_Col1;

        // 吸嘴1（轴1）参考坐标
        public double X1;

        public double Y1;

        //吸嘴1模板图的角度
        public double mode_angle1;

        //吸嘴1模板图的row
        public double mode_row1;

        //吸嘴1模板图的col
        public double mode_col1;

        [XmlIgnore]
        public HTuple HomMat2D_down1;

        #endregion 吸嘴1标定参数

        #region 吸嘴2标定参数

        // 计算出来的旋转中心吸嘴2
        public double Rotate_Center_Row2;

        public double Rotate_Center_Col2;

        // 吸嘴2（轴2）参考坐标
        public double X2;

        public double Y2;

        //吸嘴1模板图的角度
        public double mode_angle2;

        //吸嘴1模板图的row
        public double mode_row2;

        //吸嘴1模板图的col
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