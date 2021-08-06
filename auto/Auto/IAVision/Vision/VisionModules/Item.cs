using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionUtility;
using System.Runtime.Serialization;

namespace VisionModules
{
    /// <summary>
    /// 视觉模块基本单元
    /// </summary>
    [Serializable]
    public class Item : ICloneable
    {
        public Item()
        {
        }

        public Item(ItemType itemType, int itemID)
        {
            ItemType = itemType;
            ItemID = itemID;
            Owner = VisionModulesManager.CurrFlow;
        }

        private Flow _owner = null;
        [NonSerialized]
        private bool _isSuccessed = false;

        [NonSerialized]
        protected HImage _image = new HImage();

        /// <summary> 视觉模块基本单元类型</summary>
        public ItemType ItemType { get; set; }
        /// <summary>视觉模块基本单元ID（所在项目）</summary>
        public int ItemID { get; set; }

        /// <summary> 视觉模块基本单元描述/// </summary>
        public string ItemRemark { get; set; }

        /// <summary>是否屏蔽不执行 </summary>
        public bool IsShield { get; set; }

        /// <summary>
        /// 当前单元是否执行成功
        /// </summary>
        public bool IsSuccessed
        {
            get { return _isSuccessed; }
            set { _isSuccessed = value; }
        }

        public HImage Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public Flow Owner
        {
            set
            {
                if (value == null)
                    return;

                _owner = value;
                int index = _owner.ItemList.FindIndex(c => c.ItemID == ItemID);

                if (index < 0)
                {
                    _owner.ItemList.Add(this);
                    _owner.LastItemID++;
                }
                else
                {
                    _owner.ItemList[index] = this;
                }
            }
            get
            {
                return _owner;
            }
        }



        public virtual void Execute(bool blnByHand = false)
        {
        }

        public object Clone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(stream, this);
            stream.Seek(0, SeekOrigin.Begin);
            //stream.Position = 0;
            Item item = (Item)formatter.Deserialize(stream);

            return item;
        }

        [OnDeserializing()]
        public void ItemDeSerializing(StreamingContext context)
        {
            _image = new HImage();
        }
    }
}
