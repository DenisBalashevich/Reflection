using System;

namespace ReflectionContainer
{
    public interface IInstanceActivator
    {
        object CreateInstance(Type type, params object[] parameters);
    }
}
