using NeorisBackend.DTOs.Responses.Autor;

namespace NeorisBackend.DTOs.Responses
{
    /// <summary>
    /// DTO de respuesta para un libro
    /// </summary>
    public class LibroResponseDto
    {
        /// <summary>
        /// Año de publicación del libro
        /// </summary>
        public int Anio { get; set; }
        /// <summary>
        /// Autor del libro
        /// </summary>
        public AutorResponseDto Autor { get; set; }
        /// <summary>
        /// Identificador del autor del libro
        /// </summary>
        public int AutorId { get; set; }
        /// <summary>
        /// Genero del libro
        /// </summary>
        public string Genero { get; set; }
        /// <summary>
        /// Identificador único del libro
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Número de páginas del libro
        /// </summary>
        public int NumeroPaginas { get; set; }
        /// <summary>
        /// Titulo del libro
        /// </summary>
        public string Titulo { get; set; }

    }
}
