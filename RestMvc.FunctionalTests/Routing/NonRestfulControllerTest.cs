using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using RestMvc.FunctionalTests.Assertions;

namespace RestMvc.FunctionalTests.Routing
{
    [TestFixture]
    public class NonRestfulControllerTest
    {
        private const string uri = "http://localhost/RestMvc/nonRestful";

        [Test]
        public void ShouldRouteWithoutSubclassing()
        {
            var response = new HttpRequest("GET", uri).GetResponse();
            Assert.That(response.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public void ShouldSupport405StatusCodeWithoutSubclassing()
        {
            var response = new HttpRequest("POST", uri).GetResponse();
            Assert.That(response.StatusCode, Is.EqualTo(405));
            Assert.That(response.Headers["Allow"], Is.EqualTo("GET"));
            Assert.That(response.Body, Is.EqualTo(""));
        }

        [Test]
        public void ShouldSupportOptionsWithoutSubclassing()
        {
            var response = new HttpRequest("OPTIONS", uri).GetResponse();
            Assert.That(response.Headers["Allow"], Is.EqualTo("GET"));
            Assert.That(response.Body, Is.EqualTo(""));
        }

        [Test]
        public void ShouldSupportHeadWithoutSubclassing()
        {
            var getResponse = new HttpRequest("GET", uri).GetResponse();
            var headResponse = new HttpRequest("HEAD", uri).GetResponse();

            Assert.That(headResponse.Body, Is.EqualTo(""));
            Assert.That(headResponse, HasSameHeaders.As(getResponse));
        }
    }
}
