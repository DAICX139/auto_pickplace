using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AlcUtility;
using Poc2Auto.Common;

namespace Poc2Auto.GUI.UCModeUI
{
    public partial class FormOperationAxis : Form
    {
        public FormOperationAxis()
        {
            InitializeComponent();
            _timer = new System.Timers.Timer()
            {
                Interval = 200,
                Enabled = false,
            };
            _timer.Elapsed += sysTimer_Tick;
            EventCenter.OnHandlerPLCDisconnect += () => { _timer.Enabled = false; isConnected = false; };
        }

        #region Fields
        private IPlcDriver _plcDriver;
        private bool isConnected = true;
        private Dictionary<string, int> AxisNames;

        private System.Timers.Timer _timer;
        public IPlcDriver PlcDriver
        {
            private get => _plcDriver;
            set
            {
                if (value == null)
                    return;
                _plcDriver = value;
                AxisNames = new Dictionary<string, int>();
                var name =  getAxisNameDic(value);
                if (name != null)
                {
                    AxisNames = name;
                    var axisList = name.Keys.ToList();
                    cbxAxisName.DataSource = axisList;
                    _timer.Enabled = true;
                }
            }
        }
        public int AxisCount { get; set; }

        #endregion

        private void FormOperationAxis_Load(object sender, EventArgs e)
        {
            
        }

        private Dictionary<string, int> getAxisNameDic(IPlcDriver plcDriver)
        {
            if (plcDriver == null)
                return null;
            var axisName = new Dictionary<string, int>();
            for (int i = 1;  i <= AxisCount; i++)
            {
                axisName.Add(_plcDriver.GetSingleAxisCtrl(i).Info.Name, i);
            }
            return axisName;
        }

        private void cbxAxisName_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var name in AxisNames)
            {
                if (cbxAxisName.SelectedItem.ToString() == name.Key)
                {
                    var Info = _plcDriver?.GetSingleAxisCtrl(name.Value).Info;
                    if (Info == null)
                        return;
                    ckxAxisEnable.Checked = Info.PowerStatus;
                    if (Info.ActPos > -500 && Info.ActPos < 1000)
                    {
                        var vel = $"{Info.ActPos:N4}";
                        if (string.IsNullOrEmpty(vel))
                            return;
                        numAxisTarPos.Value = decimal.Parse(vel);
                    }
                }
            }

        }

        private void btnYAxisJogAdd_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var name in AxisNames)
            {
                if (name.Key.Contains("Y") || name.Key.Contains("y"))
                {
                    _plcDriver?.GetSingleAxisCtrl(name.Value)?.JogGo(Math.Abs(double.Parse(numAxisVel.Value.ToString())), true);
                    break;
                }
            }
        }

        private void btnYAxisJogAdd_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (var name in AxisNames)
            {
                if (name.Key.Contains("Y") || name.Key.Contains("y"))
                {
                    _plcDriver?.GetSingleAxisCtrl(name.Value)?.JogGo(Math.Abs(double.Parse(numAxisVel.Value.ToString())), false);
                    break;
                }
            }
        }

        private void btnYAxisJogSub_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var name in AxisNames)
            {
                if (name.Key.Contains("Y") || name.Key.Contains("y"))
                {
                    _plcDriver?.GetSingleAxisCtrl(name.Value)?.JogGo(Math.Abs(double.Parse(numAxisVel.Value.ToString())) * -1, true);
                    break;
                }
            }
        }

        private void btnYAxisJogSub_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (var name in AxisNames)
            {
                if (name.Key.Contains("Y") || name.Key.Contains("y"))
                {
                    _plcDriver?.GetSingleAxisCtrl(name.Value)?.JogGo(Math.Abs(double.Parse(numAxisVel.Value.ToString())) * -1, false);
                    break;
                }
            }
        }

        private void btnXAxisJogAdd_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var name in AxisNames)
            {
                if (name.Key.Contains("X") || name.Key.Contains("x"))
                {
                    _plcDriver?.GetSingleAxisCtrl(name.Value)?.JogGo(Math.Abs(double.Parse(numAxisVel.Value.ToString())), true);
                    break;
                }
            }
        }

        private void btnXAxisJogAdd_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (var name in AxisNames)
            {
                if (name.Key.Contains("X") || name.Key.Contains("x"))
                {
                    _plcDriver?.GetSingleAxisCtrl(name.Value)?.JogGo(Math.Abs(double.Parse(numAxisVel.Value.ToString())), false);
                    break;
                }
            }
        }

        private void btnXAxisJogSub_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var name in AxisNames)
            {
                if (name.Key.Contains("X") || name.Key.Contains("x"))
                {
                    _plcDriver?.GetSingleAxisCtrl(name.Value)?.JogGo(Math.Abs(double.Parse(numAxisVel.Value.ToString())) * -1, true);
                    break;
                }
            }
        }

        private void btnXAxisJogSub_MouseUp(object sender, MouseEventArgs e)
        {
            foreach (var name in AxisNames)
            {
                if (name.Key.Contains("X") || name.Key.Contains("x"))
                {
                    _plcDriver?.GetSingleAxisCtrl(name.Value)?.JogGo(Math.Abs(double.Parse(numAxisVel.Value.ToString())) * -1, false);
                    break;
                }
            }
        }

        private void ckxAxisEnable_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var name in AxisNames)
            {
                if (cbxAxisName.SelectedItem.ToString() == name.Key)
                {
                    _plcDriver?.GetSingleAxisCtrl(name.Value)?.PowerCtrl(ckxAxisEnable.Checked);
                }
            }
        }

        private void sysTimer_Tick(object sender, EventArgs e)
        {
            //if (InvokeRequired)
            //{
            //    Invoke(new Action( () => { sysTimer_Tick(sender, e); }) );
            //    return;
            //}
            if (_plcDriver == null)
                return;
            foreach (var name in AxisNames)
            {
                if (!isConnected)
                    return;
                if (_plcDriver.GetSingleAxisCtrl(name.Value).Info.Moving)
                {
                    var pos = $"{ _plcDriver?.GetSingleAxisCtrl(name.Value).Info.ActPos:N4}";
                    lbAxisPos.Text = pos;
                    break;
                }
            }
        }

        private void btnAbsGo_Click(object sender, EventArgs e)
        {
            foreach (var name in AxisNames)
            {
                if (cbxAxisName.SelectedItem.ToString() == name.Key)
                {
                    _plcDriver?.GetSingleAxisCtrl(name.Value)?.AbsGo(double.Parse(numAxisTarPos.Value.ToString()), Math.Abs(double.Parse(numAxisVel.Value.ToString())));
                    break;
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            foreach (var name in AxisNames)
            {
                if (cbxAxisName.SelectedItem.ToString() == name.Key)
                {
                    _plcDriver?.GetSingleAxisCtrl(name.Value)?.Stop();
                    break;
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach (var name in AxisNames)
            {
                if (cbxAxisName.SelectedItem.ToString() == name.Key)
                {
                    _plcDriver?.GetSingleAxisCtrl(name.Value)?.Reset();
                    break;
                }
            }
        }

        private void btnUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (cbxAxisName.SelectedItem == null)
                return;
            if (cbxAxisName.SelectedItem.ToString().Contains("Z") || cbxAxisName.SelectedItem.ToString().Contains("z"))
            {
                foreach (var name in AxisNames)
                {
                    if (cbxAxisName.SelectedItem.ToString() == name.Key)
                    {
                        _plcDriver?.GetSingleAxisCtrl(name.Value)?.JogGo(Math.Abs(double.Parse(numAxisVel.Value.ToString())) * -1, true);
                        break;
                    }
                }
            }
        }

        private void btnUp_MouseUp(object sender, MouseEventArgs e)
        {
            if (cbxAxisName.SelectedItem == null)
                return;
            if (cbxAxisName.SelectedItem.ToString().Contains("Z") || cbxAxisName.SelectedItem.ToString().Contains("z"))
            {
                foreach (var name in AxisNames)
                {
                    if (cbxAxisName.SelectedItem.ToString() == name.Key)
                    {
                        _plcDriver?.GetSingleAxisCtrl(name.Value)?.JogGo(Math.Abs(double.Parse(numAxisVel.Value.ToString())) * -1, false);
                        break;
                    }
                }
            }
        }

        private void btnDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (cbxAxisName.SelectedItem == null)
                return;
            if (cbxAxisName.SelectedItem.ToString().Contains("Z") || cbxAxisName.SelectedItem.ToString().Contains("z"))
            {
                foreach (var name in AxisNames)
                {
                    if (cbxAxisName.SelectedItem.ToString() == name.Key)
                    {
                        _plcDriver?.GetSingleAxisCtrl(name.Value)?.JogGo(Math.Abs(double.Parse(numAxisVel.Value.ToString())), true);
                        break;
                    }
                }
            }
        }

        private void btnDown_MouseUp(object sender, MouseEventArgs e)
        {
            if (cbxAxisName.SelectedItem == null)
                return;
            if (cbxAxisName.SelectedItem.ToString().Contains("Z") || cbxAxisName.SelectedItem.ToString().Contains("z"))
            {
                foreach (var name in AxisNames)
                {
                    if (cbxAxisName.SelectedItem.ToString() == name.Key)
                    {
                        _plcDriver?.GetSingleAxisCtrl(name.Value)?.JogGo(Math.Abs(double.Parse(numAxisVel.Value.ToString())), false);
                        break;
                    }
                }
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            foreach (var name in AxisNames)
            {
                if (cbxAxisName.SelectedItem.ToString() == name.Key)
                {
                    _plcDriver?.GetSingleAxisCtrl(name.Value)?.GoHome(double.Parse(numAxisVel.Value.ToString()));
                    break;
                }
            }
        }
    }
}
