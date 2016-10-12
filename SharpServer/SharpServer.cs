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
    public class SharpServer : IDisposable
    {
        AppDomain serverDomain;
        AppDomainProxy proxy;
        HttpListener listener;
        Thread listenerThread;
        //SiteConfig config;

        Assembly siteBinary;
        Site site;
        public Dictionary<string, Page> pages = new Dictionary<string, Page>();

        public SharpServer(SiteConfig config)
        {
            //this.config = config;
            serverDomain = AppDomain.CreateDomain("SharpServerPage_" + config.SiteName);//, null, null, null, null);//for now lets not fiddle with that security stuff
            proxy = (AppDomainProxy)serverDomain.CreateInstanceAndUnwrap(typeof(AppDomainProxy).Assembly.FullName, typeof(AppDomainProxy).FullName);
            serverDomain.AssemblyResolve += proxy.ServerDomain_AssemblyResolve;

            proxy.Server = this;
            proxy.Load(config);
            Program.prog.Log("huiii");
        }



        [Serializable]
        class AppDomainProxy
        {
            public AppDomainProxy()
            {

            }

            public SharpServer Server { get; set; }

            public void Load(SiteConfig config)
            {
                foreach(var s in config.Dependencies)
                {
                    Assembly.Load(s);
                }
                Server.siteBinary = Assembly.LoadFile(config.SiteBinary.FullName);
                Type[] sites = Server.siteBinary.GetTypes().Where((t) => { return t.BaseType == typeof(Site); }).ToArray();
                Server.site = (Site)Activator.CreateInstance(sites[0]);
                Server.listener = new HttpListener();
                Server.listener.Prefixes.Add(string.Format("http://*:{0}/", 8989));

                Program.prog.Log("huiii");
            }

            public Assembly ServerDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                Program.prog.Log(args.Name);
                return null;
            }

            public void Start()
            {
                Server.site.Setup(Server);
                Server.listener.Start();
                Server.listenerThread = new Thread(Listen);
                Server.listenerThread.Start();
            }

            public void Stop()
            {
                Server.site.Dispose();
                Server.listener.Stop();
                Server.listenerThread.Interrupt();
            }


            void Listen()
            {
                while(Server.listener.IsListening)
                {
                    try
                    {
                        HttpListenerContext context = Server.listener.GetContext();
                        ThreadPool.QueueUserWorkItem(Process, context);
                    }
                    catch(ThreadInterruptedException)
                    {
                        break;
                    }
                    catch(Exception ex)
                    {
                        Program.prog.Log(ex.ToString());
                        break;
                    }
                }
            }

            void Process(object argument)
            {
                HttpListenerContext context = (HttpListenerContext)argument;

                string page = context.Request.Url.AbsolutePath;
                if(page.EndsWith("/"))
                {
                    page = page + "index.ssp";
                }
                Page p = null;
                if(Server.pages.TryGetValue(page, out p))
                {
                    Program.prog.Log("200: " + page);
                    StreamBuffer b = new StreamBuffer(context.Response.OutputStream);
                    p.BeforeEmit(context.Response);
                    p.Emit(b);
                    b.Flush();
                    p.AfterEmit();
                    context.Response.OutputStream.Close();
                }
                else
                {
                    Program.prog.Log("404: " + page);
                    context.Response.StatusCode = 404;
                    context.Response.OutputStream.Close();
                }
            }
        }

        public void Dispose()
        {
            Stop();
            AppDomain.Unload(serverDomain);
        }

        public void Start()
        {
            proxy.Start();
        }

        public void Stop()
        {
            proxy.Stop();
        }
    }
}
