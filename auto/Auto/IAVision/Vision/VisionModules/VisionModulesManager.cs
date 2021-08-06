using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using VisionUtility;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace VisionModules
{
    public class VisionModulesManager
    {
        /// <summary>项目配置文件路径</summary>
        public static string CfgFileName = Application.StartupPath + @"\VisionModules.cfg";
        /// <summary>流程变量</summary>
        public const int U000 = 0;
        /// <summary>全局变量 </summary>
        public const int USys = -1;
        /// <summary>当前项目 </summary>
        public static Project CurrProject = null;
        /// <summary>项目列表</summary>
        public static List<Project> ProjectList = new List<Project>();
        /// <summary>当前流程 </summary>
        public static Flow CurrFlow = null;
        /// <summary>流程列表</summary>
        public static List<Flow> FlowList = new List<Flow>();
        /// <summary>全局变量列表</summary>
        public static List<F_DATA_CELL> VariableList = new List<F_DATA_CELL>();
        /// <summary>采集设备列表</summary>
        public static List<VisionCameraBase> CameraList = new List<VisionCameraBase>();
        /// <summary>系统运行状态</summary>
        public static SystemStatus SystemStatus = new SystemStatus();

        /// <summary>
        ///  初始化视觉项目
        /// </summary>
        /// <param name="cfgFileName">初始化文件名</param>
        /// <returns></returns>
        public static bool InitialVisionProgram(string cfgFileName)
        {
    
            try
            {
                if (File.Exists(cfgFileName) == false)
                    throw new Exception("视觉模块：配置文件[" + cfgFileName + "]不存在!");

                CfgFileName = cfgFileName;
                ReadConfig(CfgFileName);

                //每次读取新的配置文件时重新检测设备连接状态
                IniDevStatus();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message+ Environment.NewLine + ex.StackTrace);
                return false;
            }
        }

       

        public static void DisposeVisionProgram()
        {
            //Sys_Stop();
            //HMeasureSYS.DisposeDev();
        }

        /// <summary>
        /// 读项目配置文件，每个项目配置文件不同
        /// </summary>
        /// <param name="fileName"></param>
        public static void ReadConfig(string fileName)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    BinaryFormatter formatter = new BinaryFormatter();

                    VisionModulesManager.VariableList = (List<F_DATA_CELL>)formatter.Deserialize(fs);
                    VisionModulesManager.CameraList = (List<VisionCameraBase>)formatter.Deserialize(fs);
                    VisionCameraBase.LastCameraID = (int)formatter.Deserialize(fs);
                    VisionModulesManager.FlowList = (List<Flow>)formatter.Deserialize(fs);
                    Flow.LastFlowID = (int)formatter.Deserialize(fs);
                    //fs.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// 保存项目配置文件，每个项目配置文件不同
        /// </summary>
        /// <param name="fileName"></param>
        public static void SaveConfig(string fileName)
        {
            string tempFileName = Application.StartupPath+@"\TempVisionModules.cfg";

            try
            {
                GC.Collect();
                using (FileStream fs = new FileStream(tempFileName, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    fs.Seek(0, SeekOrigin.Begin);
                    formatter.Serialize(fs, VisionModulesManager.VariableList);
                    formatter.Serialize(fs, VisionModulesManager.CameraList);
                    formatter.Serialize(fs, VisionCameraBase.LastCameraID);
                    formatter.Serialize(fs, VisionModulesManager.FlowList);
                    formatter.Serialize(fs, Flow.LastFlowID);
                    //fs.Close();
                }

                File.Copy(tempFileName, fileName, true);
                File.Delete(tempFileName);

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        /// <summary>
        /// 初始化设备连接状态
        /// </summary>
        public static void IniDevStatus()
        {
            try
            {
                foreach (IVisionCamera cam in VisionModulesManager.CameraList)
                {
                    cam.Connect();
                    if (!cam.IsConnected)
                        continue;

                    cam.SetExposureTime(cam.ExposureTime);
                    cam.SetGain(cam.Gain);
                    cam.SetReverseX(cam.ReverseX);
                    cam.SetReverseY(cam.ReverseY);
                    cam.SetTriggerMode(TriggerMode.软件触发);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }






    }
}
