using System;
using System.Collections.Generic;
using CYGKit.Factory.Statistics;
using Poc2Auto.Common;

namespace Poc2Auto.Model
{
    public class SocketGroup
    {
        public const int ROW = 1;
        public const int COL = 1;

        public int Index { get; }
        public bool Enable { get; set; } = true;
        public string DisplayMember { get => $"{Enum.GetName(typeof(StationName), Index)} Socket"; }
        public Socket[,] Sockets = new Socket[ROW, COL];
        private Dut[,] _duts = new Dut[ROW, COL];

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
        public int Index { get; }

        public Dut Dut;

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
            SocketManager.Sockets.Add(index, this);
        }
    }

    public static class SocketManager
    {
        public static Dictionary<int, Socket> Sockets = new Dictionary<int, Socket>();
    }
}
