namespace SharpServer.HTMLPage
{
    public class ScriptTag : HTMLTag, IHeaderTag
    {
		public string async
        {
            get
            {
                return GetAttribute("async");
            }
            set
            {
                SetAttribute("async", value);
            }
        }
        
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
        
		public string defer
        {
            get
            {
                return GetAttribute("defer");
            }
            set
            {
                SetAttribute("defer", value);
            }
        }
        
		public string src
        {
            get
            {
                return GetAttribute("src");
            }
            set
            {
                SetAttribute("src", value);
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
        
        public ScriptTag() : base("script")
        {
        }
    }
}