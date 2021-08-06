using Poc2Auto.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AlcUtility.Language;

namespace Poc2Auto.GUI.RunModeConfig
{
    public partial class UCModeParams_SelectDut_SN : UserControl
    {
        public UCModeParams_SelectDut_SN()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            saveFilePath.Title = "请选择保存文件的路径";
        }

        public AutoSelectSnParam ParamData 
        { 
            get
            {
                if (CYGKit.GUI.Common.IsDesignMode())
                    return null;
                var snList = listBoxSNs.Items.Cast<string>().ToList();
                return new AutoSelectSnParam
                {
                    LoadNum = UCMain.Instance.ucTrays1.LoadLRegionNumber,
                    LoadRegion = UCMain.Instance.ucTrays1.LoadLTrayData,
                    LoadTaryID = (int)TrayName.LoadL,
                    DutSnList = snList
                };
            }
            set
            {

            }
        }

        private void ListBoxSNs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxSNs.SelectedItem == null) return;
            textBoxInput.Text = listBoxSNs.SelectedItem.ToString();
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxInput.Text))
                return;
            if (listBoxSNs.Items.Contains(textBoxInput.Text)) return;
            listBoxSNs.Items.Add(textBoxInput.Text);
        }

        private List<string> readCsvFile(string path)
        {
            try
            {
                var list = new List<string>();
                using (var reader = new StreamReader(path))
                {
                    while (!reader.EndOfStream)
                    {
                        list.Add(reader.ReadLine());
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void saveToolStrip_Click(object sender, EventArgs e)
        {
            saveSnlist(listBoxSNs);
        }

        private void saveSnlist(ListBox listBox)
        {
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                using (var fs = new FileStream(saveFile.FileName.Trim(), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    using (var sw = new StreamWriter(fs, Encoding.Default))
                    {
                        foreach (var sn in listBox.Items)
                        {
                            sw.WriteLine(sn);
                        }
                        fs.Flush();
                    }
                }
            }
        }

        private void deleteToolStrip_Click(object sender, EventArgs e)
        {
            if (listBoxSNs.SelectedItem == null) return;
            var list = listBoxSNs.SelectedItems.Cast<string>().ToList();
            foreach (var sn in list)
            {
                listBoxSNs.Items.Remove(sn);
            }
        }

        private void clearToolStrip_Click(object sender, EventArgs e)
        {
            listBoxSNs.Items.Clear();
        }

        private void btnAddSnList_Click(object sender, EventArgs e)
        {
            foreach (var sn in listBoxSNs.SelectedItems)
            {
                if (!listBoxSNs.Items.Contains(sn))
                    listBoxSNs.Items.Add(sn);
            }
        }

        private void saveSelectSn_Click(object sender, EventArgs e)
        {
            saveSnlist(listBoxSNs);
        }

        private void deleteSelectSn_Click(object sender, EventArgs e)
        {
            if (listBoxSNs.SelectedItem == null) return;
            var list = listBoxSNs.SelectedItems.Cast<string>().ToList();
            foreach (var sn in list)
            {
                listBoxSNs.Items.Remove(sn);
            }
        }

        private void clearSelectSn_Click(object sender, EventArgs e)
        {
            listBoxSNs.Items.Clear();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var path = openFile.FileName;
                var snList = readCsvFile(path);
                listBoxSNs.Items.Clear();
                foreach (var sn in snList)
                {
                    listBoxSNs.Items.Add(sn);
                }
            }
        }

        private void ckbxOnlyScan_CheckedChanged(object sender, EventArgs e)
        {
            RunModeMgr.IsOnlyScanMode = ckbxOnlyScan.Checked;

            btnImport.Enabled = !ckbxOnlyScan.Checked;
            buttonAddSnList.Enabled = !ckbxOnlyScan.Checked;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                var path = openFile.FileName;
                var snList = readCsvFile(path);
                listBoxSNs.Items.Clear();
                foreach (var sn in snList)
                {
                    listBoxSNs.Items.Add(sn);
                }
            }
        }

        private void ckbxAllSelect_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < listBoxSNs.Items.Count; i++)
            {
                listBoxSNs.SetSelected(i, ckbxAllSelect.Checked);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (saveFilePath.ShowDialog() == DialogResult.OK)
            {
                RunModeMgr.SelectSNSavePath = saveFilePath.FileName;
            }
        }
    }
}
