using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RestMvc.Conneg;

namespace RestMvc.Example
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var map = new MediaTypeFormatMap();
            map.Add(MediaType.PlainText, "text");
            map.Add(MediaType.Xml, "xml");
            var connegHandler = new SimpleContentNegotiationRouteProxy(new MvcRouteHandler(), map);

            RouteTable.Routes.Map<EchoController>(connegHandler);
        }
    }
}
