using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace RestMvc
{
    public static class RouteCollectionExtensions
    {
        /// <summary>
        /// Maps all routes on all non-abstract Controller subclasses in the given assembly
        /// annotated with a ResourceActionAttribute.  For each distinct URI template,
        /// OPTIONS and HEAD will automatically be handled, and unsupported methods
        /// for the given URI will be routed to an action that returns a 405 status code.
        /// </summary>
        public static void MapAssembly(this RouteCollection routes, Assembly assembly)
        {
            routes.MapAssembly(assembly, new MvcRouteHandler());
        }

        /// <summary>
        /// Maps all routes on all non-abstract Controller subclasses in the given assembly
        /// annotated with a ResourceActionAttribute.  For each distinct URI template,
        /// OPTIONS and HEAD will automatically be handled, and unsupported methods
        /// for the given URI will be routed to an action that returns a 405 status code.
        /// The provided routeHandler will be used for all routes.
        /// </summary>
        public static void MapAssembly(this RouteCollection routes, Assembly assembly, IRouteHandler routeHandler)
        {
            var method = typeof(RouteCollectionExtensions).GetMethods()
                .First(m => m.Name == "Map" && m.GetParameters().Length == 2);
            var mapMethods = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(Controller)) && !type.IsAbstract)
                .Select(controllerType => method.MakeGenericMethod(controllerType));

            foreach (var mapMethod in mapMethods)
                mapMethod.Invoke(null, new object[] {routes, routeHandler});
        }

        /// <summary>
        /// Maps all routes on TController annotated with a ResourceActionAttribute.
        /// OPTIONS and HEAD methods for each URI will be routed to a method on
        /// RestfulController that knows how to respond appropriately.
        /// For each URI provided, unsupported methods will be routed to a RestfulController
        /// method that returns a 405 status code.
        /// </summary>
        public static void Map<TController>(this RouteCollection routes) where TController : Controller
        {
            routes.Map<TController>(new MvcRouteHandler());
        }

        /// <summary>
        /// Maps all routes on TController annotated with a ResourceActionAttribute.
        /// OPTIONS and HEAD methods for each URI will be routed to a method on
        /// RestfulController that knows how to respond appropriately.
        /// For each URI provided, unsupported methods will be routed to a RestfulController
        /// method that returns a 405 status code.  The provided routeHandler will be used.
        /// </summary>
        public static void Map<TController>(this RouteCollection routes, IRouteHandler routeHandler)
            where TController : Controller
        {
            var mapper = new ResourceMapper<TController>(routes, routeHandler);
            mapper.MapSupportedMethods();
            mapper.MapUnsupportedMethods();
            mapper.MapHead();
            mapper.MapOptions();
        }
    }
}
