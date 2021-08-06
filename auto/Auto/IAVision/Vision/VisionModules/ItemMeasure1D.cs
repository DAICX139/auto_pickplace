using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionUtility;

namespace VisionModules
{
  public class ItemMeasure1D:Item
    {
        private ItemMeasure1D()
        {
        }

        protected ItemMeasure1D(ItemType itemType,int itemID) : base(ItemType.采集图像, itemID)
        {
        }
    }
}
