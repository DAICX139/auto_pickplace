using System;
using System.Windows.Forms;
using Poc2Auto.Model;
using Poc2Auto.Common;

namespace Poc2Auto.GUI
{
    public partial class UCSocket_New : UserControl
    {
        public UCSocket_New()
        {
            InitializeComponent();
            EventCenter.isReset += isRest;
        }

        private void ckbxSocket1_CheckedChanged(object sender, EventArgs e)
        {
            StationManager.Stations[StationName.Default].SocketGroup.Enable = ckbxSocket1.Checked;
            ConfigMgr.Instance.Socket1Enable = ckbxSocket1.Checked;
        }

        private void ckbxSocket2_CheckedChanged(object sender, EventArgs e)
        {
            StationManager.Stations[StationName.Test4_BMPF].SocketGroup.Enable = ckbxSocket2.Checked;

            ConfigMgr.Instance.Socket2Enable = ckbxSocket2.Checked;
        }

        private void ckbxSocket3_CheckedChanged(object sender, EventArgs e)
        {
            StationManager.Stations[StationName.Test3_Backup].SocketGroup.Enable = ckbxSocket3.Checked;


            ConfigMgr.Instance.Socket3Enable = ckbxSocket3.Checked;
        }

        private void ckbxSocket4_CheckedChanged(object sender, EventArgs e)
        {
            StationManager.Stations[StationName.Test2_DTGT].SocketGroup.Enable = ckbxSocket4.Checked;
            ConfigMgr.Instance.Socket4Enable = ckbxSocket4.Checked;
        }

        private void ckbxSocket5_CheckedChanged(object sender, EventArgs e)
        {
            StationManager.Stations[StationName.Test1_LIVW].SocketGroup.Enable = ckbxSocket5.Checked;
            ConfigMgr.Instance.Socket5Enable = ckbxSocket5.Checked;
        }

        public bool Socket1Enable
        {
            get => ckbxSocket1.Checked;
            set
            {
                if (InvokeRequired)
                {
                    StationManager.Stations[StationName.Default].SocketGroup.Enable = value;
                    Invoke(new Action(() => Socket1Enable = value));
                    return;
                }
                ckbxSocket1.Checked = value;
                StationManager.Stations[StationName.Default].SocketGroup.Enable = value;
            }
        }

        public bool Socket2Enable
        {
            get => ckbxSocket2.Checked;
            set
            {
                if (InvokeRequired)
                {
                    StationManager.Stations[StationName.Test4_BMPF].SocketGroup.Enable = value;
                    Invoke(new Action(() => Socket2Enable = value));
                    return;
                }
                ckbxSocket2.Checked = value;
                StationManager.Stations[StationName.Test4_BMPF].SocketGroup.Enable = value;
            }
        }

        public bool Socket3Enable
        {
            get => ckbxSocket3.Checked;
            set
            {
                if (InvokeRequired)
                {
                    StationManager.Stations[StationName.Test3_Backup].SocketGroup.Enable = value;
                    Invoke(new Action(() => Socket3Enable = value));
                    return;
                }
                ckbxSocket3.Checked = value;
                StationManager.Stations[StationName.Test3_Backup].SocketGroup.Enable = value;
            }
        }

        public bool Socket4Enable
        {
            get => ckbxSocket4.Checked;
            set
            {
                if (InvokeRequired)
                {
                    StationManager.Stations[StationName.Test2_DTGT].SocketGroup.Enable = value;
                    Invoke(new Action(() => Socket4Enable = value));
                    return;
                }
                ckbxSocket4.Checked = value;
                StationManager.Stations[StationName.Test2_DTGT].SocketGroup.Enable = value;
            }
        }

        public bool Socket5Enable
        {
            get => ckbxSocket5.Checked;
            set
            {
                if (InvokeRequired)
                {
                    StationManager.Stations[StationName.Test1_LIVW].SocketGroup.Enable = value;
                    Invoke(new Action(() => Socket5Enable = value));
                    return;
                }
                ckbxSocket5.Checked = value;
                StationManager.Stations[StationName.Test1_LIVW].SocketGroup.Enable = value;
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

        private void isRest()
        {
            StationManager.Stations[StationName.Default].SocketGroup.Enable = ckbxSocket1.Checked;
            StationManager.Stations[StationName.Test4_BMPF].SocketGroup.Enable = ckbxSocket2.Checked;
            StationManager.Stations[StationName.Test3_Backup].SocketGroup.Enable = ckbxSocket3.Checked;
            StationManager.Stations[StationName.Test2_DTGT].SocketGroup.Enable = ckbxSocket4.Checked;
            StationManager.Stations[StationName.Test1_LIVW].SocketGroup.Enable = ckbxSocket5.Checked;
        }
    }
}
