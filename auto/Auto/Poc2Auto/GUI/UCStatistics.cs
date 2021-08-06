using System;
using System.Windows.Forms;
using AlcUtility;
using Poc2Auto.Database;
using Poc2Auto.Model;

namespace Poc2Auto.GUI
{
    public partial class UCStatistics : UserControl
    {
        public UCStatistics()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            InitSocketGroup();
            Overall.InitOk += InitBinStat;

            //权限管理
            authorityManagement();
            AlcSystem.Instance.UserAuthorityChanged += (o, n) => { authorityManagement(); };
        }

        private void InitSocketGroup()
        {
            //Id赋值
            for (int i = 0; i < StationManager.RotationStations.Count; i++)
            {
                var name = StationManager.RotationStations[i];
                cmbSocketId.Items.Add(StationManager.Stations[name].SocketGroup.Index - 1);
            }
            cmbSocketId.SelectedIndex = 0;
        }

        private void InitBinStat()
        {
            uC_LotBinStatDB1.BindDataBase<DragonContext>();
            uC_StationLotBinStat1.BindDataBase<DragonContext>();
            Overall.LotInfoChanged += () => {
                uC_LotBinStatDB1.CurrentLotId = Overall.LotInfo?.LotID;
                uC_StationLotBinStat1.CurrentLotId = Overall.LotInfo?.LotID;
            };
            uC_LotBinStatDB1.CurrentLotId = Overall.LotInfo?.LotID;
            uC_StationLotBinStat1.CurrentLotId = Overall.LotInfo?.LotID;
        }

        private void cmbSocketId_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var socketId = (int)cmbSocketId.SelectedItem; 
            if (SocketManager.Sockets.ContainsKey(socketId + 1))
                uC_SocketStat1.DataSource = SocketManager.Sockets[socketId + 1].Stat;        
        }

        private void authorityManagement()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(authorityManagement));
                return;
            }
            uC_SocketStat1.AuthorityCtrl = !(AlcSystem.Instance.GetUserAuthority() == UserAuthority.OPERATOR.ToString()); 
        }
    }
}
