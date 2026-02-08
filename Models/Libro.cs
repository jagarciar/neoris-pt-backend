using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeorisBackend.Models
{
    /// <summary>
    /// Representa un libro en la base de datos
    /// </summary>
    [Table("Libros")]
    public class Libro
    {
        /// <summary>
        /// Año de publicación del libro
        /// </summary>
        [Required(ErrorMessage = "El anio es requerido")]
        [Range(1900, 2026, ErrorMessage = "El anio debe estar entre 1900 y 2026")]
        public int Anio { get; set; }
        /// <summary>
        /// Autor del libro
        /// </summary>
        [ForeignKey("AutorId")]
        public virtual Autor Autor { get; set; }
        /// <summary>
        /// Identificador del autor del libro
        /// </summary>
        [Required(ErrorMessage = "El autor es requerido")]
        public int AutorId { get; set; }
        /// <summary>
        /// Genero del libro
        /// </summary>
        [Required(ErrorMessage = "El genero es requerido")]
        [StringLength(100, ErrorMessage = "El genero no puede exceder 100 caracteres")]
        public string Genero { get; set; }
        /// <summary>
        /// Identificador unico del libro
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
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
