using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Poc2Auto.Database;
using LogLib.Managers;

namespace Poc2Auto.GUI
{
    public partial class FMDutStatList : Form
    {
        public FMDutStatList()
        {
            InitializeComponent();
        }

        private List<DUTStationBinTotal> _dataSource;
        List<DUTStationBinTotal> data;
        public List<DUTStationBinTotal> DataSource
        {
            get { return _dataSource; }
            set 
            {
                if (null == value)
                    return;
                _dataSource = value;
                List<string> idList = new List<string>();
                foreach (var data in DataSource)
                {
                    if (!idList.Contains(data.LotID))
                    {
                        idList.Add(data.LotID);
                    }
                }
                comboBoxLotID.DataSource = idList;
            }
        }

        private void FMDutStatList_Shown(object sender, EventArgs e)
        {
            DataSource = DragonDbHelper.GetStationBinTotal();
        }

        private void btnOutPut_Click(object sender, EventArgs e)
        {
            if (null == comboBoxLotID.SelectedItem)
                return;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var fName = saveFileDialog1.FileName;

                string head = string.Format(@"{0},{1},{2},{3},{4},{5},{6}", "LotID","DUTSN", "LIV_Result", "NFBP_Result", "KYRL_Result", "BP_Result", "Bin");

                foreach (var item in data)
                {
                    string text = string.Format(@"{0},{1},{2},{3},{4},{5},{6}", $"{ comboBoxLotID.SelectedItem }",
                    item.DUTSN, item.LIV_Result.ToString(), item.NFBP_Result.ToString(),
                        item.KYRL_Result.ToString(), item.BP_Result.ToString(), item.Bin.ToString());
                    Log4netMgr.Instance.SaveSCVForFullPath(fName, text, head, false);
                }
            }
        }

        private void comboBoxLotID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null == comboBoxLotID.SelectedItem)
                return;
            var lotId = comboBoxLotID.SelectedItem.ToString();

            ShowLotData(lotId);
        }

        private void ShowLotData(string lotId)
        {
            listView1.Items.Clear();
            data = _dataSource.Where(l => l.LotID == lotId).ToList();
            foreach (var item in data)
            {
                string[] d = new string[] { item.DUTSN, item.LIV_Result.ToString(), item.NFBP_Result.ToString(),
                    item.KYRL_Result.ToString(), item.BP_Result.ToString(), item.Bin.ToString() };
                ListViewItem listView = new ListViewItem(d);
                listView1.Items.Add(listView);
            }
        }
    }
}
