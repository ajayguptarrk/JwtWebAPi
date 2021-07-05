using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;

namespace JwtWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            
            // Web API routes
            config.MapHttpAttributeRoutes();
        
            config.Routes.MapHttpRoute(
                name: "LoginApi",
                routeTemplate: "api/{controller}/{userName}/{password}",
                defaults: new { controller = "Login", userName = "", password = "" }  // Parameter defaults
            );

            config.Routes.MapHttpRoute(
              name: "DefaultApi",
              routeTemplate: "api/{controller}/{id}",
              defaults: new { id = RouteParameter.Optional }
          );

            JsonMediaTypeFormatter jsonFormatter = config.Formatters.JsonFormatter;

            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

        }
    }
}
