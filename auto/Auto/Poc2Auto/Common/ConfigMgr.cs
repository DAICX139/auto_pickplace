using AlcUtility;

namespace Poc2Auto.Common
{
    public class ConfigMgr : Configur
    {
        public static ConfigMgr Instance { get; } = new ConfigMgr();

        private ConfigMgr()
        {
        }

        public RunMode RunMode
        {
            get => (RunMode)getInt("RunMode", 0);
            set => addOrUpdate("RunMode", ((int)value).ToString());
        }

        public CustomMode CustomMode
        {
            get => (CustomMode)getInt("CustomMode", 0);
            set => addOrUpdate("CustomMode", ((int)value).ToString());
        }

        public SocketStatus SocketStatus
        {
            get => (SocketStatus)getInt("SocketStatus", 0);
            set => addOrUpdate("SocketStatus", ((int)value).ToString());
        }

        public bool RotationEnable => getInt("RotationEnable", 1) == 1;
        public bool WithTM { get { return getInt("WithTM", 1) == 1; } set { addOrUpdate("WithTM", (value ? 1 : 0).ToString()); } }
        public bool CanMoveDuringTest => getInt("CanMoveDuringTest", 1) == 1;

        public int TMResetTimeout => getInt("TMResetTimeout", 120000);
        public int TMStartTestTimeout => getInt("TMStartTestTimeout", 10000);

        public string ConnectionStrings => getString("ConnectionStrings",
            "server = localhost; user id = root; password=12345678; database=CYG7953Auto");

        public string MtcpIP => getString("MTCP_IP", "169.254.111.0");
        public int MtcpPort => getInt("MTCP_Port", 61807);

        public string DefaultMode => getString("DefaultMode", "AutoNormal");

        public string HandlerDefaultRecipe
        {
            get => getString("HandlerRecipeFile", "");
            set => addOrUpdate("HandlerRecipeFile", value);
        }

        public string TesterDefaultRecipe
        {
            get => getString("TesterRecipeFile", "");
            set => addOrUpdate("TesterRecipeFile", value);
        }

        //屏蔽socketIds
        public string DisableSocketId { get { return getString("DisableSocketId", ""); } set { addOrUpdate("DisableSocketId", value); } }

        public bool EnableClientMTCP { get { return getInt("EnableClientMTCP", 1) == 1; } set { addOrUpdate("EnableClientMTCP", (value ? 1 : 0).ToString()); } }
        public bool TurnOffBuzzer { get { return getInt("TurnOffBuzzer", 1) == 1; } set { addOrUpdate("TurnOffBuzzer", (value ? 1 : 0).ToString()); } }

        public string CurrentImageType { get { return getString("CurrentImageType", ""); } set { addOrUpdate("CurrentImageType", value); } }

        public bool Socket1Enable { get { return getInt("Socket1Enable", 1) == 1; } set { addOrUpdate("Socket1Enable", (value ? 1 : 0).ToString()); } }
        public bool Socket2Enable { get { return getInt("Socket2Enable", 1) == 1; } set { addOrUpdate("Socket2Enable", (value ? 1 : 0).ToString()); } }
        public bool Socket3Enable { get { return getInt("Socket3Enable", 1) == 1; } set { addOrUpdate("Socket3Enable", (value ? 1 : 0).ToString()); } }
        public bool Socket4Enable { get { return getInt("Socket4Enable", 1) == 1; } set { addOrUpdate("Socket4Enable", (value ? 1 : 0).ToString()); } }
        public bool Socket5Enable { get { return getInt("Socket5Enable", 1) == 1; } set { addOrUpdate("Socket5Enable", (value ? 1 : 0).ToString()); } }

        public int MTCPTimeOut { get => getInt("MTCPTimeOut", 150); set => addOrUpdate("MTCPTimeOut", value.ToString()); }
        public bool Retest { get { return getInt("Retest", 1) == 1; } set { addOrUpdate("Retest", (value ? 1 : 0).ToString()); } }
        public string MTCPDataPath { get { return getString("MTCPDataPath", ""); } set { addOrUpdate("MTCPDataPath", value); } }
        public string AuditFile { get { return getString("AuditFile", ""); } set { addOrUpdate("AuditFile", value); } }
        public bool LoadTrayConfig { get => getInt("LoadTrayConfig", 1) == 1; set { addOrUpdate("LoadTrayConfig", (value ? 1 : 0).ToString()); } }  //==1 大tray ==0 小tray
        public bool NGTrayConfig { get => getInt("NGTrayConfig", 1) == 1; set { addOrUpdate("NGTrayConfig", (value ? 1 : 0).ToString()); } }  //==1 大tray ==0 小tray
        public bool UnloadTrayConfig { get => getInt("UnloadTrayConfig", 1) == 1; set { addOrUpdate("UnloadTrayConfig", (value ? 1 : 0).ToString()); } }  //==1 大tray ==0 小tray
        public int StationTestFailTimes { get => getInt("StationTestFailTimes", 5); set => addOrUpdate("StationTestFailTimes", value.ToString()); }
        public int SocketTestFailTimes { get => getInt("SocketTestFailTimes", 5); set => addOrUpdate("SocketTestFailTimes", value.ToString()); }
        public bool DoorLockControl { get => getInt("DoorLockControl", 1) == 1; set { addOrUpdate("DoorLockControl", (value ? 1 : 0).ToString()); } }  //安全门锁检测 False ：无效 ,True：有效
        public int DUTPassCount { get => getInt("DUTPassCount", 0); set => addOrUpdate("DUTPassCount", value.ToString()); }
        public int DUTFailCount { get => getInt("DUTFailCount", 0); set => addOrUpdate("DUTFailCount", value.ToString()); }
    }

}
