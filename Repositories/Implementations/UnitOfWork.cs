using System;
using NeorisBackend.Data;
using NeorisBackend.Repositories.Interfaces;

namespace NeorisBackend.Repositories.Implementations
{
    /// <summary>
    /// Permite gestionar las transacciones y el acceso a los repositorios de libros y autores de manera centralizada. Esta clase implementa el patrón Unit of Work, proporcionando una única instancia de DbContext para todas las operaciones de repositorio, lo que garantiza la consistencia de los datos y facilita la gestión de transacciones. Además, implementa IDisposable para asegurar que los recursos se liberen adecuadamente después de su uso.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        
        /// <summary>
        /// Referencia al repositorio de autores, que se inicializa de manera perezosa (lazy loading) para optimizar el rendimiento y evitar la creación innecesaria de instancias de repositorios hasta que realmente se necesiten. Esta propiedad proporciona acceso a las operaciones CRUD para los autores, utilizando el mismo contexto de base de datos para garantizar la coherencia de los datos en todas las operaciones realizadas a través de esta unidad de trabajo.
        /// </summary>
        private IAutorRepository _autores;
        /// <summary>
        /// Referencia al contexto de la base de datos, que se utiliza para acceder a los repositorios y gestionar las transacciones. Esta instancia se inyecta a través del constructor, lo que permite una mejor testabilidad y separación de responsabilidades.
        /// </summary>
        private readonly NeorisDbContext _context;
        /// <summary>
        /// Permite rastrear si la instancia de UnitOfWork ha sido eliminada para evitar intentos de acceso a recursos ya liberados, lo que podría causar errores o comportamientos inesperados. Esta variable se establece en true cuando se llama al método Dispose(), lo que indica que los recursos han sido liberados y que la instancia ya no debe ser utilizada.
        /// </summary>
        private bool _disposed = false;
        /// <summary>
        /// Referencias a los repositorios de libros y autores, que se inicializan de manera perezosa (lazy loading) para optimizar el rendimiento y evitar la creación innecesaria de instancias de repositorios hasta que realmente se necesiten. Estas propiedades proporcionan acceso a las operaciones CRUD para los libros y autores, utilizando el mismo contexto de base de datos para garantizar la coherencia de los datos en todas las operaciones realizadas a través de esta unidad de trabajo.
        /// </summary>
        private ILibroRepository _libros;

        /// <summary>
        /// Constructor de la clase UnitOfWork, que recibe una instancia de NeorisDbContext a través de inyección de dependencias. Si el contexto es nulo, se lanza una excepción ArgumentNullException para garantizar que la unidad de trabajo siempre tenga una instancia válida del contexto de base de datos. Esta instancia se utiliza para acceder a los repositorios y gestionar las transacciones, asegurando que todas las operaciones realizadas a través de esta unidad de trabajo sean consistentes y coherentes con la base de datos.
        /// </summary>
        /// <param name="context">Contexto de base de datos de Neoris</param>
        /// <exception cref="ArgumentNullException">Genera excepción cuando el contexto es nulo</exception>
        public UnitOfWork(NeorisDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Permite acceder al repositorio de autores, inicializándolo de manera perezosa (lazy loading) si aún no ha sido creado. Esto optimiza el rendimiento al evitar la creación innecesaria de instancias de repositorios hasta que realmente se necesiten. La propiedad proporciona acceso a las operaciones CRUD para los autores, utilizando el mismo contexto de base de datos para garantizar la coherencia de los datos en todas las operaciones realizadas a través de esta unidad de trabajo.
        /// </summary>
        public IAutorRepository Autores
        {
            get
            {
                if (_autores == null)
                {
                    _autores = new AutorRepository(_context);
                }
                return _autores;
            }
        }

        /// <summary>
        /// Permite acceder al repositorio de libros, inicializándolo de manera perezosa (lazy loading) si aún no ha sido creado. Esto optimiza el rendimiento al evitar la creación innecesaria de instancias de repositorios hasta que realmente se necesiten. La propiedad proporciona acceso a las operaciones CRUD para los libros, utilizando el mismo contexto de base de datos para garantizar la coherencia de los datos en todas las operaciones realizadas a través de esta unidad de trabajo.
        /// </summary>
        public ILibroRepository Libros
        {
            get
            {
                if (_libros == null)
                {
                    _libros = new LibroRepository(_context);
                }
                return _libros;
            }
        }

        /// <summary>
        /// Libera los recursos utilizados por la unidad de trabajo, incluyendo el contexto de base de datos. Este método se llama desde el método Dispose() para asegurar que los recursos se liberen adecuadamente cuando la unidad de trabajo ya no sea necesaria. Si el parámetro disposing es true, se liberan los recursos administrados, como el contexto de base de datos. Si el parámetro es false, solo se liberan los recursos no administrados. Después de liberar los recursos, se establece la variable _disposed en true para indicar que la instancia ha sido eliminada y evitar intentos de acceso a recursos ya liberados.
        /// </summary>
        /// <param name="disposing">Determina si se deben liberar recursos administrados</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Libera los recursos utilizados por la unidad de trabajo, incluyendo el contexto de base de datos. Este método llama a Dispose(true) para liberar los recursos administrados y luego suprime la finalización para evitar que el recolector de basura intente finalizar la instancia después de que ya se hayan liberado los recursos. Esto es importante para asegurar que los recursos gestionados por la unidad de trabajo se liberen adecuadamente cuando ya no sean necesarios, evitando posibles fugas de memoria y asegurando un manejo eficiente de los recursos en la aplicación.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Permite guardar los cambios realizados a través de los repositorios en la base de datos. Este método llama a SaveChanges() del contexto de base de datos, lo que asegura que todas las operaciones realizadas a través de esta unidad de trabajo se persistan de manera consistente en la base de datos. El método devuelve el número de registros afectados por la operación, lo que puede ser útil para verificar el éxito de las operaciones realizadas a través de los repositorios.
        /// </summary>
        /// <returns>Registros afectados</returns>
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }


    }
}
