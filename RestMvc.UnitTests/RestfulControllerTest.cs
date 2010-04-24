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
                return null;
            }

            [Post("test")]
            public ActionResult Create() { return null; }
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
    }
}
