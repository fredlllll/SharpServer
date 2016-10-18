using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpServer
{
    public class InstanceLender<T> where T : class, new()
    {
        ConcurrentStack<T> instances = new ConcurrentStack<T>();

        public T Lend()
        {
            T retval = null;
            if(instances.TryPop(out retval))
            {
                return retval;
            }
            return new T();
        }

        public void Return(T instance)
        {
            if(instance == null)
            {
                throw new ArgumentNullException(nameof(instance) + " can't be null");
            }
            instances.Push(instance);
        }
    }
}
