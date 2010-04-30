using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

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
    public class ResourceMapper<TController> where TController : ControllerBase
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
                    Map(routes, uri, Defaults(action.Name), attribute.HttpMethod);
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
            // We can handle this even without subclassing RestfulController
            var controllerType = IsRestfulController ? typeof(TController) : typeof(RestfulController);
            foreach (var resourceUri in typeof(TController).GetResourceUris())
            {
                var defaults = Defaults(controllerType, RestfulController.MethodNotSupportedAction, resourceUri);
                foreach (var method in typeof(TController).GetUnsupportedMethods(resourceUri))
                    Map(routes, resourceUri, defaults, method);
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
            if (!IsRestfulController)
                return;

            MapAllResources(routes, RestfulController.HeadAction, "HEAD");
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
            foreach (var resourceUri in typeof(TController).GetResourceUris())
            {
                var defaults = Defaults(RestfulController.OptionsAction, resourceUri);
                if (!IsRestfulController)
                {
                    defaults["controller"] = typeof(RestfulController).GetControllerName();
                    defaults["controllerType"] = typeof(TController);
                }
                Map(routes, resourceUri, defaults, "OPTIONS");
            }
        }

        private static bool IsRestfulController
        {
            get { return typeof(TController).IsSubclassOf(typeof(RestfulController)); }
        }

        private void MapAllResources(ICollection<RouteBase> routes, string actionName, string httpMethod)
        {
            foreach (var resourceUri in typeof(TController).GetResourceUris())
                Map(routes, resourceUri, Defaults(actionName, resourceUri), httpMethod);
        }

        private void Map(ICollection<RouteBase> routes, string urlFormat,
            RouteValueDictionary defaults, string httpMethod)
        {
            routes.Add(new Route(urlFormat, defaults,
                new RouteValueDictionary {{"httpMethod", new HttpMethodConstraint(httpMethod)}},
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
            var result = Defaults(actionName);
            result.Add("resourceUri", resourceUri);
            return result;
        }

        private static RouteValueDictionary Defaults(Type controller, string actionName, string resourceUri)
        {
            var result = Defaults(actionName, resourceUri);
            result["controller"] = controller.GetControllerName();
            return result;
        }
    }
}
