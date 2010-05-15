using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using RestMvc.FunctionalTests.Assertions;

namespace RestMvc.FunctionalTests
{
    [TestFixture]
    public class RoutingTests
    {
        private const string echoUri = "http://localhost/RestMvc/echo/hello";
        private const string nonRestfulUri = "http://localhost/RestMvc/nonRestful";
        private const string tunnelledUri = "http://localhost/RestMvc/tunnelled";

        [Test]
        public void ShouldRouteToEchoAction()
        {
            var response = new HttpRequest("GET", echoUri).GetResponse();

            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Body, Is.EqualTo("hello"));
        }

        [Test]
        public void UnsupportedMethodShouldReturn405WithAllowHeaderSet()
        {
            var response = new HttpRequest("POST", echoUri).GetResponse();

            Assert.That(response.StatusCode, Is.EqualTo(405));
            Assert.That(response.Headers["Allow"], Is.EqualTo("GET"));
            Assert.That(response.Body, Is.EqualTo(""));
        }

        [Test]
        public void OptionsShouldSendAllowHeaderWithEmptyBody()
        {
            var response = new HttpRequest("OPTIONS", echoUri).GetResponse();

            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Headers["Allow"], Is.EqualTo("GET"));
            Assert.That(response.Body, Is.EqualTo(""));
        }

        [Test]
        public void HeadShouldSendBackSameHeadersAsGetWithEmptyBody()
        {
            var getResponse = new HttpRequest("GET", echoUri).GetResponse();
            var headResponse = new HttpRequest("HEAD", echoUri).GetResponse();

            Assert.That(headResponse.Body, Is.EqualTo(""));
            Assert.That(headResponse, HasSameHeaders.As(getResponse));
        }

        [Test]
        public void RoutesToNonRestfulController()
        {
            var response = new HttpRequest("GET", nonRestfulUri).GetResponse();
            Assert.That(response.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void NonRestfulControllersSupport405StatusCode()
        {
            var response = new HttpRequest("POST", nonRestfulUri).GetResponse();
            Assert.That(response.StatusCode, Is.EqualTo(405));
            Assert.That(response.Headers["Allow"], Is.EqualTo("GET"));
            Assert.That(response.Body, Is.EqualTo(""));
        }

        [Test]
        public void NonRestfulControllerSupportsOptions()
        {
            var response = new HttpRequest("OPTIONS", nonRestfulUri).GetResponse();
            Assert.That(response.Headers["Allow"], Is.EqualTo("GET"));
        }

        [Test]
        public void NonRestfulControllerSupportsHead()
        {
            var getResponse = new HttpRequest("GET", nonRestfulUri).GetResponse();
            var headResponse = new HttpRequest("HEAD", nonRestfulUri).GetResponse();

            Assert.That(headResponse.Body, Is.EqualTo(""));
            Assert.That(headResponse, HasSameHeaders.As(getResponse));
        }

        [Test]
        public void ShouldTunnelPut()
        {
            var request = new HttpRequest("POST", tunnelledUri).WithPostData("_method", "PUT");
            Assert.That(request.GetResponse().StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void ShouldTunnelDelete()
        {
            var request = new HttpRequest("POST", tunnelledUri).WithPostData("_method", "DELETE");
            Assert.That(request.GetResponse().StatusCode, Is.EqualTo(200));
        }
    }
}
