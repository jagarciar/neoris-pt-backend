using System.Collections.Generic;
using NeorisBackend.DTOs.Requests;
using NeorisBackend.DTOs.Responses;

namespace NeorisBackend.Services.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de Libros
    /// Contiene la lógica de negocio relacionada con libros
    /// </summary>
    public interface ILibroService
    {
        /// <summary>
        /// Obtiene todos los libros
        /// </summary>
        IEnumerable<LibroResponseDto> GetAll();

        /// <summary>
        /// Obtiene un libro por ID
        /// </summary>
        LibroResponseDto GetById(int id);

        /// <summary>
        /// Crea un nuevo libro
        /// </summary>
        LibroResponseDto Create(LibroCreateDto dto);

        /// <summary>
        /// Actualiza un libro existente
        /// </summary>
        LibroResponseDto Update(int id, LibroUpdateDto dto);

        /// <summary>
        /// Elimina un libro
        /// </summary>
        bool Delete(int id);
    }
}
