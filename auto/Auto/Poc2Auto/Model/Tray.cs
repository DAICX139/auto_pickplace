using System;
using System.Collections.Generic;
using NetAndEvent.PlcDriver;
using Poc2Auto.Common;
using CYGKit.AdsProtocol;
using CYGKit.Database;
using Poc2Auto.Database;
using CYGKit.Factory.DataBase;
using AlcUtility;
using System.Runtime.InteropServices;
using System.IO;
using System.Windows.Forms;
using System.Linq;

namespace Poc2Auto.Model
{
    public class TrayManager
    {
        public static Dictionary<TrayName, Tray> Trays { get; }
        public static Dictionary<TrayName, Position[,]> TrayCoordination { get; private set; }

        public static readonly List<TrayName> TrayNames = new List<TrayName>
        {
            TrayName.Load1,
            TrayName.Load2,
            TrayName.NG,
            TrayName.Pass1,
            TrayName.Pass2,
        };

        static TrayManager()
        {
            Trays = new Dictionary<TrayName, Tray>();
            foreach (TrayName name in Enum.GetValues(typeof(TrayName)))
            {
                Trays.Add(name, new Tray { Index = (int)name });
            }
        }

        public static void SyncTrayData(AdsDriverClient client)
        {
            // 从数据库获取各个Tray盘的data值写入PLC
            foreach (var name in TrayNames)
            {
                var result = client.WriteTrayData((int)name, DragonDbHelper.TakeTrayData((int)name), out string message);
                if (!result)
                    AlcSystem.Instance.Error($"写入PLC {name} Tray盘数据写入异常:{message}", 0, AlcErrorLevel.WARN, @"Handler");
                if (name != TrayName.Load1 && name != TrayName.Load2)
                {
                    var binregion = DragonDbHelper.GetBinRegion((int)name);
                    WriteRegions(binregion, client);
                }
            }

        }

        private static void WriteRegions(List<BinRegion> binRegions, AdsDriverClient client)
        {
            if (client == null) return;
            if (!client.IsInitOk) return;
            var regions = binRegions.Select(r => new CYGKit.AdsProtocol.Models.BinRegion
            {
                TrayIndex = r.TrayID,
                StartColume = r.StartColumn,
                StartRow = r.StartRow,
                EndColumn = r.EndColumn,
                EndRow = r.EndRow,
                Value = r.Bin,
            }).ToList();
            var ret = client.WriteBinRegion(regions, Tray.ROW, Tray.COL, out var message);
            if (ret)
            {
                //AlcSystem.Instance.ShowMsgBox("写入成功!", "Information", icon: AlcMsgBoxIcon.Information);
            }
            else
            {
                AlcSystem.Instance.ShowMsgBox("写入失败!" + message, "Error", icon: AlcMsgBoxIcon.Error);
            }
        }

        public static void ReadTrayCellsCoordination(AdsDriverClient client)
        {
            TrayCoordination = new Dictionary<TrayName, Position[,]>();
            foreach (TrayName tray in Enum.GetValues(typeof(TrayName)))
            {
                var trayCellsPosition = client.ReadTrayCellsCoordination<Position>(RunModeMgr.STLoadTrayPoint((int)tray), Tray.ROW, Tray.COL);
                TrayCoordination.Add(tray, trayCellsPosition);
                string txt = $"{DateTime.Now}\r\n";
                for (int i = 0; i < Tray.ROW; i++)
                    for (int j = 0; j < Tray.COL; j++)
                        txt += $"({i},{j})  {trayCellsPosition[i, j].XPos},{trayCellsPosition[i, j].YPos}\r\n";
                File.WriteAllText($"{Application.StartupPath}\\UiParamFiles\\{tray}TrayCellPositon.txt", txt);
            }
        }

        public static string ShowCoordinate(int row, int col, TrayName name)
        {
            if (TrayCoordination == null)
                return null;
            if (row >= 0 && row < Tray.ROW && col >= 0 && col < Tray.COL)
            {
                var position = TrayCoordination[name][row, col];
                return $"({position.XPos:N3},{position.YPos:N3})";
            }
            else
            {
                AlcSystem.Instance.ShowMsgBox($"tray坐标异常, ({row},{col})", "Error", icon: AlcMsgBoxIcon.Error);
                return "*,*";
            }
        }
    }



    public class Tray
    {
        public const int ROW = 32;
        public const int COL = 14;

        public const int S_ROW = 20;
        public const int S_Col = 8;

        public int Index { get; set; }

        public void SetData(int[,] data)
        {
            DragonDbHelper.WriteTrayData(Index, data);
        }

        public Dut TakeDut(int row, int col)
        {
            Overall.Stat.Input++;
            var data = DragonDbHelper.TakeTrayData(Index, row, col);
            if (data == null)
            {
                return new Dut { Barcode = "norecord", Result = Dut.NoTestBin };
            }
            else
            {
                return new Dut { Barcode = data.Barcode, Result = data.Bin };
            }
        }

        public void PutDut(int row, int col, Dut dut)
        {
            if (dut == null)
            {
                dut = new Dut { Barcode = "noscan", Result = 98 };
            }

            var data = new TrayData
            {
                TrayID = Index,
                Row = row,
                Column = col,
                Barcode = dut?.Barcode,
                Bin = dut?.Result ?? Dut.NoTestBin,
            };
            DragonDbHelper.WriteTrayData(data);

            if (RunModeMgr.RunMode == RunMode.AutoNormal || RunModeMgr.RunMode == RunMode.HandlerSemiAuto || RunModeMgr.RunMode == RunMode.ResetMode || RunModeMgr.GRRMode)
            {
                if (dut.Result == Dut.PassBin) Overall.Stat.Passed++;
                else if (dut.Result == Dut.NoTestBin) Overall.Stat.Failed++;
                else Overall.Stat.Failed++;
            }
        }

        public void ChangeDutFlag(int row, int col, int bin)
        {
            DragonDbHelper.AbnormalDut(Index, row, col, bin);
        }

        public void RemoveTrayDut(int row, int col)
        {
            DragonDbHelper.TakeTrayData(Index, row, col);
        }
    }

    /// <summary>
    /// Tray盘单元格坐标
    /// </summary>
    [StructLayout(LayoutKind.Sequential,Pack = 0)]
    public class Position
    {
        public double XPos { get; set; }
        public double YPos { get; set; }

    }
}
