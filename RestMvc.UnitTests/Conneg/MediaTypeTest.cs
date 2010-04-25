using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using RestMvc.Conneg;

namespace RestMvc.UnitTests.Conneg
{
    [TestFixture]
    public class MediaTypeTest
    {
        [Test]
        public void ShouldReturnContentType()
        {
            var mediaType = new MediaType("text/html");
            Assert.That(mediaType.ContentType, Is.EqualTo("text"));
        }

        [Test]
        public void ShouldReturnSubType()
        {
            var mediaType = new MediaType("text/html");
            Assert.That(mediaType.SubType, Is.EqualTo("html"));
        }

        [Test]
        public void ShouldReturnEmptySubTypeOnMalformedMediaType()
        {
            var mediaType = new MediaType("text");
            Assert.That(mediaType.ContentType, Is.EqualTo("text"));
            Assert.That(mediaType.SubType, Is.EqualTo(""));
        }

        [Test]
        public void ShouldMatchExact()
        {
            var mediaType = new MediaType("text/html");
            Assert.That(mediaType.Matches("text/html"), Is.True);
        }

        [Test]
        public void ShouldNotMatchDifferentMediaType()
        {
            var mediaType = new MediaType("text/html");
            Assert.That(mediaType.Matches("text/xml"), Is.False);
        }

        [Test]
        public void MatchingIsCaseInsensitive()
        {
            var mediaType = new MediaType("text/html");
            Assert.That(mediaType.Matches("TEXT/HTML"));
        }

        [Test]
        public void ShouldMatchWildcardedSubType()
        {
            var mediaType = new MediaType("text/html");
            Assert.That(mediaType.Matches("text/*"));
        }

        [Test]
        public void ShouldMatchWildcardSubTypeCaseInsensitive()
        {
            var mediaType = new MediaType("text/html");
            Assert.That(mediaType.Matches("TEXT/*"));
        }

        [Test]
        public void ShouldMatchWildcardedMediaType()
        {
            var mediaType = new MediaType("text/html");
            Assert.That(mediaType.Matches("*/*"));
        }
    }
}
