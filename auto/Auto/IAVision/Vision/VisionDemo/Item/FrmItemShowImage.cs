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
    public partial class FrmItemShowImage : FrmItemBase
    {
        protected ItemShowImage curItem = null;
        private FrmItemShowImage()
        {
            InitializeComponent();
        }

        public FrmItemShowImage(Item item):base(item)
        {
            InitializeComponent();
            //
            curItem = item as ItemShowImage;
        }
    }
}
