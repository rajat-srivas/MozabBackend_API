using Taskboard_API.CustomModules;
using DryIoc;
using DryIoc.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Taskboard_API
{
    public static class WebApiConfig
    {
         

        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            IContainer container = new Container()
                .WithWebApi(config, throwIfUnresolved: type => type.IsController());

            container.AddDependencies();
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
