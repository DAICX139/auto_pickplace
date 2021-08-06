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
    public partial class UC_SemiAuto : UserControl
    {
        const string DataType_String = "System.String";
        const string DataType_Int    = "System.Int";
        const string DataType_Double = "System.Double";
        const string DataType_Float  = "System.Float";
        const string DataType_UInt16 = "System.UInt16";

        public UC_SemiAuto()
        {
            InitializeComponent();
            ucPlcVarList1.TextChanged += (key, value) => { TextChanged.Invoke(key, value); };
        }

        IPlcDriver _plcDriver;

        public delegate void Excute(string key, string value);

        [Browsable(true)]
        /// <summary>
        /// 列表中文本改变时触发
        /// </summary>
        public new event Excute TextChanged;

        /// <summary>
        /// PlcDriver
        /// </summary>
        public IPlcDriver PlcDriver
        {
            get => _plcDriver;
            set
            {
                if (value == null) return;
                _plcDriver = value;
                ucSemiAutoCtrl1.PlcDriver = value;

                value.OnInitOk += () =>
                {
                    ucSemiAutoCtrl1.ObjDataSrc = value.GetSemiAutoCtrl(SemiAutoCtrlIndex);
                };
                if(value.IsInitOk)
                    ucSemiAutoCtrl1.ObjDataSrc = value.GetSemiAutoCtrl(SemiAutoCtrlIndex);
            }
        }

        [Browsable(true), Description("Action Name"), Category("SemiAuto Action配置")]
        /// <summary>
        /// ActionName
        /// </summary>
        public string ActionName
        {
            get => ucSemiAutoCtrl1.ActionName;
            set
            {
                if (InvokeRequired)
                    Invoke(new Action(() => { ucSemiAutoCtrl1.ActionName = value; }));
                else
                    ucSemiAutoCtrl1.ActionName = value;
            }
        }

        [Browsable(true), Description("SemiAutoCtrlIndex"), Category("SemiAuto Action配置")]
        /// <summary>
        /// SemiAutoCtrlIndex
        /// </summary>
        public int SemiAutoCtrlIndex { get; set; }

        /// <summary>
        /// DataSrc
        /// </summary>
        public ParamsModule DataSrc
        {
            set
            {
                if (value != null)
                {
                    ucSemiAutoCtrl1.ParamDataSrc = value;

                    //把配方中的数据写入列表中
                    foreach (var dic in ucSemiAutoCtrl1.ParamDataSrc.KeyValues)
                        ucPlcVarList1.LoadData(dic.Key, dic.Value.Value.ToString());
                }
            }
        }

        public void AddData(string key, string value)
        {
            ucPlcVarList1.AddData(key, value);
        }

        private void ucSemiAutoCtrl1_GetInputData(ParamsModule paramsModule)
        {
            if (paramsModule.KeyValues != null)
            {
                foreach (var dic in ucPlcVarList1.KeyValuePairs)
                {
                    if (paramsModule.KeyValues.ContainsKey(dic.Key))
                        if (!string.IsNullOrEmpty(dic.Value))
                        {
                            try
                            {
                                if (paramsModule.KeyValues[dic.Key].DataType == DataType_UInt16)
                                    paramsModule.KeyValues[dic.Key].Value = ushort.Parse(dic.Value);
                                else if (paramsModule.KeyValues[dic.Key].DataType == DataType_Int)
                                    paramsModule.KeyValues[dic.Key].Value = int.Parse(dic.Value);
                                else if (paramsModule.KeyValues[dic.Key].DataType == DataType_Double)
                                    paramsModule.KeyValues[dic.Key].Value = double.Parse(dic.Value);
                                else if (paramsModule.KeyValues[dic.Key].DataType == DataType_Float)
                                    paramsModule.KeyValues[dic.Key].Value = float.Parse(dic.Value);
                                else if (paramsModule.KeyValues[dic.Key].DataType == DataType_String)
                                    paramsModule.KeyValues[dic.Key].Value = dic.Value;
                            }
                            catch(Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }
                }
            }
        }
    }
}
