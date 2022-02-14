using System;
using System.Windows.Forms;
using Poc2Auto.Common;

namespace Poc2Auto.GUI
{
    public partial class FMInputBinValue : Form
    {
        public FMInputBinValue()
        {
            InitializeComponent();
            radioButton4.Checked = true;
        }

        string binValue;
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (binValue == "F")
            {
                binValue = "4";
            }
            EventCenter.BinValueInput?.Invoke(binValue);
            Dispose();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            binValue = radioButton1.Text.Substring(radioButton1.Text.Length-1, 1);
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            binValue = radioButton2.Text.Substring(radioButton1.Text.Length-1, 1);
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            binValue = radioButton3.Text.Substring(radioButton1.Text.Length-1, 1);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            binValue = radioButton4.Text.Substring(radioButton1.Text.Length-1, 1);
        }
    }
}
