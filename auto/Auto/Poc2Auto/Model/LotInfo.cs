using System.ComponentModel.DataAnnotations;
using CYGKit.Factory.Lot;

namespace Poc2Auto.Model
{
    public class LotInfo
    {
        [LotItem("LotName"), Key]
        public string LotID { set; get; }
        [LotItem("SubLot")]
        public string SubLotName { set; get; }
        public string CreateDateTime { set; get; }
        [LotItem("Device", false)]
        public string DeviceName { set; get; }
        [LotItem("Step")]
        public string StepName { set; get; }
        [LotItem("TestMode")]
        public string TestMode { set; get; }
        [LotItem("StressCode", typeof(Overall), "StressCodeCreator")]
        public string StressCode { set; get; }
        public uint LotSize { set; get; }
        [LotItem("Operator")]
        public string OperatorName { set; get; }
        public string TesterID { set; get; }
        public string ALCVersion { set; get; }
        public string LoaderVersion { set; get; }
        public string UnloaderVersion { set; get; }
    }
}
