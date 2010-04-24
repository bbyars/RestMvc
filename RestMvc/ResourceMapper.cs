using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace RestMvc
{
    public class ResourceMapper<TController> where TController : RestfulController
    {
        public virtual string ControllerName
        {
            get { return typeof(TController).Name.Replace("Controller", ""); }
        }

        public virtual string[] ResourceUris
        {
            get
            {
                return typeof(TController).GetResourceActions()
                    .Select(action => action.GetResourceActionAttribute().ResourceUri)
                    .Distinct(StringComparer.InvariantCultureIgnoreCase).ToArray();
            }
        }

        public virtual string[] SupportedMethods(string resourceUri)
        {
            return typeof(TController).GetResourceActions()
                .Select(action => action.GetResourceActionAttribute())
                .Where(attribute => string.Equals(resourceUri, attribute.ResourceUri, StringComparison.InvariantCultureIgnoreCase))
                .Select(attribute => attribute.HttpMethod).ToArray();
        }

        public virtual string[] UnsupportedMethods(string resourceUri)
        {
            var supportedMethods = SupportedMethods(resourceUri);
            return new[] {"GET", "POST", "PUT", "DELETE"}
                .Where(method => !supportedMethods.Contains(method)).ToArray();
        }

        public virtual void MapSupportedMethods(ICollection<RouteBase> routes)
        {
            foreach (var action in typeof(TController).GetResourceActions())
            {
                var attribute = action.GetResourceActionAttribute();
                Map(routes, attribute.ResourceUri, Defaults(action.Name), attribute.HttpMethod);
                if (attribute.HttpMethod == "GET")
                    Map(routes, attribute.ResourceUri + ".{format}", Defaults(action.Name), attribute.HttpMethod);
            }
        }

        public virtual void MapUnsupportedMethods(RouteCollection routes)
        {
            foreach (var resourceUri in ResourceUris)
            {
                foreach (var method in UnsupportedMethods(resourceUri))
                    Map(routes, resourceUri, Defaults(RestfulController.MethodNotSupported), method);

            }
        }

        public virtual void MapHead(RouteCollection routes)
        {
            MapAllResources(routes, RestfulController.Head, "HEAD");
        }

        public virtual void MapOptions(RouteCollection routes)
        {
            MapAllResources(routes, RestfulController.Options, "OPTIONS");
        }

        private void MapAllResources(ICollection<RouteBase> routes, string actionName, string httpMethod)
        {
            foreach (var resourceUri in ResourceUris)
            {
                var defaults = Defaults(actionName);
                defaults.Add("resourceUri", resourceUri);
                Map(routes, resourceUri, defaults, httpMethod);
            }
        }

        private static void Map(ICollection<RouteBase> routes, string urlFormat, RouteValueDictionary defaults, string httpMethod)
        {
            routes.Add(new Route(urlFormat,
                defaults,
                new RouteValueDictionary {{"httpMethod", new HttpMethodConstraint(httpMethod)}},
                new MvcRouteHandler()));
        }

        private RouteValueDictionary Defaults(string actionName)
        {
            return new RouteValueDictionary {{"controller", ControllerName}, {"action", actionName}};
        }
    }
}