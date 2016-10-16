using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public class SiteFile
    {
        public string Name { get; }
        public Type PageType { get; internal set; }
        public FileInfo LocalFile { get; }

        public SiteFile(string name, SiteDirectory parent)
        {
            Name = name;
            LocalFile = new FileInfo(Path.Combine(parent.LocalDirectory.FullName, name));
        }
    }
}
