using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionUtility;
using VisionModules;

namespace VisionDemo
{
    public partial class FrmGlobalVariable : FrmBase
    {
        public FrmGlobalVariable()
        {
            InitializeComponent();

            IniDgv(dgvGlobalVariable);
        }

        private void FrmGlobalVariable_Load(object sender, EventArgs e)
        {
            //dgvGlobalVariable.Rows.Add(20);
            dgvBindingProperty(dgvGlobalVariable);
            updateVariableList();
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

        private void dgvBindingProperty(DataGridView dgv)
        {
            dgv.AutoGenerateColumns = false;
            DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
            col.HeaderText = "索引";
            //col.DataPropertyName = "CameraUserID";
            col.Name = "ID";
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "类型";
            col.DataPropertyName = "m_Data_Type";
            col.Name = "m_Data_Type";
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "名称";
            col.DataPropertyName = "m_Data_Name";
            col.Name = "m_Data_Name";
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "值";
            col.DataPropertyName = "m_Data_InitValue";
            col.Name = "m_Data_InitValue";
            dgv.Columns.Add(col);

            col = new DataGridViewTextBoxColumn();
            col.HeaderText = "备注";
            col.DataPropertyName = "m_DataTip";
            col.Name = "m_DataTip";
            dgv.Columns.Add(col);
        }


        private int CurrentIndex = 0;
        private void btnAddInt_Click(object sender, EventArgs e)
        {
            CurrentIndex = dgvGlobalVariable.RowCount;
            string varName = "val" + CurrentIndex.ToString();
            DataAtrribution atrr = DataAtrribution.全局变量;

            if (IndexOfVariable(atrr, varName) > -1)
            {
                MessageBox.Show("变量重定义");
                return;
            }

            F_DATA_CELL datacell = new F_DATA_CELL();
            datacell.m_Data_Atrr = atrr;
            datacell.m_Data_Group = DataGroup.单量;

            datacell.m_bUserDefineVariable = true;
            datacell.m_Data_Num = 1;
            DataType type = DataType.数值型;
            datacell.InitValue(type, CurrentIndex.ToString());
            datacell.m_Data_Name = varName;
            datacell.m_DataTip = "";
            switch (atrr)
            {
                case DataAtrribution.全局变量:
                    datacell.m_Data_CellID = VisionModulesManager.USys;
                    VisionModulesManager.VariableList.Add(datacell);
                    break;

                case DataAtrribution.流程变量:
                    datacell.m_Data_CellID = VisionModulesManager.U000;
                    VisionModulesManager.CurrFlow.VariableList.Add(datacell);             
                    break;
            }

            updateVariableList();
        }

        /// <summary>
        /// 获取当前变量
        /// </summary>
        /// <param name="sName">变量名称</param>
        /// <returns></returns>
        private int IndexOfVariable(DataAtrribution scope, string sName)
        {
            int index = -1;
            switch (scope)
            {
                case DataAtrribution.全局变量:
                    index = VisionModulesManager.VariableList.FindIndex(c => c.m_Data_Name == sName);
                    break;
                case DataAtrribution.流程变量:
                    index = VisionModulesManager.CurrFlow.VariableList.FindIndex(c => c.m_Data_CellID == VisionModules.VisionModulesManager.U000 && c.m_Data_Name == sName);
                    break;
            }
            return index;
        }

        private void updateVariableList()
        {
            DataAtrribution atrr = DataAtrribution.全局变量;
            switch (atrr)
            {
                case DataAtrribution.全局变量:
                    dgvGlobalVariable.DataSource = VisionModulesManager.VariableList.ToList();
                    break;

                case DataAtrribution.流程变量:
                    if (VisionModulesManager.CurrFlow == null)
                        return;

                    dgvGlobalVariable.DataSource = VisionModulesManager.CurrFlow.VariableList.FindAll(c => c.m_Data_CellID == VisionModulesManager.U000).ToList();
                    break;
            }
            CurrentIndex = dgvGlobalVariable.RowCount;
            if (CurrentIndex > 0)
            {
                dgvGlobalVariable.CurrentCell = dgvGlobalVariable.Rows[CurrentIndex-1].Cells[0];
            }
        }

        private void btnAddDouble_Click(object sender, EventArgs e)
        {
            CurrentIndex = dgvGlobalVariable.RowCount;
            string varName = "val" + CurrentIndex.ToString();
            DataAtrribution atrr = DataAtrribution.全局变量;

            if (IndexOfVariable(atrr, varName) > -1)
            {
                MessageBox.Show("变量重定义");
                return;
            }

            F_DATA_CELL datacell = new F_DATA_CELL();
            datacell.m_Data_Atrr = atrr;
            datacell.m_Data_Group = DataGroup.单量;

            datacell.m_bUserDefineVariable = true;
            datacell.m_Data_Num = 1;
            DataType type = DataType.数值型;
            datacell.InitValue(type, CurrentIndex.ToString("0.0"));
            datacell.m_Data_Name = varName;
            datacell.m_DataTip = "";
            switch (atrr)
            {
                case DataAtrribution.全局变量:
                    datacell.m_Data_CellID = VisionModulesManager.USys;
                    VisionModulesManager.VariableList.Add(datacell);
                    break;

                case DataAtrribution.流程变量:
                    datacell.m_Data_CellID = VisionModulesManager.U000;
                    VisionModulesManager.CurrFlow.VariableList.Add(datacell);
                    break;
            }

            updateVariableList();
        }

        private void btnAddString_Click(object sender, EventArgs e)
        {

        }

        private void btnAddBool_Click(object sender, EventArgs e)
        {

        }


    }
}
