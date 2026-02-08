using System;

namespace NeorisBackend.DTOs.Responses.Autor
{
    /// <summary>
    /// DTO de respuesta para un autor
    /// </summary>
    public class AutorResponseDto
    {
        /// <summary>
        /// Ciudad de procedencia del autor
        /// </summary>
        public string CiudadProcedencia { get; set; }
        /// <summary>
        /// Correo electrónico del autor
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Fecha de creación del autor
        /// </summary>
        public DateTime FechaCreacion { get; set; }
        /// <summary>
        /// Fecha de última modificación del autor
        /// </summary>
        public DateTime? FechaModificacion { get; set; }
        /// <summary>
        /// Fecha de nacimiento del autor
        /// </summary>
        public DateTime FechaNacimiento { get; set; }
        /// <summary>
        /// Identificador único del autor
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Nombre del autor
        /// </summary>
        public string Nombre { get; set; }
    }
}
