using System;
using System.ComponentModel.DataAnnotations;

namespace NeorisBackend.DTOs.Requests.Autor
{
    /// <summary>
    /// DTO para actualizar un autor existente
    /// </summary>
    public class AutorUpdateDto
    {
        /// <summary>
        /// Ciudad de procedencia del autor
        /// </summary>
        [Required(ErrorMessage = "La ciudad de procedencia es requerida")]
        [StringLength(100, ErrorMessage = "La ciudad de procedencia no puede exceder 100 caracteres")]
        public string CiudadProcedencia { get; set; }

        /// <summary>
        /// Correo electrónico del autor
        /// </summary>
        [Required(ErrorMessage = "El email es requerido")]
        [StringLength(255, ErrorMessage = "El email no puede exceder 255 caracteres")]
        [EmailAddress(ErrorMessage = "El formato del email no es valido")]
        public string Email { get; set; }

        /// <summary>
        /// Fecha de nacimiento del autor
        /// </summary>
        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        public DateTime FechaNacimiento { get; set; }

        /// <summary>
        /// Nombre del autor
        /// </summary>
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; }

        

        
        
    }
}
