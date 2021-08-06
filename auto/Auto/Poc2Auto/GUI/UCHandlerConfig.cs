using AlcUtility;
using CYGKit.AdsProtocol;
using CYGKit.Factory.OtherUI;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using Poc2Auto.Database;
using Poc2Auto.Model;
using System;
using System.Windows.Forms;

namespace Poc2Auto.GUI
{
    public partial class UCHandlerConfig : UserControl
    {
        private AdsDriverClient _client;

        public UCHandlerConfig(AdsDriverClient client)
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            _client = client;
            uC_BinRegion1.BindDataBase<DragonContext>();
            uC_BinRegion1.Client = client;
            ucModeConfig1.BindClient(_client);
        }

        private void HandlerConfig_Load(object sender, EventArgs e)
        {
            uC_BinRegion1.Dock = DockStyle.Fill;
            ucGeneralConfig1.tabPageBinRegion.Controls.Add(uC_BinRegion1);

            if (_client != null)
            {
                _client.OnInitOk += Client_OnInitOk;
                _client.OnDisconnected += Client_OnDisconnected;
                SetEnable(_client.IsInitOk);
            } 
        }

        private void Client_OnDisconnected()
        {
            if(InvokeRequired)
            {
                Invoke(new Action(Client_OnDisconnected));
                return;
            }
            SetEnable(false);
        }

        private void Client_OnInitOk()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(Client_OnInitOk));
                return;
            }
            SetEnable(true);
        }

        private void SetEnable(bool enable)
        {
        }

        private void ButtonOkClicked(TraySelectionInfo selectedInfo)
        {
            var index = selectedInfo.LoadTrayId;
            var data = selectedInfo.LoadTrayRegion;
            var result = _client.WriteTrayData(index, data, out var message);
            if (!result)
            {
                AlcSystem.Instance.Error($"Tray盘数据下发失败：{message}", 0, AlcErrorLevel.WARN, "Handler");
            }
            else
            {
                DragonDbHelper.ClearTrayData((int)(TrayName)index);
                TrayManager.Trays[(TrayName)index].SetData(data);
                AlcSystem.Instance.ShowMsgBox("Tray盘数据下发成功", "PLC");
            }
        }
    }
}
