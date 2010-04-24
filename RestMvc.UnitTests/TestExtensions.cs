using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using Moq;

namespace RestMvc.UnitTests
{
    public static class TestExtensions
    {
        public static RestfulController WithStubbedResponse(this RestfulController controller)
        {
            var headers = new NameValueCollection();
            var response = new Mock<HttpResponseBase>();
            response.Setup(r => r.Headers).Returns(headers);
            var context = new Mock<ControllerContext>();
            context.Setup(c => c.HttpContext.Response).Returns(response.Object);
            controller.ControllerContext = context.Object;
            return controller;
        }
    }
}