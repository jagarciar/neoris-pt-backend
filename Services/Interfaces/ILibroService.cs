using System.Collections.Generic;
using NeorisBackend.DTOs.Requests;
using NeorisBackend.DTOs.Requests.Libro;
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
        /// Permite crear un nuevo libro
        /// </summary>
        /// <param name="dto">DTO de creación de libro</param>
        /// <returns>DTO de respuesta de libro</returns>
        LibroResponseDto Create(LibroCreateDto dto);

        /// <summary>
        /// Elimina un libro por su ID. Devuelve true si la eliminación fue exitosa, o false si el libro no existe o no se pudo eliminar por alguna razón. Esta operación es importante para mantener la integridad de los datos y permitir a los usuarios gestionar sus libros de manera efectiva.
        /// </summary>
        /// <param name="id">Identificador del libro a eliminar</param>
        /// <returns>Verdadero si la eliminación fue exitosa, falso en caso contrario</returns>
        bool Delete(int id);

        /// <summary>
        /// Obtiene todos los libros
        /// </summary>
        IEnumerable<LibroResponseDto> GetAll();

        /// <summary>
        /// Obtiene un libro por su ID
        /// </summary>
        /// <param name="id">Identificador del libro</param>
        /// <returns>DTO de respuesta de libro</returns>
        LibroResponseDto GetById(int id);

        /// <summary>
        /// Actualiza un libro existente por su ID
        /// </summary>
        /// <param name="id">Identificador del libro</param>
        /// <param name="dto">DTO de actualización de libro</param>
        /// <returns>DTO de respuesta de libro</returns>
        LibroResponseDto Update(int id, LibroUpdateDto dto);

        
    }
}
