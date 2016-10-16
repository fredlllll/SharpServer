using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public class SiteDirectory
    {
        public DirectoryInfo LocalDirectory { get; }
        public string Name { get; }
        public ConcurrentDictionary<string, SiteDirectory> SubDirectories { get; } = new ConcurrentDictionary<string, SiteDirectory>();
        public ConcurrentDictionary<string, SiteFile> Files { get; } = new ConcurrentDictionary<string, SiteFile>();

        public SiteDirectory(string name, SiteDirectory parent)
        {
            Name = name;
            LocalDirectory = new DirectoryInfo(Path.Combine(parent.LocalDirectory.FullName, Name));
        }

        public SiteDirectory(string name, DirectoryInfo localDirectory)
        {
            Name = name;
            LocalDirectory = localDirectory;
        }

        public SiteFile GetIndexFile()
        {
            foreach(var kv in Files)
            {
                if(kv.Key == "index.ssp")//TODO: gotta add a list of stuff
                {
                    return kv.Value;
                }
            }
            return null;
        }

        //TODO: use iterative approach instead of recursive, but that should do for now

        public SiteFile AddFile(string path)
        {
            string[] sa = path.Split(Path.DirectorySeparatorChar);
            Queue<string> pathQueue = new Queue<string>(sa);
            return AddFile(pathQueue);
        }

        SiteFile AddFile(Queue<string> path)
        {
            if(path.Count > 1)
            {
                string dir = path.Dequeue();
                SiteDirectory directory;
                if(SubDirectories.TryGetValue(dir, out directory))
                {
                    return directory.AddFile(path);
                }
                directory = new SiteDirectory(dir, this);
                SubDirectories[dir] = directory;
                return directory.AddFile(path);
            }
            else
            {
                string fname = path.Dequeue();
                SiteFile file;
                if(Files.TryGetValue(fname, out file))
                {
                    return file;
                }
                file = new SiteFile(fname, this);
                Files[fname] = file;
                return file;
            }
        }

        public SiteDirectory GetDirectory(string path)
        {
            string[] sa = path.Split(Path.DirectorySeparatorChar);
            Queue<string> pathQueue = new Queue<string>(sa);
            return GetDirectory(pathQueue);
        }

        SiteDirectory GetDirectory(Queue<string> path)
        {
            string dir = path.Dequeue();
            SiteDirectory directory;
            if(SubDirectories.TryGetValue(dir, out directory))
            {
                return directory.GetDirectory(path);
            }
            return null;
        }

        public SiteFile GetFile(string path)
        {
            string[] sa = path.Split(Path.DirectorySeparatorChar);
            Queue<string> pathQueue = new Queue<string>(sa);
            return GetFile(pathQueue);
        }

        SiteFile GetFile(Queue<string> path)
        {
            if(path.Count > 1)
            {
                string dir = path.Dequeue();
                SiteDirectory directory;
                if(SubDirectories.TryGetValue(dir, out directory))
                {
                    return directory.GetFile(path);
                }
            }
            else
            {
                string fname = path.Dequeue();
                SiteFile file;
                if(Files.TryGetValue(fname, out file))
                {
                    return file;
                }
            }
            return null;
        }

        public void RemoveFile(string path)
        {
            string[] sa = path.Split(Path.DirectorySeparatorChar);
            Queue<string> pathQueue = new Queue<string>(sa);
            RemoveFile(pathQueue);
        }

        void RemoveFile(Queue<string> path)
        {
            if(path.Count > 1)
            {
                string dir = path.Dequeue();
                SiteDirectory directory;
                if(SubDirectories.TryGetValue(dir, out directory))
                {
                    directory.RemoveFile(path);
                    if(directory.Files.Count == 0 && directory.SubDirectories.Count == 0)
                    {
                        SubDirectories.TryRemove(dir, out directory);
                    }
                }
            }
            else
            {
                string fname = path.Dequeue();
                SiteFile file;
                Files.TryRemove(fname, out file);
            }
        }
    }
}
