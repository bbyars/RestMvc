using System;
using System.Web.Mvc;

namespace RestMvc
{
    /// <summary>
    /// Provides support for automatically handling OPTIONS and HEAD requests.
    /// You can use RestMvc without subclassing RestfulController.  The only
    /// reason you might want to subclass is because it gives you an opportunity
    /// to override the handling of HEAD and OPTIONS requests.
    /// </summary>
    public class RestfulController : Controller
    {
        /// <summary>
        /// The action called on any HTTP OPTIONS request.
        /// </summary>
        /// <param name="resourceUri">The URI template</param>
        /// <returns>An empty result, with the Allow header set</returns>
        public virtual ActionResult Options(string resourceUri)
        {
            SetAllowHeader(resourceUri);
            return new EmptyResult();
        }

        /// <summary>
        /// The action called on any HTTP HEAD request.
        /// </summary>
        /// <param name="resourceUri">The URI template</param>
        public virtual void Head(string resourceUri)
        {
            var action = GetType().GetAction("GET", resourceUri);
            ActionInvoker.InvokeAction(ControllerContext, action.Name);

            Response.Headers["Content-Length"] = Response.Output.ToString().Length.ToString();
            Response.SuppressContent = true;
            Response.End();
        }

        /// <summary>
        /// The method called when an HTTP method was called for a resource
        /// that does not support that method.
        /// </summary>
        /// <param name="resourceUri">The URI template</param>
        public virtual void MethodNotSupported(string resourceUri)
        {
            SetAllowHeader(resourceUri);
            Response.StatusCode = 405;
            Response.SuppressContent = true;
            Response.End();
        }

        private void SetAllowHeader(string resourceUri)
        {
            var type = (Type)RouteData.Values["controllerType"] ?? GetType();
            Response.Headers["Allow"] = string.Join(", ", type.GetSupportedMethods(resourceUri));
        }
    }
}
