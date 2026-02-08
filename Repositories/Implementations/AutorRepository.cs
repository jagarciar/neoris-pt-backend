using System.Linq;
using NeorisBackend.Data;
using NeorisBackend.Models;
using NeorisBackend.Repositories.Interfaces;

namespace NeorisBackend.Repositories.Implementations
{
    /// <summary>
    /// Implementación del repositorio de Autores
    /// </summary>
    public class AutorRepository : Repository<Autor>, IAutorRepository
    {
        public AutorRepository(NeorisDbContext context) : base(context)
        {
        }

        public bool ExistsWithEmail(string email, int? excludeId = null)
        {
            var query = _dbSet.Where(a => a.Email == email);
            
            if (excludeId.HasValue)
            {
                query = query.Where(a => a.Id != excludeId.Value);
            }

            return query.Any();
        }
    }
}
