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
using HalconDotNet;
using VisionControls;
using VisionUtility;

namespace VisionDemo
{
    public partial class FrmItemBase : FrmBase
    {
        protected Item item;
        //protected ImageWindow imageWindow = new ImageWindow();//继承会出现设计窗体显示异常，后续解决

        public FrmItemBase():base()
        {
            InitializeComponent();
        }

        public FrmItemBase(Item item):base()
        {
            InitializeComponent();
            this.item = item;
        }
        public virtual void IniImageWindow(Panel pnlImage)
        {
            //imageWindow.Dock = DockStyle.Fill;
            //imageWindow.Repaint += new Action(OnRepaint);
            //pnlImage.Controls.Add(imageWindow);
            //imageWindow.Show();
        }

        public virtual void OnRepaint()
        {
        }

    }
}
