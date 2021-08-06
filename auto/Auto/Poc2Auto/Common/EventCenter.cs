﻿using NetAndEvent.PlcDriver;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AlcUtility;
using Poc2Auto.Model;

namespace Poc2Auto.Common
{
    public static class EventCenter
    {
        /// <summary>
        /// Socket打开后触发
        /// </summary>
        public static Action SocketOpened { get; set; }
        /// <summary>
        /// 关闭Socket
        /// </summary>
        public static Action CloseSocket { get; set; }
        public static Action PickDutDone { get; set; }

        public static Action<AdsDriverClient> OnHandlerPLCInitOk { get; set; }
        public static Action OnHandlerPLCDisconnect { get; set; }

        /// <summary>
        /// PlcMode改变后出发
        /// </summary>
        public static Action<PlcMode> HandlerModeChanged { get; set; }
        public static Action<PlcMode> TesterModeChanged { get; set; }

        //=============调试用==================//
        public static Action LoadOrUnload { get; set; }
        //=============调试用==================//

        /// <summary>
        /// 运行过程中的流程信息
        /// </summary>
        public static Action<string, ErrorLevel> ProcessInfo { get; set; }

        /// <summary>
        /// 清错按钮点击后触发
        /// </summary>
        public static Action ClearError { get; set; }
        //当出需错误报警时触发
        public static Action<List<string>> ShowErrorMsgs { get; set; }
        //当某个socket屏蔽/解除屏蔽后触发
        public static Action<List<int>> SocketDisableChanged { get; set; }
        //蜂鸣器使能状态改变后触发
        public static Action<bool> EnableBuzzer { get; set; }

        public static Action isReset { get; set; }
        //各测试工站测试完成后触发
        public static Action<Station, string, int> StationTestDone { get; set; }
        //Socket一致性测试时，点击继续测试后触发
        public static Action SocketTestContinue { get; set; }
        //当TM测试超时，人为点弹框重试按钮后触发，再一次给TM发送测试命令
        public static Action<StationName> Retest { get; set; }
        //当系统状态改变后触发
        public static Action StateChanged { get; set; }
        //当主界面点击TM复位后触发
        public static Action TMReset { get; set; }
        //当主界面点击启用复位后触发
        public static Action<bool> EnableTM { get; set; }

        // 相机实时显示委托
        public static Action<int, bool> LeftCamera { get; set; }
        public static Action<int, bool> RightCamera { get; set; }
        public static Action<int, bool> DownCamera { get; set; }

        public static void ParallelInvoke<T>(this T action, params object[] args) where T : MulticastDelegate
        {
            var threads = new List<Thread>();
            foreach (var invoke in action.GetInvocationList())
            {
                var invoke1 = invoke;
                var thread = new Thread(new ThreadStart(() => invoke1.DynamicInvoke(args)));
                thread.Start();
                threads.Add(thread);
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        public static void ParallelInvokeAsync<T>(this T action, params object[] args) where T : MulticastDelegate
        {
            foreach (var invoke in action.GetInvocationList())
            {
                var invoke1 = invoke;
                Task.Run(() => invoke1.DynamicInvoke(args));
            }
        }
    }
}
