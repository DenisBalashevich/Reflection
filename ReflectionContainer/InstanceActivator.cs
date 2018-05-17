using System;

namespace ReflectionContainer
{
    public class InstanceActivator : IInstanceActivator
    {
        public object CreateInstance(Type type, params object[] parameters)
        {
            return Activator.CreateInstance(type, parameters);
        }
    }
}
