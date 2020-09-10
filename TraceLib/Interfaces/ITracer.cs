using System.Collections.Generic;

namespace TraceLib
{
    public interface ITracer
    {
        /// <summary>
        /// Called infront of the traced method
        /// </summary>
        void StartTrace();

        /// <summary>
        /// Called after the traced method
        /// </summary>
        void StopTrace();

        /// <summary>
        /// Get stats of traced method
        /// </summary>
        /// <returns></returns>
        TracingThread[] GetTraceResult();
    }
}
