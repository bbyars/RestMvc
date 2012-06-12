using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace RestMvc.FunctionalTests.Routing
{
    [TestFixture]
    public class TunnelledControllerTest
    {
        private const string uri = "http://localhost/RestMvc/tunnelled";

        [Test]
        public void ShouldTunnelPut()
        {
            var request = new HttpRequest("POST", uri).WithPostData("_method", "PUT");
            Assert.That(request.GetResponse().StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void ShouldTunnelDelete()
        {
            var request = new HttpRequest("POST", uri).WithPostData("_method", "DELETE");
            Assert.That(request.GetResponse().StatusCode, Is.EqualTo(202));
        }

        [Test]
        public void PostWithoutMethodParamShouldRouteToPost()
        {
            var request = new HttpRequest("POST", uri).WithPostData("key", "value");
            Assert.That(request.GetResponse().StatusCode, Is.EqualTo(201));
        }
    }
}
