using System;
using System.Web.Http;
using NeorisBackend.DTOs.Requests;
using NeorisBackend.Services.Interfaces;
using Microsoft.Web.Http;
using NeorisBackend.DTOs.Requests.Libro;

namespace NeorisBackend.Controllers
{
    /// <summary>
    /// Controlador para gestionar las operaciones relacionadas con los libros, incluyendo la creaci�n, actualizaci�n, eliminaci�n y obtenci�n de informaci�n de los libros. Este controlador utiliza el servicio de libro para realizar las operaciones de negocio y est� protegido por autenticaci�n para garantizar que solo los usuarios autorizados puedan acceder a sus endpoints.
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/libros")]
    [Authorize]
    public class LibrosController : ApiController
    {
        /// <summary>
        /// Referencia al servicio de libro, que maneja la l�gica de negocio relacionada con las operaciones CRUD de los libros. Este servicio es inyectado a trav�s del constructor para promover un dise�o modular y facilitar las pruebas unitarias.
        /// </summary>
        private readonly ILibroService _libroService;

        /// <summary>
        /// Constructor del controlador de libros, que recibe una instancia de ILibroService a trav�s de inyecci�n de dependencias. Si el servicio es nulo, se lanza una excepci�n ArgumentNullException para garantizar que el controlador siempre tenga una instancia v�lida del servicio de libro.
        /// </summary>
        /// <param name="libroService">Servicio de libro inyectado para ser utilizado en las operaciones del controlador.</param>
        /// <exception cref="ArgumentNullException">Genera excepci�n si el servicio de libros es nulo</exception>
        public LibrosController(ILibroService libroService)
        {
            _libroService = libroService ?? throw new ArgumentNullException(nameof(libroService));
        }


        /// <summary>
        /// Permite eliminar un libro espec�fico utilizando su ID. Este endpoint maneja cualquier excepci�n que pueda ocurrir durante la eliminaci�n, devolviendo un error interno del servidor con un mensaje descriptivo en caso de fallo. Si el libro no existe, devuelve un error 404 Not Found. Si la eliminaci�n es exitosa, devuelve un mensaje de confirmaci�n indicando que el libro ha sido eliminado exitosamente.
        /// </summary>
        /// <param name="id">Identificador del libro a eliminar</param>
        /// <returns>200 /Ok si el libro fue eliminado correctamente, 404 si no se encuentra el libro, 500 si ocurre un error interno</returns>
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteLibro(int id)
        {
            try
            {
                var result = _libroService.Delete(id);

                if (!result)
                {
                    return NotFound();
                }

                return Ok(new { message = $"Libro {id} eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error al eliminar libro: {ex.Message}"));
            }
        }

        /// <summary>
        /// Permite obtener la informaci�n de un libro espec�fico utilizando su ID. Este endpoint maneja cualquier excepci�n que pueda ocurrir durante la obtenci�n del libro, devolviendo un error interno del servidor con un mensaje descriptivo en caso de fallo. Si el libro no se encuentra, devuelve un error 404 Not Found. Si la operaci�n es exitosa, devuelve la informaci�n del libro en formato JSON.
        /// </summary>
        /// <param name="id">Identificador del libro a obtener</param>
        /// <returns>Informacion del libro solicitado</returns>
        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult GetLibro(int id)
        {
            try
            {
                var libro = _libroService.GetById(id);

                if (libro == null)
                {
                    return NotFound();
                }

                return Ok(libro);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error al obtener libro: {ex.Message}"));
            }
        }

        /// <summary>
        /// Permite obtener una lista de todos los libros disponibles. Este endpoint maneja cualquier excepci�n que pueda ocurrir durante la obtenci�n de los libros, devolviendo un error interno del servidor con un mensaje descriptivo en caso de fallo. Si la operaci�n es exitosa, devuelve una lista de libros en formato JSON.
        /// </summary>
        /// <returns>Lista de libros disponibles</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetLibros()
        {
            try
            {
                var libros = _libroService.GetAll();
                return Ok(libros);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error al obtener libros: {ex.Message}"));
            }
        }

        /// <summary>
        /// Permite crear un nuevo libro utilizando la informaci�n proporcionada en el cuerpo de la solicitud. Este endpoint valida que el DTO no sea nulo y que el modelo sea v�lido antes de intentar crear el libro. Si la creaci�n es exitosa, devuelve un c�digo 201 Created con la informaci�n del libro creado. Si ocurre una excepci�n durante la creaci�n, devuelve un error interno del servidor con un mensaje descriptivo.
        /// </summary>
        /// <param name="dto">DTO con la informacion del nuevo libro</param>
        /// <returns>Informaci�n del libro creado</returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult PostLibro([FromBody] LibroCreateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("El libro no puede ser nulo");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var libro = _libroService.Create(dto);
                var location = $"{Request.RequestUri.GetLeftPart(UriPartial.Authority)}/api/v1/libros/{libro.Id}";
                return Created(location, libro);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error al crear libro: {ex.Message}"));
            }
        }

        /// <summary>
        /// Permite actualizar la informaci�n de un libro existente utilizando su ID y la informaci�n proporcionada en el cuerpo de la solicitud. Este endpoint valida que el DTO no sea nulo y que el modelo sea v�lido antes de intentar actualizar el libro. Si el libro a actualizar no se encuentra, devuelve un error 404 Not Found. Si la actualizaci�n es exitosa, devuelve la informaci�n del libro actualizado. Si ocurre una excepci�n durante la actualizaci�n, devuelve un error interno del servidor con un mensaje descriptivo.
        /// </summary>
        /// <param name="id">Identificador del libro a actualizar</param>
        /// <param name="dto">DTO con la informacion actualizada del libro</param>
        /// <returns>Informaci�n del libro actualizado</returns>
        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult PutLibro(int id, [FromBody] LibroUpdateDto dto)
        {
            if (dto == null)
            {
                return BadRequest("El libro no puede ser nulo");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var libro = _libroService.Update(id, dto);

                if (libro == null)
                {
                    return NotFound();
                }

                return Ok(libro);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception($"Error al actualizar libro: {ex.Message}"));
            }
        }

    }
}
