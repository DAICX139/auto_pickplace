using Poc2Auto.Common;
using Poc2Auto.Database;
using Poc2Auto.Model;
using System.ComponentModel;
using System.Windows.Forms;

namespace Poc2Auto.GUI
{
    public partial class UCStation : UserControl
    {
        public UCStation()
        {
            InitializeComponent();
        }

        [Category("自定义")]
        public StationName StationName { get; set; }

        private void UCStation_Load(object sender, System.EventArgs e)
        {
            if (CYGKit.GUI.Common.IsDesignMode())
                return;

            var station = StationManager.Stations[StationName];
            uC_Station1.DataSource = station;
            uC_StationStat1.DataSource = station.Stat;
            uC_OnlineDut1.BindDataBase<DragonContext, OnlineDut>();
            uC_OnlineDut1.StationID = (int)StationName;
            uC_Station1.IsEnableChanged += () => { DragonDbHelper.SetStationEnable(StationName.ToString(), uC_Station1.ckb_IsEnable.Checked); };
        }
    }
}
