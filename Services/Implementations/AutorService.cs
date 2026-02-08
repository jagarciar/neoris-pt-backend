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
    /// Implementación del servicio de Autores
    /// </summary>
    public class AutorService : IAutorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AutorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

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
    }
}
