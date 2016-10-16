using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SharpServer
{
    public class URIResolver : IDisposable
    {
        ConcurrentDictionary<Type, ConcurrentStack<Page>> pageInstances = new ConcurrentDictionary<Type, ConcurrentStack<Page>>();

        public string Path { get; }
        public DirectoryInfo DocumentRootInfo { get; }
        public SiteDirectory DocumentRoot { get; }
        FileSystemWatcher documentRootWatcher;

        public URIResolver(string path, DirectoryInfo documentRoot)
        {
            Path = path;
            DocumentRootInfo = documentRoot;

            DocumentRoot = new SiteDirectory("documentRoot", documentRoot);
            var files = documentRoot.GetFiles("*", SearchOption.AllDirectories);
            foreach(var file in files)
            {
                string name = file.FullName.Replace(documentRoot.FullName, "").Remove(0, 1);
                UpdateFile(DocumentRoot.AddFile(name));
            }

            documentRootWatcher = new FileSystemWatcher(documentRoot.FullName);
            documentRootWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;// | NotifyFilters.Size;
            documentRootWatcher.Changed += DocumentRootWatcher_Changed;
            documentRootWatcher.Created += DocumentRootWatcher_Created;
            documentRootWatcher.Deleted += DocumentRootWatcher_Deleted;
            //documentRootWatcher.Renamed += DocumentRootWatcher_Renamed;

            documentRootWatcher.EnableRaisingEvents = true;
        }

        /*private void DocumentRootWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            throw new NotImplementedException();
        }*/

        private void DocumentRootWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            FileAttributes attr = File.GetAttributes(e.FullPath);
            if((attr & FileAttributes.Directory) != FileAttributes.Directory)
            {
                string path = e.FullPath.Replace(DocumentRootInfo.FullName, "").Remove(0, 1);
                DocumentRoot.RemoveFile(path);
            }
        }

        private void DocumentRootWatcher_Created(object sender, FileSystemEventArgs e)
        {
            FileAttributes attr = File.GetAttributes(e.FullPath);
            if((attr & FileAttributes.Directory) != FileAttributes.Directory)
            {
                string path = e.FullPath.Replace(DocumentRootInfo.FullName, "").Remove(0, 1);
                var file = DocumentRoot.AddFile(path);
                UpdateFile(file);
            }
        }

        private void DocumentRootWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            FileAttributes attr = File.GetAttributes(e.FullPath);
            if((attr & FileAttributes.Directory) != FileAttributes.Directory)
            {
                string path = e.FullPath.Replace(DocumentRootInfo.FullName, "").Remove(0, 1);
                var file = DocumentRoot.GetFile(path);
                UpdateFile(file);
            }
        }

        private void UpdateFile(SiteFile file)
        {
            //TODO: parse file
            string ext = file.LocalFile.Extension;
            if(ext == ".ssp")
            {
                XmlDocument ssp = new XmlDocument();
                ssp.Load(file.LocalFile.FullName);
                file.PageType = ServerUtil.FindType(ssp.SelectSingleNode("/ssp/pageType").InnerText);
            }
            else
            {
                file.PageType = typeof(FilePage);
            }
            return;
        }

        public virtual Page Resolve(HttpListenerRequest request)
        {
            string path = request.Url.AbsolutePath.Remove(0, Path.Length).TrimStart('/');
            SiteFile file = null;
            if(path.Length == 0 || path.EndsWith("/"))
            {
                if(path.Length == 0)// index/default page
                {
                    file = DocumentRoot.GetIndexFile();
                }
                else if(path.EndsWith("/")) // index/default page
                {
                    SiteDirectory dir = DocumentRoot.GetDirectory(path);
                    if(dir != null)
                    {
                        file = dir.GetIndexFile();
                    }
                }
            }
            else
            {
                file = DocumentRoot.GetFile(path);
            }
            if(file == null)
            {
                return null;
            }
            if(file.PageType != null)
            {
                Page p = GetPageInstance(file.PageType);
                p.RequestedSiteFile = file;
                return p;
            }
            else
            {
                Trace.WriteLine("file " + file.LocalFile + " has missing pageType");
                return null;
            }
        }

        protected T GetPageInstance<T>() where T : Page, new()
        {
            Type t = typeof(T);
            return (T)GetPageInstance(t);
        }

        protected Page GetPageInstance(Type t)
        {
            ConcurrentStack<Page> instances;
            if(pageInstances.TryGetValue(t, out instances))
            {
                Page retval = null;
                if(instances.TryPop(out retval))
                {
                    return retval;
                }
            }
            return (Page)Activator.CreateInstance(t);
        }

        public void ReturnPageInstance(Page instance)
        {
            if(instance == null)
            {
                throw new ArgumentNullException(nameof(instance) + " can't be null");
            }
            Type t = instance.GetType();
            ConcurrentStack<Page> instances;
            if(pageInstances.TryGetValue(t, out instances))
            {
                instances.Push(instance);
            }
            else
            {
                instances = new ConcurrentStack<Page>();
                instances.Push(instance);
                pageInstances[t] = instances;
            }
        }

        public void Dispose()
        {
            documentRootWatcher.Dispose();
        }
    }
}
