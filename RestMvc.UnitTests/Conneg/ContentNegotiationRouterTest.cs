using System.Web.Routing;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using RestMvc.Conneg;

namespace RestMvc.UnitTests.Conneg
{
    [TestFixture]
    public class ContentNegotiationRouterTest
    {
        [Test]
        public void ShouldMapMediaTypeToFormat()
        {
            var map = new MediaTypeFormatMap();
            map.Map("text/xml", "xml");
            var router = new ContentNegotiationRouter(map);
            var route = new RouteData();

            router.AddFormat(route, new[] { "*/*" });

            Assert.That(route.Values["format"], Is.EqualTo("xml"));
        }

        [Test]
        public void ShouldNotSetFormatIfRoutingSystemAlreadyDetectedIt()
        {
            var map = new MediaTypeFormatMap();
            map.Map("text/xml", "xml");
            var router = new ContentNegotiationRouter(map);
            var route = new RouteData();
            route.Values["format"] = "html";

            router.AddFormat(route, new[] { "*/*" });

            Assert.That(route.Values["format"], Is.EqualTo("html"));
        }

        [Test]
        public void ShouldUseDefaultFormatIfNoAcceptTypesProvided()
        {
            var map = new MediaTypeFormatMap();
            map.Map("text/xml", "xml");
            var router = new ContentNegotiationRouter(map);
            var route = new RouteData();

            router.AddFormat(route, new string[0]);

            Assert.That(route.Values["format"], Is.EqualTo("xml"));
        }

        [Test]
        public void ShouldPrioritizeFormatSelectionByAcceptTypeOrdering()
        {
            var map = new MediaTypeFormatMap();
            map.Map("text/xml", "xml");
            map.Map("text/html", "html");
            var router = new ContentNegotiationRouter(map);
            var route = new RouteData();

            router.AddFormat(route, new[] { "text/html", "text/xml" });

            Assert.That(route.Values["format"], Is.EqualTo("html"));
        }

        [Test]
        public void UnsupportedAcceptTypeMapsToDefaultFormat()
        {
            var map = new MediaTypeFormatMap();
            map.Map("text/xml", "xml");
            var router = new ContentNegotiationRouter(map);
            var route = new RouteData();

            router.AddFormat(route, new[] { "audio/*" });

            Assert.That(route.Values["format"], Is.EqualTo("xml"));
        }
    }
}
