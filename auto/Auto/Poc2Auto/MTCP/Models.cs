using CYGKit.MTCP;

namespace Poc2Auto.MTCP
{
    [CsvHeadNameOfMember(StructSendData.Head_value)]
    public class LotStart
    {
        [CsvFixedValue(1, "LOTS", "#TESTER_NAME")]
        public string TesterName { get; set; }

        [CsvFixedValue(2, "LOTS", "#TESTER_ID")]
        public string TesterID { get; set; }

        [CsvFixedValue(3, "LOTS", "#LOT_NAME")]
        public string LotName { get; set; }

        [CsvFixedValue(4, "LOTS", "#SUB_LOT_NAME")]
        public string SubLotName { get; set; }

        [CsvFixedValue(5, "LOTS", "#STRESS_CODE")]
        public string StressCode { get; set; }

        [CsvFixedValue(6, "LOTS", "#LOT_SIZE")]
        public string LotSize { get; set; }

        [CsvFixedValue(7, "LOTS", "#TEST_MODE")]
        public string TestMode { get; set; }

        [CsvFixedValue(8, "LOTS", "#OPERATOR")]
        public string Operator { get; set; }

        [CsvFixedValue(9, "LOTS", "#ALC_SW_VER")]
        public string AlcSwVer { get; set; }

        [CsvFixedValue(10, "LOTS", "#VISION_SW_VER")]
        public string VisionSwVer { get; set; }

        [CsvFixedValue(11, "LOTS", "#HANDLER_PLC_SW_VER")]
        public string HandlerPLCSwVer { get; set; }

        [CsvFixedValue(12, "LOTS", "#TESTER_PLC_SW_VER")]
        public string TesterPLCSwVer { get; set; }

        [CsvFixedValue(13, "LOTS", "#HOST_MODE")]
        public string HostMode { get; set; }
    }

    [CsvHeadNameOfMember(StructSendData.Head_value)]
    public class LotEnd
    {
        [CsvFixedValue(1, "LOTE", "#PASS_BIN_1")]
        public string PassBinP { get; set; }

        [CsvFixedValue(2, "LOTE", "#PASS_BIN_2")]
        public string PassBinA { get; set; }

        [CsvFixedValue(3, "LOTE", "#PASS_BIN_3")]
        public string PassBinB { get; set; }

        [CsvFixedValue(4, "LOTE", "#PASS_BIN_F")]
        public string PassBinF { get; set; }

        [CsvFixedValue(5, "LOTE", "#FAIL_BIN_NOTEST")]
        public string PassBinNoTest { get; set; }

        [CsvFixedValue(6, "LOTE", "#OVERALL_YIELD")]
        public string OverallYield { get; set; }

        [CsvFixedValue(7, "LOTE", "#TOTAL_INPUT")]
        public string TotalInput { get; set; }
    }

    [CsvHeadNameOfMember(StructSendData.Head_value)]
    public class Load
    {
        [CsvFixedValue(1, "TSCR", "#STATION_TYPE")]
        public int StationType { get; } = 0;

        [CsvFixedValue(2, "TSCR", "#STATION_SN")]
        public string StationSn { get; } = "IA7953-LOAD";

        [CsvFixedValue(3, "TSCR", "#MODULE_SN")]
        public string ModuleSn { get; set; }

        [CsvFixedValue(4, "TSCR", "#SOCKET_SN")]
        public string SocketSn { get; set; }

        [CsvFixedValue(5, "TSED", "#SOCKET_SN")]
        public string SocketSn2 => SocketSn;
    }

    [CsvHeadNameOfMember(StructSendData.Head_value)]
    public class Unload
    {
        [CsvFixedValue(1, "TSCR", "#STATION_TYPE")]
        public int StationType { get; } = 255;

        [CsvFixedValue(2, "TSCR", "#STATION_SN")]
        public string StationSn { get; } = "IA7953-UNLOAD";

        [CsvFixedValue(3, "TSCR", "#MODULE_SN")]
        public string ModuleSn { get; set; }

        [CsvFixedValue(4, "TSCR", "#SOCKET_SN")]
        public string SocketSn { get; set; }

        [CsvFixedValue(5, "TSCR", "#SOCKET_DISABLE")]
        public int SocketDisable { get; set; }

        [CsvFixedValue(6, "TSCR", "#RESET")]
        public int Reset { get; set; }

        [CsvFixedValue(7, "SCAN", "#MODULE_SN")]
        public string ModuleSn2 => ModuleSn;

        [CsvFixedValue(8, "SCAN", "#SOCKET_SN")]
        public string SocketSn2 => SocketSn;

        [CsvFixedValue(9, "TSED", "#SOCKET_SN")]
        public string SocketSn3 => SocketSn;
    }
}
