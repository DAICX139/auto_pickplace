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
   public class ItemShowImage:Item
    {

        private ItemShowImage():base()
        {
        }

        public ItemShowImage(int itemID) : base(ItemType.显示图像, itemID)
        {
        }
    }
}
