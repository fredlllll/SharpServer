using System;
using System.Collections.Generic;
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
        public Dictionary<string, Page> pages = new Dictionary<string, Page>();

        public SharpServer(SiteConfig config, Logger logger)
        {
            this.config = config;
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

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        void Process(object argument)
        {
            //also gotta make one page instance per thread, or find another way so the threads dont get in each others way
            stopwatch.Restart();
            HttpListenerContext context = (HttpListenerContext)argument;

            string page = context.Request.Url.AbsolutePath;
            if(page.EndsWith("/"))
            {
                page = page + "index.ssp";
            }
            Page p = null;
            if(pages.TryGetValue(page, out p))
            {
                logger.Log("200: " + page);
                p.Reset();
                Page.Request = context.Request;
                Page.Response = context.Response;
                Page.User = context.User;
                Page.Buffer = new StreamBuffer(context.Response.OutputStream,config.Encoding);
                p.BeforeEmit();
                p.Emit();
                p.AfterEmit();
                Page.Buffer.Flush();
            }
            else
            {
                logger.Log("404: " + page);
                context.Response.StatusCode = 404;
            }
            context.Response.OutputStream.Close();
            stopwatch.Stop();
            logger.Log("Request Time: " + stopwatch.Elapsed.TotalMilliseconds);
        }
    }
}
