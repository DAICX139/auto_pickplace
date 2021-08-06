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
using System.Diagnostics;

namespace VisionDemo
{
    public partial class FrmItemModelMatch : FrmItemBase
    {
        protected ImageWindow imageWindow = new ImageWindow();
        protected ItemModelMatch curItem = null;
        private FrmItemModelMatch()
        {
            InitializeComponent();
        }
        public FrmItemModelMatch(Item item) : base(item)
        {
            InitializeComponent();
            //
            curItem = item as ItemModelMatch;
            IniImageWindow(pnlImage);
        }

        private void FrmItemTemplateMatch_Load(object sender, EventArgs e)
        {
            curItem.Image = VisionModulesManager.CurrFlow.ItemList.First(c => c.ItemType == ItemType.采集图像).Image;
            imageWindow.Image = curItem.Image;
            imageWindow.FitSize();

            imageWindow.DrawingObject(curItem.DrawingObject);
            IniModelParam();
            cmbInputImage.Text = ItemType.采集图像.ToString();
            timer.Start();
        }

        private void FrmItemModelMatch_FormClosing(object sender, FormClosingEventArgs e)
        {
            imageWindow.HWindow.DetachDrawingObjectFromWindow(curItem.DrawingObject);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                curItem.CreateModeBaseRoi();
                curItem.ModelRegion.DispObj(imageWindow.HWindow);
                OnRepaint();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            FrmItemEditModel frm = new FrmItemEditModel(item);
            frm.ShowDialog();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                curItem.ReadShapeModel();
                btnExecute_Click(sender, e);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //后续优化
                double row = (curItem.Roi.Row1 + curItem.Roi.Row2) / 2.0 - curItem.ResultData.Row;
                double column = (curItem.Roi.Column1 + curItem.Roi.Column2) / 2.0 - curItem.ResultData.Column;
                curItem.ShapeModel.SetShapeModelOrigin(row, column);
                curItem.WriteShapeModel();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public override void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                curItem.ModelParam.MinScore = Convert.ToDouble(txtMinScore.Text);
                curItem.ModelParam.NumMatches = Convert.ToInt32(txtNumMatches.Text);
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

        public override void IniImageWindow(Panel pnlImage)
        {
            imageWindow.Dock = DockStyle.Fill;
            imageWindow.Repaint += new Action(OnRepaint);
            pnlImage.Controls.Add(imageWindow);
            imageWindow.Show();
        }

        private void IniModelParam()
        {
            try
            {
                txtMinScore.Text = curItem.ModelParam.MinScore.ToString("0.000");
                txtNumMatches.Text = curItem.ModelParam.NumMatches.ToString();
            }
            catch (Exception ex)
            {
            }
        }
        public override void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                txtRow1.Text = curItem.Roi.Row1.ToString("0.000");
                txtColumn1.Text = curItem.Roi.Column1.ToString("0.000");
                txtRow2.Text = curItem.Roi.Row2.ToString("0.000");
                txtColumn2.Text = curItem.Roi.Column2.ToString("0.000");

                txtResultRow.Text = curItem.ResultData.Row.ToString("0.000");
                txtResultColumn.Text = curItem.ResultData.Column.ToString("0.000");
                txtResultAngle.Text = curItem.ResultData.Angle.ToString("0.000");
                txtResultScore.Text = curItem.ResultData.Score.ToString("0.000");
            }
            catch (Exception ex)
            {
            }
        }

        public override void OnRepaint()
        {
            imageWindow.Image = curItem.Image;
            if (curItem.ShapeModelContourTrans != null && curItem.ShapeModelContourTrans.IsInitialized())
            {
                imageWindow.HWindow.SetDraw("margin");
                imageWindow.HWindow.SetColor("green");
                curItem.ShapeModelContourTrans.DispObj(imageWindow.HWindow);
            }
        }


    }
}
