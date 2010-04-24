using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using RestMvc.Attributes;
using RestMvc.UnitTests.Assertions;

namespace RestMvc.UnitTests
{
    [TestFixture]
    public class ResourceMapperTest
    {
        public class EmptyController : RestfulController { }

        public class TestController : RestfulController
        {
            [Get("Test")]
            public ActionResult List() { return null; }

            [Post("test")]
            public ActionResult Create() { return null; }

            [Get("Test/{id}")]
            public ActionResult Show() { return null; }
        }

        [Test]
        public void ControllerWithNoResourcesShouldNotMapsOptions()
        {
            var routes = new RouteCollection();
            var mapper = new ResourceMapper<EmptyController>();

            mapper.MapSupportedMethods(routes);

            Assert.That(routes.Count, Is.EqualTo(0));
        }

        [Test]
        public void ShouldCreateRoutesForAnnotatedActions()
        {
            var routes = new RouteCollection();
            var mapper = new ResourceMapper<TestController>();

            mapper.MapSupportedMethods(routes);

            Assert.That("GET /Test", Routes.To(new {controller = "Test", action = "List"}, routes));
            Assert.That("POST /Test", Routes.To(new {controller = "Test", action = "Create"}, routes));
        }

        [Test]
        public void ShouldCreateRoutesToBypassContentNegotiation()
        {
            var routes = new RouteCollection();
            var mapper = new ResourceMapper<TestController>();

            mapper.MapSupportedMethods(routes);

            Assert.That("GET /Test.xml", Routes.To(new {controller = "Test", action = "List", format = "xml"}, routes));
        }

        [Test]
        public void ShouldCreateMethodNotSupportedRoutesForUnmappedHttpMethods()
        {
            var routes = new RouteCollection();
            var mapper = new ResourceMapper<TestController>();

            mapper.MapUnsupportedMethods(routes);

            var methodNotSupported = new {controller = "Test",
                action = RestfulController.MethodNotSupportedAction, resourceUri = "test"};
            Assert.That("DELETE /test", Routes.To(methodNotSupported, routes));
            Assert.That("PUT /test", Routes.To(methodNotSupported, routes));
        }

        [Test]
        public void ShouldMapHeadForAllResources()
        {
            var routes = new RouteCollection();
            var mapper = new ResourceMapper<TestController>();

            mapper.MapHead(routes);

            Assert.That("HEAD /test", Routes.To(new {controller = "Test", action = "Head", resourceUri = "Test"}, routes));
            Assert.That("HEAD /test/1", Routes.To(new {controller = "Test", action = "Head", resourceUri = "Test/{id}"}, routes));
        }

        [Test]
        public void ShouldMapOptionsForAllResources()
        {
            var routes = new RouteCollection();
            var mapper = new ResourceMapper<TestController>();

            mapper.MapOptions(routes);

            Assert.That("OPTIONS /test", Routes.To(new {controller = "Test", action = "Options", resourceUri = "Test"}, routes));
            Assert.That("OPTIONS /test/1", Routes.To(new {controller = "Test", action = "Options", resourceUri = "Test/{id}"}, routes));
        }
    }
}
