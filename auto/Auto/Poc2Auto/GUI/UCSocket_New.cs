using System;
using System.Windows.Forms;
using Poc2Auto.Model;
using Poc2Auto.Common;
using NetAndEvent.PlcDriver;

namespace Poc2Auto.GUI
{
    public partial class UCSocket_New : UserControl
    {
        public UCSocket_New()
        {
            InitializeComponent();
            //EventCenter.StationReset += StationReset;
            if (HandlerClient != null)
                HandlerClient.OnInitOk += () => { InitPLCSocketEnable(HandlerClient); };
            if (TesterClient!=null)
                TesterClient.OnInitOk += () => { InitPLCSocketEnable(TesterClient); };
        }
        private static AdsDriverClient HandlerClient = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Handler.ToString()) as AdsDriverClient;
        private static AdsDriverClient TesterClient = PlcDriverClientManager.GetInstance().GetPlcDriver(ModuleTypes.Tester.ToString()) as AdsDriverClient;
        private void ckbxSocket1_CheckedChanged(object sender, EventArgs e)
        { 
            ConfigMgr.Instance.Socket1Enable = ckbxSocket1.Checked;

            EnableSocket((int)StationName.PNP, ckbxSocket1.Checked);

            SocketEnableChanged(HandlerClient, 1, ckbxSocket1.Checked);
            SocketEnableChanged(TesterClient, 1, ckbxSocket1.Checked);
        }

        private void ckbxSocket2_CheckedChanged(object sender, EventArgs e)
        { 
            ConfigMgr.Instance.Socket2Enable = ckbxSocket2.Checked;

            EnableSocket((int)StationName.Test4_BMPF, ckbxSocket2.Checked);

            SocketEnableChanged(HandlerClient, 2, ckbxSocket2.Checked);
            SocketEnableChanged(TesterClient, 2, ckbxSocket2.Checked);
        }

        private void ckbxSocket3_CheckedChanged(object sender, EventArgs e)
        { 
            ConfigMgr.Instance.Socket3Enable = ckbxSocket3.Checked;

            EnableSocket((int)StationName.Test3_KYRL, ckbxSocket3.Checked);
            SocketEnableChanged(HandlerClient, 3, ckbxSocket3.Checked);
            SocketEnableChanged(TesterClient, 3, ckbxSocket3.Checked);
        }

        private void ckbxSocket4_CheckedChanged(object sender, EventArgs e)
        { 
            ConfigMgr.Instance.Socket4Enable = ckbxSocket4.Checked;

            EnableSocket((int)StationName.Test2_NFBP, ckbxSocket4.Checked);
            SocketEnableChanged(HandlerClient, 4, ckbxSocket4.Checked);
            SocketEnableChanged(TesterClient, 4, ckbxSocket4.Checked);
        }

        private void ckbxSocket5_CheckedChanged(object sender, EventArgs e)
        { 
            ConfigMgr.Instance.Socket5Enable = ckbxSocket5.Checked;

            EnableSocket((int)StationName.Test1_LIVW, ckbxSocket5.Checked);
            SocketEnableChanged(HandlerClient, 5, ckbxSocket5.Checked);
            SocketEnableChanged(TesterClient, 5, ckbxSocket5.Checked);
        }

        public bool Socket1Enable
        {
            get => ckbxSocket1.Checked;
            set
            {
                if (InvokeRequired)
                {
                    EnableSocket((int)StationName.PNP, value);
                    Invoke(new Action(() => Socket1Enable = value));
                    return;
                }
                ckbxSocket1.Checked = value;
                EnableSocket((int)StationName.PNP, ckbxSocket1.Checked);
            }
        }

        public bool Socket2Enable
        {
            get => ckbxSocket2.Checked;
            set
            {
                if (InvokeRequired)
                {
                    EnableSocket((int)StationName.Test4_BMPF, value);
                    Invoke(new Action(() => Socket2Enable = value));
                    return;
                }
                ckbxSocket2.Checked = value;
                EnableSocket((int)StationName.Test4_BMPF, value);
            }
        }

        public bool Socket3Enable
        {
            get => ckbxSocket3.Checked;
            set
            {
                if (InvokeRequired)
                {
                    EnableSocket((int)StationName.Test3_KYRL, value);
                    Invoke(new Action(() => Socket3Enable = value));
                    return;
                }
                ckbxSocket3.Checked = value;
                EnableSocket((int)StationName.Test3_KYRL, value);
            }
        }

        public bool Socket4Enable
        {
            get => ckbxSocket4.Checked;
            set
            {
                if (InvokeRequired)
                {
                    EnableSocket((int)StationName.Test2_NFBP, value);
                    Invoke(new Action(() => Socket4Enable = value));
                    return;
                }
                ckbxSocket4.Checked = value;
                EnableSocket((int)StationName.Test2_NFBP, value);
            }
        }

        public bool Socket5Enable
        {
            get => ckbxSocket5.Checked;
            set
            {
                if (InvokeRequired)
                {
                    EnableSocket((int)StationName.Test1_LIVW, value);
                    Invoke(new Action(() => Socket5Enable = value));
                    return;
                }
                ckbxSocket5.Checked = value;
                EnableSocket((int)StationName.Test1_LIVW, value);
            }
        }

        private void ReadCfg()
        {
            Socket1Enable = ConfigMgr.Instance.Socket1Enable;
            Socket2Enable = ConfigMgr.Instance.Socket2Enable;
            Socket3Enable = ConfigMgr.Instance.Socket3Enable;
            Socket4Enable = ConfigMgr.Instance.Socket4Enable;
            Socket5Enable = ConfigMgr.Instance.Socket5Enable;
        }

        private void UCSocket_New_Load(object sender, EventArgs e)
        {
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            ReadCfg();
        }

        private void StationReset()
        {
            StationManager.Stations[StationName.PNP].SocketGroup.Enable = ckbxSocket1.Checked;
            StationManager.Stations[StationName.Test4_BMPF].SocketGroup.Enable = ckbxSocket2.Checked;
            StationManager.Stations[StationName.Test3_KYRL].SocketGroup.Enable = ckbxSocket3.Checked;
            StationManager.Stations[StationName.Test2_NFBP].SocketGroup.Enable = ckbxSocket4.Checked;
            StationManager.Stations[StationName.Test1_LIVW].SocketGroup.Enable = ckbxSocket5.Checked;
        }

        private void EnableSocket(int enbaleIndex, bool enable)
        {
            foreach (var name in StationManager.RotationStations)
            {
                if (StationManager.Stations[name].SocketGroup.Index == enbaleIndex)
                {
                    StationManager.Stations[name].SocketGroup.Enable = enable;
                }
            }
        }

        private void InitPLCSocketEnable(AdsDriverClient client)
        {
            client?.WriteBool(RunModeMgr.DisableSocket(1), !ckbxSocket1.Checked); 
            client?.WriteBool(RunModeMgr.DisableSocket(2), !ckbxSocket2.Checked); 
            client?.WriteBool(RunModeMgr.DisableSocket(3), !ckbxSocket3.Checked); 
            client?.WriteBool(RunModeMgr.DisableSocket(4), !ckbxSocket4.Checked); 
            client?.WriteBool(RunModeMgr.DisableSocket(5), !ckbxSocket5.Checked);
        }

        private void SocketEnableChanged(AdsDriverClient client,int socketID, bool enable)
        {
            client?.WriteBool(RunModeMgr.DisableSocket(socketID), !enable);
        }
    }
}
