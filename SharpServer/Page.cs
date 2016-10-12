using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SharpServer
{
    /// <summary>
    /// respresents a page
    /// </summary>
    public abstract class Page
    {
        public abstract void BeforeEmit(HttpListenerResponse response);
        public abstract void Emit(Buffer buf);
        public abstract void AfterEmit();
    }
}
