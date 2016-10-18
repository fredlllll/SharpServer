using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpServer.Loggers
{
    public class BlockingLogger : Logger
    {
        protected SemaphoreSlim freeSem;

        public BlockingLogger(MainLogger logger,int maxInQueue = 10000) : base(logger)
        {
            freeSem = new SemaphoreSlim(maxInQueue);
        }

        public override void Log(params string[] messageparts)
        {
            freeSem.Wait();
            //string[] sa = new string[] { "ThreadID: "+Thread.CurrentThread.ManagedThreadId }.Concat(messageparts).ToArray();
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
                        freeSem.Release();
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
