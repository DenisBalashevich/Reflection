using System;
using System.Runtime.Serialization;

namespace ReflectionContainer.Exceptions
{
    [Serializable]
    public class ContainerException : Exception
    {
        public ContainerException()
        {
        }

        public ContainerException(string message)
            : base(message)
        {
        }
    }
}
