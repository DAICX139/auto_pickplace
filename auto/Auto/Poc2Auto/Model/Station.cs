using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CYGKit.Factory.OtherUI;
using Poc2Auto.Common;
using Poc2Auto.Database;
using AlcUtility;
using System.Timers;
using Poc2Auto.MTCP;

namespace Poc2Auto.Model
{
    public class StationManager
    {
        public static event Action EventRotate;
        public static event Action EventRotateDone;
        public static Dictionary<StationName, Station> Stations { get; private set; }
        public static List<StationName> TestStations;
        public static List<StationName> RotationStations;
        private static readonly object RotationLock = new object();
        public static bool AllStationEmpty
        {
            get
            {
                var allEmpty = true;

                //判断是否所有Station Ready
                foreach (var name in RotationStations)
                {
                    if (allEmpty && !Stations[name].Empty) allEmpty = false;
                }

                return allEmpty;
            }
        }

        static StationManager()
        {
            TestStations = new List<StationName>
            {
                StationName.Test1_LIVW,
                StationName.Test2_DTGT,
                StationName.Test3_Backup,
                StationName.Test4_BMPF,
            };

            RotationStations = new List<StationName>
            {
                StationName.Default,
                StationName.Test1_LIVW,
                StationName.Test2_DTGT,
                StationName.Test3_Backup,
                StationName.Test4_BMPF,
            };

            Stations = new Dictionary<StationName, Station>();
            foreach (StationName name in Enum.GetValues(typeof(StationName)))
            {
                var station = new Station(name);
                station.ReadyToRotate += RotateIfAllReady;
                Stations.Add(name, station);
            }

            if (CYGKit.GUI.Common.IsDesignMode())
                return;
            var enable = ConfigMgr.Instance.WithTM;
            foreach (var station in TestStations)
            {
                Stations[station].Enable = DragonDbHelper.GetStationEnable(station.ToString());
            }
        }

        public static void LoadDatabaseData()
        {
            LoadOnlineDut();
            LoadStationStat();
            LoadSocketStat();
        }

        private static void LoadOnlineDut()
        {
            var context = new DragonContext();
            var onlineDuts = context.OnlineDuts.ToList();
            foreach (var onlineDut in onlineDuts)
            {
                var socket = GetSocketByLocationId(onlineDut.SocketID, out var stationName, out var row, out var col);
                if (socket == null) return;
                onlineDut.StationID = (int)stationName;
                onlineDut.Row = row;
                onlineDut.Column = col;
                var barCode = onlineDut.Barcode;
                var dut = new Dut { Barcode = onlineDut.Barcode.Split(' ', '—', ' ')[0] };
                onlineDut.Barcode = barCode;
                dut.SetTestResultByString(onlineDut.TestResult);
                socket.Dut = dut;
            }
            context.SaveChanges();
        }

        private static void LoadStationStat()
        {
            foreach (var emName in RotationStations)
            {
                Stations[emName].Stat.BindDataBase<DragonContext>();
                Stations[emName].Stat.UploadFromDb();
            }
        }

        private static void LoadSocketStat()
        {
            using (var context = new DragonContext())
            {
                var stats = context.SocketStats;
                for (int i = 0; i < stats.Count(); i++)
                {
                    foreach (var index in SocketManager.Sockets.Keys)
                    {
                        var record = stats.FirstOrDefault(p => p.Index == index);
                        record?.UpdateDataSrcFromDb(SocketManager.Sockets[index].Stat);
                    }
                }
            }
        }

        public static bool RotationEnable => ConfigMgr.Instance.RotationEnable;

        public static Socket GetSocketByLocationId(int location, out StationName stationName, out int row, out int col)
        {
            row = col = 0;
            foreach (var station in Stations.Values)
            {
                row = col = 0;
                stationName = station.Name;
                foreach (var socket in station.SocketGroup.Sockets)
                {
                    if (socket.Index == location) return socket;
                    col++;
                    if (col == SocketGroup.COL)
                    {
                        col = 0;
                        row++;
                    }
                }
            }

            stationName = StationName.Default;
            return null;
        }

        private static void RotateIfAllReady()
        {
            lock (RotationLock)
            {
                var allEmpty = true;

                //判断是否所有Station Ready
                foreach (var name in RotationStations)
                {
                    if (!Stations[name].IsReadyToRotate) return;
                    if (allEmpty && !Stations[name].Empty) allEmpty = false;
                }

                //所有工位都没有产品，上下料工位也处于Idle状态，也不应该旋转
                if (allEmpty && Stations[StationName.Default].Status == StationStatus.Idle) return;

                //开始旋转
                foreach (var name in RotationStations)
                {
                    Stations[name].Status = StationStatus.Rotating;
                }
            }

            //旋转动作
            EventRotate?.Invoke();

            //交换各个工站数据
            SocketGroup temp = Stations[RotationStations.Last()].SocketGroup;
            for (int i = RotationStations.Count - 1; i > 0; i--)
            {
                Stations[RotationStations[i]].SocketGroup = Stations[RotationStations[i - 1]].SocketGroup;
            }
            Stations[RotationStations[0]].SocketGroup = temp;

            //更新数据库
            DragonDbHelper.RotateDut();
            EventCenter.ProcessInfo?.Invoke($"数据库更新完成", ErrorLevel.Debug);

            //旋转完成
            foreach (var name in RotationStations)
            {
                Stations[name].Status = StationStatus.RotateDone;
            }

            if (EventRotateDone != null)
                Parallel.ForEach(EventRotateDone.GetInvocationList(), act => ((Action)act).Invoke());
        }
    }

    public class Station : StationBase
    {
        public new StationName Name { get; private set; }
        public SocketGroup SocketGroup { get; set; }

        public Timer TestTimer;
        public new string IP
        {
            get => base.IP;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    base.IP = "tcp://*.*";
                    ConnectState = false;
                }
                else
                {
                    base.IP = $"tcp://{value}";
                    ConnectState = true;
                }
            }
        }

        private StationStatus _status;
        public event Action ReadyToRotate;
        public bool IsReadyToRotate => _status == StationStatus.Idle || _status == StationStatus.Disabled ||
            _status == StationStatus.StartFailed || _status == StationStatus.Done || _status == StationStatus.SocketDisabled;
        public new StationStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                base.Status = value.ToString();
                if (IsReadyToRotate)
                {
                    ReadyToRotate?.BeginInvoke(null, null);
                    WorkTime = (DateTime.Now - _workStartTime).TotalSeconds;
                }
                else if (_status == StationStatus.RotateDone)
                    _workStartTime = DateTime.Now;
            }
        }
        public int TestTimesForGRR { get; set; }
        //工站超时时间计数
        public int TimeOutTotal { get; set; }

        private DateTime _workStartTime = DateTime.Now;

        public Station(StationName name)
        {
            Name = name;
            base.Name = name.ToString();
            SocketGroup = new SocketGroup((int)name);
            Stat.BindDataBase<DragonContext>();
            IP = "";
            Status = Enable ? StationStatus.Idle : StationStatus.Disabled;
            TestTimer = new Timer { Interval = 1000, };
            TestTimer.Elapsed += (o, e) => Tick(o, e);
        }

        private void Tick(object o, ElapsedEventArgs e)
        {
            if (++TimeOutTotal > RunModeMgr.TMTestTimeOut)
            {
                TimeOutTotal = 0;
                TestTimer.Enabled = false;
                if (Status == StationStatus.Testing)
                {
                    var btnResult = AlcSystem.Instance.ShowMsgBox($"检测到{Name}工站没有回复测试完成，请检查该工站，问题解决后可做以下选择：\r\n\r\n" +
                        $"中止：中止整个流程\r\n" +
                        $"重试：重新给该工站发送测试命令\r\n" +
                        $"继续：屏蔽该工站，继续走向下一个流程测试", "TM Error",
                        buttons: AlcMsgBoxButtons.AbortRetryContinue,
                        defaultButton: AlcMsgBoxDefaultButton.Button2,
                        icon: AlcMsgBoxIcon.Error);
                    if (btnResult == AlcMsgBoxResult.Retry)
                    {
                        EventCenter.Retest?.Invoke(Name);
                    }
                    else if (btnResult == AlcMsgBoxResult.Continue)
                    {
                        Enable = false;
                        Status = StationStatus.Disabled;
                    }
                }
            }
        }

        #region Get Data

        public bool Empty
        {
            get
            {
                foreach (var socket in SocketGroup.Sockets)
                {
                    if (socket.Dut != null) return false;
                }
                return true;
            }
        }

        public Dut DutAt(int row, int col) => SocketGroup.Sockets[row, col].Dut;

        public string[,] Barcodes
        {
            get
            {
                var barcodes = new string[SocketGroup.ROW, SocketGroup.COL];
                for (int i = 0; i < SocketGroup.ROW; i++)
                    for (int j = 0; j < SocketGroup.COL; j++)
                    {
                        barcodes[i, j] = SocketGroup.Sockets[i, j].Dut?.Barcode;
                    }
                return barcodes;
            }
        }

        public int TestResult(int row, int col)
        {
            if (SocketGroup.Sockets[row, col].Dut == null) return 0;
            return SocketGroup.Sockets[row, col].Dut.Result;
        }

        #endregion Get Data

        #region Set Data

        public Dut TakeDut(int row, int col)
        {
            var socket = SocketGroup.Sockets[row, col];
            var dut = socket.Dut;
            socket.Dut = null;
            if (Name == StationName.Unload)
                DragonDbHelper.UnloadDut(socket.Index);
            return dut;
        }

        public void PutDut(int row, int col, Dut dut)
        {
            if (dut == null) return;

            var socket = SocketGroup.Sockets[row, col];
            socket.Dut = dut;
            Stat.Input++;
            if (Name == StationName.Load)
                DragonDbHelper.LoadDut(socket.Index, row, col, dut);
        }

        public static void MoveDut(Station stationSource, Station stationDest)
        {
            for (int row = 0; row < SocketGroup.ROW; row++)
                for (int col = 0; col < SocketGroup.COL; col++)
                {
                    var dut = stationSource.TakeDut(row, col);
                    if (dut == null) continue;
                    stationDest.PutDut(row, col, dut);
                    DragonDbHelper.MoveDut(stationSource.SocketGroup.Sockets[row, col].Index,
                        stationDest.SocketGroup.Sockets[row, col].Index,
                        stationSource.Name == StationName.Load);
                }
        }

        public void PutDutTo(Station stationDest)
        {
            MoveDut(this, stationDest);
        }

        public void TakeDutFrom(Station stationSource)
        {
            MoveDut(stationSource, this);
        }

        public void SetTestResult(int[,] results)
        {
            for (int i = 0; i < SocketGroup.ROW; i++)
                for (int j = 0; j < SocketGroup.COL; j++)
                {
                    var result = results[i, j];
                    if (result == Dut.SkipBin) continue;
                    var socket = SocketGroup.Sockets[i, j];
                    var dut = socket.Dut;
                    if (dut == null) continue;
                    dut.TestResult[Name] = result;
                    //var changeResult = dut.ChangeDutTestResult(result);
                    //if (changeResult < 0)
                    //{
                    //    //RunModeMgr.FailDut++;
                    //    dut.TestResult[Name] = Dut.Fail_All;
                    //    //AlcSystem.Instance.ShowMsgBox($"TM{Name}工站测试出错！", "Error", icon: AlcMsgBoxIcon.Error);
                    //    Status = StationStatus.TestError;
                    //}
                    //else
                    //    dut.TestResult[Name] = changeResult;

                    //工站统计
                    if (result == Dut.PassBin) Stat.Passed++;
                    else if (result == Dut.NoTestBin) Stat.Untested++;
                    else Stat.Failed++;

                    //更新数据库
                    DragonDbHelper.SetTestResult(socket.Index, dut.Barcode, dut.GetTestResultString());

                    //工站统计
                    //SetStationStat(result);

                    //socket统计
                    SetSocketStat(socket, result);

                    //工站bin统计
                    DragonDbHelper.AddOrUpdateBin(Name.ToString(), Overall.LotInfo?.LotID, result);

                    //获取bin并入库
                    if (Name == StationName.Test4_BMPF)
                    {
                        Task.Run(() =>
                        {
                            RunModeMgr.UnloadTotal++;
                            //给MTCP发送unload数据，返回该DUT测试的总结果
                            if (MTCPHelper.SendMTCPLotUnload(socket.Dut.Barcode, SocketGroup.Enable ? 1 : 0, out int eCode, out string eString, out string bin))
                            {
                                EventCenter.ProcessInfo?.Invoke($"MTCP Unload 发送成功，Socket SN:{socket.Dut.Barcode}", ErrorLevel.Debug);
                                AlcSystem.Instance.Log($"从MTCP获取到的总结果为{bin}", "MTCP");
                                EventCenter.ProcessInfo?.Invoke($"从MTCP获取到的总结果为 {bin}", ErrorLevel.Debug);
                            }
                            else
                            {
                                EventCenter.ProcessInfo?.Invoke($"MTCP Unload 发送失败，Socket SN:{socket.Dut.Barcode}", ErrorLevel.Fatal);
                                AlcSystem.Instance.Error($"MTCP Unload send failed, error code:{eCode}, error message:{eString}.", 0, AlcErrorLevel.WARN, "MTCP");
                            }
                            //dut.GetBin();
                            var getBin = bin;
                            bin = null;
                            if (string.IsNullOrEmpty(getBin) || getBin == "999")
                                dut.Result = Dut.Fail_All;
                            else if (getBin == "F")
                                dut.Result = Dut.Fail_All;
                            else
                                dut.Result = int.Parse(getBin);

                            if (dut.Result == Dut.PassBin)
                                RunModeMgr.YieldDut++;
                            else
                                RunModeMgr.FailDut++;
                            DragonDbHelper.SetTotalBin(dut.Result);
                            DragonDbHelper.AddOrUpdateBin(Overall.LotInfo?.LotID, dut.Result);
                            DragonDbHelper.SetBin(socket.Index, dut.Barcode, dut.Result);
                        });
                    }
                }
        }

        /// <summary>
        /// 根据result和分bin规则，更新SocketStat
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="testResult"></param>
        void SetSocketStat(Socket socket, int testResult)
        {
            switch (testResult)
            {
                case Dut.PassBin:
                    socket.Stat.SiteStats[Name.ToString()].Passed++;
                    break;

                case Dut.NoTestBin:
                    break;

                default:
                    socket.Stat.SiteStats[Name.ToString()].Failed++;
                    break;
            }
        }

        /// <summary>
        /// 根据result和分bin规则，更新统计
        /// </summary>
        /// <param name="result"></param>
        void SetStationStat(int result)
        {
            if (result == Dut.PassBin) Stat.Passed++;
            else if (result == Dut.NoTestBin) Stat.Untested++;
            else Stat.Failed++;
        }

        #endregion Set Data
    }
}
