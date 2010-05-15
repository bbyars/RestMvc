using System;
using System.Web;
using System.Web.Routing;

namespace RestMvc
{
    public class PostDataConstraint : IRouteConstraint
    {
        private readonly string postDataKey;
        private readonly string postDataValue;

        public PostDataConstraint(string postDataKey, string postDataValue)
        {
            this.postDataKey = postDataKey;
            this.postDataValue = postDataValue;
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName,
            RouteValueDictionary values, RouteDirection routeDirection)
        {
            return httpContext.Request.Form[postDataKey] == postDataValue;
        }
    }
}