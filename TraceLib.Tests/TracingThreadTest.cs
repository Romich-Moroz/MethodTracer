using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TraceLib.Tests
{
    [TestClass]
    public class TracingThreadTest
    {
        TracingThread thread;
        TraceResult traceResult;

        [TestInitialize]
        public void TestInit()
        {
            thread = new TracingThread(0, new Thread(() => { }));
            traceResult = new TraceResult("func", "parent", "parentfunc", 0);
            thread.SetTraceResult(traceResult);
        }

        [TestMethod]
        public void TimeMeasure_IdandThread_NotZero()
        {
            thread.StartThread();
            Thread.Sleep(10);
            thread.StopThread();
            Assert.AreNotEqual(0, thread.ThreadTimeElapsed, "Elapsed time is zero");
        }

        [TestMethod]
        public void ThreadWorkResult_TraceResult_MeasureCorrect()
        {           
            
            thread.StartThread();
            Thread.Sleep(10);
            thread.StopThread();

            Assert.IsNotNull(thread.Result, "Thread does not contain result (result == null)");
        }
    }
}
