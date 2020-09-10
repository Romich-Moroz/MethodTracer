using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.Linq;

namespace TraceLib
{
    [System.Runtime.Serialization.DataContract]
    [Serializable]
    /// <summary>
    /// Represents the final result of method tracing procedure
    /// </summary>
    public class TraceResult
    {

        [System.Runtime.Serialization.DataMember(Name = "name")]
        public string MethodName { get; private set; }

        [System.Runtime.Serialization.DataMember(Name = "class")]
        public string ParentClassName { get; private set; }

        [JsonIgnore]
        [XmlIgnore]
        public string ParentMethodName { get; private set; }

        [JsonIgnore]
        [XmlIgnore]
        public int CallDepth { get; private set; }

        [System.Runtime.Serialization.DataMember(Name = "time")]
        public long ExecutionTime { get; private set; }

        private Stopwatch timer = new Stopwatch();

        [System.Runtime.Serialization.DataMember(Name = "methods")]
        /// <summary>
        /// List of methods that were called within traced method
        /// </summary>
        public ConcurrentBag<TraceResult> CallStack { get; private set; } = new ConcurrentBag<TraceResult>();

        public TraceResult(string name, string parentName,string parentMethod, int callDepth)
        {
            MethodName = name;
            ParentClassName = parentName;
            ParentMethodName = parentMethod;
            CallDepth = callDepth;
            timer.Start();

        }
        public void ExecutionFinished()
        {
            timer.Stop();
            ExecutionTime = timer.ElapsedMilliseconds;
        }
    }

}
