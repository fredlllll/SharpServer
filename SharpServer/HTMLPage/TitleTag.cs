using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer.HTMLPage
{
    public class TitleTag : HTMLTag, IHeaderTag
    {
        public TitleTag() : base("title")
        {
            AllowSelfClosing = false;
            //TextContent = "";
        }
    }
}
