using System;
using System.ComponentModel;
using System.Windows.Forms;
using VisionFlows.VisionCalculate;

namespace VisionFlows
{
    public partial class FrmAutoNormal : Form
    {
        public FrmAutoNormal()
        {
            InitializeComponent();
        }

        private void FrmAutoNormal_Load(object sender, EventArgs e)
        {
            InitCmbDevice();
            //
        }

        private void cmbWorkID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbWorkID.SelectedIndex;
            if (index < 0) return;
            ShowPosiParam(index);
            ShowWorkDataTable(index);
        }

        private void cmbAlgoID_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbAlgoID.SelectedIndex;
            if (index < 0) return;
            ShowAlgoParam(index);
        }

        private void btnBaseX_Click(object sender, EventArgs e)
        {
            var id = (int)((EnumAxis)Enum.Parse(typeof(EnumAxis), txtXAxis.Text));
            var pos = double.Parse(txtBaseX.Text);
            Plc.AxisAbsGo(id, pos, 50);
        }

        private void btnBaseY_Click(object sender, EventArgs e)
        {
            var id = (int)((EnumAxis)Enum.Parse(typeof(EnumAxis), txtYAxis.Text));
            var pos = double.Parse(txtBaseY.Text);
            Plc.AxisAbsGo(id, pos, 50);
        }

        private void btnBaseZ_Click(object sender, EventArgs e)
        {
            var id = (int)((EnumAxis)Enum.Parse(typeof(EnumAxis), txtZAxis.Text));
            var pos = double.Parse(txtBaseZ.Text);
            Plc.AxisAbsGo(id, pos, 50);
        }

        private void btnBaseR_Click(object sender, EventArgs e)
        {
            var id = (int)((EnumAxis)Enum.Parse(typeof(EnumAxis), txtRAxis.Text));
            var pos = double.Parse(txtBaseR.Text);
            Plc.AxisAbsGo(id, pos, 50);
        }

        private void btnOffsetX_Click(object sender, EventArgs e)
        {
            var id = (int)((EnumAxis)Enum.Parse(typeof(EnumAxis), txtXAxis.Text));
            var dis = double.Parse(txtOffsetX.Text);
            Plc.AxisRelGo(id, dis, 50);
        }

        private void btnOffsetY_Click(object sender, EventArgs e)
        {
            var id = (int)((EnumAxis)Enum.Parse(typeof(EnumAxis), txtYAxis.Text));
            var dis = double.Parse(txtOffsetY.Text);
            Plc.AxisRelGo(id, dis, 50);
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            int index = cmbWorkID.SelectedIndex;
            if (index < 0) return;
            //
            var indexWork = cmbWorkID.SelectedIndex;
            var indexAlgo = cmbAlgoID.SelectedIndex;
            if (indexWork < 0 || indexAlgo < 0) return;
            UpdatePosiParam(indexWork, indexAlgo);
            //
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var indexWork = cmbWorkID.SelectedIndex;
            var indexAlgo = cmbAlgoID.SelectedIndex;
            if (indexWork < 0 || indexAlgo < 0) return;
            UpdatePosiParam(indexWork, indexAlgo);
            Flow.XmlHelper.SerializeToXml(Utility.Config + "AcqPosi.xml", AcqPosiData.Instance);
            Flow.XmlHelper.SerializeToXml(Utility.Config + "Algorithm.xml", AlgorithmData.Instance);
            Flow.XmlHelper.SerializeToXml(Utility.Config + "AutoNormal.xml", AutoNormalData.Instance);

            MessageBox.Show("Save To AutoNormal.xml");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region""

        private void InitCmbDevice()
        {
            cmbWorkID.Items.AddRange(Enum.GetNames(typeof(EnumAutoNormal)));
            cmbWorkID.Items.Remove("None");
            cmbAlgoID.Items.AddRange(Enum.GetNames(typeof(EnumAlgorithm)));
            cmbAlgoID.Items.Remove("None");
            cmbWorkID.SelectedIndex = 0;
        }

        private void ShowPosiParam(int index)
        {
            AutoNormalPara para = AutoNormalData.Instance.AutoNormalParaList[index];
            var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
            //
            txtCompX.Text = para.CompensateX.ToString("0.000");
            txtCompY.Text = para.CompensateY.ToString("0.000");
            txtCompR.Text = para.CompensateR.ToString("0.000");
            //
            txtXAxis.Text = ((EnumAxis)posi.XAxisID).ToString();
            txtYAxis.Text = ((EnumAxis)posi.YAxisID).ToString();
            txtZAxis.Text = ((EnumAxis)posi.ZAxisID).ToString();
            txtRAxis.Text = ((EnumAxis)posi.RAxisID).ToString();
            //
            txtBaseX.Text = posi.BaseX.ToString("0.000");
            txtBaseY.Text = posi.BaseY.ToString("0.000");
            txtBaseZ.Text = posi.BaseZ.ToString("0.000");
            txtBaseR.Text = posi.BaseR.ToString("0.000");
            txtOffsetX.Text = posi.OffsetX.ToString("0.000");
            txtOffsetY.Text = posi.OffsetY.ToString("0.000");

            //
            cmbAlgoID.SelectedIndex = para.AlgorithmID;
            ShowAlgoParam(index);
        }

        private void ShowAlgoParam(int index)
        {
            AutoNormalPara para = AutoNormalData.Instance.AutoNormalParaList[index];
            var algo = AlgorithmData.Instance.AlgorithmParaList[para.AlgorithmID];
            //
            txtExposureTime.Text = algo.ExposureTime.ToString("0.000");
            txtGain.Text = algo.Gain.ToString("0.000");
            txtMinScore.Text = algo.MatchMinScore.ToString("0.000");
            //txtNumMatches.Text = para.NumMatches.ToString("0.000");
            //txtShapeModelFileName.Text = para.ShapeModelFileName;
        }

        private void UpdatePosiParam(int indexWork, int indexAlgo)
        {
            try
            {
                AutoNormalPara para = AutoNormalData.Instance.AutoNormalParaList[indexWork];
                para.PosiID = (int)Enum.Parse(typeof(EnumAcqPosi), ((EnumAutoNormal)indexWork).ToString());
                para.AlgorithmID = indexAlgo;
                var posi = AcqPosiData.Instance.AcqPosiParaList[para.PosiID];
                var algo = AlgorithmData.Instance.AlgorithmParaList[para.AlgorithmID];
                //
                para.CompensateX = Convert.ToDouble(txtCompX.Text);
                para.CompensateY = Convert.ToDouble(txtCompY.Text);
                para.CompensateR = Convert.ToDouble(txtCompR.Text);
                //
                posi.XAxisID = (int)Enum.Parse(typeof(EnumAxis), txtXAxis.Text);
                posi.YAxisID = (int)Enum.Parse(typeof(EnumAxis), txtYAxis.Text);
                posi.ZAxisID = (int)Enum.Parse(typeof(EnumAxis), txtZAxis.Text);
                posi.RAxisID = (int)Enum.Parse(typeof(EnumAxis), txtRAxis.Text);
                //
                posi.BaseX = Convert.ToDouble(txtBaseX.Text);
                posi.BaseY = Convert.ToDouble(txtBaseY.Text);
                posi.BaseZ = Convert.ToDouble(txtBaseZ.Text);
                posi.BaseR = Convert.ToDouble(txtBaseR.Text);
                posi.OffsetX = Convert.ToDouble(txtOffsetX.Text);
                posi.OffsetY = Convert.ToDouble(txtOffsetY.Text);
                //
                algo.ExposureTime = Convert.ToDouble(txtExposureTime.Text);
                algo.Gain = Convert.ToDouble(txtGain.Text);
                algo.MatchMinScore = Convert.ToDouble(txtMinScore.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowWorkDataTable(int index)
        {
            dgvCalibResult.DataSource =
                new BindingList<AutoNormalCoordi>(AutoNormalData.Instance.AutoNormalResultList[index].AutoNormalCoordiList);
        }

        #endregion
    }
}