using CYGKit.Factory.DataBase;
using Poc2Auto.Common;
using Poc2Auto.Database;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Poc2Auto.GUI.RunModeConfig
{
    public partial class UCModeParams_SelectDut_Bin : UserControl
    {
        public UCModeParams_SelectDut_Bin()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            var unloadTrays = new List<TrayInfo>
                {
                    new TrayInfo
                    {
                        ID = (int)TrayName.NG + RunModeMgr.SelectTrayOffsetID,
                        Name = TrayName.NG.ToString()
                    },
                    //new TrayInfo
                    //{
                    //    ID = (int)TrayName.UnloadL + RunModeMgr.SelectTrayOffsetID,
                    //    Name = TrayName.UnloadL.ToString()
                    //},
                };
            ucTraySetting_New1.BindDataBase<DragonContext>(unloadTrays, true);
        }

        public AutoSelectBinParam ParamData
        {
            get
            {
                return new AutoSelectBinParam
                {
                    LoadNum = UCMain.Instance.IsLoadLTray ? UCMain.Instance.ucTrays1.LoadLRegionNumber : UCMain.Instance.ucTrays1.LoadRRegionNumber,
                    LoadTaryID = UCMain.Instance.IsLoadLTray ? (int)TrayName.Load1 : (int)TrayName.Load2,
                    LoadRegion = UCMain.Instance.IsLoadLTray ? UCMain.Instance.ucTrays1.LoadLTrayData : UCMain.Instance.ucTrays1.LoadRTrayData,
                    UnloadNum = ucTraySetting_New1.SelectionCount,
                    UnloadTrayID = ucTraySetting_New1.TrayID - RunModeMgr.SelectTrayOffsetID,
                    UnloadRegion = ucTraySetting_New1.Region2D
                };
            }
        }
    }
}
