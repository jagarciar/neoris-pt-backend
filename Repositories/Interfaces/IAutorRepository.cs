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
        /// Verifica si existe un autor con el mismo email, excluyendo un ID específico (útil para actualizaciones)
        /// </summary>
        /// <param name="email">Correo electrónico del autor a verificar</param>
        /// <param name="excludeId">Identificador del autor a excluir en la verificación</param>
        /// <returns></returns>
        bool ExistsWithEmail(string email, int? excludeId = null);
    }
}
