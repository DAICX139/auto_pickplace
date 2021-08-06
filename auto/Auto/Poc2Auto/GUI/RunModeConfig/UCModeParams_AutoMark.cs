using System.Windows.Forms;
using Poc2Auto.Common;
using System.Collections.Generic;
using AlcUtility.Language;

namespace Poc2Auto.GUI.RunModeConfig
{
    public partial class UCModeParams_AutoMark : UserControl
    {
        List<TrayName> loadVisionTray = new List<TrayName> { TrayName.LoadL, TrayName.LoadR, TrayName.NG };
        List<TrayName> unLoadVisionTray = new List<TrayName> { TrayName.NG, TrayName.UnloadL, TrayName.UnloadR };

        public UCModeParams_AutoMark()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            rdbtnCompleteProcessMark.Checked = true;
            groupBox2.Enabled = rdbtnSpecifyTrayMark.Checked;
            groupBox3.Enabled = rdbtnSpecifyTrayMark.Checked;
            rdbtnLoadMark.Checked = true;
            cbxMarkTrayId.DataSource = loadVisionTray;
        }

        public AutoMark AutoMarkParam
        {
            get
            {
                return new AutoMark
                {
                    MarkMode = rdbtnCompleteProcessMark.Checked ? 0 : 1,
                    MarkTrayChoose = rdbtnLoadMark.Checked ? 0 : 1,
                    MarkTrayId = (int)(TrayName)cbxMarkTrayId.SelectedValue,
                };
            }
        }

        private void rdbtnSpecifyTrayMark_CheckedChanged(object sender, System.EventArgs e)
        {
            groupBox2.Enabled = rdbtnSpecifyTrayMark.Checked;
            groupBox3.Enabled = rdbtnSpecifyTrayMark.Checked;
        }

        private void rdbtnLoadMark_CheckedChanged(object sender, System.EventArgs e)
        {
            cbxMarkTrayId.DataSource = rdbtnLoadMark.Checked ? loadVisionTray : unLoadVisionTray;
            
        }

        private void rdbtnUnLoadMark_CheckedChanged(object sender, System.EventArgs e)
        {
            cbxMarkTrayId.DataSource = rdbtnUnLoadMark.Checked ? unLoadVisionTray : loadVisionTray;
        }
    }
}
