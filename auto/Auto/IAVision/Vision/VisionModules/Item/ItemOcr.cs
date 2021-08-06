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
    public class ItemOcr:Item
    {
        public ItemOcr() : base()
        {
        }

        public ItemOcr(int itemID) : base(ItemType.字符识别, itemID)
        {
        }
        public override void Execute(bool blnByHand = false)
        {
            base.Execute(blnByHand);
        }

        [OnSerializing()]
        public void ItemOcrSerializing(StreamingContext context)
        {
        }

        [OnDeserializing()]
        public void ItemOcrDeSerializing(StreamingContext context)
        {
        }



        [OnDeserialized()]
        public void ItemOcrDeSerialized(StreamingContext context)
        {

        }

    }
}
