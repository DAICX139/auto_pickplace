using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VisionModules
{
    /// <summary>
    /// 流程使用的线程
    /// </summary>
    [Serializable]
   public  class FlowThread : IDisposable
    {
        public FlowThread()
        {
            _threadID++;
        }

        private static int _threadID = 0;
        [NonSerialized]
        private Thread _thread;

        protected Thread Thread
        {
            get { return _thread; }
            set { _thread = value; }
        }
        public int ThreadID
        {
            get { return _threadID; }
        }

        public bool ThreadState { get; set; }

        public virtual void StartThread()
        {
            this.ThreadState = true;
        }

        public virtual void StopThread()
        {
            this.ThreadState = false;
        }

        /// <summary>
        /// 释放线程资源
        /// </summary>
        public virtual void Dispose()
        {
            if (_thread != null && _thread.ThreadState != System.Threading.ThreadState.Aborted)
            {
                _thread.Abort();
                _thread = null;
                this.ThreadState = false;
            }
        }

        [OnDeserializing()]
        internal void OnDeSerializingMethod(StreamingContext context)
        {
        }

    }
}
