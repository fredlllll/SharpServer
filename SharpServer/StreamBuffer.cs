using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public class StreamBuffer : Buffer
    {
        StreamWriter sw;

        public StreamBuffer(Stream s, Encoding enc) {
            sw = new StreamWriter(s,enc);
        }

        public override void Flush()
        {
            sw.Flush();
        }

        public override void Write(string str)
        {
            sw.Write(str);
        }

        public override void WriteLine(string str)
        {
            sw.WriteLine(str);
        }
    }
}
