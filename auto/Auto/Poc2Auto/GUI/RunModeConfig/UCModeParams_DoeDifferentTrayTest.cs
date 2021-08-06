using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Poc2Auto.Common;
using AlcUtility;
using Poc2Auto.Model;
using AlcUtility.Language;

namespace Poc2Auto.GUI.RunModeConfig
{
    public partial class UCModeParams_DoeDifferentTrayTest : UserControl
    {
        public static UCModeParams_DoeDifferentTrayTest Instance = new UCModeParams_DoeDifferentTrayTest();
        /// <summary>
        /// 不同Tray测试配置文件路径
        /// </summary>
        public string DifferentTrayFilePath { get; set; }

        /// <summary>
        /// 不同Tray测试数据源
        /// </summary>
        public ParamsConfig DifferentTrayDataSrc { get; set; }
        public UCModeParams_DoeDifferentTrayTest()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            var trays = new List<TrayName> { TrayName.LoadL, TrayName.LoadR, };
            uC_GridviewTray1.comboxTrayName.DataSource = trays;

            uC_GridviewTray1.ShowCoordinate += (row, col) => { return TrayManager.ShowCoordinate(row, col, (TrayName)uC_GridviewTray1.comboxTrayName.SelectedItem); };
        }
        public DifferentTrayParam DifferentTrayParam
        {
            get
            {
                return new DifferentTrayParam
                {
                    LoadNum = uC_GridviewTray1.TrayDutNumber,
                    LoadTaryID = uC_GridviewTray1.TrayId,
                    LoadRegion = uC_GridviewTray1.TrayRegion,
                };
            }
        }

        public string CurrentLanguage { get; set; } = LangParser.DefaultLanguage;
        public void LangSwitch()
        {
            if (LangParser.CurrentLanguage == CurrentLanguage) return;
            LanguageSwitch.SetCtrl(Instance, LangParser.CurrentLanguage, CurrentLanguage);
            CurrentLanguage = LangParser.CurrentLanguage;
        }
    }
}
