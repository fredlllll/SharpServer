using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public class MainLogger : MarshalByRefObject
    {
        public void Log(string message)
        {
            System.Diagnostics.Trace.WriteLine(message);
            Console.WriteLine(message);
        }
    }
}
