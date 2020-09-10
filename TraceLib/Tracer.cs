using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace TraceLib
{
    /// <summary>
    /// General purpose tracer for measuring execution time
    /// </summary>
    public class Tracer : ITracer
    {
        public enum SerializationType { JSON, XML }

        private ConcurrentDictionary<int,TracingThread> threadDict = new ConcurrentDictionary<int,TracingThread>();

        private StackTrace mainCallStack = null;

        private readonly object mainCallStackLock = new object();

        public TracingThread[] GetTraceResult()
        {
            TracingThread[] tmp = threadDict.Values.ToArray();
            for (int i = tmp.Length - 1; i > 0; i--)
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (tmp[j].Result.MethodName == tmp[i].Result.ParentMethodName && tmp[j].Result.CallDepth == (tmp[i].Result.CallDepth - 1))
                    {
                        tmp[j].Result.CallStack.Add(tmp[i].Result);
                        break;
                    }
                }
            }
            return tmp;          
        }

        public byte[] GetSerializedResult(TracingThread[] tree, SerializationType type)
        {
            ISerializer serializer;
            if (type == SerializationType.JSON)
            {
                serializer = new JSONSerializer();
            }
            else
            {
                serializer = new XMLSerializer();
            }
            return serializer.Serialize(tree);
        }


        public void StartTrace()
        {
            int localThreadId = threadDict.Count;
            StackTrace st = new StackTrace();
            lock(mainCallStackLock)
            {
                if (!st.Equals(mainCallStack))
                {                    
                    mainCallStack = st;
                }
            }
            TracingThread thread = new TracingThread(localThreadId, new Thread(()=> {
                MethodBase tracedMethod = st.GetFrame(1).GetMethod();
                MethodBase parentMethod = null;
                if (st.FrameCount > 2)
                {
                    parentMethod = st.GetFrame(2).GetMethod();
                }  
                threadDict[localThreadId].SetTraceResult(new TraceResult(tracedMethod.Name, tracedMethod.DeclaringType.Name, parentMethod?.Name, st.FrameCount-2));
            }));
            threadDict.TryAdd(localThreadId, thread);
            thread.StartThread();
        }

        public void StopTrace()
        {
            int localThreadId = threadDict.Count - 1;
            while (localThreadId != 0 && !threadDict[localThreadId].IsAlive)
            {
                localThreadId--;
            }
            TracingThread tmp;
            threadDict.TryGetValue(localThreadId, out tmp);
            while (tmp.Result == null)
            {  
                Thread.Sleep(5);
            }
            tmp.Result.ExecutionFinished();
            tmp.StopThread();
        }
    }
}
