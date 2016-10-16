using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpServer
{
    public class Logger : MarshalByRefObject, IDisposable
    {
        MainLogger logger;
        AppDomain domain;
        Thread loggingThread;
        ConcurrentQueue<string[]> queue = new ConcurrentQueue<string[]>();

        public Logger(MainLogger logger)
        {
            this.logger = logger;
            this.domain = AppDomain.CurrentDomain;
            loggingThread = new Thread(Run);
            loggingThread.IsBackground = true;
            loggingThread.Start();
        }

        ~Logger()
        {
            Dispose();
        }

        void Run()
        {
            string prefix = "["+domain.FriendlyName+"]:";
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
                        logger.Log(prefix + string.Join("",sa));
                    }
                }
                catch(ThreadInterruptedException)
                {
                    break;
                }
            }
        }

        public void Log(params string[] messageparts)
        {
            queue.Enqueue(messageparts);
        }

        public void Dispose()
        {
            loggingThread.Interrupt();
        }
    }
}
