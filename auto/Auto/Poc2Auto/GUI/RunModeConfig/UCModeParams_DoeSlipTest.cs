using System.Collections.Generic;
using System.Windows.Forms;
using Poc2Auto.Common;
using AlcUtility;
using Poc2Auto.Model;
using AlcUtility.Language;

namespace Poc2Auto.GUI.RunModeConfig
{
    public partial class UCModeParams_DoeSlipTest : UserControl
    {
        public static UCModeParams_DoeSlipTest Instance = new UCModeParams_DoeSlipTest();
        /// <summary>
        /// 滑移测试配置文件路径
        /// </summary>
        public string SlipFilePath { get; set; }

        public UCModeParams_DoeSlipTest()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            var trayIds = new List<TrayName> { TrayName.NG };
            var nozzles = new List<Nozzle> { Nozzle.LoadNozzle, Nozzle.UnloadNozzle };
            cbxTrayID.DataSource = trayIds;
            cbxTestNozzle.DataSource = nozzles;
        }

        public SlipParam SlipParam
        {
            get
            {
                double[,] aPos = new double[1,2];
                double[,] bPos = new double[1,2];
                aPos[0, 0] = (double)numAPosX.Value;
                aPos[0, 1] = (double)numAPosY.Value;
                bPos[0, 0] = (double)numBPosX.Value;
                bPos[0, 1] = (double)numBPosY.Value;
                return new SlipParam
                {
                    TaryID = (ushort)(TrayName)cbxTrayID.SelectedValue,
                    TestNozzle = (ushort)(Nozzle)cbxTestNozzle.SelectedValue,
                    ATestPos = aPos,
                    BTestPos = bPos,
                    TestNumber = (ushort)numTestTimes.Value,
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

    /// <summary>
    /// 测试吸嘴选择
    /// </summary>
    public enum Nozzle
    {
        LoadNozzle = 1,
        UnloadNozzle,
    }


}
