using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using NeorisBackend.Data;
using NeorisBackend.Repositories.Interfaces;

namespace NeorisBackend.Repositories.Implementations
{
    /// <summary>
    /// Implementación genérica del patrón Repository para manejar operaciones CRUD y consultas en la base de datos utilizando Entity Framework. Esta clase proporciona métodos comunes para interactuar con cualquier entidad del modelo, permitiendo la reutilización de código y la separación de preocupaciones entre la lógica de acceso a datos y la lógica de negocio. Al utilizar esta implementación genérica, se facilita el mantenimiento y la escalabilidad del código, ya que se pueden agregar nuevas entidades sin necesidad de crear repositorios específicos para cada una, siempre y cuando sigan las convenciones establecidas por Entity Framework.
    /// </summary>
    /// <typeparam name="T">Tipo de entidad manejada por el repositorio</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        /// <summary>
        /// Representa el conjunto de entidades del tipo T en la base de datos, permitiendo realizar operaciones de consulta y manipulación de datos. Este DbSet se inicializa en el constructor utilizando el contexto de la base de datos, lo que facilita el acceso a las entidades y la ejecución de operaciones CRUD de manera eficiente. Al ser protegido, permite que las clases derivadas puedan acceder a él para realizar operaciones específicas sin exponerlo directamente a otras partes del código, manteniendo así una buena encapsulación y separación de responsabilidades.
        /// </summary>
        protected readonly DbSet<T> _dbSet;
        /// <summary>
        /// Provee el contexto de la base de datos y el DbSet correspondiente a la entidad T, permitiendo realizar operaciones de consulta y manipulación de datos. El contexto se inyecta a través del constructor, lo que facilita la gestión de dependencias y la realización de pruebas unitarias. El DbSet se utiliza para acceder a las entidades del tipo T en la base de datos, proporcionando métodos para agregar, actualizar, eliminar y consultar los datos de manera eficiente.
        /// </summary>
        protected readonly Data.NeorisDbContext _context;

        /// <summary>
        /// Constructor que recibe una instancia del contexto de la base de datos y la utiliza para inicializar el DbSet correspondiente a la entidad T. Si el contexto es nulo, se lanza una excepción ArgumentNullException para garantizar que el repositorio siempre tenga una instancia válida del contexto de la base de datos. Esta implementación asegura que el repositorio esté correctamente configurado para interactuar con la base de datos y realizar las operaciones necesarias sobre las entidades del tipo T.
        /// </summary>
        /// <param name="context">Contexto de la base de datos</param>
        /// <exception cref="ArgumentNullException">Genera excepción si el contexto es nulo</exception>
        public Repository(Data.NeorisDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Permite agregar una nueva entidad del tipo T a la base de datos. Este método utiliza el DbSet para agregar la entidad al contexto, lo que marca la entidad como agregada y lista para ser insertada en la base de datos cuando se guarden los cambios. Al ser virtual, permite que las clases derivadas puedan sobrescribir este método para implementar lógica adicional o personalizada al agregar entidades, como validar los datos antes de agregarlos o manejar casos específicos de inserción. Esta implementación básica proporciona una forma sencilla de agregar nuevos registros a la base de datos sin necesidad de escribir consultas específicas cada vez.
        /// </summary>
        /// <param name="entity">Información de la entidad a agregar</param>
        /// <exception cref="ArgumentNullException">Genera excepción si la entidad es nula</exception>
        public virtual void Add(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Add(entity);
        }

        /// <summary>
        /// Permite agregar un rango de entidades del tipo T a la base de datos. Este método utiliza el DbSet para agregar múltiples entidades al contexto, lo que marca cada entidad como agregada y lista para ser insertada en la base de datos cuando se guarden los cambios. Al ser virtual, permite que las clases derivadas puedan sobrescribir este método para implementar lógica adicional o personalizada al agregar múltiples entidades, como validar los datos antes de agregarlos o manejar casos específicos de inserción. Esta implementación básica proporciona una forma sencilla de agregar varios registros a la base de datos sin necesidad de escribir consultas específicas cada vez.
        /// </summary>
        /// <param name="entities">Lista de entidades a agregar</param>
        /// <exception cref="ArgumentNullException">Genera excepción si la lista de entidades es nula</exception>
        public virtual void AddRange(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _dbSet.AddRange(entities);
        }

        /// <summary>
        /// Permite verificar si existe al menos una entidad del tipo T en la base de datos que cumpla con una condición específica definida por el predicado. Este método utiliza el DbSet para aplicar el filtro definido por el predicado y devuelve un valor booleano que indica si se encontró alguna entidad que coincida con la condición. Al ser virtual, permite que las clases derivadas puedan sobrescribir este método para implementar lógica adicional o personalizada al verificar la existencia de entidades, como aplicar filtros más complejos o manejar casos específicos de búsqueda. Esta implementación proporciona una forma sencilla de determinar si existen registros de una entidad que cumplen con ciertos criterios sin necesidad de escribir consultas específicas cada vez.
        /// </summary>
        /// <param name="predicate">Predicate que define la condición de filtrado</param>
        /// <returns>Verdadero si existe al menos una entidad que cumple con el predicado, falso en caso contrario</returns>
        public virtual bool Any(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }

        /// <summary>
        /// Permite contar el número de entidades del tipo T en la base de datos que cumplan con una condición específica definida por el predicado. Este método utiliza el DbSet para aplicar el filtro definido por el predicado y devuelve un entero que representa la cantidad de entidades que coinciden con la condición. Al ser virtual, permite que las clases derivadas puedan sobrescribir este método para implementar lógica adicional o personalizada al contar entidades, como aplicar filtros más complejos o manejar casos específicos de búsqueda. Esta implementación proporciona una forma sencilla de obtener el número de registros de una entidad que cumplen con ciertos criterios sin necesidad de escribir consultas específicas cada vez.
        /// </summary>
        /// <param name="predicate">Predicate que define la condición de filtrado</param>
        /// <returns>Cantidad de entidades del tipo T que cumplen con el predicado</returns>
        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Count(predicate);
        }

        /// <summary>
        /// Permite encontrar entidades del tipo T en la base de datos que cumplan con una condición específica definida por el predicado. Este método utiliza el DbSet para aplicar el filtro definido por el predicado y devuelve una lista de entidades que coinciden con la condición. Al ser virtual, permite que las clases derivadas puedan sobrescribir este método para implementar lógica adicional o personalizada al obtener los datos, como aplicar filtros más complejos, ordenar los resultados o incluir relaciones con otras entidades. Esta implementación básica proporciona una forma sencilla de acceder a los registros de una entidad que cumplen con ciertos criterios sin necesidad de escribir consultas específicas cada vez.
        /// </summary>
        /// <param name="predicate">Predicate que define la condición de filtrado</param>
        /// <returns>Lista de entidades del tipo T que cumplen con el predicado</returns>
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        /// <summary>
        /// Permite encontrar entidades del tipo T en la base de datos que cumplan con una condición específica definida por el predicado, incluyendo las propiedades de navegación especificadas. Este método utiliza el DbSet para aplicar el filtro definido por el predicado y aplica las inclusiones de propiedades de navegación utilizando el método Include de Entity Framework. Al ser virtual, permite que las clases derivadas puedan sobrescribir este método para implementar lógica adicional o personalizada al obtener los datos, como aplicar filtros más complejos, ordenar los resultados o incluir relaciones con otras entidades. Esta implementación proporciona una forma flexible de acceder a los registros de una entidad que cumplen con ciertos criterios junto con sus relaciones, evitando la necesidad de escribir consultas específicas cada vez que se requiera incluir propiedades relacionadas.
        /// </summary>
        /// <param name="predicate">Predicate que define la condición de filtrado</param>
        /// <param name="includeProperties">Propiedades de navegación a incluir</param>
        /// <returns>Lista de entidades del tipo T que cumplen con el predicado con las propiedades incluidas</returns>
        public virtual IEnumerable<T> FindIncluding(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.Where(predicate).ToList();
        }

        /// <summary>
        /// Permite obtener todas las entidades del tipo T desde la base de datos. Este método utiliza el DbSet para recuperar todos los registros correspondientes a la entidad T y los devuelve como una lista. Al ser virtual, permite que las clases derivadas puedan sobrescribir este método para implementar lógica adicional o personalizada al obtener los datos, como aplicar filtros, ordenar los resultados o incluir relaciones con otras entidades. Esta implementación básica proporciona una forma sencilla de acceder a todos los registros de una entidad sin necesidad de escribir consultas específicas cada vez.
        /// </summary>
        /// <returns>Lista de todas las entidades del tipo T</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        /// <summary>
        /// Permite obtener todas las entidades del tipo T desde la base de datos, incluyendo las propiedades de navegación especificadas. Este método utiliza el DbSet para recuperar los registros correspondientes a la entidad T y aplica las inclusiones de propiedades de navegación utilizando el método Include de Entity Framework. Al ser virtual, permite que las clases derivadas puedan sobrescribir este método para implementar lógica adicional o personalizada al obtener los datos, como aplicar filtros, ordenar los resultados o incluir relaciones con otras entidades. Esta implementación proporciona una forma flexible de acceder a los registros de una entidad junto con sus relaciones, evitando la necesidad de escribir consultas específicas cada vez que se requiera incluir propiedades relacionadas.
        /// </summary>
        /// <param name="includeProperties">Propiedades de navegación a incluir</param>
        /// <returns>Lista de todas las entidades del tipo T con las propiedades incluidas</returns>
        public virtual IEnumerable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }

        /// <summary>
        /// Permite obtener una entidad del tipo T desde la base de datos utilizando su identificador único. Este método utiliza el DbSet para buscar el registro correspondiente al identificador proporcionado y lo devuelve. Al ser virtual, permite que las clases derivadas puedan sobrescribir este método para implementar lógica adicional o personalizada al obtener los datos, como aplicar filtros, incluir propiedades de navegación o manejar casos específicos de búsqueda. Esta implementación básica proporciona una forma sencilla de acceder a un registro específico de una entidad sin necesidad de escribir consultas específicas cada vez.
        /// </summary>
        /// <param name="id">Identificador único de la entidad</param>
        /// <returns>Entidad del tipo T con el identificador especificado</returns>
        public virtual T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        /// <summary>
        /// Permite eliminar una entidad del tipo T de la base de datos. Este método utiliza el DbSet para eliminar la entidad del contexto, lo que marca la entidad como eliminada y lista para ser removida de la base de datos cuando se guarden los cambios. Al ser virtual, permite que las clases derivadas puedan sobrescribir este método para implementar lógica adicional o personalizada al eliminar entidades, como validar los datos antes de eliminarlos o manejar casos específicos de eliminación. Esta implementación básica proporciona una forma sencilla de eliminar un registro existente en la base de datos sin necesidad de escribir consultas específicas cada vez.
        /// </summary>
        /// <param name="entity">Entidad a eliminar</param>
        /// <exception cref="ArgumentNullException">Genera excepción si la entidad es nula</exception>
        public virtual void Remove(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Permite eliminar un rango de entidades del tipo T de la base de datos. Este método utiliza el DbSet para eliminar múltiples entidades del contexto, lo que marca cada entidad como eliminada y lista para ser removida de la base de datos cuando se guarden los cambios. Al ser virtual, permite que las clases derivadas puedan sobrescribir este método para implementar lógica adicional o personalizada al eliminar múltiples entidades, como validar los datos antes de eliminarlos o manejar casos específicos de eliminación. Esta implementación básica proporciona una forma sencilla de eliminar varios registros existentes en la base de datos sin necesidad de escribir consultas específicas cada vez.
        /// </summary>
        /// <param name="entities">Entidades a eliminar</param>
        /// <exception cref="ArgumentNullException">Genera excepción si la lista de entidades es nula</exception>
        public virtual void RemoveRange(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _dbSet.RemoveRange(entities);
        }


        /// <summary>
        /// Permite obtener una única entidad del tipo T desde la base de datos que cumpla con una condición específica definida por el predicado. Este método utiliza el DbSet para aplicar el filtro definido por el predicado y devuelve la única entidad que coincide con la condición, o un valor predeterminado (null) si no se encuentra ninguna coincidencia. Al ser virtual, permite que las clases derivadas puedan sobrescribir este método para implementar lógica adicional o personalizada al obtener los datos, como aplicar filtros más complejos, ordenar los resultados o incluir relaciones con otras entidades. Esta implementación proporciona una forma sencilla de acceder a un registro específico de una entidad que cumple con ciertos criterios sin necesidad de escribir consultas específicas cada vez.
        /// </summary>
        /// <param name="predicate">Predicate que define la condición de filtrado</param>
        /// <returns>Entidad del tipo T que cumple con el predicado o null si no se encuentra</returns>
        public virtual T SingleOrDefault(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.SingleOrDefault(predicate);
        }


        /// <summary>
        /// Permite actualizar una entidad del tipo T en la base de datos. Este método utiliza el DbSet para adjuntar la entidad al contexto y marcar su estado como modificado, lo que indica que la entidad ha sido modificada y debe ser actualizada en la base de datos cuando se guarden los cambios. Al ser virtual, permite que las clases derivadas puedan sobrescribir este método para implementar lógica adicional o personalizada al actualizar entidades, como validar los datos antes de actualizarlos o manejar casos específicos de actualización. Esta implementación básica proporciona una forma sencilla de actualizar un registro existente en la base de datos sin necesidad de escribir consultas específicas cada vez.
        /// </summary>
        /// <param name="entity">Entidad con la información actualizada</param>
        /// <exception cref="ArgumentNullException">Genera excepción si la entidad es nula</exception>
        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

    }
}
