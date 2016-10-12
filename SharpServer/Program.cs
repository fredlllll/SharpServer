using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    class Program
    {
        static SharpServer ss;
        static void Main(string[] args)
        {
            ss = new SharpServer(new SiteConfig(new FileInfo(@"C:\haha.bin")));
            ss.Start();
            Console.Read();
            ss.Stop();
        }

        public static Program prog = new Program();

        public void Log(String msg)
        {
            System.Diagnostics.Debug.WriteLine(msg);
            Console.WriteLine(msg);
        }
    }
}
