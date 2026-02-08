using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace NeorisBackend.Logging
{
    /// <summary>
    /// Filtro que valida automáticamente el ModelState antes de ejecutar la acción
    /// </summary>
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        /// <summary>
        /// Valida el ModelState antes de ejecutar la acción
        /// </summary>
        /// <param name="actionContext">Contexto de la acción a ejecutar</param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var errors = actionContext.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => new
                    {
                        Field = e.Key,
                        Errors = e.Value.Errors.Select(x => x.ErrorMessage).ToArray()
                    }).ToArray();

                actionContext.Response = actionContext.Request.CreateResponse(
                    HttpStatusCode.BadRequest,
                    new
                    {
                        Message = "Errores de validación",
                        Errors = errors
                    });
            }

            base.OnActionExecuting(actionContext);
        }
    }
}
