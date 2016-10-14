using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer.HTMLPage
{
    public class NoScriptTag : HTMLTag, IHeaderTag
    {
        public NoScriptTag() : base("noscript")
        {
            TextContent = "";
        }
    }
}
