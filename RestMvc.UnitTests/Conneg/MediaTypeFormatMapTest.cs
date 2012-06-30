using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using RestMvc.Conneg;

namespace RestMvc.UnitTests.Conneg
{
    [TestFixture]
    public class MediaTypeFormatMapTest
    {
        [Test]
        public void EmptyMapShouldNotHaveAnyMediaTypes()
        {
            var map = new MediaTypeFormatMap();
            Assert.That(map.SupportsMediaType("*/*"), Is.False);
        }

        [Test]
        public void ShouldSupportAddedMediaType()
        {
            var map = new MediaTypeFormatMap();
            map.Add("text/html", "html");
            Assert.That(map.SupportsMediaType("text/html"), Is.True);
        }

        [Test]
        public void ShouldSupportWildCardedTextMediaType()
        {
            var map = new MediaTypeFormatMap();
            map.Add("text/html", "html");
            Assert.That(map.SupportsMediaType("text/*"), Is.True);
        }

        [Test]
        public void ShouldSupportWildCardedMediaType()
        {
            var map = new MediaTypeFormatMap();
            map.Add("text/html", "html");
            Assert.That(map.SupportsMediaType("*/*"), Is.True);
        }

        [Test]
        public void ShouldNotSupportNonAddsedMediaTypeWithoutWildcard()
        {
            var map = new MediaTypeFormatMap();
            map.Add("text/html", "html");
            Assert.That(map.SupportsMediaType("text/plain"), Is.False);
        }

        [Test]
        public void ShouldReturnNullFormatIfUnsupportedMediaTypeRequested()
        {
            var map = new MediaTypeFormatMap();
            Assert.That(map.FormatFor("*/*"), Is.Null);
        }

        [Test]
        public void ShouldReturnFormatForExactMediaType()
        {
            var map = new MediaTypeFormatMap();
            map.Add("text/html", "html");
            Assert.That(map.FormatFor("text/html"), Is.EqualTo("html"));
        }

        [Test]
        public void MapShouldTakePriorityOverParameterMediaTypes()
        {
            var map = new MediaTypeFormatMap();
            map.Add("text/html", "html");
            map.Add("application/xml", "xml");
            Assert.That(map.FormatFor("application/xml", "text/html"), Is.EqualTo("html"));
        }

        [Test]
        public void EmptyMapHasNoDefaultFormat()
        {
            var map = new MediaTypeFormatMap();
            Assert.That(map.DefaultFormat, Is.Null);
        }

        [Test]
        public void DefaultFormatIsFirstEntry()
        {
            var map = new MediaTypeFormatMap();
            map.Add("text/html", "html");
            map.Add("text/xml", "xml");

            Assert.That(map.DefaultFormat, Is.EqualTo("html"));
        }
    }
}
