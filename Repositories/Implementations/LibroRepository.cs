using NeorisBackend.Data;
using NeorisBackend.Models;
using NeorisBackend.Repositories.Interfaces;

namespace NeorisBackend.Repositories.Implementations
{
    /// <summary>
    /// Implementación del repositorio para la entidad Libro, que proporciona métodos específicos para interactuar con la base de datos en relación a los libros. Este repositorio hereda de la clase base Repository<Libro> y se encarga de realizar las operaciones CRUD específicas para los libros utilizando el contexto de datos NeorisDbContext.
    /// </summary>
    public class LibroRepository : Repository<Libro>, ILibroRepository
    {
        /// <summary>
        /// Constructor que recibe el contexto de datos NeorisDbContext y lo pasa a la clase base Repository para su uso en las operaciones de base de datos relacionadas con los libros.
        /// </summary>
        /// <param name="context">Conexión al contexto de datos de la base de datos</param>
        public LibroRepository(NeorisDbContext context) : base(context)
        {
        }
    }
}
