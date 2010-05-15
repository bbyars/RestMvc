using System.Web;
using System.Web.Routing;
using Moq;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using RestMvc.Conneg;

namespace RestMvc.UnitTests.Conneg
{
    [TestFixture]
    public class ContentNegotiationRouteProxyTest
    {
        [Test]
        public void ShouldMapMediaTypeToFormat()
        {
            var map = new MediaTypeFormatMap();
            map.Add("application/xml", "xml");
            var router = new ContentNegotiationRouteProxy(null, map);
            var route = new RouteData();

            router.AddFormat(route, new[] {"*/*"});

            Assert.That(route.Values["format"], Is.EqualTo("xml"));
        }

        [Test]
        public void ShouldNotSetFormatIfRoutingSystemAlreadyDetectedIt()
        {
            var map = new MediaTypeFormatMap();
            map.Add("application/xml", "xml");
            var router = new ContentNegotiationRouteProxy(null, map);
            var route = new RouteData();
            route.Values["format"] = "html";

            router.AddFormat(route, new[] {"*/*"});

            Assert.That(route.Values["format"], Is.EqualTo("html"));
        }

        [Test]
        public void ShouldUseDefaultFormatIfNoAcceptTypesProvided()
        {
            var map = new MediaTypeFormatMap();
            map.Add("application/xml", "xml");
            var router = new ContentNegotiationRouteProxy(null, map);
            var route = new RouteData();

            router.AddFormat(route, new string[0]);

            Assert.That(route.Values["format"], Is.EqualTo("xml"));
        }

        [Test]
        public void ShouldUseDefaultFormatIfNullAcceptTypesProvided()
        {
            var map = new MediaTypeFormatMap();
            map.Add("application/xml", "xml");
            var router = new ContentNegotiationRouteProxy(null, map);
            var route = new RouteData();

            router.AddFormat(route, null);

            Assert.That(route.Values["format"], Is.EqualTo("xml"));
        }

        [Test]
        public void ShouldPrioritizeFormatSelectionByAcceptTypeOrderingByDefault()
        {
            var map = new MediaTypeFormatMap();
            map.Add("application/xml", "xml");
            map.Add("text/html", "html");
            var router = new ContentNegotiationRouteProxy(null, map);
            var route = new RouteData();

            router.AddFormat(route, new[] {"text/html", "application/xml"});

            Assert.That(route.Values["format"], Is.EqualTo("html"));
        }

        [Test]
        public void ShouldIgnoreUnsupportedMediaTypes()
        {
            var map = new MediaTypeFormatMap();
            map.Add("text/plain", "text");
            map.Add("application/xml", "xml");
            var router = new ContentNegotiationRouteProxy(null, map);
            var route = new RouteData();

            router.AddFormat(route, new[] {"text/html", "application/xml"});

            Assert.That(route.Values["format"], Is.EqualTo("xml"));
        }

        [Test]
        public void ShouldPrioritizeFormatSelectionByMapEntriesIfAskedTo()
        {
            var map = new MediaTypeFormatMap();
            map.Add("text/html", "html");
            map.Add("application/xml", "xml");
            var router = new ContentNegotiationRouteProxy(null, map, ConnegPriorityGivenTo.Server);
            var route = new RouteData();

            router.AddFormat(route, new[] {"application/xml", "text/html"});

            Assert.That(route.Values["format"], Is.EqualTo("html"));
        }

        [Test]
        public void UnsupportedAcceptTypeMapsToDefaultFormat()
        {
            var map = new MediaTypeFormatMap();
            map.Add("application/xml", "xml");
            var router = new ContentNegotiationRouteProxy(null, map);
            var route = new RouteData();

            router.AddFormat(route, new[] {"audio/*"});

            Assert.That(route.Values["format"], Is.EqualTo("xml"));
        }

        [Test]
        public void GetHandlerProxiesToPassedInHandler()
        {
            var proxiedHandler = new Mock<IRouteHandler>();
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(ctx => ctx.Request.AcceptTypes).Returns(new string[0]);
            var request = new RequestContext(httpContext.Object, new RouteData());
            var router = new ContentNegotiationRouteProxy(proxiedHandler.Object, new MediaTypeFormatMap());

            router.GetHttpHandler(request);

            proxiedHandler.Verify(h => h.GetHttpHandler(request));
        }
    }
}
