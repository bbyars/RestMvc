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
            var type = (Type)RouteData.Values["controllerType"] ?? GetType();
            SetAllowHeader(resourceUri, type);
            return new EmptyResult();
        }

        public virtual void Head(string resourceUri)
        {
            var action = GetType().GetAction("GET", resourceUri);
//            ActionInvoker.InvokeAction(ControllerContext, action.Name);
            var result = (ActionResult)action.Invoke(this, new object[0]);
            result.ExecuteResult(ControllerContext);

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

        protected virtual void SetAllowHeader(string resourceUri)
        {
            SetAllowHeader(resourceUri, GetType());
        }

        private void SetAllowHeader(string resourceUri, Type type)
        {
            Response.Headers["Allow"] = string.Join(", ", type.GetSupportedMethods(resourceUri));
        }
    }
}
