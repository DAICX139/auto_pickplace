using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionModules;

namespace VisionDemo
{
    public partial class FrmItemSaveImage : FrmItemBase
    {
        protected ItemSaveImage curItem = null;
        private FrmItemSaveImage()
        {
            InitializeComponent();
        }

        public FrmItemSaveImage(Item item):base(item)
        {
            InitializeComponent();
            //
            curItem = item as ItemSaveImage;
    }
    }
}
