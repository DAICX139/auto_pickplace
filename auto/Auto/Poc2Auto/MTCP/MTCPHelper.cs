using CYGKit.MTCP;
using Poc2Auto.Common;
using Poc2Auto.Model;
using Poc2Auto.Database;
using AlcUtility;
using System.Data;

namespace Poc2Auto.MTCP
{
    public class MTCPHelper
    {
        public static int TimeOut = 10 * 60 * 1000;

        public static int NUM_BIN_ERROR = 0;
        public static int NUM_BIN_FAIL = 0;
        public static int NUM_BIN_PASS = 0;
        public static int NUM_BIN_VA1PASS = 0;
        public static int NUM_BIN_VA2PASS = 0;
        public static int NUM_BIN_SUPER = 0;
        public static int NUM_BIN_Yield = 0;

        static MTCPHelper()
        {
            var binStats = DragonDbHelper.GetTotalBin();
            if (binStats == null) return;
            NUM_BIN_SUPER = binStats.PASS_BIN_S;
            NUM_BIN_FAIL = binStats.FAIL_BIN_F;
            NUM_BIN_PASS = binStats.PASS_BIN_P;
            NUM_BIN_VA1PASS = binStats.FAIL_BIN_A;
            NUM_BIN_VA2PASS = binStats.FAIL_BIN_B;
            NUM_BIN_ERROR = binStats.FAIL_BIN_NOTEST;
            NUM_BIN_Yield = binStats.OVERALL_YIELD;
        } 

        public static void ClearTotalBinStats()
        {
            NUM_BIN_SUPER = 0;
            NUM_BIN_FAIL = 0;
            NUM_BIN_PASS = 0;
            NUM_BIN_VA1PASS = 0;
            NUM_BIN_VA2PASS = 0;
            NUM_BIN_ERROR = 0;
            NUM_BIN_Yield = 0;
        }
        public static bool SendMTCPLotStart(out int errorCode, out string errorString)
        {
            if (Overall.LotInfo == null)
            {
                errorCode = -1;
                errorString = "LotInfo 为 null.";
                return false;
            }
            LotStart lotStartData = new LotStart
            {
                TesterName = "ALC",
                TesterID = "ALC_0000001",
                LotName = Overall.LotInfo.LotID ?? "NoLotName",
                SubLotName = Overall.LotInfo.SubLotName ?? "NoLotSubName",
                StressCode = Overall.LotInfo.StressCode ?? "NoStressCode",
                LotSize = Overall.LotInfo.LotSize.ToString() ?? "0",
                TestMode = Overall.LotInfo.TestMode ?? "NoTestMode",
                Operator = Overall.LotInfo.OperatorName ?? "A01",
                AlcSwVer = Overall.LotInfo.ALCVersion ?? "2.0.4.2",
                VisionSwVer = Overall.LotInfo.LoaderVersion ?? "3.1.4.1",
                HandlerPLCSwVer = Overall.LotInfo.UnloaderVersion ?? "6.2.8.2",
                TesterPLCSwVer = "0.0.0.1",
            };
            if (!SendLotStart(lotStartData, out int eCode, out string eString))
            {
                errorCode = eCode;
                errorString = eString;
                return false;
            }
            else
            {
                errorCode = 0;
                errorString = "NoMessage";
                return true;
            }

        }

        public static bool SendMTCPLotEnd(out int errorCode, out string errorString)
        {
            var binStats = DragonDbHelper.GetTotalBin();
            if (binStats == null)
            {
                errorCode = -1;
                errorString = "binStats is null";
                return false;
            }
            LotEnd lotEndData = new LotEnd
            { 
                PassBinA = binStats.FAIL_BIN_A.ToString(),
                PassBinB = binStats.FAIL_BIN_B.ToString(),
                PassBinF = binStats.FAIL_BIN_F.ToString(),
                PassBinP = binStats.PASS_BIN_P.ToString(),
                PassBinNoTest = binStats.FAIL_BIN_NOTEST.ToString(),
                TotalInput = binStats.TOTAL_INPUT.ToString(), 
                OverallYield = binStats.OVERALL_YIELD.ToString(),  
            };
            if (!SendLotEnd(lotEndData, out int eCode, out string eString))
            {
                errorCode = eCode;
                errorString = eString;
                return false;
            }
            else
            {
                errorCode = 0;
                errorString = "NoMessage";
                return true;
            }
        }

        public static bool SendMTCPLotLoad(string DutSn,  out int errorCode, out string errorString)
        {
            Load lotLoadData = new Load
            {
                ModuleSn = DutSn,
                SocketSn = DutSn,
            };
            if (!SendLoad(lotLoadData, out int eCode, out string eString))
            {
                errorCode = eCode;
                errorString = eString;
                return false;
            }
            else
            {
                errorCode = 0;
                errorString = "NoMessage";
                return true;
            }
        }

        public static bool SendMTCPLotUnload(string DutSn, int disable, out int errorCode, out string errorString, out string bin)
        {
            Unload lotUnloadData = new Unload
            {
                ModuleSn = DutSn,
                SocketSn = DutSn,
                SocketDisable = disable,
                Reset = 1,
            };
            if (!SendUnload(lotUnloadData, out int eCode, out string eString, out string recvCsv))
            {
                errorCode = eCode;
                errorString = eString;
                bin = "999";
                return false;
            }
            else
            {
                AlcSystem.Instance.Log(recvCsv, "MTCP_Send_Unload");
                var dt = MtcpOperation.Csv2DataTable(recvCsv);
                DataColumn[] pk = new DataColumn[] { dt.Columns["TID"] };
                dt.PrimaryKey = pk;
                bin = dt?.Rows.Find("#TEST_BIN")["PARAM1"].ToString();
                errorCode = 0;
                errorString = "NoMessage";
                return true;
            }
        }

        public static bool SendLotStart(LotStart data, out int errorCode, out string errorString)
        {
            var csvStr = MtcpOperation.GenCSV(data);
            AlcSystem.Instance.Log(csvStr, "Send_MTCP_Lot_Start");
            return MtcpOperation.SendAndRecv(csvStr, 1, ConfigMgr.Instance.MtcpIP, ConfigMgr.Instance.MtcpPort, TimeOut, false,
                out _, out errorCode, out errorString);
        }

        public static bool SendLotEnd(LotEnd data, out int errorCode, out string errorString)
        {
            var csvStr = MtcpOperation.GenCSV(data);
            AlcSystem.Instance.Log(csvStr, "Send_MTCP_Lot_End");
            return MtcpOperation.SendAndRecv(csvStr, 1, ConfigMgr.Instance.MtcpIP, ConfigMgr.Instance.MtcpPort, TimeOut, false,
                out _, out errorCode, out errorString);
        }

        public static bool SendLoad(Load data, out int errorCode, out string errorString)
        {
            var csvStr = MtcpOperation.GenCSV(data);
            AlcSystem.Instance.Log(csvStr, "Send_MTCP_Load");
            return MtcpOperation.SendAndRecv(csvStr, 1, ConfigMgr.Instance.MtcpIP, ConfigMgr.Instance.MtcpPort, TimeOut, false,
                out _, out errorCode, out errorString);
        }

        public static bool SendUnload(Unload data, out int errorCode, out string errorString, out string recvString)
        {
            var csvStr = MtcpOperation.GenCSV(data);
            AlcSystem.Instance.Log(csvStr, "Send_MTCP_Unload");
            return MtcpOperation.SendAndRecv(csvStr, 1, ConfigMgr.Instance.MtcpIP, ConfigMgr.Instance.MtcpPort, TimeOut, false,
                out recvString, out errorCode, out errorString);
        }
    }
}
