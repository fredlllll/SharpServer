using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public class SiteConfig
    {
        public string SiteName { get; }

        public DirectoryInfo FileRootDir { get; }
        public FileInfo SiteBinary { get;}
        public string[] Dependencies { get; }

        public SiteConfig(FileInfo siteConfig)
        {
            FileRootDir = new DirectoryInfo(@"I:\Projects\SharpServer\TestWebSite\bin\Debug");
            SiteBinary = new FileInfo(@"I:\Projects\SharpServer\TestWebSite\bin\Debug\TestWebSite.dll");
            Dependencies = new string[0];
        }
    }
}
