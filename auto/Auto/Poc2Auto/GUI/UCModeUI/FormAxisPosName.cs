using System;
using AlcUtility;
using System.Windows.Forms;

namespace Poc2Auto.GUI.UCModeUI
{
    public partial class FormAxisPosName : Form
    {
        public FormAxisPosName()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
            {
                AlcSystem.Instance.ShowMsgBox("Name cannot be empty!", "Error", icon:AlcMsgBoxIcon.Error);
                return;
            }

            LocationNameMgr.Name = txtName.Text;
            Dispose();
        }
    }
}
