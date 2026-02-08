using Microsoft.Web.Http;
using Microsoft.Web.Http.Routing;
using Microsoft.Web.Http.Versioning;
using NeorisBackend.Logging;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

namespace NeorisBackend
{
    /// <summary>
    /// Provee la configuración de Web API para la aplicación, incluyendo el enrutamiento, el versionado de la API, la habilitación de CORS, la configuración de formateadores y la adición de filtros globales. Esta clase es fundamental para establecer cómo se manejarán las solicitudes HTTP entrantes y cómo se estructurán las rutas de la API, asegurando que la aplicación sea escalable, mantenible y fácil de consumir por los clientes.
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Configura la aplicación Web API. Este método se llama desde el método de inicio de la aplicación para establecer las rutas, filtros, formateadores y otras configuraciones necesarias para que la API funcione correctamente.
        /// </summary>
        /// <param name="config">Configuración de la aplicación Web API.</param>
        public static void Register(HttpConfiguration config)
        {
            // Habilitar CORS
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            // Versionado de API 
            config.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            // Registrar la restricción apiVersion en el resolver
            var constraintResolver = new DefaultInlineConstraintResolver()
            {
                ConstraintMap =
        {
            ["apiVersion"] = typeof(ApiVersionRouteConstraint)
        }
            };
            config.MapHttpAttributeRoutes(constraintResolver);

            // Logging de requests
            config.MessageHandlers.Add(new RequestLoggingHandler());

            // Filtros globales
            config.Filters.Add(new GlobalExceptionFilter());
            config.Filters.Add(new ValidateModelStateFilter());

            // Rutas de Web API
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v{version:apiVersion}/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { version = new ApiVersionRouteConstraint() }
            );

            // Configurar JSON como formato por defecto
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
        }
    }
}
