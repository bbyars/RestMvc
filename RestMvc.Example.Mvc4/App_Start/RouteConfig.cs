﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RestMvc.Conneg;

namespace RestMvc.Example.Mvc4
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            // Routes are generated by RestMvc using reflection to inspect controller attributes.
            // Other routes could also be added here manually.

            var map = new MediaTypeFormatMap();
            map.Add(MediaType.PlainText, "text");
            map.Add(MediaType.Xml, "xml");
            var connegHandler = new ContentNegotiationRouteProxy(new MvcRouteHandler(), map);
            routes.MapAssembly(Assembly.GetExecutingAssembly(), connegHandler);
        }
    }
}