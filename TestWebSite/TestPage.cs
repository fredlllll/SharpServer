using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SharpServer;

namespace TestWebSite
{
    public class TestPage : WebPage
    {
        public override void BeforeEmit(HttpListenerResponse response)
        {
            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = "text/html";
        }

        public override void Emit(SharpServer.Buffer buf)
        {
            EmitHTMLStart(buf);
            EmitHeader(buf);
            EmitBodyStart(buf);
            buf.Write("hello world");
            EmitBodyEnd(buf);
            EmitHTMLEnd(buf);
        }

        public override void AfterEmit()
        {
            
        }
    }
}
