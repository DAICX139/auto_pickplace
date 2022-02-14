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
        PNP,
        Test1_LIVW,
        Test2_NFBP,
        Test3_KYRL,
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
        SocketClosed,
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
        Load1 = 1,
        Load2,
        NG,
        Pass1,
        Pass2,
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
        DEBUG,
        INFO,
        WARNING,
        FATAL,
    }

    public enum CtrlType
    {
        Both,
        Handler,
        Tester,
    }
}
