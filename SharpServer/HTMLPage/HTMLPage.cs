using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer.HTMLPage
{
    public abstract class HTMLPage : Page
    {
        protected HTMLPageHeader Header { get; set; } = new DefaultHTMLPageHeader();

        protected void EmitHTMLStart()
        {
            Buffer.Write("<!DOCTYPE html><html>");
        }
        protected void EmitHeader()
        {
            Buffer.Write("<head>");
            if(Header != null)
            {
                Buffer.Write(Header.Emit());
            }
            Buffer.Write("</head>");
        }
        protected void EmitBodyStart()
        {
            Buffer.Write("<body>");
        }
        protected void EmitBodyEnd()
        {
            Buffer.Write("</body>");
        }
        protected void EmitHTMLEnd()
        {
            Buffer.Write("</html>");
        }
    }
}
