using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer.HTMLPage
{
    public class StyleTag : HTMLTag, IHeaderTag
    {
        public string media
        {
            get
            {
                return GetAttribute("media");
            }
            set
            {
                SetAttribute("media", value);
            }
        }

        public bool scoped
        {
            get
            {
                string tmp;
                return TryGetAttribute("scoped",out tmp);
            }
            set
            {
                if(scoped)
                {
                    SetAttribute("scoped", null);
                }
                else
                {
                    RemoveAttribute("scoped");
                }
            }
        }

        public string type
        {
            get
            {
                return GetAttribute("type");
            }
            set
            {
                SetAttribute("type", value);
            }
        }

        public StyleTag() : base("style")
        {
        }
    }
}
