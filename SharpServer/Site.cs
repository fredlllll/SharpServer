using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    /// <summary>
    /// responsible for creating the site state
    /// </summary>
    public abstract class Site
    {
        /// <summary>
        /// setup site state and pages. should move the page routing to the config maybe
        /// </summary>
        public abstract void Start(SharpServer server);
        /// <summary>
        /// dispose of site state
        /// </summary>
        public abstract void Stop();

        //public abstract 
    }
}
