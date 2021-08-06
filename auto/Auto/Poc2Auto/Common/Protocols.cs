using System.Collections.Generic;
using Newtonsoft.Json;
using Poc2Auto.Model;
using System.Linq;

namespace Poc2Auto.Common
{
    public class MessageNames
    {
        #region HandlerPLC
        public const string CMD_TrayDutStatus = "30";
        public const string CMD_PickDutFromTray = "31";
        public const string CMD_PutDutToSocket = "32";
        public const string CMD_LoadPutDutToTray = "33";
        public const string CMD_InformSocketData = "34";
        public const string CMD_PickDutFromSocket = "35";
        public const string CMD_PutDutToTray = "36";
        public const string CMD_CompelteFinish = "37";
        public const string CMD_GRRStrat = "40";
        public const string CMD_GRREnd = "41";


        public const string CMD_BottomCamera = "60";
        public const string CMD_LeftTopCamera = "61";
        public const string CMD_RightTopCamera = "62";
        #endregion HandlerPLC

        #region TesterPLC
        public const string CMD_SocketOpened = "30";
        public const string CMD_SocketClosed = "31";
        public const string CMD_RotateDone = "32";
        public const string CMD_CloseSocket = "33";
        public const string CMD_Rotate = "34";
        public const string CMD_SocketDisable = "35";
        public const string CMD_PickupDutDone  = "36";
        #endregion TesterPLC

        #region TM
        public const string CMD_TestStart = "teststart";
        public const string CMD_TestDone = "testdone";
        #endregion TM
    }

    public class PLCParamNames
    {
        //视觉用
        public const string PLCSend = "PLCSend";
        public const string PLCRecv = "PLCRecv";
        //ALC用
        public const string TrayID = "TrayID"; 
        public const string Results = "Results";
        public const string TrayRows = "TrayRows";
        public const string TrayCols = "TrayCols";
        public const string Bins = "Bins";

    }

    public class StartTestParam
    {
        [JsonProperty("dutsn")]
        public List<string> DutSn { get; set; }

        [JsonProperty("socketsn")]
        public string SocketSn { get; set; }

        [JsonProperty("stresscode")]
        public string StressCode { get; set; }

        [JsonProperty("lotid")]
        public string LotId { get; set; }

        [JsonIgnore]
        public string[,] DutSnArray
        {
            set
            {
                DutSn = new List<string>();
                foreach (var barcode in value)
                {
                    DutSn.Add(barcode);
                }
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class TestResult
    {
        [JsonProperty("result")]
        public List<int> Result { get; set; }

        [JsonProperty("socketSn")]
        public string SocketSn { get; set; }

        [JsonIgnore]
        public int[,] ResultArray
        {
            get
            {
                int[,] ret = new int[SocketGroup.ROW, SocketGroup.COL];
                for (var row = 0; row < SocketGroup.ROW; row++)
                    for (var col = 0; col < SocketGroup.COL; col++)
                    {
                        var index = row * SocketGroup.COL + col;
                        ret[row, col] = Result[index];
                    }

                return ret;
            }
        }

        public bool AllOK => Result?.All(r => r == Dut.PassBin) ?? false;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
