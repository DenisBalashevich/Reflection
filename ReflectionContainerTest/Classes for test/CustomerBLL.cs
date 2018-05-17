using ReflectionContainer.Attributes;

namespace ReflectionContainerTest.Classes_for_test
{
    [ImportConstructor]
    public class CustomerBLL
    {
        public CustomerBLL(ICustomerDAL dal, Logger logger)
        { }
    }
}
