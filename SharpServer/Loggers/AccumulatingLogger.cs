using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpServer.Loggers
{
    public class AccumulatingLogger : Logger
    {
        public AccumulatingLogger(MainLogger logger) : base(logger)
        {
        }

        public override void Log(params string[] messageparts)
        {
            queue.Enqueue(messageparts);
        }

        protected override void Run()
        {
            string prefix = "[" + domain.FriendlyName + "]:";
            string[] sa;
            while(true)
            {
                try
                {
                    if(queue.Count == 0)
                    {
                        Thread.Sleep(500);
                    }
                    if(queue.TryDequeue(out sa))
                    {
                        logger.Log(prefix + string.Join("", sa));
                    }
                }
                catch(ThreadInterruptedException)
                {
                    break;
                }
            }
        }
    }
}
