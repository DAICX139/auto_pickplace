using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poc2Auto.GUI.UCModeUI.UCSemiAuto
{
    public partial class UC_PlcVarList : UserControl
    {
        public UC_PlcVarList()
        {
            InitializeComponent();
        }
        public delegate void Action(string key, string value);
        /// <summary>
        /// 列表文本改变时触发
        /// </summary>
        public new event Action TextChanged;

        /// <summary>
        /// key = key, value = value
        /// </summary>
        public Dictionary<string, string> KeyValuePairs { get; private set; } = new Dictionary<string, string>();

        public bool AuthorityCtrl { set => dataGridView1.Columns[1].ReadOnly = value; }

        /// <summary>
        /// 写入表格
        /// </summary>
        /// <param name="key">第一列数据</param>
        /// <param name="value">第二列数据</param>
        public void LoadData(string key, string value = "")
        {
            dataGridView1.Rows.Add();
            dataGridView1.Rows[dataGridView1.RowCount - 1].Height = 20;
            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = key;
            dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = value;
            KeyValuePairs.Add(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddData(string key, string value)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value.ToString() == key)
                {
                    row.Cells[1].Value = value;
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var key = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value?.ToString();
            var value = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value?.ToString();
            key = key ?? "";
            value = value ?? "";
            if (KeyValuePairs.ContainsKey(key))
                KeyValuePairs[key] = value;
            else
                KeyValuePairs.Add(key, value);
            TextChanged?.Invoke(key, value);
        }
    }
}
