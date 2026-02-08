using System.ComponentModel.DataAnnotations;

namespace NeorisBackend.DTOs.Requests.Libro
{
    /// <summary>
    /// DTO para crear un nuevo libro
    /// </summary>
    public class LibroCreateDto
    {
        /// <summary>
        /// Identificador del autor del libro
        /// </summary>
        [Required(ErrorMessage = "El autor es requerido")]
        public int AutorId { get; set; }
        /// <summary>
        /// Año de publicación del libro
        /// </summary>
        [Required(ErrorMessage = "El anio es requerido")]
        [Range(1900, 2026, ErrorMessage = "El anio debe estar entre 1900s y 2026")]
        public int Anio { get; set; }
        /// <summary>
        /// Genero del libro
        /// </summary>
        [Required(ErrorMessage = "El genero es requerido")]
        [StringLength(100, ErrorMessage = "El genero no puede exceder 100 caracteres")]
        public string Genero { get; set; }
        /// <summary>
        /// Número de páginas del libro
        /// </summary>
        [Required(ErrorMessage = "El numero de paginas es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "El numero de paginas debe ser mayor que cero")]
        public int NumeroPaginas { get; set; }
        /// <summary>
        /// Título del libro
        /// </summary>
        [Required(ErrorMessage = "El titulo es requerido")]
        [StringLength(200, ErrorMessage = "El titulo no puede exceder 200 caracteres")]
        public string Titulo { get; set; }
    }
}
