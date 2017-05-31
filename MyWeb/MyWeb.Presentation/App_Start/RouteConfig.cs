﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyWeb.Presentation
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Search",
                url: "search/{key}",
                defaults: new { controller = "Home", action = "Search", key = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "ListCategory",
                url: "the-loai/{slug}",
                defaults: new { controller = "Home", action = "ListCategory" }
            );

            routes.MapRoute(
                name: "Detail",
                url: "{slug}",
                defaults: new { controller = "Home", action = "Detail" }
            );

            routes.MapRoute(
                name: "Login",
                url: "Account/Login",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                namespaces: new string[] { "MyWeb.Presentation.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
