using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeorisBackend.Models
{
    /// <summary>
    /// Representa un autor en el sistema
    /// </summary>
    [Table("Autores")]
    public class Autor
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
        /// Fecha de creación del autor
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime FechaCreacion { get; set; }
        /// <summary>
        /// Fecha de última modificación del autor
        /// </summary>
        public DateTime? FechaModificacion { get; set; }
        /// <summary>
        /// Fecha de nacimiento del autor
        /// </summary>
        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        [Column(TypeName = "date")]
        public DateTime FechaNacimiento { get; set; }
        /// <summary>
        /// Identificador único del autor
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// Nombre del autor
        /// </summary>
        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        public string Nombre { get; set; }
        
    }
}
