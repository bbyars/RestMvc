using System.Web.Mvc;
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
            public void Test() { }
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
    }
}
