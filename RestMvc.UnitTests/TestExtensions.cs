using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace RestMvc.UnitTests
{
    public static class TestExtensions
    {
        public static TController WithRouteValue<TController>(this TController controller, string key, object value)
            where TController : Controller
        {
            controller.RouteData.Values[key] = value;
            return controller;
        }

        public static TController WithStubbedContext<TController>(this TController controller)
            where TController : Controller
        {
            var context = new Mock<HttpContextBase>();
            context.Setup(c => c.Request).Returns(GetRequestStub().Object);
            context.Setup(c => c.Response).Returns(GetResponseStub().Object);

            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);
            return controller;
        }

        private static Mock<HttpRequestBase> GetRequestStub()
        {
            return new Mock<HttpRequestBase>();
        }

        private static Mock<HttpResponseBase> GetResponseStub()
        {
            var headers = new NameValueCollection();
            var output = new StringWriter();
            var response = new Mock<HttpResponseBase>();
            response.SetupAllProperties();
            response.Setup(r => r.Headers).Returns(headers);
            response.Setup(r => r.Output).Returns(output);
            response.Setup(r => r.Write(It.IsAny<string>())).Callback((string s) => output.Write(s));
            return response;
        }
    }
}
