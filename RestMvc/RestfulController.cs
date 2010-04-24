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

        public virtual void MethodNotSupported(string resourceUri)
        {
            Response.Headers["Allow"] = string.Join(", ", GetType().GetSupportedMethods(resourceUri));
            throw new HttpException(405, "Method Not Supported");
        }
    }
}
