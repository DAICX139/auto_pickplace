using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VisionFlows.VisionCalculate
{
    public partial class FormSystemPara : Form
    {
        public FormSystemPara()
        {
            InitializeComponent();
        }

        private void FormSystemPara_Load(object sender, EventArgs e)
        {
            dataGridViewSystemPara.Rows.Clear();

            DisplayDataGrid("ImageSavePath", SystemPara.Instance.ImageSavePath,"");
            DisplayDataGrid("NozzleMarkAndCenterOffset", SystemPara.Instance.NozzleMarkAndCenterOffset.X.ToString(), SystemPara.Instance.NozzleMarkAndCenterOffset.Y.ToString());
            DisplayDataGrid("DutFrontMarkAndCenterOffset", SystemPara.Instance.DutFrontMarkAndCenterOffset.X.ToString(), SystemPara.Instance.DutFrontMarkAndCenterOffset.Y.ToString());

            DisplayDataGrid("DutBackMarkAndCenterOffset", SystemPara.Instance.DutBackMarkAndCenterOffset.X.ToString(), SystemPara.Instance.DutBackMarkAndCenterOffset.Y.ToString());
            DisplayDataGrid("SocketMarkAndCenterOffset", SystemPara.Instance.SocketMarkAndCenterOffset.X.ToString(), SystemPara.Instance.SocketMarkAndCenterOffset.Y.ToString());

            DisplayDataGrid("SocketMarkAndCenterAngle", SystemPara.Instance.SocketMarkAndCenterAngle.ToString(), "");
            DisplayDataGrid("TraySlotAndCenterOffset", SystemPara.Instance.TraySlotAndCenterOffset.X.ToString(), SystemPara.Instance.TraySlotAndCenterOffset.Y.ToString());

            DisplayDataGrid("PreciseMarkAndCenterOffset", SystemPara.Instance.PreciseMarkAndCenterOffset.X.ToString(), SystemPara.Instance.PreciseMarkAndCenterOffset.Y.ToString());
            DisplayDataGrid("Nozzle1AngleInInitStatus", SystemPara.Instance.Nozzle1AngleInInitStatus.ToString(),"");

            DisplayDataGrid("Nozzle1CenterCoordiInInitStatus", SystemPara.Instance.Nozzle1CenterCoordiInInitStatus.X.ToString(), SystemPara.Instance.Nozzle1CenterCoordiInInitStatus.Y.ToString());
            DisplayDataGrid("Nozzle1RotateCenterCoordiInInitStatus", SystemPara.Instance.Nozzle1RotateCenterCoordiInInitStatus.X.ToString(), SystemPara.Instance.Nozzle1RotateCenterCoordiInInitStatus.Y.ToString());

            DisplayDataGrid("Cam1CenterAndNozzle1RotateCenterOffset", SystemPara.Instance.Cam1CenterAndNozzle1RotateCenterOffset.X.ToString(), SystemPara.Instance.Cam1CenterAndNozzle1RotateCenterOffset.Y.ToString());
            DisplayDataGrid("Nozzle2AngleInInitStatus", SystemPara.Instance.Nozzle2AngleInInitStatus.ToString(), "");

            DisplayDataGrid("Nozzle2CenterCoordiInInitStatus", SystemPara.Instance.Nozzle2CenterCoordiInInitStatus.X.ToString(), SystemPara.Instance.Nozzle2CenterCoordiInInitStatus.Y.ToString());
            DisplayDataGrid("Nozzle2RotateCenterCoordiInInitStatus", SystemPara.Instance.Nozzle2RotateCenterCoordiInInitStatus.X.ToString(), SystemPara.Instance.Nozzle2RotateCenterCoordiInInitStatus.Y.ToString());

            DisplayDataGrid("Cam2CenterAndNozzle2RotateCenterOffset", SystemPara.Instance.Cam2CenterAndNozzle2RotateCenterOffset.X.ToString(), SystemPara.Instance.Cam2CenterAndNozzle2RotateCenterOffset.Y.ToString());
        }


        void DisplayDataGrid(string key, string valueX, string valueY)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dataGridViewSystemPara);

            row.Cells[0].Value = key;
            row.Cells[1].Value = valueX;
            row.Cells[2].Value = valueY;

            dataGridViewSystemPara.Rows.Add(row);
        }

        private void BtnSaveAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridViewSystemPara.Rows)
            {
                if (item.Cells[0].Value == null)
                {
                    continue;
                }
                if (item.Cells[0].Value.ToString() == "ImageSavePath")
                {
                    SystemPara.Instance.ImageSavePath = item.Cells[1].Value.ToString();
                }
                if (item.Cells[0].Value.ToString() == "NozzleMarkAndCenterOffset")
                {
                    SystemPara.Instance.NozzleMarkAndCenterOffset.X = double.Parse(item.Cells[1].Value.ToString());
                    SystemPara.Instance.NozzleMarkAndCenterOffset.Y = double.Parse(item.Cells[2].Value.ToString());
                }
                if (item.Cells[0].Value.ToString() == "DutFrontMarkAndCenterOffset")
                {
                    SystemPara.Instance.DutFrontMarkAndCenterOffset.X = double.Parse(item.Cells[1].Value.ToString());
                    SystemPara.Instance.DutFrontMarkAndCenterOffset.Y = double.Parse(item.Cells[2].Value.ToString());
                }
                if (item.Cells[0].Value.ToString() == "DutBackMarkAndCenterOffset")
                {
                    SystemPara.Instance.DutBackMarkAndCenterOffset.X = double.Parse(item.Cells[1].Value.ToString());
                    SystemPara.Instance.DutBackMarkAndCenterOffset.Y = double.Parse(item.Cells[2].Value.ToString());
                }
                if (item.Cells[0].Value.ToString() == "SocketMarkAndCenterOffset")
                {
                    SystemPara.Instance.SocketMarkAndCenterOffset.X = double.Parse(item.Cells[1].Value.ToString());
                    SystemPara.Instance.SocketMarkAndCenterOffset.Y = double.Parse(item.Cells[2].Value.ToString());
                }
                if (item.Cells[0].Value.ToString() == "SocketMarkAndCenterAngle")
                {
                    SystemPara.Instance.SocketMarkAndCenterAngle = double.Parse(item.Cells[1].Value.ToString());
                }
                if (item.Cells[0].Value.ToString() == "TraySlotAndCenterOffset")
                {
                    SystemPara.Instance.TraySlotAndCenterOffset.X = double.Parse(item.Cells[1].Value.ToString());
                    SystemPara.Instance.TraySlotAndCenterOffset.Y = double.Parse(item.Cells[2].Value.ToString());
                }
                if (item.Cells[0].Value.ToString() == "PreciseMarkAndCenterOffset")
                {
                    SystemPara.Instance.PreciseMarkAndCenterOffset.X = double.Parse(item.Cells[1].Value.ToString());
                    SystemPara.Instance.PreciseMarkAndCenterOffset.Y = double.Parse(item.Cells[2].Value.ToString());
                }
                if (item.Cells[0].Value.ToString() == "Nozzle1AngleInInitStatus")
                {
                    SystemPara.Instance.Nozzle1AngleInInitStatus = double.Parse(item.Cells[1].Value.ToString());
                }
                if (item.Cells[0].Value.ToString() == "Nozzle1CenterCoordiInInitStatus")
                {
                    SystemPara.Instance.Nozzle1CenterCoordiInInitStatus.X = double.Parse(item.Cells[1].Value.ToString());
                    SystemPara.Instance.Nozzle1CenterCoordiInInitStatus.Y = double.Parse(item.Cells[2].Value.ToString());
                }
                if (item.Cells[0].Value.ToString() == "Nozzle1RotateCenterCoordiInInitStatus")
                {
                    SystemPara.Instance.Nozzle1RotateCenterCoordiInInitStatus.X = double.Parse(item.Cells[1].Value.ToString());
                    SystemPara.Instance.Nozzle1RotateCenterCoordiInInitStatus.Y = double.Parse(item.Cells[2].Value.ToString());
                }
                if (item.Cells[0].Value.ToString() == "Cam1CenterAndNozzle1RotateCenterOffset")
                {
                    SystemPara.Instance.Cam1CenterAndNozzle1RotateCenterOffset.X = double.Parse(item.Cells[1].Value.ToString());
                    SystemPara.Instance.Cam1CenterAndNozzle1RotateCenterOffset.Y = double.Parse(item.Cells[2].Value.ToString());
                }
                if (item.Cells[0].Value.ToString() == "Nozzle2AngleInInitStatus")
                {
                    SystemPara.Instance.Nozzle2AngleInInitStatus = double.Parse(item.Cells[1].Value.ToString());
                }
                if (item.Cells[0].Value.ToString() == "Nozzle2CenterCoordiInInitStatus")
                {
                    SystemPara.Instance.Nozzle2CenterCoordiInInitStatus.X = double.Parse(item.Cells[1].Value.ToString());
                    SystemPara.Instance.Nozzle2CenterCoordiInInitStatus.Y = double.Parse(item.Cells[2].Value.ToString());
                }
                if (item.Cells[0].Value.ToString() == "Nozzle2RotateCenterCoordiInInitStatus")
                {
                    SystemPara.Instance.Nozzle2RotateCenterCoordiInInitStatus.X = double.Parse(item.Cells[1].Value.ToString());
                    SystemPara.Instance.Nozzle2RotateCenterCoordiInInitStatus.Y = double.Parse(item.Cells[2].Value.ToString());
                }
                if (item.Cells[0].Value.ToString() == "Cam2CenterAndNozzle2RotateCenterOffset")
                {
                    SystemPara.Instance.Cam2CenterAndNozzle2RotateCenterOffset.X = double.Parse(item.Cells[1].Value.ToString());
                    SystemPara.Instance.Cam2CenterAndNozzle2RotateCenterOffset.Y = double.Parse(item.Cells[2].Value.ToString());
                }
            }
            Flow.XmlHelper.SerializeToXml(Utility.Config + "System.xml", SystemPara.Instance);
            GeometryCalculate.Init();
            MessageBox.Show("Saved");

            this.Close();
        }
    }
}
