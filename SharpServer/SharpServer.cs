using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpServer
{
    public class SharpServer
    {
        HttpListener listener = new HttpListener();
        Thread listenerThread;

        void Start()
        {
            listener.Start();
            listenerThread = new Thread(Listen);
            listenerThread.Start();
        }

        void Stop()
        {
            listener.Stop();
            listenerThread.Interrupt();
        }

        void Listen()
        {
            while(listener.IsListening)
            {
                try
                {
                    HttpListenerContext context = listener.GetContext();
                    ThreadPool.QueueUserWorkItem(Process, context);
                }
                catch(ThreadInterruptedException)
                {
                    break;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    break;
                }
            }
        }

        void Process(object argument) {
            HttpListenerContext context = (HttpListenerContext)argument;
            //parse request and find file for it
            //or should we try dynamic loading and unloading of assemblies again?
        }
    }
}
