using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;

namespace DormWebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            /*   var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
               json.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
               json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
               json.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
               json.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
               json.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
               json.SerializerSettings.Culture = new CultureInfo("it-IT");
               */
            config.EnableCors();
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.MediaTypeMappings.Add(new System.Net.Http.Formatting.QueryStringMapping("format", "json", "application/json"));
            // Web API routes
            config.MapHttpAttributeRoutes();
          
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
