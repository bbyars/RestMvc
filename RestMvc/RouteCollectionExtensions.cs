using System.Web.Routing;

namespace RestMvc
{
    public static class RouteCollectionExtensions
    {
        public static void Map<TController>(this RouteCollection routes) where TController : RestfulController
        {
            var mapper = new ResourceMapper<TController>();
            mapper.MapSupportedMethods(routes);
            mapper.MapUnsupportedMethods(routes);
            mapper.MapHead(routes);
            mapper.MapOptions(routes);
        }
    }
}
