using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using VisionUtility;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace VisionModules
{
    [Serializable]
    public class Flow : FlowThread, ICloneable
    {
        #region"构造函数和析构函数"
        public Flow()
        {
            LastFlowID++;
            FlowID = LastFlowID;
            FlowName = "Flow" + FlowID.ToString();
        }

        public Flow(int ID)
        {
            FlowID = ID;
            FlowName = "Flow" + FlowID.ToString();
        }
        #endregion

        #region"字段"
        private int _lastItemID = 1;
        [NonSerialized]
        private int _curItemID = -1;
        [NonSerialized]
        private string _result;
        private List<Item> _itemList = new List<Item>();
        private List<F_DATA_CELL> _variableList = new List<F_DATA_CELL>();


        #endregion

        #region"属性"
        /// <summary>
        /// 最新流程ID
        /// </summary>
        public static int LastFlowID { get; set; }
        /// <summary>
        /// 流程ID，唯一
        /// </summary>
        public int FlowID { get; set; }
        /// <summary>
        /// 最新项ID
        /// </summary>
        public int LastItemID
        {
            get { return _lastItemID; }
            set { _lastItemID = value; }
        }
        /// <summary>
        /// 当前执行项ID
        /// </summary>
        public int CurItemID
        {
            get { return _curItemID; }
            set { _curItemID = value; }
        }
        /// <summary>
        /// 流程是否启动加载
        /// </summary>
        public bool IsStart { get; set; }
        /// <summary>
        /// 流程名称，唯一
        /// </summary>
        public string FlowName { get; set; }
        /// <summary>
        /// 当前检测结果
        /// </summary>
        public string Result
        {
            get { return _result; }
            set { _result = value; }
        }

        /// <summary>
        /// 项列表
        /// </summary>
        public List<Item> ItemList
        {
            get { return _itemList; }
            set { _itemList = value; }
        }

        /// <summary>
        /// 流程变量列表
        /// </summary>
        public List<F_DATA_CELL> VariableList
        {
            get { return _variableList; }
            set { _variableList = value; }
        }

        #endregion

        #region"事件"
        //

        #endregion

        #region"方法"
        /// <summary>
        /// 获取图像
        /// </summary>
        /// <param name="imgName">图像名称</param>
        /// <returns></returns>
        public HImage GetImage(string imgName)
        {
            return new HImage(imgName);
        }

        /// <summary>
        /// 获取变量值
        /// </summary>
        /// <param name="itemID">项ID</param>
        /// <param name="varName">变量名称</param>
        public F_DATA_CELL GetVariableValue(int itemID, string varName)
        {
            return new F_DATA_CELL();
        }

        /// <summary>
        /// 更新（修改）变量值
        /// </summary>
        /// <param name="itemID">变量ID</param>
        /// <param name="varName">变量名称</param>
        /// <param name="varValue">变量值</param>
        public void UpdateVariableValue(int itemID, string varName, object varValue)
        {
        }

        public override void StartThread()
        {
            base.StartThread();

            if (Thread == null)
            {
                ThreadState = true;
                Thread = new System.Threading.Thread(ImageProcess);
                Thread.Name = FlowName;
                Thread.IsBackground = true;
                Thread.Start();
            }

            VisionModulesHelper.UpdateFlow?.Invoke(this);
        }

        private void ImageProcess()
        {
            try
            {
                while (ThreadState)
                {
                    //主动回收下系统未使用的资源
                    GC.Collect();
                    foreach (Item item in ItemList)
                    {
                        //屏蔽不执行
                        if (!item.IsShield)
                            item.Execute();
                       
                        //线程终止
                        if (ThreadState == false)
                            return;
                    }

                    if (VisionModulesManager.SystemStatus.m_RunMode == RunMode.执行一次)
                    {
                        VisionModulesManager.SystemStatus.m_RunMode = RunMode.None;
                        ThreadState = false;
                        return;
                    }

                    System.Threading.Thread.Sleep(5);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return;
            }
        }

        public object Clone()
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Position = 0;
            Flow flow = (Flow)formatter.Deserialize(stream);
            return flow;
        }

        [OnDeserialized()]
        internal void OnDeSerializedMethod(StreamingContext context)
        {
            CurItemID = -1;
            Result = "";
        }

        #endregion
















    }
}
