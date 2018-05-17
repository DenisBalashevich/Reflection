using ReflectionContainer;
using ReflectionContainer.Attributes;

namespace ReflectionContainerTest.Classes_for_test
{
    [Export(typeof(ICustomerDAL))]
    public class CustomerDAL : ICustomerDAL
    {
        public CustomerDAL() { }
    }
}
