using AlcUtility.PlcDriver.CommonCtrl;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Poc2Auto.GUI.UCModeUI.UCAxisesCylinders
{
    public partial class UC_Cylinder_New : UserControl
    {
        public UC_Cylinder_New()
        {
            InitializeComponent();
            timer1.Tick += (e, sender) => { updateStatus(); };
        }

        public CylinderCtrl Cylinder { get; set; }

        public bool AuthorityCtrl
        {
            set
            {
                if (InvokeRequired)
                    Invoke(new Action(() => { authorityCtrl(value); }));
                else
                    authorityCtrl(value);
            }
        }

        private void authorityCtrl(bool enable)
        {
            button1.Enabled = enable;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (Cylinder == null)
                return;
            if (Cylinder.Info.IsBase)
                Cylinder.MoveToWork();
            else if (Cylinder.Info.IsWork)
            {
                if (Cylinder.Info.Name == "LoadVacuum" || Cylinder.Info.Name == "UloadVacuum" || Cylinder.Info.Name == "SecondVacuum")
                {
                    Cylinder.MoveToBase();
                    Cylinder.MoveToNone();
                }
                else
                    Cylinder.MoveToBase();
            }
                
            else if (Cylinder.Info.IsError)
            {
                Cylinder.MoveToNone();
                Cylinder.Reset();
            }
            else
                Cylinder.MoveToWork();


        }

        private void updateStatus()
        {
            if (Cylinder == null)
                return;
            var info = Cylinder.Info;
            button1.Text = info.Name;
            label1.BackColor = GetColor(info);
        }

        private Color GetColor(PLCCylInfo info)
        {
            if (info.IsBase)
                return Color.Blue;
            else if (info.IsWork)
                return Color.Green;
            else if (info.IsError)
                return Color.Red;
            else
                return Color.White;
        }

        public bool EnableUpdate { set
            {
                if (InvokeRequired)
                    Invoke(new Action(() => timer1.Enabled = value));
                else
                    timer1.Enabled = value;
            }
        }

        public bool EnableUI
        {
            set
            {
                if (InvokeRequired)
                    Invoke(new Action(() => button1.Enabled = value));
                else
                    button1.Enabled = value;
            }
        }
    }
}
