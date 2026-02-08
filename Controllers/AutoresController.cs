using System;
using System.Web.Http;
using NeorisBackend.DTOs.Requests;
using NeorisBackend.Services.Interfaces;
using Microsoft.Web.Http;

namespace NeorisBackend.Controllers
{
    /// <summary>
    /// Controlador de Autores - Refactorizado con Clean Architecture
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/autores")]
    [Authorize]
    public class AutoresController : ApiController
    {
        private readonly IAutorService _autorService;

        public AutoresController(IAutorService autorService)
        {
            _autorService = autorService ?? throw new ArgumentNullException(nameof(autorService));
        }

        // GET: api/autores
        /// <summary>
        /// Obtiene todos los autores activos
        /// </summary>
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

        // GET: api/autores/5
        /// <summary>
        /// Obtiene un autor especifico por ID
        /// </summary>
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

        // POST: api/autores
        /// <summary>
        /// Crea un nuevo autor
        /// </summary>
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
                return CreatedAtRoute("DefaultApi", new { id = autor.Id }, autor);
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

        // PUT: api/autores/5
        /// <summary>
        /// Actualiza un autor existente
        /// </summary>
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

        // DELETE: api/autores/5
        /// <summary>
        /// Elimina un autor
        /// </summary>
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
    }
}
