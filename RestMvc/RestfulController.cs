using System.Web.Mvc;

namespace RestMvc
{
    /// <summary>
    /// Provides support for automatically handling OPTIONS and HEAD requests.
    /// </summary>
    public class RestfulController : Controller
    {
        public const string MethodNotSupported = "MethodNotSupported";
        public const string Head = "Head";
        public const string Options = "Options";
    }
}
