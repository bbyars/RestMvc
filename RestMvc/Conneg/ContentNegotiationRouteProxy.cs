using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace RestMvc.Conneg
{
    /// <summary>
    /// Acts as a decorator to the standard RouteHandler, and adds content
    /// negotiation based on the prioritized associations that the application declares
    /// in the MediaTypeFormatMap passed in.
    /// </summary>
    public class ContentNegotiationRouteProxy : IRouteHandler
    {
        private readonly IRouteHandler proxiedHandler;
        private readonly MediaTypeFormatMap map;
        private readonly ConnegPriorityGivenTo priority;

        /// <summary>
        /// Creates a new ContentNegotiationRouteProxy.
        /// </summary>
        /// <param name="proxiedHandler">The handler to proxy, typically an MvcRouteHandler</param>
        /// <param name="map">The map containing the associations between media types and formats</param>
        /// <param name="priority">Whether the client sets the priority or the server.  The HTTP spec
        /// indicates that the client should set it, but on occasion the server may need to set it
        /// to work around bugs in browsers (e.g. Chrome prioritizes xml over html).</param>
        public ContentNegotiationRouteProxy(IRouteHandler proxiedHandler, MediaTypeFormatMap map,
            ConnegPriorityGivenTo priority = ConnegPriorityGivenTo.Client)
        {
            this.proxiedHandler = proxiedHandler;
            this.map = map;
            this.priority = priority;
        }

        public virtual IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            AddFormat(requestContext.RouteData, requestContext.HttpContext.Request.AcceptTypes);
            return proxiedHandler.GetHttpHandler(requestContext);
        }

        /// <summary>
        /// If the routing system hasn't already added a format to the route,
        /// add a format based on the first Accept header match in our map.
        /// </summary>
        /// <param name="route"></param>
        /// <param name="acceptTypes"></param>
        public virtual void AddFormat(RouteData route, string[] acceptTypes)
        {
            // Bypass content negotiation by appending an extension to the route.
            if (route.Values["format"] == null)
                route.Values["format"] = FormatFor(acceptTypes);
        }

        private string FormatFor(string[] acceptTypes)
        {
            if (!SupportsAcceptType(acceptTypes))
                return map.DefaultFormat;

            if (priority == ConnegPriorityGivenTo.Client)
                acceptTypes = new[] {acceptTypes.First(accept => map.SupportsMediaType(accept))};

            return map.FormatFor(acceptTypes);
        }

        private bool SupportsAcceptType(IEnumerable<string> acceptTypes)
        {
            return acceptTypes != null && acceptTypes.Any(accept => map.SupportsMediaType(accept));
        }
    }
}
