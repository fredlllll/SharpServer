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
    public abstract class Site : IDisposable
    {
        /// <summary>
        /// setup site state and pages. should move the page routing to the config maybe
        /// </summary>
        public abstract void Setup(SharpServer server);
        /// <summary>
        /// dispose of site state
        /// probably have to replace that with a method that can be called multiple times in the future
        /// </summary>
        public abstract void Dispose();
    }
}
