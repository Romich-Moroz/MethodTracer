using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace TraceLib
{
    [System.Runtime.Serialization.DataContract(Name = "thread")]
    [Serializable]
    public class TracingThread
    {
        
        private Stopwatch timer = new Stopwatch();

        [System.Runtime.Serialization.DataMember(Name = "id")]
        public int ThreadId { get; private set; }

        [System.Runtime.Serialization.DataMember(Name = "time")]
        public long ThreadTimeElapsed { get; private set; }

        [System.Runtime.Serialization.DataMember(Name = "Methods")]
        public List<TraceResult> MethodList { get; private set; } = new List<TraceResult>();

        public Stack<TraceResult> MethodStack { get; private set; } = new Stack<TraceResult>();

        public TracingThread()
        {
            ThreadId = Thread.CurrentThread.ManagedThreadId;
            
        }
        public void BeginMethodTrace(TraceResult result)
        {
            result.ExecutionStart();
            if (MethodStack.Count == 0)
            {
                MethodList.Add(result);               
            }
            else
            {
                MethodStack.Peek().Methods.Add(result);
            }
            MethodStack.Push(result);
        }

        public void StopMethodTrace()
        {
            TraceResult t = MethodStack.Pop();
            t.ExecutionFinished();   
        }

        public void CalculateThreadElapsedTime()
        {
            ThreadTimeElapsed = 0;
            foreach(TraceResult t in MethodList)
            {
                ThreadTimeElapsed += t.ExecutionTime;
            }
        }

    }
}
