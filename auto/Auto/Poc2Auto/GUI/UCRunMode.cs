using System;
using System.Windows.Forms;
using AlcUtility;
using Poc2Auto.Common;
using System.ComponentModel;

namespace Poc2Auto.GUI
{
    public partial class UCRunMode : UserControl
    {
        private IPlcDriver _plcDriver;
        [Description("标题"), Category("自定义")]
        public string Title 
        { 
            get => labelTitle.Text;
            set
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => Title = value));
                    return;
                }
                labelTitle.Text = value;
            }
        }
        public IPlcDriver PlcDriver
        {
            set
            {
                if (value == null)
                    return;
                _plcDriver = value;
            }
        }
        public UCRunMode()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
        }

        public event Action<PlcMode> ModeChanged;
        private void update()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(update));
                return;
            }

            if (_plcDriver == null)
                return;
            PlcMode mode = PlcMode.Invalid;
            if (_plcDriver.IsInitOk && _plcDriver.IsConnected)
            {
                mode = (PlcMode)(uint)_plcDriver.ReadObject("GVL_MachineInterface.MachineCmd.nMode", typeof(uint));
            }
            labelRunMode.Text = mode.ToString();
            ModeChanged?.Invoke(mode);
        }

        private void timerSync_Tick(object sender, EventArgs e)
        {
            update();
        }
    }
}
