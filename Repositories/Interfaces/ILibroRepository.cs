using NeorisBackend.Models;

namespace NeorisBackend.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz específica del repositorio de Libros
    /// Extiende IRepository con métodos específicos si son necesarios
    /// </summary>
    public interface ILibroRepository : IRepository<Libro>
    {
        // Aquí se pueden agregar métodos específicos de Libro si son necesarios
        // Por ejemplo: IEnumerable<Libro> GetLibrosByGenero(string genero);
    }
}
