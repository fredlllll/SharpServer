namespace SharpServer.HTMLPage
{
    public class MetaTag : HTMLTag, IHeaderTag
    {
		public string charset
        {
            get
            {
                return GetAttribute("charset");
            }
            set
            {
                SetAttribute("charset", value);
            }
        }
        
		public string content
        {
            get
            {
                return GetAttribute("content");
            }
            set
            {
                SetAttribute("content", value);
            }
        }
        
		public string http_equiv
        {
            get
            {
                return GetAttribute("http-equiv");
            }
            set
            {
                SetAttribute("http-equiv", value);
            }
        }
        
		public string name
        {
            get
            {
                return GetAttribute("name");
            }
            set
            {
                SetAttribute("name", value);
            }
        }
        
		public string scheme
        {
            get
            {
                return GetAttribute("scheme");
            }
            set
            {
                SetAttribute("scheme", value);
            }
        }
        
        public MetaTag() : base("meta")
        {
        }
    }
}