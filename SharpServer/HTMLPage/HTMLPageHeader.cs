using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer.HTMLPage
{
    public abstract class HTMLPageHeader
    {
        public List<IHeaderTag> Tags { get; set; } = new List<IHeaderTag>();

        public string Emit()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < Tags.Count; i++)
            {
                sb.AppendLine(Tags[i].Emit());
            }
            return sb.ToString();
        }
    }
}
