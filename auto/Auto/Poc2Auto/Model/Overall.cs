using System;
using AlcUtility;
using CYGKit.Database;
using Poc2Auto.Database;
using CYGKit.Factory.Lot;
using Poc2Auto.Common;
using System.Collections.Generic;
using CYGKit.Factory.StressCodeKit;

namespace Poc2Auto.Model
{
    public class Overall
    {
        public static DutStatistics Stat { get; } = new DutStatistics();

        public static event Action LotInfoChanged;
        public static event Action InitOk;

        public static LotInfo LotInfo { get; private set; } = new LotInfo { LotID = "1234", StressCode = "56789" };

        static Overall()
        {
            RunModeMgr.Init();

            AlcSystem.Instance.CanReset += () => Database<DragonContext>.Inited ?
                null : @"数据库未初始化成功，请检查连接地址、密码是否正确，然后重启软件";
            AlcSystem.Instance.CanStart += () => LotInfo == null ?
                @"请先新建Lot" : null;

            EventCenter.OnHandlerPLCInitOk += TrayManager.SyncTrayData;
        }

        public static bool InitModels()
        {
            var result = Database<DragonContext>.InitDatabase(out string message);
            if (!result)
            {
                AlcSystem.Instance.ShowMsgBox(
                    @"数据库未初始化成功，请检查连接地址、密码是否正确，然后重启软件。" +
                    $"\r\n\t 更多信息：{message}",
                    @"Database", AlcMsgBoxButtons.OK, AlcMsgBoxIcon.Error);
                return false;
            }
            StationManager.LoadDatabaseData();
            LotManager<DragonContext, LotInfo>.LotChanged += () =>
              {
                  LotInfo = DragonDbHelper.GetLotInfo();
                  LotInfoChanged?.Invoke();
              };
            LotInfo = DragonDbHelper.GetLotInfo();
            InitOk?.Invoke();
            return true;
        }

        public static string StressCodeCreator()
        {
            StressCodeMgr.ShowStressCodeEditor();
            return StressCodeMgr.StressCode;
        }

        #region common data

        private static string _scanResult = "sample";
        /// <summary>
        /// 相机扫码结果
        /// </summary>
        public static string ScanResult
        {
            get
            {
                if(!RunModeMgr.CustomMode.HasFlag(CustomMode.NoSn))
                {
                    return "FNG117401THPX" + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss");
                }
                else
                {
                    return _scanResult;
                }
            }
            set
            {
                _scanResult = value;
            }
        }

        /// <summary>
        /// 挑料模式下挑选列表
        /// </summary>
        public static List<string> SelectionList = new List<string>();

        public static bool IsResetting = false;

        #endregion
    }
}
