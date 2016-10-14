using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public abstract partial class Page
    {
        public void Echo(string str) {
            Buffer.Write(str);
        }
    }
}
