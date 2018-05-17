using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReflectionContainer;
using ReflectionContainerTest.Classes_for_test;
using System.Reflection;

namespace ReflectionContainerTest
{
    [TestClass]
    public class ReflectionTest
    {
        private ReflectionContainerInjector _container;

        [TestInitialize]
        public void Init()
        {
            _container = new ReflectionContainerInjector(new InstanceActivator());
        }

        [TestMethod]
        public void ContainerAddType_ConstructorInjectionTest()
        {
            _container.AddType(typeof(CustomerBLL));
            _container.AddType(typeof(Logger));
            _container.AddType(typeof(CustomerDAL), typeof(ICustomerDAL));

            var customerBll = _container.CreateInstance(typeof(CustomerBLL));
            var logger = _container.CreateInstance(typeof(Logger));
            var customerDal = _container.CreateInstance(typeof(ICustomerDAL));

            Assert.IsNotNull(customerBll);
            Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL));

            Assert.IsNotNull(logger);
            Assert.IsTrue(logger.GetType() == typeof(Logger));

            Assert.IsNotNull(customerDal);
            Assert.IsTrue(customerDal.GetType() == typeof(CustomerDAL));
        }

        [TestMethod]
        public void CreateGenericInstance_ConstructorInjectionTest()
        {
            _container.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());

            var customerBll = _container.CreateInstance<CustomerBLL>();

            Assert.IsNotNull(customerBll);
            Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL));
        }


        [TestMethod]
        public void CreateInstance_ConstructorInjectionTest()
        {
            _container.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());

            var customerBll = _container.CreateInstance(typeof(CustomerBLL));

            Assert.IsNotNull(customerBll);
            Assert.IsTrue(customerBll.GetType() == typeof(CustomerBLL));
        }

    }
}
