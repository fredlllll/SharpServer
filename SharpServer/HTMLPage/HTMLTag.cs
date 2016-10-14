using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer.HTMLPage
{
    public abstract class HTMLTag
    {
        public string Tag { get; }
        public string TextContent { get; set; }
        public Dictionary<string, string> Attributes { get; } = new Dictionary<string, string>();
        protected bool AllowSelfClosing = true;

        public HTMLTag(string tag)
        {
            Tag = tag;
        }

        public string Emit()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<");
            sb.Append(Tag);

            if(Attributes.Count > 0)
            {

                foreach(var att in Attributes)
                {
                    sb.Append(" ");
                    sb.Append(att.Key);
                    if(!string.IsNullOrEmpty(att.Value))
                    {
                        sb.Append("='");
                        sb.Append(att.Value);
                        sb.Append("'");
                    }
                }
            }

            if(TextContent == null)
            {
                if(AllowSelfClosing)
                {
                    sb.Append(" ");
                    sb.Append("/>");
                }
                else
                {
                    sb.Append(">");
                    sb.Append("</");
                    sb.Append(Tag);
                    sb.Append(">");
                }
            }
            else
            {
                sb.Append(">");
                sb.Append(TextContent);
                sb.Append("</");
                sb.Append(Tag);
                sb.Append(">");
            }
            return sb.ToString();
        }

        public void RemoveAttribute(string att)
        {
            Attributes.Remove(att);
        }

        public string GetAttribute(string att, string defaultValue = null)
        {
            string retval;
            if(Attributes.TryGetValue("href", out retval))
            {
                return retval;
            }
            return defaultValue;
        }

        public bool TryGetAttribute(string att, out string value)
        {
            if(Attributes.TryGetValue("href", out value))
            {
                return true;
            }
            value = null;
            return false;
        }

        public void SetAttribute(string att, string value)
        {
            Attributes[att] = value;
        }
    }
}
