using NUnit.Framework;
using RestMvc.FunctionalTests.Assertions;

namespace RestMvc.FunctionalTests.Routing
{
    [TestFixture]
    public class EchoControllerTest
    {
        private const string uri = "http://localhost/RestMvc/echo/hello";

        [Test]
        public void ShouldRouteToEchoAction()
        {
            var response = new HttpRequest("GET", uri).GetResponse();

            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Body, Is.EqualTo("hello"));
        }

        [Test]
        public void UnsupportedMethodShouldReturn405WithAllowHeaderSet()
        {
            var response = new HttpRequest("POST", uri).GetResponse();

            Assert.That(response.StatusCode, Is.EqualTo(405));
            Assert.That(response.Headers["Allow"], Is.EqualTo("GET"));
            Assert.That(response.Body, Is.EqualTo(""));
        }

        [Test]
        public void OptionsShouldSendAllowHeaderWithBody()
        {
            var response = new HttpRequest("OPTIONS", uri).GetResponse();

            Assert.That(response.StatusCode, Is.EqualTo(200));
            Assert.That(response.Headers["Allow"], Is.EqualTo("GET"));
            Assert.That(response.Body, Is.EqualTo("Options body goes here..."));
        }

        [Test]
        public void HeadShouldSendBackSameHeadersAsGetWithEmptyBody()
        {
            var getResponse = new HttpRequest("GET", uri).GetResponse();
            var headResponse = new HttpRequest("HEAD", uri).GetResponse();

            Assert.That(headResponse.Body, Is.EqualTo(""));
            Assert.That(headResponse, HasSameHeaders.As(getResponse));
        }
    }
}
