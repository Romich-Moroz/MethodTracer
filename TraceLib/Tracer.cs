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

        public TracingThread[] GetTraceResult()
        {
            
            TracingThread[] tmp = threadDict.Values.ToArray();
            foreach (TracingThread t in tmp)
            {
                t.CalculateThreadElapsedTime();
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
            if (!threadDict.ContainsKey(Thread.CurrentThread.ManagedThreadId))
            {
                TracingThread thread = new TracingThread();
                threadDict.TryAdd(thread.ThreadId, thread);
            }        
            MethodBase tracedMethod = new StackTrace().GetFrame(1).GetMethod();
            threadDict[Thread.CurrentThread.ManagedThreadId].BeginMethodTrace(new TraceResult(tracedMethod.Name, tracedMethod.DeclaringType.Name));                      
        }

        public void StopTrace()
        {
            threadDict[Thread.CurrentThread.ManagedThreadId].StopMethodTrace();

        }
    }
}
