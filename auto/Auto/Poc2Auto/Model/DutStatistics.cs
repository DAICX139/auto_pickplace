using Poc2Auto.Common;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Poc2Auto.Model
{
    /// <summary>
    /// 产品数量统计
    /// </summary>
    public class DutStatistics: INotifyPropertyChanged
    {

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 产品输入数量
        /// </summary>
        private int _input;

        public int Input
        {
            get { return _input; }
            set { _input = value; NotifyPropertyChanged(); }
        }


        /// <summary>
        /// 测试通过产品数量, 输出时才统计
        /// </summary>
        private int _passed;

        public int Passed
        {
            get { return _passed; }
            set { _passed = value; ConfigMgr.Instance.DUTPassCount = value; NotifyPropertyChanged(); }
        }


        /// <summary>
        /// 测试失败产品数量, 输出时才统计
        /// </summary>
        private int _failed;

        public int Failed
        {
            get { return _failed; }
            set { _failed = value; ConfigMgr.Instance.DUTPassCount = value; NotifyPropertyChanged(); }
        }


        /// <summary>
        /// 未测试产品数量, 输出时才统计
        /// </summary>
        private int _untested;

        public int Untested
        {
            get { return _untested; }
            set { _untested = value; NotifyPropertyChanged(); }
        }


        /// <summary>
        /// 测试通过率
        /// </summary>
        public float PassRate 
        {
            get
            {
                if (Passed <= 0)
                {
                    return 0;
                }
                return (float)Passed / (Passed + Failed);
            }
        }

        /// <summary>
        /// 产品输出数量
        /// </summary>
        public int Output => Passed + Failed + Untested;

        /// <summary>
        /// 剔除的产品数量
        /// </summary>
        public int Kickoff { get; set; }

        /// <summary>
        /// 丢失的产品数量
        /// </summary>
        public int Miss { get; set; }

        /// <summary>
        /// 线上的产品数量
        /// </summary>
        public int Online => Input - Output - Kickoff - Miss;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
