using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlcUtility;
using AlcUtility.Config;
using AlcUtility.PlcDriver.CommonCtrl;

namespace DragonFlex.GUI.Factory.UC_handlePLC
{
    public partial class UC_SingleAxises : UserControl
    {
        public UC_SingleAxises()
        {
            InitializeComponent();
            init();
        }

        IPlcDriver _plcDriver;
        public IPlcDriver PlcDriver
        {
            get
            {
                return _plcDriver;
            }
            set
            {
                if (value == null)
                    return;
                _plcDriver = value;
                value.OnInitOk += BindData;
            }
        }
        List<UC_SingleAxis> singleAxes = new List<UC_SingleAxis>();
        Dictionary<Pos, UC_SingleAxis> singleAxisDic = new Dictionary<Pos, UC_SingleAxis>();

        Dictionary<string, List<ParamsValue>> valueAxis = new Dictionary<string, List<ParamsValue>>();
        public int Axis_COUNT { get; set; } = 6;
        string recipeFile = $"{Application.StartupPath}\\paramFiles\\HandlerConfigFile\\HanderPLC_cfgParams.xml";
        ParamsConfig config;

        void init()
        {
            if (CYGKit.GUI.Common.IsDesignMode()) return;
            List<string> Axises = new List<string>()
            {
                Pos.X.ToString() + "pos",
                Pos.Y.ToString() + "pos",
                Pos.Z.ToString() + "pos",
                Pos.R.ToString() + "pos",
                Pos.R1.ToString()+ "pos",
                Pos.R2.ToString()+ "pos"
            };
            foreach (var axis in Axises)
            {
                valueAxis.Add(axis, new List<ParamsValue>());
            }
            loadDataFromFile();
            for (int i = 0; i < Axis_COUNT; i++)
            {
                UC_SingleAxis axis = new UC_SingleAxis();
                axis.Dock = DockStyle.Fill;
                axis.Margin = new Padding(0, 0, 0, 0);
                //axis.AxisName = (Pos.X + i).ToString();
                singleAxes.Add(axis);
                singleAxisDic.Add(Pos.X+i, axis);
                axis.LoadPositions<ParamsValue>(valueAxis[(Pos.X+i).ToString() + "pos"], "Remark", "Value");
            }
            cBoxPos.DataSource = Enum.GetValues(typeof(Pos));
            cBoxModule.DataSource = Enum.GetValues(typeof(Module));
        }

        void BindData()
        {
            for (int i = 0; i < Axis_COUNT; i++)
            {
                singleAxes[i].SingleAxis = PlcDriver.GetSingleAxisCtrl(i);
                singleAxes[i].EnableUpdate = true;
            }
            btnMove.Enabled = true;
        }

        public bool EnableUI { set
            {
                foreach (var ui in singleAxes)
                    ui.EnableUI = value;
                btnSave.Enabled = value;
                btnMove.Enabled = value;
            }
        }

        private void cBoxModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            panel2.Controls.Clear();
            if ((Module)cBoxModule.SelectedItem == Module.XY)
            {
                panel1.Controls.Add(singleAxisDic[Pos.X]);
                panel2.Controls.Add(singleAxisDic[Pos.Y]);
            }
            else if ((Module)cBoxModule.SelectedItem == Module.ZR)
            {
                panel1.Controls.Add(singleAxisDic[Pos.Z]);
                panel2.Controls.Add(singleAxisDic[Pos.R]);
            }
            else if((Module)cBoxModule.SelectedItem == Module.R1R2)
            {
                panel1.Controls.Add(singleAxisDic[Pos.R1]);
                panel2.Controls.Add(singleAxisDic[Pos.R2]);
            }
        }

        private void UC_SingleAxises_Load(object sender, EventArgs e)
        {
        }

        private void loadDataFromFile()
        {
            listBoxPos.Items.Clear();
            listBoxPos.DisplayMember = "Remark";
            config = ParamsConfig.Upload(recipeFile);
            if (config == null) return;
            foreach (var moduleItem in config.ParamsModules.Values)
                foreach (var valueItem in moduleItem.KeyValues.Values)
                {
                    string axis = valueItem.Key.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Last();
                    valueAxis[axis].Add(valueItem);
                }
        }

        private void cBoxPos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (valueAxis.Count == 0)
                return;
            if ((Pos)cBoxPos.SelectedItem == Pos.Pos)
            {
                //
                return;
            }
            listBoxPos.DataSource = valueAxis[cBoxPos.SelectedItem.ToString() + "pos"];
        }

        private void listBoxPos_Click(object sender, EventArgs e)
        {
            if (listBoxPos.SelectedItem == null)
                return;
            if (!cBoxModule.SelectedItem.ToString().Contains(cBoxPos.SelectedItem.ToString()))
                return;
            singleAxisDic[(Pos)cBoxPos.SelectedItem].CurrentSelectItem = listBoxPos.SelectedItem;
        }
        #region Enum
        enum Module
        {
            XY,
            ZR,
            R1R2
        }

        enum Pos
        {
            X,
            Y,
            Z,
            R,
            R1,
            R2,
            Pos
        }
        #endregion

        private void btnMove_Click(object sender, EventArgs e)
        {

        }

        private void Save_Click(object sender, EventArgs e)
        {
            ParamsValue val = listBoxPos.SelectedItem as ParamsValue;
            val.Value = singleAxisDic[(Pos)cBoxPos.SelectedItem].Pos;
            config.Save();
        }
    }
}
