using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SharpServer
{
    public class SiteConfig : MarshalByRefObject
    {
        public string SiteName { get; }

        public DirectoryInfo DocumentRoot { get; }
        public FileInfo SiteBinary { get; }
        public int Port { get; }
        public string[] Dependencies { get; }
        public Encoding Encoding { get; }

        public SiteConfig(FileInfo siteConfig)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(siteConfig.FullName);

            SiteName = doc.SelectSingleNode("/site/name").InnerText;

            DocumentRoot = new DirectoryInfo(doc.SelectSingleNode("/site/documentRoot").InnerText);
            SiteBinary = new FileInfo(Path.Combine(DocumentRoot.FullName, doc.SelectSingleNode("/site/siteBinary").InnerText));
            Port = Convert.ToInt32(doc.SelectSingleNode("/site/port").InnerText);
            List<string> dependencies = new List<string>();
            foreach(XmlNode node in doc.SelectNodes("/site/dependencies/dependency"))
            {
                dependencies.Add(node.InnerText);
            }
            Dependencies = dependencies.ToArray();
            var n = doc.SelectSingleNode("/site/encoding");
            if(n != null)
            {
                try
                {
                    Encoding = Encoding.GetEncoding(n.InnerText);
                }
                catch
                {
                    Encoding = Encoding.UTF8;
                }
            }
            else
            {
                
                Encoding = Encoding.UTF8;
            }
        }
    }
}
