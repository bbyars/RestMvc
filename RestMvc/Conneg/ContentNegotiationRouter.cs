using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace RestMvc.Conneg
{
    /// <summary>
    /// Since several resources have multiple representations, we use
    /// a primitive content negotiation ("conneg") scheme to determine
    /// which representation the user wants -- we use the first media type in
    /// the Accept header that we match, and translate it into a format
    /// value in the routing subsystem.
    /// 
    /// Because setting HTTP headers can be awkward for browser testing,
    /// the routing subsystem allows you to bypass conneg by adding
    /// an extension to the end of the URL to specify the format
    /// (e.g. /orders.xml, orders.html)
    /// </summary>
    public class ContentNegotiationRouter
    {
        private readonly MediaTypeFormatMap map;

        public ContentNegotiationRouter(MediaTypeFormatMap map)
        {
            this.map = map;
        }

        /// <summary>
        /// If the routing system hasn't already added a format to the route,
        /// add a format based on the first Accept header match in our map.
        /// </summary>
        /// <param name="route"></param>
        /// <param name="acceptTypes"></param>
        public void AddFormat(RouteData route, string[] acceptTypes)
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
