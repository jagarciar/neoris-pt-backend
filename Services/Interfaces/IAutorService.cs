using NeorisBackend.DTOs.Requests;
using NeorisBackend.DTOs.Requests.Autor;
using NeorisBackend.DTOs.Responses;
using NeorisBackend.DTOs.Responses.Autor;
using System.Collections.Generic;

namespace NeorisBackend.Services.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de Autores
    /// Contiene la lógica de negocio relacionada con autores
    /// </summary>
    public interface IAutorService
    {

        /// <summary>
        /// Permite crear un nuevo autor
        /// </summary>
        /// <param name="dto">DTO para crear el autor</param>
        /// <returns>Información del autor creado</returns>
        AutorResponseDto Create(AutorCreateDto dto);

        /// <summary>
        /// Elimina un autor por su ID. Devuelve true si la eliminación fue exitosa, o false si el autor no existe o no se pudo eliminar por alguna razón. Este método maneja la lógica de negocio relacionada con la eliminación de autores, asegurando que se realicen las validaciones necesarias antes de intentar eliminar el autor de la base de datos. Si el autor no existe, devuelve false para indicar que no se pudo realizar la eliminación. Si ocurre algún error durante el proceso de eliminación, también devuelve false para indicar que la operación no fue exitosa.
        /// </summary>
        /// <param name="id">Identificador del autor a eliminar</param>
        /// <returns>Verdadero si se eliminó correctamente, falso en caso contrario</returns>
        bool Delete(int id);

        /// <summary>
        /// Obtiene todos los autores
        /// </summary>
        IEnumerable<AutorResponseDto> GetAll();

        /// <summary>
        /// Obtiene un autor por su ID
        /// </summary>
        /// <param name="id">Identificador del autor</param>
        /// <returns>Información del autor encontrado</returns>
        AutorResponseDto GetById(int id);

        /// <summary>
        /// Permite actualizar la información de un autor existente
        /// </summary>
        /// <param name="id">Identificador del autor</param>
        /// <param name="dto">DTO con la nueva información del autor</param>
        /// <returns>Información del autor actualizado</returns>
        AutorResponseDto Update(int id, AutorUpdateDto dto);

    }
}
