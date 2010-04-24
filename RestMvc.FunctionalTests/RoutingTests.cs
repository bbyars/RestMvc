using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace RestMvc.FunctionalTests
{
    [TestFixture]
    public class RoutingTests
    {
        [Test]
        public void ShouldRouteToEchoAction()
        {
            var request = HttpRequest.Get("http://localhost/RestMvc/echo/hello");
            var response = request.GetResponse();

            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Body, Is.EqualTo("hello"));
        }
    }
}
