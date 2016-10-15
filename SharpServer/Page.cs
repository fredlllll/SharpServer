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
    /// represents a page
    /// </summary>
    public abstract partial class Page
    {
        [ThreadStatic]
        static HttpListenerRequest request = null;
        public static HttpListenerRequest Request { get { return request; } internal set { request = value; } }
        [ThreadStatic]
        static HttpListenerResponse response = null;
        public static HttpListenerResponse Response { get { return response; } internal set { response = value; } }
        [ThreadStatic]
        static IPrincipal user = null;
        public static IPrincipal User { get { return user; } internal set { user = value; } }
        [ThreadStatic]
        static Buffer buffer = null;
        public static Buffer Buffer { get { return buffer; } internal set { buffer = value; } }

        public abstract void BeforeEmit();
        public abstract void Emit();
        public abstract void AfterEmit();
        public abstract void Reset();
    }
}
