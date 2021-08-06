namespace Poc2Auto.Common
{
    public enum ModuleTypes
    {
        Handler,
        Tester,
        Vision,
        TM,
    }

    public enum StationName
    {
        Load,
        Unload,
        Default,
        Test1_LIVW,
        Test2_DTGT,
        Test3_Backup,
        Test4_BMPF,
    }

    public enum StationStatus
    {
        Idle,
        Starting,
        StartFailed,
        Testing,
        Done,
        Rotating,
        RotateDone,
        SocketOpened,
        WaitUnload,
        WaitLoad,
        LoadDone,
        WaitSocketClose,
        Disabled,
        SocketDisabled,
        WaitSocketOpen,
        TestError,
        Waiting,
    }

    /// <summary>
    /// Tray位置编号。
    /// </summary>
    public enum TrayName
    {
        LoadL = 1,
        LoadR,
        NG,
        UnloadL,
        UnloadR,
    }

    /// <summary>
    /// TM测试结束后返回产品的Bin值定义
    /// </summary>
    public enum TMBinValue
    {
        BIN_ERROR = -1,
        BIN_PASS = 1,
        BIN_VA1PASS,
        BIN_VA2PASS,
        BIN_FAIL,
    }

    public enum ErrorLevel
    {
        Debug,
        Warning,
        Fatal,
    }
}
