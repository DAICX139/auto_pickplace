using System;
using System.Threading;
using System.Windows.Forms;
using HalconDotNet;
using VisionControls;
using VisionUtility;
using VisionModules;

namespace VisionDemo
{
    public partial class FrmItemEditModel : FrmItemBase
    {
        private ImageWindow imageWindow = new ImageWindow();
        private HRegion oldMaskRegion = new HRegion();
        protected ItemModelMatch curItem = null;
        private FrmItemEditModel()
        {
            InitializeComponent();
        }
        public FrmItemEditModel(Item item) : base(item)
        {
            InitializeComponent();
            //
            curItem = item as ItemModelMatch;
            IniImageWindow(pnlImage);

        }
        private void FrmItemEditModel_Load(object sender, EventArgs e)
        {
            imageWindow.Image = curItem.ImageCrop;
            imageWindow.FitSize();
            oldMaskRegion = curItem.MaskRegion;
            IniModelParam();
            rdbNormal.Checked = true;

            curItem.CreateModeBaseMask();
            OnRepaint();
            timer.Start();
        }
        private void FrmItemEditModel_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        private void rdbNormal_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDraw.Checked)
                imageWindow.DrawMode = 0;
            else if (rdbErase.Checked)
                imageWindow.DrawMode = 1;
            else
                imageWindow.DrawMode = -1;
        }
        private void btnDraw_Click(object sender, EventArgs e)
        {
            curItem.MaskRegion = imageWindow.SetROI(curItem.MaskRegion);
            OnRepaint();
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            curItem.MaskRegion = new HRegion((double)0, 0, 0);
            OnRepaint();
        }
        public override void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                curItem.ModelParam.Contrast = Convert.ToInt32(txtContrast.Text);
                curItem.ModelParam.AngleStart = Convert.ToDouble(txtAngleStart.Text);
                curItem.ModelParam.AngleExtent = Convert.ToDouble(txtAngleExtent.Text);
                curItem.ModelParam.ScaleMin = Convert.ToDouble(txtScaleMin.Text);
                curItem.ModelParam.ScaleMax = Convert.ToDouble(txtScaleMax.Text);

                curItem.CreateModeBaseMask();
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
            curItem.MaskRegion = oldMaskRegion;
        }
        public override void IniImageWindow(Panel pnlImage)
        {
            imageWindow.Dock = DockStyle.Fill;
            imageWindow.Repaint += new Action(OnRepaint);
            pnlImage.Controls.Add(imageWindow);
            imageWindow.Show();
        }
        private void IniModelParam()
        {
            ModelParam modelParam = curItem.ModelParam;
            txtContrast.Text = modelParam.Contrast.ToString();
            txtAngleStart.Text = modelParam.AngleStart.ToString("0.000");
            txtAngleExtent.Text = modelParam.AngleExtent.ToString("0.000");
            txtScaleMin.Text = modelParam.ScaleMin.ToString("0.000");
            txtScaleMax.Text = modelParam.ScaleMax.ToString("0.000");
        }
        public override void timer_Tick(object sender, EventArgs e)
        {
            btnDraw.Enabled = !imageWindow.IsDrawing;
            btnClear.Enabled = !imageWindow.IsDrawing;
            btnExecute.Enabled = !imageWindow.IsDrawing;
            btnOk.Enabled = !imageWindow.IsDrawing;
            btnCancel.Enabled = !imageWindow.IsDrawing;
        }
        public override void OnRepaint()
        {
            if (!imageWindow.IsDrawing)
            {
                imageWindow.Image = curItem.ImageCrop;
                imageWindow.HWindow.SetDraw("fill");
                imageWindow.HWindow.SetColor("red");
                curItem.MaskRegion.DispObj(imageWindow.HWindow);
            }

            if (curItem.ModelRegion != null && curItem.ModelRegion.IsInitialized())
            {
                imageWindow.HWindow.SetDraw("margin");
                imageWindow.HWindow.SetColor("green");
                curItem.ModelRegion.DispObj(imageWindow.HWindow);
            }
        }
    }
}
