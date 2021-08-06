using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using VisionControls;
using VisionUtility;
using VisionModules;

namespace VisionDemo
{
    public partial class FrmItemOcr : FrmItemBase
    {
        protected ImageWindow imageWindow = new ImageWindow();
        protected ItemOcr curItem = null;
        private FrmItemOcr():base()
        {
            InitializeComponent();
        }

        public  FrmItemOcr(Item item) : base(item)
        {
            InitializeComponent();
            //
            curItem = item as ItemOcr;
        }
    }
}
