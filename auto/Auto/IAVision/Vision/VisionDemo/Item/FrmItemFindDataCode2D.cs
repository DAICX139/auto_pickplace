using System;
using System.Linq;
using System.Windows.Forms;
using VisionModules;
using VisionControls;
using VisionUtility;

namespace VisionDemo
{
    public partial class FrmItemFindDataCode2D : FrmItemBase
    {
        protected ImageWindow imageWindow = new ImageWindow();
        protected ItemFindDataCode2D curItem=null;
        private FrmItemFindDataCode2D()
        {
            InitializeComponent();
        }

        public FrmItemFindDataCode2D(Item item) : base(item)
        {
            InitializeComponent();
            //
            curItem = item as ItemFindDataCode2D;
            IniImageWindow(pnlImage);
       
        }
        private void FrmItemFindDataCode2D_Load(object sender, EventArgs e)
        {
            curItem.Image = VisionModulesManager.CurrFlow.ItemList.FirstOrDefault(c => c.ItemType == ItemType.采集图像).Image;
            imageWindow.Image = curItem.Image;
            imageWindow.FitSize();

            imageWindow.DrawingObject(curItem.DrawingObject);
            IniModelParam();
            cmbInputImage.Text = ItemType.采集图像.ToString();
            cmbSymbolType.Text = curItem.SymbolType;
            txtPath.Text = curItem.TrainFolderName;
            timer.Start();
        }

        private void FrmItemFindDataCode2D_FormClosing(object sender, FormClosingEventArgs e)
        {
            imageWindow.HWindow.DetachDrawingObjectFromWindow(curItem.DrawingObject);
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = Application.StartupPath;
            fbd.ShowNewFolderButton = false;
            if (fbd.ShowDialog() == DialogResult.OK)
                txtPath.Text = fbd.SelectedPath;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                curItem.SymbolType = cmbSymbolType.Text;
                curItem.TrainFolderName = txtPath.Text;
                curItem.TrainDataCode2DModel(txtPath.Text);
            }
            catch (Exception ex)
            {
            }
        }

        public override void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
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
            //ModelParam modelParam = ((ItemFindDataCode2D)item).ModelParam;

            //txtMinScore.Text = modelParam.MinScore.ToString("0.000");
            //txtNumMatches.Text = modelParam.NumMatches.ToString();
        }

        public override void timer_Tick(object sender, EventArgs e)
        {
            Rectangle2Roi roi = curItem.Roi;

            txtRow.Text = roi.Row.ToString("0.000");
            txtColumn.Text = roi.Column.ToString("0.000");
            txtPhi.Text = roi.Phi.ToString("0.000");
            txtLength1.Text = roi.Length1.ToString("0.000");
            txtLength2.Text = roi.Length2.ToString("0.000");

            txtDataCodeString.Text = ((ItemFindDataCode2D)item).DecodedDataStrings;
        }
       public override void OnRepaint()
        {
            imageWindow.Image = curItem.Image;
            if (curItem.SymbolXLD != null && curItem.SymbolXLD.IsInitialized())
            {
                imageWindow.HWindow.SetDraw("margin");
                imageWindow.HWindow.SetColor("green");
                curItem.SymbolXLD.DispObj(imageWindow.HWindow);
            }
        }
    }
}
