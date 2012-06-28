using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using RestMvc.Attributes;

namespace RestMvc.UnitTests
{
    [TestFixture]
    public class RestfulControllerTest
    {
        public class TestController : RestfulController
        {
            [Get("test")]
            public ActionResult Index()
            {
                Response.Headers["Cache-Control"] = "public";
                return new ContentResult {Content = "hello"};
            }

            [Post("test")]
            public void Create() { }

            [Get("test/{id}")]
            public ActionResult Show(string id)
            {
                return new ContentResult {Content = "hello " + id};
            }
        }

        public class DifferentSubclassController : Controller
        {
            [Get("test")]
            public ActionResult Test()
            {
                return new ContentResult {Content = "test"};
            }
        }

        public class TestControllerFactory : IControllerFactory, IDisposable
        {
            private readonly IControllerFactory factory;

            public TestControllerFactory()
            {
                factory = ControllerBuilder.Current.GetControllerFactory();
                ControllerBuilder.Current.SetControllerFactory(this);
            }

            public IController CreateController(RequestContext requestContext, string controllerName)
            {
                return new DifferentSubclassController();
            }

            public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName) {
                throw new NotImplementedException();
            }

            public void ReleaseController(IController controller) { }

            public void Dispose()
            {
                ControllerBuilder.Current.SetControllerFactory(factory);
            }
        }

        [Test]
        public void MethodNotSupportedShouldReturn405()
        {
            var controller = new TestController().WithStubbedContext();

            controller.MethodNotSupported("test");

            Assert.That(controller.Response.StatusCode, Is.EqualTo(405));
        }

        [Test]
        public void MethodNotSupportedShouldSetAllowHeader()
        {
            var controller = new TestController().WithStubbedContext();

            controller.MethodNotSupported("test");

            Assert.That(controller.Response.Headers["Allow"], Is.EqualTo("GET, POST"));
        }

        [Test]
        public void MethodNotSupportedShouldSetAllowHeaderWithoutSubclassing()
        {
            var controller = new TestController().WithStubbedContext()
                .WithRouteValue("controllerType", typeof(DifferentSubclassController));

            controller.MethodNotSupported("test");

            Assert.That(controller.Response.Headers["Allow"], Is.EqualTo("GET"));
        }

        [Test]
        public void OptionsShouldSetAllowHeader()
        {
            var controller = new TestController().WithStubbedContext();

            controller.Options("test/{id}");

            Assert.That(controller.Response.Headers["Allow"], Is.EqualTo("GET"));
        }

        [Test]
        public void OptionsShouldSetAllowHeaderForResourceWithoutSubclassing()
        {
            var controller = new RestfulController().WithStubbedContext()
                .WithRouteValue("controllerType", typeof(DifferentSubclassController));

            controller.Options("test");

            Assert.That(controller.Response.Headers["Allow"], Is.EqualTo("GET"));
        }

        [Test]
        public void HeadShouldSendHeadersSetInGetMethod()
        {
            var controller = new TestController().WithStubbedContext();

            controller.Head("test");

            Assert.That(controller.Response.Headers["Cache-Control"], Is.EqualTo("public"));
        }

        [Test]
        public void HeadShouldSetContentLengthHeaderButClearBody()
        {
            var controller = new TestController().WithStubbedContext();

            controller.Head("test");

            Assert.That(controller.Response.Headers["Content-Length"], Is.EqualTo("hello".Length.ToString()));
            Assert.That(controller.Response.Output.ToString(), Is.EqualTo(""));
        }

        [Test]
        public void HeadShouldDelegateToGetMethodWithParameters()
        {
            var controller = new TestController().WithStubbedContext()
                .WithRouteValue("id", "world");
            controller.ValueProvider = new RouteDataValueProvider(controller.ControllerContext);

            controller.Head("test/{id}");

            Assert.That(controller.Response.Headers["Content-Length"],
                Is.EqualTo("hello world".Length.ToString()));
            Assert.That(controller.Response.Output.ToString(), Is.EqualTo(""));
        }

        [Test]
        public void HeadShouldBeHandledWithoutSubclassing()
        {
            using (new TestControllerFactory())
            {
                var controller = new RestfulController().WithStubbedContext()
                    .WithRouteValue("controllerType", typeof(DifferentSubclassController));

                controller.Head("test");

                Assert.That(controller.Response.Headers["Content-Length"], Is.EqualTo("test".Length.ToString()));
            }
        }

        [Test]
        public void HeadShouldChangeRouteAction()
        {
            // For example, the Representation class depends on the route action, and would
            // fail if called under the context of a HEAD call
            var controller = new TestController().WithStubbedContext()
                .WithRouteValue("action", "Head");

            controller.Head("test");

            Assert.That(controller.RouteData.Values["action"], Is.EqualTo("Index"));
        }
    }
}
