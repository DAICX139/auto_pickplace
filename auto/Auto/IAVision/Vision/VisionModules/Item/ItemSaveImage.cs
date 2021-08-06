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
    public  class ItemSaveImage:Item
    {
        private ItemSaveImage():base()
        {
        }

        public ItemSaveImage(int itemID) : base(ItemType.存储图像, itemID)
        {
        }
    }
}
