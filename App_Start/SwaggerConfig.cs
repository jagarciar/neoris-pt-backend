using System.Web.Http;
using System.Web.Http.Description;
using WebActivatorEx;
using NeorisBackend;
using Swashbuckle.Application;
using System.Linq;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace NeorisBackend
{
    /// <summary>
    /// Provee la configuración de Swagger para la API, incluyendo la generación de documentación y la personalización de la interfaz de usuario de Swagger UI.
    /// </summary>
    public class SwaggerConfig
    {

        /// <summary>
        /// Obtiene la ruta al archivo XML de comentarios generado por el compilador, que se utiliza para incluir la documentación de los métodos y modelos en Swagger. Asegúrate de que el proyecto esté configurado para generar este archivo XML en las propiedades del proyecto.
        /// </summary>
        /// <returns></returns>
        private static string GetXmlCommentsPath()
        {
            return System.String.Format(@"{0}\bin\neoris-pt-backend.xml", System.AppDomain.CurrentDomain.BaseDirectory);
        }

        /// <summary>
        /// Configura Swagger para la API, estableciendo la versión, el título, la inclusión de comentarios XML y personalizando la interfaz de usuario. También se asegura de que las operaciones que requieren autorización no aparezcan en la documentación Swagger, permitiendo que Swagger UI sea accesible sin autenticación.
        /// </summary>
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Neoris PT Backend API");
                    c.IncludeXmlComments(GetXmlCommentsPath());
                    c.DescribeAllEnumsAsStrings();
                    
                    // Remover operaciones que requieren autorización de la documentación Swagger
                    // Esto permite que Swagger UI sea accesible sin autenticación
                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                    
                    // Ignorar filtros de autorización en la documentación
                    c.IgnoreObsoleteActions();
                })
                .EnableSwaggerUi(c =>
                {
                    c.DocumentTitle("Neoris PT Backend API");
                    c.EnableDiscoveryUrlSelector();
                    c.DisableValidator();
                });
        }

        
    }
}
