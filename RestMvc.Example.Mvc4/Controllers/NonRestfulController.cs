using System.Web.Mvc;
using RestMvc.Attributes;

namespace RestMvc.Example
{
    public class NonRestfulController : Controller
    {
        [Get("/nonRestful")]
        public ActionResult Test()
        {
            return new ContentResult {Content = "OK", ContentType = "text/plain"};
        }
    }
}
