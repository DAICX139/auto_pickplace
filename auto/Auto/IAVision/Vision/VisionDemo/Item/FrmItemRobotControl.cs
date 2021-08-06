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
    public partial class FrmItemRobotControl : FrmItemBase
    {
        protected ItemRobotControl curItem = null;
        private FrmItemRobotControl()
        {
            InitializeComponent();
        }

        public FrmItemRobotControl(Item item):base(item)
        {
            InitializeComponent();
            //
            curItem = item as ItemRobotControl;
        }
    }
}
