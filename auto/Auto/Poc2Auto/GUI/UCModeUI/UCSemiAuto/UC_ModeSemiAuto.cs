using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlcUtility;

namespace Poc2Auto.GUI.UCModeUI.UCSemiAuto
{
    public partial class UC_ModeSemiAuto : UserControl
    {
        public UC_ModeSemiAuto()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 该变量应被第一个赋值，否则其他变量无效
        /// </summary>
        public IPlcDriver PlcDriver
        {
            set
            {
                if (value == null) return;
                foreach (var control in controlList)
                {
                    control.PlcDriver = value;
                }
            }
        }

        public string FilePath
        {
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                var dataSrc = ParamsConfig.Upload(value);
                if (dataSrc == null) return;
                foreach (var paramsModule in dataSrc.ParamsModules)
                {
                    init(paramsModule.Value);
                    
                    //获取公共变量
                    foreach (var paramsValue in paramsModule.Value.KeyValues.Values)
                    {
                        if (!TheOnes.Contains(paramsValue.Value))
                        {
                            TheOnes.Add(paramsValue.Value.ToString());
                        }
                    }
                }
            }
        }

        private List<UC_SemiAuto> controlList = new List<UC_SemiAuto>();

        public bool AuthorityCtrl 
        {
            set
            {
                foreach (var semiAuto in controlList)
                {
                    semiAuto.ucSemiAutoCtrl1.AuthorityCtrl = value;
                    semiAuto.ucPlcVarList1.AuthorityCtrl = !value;
                }
            }
        }

        void init(ParamsModule module)
        {
            UC_SemiAuto semiAuto = new UC_SemiAuto();
            semiAuto.Dock = DockStyle.Fill;
            semiAuto.Margin = new Padding(0);
            semiAuto.SemiAutoCtrlIndex = module.Id;
            semiAuto.ActionName = module.Description;
            semiAuto.DataSrc = module;
            semiAuto.TextChanged += SemiAuto_TextChanged;
            controlList.Add(semiAuto);

            TabPage tabPage = new TabPage();
            tabPage.Text = module.Name;
            tabPage.Controls.Add(semiAuto);
            tabControl1.Controls.Add(tabPage);
        }

        private List<string> TheOnes = new List<string>();

        /// <summary>
        /// 共享的plc变量改变时应全部改变
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void SemiAuto_TextChanged(string key, string value)
        {
            foreach (var control in controlList)
            {
                //if (flag)
                control.AddData(key, value);
            }
        }
    }
}
