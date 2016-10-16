using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SharpServer
{
    public abstract partial class Page
    {
        public NameValueCollection GET { get { return Request.QueryString; } }
        //TODO: post

        public void Echo(string str)
        {
            Buffer.Write(str);
        }
    }
}
