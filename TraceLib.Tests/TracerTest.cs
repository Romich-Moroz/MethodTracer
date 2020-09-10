using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TraceLib.Tests
{
    [TestClass]
    public class TracerTest
    {

        private Tracer t;

        [TestInitialize]
        public void TestInit()
        {
            t = new Tracer();
            t.StartTrace();
            Thread.Sleep(50);
            t.StopTrace();
        }

        [TestMethod]
        public void TraceCheck_None_NotNull()
        {
            Assert.IsNotNull(t.GetTraceResult());
        }

        [TestMethod]
        public void Serialization_Json_NotNull()
        {
            Assert.IsNotNull(t.GetSerializedResult(t.GetTraceResult(), Tracer.SerializationType.JSON));
        }

        [TestMethod]
        public void Serialization_Xml_NotNull()
        {
            Assert.IsNotNull(t.GetSerializedResult(t.GetTraceResult(), Tracer.SerializationType.XML));
        }

    }
}
