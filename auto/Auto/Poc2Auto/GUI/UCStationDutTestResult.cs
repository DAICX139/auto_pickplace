using System;
using System.Drawing;
using System.Windows.Forms;
using AlcUtility;
using Poc2Auto.Common;
using Poc2Auto.Model;

namespace Poc2Auto.GUI
{
    public partial class UCStationDutTestResult : UserControl
    {
        public UCStationDutTestResult()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
        }
        private void UC_DutResult_Load(object sender, EventArgs e)
        {
            EventCenter.StationTestDone += stationTestDone;
        }
        private void stationTestDone(Station station, string dutSn, int result)
        {        
            if (InvokeRequired)
            {
                Invoke(new Action(() =>stationTestDone(station, dutSn, result)));
                return;
            }
            int rowIndex = 0;
            bool isExistDut = false;
                      
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row == null || row.Cells[ColDutSn.Index] == null || row.Cells[ColDutSn.Index].Value == null)
                    continue;
                if (row.Cells[ColDutSn.Index].Value.ToString() == dutSn)
                {
                    isExistDut = true;
                    rowIndex = row.Index;
                    break;
                }
            }
            if (!isExistDut)
            {
                dataGridView1.Rows.Add();
                rowIndex = dataGridView1.Rows.Count - 1;
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;             
                dataGridView1.Rows[rowIndex].Cells[ColDutSn.Index].Value = dutSn;
            }
            var colIndex = GetColIndex(station.Name);
            if (colIndex == -1) return;
            dataGridView1.Rows[rowIndex].Cells[colIndex].Value = GetTestRes(result);
            dataGridView1.Rows[rowIndex].Cells[colIndex].Style.BackColor = GetColor(result);
            SaveTestResult(dutSn, station.Name, result);
        }

        private int GetColIndex(StationName station)
        {
            switch (station)
            {
                case StationName.Test1_LIVW:
                    return ColLIVW.Index;
                case StationName.Test2_DTGT:
                    return ColDTGT.Index;
                case StationName.Test4_BMPF:
                    return ColBMPF.Index;
                default:
                    return -1;
            }
        }

        private Color GetColor(int result)
        {         
            if (result == 1||result==4)
                return Color.Green;
            else if (result ==2||result==3||
                 result == 0|| result == -1)
                return Color.Red;          
            else
                return Color.White;
        }

        private string GetTestRes(int result)
        {
            if (result == Dut.PassBin)
                return "Pass";
            else if (result == Dut.Fail_A)
                return "Fail_A";
            else if (result == Dut.Fail_B)
                return "Fail_B";
            else if (result == Dut.Fail_All)
                return "Fail";
            else
                return "Error";
        }
        private void SaveTestResult(string dutsn, StationName station, int result)
        {
            string head = string.Format(@"{0},{1},{2}", "DutSn", "StationName","TestResult");
            string text = string.Format(@"{0},{1},{2}", dutsn, station.ToString(), result);
            AlcSystem.Instance.SaveCsv("StationTestResult", head, text);
        }
    }
}
