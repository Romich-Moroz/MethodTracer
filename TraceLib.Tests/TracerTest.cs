using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TraceLib.Tests
{
    [TestClass]
    public class TracerTest
    {

        static readonly int sleepTime = 20;

        static public Tracer tracer;

        [TestInitialize]
        public void TestInit()
        {
            tracer = new Tracer();            
        }

        static public class SimpleTest {
            static public void SimpleCall()
            {
                tracer.StartTrace();

                Thread.Sleep(sleepTime);

                tracer.StopTrace();
            }
        }

        static public class SequencedTest
        {
            static public void SimpleSequenceCall()
            {
                SimpleTest.SimpleCall();
                SimpleTest.SimpleCall();
                SimpleTest.SimpleCall();
            }

            static public void NestedSequenceCall()
            {
                tracer.StartTrace();

                SimpleSequenceCall();

                tracer.StopTrace();
            }
            
        }

        static public class NestedTest
        {
            static public void NestedCall()
            {
                tracer.StartTrace();

                SimpleTest.SimpleCall();

                tracer.StopTrace();
            }
        }
        
        static public class RecursionTest
        {
            static public void RecursiveCall(int count)
            {
                tracer.StartTrace();

                if (count != 0)
                {
                    RecursiveCall(count-1);                   
                }
                Thread.Sleep(sleepTime);

                tracer.StopTrace();
            }

            static public void CascadeCall(int count)
            {
                tracer.StartTrace();

                if (count != 0)
                {
                    CascadeCall(count - 1);
                    CascadeCall(count - 1);                   
                }
                Thread.Sleep(sleepTime);

                tracer.StopTrace();
            }
            
            static private void MutualCall2(int count)
            {
                tracer.StartTrace();

                if (count != 0)
                {
                    MutualCall(count - 1);                    
                }
                Thread.Sleep(sleepTime);

                tracer.StopTrace();
            }

            static public void MutualCall(int count)
            {
                tracer.StartTrace();

                if (count != 0)
                {
                    MutualCall2(count);                   
                }
                Thread.Sleep(sleepTime);

                tracer.StopTrace();
            }
        }

        static public class OverloadTest
        {

            static public class OverloadClass
            {
                static public void Overload(int arg)
                {
                    tracer.StartTrace();
                    Thread.Sleep(sleepTime);
                    tracer.StopTrace();

                }

                static public void Overload(char arg)
                {
                    tracer.StartTrace();

                    Thread.Sleep(sleepTime);
                    tracer.StopTrace();

                }
            }

            static private void Overload(int arg)
            {
                tracer.StartTrace();
                OverloadClass.Overload('b');
                Thread.Sleep(sleepTime);
                tracer.StopTrace();

            }
            static private void Overload(char arg)
            {
                tracer.StartTrace();
                OverloadClass.Overload(2);
                Thread.Sleep(sleepTime);
                tracer.StopTrace();

            }
            

            static public void OverloadCall()
            {
                tracer.StartTrace();
                Overload(1);
                Overload('a');

                Thread.Sleep(sleepTime);
                tracer.StopTrace();
            }

        }
        

        public void Equals(TraceResult expected, TraceResult actual)
        {
            Assert.AreEqual(expected.MethodName, actual.MethodName);
            Assert.AreEqual(expected.ParentClassName, actual.ParentClassName);
            Assert.AreEqual(expected.Methods.Count, actual.Methods.Count);
            Assert.IsNotNull(actual.ExecutionTime);
        }

        [TestMethod]
        public void MeasureSimpleCall_None_Equal()
        {
            SimpleTest.SimpleCall();
            var actual = tracer.GetTraceResult()[0].MethodList[0];
            var expected = new TraceResult("SimpleCall", "SimpleTest");
            Equals(actual, expected);          
        }

        [TestMethod]
        public void MeasureNestedCall_None_Equal()
        {
            NestedTest.NestedCall();
            var actual = tracer.GetTraceResult()[0].MethodList[0].Methods[0];
            var expected = new TraceResult("SimpleCall", "SimpleTest");
            Equals(actual, expected);
        }

        [TestMethod]
        public void RecursiveCall_2_Equal()
        {
            RecursionTest.RecursiveCall(2);
            var actual = tracer.GetTraceResult()[0].MethodList[0].Methods[0].Methods[0];
            var expected = new TraceResult("RecursiveCall", "RecursionTest");
            Equals(actual, expected);
        }

        [TestMethod]
        public void CascadeCall_1_Equal()
        {
            RecursionTest.CascadeCall(1);
            var actual = tracer.GetTraceResult()[0].MethodList[0].Methods[1];
            var expected = new TraceResult("CascadeCall", "RecursionTest");
            Equals(actual, expected);
        }

        [TestMethod]
        public void MutualRecursionCall_1_Equal()
        {
            RecursionTest.MutualCall(1);
            var actual1 = tracer.GetTraceResult()[0].MethodList[0].Methods[0];
            var actual2 = tracer.GetTraceResult()[0].MethodList[0].Methods[0].Methods[0];
            var expected1 = new TraceResult("MutualCall2", "RecursionTest");
            expected1.Methods.Add(new TraceResult("MutualCall", "RecursionTest"));
            var expected2 = new TraceResult("MutualCall", "RecursionTest");
            Equals(actual1, expected1);
            Equals(actual2, expected2);
        }

        [TestMethod]
        public void SimpleSequencedCall_None_Equal()
        {
            SequencedTest.SimpleSequenceCall();
            var actual = tracer.GetTraceResult()[0].MethodList[0].Methods;
            var expected = new TraceResult("SimpleCall", "SequencedTest");
            expected.Methods.Add(new TraceResult("SimpleCall","SimpleTest"));
            expected.Methods.Add(new TraceResult("SimpleCall", "SimpleTest"));
            expected.Methods.Add(new TraceResult("SimpleCall", "SimpleTest"));
            Equals(actual, expected);
        }

        [TestMethod]
        public void NestedSequencedCall_None_Equal()
        {
            SequencedTest.NestedSequenceCall();
            var actual = tracer.GetTraceResult()[0].MethodList[0];
            var expected = new TraceResult("NestedSequenceCall", "SequencedTest");
            expected.Methods.Add(new TraceResult("SimpleCall", "SimpleTest"));
            expected.Methods.Add(new TraceResult("SimpleCall", "SimpleTest"));
            expected.Methods.Add(new TraceResult("SimpleCall", "SimpleTest"));
            Equals(actual, expected);
        }

        [TestMethod]
        public void OverloadCall_None_Equal()
        {
            OverloadTest.OverloadCall();
            var actualMain = tracer.GetTraceResult()[0].MethodList[0];
            var expectedMain = new TraceResult("OverloadCall", "OverloadTest");
            var actual1 = tracer.GetTraceResult()[0].MethodList[0].Methods[0];
            var expected1 = new TraceResult("Overload", "OverloadTest");
            var actual2 = tracer.GetTraceResult()[0].MethodList[0].Methods[1];
            var expected2 = new TraceResult("Overload", "OverloadTest");
            var actual3 = tracer.GetTraceResult()[0].MethodList[0].Methods[0].Methods[0];
            var expected3 = new TraceResult("Overload", "OverloadClass");
            var actual4 = tracer.GetTraceResult()[0].MethodList[0].Methods[1].Methods[0];
            var expected4 = new TraceResult("Overload", "OverloadClass");

            expectedMain.Methods.Add(expected1);
            expectedMain.Methods.Add(expected2);
            expected1.Methods.Add(expected3);
            expected2.Methods.Add(expected4);


            Equals(actualMain, expectedMain);
            Equals(actual1, expected1);
            Equals(actual2, expected2);
            Equals(actual3, expected3);
            Equals(actual4, expected4);

        }

    }
}
