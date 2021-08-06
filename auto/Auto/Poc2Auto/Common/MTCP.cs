using CYGKit.Factory.ConditionEditor;
using Poc2Auto.GUI;
using Poc2Auto.Model;
using System.Linq;

namespace Poc2Auto.Common
{
    public static class MTCP
    {
        public static void GetBin(this Dut dut)
        {
            GetBinByRule(dut, out var bin);
            dut.Result = bin;
        }

        public static void GetBinByRule(Dut dut, out int bin)
        {
            var root = UCTMConfig.Instance.Root;

            ConditionHelper.Calculate(dut.StationResults() , root, out bin);
        }

        public static int[] StationResults(this Dut dut)
        {
            var stations = StationManager.TestStations.
                Where(s => StationManager.Stations[s].Enable).ToList();
            var results = new int[stations.Count];
            for (var i = 0; i < stations.Count; i++)
            {
                results[i] = dut.TestResult[stations[i]];
            }
            return results;
        }
    }
}
