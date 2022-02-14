using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using CYGKit.Database;
using CYGKit.Factory.DataBase;
using CYGKit.Factory.Statistics;
using Poc2Auto.Common;
using Poc2Auto.Model;

namespace Poc2Auto.Database
{
    public class DragonContext : DbContext
    {
        public DragonContext() : base(DatabaseHelper.GetMySqlConnection(ConfigMgr.Instance.ConnectionStrings), true)
        {
        }

        public DbSet<OnlineDut> OnlineDuts { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<BinColor> BinColors { get; set; }
        public DbSet<BinRegion> BinRegions { get; set; }
        public DbSet<TrayData> TrayDatas { get; set; }
        public DbSet<TrayInfo> TrayInfos { get; set; }

        public DbSet<LotBinStat> LotBinStats { get; set; }
        public DbSet<StationBinStat> StationBinStats { get; set; }
        public DbSet<DbSocketStat> SocketStats { get; set; }
        public DbSet<DbStationStat> StationStats { get; set; }
        public DbSet<LotInfo> LotInfo { get; set; }
        public DbSet<AxisLocation> AxisLocations { get; set; }
        public DbSet<TotalBinStat> TotalBinStats { get; set; }
        public DbSet<DUTStationBinTotal> DUTTotal { get; set; }
    }

    /// <summary>
    /// 记录在线location的dut信息
    /// </summary>
    public class OnlineDut : OnlineDutBase
    {
        public int SocketID { get; set; }
        [MaxLength(128)]
        public override string Barcode { get; set; }
        [MaxLength(255)]
        public string TestResult { get; set; }
        public string ErrString { get; set; }
        public DateTime LoadTime { get; set; }
        public DateTime DefaultInTime { get; set; }
        public DateTime Test1Time { get; set; }
        public DateTime Test2Time { get; set; }
        public DateTime Test3Time { get; set; }
        public DateTime Test4Time { get; set; }
        public DateTime UnloadTime { get; set; }
        public DateTime DefaultOutTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }

    public class Product : ProductBase
    {
        [MaxLength(128)]
        public string LotId { get; set; }
        public string TestResult { get; set; }
        public string ErrString { get; set; }
        public DateTime LoadTime { get; set; }
        public DateTime DefaultInTime { get; set; }
        public DateTime Test1Time { get; set; }
        public DateTime Test2Time { get; set; }
        public DateTime Test3Time { get; set; }
        public DateTime Test4Time { get; set; }
        public DateTime DefaultOutTime { get; set; }
        public DateTime UnloadTime { get; set; }
        public int CycleTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }

    /// <summary>
    /// 用于Socket良率
    /// </summary>
    public class DbSocketStat : DbStatBase
    {
        [Key, Column(Order = 0)]
        public override int Id { get; set; }

        //Test1
        public int YieldAtTest1LIV { get; set; }
        public int PassAtTest1LIV { get; set; }
        public int FailAtTest1LIV { get; set; }
        //Test2
        public int YieldAtTest2Dynamic { get; set; }
        public int PassAtTest2Dynamic { get; set; }
        public int FailAtTest2Dynamic { get; set; }
        //Test3
        public int YieldAtTest3Backup { get; set; }
        public int PassAtTest3Backup { get; set; }
        public int FailAtTest3Backup { get; set; }
        //Test4
        public int YieldAtTest4BP { get; set; }
        public int PassAtTest4BP { get; set; }
        public int FailAtTest4BP { get; set; }

        public override void UpdateDataSrcFromDb(SocketStat dataSrc)
        {
            base.UpdateDataSrcFromDb(dataSrc);

            dataSrc.SiteStats[StationName.Test1_LIVW.ToString()].Passed = PassAtTest1LIV ;
            dataSrc.SiteStats[StationName.Test2_NFBP.ToString()].Passed = PassAtTest2Dynamic;
            dataSrc.SiteStats[StationName.Test3_KYRL.ToString()].Passed = PassAtTest3Backup;
            dataSrc.SiteStats[StationName.Test4_BMPF.ToString()].Passed = PassAtTest4BP;

            dataSrc.SiteStats[StationName.Test1_LIVW.ToString()].Failed = FailAtTest1LIV;
            dataSrc.SiteStats[StationName.Test2_NFBP.ToString()].Failed = FailAtTest2Dynamic;
            dataSrc.SiteStats[StationName.Test3_KYRL.ToString()].Failed = FailAtTest3Backup;
            dataSrc.SiteStats[StationName.Test4_BMPF.ToString()].Failed = FailAtTest4BP;
        }

        public override void UpdateDbFromDataSrc(SocketStat dataSrc)
        {
            base.UpdateDbFromDataSrc(dataSrc);

            PassAtTest1LIV = dataSrc.SiteStats[StationName.Test1_LIVW.ToString()].Passed;
            PassAtTest2Dynamic = dataSrc.SiteStats[StationName.Test2_NFBP.ToString()].Passed;
            PassAtTest3Backup = dataSrc.SiteStats[StationName.Test3_KYRL.ToString()].Passed;
            PassAtTest4BP = dataSrc.SiteStats[StationName.Test4_BMPF.ToString()].Passed;

            FailAtTest1LIV = dataSrc.SiteStats[StationName.Test1_LIVW.ToString()].Failed;
            FailAtTest2Dynamic = dataSrc.SiteStats[StationName.Test2_NFBP.ToString()].Failed;
            FailAtTest3Backup = dataSrc.SiteStats[StationName.Test3_KYRL.ToString()].Failed;
            FailAtTest4BP = dataSrc.SiteStats[StationName.Test4_BMPF.ToString()].Failed;
        }
    }

    public class AxisLocation
    {
        public DateTime Time { get; set; }
        [Key]
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z1 { get; set; }
        public double R1 { get; set; }
        public double Z2 { get; set; }
        public double R2 { get; set; }
    }

    public class TotalBinStat
    {
        [Key]
        public int ID { get; set; }
        public int Index { get; set; }
        public int PASS_BIN_S { get; set; }
        public int PASS_BIN_P { get; set; }
        public int FAIL_BIN_A { get; set; }
        public int FAIL_BIN_B { get; set; }
        public int FAIL_BIN_F { get; set; }
        public int FAIL_BIN_NOTEST { get; set; }
        public int OVERALL_YIELD { get; set; }
        public int TOTAL_INPUT { get; set; }
    }

    public class DUTStationBinTotal
    {
        [Key]
        public int ID { get; set; }
        public string LotID { get; set; }
        public string DUTSN { get; set; }
        public int LIV_Result { get; set; }
        public int NFBP_Result { get; set; }
        public int KYRL_Result { get; set; }
        public int BP_Result { get; set; }
        public int Bin { get; set; }
    }
}
