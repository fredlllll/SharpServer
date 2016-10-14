using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    class Program
    {
        static List<SharpServer> servers;
        static MainLogger logger;
        static void Main(string[] args)
        {
            servers = new List<SharpServer>();
            logger = new MainLogger();

            List<SiteConfig> configs = new List<SiteConfig>();
            for(int i = 0; i < args.Length; i++)
            {
                configs.Add(new SiteConfig(new FileInfo(args[i])));
            }
            
            foreach(var sc in configs)
            {
                var serverDomain = AppDomain.CreateDomain("SharpServerPage_" + sc.SiteName);//, null, null, null, null);//for now lets not fiddle with that security stuff
                AppDomainProxy proxy = (AppDomainProxy)serverDomain.CreateInstanceAndUnwrap(typeof(AppDomainProxy).Assembly.FullName, typeof(AppDomainProxy).FullName);
                proxy.logger = new Logger(logger);
                serverDomain.AssemblyResolve += proxy.ServerDomain_AssemblyResolve;
                SharpServer ss = proxy.CreateInstance<SharpServer>(sc,proxy.logger);
                servers.Add(ss);
            }
            foreach(var s in servers)
            {
                s.Start();
            }
            if(servers.Count == 0)
            {
                Console.WriteLine("No sites given :C");
            }
            Console.Read();
            foreach(var s in servers)
            {
                s.Stop();
            }
        }

        class AppDomainProxy : MarshalByRefObject
        {
            public Logger logger;

            public T CreateInstance<T>(params object[] args)
            {
                logger.Log("createInstance");
                return (T)Activator.CreateInstance(typeof(T), args);
            }

            public Assembly ServerDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                logger.Log(args.Name);
                return null;
            }
        }
    }
}
