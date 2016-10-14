namespace SharpServer.HTMLPage
{
    public class LinkTag : HTMLTag, IHeaderTag
    {
		public string crossorigin
        {
            get
            {
                return GetAttribute("crossorigin");
            }
            set
            {
                SetAttribute("crossorigin", value);
            }
        }
        
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
        
		public string hreflang
        {
            get
            {
                return GetAttribute("hreflang");
            }
            set
            {
                SetAttribute("hreflang", value);
            }
        }
        
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
        
		public string rel
        {
            get
            {
                return GetAttribute("rel");
            }
            set
            {
                SetAttribute("rel", value);
            }
        }
        
		public string sizes
        {
            get
            {
                return GetAttribute("sizes");
            }
            set
            {
                SetAttribute("sizes", value);
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
        
        public LinkTag() : base("link")
        {
        }
    }
}