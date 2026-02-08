using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NeorisBackend.Repositories.Interfaces
{
    /// <summary>
    /// Interfaz genérica de repositorio con operaciones CRUD básicas
    /// </summary>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Obtiene todas las entidades
        /// </summary>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Obtiene todas las entidades con carga ansiosa de propiedades relacionadas
        /// </summary>
        IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Obtiene una entidad por su ID
        /// </summary>
        T GetById(int id);

        /// <summary>
        /// Busca entidades que cumplan con un predicado
        /// </summary>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Busca entidades con carga ansiosa de propiedades relacionadas
        /// </summary>
        IEnumerable<T> FindIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Obtiene un único registro que cumpla con el predicado
        /// </summary>
        T SingleOrDefault(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Agrega una nueva entidad
        /// </summary>
        void Add(T entity);

        /// <summary>
        /// Agrega múltiples entidades
        /// </summary>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Actualiza una entidad existente
        /// </summary>
        void Update(T entity);

        /// <summary>
        /// Elimina una entidad
        /// </summary>
        void Remove(T entity);

        /// <summary>
        /// Elimina múltiples entidades
        /// </summary>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Verifica si existe alguna entidad que cumpla con el predicado
        /// </summary>
        bool Any(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Cuenta las entidades que cumplen con el predicado
        /// </summary>
        int Count(Expression<Func<T, bool>> predicate);
    }
}
