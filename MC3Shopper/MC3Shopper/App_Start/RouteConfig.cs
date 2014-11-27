﻿using System.Web.Mvc;
using System.Web.Routing;

namespace MC3Shopper
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapMvcAttributeRoutes();

            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
            /**
            routes.MapRoute(
               name: "Produit",
               url: "{controller}/{action}/{Stat02}/{page}"

               );**/
        }
    }
}