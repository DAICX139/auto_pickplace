using System.Collections.Generic;
using System.Windows.Forms;
using Poc2Auto.Common;
using AlcUtility;
using Poc2Auto.Model;
using AlcUtility.Language;


namespace Poc2Auto.GUI.RunModeConfig
{
    public partial class UCModeParams_DoeSameTrayTest : UserControl
    {
        public static UCModeParams_DoeSameTrayTest Instance = new UCModeParams_DoeSameTrayTest();
        /// <summary>
        /// 同Tray测试配置文件路径
        /// </summary>
        public string SameTrayFilePath { get; set; }

        /// <summary>
        /// 同Tray测试数据源
        /// </summary>
        //public ParamsConfig SameTrayDataSrc { get; set; }

        //public Dictionary<string, string> KeyValues { get => uC_XmlList1.KeyValuePairs; }
        public UCModeParams_DoeSameTrayTest()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            var trays = new List<TrayName> { TrayName.LoadL, TrayName.LoadR, TrayName.NG, TrayName.UnloadL, TrayName.UnloadR };
            uC_GridviewTray1.comboxTrayName.DataSource = trays;
            uC_GridviewTray1.ShowCoordinate += (row, col) => { return TrayManager.ShowCoordinate(row, col, (TrayName)uC_GridviewTray1.comboxTrayName.SelectedItem);};
        }

        public SameTrayParam SameTrayParam
        {
            get
            {
                return new SameTrayParam
                {
                    TaryID = uC_GridviewTray1.TrayId,
                    TrayRegion = uC_GridviewTray1.TrayRegion,
                    StartRow = (ushort)(uC_GridviewTray1.Tray.SelectedRegion.Top + 1),
                    StartCol = (ushort)(uC_GridviewTray1.Tray.SelectedRegion.Left + 1),
                };
            }
        }
    }
}
