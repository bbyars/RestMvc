using System.Web;
using System.Web.Routing;

namespace RestMvc
{
    /// <summary>
    /// A routing constraint that verifies one of the form elements
    /// in an HTTP post matches a given value.
    /// </summary>
    public class PostDataConstraint : IRouteConstraint
    {
        private readonly string postDataKey;
        private readonly string postDataValue;

        /// <summary>
        /// Creates a new PostDataConstraint
        /// </summary>
        /// <param name="postDataKey">The key to look for in Request.Form</param>
        /// <param name="postDataValue">The value that must match in Request.Form[postDataKey]</param>
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
