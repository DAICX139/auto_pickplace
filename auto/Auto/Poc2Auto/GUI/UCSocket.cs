using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Poc2Auto.Model;
using Poc2Auto.Common;
using Newtonsoft.Json;

namespace Poc2Auto.GUI
{
    public partial class UCSocket : UserControl
    {
        public UCSocket()
        {
            InitializeComponent(); 
            initSocketGroup();
        }

        void initSocketGroup()
        {
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            //Id赋值
            for (int i = 0; i < StationManager.RotationStations.Count; i++)
            {
                var name = StationManager.RotationStations[i];
                cmbSocketId.Items.Add(name);
            }
            cmbSocketId.SelectedIndex = 0;

            listBoxDisable.DisplayMember = "DisplayMember";
        }

        /// <summary>
        /// 从配置文件上载disableSocketList
        /// </summary>
        public void Upload()
        {
            var disableList = JsonConvert.DeserializeObject<List<int>>(ConfigMgr.Instance.DisableSocketId);
            assignDisable(disableList);
        }

        void assignDisable(List<int> disablelist)
        {
            if (disablelist == null)
                return;
            listBoxDisable.Items.Clear();
            foreach (var station in StationManager.Stations)
            {
                if (station.Key != StationName.Load && station.Key != StationName.Unload)
                {
                    station.Value.SocketGroup.Enable = !disablelist.Contains(station.Value.SocketGroup.Index);
                    if (!station.Value.SocketGroup.Enable)
                        listBoxDisable.Items.Add(station.Value.SocketGroup);
                }
            }
        }

        #region EventHandler
        private void btnAddList_Click(object sender, EventArgs e)
        {
            var index = cmbSocketId.SelectedItem != null ? (int)(cmbSocketId.SelectedItem) : 0;

            //var locationId = Common.Common.LocationId(index, row, col);
            if (!listBoxDisable.Items.Contains(StationManager.Stations.ElementAt(index).Value.SocketGroup))
                listBoxDisable.Items.Add(StationManager.Stations.ElementAt(index).Value.SocketGroup);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<int> disablelist = new List<int>();
            foreach (var item in listBoxDisable.Items)
            {
                disablelist.Add((item as SocketGroup).Index);
            }

            assignDisable(disablelist);
            ConfigMgr.Instance.DisableSocketId = JsonConvert.SerializeObject(disablelist);
            EventCenter.SocketDisableChanged?.Invoke(disablelist);
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (listBoxDisable.SelectedIndex != -1)
                listBoxDisable.Items.RemoveAt(listBoxDisable.SelectedIndex);
        }

        #endregion
    }
}
