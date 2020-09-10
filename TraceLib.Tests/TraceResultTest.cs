using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TraceLib.Tests
{
    [TestClass]
    public class TraceResultTest
    {
        [TestMethod]
        public void TimeMeasure_FuncParams_NotZero()
        {
            
            TraceResult t = new TraceResult("funcname", "parentclassname", "parentmethodname", 0);
            Thread.Sleep(5);
            t.ExecutionFinished();
            long timeElapsed = t.ExecutionTime;

            Assert.AreNotEqual(0, timeElapsed);
        }

    }
}
