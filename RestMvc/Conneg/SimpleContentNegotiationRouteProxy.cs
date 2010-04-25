using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace RestMvc.Conneg
{
    /// <summary>
    /// Acts as a decorator to the standard RouteHandler, and adds simplistic content
    /// negotiation based on the prioritized associations that the application declares
    /// in the MediaTypeFormatMap passed in.  This is simplistic for at least two reasons:
    /// 1) It ignores the client prioritization
    /// 2) It ignores the client quality (q) parameters
    /// But it is likely appropriate for a number of services.  A more robust negotiation
    /// scheme can be a separate decorator, allowing the developer to choose.
    /// </summary>
    public class SimpleContentNegotiationRouteProxy : IRouteHandler
    {
        private readonly IRouteHandler proxiedHandler;
        private readonly MediaTypeFormatMap map;

        public SimpleContentNegotiationRouteProxy(IRouteHandler proxiedHandler, MediaTypeFormatMap map)
        {
            this.proxiedHandler = proxiedHandler;
            this.map = map;
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
            if (route.Values["format"] != null)
                return;

            route.Values["format"] = FormatFor(acceptTypes);
        }

        private string FormatFor(ICollection<string> acceptTypes)
        {
            if (acceptTypes == null || acceptTypes.Count == 0)
                return map.DefaultFormat;

            var acceptType = acceptTypes.FirstOrDefault(accept => map.SupportsMediaType(accept));
            return acceptType == null ? map.DefaultFormat : map.FormatFor(acceptType);
        }
    }
}