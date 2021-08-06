using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Poc2Auto.Common;
using System.IO;

namespace Poc2Auto.GUI
{
    public partial class UCErrorList : UserControl
    {
        public UCErrorList()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            lbxErrorList.IntegralHeight = false;
        }

        public void ShowErrorList(List<string> data)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<List<string>>(ShowErrorList), data);
                return;
            }
            if (data.Count == 0 || data == null)
            {
                lbxErrorList.DataSource = null;
            }
            else
                lbxErrorList.DataSource = data;

        }

        private void btnClearError_Click(object sender, EventArgs e)
        {
            EventCenter.ClearError?.Invoke();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbxErrorList.Items.Count == 0)
                return;
            string txt = $"{DateTime.Now}\r\n";
            foreach (var rowData in lbxErrorList.Items)
                txt += rowData.ToString() + "\r\n";
            var name = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + "_Error.txt";
            File.WriteAllText($"C:\\Users\\Administrator.DESKTOP-KDKC337\\Desktop\\{name}", txt);
        }
    }
}
