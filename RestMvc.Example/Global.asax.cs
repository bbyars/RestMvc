using System.Web;
using System.Web.Routing;

namespace RestMvc.Example
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.Map<EchoController>();
        }
    }
}
