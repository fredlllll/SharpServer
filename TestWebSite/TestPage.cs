using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SharpServer;
using SharpServer.HTMLPage;

namespace TestWebSite
{
    public class TestPage : HTMLPage
    {
        public TestPage()
        {
            DefaultHTMLPageHeader h = (DefaultHTMLPageHeader)Header;
            h.Title = "Hello World";
        }

        public override void BeforeEmit()
        {
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "text/html";
        }

        public override void Emit()
        {
            EmitHTMLStart();
            EmitHeader();
            EmitBodyStart();
            Buffer.Write("hello world");
            EmitBodyEnd();
            EmitHTMLEnd();
        }

        public override void AfterEmit()
        {
            
        }

        public override void Reset()
        {
            
        }
    }
}
