using System.Web.Mvc;
using RestMvc.Attributes;

namespace RestMvc.Example
{
    public class EchoController : RestfulController
    {
        [Get("echo/{thingToEcho}")]
        public ActionResult Echo(string thingToEcho)
        {
            return new ContentResult {Content = thingToEcho};
        }
    }
}
