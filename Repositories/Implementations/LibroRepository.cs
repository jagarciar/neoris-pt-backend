using NeorisBackend.Data;
using NeorisBackend.Models;
using NeorisBackend.Repositories.Interfaces;

namespace NeorisBackend.Repositories.Implementations
{
    /// <summary>
    /// Implementación del repositorio de Libros
    /// </summary>
    public class LibroRepository : Repository<Libro>, ILibroRepository
    {
        public LibroRepository(NeorisDbContext context) : base(context)
        {
        }

        // Aquí se pueden agregar implementaciones de métodos específicos de Libro
        // Por ejemplo:
        // public IEnumerable<Libro> GetLibrosByGenero(string genero)
        // {
        //     return Find(l => l.Genero == genero);
        // }
    }
}
