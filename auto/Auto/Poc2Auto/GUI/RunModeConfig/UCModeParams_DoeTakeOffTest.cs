using System.Collections.Generic;
using System.Windows.Forms;
using Poc2Auto.Common;
using AlcUtility;
using Poc2Auto.Model;
using AlcUtility.Language;

namespace Poc2Auto.GUI.RunModeConfig
{
    public partial class UCModeParams_DoeTakeOffTest : UserControl
    {
        public static UCModeParams_DoeTakeOffTest Instance = new UCModeParams_DoeTakeOffTest();
        public UCModeParams_DoeTakeOffTest()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            var trays = new List<TrayName> { TrayName.NG };
            cbxTrayId.DataSource = trays;
        }

        public TakeOffParam TakeOffParam
        {
            get
            {
                return new TakeOffParam
                {
                    TrayID = (int)cbxTrayId.SelectedValue,
                    TestRow = (int)nbTestRow.Value,
                    TestCol = (int)nbTestCol.Value,
                    TestTimes = (int)nbTestTimes.Value,
                    IsCloseCap = ckxCloseCap.Checked,  
                    SocketID = (int)nmSocketID.Value,
                };
            }
        }
    }
}
