using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpServer
{
    public class Logger : MarshalByRefObject
    {
        MainLogger logger;
        AppDomain domain;

        public Logger(MainLogger logger)
        {
            this.logger = logger;
            this.domain = AppDomain.CurrentDomain;
        }

        public void Log(string message)
        {
            string line = "[" + domain.FriendlyName + "]:" + message;
            logger.Log(line);
            //System.Diagnostics.Trace.WriteLine(line);
            //Console.WriteLine(line);
        }
    }
}
