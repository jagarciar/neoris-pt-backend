using NeorisBackend.Models;

namespace NeorisBackend.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz específica del repositorio de Autores
    /// Extiende IRepository con métodos específicos si son necesarios
    /// </summary>
    public interface IAutorRepository : IRepository<Autor>
    {
        /// <summary>
        /// Verifica si existe un autor con el email especificado
        /// </summary>
        bool ExistsWithEmail(string email, int? excludeId = null);
    }
}
