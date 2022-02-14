using AlcUtility.PlcDriver.CommonCtrl;
using System;
using System.Drawing;
using System.Windows.Forms;
using AlcUtility.Language;

namespace Poc2Auto.GUI.UCModeUI.UCAxisesCylinders
{
    public partial class UC_Cylinder_New : UserControl
    {
        public UC_Cylinder_New()
        {
            InitializeComponent();
            timer1.Tick += (e, sender) => { updateStatus(); };
        }

        const string cn = "zh-CN";
        const string en = "en-US";
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
            if (Cylinder.Info == null)
            {
                return;
            }
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
            try
            {
                if (Cylinder == null)
                    return;
                var lg = LangParser.CurrentLanguage;
                var info = Cylinder.Info;
                if (info == null)
                {
                    return;
                }
                button1.Text = info.Name;
                var color = GetColor(info);
                label1.BackColor = color;
                if (Color.Red == color)
                {
                    label1.ForeColor = Color.White;
                    if (cn == lg)
                    {
                        label1.Text = "报错";
                    }
                    else if (en == lg)
                    {
                        label1.Text = "Error";
                    }
                    else
                    {
                        label1.Text = "报错";
                    }

                }
                else if (Color.Blue == color)
                {
                    label1.ForeColor = Color.White;
                    if (cn == lg)
                    {
                        label1.Text = "基础位";
                    }
                    else if (en == lg)
                    {
                        label1.Text = "Base";
                    }
                    else
                    {
                        label1.Text = "基础位";
                    }


                }
                else if (Color.Green == color)
                {
                    label1.ForeColor = Color.White;
                    if (cn == lg)
                    {
                        label1.Text = "工作位";
                    }
                    else if (en == lg)
                    {
                        label1.Text = "Work";
                    }
                    else
                    {
                        label1.Text = "工作位";
                    }

                }
                else
                {
                    label1.ForeColor = Color.Black;
                    if (cn == lg)
                    {
                        label1.Text = "关闭输出";
                    }
                    else if (en == lg)
                    {
                        label1.Text = "None";
                    }
                    else
                    {
                        label1.Text = "关闭输出";
                    }
                }
            }
            catch (Exception ex)
            {

            }
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
