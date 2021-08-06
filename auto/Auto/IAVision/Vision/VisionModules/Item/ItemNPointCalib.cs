using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionUtility;
using System.Runtime.Serialization;
using System.Data;

namespace VisionModules
{
    [Serializable]
    public class ItemNPointCalib : Item
    {
        private ItemNPointCalib() : base()
        {
        }
        public ItemNPointCalib(int itemID) : base(ItemType.N点标定, itemID)
        {
            NPointData = InitNPointData();
        }

        [NonSerialized]
        private double _imageR;
        /// <summary>图像行坐标 </summary>
        public double ImageR
        {
            get { return _imageR; }
            set { _imageR = value; }
        }

        [NonSerialized]
        private double _imageC;
        /// <summary>图像列坐标</summary>
        public double ImageC
        {
            get { return _imageC; }
            set { _imageC = value; }
        }

        [NonSerialized]
        private double _machineX;
        /// <summary>机械X坐标</summary>
        public double MachineX
        {
            get { return _machineX; }
            set { _machineX = value; }
        }

        [NonSerialized]
        private double _machineY;
        /// <summary> 机械坐标Y</summary>
        public double MachineY
        {
            get { return _machineY; }
            set { _machineY = value; }
        }

        ///<summary>相机用户ID</summary>
        public string CameraUserID { get; set; }
        ///<summary>是否自动标定</summary>
        public bool IsAutoCalib { get; set; } = true;
        ///<summary>相机安装方式</summary>
        public InstallMode InstallMode { get; set; } = VisionUtility.InstallMode.Fix;
        ///<summary>是否旋转中心标定</summary>
        public bool IsRotateCenterCalib { get; set; } = true;
        [NonSerialized]
        private double[] NPointPathX = new double[] { 0, 1, 1, 0, -1, -1, -1, 0, 1 };
        [NonSerialized]
        private double[] NPointPathY = new double[] { 0, 0, 1, 1, 1, 0, -1, -1, -1 };
        /// <summary>N点标定数据</summary>
        public DataTable NPointData { get; set; }
        /// <summary>N点标定基准参数</summary>
        public NPointParam NPointParam { get; set; } = new NPointParam();
        /// <summary>N点标定结果数据</summary>
        public NPointResult NPointResult { get; set; } = new NPointResult();
        /// <summary>N点标定变换</summary>
        public HHomMat2D HomMat2D = new HHomMat2D();
        [NonSerialized]
        private VisionCameraBase _camera;
        /// <summary>相机 </summary>
        public VisionCameraBase Camera
        {
            get
            {
                _camera = VisionModulesManager.CameraList.FirstOrDefault(c => c.CameraUserID == CameraUserID);
                return _camera;
            }
        }






        /// <summary>
        /// 初始化N点数据表
        /// </summary>
        /// <returns></returns>
        private DataTable InitNPointData()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", Type.GetType("System.Int32"));
            dt.Columns.Add("ImageR", Type.GetType("System.Double"));
            dt.Columns.Add("ImageC", Type.GetType("System.Double"));
            dt.Columns.Add("MachineX", Type.GetType("System.Double"));
            dt.Columns.Add("MachineY", Type.GetType("System.Double"));

            for (int i = 0; i < 14; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i + 1;
                dr[1] = 0;
                dr[2] = 0;

                if (i < 9)
                {
                    dr[3] = NPointParam.BaseX + NPointPathX[i] * NPointParam.OffsetX;
                    dr[4] = NPointParam.BaseY + NPointPathY[i] * NPointParam.OffsetY;
                    dt.Rows.Add(dr);
                }
                else if (IsRotateCenterCalib == false && i >= 9)
                {
                    dr[3] = NPointParam.BaseX;
                    dr[4] = NPointParam.BaseY;
                    dt.Rows.Add(dr);
                }


            }

            return dt;
        }





        public override void Execute(bool blnByHand = false)
        {
            base.Execute(blnByHand);
        }

        [OnDeserialized()]
        public void ItemNPointCalibDeSerialized(StreamingContext context)
        {

        }


    }
}
