using NeorisBackend.DTOs.Requests;
using NeorisBackend.DTOs.Requests.Libro;
using NeorisBackend.DTOs.Responses;
using NeorisBackend.DTOs.Responses.Autor;
using NeorisBackend.Models;
using NeorisBackend.Repositories.Interfaces;
using NeorisBackend.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace NeorisBackend.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de Libros
    /// </summary>
    public class LibroService : ILibroService
    {
        /// <summary>
        /// Referencia al Unit of Work para acceder a los repositorios y gestionar las transacciones de manera centralizada. Esta instancia se inyecta a través del constructor, lo que permite una mejor testabilidad y separación de responsabilidades. El Unit of Work proporciona acceso a los repositorios de libros y autores, asegurando que todas las operaciones realizadas a través de este servicio sean consistentes y coherentes con la base de datos.
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor del servicio de libros, que recibe una instancia de IUnitOfWork a través de inyección de dependencias. Si el Unit of Work es nulo, se lanza una excepción ArgumentNullException para garantizar que el servicio siempre tenga una instancia válida del Unit of Work. Esta instancia se utiliza para acceder a los repositorios de libros y autores, y para gestionar las transacciones de manera centralizada, asegurando que todas las operaciones realizadas a través de este servicio sean consistentes y coherentes con la base de datos.
        /// </summary>
        /// <param name="unitOfWork">Unidad de trabajo que contiene los repositorios y gestiona las transacciones</param>
        /// <exception cref="ArgumentNullException">Genera excepción si el Unit of Work es nulo</exception>
        public LibroService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Permite crear un nuevo libro en la base de datos. Antes de crear el libro, se valida que el autor asociado al libro exista en la base de datos. Si el autor no existe, se lanza una excepción InvalidOperationException con un mensaje indicando que el autor no existe. Si el autor existe, se crea una nueva instancia de Libro con los datos proporcionados en el DTO, se agrega al repositorio de libros a través del Unit of Work, y se guardan los cambios en la base de datos. Finalmente, se devuelve un LibroResponseDto con los detalles del libro recién creado, incluyendo la información del autor asociado.
        /// </summary>
        /// <param name="dto">DTO de creación de libro</param>
        /// <returns>DTO de respuesta con los detalles del libro creado</returns>
        /// <exception cref="InvalidOperationException">Genera excepción si el autor no existe</exception>
        public LibroResponseDto Create(LibroCreateDto dto)
        {
            // Validar límite máximo de libros
            var maxLibrosConfig = ConfigurationManager.AppSettings["MaxLibros"];
            if (!string.IsNullOrEmpty(maxLibrosConfig) && int.TryParse(maxLibrosConfig, out int maxLibros))
            {
                var cantidadLibrosActual = _unitOfWork.Libros.Count(l => true);
                if (cantidadLibrosActual >= maxLibros)
                {
                    throw new InvalidOperationException($"No se puede crear el libro. Se ha alcanzado el límite máximo de {maxLibros} libros permitidos.");
                }
            }

            // Validar que el autor existe
            var autorExiste = _unitOfWork.Autores.Any(a => a.Id == dto.AutorId);
            if (!autorExiste)
            {
                throw new InvalidOperationException($"El autor con Id {dto.AutorId} no existe");
            }

            var libro = new Libro
            {
                Titulo = dto.Titulo,
                Anio = dto.Anio,
                Genero = dto.Genero,
                NumeroPaginas = dto.NumeroPaginas,
                AutorId = dto.AutorId
            };

            _unitOfWork.Libros.Add(libro);
            _unitOfWork.SaveChanges();

            return GetById(libro.Id);
        }

        /// <summary>
        /// Permite eliminar un libro de la base de datos por su ID. Se busca el libro por su ID utilizando el repositorio de libros a través del Unit of Work, y si no se encuentra el libro, se devuelve false. Si se encuentra el libro, se elimina del repositorio de libros, se guardan los cambios en la base de datos, y se devuelve true para indicar que la eliminación fue exitosa.
        /// </summary>
        /// <param name="id">Identificador del libro a eliminar</param>
        /// <returns>Verdadero si se eliminó el libro, falso si no se encontró el libro</returns>
        public bool Delete(int id)
        {
            var libro = _unitOfWork.Libros.GetById(id);
            if (libro == null)
            {
                return false;
            }

            _unitOfWork.Libros.Remove(libro);
            _unitOfWork.SaveChanges();

            return true;
        }

        /// <summary>
        /// Permite obtener una lista de todos los libros disponibles en la base de datos, incluyendo la información del autor asociado a cada libro. Se utiliza el método GetAllIncluding del repositorio de libros para obtener todos los libros junto con sus autores, y luego se proyecta cada libro a un LibroResponseDto que incluye los detalles del libro y la información del autor. Finalmente, se devuelve una lista de LibroResponseDto con los detalles de todos los libros disponibles.
        /// </summary>
        /// <returns>Lista de DTOs con los detalles de todos los libros disponibles</returns>
        public IEnumerable<LibroResponseDto> GetAll()
        {
            var libros = _unitOfWork.Libros.GetAllIncluding(l => l.Autor);

            return libros.Select(l => new LibroResponseDto
            {
                Id = l.Id,
                Titulo = l.Titulo,
                Anio = l.Anio,
                Genero = l.Genero,
                NumeroPaginas = l.NumeroPaginas,
                AutorId = l.AutorId,
                Autor = l.Autor != null ? new AutorResponseDto
                {
                    Id = l.Autor.Id,
                    Nombre = l.Autor.Nombre,
                    FechaNacimiento = l.Autor.FechaNacimiento,
                    CiudadProcedencia = l.Autor.CiudadProcedencia,
                    Email = l.Autor.Email,
                    FechaCreacion = l.Autor.FechaCreacion,
                    FechaModificacion = l.Autor.FechaModificacion
                } : null
            }).ToList();
        }

        /// <summary>
        /// Permite obtener los detalles de un libro específico por su ID, incluyendo la información del autor asociado. Se utiliza el método FindIncluding del repositorio de libros para buscar el libro por su ID junto con su autor, y si se encuentra el libro, se proyecta a un LibroResponseDto que incluye los detalles del libro y la información del autor. Si no se encuentra el libro, se devuelve null. Finalmente, se devuelve un LibroResponseDto con los detalles del libro encontrado o null si no se encuentra el libro.
        /// </summary>
        /// <param name="id">Identificador del libro a obtener</param>
        /// <returns>DTO de respuesta con los detalles del libro encontrado o null si no se encuentra el libro</returns>
        public LibroResponseDto GetById(int id)
        {
            var libro = _unitOfWork.Libros.FindIncluding(l => l.Id == id, l => l.Autor).FirstOrDefault();

            if (libro == null)
            {
                return null;
            }

            return new LibroResponseDto
            {
                Id = libro.Id,
                Titulo = libro.Titulo,
                Anio = libro.Anio,
                Genero = libro.Genero,
                NumeroPaginas = libro.NumeroPaginas,
                AutorId = libro.AutorId,
                Autor = libro.Autor != null ? new AutorResponseDto
                {
                    Id = libro.Autor.Id,
                    Nombre = libro.Autor.Nombre,
                    FechaNacimiento = libro.Autor.FechaNacimiento,
                    CiudadProcedencia = libro.Autor.CiudadProcedencia,
                    Email = libro.Autor.Email,
                    FechaCreacion = libro.Autor.FechaCreacion,
                    FechaModificacion = libro.Autor.FechaModificacion
                } : null
            };
        }

        /// <summary>
        /// Permite actualizar los detalles de un libro existente en la base de datos por su ID. Se busca el libro por su ID utilizando el repositorio de libros a través del Unit of Work, y si no se encuentra el libro, se devuelve null. Si se encuentra el libro, se valida que el autor asociado al libro exista en la base de datos. Si el autor no existe, se lanza una excepción InvalidOperationException con un mensaje indicando que el autor no existe. Si el autor existe, se actualizan los detalles del libro con los datos proporcionados en el DTO, se actualiza el libro en el repositorio de libros a través del Unit of Work, y se guardan los cambios en la base de datos. Finalmente, se devuelve un LibroResponseDto con los detalles del libro actualizado, incluyendo la información del autor asociado.
        /// </summary>
        /// <param name="id">Identificador del libro a actualizar</param>
        /// <param name="dto">DTO de actualización de libro</param>
        /// <returns>DTO de respuesta con los detalles del libro actualizado</returns>
        /// <exception cref="InvalidOperationException">Genera excepción si el autor no existe</exception>
        public LibroResponseDto Update(int id, LibroUpdateDto dto)
        {
            var libro = _unitOfWork.Libros.GetById(id);
            if (libro == null)
            {
                return null;
            }

            // Validar que el autor existe
            var autorExiste = _unitOfWork.Autores.Any(a => a.Id == dto.AutorId);
            if (!autorExiste)
            {
                throw new InvalidOperationException($"El autor con Id {dto.AutorId} no existe");
            }

            libro.Titulo = dto.Titulo;
            libro.Anio = dto.Anio;
            libro.Genero = dto.Genero;
            libro.NumeroPaginas = dto.NumeroPaginas;
            libro.AutorId = dto.AutorId;

            _unitOfWork.Libros.Update(libro);
            _unitOfWork.SaveChanges();

            return GetById(libro.Id);
        }
    }
}
