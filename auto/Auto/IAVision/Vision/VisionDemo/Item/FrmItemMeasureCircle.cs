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
using VisionModules;
using VisionControls;
using VisionUtility;

namespace VisionDemo
{
    public partial class FrmItemMeasureCircle :   FrmItemBase
    {
        protected ImageWindow imageWindow = new ImageWindow();
        protected ItemMeasureCircle curItem = new ItemMeasureCircle();

        private FrmItemMeasureCircle()
        {
            InitializeComponent();
        }
        public FrmItemMeasureCircle(Item item):base(item)
        {
            InitializeComponent();
            //
            curItem = item as ItemMeasureCircle;
            IniImageWindow(pnlImage);
            
        }
        private void FrmItemCircleMeasure_Load(object sender, EventArgs e)
        {
            curItem.Image = VisionModulesManager.CurrFlow.ItemList.First(c => c.ItemType == ItemType.采集图像).Image;
            imageWindow.Image = curItem.Image;
            imageWindow.FitSize();
            imageWindow.DrawingObject(curItem.DrawingObject);
            IniMeasureParam();
            timer.Start();
        }

        private void FrmItemCircleMeasure_FormClosing(object sender, FormClosingEventArgs e)
        {
            imageWindow.HWindow.DetachDrawingObjectFromWindow(((ItemMeasureCircle)item).DrawingObject);
        }
   
        public override void btnExecute_Click(object sender, EventArgs e)
        {
            base.btnExecute_Click(sender, e);

            try
            {
                curItem.MeasureParam.MeasureLength1 = Convert.ToDouble(txtMeasureLength1.Text);
                curItem.MeasureParam.MeasureLength2 = Convert.ToDouble(txtMeasureLength2.Text);
                curItem.MeasureParam.MeasureThreshold = Convert.ToDouble(txtMeasureThreshold.Text);
                curItem.MeasureParam.MeasureDistance = Convert.ToDouble(txtMeasureDistance.Text);
                curItem.MeasureParam.MeasureTransition = cboMeasureTransition.Text.ToString();
                curItem.MeasureParam.MeasureSelect = cboMeasureSelect.Text.ToString();

                curItem.Execute();

                OnRepaint();
            }
            catch (Exception ex)
            {

            }
        }

        public override void btnOk_Click(object sender, EventArgs e)
        {
            base.btnOk_Click(sender, e);
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            base.btnCancel_Click(sender, e);
        }

        public  override  void IniImageWindow(Panel pnlImage)
        {
            imageWindow.Dock = DockStyle.Fill;
            imageWindow.Repaint += new Action(OnRepaint);
            pnlImage.Controls.Add(imageWindow);
            imageWindow.Show();
        }

        private void IniMeasureParam()
        {
            MeasureParam measureParam = curItem.MeasureParam;

            txtMeasureLength1.Text= measureParam.MeasureLength1.ToString("0.000");
            txtMeasureLength2.Text = measureParam.MeasureLength2.ToString("0.000");
            txtMeasureThreshold.Text = measureParam.MeasureThreshold.ToString("0.000");
            txtMeasureDistance.Text = measureParam.MeasureDistance.ToString("0.000");
            cboMeasureTransition.Text = measureParam.MeasureTransition;
            cboMeasureSelect.Text = measureParam.MeasureSelect;
        }

        public override void timer_Tick(object sender, EventArgs e)
        {
            CircleRoi roi = curItem.Roi;
            txtRow.Text = roi.Row.ToString("0.000");
            txtColumn.Text = roi.Column.ToString("0.000");
            txtRadius.Text = roi.Radius.ToString("0.000");

            CircleRoi result = curItem.Result;
            txtResultRow.Text = result.Row.ToString("0.000");
            txtResultColumn.Text = result.Column.ToString("0.000");
            txtResultRadius.Text = result.Radius.ToString("0.000");
        }

        public override void OnRepaint()
        {
            imageWindow.Image = curItem.Image;
            if (curItem.MeasureObjectContour != null && curItem.MeasureObjectContour.IsInitialized())
            {
                imageWindow.HWindow.SetDraw("margin");
                imageWindow.HWindow.SetColor("cyan");
                curItem.MeasureObjectContour.DispObj(imageWindow.HWindow);
            }

            if (curItem.MeasureResultContour != null && curItem.MeasureResultContour.IsInitialized())
            {
                imageWindow.HWindow.SetDraw("margin");
                imageWindow.HWindow.SetColor("green");
                curItem.MeasureResultContour.DispObj(imageWindow.HWindow);
            }

            if (curItem.MeasurePointCross != null && curItem.MeasurePointCross.IsInitialized())
            {
                imageWindow.HWindow.SetDraw("margin");
                imageWindow.HWindow.SetColor("yellow");
                curItem.MeasurePointCross.DispObj(imageWindow.HWindow);
            }

            if (curItem.MeasureCenterCross != null && curItem.MeasureCenterCross.IsInitialized())
            {
                imageWindow.HWindow.SetDraw("margin");
                imageWindow.HWindow.SetColor("yellow");
                curItem.MeasureCenterCross.DispObj(imageWindow.HWindow);
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}
