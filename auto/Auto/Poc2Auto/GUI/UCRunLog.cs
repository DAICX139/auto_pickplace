using System;
using System.Windows.Forms;
using Poc2Auto.Common;
using System.Drawing;

namespace Poc2Auto.GUI
{
    public partial class UCRunLog : UserControl
    {
        public UCRunLog()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[0].Width = 80;
            EventCenter.ProcessInfo += addText;
        }

        private void addText(string txt, ErrorLevel level)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { addRowData(txt, level);  } ));
                return;
            }
            addRowData(txt, level);
        }

        private void addRowData(string msg, ErrorLevel level)
        {
            dataGridView1.Rows.Add();
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.Rows.Count - 1;
            var row = dataGridView1.Rows[dataGridView1.Rows.Count - 1];
            switch (level)
            {
                case ErrorLevel.Debug:
                    break;
                case ErrorLevel.Warning:
                    row.DefaultCellStyle.ForeColor = Color.Brown;
                    break;
                case ErrorLevel.Fatal:
                    row.DefaultCellStyle.ForeColor = Color.Red;
                    break;
                default:
                    break;
            }
            var time = DateTime.Now.ToString("MM/dd HH:mm:ss");
            row.Cells[ColTime.Index].Value = time;
            row.Cells[ColLogMsg.Index].Value = msg;
        }

        private void menuClear_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }
    }
}
