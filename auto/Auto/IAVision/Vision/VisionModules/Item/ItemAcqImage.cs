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
    /// 采集图像项
    /// </summary>
    [Serializable]
    public class ItemAcqImage:Item
    {
        private ItemAcqImage()
        {
        }

        public ItemAcqImage(int itemID) : base(ItemType.采集图像, itemID)
        {
        }

        public string CameraUserID { get; set; }
        public string ImageName { get; set; }
        public AcqMode AcqMode { get; set; } = AcqMode.Camera;

        public string ImageWindowID { get; set; }






        public override void Execute(bool blnByHand = false)
        {
            base.Execute(blnByHand);
        }

    }
}
