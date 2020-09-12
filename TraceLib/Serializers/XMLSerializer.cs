using System.Xml;
using System.Runtime.Serialization;
using System.IO;

namespace TraceLib
{
    
    class XMLSerializer : ISerializer
    {
        public byte[] Serialize(object o)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(TracingThread[]));
            var settings = new XmlWriterSettings { Indent = true };
            MemoryStream ms = new MemoryStream();
            using (var w = XmlWriter.Create(ms, settings))
            {
                serializer.WriteObject(w, o);
            }
            return ms.GetBuffer();
        }
    }
}
