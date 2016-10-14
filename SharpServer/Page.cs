using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;

namespace SharpServer
{
    /// <summary>
    /// respresents a page
    /// </summary>
    public abstract partial class Page
    {
        public HttpListenerRequest Request { get; internal set; }
        public HttpListenerResponse Response { get; internal set; }
        public IPrincipal User { get; internal set; }
        public Buffer Buffer { get; internal set; }

        public abstract void BeforeEmit();
        public abstract void Emit();
        public abstract void AfterEmit();
    }
}
