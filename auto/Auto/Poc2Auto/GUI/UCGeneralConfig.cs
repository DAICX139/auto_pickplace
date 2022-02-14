using Poc2Auto.Common;
using System;
using System.Windows.Forms;

namespace Poc2Auto.GUI
{
    public partial class UCGeneralConfig : UserControl
    {

        public bool NoSn
        {
            get => checkBoxNoSn.Checked;
            set
            {
                if (checkBoxNoSn.Checked == value) return;
                if(InvokeRequired)
                {
                    Invoke(new Action(() => NoSn = value));
                    return;
                }
                checkBoxNoSn.Checked = value;
            }
        }

        public bool AllOk
        {
            get => checkBoxAllOk.Checked;
            set
            {
                if (checkBoxAllOk.Checked == value) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => AllOk = value));
                    return;
                }
                checkBoxAllOk.Checked = value;
            }
        }
        public bool EnableMTCP
        {
            get => ckbxEnableMTCP.Checked;
            set
            {
                if (ckbxEnableMTCP.Checked == value) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => EnableMTCP = value));
                    return;
                }
                ckbxEnableMTCP.Checked = value;
            }
        }

        public bool TurnOffBuzzer
        {
            get => ckbxCloseBuzzer.Checked;
            set
            {
                if (ckbxCloseBuzzer.Checked == value) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => TurnOffBuzzer = value));
                    return;
                }
                ckbxCloseBuzzer.Checked = value;
            }
        }

        public bool WithTM
        {
            get => ckbxWithTm.Checked;
            set
            {
                if (ckbxWithTm.Checked == value) return;
                if (InvokeRequired)
                {
                    Invoke(new Action(() => WithTM = value));
                    return;
                }
                ckbxWithTm.Checked = value;
            }
        }


        public UCGeneralConfig()
        {
            InitializeComponent();
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            ucSocket1.Upload();
        }

        private void UCGeneralConfig_Load(object sender, EventArgs e)
        {
            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            RunModeMgr.CustomModeChanged += ReadCfg;
            ReadCfg();
        }

        private void ReadCfg()
        {
            NoSn = RunModeMgr.CustomMode.HasFlag(CustomMode.NoSn);
            AllOk = RunModeMgr.CustomMode.HasFlag(CustomMode.AllBinOk);
            EnableMTCP = ConfigMgr.Instance.EnableClientMTCP;
            TurnOffBuzzer = ConfigMgr.Instance.TurnOffBuzzer;
            WithTM = ConfigMgr.Instance.WithTM;
        }

        private void CheckBoxNoSn_CheckedChanged(object sender, EventArgs e)
        {
            if(NoSn)
            {
                RunModeMgr.CustomMode |= CustomMode.NoSn;
            }
            else
            {
                RunModeMgr.CustomMode &= ~CustomMode.NoSn;
            }
        }

        private void CheckBoxAllOk_CheckedChanged(object sender, EventArgs e)
        {
            if (AllOk)
            {
                RunModeMgr.CustomMode |= CustomMode.AllBinOk;
            }
            else
            {
                RunModeMgr.CustomMode &= ~CustomMode.AllBinOk;
            }
        }

        private void cbxEnableMTCP_CheckedChanged(object sender, EventArgs e)
        {
            ConfigMgr.Instance.EnableClientMTCP = ckbxEnableMTCP.Checked;
        }

        private void ckbxDutJudge_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ckbxCloseBuzzer_CheckedChanged(object sender, EventArgs e)
        {
            ConfigMgr.Instance.TurnOffBuzzer = ckbxCloseBuzzer.Checked;
            EventCenter.EnableBuzzer?.Invoke(ckbxCloseBuzzer.Checked);
        }

        private void ckbxWithTm_CheckedChanged(object sender, EventArgs e)
        {
            ConfigMgr.Instance.WithTM = ckbxWithTm.Checked;
        }
    }
}
