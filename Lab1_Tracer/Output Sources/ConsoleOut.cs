using System;
using System.Text;

namespace Lab1_Tracer
{
    public class ConsoleOut : IOutput
    {
        public void PrintData(byte[] data)
        {
            Console.Write(Encoding.UTF8.GetString(data).ToCharArray());
        }
    }
}
