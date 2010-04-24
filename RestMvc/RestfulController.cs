using System.Web;
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
            var result = (ActionResult)action.Invoke(this, new object[0]);
            result.ExecuteResult(ControllerContext);

            var response = ControllerContext.HttpContext.Response;
            response.Headers["Content-Length"] = response.Output.ToString().Length.ToString();
            response.ClearContent();
        }

        public virtual void MethodNotSupported(string resourceUri)
        {
            SetAllowHeader(resourceUri);
            throw new HttpException(405, "Method Not Supported");
        }

        private void SetAllowHeader(string resourceUri)
        {
            Response.Headers["Allow"] = string.Join(", ", GetType().GetSupportedMethods(resourceUri));
        }
    }
}
