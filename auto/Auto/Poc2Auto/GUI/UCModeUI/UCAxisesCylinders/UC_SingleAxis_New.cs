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
using AlcUtility.PlcDriver.CommonCtrl;
using NetAndEvent.PlcDriver;


namespace Poc2Auto.GUI.UCModeUI.UCAxisesCylinders
{
    public partial class UC_SingleAxis_New : UserControl
    {
        public UC_SingleAxis_New()
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
        private const double _defaultVelocity = 5;

        private SingleAxisCtrl _dataSource;

        private double _goHomeVel = 5;

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

        //private ParamsConfig _config;
        ///// <summary>
        ///// 配方数据
        ///// </summary>
        //public ParamsConfig Config
        //{
        //    get { return _config; }
        //    set
        //    {
        //        //if (value == null)
        //        //    return;
        //        //_config = value;

        //        //foreach (var module in _config.ParamsModules.Values)
        //        //{
        //        //    foreach (var val in module.KeyValues.Values)
        //        //    {
        //        //        if (val.Remark.Contains("速度") || val.Remark.Contains("间距") || val.Remark.Contains("长度") || val.Remark.Contains("行数") || val.Remark.Contains("列数"))
        //        //            continue;
        //        //        if (val.Key.Contains(string.IsNullOrEmpty(Info?.Name) ? AxisName : Info.Name))
        //        //        {
        //        //            AxisPosInfo.Add(val);
        //        //        }
        //        //    }
        //        //}
        //        //cmbxPosList.BeginInvoke(new Action(() =>
        //        //{
        //        //    cmbxPosList.DataSource = AxisPosInfo;
        //        //}));
        //    }
        //}

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
            get => labAxisName.Text;
            set
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => Title = value));
                    return;
                }
                labAxisName.Text = value;
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
                labAxisName.Text = value == true ? Title : "";
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
            btnMoveTo.Enabled = enable;
            btnHome.Enabled = enable;
            btnJogAdd.Enabled = enable;
            btnJogSub.Enabled = enable;
            btnReset.Enabled = enable;
            btnStop.Enabled = enable;
            ckbxEnable.Enabled = enable;
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
            toolTip1.InitialDelay = 200;
            toolTip1.AutoPopDelay = 10 * 1000;
            toolTip1.ReshowDelay = 200;
            toolTip1.ShowAlways = true;
            toolTip1.IsBalloon = true;

            toolTip1.SetToolTip(numericVelocity, "默认速度5mm/s");
            toolTip1.SetToolTip(btnJogAdd, "点动+");
            toolTip1.SetToolTip(btnJogSub, "点动-");
            toolTip1.SetToolTip(numericMovePos, "目标位置");
            toolTip1.SetToolTip(labActPos, "实时位置");
        }
        public new bool Update()
        {
            if (!IsInnerUpdatingOpen)
                RefreshDisplay();
            return true;
        }

        private void RefreshDisplay()
        {
            try
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
                labAxisName.Text = string.IsNullOrEmpty(Info.Name) ? Title : Info.Name;
                labActPos.Text = $"{Info.ActPos:N4}";
                ckbxEnable.Checked = Info.PowerStatus;

                //labError.ForeColor = Info.Error ? Color.Red : Color.Black;
                //labHomed.ForeColor = Info.Homed ? Color.Red : Color.Black;
                //labMoveFinish.ForeColor = Info.MoveFinish ? Color.Lime : Color.Black;
                //labMoving.ForeColor = Info.Moving ? Color.Lime : Color.Black;
                //labNEL.ForeColor = Info.NegLimit ? Color.Red : Color.Black;
                //labPEL.ForeColor = Info.PosLimit ? Color.Red : Color.Black;
            }
            catch (Exception ex)
            {
 
            }
        }

        #endregion Method

        private void btnMoveTo_Click(object sender, EventArgs e)
        {
            var ret = _dataSource?.AbsGo(double.Parse(numericMovePos.Value.ToString()), numericVelocity.Text == "0" ? _defaultVelocity : double.Parse(numericVelocity.Text), _isBlock);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _dataSource?.Stop(_isBlock);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _dataSource?.Reset(_isBlock);
        }

        private void btnJogAdd_MouseDown(object sender, MouseEventArgs e)
        {
            _dataSource?.JogGo(numericVelocity.Text == "0" ? _defaultVelocity : double.Parse(numericVelocity.Text), true);
        }

        private void btnJogAdd_MouseUp(object sender, MouseEventArgs e)
        {
            _dataSource?.JogGo(numericVelocity.Text == "0" ? _defaultVelocity : double.Parse(numericVelocity.Text), false);
        }

        private void btnJogSub_MouseDown(object sender, MouseEventArgs e)
        {
            _dataSource?.JogGo((numericVelocity.Text == "0" ? _defaultVelocity : double.Parse(numericVelocity.Text)) * -1, true);
        }

        private void btnJogSub_MouseUp(object sender, MouseEventArgs e)
        {
            _dataSource?.JogGo((numericVelocity.Text == "0" ? _defaultVelocity : double.Parse(numericVelocity.Text)) * -1, false);
        }

        private void cmbxPosList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbxPosList.SelectedItem == null)
            //    return;
            //_currentSelectItem = cmbxPosList?.SelectedItem as ParamsValue;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            _dataSource?.GoHome(string.IsNullOrEmpty(numericVelocity.Text) ? _goHomeVel : double.Parse(numericVelocity.Text), _isBlock);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshDisplay();
        }

        private void ckbxEnable_Click(object sender, EventArgs e)
        {
            _dataSource?.PowerCtrl(ckbxEnable.Checked);
        }
    }
}
