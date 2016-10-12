using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public abstract class WebPage : Page
    {
        protected WebPageHeader Header { get; } = new WebPageHeader();

        protected void EmitHTMLStart(Buffer buf)
        {
            buf.Write("<!DOCTYPE html><html>");
        }
        protected void EmitHeader(Buffer buf)
        {
            buf.Write("<head></head");//TODO: emit header stuff
        }
        protected void EmitBodyStart(Buffer buf)
        {
            buf.Write("<body>");
        }
        protected void EmitBodyEnd(Buffer buf)
        {
            buf.Write("</body>");
        }
        protected void EmitHTMLEnd(Buffer buf)
        {
            buf.Write("</html>");
        }
    }
}
