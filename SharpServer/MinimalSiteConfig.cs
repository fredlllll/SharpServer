using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SharpServer
{
    internal class MinimalSiteConfig
    {
        public string SiteName { get; }
        public FileInfo File { get; }
        public MinimalSiteConfig(FileInfo siteConfig)
        {
            File = siteConfig;
            XmlDocument doc = new XmlDocument();
            doc.Load(siteConfig.FullName);

            SiteName = doc.SelectSingleNode("/site/name").InnerText;

        }
    }
}