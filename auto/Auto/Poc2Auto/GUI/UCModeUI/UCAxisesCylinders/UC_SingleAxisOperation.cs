using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using AlcUtility.PlcDriver.CommonCtrl;
using AlcUtility.PlcDriver;
using AlcUtility;

namespace Poc2Auto.GUI.UCModeUI.UCAxisesCylinders
{
    public partial class UC_SingleAxisOperation : UserControl,ICommonCtrlUI
    {
        public UC_SingleAxisOperation()
        {
            InitializeComponent();
            init();
        }

        #region Filed

        /// <summary>
        /// ListBox当前选中的项
        /// </summary>
        private ParamsValue _currentSelectItem;
        /// <summary>
        /// 是否阻塞
        /// </summary>
        private bool _isBlock = false;
        /// <summary>
        /// 轴运动默认速度
        /// </summary>
        private const double _defaultVelocity = 20;
        
        private SingleAxisCtrl _dataSource;

        private double _goHomeVel = 20;

        /// <summary>
        /// 从配方文件中获取到的当前轴的位置参数信息
        /// </summary>
        private List<ParamsValue> AxisPosInfo = new List<ParamsValue>();

        #endregion Filed

        #region Property

        /// <summary>
        /// 轴回原速度设置
        /// </summary>
        public double GoHomeVel
        {
            private get => _goHomeVel;
            set
            {
                if (value > 0)
                {
                    _goHomeVel = value;
                }
            }
        }

        public PLCSingleAxisInfo Info { get; private set; }
        
        private ParamsConfig _config;
        /// <summary>
        /// 配方数据
        /// </summary>
        public ParamsConfig Config
        {
            get { return _config; }
            set 
            {
                if (value == null)
                    return;
                _config = value;

                foreach (var module in _config.ParamsModules.Values)
                {
                    foreach (var val in module.KeyValues.Values)
                    {
                        if (val.Remark.Contains("速度") || val.Remark.Contains("间距") || val.Remark.Contains("长度") || val.Remark.Contains("行数") || val.Remark.Contains("列数"))
                            continue;
                        if (val.Key.Contains(string.IsNullOrEmpty(Info?.Name) ? AxisName : Info.Name))
                        {
                            AxisPosInfo.Add(val);
                        }
                    }
                }
                ListBoxDisplay.BeginInvoke(new Action(() =>
                {
                    ListBoxDisplay.DataSource = AxisPosInfo;
                }));
            }
        }

        /// <summary>
        /// 当前轴的Name
        /// </summary>
        [Description("标题"), Category("自定义")]
        public string AxisName { get; set; }
        /// <summary>
        /// 标题设置属性
        /// </summary>
        [Description("标题"), Category("自定义")]
        public string Title
        {
            get => LabelTitle.Text;
            set
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => Title = value));
                    return;
                }
                LabelTitle.Text = value;
            }
        }

        /// <summary>
        /// 标题显示属性
        /// </summary>
        [DefaultValue(true), Description("是否显示标题"), Category("自定义")]
        public bool ShowTitle
        {
            get => Title != null;
            set
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => ShowTitle = value));
                    return;
                }
                LabelTitle.Text = value == true ? Title : "";
            }
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public object DataSource
        {
            get => _dataSource;
            set
            {
                if (null == value)
                    return;
                _dataSource = value as SingleAxisCtrl;
            }
        }

        private void authorityCtrl(bool enable)
        {
            BtnGoTo.Enabled = enable;
            BtnHome.Enabled = enable;
            BtnJogAdd.Enabled = enable;
            BtnJogSub.Enabled = enable;
            BtnReset.Enabled = enable;
            BtnStop.Enabled = enable;
            CheckIsEnable.Enabled = enable;
            TextTargetPos.Enabled = enable;
            TextRunVeloctity.Enabled = enable;
        }

        [DefaultValue(true), Description("是否开启内部刷新"), Category("自定义")]
        public bool IsInnerUpdatingOpen
        { 
            get => timer1.Enabled;
            set
            {
                if (InvokeRequired)
                    Invoke(new Action(() => timer1.Enabled = value));
                else
                    timer1.Enabled = value;
            }

        }

        [DefaultValue(true), Description("背景刷新频率(以毫秒为单位)"), Category("自定义")]
        public int Interval
        {
            get => timer1.Interval;
            set
            {
                if (InvokeRequired)
                    Invoke(new Action(() => timer1.Interval = value));
                else
                    timer1.Interval = value;
            }
        }

        #endregion Property

        #region Method
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
        private void init()
        {
            ListBoxDisplay.DisplayMember = "Remark";
        }
        public new bool Update()
        {
            if (!IsInnerUpdatingOpen)
                RefreshDisplay();
            return true;
        }
        private void TextInputConctrol(TextBox textBox, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)
                e.Handled = true;
            //小数点的处理。
            if ((int)e.KeyChar == 46)   //小数点
            {
                if (textBox.Text.Length <= 0)
                    e.Handled = true;   //小数点不能在第一位
                else
                {
                    bool b1 = false, b2 = false;
                    b1 = float.TryParse(textBox.Text, out var oldf);
                    b2 = float.TryParse(textBox.Text + e.KeyChar.ToString(), out var outFloatType);
                    if (b2 == false)
                        if (b1 == true)
                            e.Handled = true;
                        else
                            e.Handled = false;
                }
            }
        }

        private void RefreshDisplay()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(RefreshDisplay));
                return;
            }
            Info = _dataSource?.Info;
            if (Info == null)
            {
                Enabled = false;
                return;
            }
            LabelTitle.Text = string.IsNullOrEmpty(Info.Name) ? Title : Info.Name;
            TextActualPos.Text = $"{Info.ActPos:N4}";
            TextActualVel.Text = $"{Info.ActVel:N4}";
            CheckIsEnable.Checked = Info.PowerStatus;

            LabPel.BackColor = Info.PosLimit ? Color.Black : Color.Lime;
            LabNel.BackColor = Info.NegLimit? Color.Black : Color.Lime;

            //BtnGoTo.Enabled = !Info.Moving;
            //BtnHome.Enabled = !Info.Moving;
        }

        #endregion Method

        #region Event

        private void ListBoxDisplay_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListBoxDisplay.SelectedItem == null)
                return;
            _currentSelectItem = ListBoxDisplay?.SelectedItem as ParamsValue;
            groupBox1.Text = _currentSelectItem.Remark + " " + "Axis";

            TextTargetPos.Text = _currentSelectItem.Value.ToString();
        }

        private void BtnGoTo_Click(object sender, EventArgs e)
        {
           var ret = _dataSource?.AbsGo(double.Parse(TextTargetPos.Text), TextRunVeloctity.Text == "0" ? _defaultVelocity : double.Parse(TextRunVeloctity.Text), _isBlock);
        }

        private void BtnHome_Click(object sender, EventArgs e)
        {
            _dataSource?.GoHome(string.IsNullOrEmpty(TextRunVeloctity.Text)?_goHomeVel:double.Parse(TextRunVeloctity.Text), _isBlock);
        }

        private void TextRunVeloctity_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextInputConctrol(TextRunVeloctity, e);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshDisplay();
        }

        private void BtnJogAdd_MouseDown(object sender, MouseEventArgs e)
        {
            _dataSource?.JogGo(TextRunVeloctity.Text == "0" ? _defaultVelocity : double.Parse(TextRunVeloctity.Text), true);
        }

        private void BtnJogAdd_MouseUp(object sender, MouseEventArgs e)
        {
            _dataSource?.JogGo(TextRunVeloctity.Text == "0" ? _defaultVelocity : double.Parse(TextRunVeloctity.Text), false);
        }

        private void BtnJogSub_MouseDown(object sender, MouseEventArgs e)
        {
            _dataSource?.JogGo((TextRunVeloctity.Text == "0" ? _defaultVelocity : double.Parse(TextRunVeloctity.Text)) * -1, true);
        }

        private void BtnJogSub_MouseUp(object sender, MouseEventArgs e)
        {
            _dataSource?.JogGo((TextRunVeloctity.Text == "0" ? _defaultVelocity : double.Parse(TextRunVeloctity.Text)) * -1, false);
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            _dataSource?.Reset(_isBlock);
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            _dataSource?.Stop(_isBlock);
        }

        private void CheckIsEnable_Click(object sender, EventArgs e)
        {
            _dataSource?.PowerCtrl(CheckIsEnable.Checked);
        }

        #endregion Event
    }
}
