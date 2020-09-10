using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab1_Tracer
{
    public class FileOut : IOutput
    {
        private string filename;
        public void PrintData(byte[] data)
        {
            File.WriteAllBytes(filename, data);
        }

        public FileOut(string filename)
        {
            this.filename = filename;
        }
    }
}
