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

        //public DirectoryInfo DocumentRoot { get; }
        public FileInfo SiteBinary { get; }
        public int Port { get; }
        public string[] Dependencies { get; }
        public Encoding Encoding { get; }
        public Dictionary<string, URIResolver> Directories { get; } = new Dictionary<string, URIResolver>();
        XmlDocument doc;

        public SiteConfig(FileInfo siteConfig)
        {
            doc = new XmlDocument();
            doc.Load(siteConfig.FullName);

            SiteName = doc.SelectSingleNode("/site/name").InnerText;

            //DocumentRoot = new DirectoryInfo(doc.SelectSingleNode("/site/documentRoot").InnerText);
            SiteBinary = new FileInfo(doc.SelectSingleNode("/site/siteBinary").InnerText);
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

        public void LoadDirectories()
        {
            foreach(XmlNode node in doc.SelectNodes("/site/directories/directory"))
            {
                string path = node.Attributes["path"].Value;
                DirectoryInfo documentRoot = new DirectoryInfo(node.Attributes["documentRoot"].Value);
                string resolverType = node.Attributes["resolverType"]?.Value;
                Type t = null;
                if(resolverType != null && (t = ServerUtil.FindType(resolverType)) != null)
                {
                    try
                    {
                        Directories[path] = (URIResolver)Activator.CreateInstance(t, new object[] { path, documentRoot });
                        continue;
                    }
                    catch { }
                }
                Directories[path] = new URIResolver(path, documentRoot);
            }
        }
    }
}
