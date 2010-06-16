using System;
using System.Reflection;
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
        /// <returns>An empty result, with the Allow header set.  Override in a subclass
        /// and call SetAllowHeader to return content.</returns>
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
            var action = GetControllerType().GetAction("GET", resourceUri);
            RouteData.Values["action"] = action.Name;
            Response.Headers["Content-Length"] = GetResourceOutput(action).Length.ToString();
            Response.Headers["Content-Type"] = string.Format("{0}; charset={1}", Response.ContentType, Response.Charset);
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

        /// <summary>
        /// Sets the Allow header to all supported methods for the resourceUri.
        /// </summary>
        /// <param name="resourceUri">The URI template</param>
        protected virtual void SetAllowHeader(string resourceUri)
        {
            Response.Headers["Allow"] = string.Join(", ", GetControllerType().GetSupportedMethods(resourceUri));
        }

        private Type GetControllerType()
        {
            return (Type)RouteData.Values["controllerType"] ?? GetType();
        }

        private string GetResourceOutput(MethodInfo action)
        {
            var controller = GetController();

            using (var proxy = new HttpContextWithReadableOutputStream(controller))
            {
                controller.ActionInvoker.InvokeAction(controller.ControllerContext, action.Name);
                return proxy.GetResponseText();
            }
        }

        private Controller GetController()
        {
            if (GetType().Equals(GetControllerType()))
                return this;

            var factory = ControllerBuilder.Current.GetControllerFactory();
            var controllerName = GetControllerType().Name.Replace("Controller", "");
            var controller = (Controller)factory.CreateController(ControllerContext.RequestContext, controllerName);
            controller.ControllerContext = new ControllerContext(ControllerContext.HttpContext, RouteData, controller);
            return controller;
        }
    }
}
