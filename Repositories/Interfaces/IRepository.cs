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
        /// Permite agregar una nueva entidad al repositorio
        /// </summary>
        /// <param name="entity">Entidad a agregar</param>
        void Add(T entity);

        /// <summary>
        /// Permite agregar múltiples entidades al repositorio
        /// </summary>
        /// <param name="entities">Entidades a agregar</param>
        void AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Verifica si existe alguna entidad que cumpla con el predicado especificado
        /// </summary>
        /// <param name="predicate">Predicado de búsqueda</param>
        /// <returns>Verdadero si existe al menos una entidad que cumpla con el predicado</returns>
        bool Any(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Cuenta el número de entidades que cumplen con el predicado especificado
        /// </summary>
        /// <param name="predicate">Predicado de búsqueda</param>
        /// <returns>Total de entidades que cumplen con el predicado</returns>
        int Count(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Busca entidades que cumplan con el predicado especificado
        /// </summary>
        /// <param name="predicate">Predicado de búsqueda</param>
        /// <returns>Conjunto de entidades que cumple con el predicado</returns>
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Busca entidades que cumplan con el predicado especificado e incluye propiedades relacionadas
        /// </summary>
        /// <param name="predicate">Prediccado de búsqueda</param>
        /// <param name="includeProperties">Propiedades relacionadas a incluir en la carga</param>
        /// <returns>Conjunto de entidades que cumple con el predicado e incluye propiedades relacionadas</returns>
        IEnumerable<T> FindIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Obtiene todas las entidades
        /// </summary>
        IEnumerable<T> GetAll();

        /// <summary>
        /// Obtiene todas las entidades con carga ansiosa de propiedades relacionadas
        /// </summary>
        /// <param name="includeProperties">Propiedades relacionadas a cargar</param>
        /// <returns>Conjunto de entidades</returns>
        IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Obtiene una entidad por su identificador
        /// </summary>
        /// <param name="id">Identificador de la entidad</param>
        /// <returns>Entidad encontrada o null</returns>
        T GetById(int id);

        /// <summary>
        /// Elimina una entidad del repositorio
        /// </summary>
        /// <param name="entity">Entidad a eliminar</param>
        void Remove(T entity);

        /// <summary>
        /// Elimina múltiples entidades del repositorio
        /// </summary>
        /// <param name="entities">Entidades a eliminar</param>
        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Obtiene una única entidad que cumpla con el predicado especificado o null si no se encuentra ninguna
        /// </summary>
        /// <param name="predicate">Predicado de búsqueda</param>
        /// <returns>Entidad encontrada o null</returns>
        T SingleOrDefault(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Permite actualizar una entidad existente en el repositorio
        /// </summary>
        /// <param name="entity">Entidad a actualizar</param>
        void Update(T entity);
    }
}
