using CYGKit.MTCP;
using Poc2Auto.Common;
using Poc2Auto.Model;
using Poc2Auto.Database;
using AlcUtility;
using System.Data;
using System.IO;
using System;

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

        public static readonly string MTCPefaultPath = @"C:\data\";

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
                errorCode = -111;
                errorString = "LotInfo 为 null.";
                return false;
            }
            LotStart lotStartData = new LotStart
            {
                TesterName = "SA2ALC",
                TesterID = "SA2_ALC_0000001",
                LotName = Overall.LotInfo.LotID ?? "NoLotName",
                SubLotName = Overall.LotInfo.SubLotName ?? "NoLotSubName",
                StressCode = Overall.LotInfo.StressCode ?? "NoStressCode",
                LotSize = Overall.LotInfo.LotID.Length.ToString() ?? "33",
                TestMode = Overall.LotInfo.TestMode ?? "NoTestMode",
                Operator = Overall.LotInfo.OperatorName ?? "A01",
                AlcSwVer = Overall.LotInfo.ALCVersion ?? "2.0.0.0",
                VisionSwVer = Overall.LotInfo.LoaderVersion ?? "2.0.0.0",
                HandlerPLCSwVer = Overall.LotInfo.UnloaderVersion ?? "1.1.3",
                TesterPLCSwVer = "1.1.1",
                HostMode = Overall.LotInfo.HostMode.ToString() ?? "PROD",
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

        public static bool SendMTCPLotLoad(string DutSn, string socketSN , out int errorCode, out string errorString)
        {
            Load lotLoadData = new Load
            {
                ModuleSn = DutSn,
                SocketSn = socketSN,
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

        public static bool SendMTCPLotUnload(string DutSn, string SocketSN, int disable, out int errorCode, out string errorString, out string bin)
        {
            Unload lotUnloadData = new Unload
            {
                ModuleSn = DutSn,
                SocketSn = SocketSN,
                SocketDisable = disable,
                Reset = 1,
            };
            if (!SendUnload(lotUnloadData, out int eCode, out string eString, out string recvCsv))
            {
                errorCode = eCode;
                errorString = eString;
                bin = "99";
                return false;
            }
            else
            {
                AlcSystem.Instance.Log(recvCsv, "MTCP_Return_ALC_Unload");
                var dt = MtcpOperation.Csv2DataTable(recvCsv);
                if (dt == null)
                {
                    bin = "100";
                    errorCode = eCode;
                    errorString = eString;
                    return false;
                }
                DataColumn[] pk = new DataColumn[] { dt.Columns["TID"] };
                dt.PrimaryKey = pk;
                bin = dt?.Rows.Find("#TEST_BIN")["PARAM2"].ToString();
                errorCode = 0;
                errorString = "NoMessage";
                return true;
            }
        }

        public static bool SendLotStart(LotStart data, out int errorCode, out string errorString)
        {
            var csvStr = MtcpOperation.GenCSV(data);
            
            var res = MtcpOperation.SendAndRecv(csvStr, 1, ConfigMgr.Instance.MtcpIP, ConfigMgr.Instance.MtcpPort, TimeOut, false,
                out _, out errorCode, out errorString);
            string path;
            if (!Directory.Exists(ConfigMgr.Instance.MTCPDataPath))
            {
                AlcSystem.Instance.Log($"配置路径不存在, LotStart文件已存入默认文件夹{MTCPefaultPath}下", "MTCP");
                path = MTCPefaultPath;
            }
            else
            {
                path = ConfigMgr.Instance.MTCPDataPath;
            }
            var time = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
            string file = $"{path}{time}_{Overall.LotInfo.LotID}_LotStart.csv";
            File.WriteAllText(file, csvStr);
            AlcSystem.Instance.Log(csvStr, "Send_MTCP_Lot_Start");
            AlcSystem.Instance.Log("LotStartErrorCode:" + errorCode, "MTCP");
            AlcSystem.Instance.Log("LotStartErrorString:" + errorString, "MTCP");
            return res;
        }

        public static bool SendLotEnd(LotEnd data, out int errorCode, out string errorString)
        {
            var csvStr = MtcpOperation.GenCSV(data);
            var res = MtcpOperation.SendAndRecv(csvStr, 1, ConfigMgr.Instance.MtcpIP, ConfigMgr.Instance.MtcpPort, TimeOut, false,
                out _, out errorCode, out errorString);
            string path;
            if (!Directory.Exists(ConfigMgr.Instance.MTCPDataPath))
            {
                AlcSystem.Instance.Log($"配置路径不存在, LotEnd文件已存入默认文件夹{MTCPefaultPath}下", "MTCP");
                path = MTCPefaultPath;
            }
            else
            {
                path = ConfigMgr.Instance.MTCPDataPath;
            }
            var time = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
            string file = $"{path}{time}_{Overall.LotInfo?.LotID}_LotEnd.csv";
            File.WriteAllText(file, csvStr);
            AlcSystem.Instance.Log(csvStr, "Send_MTCP_Lot_End");
            AlcSystem.Instance.Log("LotEndErrorCode:" + errorCode, "MTCP");
            AlcSystem.Instance.Log("Lot_EndErrorString:" + errorString, "MTCP");
            return res;
        }

        public static bool SendLoad(Load data, out int errorCode, out string errorString)
        {
            var csvStr = MtcpOperation.GenCSV(data);
            var res = MtcpOperation.SendAndRecv(csvStr, 1, ConfigMgr.Instance.MtcpIP, ConfigMgr.Instance.MtcpPort, TimeOut, true,
                out _, out errorCode, out errorString);
            string path;
            if (!Directory.Exists(ConfigMgr.Instance.MTCPDataPath))
            {
                AlcSystem.Instance.Log($"配置路径不存在, Load文件已存入默认文件夹{MTCPefaultPath}下", "MTCP");
                path = MTCPefaultPath;
            }
            else
            {
                path = ConfigMgr.Instance.MTCPDataPath;
            }
            var time = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
            string file = $"{path}{time}_{data.ModuleSn}_0.csv";
            File.WriteAllText(file, csvStr);
            AlcSystem.Instance.Log(csvStr, "Send_MTCP_Load");
            AlcSystem.Instance.Log("LoadErrorCode:" + errorCode, "MTCP");
            AlcSystem.Instance.Log("LoadErrorString:" + errorString, "MTCP");
            return res;
        }

        public static bool SendUnload(Unload data, out int errorCode, out string errorString, out string recvString)
        {
            if (data.ModuleSn == null || data.ModuleSn2 == null)
            {
                errorCode = -100;
                errorString = "ModuleSn 为空";
                recvString = "";
                return false;
            }
            var csvStr = MtcpOperation.GenCSV(data);
            var res = MtcpOperation.SendAndRecv(csvStr, 1, ConfigMgr.Instance.MtcpIP, ConfigMgr.Instance.MtcpPort, TimeOut, true,
                out recvString, out errorCode, out errorString);
            string path;
            if (!Directory.Exists(ConfigMgr.Instance.MTCPDataPath))
            {
                AlcSystem.Instance.Log($"配置路径不存在, UnLoad文件已存入默认文件夹{MTCPefaultPath}下", "MTCP");
                // Directory.CreateDirectory(ConfigMgr.Instance.MTCPDataPath);
                path = MTCPefaultPath;
            }
            else
            {
                path = ConfigMgr.Instance.MTCPDataPath;
            }
            var time = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff");
            string file = $"{path}{time}_{data.ModuleSn}_255.csv";
            File.WriteAllText(file, recvString);
            AlcSystem.Instance.Log(csvStr, "Send_MTCP_Unload");
            AlcSystem.Instance.Log("UnloadErrorCode:" + errorCode, "MTCP");
            AlcSystem.Instance.Log("UnloadErrorString:" + errorString, "MTCP");
            return res;
        }
    }
}
