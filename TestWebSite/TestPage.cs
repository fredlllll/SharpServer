using System;
using System.Text;
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

        public override void Emit()
        {
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "text/html";

            EmitHTMLStart();
            EmitHeader();
            EmitBodyStart();
            Buffer.Write("hello world");
            EmitBodyEnd();
            EmitHTMLEnd();
        }
    }
}
