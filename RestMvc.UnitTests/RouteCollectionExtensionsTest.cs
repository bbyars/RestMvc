using System.Reflection;
using System.Web.Mvc;
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

        public class NonRestfulController : Controller
        {
            [Get("nonRestful")]
            public void NonRestful() {}
        }

        [Test]
        public void MapShouldAddAllResourcefulRoutes()
        {
            var routes = new RouteCollection();
            routes.Map<FirstController>();

            Assert.That("GET /First", Routes.To(new {controller = "First", action = "First"}, routes));
            Assert.That("POST /First", Routes.To(new {controller = "First", action = "MethodNotSupported"}, routes));
            Assert.That("HEAD /First", Routes.To(new {controller = "First", action = "Head"}, routes));
            Assert.That("OPTIONS /First", Routes.To(new {controller = "First", action = "Options"}, routes));
        }

        [Test]
        public void MapAllShouldMapAllNonAbstractControllersInAssembly()
        {
            var routes = new RouteCollection();
            routes.MapAssembly(Assembly.GetExecutingAssembly());

            Assert.That("GET /First", Routes.To(new {controller = "First", action = "First"}, routes));
            Assert.That("POST /First", Routes.To(new {controller = "First", action = "MethodNotSupported"}, routes));
            Assert.That("HEAD /First", Routes.To(new {controller = "First", action = "Head"}, routes));
            Assert.That("OPTIONS /First", Routes.To(new {controller = "First", action = "Options"}, routes));

            Assert.That("GET /Second", Routes.To(new {controller = "Second", action = "Second"}, routes));
            Assert.That("POST /Second", Routes.To(new {controller = "Second", action = "MethodNotSupported"}, routes));
            Assert.That("HEAD /Second", Routes.To(new {controller = "Second", action = "Head"}, routes));
            Assert.That("OPTIONS /Second", Routes.To(new {controller = "Second", action = "Options"}, routes));

            Assert.That("GET /NonRestful", Routes.To(new {controller = "NonRestful", action = "NonRestful"}, routes));
            Assert.That("POST /NonRestful", Routes.To(new {controller = "Restful", action = "MethodNotSupported"}, routes));
        }
    }
}
