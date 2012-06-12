using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using RestMvc.Attributes;

namespace RestMvc
{
    /// <summary>
    /// Reflects on the attributes in TController to discover the routes
    /// to be added.  Each TController can support multiple resource types.
    /// This is quite common - typically an Index (list) and Show (entity)
    /// are supported by the same controller, as are edit and create forms,
    /// all of which have different resource types defined by different
    /// URI templates.
    /// </summary>
    /// <typeparam name="TController">The type of controller to add routes for</typeparam>
    public class ResourceMapper<TController> where TController : Controller
    {
        private readonly RouteCollection routes;
        private readonly IRouteHandler routeHandler;

        public ResourceMapper(RouteCollection routes, IRouteHandler routeHandler)
        {
            this.routes = routes;
            this.routeHandler = routeHandler;
        }

        /// <summary>
        /// Maps all the routes provided by ResourceActionAttribute annotations.
        /// For GET requests, also maps an additional "formatted" route.
        /// e.g. [Get "test/{id}"] also maps test/{id}.{format}, which allows
        /// the resource to serve up multiple representations without relying
        /// on HTTP header content negotiation.
        /// </summary>
        public virtual void MapSupportedMethods()
        {
            foreach (var action in typeof(TController).GetResourceActions())
            {
                var attribute = action.GetResourceActionAttribute();
                foreach (var uri in attribute.ResourceUris)
                    Map(uri, Defaults(action.Name), attribute.HttpMethod);
            }
        }

        /// <summary>
        /// For every resource URI referenced in a ResourceActionAttribute,
        /// maps the HTTP methods _not_ supported at that URI to a method
        /// on the RestfulController that returns a 405 HTTP code.
        /// This does not include the HEAD, OPTIONS, or WebDAV methods.
        /// </summary>
        public virtual void MapUnsupportedMethods()
        {
            foreach (var resourceUri in typeof(TController).GetResourceUris())
            {
                foreach (var method in typeof(TController).GetUnsupportedMethods(resourceUri))
                    Map(resourceUri, Defaults("MethodNotSupported", resourceUri), method);
            }
        }

        /// <summary>
        /// For every resource URI referenced in a ResourceActionAttribute,
        /// maps the HEAD method to a RestfulController action that knows
        /// how to respond appropriately.
        /// For controllers that don't subclass RestfulController, this
        /// method will do nothing.
        /// </summary>
        public virtual void MapHead()
        {
            MapAllResources("Head");
        }

        /// <summary>
        /// For every resource URI referenced in a ResourceActionAttribute,
        /// maps the OPTIONS method to a RestfulController action that knows
        /// how to respond appropriately.  If your controller subclasses
        /// RestfulController, you can hook into the Options handling by
        /// overriding the Options method.
        /// </summary>
        public virtual void MapOptions()
        {
            MapAllResources("Options");
        }

        /// <summary>
        /// Maps all the routes provided by PutAttribute and DeleteAttribute annotations
        /// tunnelled through a POST routes, as long as the post data contains
        /// a key matching postDataKey, and a value of either PUT or DELETE.
        /// This is needed for browser support.
        /// </summary>
        public virtual void MapTunnelledMethods(string postDataKey = "_method")
        {
            var putsAndDeletes = typeof(TController).GetResourceActions()
                .Where(action => action.IsDefined(typeof(PutAttribute), true)
                    || action.IsDefined(typeof(DeleteAttribute), true));

            foreach (var action in putsAndDeletes)
            {
                var attribute = action.GetResourceActionAttribute();
                foreach (var uri in attribute.ResourceUris)
                    Map(uri, Defaults(action.Name), attribute.HttpMethod, postDataKey);
            }
        }

        private void MapAllResources(string method)
        {
            foreach (var resourceUri in typeof(TController).GetResourceUris())
                Map(resourceUri, Defaults(method, resourceUri), method.ToUpper());
        }

        private static bool IsRestfulController
        {
            get { return typeof(TController).IsSubclassOf(typeof(RestfulController)); }
        }

        private void Map(string urlFormat, RouteValueDictionary defaults, string httpMethod)
        {
            routes.Add(new Route(urlFormat, defaults,
                new RouteValueDictionary {{"httpMethod", new HttpMethodConstraint(httpMethod)}},
                new RouteValueDictionary { { "Namespaces", new[] { typeof(TController).Namespace} } },
                routeHandler));
        }

        private void Map(string urlFormat, RouteValueDictionary defaults, string httpMethod, string postDataKey)
        {
            routes.Add(new Route(urlFormat, defaults,
                new RouteValueDictionary {{"httpMethod", new HttpMethodConstraint("POST")},
                                          {"postData", new PostDataConstraint(postDataKey, httpMethod)}},
                new RouteValueDictionary { { "Namespaces", new[] { typeof(TController).Namespace } } },
                routeHandler));
        }

        private static RouteValueDictionary Defaults(string actionName)
        {
            return new RouteValueDictionary
            {
                {"controller", typeof(TController).GetControllerName()}, {"action", actionName}
            };
        }

        private static RouteValueDictionary Defaults(string actionName, string resourceUri)
        {
            var defaults = Defaults(actionName);
            defaults.Add("resourceUri", resourceUri);
            if (!IsRestfulController)
            {
                defaults["controller"] = typeof(RestfulController).GetControllerName();
                defaults["controllerType"] = typeof(TController);
            }
            return defaults;
        }
    }
}
