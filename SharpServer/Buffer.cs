using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public abstract class Buffer
    {
        public abstract void Write(string str);
        public abstract void WriteLine(string str);
        public abstract void Flush();
    }
}
