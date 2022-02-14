using System;
using System.Collections.Generic;
using CYGKit.Factory.Statistics;
using Newtonsoft.Json;
using Poc2Auto.Common;

namespace Poc2Auto.Model
{
    public class SocketGroup
    {
        public const int ROW = 1;
        public const int COL = 1;
        [JsonProperty("Index")]
        public int Index { get; }
        [JsonProperty("Enable")]
        public bool Enable { get; set; } = true;
        /// <summary>
        /// Socket连续测试失败次数
        /// </summary>
        [JsonProperty("SocketTestFailTimes")]
        public int TestFailTimes { get; set; }
        [JsonIgnore]
        public string DisplayMember { get => $"{Enum.GetName(typeof(StationName), Index)} Socket"; }
        public Socket[,] Sockets = new Socket[ROW, COL];
        private Dut[,] _duts = new Dut[ROW, COL];
        [JsonProperty("Duts")]
        public Dut[,] Duts
        {
            get
            {
                for (var i = 0; i < ROW; i++)
                for (var j = 0; j < COL; j++)
                {
                    _duts[i, j] = Sockets[i, j].Dut;
                }

                return _duts;
            }
            set
            {
                _duts = value;
            }
        }

        public SocketGroup(int index)
        {
            Index = index;
            for (var i = 0; i < ROW; i++)
            for (var j = 0; j < COL; j++)
            {
                var subIndex = (index * ROW + i) * COL + j;
                Sockets[i, j] = new Socket(subIndex);
            }
        }
    }

    public class Socket
    {
        [JsonProperty("SocketIndex")]
        public int Index { get; }

        public Dut Dut;
        [JsonIgnore]
        public SocketStat Stat { get; set; }
        public Socket(int index)
        {
            Index = index;
            Stat = new SocketStat(index);

            var siteSites = new Dictionary<string, SiteStatUnit>();
            for (int i = 0; i < StationManager.RotationStations.Count; i++)
            {
                siteSites.Add(StationManager.RotationStations[i].ToString(), new SiteStatUnit());
            }

            Stat.SiteStats = siteSites;
            Stat.UpdateDb += Database.DragonDbHelper.AddOrUpdateSocketStat;
            if (!SocketManager.Sockets.ContainsKey(index))
            {
                SocketManager.Sockets.Add(index, this);
            }
        }
    }

    public static class SocketManager
    {
        public static Dictionary<int, Socket> Sockets = new Dictionary<int, Socket>();
    }
}
