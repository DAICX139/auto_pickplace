using System;
using System.Collections.Generic;
using System.Linq;
using Poc2Auto.Common;

namespace Poc2Auto.Model
{
    public class Dut
    {
        /// <summary>
        /// 沒有产品
        /// </summary>
        public const int NoDut = 0;
        /// <summary>
        /// 测试通过
        /// </summary>
        public const int PassBin = 1;
        /// <summary>
        /// A测试失败
        /// </summary>
        public const int Fail_A = 2;
        /// <summary>
        /// B测试失败
        /// </summary>
        public const int Fail_B = 3;
        /// <summary>
        /// All测试失败
        /// </summary>
        public const int Fail_All = 4;
        /// <summary>
        /// 未测试(本来应该测试，由于各种原因而导致未测试)
        /// </summary>
        public const int NoTestBin = 98;
        /// <summary>
        /// 获取MTCP Bin信息失败时
        /// </summary>
        public const int MTCPFail = 99;
        /// <summary>
        /// 还未从MTCP拿到测试结果
        /// </summary>
        public const int NoResult = 100;
        /// <summary>
        /// 异常Dut（包括吸嘴未取起来的, 挑料模式下不是我们想要的DUT）
        /// </summary>
        public const int AbnormalDut = 4;
        /// <summary>
        /// 扫码模式下用于放回原位后的DUT
        /// </summary>
        public const int ScanDut = 200;
        /// <summary>
        /// socket禁用
        /// </summary>
        public const int Disable = 999;

        public string Barcode { get; set; }
        public Dictionary<StationName, int> TestResult { get; } = new Dictionary<StationName, int>();
        public string Remark { get; set; }
        public int Result
        {
            get
            {
                if (TestResult.Count == 0)//产品未测试
                {
                    return Fail_All;
                }
                else if (!TestResult.ContainsKey(StationName.PNP))//产品没有经过上下料工站
                {
                    return Fail_All;
                }
                return TestResult[StationName.PNP];
            }
            set => TestResult[StationName.PNP] = value;
        }

        public void SetTestResultByString(string resultStr)
        {
            TestResult.Clear();
            if (string.IsNullOrEmpty(resultStr)) return;
            var results = resultStr.Split(',');
            foreach(var result in results)
            {
                var keyValue = result.Split(':');
                if (keyValue.Length < 2) continue;
                if (!Enum.TryParse<StationName>(keyValue[0], out var key)) continue;
                if (!int.TryParse(keyValue[1], out var value)) continue;
                TestResult.Add(key, value);
            }
        }

        public string GetTestResultString()
        {
            return string.Join(",", TestResult.Select(p => $"{p.Key}:{p.Value}"));
        }

        public int ChangeDutTestResult(int result)
        {
            //TM返回Dut测试失败时
            if (result == 0)
                return Fail_All;

            return result;
        }
    }
}
