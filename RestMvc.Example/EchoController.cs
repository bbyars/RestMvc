using System.Web.Mvc;
using RestMvc.Attributes;
using RestMvc.Conneg;

namespace RestMvc.Example
{
    public class EchoController : RestfulController
    {
        [Get("echo/{thingToEcho}.{format}", "echo/{thingToEcho}")]
        public ActionResult Echo(string thingToEcho, string format)
        {
            if (format == "xml")
                return new ContentResult
                {
                    Content = string.Format("<echo>{0}</echo>", thingToEcho),
                    ContentType = MediaType.Xml
                };

            return new ContentResult
            {
                Content = thingToEcho,
                ContentType = MediaType.PlainText
            };
        }

        public override ActionResult Options(string resourceUri)
        {
            SetAllowHeader(resourceUri);
            return new ContentResult {Content = "Options body goes here..."};
        }
    }
}
