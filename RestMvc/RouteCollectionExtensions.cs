using System.Web.Mvc;
using System.Web.Routing;

namespace RestMvc
{
    public static class RouteCollectionExtensions
    {
        public static void Map<TController>(this RouteCollection routes) where TController : RestfulController
        {
            routes.Map<TController>(new MvcRouteHandler());
        }

        /// <summary>
        /// Maps all routes on TController annotated with a ResourceActionAttribute.
        /// OPTIONS and HEAD methods for each URI will be routed to a method on
        /// RestfulController that knows how to respond appropriately.
        /// For each URI provided, unsupported methods will be routed to a RestfulController
        /// method that returns a 405 status code.
        /// </summary>
        public static void Map<TController>(this RouteCollection routes, IRouteHandler routeHandler)
            where TController : RestfulController
        {
            var mapper = new ResourceMapper<TController>(routeHandler);
            mapper.MapSupportedMethods(routes);
            mapper.MapUnsupportedMethods(routes);
            mapper.MapHead(routes);
            mapper.MapOptions(routes);
        }
    }
}
