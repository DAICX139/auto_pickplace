using AlcUtility;
using AlcUtility.PlcDriver.CommonCtrl;
using Poc2Auto.Common;
using System;

namespace VisionFlows
{
    [Serializable]
    public class PlcPara
    {
        public static PlcPara Instance = new PlcPara();
        public double P2PVel { get; set; }
        public double JogVel { get; set; }
        public double HomeVel { get; set; }
        public int Timeout { get; set; }
    }

    public class Plc
    {
        public static IPlcDriver PlcDriver { get; private set; }

        static Plc()
        {
            PlcDriver = VisionPlugin.GetInstance().PlcDriver;
        }

        /// <summary>
        /// Plc模式控制，标定需要切换到手动模式
        /// </summary>
        /// <param name="model">1:Manual 2:Auto</param>
        public static void SetPlcMode(int model)
        {
            SystemInfoCtrl systemInfo = PlcDriver?.GetSysInfoCtrl;
            systemInfo?.ModeCtrl(model);
        }

        public static SingleAxisCtrl GetAxis(int id)
        {
            return PlcDriver?.GetSingleAxisCtrl(id);
        }

        public static MessageSendResult SendMessage(MessageHandler handler, PLCRecv plcRecv)
        {
            try
            {
                var temp = new double[7] { plcRecv.XPos, plcRecv.YPos, plcRecv.ZPos, plcRecv.RPos, plcRecv.Result, plcRecv.BinValue , plcRecv.RecvSocketID};
                handler.CmdParam.KeyValues[PLCParamNames.PLCRecv].Value = temp;
                return handler.SendMessage(new ReceivedData()
                {
                    ModuleId = ModuleTypes.Handler.ToString(),
                    Data = new MessageData() { Param = handler.CmdParam }
                }, -2);
            }
            catch (Exception ex)
            {
                Flow.Log(ex.Message + Environment.NewLine + ex.StackTrace);
                return new MessageSendResult();
            }
        }

        public static AxisStatus GetAxisStatus(int id)
        {
            try
            {
                SingleAxisCtrl axis = PlcDriver?.GetSingleAxisCtrl(id);
                AxisStatus axisStatus = new AxisStatus();
                //
                axisStatus.Enable = axis.Info.PowerStatus;
                axisStatus.Homed = axis.Info.Homed;
                axisStatus.Busy = axis.Info.Moving;
                axisStatus.Done = axis.Info.MoveFinish;
                axisStatus.Error = axis.Info.Error;
                axisStatus.ErrorID = axis.Info.ErrorId;
                axisStatus.ActPos = axis.Info.ActPos;
                axisStatus.ActVel = axis.Info.ActVel;

                return axisStatus;
            }
            catch
            {
                return new AxisStatus();
            }
        }

        public static MessageSendResult AxisAbsGo(int id, double pos, double vel, bool isBlock = false, int timeout = -1)
        {
            SingleAxisCtrl axis = PlcDriver?.GetSingleAxisCtrl(id);
            return axis.AbsGo(pos, vel, isBlock, timeout);
        }

        public static MessageSendResult AxisRelGo(int id, double dis, double vel, bool isBlock = false, int timeout = -1)
        {
            SingleAxisCtrl axis = PlcDriver.GetSingleAxisCtrl(id);
            return axis.RelGo(dis, vel, isBlock, timeout);
        }

        public static MessageSendResult AxisJogGo(int id, double vel, bool onoff)
        {
            SingleAxisCtrl axis = PlcDriver.GetSingleAxisCtrl(id);
            return axis.JogGo(vel, onoff);
        }

        public static MessageSendResult AxisGoHome(int id, bool isBlock = false, int timeout = -1)
        {
            SingleAxisCtrl axis = PlcDriver.GetSingleAxisCtrl(id);
            var ret = axis.GoHome(PlcPara.Instance.HomeVel, isBlock, timeout);
            return ret;
        }

        public static MessageSendResult AxisStop(int id, bool isBlock = false, int timeout = -1)
        {
            SingleAxisCtrl axis = PlcDriver.GetSingleAxisCtrl(id);
            return axis.Stop(isBlock, timeout);
        }

        public static MessageSendResult AxisReset(int id, bool isBlock = false, int timeout = -1)
        {
            SingleAxisCtrl axis = PlcDriver.GetSingleAxisCtrl(id);
            return axis.Reset(isBlock, timeout);
        }

        public static int AxisEnable(int id, bool isEnable = false)
        {
            SingleAxisCtrl axis = PlcDriver.GetSingleAxisCtrl(id);
            return axis.PowerCtrl(isEnable);
        }

        /// <summary>
        /// 获取指定气缸
        /// </summary>
        /// <param name="id">气缸ID</param>
        /// <returns></returns>
        public static CylinderCtrl GetCylinder(int id)
        {
            return PlcDriver?.GetCylinderCtrl(id);
        }

        public static MessageSendResult CylinderToBase(int id, bool isBlock = false, int timeout = -1)
        {
            CylinderCtrl cylinderCtrl = PlcDriver.GetCylinderCtrl(id);
            return cylinderCtrl.MoveToBase(isBlock, timeout);
        }

        public static MessageSendResult CylinderToWork(int id, bool isBlock = false, int timeout = -1)
        {
            CylinderCtrl cylinderCtrl = PlcDriver?.GetCylinderCtrl(id);
            return cylinderCtrl.MoveToWork(isBlock, timeout);
        }

        public static MessageSendResult CylinderToNone(int id, bool isBlock = false, int timeout = -1)
        {
            CylinderCtrl cylinderCtrl = PlcDriver?.GetCylinderCtrl(id);
            return cylinderCtrl.MoveToNone(isBlock, timeout);
        }

        public static MessageSendResult CylinderReset(int id, bool isBlock = false, int timeout = -1)
        {
            CylinderCtrl cylinderCtrl = PlcDriver?.GetCylinderCtrl(id);
            return cylinderCtrl.Reset(isBlock, timeout);
        }

        public static bool GetIO(int id)
        {
            try
            {
                DoCtrl io = PlcDriver.GetDoCtrl(id);
                bool value = false;
                io?.Read(out value);
                return value;
            }
            catch (Exception ex)
            {
                Flow.Log(ex.Message + Environment.NewLine + ex.StackTrace);
                return false;
            }
        }

        public static void SetIO(int id, bool value)
        {
            try
            {
                PlcDriver.WriteObject(RunModeMgr.GetIOVarName(id), value);
            }
            catch (Exception ex)
            {
                Flow.Log(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        public static bool ReadIO(int id)
        {
            try
            {
                return (bool)PlcDriver.ReadObject(RunModeMgr.GetIOVarName(id), typeof(bool));
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}