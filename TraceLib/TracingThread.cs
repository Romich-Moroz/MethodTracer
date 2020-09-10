using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace TraceLib
{
    [System.Runtime.Serialization.DataContract(Name = "thread")]
    [Serializable]
    public class TracingThread
    {
        
        private Stopwatch timer = new Stopwatch();
        private Thread thread;


        [System.Runtime.Serialization.DataMember(Name = "id")]
        public int ThreadId { get; private set; }

        [System.Runtime.Serialization.DataMember(Name = "time")]
        public long ThreadTimeElapsed { get; private set; }

        [System.Runtime.Serialization.DataMember(Name = "Methods")]
        public TraceResult Result { get; private set; }

        [JsonIgnore]
        [XmlIgnore]
        public bool IsAlive { get; private set; }
      

        public TracingThread(int id, Thread thread)
        {
            ThreadId = id;
            this.thread = thread;
        }

        public void SetTraceResult(TraceResult result)
        {
            Result = result;
        }

        public void StartThread()
        {                      
            IsAlive = true;
            timer.Start();
            thread.Start();
        }

        public void StopThread()
        {
            Result.ExecutionFinished();
            timer.Stop();
            thread.Join();            
            IsAlive = false;
            ThreadTimeElapsed = timer.ElapsedMilliseconds;
        }

    }
}
