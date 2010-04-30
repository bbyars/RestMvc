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
        public static RestfulController WithStubbedResponse(this RestfulController controller)
        {
            var headers = new NameValueCollection();
            var output = new StringWriter();
            var response = new Mock<HttpResponseBase>();
            response.SetupAllProperties();
            response.Setup(r => r.Headers).Returns(headers);
            response.Setup(r => r.Output).Returns(output);
            response.Setup(r => r.Write(It.IsAny<string>())).Callback((string s) => output.Write(s));
            response.Setup(r => r.End()).Callback(
                () => output.GetStringBuilder().Remove(0, output.GetStringBuilder().Length));

            var context = new Mock<ControllerContext>();
            context.Setup(c => c.HttpContext.Response).Returns(response.Object);

            var route = new RouteData();
            context.Setup(c => c.RouteData).Returns(route);

            controller.ControllerContext = context.Object;
            return controller;
        }

        public static RestfulController WithRouteValue(this RestfulController controller, string key, object value)
        {
            controller.RouteData.Values[key] = value;
            return controller;
        }
    }
}
