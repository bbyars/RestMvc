using System.Reflection;
using System.Web.Routing;
using NUnit.Framework;
using RestMvc.Attributes;
using RestMvc.UnitTests.Assertions;

namespace RestMvc.UnitTests
{
    [TestFixture]
    public class RouteCollectionExtensionsTest
    {
        public class FirstController : RestfulController
        {
            [Get("first")]
            public void First() {}
        }

        public class SecondController : RestfulController
        {
            [Get("second")]
            public void Second() {}
        }

        [Test]
        public void MapShouldAddAllResourcefulRoutes()
        {
            var routes = new RouteCollection();
            routes.Map<FirstController>();

            Assert.That("GET /First", Routes.To(new {controller = "First", action = "First"}, routes));
            Assert.That("POST /First", Routes.To(new {controller = "First", action = RestfulController.MethodNotSupportedAction}, routes));
            Assert.That("HEAD /First", Routes.To(new {controller = "First", action = RestfulController.HeadAction}, routes));
            Assert.That("OPTIONS /First", Routes.To(new {controller = "First", action = RestfulController.OptionsAction}, routes));
        }

        [Test]
        public void MapAllShouldMapAllControllersInAssembly()
        {
            var routes = new RouteCollection();
            routes.MapAssembly(Assembly.GetExecutingAssembly());

            Assert.That("GET /First", Routes.To(new {controller = "First", action = "First"}, routes));
            Assert.That("POST /First", Routes.To(new {controller = "First", action = RestfulController.MethodNotSupportedAction}, routes));
            Assert.That("HEAD /First", Routes.To(new {controller = "First", action = RestfulController.HeadAction}, routes));
            Assert.That("OPTIONS /First", Routes.To(new {controller = "First", action = RestfulController.OptionsAction}, routes));

            Assert.That("GET /Second", Routes.To(new {controller = "Second", action = "Second"}, routes));
            Assert.That("POST /Second", Routes.To(new {controller = "Second", action = RestfulController.MethodNotSupportedAction}, routes));
            Assert.That("HEAD /Second", Routes.To(new {controller = "Second", action = RestfulController.HeadAction}, routes));
            Assert.That("OPTIONS /Second", Routes.To(new {controller = "Second", action = RestfulController.OptionsAction}, routes));
        }
    }
}
