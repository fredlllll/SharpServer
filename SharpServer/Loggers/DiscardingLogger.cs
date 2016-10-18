using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpServer.Loggers
{
    public class DiscardingLogger : BlockingLogger
    {
        int waitTimeout;
        public DiscardingLogger(MainLogger logger, int maxInQueue = 10000, int waitTimeout = 0) : base(logger, maxInQueue)
        {
            this.waitTimeout = waitTimeout;
        }

        public override void Log(params string[] messageparts)
        {
            if(freeSem.Wait(waitTimeout))
            {
                queue.Enqueue(messageparts);
            }
        }
    }
}
