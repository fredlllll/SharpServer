using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    internal class ServerUtil
    {
        public static Type FindType(string NameWithNamespace)
        {
            Type retval = null;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i =0; i< assemblies.Length && retval == null;i++) {
                var types = assemblies[i].GetTypes();
                for(int j = 0; j < types.Length && retval == null; j++)
                {
                    var t = types[j];
                    if(t.FullName.Equals(NameWithNamespace))
                    {
                        retval = t;
                    }
                }
            }

            return retval;
        }
    }
}
