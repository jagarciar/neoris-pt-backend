using System.Collections.Generic;
using NeorisBackend.DTOs.Requests;
using NeorisBackend.DTOs.Responses;

namespace NeorisBackend.Services.Interfaces
{
    /// <summary>
    /// Interfaz del servicio de Autores
    /// Contiene la lógica de negocio relacionada con autores
    /// </summary>
    public interface IAutorService
    {
        /// <summary>
        /// Obtiene todos los autores
        /// </summary>
        IEnumerable<AutorResponseDto> GetAll();

        /// <summary>
        /// Obtiene un autor por ID
        /// </summary>
        AutorResponseDto GetById(int id);

        /// <summary>
        /// Crea un nuevo autor
        /// </summary>
        AutorResponseDto Create(AutorCreateDto dto);

        /// <summary>
        /// Actualiza un autor existente
        /// </summary>
        AutorResponseDto Update(int id, AutorUpdateDto dto);

        /// <summary>
        /// Elimina un autor
        /// </summary>
        bool Delete(int id);
    }
}
