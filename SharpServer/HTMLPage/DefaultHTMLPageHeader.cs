using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer.HTMLPage
{
    public class DefaultHTMLPageHeader : HTMLPageHeader
    {
        TitleTag title = new TitleTag();
        public string Title
        {
            get
            {
                return title.TextContent;
            }
            set
            {
                title.TextContent = value;
            }
        }

        public DefaultHTMLPageHeader()
        {
            var metaCharset = new MetaTag();
            metaCharset.charset = "utf-8";
            Tags.Add(metaCharset);
            Tags.Add(title);
        }
    }
}
