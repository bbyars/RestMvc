using System;
using System.Web.Mvc;

namespace RestMvc
{
    /// <summary>
    /// Provides support for automatically handling OPTIONS and HEAD requests.
    /// </summary>
    public class RestfulController : Controller
    {
        public const string MethodNotSupportedAction = "MethodNotSupported";
        public const string HeadAction = "Head";
        public const string OptionsAction = "Options";

        public virtual ActionResult Options(string resourceUri)
        {
            SetAllowHeader(resourceUri);
            return new EmptyResult();
        }

        public virtual void Head(string resourceUri)
        {
            var action = GetType().GetAction("GET", resourceUri);
            ActionInvoker.InvokeAction(ControllerContext, action.Name);

            Response.Headers["Content-Length"] = Response.Output.ToString().Length.ToString();
            Response.SuppressContent = true;
            Response.End();
        }

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
