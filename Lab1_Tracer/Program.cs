using System;
using System.Collections.Generic;
using TraceLib;

namespace Lab1_Tracer
{
    static public class StaticTracer
    {
        public static Tracer t = new Tracer();
    }
    class TestClass1
    {
        public void PrintFunc(string text, int count)
        {
            StaticTracer.t.StartTrace();
            for (int i = 0; i < count; i++)
                Console.WriteLine(text);
            StaticTracer.t.StopTrace();
        }

        public void NestedFunc(int callTimes)
        {
            StaticTracer.t.StartTrace();
            Console.WriteLine(string.Format("NestedFunc was called, {0} calls left", callTimes));
            if (callTimes != 0)
            {
                NestedFunc(callTimes - 1);
            }
                
            Console.WriteLine(string.Format("NestedFunc that had {0} calls left is returning", callTimes));
            StaticTracer.t.StopTrace();
        }

        public void CascadeFunc(int callTimes)
        {
            StaticTracer.t.StartTrace();
            Console.WriteLine(string.Format("CascadeFunc was called, {0} calls left", callTimes));
            if (callTimes != 0)
            {
                CascadeFunc(callTimes - 1);
                CascadeFunc(callTimes - 1);
            }            
            StaticTracer.t.StopTrace();
        }
    }

    class TestClass2
    {
        public void MultiClassFunc()
        {
            StaticTracer.t.StartTrace();
            TestClass1 t1 = new TestClass1();
            for (int i = 0; i < 3; i++)
            {
                t1.PrintFunc(string.Format("MultiClassFunc called print func with arg {0}",i), i);
            }
            t1.NestedFunc(2);
            t1.CascadeFunc(2);
            StaticTracer.t.StopTrace();
        }
    }
    class Program
    {
        
        static void Main(string[] args)
        {
            StaticTracer.t.StartTrace();
            TestClass2 t2 = new TestClass2();
            t2.MultiClassFunc();
            StaticTracer.t.StopTrace();
            TracingThread[] tmp = StaticTracer.t.GetTraceResult();
            Console.WriteLine("1 for JSON, 2 for XML");
            int k = 0;
            byte[] printData = null;
            while (k != '1' && k != '2')
            {
                k = Console.Read();
                if (k == '1')
                {
                    printData = StaticTracer.t.GetSerializedResult(tmp, Tracer.SerializationType.JSON);
                }
                else if (k == '2')
                {
                    printData = StaticTracer.t.GetSerializedResult(tmp, Tracer.SerializationType.XML);
                }
            } 
            Console.WriteLine("1 for Console, 2 for file");
            k = Console.Read();
            IOutput o = null;
            while (k != '1' && k != '2')
            {
                k = Console.Read();
                if (k == '1')
                {
                    o = new ConsoleOut();
                }
                else if (k == '2')
                {
                    o = new FileOut("tmp.txt");
                }
            }           
            o.PrintData(printData);
            Console.Read();
        }
    }
}
