using AlcUtility;
using AlcUtility.PlcDriver.CommonCtrl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Poc2Auto.Common
{
    public static class TricolorLamp
    {
        static System.Timers.Timer timer;
        private static bool _isPaused;
        private static bool _isError;
        private static bool _isTrace;
        public static PLCActiveEvent TesterActiveEvent { get; set; }
        public static PLCActiveEvent HandlererActiveEvent { get; set; }
        public static void Init()
        {
            //timer = new System.Timers.Timer() { Interval = 1000 };
            //timer.Elapsed += Timer_Elapsed;
            //timer.Enabled = false;
            RunModeMgr.HandlerStateChanged += StateChanged;
            RunModeMgr.TesterStateChanged += StateChanged;

            //Thread thread = new Thread(ReadData);
            //thread.IsBackground = true;
            //thread.Start();
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_isPaused)
            {
                RunModeMgr.LampYellow = !RunModeMgr.LampYellow;
            }

            if (_isError)
            {
                RunModeMgr.LampRed = true;
                RunModeMgr.Buzzer = !RunModeMgr.Buzzer;
            }

            if (_isTrace)
            {
                RunModeMgr.LampGreen = !RunModeMgr.LampGreen;
            }
        }

        private static void StateChanged()
        {
            // Idle 状态下：亮黄灯
            if ((SYSTEM_STATUS)RunModeMgr.HandlerCurrentState == SYSTEM_STATUS.Idle && (SYSTEM_STATUS)RunModeMgr.TesterCurrentState == SYSTEM_STATUS.Idle)
            {
                timer.Enabled = false;
                _isPaused = false;
                _isError = false;
                _isTrace = false;

                RunModeMgr.LampGreen = false;
                RunModeMgr.LampRed = false;
                RunModeMgr.LampYellow = true;
            }
            // Running 状态下：亮绿灯
            else if ((SYSTEM_STATUS)RunModeMgr.HandlerCurrentState == SYSTEM_STATUS.Running || (SYSTEM_STATUS)RunModeMgr.TesterCurrentState == SYSTEM_STATUS.Running)
            {
                timer.Enabled = false;
                _isPaused = false;
                _isError = false;
                _isTrace = false;

                RunModeMgr.LampGreen = true;
                RunModeMgr.LampRed = false;
                RunModeMgr.LampYellow = false;
            }
            // Pause 状态下：黄灯闪烁
            else if ((SYSTEM_STATUS)RunModeMgr.HandlerCurrentState == SYSTEM_STATUS.Paused || (SYSTEM_STATUS)RunModeMgr.TesterCurrentState == SYSTEM_STATUS.Paused)
            {
                timer.Enabled = true;
                timer.Interval = 1000;

                _isPaused = true;

                RunModeMgr.LampGreen = false;
                RunModeMgr.LampRed = false;
                RunModeMgr.LampYellow = true;
            }
            else if ((SYSTEM_STATUS)RunModeMgr.HandlerCurrentState == SYSTEM_STATUS.Error || (SYSTEM_STATUS)RunModeMgr.TesterCurrentState == SYSTEM_STATUS.Error)
            {
                //if (RunModeMgr.HandlerErrorLevel >= AlcErrorLevel.WARN || RunModeMgr.TesterErrorLevel >= AlcErrorLevel.WARN)
                //{
                //    _isError = true;
                //    timer.Enabled = true;
                //    timer.Interval = 1000;

                //    RunModeMgr.LampGreen = false;
                //    RunModeMgr.LampRed = true;
                //    RunModeMgr.LampYellow = false;
                //}
            }
        }

        private static void LampInit()
        {
            RunModeMgr.LampGreen = false;
            RunModeMgr.LampRed = false;
            RunModeMgr.LampYellow = false;
            RunModeMgr.Buzzer = false;
        }

        private static void ReadData()
        {
            while (true)
            {
                while (true)
                {
                    //if (RunModeMgr.testerClient != null && RunModeMgr.testerClient.IsInitOk && RunModeMgr.testerClient.IsConnected)
                    //{
                    //    TesterActiveEvent = RunModeMgr.testerClient.GetSysInfoCtrl.ActiveEvent;
                    //}

                    //if (RunModeMgr.HandlerClient != null && RunModeMgr.HandlerClient.IsInitOk && RunModeMgr.HandlerClient.IsConnected)
                    //{
                    //    HandlererActiveEvent = RunModeMgr.HandlerClient.GetSysInfoCtrl.ActiveEvent;
                    //}
                    Thread.Sleep(100);
                }
            }
        }
    }
}
