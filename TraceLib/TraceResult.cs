using System;
using System.Diagnostics;
using System.Collections.Generic;

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

        [System.Runtime.Serialization.DataMember(Name = "time")]
        public long ExecutionTime { get; private set; }

        private Stopwatch timer = new Stopwatch();

        [System.Runtime.Serialization.DataMember(Name = "methods")]
        /// <summary>
        /// List of methods that were called within traced method
        /// </summary>
        public List<TraceResult> Methods { get; private set; } = new List<TraceResult>();

        public TraceResult(string name, string parentName)
        {
            MethodName = name;
            ParentClassName = parentName;
        }


        public void ExecutionStart()
        {
            timer.Start();
        }
        public void ExecutionFinished()
        {
            timer.Stop();
            ExecutionTime = timer.ElapsedMilliseconds;
        }
    }

}
