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
using VisionUtility;

namespace VisionDemo
{
    public partial class FrmItemNPointCalib : FrmItemBase
    {
        protected ItemNPointCalib curItem = null;
        private FrmItemNPointCalib()
        {
            InitializeComponent();
        }

        public FrmItemNPointCalib(Item item) : base(item)
        {
            InitializeComponent();
            //
            curItem = item as ItemNPointCalib;
            IniCmbDevice();
            IniNPointParam();
            IniNPointResult();
            //
            IniDgv(dgvNPointData);
            IniNPointDataTable();

         

        }

        private void FrmItemNPointCalib_Load(object sender, EventArgs e)
        {
            dgvNPointData.DataSource = curItem.NPointData;
        }

        private void btnEditMode_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void IniCmbDevice()
        {
            cmbCameraUserID.DataSource = new BindingList<VisionCameraBase>(VisionModulesManager.CameraList);
            cmbCameraUserID.DisplayMember = "CameraUserID";
        }

        private void IniDgv(DataGridView dgv)
        {
            dgv.RowHeadersVisible = false;
            dgv.BorderStyle = BorderStyle.None;
            dgv.BackgroundColor = Color.White;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ScrollBars = ScrollBars.Vertical;
            dgv.Dock = DockStyle.Fill;
        }

        private void IniNPointDataTable()
        {
            curItem.NPointData.Columns[0].ColumnName = "序号";
            curItem.NPointData.Columns[1].ColumnName = "图像坐标R";
            curItem.NPointData.Columns[2].ColumnName = "图像坐标C";
            curItem.NPointData.Columns[3].ColumnName = "机械坐标X";
            curItem.NPointData.Columns[3].ColumnName = "机械坐标Y";
        }

        private void IniNPointParam()
        {
            NPointParam param = curItem.NPointParam;
            txtBaseX.Text = param.BaseX.ToString("0.000");
            txtBaseY.Text = param.BaseY.ToString("0.000");
            txtOffsetX.Text = param.OffsetX.ToString("0.000");
            txtOffsetY.Text = param.OffsetY.ToString("0.000");
            txtBaseAngle.Text = param.BaseAngle.ToString("0.000");
            //
            txtImageR.Text= curItem.ImageR.ToString("0.000");
            txtImageC.Text= curItem.ImageC.ToString("0.000");

            if (curItem.IsAutoCalib)
                rdbAuto.Checked = true;
            else
                rdbManual.Checked = true;

            if (curItem.InstallMode==InstallMode.Fix)
                rdbFix.Checked = true;
            else
                rdbMove.Checked = true;

            if (curItem.IsRotateCenterCalib)
                chkRotateCenter.Checked = true;
            else
                chkRotateCenter.Checked = false;
        }

        private void IniNPointResult()
        {
            NPointResult result = curItem.NPointResult;
            txtSx.Text = result.Sx.ToString("0.000");
            txtSy.Text = result.Sy.ToString("0.000");
            txtPhi.Text = result.Phi.ToString("0.000");
            txtTheta.Text = result.Theta.ToString("0.000");
            txtTx.Text = result.Tx.ToString("0.000");
            txtTy.Text = result.Ty.ToString("0.000");
            txtRr.Text = result.Rr.ToString("0.000");
            txtRc.Text = result.Rc.ToString("0.000");
        }




        private void cmbInputImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbCameraUserID.SelectedIndex;
            if (index < 0)
                return;

            curItem.CameraUserID = VisionModulesManager.CameraList[index].CameraUserID;
        }

        private void rdbAuto_CheckedChanged(object sender, EventArgs e)
        {
            //
            if (rdbAuto.Checked)
                curItem.IsAutoCalib = true;
            else
                curItem.IsAutoCalib = false;
            //
            if (rdbFix.Checked)
                curItem.InstallMode = InstallMode.Fix;
            else
                curItem.InstallMode = InstallMode.Move;
            //
            if (chkRotateCenter.Checked)
                curItem.IsRotateCenterCalib = true;
            else
                curItem.IsRotateCenterCalib = false;

        }
    }
}
