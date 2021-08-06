using System;
using System.Linq;
using System.Windows.Forms;
using VisionModules;
using HalconDotNet;
using VisionControls;
using VisionUtility;

namespace VisionDemo
{
    public partial class FrmItemCreateRoi : FrmItemBase
    {
        protected ImageWindow imageWindow = new ImageWindow();
        protected ItemCreateRoi curItem=null;
        private FrmItemCreateRoi()
        {
            InitializeComponent();
        }
        public FrmItemCreateRoi(Item item):base(item)
        {
            InitializeComponent();
            //
            curItem = item as ItemCreateRoi;
            IniImageWindow(pnlImage);
          
        }
        private void FrmItemCreateRoi_Load(object sender, EventArgs e)
        {
            curItem.Image = VisionModulesManager.CurrFlow.ItemList.First(c => c.ItemType == ItemType.采集图像).Image;
            imageWindow.Image = curItem.Image;
            imageWindow.FitSize();

            imageWindow.DrawingObject(curItem.DrawingObject);
            timer.Start();
        }
        private void FrmItemCreateRoi_FormClosing(object sender, FormClosingEventArgs e)
        {
            imageWindow.HWindow.DetachDrawingObjectFromWindow(curItem.DrawingObject);
        }
        public override void IniImageWindow(Panel pnlImage)
        {
            imageWindow.Dock = DockStyle.Fill;
            imageWindow.Repaint += new Action(OnRepaint);
            pnlImage.Controls.Add(imageWindow);
            imageWindow.Show();
        }
        public override void timer_Tick(object sender, EventArgs e)
        {
            Rectangle2Roi roi = curItem.Roi;

            txtRow.Text = roi.Row.ToString("0.000");
            txtColumn.Text = roi.Column.ToString("0.000");
            txtPhi.Text = roi.Phi.ToString("0.000");
            txtLength1.Text = roi.Length1.ToString("0.000");
            txtLength2.Text = roi.Length2.ToString("0.000");
        }
    }
}
