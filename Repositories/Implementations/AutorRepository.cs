using System.Linq;
using NeorisBackend.Data;
using NeorisBackend.Models;
using NeorisBackend.Repositories.Interfaces;

namespace NeorisBackend.Repositories.Implementations
{
    /// <summary>
    /// Implementación del repositorio para la entidad Autor, proporcionando métodos específicos para la gestión de autores en la base de datos, como verificar la existencia de un autor por su correo electrónico, lo que es crucial para mantener la integridad de los datos y evitar duplicados en el sistema.
    /// </summary>
    public class AutorRepository : Repository<Autor>, IAutorRepository
    {
        /// <summary>
        /// Constructor que recibe el contexto de la base de datos y lo pasa al constructor de la clase base Repository, permitiendo así la interacción con la base de datos para realizar operaciones CRUD y consultas específicas relacionadas con los autores.
        /// </summary>
        /// <param name="context">Conexión con la base de datos</param>
        public AutorRepository(NeorisDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Permite validar si ya existe un autor registrado con el mismo correo electrónico, lo que es fundamental para evitar la duplicación de registros y garantizar la integridad de los datos en la base de datos. Este método también permite excluir un autor específico por su ID, lo que es útil durante las operaciones de actualización para asegurarse de que el correo electrónico no se duplique con otro autor diferente al que se está actualizando.
        /// </summary>
        /// <param name="email">Correo del autor a validar</param>
        /// <param name="excludeId">Id del autor a excluir en la validación</param>
        /// <returns>Verdadero si existe un autor con ese correo, falso en caso contrario</returns>
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
