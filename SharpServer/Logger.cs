using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpServer
{
    public abstract class Logger : MarshalByRefObject, IDisposable
    {
        protected MainLogger logger;
        protected AppDomain domain;
        Thread loggingThread;
        protected ConcurrentQueue<string[]> queue = new ConcurrentQueue<string[]>();

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

        protected abstract void Run();

        public abstract void Log(params string[] messageparts);

        public void Dispose()
        {
            loggingThread.Interrupt();
        }
    }
}
