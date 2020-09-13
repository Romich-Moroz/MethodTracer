using System.Text;
using Newtonsoft.Json;

namespace TraceLib
{
    class JSONSerializer : ISerializer
    {
        public byte[] Serialize(object o)
        {
            return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(o,Formatting.Indented).ToCharArray());           
        }
    }
}
