using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace SIM
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            configuration.EnableCors();
            //configuration.Routes.MapHttpRoute("API Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });

            configuration.Routes.MapHttpRoute(
                "API Default Actions",
                "api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional }
            );

            configuration.Routes.MapHttpRoute(
                "API Default", 
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );

            // Establece json como el formato de intercambio de información
            configuration.Formatters.Clear();
            configuration.Formatters.Add(new JsonMediaTypeFormatter());

            // Para evitar el error de referencia circular (la ignora)
            configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Para evitar el error de referencia circular (la evita - no la he probado)
            //configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
            //configuration.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
        }
    }
}