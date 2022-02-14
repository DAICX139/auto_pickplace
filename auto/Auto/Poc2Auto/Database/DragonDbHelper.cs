using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AlcUtility;
using CYGKit.Factory.DataBase;
using CYGKit.Factory.Statistics;
using Poc2Auto.Common;
using Poc2Auto.Model;
using Poc2Auto.MTCP;

namespace Poc2Auto.Database
{
    public partial class DragonDbHelper
    {
        public static void LoadDut(int socketID, int row, int col, Dut dut)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var duts = context.OnlineDuts.Where(r => r.SocketID == socketID).ToList();
                    foreach (var ondut in duts)
                    {
                        context.OnlineDuts.Remove(ondut);
                    }
                    context.OnlineDuts.Add(new OnlineDut() 
                    {
                        StationID = (int)StationName.Load,
                        Row = row,
                        Column = col,
                        SocketID = socketID,
                        Barcode = dut.Barcode,
                        Bin = dut.Result,
                        TestResult = dut.GetTestResultString(),
                        LoadTime = DateTime.Now,
                        LastUpdateTime = DateTime.Now,
                    });

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static void RemoveDut(int socketID)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var duts = context.OnlineDuts.Where(r => r.SocketID == socketID).ToList();
                    foreach (var ondut in duts)
                    {
                        context.OnlineDuts.Remove(ondut);
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static void AddBarcode(int socketID, string barcode)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var dut = context.OnlineDuts.FirstOrDefault(d => d.SocketID == socketID);
                    if (dut == null)
                    {
                        dut = new OnlineDut
                        {
                            Barcode = "norecord", 
                        };
                        context.OnlineDuts.Add(dut);
                    }
                    else
                        dut.Barcode = barcode;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }
        public static void MoveDut(int sourceSocket, int destSocket, bool load)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var dut = context.OnlineDuts.FirstOrDefault(d => d.SocketID == sourceSocket);
                    if (dut == null)
                    {
                        dut = new OnlineDut
                        {
                            Barcode = "norecord",
                            Bin = Dut.NoTestBin,
                        };
                        context.OnlineDuts.Add(dut);
                    }

                    dut.SocketID = destSocket;
                    dut.LastUpdateTime = DateTime.Now;
                    if (load)
                    {
                        var onlineDut = context.OnlineDuts.Where(d => d.StationID == (int)StationName.PNP).ToList();
                        foreach (var ondut in onlineDut)
                        {
                            context.OnlineDuts.Remove(ondut);
                        }
                        dut.StationID = (int)StationName.PNP;
                        dut.DefaultInTime = DateTime.Now;
                    }
                    else
                    {
                        var onlineDut = context.OnlineDuts.Where(d => d.StationID == (int)StationName.Unload).ToList();
                        foreach (var ondut in onlineDut)
                        {
                            context.OnlineDuts.Remove(ondut);
                        }
                        dut.StationID = (int)StationName.Unload;
                        dut.UnloadTime = DateTime.Now;
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static void RotateDut()
        {
            try
            {
                using (var context = new DragonContext())
                {
                    foreach (var station in StationManager.RotationStations)
                    {
                        var sockets = StationManager.Stations[station].SocketGroup.Sockets;
                        foreach (var socket in sockets)
                        {
                            var dut = context.OnlineDuts.FirstOrDefault(d => d.SocketID == socket.Index);
                            if (dut == null)
                            {
                                continue;
                                //dut = new OnlineDut() { SocketID = socket.Index };
                                //context.OnlineDuts.Add(dut);
                            }

                            dut.StationID = (int)station;
                            dut.LastUpdateTime = DateTime.Now;
                            switch (station)
                            {
                                case StationName.PNP:
                                    dut.DefaultOutTime = DateTime.Now;
                                    break;
                                case StationName.Test1_LIVW:
                                    dut.Test1Time = DateTime.Now;
                                    break;
                                case StationName.Test2_NFBP:
                                    dut.Test2Time = DateTime.Now;
                                    break;
                                case StationName.Test3_KYRL:
                                    dut.Test3Time = DateTime.Now;
                                    break;
                                case StationName.Test4_BMPF:
                                    dut.Test4Time = DateTime.Now;
                                    break;
                            }
                        }
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static void SetTestResult(int socketID, string sn, string result)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var dut = context.OnlineDuts.FirstOrDefault(d => d.SocketID == socketID);
                    if (dut == null)
                    {
                        dut = new OnlineDut
                        {
                            SocketID = socketID,
                            Barcode = sn,
                        };
                        context.OnlineDuts.Add(dut);
                    }

                    dut.TestResult = result;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static void SetBin(int socketID, string sn, int bin)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var dut = context.OnlineDuts.FirstOrDefault(d => d.SocketID == socketID);
                    if (dut == null)
                    {
                        dut = new OnlineDut
                        {
                            SocketID = socketID,
                            Barcode = sn,
                        };
                        context.OnlineDuts.Add(dut);
                    }

                    dut.Bin = bin;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static void UnloadDut(int socketID)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var product = new Product();
                    var dut = context.OnlineDuts.FirstOrDefault(d => d.SocketID == socketID);
                    if (dut != null)
                    {
                        product = new Product
                        {
                            Barcode = dut.Barcode,
                            Bin = dut.Bin,
                            TestResult = dut.TestResult,
                            LoadTime = dut.LoadTime,
                            DefaultInTime = dut.DefaultInTime,
                            Test1Time = dut.Test1Time,
                            Test2Time = dut.Test2Time,
                            Test3Time = dut.Test3Time,
                            Test4Time = dut.Test4Time,
                            DefaultOutTime = dut.DefaultOutTime,
                            UnloadTime = dut.UnloadTime,
                            CycleTime = (int)(DateTime.Now - dut.LoadTime).TotalSeconds,
                        };
                        context.OnlineDuts.Remove(dut);
                    }

                    product.OutputTime = DateTime.Now;
                    product.LastUpdateTime = DateTime.Now;
                    context.Products.Add(product);

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static void LoadPutDutToTray(int socketID)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var dut = context.OnlineDuts.FirstOrDefault(d => d.SocketID == socketID);
                    if (dut == null)
                        return;
                    context.OnlineDuts.Remove(dut);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static LotInfo GetLotInfo()
        {
            try
            {
                using (var context = new DragonContext())
                {
                    return context.LotInfo.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
                return null;
            }
        }

        public static void WriteTrayData(int trayID, params Rectangle[] regions)
        {
            try
            {
                if (regions == null || regions.Count() == 0) return;
                using (var context = new DragonContext())
                {
                    foreach (var region in regions)
                        for (var row = region.Top; row <= region.Bottom; row++)
                            for (var column = region.Left; column <= region.Right; column++)
                                context.TrayDatas.Add(new TrayData
                                {
                                    TrayID = trayID,
                                    Row = row,
                                    Column = column
                                });
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static void WriteTrayData(int trayID, int[,] data)
        {
            try
            {
                if (data == null) return;

                using (var context = new DragonContext())
                {
                    var row = data.GetLength(0);
                    var column = data.GetLength(1);
                    for (var i = 0; i < row; i++)
                        for (var j = 0; j < column; j++)
                            if (data[i, j] == 1)
                                context.TrayDatas.Add(new TrayData
                                {
                                    TrayID = trayID,
                                    Row = i,
                                    Column = j,
                                    Barcode = "noscan",
                                    Bin = 98,
                                });
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static TrayData TakeTrayData(int trayID, int row, int col)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var data = context.TrayDatas.FirstOrDefault(d => d.TrayID == trayID && d.Row == row && d.Column == col);
                    if (data != null)
                    {
                        context.TrayDatas.Remove(data);
                        context.SaveChanges();
                    }

                    return data;
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
                return null;
            }
        }

        public static TrayData GetTrayData(int trayID, int row, int col)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var data = context.TrayDatas.FirstOrDefault(d => d.TrayID == trayID && d.Row == row && d.Column == col);
                    return data;
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
                return null;
            }
        }

        public static List<TrayData> GetTrayData(int trayID)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var data = context.TrayDatas.Where(d => d.TrayID == trayID).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
                return null;
            }
        }

        public static int[,] TakeTrayData(int trayID)
        {
            int[,] data = new int[Tray.ROW, Tray.COL];
            try
            {
                using (var context = new DragonContext())
                {
                    for (int i = 0; i < Tray.ROW; i++)
                        for (int j = 0; j < Tray.COL; j++)
                        {
                            data[i, j] = context.TrayDatas.FirstOrDefault(d => d.TrayID == trayID && d.Row == i && d.Column == j) == null ? 0 : 1;
                        }
                    return data;
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
                return null;
            }
        }

        public static void WriteTrayData(TrayData data)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var data2 = context.TrayDatas.FirstOrDefault(d => d.TrayID == data.TrayID && d.Row == data.Row && d.Column == data.Column);
                    if (data != null)
                    {
                        context.TrayDatas.Add(data);
                    }
                    else
                    {
                        data2.Barcode = data.Barcode;
                        data2.Bin = data.Bin;
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static void AbnormalDut(int trayID, int row, int col, int bin)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var data = context.TrayDatas.FirstOrDefault(d => d.TrayID == trayID && d.Row == row && d.Column == col);
                    if (data == null)
                    {
                        data = new TrayData
                        {
                            Row = row,
                            Column = col,
                            TrayID = trayID,
                            Barcode = "noscan",
                            Bin = bin,
                        };
                        context.TrayDatas.Add(data);
                    }
                    else
                    {
                        data.Bin = bin;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static int GetBinFromProducts(string sn)
        {
            using (var context = new DragonContext())
            {
                var product = context.Products.FirstOrDefault(p => p.Barcode == sn);
                if (product == null)
                {
                    return Dut.NoTestBin;
                }
                else
                {
                    return product.Bin;
                }
            }
        }

        public static void ClearTrayData(int index)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var data = context.TrayDatas.Where(d => d.TrayID == index);
                    if (data != null)
                    {
                        foreach (var dut in data)
                        {
                            context.TrayDatas.Remove(dut);
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static void ClearBinRegion(int index)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var regions = context.BinRegions.Where(d => d.TrayID == index).ToList();
                    if (regions != null)
                    {
                        foreach (var region in regions)
                        {
                            context.BinRegions.Remove(region);
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static void SetTotalBin(int bin)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var binStats = context.TotalBinStats.FirstOrDefault(d => d.Index == 1);
                    if (binStats == null)
                    {
                        var stats = new TotalBinStat { Index = 1 };
                        context.TotalBinStats.Add(stats);
                        UpdateOrAddBinStat(bin, stats);
                    }
                    else
                    {
                        UpdateOrAddBinStat(bin, binStats);   
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static void ClearToatlBinStats()
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var binStats = context.TotalBinStats.FirstOrDefault(d => d.Index == 1);
                    if (binStats == null)
                    {
                        var stats = new TotalBinStat { Index = 1 };
                        context.TotalBinStats.Add(stats);
                    }
                    else
                    {
                        binStats.FAIL_BIN_A = 0;
                        binStats.FAIL_BIN_B = 0;
                        binStats.FAIL_BIN_F = 0;
                        binStats.FAIL_BIN_NOTEST = 0;
                        binStats.OVERALL_YIELD = 0;
                        binStats.PASS_BIN_P = 0;
                        binStats.PASS_BIN_S = 0;
                        binStats.TOTAL_INPUT = 0;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
            }
        }

        public static TotalBinStat GetTotalBin()
        {
            try
            {
                using (var context = new DragonContext())
                {
                    return context.TotalBinStats.FirstOrDefault(d => d.Index == 1);
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
                return null;
            }
        }

        private static void UpdateOrAddBinStat(int bin, TotalBinStat stats)
        {
            switch (bin)
            {
                case (int)TMBinValue.BIN_ERROR:
                    stats.FAIL_BIN_NOTEST = ++MTCPHelper.NUM_BIN_ERROR;
                    break;
                case (int)TMBinValue.BIN_PASS:
                    stats.PASS_BIN_P = ++MTCPHelper.NUM_BIN_PASS;
                    break;
                case (int)TMBinValue.BIN_VA1PASS:
                    stats.FAIL_BIN_A = ++MTCPHelper.NUM_BIN_VA1PASS;
                    break;
                case (int)TMBinValue.BIN_VA2PASS:
                    stats.FAIL_BIN_B = ++MTCPHelper.NUM_BIN_VA2PASS;
                    break;
                case (int)TMBinValue.BIN_FAIL:
                    stats.FAIL_BIN_F = ++MTCPHelper.NUM_BIN_FAIL;
                    break;
                default:
                    break;
            }
            stats.TOTAL_INPUT = RunModeMgr.LoadCount;
        }

        public static List<LotBinStat> GetLotBinStats(string lotId)
        {
            using (var context = new DragonContext())
            {
               return context.LotBinStats.Where(l=>l.LotId == lotId).ToList();
            }
        }

        public static List<StationBinStat> GetStationBinStats(string lotId)
        {
            using (var context = new DragonContext())
            {
                return context.StationBinStats.Where(l => l.LotId == lotId).ToList();
            }
        }

        public static List<BinRegion> GetBinRegion(int TrayID)
        {
            using (var context = new DragonContext())
            {
                return context.BinRegions?.Where(r => r.TrayID == TrayID).ToList();
            }
        }
        public static Color GetBinColor(int bin)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    Color color = default;
                    var binColors = context.BinColors.FirstOrDefault(r => r.Bin == bin);

                    if(binColors == null)
                    {
                        color = Color.DarkOrange;
                    }
                    else
                    {
                        color = binColors.Color;
                    }
                    return color;
                }
            }
            catch (Exception ex)
            {
                return Color.DarkOrange;
            }
            
        }

        public static void SetBinColor(int bin, Color color, bool isUseColor)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var binColor = context.BinColors.FirstOrDefault(d => d.Bin == bin);
                    if (binColor == null)
                    {
                        context.BinColors.Add(
                            new BinColor()
                            {
                                Bin = bin,
                                Color = GetDefaultBinColor(bin)
                            }
                            );
                    }
                    else if (isUseColor)
                        binColor.Color = color;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static Color GetDefaultBinColor(int bin)
        {
            Color color = Color.Blue;
            switch (bin)
            {
                case 1: color = Color.Green; break;
                case 2: color = Color.Blue; break;
                case 3: color = Color.Brown; break;
                case 4: color = Color.Red; break;
                case 5: color = Color.Violet; break;

                case 6: color = Color.LightSalmon; break;
                case 7: color = Color.LightBlue; break;
                case 8: color = Color.Red; break;
                case 9: color = Color.LightSeaGreen; break;
                case 10: color = Color.Yellow; break;

                case 98: color = Color.DarkOrange; break;
                case 99: color = Color.Orange; break;//启用MTCP获取bin信息失败
                case 200: color = Color.DarkGray;break;
                default:
                    break;
            }
            return color;
        }

        public static void SetDutTotal(string lotID, string dutSN, int result, StationName name) 
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var stat = context.DUTTotal.FirstOrDefault(d => d.DUTSN == dutSN && d.LotID == lotID);
                    if (stat == null)
                    {
                        stat = new DUTStationBinTotal
                        {
                            LotID = lotID,
                            DUTSN = dutSN,
                        };
                        UpdateOrAddDutstat(result, name, stat);
                        context.DUTTotal.Add(stat);
                    }
                    else
                    {
                        UpdateOrAddDutstat(result, name, stat);
                    }
                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {

            }
        }

        private static void UpdateOrAddDutstat(int result, StationName name, DUTStationBinTotal total)
        {
            switch (name)
            {
                case StationName.Test1_LIVW:
                    total.LIV_Result = result;
                    break;
                case StationName.Test2_NFBP:
                    total.NFBP_Result = result;
                    break;
                case StationName.Test3_KYRL:
                    total.KYRL_Result = result;
                    break;
                case StationName.Test4_BMPF:
                    total.BP_Result = result;
                    break;
                default:
                    break;
            }
        }

        public static void UpdateDutBin(string lotID, string sn, int bin)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var stat = context.DUTTotal.FirstOrDefault(d => d.DUTSN == sn && d.LotID == lotID);
                    if (stat == null)
                    {
                        stat = new DUTStationBinTotal
                        {
                            LotID = lotID,
                            DUTSN = sn,
                            Bin = bin,
                        };
                        context.DUTTotal.Add(stat);
                    }
                    else
                    {
                        stat.Bin = bin;
                    }
                    context.SaveChanges();

                }
            }
            catch (Exception ex)
            {

            }
        }

        public static List<DUTStationBinTotal> GetStationBinTotal()
        {
            using (var context = new DragonContext())
            {
                return context.DUTTotal.ToList();
            }
        }

        public static void ClearOnlineDut()
        {
            using (var context = new DragonContext())
            {
                var onDut = context.OnlineDuts.ToList();

                foreach (var dut in onDut)
                {
                    context.OnlineDuts.Remove(dut);
                }
                context.SaveChanges();
            }
        }
    }
 
    /// <summary>
    /// Socket,Bin,Station 良率统计
    /// </summary>
    public partial class DragonDbHelper
    {
        /// <summary>
        /// 更新Table[LotBinStat]
        /// </summary>
        /// <param name="lotId"></param>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static int AddOrUpdateBin(string lotId, int bin)
        {
            using (var context = new DragonContext())
            {
                DbTableHelper.AddOrUpdateBin(lotId, bin, context.LotBinStats);
                return context.SaveChanges();
            }
        }

        /// <summary>
        /// 更新Table[StationBinStat]
        /// </summary>
        /// <param name="stationName"></param>
        /// <param name="lotId"></param>
        /// <param name="bin"></param>
        /// <returns></returns>
        public static int AddOrUpdateBin(string stationName, string lotId, int bin)
        {
            using (var context = new DragonContext())
            {
                DbTableHelper.AddOrUpdateBin(stationName, lotId, bin, context.StationBinStats);
                return context.SaveChanges();
            }
        }

        public static int AddOrUpdateSocketStat(int index, SocketStat socketStat)
        {
            using (var context = new DragonContext())
            {
                var stats = context.SocketStats;
                var record = stats.FirstOrDefault(p => p.Index == index);
                if (record != null)
                {
                    record.UpdateDbFromDataSrc(socketStat);
                }
                else
                {
                    var newRecord = new DbSocketStat() { Index = index };
                    newRecord.UpdateDbFromDataSrc(socketStat);
                    stats.Add(newRecord);
                }
                return context.SaveChanges();
            }
        }

        public static int AddLocation(AxisLocation location)
        {
            using (var context = new DragonContext())
            {
                var data = context.AxisLocations.FirstOrDefault(r => r.Name == location.Name);
                if (data == null)
                {
                    context.AxisLocations.Add(location);
                }
                return context.SaveChanges();
            }
        }

        public static List<AxisLocation> LoadAxisLocations()
        {
            using (var context = new DragonContext())
            {
                return context.AxisLocations.OrderBy(r => r.Name).ToList();
            }
        }

        public static int DelAxisLocation(string name)
        {
            using (var context = new DragonContext())
            {
                var data = context.AxisLocations.FirstOrDefault(r => r.Name == name);
                if (data != null)
                {
                    context.AxisLocations.Remove(data);
                }
                return context.SaveChanges();
            }
        }
        public static bool GetStationEnable(string stationName)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var station = context.StationStats.FirstOrDefault(d => d.StationName == stationName);
                    if (station != null)
                        return station.Enable == true;
                    else
                        return false;

                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");
                return false;
            }

        }

        public static void SetStationEnable(string stationName, bool enable)
        {
            try
            {
                using (var context = new DragonContext())
                {
                    var station = context.StationStats.FirstOrDefault(d => d.StationName == stationName);
                    if (station == null)
                        context.StationStats.Add(new DbStationStat() { StationName = stationName, Enable = enable });
                    else
                        station.Enable = enable;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AlcSystem.Instance.Error(ex.Message + "\r\n" + ex.StackTrace, -1, AlcErrorLevel.WARN, "Database");

            }
        }
    }
}
