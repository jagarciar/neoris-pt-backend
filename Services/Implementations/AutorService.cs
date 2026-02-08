using System;
using System.Collections.Generic;
using System.Linq;
using NeorisBackend.DTOs.Requests;
using NeorisBackend.DTOs.Requests.Autor;
using NeorisBackend.DTOs.Responses;
using NeorisBackend.DTOs.Responses.Autor;
using NeorisBackend.Models;
using NeorisBackend.Repositories.Interfaces;
using NeorisBackend.Services.Interfaces;

namespace NeorisBackend.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de Autores
    /// </summary>
    public class AutorService : IAutorService
    {
        /// <summary>
        /// Referencia al Unit of Work para acceder a los repositorios y manejar las transacciones. Este servicio utiliza el patrón Unit of Work para garantizar que todas las operaciones relacionadas con los autores se realicen de manera consistente y eficiente, permitiendo la gestión de múltiples repositorios y la coordinación de las operaciones de base de datos en una sola transacción.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor del servicio de autores, que recibe una instancia de IUnitOfWork a través de inyección de dependencias. Si el Unit of Work es nulo, se lanza una excepción ArgumentNullException para garantizar que el servicio siempre tenga una instancia válida del Unit of Work, lo que es esencial para acceder a los repositorios y gestionar las transacciones de manera adecuada en las operaciones relacionadas con los autores.
        /// </summary>
        /// <param name="unitOfWork">Unidad de trabajo que proporciona acceso a los repositorios y permite gestionar transacciones.</param>
        /// <exception cref="ArgumentNullException">Genera excepción si el Unit of Work es nulo.</exception>
        public AutorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Permite crear un nuevo autor en la base de datos. Antes de crear el autor, se valida que el email proporcionado no exista ya registrado para otro autor, lo que garantiza la unicidad del email en el sistema. Si el email ya existe, se lanza una excepción InvalidOperationException con un mensaje indicando que el email ya está registrado. Si la validación es exitosa, se crea una nueva instancia de Autor con los datos proporcionados en el DTO, se agrega al repositorio de autores a través del Unit of Work y se guardan los cambios en la base de datos. Finalmente, se retorna un AutorResponseDto con los datos del autor recién creado.
        /// </summary>
        /// <param name="dto">DTO de creación de autor</param>
        /// <returns>Información del autor creado</returns>
        /// <exception cref="InvalidOperationException">Genera excepción si el email ya está registrado.</exception>
        public AutorResponseDto Create(AutorCreateDto dto)
        {
            // Validar que el email no exista
            if (_unitOfWork.Autores.ExistsWithEmail(dto.Email))
            {
                throw new InvalidOperationException($"El email {dto.Email} ya esta registrado");
            }

            var autor = new Autor
            {
                Nombre = dto.Nombre,
                FechaNacimiento = dto.FechaNacimiento,
                CiudadProcedencia = dto.CiudadProcedencia,
                Email = dto.Email,
                FechaCreacion = DateTime.Now
            };

            _unitOfWork.Autores.Add(autor);
            _unitOfWork.SaveChanges();

            return GetById(autor.Id);
        }

        /// <summary>
        /// Permite eliminar un autor de la base de datos. Se busca el autor por su ID utilizando el repositorio de autores a través del Unit of Work. Si el autor no existe, se retorna false. Antes de eliminar, se verifica si el autor tiene libros asociados utilizando el repositorio de libros a través del Unit of Work. Si el autor tiene libros asociados, se lanza una excepción InvalidOperationException con un mensaje indicando que no se puede eliminar el autor porque tiene libros asociados. Si no hay libros asociados, se elimina el autor del repositorio a través del Unit of Work y se guardan los cambios en la base de datos. Finalmente, se retorna true para indicar que la eliminación fue exitosa.
        /// </summary>
        /// <param name="id">Identificador del autor a eliminar</param>
        /// <returns>Verdadero si se eliminó el autor, falso si no existía</returns>
        /// <exception cref="InvalidOperationException">Genera excepción si el autor tiene libros asociados.</exception>
        public bool Delete(int id)
        {
            var autor = _unitOfWork.Autores.GetById(id);
            if (autor == null)
            {
                return false;
            }

            // Verificar si hay libros asociados
            var tieneLibros = _unitOfWork.Libros.Any(l => l.AutorId == id);
            if (tieneLibros)
            {
                throw new InvalidOperationException($"No se puede eliminar el autor porque tiene libros asociados");
            }

            _unitOfWork.Autores.Remove(autor);
            _unitOfWork.SaveChanges();

            return true;
        }

        /// <summary>
        /// Retorna todos los autores registrados en la base de datos. Se obtiene la lista de autores a través del repositorio de autores utilizando el Unit of Work, y luego se proyecta cada autor a un AutorResponseDto para retornar solo la información relevante al cliente. Esta operación permite obtener una lista completa de todos los autores disponibles en el sistema, facilitando su visualización y gestión.
        /// </summary>
        /// <returns>Lista de todos los autores</returns>
        public IEnumerable<AutorResponseDto> GetAll()
        {
            var autores = _unitOfWork.Autores.GetAll();

            return autores.Select(a => new AutorResponseDto
            {
                Id = a.Id,
                Nombre = a.Nombre,
                FechaNacimiento = a.FechaNacimiento,
                CiudadProcedencia = a.CiudadProcedencia,
                Email = a.Email,
                FechaCreacion = a.FechaCreacion,
                FechaModificacion = a.FechaModificacion
            }).ToList();
        }

        /// <summary>
        /// Permite obtener un autor por su ID. Se busca el autor en la base de datos utilizando el repositorio de autores a través del Unit of Work. Si el autor no existe, se retorna null. Si el autor existe, se proyecta a un AutorResponseDto para retornar solo la información relevante al cliente. Esta operación es esencial para obtener los detalles de un autor específico, facilitando su visualización y gestión individual.
        /// </summary>
        /// <param name="id">Identificador del autor a obtener</param>
        /// <returns>Información del autor encontrado o null si no existe</returns>
        public AutorResponseDto GetById(int id)
        {
            var autor = _unitOfWork.Autores.GetById(id);

            if (autor == null)
            {
                return null;
            }

            return new AutorResponseDto
            {
                Id = autor.Id,
                Nombre = autor.Nombre,
                FechaNacimiento = autor.FechaNacimiento,
                CiudadProcedencia = autor.CiudadProcedencia,
                Email = autor.Email,
                FechaCreacion = autor.FechaCreacion,
                FechaModificacion = autor.FechaModificacion
            };
        }


        /// <summary>
        /// Permite actualizar la información de un autor existente. Se busca el autor por su ID utilizando el repositorio de autores a través del Unit of Work. Si el autor no existe, se retorna null. Antes de actualizar, se valida que el email proporcionado en el DTO no exista registrado para otro autor (excluyendo al autor que se está actualizando), lo que garantiza la unicidad del email en el sistema. Si el email ya existe para otro autor, se lanza una excepción InvalidOperationException con un mensaje indicando que el email ya está registrado para otro autor. Si la validación es exitosa, se actualizan los campos del autor con los datos proporcionados en el DTO, se establece la fecha de modificación a la fecha actual, se actualiza el autor en el repositorio a través del Unit of Work y se guardan los cambios en la base de datos. Finalmente, se retorna un AutorResponseDto con los datos del autor actualizado.
        /// </summary>
        /// <param name="id">Identificador del autor a actualizar</param>
        /// <param name="dto">DTO con los nuevos datos del autor</param>
        /// <returns>Información del autor actualizado o null si no existe</returns>
        /// <exception cref="InvalidOperationException">Genera excepción si el email ya está registrado para otro autor.</exception>
        public AutorResponseDto Update(int id, AutorUpdateDto dto)
        {
            var autor = _unitOfWork.Autores.GetById(id);
            if (autor == null)
            {
                return null;
            }

            // Validar que el email no exista para otro autor
            if (_unitOfWork.Autores.ExistsWithEmail(dto.Email, id))
            {
                throw new InvalidOperationException($"El email {dto.Email} ya esta registrado para otro autor");
            }

            autor.Nombre = dto.Nombre;
            autor.FechaNacimiento = dto.FechaNacimiento;
            autor.CiudadProcedencia = dto.CiudadProcedencia;
            autor.Email = dto.Email;
            autor.FechaModificacion = DateTime.Now;

            _unitOfWork.Autores.Update(autor);
            _unitOfWork.SaveChanges();

            return GetById(autor.Id);
        } 
    }
}
