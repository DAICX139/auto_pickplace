namespace Poc2Auto.Model
{
    /// <summary>
    /// 产品数量统计
    /// </summary>
    public class DutStatistics
    {
        /// <summary>
        /// 产品输入数量
        /// </summary>
        public int Input { get; set; }

        /// <summary>
        /// 测试通过产品数量, 输出时才统计
        /// </summary>
        public int Passed { get; set; }

        /// <summary>
        /// 测试失败产品数量, 输出时才统计
        /// </summary>
        public int Failed { get; set; }

        /// <summary>
        /// 未测试产品数量, 输出时才统计
        /// </summary>
        public int Untested { get; set; }

        /// <summary>
        /// 测试通过率
        /// </summary>
        public float PassRate => (float)Passed / (Passed + Failed);

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
    }
}
