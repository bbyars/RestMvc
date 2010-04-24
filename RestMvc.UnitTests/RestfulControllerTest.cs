using System.Web;
using System.Web.Mvc;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using RestMvc.Attributes;
using RestMvc.UnitTests.Assertions;

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
            public ActionResult Create() { return null; }

            [Get("test/{id}")]
            public ActionResult Show(string id)
            {
                Response.Headers["Cache-Control"] = "no-cache";
                return new ContentResult {Content = "hello " + id};
            }
        }

        [Test]
        public void MethodNotSupportedShouldThrow405()
        {
            var controller = new TestController().WithStubbedResponse();
            CustomAssert.That(() => controller.MethodNotSupported("test"),
                Throws<HttpException>.Where(ex => ex.GetHttpCode(), Is.EqualTo(405)));
        }

        [Test]
        public void MethodNotSupportedShouldSetAllowHeader()
        {
            var controller = new TestController().WithStubbedResponse();
            CustomAssert.That(() => controller.MethodNotSupported("test"),
                Throws<HttpException>.Where(ex => controller.Response.Headers["Allow"], Is.EqualTo("GET, POST")));
        }

        [Test]
        public void OptionsShouldSetAllowHeader()
        {
            var controller = new TestController().WithStubbedResponse();

            controller.Options("test/{id}");

            Assert.That(controller.Response.Headers["Allow"], Is.EqualTo("GET"));
        }

        [Test]
        public void HeadShouldSendHeadersSetInGetMethod()
        {
            var controller = new TestController().WithStubbedResponse();

            controller.Head("test");

            Assert.That(controller.Response.Headers["Cache-Control"], Is.EqualTo("public"));
        }

        [Test]
        public void HeadShouldSetContentLengthHeaderButClearBody()
        {
            var controller = new TestController().WithStubbedResponse();

            controller.Head("test");

            Assert.That(controller.Response.Headers["Content-Length"], Is.EqualTo("hello".Length.ToString()));
            Assert.That(controller.Response.Output.ToString(), Is.EqualTo(""));
        }
    }
}
