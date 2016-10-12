using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpServer;

namespace TestWebSite
{
    public class TestSite : Site
    {
        public override void Setup(SharpServer.SharpServer server)
        {
            server.pages.Add("/index.ssp", new TestPage());
        }

        public override void Dispose()
        {
         
        }
    }
}
