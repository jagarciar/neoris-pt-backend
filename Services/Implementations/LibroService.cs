using System;
using System.Collections.Generic;
using System.Linq;
using NeorisBackend.DTOs.Requests;
using NeorisBackend.DTOs.Responses;
using NeorisBackend.Models;
using NeorisBackend.Repositories.Interfaces;
using NeorisBackend.Services.Interfaces;

namespace NeorisBackend.Services.Implementations
{
    /// <summary>
    /// Implementación del servicio de Libros
    /// </summary>
    public class LibroService : ILibroService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LibroService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

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

        public LibroResponseDto Create(LibroCreateDto dto)
        {
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
    }
}
