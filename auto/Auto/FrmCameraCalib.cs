using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionModules;
using VisionUtility;

namespace VisionFlows
{
    public partial class FrmCameraCalib : Form
    {
        private Thread threadNPointCalib;//相机标定

        public FrmCameraCalib() : base()
        {
            InitializeComponent();
        }

        private void FrmCameraCalib_Load(object sender, EventArgs e)
        {
            IniCmbDevice(cmbCameraUserID);
            IniDgv(dgvCalibData);
        }

        private void cmbCameraUserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbCameraUserID.SelectedIndex;
            if (index < 0) return;
            //
            IniCameraCalibParam(index);
            IniCameraCalibResult(index);
            UpdateCalibDataTable(index);
        }

        private void btnGoBaseX_Click(object sender, EventArgs e)
        {
            var id = (int)((EnumAxis)Enum.Parse(typeof(EnumAxis), txtXAxis.Text));
            var pos = double.Parse(txtBaseX.Text);
            Plc.AxisAbsGo(id, pos, 50);
        }

        private void btnGoBaseY_Click(object sender, EventArgs e)
        {
            var id = (int)((EnumAxis)Enum.Parse(typeof(EnumAxis), txtYAxis.Text));
            var pos = double.Parse(txtBaseY.Text);
            Plc.AxisAbsGo(id, pos, 50);
        }

        private void btnGoBaseZ_Click(object sender, EventArgs e)
        {
            var id = (int)((EnumAxis)Enum.Parse(typeof(EnumAxis), txtZAxis.Text));
            var pos = double.Parse(txtBaseZ.Text);
            Plc.AxisAbsGo(id, pos, 50);
        }
        private void btnGoBaseR_Click(object sender, EventArgs e)
        {
            var id = (int)((EnumAxis)Enum.Parse(typeof(EnumAxis), txtRAxis.Text));
            var pos = double.Parse(txtBaseR.Text);
            Plc.AxisAbsGo(id, pos, 50);
        }

        private void btnGoOffsetX_Click(object sender, EventArgs e)
        {
            var id = (int)((EnumAxis)Enum.Parse(typeof(EnumAxis), txtXAxis.Text));
            var dis = double.Parse(txtOffsetX.Text);
            Plc.AxisRelGo(id, dis, 50);
        }

        private void btnGoOffsetY_Click(object sender, EventArgs e)
        {
            var id = (int)((EnumAxis)Enum.Parse(typeof(EnumAxis), txtYAxis.Text));
            var dis = double.Parse(txtOffsetY.Text);
            Plc.AxisRelGo(id, dis, 50);
        }

        
        private void btnGoOffsetZ_Click(object sender, EventArgs e)
        {
            var id = (int)((EnumAxis)Enum.Parse(typeof(EnumAxis), txtZAxis.Text));
            var dis = double.Parse(txtOffsetZ.Text);
            Plc.AxisRelGo(id, dis, 50);
        }

        private void btnGoOffsetR_Click(object sender, EventArgs e)
        {
            var id = (int)((EnumAxis)Enum.Parse(typeof(EnumAxis), txtRAxis.Text));
            var dis = double.Parse(txtOffsetR.Text);
            Plc.AxisRelGo(id, dis, 50);
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            try
            {
                int index = cmbCameraUserID.SelectedIndex;
                if (index < 0) return;
                //
                UpdateCameraCalibParam(index);
                //Task t = new Task(new Action<object>(NPointCalib.Execute),index);
                //t.Start();
                if (threadNPointCalib != null && threadNPointCalib.IsAlive)
                {
                    threadNPointCalib.Abort();
                    Thread.Sleep(20);
                }         
                threadNPointCalib = new Thread(new ParameterizedThreadStart(CameraCalib.Execute));
                threadNPointCalib.IsBackground = true;//
                CameraCalib.CameraCalibCancel = false;
                threadNPointCalib.Start(index);
            }
            catch (Exception ex)
            {
                Flow.Log(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            int index = cmbCameraUserID.SelectedIndex;
            if (index < 0) return;
            UpdateCameraCalibParam(index);
            Flow.XmlHelper.SerializeToXml(Utility.Config + "CameraCalib.xml", CameraCalib.CameraCalibData);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //NPointCalib.NPointCalibData = (NPointCalibData)Flow.XmlHelper.DeserializeFromXml(Utility.configPath + "NPointCalib.xml", typeof(NPointCalibData));
            CameraCalib.CameraCalibCancel = true;
        }

        private void IniCmbDevice(ComboBox cmb)
        {
            //cmb.Items.AddRange(new string[] { "LoadDownLook", "LoadUpLook", "UnloadDownLook", "UnloadUpLook" });
            cmbCameraUserID.DataSource = new BindingList<VisionCameraBase>(VisionModulesManager.CameraList);
            cmbCameraUserID.DisplayMember = "CameraUserID";
        }
        private void IniCameraCalibParam(int index)
        {
            CameraCalibPara para = CameraCalib.CameraCalibData.CameraCalibParaList[index];
            txtLightID.Text= ((EnumLight)para.LightID).ToString();
            txtXAxis.Text = ((EnumAxis)para.XAxisID).ToString();
            txtYAxis.Text = ((EnumAxis)para.YAxisID).ToString();
            txtZAxis.Text = ((EnumAxis)para.ZAxisID).ToString();
            txtRAxis.Text = ((EnumAxis)para.RAxisID).ToString();
            //
            txtBaseX.Text = para.BaseX.ToString("0.000");
            txtBaseY.Text = para.BaseY.ToString("0.000");
            txtBaseZ.Text = para.BaseZ.ToString("0.000");
            txtBaseR.Text = para.BaseR.ToString("0.000");
            txtOffsetX.Text = para.OffsetX.ToString("0.000");
            txtOffsetY.Text = para.OffsetY.ToString("0.000");
            txtOffsetZ.Text = para.OffsetZ.ToString("0.000");
            txtOffsetR.Text = para.OffsetR.ToString("0.000");
            //
            txtExposureTime.Text = para.ExposureTime.ToString("0.000");
            txtGain.Text = para.Gain.ToString("0.000");
            txtMatchMinScore.Text = para.ShapeModelMinScore.ToString("0.000");
            txtMetroMinScore.Text = para.MetrologyModelMinScore.ToString("0.000");
        }
        private void UpdateCameraCalibParam(int index)
        {
            CameraCalibPara para = CameraCalib.CameraCalibData.CameraCalibParaList[index];
            //
            para.LightID = (int)Enum.Parse(typeof(EnumLight), txtLightID.Text); ;
            para.XAxisID = (int)Enum.Parse(typeof(EnumAxis), txtXAxis.Text);
            para.YAxisID = (int)Enum.Parse(typeof(EnumAxis), txtYAxis.Text);
            para.ZAxisID = (int)Enum.Parse(typeof(EnumAxis), txtZAxis.Text);
            para.RAxisID = (int)Enum.Parse(typeof(EnumAxis), txtRAxis.Text);
            //
            para.BaseX = Convert.ToDouble(txtBaseX.Text);
            para.BaseY = Convert.ToDouble(txtBaseY.Text);
            para.BaseZ = Convert.ToDouble(txtBaseZ.Text);
            para.BaseR = Convert.ToDouble(txtBaseR.Text);
            para.OffsetX = Convert.ToDouble(txtOffsetX.Text);
            para.OffsetY = Convert.ToDouble(txtOffsetY.Text);
            para.OffsetZ = Convert.ToDouble(txtOffsetZ.Text);
            para.OffsetR = Convert.ToDouble(txtOffsetR.Text);
            //
            para.ExposureTime= Convert.ToDouble(txtExposureTime.Text);
            para.Gain= Convert.ToDouble(txtGain.Text);
            para.ShapeModelMinScore = Convert.ToDouble(txtMatchMinScore.Text);
            para.MetrologyModelMinScore = Convert.ToDouble(txtMetroMinScore.Text);
        }
        private void IniCameraCalibResult(int index)
        {
            CameraCalibResult result = CameraCalib.CameraCalibData.CameraCalibResultList[index];
            txtSx.Text = result.Sx.ToString("0.0000");
            txtSy.Text = result.Sy.ToString("0.0000");
            txtPhi.Text = result.Phi.ToString("0.0000");
            txtTheta.Text = result.Theta.ToString("0.0000");
            txtTx.Text = result.Tx.ToString("0.0000");
            txtTy.Text = result.Ty.ToString("0.0000");
            txtRms.Text = result.Rms.ToString("0.0000");
            txtSxyError.Text = result.SxyError.ToString("0.0000");
            txtRmsError.Text = result.RmsError.ToString("0.0000"); 
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

        public void UpdateCalibDataTable(int index)
        {
            IniCameraCalibResult(index);
            dgvCalibData.DataSource = new BindingList<CameraCalibCoordi>(CameraCalib.CameraCalibData.CameraCalibResultList[index].CameraCalibCoordiList);
        }
    }
}
