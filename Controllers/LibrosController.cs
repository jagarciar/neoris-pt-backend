using System;
using System.Web.Http;
using NeorisBackend.DTOs.Requests;
using NeorisBackend.Services.Interfaces;
using Microsoft.Web.Http;

namespace NeorisBackend.Controllers
{
    /// <summary>
    /// Controlador de Libros - Refactorizado con Clean Architecture
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/libros")]
    [Authorize]
    public class LibrosController : ApiController
    {
        private readonly ILibroService _libroService;

        public LibrosController(ILibroService libroService)
        {
            _libroService = libroService ?? throw new ArgumentNullException(nameof(libroService));
        }

        // GET: api/libros
        /// <summary>
        /// Obtiene todos los libros con su autor
        /// </summary>
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

        // GET: api/libros/5
        /// <summary>
        /// Obtiene un libro especifico por ID
        /// </summary>
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

        // POST: api/libros
        /// <summary>
        /// Crea un nuevo libro
        /// </summary>
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
                return CreatedAtRoute("DefaultApi", new { id = libro.Id }, libro);
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

        // PUT: api/libros/5
        /// <summary>
        /// Actualiza un libro existente
        /// </summary>
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

        // DELETE: api/libros/5
        /// <summary>
        /// Elimina un libro
        /// </summary>
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
    }
}
