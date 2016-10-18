using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpServer
{
    public class SharpServer : MarshalByRefObject
    {
        HttpListener listener;
        Thread listenerThread;
        SiteConfig config;
        Logger logger;

        Assembly siteBinary;
        Site site;

        public SharpServer(FileInfo siteConfigFile, Logger logger)
        {
            this.config = new SiteConfig(siteConfigFile);
            this.logger = logger;
            foreach(var s in config.Dependencies)
            {
                Assembly.Load(s);
            }
            siteBinary = Assembly.LoadFile(config.SiteBinary.FullName);
            Type[] sites = siteBinary.GetTypes().Where((t) => { return t.BaseType == typeof(Site); }).ToArray();
            site = (Site)Activator.CreateInstance(sites[0]);
            listener = new HttpListener();
            listener.Prefixes.Add(string.Format("http://*:{0}/", config.Port));

            config.LoadDirectories();
        }

        public void Start()
        {
            site.Start(this);
            listener.Start();
            listenerThread = new Thread(Listen);
            listenerThread.Start();
        }

        public void Stop()
        {
            site.Stop();
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
                    logger.Log(ex.ToString());
                    break;
                }
            }
        }
        InstanceLender<Stopwatch> stopwatchLender = new InstanceLender<Stopwatch>();
        void Process(object argument)
        {
            var stopwatch = stopwatchLender.Lend();
            stopwatch.Restart();
            HttpListenerContext context = (HttpListenerContext)argument;
            HttpListenerRequest request = context.Request;
            string absolutePath = request.Url.AbsolutePath;

            URIResolver resolver = null;
            Page p = null;
            foreach(var kv in config.Directories)
            {
                if(absolutePath.StartsWith(kv.Key, StringComparison.InvariantCultureIgnoreCase))
                {
                    p = kv.Value.Resolve(request);
                    if(p != null)
                    {
                        resolver = kv.Value;
                        break;
                    }
                }
            }

            if(p != null)
            {
                logger.Log("200: ", absolutePath);
                try
                {
                    p.Reset();
                    p.Request = request;
                    p.Response = context.Response;
                    p.User = context.User;
                    p.Buffer = new StreamBuffer(context.Response.OutputStream, config.Encoding);
                    p.Emit();
                    p.Buffer.Flush();
                }
                catch(Exception ex)
                {
                    logger.Log("Exception: ", ex.ToString());
                }
            }
            else
            {
                logger.Log("404: ", absolutePath);
                context.Response.StatusCode = 404;
            }
            context.Response.OutputStream.Close();
            if(p != null)
            {
                resolver.ReturnPageInstance(p);
            }
            stopwatch.Stop();
            logger.Log("Request Time: " + stopwatch.Elapsed.TotalMilliseconds);
            stopwatchLender.Return(stopwatch);
        }
    }
}
