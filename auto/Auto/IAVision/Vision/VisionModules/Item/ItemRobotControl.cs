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
  public class ItemRobotControl:Item
    {
        private ItemRobotControl():base()
        {
        }

        public ItemRobotControl(int itemID) : base(ItemType.机械手控制, itemID)
        {
        }

        public override void Execute(bool blnByHand = false)
        {
            base.Execute(blnByHand);
        }

        [OnSerializing()]
        public void ItemMeasureCircleSerializing(StreamingContext context)
        {
        }
        [OnDeserializing()]
        public void ItemRobotControlDeSerializing(StreamingContext context)
        {
        }
        [OnDeserialized()]
        public void ItemRobotControlDeSerialized(StreamingContext context)
        {
        }

    }
}
