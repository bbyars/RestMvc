using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace RestMvc.FunctionalTests
{
    [TestFixture]
    public class ConnegTests
    {
        private const string echoUri = "http://localhost/RestMvc/echo/hello";

        [Test]
        public void ShouldSendPlainTextIfNoAcceptTypeGiven()
        {
            var request = new HttpRequest("GET", echoUri).WithAcceptTypes("");
            var response = request.GetResponse();

            Assert.That(response.ContentType, Text.StartsWith("text/plain"));
            Assert.That(response.Body, Is.EqualTo("hello"));
        }

        [Test]
        public void ShouldSendXmlIfClientAcceptsXmlButNotPlainText()
        {
            var request = new HttpRequest("GET", echoUri).WithAcceptTypes("text/html", "application/xml");
            var response = request.GetResponse();

            Assert.That(response.ContentType, Text.StartsWith("application/xml"));
            Assert.That(response.Body, Is.EqualTo("<echo>hello</echo>"));
        }

        [Test]
        public void ShouldSendPlainTextIfClientSendsUnacceptableAcceptTypes()
        {
            var request = new HttpRequest("GET", echoUri).WithAcceptTypes("audio/*", "text/csv");
            var response = request.GetResponse();

            Assert.That(response.ContentType, Text.StartsWith("text/plain"));
        }

        [Test]
        public void ShouldRespectClientPrioritization()
        {
            var request = new HttpRequest("GET", echoUri).WithAcceptTypes("application/xml", "text/plain");
            var response = request.GetResponse();

            Assert.That(response.ContentType, Text.StartsWith("application/xml"));
        }

        [Test]
        public void PassingFormatOnUrlShouldBypassContentNegotiation()
        {
            var request = new HttpRequest("GET", echoUri + ".xml").WithAcceptTypes("text/plain");
            var response = request.GetResponse();

            Assert.That(response.Body, Is.EqualTo("<echo>hello</echo>"));
            Assert.That(response.ContentType, Text.StartsWith("application/xml"));
        }
    }
}
