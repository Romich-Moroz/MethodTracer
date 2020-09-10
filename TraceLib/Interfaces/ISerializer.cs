using System;
using System.Collections.Generic;
using System.Text;

namespace TraceLib
{
    interface ISerializer
    {
        byte[] Serialize(object o);
    }
}
