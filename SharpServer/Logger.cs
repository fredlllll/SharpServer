using System;
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
        System.Collections.Concurrent.ConcurrentQueue<string> queue = new System.Collections.Concurrent.ConcurrentQueue<string>();

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
            string s;
            while(true)
            {
                try
                {
                    if(queue.Count == 0)
                    {
                        Thread.Sleep(500);
                    }
                    if(queue.TryDequeue(out s))
                    {
                        logger.Log(prefix + s);
                    }
                }
                catch(ThreadInterruptedException)
                {
                    break;
                }
            }
        }

        public void Log(string message)
        {
            queue.Enqueue(message);
        }

        public void Dispose()
        {
            loggingThread.Interrupt();
        }
    }
}
