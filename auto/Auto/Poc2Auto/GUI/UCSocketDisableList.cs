using System.Collections.Generic;
using System.Windows.Forms;
using Poc2Auto.Common;
using Newtonsoft.Json;
using Poc2Auto.Model;

namespace Poc2Auto.GUI
{
    public partial class UCSocketDisableList : UserControl
    {
        public UCSocketDisableList()
        {
            InitializeComponent();
            Upload();
            EventCenter.SocketDisableChanged += assignDisable;
            lbxDisableList.DisplayMember = "DisplayMember";
        }

        public void Upload()
        {
            var disableList = JsonConvert.DeserializeObject<List<int>>(ConfigMgr.Instance.DisableSocketId);
            assignDisable(disableList);
        }

        void assignDisable(List<int> disablelist)
        {
            if (disablelist == null)
                return;
            lbxDisableList.Items.Clear();
            foreach (var station in StationManager.Stations)
            {
                if (station.Key != StationName.Load && station.Key != StationName.Unload)
                {
                    station.Value.SocketGroup.Enable = !disablelist.Contains(station.Value.SocketGroup.Index);
                    if (!station.Value.SocketGroup.Enable)
                        lbxDisableList.Items.Add(station.Value.SocketGroup);
                }
            }
        }
    }
}
