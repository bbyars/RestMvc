using System.Web.Mvc;

namespace RestMvc
{
    public class Representation : ViewResult
    {
        public Representation(object model)
            : this(model, new ViewDataDictionary())
        {
        }

        public Representation(object model, ViewDataDictionary viewData)
        {
            ViewData = viewData;
            ViewData.Model = model;
        }


        protected override ViewEngineResult FindView(ControllerContext context)
        {
            ViewName = GetViewName(context);
            return base.FindView(context);
        }

        public static string GetViewName(ControllerContext context)
        {
            var format = context.RouteData.GetRequiredString("format");
            return string.Format("{0}.{1}", context.RouteData.GetRequiredString("action"), format);
        }
    }
}
