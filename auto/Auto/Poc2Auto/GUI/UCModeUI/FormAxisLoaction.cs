using System;
using System.Windows.Forms;
using AlcUtility;

namespace Poc2Auto.GUI.UCModeUI
{
    public partial class FormAxisLoaction : Form
    {
        public FormAxisLoaction()
        {
            InitializeComponent();
            //设置DataGridView文本居中
            dataGridView1.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            loadData();
        }

        private void loadData()
        {
            var data = Database.DragonDbHelper.LoadAxisLocations();
            foreach (var d in data)
            {
                dataGridView1.Rows.Add();
                var rowIndex = dataGridView1.Rows.Count - 1;
                var row = dataGridView1.Rows[rowIndex];
                row.Cells[ColTime.Index].Value = d.Time;
                row.Cells[ColName.Index].Value = d.Name;
                row.Cells[ColX.Index].Value = d.X;
                row.Cells[ColY.Index].Value = d.Y;
                row.Cells[ColZ1.Index].Value = d.Z1;
                row.Cells[ColR1.Index].Value = d.R1;
                row.Cells[ColZ2.Index].Value = d.Z2;
                row.Cells[ColR2.Index].Value = d.R2;
            }
        }

        public IPlcDriver PlcDriver;

        public int AxisCount { get; set; }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            LocationNameMgr.ShowNameEditor();
            var posName = LocationNameMgr.Name;
            dataGridView1.Rows.Add();
            var row = dataGridView1.Rows[dataGridView1.Rows.Count - 1];
            row.Cells[ColTime.Index].Value = DateTime.Now;
            row.Cells[ColName.Index].Value = posName;
            for (int i = 1; i <= AxisCount; i++)
            {
                var name = PlcDriver?.GetSingleAxisCtrl(i).Info.Name;
                switch (name)
                {
                    case "X":
                        row.Cells[ColX.Index].Value = PlcDriver?.GetSingleAxisCtrl(i).Info.ActPos;
                        break;
                    case "Y":
                        row.Cells[ColY.Index].Value = PlcDriver?.GetSingleAxisCtrl(i).Info.ActPos;
                        break;
                    case "Z1":
                        row.Cells[ColZ1.Index].Value = PlcDriver?.GetSingleAxisCtrl(i).Info.ActPos;
                        break;
                    case "R1":
                        row.Cells[ColR1.Index].Value = PlcDriver?.GetSingleAxisCtrl(i).Info.ActPos;
                        break;
                    case "Z2":
                        row.Cells[ColZ2.Index].Value = PlcDriver?.GetSingleAxisCtrl(i).Info.ActPos;
                        break;
                    case "R2":
                        row.Cells[ColR2.Index].Value = PlcDriver?.GetSingleAxisCtrl(i).Info.ActPos;
                        break;
                }
            }
            

            if (!DateTime.TryParse(row.Cells[ColTime.Index].Value?.ToString(), out DateTime time))
            {
                time = default;
            }
            if (!double.TryParse(row.Cells[ColX.Index].Value?.ToString(), out double x))
            {
                x = default;
            }
            if (!double.TryParse(row.Cells[ColY.Index].Value?.ToString(), out double y))
            {
                y = default;
            }
            if (!double.TryParse(row.Cells[ColZ1.Index].Value?.ToString(), out double z1))
            {
                z1 = default;
            }
            if (!double.TryParse(row.Cells[ColR1.Index].Value?.ToString(), out double r1))
            {
                r1 = default;
            }
            if (!double.TryParse(row.Cells[ColZ2.Index].Value?.ToString(), out double z2))
            {
                z2 = default;
            }
            if (!double.TryParse(row.Cells[ColR2.Index].Value?.ToString(), out double r2))
            {
                r2 = default;
            }
            Database.AxisLocation axisLocation = new Database.AxisLocation()
            {
                Time = time,
                Name = posName,
                X = x,
                Y = y,
                Z1 = z1,
                R1 = r1,
                Z2 = z2,
                R2 = r2
            };
            Database.DragonDbHelper.AddLocation(axisLocation);
        }
        

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
                return;
            var row = dataGridView1.SelectedRows[0];
            if (row.Cells[ColTime.Index].Value == null)
                return;
            var name = row.Cells[ColName.Index].Value.ToString();
            if (AlcSystem.Instance.ShowMsgBox("Are you sure to delete this row data ?", "", buttons: AlcMsgBoxButtons.YesNo, icon: AlcMsgBoxIcon.Question) == AlcMsgBoxResult.No)
                return;
            dataGridView1.Rows.RemoveAt(row.Index);
            Database.DragonDbHelper.DelAxisLocation(name);
        }

        private void Go_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
                return;
            var row = dataGridView1.SelectedRows[0];
            double X = double.Parse(row.Cells[ColX.Index].Value?.ToString());
            double Y = double.Parse(row.Cells[ColY.Index].Value?.ToString());
            double Z1 = double.Parse(row.Cells[ColZ1.Index].Value?.ToString());
            double R1 = double.Parse(row.Cells[ColR1.Index].Value?.ToString());
            double Z2 = double.Parse(row.Cells[ColZ2.Index].Value?.ToString());
            double R2 = double.Parse(row.Cells[ColR2.Index].Value?.ToString());
            if (!double.TryParse(row.Cells[ColVel.Index].Value?.ToString(), out double Vel))
            {
                AlcSystem.Instance.ShowMsgBox("Vel is Valid", "");
                return;
            }
            for (int i = 1; i <= AxisCount; i++)
            {
                var name = PlcDriver?.GetSingleAxisCtrl(i).Info.Name;
                switch (name)
                {
                    case "X":
                        PlcDriver?.GetSingleAxisCtrl(i).AbsGo(X, Vel);
                        break;
                    case "Y":
                        PlcDriver?.GetSingleAxisCtrl(i).AbsGo(Y, Vel);
                        break;
                    case "Z1":
                        PlcDriver?.GetSingleAxisCtrl(i).AbsGo(Z1, Vel);
                        break;
                    case "R1":
                        PlcDriver?.GetSingleAxisCtrl(i).AbsGo(R1, Vel);
                        break;
                    case "Z2":
                        PlcDriver?.GetSingleAxisCtrl(i).AbsGo(Z2, Vel);
                        break;
                    case "R2":
                        PlcDriver?.GetSingleAxisCtrl(i).AbsGo(R2, Vel);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
