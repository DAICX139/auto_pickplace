using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AlcUtility;
using Poc2Auto.Common;
using Poc2Auto.Database;
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
            EventCenter.EndLot += EndLot;
        }

        private void UC_DutResult_Load(object sender, EventArgs e)
        {
            EventCenter.StationTestDone += StationTestDone;
            EventCenter.LoadDutDone += LoadDutDone;
            EventCenter.UnloadDutDone += UnloadDutDone;
        }
        private void StationTestDone(Station station, string dutSn, int result)
        {
            AlcSystem.Instance.Log($"input StationTestDone()", "界面测试结果");
            if (InvokeRequired)
            {
                Invoke(new Action(() =>StationTestDone(station, dutSn, result)));
                return;
            }

            var stn = station;
            var sn = dutSn;

            AlcSystem.Instance.Log($"StationTestDone() --> DUTSN: {sn}", "界面测试结果");

            var row = dataGridView1.Rows.Cast<DataGridViewRow>().LastOrDefault(
                    r => r.Cells[ColDutSn.Index].Value.ToString() == sn);
             
            if (row == null)
            {
                AlcSystem.Instance.Log($"Get DUT Row is null", "界面测试结果");
                return;
            }
            
            if (GetColIndex(stn.Name) == -1)
            {
                AlcSystem.Instance.Log($"GetColIndex({stn.Name}) --> -1", "界面测试结果");
                return;
            }

            string testResult;
            if (result == 0)
                testResult = "4";
            else if (result == -1)
                testResult = "TMError";
            else
                testResult = result.ToString();
            row.Cells[GetColIndex(stn.Name)].Value = testResult;
            row.Cells[GetColIndex(stn.Name)].Style.BackColor = GetColor(result);
            SaveTestResult(sn, stn.Name, testResult);

            AlcSystem.Instance.Log($"output StationTestDone()", "界面测试结果");
        }

        private void LoadDutDone(string dutSN, string socketID)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { LoadDutDone(dutSN, socketID); }));
                return;
            }
            dataGridView1.Rows.Add();
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
            var rowIndex = dataGridView1.Rows.Count - 1;
            dataGridView1.Rows[rowIndex].Cells[ColSocketSN.Index].Value =socketID;
            dataGridView1.Rows[rowIndex].Cells[ColDutSn.Index].Value = dutSN;
        }

        private void UnloadDutDone(string[] dutSN, string socketID, int[] bin)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { UnloadDutDone(dutSN, socketID, bin); }));
                return;
            }

            var row = dataGridView1.Rows.Cast<DataGridViewRow>().LastOrDefault(
                    r => r.Cells[ColSocketSN.Index].Value.ToString() == socketID
                    && r.Cells[ColDutSn.Index].Value.ToString() == dutSN[0]);
            if (row == null) return;


            //if (bin[0] == 4)
            //    binText = "F";
            //else if (bin[0] == 5)
            //    binText = "99";
            //else
            string binText = bin[0].ToString();

            row.Cells[ColBin.Index].Value = binText;
            row.Cells[ColBin.Index].Style.BackColor = DragonDbHelper.GetBinColor(bin[0]);

        }

        private int GetColIndex(StationName name)
        {
            switch (name)
            {
                case StationName.Test1_LIVW:
                    AlcSystem.Instance.Log($"GetColIndex({name}) --> {ColLIVW.Index}", "界面测试结果");
                    return ColLIVW.Index;
                case StationName.Test2_NFBP:
                    AlcSystem.Instance.Log($"GetColIndex({name}) --> {ColNFBP.Index}", "界面测试结果");
                    return ColNFBP.Index;
                case StationName.Test3_KYRL:
                    AlcSystem.Instance.Log($"GetColIndex({name}) --> {ColKYRL.Index}", "界面测试结果");
                    return ColKYRL.Index;
                case StationName.Test4_BMPF:
                    AlcSystem.Instance.Log($"GetColIndex({name}) --> {ColBMPF.Index}", "界面测试结果");
                    return ColBMPF.Index;
                default:
                    return -1;
            }
        }

        private Color GetColor(int result)
        {
            if (result == -1) return Color.Red;
            var color = DragonDbHelper.GetBinColor(result == 0 ? 4 : result);
            if (color == null) return Color.White;
            return color;
        }
 
        private void SaveTestResult(string dutsn, StationName station, string result)
        {
            string head = string.Format(@"{0},{1},{2}", "DutSn", "StationName","TestResult");
            string text = string.Format(@"{0},{1},{2}", dutsn, station.ToString(), result);
            AlcSystem.Instance.SaveCsv("StationTestResult", head, text);
        }
 
        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { dataGridView1.Rows.Clear(); }));
                return;
            }
            dataGridView1.Rows.Clear();
        }
        
        private void EndLot()
        {
            ToolStripMenuItem_Click(null, null);
        }
    }
}
