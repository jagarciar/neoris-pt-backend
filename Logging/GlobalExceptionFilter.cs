using System.Web.Http.Filters;
using Serilog;

namespace NeorisBackend.Logging
{
    /// <summary>
    /// Filtro global de excepciones para capturar y registrar errores no manejados en la API.
    /// </summary>
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// Captura y registra las excepciones no manejadas.
        /// </summary>
        /// <param name="context">Contexto de la acción ejecutada.</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            Log.Error(context.Exception, "Unhandled exception in {Controller}/{Action}",
                context.ActionContext?.ControllerContext?.ControllerDescriptor?.ControllerName,
                context.ActionContext?.ActionDescriptor?.ActionName);
        }
    }
}
