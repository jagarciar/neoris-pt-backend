using System;
using System.Web.Http;
using NeorisBackend.DTOs.Requests;
using NeorisBackend.Services.Interfaces;
using Microsoft.Web.Http;
using NeorisBackend.DTOs.Requests.Autor;

namespace NeorisBackend.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con los autores, incluyendo la creaci�n, actualizaci�n, eliminaci�n y obtenci�n de informaci�n de los autores. Este controlador utiliza el servicio de autor para realizar las operaciones de negocio y est� protegido por autenticaci�n para garantizar que solo los usuarios autorizados puedan acceder a sus endpoints.
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/autores")]
    [Authorize]
    public class AutoresController : ApiController
    {
        /// <summary>
        /// Referencia al servicio de autor, que maneja la l�gica de negocio relacionada con las operaciones CRUD de los autores. Este servicio es inyectado a trav�s del constructor para promover un dise�o modular y facilitar las pruebas unitarias.
        /// </summary>
        private readonly IAutorService _autorService;

        /// <summary>
        /// Permite eliminar un autor espec�fico utilizando su ID. Este endpoint maneja cualquier excepci�n que pueda ocurrir durante la eliminaci�n, devolviendo un error interno del servidor con un mensaje descriptivo en caso de fallo. Si el autor no existe, devuelve un error 404 Not Found. Si la eliminaci�n es exitosa, devuelve un mensaje de confirmaci�n.
        /// </summary>
        /// <param name="id">Identificador �nico del autor a eliminar</param>
        /// <returns>200 /Ok con mensaje de confirmaci�n o 404 si no se encuentra el autor</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteAutor(int id)
        {
            try
            {
                var result = _autorService.Delete(id);

                if (!result)
                {
                    return NotFound();
                }

                return Ok(new { message = $"Autor {id} eliminado exitosamente" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error al eliminar autor: {ex.Message}"));
            }
        }

        /// <summary>
        /// Constructor del controlador de autores, que recibe una instancia de IAutorService a trav�s de inyecci�n de dependencias. Si el servicio es nulo, se lanza una excepci�n ArgumentNullException para garantizar que el controlador siempre tenga una instancia v�lida del servicio de autor.
        /// </summary>
        /// <param name="autorService">Servicio de autor que contiene la l�gica de negocio para las operaciones CRUD de autores.</param>
        /// <exception cref="ArgumentNullException">Genera excepci�n si el servicio de autor es nulo.</exception>
        public AutoresController(IAutorService autorService)
        {
            _autorService = autorService ?? throw new ArgumentNullException(nameof(autorService));
        }

        /// <summary>
        /// Permite obtener la informaci�n de un autor espec�fico utilizando su ID. Este endpoint maneja cualquier excepci�n que pueda ocurrir durante la obtenci�n de los datos, devolviendo un error interno del servidor con un mensaje descriptivo en caso de fallo. Si el autor no existe, devuelve un error 404 Not Found.
        /// </summary>
        /// <param name="id">Identificcador �nico del autor a obtener</param>
        /// <returns>Autor espec�fico basado en su ID.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetAutor(int id)
        {
            try
            {
                var autor = _autorService.GetById(id);

                if (autor == null)
                {
                    return NotFound();
                }

                return Ok(autor);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error al obtener autor: {ex.Message}"));
            }
        }

        /// <summary>
        /// Obtiene todos los autores disponibles en el sistema. Este endpoint devuelve una lista de autores, y maneja cualquier excepci�n que pueda ocurrir durante la obtenci�n de los datos, devolviendo un error interno del servidor con un mensaje descriptivo en caso de fallo.
        /// </summary>
        /// <returns>Lista de autores disponibles en el sistema.</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAutores()
        {
            try
            {
                var autores = _autorService.GetAll();
                return Ok(autores);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error al obtener autores: {ex.Message}"));
            }
        }


        /// <summary>
        /// Permite crear un nuevo autor en el sistema utilizando los datos proporcionados en el cuerpo de la solicitud. Este endpoint valida que el DTO no sea nulo y que el modelo sea v�lido antes de intentar crear el autor. Si la creaci�n es exitosa, devuelve un c�digo 201 Created con la ubicaci�n del nuevo recurso. Si ocurre una excepci�n durante la creaci�n, devuelve un error interno del servidor con un mensaje descriptivo.
        /// </summary>
        /// <param name="dto">DTO con la informaci�n del autor a crear</param>
        /// <returns>Informaci�n del autor creado.</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostAutor([FromBody] AutorCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("El autor no puede ser nulo");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var autor = _autorService.Create(dto);
                var location = $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/api/v1/autores/{autor.Id}";
                return Created(location, autor);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error al crear autor: {ex.Message}"));
            }
        }

        /// <summary>
        /// Permite actualizar la informaci�n de un autor existente utilizando su ID y los datos proporcionados en el cuerpo de la solicitud. Este endpoint valida que el DTO no sea nulo y que el modelo sea v�lido antes de intentar actualizar el autor. Si el autor no existe, devuelve un error 404 Not Found. Si la actualizaci�n es exitosa, devuelve la informaci�n del autor actualizado. Si ocurre una excepci�n durante la actualizaci�n, devuelve un error interno del servidor con un mensaje descriptivo.
        /// </summary>
        /// <param name="id">Identiificador �nico del autor a actualizar</param>
        /// <param name="dto">DTO con la nueva informaci�n del autor a actualizar</param>
        /// <returns>Informaci�n del autor actualizado.</returns>
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult PutAutor(int id, [FromBody] AutorUpdateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("El autor no puede ser nulo");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var autor = _autorService.Update(id, dto);

                if (autor == null)
                {
                    return NotFound();
                }

                return Ok(autor);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error al actualizar autor: {ex.Message}"));
            }
        }

        
    }
}
