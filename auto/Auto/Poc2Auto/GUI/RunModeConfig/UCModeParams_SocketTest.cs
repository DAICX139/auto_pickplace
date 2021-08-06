using Poc2Auto.Common;
using System.Collections.Generic;
using System.Windows.Forms;
using CYGKit.GUI;

namespace Poc2Auto.GUI.RunModeConfig
{
    public partial class UCModeParams_SocketTest : UserControl
    {
        public UCModeParams_SocketTest()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            var trays = new List<TrayName> { TrayName.NG };
            cbxTrayID.DataSource = trays;
            cbxSocketID.BindEnum<SocketID>();
            comboBox1.BindEnum<StationID>();
        }

        public SocketUniformityTest ParamData
        {
            get
            {
                return new SocketUniformityTest
                {
                    TrayID = (int)cbxTrayID.SelectedItem,
                    TestTimes = (int)numTestTimes.Value,
                    TestRow = (int)numTestRow.Value,
                    TestCol = (int)numTestCol.Value,
                    SocketID = (int)cbxSocketID.SelectedValue,
                    StationID = (int)comboBox1.SelectedValue,
                };
            }
        }
    }

    enum SocketID
    {
        Socket1 = 1,
        Socket2,
        Socket3,
        Socket4,
        Socket5,
    }

    enum StationID
    {
        LIV = 1,
        DYN,
        BP,
    }
    

}
