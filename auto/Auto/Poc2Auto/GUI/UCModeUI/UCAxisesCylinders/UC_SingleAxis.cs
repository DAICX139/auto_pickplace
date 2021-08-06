using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlcUtility.PlcDriver.CommonCtrl;

namespace DragonFlex.GUI.Factory.UC_handlePLC
{
    public partial class UC_SingleAxis : UserControl
    {
        public UC_SingleAxis()
        {
            InitializeComponent();
            timer1.Tick += (e, sender) => { updateStatus(); };
        }

        public SingleAxisCtrl SingleAxis { get; set; }

        public string AxisName { 
            get => labelName.Text;
            set {
                if (string.IsNullOrEmpty(value))
                    return;
                if (InvokeRequired)
                    Invoke(new Action(() => labelName.Text = value));
                else
                    labelName.Text = value;
            }
        }

        public double Pos
        {
            get
            {
                if (cBoxPos.SelectedValue == null)
                    if (double.TryParse(cBoxPos.Text, out double result))
                        return result;
                    else
                        return 0;
                else
                    return (double)cBoxPos.SelectedValue;
            }
        }
        public double Speed { get
            {
                if (double.TryParse(textBoxSpeed.Text, out double result))
                    return result;
                else
                    return 0;
            }
        }

        public bool EnableUpdate { set
            {
                if (InvokeRequired)
                    Invoke(new Action(() => timer1.Enabled = value));
                else
                    timer1.Enabled = true;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (SingleAxis == null)
                return;
           var ret = SingleAxis.PowerCtrl(checkBox1.Checked);
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (SingleAxis == null)
                return;
            var ret = SingleAxis.AbsGo(Pos,Speed);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (SingleAxis == null)
                return;
            var ret = SingleAxis.GoHome(Speed);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (SingleAxis == null)
                return;
            var ret = SingleAxis.Stop();
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            if (SingleAxis == null)
                return;
            var ret = SingleAxis.Reset();
        }

        private void btnJogGo_MouseDown(object sender, MouseEventArgs e)
        {
            if (SingleAxis == null)
                return;
            var ret = SingleAxis.JogGo(Speed,true);
        }

        private void btnJogGo_MouseUp(object sender, MouseEventArgs e)
        {
            if (SingleAxis == null)
                return;
            var ret = SingleAxis.JogGo(Speed, false);
        }

        private void btnJogBack_MouseDown(object sender, MouseEventArgs e)
        {
            if (SingleAxis == null)
                return;
            var ret = SingleAxis.JogGo(Speed*-1,true);
        }

        private void btnJogBack_MouseUp(object sender, MouseEventArgs e)
        {
            if (SingleAxis == null)
                return;
            var ret = SingleAxis.JogGo(Speed * -1, false);
        }

        private void updateStatus()
        {
            if (SingleAxis == null)
                return;
            labelCurrent.Text = SingleAxis.Info.ActPos.ToString();
            labelDone.BackColor = SingleAxis.IsDone == true ? Color.Green : Color.White;
            labelError.BackColor = SingleAxis.Info.Error == true ? Color.Red : Color.White;
            labelHome.BackColor = SingleAxis.Info.Homed == true ? Color.Green : Color.White;
            labelMove.BackColor = SingleAxis.Info.Moving == true ? Color.Green : Color.White;
            labelEnable.BackColor = SingleAxis.Info.PowerStatus == true ? Color.Green : Color.White;
            labelName.Text = SingleAxis.Info.Name;
        }

        public bool EnableUI { set
            {
                if (InvokeRequired)
                    Invoke(new Action(() =>
                    {
                        btnGo.Enabled = value;
                        btnHome.Enabled = value;
                        btnJogBack.Enabled = value;
                        btnJogGo.Enabled = value;
                        btnStop.Enabled = value;
                        btnReset.Enabled = value;
                    }));
                else
                {
                    btnGo.Enabled = value;
                    btnHome.Enabled = value;
                    btnJogBack.Enabled = value;
                    btnJogGo.Enabled = value;
                    btnStop.Enabled = value;
                    btnReset.Enabled = value;
                }
            }
        }

        public void LoadPositions<T>(List<T> data, string displayMember, string valueMember)
        {
            if (data == null)
                return;
            cBoxPos.DisplayMember = displayMember;
            cBoxPos.ValueMember = valueMember;
            cBoxPos.DataSource = data;
        }

        public object CurrentSelectItem
        {
            get
            {
                return cBoxPos.SelectedItem;
            }
            set
            {
                if (value == null)
                    return;
                cBoxPos.SelectedItem = value;
            }
        }
    }
}
