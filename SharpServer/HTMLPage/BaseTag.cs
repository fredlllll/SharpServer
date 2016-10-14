using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer.HTMLPage
{
    public class BaseTag : HTMLTag, IHeaderTag
    {
        //<base href="http://www.w3schools.com/images/" target="_blank">
        public string href
        {
            get
            {
                return GetAttribute("href");
            }
            set
            {
                SetAttribute("href", value);
            }
        }
        public string target
        {
            get
            {
                return GetAttribute("target");
            }
            set
            {
                SetAttribute("href", value);
            }
        }

        public BaseTag() : base("base")
        {

        }
    }
}
